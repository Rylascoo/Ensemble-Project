using System.Reflection;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.PerformerAttempt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.PerformerAttempt;

[TestClass]
public sealed class E0PerformerAttemptContractTests
{
    [TestMethod]
    public void PerformerAttemptNamespace_ExportsOnlyApprovedPatch0017Surface()
    {
        var names = typeof(E0PerformerAttemptResult).Assembly
            .GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.PerformerAttempt",
                StringComparison.Ordinal))
            .Select(type => type.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                nameof(DeterministicE0PerformerAttemptBoundary),
                nameof(E0PerformerAttemptDisposition),
                nameof(E0PerformerAttemptException),
                nameof(E0PerformerAttemptResult)
            }.OrderBy(name => name, StringComparer.Ordinal).ToArray(),
            names);
    }

    [TestMethod]
    public void Disposition_HasExactlyApprovedNonDefaultValues()
    {
        var values = Enum.GetValues<E0PerformerAttemptDisposition>();

        CollectionAssert.AreEqual(
            new[]
            {
                E0PerformerAttemptDisposition.CandidateReady,
                E0PerformerAttemptDisposition.TechnicalFailure,
                E0PerformerAttemptDisposition.Cancelled
            },
            values);
        CollectionAssert.AreEqual(new[] { 1, 2, 3 }, values.Select(value => (int)value).ToArray());
    }

    [TestMethod]
    public void Result_IsClosedImmutableThreePropertyProjection()
    {
        var type = typeof(E0PerformerAttemptResult);
        Assert.IsTrue(type.IsSealed);
        Assert.AreEqual(0, type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(property => property.Name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[] { "Candidate", "Disposition", "SourceContextPacketId" },
            properties.Select(property => property.Name).ToArray());
        Assert.AreEqual(typeof(CandidatePerformance), properties.Single(p => p.Name == "Candidate").PropertyType);
        Assert.AreEqual(typeof(E0PerformerAttemptDisposition), properties.Single(p => p.Name == "Disposition").PropertyType);
        Assert.AreEqual(typeof(ContextPacketId), properties.Single(p => p.Name == "SourceContextPacketId").PropertyType);
        Assert.IsTrue(properties.All(property => property.SetMethod is null));
        Assert.AreEqual(
            0,
            type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Count(method => !method.IsSpecialName));
    }

    [TestMethod]
    public void Boundary_HasExactlyTwoApprovedMethods()
    {
        var methods = typeof(DeterministicE0PerformerAttemptBoundary)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .OrderBy(method => method.Name, StringComparer.Ordinal)
            .ToArray();

        Assert.AreEqual(2, methods.Length);
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0PerformerAttemptBoundary.BindCandidate)),
            typeof(E0PerformerAttemptResult),
            typeof(ContextPacket),
            typeof(CandidatePerformance));
        AssertSignature(
            methods.Single(method => method.Name == nameof(DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome)),
            typeof(E0PerformerAttemptResult),
            typeof(ContextPacket),
            typeof(E0PerformerAttemptDisposition));
    }

    [TestMethod]
    public void Exception_IsCatchableButNotPubliclyConstructible()
    {
        Assert.IsTrue(typeof(Exception).IsAssignableFrom(typeof(E0PerformerAttemptException)));
        Assert.IsTrue(typeof(E0PerformerAttemptException).IsSealed);
        Assert.AreEqual(
            0,
            typeof(E0PerformerAttemptException)
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Length);
    }

    [TestMethod]
    public void PublicSurface_ContainsNoOperationalOrAuthorityContract()
    {
        var publicTypes = typeof(E0PerformerAttemptResult).Assembly
            .GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.PerformerAttempt",
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
                     "Provider", "Model", "Request", "Raw", "Provenance", "RunId", "AttemptId",
                     "Take", "Commit", "Task", "Thread", "CancellationToken", "File", "Directory",
                     "Http", "Socket", "Windows", "Persistence", "Repository"
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
