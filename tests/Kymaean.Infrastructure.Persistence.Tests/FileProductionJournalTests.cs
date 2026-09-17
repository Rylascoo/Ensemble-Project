using System.Text;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class FileProductionJournalTests
{
    private const string HeadFileName = ".journal.head";

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
