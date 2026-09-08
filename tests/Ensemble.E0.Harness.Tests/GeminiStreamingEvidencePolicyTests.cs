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
public sealed class GeminiStreamingEvidencePolicyTests
{
    [TestMethod]
    public async Task StreamingThoughtMaterial_IsRejectedBeforeDiagnosticPersistence()
    {
        var thoughtChunk = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new
                    {
                        parts = new[] { new { text = "private thought", thought = true } }
                    }
                }
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-thought-stream"
        });
        var handler = new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"data: {thoughtChunk}\n\n", Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(
            RunId.From("E0A-GEMINI-STREAM-THOUGHT"),
            1,
            envelope.Performer,
            context);
        var diagnostics = new CollectingDiagnosticSink();

        var receipt = await port.ExecuteAsync(attempt, diagnostics, CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-thought-material-returned", receipt.DiagnosticCode);
        Assert.AreEqual(0, diagnostics.Events.Count);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task StreamingGemini35ThoughtSignature_IsAcceptedButStrippedBeforeDiagnosticPersistence()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var chunk = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new
                    {
                        parts = new[] { new { text = output, thoughtSignature = "c2lnbmF0dXJl" } }
                    },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 20,
                candidatesTokenCount = 10,
                thoughtsTokenCount = 4,
                cachedContentTokenCount = 0,
                totalTokenCount = 34
            },
            modelVersion = "gemini-3.5-flash-lite-20260907",
            responseId = "resp-35-signature-stream"
        });
        var handler = new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"data: {chunk}\n\n", Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(
            RunId.From("E0A-GEMINI-35-SIGNATURE-STREAM"),
            1,
            envelope.Performer,
            context);
        var diagnostics = new CollectingDiagnosticSink();

        var receipt = await port.ExecuteAsync(attempt, diagnostics, CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        CollectionAssert.AreEqual(E0ATestSupport.PerformerOutput(), receipt.StructuredOutput!);
        Assert.AreEqual(1, diagnostics.Events.Count);
        using var diagnostic = JsonDocument.Parse(diagnostics.Events[0]);
        var persistedPart = diagnostic.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0];
        Assert.AreEqual(output, persistedPart.GetProperty("text").GetString());
        Assert.IsFalse(persistedPart.TryGetProperty("thoughtSignature", out _));
        Assert.IsFalse(diagnostics.Events[0].Contains("c2lnbmF0dXJl", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task BufferedGemini35ThoughtSignature_IsIgnoredOutsideSemanticReceipt()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var response = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new
                    {
                        parts = new[] { new { text = output, thoughtSignature = "c2lnbmF0dXJl" } }
                    },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 30,
                candidatesTokenCount = 2,
                thoughtsTokenCount = 6,
                cachedContentTokenCount = 0,
                totalTokenCount = 38
            },
            modelVersion = "gemini-3.5-flash-lite-20260907",
            responseId = "resp-35-signature-buffered"
        });
        var handler = new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(response, Encoding.UTF8, "application/json")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = Gemini35IntegrityAttempt("E0A-GEMINI-35-SIGNATURE-BUFFERED");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        CollectionAssert.AreEqual(E0ATestSupport.IntegrityOutput(), receipt.StructuredOutput!);
        Assert.AreEqual(6L, receipt.Usage!.ReasoningTokens);
    }

    [TestMethod]
    public async Task StreamingGemini35ThoughtSummary_RemainsRejectedEvenWhenSignatureMetadataIsAllowed()
    {
        var chunk = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new
                    {
                        parts = new[]
                        {
                            new { text = "private thought", thought = true, thoughtSignature = "c2lnbmF0dXJl" }
                        }
                    }
                }
            },
            modelVersion = "gemini-3.5-flash-lite-20260907",
            responseId = "resp-35-thought-stream"
        });
        var handler = new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"data: {chunk}\n\n", Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(
            RunId.From("E0A-GEMINI-35-THOUGHT-STREAM"),
            1,
            envelope.Performer,
            context);
        var diagnostics = new CollectingDiagnosticSink();

        var receipt = await port.ExecuteAsync(attempt, diagnostics, CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-thought-material-returned", receipt.DiagnosticCode);
        Assert.AreEqual(0, diagnostics.Events.Count);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task StreamingGemini25ThoughtSignature_RemainsRejectedBeforeDiagnosticPersistence()
    {
        var chunk = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new
                    {
                        parts = new[] { new { text = "opaque", thoughtSignature = "c2lnbmF0dXJl" } }
                    }
                }
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-25-signature-stream"
        });
        var handler = new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"data: {chunk}\n\n", Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(
            RunId.From("E0A-GEMINI-25-SIGNATURE-STREAM"),
            1,
            envelope.Performer,
            context);
        var diagnostics = new CollectingDiagnosticSink();

        var receipt = await port.ExecuteAsync(attempt, diagnostics, CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-thought-material-returned", receipt.DiagnosticCode);
        Assert.AreEqual(0, diagnostics.Events.Count);
    }

    private static PreparedRoleAttempt Gemini35IntegrityAttempt(string runId)
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
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

    private sealed class SingleResponseHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        internal SingleResponseHandler(HttpResponseMessage response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(_response);
    }
}
