using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Access;

public sealed record SceneParticipant(CharacterId CharacterId, string DisplayName);

public sealed record PermittedRecord(RecordId RecordId, string Text);

public sealed record PermittedRelationship(
    RecordId RecordId,
    CharacterId TargetCharacterId,
    string Text);

public sealed record CharacterAccessProjection(
    SceneId SceneId,
    CharacterId SubjectCharacterId,
    ImmutableArray<SceneParticipant> Roster,
    ImmutableArray<PermittedRecord> SceneState,
    ImmutableArray<PermittedRecord> Pressures,
    ImmutableArray<PermittedRecord> Constitution,
    ImmutableArray<PermittedRecord> Disposition,
    ImmutableArray<PermittedRecord> Circumstance,
    ImmutableArray<PermittedRecord> Observations,
    ImmutableArray<PermittedRecord> Knowledge,
    ImmutableArray<PermittedRecord> Beliefs,
    ImmutableArray<PermittedRecord> Suspicions,
    ImmutableArray<PermittedRecord> Memories,
    ImmutableArray<PermittedRecord> Goals,
    ImmutableArray<PermittedRelationship> Relationships);

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
    OwnedByOtherCharacterExcluded
}

public sealed record AccessDecision(
    RecordId RecordId,
    AccessDisposition Disposition,
    AccessReason Reason);

public sealed record CharacterAccessEvaluation(
    CharacterAccessProjection Projection,
    ImmutableArray<AccessDecision> Decisions);

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
