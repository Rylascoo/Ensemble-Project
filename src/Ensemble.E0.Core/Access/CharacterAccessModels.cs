using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Core.Access;

public sealed class SceneParticipant
{
    internal SceneParticipant(CharacterId characterId, string displayName)
    {
        CharacterId = characterId;
        DisplayName = displayName;
    }

    public CharacterId CharacterId { get; }
    public string DisplayName { get; }
}

public sealed class PermittedRecord
{
    internal PermittedRecord(RecordId recordId, string text)
    {
        RecordId = recordId;
        Text = text;
    }

    public RecordId RecordId { get; }
    public string Text { get; }
}

public sealed class PermittedRelationship
{
    internal PermittedRelationship(RecordId recordId, CharacterId targetCharacterId, string text)
    {
        RecordId = recordId;
        TargetCharacterId = targetCharacterId;
        Text = text;
    }

    public RecordId RecordId { get; }
    public CharacterId TargetCharacterId { get; }
    public string Text { get; }
}

public sealed class CharacterAccessProjection
{
    internal CharacterAccessProjection(
        StateHash? sourceStateHash,
        SceneId sceneId,
        CharacterId subjectCharacterId,
        ImmutableArray<SceneParticipant> roster,
        ImmutableArray<PermittedRecord> sceneState,
        ImmutableArray<PermittedRecord> pressures,
        ImmutableArray<PermittedRecord> constitution,
        ImmutableArray<PermittedRecord> disposition,
        ImmutableArray<PermittedRecord> circumstance,
        ImmutableArray<PermittedRecord> observations,
        ImmutableArray<PermittedRecord> knowledge,
        ImmutableArray<PermittedRecord> beliefs,
        ImmutableArray<PermittedRecord> suspicions,
        ImmutableArray<PermittedRecord> memories,
        ImmutableArray<PermittedRecord> goals,
        ImmutableArray<PermittedRelationship> relationships)
    {
        SourceStateHash = sourceStateHash;
        SceneId = sceneId;
        SubjectCharacterId = subjectCharacterId;
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

    public StateHash? SourceStateHash { get; }
    public SceneId SceneId { get; }
    public CharacterId SubjectCharacterId { get; }
    public ImmutableArray<SceneParticipant> Roster { get; }
    public ImmutableArray<PermittedRecord> SceneState { get; }
    public ImmutableArray<PermittedRecord> Pressures { get; }
    public ImmutableArray<PermittedRecord> Constitution { get; }
    public ImmutableArray<PermittedRecord> Disposition { get; }
    public ImmutableArray<PermittedRecord> Circumstance { get; }
    public ImmutableArray<PermittedRecord> Observations { get; }
    public ImmutableArray<PermittedRecord> Knowledge { get; }
    public ImmutableArray<PermittedRecord> Beliefs { get; }
    public ImmutableArray<PermittedRecord> Suspicions { get; }
    public ImmutableArray<PermittedRecord> Memories { get; }
    public ImmutableArray<PermittedRecord> Goals { get; }
    public ImmutableArray<PermittedRelationship> Relationships { get; }
}

public enum AccessDisposition
{
    Permit,
    Deny
}

public enum AccessReason
{
    OwnedBySubject,
    SharedSceneState,
    PublicPressure,
    ProductionAuthorityExcluded,
    OwnedByOtherCharacterExcluded,
    InactiveRecordExcluded,
    CharacterClaimDisclosureDeferred
}

public sealed class AccessDecision
{
    internal AccessDecision(RecordId recordId, AccessDisposition disposition, AccessReason reason)
    {
        RecordId = recordId;
        Disposition = disposition;
        Reason = reason;
    }

    public RecordId RecordId { get; }
    public AccessDisposition Disposition { get; }
    public AccessReason Reason { get; }
}

public sealed class CharacterAccessEvaluation
{
    internal CharacterAccessEvaluation(
        CharacterAccessProjection projection,
        ImmutableArray<AccessDecision> decisions)
    {
        Projection = projection;
        Decisions = decisions;
    }

    public CharacterAccessProjection Projection { get; }
    public ImmutableArray<AccessDecision> Decisions { get; }
}

public sealed class CharacterAccessException : Exception
{
    public CharacterAccessException(string message)
        : base(message)
    {
    }

    public CharacterAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
