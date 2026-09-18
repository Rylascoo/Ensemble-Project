namespace Kymaean.Application;

public interface IProductionCatalog
{
    ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions();

    ProductAccessResult<ProductionReplayProjection> OpenProduction(
        ProductionId productionId);

    ProductAccessResult<ProductionReplayProjection> RecoverProduction(
        ProductionId productionId);
}
