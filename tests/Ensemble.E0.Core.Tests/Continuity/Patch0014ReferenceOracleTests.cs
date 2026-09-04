using System.Collections.Immutable;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class Patch0014ReferenceOracleTests
{
    private const string ExpectedGenesisStateHash =
        "30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104";
    private const string ExpectedPatch0012PostCommitStateHash =
        "057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30";
    private const string ExpectedPatch0013OpportunityStateHash =
        "dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310";

    private const string ExpectedGenesisV2StructuredHash =
        "27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565";
    private const int ExpectedGenesisV2StructuredBytes = 2655;
    private const string ExpectedGenesisRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const int ExpectedGenesisRenderedBytes = 1905;

    private const string ExpectedEvolvedV2StructuredHash =
        "9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00";
    private const int ExpectedEvolvedV2StructuredBytes = 3456;
    private const string ExpectedEvolvedRenderedHash =
        "9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d";
    private const int ExpectedEvolvedRenderedBytes = 2389;

    [TestMethod]
    public void MissingRaftReferenceChain_HasExactProductionBoundContextOracles()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var genesis = Patch0012TestSupport.Genesis(fixture);
        var genesisCheckpoint = ProductionStateCheckpoint.Capture(genesis);
        var genesisContinuity = E0ProductionContextContinuity.Compose(genesisCheckpoint);
        var genesisPacket = genesisContinuity.ContextEvaluation.Packet;

        Assert.AreEqual(ExpectedGenesisStateHash, genesis.StateHash.Value);
        Assert.AreEqual(ExpectedGenesisV2StructuredHash, genesisPacket.StructuredContextHash);
        Assert.AreEqual(
            $"CTX:{ExpectedGenesisV2StructuredHash}",
            genesisPacket.ContextPacketId.Value);
        Assert.AreEqual(ExpectedGenesisRenderedHash, genesisPacket.RenderedContextHash);
        Assert.AreEqual(
            ExpectedGenesisV2StructuredBytes,
            ContextPacketCanonicalizer.SerializeStructured(genesisPacket).Length);
        Assert.AreEqual(
            ExpectedGenesisRenderedBytes,
            ContextPacketCanonicalizer.SerializeRendered(genesisPacket.Rendered).Length);

        var history = E0OpportunityHistory.Initialize(genesis);
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure },
            fixture: fixture);
        var take = Patch0012TestSupport.AcceptedTake(
            pipeline,
            "TAKE-PATCH-0012-ORACLE");
        var binding = E0TakeStateBinding.Bind(genesisCheckpoint, pipeline.Context, take);
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
            opportunity.State.StateHash.Value);

        var evolvedCheckpoint = ProductionStateCheckpoint.Capture(opportunity.State);
        var evolvedContinuity = E0ProductionContextContinuity.Compose(evolvedCheckpoint);
        var evolvedPacket = evolvedContinuity.ContextEvaluation.Packet;

        Assert.AreEqual(ExpectedEvolvedV2StructuredHash, evolvedPacket.StructuredContextHash);
        Assert.AreEqual(
            $"CTX:{ExpectedEvolvedV2StructuredHash}",
            evolvedPacket.ContextPacketId.Value);
        Assert.AreEqual(ExpectedEvolvedRenderedHash, evolvedPacket.RenderedContextHash);
        Assert.AreEqual(
            ExpectedEvolvedV2StructuredBytes,
            ContextPacketCanonicalizer.SerializeStructured(evolvedPacket).Length);
        Assert.AreEqual(
            ExpectedEvolvedRenderedBytes,
            ContextPacketCanonicalizer.SerializeRendered(evolvedPacket.Rendered).Length);
        CollectionAssert.AreEqual(
            new[] { "PRESSURE-ISOLATION", "PRESSURE-PATCH-0012-ORACLE" },
            evolvedPacket.Pressures.Select(record => record.RecordId.Value).ToArray());
    }
}
