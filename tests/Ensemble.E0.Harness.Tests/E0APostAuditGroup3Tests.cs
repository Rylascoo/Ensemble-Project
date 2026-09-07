using System.Net;
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
public sealed class E0APostAuditGroup3Tests
{
    [TestMethod]
    public async Task BufferedRefusal_PreservesValidatedIdentityAndUsageWithoutSemanticOutput()
    {
        var response = JsonSerializer.Serialize(new
        {
            status = "completed",
            id = "resp-refused",
            model = "gpt-5.6-sol",
            output = new[]
            {
                new { content = new[] { new { type = "refusal", refusal = "no" } } }
            },
            usage = new { input_tokens = 21, output_tokens = 3 }
        });
        using var http = new HttpClient(new JsonHandler(response));
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-G3-REFUSAL");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("provider-refusal", receipt.DiagnosticCode);
        Assert.AreEqual("resp-refused", receipt.ResponseId);
        Assert.AreEqual("gpt-5.6-sol", receipt.ReturnedModel);
        Assert.AreEqual(21L, receipt.Usage!.InputTokens);
        Assert.AreEqual(3L, receipt.Usage.OutputTokens);
        Assert.IsNull(receipt.StructuredOutput);
        Assert.IsNull(receipt.StructuredOutputHash);
    }

    [TestMethod]
    public async Task BufferedIncomplete_PreservesValidatedIdentityAndUsageWithoutSemanticOutput()
    {
        var response = JsonSerializer.Serialize(new
        {
            status = "incomplete",
            id = "resp-incomplete",
            model = "gpt-5.6-sol",
            usage = new { input_tokens = 17, output_tokens = 2 }
        });
        using var http = new HttpClient(new JsonHandler(response));
        var port = new OpenAIResponsesPort(http, "test-secret");
        var attempt = IntegrityAttempt("E0A-G3-INCOMPLETE");

        var receipt = await port.ExecuteAsync(attempt, new CollectingDiagnosticSink(), CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.TechnicalFailure, receipt.Outcome);
        Assert.AreEqual("provider-incomplete", receipt.DiagnosticCode);
        Assert.AreEqual("resp-incomplete", receipt.ResponseId);
        Assert.AreEqual("gpt-5.6-sol", receipt.ReturnedModel);
        Assert.AreEqual(17L, receipt.Usage!.InputTokens);
        Assert.AreEqual(2L, receipt.Usage.OutputTokens);
        Assert.IsNull(receipt.StructuredOutput);
    }

    [TestMethod]
    public async Task FailedReceiptWithUsage_ReconcilesUsageAndCannotCreateFiction()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-G3-FAILED-USAGE");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider((attempt, _) =>
                RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    "provider-refusal",
                    "resp-failed-usage",
                    "gpt-5.6-sol",
                    new E0AUsage(100, 5)));
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(100),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.IsFalse(result.HasUnknownProviderUsage);
            Assert.AreEqual(E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions, result.SpendEstimateStatus);
            Assert.IsTrue(result.EstimatedSpendUsd > 0m);

            using var terminal = JsonDocument.Parse(File.ReadAllBytes(
                Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Single()));
            Assert.AreEqual("resp-failed-usage", terminal.RootElement.GetProperty("responseId").GetString());
            Assert.AreEqual(100L, terminal.RootElement.GetProperty("usage").GetProperty("InputTokens").GetInt64());
            Assert.AreEqual(JsonValueKind.Null, terminal.RootElement.GetProperty("structuredOutputUtf8").ValueKind);

            using var transcript = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "transcript.json")));
            Assert.AreEqual(0, transcript.RootElement.GetProperty("performances").GetArrayLength());
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task FailedReceiptWithoutUsage_CommitsExplicitReservationFallback()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-G3-UNKNOWN-USAGE");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider((attempt, _) =>
                RoleAttemptReceipt.TechnicalFailure(attempt, "provider-incomplete", "resp-unknown", "gpt-5.6-sol"));
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(100),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.IsTrue(result.HasUnknownProviderUsage);
            Assert.IsTrue(result.EstimatedSpendUsd > 0m);
            Assert.AreEqual(E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions, result.SpendEstimateStatus);

            var events = File.ReadAllText(Path.Combine(root, "events.ndjson"));
            Assert.IsTrue(events.Contains("\"usageKnown\":false", StringComparison.Ordinal));
            Assert.IsTrue(events.Contains("\"hasUnknownProviderUsage\":true", StringComparison.Ordinal));
            Assert.IsFalse(events.Contains("spend.released", StringComparison.Ordinal));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public void OutOfTierReportedUsage_IsMarkedOutsideVerifiedPricingInsteadOfRepriced()
    {
        var ledger = new E0ASpendLedger(E0ATestSupport.Pricing());
        var reservation = ledger.Reserve(100, E0ARunEnvelope.RoleMaxOutputTokens);

        var reconciliation = ledger.Reconcile(
            reservation,
            new E0AUsage(E0APricingPolicy.StandardTierMaxInputTokens + 1, 1));

        Assert.AreEqual(E0ASpendEstimateStatus.OutsideVerifiedInputTier, reconciliation.EstimateStatus);
        Assert.IsFalse(reconciliation.PricingAssumptionsValid);
        Assert.IsTrue(reconciliation.UsageKnown);
        Assert.IsTrue(reconciliation.ReservationExceeded);
        Assert.AreEqual(reservation.ReservedUsd, reconciliation.EstimatedUsd);
        Assert.AreEqual(E0ASpendEstimateStatus.OutsideVerifiedInputTier, ledger.EstimateStatus);
        Assert.IsFalse(ledger.HasActiveReservation);
    }

    [TestMethod]
    public void PricingAssumptions_RejectUnrepresentableTinyAndExtremeRates()
    {
        var tooSmall = new E0APricingAssumptions(
            0.00000000000000000000001m,
            0m,
            1m);
        var tooLarge = new E0APricingAssumptions(
            decimal.MaxValue,
            0m,
            decimal.MaxValue);

        Assert.Throws<E0AHarnessException>(() => tooSmall.Validate());
        Assert.Throws<E0AHarnessException>(() => tooLarge.Validate());
    }

    [TestMethod]
    public void ReservationActivity_IsTrackedIndependentlyFromAmountSentinel()
    {
        var ledger = new E0ASpendLedger(E0ATestSupport.Pricing());
        var reservation = ledger.Reserve(0, 1);

        Assert.IsTrue(ledger.HasActiveReservation);
        Assert.IsTrue(ledger.ReservedUsd > 0m);

        ledger.Release(reservation);

        Assert.IsFalse(ledger.HasActiveReservation);
        Assert.AreEqual(0m, ledger.ReservedUsd);
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

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private sealed class JsonHandler : HttpMessageHandler
    {
        private readonly string _json;
        internal JsonHandler(string json) => _json = json;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            });
    }
}
