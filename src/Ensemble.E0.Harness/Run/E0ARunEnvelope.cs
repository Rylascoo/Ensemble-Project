using System.Collections.Immutable;
using Ensemble.E0.Core.StateInterpreter;

namespace Ensemble.E0.Harness.Run;

internal enum E0ARole
{
    Performer = 1,
    Integrity = 2,
    Interpreter = 3
}

internal enum E0AReasoningLevel
{
    None = 1,
    Low = 2,
    Medium = 3,
    High = 4
}

internal static class E0AProviderTransportPolicy
{
    internal const string PromptCacheMode = "explicit";
}

// Historical Proposal 0.15 OpenAI pricing snapshot. It remains evidence for the
// validated OpenAI implementation lineage but is no longer the active live-host
// pricing authority after the approved Gemini normative amendment.
internal static class E0APricingPolicy
{
    internal const string SourceUri = "https://developers.openai.com/api/docs/models/gpt-5.6-sol";
    internal const string VerifiedOn = "2026-09-06";
    internal const string PromotionalPricingGuaranteedThrough = "2026-11-21";
    internal const decimal PublishedInputUsdPerMillionTokens = 4.00m;
    internal const decimal PublishedCachedInputUsdPerMillionTokens = 0.40m;
    internal const decimal PublishedOutputUsdPerMillionTokens = 20.00m;
    internal const decimal CacheWriteMultiplier = 1.25m;
    internal const long StandardTierMaxInputTokens = 272_000;

    internal static E0APricingAssumptions ConservativePricing { get; } = new(
        PublishedInputUsdPerMillionTokens * CacheWriteMultiplier,
        PublishedCachedInputUsdPerMillionTokens,
        PublishedOutputUsdPerMillionTokens);

    internal static void RequireNonStaleSnapshot(DateTimeOffset now)
    {
        var guaranteedThrough = new DateOnly(2026, 11, 21);
        var currentDate = DateOnly.FromDateTime(now.UtcDateTime);
        if (currentDate > guaranteedThrough)
        {
            throw new E0AHarnessException(
                "E0-A pricing snapshot is outside the provider's published promotional guarantee and must be re-verified before inference.");
        }
    }
}

internal static class E0AGeminiProviderPolicy
{
    internal const string Provider = "GoogleGemini";
    internal const string Model = "gemini-2.5-flash";
    internal const string ServiceTier = "standard";
    internal const int CreativeThinkingBudgetTokens = 0;
    internal const int IntegrityThinkingBudgetTokens = 3_584;
    internal const int IntegrityCandidateMaxOutputTokens = 512;
    internal const int IntendedGeneratedTokenCeiling = 4_096;
    internal const long ModelInputTokenLimit = 1_048_576;
    internal const int ModelOutputTokenLimit = 65_536;

    internal static int ThinkingBudgetTokens(E0ARoleProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        if (!string.Equals(profile.Provider, Provider, StringComparison.Ordinal) ||
            !string.Equals(profile.Model, Model, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Gemini thinking policy received a non-Gemini profile.");
        }

        return profile.Role switch
        {
            E0ARole.Performer when profile.Reasoning == E0AReasoningLevel.None => CreativeThinkingBudgetTokens,
            E0ARole.Interpreter when profile.Reasoning == E0AReasoningLevel.None => CreativeThinkingBudgetTokens,
            E0ARole.Integrity when profile.Reasoning == E0AReasoningLevel.High => IntegrityThinkingBudgetTokens,
            _ => throw new E0AHarnessException("E0-A Gemini role reasoning is outside the approved normative amendment.")
        };
    }
}

internal static class E0AGeminiPricingPolicy
{
    internal const string SourceUri = "https://ai.google.dev/gemini-api/docs/pricing";
    internal const string VerifiedOn = "2026-09-06";
    internal const string SnapshotValidThrough = "2026-09-13";
    internal const decimal PublishedPaidInputUsdPerMillionTokens = 0.30m;
    internal const decimal PublishedPaidCachedInputUsdPerMillionTokens = 0.03m;
    internal const decimal PublishedPaidOutputUsdPerMillionTokens = 2.50m;

    // The active AI Studio free-tier route is expected to bill USD 0, but the
    // deterministic E0-A budget remains active using paid standard-tier rates as
    // a conservative shadow estimate. Cached input is deliberately estimated at
    // the full uncached input rate so implicit cache savings never weaken the cap.
    internal static E0APricingAssumptions ConservativeShadowPricing { get; } = new(
        PublishedPaidInputUsdPerMillionTokens,
        PublishedPaidInputUsdPerMillionTokens,
        PublishedPaidOutputUsdPerMillionTokens);

    internal static void RequireNonStaleSnapshot(DateTimeOffset now)
    {
        var validThrough = new DateOnly(2026, 9, 13);
        var currentDate = DateOnly.FromDateTime(now.UtcDateTime);
        if (currentDate > validThrough)
        {
            throw new E0AHarnessException(
                "E0-A Gemini pricing/data-use snapshot is stale and must be re-verified before inference.");
        }
    }
}

internal static class E0AProviderBudgetPolicy
{
    internal static int ReservationOutputTokens(E0ARoleProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        if (string.Equals(profile.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal) &&
            profile.Role == E0ARole.Integrity)
        {
            // Gemini thinkingBudget is advisory and may overflow. Reserve against
            // the model's published output-token limit rather than the requested
            // 3,584 thinking budget so the USD ceiling stays fail-closed.
            return E0AGeminiProviderPolicy.ModelOutputTokenLimit;
        }
        return profile.MaxOutputTokens;
    }
}

internal static class E0AProviderUsagePolicy
{
    internal static string? Violation(E0ARoleProfile profile, E0AUsage usage)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(usage);
        usage.Validate();

        if (!string.Equals(profile.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal))
        {
            return null;
        }

        var candidateTokens = usage.OutputTokens - usage.ReasoningTokens;
        if (candidateTokens > profile.MaxOutputTokens)
        {
            return "gemini-candidate-output-overrun";
        }
        if (usage.CachedInputTokens != 0)
        {
            return "gemini-implicit-cache-hit";
        }
        if (usage.CacheWriteTokens != 0)
        {
            return "gemini-unexpected-cache-write";
        }
        if (profile.Role is E0ARole.Performer or E0ARole.Interpreter)
        {
            return usage.ReasoningTokens == 0
                ? null
                : "gemini-thinking-not-disabled";
        }
        if (profile.Role == E0ARole.Integrity &&
            usage.OutputTokens > E0AGeminiProviderPolicy.IntendedGeneratedTokenCeiling)
        {
            return "gemini-generated-token-overrun";
        }
        return null;
    }
}

internal sealed record E0APricingAssumptions(
    decimal InputUsdPerMillionTokens,
    decimal CachedInputUsdPerMillionTokens,
    decimal OutputUsdPerMillionTokens)
{
    internal void Validate()
    {
        if (InputUsdPerMillionTokens <= 0m ||
            CachedInputUsdPerMillionTokens < 0m ||
            OutputUsdPerMillionTokens <= 0m ||
            CachedInputUsdPerMillionTokens > InputUsdPerMillionTokens)
        {
            throw new E0AHarnessException("E0-A pricing assumptions are invalid.");
        }
    }
}

internal sealed record E0ARoleProfile(
    E0ARole Role,
    string Provider,
    string Model,
    E0AReasoningLevel Reasoning,
    bool Stream,
    int MaxOutputTokens,
    string ServiceTier)
{
    internal void Validate()
    {
        if (!Enum.IsDefined(Role) ||
            !Enum.IsDefined(Reasoning) ||
            string.IsNullOrWhiteSpace(Provider) ||
            string.IsNullOrWhiteSpace(Model) ||
            string.IsNullOrWhiteSpace(ServiceTier) ||
            MaxOutputTokens <= 0)
        {
            throw new E0AHarnessException("E0-A role profile is invalid.");
        }
    }
}

internal sealed class E0ARunEnvelope
{
    private static readonly ImmutableArray<StateMutationDomain> AutoApprove =
        ImmutableArray.Create(
            StateMutationDomain.UnresolvedProposition,
            StateMutationDomain.CharacterBelief,
            StateMutationDomain.CharacterSuspicion,
            StateMutationDomain.CharacterGoal,
            StateMutationDomain.CharacterCircumstance,
            StateMutationDomain.CharacterClaim,
            StateMutationDomain.Pressure);

    private E0ARunEnvelope(
        string variant,
        E0ARoleProfile performer,
        E0ARoleProfile integrity,
        E0ARoleProfile interpreter,
        E0APricingAssumptions pricing)
    {
        Variant = variant;
        Performer = performer;
        Integrity = integrity;
        Interpreter = interpreter;
        Pricing = pricing;
    }

    internal const int AcceptedTurnCap = 12;
    internal const int AttemptsPerRoleInvocation = 1;
    internal const int AutomaticRetries = 0;
    internal const int AttemptTimeoutSeconds = 300;
    internal const decimal EstimatedSpendCeilingUsd = 5.00m;
    internal const int RoleMaxOutputTokens = 4096;

    internal string Variant { get; }
    internal E0ARoleProfile Performer { get; }
    internal E0ARoleProfile Integrity { get; }
    internal E0ARoleProfile Interpreter { get; }
    internal E0APricingAssumptions Pricing { get; }
    internal ImmutableArray<StateMutationDomain> AutoApproveDomains => AutoApprove;
    internal long MaxInputTokens =>
        string.Equals(Performer.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal)
            ? E0AGeminiProviderPolicy.ModelInputTokenLimit
            : E0APricingPolicy.StandardTierMaxInputTokens;

    // Proposal 0.15 historical constructors remain available for fake regression
    // coverage and evidence replay. They are not live-host authorization after the
    // Gemini normative amendment.
    internal static E0ARunEnvelope CreativeNone(E0APricingAssumptions pricing) =>
        CreateOpenAI("CREATIVE-NONE", E0AReasoningLevel.None, pricing);

    internal static E0ARunEnvelope CreativeLow(E0APricingAssumptions pricing) =>
        CreateOpenAI("CREATIVE-LOW", E0AReasoningLevel.Low, pricing);

    internal static E0ARunEnvelope CreativeMedium(E0APricingAssumptions pricing) =>
        CreateOpenAI("CREATIVE-MEDIUM", E0AReasoningLevel.Medium, pricing);

    internal static E0ARunEnvelope CreativeHigh(E0APricingAssumptions pricing) =>
        CreateOpenAI("CREATIVE-HIGH", E0AReasoningLevel.High, pricing);

    internal static E0ARunEnvelope GeminiNormativeReference(E0APricingAssumptions pricing)
    {
        ArgumentNullException.ThrowIfNull(pricing);
        var envelope = new E0ARunEnvelope(
            "CREATIVE-NONE",
            new E0ARoleProfile(
                E0ARole.Performer,
                E0AGeminiProviderPolicy.Provider,
                E0AGeminiProviderPolicy.Model,
                E0AReasoningLevel.None,
                true,
                RoleMaxOutputTokens,
                E0AGeminiProviderPolicy.ServiceTier),
            new E0ARoleProfile(
                E0ARole.Integrity,
                E0AGeminiProviderPolicy.Provider,
                E0AGeminiProviderPolicy.Model,
                E0AReasoningLevel.High,
                false,
                E0AGeminiProviderPolicy.IntegrityCandidateMaxOutputTokens,
                E0AGeminiProviderPolicy.ServiceTier),
            new E0ARoleProfile(
                E0ARole.Interpreter,
                E0AGeminiProviderPolicy.Provider,
                E0AGeminiProviderPolicy.Model,
                E0AReasoningLevel.None,
                true,
                RoleMaxOutputTokens,
                E0AGeminiProviderPolicy.ServiceTier),
            pricing);
        envelope.Validate();
        return envelope;
    }

    internal void Validate()
    {
        Performer.Validate();
        Integrity.Validate();
        Interpreter.Validate();
        Pricing.Validate();

        if (!string.Equals(Performer.Provider, Integrity.Provider, StringComparison.Ordinal) ||
            !string.Equals(Performer.Provider, Interpreter.Provider, StringComparison.Ordinal) ||
            !string.Equals(Performer.Model, Integrity.Model, StringComparison.Ordinal) ||
            !string.Equals(Performer.Model, Interpreter.Model, StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, Integrity.ServiceTier, StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, Interpreter.ServiceTier, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A reference provider/model/tier configuration is inconsistent.");
        }

        if (string.Equals(Performer.Provider, "OpenAI", StringComparison.Ordinal))
        {
            ValidateHistoricalOpenAI();
            return;
        }
        if (string.Equals(Performer.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal))
        {
            ValidateGeminiNormative();
            return;
        }
        throw new E0AHarnessException("E0-A reference provider is not approved.");
    }

    private void ValidateHistoricalOpenAI()
    {
        if (Variant is not "CREATIVE-NONE" and
            not "CREATIVE-LOW" and
            not "CREATIVE-MEDIUM" and
            not "CREATIVE-HIGH" ||
            !string.Equals(Performer.Model, "gpt-5.6-sol", StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, "default", StringComparison.Ordinal) ||
            Performer.Reasoning != Interpreter.Reasoning ||
            Integrity.Reasoning != E0AReasoningLevel.High ||
            Performer.MaxOutputTokens != RoleMaxOutputTokens ||
            Integrity.MaxOutputTokens != RoleMaxOutputTokens ||
            Interpreter.MaxOutputTokens != RoleMaxOutputTokens ||
            !Performer.Stream || Integrity.Stream || !Interpreter.Stream)
        {
            throw new E0AHarnessException("E0-A historical OpenAI role configuration is inconsistent.");
        }
    }

    private void ValidateGeminiNormative()
    {
        if (Variant != "CREATIVE-NONE" ||
            !string.Equals(Performer.Model, E0AGeminiProviderPolicy.Model, StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, E0AGeminiProviderPolicy.ServiceTier, StringComparison.Ordinal) ||
            Performer.Reasoning != E0AReasoningLevel.None ||
            Interpreter.Reasoning != E0AReasoningLevel.None ||
            Integrity.Reasoning != E0AReasoningLevel.High ||
            Performer.MaxOutputTokens != RoleMaxOutputTokens ||
            Integrity.MaxOutputTokens != E0AGeminiProviderPolicy.IntegrityCandidateMaxOutputTokens ||
            Interpreter.MaxOutputTokens != RoleMaxOutputTokens ||
            !Performer.Stream || Integrity.Stream || !Interpreter.Stream ||
            E0AGeminiProviderPolicy.ThinkingBudgetTokens(Performer) != 0 ||
            E0AGeminiProviderPolicy.ThinkingBudgetTokens(Interpreter) != 0 ||
            E0AGeminiProviderPolicy.ThinkingBudgetTokens(Integrity) != E0AGeminiProviderPolicy.IntegrityThinkingBudgetTokens)
        {
            throw new E0AHarnessException("E0-A Gemini normative role configuration is inconsistent.");
        }
    }

    private static E0ARunEnvelope CreateOpenAI(
        string variant,
        E0AReasoningLevel creativeReasoning,
        E0APricingAssumptions pricing)
    {
        ArgumentNullException.ThrowIfNull(pricing);
        var envelope = new E0ARunEnvelope(
            variant,
            new E0ARoleProfile(E0ARole.Performer, "OpenAI", "gpt-5.6-sol", creativeReasoning, true, RoleMaxOutputTokens, "default"),
            new E0ARoleProfile(E0ARole.Integrity, "OpenAI", "gpt-5.6-sol", E0AReasoningLevel.High, false, RoleMaxOutputTokens, "default"),
            new E0ARoleProfile(E0ARole.Interpreter, "OpenAI", "gpt-5.6-sol", creativeReasoning, true, RoleMaxOutputTokens, "default"),
            pricing);
        envelope.Validate();
        return envelope;
    }
}

internal sealed class E0AHarnessException : Exception
{
    internal E0AHarnessException(string message) : base(message) { }
}
