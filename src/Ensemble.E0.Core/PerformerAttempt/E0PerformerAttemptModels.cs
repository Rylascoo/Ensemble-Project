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
        ContextPacketId sourceContextPacketId,
        CandidatePerformance candidate)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        _ = sourceContextPacketId.Value;

        return new E0PerformerAttemptResult(
            E0PerformerAttemptDisposition.CandidateReady,
            sourceContextPacketId,
            candidate);
    }

    internal static E0PerformerAttemptResult CreateTechnicalOutcome(
        ContextPacketId sourceContextPacketId,
        E0PerformerAttemptDisposition disposition)
    {
        _ = sourceContextPacketId.Value;

        if (disposition is not E0PerformerAttemptDisposition.TechnicalFailure and
            not E0PerformerAttemptDisposition.Cancelled)
        {
            throw new ArgumentOutOfRangeException(nameof(disposition));
        }

        return new E0PerformerAttemptResult(
            disposition,
            sourceContextPacketId,
            null);
    }
}

public sealed class E0PerformerAttemptException : Exception
{
    internal E0PerformerAttemptException(string message)
        : base(message)
    {
    }
}
