using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.OpenAI;

internal sealed class OpenAIResponsesPort : IE0AProviderRolePort, IE0AInputTokenCounter
{
    private static readonly Uri ResponsesUri = new("https://api.openai.com/v1/responses");
    private static readonly Uri InputTokensUri = new("https://api.openai.com/v1/responses/input_tokens");
    private static readonly string[] InputTokenRequestFields =
    {
        "model",
        "instructions",
        "input",
        "reasoning",
        "text",
        "truncation"
    };

    private readonly HttpClient _http;
    private readonly string _apiKey;

    internal OpenAIResponsesPort(HttpClient http, string apiKey)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new E0AHarnessException("OPENAI_API_KEY is required at the E0-A provider edge.");
        }
        _apiKey = apiKey;
    }

    internal static OpenAIResponsesPort FromEnvironment(HttpClient http)
    {
        var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new E0AHarnessException("OPENAI_API_KEY is required at the E0-A provider edge.");
        }
        return new OpenAIResponsesPort(http, key);
    }

    public async Task<long> CountInputTokensAsync(
        PreparedRoleAttempt attempt,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attempt);
        try
        {
            var body = BuildInputTokenRequestBody(attempt.RequestBody);
            using var request = CreateRequest(HttpMethod.Post, InputTokensUri, body);
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new E0AHarnessException("E0-A provider input-token preflight failed.");
            }

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            using var document = JsonDocument.Parse(bytes);
            if (document.RootElement.ValueKind != JsonValueKind.Object ||
                !document.RootElement.TryGetProperty("input_tokens", out var count) ||
                count.ValueKind != JsonValueKind.Number ||
                !count.TryGetInt64(out var tokens) || tokens < 0)
            {
                throw new E0AHarnessException("E0-A provider input-token response is invalid.");
            }
            return tokens;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (E0AHarnessException)
        {
            throw;
        }
        catch (Exception exception) when (exception is HttpRequestException or IOException or JsonException)
        {
            throw new E0AHarnessException("E0-A provider input-token preflight failed.");
        }
    }

    public async Task<RoleAttemptReceipt> ExecuteAsync(
        PreparedRoleAttempt attempt,
        IE0AProviderDiagnosticSink diagnostics,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attempt);
        ArgumentNullException.ThrowIfNull(diagnostics);
        try
        {
            return attempt.Profile.Stream
                ? await ExecuteStreamingAsync(attempt, diagnostics, cancellationToken).ConfigureAwait(false)
                : await ExecuteBufferedAsync(attempt, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception) when (
            exception is HttpRequestException or
            IOException or
            JsonException or
            DecoderFallbackException)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "malformed-or-transport");
        }
    }

    private async Task<RoleAttemptReceipt> ExecuteBufferedAsync(
        PreparedRoleAttempt attempt,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(HttpMethod.Post, ResponsesUri, attempt.RequestBody);
        using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, $"http-{(int)response.StatusCode}");
        }

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        using var document = JsonDocument.Parse(bytes);
        return ReceiptFromResponse(attempt, document.RootElement);
    }

    private async Task<RoleAttemptReceipt> ExecuteStreamingAsync(
        PreparedRoleAttempt attempt,
        IE0AProviderDiagnosticSink diagnostics,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(HttpMethod.Post, ResponsesUri, attempt.RequestBody);
        using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, $"http-{(int)response.StatusCode}");
        }

        await using var body = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var reader = new StreamReader(
            body,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true),
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 4096,
            leaveOpen: false);
        JsonElement? completed = null;
        var refused = false;
        var failed = false;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (line is null)
            {
                break;
            }
            if (!line.StartsWith("data: ", StringComparison.Ordinal))
            {
                continue;
            }

            var data = line[6..];
            if (string.Equals(data, "[DONE]", StringComparison.Ordinal))
            {
                continue;
            }

            var utf8 = Encoding.UTF8.GetBytes(data);
            diagnostics.RecordStreamEvent(attempt, utf8);
            using var eventDoc = JsonDocument.Parse(utf8);
            var root = eventDoc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return RoleAttemptReceipt.TechnicalFailure(attempt, "malformed-provider-event");
            }

            string? type = null;
            if (root.TryGetProperty("type", out var typeElement) && !TryDecodeString(typeElement, out type))
            {
                return RoleAttemptReceipt.TechnicalFailure(attempt, "malformed-provider-event");
            }
            switch (type)
            {
                case "response.refusal.delta":
                case "response.refusal.done":
                    refused = true;
                    break;
                case "response.failed":
                case "response.incomplete":
                case "error":
                    failed = true;
                    break;
                case "response.completed":
                    if (root.TryGetProperty("response", out var responseElement))
                    {
                        if (responseElement.ValueKind != JsonValueKind.Object)
                        {
                            return RoleAttemptReceipt.TechnicalFailure(attempt, "malformed-provider-event");
                        }
                        completed = responseElement.Clone();
                    }
                    break;
            }
        }

        if (refused)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-refusal");
        }
        if (failed || !completed.HasValue)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-incomplete");
        }

        return ReceiptFromResponse(attempt, completed.Value);
    }

    private static RoleAttemptReceipt ReceiptFromResponse(
        PreparedRoleAttempt attempt,
        JsonElement response)
    {
        if (response.ValueKind != JsonValueKind.Object ||
            !response.TryGetProperty("status", out var statusElement) ||
            !TryDecodeString(statusElement, out var status) ||
            !string.Equals(status, "completed", StringComparison.Ordinal))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-incomplete");
        }

        if (!TryContainsRefusal(response, out var refused))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-response-incomplete");
        }
        if (refused)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-refusal");
        }

        string? id = null;
        string? model = null;
        var hasId = response.TryGetProperty("id", out var idElement) && TryDecodeString(idElement, out id);
        var hasModel = response.TryGetProperty("model", out var modelElement) && TryDecodeString(modelElement, out model);
        var hasText = TryExtractOutputText(response, out var text);
        var usage = ParseUsage(response);
        if (!hasId || !hasModel || !hasText ||
            string.IsNullOrWhiteSpace(id) ||
            string.IsNullOrWhiteSpace(model) ||
            string.IsNullOrEmpty(text) ||
            usage is null)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-response-incomplete");
        }

        return RoleAttemptReceipt.Success(attempt, id, model, usage, Encoding.UTF8.GetBytes(text));
    }

    private static bool TryContainsRefusal(JsonElement response, out bool refused)
    {
        refused = false;
        if (!response.TryGetProperty("output", out var output))
        {
            return true;
        }
        if (output.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        foreach (var item in output.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                return false;
            }
            if (!item.TryGetProperty("content", out var content))
            {
                continue;
            }
            if (content.ValueKind != JsonValueKind.Array)
            {
                return false;
            }
            foreach (var part in content.EnumerateArray())
            {
                if (part.ValueKind != JsonValueKind.Object)
                {
                    return false;
                }
                if (!part.TryGetProperty("type", out var type))
                {
                    continue;
                }
                if (!TryDecodeString(type, out var typeName))
                {
                    return false;
                }
                if (string.Equals(typeName, "refusal", StringComparison.Ordinal))
                {
                    refused = true;
                }
            }
        }
        return true;
    }

    private static bool TryExtractOutputText(JsonElement response, out string? value)
    {
        value = null;
        if (response.TryGetProperty("output_text", out var direct))
        {
            return TryDecodeString(direct, out value);
        }
        if (!response.TryGetProperty("output", out var output))
        {
            return true;
        }
        if (output.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        var builder = new StringBuilder();
        foreach (var item in output.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                return false;
            }
            if (!item.TryGetProperty("content", out var content))
            {
                continue;
            }
            if (content.ValueKind != JsonValueKind.Array)
            {
                return false;
            }
            foreach (var part in content.EnumerateArray())
            {
                if (part.ValueKind != JsonValueKind.Object)
                {
                    return false;
                }
                if (!part.TryGetProperty("type", out var type))
                {
                    continue;
                }
                if (!TryDecodeString(type, out var typeName))
                {
                    return false;
                }
                if (!string.Equals(typeName, "output_text", StringComparison.Ordinal))
                {
                    continue;
                }
                if (!part.TryGetProperty("text", out var text) || !TryDecodeString(text, out var decodedText))
                {
                    return false;
                }
                builder.Append(decodedText);
            }
        }
        value = builder.Length == 0 ? null : builder.ToString();
        return true;
    }

    private static E0AUsage? ParseUsage(JsonElement response)
    {
        if (!response.TryGetProperty("usage", out var usage) || usage.ValueKind != JsonValueKind.Object ||
            !usage.TryGetProperty("input_tokens", out var input) || input.ValueKind != JsonValueKind.Number ||
            !input.TryGetInt64(out var inputTokens) ||
            !usage.TryGetProperty("output_tokens", out var output) || output.ValueKind != JsonValueKind.Number ||
            !output.TryGetInt64(out var outputTokens))
        {
            return null;
        }

        long cached = 0;
        long cacheWrite = 0;
        if (usage.TryGetProperty("input_tokens_details", out var inputDetails))
        {
            if (inputDetails.ValueKind != JsonValueKind.Object)
            {
                return null;
            }
            if (inputDetails.TryGetProperty("cached_tokens", out var cachedElement) &&
                (cachedElement.ValueKind != JsonValueKind.Number || !cachedElement.TryGetInt64(out cached)))
            {
                return null;
            }
            if (inputDetails.TryGetProperty("cache_write_tokens", out var cacheWriteElement) &&
                (cacheWriteElement.ValueKind != JsonValueKind.Number || !cacheWriteElement.TryGetInt64(out cacheWrite)))
            {
                return null;
            }
        }

        long reasoning = 0;
        if (usage.TryGetProperty("output_tokens_details", out var outputDetails))
        {
            if (outputDetails.ValueKind != JsonValueKind.Object)
            {
                return null;
            }
            if (outputDetails.TryGetProperty("reasoning_tokens", out var reasoningElement) &&
                (reasoningElement.ValueKind != JsonValueKind.Number || !reasoningElement.TryGetInt64(out reasoning)))
            {
                return null;
            }
        }

        if (inputTokens < 0 ||
            outputTokens < 0 ||
            cached < 0 ||
            cacheWrite < 0 ||
            reasoning < 0 ||
            cached > inputTokens ||
            cacheWrite > inputTokens ||
            reasoning > outputTokens)
        {
            return null;
        }

        return new E0AUsage(inputTokens, outputTokens, cached, reasoning, cacheWrite);
    }

    private static bool TryDecodeString(JsonElement element, out string? value)
    {
        value = null;
        if (element.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        try
        {
            value = element.GetString();
        }
        catch (InvalidOperationException)
        {
            return false;
        }
        catch (JsonException)
        {
            return false;
        }

        return value is not null && IsWellFormedUtf16(value);
    }

    private static bool IsWellFormedUtf16(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];
            if (char.IsHighSurrogate(current))
            {
                if (index + 1 >= value.Length || !char.IsLowSurrogate(value[index + 1]))
                {
                    return false;
                }
                index++;
            }
            else if (char.IsLowSurrogate(current))
            {
                return false;
            }
        }
        return true;
    }

    private static byte[] BuildInputTokenRequestBody(byte[] responseRequestBody)
    {
        using var source = JsonDocument.Parse(responseRequestBody);
        if (source.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new E0AHarnessException("E0-A prepared provider request body is invalid.");
        }

        var projected = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        foreach (var field in InputTokenRequestFields)
        {
            if (source.RootElement.TryGetProperty(field, out var value))
            {
                projected[field] = value.Clone();
            }
        }

        if (!projected.ContainsKey("model") ||
            !projected.ContainsKey("input") ||
            !projected.ContainsKey("instructions"))
        {
            throw new E0AHarnessException("E0-A prepared provider request cannot be projected for input-token counting.");
        }
        return JsonSerializer.SerializeToUtf8Bytes(projected);
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, byte[] body)
    {
        var request = new HttpRequestMessage(method, uri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Content = new ByteArrayContent(body);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return request;
    }
}
