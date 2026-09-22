namespace Kymaean.Application;

public interface IProductionCharacterCreator
{
    ProductAccessResult<CharacterCreation> CreateCharacter(
        ProductionId productionId,
        string characterName);
}

public sealed record CharacterCreation
{
    public CharacterCreation(
        CharacterSummary character,
        ProductionReplayProjection replay)
    {
        ArgumentNullException.ThrowIfNull(character);
        ArgumentNullException.ThrowIfNull(replay);
        Character = character;
        Replay = replay;
    }

    public CharacterSummary Character { get; }

    public ProductionReplayProjection Replay { get; }
}
