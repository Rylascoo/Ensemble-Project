namespace Ensemble.E0.Harness.Gemini;

internal sealed class E0AGeminiCountTokensFailureException : Exception
{
    private E0AGeminiCountTokensFailureException(string diagnostic) : base(diagnostic) =>
        Diagnostic = diagnostic;

    internal string Diagnostic { get; }

    internal static E0AGeminiCountTokensFailureException ResponseInvalid() =>
        new("gemini-counttokens-response-invalid");

    internal static E0AGeminiCountTokensFailureException Transport() =>
        new("gemini-counttokens-transport");

    internal static async Task<E0AGeminiCountTokensFailureException> FromHttpResponseAsync(
        HttpResponseMessage response,
        byte[] requestBody,
        CancellationToken cancellationToken) =>
        new(await E0AGeminiHttpFailureDiagnostic
            .CountTokensAsync(response, requestBody, cancellationToken)
            .ConfigureAwait(false));
}
