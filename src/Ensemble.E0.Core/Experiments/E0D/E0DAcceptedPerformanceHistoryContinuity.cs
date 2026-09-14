using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Experiments.E0D;

internal static class E0DAcceptedPerformanceHistoryContinuity
{
    internal static E0AcceptedPerformanceHistory RecordCommit(
        E0AcceptedPerformanceHistory sourceHistory,
        ProductionState parentState,
        E0CausalCommit committedEvent,
        ContextPacket expectedContext)
    {
        ArgumentNullException.ThrowIfNull(sourceHistory);
        ArgumentNullException.ThrowIfNull(parentState);
        ArgumentNullException.ThrowIfNull(committedEvent);
        ArgumentNullException.ThrowIfNull(expectedContext);

        try
        {
            _ = AcceptedPerformanceHistoryInvariants.ValidateAndProject(
                sourceHistory,
                parentState.SceneId,
                parentState.StateHash,
                parentState.RosterCharacterIds);
            if (!expectedContext.SourceStateHash.HasValue ||
                expectedContext.SourceStateHash.Value != parentState.StateHash ||
                expectedContext.SceneId != parentState.SceneId ||
                !E0DExperimentContracts.IsContextAblationCompositionContract(
                    expectedContext.CompositionContract))
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "E0-D accepted Performance expected Context identity is invalid.");
            }

            var take = committedEvent.Take;
            if (take is null || take.Disposition != E0TakeDisposition.Accepted ||
                take.Performance is null ||
                take.Performance.SubjectCharacterId != expectedContext.SubjectCharacterId ||
                take.Performance.ContextPacketId != expectedContext.ContextPacketId)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "E0-D accepted Performance causal event source Context identity is invalid.");
            }

            var replayed = DeterministicCausalCommit.Replay(parentState, committedEvent);
            if (replayed.SceneId != parentState.SceneId ||
                replayed.CurrentOpportunityCharacterId.HasValue)
            {
                throw new E0AcceptedPerformanceHistoryException(
                    "E0-D accepted Performance causal replay result is inconsistent.");
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
                "E0-D accepted Performance history validation failed.",
                exception);
        }
        catch (E0CausalCommitException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "E0-D accepted Performance causal replay failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0AcceptedPerformanceHistoryException(
                "E0-D accepted Performance causal identity is invalid.",
                exception);
        }
    }
}
