using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.CausalCommit;

[TestClass]
public sealed class Patch0012ContractAuditTests
{
    [TestMethod]
    public void NewPublicNamespaces_ContainOnlyApprovedPatch0012Surface()
    {
        var assembly = typeof(ProductionState).Assembly;

        CollectionAssert.AreEquivalent(
            new[]
            {
                nameof(ProductionStateContracts),
                nameof(ProductionRecordDomain),
                nameof(ProductionRecordLifecycle),
                nameof(ProductionRecordProtection),
                nameof(StateHash),
                nameof(ProductionCharacter),
                nameof(ProductionRecord),
                nameof(GlobalProductionRecord),
                nameof(CharacterProductionRecord),
                nameof(RelationshipProductionRecord),
                nameof(ProductionState),
                nameof(ProductionStateException)
            },
            PublicTypeNames(assembly, "Ensemble.E0.Core.Production"));

        CollectionAssert.AreEquivalent(
            new[]
            {
                nameof(E0CausalCommitContracts),
                nameof(ProductionStateCheckpoint),
                nameof(E0TakeStateBinding),
                nameof(E0RecordMaterialization),
                nameof(E0RecordMaterializationSet),
                nameof(E0CausalCommit),
                nameof(E0CausalCommitResult),
                nameof(E0CausalCommitException),
                nameof(DeterministicCausalCommit)
            },
            PublicTypeNames(assembly, "Ensemble.E0.Core.CausalCommit"));

        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.Production.SnapshotHash"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.CausalCommit.AppliedEffect"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.CausalCommit.AppliedEffects"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.CausalCommit.RecordIdAllocator"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.CausalCommit.CommitIdAllocator"));
    }

    [TestMethod]
    public void ExistingStrongIds_AreReusedRatherThanShadowed()
    {
        var assembly = typeof(ProductionState).Assembly;
        Assert.AreEqual("Ensemble.E0.Core.Domain", typeof(CommitId).Namespace);
        Assert.AreEqual("Ensemble.E0.Core.Domain", typeof(RecordId).Namespace);
        Assert.AreEqual("Ensemble.E0.Core.Domain", typeof(TakeId).Namespace);
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.Production.RecordId"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.CausalCommit.RecordId"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.CausalCommit.CommitId"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.CausalCommit.TakeId"));
    }

    [TestMethod]
    public void ProductionToStateAuthorityMapping_IsExplicitAndExhaustive()
    {
        var mapType = typeof(ProductionState).Assembly.GetType(
            "Ensemble.E0.Core.StateAuthority.ProductionStateAuthoritySnapshot",
            throwOnError: true)!;
        var mapDomain = mapType.GetMethod("MapDomain", BindingFlags.Static | BindingFlags.NonPublic)!;
        var mapLifecycle = mapType.GetMethod("MapLifecycle", BindingFlags.Static | BindingFlags.NonPublic)!;
        var mapProtection = mapType.GetMethod("MapProtection", BindingFlags.Static | BindingFlags.NonPublic)!;

        var expectedDomains = new Dictionary<ProductionRecordDomain, StateAuthorityRecordDomain>
        {
            [ProductionRecordDomain.HistoricalTruth] = StateAuthorityRecordDomain.HistoricalTruth,
            [ProductionRecordDomain.UnresolvedProposition] = StateAuthorityRecordDomain.UnresolvedProposition,
            [ProductionRecordDomain.WorldState] = StateAuthorityRecordDomain.WorldState,
            [ProductionRecordDomain.SceneState] = StateAuthorityRecordDomain.SceneState,
            [ProductionRecordDomain.CharacterConstitution] = StateAuthorityRecordDomain.CharacterConstitution,
            [ProductionRecordDomain.CharacterDisposition] = StateAuthorityRecordDomain.CharacterDisposition,
            [ProductionRecordDomain.CharacterCircumstance] = StateAuthorityRecordDomain.CharacterCircumstance,
            [ProductionRecordDomain.CharacterObservation] = StateAuthorityRecordDomain.CharacterObservation,
            [ProductionRecordDomain.CharacterKnowledge] = StateAuthorityRecordDomain.CharacterKnowledge,
            [ProductionRecordDomain.CharacterBelief] = StateAuthorityRecordDomain.CharacterBelief,
            [ProductionRecordDomain.CharacterSuspicion] = StateAuthorityRecordDomain.CharacterSuspicion,
            [ProductionRecordDomain.CharacterMemory] = StateAuthorityRecordDomain.CharacterMemory,
            [ProductionRecordDomain.CharacterGoal] = StateAuthorityRecordDomain.CharacterGoal,
            [ProductionRecordDomain.CharacterClaim] = StateAuthorityRecordDomain.CharacterClaim,
            [ProductionRecordDomain.Relationship] = StateAuthorityRecordDomain.Relationship,
            [ProductionRecordDomain.Pressure] = StateAuthorityRecordDomain.Pressure
        };

        foreach (var pair in expectedDomains)
        {
            Assert.AreEqual(pair.Value, mapDomain.Invoke(null, new object[] { pair.Key }));
        }

        Assert.AreEqual(
            StateAuthorityRecordLifecycle.Active,
            mapLifecycle.Invoke(null, new object[] { ProductionRecordLifecycle.Active }));
        Assert.AreEqual(
            StateAuthorityRecordLifecycle.Inactive,
            mapLifecycle.Invoke(null, new object[] { ProductionRecordLifecycle.Inactive }));
        Assert.AreEqual(
            StateAuthorityRecordProtection.None,
            mapProtection.Invoke(null, new object[] { ProductionRecordProtection.None }));
        Assert.AreEqual(
            StateAuthorityRecordProtection.SystemImmutable,
            mapProtection.Invoke(null, new object[] { ProductionRecordProtection.SystemImmutable }));
        Assert.AreEqual(
            StateAuthorityRecordProtection.CreatorLocked,
            mapProtection.Invoke(null, new object[] { ProductionRecordProtection.CreatorLocked }));

        AssertInvocationInner<StateAuthorityException>(() =>
            mapDomain.Invoke(null, new object[] { ProductionRecordDomain.Unspecified }));
        AssertInvocationInner<StateAuthorityException>(() =>
            mapLifecycle.Invoke(null, new object[] { ProductionRecordLifecycle.Unspecified }));
        AssertInvocationInner<StateAuthorityException>(() =>
            mapProtection.Invoke(null, new object[] { ProductionRecordProtection.Unspecified }));
    }

    [TestMethod]
    public void SharedMutationDomainCanonicalTokens_AreExactAndExhaustive()
    {
        var tokenType = typeof(ProductionState).Assembly.GetType(
            "Ensemble.E0.Core.StateInterpreter.StateMutationDomainCanonicalTokens",
            throwOnError: true)!;
        var get = tokenType.GetMethod("Get", BindingFlags.Static | BindingFlags.NonPublic)!;
        var expected = new Dictionary<StateMutationDomain, string>
        {
            [StateMutationDomain.WorldState] = "worldState",
            [StateMutationDomain.SceneState] = "sceneState",
            [StateMutationDomain.UnresolvedProposition] = "unresolvedProposition",
            [StateMutationDomain.CharacterKnowledge] = "characterKnowledge",
            [StateMutationDomain.CharacterBelief] = "characterBelief",
            [StateMutationDomain.CharacterSuspicion] = "characterSuspicion",
            [StateMutationDomain.CharacterMemory] = "characterMemory",
            [StateMutationDomain.CharacterGoal] = "characterGoal",
            [StateMutationDomain.CharacterDisposition] = "characterDisposition",
            [StateMutationDomain.CharacterCircumstance] = "characterCircumstance",
            [StateMutationDomain.CharacterClaim] = "characterClaim",
            [StateMutationDomain.Relationship] = "relationship",
            [StateMutationDomain.Pressure] = "pressure"
        };

        Assert.AreEqual(Enum.GetValues<StateMutationDomain>().Length, expected.Count);
        foreach (var pair in expected)
        {
            Assert.AreEqual(pair.Value, get.Invoke(null, new object[] { pair.Key }));
        }
    }

    [TestMethod]
    public void SharedProvenancePrimitiveAndNeutralGenesisProjection_AreSingleInternalOwnershipPoints()
    {
        var assembly = typeof(ProductionState).Assembly;
        var provenance = assembly.GetType(
            "Ensemble.E0.Core.Provenance.RecordProvenanceGraphValidator",
            throwOnError: true)!;
        var fixtureAdapter = assembly.GetType(
            "Ensemble.E0.Core.Fixture.ProvenanceDagValidator",
            throwOnError: true)!;
        var genesis = assembly.GetType(
            "Ensemble.E0.Core.Production.ProductionGenesisProjection",
            throwOnError: true)!;

        Assert.IsFalse(provenance.IsPublic);
        Assert.IsFalse(fixtureAdapter.IsPublic);
        Assert.IsFalse(genesis.IsPublic);
        Assert.IsNotNull(genesis.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic));
        Assert.IsNotNull(genesis.GetMethod("CreateForAuthority", BindingFlags.Static | BindingFlags.NonPublic));

        var fixtureBind = typeof(StateAuthoritySnapshot).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method =>
                method.Name == nameof(StateAuthoritySnapshot.Bind) &&
                method.GetParameters().First().ParameterType.Name == "ValidatedFixture");
        var productionBind = typeof(StateAuthoritySnapshot).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method =>
                method.Name == nameof(StateAuthoritySnapshot.Bind) &&
                method.GetParameters().First().ParameterType == typeof(ProductionState));
        Assert.AreEqual(typeof(StateAuthoritySnapshot), fixtureBind.ReturnType);
        Assert.AreEqual(typeof(StateAuthoritySnapshot), productionBind.ReturnType);
    }

    [TestMethod]
    public void SnapshotComparator_IsInternalOnlyAndNoSnapshotHashExists()
    {
        var assembly = typeof(ProductionState).Assembly;
        var comparer = assembly.GetType(
            "Ensemble.E0.Core.StateAuthority.StateAuthoritySnapshotSemanticComparer",
            throwOnError: true)!;
        Assert.IsFalse(comparer.IsPublic);
        Assert.IsNull(assembly.GetTypes().SingleOrDefault(type => type.Name == "SnapshotHash"));
    }

    [TestMethod]
    public void Patch0012_CausalCommitSurfaceRemainsNarrowAfterLaterAccessIntegration()
    {
        var assembly = typeof(ProductionState).Assembly;
        var contextComposer = assembly.GetType(
            "Ensemble.E0.Core.Context.DeterministicContextComposer",
            throwOnError: true)!;
        Assert.IsFalse(contextComposer.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Any(method => method.GetParameters().Any(parameter => parameter.ParameterType == typeof(ProductionState))));

        Assert.IsFalse(typeof(DeterministicCausalCommit)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Any(method => method.Name.Contains("Next", StringComparison.OrdinalIgnoreCase)));
        Assert.IsFalse(typeof(E0CausalCommitResult)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Any(property => property.Name.Contains("Next", StringComparison.OrdinalIgnoreCase)));
    }

    [TestMethod]
    public void Patch0012_PublicSurface_HasNoPersistenceProviderWindowsOrHardwareDependencies()
    {
        var newTypes = typeof(ProductionState).Assembly.GetExportedTypes()
            .Where(type => type.Namespace is
                "Ensemble.E0.Core.Production" or
                "Ensemble.E0.Core.CausalCommit")
            .ToArray();
        var forbiddenFragments = new[]
        {
            "Windows", "Microsoft.Windows", "FileStream", "DirectoryInfo", "HttpClient",
            "Socket", "Random", "DateTime", "DateTimeOffset", "Task", "Thread", "CancellationToken",
            "Gpu", "Npu", "Qnn", "Onnx"
        };

        foreach (var type in newTypes)
        {
            foreach (var memberType in PublicSignatureTypes(type))
            {
                var name = memberType.FullName ?? memberType.Name;
                Assert.IsFalse(
                    forbiddenFragments.Any(fragment => name.Contains(fragment, StringComparison.OrdinalIgnoreCase)),
                    $"Forbidden Patch 0012 public dependency '{name}' appears on {type.FullName}.");
            }
        }
    }

    [TestMethod]
    public void CausalCommit_ExposesNoStateOnlyCommitOrEventRewriteDeleteAPI()
    {
        var methods = typeof(DeterministicCausalCommit)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .ToArray();
        CollectionAssert.AreEquivalent(
            new[] { nameof(DeterministicCausalCommit.Commit), nameof(DeterministicCausalCommit.Replay) },
            methods.Select(method => method.Name).Distinct(StringComparer.Ordinal).ToArray());
        Assert.IsFalse(methods.Any(method => method.ReturnType == typeof(ProductionState) && method.Name != nameof(DeterministicCausalCommit.Replay)));
        Assert.IsFalse(methods.Any(method =>
            method.Name.Contains("Delete", StringComparison.OrdinalIgnoreCase) ||
            method.Name.Contains("Rewrite", StringComparison.OrdinalIgnoreCase) ||
            method.Name.Contains("Update", StringComparison.OrdinalIgnoreCase)));
    }

    private static string[] PublicTypeNames(Assembly assembly, string @namespace) =>
        assembly.GetExportedTypes()
            .Where(type => string.Equals(type.Namespace, @namespace, StringComparison.Ordinal))
            .Select(type => type.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

    private static void AssertInvocationInner<TException>(Action action)
        where TException : Exception
    {
        var wrapper = Assert.Throws<TargetInvocationException>(action);
        Assert.IsInstanceOfType<TException>(wrapper.InnerException);
    }

    private static IEnumerable<Type> PublicSignatureTypes(Type type)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
        {
            yield return Unwrap(property.PropertyType);
        }

        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
        {
            yield return Unwrap(method.ReturnType);
            foreach (var parameter in method.GetParameters())
            {
                yield return Unwrap(parameter.ParameterType);
            }
        }
    }

    private static Type Unwrap(Type type)
    {
        if (type.IsGenericType)
        {
            return type.GetGenericArguments().FirstOrDefault() ?? type;
        }

        return type;
    }
}
