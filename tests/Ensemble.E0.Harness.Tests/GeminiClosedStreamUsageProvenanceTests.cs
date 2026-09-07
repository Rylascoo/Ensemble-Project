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
public sealed class GeminiClosedStreamUsageProvenanceTests
{
    [TestMethod]
    public async Task LaterInvalidUsage_DropsEarlierTupleSoSpendFallsBackConservatively()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var first = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = output } } },
                    finishReason = "STOP"
                }
            },
            usageMetadata = Usage(totalTokenCount: 14),
            modelVersion = "gemini-2.5-flash-test",
            responseId = "resp-later-invalid-usage"
        });
        var second = JsonSerializer.Serialize(new
        {
            usageMetadata = Usage(totalTokenCount: 999),
            modelVersion = "gemini-2.5-flash-test",
            responseId = "resp-later-invalid-usage"
        });

        var receipt = await ExecuteAsync(
            $"data: {first}\n\ndata: {second}\n\n",
            "E0A-GEMINI-LATER-INVALID-USAGE");

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-usage-invalid", receipt.DiagnosticCode);
        Assert.AreEqual("resp-later-invalid-usage", receipt.ResponseId);
        Assert.AreEqual("gemini-2.5-flash-test", receipt.ReturnedModel);
        Assert.IsNull(receipt.Usage);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task FullyConsumedNonStopStream_RetainsValidatedTerminalUsage()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var chunk = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = output } } },
                    finishReason = "MAX_TOKENS"
                }
            },
            usageMetadata = Usage(totalTokenCount: 14),
            modelVersion = "gemini-2.5-flash-test",
            responseId = "resp-nonstop-closed"
        });

        var receipt = await ExecuteAsync(
            $"data: {chunk}\n\n",
            "E0A-GEMINI-CLOSED-NONSTOP");

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-response-incomplete", receipt.DiagnosticCode);
        Assert.AreEqual("resp-nonstop-closed", receipt.ResponseId);
        Assert.AreEqual("gemini-2.5-flash-test", receipt.ReturnedModel);
        Assert.AreEqual(10L, receipt.Usage!.InputTokens);
        Assert.AreEqual(4L, receipt.Usage.OutputTokens);
        Assert.IsNull(receipt.StructuredOutput);
    }

    private static object Usage(long totalTokenCount) => new
    {
        promptTokenCount = 10,
        candidatesTokenCount = 4,
        thoughtsTokenCount = 0,
        cachedContentTokenCount = 0,
        toolUsePromptTokenCount = 0,
        totalTokenCount
    };

    private static async Task<RoleAttemptReceipt> ExecuteAsync(string sse, string runId)
    {
        using var http = new HttpClient(new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(sse, Encoding.UTF8, "text/event-stream")
        }));
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From(runId), 1, envelope.Performer, context);

        return await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);
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
