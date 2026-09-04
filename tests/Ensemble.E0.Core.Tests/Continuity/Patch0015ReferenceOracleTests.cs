using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class Patch0015ReferenceOracleTests
{
    private const string ExpectedGenesisStateHash =
        "30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104";
    private const string ExpectedGenesisV2StructuredHash =
        "27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565";
    private const string ExpectedGenesisV2RenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const string ExpectedV2SourceCandidateHash =
        "6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1";
    private const string ExpectedV2SourceProposalHash =
        "ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3";
    private const string ExpectedLivePostCommitStateHash =
        "a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c";
    private const string ExpectedLiveOpportunityStateHash =
        "e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151";
    private const string ExpectedV3StructuredHash =
        "ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f";
    private const string ExpectedV3RenderedHash =
        "668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16";

    [TestMethod]
    public void MissingRaftV2SourceLiveBranch_HasExactPatch0015OracleLineage()
    {
        var first = Patch0015TestSupport.FirstOracleTurn();
        var sourceContext = first.Context.ContextEvaluation.Packet;

        Assert.AreEqual(ExpectedGenesisStateHash, first.SourceState.StateHash.Value);
        Assert.AreEqual(MissingRaftContract.VossId, sourceContext.SubjectCharacterId);
        Assert.AreEqual(E0ContextContracts.ProductionBoundSchemaVersion, sourceContext.SchemaVersion);
        Assert.AreEqual(ExpectedGenesisV2StructuredHash, sourceContext.StructuredContextHash);
        Assert.AreEqual(ExpectedGenesisV2RenderedHash, sourceContext.RenderedContextHash);
        Assert.AreEqual(
            $"CTX:{ExpectedGenesisV2StructuredHash}",
            sourceContext.ContextPacketId.Value);
        Assert.AreEqual(2655, ContextPacketCanonicalizer.SerializeStructured(sourceContext).Length);
        Assert.AreEqual(1905, ContextPacketCanonicalizer.SerializeRendered(sourceContext.Rendered).Length);

        Assert.AreEqual(
            ExpectedV2SourceCandidateHash,
            first.Take.InterpretationProposal.CandidateContentHash);
        Assert.AreEqual(
            ExpectedV2SourceProposalHash,
            first.Take.AuthorityEvaluation.Trace.Input.ProposalContentHash);

        Assert.AreEqual(
            ExpectedLivePostCommitStateHash,
            first.Commit.ResultState.StateHash.Value);
        Assert.AreEqual(
            ExpectedLivePostCommitStateHash,
            first.Commit.Commit.ResultStateHash.Value);

        Assert.AreEqual(MissingRaftContract.MarloweId, first.Opportunity.Event.SelectedCharacterId);
        Assert.AreEqual(
            ExpectedLiveOpportunityStateHash,
            first.Opportunity.Event.ResultStateHash.Value);
        Assert.AreEqual(
            ExpectedLiveOpportunityStateHash,
            first.Opportunity.State.StateHash.Value);
        Assert.AreEqual(
            ExpectedLiveOpportunityStateHash,
            first.Opportunity.History.LastOpportunityStateHash.Value);
        CollectionAssert.AreEqual(
            new[] { MissingRaftContract.VossId, MissingRaftContract.MarloweId },
            first.Opportunity.History.CharacterIds.ToArray());

        var nextContext = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            ProductionStateCheckpoint.Capture(first.Opportunity.State),
            first.HistoryAfterOpportunity).ContextEvaluation.Packet;

        Assert.AreEqual(MissingRaftContract.MarloweId, nextContext.SubjectCharacterId);
        Assert.AreEqual(E0ContextContracts.AcceptedHistorySchemaVersion, nextContext.SchemaVersion);
        Assert.AreEqual(
            E0ContextContracts.AcceptedHistoryCompositionContract,
            nextContext.CompositionContract);
        Assert.AreEqual(
            E0ContextContracts.AcceptedHistoryRenderingContract,
            nextContext.Rendered.RenderingContract);
        Assert.AreEqual(ExpectedLiveOpportunityStateHash, nextContext.SourceStateHash!.Value.Value);
        Assert.AreEqual(ExpectedV3StructuredHash, nextContext.StructuredContextHash);
        Assert.AreEqual(ExpectedV3RenderedHash, nextContext.RenderedContextHash);
        Assert.AreEqual($"CTX:{ExpectedV3StructuredHash}", nextContext.ContextPacketId.Value);
        Assert.AreEqual(3521, ContextPacketCanonicalizer.SerializeStructured(nextContext).Length);
        Assert.AreEqual(2443, ContextPacketCanonicalizer.SerializeRendered(nextContext.Rendered).Length);
        Assert.AreEqual(1, nextContext.RecentPerformances.Length);
        Assert.AreEqual(MissingRaftContract.VossId, nextContext.RecentPerformances[0].SourceCharacterId);
        Assert.AreEqual("No.", nextContext.RecentPerformances[0].VisibleText);
        Assert.AreEqual(
            "[RECENT PERFORMANCES]\nDr. Voss:\n[PERFORMANCE]\n- No.",
            nextContext.Rendered.RecentPerformanceText);
    }
}
