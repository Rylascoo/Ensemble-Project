using System.Net;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Gemini;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiRpdModelSelectionTests
{
    [TestMethod]
    public void LiveCatalog_BindsRpdCapsAndExcludesHistoricalFlash25()
    {
        CollectionAssert.AreEqual(
            new[]
            {
                E0AGeminiModelCatalog.FlashLite35MinimalId,
                E0AGeminiModelCatalog.FlashLite31MinimalId,
                E0AGeminiModelCatalog.FlashLite25NoneId
            },
            E0AGeminiModelCatalog.All.Select(x => x.ProfileId).ToArray());

        AssertProfile(E0AGeminiModelCatalog.FlashLite35Minimal, 500, 12, 72);
        AssertProfile(E0AGeminiModelCatalog.FlashLite31Minimal, 500, 12, 72);
        AssertProfile(E0AGeminiModelCatalog.FlashLite25None, 20, 3, 18);

        Assert.IsFalse(E0AGeminiModelCatalog.Flash25None.LiveSelectable);
        Assert.IsFalse(E0AGeminiModelCatalog.All.Any(
            x => string.Equals(x.ProfileId, E0AGeminiModelCatalog.Flash25NoneId, StringComparison.Ordinal)));
        Assert.ThrowsException<E0AHarnessException>(() =>
            E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE", E0AGeminiModelCatalog.Flash25NoneId));
    }

    [TestMethod]
    public void Gemini31FlashLite_UsesMinimalHighThinkingAndRouteSpecificPricing()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite31MinimalId);
        var cycle = E0ATestSupport.Cycle();
        var continuity = DeterministicE0CausalCycle.ComposeContext(cycle);
        var context = continuity.ContextEvaluation.Packet;
        var performer = E0ARequestBuilder.Performer(
            RunId.From("E0A-31-LITE-PERFORMER"),
            1,
            envelope.Performer,
            context);
        var candidate = E0ATestSupport.Candidate(context, "No.");
        var input = IntegrityCandidateInput.Bind(context, candidate);
        var packet = E0AIntegrityAssessmentPacketBuilder.Build(
            cycle.ProductionState,
            continuity.AccessEvaluation,
            context,
            candidate);
        var integrity = E0ARequestBuilder.Integrity(
            RunId.From("E0A-31-LITE-INTEGRITY"),
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
        Assert.AreEqual(0.25m, envelope.Pricing.InputUsdPerMillionTokens);
        Assert.AreEqual(0.25m, envelope.Pricing.CachedInputUsdPerMillionTokens);
        Assert.AreEqual(1.50m, envelope.Pricing.OutputUsdPerMillionTokens);
        Assert.AreEqual(0.025m, envelope.ModelProfile.PublishedPaidCachedInputUsdPerMillionTokens);
        Assert.AreEqual(12, envelope.RunAcceptedTurnCap);
    }

    [TestMethod]
    public async Task Gemini25FlashLiteControl_StopsAfterThreeAcceptedTurns()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-25-LITE-THREE-TURN-CONTROL");
            var state = E0ATestSupport.Genesis();
            var envelope = E0AReferenceRunHost.CreateEnvelope(
                "CREATIVE-NONE",
                E0AGeminiModelCatalog.FlashLite25NoneId);
            var provider = new ScriptedProvider();
            var counter = new FixedTokenCounter();
            var evidence = E0ATestSupport.Evidence(root, runId, envelope, state);
            var driver = new E0AReferenceRunDriver(envelope, provider, counter, evidence);

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.AcceptedTurnCapReached, result.Status);
            Assert.AreEqual(3, result.AcceptedTurns);
            Assert.AreEqual(9, provider.Calls);
            Assert.AreEqual(9, counter.Calls);
            Assert.AreEqual(3, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories)
                .Count(path => path.Contains("ATTEMPT_PERFORMER", StringComparison.Ordinal)));

            using var summary = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "run.summary.json")));
            Assert.AreEqual(3, summary.RootElement.GetProperty("acceptedTurns").GetInt32());
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
    public void LiveManifests_BindRpdTurnCapWorstCaseAndAmendmentIdentity()
    {
        AssertManifest(
            E0AGeminiModelCatalog.FlashLite31MinimalId,
            "CREATIVE-MINIMAL",
            expectedRpd: 500,
            expectedCap: 12,
            expectedWorstCase: 72,
            expectedInputRate: 0.25m,
            expectedOutputRate: 1.50m);

        AssertManifest(
            E0AGeminiModelCatalog.FlashLite25NoneId,
            "CREATIVE-NONE",
            expectedRpd: 20,
            expectedCap: 3,
            expectedWorstCase: 18,
            expectedInputRate: 0.10m,
            expectedOutputRate: 0.40m);
    }

    [TestMethod]
    public async Task StreamingGemini31ThoughtSignature_IsAcceptedAndStripped()
    {
        var output = Encoding.UTF8.GetString(E0ATestSupport.PerformerOutput());
        var chunk = JsonSerializer.Serialize(new
        {
            candidates = new[]
            {
                new
                {
                    content = new
                    {
                        parts = new[] { new { text = output, thoughtSignature = "c2lnbmF0dXJl" } }
                    },
                    finishReason = "STOP"
                }
            },
            usageMetadata = new
            {
                promptTokenCount = 20,
                candidatesTokenCount = 10,
                thoughtsTokenCount = 4,
                cachedContentTokenCount = 0,
                totalTokenCount = 34
            },
            modelVersion = "gemini-3.1-flash-lite-20260907",
            responseId = "resp-31-signature-stream"
        });
        using var http = new HttpClient(new SingleResponseHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"data: {chunk}\n\n", Encoding.UTF8, "text/event-stream")
        }));
        var port = new GeminiGenerateContentPort(http, "test-key");
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite31MinimalId);
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(
            RunId.From("E0A-GEMINI-31-SIGNATURE-STREAM"),
            1,
            envelope.Performer,
            context);
        var diagnostics = new CollectingDiagnosticSink();

        var receipt = await port.ExecuteAsync(attempt, diagnostics, CancellationToken.None);

        Assert.AreEqual(E0ARoleAttemptOutcome.Success, receipt.Outcome);
        CollectionAssert.AreEqual(E0ATestSupport.PerformerOutput(), receipt.StructuredOutput!);
        Assert.AreEqual(1, diagnostics.Events.Count);
        Assert.IsFalse(diagnostics.Events[0].Contains("thoughtSignature", StringComparison.Ordinal));
        Assert.IsFalse(diagnostics.Events[0].Contains("c2lnbmF0dXJl", StringComparison.Ordinal));
    }

    private static void AssertProfile(
        E0AGeminiModelProfile profile,
        int expectedRpd,
        int expectedCap,
        int expectedWorstCase)
    {
        profile.Validate();
        Assert.IsTrue(profile.LiveSelectable);
        Assert.AreEqual(expectedRpd, profile.RequestsPerDay);
        Assert.AreEqual(expectedCap, profile.AcceptedTurnCap);
        Assert.AreEqual(
            expectedWorstCase,
            profile.AcceptedTurnCap * E0AGeminiModelCatalog.ConservativeProviderRequestsPerAcceptedTurn);
        Assert.IsTrue(expectedWorstCase <= profile.RequestsPerDay);
    }

    private static void AssertManifest(
        string profileId,
        string variant,
        int expectedRpd,
        int expectedCap,
        int expectedWorstCase,
        decimal expectedInputRate,
        decimal expectedOutputRate)
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var state = E0ATestSupport.Genesis();
            var envelope = E0AReferenceRunHost.CreateEnvelope(variant, profileId);
            _ = E0ATestSupport.Evidence(
                root,
                RunId.From($"E0A-RPD-MANIFEST-{expectedRpd}-{expectedCap}"),
                envelope,
                state);

            using var document = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "manifest.json")));
            var manifest = document.RootElement;
            Assert.AreEqual(E0AEvidenceContracts.GeminiRpdModelSelectionAmendment,
                manifest.GetProperty("referenceEnvelopeBlueprint").GetString());
            Assert.AreEqual(E0AEvidenceContracts.GeminiRpdModelSelectionApprovedCommit,
                manifest.GetProperty("approvedBlueprintCommit").GetString());
            Assert.AreEqual(expectedCap, manifest.GetProperty("acceptedTurnCap").GetInt32());

            var pricing = manifest.GetProperty("pricing");
            Assert.AreEqual(expectedInputRate, pricing.GetProperty("publishedInputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(expectedOutputRate, pricing.GetProperty("publishedOutputUsdPerMillionTokens").GetDecimal());

            var transport = manifest.GetProperty("providerTransport");
            Assert.AreEqual(expectedRpd, transport.GetProperty("requestsPerDay").GetInt32());
            Assert.AreEqual(expectedCap, transport.GetProperty("acceptedTurnCap").GetInt32());
            Assert.AreEqual(E0AGeminiModelCatalog.ConservativeProviderRequestsPerAcceptedTurn,
                transport.GetProperty("conservativeProviderRequestsPerAcceptedTurn").GetInt32());
            Assert.AreEqual(expectedWorstCase, transport.GetProperty("worstCaseRunProviderRequests").GetInt32());
            Assert.AreEqual("all-countTokens-and-generation-requests-conservatively-share-rpd",
                transport.GetProperty("rpdAdmissionPolicy").GetString());
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
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
