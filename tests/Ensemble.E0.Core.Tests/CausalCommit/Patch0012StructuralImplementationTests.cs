using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using System.Text;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.CausalCommit;

[TestClass]
public sealed class Patch0012StructuralImplementationTests
{
    [TestMethod]
    public void CheckpointCapture_RetainsExactStateAndApprovedPublicSurface()
    {
        var state = Patch0012TestSupport.Genesis();
        var checkpoint = ProductionStateCheckpoint.Capture(state);

        Assert.AreEqual(state.StateHash, checkpoint.StateHash);
        Assert.AreEqual(state.SceneId, checkpoint.SceneId);
        Assert.AreEqual(state.CurrentOpportunityCharacterId!.Value, checkpoint.CurrentOpportunityCharacterId);

        // Reflection is test plumbing here: the frozen contract requires retention of
        // the exact immutable source-state reference, but does not freeze a private field name.
        var stateFields = typeof(ProductionStateCheckpoint)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(field => field.FieldType == typeof(ProductionState))
            .ToArray();
        Assert.AreEqual(1, stateFields.Length);
        Assert.AreSame(state, stateFields[0].GetValue(checkpoint));

        CollectionAssert.AreEquivalent(
            new[] { "StateHash", "SceneId", "CurrentOpportunityCharacterId" },
            typeof(ProductionStateCheckpoint)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(property => property.Name)
                .ToArray());
        Assert.AreEqual(
            0,
            typeof(ProductionStateCheckpoint)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);

        var completed = CommitZeroMutation(state, "TAKE-CHECKPOINT-CONSUMED", "COMMIT-CHECKPOINT-CONSUMED");
        Assert.Throws<E0CausalCommitException>(() =>
            ProductionStateCheckpoint.Capture(completed.ResultState));
    }

    [TestMethod]
    public void BindingCommitAndReplay_PublicSurfacePreservesFrozenAuthoritySeparation()
    {
        var bind = typeof(E0TakeStateBinding)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == nameof(E0TakeStateBinding.Bind));
        CollectionAssert.AreEqual(
            new[] { typeof(ProductionStateCheckpoint), typeof(ContextPacket), typeof(E0Take) },
            bind.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(E0TakeStateBinding), bind.ReturnType);

        var bindWithHistory = typeof(E0TakeStateBinding).GetMethod(
            nameof(E0TakeStateBinding.BindWithAcceptedHistory),
            BindingFlags.Public | BindingFlags.Static)!;
        CollectionAssert.AreEqual(
            new[]
            {
                typeof(ProductionStateCheckpoint),
                typeof(ContextPacket),
                typeof(E0Take),
                typeof(E0AcceptedPerformanceHistory)
            },
            bindWithHistory.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(E0TakeStateBinding), bindWithHistory.ReturnType);

        var commit = typeof(DeterministicCausalCommit).GetMethod(
            nameof(DeterministicCausalCommit.Commit),
            BindingFlags.Public | BindingFlags.Static)!;
        CollectionAssert.AreEqual(
            new[]
            {
                typeof(CommitId),
                typeof(ProductionState),
                typeof(E0TakeStateBinding),
                typeof(E0RecordMaterializationSet)
            },
            commit.GetParameters().Select(parameter => parameter.ParameterType).ToArray());

        var replay = typeof(DeterministicCausalCommit).GetMethod(
            nameof(DeterministicCausalCommit.Replay),
            BindingFlags.Public | BindingFlags.Static)!;
        CollectionAssert.AreEqual(
            new[] { typeof(ProductionState), typeof(E0CausalCommit) },
            replay.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
    }

    [TestMethod]
    public void CommitAndReplay_ProduceTheSameCanonicalTransition()
    {
        var state = Patch0012TestSupport.Genesis();
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure });
        var take = Patch0012TestSupport.AcceptedTake(pipeline, "TAKE-COMMIT-REPLAY-EQUIVALENCE");
        var binding = E0TakeStateBinding.Bind(
            ProductionStateCheckpoint.Capture(state),
            pipeline.Context,
            take);
        var committed = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-COMMIT-REPLAY-EQUIVALENCE"),
            state,
            binding,
            Materials((0, "PRESSURE-COMMIT-REPLAY-EQUIVALENCE")));

        var replayed = DeterministicCausalCommit.Replay(state, committed.Commit);

        Assert.AreEqual(committed.ResultState.StateHash, replayed.StateHash);
        Assert.AreEqual(committed.ResultState.SceneId, replayed.SceneId);
        Assert.AreEqual(
            committed.ResultState.CurrentOpportunityCharacterId,
            replayed.CurrentOpportunityCharacterId);
        CollectionAssert.AreEqual(
            committed.ResultState.Records.Select(record => record.RecordId.Value).ToArray(),
            replayed.Records.Select(record => record.RecordId.Value).ToArray());
        CollectionAssert.AreEqual(
            committed.ResultState.Records.Select(record => record.Lifecycle).ToArray(),
            replayed.Records.Select(record => record.Lifecycle).ToArray());
    }

    [TestMethod]
    public void ReplayRejectsNonAcceptedTakeInvalidMaterializationsAndDuplicateEffectiveIdentities()
    {
        var state = Patch0012TestSupport.Genesis();
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure });
        var accepted = Patch0012TestSupport.AcceptedTake(pipeline, "TAKE-REPLAY-STRUCTURAL");
        var binding = E0TakeStateBinding.Bind(
            ProductionStateCheckpoint.Capture(state),
            pipeline.Context,
            accepted);
        var materializations = Materials((0, "PRESSURE-REPLAY-STRUCTURAL"));
        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-REPLAY-STRUCTURAL"),
            state,
            binding,
            materializations);

        var rejected = Patch0012TestSupport.Take(
            pipeline,
            E0TakeDisposition.Rejected,
            "TAKE-REPLAY-REJECTED");
        var rejectedEvent = ConstructNonPublic<E0CausalCommit>(
            result.Commit.CommitId,
            result.Commit.ParentStateHash,
            result.Commit.ResultStateHash,
            rejected,
            result.Commit.RecordMaterializations);
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Replay(state, rejectedEvent));

        var missingMaterializationEvent = ConstructNonPublic<E0CausalCommit>(
            result.Commit.CommitId,
            result.Commit.ParentStateHash,
            result.Commit.ResultStateHash,
            result.Commit.Take,
            E0RecordMaterializationSet.Bind(ImmutableArray<E0RecordMaterialization>.Empty));
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Replay(state, missingMaterializationEvent));

        var duplicateCommitParent = WithDuplicateEffectiveIdentity(
            state,
            result.Commit.CommitId.Value);
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Replay(duplicateCommitParent, result.Commit));

        var duplicateTakeParent = WithDuplicateEffectiveIdentity(
            state,
            result.Commit.Take.TakeId.Value);
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Replay(duplicateTakeParent, result.Commit));
    }

    [TestMethod]
    public void CandidateAndProposalSemanticIdentityBothAffectCausalStateHash()
    {
        var baseline = CommitPressureHash("No.", "Pressure increases.");
        var candidateChanged = CommitPressureHash("Different candidate.", "Pressure increases.");
        var proposalChanged = CommitPressureHash("No.", "Different pressure consequence.");

        Assert.AreNotEqual(baseline, candidateChanged);
        Assert.AreNotEqual(baseline, proposalChanged);
        Assert.AreNotEqual(candidateChanged, proposalChanged);
    }

    [TestMethod]
    public void CanonicalMutationIndexUsesInvariantMinimalAsciiDecimal()
    {
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure });
        var take = Patch0012TestSupport.AcceptedTake(pipeline, "TAKE-INVARIANT-INDEX");
        var materializations = E0RecordMaterializationSet.Bind(
            ImmutableArray.Create(
                E0RecordMaterialization.Create(12, RecordId.From("RECORD-INVARIANT-INDEX"))));

        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ar-SA");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ar-SA");
            var bytes = InvokeCanonicalCommitPayload(
                CommitId.From("COMMIT-INVARIANT-INDEX"),
                take,
                materializations);
            var json = Encoding.UTF8.GetString(bytes);
            Assert.IsTrue(json.Contains("\"mutationIndex\":12", StringComparison.Ordinal));
            Assert.IsFalse(json.Contains('١'));
            Assert.IsFalse(json.Contains('٢'));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    private static E0CausalCommitResult CommitZeroMutation(
        ProductionState state,
        string takeId,
        string commitId)
    {
        var pipeline = Patch0012TestSupport.BuildPipeline(
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>());
        var take = Patch0012TestSupport.AcceptedTake(pipeline, takeId);
        var binding = E0TakeStateBinding.Bind(
            ProductionStateCheckpoint.Capture(state),
            pipeline.Context,
            take);
        return DeterministicCausalCommit.Commit(
            CommitId.From(commitId),
            state,
            binding,
            E0RecordMaterializationSet.Bind(ImmutableArray<E0RecordMaterialization>.Empty));
    }

    private static StateHash CommitPressureHash(string candidateText, string mutationText)
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd(mutationText) },
            new[] { StateMutationDomain.Pressure },
            candidateText: candidateText,
            fixture: fixture);
        var take = Patch0012TestSupport.AcceptedTake(pipeline, "TAKE-SEMANTIC-HASH");
        var binding = E0TakeStateBinding.Bind(
            ProductionStateCheckpoint.Capture(state),
            pipeline.Context,
            take);
        return DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-SEMANTIC-HASH"),
            state,
            binding,
            Materials((0, "PRESSURE-SEMANTIC-HASH"))).ResultState.StateHash;
    }

    private static E0RecordMaterializationSet Materials(params (int Index, string Id)[] values) =>
        E0RecordMaterializationSet.Bind(
            values
                .Select(value => E0RecordMaterialization.Create(value.Index, RecordId.From(value.Id)))
                .ToImmutableArray());

    private static ProductionState WithDuplicateEffectiveIdentity(
        ProductionState source,
        string identity)
    {
        // Reflection is test plumbing to construct an otherwise-unrepresentable invalid
        // parent state. No private field or constructor parameter name is part of the assertion.
        var projectionMember = typeof(ProductionState)
            .GetMembers(BindingFlags.Instance | BindingFlags.NonPublic)
            .OfType<PropertyInfo>()
            .Single(property => property.PropertyType.Name == "ProductionStateProjection");
        var projection = projectionMember.GetValue(source)!;
        var identities = ImmutableHashSet.Create(StringComparer.Ordinal, identity);
        var constructor = typeof(ProductionState)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate =>
            {
                var types = candidate.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                return types.Length == 4 &&
                       types.Count(type => type == typeof(ImmutableHashSet<string>)) == 2 &&
                       types.Contains(typeof(StateHash));
            });
        var arguments = constructor.GetParameters()
            .Select(parameter =>
                parameter.ParameterType == typeof(StateHash)
                    ? (object)source.StateHash
                    : parameter.ParameterType == typeof(ImmutableHashSet<string>)
                        ? identities
                        : projection)
            .ToArray();
        return (ProductionState)constructor.Invoke(arguments);
    }

    private static byte[] InvokeCanonicalCommitPayload(
        CommitId commitId,
        E0Take take,
        E0RecordMaterializationSet materializations)
    {
        // The exact canonical payload is frozen. Reflection is only plumbing to reach
        // the internal serializer; its private owner/method name is intentionally not frozen.
        var method = typeof(ProductionState).Assembly
            .GetTypes()
            .Where(type => string.Equals(type.Namespace, "Ensemble.E0.Core.CausalCommit", StringComparison.Ordinal))
            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            .Single(candidate =>
            {
                var parameters = candidate.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                return candidate.ReturnType == typeof(byte[]) &&
                       parameters.SequenceEqual(new[]
                       {
                           typeof(CommitId),
                           typeof(E0Take),
                           typeof(E0RecordMaterializationSet)
                       });
            });
        return (byte[])method.Invoke(null, new object[] { commitId, take, materializations })!;
    }

    private static T ConstructNonPublic<T>(params object[] args)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == args.Length);
        return (T)constructor.Invoke(args);
    }
}
