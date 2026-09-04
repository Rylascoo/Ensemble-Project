using System.Collections.Immutable;
using System.Reflection;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class Patch0014ContractAuditTests
{
    [TestMethod]
    public void ProductionAccessOverload_IsExactAndHistoricalOverloadRemains()
    {
        var methods = typeof(CharacterBoundedAccessControl)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(method => method.Name == nameof(CharacterBoundedAccessControl.Evaluate))
            .ToArray();

        Assert.AreEqual(2, methods.Length);
        Assert.IsTrue(methods.Any(method =>
            method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(
                new[] { typeof(Ensemble.E0.Core.Fixture.ValidatedFixture), typeof(CharacterId) })));
        Assert.IsTrue(methods.Any(method =>
            method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(
                new[] { typeof(ProductionState), typeof(CharacterId) })));
        Assert.IsTrue(methods.All(method => method.ReturnType == typeof(CharacterAccessEvaluation)));
    }

    [TestMethod]
    public void AccessProjection_AddsOnlySourceStateHashToHistoricalPublicShape()
    {
        CollectionAssert.AreEquivalent(
            new[]
            {
                "SourceStateHash",
                "SceneId",
                "SubjectCharacterId",
                "Roster",
                "SceneState",
                "Pressures",
                "Constitution",
                "Disposition",
                "Circumstance",
                "Observations",
                "Knowledge",
                "Beliefs",
                "Suspicions",
                "Memories",
                "Goals",
                "Relationships"
            },
            PublicPropertyNames(typeof(CharacterAccessProjection)));

        Assert.AreEqual(typeof(StateHash?),
            typeof(CharacterAccessProjection).GetProperty("SourceStateHash")!.PropertyType);
        Assert.IsNull(typeof(CharacterAccessProjection).GetProperty("Claims"));
        Assert.IsNull(typeof(CharacterAccessProjection).GetProperty("Lifecycle"));
        Assert.IsNull(typeof(CharacterAccessProjection).GetProperty("Protection"));
        Assert.IsNull(typeof(CharacterAccessProjection).GetProperty("Provenance"));
        Assert.IsNull(typeof(CharacterAccessProjection).GetProperty("AccessDecisions"));
    }

    [TestMethod]
    public void ContextAndTrace_AddOnlyApprovedPatch0014AndPatch0015Surface()
    {
        Assert.AreEqual(typeof(StateHash?),
            typeof(ContextPacket).GetProperty("SourceStateHash")!.PropertyType);
        Assert.AreEqual(typeof(StateHash?),
            typeof(ContextCompositionTrace).GetProperty("SourceStateHash")!.PropertyType);
        Assert.AreEqual(
            typeof(ImmutableArray<ContextRecentPerformance>),
            typeof(ContextPacket).GetProperty("RecentPerformances")!.PropertyType);

        Assert.IsNull(typeof(ContextPacket).GetProperty("Claims"));
        Assert.IsNull(typeof(ContextCompositionTrace).GetProperty("IncludedRecentTakeIds"));

        var performanceTypes = typeof(ContextPacket).Assembly.GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                typeof(ContextPacket).Namespace,
                StringComparison.Ordinal))
            .Where(type => type.Name.Contains("Performance", StringComparison.Ordinal))
            .Select(type => type.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        CollectionAssert.AreEqual(
            new[] { nameof(ContextRecentPerformance) },
            performanceTypes);
    }

    [TestMethod]
    public void ContextContracts_AddOnlyApprovedVersionTokens()
    {
        var publicFields = typeof(E0ContextContracts)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(field => field.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                nameof(E0ContextContracts.AcceptedHistoryCompositionContract),
                nameof(E0ContextContracts.AcceptedHistoryRenderingContract),
                nameof(E0ContextContracts.AcceptedHistorySchemaVersion),
                nameof(E0ContextContracts.CompositionContract),
                nameof(E0ContextContracts.ProductionBoundCompositionContract),
                nameof(E0ContextContracts.ProductionBoundSchemaVersion),
                nameof(E0ContextContracts.RenderingContract),
                nameof(E0ContextContracts.SchemaVersion)
            }.OrderBy(name => name, StringComparer.Ordinal).ToArray(),
            publicFields);
        Assert.IsNull(typeof(E0ContextContracts).GetField(
            "ProductionBoundRenderingContract",
            BindingFlags.Public | BindingFlags.Static));
    }

    [TestMethod]
    public void ProductionBoundComposer_IsInternalAndHistoricalComposerRemainsSolePublicComposer()
    {
        var publicMethods = typeof(DeterministicContextComposer)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        Assert.AreEqual(1, publicMethods.Length);
        Assert.AreEqual(nameof(DeterministicContextComposer.Compose), publicMethods[0].Name);
        CollectionAssert.AreEqual(
            new[] { typeof(CharacterAccessProjection), typeof(CharacterId) },
            publicMethods[0].GetParameters().Select(parameter => parameter.ParameterType).ToArray());

        var internalProductionBound = typeof(DeterministicContextComposer).GetMethod(
            "ComposeProductionBound",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(internalProductionBound);
        Assert.IsFalse(internalProductionBound!.IsPublic);

        var internalHistoryBound = typeof(DeterministicContextComposer).GetMethod(
            "ComposeProductionBoundWithAcceptedHistory",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(internalHistoryBound);
        Assert.IsFalse(internalHistoryBound!.IsPublic);
    }

    [TestMethod]
    public void ContinuityResult_IsClosedAndCarriesOnlyAccessAndContextEvaluations()
    {
        CollectionAssert.AreEquivalent(
            new[] { "AccessEvaluation", "ContextEvaluation" },
            PublicPropertyNames(typeof(E0ProductionContextContinuityResult)));
        Assert.AreEqual(0, typeof(E0ProductionContextContinuityResult)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        Assert.IsFalse(typeof(E0ProductionContextContinuityResult)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Any(property => property.SetMethod?.IsPublic == true));

        var methods = typeof(E0ProductionContextContinuity)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .OrderBy(method => method.Name, StringComparer.Ordinal)
            .ToArray();
        Assert.AreEqual(2, methods.Length);
        Assert.IsTrue(methods.Any(method =>
            method.Name == nameof(E0ProductionContextContinuity.Compose) &&
            method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(
                new[] { typeof(ProductionStateCheckpoint) })));
        Assert.IsTrue(methods.Any(method =>
            method.Name == nameof(E0ProductionContextContinuity.ComposeWithAcceptedHistory) &&
            method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(
                new[] { typeof(ProductionStateCheckpoint), typeof(E0AcceptedPerformanceHistory) })));
    }

    private static string[] PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Select(property => property.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
}
