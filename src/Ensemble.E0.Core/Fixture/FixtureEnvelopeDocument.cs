using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ensemble.E0.Core.Fixture;

public sealed class FixtureEnvelopeDocument
{
    [JsonPropertyName("fixture")]
    public FixtureMetadataDocument? Fixture { get; init; }

    [JsonPropertyName("chronology")]
    public JsonElement Chronology { get; init; }

    [JsonPropertyName("historicalTruth")]
    public JsonElement HistoricalTruth { get; init; }

    [JsonPropertyName("unresolvedPropositions")]
    public JsonElement UnresolvedPropositions { get; init; }

    [JsonPropertyName("worldState")]
    public JsonElement WorldState { get; init; }

    [JsonPropertyName("scene")]
    public JsonElement Scene { get; init; }

    [JsonPropertyName("characters")]
    public JsonElement Characters { get; init; }

    [JsonPropertyName("pressures")]
    public JsonElement Pressures { get; init; }

    [JsonPropertyName("accessPolicies")]
    public JsonElement AccessPolicies { get; init; }

    [JsonPropertyName("observationContract")]
    public JsonElement ObservationContract { get; init; }

    [JsonPropertyName("initialOpportunity")]
    public JsonElement InitialOpportunity { get; init; }
}

public sealed class FixtureMetadataDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }
}
