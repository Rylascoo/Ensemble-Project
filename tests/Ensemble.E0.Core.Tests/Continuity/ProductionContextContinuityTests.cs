using System.Reflection;
using System.Text;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class ProductionContextContinuityTests
{
    [TestMethod]
    public void GenesisContinuity_BindsAccessPacketAndTraceToExactState()
    {
        var scenario = Patch0014TestSupport.Genesis();
        var access = scenario.Result.AccessEvaluation;
        var context = scenario.Result.ContextEvaluation;

        Assert.AreEqual(scenario.State.StateHash, access.Projection.SourceStateHash);
        Assert.AreEqual(scenario.State.StateHash, context.Packet.SourceStateHash);
        Assert.AreEqual(scenario.State.StateHash, context.Trace.SourceStateHash);
        Assert.AreEqual(scenario.Checkpoint.CurrentOpportunityCharacterId, context.Packet.SubjectCharacterId);
        Assert.AreEqual(scenario.Checkpoint.CurrentOpportunityCharacterId, context.Packet.OpportunityCharacterId);
        Assert.AreEqual(E0ContextContracts.ProductionBoundSchemaVersion, context.Packet.SchemaVersion);
        Assert.AreEqual(
            E0ContextContracts.ProductionBoundCompositionContract,
            context.Packet.CompositionContract);
        Assert.AreEqual(E0ContextContracts.RenderingContract, context.Packet.Rendered.RenderingContract);
    }

    [TestMethod]
    public void EvolvedContinuity_ComposesFromPatch0013CurrentOpportunityWithoutHistoryInputs()
    {
        var evolved = Patch0014TestSupport.Evolved("EVOLVED-CONTEXT");
        var packet = evolved.Result.ContextEvaluation.Packet;

        Assert.AreEqual(evolved.Opportunity.State.StateHash, packet.SourceStateHash);
        Assert.AreEqual(evolved.Checkpoint.CurrentOpportunityCharacterId, packet.SubjectCharacterId);
        Assert.AreEqual(MissingRaftContract.MarloweId, packet.SubjectCharacterId);
        Assert.AreEqual(string.Empty, packet.Rendered.RecentPerformanceText);
        Assert.AreEqual(
            "You have the current opportunity to act.",
            packet.Rendered.OpportunityText);
    }

    [TestMethod]
    public void GenesisV1AndV2_RenderedCanonicalBytesAndHashRemainIdentical()
    {
        var scenario = Patch0014TestSupport.Genesis();
        var v1 = Patch0012TestSupport.Compose(
            scenario.Fixture,
            scenario.Checkpoint.CurrentOpportunityCharacterId);
        var v2 = scenario.Result.ContextEvaluation.Packet;

        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeRendered(v1.Rendered),
            ContextPacketCanonicalizer.SerializeRendered(v2.Rendered));
        Assert.AreEqual(v1.RenderedContextHash, v2.RenderedContextHash);
        Assert.AreNotEqual(v1.StructuredContextHash, v2.StructuredContextHash);
        Assert.AreNotEqual(v1.ContextPacketId, v2.ContextPacketId);
    }

    [TestMethod]
    public void ProductionBoundStructuredJson_HasExactNewPrefixAndEmptyRecentPerformances()
    {
        var packet = Patch0014TestSupport.Genesis().Result.ContextEvaluation.Packet;
        var json = Encoding.UTF8.GetString(
            ContextPacketCanonicalizer.SerializeStructured(packet));
        var expectedPrefix =
            "{\"schemaVersion\":\"ensemble.e0.context.v2\",\"compositionContract\":\"ensemble.e0.context.production-bound.v1\",\"sourceStateHash\":\"" +
            packet.SourceStateHash!.Value.Value +
            "\",\"sceneId\":";

        Assert.IsTrue(json.StartsWith(expectedPrefix, StringComparison.Ordinal));
        StringAssert.EndsWith(json, "\"recentPerformances\":[]}");
        Assert.IsTrue(
            json.IndexOf("\"sourceStateHash\":", StringComparison.Ordinal) <
            json.IndexOf("\"sceneId\":", StringComparison.Ordinal));
    }

    [TestMethod]
    public void SourceStateHash_IsNeverRenderedAsCharacterFacingText()
    {
        var packet = Patch0014TestSupport.Genesis().Result.ContextEvaluation.Packet;
        var stateHash = packet.SourceStateHash!.Value.Value;
        var rendered = Encoding.UTF8.GetString(
            ContextPacketCanonicalizer.SerializeRendered(packet.Rendered));

        Assert.IsFalse(packet.Rendered.TrustedStateText.Contains(stateHash, StringComparison.Ordinal));
        Assert.IsFalse(packet.Rendered.OpportunityText.Contains(stateHash, StringComparison.Ordinal));
        Assert.IsFalse(packet.Rendered.RecentPerformanceText.Contains(stateHash, StringComparison.Ordinal));
        Assert.IsFalse(rendered.Contains(stateHash, StringComparison.Ordinal));
    }

    [TestMethod]
    public void HistoricalPublicComposer_RejectsProductionBackedProjection()
    {
        var scenario = Patch0014TestSupport.Genesis();

        Assert.Throws<ContextCompositionException>(() =>
            DeterministicContextComposer.Compose(
                scenario.Result.AccessEvaluation.Projection,
                scenario.Checkpoint.CurrentOpportunityCharacterId));
    }

    [TestMethod]
    public void GenericSmokeContinuity_IsProductionBoundAndDeterministic()
    {
        var fixture = Patch0014TestSupport.LoadGenericSmoke();
        var state = ProductionState.Initialize(fixture, System.Collections.Immutable.ImmutableArray<RecordId>.Empty);
        var checkpoint = ProductionStateCheckpoint.Capture(state);

        var first = E0ProductionContextContinuity.Compose(checkpoint);
        var second = E0ProductionContextContinuity.Compose(checkpoint);

        Assert.AreEqual(state.StateHash, first.ContextEvaluation.Packet.SourceStateHash);
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeStructured(first.ContextEvaluation.Packet),
            ContextPacketCanonicalizer.SerializeStructured(second.ContextEvaluation.Packet));
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeRendered(first.ContextEvaluation.Packet.Rendered),
            ContextPacketCanonicalizer.SerializeRendered(second.ContextEvaluation.Packet.Rendered));
    }

    [TestMethod]
    public void ProductionBoundContext_StillWorksWithPerformerCandidateContract()
    {
        var evolved = Patch0014TestSupport.Evolved("PERFORMER-V2");
        var take = Patch0014TestSupport.AcceptedTake(
            evolved.Opportunity.State,
            evolved.Result.ContextEvaluation.Packet,
            "PERFORMER-V2");

        Assert.AreEqual(
            evolved.Result.ContextEvaluation.Packet.ContextPacketId,
            take.Performance.ContextPacketId);
        Assert.AreEqual(evolved.Checkpoint.CurrentOpportunityCharacterId, take.Performance.SubjectCharacterId);
    }

    [TestMethod]
    public void ProductionBoundContractConstants_AreExactWithoutCompileTimeTautology()
    {
        var schema = typeof(E0ContextContracts).GetField(
            nameof(E0ContextContracts.ProductionBoundSchemaVersion),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Production-bound schema field is missing.");
        var composition = typeof(E0ContextContracts).GetField(
            nameof(E0ContextContracts.ProductionBoundCompositionContract),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Production-bound composition field is missing.");

        Assert.AreEqual("ensemble.e0.context.v2", schema.GetRawConstantValue());
        Assert.AreEqual(
            "ensemble.e0.context.production-bound.v1",
            composition.GetRawConstantValue());
        Assert.IsNull(typeof(E0ContextContracts).GetField(
            "ProductionBoundRenderingContract",
            BindingFlags.Public | BindingFlags.Static));
    }

    [TestMethod]
    public void ContinuityPublicSurface_IsClosedAndMinimal()
    {
        var assembly = typeof(E0ProductionContextContinuity).Assembly;
        CollectionAssert.AreEquivalent(
            new[]
            {
                nameof(E0ProductionContextContinuity),
                nameof(E0ProductionContextContinuityResult),
                nameof(E0ContextContinuityException)
            },
            assembly.GetExportedTypes()
                .Where(type => string.Equals(
                    type.Namespace,
                    "Ensemble.E0.Core.Continuity",
                    StringComparison.Ordinal))
                .Select(type => type.Name)
                .ToArray());

        Assert.AreEqual(0, typeof(E0ProductionContextContinuityResult)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        Assert.AreEqual(0, typeof(E0ContextContinuityException)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);

        var compose = typeof(E0ProductionContextContinuity)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Single();
        Assert.AreEqual(nameof(E0ProductionContextContinuity.Compose), compose.Name);
        CollectionAssert.AreEqual(
            new[] { typeof(ProductionStateCheckpoint) },
            compose.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(E0ProductionContextContinuityResult), compose.ReturnType);
    }
}
