namespace Kymaean.Application;

public sealed record ProductionReplayProjection
{
    public ProductionReplayProjection(string productionName)
        : this(productionName, WorldCurrentState.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);
        ArgumentNullException.ThrowIfNull(worldCurrentState);

        ProductionName = productionName;
        WorldCurrentState = worldCurrentState;
    }

    public string ProductionName { get; }

    public WorldCurrentState WorldCurrentState { get; }
}

public static class ProductionReplay
{
    public static ProductionReplayProjection Rebuild(
        IReadOnlyList<ProductionEvent> history)
    {
        ArgumentNullException.ThrowIfNull(history);

        ProductionReplayProjection? projection = null;
        for (var index = 0; index < history.Count; index++)
        {
            var productionEvent = history[index]
                ?? throw new InvalidOperationException(
                    $"Production history contains a null event at index {index}.");

            switch (productionEvent)
            {
                case ProductionCreatedEvent created when projection is null:
                    projection = new ProductionReplayProjection(
                        created.ProductionName);
                    break;

                case ProductionCreatedEvent:
                    throw new InvalidOperationException(
                        "Production history contains more than one creation event.");

                case CreatorReplacedWorldCurrentStateEvent
                    when projection is null:
                    throw new InvalidOperationException(
                        "World current state cannot be established before Production creation.");

                case CreatorReplacedWorldCurrentStateEvent replaced:
                    projection = new ProductionReplayProjection(
                        projection.ProductionName,
                        replaced.CurrentState);
                    break;

                default:
                    throw new NotSupportedException(
                        $"Production event type '{productionEvent.GetType().Name}' is not replayable.");
            }
        }

        return projection
            ?? throw new InvalidOperationException(
                "Production history does not contain a creation event.");
    }
}
