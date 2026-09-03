using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;

namespace Ensemble.E0.Core.Take;

public static class E0TakeContracts
{
    public const string ContractVersion = "ensemble.e0.take.v1";
}

public enum E0TakeDisposition
{
    Unspecified = 0,
    Accepted = 1,
    Rejected = 2,
    Alternate = 3
}

public sealed class E0Take
{
    private E0Take(
        TakeId takeId,
        E0TakeDisposition disposition,
        CandidatePerformance performance,
        StateInterpretationProposal interpretationProposal,
        StateAuthorityEvaluation authorityEvaluation)
    {
        ContractVersion = E0TakeContracts.ContractVersion;
        TakeId = takeId;
        Disposition = disposition;
        Performance = performance;
        InterpretationProposal = interpretationProposal;
        AuthorityEvaluation = authorityEvaluation;
    }

    public string ContractVersion { get; }
    public TakeId TakeId { get; }
    public E0TakeDisposition Disposition { get; }
    public CandidatePerformance Performance { get; }
    public StateInterpretationProposal InterpretationProposal { get; }
    public StateAuthorityEvaluation AuthorityEvaluation { get; }

    public static E0Take Bind(
        TakeId takeId,
        ContextPacket sourceContext,
        CandidatePerformance performance,
        IntegrityValidationEvaluation integrityEvaluation,
        StateInterpretationProposal interpretationProposal,
        StateAuthorityEvaluation authorityEvaluation,
        E0TakeDisposition disposition)
    {
        RequireInitializedTakeId(takeId);
        ValidateDisposition(disposition);

        if (sourceContext is null)
        {
            throw new E0TakeException("E0 Take source ContextPacket is required.");
        }

        if (performance is null)
        {
            throw new E0TakeException("E0 Take CandidatePerformance is required.");
        }

        if (integrityEvaluation is null)
        {
            throw new E0TakeException("E0 Take Integrity evaluation is required.");
        }

        if (interpretationProposal is null)
        {
            throw new E0TakeException("E0 Take State Interpretation proposal is required.");
        }

        if (authorityEvaluation is null)
        {
            throw new E0TakeException("E0 Take State Authority evaluation is required.");
        }

        if (!string.Equals(
                authorityEvaluation.ContractVersion,
                StateAuthorityContracts.ContractVersion,
                StringComparison.Ordinal))
        {
            throw new E0TakeException("E0 Take State Authority evaluation contract is unsupported.");
        }

        var suppliedTrace = authorityEvaluation.Trace;
        if (suppliedTrace is null ||
            suppliedTrace.Input is null ||
            suppliedTrace.Policy is null ||
            suppliedTrace.ReviewSet is null ||
            suppliedTrace.Input.Snapshot is null)
        {
            throw new E0TakeException("E0 Take State Authority trace is not structurally initialized.");
        }

        StateInterpretationSource freshSource;
        try
        {
            freshSource = StateInterpretationSource.Bind(
                sourceContext,
                performance,
                integrityEvaluation);
        }
        catch (StateInterpretationException exception)
        {
            throw new E0TakeException(
                "E0 Take source association is invalid.",
                exception);
        }

        StateAuthorityEvaluation freshAuthorityEvaluation;
        try
        {
            var freshAuthorityInput = StateAuthorityInput.Bind(
                suppliedTrace.Input.Snapshot,
                freshSource,
                interpretationProposal);

            freshAuthorityEvaluation = DeterministicStateAuthority.Evaluate(
                freshAuthorityInput,
                suppliedTrace.Policy,
                suppliedTrace.ReviewSet);
        }
        catch (StateAuthorityException exception)
        {
            throw new E0TakeException(
                "E0 Take State Authority replay failed.",
                exception);
        }

        if (freshAuthorityEvaluation.Status != StateAuthorityEvaluationStatus.Complete)
        {
            throw new E0TakeException("E0 Take requires a terminal Complete State Authority evaluation.");
        }

        if (freshAuthorityEvaluation.Decisions.IsDefault ||
            freshAuthorityEvaluation.Decisions.Length != freshAuthorityEvaluation.Trace.Input.Mutations.Length)
        {
            throw new E0TakeException("E0 Take State Authority replay produced an invalid decision set.");
        }

        for (var index = 0; index < freshAuthorityEvaluation.Decisions.Length; index++)
        {
            var decision = freshAuthorityEvaluation.Decisions[index];
            if (decision is null || decision.MutationIndex != index)
            {
                throw new E0TakeException("E0 Take State Authority replay produced an invalid decision order.");
            }

            if (decision.Disposition == StateAuthorityDisposition.RequiresReview)
            {
                throw new E0TakeException("E0 Take cannot bind unresolved State Authority review.");
            }
        }

        return new E0Take(
            takeId,
            disposition,
            performance,
            interpretationProposal,
            freshAuthorityEvaluation);
    }

    private static void RequireInitializedTakeId(TakeId takeId)
    {
        try
        {
            _ = takeId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0TakeException("E0 TakeId is uninitialized.");
        }
    }

    private static void ValidateDisposition(E0TakeDisposition disposition)
    {
        if (disposition == E0TakeDisposition.Unspecified || !Enum.IsDefined(disposition))
        {
            throw new E0TakeException("E0 Take disposition is invalid.");
        }
    }
}

public sealed class E0TakeException : Exception
{
    internal E0TakeException(string message)
        : base(message)
    {
    }

    internal E0TakeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
