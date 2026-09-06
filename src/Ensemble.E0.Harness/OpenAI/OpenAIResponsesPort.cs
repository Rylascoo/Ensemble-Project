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
            if (!document.RootElement.TryGetProperty("input_tokens", out var count) ||
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
            JsonException)
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
        using var reader = new StreamReader(body, Encoding.UTF8, false, 4096, leaveOpen: false);
        var output = new StringBuilder();
        JsonElement? completed = null;
        var refused = false;
        var failed = false;

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (line is null || !line.StartsWith("data: ", StringComparison.Ordinal))
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
            var type = root.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : null;
            switch (type)
            {
                case "response.output_text.delta":
                    if (root.TryGetProperty("delta", out var delta) && delta.ValueKind == JsonValueKind.String)
                    {
                        output.Append(delta.GetString());
                    }
                    break;
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

        return ReceiptFromResponse(attempt, completed.Value, output.Length == 0 ? null : output.ToString());
    }

    private static RoleAttemptReceipt ReceiptFromResponse(
        PreparedRoleAttempt attempt,
        JsonElement response,
        string? streamedOutput = null)
    {
        var status = response.TryGetProperty("status", out var statusElement) ? statusElement.GetString() : null;
        if (!string.Equals(status, "completed", StringComparison.Ordinal))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-incomplete");
        }

        if (ContainsRefusal(response))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-refusal");
        }

        var id = response.TryGetProperty("id", out var idElement) ? idElement.GetString() : null;
        var model = response.TryGetProperty("model", out var modelElement) ? modelElement.GetString() : null;
        var text = streamedOutput ?? ExtractOutputText(response);
        var usage = ParseUsage(response);
        if (string.IsNullOrWhiteSpace(id) ||
            string.IsNullOrWhiteSpace(model) ||
            string.IsNullOrEmpty(text) ||
            usage is null)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "provider-response-incomplete");
        }

        return RoleAttemptReceipt.Success(attempt, id, model, usage, Encoding.UTF8.GetBytes(text));
    }

    private static bool ContainsRefusal(JsonElement response)
    {
        if (!response.TryGetProperty("output", out var output) || output.ValueKind != JsonValueKind.Array)
        {
            return false;
        }
        foreach (var item in output.EnumerateArray())
        {
            if (!item.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
            {
                continue;
            }
            foreach (var part in content.EnumerateArray())
            {
                if (part.TryGetProperty("type", out var type) && string.Equals(type.GetString(), "refusal", StringComparison.Ordinal))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static string? ExtractOutputText(JsonElement response)
    {
        if (response.TryGetProperty("output_text", out var direct) && direct.ValueKind == JsonValueKind.String)
        {
            return direct.GetString();
        }
        if (!response.TryGetProperty("output", out var output) || output.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        var builder = new StringBuilder();
        foreach (var item in output.EnumerateArray())
        {
            if (!item.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
            {
                continue;
            }
            foreach (var part in content.EnumerateArray())
            {
                if (part.TryGetProperty("type", out var type) &&
                    string.Equals(type.GetString(), "output_text", StringComparison.Ordinal) &&
                    part.TryGetProperty("text", out var text) && text.ValueKind == JsonValueKind.String)
                {
                    builder.Append(text.GetString());
                }
            }
        }
        return builder.Length == 0 ? null : builder.ToString();
    }

    private static E0AUsage? ParseUsage(JsonElement response)
    {
        if (!response.TryGetProperty("usage", out var usage) || usage.ValueKind != JsonValueKind.Object ||
            !usage.TryGetProperty("input_tokens", out var input) || !input.TryGetInt64(out var inputTokens) ||
            !usage.TryGetProperty("output_tokens", out var output) || !output.TryGetInt64(out var outputTokens))
        {
            return null;
        }

        long cached = 0;
        if (usage.TryGetProperty("input_tokens_details", out var inputDetails) &&
            inputDetails.ValueKind == JsonValueKind.Object &&
            inputDetails.TryGetProperty("cached_tokens", out var cachedElement) &&
            !cachedElement.TryGetInt64(out cached))
        {
            return null;
        }

        long reasoning = 0;
        if (usage.TryGetProperty("output_tokens_details", out var outputDetails) &&
            outputDetails.ValueKind == JsonValueKind.Object &&
            outputDetails.TryGetProperty("reasoning_tokens", out var reasoningElement) &&
            !reasoningElement.TryGetInt64(out reasoning))
        {
            return null;
        }

        if (inputTokens < 0 ||
            outputTokens < 0 ||
            cached < 0 ||
            reasoning < 0 ||
            cached > inputTokens)
        {
            return null;
        }

        return new E0AUsage(inputTokens, outputTokens, cached, reasoning);
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
