using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class Patch0015DeterminismTests
{
    [TestMethod]
    public void AcceptedHistoryV3_RepeatedCompositionIsByteIdentical()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("DETERMINISM-REPEAT", "First.");
        var checkpoint = ProductionStateCheckpoint.Capture(first.Opportunity.State);

        var expected = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;
        var actual = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;

        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeStructured(expected),
            ContextPacketCanonicalizer.SerializeStructured(actual));
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeRendered(expected.Rendered),
            ContextPacketCanonicalizer.SerializeRendered(actual.Rendered));
        Assert.AreEqual(expected.ContextPacketId, actual.ContextPacketId);
        Assert.AreEqual(expected.StructuredContextHash, actual.StructuredContextHash);
        Assert.AreEqual(expected.RenderedContextHash, actual.RenderedContextHash);
    }

    [TestMethod]
    public void AcceptedHistoryV3_IsCultureInvariant()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("DETERMINISM-CULTURE", "First.");
        var checkpoint = ProductionStateCheckpoint.Capture(first.Opportunity.State);
        var expected = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ar-SA");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ar-SA");

            var actual = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
                checkpoint,
                first.HistoryAfterOpportunity).ContextEvaluation.Packet;

            CollectionAssert.AreEqual(
                ContextPacketCanonicalizer.SerializeStructured(expected),
                ContextPacketCanonicalizer.SerializeStructured(actual));
            CollectionAssert.AreEqual(
                ContextPacketCanonicalizer.SerializeRendered(expected.Rendered),
                ContextPacketCanonicalizer.SerializeRendered(actual.Rendered));
            Assert.AreEqual(expected.ContextPacketId, actual.ContextPacketId);
            Assert.AreEqual(expected.StructuredContextHash, actual.StructuredContextHash);
            Assert.AreEqual(expected.RenderedContextHash, actual.RenderedContextHash);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    [TestMethod]
    public void AcceptedHistoryOrder_IsIntentionallyStructuredByteSignificant()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("DETERMINISM-ORDER-1", "First.");
        var second = Patch0015TestSupport.RunNextTurn(first, "DETERMINISM-ORDER-2", "Second.");
        var packet = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            ProductionStateCheckpoint.Capture(second.Opportunity.State),
            second.HistoryAfterOpportunity).ContextEvaluation.Packet;

        Assert.AreEqual(2, packet.RecentPerformances.Length);
        var reversed = packet.RecentPerformances.Reverse().ToImmutableArray();
        var reorderedPacket = CloneWithRecentPerformances(packet, reversed);

        CollectionAssert.AreNotEqual(
            ContextPacketCanonicalizer.SerializeStructured(packet),
            ContextPacketCanonicalizer.SerializeStructured(reorderedPacket));
    }

    private static ContextPacket CloneWithRecentPerformances(
        ContextPacket source,
        ImmutableArray<ContextRecentPerformance> recentPerformances)
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
            recentPerformances,
            source.StructuredContextHash,
            source.Rendered,
            source.RenderedContextHash
        });
    }
}
