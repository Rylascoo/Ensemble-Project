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
    High = 2,
    Minimal = 3
}

internal static class E0AGeminiProviderPolicy
{
    internal const string Provider = "GoogleGemini";
    internal const string Model = "gemini-2.5-flash"; // compatibility anchor for the original normative route
    internal const string ServiceTier = "standard";
    internal const int CreativeThinkingBudgetTokens = 0;
    internal const int IntegrityThinkingBudgetTokens = 3_584;
    internal const int IntegrityCandidateMaxOutputTokens = 4_096;
    internal const int IntendedGeneratedTokenCeiling = 4_096;
    internal const long ModelInputTokenLimit = 1_048_576;
    internal const int ModelOutputTokenLimit = 65_536;

    internal static E0AGeminiModelProfile ModelProfile(E0ARoleProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        if (!string.Equals(profile.Provider, Provider, StringComparison.Ordinal) ||
            !string.Equals(profile.ServiceTier, ServiceTier, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Gemini policy received a non-Gemini profile.");
        }
        return E0AGeminiModelCatalog.ForModel(profile.Model);
    }

    internal static int ThinkingBudgetTokens(E0ARoleProfile profile)
    {
        var model = ModelProfile(profile);
        if (model.ThinkingControl != E0AGeminiThinkingControlKind.Budget)
        {
            throw new E0AHarnessException("E0-A Gemini thinking-budget policy received a thinking-level profile.");
        }

        return profile.Role switch
        {
            E0ARole.Performer when profile.Reasoning == E0AReasoningLevel.None => CreativeThinkingBudgetTokens,
            E0ARole.Interpreter when profile.Reasoning == E0AReasoningLevel.None => CreativeThinkingBudgetTokens,
            E0ARole.Integrity when profile.Reasoning == E0AReasoningLevel.High => IntegrityThinkingBudgetTokens,
            _ => throw new E0AHarnessException("E0-A Gemini role reasoning is outside the approved model profile.")
        };
    }

    internal static string ThinkingLevel(E0ARoleProfile profile)
    {
        var model = ModelProfile(profile);
        if (model.ThinkingControl != E0AGeminiThinkingControlKind.Level)
        {
            throw new E0AHarnessException("E0-A Gemini thinking-level policy received a thinking-budget profile.");
        }

        return profile.Role switch
        {
            E0ARole.Performer when profile.Reasoning == E0AReasoningLevel.Minimal => "minimal",
            E0ARole.Interpreter when profile.Reasoning == E0AReasoningLevel.Minimal => "minimal",
            E0ARole.Integrity when profile.Reasoning == E0AReasoningLevel.High => "high",
            _ => throw new E0AHarnessException("E0-A Gemini role reasoning is outside the approved model profile.")
        };
    }
}

internal static class E0AGeminiPricingPolicy
{
    internal const string SourceUri = "https://ai.google.dev/gemini-api/docs/pricing";
    internal const string VerifiedOn = "2026-09-07";
    internal const string SnapshotValidThrough = "2026-09-14";
    internal const decimal PublishedPaidInputUsdPerMillionTokens = 0.30m;
    internal const decimal PublishedPaidCachedInputUsdPerMillionTokens = 0.03m;
    internal const decimal PublishedPaidOutputUsdPerMillionTokens = 2.50m;

    // Compatibility anchor for existing tests and the original 2.5 Flash route.
    internal static E0APricingAssumptions ConservativeShadowPricing { get; } =
        E0AGeminiModelCatalog.Flash25None.ConservativeShadowPricing;

    internal static E0APricingAssumptions ConservativeShadowPricingFor(E0AGeminiModelProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        profile.Validate();
        return profile.ConservativeShadowPricing;
    }

    internal static void RequireNonStaleSnapshot(DateTimeOffset now)
    {
        var validThrough = new DateOnly(2026, 9, 14);
        var currentDate = DateOnly.FromDateTime(now.UtcDateTime);
        if (currentDate > validThrough)
        {
            throw new E0AHarnessException(
                "E0-A Gemini pricing/data-use/quota snapshot is stale and must be re-verified before inference.");
        }
    }
}

internal static class E0AProviderBudgetPolicy
{
    internal static int ReservationOutputTokens(E0ARoleProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        var model = E0AGeminiProviderPolicy.ModelProfile(profile);

        // Thinking-level profiles can reason even at minimal, so every 3.5 role
        // reserves against the published output limit. Budget-controlled 2.5
        // creative roles retain the configured candidate cap; Integrity remains
        // conservatively reserved against the model limit because its budget is advisory.
        if (model.ThinkingControl == E0AGeminiThinkingControlKind.Level ||
            profile.Role == E0ARole.Integrity)
        {
            return model.ModelOutputTokenLimit;
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
        var model = E0AGeminiProviderPolicy.ModelProfile(profile);

        if (usage.OutputTokens > E0AGeminiProviderPolicy.IntendedGeneratedTokenCeiling)
        {
            return "gemini-generated-token-overrun";
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
        if (model.CreativeRequiresZeroReasoning &&
            profile.Role is E0ARole.Performer or E0ARole.Interpreter)
        {
            return usage.ReasoningTokens == 0
                ? null
                : "gemini-thinking-not-disabled";
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
            CachedInputUsdPerMillionTokens > InputUsdPerMillionTokens ||
            !HasRepresentableSingleTokenCost(InputUsdPerMillionTokens) ||
            !HasRepresentableSingleTokenCost(OutputUsdPerMillionTokens) ||
            (CachedInputUsdPerMillionTokens > 0m &&
             !HasRepresentableSingleTokenCost(CachedInputUsdPerMillionTokens)) ||
            !SupportsRepresentableUsageDomain())
        {
            throw new E0AHarnessException("E0-A pricing assumptions are invalid.");
        }
    }

    private static bool HasRepresentableSingleTokenCost(decimal rate) =>
        rate / 1_000_000m > 0m;

    private bool SupportsRepresentableUsageDomain()
    {
        try
        {
            var maximumTokenCount = (decimal)long.MaxValue;
            _ = checked(
                (maximumTokenCount / 1_000_000m * InputUsdPerMillionTokens) +
                (maximumTokenCount / 1_000_000m * OutputUsdPerMillionTokens));
            return true;
        }
        catch (OverflowException)
        {
            return false;
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
        string providerProfileId,
        E0ARoleProfile performer,
        E0ARoleProfile integrity,
        E0ARoleProfile interpreter,
        E0APricingAssumptions pricing)
    {
        Variant = variant;
        ProviderProfileId = providerProfileId;
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
    internal string ProviderProfileId { get; }
    internal E0ARoleProfile Performer { get; }
    internal E0ARoleProfile Integrity { get; }
    internal E0ARoleProfile Interpreter { get; }
    internal E0APricingAssumptions Pricing { get; }
    internal ImmutableArray<StateMutationDomain> AutoApproveDomains => AutoApprove;
    internal E0AGeminiModelProfile ModelProfile => E0AGeminiModelCatalog.ForProfileId(ProviderProfileId);
    internal long MaxInputTokens => ModelProfile.ModelInputTokenLimit;

    // Historical compact factory retained for provider-neutral tests. It remains
    // the original Gemini 2.5 Flash CREATIVE-NONE anchor.
    internal static E0ARunEnvelope CreativeNone(E0APricingAssumptions pricing) =>
        GeminiNormativeReference(pricing);

    internal static E0ARunEnvelope GeminiNormativeReference(E0APricingAssumptions pricing)
    {
        ArgumentNullException.ThrowIfNull(pricing);
        return Build(E0AGeminiModelCatalog.Flash25None, pricing);
    }

    internal static E0ARunEnvelope GeminiComparison(string providerProfileId)
    {
        var model = E0AGeminiModelCatalog.ForProfileId(providerProfileId);
        return Build(model, E0AGeminiPricingPolicy.ConservativeShadowPricingFor(model));
    }

    private static E0ARunEnvelope Build(
        E0AGeminiModelProfile model,
        E0APricingAssumptions pricing)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(pricing);
        model.Validate();

        var creativeReasoning = model.ThinkingControl == E0AGeminiThinkingControlKind.Budget
            ? E0AReasoningLevel.None
            : E0AReasoningLevel.Minimal;
        var envelope = new E0ARunEnvelope(
            model.Variant,
            model.ProfileId,
            new E0ARoleProfile(
                E0ARole.Performer,
                E0AGeminiProviderPolicy.Provider,
                model.Model,
                creativeReasoning,
                true,
                RoleMaxOutputTokens,
                E0AGeminiProviderPolicy.ServiceTier),
            new E0ARoleProfile(
                E0ARole.Integrity,
                E0AGeminiProviderPolicy.Provider,
                model.Model,
                E0AReasoningLevel.High,
                false,
                E0AGeminiProviderPolicy.IntegrityCandidateMaxOutputTokens,
                E0AGeminiProviderPolicy.ServiceTier),
            new E0ARoleProfile(
                E0ARole.Interpreter,
                E0AGeminiProviderPolicy.Provider,
                model.Model,
                creativeReasoning,
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

        ValidateGeminiComparison();
    }

    private void ValidateGeminiComparison()
    {
        var model = ModelProfile;
        if (!string.Equals(Variant, model.Variant, StringComparison.Ordinal) ||
            !string.Equals(Performer.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal) ||
            !string.Equals(Performer.Model, model.Model, StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, E0AGeminiProviderPolicy.ServiceTier, StringComparison.Ordinal) ||
            Integrity.Reasoning != E0AReasoningLevel.High ||
            Performer.MaxOutputTokens != RoleMaxOutputTokens ||
            Integrity.MaxOutputTokens != E0AGeminiProviderPolicy.IntegrityCandidateMaxOutputTokens ||
            Interpreter.MaxOutputTokens != RoleMaxOutputTokens ||
            !Performer.Stream || Integrity.Stream || !Interpreter.Stream)
        {
            throw new E0AHarnessException("E0-A Gemini comparison role configuration is inconsistent.");
        }

        if (model.ThinkingControl == E0AGeminiThinkingControlKind.Budget)
        {
            if (Performer.Reasoning != E0AReasoningLevel.None ||
                Interpreter.Reasoning != E0AReasoningLevel.None ||
                E0AGeminiProviderPolicy.ThinkingBudgetTokens(Performer) != 0 ||
                E0AGeminiProviderPolicy.ThinkingBudgetTokens(Interpreter) != 0 ||
                E0AGeminiProviderPolicy.ThinkingBudgetTokens(Integrity) != E0AGeminiProviderPolicy.IntegrityThinkingBudgetTokens)
            {
                throw new E0AHarnessException("E0-A Gemini thinking-budget configuration is inconsistent.");
            }
            return;
        }

        if (Performer.Reasoning != E0AReasoningLevel.Minimal ||
            Interpreter.Reasoning != E0AReasoningLevel.Minimal ||
            !string.Equals(E0AGeminiProviderPolicy.ThinkingLevel(Performer), "minimal", StringComparison.Ordinal) ||
            !string.Equals(E0AGeminiProviderPolicy.ThinkingLevel(Integrity), "high", StringComparison.Ordinal) ||
            !string.Equals(E0AGeminiProviderPolicy.ThinkingLevel(Interpreter), "minimal", StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Gemini thinking-level configuration is inconsistent.");
        }
    }
}

internal sealed class E0AHarnessException : Exception
{
    internal E0AHarnessException(string message) : base(message) { }
}
