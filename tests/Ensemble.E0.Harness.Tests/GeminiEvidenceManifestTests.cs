using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiEvidenceManifestTests
{
    [TestMethod]
    public void Manifest_BindsGeminiComparisonTransportShadowPricingThinkingAndQuota()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var state = E0ATestSupport.Genesis();
            var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
            _ = E0ATestSupport.Evidence(
                root,
                RunId.From("E0A-GEMINI-MANIFEST"),
                envelope,
                state);

            var text = File.ReadAllText(Path.Combine(root, "manifest.json"));
            using var document = JsonDocument.Parse(text);
            var manifest = document.RootElement;

            Assert.AreEqual(
                E0AEvidenceContracts.GeminiReferenceAmendment,
                manifest.GetProperty("referenceEnvelopeBlueprint").GetString());
            Assert.AreEqual(
                E0AEvidenceContracts.GeminiApprovedAmendmentCommit,
                manifest.GetProperty("approvedBlueprintCommit").GetString());
            Assert.AreEqual(
                $"E0A-GEMINI-COMPARISON-2026-09-07:CREATIVE-NONE:{E0AGeminiModelCatalog.Flash25NoneId}",
                manifest.GetProperty("referenceConfigurationIdentity").GetString());
            Assert.AreEqual(E0AGeminiModelCatalog.Flash25NoneId, manifest.GetProperty("providerProfileId").GetString());

            var pricing = manifest.GetProperty("pricing");
            Assert.AreEqual(E0AGeminiPricingPolicy.SourceUri, pricing.GetProperty("sourceUri").GetString());
            Assert.AreEqual(E0AGeminiPricingPolicy.VerifiedOn, pricing.GetProperty("verifiedOn").GetString());
            Assert.AreEqual(E0AGeminiPricingPolicy.SnapshotValidThrough, pricing.GetProperty("snapshotValidThrough").GetString());
            Assert.IsTrue(pricing.GetProperty("shadowEstimateOnly").GetBoolean());
            Assert.AreEqual(E0AGeminiPricingPolicy.PublishedPaidInputUsdPerMillionTokens, pricing.GetProperty("publishedInputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(E0AGeminiPricingPolicy.PublishedPaidCachedInputUsdPerMillionTokens, pricing.GetProperty("publishedCachedInputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(E0AGeminiPricingPolicy.PublishedPaidOutputUsdPerMillionTokens, pricing.GetProperty("publishedOutputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(E0AGeminiProviderPolicy.ModelInputTokenLimit, pricing.GetProperty("modelInputTokenLimit").GetInt64());
            Assert.AreEqual(E0AGeminiProviderPolicy.ModelOutputTokenLimit, pricing.GetProperty("modelOutputTokenLimit").GetInt32());
            Assert.AreEqual(E0AGeminiPricingPolicy.PublishedPaidInputUsdPerMillionTokens, pricing.GetProperty("inputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(E0AGeminiPricingPolicy.PublishedPaidInputUsdPerMillionTokens, pricing.GetProperty("cachedInputUsdPerMillionTokens").GetDecimal());
            Assert.AreEqual(E0AGeminiPricingPolicy.PublishedPaidOutputUsdPerMillionTokens, pricing.GetProperty("outputUsdPerMillionTokens").GetDecimal());
            Assert.IsFalse(pricing.TryGetProperty("promotionalPricingGuaranteedThrough", out _));
            Assert.IsFalse(pricing.TryGetProperty("cacheWriteMultiplier", out _));

            var transport = manifest.GetProperty("providerTransport");
            Assert.AreEqual("generateContent/streamGenerateContent", transport.GetProperty("api").GetString());
            Assert.AreEqual("models.countTokens(generateContentRequest)", transport.GetProperty("inputTokenCounter").GetString());
            Assert.IsFalse(transport.GetProperty("requestStore").GetBoolean());
            Assert.AreEqual(E0AGeminiProviderPolicy.ServiceTier, transport.GetProperty("serviceTier").GetString());
            Assert.IsFalse(transport.GetProperty("explicitCacheObject").GetBoolean());
            Assert.IsTrue(transport.GetProperty("implicitCachingProviderManaged").GetBoolean());
            Assert.AreEqual(E0AGeminiProviderPolicy.IntendedGeneratedTokenCeiling, transport.GetProperty("intendedGeneratedTokenCeiling").GetInt32());
            Assert.AreEqual(E0AGeminiModelCatalog.Flash25NoneId, transport.GetProperty("providerProfileId").GetString());
            Assert.AreEqual(5, transport.GetProperty("requestsPerMinute").GetInt32());
            Assert.AreEqual(250_000L, transport.GetProperty("inputTokensPerMinute").GetInt64());
            Assert.AreEqual("unverified-pre-live", transport.GetProperty("requestsPerDay").GetString());
            Assert.IsFalse(transport.TryGetProperty("promptCacheMode", out _));

            var roles = manifest.GetProperty("roles").EnumerateArray().ToArray();
            Assert.AreEqual(3, roles.Length);
            Assert.AreEqual("Budget", roles[0].GetProperty("thinkingControl").GetString());
            Assert.AreEqual(0, roles[0].GetProperty("thinkingBudgetTokens").GetInt32());
            Assert.AreEqual(JsonValueKind.Null, roles[0].GetProperty("thinkingLevel").ValueKind);
            Assert.AreEqual(E0ARunEnvelope.RoleMaxOutputTokens, roles[0].GetProperty("maxOutputTokens").GetInt32());
            Assert.AreEqual(E0AGeminiProviderPolicy.IntegrityThinkingBudgetTokens, roles[1].GetProperty("thinkingBudgetTokens").GetInt32());
            Assert.AreEqual(E0ARunEnvelope.RoleMaxOutputTokens, roles[1].GetProperty("maxOutputTokens").GetInt32());
            Assert.AreEqual(E0AGeminiProviderPolicy.ModelOutputTokenLimit, roles[1].GetProperty("reservationOutputTokens").GetInt32());
            Assert.AreEqual(0, roles[2].GetProperty("thinkingBudgetTokens").GetInt32());

            Assert.IsFalse(text.Contains("OPENAI_API_KEY", StringComparison.Ordinal));
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
}
