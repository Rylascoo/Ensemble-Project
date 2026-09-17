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
            "{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"Glass Harbor\"}",
            payload);
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
