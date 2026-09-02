using System.Text.Json;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Fixture;

public static class FixtureEnvelopeValidator
{
    public static FixtureId Validate(FixtureEnvelopeDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var metadata = document.Fixture
            ?? throw new FixtureValidationException("Required 'fixture' metadata is missing.");

        if (string.IsNullOrWhiteSpace(metadata.Id))
        {
            throw new FixtureValidationException("Fixture metadata 'id' is required.");
        }

        if (string.IsNullOrWhiteSpace(metadata.Version))
        {
            throw new FixtureValidationException("Fixture metadata 'version' is required.");
        }

        EnsurePresent(document.Chronology, "chronology");
        EnsurePresent(document.HistoricalTruth, "historicalTruth");
        EnsurePresent(document.UnresolvedPropositions, "unresolvedPropositions");
        EnsurePresent(document.WorldState, "worldState");
        EnsurePresent(document.Scene, "scene");
        EnsurePresent(document.Characters, "characters");
        EnsurePresent(document.Pressures, "pressures");
        EnsurePresent(document.AccessPolicies, "accessPolicies");
        EnsurePresent(document.ObservationContract, "observationContract");
        EnsurePresent(document.InitialOpportunity, "initialOpportunity");

        try
        {
            return FixtureId.From(metadata.Id);
        }
        catch (ArgumentException exception)
        {
            throw new FixtureValidationException("Fixture metadata 'id' is not a valid canonical ID.", exception);
        }
    }

    private static void EnsurePresent(JsonElement element, string propertyName)
    {
        if (element.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            throw new FixtureValidationException($"Required '{propertyName}' section is missing.");
        }
    }
}
