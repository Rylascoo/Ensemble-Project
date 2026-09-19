using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed record WorldCurrentFact
{
    public WorldCurrentFact(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Text = text;
    }

    public string Text { get; }
}

public sealed class WorldCurrentState : IEquatable<WorldCurrentState>
{
    public static WorldCurrentState Empty { get; } =
        new(Array.Empty<WorldCurrentFact>());

    public WorldCurrentState(IEnumerable<WorldCurrentFact> facts)
    {
        ArgumentNullException.ThrowIfNull(facts);

        var materialized = facts.ToArray();
        if (materialized.Any(fact => fact is null))
        {
            throw new ArgumentException(
                "World current state cannot contain a null fact.",
                nameof(facts));
        }

        Array.Sort(
            materialized,
            static (left, right) =>
                StringComparer.Ordinal.Compare(left.Text, right.Text));

        for (var index = 1; index < materialized.Length; index++)
        {
            if (StringComparer.Ordinal.Equals(
                    materialized[index - 1].Text,
                    materialized[index].Text))
            {
                throw new ArgumentException(
                    "World current state cannot contain duplicate facts.",
                    nameof(facts));
            }
        }

        Facts = ImmutableArray.Create(materialized);
    }

    public ImmutableArray<WorldCurrentFact> Facts { get; }

    public bool IsEmpty => Facts.IsEmpty;

    public bool Equals(WorldCurrentState? other) =>
        ReferenceEquals(this, other)
        || (other is not null && Facts.SequenceEqual(other.Facts));

    public override bool Equals(object? obj) =>
        obj is WorldCurrentState other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var fact in Facts)
        {
            hash.Add(fact);
        }

        return hash.ToHashCode();
    }
}