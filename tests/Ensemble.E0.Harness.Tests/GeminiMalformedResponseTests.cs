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
public sealed class GeminiMalformedResponseTests
{
    [TestMethod]
    public async Task CountTokensRejectsWrongTypedTotalWithoutEscapingProviderBoundary()
    {
        var handler = new QueueHandler(Json("{\"totalTokens\":\"37\"}"));
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = PerformerAttempt("E0A-GEMINI-BAD-COUNT");

        await Assert.ThrowsAsync<E0AHarnessException>(() =>
            port.CountInputTokensAsync(attempt, CancellationToken.None));
    }

    [TestMethod]
    public async Task BufferedResponseRejectsWrongTypedIdentityAsTechnicalFailure()
    {
        var response = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput()) } } },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 10,
                candidatesTokenCount = 3,
                thoughtsTokenCount = 2,
                totalTokenCount = 15
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = 42
        });
        var handler = new QueueHandler(Json(response));
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = IntegrityAttempt("E0A-GEMINI-BAD-IDENTITY");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-response-identity-missing", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task BufferedResponseRejectsWrongTypedUsageAsTechnicalFailure()
    {
        var response = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput()) } } },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new
            {
                promptTokenCount = "10",
                candidatesTokenCount = 3,
                thoughtsTokenCount = 2,
                totalTokenCount = 15
            },
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-bad-usage"
        });
        var handler = new QueueHandler(Json(response));
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = IntegrityAttempt("E0A-GEMINI-BAD-USAGE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-usage-invalid", receipt.DiagnosticCode);
    }

    private static PreparedRoleAttempt PerformerAttempt(string runId)
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        return E0ARequestBuilder.Performer(RunId.From(runId), 1, envelope.Performer, context);
    }

    private static PreparedRoleAttempt IntegrityAttempt(string runId)
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

    private static HttpResponseMessage Json(string body) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };

    private sealed class QueueHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;

        internal QueueHandler(params HttpResponseMessage[] responses) =>
            _responses = new Queue<HttpResponseMessage>(responses);

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(_responses.Dequeue());
    }
}
