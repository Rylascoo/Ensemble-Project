using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Performer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Opportunity;

[TestClass]
public sealed class Patch0013EdgeContractTests
{
    [TestMethod]
    public void ImmediateSameCharacterReselection_IsUnreachableUnderFrozenE0V1Inputs()
    {
        Assert.Throws<PerformerCandidateException>(() =>
            Patch0013TestSupport.BuildScenario(
                "SELF-NOMINATION",
                nominatedCharacterId: MissingRaftContract.VossCharacterId));

        Assert.Throws<PerformerCandidateException>(() =>
            Patch0013TestSupport.BuildScenario(
                "SELF-ADDRESS",
                addressedCharacterIds: new[] { MissingRaftContract.VossCharacterId }));

        var fallback = Patch0013TestSupport.Establish(
            Patch0013TestSupport.BuildScenario("SELF-FALLBACK"));
        Assert.AreNotEqual(
            MissingRaftContract.VossId,
            fallback.Event.SelectedCharacterId);
    }

    [TestMethod]
    public void DirectAddress_WithTwoNeverSeenCharactersUsesFrozenOrdinalTieBreak()
    {
        var result = Patch0013TestSupport.Establish(
            Patch0013TestSupport.BuildScenario(
                "DIRECT-TIE",
                addressedCharacterIds: new[]
                {
                    MissingRaftContract.WrenCharacterId,
                    MissingRaftContract.MarloweCharacterId
                }));

        Assert.AreEqual(
            LeastInterventionDirectorRule.DirectAddress,
            result.DirectorEvaluation.Trace.Rule);
        Assert.AreEqual(MissingRaftContract.MarloweId, result.Event.SelectedCharacterId);
    }

    [TestMethod]
    public void Replay_RejectsForeignSourceCommitAndMismatchedHistoryAnchor()
    {
        var scenario = Patch0013TestSupport.BuildScenario("REPLAY-EDGE");
        var live = Patch0013TestSupport.Establish(scenario);
        var foreign = Patch0013TestSupport.BuildScenario(
            "REPLAY-FOREIGN",
            candidateText: "A different accepted source performance.");

        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Replay(
                scenario.PostCommitState,
                foreign.SourceCommit,
                scenario.SourceHistory,
                live.Event));

        var advancedForeignHistory = Patch0013TestSupport.Establish(foreign).History;
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Replay(
                scenario.PostCommitState,
                scenario.SourceCommit,
                advancedForeignHistory,
                live.Event));
    }

    [TestMethod]
    public void SameSelectedCharacterFromDifferentAcceptedControlHistory_RemainsCausallyDistinct()
    {
        var directAddress = Patch0013TestSupport.Establish(
            Patch0013TestSupport.BuildScenario(
                "CONTROL-DIRECT",
                addressedCharacterIds: new[] { MissingRaftContract.WrenCharacterId }));
        var nomination = Patch0013TestSupport.Establish(
            Patch0013TestSupport.BuildScenario(
                "CONTROL-NOMINATE",
                nominatedCharacterId: MissingRaftContract.WrenCharacterId));

        Assert.AreEqual(MissingRaftContract.WrenId, directAddress.Event.SelectedCharacterId);
        Assert.AreEqual(MissingRaftContract.WrenId, nomination.Event.SelectedCharacterId);
        Assert.AreNotEqual(directAddress.Event.ParentStateHash, nomination.Event.ParentStateHash);
        Assert.AreNotEqual(directAddress.Event.ResultStateHash, nomination.Event.ResultStateHash);
    }
}
