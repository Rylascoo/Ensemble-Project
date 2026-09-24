namespace Kymaean.Application;

public interface IProductionPerformanceCommitter
{
    ProductAccessResult<ProductionReplayProjection> CommitAcceptedPerformance(
        ProductionId productionId,
        ProductionReplayProjection expectedSource,
        AcceptedPerformance acceptedPerformance);
}
