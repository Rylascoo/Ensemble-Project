using System.Net;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Harness.Gemini;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiGenerateContentPortWireTests
{
    [TestMethod]
    public void PerformerRequest_UsesNativeGeminiNormativeShape()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-GEMINI-REQUEST"), 1, envelope.Performer, context);

        using var body = JsonDocument.Parse(attempt.RequestBody);
        var root = body.RootElement;
        Assert.IsTrue(root.TryGetProperty("systemInstruction", out var system));
        Assert.IsTrue(root.TryGetProperty("contents", out var contents));
        Assert.IsTrue(root.TryGetProperty("generationConfig", out var config));
        Assert.IsFalse(root.TryGetProperty("model", out _));
        Assert.IsFalse(root.TryGetProperty("tools", out _));
        Assert.IsFalse(root.TryGetProperty("cachedContent", out _));
        Assert.IsFalse(root.TryGetProperty("serviceTier", out _));
        Assert.IsFalse(root.GetProperty("store").GetBoolean());
        Assert.AreEqual(E0APromptContracts.PerformerInstructions, system.GetProperty("parts")[0].GetProperty("text").GetString());
        Assert.AreEqual("user", contents[0].GetProperty("role").GetString());
        Assert.AreEqual(1, config.GetProperty("candidateCount").GetInt32());
        Assert.AreEqual(E0ARunEnvelope.RoleMaxOutputTokens, config.GetProperty("maxOutputTokens").GetInt32());
        Assert.AreEqual(0, config.GetProperty("thinkingConfig").GetProperty("thinkingBudget").GetInt32());
        var responseFormat = config.GetProperty("responseFormat").GetProperty("text");
        Assert.AreEqual("application/json", responseFormat.GetProperty("mimeType").GetString());
        Assert.AreEqual(JsonValueKind.Object, responseFormat.GetProperty("schema").ValueKind);
        Assert.IsFalse(config.TryGetProperty("responseMimeType", out _));
        Assert.IsFalse(config.TryGetProperty("responseJsonSchema", out _));
    }

    [TestMethod]
    public void IntegrityRequest_BoundsCandidateAndFixesElevatedThinkingControl()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var attempt = GeminiIntegrityAttempt("E0A-GEMINI-INTEGRITY-REQUEST");

        using var body = JsonDocument.Parse(attempt.RequestBody);
        var config = body.RootElement.GetProperty("generationConfig");
        Assert.AreEqual(1, config.GetProperty("candidateCount").GetInt32());
        Assert.AreEqual(E0AGeminiProviderPolicy.IntegrityCandidateMaxOutputTokens, config.GetProperty("maxOutputTokens").GetInt32());
        Assert.AreEqual(E0AGeminiProviderPolicy.IntegrityThinkingBudgetTokens, config.GetProperty("thinkingConfig").GetProperty("thinkingBudget").GetInt32());
        Assert.AreEqual(envelope.Integrity.MaxOutputTokens, attempt.Profile.MaxOutputTokens);
    }

    [TestMethod]
    public async Task CountTokens_AddsRequiredNestedModelPreservesGenerationFieldsAndUsesApiKeyHeader()
    {
        var handler = new QueueResponseHandler(JsonResponse("{\"totalTokens\":37}"));
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "gemini-test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-GEMINI-COUNT"), 1, envelope.Performer, context);

        var tokens = await port.CountInputTokensAsync(attempt, CancellationToken.None);

        Assert.AreEqual(37L, tokens);
        Assert.AreEqual(1, handler.Requests.Count);
        var request = handler.Requests[0];
        StringAssert.Contains(request.Uri, "/v1beta/models/gemini-2.5-flash:countTokens");
        Assert.AreEqual("gemini-test-key", request.ApiKey);
        using var original = JsonDocument.Parse(attempt.RequestBody);
        using var sent = JsonDocument.Parse(request.Body);
        var projected = sent.RootElement.GetProperty("generateContentRequest");
        Assert.IsFalse(original.RootElement.TryGetProperty("model", out _));
        Assert.AreEqual("models/gemini-2.5-flash", projected.GetProperty("model").GetString());
        Assert.AreEqual(
            original.RootElement.EnumerateObject().Count() + 1,
            projected.EnumerateObject().Count());
        foreach (var property in original.RootElement.EnumerateObject())
        {
            Assert.IsTrue(projected.TryGetProperty(property.Name, out var projectedProperty));
            Assert.AreEqual(
                PreparedRoleAttempt.LowerSha256(JsonSerializer.SerializeToUtf8Bytes(property.Value)),
                PreparedRoleAttempt.LowerSha256(JsonSerializer.SerializeToUtf8Bytes(projectedProperty)));
        }
    }

    [TestMethod]
    public async Task CountTokens_NonSuccessExposesOnlyBoundedHttpDiagnostic()
    {
        var handler = new QueueResponseHandler(new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("provider body must not escape", Encoding.UTF8, "application/json")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "gemini-test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-GEMINI-COUNT-400"), 1, envelope.Performer, context);

        var exception = await Assert.ThrowsAsync<E0AHarnessException>(() =>
            port.CountInputTokensAsync(attempt, CancellationToken.None));

        Assert.AreEqual("gemini-counttokens-http-400", exception.Message);
        Assert.IsFalse(exception.Message.Contains("provider body", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task BufferedIntegrity_MapsIdentityUsageAndStructuredBytes()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var response = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = output } } },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 101,
                candidatesTokenCount = 12,
                thoughtsTokenCount = 3500,
                cachedContentTokenCount = 0,
                totalTokenCount = 3613
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-gemini-integrity"
        });
        var handler = new QueueResponseHandler(JsonResponse(response));
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = GeminiIntegrityAttempt("E0A-GEMINI-BUFFERED");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        Assert.AreEqual("resp-gemini-integrity", receipt.ResponseId);
        Assert.AreEqual("gemini-2.5-flash-20260901", receipt.ReturnedModel);
        Assert.AreEqual(101L, receipt.Usage!.InputTokens);
        Assert.AreEqual(3512L, receipt.Usage.OutputTokens);
        Assert.AreEqual(3500L, receipt.Usage.ReasoningTokens);
        Assert.AreEqual(0L, receipt.Usage.CachedInputTokens);
        CollectionAssert.AreEqual(E0ATestSupport.IntegrityOutput(), receipt.StructuredOutput!);
    }

    [TestMethod]
    public async Task BufferedResponse_RejectsThoughtMaterialBeforeSemanticReceipt()
    {
        var response = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = "internal", thought = true } } },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new { promptTokenCount = 10, candidatesTokenCount = 1, thoughtsTokenCount = 1 },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-thought"
        });
        var handler = new QueueResponseHandler(JsonResponse(response));
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = GeminiIntegrityAttempt("E0A-GEMINI-THOUGHT-REJECT");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-thought-material-returned", receipt.DiagnosticCode);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task StreamingPerformer_AssemblesClosedResponseAndRetainsDiagnosticChunks()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var split = output.Length / 2;
        var first = JsonSerializer.Serialize(new
        {
            candidates = new[] { new { content = new { parts = new[] { new { text = output[..split] } } } } },
            usageMetadata = new { promptTokenCount = 44 },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-stream"
        });
        var second = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = output[split..] } } },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 44,
                candidatesTokenCount = 20,
                thoughtsTokenCount = 0,
                cachedContentTokenCount = 0,
                totalTokenCount = 64
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-stream"
        });
        var sse = $"data: {first}\n\ndata: {second}\n\n";
        var handler = new QueueResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(sse, Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-GEMINI-STREAM"), 1, envelope.Performer, context);
        var diagnostics = new CollectingDiagnosticSink();

        var receipt = await port.ExecuteAsync(attempt, diagnostics, CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        Assert.AreEqual(2, diagnostics.Events.Count);
        Assert.AreEqual(0L, receipt.Usage!.ReasoningTokens);
        CollectionAssert.AreEqual(E0ATestSupport.PerformerOutput(), receipt.StructuredOutput!);
        StringAssert.Contains(handler.Requests[0].Uri, ":streamGenerateContent?alt=sse");
    }

    [TestMethod]
    public async Task StreamingResponse_RejectsIdentityChangeAcrossChunks()
    {
        var first = JsonSerializer.Serialize(new
        {
            candidates = new[] { new { content = new { parts = new[] { new { text = "{" } } } } },
            modelVersion = "gemini-2.5-flash-A",
            responseId = "resp-A"
        });
        var second = JsonSerializer.Serialize(new
        {
            candidates = new[] { new { content = new { parts = new[] { new { text = "}" } } }, finishReason = "STOP" } },
            usageMetadata = new { promptTokenCount = 10, candidatesTokenCount = 2 },
            modelVersion = "gemini-2.5-flash-B",
            responseId = "resp-A"
        });
        var handler = new QueueResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"data: {first}\n\ndata: {second}\n\n", Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-GEMINI-IDENTITY"), 1, envelope.Performer, context);

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-response-identity-changed", receipt.DiagnosticCode);
    }

    private static PreparedRoleAttempt GeminiIntegrityAttempt(string runId)
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
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

    private static HttpResponseMessage JsonResponse(string json) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

    private sealed record CapturedRequest(string Uri, string? ApiKey, byte[] Body);

    private sealed class QueueResponseHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;

        internal QueueResponseHandler(params HttpResponseMessage[] responses) =>
            _responses = new Queue<HttpResponseMessage>(responses);

        internal List<CapturedRequest> Requests { get; } = new();

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var body = request.Content is null
                ? Array.Empty<byte>()
                : await request.Content.ReadAsByteArrayAsync(cancellationToken);
            request.Headers.TryGetValues("x-goog-api-key", out var values);
            Requests.Add(new CapturedRequest(
                request.RequestUri!.ToString(),
                values?.SingleOrDefault(),
                body));
            return _responses.Dequeue();
        }
    }
}