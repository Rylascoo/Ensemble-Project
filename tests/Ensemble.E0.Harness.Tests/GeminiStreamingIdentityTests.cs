using System.Net;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Gemini;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiStreamingIdentityTests
{
    [TestMethod]
    public async Task IntermediateChunkMayOmitIdentityWhileCompletedStreamStillBindsIt()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var split = output.Length / 2;
        var first = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new { content = new { parts = new[] { new { text = output[..split] } } } }
            }
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
                toolUsePromptTokenCount = 0,
                totalTokenCount = 64
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-stream-late-identity"
        });
        var handler = new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"data: {first}\n\ndata: {second}\n\n", Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(
            RunId.From("E0A-GEMINI-LATE-IDENTITY"),
            1,
            envelope.Performer,
            context);

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        Assert.AreEqual("resp-stream-late-identity", receipt.ResponseId);
        Assert.AreEqual("gemini-2.5-flash-20260901", receipt.ReturnedModel);
        CollectionAssert.AreEqual(E0ATestSupport.PerformerOutput(), receipt.StructuredOutput!);
    }

    [TestMethod]
    public async Task ChunkWithOnlyHalfOfIdentityPairFailsClosed()
    {
        var chunk = JsonSerializer.Serialize(new
        {
            candidates = new[] { new { content = new { parts = new[] { new { text = "{" } } } } },
            responseId = "resp-unpaired"
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
            RunId.From("E0A-GEMINI-UNPAIRED-IDENTITY"),
            1,
            envelope.Performer,
            context);

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-response-identity-changed", receipt.DiagnosticCode);
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
