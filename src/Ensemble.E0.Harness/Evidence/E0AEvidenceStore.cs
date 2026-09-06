using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Evidence;

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
    void SealRuntime(string terminalStatus, int acceptedTurns, decimal estimatedSpendUsd);
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
        string fixtureHash,
        string executableCommit,
        IReadOnlyList<CharacterId> roster)
    {
        if (Directory.Exists(root) || File.Exists(root))
        {
            throw new E0AHarnessException("E0-A run evidence directory already exists.");
        }

        ArgumentNullException.ThrowIfNull(envelope);
        ArgumentNullException.ThrowIfNull(roster);
        E0ADeterministicIds.ValidateRunId(runId);
        envelope.Validate();
        _root = root;
        Directory.CreateDirectory(_root);
        _blindLabels = roster
            .OrderBy(x => x.Value, StringComparer.Ordinal)
            .Select((id, index) => new { id = id.Value, label = $"SPEAKER-{index + 1:D2}" })
            .ToDictionary(x => x.id, x => x.label, StringComparer.Ordinal);

        WriteNew("manifest.json", new
        {
            contract = "ensemble.e0a.run-manifest.v1",
            runId = runId.Value,
            fixtureId,
            fixtureHash,
            executableCommit,
            variant = envelope.Variant,
            acceptedTurnCap = E0ARunEnvelope.AcceptedTurnCap,
            automaticRetries = E0ARunEnvelope.AutomaticRetries,
            attemptTimeoutSeconds = E0ARunEnvelope.AttemptTimeoutSeconds,
            estimatedSpendCeilingUsd = E0ARunEnvelope.EstimatedSpendCeilingUsd,
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
        var relative = $"attempts/{Safe(attempt.AttemptId)}/terminal.json";
        WriteNew(relative, new
        {
            attemptId = receipt.AttemptId,
            outcome = receipt.Outcome.ToString(),
            responseId = receipt.ResponseId,
            returnedModel = receipt.ReturnedModel,
            usage = receipt.Usage,
            structuredOutputHash = receipt.StructuredOutputHash,
            structuredOutputUtf8 = receipt.StructuredOutput is null ? null : Encoding.UTF8.GetString(receipt.StructuredOutput),
            diagnosticCode = receipt.DiagnosticCode
        });
    }

    public void RecordEvent(string kind, object data)
    {
        EnsureRuntimeOpen();
        if (string.IsNullOrWhiteSpace(kind))
        {
            throw new E0AHarnessException("E0-A evidence event kind is required.");
        }

        var line = JsonSerializer.Serialize(new { kind, data });
        File.AppendAllText(Path.Combine(_root, "events.ndjson"), line + "\n", new UTF8Encoding(false));
    }

    public void RecordAcceptedPerformance(int turn, CharacterId characterId, CandidatePerformance candidate)
    {
        EnsureRuntimeOpen();
        ArgumentNullException.ThrowIfNull(candidate);
        var id = characterId.Value;
        if (!_blindLabels.ContainsKey(id))
        {
            throw new E0AHarnessException("E0-A accepted Performance Character is outside the evidence roster.");
        }

        _accepted.Add(new { turn, characterId = id, text = candidate.VisibleText });
    }

    public void SealRuntime(string terminalStatus, int acceptedTurns, decimal estimatedSpendUsd)
    {
        EnsureRuntimeOpen();
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
        WriteNew("evaluation/hard-gates.json", evaluation);

        var runFinalBytes = File.ReadAllBytes(Path.Combine(_root, "run.final.json"));
        var runFinal = JsonDocument.Parse(runFinalBytes).RootElement;
        var recordedRoot = runFinal.GetProperty("runtimeRoot").GetString()
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

    private static object RoleManifest(E0ARoleProfile profile) => new
    {
        role = profile.Role.ToString(),
        provider = profile.Provider,
        model = profile.Model,
        reasoning = profile.Reasoning.ToString(),
        stream = profile.Stream,
        maxOutputTokens = profile.MaxOutputTokens,
        serviceTier = profile.ServiceTier
    };

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
}
