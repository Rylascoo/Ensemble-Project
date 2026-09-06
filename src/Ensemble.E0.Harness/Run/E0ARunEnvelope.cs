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

    internal static E0ARunEnvelope CreativeNone(E0APricingAssumptions pricing) =>
        Create("CREATIVE-NONE", E0AReasoningLevel.None, pricing);

    internal static E0ARunEnvelope CreativeLow(E0APricingAssumptions pricing) =>
        Create("CREATIVE-LOW", E0AReasoningLevel.Low, pricing);

    internal static E0ARunEnvelope CreativeMedium(E0APricingAssumptions pricing) =>
        Create("CREATIVE-MEDIUM", E0AReasoningLevel.Medium, pricing);

    internal static E0ARunEnvelope CreativeHigh(E0APricingAssumptions pricing) =>
        Create("CREATIVE-HIGH", E0AReasoningLevel.High, pricing);

    internal void Validate()
    {
        if (Variant is not "CREATIVE-NONE" and
            not "CREATIVE-LOW" and
            not "CREATIVE-MEDIUM" and
            not "CREATIVE-HIGH")
        {
            throw new E0AHarnessException("E0-A run variant is invalid.");
        }

        Performer.Validate();
        Integrity.Validate();
        Interpreter.Validate();
        Pricing.Validate();

        if (!string.Equals(Performer.Provider, "OpenAI", StringComparison.Ordinal) ||
            !string.Equals(Performer.Provider, Integrity.Provider, StringComparison.Ordinal) ||
            !string.Equals(Performer.Provider, Interpreter.Provider, StringComparison.Ordinal) ||
            !string.Equals(Performer.Model, "gpt-5.6-sol", StringComparison.Ordinal) ||
            !string.Equals(Performer.Model, Integrity.Model, StringComparison.Ordinal) ||
            !string.Equals(Performer.Model, Interpreter.Model, StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, "default", StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, Integrity.ServiceTier, StringComparison.Ordinal) ||
            !string.Equals(Performer.ServiceTier, Interpreter.ServiceTier, StringComparison.Ordinal) ||
            Performer.Reasoning != Interpreter.Reasoning ||
            Integrity.Reasoning != E0AReasoningLevel.High ||
            Performer.MaxOutputTokens != RoleMaxOutputTokens ||
            Integrity.MaxOutputTokens != RoleMaxOutputTokens ||
            Interpreter.MaxOutputTokens != RoleMaxOutputTokens ||
            !Performer.Stream || Integrity.Stream || !Interpreter.Stream)
        {
            throw new E0AHarnessException("E0-A reference role configuration is inconsistent.");
        }
    }

    private static E0ARunEnvelope Create(
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
