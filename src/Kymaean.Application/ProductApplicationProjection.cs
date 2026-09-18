using System.Collections.Immutable;

namespace Kymaean.Application;

public enum ApplicationScope
{
    Home,
    ProductionLibrary,
    CurrentProduction,
    Settings
}

public sealed record ProductApplicationProjection
{
    internal ProductApplicationProjection(
        ApplicationScope activeScope,
        ImmutableArray<ProductionSummary> productions,
        ProductionSummary? currentProduction,
        ProductionReplayProjection? currentProductionReplay,
        ProductSpace? activeProductSpace)
    {
        ActiveScope = activeScope;
        Productions = productions;
        CurrentProduction = currentProduction;
        CurrentProductionReplay = currentProductionReplay;
        ActiveProductSpace = activeProductSpace;
    }

    public ApplicationScope ActiveScope { get; }

    public ImmutableArray<ProductionSummary> Productions { get; }

    public ProductionSummary? CurrentProduction { get; }

    public ProductionReplayProjection? CurrentProductionReplay { get; }

    public ProductSpace? ActiveProductSpace { get; }

    public bool HasCurrentProduction =>
        CurrentProduction is not null && CurrentProductionReplay is not null;
}
