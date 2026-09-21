using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed record WorldCurrentTruth
{
    public WorldCurrentTruth(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Text = text;
    }

    public string Text { get; }
}

public sealed class WorldCurrentState : IEquatable<WorldCurrentState>
{
    public static WorldCurrentState Empty { get; } =
        new(Array.Empty<WorldCurrentTruth>());

    public WorldCurrentState(IEnumerable<WorldCurrentTruth> truths)
    {
        ArgumentNullException.ThrowIfNull(truths);

        var materialized = truths.ToArray();
        if (materialized.Any(truth => truth is null))
        {
            throw new ArgumentException(
                "World current state cannot contain a null truth.",
                nameof(truths));
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
                    "World current state cannot contain duplicate truths.",
                    nameof(truths));
            }
        }

        Truths = ImmutableArray.Create(materialized);
    }

    public ImmutableArray<WorldCurrentTruth> Truths { get; }

    public bool IsEmpty => Truths.IsEmpty;

    public bool Equals(WorldCurrentState? other) =>
        ReferenceEquals(this, other)
        || (other is not null && Truths.SequenceEqual(other.Truths));

    public override bool Equals(object? obj) =>
        obj is WorldCurrentState other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var truth in Truths)
        {
            hash.Add(truth);
        }

        return hash.ToHashCode();
    }
}
