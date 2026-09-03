using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Provenance;
using Ensemble.E0.Core.Serialization;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.CausalCommit;

public static class DeterministicCausalCommit
{
    public static E0CausalCommitResult Commit(
        CommitId commitId,
        ProductionState currentState,
        E0TakeStateBinding binding,
        E0RecordMaterializationSet materializations)
    {
        if (currentState is null)
        {
            throw new E0CausalCommitException(
                "Causal commit current Production state is required.");
        }

        if (binding is null)
        {
            throw new E0CausalCommitException(
                "Causal commit Accepted-Take binding is required.");
        }

        if (materializations is null)
        {
            throw new E0CausalCommitException(
                "Causal commit record materializations are required.");
        }

        RequireInitialized(commitId, "CommitId");
        ValidateAcceptedTake(binding.Take);

        StateHash currentHash;
        try
        {
            currentHash = currentState.StateHash;
            _ = currentHash.Value;
            _ = binding.SourceStateHash.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                "Causal commit StateHash is uninitialized.");
        }

        if (currentHash != binding.SourceStateHash)
        {
            throw new E0CausalCommitException(
                "Causal commit source Production state is stale.");
        }

        ValidateCurrentOpportunity(currentState, binding.Take);
        ValidateDuplicateIdentities(currentState, commitId, binding.Take.TakeId);
        ValidateMaterializations(currentState, binding.Take, materializations);

        try
        {
            var projection = CausalCommitTransition.Apply(
                currentState,
                binding.Take,
                materializations);
            var resultHash = CausalCommitCanonicalizer.ComputeResultHash(
                currentState.StateHash,
                commitId,
                binding.Take,
                materializations,
                projection);
            var committedEvent = new E0CausalCommit(
                commitId,
                currentState.StateHash,
                resultHash,
                binding.Take,
                materializations);
            var resultState = currentState.WithCommittedTransition(
                projection,
                resultHash,
                commitId,
                binding.Take.TakeId);
            return new E0CausalCommitResult(committedEvent, resultState);
        }
        catch (RecordProvenanceGraphException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit result provenance graph is invalid.",
                exception);
        }
        catch (CanonicalJsonException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit canonicalization failed.",
                exception);
        }
        catch (StateInterpretationException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit mutation canonicalization failed.",
                exception);
        }
    }

    public static ProductionState Replay(
        ProductionState parentState,
        E0CausalCommit committedEvent)
    {
        if (parentState is null)
        {
            throw new E0CausalCommitException(
                "Causal replay parent Production state is required.");
        }

        if (committedEvent is null)
        {
            throw new E0CausalCommitException(
                "Causal replay event is required.");
        }

        if (!string.Equals(
                committedEvent.ContractVersion,
                E0CausalCommitContracts.ContractVersion,
                StringComparison.Ordinal))
        {
            throw new E0CausalCommitException(
                "Causal replay event contract is unsupported.");
        }

        RequireInitialized(committedEvent.CommitId, "CommitId");
        ValidateAcceptedTake(committedEvent.Take);

        try
        {
            _ = parentState.StateHash.Value;
            _ = committedEvent.ParentStateHash.Value;
            _ = committedEvent.ResultStateHash.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                "Causal replay StateHash is uninitialized.");
        }

        if (parentState.StateHash != committedEvent.ParentStateHash)
        {
            throw new E0CausalCommitException(
                "Causal replay parent StateHash does not match the event.");
        }

        ValidateCurrentOpportunity(parentState, committedEvent.Take);
        ValidateDuplicateIdentities(
            parentState,
            committedEvent.CommitId,
            committedEvent.Take.TakeId);

        try
        {
            var parentSnapshot = ProductionStateAuthoritySnapshot.Bind(parentState);
            var takeSnapshot = committedEvent.Take.AuthorityEvaluation.Trace.Input.Snapshot;
            if (!StateAuthoritySnapshotSemanticComparer.Equals(parentSnapshot, takeSnapshot))
            {
                throw new E0CausalCommitException(
                    "Causal replay Take authority snapshot does not match the parent Production state.");
            }
        }
        catch (StateAuthorityException exception)
        {
            throw new E0CausalCommitException(
                "Causal replay parent State Authority projection failed.",
                exception);
        }

        ValidateMaterializations(
            parentState,
            committedEvent.Take,
            committedEvent.RecordMaterializations);

        try
        {
            var projection = CausalCommitTransition.Apply(
                parentState,
                committedEvent.Take,
                committedEvent.RecordMaterializations);
            var replayedHash = CausalCommitCanonicalizer.ComputeResultHash(
                parentState.StateHash,
                committedEvent.CommitId,
                committedEvent.Take,
                committedEvent.RecordMaterializations,
                projection);
            if (replayedHash != committedEvent.ResultStateHash)
            {
                throw new E0CausalCommitException(
                    "Causal replay result StateHash does not match the event.");
            }

            return parentState.WithCommittedTransition(
                projection,
                replayedHash,
                committedEvent.CommitId,
                committedEvent.Take.TakeId);
        }
        catch (RecordProvenanceGraphException exception)
        {
            throw new E0CausalCommitException(
                "Causal replay result provenance graph is invalid.",
                exception);
        }
        catch (CanonicalJsonException exception)
        {
            throw new E0CausalCommitException(
                "Causal replay canonicalization failed.",
                exception);
        }
        catch (StateInterpretationException exception)
        {
            throw new E0CausalCommitException(
                "Causal replay mutation canonicalization failed.",
                exception);
        }
    }

    private static void ValidateAcceptedTake(E0Take take)
    {
        if (take is null || take.Disposition != E0TakeDisposition.Accepted)
        {
            throw new E0CausalCommitException(
                "Causal commit requires an Accepted E0 Take.");
        }

        try
        {
            _ = take.TakeId.Value;
            _ = take.Performance.SubjectCharacterId.Value;
            _ = take.Performance.ContextPacketId.Value;
            _ = take.InterpretationProposal.SourceSceneId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                "Causal commit Take identity is uninitialized.");
        }

        var authority = take.AuthorityEvaluation;
        if (authority is null ||
            !string.Equals(
                authority.ContractVersion,
                StateAuthorityContracts.ContractVersion,
                StringComparison.Ordinal) ||
            authority.Status != StateAuthorityEvaluationStatus.Complete ||
            authority.Trace is null ||
            authority.Trace.Input is null ||
            authority.Trace.Input.Snapshot is null ||
            authority.Trace.Policy is null ||
            authority.Trace.ReviewSet is null ||
            authority.Decisions.IsDefault ||
            take.InterpretationProposal.Mutations.IsDefault ||
            authority.Decisions.Length != take.InterpretationProposal.Mutations.Length)
        {
            throw new E0CausalCommitException(
                "Causal commit Take authority package is invalid.");
        }

        for (var index = 0; index < authority.Decisions.Length; index++)
        {
            var decision = authority.Decisions[index];
            if (decision is null ||
                decision.MutationIndex != index ||
                decision.Disposition == StateAuthorityDisposition.RequiresReview)
            {
                throw new E0CausalCommitException(
                    "Causal commit Take authority decisions are not terminal and canonical.");
            }
        }
    }

    private static void ValidateCurrentOpportunity(
        ProductionState state,
        E0Take take)
    {
        if (!state.CurrentOpportunityCharacterId.HasValue ||
            state.CurrentOpportunityCharacterId.Value != take.Performance.SubjectCharacterId)
        {
            throw new E0CausalCommitException(
                "Causal commit Take subject does not match the current Production opportunity.");
        }
    }

    private static void ValidateDuplicateIdentities(
        ProductionState state,
        CommitId commitId,
        TakeId takeId)
    {
        if (state.ContainsEffectiveCommitId(commitId))
        {
            throw new E0CausalCommitException(
                "Causal commit CommitId is already effective in the parent state.");
        }

        if (state.ContainsCommittedTakeId(takeId))
        {
            throw new E0CausalCommitException(
                "Causal commit TakeId is already effective in the parent state.");
        }
    }

    private static void ValidateMaterializations(
        ProductionState state,
        E0Take take,
        E0RecordMaterializationSet materializations)
    {
        if (materializations is null || materializations.Items.IsDefault)
        {
            throw new E0CausalCommitException(
                "Causal commit record materialization set is invalid.");
        }

        var expected = ImmutableArray.CreateBuilder<int>();
        var decisions = take.AuthorityEvaluation.Decisions;
        for (var index = 0; index < decisions.Length; index++)
        {
            if (decisions[index].Disposition != StateAuthorityDisposition.Approved)
            {
                continue;
            }

            var mutation = take.InterpretationProposal.Mutations[index];
            if (MutationSemantics.Operation(mutation) is MutationOperation.Add or MutationOperation.Supersede)
            {
                expected.Add(index);
            }
        }

        if (materializations.Items.Length != expected.Count)
        {
            throw new E0CausalCommitException(
                "Causal commit record materializations do not match Approved mutation requirements.");
        }

        var parentIds = state.Records
            .Select(record => record.RecordId.Value)
            .ToHashSet(StringComparer.Ordinal);
        var seenIds = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < expected.Count; index++)
        {
            var item = materializations.Items[index];
            if (item is null || item.MutationIndex != expected[index])
            {
                throw new E0CausalCommitException(
                    "Causal commit record materialization indexes are not exact and canonical.");
            }

            string recordId;
            try
            {
                recordId = item.RecordId.Value;
            }
            catch (InvalidOperationException)
            {
                throw new E0CausalCommitException(
                    "Causal commit materialized RecordId is uninitialized.");
            }

            if (!seenIds.Add(recordId) || parentIds.Contains(recordId))
            {
                throw new E0CausalCommitException(
                    "Causal commit materialized RecordId collides with effective Production history.");
            }
        }
    }

    private static void RequireInitialized(CommitId id, string name)
    {
        try
        {
            _ = id.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                $"Causal commit {name} is uninitialized.");
        }
    }
}

internal static class CausalCommitTransition
{
    internal static ProductionStateProjection Apply(
        ProductionState parentState,
        E0Take take,
        E0RecordMaterializationSet materializations)
    {
        var byId = parentState.Records.ToDictionary(
            record => record.RecordId.Value,
            record => record,
            StringComparer.Ordinal);
        var materializationByIndex = materializations.Items.ToDictionary(
            item => item.MutationIndex,
            item => item.RecordId);
        var decisions = take.AuthorityEvaluation.Decisions;

        for (var index = 0; index < decisions.Length; index++)
        {
            var decision = decisions[index];
            if (decision.Disposition == StateAuthorityDisposition.Rejected)
            {
                continue;
            }

            if (decision.Disposition != StateAuthorityDisposition.Approved)
            {
                throw new E0CausalCommitException(
                    "Causal transition contains a nonterminal authority decision.");
            }

            ApplyApproved(
                byId,
                index,
                take.InterpretationProposal.Mutations[index],
                materializationByIndex);
        }

        var records = byId.Values
            .OrderBy(record => record.RecordId.Value, StringComparer.Ordinal)
            .ToImmutableArray();

        RecordProvenanceGraphValidator.Validate(
            records.Select(record =>
                new KeyValuePair<RecordId, ImmutableArray<RecordId>>(
                    record.RecordId,
                    record.Provenance)));

        return parentState.Projection with
        {
            CurrentOpportunityCharacterId = null,
            Records = records
        };
    }

    private static void ApplyApproved(
        Dictionary<string, ProductionRecord> byId,
        int mutationIndex,
        StateMutationCandidate mutation,
        Dictionary<int, RecordId> materializationByIndex)
    {
        var operation = MutationSemantics.Operation(mutation);
        var semantics = MutationSemantics.Describe(mutation);

        switch (operation)
        {
            case MutationOperation.Add:
            {
                if (!materializationByIndex.TryGetValue(mutationIndex, out var recordId))
                {
                    throw new E0CausalCommitException(
                        "Approved Add is missing its RecordId materialization.");
                }

                AddNew(byId, recordId, semantics, mutation.SupportingRecordIds);
                break;
            }
            case MutationOperation.Supersede:
            {
                if (!materializationByIndex.TryGetValue(mutationIndex, out var recordId) ||
                    !semantics.ExistingRecordId.HasValue)
                {
                    throw new E0CausalCommitException(
                        "Approved Supersede is missing required transition identity.");
                }

                var existing = RequireExactActiveTarget(byId, semantics);
                byId[existing.RecordId.Value] =
                    existing.WithLifecycle(ProductionRecordLifecycle.Inactive);
                AddNew(byId, recordId, semantics, mutation.SupportingRecordIds);
                break;
            }
            case MutationOperation.Deactivate:
            {
                if (!semantics.ExistingRecordId.HasValue)
                {
                    throw new E0CausalCommitException(
                        "Approved Deactivate is missing its transition target.");
                }

                var existing = RequireExactActiveTarget(byId, semantics);
                byId[existing.RecordId.Value] =
                    existing.WithLifecycle(ProductionRecordLifecycle.Inactive);
                break;
            }
            default:
                throw new E0CausalCommitException(
                    "Causal transition mutation operation is unsupported.");
        }
    }

    private static ProductionRecord RequireExactActiveTarget(
        Dictionary<string, ProductionRecord> byId,
        MutationDescriptor semantics)
    {
        var existingId = semantics.ExistingRecordId!.Value.Value;
        if (!byId.TryGetValue(existingId, out var existing) ||
            existing.Lifecycle != ProductionRecordLifecycle.Active)
        {
            throw new E0CausalCommitException(
                "Causal transition target is missing or inactive.");
        }

        if (existing.Domain != semantics.Domain)
        {
            throw new E0CausalCommitException(
                "Causal transition target domain does not match the Approved mutation.");
        }

        switch (semantics.Scope)
        {
            case MutationScope.Global when existing is not GlobalProductionRecord:
                throw new E0CausalCommitException(
                    "Causal transition target scope does not match the Approved mutation.");
            case MutationScope.Character:
                if (existing is not CharacterProductionRecord character ||
                    !semantics.SubjectCharacterId.HasValue ||
                    character.SubjectCharacterId != semantics.SubjectCharacterId.Value)
                {
                    throw new E0CausalCommitException(
                        "Causal transition target Character scope does not match the Approved mutation.");
                }

                break;
            case MutationScope.Relationship:
                if (existing is not RelationshipProductionRecord relationship ||
                    !semantics.SubjectCharacterId.HasValue ||
                    !semantics.TargetCharacterId.HasValue ||
                    relationship.SubjectCharacterId != semantics.SubjectCharacterId.Value ||
                    relationship.TargetCharacterId != semantics.TargetCharacterId.Value)
                {
                    throw new E0CausalCommitException(
                        "Causal transition target relationship scope does not match the Approved mutation.");
                }

                break;
        }

        return existing;
    }

    private static void AddNew(
        Dictionary<string, ProductionRecord> byId,
        RecordId recordId,
        MutationDescriptor semantics,
        ImmutableArray<RecordId> provenance)
    {
        if (byId.ContainsKey(recordId.Value))
        {
            throw new E0CausalCommitException(
                "Causal transition cannot reuse a Production RecordId.");
        }

        if (semantics.Text is null)
        {
            throw new E0CausalCommitException(
                "Causal transition Add/Supersede text is missing.");
        }

        ProductionRecord record = semantics.Scope switch
        {
            MutationScope.Global => new GlobalProductionRecord(
                recordId,
                semantics.Domain,
                ProductionRecordLifecycle.Active,
                ProductionRecordProtection.None,
                semantics.Text,
                provenance),
            MutationScope.Character when semantics.SubjectCharacterId.HasValue =>
                new CharacterProductionRecord(
                    recordId,
                    semantics.Domain,
                    semantics.SubjectCharacterId.Value,
                    ProductionRecordLifecycle.Active,
                    ProductionRecordProtection.None,
                    semantics.Text,
                    provenance),
            MutationScope.Relationship when
                semantics.SubjectCharacterId.HasValue &&
                semantics.TargetCharacterId.HasValue =>
                new RelationshipProductionRecord(
                    recordId,
                    semantics.SubjectCharacterId.Value,
                    semantics.TargetCharacterId.Value,
                    ProductionRecordLifecycle.Active,
                    ProductionRecordProtection.None,
                    semantics.Text,
                    provenance),
            _ => throw new E0CausalCommitException(
                "Causal transition mutation scope is invalid.")
        };

        byId.Add(recordId.Value, record);
    }
}

internal enum MutationOperation
{
    Add,
    Supersede,
    Deactivate
}

internal enum MutationScope
{
    Global,
    Character,
    Relationship
}

internal sealed record MutationDescriptor(
    ProductionRecordDomain Domain,
    MutationScope Scope,
    CharacterId? SubjectCharacterId,
    CharacterId? TargetCharacterId,
    RecordId? ExistingRecordId,
    string? Text);

internal static class MutationSemantics
{
    internal static MutationOperation Operation(StateMutationCandidate mutation) =>
        mutation switch
        {
            CharacterClaimMutationCandidate => MutationOperation.Add,
            GlobalStateMutationCandidate global => FromChange(global.Change),
            AppendOnlyCharacterStateMutationCandidate appendOnly => FromChange(appendOnly.Change),
            MutableCharacterStateMutationCandidate mutable => FromChange(mutable.Change),
            RelationshipStateMutationCandidate relationship => FromChange(relationship.Change),
            _ => throw new E0CausalCommitException(
                "Causal transition mutation shape is unsupported.")
        };

    internal static MutationDescriptor Describe(StateMutationCandidate mutation) =>
        mutation switch
        {
            GlobalStateMutationCandidate global =>
                FromChangeDescriptor(
                    MapDomain(global.Domain),
                    MutationScope.Global,
                    null,
                    null,
                    global.Change),
            AppendOnlyCharacterStateMutationCandidate appendOnly =>
                FromChangeDescriptor(
                    MapDomain(appendOnly.Domain),
                    MutationScope.Character,
                    appendOnly.SubjectCharacterId,
                    null,
                    appendOnly.Change),
            MutableCharacterStateMutationCandidate mutable =>
                FromChangeDescriptor(
                    MapDomain(mutable.Domain),
                    MutationScope.Character,
                    mutable.SubjectCharacterId,
                    null,
                    mutable.Change),
            CharacterClaimMutationCandidate claim =>
                new MutationDescriptor(
                    ProductionRecordDomain.CharacterClaim,
                    MutationScope.Character,
                    claim.SubjectCharacterId,
                    null,
                    null,
                    claim.Text),
            RelationshipStateMutationCandidate relationship =>
                FromChangeDescriptor(
                    ProductionRecordDomain.Relationship,
                    MutationScope.Relationship,
                    relationship.SubjectCharacterId,
                    relationship.TargetCharacterId,
                    relationship.Change),
            _ => throw new E0CausalCommitException(
                "Causal transition mutation shape is unsupported.")
        };

    private static MutationDescriptor FromChangeDescriptor(
        ProductionRecordDomain domain,
        MutationScope scope,
        CharacterId? subject,
        CharacterId? target,
        StateMutationChange change) =>
        change switch
        {
            AddStateMutationChange add =>
                new MutationDescriptor(domain, scope, subject, target, null, add.Text),
            SupersedeStateMutationChange supersede =>
                new MutationDescriptor(
                    domain,
                    scope,
                    subject,
                    target,
                    supersede.ExistingRecordId,
                    supersede.Text),
            DeactivateStateMutationChange deactivate =>
                new MutationDescriptor(
                    domain,
                    scope,
                    subject,
                    target,
                    deactivate.ExistingRecordId,
                    null),
            _ => throw new E0CausalCommitException(
                "Causal transition mutation change is unsupported.")
        };

    private static MutationOperation FromChange(StateMutationChange change) =>
        change switch
        {
            AddStateMutationChange => MutationOperation.Add,
            SupersedeStateMutationChange => MutationOperation.Supersede,
            DeactivateStateMutationChange => MutationOperation.Deactivate,
            _ => throw new E0CausalCommitException(
                "Causal transition mutation change is unsupported.")
        };

    internal static ProductionRecordDomain MapDomain(StateMutationDomain domain) =>
        domain switch
        {
            StateMutationDomain.WorldState => ProductionRecordDomain.WorldState,
            StateMutationDomain.SceneState => ProductionRecordDomain.SceneState,
            StateMutationDomain.UnresolvedProposition => ProductionRecordDomain.UnresolvedProposition,
            StateMutationDomain.CharacterKnowledge => ProductionRecordDomain.CharacterKnowledge,
            StateMutationDomain.CharacterBelief => ProductionRecordDomain.CharacterBelief,
            StateMutationDomain.CharacterSuspicion => ProductionRecordDomain.CharacterSuspicion,
            StateMutationDomain.CharacterMemory => ProductionRecordDomain.CharacterMemory,
            StateMutationDomain.CharacterGoal => ProductionRecordDomain.CharacterGoal,
            StateMutationDomain.CharacterDisposition => ProductionRecordDomain.CharacterDisposition,
            StateMutationDomain.CharacterCircumstance => ProductionRecordDomain.CharacterCircumstance,
            StateMutationDomain.CharacterClaim => ProductionRecordDomain.CharacterClaim,
            StateMutationDomain.Relationship => ProductionRecordDomain.Relationship,
            StateMutationDomain.Pressure => ProductionRecordDomain.Pressure,
            _ => throw new E0CausalCommitException(
                "Causal transition mutation domain is unsupported.")
        };
}
