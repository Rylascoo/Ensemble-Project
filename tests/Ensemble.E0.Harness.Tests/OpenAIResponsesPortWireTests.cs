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
    public async Task InputTokenPreflight_WrongJsonTypeFailsInsideHarnessDomain()
    {
        var handler = new RecordingHandler(_ => JsonResponse("{\"input_tokens\":\"321\"}"));
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = PerformerAttempt("E0A-WIRE-TOKENS-WRONG-TYPE");

        try
        {
            _ = await port.CountInputTokensAsync(attempt, CancellationToken.None);
            Assert.Fail("Wrongly typed provider token counts must fail inside the Harness domain.");
        }
        catch (E0AHarnessException)
        {
        }
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
    public async Task BufferedWrongJsonType_FailsClosedAsTechnicalReceipt()
    {
        var responseJson = "{\"status\":7,\"id\":\"resp-wrong-type\",\"model\":\"gpt-5.6-sol\",\"output_text\":\"{}\",\"usage\":{\"input_tokens\":1,\"output_tokens\":1}}";
        var handler = new RecordingHandler(_ => JsonResponse(responseJson));
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-WIRE-BUFFERED-WRONG-TYPE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.AreEqual("provider-incomplete", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task BufferedMalformedUnicode_FailsClosedAsTechnicalReceipt()
    {
        var responseJson = "{\"status\":\"completed\",\"id\":\"\\uD800\",\"model\":\"gpt-5.6-sol\",\"output_text\":\"{}\",\"usage\":{\"input_tokens\":1,\"output_tokens\":1}}";
        var handler = new RecordingHandler(_ => JsonResponse(responseJson));
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-WIRE-BUFFERED-UNICODE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task StreamingCompletedResponse_UsesCompletedSemanticOutputAndRecordsProvisionalDiagnostics()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var delta = JsonSerializer.Serialize(new { type = "response.output_text.delta", delta = output });
        var completed = StreamingCompletedEvent("resp-stream", output, inputTokens: 8, outputTokens: 4);
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
    public async Task StreamingWrongJsonType_FailsClosedAsTechnicalReceipt()
    {
        var sse = "data: {\"type\":7}\n\n";
        var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(sse, Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = PerformerAttempt("E0A-WIRE-STREAM-WRONG-TYPE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.AreEqual("malformed-provider-event", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task StreamingMalformedUtf8_FailsClosedAsTechnicalReceipt()
    {
        var prefix = Encoding.ASCII.GetBytes("data: ");
        var suffix = Encoding.ASCII.GetBytes("\n\n");
        var body = new byte[prefix.Length + 1 + suffix.Length];
        prefix.CopyTo(body, 0);
        body[prefix.Length] = 0xff;
        suffix.CopyTo(body, prefix.Length + 1);

        var handler = new RecordingHandler(_ =>
        {
            var content = new ByteArrayContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        });
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = PerformerAttempt("E0A-WIRE-STREAM-UNICODE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.AreEqual("malformed-or-transport", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task StreamingStalledBody_CancelsWithoutSynchronousEofProbe()
    {
        var stream = new CancellableStalledStream();
        var handler = new RecordingHandler(_ =>
        {
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        });
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = PerformerAttempt("E0A-WIRE-STREAM-CANCEL");
        using var cancellation = new CancellationTokenSource();

        var execution = port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), cancellation.Token);
        await stream.ReadStarted.WaitAsync(TimeSpan.FromSeconds(2));
        cancellation.Cancel();

        try
        {
            _ = await execution;
            Assert.Fail("A stalled streaming body must remain cancellable.");
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
        }

        Assert.AreEqual(0, stream.SynchronousReadCount);
    }

    [TestMethod]
    public async Task StreamingProvisionalDeltaCannotOverrideCompletedSemanticOutput()
    {
        var provisional = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput("Provisional."));
        var final = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput("Final."));
        var delta = JsonSerializer.Serialize(new { type = "response.output_text.delta", delta = provisional });
        var completed = StreamingCompletedEvent("resp-stream-final", final, inputTokens: 8, outputTokens: 4);
        var sse = $"data: {delta}\n\ndata: {completed}\n\n";
        var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(sse, Encoding.UTF8, "text/event-stream")
        });
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = PerformerAttempt("E0A-WIRE-STREAM-FINAL");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        CollectionAssert.AreEqual(E0ATestSupport.PerformerOutput("Final."), receipt.StructuredOutput!);
        Assert.AreEqual(8L, receipt.Usage!.InputTokens);
        Assert.AreEqual(4L, receipt.Usage.OutputTokens);
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
        Assert.AreEqual("provider-response-incomplete", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task ReasoningUsageAboveOutputUsage_FailsClosedAsTechnicalReceipt()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var responseJson = JsonSerializer.Serialize(new
        {
            status = "completed",
            id = "resp-invalid-reasoning-usage",
            model = "gpt-5.6-sol",
            output_text = output,
            usage = new
            {
                input_tokens = 10,
                output_tokens = 5,
                output_tokens_details = new { reasoning_tokens = 6 }
            }
        });
        var handler = new RecordingHandler(_ => JsonResponse(responseJson));
        using var http = new HttpClient(handler);
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-WIRE-REASONING-USAGE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.IsNull(receipt.Usage);
        Assert.AreEqual("provider-response-incomplete", receipt.DiagnosticCode);
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

    private static string StreamingCompletedEvent(
        string responseId,
        string output,
        long inputTokens,
        long outputTokens) =>
        JsonSerializer.Serialize(new
        {
            type = "response.completed",
            response = new
            {
                status = "completed",
                id = responseId,
                model = "gpt-5.6-sol",
                output = new[]
                {
                    new
                    {
                        type = "message",
                        content = new[]
                        {
                            new { type = "output_text", text = output }
                        }
                    }
                },
                usage = new { input_tokens = inputTokens, output_tokens = outputTokens }
            }
        });

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

    private sealed class CancellableStalledStream : Stream
    {
        private readonly TaskCompletionSource<bool> _readStarted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        internal int SynchronousReadCount { get; private set; }
        internal Task ReadStarted => _readStarted.Task;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            SynchronousReadCount++;
            throw new InvalidOperationException("Synchronous reads are forbidden for this test stream.");
        }

        public override Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken) =>
            WaitForCancellationAsync(cancellationToken);

        public override ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default) =>
            new(WaitForCancellationAsync(cancellationToken));

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        private async Task<int> WaitForCancellationAsync(CancellationToken cancellationToken)
        {
            _readStarted.TrySetResult(true);
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
            return 0;
        }
    }
}
