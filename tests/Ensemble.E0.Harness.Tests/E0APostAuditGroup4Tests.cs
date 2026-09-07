using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0APostAuditGroup4Tests
{
    [TestMethod]
    public async Task ConcurrentRunCreation_ClaimsOneRootAndCannotOverwriteManifest()
    {
        var root = E0ATestSupport.TempRunRoot();
        var runId = RunId.From("E0A-G4-CONCURRENT-RUN");
        var state = E0ATestSupport.Genesis();
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        try
        {
            var results = await Task.WhenAll(
                Task.Run(() => TryCreate(root, runId, envelope, state)),
                Task.Run(() => TryCreate(root, runId, envelope, state)));

            Assert.AreEqual(1, results.Count(x => x));
            Assert.AreEqual(1, results.Count(x => !x));
            Assert.IsTrue(File.Exists(Path.Combine(root, "manifest.json")));
            using var manifest = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "manifest.json")));
            Assert.AreEqual(runId.Value, manifest.RootElement.GetProperty("runId").GetString());
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void RunIdReuseAcrossFreshRootsInOneNamespace_IsRejected()
    {
        var firstRoot = E0ATestSupport.TempRunRoot();
        var secondRoot = E0ATestSupport.TempRunRoot();
        var runId = RunId.From("E0A-G4-RUNID-REUSE");
        var state = E0ATestSupport.Genesis();
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        try
        {
            _ = E0ATestSupport.Evidence(firstRoot, runId, envelope, state);

            Assert.Throws<E0AHarnessException>(() =>
                E0ATestSupport.Evidence(secondRoot, runId, envelope, state));
            Assert.IsFalse(Directory.Exists(secondRoot));
        }
        finally
        {
            Delete(firstRoot);
            Delete(secondRoot);
        }
    }

    [TestMethod]
    public async Task ConcurrentWriteOnceArtifactCreation_AllowsExactlyOnePublisher()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-G4-WRITE-ONCE");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var store = E0ATestSupport.Evidence(root, runId, envelope, state);
            var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
            var attempt = E0ARequestBuilder.Performer(runId, 1, envelope.Performer, context);

            var results = await Task.WhenAll(
                Task.Run(() => TryRecordPrepared(store, attempt)),
                Task.Run(() => TryRecordPrepared(store, attempt)));

            Assert.AreEqual(1, results.Count(x => x));
            Assert.AreEqual(1, results.Count(x => !x));
            var requestPath = Directory.GetFiles(root, "request.json", SearchOption.AllDirectories).Single();
            using var request = JsonDocument.Parse(File.ReadAllBytes(requestPath));
            Assert.AreEqual(attempt.AttemptId, request.RootElement.GetProperty("attemptId").GetString());
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void AlteredRunFinalSummaryCannotBeEndorsedWithOriginalRuntimeRoot()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var store = SealedStore(root, "E0A-G4-SUMMARY-TAMPER");
            var finalPath = Path.Combine(root, "run.final.json");
            var text = File.ReadAllText(finalPath);
            File.WriteAllText(
                finalPath,
                text.Replace("\"terminalStatus\": \"Synthetic\"", "\"terminalStatus\": \"Tampered\"", StringComparison.Ordinal));

            Assert.Throws<E0AHarnessException>(() =>
                E0AExistingEvidenceEvaluationSealer.Seal(root, PassingEvaluation()));
            AssertNoEvaluationPublication(root);

            // The in-process/legacy entry point must use the same verification authority.
            Assert.Throws<E0AHarnessException>(() => store.SealEvaluation(PassingEvaluation()));
            AssertNoEvaluationPublication(root);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void RuntimeArtifactTamper_FailsBeforeAnyEvaluationWrite()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            _ = SealedStore(root, "E0A-G4-RUNTIME-TAMPER");
            File.AppendAllText(Path.Combine(root, "manifest.json"), " ");

            Assert.Throws<E0AHarnessException>(() =>
                E0AExistingEvidenceEvaluationSealer.Seal(root, PassingEvaluation()));
            AssertNoEvaluationPublication(root);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void MalformedEvaluationUnicode_FailsBeforeAnyEvaluationWrite()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            _ = SealedStore(root, "E0A-G4-EVAL-UNICODE");
            var malformed = new E0AHardGateEvaluation(
                E0AEvidenceContracts.HardGateChecklistVersion,
                "REVIEWER\uD800",
                "METHOD",
                true,
                Array.Empty<string>());

            Assert.Throws<E0AHarnessException>(() =>
                E0AExistingEvidenceEvaluationSealer.Seal(root, malformed));
            AssertNoEvaluationPublication(root);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void WronglyTypedRuntimeSealContract_FailsThroughEvaluationBoundaryBeforeWrite()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            _ = SealedStore(root, "E0A-G4-EVAL-TYPE");
            var finalPath = Path.Combine(root, "run.final.json");
            var text = File.ReadAllText(finalPath);
            File.WriteAllText(
                finalPath,
                text.Replace(
                    "\"contract\": \"ensemble.e0a.runtime-seal.v1\"",
                    "\"contract\": 7",
                    StringComparison.Ordinal));

            Assert.Throws<E0AHarnessException>(() =>
                E0AExistingEvidenceEvaluationSealer.Seal(root, PassingEvaluation()));
            AssertNoEvaluationPublication(root);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task ConcurrentEvaluationPublication_ProducesOneAuthoritativeSeal()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            _ = SealedStore(root, "E0A-G4-EVAL-CONCURRENT");
            var evaluation = PassingEvaluation();

            var results = await Task.WhenAll(
                Task.Run(() => TrySealEvaluation(root, evaluation)),
                Task.Run(() => TrySealEvaluation(root, evaluation)));

            Assert.AreEqual(1, results.Count(x => x));
            Assert.AreEqual(1, results.Count(x => !x));
            var hardGatesPath = Path.Combine(root, "evaluation", "hard-gates.json");
            var finalPath = Path.Combine(root, "evaluation.final.json");
            Assert.IsTrue(File.Exists(hardGatesPath));
            Assert.IsTrue(File.Exists(finalPath));

            using var final = JsonDocument.Parse(File.ReadAllBytes(finalPath));
            Assert.AreEqual(
                PreparedRoleAttempt.LowerSha256(File.ReadAllBytes(hardGatesPath)),
                final.RootElement.GetProperty("hardGatesSha256").GetString());
            using var runFinal = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "run.final.json")));
            Assert.AreEqual(
                runFinal.RootElement.GetProperty("runtimeSealIdentity").GetString(),
                final.RootElement.GetProperty("runtimeSealIdentity").GetString());
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void InProcessEvaluationPath_BindsSameRuntimeSealIdentity()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var store = SealedStore(root, "E0A-G4-INPROCESS-EVAL");
            store.SealEvaluation(PassingEvaluation());

            using var runtime = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "run.final.json")));
            using var evaluation = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "evaluation.final.json")));
            Assert.AreEqual(
                runtime.RootElement.GetProperty("runtimeSealIdentity").GetString(),
                evaluation.RootElement.GetProperty("runtimeSealIdentity").GetString());
        }
        finally
        {
            Delete(root);
        }
    }

    private static E0AFileEvidenceStore SealedStore(string root, string runIdValue)
    {
        var state = E0ATestSupport.Genesis();
        var store = E0ATestSupport.Evidence(
            root,
            RunId.From(runIdValue),
            E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing()),
            state);
        store.SealRuntime(
            "Synthetic",
            0,
            0m,
            E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions,
            hasUnknownProviderUsage: false,
            state.StateHash.Value,
            state.CurrentOpportunityCharacterId!.Value);
        return store;
    }

    private static E0AHardGateEvaluation PassingEvaluation() =>
        new(
            E0AEvidenceContracts.HardGateChecklistVersion,
            "DIRECTOR",
            "MANUAL-BLIND-HARD-GATE-REVIEW",
            true,
            Array.Empty<string>());

    private static bool TryCreate(
        string root,
        RunId runId,
        E0ARunEnvelope envelope,
        Ensemble.E0.Core.Production.ProductionState state)
    {
        try
        {
            _ = E0ATestSupport.Evidence(root, runId, envelope, state);
            return true;
        }
        catch (E0AHarnessException)
        {
            return false;
        }
    }

    private static bool TryRecordPrepared(E0AFileEvidenceStore store, PreparedRoleAttempt attempt)
    {
        try
        {
            store.RecordPrepared(attempt);
            return true;
        }
        catch (E0AHarnessException)
        {
            return false;
        }
    }

    private static bool TrySealEvaluation(string root, E0AHardGateEvaluation evaluation)
    {
        try
        {
            E0AExistingEvidenceEvaluationSealer.Seal(root, evaluation);
            return true;
        }
        catch (E0AHarnessException)
        {
            return false;
        }
    }

    private static void AssertNoEvaluationPublication(string root)
    {
        Assert.IsFalse(File.Exists(Path.Combine(root, "evaluation", "hard-gates.json")));
        Assert.IsFalse(File.Exists(Path.Combine(root, "evaluation.final.json")));
    }

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
