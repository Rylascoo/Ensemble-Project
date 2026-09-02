using System.Text.Json;

namespace Ensemble.E0.Core.Fixture;

public static class StrictJsonPreflight
{
    private const int MaxDepth = 64;

    public static void Validate(ReadOnlySpan<byte> utf8Json)
    {
        if (utf8Json.IsEmpty)
        {
            throw new FixtureValidationException("Fixture JSON is empty.");
        }

        if (utf8Json.Length > E0FixtureDialect.MaxFixtureBytes)
        {
            throw new FixtureValidationException(
                $"Fixture JSON exceeds the E0 limit of {E0FixtureDialect.MaxFixtureBytes} bytes.");
        }

        var options = new JsonReaderOptions
        {
            AllowTrailingCommas = false,
            CommentHandling = JsonCommentHandling.Disallow,
            MaxDepth = MaxDepth
        };

        var reader = new Utf8JsonReader(utf8Json, options);
        var objectProperties = new Stack<HashSet<string>>();
        var sawRoot = false;
        var rootClosed = false;

        try
        {
            while (reader.Read())
            {
                if (!sawRoot)
                {
                    if (reader.TokenType != JsonTokenType.StartObject || reader.CurrentDepth != 0)
                    {
                        throw new FixtureValidationException("Fixture JSON root must be an object.");
                    }

                    sawRoot = true;
                }
                else if (rootClosed)
                {
                    throw new FixtureValidationException("Fixture JSON contains content after the root object.");
                }

                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        objectProperties.Push(new HashSet<string>(StringComparer.Ordinal));
                        break;

                    case JsonTokenType.PropertyName:
                        if (objectProperties.Count == 0)
                        {
                            throw new FixtureValidationException("Property encountered outside an object.");
                        }

                        var propertyName = reader.GetString()
                            ?? throw new FixtureValidationException("Fixture contains a null property name.");

                        if (!objectProperties.Peek().Add(propertyName))
                        {
                            throw new FixtureValidationException(
                                $"Duplicate JSON property '{propertyName}' at byte offset {reader.TokenStartIndex}.");
                        }

                        break;

                    case JsonTokenType.Number:
                        throw new FixtureValidationException(
                            $"E0 Fixture Dialect v1 does not permit JSON Number tokens (byte offset {reader.TokenStartIndex}).");

                    case JsonTokenType.EndObject:
                        if (objectProperties.Count == 0)
                        {
                            throw new FixtureValidationException("Unexpected object terminator.");
                        }

                        objectProperties.Pop();
                        if (reader.CurrentDepth == 0)
                        {
                            rootClosed = true;
                        }

                        break;
                }
            }
        }
        catch (JsonException exception)
        {
            throw new FixtureValidationException("Fixture JSON is syntactically invalid.", exception);
        }

        if (!sawRoot || !rootClosed || objectProperties.Count != 0)
        {
            throw new FixtureValidationException("Fixture JSON ended before the root object was complete.");
        }
    }
}
