using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed class CharacterPresentationRow
{
    private CharacterPresentationRow(
        CharacterSummary summary,
        string? characterCode)
    {
        Summary = summary;
        CharacterCode = characterCode;
    }

    public CharacterSummary Summary { get; }

    public CharacterId Id => Summary.Id;

    public string CharacterName => Summary.CharacterName;

    public string? CharacterCode { get; }

    public bool HasCharacterCode =>
        !string.IsNullOrEmpty(CharacterCode);

    public string CharacterCodeLabel =>
        HasCharacterCode
            ? $"Character code {CharacterCode}"
            : string.Empty;

    public string AccessibleName =>
        HasCharacterCode
            ? $"{CharacterName}, Character code {CharacterCode}"
            : CharacterName;

    public static IReadOnlyList<CharacterPresentationRow> Build(
        IReadOnlyList<CharacterSummary> characters)
    {
        ArgumentNullException.ThrowIfNull(characters);

        var duplicateCodes = PresentationIdentityCode.BuildDuplicateCodes(
            characters,
            character => character.CharacterName,
            character => character.Id.Value);

        return characters
            .Select(
                character =>
                    new CharacterPresentationRow(
                        character,
                        duplicateCodes.GetValueOrDefault(
                            character.Id.Value)))
            .ToArray();
    }
}
