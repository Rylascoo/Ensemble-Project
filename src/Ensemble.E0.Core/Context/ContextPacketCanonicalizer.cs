using System.Collections.Immutable;
using System.Text;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Context;

public static class ContextPacketCanonicalizer
{
    public static byte[] SerializeStructured(ContextPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);

        var content = new ContextSemanticContent(
            packet.SchemaVersion,
            packet.CompositionContract,
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
            packet.Relationships);

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

        try
        {
            var builder = new StringBuilder(capacity: 8 * 1024);
            builder.Append('{');

            AppendStringProperty(builder, "schemaVersion", content.SchemaVersion);
            builder.Append(',');
            AppendStringProperty(builder, "compositionContract", content.CompositionContract);
            builder.Append(',');
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
            builder.Append("[]");

            builder.Append('}');
            return CanonicalJson.EncodeUtf8(builder.ToString());
        }
        catch (CanonicalJsonException exception)
        {
            throw new ContextCompositionException(
                $"Structured Context canonicalization failed: {exception.Message}",
                exception);
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
}

internal sealed class ContextSemanticContent
{
    public ContextSemanticContent(
        string schemaVersion,
        string compositionContract,
        Ensemble.E0.Core.Domain.SceneId sceneId,
        Ensemble.E0.Core.Domain.CharacterId subjectCharacterId,
        Ensemble.E0.Core.Domain.CharacterId opportunityCharacterId,
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
        ImmutableArray<ContextRelationship> relationships)
    {
        SchemaVersion = schemaVersion;
        CompositionContract = compositionContract;
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
    }

    public string SchemaVersion { get; }
    public string CompositionContract { get; }
    public Ensemble.E0.Core.Domain.SceneId SceneId { get; }
    public Ensemble.E0.Core.Domain.CharacterId SubjectCharacterId { get; }
    public Ensemble.E0.Core.Domain.CharacterId OpportunityCharacterId { get; }
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
}
