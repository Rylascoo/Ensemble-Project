namespace Kymaean.Application;

public interface IProductionCatalog
{
    IReadOnlyList<ProductionSummary> ListProductions();

    ProductionReplayProjection OpenProduction(ProductionId productionId);
}
