using System.Collections.Immutable;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class ProductionAccessContinuityTests
{
    [TestMethod]
    public void GenesisProductionAccess_MatchesFixtureAccessForAllCharacters()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);

        foreach (var character in fixture.Characters)
        {
            var historical = CharacterBoundedAccessControl.Evaluate(fixture, character.Id);
            var production = CharacterBoundedAccessControl.Evaluate(state, character.Id);

            Assert.AreEqual(state.StateHash, production.Projection.SourceStateHash);
            Assert.AreEqual(
                ProjectionSignature(historical.Projection),
                ProjectionSignature(production.Projection));
            CollectionAssert.AreEqual(
                historical.Decisions.Select(DecisionSignature).ToArray(),
                production.Decisions.Select(DecisionSignature).ToArray());
        }
    }

    [TestMethod]
    public void GenericSmokeProductionAccess_MatchesHistoricalFixtureProjection()
    {
        var fixture = Patch0014TestSupport.LoadGenericSmoke();
        var state = ProductionState.Initialize(fixture, ImmutableArray<RecordId>.Empty);
        var subject = CharacterId.From("CHAR-A");

        var historical = CharacterBoundedAccessControl.Evaluate(fixture, subject);
        var production = CharacterBoundedAccessControl.Evaluate(state, subject);

        Assert.AreEqual(state.StateHash, production.Projection.SourceStateHash);
        Assert.AreEqual(
            ProjectionSignature(historical.Projection),
            ProjectionSignature(production.Projection));
    }

    [TestMethod]
    public void CommittedPressureAdd_IsVisibleThroughProductionAccess()
    {
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd("A new public pressure.") },
            new[] { StateMutationDomain.Pressure });
        var state = Commit(
            pipeline,
            "PRESSURE",
            E0RecordMaterialization.Create(0, RecordId.From("PRESSURE-PATCH-0014")));

        var evaluation = CharacterBoundedAccessControl.Evaluate(
            state,
            MissingRaftContract.VossId);

        Assert.IsTrue(evaluation.Projection.Pressures.Any(record =>
            record.RecordId == RecordId.From("PRESSURE-PATCH-0014") &&
            string.Equals(record.Text, "A new public pressure.", StringComparison.Ordinal)));
        var decision = evaluation.Decisions.Single(value =>
            value.RecordId == RecordId.From("PRESSURE-PATCH-0014"));
        Assert.AreEqual(AccessDisposition.Permit, decision.Disposition);
        Assert.AreEqual(AccessReason.PublicPressure, decision.Reason);
    }

    [TestMethod]
    public void InactiveCommittedBelief_IsDeniedAndRemovedFromProjection()
    {
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.BeliefDeactivate() },
            new[] { StateMutationDomain.CharacterBelief });
        var state = Commit(pipeline, "INACTIVE-BELIEF");

        var evaluation = CharacterBoundedAccessControl.Evaluate(
            state,
            MissingRaftContract.VossId);
        var beliefId = RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId);

        Assert.IsFalse(evaluation.Projection.Beliefs.Any(record => record.RecordId == beliefId));
        var decision = evaluation.Decisions.Single(value => value.RecordId == beliefId);
        Assert.AreEqual(AccessDisposition.Deny, decision.Disposition);
        Assert.AreEqual(AccessReason.InactiveRecordExcluded, decision.Reason);
    }

    [TestMethod]
    public void ActiveCharacterClaim_IsDeferredForOwnerAndOtherCharacters()
    {
        var claim = Patch0012TestSupport.Mutation(
            "characterClaim",
            "add",
            subjectCharacterId: MissingRaftContract.VossCharacterId,
            text: "Voss claims the loss was preventable.");
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { claim },
            new[] { StateMutationDomain.CharacterClaim });
        var claimId = RecordId.From("CLAIM-PATCH-0014");
        var state = Commit(
            pipeline,
            "CLAIM",
            E0RecordMaterialization.Create(0, claimId));

        foreach (var subject in new[]
                 {
                     MissingRaftContract.VossId,
                     MissingRaftContract.MarloweId,
                     MissingRaftContract.WrenId
                 })
        {
            var evaluation = CharacterBoundedAccessControl.Evaluate(state, subject);
            var decision = evaluation.Decisions.Single(value => value.RecordId == claimId);

            Assert.AreEqual(AccessDisposition.Deny, decision.Disposition);
            Assert.AreEqual(AccessReason.CharacterClaimDisclosureDeferred, decision.Reason);
            Assert.IsFalse(ProjectionRecordIds(evaluation.Projection).Contains(claimId.Value));
        }
    }

    [TestMethod]
    public void ProductionAccess_DecisionsCoverEveryRetainedRecordExactlyOnceInOrdinalOrder()
    {
        var evolved = Patch0014TestSupport.Evolved("ACCESS-DECISIONS");
        var evaluation = CharacterBoundedAccessControl.Evaluate(
            evolved.Opportunity.State,
            evolved.Checkpoint.CurrentOpportunityCharacterId);
        var expected = evolved.Opportunity.State.Records
            .Select(record => record.RecordId.Value)
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();
        var actual = evaluation.Decisions
            .Select(decision => decision.RecordId.Value)
            .ToArray();

        CollectionAssert.AreEqual(expected, actual);
        Assert.AreEqual(actual.Length, actual.Distinct(StringComparer.Ordinal).Count());
    }

    private static ProductionState Commit(
        Patch0012TestSupport.Pipeline pipeline,
        string suffix,
        params E0RecordMaterialization[] materializations)
    {
        var state = Patch0012TestSupport.Genesis(pipeline.Fixture);
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var take = Patch0012TestSupport.AcceptedTake(
            pipeline,
            $"TAKE-PATCH-0014-{suffix}");
        var binding = E0TakeStateBinding.Bind(checkpoint, pipeline.Context, take);
        var result = DeterministicCausalCommit.Commit(
            CommitId.From($"COMMIT-PATCH-0014-{suffix}"),
            state,
            binding,
            E0RecordMaterializationSet.Bind(materializations.ToImmutableArray()));
        return result.ResultState;
    }

    private static string ProjectionSignature(CharacterAccessProjection projection) =>
        string.Join(
            "|",
            ProjectionRecordIds(projection).OrderBy(value => value, StringComparer.Ordinal)) +
        "#" +
        string.Join(
            ",",
            projection.Roster
                .OrderBy(participant => participant.CharacterId.Value, StringComparer.Ordinal)
                .Select(participant =>
                    $"{participant.CharacterId.Value}:{participant.DisplayName}"));

    private static HashSet<string> ProjectionRecordIds(CharacterAccessProjection projection) =>
        projection.SceneState.Select(record => record.RecordId.Value)
            .Concat(projection.Pressures.Select(record => record.RecordId.Value))
            .Concat(projection.Constitution.Select(record => record.RecordId.Value))
            .Concat(projection.Disposition.Select(record => record.RecordId.Value))
            .Concat(projection.Circumstance.Select(record => record.RecordId.Value))
            .Concat(projection.Observations.Select(record => record.RecordId.Value))
            .Concat(projection.Knowledge.Select(record => record.RecordId.Value))
            .Concat(projection.Beliefs.Select(record => record.RecordId.Value))
            .Concat(projection.Suspicions.Select(record => record.RecordId.Value))
            .Concat(projection.Memories.Select(record => record.RecordId.Value))
            .Concat(projection.Goals.Select(record => record.RecordId.Value))
            .Concat(projection.Relationships.Select(record => record.RecordId.Value))
            .ToHashSet(StringComparer.Ordinal);

    private static string DecisionSignature(AccessDecision decision) =>
        $"{decision.RecordId.Value}:{decision.Disposition}:{decision.Reason}";
}
