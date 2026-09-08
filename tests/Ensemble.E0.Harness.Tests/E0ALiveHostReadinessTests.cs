using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0ALiveHostReadinessTests
{
    private const string ExpectedCommit = "0123456789abcdef0123456789abcdef01234567";

    [TestMethod]
    public void GeminiPricingPolicy_FreezesSourceFreshnessAndConservativeShadowRates()
    {
        object[] expected =
        {
            "https://ai.google.dev/gemini-api/docs/pricing",
            "2026-09-07",
            "2026-09-14",
            0.30m,
            0.03m,
            2.50m,
            0.30m,
            0.30m,
            2.50m
        };
        object[] actual =
        {
            E0AGeminiPricingPolicy.SourceUri,
            E0AGeminiPricingPolicy.VerifiedOn,
            E0AGeminiPricingPolicy.SnapshotValidThrough,
            E0AGeminiPricingPolicy.PublishedPaidInputUsdPerMillionTokens,
            E0AGeminiPricingPolicy.PublishedPaidCachedInputUsdPerMillionTokens,
            E0AGeminiPricingPolicy.PublishedPaidOutputUsdPerMillionTokens,
            E0AReferenceRunHost.ConservativePricing.InputUsdPerMillionTokens,
            E0AReferenceRunHost.ConservativePricing.CachedInputUsdPerMillionTokens,
            E0AReferenceRunHost.ConservativePricing.OutputUsdPerMillionTokens
        };

        CollectionAssert.AreEqual(expected, actual);
        E0AReferenceRunHost.ConservativePricing.Validate();
    }

    [TestMethod]
    public void GeminiPricingPolicy_FailsClosedAfterShortExperimentalSnapshot()
    {
        E0AGeminiPricingPolicy.RequireNonStaleSnapshot(new DateTimeOffset(2026, 9, 14, 23, 59, 59, TimeSpan.Zero));
        Assert.Throws<E0AHarnessException>(() =>
            E0AGeminiPricingPolicy.RequireNonStaleSnapshot(new DateTimeOffset(2026, 9, 15, 0, 0, 0, TimeSpan.Zero)));
    }

    [TestMethod]
    public void GeminiSpendPreflight_BlocksBeyondModelInputLimitBeforeInference()
    {
        var ledger = new E0ASpendLedger(
            E0AReferenceRunHost.ConservativePricing,
            E0AGeminiProviderPolicy.ModelInputTokenLimit);
        var boundary = ledger.Reserve(E0AGeminiProviderPolicy.ModelInputTokenLimit, 1);
        Assert.AreEqual(E0AGeminiProviderPolicy.ModelInputTokenLimit, boundary.InputTokens);
        ledger.Release(boundary);

        Assert.Throws<E0ABudgetExceededException>(() =>
            ledger.Reserve(E0AGeminiProviderPolicy.ModelInputTokenLimit + 1, 1));
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void LiveHost_HistoricalHelperRemainsGemini25FlashAnchor()
    {
        var none = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");

        Assert.AreEqual("CREATIVE-NONE", none.Variant);
        Assert.AreEqual(E0AGeminiModelCatalog.Flash25NoneId, none.ProviderProfileId);
        Assert.AreEqual(E0AGeminiProviderPolicy.Provider, none.Performer.Provider);
        Assert.AreEqual(E0AGeminiProviderPolicy.Model, none.Performer.Model);
        Assert.AreEqual(E0AReasoningLevel.None, none.Performer.Reasoning);
        Assert.AreEqual(E0AReasoningLevel.High, none.Integrity.Reasoning);
        Assert.AreEqual(E0AReasoningLevel.None, none.Interpreter.Reasoning);
        Assert.AreEqual(0, E0AGeminiProviderPolicy.ThinkingBudgetTokens(none.Performer));
        Assert.AreEqual(E0AGeminiProviderPolicy.IntegrityThinkingBudgetTokens, E0AGeminiProviderPolicy.ThinkingBudgetTokens(none.Integrity));
        Assert.AreEqual(E0AGeminiProviderPolicy.IntegrityCandidateMaxOutputTokens, none.Integrity.MaxOutputTokens);
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope("CREATIVE-LOW"));
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope("CREATIVE-MEDIUM"));
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope("CREATIVE-HIGH"));
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope("CREATIVE-XHIGH"));
    }

    [TestMethod]
    public void LiveHost_ExposesOnlyApprovedArmProfilePairings()
    {
        var lite25 = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-NONE",
            E0AGeminiModelCatalog.FlashLite25NoneId);
        var lite35 = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        var lite31 = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite31MinimalId);

        Assert.AreEqual("gemini-2.5-flash-lite", lite25.Performer.Model);
        Assert.AreEqual(10, lite25.ModelProfile.RequestsPerMinute);
        Assert.AreEqual(250_000L, lite25.ModelProfile.InputTokensPerMinute);
        Assert.AreEqual(E0AReasoningLevel.None, lite25.Performer.Reasoning);

        Assert.AreEqual("gemini-3.5-flash-lite", lite35.Performer.Model);
        Assert.AreEqual(15, lite35.ModelProfile.RequestsPerMinute);
        Assert.AreEqual(250_000L, lite35.ModelProfile.InputTokensPerMinute);
        Assert.AreEqual(E0AReasoningLevel.Minimal, lite35.Performer.Reasoning);
        Assert.AreEqual("minimal", E0AGeminiProviderPolicy.ThinkingLevel(lite35.Performer));
        Assert.AreEqual("high", E0AGeminiProviderPolicy.ThinkingLevel(lite35.Integrity));

        Assert.AreEqual("gemini-3.1-flash-lite", lite31.Performer.Model);
        Assert.AreEqual(15, lite31.ModelProfile.RequestsPerMinute);
        Assert.AreEqual(250_000L, lite31.ModelProfile.InputTokensPerMinute);
        Assert.AreEqual(E0AReasoningLevel.Minimal, lite31.Performer.Reasoning);
        Assert.AreEqual("minimal", E0AGeminiProviderPolicy.ThinkingLevel(lite31.Performer));
        Assert.AreEqual("high", E0AGeminiProviderPolicy.ThinkingLevel(lite31.Integrity));

        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-NONE",
            E0AGeminiModelCatalog.Flash25NoneId));
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-NONE",
            E0AGeminiModelCatalog.FlashLite35MinimalId));
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-NONE",
            E0AGeminiModelCatalog.FlashLite31MinimalId));
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite25NoneId));
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-NONE",
            "UNAPPROVED-MODEL"));
    }

    [TestMethod]
    public void GeminiIntegrityBudget_ReservesAgainstPublishedModelOutputLimit()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");

        Assert.AreEqual(
            E0ARunEnvelope.RoleMaxOutputTokens,
            E0AProviderBudgetPolicy.ReservationOutputTokens(envelope.Performer));
        Assert.AreEqual(
            E0AGeminiProviderPolicy.ModelOutputTokenLimit,
            E0AProviderBudgetPolicy.ReservationOutputTokens(envelope.Integrity));
        Assert.AreEqual(
            E0ARunEnvelope.RoleMaxOutputTokens,
            E0AProviderBudgetPolicy.ReservationOutputTokens(envelope.Interpreter));
    }

    [TestMethod]
    public void Gemini35Budget_ReservesEveryRoleAgainstPublishedModelOutputLimit()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);

        Assert.AreEqual(envelope.ModelProfile.ModelOutputTokenLimit, E0AProviderBudgetPolicy.ReservationOutputTokens(envelope.Performer));
        Assert.AreEqual(envelope.ModelProfile.ModelOutputTokenLimit, E0AProviderBudgetPolicy.ReservationOutputTokens(envelope.Integrity));
        Assert.AreEqual(envelope.ModelProfile.ModelOutputTokenLimit, E0AProviderBudgetPolicy.ReservationOutputTokens(envelope.Interpreter));
    }

    [TestMethod]
    public void GeminiUsagePolicy_FailsClosedOnThinkingCacheAndGeneratedOverrun()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");

        Assert.IsNull(E0AProviderUsagePolicy.Violation(envelope.Performer, new E0AUsage(100, 100, 0, 0, 0)));
        Assert.AreEqual(
            "gemini-thinking-not-disabled",
            E0AProviderUsagePolicy.Violation(envelope.Performer, new E0AUsage(100, 101, 0, 1, 0)));
        Assert.AreEqual(
            "gemini-implicit-cache-hit",
            E0AProviderUsagePolicy.Violation(envelope.Performer, new E0AUsage(100, 100, 1, 0, 0)));
        Assert.IsNull(E0AProviderUsagePolicy.Violation(envelope.Integrity, new E0AUsage(100, 4096, 0, 3584, 0)));
        Assert.AreEqual(
            "gemini-generated-token-overrun",
            E0AProviderUsagePolicy.Violation(envelope.Integrity, new E0AUsage(100, 4097, 0, 3584, 0)));
    }

    [TestMethod]
    public void Gemini35MinimalUsage_AllowsObservedThinkingOnlyWithinCombinedCeiling()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);

        Assert.IsNull(E0AProviderUsagePolicy.Violation(
            envelope.Performer,
            new E0AUsage(100, 110, 0, 10, 0)));
        Assert.AreEqual(
            "gemini-generated-token-overrun",
            E0AProviderUsagePolicy.Violation(
                envelope.Performer,
                new E0AUsage(100, 4097, 0, 10, 0)));
    }

    [TestMethod]
    public void CheckoutGuard_AcceptsExactCleanAuthorityWithNonMaterialUntrackedFile()
    {
        var runner = CleanGitRunner("patch0012-local-edit.txt");

        E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, runner);

        Assert.AreEqual(5, runner.Calls.Count);
    }

    [TestMethod]
    public void CheckoutGuard_RejectsWrongHead()
    {
        var runner = new ScriptedGitRunner(
            new E0AGitCommandResult(0, "true"),
            new E0AGitCommandResult(0, new string('a', 40)));

        Assert.Throws<E0AHarnessException>(() => E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, runner));
        Assert.AreEqual(2, runner.Calls.Count);
    }

    [TestMethod]
    public void CheckoutGuard_RejectsTrackedOrStagedChanges()
    {
        var tracked = new ScriptedGitRunner(
            new E0AGitCommandResult(0, "true"),
            new E0AGitCommandResult(0, ExpectedCommit),
            new E0AGitCommandResult(1, ""));
        Assert.Throws<E0AHarnessException>(() => E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, tracked));

        var staged = new ScriptedGitRunner(
            new E0AGitCommandResult(0, "true"),
            new E0AGitCommandResult(0, ExpectedCommit),
            new E0AGitCommandResult(0, ""),
            new E0AGitCommandResult(1, ""));
        Assert.Throws<E0AHarnessException>(() => E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, staged));
    }

    [TestMethod]
    public void CheckoutGuard_RejectsMaterialUntrackedFiles()
    {
        var runner = CleanGitRunner("src/Ensemble.E0.Harness/local.cs\nnotes.txt");

        Assert.Throws<E0AHarnessException>(() => E0ARepositoryCheckoutGuard.Validate(ExpectedCommit, runner));
    }

    [TestMethod]
    public void ExistingRuntime_CanBeEvaluatedExactlyOnceAfterSeal()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var state = E0ATestSupport.Genesis();
            var store = E0ATestSupport.Evidence(
                root,
                RunId.From("E0A-POST-RUN-EVAL"),
                E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing()),
                state);
            store.SealRuntime(
                "Synthetic",
                0,
                0m,
                E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions,
                false,
                state.StateHash.Value,
                state.CurrentOpportunityCharacterId!.Value);

            var evaluation = new E0AHardGateEvaluation(
                E0AEvidenceContracts.HardGateChecklistVersion,
                "DIRECTOR",
                "MANUAL-BLIND-HARD-GATE-REVIEW",
                true,
                Array.Empty<string>());
            E0AExistingEvidenceEvaluationSealer.Seal(root, evaluation);

            Assert.IsTrue(File.Exists(Path.Combine(root, "evaluation", "hard-gates.json")));
            Assert.IsTrue(File.Exists(Path.Combine(root, "evaluation.final.json")));
            Assert.Throws<E0AHarnessException>(() => E0AExistingEvidenceEvaluationSealer.Seal(root, evaluation));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void ExistingRuntimeEvaluation_RejectsPostSealTampering()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var state = E0ATestSupport.Genesis();
            var store = E0ATestSupport.Evidence(
                root,
                RunId.From("E0A-POST-RUN-TAMPER"),
                E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing()),
                state);
            store.SealRuntime(
                "Synthetic",
                0,
                0m,
                E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions,
                false,
                state.StateHash.Value,
                state.CurrentOpportunityCharacterId!.Value);
            File.AppendAllText(Path.Combine(root, "manifest.json"), " ");

            Assert.Throws<E0AHarnessException>(() => E0AExistingEvidenceEvaluationSealer.Seal(
                root,
                new E0AHardGateEvaluation(
                    E0AEvidenceContracts.HardGateChecklistVersion,
                    "DIRECTOR",
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
    public void HostEvaluation_RejectsInconsistentPassFindings()
    {
        var root = E0ATestSupport.TempRunRoot();
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.SealEvaluation(
            new[] { root, "DIRECTOR", "METHOD", "pass", "unexpected finding" }));
        Assert.IsFalse(Directory.Exists(root));
    }

    private static ScriptedGitRunner CleanGitRunner(string untracked = "") =>
        new(
            new E0AGitCommandResult(0, "true"),
            new E0AGitCommandResult(0, ExpectedCommit),
            new E0AGitCommandResult(0, ""),
            new E0AGitCommandResult(0, ""),
            new E0AGitCommandResult(0, untracked));

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private sealed class ScriptedGitRunner : IE0AGitCommandRunner
    {
        private readonly Queue<E0AGitCommandResult> _results;

        internal ScriptedGitRunner(params E0AGitCommandResult[] results) => _results = new Queue<E0AGitCommandResult>(results);
        internal List<string[]> Calls { get; } = new();

        public E0AGitCommandResult Run(params string[] arguments)
        {
            Calls.Add(arguments);
            return _results.Dequeue();
        }
    }
}
