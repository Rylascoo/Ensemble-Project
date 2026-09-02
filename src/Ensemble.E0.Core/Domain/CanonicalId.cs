namespace Ensemble.E0.Core.Domain;

internal static class CanonicalId
{
    private const int MaxLength = 128;

    public static string Validate(string value, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(value, parameterName);

        if (value.Length is 0 or > MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                $"Canonical IDs must contain between 1 and {MaxLength} characters.");
        }

        if (!string.Equals(value, value.Trim(), StringComparison.Ordinal))
        {
            throw new ArgumentException("Canonical IDs may not contain leading or trailing whitespace.", parameterName);
        }

        foreach (var character in value)
        {
            if (!IsAllowed(character))
            {
                throw new ArgumentException(
                    $"Canonical ID contains unsupported character U+{(int)character:X4}.",
                    parameterName);
            }
        }

        return value;
    }

    private static bool IsAllowed(char value) =>
        value is >= 'A' and <= 'Z'
        or >= 'a' and <= 'z'
        or >= '0' and <= '9'
        or '-' or '_' or '.' or ':' or '@';
}
