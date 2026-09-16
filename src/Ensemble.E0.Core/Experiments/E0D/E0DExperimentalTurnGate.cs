using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.Turn;

namespace Ensemble.E0.Core.Experiments.E0D;

public static class E0DExperimentalTurnGate
{
    public static E0TurnProgress GateCandidate(
        E0OpportunityBearingCycleState source,
        ContextPacket context,
        E0PerformerAttemptResult attemptResult,
        E0DExperimentVariant variant)
    {
        ValidateContextAblationAttempt(source, context, attemptResult, variant);
        if (attemptResult.Disposition != E0PerformerAttemptDisposition.CandidateReady ||
            attemptResult.Candidate is null)
        {
            throw new E0DExperimentException("E0-D context-ablation Candidate attempt is incomplete.");
        }
        var replayAttempt = DeterministicE0PerformerAttemptBoundary.BindCandidate(
            context,
            attemptResult.Candidate);
        if (!ReferenceEquals(replayAttempt.Candidate, attemptResult.Candidate))
        {
            throw new E0DExperimentException("E0-D context-ablation Candidate replay changed the Candidate.");
        }

        try
        {
            return E0TurnProgress.CreateCandidate(
                source,
                context,
                attemptResult.Candidate);
        }
        catch (E0TurnInvariantException exception)
        {
            throw new E0DExperimentException(
                "E0-D context-ablation turn progress binding failed.",
                exception);
        }
    }

    public static E0TurnProgress GateTechnical(
        E0OpportunityBearingCycleState source,
        ContextPacket context,
        E0PerformerAttemptResult attemptResult,
        E0DExperimentVariant variant)
    {
        ValidateContextAblationAttempt(source, context, attemptResult, variant);
        if (attemptResult.Disposition is not E0PerformerAttemptDisposition.TechnicalFailure and
            not E0PerformerAttemptDisposition.Cancelled ||
            attemptResult.Candidate is not null)
        {
            throw new E0DExperimentException("E0-D context-ablation technical attempt is invalid.");
        }

        var disposition = attemptResult.Disposition == E0PerformerAttemptDisposition.Cancelled
            ? E0TurnProgressDisposition.Cancelled
            : E0TurnProgressDisposition.TechnicalFailure;
        try
        {
            return E0TurnProgress.CreateTechnical(source, context, disposition);
        }
        catch (E0TurnInvariantException exception)
        {
            throw new E0DExperimentException(
                "E0-D context-ablation technical progress binding failed.",
                exception);
        }
    }

    private static void ValidateContextAblationAttempt(
        E0OpportunityBearingCycleState source,
        ContextPacket context,
        E0PerformerAttemptResult attemptResult,
        E0DExperimentVariant variant)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(attemptResult);
        if (variant is not E0DExperimentVariant.RelationshipsOmitted and
            not E0DExperimentVariant.OmniscientContext)
        {
            throw new E0DExperimentException("E0-D experimental turn gate is only valid for context ablations.");
        }
        var replayContext = E0DExperimentalCycle.ComposeContext(source, variant).ContextEvaluation.Packet;
        if (replayContext.ContextPacketId != context.ContextPacketId ||
            attemptResult.SourceContextPacketId != context.ContextPacketId)
        {
            throw new E0DExperimentException("E0-D context-ablation turn attempt is stale.");
        }
    }
}
