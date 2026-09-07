using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0AHardeningLiveHostPreservationTests
{
    [TestMethod]
    public void HistoricalOpenAIPricingPolicy_FailsClosedAfterPublishedPromotionalGuarantee()
    {
        E0APricingPolicy.RequireNonStaleSnapshot(new DateTimeOffset(2026, 11, 21, 23, 59, 59, TimeSpan.Zero));
        Assert.Throws<E0AHarnessException>(() =>
            E0APricingPolicy.RequireNonStaleSnapshot(new DateTimeOffset(2026, 11, 22, 0, 0, 0, TimeSpan.Zero)));
    }

    [TestMethod]
    public void HistoricalOpenAISpendPreflight_BlocksLongContextPricingTierBeforeInference()
    {
        var ledger = new E0ASpendLedger(E0APricingPolicy.ConservativePricing);
        var boundary = ledger.Reserve(E0APricingPolicy.StandardTierMaxInputTokens, 1);
        Assert.AreEqual(E0APricingPolicy.StandardTierMaxInputTokens, boundary.InputTokens);
        ledger.Release(boundary);

        Assert.Throws<E0ABudgetExceededException>(() =>
            ledger.Reserve(E0APricingPolicy.StandardTierMaxInputTokens + 1, 1));
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void LiveHost_HttpTransportCannotPreemptFrozenAttemptDeadline()
    {
        using var http = E0AReferenceRunHost.CreateProviderHttpClient();

        Assert.AreEqual(Timeout.InfiniteTimeSpan, http.Timeout);
    }
}
