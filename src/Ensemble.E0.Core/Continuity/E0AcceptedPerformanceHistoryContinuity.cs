using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Continuity;

public static class E0AcceptedPerformanceHistoryContinuity
{
    public static E0AcceptedPerformanceHistory Initialize(ProductionState genesisState)
    {
        if (genesisState is null)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance history genesis Production state is required.");
        }

        try
        {
            var opportunityHistory = E0OpportunityHistory.Initialize(genesisState);
            if (opportunityHistory.SceneId != genesisState.SceneId ||
                opportunityHistory.LastOpportunityStateHash != genesisState.StateHash ||
                opportunityHistory.CharacterIds.Length != 1)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance history genesis Opportunity history is inconsistent.");
            }

            return AcceptedPerformanceHistoryInvariants.Create(
                genesisState.SceneId,
                genesisState.StateHash,
                System.Collections.Immutable.ImmutableArray<Context.ContextRecentPerformance>.Empty);
        }
        catch (E0AcceptedPerformanceHistoryException)
        {
            throw;
        }
        catch (E0AcceptedPerformanceHistoryInvariantException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance history genesis initialization failed.",
                exception);
        }
        catch (E0OpportunityTransitionException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance history genesis Opportunity validation failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance history genesis identity is invalid.",
                exception);
        }
    }

    public static E0AcceptedPerformanceHistory RecordCommit(
        E0AcceptedPerformanceHistory sourceHistory,
        ProductionState parentState,
        E0CausalCommit committedEvent)
    {
        if (sourceHistory is null)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance source history is required.");
        }

        if (parentState is null)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance parent Production state is required.");
        }

        if (committedEvent is null)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance causal event is required.");
        }

        try
        {
            _ = AcceptedPerformanceHistoryInvariants.ValidateAndProject(
                sourceHistory,
                parentState.SceneId,
                parentState.StateHash,
                parentState.RosterCharacterIds);

            var checkpoint = ProductionStateCheckpoint.Capture(parentState);
            var expectedContext = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
                checkpoint,
                sourceHistory).ContextEvaluation.Packet;

            var take = committedEvent.Take;
            if (take is null || take.Disposition != E0TakeDisposition.Accepted ||
                take.Performance is null ||
                take.Performance.SubjectCharacterId != expectedContext.SubjectCharacterId ||
                take.Performance.ContextPacketId != expectedContext.ContextPacketId)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance causal event source Context identity is invalid.");
            }

            var replayed = DeterministicCausalCommit.Replay(parentState, committedEvent);
            if (replayed.SceneId != parentState.SceneId ||
                replayed.CurrentOpportunityCharacterId.HasValue)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance causal replay result is inconsistent.");
            }

            return AcceptedPerformanceHistoryInvariants.Append(
                sourceHistory,
                replayed.StateHash,
                take.Performance.SubjectCharacterId,
                take.Performance.VisibleText);
        }
        catch (E0AcceptedPerformanceHistoryException)
        {
            throw;
        }
        catch (E0AcceptedPerformanceHistoryInvariantException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance history validation failed.",
                exception);
        }
        catch (E0ContextContinuityException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance source Context recomposition failed.",
                exception);
        }
        catch (E0CausalCommitException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance causal replay failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance causal identity is invalid.",
                exception);
        }
    }

    public static E0AcceptedPerformanceHistory RecordOpportunity(
        E0AcceptedPerformanceHistory sourceHistory,
        ProductionState postCommitState,
        E0CausalCommit sourceCommit,
        E0OpportunityHistory sourceOpportunityHistory,
        E0OpportunityTransition establishedEvent)
    {
        if (sourceHistory is null)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance source history is required.");
        }

        if (postCommitState is null)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance postcommit Production state is required.");
        }

        if (sourceCommit is null || sourceOpportunityHistory is null || establishedEvent is null)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance Opportunity transition inputs are required.");
        }

        try
        {
            var entries = AcceptedPerformanceHistoryInvariants.ValidateAndProject(
                sourceHistory,
                postCommitState.SceneId,
                postCommitState.StateHash,
                postCommitState.RosterCharacterIds);
            if (entries.Length == 0)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance Opportunity advancement requires nonempty history.");
            }

            if (sourceCommit.ResultStateHash != postCommitState.StateHash)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance Opportunity source commit does not match the postcommit state.");
            }

            var performance = sourceCommit.Take?.Performance;
            var last = entries[^1];
            if (performance is null ||
                last.SourceCharacterId != performance.SubjectCharacterId ||
                !string.Equals(last.VisibleText, performance.VisibleText, StringComparison.Ordinal))
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance history does not end at the source committed Performance.");
            }

            if (sourceOpportunityHistory.CharacterIds.IsDefault ||
                sourceOpportunityHistory.CharacterIds.Length != entries.Length)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance and Opportunity history counts are inconsistent before advancement.");
            }

            var replayed = DeterministicOpportunityAuthority.Replay(
                postCommitState,
                sourceCommit,
                sourceOpportunityHistory,
                establishedEvent);

            if (replayed.History.CharacterIds.IsDefault ||
                replayed.History.CharacterIds.Length != entries.Length + 1 ||
                replayed.History.CharacterIds[^1] != establishedEvent.SelectedCharacterId ||
                replayed.State.CurrentOpportunityCharacterId != establishedEvent.SelectedCharacterId ||
                replayed.History.LastOpportunityStateHash != replayed.State.StateHash)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "Accepted Performance Opportunity replay result is inconsistent.");
            }

            return AcceptedPerformanceHistoryInvariants.AdvanceState(
                sourceHistory,
                replayed.State.StateHash);
        }
        catch (E0AcceptedPerformanceHistoryException)
        {
            throw;
        }
        catch (E0AcceptedPerformanceHistoryInvariantException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance history validation failed.",
                exception);
        }
        catch (E0OpportunityTransitionException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance Opportunity replay failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "Accepted Performance Opportunity identity is invalid.",
                exception);
        }
    }
}

public sealed class E0AcceptedPerformanceHistoryException : Exception
{
    internal E0AcceptedPerformanceHistoryException(string message)
        : base(message)
    {
    }

    internal E0AcceptedPerformanceHistoryException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}
