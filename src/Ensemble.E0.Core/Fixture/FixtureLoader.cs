using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ensemble.E0.Core.Fixture;

public static class FixtureLoader
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        AllowTrailingCommas = false,
        PropertyNameCaseInsensitive = false,
        ReadCommentHandling = JsonCommentHandling.Disallow,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    public static E0FixtureDocument Load(ReadOnlySpan<byte> utf8Json)
    {
        StrictJsonPreflight.Validate(utf8Json);

        try
        {
            return JsonSerializer.Deserialize<E0FixtureDocument>(utf8Json, SerializerOptions)
                ?? throw new FixtureValidationException("Fixture JSON deserialized to null.");
        }
        catch (JsonException exception)
        {
            throw new FixtureValidationException("Fixture JSON does not match E0 Fixture Dialect v1.", exception);
        }
    }
}
