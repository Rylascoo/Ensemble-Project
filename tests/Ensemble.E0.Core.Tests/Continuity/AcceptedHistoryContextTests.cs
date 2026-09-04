using System.Text;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class AcceptedHistoryContextTests
{
    [TestMethod]
    public void EmptyGenesisHistory_ComposesExactHistoricalV2Identity()
    {
        var genesis = Patch0014TestSupport.Genesis();
        var history = E0AcceptedPerformanceHistoryContinuity.Initialize(genesis.State);
        var live = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            genesis.Checkpoint,
            history).ContextEvaluation.Packet;
        var historical = genesis.Result.ContextEvaluation.Packet;

        Assert.AreEqual(E0ContextContracts.ProductionBoundSchemaVersion, live.SchemaVersion);
        Assert.AreEqual(E0ContextContracts.ProductionBoundCompositionContract, live.CompositionContract);
        Assert.AreEqual(E0ContextContracts.RenderingContract, live.Rendered.RenderingContract);
        Assert.AreEqual(0, live.RecentPerformances.Length);
        Assert.AreEqual(string.Empty, live.Rendered.RecentPerformanceText);
        Assert.AreEqual(historical.ContextPacketId, live.ContextPacketId);
        Assert.AreEqual(historical.StructuredContextHash, live.StructuredContextHash);
        Assert.AreEqual(historical.RenderedContextHash, live.RenderedContextHash);
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeStructured(historical),
            ContextPacketCanonicalizer.SerializeStructured(live));
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeRendered(historical.Rendered),
            ContextPacketCanonicalizer.SerializeRendered(live.Rendered));
    }

    [TestMethod]
    public void FirstAcceptedPerformance_ProducesV3ForNextOpportunity()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("V3", "No.");
        var checkpoint = ProductionStateCheckpoint.Capture(first.Opportunity.State);
        var packet = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;

        Assert.AreEqual(MissingRaftContract.MarloweId, packet.SubjectCharacterId);
        Assert.AreEqual(E0ContextContracts.AcceptedHistorySchemaVersion, packet.SchemaVersion);
        Assert.AreEqual(E0ContextContracts.AcceptedHistoryCompositionContract, packet.CompositionContract);
        Assert.AreEqual(E0ContextContracts.AcceptedHistoryRenderingContract, packet.Rendered.RenderingContract);
        Assert.AreEqual(first.Opportunity.State.StateHash, packet.SourceStateHash);
        Assert.AreEqual(1, packet.RecentPerformances.Length);
        Assert.AreEqual(MissingRaftContract.VossId, packet.RecentPerformances[0].SourceCharacterId);
        Assert.AreEqual("No.", packet.RecentPerformances[0].VisibleText);
        Assert.AreEqual(
            "[RECENT PERFORMANCES]\nDr. Voss:\n[PERFORMANCE]\n- No.",
            packet.Rendered.RecentPerformanceText);
    }

    [TestMethod]
    public void V3StructuredJson_UsesExactFinalRecentItemShapeAndCausalOrder()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("JSON-1", "First.");
        var second = Patch0015TestSupport.RunNextTurn(first, "JSON-2", "Second.");
        var checkpoint = ProductionStateCheckpoint.Capture(second.Opportunity.State);
        var packet = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            second.HistoryAfterOpportunity).ContextEvaluation.Packet;
        var json = Encoding.UTF8.GetString(ContextPacketCanonicalizer.SerializeStructured(packet));

        Assert.IsTrue(json.StartsWith(
            "{\"schemaVersion\":\"ensemble.e0.context.v3\",\"compositionContract\":\"ensemble.e0.context.production-bound.accepted-history.v1\",\"sourceStateHash\":\"",
            StringComparison.Ordinal));
        StringAssert.EndsWith(
            json,
            "\"recentPerformances\":[{\"sourceCharacterId\":\"VOSS\",\"visibleText\":\"First.\"},{\"sourceCharacterId\":\"MARLOWE\",\"visibleText\":\"Second.\"}]}");
        Assert.IsTrue(
            json.IndexOf("\"sourceCharacterId\":\"VOSS\"", StringComparison.Ordinal) <
            json.IndexOf("\"sourceCharacterId\":\"MARLOWE\"", StringComparison.Ordinal));
    }

    [TestMethod]
    public void SemanticSilence_HasDistinctRenderingFromLiteralMarkerText()
    {
        var silent = Patch0015TestSupport.FirstLiveTurn("SILENCE", string.Empty);
        var silentPacket = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            ProductionStateCheckpoint.Capture(silent.Opportunity.State),
            silent.HistoryAfterOpportunity).ContextEvaluation.Packet;

        Assert.AreEqual(
            "[RECENT PERFORMANCES]\nDr. Voss:\n[PERFORMANCE: SILENCE]",
            silentPacket.Rendered.RecentPerformanceText);

        var literal = Patch0015TestSupport.FirstLiveTurn(
            "LITERAL-SILENCE",
            "[PERFORMANCE: SILENCE]");
        var literalPacket = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            ProductionStateCheckpoint.Capture(literal.Opportunity.State),
            literal.HistoryAfterOpportunity).ContextEvaluation.Packet;

        Assert.AreEqual(
            "[RECENT PERFORMANCES]\nDr. Voss:\n[PERFORMANCE]\n- [PERFORMANCE: SILENCE]",
            literalPacket.Rendered.RecentPerformanceText);
        Assert.AreNotEqual(
            silentPacket.StructuredContextHash,
            literalPacket.StructuredContextHash);
    }

    [TestMethod]
    public void MultilineAcceptedPerformance_UsesInheritedContinuationDiscipline()
    {
        var first = Patch0015TestSupport.FirstLiveTurn(
            "MULTILINE",
            "Line one\nLine two");
        var packet = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            ProductionStateCheckpoint.Capture(first.Opportunity.State),
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;

        Assert.AreEqual(
            "[RECENT PERFORMANCES]\nDr. Voss:\n[PERFORMANCE]\n- Line one\n  Line two",
            packet.Rendered.RecentPerformanceText);
        Assert.IsFalse(packet.Rendered.RecentPerformanceText.EndsWith('\n'));
        Assert.IsFalse(packet.Rendered.RecentPerformanceText.Contains('\r'));
    }

    [TestMethod]
    public void AcceptedPerformanceAndDurableConsequence_RemainIndependentLayers()
    {
        var first = Patch0015TestSupport.FirstOracleTurn();
        var packet = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            ProductionStateCheckpoint.Capture(first.Opportunity.State),
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;

        Assert.AreEqual("No.", packet.RecentPerformances.Single().VisibleText);
        Assert.IsTrue(packet.Pressures.Any(record =>
            string.Equals(record.Text, "Pressure increases.", StringComparison.Ordinal)));
        Assert.IsFalse(packet.Rendered.RecentPerformanceText.Contains(
            "Pressure increases.",
            StringComparison.Ordinal));
    }

    [TestMethod]
    public void EvolvedStateWithEmptyGenesisHistory_FailsClosed()
    {
        var first = Patch0015TestSupport.FirstLiveTurn("STALE-EMPTY");

        Assert.Throws<E0ContextContinuityException>(() =>
            E0ProductionContextContinuity.ComposeWithAcceptedHistory(
                ProductionStateCheckpoint.Capture(first.Opportunity.State),
                first.SourceAcceptedHistory));
    }
}
