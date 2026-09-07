using System.Net;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.OpenAI;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0ALiveHostReadinessTests
{
    private const string ExpectedCommit = "0123456789abcdef0123456789abcdef01234567";

    [TestMethod]
    public void PreparedRequest_DisablesImplicitPromptCaching()
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-CACHE-MODE"), 1, envelope.Performer, context);

        using var body = JsonDocument.Parse(attempt.RequestBody);
        var options = body.RootElement.GetProperty("prompt_cache_options");
        Assert.AreEqual(E0AProviderTransportPolicy.PromptCacheMode, options.GetProperty("mode").GetString());
    }

    [TestMethod]
    public async Task InputTokenPreflight_DoesNotForwardPromptCacheControl()
    {
        var handler = new StaticResponseHandler("{\"input_tokens\":7}");
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-CACHE-PREFLIGHT"), 1, envelope.Performer, context);

        var count = await port.CountInputTokensAsync(attempt, CancellationToken.None);

        Assert.AreEqual(7L, count);
        using var body = JsonDocument.Parse(handler.LastBody!);
        Assert.IsFalse(body.RootElement.TryGetProperty("prompt_cache_options", out _));
    }

    [TestMethod]
    public async Task CacheWriteUsage_IsInputDetailAndOverlapIsPreservedForConservativeReconciliation()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var responseJson = JsonSerializer.Serialize(new
        {
            status = "completed",
            id = "resp-cache-write",
            model = "gpt-5.6-sol",
            output_text = output,
            usage = new
            {
                input_tokens = 10,
                output_tokens = 5,
                input_tokens_details = new { cached_tokens = 7, cache_write_tokens = 4 },
                output_tokens_details = new { reasoning_tokens = 3 }
            }
        });
        using var http = new HttpClient(new StaticResponseHandler(responseJson));
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-CACHE-WRITE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        Assert.AreEqual(10L, receipt.Usage!.InputTokens);
        Assert.AreEqual(7L, receipt.Usage.CachedInputTokens);
        Assert.AreEqual(4L, receipt.Usage.CacheWriteTokens);
        CollectionAssert.AreEqual(E0ATestSupport.IntegrityOutput(), receipt.StructuredOutput!);
    }

    [TestMethod]
    public void CacheWriteUsage_ReconcilesConservativelyThenMarksReservationMismatch()
    {
        var ledger = new E0ASpendLedger(E0AReferenceRunHost.ConservativePricing);
        var reservation = ledger.Reserve(10, E0ARunEnvelope.RoleMaxOutputTokens);

        var reconciliation = ledger.Reconcile(
            reservation,
            new E0AUsage(10, 5, 7, 3, 4));

        Assert.IsTrue(reconciliation.ReservationExceeded);
        Assert.AreEqual(0.00015m, reconciliation.EstimatedUsd);
        Assert.AreEqual(reconciliation.EstimatedUsd, ledger.EstimatedCommittedUsd);
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void PricingPolicy_FreezesSourcePromotionAndConservativeRates()
    {
        object[] expected =
        {
            "https://developers.openai.com/api/docs/models/gpt-5.6-sol",
            "2026-09-06",
            "2026-11-21",
            4.00m,
            0.40m,
            20.00m,
            1.25m,
            5.00m,
            0.40m,
            20.00m
        };
        object[] actual =
        {
            E0APricingPolicy.SourceUri,
            E0APricingPolicy.VerifiedOn,
            E0APricingPolicy.PromotionalPricingGuaranteedThrough,
            E0APricingPolicy.PublishedInputUsdPerMillionTokens,
            E0APricingPolicy.PublishedCachedInputUsdPerMillionTokens,
            E0APricingPolicy.PublishedOutputUsdPerMillionTokens,
            E0APricingPolicy.CacheWriteMultiplier,
            E0AReferenceRunHost.ConservativePricing.InputUsdPerMillionTokens,
            E0AReferenceRunHost.ConservativePricing.CachedInputUsdPerMillionTokens,
            E0AReferenceRunHost.ConservativePricing.OutputUsdPerMillionTokens
        };

        CollectionAssert.AreEqual(expected, actual);
        E0AReferenceRunHost.ConservativePricing.Validate();
    }

    [TestMethod]
    public void PricingPolicy_FailsClosedAfterPublishedPromotionalGuarantee()
    {
        E0APricingPolicy.RequireNonStaleSnapshot(new DateTimeOffset(2026, 11, 21, 23, 59, 59, TimeSpan.Zero));
        Assert.Throws<E0AHarnessException>(() =>
            E0APricingPolicy.RequireNonStaleSnapshot(new DateTimeOffset(2026, 11, 22, 0, 0, 0, TimeSpan.Zero)));
    }

    [TestMethod]
    public void SpendPreflight_BlocksLongContextPricingTierBeforeInference()
    {
        var ledger = new E0ASpendLedger(E0AReferenceRunHost.ConservativePricing);
        var boundary = ledger.Reserve(E0APricingPolicy.StandardTierMaxInputTokens, 1);
        Assert.AreEqual(E0APricingPolicy.StandardTierMaxInputTokens, boundary.InputTokens);
        ledger.Release(boundary);

        Assert.Throws<E0ABudgetExceededException>(() =>
            ledger.Reserve(E0APricingPolicy.StandardTierMaxInputTokens + 1, 1));
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void LiveHost_ExposesExactlyTheFourApprovedPhaseBVariants()
    {
        var none = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var low = E0AReferenceRunHost.CreateEnvelope("CREATIVE-LOW");
        var medium = E0AReferenceRunHost.CreateEnvelope("CREATIVE-MEDIUM");
        var high = E0AReferenceRunHost.CreateEnvelope("CREATIVE-HIGH");

        Assert.AreEqual(E0AReasoningLevel.None, none.Performer.Reasoning);
        Assert.AreEqual(E0AReasoningLevel.Low, low.Performer.Reasoning);
        Assert.AreEqual(E0AReasoningLevel.Medium, medium.Performer.Reasoning);
        Assert.AreEqual(E0AReasoningLevel.High, high.Performer.Reasoning);
        foreach (var envelope in new[] { none, low, medium, high })
        {
            Assert.AreEqual(envelope.Performer.Reasoning, envelope.Interpreter.Reasoning);
            Assert.AreEqual(E0AReasoningLevel.High, envelope.Integrity.Reasoning);
            Assert.AreEqual("OpenAI", envelope.Performer.Provider);
            Assert.AreEqual("gpt-5.6-sol", envelope.Performer.Model);
        }
        Assert.Throws<E0AHarnessException>(() => E0AReferenceRunHost.CreateEnvelope("CREATIVE-XHIGH"));
    }

    [TestMethod]
    public void LiveHost_HttpTransportCannotPreemptFrozenAttemptDeadline()
    {
        using var http = E0AReferenceRunHost.CreateProviderHttpClient();

        Assert.AreEqual(Timeout.InfiniteTimeSpan, http.Timeout);
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
                state.StateHash.Value,
                state.CurrentOpportunityCharacterId!.Value);
            File.AppendAllText(Path.Combine(root, "manifest.json"), " ");

            Assert.Throws<E0AHarnessException>(() => E0AExistingEvidenceEvaluationSealer.Seal(
                root,
                new E0AHardGateEvaluation(
                    E0AEvidenceContracts.HardGateChecklistVersion,
                    "DIRECTOR",
                    "MANUAL-BLIND-HARD-GATE-REVIEW",
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

    private static PreparedRoleAttempt IntegrityAttempt(string runId)
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var cycle = E0ATestSupport.Cycle();
        var continuity = DeterministicE0CausalCycle.ComposeContext(cycle);
        var context = continuity.ContextEvaluation.Packet;
        var candidate = E0ATestSupport.Candidate(context, "No.");
        var input = IntegrityCandidateInput.Bind(context, candidate);
        var packet = E0AIntegrityAssessmentPacketBuilder.Build(
            cycle.ProductionState,
            continuity.AccessEvaluation,
            context,
            candidate);
        return E0ARequestBuilder.Integrity(
            RunId.From(runId),
            1,
            envelope.Integrity,
            context,
            input.CandidateContentHash,
            packet);
    }

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

    private sealed class StaticResponseHandler : HttpMessageHandler
    {
        private readonly string _json;
        internal StaticResponseHandler(string json) => _json = json;
        internal string? LastBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastBody = request.Content is null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            };
        }
    }
}
