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
public sealed class GeminiUsageContractTests
{
    [TestMethod]
    public async Task BufferedResponse_RejectsProviderTotalThatDoesNotMatchMappedUsage()
    {
        var response = SuccessResponse(new
        {
            promptTokenCount = 10,
            candidatesTokenCount = 3,
            thoughtsTokenCount = 2,
            totalTokenCount = 99
        });

        var receipt = await ExecuteBufferedAsync(response, "E0A-GEMINI-USAGE-TOTAL");

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-usage-invalid", receipt.DiagnosticCode);
        Assert.IsNull(receipt.Usage);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task BufferedResponse_RejectsUnexpectedToolUsePromptTokens()
    {
        var response = SuccessResponse(new
        {
            promptTokenCount = 10,
            candidatesTokenCount = 3,
            thoughtsTokenCount = 2,
            toolUsePromptTokenCount = 1,
            totalTokenCount = 15
        });

        var receipt = await ExecuteBufferedAsync(response, "E0A-GEMINI-USAGE-TOOLS");

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-usage-invalid", receipt.DiagnosticCode);
        Assert.IsNull(receipt.Usage);
        Assert.IsNull(receipt.StructuredOutput);
    }

    private static string SuccessResponse(object usageMetadata) =>
        JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput()) } } },
                    finishReason = "STOP"
                }
            },
            usageMetadata,
            modelVersion = "gemini-2.5-flash-20260901",
            responseId = "resp-usage-contract"
        });

    private static async Task<RoleAttemptReceipt> ExecuteBufferedAsync(string responseJson, string runId)
    {
        var handler = new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From(runId), 1, envelope.Performer, context);

        // Force the buffered transport while preserving the approved Gemini role
        // profile so the response parser can be tested independently of SSE.
        var bufferedProfile = envelope.Integrity;
        var integrityAttempt = new PreparedRoleAttempt(
            RunId.From(runId),
            E0ADeterministicIds.Attempt(RunId.From(runId), E0ARole.Integrity, 1, 1),
            bufferedProfile,
            1,
            context.SubjectCharacterId,
            context.ContextPacketId,
            context.StructuredContextHash,
            context.RenderedContextHash,
            new string('a', 64),
            new string('b', 64),
            new string('c', 64),
            new string('d', 64),
            attempt.RequestBody);

        return await port.ExecuteAsync(bufferedProfile.Stream ? attempt : integrityAttempt, new CollectingDiagnosticSink(), CancellationToken.None);
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
