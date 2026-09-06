using System.Text;
using System.Text.Json;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Gemini;

internal sealed class GeminiGenerateContentPort : IE0AProviderRolePort, IE0AInputTokenCounter
{
    private const string ApiBase = "https://generativelanguage.googleapis.com/v1beta/models/";
    private readonly HttpClient _http;
    private readonly string _apiKey;

    internal GeminiGenerateContentPort(HttpClient http, string apiKey)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new E0AHarnessException("GEMINI_API_KEY is required at the E0-A provider edge.");
        }
        _apiKey = apiKey;
    }

    internal static GeminiGenerateContentPort FromEnvironment(HttpClient http)
    {
        var key = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new E0AHarnessException("GEMINI_API_KEY is required at the E0-A provider edge.");
        }
        return new GeminiGenerateContentPort(http, key);
    }

    public async Task<long> CountInputTokensAsync(
        PreparedRoleAttempt attempt,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attempt);
        RequireGeminiAttempt(attempt);
        try
        {
            using var source = JsonDocument.Parse(attempt.RequestBody);
            if (source.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new E0AHarnessException("E0-A Gemini prepared request body is invalid.");
            }

            var body = JsonSerializer.SerializeToUtf8Bytes(new Dictionary<string, object?>
            {
                ["generateContentRequest"] = source.RootElement.Clone()
            });
            using var request = CreateRequest(ModelUri(attempt.Profile.Model, "countTokens"), body);
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new E0AHarnessException("E0-A Gemini input-token preflight failed.");
            }

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            using var document = JsonDocument.Parse(bytes);
            if (!document.RootElement.TryGetProperty("totalTokens", out var count) ||
                !count.TryGetInt64(out var tokens) || tokens < 0)
            {
                throw new E0AHarnessException("E0-A Gemini input-token response is invalid.");
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
            throw new E0AHarnessException("E0-A Gemini input-token preflight failed.");
        }
    }

    public async Task<RoleAttemptReceipt> ExecuteAsync(
        PreparedRoleAttempt attempt,
        IE0AProviderDiagnosticSink diagnostics,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attempt);
        ArgumentNullException.ThrowIfNull(diagnostics);
        RequireGeminiAttempt(attempt);
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
            OverflowException)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-malformed-or-transport");
        }
    }

    private async Task<RoleAttemptReceipt> ExecuteBufferedAsync(
        PreparedRoleAttempt attempt,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(ModelUri(attempt.Profile.Model, "generateContent"), attempt.RequestBody);
        using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, $"gemini-http-{(int)response.StatusCode}");
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
        using var request = CreateRequest(ModelUri(attempt.Profile.Model, "streamGenerateContent", "alt=sse"), attempt.RequestBody);
        using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, $"gemini-http-{(int)response.StatusCode}");
        }

        await using var body = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var reader = new StreamReader(body, Encoding.UTF8, false, 4096, leaveOpen: false);
        var text = new StringBuilder();
        string? responseId = null;
        string? modelVersion = null;
        string? finishReason = null;
        E0AUsage? usage = null;

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: ", StringComparison.Ordinal))
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

            if (PromptBlocked(root))
            {
                return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-prompt-blocked");
            }
            if (!BindStableIdentity(root, ref responseId, ref modelVersion))
            {
                return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-response-identity-changed");
            }

            if (!AppendCandidateText(root, text, ref finishReason, out var candidateError))
            {
                return RoleAttemptReceipt.TechnicalFailure(attempt, candidateError!);
            }

            // Intermediate SSE chunks may expose partial usage metadata. Only the
            // STOP chunk is authoritative for the complete cumulative usage tuple.
            if (string.Equals(finishReason, "STOP", StringComparison.Ordinal) &&
                root.TryGetProperty("usageMetadata", out var usageElement))
            {
                usage = ParseUsage(usageElement);
                if (usage is null)
                {
                    return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-usage-invalid");
                }
            }
        }

        if (!string.Equals(finishReason, "STOP", StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(responseId) ||
            string.IsNullOrWhiteSpace(modelVersion) ||
            text.Length == 0 ||
            usage is null)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-response-incomplete");
        }

        return RoleAttemptReceipt.Success(
            attempt,
            responseId,
            modelVersion,
            usage,
            Encoding.UTF8.GetBytes(text.ToString()));
    }

    private static RoleAttemptReceipt ReceiptFromResponse(
        PreparedRoleAttempt attempt,
        JsonElement response)
    {
        if (PromptBlocked(response))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-prompt-blocked");
        }

        var responseId = response.TryGetProperty("responseId", out var idElement) ? idElement.GetString() : null;
        var modelVersion = response.TryGetProperty("modelVersion", out var modelElement) ? modelElement.GetString() : null;
        if (string.IsNullOrWhiteSpace(responseId) || string.IsNullOrWhiteSpace(modelVersion))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-response-identity-missing");
        }

        var text = new StringBuilder();
        string? finishReason = null;
        if (!AppendCandidateText(response, text, ref finishReason, out var candidateError))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, candidateError!);
        }
        if (!string.Equals(finishReason, "STOP", StringComparison.Ordinal) || text.Length == 0)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-response-incomplete");
        }

        if (!response.TryGetProperty("usageMetadata", out var usageElement))
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-usage-missing");
        }
        var usage = ParseUsage(usageElement);
        if (usage is null)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-usage-invalid");
        }

        return RoleAttemptReceipt.Success(
            attempt,
            responseId,
            modelVersion,
            usage,
            Encoding.UTF8.GetBytes(text.ToString()));
    }

    private static bool PromptBlocked(JsonElement response)
    {
        if (!response.TryGetProperty("promptFeedback", out var feedback) || feedback.ValueKind != JsonValueKind.Object)
        {
            return false;
        }
        return feedback.TryGetProperty("blockReason", out var blockReason) &&
               blockReason.ValueKind == JsonValueKind.String &&
               !string.IsNullOrWhiteSpace(blockReason.GetString()) &&
               !string.Equals(blockReason.GetString(), "BLOCK_REASON_UNSPECIFIED", StringComparison.Ordinal);
    }

    private static bool BindStableIdentity(
        JsonElement response,
        ref string? responseId,
        ref string? modelVersion)
    {
        var id = response.TryGetProperty("responseId", out var idElement) ? idElement.GetString() : null;
        var model = response.TryGetProperty("modelVersion", out var modelElement) ? modelElement.GetString() : null;
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(model))
        {
            return false;
        }
        if (responseId is null)
        {
            responseId = id;
            modelVersion = model;
            return true;
        }
        return string.Equals(responseId, id, StringComparison.Ordinal) &&
               string.Equals(modelVersion, model, StringComparison.Ordinal);
    }

    private static bool AppendCandidateText(
        JsonElement response,
        StringBuilder target,
        ref string? finishReason,
        out string? error)
    {
        error = null;
        if (!response.TryGetProperty("candidates", out var candidates) || candidates.ValueKind != JsonValueKind.Array)
        {
            return true;
        }
        var items = candidates.EnumerateArray().ToArray();
        if (items.Length != 1)
        {
            error = "gemini-candidate-count-invalid";
            return false;
        }
        var candidate = items[0];
        if (candidate.TryGetProperty("finishReason", out var finishElement) && finishElement.ValueKind == JsonValueKind.String)
        {
            var observed = finishElement.GetString();
            if (!string.IsNullOrWhiteSpace(finishReason) &&
                !string.IsNullOrWhiteSpace(observed) &&
                !string.Equals(finishReason, observed, StringComparison.Ordinal))
            {
                error = "gemini-finish-reason-changed";
                return false;
            }
            finishReason = observed;
        }

        if (!candidate.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Object ||
            !content.TryGetProperty("parts", out var parts) || parts.ValueKind != JsonValueKind.Array)
        {
            return true;
        }
        foreach (var part in parts.EnumerateArray())
        {
            if (part.ValueKind != JsonValueKind.Object ||
                part.TryGetProperty("thoughtSignature", out _) ||
                (part.TryGetProperty("thought", out var thought) && thought.ValueKind == JsonValueKind.True))
            {
                error = "gemini-thought-material-returned";
                return false;
            }
            if (!part.TryGetProperty("text", out var text) || text.ValueKind != JsonValueKind.String)
            {
                error = "gemini-nontext-part-returned";
                return false;
            }
            target.Append(text.GetString());
        }
        return true;
    }

    private static E0AUsage? ParseUsage(JsonElement usage)
    {
        if (usage.ValueKind != JsonValueKind.Object ||
            !TryNonNegativeInt64(usage, "promptTokenCount", out var input) ||
            !TryNonNegativeInt64(usage, "candidatesTokenCount", out var candidate))
        {
            return null;
        }
        if (!TryOptionalNonNegativeInt64(usage, "thoughtsTokenCount", out var thoughts) ||
            !TryOptionalNonNegativeInt64(usage, "cachedContentTokenCount", out var cached))
        {
            return null;
        }

        long output;
        try
        {
            output = checked(candidate + thoughts);
        }
        catch (OverflowException)
        {
            return null;
        }
        if (cached > input)
        {
            return null;
        }
        return new E0AUsage(input, output, cached, thoughts, 0);
    }

    private static bool TryNonNegativeInt64(JsonElement root, string name, out long value)
    {
        value = 0;
        return root.TryGetProperty(name, out var element) &&
               element.TryGetInt64(out value) &&
               value >= 0;
    }

    private static bool TryOptionalNonNegativeInt64(JsonElement root, string name, out long value)
    {
        value = 0;
        if (!root.TryGetProperty(name, out var element))
        {
            return true;
        }
        return element.TryGetInt64(out value) && value >= 0;
    }

    private HttpRequestMessage CreateRequest(Uri uri, byte[] body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.TryAddWithoutValidation("x-goog-api-key", _apiKey);
        request.Content = new ByteArrayContent(body);
        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        return request;
    }

    private static Uri ModelUri(string model, string method, string? query = null)
    {
        var suffix = query is null ? string.Empty : $"?{query}";
        return new Uri($"{ApiBase}{Uri.EscapeDataString(model)}:{method}{suffix}");
    }

    private static void RequireGeminiAttempt(PreparedRoleAttempt attempt)
    {
        if (!string.Equals(attempt.Profile.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal) ||
            !string.Equals(attempt.Profile.Model, E0AGeminiProviderPolicy.Model, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Gemini provider received an attempt outside the approved route.");
        }
    }
}
