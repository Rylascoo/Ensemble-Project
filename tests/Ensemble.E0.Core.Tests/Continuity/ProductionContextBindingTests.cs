using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class ProductionContextBindingTests
{
    [TestMethod]
    public void ExactProductionBoundV2_BindsToEvolvedCheckpoint()
    {
        var evolved = Patch0014TestSupport.Evolved("BIND-V2");
        var context = evolved.Result.ContextEvaluation.Packet;
        var take = Patch0014TestSupport.AcceptedTake(
            evolved.Opportunity.State,
            context,
            "BIND-V2");

        var binding = E0TakeStateBinding.Bind(evolved.Checkpoint, context, take);

        Assert.AreEqual(evolved.Checkpoint.StateHash, binding.SourceStateHash);
        Assert.AreSame(take, binding.Take);
    }

    [TestMethod]
    public void SameSourceStateHash_WithTamperedDisclosedContent_FailsExactRecomposition()
    {
        var evolved = Patch0014TestSupport.Evolved("BIND-TAMPER");
        var original = evolved.Result.ContextEvaluation.Packet;
        var tampered = CloneWithFirstSceneStateText(
            original,
            original.SceneState[0].Text + " Tampered.");
        var take = Patch0014TestSupport.AcceptedTake(
            evolved.Opportunity.State,
            original,
            "BIND-TAMPER");

        Assert.AreEqual(original.SourceStateHash, tampered.SourceStateHash);
        Assert.AreEqual(original.ContextPacketId, tampered.ContextPacketId);
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(evolved.Checkpoint, tampered, take));
    }

    [TestMethod]
    public void SameStructuredIdentity_WithTamperedRenderedBytes_FailsExactRecomposition()
    {
        var evolved = Patch0014TestSupport.Evolved("BIND-RENDER-TAMPER");
        var original = evolved.Result.ContextEvaluation.Packet;
        var tamperedRendered = ConstructNonPublic<RenderedContext>(
            original.Rendered.RenderingContract,
            original.Rendered.TrustedStateText,
            original.Rendered.RecentPerformanceText,
            original.Rendered.OpportunityText + " Tampered.");
        var tampered = Clone(
            original,
            original.SchemaVersion,
            original.CompositionContract,
            original.SceneState,
            rendered: tamperedRendered);
        var take = Patch0014TestSupport.AcceptedTake(
            evolved.Opportunity.State,
            original,
            "BIND-RENDER-TAMPER");

        Assert.AreEqual(original.ContextPacketId, tampered.ContextPacketId);
        Assert.AreEqual(original.StructuredContextHash, tampered.StructuredContextHash);
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(evolved.Checkpoint, tampered, take));
    }

    [TestMethod]
    public void ExactBytes_WithTamperedStoredHashes_FailExactRecomposition()
    {
        var evolved = Patch0014TestSupport.Evolved("BIND-HASH-TAMPER");
        var original = evolved.Result.ContextEvaluation.Packet;
        var take = Patch0014TestSupport.AcceptedTake(
            evolved.Opportunity.State,
            original,
            "BIND-HASH-TAMPER");

        var structuredHashTamper = Clone(
            original,
            original.SchemaVersion,
            original.CompositionContract,
            original.SceneState,
            structuredContextHash: new string('0', 64));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(evolved.Checkpoint, structuredHashTamper, take));

        var renderedHashTamper = Clone(
            original,
            original.SchemaVersion,
            original.CompositionContract,
            original.SceneState,
            renderedContextHash: new string('0', 64));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(evolved.Checkpoint, renderedHashTamper, take));
    }

    [TestMethod]
    public void EvolvedCheckpoint_RejectsHistoricalV1EvenWhenSubjectAndSceneMatch()
    {
        var evolved = Patch0014TestSupport.Evolved("BIND-V1-EVOLVED");
        var v1 = Patch0012TestSupport.Compose(
            evolved.Scenario.Fixture,
            evolved.Checkpoint.CurrentOpportunityCharacterId);
        var take = Patch0014TestSupport.AcceptedTake(
            evolved.Opportunity.State,
            v1,
            "BIND-V1-EVOLVED");

        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(evolved.Checkpoint, v1, take));
    }

    [TestMethod]
    public void ExactGenesisProductionDerivedV1_RemainsBindCompatible()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var context = Patch0012TestSupport.Compose(
            fixture,
            checkpoint.CurrentOpportunityCharacterId);
        var take = Patch0014TestSupport.AcceptedTake(
            state,
            context,
            "BIND-V1-GENESIS");

        var binding = E0TakeStateBinding.Bind(checkpoint, context, take);

        Assert.AreEqual(state.StateHash, binding.SourceStateHash);
    }

    [TestMethod]
    public void GenesisCheckpoint_RejectsForeignSemanticallyDifferentV1Context()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var foreignFixture = MutatedMissingRaftFixture();
        var foreignContext = Patch0012TestSupport.Compose(
            foreignFixture,
            checkpoint.CurrentOpportunityCharacterId);
        var take = Patch0014TestSupport.AcceptedTake(
            state,
            foreignContext,
            "BIND-V1-FOREIGN");

        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(checkpoint, foreignContext, take));
    }

    [TestMethod]
    public void HybridV1PacketWithSourceStateHash_FailsCanonicalization()
    {
        var v2 = Patch0014TestSupport.Genesis().Result.ContextEvaluation.Packet;
        var hybrid = Clone(
            v2,
            schemaVersion: E0ContextContracts.SchemaVersion,
            compositionContract: E0ContextContracts.CompositionContract,
            sceneState: v2.SceneState);

        Assert.Throws<ContextCompositionException>(() =>
            ContextPacketCanonicalizer.SerializeStructured(hybrid));
    }

    [TestMethod]
    public void HybridV2PacketWithoutSourceStateHash_FailsCanonicalization()
    {
        var v2 = Patch0014TestSupport.Genesis().Result.ContextEvaluation.Packet;
        var hybrid = Clone(
            v2,
            schemaVersion: E0ContextContracts.ProductionBoundSchemaVersion,
            compositionContract: E0ContextContracts.ProductionBoundCompositionContract,
            sceneState: v2.SceneState,
            sourceStateHash: null,
            overrideSourceStateHash: true);

        Assert.Throws<ContextCompositionException>(() =>
            ContextPacketCanonicalizer.SerializeStructured(hybrid));
    }

    [TestMethod]
    public void UnsupportedSchemaCompositionCombination_FailsCanonicalization()
    {
        var v2 = Patch0014TestSupport.Genesis().Result.ContextEvaluation.Packet;
        var hybrid = Clone(
            v2,
            schemaVersion: E0ContextContracts.ProductionBoundSchemaVersion,
            compositionContract: E0ContextContracts.CompositionContract,
            sceneState: v2.SceneState);

        Assert.Throws<ContextCompositionException>(() =>
            ContextPacketCanonicalizer.SerializeStructured(hybrid));
    }

    private static ContextPacket CloneWithFirstSceneStateText(
        ContextPacket source,
        string replacementText)
    {
        var replacement = ConstructNonPublic<ContextRecord>(
            source.SceneState[0].RecordId,
            replacementText);
        var builder = source.SceneState.ToBuilder();
        builder[0] = replacement;
        return Clone(
            source,
            source.SchemaVersion,
            source.CompositionContract,
            builder.ToImmutable());
    }

    private static ContextPacket Clone(
        ContextPacket source,
        string schemaVersion,
        string compositionContract,
        ImmutableArray<ContextRecord> sceneState,
        Ensemble.E0.Core.Production.StateHash? sourceStateHash = null,
        bool overrideSourceStateHash = false,
        RenderedContext? rendered = null,
        string? structuredContextHash = null,
        string? renderedContextHash = null)
    {
        var constructor = typeof(ContextPacket)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        var effectiveSourceStateHash = overrideSourceStateHash
            ? sourceStateHash
            : source.SourceStateHash;

        return (ContextPacket)constructor.Invoke(new object?[]
        {
            source.ContextPacketId,
            schemaVersion,
            compositionContract,
            effectiveSourceStateHash,
            source.SceneId,
            source.SubjectCharacterId,
            source.OpportunityCharacterId,
            source.Roster,
            sceneState,
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
            source.RecentPerformances,
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

    private static ValidatedFixture MutatedMissingRaftFixture()
    {
        var root = JsonNode.Parse(Patch0012TestSupport.ReadFixture("missing-raft-0.1.0.json"))!
            .AsObject();
        var voss = root["characters"]!
            .AsArray()
            .Select(node => node!.AsObject())
            .Single(character => string.Equals(
                character["id"]!.GetValue<string>(),
                MissingRaftContract.VossCharacterId,
                StringComparison.Ordinal));
        voss["goals"]!.AsArray()[0]!.AsObject()["text"] =
            "Voss now has a semantically different but structurally valid goal.";

        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(root.ToJsonString()));
        return GenericE0FixtureValidator.Validate(document);
    }
}
