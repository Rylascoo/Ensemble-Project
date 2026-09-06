using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Harness.OpenAI;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class OpenAIResponsesPortWireTests
{
    [TestMethod]
    public async Task InputTokenPreflight_ProjectsOnlySupportedRequestFields()
    {
        var handler = new RecordingHandler(_ => JsonResponse("{\"input_tokens\":321}"));
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = PerformerAttempt("E0A-WIRE-TOKENS");

        var count = await port.CountInputTokensAsync(attempt, CancellationToken.None);

        Assert.AreEqual(321L, count);
        Assert.AreEqual("https://api.openai.com/v1/responses/input_tokens", handler.LastUri?.ToString());
        Assert.AreEqual("Bearer", handler.LastAuthorization?.Scheme);
        Assert.AreEqual("test-secret", handler.LastAuthorization?.Parameter);
        using var document = JsonDocument.Parse(handler.LastBody!);
        var root = document.RootElement;
        foreach (var required in new[] { "model", "instructions", "input", "reasoning", "text", "truncation" })
        {
            Assert.IsTrue(root.TryGetProperty(required, out _), required);
        }
        foreach (var excluded in new[] { "stream", "store", "service_tier", "max_output_tokens" })
        {
            Assert.IsFalse(root.TryGetProperty(excluded, out _), excluded);
        }
        Assert.IsFalse(handler.LastBody!.Contains("test-secret", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task BufferedCompletedResponse_ProducesClosedSuccessReceipt()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var responseJson = JsonSerializer.Serialize(new
        {
            status = "completed",
            id = "resp-buffered",
            model = "gpt-5.6-sol",
            output_text = output,
            usage = new
            {
                input_tokens = 10,
                output_tokens = 5,
                input_tokens_details = new { cached_tokens = 2 },
                output_tokens_details = new { reasoning_tokens = 3 }
            }
        });
        var handler = new RecordingHandler(_ => JsonResponse(responseJson));
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-WIRE-BUFFERED");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        Assert.AreEqual("resp-buffered", receipt.ResponseId);
        Assert.AreEqual("gpt-5.6-sol", receipt.ReturnedModel);
        Assert.AreEqual(10L, receipt.Usage!.InputTokens);
        Assert.AreEqual(2L, receipt.Usage.CachedInputTokens);
        Assert.AreEqual(3L, receipt.Usage.ReasoningTokens);
        CollectionAssert.AreEqual(E0ATestSupport.IntegrityOutput(), receipt.StructuredOutput!);
    }

    [TestMethod]
    public async Task StreamingCompletedResponse_UsesSemanticDeltasAndRecordsDiagnostics()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var delta = JsonSerializer.Serialize(new { type = "response.output_text.delta", delta = output });
        var completed = JsonSerializer.Serialize(new
        {
            type = "response.completed",
            response = new
            {
                status = "completed",
                id = "resp-stream",
                model = "gpt-5.6-sol",
                usage = new { input_tokens = 8, output_tokens = 4 }
            }
        });
        var sse = $"data: {delta}\n\ndata: {completed}\n\n";
        var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(sse, Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = PerformerAttempt("E0A-WIRE-STREAM");
        var diagnostics = new CollectingDiagnosticSink();

        var receipt = await port.ExecuteAsync(attempt, diagnostics, CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        Assert.AreEqual("resp-stream", receipt.ResponseId);
        CollectionAssert.AreEqual(E0ATestSupport.PerformerOutput(), receipt.StructuredOutput!);
        Assert.AreEqual(2, diagnostics.Events.Count);
    }

    [TestMethod]
    public async Task MalformedProviderUsage_FailsClosedAsTechnicalReceipt()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var responseJson = JsonSerializer.Serialize(new
        {
            status = "completed",
            id = "resp-invalid-usage",
            model = "gpt-5.6-sol",
            output_text = output,
            usage = new { input_tokens = -1, output_tokens = 5 }
        });
        var handler = new RecordingHandler(_ => JsonResponse(responseJson));
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-WIRE-INVALID");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.IsNull(receipt.Usage);
        Assert.AreEqual("malformed-or-transport", receipt.DiagnosticCode);
    }

    [TestMethod]
    public void BlankApiKey_IsRejectedBeforeAnyRequest()
    {
        using var http = new HttpClient(new RecordingHandler(_ => JsonResponse("{}")));
        Assert.Throws<E0AHarnessException>(() => new OpenAIResponsesPort(http, " "));
    }

    private static PreparedRoleAttempt PerformerAttempt(string runId)
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        return E0ARequestBuilder.Performer(RunId.From(runId), 1, envelope.Performer, context);
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

    private static HttpResponseMessage JsonResponse(string json) => new(HttpStatusCode.OK)
    {
        Content = new StringContent(json, Encoding.UTF8, "application/json")
    };

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _response;
        internal RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> response) => _response = response;

        internal Uri? LastUri { get; private set; }
        internal AuthenticationHeaderValue? LastAuthorization { get; private set; }
        internal string? LastBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastUri = request.RequestUri;
            LastAuthorization = request.Headers.Authorization;
            LastBody = request.Content is null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);
            return _response(request);
        }
    }
}
