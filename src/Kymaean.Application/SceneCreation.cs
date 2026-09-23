namespace Kymaean.Application;

public interface IProductionSceneCreator
{
    ProductAccessResult<SceneCreation> EstablishScene(
        ProductionId productionId,
        SceneRoster initialRoster);
}

public sealed record SceneCreation
{
    public SceneCreation(
        EstablishedScene scene,
        ProductionReplayProjection replay)
    {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(replay);
        Scene = scene;
        Replay = replay;
    }

    public EstablishedScene Scene { get; }

    public ProductionReplayProjection Replay { get; }
}
