using System.Text;
using System.Text.Json;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Gemini;

internal sealed class GeminiGenerateContentPort : IE0AProviderRolePort, IE0AInputTokenCounter
{
    private const string ApiBase = "https://generativelanguage.googleapis.com/v1beta/models/";
    private static readonly Encoding StrictUtf8 = new UTF8Encoding(false, true);
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
            if (document.RootElement.ValueKind != JsonValueKind.Object ||
                !document.RootElement.TryGetProperty("totalTokens", out var count) ||
                count.ValueKind != JsonValueKind.Number ||
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
            DecoderFallbackException or
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
        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-response-shape-invalid");
        }
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
        using var reader = new StreamReader(body, StrictUtf8, false, 4096, leaveOpen: false);
        var text = new StringBuilder();
        string? responseId = null;
        string? modelVersion = null;
        string? finishReason = null;
        E0AUsage? usage = null;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (line is null)
            {
                break;
            }
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:", StringComparison.Ordinal))
            {
                continue;
            }

            var data = line[5..];
            if (data.StartsWith(' '))
            {
                data = data[1..];
            }
            if (string.IsNullOrWhiteSpace(data) || string.Equals(data, "[DONE]", StringComparison.Ordinal))
            {
                continue;
            }

            var utf8 = Encoding.UTF8.GetBytes(data);
            using var eventDoc = JsonDocument.Parse(utf8);
            var root = eventDoc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    "gemini-response-shape-invalid",
                    responseId,
                    modelVersion);
            }

            // Thought summaries remain outside E0-A evidence authority. Approved
            // Gemini 3 Flash-Lite routes may attach an opaque thoughtSignature to
            // otherwise semantic text; accept that provider metadata only where the
            // explicit model profile permits it and strip it before persistence.
            var thoughtDiagnostic = ThoughtMetadataDiagnostic(attempt, root);
            if (thoughtDiagnostic is not null)
            {
                return RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    thoughtDiagnostic,
                    responseId,
                    modelVersion);
            }
            diagnostics.RecordStreamEvent(attempt, DiagnosticEventBytes(attempt, root, utf8));

            if (!TryPromptBlocked(root, out var promptBlocked))
            {
                return RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    "gemini-response-shape-invalid",
                    responseId,
                    modelVersion);
            }
            if (!BindStableIdentity(root, ref responseId, ref modelVersion))
            {
                return RoleAttemptReceipt.TechnicalFailure(attempt, "gemini-response-identity-changed");
            }
            if (promptBlocked)
            {
                return RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    "gemini-prompt-blocked",
                    responseId,
                    modelVersion);
            }

            var alreadyStopped = string.Equals(finishReason, "STOP", StringComparison.Ordinal);
            var textLengthBefore = text.Length;
            if (!AppendCandidateText(root, text, ref finishReason, out var candidateError))
            {
                return RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    candidateError!,
                    responseId,
                    modelVersion);
            }
            if (alreadyStopped && text.Length != textLengthBefore)
            {
                return RoleAttemptReceipt.TechnicalFailure(
                    attempt,
                    "gemini-content-after-stop",
                    responseId,
                    modelVersion);
            }

            if (!string.IsNullOrWhiteSpace(finishReason) &&
                root.TryGetProperty("usageMetadata", out var usageElement))
            {
                usage = ParseUsage(usageElement);
                if (usage is null)
                {
                    return RoleAttemptReceipt.TechnicalFailure(
                        attempt,
                        "gemini-usage-invalid",
                        responseId,
                        modelVersion);
                }
            }
        }

        if (!string.Equals(finishReason, "STOP", StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(responseId) ||
            string.IsNullOrWhiteSpace(modelVersion) ||
            text.Length == 0 ||
            usage is null)
        {
            return RoleAttemptReceipt.TechnicalFailure(
                attempt,
                "gemini-response-incomplete",
                responseId,
                modelVersion,
                usage);
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
        // Thought summaries are never eligible for semantic adoption. Approved
        // Gemini 3 opaque signatures are provider metadata and are ignored rather
        // than entering the receipt or semantic output.
        var thoughtDiagnostic = ThoughtMetadataDiagnostic(attempt, response);
        if (thoughtDiagnostic is not null)
        {
            return RoleAttemptReceipt.TechnicalFailure(attempt, thoughtDiagnostic);
        }

        var metadataDiagnostic = ReadProviderMetadata(
            response,
            out var responseId,
            out var modelVersion,
            out var usage);
        if (metadataDiagnostic is not null)
        {
            return RoleAttemptReceipt.TechnicalFailure(
                attempt,
                metadataDiagnostic,
                responseId,
                modelVersion,
                usage);
        }

        if (!TryPromptBlocked(response, out var promptBlocked))
        {
            return RoleAttemptReceipt.TechnicalFailure(
                attempt,
                "gemini-response-shape-invalid",
                responseId,
                modelVersion,
                usage);
        }
        if (promptBlocked)
        {
            return RoleAttemptReceipt.TechnicalFailure(
                attempt,
                "gemini-prompt-blocked",
                responseId,
                modelVersion,
                usage);
        }

        var text = new StringBuilder();
        string? finishReason = null;
        if (!AppendCandidateText(response, text, ref finishReason, out var candidateError))
        {
            return RoleAttemptReceipt.TechnicalFailure(
                attempt,
                candidateError!,
                responseId,
                modelVersion,
                usage);
        }
        if (!string.Equals(finishReason, "STOP", StringComparison.Ordinal) ||
            text.Length == 0 ||
            string.IsNullOrWhiteSpace(responseId) ||
            string.IsNullOrWhiteSpace(modelVersion) ||
            usage is null)
        {
            return RoleAttemptReceipt.TechnicalFailure(
                attempt,
                "gemini-response-incomplete",
                responseId,
                modelVersion,
                usage);
        }

        return RoleAttemptReceipt.Success(
            attempt,
            responseId,
            modelVersion,
            usage,
            Encoding.UTF8.GetBytes(text.ToString()));
    }

    private static string? ReadProviderMetadata(
        JsonElement response,
        out string? responseId,
        out string? modelVersion,
        out E0AUsage? usage)
    {
        responseId = null;
        modelVersion = null;
        usage = null;
        if (response.ValueKind != JsonValueKind.Object)
        {
            return "gemini-response-shape-invalid";
        }

        string? diagnostic = null;
        if (response.TryGetProperty("responseId", out var idElement) &&
            (!TryDecodeString(idElement, out responseId) || string.IsNullOrWhiteSpace(responseId)))
        {
            responseId = null;
            diagnostic ??= "gemini-response-identity-missing";
        }
        if (response.TryGetProperty("modelVersion", out var modelElement) &&
            (!TryDecodeString(modelElement, out modelVersion) || string.IsNullOrWhiteSpace(modelVersion)))
        {
            modelVersion = null;
            diagnostic ??= "gemini-response-identity-missing";
        }
        if (response.TryGetProperty("usageMetadata", out var usageElement))
        {
            usage = ParseUsage(usageElement);
            if (usage is null)
            {
                diagnostic ??= "gemini-usage-invalid";
            }
        }

        return diagnostic;
    }

    private static bool TryPromptBlocked(JsonElement response, out bool blocked)
    {
        blocked = false;
        if (!response.TryGetProperty("promptFeedback", out var feedback))
        {
            return true;
        }
        if (feedback.ValueKind != JsonValueKind.Object)
        {
            return false;
        }
        if (!feedback.TryGetProperty("blockReason", out var blockReason))
        {
            return true;
        }
        if (!TryDecodeString(blockReason, out var reason))
        {
            return false;
        }

        blocked = !string.IsNullOrWhiteSpace(reason) &&
                  !string.Equals(reason, "BLOCK_REASON_UNSPECIFIED", StringComparison.Ordinal);
        return true;
    }

    private static bool BindStableIdentity(
        JsonElement response,
        ref string? responseId,
        ref string? modelVersion)
    {
        var hasId = response.TryGetProperty("responseId", out var idElement);
        var hasModel = response.TryGetProperty("modelVersion", out var modelElement);
        if (!hasId && !hasModel)
        {
            return true;
        }
        if (!hasId || !hasModel ||
            !TryDecodeString(idElement, out var id) ||
            !TryDecodeString(modelElement, out var model) ||
            string.IsNullOrWhiteSpace(id) ||
            string.IsNullOrWhiteSpace(model))
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

    private static string? ThoughtMetadataDiagnostic(
        PreparedRoleAttempt attempt,
        JsonElement response)
    {
        if (ContainsExplicitThoughtMaterial(response))
        {
            return "gemini-thought-material-returned";
        }
        if (!AllowsOpaqueThoughtSignature(attempt) && ContainsThoughtSignature(response))
        {
            return "gemini-thought-material-returned";
        }
        return null;
    }

    private static bool AllowsOpaqueThoughtSignature(PreparedRoleAttempt attempt) =>
        E0AGeminiModelCatalog.ForModel(attempt.Profile.Model).AllowsOpaqueThoughtSignature;

    private static bool ContainsExplicitThoughtMaterial(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if ((property.NameEquals("thought") && property.Value.ValueKind == JsonValueKind.True) ||
                    ContainsExplicitThoughtMaterial(property.Value))
                {
                    return true;
                }
            }
            return false;
        }
        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                if (ContainsExplicitThoughtMaterial(item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool ContainsThoughtSignature(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("thoughtSignature") || ContainsThoughtSignature(property.Value))
                {
                    return true;
                }
            }
            return false;
        }
        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                if (ContainsThoughtSignature(item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static byte[] DiagnosticEventBytes(
        PreparedRoleAttempt attempt,
        JsonElement response,
        byte[] original)
    {
        if (!AllowsOpaqueThoughtSignature(attempt) || !ContainsThoughtSignature(response))
        {
            return original;
        }

        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            WriteWithoutThoughtSignatures(response, writer);
        }
        return stream.ToArray();
    }

    private static void WriteWithoutThoughtSignatures(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    if (property.NameEquals("thoughtSignature"))
                    {
                        continue;
                    }
                    writer.WritePropertyName(property.Name);
                    WriteWithoutThoughtSignatures(property.Value, writer);
                }
                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    WriteWithoutThoughtSignatures(item, writer);
                }
                writer.WriteEndArray();
                break;
            default:
                element.WriteTo(writer);
                break;
        }
    }

    private static bool AppendCandidateText(
        JsonElement response,
        StringBuilder target,
        ref string? finishReason,
        out string? error)
    {
        error = null;
        if (!response.TryGetProperty("candidates", out var candidates))
        {
            return true;
        }
        if (candidates.ValueKind != JsonValueKind.Array)
        {
            error = "gemini-response-shape-invalid";
            return false;
        }

        var items = candidates.EnumerateArray().ToArray();
        if (items.Length != 1)
        {
            error = "gemini-candidate-count-invalid";
            return false;
        }
        var candidate = items[0];
        if (candidate.ValueKind != JsonValueKind.Object)
        {
            error = "gemini-response-shape-invalid";
            return false;
        }

        if (candidate.TryGetProperty("finishReason", out var finishElement))
        {
            if (!TryDecodeString(finishElement, out var observed) || string.IsNullOrWhiteSpace(observed))
            {
                error = "gemini-response-shape-invalid";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(finishReason) &&
                !string.Equals(finishReason, observed, StringComparison.Ordinal))
            {
                error = "gemini-finish-reason-changed";
                return false;
            }
            finishReason = observed;
        }

        if (!candidate.TryGetProperty("content", out var content))
        {
            return true;
        }
        if (content.ValueKind != JsonValueKind.Object)
        {
            error = "gemini-response-shape-invalid";
            return false;
        }
        if (!content.TryGetProperty("parts", out var parts))
        {
            return true;
        }
        if (parts.ValueKind != JsonValueKind.Array)
        {
            error = "gemini-response-shape-invalid";
            return false;
        }

        foreach (var part in parts.EnumerateArray())
        {
            if (part.ValueKind != JsonValueKind.Object)
            {
                error = "gemini-response-shape-invalid";
                return false;
            }
            if (part.TryGetProperty("thought", out var thought))
            {
                if (thought.ValueKind is not JsonValueKind.True and not JsonValueKind.False)
                {
                    error = "gemini-response-shape-invalid";
                    return false;
                }
                if (thought.ValueKind == JsonValueKind.True)
                {
                    error = "gemini-thought-material-returned";
                    return false;
                }
            }
            if (!part.TryGetProperty("text", out var text) ||
                !TryDecodeString(text, out var decodedText))
            {
                error = "gemini-nontext-part-returned";
                return false;
            }
            target.Append(decodedText);
        }
        return true;
    }

    private static E0AUsage? ParseUsage(JsonElement usage)
    {
        if (usage.ValueKind != JsonValueKind.Object ||
            !TryNonNegativeInt64(usage, "promptTokenCount", out var input) ||
            !TryNonNegativeInt64(usage, "candidatesTokenCount", out var candidate) ||
            !TryNonNegativeInt64(usage, "totalTokenCount", out var providerTotal) ||
            !TryOptionalNonNegativeInt64(usage, "thoughtsTokenCount", out var thoughts) ||
            !TryOptionalNonNegativeInt64(usage, "cachedContentTokenCount", out var cached) ||
            !TryOptionalNonNegativeInt64(usage, "toolUsePromptTokenCount", out var toolUsePromptTokens) ||
            toolUsePromptTokens != 0)
        {
            return null;
        }

        long output;
        long expectedTotal;
        try
        {
            output = checked(candidate + thoughts);
            expectedTotal = checked(input + output);
        }
        catch (OverflowException)
        {
            return null;
        }
        if (cached > input || providerTotal != expectedTotal)
        {
            return null;
        }
        return new E0AUsage(input, output, cached, thoughts, 0);
    }

    private static bool TryNonNegativeInt64(JsonElement root, string name, out long value)
    {
        value = 0;
        return root.TryGetProperty(name, out var element) &&
               element.ValueKind == JsonValueKind.Number &&
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
        return element.ValueKind == JsonValueKind.Number &&
               element.TryGetInt64(out value) &&
               value >= 0;
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
        if (!string.Equals(attempt.Profile.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Gemini provider received an attempt outside the approved route.");
        }

        _ = E0AGeminiModelCatalog.ForModel(attempt.Profile.Model);
    }
}
