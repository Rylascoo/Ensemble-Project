namespace Kymaean.Application;

public interface IProductionWorldStateWriter
{
    ProductAccessResult<ProductionReplayProjection> ReplaceWorldCurrentState(
        ProductionId productionId,
        WorldCurrentState currentState);
}
