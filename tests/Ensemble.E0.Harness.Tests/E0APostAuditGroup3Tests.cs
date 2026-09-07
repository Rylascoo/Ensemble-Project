using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0APostAuditGroup3Tests
{
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
                    E0AGeminiProviderPolicy.Model,
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
                RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    "provider-incomplete",
                    "resp-unknown",
                    E0AGeminiProviderPolicy.Model));
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
            new E0AUsage(E0AGeminiProviderPolicy.ModelInputTokenLimit + 1, 1));

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

    [TestMethod]
    public void StaleReservationCannotReleaseNewReservationWithSameAmount()
    {
        var ledger = new E0ASpendLedger(E0ATestSupport.Pricing());
        var first = ledger.Reserve(100, 10);
        ledger.Release(first);
        var second = ledger.Reserve(100, 10);

        Assert.Throws<E0AHarnessException>(() => ledger.Release(first));
        Assert.IsTrue(ledger.HasActiveReservation);

        ledger.Release(second);
        Assert.IsFalse(ledger.HasActiveReservation);
    }

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
