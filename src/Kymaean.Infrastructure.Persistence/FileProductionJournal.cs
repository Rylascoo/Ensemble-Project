using System.Buffers.Binary;
using System.Security.Cryptography;

namespace Kymaean.Infrastructure.Persistence;

public sealed class FileProductionJournal
{
    private const int HashLength = 32;
    private const int EntryPrefixLength = 8 + sizeof(uint) + sizeof(ulong) + HashLength + sizeof(ulong) + HashLength;
    private const int EntryHeaderLength = EntryPrefixLength + HashLength;
    private const int HeadPrefixLength = 8 + sizeof(uint) + sizeof(ulong) + HashLength;
    private const int HeadLength = HeadPrefixLength + HashLength;
    private const string EntryExtension = ".kjr";
    private const string EntryPendingPrefix = ".pending-entry-";
    private const string HeadFileName = ".journal.head";
    private const string HeadPendingPrefix = ".pending-head-";
    private const string LockFileName = ".journal.lock";

    // File-family magic stays stable across schema revisions; the uint version field is authoritative.
    private static readonly byte[] EntryFormatFamilyMagic = "KYMJRN01"u8.ToArray();
    private static readonly byte[] HeadFormatFamilyMagic = "KYMJHD01"u8.ToArray();
    private static readonly byte[] GenesisHash = new byte[HashLength];

    private readonly string _rootDirectory;

    public FileProductionJournal(string rootDirectory)
        : this(rootDirectory, createDirectory: true)
    {
    }

    private FileProductionJournal(string rootDirectory, bool createDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        _rootDirectory = Path.GetFullPath(rootDirectory);

        if (createDirectory)
        {
            Directory.CreateDirectory(_rootDirectory);
            return;
        }

        try
        {
            var attributes = File.GetAttributes(_rootDirectory);
            if ((attributes & FileAttributes.Directory) == 0)
            {
                throw new IOException(
                    "Production journal path is not a directory.");
            }
        }
        catch (FileNotFoundException exception)
        {
            throw new DirectoryNotFoundException(
                "Production journal directory does not exist.",
                exception);
        }
    }

    internal static FileProductionJournal OpenExisting(string rootDirectory) =>
        new(rootDirectory, createDirectory: false);

    public ProductionJournalEntry Append(ReadOnlySpan<byte> payload) =>
        AppendCore(payload, validateCandidate: null);

    internal ProductionJournalEntry AppendValidated(
        ReadOnlySpan<byte> payload,
        Action<IReadOnlyList<ProductionJournalEntry>> validateCandidate)
    {
        ArgumentNullException.ThrowIfNull(validateCandidate);
        return AppendCore(payload, validateCandidate);
    }

    private ProductionJournalEntry AppendCore(
        ReadOnlySpan<byte> payload,
        Action<IReadOnlyList<ProductionJournalEntry>>? validateCandidate)
    {
        using var gate = AcquireGate();
        var entries = ReadValidatedEntriesUnsafe();
        ValidateCommittedHeadUnsafe(entries);

        var sequence = entries.Count == 0
            ? 1UL
            : checked(entries[^1].Sequence + 1UL);
        var previousHash = entries.Count == 0
            ? GenesisHash
            : Convert.FromHexString(entries[^1].RecordHash);
        var payloadBytes = payload.ToArray();
        var prefix = BuildEntryPrefix(sequence, previousHash, payloadBytes);
        var recordHash = ComputeRecordHash(prefix, payloadBytes);
        var header = new byte[EntryHeaderLength];
        prefix.CopyTo(header, 0);
        recordHash.CopyTo(header, EntryPrefixLength);

        var candidate = new ProductionJournalEntry(
            sequence,
            Hex(previousHash),
            Hex(recordHash),
            payloadBytes);
        if (validateCandidate is not null)
        {
            var candidateEntries = new List<ProductionJournalEntry>(entries.Count + 1);
            candidateEntries.AddRange(entries);
            candidateEntries.Add(candidate);
            validateCandidate(candidateEntries.AsReadOnly());
        }

        var finalPath = Path.Combine(_rootDirectory, EntryFileName(sequence, recordHash));
        if (File.Exists(finalPath))
        {
            throw new ProductionJournalCorruptionException(
                $"Journal entry already exists at sequence {sequence}.");
        }

        var pendingPath = Path.Combine(_rootDirectory, $"{EntryPendingPrefix}{Guid.NewGuid():N}");
        try
        {
            WriteDurableFile(pendingPath, header, payloadBytes);
            File.Move(pendingPath, finalPath, overwrite: false);
            WriteHeadUnsafe(sequence, recordHash);
        }
        finally
        {
            TryDeletePending(pendingPath);
        }

        return candidate;
    }

    public IReadOnlyList<ProductionJournalEntry> ReadAll()
    {
        using var gate = AcquireGate();
        var entries = ReadValidatedEntriesUnsafe();
        ValidateCommittedHeadUnsafe(entries);
        return entries.AsReadOnly();
    }

    public IReadOnlyList<ProductionJournalEntry> Recover() =>
        RecoverCore(validateCandidate: null);

    internal IReadOnlyList<ProductionJournalEntry> RecoverValidated(
        Action<IReadOnlyList<ProductionJournalEntry>> validateCandidate)
    {
        ArgumentNullException.ThrowIfNull(validateCandidate);
        return RecoverCore(validateCandidate);
    }

    private IReadOnlyList<ProductionJournalEntry> RecoverCore(
        Action<IReadOnlyList<ProductionJournalEntry>>? validateCandidate)
    {
        using var gate = AcquireGate();
        DeletePendingFilesUnsafe();

        var entries = ReadValidatedEntriesUnsafe();
        var head = ReadHeadUnsafe();
        var candidate = entries.AsReadOnly();

        if (entries.Count == 0)
        {
            if (head is not null)
            {
                throw new ProductionJournalCorruptionException(
                    "Journal head claims committed history but no journal entries exist.");
            }

            validateCandidate?.Invoke(candidate);
            return candidate;
        }

        if (head is not null)
        {
            if (head.Sequence > checked((ulong)entries.Count))
            {
                throw new ProductionJournalCorruptionException(
                    "Journal head is ahead of the available committed entry chain.");
            }

            var anchoredEntry = entries[checked((int)head.Sequence - 1)];
            if (!StringComparer.Ordinal.Equals(head.RecordHash, anchoredEntry.RecordHash))
            {
                throw new ProductionJournalCorruptionException(
                    "Journal head does not match its committed entry.");
            }
        }

        validateCandidate?.Invoke(candidate);

        var last = entries[^1];
        if (head is null ||
            head.Sequence != last.Sequence ||
            !StringComparer.Ordinal.Equals(head.RecordHash, last.RecordHash))
        {
            WriteHeadUnsafe(last.Sequence, Convert.FromHexString(last.RecordHash));
        }

        ValidateCommittedHeadUnsafe(entries);
        return candidate;
    }

    private FileStream AcquireGate() =>
        new(
            Path.Combine(_rootDirectory, LockFileName),
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.None);

    private List<ProductionJournalEntry> ReadValidatedEntriesUnsafe()
    {
        var entryPaths = Directory
            .EnumerateFiles(_rootDirectory, $"*{EntryExtension}", SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName, StringComparer.Ordinal)
            .ToArray();
        var entries = new List<ProductionJournalEntry>(entryPaths.Length);
        var expectedSequence = 1UL;
        var expectedPreviousHash = GenesisHash;

        foreach (var entryPath in entryPaths)
        {
            var bytes = File.ReadAllBytes(entryPath);
            if (bytes.Length < EntryHeaderLength)
            {
                throw Corrupt(entryPath, "Entry is shorter than the journal header.");
            }

            var span = bytes.AsSpan();
            if (!span[..EntryFormatFamilyMagic.Length].SequenceEqual(EntryFormatFamilyMagic))
            {
                throw Corrupt(entryPath, "Magic does not match.");
            }

            var version = BinaryPrimitives.ReadUInt32BigEndian(
                span.Slice(EntryFormatFamilyMagic.Length, sizeof(uint)));

            var sequenceOffset = EntryFormatFamilyMagic.Length + sizeof(uint);
            var sequence = BinaryPrimitives.ReadUInt64BigEndian(
                span.Slice(sequenceOffset, sizeof(ulong)));
            if (sequence != expectedSequence)
            {
                throw Corrupt(entryPath, $"Expected sequence {expectedSequence}, found {sequence}.");
            }

            var previousHashOffset = sequenceOffset + sizeof(ulong);
            var previousHash = span.Slice(previousHashOffset, HashLength).ToArray();
            if (!CryptographicOperations.FixedTimeEquals(previousHash, expectedPreviousHash))
            {
                throw Corrupt(entryPath, "Previous-record hash chain does not match.");
            }

            var payloadLengthOffset = previousHashOffset + HashLength;
            var payloadLength = BinaryPrimitives.ReadUInt64BigEndian(
                span.Slice(payloadLengthOffset, sizeof(ulong)));
            var actualPayloadLength = checked((ulong)(bytes.Length - EntryHeaderLength));
            if (payloadLength != actualPayloadLength)
            {
                throw Corrupt(entryPath, "Payload length does not match the entry body.");
            }

            var payloadHashOffset = payloadLengthOffset + sizeof(ulong);
            var payloadHash = span.Slice(payloadHashOffset, HashLength).ToArray();
            var recordHash = span.Slice(EntryPrefixLength, HashLength).ToArray();
            var payload = span[EntryHeaderLength..].ToArray();
            var computedPayloadHash = SHA256.HashData(payload);
            if (!CryptographicOperations.FixedTimeEquals(payloadHash, computedPayloadHash))
            {
                throw Corrupt(entryPath, "Payload hash does not match.");
            }

            var computedRecordHash = ComputeRecordHash(span[..EntryPrefixLength], payload);
            if (!CryptographicOperations.FixedTimeEquals(recordHash, computedRecordHash))
            {
                throw Corrupt(entryPath, "Record hash does not match.");
            }

            var expectedFileName = EntryFileName(sequence, recordHash);
            if (!StringComparer.Ordinal.Equals(Path.GetFileName(entryPath), expectedFileName))
            {
                throw Corrupt(entryPath, "Entry filename does not match its sequence and record hash.");
            }

            ProductionPersistenceVersionPolicy.RequireJournalSchema(
                version,
                "journal entry schema");

            entries.Add(new ProductionJournalEntry(
                sequence,
                Hex(previousHash),
                Hex(recordHash),
                payload));

            expectedPreviousHash = recordHash;
            expectedSequence = checked(expectedSequence + 1UL);
        }

        return entries;
    }

    private void ValidateCommittedHeadUnsafe(IReadOnlyList<ProductionJournalEntry> entries)
    {
        var head = ReadHeadUnsafe();
        if (entries.Count == 0)
        {
            if (head is not null)
            {
                throw new ProductionJournalCorruptionException(
                    "Journal head exists without committed entries.");
            }

            return;
        }

        if (head is null)
        {
            throw new ProductionJournalCorruptionException(
                "Committed journal entries exist without a journal head.");
        }

        var last = entries[^1];
        if (head.Sequence != last.Sequence ||
            !StringComparer.Ordinal.Equals(head.RecordHash, last.RecordHash))
        {
            throw new ProductionJournalCorruptionException(
                "Journal head does not match the final committed entry.");
        }
    }

    private JournalHead? ReadHeadUnsafe()
    {
        var path = Path.Combine(_rootDirectory, HeadFileName);
        if (!File.Exists(path))
        {
            return null;
        }

        var bytes = File.ReadAllBytes(path);
        if (bytes.Length != HeadLength)
        {
            throw Corrupt(path, "Head length is invalid.");
        }

        var span = bytes.AsSpan();
        if (!span[..HeadFormatFamilyMagic.Length].SequenceEqual(HeadFormatFamilyMagic))
        {
            throw Corrupt(path, "Head magic does not match.");
        }

        var version = BinaryPrimitives.ReadUInt32BigEndian(
            span.Slice(HeadFormatFamilyMagic.Length, sizeof(uint)));

        var sequenceOffset = HeadFormatFamilyMagic.Length + sizeof(uint);
        var sequence = BinaryPrimitives.ReadUInt64BigEndian(
            span.Slice(sequenceOffset, sizeof(ulong)));
        if (sequence == 0)
        {
            throw Corrupt(path, "Head sequence must be positive.");
        }

        var recordHashOffset = sequenceOffset + sizeof(ulong);
        var recordHash = span.Slice(recordHashOffset, HashLength).ToArray();
        var storedChecksum = span.Slice(HeadPrefixLength, HashLength);
        var computedChecksum = SHA256.HashData(span[..HeadPrefixLength]);
        if (!CryptographicOperations.FixedTimeEquals(storedChecksum, computedChecksum))
        {
            throw Corrupt(path, "Head checksum does not match.");
        }

        ProductionPersistenceVersionPolicy.RequireJournalSchema(
            version,
            "journal head schema");

        return new JournalHead(sequence, Hex(recordHash));
    }

    private void WriteHeadUnsafe(ulong sequence, ReadOnlySpan<byte> recordHash)
    {
        if (sequence == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence));
        }

        if (recordHash.Length != HashLength)
        {
            throw new ArgumentException("Record hash must be SHA-256 sized.", nameof(recordHash));
        }

        var head = new byte[HeadLength];
        HeadFormatFamilyMagic.CopyTo(head, 0);
        BinaryPrimitives.WriteUInt32BigEndian(
            head.AsSpan(HeadFormatFamilyMagic.Length, sizeof(uint)),
            ProductionPersistenceVersionPolicy.JournalSchemaVersion);
        var sequenceOffset = HeadFormatFamilyMagic.Length + sizeof(uint);
        BinaryPrimitives.WriteUInt64BigEndian(
            head.AsSpan(sequenceOffset, sizeof(ulong)),
            sequence);
        var recordHashOffset = sequenceOffset + sizeof(ulong);
        recordHash.CopyTo(head.AsSpan(recordHashOffset, HashLength));
        SHA256.HashData(head.AsSpan(0, HeadPrefixLength))
            .CopyTo(head.AsSpan(HeadPrefixLength, HashLength));

        var pendingPath = Path.Combine(_rootDirectory, $"{HeadPendingPrefix}{Guid.NewGuid():N}");
        var finalPath = Path.Combine(_rootDirectory, HeadFileName);
        try
        {
            WriteDurableFile(pendingPath, head, ReadOnlySpan<byte>.Empty);
            File.Move(pendingPath, finalPath, overwrite: true);
        }
        finally
        {
            TryDeletePending(pendingPath);
        }
    }

    private void DeletePendingFilesUnsafe()
    {
        foreach (var pattern in new[] { $"{EntryPendingPrefix}*", $"{HeadPendingPrefix}*" })
        {
            foreach (var pendingPath in Directory.EnumerateFiles(
                         _rootDirectory,
                         pattern,
                         SearchOption.TopDirectoryOnly))
            {
                File.Delete(pendingPath);
            }
        }
    }

    private static byte[] BuildEntryPrefix(
        ulong sequence,
        ReadOnlySpan<byte> previousHash,
        ReadOnlySpan<byte> payload)
    {
        if (previousHash.Length != HashLength)
        {
            throw new ArgumentException("Previous hash must be SHA-256 sized.", nameof(previousHash));
        }

        var prefix = new byte[EntryPrefixLength];
        EntryFormatFamilyMagic.CopyTo(prefix, 0);
        BinaryPrimitives.WriteUInt32BigEndian(
            prefix.AsSpan(EntryFormatFamilyMagic.Length, sizeof(uint)),
            ProductionPersistenceVersionPolicy.JournalSchemaVersion);

        var sequenceOffset = EntryFormatFamilyMagic.Length + sizeof(uint);
        BinaryPrimitives.WriteUInt64BigEndian(
            prefix.AsSpan(sequenceOffset, sizeof(ulong)),
            sequence);

        var previousHashOffset = sequenceOffset + sizeof(ulong);
        previousHash.CopyTo(prefix.AsSpan(previousHashOffset, HashLength));

        var payloadLengthOffset = previousHashOffset + HashLength;
        BinaryPrimitives.WriteUInt64BigEndian(
            prefix.AsSpan(payloadLengthOffset, sizeof(ulong)),
            checked((ulong)payload.Length));

        var payloadHashOffset = payloadLengthOffset + sizeof(ulong);
        SHA256.HashData(payload).CopyTo(prefix.AsSpan(payloadHashOffset, HashLength));
        return prefix;
    }

    private static byte[] ComputeRecordHash(
        ReadOnlySpan<byte> prefix,
        ReadOnlySpan<byte> payload)
    {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        hash.AppendData(prefix);
        hash.AppendData(payload);
        return hash.GetHashAndReset();
    }

    private static void WriteDurableFile(
        string path,
        ReadOnlySpan<byte> first,
        ReadOnlySpan<byte> second)
    {
        using var stream = new FileStream(
            path,
            new FileStreamOptions
            {
                Mode = FileMode.CreateNew,
                Access = FileAccess.Write,
                Share = FileShare.None,
                BufferSize = 4096,
                Options = FileOptions.SequentialScan | FileOptions.WriteThrough,
            });
        stream.Write(first);
        stream.Write(second);
        stream.Flush(flushToDisk: true);
    }

    private static string EntryFileName(ulong sequence, ReadOnlySpan<byte> recordHash) =>
        $"{sequence:D20}-{Hex(recordHash)}{EntryExtension}";

    private static string Hex(ReadOnlySpan<byte> bytes) =>
        Convert.ToHexString(bytes).ToLowerInvariant();

    private static ProductionJournalCorruptionException Corrupt(string path, string detail) =>
        new($"Production journal corruption in '{Path.GetFileName(path)}': {detail}");

    private static void TryDeletePending(string pendingPath)
    {
        try
        {
            if (File.Exists(pendingPath))
            {
                File.Delete(pendingPath);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private sealed record JournalHead(ulong Sequence, string RecordHash);
}

public sealed class ProductionJournalEntry
{
    private readonly byte[] _payload;

    internal ProductionJournalEntry(
        ulong sequence,
        string previousRecordHash,
        string recordHash,
        byte[] payload)
    {
        Sequence = sequence;
        PreviousRecordHash = previousRecordHash;
        RecordHash = recordHash;
        _payload = payload.ToArray();
    }

    public ulong Sequence { get; }

    public string PreviousRecordHash { get; }

    public string RecordHash { get; }

    public ReadOnlyMemory<byte> Payload => _payload;
}

public sealed class ProductionJournalCorruptionException : IOException
{
    internal ProductionJournalCorruptionException(string message)
        : base(message)
    {
    }
}
