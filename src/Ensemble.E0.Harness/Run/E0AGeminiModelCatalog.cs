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
    int RequestsPerDay,
    int AcceptedTurnCap,
    decimal PublishedPaidInputUsdPerMillionTokens,
    decimal PublishedPaidCachedInputUsdPerMillionTokens,
    decimal PublishedPaidOutputUsdPerMillionTokens,
    long ModelInputTokenLimit,
    int ModelOutputTokenLimit,
    bool CreativeRequiresZeroReasoning,
    bool AllowsOpaqueThoughtSignature,
    bool LiveSelectable)
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
            RequestsPerDay <= 0 ||
            AcceptedTurnCap <= 0 ||
            AcceptedTurnCap > E0ARunEnvelope.AcceptedTurnCap ||
            PublishedPaidInputUsdPerMillionTokens <= 0m ||
            PublishedPaidCachedInputUsdPerMillionTokens < 0m ||
            PublishedPaidOutputUsdPerMillionTokens <= 0m ||
            PublishedPaidCachedInputUsdPerMillionTokens > PublishedPaidInputUsdPerMillionTokens ||
            ModelInputTokenLimit <= 0 ||
            ModelOutputTokenLimit <= 0 ||
            (ThinkingControl == E0AGeminiThinkingControlKind.Budget && AllowsOpaqueThoughtSignature) ||
            (LiveSelectable &&
             checked((long)AcceptedTurnCap * E0AGeminiModelCatalog.ConservativeProviderRequestsPerAcceptedTurn) > RequestsPerDay))
        {
            throw new E0AHarnessException("E0-A Gemini model profile is invalid.");
        }
        ConservativeShadowPricing.Validate();
    }
}

internal static class E0AGeminiModelCatalog
{
    internal const int ConservativeProviderRequestsPerAcceptedTurn = 6;

    internal const string FlashLite35MinimalId = "GEMINI-3.5-FLASH-LITE-MINIMAL";
    internal const string FlashLite31MinimalId = "GEMINI-3.1-FLASH-LITE-MINIMAL";
    internal const string FlashLite25NoneId = "GEMINI-2.5-FLASH-LITE-NONE";
    internal const string Flash25NoneId = "GEMINI-2.5-FLASH-NONE";

    internal static E0AGeminiModelProfile FlashLite35Minimal { get; } = new(
        FlashLite35MinimalId,
        "gemini-3.5-flash-lite",
        "CREATIVE-MINIMAL",
        E0AGeminiThinkingControlKind.Level,
        15,
        250_000,
        500,
        12,
        0.30m,
        0.03m,
        2.50m,
        1_048_576,
        65_536,
        CreativeRequiresZeroReasoning: false,
        AllowsOpaqueThoughtSignature: true,
        LiveSelectable: true);

    internal static E0AGeminiModelProfile FlashLite31Minimal { get; } = new(
        FlashLite31MinimalId,
        "gemini-3.1-flash-lite",
        "CREATIVE-MINIMAL",
        E0AGeminiThinkingControlKind.Level,
        15,
        250_000,
        500,
        12,
        0.25m,
        0.025m,
        1.50m,
        1_048_576,
        65_536,
        CreativeRequiresZeroReasoning: false,
        AllowsOpaqueThoughtSignature: true,
        LiveSelectable: true);

    internal static E0AGeminiModelProfile FlashLite25None { get; } = new(
        FlashLite25NoneId,
        "gemini-2.5-flash-lite",
        "CREATIVE-NONE",
        E0AGeminiThinkingControlKind.Budget,
        10,
        250_000,
        20,
        3,
        0.10m,
        0.01m,
        0.40m,
        1_048_576,
        65_536,
        CreativeRequiresZeroReasoning: true,
        AllowsOpaqueThoughtSignature: false,
        LiveSelectable: true);

    // Historical compatibility anchor for the original normative route. It is
    // deliberately not live-selectable under the RPD model-selection amendment.
    internal static E0AGeminiModelProfile Flash25None { get; } = new(
        Flash25NoneId,
        "gemini-2.5-flash",
        "CREATIVE-NONE",
        E0AGeminiThinkingControlKind.Budget,
        5,
        250_000,
        20,
        12,
        0.30m,
        0.03m,
        2.50m,
        1_048_576,
        65_536,
        CreativeRequiresZeroReasoning: true,
        AllowsOpaqueThoughtSignature: false,
        LiveSelectable: false);

    internal static IReadOnlyList<E0AGeminiModelProfile> All { get; } =
        new[] { FlashLite35Minimal, FlashLite31Minimal, FlashLite25None };

    internal static IReadOnlyList<E0AGeminiModelProfile> Known { get; } =
        new[] { FlashLite35Minimal, FlashLite31Minimal, FlashLite25None, Flash25None };

    internal static E0AGeminiModelProfile ForProfileId(string profileId)
    {
        if (string.IsNullOrWhiteSpace(profileId))
        {
            throw new E0AHarnessException("E0-A Gemini provider profile is required.");
        }
        var profile = All.SingleOrDefault(x => string.Equals(x.ProfileId, profileId, StringComparison.Ordinal))
            ?? throw new E0AHarnessException("E0-A Gemini provider profile is not approved for live selection.");
        profile.Validate();
        return profile;
    }

    internal static E0AGeminiModelProfile ForKnownProfileId(string profileId)
    {
        if (string.IsNullOrWhiteSpace(profileId))
        {
            throw new E0AHarnessException("E0-A Gemini provider profile is required.");
        }
        var profile = Known.SingleOrDefault(x => string.Equals(x.ProfileId, profileId, StringComparison.Ordinal))
            ?? throw new E0AHarnessException("E0-A Gemini provider profile is unknown.");
        profile.Validate();
        return profile;
    }

    internal static E0AGeminiModelProfile ForModel(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new E0AHarnessException("E0-A Gemini model identity is required.");
        }
        var profile = Known.SingleOrDefault(x => string.Equals(x.Model, model, StringComparison.Ordinal))
            ?? throw new E0AHarnessException("E0-A Gemini model is not approved by the current comparison authority.");
        profile.Validate();
        return profile;
    }
}
