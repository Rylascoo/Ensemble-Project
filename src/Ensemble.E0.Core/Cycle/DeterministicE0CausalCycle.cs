using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Cycle;

public static class DeterministicE0CausalCycle
{
    public static E0OpportunityBearingCycleState Initialize(
        ProductionState genesisState)
    {
        if (genesisState is null)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle initialization failed.");
        }

        try
        {
            var acceptedHistory = E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState);
            var opportunityHistory = E0OpportunityHistory.Initialize(genesisState);
            return E0OpportunityBearingCycleState.Create(
                genesisState,
                acceptedHistory,
                opportunityHistory);
        }
        catch (E0AcceptedPerformanceHistoryException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle initialization failed.");
        }
        catch (E0OpportunityTransitionException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle initialization failed.");
        }
        catch (E0CausalCycleInvariantException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle initialization failed.");
        }
    }

    public static E0ProductionContextContinuityResult ComposeContext(
        E0OpportunityBearingCycleState source)
    {
        if (source is null)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle Context composition failed.");
        }

        try
        {
            var checkpoint = ProductionStateCheckpoint.Capture(source.ProductionState);
            return E0ProductionContextContinuity.ComposeWithAcceptedHistory(
                checkpoint,
                source.AcceptedPerformanceHistory);
        }
        catch (E0CausalCommitException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle Context composition failed.");
        }
        catch (E0ContextContinuityException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle Context composition failed.");
        }
    }

    public static E0PostCommitCycleState CommitAcceptedTake(
        CommitId commitId,
        E0OpportunityBearingCycleState source,
        ContextPacket sourceContext,
        E0Take acceptedTake,
        E0RecordMaterializationSet materializations)
    {
        if (source is null ||
            sourceContext is null ||
            acceptedTake is null ||
            materializations is null)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle accepted Take commit failed.");
        }

        try
        {
            var checkpoint = ProductionStateCheckpoint.Capture(source.ProductionState);
            var binding = E0TakeStateBinding.BindWithAcceptedHistory(
                checkpoint,
                sourceContext,
                acceptedTake,
                source.AcceptedPerformanceHistory);
            var commitResult = DeterministicCausalCommit.Commit(
                commitId,
                source.ProductionState,
                binding,
                materializations);
            var acceptedHistory = E0AcceptedPerformanceHistoryContinuity.RecordCommit(
                source.AcceptedPerformanceHistory,
                source.ProductionState,
                commitResult.Commit);

            return E0PostCommitCycleState.Create(
                commitResult.ResultState,
                acceptedHistory,
                commitResult.Commit,
                sourceContext,
                source.OpportunityHistory);
        }
        catch (E0CausalCommitException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle accepted Take commit failed.");
        }
        catch (E0AcceptedPerformanceHistoryException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle accepted Take commit failed.");
        }
        catch (E0CausalCycleInvariantException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle accepted Take commit failed.");
        }
    }

    public static E0OpportunityBearingCycleResult EstablishOpportunity(
        E0PostCommitCycleState source)
    {
        if (source is null)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle Opportunity establishment failed.");
        }

        try
        {
            var opportunity = DeterministicOpportunityAuthority.Establish(
                source.ProductionState,
                source.Commit,
                source.SourceContext,
                source.SourceOpportunityHistory);
            var acceptedHistory = E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
                source.AcceptedPerformanceHistory,
                source.ProductionState,
                source.Commit,
                source.SourceOpportunityHistory,
                opportunity.Event);
            var nextState = E0OpportunityBearingCycleState.Create(
                opportunity.State,
                acceptedHistory,
                opportunity.History);

            return E0OpportunityBearingCycleResult.Create(
                nextState,
                opportunity.Event,
                opportunity.DirectorEvaluation);
        }
        catch (E0OpportunityTransitionException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle Opportunity establishment failed.");
        }
        catch (E0AcceptedPerformanceHistoryException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle Opportunity establishment failed.");
        }
        catch (E0CausalCycleInvariantException)
        {
            throw new E0CausalCycleException(
                "E0 causal cycle Opportunity establishment failed.");
        }
    }
}
