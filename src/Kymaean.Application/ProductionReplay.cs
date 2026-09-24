using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed record ProductionReplayProjection
{
    public ProductionReplayProjection(string productionName)
        : this(
            productionName,
            WorldCurrentState.Empty,
            ProductionCast.Empty,
            ProductionScenes.Empty,
            AcceptedPerformanceHistory.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState)
        : this(
            productionName,
            worldCurrentState,
            ProductionCast.Empty,
            ProductionScenes.Empty,
            AcceptedPerformanceHistory.Empty)
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
            ProductionScenes.Empty,
            AcceptedPerformanceHistory.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState,
        ProductionCast productionCast,
        ProductionScenes productionScenes)
        : this(
            productionName,
            worldCurrentState,
            productionCast,
            productionScenes,
            AcceptedPerformanceHistory.Empty)
    {
    }

    public ProductionReplayProjection(
        string productionName,
        WorldCurrentState worldCurrentState,
        ProductionCast productionCast,
        ProductionScenes productionScenes,
        AcceptedPerformanceHistory acceptedPerformanceHistory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);
        ArgumentNullException.ThrowIfNull(worldCurrentState);
        ArgumentNullException.ThrowIfNull(productionCast);
        ArgumentNullException.ThrowIfNull(productionScenes);
        ArgumentNullException.ThrowIfNull(acceptedPerformanceHistory);

        foreach (var scene in productionScenes.Scenes)
        {
            SceneRoster canonicalRoster;
            try
            {
                canonicalRoster = SceneRoster.Canonicalize(
                    productionCast,
                    scene.InitialRoster.CharacterIds);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(
                    "Every Production Scene roster Character must exist in the Production Cast.",
                    nameof(productionScenes),
                    exception);
            }

            if (!canonicalRoster.Equals(scene.InitialRoster))
            {
                throw new ArgumentException(
                    "Production Scene rosters must follow Production Cast order.",
                    nameof(productionScenes));
            }
        }

        foreach (var performance in acceptedPerformanceHistory.Performances)
        {
            var scene = productionScenes.Scenes.SingleOrDefault(
                item => item.Id == performance.SceneId);
            if (scene is null ||
                !scene.InitialRoster.CharacterIds.Contains(performance.CharacterId))
            {
                throw new ArgumentException(
                    "Every accepted Performance must belong to an established Scene roster Character.",
                    nameof(acceptedPerformanceHistory));
            }
        }

        ProductionName = productionName;
        WorldCurrentState = worldCurrentState;
        ProductionCast = productionCast;
        ProductionScenes = productionScenes;
        AcceptedPerformanceHistory = acceptedPerformanceHistory;
    }

    public string ProductionName { get; }

    public WorldCurrentState WorldCurrentState { get; }

    public ProductionCast ProductionCast { get; }

    public ProductionScenes ProductionScenes { get; }

    public AcceptedPerformanceHistory AcceptedPerformanceHistory { get; }
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
                        projection.ProductionScenes,
                        projection.AcceptedPerformanceHistory);
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
                                    canonicalRoster))),
                        projection.AcceptedPerformanceHistory);
                    break;

                case AcceptedPerformanceCommittedEvent when projection is null:
                    throw new InvalidOperationException(
                        "A Performance cannot be accepted before Production creation.");

                case AcceptedPerformanceCommittedEvent committed:
                    var performance = committed.Performance;
                    var sourceScene = projection.ProductionScenes.Scenes
                        .SingleOrDefault(scene => scene.Id == performance.SceneId);
                    if (sourceScene is null ||
                        !sourceScene.InitialRoster.CharacterIds.Contains(
                            performance.CharacterId))
                    {
                        throw new InvalidOperationException(
                            "An accepted Performance must belong to an established Scene roster Character.");
                    }

                    projection = new ProductionReplayProjection(
                        projection.ProductionName,
                        projection.WorldCurrentState,
                        projection.ProductionCast,
                        projection.ProductionScenes,
                        new AcceptedPerformanceHistory(
                            projection.AcceptedPerformanceHistory.Performances.Add(
                                performance)));
                    break;

                case CreatorReplacedWorldCurrentStateEvent when projection is null:
                    throw new InvalidOperationException(
                        "World current state cannot be established before Production creation.");

                case CreatorReplacedWorldCurrentStateEvent replaced:
                    projection = new ProductionReplayProjection(
                        projection.ProductionName,
                        replaced.CurrentState,
                        projection.ProductionCast,
                        projection.ProductionScenes,
                        projection.AcceptedPerformanceHistory);
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
