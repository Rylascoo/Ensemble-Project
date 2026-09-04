using System.Collections.Immutable;
using System.Text;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Context;

public static class ContextPacketCanonicalizer
{
    public static byte[] SerializeStructured(ContextPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        ValidatePacketVersionShape(packet);

        var content = new ContextSemanticContent(
            packet.SchemaVersion,
            packet.CompositionContract,
            packet.SourceStateHash,
            packet.SceneId,
            packet.SubjectCharacterId,
            packet.OpportunityCharacterId,
            packet.Roster,
            packet.SceneState,
            packet.Pressures,
            packet.Constitution,
            packet.Disposition,
            packet.Circumstance,
            packet.Observations,
            packet.Knowledge,
            packet.Beliefs,
            packet.Suspicions,
            packet.Memories,
            packet.Goals,
            packet.Relationships,
            packet.RecentPerformances);

        return SerializeStructured(content);
    }

    public static byte[] SerializeRendered(RenderedContext rendered)
    {
        ArgumentNullException.ThrowIfNull(rendered);

        try
        {
            var builder = new StringBuilder(capacity: rendered.TrustedStateText.Length + 256);
            builder.Append('{');

            CanonicalJson.AppendPropertyName(builder, "renderingContract");
            CanonicalJson.AppendString(builder, rendered.RenderingContract);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "trustedStateText");
            CanonicalJson.AppendString(builder, rendered.TrustedStateText);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "recentPerformanceText");
            CanonicalJson.AppendString(builder, rendered.RecentPerformanceText);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "opportunityText");
            CanonicalJson.AppendString(builder, rendered.OpportunityText);

            builder.Append('}');
            return CanonicalJson.EncodeUtf8(builder.ToString());
        }
        catch (CanonicalJsonException exception)
        {
            throw new ContextCompositionException(
                $"Rendered Context canonicalization failed: {exception.Message}",
                exception);
        }
    }

    internal static byte[] SerializeStructured(ContextSemanticContent content)
    {
        ArgumentNullException.ThrowIfNull(content);
        var version = ValidateSemanticVersionShape(content);

        try
        {
            var builder = new StringBuilder(capacity: 8 * 1024);
            builder.Append('{');

            AppendStringProperty(builder, "schemaVersion", content.SchemaVersion);
            builder.Append(',');
            AppendStringProperty(builder, "compositionContract", content.CompositionContract);
            builder.Append(',');

            if (version is ContextSemanticVersion.V2 or ContextSemanticVersion.V3)
            {
                AppendStringProperty(
                    builder,
                    "sourceStateHash",
                    content.SourceStateHash!.Value.Value);
                builder.Append(',');
            }

            AppendStringProperty(builder, "sceneId", content.SceneId.Value);
            builder.Append(',');
            AppendStringProperty(builder, "subjectCharacterId", content.SubjectCharacterId.Value);
            builder.Append(',');
            AppendStringProperty(builder, "opportunityCharacterId", content.OpportunityCharacterId.Value);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "roster");
            AppendParticipants(builder, content.Roster);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "sceneState");
            AppendRecords(builder, content.SceneState);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "pressures");
            AppendRecords(builder, content.Pressures);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "constitution");
            AppendRecords(builder, content.Constitution);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "disposition");
            AppendRecords(builder, content.Disposition);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "circumstance");
            AppendRecords(builder, content.Circumstance);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "observations");
            AppendRecords(builder, content.Observations);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "knowledge");
            AppendRecords(builder, content.Knowledge);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "beliefs");
            AppendRecords(builder, content.Beliefs);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "suspicions");
            AppendRecords(builder, content.Suspicions);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "memories");
            AppendRecords(builder, content.Memories);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "goals");
            AppendRecords(builder, content.Goals);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "relationships");
            AppendRelationships(builder, content.Relationships);
            builder.Append(',');

            CanonicalJson.AppendPropertyName(builder, "recentPerformances");
            AppendRecentPerformances(builder, content.RecentPerformances);

            builder.Append('}');
            return CanonicalJson.EncodeUtf8(builder.ToString());
        }
        catch (CanonicalJsonException exception)
        {
            throw new ContextCompositionException(
                $"Structured Context canonicalization failed: {exception.Message}",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new ContextCompositionException(
                "Structured Context source-state identity is invalid.",
                exception);
        }
    }

    private static void ValidatePacketVersionShape(ContextPacket packet)
    {
        if (packet.Rendered is null)
        {
            throw new ContextCompositionException(
                "Context packet rendered content is required.");
        }

        var version = ValidateSemanticVersionShape(new ContextSemanticContent(
            packet.SchemaVersion,
            packet.CompositionContract,
            packet.SourceStateHash,
            packet.SceneId,
            packet.SubjectCharacterId,
            packet.OpportunityCharacterId,
            packet.Roster,
            packet.SceneState,
            packet.Pressures,
            packet.Constitution,
            packet.Disposition,
            packet.Circumstance,
            packet.Observations,
            packet.Knowledge,
            packet.Beliefs,
            packet.Suspicions,
            packet.Memories,
            packet.Goals,
            packet.Relationships,
            packet.RecentPerformances));

        if (version is ContextSemanticVersion.V1 or ContextSemanticVersion.V2)
        {
            if (!string.Equals(
                    packet.Rendered.RenderingContract,
                    E0ContextContracts.RenderingContract,
                    StringComparison.Ordinal) ||
                !string.Equals(
                    packet.Rendered.RecentPerformanceText,
                    string.Empty,
                    StringComparison.Ordinal))
            {
                throw new ContextCompositionException(
                    "Context packet historical rendering shape is unsupported.");
            }

            return;
        }

        if (!string.Equals(
                packet.Rendered.RenderingContract,
                E0ContextContracts.AcceptedHistoryRenderingContract,
                StringComparison.Ordinal) ||
            string.IsNullOrEmpty(packet.Rendered.RecentPerformanceText))
        {
            throw new ContextCompositionException(
                "Context packet accepted-history rendering shape is unsupported.");
        }
    }

    private static ContextSemanticVersion ValidateSemanticVersionShape(ContextSemanticContent content)
    {
        ValidateRecentPerformances(content.RecentPerformances);

        var isV1 = string.Equals(
                content.SchemaVersion,
                E0ContextContracts.SchemaVersion,
                StringComparison.Ordinal) &&
            string.Equals(
                content.CompositionContract,
                E0ContextContracts.CompositionContract,
                StringComparison.Ordinal);
        var isV2 = string.Equals(
                content.SchemaVersion,
                E0ContextContracts.ProductionBoundSchemaVersion,
                StringComparison.Ordinal) &&
            string.Equals(
                content.CompositionContract,
                E0ContextContracts.ProductionBoundCompositionContract,
                StringComparison.Ordinal);
        var isV3 = string.Equals(
                content.SchemaVersion,
                E0ContextContracts.AcceptedHistorySchemaVersion,
                StringComparison.Ordinal) &&
            string.Equals(
                content.CompositionContract,
                E0ContextContracts.AcceptedHistoryCompositionContract,
                StringComparison.Ordinal);

        if (isV1)
        {
            if (content.SourceStateHash.HasValue || content.RecentPerformances.Length != 0)
            {
                throw new ContextCompositionException(
                    "Context v1 cannot carry Production source identity or accepted Performance history.");
            }

            return ContextSemanticVersion.V1;
        }

        if (isV2)
        {
            ValidateProductionStateHash(content.SourceStateHash, "Production-bound Context v2");
            if (content.RecentPerformances.Length != 0)
            {
                throw new ContextCompositionException(
                    "Production-bound Context v2 cannot carry accepted Performance history.");
            }

            return ContextSemanticVersion.V2;
        }

        if (isV3)
        {
            ValidateProductionStateHash(content.SourceStateHash, "Accepted-history Context v3");
            if (content.RecentPerformances.Length == 0)
            {
                throw new ContextCompositionException(
                    "Accepted-history Context v3 requires nonempty recent Performance history.");
            }

            return ContextSemanticVersion.V3;
        }

        throw new ContextCompositionException(
            "Context schema/composition contract combination is unsupported.");
    }

    private static void ValidateProductionStateHash(StateHash? stateHash, string contextName)
    {
        if (!stateHash.HasValue)
        {
            throw new ContextCompositionException(
                $"{contextName} requires source-state identity.");
        }

        try
        {
            _ = stateHash.Value.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new ContextCompositionException(
                $"{contextName} source-state identity is uninitialized.",
                exception);
        }
    }

    private static void ValidateRecentPerformances(
        ImmutableArray<ContextRecentPerformance> recentPerformances)
    {
        if (recentPerformances.IsDefault)
        {
            throw new ContextCompositionException(
                "Context recent Performance history is uninitialized.");
        }

        foreach (var performance in recentPerformances)
        {
            if (performance is null)
            {
                throw new ContextCompositionException(
                    "Context recent Performance history contains an invalid item.");
            }

            try
            {
                _ = performance.SourceCharacterId.Value;
            }
            catch (InvalidOperationException exception)
            {
                throw new ContextCompositionException(
                    "Context recent Performance source Character is uninitialized.",
                    exception);
            }

            if (CharacterLegibleTextInvariants.Validate(performance.VisibleText) !=
                CharacterLegibleTextFailure.None)
            {
                throw new ContextCompositionException(
                    "Context recent Performance text is invalid.");
            }
        }
    }

    private static void AppendStringProperty(StringBuilder builder, string name, string value)
    {
        CanonicalJson.AppendPropertyName(builder, name);
        CanonicalJson.AppendString(builder, value);
    }

    private static void AppendParticipants(
        StringBuilder builder,
        ImmutableArray<ContextParticipant> participants)
    {
        builder.Append('[');
        var first = true;
        foreach (var participant in participants.OrderBy(
                     value => value.CharacterId.Value,
                     StringComparer.Ordinal))
        {
            CanonicalJson.AppendSeparator(builder, ref first);
            builder.Append('{');
            AppendStringProperty(builder, "characterId", participant.CharacterId.Value);
            builder.Append(',');
            AppendStringProperty(builder, "displayName", participant.DisplayName);
            builder.Append('}');
        }

        builder.Append(']');
    }

    private static void AppendRecords(StringBuilder builder, ImmutableArray<ContextRecord> records)
    {
        builder.Append('[');
        var first = true;
        foreach (var record in records.OrderBy(value => value.RecordId.Value, StringComparer.Ordinal))
        {
            CanonicalJson.AppendSeparator(builder, ref first);
            builder.Append('{');
            AppendStringProperty(builder, "recordId", record.RecordId.Value);
            builder.Append(',');
            AppendStringProperty(builder, "text", record.Text);
            builder.Append('}');
        }

        builder.Append(']');
    }

    private static void AppendRelationships(
        StringBuilder builder,
        ImmutableArray<ContextRelationship> relationships)
    {
        builder.Append('[');
        var first = true;
        foreach (var relationship in relationships.OrderBy(
                     value => value.RecordId.Value,
                     StringComparer.Ordinal))
        {
            CanonicalJson.AppendSeparator(builder, ref first);
            builder.Append('{');
            AppendStringProperty(builder, "recordId", relationship.RecordId.Value);
            builder.Append(',');
            AppendStringProperty(
                builder,
                "targetCharacterId",
                relationship.TargetCharacterId.Value);
            builder.Append(',');
            AppendStringProperty(builder, "text", relationship.Text);
            builder.Append('}');
        }

        builder.Append(']');
    }

    private static void AppendRecentPerformances(
        StringBuilder builder,
        ImmutableArray<ContextRecentPerformance> recentPerformances)
    {
        builder.Append('[');
        var first = true;
        foreach (var performance in recentPerformances)
        {
            CanonicalJson.AppendSeparator(builder, ref first);
            builder.Append('{');
            AppendStringProperty(
                builder,
                "sourceCharacterId",
                performance.SourceCharacterId.Value);
            builder.Append(',');
            AppendStringProperty(builder, "visibleText", performance.VisibleText);
            builder.Append('}');
        }

        builder.Append(']');
    }

    private enum ContextSemanticVersion
    {
        V1,
        V2,
        V3
    }
}

internal sealed class ContextSemanticContent
{
    public ContextSemanticContent(
        string schemaVersion,
        string compositionContract,
        StateHash? sourceStateHash,
        SceneId sceneId,
        CharacterId subjectCharacterId,
        CharacterId opportunityCharacterId,
        ImmutableArray<ContextParticipant> roster,
        ImmutableArray<ContextRecord> sceneState,
        ImmutableArray<ContextRecord> pressures,
        ImmutableArray<ContextRecord> constitution,
        ImmutableArray<ContextRecord> disposition,
        ImmutableArray<ContextRecord> circumstance,
        ImmutableArray<ContextRecord> observations,
        ImmutableArray<ContextRecord> knowledge,
        ImmutableArray<ContextRecord> beliefs,
        ImmutableArray<ContextRecord> suspicions,
        ImmutableArray<ContextRecord> memories,
        ImmutableArray<ContextRecord> goals,
        ImmutableArray<ContextRelationship> relationships,
        ImmutableArray<ContextRecentPerformance> recentPerformances)
    {
        SchemaVersion = schemaVersion;
        CompositionContract = compositionContract;
        SourceStateHash = sourceStateHash;
        SceneId = sceneId;
        SubjectCharacterId = subjectCharacterId;
        OpportunityCharacterId = opportunityCharacterId;
        Roster = roster;
        SceneState = sceneState;
        Pressures = pressures;
        Constitution = constitution;
        Disposition = disposition;
        Circumstance = circumstance;
        Observations = observations;
        Knowledge = knowledge;
        Beliefs = beliefs;
        Suspicions = suspicions;
        Memories = memories;
        Goals = goals;
        Relationships = relationships;
        RecentPerformances = recentPerformances;
    }

    public string SchemaVersion { get; }
    public string CompositionContract { get; }
    public StateHash? SourceStateHash { get; }
    public SceneId SceneId { get; }
    public CharacterId SubjectCharacterId { get; }
    public CharacterId OpportunityCharacterId { get; }
    public ImmutableArray<ContextParticipant> Roster { get; }
    public ImmutableArray<ContextRecord> SceneState { get; }
    public ImmutableArray<ContextRecord> Pressures { get; }
    public ImmutableArray<ContextRecord> Constitution { get; }
    public ImmutableArray<ContextRecord> Disposition { get; }
    public ImmutableArray<ContextRecord> Circumstance { get; }
    public ImmutableArray<ContextRecord> Observations { get; }
    public ImmutableArray<ContextRecord> Knowledge { get; }
    public ImmutableArray<ContextRecord> Beliefs { get; }
    public ImmutableArray<ContextRecord> Suspicions { get; }
    public ImmutableArray<ContextRecord> Memories { get; }
    public ImmutableArray<ContextRecord> Goals { get; }
    public ImmutableArray<ContextRelationship> Relationships { get; }
    public ImmutableArray<ContextRecentPerformance> RecentPerformances { get; }
}
