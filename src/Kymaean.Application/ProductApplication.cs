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

    public ProductApplication(IProductionCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        _catalog = catalog;

        var listed = catalog.ListProductions()
            ?? throw new InvalidOperationException(
                "Production catalog returned no production list.");

        _productions = listed.ToImmutableArray();
        ValidateProductionList(_productions);
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

    public ProductApplicationProjection OpenProduction(
        ProductionId productionId,
        ProductSpace destination = ProductSpace.Stage)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ValidateProductSpace(destination);

        var summary = _productions.FirstOrDefault(item => item.Id == productionId)
            ?? throw new KeyNotFoundException(
                $"Production '{productionId}' is not available.");

        var replay = _catalog.OpenProduction(productionId)
            ?? throw new InvalidOperationException(
                "Production catalog returned no replay projection.");

        if (!string.Equals(
                summary.ProductionName,
                replay.ProductionName,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Production catalog summary does not match the opened Production.");
        }

        _currentProduction = summary;
        _currentProductionReplay = replay;
        _currentProductSpace = destination;
        _activeScope = ApplicationScope.CurrentProduction;
        return Query();
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

    private static void ValidateProductSpace(ProductSpace space)
    {
        if (!Enum.IsDefined(space))
        {
            throw new ArgumentOutOfRangeException(nameof(space));
        }
    }

    private static void ValidateProductionList(
        ImmutableArray<ProductionSummary> productions)
    {
        var ids = new HashSet<ProductionId>();

        for (var index = 0; index < productions.Length; index++)
        {
            var production = productions[index]
                ?? throw new InvalidOperationException(
                    $"Production catalog contains a null entry at index {index}.");

            if (!ids.Add(production.Id))
            {
                throw new InvalidOperationException(
                    $"Production catalog contains duplicate id '{production.Id}'.");
            }
        }
    }
}
