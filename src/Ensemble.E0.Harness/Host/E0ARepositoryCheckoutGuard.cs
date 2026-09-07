using System.Diagnostics;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Host;

internal readonly record struct E0AGitCommandResult(int ExitCode, string Output);

internal interface IE0AGitCommandRunner
{
    E0AGitCommandResult Run(params string[] arguments);
}

internal sealed class E0AProcessGitCommandRunner : IE0AGitCommandRunner
{
    public E0AGitCommandResult Run(params string[] arguments)
    {
        var start = new ProcessStartInfo("git")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (var argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        try
        {
            using var process = Process.Start(start)
                ?? throw new E0AHarnessException("E0-A could not start git checkout verification.");
            var output = process.StandardOutput.ReadToEnd();
            _ = process.StandardError.ReadToEnd();
            process.WaitForExit();
            return new E0AGitCommandResult(process.ExitCode, output.Trim());
        }
        catch (E0AHarnessException)
        {
            throw;
        }
        catch (Exception exception) when (exception is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            throw new E0AHarnessException("E0-A could not execute git checkout verification.");
        }
    }
}

internal static class E0ARepositoryCheckoutGuard
{
    internal static void Validate(string expectedCommit, IE0AGitCommandRunner? runner = null)
    {
        if (!IsLowerHex(expectedCommit, 40))
        {
            throw new E0AHarnessException("E0-A executable commit must be lowercase 40-hex.");
        }

        runner ??= new E0AProcessGitCommandRunner();
        var rootResult = runner.Run("rev-parse", "--show-toplevel");
        if (rootResult.ExitCode != 0 || string.IsNullOrWhiteSpace(rootResult.Output))
        {
            throw new E0AHarnessException("E0-A Git worktree does not match the live-run authority.");
        }
        var repositoryRoot = rootResult.Output;

        RequireExact(
            runner.Run("-C", repositoryRoot, "rev-parse", "HEAD"),
            0,
            expectedCommit,
            "Git HEAD");

        var tracked = runner.Run("-C", repositoryRoot, "diff", "--quiet");
        if (tracked.ExitCode != 0)
        {
            throw new E0AHarnessException("E0-A live run requires a clean tracked working tree.");
        }

        var staged = runner.Run("-C", repositoryRoot, "diff", "--cached", "--quiet");
        if (staged.ExitCode != 0)
        {
            throw new E0AHarnessException("E0-A live run requires a clean staged index.");
        }

        var untracked = runner.Run(
            "-C",
            repositoryRoot,
            "ls-files",
            "--others",
            "--exclude-standard",
            "--full-name");
        if (untracked.ExitCode != 0)
        {
            throw new E0AHarnessException("E0-A could not inspect untracked files.");
        }

        var material = untracked.Output
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(path => path.StartsWith("src/", StringComparison.Ordinal) ||
                           path.StartsWith("tests/", StringComparison.Ordinal) ||
                           path.StartsWith("fixtures/", StringComparison.Ordinal))
            .ToArray();
        if (material.Length != 0)
        {
            throw new E0AHarnessException("E0-A live run has untracked source, test, or fixture files.");
        }
    }

    private static void RequireExact(
        E0AGitCommandResult result,
        int expectedExit,
        string expectedOutput,
        string label)
    {
        if (result.ExitCode != expectedExit ||
            !string.Equals(result.Output, expectedOutput, StringComparison.Ordinal))
        {
            throw new E0AHarnessException($"E0-A {label} does not match the live-run authority.");
        }
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
}
