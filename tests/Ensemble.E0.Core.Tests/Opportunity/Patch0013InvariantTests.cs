using System.Collections.Immutable;
using System.Reflection;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Opportunity;

[TestClass]
public sealed class Patch0013InvariantTests
{
    [TestMethod]
    public void Establish_RejectsMalformedProductionRosterAndForeignHistoryScene()
    {
        var scenario = Patch0013TestSupport.BuildScenario("INVARIANT-ROSTER-SCENE");
        var malformedState = CloneStateWithRoster(
            scenario.PostCommitState,
            ImmutableArray.Create(
                MissingRaftContract.MarloweId,
                MissingRaftContract.VossId));

        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                malformedState,
                scenario.SourceCommit,
                scenario.Context,
                scenario.SourceHistory));

        var foreignSceneHistory = ConstructNonPublic<E0OpportunityHistory>(
            SceneId.From("SCENE-FOREIGN"),
            scenario.SourceHistory.LastOpportunityStateHash,
            scenario.SourceHistory.CharacterIds);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.Context,
                foreignSceneHistory));
    }

    [TestMethod]
    public void CandidatePerformanceProse_DoesNotChangeExplicitNominationSelection()
    {
        var first = Patch0013TestSupport.Establish(
            Patch0013TestSupport.BuildScenario(
                "PROSE-FIRST",
                nominatedCharacterId: MissingRaftContract.WrenCharacterId,
                candidateText: "First accepted performance text."));
        var second = Patch0013TestSupport.Establish(
            Patch0013TestSupport.BuildScenario(
                "PROSE-SECOND",
                nominatedCharacterId: MissingRaftContract.WrenCharacterId,
                candidateText: "Completely different accepted performance text."));

        Assert.AreEqual(MissingRaftContract.WrenId, first.Event.SelectedCharacterId);
        Assert.AreEqual(MissingRaftContract.WrenId, second.Event.SelectedCharacterId);
        Assert.AreEqual(first.DirectorEvaluation.Trace.Rule, second.DirectorEvaluation.Trace.Rule);
        Assert.AreNotEqual(first.Event.ParentStateHash, second.Event.ParentStateHash);
    }

    [TestMethod]
    public void FailedEstablish_LeavesSuppliedStateHistoryAndCommitUnchanged()
    {
        var scenario = Patch0013TestSupport.BuildScenario("FAILURE-IMMUTABILITY");
        var beforeStateHash = scenario.PostCommitState.StateHash;
        var beforeOpportunity = scenario.PostCommitState.CurrentOpportunityCharacterId;
        var beforeRecords = scenario.PostCommitState.Records.Select(RecordSignature).ToArray();
        var beforeHistoryHash = scenario.SourceHistory.LastOpportunityStateHash;
        var beforeHistory = scenario.SourceHistory.CharacterIds.ToArray();
        var beforeCommitHash = scenario.SourceCommit.ResultStateHash;

        var wrongContext = Ensemble.E0.Core.Tests.Patch0012.Patch0012TestSupport.Compose(
            scenario.Fixture,
            MissingRaftContract.WrenId);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                scenario.PostCommitState,
                scenario.SourceCommit,
                wrongContext,
                scenario.SourceHistory));

        Assert.AreEqual(beforeStateHash, scenario.PostCommitState.StateHash);
        Assert.AreEqual(beforeOpportunity, scenario.PostCommitState.CurrentOpportunityCharacterId);
        CollectionAssert.AreEqual(beforeRecords, scenario.PostCommitState.Records.Select(RecordSignature).ToArray());
        Assert.AreEqual(beforeHistoryHash, scenario.SourceHistory.LastOpportunityStateHash);
        CollectionAssert.AreEqual(beforeHistory, scenario.SourceHistory.CharacterIds.ToArray());
        Assert.AreEqual(beforeCommitHash, scenario.SourceCommit.ResultStateHash);
    }

    [TestMethod]
    public void Replay_RejectsUninitializedSelectedCharacter()
    {
        var scenario = Patch0013TestSupport.BuildScenario("REPLAY-DEFAULT-ID");
        var live = Patch0013TestSupport.Establish(scenario);
        var malformedEvent = ConstructNonPublic<E0OpportunityTransition>(
            live.Event.ParentStateHash,
            live.Event.ResultStateHash,
            live.Event.StrategyContract,
            default(CharacterId));

        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Replay(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.SourceHistory,
                malformedEvent));
    }

    private static ProductionState CloneStateWithRoster(
        ProductionState state,
        ImmutableArray<CharacterId> roster)
    {
        var assembly = typeof(ProductionState).Assembly;
        var projectionType = assembly.GetType(
            "Ensemble.E0.Core.Production.ProductionStateProjection",
            throwOnError: true)!;
        var projectionConstructor = projectionType.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Single(constructor => constructor.GetParameters().Length == 9);
        var projection = projectionConstructor.Invoke(new object?[]
        {
            state.OriginFixtureId,
            state.OriginFixtureFamilyId,
            state.OriginFixtureVersion,
            state.OriginFixtureHash,
            state.SceneId,
            state.Characters,
            roster,
            state.CurrentOpportunityCharacterId,
            state.Records
        });

        var effectiveCommits = typeof(ProductionState).GetField(
            "_effectiveCommitIds",
            BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(state)!;
        var committedTakes = typeof(ProductionState).GetField(
            "_committedTakeIds",
            BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(state)!;
        var stateConstructor = typeof(ProductionState).GetConstructors(
                BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        return (ProductionState)stateConstructor.Invoke(new[]
        {
            projection,
            (object)state.StateHash,
            effectiveCommits,
            committedTakes
        });
    }

    private static string RecordSignature(ProductionRecord record)
    {
        var scope = record switch
        {
            CharacterProductionRecord character => character.SubjectCharacterId.Value,
            RelationshipProductionRecord relationship =>
                $"{relationship.SubjectCharacterId.Value}>{relationship.TargetCharacterId.Value}",
            _ => string.Empty
        };
        return string.Join(
            "|",
            record.RecordId.Value,
            record.Domain,
            record.Lifecycle,
            record.Protection,
            scope,
            record.Text,
            string.Join(",", record.Provenance.Select(id => id.Value)));
    }

    private static T ConstructNonPublic<T>(params object[] arguments)
    {
        var constructor = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == arguments.Length);
        return (T)constructor.Invoke(arguments);
    }
}
