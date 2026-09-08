using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiModelComparisonTests
{
    [TestMethod]
    public void Gemini25FlashLite_UsesTrueThinkingOffAndRouteSpecificPricing()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-NONE",
            E0AGeminiModelCatalog.FlashLite25NoneId);
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(
            RunId.From("E0A-25-LITE-SHAPE"),
            1,
            envelope.Performer,
            context);

        using var body = JsonDocument.Parse(attempt.RequestBody);
        var thinking = body.RootElement
            .GetProperty("generationConfig")
            .GetProperty("thinkingConfig");

        Assert.AreEqual(0, thinking.GetProperty("thinkingBudget").GetInt32());
        Assert.IsFalse(thinking.TryGetProperty("thinkingLevel", out _));
        Assert.AreEqual(0.10m, envelope.Pricing.InputUsdPerMillionTokens);
        Assert.AreEqual(0.10m, envelope.Pricing.CachedInputUsdPerMillionTokens);
        Assert.AreEqual(0.40m, envelope.Pricing.OutputUsdPerMillionTokens);
    }

    [TestMethod]
    public void Gemini35FlashLite_UsesMinimalAndHighThinkingLevelsWithoutBudgetField()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        var cycle = E0ATestSupport.Cycle();
        var continuity = DeterministicE0CausalCycle.ComposeContext(cycle);
        var context = continuity.ContextEvaluation.Packet;
        var performer = E0ARequestBuilder.Performer(
            RunId.From("E0A-35-LITE-PERFORMER"),
            1,
            envelope.Performer,
            context);
        var candidate = E0ATestSupport.Candidate(context, "No.");
        var input = Ensemble.E0.Core.Integrity.IntegrityCandidateInput.Bind(context, candidate);
        var packet = E0AIntegrityAssessmentPacketBuilder.Build(
            cycle.ProductionState,
            continuity.AccessEvaluation,
            context,
            candidate);
        var integrity = E0ARequestBuilder.Integrity(
            RunId.From("E0A-35-LITE-INTEGRITY"),
            1,
            envelope.Integrity,
            context,
            input.CandidateContentHash,
            packet);

        using var performerBody = JsonDocument.Parse(performer.RequestBody);
        using var integrityBody = JsonDocument.Parse(integrity.RequestBody);
        var performerThinking = performerBody.RootElement.GetProperty("generationConfig").GetProperty("thinkingConfig");
        var integrityThinking = integrityBody.RootElement.GetProperty("generationConfig").GetProperty("thinkingConfig");

        Assert.AreEqual("minimal", performerThinking.GetProperty("thinkingLevel").GetString());
        Assert.IsFalse(performerThinking.TryGetProperty("thinkingBudget", out _));
        Assert.AreEqual("high", integrityThinking.GetProperty("thinkingLevel").GetString());
        Assert.IsFalse(integrityThinking.TryGetProperty("thinkingBudget", out _));
    }

    [TestMethod]
    public void Gemini35Evidence_LabelsMinimalArmQuotaThinkingAndPricingWithoutCredential()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var state = E0ATestSupport.Genesis();
            var envelope = E0AReferenceRunHost.CreateEnvelope(
                "CREATIVE-MINIMAL",
                E0AGeminiModelCatalog.FlashLite35MinimalId);
            _ = E0ATestSupport.Evidence(
                root,
                RunId.From("E0A-35-LITE-EVIDENCE"),
                envelope,
                state);

            var text = File.ReadAllText(Path.Combine(root, "manifest.json"));
            using var document = JsonDocument.Parse(text);
            var manifest = document.RootElement;
            Assert.AreEqual("CREATIVE-MINIMAL", manifest.GetProperty("variant").GetString());
            Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35MinimalId, manifest.GetProperty("providerProfileId").GetString());

            var pricing = manifest.GetProperty("pricing");
            Assert.AreEqual(0.30m, pricing.GetProperty("publishedInputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(0.03m, pricing.GetProperty("publishedCachedInputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(2.50m, pricing.GetProperty("publishedOutputUsdPerMillionTokens").GetDecimal());

            var transport = manifest.GetProperty("providerTransport");
            Assert.AreEqual(15, transport.GetProperty("requestsPerMinute").GetInt32());
            Assert.AreEqual(250_000L, transport.GetProperty("inputTokensPerMinute").GetInt64());
            Assert.AreEqual("all-gemini-api-http-requests", transport.GetProperty("rateDisciplineScope").GetString());

            var roles = manifest.GetProperty("roles").EnumerateArray().ToArray();
            Assert.AreEqual("Level", roles[0].GetProperty("thinkingControl").GetString());
            Assert.AreEqual(JsonValueKind.Null, roles[0].GetProperty("thinkingBudgetTokens").ValueKind);
            Assert.AreEqual("minimal", roles[0].GetProperty("thinkingLevel").GetString());
            Assert.AreEqual("high", roles[1].GetProperty("thinkingLevel").GetString());
            Assert.AreEqual(envelope.ModelProfile.ModelOutputTokenLimit, roles[0].GetProperty("reservationOutputTokens").GetInt32());

            Assert.IsFalse(text.Contains("GEMINI_API_KEY", StringComparison.Ordinal));
            Assert.IsFalse(text.Contains("x-goog-api-key", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [TestMethod]
    public async Task SmoothRateDiscipline_PacesEveryApiRequestAndUsesExactGenerationInput()
    {
        var clock = new FakeClock();
        using var discipline = new E0ASmoothGeminiRateDiscipline(
            E0AGeminiModelCatalog.FlashLite25None,
            clock);

        await discipline.BeforeRequestAsync(E0AGeminiRequestKind.CountTokens, null, CancellationToken.None);
        await discipline.BeforeRequestAsync(E0AGeminiRequestKind.Generation, 1_000, CancellationToken.None);
        await discipline.BeforeRequestAsync(E0AGeminiRequestKind.CountTokens, null, CancellationToken.None);
        await discipline.BeforeRequestAsync(E0AGeminiRequestKind.Generation, 1_000, CancellationToken.None);

        CollectionAssert.AreEqual(
            new[] { TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(6) },
            clock.Delays.ToArray());
    }

    [TestMethod]
    public async Task SmoothRateDiscipline_EnforcesExactRollingGenerationTpmWindow()
    {
        var clock = new FakeClock();
        using var discipline = new E0ASmoothGeminiRateDiscipline(
            E0AGeminiModelCatalog.FlashLite25None,
            clock);

        await discipline.BeforeRequestAsync(
            E0AGeminiRequestKind.Generation,
            200_000,
            CancellationToken.None);
        await discipline.BeforeRequestAsync(
            E0AGeminiRequestKind.Generation,
            100_000,
            CancellationToken.None);

        CollectionAssert.AreEqual(
            new[] { TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(54) },
            clock.Delays.ToArray());
    }

    [TestMethod]
    public async Task SmoothRateDiscipline_FailsClosedWhenOneGenerationExceedsProfileTpm()
    {
        var clock = new FakeClock();
        using var discipline = new E0ASmoothGeminiRateDiscipline(
            E0AGeminiModelCatalog.FlashLite35Minimal,
            clock);

        await Assert.ThrowsAsync<E0AHarnessException>(() =>
            discipline.BeforeRequestAsync(
                E0AGeminiRequestKind.Generation,
                250_001,
                CancellationToken.None));
        Assert.AreEqual(0, clock.Delays.Count);
    }

    private sealed class FakeClock : IE0AGeminiRateClock
    {
        private long _milliseconds;
        internal List<TimeSpan> Delays { get; } = new();

        public long GetTimestamp() => _milliseconds;

        public TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp) =>
            TimeSpan.FromMilliseconds(endingTimestamp - startingTimestamp);

        public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Delays.Add(delay);
            _milliseconds += checked((long)Math.Round(delay.TotalMilliseconds, MidpointRounding.AwayFromZero));
            return Task.CompletedTask;
        }
    }
}
