using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed record ScenePresentationRow(
    SceneId Id,
    string? Code,
    IReadOnlyList<CharacterPresentationRow> Roster)
{
    public bool CanInspect => Code is not null;
    public string CodeLabel => Code is null ? "Scene code unavailable" : $"Scene code {Code}";
    public bool IsEmpty => Roster.Count == 0;
    public string RosterSummary => IsEmpty
        ? "No Characters in this initial roster."
        : $"Initial roster: {string.Join("; ", Roster.Take(3).Select(character => character.AccessibleName))}" +
          (Roster.Count > 3 ? $"; +{Roster.Count - 3} more" : string.Empty);
    public string InspectName => $"Inspect initial roster for {CodeLabel}";
    public string DetailHeadingName => $"Initial roster. {CodeLabel}";

    public static IReadOnlyList<ScenePresentationRow> Build(
        ProductionReplayProjection replay,
        IReadOnlyList<CharacterPresentationRow> cast)
    {
        var codes = PresentationIdentityCode.BuildSceneCodes(
            replay.ProductionScenes.Scenes.Select(scene => scene.Id.Value).ToArray());
        var characters = cast.ToDictionary(character => character.Id);
        return replay.ProductionScenes.Scenes.Select(scene => new ScenePresentationRow(
            scene.Id, codes[scene.Id.Value], scene.InitialRoster.CharacterIds
                .Select(id => characters[id]).ToArray())).ToArray();
    }
}
