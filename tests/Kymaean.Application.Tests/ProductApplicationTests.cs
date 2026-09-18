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

        var projection = Start(catalog).Query();

        Assert.AreEqual(ApplicationScope.Home, projection.ActiveScope);
        Assert.AreEqual(2, projection.Productions.Length);
        Assert.IsNull(projection.CurrentProduction);
        Assert.IsNull(projection.CurrentProductionReplay);
        Assert.IsFalse(projection.HasCurrentProduction);
        Assert.IsNull(projection.ActiveProductSpace);
        Assert.AreEqual(1, catalog.ListCount);
        Assert.AreEqual(0, catalog.OpenCount);
    }

    [TestMethod]
    public void EmptyProductionLibraryIsValidBootstrapSuccess()
    {
        var catalog = new StubCatalog();

        var result = ProductApplication.Start(catalog);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(0, result.Value.Query().Productions.Length);
        Assert.AreEqual(ApplicationScope.Home, result.Value.Query().ActiveScope);
        Assert.AreEqual(1, catalog.ListCount);
    }

    [TestMethod]
    public void BootstrapIncompatibleReturnsTypedFailure()
    {
        var result = ProductApplication.Start(
            StubCatalog.WithListFailure(ProductAccessFailureKind.Incompatible));

        AssertFailure(result, ProductAccessFailureKind.Incompatible);
    }

    [TestMethod]
    public void BootstrapInvalidReturnsTypedFailure()
    {
        var result = ProductApplication.Start(
            StubCatalog.WithListFailure(ProductAccessFailureKind.Invalid));

        AssertFailure(result, ProductAccessFailureKind.Invalid);
    }

    [TestMethod]
    public void DuplicateProductionIdentityReturnsTypedInvalid()
    {
        var first = Summary("P-001", "First");
        var duplicate = Summary("P-001", "Different name");

        var result = ProductApplication.Start(
            new StubCatalog(first, duplicate));

        AssertFailure(result, ProductAccessFailureKind.Invalid);
    }

    [TestMethod]
    public void NullProductionEntryReturnsTypedInvalid()
    {
        var result = ProductApplication.Start(
            new StubCatalog(
                Summary("P-001", "First"),
                null!));

        AssertFailure(result, ProductAccessFailureKind.Invalid);
    }

    [TestMethod]
    public void ApplicationScopeNavigationDoesNotRequireProduction()
    {
        var application = Start(
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
        var application = Start(
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
        catalog.SetOpenSuccess(first.Id, Replay("Same Name"));
        catalog.SetOpenSuccess(second.Id, Replay("Same Name"));

        var result = Start(catalog).OpenProduction(second.Id);

        Assert.IsTrue(result.IsSuccess);
        var projection = result.Value;
        Assert.AreEqual(ApplicationScope.CurrentProduction, projection.ActiveScope);
        Assert.AreEqual(ProductSpace.Stage, projection.ActiveProductSpace);
        Assert.AreEqual(second.Id, projection.CurrentProduction!.Id);
        Assert.AreEqual(
            "Same Name",
            projection.CurrentProductionReplay!.ProductionName);
        Assert.AreEqual(second.Id, catalog.LastOpenedId);
        Assert.AreEqual(1, catalog.OpenCount);
    }

    [TestMethod]
    public void UnknownProductionReturnsInvalidWithoutOpening()
    {
        var catalog = new StubCatalog(Summary("P-001", "First"));
        var application = Start(catalog);

        var result = application.OpenProduction(new ProductionId("P-404"));

        AssertFailure(result, ProductAccessFailureKind.Invalid);
        Assert.IsFalse(application.Query().HasCurrentProduction);
        Assert.AreEqual(0, catalog.OpenCount);
    }

    [TestMethod]
    public void OpenIncompatibleLeavesPriorStateUnchanged()
    {
        var first = Summary("P-001", "First");
        var second = Summary("P-002", "Second");
        var catalog = new StubCatalog(first, second);
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        catalog.SetOpenFailure(
            second.Id,
            ProductAccessFailureKind.Incompatible);
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Archive);
        var before = application.Query();

        var result = application.OpenProduction(second.Id);

        AssertFailure(result, ProductAccessFailureKind.Incompatible);
        AssertSameState(before, application.Query());
    }

    [TestMethod]
    public void OpenInvalidLeavesPriorStateUnchanged()
    {
        var first = Summary("P-001", "First");
        var second = Summary("P-002", "Second");
        var catalog = new StubCatalog(first, second);
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        catalog.SetOpenFailure(second.Id, ProductAccessFailureKind.Invalid);
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Studio);
        var before = application.Query();

        var result = application.OpenProduction(second.Id);

        AssertFailure(result, ProductAccessFailureKind.Invalid);
        AssertSameState(before, application.Query());
    }

    [TestMethod]
    public void CatalogNameMismatchReturnsInvalidAndLeavesStateUnchanged()
    {
        var first = Summary("P-001", "First");
        var second = Summary("P-002", "Listed Name");
        var catalog = new StubCatalog(first, second);
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        catalog.SetOpenSuccess(second.Id, Replay("Persisted Name"));
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Archive);
        var before = application.Query();

        var result = application.OpenProduction(second.Id);

        AssertFailure(result, ProductAccessFailureKind.Invalid);
        AssertSameState(before, application.Query());
        Assert.AreEqual(2, catalog.OpenCount);
    }

    [TestMethod]
    public void ProductionNavigationUsesSeparateProductSpaceAxis()
    {
        var summary = Summary("P-001", "First");
        var catalog = new StubCatalog(summary);
        catalog.SetOpenSuccess(summary.Id, Replay("First"));
        var application = Start(catalog);

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
        catalog.SetOpenSuccess(summary.Id, Replay("First"));
        var application = Start(catalog);

        application.OpenProduction(summary.Id, ProductSpace.Archive);
        var settings = application.Navigate(ApplicationScope.Settings);
        var returned = application.Navigate(ApplicationScope.CurrentProduction);

        Assert.IsNull(settings.ActiveProductSpace);
        Assert.AreEqual(ProductSpace.Archive, returned.ActiveProductSpace);
        Assert.AreEqual(1, catalog.OpenCount);
    }

    [TestMethod]
    public void UnknownProductSpaceFailsClosedBeforeCatalogOpen()
    {
        var summary = Summary("P-001", "First");
        var catalog = new StubCatalog(summary);
        catalog.SetOpenSuccess(summary.Id, Replay("First"));
        var application = Start(catalog);

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => application.OpenProduction(summary.Id, (ProductSpace)99));

        Assert.AreEqual(0, catalog.OpenCount);
        Assert.IsFalse(application.Query().HasCurrentProduction);
    }

    [TestMethod]
    public void UnknownApplicationScopeFailsClosed()
    {
        var application = Start(
            new StubCatalog(Summary("P-001", "First")));

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => application.Navigate((ApplicationScope)99));

        Assert.AreEqual(ApplicationScope.Home, application.Query().ActiveScope);
    }

    [TestMethod]
    public void UnknownProductionRecoveryReturnsInvalidWithoutCatalogCall()
    {
        var catalog = new StubCatalog(Summary("P-001", "First"));
        var application = Start(catalog);

        var result = application.RecoverProduction(
            new ProductionId("P-404"));

        AssertFailure(result, ProductAccessFailureKind.Invalid);
        Assert.IsFalse(application.Query().HasCurrentProduction);
        Assert.AreEqual(0, catalog.RecoverCount);
    }

    [TestMethod]
    public void RecoverProductionIsExplicitAndCanEstablishCurrentProduction()
    {
        var summary = Summary("P-001", "First");
        var catalog = new StubCatalog(summary);
        catalog.SetRecoverSuccess(summary.Id, Replay("First"));
        var application = Start(catalog);

        var result = application.RecoverProduction(
            summary.Id,
            ProductSpace.Archive);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(summary.Id, result.Value.CurrentProduction!.Id);
        Assert.AreEqual(ProductSpace.Archive, result.Value.ActiveProductSpace);
        Assert.AreEqual(0, catalog.OpenCount);
        Assert.AreEqual(1, catalog.RecoverCount);
    }

    [TestMethod]
    public void RecoverIncompatibleLeavesPriorStateUnchanged()
    {
        var first = Summary("P-001", "First");
        var second = Summary("P-002", "Second");
        var catalog = new StubCatalog(first, second);
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        catalog.SetRecoverFailure(
            second.Id,
            ProductAccessFailureKind.Incompatible);
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Studio);
        var before = application.Query();

        var result = application.RecoverProduction(second.Id);

        AssertFailure(result, ProductAccessFailureKind.Incompatible);
        AssertSameState(before, application.Query());
    }

    [TestMethod]
    public void RecoverInvalidLeavesPriorStateUnchanged()
    {
        var first = Summary("P-001", "First");
        var second = Summary("P-002", "Second");
        var catalog = new StubCatalog(first, second);
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        catalog.SetRecoverFailure(second.Id, ProductAccessFailureKind.Invalid);
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Archive);
        var before = application.Query();

        var result = application.RecoverProduction(second.Id);

        AssertFailure(result, ProductAccessFailureKind.Invalid);
        AssertSameState(before, application.Query());
    }

    [TestMethod]
    public void RecoveredNameMismatchReturnsInvalidAndLeavesStateUnchanged()
    {
        var first = Summary("P-001", "First");
        var second = Summary("P-002", "Listed Name");
        var catalog = new StubCatalog(first, second);
        catalog.SetOpenSuccess(first.Id, Replay("First"));
        catalog.SetRecoverSuccess(second.Id, Replay("Persisted Name"));
        var application = Start(catalog);
        application.OpenProduction(first.Id, ProductSpace.Archive);
        var before = application.Query();

        var result = application.RecoverProduction(second.Id);

        AssertFailure(result, ProductAccessFailureKind.Invalid);
        AssertSameState(before, application.Query());
        Assert.AreEqual(1, catalog.RecoverCount);
    }

    [TestMethod]
    public void ResultSuccessRejectsNullValue()
    {
        Assert.ThrowsExactly<ArgumentNullException>(
            () => ProductAccessResult<string>.Success(null!));
    }

    [TestMethod]
    public void ResultFailureRejectsUnknownKind()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => ProductAccessResult<string>.Failure(
                (ProductAccessFailureKind)99));
    }

    [TestMethod]
    public void ResultVariantsExposeOnlyTheirOwnPayload()
    {
        var success = ProductAccessResult<string>.Success("value");
        var failure = ProductAccessResult<string>.Failure(
            ProductAccessFailureKind.Invalid);

        Assert.AreEqual("value", success.Value);
        Assert.ThrowsExactly<InvalidOperationException>(
            () => _ = success.FailureKind);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, failure.FailureKind);
        Assert.ThrowsExactly<InvalidOperationException>(
            () => _ = failure.Value);
    }

    private static ProductApplication Start(StubCatalog catalog)
    {
        var result = ProductApplication.Start(catalog);
        Assert.IsTrue(result.IsSuccess);
        return result.Value;
    }

    private static void AssertFailure<T>(
        ProductAccessResult<T> result,
        ProductAccessFailureKind expected)
    {
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(expected, result.FailureKind);
    }

    private static void AssertSameState(
        ProductApplicationProjection expected,
        ProductApplicationProjection actual)

    {
        Assert.AreEqual(expected.ActiveScope, actual.ActiveScope);
        Assert.AreEqual(expected.CurrentProduction, actual.CurrentProduction);
        Assert.AreEqual(
            expected.CurrentProductionReplay,
            actual.CurrentProductionReplay);
        Assert.AreEqual(
            expected.ActiveProductSpace,
            actual.ActiveProductSpace);
        CollectionAssert.AreEqual(
            expected.Productions.ToArray(),
            actual.Productions.ToArray());
    }

    private static ProductionSummary Summary(string id, string name) =>
        new(new ProductionId(id), name);

    private static ProductionReplayProjection Replay(string name) =>
        new(name);

    private sealed class StubCatalog : IProductionCatalog
    {
        private readonly ProductAccessResult<IReadOnlyList<ProductionSummary>>
            _listResult;

        private readonly Dictionary<
            ProductionId,
            ProductAccessResult<ProductionReplayProjection>> _openResults = [];
        private readonly Dictionary<
            ProductionId,
            ProductAccessResult<ProductionReplayProjection>> _recoverResults = [];

        public StubCatalog(params ProductionSummary[] productions)
            : this(
                ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                    productions))
        {
        }

        private StubCatalog(
            ProductAccessResult<IReadOnlyList<ProductionSummary>> listResult)
        {
            _listResult = listResult;
        }

        public int ListCount { get; private set; }
        public int OpenCount { get; private set; }
        public int RecoverCount { get; private set; }
        public ProductionId? LastOpenedId { get; private set; }
        public ProductionId? LastRecoveredId { get; private set; }

        public static StubCatalog WithListFailure(
            ProductAccessFailureKind kind) =>
            new(
                ProductAccessResult<IReadOnlyList<ProductionSummary>>.Failure(
                    kind));

        public void SetOpenSuccess(
            ProductionId id,
            ProductionReplayProjection replay) =>
            _openResults[id] =
                ProductAccessResult<ProductionReplayProjection>.Success(replay);

        public void SetOpenFailure(
            ProductionId id,
            ProductAccessFailureKind kind) =>
            _openResults[id] =
                ProductAccessResult<ProductionReplayProjection>.Failure(kind);

        public void SetRecoverSuccess(
            ProductionId id,
            ProductionReplayProjection replay) =>
            _recoverResults[id] =
                ProductAccessResult<ProductionReplayProjection>.Success(replay);
        public void SetRecoverFailure(
            ProductionId id,
            ProductAccessFailureKind kind) =>
            _recoverResults[id] =
                ProductAccessResult<ProductionReplayProjection>.Failure(kind);

        public ProductAccessResult<IReadOnlyList<ProductionSummary>>
            ListProductions()
        {
            ListCount++;
            return _listResult;
        }

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId)
        {
            OpenCount++;
            LastOpenedId = productionId;
            return _openResults.TryGetValue(productionId, out var result)
                ? result
                : ProductAccessResult<ProductionReplayProjection>.Failure(
                    ProductAccessFailureKind.Invalid);
        }
        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId)
        {
            RecoverCount++;
            LastRecoveredId = productionId;
            return _recoverResults.TryGetValue(productionId, out var result)
                ? result
                : ProductAccessResult<ProductionReplayProjection>.Failure(
                    ProductAccessFailureKind.Invalid);
        }
    }
}
