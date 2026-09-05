using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Performer;

namespace Ensemble.E0.Core.PerformerAttempt;

public static class DeterministicE0PerformerAttemptBoundary
{
    public static E0PerformerAttemptResult BindCandidate(
        ContextPacket sourceContext,
        CandidatePerformance candidate) =>
        E0PerformerAttemptResult.CreateCandidate(sourceContext, candidate);

    public static E0PerformerAttemptResult BindTechnicalOutcome(
        ContextPacket sourceContext,
        E0PerformerAttemptDisposition disposition) =>
        E0PerformerAttemptResult.CreateTechnicalOutcome(sourceContext, disposition);
}
