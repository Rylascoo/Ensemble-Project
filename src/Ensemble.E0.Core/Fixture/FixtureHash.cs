using System.Security.Cryptography;

namespace Ensemble.E0.Core.Fixture;

public static class FixtureHash
{
    public static string Compute(ValidatedFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        var canonicalBytes = Ecj1FixtureCanonicalizer.Serialize(fixture);
        var digest = SHA256.HashData(canonicalBytes);
        return Convert.ToHexString(digest).ToLowerInvariant();
    }
}
