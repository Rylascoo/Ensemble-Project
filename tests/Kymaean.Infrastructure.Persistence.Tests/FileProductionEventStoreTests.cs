using System.Text;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class FileProductionEventStoreTests
{
    [TestMethod]
    public void ReopenLoadsTypedCreationEventAndDeterministicallyRebuilds()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        store.Append(new ProductionCreatedEvent("The Glass Harbor"));

        var reopened = new FileProductionEventStore(directory.Path);
        var history = reopened.LoadAll();
        var created = Assert.IsInstanceOfType<ProductionCreatedEvent>(history.Single());
        var state = ProductionReplay.Rebuild(history);

        Assert.AreEqual("The Glass Harbor", created.ProductionName);
        Assert.AreEqual("The Glass Harbor", state.ProductionName);
    }

    [TestMethod]
    public void CreationEventUsesDeterministicVersionedPayload()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        store.Append(new ProductionCreatedEvent("Glass Harbor"));

        var journal = new FileProductionJournal(directory.Path);
        var payload = Encoding.UTF8.GetString(journal.ReadAll().Single().Payload.Span);

        Assert.AreEqual(
            "{\"contract\":\"kymaean.production.created.v2\",\"productionNameUtf16Be\":\"AEcAbABhAHMAcwAgAEgAYQByAGIAbwBy\"}",
            payload);
    }

    [TestMethod]
    public void FutureProductionCreatedContractFailsAsCompatibilityError()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes(
            "{\"contract\":\"kymaean.production.created.v3\",\"productionName\":\"Harbor\"}"));

        var store = new FileProductionEventStore(directory.Path);
        var exception = Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
            () => store.LoadAll());

        Assert.AreEqual("ProductionCreated event contract", exception.Artifact);
        Assert.AreEqual("kymaean.production.created.v3", exception.FoundIdentifier);
        Assert.AreEqual("kymaean.production.created.v1, kymaean.production.created.v2", exception.SupportedIdentifier);
    }

    [TestMethod]
    public void UnknownEventContractFailsClosed()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes(
            "{\"contract\":\"kymaean.production.unknown.v1\"}"));

        var store = new FileProductionEventStore(directory.Path);

        Assert.ThrowsExactly<InvalidDataException>(() => store.LoadAll());
    }

    [TestMethod]
    public void ExtraV1PropertyFailsClosed()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes(
            "{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"Harbor\",\"extra\":true}"));

        var store = new FileProductionEventStore(directory.Path);

        Assert.ThrowsExactly<InvalidDataException>(() => store.LoadAll());
    }

    [TestMethod]
    public void MalformedEventPayloadFailsClosed()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(Encoding.UTF8.GetBytes("{not-json"));

        var store = new FileProductionEventStore(directory.Path);

        Assert.ThrowsExactly<InvalidDataException>(() => store.LoadAll());
    }

    [TestMethod]
    public void EmptyStoreRemainsAValidStorageState()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);

        Assert.HasCount(0, store.LoadAll());
        Assert.HasCount(0, store.Recover());
    }

    [TestMethod]
    public void AppendRejectsReplayInvalidHistoryBeforeJournalMutation()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        store.Append(new ProductionCreatedEvent("First"));

        var before = new FileProductionJournal(directory.Path).ReadAll().Single();

        Assert.ThrowsExactly<InvalidOperationException>(
            () => store.Append(new ProductionCreatedEvent("Second")));

        var after = new FileProductionJournal(directory.Path).ReadAll().Single();
        Assert.AreEqual(before.RecordHash, after.RecordHash);
        Assert.AreEqual(before.Sequence, after.Sequence);
    }

    [TestMethod]
    public void LoadAllRejectsReplayInvalidCommittedHistory()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(CreatedPayload("First"));
        journal.Append(CreatedPayload("Second"));

        var store = new FileProductionEventStore(directory.Path);

        Assert.ThrowsExactly<InvalidOperationException>(() => store.LoadAll());
    }

    [TestMethod]
    public void RecoverPromotesSemanticallyValidInitialCrashEntry()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(CreatedPayload("Harbor"));
        File.Delete(HeadPath(directory.Path));

        var recovered = new FileProductionEventStore(directory.Path).Recover();

        var created = Assert.IsInstanceOfType<ProductionCreatedEvent>(recovered.Single());
        Assert.AreEqual("Harbor", created.ProductionName);
        Assert.HasCount(1, new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void RecoverRejectsFutureKnownContractBeforeHeadPromotion()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(CreatedPayload("Harbor"));
        var committedHead = File.ReadAllBytes(HeadPath(directory.Path));
        journal.Append(Encoding.UTF8.GetBytes(
            "{\"contract\":\"kymaean.production.created.v3\",\"productionName\":\"Future\"}"));
        File.WriteAllBytes(HeadPath(directory.Path), committedHead);

        var store = new FileProductionEventStore(directory.Path);

        Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
            () => store.Recover());
        CollectionAssert.AreEqual(
            committedHead,
            File.ReadAllBytes(HeadPath(directory.Path)));
    }

    [TestMethod]
    public void RecoverRejectsUndecodableCrashSuffixBeforeHeadPromotion()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(CreatedPayload("Harbor"));
        var committedHead = File.ReadAllBytes(HeadPath(directory.Path));
        journal.Append(Encoding.UTF8.GetBytes(
            "{\"contract\":\"kymaean.production.unknown.v1\"}"));
        File.WriteAllBytes(HeadPath(directory.Path), committedHead);

        var store = new FileProductionEventStore(directory.Path);

        Assert.ThrowsExactly<InvalidDataException>(() => store.Recover());
        CollectionAssert.AreEqual(
            committedHead,
            File.ReadAllBytes(HeadPath(directory.Path)));
        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void RecoverRejectsReplayInvalidCrashSuffixBeforeHeadPromotion()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(CreatedPayload("First"));
        var committedHead = File.ReadAllBytes(HeadPath(directory.Path));
        journal.Append(CreatedPayload("Second"));
        File.WriteAllBytes(HeadPath(directory.Path), committedHead);

        var store = new FileProductionEventStore(directory.Path);

        Assert.ThrowsExactly<InvalidOperationException>(() => store.Recover());
        CollectionAssert.AreEqual(
            committedHead,
            File.ReadAllBytes(HeadPath(directory.Path)));
        Assert.Throws<ProductionJournalCorruptionException>(
            () => new FileProductionJournal(directory.Path).ReadAll());
    }

    [TestMethod]
    public void ProductionApplicationFreshStoreReopenReconstructsProjection()
    {
        using var directory = new TestDirectory();
        CreateProduction(directory.Path, "The Glass Harbor");

        var reopened = new ProductionApplication(
            new FileProductionEventStore(directory.Path));
        var projection = reopened.Open();

        Assert.AreEqual("The Glass Harbor", projection.ProductionName);
    }

    [TestMethod]
    public void ProductionApplicationRecoversAfterInitialHeadPublicationInterruption()
    {
        using var directory = new TestDirectory();
        CreateProduction(directory.Path, "The Glass Harbor");
        File.Delete(HeadPath(directory.Path));

        var reopened = new ProductionApplication(
            new FileProductionEventStore(directory.Path));
        var projection = reopened.Recover();

        Assert.AreEqual("The Glass Harbor", projection.ProductionName);
        Assert.AreEqual(
            "The Glass Harbor",
            new ProductionApplication(
                new FileProductionEventStore(directory.Path)).Open().ProductionName);
    }

    private static void CreateProduction(string rootDirectory, string productionName)
    {
        var application = new ProductionApplication(
            new FileProductionEventStore(rootDirectory));
        Assert.AreEqual(productionName, application.Create(productionName).ProductionName);
    }

    private static byte[] CreatedPayload(string productionName) =>
        Encoding.UTF8.GetBytes(
            $"{{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"{productionName}\"}}");

    private static string HeadPath(string rootDirectory) =>
        Path.Combine(rootDirectory, ".journal.head");

    private sealed class TestDirectory : IDisposable
    {
        public TestDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                $"kymaean-event-store-tests-{Guid.NewGuid():N}");
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
