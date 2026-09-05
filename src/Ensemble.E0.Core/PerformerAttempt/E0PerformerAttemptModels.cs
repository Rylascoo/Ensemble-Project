using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;

namespace Ensemble.E0.Core.PerformerAttempt;

public enum E0PerformerAttemptDisposition
{
    CandidateReady = 1,
    TechnicalFailure = 2,
    Cancelled = 3
}

public sealed class E0PerformerAttemptResult
{
    private E0PerformerAttemptResult(
        E0PerformerAttemptDisposition disposition,
        ContextPacketId sourceContextPacketId,
        CandidatePerformance? candidate)
    {
        Disposition = disposition;
        SourceContextPacketId = sourceContextPacketId;
        Candidate = candidate;
    }

    public E0PerformerAttemptDisposition Disposition { get; }
    public ContextPacketId SourceContextPacketId { get; }
    public CandidatePerformance? Candidate { get; }

    internal static E0PerformerAttemptResult CreateCandidate(
        ContextPacket? sourceContext,
        CandidatePerformance? candidate)
    {
        if (!IsValidSourceContext(sourceContext) ||
            !IsValidCandidate(sourceContext!, candidate))
        {
            throw new E0PerformerAttemptException(
                "E0 Performer attempt candidate binding failed.");
        }

        return new E0PerformerAttemptResult(
            E0PerformerAttemptDisposition.CandidateReady,
            sourceContext!.ContextPacketId,
            candidate);
    }

    internal static E0PerformerAttemptResult CreateTechnicalOutcome(
        ContextPacket? sourceContext,
        E0PerformerAttemptDisposition disposition)
    {
        if (!IsValidSourceContext(sourceContext) ||
            disposition is not E0PerformerAttemptDisposition.TechnicalFailure and
            not E0PerformerAttemptDisposition.Cancelled)
        {
            throw new E0PerformerAttemptException(
                "E0 Performer attempt technical outcome binding failed.");
        }

        return new E0PerformerAttemptResult(
            disposition,
            sourceContext!.ContextPacketId,
            null);
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

public sealed class E0PerformerAttemptException : Exception
{
    internal E0PerformerAttemptException(string message)
        : base(message)
    {
    }
}
