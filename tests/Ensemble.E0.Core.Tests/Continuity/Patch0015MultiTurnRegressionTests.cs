using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class Patch0015MultiTurnRegressionTests
{
    [TestMethod]
    public void RepeatedSemanticItemsRemainDistinctAndRoutingRecurrenceRetainsSelfHistory()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("RECUR-1", "Repeat.");
        var second = Patch0015TestSupport.RunNextTurn(first, "RECUR-2", "Repeat.");
        var third = Patch0015TestSupport.RunNextTurn(second, "RECUR-3", "Repeat.");
        var fourth = Patch0015TestSupport.RunNextTurn(third, "RECUR-4", "Repeat.");

        Assert.AreEqual(MissingRaftContract.VossId, first.SourceState.CurrentOpportunityCharacterId);
        Assert.AreEqual(MissingRaftContract.MarloweId, second.SourceState.CurrentOpportunityCharacterId);
        Assert.AreEqual(MissingRaftContract.WrenId, third.SourceState.CurrentOpportunityCharacterId);
        Assert.AreEqual(MissingRaftContract.VossId, fourth.SourceState.CurrentOpportunityCharacterId);

        var fourthSourceContext = fourth.Context.ContextEvaluation.Packet;
        Assert.AreEqual(MissingRaftContract.VossId, fourthSourceContext.SubjectCharacterId);
        CollectionAssert.AreEqual(
            new[]
            {
                MissingRaftContract.VossId,
                MissingRaftContract.MarloweId,
                MissingRaftContract.WrenId
            },
            fourthSourceContext.RecentPerformances
                .Select(item => item.SourceCharacterId)
                .ToArray());
        Assert.IsTrue(fourthSourceContext.RecentPerformances.Any(item =>
            item.SourceCharacterId == fourthSourceContext.SubjectCharacterId));

        var afterFourth = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            ProductionStateCheckpoint.Capture(fourth.Opportunity.State),
            fourth.HistoryAfterOpportunity).ContextEvaluation.Packet;

        CollectionAssert.AreEqual(
            new[]
            {
                MissingRaftContract.VossId,
                MissingRaftContract.MarloweId,
                MissingRaftContract.WrenId,
                MissingRaftContract.VossId
            },
            afterFourth.RecentPerformances
                .Select(item => item.SourceCharacterId)
                .ToArray());
        CollectionAssert.AreEqual(
            new[] { "Repeat.", "Repeat.", "Repeat.", "Repeat." },
            afterFourth.RecentPerformances
                .Select(item => item.VisibleText)
                .ToArray());
        Assert.AreEqual(4, afterFourth.RecentPerformances.Length);
    }
}
