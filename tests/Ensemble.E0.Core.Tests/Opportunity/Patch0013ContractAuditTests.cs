using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Opportunity;

[TestClass]
public sealed class Patch0013ContractAuditTests
{
    [TestMethod]
    public void OpportunityNamespace_ContainsOnlyApprovedPublicSurface()
    {
        var assembly = typeof(E0OpportunityTransition).Assembly;
        CollectionAssert.AreEquivalent(
            new[]
            {
                nameof(E0OpportunityTransitionContracts),
                nameof(E0OpportunityHistory),
                nameof(E0OpportunityTransition),
                nameof(E0OpportunityTransitionResult),
                nameof(DeterministicOpportunityAuthority),
                nameof(E0OpportunityTransitionException)
            },
            assembly.GetExportedTypes()
                .Where(type => string.Equals(
                    type.Namespace,
                    "Ensemble.E0.Core.Opportunity",
                    StringComparison.Ordinal))
                .Select(type => type.Name)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray());

        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.Opportunity.OpportunityHistoryHash"));
        Assert.IsNull(assembly.GetType("Ensemble.E0.Core.Opportunity.HistoryHash"));
    }

    [TestMethod]
    public void EventHistoryAndResult_AreClosedMinimalAndReadOnly()
    {
        var contract = typeof(E0OpportunityTransitionContracts).GetField(
            nameof(E0OpportunityTransitionContracts.ContractVersion),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Opportunity contract field is missing.");
        Assert.AreEqual(
            "ensemble.e0.opportunity-transition.v1",
            contract.GetRawConstantValue());

        CollectionAssert.AreEquivalent(
            new[]
            {
                "ContractVersion", "ParentStateHash", "ResultStateHash",
                "StrategyContract", "SelectedCharacterId"
            },
            PublicPropertyNames(typeof(E0OpportunityTransition)));
        Assert.AreEqual(0, typeof(E0OpportunityTransition)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        AssertNoPublicSetters(typeof(E0OpportunityTransition));

        var eventProperties = PublicPropertyNames(typeof(E0OpportunityTransition));
        Assert.IsFalse(eventProperties.Contains("SourceCommitId"));
        Assert.IsFalse(eventProperties.Contains("SourceTakeId"));
        Assert.IsFalse(eventProperties.Contains("OpportunityHistory"));
        Assert.IsFalse(eventProperties.Contains("DirectorEvaluation"));
        Assert.IsFalse(eventProperties.Contains("Rule"));
        Assert.IsFalse(eventProperties.Contains("Control"));
        Assert.IsFalse(eventProperties.Contains("ContextPacket"));

        CollectionAssert.AreEquivalent(
            new[] { "SceneId", "LastOpportunityStateHash", "CharacterIds" },
            PublicPropertyNames(typeof(E0OpportunityHistory)));
        Assert.AreEqual(0, typeof(E0OpportunityHistory)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        AssertNoPublicSetters(typeof(E0OpportunityHistory));
        CollectionAssert.AreEquivalent(
            new[] { nameof(E0OpportunityHistory.Initialize) },
            typeof(E0OpportunityHistory)
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(method => method.Name)
                .Distinct(StringComparer.Ordinal)
                .ToArray());

        CollectionAssert.AreEquivalent(
            new[] { "Event", "State", "History", "DirectorEvaluation" },
            PublicPropertyNames(typeof(E0OpportunityTransitionResult)));
        Assert.AreEqual(0, typeof(E0OpportunityTransitionResult)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        AssertNoPublicSetters(typeof(E0OpportunityTransitionResult));

        Assert.IsTrue(typeof(E0OpportunityTransitionException).IsSealed);
        Assert.AreEqual(0, typeof(E0OpportunityTransitionException)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
    }

    [TestMethod]
    public void Authority_ExposesOnlyApprovedLiveAndReplayPaths()
    {
        var methods = typeof(DeterministicOpportunityAuthority)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .OrderBy(method => method.Name, StringComparer.Ordinal)
            .ToArray();
        CollectionAssert.AreEqual(
            new[]
            {
                nameof(DeterministicOpportunityAuthority.Establish),
                nameof(DeterministicOpportunityAuthority.Replay)
            },
            methods.Select(method => method.Name).ToArray());

        var establish = methods.Single(method =>
            method.Name == nameof(DeterministicOpportunityAuthority.Establish));
        CollectionAssert.AreEqual(
            new[]
            {
                typeof(ProductionState),
                typeof(E0CausalCommit),
                typeof(ContextPacket),
                typeof(E0OpportunityHistory)
            },
            establish.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(E0OpportunityTransitionResult), establish.ReturnType);
        Assert.IsFalse(establish.GetParameters().Any(parameter =>
            parameter.ParameterType == typeof(LeastInterventionDirectorEvaluation) ||
            parameter.ParameterType == typeof(DirectorOpportunityProposal)));

        var replay = methods.Single(method =>
            method.Name == nameof(DeterministicOpportunityAuthority.Replay));
        CollectionAssert.AreEqual(
            new[]
            {
                typeof(ProductionState),
                typeof(E0CausalCommit),
                typeof(E0OpportunityHistory),
                typeof(E0OpportunityTransition)
            },
            replay.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(E0OpportunityTransitionResult), replay.ReturnType);
    }

    [TestMethod]
    public void Production_AddsOnlyNarrowInternalOpportunityMutationHelper()
    {
        var helper = typeof(ProductionState).GetMethod(
            "WithEstablishedOpportunity",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("WithEstablishedOpportunity helper is missing.");
        Assert.IsFalse(helper.IsPublic);
        CollectionAssert.AreEqual(
            new[] { typeof(CharacterId), typeof(StateHash) },
            helper.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(ProductionState), helper.ReturnType);

        Assert.IsFalse(typeof(ProductionState)
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
            .Where(method => method.Name.Contains("Opportunity", StringComparison.OrdinalIgnoreCase))
            .Any(method => method.GetParameters().Any(parameter =>
                string.Equals(
                    parameter.ParameterType.Name,
                    "ProductionStateProjection",
                    StringComparison.Ordinal))));

        Assert.IsFalse(typeof(ProductionState)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Any(method =>
                method.Name.Contains("Opportunity", StringComparison.OrdinalIgnoreCase) &&
                method.Name != "get_CurrentOpportunityCharacterId"));
    }

    [TestMethod]
    public void Patch0013_DoesNotCreatePrematureProductionAccessContextOrHardwareDependencies()
    {
        var assembly = typeof(E0OpportunityTransition).Assembly;
        var accessMethods = assembly.GetType(
            "Ensemble.E0.Core.Access.CharacterBoundedAccessControl",
            throwOnError: true)!
            .GetMethods(BindingFlags.Public | BindingFlags.Static);
        Assert.IsFalse(accessMethods.Any(method => method.GetParameters().Any(parameter =>
            parameter.ParameterType == typeof(ProductionState))));

        var contextComposer = assembly.GetType(
            "Ensemble.E0.Core.Context.DeterministicContextComposer",
            throwOnError: true)!;
        Assert.IsFalse(contextComposer.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Any(method => method.GetParameters().Any(parameter =>
                parameter.ParameterType == typeof(ProductionState))));

        var opportunityTypes = assembly.GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Opportunity",
                StringComparison.Ordinal))
            .ToArray();
        var forbiddenFragments = new[]
        {
            "Windows", "Microsoft.Windows", "FileStream", "DirectoryInfo", "HttpClient",
            "Socket", "Random", "DateTime", "DateTimeOffset", "Task", "Thread",
            "CancellationToken", "Gpu", "Npu", "Qnn", "Onnx"
        };

        foreach (var type in opportunityTypes)
        {
            foreach (var signatureType in PublicSignatureTypes(type))
            {
                var name = signatureType.FullName ?? signatureType.Name;
                Assert.IsFalse(
                    forbiddenFragments.Any(fragment =>
                        name.Contains(fragment, StringComparison.OrdinalIgnoreCase)),
                    $"Forbidden Patch 0013 public dependency '{name}' appears on {type.FullName}.");
            }
        }
    }

    [TestMethod]
    public void StrategyContract_RemainsExactReplayLaw()
    {
        var strategyContract = typeof(E0DirectorContracts).GetField(
            nameof(E0DirectorContracts.LeastInterventionStrategyContract),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Director strategy contract field is missing.");
        var opportunityContract = typeof(E0DirectorContracts).GetField(
            nameof(E0DirectorContracts.OpportunityContractVersion),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Director opportunity contract field is missing.");

        Assert.AreEqual(
            "ensemble.e0.director.least-intervention.v1",
            strategyContract.GetRawConstantValue());
        Assert.AreEqual(
            "ensemble.e0.director.opportunity.v1",
            opportunityContract.GetRawConstantValue());
    }

    private static string[] PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Select(property => property.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

    private static void AssertNoPublicSetters(Type type)
    {
        Assert.IsFalse(type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Any(property => property.SetMethod?.IsPublic == true));
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

    private static Type Unwrap(Type type)
    {
        if (type.IsGenericType)
        {
            return type.GetGenericArguments().FirstOrDefault() ?? type;
        }

        return type;
    }
}
