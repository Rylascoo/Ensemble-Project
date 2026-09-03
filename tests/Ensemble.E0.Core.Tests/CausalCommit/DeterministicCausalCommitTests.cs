using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.CausalCommit;

[TestClass]
public sealed class DeterministicCausalCommitTests
{
    private const string ExpectedGenesisStateHash =
        "30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104";
    private const string ExpectedOracleCandidateHash =
        "cced4de8efbaf3bf707c92192cdbe0f084a46156205c4f88c326e5f7535dc153";
    private const string ExpectedOracleProposalHash =
        "16f20511ddd9655640d532be37b6431e9e9fa8abc07cda3033bf7dba5ed8e248";
    private const string ExpectedPostCommitStateHash =
        "057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30";

    private const string ExpectedOracleCommitPayload =
        "{\"schemaVersion\":\"ensemble.e0.causal-commit.v1\",\"commitId\":\"COMMIT-PATCH-0012-ORACLE\",\"take\":{" +
        "\"contractVersion\":\"ensemble.e0.take.v1\",\"takeId\":\"TAKE-PATCH-0012-ORACLE\",\"disposition\":\"accepted\"," +
        "\"performanceSubjectCharacterId\":\"VOSS\",\"performanceContextPacketId\":\"CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b\"," +
        "\"candidateContentIdentityContract\":\"ensemble.e0.integrity.candidate-content.v1\",\"candidateContentHash\":\"cced4de8efbaf3bf707c92192cdbe0f084a46156205c4f88c326e5f7535dc153\"," +
        "\"proposalContentIdentityContract\":\"ensemble.e0.state-authority.interpreter-proposal-content.v1\",\"proposalContentHash\":\"16f20511ddd9655640d532be37b6431e9e9fa8abc07cda3033bf7dba5ed8e248\"," +
        "\"sourceSceneId\":\"SCENE-MISSING-RAFT\",\"authorityContractVersion\":\"ensemble.e0.state-authority.review.v1\",\"authorityStatus\":\"complete\"," +
        "\"authorityPolicy\":{\"contractVersion\":\"ensemble.e0.state-authority.policy.v1\",\"autoApproveDomains\":[\"pressure\"]}," +
        "\"authorityReviewSet\":{\"contractVersion\":\"ensemble.e0.state-authority.review-set.v1\",\"proposalContentIdentityContract\":\"ensemble.e0.state-authority.interpreter-proposal-content.v1\",\"proposalContentHash\":\"16f20511ddd9655640d532be37b6431e9e9fa8abc07cda3033bf7dba5ed8e248\",\"choices\":[]}," +
        "\"authorityDecisions\":[{\"mutationIndex\":0,\"disposition\":\"approved\",\"reasons\":[\"policyAutoApproved\"]}]}," +
        "\"recordMaterializations\":[{\"mutationIndex\":0,\"recordId\":\"PRESSURE-PATCH-0012-ORACLE\"}]}";

    [TestMethod]
    public void CheckpointAndBinding_AreAcceptedOnlyAndRetainExactTakeReference()
    {
        var state = Patch0012TestSupport.Genesis();
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure });
        var accepted = Patch0012TestSupport.AcceptedTake(pipeline, "TAKE-BIND-ACCEPTED");
        var rejected = Patch0012TestSupport.Take(
            pipeline,
            E0TakeDisposition.Rejected,
            "TAKE-BIND-REJECTED");
        var alternate = Patch0012TestSupport.Take(
            pipeline,
            E0TakeDisposition.Alternate,
            "TAKE-BIND-ALTERNATE");

        var binding = E0TakeStateBinding.Bind(checkpoint, pipeline.Context, accepted);

        Assert.AreEqual(state.StateHash, binding.SourceStateHash);
        Assert.AreSame(accepted, binding.Take);
        Assert.AreEqual(0, typeof(E0TakeStateBinding)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
        Assert.IsFalse(typeof(E0TakeStateBinding)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Any(property => property.SetMethod?.IsPublic == true));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(checkpoint, pipeline.Context, rejected));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(checkpoint, pipeline.Context, alternate));
    }

    [TestMethod]
    public void CheckpointAndBinding_FailClosedOnNullOpportunityContextOrSnapshotMismatch()
    {
        Assert.Throws<E0CausalCommitException>(() => ProductionStateCheckpoint.Capture(null!));

        var state = Patch0012TestSupport.Genesis();
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var pipeline = Patch0012TestSupport.BuildPipeline(
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>());
        var take = Patch0012TestSupport.AcceptedTake(pipeline, "TAKE-BIND-FAILURES");

        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(null!, pipeline.Context, take));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(checkpoint, null!, take));
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(checkpoint, pipeline.Context, null!));

        var wrenContext = Patch0012TestSupport.Compose(pipeline.Fixture, MissingRaftContract.WrenId);
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(checkpoint, wrenContext, take));

        var lockedState = ProductionState.Initialize(
            pipeline.Fixture,
            ImmutableArray.Create(RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId)));
        var lockedCheckpoint = ProductionStateCheckpoint.Capture(lockedState);
        Assert.Throws<E0CausalCommitException>(() =>
            E0TakeStateBinding.Bind(lockedCheckpoint, pipeline.Context, take));
    }

    [TestMethod]
    public void MaterializationSet_IsClosedCanonicalAndAcceptsEmpty()
    {
        var empty = E0RecordMaterializationSet.Bind(ImmutableArray<E0RecordMaterialization>.Empty);
        Assert.AreEqual(0, empty.Items.Length);

        var second = E0RecordMaterialization.Create(2, RecordId.From("REC-B"));
        var first = E0RecordMaterialization.Create(0, RecordId.From("REC-A"));
        var set = E0RecordMaterializationSet.Bind(ImmutableArray.Create(second, first));
        CollectionAssert.AreEqual(new[] { 0, 2 }, set.Items.Select(item => item.MutationIndex).ToArray());

        Assert.Throws<E0CausalCommitException>(() =>
            E0RecordMaterialization.Create(-1, RecordId.From("REC-X")));
        Assert.Throws<E0CausalCommitException>(() =>
            E0RecordMaterialization.Create(0, default));
        Assert.Throws<E0CausalCommitException>(() =>
            E0RecordMaterializationSet.Bind(default));
        Assert.Throws<E0CausalCommitException>(() =>
            E0RecordMaterializationSet.Bind(
                ImmutableArray.CreateRange<E0RecordMaterialization>(
                    new E0RecordMaterialization[] { null! })));
        Assert.Throws<E0CausalCommitException>(() =>
            E0RecordMaterializationSet.Bind(
                ImmutableArray.Create(
                    E0RecordMaterialization.Create(0, RecordId.From("REC-A")),
                    E0RecordMaterialization.Create(0, RecordId.From("REC-B")))));
        Assert.Throws<E0CausalCommitException>(() =>
            E0RecordMaterializationSet.Bind(
                ImmutableArray.Create(
                    E0RecordMaterialization.Create(0, RecordId.From("REC-DUP")),
                    E0RecordMaterialization.Create(1, RecordId.From("REC-DUP")))));

        Assert.AreEqual(0, typeof(E0RecordMaterialization)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
        Assert.AreEqual(0, typeof(E0RecordMaterializationSet)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
    }

    [TestMethod]
    public void AcceptedZeroMutationCommit_PersistsPerformanceConsumesOpportunityAndChangesHistoryHash()
    {
        var scenario = Scenario(
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-ZERO");
        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-ZERO"),
            scenario.State,
            scenario.Binding,
            EmptyMaterializations());

        Assert.AreEqual(scenario.State.StateHash, result.Commit.ParentStateHash);
        Assert.AreEqual(result.ResultState.StateHash, result.Commit.ResultStateHash);
        Assert.AreNotEqual(scenario.State.StateHash, result.ResultState.StateHash);
        Assert.AreSame(scenario.Take, result.Commit.Take);
        Assert.AreEqual(0, result.Commit.RecordMaterializations.Items.Length);
        Assert.IsFalse(result.ResultState.CurrentOpportunityCharacterId.HasValue);
        Assert.AreEqual(scenario.State.Records.Length, result.ResultState.Records.Length);
        CollectionAssert.AreEqual(
            scenario.State.Records.Select(Signature).ToArray(),
            result.ResultState.Records.Select(Signature).ToArray());
    }

    [TestMethod]
    public void AcceptedAllRejectedCommit_PersistsPerformanceWithoutApplyingRejectedMutation()
    {
        var scenario = Scenario(
            new[] { Patch0012TestSupport.InvalidBeliefSupersede() },
            new[] { StateMutationDomain.CharacterBelief },
            "TAKE-ALL-REJECTED");
        Assert.AreEqual(
            StateAuthorityDisposition.Rejected,
            scenario.Take.AuthorityEvaluation.Decisions.Single().Disposition);

        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-ALL-REJECTED"),
            scenario.State,
            scenario.Binding,
            EmptyMaterializations());

        Assert.AreNotEqual(scenario.State.StateHash, result.ResultState.StateHash);
        Assert.IsFalse(result.ResultState.CurrentOpportunityCharacterId.HasValue);
        CollectionAssert.AreEqual(
            scenario.State.Records.Select(Signature).ToArray(),
            result.ResultState.Records.Select(Signature).ToArray());
    }

    [TestMethod]
    public void ApprovedAdd_MaterializesExactActiveNoneProtectedRecordAndProvenance()
    {
        var scenario = Scenario(
            new[]
            {
                Patch0012TestSupport.PressureAdd(
                    "Pressure changes.",
                    MissingRaftContract.SceneRaftGoneId,
                    MissingRaftContract.SceneProvisionsLimitedId)
            },
            new[] { StateMutationDomain.Pressure },
            "TAKE-ADD");
        var materializations = Materials((0, "PRESSURE-ADD-NEW"));
        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-ADD"),
            scenario.State,
            scenario.Binding,
            materializations);

        var added = Assert.IsInstanceOfType<GlobalProductionRecord>(
            result.ResultState.Records.Single(record => record.RecordId == RecordId.From("PRESSURE-ADD-NEW")));
        Assert.AreEqual(ProductionRecordDomain.Pressure, added.Domain);
        Assert.AreEqual(ProductionRecordLifecycle.Active, added.Lifecycle);
        Assert.AreEqual(ProductionRecordProtection.None, added.Protection);
        Assert.AreEqual("Pressure changes.", added.Text);
        CollectionAssert.AreEqual(
            new[]
            {
                RecordId.From(MissingRaftContract.SceneProvisionsLimitedId),
                RecordId.From(MissingRaftContract.SceneRaftGoneId)
            },
            added.Provenance.ToArray());
    }

    [TestMethod]
    public void ApprovedSupersede_PreservesOldRecordInactiveAndAddsExactNewRecordWithoutSyntheticLineage()
    {
        var scenario = Scenario(
            new[]
            {
                Patch0012TestSupport.BeliefSupersede(
                    "Voss now considers deliberate action plausible.",
                    MissingRaftContract.GoalVossId)
            },
            new[] { StateMutationDomain.CharacterBelief },
            "TAKE-SUPERSEDE");
        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-SUPERSEDE"),
            scenario.State,
            scenario.Binding,
            Materials((0, "BEL-VOSS-REVISED")));

        var oldRecord = result.ResultState.Records.Single(
            record => record.RecordId == RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId));
        Assert.AreEqual(ProductionRecordLifecycle.Inactive, oldRecord.Lifecycle);

        var newRecord = Assert.IsInstanceOfType<CharacterProductionRecord>(
            result.ResultState.Records.Single(record => record.RecordId == RecordId.From("BEL-VOSS-REVISED")));
        Assert.AreEqual(ProductionRecordDomain.CharacterBelief, newRecord.Domain);
        Assert.AreEqual(MissingRaftContract.VossId, newRecord.SubjectCharacterId);
        Assert.AreEqual(ProductionRecordLifecycle.Active, newRecord.Lifecycle);
        Assert.AreEqual(ProductionRecordProtection.None, newRecord.Protection);
        CollectionAssert.AreEqual(
            new[] { RecordId.From(MissingRaftContract.GoalVossId) },
            newRecord.Provenance.ToArray());
        CollectionAssert.DoesNotContain(
            newRecord.Provenance.ToArray(),
            RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId));
    }

    [TestMethod]
    public void ApprovedDeactivate_OnlyInactivatesExactExistingRecordAndNeedsNoMaterialization()
    {
        var scenario = Scenario(
            new[] { Patch0012TestSupport.BeliefDeactivate(MissingRaftContract.GoalVossId) },
            new[] { StateMutationDomain.CharacterBelief },
            "TAKE-DEACTIVATE");
        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-DEACTIVATE"),
            scenario.State,
            scenario.Binding,
            EmptyMaterializations());

        var oldRecord = result.ResultState.Records.Single(
            record => record.RecordId == RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId));
        Assert.AreEqual(ProductionRecordLifecycle.Inactive, oldRecord.Lifecycle);
        Assert.AreEqual(scenario.State.Records.Length, result.ResultState.Records.Length);
    }

    [TestMethod]
    public void MixedDecisions_ApplyOnlyApprovedMutationsAndMaterializeOnlyApprovedAddsOrSupersedes()
    {
        var scenario = Scenario(
            new[]
            {
                Patch0012TestSupport.PressureAdd("Approved pressure."),
                Patch0012TestSupport.InvalidBeliefSupersede()
            },
            new[] { StateMutationDomain.Pressure, StateMutationDomain.CharacterBelief },
            "TAKE-MIXED");
        CollectionAssert.AreEqual(
            new[] { StateAuthorityDisposition.Approved, StateAuthorityDisposition.Rejected },
            scenario.Take.AuthorityEvaluation.Decisions.Select(decision => decision.Disposition).ToArray());

        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-MIXED"),
            scenario.State,
            scenario.Binding,
            Materials((0, "PRESSURE-MIXED")));

        Assert.IsTrue(result.ResultState.Records.Any(record => record.RecordId == RecordId.From("PRESSURE-MIXED")));
        Assert.AreEqual(
            ProductionRecordLifecycle.Active,
            result.ResultState.Records.Single(record =>
                record.RecordId == RecordId.From(MissingRaftContract.ConVossId)).Lifecycle);
    }

    [TestMethod]
    public void MaterializationRequirements_AreExactAndRejectMissingExtraDuplicateOrHistoricalCollisions()
    {
        var addScenario = Scenario(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure },
            "TAKE-MATERIALIZATION");

        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Commit(
                CommitId.From("COMMIT-MISSING-MAT"),
                addScenario.State,
                addScenario.Binding,
                EmptyMaterializations()));
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Commit(
                CommitId.From("COMMIT-WRONG-INDEX"),
                addScenario.State,
                addScenario.Binding,
                Materials((1, "PRESSURE-WRONG-INDEX"))));
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Commit(
                CommitId.From("COMMIT-COLLIDE"),
                addScenario.State,
                addScenario.Binding,
                Materials((0, MissingRaftContract.PressureIsolationId))));

        var zeroScenario = Scenario(
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-EXTRA-MAT");
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Commit(
                CommitId.From("COMMIT-EXTRA-MAT"),
                zeroScenario.State,
                zeroScenario.Binding,
                Materials((0, "EXTRA-RECORD"))));
    }

    [TestMethod]
    public void StaleBindingFailsBeforeMutationAndFailedCommitLeavesOriginalStateUnchanged()
    {
        var first = Scenario(
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-FIRST");
        var firstResult = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-FIRST"),
            first.State,
            first.Binding,
            EmptyMaterializations());
        var before = firstResult.ResultState.Records.Select(Signature).ToArray();

        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Commit(
                CommitId.From("COMMIT-STALE"),
                firstResult.ResultState,
                first.Binding,
                EmptyMaterializations()));

        CollectionAssert.AreEqual(before, firstResult.ResultState.Records.Select(Signature).ToArray());
        Assert.AreEqual(firstResult.Commit.ResultStateHash, firstResult.ResultState.StateHash);
    }

    [TestMethod]
    public void StateHash_IsHistorySensitiveAndDeterministicAcrossIdenticalInputs()
    {
        var first = OracleScenario();
        var second = OracleScenario();
        var firstResult = CommitOracle(first);
        var secondResult = CommitOracle(second);

        Assert.AreEqual(ExpectedGenesisStateHash, first.State.StateHash.Value);
        Assert.AreEqual(ExpectedPostCommitStateHash, firstResult.ResultState.StateHash.Value);
        Assert.AreEqual(firstResult.ResultState.StateHash, secondResult.ResultState.StateHash);
        Assert.AreEqual(firstResult.Commit.ResultStateHash, secondResult.Commit.ResultStateHash);

        var differentCommit = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-PATCH-0012-DIFFERENT"),
            first.State,
            first.Binding,
            Materials((0, "PRESSURE-PATCH-0012-ORACLE")));
        Assert.AreNotEqual(firstResult.ResultState.StateHash, differentCommit.ResultState.StateHash);

        var differentRecord = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-PATCH-0012-ORACLE"),
            first.State,
            first.Binding,
            Materials((0, "PRESSURE-PATCH-0012-DIFFERENT")));
        Assert.AreNotEqual(firstResult.ResultState.StateHash, differentRecord.ResultState.StateHash);
    }

    [TestMethod]
    public void OracleCommitPayloadAndPostCommitStateHash_MatchFixedCanonicalBytes()
    {
        var scenario = OracleScenario();
        Assert.AreEqual(ExpectedOracleCandidateHash, scenario.Pipeline.Integrity.Trace.Input.CandidateContentHash);
        Assert.AreEqual(ExpectedOracleProposalHash, scenario.Pipeline.Authority.Trace.Input.ProposalContentHash);

        var result = CommitOracle(scenario);
        var bytes = InvokeInternalBytes(
            "Ensemble.E0.Core.CausalCommit.CausalCommitCanonicalizer",
            "SerializeCommitPayload",
            result.Commit.CommitId,
            result.Commit.Take,
            result.Commit.RecordMaterializations);

        CollectionAssert.AreEqual(Encoding.UTF8.GetBytes(ExpectedOracleCommitPayload), bytes);
        Assert.AreEqual(ExpectedPostCommitStateHash, result.ResultState.StateHash.Value);
        Assert.AreEqual(result.ResultState.StateHash, result.Commit.ResultStateHash);
    }

    [TestMethod]
    public void Replay_RebuildsSameProjectionAndHashAndRejectsWrongParentOrTamperedResultHash()
    {
        var scenario = OracleScenario();
        var result = CommitOracle(scenario);
        var replayed = DeterministicCausalCommit.Replay(scenario.State, result.Commit);

        Assert.AreEqual(result.ResultState.StateHash, replayed.StateHash);
        CollectionAssert.AreEqual(
            result.ResultState.Records.Select(Signature).ToArray(),
            replayed.Records.Select(Signature).ToArray());
        Assert.AreEqual(result.ResultState.CurrentOpportunityCharacterId, replayed.CurrentOpportunityCharacterId);

        var otherParent = Patch0012TestSupport.Genesis(
            creatorLocks: ImmutableArray.Create(RecordId.From(MissingRaftContract.GoalVossId)));
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Replay(otherParent, result.Commit));

        var tampered = ConstructNonPublic<E0CausalCommit>(
            result.Commit.CommitId,
            result.Commit.ParentStateHash,
            CreateStateHash(new string('a', 64)),
            result.Commit.Take,
            result.Commit.RecordMaterializations);
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Replay(scenario.State, tampered));
    }

    [TestMethod]
    public void EvolvedStateAuthoritySnapshot_ReflectsLifecycleAndNewRecordScope()
    {
        var scenario = Scenario(
            new[] { Patch0012TestSupport.BeliefSupersede("Revised belief.") },
            new[] { StateMutationDomain.CharacterBelief },
            "TAKE-EVOLVED-SNAPSHOT");
        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-EVOLVED-SNAPSHOT"),
            scenario.State,
            scenario.Binding,
            Materials((0, "BEL-VOSS-EVOLVED")));
        var snapshot = StateAuthoritySnapshot.Bind(result.ResultState);

        var oldRecord = snapshot.Records.Single(record =>
            record.RecordId == RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId));
        Assert.AreEqual(StateAuthorityRecordLifecycle.Inactive, oldRecord.Lifecycle);
        var newRecord = Assert.IsInstanceOfType<CharacterStateAuthorityRecordDescriptor>(
            snapshot.Records.Single(record => record.RecordId == RecordId.From("BEL-VOSS-EVOLVED")));
        Assert.AreEqual(StateAuthorityRecordDomain.CharacterBelief, newRecord.RecordDomain);
        Assert.AreEqual(StateAuthorityRecordLifecycle.Active, newRecord.Lifecycle);
        Assert.AreEqual(StateAuthorityRecordProtection.None, newRecord.Protection);
        Assert.AreEqual(MissingRaftContract.VossId, newRecord.SubjectCharacterId);
    }

    [TestMethod]
    public void EventAndCommitPublicSurfaces_AreMinimalAndPaired()
    {
        var contractVersion = typeof(E0CausalCommitContracts).GetField(
            nameof(E0CausalCommitContracts.ContractVersion),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Causal commit contract version field is missing.");
        Assert.AreEqual(
            "ensemble.e0.causal-commit.v1",
            contractVersion.GetRawConstantValue());
        CollectionAssert.AreEquivalent(
            new[]
            {
                "ContractVersion", "CommitId", "ParentStateHash", "ResultStateHash",
                "Take", "RecordMaterializations"
            },
            PublicPropertyNames(typeof(E0CausalCommit)).ToArray());
        Assert.IsFalse(PublicPropertyNames(typeof(E0CausalCommit)).Contains("AppliedEffects"));
        Assert.IsFalse(PublicPropertyNames(typeof(E0CausalCommit)).Contains("OpportunityCharacterId"));
        Assert.IsFalse(PublicPropertyNames(typeof(E0CausalCommit)).Contains("MutationDomain"));
        Assert.AreEqual(0, typeof(E0CausalCommit)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);

        CollectionAssert.AreEquivalent(
            new[] { "Commit", "ResultState" },
            PublicPropertyNames(typeof(E0CausalCommitResult)).ToArray());
        Assert.AreEqual(0, typeof(E0CausalCommitResult)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);

        var commit = typeof(DeterministicCausalCommit).GetMethod(
            nameof(DeterministicCausalCommit.Commit),
            BindingFlags.Public | BindingFlags.Static)!;
        CollectionAssert.AreEqual(
            new[]
            {
                typeof(CommitId), typeof(ProductionState), typeof(E0TakeStateBinding),
                typeof(E0RecordMaterializationSet)
            },
            commit.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(E0CausalCommitResult), commit.ReturnType);
        Assert.IsFalse(commit.GetParameters().Any(parameter => parameter.ParameterType == typeof(E0Take)));

        var replay = typeof(DeterministicCausalCommit).GetMethod(
            nameof(DeterministicCausalCommit.Replay),
            BindingFlags.Public | BindingFlags.Static)!;
        CollectionAssert.AreEqual(
            new[] { typeof(ProductionState), typeof(E0CausalCommit) },
            replay.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(ProductionState), replay.ReturnType);
    }

    [TestMethod]
    public void DuplicateEffectiveIdentities_AreIndexedOutsideProjectionAndFailClosed()
    {
        var scenario = Scenario(
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-DUPLICATE-INDEX");
        var commitId = CommitId.From("COMMIT-DUPLICATE-INDEX");
        var result = DeterministicCausalCommit.Commit(
            commitId,
            scenario.State,
            scenario.Binding,
            EmptyMaterializations());

        Assert.IsTrue(InvokeInternalBool(result.ResultState, "ContainsEffectiveCommitId", commitId));
        Assert.IsTrue(InvokeInternalBool(result.ResultState, "ContainsCommittedTakeId", scenario.Take.TakeId));
        AssertPrivateDuplicateValidationThrows(result.ResultState, commitId, scenario.Take.TakeId);

        var projectionBytes = InvokeInternalBytes(
            "Ensemble.E0.Core.Production.ProductionStateCanonicalizer",
            "SerializeProjection",
            result.ResultState);
        var text = Encoding.UTF8.GetString(projectionBytes);
        Assert.IsFalse(text.Contains("COMMIT-DUPLICATE-INDEX", StringComparison.Ordinal));
        Assert.IsFalse(text.Contains("TAKE-DUPLICATE-INDEX", StringComparison.Ordinal));
    }

    [TestMethod]
    public void CausalExceptions_AreClosedSanitizedAndDoNotLeakMutationText()
    {
        const string secret = "SECRET-MUTATION-PROSE-PATCH-0012";
        var scenario = Scenario(
            new[] { Patch0012TestSupport.PressureAdd(secret) },
            new[] { StateMutationDomain.Pressure },
            "TAKE-SANITIZED");
        var exception = Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Commit(
                CommitId.From("COMMIT-SANITIZED"),
                scenario.State,
                scenario.Binding,
                EmptyMaterializations()));

        Assert.IsTrue(typeof(E0CausalCommitException).IsSealed);
        Assert.AreEqual(0, typeof(E0CausalCommitException)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
        Assert.AreEqual(0, exception.Data.Count);
        Assert.IsFalse(exception.ToString().Contains(secret, StringComparison.Ordinal));
    }

    private static ScenarioData Scenario(
        IReadOnlyList<Dictionary<string, object?>> mutations,
        IReadOnlyList<StateMutationDomain> autoApproveDomains,
        string takeId,
        IReadOnlyList<StateAuthorityReviewChoice>? reviewChoices = null)
    {
        var state = Patch0012TestSupport.Genesis();
        var pipeline = Patch0012TestSupport.BuildPipeline(
            mutations,
            autoApproveDomains,
            reviewChoices);
        var take = Patch0012TestSupport.AcceptedTake(pipeline, takeId);
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var binding = E0TakeStateBinding.Bind(checkpoint, pipeline.Context, take);
        return new ScenarioData(state, pipeline, take, binding);
    }

    private static ScenarioData OracleScenario() =>
        Scenario(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure },
            "TAKE-PATCH-0012-ORACLE");

    private static E0CausalCommitResult CommitOracle(ScenarioData scenario) =>
        DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-PATCH-0012-ORACLE"),
            scenario.State,
            scenario.Binding,
            Materials((0, "PRESSURE-PATCH-0012-ORACLE")));

    private static E0RecordMaterializationSet EmptyMaterializations() =>
        E0RecordMaterializationSet.Bind(ImmutableArray<E0RecordMaterialization>.Empty);

    private static E0RecordMaterializationSet Materials(params (int Index, string Id)[] values) =>
        E0RecordMaterializationSet.Bind(
            values
                .Select(value => E0RecordMaterialization.Create(value.Index, RecordId.From(value.Id)))
                .ToImmutableArray());

    private static string Signature(ProductionRecord record)
    {
        var scope = record switch
        {
            CharacterProductionRecord character => character.SubjectCharacterId.Value,
            RelationshipProductionRecord relationship =>
                $"{relationship.SubjectCharacterId.Value}>{relationship.TargetCharacterId.Value}",
            _ => string.Empty
        };
        return $"{record.RecordId.Value}|{record.Domain}|{record.Lifecycle}|{record.Protection}|{scope}|{record.Text}|{string.Join(',', record.Provenance.Select(id => id.Value))}";
    }

    private static byte[] InvokeInternalBytes(string typeName, string methodName, params object[] args)
    {
        var type = typeof(ProductionState).Assembly.GetType(typeName, throwOnError: true)!;
        var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Internal method {typeName}.{methodName} is missing.");
        return (byte[])method.Invoke(null, args)!;
    }

    private static bool InvokeInternalBool(object instance, string methodName, object argument)
    {
        var method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Internal method {methodName} is missing.");
        return (bool)method.Invoke(instance, new[] { argument })!;
    }

    private static void AssertPrivateDuplicateValidationThrows(
        ProductionState state,
        CommitId commitId,
        TakeId takeId)
    {
        var method = typeof(DeterministicCausalCommit).GetMethod(
            "ValidateDuplicateIdentities",
            BindingFlags.Static | BindingFlags.NonPublic)!;
        var exception = Assert.Throws<TargetInvocationException>(() =>
            method.Invoke(null, new object[] { state, commitId, takeId }));
        Assert.IsInstanceOfType<E0CausalCommitException>(exception.InnerException);
    }

    private static StateHash CreateStateHash(string value)
    {
        var constructor = typeof(StateHash)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == 1);
        return (StateHash)constructor.Invoke(new object[] { value });
    }

    private static T ConstructNonPublic<T>(params object[] args)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == args.Length);
        return (T)constructor.Invoke(args);
    }

    private static HashSet<string> PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

    private sealed record ScenarioData(
        ProductionState State,
        Patch0012TestSupport.Pipeline Pipeline,
        E0Take Take,
        E0TakeStateBinding Binding);
}
