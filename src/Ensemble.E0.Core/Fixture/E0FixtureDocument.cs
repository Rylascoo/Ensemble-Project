using System.Text.Json.Serialization;

namespace Ensemble.E0.Core.Fixture;

public sealed class E0FixtureDocument
{
    [JsonPropertyName("schemaVersion")]
    public string? SchemaVersion { get; init; }

    [JsonPropertyName("fixture")]
    public FixtureMetadataDocument? Fixture { get; init; }

    [JsonPropertyName("accessContract")]
    public string? AccessContract { get; init; }

    [JsonPropertyName("observationContract")]
    public string? ObservationContract { get; init; }

    [JsonPropertyName("chronology")]
    public string[]? Chronology { get; init; }

    [JsonPropertyName("historicalTruth")]
    public FixtureRecordDocument[]? HistoricalTruth { get; init; }

    [JsonPropertyName("unresolvedPropositions")]
    public FixtureRecordDocument[]? UnresolvedPropositions { get; init; }

    [JsonPropertyName("worldState")]
    public FixtureRecordDocument[]? WorldState { get; init; }

    [JsonPropertyName("scene")]
    public SceneDocument? Scene { get; init; }

    [JsonPropertyName("sceneState")]
    public FixtureRecordDocument[]? SceneState { get; init; }

    [JsonPropertyName("characters")]
    public CharacterDocument[]? Characters { get; init; }

    [JsonPropertyName("pressures")]
    public FixtureRecordDocument[]? Pressures { get; init; }

    [JsonPropertyName("initialOpportunity")]
    public string? InitialOpportunity { get; init; }
}

public sealed class FixtureMetadataDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }
}

public sealed class SceneDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("roster")]
    public string[]? Roster { get; init; }
}

public sealed class CharacterDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; init; }

    [JsonPropertyName("constitution")]
    public FixtureRecordDocument[]? Constitution { get; init; }

    [JsonPropertyName("disposition")]
    public FixtureRecordDocument[]? Disposition { get; init; }

    [JsonPropertyName("circumstance")]
    public FixtureRecordDocument[]? Circumstance { get; init; }

    [JsonPropertyName("observations")]
    public FixtureRecordDocument[]? Observations { get; init; }

    [JsonPropertyName("knowledge")]
    public FixtureRecordDocument[]? Knowledge { get; init; }

    [JsonPropertyName("beliefs")]
    public FixtureRecordDocument[]? Beliefs { get; init; }

    [JsonPropertyName("suspicions")]
    public FixtureRecordDocument[]? Suspicions { get; init; }

    [JsonPropertyName("memories")]
    public FixtureRecordDocument[]? Memories { get; init; }

    [JsonPropertyName("goals")]
    public FixtureRecordDocument[]? Goals { get; init; }

    [JsonPropertyName("relationships")]
    public RelationshipDocument[]? Relationships { get; init; }
}

public sealed class FixtureRecordDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("provenance")]
    public string[]? Provenance { get; init; }
}

public sealed class RelationshipDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("targetCharacterId")]
    public string? TargetCharacterId { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("provenance")]
    public string[]? Provenance { get; init; }
}
