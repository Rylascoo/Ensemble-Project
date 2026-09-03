using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Opportunity;

[TestClass]
public sealed class DeterministicOpportunityAuthorityTests
{
    private const string ExpectedGenesisStateHash =
        "30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104";
    private const string ExpectedPatch0012PostCommitStateHash =
        "057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30";
    private const string ExpectedFallbackPayload =
        "{\"schemaVersion\":\"ensemble.e0.opportunity-transition.v1\"," +
        "\"strategyContract\":\"ensemble.e0.director.least-intervention.v1\"," +
        "\"selectedCharacterId\":\"MARLOWE\"}";

    [TestMethod]
    public void GenesisHistory_AnchorsExactOpeningOpportunityAndCannotResetFromEvolvedState()
    {
        var scenario = Patch0013TestSupport.BuildScenario("HISTORY");
        var history = scenario.SourceHistory;

        Assert.AreEqual(ExpectedGenesisStateHash, scenario.GenesisState.StateHash.Value);
        Assert.AreEqual(scenario.GenesisState.SceneId, history.SceneId);
        Assert.AreEqual(scenario.GenesisState.StateHash, history.LastOpportunityStateHash);
        CollectionAssert.AreEqual(
            new[] { MissingRaftContract.VossId },
            history.CharacterIds.ToArray());

        Assert.Throws<E0OpportunityTransitionException>(() =>
            E0OpportunityHistory.Initialize(scenario.PostCommitState));

        var established = Patch0013TestSupport.Establish(scenario);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            E0OpportunityHistory.Initialize(established.State));
    }

    [TestMethod]
    public void Establish_RecencyFallbackAtomicallyChangesOnlyOpportunityAndAdvancesHistory()
    {
        var scenario = Patch0013TestSupport.BuildScenario("FALLBACK");
        var result = Patch0013TestSupport.Establish(scenario);

        Assert.AreEqual(
            LeastInterventionDirectorRule.RecencyFallback,
            result.DirectorEvaluation.Trace.Rule);
        Assert.AreEqual(MissingRaftContract.MarloweId, result.Event.SelectedCharacterId);
        Assert.AreEqual(MissingRaftContract.MarloweId, result.State.CurrentOpportunityCharacterId);
        Assert.AreEqual(scenario.PostCommitState.StateHash, result.Event.ParentStateHash);
        Assert.AreEqual(result.State.StateHash, result.Event.ResultStateHash);
        Assert.AreNotEqual(scenario.PostCommitState.StateHash, result.State.StateHash);
        Assert.AreEqual(E0DirectorContracts.LeastInterventionStrategyContract, result.Event.StrategyContract);
        CollectionAssert.AreEqual(
            new[] { MissingRaftContract.VossId, MissingRaftContract.MarloweId },
            result.History.CharacterIds.ToArray());
        Assert.AreEqual(result.State.StateHash, result.History.LastOpportunityStateHash);

        AssertProjectionExceptOpportunityEqual(scenario.PostCommitState, result.State);
        Assert.IsFalse(scenario.PostCommitState.CurrentOpportunityCharacterId.HasValue);
        Assert.IsFalse(scenario.SourceHistory.CharacterIds.SequenceEqual(result.History.CharacterIds));
        Assert.IsTrue(InvokeInternalBool(
            result.State,
            "ContainsEffectiveCommitId",
            scenario.SourceCommit.CommitId));
        Assert.IsTrue(InvokeInternalBool(
            result.State,
            "ContainsCommittedTakeId",
            scenario.Take.TakeId));
    }

    [TestMethod]
    public void Establish_NominationAndDirectAddressPreserveExactCandidateControlSemantics()
    {
        var nominatedScenario = Patch0013TestSupport.BuildScenario(
            "NOMINATION",
            nominatedCharacterId: MissingRaftContract.WrenCharacterId);
        var nominated = Patch0013TestSupport.Establish(nominatedScenario);
        Assert.AreEqual(LeastInterventionDirectorRule.Nomination, nominated.DirectorEvaluation.Trace.Rule);
        Assert.AreEqual(MissingRaftContract.WrenId, nominated.Event.SelectedCharacterId);

        var addressedScenario = Patch0013TestSupport.BuildScenario(
            "ADDRESS",
            addressedCharacterIds: new[] { MissingRaftContract.WrenCharacterId });
        var addressed = Patch0013TestSupport.Establish(addressedScenario);
        Assert.AreEqual(LeastInterventionDirectorRule.DirectAddress, addressed.DirectorEvaluation.Trace.Rule);
        Assert.AreEqual(MissingRaftContract.WrenId, addressed.Event.SelectedCharacterId);

        var plainCandidateHash = IntegrityCandidateInput.Bind(
            nominatedScenario.Context,
            Patch0013TestSupport.BuildScenario("PLAIN-HASH").Candidate).CandidateContentHash;
        var nominatedCandidateHash = IntegrityCandidateInput.Bind(
            nominatedScenario.Context,
            nominatedScenario.Candidate).CandidateContentHash;
        Assert.AreNotEqual(plainCandidateHash, nominatedCandidateHash);
    }

    [TestMethod]
    public void Establish_FailsClosedOnWrongCommitHistoryContextCachesOrAlreadyEstablishedState()
    {
        var scenario = Patch0013TestSupport.BuildScenario("FAIL-CANONICAL");
        var foreign = Patch0013TestSupport.BuildScenario(
            "FAIL-FOREIGN",
            candidateText: "Different accepted performance.");

        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                scenario.PostCommitState,
                foreign.SourceCommit,
                scenario.Context,
                scenario.SourceHistory));

        var wrongAnchor = ConstructNonPublic<E0OpportunityHistory>(
            scenario.SourceHistory.SceneId,
            CreateStateHash(new string('a', 64)),
            scenario.SourceHistory.CharacterIds);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.Context,
                wrongAnchor));

        var wrongLastCharacter = ConstructNonPublic<E0OpportunityHistory>(
            scenario.SourceHistory.SceneId,
            scenario.SourceHistory.LastOpportunityStateHash,
            ImmutableArray.Create(MissingRaftContract.WrenId));
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.Context,
                wrongLastCharacter));

        var wrenContext = Patch0012TestSupport.Compose(
            scenario.Fixture,
            MissingRaftContract.WrenId);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                scenario.PostCommitState,
                scenario.SourceCommit,
                wrenContext,
                scenario.SourceHistory));

        var noCaches = CloneStateWithoutEffectiveCaches(scenario.PostCommitState);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                noCaches,
                scenario.SourceCommit,
                scenario.Context,
                scenario.SourceHistory));

        var established = Patch0013TestSupport.Establish(scenario);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Establish(
                established.State,
                scenario.SourceCommit,
                scenario.Context,
                scenario.SourceHistory));
    }

    [TestMethod]
    public void ProductionHelper_IsNarrowRejectsInvalidTransitionAndPreservesCommitTakeCaches()
    {
        var scenario = Patch0013TestSupport.BuildScenario("PRODUCTION-HELPER");
        var helper = typeof(ProductionState).GetMethod(
            "WithEstablishedOpportunity",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("WithEstablishedOpportunity helper is missing.");

        CollectionAssert.AreEqual(
            new[] { typeof(CharacterId), typeof(StateHash) },
            helper.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(ProductionState), helper.ReturnType);

        AssertInvocationInner<ProductionStateException>(() =>
            helper.Invoke(
                scenario.GenesisState,
                new object[] { MissingRaftContract.WrenId, scenario.GenesisState.StateHash }));
        AssertInvocationInner<ProductionStateException>(() =>
            helper.Invoke(
                scenario.PostCommitState,
                new object[] { default(CharacterId), scenario.PostCommitState.StateHash }));
        AssertInvocationInner<ProductionStateException>(() =>
            helper.Invoke(
                scenario.PostCommitState,
                new object[] { CharacterId.From("OUTSIDE-ROSTER"), scenario.PostCommitState.StateHash }));

        var result = Patch0013TestSupport.Establish(scenario);
        Assert.IsTrue(InvokeInternalBool(
            result.State,
            "ContainsEffectiveCommitId",
            scenario.SourceCommit.CommitId));
        Assert.IsTrue(InvokeInternalBool(
            result.State,
            "ContainsCommittedTakeId",
            scenario.Take.TakeId));
    }

    [TestMethod]
    public void CanonicalPayloadAndStateHashEnvelope_AreExactDeterministicAndCultureIndependent()
    {
        var scenario = Patch0013TestSupport.BuildScenario("CANONICAL");
        var result = Patch0013TestSupport.Establish(scenario);

        var payload = InvokeOpportunityPayload(
            result.Event.StrategyContract,
            result.Event.SelectedCharacterId);
        Assert.AreEqual(ExpectedFallbackPayload, Encoding.UTF8.GetString(payload));

        var projectionBytes = InvokeProductionProjectionBytes(result.State);
        var expectedHash = HashOpportunityEnvelope(
            scenario.PostCommitState.StateHash,
            payload,
            projectionBytes);
        Assert.AreEqual(expectedHash, result.State.StateHash.Value);

        var previousCulture = CultureInfo.CurrentCulture;
        var previousUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ar-SA");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ar-SA");
            var second = Patch0013TestSupport.Establish(scenario);
            Assert.AreEqual(result.Event.ResultStateHash, second.Event.ResultStateHash);
            Assert.AreEqual(result.Event.SelectedCharacterId, second.Event.SelectedCharacterId);
            CollectionAssert.AreEqual(result.History.CharacterIds.ToArray(), second.History.CharacterIds.ToArray());
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
            CultureInfo.CurrentUICulture = previousUiCulture;
        }
    }

    [TestMethod]
    public void Replay_ReconstructsSameDirectorStateAndHistoryAndRejectsTampering()
    {
        var scenario = Patch0013TestSupport.BuildScenario(
            "REPLAY",
            nominatedCharacterId: MissingRaftContract.WrenCharacterId);
        var live = Patch0013TestSupport.Establish(scenario);
        var replay = DeterministicOpportunityAuthority.Replay(
            scenario.PostCommitState,
            scenario.SourceCommit,
            scenario.SourceHistory,
            live.Event);

        Assert.AreEqual(live.State.StateHash, replay.State.StateHash);
        Assert.AreEqual(live.State.CurrentOpportunityCharacterId, replay.State.CurrentOpportunityCharacterId);
        CollectionAssert.AreEqual(live.History.CharacterIds.ToArray(), replay.History.CharacterIds.ToArray());
        Assert.AreEqual(live.History.LastOpportunityStateHash, replay.History.LastOpportunityStateHash);
        Assert.AreEqual(live.DirectorEvaluation.Trace.Rule, replay.DirectorEvaluation.Trace.Rule);
        Assert.AreEqual(
            live.DirectorEvaluation.Proposal.SelectedCharacterId,
            replay.DirectorEvaluation.Proposal.SelectedCharacterId);
        CollectionAssert.AreEqual(
            live.DirectorEvaluation.Trace.Input.OpportunityHistory.ToArray(),
            replay.DirectorEvaluation.Trace.Input.OpportunityHistory.ToArray());
        CollectionAssert.AreEqual(
            live.DirectorEvaluation.Trace.Input.AddressedCharacterIds.ToArray(),
            replay.DirectorEvaluation.Trace.Input.AddressedCharacterIds.ToArray());
        Assert.AreEqual(
            live.DirectorEvaluation.Trace.Input.NominatedCharacterId,
            replay.DirectorEvaluation.Trace.Input.NominatedCharacterId);

        var wrongStrategy = ConstructNonPublic<E0OpportunityTransition>(
            live.Event.ParentStateHash,
            live.Event.ResultStateHash,
            "ensemble.e0.director.unsupported.v1",
            live.Event.SelectedCharacterId);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Replay(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.SourceHistory,
                wrongStrategy));

        var wrongSelected = ConstructNonPublic<E0OpportunityTransition>(
            live.Event.ParentStateHash,
            live.Event.ResultStateHash,
            live.Event.StrategyContract,
            MissingRaftContract.MarloweId);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Replay(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.SourceHistory,
                wrongSelected));

        var wrongHash = ConstructNonPublic<E0OpportunityTransition>(
            live.Event.ParentStateHash,
            CreateStateHash(new string('b', 64)),
            live.Event.StrategyContract,
            live.Event.SelectedCharacterId);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Replay(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.SourceHistory,
                wrongHash));

        var wrongParent = ConstructNonPublic<E0OpportunityTransition>(
            CreateStateHash(new string('c', 64)),
            live.Event.ResultStateHash,
            live.Event.StrategyContract,
            live.Event.SelectedCharacterId);
        Assert.Throws<E0OpportunityTransitionException>(() =>
            DeterministicOpportunityAuthority.Replay(
                scenario.PostCommitState,
                scenario.SourceCommit,
                scenario.SourceHistory,
                wrongParent));
    }

    [TestMethod]
    public void Patch0012GenesisAndPostCommitHashOracles_RemainUnchanged()
    {
        var state = Patch0012TestSupport.Genesis();
        Assert.AreEqual(ExpectedGenesisStateHash, state.StateHash.Value);

        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure });
        var take = Patch0012TestSupport.AcceptedTake(
            pipeline,
            "TAKE-PATCH-0012-ORACLE");
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var binding = E0TakeStateBinding.Bind(checkpoint, pipeline.Context, take);
        var materials = E0RecordMaterializationSet.Bind(
            ImmutableArray.Create(
                E0RecordMaterialization.Create(
                    0,
                    RecordId.From("PRESSURE-PATCH-0012-ORACLE"))));
        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-PATCH-0012-ORACLE"),
            state,
            binding,
            materials);

        Assert.AreEqual(ExpectedPatch0012PostCommitStateHash, result.ResultState.StateHash.Value);
    }

    private static void AssertProjectionExceptOpportunityEqual(
        ProductionState expected,
        ProductionState actual)
    {
        Assert.AreEqual(expected.ContractVersion, actual.ContractVersion);
        Assert.AreEqual(expected.OriginFixtureId, actual.OriginFixtureId);
        Assert.AreEqual(expected.OriginFixtureFamilyId, actual.OriginFixtureFamilyId);
        Assert.AreEqual(expected.OriginFixtureVersion, actual.OriginFixtureVersion);
        Assert.AreEqual(expected.OriginFixtureHash, actual.OriginFixtureHash);
        Assert.AreEqual(expected.SceneId, actual.SceneId);
        CollectionAssert.AreEqual(
            expected.Characters.Select(character =>
                $"{character.CharacterId.Value}|{character.DisplayName}").ToArray(),
            actual.Characters.Select(character =>
                $"{character.CharacterId.Value}|{character.DisplayName}").ToArray());
        CollectionAssert.AreEqual(expected.RosterCharacterIds.ToArray(), actual.RosterCharacterIds.ToArray());
        CollectionAssert.AreEqual(
            expected.Records.Select(RecordSignature).ToArray(),
            actual.Records.Select(RecordSignature).ToArray());
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

    private static byte[] InvokeOpportunityPayload(string strategyContract, CharacterId selectedCharacterId)
    {
        var type = typeof(E0OpportunityTransition).Assembly.GetType(
            "Ensemble.E0.Core.Opportunity.OpportunityCanonicalizer",
            throwOnError: true)!;
        var method = type.GetMethod("SerializePayload", BindingFlags.Static | BindingFlags.NonPublic)!;
        return (byte[])method.Invoke(null, new object[] { strategyContract, selectedCharacterId })!;
    }

    private static byte[] InvokeProductionProjectionBytes(ProductionState state)
    {
        var type = typeof(ProductionState).Assembly.GetType(
            "Ensemble.E0.Core.Production.ProductionStateCanonicalizer",
            throwOnError: true)!;
        var method = type.GetMethod("SerializeProjection", BindingFlags.Static | BindingFlags.NonPublic)!;
        return (byte[])method.Invoke(null, new object[] { state })!;
    }

    private static string HashOpportunityEnvelope(
        StateHash parentStateHash,
        byte[] payload,
        byte[] resultProjection)
    {
        var prefix = Encoding.UTF8.GetBytes(
            "{\"hashContract\":\"ensemble.e0.production-state-hash.sha256.v1\"," +
            "\"kind\":\"opportunityTransition\"," +
            $"\"parentStateHash\":\"{parentStateHash.Value}\"," +
            "\"opportunityPayload\":");
        var middle = Encoding.UTF8.GetBytes(",\"resultProjection\":");
        var bytes = new byte[
            prefix.Length + payload.Length + middle.Length + resultProjection.Length + 1];
        var offset = 0;
        Buffer.BlockCopy(prefix, 0, bytes, offset, prefix.Length);
        offset += prefix.Length;
        Buffer.BlockCopy(payload, 0, bytes, offset, payload.Length);
        offset += payload.Length;
        Buffer.BlockCopy(middle, 0, bytes, offset, middle.Length);
        offset += middle.Length;
        Buffer.BlockCopy(resultProjection, 0, bytes, offset, resultProjection.Length);
        bytes[^1] = (byte)'}';
        return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    }

    private static bool InvokeInternalBool<T>(ProductionState state, string methodName, T value)
    {
        var method = typeof(ProductionState).GetMethod(
            methodName,
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"{methodName} was not found.");
        return (bool)method.Invoke(state, new object[] { value! })!;
    }

    private static ProductionState CloneStateWithoutEffectiveCaches(ProductionState state)
    {
        var projection = typeof(ProductionState).GetField(
            "_projection",
            BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(state)!;
        var constructor = typeof(ProductionState).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        return (ProductionState)constructor.Invoke(new object[]
        {
            projection,
            state.StateHash,
            ImmutableHashSet.Create<string>(StringComparer.Ordinal),
            ImmutableHashSet.Create<string>(StringComparer.Ordinal)
        });
    }

    private static StateHash CreateStateHash(string value)
    {
        var method = typeof(StateHash).GetMethod(
            "Create",
            BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("StateHash.Create was not found.");
        return (StateHash)method.Invoke(null, new object[] { value })!;
    }

    private static T ConstructNonPublic<T>(params object[] arguments)
    {
        var constructor = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == arguments.Length);
        return (T)constructor.Invoke(arguments);
    }

    private static void AssertInvocationInner<TException>(Action action)
        where TException : Exception
    {
        var wrapper = Assert.Throws<TargetInvocationException>(action);
        Assert.IsInstanceOfType<TException>(wrapper.InnerException);
    }
}
