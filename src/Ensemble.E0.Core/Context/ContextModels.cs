using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Core.Context;

public static class E0ContextContracts
{
    public const string SchemaVersion = "ensemble.e0.context.v1";
    public const string CompositionContract = "ensemble.e0.context.full-authorized.v1";
    public const string RenderingContract = "ensemble.e0.context.render.v1";
    public const string ProductionBoundSchemaVersion = "ensemble.e0.context.v2";
    public const string ProductionBoundCompositionContract = "ensemble.e0.context.production-bound.v1";
    public const string AcceptedHistorySchemaVersion = "ensemble.e0.context.v3";
    public const string AcceptedHistoryCompositionContract =
        "ensemble.e0.context.production-bound.accepted-history.v1";
    public const string AcceptedHistoryRenderingContract = "ensemble.e0.context.render.v2";
}

public sealed class ContextParticipant
{
    internal ContextParticipant(CharacterId characterId, string displayName)
    {
        CharacterId = characterId;
        DisplayName = displayName;
    }

    public CharacterId CharacterId { get; }
    public string DisplayName { get; }
}

public sealed class ContextRecord
{
    internal ContextRecord(RecordId recordId, string text)
    {
        RecordId = recordId;
        Text = text;
    }

    public RecordId RecordId { get; }
    public string Text { get; }
}

public sealed class ContextRelationship
{
    internal ContextRelationship(RecordId recordId, CharacterId targetCharacterId, string text)
    {
        RecordId = recordId;
        TargetCharacterId = targetCharacterId;
        Text = text;
    }

    public RecordId RecordId { get; }
    public CharacterId TargetCharacterId { get; }
    public string Text { get; }
}

public sealed class ContextRecentPerformance
{
    internal ContextRecentPerformance(CharacterId sourceCharacterId, string visibleText)
    {
        SourceCharacterId = sourceCharacterId;
        VisibleText = visibleText;
    }

    public CharacterId SourceCharacterId { get; }
    public string VisibleText { get; }
}

public sealed class RenderedContext
{
    internal RenderedContext(
        string renderingContract,
        string trustedStateText,
        string recentPerformanceText,
        string opportunityText)
    {
        RenderingContract = renderingContract;
        TrustedStateText = trustedStateText;
        RecentPerformanceText = recentPerformanceText;
        OpportunityText = opportunityText;
    }

    public string RenderingContract { get; }
    public string TrustedStateText { get; }
    public string RecentPerformanceText { get; }
    public string OpportunityText { get; }
}

public sealed class ContextPacket
{
    internal ContextPacket(
        ContextPacketId contextPacketId,
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
        ImmutableArray<ContextRecentPerformance> recentPerformances,
        string structuredContextHash,
        RenderedContext rendered,
        string renderedContextHash)
    {
        ContextPacketId = contextPacketId;
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
        StructuredContextHash = structuredContextHash;
        Rendered = rendered;
        RenderedContextHash = renderedContextHash;
    }

    public ContextPacketId ContextPacketId { get; }
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
    public string StructuredContextHash { get; }
    public RenderedContext Rendered { get; }
    public string RenderedContextHash { get; }
}

public sealed class ContextCompositionTrace
{
    internal ContextCompositionTrace(
        string compositionContract,
        string renderingContract,
        StateHash? sourceStateHash,
        ImmutableArray<RecordId> includedRecordIds,
        ImmutableArray<CharacterId> includedRosterCharacterIds,
        CharacterId opportunityCharacterId,
        string structuredContextHash,
        string renderedContextHash)
    {
        CompositionContract = compositionContract;
        RenderingContract = renderingContract;
        SourceStateHash = sourceStateHash;
        IncludedRecordIds = includedRecordIds;
        IncludedRosterCharacterIds = includedRosterCharacterIds;
        OpportunityCharacterId = opportunityCharacterId;
        StructuredContextHash = structuredContextHash;
        RenderedContextHash = renderedContextHash;
    }

    public string CompositionContract { get; }
    public string RenderingContract { get; }
    public StateHash? SourceStateHash { get; }
    public ImmutableArray<RecordId> IncludedRecordIds { get; }
    public ImmutableArray<CharacterId> IncludedRosterCharacterIds { get; }
    public CharacterId OpportunityCharacterId { get; }
    public string StructuredContextHash { get; }
    public string RenderedContextHash { get; }
}

public sealed class ContextCompositionEvaluation
{
    internal ContextCompositionEvaluation(ContextPacket packet, ContextCompositionTrace trace)
    {
        Packet = packet;
        Trace = trace;
    }

    public ContextPacket Packet { get; }
    public ContextCompositionTrace Trace { get; }
}

public sealed class ContextCompositionException : Exception
{
    public ContextCompositionException(string message)
        : base(message)
    {
    }

    public ContextCompositionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
