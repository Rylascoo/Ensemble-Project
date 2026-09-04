using System.Collections.Immutable;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.CausalCommit;

public static class E0CausalCommitContracts
{
    public const string ContractVersion = "ensemble.e0.causal-commit.v1";
}

public sealed class ProductionStateCheckpoint
{
    private readonly ProductionState _sourceState;

    private ProductionStateCheckpoint(
        ProductionState sourceState,
        StateHash stateHash,
        SceneId sceneId,
        CharacterId currentOpportunityCharacterId)
    {
        _sourceState = sourceState;
        StateHash = stateHash;
        SceneId = sceneId;
        CurrentOpportunityCharacterId = currentOpportunityCharacterId;
    }

    public StateHash StateHash { get; }
    public SceneId SceneId { get; }
    public CharacterId CurrentOpportunityCharacterId { get; }

    internal ProductionState SourceState => _sourceState;

    public static ProductionStateCheckpoint Capture(ProductionState sourceState)
    {
        if (sourceState is null)
        {
            throw new E0CausalCommitException(
                "Causal commit source Production state is required.");
        }

        try
        {
            _ = sourceState.StateHash.Value;
            _ = sourceState.SceneId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                "Causal commit source Production state identity is uninitialized.");
        }

        var opportunity = sourceState.CurrentOpportunityCharacterId;
        if (!opportunity.HasValue)
        {
            throw new E0CausalCommitException(
                "Causal commit source Production state has no current opportunity.");
        }

        try
        {
            _ = opportunity.Value.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                "Causal commit source current opportunity is uninitialized.");
        }

        if (!sourceState.RosterCharacterIds.Contains(opportunity.Value))
        {
            throw new E0CausalCommitException(
                "Causal commit source current opportunity is outside the Scene roster.");
        }

        return new ProductionStateCheckpoint(
            sourceState,
            sourceState.StateHash,
            sourceState.SceneId,
            opportunity.Value);
    }
}

public sealed class E0TakeStateBinding
{
    private E0TakeStateBinding(StateHash sourceStateHash, E0Take take)
    {
        SourceStateHash = sourceStateHash;
        Take = take;
    }

    public StateHash SourceStateHash { get; }
    public E0Take Take { get; }

    public static E0TakeStateBinding Bind(
        ProductionStateCheckpoint sourceCheckpoint,
        ContextPacket sourceContext,
        E0Take take)
    {
        if (sourceCheckpoint is null)
        {
            throw new E0CausalCommitException(
                "Causal commit source checkpoint is required.");
        }

        if (sourceContext is null)
        {
            throw new E0CausalCommitException(
                "Causal commit source ContextPacket is required.");
        }

        if (take is null)
        {
            throw new E0CausalCommitException(
                "Causal commit Take is required.");
        }

        if (take.Disposition != E0TakeDisposition.Accepted)
        {
            throw new E0CausalCommitException(
                "Only an Accepted E0 Take may bind to Production state.");
        }

        try
        {
            _ = sourceCheckpoint.StateHash.Value;
            _ = sourceCheckpoint.SceneId.Value;
            _ = sourceCheckpoint.CurrentOpportunityCharacterId.Value;
            _ = sourceContext.ContextPacketId.Value;
            _ = sourceContext.SceneId.Value;
            _ = sourceContext.SubjectCharacterId.Value;
            _ = sourceContext.OpportunityCharacterId.Value;
            _ = take.TakeId.Value;
            _ = take.Performance.ContextPacketId.Value;
            _ = take.Performance.SubjectCharacterId.Value;
            _ = take.InterpretationProposal.SourceSceneId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                "Causal commit source identity is uninitialized.");
        }

        if (take.Performance.ContextPacketId != sourceContext.ContextPacketId ||
            take.Performance.SubjectCharacterId != sourceContext.SubjectCharacterId ||
            sourceContext.SubjectCharacterId != sourceContext.OpportunityCharacterId ||
            sourceContext.SubjectCharacterId != sourceCheckpoint.CurrentOpportunityCharacterId)
        {
            throw new E0CausalCommitException(
                "Causal commit Take and source opportunity association is invalid.");
        }

        if (sourceContext.SceneId != sourceCheckpoint.SceneId ||
            take.InterpretationProposal.SourceSceneId != sourceCheckpoint.SceneId)
        {
            throw new E0CausalCommitException(
                "Causal commit Take and source Scene association is invalid.");
        }

        var sourceRoster = CanonicalizeContextRoster(sourceContext);
        if (!sourceRoster.SequenceEqual(sourceCheckpoint.SourceState.RosterCharacterIds))
        {
            throw new E0CausalCommitException(
                "Causal commit Context roster does not match the source Production state.");
        }

        ValidateExactSourceContext(sourceCheckpoint, sourceContext);

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
            authority.Decisions.IsDefault ||
            take.InterpretationProposal.Mutations.IsDefault ||
            authority.Decisions.Length != take.InterpretationProposal.Mutations.Length)
        {
            throw new E0CausalCommitException(
                "Causal commit Take authority package is not terminal and complete.");
        }

        for (var index = 0; index < authority.Decisions.Length; index++)
        {
            var decision = authority.Decisions[index];
            if (decision is null ||
                decision.MutationIndex != index ||
                decision.Disposition == StateAuthorityDisposition.RequiresReview)
            {
                throw new E0CausalCommitException(
                    "Causal commit Take authority decisions are invalid.");
            }
        }

        try
        {
            var sourceSnapshot = ProductionStateAuthoritySnapshot.Bind(
                sourceCheckpoint.SourceState);
            if (!StateAuthoritySnapshotSemanticComparer.Equals(
                    sourceSnapshot,
                    authority.Trace.Input.Snapshot))
            {
                throw new E0CausalCommitException(
                    "Causal commit Take authority snapshot does not match source Production state.");
            }
        }
        catch (StateAuthorityException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit source State Authority projection failed.",
                exception);
        }

        return new E0TakeStateBinding(sourceCheckpoint.StateHash, take);
    }

    private static void ValidateExactSourceContext(
        ProductionStateCheckpoint sourceCheckpoint,
        ContextPacket sourceContext)
    {
        try
        {
            if (IsProductionBoundV2(sourceContext))
            {
                if (!sourceContext.SourceStateHash.HasValue ||
                    sourceContext.SourceStateHash.Value != sourceCheckpoint.StateHash)
                {
                    throw new E0CausalCommitException(
                        "Causal commit production-bound Context source state does not match the checkpoint.");
                }

                var access = CharacterBoundedAccessControl.Evaluate(
                    sourceCheckpoint.SourceState,
                    sourceCheckpoint.CurrentOpportunityCharacterId);
                var expected = DeterministicContextComposer.ComposeProductionBound(
                    access.Projection,
                    sourceCheckpoint.CurrentOpportunityCharacterId).Packet;
                RequireExactContext(expected, sourceContext);
                return;
            }

            if (IsHistoricalV1(sourceContext))
            {
                var genesisHash = ProductionStateCanonicalizer.ComputeGenesisHash(
                    sourceCheckpoint.SourceState.Projection);
                if (genesisHash != sourceCheckpoint.StateHash)
                {
                    throw new E0CausalCommitException(
                        "Causal commit historical Context v1 is permitted only for exact genesis Production state.");
                }

                var access = CharacterBoundedAccessControl.Evaluate(
                    sourceCheckpoint.SourceState,
                    sourceCheckpoint.CurrentOpportunityCharacterId);
                var compatibilityProjection = WithoutSourceStateHash(access.Projection);
                var expected = DeterministicContextComposer.Compose(
                    compatibilityProjection,
                    sourceCheckpoint.CurrentOpportunityCharacterId).Packet;
                RequireExactContext(expected, sourceContext);
                return;
            }

            throw new E0CausalCommitException(
                "Causal commit source Context contract combination is unsupported.");
        }
        catch (E0CausalCommitException)
        {
            throw;
        }
        catch (CharacterAccessException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit source Production Access proof failed.",
                exception);
        }
        catch (ContextCompositionException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit source Context recomposition proof failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit source Context derivation identity is invalid.",
                exception);
        }
    }

    private static bool IsHistoricalV1(ContextPacket context) =>
        string.Equals(
            context.SchemaVersion,
            E0ContextContracts.SchemaVersion,
            StringComparison.Ordinal) &&
        string.Equals(
            context.CompositionContract,
            E0ContextContracts.CompositionContract,
            StringComparison.Ordinal);

    private static bool IsProductionBoundV2(ContextPacket context) =>
        string.Equals(
            context.SchemaVersion,
            E0ContextContracts.ProductionBoundSchemaVersion,
            StringComparison.Ordinal) &&
        string.Equals(
            context.CompositionContract,
            E0ContextContracts.ProductionBoundCompositionContract,
            StringComparison.Ordinal);

    private static CharacterAccessProjection WithoutSourceStateHash(
        CharacterAccessProjection projection) =>
        new(
            sourceStateHash: null,
            projection.SceneId,
            projection.SubjectCharacterId,
            projection.Roster,
            projection.SceneState,
            projection.Pressures,
            projection.Constitution,
            projection.Disposition,
            projection.Circumstance,
            projection.Observations,
            projection.Knowledge,
            projection.Beliefs,
            projection.Suspicions,
            projection.Memories,
            projection.Goals,
            projection.Relationships);

    private static void RequireExactContext(
        ContextPacket expected,
        ContextPacket actual)
    {
        if (expected.ContextPacketId != actual.ContextPacketId ||
            !string.Equals(expected.SchemaVersion, actual.SchemaVersion, StringComparison.Ordinal) ||
            !string.Equals(expected.CompositionContract, actual.CompositionContract, StringComparison.Ordinal) ||
            expected.SourceStateHash != actual.SourceStateHash ||
            expected.SceneId != actual.SceneId ||
            expected.SubjectCharacterId != actual.SubjectCharacterId ||
            expected.OpportunityCharacterId != actual.OpportunityCharacterId ||
            !string.Equals(
                expected.StructuredContextHash,
                actual.StructuredContextHash,
                StringComparison.Ordinal) ||
            !string.Equals(
                expected.RenderedContextHash,
                actual.RenderedContextHash,
                StringComparison.Ordinal) ||
            !ContextPacketCanonicalizer.SerializeStructured(expected)
                .AsSpan()
                .SequenceEqual(ContextPacketCanonicalizer.SerializeStructured(actual)) ||
            !ContextPacketCanonicalizer.SerializeRendered(expected.Rendered)
                .AsSpan()
                .SequenceEqual(ContextPacketCanonicalizer.SerializeRendered(actual.Rendered)))
        {
            throw new E0CausalCommitException(
                "Causal commit source Context was not derived exactly from the checkpoint Production state.");
        }
    }

    private static ImmutableArray<CharacterId> CanonicalizeContextRoster(ContextPacket context)
    {
        if (context.Roster.IsDefault || context.Roster.Length != 3)
        {
            throw new E0CausalCommitException(
                "Causal commit Context roster is invalid.");
        }

        var values = new HashSet<string>(StringComparer.Ordinal);
        var ids = ImmutableArray.CreateBuilder<CharacterId>(context.Roster.Length);
        foreach (var participant in context.Roster)
        {
            if (participant is null)
            {
                throw new E0CausalCommitException(
                    "Causal commit Context roster contains an invalid participant.");
            }

            string value;
            try
            {
                value = participant.CharacterId.Value;
            }
            catch (InvalidOperationException)
            {
                throw new E0CausalCommitException(
                    "Causal commit Context roster contains an uninitialized Character ID.");
            }

            if (!values.Add(value))
            {
                throw new E0CausalCommitException(
                    "Causal commit Context roster contains duplicate Character IDs.");
            }

            ids.Add(participant.CharacterId);
        }

        return ids
            .ToImmutable()
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }
}

public sealed class E0RecordMaterialization
{
    private E0RecordMaterialization(int mutationIndex, RecordId recordId)
    {
        MutationIndex = mutationIndex;
        RecordId = recordId;
    }

    public int MutationIndex { get; }
    public RecordId RecordId { get; }

    public static E0RecordMaterialization Create(int mutationIndex, RecordId recordId)
    {
        if (mutationIndex < 0)
        {
            throw new E0CausalCommitException(
                "Record materialization mutation index must be nonnegative.");
        }

        try
        {
            _ = recordId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0CausalCommitException(
                "Record materialization RecordId is uninitialized.");
        }

        return new E0RecordMaterialization(mutationIndex, recordId);
    }
}

public sealed class E0RecordMaterializationSet
{
    private E0RecordMaterializationSet(ImmutableArray<E0RecordMaterialization> items) =>
        Items = items;

    public ImmutableArray<E0RecordMaterialization> Items { get; }

    public static E0RecordMaterializationSet Bind(
        ImmutableArray<E0RecordMaterialization> items)
    {
        if (items.IsDefault)
        {
            throw new E0CausalCommitException(
                "Record materialization input is required.");
        }

        var indexes = new HashSet<int>();
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var item in items)
        {
            if (item is null || item.MutationIndex < 0)
            {
                throw new E0CausalCommitException(
                    "Record materialization set contains an invalid item.");
            }

            string recordId;
            try
            {
                recordId = item.RecordId.Value;
            }
            catch (InvalidOperationException)
            {
                throw new E0CausalCommitException(
                    "Record materialization set contains an uninitialized RecordId.");
            }

            if (!indexes.Add(item.MutationIndex))
            {
                throw new E0CausalCommitException(
                    "Record materialization set contains a duplicate mutation index.");
            }

            if (!ids.Add(recordId))
            {
                throw new E0CausalCommitException(
                    "Record materialization set contains a duplicate RecordId.");
            }
        }

        return new E0RecordMaterializationSet(
            items.OrderBy(item => item.MutationIndex).ToImmutableArray());
    }
}

public sealed class E0CausalCommit
{
    internal E0CausalCommit(
        CommitId commitId,
        StateHash parentStateHash,
        StateHash resultStateHash,
        E0Take take,
        E0RecordMaterializationSet recordMaterializations)
    {
        ContractVersion = E0CausalCommitContracts.ContractVersion;
        CommitId = commitId;
        ParentStateHash = parentStateHash;
        ResultStateHash = resultStateHash;
        Take = take;
        RecordMaterializations = recordMaterializations;
    }

    public string ContractVersion { get; }
    public CommitId CommitId { get; }
    public StateHash ParentStateHash { get; }
    public StateHash ResultStateHash { get; }
    public E0Take Take { get; }
    public E0RecordMaterializationSet RecordMaterializations { get; }
}

public sealed class E0CausalCommitResult
{
    internal E0CausalCommitResult(E0CausalCommit commit, ProductionState resultState)
    {
        Commit = commit;
        ResultState = resultState;
    }

    public E0CausalCommit Commit { get; }
    public ProductionState ResultState { get; }
}

public sealed class E0CausalCommitException : Exception
{
    internal E0CausalCommitException(string message)
        : base(message)
    {
    }

    internal E0CausalCommitException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
