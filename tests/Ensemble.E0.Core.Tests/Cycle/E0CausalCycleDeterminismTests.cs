using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Tests.Continuity;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Cycle;

[TestClass]
public sealed class E0CausalCycleDeterminismTests
{
    [TestMethod]
    public void IdenticalExplicitInputs_ProduceEquivalentCanonicalSuccessorsWithoutMutatingSource()
    {
        var genesis = Patch0012TestSupport.Genesis();
        var source = DeterministicE0CausalCycle.Initialize(genesis);
        var contextA = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var contextB = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;

        Assert.AreEqual(contextA.ContextPacketId, contextB.ContextPacketId);
        Assert.AreEqual(contextA.StructuredContextHash, contextB.StructuredContextHash);
        Assert.AreEqual(contextA.RenderedContextHash, contextB.RenderedContextHash);

        var takeA = Patch0015TestSupport.AcceptedTake(
            source.ProductionState,
            contextA,
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-PATCH-0016-DETERMINISM");
        var takeB = Patch0015TestSupport.AcceptedTake(
            source.ProductionState,
            contextB,
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-PATCH-0016-DETERMINISM");

        var postA = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0016-DETERMINISM"),
            source,
            contextA,
            takeA,
            Patch0015TestSupport.EmptyMaterializations());
        var postB = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0016-DETERMINISM"),
            source,
            contextB,
            takeB,
            Patch0015TestSupport.EmptyMaterializations());

        Assert.AreEqual(postA.ProductionState.StateHash, postB.ProductionState.StateHash);
        Assert.AreEqual(postA.Commit.CommitId, postB.Commit.CommitId);
        Assert.AreEqual(postA.Commit.ResultStateHash, postB.Commit.ResultStateHash);
        Assert.AreEqual(Patch0012TestSupport.Genesis().StateHash, source.ProductionState.StateHash);
        Assert.AreEqual(MissingRaftContract.VossId, source.ProductionState.CurrentOpportunityCharacterId);

        var nextA = DeterministicE0CausalCycle.EstablishOpportunity(postA);
        var nextB = DeterministicE0CausalCycle.EstablishOpportunity(postB);
        Assert.AreEqual(nextA.State.ProductionState.StateHash, nextB.State.ProductionState.StateHash);
        Assert.AreEqual(nextA.OpportunityEvent.SelectedCharacterId, nextB.OpportunityEvent.SelectedCharacterId);
        Assert.AreEqual(nextA.OpportunityEvent.ResultStateHash, nextB.OpportunityEvent.ResultStateHash);
        Assert.IsFalse(postA.ProductionState.CurrentOpportunityCharacterId.HasValue);
        Assert.IsFalse(postB.ProductionState.CurrentOpportunityCharacterId.HasValue);
    }

    [TestMethod]
    public void MixedOtherwiseValidHistoryTokens_FailClosedAtPublicContextBoundary()
    {
        var genesis = Patch0012TestSupport.Genesis();
        var source = DeterministicE0CausalCycle.Initialize(genesis);
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var take = Patch0015TestSupport.AcceptedTake(
            source.ProductionState,
            context,
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-PATCH-0016-MIXED");
        var post = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0016-MIXED"),
            source,
            context,
            take,
            Patch0015TestSupport.EmptyMaterializations());
        var next = DeterministicE0CausalCycle.EstablishOpportunity(post).State;

        var acceptedHistory = typeof(E0OpportunityBearingCycleState)
            .GetProperty("AcceptedPerformanceHistory", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(source)!;
        var opportunityHistory = typeof(E0OpportunityBearingCycleState)
            .GetProperty("OpportunityHistory", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(next)!;
        var constructor = typeof(E0OpportunityBearingCycleState)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        var forged = (E0OpportunityBearingCycleState)constructor.Invoke(new[]
        {
            next.ProductionState,
            acceptedHistory,
            opportunityHistory
        });

        var exception = Assert.Throws<E0CausalCycleException>(() =>
            DeterministicE0CausalCycle.ComposeContext(forged));

        Assert.AreEqual("E0 causal cycle Context composition failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void ThreeAcceptedCycles_PreserveLeastInterventionRecurrenceAndHistoryContinuity()
    {
        var current = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var selected = new List<CharacterId>();

        for (var turn = 1; turn <= 3; turn++)
        {
            var context = DeterministicE0CausalCycle.ComposeContext(current).ContextEvaluation.Packet;
            var take = Patch0015TestSupport.AcceptedTake(
                current.ProductionState,
                context,
                "No.",
                Array.Empty<Dictionary<string, object?>>(),
                Array.Empty<StateMutationDomain>(),
                $"TAKE-PATCH-0016-TURN-{turn}");
            var post = DeterministicE0CausalCycle.CommitAcceptedTake(
                CommitId.From($"COMMIT-PATCH-0016-TURN-{turn}"),
                current,
                context,
                take,
                Patch0015TestSupport.EmptyMaterializations());
            var established = DeterministicE0CausalCycle.EstablishOpportunity(post);
            selected.Add(established.OpportunityEvent.SelectedCharacterId);
            current = established.State;
        }

        CollectionAssert.AreEqual(
            new[]
            {
                MissingRaftContract.MarloweId,
                MissingRaftContract.WrenId,
                MissingRaftContract.VossId
            },
            selected.ToArray());

        var fourthContext = DeterministicE0CausalCycle.ComposeContext(current).ContextEvaluation.Packet;
        Assert.AreEqual(MissingRaftContract.VossId, fourthContext.SubjectCharacterId);
        Assert.AreEqual(3, fourthContext.RecentPerformances.Length);
        CollectionAssert.AreEqual(
            new[]
            {
                MissingRaftContract.VossId,
                MissingRaftContract.MarloweId,
                MissingRaftContract.WrenId
            },
            fourthContext.RecentPerformances.Select(item => item.SourceCharacterId).ToArray());
        CollectionAssert.AreEqual(
            new[] { "No.", "No.", "No." },
            fourthContext.RecentPerformances.Select(item => item.VisibleText).ToArray());
    }

    [TestMethod]
    public void ForgedPhaseTwoFailure_CannotMutateOrEraseValidPostcommitPredecessor()
    {
        var genesis = Patch0012TestSupport.Genesis();
        var source = DeterministicE0CausalCycle.Initialize(genesis);
        var sourceContext = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var take = Patch0015TestSupport.AcceptedTake(
            source.ProductionState,
            sourceContext,
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-PATCH-0016-PHASE-TWO");
        var post = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0016-PHASE-TWO"),
            source,
            sourceContext,
            take,
            Patch0015TestSupport.EmptyMaterializations());
        var committedHash = post.ProductionState.StateHash;

        var validNext = DeterministicE0CausalCycle.EstablishOpportunity(post);
        var wrongContext = DeterministicE0CausalCycle.ComposeContext(validNext.State).ContextEvaluation.Packet;
        var acceptedHistory = E0AcceptedPerformanceHistoryContinuity.RecordCommit(
            E0AcceptedPerformanceHistoryContinuity.Initialize(genesis),
            genesis,
            post.Commit);
        var sourceOpportunityHistory = E0OpportunityHistory.Initialize(genesis);
        var constructor = typeof(E0PostCommitCycleState)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        var forged = (E0PostCommitCycleState)constructor.Invoke(new object[]
        {
            post.ProductionState,
            acceptedHistory,
            post.Commit,
            wrongContext,
            sourceOpportunityHistory
        });

        var exception = Assert.Throws<E0CausalCycleException>(() =>
            DeterministicE0CausalCycle.EstablishOpportunity(forged));

        Assert.AreEqual("E0 causal cycle Opportunity establishment failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
        Assert.AreEqual(committedHash, forged.ProductionState.StateHash);
        Assert.AreEqual(committedHash, post.ProductionState.StateHash);
        Assert.IsFalse(post.ProductionState.CurrentOpportunityCharacterId.HasValue);
    }
}
