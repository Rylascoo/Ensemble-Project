using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Take;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Cycle;

[TestClass]
public sealed class E0CausalCycleContractTests
{
    [TestMethod]
    public void CycleNamespace_ExportsOnlyApprovedPatch0016Surface()
    {
        var names = typeof(DeterministicE0CausalCycle).Assembly
            .GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Cycle",
                StringComparison.Ordinal))
            .Select(type => type.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                nameof(DeterministicE0CausalCycle),
                nameof(E0CausalCycleException),
                nameof(E0OpportunityBearingCycleResult),
                nameof(E0OpportunityBearingCycleState),
                nameof(E0PostCommitCycleState)
            }.OrderBy(name => name, StringComparer.Ordinal).ToArray(),
            names);
    }

    [TestMethod]
    public void CycleStateAndResultTypes_AreClosedImmutablePublicProjections()
    {
        AssertClosedProjection(
            typeof(E0OpportunityBearingCycleState),
            new Dictionary<string, Type>
            {
                [nameof(E0OpportunityBearingCycleState.ProductionState)] = typeof(ProductionState)
            });

        AssertClosedProjection(
            typeof(E0PostCommitCycleState),
            new Dictionary<string, Type>
            {
                [nameof(E0PostCommitCycleState.ProductionState)] = typeof(ProductionState),
                [nameof(E0PostCommitCycleState.Commit)] = typeof(E0CausalCommit)
            });

        AssertClosedProjection(
            typeof(E0OpportunityBearingCycleResult),
            new Dictionary<string, Type>
            {
                [nameof(E0OpportunityBearingCycleResult.State)] = typeof(E0OpportunityBearingCycleState),
                [nameof(E0OpportunityBearingCycleResult.OpportunityEvent)] = typeof(E0OpportunityTransition),
                [nameof(E0OpportunityBearingCycleResult.DirectorEvaluation)] = typeof(LeastInterventionDirectorEvaluation)
            });
    }

    [TestMethod]
    public void DeterministicCycle_HasExactlyFourApprovedPhaseTypedOperations()
    {
        var methods = typeof(DeterministicE0CausalCycle)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .OrderBy(method => method.Name, StringComparer.Ordinal)
            .ToArray();

        Assert.AreEqual(4, methods.Length);

        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0CausalCycle.Initialize)),
            typeof(E0OpportunityBearingCycleState),
            typeof(ProductionState));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0CausalCycle.ComposeContext)),
            typeof(E0ProductionContextContinuityResult),
            typeof(E0OpportunityBearingCycleState));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0CausalCycle.CommitAcceptedTake)),
            typeof(E0PostCommitCycleState),
            typeof(CommitId),
            typeof(E0OpportunityBearingCycleState),
            typeof(ContextPacket),
            typeof(E0Take),
            typeof(E0RecordMaterializationSet));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0CausalCycle.EstablishOpportunity)),
            typeof(E0OpportunityBearingCycleResult),
            typeof(E0PostCommitCycleState));
    }

    [TestMethod]
    public void CycleException_IsCatchableButNotPubliclyConstructible()
    {
        Assert.IsTrue(typeof(Exception).IsAssignableFrom(typeof(E0CausalCycleException)));
        Assert.IsTrue(typeof(E0CausalCycleException).IsSealed);
        Assert.AreEqual(
            0,
            typeof(E0CausalCycleException)
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Length);
    }

    [TestMethod]
    public void PublicCycleSurface_ContainsNoProviderPlatformPersistenceOrAllocationContract()
    {
        var publicSurface = typeof(DeterministicE0CausalCycle).Assembly
            .GetExportedTypes()
            .Where(type => string.Equals(type.Namespace, "Ensemble.E0.Core.Cycle", StringComparison.Ordinal))
            .SelectMany(type => new[] { type.FullName ?? type.Name }
                .Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                    .Select(property => property.PropertyType.FullName ?? property.PropertyType.Name))
                .Concat(type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .SelectMany(method => method.GetParameters()
                        .Select(parameter => parameter.ParameterType.FullName ?? parameter.ParameterType.Name))))
            .ToArray();

        foreach (var forbidden in new[]
                 {
                     "Provider", "Model", "Network", "Http", "File", "Persistence",
                     "Repository", "Windows", "Npu", "Gpu", "Task", "Timer",
                     "Allocator", "RunId", "AttemptId"
                 })
        {
            Assert.IsFalse(
                publicSurface.Any(value => value.Contains(forbidden, StringComparison.OrdinalIgnoreCase)),
                forbidden);
        }
    }

    private static void AssertClosedProjection(
        Type type,
        IReadOnlyDictionary<string, Type> expectedProperties)
    {
        Assert.IsTrue(type.IsSealed);
        Assert.AreEqual(
            0,
            type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        CollectionAssert.AreEquivalent(
            expectedProperties.Keys.ToArray(),
            properties.Select(property => property.Name).ToArray());
        foreach (var property in properties)
        {
            Assert.AreEqual(expectedProperties[property.Name], property.PropertyType);
            Assert.IsNull(property.SetMethod);
        }

        Assert.AreEqual(
            0,
            type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Count(method => !method.IsSpecialName));
    }

    private static void AssertSignature(
        MethodInfo method,
        Type returnType,
        params Type[] parameterTypes)
    {
        Assert.AreEqual(returnType, method.ReturnType);
        CollectionAssert.AreEqual(
            parameterTypes,
            method.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
    }
}
