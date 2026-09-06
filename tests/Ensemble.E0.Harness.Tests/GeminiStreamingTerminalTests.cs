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
public sealed class GeminiStreamingTerminalTests
{
    [TestMethod]
    public async Task SemanticBytesAfterStopAreRejectedWhileMetadataOnlyCompletionRemainsAllowed()
    {
        var first = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = "{\"schemaVersion\":\"ensemble.e0.performer-candidate.v1\"" } } },
                    finishReason = "STOP"
                }
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-after-stop"
        });
        var second = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new { content = new { parts = new[] { new { text = ",\"performance\":{} }" } } } }
            },
            usageMetadata = new
            {
                promptTokenCount = 10,
                candidatesTokenCount = 4,
                thoughtsTokenCount = 0,
                totalTokenCount = 14
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-after-stop"
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
            RunId.From("E0A-GEMINI-AFTER-STOP"),
            1,
            envelope.Performer,
            context);

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-content-after-stop", receipt.DiagnosticCode);
        Assert.IsNull(receipt.StructuredOutput);
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
