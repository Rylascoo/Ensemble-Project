using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Evidence;

internal static class E0AEvidenceContracts
{
    internal const string FrozenBlueprintVersion = "0.1";
    internal const string ReferenceEnvelopeBlueprint = "E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_PROPOSAL_0.15";
    internal const string ApprovedBlueprintCommit = "e1d0b4aea0f7ba29cf85e765bea33d14eab35fde";
    internal const string GeminiReferenceAmendment = "E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT";
    internal const string GeminiApprovedAmendmentCommit = "267575a1c024ea1e43c699994ce68fd6daefd68c";
    internal const string HardGateChecklistVersion = "ensemble.e0a.hard-gates.v1";
    internal const string MandatoryReviewResolution = "deterministic-reject-all";

    internal static ImmutableArray<string> HardGateChecklist { get; } = ImmutableArray.Create(
        "A character receives inaccessible secret information.",
        "A context packet contains prohibited information even if the performer appears not to use it.",
        "A model-created claim silently becomes objective world truth.",
        "A possibility silently becomes fact.",
        "Creator-locked canon or Constitution changes.",
        "A technical provider failure, refusal, timeout, or retry becomes fictional action.",
        "An unaccepted or cancelled partial performance enters Production history.",
        "Accepted history changes retroactively or without an explicit causal event.",
        "A committed consequence lacks a traceable accepted performance or authorized creator/world cause.",
        "Performance commits without its approved consequences, or consequences commit without the accepted Performance.",
        "The State Interpreter directly mutates authority.",
        "A deterministic cost, cancellation, eligibility, or access rule is delegated to an LLM.");

    internal static string ReferenceEnvelopeBlueprintFor(E0ARunEnvelope envelope) =>
        IsGemini(envelope) ? GeminiReferenceAmendment : ReferenceEnvelopeBlueprint;

    internal static string ApprovedBlueprintCommitFor(E0ARunEnvelope envelope) =>
        IsGemini(envelope) ? GeminiApprovedAmendmentCommit : ApprovedBlueprintCommit;

    internal static string ReferenceConfigurationIdentity(E0ARunEnvelope envelope) =>
        IsGemini(envelope)
            ? $"E0A-GEMINI-NORMATIVE-2026-09-06:{envelope.Variant}"
            : $"E0A-P0.15:{envelope.Variant}";

    private static bool IsGemini(E0ARunEnvelope envelope) =>
        string.Equals(
            envelope.Performer.Provider,
            E0AGeminiProviderPolicy.Provider,
            StringComparison.Ordinal);
}

internal sealed record E0AHardGateEvaluation(
    string ChecklistVersion,
    string ReviewerIdentity,
    string MethodIdentity,
    bool Passed,
    string[] Findings);

internal interface IE0AEvidenceSink : IE0AProviderDiagnosticSink
{
    void RecordPrepared(PreparedRoleAttempt attempt);
    void RecordReceipt(PreparedRoleAttempt attempt, RoleAttemptReceipt receipt);
    void RecordEvent(string kind, object data);
    void RecordAcceptedPerformance(int turn, CharacterId characterId, CandidatePerformance candidate);
    void SealRuntime(
        string terminalStatus,
        int acceptedTurns,
        decimal estimatedSpendUsd,
        string finalStateHash,
        CharacterId finalOpportunityCharacterId);
    void SealEvaluation(E0AHardGateEvaluation evaluation);
}

internal sealed class E0AFileEvidenceStore : IE0AEvidenceSink
{
    private readonly string _root;
    private readonly Dictionary<string, string> _blindLabels;
    private readonly List<object> _accepted = new();
    private bool _runtimeSealed;
    private bool _evaluationSealed;

    internal E0AFileEvidenceStore(
        string root,
        RunId runId,
        E0ARunEnvelope envelope,
        string fixtureId,
        string fixtureVersion,
        string fixtureHash,
        string executableCommit,
        IReadOnlyList<CharacterId> roster)
    {
        if (string.IsNullOrWhiteSpace(root))
        {
            throw new E0AHarnessException("E0-A run evidence root is required.");
        }
        if (Directory.Exists(root) || File.Exists(root))
        {
            throw new E0AHarnessException("E0-A run evidence directory already exists.");
        }
        if (string.IsNullOrWhiteSpace(fixtureId) ||
            string.IsNullOrWhiteSpace(fixtureVersion) ||
            !IsLowerHex(fixtureHash, 64) ||
            !IsGitCommitIdentity(executableCommit))
        {
            throw new E0AHarnessException("E0-A run manifest identity is invalid.");
        }

        ArgumentNullException.ThrowIfNull(envelope);
        ArgumentNullException.ThrowIfNull(roster);
        E0ADeterministicIds.ValidateRunId(runId);
        envelope.Validate();
        if (roster.Count != 3)
        {
            throw new E0AHarnessException("E0-A evidence roster must contain exactly three Characters.");
        }

        string[] rosterValues;
        try
        {
            rosterValues = roster.Select(x => x.Value).ToArray();
        }
        catch (InvalidOperationException)
        {
            throw new E0AHarnessException("E0-A evidence roster contains an uninitialized CharacterId.");
        }
        if (rosterValues.Distinct(StringComparer.Ordinal).Count() != rosterValues.Length)
        {
            throw new E0AHarnessException("E0-A evidence roster contains duplicate Characters.");
        }

        _root = root;
        Directory.CreateDirectory(_root);
        _blindLabels = rosterValues
            .OrderBy(x => x, StringComparer.Ordinal)
            .Select((id, index) => new { id, label = $"SPEAKER-{index + 1:D2}" })
            .ToDictionary(x => x.id, x => x.label, StringComparer.Ordinal);

        WriteNew("manifest.json", new
        {
            contract = "ensemble.e0a.run-manifest.v1",
            runId = runId.Value,
            frozenBlueprintVersion = E0AEvidenceContracts.FrozenBlueprintVersion,
            referenceEnvelopeBlueprint = E0AEvidenceContracts.ReferenceEnvelopeBlueprintFor(envelope),
            approvedBlueprintCommit = E0AEvidenceContracts.ApprovedBlueprintCommitFor(envelope),
            referenceConfigurationIdentity = E0AEvidenceContracts.ReferenceConfigurationIdentity(envelope),
            hardGateChecklistVersion = E0AEvidenceContracts.HardGateChecklistVersion,
            hardGateChecklist = E0AEvidenceContracts.HardGateChecklist,
            fixtureId,
            fixtureVersion,
            fixtureHash,
            executableCommit,
            variant = envelope.Variant,
            acceptedTurnCap = E0ARunEnvelope.AcceptedTurnCap,
            attemptsPerRoleInvocation = E0ARunEnvelope.AttemptsPerRoleInvocation,
            automaticRetries = E0ARunEnvelope.AutomaticRetries,
            attemptTimeoutSeconds = E0ARunEnvelope.AttemptTimeoutSeconds,
            estimatedSpendCeilingUsd = E0ARunEnvelope.EstimatedSpendCeilingUsd,
            pricing = PricingManifest(envelope),
            providerTransport = ProviderTransportManifest(envelope),
            stateAuthority = new
            {
                autoApproveDomains = envelope.AutoApproveDomains.Select(x => x.ToString()).ToArray(),
                mandatoryReviewResolution = E0AEvidenceContracts.MandatoryReviewResolution
            },
            host = new
            {
                osDescription = RuntimeInformation.OSDescription,
                processArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
                frameworkDescription = RuntimeInformation.FrameworkDescription
            },
            roles = new[]
            {
                RoleManifest(envelope.Performer),
                RoleManifest(envelope.Integrity),
                RoleManifest(envelope.Interpreter)
            },
            promptHashes = new
            {
                performer = E0APromptContracts.PerformerPromptHash,
                integrity = E0APromptContracts.IntegrityPromptHash,
                interpreter = E0APromptContracts.InterpreterPromptHash
            },
            schemaHashes = new
            {
                performer = E0APromptContracts.PerformerSchemaHash,
                integrity = E0APromptContracts.IntegritySchemaHash,
                interpreter = E0APromptContracts.InterpreterSchemaHash
            }
        });
    }

    internal string RootPath => _root;

    public void RecordStreamEvent(PreparedRoleAttempt attempt, ReadOnlyMemory<byte> utf8Event)
    {
        EnsureRuntimeOpen();
        ArgumentNullException.ThrowIfNull(attempt);
        if (utf8Event.IsEmpty)
        {
            throw new E0AHarnessException("E0-A stream diagnostic event is empty.");
        }
        var path = Path.Combine(_root, "attempts", Safe(attempt.AttemptId), "stream.ndjson");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
        stream.Write(utf8Event.Span);
        stream.WriteByte((byte)'\n');
    }

    public void RecordPrepared(PreparedRoleAttempt attempt)
    {
        EnsureRuntimeOpen();
        ArgumentNullException.ThrowIfNull(attempt);
        var relative = $"attempts/{Safe(attempt.AttemptId)}/request.json";
        WriteNew(relative, new
        {
            attemptId = attempt.AttemptId,
            preparedIdentityHash = attempt.IdentityHash,
            role = attempt.Profile.Role.ToString(),
            turn = attempt.Turn,
            characterId = attempt.CharacterId.HasValue ? attempt.CharacterId.Value.Value : null,
            provider = attempt.Profile.Provider,
            requestedModel = attempt.Profile.Model,
            reasoning = attempt.Profile.Reasoning.ToString(),
            stream = attempt.Profile.Stream,
            maxOutputTokens = attempt.Profile.MaxOutputTokens,
            serviceTier = attempt.Profile.ServiceTier,
            contextPacketId = attempt.ContextPacketId.Value,
            structuredContextHash = attempt.StructuredContextHash,
            renderedContextHash = attempt.RenderedContextHash,
            candidateContentHash = attempt.CandidateContentHash,
            promptHash = attempt.PromptHash,
            responseSchemaHash = attempt.ResponseSchemaHash,
            integrityPacketHash = attempt.IntegrityPacketHash,
            requestBodyHash = attempt.RequestBodyHash,
            requestBodyUtf8 = Encoding.UTF8.GetString(attempt.RequestBody)
        });
    }

    public void RecordReceipt(PreparedRoleAttempt attempt, RoleAttemptReceipt receipt)
    {
        EnsureRuntimeOpen();
        var verified = ConfiguredRoleAttemptBoundary.Accept(attempt, receipt);
        var relative = $"attempts/{Safe(attempt.AttemptId)}/terminal.json";
        WriteNew(relative, new
        {
            attemptId = verified.AttemptId,
            preparedIdentityHash = verified.PreparedIdentityHash,
            outcome = verified.Outcome.ToString(),
            responseId = verified.ResponseId,
            returnedModel = verified.ReturnedModel,
            usage = verified.Usage,
            structuredOutputHash = verified.StructuredOutputHash,
            structuredOutputUtf8 = verified.StructuredOutput is null ? null : Encoding.UTF8.GetString(verified.StructuredOutput),
            diagnosticCode = verified.DiagnosticCode
        });
    }

    public void RecordEvent(string kind, object data)
    {
        EnsureRuntimeOpen();
        if (string.IsNullOrWhiteSpace(kind))
        {
            throw new E0AHarnessException("E0-A evidence event kind is required.");
        }
        ArgumentNullException.ThrowIfNull(data);

        var line = JsonSerializer.Serialize(new { kind, data });
        File.AppendAllText(Path.Combine(_root, "events.ndjson"), line + "\n", new UTF8Encoding(false));
    }

    public void RecordAcceptedPerformance(int turn, CharacterId characterId, CandidatePerformance candidate)
    {
        EnsureRuntimeOpen();
        ArgumentNullException.ThrowIfNull(candidate);
        if (turn is < 1 or > E0ARunEnvelope.AcceptedTurnCap ||
            candidate.SubjectCharacterId != characterId)
        {
            throw new E0AHarnessException("E0-A accepted Performance evidence is not bound to its turn and Character.");
        }

        var id = characterId.Value;
        if (!_blindLabels.ContainsKey(id))
        {
            throw new E0AHarnessException("E0-A accepted Performance Character is outside the evidence roster.");
        }

        _accepted.Add(new { turn, characterId = id, text = candidate.VisibleText });
    }

    public void SealRuntime(
        string terminalStatus,
        int acceptedTurns,
        decimal estimatedSpendUsd,
        string finalStateHash,
        CharacterId finalOpportunityCharacterId)
    {
        EnsureRuntimeOpen();
        string finalOpportunity;
        try
        {
            finalOpportunity = finalOpportunityCharacterId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0AHarnessException("E0-A runtime seal final Opportunity is uninitialized.");
        }
        if (string.IsNullOrWhiteSpace(terminalStatus) ||
            acceptedTurns is < 0 or > E0ARunEnvelope.AcceptedTurnCap ||
            estimatedSpendUsd < 0m ||
            !IsLowerHex(finalStateHash, 64) ||
            !_blindLabels.ContainsKey(finalOpportunity))
        {
            throw new E0AHarnessException("E0-A runtime seal summary is invalid.");
        }

        WriteNew("transcript.json", new { performances = _accepted });

        var blind = _accepted.Select(item =>
        {
            var json = JsonSerializer.SerializeToElement(item);
            var id = json.GetProperty("characterId").GetString()!;
            return new
            {
                turn = json.GetProperty("turn").GetInt32(),
                speaker = _blindLabels[id],
                text = json.GetProperty("text").GetString()
            };
        }).ToArray();
        WriteNew("blind/transcript.json", new { performances = blind });
        WriteNew("blind/mapping.json", new
        {
            mapping = _blindLabels.OrderBy(x => x.Value, StringComparer.Ordinal)
                .Select(x => new { speaker = x.Value, characterId = x.Key }).ToArray()
        });

        var digests = RuntimeDigests();
        var root = RootDigest(digests);
        WriteNew("run.final.json", new
        {
            contract = "ensemble.e0a.runtime-seal.v1",
            terminalStatus,
            acceptedTurns,
            estimatedSpendUsd,
            finalStateHash,
            finalOpportunityCharacterId = finalOpportunity,
            artifacts = digests.Select(x => new { path = x.Path, sha256 = x.Hash }).ToArray(),
            runtimeRoot = root
        });
        _runtimeSealed = true;
    }

    public void SealEvaluation(E0AHardGateEvaluation evaluation)
    {
        if (!_runtimeSealed || _evaluationSealed)
        {
            throw new E0AHarnessException("E0-A evaluation sealing state is invalid.");
        }
        ArgumentNullException.ThrowIfNull(evaluation);
        if (!string.Equals(
                evaluation.ChecklistVersion,
                E0AEvidenceContracts.HardGateChecklistVersion,
                StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(evaluation.ReviewerIdentity) ||
            string.IsNullOrWhiteSpace(evaluation.MethodIdentity) ||
            evaluation.Findings is null)
        {
            throw new E0AHarnessException("E0-A hard-gate evaluation identity is invalid.");
        }
        WriteNew("evaluation/hard-gates.json", evaluation);

        var runFinalBytes = File.ReadAllBytes(Path.Combine(_root, "run.final.json"));
        using var runFinalDocument = JsonDocument.Parse(runFinalBytes);
        var recordedRoot = runFinalDocument.RootElement.GetProperty("runtimeRoot").GetString()
            ?? throw new E0AHarnessException("E0-A runtime seal root is missing.");
        var recomputedRoot = RootDigest(RuntimeDigests());
        if (!string.Equals(recordedRoot, recomputedRoot, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A runtime evidence changed after sealing.");
        }

        var hardGateBytes = File.ReadAllBytes(Path.Combine(_root, "evaluation", "hard-gates.json"));
        WriteNew("evaluation.final.json", new
        {
            contract = "ensemble.e0a.evaluation-seal.v1",
            runtimeRoot = recordedRoot,
            runFinalSha256 = PreparedRoleAttempt.LowerSha256(runFinalBytes),
            hardGatesSha256 = PreparedRoleAttempt.LowerSha256(hardGateBytes),
            passed = evaluation.Passed
        });
        _evaluationSealed = true;
    }

    private static object PricingManifest(E0ARunEnvelope envelope)
    {
        if (string.Equals(envelope.Performer.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal))
        {
            return new
            {
                sourceUri = E0AGeminiPricingPolicy.SourceUri,
                verifiedOn = E0AGeminiPricingPolicy.VerifiedOn,
                snapshotValidThrough = E0AGeminiPricingPolicy.SnapshotValidThrough,
                actualRouteBillingExpectation = "ai-studio-free-tier-expected-zero-pending-pre-run-tier-verification",
                shadowEstimateOnly = true,
                shadowPricingBasis = "gemini-2.5-flash-standard-paid-tier",
                publishedInputUsdPerMillionTokens = E0AGeminiPricingPolicy.PublishedPaidInputUsdPerMillionTokens,
                publishedCachedInputUsdPerMillionTokens = E0AGeminiPricingPolicy.PublishedPaidCachedInputUsdPerMillionTokens,
                publishedOutputUsdPerMillionTokens = E0AGeminiPricingPolicy.PublishedPaidOutputUsdPerMillionTokens,
                modelInputTokenLimit = E0AGeminiProviderPolicy.ModelInputTokenLimit,
                modelOutputTokenLimit = E0AGeminiProviderPolicy.ModelOutputTokenLimit,
                accountingMethod = "all-reported-input-at-full-uncached-shadow-rate; output-includes-thinking",
                inputUsdPerMillionTokens = envelope.Pricing.InputUsdPerMillionTokens,
                cachedInputUsdPerMillionTokens = envelope.Pricing.CachedInputUsdPerMillionTokens,
                outputUsdPerMillionTokens = envelope.Pricing.OutputUsdPerMillionTokens
            };
        }

        return new
        {
            sourceUri = E0APricingPolicy.SourceUri,
            verifiedOn = E0APricingPolicy.VerifiedOn,
            promotionalPricingGuaranteedThrough = E0APricingPolicy.PromotionalPricingGuaranteedThrough,
            publishedInputUsdPerMillionTokens = E0APricingPolicy.PublishedInputUsdPerMillionTokens,
            publishedCachedInputUsdPerMillionTokens = E0APricingPolicy.PublishedCachedInputUsdPerMillionTokens,
            publishedOutputUsdPerMillionTokens = E0APricingPolicy.PublishedOutputUsdPerMillionTokens,
            cacheWriteMultiplier = E0APricingPolicy.CacheWriteMultiplier,
            standardTierMaxInputTokens = E0APricingPolicy.StandardTierMaxInputTokens,
            accountingMethod = "all-reported-input-at-conservative-cache-write-capable-rate",
            inputUsdPerMillionTokens = envelope.Pricing.InputUsdPerMillionTokens,
            cachedInputUsdPerMillionTokens = envelope.Pricing.CachedInputUsdPerMillionTokens,
            outputUsdPerMillionTokens = envelope.Pricing.OutputUsdPerMillionTokens
        };
    }

    private static object ProviderTransportManifest(E0ARunEnvelope envelope)
    {
        if (string.Equals(envelope.Performer.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal))
        {
            return new
            {
                api = "generateContent/streamGenerateContent",
                inputTokenCounter = "models.countTokens(generateContentRequest)",
                requestStore = false,
                serviceTier = E0AGeminiProviderPolicy.ServiceTier,
                serviceTierRequestField = "omitted-provider-default-standard",
                explicitCacheObject = false,
                implicitCachingProviderManaged = true,
                implicitCacheContributionPolicy = "nonzero-cachedContentTokenCount-technical-before-semantic-consumption",
                intendedGeneratedTokenCeiling = E0AGeminiProviderPolicy.IntendedGeneratedTokenCeiling
            };
        }

        return new
        {
            promptCacheMode = E0AProviderTransportPolicy.PromptCacheMode
        };
    }

    private static object RoleManifest(E0ARoleProfile profile)
    {
        if (string.Equals(profile.Provider, E0AGeminiProviderPolicy.Provider, StringComparison.Ordinal))
        {
            return new
            {
                role = profile.Role.ToString(),
                provider = profile.Provider,
                model = profile.Model,
                reasoning = profile.Reasoning.ToString(),
                thinkingBudgetTokens = E0AGeminiProviderPolicy.ThinkingBudgetTokens(profile),
                stream = profile.Stream,
                maxOutputTokens = profile.MaxOutputTokens,
                serviceTier = profile.ServiceTier,
                reservationOutputTokens = E0AProviderBudgetPolicy.ReservationOutputTokens(profile)
            };
        }

        return new
        {
            role = profile.Role.ToString(),
            provider = profile.Provider,
            model = profile.Model,
            reasoning = profile.Reasoning.ToString(),
            stream = profile.Stream,
            maxOutputTokens = profile.MaxOutputTokens,
            serviceTier = profile.ServiceTier
        };
    }

    private (string Path, string Hash)[] RuntimeDigests()
    {
        return Directory.GetFiles(_root, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(_root, path).Replace('\\', '/'))
            .Where(path => !string.Equals(path, "run.final.json", StringComparison.Ordinal) &&
                           !string.Equals(path, "evaluation.final.json", StringComparison.Ordinal) &&
                           !path.StartsWith("evaluation/", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .Select(path => (Path: path, Hash: PreparedRoleAttempt.LowerSha256(File.ReadAllBytes(Path.Combine(_root, path.Replace('/', Path.DirectorySeparatorChar))))))
            .ToArray();
    }

    private static string RootDigest(IEnumerable<(string Path, string Hash)> digests)
    {
        var canonical = string.Concat(digests.Select(x => $"{x.Path}\0{x.Hash}\n"));
        return PreparedRoleAttempt.LowerSha256(Encoding.UTF8.GetBytes(canonical));
    }

    private void WriteNew(string relative, object value)
    {
        var path = Path.Combine(_root, relative.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(path))
        {
            throw new E0AHarnessException("E0-A evidence artifact is write-once.");
        }
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(value, new JsonSerializerOptions { WriteIndented = true }));
    }

    private void EnsureRuntimeOpen()
    {
        if (_runtimeSealed)
        {
            throw new E0AHarnessException("E0-A runtime evidence is already sealed.");
        }
    }

    private static string Safe(string value) => value.Replace(':', '_');

    private static bool IsGitCommitIdentity(string? value) =>
        IsLowerHex(value, 40) || IsLowerHex(value, 64);

    private static bool IsLowerHex(string? value, int length)
    {
        if (value is null || value.Length != length)
        {
            return false;
        }
        foreach (var character in value)
        {
            if (!((character >= '0' && character <= '9') ||
                  (character >= 'a' && character <= 'f')))
            {
                return false;
            }
        }
        return true;
    }
}
