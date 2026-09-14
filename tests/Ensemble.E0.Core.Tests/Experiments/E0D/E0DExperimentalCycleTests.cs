using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Experiments.E0D;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Tests.Patch0012;

namespace Ensemble.E0.Core.Tests.Experiments.E0D;

[TestClass]
public sealed class E0DExperimentalCycleTests
{
    [TestMethod]
    public void FullReferenceContext_IsExactReferencePath()
    {
        var state = GenesisCycle();
        var expected = DeterministicE0CausalCycle.ComposeContext(state);
        var actual = E0DExperimentalCycle.ComposeContext(state, E0DExperimentVariant.FullReference);

        Assert.AreEqual(expected.ContextEvaluation.Packet.ContextPacketId, actual.ContextEvaluation.Packet.ContextPacketId);
        Assert.AreEqual(expected.ContextEvaluation.Packet.StructuredContextHash, actual.ContextEvaluation.Packet.StructuredContextHash);
        Assert.AreEqual(expected.ContextEvaluation.Packet.RenderedContextHash, actual.ContextEvaluation.Packet.RenderedContextHash);
        Assert.AreEqual(expected.ContextEvaluation.Trace.CompositionContract, actual.ContextEvaluation.Trace.CompositionContract);
        CollectionAssert.AreEqual(
            expected.ContextEvaluation.Trace.IncludedRecordIds.Select(x => x.Value).ToArray(),
            actual.ContextEvaluation.Trace.IncludedRecordIds.Select(x => x.Value).ToArray());
    }

    [TestMethod]
    public void RelationshipOmission_RemovesOnlyFrozenRelationshipSurface()
    {
        var state = GenesisCycle();
        var full = E0DExperimentalCycle.ComposeContext(state, E0DExperimentVariant.FullReference).ContextEvaluation.Packet;
        var omitted = E0DExperimentalCycle.ComposeContext(state, E0DExperimentVariant.RelationshipsOmitted).ContextEvaluation.Packet;
        var excluded = MissingRaftContract.RelationshipContextRecordIds.Select(x => x.Value).ToHashSet(StringComparer.Ordinal);
        var fullIds = RecordIds(full);
        var omittedIds = RecordIds(omitted);
        var expected = fullIds.Where(x => !excluded.Contains(x)).OrderBy(x => x, StringComparer.Ordinal).ToArray();

        CollectionAssert.AreEqual(expected, omittedIds.OrderBy(x => x, StringComparer.Ordinal).ToArray());
        Assert.IsTrue(fullIds.Any(excluded.Contains));
        Assert.AreEqual(full.SourceStateHash, omitted.SourceStateHash);
        Assert.AreEqual(full.SubjectCharacterId, omitted.SubjectCharacterId);
        Assert.AreEqual(full.OpportunityCharacterId, omitted.OpportunityCharacterId);
        CollectionAssert.AreEqual(full.Roster.Select(x => x.CharacterId).ToArray(), omitted.Roster.Select(x => x.CharacterId).ToArray());
        CollectionAssert.AreEqual(full.RecentPerformances.ToArray(), omitted.RecentPerformances.ToArray());
        Assert.AreEqual(E0DExperimentContracts.RelationshipsOmittedCompositionContract, omitted.CompositionContract);
    }

    [TestMethod]
    public void OmniscientContext_IsExactCategoryPreservingSafeProjectionUnion()
    {
        var state = GenesisCycle();
        var production = state.ProductionState;
        var actual = E0DExperimentalCycle.ComposeContext(state, E0DExperimentVariant.OmniscientContext).ContextEvaluation.Packet;
        var safe = production.RosterCharacterIds
            .Select(id => CharacterBoundedAccessControl.Evaluate(production, id).Projection)
            .ToArray();

        AssertCategoryUnion(safe.Select(x => x.SceneState), actual.SceneState);
        AssertCategoryUnion(safe.Select(x => x.Pressures), actual.Pressures);
        AssertCategoryUnion(safe.Select(x => x.Constitution), actual.Constitution);
        AssertCategoryUnion(safe.Select(x => x.Disposition), actual.Disposition);
        AssertCategoryUnion(safe.Select(x => x.Circumstance), actual.Circumstance);
        AssertCategoryUnion(safe.Select(x => x.Observations), actual.Observations);
        AssertCategoryUnion(safe.Select(x => x.Knowledge), actual.Knowledge);
        AssertCategoryUnion(safe.Select(x => x.Beliefs), actual.Beliefs);
        AssertCategoryUnion(safe.Select(x => x.Suspicions), actual.Suspicions);
        AssertCategoryUnion(safe.Select(x => x.Memories), actual.Memories);
        AssertCategoryUnion(safe.Select(x => x.Goals), actual.Goals);
        AssertRelationshipUnion(safe.Select(x => x.Relationships), actual.Relationships);

        var safeUnion = safe.SelectMany(ProjectionRecordIds).ToHashSet(StringComparer.Ordinal);
        Assert.IsTrue(RecordIds(actual).All(safeUnion.Contains));
        Assert.IsFalse(RecordIds(actual).Contains(MissingRaftContract.HtMarloweReleasedRaftId));
        Assert.AreEqual(E0DExperimentContracts.OmniscientCompositionContract, actual.CompositionContract);
    }

    [TestMethod]
    public void RoundRobin_IgnoresSocialControlAndFollowsFrozenTwelveTurnSchedule()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var expectedSubjects = new[]
        {
            "VOSS", "WREN", "MARLOWE", "VOSS", "WREN", "MARLOWE",
            "VOSS", "WREN", "MARLOWE", "VOSS", "WREN", "MARLOWE"
        };
        var expectedNext = new[]
        {
            "WREN", "MARLOWE", "VOSS", "WREN", "MARLOWE", "VOSS",
            "WREN", "MARLOWE", "VOSS", "WREN", "MARLOWE", "VOSS"
        };
        var history = ImmutableArray.CreateBuilder<CharacterId>();

        for (var turn = 0; turn < expectedSubjects.Length; turn++)
        {
            var subject = CharacterId.From(expectedSubjects[turn]);
            history.Add(subject);
            var context = Patch0012TestSupport.Compose(fixture, subject);
            var candidate = PerformerCandidateContract.ParseJson(
                context,
                Encoding.UTF8.GetBytes(CandidateJson(subject)));
            var input = DirectorOpportunityInput.Bind(context, candidate, history.ToImmutable());
            var evaluation = E0DExperimentalCycle.EvaluateRoundRobin(input);
            var proposal = evaluation.Proposal;

            Assert.AreEqual(expectedNext[turn], proposal.SelectedCharacterId.Value);
            Assert.AreEqual(subject, proposal.SourceCharacterId);
            Assert.AreEqual(E0DirectorContracts.OpportunityContractVersion, proposal.ContractVersion);
            Assert.AreEqual(E0DExperimentContracts.RoundRobinStrategyContract, evaluation.Trace.StrategyContract);
            Assert.AreSame(input, evaluation.Trace.Input);
            CollectionAssert.AreEqual(
                new[] { "MARLOWE", "VOSS", "WREN" },
                evaluation.Trace.CanonicalRoster.Select(x => x.Value).ToArray());
            Assert.AreEqual(Array.IndexOf(new[] { "MARLOWE", "VOSS", "WREN" }, subject.Value), evaluation.Trace.SourceIndex);
        }
    }

    private static E0OpportunityBearingCycleState GenesisCycle()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var production = ProductionState.Initialize(fixture, ImmutableArray<RecordId>.Empty);
        return DeterministicE0CausalCycle.Initialize(production);
    }

    private static HashSet<string> RecordIds(ContextPacket packet) =>
        ProjectionRecordIds(packet).ToHashSet(StringComparer.Ordinal);

    private static IEnumerable<string> ProjectionRecordIds(ContextPacket packet) =>
        packet.SceneState.Select(x => x.RecordId.Value)
            .Concat(packet.Pressures.Select(x => x.RecordId.Value))
            .Concat(packet.Constitution.Select(x => x.RecordId.Value))
            .Concat(packet.Disposition.Select(x => x.RecordId.Value))
            .Concat(packet.Circumstance.Select(x => x.RecordId.Value))
            .Concat(packet.Observations.Select(x => x.RecordId.Value))
            .Concat(packet.Knowledge.Select(x => x.RecordId.Value))
            .Concat(packet.Beliefs.Select(x => x.RecordId.Value))
            .Concat(packet.Suspicions.Select(x => x.RecordId.Value))
            .Concat(packet.Memories.Select(x => x.RecordId.Value))
            .Concat(packet.Goals.Select(x => x.RecordId.Value))
            .Concat(packet.Relationships.Select(x => x.RecordId.Value));

    private static IEnumerable<string> ProjectionRecordIds(CharacterAccessProjection projection) =>
        projection.SceneState.Select(x => x.RecordId.Value)
            .Concat(projection.Pressures.Select(x => x.RecordId.Value))
            .Concat(projection.Constitution.Select(x => x.RecordId.Value))
            .Concat(projection.Disposition.Select(x => x.RecordId.Value))
            .Concat(projection.Circumstance.Select(x => x.RecordId.Value))
            .Concat(projection.Observations.Select(x => x.RecordId.Value))
            .Concat(projection.Knowledge.Select(x => x.RecordId.Value))
            .Concat(projection.Beliefs.Select(x => x.RecordId.Value))
            .Concat(projection.Suspicions.Select(x => x.RecordId.Value))
            .Concat(projection.Memories.Select(x => x.RecordId.Value))
            .Concat(projection.Goals.Select(x => x.RecordId.Value))
            .Concat(projection.Relationships.Select(x => x.RecordId.Value));

    private static void AssertCategoryUnion(
        IEnumerable<ImmutableArray<PermittedRecord>> inputs,
        ImmutableArray<ContextRecord> actual)
    {
        var expected = inputs.SelectMany(x => x)
            .GroupBy(x => x.RecordId.Value, StringComparer.Ordinal)
            .Select(group => group.First().RecordId.Value)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        CollectionAssert.AreEqual(expected, actual.Select(x => x.RecordId.Value).ToArray());
    }

    private static void AssertRelationshipUnion(
        IEnumerable<ImmutableArray<PermittedRelationship>> inputs,
        ImmutableArray<ContextRelationship> actual)
    {
        var expected = inputs.SelectMany(x => x)
            .GroupBy(x => x.RecordId.Value, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(x => x.RecordId.Value, StringComparer.Ordinal)
            .ToArray();
        CollectionAssert.AreEqual(expected.Select(x => x.RecordId.Value).ToArray(), actual.Select(x => x.RecordId.Value).ToArray());
        CollectionAssert.AreEqual(expected.Select(x => x.TargetCharacterId).ToArray(), actual.Select(x => x.TargetCharacterId).ToArray());
    }

    private static string CandidateJson(CharacterId subject)
    {
        var others = new[] { MissingRaftContract.MarloweId, MissingRaftContract.VossId, MissingRaftContract.WrenId }
            .Where(id => id != subject)
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToArray();
        return JsonSerializer.Serialize(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text = "Still here." },
            control = new
            {
                addressedCharacterIds = others.Select(x => x.Value).ToArray(),
                nominatedCharacterId = others[0].Value
            }
        });
    }
}