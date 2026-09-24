using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class ProductApplicationSceneTests
{
    [TestMethod]
    public void EstablishSceneUpdatesReplayWithoutChangingNavigationOrProductState()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var cast = Cast("C-1", "C-2");
        var before = Replay(cast, ProductionScenes.Empty);
        var scene = new EstablishedScene(
            new SceneId("S-1"),
            new SceneRoster([new CharacterId("C-1"), new CharacterId("C-2")]));
        var after = Replay(cast, new ProductionScenes([scene]));
        var catalog = new SceneCatalog(production, before)
        {
            EstablishmentResult = ProductAccessResult<SceneCreation>.Success(
                new SceneCreation(scene, after))
        };
        var application = Start(catalog);
        Assert.IsTrue(
            application.OpenProduction(production.Id, ProductSpace.Archive).IsSuccess);

        var result = application.EstablishScene(
            [new CharacterId("C-2"), new CharacterId("C-1")]);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(scene, result.Value.Scene);
        Assert.AreEqual(after, application.Query().CurrentProductionReplay);
        Assert.AreEqual(ApplicationScope.CurrentProduction, application.Query().ActiveScope);
        Assert.AreEqual(ProductSpace.Archive, application.Query().ActiveProductSpace);
        Assert.AreEqual(1, catalog.EstablishCount);
    }

    [TestMethod]
    public void EstablishSceneAcceptsConcurrentPerformanceHistoryExtension()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var cast = Cast("C-1");
        var existingScene = new EstablishedScene(
            new SceneId("S-0"),
            new SceneRoster([new CharacterId("C-1")]));
        var before = new ProductionReplayProjection(
            "Harbor",
            new WorldCurrentState([new WorldCurrentTruth("Existing.")]),
            cast,
            new ProductionScenes([existingScene]));
        var newScene = new EstablishedScene(
            new SceneId("S-1"),
            new SceneRoster([new CharacterId("C-1")]));
        var concurrent = new AcceptedPerformance(
            existingScene.Id,
            new CharacterId("C-1"),
            "I keep watch.",
            new CharacterCircumstance("Name C-1 is keeping watch."));
        var after = new ProductionReplayProjection(
            before.ProductionName,
            before.WorldCurrentState,
            before.ProductionCast,
            new ProductionScenes([existingScene, newScene]),
            new AcceptedPerformanceHistory([concurrent]));
        var catalog = new SceneCatalog(production, before)
        {
            EstablishmentResult =
                ProductAccessResult<SceneCreation>.Success(
                    new SceneCreation(newScene, after))
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var result = application.EstablishScene([new CharacterId("C-1")]);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(after, application.Query().CurrentProductionReplay);
    }

    [TestMethod]
    public void EstablishSceneRequiresOpenProductionAndCapability()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var replay = Replay(Cast("C-1"), ProductionScenes.Empty);
        var application = Start(new SceneCatalog(production, replay));

        Assert.ThrowsExactly<InvalidOperationException>(
            () => application.EstablishScene([new CharacterId("C-1")]));

        var readOnly = Start(new ReadOnlyCatalog(production, replay));
        Assert.IsTrue(readOnly.OpenProduction(production.Id).IsSuccess);
        Assert.ThrowsExactly<InvalidOperationException>(
            () => readOnly.EstablishScene([new CharacterId("C-1")]));
    }

    [TestMethod]
    public void InvalidRosterIsRejectedBeforePersistence()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var catalog = new SceneCatalog(
            production,
            Replay(Cast("C-1"), ProductionScenes.Empty));
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        Assert.ThrowsExactly<ArgumentException>(
            () => application.EstablishScene(
                [new CharacterId("C-1"), new CharacterId("C-1")]));
        Assert.ThrowsExactly<ArgumentException>(
            () => application.EstablishScene([new CharacterId("C-2")]));
        Assert.AreEqual(0, catalog.EstablishCount);
    }

    [TestMethod]
    public void TypedFailureOrInvalidReturnedStatePreservesApplicationState()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var cast = Cast("C-1");
        var before = Replay(cast, ProductionScenes.Empty);
        var catalog = new SceneCatalog(production, before)
        {
            EstablishmentResult = ProductAccessResult<SceneCreation>.Failure(
                ProductAccessFailureKind.Incompatible)
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);
        var stateBefore = application.Query();

        var failed = application.EstablishScene([new CharacterId("C-1")]);
        Assert.IsFalse(failed.IsSuccess);
        Assert.AreEqual(stateBefore, application.Query());

        var wrongScene = new EstablishedScene(
            new SceneId("S-1"),
            SceneRoster.Empty);
        catalog.EstablishmentResult = ProductAccessResult<SceneCreation>.Success(
            new SceneCreation(
                wrongScene,
                new ProductionReplayProjection(
                    "Changed",
                    new WorldCurrentState([new WorldCurrentTruth("Invented.")]),
                    ProductionCast.Empty,
                    new ProductionScenes([wrongScene]))));

        var invalid = application.EstablishScene([new CharacterId("C-1")]);
        Assert.IsFalse(invalid.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, invalid.FailureKind);
        Assert.AreEqual(stateBefore, application.Query());
    }

    private static ProductApplication Start(IProductionCatalog catalog)
    {
        var result = ProductApplication.Start(catalog);
        Assert.IsTrue(result.IsSuccess);
        return result.Value;
    }

    private static ProductionCast Cast(params string[] ids) =>
        new(ids.Select(
            id => new CharacterSummary(new CharacterId(id), $"Name {id}")));

    private static ProductionReplayProjection Replay(
        ProductionCast cast,
        ProductionScenes scenes) =>
        new(
            "Harbor",
            new WorldCurrentState([new WorldCurrentTruth("Existing.")]),
            cast,
            scenes);

    private sealed class SceneCatalog : IProductionCatalog, IProductionSceneCreator
    {
        private readonly ProductionSummary _production;
        private readonly ProductionReplayProjection _replay;

        public SceneCatalog(
            ProductionSummary production,
            ProductionReplayProjection replay)
        {
            _production = production;
            _replay = replay;
        }

        public ProductAccessResult<SceneCreation> EstablishmentResult { get; set; } =
            ProductAccessResult<SceneCreation>.Failure(ProductAccessFailureKind.Invalid);

        public int EstablishCount { get; private set; }

        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                new[] { _production });

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Success(_replay);

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            OpenProduction(productionId);

        public ProductAccessResult<SceneCreation> EstablishScene(
            ProductionId productionId,
            SceneRoster initialRoster)
        {
            Assert.AreEqual(_production.Id, productionId);
            EstablishCount++;
            return EstablishmentResult;
        }
    }

    private sealed class ReadOnlyCatalog : IProductionCatalog
    {
        private readonly ProductionSummary _production;
        private readonly ProductionReplayProjection _replay;

        public ReadOnlyCatalog(
            ProductionSummary production,
            ProductionReplayProjection replay)
        {
            _production = production;
            _replay = replay;
        }

        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                new[] { _production });

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Success(_replay);

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            OpenProduction(productionId);
    }
}
