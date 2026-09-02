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

        if (value.Contains('\0', StringComparison.Ordinal))
        {
            throw new FixtureValidationException($"Text field '{fieldName}' contains a NUL character.");
        }

        if (!value.IsNormalized(NormalizationForm.FormC))
        {
            throw new FixtureValidationException($"Text field '{fieldName}' must already be Unicode NFC.");
        }

        return value;
    }
}
