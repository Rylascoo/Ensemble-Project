using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class ProductionApplicationTests
{
    [TestMethod]
    public void CreateAppendsThenRebuildsFromCommittedHistory()
    {
        var store = new StubEventStore();
        var application = new ProductionApplication(store);

        var projection = application.Create("The Glass Harbor");

        Assert.AreEqual("The Glass Harbor", projection.ProductionName);
        Assert.AreEqual(1, store.AppendCount);
        Assert.AreEqual(1, store.LoadCount);
        Assert.AreEqual(0, store.RecoverCount);
        var created = Assert.IsInstanceOfType<ProductionCreatedEvent>(
            store.Committed.Single());
        Assert.AreEqual("The Glass Harbor", created.ProductionName);
    }

    [TestMethod]
    public void OpenRebuildsOnlyFromCommittedHistory()
    {
        var store = new StubEventStore();
        store.Committed.Add(new ProductionCreatedEvent("Open"));
        store.Recovered.Add(new ProductionCreatedEvent("Recovered"));
        var application = new ProductionApplication(store);
        var projection = application.Open();

        Assert.AreEqual("Open", projection.ProductionName);
        Assert.AreEqual(1, store.LoadCount);
        Assert.AreEqual(0, store.RecoverCount);
    }

    [TestMethod]
    public void RecoverRebuildsOnlyFromRecoveredHistory()
    {
        var store = new StubEventStore();
        store.Committed.Add(new ProductionCreatedEvent("Open"));
        store.Recovered.Add(new ProductionCreatedEvent("Recovered"));

        var application = new ProductionApplication(store);
        var projection = application.Recover();

        Assert.AreEqual("Recovered", projection.ProductionName);
        Assert.AreEqual(0, store.LoadCount);
        Assert.AreEqual(1, store.RecoverCount);
    }

    private sealed class StubEventStore : IProductionEventStore
    {
        public List<ProductionEvent> Committed { get; } = [];

        public List<ProductionEvent> Recovered { get; } = [];
        public int AppendCount { get; private set; }

        public int LoadCount { get; private set; }

        public int RecoverCount { get; private set; }

        public void Append(ProductionEvent productionEvent)
        {
            AppendCount++;
            Committed.Add(productionEvent);
        }

        public IReadOnlyList<ProductionEvent> LoadAll()
        {
            LoadCount++;
            return Committed.ToArray();
        }

        public IReadOnlyList<ProductionEvent> Recover()
        {
            RecoverCount++;
            return Recovered.ToArray();
        }
    }
}
