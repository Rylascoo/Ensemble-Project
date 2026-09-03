using System.Collections.Immutable;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Opportunity;

[TestClass]
public sealed class Patch0013ReferenceOracleTests
{
    private const string ExpectedGenesisStateHash =
        "30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104";
    private const string ExpectedPatch0012PostCommitStateHash =
        "057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30";
    private const string ExpectedPatch0013OpportunityStateHash =
        "dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310";

    [TestMethod]
    public void MissingRaftPatch0012Oracle_ThenFallbackOpportunity_HasExactPatch0013Digest()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var genesis = Patch0012TestSupport.Genesis(fixture);
        var history = E0OpportunityHistory.Initialize(genesis);
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure },
            fixture: fixture);
        var take = Patch0012TestSupport.AcceptedTake(
            pipeline,
            "TAKE-PATCH-0012-ORACLE");
        var checkpoint = ProductionStateCheckpoint.Capture(genesis);
        var binding = E0TakeStateBinding.Bind(checkpoint, pipeline.Context, take);
        var materializations = E0RecordMaterializationSet.Bind(
            ImmutableArray.Create(
                E0RecordMaterialization.Create(
                    0,
                    RecordId.From("PRESSURE-PATCH-0012-ORACLE"))));
        var commit = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-PATCH-0012-ORACLE"),
            genesis,
            binding,
            materializations);

        Assert.AreEqual(ExpectedGenesisStateHash, genesis.StateHash.Value);
        Assert.AreEqual(
            ExpectedPatch0012PostCommitStateHash,
            commit.ResultState.StateHash.Value);

        var opportunity = DeterministicOpportunityAuthority.Establish(
            commit.ResultState,
            commit.Commit,
            pipeline.Context,
            history);

        Assert.AreEqual(MissingRaftContract.MarloweId, opportunity.Event.SelectedCharacterId);
        Assert.AreEqual(
            ExpectedPatch0013OpportunityStateHash,
            opportunity.Event.ResultStateHash.Value);
        Assert.AreEqual(
            ExpectedPatch0013OpportunityStateHash,
            opportunity.State.StateHash.Value);
        Assert.AreEqual(
            ExpectedPatch0013OpportunityStateHash,
            opportunity.History.LastOpportunityStateHash.Value);
    }
}
