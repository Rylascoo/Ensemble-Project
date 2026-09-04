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
    public void ContextAndTrace_AddOnlySourceStateHashAndNoHistorySurface()
    {
        Assert.AreEqual(typeof(StateHash?),
            typeof(ContextPacket).GetProperty("SourceStateHash")!.PropertyType);
        Assert.AreEqual(typeof(StateHash?),
            typeof(ContextCompositionTrace).GetProperty("SourceStateHash")!.PropertyType);

        Assert.IsNull(typeof(ContextPacket).GetProperty("Claims"));
        Assert.IsNull(typeof(ContextPacket).GetProperty("RecentPerformances"));
        Assert.IsNull(typeof(ContextCompositionTrace).GetProperty("IncludedRecentTakeIds"));

        var performanceTypes = typeof(ContextPacket).Assembly.GetTypes()
            .Where(type => string.Equals(
                type.Namespace,
                typeof(ContextPacket).Namespace,
                StringComparison.Ordinal))
            .Where(type => type.Name.Contains("Performance", StringComparison.Ordinal))
            .ToArray();
        Assert.AreEqual(0, performanceTypes.Length);
    }

    [TestMethod]
    public void ContextContracts_AddOnlyProductionBoundSchemaAndCompositionTokens()
    {
        var publicFields = typeof(E0ContextContracts)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(field => field.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
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

        var compose = typeof(E0ProductionContextContinuity)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Single();
        CollectionAssert.AreEqual(
            new[] { typeof(ProductionStateCheckpoint) },
            compose.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
    }

    private static string[] PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Select(property => property.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
}
