namespace Ensemble.E0.Core.Fixture;

public readonly record struct FixtureVersion
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("FixtureVersion is uninitialized.");
    private FixtureVersion(string value) => _value = value;

    public static FixtureVersion From(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var parts = value.Split('.', StringSplitOptions.None);
        if (parts.Length != 3 || parts.Any(part => !IsCanonicalNumericIdentifier(part)))
        {
            throw new ArgumentException(
                "Fixture version must use canonical stable major.minor.patch syntax.",
                nameof(value));
        }

        return new FixtureVersion(value);
    }

    public override string ToString() => Value;

    private static bool IsCanonicalNumericIdentifier(string value)
    {
        if (value.Length == 0 || (value.Length > 1 && value[0] == '0'))
        {
            return false;
        }

        foreach (var character in value)
        {
            if (character is < '0' or > '9')
            {
                return false;
            }
        }

        return true;
    }
}
