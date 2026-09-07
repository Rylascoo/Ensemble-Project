using System.Text;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Evidence;

internal static class E0AEvidenceNamespaceAuthority
{
    private const string ClaimDirectoryName = ".ensemble-e0a-claims";

    internal static string Claim(string root, RunId runId)
    {
        if (string.IsNullOrWhiteSpace(root))
        {
            throw new E0AHarnessException("E0-A run evidence root is required.");
        }

        E0ADeterministicIds.ValidateRunId(runId);
        var fullRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        if (Directory.Exists(fullRoot) || File.Exists(fullRoot))
        {
            throw new E0AHarnessException("E0-A run evidence directory already exists.");
        }

        var parent = Path.GetDirectoryName(fullRoot);
        if (string.IsNullOrWhiteSpace(parent))
        {
            throw new E0AHarnessException("E0-A run evidence namespace is invalid.");
        }

        Directory.CreateDirectory(parent);
        var claimDirectory = Path.Combine(parent, ClaimDirectoryName);
        Directory.CreateDirectory(claimDirectory);

        var normalizedRootIdentity = OperatingSystem.IsWindows()
            ? fullRoot.ToUpperInvariant()
            : fullRoot;
        var runClaimPath = Path.Combine(
            claimDirectory,
            $"run-{PreparedRoleAttempt.LowerSha256(Encoding.UTF8.GetBytes(runId.Value))}.claim");
        var rootClaimPath = Path.Combine(
            claimDirectory,
            $"root-{PreparedRoleAttempt.LowerSha256(Encoding.UTF8.GetBytes(normalizedRootIdentity))}.claim");

        var runClaimCreated = false;
        var rootClaimCreated = false;
        try
        {
            CreateClaim(runClaimPath, runId.Value, fullRoot);
            runClaimCreated = true;
            CreateClaim(rootClaimPath, runId.Value, fullRoot);
            rootClaimCreated = true;

            if (Directory.Exists(fullRoot) || File.Exists(fullRoot))
            {
                throw new E0AHarnessException("E0-A run evidence directory already exists.");
            }

            Directory.CreateDirectory(fullRoot);
            return fullRoot;
        }
        catch
        {
            if (rootClaimCreated)
            {
                DeleteIfExists(rootClaimPath);
            }
            if (runClaimCreated)
            {
                DeleteIfExists(runClaimPath);
            }
            throw;
        }
    }

    private static void CreateClaim(string path, string runId, string fullRoot)
    {
        var bytes = Encoding.UTF8.GetBytes($"runId={runId}\nroot={fullRoot}\n");
        try
        {
            using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            stream.Write(bytes);
            stream.Flush(flushToDisk: true);
        }
        catch (IOException)
        {
            throw new E0AHarnessException("E0-A run or evidence identity is already claimed in this evidence namespace.");
        }
    }

    private static void DeleteIfExists(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (IOException)
        {
            // A failed claim remains fail-closed if cleanup itself cannot complete.
        }
    }
}
