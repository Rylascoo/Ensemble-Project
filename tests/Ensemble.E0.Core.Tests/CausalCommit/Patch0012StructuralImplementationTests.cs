using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.CausalCommit;

[TestClass]
public sealed class Patch0012StructuralImplementationTests
{
    private static readonly IReadOnlyDictionary<short, OpCode> OpCodesByValue =
        typeof(OpCodes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(OpCode))
            .Select(field => (OpCode)field.GetValue(null)!)
            .ToDictionary(opCode => opCode.Value);

    [TestMethod]
    public void CheckpointCapture_IsShallowRetainsExactStateAndDoesNoLedgerScaleWork()
    {
        var state = Patch0012TestSupport.Genesis();
        var checkpoint = ProductionStateCheckpoint.Capture(state);

        Assert.AreEqual(state.StateHash, checkpoint.StateHash);
        Assert.AreEqual(state.SceneId, checkpoint.SceneId);
        Assert.AreEqual(state.CurrentOpportunityCharacterId, checkpoint.CurrentOpportunityCharacterId);
        var sourceField = typeof(ProductionStateCheckpoint).GetField(
            "_sourceState",
            BindingFlags.Instance | BindingFlags.NonPublic)!;
        Assert.AreSame(state, sourceField.GetValue(checkpoint));
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

        var calls = CalledMethods(typeof(ProductionStateCheckpoint).GetMethod(
            nameof(ProductionStateCheckpoint.Capture),
            BindingFlags.Public | BindingFlags.Static)!);
        var forbiddenOwners = new HashSet<string>(StringComparer.Ordinal)
        {
            "ProductionStateCanonicalizer",
            "ProductionStateAuthoritySnapshot",
            "StateAuthoritySnapshot",
            "StateAuthoritySnapshotSemanticComparer",
            "RecordProvenanceGraphValidator"
        };
        Assert.IsFalse(calls.Any(call =>
            call.DeclaringType is not null && forbiddenOwners.Contains(call.DeclaringType.Name)));

        var completed = CommitZeroMutation(state, "TAKE-CHECKPOINT-CONSUMED", "COMMIT-CHECKPOINT-CONSUMED");
        Assert.Throws<E0CausalCommitException>(() =>
            ProductionStateCheckpoint.Capture(completed.ResultState));
    }

    [TestMethod]
    public void BindingOwnsTheSingleSourceSnapshotProofAndCommitDoesNotRepeatIt()
    {
        var bindCalls = CalledMethods(typeof(E0TakeStateBinding).GetMethod(
            nameof(E0TakeStateBinding.Bind),
            BindingFlags.Public | BindingFlags.Static)!);
        Assert.AreEqual(
            1,
            bindCalls.Count(call =>
                call.DeclaringType?.Name == "ProductionStateAuthoritySnapshot" &&
                call.Name == "Bind"));
        Assert.AreEqual(
            1,
            bindCalls.Count(call =>
                call.DeclaringType?.Name == "StateAuthoritySnapshotSemanticComparer" &&
                call.Name == "Equals"));

        var commitCalls = CalledMethods(typeof(DeterministicCausalCommit).GetMethod(
            nameof(DeterministicCausalCommit.Commit),
            BindingFlags.Public | BindingFlags.Static)!);
        Assert.IsFalse(commitCalls.Any(call =>
            call.DeclaringType?.Name is
                "ProductionStateAuthoritySnapshot" or
                "StateAuthoritySnapshotSemanticComparer"));
    }

    [TestMethod]
    public void CommitAndReplayShareTransitionAndCanonicalResultConstruction()
    {
        var commitCalls = CalledMethods(typeof(DeterministicCausalCommit).GetMethod(
            nameof(DeterministicCausalCommit.Commit),
            BindingFlags.Public | BindingFlags.Static)!);
        var replayCalls = CalledMethods(typeof(DeterministicCausalCommit).GetMethod(
            nameof(DeterministicCausalCommit.Replay),
            BindingFlags.Public | BindingFlags.Static)!);

        Assert.AreEqual(1, CountCall(commitCalls, "CausalCommitTransition", "Apply"));
        Assert.AreEqual(1, CountCall(replayCalls, "CausalCommitTransition", "Apply"));
        Assert.AreEqual(1, CountCall(commitCalls, "CausalCommitCanonicalizer", "ComputeResultHash"));
        Assert.AreEqual(1, CountCall(replayCalls, "CausalCommitCanonicalizer", "ComputeResultHash"));
        Assert.AreEqual(0, CountCall(commitCalls, "ProductionStateAuthoritySnapshot", "Bind"));
        Assert.AreEqual(1, CountCall(replayCalls, "ProductionStateAuthoritySnapshot", "Bind"));
        Assert.AreEqual(1, CountCall(replayCalls, "StateAuthoritySnapshotSemanticComparer", "Equals"));
    }

    [TestMethod]
    public void PublicBoundariesUseNarrowExpectedExceptionClausesRatherThanCatchAllRelabeling()
    {
        foreach (var method in new[]
                 {
                     typeof(ProductionState).GetMethod(
                         nameof(ProductionState.Initialize),
                         BindingFlags.Public | BindingFlags.Static)!,
                     typeof(DeterministicCausalCommit).GetMethod(
                         nameof(DeterministicCausalCommit.Commit),
                         BindingFlags.Public | BindingFlags.Static)!,
                     typeof(DeterministicCausalCommit).GetMethod(
                         nameof(DeterministicCausalCommit.Replay),
                         BindingFlags.Public | BindingFlags.Static)!
                 })
        {
            var body = method.GetMethodBody()
                ?? throw new InvalidOperationException($"Method body missing for {method.Name}.");
            var catchClauses = body.ExceptionHandlingClauses
                .Where(clause => clause.Flags == ExceptionHandlingClauseOptions.Clause)
                .ToArray();
            Assert.IsTrue(catchClauses.Length != 0, method.Name);
            Assert.IsFalse(catchClauses.Any(clause => clause.CatchType == typeof(Exception)), method.Name);
        }
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

        var duplicateCommitParent = WithDuplicateIndexes(
            state,
            result.Commit.CommitId.Value,
            takeId: null);
        Assert.Throws<E0CausalCommitException>(() =>
            DeterministicCausalCommit.Replay(duplicateCommitParent, result.Commit));

        var duplicateTakeParent = WithDuplicateIndexes(
            state,
            commitId: null,
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
            var bytes = InvokeInternalBytes(
                "Ensemble.E0.Core.CausalCommit.CausalCommitCanonicalizer",
                "SerializeCommitPayload",
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

    private static ProductionState WithDuplicateIndexes(
        ProductionState source,
        string? commitId,
        string? takeId)
    {
        var projectionField = typeof(ProductionState).GetField(
            "_projection",
            BindingFlags.Instance | BindingFlags.NonPublic)!;
        var projection = projectionField.GetValue(source)!;
        var constructor = typeof(ProductionState)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == 4);
        var commitIds = commitId is null
            ? ImmutableHashSet.Create<string>(StringComparer.Ordinal)
            : ImmutableHashSet.Create(StringComparer.Ordinal, commitId);
        var takeIds = takeId is null
            ? ImmutableHashSet.Create<string>(StringComparer.Ordinal)
            : ImmutableHashSet.Create(StringComparer.Ordinal, takeId);
        return (ProductionState)constructor.Invoke(
            new object[] { projection, source.StateHash, commitIds, takeIds });
    }

    private static int CountCall(
        IReadOnlyList<MethodBase> calls,
        string declaringTypeName,
        string methodName) =>
        calls.Count(call =>
            call.DeclaringType?.Name == declaringTypeName &&
            call.Name == methodName);

    private static IReadOnlyList<MethodBase> CalledMethods(MethodInfo method)
    {
        var body = method.GetMethodBody()
            ?? throw new InvalidOperationException($"Method body missing for {method.Name}.");
        var il = body.GetILAsByteArray()
            ?? throw new InvalidOperationException($"IL missing for {method.Name}.");
        var calls = new List<MethodBase>();
        var offset = 0;
        while (offset < il.Length)
        {
            short value = il[offset++] == 0xFE
                ? unchecked((short)(0xFE00 | il[offset++]))
                : il[offset - 1];
            if (!OpCodesByValue.TryGetValue(value, out var opCode))
            {
                throw new InvalidOperationException($"Unknown IL opcode 0x{value:X4}.");
            }

            var operandOffset = offset;
            if (opCode.OperandType == OperandType.InlineMethod)
            {
                var token = BitConverter.ToInt32(il, operandOffset);
                try
                {
                    var resolved = method.Module.ResolveMethod(
                        token,
                        method.DeclaringType?.GetGenericArguments(),
                        method.GetGenericArguments());
                    if (resolved is not null)
                    {
                        calls.Add(resolved);
                    }
                }
                catch (ArgumentException)
                {
                    // A malformed token would already make the assembly invalid; unresolved generic
                    // context is irrelevant to the direct call ownership assertions in this test.
                }
            }

            offset += OperandSize(opCode.OperandType, il, operandOffset);
        }

        return calls;
    }

    private static int OperandSize(OperandType operandType, byte[] il, int operandOffset) =>
        operandType switch
        {
            OperandType.InlineNone => 0,
            OperandType.ShortInlineBrTarget or
            OperandType.ShortInlineI or
            OperandType.ShortInlineVar => 1,
            OperandType.InlineVar => 2,
            OperandType.InlineBrTarget or
            OperandType.InlineField or
            OperandType.InlineI or
            OperandType.InlineMethod or
            OperandType.InlineSig or
            OperandType.InlineString or
            OperandType.InlineTok or
            OperandType.ShortInlineR => 4,
            OperandType.InlineI8 or OperandType.InlineR => 8,
            OperandType.InlineSwitch => 4 + (BitConverter.ToInt32(il, operandOffset) * 4),
            _ => throw new InvalidOperationException($"Unsupported IL operand type {operandType}.")
        };

    private static byte[] InvokeInternalBytes(string typeName, string methodName, params object[] args)
    {
        var type = typeof(ProductionState).Assembly.GetType(typeName, throwOnError: true)!;
        var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Internal method {typeName}.{methodName} is missing.");
        return (byte[])method.Invoke(null, args)!;
    }

    private static T ConstructNonPublic<T>(params object[] args)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == args.Length);
        return (T)constructor.Invoke(args);
    }
}