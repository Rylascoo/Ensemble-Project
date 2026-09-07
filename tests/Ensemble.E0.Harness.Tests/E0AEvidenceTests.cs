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
    public void Manifest_FreezesArchitectureChecklistFixtureAuthorityAndContainsNoCredentialField()
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
            Assert.AreEqual(E0AEvidenceContracts.FrozenBlueprintVersion, manifest.GetProperty("frozenBlueprintVersion").GetString());
            Assert.AreEqual(E0AEvidenceContracts.GeminiReferenceAmendment, manifest.GetProperty("referenceEnvelopeBlueprint").GetString());
            Assert.AreEqual(E0AEvidenceContracts.GeminiApprovedAmendmentCommit, manifest.GetProperty("approvedBlueprintCommit").GetString());
            Assert.AreEqual("E0A-GEMINI-NORMATIVE-2026-09-06:CREATIVE-NONE", manifest.GetProperty("referenceConfigurationIdentity").GetString());
            Assert.AreEqual(E0AEvidenceContracts.HardGateChecklistVersion, manifest.GetProperty("hardGateChecklistVersion").GetString());

            var expectedHardGates = new[]
            {
                "A character receives inaccessible secret information.",
                "A context packet contains prohibited information even if the performer appears not to use it.",
                "A model-created claim silently becomes objective world truth.",
                "A possibility silently becomes fact.",
                "Creator-locked canon or Constitution changes.",
                "A technical provider failure, refusal, timeout, or retry becomes fictional action.",
                "An unaccepted or cancelled partial performance enters Production history.",
                "Accepted history changes retroactively or without an explicit causal event.",
                "A committed consequence lacks a traceable accepted performance or authorized creator/world cause.",
                "Performance commits without its approved consequences, or consequences commit without the accepted Performance.",
                "The State Interpreter directly mutates authority.",
                "A deterministic cost, cancellation, eligibility, or access rule is delegated to an LLM."
            };
            var actualHardGates = manifest.GetProperty("hardGateChecklist")
                .EnumerateArray()
                .Select(x => x.GetString())
                .ToArray();
            Assert.AreEqual(12, actualHardGates.Length);
            CollectionAssert.AreEqual(expectedHardGates, actualHardGates);

            Assert.AreEqual(state.OriginFixtureVersion.Value, manifest.GetProperty("fixtureVersion").GetString());
            Assert.AreEqual(E0ATestSupport.TestExecutableCommit, manifest.GetProperty("executableCommit").GetString());
            Assert.AreEqual(3, manifest.GetProperty("roles").GetArrayLength());
            Assert.AreEqual(1, manifest.GetProperty("attemptsPerRoleInvocation").GetInt32());
            Assert.IsTrue(manifest.GetProperty("host").TryGetProperty("processArchitecture", out _));

            var authority = manifest.GetProperty("stateAuthority");
            CollectionAssert.AreEqual(
                new[]
                {
                    "UnresolvedProposition",
                    "CharacterBelief",
                    "CharacterSuspicion",
                    "CharacterGoal",
                    "CharacterCircumstance",
                    "CharacterClaim",
                    "Pressure"
                },
                authority.GetProperty("autoApproveDomains").EnumerateArray().Select(x => x.GetString()).ToArray());
            Assert.AreEqual(
                E0AEvidenceContracts.MandatoryReviewResolution,
                authority.GetProperty("mandatoryReviewResolution").GetString());

            Assert.IsFalse(text.Contains("OPENAI_API_KEY", StringComparison.Ordinal));
            Assert.IsFalse(text.Contains("GEMINI_API_KEY", StringComparison.Ordinal));
            Assert.IsFalse(text.Contains("Bearer ", StringComparison.Ordinal));
            Assert.IsFalse(text.Contains("x-goog-api-key", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void EvidenceStore_RejectsMalformedExecutableCommitBeforeCreatingRunDirectory()
    {
        var root = E0ATestSupport.TempRunRoot();
        var state = E0ATestSupport.Genesis();
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());

        Assert.Throws<E0AHarnessException>(() =>
            new E0AFileEvidenceStore(
                root,
                RunId.From("E0A-BAD-COMMIT"),
                envelope,
                state.OriginFixtureId.Value,
                state.OriginFixtureVersion.Value,
                state.OriginFixtureHash,
                "not-a-commit",
                state.RosterCharacterIds));
        Assert.IsFalse(Directory.Exists(root));
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

            using var request = JsonDocument.Parse(File.ReadAllBytes(
                Directory.GetFiles(root, "request.json", SearchOption.AllDirectories).Single()));
            using var terminal = JsonDocument.Parse(File.ReadAllBytes(
                Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Single()));
            Assert.AreEqual(
                request.RootElement.GetProperty("preparedIdentityHash").GetString(),
                terminal.RootElement.GetProperty("preparedIdentityHash").GetString());
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
                    E0AEvidenceContracts.HardGateChecklistVersion,
                    "REVIEWER",
                    "METHOD",
                    true,
                    Array.Empty<string>())));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void EvaluationSeal_RejectsChecklistChosenAfterRun()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-CHECKLIST");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var store = E0ATestSupport.Evidence(root, runId, envelope, state);
            store.SealRuntime(
                "Synthetic",
                0,
                0m,
                state.StateHash.Value,
                state.CurrentOpportunityCharacterId!.Value);

            Assert.Throws<E0AHarnessException>(() =>
                store.SealEvaluation(new E0AHardGateEvaluation(
                    "post-hoc-checklist",
                    "REVIEWER",
                    "METHOD",
                    true,
                    Array.Empty<string>())));
            Assert.IsFalse(File.Exists(Path.Combine(root, "evaluation", "hard-gates.json")));
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
            Assert.IsFalse(blind.Contains(E0AGeminiProviderPolicy.Model, StringComparison.Ordinal));
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
