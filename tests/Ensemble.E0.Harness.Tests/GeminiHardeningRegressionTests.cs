using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
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
        Assert.AreEqual("gemini-malformed-or-transport", receipt.DiagnosticCode);
    }

    private static PreparedRoleAttempt PerformerAttempt(string runId)
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        return E0ARequestBuilder.Performer(RunId.From(runId), 1, envelope.Performer, context);
    }

    private sealed class StaticHandler : HttpMessageHandler
    {
        private readonly Func<HttpResponseMessage> _response;

        internal StaticHandler(Func<HttpResponseMessage> response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(_response());
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
