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
    [TestMethod]
    public void PreparedRequest_DisablesImplicitPromptCaching()
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-CACHE-MODE"), 1, envelope.Performer, context);

        using var body = JsonDocument.Parse(attempt.RequestBody);
        var options = body.RootElement.GetProperty("prompt_cache_options");
        Assert.AreEqual("explicit", options.GetProperty("mode").GetString());
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
    public async Task UnexpectedCacheWriteUsage_FailsClosedAsTechnicalReceipt()
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
                input_tokens_details = new { cached_tokens = 0, cache_write_tokens = 4 },
                output_tokens_details = new { reasoning_tokens = 3 }
            }
        });
        using var http = new HttpClient(new StaticResponseHandler(responseJson));
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-CACHE-WRITE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.IsNull(receipt.Usage);
        Assert.AreEqual("provider-response-incomplete", receipt.DiagnosticCode);
    }

    [TestMethod]
    public void HostPricing_IsConservativeForPublishedLongContextTier()
    {
        Assert.AreEqual(8.00m, E0AReferenceRunHost.ConservativePricing.InputUsdPerMillionTokens);
        Assert.AreEqual(0.80m, E0AReferenceRunHost.ConservativePricing.CachedInputUsdPerMillionTokens);
        Assert.AreEqual(30.00m, E0AReferenceRunHost.ConservativePricing.OutputUsdPerMillionTokens);
        E0AReferenceRunHost.ConservativePricing.Validate();
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
