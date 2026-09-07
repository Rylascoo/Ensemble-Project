using System.Text;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0APostAuditGroup5Tests
{
    private const string ExpectedCommit = "0123456789abcdef0123456789abcdef01234567";
    private const string RepositoryRoot = "C:/repo/Ensemble-Project";

    [TestMethod]
    public void IntegrityConcernParser_AcceptsExactlyTheFiveFrozenOrdinalNames()
    {
        var expected = new[]
        {
            IntegrityConcernKind.PotentialInaccessibleInformationUse,
            IntegrityConcernKind.PotentialProtectedInformationExposure,
            IntegrityConcernKind.PotentialLockedAuthorityViolation,
            IntegrityConcernKind.PotentialTechnicalArtifactLeak,
            IntegrityConcernKind.IndeterminateSemanticIntegrity
        };

        CollectionAssert.AreEqual(
            expected.Select(x => x.ToString()).ToArray(),
            E0AIntegrityConcernParser.AllowedNames.ToArray());

        foreach (var concern in expected)
        {
            var parsed = E0AIntegrityConcernParser.Parse(E0ATestSupport.IntegrityOutput(concern.ToString()));
            Assert.AreEqual(1, parsed.Length);
            Assert.AreEqual(concern, parsed[0]);
        }
    }

    [TestMethod]
    public void IntegrityConcernParser_RejectsNumericPaddedUndefinedAndNoncanonicalRepresentations()
    {
        var canonical = nameof(IntegrityConcernKind.PotentialTechnicalArtifactLeak);
        var invalidPayloads = new[]
        {
            E0ATestSupport.IntegrityOutput("1"),
            E0ATestSupport.IntegrityOutput($" {canonical}"),
            E0ATestSupport.IntegrityOutput($"{canonical} "),
            E0ATestSupport.IntegrityOutput("UndefinedConcern"),
            E0ATestSupport.IntegrityOutput(canonical.ToLowerInvariant()),
            Encoding.UTF8.GetBytes("{\"concerns\":[1]}")
        };

        foreach (var payload in invalidPayloads)
        {
            Assert.Throws<E0AHarnessException>(() => E0AIntegrityConcernParser.Parse(payload));
        }
    }

    [TestMethod]
    public void CheckoutGuard_ForcesInspectionThroughDiscoveredRepositoryRoot()
    {
        var runner = CleanRunner("notes.txt");

        E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, runner);

        CollectionAssert.AreEqual(
            new[] { "rev-parse", "--show-toplevel" },
            runner.Calls[0]);
        for (var index = 1; index < runner.Calls.Count; index++)
        {
            Assert.AreEqual("-C", runner.Calls[index][0]);
            Assert.AreEqual(RepositoryRoot, runner.Calls[index][1]);
        }
        CollectionAssert.AreEqual(
            new[]
            {
                "-C", RepositoryRoot, "ls-files", "--others", "--exclude-standard", "--full-name"
            },
            runner.Calls[^1]);
    }

    [TestMethod]
    public void CheckoutGuard_SubdirectoryLaunchCannotHideMaterialUntrackedPath()
    {
        var rootLaunch = CleanRunner("src/Ensemble.E0.Harness/root-local.cs");
        var subdirectoryLaunch = CleanRunner("src/Ensemble.E0.Harness/root-local.cs");

        Assert.Throws<E0AHarnessException>(() =>
            E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, rootLaunch));
        Assert.Throws<E0AHarnessException>(() =>
            E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, subdirectoryLaunch));

        CollectionAssert.AreEqual(rootLaunch.Calls[^1], subdirectoryLaunch.Calls[^1]);
        Assert.AreEqual("-C", subdirectoryLaunch.Calls[^1][0]);
        Assert.AreEqual(RepositoryRoot, subdirectoryLaunch.Calls[^1][1]);
        Assert.AreEqual("--full-name", subdirectoryLaunch.Calls[^1][^1]);
    }

    private static RecordingGitRunner CleanRunner(string untracked) =>
        new(
            new E0AGitCommandResult(0, RepositoryRoot),
            new E0AGitCommandResult(0, ExpectedCommit),
            new E0AGitCommandResult(0, string.Empty),
            new E0AGitCommandResult(0, string.Empty),
            new E0AGitCommandResult(0, untracked));

    private sealed class RecordingGitRunner : IE0AGitCommandRunner
    {
        private readonly Queue<E0AGitCommandResult> _results;

        internal RecordingGitRunner(params E0AGitCommandResult[] results) =>
            _results = new Queue<E0AGitCommandResult>(results);

        internal List<string[]> Calls { get; } = new();

        public E0AGitCommandResult Run(params string[] arguments)
        {
            Calls.Add(arguments);
            return _results.Dequeue();
        }
    }
}
