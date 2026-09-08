namespace Ensemble.E0.Harness.Run;

internal enum E0AGeminiThinkingControlKind
{
    Budget = 1,
    Level = 2
}

internal sealed record E0AGeminiModelProfile(
    string ProfileId,
    string Model,
    string Variant,
    E0AGeminiThinkingControlKind ThinkingControl,
    int RequestsPerMinute,
    long InputTokensPerMinute,
    decimal PublishedPaidInputUsdPerMillionTokens,
    decimal PublishedPaidCachedInputUsdPerMillionTokens,
    decimal PublishedPaidOutputUsdPerMillionTokens,
    long ModelInputTokenLimit,
    int ModelOutputTokenLimit,
    bool CreativeRequiresZeroReasoning)
{
    internal E0APricingAssumptions ConservativeShadowPricing => new(
        PublishedPaidInputUsdPerMillionTokens,
        PublishedPaidInputUsdPerMillionTokens,
        PublishedPaidOutputUsdPerMillionTokens);

    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(ProfileId) ||
            string.IsNullOrWhiteSpace(Model) ||
            string.IsNullOrWhiteSpace(Variant) ||
            !Enum.IsDefined(ThinkingControl) ||
            RequestsPerMinute <= 0 ||
            InputTokensPerMinute <= 0 ||
            PublishedPaidInputUsdPerMillionTokens <= 0m ||
            PublishedPaidCachedInputUsdPerMillionTokens < 0m ||
            PublishedPaidOutputUsdPerMillionTokens <= 0m ||
            PublishedPaidCachedInputUsdPerMillionTokens > PublishedPaidInputUsdPerMillionTokens ||
            ModelInputTokenLimit <= 0 ||
            ModelOutputTokenLimit <= 0)
        {
            throw new E0AHarnessException("E0-A Gemini model profile is invalid.");
        }
        ConservativeShadowPricing.Validate();
    }
}

internal static class E0AGeminiModelCatalog
{
    internal const string FlashLite25NoneId = "GEMINI-2.5-FLASH-LITE-NONE";
    internal const string FlashLite35MinimalId = "GEMINI-3.5-FLASH-LITE-MINIMAL";
    internal const string Flash25NoneId = "GEMINI-2.5-FLASH-NONE";

    internal static E0AGeminiModelProfile FlashLite25None { get; } = new(
        FlashLite25NoneId,
        "gemini-2.5-flash-lite",
        "CREATIVE-NONE",
        E0AGeminiThinkingControlKind.Budget,
        10,
        250_000,
        0.10m,
        0.01m,
        0.40m,
        1_048_576,
        65_536,
        CreativeRequiresZeroReasoning: true);

    internal static E0AGeminiModelProfile FlashLite35Minimal { get; } = new(
        FlashLite35MinimalId,
        "gemini-3.5-flash-lite",
        "CREATIVE-MINIMAL",
        E0AGeminiThinkingControlKind.Level,
        15,
        250_000,
        0.30m,
        0.03m,
        2.50m,
        1_048_576,
        65_536,
        CreativeRequiresZeroReasoning: false);

    internal static E0AGeminiModelProfile Flash25None { get; } = new(
        Flash25NoneId,
        "gemini-2.5-flash",
        "CREATIVE-NONE",
        E0AGeminiThinkingControlKind.Budget,
        5,
        250_000,
        0.30m,
        0.03m,
        2.50m,
        1_048_576,
        65_536,
        CreativeRequiresZeroReasoning: true);

    internal static IReadOnlyList<E0AGeminiModelProfile> All { get; } =
        new[] { FlashLite25None, FlashLite35Minimal, Flash25None };

    internal static E0AGeminiModelProfile ForProfileId(string profileId)
    {
        if (string.IsNullOrWhiteSpace(profileId))
        {
            throw new E0AHarnessException("E0-A Gemini provider profile is required.");
        }
        var profile = All.SingleOrDefault(x => string.Equals(x.ProfileId, profileId, StringComparison.Ordinal))
            ?? throw new E0AHarnessException("E0-A Gemini provider profile is not approved by the model-comparison amendment.");
        profile.Validate();
        return profile;
    }

    internal static E0AGeminiModelProfile ForModel(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new E0AHarnessException("E0-A Gemini model identity is required.");
        }
        var profile = All.SingleOrDefault(x => string.Equals(x.Model, model, StringComparison.Ordinal))
            ?? throw new E0AHarnessException("E0-A Gemini model is not approved by the model-comparison amendment.");
        profile.Validate();
        return profile;
    }
}
