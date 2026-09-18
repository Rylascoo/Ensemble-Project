using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed class ProductApplication
{
    private readonly IProductionCatalog _catalog;
    private readonly ImmutableArray<ProductionSummary> _productions;
    private ProductionSummary? _currentProduction;
    private ProductionReplayProjection? _currentProductionReplay;
    private ApplicationScope _activeScope = ApplicationScope.Home;
    private ProductSpace _currentProductSpace = ProductSpace.Stage;

    private ProductApplication(
        IProductionCatalog catalog,
        ImmutableArray<ProductionSummary> productions)
    {
        _catalog = catalog;
        _productions = productions;
    }

    public static ProductAccessResult<ProductApplication> Start(
        IProductionCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);

        var listed = catalog.ListProductions();
        if (!listed.IsSuccess)
        {
            return ProductAccessResult<ProductApplication>.Failure(
                listed.FailureKind);
        }

        var productions = listed.Value.ToImmutableArray();
        if (!IsValidProductionList(productions))
        {
            return ProductAccessResult<ProductApplication>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return ProductAccessResult<ProductApplication>.Success(
            new ProductApplication(catalog, productions));
    }

    public ProductApplicationProjection Query() =>
        new(
            _activeScope,
            _productions,
            _currentProduction,
            _currentProductionReplay,
            _activeScope == ApplicationScope.CurrentProduction
                ? _currentProductSpace
                : null);

    public ProductApplicationProjection Navigate(ApplicationScope destination)
    {
        if (!Enum.IsDefined(destination))
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        if (destination == ApplicationScope.CurrentProduction
            && _currentProduction is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before entering its workspace.");
        }

        _activeScope = destination;
        return Query();
    }

    public ProductAccessResult<ProductApplicationProjection> OpenProduction(
        ProductionId productionId,
        ProductSpace destination = ProductSpace.Stage)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ValidateProductSpace(destination);

        var summary = FindProduction(productionId);
        if (summary is null)
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return CompleteProductionAccess(
            summary,
            _catalog.OpenProduction(productionId),
            destination);
    }

    public ProductAccessResult<ProductApplicationProjection> RecoverProduction(
        ProductionId productionId,
        ProductSpace destination = ProductSpace.Stage)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ValidateProductSpace(destination);

        var summary = FindProduction(productionId);
        if (summary is null)
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return CompleteProductionAccess(
            summary,
            _catalog.RecoverProduction(productionId),
            destination);
    }

    public ProductApplicationProjection NavigateProduction(ProductSpace destination)
    {
        if (_currentProduction is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before navigating within it.");
        }

        ValidateProductSpace(destination);
        _currentProductSpace = destination;
        _activeScope = ApplicationScope.CurrentProduction;
        return Query();
    }

    private ProductAccessResult<ProductApplicationProjection>
        CompleteProductionAccess(
            ProductionSummary summary,
            ProductAccessResult<ProductionReplayProjection> access,
            ProductSpace destination)
    {
        if (!access.IsSuccess)
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                access.FailureKind);
        }

        var replay = access.Value;
        if (!string.Equals(
                summary.ProductionName,
                replay.ProductionName,
                StringComparison.Ordinal))
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        _currentProduction = summary;
        _currentProductionReplay = replay;
        _currentProductSpace = destination;
        _activeScope = ApplicationScope.CurrentProduction;

        return ProductAccessResult<ProductApplicationProjection>.Success(
            Query());
    }

    private ProductionSummary? FindProduction(ProductionId productionId) =>
        _productions.FirstOrDefault(item => item.Id == productionId);

    private static void ValidateProductSpace(ProductSpace space)
    {
        if (!Enum.IsDefined(space))
        {
            throw new ArgumentOutOfRangeException(nameof(space));
        }
    }

    private static bool IsValidProductionList(
        ImmutableArray<ProductionSummary> productions)
    {
        var ids = new HashSet<ProductionId>();

        foreach (var production in productions)
        {
            if (production is null || !ids.Add(production.Id))
            {
                return false;
            }
        }

        return true;
    }
}
