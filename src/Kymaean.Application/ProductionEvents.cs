namespace Kymaean.Application;

public abstract record ProductionEvent
{
    internal ProductionEvent()
    {
    }
}

public sealed record ProductionCreatedEvent : ProductionEvent
{
    public ProductionCreatedEvent(string productionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);
        ProductionName = productionName;
    }

    public string ProductionName { get; }
}

public sealed record CharacterCreatedEvent : ProductionEvent
{
    public CharacterCreatedEvent(
        CharacterId characterId,
        string characterName)
    {
        ArgumentNullException.ThrowIfNull(characterId);
        ArgumentException.ThrowIfNullOrWhiteSpace(characterName);
        CharacterId = characterId;
        CharacterName = characterName;
    }

    public CharacterId CharacterId { get; }

    public string CharacterName { get; }
}

public sealed record CreatorReplacedWorldCurrentStateEvent : ProductionEvent
{
    public CreatorReplacedWorldCurrentStateEvent(WorldCurrentState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState);
        CurrentState = currentState;
    }

    public WorldCurrentState CurrentState { get; }
}
