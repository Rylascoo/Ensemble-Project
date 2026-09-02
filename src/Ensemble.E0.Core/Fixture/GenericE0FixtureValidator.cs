using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Fixture;

public static class GenericE0FixtureValidator
{
    private const int RequiredCharacterCount = 3;

    public static ValidatedFixture Validate(E0FixtureDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        RequireExact(document.SchemaVersion, E0FixtureDialect.SchemaVersion, "schemaVersion");
        RequireExact(document.AccessContract, E0FixtureDialect.AccessContract, "accessContract");
        RequireExact(document.ObservationContract, E0FixtureDialect.ObservationContract, "observationContract");

        var metadata = document.Fixture
            ?? throw new FixtureValidationException("Required 'fixture' metadata is missing.");

        var familyId = ParseFixtureFamilyId(metadata.Id);
        var version = ParseFixtureVersion(metadata.Version);
        var fixtureId = ParseFixtureId($"{familyId.Value}@{version.Value}");

        var allRecordIds = new HashSet<RecordId>();
        var provenanceOwners = new List<(RecordId Id, ImmutableArray<RecordId> Provenance, string Location)>();

        var historicalTruth = ConvertRecords(
            RequireArray(document.HistoricalTruth, "historicalTruth"),
            "historicalTruth",
            allRecordIds,
            provenanceOwners);

        var unresolvedPropositions = ConvertRecords(
            RequireArray(document.UnresolvedPropositions, "unresolvedPropositions"),
            "unresolvedPropositions",
            allRecordIds,
            provenanceOwners);

        var worldState = ConvertRecords(
            RequireArray(document.WorldState, "worldState"),
            "worldState",
            allRecordIds,
            provenanceOwners);

        var sceneState = ConvertRecords(
            RequireArray(document.SceneState, "sceneState"),
            "sceneState",
            allRecordIds,
            provenanceOwners);

        var pressures = ConvertRecords(
            RequireArray(document.Pressures, "pressures"),
            "pressures",
            allRecordIds,
            provenanceOwners);

        var relationshipOwners = new List<(CharacterId Source, ValidatedRelationship Relationship)>();
        var characterDocuments = RequireArray(document.Characters, "characters");
        if (characterDocuments.Length != RequiredCharacterCount)
        {
            throw new FixtureValidationException(
                $"Generic E0 fixtures require exactly {RequiredCharacterCount} Characters.");
        }

        var characterIds = new HashSet<CharacterId>();
        var characterBuilder = ImmutableArray.CreateBuilder<ValidatedCharacter>(RequiredCharacterCount);

        for (var index = 0; index < characterDocuments.Length; index++)
        {
            var character = ConvertCharacter(
                characterDocuments[index],
                $"characters[{index}]",
                allRecordIds,
                provenanceOwners,
                relationshipOwners);

            if (!characterIds.Add(character.Id))
            {
                throw new FixtureValidationException($"Duplicate Character ID '{character.Id}'.");
            }

            characterBuilder.Add(character);
        }

        ValidateRelationships(relationshipOwners, characterIds);
        ValidateProvenance(provenanceOwners, allRecordIds);

        var scene = ConvertScene(document.Scene, characterIds);
        var initialOpportunity = ParseCharacterId(document.InitialOpportunity, "initialOpportunity");
        if (!scene.Roster.Contains(initialOpportunity))
        {
            throw new FixtureValidationException(
                $"Initial opportunity Character '{initialOpportunity}' is not in the Scene roster.");
        }

        var chronology = ConvertChronology(document.Chronology, historicalTruth);

        return new ValidatedFixture(
            fixtureId,
            familyId,
            version,
            scene,
            initialOpportunity,
            chronology,
            historicalTruth,
            unresolvedPropositions,
            worldState,
            sceneState,
            characterBuilder.MoveToImmutable(),
            pressures);
    }

    private static ValidatedCharacter ConvertCharacter(
        CharacterDocument? document,
        string location,
        HashSet<RecordId> allRecordIds,
        List<(RecordId Id, ImmutableArray<RecordId> Provenance, string Location)> provenanceOwners,
        List<(CharacterId Source, ValidatedRelationship Relationship)> relationshipOwners)
    {
        if (document is null)
        {
            throw new FixtureValidationException($"'{location}' is null.");
        }

        var id = ParseCharacterId(document.Id, $"{location}.id");
        var displayName = CanonicalText.Required(document.DisplayName, $"{location}.displayName");

        var constitution = ConvertRecords(
            RequireArray(document.Constitution, $"{location}.constitution"),
            $"{location}.constitution",
            allRecordIds,
            provenanceOwners);

        var disposition = ConvertRecords(
            RequireArray(document.Disposition, $"{location}.disposition"),
            $"{location}.disposition",
            allRecordIds,
            provenanceOwners);

        var circumstance = ConvertRecords(
            RequireArray(document.Circumstance, $"{location}.circumstance"),
            $"{location}.circumstance",
            allRecordIds,
            provenanceOwners);

        var observations = ConvertRecords(
            RequireArray(document.Observations, $"{location}.observations"),
            $"{location}.observations",
            allRecordIds,
            provenanceOwners);

        var knowledge = ConvertRecords(
            RequireArray(document.Knowledge, $"{location}.knowledge"),
            $"{location}.knowledge",
            allRecordIds,
            provenanceOwners);

        var beliefs = ConvertRecords(
            RequireArray(document.Beliefs, $"{location}.beliefs"),
            $"{location}.beliefs",
            allRecordIds,
            provenanceOwners);

        var suspicions = ConvertRecords(
            RequireArray(document.Suspicions, $"{location}.suspicions"),
            $"{location}.suspicions",
            allRecordIds,
            provenanceOwners);

        var memories = ConvertRecords(
            RequireArray(document.Memories, $"{location}.memories"),
            $"{location}.memories",
            allRecordIds,
            provenanceOwners);

        var goals = ConvertRecords(
            RequireArray(document.Goals, $"{location}.goals"),
            $"{location}.goals",
            allRecordIds,
            provenanceOwners);

        var relationshipDocuments = RequireArray(document.Relationships, $"{location}.relationships");
        var relationships = ImmutableArray.CreateBuilder<ValidatedRelationship>(relationshipDocuments.Length);

        for (var index = 0; index < relationshipDocuments.Length; index++)
        {
            var relationshipLocation = $"{location}.relationships[{index}]";
            var relationship = ConvertRelationship(
                relationshipDocuments[index],
                relationshipLocation,
                allRecordIds,
                provenanceOwners);

            relationshipOwners.Add((id, relationship));
            relationships.Add(relationship);
        }

        return new ValidatedCharacter(
            id,
            displayName,
            constitution,
            disposition,
            circumstance,
            observations,
            knowledge,
            beliefs,
            suspicions,
            memories,
            goals,
            relationships.MoveToImmutable());
    }

    private static ImmutableArray<ValidatedRecord> ConvertRecords(
        FixtureRecordDocument?[] documents,
        string location,
        HashSet<RecordId> allRecordIds,
        List<(RecordId Id, ImmutableArray<RecordId> Provenance, string Location)> provenanceOwners)
    {
        var builder = ImmutableArray.CreateBuilder<ValidatedRecord>(documents.Length);

        for (var index = 0; index < documents.Length; index++)
        {
            var recordLocation = $"{location}[{index}]";
            var record = ConvertRecord(documents[index], recordLocation, allRecordIds);
            provenanceOwners.Add((record.Id, record.Provenance, recordLocation));
            builder.Add(record);
        }

        return builder.MoveToImmutable();
    }

    private static ValidatedRecord ConvertRecord(
        FixtureRecordDocument? document,
        string location,
        HashSet<RecordId> allRecordIds)
    {
        if (document is null)
        {
            throw new FixtureValidationException($"'{location}' is null.");
        }

        var id = ParseRecordId(document.Id, $"{location}.id");
        RegisterRecordId(id, location, allRecordIds);

        return new ValidatedRecord(
            id,
            CanonicalText.Required(document.Text, $"{location}.text"),
            ConvertProvenance(document.Provenance, $"{location}.provenance"));
    }

    private static ValidatedRelationship ConvertRelationship(
        RelationshipDocument? document,
        string location,
        HashSet<RecordId> allRecordIds,
        List<(RecordId Id, ImmutableArray<RecordId> Provenance, string Location)> provenanceOwners)
    {
        if (document is null)
        {
            throw new FixtureValidationException($"'{location}' is null.");
        }

        var id = ParseRecordId(document.Id, $"{location}.id");
        RegisterRecordId(id, location, allRecordIds);

        var relationship = new ValidatedRelationship(
            id,
            ParseCharacterId(document.TargetCharacterId, $"{location}.targetCharacterId"),
            CanonicalText.Required(document.Text, $"{location}.text"),
            ConvertProvenance(document.Provenance, $"{location}.provenance"));

        provenanceOwners.Add((relationship.Id, relationship.Provenance, location));
        return relationship;
    }

    private static ValidatedScene ConvertScene(SceneDocument? document, HashSet<CharacterId> characterIds)
    {
        if (document is null)
        {
            throw new FixtureValidationException("Required 'scene' section is missing.");
        }

        var id = ParseSceneId(document.Id, "scene.id");
        var rosterDocuments = RequireArray(document.Roster, "scene.roster");
        if (rosterDocuments.Length != RequiredCharacterCount)
        {
            throw new FixtureValidationException(
                $"Generic E0 Scene roster must contain exactly {RequiredCharacterCount} Characters.");
        }

        var rosterIds = new HashSet<CharacterId>();
        var rosterBuilder = ImmutableArray.CreateBuilder<CharacterId>(RequiredCharacterCount);

        for (var index = 0; index < rosterDocuments.Length; index++)
        {
            var rosterId = ParseCharacterId(rosterDocuments[index], $"scene.roster[{index}]");
            if (!rosterIds.Add(rosterId))
            {
                throw new FixtureValidationException($"Duplicate Scene roster Character ID '{rosterId}'.");
            }

            if (!characterIds.Contains(rosterId))
            {
                throw new FixtureValidationException(
                    $"Scene roster Character '{rosterId}' does not exist in the fixture Character set.");
            }

            rosterBuilder.Add(rosterId);
        }

        if (!rosterIds.SetEquals(characterIds))
        {
            throw new FixtureValidationException("Scene roster must contain exactly the fixture Character set.");
        }

        return new ValidatedScene(id, rosterBuilder.MoveToImmutable());
    }

    private static ImmutableArray<RecordId> ConvertChronology(
        string[]? chronologyDocuments,
        ImmutableArray<ValidatedRecord> historicalTruth)
    {
        var chronology = RequireArray(chronologyDocuments, "chronology");
        var historicalTruthIds = historicalTruth.Select(record => record.Id).ToHashSet();
        var seen = new HashSet<RecordId>();
        var builder = ImmutableArray.CreateBuilder<RecordId>(chronology.Length);

        for (var index = 0; index < chronology.Length; index++)
        {
            var id = ParseRecordId(chronology[index], $"chronology[{index}]");
            if (!seen.Add(id))
            {
                throw new FixtureValidationException($"Chronology contains duplicate record '{id}'.");
            }

            if (!historicalTruthIds.Contains(id))
            {
                throw new FixtureValidationException(
                    $"Chronology record '{id}' is not a HistoricalTruth record.");
            }

            builder.Add(id);
        }

        return builder.MoveToImmutable();
    }

    private static ImmutableArray<RecordId> ConvertProvenance(string[]? provenanceDocuments, string location)
    {
        var provenance = RequireArray(provenanceDocuments, location);
        var seen = new HashSet<RecordId>();
        var builder = ImmutableArray.CreateBuilder<RecordId>(provenance.Length);

        for (var index = 0; index < provenance.Length; index++)
        {
            var id = ParseRecordId(provenance[index], $"{location}[{index}]");
            if (!seen.Add(id))
            {
                throw new FixtureValidationException($"'{location}' contains duplicate record '{id}'.");
            }

            builder.Add(id);
        }

        return builder.MoveToImmutable();
    }

    private static void ValidateRelationships(
        IEnumerable<(CharacterId Source, ValidatedRelationship Relationship)> relationships,
        HashSet<CharacterId> characterIds)
    {
        foreach (var (source, relationship) in relationships)
        {
            if (!characterIds.Contains(relationship.TargetCharacterId))
            {
                throw new FixtureValidationException(
                    $"Relationship '{relationship.Id}' targets unknown Character '{relationship.TargetCharacterId}'.");
            }

            if (relationship.TargetCharacterId == source)
            {
                throw new FixtureValidationException(
                    $"Relationship '{relationship.Id}' may not target its owning Character '{source}'.");
            }
        }
    }

    private static void ValidateProvenance(
        IEnumerable<(RecordId Id, ImmutableArray<RecordId> Provenance, string Location)> provenanceOwners,
        HashSet<RecordId> allRecordIds)
    {
        foreach (var (id, provenance, location) in provenanceOwners)
        {
            foreach (var sourceId in provenance)
            {
                if (sourceId == id)
                {
                    throw new FixtureValidationException($"'{location}' may not cite itself as provenance.");
                }

                if (!allRecordIds.Contains(sourceId))
                {
                    throw new FixtureValidationException(
                        $"'{location}' cites unknown provenance record '{sourceId}'.");
                }
            }
        }
    }

    private static void RegisterRecordId(RecordId id, string location, HashSet<RecordId> allRecordIds)
    {
        if (!allRecordIds.Add(id))
        {
            throw new FixtureValidationException(
                $"Duplicate Record ID '{id}' encountered at '{location}'. Record IDs are fixture-global.");
        }
    }

    private static T[] RequireArray<T>(T[]? values, string fieldName) =>
        values ?? throw new FixtureValidationException($"Required array '{fieldName}' is missing.");

    private static void RequireExact(string? actual, string expected, string fieldName)
    {
        if (!string.Equals(actual, expected, StringComparison.Ordinal))
        {
            throw new FixtureValidationException(
                $"'{fieldName}' must be exactly '{expected}'.");
        }
    }

    private static FixtureFamilyId ParseFixtureFamilyId(string? value)
    {
        try
        {
            return FixtureFamilyId.From(value ?? string.Empty);
        }
        catch (ArgumentException exception)
        {
            throw new FixtureValidationException("Fixture metadata 'id' is not a valid fixture family ID.", exception);
        }
    }

    private static FixtureVersion ParseFixtureVersion(string? value)
    {
        try
        {
            return FixtureVersion.From(value ?? string.Empty);
        }
        catch (ArgumentException exception)
        {
            throw new FixtureValidationException("Fixture metadata 'version' is not valid.", exception);
        }
    }

    private static FixtureId ParseFixtureId(string value)
    {
        try
        {
            return FixtureId.From(value);
        }
        catch (ArgumentException exception)
        {
            throw new FixtureValidationException("Derived authoritative FixtureId is invalid.", exception);
        }
    }

    private static SceneId ParseSceneId(string? value, string fieldName)
    {
        try
        {
            return SceneId.From(value ?? string.Empty);
        }
        catch (ArgumentException exception)
        {
            throw new FixtureValidationException($"'{fieldName}' is not a valid Scene ID.", exception);
        }
    }

    private static CharacterId ParseCharacterId(string? value, string fieldName)
    {
        try
        {
            return CharacterId.From(value ?? string.Empty);
        }
        catch (ArgumentException exception)
        {
            throw new FixtureValidationException($"'{fieldName}' is not a valid Character ID.", exception);
        }
    }

    private static RecordId ParseRecordId(string? value, string fieldName)
    {
        try
        {
            return RecordId.From(value ?? string.Empty);
        }
        catch (ArgumentException exception)
        {
            throw new FixtureValidationException($"'{fieldName}' is not a valid Record ID.", exception);
        }
    }
}
