using System.Security.Cryptography;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Harness.Run;

internal enum E0ARoleAttemptOutcome
{
    Success = 1,
    TechnicalFailure = 2,
    Cancelled = 3
}

internal sealed class PreparedRoleAttempt
{
    internal PreparedRoleAttempt(
        RunId runId,
        string attemptId,
        E0ARoleProfile profile,
        int turn,
        CharacterId? characterId,
        ContextPacketId contextPacketId,
        string structuredContextHash,
        string renderedContextHash,
        string? candidateContentHash,
        string promptHash,
        string responseSchemaHash,
        string? integrityPacketHash,
        byte[] requestBody)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(requestBody);
        E0ADeterministicIds.ValidateRunId(runId);
        profile.Validate();

        if (turn is < 1 or > E0ARunEnvelope.AcceptedTurnCap ||
            string.IsNullOrWhiteSpace(attemptId) ||
            string.IsNullOrWhiteSpace(structuredContextHash) ||
            string.IsNullOrWhiteSpace(renderedContextHash) ||
            string.IsNullOrWhiteSpace(promptHash) ||
            string.IsNullOrWhiteSpace(responseSchemaHash))
        {
            throw new E0AHarnessException("E0-A prepared role attempt is invalid.");
        }

        _ = contextPacketId.Value;
        if (characterId.HasValue)
        {
            _ = characterId.Value.Value;
        }

        RunId = runId;
        AttemptId = attemptId;
        Profile = profile;
        Turn = turn;
        CharacterId = characterId;
        ContextPacketId = contextPacketId;
        StructuredContextHash = structuredContextHash;
        RenderedContextHash = renderedContextHash;
        CandidateContentHash = candidateContentHash;
        PromptHash = promptHash;
        ResponseSchemaHash = responseSchemaHash;
        IntegrityPacketHash = integrityPacketHash;
        RequestBody = requestBody.ToArray();
        RequestBodyHash = LowerSha256(RequestBody);
        IdentityHash = ComputeIdentityHash();
    }

    internal RunId RunId { get; }
    internal string AttemptId { get; }
    internal E0ARoleProfile Profile { get; }
    internal int Turn { get; }
    internal CharacterId? CharacterId { get; }
    internal ContextPacketId ContextPacketId { get; }
    internal string StructuredContextHash { get; }
    internal string RenderedContextHash { get; }
    internal string? CandidateContentHash { get; }
    internal string PromptHash { get; }
    internal string ResponseSchemaHash { get; }
    internal string? IntegrityPacketHash { get; }
    internal byte[] RequestBody { get; }
    internal string RequestBodyHash { get; }
    internal string IdentityHash { get; }

    private string ComputeIdentityHash()
    {
        var value = string.Join(
            "\n",
            RunId.Value,
            AttemptId,
            Profile.Role,
            Profile.Provider,
            Profile.Model,
            Profile.Reasoning,
            Profile.Stream,
            Profile.MaxOutputTokens,
            Profile.ServiceTier,
            Turn,
            CharacterId.HasValue ? CharacterId.Value.Value : string.Empty,
            ContextPacketId.Value,
            StructuredContextHash,
            RenderedContextHash,
            CandidateContentHash ?? string.Empty,
            PromptHash,
            ResponseSchemaHash,
            IntegrityPacketHash ?? string.Empty,
            RequestBodyHash);
        return LowerSha256(System.Text.Encoding.UTF8.GetBytes(value));
    }

    internal static string LowerSha256(ReadOnlySpan<byte> bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
}

internal sealed record E0AUsage(
    long InputTokens,
    long OutputTokens,
    long CachedInputTokens = 0,
    long ReasoningTokens = 0)
{
    internal void Validate()
    {
        if (InputTokens < 0 || OutputTokens < 0 || CachedInputTokens < 0 || ReasoningTokens < 0 ||
            CachedInputTokens > InputTokens)
        {
            throw new E0AHarnessException("E0-A provider usage is invalid.");
        }
    }
}

internal sealed class RoleAttemptReceipt
{
    private RoleAttemptReceipt(
        string attemptId,
        string preparedIdentityHash,
        E0ARoleAttemptOutcome outcome,
        string? responseId,
        string? returnedModel,
        E0AUsage? usage,
        byte[]? structuredOutput,
        string? diagnosticCode)
    {
        AttemptId = attemptId;
        PreparedIdentityHash = preparedIdentityHash;
        Outcome = outcome;
        ResponseId = responseId;
        ReturnedModel = returnedModel;
        Usage = usage;
        StructuredOutput = structuredOutput?.ToArray();
        StructuredOutputHash = structuredOutput is null
            ? null
            : PreparedRoleAttempt.LowerSha256(structuredOutput);
        DiagnosticCode = diagnosticCode;
    }

    internal string AttemptId { get; }
    internal string PreparedIdentityHash { get; }
    internal E0ARoleAttemptOutcome Outcome { get; }
    internal string? ResponseId { get; }
    internal string? ReturnedModel { get; }
    internal E0AUsage? Usage { get; }
    internal byte[]? StructuredOutput { get; }
    internal string? StructuredOutputHash { get; }
    internal string? DiagnosticCode { get; }

    internal static RoleAttemptReceipt Success(
        PreparedRoleAttempt attempt,
        string responseId,
        string returnedModel,
        E0AUsage usage,
        byte[] structuredOutput)
    {
        ArgumentNullException.ThrowIfNull(attempt);
        ArgumentNullException.ThrowIfNull(usage);
        ArgumentNullException.ThrowIfNull(structuredOutput);
        usage.Validate();
        if (string.IsNullOrWhiteSpace(responseId) ||
            string.IsNullOrWhiteSpace(returnedModel) ||
            structuredOutput.Length == 0)
        {
            throw new E0AHarnessException("E0-A successful receipt is invalid.");
        }

        return new RoleAttemptReceipt(
            attempt.AttemptId,
            attempt.IdentityHash,
            E0ARoleAttemptOutcome.Success,
            responseId,
            returnedModel,
            usage,
            structuredOutput,
            null);
    }

    internal static RoleAttemptReceipt TechnicalFailure(
        PreparedRoleAttempt attempt,
        string diagnosticCode) =>
        Technical(attempt, E0ARoleAttemptOutcome.TechnicalFailure, diagnosticCode);

    internal static RoleAttemptReceipt Cancelled(
        PreparedRoleAttempt attempt,
        string diagnosticCode) =>
        Technical(attempt, E0ARoleAttemptOutcome.Cancelled, diagnosticCode);

    private static RoleAttemptReceipt Technical(
        PreparedRoleAttempt attempt,
        E0ARoleAttemptOutcome outcome,
        string diagnosticCode)
    {
        ArgumentNullException.ThrowIfNull(attempt);
        if (string.IsNullOrWhiteSpace(diagnosticCode))
        {
            throw new E0AHarnessException("E0-A technical receipt diagnostic code is required.");
        }

        return new RoleAttemptReceipt(
            attempt.AttemptId,
            attempt.IdentityHash,
            outcome,
            null,
            null,
            null,
            null,
            diagnosticCode);
    }
}

internal interface IE0AProviderDiagnosticSink
{
    void RecordStreamEvent(PreparedRoleAttempt attempt, ReadOnlyMemory<byte> utf8Event);
}

internal interface IE0AProviderRolePort
{
    Task<RoleAttemptReceipt> ExecuteAsync(
        PreparedRoleAttempt attempt,
        IE0AProviderDiagnosticSink diagnostics,
        CancellationToken cancellationToken);
}

internal interface IE0AInputTokenCounter
{
    Task<long> CountInputTokensAsync(
        PreparedRoleAttempt attempt,
        CancellationToken cancellationToken);
}
