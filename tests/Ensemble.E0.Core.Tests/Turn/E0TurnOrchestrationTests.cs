using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Ensemble.E0.Core.Tests.Continuity;
using Ensemble.E0.Core.Tests.Patch0012;
using Ensemble.E0.Core.Turn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Turn;

[TestClass]
public sealed class E0TurnOrchestrationTests
{
    [TestMethod]
    public void GateAttempt_RecomposesCurrentContextAndRejectsStaleAttempt()
    {
        var source = NewCycle();
        var context = Context(source);
        var candidate = Candidate(context, "No.");
        var staleAttempt = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);
        var accepted = Patch0015TestSupport.AcceptedTake(
            source.ProductionState,
            context,
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-PATCH-0018-STALE");
        var post = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0018-STALE"),
            source,
            context,
            accepted,
            Patch0015TestSupport.EmptyMaterializations());
        var next = DeterministicE0CausalCycle.EstablishOpportunity(post).State;

        var exception = Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.GateAttempt(next, staleAttempt));

        Assert.AreEqual("E0 turn attempt gate failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
        Assert.AreNotEqual(context.ContextPacketId, Context(next).ContextPacketId);
    }

    [TestMethod]
    public void TechnicalFailureAndCancellation_ArePayloadFreeAndMutateNothing()
    {
        var source = NewCycle();
        var context = Context(source);
        var stateHash = source.ProductionState.StateHash;

        foreach (var (attemptDisposition, turnDisposition) in new[]
                 {
                     (E0PerformerAttemptDisposition.TechnicalFailure, E0TurnProgressDisposition.TechnicalFailure),
                     (E0PerformerAttemptDisposition.Cancelled, E0TurnProgressDisposition.Cancelled)
                 })
        {
            var attempt = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
                context,
                attemptDisposition);
            var progress = DeterministicE0TurnOrchestrator.GateAttempt(source, attempt);

            Assert.AreEqual(turnDisposition, progress.Disposition);
            Assert.AreSame(context, progress.SourceContext);
            Assert.IsNull(progress.Candidate);
            Assert.IsNull(progress.IntegrityEvaluation);
            Assert.IsNull(progress.InterpretationSource);
            Assert.IsNull(progress.InterpretationProposal);
            Assert.IsNull(progress.AuthorityEvaluation);
            Assert.IsNull(progress.AcceptedTake);
            Assert.AreEqual(stateHash, source.ProductionState.StateHash);
        }
    }

    [TestMethod]
    public void CandidateReady_EmptyIntegrityConcerns_BecomesReadyForInterpretation()
    {
        var (source, context, candidate, gated) = CandidateReady();

        var ready = DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            gated,
            ImmutableArray<IntegrityConcernKind>.Empty);

        Assert.AreEqual(E0TurnProgressDisposition.ReadyForInterpretation, ready.Disposition);
        Assert.AreSame(context, ready.SourceContext);
        Assert.AreSame(candidate, ready.Candidate);
        Assert.AreEqual(IntegrityDisposition.Accept, ready.IntegrityEvaluation!.Disposition);
        Assert.IsNotNull(ready.InterpretationSource);
        Assert.AreEqual(context.ContextPacketId, ready.InterpretationSource!.SourceContextPacketId);
        Assert.IsNull(ready.InterpretationProposal);
        Assert.IsNull(ready.AuthorityEvaluation);
        Assert.IsNull(ready.AcceptedTake);
        Assert.AreEqual(source.ProductionState.StateHash, source.ProductionState.StateHash);
    }

    [TestMethod]
    public void IntegrityConcern_RequestsAnotherTakeWithoutCreatingTakeOrMutation()
    {
        var (source, _, candidate, gated) = CandidateReady();
        var stateHash = source.ProductionState.StateHash;

        var result = DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            gated,
            ImmutableArray.Create(IntegrityConcernKind.IndeterminateSemanticIntegrity));

        Assert.AreEqual(E0TurnProgressDisposition.RequestAnotherTake, result.Disposition);
        Assert.AreSame(candidate, result.Candidate);
        Assert.AreEqual(IntegrityDisposition.RequestAnotherTake, result.IntegrityEvaluation!.Disposition);
        Assert.IsNull(result.InterpretationSource);
        Assert.IsNull(result.InterpretationProposal);
        Assert.IsNull(result.AuthorityEvaluation);
        Assert.IsNull(result.AcceptedTake);
        Assert.AreEqual(stateHash, source.ProductionState.StateHash);
    }

    [TestMethod]
    public void DefaultIntegrityConcerns_FailWithSanitizedMessage()
    {
        var (_, _, _, gated) = CandidateReady();

        var exception = Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.EvaluateIntegrity(
                gated,
                default));

        Assert.AreEqual("E0 turn Integrity evaluation failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void MandatoryReview_RemainsBoundToExactProposalAndPolicyUntilResolved()
    {
        var ready = ReadyForInterpretation();
        var proposal = Proposal(ready, Patch0012TestSupport.PressureAdd());
        var policy = StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty);

        var review = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            proposal,
            policy,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);

        Assert.AreEqual(E0TurnProgressDisposition.AuthorityReviewRequired, review.Disposition);
        Assert.AreSame(proposal, review.InterpretationProposal);
        Assert.AreEqual(StateAuthorityEvaluationStatus.ReviewRequired, review.AuthorityEvaluation!.Status);
        Assert.IsNull(review.AcceptedTake);

        var stillReview = DeterministicE0TurnOrchestrator.ResolveAuthorityReview(
            review,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        Assert.AreEqual(E0TurnProgressDisposition.AuthorityReviewRequired, stillReview.Disposition);
        Assert.AreSame(proposal, stillReview.InterpretationProposal);

        var resolved = DeterministicE0TurnOrchestrator.ResolveAuthorityReview(
            stillReview,
            ImmutableArray.Create(StateAuthorityReviewChoice.Approve(0)));
        Assert.AreEqual(E0TurnProgressDisposition.TakeBindable, resolved.Disposition);
        Assert.AreSame(proposal, resolved.InterpretationProposal);
        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, resolved.AuthorityEvaluation!.Status);
        Assert.IsNull(resolved.AcceptedTake);
    }

    [TestMethod]
    public void CompleteAuthority_IsTakeBindableBeforeTakeIdOrTakeExists()
    {
        var ready = ReadyForInterpretation();
        var proposal = Proposal(ready);
        var policy = StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty);

        var bindable = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            proposal,
            policy,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);

        Assert.AreEqual(E0TurnProgressDisposition.TakeBindable, bindable.Disposition);
        Assert.AreSame(proposal, bindable.InterpretationProposal);
        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, bindable.AuthorityEvaluation!.Status);
        Assert.IsNull(bindable.AcceptedTake);
    }

    [TestMethod]
    public void BindAcceptedTake_UsesReferenceAcceptedPolicyAndProjectsTakeReplay()
    {
        var bindable = TakeBindable();
        var beforeAuthority = bindable.AuthorityEvaluation;

        var accepted = DeterministicE0TurnOrchestrator.BindAcceptedTake(
            TakeId.From("TAKE-PATCH-0018-REFERENCE"),
            bindable);

        Assert.AreEqual(E0TurnProgressDisposition.AcceptedTakeReady, accepted.Disposition);
        Assert.IsNotNull(accepted.AcceptedTake);
        Assert.AreEqual(E0TakeDisposition.Accepted, accepted.AcceptedTake!.Disposition);
        Assert.AreSame(accepted.AcceptedTake.Performance, accepted.Candidate);
        Assert.AreSame(accepted.AcceptedTake.InterpretationProposal, accepted.InterpretationProposal);
        Assert.AreSame(accepted.AcceptedTake.AuthorityEvaluation, accepted.AuthorityEvaluation);
        Assert.AreNotSame(beforeAuthority, accepted.AuthorityEvaluation);
    }

    [TestMethod]
    public void HardRejectedMutation_DoesNotRejectReferenceTake()
    {
        var ready = ReadyForInterpretation();
        var proposal = Proposal(ready, Patch0012TestSupport.InvalidBeliefSupersede());
        var policy = StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty);
        var bindable = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            proposal,
            policy,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);

        Assert.AreEqual(E0TurnProgressDisposition.TakeBindable, bindable.Disposition);
        Assert.AreEqual(StateAuthorityDisposition.Rejected, bindable.AuthorityEvaluation!.Decisions.Single().Disposition);

        var accepted = DeterministicE0TurnOrchestrator.BindAcceptedTake(
            TakeId.From("TAKE-PATCH-0018-REJECTED-MUTATION"),
            bindable);

        Assert.AreEqual(E0TakeDisposition.Accepted, accepted.AcceptedTake!.Disposition);
        Assert.AreEqual(
            StateAuthorityDisposition.Rejected,
            accepted.AuthorityEvaluation!.Decisions.Single().Disposition);
    }

    [TestMethod]
    public void CommitAccepted_MatchesInheritedCycleCommitAndDoesNotEstablishOpportunity()
    {
        var accepted = AcceptedReady("TAKE-PATCH-0018-COMMIT");
        var materials = Patch0015TestSupport.EmptyMaterializations();
        var commitId = CommitId.From("COMMIT-PATCH-0018-COMMIT");

        var viaTurn = DeterministicE0TurnOrchestrator.CommitAccepted(
            commitId,
            accepted,
            materials);
        var direct = DeterministicE0CausalCycle.CommitAcceptedTake(
            commitId,
            SourceCycle(accepted),
            accepted.SourceContext,
            accepted.AcceptedTake!,
            materials);

        Assert.AreEqual(direct.ProductionState.StateHash, viaTurn.ProductionState.StateHash);
        Assert.AreEqual(direct.Commit.CommitId, viaTurn.Commit.CommitId);
        Assert.AreEqual(direct.Commit.Take.TakeId, viaTurn.Commit.Take.TakeId);
        Assert.IsFalse(viaTurn.ProductionState.CurrentOpportunityCharacterId.HasValue);

        var turnNext = DeterministicE0CausalCycle.EstablishOpportunity(viaTurn).State;
        var directNext = DeterministicE0CausalCycle.EstablishOpportunity(direct).State;
        Assert.AreEqual(directNext.ProductionState.StateHash, turnNext.ProductionState.StateHash);
        Assert.AreEqual(
            directNext.ProductionState.CurrentOpportunityCharacterId,
            turnNext.ProductionState.CurrentOpportunityCharacterId);
    }

    [TestMethod]
    public void CommitAccepted_RejectsEveryPreAcceptedProgressState()
    {
        var (source, context, candidate, candidateReady) = CandidateReady();
        var technical = DeterministicE0TurnOrchestrator.GateAttempt(
            source,
            DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
                context,
                E0PerformerAttemptDisposition.TechnicalFailure));
        var cancelled = DeterministicE0TurnOrchestrator.GateAttempt(
            source,
            DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
                context,
                E0PerformerAttemptDisposition.Cancelled));
        var request = DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            candidateReady,
            ImmutableArray.Create(IntegrityConcernKind.IndeterminateSemanticIntegrity));
        var ready = DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            candidateReady,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var review = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            Proposal(ready, Patch0012TestSupport.PressureAdd()),
            StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty),
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        var bindable = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            Proposal(ready),
            StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty),
            ImmutableArray<StateAuthorityReviewChoice>.Empty);

        foreach (var progress in new[] { technical, cancelled, candidateReady, request, ready, review, bindable })
        {
            var exception = Assert.Throws<E0TurnOrchestrationException>(() =>
                DeterministicE0TurnOrchestrator.CommitAccepted(
                    CommitId.From("COMMIT-PATCH-0018-PREACCEPTED"),
                    progress,
                    Patch0015TestSupport.EmptyMaterializations()));
            Assert.AreEqual("E0 turn accepted commit failed.", exception.Message);
            Assert.IsNull(exception.InnerException);
        }

        Assert.AreSame(candidate, candidateReady.Candidate);
    }

    [TestMethod]
    public void NullAndWrongStageInputs_UseOnlyApprovedSanitizedMessages()
    {
        var source = NewCycle();
        var context = Context(source);
        var attempt = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
            context,
            E0PerformerAttemptDisposition.TechnicalFailure);

        AssertMessage(
            "E0 turn attempt gate failed.",
            () => DeterministicE0TurnOrchestrator.GateAttempt(null!, attempt));
        AssertMessage(
            "E0 turn attempt gate failed.",
            () => DeterministicE0TurnOrchestrator.GateAttempt(source, null!));
        AssertMessage(
            "E0 turn Integrity evaluation failed.",
            () => DeterministicE0TurnOrchestrator.EvaluateIntegrity(null!, ImmutableArray<IntegrityConcernKind>.Empty));
        AssertMessage(
            "E0 turn State Authority evaluation failed.",
            () => DeterministicE0TurnOrchestrator.EvaluateAuthority(
                null!, null!, null!, ImmutableArray<StateAuthorityReviewChoice>.Empty));
        AssertMessage(
            "E0 turn State Authority review resolution failed.",
            () => DeterministicE0TurnOrchestrator.ResolveAuthorityReview(
                null!, ImmutableArray<StateAuthorityReviewChoice>.Empty));
        AssertMessage(
            "E0 turn accepted Take binding failed.",
            () => DeterministicE0TurnOrchestrator.BindAcceptedTake(default, null!));
        AssertMessage(
            "E0 turn accepted commit failed.",
            () => DeterministicE0TurnOrchestrator.CommitAccepted(
                default,
                null!,
                Patch0015TestSupport.EmptyMaterializations()));
    }

    private static E0OpportunityBearingCycleState NewCycle() =>
        DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());

    private static ContextPacket Context(E0OpportunityBearingCycleState source) =>
        DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;

    private static (E0OpportunityBearingCycleState Source, ContextPacket Context, CandidatePerformance Candidate, E0TurnProgress Progress)
        CandidateReady()
    {
        var source = NewCycle();
        var context = Context(source);
        var candidate = Candidate(context, "No.");
        var attempt = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);
        var progress = DeterministicE0TurnOrchestrator.GateAttempt(source, attempt);
        return (source, context, candidate, progress);
    }

    private static E0TurnProgress ReadyForInterpretation()
    {
        var (_, _, _, gated) = CandidateReady();
        return DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            gated,
            ImmutableArray<IntegrityConcernKind>.Empty);
    }

    private static E0TurnProgress TakeBindable()
    {
        var ready = ReadyForInterpretation();
        return DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            Proposal(ready),
            StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty),
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
    }

    private static E0TurnProgress AcceptedReady(string takeId)
    {
        var bindable = TakeBindable();
        return DeterministicE0TurnOrchestrator.BindAcceptedTake(
            TakeId.From(takeId),
            bindable);
    }

    private static E0OpportunityBearingCycleState SourceCycle(E0TurnProgress progress)
    {
        var field = typeof(E0TurnProgress)
            .GetProperty("SourceCycle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (E0OpportunityBearingCycleState)field!.GetValue(progress)!;
    }

    private static CandidatePerformance Candidate(ContextPacket context, string text) =>
        PerformerCandidateContract.ParseJson(
            context,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
                performance = new { text },
                control = new
                {
                    addressedCharacterIds = Array.Empty<string>(),
                    nominatedCharacterId = (string?)null
                }
            })));

    private static StateInterpretationProposal Proposal(
        E0TurnProgress ready,
        params Dictionary<string, object?>[] mutations) =>
        StateInterpretationContract.ParseJson(
            ready.InterpretationSource!,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                schemaVersion = StateInterpretationContract.JsonSchemaVersion,
                mutations
            })));

    private static void AssertMessage(string expected, Action action)
    {
        var exception = Assert.Throws<E0TurnOrchestrationException>(action);
        Assert.AreEqual(expected, exception.Message);
        Assert.IsNull(exception.InnerException);
    }
}
