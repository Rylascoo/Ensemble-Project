using System.Globalization;
using System.Text;

namespace Ensemble.E0.Core.Domain;

internal enum CharacterLegibleTextFailure
{
    None = 0,
    Required = 1,
    InvalidUnicode = 2,
    NotNormalized = 3,
    ForbiddenControl = 4,
    NoDisplayBearingScalar = 5
}

internal static class CharacterLegibleTextInvariants
{
    internal static CharacterLegibleTextFailure Validate(string? value)
    {
        if (value is null)
        {
            return CharacterLegibleTextFailure.Required;
        }

        if (value.Length == 0)
        {
            return CharacterLegibleTextFailure.None;
        }

        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            if (char.IsHighSurrogate(character))
            {
                if (index + 1 >= value.Length ||
                    !char.IsLowSurrogate(value[index + 1]))
                {
                    return CharacterLegibleTextFailure.InvalidUnicode;
                }

                index++;
            }
            else if (char.IsLowSurrogate(character))
            {
                return CharacterLegibleTextFailure.InvalidUnicode;
            }
        }

        if (!value.IsNormalized(NormalizationForm.FormC))
        {
            return CharacterLegibleTextFailure.NotNormalized;
        }

        var hasDisplayBearingScalar = false;
        foreach (var rune in value.EnumerateRunes())
        {
            var category = Rune.GetUnicodeCategory(rune);
            if (category == UnicodeCategory.Control &&
                rune.Value is not 0x09 and not 0x0A)
            {
                return CharacterLegibleTextFailure.ForbiddenControl;
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

        return hasDisplayBearingScalar
            ? CharacterLegibleTextFailure.None
            : CharacterLegibleTextFailure.NoDisplayBearingScalar;
    }
}
