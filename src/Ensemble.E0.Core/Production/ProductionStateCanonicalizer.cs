using System.Security.Cryptography;
using System.Text;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Production;

internal static class ProductionStateCanonicalizer
{
    internal static StateHash ComputeGenesisHash(ProductionStateProjection projection)
    {
        var builder = new StringBuilder();
        builder.Append('{');
        var first = true;
        AppendStringProperty(
            builder,
            ref first,
            "hashContract",
            ProductionStateContracts.StateHashContractVersion);
        AppendStringProperty(builder, ref first, "kind", "genesis");
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "projection");
        AppendProjection(builder, projection);
        builder.Append('}');
        return Hash(builder);
    }

    internal static byte[] SerializeProjection(ProductionState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        var builder = new StringBuilder();
        AppendProjection(builder, state.Projection);
        return CanonicalJson.EncodeUtf8(builder.ToString());
    }

    internal static void AppendProjection(
        StringBuilder builder,
        ProductionStateProjection projection)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(projection);

        builder.Append('{');
        var first = true;
        AppendStringProperty(
            builder,
            ref first,
            "schemaVersion",
            ProductionStateContracts.StateContractVersion);

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "origin");
        builder.Append('{');
        var originFirst = true;
        AppendStringProperty(builder, ref originFirst, "fixtureId", projection.OriginFixtureId.Value);
        AppendStringProperty(builder, ref originFirst, "fixtureFamilyId", projection.OriginFixtureFamilyId.Value);
        AppendStringProperty(builder, ref originFirst, "fixtureVersion", projection.OriginFixtureVersion.Value);
        AppendStringProperty(builder, ref originFirst, "fixtureHash", projection.OriginFixtureHash);
        builder.Append('}');

        AppendStringProperty(builder, ref first, "sceneId", projection.SceneId.Value);

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "characters");
        builder.Append('[');
        for (var index = 0; index < projection.Characters.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            var character = projection.Characters[index];
            builder.Append('{');
            var characterFirst = true;
            AppendStringProperty(builder, ref characterFirst, "id", character.CharacterId.Value);
            AppendStringProperty(builder, ref characterFirst, "displayName", character.DisplayName);
            builder.Append('}');
        }

        builder.Append(']');

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "roster");
        builder.Append('[');
        for (var index = 0; index < projection.RosterCharacterIds.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            CanonicalJson.AppendString(builder, projection.RosterCharacterIds[index].Value);
        }

        builder.Append(']');

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "currentOpportunityCharacterId");
        if (projection.CurrentOpportunityCharacterId.HasValue)
        {
            CanonicalJson.AppendString(
                builder,
                projection.CurrentOpportunityCharacterId.Value.Value);
        }
        else
        {
            builder.Append("null");
        }

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "records");
        builder.Append('[');
        for (var index = 0; index < projection.Records.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            AppendRecord(builder, projection.Records[index]);
        }

        builder.Append(']');
        builder.Append('}');
    }

    private static void AppendRecord(StringBuilder builder, ProductionRecord record)
    {
        var subject = record switch
        {
            CharacterProductionRecord character => character.SubjectCharacterId.Value,
            RelationshipProductionRecord relationship => relationship.SubjectCharacterId.Value,
            _ => null
        };
        var target = record is RelationshipProductionRecord relationshipRecord
            ? relationshipRecord.TargetCharacterId.Value
            : null;

        builder.Append('{');
        var first = true;
        AppendStringProperty(builder, ref first, "id", record.RecordId.Value);
        AppendStringProperty(builder, ref first, "domain", ProductionRecordTokens.Domain(record.Domain));
        AppendStringProperty(builder, ref first, "lifecycle", ProductionRecordTokens.Lifecycle(record.Lifecycle));
        AppendStringProperty(builder, ref first, "protection", ProductionRecordTokens.Protection(record.Protection));
        AppendNullableStringProperty(builder, ref first, "subjectCharacterId", subject);
        AppendNullableStringProperty(builder, ref first, "targetCharacterId", target);
        AppendStringProperty(builder, ref first, "text", record.Text);

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "provenance");
        builder.Append('[');
        for (var index = 0; index < record.Provenance.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            CanonicalJson.AppendString(builder, record.Provenance[index].Value);
        }

        builder.Append(']');
        builder.Append('}');
    }

    private static StateHash Hash(StringBuilder builder)
    {
        var bytes = CanonicalJson.EncodeUtf8(builder.ToString());
        var digest = SHA256.HashData(bytes);
        return StateHash.Create(Convert.ToHexString(digest).ToLowerInvariant());
    }

    private static void AppendStringProperty(
        StringBuilder builder,
        ref bool first,
        string name,
        string value)
    {
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, name);
        CanonicalJson.AppendString(builder, value);
    }

    private static void AppendNullableStringProperty(
        StringBuilder builder,
        ref bool first,
        string name,
        string? value)
    {
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, name);
        if (value is null)
        {
            builder.Append("null");
        }
        else
        {
            CanonicalJson.AppendString(builder, value);
        }
    }
}

internal static class ProductionRecordTokens
{
    internal static string Domain(ProductionRecordDomain domain) =>
        domain switch
        {
            ProductionRecordDomain.HistoricalTruth => "historicalTruth",
            ProductionRecordDomain.UnresolvedProposition => "unresolvedProposition",
            ProductionRecordDomain.WorldState => "worldState",
            ProductionRecordDomain.SceneState => "sceneState",
            ProductionRecordDomain.CharacterConstitution => "characterConstitution",
            ProductionRecordDomain.CharacterDisposition => "characterDisposition",
            ProductionRecordDomain.CharacterCircumstance => "characterCircumstance",
            ProductionRecordDomain.CharacterObservation => "characterObservation",
            ProductionRecordDomain.CharacterKnowledge => "characterKnowledge",
            ProductionRecordDomain.CharacterBelief => "characterBelief",
            ProductionRecordDomain.CharacterSuspicion => "characterSuspicion",
            ProductionRecordDomain.CharacterMemory => "characterMemory",
            ProductionRecordDomain.CharacterGoal => "characterGoal",
            ProductionRecordDomain.CharacterClaim => "characterClaim",
            ProductionRecordDomain.Relationship => "relationship",
            ProductionRecordDomain.Pressure => "pressure",
            _ => throw new CanonicalJsonException(
                "Canonical Production record domain is invalid.")
        };

    internal static string Lifecycle(ProductionRecordLifecycle lifecycle) =>
        lifecycle switch
        {
            ProductionRecordLifecycle.Active => "active",
            ProductionRecordLifecycle.Inactive => "inactive",
            _ => throw new CanonicalJsonException(
                "Canonical Production record lifecycle is invalid.")
        };

    internal static string Protection(ProductionRecordProtection protection) =>
        protection switch
        {
            ProductionRecordProtection.None => "none",
            ProductionRecordProtection.SystemImmutable => "systemImmutable",
            ProductionRecordProtection.CreatorLocked => "creatorLocked",
            _ => throw new CanonicalJsonException(
                "Canonical Production record protection is invalid.")
        };
}
