namespace Kymaean.Application;

public sealed record ProductionReplayProjection
{
    public ProductionReplayProjection(string productionName)
        : this(
            productionName,
            WorldCurrentState.Empty,
            ProductionCast.Empty,
            ProductionScenes.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState)
        : this(
            productionName,
            worldCurrentState,
            ProductionCast.Empty,
            ProductionScenes.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState,
        ProductionCast productionCast)
        : this(
            productionName,
            worldCurrentState,
            productionCast,
            ProductionScenes.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState,
        ProductionCast productionCast,
        ProductionScenes productionScenes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);
        ArgumentNullException.ThrowIfNull(worldCurrentState);
        ArgumentNullException.ThrowIfNull(productionCast);
        ArgumentNullException.ThrowIfNull(productionScenes);

        ProductionName = productionName;
        WorldCurrentState = worldCurrentState;
        ProductionCast = productionCast;
        ProductionScenes = productionScenes;
    }

    public string ProductionName { get; }

    public WorldCurrentState WorldCurrentState { get; }

    public ProductionCast ProductionCast { get; }

    public ProductionScenes ProductionScenes { get; }
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
                                    created.CharacterName))),
                        projection.ProductionScenes);
                    break;

                case CreatorEstablishedSceneEvent when projection is null:
                    throw new InvalidOperationException(
                        "A Scene cannot be established before Production creation.");

                case CreatorEstablishedSceneEvent established:
                    if (projection.ProductionScenes.Scenes.Any(
                            scene => scene.Id == established.SceneId))
                    {
                        throw new InvalidOperationException(
                            "Production history contains duplicate Scene identity.");
                    }

                    SceneRoster canonicalRoster;
                    try
                    {
                        canonicalRoster = SceneRoster.Canonicalize(
                            projection.ProductionCast,
                            established.InitialRoster.CharacterIds);
                    }
                    catch (ArgumentException exception)
                    {
                        throw new InvalidOperationException(
                            "A Scene roster contains a Character outside the Production Cast.",
                            exception);
                    }

                    projection = new ProductionReplayProjection(
                        projection.ProductionName,
                        projection.WorldCurrentState,
                        projection.ProductionCast,
                        new ProductionScenes(
                            projection.ProductionScenes.Scenes.Add(
                                new EstablishedScene(
                                    established.SceneId,
                                    canonicalRoster))));
                    break;

                case CreatorReplacedWorldCurrentStateEvent when projection is null:
                    throw new InvalidOperationException(
                        "World current state cannot be established before Production creation.");

                case CreatorReplacedWorldCurrentStateEvent replaced:
                    projection = new ProductionReplayProjection(
                        projection.ProductionName,
                        replaced.CurrentState,
                        projection.ProductionCast,
                        projection.ProductionScenes);
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
