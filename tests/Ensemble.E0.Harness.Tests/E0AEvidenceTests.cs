using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0AEvidenceTests
{
    [TestMethod]
    public void Manifest_FreezesPricingProfilesHostAndContainsNoCredentialField()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-MANIFEST");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            _ = E0ATestSupport.Evidence(root, runId, envelope, state);

            var text = File.ReadAllText(Path.Combine(root, "manifest.json"));
            using var document = JsonDocument.Parse(text);
            var manifest = document.RootElement;
            Assert.AreEqual(1m, manifest.GetProperty("pricing").GetProperty("inputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(0.25m, manifest.GetProperty("pricing").GetProperty("cachedInputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(2m, manifest.GetProperty("pricing").GetProperty("outputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(3, manifest.GetProperty("roles").GetArrayLength());
            Assert.AreEqual(1, manifest.GetProperty("attemptsPerRoleInvocation").GetInt32());
            Assert.IsTrue(manifest.GetProperty("host").TryGetProperty("processArchitecture", out _));
            Assert.IsFalse(text.Contains("OPENAI_API_KEY", StringComparison.Ordinal));
            Assert.IsFalse(text.Contains("Bearer ", StringComparison.Ordinal));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void RequestAndTerminalEvidence_SharePreparedIdentityHash()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-RECEIPT-EVIDENCE");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var store = E0ATestSupport.Evidence(root, runId, envelope, state);
            var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
            var attempt = E0ARequestBuilder.Performer(runId, 1, envelope.Performer, context);
            var receipt = E0ATestSupport.Success(attempt, E0ATestSupport.PerformerOutput());
            store.RecordPrepared(attempt);
            store.RecordReceipt(attempt, receipt);

            var request = JsonDocument.Parse(File.ReadAllBytes(
                Directory.GetFiles(root, "request.json", SearchOption.AllDirectories).Single())).RootElement;
            var terminal = JsonDocument.Parse(File.ReadAllBytes(
                Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Single())).RootElement;
            Assert.AreEqual(
                request.GetProperty("preparedIdentityHash").GetString(),
                terminal.GetProperty("preparedIdentityHash").GetString());
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void RuntimeSeal_RecordsFinalSynchronizedCheckpointAndDetectsTampering()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-SEAL");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var store = E0ATestSupport.Evidence(root, runId, envelope, state);
            var opportunity = state.CurrentOpportunityCharacterId!.Value;
            store.SealRuntime("Synthetic", 0, 0m, state.StateHash.Value, opportunity);

            using var final = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "run.final.json")));
            Assert.AreEqual(state.StateHash.Value, final.RootElement.GetProperty("finalStateHash").GetString());
            Assert.AreEqual(opportunity.Value, final.RootElement.GetProperty("finalOpportunityCharacterId").GetString());

            File.AppendAllText(Path.Combine(root, "manifest.json"), " ");
            Assert.Throws<E0AHarnessException>(() =>
                store.SealEvaluation(new E0AHardGateEvaluation(
                    "CHECKLIST-V1", "REVIEWER", "METHOD", true, Array.Empty<string>())));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void BlindTranscript_ContainsOnlyNeutralSpeakerTurnAndAcceptedVisibleText()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-BLIND");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var store = E0ATestSupport.Evidence(root, runId, envelope, state);
            var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
            var candidate = E0ATestSupport.Candidate(context, "Only accepted visible text.");
            store.RecordAcceptedPerformance(1, candidate.SubjectCharacterId, candidate);
            store.SealRuntime(
                "Synthetic",
                1,
                0m,
                state.StateHash.Value,
                state.CurrentOpportunityCharacterId!.Value);

            var blind = File.ReadAllText(Path.Combine(root, "blind", "transcript.json"));
            Assert.IsTrue(blind.Contains("SPEAKER-", StringComparison.Ordinal));
            Assert.IsTrue(blind.Contains("Only accepted visible text.", StringComparison.Ordinal));
            Assert.IsFalse(blind.Contains(candidate.SubjectCharacterId.Value, StringComparison.Ordinal));
            Assert.IsFalse(blind.Contains(runId.Value, StringComparison.Ordinal));
            Assert.IsFalse(blind.Contains("gpt-5.6-sol", StringComparison.Ordinal));
            Assert.IsFalse(blind.Contains("reasoning", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            Delete(root);
        }
    }

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
