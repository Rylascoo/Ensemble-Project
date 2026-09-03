using System.Collections.Immutable;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;

namespace Ensemble.E0.Core.StateInterpreter;

public enum StateMutationDomain
{
    WorldState,
    SceneState,
    UnresolvedProposition,
    CharacterKnowledge,
    CharacterBelief,
    CharacterSuspicion,
    CharacterMemory,
    CharacterGoal,
    CharacterDisposition,
    CharacterCircumstance,
    CharacterClaim,
    Relationship,
    Pressure
}

public sealed class StateInterpretationSource
{
    internal StateInterpretationSource(
        string candidateContentIdentityContract,
        string candidateContentHash,
        SceneId sourceSceneId,
        CharacterId sourceCharacterId,
        ContextPacketId sourceContextPacketId,
        ImmutableArray<CharacterId> rosterCharacterIds)
    {
        CandidateContentIdentityContract = candidateContentIdentityContract;
        CandidateContentHash = candidateContentHash;
        SourceSceneId = sourceSceneId;
        SourceCharacterId = sourceCharacterId;
        SourceContextPacketId = sourceContextPacketId;
        RosterCharacterIds = rosterCharacterIds;
    }

    public string CandidateContentIdentityContract { get; }
    public string CandidateContentHash { get; }
    public SceneId SourceSceneId { get; }
    public CharacterId SourceCharacterId { get; }
    public ContextPacketId SourceContextPacketId { get; }
    public ImmutableArray<CharacterId> RosterCharacterIds { get; }

    public static StateInterpretationSource Bind(
        ContextPacket sourceContext,
        CandidatePerformance sourceCandidate,
        IntegrityValidationEvaluation integrityEvaluation)
    {
        if (sourceContext is null)
        {
            throw new StateInterpretationException(
                "State interpretation source ContextPacket is required.");
        }

        if (sourceCandidate is null)
        {
            throw new StateInterpretationException(
                "State interpretation source CandidatePerformance is required.");
        }

        if (integrityEvaluation is null)
        {
            throw new StateInterpretationException(
                "State interpretation Integrity evaluation is required.");
        }

        RequireInitialized(sourceContext.SceneId, "source SceneId");
        RequireInitialized(sourceContext.SubjectCharacterId, "source CharacterId");
        RequireInitialized(sourceContext.OpportunityCharacterId, "opportunity CharacterId");
        RequireInitialized(sourceContext.ContextPacketId, "source ContextPacketId");

        if (sourceContext.SubjectCharacterId != sourceContext.OpportunityCharacterId)
        {
            throw new StateInterpretationException(
                "State interpretation source subject must equal its opportunity Character.");
        }

        var roster = ValidateAndCanonicalizeRoster(
            sourceContext.Roster,
            sourceContext.SubjectCharacterId);

        IntegrityCandidateInput freshInput;
        try
        {
            freshInput = IntegrityCandidateInput.Bind(sourceContext, sourceCandidate);
        }
        catch (IntegrityValidationException exception)
        {
            throw new StateInterpretationException(
                "State interpretation source could not bind the supplied Candidate to Integrity input.",
                exception);
        }

        if (freshInput.DeterministicRejectCodes.Length != 0)
        {
            throw new StateInterpretationException(
                "State interpretation source Candidate is deterministically rejected for the supplied Context.");
        }

        if (integrityEvaluation.Disposition != IntegrityDisposition.Accept)
        {
            throw new StateInterpretationException(
                "State interpretation requires an Integrity Accept evaluation.");
        }

        var trace = integrityEvaluation.Trace
            ?? throw new StateInterpretationException(
                "State interpretation Integrity evaluation trace is required.");

        if (!string.Equals(
                trace.ValidationContract,
                E0IntegrityContracts.ValidationContract,
                StringComparison.Ordinal))
        {
            throw new StateInterpretationException(
                "State interpretation Integrity validation contract is unsupported.");
        }

        var evaluatedInput = trace.Input
            ?? throw new StateInterpretationException(
                "State interpretation Integrity trace input is required.");

        if (!string.Equals(
                evaluatedInput.CandidateContentIdentityContract,
                freshInput.CandidateContentIdentityContract,
                StringComparison.Ordinal) ||
            !string.Equals(
                evaluatedInput.CandidateContentHash,
                freshInput.CandidateContentHash,
                StringComparison.Ordinal) ||
            evaluatedInput.SourceContextPacketId != freshInput.SourceContextPacketId)
        {
            throw new StateInterpretationException(
                "State interpretation Integrity evaluation does not match the supplied Candidate source identity.");
        }

        return new StateInterpretationSource(
            freshInput.CandidateContentIdentityContract,
            freshInput.CandidateContentHash,
            sourceContext.SceneId,
            sourceContext.SubjectCharacterId,
            sourceContext.ContextPacketId,
            roster);
    }

    private static ImmutableArray<CharacterId> ValidateAndCanonicalizeRoster(
        ImmutableArray<ContextParticipant> participants,
        CharacterId sourceCharacterId)
    {
        if (participants.IsDefault || participants.Length != 3)
        {
            throw new StateInterpretationException(
                "State interpretation source roster must contain exactly three E0 Characters.");
        }

        var values = new HashSet<string>(StringComparer.Ordinal);
        var ids = ImmutableArray.CreateBuilder<CharacterId>(participants.Length);
        var sourceCount = 0;

        foreach (var participant in participants)
        {
            if (participant is null)
            {
                throw new StateInterpretationException(
                    "State interpretation source roster contains an invalid participant.");
            }

            var value = RequireInitialized(
                participant.CharacterId,
                "roster CharacterId");

            if (!values.Add(value))
            {
                throw new StateInterpretationException(
                    "State interpretation source roster contains duplicate Character IDs.");
            }

            if (participant.CharacterId == sourceCharacterId)
            {
                sourceCount++;
            }

            ids.Add(participant.CharacterId);
        }

        if (sourceCount != 1)
        {
            throw new StateInterpretationException(
                "State interpretation source Character must occur exactly once in the roster.");
        }

        return ids
            .ToImmutable()
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static string RequireInitialized(SceneId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateInterpretationException(
                $"State interpretation trusted {fieldName} is uninitialized.",
                exception);
        }
    }

    private static string RequireInitialized(CharacterId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateInterpretationException(
                $"State interpretation trusted {fieldName} is uninitialized.",
                exception);
        }
    }

    private static string RequireInitialized(ContextPacketId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateInterpretationException(
                $"State interpretation trusted {fieldName} is uninitialized.",
                exception);
        }
    }
}

public sealed class StateInterpretationProposal
{
    internal StateInterpretationProposal(
        string contractVersion,
        string candidateContentIdentityContract,
        string candidateContentHash,
        SceneId sourceSceneId,
        ImmutableArray<StateMutationCandidate> mutations)
    {
        ContractVersion = contractVersion;
        CandidateContentIdentityContract = candidateContentIdentityContract;
        CandidateContentHash = candidateContentHash;
        SourceSceneId = sourceSceneId;
        Mutations = mutations;
    }

    public string ContractVersion { get; }
    public string CandidateContentIdentityContract { get; }
    public string CandidateContentHash { get; }
    public SceneId SourceSceneId { get; }
    public ImmutableArray<StateMutationCandidate> Mutations { get; }
}

public abstract class StateMutationChange
{
    internal StateMutationChange()
    {
    }
}

public sealed class AddStateMutationChange : StateMutationChange
{
    internal AddStateMutationChange(string text) => Text = text;

    public string Text { get; }
}

public sealed class SupersedeStateMutationChange : StateMutationChange
{
    internal SupersedeStateMutationChange(RecordId existingRecordId, string text)
    {
        ExistingRecordId = existingRecordId;
        Text = text;
    }

    public RecordId ExistingRecordId { get; }
    public string Text { get; }
}

public sealed class DeactivateStateMutationChange : StateMutationChange
{
    internal DeactivateStateMutationChange(RecordId existingRecordId) =>
        ExistingRecordId = existingRecordId;

    public RecordId ExistingRecordId { get; }
}

public abstract class StateMutationCandidate
{
    internal StateMutationCandidate(
        StateMutationDomain domain,
        ImmutableArray<RecordId> supportingRecordIds)
    {
        Domain = domain;
        SupportingRecordIds = supportingRecordIds;
    }

    public StateMutationDomain Domain { get; }
    public ImmutableArray<RecordId> SupportingRecordIds { get; }
}

public sealed class GlobalStateMutationCandidate : StateMutationCandidate
{
    internal GlobalStateMutationCandidate(
        StateMutationDomain domain,
        StateMutationChange change,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(domain, supportingRecordIds)
    {
        StateMutationSemanticInvariants.RequireGlobalDomain(domain);
        Change = change;
    }

    public StateMutationChange Change { get; }
}

public sealed class AppendOnlyCharacterStateMutationCandidate : StateMutationCandidate
{
    internal AppendOnlyCharacterStateMutationCandidate(
        StateMutationDomain domain,
        CharacterId subjectCharacterId,
        AddStateMutationChange change,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(domain, supportingRecordIds)
    {
        StateMutationSemanticInvariants.RequireAppendOnlyCharacterDomain(domain);
        SubjectCharacterId = subjectCharacterId;
        Change = change;
    }

    public CharacterId SubjectCharacterId { get; }
    public AddStateMutationChange Change { get; }
}

public sealed class MutableCharacterStateMutationCandidate : StateMutationCandidate
{
    internal MutableCharacterStateMutationCandidate(
        StateMutationDomain domain,
        CharacterId subjectCharacterId,
        StateMutationChange change,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(domain, supportingRecordIds)
    {
        StateMutationSemanticInvariants.RequireMutableCharacterDomain(domain);
        SubjectCharacterId = subjectCharacterId;
        Change = change;
    }

    public CharacterId SubjectCharacterId { get; }
    public StateMutationChange Change { get; }
}

public sealed class CharacterClaimMutationCandidate : StateMutationCandidate
{
    internal CharacterClaimMutationCandidate(
        CharacterId subjectCharacterId,
        string text,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(StateMutationDomain.CharacterClaim, supportingRecordIds)
    {
        SubjectCharacterId = subjectCharacterId;
        Text = text;
    }

    public CharacterId SubjectCharacterId { get; }
    public string Text { get; }
}

public sealed class RelationshipStateMutationCandidate : StateMutationCandidate
{
    internal RelationshipStateMutationCandidate(
        CharacterId subjectCharacterId,
        CharacterId targetCharacterId,
        StateMutationChange change,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(StateMutationDomain.Relationship, supportingRecordIds)
    {
        SubjectCharacterId = subjectCharacterId;
        TargetCharacterId = targetCharacterId;
        Change = change;
    }

    public CharacterId SubjectCharacterId { get; }
    public CharacterId TargetCharacterId { get; }
    public StateMutationChange Change { get; }
}

public sealed class StateInterpretationException : Exception
{
    public StateInterpretationException(string message)
        : base(message)
    {
    }

    public StateInterpretationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

internal static class StateMutationSemanticInvariants
{
    public static void RequireGlobalDomain(StateMutationDomain domain)
    {
        if (domain is not StateMutationDomain.WorldState and
            not StateMutationDomain.SceneState and
            not StateMutationDomain.UnresolvedProposition and
            not StateMutationDomain.Pressure)
        {
            throw new StateInterpretationException(
                "Global State mutation candidate contains an invalid domain family.");
        }
    }

    public static void RequireAppendOnlyCharacterDomain(StateMutationDomain domain)
    {
        if (domain is not StateMutationDomain.CharacterKnowledge and
            not StateMutationDomain.CharacterMemory)
        {
            throw new StateInterpretationException(
                "Append-only Character State mutation candidate contains an invalid domain family.");
        }
    }

    public static void RequireMutableCharacterDomain(StateMutationDomain domain)
    {
        if (domain is not StateMutationDomain.CharacterBelief and
            not StateMutationDomain.CharacterSuspicion and
            not StateMutationDomain.CharacterGoal and
            not StateMutationDomain.CharacterDisposition and
            not StateMutationDomain.CharacterCircumstance)
        {
            throw new StateInterpretationException(
                "Mutable Character State mutation candidate contains an invalid domain family.");
        }
    }
}

internal static class StateInterpretationInvariants
{
    public static void ValidateSource(StateInterpretationSource? source)
    {
        if (source is null)
        {
            throw new StateInterpretationException(
                "StateInterpretationSource is required.");
        }

        if (!string.Equals(
                source.CandidateContentIdentityContract,
                E0IntegrityContracts.CandidateContentIdentityContract,
                StringComparison.Ordinal))
        {
            throw new StateInterpretationException(
                "StateInterpretationSource Candidate content identity contract is unsupported.");
        }

        ValidateLowerHexHash(source.CandidateContentHash);
        RequireInitialized(source.SourceSceneId, "SourceSceneId");
        RequireInitialized(source.SourceCharacterId, "SourceCharacterId");
        RequireInitialized(source.SourceContextPacketId, "SourceContextPacketId");

        if (source.RosterCharacterIds.IsDefault ||
            source.RosterCharacterIds.Length != 3)
        {
            throw new StateInterpretationException(
                "StateInterpretationSource roster must contain exactly three E0 Characters.");
        }

        var previous = string.Empty;
        var sourceCount = 0;
        for (var index = 0; index < source.RosterCharacterIds.Length; index++)
        {
            var id = source.RosterCharacterIds[index];
            var value = RequireInitialized(id, "RosterCharacterId");

            if (index != 0 && string.CompareOrdinal(previous, value) >= 0)
            {
                throw new StateInterpretationException(
                    "StateInterpretationSource roster must be unique and in canonical ordinal order.");
            }

            if (id == source.SourceCharacterId)
            {
                sourceCount++;
            }

            previous = value;
        }

        if (sourceCount != 1)
        {
            throw new StateInterpretationException(
                "StateInterpretationSource source Character must occur exactly once in its roster.");
        }
    }

    public static HashSet<string> RosterValues(StateInterpretationSource source) =>
        source.RosterCharacterIds
            .Select(id => id.Value)
            .ToHashSet(StringComparer.Ordinal);

    private static void ValidateLowerHexHash(string? hash)
    {
        if (hash is null || hash.Length != 64)
        {
            throw new StateInterpretationException(
                "StateInterpretationSource Candidate content hash is invalid.");
        }

        foreach (var character in hash)
        {
            if (!((character >= '0' && character <= '9') ||
                  (character >= 'a' && character <= 'f')))
            {
                throw new StateInterpretationException(
                    "StateInterpretationSource Candidate content hash is invalid.");
            }
        }
    }

    private static string RequireInitialized(SceneId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateInterpretationException(
                $"StateInterpretationSource {fieldName} is uninitialized.",
                exception);
        }
    }

    private static string RequireInitialized(CharacterId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateInterpretationException(
                $"StateInterpretationSource {fieldName} is uninitialized.",
                exception);
        }
    }

    private static string RequireInitialized(ContextPacketId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateInterpretationException(
                $"StateInterpretationSource {fieldName} is uninitialized.",
                exception);
        }
    }
}
