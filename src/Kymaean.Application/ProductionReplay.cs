namespace Kymaean.Application;

public sealed record ProductionReplayProjection(string ProductionName);

public static class ProductionReplay
{
    public static ProductionReplayProjection Rebuild(IReadOnlyList<ProductionEvent> history)
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
                    projection = new ProductionReplayProjection(created.ProductionName);
                    break;
                case ProductionCreatedEvent:
                    throw new InvalidOperationException(
                        "Production history contains more than one creation event.");
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
