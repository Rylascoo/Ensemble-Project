using System.Collections.Immutable;
using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class Patch0015ContractAuditTests
{
    [TestMethod]
    public void AcceptedHistoryToken_IsExactlyOpaqueAndInternallyMinimal()
    {
        var type = typeof(E0AcceptedPerformanceHistory);

        Assert.AreEqual(0, type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        Assert.AreEqual(0, type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length);
        Assert.AreEqual(0, type.GetMethods(
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly).Length);

        CollectionAssert.AreEquivalent(
            new[] { "SceneId", "CurrentStateHash", "Entries" },
            type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(property => property.Name)
                .ToArray());
        Assert.AreEqual(
            typeof(ImmutableArray<ContextRecentPerformance>),
            type.GetProperty("Entries", BindingFlags.NonPublic | BindingFlags.Instance)!.PropertyType);

        var invariantException = type.Assembly.GetType(
            "Ensemble.E0.Core.CausalCommit.E0AcceptedPerformanceHistoryInvariantException",
            throwOnError: true)!;
        Assert.IsFalse(invariantException.IsPublic);
    }

    [TestMethod]
    public void RecentPerformanceDto_HasOnlyCharacterSafeSemanticFields()
    {
        CollectionAssert.AreEquivalent(
            new[] { "SourceCharacterId", "VisibleText" },
            typeof(ContextRecentPerformance)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(property => property.Name)
                .ToArray());
        Assert.AreEqual(0, typeof(ContextRecentPerformance)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);

        foreach (var forbidden in new[]
                 {
                     "CommitId", "TakeId", "StateHash", "ContextPacketId", "CandidateContentHash",
                     "Control", "AddressedCharacterIds", "NominatedCharacterId", "Provider", "Provenance"
                 })
        {
            Assert.IsNull(typeof(ContextRecentPerformance).GetProperty(forbidden));
        }
    }

    [TestMethod]
    public void ContextV3Contracts_AreExact()
    {
        Assert.AreEqual(
            "ensemble.e0.context.v3",
            Constant(nameof(E0ContextContracts.AcceptedHistorySchemaVersion)));
        Assert.AreEqual(
            "ensemble.e0.context.production-bound.accepted-history.v1",
            Constant(nameof(E0ContextContracts.AcceptedHistoryCompositionContract)));
        Assert.AreEqual(
            "ensemble.e0.context.render.v2",
            Constant(nameof(E0ContextContracts.AcceptedHistoryRenderingContract)));

        Assert.AreEqual("ensemble.e0.context.v1", Constant(nameof(E0ContextContracts.SchemaVersion)));
        Assert.AreEqual(
            "ensemble.e0.context.full-authorized.v1",
            Constant(nameof(E0ContextContracts.CompositionContract)));
        Assert.AreEqual("ensemble.e0.context.render.v1", Constant(nameof(E0ContextContracts.RenderingContract)));
        Assert.AreEqual(
            "ensemble.e0.context.v2",
            Constant(nameof(E0ContextContracts.ProductionBoundSchemaVersion)));
        Assert.AreEqual(
            "ensemble.e0.context.production-bound.v1",
            Constant(nameof(E0ContextContracts.ProductionBoundCompositionContract)));
    }

    [TestMethod]
    public void HistoryAwarePublicMethods_AreExplicitlyNamedAndHistoricalMethodsRemain()
    {
        var composeMethods = typeof(E0ProductionContextContinuity)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        Assert.AreEqual(2, composeMethods.Length);
        AssertSignature(
            composeMethods.Single(method => method.Name == nameof(E0ProductionContextContinuity.Compose)),
            typeof(E0ProductionContextContinuityResult),
            typeof(ProductionStateCheckpoint));
        AssertSignature(
            composeMethods.Single(method => method.Name == nameof(E0ProductionContextContinuity.ComposeWithAcceptedHistory)),
            typeof(E0ProductionContextContinuityResult),
            typeof(ProductionStateCheckpoint),
            typeof(E0AcceptedPerformanceHistory));

        var bindMethods = typeof(E0TakeStateBinding)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        Assert.AreEqual(2, bindMethods.Length);
        AssertSignature(
            bindMethods.Single(method => method.Name == nameof(E0TakeStateBinding.Bind)),
            typeof(E0TakeStateBinding),
            typeof(ProductionStateCheckpoint),
            typeof(ContextPacket),
            typeof(Ensemble.E0.Core.Take.E0Take));
        AssertSignature(
            bindMethods.Single(method => method.Name == nameof(E0TakeStateBinding.BindWithAcceptedHistory)),
            typeof(E0TakeStateBinding),
            typeof(ProductionStateCheckpoint),
            typeof(ContextPacket),
            typeof(Ensemble.E0.Core.Take.E0Take),
            typeof(E0AcceptedPerformanceHistory));
    }

    [TestMethod]
    public void HistoryContinuity_PublicSurface_IsExactAndClosed()
    {
        var methods = typeof(E0AcceptedPerformanceHistoryContinuity)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        Assert.AreEqual(3, methods.Length);
        AssertSignature(
            methods.Single(method => method.Name == nameof(E0AcceptedPerformanceHistoryContinuity.Initialize)),
            typeof(E0AcceptedPerformanceHistory),
            typeof(ProductionState));
        AssertSignature(
            methods.Single(method => method.Name == nameof(E0AcceptedPerformanceHistoryContinuity.RecordCommit)),
            typeof(E0AcceptedPerformanceHistory),
            typeof(E0AcceptedPerformanceHistory),
            typeof(ProductionState),
            typeof(E0CausalCommit));
        AssertSignature(
            methods.Single(method => method.Name == nameof(E0AcceptedPerformanceHistoryContinuity.RecordOpportunity)),
            typeof(E0AcceptedPerformanceHistory),
            typeof(E0AcceptedPerformanceHistory),
            typeof(ProductionState),
            typeof(E0CausalCommit),
            typeof(Ensemble.E0.Core.Opportunity.E0OpportunityHistory),
            typeof(Ensemble.E0.Core.Opportunity.E0OpportunityTransition));

        Assert.AreEqual(0, typeof(E0AcceptedPerformanceHistoryException)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
    }

    [TestMethod]
    public void NeutralCharacterLegibleTextInvariant_IsInternalAndHasNoPlatformDependencies()
    {
        var assembly = typeof(CharacterId).Assembly;
        var helper = assembly.GetType(
            "Ensemble.E0.Core.Domain.CharacterLegibleTextInvariants",
            throwOnError: true)!;
        var failure = assembly.GetType(
            "Ensemble.E0.Core.Domain.CharacterLegibleTextFailure",
            throwOnError: true)!;

        Assert.IsFalse(helper.IsPublic);
        Assert.IsFalse(failure.IsPublic);
        Assert.IsNotNull(helper.GetMethod("Validate", BindingFlags.Static | BindingFlags.NonPublic));

        var sourceNamespaces = new[]
        {
            typeof(ContextPacket).Namespace!,
            typeof(E0AcceptedPerformanceHistory).Namespace!,
            typeof(E0ProductionContextContinuity).Namespace!
        };
        Assert.IsFalse(sourceNamespaces.Any(value => value.Contains("Windows", StringComparison.Ordinal)));
    }

    [TestMethod]
    public void Patch0015_AddsNoPersistenceProviderClockNetworkWindowsOrHardwarePublicSurface()
    {
        var publicTypes = typeof(ContextPacket).Assembly.GetExportedTypes()
            .Where(type => type.Namespace is
                "Ensemble.E0.Core.Context" or
                "Ensemble.E0.Core.CausalCommit" or
                "Ensemble.E0.Core.Continuity")
            .ToArray();
        var forbidden = new[]
        {
            "Windows", "Microsoft.Windows", "File", "Directory", "Http", "Socket",
            "Task", "Thread", "Timer", "DateTime", "Random", "Gpu", "Npu", "Qnn", "Onnx",
            "Provider", "Repository"
        };

        foreach (var type in publicTypes)
        {
            foreach (var signatureType in PublicSignatureTypes(type))
            {
                var name = signatureType.FullName ?? signatureType.Name;
                Assert.IsFalse(
                    forbidden.Any(fragment => name.Contains(fragment, StringComparison.OrdinalIgnoreCase)),
                    $"Forbidden dependency '{name}' appears on {type.FullName}.");
            }
        }
    }

    private static object? Constant(string name) =>
        typeof(E0ContextContracts)
            .GetField(name, BindingFlags.Public | BindingFlags.Static)!
            .GetRawConstantValue();

    private static void AssertSignature(
        MethodInfo method,
        Type returnType,
        params Type[] parameters)
    {
        Assert.AreEqual(returnType, method.ReturnType);
        CollectionAssert.AreEqual(
            parameters,
            method.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
    }

    private static IEnumerable<Type> PublicSignatureTypes(Type type)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
        {
            yield return Unwrap(property.PropertyType);
        }

        foreach (var method in type.GetMethods(
                     BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
        {
            yield return Unwrap(method.ReturnType);
            foreach (var parameter in method.GetParameters())
            {
                yield return Unwrap(parameter.ParameterType);
            }
        }
    }

    private static Type Unwrap(Type type) =>
        type.IsGenericType
            ? type.GetGenericArguments().FirstOrDefault() ?? type
            : type;
}
