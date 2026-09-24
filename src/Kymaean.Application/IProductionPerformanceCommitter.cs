namespace Kymaean.Application;

public interface IProductionPerformanceCommitter
{
    ProductAccessResult<ProductionReplayProjection> CommitAcceptedPerformance(
        ProductionId productionId,
        AcceptedPerformance acceptedPerformance);
}
