using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class ProductApplicationTests
{
    [TestMethod]
    public void StartsAtHomeWithCatalogAndNoOpenProduction()
    {
        var first = Summary("P-001", "First");
        var second = Summary("P-002", "Second");
        var catalog = new StubCatalog(first, second);

        var projection = new ProductApplication(catalog).Query();

        Assert.AreEqual(ApplicationScope.Home, projection.ActiveScope);
        Assert.AreEqual(2, projection.Productions.Length);
        Assert.IsNull(projection.CurrentProduction);
        Assert.IsNull(projection.CurrentProductionReplay);
        Assert.IsFalse(projection.HasCurrentProduction);
        Assert.IsNull(projection.ActiveProductSpace);
        Assert.AreEqual(0, catalog.OpenCount);
    }

    [TestMethod]
    public void ApplicationScopeNavigationDoesNotRequireProduction()
    {
        var application = new ProductApplication(
            new StubCatalog(Summary("P-001", "First")));

        var library = application.Navigate(ApplicationScope.ProductionLibrary);
        var settings = application.Navigate(ApplicationScope.Settings);

        Assert.AreEqual(ApplicationScope.ProductionLibrary, library.ActiveScope);
        Assert.IsNull(library.ActiveProductSpace);
        Assert.AreEqual(ApplicationScope.Settings, settings.ActiveScope);
        Assert.IsNull(settings.ActiveProductSpace);
    }

    [TestMethod]
    public void CurrentProductionScopeRequiresOpenProduction()
    {
        var application = new ProductApplication(
            new StubCatalog(Summary("P-001", "First")));

        Assert.ThrowsExactly<InvalidOperationException>(
            () => application.Navigate(ApplicationScope.CurrentProduction));

        Assert.AreEqual(ApplicationScope.Home, application.Query().ActiveScope);
    }

    [TestMethod]
    public void OpensProductionByStableIdentityNotName()
    {
        var first = Summary("P-001", "Same Name");
        var second = Summary("P-002", "Same Name");
        var catalog = new StubCatalog(first, second);
        catalog.AddReplay(first.Id, Replay("Same Name"));
        catalog.AddReplay(second.Id, Replay("Same Name"));

        var projection = new ProductApplication(catalog)
            .OpenProduction(second.Id);

        Assert.AreEqual(ApplicationScope.CurrentProduction, projection.ActiveScope);
        Assert.AreEqual(ProductSpace.Stage, projection.ActiveProductSpace);
        Assert.AreEqual(second.Id, projection.CurrentProduction!.Id);
        Assert.AreEqual("Same Name",
            projection.CurrentProductionReplay!.ProductionName);
        Assert.AreEqual(second.Id, catalog.LastOpenedId);
        Assert.AreEqual(1, catalog.OpenCount);
    }

    [TestMethod]
    public void UnknownProductionFailsClosedWithoutOpening()
    {
        var catalog = new StubCatalog(Summary("P-001", "First"));
        var application = new ProductApplication(catalog);

        Assert.ThrowsExactly<KeyNotFoundException>(
            () => application.OpenProduction(new ProductionId("P-404")));

        Assert.IsFalse(application.Query().HasCurrentProduction);
        Assert.AreEqual(0, catalog.OpenCount);
    }

    [TestMethod]
    public void CatalogNameMismatchFailsClosed()
    {
        var summary = Summary("P-001", "Listed Name");
        var catalog = new StubCatalog(summary);
        catalog.AddReplay(summary.Id, Replay("Persisted Name"));
        var application = new ProductApplication(catalog);

        Assert.ThrowsExactly<InvalidOperationException>(
            () => application.OpenProduction(summary.Id));

        Assert.IsFalse(application.Query().HasCurrentProduction);
        Assert.AreEqual(1, catalog.OpenCount);
    }

    [TestMethod]
    public void ProductionNavigationUsesSeparateProductSpaceAxis()
    {
        var summary = Summary("P-001", "First");
        var catalog = new StubCatalog(summary);
        catalog.AddReplay(summary.Id, Replay("First"));
        var application = new ProductApplication(catalog);

        application.OpenProduction(summary.Id);
        var shaping = application.NavigateProduction(ProductSpace.Studio);
        var history = application.NavigateProduction(ProductSpace.Archive);

        Assert.AreEqual(ApplicationScope.CurrentProduction, shaping.ActiveScope);
        Assert.AreEqual(ProductSpace.Studio, shaping.ActiveProductSpace);
        Assert.AreEqual(ApplicationScope.CurrentProduction, history.ActiveScope);
        Assert.AreEqual(ProductSpace.Archive, history.ActiveProductSpace);
        Assert.AreSame(
            shaping.CurrentProductionReplay,
            history.CurrentProductionReplay);
        Assert.AreEqual(1, catalog.OpenCount);
    }

    [TestMethod]
    public void ReturningToProductionPreservesPriorProductSpace()
    {
        var summary = Summary("P-001", "First");
        var catalog = new StubCatalog(summary);
        catalog.AddReplay(summary.Id, Replay("First"));
        var application = new ProductApplication(catalog);

        application.OpenProduction(summary.Id, ProductSpace.Archive);
        var settings = application.Navigate(ApplicationScope.Settings);
        var returned = application.Navigate(ApplicationScope.CurrentProduction);

        Assert.IsNull(settings.ActiveProductSpace);
        Assert.AreEqual(ProductSpace.Archive, returned.ActiveProductSpace);
        Assert.AreEqual(1, catalog.OpenCount);
    }

    [TestMethod]
    public void DuplicateProductionIdentityFailsClosed()
    {
        var first = Summary("P-001", "First");
        var duplicate = Summary("P-001", "Different name");

        Assert.ThrowsExactly<InvalidOperationException>(
            () => new ProductApplication(new StubCatalog(first, duplicate)));
    }

    [TestMethod]
    public void UnknownProductSpaceFailsClosedBeforeCatalogOpen()
    {
        var summary = Summary("P-001", "First");
        var catalog = new StubCatalog(summary);
        catalog.AddReplay(summary.Id, Replay("First"));
        var application = new ProductApplication(catalog);

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => application.OpenProduction(summary.Id, (ProductSpace)99));

        Assert.AreEqual(0, catalog.OpenCount);
        Assert.IsFalse(application.Query().HasCurrentProduction);
    }

    [TestMethod]
    public void UnknownApplicationScopeFailsClosed()
    {
        var application = new ProductApplication(
            new StubCatalog(Summary("P-001", "First")));

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => application.Navigate((ApplicationScope)99));

        Assert.AreEqual(ApplicationScope.Home, application.Query().ActiveScope);
    }

    private static ProductionSummary Summary(string id, string name) =>
        new(new ProductionId(id), name);

    private static ProductionReplayProjection Replay(string name) =>
        new(name);

    private sealed class StubCatalog : IProductionCatalog
    {
        private readonly ProductionSummary[] _productions;
        private readonly Dictionary<ProductionId, ProductionReplayProjection> _replays = [];

        public StubCatalog(params ProductionSummary[] productions)
        {
            _productions = productions;
        }

        public int OpenCount { get; private set; }
        public ProductionId? LastOpenedId { get; private set; }

        public void AddReplay(
            ProductionId id,
            ProductionReplayProjection replay) =>
            _replays.Add(id, replay);

        public IReadOnlyList<ProductionSummary> ListProductions() =>
            _productions;

        public ProductionReplayProjection OpenProduction(
            ProductionId productionId)
        {
            OpenCount++;
            LastOpenedId = productionId;
            return _replays.TryGetValue(productionId, out var replay)
                ? replay
                : throw new InvalidOperationException(
                    $"No test Production replay for '{productionId}'.");
        }
    }
}
