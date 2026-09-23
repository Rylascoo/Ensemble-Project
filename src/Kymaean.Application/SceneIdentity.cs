using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed record SceneId
{
    public SceneId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}

public sealed class SceneRoster : IEquatable<SceneRoster>
{
    public static SceneRoster Empty { get; } =
        new(Array.Empty<CharacterId>());

    public SceneRoster(IEnumerable<CharacterId> characterIds)
    {
        ArgumentNullException.ThrowIfNull(characterIds);

        var materialized = characterIds.ToArray();
        if (materialized.Any(characterId => characterId is null))
        {
            throw new ArgumentException(
                "A Scene roster cannot contain a null Character identity.",
                nameof(characterIds));
        }

        var identities = new HashSet<CharacterId>();
        foreach (var characterId in materialized)
        {
            if (!identities.Add(characterId))
            {
                throw new ArgumentException(
                    "A Scene roster cannot contain duplicate Character identity.",
                    nameof(characterIds));
            }
        }

        CharacterIds = ImmutableArray.Create(materialized);
    }

    public ImmutableArray<CharacterId> CharacterIds { get; }

    public bool IsEmpty => CharacterIds.IsEmpty;

    public static SceneRoster Canonicalize(
        ProductionCast productionCast,
        IEnumerable<CharacterId> characterIds)
    {
        ArgumentNullException.ThrowIfNull(productionCast);
        ArgumentNullException.ThrowIfNull(characterIds);

        var requested = new SceneRoster(characterIds);
        var requestedIds = requested.CharacterIds.ToHashSet();
        var castIds = productionCast.Characters
            .Select(character => character.Id)
            .ToHashSet();

        if (!requestedIds.IsSubsetOf(castIds))
        {
            throw new ArgumentException(
                "Every Scene roster Character must already exist in the Production Cast.",
                nameof(characterIds));
        }

        return new SceneRoster(
            productionCast.Characters
                .Select(character => character.Id)
                .Where(requestedIds.Contains));
    }

    public bool Equals(SceneRoster? other) =>
        ReferenceEquals(this, other)
        || (other is not null && CharacterIds.SequenceEqual(other.CharacterIds));

    public override bool Equals(object? obj) =>
        obj is SceneRoster other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var characterId in CharacterIds)
        {
            hash.Add(characterId);
        }

        return hash.ToHashCode();
    }
}

public sealed record EstablishedScene
{
    public EstablishedScene(SceneId id, SceneRoster initialRoster)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(initialRoster);
        Id = id;
        InitialRoster = initialRoster;
    }

    public SceneId Id { get; }

    public SceneRoster InitialRoster { get; }
}

public sealed class ProductionScenes : IEquatable<ProductionScenes>
{
    public static ProductionScenes Empty { get; } =
        new(Array.Empty<EstablishedScene>());

    public ProductionScenes(IEnumerable<EstablishedScene> scenes)
    {
        ArgumentNullException.ThrowIfNull(scenes);

        var materialized = scenes.ToArray();
        if (materialized.Any(scene => scene is null))
        {
            throw new ArgumentException(
                "Production Scenes cannot contain a null Scene.",
                nameof(scenes));
        }

        var identities = new HashSet<SceneId>();
        foreach (var scene in materialized)
        {
            if (!identities.Add(scene.Id))
            {
                throw new ArgumentException(
                    "Production Scenes cannot contain duplicate Scene identity.",
                    nameof(scenes));
            }
        }

        Scenes = ImmutableArray.Create(materialized);
    }

    public ImmutableArray<EstablishedScene> Scenes { get; }

    public bool IsEmpty => Scenes.IsEmpty;

    public bool Equals(ProductionScenes? other) =>
        ReferenceEquals(this, other)
        || (other is not null && Scenes.SequenceEqual(other.Scenes));

    public override bool Equals(object? obj) =>
        obj is ProductionScenes other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var scene in Scenes)
        {
            hash.Add(scene);
        }

        return hash.ToHashCode();
    }
}
