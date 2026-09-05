using System.Collections.Immutable;
using System.Reflection;
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
using Ensemble.E0.Core.Tests.Continuity;
using Ensemble.E0.Core.Tests.Patch0012;
using Ensemble.E0.Core.Turn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Turn;

[TestClass]
public sealed class E0TurnDeterminismTests
{
    [TestMethod]
    public void IdenticalExplicitInputs_ProduceEquivalentTurnCommitAndSuccessorSemantics()
    {
        var left = RunTurn(
            NewCycle(),
            "No.",
            "TAKE-PATCH-0018-DETERMINISM",
            "COMMIT-PATCH-0018-DETERMINISM");
        var right = RunTurn(
            NewCycle(),
            "No.",
            "TAKE-PATCH-0018-DETERMINISM",
            "COMMIT-PATCH-0018-DETERMINISM");

        Assert.AreEqual(left.Context.ContextPacketId, right.Context.ContextPacketId);
        Assert.AreEqual(left.Candidate.VisibleText, right.Candidate.VisibleText);
        Assert.AreEqual(
            left.Ready.IntegrityEvaluation!.Trace.Input.CandidateContentHash,
            right.Ready.IntegrityEvaluation!.Trace.Input.CandidateContentHash);
        Assert.AreEqual(
            left.Bindable.AuthorityEvaluation!.Trace.Input.ProposalContentHash,
            right.Bindable.AuthorityEvaluation!.Trace.Input.ProposalContentHash);
        Assert.AreEqual(left.Accepted.AcceptedTake!.TakeId, right.Accepted.AcceptedTake!.TakeId);
        Assert.AreEqual(left.Post.ProductionState.StateHash, right.Post.ProductionState.StateHash);
        Assert.AreEqual(left.Next.ProductionState.StateHash, right.Next.ProductionState.StateHash);
        Assert.AreEqual(
            left.Next.ProductionState.CurrentOpportunityCharacterId,
            right.Next.ProductionState.CurrentOpportunityCharacterId);
    }

    [TestMethod]
    public void ThreeAcceptedTurns_PreserveInheritedOpportunityRotation()
    {
        var state = NewCycle();
        var observed = new List<string>
        {
            state.ProductionState.CurrentOpportunityCharacterId!.Value.Value
        };

        for (var index = 1; index <= 3; index++)
        {
            var turn = RunTurn(
                state,
                "No.",
                $"TAKE-PATCH-0018-ROTATION-{index}",
                $"COMMIT-PATCH-0018-ROTATION-{index}");
            state = turn.Next;
            observed.Add(state.ProductionState.CurrentOpportunityCharacterId!.Value.Value);
        }

        CollectionAssert.AreEqual(
            new[] { "VOSS", "MARLOWE", "WREN", "VOSS" },
            observed);
    }

    [TestMethod]
    public void ForgedSameStateContextPacket_CannotReenterTurnProgression()
    {
        var source = NewCycle();
        var current = Context(source);
        var forgedContext = ForgeContextWithDifferentId(current);
        var candidate = Candidate(forgedContext, "PRIVATE-FORGED-TEXT");
        var forgedProgress = ForgeCandidateReadyProgress(source, forgedContext, candidate);

        var exception = Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.EvaluateIntegrity(
                forgedProgress,
                ImmutableArray<IntegrityConcernKind>.Empty));

        Assert.AreEqual("E0 turn Integrity evaluation failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
        Assert.IsFalse(
            exception.ToString().Contains("PRIVATE-FORGED-TEXT", StringComparison.Ordinal));
        Assert.AreNotEqual(current.ContextPacketId, forgedContext.ContextPacketId);
    }

    [TestMethod]
    public void StaleTechnicalOutcome_CannotEnterNextCycle()
    {
        var source = NewCycle();
        var context = Context(source);
        var stale = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
            context,
            E0PerformerAttemptDisposition.TechnicalFailure);
        var accepted = Patch0015TestSupport.AcceptedTake(
            source.ProductionState,
            context,
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-PATCH-0018-STALE-TECHNICAL");
        var post = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0018-STALE-TECHNICAL"),
            source,
            context,
            accepted,
            Patch0015TestSupport.EmptyMaterializations());
        var next = DeterministicE0CausalCycle.EstablishOpportunity(post).State;

        var exception = Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.GateAttempt(next, stale));

        Assert.AreEqual("E0 turn attempt gate failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void MismatchedInterpreterProposal_FailsBeforeStateAuthorityAdoption()
    {
        var ready = ReadyForInterpretation(NewCycle(), "No.");
        var different = ReadyForInterpretation(NewCycle(), "Different candidate.");
        var mismatchedProposal = Proposal(different);

        var exception = Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.EvaluateAuthority(
                ready,
                mismatchedProposal,
                StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty),
                ImmutableArray<StateAuthorityReviewChoice>.Empty));

        Assert.AreEqual("E0 turn State Authority evaluation failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void DefaultTakeAndCommitIds_FailAtTheirApprovedStages()
    {
        var source = NewCycle();
        var ready = ReadyForInterpretation(source, "No.");
        var bindable = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            Proposal(ready),
            StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty),
            ImmutableArray<StateAuthorityReviewChoice>.Empty);

        var takeException = Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.BindAcceptedTake(default, bindable));
        Assert.AreEqual("E0 turn accepted Take binding failed.", takeException.Message);
        Assert.IsNull(takeException.InnerException);

        var accepted = DeterministicE0TurnOrchestrator.BindAcceptedTake(
            TakeId.From("TAKE-PATCH-0018-DEFAULT-COMMIT"),
            bindable);
        var commitException = Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.CommitAccepted(
                default,
                accepted,
                Patch0015TestSupport.EmptyMaterializations()));
        Assert.AreEqual("E0 turn accepted commit failed.", commitException.Message);
        Assert.IsNull(commitException.InnerException);
    }

    private static TurnRun RunTurn(
        E0OpportunityBearingCycleState source,
        string visibleText,
        string takeId,
        string commitId)
    {
        var context = Context(source);
        var candidate = Candidate(context, visibleText);
        var attempt = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);
        var gated = DeterministicE0TurnOrchestrator.GateAttempt(source, attempt);
        var ready = DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            gated,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var proposal = Proposal(ready);
        var bindable = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            proposal,
            StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty),
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        var accepted = DeterministicE0TurnOrchestrator.BindAcceptedTake(
            TakeId.From(takeId),
            bindable);
        var post = DeterministicE0TurnOrchestrator.CommitAccepted(
            CommitId.From(commitId),
            accepted,
            Patch0015TestSupport.EmptyMaterializations());
        var next = DeterministicE0CausalCycle.EstablishOpportunity(post).State;
        return new TurnRun(
            context,
            candidate,
            ready,
            bindable,
            accepted,
            post,
            next);
    }

    private static E0TurnProgress ReadyForInterpretation(
        E0OpportunityBearingCycleState source,
        string visibleText)
    {
        var context = Context(source);
        var candidate = Candidate(context, visibleText);
        var attempt = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);
        var gated = DeterministicE0TurnOrchestrator.GateAttempt(source, attempt);
        return DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            gated,
            ImmutableArray<IntegrityConcernKind>.Empty);
    }

    private static E0OpportunityBearingCycleState NewCycle() =>
        DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());

    private static ContextPacket Context(E0OpportunityBearingCycleState source) =>
        DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;

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

    private static StateInterpretationProposal Proposal(E0TurnProgress ready) =>
        StateInterpretationContract.ParseJson(
            ready.InterpretationSource!,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                schemaVersion = StateInterpretationContract.JsonSchemaVersion,
                mutations = Array.Empty<Dictionary<string, object?>>()
            })));

    private static ContextPacket ForgeContextWithDifferentId(ContextPacket source)
    {
        var constructor = typeof(ContextPacket)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        return (ContextPacket)constructor.Invoke(new object?[]
        {
            ContextPacketId.From("CTX-PATCH-0018-FORGED"),
            source.SchemaVersion,
            source.CompositionContract,
            source.SourceStateHash,
            source.SceneId,
            source.SubjectCharacterId,
            source.OpportunityCharacterId,
            source.Roster,
            source.SceneState,
            source.Pressures,
            source.Constitution,
            source.Disposition,
            source.Circumstance,
            source.Observations,
            source.Knowledge,
            source.Beliefs,
            source.Suspicions,
            source.Memories,
            source.Goals,
            source.Relationships,
            source.RecentPerformances,
            source.StructuredContextHash,
            source.Rendered,
            source.RenderedContextHash
        });
    }

    private static E0TurnProgress ForgeCandidateReadyProgress(
        E0OpportunityBearingCycleState source,
        ContextPacket context,
        CandidatePerformance candidate)
    {
        var constructor = typeof(E0TurnProgress)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        return (E0TurnProgress)constructor.Invoke(new object?[]
        {
            E0TurnProgressDisposition.CandidateReady,
            source,
            context,
            candidate,
            null,
            null,
            null,
            null,
            null,
            null
        });
    }

    private sealed record TurnRun(
        ContextPacket Context,
        CandidatePerformance Candidate,
        E0TurnProgress Ready,
        E0TurnProgress Bindable,
        E0TurnProgress Accepted,
        E0PostCommitCycleState Post,
        E0OpportunityBearingCycleState Next);
}
