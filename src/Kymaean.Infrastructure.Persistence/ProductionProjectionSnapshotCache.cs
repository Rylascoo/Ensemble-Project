using System.Buffers.Binary;
using System.Security.Cryptography;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

internal static class ProductionProjectionSnapshotCache
{
    private const int HashLength = 32;
    private const int PrefixLength =
        8 + sizeof(uint) + sizeof(ulong) + HashLength + sizeof(uint);
    private const int ChecksumLength = 32;
    private const string SnapshotFileName = ".projection.snapshot";
    private const string PendingPrefix = ".pending-projection-snapshot-";

    private static readonly byte[] FormatFamilyMagic = "KYMSNP01"u8.ToArray();

    public static void ReconcileBestEffort(
        string entryDirectory,
        ProductionJournalAnchor anchor,
        ProductionReplayProjection authoritativeProjection,
        bool forceWrite)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entryDirectory);
        ArgumentNullException.ThrowIfNull(anchor);
        ArgumentNullException.ThrowIfNull(authoritativeProjection);

        try
        {
            DeletePendingFiles(entryDirectory);

            if (!forceWrite &&
                TryRead(
                    entryDirectory,
                    anchor,
                    out var cachedProjection) &&
                cachedProjection == authoritativeProjection)
            {
                return;
            }

            Write(
                entryDirectory,
                anchor,
                authoritativeProjection);
        }
        catch (IOException)
        {
            // Snapshot persistence is optional acceleration only. Authoritative
            // journal validation/replay has already succeeded before this call.
        }
        catch (UnauthorizedAccessException)
        {
            // An unavailable cache must not retroactively invalidate Product state.
        }
    }

    private static bool TryRead(
        string entryDirectory,
        ProductionJournalAnchor expectedAnchor,
        out ProductionReplayProjection? projection)
    {
        projection = null;
        var path = Path.Combine(entryDirectory, SnapshotFileName);
        if (!File.Exists(path))
        {
            return false;
        }

        var bytes = File.ReadAllBytes(path);
        if (bytes.Length < PrefixLength + ChecksumLength)
        {
            return false;
        }

        var contentLength = bytes.Length - ChecksumLength;
        var content = bytes.AsSpan(0, contentLength);
        var storedChecksum = bytes.AsSpan(contentLength, ChecksumLength);
        var computedChecksum = SHA256.HashData(content);
        if (!CryptographicOperations.FixedTimeEquals(
                storedChecksum,
                computedChecksum))
        {
            return false;
        }

        if (!content[..FormatFamilyMagic.Length].SequenceEqual(
                FormatFamilyMagic))
        {
            return false;
        }

        var version = BinaryPrimitives.ReadUInt32BigEndian(
            content.Slice(FormatFamilyMagic.Length, sizeof(uint)));
        if (version !=
            ProductionPersistenceVersionPolicy.ProductionProjectionSnapshotVersion)
        {
            return false;
        }

        var sequenceOffset = FormatFamilyMagic.Length + sizeof(uint);
        var sequence = BinaryPrimitives.ReadUInt64BigEndian(
            content.Slice(sequenceOffset, sizeof(ulong)));
        if (sequence != expectedAnchor.Sequence)
        {
            return false;
        }

        var hashOffset = sequenceOffset + sizeof(ulong);
        var expectedHash = Convert.FromHexString(
            expectedAnchor.RecordHash);
        if (!CryptographicOperations.FixedTimeEquals(
                content.Slice(hashOffset, HashLength),
                expectedHash))
        {
            return false;
        }

        var nameLengthOffset = hashOffset + HashLength;
        var nameLength = BinaryPrimitives.ReadUInt32BigEndian(
            content.Slice(nameLengthOffset, sizeof(uint)));
        if (nameLength > int.MaxValue)
        {
            return false;
        }

        var expectedContentLength =
            (long)PrefixLength +
            ((long)nameLength * sizeof(ushort));
        if (content.Length != expectedContentLength)
        {
            return false;
        }

        var characters = new char[(int)nameLength];
        var offset = PrefixLength;
        for (var index = 0; index < characters.Length; index++)
        {
            characters[index] = (char)BinaryPrimitives.ReadUInt16BigEndian(
                content.Slice(offset, sizeof(ushort)));
            offset += sizeof(ushort);
        }

        projection = new ProductionReplayProjection(
            new string(characters));
        return true;
    }

    private static void Write(
        string entryDirectory,
        ProductionJournalAnchor anchor,
        ProductionReplayProjection projection)
    {
        var recordHash = Convert.FromHexString(anchor.RecordHash);
        if (recordHash.Length != HashLength)
        {
            throw new InvalidOperationException(
                "Validated journal anchor is not SHA-256 sized.");
        }

        var name = projection.ProductionName;
        var content = new byte[
            checked(PrefixLength + checked(name.Length * sizeof(ushort)))];

        FormatFamilyMagic.CopyTo(content, 0);
        BinaryPrimitives.WriteUInt32BigEndian(
            content.AsSpan(FormatFamilyMagic.Length, sizeof(uint)),
            ProductionPersistenceVersionPolicy.ProductionProjectionSnapshotVersion);

        var sequenceOffset = FormatFamilyMagic.Length + sizeof(uint);
        BinaryPrimitives.WriteUInt64BigEndian(
            content.AsSpan(sequenceOffset, sizeof(ulong)),
            anchor.Sequence);

        var hashOffset = sequenceOffset + sizeof(ulong);
        recordHash.CopyTo(content, hashOffset);

        var nameLengthOffset = hashOffset + HashLength;
        BinaryPrimitives.WriteUInt32BigEndian(
            content.AsSpan(nameLengthOffset, sizeof(uint)),
            checked((uint)name.Length));

        var offset = PrefixLength;
        foreach (var character in name)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                content.AsSpan(offset, sizeof(ushort)),
                character);
            offset += sizeof(ushort);
        }

        var checksum = SHA256.HashData(content);
        var framed = new byte[content.Length + checksum.Length];
        content.CopyTo(framed, 0);
        checksum.CopyTo(framed, content.Length);

        var pendingPath = Path.Combine(
            entryDirectory,
            $"{PendingPrefix}{Guid.NewGuid():N}");
        var finalPath = Path.Combine(
            entryDirectory,
            SnapshotFileName);

        try
        {
            using (var stream = new FileStream(
                       pendingPath,
                       new FileStreamOptions
                       {
                           Mode = FileMode.CreateNew,
                           Access = FileAccess.Write,
                           Share = FileShare.None,
                           BufferSize = 4096,
                           Options =
                               FileOptions.SequentialScan |
                               FileOptions.WriteThrough,
                       }))
            {
                stream.Write(framed);
                stream.Flush(flushToDisk: true);
            }

            File.Move(
                pendingPath,
                finalPath,
                overwrite: true);
        }
        finally
        {
            TryDeletePending(pendingPath);
        }
    }

    private static void DeletePendingFiles(string entryDirectory)
    {
        foreach (var pendingPath in Directory.EnumerateFiles(
                     entryDirectory,
                     $"{PendingPrefix}*",
                     SearchOption.TopDirectoryOnly))
        {
            File.Delete(pendingPath);
        }
    }

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
}
