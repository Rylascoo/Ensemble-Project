using System.Collections.Immutable;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Serialization;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Experiments.E0D;

public sealed class E0DOpportunityResult
{
    internal E0DOpportunityResult(
        E0OpportunityBearingCycleState state,
        E0OpportunityTransition @event,
        string strategyContract,
        E0DRoundRobinDirectorEvaluation? directorEvaluation)
    {
        State = state;
        Event = @event;
        StrategyContract = strategyContract;
        DirectorEvaluation = directorEvaluation;
    }

    public E0OpportunityBearingCycleState State { get; }
    public E0OpportunityTransition Event { get; }
    public string StrategyContract { get; }
    public E0DRoundRobinDirectorEvaluation? DirectorEvaluation { get; }
}

public static class E0DExperimentalCycle
{
    public static E0ProductionContextContinuityResult ComposeContext(
        E0OpportunityBearingCycleState source,
        E0DExperimentVariant variant)
    {
        ArgumentNullException.ThrowIfNull(source);
        return variant switch
        {
            E0DExperimentVariant.FullReference or E0DExperimentVariant.RoundRobin =>
                DeterministicE0CausalCycle.ComposeContext(source),
            E0DExperimentVariant.RelationshipsOmitted => ComposeRelationshipsOmitted(source),
            E0DExperimentVariant.OmniscientContext => ComposeOmniscient(source),
            _ => throw new E0DExperimentException("E0-D context variant is unsupported.")
        };
    }

    public static E0PostCommitCycleState CommitAcceptedTake(
        CommitId commitId,
        E0OpportunityBearingCycleState source,
        ContextPacket sourceContext,
        E0Take acceptedTake,
        E0RecordMaterializationSet materializations,
        E0DExperimentVariant variant)
    {
        if (variant is not E0DExperimentVariant.RelationshipsOmitted and
            not E0DExperimentVariant.OmniscientContext)
        {
            throw new E0DExperimentException(
                "E0-D experimental commit path is only valid for context ablations.");
        }
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sourceContext);
        ArgumentNullException.ThrowIfNull(acceptedTake);
        ArgumentNullException.ThrowIfNull(materializations);

        try
        {
            var expectedContext = ComposeContext(source, variant).ContextEvaluation.Packet;
            var checkpoint = ProductionStateCheckpoint.Capture(source.ProductionState);
            var binding = E0TakeStateBinding.BindWithAcceptedHistoryE0D(
                checkpoint,
                sourceContext,
                acceptedTake,
                source.AcceptedPerformanceHistory,
                expectedContext);
            var commitResult = DeterministicCausalCommit.Commit(
                commitId,
                source.ProductionState,
                binding,
                materializations);
            var acceptedHistory = E0DAcceptedPerformanceHistoryContinuity.RecordCommit(
                source.AcceptedPerformanceHistory,
                source.ProductionState,
                commitResult.Commit,
                expectedContext);
            return E0PostCommitCycleState.Create(
                commitResult.ResultState,
                acceptedHistory,
                commitResult.Commit,
                sourceContext,
                source.OpportunityHistory);
        }
        catch (Exception exception) when (
            exception is E0CausalCommitException or
            E0AcceptedPerformanceHistoryException or
            E0CausalCycleInvariantException or
            E0DExperimentException)
        {
            throw new E0DExperimentException(
                "E0-D context-ablation accepted Take commit failed.",
                exception);
        }
    }

    public static E0DRoundRobinDirectorEvaluation EvaluateRoundRobin(DirectorOpportunityInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        var (canonicalRoster, sourceIndex, selected) = SelectRoundRobin(input);
        var proposal = new DirectorOpportunityProposal(
            E0DirectorContracts.OpportunityContractVersion,
            input.SceneId,
            input.SourceCharacterId,
            selected);
        var trace = new E0DRoundRobinDirectorTrace(
            E0DExperimentContracts.RoundRobinStrategyContract,
            input,
            canonicalRoster,
            sourceIndex);
        return new E0DRoundRobinDirectorEvaluation(proposal, trace);
    }

    public static E0DOpportunityResult EstablishOpportunity(
        E0PostCommitCycleState source,
        E0DExperimentVariant variant)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (variant != E0DExperimentVariant.RoundRobin)
        {
            var reference = DeterministicE0CausalCycle.EstablishOpportunity(source);
            return new E0DOpportunityResult(
                reference.State,
                reference.OpportunityEvent,
                E0DirectorContracts.LeastInterventionStrategyContract,
                directorEvaluation: null);
        }

        return EstablishRoundRobin(source);
    }

    private static E0ProductionContextContinuityResult ComposeRelationshipsOmitted(
        E0OpportunityBearingCycleState source)
    {
        var reference = DeterministicE0CausalCycle.ComposeContext(source);
        var projection = reference.AccessEvaluation.Projection;
        var excluded = MissingRaftContract.RelationshipContextRecordIds;
        var experimental = new CharacterAccessProjection(
            projection.SourceStateHash,
            projection.SceneId,
            projection.SubjectCharacterId,
            projection.Roster,
            Filter(projection.SceneState, excluded),
            Filter(projection.Pressures, excluded),
            Filter(projection.Constitution, excluded),
            Filter(projection.Disposition, excluded),
            Filter(projection.Circumstance, excluded),
            Filter(projection.Observations, excluded),
            Filter(projection.Knowledge, excluded),
            Filter(projection.Beliefs, excluded),
            Filter(projection.Suspicions, excluded),
            Filter(projection.Memories, excluded),
            Filter(projection.Goals, excluded),
            Filter(projection.Relationships, excluded));

        var context = E0DExperimentalContextComposer.Compose(
            experimental,
            projection.SubjectCharacterId,
            reference.ContextEvaluation.Packet.RecentPerformances,
            E0DExperimentContracts.RelationshipsOmittedCompositionContract);
        return new E0ProductionContextContinuityResult(reference.AccessEvaluation, context);
    }

    private static E0ProductionContextContinuityResult ComposeOmniscient(
        E0OpportunityBearingCycleState source)
    {
        var reference = DeterministicE0CausalCycle.ComposeContext(source);
        var referenceProjection = reference.AccessEvaluation.Projection;
        var evaluations = source.ProductionState.RosterCharacterIds
            .Select(id => CharacterBoundedAccessControl.Evaluate(source.ProductionState, id))
            .ToImmutableArray();
        ValidateUnionInputs(referenceProjection, evaluations);

        var experimental = new CharacterAccessProjection(
            referenceProjection.SourceStateHash,
            referenceProjection.SceneId,
            referenceProjection.SubjectCharacterId,
            referenceProjection.Roster,
            UnionRecords(evaluations.Select(x => x.Projection.SceneState), "SceneState"),
            UnionRecords(evaluations.Select(x => x.Projection.Pressures), "Pressures"),
            UnionRecords(evaluations.Select(x => x.Projection.Constitution), "Constitution"),
            UnionRecords(evaluations.Select(x => x.Projection.Disposition), "Disposition"),
            UnionRecords(evaluations.Select(x => x.Projection.Circumstance), "Circumstance"),
            UnionRecords(evaluations.Select(x => x.Projection.Observations), "Observations"),
            UnionRecords(evaluations.Select(x => x.Projection.Knowledge), "Knowledge"),
            UnionRecords(evaluations.Select(x => x.Projection.Beliefs), "Beliefs"),
            UnionRecords(evaluations.Select(x => x.Projection.Suspicions), "Suspicions"),
            UnionRecords(evaluations.Select(x => x.Projection.Memories), "Memories"),
            UnionRecords(evaluations.Select(x => x.Projection.Goals), "Goals"),
            UnionRelationships(evaluations.Select(x => x.Projection.Relationships)));

        var context = E0DExperimentalContextComposer.Compose(
            experimental,
            referenceProjection.SubjectCharacterId,
            reference.ContextEvaluation.Packet.RecentPerformances,
            E0DExperimentContracts.OmniscientCompositionContract);
        return new E0ProductionContextContinuityResult(reference.AccessEvaluation, context);
    }

    private static E0DOpportunityResult EstablishRoundRobin(E0PostCommitCycleState source)
    {
        // Execute the reference authority once as a pure source-chain validator. Its
        // result is intentionally discarded; the experiment changes only selection.
        _ = DeterministicOpportunityAuthority.Establish(
            source.ProductionState,
            source.Commit,
            source.SourceContext,
            source.SourceOpportunityHistory);

        var candidate = source.Commit.Take?.Performance
            ?? throw new E0DExperimentException("E0-D round-robin source Performance is missing.");
        var input = DirectorOpportunityInput.Bind(
            source.SourceContext,
            candidate,
            source.SourceOpportunityHistory.CharacterIds);
        var directorEvaluation = EvaluateRoundRobin(input);
        var selected = directorEvaluation.Proposal.SelectedCharacterId;
        var resultProjection = source.ProductionState.Projection with
        {
            CurrentOpportunityCharacterId = selected
        };

        try
        {
            var resultHash = OpportunityCanonicalizer.ComputeResultHash(
                source.ProductionState.StateHash,
                E0DExperimentContracts.RoundRobinStrategyContract,
                selected,
                resultProjection);
            var @event = new E0OpportunityTransition(
                source.ProductionState.StateHash,
                resultHash,
                E0DExperimentContracts.RoundRobinStrategyContract,
                selected);
            var state = source.ProductionState.WithEstablishedOpportunity(selected, resultHash);
            var history = source.SourceOpportunityHistory.Advance(state, selected);
            _ = AcceptedPerformanceHistoryInvariants.ValidateAndProject(
                source.AcceptedPerformanceHistory,
                source.ProductionState.SceneId,
                source.ProductionState.StateHash,
                source.ProductionState.RosterCharacterIds);
            var accepted = AcceptedPerformanceHistoryInvariants.AdvanceState(
                source.AcceptedPerformanceHistory,
                state.StateHash);
            var next = E0OpportunityBearingCycleState.Create(state, accepted, history);
            return new E0DOpportunityResult(
                next,
                @event,
                E0DExperimentContracts.RoundRobinStrategyContract,
                directorEvaluation);
        }
        catch (Exception exception) when (
            exception is CanonicalJsonException or
            ProductionStateException or
            E0OpportunityTransitionException or
            E0AcceptedPerformanceHistoryInvariantException or
            E0CausalCycleInvariantException)
        {
            throw new E0DExperimentException("E0-D round-robin transition failed.", exception);
        }
    }

    private static (ImmutableArray<CharacterId> CanonicalRoster, int SourceIndex, CharacterId Selected) SelectRoundRobin(
        DirectorOpportunityInput input)
    {
        var roster = input.RosterCharacterIds;
        if (roster.IsDefault || roster.Length != 3)
        {
            throw new E0DExperimentException("E0-D round-robin requires the canonical three-Character roster.");
        }
        var canonical = roster.OrderBy(id => id.Value, StringComparer.Ordinal).ToImmutableArray();
        if (!canonical.SequenceEqual(roster))
        {
            throw new E0DExperimentException("E0-D round-robin roster is not canonical.");
        }
        var index = roster.IndexOf(input.SourceCharacterId);
        if (index < 0)
        {
            throw new E0DExperimentException("E0-D round-robin source Character is outside the roster.");
        }
        return (canonical, index, roster[(index + 1) % roster.Length]);
    }

    private static void ValidateUnionInputs(
        CharacterAccessProjection reference,
        ImmutableArray<CharacterAccessEvaluation> evaluations)
    {
        if (evaluations.Length != 3)
        {
            throw new E0DExperimentException("E0-D omniscient union requires exactly three safe projections.");
        }
        foreach (var evaluation in evaluations)
        {
            var projection = evaluation.Projection;
            if (projection.SourceStateHash != reference.SourceStateHash ||
                projection.SceneId != reference.SceneId ||
                !projection.Roster.Select(x => x.CharacterId).SequenceEqual(reference.Roster.Select(x => x.CharacterId)))
            {
                throw new E0DExperimentException("E0-D omniscient source projections are not one exact Production state/roster.");
            }
        }
    }

    private static ImmutableArray<PermittedRecord> Filter(
        ImmutableArray<PermittedRecord> records,
        ImmutableHashSet<RecordId> excluded) =>
        records.Where(record => !excluded.Contains(record.RecordId)).ToImmutableArray();

    private static ImmutableArray<PermittedRelationship> Filter(
        ImmutableArray<PermittedRelationship> records,
        ImmutableHashSet<RecordId> excluded) =>
        records.Where(record => !excluded.Contains(record.RecordId)).ToImmutableArray();

    private static ImmutableArray<PermittedRecord> UnionRecords(
        IEnumerable<ImmutableArray<PermittedRecord>> categories,
        string category)
    {
        var byId = new Dictionary<string, PermittedRecord>(StringComparer.Ordinal);
        foreach (var record in categories.SelectMany(x => x))
        {
            if (byId.TryGetValue(record.RecordId.Value, out var existing))
            {
                if (!string.Equals(existing.Text, record.Text, StringComparison.Ordinal))
                {
                    throw new E0DExperimentException($"E0-D omniscient {category} union contains conflicting text.");
                }
                continue;
            }
            byId.Add(record.RecordId.Value, record);
        }
        return byId.Values.OrderBy(x => x.RecordId.Value, StringComparer.Ordinal).ToImmutableArray();
    }

    private static ImmutableArray<PermittedRelationship> UnionRelationships(
        IEnumerable<ImmutableArray<PermittedRelationship>> categories)
    {
        var byId = new Dictionary<string, PermittedRelationship>(StringComparer.Ordinal);
        foreach (var relationship in categories.SelectMany(x => x))
        {
            if (byId.TryGetValue(relationship.RecordId.Value, out var existing))
            {
                if (existing.TargetCharacterId != relationship.TargetCharacterId ||
                    !string.Equals(existing.Text, relationship.Text, StringComparison.Ordinal))
                {
                    throw new E0DExperimentException("E0-D omniscient relationship union contains conflicting content.");
                }
                continue;
            }
            byId.Add(relationship.RecordId.Value, relationship);
        }
        return byId.Values.OrderBy(x => x.RecordId.Value, StringComparer.Ordinal).ToImmutableArray();
    }
}