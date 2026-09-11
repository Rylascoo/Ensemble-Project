using System.Net;
using System.Net.Http.Headers;
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
public sealed class GeminiHardeningRegressionTests
{
    [TestMethod]
    public async Task StreamingStalledBody_CancelsWithoutSynchronousEofProbe()
    {
        var stream = new CancellableStalledStream();
        var handler = new StaticHandler(() =>
        {
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = PerformerAttempt("E0A-GEMINI-HARDENING-CANCEL");
        using var cancellation = new CancellationTokenSource();

        var execution = port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), cancellation.Token);
        await stream.ReadStarted.WaitAsync(TimeSpan.FromSeconds(2));
        cancellation.Cancel();

        try
        {
            _ = await execution;
            Assert.Fail("A stalled Gemini streaming body must remain cancellable.");
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
        }

        Assert.AreEqual(0, stream.SynchronousReadCount);
    }

    [TestMethod]
    public async Task StreamingMalformedUtf8_FailsClosedBeforeSemanticAdoption()
    {
        var prefix = Encoding.ASCII.GetBytes(
            "data: {\"responseId\":\"resp-bad-utf8\",\"modelVersion\":\"gemini-2.5-flash-test\",\"candidates\":[{\"content\":{\"parts\":[{\"text\":\"");
        var suffix = Encoding.ASCII.GetBytes(
            "\"}]},\"finishReason\":\"STOP\"}],\"usageMetadata\":{\"promptTokenCount\":1,\"candidatesTokenCount\":1,\"totalTokenCount\":2}}\n\n");
        var body = new byte[prefix.Length + 1 + suffix.Length];
        prefix.CopyTo(body, 0);
        body[prefix.Length] = 0xff;
        suffix.CopyTo(body, prefix.Length + 1);

        var handler = new StaticHandler(() =>
        {
            var content = new ByteArrayContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = PerformerAttempt("E0A-GEMINI-HARDENING-UTF8");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.AreEqual("gemini-utf8-invalid", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task StreamingMalformedJson_FailsClosedWithClassifiedDiagnostic()
    {
        var body = Encoding.UTF8.GetBytes("data: {not-json}\n\n");
        var handler = new StaticHandler(() =>
        {
            var content = new ByteArrayContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = PerformerAttempt("E0A-GEMINI-HARDENING-JSON");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.AreEqual("gemini-json-invalid", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task StreamingIoFailure_FailsClosedWithClassifiedDiagnostic()
    {
        var handler = new StaticHandler(() =>
        {
            var content = new StreamContent(new ThrowingIoStream());
            content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        });
        using var http = new HttpClient(handler);
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = PerformerAttempt("E0A-GEMINI-HARDENING-IO-TRANSPORT");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.AreEqual("gemini-io-transport", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task HttpTransportFailure_FailsClosedWithClassifiedDiagnostic()
    {
        using var http = new HttpClient(new ThrowingHandler());
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = PerformerAttempt("E0A-GEMINI-HARDENING-HTTP-TRANSPORT");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.AreEqual("gemini-http-transport", receipt.DiagnosticCode);
    }

    [TestMethod]
    public async Task BufferedWrongTypedOptionalField_FailsClosedAndRetainsSafeMetadata()
    {
        var response = SuccessfulBufferedShape("STOP");
        using var source = JsonDocument.Parse(response);
        var malformed = JsonSerializer.Serialize(new
        {
            candidates = source.RootElement.GetProperty("candidates").Clone(),
            usageMetadata = source.RootElement.GetProperty("usageMetadata").Clone(),
            modelVersion = "gemini-2.5-flash-test",
            responseId = "resp-wrong-prompt-feedback",
            promptFeedback = 7
        });
        using var http = new HttpClient(new StaticHandler(() => Json(malformed)));
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = IntegrityAttempt("E0A-GEMINI-HARDENING-WRONG-OPTIONAL");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-response-shape-invalid", receipt.DiagnosticCode);
        Assert.AreEqual("resp-wrong-prompt-feedback", receipt.ResponseId);
        Assert.AreEqual("gemini-2.5-flash-test", receipt.ReturnedModel);
        Assert.IsNotNull(receipt.Usage);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task BufferedWrongTypedFinishReason_FailsClosedAndRetainsSafeMetadata()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var response = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = output } } },
                    finishReason = (object)7
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 10,
                candidatesTokenCount = 3,
                thoughtsTokenCount = 2,
                totalTokenCount = 15
            },
            modelVersion = "gemini-2.5-flash-test",
            responseId = "resp-wrong-finish"
        });
        using var http = new HttpClient(new StaticHandler(() => Json(response)));
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = IntegrityAttempt("E0A-GEMINI-HARDENING-WRONG-FINISH");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-response-shape-invalid", receipt.DiagnosticCode);
        Assert.AreEqual("resp-wrong-finish", receipt.ResponseId);
        Assert.IsNotNull(receipt.Usage);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task BufferedNonStopTerminal_RetainsValidatedUsageWithoutSemanticOutput()
    {
        var response = SuccessfulBufferedShape("MAX_TOKENS");
        using var http = new HttpClient(new StaticHandler(() => Json(response)));
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = IntegrityAttempt("E0A-GEMINI-HARDENING-NONSTOP");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("gemini-response-incomplete", receipt.DiagnosticCode);
        Assert.AreEqual("resp-provenance", receipt.ResponseId);
        Assert.AreEqual("gemini-2.5-flash-test", receipt.ReturnedModel);
        Assert.AreEqual(10L, receipt.Usage!.InputTokens);
        Assert.AreEqual(5L, receipt.Usage.OutputTokens);
        Assert.AreEqual(2L, receipt.Usage.ReasoningTokens);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task BufferedMalformedUnicodeIdentity_FailsClosed()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        var response = "{\"candidates\":[{\"content\":{\"parts\":[{\"text\":" + JsonSerializer.Serialize(output) + "}]},\"finishReason\":\"STOP\"}]," +
            "\"usageMetadata\":{\"promptTokenCount\":10,\"candidatesTokenCount\":3,\"thoughtsTokenCount\":2,\"totalTokenCount\":15}," +
            "\"modelVersion\":\"gemini-2.5-flash-test\",\"responseId\":\"\\uD800\"}";
        using var http = new HttpClient(new StaticHandler(() => Json(response)));
        var port = new GeminiGenerateContentPort(http, "test-key");
        var attempt = IntegrityAttempt("E0A-GEMINI-HARDENING-UNICODE-ID");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.IsNull(receipt.StructuredOutput);
    }

    private static string SuccessfulBufferedShape(string finishReason)
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.IntegrityOutput());
        return JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new { parts = new[] { new { text = output } } },
                    finishReason
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 10,
                candidatesTokenCount = 3,
                thoughtsTokenCount = 2,
                totalTokenCount = 15
            },
            modelVersion = "gemini-2.5-flash-test",
            responseId = "resp-provenance"
        });
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

    private sealed class ThrowingHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromException<HttpResponseMessage>(new HttpRequestException("simulated transport"));
    }

    private sealed class StaticHandler : HttpMessageHandler
    {
        private readonly Func<HttpResponseMessage> _response;

        internal StaticHandler(Func<HttpResponseMessage> response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(_response());
    }

    private sealed class ThrowingIoStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush() { }
        public override int Read(byte[] buffer, int offset, int count) => throw new IOException("simulated read");
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            Task.FromException<int>(new IOException("simulated read"));
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
            ValueTask.FromException<int>(new IOException("simulated read"));
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
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
