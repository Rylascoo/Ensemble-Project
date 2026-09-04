using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class AcceptedHistoryBindingTests
{
    [TestMethod]
    public void GenesisV2_BindsOnlyThroughExactEmptyAcceptedHistory()
    {
        var genesis = Patch0014TestSupport.Genesis();
        var history = E0AcceptedPerformanceHistoryContinuity.Initialize(genesis.State);
        var context = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            genesis.Checkpoint,
            history).ContextEvaluation.Packet;
        var take = Patch0015TestSupport.AcceptedTake(
            genesis.State,
            context,
            "Continue.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<Ensemble.E0.Core.StateInterpreter.StateMutationDomain>(),
            "TAKE-PATCH-0015-BIND-GENESIS");

        var binding = E0TakeStateBinding.BindWithAcceptedHistory(
            genesis.Checkpoint,
            context,
            take,
            history);

        Assert.AreEqual(genesis.State.StateHash, binding.SourceStateHash);
        Assert.AreSame(take, binding.Take);
    }

    [TestMethod]
    public void GenesisV1_IsRejectedByHistoryAwareBinding()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var history = E0AcceptedPerformanceHistoryContinuity.Initialize(state);
        var v1 = Patch0012TestSupport.Compose(fixture, checkpoint.CurrentOpportunityCharacterId);
        var take = Patch0014TestSupport.AcceptedTake(state, v1, "LIVE-REJECT-V1");

        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.BindWithAcceptedHistory(checkpoint, v1, take, history));
    }

    [TestMethod]
    public void EvolvedV3_BindsAndHistoricalBindingRejectsIt()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("BIND-V3-SOURCE", "No.");
        var state = first.Opportunity.State;
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var context = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;
        var take = Patch0015TestSupport.AcceptedTake(
            state,
            context,
            "Second.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<Ensemble.E0.Core.StateInterpreter.StateMutationDomain>(),
            "TAKE-PATCH-0015-BIND-V3");

        var binding = E0TakeStateBinding.BindWithAcceptedHistory(
            checkpoint,
            context,
            take,
            first.HistoryAfterOpportunity);
        Assert.AreEqual(state.StateHash, binding.SourceStateHash);

        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(checkpoint, context, take));
    }

    [TestMethod]
    public void EvolvedV3_RejectsStaleAcceptedHistory()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("BIND-STALE-HISTORY", "No.");
        var state = first.Opportunity.State;
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var context = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;
        var take = Patch0015TestSupport.AcceptedTake(
            state,
            context,
            "Second.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<Ensemble.E0.Core.StateInterpreter.StateMutationDomain>(),
            "TAKE-PATCH-0015-BIND-STALE-HISTORY");

        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.BindWithAcceptedHistory(
                checkpoint,
                context,
                take,
                first.SourceAcceptedHistory));
    }

    [TestMethod]
    public void EvolvedV3_RejectsTamperedStructuredRenderedAndStoredHashes()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("BIND-V3-TAMPER", "No.");
        var state = first.Opportunity.State;
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var context = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;
        var take = Patch0015TestSupport.AcceptedTake(
            state,
            context,
            "Second.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<Ensemble.E0.Core.StateInterpreter.StateMutationDomain>(),
            "TAKE-PATCH-0015-BIND-V3-TAMPER");

        var alteredRecent = ImmutableArray.Create(
            ConstructNonPublic<ContextRecentPerformance>(
                context.RecentPerformances[0].SourceCharacterId,
                "Tampered."));
        var structuredTamper = CloneContext(
            context,
            recentPerformances: alteredRecent);
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.BindWithAcceptedHistory(
                checkpoint,
                structuredTamper,
                take,
                first.HistoryAfterOpportunity));

        var alteredRendered = ConstructNonPublic<RenderedContext>(
            context.Rendered.RenderingContract,
            context.Rendered.TrustedStateText,
            context.Rendered.RecentPerformanceText + " Tampered.",
            context.Rendered.OpportunityText);
        var renderedTamper = CloneContext(context, rendered: alteredRendered);
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.BindWithAcceptedHistory(
                checkpoint,
                renderedTamper,
                take,
                first.HistoryAfterOpportunity));

        var structuredHashTamper = CloneContext(
            context,
            structuredContextHash: new string('0', 64));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.BindWithAcceptedHistory(
                checkpoint,
                structuredHashTamper,
                take,
                first.HistoryAfterOpportunity));

        var renderedHashTamper = CloneContext(
            context,
            renderedContextHash: new string('0', 64));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.BindWithAcceptedHistory(
                checkpoint,
                renderedHashTamper,
                take,
                first.HistoryAfterOpportunity));
    }

    [TestMethod]
    public void V3PassesExistingCandidateAndTakePipelineWithoutSemanticRedesign()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("PIPELINE-V3", "No.");
        var state = first.Opportunity.State;
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var context = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;

        var candidate = PerformerCandidateContract.ParseJson(
            context,
            Encoding.UTF8.GetBytes(
                "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"Still here.\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}"));

        Assert.AreEqual(context.ContextPacketId, candidate.ContextPacketId);
        Assert.AreEqual(context.SubjectCharacterId, candidate.SubjectCharacterId);

        var take = Patch0015TestSupport.AcceptedTake(
            state,
            context,
            "Still here.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<Ensemble.E0.Core.StateInterpreter.StateMutationDomain>(),
            "TAKE-PATCH-0015-PIPELINE-V3");
        var binding = E0TakeStateBinding.BindWithAcceptedHistory(
            checkpoint,
            context,
            take,
            first.HistoryAfterOpportunity);

        Assert.AreSame(take, binding.Take);
    }

    private static ContextPacket CloneContext(
        ContextPacket source,
        ImmutableArray<ContextRecentPerformance>? recentPerformances = null,
        RenderedContext? rendered = null,
        string? structuredContextHash = null,
        string? renderedContextHash = null)
    {
        var constructor = typeof(ContextPacket)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();

        return (ContextPacket)constructor.Invoke(new object?[]
        {
            source.ContextPacketId,
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
            recentPerformances ?? source.RecentPerformances,
            structuredContextHash ?? source.StructuredContextHash,
            rendered ?? source.Rendered,
            renderedContextHash ?? source.RenderedContextHash
        });
    }

    private static T ConstructNonPublic<T>(params object?[] arguments)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == arguments.Length);
        return (T)constructor.Invoke(arguments);
    }
}
