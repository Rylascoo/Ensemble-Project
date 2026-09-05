using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Turn;

public enum E0TurnProgressDisposition
{
    TechnicalFailure = 1,
    Cancelled = 2,
    CandidateReady = 3,
    RequestAnotherTake = 4,
    ReadyForInterpretation = 5,
    AuthorityReviewRequired = 6,
    TakeBindable = 7,
    AcceptedTakeReady = 8
}

public sealed class E0TurnProgress
{
    private E0TurnProgress(
        E0TurnProgressDisposition disposition,
        E0OpportunityBearingCycleState sourceCycle,
        ContextPacket sourceContext,
        CandidatePerformance? candidate,
        IntegrityValidationEvaluation? integrityEvaluation,
        StateInterpretationSource? interpretationSource,
        StateInterpretationProposal? interpretationProposal,
        StateAuthorityEvaluation? authorityEvaluation,
        StateAuthorityPolicy? authorityPolicy,
        E0Take? acceptedTake)
    {
        Disposition = disposition;
        SourceCycle = sourceCycle;
        SourceContext = sourceContext;
        Candidate = candidate;
        IntegrityEvaluation = integrityEvaluation;
        InterpretationSource = interpretationSource;
        InterpretationProposal = interpretationProposal;
        AuthorityEvaluation = authorityEvaluation;
        AuthorityPolicy = authorityPolicy;
        AcceptedTake = acceptedTake;
    }

    public E0TurnProgressDisposition Disposition { get; }
    public ContextPacket SourceContext { get; }
    public CandidatePerformance? Candidate { get; }
    public IntegrityValidationEvaluation? IntegrityEvaluation { get; }
    public StateInterpretationSource? InterpretationSource { get; }
    public StateInterpretationProposal? InterpretationProposal { get; }
    public StateAuthorityEvaluation? AuthorityEvaluation { get; }
    public E0Take? AcceptedTake { get; }

    internal E0OpportunityBearingCycleState SourceCycle { get; }
    internal StateAuthorityPolicy? AuthorityPolicy { get; }

    internal static E0TurnProgress CreateTechnical(
        E0OpportunityBearingCycleState sourceCycle,
        ContextPacket sourceContext,
        E0TurnProgressDisposition disposition)
    {
        if (disposition is not E0TurnProgressDisposition.TechnicalFailure and
            not E0TurnProgressDisposition.Cancelled)
        {
            throw new E0TurnInvariantException("Turn technical disposition is invalid.");
        }

        ValidateBase(sourceCycle, sourceContext);
        ReplayTechnical(sourceContext, disposition);
        return new E0TurnProgress(
            disposition,
            sourceCycle,
            sourceContext,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
    }

    internal static E0TurnProgress CreateCandidate(
        E0OpportunityBearingCycleState sourceCycle,
        ContextPacket sourceContext,
        CandidatePerformance candidate)
    {
        ValidateBase(sourceCycle, sourceContext);
        ValidateCandidate(sourceContext, candidate);
        return new E0TurnProgress(
            E0TurnProgressDisposition.CandidateReady,
            sourceCycle,
            sourceContext,
            candidate,
            null,
            null,
            null,
            null,
            null,
            null);
    }

    internal static E0TurnProgress CreateIntegrity(
        E0TurnProgress source,
        IntegrityValidationEvaluation integrityEvaluation,
        StateInterpretationSource? interpretationSource)
    {
        Validate(source);
        if (source.Disposition != E0TurnProgressDisposition.CandidateReady ||
            source.Candidate is null ||
            integrityEvaluation is null)
        {
            throw new E0TurnInvariantException("Turn Integrity source is invalid.");
        }

        ValidateIntegrityEvaluation(source.SourceContext, source.Candidate, integrityEvaluation);
        if (integrityEvaluation.Disposition == IntegrityDisposition.RequestAnotherTake)
        {
            if (interpretationSource is not null)
            {
                throw new E0TurnInvariantException("Request-another-Take cannot retain interpretation source.");
            }

            return new E0TurnProgress(
                E0TurnProgressDisposition.RequestAnotherTake,
                source.SourceCycle,
                source.SourceContext,
                source.Candidate,
                integrityEvaluation,
                null,
                null,
                null,
                null,
                null);
        }

        if (integrityEvaluation.Disposition != IntegrityDisposition.Accept ||
            interpretationSource is null)
        {
            throw new E0TurnInvariantException("Turn Integrity disposition is invalid.");
        }

        ValidateInterpretationSource(
            source.SourceContext,
            source.Candidate,
            integrityEvaluation,
            interpretationSource);
        return new E0TurnProgress(
            E0TurnProgressDisposition.ReadyForInterpretation,
            source.SourceCycle,
            source.SourceContext,
            source.Candidate,
            integrityEvaluation,
            interpretationSource,
            null,
            null,
            null,
            null);
    }

    internal static E0TurnProgress CreateAuthority(
        E0TurnProgress source,
        StateInterpretationProposal proposal,
        StateAuthorityPolicy policy,
        StateAuthorityEvaluation authorityEvaluation)
    {
        Validate(source);
        if (source.Disposition is not E0TurnProgressDisposition.ReadyForInterpretation and
            not E0TurnProgressDisposition.AuthorityReviewRequired ||
            source.Candidate is null ||
            source.IntegrityEvaluation is null ||
            source.InterpretationSource is null ||
            proposal is null ||
            policy is null ||
            authorityEvaluation is null)
        {
            throw new E0TurnInvariantException("Turn State Authority source is invalid.");
        }

        if (source.Disposition == E0TurnProgressDisposition.AuthorityReviewRequired &&
            (!ReferenceEquals(source.InterpretationProposal, proposal) ||
             !ReferenceEquals(source.AuthorityPolicy, policy)))
        {
            throw new E0TurnInvariantException("Turn State Authority review package changed.");
        }

        ValidateAuthorityEvaluation(
            source.SourceCycle,
            source.InterpretationSource,
            proposal,
            policy,
            authorityEvaluation);
        var disposition = authorityEvaluation.Status switch
        {
            StateAuthorityEvaluationStatus.ReviewRequired =>
                E0TurnProgressDisposition.AuthorityReviewRequired,
            StateAuthorityEvaluationStatus.Complete =>
                E0TurnProgressDisposition.TakeBindable,
            _ => throw new E0TurnInvariantException("Turn State Authority status is invalid.")
        };

        return new E0TurnProgress(
            disposition,
            source.SourceCycle,
            source.SourceContext,
            source.Candidate,
            source.IntegrityEvaluation,
            source.InterpretationSource,
            proposal,
            authorityEvaluation,
            policy,
            null);
    }

    internal static E0TurnProgress CreateAcceptedTake(
        E0TurnProgress source,
        E0Take acceptedTake)
    {
        Validate(source);
        if (source.Disposition != E0TurnProgressDisposition.TakeBindable ||
            source.Candidate is null ||
            source.IntegrityEvaluation is null ||
            source.InterpretationSource is null ||
            source.InterpretationProposal is null ||
            source.AuthorityPolicy is null ||
            acceptedTake is null ||
            acceptedTake.Disposition != E0TakeDisposition.Accepted ||
            !ReferenceEquals(acceptedTake.Performance, source.Candidate) ||
            !ReferenceEquals(acceptedTake.InterpretationProposal, source.InterpretationProposal) ||
            acceptedTake.AuthorityEvaluation is null ||
            acceptedTake.AuthorityEvaluation.Status != StateAuthorityEvaluationStatus.Complete)
        {
            throw new E0TurnInvariantException("Turn Accepted Take is invalid.");
        }

        ValidateIntegrityEvaluation(
            source.SourceContext,
            acceptedTake.Performance,
            source.IntegrityEvaluation);
        ValidateAuthorityEvaluation(
            source.SourceCycle,
            source.InterpretationSource,
            acceptedTake.InterpretationProposal,
            source.AuthorityPolicy,
            acceptedTake.AuthorityEvaluation);
        return new E0TurnProgress(
            E0TurnProgressDisposition.AcceptedTakeReady,
            source.SourceCycle,
            source.SourceContext,
            acceptedTake.Performance,
            source.IntegrityEvaluation,
            source.InterpretationSource,
            acceptedTake.InterpretationProposal,
            acceptedTake.AuthorityEvaluation,
            source.AuthorityPolicy,
            acceptedTake);
    }

    internal static void Validate(E0TurnProgress? progress)
    {
        if (progress is null)
        {
            throw new E0TurnInvariantException("Turn progress is required.");
        }

        ValidateBase(progress.SourceCycle, progress.SourceContext);
        if (!Enum.IsDefined(progress.Disposition))
        {
            throw new E0TurnInvariantException("Turn progress disposition is invalid.");
        }

        switch (progress.Disposition)
        {
            case E0TurnProgressDisposition.TechnicalFailure:
            case E0TurnProgressDisposition.Cancelled:
                RequireAllLaterNull(progress);
                ReplayTechnical(progress.SourceContext, progress.Disposition);
                return;
            case E0TurnProgressDisposition.CandidateReady:
                RequireCandidate(progress);
                RequireLaterNullAfterCandidate(progress);
                return;
            case E0TurnProgressDisposition.RequestAnotherTake:
                RequireCandidate(progress);
                if (progress.IntegrityEvaluation is null ||
                    progress.IntegrityEvaluation.Disposition != IntegrityDisposition.RequestAnotherTake ||
                    progress.InterpretationSource is not null ||
                    progress.InterpretationProposal is not null ||
                    progress.AuthorityEvaluation is not null ||
                    progress.AuthorityPolicy is not null ||
                    progress.AcceptedTake is not null)
                {
                    throw new E0TurnInvariantException("Turn request-another-Take progress is invalid.");
                }

                ValidateIntegrityEvaluation(
                    progress.SourceContext,
                    progress.Candidate!,
                    progress.IntegrityEvaluation);
                return;
            case E0TurnProgressDisposition.ReadyForInterpretation:
                RequireReadyForInterpretation(progress);
                return;
            case E0TurnProgressDisposition.AuthorityReviewRequired:
            case E0TurnProgressDisposition.TakeBindable:
                RequireAuthorityProgress(progress);
                return;
            case E0TurnProgressDisposition.AcceptedTakeReady:
                RequireAcceptedTake(progress);
                return;
            default:
                throw new E0TurnInvariantException("Turn progress disposition is unsupported.");
        }
    }

    private static void RequireAllLaterNull(E0TurnProgress progress)
    {
        if (progress.Candidate is not null ||
            progress.IntegrityEvaluation is not null ||
            progress.InterpretationSource is not null ||
            progress.InterpretationProposal is not null ||
            progress.AuthorityEvaluation is not null ||
            progress.AuthorityPolicy is not null ||
            progress.AcceptedTake is not null)
        {
            throw new E0TurnInvariantException("Turn technical progress carries forbidden semantic state.");
        }
    }

    private static void RequireLaterNullAfterCandidate(E0TurnProgress progress)
    {
        if (progress.IntegrityEvaluation is not null ||
            progress.InterpretationSource is not null ||
            progress.InterpretationProposal is not null ||
            progress.AuthorityEvaluation is not null ||
            progress.AuthorityPolicy is not null ||
            progress.AcceptedTake is not null)
        {
            throw new E0TurnInvariantException("Turn Candidate-ready progress carries later semantic state.");
        }
    }

    private static void RequireReadyForInterpretation(E0TurnProgress progress)
    {
        RequireCandidate(progress);
        if (progress.IntegrityEvaluation is null ||
            progress.IntegrityEvaluation.Disposition != IntegrityDisposition.Accept ||
            progress.InterpretationSource is null ||
            progress.InterpretationProposal is not null ||
            progress.AuthorityEvaluation is not null ||
            progress.AuthorityPolicy is not null ||
            progress.AcceptedTake is not null)
        {
            throw new E0TurnInvariantException("Turn interpretation-ready progress is invalid.");
        }

        ValidateIntegrityEvaluation(
            progress.SourceContext,
            progress.Candidate!,
            progress.IntegrityEvaluation);
        ValidateInterpretationSource(
            progress.SourceContext,
            progress.Candidate!,
            progress.IntegrityEvaluation,
            progress.InterpretationSource);
    }

    private static void RequireAuthorityProgress(E0TurnProgress progress)
    {
        RequireCandidate(progress);
        if (progress.IntegrityEvaluation is null ||
            progress.IntegrityEvaluation.Disposition != IntegrityDisposition.Accept ||
            progress.InterpretationSource is null ||
            progress.InterpretationProposal is null ||
            progress.AuthorityEvaluation is null ||
            progress.AuthorityPolicy is null ||
            progress.AcceptedTake is not null)
        {
            throw new E0TurnInvariantException("Turn State Authority progress is incomplete.");
        }

        var expectedStatus = progress.Disposition == E0TurnProgressDisposition.AuthorityReviewRequired
            ? StateAuthorityEvaluationStatus.ReviewRequired
            : StateAuthorityEvaluationStatus.Complete;
        if (progress.AuthorityEvaluation.Status != expectedStatus)
        {
            throw new E0TurnInvariantException("Turn State Authority status does not match progress.");
        }

        ValidateIntegrityEvaluation(
            progress.SourceContext,
            progress.Candidate!,
            progress.IntegrityEvaluation);
        ValidateInterpretationSource(
            progress.SourceContext,
            progress.Candidate!,
            progress.IntegrityEvaluation,
            progress.InterpretationSource);
        ValidateAuthorityEvaluation(
            progress.SourceCycle,
            progress.InterpretationSource,
            progress.InterpretationProposal,
            progress.AuthorityPolicy,
            progress.AuthorityEvaluation);
    }

    private static void RequireAcceptedTake(E0TurnProgress progress)
    {
        RequireCandidate(progress);
        if (progress.IntegrityEvaluation is null ||
            progress.IntegrityEvaluation.Disposition != IntegrityDisposition.Accept ||
            progress.InterpretationSource is null ||
            progress.InterpretationProposal is null ||
            progress.AuthorityEvaluation is null ||
            progress.AuthorityEvaluation.Status != StateAuthorityEvaluationStatus.Complete ||
            progress.AuthorityPolicy is null ||
            progress.AcceptedTake is null ||
            progress.AcceptedTake.Disposition != E0TakeDisposition.Accepted ||
            !ReferenceEquals(progress.Candidate, progress.AcceptedTake.Performance) ||
            !ReferenceEquals(progress.InterpretationProposal, progress.AcceptedTake.InterpretationProposal) ||
            !ReferenceEquals(progress.AuthorityEvaluation, progress.AcceptedTake.AuthorityEvaluation))
        {
            throw new E0TurnInvariantException("Turn Accepted-Take-ready progress is invalid.");
        }

        ValidateIntegrityEvaluation(
            progress.SourceContext,
            progress.Candidate!,
            progress.IntegrityEvaluation);
        ValidateAuthorityEvaluation(
            progress.SourceCycle,
            progress.InterpretationSource,
            progress.InterpretationProposal,
            progress.AuthorityPolicy,
            progress.AuthorityEvaluation);
    }

    private static void RequireCandidate(E0TurnProgress progress) =>
        ValidateCandidate(
            progress.SourceContext,
            progress.Candidate ?? throw new E0TurnInvariantException("Turn Candidate is required."));

    private static void ReplayTechnical(
        ContextPacket sourceContext,
        E0TurnProgressDisposition disposition)
    {
        var performerDisposition = disposition switch
        {
            E0TurnProgressDisposition.TechnicalFailure => E0PerformerAttemptDisposition.TechnicalFailure,
            E0TurnProgressDisposition.Cancelled => E0PerformerAttemptDisposition.Cancelled,
            _ => throw new E0TurnInvariantException("Turn technical disposition is invalid.")
        };

        try
        {
            _ = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
                sourceContext,
                performerDisposition);
        }
        catch (E0PerformerAttemptException exception)
        {
            throw new E0TurnInvariantException("Turn technical source is invalid.", exception);
        }
    }

    private static void ValidateCandidate(
        ContextPacket sourceContext,
        CandidatePerformance candidate)
    {
        if (candidate is null)
        {
            throw new E0TurnInvariantException("Turn Candidate is required.");
        }

        try
        {
            var replay = DeterministicE0PerformerAttemptBoundary.BindCandidate(
                sourceContext,
                candidate);
            if (!ReferenceEquals(replay.Candidate, candidate))
            {
                throw new E0TurnInvariantException("Turn Candidate replay changed the Candidate.");
            }
        }
        catch (E0PerformerAttemptException exception)
        {
            throw new E0TurnInvariantException("Turn Candidate source is invalid.", exception);
        }
    }

    private static void ValidateBase(
        E0OpportunityBearingCycleState? sourceCycle,
        ContextPacket? sourceContext)
    {
        if (sourceCycle is null || sourceContext is null)
        {
            throw new E0TurnInvariantException("Turn source Cycle and Context are required.");
        }

        try
        {
            _ = sourceCycle.ProductionState.StateHash.Value;
            _ = sourceCycle.ProductionState.SceneId.Value;
            _ = sourceContext.ContextPacketId.Value;
            _ = sourceContext.SceneId.Value;
            _ = sourceContext.SubjectCharacterId.Value;
            _ = sourceContext.OpportunityCharacterId.Value;
            if (!sourceContext.SourceStateHash.HasValue)
            {
                throw new E0TurnInvariantException("Turn source Context has no source StateHash.");
            }

            _ = sourceContext.SourceStateHash.Value.Value;
            if (sourceContext.SourceStateHash.Value != sourceCycle.ProductionState.StateHash ||
                sourceContext.SceneId != sourceCycle.ProductionState.SceneId ||
                !sourceCycle.ProductionState.CurrentOpportunityCharacterId.HasValue ||
                sourceContext.SubjectCharacterId != sourceCycle.ProductionState.CurrentOpportunityCharacterId.Value ||
                sourceContext.OpportunityCharacterId != sourceContext.SubjectCharacterId)
            {
                throw new E0TurnInvariantException("Turn source Context does not match the source Cycle.");
            }

            var freshContext = DeterministicE0CausalCycle
                .ComposeContext(sourceCycle)
                .ContextEvaluation
                .Packet;
            if (freshContext.ContextPacketId != sourceContext.ContextPacketId)
            {
                throw new E0TurnInvariantException("Turn source Context is not the exact current Cycle Context.");
            }
        }
        catch (InvalidOperationException exception)
        {
            throw new E0TurnInvariantException("Turn source identity is uninitialized.", exception);
        }
        catch (E0CausalCycleException exception)
        {
            throw new E0TurnInvariantException("Turn source Cycle Context cannot be recomposed.", exception);
        }
    }

    private static void ValidateIntegrityEvaluation(
        ContextPacket sourceContext,
        CandidatePerformance candidate,
        IntegrityValidationEvaluation integrityEvaluation)
    {
        try
        {
            var input = IntegrityCandidateInput.Bind(sourceContext, candidate);
            if (input.DeterministicRejectCodes.Length != 0)
            {
                throw new E0TurnInvariantException(
                    "Turn Candidate has an impossible deterministic Integrity rejection.");
            }

            var trace = integrityEvaluation.Trace
                ?? throw new E0TurnInvariantException("Turn Integrity trace is required.");
            var replay = DeterministicIntegrityValidator.Validate(input, trace.ConcernEvidence);
            if (replay.Disposition != integrityEvaluation.Disposition ||
                replay.Disposition == IntegrityDisposition.Reject)
            {
                throw new E0TurnInvariantException("Turn Integrity evaluation does not replay.");
            }
        }
        catch (IntegrityValidationException exception)
        {
            throw new E0TurnInvariantException("Turn Integrity evaluation is invalid.", exception);
        }
    }

    private static void ValidateInterpretationSource(
        ContextPacket sourceContext,
        CandidatePerformance candidate,
        IntegrityValidationEvaluation integrityEvaluation,
        StateInterpretationSource interpretationSource)
    {
        try
        {
            var replay = StateInterpretationSource.Bind(
                sourceContext,
                candidate,
                integrityEvaluation);
            if (!string.Equals(
                    replay.CandidateContentIdentityContract,
                    interpretationSource.CandidateContentIdentityContract,
                    StringComparison.Ordinal) ||
                !string.Equals(
                    replay.CandidateContentHash,
                    interpretationSource.CandidateContentHash,
                    StringComparison.Ordinal) ||
                replay.SourceSceneId != interpretationSource.SourceSceneId ||
                replay.SourceCharacterId != interpretationSource.SourceCharacterId ||
                replay.SourceContextPacketId != interpretationSource.SourceContextPacketId ||
                !replay.RosterCharacterIds.SequenceEqual(interpretationSource.RosterCharacterIds))
            {
                throw new E0TurnInvariantException("Turn interpretation source does not replay.");
            }
        }
        catch (StateInterpretationException exception)
        {
            throw new E0TurnInvariantException("Turn interpretation source is invalid.", exception);
        }
    }

    private static void ValidateAuthorityEvaluation(
        E0OpportunityBearingCycleState sourceCycle,
        StateInterpretationSource interpretationSource,
        StateInterpretationProposal proposal,
        StateAuthorityPolicy policy,
        StateAuthorityEvaluation authorityEvaluation)
    {
        try
        {
            var trace = authorityEvaluation.Trace
                ?? throw new E0TurnInvariantException("Turn State Authority trace is required.");
            if (!ReferenceEquals(trace.Policy, policy) || trace.ReviewSet is null)
            {
                throw new E0TurnInvariantException("Turn State Authority policy or review trace changed.");
            }

            var snapshot = StateAuthoritySnapshot.Bind(sourceCycle.ProductionState);
            var input = StateAuthorityInput.Bind(snapshot, interpretationSource, proposal);
            var reviewSet = StateAuthorityReviewSet.Bind(input, trace.ReviewSet.Choices);
            var replay = DeterministicStateAuthority.Evaluate(input, policy, reviewSet);
            if (replay.Status != authorityEvaluation.Status ||
                replay.Decisions.Length != authorityEvaluation.Decisions.Length)
            {
                throw new E0TurnInvariantException("Turn State Authority evaluation does not replay.");
            }

            for (var index = 0; index < replay.Decisions.Length; index++)
            {
                var left = replay.Decisions[index];
                var right = authorityEvaluation.Decisions[index];
                if (right is null ||
                    left.MutationIndex != right.MutationIndex ||
                    left.Disposition != right.Disposition ||
                    !left.Reasons.SequenceEqual(right.Reasons))
                {
                    throw new E0TurnInvariantException("Turn State Authority decisions do not replay.");
                }
            }
        }
        catch (StateAuthorityException exception)
        {
            throw new E0TurnInvariantException("Turn State Authority evaluation is invalid.", exception);
        }
    }
}

public sealed class E0TurnOrchestrationException : Exception
{
    internal E0TurnOrchestrationException(string message)
        : base(message)
    {
    }
}

internal sealed class E0TurnInvariantException : Exception
{
    internal E0TurnInvariantException(string message)
        : base(message)
    {
    }

    internal E0TurnInvariantException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
