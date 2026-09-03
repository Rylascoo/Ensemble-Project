using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Serialization;
using Ensemble.E0.Core.StateInterpreter;

namespace Ensemble.E0.Core.StateAuthority;

public static class StateAuthorityContracts
{
    public const string ContractVersion = "ensemble.e0.state-authority.review.v1";
    public const string PolicyContractVersion = "ensemble.e0.state-authority.policy.v1";
    public const string ProposalContentIdentityContract =
        "ensemble.e0.state-authority.interpreter-proposal-content.v1";
    public const string ReviewSetContractVersion = "ensemble.e0.state-authority.review-set.v1";
}

public enum StateAuthorityRecordDomain
{
    HistoricalTruth,
    UnresolvedProposition,
    WorldState,
    SceneState,
    CharacterConstitution,
    CharacterDisposition,
    CharacterCircumstance,
    CharacterObservation,
    CharacterKnowledge,
    CharacterBelief,
    CharacterSuspicion,
    CharacterMemory,
    CharacterGoal,
    CharacterClaim,
    Relationship,
    Pressure
}

public enum StateAuthorityRecordLifecycle
{
    Active,
    Inactive
}

public enum StateAuthorityRecordProtection
{
    None,
    SystemImmutable,
    CreatorLocked
}

public abstract class StateAuthorityRecordDescriptor
{
    internal StateAuthorityRecordDescriptor(
        RecordId recordId,
        StateAuthorityRecordDomain recordDomain,
        StateAuthorityRecordLifecycle lifecycle,
        StateAuthorityRecordProtection protection)
    {
        RecordId = recordId;
        RecordDomain = recordDomain;
        Lifecycle = lifecycle;
        Protection = protection;
    }

    public RecordId RecordId { get; }
    public StateAuthorityRecordDomain RecordDomain { get; }
    public StateAuthorityRecordLifecycle Lifecycle { get; }
    public StateAuthorityRecordProtection Protection { get; }

    internal abstract StateAuthorityRecordDescriptor WithProtection(
        StateAuthorityRecordProtection protection);
}

public sealed class GlobalStateAuthorityRecordDescriptor : StateAuthorityRecordDescriptor
{
    internal GlobalStateAuthorityRecordDescriptor(
        RecordId recordId,
        StateAuthorityRecordDomain recordDomain,
        StateAuthorityRecordLifecycle lifecycle,
        StateAuthorityRecordProtection protection)
        : base(recordId, recordDomain, lifecycle, protection)
    {
    }

    internal override StateAuthorityRecordDescriptor WithProtection(
        StateAuthorityRecordProtection protection) =>
        new GlobalStateAuthorityRecordDescriptor(RecordId, RecordDomain, Lifecycle, protection);
}

public sealed class CharacterStateAuthorityRecordDescriptor : StateAuthorityRecordDescriptor
{
    internal CharacterStateAuthorityRecordDescriptor(
        RecordId recordId,
        StateAuthorityRecordDomain recordDomain,
        CharacterId subjectCharacterId,
        StateAuthorityRecordLifecycle lifecycle,
        StateAuthorityRecordProtection protection)
        : base(recordId, recordDomain, lifecycle, protection)
    {
        SubjectCharacterId = subjectCharacterId;
    }

    public CharacterId SubjectCharacterId { get; }

    internal override StateAuthorityRecordDescriptor WithProtection(
        StateAuthorityRecordProtection protection) =>
        new CharacterStateAuthorityRecordDescriptor(
            RecordId,
            RecordDomain,
            SubjectCharacterId,
            Lifecycle,
            protection);
}

public sealed class RelationshipStateAuthorityRecordDescriptor : StateAuthorityRecordDescriptor
{
    internal RelationshipStateAuthorityRecordDescriptor(
        RecordId recordId,
        CharacterId subjectCharacterId,
        CharacterId targetCharacterId,
        StateAuthorityRecordLifecycle lifecycle,
        StateAuthorityRecordProtection protection)
        : base(recordId, StateAuthorityRecordDomain.Relationship, lifecycle, protection)
    {
        SubjectCharacterId = subjectCharacterId;
        TargetCharacterId = targetCharacterId;
    }

    public CharacterId SubjectCharacterId { get; }
    public CharacterId TargetCharacterId { get; }

    internal override StateAuthorityRecordDescriptor WithProtection(
        StateAuthorityRecordProtection protection) =>
        new RelationshipStateAuthorityRecordDescriptor(
            RecordId,
            SubjectCharacterId,
            TargetCharacterId,
            Lifecycle,
            protection);
}

public sealed class StateAuthoritySnapshot
{
    internal StateAuthoritySnapshot(
        SceneId sceneId,
        ImmutableArray<CharacterId> rosterCharacterIds,
        ImmutableArray<StateAuthorityRecordDescriptor> records)
    {
        SceneId = sceneId;
        RosterCharacterIds = rosterCharacterIds;
        Records = records;
    }

    public SceneId SceneId { get; }
    public ImmutableArray<CharacterId> RosterCharacterIds { get; }
    public ImmutableArray<StateAuthorityRecordDescriptor> Records { get; }

    public static StateAuthoritySnapshot Bind(
        ValidatedFixture fixture,
        ImmutableArray<RecordId> creatorLockedRecordIds)
    {
        if (fixture is null)
        {
            throw new StateAuthorityException("State Authority fixture is required.");
        }

        if (creatorLockedRecordIds.IsDefault)
        {
            throw new StateAuthorityException("State Authority creator-lock input is required.");
        }

        RequireInitialized(fixture.Scene.Id, "SceneId");
        var roster = CanonicalizeRoster(fixture);
        var descriptors = BuildRecordDescriptors(fixture);

        var byRecordId = new Dictionary<string, StateAuthorityRecordDescriptor>(StringComparer.Ordinal);
        foreach (var descriptor in descriptors)
        {
            var recordValue = RequireInitialized(descriptor.RecordId, "RecordId");
            if (!byRecordId.TryAdd(recordValue, descriptor))
            {
                throw new StateAuthorityException(
                    "State Authority snapshot contains duplicate Record IDs.");
            }
        }

        var locks = new HashSet<string>(StringComparer.Ordinal);
        foreach (var lockId in creatorLockedRecordIds)
        {
            var lockValue = RequireInitialized(lockId, "creator lock RecordId");
            if (!locks.Add(lockValue))
            {
                throw new StateAuthorityException(
                    "State Authority creator-lock input contains a duplicate Record ID.");
            }

            if (!byRecordId.ContainsKey(lockValue))
            {
                throw new StateAuthorityException(
                    "State Authority creator-lock input contains an unknown Record ID.");
            }
        }

        var protectedDescriptors = descriptors
            .Select(descriptor =>
            {
                if (!locks.Contains(descriptor.RecordId.Value) ||
                    descriptor.Protection == StateAuthorityRecordProtection.SystemImmutable)
                {
                    return descriptor;
                }

                return descriptor.WithProtection(StateAuthorityRecordProtection.CreatorLocked);
            })
            .OrderBy(descriptor => descriptor.RecordId.Value, StringComparer.Ordinal)
            .ToImmutableArray();

        return new StateAuthoritySnapshot(fixture.Scene.Id, roster, protectedDescriptors);
    }

    private static ImmutableArray<CharacterId> CanonicalizeRoster(ValidatedFixture fixture)
    {
        if (fixture.Scene.Roster.IsDefault || fixture.Scene.Roster.Length != 3)
        {
            throw new StateAuthorityException(
                "State Authority snapshot roster must contain exactly three E0 Characters.");
        }

        var knownCharacters = fixture.Characters
            .Select(character => RequireInitialized(character.Id, "fixture CharacterId"))
            .ToHashSet(StringComparer.Ordinal);
        var values = new HashSet<string>(StringComparer.Ordinal);
        var roster = ImmutableArray.CreateBuilder<CharacterId>(fixture.Scene.Roster.Length);

        foreach (var characterId in fixture.Scene.Roster)
        {
            var value = RequireInitialized(characterId, "roster CharacterId");
            if (!values.Add(value))
            {
                throw new StateAuthorityException(
                    "State Authority snapshot roster contains duplicate Character IDs.");
            }

            if (!knownCharacters.Contains(value))
            {
                throw new StateAuthorityException(
                    "State Authority snapshot roster contains an unknown Character ID.");
            }

            roster.Add(characterId);
        }

        return roster
            .ToImmutable()
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static ImmutableArray<StateAuthorityRecordDescriptor> BuildRecordDescriptors(
        ValidatedFixture fixture)
    {
        var records = ImmutableArray.CreateBuilder<StateAuthorityRecordDescriptor>();

        AddGlobal(records, fixture.HistoricalTruth, StateAuthorityRecordDomain.HistoricalTruth, true);
        AddGlobal(records, fixture.UnresolvedPropositions, StateAuthorityRecordDomain.UnresolvedProposition, false);
        AddGlobal(records, fixture.WorldState, StateAuthorityRecordDomain.WorldState, false);
        AddGlobal(records, fixture.SceneState, StateAuthorityRecordDomain.SceneState, false);
        AddGlobal(records, fixture.Pressures, StateAuthorityRecordDomain.Pressure, false);

        var rosterValues = fixture.Scene.Roster
            .Select(id => RequireInitialized(id, "roster CharacterId"))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var character in fixture.Characters)
        {
            var subjectValue = RequireInitialized(character.Id, "CharacterId");
            AddCharacter(records, character.Constitution, character.Id, StateAuthorityRecordDomain.CharacterConstitution, true);
            AddCharacter(records, character.Disposition, character.Id, StateAuthorityRecordDomain.CharacterDisposition, false);
            AddCharacter(records, character.Circumstance, character.Id, StateAuthorityRecordDomain.CharacterCircumstance, false);
            AddCharacter(records, character.Observations, character.Id, StateAuthorityRecordDomain.CharacterObservation, true);
            AddCharacter(records, character.Knowledge, character.Id, StateAuthorityRecordDomain.CharacterKnowledge, false);
            AddCharacter(records, character.Beliefs, character.Id, StateAuthorityRecordDomain.CharacterBelief, false);
            AddCharacter(records, character.Suspicions, character.Id, StateAuthorityRecordDomain.CharacterSuspicion, false);
            AddCharacter(records, character.Memories, character.Id, StateAuthorityRecordDomain.CharacterMemory, false);
            AddCharacter(records, character.Goals, character.Id, StateAuthorityRecordDomain.CharacterGoal, false);

            foreach (var relationship in character.Relationships)
            {
                RequireInitialized(relationship.Id, "Relationship RecordId");
                var targetValue = RequireInitialized(
                    relationship.TargetCharacterId,
                    "Relationship target CharacterId");
                if (!rosterValues.Contains(subjectValue) || !rosterValues.Contains(targetValue))
                {
                    throw new StateAuthorityException(
                        "State Authority relationship record references a Character outside the Scene roster.");
                }

                if (character.Id == relationship.TargetCharacterId)
                {
                    throw new StateAuthorityException(
                        "State Authority relationship record cannot target its subject Character.");
                }

                records.Add(new RelationshipStateAuthorityRecordDescriptor(
                    relationship.Id,
                    character.Id,
                    relationship.TargetCharacterId,
                    StateAuthorityRecordLifecycle.Active,
                    StateAuthorityRecordProtection.None));
            }
        }

        return records.ToImmutable();
    }

    private static void AddGlobal(
        ImmutableArray<StateAuthorityRecordDescriptor>.Builder target,
        ImmutableArray<ValidatedRecord> records,
        StateAuthorityRecordDomain domain,
        bool systemImmutable)
    {
        foreach (var record in records)
        {
            RequireInitialized(record.Id, "RecordId");
            target.Add(new GlobalStateAuthorityRecordDescriptor(
                record.Id,
                domain,
                StateAuthorityRecordLifecycle.Active,
                systemImmutable
                    ? StateAuthorityRecordProtection.SystemImmutable
                    : StateAuthorityRecordProtection.None));
        }
    }

    private static void AddCharacter(
        ImmutableArray<StateAuthorityRecordDescriptor>.Builder target,
        ImmutableArray<ValidatedRecord> records,
        CharacterId subjectCharacterId,
        StateAuthorityRecordDomain domain,
        bool systemImmutable)
    {
        RequireInitialized(subjectCharacterId, "subject CharacterId");
        foreach (var record in records)
        {
            RequireInitialized(record.Id, "RecordId");
            target.Add(new CharacterStateAuthorityRecordDescriptor(
                record.Id,
                domain,
                subjectCharacterId,
                StateAuthorityRecordLifecycle.Active,
                systemImmutable
                    ? StateAuthorityRecordProtection.SystemImmutable
                    : StateAuthorityRecordProtection.None));
        }
    }

    internal static string RequireInitialized(SceneId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateAuthorityException(
                $"State Authority {fieldName} is uninitialized.",
                exception);
        }
    }

    internal static string RequireInitialized(CharacterId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateAuthorityException(
                $"State Authority {fieldName} is uninitialized.",
                exception);
        }
    }

    internal static string RequireInitialized(RecordId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateAuthorityException(
                $"State Authority {fieldName} is uninitialized.",
                exception);
        }
    }
}

public abstract class StateAuthorityTransition
{
    internal StateAuthorityTransition()
    {
    }
}

public sealed class AddStateAuthorityTransition : StateAuthorityTransition
{
    internal AddStateAuthorityTransition()
    {
    }
}

public sealed class SupersedeStateAuthorityTransition : StateAuthorityTransition
{
    internal SupersedeStateAuthorityTransition(RecordId existingRecordId) =>
        ExistingRecordId = existingRecordId;

    public RecordId ExistingRecordId { get; }
}

public sealed class DeactivateStateAuthorityTransition : StateAuthorityTransition
{
    internal DeactivateStateAuthorityTransition(RecordId existingRecordId) =>
        ExistingRecordId = existingRecordId;

    public RecordId ExistingRecordId { get; }
}

public abstract class StateAuthorityMutationInput
{
    internal StateAuthorityMutationInput(
        int mutationIndex,
        StateMutationDomain domain,
        StateAuthorityTransition transition,
        ImmutableArray<RecordId> supportingRecordIds)
    {
        MutationIndex = mutationIndex;
        Domain = domain;
        Transition = transition;
        SupportingRecordIds = supportingRecordIds;
    }

    public int MutationIndex { get; }
    public StateMutationDomain Domain { get; }
    public StateAuthorityTransition Transition { get; }
    public ImmutableArray<RecordId> SupportingRecordIds { get; }
}

public sealed class GlobalStateAuthorityMutationInput : StateAuthorityMutationInput
{
    internal GlobalStateAuthorityMutationInput(
        int mutationIndex,
        StateMutationDomain domain,
        StateAuthorityTransition transition,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(mutationIndex, domain, transition, supportingRecordIds)
    {
    }
}

public sealed class CharacterStateAuthorityMutationInput : StateAuthorityMutationInput
{
    internal CharacterStateAuthorityMutationInput(
        int mutationIndex,
        StateMutationDomain domain,
        CharacterId subjectCharacterId,
        StateAuthorityTransition transition,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(mutationIndex, domain, transition, supportingRecordIds)
    {
        SubjectCharacterId = subjectCharacterId;
    }

    public CharacterId SubjectCharacterId { get; }
}

public sealed class RelationshipStateAuthorityMutationInput : StateAuthorityMutationInput
{
    internal RelationshipStateAuthorityMutationInput(
        int mutationIndex,
        CharacterId subjectCharacterId,
        CharacterId targetCharacterId,
        StateAuthorityTransition transition,
        ImmutableArray<RecordId> supportingRecordIds)
        : base(mutationIndex, StateMutationDomain.Relationship, transition, supportingRecordIds)
    {
        SubjectCharacterId = subjectCharacterId;
        TargetCharacterId = targetCharacterId;
    }

    public CharacterId SubjectCharacterId { get; }
    public CharacterId TargetCharacterId { get; }
}

public sealed class StateAuthorityInput
{
    internal StateAuthorityInput(
        string proposalContentIdentityContract,
        string proposalContentHash,
        StateAuthoritySnapshot snapshot,
        ImmutableArray<StateAuthorityMutationInput> mutations)
    {
        ProposalContentIdentityContract = proposalContentIdentityContract;
        ProposalContentHash = proposalContentHash;
        Snapshot = snapshot;
        Mutations = mutations;
    }

    public string ProposalContentIdentityContract { get; }
    public string ProposalContentHash { get; }
    public StateAuthoritySnapshot Snapshot { get; }
    public ImmutableArray<StateAuthorityMutationInput> Mutations { get; }

    public static StateAuthorityInput Bind(
        StateAuthoritySnapshot snapshot,
        StateInterpretationSource source,
        StateInterpretationProposal proposal)
    {
        if (snapshot is null)
        {
            throw new StateAuthorityException("State Authority snapshot is required.");
        }

        if (source is null)
        {
            throw new StateAuthorityException("State Authority source is required.");
        }

        if (proposal is null)
        {
            throw new StateAuthorityException("State Authority proposal is required.");
        }

        ValidateSnapshot(snapshot);
        ValidateSourceProposalAssociation(snapshot, source, proposal);

        var mutations = ImmutableArray.CreateBuilder<StateAuthorityMutationInput>(
            proposal.Mutations.Length);
        for (var index = 0; index < proposal.Mutations.Length; index++)
        {
            mutations.Add(ProjectMutation(index, proposal.Mutations[index], snapshot.RosterCharacterIds));
        }

        var hash = StateAuthorityProposalCanonicalizer.Hash(proposal);
        return new StateAuthorityInput(
            StateAuthorityContracts.ProposalContentIdentityContract,
            hash,
            snapshot,
            mutations.ToImmutable());
    }

    private static void ValidateSnapshot(StateAuthoritySnapshot snapshot)
    {
        StateAuthoritySnapshot.RequireInitialized(snapshot.SceneId, "snapshot SceneId");
        if (snapshot.RosterCharacterIds.IsDefault || snapshot.RosterCharacterIds.Length != 3)
        {
            throw new StateAuthorityException(
                "State Authority snapshot roster is invalid.");
        }

        var previousRoster = string.Empty;
        for (var index = 0; index < snapshot.RosterCharacterIds.Length; index++)
        {
            var value = StateAuthoritySnapshot.RequireInitialized(
                snapshot.RosterCharacterIds[index],
                "snapshot roster CharacterId");
            if (index != 0 && string.CompareOrdinal(previousRoster, value) >= 0)
            {
                throw new StateAuthorityException(
                    "State Authority snapshot roster is not canonical.");
            }

            previousRoster = value;
        }

        if (snapshot.Records.IsDefault)
        {
            throw new StateAuthorityException("State Authority snapshot records are invalid.");
        }

        var previousRecord = string.Empty;
        for (var index = 0; index < snapshot.Records.Length; index++)
        {
            var descriptor = snapshot.Records[index]
                ?? throw new StateAuthorityException(
                    "State Authority snapshot contains an invalid record descriptor.");
            var value = StateAuthoritySnapshot.RequireInitialized(
                descriptor.RecordId,
                "snapshot RecordId");
            if (index != 0 && string.CompareOrdinal(previousRecord, value) >= 0)
            {
                throw new StateAuthorityException(
                    "State Authority snapshot records are not canonical.");
            }

            if (!Enum.IsDefined(descriptor.RecordDomain) ||
                !Enum.IsDefined(descriptor.Lifecycle) ||
                !Enum.IsDefined(descriptor.Protection))
            {
                throw new StateAuthorityException(
                    "State Authority snapshot contains an invalid record descriptor value.");
            }

            previousRecord = value;
        }
    }

    private static void ValidateSourceProposalAssociation(
        StateAuthoritySnapshot snapshot,
        StateInterpretationSource source,
        StateInterpretationProposal proposal)
    {
        if (!string.Equals(
                proposal.ContractVersion,
                StateInterpretationContract.ContractVersion,
                StringComparison.Ordinal))
        {
            throw new StateAuthorityException(
                "State Authority proposal contract is unsupported.");
        }

        if (!string.Equals(
                source.CandidateContentIdentityContract,
                E0IntegrityContracts.CandidateContentIdentityContract,
                StringComparison.Ordinal) ||
            !string.Equals(
                proposal.CandidateContentIdentityContract,
                E0IntegrityContracts.CandidateContentIdentityContract,
                StringComparison.Ordinal))
        {
            throw new StateAuthorityException(
                "State Authority Candidate content identity contract is unsupported.");
        }

        ValidateLowerHexHash(source.CandidateContentHash);
        ValidateLowerHexHash(proposal.CandidateContentHash);
        StateAuthoritySnapshot.RequireInitialized(source.SourceSceneId, "source SceneId");
        StateAuthoritySnapshot.RequireInitialized(proposal.SourceSceneId, "proposal SceneId");

        if (snapshot.SceneId != source.SourceSceneId || snapshot.SceneId != proposal.SourceSceneId)
        {
            throw new StateAuthorityException(
                "State Authority snapshot, source, and proposal Scene IDs do not match.");
        }

        if (source.RosterCharacterIds.IsDefault ||
            !source.RosterCharacterIds.SequenceEqual(snapshot.RosterCharacterIds))
        {
            throw new StateAuthorityException(
                "State Authority source roster does not match the snapshot roster.");
        }

        if (!string.Equals(
                source.CandidateContentIdentityContract,
                proposal.CandidateContentIdentityContract,
                StringComparison.Ordinal) ||
            !string.Equals(
                source.CandidateContentHash,
                proposal.CandidateContentHash,
                StringComparison.Ordinal))
        {
            throw new StateAuthorityException(
                "State Authority proposal does not match its source Candidate identity.");
        }

        if (proposal.Mutations.IsDefault)
        {
            throw new StateAuthorityException("State Authority proposal mutations are invalid.");
        }
    }

    private static StateAuthorityMutationInput ProjectMutation(
        int mutationIndex,
        StateMutationCandidate mutation,
        ImmutableArray<CharacterId> roster)
    {
        if (mutation is null || !Enum.IsDefined(mutation.Domain))
        {
            throw new StateAuthorityException(
                "State Authority proposal contains an invalid mutation.");
        }

        ValidateSupportingRecordIds(mutation.SupportingRecordIds);
        var rosterValues = roster.Select(id => id.Value).ToHashSet(StringComparer.Ordinal);

        return mutation switch
        {
            GlobalStateMutationCandidate global => ProjectGlobal(
                mutationIndex,
                global,
                rosterValues),
            AppendOnlyCharacterStateMutationCandidate appendOnly => ProjectAppendOnlyCharacter(
                mutationIndex,
                appendOnly,
                rosterValues),
            MutableCharacterStateMutationCandidate mutable => ProjectMutableCharacter(
                mutationIndex,
                mutable,
                rosterValues),
            CharacterClaimMutationCandidate claim => ProjectClaim(
                mutationIndex,
                claim,
                rosterValues),
            RelationshipStateMutationCandidate relationship => ProjectRelationship(
                mutationIndex,
                relationship,
                rosterValues),
            _ => throw new StateAuthorityException(
                "State Authority proposal contains an unsupported mutation shape.")
        };
    }

    private static GlobalStateAuthorityMutationInput ProjectGlobal(
        int mutationIndex,
        GlobalStateMutationCandidate mutation,
        HashSet<string> roster)
    {
        _ = roster;
        if (mutation.Domain is not StateMutationDomain.WorldState and
            not StateMutationDomain.SceneState and
            not StateMutationDomain.UnresolvedProposition and
            not StateMutationDomain.Pressure)
        {
            throw new StateAuthorityException(
                "State Authority global mutation contains an invalid domain family.");
        }

        return new GlobalStateAuthorityMutationInput(
            mutationIndex,
            mutation.Domain,
            ProjectChange(mutation.Change),
            mutation.SupportingRecordIds);
    }

    private static CharacterStateAuthorityMutationInput ProjectAppendOnlyCharacter(
        int mutationIndex,
        AppendOnlyCharacterStateMutationCandidate mutation,
        HashSet<string> roster)
    {
        if (mutation.Domain is not StateMutationDomain.CharacterKnowledge and
            not StateMutationDomain.CharacterMemory)
        {
            throw new StateAuthorityException(
                "State Authority append-only Character mutation contains an invalid domain family.");
        }

        ValidateRosterCharacter(mutation.SubjectCharacterId, roster, "subject CharacterId");
        return new CharacterStateAuthorityMutationInput(
            mutationIndex,
            mutation.Domain,
            mutation.SubjectCharacterId,
            ProjectChange(mutation.Change),
            mutation.SupportingRecordIds);
    }

    private static CharacterStateAuthorityMutationInput ProjectMutableCharacter(
        int mutationIndex,
        MutableCharacterStateMutationCandidate mutation,
        HashSet<string> roster)
    {
        if (mutation.Domain is not StateMutationDomain.CharacterBelief and
            not StateMutationDomain.CharacterSuspicion and
            not StateMutationDomain.CharacterGoal and
            not StateMutationDomain.CharacterDisposition and
            not StateMutationDomain.CharacterCircumstance)
        {
            throw new StateAuthorityException(
                "State Authority mutable Character mutation contains an invalid domain family.");
        }

        ValidateRosterCharacter(mutation.SubjectCharacterId, roster, "subject CharacterId");
        return new CharacterStateAuthorityMutationInput(
            mutationIndex,
            mutation.Domain,
            mutation.SubjectCharacterId,
            ProjectChange(mutation.Change),
            mutation.SupportingRecordIds);
    }

    private static CharacterStateAuthorityMutationInput ProjectClaim(
        int mutationIndex,
        CharacterClaimMutationCandidate mutation,
        HashSet<string> roster)
    {
        ValidateRosterCharacter(mutation.SubjectCharacterId, roster, "claim subject CharacterId");
        return new CharacterStateAuthorityMutationInput(
            mutationIndex,
            StateMutationDomain.CharacterClaim,
            mutation.SubjectCharacterId,
            new AddStateAuthorityTransition(),
            mutation.SupportingRecordIds);
    }

    private static RelationshipStateAuthorityMutationInput ProjectRelationship(
        int mutationIndex,
        RelationshipStateMutationCandidate mutation,
        HashSet<string> roster)
    {
        ValidateRosterCharacter(mutation.SubjectCharacterId, roster, "relationship subject CharacterId");
        ValidateRosterCharacter(mutation.TargetCharacterId, roster, "relationship target CharacterId");
        if (mutation.SubjectCharacterId == mutation.TargetCharacterId)
        {
            throw new StateAuthorityException(
                "State Authority relationship mutation cannot target its subject Character.");
        }

        return new RelationshipStateAuthorityMutationInput(
            mutationIndex,
            mutation.SubjectCharacterId,
            mutation.TargetCharacterId,
            ProjectChange(mutation.Change),
            mutation.SupportingRecordIds);
    }

    private static StateAuthorityTransition ProjectChange(StateMutationChange change) =>
        change switch
        {
            AddStateMutationChange => new AddStateAuthorityTransition(),
            SupersedeStateMutationChange supersede =>
                new SupersedeStateAuthorityTransition(ValidateExistingRecordId(supersede.ExistingRecordId)),
            DeactivateStateMutationChange deactivate =>
                new DeactivateStateAuthorityTransition(ValidateExistingRecordId(deactivate.ExistingRecordId)),
            _ => throw new StateAuthorityException(
                "State Authority proposal contains an unsupported mutation transition.")
        };

    private static RecordId ValidateExistingRecordId(RecordId id)
    {
        StateAuthoritySnapshot.RequireInitialized(id, "existing RecordId");
        return id;
    }

    private static void ValidateSupportingRecordIds(ImmutableArray<RecordId> ids)
    {
        if (ids.IsDefault)
        {
            throw new StateAuthorityException(
                "State Authority supporting Record IDs are invalid.");
        }

        var previous = string.Empty;
        for (var index = 0; index < ids.Length; index++)
        {
            var value = StateAuthoritySnapshot.RequireInitialized(ids[index], "supporting RecordId");
            if (index != 0 && string.CompareOrdinal(previous, value) >= 0)
            {
                throw new StateAuthorityException(
                    "State Authority supporting Record IDs are not canonical.");
            }

            previous = value;
        }
    }

    private static void ValidateRosterCharacter(
        CharacterId id,
        HashSet<string> roster,
        string fieldName)
    {
        var value = StateAuthoritySnapshot.RequireInitialized(id, fieldName);
        if (!roster.Contains(value))
        {
            throw new StateAuthorityException(
                "State Authority mutation references a Character outside the snapshot roster.");
        }
    }

    private static void ValidateLowerHexHash(string? hash)
    {
        if (hash is null || hash.Length != 64 ||
            hash.Any(character =>
                !((character >= '0' && character <= '9') ||
                  (character >= 'a' && character <= 'f'))))
        {
            throw new StateAuthorityException(
                "State Authority Candidate content hash is invalid.");
        }
    }
}

public sealed class StateAuthorityPolicy
{
    internal StateAuthorityPolicy(ImmutableArray<StateMutationDomain> autoApproveDomains)
    {
        ContractVersion = StateAuthorityContracts.PolicyContractVersion;
        AutoApproveDomains = autoApproveDomains;
    }

    public string ContractVersion { get; }
    public ImmutableArray<StateMutationDomain> AutoApproveDomains { get; }

    public static StateAuthorityPolicy Create(
        ImmutableArray<StateMutationDomain> autoApproveDomains)
    {
        if (autoApproveDomains.IsDefault)
        {
            throw new StateAuthorityException("State Authority policy domain input is required.");
        }

        var seen = new HashSet<StateMutationDomain>();
        foreach (var domain in autoApproveDomains)
        {
            if (!Enum.IsDefined(domain))
            {
                throw new StateAuthorityException(
                    "State Authority policy contains an undefined mutation domain.");
            }

            if (!seen.Add(domain))
            {
                throw new StateAuthorityException(
                    "State Authority policy contains a duplicate mutation domain.");
            }

            if (IsAlwaysMandatoryReviewDomain(domain))
            {
                throw new StateAuthorityException(
                    "State Authority policy cannot auto-approve a mandatory-review domain.");
            }

            if (!IsPolicyEligibleDomain(domain))
            {
                throw new StateAuthorityException(
                    "State Authority policy contains a non-policy-eligible mutation domain.");
            }
        }

        return new StateAuthorityPolicy(
            autoApproveDomains
                .OrderBy(domain => (int)domain)
                .ToImmutableArray());
    }

    internal static bool IsAlwaysMandatoryReviewDomain(StateMutationDomain domain) =>
        domain is StateMutationDomain.WorldState or
            StateMutationDomain.SceneState or
            StateMutationDomain.CharacterKnowledge or
            StateMutationDomain.CharacterMemory or
            StateMutationDomain.CharacterDisposition or
            StateMutationDomain.Relationship;

    internal static bool IsPolicyEligibleDomain(StateMutationDomain domain) =>
        domain is StateMutationDomain.UnresolvedProposition or
            StateMutationDomain.CharacterBelief or
            StateMutationDomain.CharacterSuspicion or
            StateMutationDomain.CharacterGoal or
            StateMutationDomain.CharacterCircumstance or
            StateMutationDomain.CharacterClaim or
            StateMutationDomain.Pressure;
}

public enum StateAuthorityReviewChoiceKind
{
    Approve,
    Reject
}

public sealed class StateAuthorityReviewChoice
{
    private StateAuthorityReviewChoice(int mutationIndex, StateAuthorityReviewChoiceKind choice)
    {
        MutationIndex = mutationIndex;
        Choice = choice;
    }

    public int MutationIndex { get; }
    public StateAuthorityReviewChoiceKind Choice { get; }

    public static StateAuthorityReviewChoice Approve(int mutationIndex) =>
        Create(mutationIndex, StateAuthorityReviewChoiceKind.Approve);

    public static StateAuthorityReviewChoice Reject(int mutationIndex) =>
        Create(mutationIndex, StateAuthorityReviewChoiceKind.Reject);

    private static StateAuthorityReviewChoice Create(
        int mutationIndex,
        StateAuthorityReviewChoiceKind choice)
    {
        if (mutationIndex < 0)
        {
            throw new StateAuthorityException(
                "State Authority review choice mutation index must be nonnegative.");
        }

        return new StateAuthorityReviewChoice(mutationIndex, choice);
    }
}

public sealed class StateAuthorityReviewSet
{
    internal StateAuthorityReviewSet(
        string proposalContentIdentityContract,
        string proposalContentHash,
        ImmutableArray<StateAuthorityReviewChoice> choices)
    {
        ContractVersion = StateAuthorityContracts.ReviewSetContractVersion;
        ProposalContentIdentityContract = proposalContentIdentityContract;
        ProposalContentHash = proposalContentHash;
        Choices = choices;
    }

    public string ContractVersion { get; }
    public string ProposalContentIdentityContract { get; }
    public string ProposalContentHash { get; }
    public ImmutableArray<StateAuthorityReviewChoice> Choices { get; }

    public static StateAuthorityReviewSet Bind(
        StateAuthorityInput input,
        ImmutableArray<StateAuthorityReviewChoice> choices)
    {
        if (input is null)
        {
            throw new StateAuthorityException("State Authority input is required for review binding.");
        }

        if (choices.IsDefault)
        {
            throw new StateAuthorityException("State Authority review choices are required.");
        }

        if (choices.Length > input.Mutations.Length)
        {
            throw new StateAuthorityException(
                "State Authority review choice count exceeds the mutation count.");
        }

        var seen = new HashSet<int>();
        var canonical = ImmutableArray.CreateBuilder<StateAuthorityReviewChoice>(choices.Length);
        foreach (var choice in choices)
        {
            if (choice is null || !Enum.IsDefined(choice.Choice))
            {
                throw new StateAuthorityException(
                    "State Authority review set contains an invalid choice.");
            }

            if (choice.MutationIndex < 0 || choice.MutationIndex >= input.Mutations.Length)
            {
                throw new StateAuthorityException(
                    "State Authority review choice mutation index is out of range.");
            }

            if (!seen.Add(choice.MutationIndex))
            {
                throw new StateAuthorityException(
                    "State Authority review set contains duplicate mutation indices.");
            }

            canonical.Add(choice);
        }

        return new StateAuthorityReviewSet(
            input.ProposalContentIdentityContract,
            input.ProposalContentHash,
            canonical
                .ToImmutable()
                .OrderBy(choice => choice.MutationIndex)
                .ToImmutableArray());
    }
}

public enum StateAuthorityDisposition
{
    Approved,
    Rejected,
    RequiresReview
}

public enum StateAuthorityReasonCode
{
    SupportingRecordMissing,
    ExistingRecordMissing,
    ExistingRecordInactive,
    ExistingRecordDomainMismatch,
    ExistingRecordSubjectMismatch,
    ExistingRecordTargetMismatch,
    ExistingRecordProtected,
    ConflictingExistingRecordTarget,
    MandatoryReview,
    PolicyReviewRequired,
    PolicyAutoApproved,
    ExplicitReviewApproved,
    ExplicitReviewRejected
}

public enum StateAuthorityEvaluationStatus
{
    Complete,
    ReviewRequired
}

public sealed class StateAuthorityDecision
{
    internal StateAuthorityDecision(
        int mutationIndex,
        StateAuthorityDisposition disposition,
        ImmutableArray<StateAuthorityReasonCode> reasons)
    {
        MutationIndex = mutationIndex;
        Disposition = disposition;
        Reasons = reasons;
    }

    public int MutationIndex { get; }
    public StateAuthorityDisposition Disposition { get; }
    public ImmutableArray<StateAuthorityReasonCode> Reasons { get; }
}

public sealed class StateAuthorityTrace
{
    internal StateAuthorityTrace(
        StateAuthorityInput input,
        StateAuthorityPolicy policy,
        StateAuthorityReviewSet reviewSet)
    {
        Input = input;
        Policy = policy;
        ReviewSet = reviewSet;
    }

    public StateAuthorityInput Input { get; }
    public StateAuthorityPolicy Policy { get; }
    public StateAuthorityReviewSet ReviewSet { get; }
}

public sealed class StateAuthorityEvaluation
{
    internal StateAuthorityEvaluation(
        StateAuthorityEvaluationStatus status,
        ImmutableArray<StateAuthorityDecision> decisions,
        StateAuthorityTrace trace)
    {
        ContractVersion = StateAuthorityContracts.ContractVersion;
        Status = status;
        Decisions = decisions;
        Trace = trace;
    }

    public string ContractVersion { get; }
    public StateAuthorityEvaluationStatus Status { get; }
    public ImmutableArray<StateAuthorityDecision> Decisions { get; }
    public StateAuthorityTrace Trace { get; }
}

public sealed class StateAuthorityException : Exception
{
    public StateAuthorityException(string message)
        : base(message)
    {
    }

    public StateAuthorityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

internal static class StateAuthorityProposalCanonicalizer
{
    public static string Hash(StateInterpretationProposal proposal)
    {
        var bytes = Serialize(proposal);
        return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    }

    internal static byte[] Serialize(StateInterpretationProposal proposal)
    {
        var builder = new StringBuilder();
        builder.Append('{');
        var first = true;
        AppendStringProperty(builder, ref first, "identityContract", StateAuthorityContracts.ProposalContentIdentityContract);
        AppendStringProperty(builder, ref first, "contractVersion", proposal.ContractVersion);
        AppendStringProperty(builder, ref first, "candidateContentIdentityContract", proposal.CandidateContentIdentityContract);
        AppendStringProperty(builder, ref first, "candidateContentHash", proposal.CandidateContentHash);
        AppendStringProperty(builder, ref first, "sourceSceneId", proposal.SourceSceneId.Value);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "mutations");
        builder.Append('[');
        for (var index = 0; index < proposal.Mutations.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            AppendMutation(builder, proposal.Mutations[index]);
        }

        builder.Append(']');
        builder.Append('}');
        return CanonicalJson.EncodeUtf8(builder.ToString());
    }

    private static void AppendMutation(StringBuilder builder, StateMutationCandidate mutation)
    {
        var semantic = DescribeMutation(mutation);
        builder.Append('{');
        var first = true;
        AppendStringProperty(builder, ref first, "domain", DomainToken(mutation.Domain));
        AppendStringProperty(builder, ref first, "operation", semantic.Operation);
        AppendNullableStringProperty(builder, ref first, "subjectCharacterId", semantic.SubjectCharacterId);
        AppendNullableStringProperty(builder, ref first, "targetCharacterId", semantic.TargetCharacterId);
        AppendNullableStringProperty(builder, ref first, "existingRecordId", semantic.ExistingRecordId);
        AppendNullableStringProperty(builder, ref first, "text", semantic.Text);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "supportingRecordIds");
        builder.Append('[');
        for (var index = 0; index < mutation.SupportingRecordIds.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            CanonicalJson.AppendString(builder, mutation.SupportingRecordIds[index].Value);
        }

        builder.Append(']');
        builder.Append('}');
    }

    private static CanonicalMutation DescribeMutation(StateMutationCandidate mutation) =>
        mutation switch
        {
            GlobalStateMutationCandidate global =>
                FromChange(global.Change, null, null),
            AppendOnlyCharacterStateMutationCandidate appendOnly =>
                FromChange(appendOnly.Change, appendOnly.SubjectCharacterId.Value, null),
            MutableCharacterStateMutationCandidate mutable =>
                FromChange(mutable.Change, mutable.SubjectCharacterId.Value, null),
            CharacterClaimMutationCandidate claim =>
                new CanonicalMutation("add", claim.SubjectCharacterId.Value, null, null, claim.Text),
            RelationshipStateMutationCandidate relationship =>
                FromChange(
                    relationship.Change,
                    relationship.SubjectCharacterId.Value,
                    relationship.TargetCharacterId.Value),
            _ => throw new StateAuthorityException(
                "State Authority cannot canonicalize an unsupported mutation shape.")
        };

    private static CanonicalMutation FromChange(
        StateMutationChange change,
        string? subjectCharacterId,
        string? targetCharacterId) =>
        change switch
        {
            AddStateMutationChange add =>
                new CanonicalMutation("add", subjectCharacterId, targetCharacterId, null, add.Text),
            SupersedeStateMutationChange supersede =>
                new CanonicalMutation(
                    "supersede",
                    subjectCharacterId,
                    targetCharacterId,
                    supersede.ExistingRecordId.Value,
                    supersede.Text),
            DeactivateStateMutationChange deactivate =>
                new CanonicalMutation(
                    "deactivate",
                    subjectCharacterId,
                    targetCharacterId,
                    deactivate.ExistingRecordId.Value,
                    null),
            _ => throw new StateAuthorityException(
                "State Authority cannot canonicalize an unsupported mutation transition.")
        };

    private static string DomainToken(StateMutationDomain domain) =>
        domain switch
        {
            StateMutationDomain.WorldState => "worldState",
            StateMutationDomain.SceneState => "sceneState",
            StateMutationDomain.UnresolvedProposition => "unresolvedProposition",
            StateMutationDomain.CharacterKnowledge => "characterKnowledge",
            StateMutationDomain.CharacterBelief => "characterBelief",
            StateMutationDomain.CharacterSuspicion => "characterSuspicion",
            StateMutationDomain.CharacterMemory => "characterMemory",
            StateMutationDomain.CharacterGoal => "characterGoal",
            StateMutationDomain.CharacterDisposition => "characterDisposition",
            StateMutationDomain.CharacterCircumstance => "characterCircumstance",
            StateMutationDomain.CharacterClaim => "characterClaim",
            StateMutationDomain.Relationship => "relationship",
            StateMutationDomain.Pressure => "pressure",
            _ => throw new StateAuthorityException(
                "State Authority cannot canonicalize an undefined mutation domain.")
        };

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

    private sealed record CanonicalMutation(
        string Operation,
        string? SubjectCharacterId,
        string? TargetCharacterId,
        string? ExistingRecordId,
        string? Text);
}
