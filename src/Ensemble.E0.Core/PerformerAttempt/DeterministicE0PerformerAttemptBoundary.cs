using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Performer;

namespace Ensemble.E0.Core.PerformerAttempt;

public static class DeterministicE0PerformerAttemptBoundary
{
    public static E0PerformerAttemptResult BindCandidate(
        ContextPacket sourceContext,
        CandidatePerformance candidate)
    {
        if (!IsValidSourceContext(sourceContext) || !IsValidCandidate(sourceContext, candidate))
        {
            throw new E0PerformerAttemptException(
                "E0 Performer attempt candidate binding failed.");
        }

        return E0PerformerAttemptResult.CreateCandidate(
            sourceContext.ContextPacketId,
            candidate);
    }

    public static E0PerformerAttemptResult BindTechnicalOutcome(
        ContextPacket sourceContext,
        E0PerformerAttemptDisposition disposition)
    {
        if (!IsValidSourceContext(sourceContext) ||
            disposition is not E0PerformerAttemptDisposition.TechnicalFailure and
            not E0PerformerAttemptDisposition.Cancelled)
        {
            throw new E0PerformerAttemptException(
                "E0 Performer attempt technical outcome binding failed.");
        }

        return E0PerformerAttemptResult.CreateTechnicalOutcome(
            sourceContext.ContextPacketId,
            disposition);
    }

    private static bool IsValidSourceContext(ContextPacket? sourceContext)
    {
        if (sourceContext is null)
        {
            return false;
        }

        try
        {
            _ = sourceContext.ContextPacketId.Value;
            _ = sourceContext.SubjectCharacterId.Value;
            _ = sourceContext.OpportunityCharacterId.Value;
            return sourceContext.SubjectCharacterId == sourceContext.OpportunityCharacterId;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    private static bool IsValidCandidate(
        ContextPacket sourceContext,
        CandidatePerformance? candidate)
    {
        if (candidate is null ||
            !string.Equals(
                candidate.ContractVersion,
                PerformerCandidateContract.CandidateContractVersion,
                StringComparison.Ordinal))
        {
            return false;
        }

        try
        {
            _ = candidate.ContextPacketId.Value;
            _ = candidate.SubjectCharacterId.Value;

            return candidate.ContextPacketId == sourceContext.ContextPacketId &&
                candidate.SubjectCharacterId == sourceContext.SubjectCharacterId;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
