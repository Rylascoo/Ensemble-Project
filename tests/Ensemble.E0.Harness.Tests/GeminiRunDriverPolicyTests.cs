using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiRunDriverPolicyTests
{
    [TestMethod]
    public async Task ImplicitCacheHit_RecordsSuccessfulReceiptThenStopsBeforeCandidateSemantics()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-GEMINI-CACHE-POLICY");
            var state = E0ATestSupport.Genesis();
            var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
            var provider = new ScriptedProvider((attempt, _) =>
                RoleAttemptReceipt.Success(
                    attempt,
                    "resp-gemini-cache-hit",
                    "gemini-2.5-flash-20260901",
                    new E0AUsage(100, 10, 1, 0, 0),
                    E0ATestSupport.PerformerOutput("This cached candidate must never become fiction.")));
            var evidence = E0ATestSupport.Evidence(root, runId, envelope, state);
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(100),
                evidence);

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(1, provider.Calls);
            Assert.AreEqual(state.StateHash, result.State.ProductionState.StateHash);

            var terminalFiles = Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories);
            Assert.AreEqual(1, terminalFiles.Length);
            using (var terminal = JsonDocument.Parse(File.ReadAllBytes(terminalFiles[0])))
            {
                Assert.AreEqual("Success", terminal.RootElement.GetProperty("outcome").GetString());
                Assert.AreEqual(1L, terminal.RootElement.GetProperty("usage").GetProperty("CachedInputTokens").GetInt64());
            }

            var events = File.ReadAllText(Path.Combine(root, "events.ndjson"));
            Assert.IsTrue(events.Contains("usage.policy-violation", StringComparison.Ordinal));
            Assert.IsTrue(events.Contains("gemini-implicit-cache-hit", StringComparison.Ordinal));
            Assert.IsFalse(events.Contains("performer.candidate", StringComparison.Ordinal));
            Assert.IsFalse(events.Contains("turn.committed", StringComparison.Ordinal));
            Assert.IsFalse(File.ReadAllText(Path.Combine(root, "transcript.json"))
                .Contains("This cached candidate must never become fiction.", StringComparison.Ordinal));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }
}
