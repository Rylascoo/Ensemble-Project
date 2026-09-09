using System.Text;
using System.Text.Json;

namespace Ensemble.E0.Harness.Gemini;

internal sealed class E0AGeminiCountTokensFailureException : Exception
{
    private const int MaxErrorBodyBytes = 16 * 1024;
    private const int MaxFieldPaths = 4;
    private const int MaxFieldPathLength = 160;
    private const string BadRequestType = "type.googleapis.com/google.rpc.BadRequest";

    private E0AGeminiCountTokensFailureException(string diagnostic) : base(diagnostic) =>
        Diagnostic = diagnostic;

    internal string Diagnostic { get; }

    internal static E0AGeminiCountTokensFailureException ResponseInvalid() =>
        new("gemini-counttokens-response-invalid");

    internal static E0AGeminiCountTokensFailureException Transport() =>
        new("gemini-counttokens-transport");

    internal static E0AGeminiCountTokensFailureException Http(
        int httpStatus,
        string? rpcStatus = null,
        IEnumerable<string>? fieldPaths = null)
    {
        if (httpStatus is < 100 or > 599)
        {
            throw new ArgumentOutOfRangeException(nameof(httpStatus));
        }
        if (rpcStatus is not null && !IsKnownRpcStatus(rpcStatus))
        {
            throw new ArgumentException("Gemini RPC status is not evidence-eligible.", nameof(rpcStatus));
        }

        var fields = fieldPaths is null
            ? Array.Empty<string>()
            : fieldPaths
                .Where(IsFieldPathSyntaxSafe)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(x => x, StringComparer.Ordinal)
                .Take(MaxFieldPaths)
                .ToArray();

        var diagnostic = new StringBuilder($"gemini-counttokens-http-{httpStatus}");
        if (rpcStatus is not null)
        {
            diagnostic.Append(";status=").Append(rpcStatus);
        }
        foreach (var field in fields)
        {
            diagnostic.Append(";field=").Append(field);
        }
        return new E0AGeminiCountTokensFailureException(diagnostic.ToString());
    }

    internal static async Task<E0AGeminiCountTokensFailureException> FromHttpResponseAsync(
        HttpResponseMessage response,
        byte[] requestBody,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);
        ArgumentNullException.ThrowIfNull(requestBody);
        var httpStatus = (int)response.StatusCode;
        if (httpStatus is < 100 or > 599)
        {
            return Http(500);
        }

        try
        {
            var body = await ReadBoundedBodyAsync(response.Content, cancellationToken).ConfigureAwait(false);
            if (body is null)
            {
                return Http(httpStatus);
            }

            using var errorDocument = JsonDocument.Parse(body);
            if (errorDocument.RootElement.ValueKind != JsonValueKind.Object ||
                !errorDocument.RootElement.TryGetProperty("error", out var error) ||
                error.ValueKind != JsonValueKind.Object)
            {
                return Http(httpStatus);
            }

            string? rpcStatus = null;
            if (error.TryGetProperty("status", out var statusElement) &&
                statusElement.ValueKind == JsonValueKind.String)
            {
                var candidate = statusElement.GetString();
                if (candidate is not null && IsKnownRpcStatus(candidate))
                {
                    rpcStatus = candidate;
                }
            }

            var requestPropertyNames = RequestPropertyNames(requestBody);
            var fields = new SortedSet<string>(StringComparer.Ordinal);
            if (error.TryGetProperty("details", out var details) &&
                details.ValueKind == JsonValueKind.Array)
            {
                foreach (var detail in details.EnumerateArray())
                {
                    if (detail.ValueKind != JsonValueKind.Object ||
                        !detail.TryGetProperty("@type", out var typeElement) ||
                        typeElement.ValueKind != JsonValueKind.String ||
                        !string.Equals(typeElement.GetString(), BadRequestType, StringComparison.Ordinal) ||
                        !detail.TryGetProperty("fieldViolations", out var violations) ||
                        violations.ValueKind != JsonValueKind.Array)
                    {
                        continue;
                    }

                    foreach (var violation in violations.EnumerateArray())
                    {
                        if (violation.ValueKind != JsonValueKind.Object ||
                            !violation.TryGetProperty("field", out var fieldElement) ||
                            fieldElement.ValueKind != JsonValueKind.String)
                        {
                            continue;
                        }

                        var field = fieldElement.GetString();
                        if (field is not null && IsRequestFieldPath(field, requestPropertyNames))
                        {
                            fields.Add(field);
                        }
                    }
                }
            }

            return Http(httpStatus, rpcStatus, fields);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception) when (
            exception is HttpRequestException or
            IOException or
            JsonException)
        {
            return Http(httpStatus);
        }
    }

    private static async Task<byte[]?> ReadBoundedBodyAsync(
        HttpContent content,
        CancellationToken cancellationToken)
    {
        await using var stream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var destination = new MemoryStream();
        var buffer = new byte[4096];

        while (true)
        {
            var remaining = MaxErrorBodyBytes + 1 - checked((int)destination.Length);
            var count = Math.Min(buffer.Length, remaining);
            var read = await stream.ReadAsync(buffer.AsMemory(0, count), cancellationToken).ConfigureAwait(false);
            if (read == 0)
            {
                return destination.ToArray();
            }

            destination.Write(buffer, 0, read);
            if (destination.Length > MaxErrorBodyBytes)
            {
                return null;
            }
        }
    }

    private static HashSet<string> RequestPropertyNames(byte[] requestBody)
    {
        using var document = JsonDocument.Parse(requestBody);
        var names = new HashSet<string>(StringComparer.Ordinal);
        CollectPropertyNames(document.RootElement, names);
        return names;
    }

    private static void CollectPropertyNames(JsonElement element, HashSet<string> names)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                names.Add(property.Name);
                names.Add(ToSnakeCase(property.Name));
                CollectPropertyNames(property.Value, names);
            }
            return;
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                CollectPropertyNames(item, names);
            }
        }
    }

    private static bool IsRequestFieldPath(string path, HashSet<string> requestPropertyNames)
    {
        if (!IsFieldPathSyntaxSafe(path))
        {
            return false;
        }

        foreach (var segment in path.Split('.'))
        {
            var bracket = segment.IndexOf('[');
            var name = bracket < 0 ? segment : segment[..bracket];
            if (!requestPropertyNames.Contains(name))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsFieldPathSyntaxSafe(string path)
    {
        if (path.Length is 0 or > MaxFieldPathLength)
        {
            return false;
        }

        foreach (var segment in path.Split('.'))
        {
            if (segment.Length == 0)
            {
                return false;
            }

            var bracket = segment.IndexOf('[');
            var name = bracket < 0 ? segment : segment[..bracket];
            if (!IsAsciiIdentifier(name))
            {
                return false;
            }

            if (bracket < 0)
            {
                continue;
            }

            if (segment[^1] != ']' ||
                segment.IndexOf('[', bracket + 1) >= 0 ||
                segment.IndexOf(']', bracket) != segment.Length - 1)
            {
                return false;
            }

            var index = segment[(bracket + 1)..^1];
            if (index.Length == 0 || index.Any(character => character is < '0' or > '9'))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsAsciiIdentifier(string value)
    {
        if (value.Length == 0 || !IsAsciiIdentifierStart(value[0]))
        {
            return false;
        }
        for (var index = 1; index < value.Length; index++)
        {
            var character = value[index];
            if (!IsAsciiIdentifierStart(character) && character is < '0' or > '9')
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsAsciiIdentifierStart(char value) =>
        value is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '_';

    private static string ToSnakeCase(string value)
    {
        var result = new StringBuilder(value.Length + 8);
        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            if (character is >= 'A' and <= 'Z')
            {
                if (index > 0 && value[index - 1] != '_')
                {
                    result.Append('_');
                }
                result.Append((char)(character + ('a' - 'A')));
            }
            else
            {
                result.Append(character);
            }
        }
        return result.ToString();
    }

    private static bool IsKnownRpcStatus(string value) => value switch
    {
        "CANCELLED" or
        "UNKNOWN" or
        "INVALID_ARGUMENT" or
        "DEADLINE_EXCEEDED" or
        "NOT_FOUND" or
        "ALREADY_EXISTS" or
        "PERMISSION_DENIED" or
        "RESOURCE_EXHAUSTED" or
        "FAILED_PRECONDITION" or
        "ABORTED" or
        "OUT_OF_RANGE" or
        "UNIMPLEMENTED" or
        "INTERNAL" or
        "UNAVAILABLE" or
        "DATA_LOSS" or
        "UNAUTHENTICATED" => true,
        _ => false
    };
}
