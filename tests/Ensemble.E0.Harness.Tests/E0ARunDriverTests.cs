using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0ARunDriverTests
{
    [TestMethod]
    public async Task FullFakeRun_CompletesTwelveAcceptedTurnsAndSynchronizedOpportunity()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-FULL-FAKE");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var evidence = E0ATestSupport.Evidence(root, runId, envelope, state);
            var provider = new ScriptedProvider();
            var counter = new FixedTokenCounter();
            var driver = new E0AReferenceRunDriver(envelope, provider, counter, evidence);

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.AcceptedTurnCapReached, result.Status);
            Assert.AreEqual(12, result.AcceptedTurns);
            Assert.AreEqual(36, provider.Calls);
            Assert.AreEqual(36, counter.Calls);
            Assert.AreEqual("VOSS", result.State.ProductionState.CurrentOpportunityCharacterId!.Value.Value);
            Assert.AreEqual(12, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories)
                .Count(path => path.Contains("ATTEMPT_PERFORMER", StringComparison.Ordinal)));
            for (var index = 0; index < provider.Roles.Count; index += 3)
            {
                Assert.AreEqual(E0ARole.Performer, provider.Roles[index]);
                Assert.AreEqual(E0ARole.Integrity, provider.Roles[index + 1]);
                Assert.AreEqual(E0ARole.Interpreter, provider.Roles[index + 2]);
            }
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task IntegrityConcern_StopsBeforeInterpreterAndCommit()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-INTEGRITY-STOP");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var concern = nameof(IntegrityConcernKind.PotentialTechnicalArtifactLeak);
            var provider = new ScriptedProvider((attempt, _) =>
                E0ATestSupport.Success(
                    attempt,
                    attempt.Profile.Role == E0ARole.Performer
                        ? E0ATestSupport.PerformerOutput()
                        : E0ATestSupport.IntegrityOutput(concern)));
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.IntegrityConcern, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(2, provider.Calls);
            CollectionAssert.AreEqual(new[] { E0ARole.Performer, E0ARole.Integrity }, provider.Roles);
            Assert.AreEqual(state.StateHash, result.State.ProductionState.StateHash);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task DuplicateIntegrityConcerns_TerminateAsInvalidOutputBeforeInterpreterAndSealEvidence()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-INTEGRITY-DUPLICATE");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var concern = nameof(IntegrityConcernKind.PotentialTechnicalArtifactLeak);
            var provider = new ScriptedProvider((attempt, _) =>
                E0ATestSupport.Success(
                    attempt,
                    attempt.Profile.Role == E0ARole.Performer
                        ? E0ATestSupport.PerformerOutput()
                        : E0ATestSupport.IntegrityOutput(concern, concern)));
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.InvalidOutput, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(2, provider.Calls);
            CollectionAssert.AreEqual(new[] { E0ARole.Performer, E0ARole.Integrity }, provider.Roles);
            Assert.AreEqual(state.StateHash, result.State.ProductionState.StateHash);
            Assert.AreEqual(2, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Length);

            var events = File.ReadAllText(Path.Combine(root, "events.ndjson"));
            Assert.IsFalse(events.Contains("interpreter.proposal", StringComparison.Ordinal));
            Assert.IsFalse(events.Contains("turn.committed", StringComparison.Ordinal));
            Assert.IsTrue(events.Contains("run.terminal", StringComparison.Ordinal));

            using var transcript = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "transcript.json")));
            Assert.AreEqual(0, transcript.RootElement.GetProperty("performances").GetArrayLength());

            var finalPath = Path.Combine(root, "run.final.json");
            Assert.IsTrue(File.Exists(finalPath));
            using var final = JsonDocument.Parse(File.ReadAllBytes(finalPath));
            Assert.AreEqual("InvalidOutput", final.RootElement.GetProperty("terminalStatus").GetString());
            var runtimeRoot = final.RootElement.GetProperty("runtimeRoot").GetString();
            Assert.IsNotNull(runtimeRoot);
            Assert.AreEqual(64, runtimeRoot!.Length);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task BudgetRefusal_HappensBeforeProviderAndCreatesNoProviderTerminalReceipt()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-BUDGET-STOP");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider();
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(5_000_000),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.BudgetExceeded, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(0, provider.Calls);
            Assert.AreEqual(0, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Length);
            Assert.AreEqual(1, Directory.GetFiles(root, "request.json", SearchOption.AllDirectories).Length);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task TokenPreflightFailure_HappensBeforeProviderAndCreatesNoProviderTerminalReceipt()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-PREFLIGHT-STOP");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider();
            var counter = new FailingTokenCounter();
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                counter,
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(1, counter.Calls);
            Assert.AreEqual(0, provider.Calls);
            Assert.AreEqual(0, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Length);
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task ProviderTimeout_CommitsUnknownReservationFallbackAndSealsTechnicalFailure()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-PROVIDER-TIMEOUT");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider((_, _) => throw new OperationCanceledException());
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(1, provider.Calls);
            Assert.IsTrue(result.HasUnknownProviderUsage);
            Assert.IsTrue(result.EstimatedSpendUsd > 0m);
            Assert.AreEqual(1, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Length);
            Assert.IsTrue(File.Exists(Path.Combine(root, "run.final.json")));
            Assert.IsTrue(File.ReadAllText(Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Single())
                .Contains("timeout", StringComparison.Ordinal));
            Assert.IsFalse(File.ReadAllText(Path.Combine(root, "events.ndjson"))
                .Contains("spend.released", StringComparison.Ordinal));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task ProviderUsageOverrun_IsMarkedOutsideVerifiedPricingAndNeverConsumedAsFiction()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-USAGE-OVERRUN");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider((attempt, _) =>
                RoleAttemptReceipt.Success(
                    attempt,
                    "resp-usage-overrun",
                    E0AGeminiProviderPolicy.Model,
                    new E0AUsage(E0AGeminiProviderPolicy.ModelInputTokenLimit + 1, 1, 0, 0),
                    E0ATestSupport.PerformerOutput("This must never be consumed as fiction.")));
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(100),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(1, provider.Calls);
            Assert.AreEqual(E0ASpendEstimateStatus.OutsideVerifiedInputTier, result.SpendEstimateStatus);
            Assert.IsTrue(result.EstimatedSpendUsd > 0m);
            Assert.IsTrue(result.EstimatedSpendUsd < E0ARunEnvelope.EstimatedSpendCeilingUsd);
            Assert.AreEqual(1, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Length);
            Assert.IsTrue(File.Exists(Path.Combine(root, "run.final.json")));
            Assert.IsFalse(File.ReadAllText(Path.Combine(root, "transcript.json"))
                .Contains("This must never be consumed as fiction.", StringComparison.Ordinal));

            var events = File.ReadAllText(Path.Combine(root, "events.ndjson"));
            Assert.IsTrue(events.Contains("pricing.assumptions-invalid", StringComparison.Ordinal));
            Assert.IsTrue(events.Contains("OutsideVerifiedInputTier", StringComparison.Ordinal));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task CancellationAfterInterpreterReceipt_StopsBeforeAuthorityAndCommit()
    {
        var root = E0ATestSupport.TempRunRoot();
        using var cancellation = new CancellationTokenSource();
        try
        {
            var runId = RunId.From("E0A-CANCEL-BEFORE-AUTHORITY");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider((attempt, call) =>
            {
                var output = attempt.Profile.Role switch
                {
                    E0ARole.Performer => E0ATestSupport.PerformerOutput(),
                    E0ARole.Integrity => E0ATestSupport.IntegrityOutput(),
                    E0ARole.Interpreter => E0ATestSupport.EmptyInterpreterOutput(),
                    _ => throw new InvalidOperationException()
                };
                var receipt = E0ATestSupport.Success(attempt, output);
                if (call == 3)
                {
                    cancellation.Cancel();
                }
                return receipt;
            });
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, cancellation.Token);

            Assert.AreEqual(E0ARunTerminalStatus.Cancelled, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(3, provider.Calls);
            Assert.AreEqual(state.StateHash, result.State.ProductionState.StateHash);
            var events = File.ReadAllText(Path.Combine(root, "events.ndjson"));
            Assert.IsFalse(events.Contains("authority.evaluated", StringComparison.Ordinal));
            Assert.IsFalse(events.Contains("turn.committed", StringComparison.Ordinal));
            Assert.IsTrue(events.Contains("run.terminal", StringComparison.Ordinal));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task ObservableModelIdentityChange_StopsBeforeSecondReceiptSemanticConsumption()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-MODEL-CHANGE");
            var state = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider((attempt, call) =>
            {
                var output = attempt.Profile.Role == E0ARole.Performer
                    ? E0ATestSupport.PerformerOutput()
                    : E0ATestSupport.IntegrityOutput();
                return E0ATestSupport.Success(
                    attempt,
                    output,
                    call == 1 ? E0AGeminiProviderPolicy.Model : E0AGeminiProviderPolicy.Model + "-changed");
            });
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, state));

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.ModelIdentityChanged, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(2, provider.Calls);
            Assert.AreEqual(state.StateHash, result.State.ProductionState.StateHash);
        }
        finally
        {
            Delete(root);
        }
    }

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
