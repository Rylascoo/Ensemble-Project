using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class FileProductionJournalTests
{
    private const string HeadFileName = ".journal.head";
    private const int SchemaVersionOffset = 8;
    private const int HashLength = 32;
    private const int EntryPrefixLength =
        8 + sizeof(uint) + sizeof(ulong) + HashLength + sizeof(ulong) + HashLength;
    private const int EntryHeaderLength = EntryPrefixLength + HashLength;
    private const int HeadPrefixLength = 8 + sizeof(uint) + sizeof(ulong) + HashLength;

    [TestMethod]
    public void WriterPersistsJournalSchemaVersionOne()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));

        var entryBytes = File.ReadAllBytes(OrderedEntryPaths(directory.Path).Single());
        var headBytes = File.ReadAllBytes(Path.Combine(directory.Path, HeadFileName));

        Assert.AreEqual(
            1U,
            BinaryPrimitives.ReadUInt32BigEndian(
                entryBytes.AsSpan(SchemaVersionOffset, sizeof(uint))));
        Assert.AreEqual(
            1U,
            BinaryPrimitives.ReadUInt32BigEndian(
                headBytes.AsSpan(SchemaVersionOffset, sizeof(uint))));
    }

    [TestMethod]
    public void UnsupportedJournalEntrySchemaVersionFailsAsCompatibilityError()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));

        var entryPath = OrderedEntryPaths(directory.Path).Single();
        RewriteEntrySchemaVersionPreservingIntegrity(entryPath, 2);

        var exception = Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
            () => new FileProductionJournal(directory.Path).ReadAll());

        Assert.AreEqual("journal entry schema", exception.Artifact);
        Assert.AreEqual("2", exception.FoundIdentifier);
        Assert.AreEqual("1", exception.SupportedIdentifier);
    }

    [TestMethod]
    public void UnsupportedJournalHeadSchemaVersionFailsAsCompatibilityError()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));

        var headPath = Path.Combine(directory.Path, HeadFileName);
        RewriteHeadSchemaVersionPreservingIntegrity(headPath, 2);

        var exception = Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
            () => new FileProductionJournal(directory.Path).ReadAll());

        Assert.AreEqual("journal head schema", exception.Artifact);
        Assert.AreEqual("2", exception.FoundIdentifier);
        Assert.AreEqual("1", exception.SupportedIdentifier);
    }

    [TestMethod]
    public void RecoveryDoesNotMigrateUnsupportedJournalVersion()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("first"));
        var committedHead = File.ReadAllBytes(Path.Combine(directory.Path, HeadFileName));
        journal.Append(Encoding.UTF8.GetBytes("second"));
        File.WriteAllBytes(Path.Combine(directory.Path, HeadFileName), committedHead);

        var secondEntryPath = OrderedEntryPaths(directory.Path)[1];
        RewriteEntrySchemaVersionPreservingIntegrity(secondEntryPath, 2);

        Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
            () => new FileProductionJournal(directory.Path).Recover());
        CollectionAssert.AreEqual(
            committedHead,
            File.ReadAllBytes(Path.Combine(directory.Path, HeadFileName)));
    }

    [TestMethod]
    public void CorruptedJournalEntryVersionByteRemainsCorruption()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));

        var entryPath = OrderedEntryPaths(directory.Path).Single();
        var bytes = File.ReadAllBytes(entryPath);
        BinaryPrimitives.WriteUInt32BigEndian(
            bytes.AsSpan(SchemaVersionOffset, sizeof(uint)),
            2);
        File.WriteAllBytes(entryPath, bytes);

        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void CorruptedJournalHeadVersionByteRemainsCorruption()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));

        var headPath = Path.Combine(directory.Path, HeadFileName);
        var bytes = File.ReadAllBytes(headPath);
        BinaryPrimitives.WriteUInt32BigEndian(
            bytes.AsSpan(SchemaVersionOffset, sizeof(uint)),
            2);
        File.WriteAllBytes(headPath, bytes);

        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void AppendAndReopenPreservesOrderedPayloadsAndHashChain()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);

        var first = journal.Append(Encoding.UTF8.GetBytes("first"));
        var second = journal.Append(Encoding.UTF8.GetBytes("second"));

        var reopened = new FileProductionJournal(directory.Path).ReadAll();

        Assert.HasCount(2, reopened);
        Assert.AreEqual(1UL, first.Sequence);
        Assert.AreEqual(2UL, second.Sequence);
        Assert.AreEqual(new string('0', 64), first.PreviousRecordHash);
        Assert.AreEqual(first.RecordHash, second.PreviousRecordHash);
        CollectionAssert.AreEqual(
            Encoding.UTF8.GetBytes("first"),
            reopened[0].Payload.ToArray());
        CollectionAssert.AreEqual(
            Encoding.UTF8.GetBytes("second"),
            reopened[1].Payload.ToArray());
    }

    [TestMethod]
    public void CorruptedCommittedPayloadFailsClosed()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("authoritative payload"));

        var entryPath = Directory.GetFiles(directory.Path, "*.kjr").Single();
        var bytes = File.ReadAllBytes(entryPath);
        bytes[^1] ^= 0xff;
        File.WriteAllBytes(entryPath, bytes);

        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void MissingCommittedSequenceFailsClosed()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("one"));
        journal.Append(Encoding.UTF8.GetBytes("two"));

        var entryPaths = OrderedEntryPaths(directory.Path);
        File.Delete(entryPaths[0]);

        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void MissingFinalCommittedEntryFailsClosedEvenDuringRecovery()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("one"));
        journal.Append(Encoding.UTF8.GetBytes("two"));

        File.Delete(OrderedEntryPaths(directory.Path)[1]);
        var reopened = new FileProductionJournal(directory.Path);

        Assert.Throws<ProductionJournalCorruptionException>(() => reopened.ReadAll());
        Assert.Throws<ProductionJournalCorruptionException>(() => reopened.Recover());
    }

    [TestMethod]
    public void CorruptedHeadChecksumFailsClosed()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));

        var headPath = Path.Combine(directory.Path, HeadFileName);
        var bytes = File.ReadAllBytes(headPath);
        bytes[^1] ^= 0xff;
        File.WriteAllBytes(headPath, bytes);

        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void TruncatedCommittedEntryFailsClosedEvenDuringRecovery()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));
        var headPath = Path.Combine(directory.Path, HeadFileName);
        var originalHead = File.ReadAllBytes(headPath);

        var entryPath = OrderedEntryPaths(directory.Path).Single();
        var entryBytes = File.ReadAllBytes(entryPath);
        File.WriteAllBytes(
            entryPath,
            entryBytes.AsSpan(0, entryBytes.Length / 2).ToArray());

        var reopened = new FileProductionJournal(directory.Path);
        Assert.Throws<ProductionJournalCorruptionException>(
            () => reopened.ReadAll());
        Assert.Throws<ProductionJournalCorruptionException>(
            () => reopened.Recover());
        CollectionAssert.AreEqual(
            originalHead,
            File.ReadAllBytes(headPath));
    }

    [TestMethod]
    public void TruncatedCommittedHeadFailsClosedEvenDuringRecovery()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));
        var headPath = Path.Combine(directory.Path, HeadFileName);
        var headBytes = File.ReadAllBytes(headPath);
        var truncated = headBytes.AsSpan(
            0,
            headBytes.Length / 2)
            .ToArray();
        File.WriteAllBytes(
            headPath,
            truncated);

        var reopened = new FileProductionJournal(directory.Path);
        Assert.Throws<ProductionJournalCorruptionException>(
            () => reopened.ReadAll());
        Assert.Throws<ProductionJournalCorruptionException>(
            () => reopened.Recover());
        CollectionAssert.AreEqual(
            truncated,
            File.ReadAllBytes(headPath));
    }

    [TestMethod]
    public void ImpossibleChecksummedHeadSequenceFailsClosedEvenDuringRecovery()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));
        var headPath = Path.Combine(directory.Path, HeadFileName);
        RewriteHeadSequencePreservingIntegrity(
            headPath,
            2UL);
        var impossibleHead = File.ReadAllBytes(headPath);

        var reopened = new FileProductionJournal(directory.Path);
        Assert.Throws<ProductionJournalCorruptionException>(
            () => reopened.ReadAll());
        Assert.Throws<ProductionJournalCorruptionException>(
            () => reopened.Recover());
        CollectionAssert.AreEqual(
            impossibleHead,
            File.ReadAllBytes(headPath));
    }

    [TestMethod]
    public void RecoveryDoesNotPartiallyPromotePastCorruptCrashSuffix()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("first"));
        var headPath = Path.Combine(directory.Path, HeadFileName);
        var firstHead = File.ReadAllBytes(headPath);

        journal.Append(Encoding.UTF8.GetBytes("valid crash suffix"));
        journal.Append(Encoding.UTF8.GetBytes("corrupt crash suffix"));
        File.WriteAllBytes(
            headPath,
            firstHead);

        var thirdEntryPath = OrderedEntryPaths(directory.Path)[2];
        var thirdEntry = File.ReadAllBytes(thirdEntryPath);
        thirdEntry[^1] ^= 0xff;
        File.WriteAllBytes(
            thirdEntryPath,
            thirdEntry);

        var reopened = new FileProductionJournal(directory.Path);
        Assert.Throws<ProductionJournalCorruptionException>(
            () => reopened.Recover());
        CollectionAssert.AreEqual(
            firstHead,
            File.ReadAllBytes(headPath));
    }

    [TestMethod]
    public void ReadAllIgnoresPendingArtifactsWithoutInventingHistory()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));
        var pendingEntry = Path.Combine(
            directory.Path,
            ".pending-entry-uncommitted");
        var pendingHead = Path.Combine(
            directory.Path,
            ".pending-head-uncommitted");
        File.WriteAllBytes(
            pendingEntry,
            Enumerable.Repeat((byte)0xa5, 37).ToArray());
        File.WriteAllBytes(
            pendingHead,
            Enumerable.Repeat((byte)0x5a, 19).ToArray());

        var reopened = new FileProductionJournal(directory.Path)
            .ReadAll();

        Assert.HasCount(1, reopened);
        CollectionAssert.AreEqual(
            Encoding.UTF8.GetBytes("committed"),
            reopened[0].Payload.ToArray());
        Assert.IsTrue(File.Exists(pendingEntry));
        Assert.IsTrue(File.Exists(pendingHead));
    }

    [TestMethod]
    public void RecoverPromotesValidPostCrashSuffixToCommittedHead()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("first"));
        var firstHead = File.ReadAllBytes(Path.Combine(directory.Path, HeadFileName));
        journal.Append(Encoding.UTF8.GetBytes("second"));

        File.WriteAllBytes(Path.Combine(directory.Path, HeadFileName), firstHead);
        var reopened = new FileProductionJournal(directory.Path);
        Assert.Throws<ProductionJournalCorruptionException>(() => reopened.ReadAll());

        var recovered = reopened.Recover();

        Assert.HasCount(2, recovered);
        Assert.HasCount(2, reopened.ReadAll());
        CollectionAssert.AreEqual(
            Encoding.UTF8.GetBytes("second"),
            recovered[1].Payload.ToArray());
    }

    [TestMethod]
    public void RecoverAdoptsValidFirstEntryWhenCrashPrecededInitialHeadPublish()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("first"));
        File.Delete(Path.Combine(directory.Path, HeadFileName));

        var reopened = new FileProductionJournal(directory.Path);
        Assert.Throws<ProductionJournalCorruptionException>(() => reopened.ReadAll());

        var recovered = reopened.Recover();

        Assert.HasCount(1, recovered);
        Assert.IsTrue(File.Exists(Path.Combine(directory.Path, HeadFileName)));
        Assert.HasCount(1, reopened.ReadAll());
    }

    [TestMethod]
    public void RecoverRemovesStalePendingFilesWithoutInventingHistory()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));
        var pendingEntry = Path.Combine(directory.Path, ".pending-entry-simulated-crash");
        var pendingHead = Path.Combine(directory.Path, ".pending-head-simulated-crash");
        File.WriteAllText(pendingEntry, "partial entry");
        File.WriteAllText(pendingHead, "partial head");

        var recovered = new FileProductionJournal(directory.Path).Recover();

        Assert.HasCount(1, recovered);
        Assert.IsFalse(File.Exists(pendingEntry));
        Assert.IsFalse(File.Exists(pendingHead));
        CollectionAssert.AreEqual(
            Encoding.UTF8.GetBytes("committed"),
            recovered[0].Payload.ToArray());
    }

    [TestMethod]
    public void RenamedCommittedRecordFailsClosed()
    {
        using var directory = new TempDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("committed"));

        var entryPath = Directory.GetFiles(directory.Path, "*.kjr").Single();
        var renamedPath = Path.Combine(
            directory.Path,
            $"{1UL:D20}-{new string('f', 64)}.kjr");
        File.Move(entryPath, renamedPath);

        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    private static string RewriteEntrySchemaVersionPreservingIntegrity(
        string entryPath,
        uint schemaVersion)
    {
        var bytes = File.ReadAllBytes(entryPath);
        BinaryPrimitives.WriteUInt32BigEndian(
            bytes.AsSpan(SchemaVersionOffset, sizeof(uint)),
            schemaVersion);

        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        hash.AppendData(bytes.AsSpan(0, EntryPrefixLength));
        hash.AppendData(bytes.AsSpan(EntryHeaderLength));
        var recordHash = hash.GetHashAndReset();
        recordHash.CopyTo(bytes.AsSpan(EntryPrefixLength, HashLength));
        File.WriteAllBytes(entryPath, bytes);

        var sequence = BinaryPrimitives.ReadUInt64BigEndian(
            bytes.AsSpan(SchemaVersionOffset + sizeof(uint), sizeof(ulong)));
        var newPath = Path.Combine(
            Path.GetDirectoryName(entryPath)!,
            $"{sequence:D20}-{Convert.ToHexString(recordHash).ToLowerInvariant()}.kjr");
        File.Move(entryPath, newPath);
        return newPath;
    }

    private static void RewriteHeadSchemaVersionPreservingIntegrity(
        string headPath,
        uint schemaVersion)
    {
        var bytes = File.ReadAllBytes(headPath);
        BinaryPrimitives.WriteUInt32BigEndian(
            bytes.AsSpan(SchemaVersionOffset, sizeof(uint)),
            schemaVersion);
        SHA256.HashData(bytes.AsSpan(0, HeadPrefixLength))
            .CopyTo(bytes.AsSpan(HeadPrefixLength, HashLength));
        File.WriteAllBytes(headPath, bytes);
    }

    private static void RewriteHeadSequencePreservingIntegrity(
        string headPath,
        ulong sequence)
    {
        var bytes = File.ReadAllBytes(headPath);
        BinaryPrimitives.WriteUInt64BigEndian(
            bytes.AsSpan(
                SchemaVersionOffset + sizeof(uint),
                sizeof(ulong)),
            sequence);
        SHA256.HashData(bytes.AsSpan(0, HeadPrefixLength))
            .CopyTo(bytes.AsSpan(HeadPrefixLength, HashLength));
        File.WriteAllBytes(
            headPath,
            bytes);
    }

    private static string[] OrderedEntryPaths(string path) =>
        Directory.GetFiles(path, "*.kjr")
            .OrderBy(Path.GetFileName, StringComparer.Ordinal)
            .ToArray();

    private sealed class TempDirectory : IDisposable
    {
        public TempDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Kymaean.Persistence.Tests",
                Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(Path))
                {
                    Directory.Delete(Path, recursive: true);
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
}
