using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed record CharacterId
{
    public CharacterId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}

public sealed record CharacterSummary
{
    public CharacterSummary(CharacterId id, string characterName)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(characterName);
        Id = id;
        CharacterName = characterName;
    }

    public CharacterId Id { get; }

    public string CharacterName { get; }
}

public sealed class ProductionCast : IEquatable<ProductionCast>
{
    public static ProductionCast Empty { get; } =
        new(Array.Empty<CharacterSummary>());

    public ProductionCast(IEnumerable<CharacterSummary> characters)
    {
        ArgumentNullException.ThrowIfNull(characters);

        var materialized = characters.ToArray();
        if (materialized.Any(character => character is null))
        {
            throw new ArgumentException(
                "Production Cast cannot contain a null Character.",
                nameof(characters));
        }

        var ids = new HashSet<CharacterId>();
        foreach (var character in materialized)
        {
            if (!ids.Add(character.Id))
            {
                throw new ArgumentException(
                    "Production Cast cannot contain duplicate Character identity.",
                    nameof(characters));
            }
        }

        Characters = ImmutableArray.Create(materialized);
    }

    public ImmutableArray<CharacterSummary> Characters { get; }

    public bool IsEmpty => Characters.IsEmpty;

    public bool Equals(ProductionCast? other) =>
        ReferenceEquals(this, other)
        || (other is not null && Characters.SequenceEqual(other.Characters));

    public override bool Equals(object? obj) =>
        obj is ProductionCast other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var character in Characters)
        {
            hash.Add(character);
        }

        return hash.ToHashCode();
    }
}
