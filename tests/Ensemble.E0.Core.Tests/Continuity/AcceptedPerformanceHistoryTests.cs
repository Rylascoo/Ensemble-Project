using System.Collections.Immutable;
using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.Tests.Opportunity;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class AcceptedPerformanceHistoryTests
{
    [TestMethod]
    public void HistoryToken_IsOpaqueAndInitializationIsExactGenesisOnly()
    {
        var genesis = Patch0014TestSupport.Genesis();
        var history = E0AcceptedPerformanceHistoryContinuity.Initialize(genesis.State);

        Assert.AreEqual(0, typeof(E0AcceptedPerformanceHistory)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        Assert.AreEqual(0, typeof(E0AcceptedPerformanceHistory)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length);
        Assert.AreEqual(0, typeof(E0AcceptedPerformanceHistory)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly).Length);
        Assert.AreEqual(0, typeof(E0AcceptedPerformanceHistoryException)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);

        var entries = Entries(history);
        Assert.IsTrue(entries.IsEmpty);
        Assert.AreEqual(genesis.State.SceneId, InternalSceneId(history));
        Assert.AreEqual(genesis.State.StateHash, InternalStateHash(history));

        var first = Patch0015TestSupport.FirstLiveTurn("INIT-NONGENESIS");
        Assert.Throws<E0AcceptedPerformanceHistoryException>(() =>
            E0AcceptedPerformanceHistoryContinuity.Initialize(first.Commit.ResultState));
    }

    [TestMethod]
    public void RecordCommit_AppendsExactlyOneCharacterSafePerformanceAndAdvancesAnchor()
    {
        var turn = Patch0015TestSupport.FirstLiveTurn("APPEND", "No.");
        var before = Entries(turn.SourceAcceptedHistory);
        var after = Entries(turn.HistoryAfterCommit);

        Assert.AreEqual(0, before.Length);
        Assert.AreEqual(1, after.Length);
        Assert.AreEqual(MissingRaftContract.VossId, after[0].SourceCharacterId);
        Assert.AreEqual("No.", after[0].VisibleText);
        Assert.AreEqual(turn.Commit.ResultState.StateHash, InternalStateHash(turn.HistoryAfterCommit));
        Assert.AreEqual(turn.SourceState.StateHash, InternalStateHash(turn.SourceAcceptedHistory));
    }

    [TestMethod]
    public void RecordCommit_RejectsStaleHistoryAndWrongParentEventWithoutMutatingSource()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("COMMIT-FAIL-1", "First.");
        var second = Patch0015TestSupport.RunNextTurn(first, "COMMIT-FAIL-2", "Second.");
        var originalHash = InternalStateHash(first.SourceAcceptedHistory);
        var originalEntries = Entries(first.SourceAcceptedHistory);

        Assert.Throws<E0AcceptedPerformanceHistoryException>(() =>
            E0AcceptedPerformanceHistoryContinuity.RecordCommit(
                first.HistoryAfterOpportunity,
                first.SourceState,
                first.Commit.Commit));

        Assert.Throws<E0AcceptedPerformanceHistoryException>(() =>
            E0AcceptedPerformanceHistoryContinuity.RecordCommit(
                first.SourceAcceptedHistory,
                first.SourceState,
                second.Commit.Commit));

        Assert.AreEqual(originalHash, InternalStateHash(first.SourceAcceptedHistory));
        CollectionAssert.AreEqual(originalEntries.ToArray(), Entries(first.SourceAcceptedHistory).ToArray());
    }

    [TestMethod]
    public void AcceptedTakeWithRejectedConsequence_StillAppendsPerformanceWithoutDurableRecordChange()
    {
        var turn = Patch0015TestSupport.FirstLiveTurn(
            suffix: "REJECTED-CONSEQUENCE",
            visibleText: "I said it.",
            mutations: new[] { Patch0012TestSupport.InvalidBeliefSupersede() },
            autoApproveDomains: Array.Empty<Ensemble.E0.Core.StateInterpreter.StateMutationDomain>(),
            materializations: Patch0015TestSupport.EmptyMaterializations());

        Assert.AreEqual(1, turn.Take.AuthorityEvaluation.Decisions.Length);
        Assert.AreEqual(
            StateAuthorityDisposition.Rejected,
            turn.Take.AuthorityEvaluation.Decisions[0].Disposition);
        CollectionAssert.AreEqual(
            turn.SourceState.Records.Select(record => record.RecordId).ToArray(),
            turn.Commit.ResultState.Records.Select(record => record.RecordId).ToArray());

        var entries = Entries(turn.HistoryAfterCommit);
        Assert.AreEqual(1, entries.Length);
        Assert.AreEqual(MissingRaftContract.VossId, entries[0].SourceCharacterId);
        Assert.AreEqual("I said it.", entries[0].VisibleText);
    }

    [TestMethod]
    public void RecordOpportunity_AppendsNothingAndCouplesRoutingCount()
    {
        var turn = Patch0015TestSupport.FirstLiveTurn("OPPORTUNITY", "No.");
        var afterCommit = Entries(turn.HistoryAfterCommit);
        var afterOpportunity = Entries(turn.HistoryAfterOpportunity);

        Assert.AreEqual(1, afterCommit.Length);
        Assert.AreEqual(1, afterOpportunity.Length);
        Assert.AreSame(afterCommit[0], afterOpportunity[0]);
        Assert.AreEqual(turn.Opportunity.State.StateHash, InternalStateHash(turn.HistoryAfterOpportunity));
        Assert.AreEqual(afterOpportunity.Length + 1, turn.Opportunity.History.CharacterIds.Length);
        Assert.AreEqual(turn.Opportunity.Event.SelectedCharacterId, turn.Opportunity.History.CharacterIds[^1]);
    }

    [TestMethod]
    public void RecordOpportunity_RejectsWrongRoutingHistoryAndForeignEventWithoutMutatingPostCommitHistory()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("OPPORTUNITY-FAIL-1", "First.");
        var foreign = Patch0015TestSupport.FirstLiveTurn("OPPORTUNITY-FAIL-FOREIGN", "Foreign.");
        var originalHash = InternalStateHash(first.HistoryAfterCommit);
        var originalEntries = Entries(first.HistoryAfterCommit);

        Assert.Throws<E0AcceptedPerformanceHistoryException>(() =>
            E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
                first.HistoryAfterCommit,
                first.Commit.ResultState,
                first.Commit.Commit,
                first.Opportunity.History,
                first.Opportunity.Event));

        Assert.Throws<E0AcceptedPerformanceHistoryException>(() =>
            E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
                first.HistoryAfterCommit,
                first.Commit.ResultState,
                first.Commit.Commit,
                first.SourceOpportunityHistory,
                foreign.Opportunity.Event));

        Assert.AreEqual(originalHash, InternalStateHash(first.HistoryAfterCommit));
        CollectionAssert.AreEqual(originalEntries.ToArray(), Entries(first.HistoryAfterCommit).ToArray());
    }

    [TestMethod]
    public void HistoricalV1SourceCommit_CannotEnterLiveAcceptedHistoryProjection()
    {
        var historical = Patch0013TestSupport.BuildScenario("HISTORY-OMITTING");
        var liveHistory = E0AcceptedPerformanceHistoryContinuity.Initialize(historical.GenesisState);

        Assert.Throws<E0AcceptedPerformanceHistoryException>(() =>
            E0AcceptedPerformanceHistoryContinuity.RecordCommit(
                liveHistory,
                historical.GenesisState,
                historical.SourceCommit));
    }

    [TestMethod]
    public void MultiTurnHistory_PreservesOrderRepeatsAndSelfHistoryWithoutDurablePromotion()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("MULTI-1", "First.");
        var second = Patch0015TestSupport.RunNextTurn(first, "MULTI-2", "Second.");
        var thirdCheckpoint = ProductionStateCheckpoint.Capture(second.Opportunity.State);
        var thirdContext = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            thirdCheckpoint,
            second.HistoryAfterOpportunity).ContextEvaluation.Packet;

        CollectionAssert.AreEqual(
            new[] { MissingRaftContract.VossId, MissingRaftContract.MarloweId },
            thirdContext.RecentPerformances.Select(item => item.SourceCharacterId).ToArray());
        CollectionAssert.AreEqual(
            new[] { "First.", "Second." },
            thirdContext.RecentPerformances.Select(item => item.VisibleText).ToArray());
        Assert.IsTrue(thirdContext.RecordsDoNotContainPerformanceText("First.", "Second."));
    }

    private static ImmutableArray<ContextRecentPerformance> Entries(E0AcceptedPerformanceHistory history) =>
        (ImmutableArray<ContextRecentPerformance>)(typeof(E0AcceptedPerformanceHistory)
            .GetProperty("Entries", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(history) ?? throw new InvalidOperationException("History Entries are unavailable."));

    private static Ensemble.E0.Core.Domain.SceneId InternalSceneId(E0AcceptedPerformanceHistory history) =>
        (Ensemble.E0.Core.Domain.SceneId)(typeof(E0AcceptedPerformanceHistory)
            .GetProperty("SceneId", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(history) ?? throw new InvalidOperationException("History SceneId is unavailable."));

    private static Ensemble.E0.Core.Production.StateHash InternalStateHash(E0AcceptedPerformanceHistory history) =>
        (Ensemble.E0.Core.Production.StateHash)(typeof(E0AcceptedPerformanceHistory)
            .GetProperty("CurrentStateHash", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(history) ?? throw new InvalidOperationException("History StateHash is unavailable."));
}

internal static class Patch0015ContextAssertions
{
    internal static bool RecordsDoNotContainPerformanceText(
        this ContextPacket packet,
        params string[] values)
    {
        var text = packet.SceneState.Select(record => record.Text)
            .Concat(packet.Pressures.Select(record => record.Text))
            .Concat(packet.Constitution.Select(record => record.Text))
            .Concat(packet.Disposition.Select(record => record.Text))
            .Concat(packet.Circumstance.Select(record => record.Text))
            .Concat(packet.Observations.Select(record => record.Text))
            .Concat(packet.Knowledge.Select(record => record.Text))
            .Concat(packet.Beliefs.Select(record => record.Text))
            .Concat(packet.Suspicions.Select(record => record.Text))
            .Concat(packet.Memories.Select(record => record.Text))
            .Concat(packet.Goals.Select(record => record.Text))
            .Concat(packet.Relationships.Select(record => record.Text))
            .ToArray();
        return values.All(value => !text.Contains(value, StringComparer.Ordinal));
    }
}
