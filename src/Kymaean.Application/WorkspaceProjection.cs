using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed record WorkspaceCharacter(string Id, string DisplayName);

public sealed record StudioProjection(
    string ProductionName,
    ImmutableArray<WorkspaceCharacter> Cast,
    int SceneCount);

public sealed record StageProjection(
    string SceneName,
    ImmutableArray<WorkspaceCharacter> PresentCharacters,
    ImmutableArray<string> SituationLines,
    string OpportunityCharacterId);

public sealed record ArchiveProjection(
    ImmutableArray<string> RecentHistory,
    int HistoricalEventCount);

public sealed record WorkspaceProjection(
    StudioProjection Studio,
    StageProjection Stage,
    ArchiveProjection Archive);
