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

    private sealed class SingleResponseHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        internal SingleResponseHandler(HttpResponseMessage response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(_response);
    }
}
