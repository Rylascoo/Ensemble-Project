using System.Text.Json;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.StateInterpreter;

namespace Ensemble.E0.Harness.Run;

internal static class E0ARequestBuilder
{
    internal static PreparedRoleAttempt Performer(
        Ensemble.E0.Core.Domain.RunId runId,
        int turn,
        E0ARoleProfile profile,
        ContextPacket context)
    {
        RequireContext(context);
        var data = JsonSerializer.Serialize(new { context = E0APromptContracts.ContextData(context) });
        var body = BuildBody(profile, E0APromptContracts.PerformerInstructions, data, "e0a_performer_candidate", E0APromptContracts.PerformerSchemaJson);
        return New(runId, turn, profile, context, null, E0APromptContracts.PerformerPromptHash, E0APromptContracts.PerformerSchemaHash, null, body);
    }

    internal static PreparedRoleAttempt Integrity(
        Ensemble.E0.Core.Domain.RunId runId,
        int turn,
        E0ARoleProfile profile,
        ContextPacket context,
        string candidateContentHash,
        E0AIntegrityAssessmentPacket packet)
    {
        RequireContext(context);
        ArgumentNullException.ThrowIfNull(packet);
        IntegrityCandidateInput input;
        try
        {
            input = IntegrityCandidateInput.Bind(context, packet.Candidate);
        }
        catch (IntegrityValidationException)
        {
            throw new E0AHarnessException("E0-A Integrity request Candidate association is invalid.");
        }
        if (input.DeterministicRejectCodes.Length != 0 ||
            packet.Context.ContextPacketId != context.ContextPacketId ||
            !string.Equals(input.CandidateContentHash, candidateContentHash, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Integrity request inputs are not bound to one exact Candidate source.");
        }

        var data = JsonSerializer.Serialize(packet.ToTransport());
        var body = BuildBody(profile, E0APromptContracts.IntegrityInstructions, data, "e0a_integrity_concerns", E0APromptContracts.IntegritySchemaJson);
        return New(runId, turn, profile, context, candidateContentHash, E0APromptContracts.IntegrityPromptHash, E0APromptContracts.IntegritySchemaHash, packet.PacketHash, body);
    }

    internal static PreparedRoleAttempt Interpreter(
        Ensemble.E0.Core.Domain.RunId runId,
        int turn,
        E0ARoleProfile profile,
        ContextPacket context,
        CandidatePerformance candidate,
        StateInterpretationSource source)
    {
        RequireContext(context);
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(source);
        IntegrityCandidateInput input;
        try
        {
            input = IntegrityCandidateInput.Bind(context, candidate);
        }
        catch (IntegrityValidationException)
        {
            throw new E0AHarnessException("E0-A Interpreter request Candidate association is invalid.");
        }
        if (input.DeterministicRejectCodes.Length != 0 ||
            source.SourceContextPacketId != context.ContextPacketId ||
            source.SourceSceneId != context.SceneId ||
            source.SourceCharacterId != context.SubjectCharacterId ||
            !string.Equals(source.CandidateContentHash, input.CandidateContentHash, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Interpreter request inputs are not bound to one exact Interpretation source.");
        }

        var data = JsonSerializer.Serialize(new
        {
            context = E0APromptContracts.ContextData(context),
            candidate = new
            {
                candidateContentHash = source.CandidateContentHash,
                subjectCharacterId = candidate.SubjectCharacterId.Value,
                contextPacketId = candidate.ContextPacketId.Value,
                visibleText = candidate.VisibleText,
                addressedCharacterIds = candidate.Control.AddressedCharacterIds.Select(x => x.Value).ToArray(),
                nominatedCharacterId = candidate.Control.NominatedCharacterId.HasValue ? candidate.Control.NominatedCharacterId.Value.Value : null
            }
        });
        var body = BuildBody(profile, E0APromptContracts.InterpreterInstructions, data, "e0a_state_interpretation", E0APromptContracts.InterpreterSchemaJson);
        return New(runId, turn, profile, context, source.CandidateContentHash, E0APromptContracts.InterpreterPromptHash, E0APromptContracts.InterpreterSchemaHash, null, body);
    }

    private static PreparedRoleAttempt New(
        Ensemble.E0.Core.Domain.RunId runId,
        int turn,
        E0ARoleProfile profile,
        ContextPacket context,
        string? candidateContentHash,
        string promptHash,
        string schemaHash,
        string? integrityPacketHash,
        byte[] body) =>
        new(
            runId,
            E0ADeterministicIds.Attempt(runId, profile.Role, turn, E0ARunEnvelope.AttemptsPerRoleInvocation),
            profile,
            turn,
            context.SubjectCharacterId,
            context.ContextPacketId,
            context.StructuredContextHash,
            context.RenderedContextHash,
            candidateContentHash,
            promptHash,
            schemaHash,
            integrityPacketHash,
            body);

    private static byte[] BuildBody(
        E0ARoleProfile profile,
        string instructions,
        string data,
        string schemaName,
        string schemaJson)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return string.Equals(profile.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal)
            ? BuildGeminiBody(profile, instructions, data, schemaJson)
            : BuildOpenAIBody(profile, instructions, data, schemaName, schemaJson);
    }

    private static byte[] BuildOpenAIBody(
        E0ARoleProfile profile,
        string instructions,
        string data,
        string schemaName,
        string schemaJson)
    {
        using var schema = JsonDocument.Parse(schemaJson);
        var body = new Dictionary<string, object?>
        {
            ["model"] = profile.Model,
            ["reasoning"] = new Dictionary<string, object?> { ["effort"] = Reasoning(profile.Reasoning) },
            ["stream"] = profile.Stream,
            ["store"] = false,
            ["service_tier"] = profile.ServiceTier,
            ["truncation"] = "disabled",
            ["max_output_tokens"] = profile.MaxOutputTokens,
            ["prompt_cache_options"] = new Dictionary<string, object?> { ["mode"] = E0AProviderTransportPolicy.PromptCacheMode },
            ["instructions"] = instructions,
            ["input"] = data,
            ["text"] = new Dictionary<string, object?>
            {
                ["format"] = new Dictionary<string, object?>
                {
                    ["type"] = "json_schema",
                    ["name"] = schemaName,
                    ["strict"] = true,
                    ["schema"] = schema.RootElement.Clone()
                }
            }
        };
        return JsonSerializer.SerializeToUtf8Bytes(body);
    }

    private static byte[] BuildGeminiBody(
        E0ARoleProfile profile,
        string instructions,
        string data,
        string schemaJson)
    {
        if (!string.Equals(profile.Model, E0AGeminiProviderPolicy.Model, StringComparison.Ordinal) ||
            !string.Equals(profile.ServiceTier, E0AGeminiProviderPolicy.ServiceTier, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A Gemini request profile is outside the approved normative route.");
        }

        using var schema = JsonDocument.Parse(schemaJson);
        var body = new Dictionary<string, object?>
        {
            ["systemInstruction"] = new
            {
                parts = new[] { new { text = instructions } }
            },
            ["contents"] = new[]
            {
                new
                {
                    role = "user",
                    parts = new[] { new { text = data } }
                }
            },
            ["generationConfig"] = new Dictionary<string, object?>
            {
                ["maxOutputTokens"] = profile.MaxOutputTokens,
                ["responseMimeType"] = "application/json",
                ["responseJsonSchema"] = schema.RootElement.Clone(),
                ["thinkingConfig"] = new Dictionary<string, object?>
                {
                    ["thinkingBudget"] = E0AGeminiProviderPolicy.ThinkingBudgetTokens(profile)
                }
            },
            // Standard inference is the provider default. The profile records the
            // resolved semantic tier while omitting an unnecessary provider field.
            ["store"] = false
        };
        return JsonSerializer.SerializeToUtf8Bytes(body);
    }

    private static void RequireContext(ContextPacket context)
    {
        ArgumentNullException.ThrowIfNull(context);
        try
        {
            _ = context.ContextPacketId.Value;
            _ = context.SceneId.Value;
            _ = context.SubjectCharacterId.Value;
            _ = context.OpportunityCharacterId.Value;
            _ = context.SourceStateHash?.Value
                ?? throw new E0AHarnessException("E0-A provider request requires a Production-bound Context.");
        }
        catch (InvalidOperationException)
        {
            throw new E0AHarnessException("E0-A provider request Context identity is uninitialized.");
        }
        if (context.SubjectCharacterId != context.OpportunityCharacterId)
        {
            throw new E0AHarnessException("E0-A provider request Context subject does not hold the current Opportunity.");
        }
    }

    private static string Reasoning(E0AReasoningLevel value) => value switch
    {
        E0AReasoningLevel.None => "none",
        E0AReasoningLevel.Low => "low",
        E0AReasoningLevel.Medium => "medium",
        E0AReasoningLevel.High => "high",
        _ => throw new E0AHarnessException("E0-A reasoning level is invalid.")
    };
}
