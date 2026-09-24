using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace Kymaean.Application;

public sealed record PerformanceOpportunity
{
    public PerformanceOpportunity(SceneId sceneId, CharacterId characterId)
    {
        ArgumentNullException.ThrowIfNull(sceneId);
        ArgumentNullException.ThrowIfNull(characterId);
        SceneId = sceneId;
        CharacterId = characterId;
    }

    public SceneId SceneId { get; }

    public CharacterId CharacterId { get; }
}

public sealed record CharacterCircumstance
{
    public CharacterCircumstance(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Text = text;
    }

    public string Text { get; }
}

public sealed class CharacterPerformanceContext
{
    internal CharacterPerformanceContext(
        SceneId sceneId,
        CharacterId characterId,
        string characterName,
        IEnumerable<CharacterCircumstance> circumstances)
    {
        ArgumentNullException.ThrowIfNull(sceneId);
        ArgumentNullException.ThrowIfNull(characterId);
        ArgumentException.ThrowIfNullOrWhiteSpace(characterName);
        ArgumentNullException.ThrowIfNull(circumstances);

        var materialized = circumstances.ToArray();
        if (materialized.Any(circumstance => circumstance is null))
        {
            throw new ArgumentException(
                "Character Performance context cannot contain a null Circumstance.",
                nameof(circumstances));
        }

        SceneId = sceneId;
        CharacterId = characterId;
        CharacterName = characterName;
        Circumstances = ImmutableArray.Create(materialized);
    }

    public SceneId SceneId { get; }

    public CharacterId CharacterId { get; }

    public string CharacterName { get; }

    public ImmutableArray<CharacterCircumstance> Circumstances { get; }
}

public sealed record PerformanceCandidate
{
    public PerformanceCandidate(string visibleText)
    {
        VisibleText = ProductPerformanceText.Validate(visibleText);
    }

    public string VisibleText { get; }
}

public sealed record CharacterCircumstanceProposal
{
    public CharacterCircumstanceProposal(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Text = text;
    }

    public string Text { get; }
}

public interface IProductPerformer
{
    PerformanceCandidate Perform(CharacterPerformanceContext context);
}

public interface IProductConsequenceInterpreter
{
    CharacterCircumstanceProposal Interpret(
        CharacterPerformanceContext context,
        PerformanceCandidate performance);
}

public sealed record AcceptedPerformance
{
    public AcceptedPerformance(
        SceneId sceneId,
        CharacterId characterId,
        string visibleText,
        CharacterCircumstance consequence)
    {
        ArgumentNullException.ThrowIfNull(sceneId);
        ArgumentNullException.ThrowIfNull(characterId);
        ArgumentNullException.ThrowIfNull(consequence);

        SceneId = sceneId;
        CharacterId = characterId;
        VisibleText = ProductPerformanceText.Validate(visibleText);
        Consequence = consequence;
    }

    public SceneId SceneId { get; }

    public CharacterId CharacterId { get; }

    public string VisibleText { get; }

    public CharacterCircumstance Consequence { get; }
}

public sealed class AcceptedPerformanceHistory :
    IEquatable<AcceptedPerformanceHistory>
{
    public static AcceptedPerformanceHistory Empty { get; } =
        new(Array.Empty<AcceptedPerformance>());

    public AcceptedPerformanceHistory(
        IEnumerable<AcceptedPerformance> performances)
    {
        ArgumentNullException.ThrowIfNull(performances);

        var materialized = performances.ToArray();
        if (materialized.Any(performance => performance is null))
        {
            throw new ArgumentException(
                "Accepted Performance history cannot contain a null Performance.",
                nameof(performances));
        }

        Performances = ImmutableArray.Create(materialized);
    }

    public ImmutableArray<AcceptedPerformance> Performances { get; }

    public bool IsEmpty => Performances.IsEmpty;

    public bool Equals(AcceptedPerformanceHistory? other) =>
        ReferenceEquals(this, other)
        || (other is not null && Performances.SequenceEqual(other.Performances));

    public override bool Equals(object? obj) =>
        obj is AcceptedPerformanceHistory other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var performance in Performances)
        {
            hash.Add(performance);
        }

        return hash.ToHashCode();
    }
}

public sealed record PerformanceExecution
{
    internal PerformanceExecution(
        AcceptedPerformance acceptedPerformance,
        ProductionReplayProjection replay)
    {
        ArgumentNullException.ThrowIfNull(acceptedPerformance);
        ArgumentNullException.ThrowIfNull(replay);
        AcceptedPerformance = acceptedPerformance;
        Replay = replay;
    }

    public AcceptedPerformance AcceptedPerformance { get; }

    public ProductionReplayProjection Replay { get; }
}

internal static class ProvisionalPerformanceAcceptancePolicy
{
    public static AcceptedPerformance Accept(
        PerformanceOpportunity opportunity,
        PerformanceCandidate candidate,
        CharacterCircumstanceProposal consequence)
    {
        ArgumentNullException.ThrowIfNull(opportunity);
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(consequence);

        return new AcceptedPerformance(
            opportunity.SceneId,
            opportunity.CharacterId,
            candidate.VisibleText,
            new CharacterCircumstance(consequence.Text));
    }
}

internal static class ProductPerformanceText
{
    public static string Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
        {
            return value;
        }

        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            if (char.IsHighSurrogate(character))
            {
                if (index + 1 >= value.Length ||
                    !char.IsLowSurrogate(value[index + 1]))
                {
                    throw new ArgumentException(
                        "Performance text contains invalid Unicode.",
                        nameof(value));
                }

                index++;
            }
            else if (char.IsLowSurrogate(character))
            {
                throw new ArgumentException(
                    "Performance text contains invalid Unicode.",
                    nameof(value));
            }
        }

        if (!value.IsNormalized(NormalizationForm.FormC))
        {
            throw new ArgumentException(
                "Performance text must already be Unicode NFC.",
                nameof(value));
        }

        var hasDisplayBearingScalar = false;
        foreach (var rune in value.EnumerateRunes())
        {
            var category = Rune.GetUnicodeCategory(rune);
            if (category == UnicodeCategory.Control &&
                rune.Value is not 0x09 and not 0x0A)
            {
                throw new ArgumentException(
                    "Performance text contains a forbidden control character.",
                    nameof(value));
            }

            if (!Rune.IsWhiteSpace(rune) &&
                category is not UnicodeCategory.Control and
                not UnicodeCategory.Format and
                not UnicodeCategory.NonSpacingMark and
                not UnicodeCategory.SpacingCombiningMark and
                not UnicodeCategory.EnclosingMark)
            {
                hasDisplayBearingScalar = true;
            }
        }

        if (!hasDisplayBearingScalar)
        {
            throw new ArgumentException(
                "Non-silent Performance text must contain visible Character-legible content.",
                nameof(value));
        }

        return value;
    }
}
