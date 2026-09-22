namespace Kymaean.Application;

public interface IProductionCreator
{
    ProductAccessResult<ProductionCreation> CreateProduction(
        string productionName);
}

public sealed record ProductionCreation
{
    public ProductionCreation(
        ProductionId id,
        ProductionReplayProjection replay)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(replay);

        Id = id;
        Replay = replay;
    }

    public ProductionId Id { get; }

    public ProductionReplayProjection Replay { get; }

    public ProductionSummary Summary =>
        new(Id, Replay.ProductionName);
}
