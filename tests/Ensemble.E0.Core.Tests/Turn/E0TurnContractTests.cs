using System.Collections.Immutable;
using System.Reflection;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Turn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Turn;

[TestClass]
public sealed class E0TurnContractTests
{
    [TestMethod]
    public void TurnNamespace_ExportsOnlyApprovedPatch0018Surface()
    {
        var names = typeof(E0TurnProgress).Assembly
            .GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Turn",
                StringComparison.Ordinal))
            .Select(type => type.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                nameof(DeterministicE0TurnOrchestrator),
                nameof(E0TurnOrchestrationException),
                nameof(E0TurnProgress),
                nameof(E0TurnProgressDisposition)
            }.OrderBy(name => name, StringComparer.Ordinal).ToArray(),
            names);
    }

    [TestMethod]
    public void Disposition_HasExactlyApprovedEightNonDefaultValues()
    {
        var values = Enum.GetValues<E0TurnProgressDisposition>();

        CollectionAssert.AreEqual(
            new[]
            {
                E0TurnProgressDisposition.TechnicalFailure,
                E0TurnProgressDisposition.Cancelled,
                E0TurnProgressDisposition.CandidateReady,
                E0TurnProgressDisposition.RequestAnotherTake,
                E0TurnProgressDisposition.ReadyForInterpretation,
                E0TurnProgressDisposition.AuthorityReviewRequired,
                E0TurnProgressDisposition.TakeBindable,
                E0TurnProgressDisposition.AcceptedTakeReady
            },
            values);
        CollectionAssert.AreEqual(
            Enumerable.Range(1, 8).ToArray(),
            values.Select(value => (int)value).ToArray());
    }

    [TestMethod]
    public void Progress_IsClosedImmutableEightPropertyProjection()
    {
        var type = typeof(E0TurnProgress);
        Assert.IsTrue(type.IsSealed);
        Assert.AreEqual(0, type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(property => property.Name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                "AcceptedTake",
                "AuthorityEvaluation",
                "Candidate",
                "Disposition",
                "IntegrityEvaluation",
                "InterpretationProposal",
                "InterpretationSource",
                "SourceContext"
            },
            properties.Select(property => property.Name).ToArray());
        Assert.IsTrue(properties.All(property => property.SetMethod is null));
        Assert.AreEqual(
            0,
            type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Count(method => !method.IsSpecialName));
    }

    [TestMethod]
    public void Orchestrator_HasExactlySixApprovedMethods()
    {
        var methods = typeof(DeterministicE0TurnOrchestrator)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .OrderBy(method => method.Name, StringComparer.Ordinal)
            .ToArray();

        Assert.AreEqual(6, methods.Length);
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0TurnOrchestrator.GateAttempt)),
            typeof(E0TurnProgress),
            typeof(E0OpportunityBearingCycleState),
            typeof(E0PerformerAttemptResult));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0TurnOrchestrator.EvaluateIntegrity)),
            typeof(E0TurnProgress),
            typeof(E0TurnProgress),
            typeof(ImmutableArray<IntegrityConcernKind>));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0TurnOrchestrator.EvaluateAuthority)),
            typeof(E0TurnProgress),
            typeof(E0TurnProgress),
            typeof(StateInterpretationProposal),
            typeof(StateAuthorityPolicy),
            typeof(ImmutableArray<StateAuthorityReviewChoice>));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0TurnOrchestrator.ResolveAuthorityReview)),
            typeof(E0TurnProgress),
            typeof(E0TurnProgress),
            typeof(ImmutableArray<StateAuthorityReviewChoice>));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0TurnOrchestrator.BindAcceptedTake)),
            typeof(E0TurnProgress),
            typeof(TakeId),
            typeof(E0TurnProgress));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0TurnOrchestrator.CommitAccepted)),
            typeof(E0PostCommitCycleState),
            typeof(CommitId),
            typeof(E0TurnProgress),
            typeof(E0RecordMaterializationSet));
    }

    [TestMethod]
    public void Exception_IsCatchableButNotPubliclyConstructible()
    {
        Assert.IsTrue(typeof(Exception).IsAssignableFrom(typeof(E0TurnOrchestrationException)));
        Assert.IsTrue(typeof(E0TurnOrchestrationException).IsSealed);
        Assert.AreEqual(
            0,
            typeof(E0TurnOrchestrationException)
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Length);
    }

    [TestMethod]
    public void PublicSurface_ContainsNoProviderPlatformOrRunLoopContract()
    {
        var publicTypes = typeof(E0TurnProgress).Assembly
            .GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Turn",
                StringComparison.Ordinal))
            .ToArray();
        var surface = publicTypes
            .SelectMany(type => new[] { type.FullName ?? type.Name }
                .Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                    .Select(property => property.PropertyType.FullName ?? property.PropertyType.Name))
                .Concat(type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .SelectMany(method => method.GetParameters()
                        .Select(parameter => parameter.ParameterType.FullName ?? parameter.ParameterType.Name))))
            .ToArray();

        foreach (var forbidden in new[]
                 {
                     "Provider", "Model", "Http", "Socket", "Network", "Task", "Thread",
                     "CancellationToken", "Persistence", "Repository", "Retry", "Spend",
                     "RunId", "AttemptId", "Windows", "WinUI"
                 })
        {
            Assert.IsFalse(
                surface.Any(value => value.Contains(forbidden, StringComparison.OrdinalIgnoreCase)),
                forbidden);
        }
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
