using System.Text;

namespace Ensemble.E0.Core.Fixture;

internal static class CanonicalText
{
    public static string Required(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new FixtureValidationException($"Required text field '{fieldName}' is empty.");
        }

        if (value.Contains('\0'))
        {
            throw new FixtureValidationException($"Text field '{fieldName}' contains a NUL character.");
        }

        if (value.Contains('\r'))
        {
            throw new FixtureValidationException($"Text field '{fieldName}' contains a carriage return.");
        }

        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            if (char.IsHighSurrogate(character))
            {
                if (index + 1 >= value.Length || !char.IsLowSurrogate(value[index + 1]))
                {
                    throw new FixtureValidationException(
                        $"Text field '{fieldName}' contains an invalid Unicode surrogate sequence.");
                }

                index++;
            }
            else if (char.IsLowSurrogate(character))
            {
                throw new FixtureValidationException(
                    $"Text field '{fieldName}' contains an invalid Unicode surrogate sequence.");
            }
        }

        if (!value.IsNormalized(NormalizationForm.FormC))
        {
            throw new FixtureValidationException($"Text field '{fieldName}' must already be Unicode NFC.");
        }

        return value;
    }
}
