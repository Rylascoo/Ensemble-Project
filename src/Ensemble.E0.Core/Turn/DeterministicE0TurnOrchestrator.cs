using System.Collections.Immutable;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Turn;

public static class DeterministicE0TurnOrchestrator
{
    public static E0TurnProgress GateAttempt(
        E0OpportunityBearingCycleState source,
        E0PerformerAttemptResult attemptResult)
    {
        if (source is null || attemptResult is null)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn attempt gate failed.");
        }

        try
        {
            var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
            if (attemptResult.SourceContextPacketId != context.ContextPacketId)
            {
                throw new E0TurnInvariantException("Turn attempt is stale for the source Cycle.");
            }

            return attemptResult.Disposition switch
            {
                E0PerformerAttemptDisposition.CandidateReady =>
                    GateCandidate(source, context, attemptResult),
                E0PerformerAttemptDisposition.TechnicalFailure =>
                    GateTechnical(
                        source,
                        context,
                        attemptResult,
                        E0TurnProgressDisposition.TechnicalFailure),
                E0PerformerAttemptDisposition.Cancelled =>
                    GateTechnical(
                        source,
                        context,
                        attemptResult,
                        E0TurnProgressDisposition.Cancelled),
                _ => throw new E0TurnInvariantException("Turn attempt disposition is unsupported.")
            };
        }
        catch (E0CausalCycleException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn attempt gate failed.");
        }
        catch (E0PerformerAttemptException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn attempt gate failed.");
        }
        catch (E0TurnInvariantException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn attempt gate failed.");
        }
    }

    public static E0TurnProgress EvaluateIntegrity(
        E0TurnProgress source,
        ImmutableArray<IntegrityConcernKind> concernKinds)
    {
        try
        {
            E0TurnProgress.Validate(source);
            if (source.Disposition != E0TurnProgressDisposition.CandidateReady ||
                source.Candidate is null ||
                concernKinds.IsDefault)
            {
                throw new E0TurnInvariantException("Turn Integrity source is invalid.");
            }

            var input = IntegrityCandidateInput.Bind(source.SourceContext, source.Candidate);
            if (input.DeterministicRejectCodes.Length != 0)
            {
                throw new E0TurnInvariantException(
                    "Turn Candidate has an impossible deterministic Integrity rejection.");
            }

            var evidence = IntegrityConcernEvidence.Bind(input, concernKinds);
            var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);
            if (evaluation.Disposition == IntegrityDisposition.Reject)
            {
                throw new E0TurnInvariantException("Turn Integrity produced an impossible Reject disposition.");
            }

            var interpretationSource = evaluation.Disposition == IntegrityDisposition.Accept
                ? StateInterpretationSource.Bind(
                    source.SourceContext,
                    source.Candidate,
                    evaluation)
                : null;

            return E0TurnProgress.CreateIntegrity(
                source,
                evaluation,
                interpretationSource);
        }
        catch (IntegrityValidationException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn Integrity evaluation failed.");
        }
        catch (StateInterpretationException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn Integrity evaluation failed.");
        }
        catch (E0TurnInvariantException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn Integrity evaluation failed.");
        }
    }

    public static E0TurnProgress EvaluateAuthority(
        E0TurnProgress source,
        StateInterpretationProposal proposal,
        StateAuthorityPolicy policy,
        ImmutableArray<StateAuthorityReviewChoice> reviewChoices)
    {
        try
        {
            E0TurnProgress.Validate(source);
            if (source.Disposition != E0TurnProgressDisposition.ReadyForInterpretation ||
                source.InterpretationSource is null ||
                proposal is null ||
                policy is null ||
                reviewChoices.IsDefault)
            {
                throw new E0TurnInvariantException("Turn State Authority source is invalid.");
            }

            var evaluation = EvaluateAuthorityCore(
                source,
                proposal,
                policy,
                reviewChoices);
            return E0TurnProgress.CreateAuthority(
                source,
                proposal,
                policy,
                evaluation);
        }
        catch (StateAuthorityException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn State Authority evaluation failed.");
        }
        catch (E0TurnInvariantException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn State Authority evaluation failed.");
        }
    }

    public static E0TurnProgress ResolveAuthorityReview(
        E0TurnProgress source,
        ImmutableArray<StateAuthorityReviewChoice> reviewChoices)
    {
        try
        {
            E0TurnProgress.Validate(source);
            if (source.Disposition != E0TurnProgressDisposition.AuthorityReviewRequired ||
                source.InterpretationProposal is null ||
                source.AuthorityPolicy is null ||
                reviewChoices.IsDefault)
            {
                throw new E0TurnInvariantException("Turn State Authority review source is invalid.");
            }

            var evaluation = EvaluateAuthorityCore(
                source,
                source.InterpretationProposal,
                source.AuthorityPolicy,
                reviewChoices);
            return E0TurnProgress.CreateAuthority(
                source,
                source.InterpretationProposal,
                source.AuthorityPolicy,
                evaluation);
        }
        catch (StateAuthorityException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn State Authority review resolution failed.");
        }
        catch (E0TurnInvariantException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn State Authority review resolution failed.");
        }
    }

    public static E0TurnProgress BindAcceptedTake(
        TakeId takeId,
        E0TurnProgress source)
    {
        try
        {
            E0TurnProgress.Validate(source);
            if (source.Disposition != E0TurnProgressDisposition.TakeBindable ||
                source.Candidate is null ||
                source.IntegrityEvaluation is null ||
                source.InterpretationProposal is null ||
                source.AuthorityEvaluation is null)
            {
                throw new E0TurnInvariantException("Turn Take-bindable source is invalid.");
            }

            var take = E0Take.Bind(
                takeId,
                source.SourceContext,
                source.Candidate,
                source.IntegrityEvaluation,
                source.InterpretationProposal,
                source.AuthorityEvaluation,
                E0TakeDisposition.Accepted);

            return E0TurnProgress.CreateAcceptedTake(source, take);
        }
        catch (E0TakeException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn accepted Take binding failed.");
        }
        catch (E0TurnInvariantException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn accepted Take binding failed.");
        }
    }

    public static E0PostCommitCycleState CommitAccepted(
        CommitId commitId,
        E0TurnProgress source,
        E0RecordMaterializationSet materializations)
    {
        try
        {
            E0TurnProgress.Validate(source);
            if (source.Disposition != E0TurnProgressDisposition.AcceptedTakeReady ||
                source.AcceptedTake is null ||
                materializations is null)
            {
                throw new E0TurnInvariantException("Turn Accepted-Take-ready source is invalid.");
            }

            return DeterministicE0CausalCycle.CommitAcceptedTake(
                commitId,
                source.SourceCycle,
                source.SourceContext,
                source.AcceptedTake,
                materializations);
        }
        catch (E0CausalCycleException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn accepted commit failed.");
        }
        catch (E0TurnInvariantException)
        {
            throw new E0TurnOrchestrationException(
                "E0 turn accepted commit failed.");
        }
    }

    private static E0TurnProgress GateCandidate(
        E0OpportunityBearingCycleState source,
        Context.ContextPacket context,
        E0PerformerAttemptResult attemptResult)
    {
        if (attemptResult.Candidate is null)
        {
            throw new E0TurnInvariantException("Candidate-ready attempt has no Candidate.");
        }

        var replay = DeterministicE0PerformerAttemptBoundary.BindCandidate(
            context,
            attemptResult.Candidate);
        if (!ReferenceEquals(replay.Candidate, attemptResult.Candidate))
        {
            throw new E0TurnInvariantException("Candidate-ready replay changed the Candidate.");
        }

        return E0TurnProgress.CreateCandidate(
            source,
            context,
            attemptResult.Candidate);
    }

    private static E0TurnProgress GateTechnical(
        E0OpportunityBearingCycleState source,
        Context.ContextPacket context,
        E0PerformerAttemptResult attemptResult,
        E0TurnProgressDisposition disposition)
    {
        if (attemptResult.Candidate is not null)
        {
            throw new E0TurnInvariantException("Technical attempt carries a Candidate.");
        }

        var performerDisposition = disposition == E0TurnProgressDisposition.TechnicalFailure
            ? E0PerformerAttemptDisposition.TechnicalFailure
            : E0PerformerAttemptDisposition.Cancelled;
        _ = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
            context,
            performerDisposition);

        return E0TurnProgress.CreateTechnical(
            source,
            context,
            disposition);
    }

    private static StateAuthorityEvaluation EvaluateAuthorityCore(
        E0TurnProgress source,
        StateInterpretationProposal proposal,
        StateAuthorityPolicy policy,
        ImmutableArray<StateAuthorityReviewChoice> reviewChoices)
    {
        if (source.InterpretationSource is null)
        {
            throw new E0TurnInvariantException("Turn interpretation source is required.");
        }

        var snapshot = StateAuthoritySnapshot.Bind(source.SourceCycle.ProductionState);
        var input = StateAuthorityInput.Bind(
            snapshot,
            source.InterpretationSource,
            proposal);
        var reviewSet = StateAuthorityReviewSet.Bind(input, reviewChoices);
        return DeterministicStateAuthority.Evaluate(input, policy, reviewSet);
    }
}
