using System.Text;
using System.Text.Json;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Evidence;

internal sealed record E0ARuntimeSealVerification(
    string RuntimeRoot,
    byte[] RunFinalBytes,
    string TerminalStatus,
    int AcceptedTurns,
    decimal EstimatedSpendUsd,
    E0ASpendEstimateStatus SpendEstimateStatus,
    bool HasUnknownProviderUsage,
    string FinalStateHash,
    string FinalOpportunityCharacterId);

internal static class E0AEvidenceSealAuthority
{
    private const string RuntimeSummaryContract = "ensemble.e0a.runtime-summary.v1";
    private const string RuntimeSealContract = "ensemble.e0a.runtime-seal.v1";
    private const string EvaluationSealContract = "ensemble.e0a.evaluation-seal.v1";

    internal static (string Path, string Hash)[] RuntimeDigests(string root) =>
        Directory.GetFiles(root, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(root, path).Replace('\\', '/'))
            .Where(path => !string.Equals(path, "run.final.json", StringComparison.Ordinal) &&
                           !string.Equals(path, "evaluation.final.json", StringComparison.Ordinal) &&
                           !path.StartsWith("evaluation/", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .Select(path => (
                Path: path,
                Hash: PreparedRoleAttempt.LowerSha256(
                    File.ReadAllBytes(Path.Combine(root, path.Replace('/', Path.DirectorySeparatorChar))))))
            .ToArray();

    internal static string RootDigest(IEnumerable<(string Path, string Hash)> digests)
    {
        var canonical = string.Concat(digests.Select(x => $"{x.Path}\0{x.Hash}\n"));
        return PreparedRoleAttempt.LowerSha256(Encoding.UTF8.GetBytes(canonical));
    }

    internal static object RuntimeSummary(
        string terminalStatus,
        int acceptedTurns,
        decimal estimatedSpendUsd,
        E0ASpendEstimateStatus spendEstimateStatus,
        bool hasUnknownProviderUsage,
        string finalStateHash,
        string finalOpportunityCharacterId) =>
        new
        {
            contract = RuntimeSummaryContract,
            terminalStatus,
            acceptedTurns,
            estimatedSpendUsd,
            spendEstimateStatus = spendEstimateStatus.ToString(),
            hasUnknownProviderUsage,
            finalStateHash,
            finalOpportunityCharacterId
        };

    internal static E0ARuntimeSealVerification VerifyRuntime(string root)
    {
        if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
        {
            throw new E0AHarnessException("E0-A sealed runtime evidence directory is required.");
        }

        var runFinalPath = Path.Combine(root, "run.final.json");
        var summaryPath = Path.Combine(root, "run.summary.json");
        if (!File.Exists(runFinalPath) || !File.Exists(summaryPath))
        {
            throw new E0AHarnessException("E0-A runtime evidence is not sealed.");
        }

        var summary = ReadRuntimeSummary(File.ReadAllBytes(summaryPath));
        var runFinalBytes = File.ReadAllBytes(runFinalPath);
        var digests = RuntimeDigests(root);
        var recomputedRoot = RootDigest(digests);

        try
        {
            using var finalDocument = JsonDocument.Parse(runFinalBytes);
            var final = finalDocument.RootElement;
            if (final.ValueKind != JsonValueKind.Object ||
                !ReadExactString(final, "contract", out var contract) ||
                !string.Equals(contract, RuntimeSealContract, StringComparison.Ordinal) ||
                !ReadExactString(final, "runtimeRoot", out var recordedRoot) ||
                !IsLowerHex(recordedRoot, 64) ||
                !string.Equals(recordedRoot, recomputedRoot, StringComparison.Ordinal) ||
                !ReadExactString(final, "terminalStatus", out var terminalStatus) ||
                !ReadExactInt32(final, "acceptedTurns", out var acceptedTurns) ||
                !ReadExactDecimal(final, "estimatedSpendUsd", out var estimatedSpendUsd) ||
                !ReadExactString(final, "spendEstimateStatus", out var spendStatusName) ||
                !Enum.TryParse<E0ASpendEstimateStatus>(spendStatusName, ignoreCase: false, out var spendStatus) ||
                !Enum.IsDefined(spendStatus) ||
                !ReadExactBoolean(final, "hasUnknownProviderUsage", out var hasUnknownProviderUsage) ||
                !ReadExactString(final, "finalStateHash", out var finalStateHash) ||
                !ReadExactString(final, "finalOpportunityCharacterId", out var finalOpportunityCharacterId) ||
                !ReadArtifacts(final, digests))
            {
                throw new E0AHarnessException("E0-A runtime seal is invalid.");
            }

            if (!string.Equals(terminalStatus, summary.TerminalStatus, StringComparison.Ordinal) ||
                acceptedTurns != summary.AcceptedTurns ||
                estimatedSpendUsd != summary.EstimatedSpendUsd ||
                spendStatus != summary.SpendEstimateStatus ||
                hasUnknownProviderUsage != summary.HasUnknownProviderUsage ||
                !string.Equals(finalStateHash, summary.FinalStateHash, StringComparison.Ordinal) ||
                !string.Equals(finalOpportunityCharacterId, summary.FinalOpportunityCharacterId, StringComparison.Ordinal))
            {
                throw new E0AHarnessException("E0-A runtime seal summary is not bound to rooted runtime evidence.");
            }

            return new E0ARuntimeSealVerification(
                recordedRoot!,
                runFinalBytes,
                terminalStatus!,
                acceptedTurns,
                estimatedSpendUsd,
                spendStatus,
                hasUnknownProviderUsage,
                finalStateHash!,
                finalOpportunityCharacterId!);
        }
        catch (JsonException)
        {
            throw new E0AHarnessException("E0-A runtime seal is invalid.");
        }
    }

    internal static void SealEvaluation(string root, E0AHardGateEvaluation evaluation)
    {
        ValidateEvaluation(evaluation);

        // First verification performs all external/runtime decoding before any
        // evaluation artifact is created.
        _ = VerifyRuntime(root);

        var evaluationDirectory = Path.Combine(root, "evaluation");
        Directory.CreateDirectory(evaluationDirectory);
        var lockPath = Path.Combine(evaluationDirectory, ".seal.lock");
        FileStream? evaluationLock = null;
        try
        {
            try
            {
                evaluationLock = new FileStream(
                    lockPath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None);
            }
            catch (IOException)
            {
                throw new E0AHarnessException("E0-A evaluation sealing is already in progress.");
            }

            // Re-verify after acquiring publication ownership to close the
            // verification/publication race.
            var runtime = VerifyRuntime(root);
            var hardGatesPath = Path.Combine(evaluationDirectory, "hard-gates.json");
            var evaluationFinalPath = Path.Combine(root, "evaluation.final.json");
            if (File.Exists(evaluationFinalPath))
            {
                throw new E0AHarnessException("E0-A evaluation evidence is write-once.");
            }

            // A hard-gates file without the authoritative evaluation seal is an
            // interrupted prior publication, not an endorsed result. Holding the
            // lock makes cleanup deterministic and retryable.
            if (File.Exists(hardGatesPath))
            {
                File.Delete(hardGatesPath);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var hardGateBytes = JsonSerializer.SerializeToUtf8Bytes(evaluation, options);
            var evaluationFinalBytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                contract = EvaluationSealContract,
                runtimeRoot = runtime.RuntimeRoot,
                runFinalSha256 = PreparedRoleAttempt.LowerSha256(runtime.RunFinalBytes),
                hardGatesSha256 = PreparedRoleAttempt.LowerSha256(hardGateBytes),
                passed = evaluation.Passed
            }, options);

            var hardTemp = Path.Combine(evaluationDirectory, $".hard-gates.{Guid.NewGuid():N}.tmp");
            var finalTemp = Path.Combine(evaluationDirectory, $".evaluation-final.{Guid.NewGuid():N}.tmp");
            try
            {
                WriteExclusive(hardTemp, hardGateBytes);
                WriteExclusive(finalTemp, evaluationFinalBytes);
                File.Move(hardTemp, hardGatesPath, overwrite: false);
                try
                {
                    File.Move(finalTemp, evaluationFinalPath, overwrite: false);
                }
                catch
                {
                    // hard-gates without evaluation.final is explicitly unsealed;
                    // remove our publication so a clean retry remains possible.
                    if (File.Exists(hardGatesPath))
                    {
                        File.Delete(hardGatesPath);
                    }
                    throw;
                }
            }
            catch (IOException)
            {
                throw new E0AHarnessException("E0-A evaluation evidence is write-once.");
            }
            finally
            {
                DeleteIfExists(hardTemp);
                DeleteIfExists(finalTemp);
            }
        }
        finally
        {
            evaluationLock?.Dispose();
        }
    }

    private static E0ARuntimeSealVerification ReadRuntimeSummary(byte[] bytes)
    {
        try
        {
            using var document = JsonDocument.Parse(bytes);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object ||
                !ReadExactString(root, "contract", out var contract) ||
                !string.Equals(contract, RuntimeSummaryContract, StringComparison.Ordinal) ||
                !ReadExactString(root, "terminalStatus", out var terminalStatus) ||
                string.IsNullOrWhiteSpace(terminalStatus) ||
                !ReadExactInt32(root, "acceptedTurns", out var acceptedTurns) ||
                acceptedTurns is < 0 or > E0ARunEnvelope.AcceptedTurnCap ||
                !ReadExactDecimal(root, "estimatedSpendUsd", out var estimatedSpendUsd) ||
                estimatedSpendUsd < 0m ||
                !ReadExactString(root, "spendEstimateStatus", out var spendStatusName) ||
                !Enum.TryParse<E0ASpendEstimateStatus>(spendStatusName, ignoreCase: false, out var spendStatus) ||
                !Enum.IsDefined(spendStatus) ||
                !ReadExactBoolean(root, "hasUnknownProviderUsage", out var hasUnknownProviderUsage) ||
                !ReadExactString(root, "finalStateHash", out var finalStateHash) ||
                !IsLowerHex(finalStateHash, 64) ||
                !ReadExactString(root, "finalOpportunityCharacterId", out var finalOpportunityCharacterId) ||
                string.IsNullOrWhiteSpace(finalOpportunityCharacterId))
            {
                throw new E0AHarnessException("E0-A rooted runtime summary is invalid.");
            }

            return new E0ARuntimeSealVerification(
                string.Empty,
                Array.Empty<byte>(),
                terminalStatus!,
                acceptedTurns,
                estimatedSpendUsd,
                spendStatus,
                hasUnknownProviderUsage,
                finalStateHash!,
                finalOpportunityCharacterId!);
        }
        catch (JsonException)
        {
            throw new E0AHarnessException("E0-A rooted runtime summary is invalid.");
        }
    }

    private static void ValidateEvaluation(E0AHardGateEvaluation evaluation)
    {
        ArgumentNullException.ThrowIfNull(evaluation);
        if (!string.Equals(
                evaluation.ChecklistVersion,
                E0AEvidenceContracts.HardGateChecklistVersion,
                StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(evaluation.ReviewerIdentity) ||
            !IsWellFormedUnicode(evaluation.ReviewerIdentity) ||
            string.IsNullOrWhiteSpace(evaluation.MethodIdentity) ||
            !IsWellFormedUnicode(evaluation.MethodIdentity) ||
            evaluation.Findings is null ||
            evaluation.Findings.Any(finding =>
                string.IsNullOrWhiteSpace(finding) || !IsWellFormedUnicode(finding)) ||
            (evaluation.Passed && evaluation.Findings.Length != 0) ||
            (!evaluation.Passed && evaluation.Findings.Length == 0))
        {
            throw new E0AHarnessException("E0-A hard-gate evaluation identity is invalid.");
        }
    }

    private static bool ReadArtifacts(
        JsonElement final,
        IReadOnlyList<(string Path, string Hash)> expected)
    {
        if (!final.TryGetProperty("artifacts", out var artifacts) ||
            artifacts.ValueKind != JsonValueKind.Array ||
            artifacts.GetArrayLength() != expected.Count)
        {
            return false;
        }

        var index = 0;
        foreach (var item in artifacts.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object ||
                !ReadExactString(item, "path", out var path) ||
                !ReadExactString(item, "sha256", out var hash) ||
                !string.Equals(path, expected[index].Path, StringComparison.Ordinal) ||
                !string.Equals(hash, expected[index].Hash, StringComparison.Ordinal))
            {
                return false;
            }
            index++;
        }
        return true;
    }

    private static bool ReadExactString(JsonElement root, string name, out string? value)
    {
        value = null;
        if (!root.TryGetProperty(name, out var element) || element.ValueKind != JsonValueKind.String)
        {
            return false;
        }
        try
        {
            value = element.GetString();
        }
        catch (JsonException)
        {
            return false;
        }
        return value is not null && IsWellFormedUnicode(value);
    }

    private static bool ReadExactInt32(JsonElement root, string name, out int value)
    {
        value = default;
        return root.TryGetProperty(name, out var element) &&
               element.ValueKind == JsonValueKind.Number &&
               element.TryGetInt32(out value);
    }

    private static bool ReadExactDecimal(JsonElement root, string name, out decimal value)
    {
        value = default;
        return root.TryGetProperty(name, out var element) &&
               element.ValueKind == JsonValueKind.Number &&
               element.TryGetDecimal(out value);
    }

    private static bool ReadExactBoolean(JsonElement root, string name, out bool value)
    {
        value = default;
        if (!root.TryGetProperty(name, out var element) ||
            element.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
        {
            return false;
        }
        value = element.GetBoolean();
        return true;
    }

    private static bool IsWellFormedUnicode(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            if (char.IsHighSurrogate(value[index]))
            {
                if (index + 1 >= value.Length || !char.IsLowSurrogate(value[index + 1]))
                {
                    return false;
                }
                index++;
            }
            else if (char.IsLowSurrogate(value[index]))
            {
                return false;
            }
        }
        return true;
    }

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

    private static void WriteExclusive(string path, byte[] bytes)
    {
        using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
        stream.Write(bytes);
        stream.Flush(flushToDisk: true);
    }

    private static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
