using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class ProductApplicationCreationTests
{
    [TestMethod]
    public void CreateUpdatesKnownCatalogWithoutImplicitOpenAndAllowsDuplicateNames()
    {
        var first = Summary("P-001", "Same Name");
        var second = new ProductionCreation(
            new ProductionId("P-002"),
            Replay("Same Name"));
        var catalog = new CreationCatalog(first)
        {
            CreationResult =
                ProductAccessResult<ProductionCreation>.Success(second),
        };
        catalog.SetOpenSuccess(second.Id, second.Replay);
        var application = Start(catalog);
        application.Navigate(ApplicationScope.ProductionLibrary);

        var result = application.CreateProduction("Same Name");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(second.Id, result.Value.Id);
        Assert.AreEqual("Same Name", result.Value.Replay.ProductionName);
        Assert.IsTrue(result.Value.Replay.WorldCurrentState.IsEmpty);

        var projection = application.Query();
        Assert.AreEqual(ApplicationScope.ProductionLibrary, projection.ActiveScope);
        Assert.IsNull(projection.CurrentProduction);
        Assert.IsNull(projection.CurrentProductionReplay);
        Assert.IsNull(projection.ActiveProductSpace);
        Assert.HasCount(2, projection.Productions);
        Assert.AreEqual(2, projection.Productions.Count(
            item => item.ProductionName == "Same Name"));
        Assert.AreEqual(1, catalog.CreateCount);
        Assert.AreEqual(0, catalog.OpenCount);

        var opened = application.OpenProduction(second.Id);
        Assert.IsTrue(opened.IsSuccess);
        Assert.AreEqual(second.Id, opened.Value.CurrentProduction!.Id);
        Assert.AreEqual(1, catalog.OpenCount);
    }

    [TestMethod]
    public void CreatePreservesAlreadyOpenProductionScopeAndProductSpace()
    {
        var first = Summary("P-001", "First");
        var second = new ProductionCreation(
            new ProductionId("P-002"),
            Replay("Second"));
        var catalog = new CreationCatalog(first)
        {
            CreationResult =
                ProductAccessResult<ProductionCreation>.Success(second),
        };
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Archive);
        application.Navigate(ApplicationScope.Settings);

        var before = application.Query();
        var result = application.CreateProduction("Second");
        var after = application.Query();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(before.ActiveScope, after.ActiveScope);
        Assert.AreEqual(before.CurrentProduction, after.CurrentProduction);
        Assert.AreEqual(
            before.CurrentProductionReplay,
            after.CurrentProductionReplay);
        Assert.IsNull(after.ActiveProductSpace);
        Assert.HasCount(2, after.Productions);

        var returned = application.Navigate(ApplicationScope.CurrentProduction);
        Assert.AreEqual(first.Id, returned.CurrentProduction!.Id);
        Assert.AreEqual(ProductSpace.Archive, returned.ActiveProductSpace);
    }

    [TestMethod]
    public void TypedCreateFailurePreservesApplicationState()
    {
        var first = Summary("P-001", "First");
        var catalog = new CreationCatalog(first)
        {
            CreationResult =
                ProductAccessResult<ProductionCreation>.Failure(
                    ProductAccessFailureKind.Incompatible),
        };
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Studio);
        var before = application.Query();

        var result = application.CreateProduction("Second");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
        AssertSameState(before, application.Query());
    }

    [TestMethod]
    public void CreatorCapabilityIsRequired()
    {
        var application = Start(
            new ReadOnlyCatalog(Summary("P-001", "First")));

        Assert.ThrowsExactly<InvalidOperationException>(
            () => application.CreateProduction("Second"));
    }

    [TestMethod]
    public void InvalidNameFailsBeforeCreatorCall()
    {
        var catalog = new CreationCatalog();
        var application = Start(catalog);

        Assert.ThrowsExactly<ArgumentException>(
            () => application.CreateProduction("   "));

        Assert.AreEqual(0, catalog.CreateCount);
        Assert.HasCount(0, application.Query().Productions);
    }

    [TestMethod]
    public void DuplicateReturnedIdentityFailsClosed()
    {
        var first = Summary("P-001", "First");
        var catalog = new CreationCatalog(first)
        {
            CreationResult =
                ProductAccessResult<ProductionCreation>.Success(
                    new ProductionCreation(
                        first.Id,
                        Replay("Second"))),
        };
        var application = Start(catalog);

        var result = application.CreateProduction("Second");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.HasCount(1, application.Query().Productions);
    }

    [TestMethod]
    public void MismatchedCreationReplayFailsClosed()
    {
        var catalog = new CreationCatalog
        {
            CreationResult =
                ProductAccessResult<ProductionCreation>.Success(
                    new ProductionCreation(
                        new ProductionId("P-001"),
                        Replay("Different"))),
        };
        var application = Start(catalog);

        var result = application.CreateProduction("Requested");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.HasCount(0, application.Query().Productions);
    }

    [TestMethod]
    public void NonEmptyCreationReplayFailsClosed()
    {
        var catalog = new CreationCatalog
        {
            CreationResult =
                ProductAccessResult<ProductionCreation>.Success(
                    new ProductionCreation(
                        new ProductionId("P-001"),
                        Replay(
                            "First",
                            new WorldCurrentState(
                                new[] { new WorldCurrentTruth("Already true") })))),
        };
        var application = Start(catalog);

        var result = application.CreateProduction("First");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.HasCount(0, application.Query().Productions);
    }

    [TestMethod]
    public void ExactCreationNameIsNotNormalized()
    {
        var name = new string(new[] { ' ', (char)0xD800, ' ' });
        var creation = new ProductionCreation(
            new ProductionId("P-001"),
            Replay(name));
        var catalog = new CreationCatalog
        {
            CreationResult =
                ProductAccessResult<ProductionCreation>.Success(creation),
        };
        var application = Start(catalog);

        var result = application.CreateProduction(name);

        Assert.IsTrue(result.IsSuccess);
        CollectionAssert.AreEqual(
            name.Select(character => (ushort)character).ToArray(),
            result.Value.Replay.ProductionName
                .Select(character => (ushort)character)
                .ToArray());
        CollectionAssert.AreEqual(
            name.Select(character => (ushort)character).ToArray(),
            application.Query().Productions[0].ProductionName
                .Select(character => (ushort)character)
                .ToArray());
    }

    private static ProductApplication Start(IProductionCatalog catalog)
    {
        var result = ProductApplication.Start(catalog);
        Assert.IsTrue(result.IsSuccess);
        return result.Value;
    }

    private static ProductionSummary Summary(string id, string name) =>
        new(new ProductionId(id), name);

    private static ProductionReplayProjection Replay(string name) =>
        new(name);

    private static ProductionReplayProjection Replay(
        string name,
        WorldCurrentState currentState) =>
        new(name, currentState);

    private static void AssertSameState(
        ProductApplicationProjection expected,
        ProductApplicationProjection actual)
    {
        Assert.AreEqual(expected.ActiveScope, actual.ActiveScope);
        Assert.AreEqual(expected.CurrentProduction, actual.CurrentProduction);
        Assert.AreEqual(
            expected.CurrentProductionReplay,
            actual.CurrentProductionReplay);
        Assert.AreEqual(expected.ActiveProductSpace, actual.ActiveProductSpace);
        CollectionAssert.AreEqual(
            expected.Productions.ToArray(),
            actual.Productions.ToArray());
    }

    private sealed class CreationCatalog :
        IProductionCatalog,
        IProductionCreator
    {
        private readonly List<ProductionSummary> _productions;
        private readonly Dictionary<
            ProductionId,
            ProductAccessResult<ProductionReplayProjection>> _openResults = [];

        public CreationCatalog(params ProductionSummary[] productions)
        {
            _productions = productions.ToList();
        }

        public ProductAccessResult<ProductionCreation> CreationResult { get; set; } =
            ProductAccessResult<ProductionCreation>.Failure(
                ProductAccessFailureKind.Invalid);

        public int CreateCount { get; private set; }
        public int OpenCount { get; private set; }

        public ProductAccessResult<IReadOnlyList<ProductionSummary>>
            ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                _productions.ToArray());

        public ProductAccessResult<ProductionCreation> CreateProduction(
            string productionName)
        {
            CreateCount++;
            return CreationResult;
        }

        public void SetOpenSuccess(
            ProductionId id,
            ProductionReplayProjection replay) =>
            _openResults[id] =
                ProductAccessResult<ProductionReplayProjection>.Success(replay);

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId)
        {
            OpenCount++;
            return _openResults.TryGetValue(productionId, out var result)
                ? result
                : ProductAccessResult<ProductionReplayProjection>.Failure(
                    ProductAccessFailureKind.Invalid);
        }

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
    }

    private sealed class ReadOnlyCatalog : IProductionCatalog
    {
        private readonly ProductionSummary _summary;

        public ReadOnlyCatalog(ProductionSummary summary)
        {
            _summary = summary;
        }

        public ProductAccessResult<IReadOnlyList<ProductionSummary>>
            ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                new[] { _summary });

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
    }
}
