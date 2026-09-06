using System.Text;
using System.Text.Json;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Evidence;

internal static class E0AExistingEvidenceEvaluationSealer
{
    internal static void Seal(string root, E0AHardGateEvaluation evaluation)
    {
        if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
        {
            throw new E0AHarnessException("E0-A sealed runtime evidence directory is required.");
        }
        ArgumentNullException.ThrowIfNull(evaluation);
        if (!string.Equals(
                evaluation.ChecklistVersion,
                E0AEvidenceContracts.HardGateChecklistVersion,
                StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(evaluation.ReviewerIdentity) ||
            string.IsNullOrWhiteSpace(evaluation.MethodIdentity) ||
            evaluation.Findings is null ||
            evaluation.Findings.Any(string.IsNullOrWhiteSpace) ||
            (evaluation.Passed && evaluation.Findings.Length != 0) ||
            (!evaluation.Passed && evaluation.Findings.Length == 0))
        {
            throw new E0AHarnessException("E0-A hard-gate evaluation identity is invalid.");
        }

        var runFinalPath = Path.Combine(root, "run.final.json");
        var hardGatesPath = Path.Combine(root, "evaluation", "hard-gates.json");
        var evaluationFinalPath = Path.Combine(root, "evaluation.final.json");
        if (!File.Exists(runFinalPath))
        {
            throw new E0AHarnessException("E0-A runtime evidence is not sealed.");
        }
        if (File.Exists(hardGatesPath) || File.Exists(evaluationFinalPath))
        {
            throw new E0AHarnessException("E0-A evaluation evidence is write-once.");
        }

        var runFinalBytes = File.ReadAllBytes(runFinalPath);
        var recordedRoot = ReadRuntimeRoot(runFinalBytes);
        var recomputedRoot = RootDigest(RuntimeDigests(root));
        if (!string.Equals(recordedRoot, recomputedRoot, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A runtime evidence changed after sealing.");
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        var hardGateBytes = JsonSerializer.SerializeToUtf8Bytes(evaluation, options);
        var evaluationFinalBytes = JsonSerializer.SerializeToUtf8Bytes(new
        {
            contract = "ensemble.e0a.evaluation-seal.v1",
            runtimeRoot = recordedRoot,
            runFinalSha256 = PreparedRoleAttempt.LowerSha256(runFinalBytes),
            hardGatesSha256 = PreparedRoleAttempt.LowerSha256(hardGateBytes),
            passed = evaluation.Passed
        }, options);

        Directory.CreateDirectory(Path.GetDirectoryName(hardGatesPath)!);
        WriteNew(hardGatesPath, hardGateBytes);
        WriteNew(evaluationFinalPath, evaluationFinalBytes);
    }

    private static string ReadRuntimeRoot(byte[] runFinalBytes)
    {
        try
        {
            using var runFinal = JsonDocument.Parse(runFinalBytes);
            var root = runFinal.RootElement;
            if (root.ValueKind != JsonValueKind.Object ||
                !root.TryGetProperty("contract", out var contract) ||
                !string.Equals(contract.GetString(), "ensemble.e0a.runtime-seal.v1", StringComparison.Ordinal) ||
                !root.TryGetProperty("runtimeRoot", out var runtimeRoot) ||
                runtimeRoot.ValueKind != JsonValueKind.String)
            {
                throw new E0AHarnessException("E0-A runtime seal is invalid.");
            }

            var value = runtimeRoot.GetString();
            if (!IsLowerHex(value, 64))
            {
                throw new E0AHarnessException("E0-A runtime seal root is invalid.");
            }
            return value!;
        }
        catch (JsonException)
        {
            throw new E0AHarnessException("E0-A runtime seal is invalid.");
        }
    }

    private static (string Path, string Hash)[] RuntimeDigests(string root) =>
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

    private static string RootDigest(IEnumerable<(string Path, string Hash)> digests)
    {
        var canonical = string.Concat(digests.Select(x => $"{x.Path}\0{x.Hash}\n"));
        return PreparedRoleAttempt.LowerSha256(Encoding.UTF8.GetBytes(canonical));
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

    private static void WriteNew(string path, byte[] bytes)
    {
        try
        {
            using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            stream.Write(bytes);
        }
        catch (IOException) when (File.Exists(path))
        {
            throw new E0AHarnessException("E0-A evaluation evidence is write-once.");
        }
    }
}
