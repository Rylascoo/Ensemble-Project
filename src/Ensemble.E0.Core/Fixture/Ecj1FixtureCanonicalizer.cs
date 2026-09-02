using System.Collections.Immutable;
using System.Text;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Fixture;

public static class Ecj1FixtureCanonicalizer
{
    public static byte[] Serialize(ValidatedFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        try
        {
            var builder = new StringBuilder(capacity: 16 * 1024);
            builder.Append('{');

            AppendPropertyName(builder, "schemaVersion");
            AppendString(builder, fixture.SchemaVersion);
            builder.Append(',');

            AppendPropertyName(builder, "fixture");
            builder.Append('{');
            AppendPropertyName(builder, "id");
            AppendString(builder, fixture.FamilyId.Value);
            builder.Append(',');
            AppendPropertyName(builder, "version");
            AppendString(builder, fixture.Version.Value);
            builder.Append('}');
            builder.Append(',');

            AppendPropertyName(builder, "accessContract");
            AppendString(builder, fixture.AccessContract);
            builder.Append(',');

            AppendPropertyName(builder, "observationContract");
            AppendString(builder, fixture.ObservationContract);
            builder.Append(',');

            AppendPropertyName(builder, "chronology");
            AppendRecordIds(builder, fixture.Chronology, preserveOrder: true);
            builder.Append(',');

            AppendPropertyName(builder, "historicalTruth");
            AppendRecords(builder, fixture.HistoricalTruth);
            builder.Append(',');

            AppendPropertyName(builder, "unresolvedPropositions");
            AppendRecords(builder, fixture.UnresolvedPropositions);
            builder.Append(',');

            AppendPropertyName(builder, "worldState");
            AppendRecords(builder, fixture.WorldState);
            builder.Append(',');

            AppendPropertyName(builder, "scene");
            builder.Append('{');
            AppendPropertyName(builder, "id");
            AppendString(builder, fixture.Scene.Id.Value);
            builder.Append(',');
            AppendPropertyName(builder, "roster");
            AppendCharacterIds(builder, fixture.Scene.Roster);
            builder.Append('}');
            builder.Append(',');

            AppendPropertyName(builder, "sceneState");
            AppendRecords(builder, fixture.SceneState);
            builder.Append(',');

            AppendPropertyName(builder, "characters");
            AppendCharacters(builder, fixture.Characters);
            builder.Append(',');

            AppendPropertyName(builder, "pressures");
            AppendRecords(builder, fixture.Pressures);
            builder.Append(',');

            AppendPropertyName(builder, "initialOpportunity");
            AppendString(builder, fixture.InitialOpportunity.Value);

            builder.Append('}');
            return CanonicalJson.EncodeUtf8(builder.ToString());
        }
        catch (CanonicalJsonException exception)
        {
            throw new FixtureValidationException(
                $"ECJ-1 canonicalization failed: {exception.Message}",
                exception);
        }
    }

    private static void AppendCharacters(StringBuilder builder, ImmutableArray<ValidatedCharacter> characters)
    {
        builder.Append('[');
        var first = true;
        foreach (var character in characters.OrderBy(value => value.Id.Value, StringComparer.Ordinal))
        {
            AppendSeparator(builder, ref first);
            AppendCharacter(builder, character);
        }

        builder.Append(']');
    }

    private static void AppendCharacter(StringBuilder builder, ValidatedCharacter character)
    {
        builder.Append('{');

        AppendPropertyName(builder, "id");
        AppendString(builder, character.Id.Value);
        builder.Append(',');

        AppendPropertyName(builder, "displayName");
        AppendString(builder, character.DisplayName);
        builder.Append(',');

        AppendPropertyName(builder, "constitution");
        AppendRecords(builder, character.Constitution);
        builder.Append(',');

        AppendPropertyName(builder, "disposition");
        AppendRecords(builder, character.Disposition);
        builder.Append(',');

        AppendPropertyName(builder, "circumstance");
        AppendRecords(builder, character.Circumstance);
        builder.Append(',');

        AppendPropertyName(builder, "observations");
        AppendRecords(builder, character.Observations);
        builder.Append(',');

        AppendPropertyName(builder, "knowledge");
        AppendRecords(builder, character.Knowledge);
        builder.Append(',');

        AppendPropertyName(builder, "beliefs");
        AppendRecords(builder, character.Beliefs);
        builder.Append(',');

        AppendPropertyName(builder, "suspicions");
        AppendRecords(builder, character.Suspicions);
        builder.Append(',');

        AppendPropertyName(builder, "memories");
        AppendRecords(builder, character.Memories);
        builder.Append(',');

        AppendPropertyName(builder, "goals");
        AppendRecords(builder, character.Goals);
        builder.Append(',');

        AppendPropertyName(builder, "relationships");
        AppendRelationships(builder, character.Relationships);

        builder.Append('}');
    }

    private static void AppendRecords(StringBuilder builder, ImmutableArray<ValidatedRecord> records)
    {
        builder.Append('[');
        var first = true;
        foreach (var record in records.OrderBy(value => value.Id.Value, StringComparer.Ordinal))
        {
            AppendSeparator(builder, ref first);
            AppendRecord(builder, record);
        }

        builder.Append(']');
    }

    private static void AppendRecord(StringBuilder builder, ValidatedRecord record)
    {
        builder.Append('{');

        AppendPropertyName(builder, "id");
        AppendString(builder, record.Id.Value);
        builder.Append(',');

        AppendPropertyName(builder, "text");
        AppendString(builder, record.Text);
        builder.Append(',');

        AppendPropertyName(builder, "provenance");
        AppendRecordIds(builder, record.Provenance, preserveOrder: false);

        builder.Append('}');
    }

    private static void AppendRelationships(StringBuilder builder, ImmutableArray<ValidatedRelationship> relationships)
    {
        builder.Append('[');
        var first = true;
        foreach (var relationship in relationships.OrderBy(value => value.Id.Value, StringComparer.Ordinal))
        {
            AppendSeparator(builder, ref first);
            AppendRelationship(builder, relationship);
        }

        builder.Append(']');
    }

    private static void AppendRelationship(StringBuilder builder, ValidatedRelationship relationship)
    {
        builder.Append('{');

        AppendPropertyName(builder, "id");
        AppendString(builder, relationship.Id.Value);
        builder.Append(',');

        AppendPropertyName(builder, "targetCharacterId");
        AppendString(builder, relationship.TargetCharacterId.Value);
        builder.Append(',');

        AppendPropertyName(builder, "text");
        AppendString(builder, relationship.Text);
        builder.Append(',');

        AppendPropertyName(builder, "provenance");
        AppendRecordIds(builder, relationship.Provenance, preserveOrder: false);

        builder.Append('}');
    }

    private static void AppendCharacterIds(StringBuilder builder, ImmutableArray<CharacterId> ids)
    {
        builder.Append('[');
        var first = true;
        foreach (var id in ids.OrderBy(value => value.Value, StringComparer.Ordinal))
        {
            AppendSeparator(builder, ref first);
            AppendString(builder, id.Value);
        }

        builder.Append(']');
    }

    private static void AppendRecordIds(StringBuilder builder, ImmutableArray<RecordId> ids, bool preserveOrder)
    {
        var values = preserveOrder
            ? ids.AsEnumerable()
            : ids.OrderBy(value => value.Value, StringComparer.Ordinal);

        builder.Append('[');
        var first = true;
        foreach (var id in values)
        {
            AppendSeparator(builder, ref first);
            AppendString(builder, id.Value);
        }

        builder.Append(']');
    }

    private static void AppendPropertyName(StringBuilder builder, string name) =>
        CanonicalJson.AppendPropertyName(builder, name);

    private static void AppendSeparator(StringBuilder builder, ref bool first) =>
        CanonicalJson.AppendSeparator(builder, ref first);

    private static void AppendString(StringBuilder builder, string value) =>
        CanonicalJson.AppendString(builder, value);
}
