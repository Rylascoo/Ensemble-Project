namespace Kymaean.Application;

public sealed record ProductionReplayProjection
{
    public ProductionReplayProjection(string productionName)
        : this(
            productionName,
            WorldCurrentState.Empty,
            ProductionCast.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState)
        : this(
            productionName,
            worldCurrentState,
            ProductionCast.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState,
        ProductionCast productionCast)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);
        ArgumentNullException.ThrowIfNull(worldCurrentState);
        ArgumentNullException.ThrowIfNull(productionCast);

        ProductionName = productionName;
        WorldCurrentState = worldCurrentState;
        ProductionCast = productionCast;
    }

    public string ProductionName { get; }

    public WorldCurrentState WorldCurrentState { get; }

    public ProductionCast ProductionCast { get; }
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

                case CharacterCreatedEvent when projection is null:
                    throw new InvalidOperationException(
                        "A Character cannot be established before Production creation.");

                case CharacterCreatedEvent created:
                    if (projection.ProductionCast.Characters.Any(
                            character => character.Id == created.CharacterId))
                    {
                        throw new InvalidOperationException(
                            "Production history contains duplicate Character identity.");
                    }

                    projection = new ProductionReplayProjection(
                        projection.ProductionName,
                        projection.WorldCurrentState,
                        new ProductionCast(
                            projection.ProductionCast.Characters.Add(
                                new CharacterSummary(
                                    created.CharacterId,
                                    created.CharacterName))));
                    break;

                case CreatorReplacedWorldCurrentStateEvent when projection is null:
                    throw new InvalidOperationException(
                        "World current state cannot be established before Production creation.");

                case CreatorReplacedWorldCurrentStateEvent replaced:
                    projection = new ProductionReplayProjection(
                        projection.ProductionName,
                        replaced.CurrentState,
                        projection.ProductionCast);
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
