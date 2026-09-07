using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Turn;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0AFinalRegressionGapTests
{
    [TestMethod]
    public async Task ProviderTechnicalFailureMatrix_AllThreeRolesFailClosedBeforeCommit()
    {
        foreach (var failedRole in new[] { E0ARole.Performer, E0ARole.Integrity, E0ARole.Interpreter })
        {
            var root = E0ATestSupport.TempRunRoot();
            try
            {
                var runId = RunId.From($"E0A-GAP-ROLE-{failedRole.ToString().ToUpperInvariant()}");
                var genesis = E0ATestSupport.Genesis();
                var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
                var provider = new ScriptedProvider((attempt, _) =>
                    attempt.Profile.Role == failedRole
                        ? RoleAttemptReceipt.TechnicalFailure(attempt, "synthetic-role-failure")
                        : E0ATestSupport.Success(
                            attempt,
                            attempt.Profile.Role switch
                            {
                                E0ARole.Performer => E0ATestSupport.PerformerOutput(),
                                E0ARole.Integrity => E0ATestSupport.IntegrityOutput(),
                                E0ARole.Interpreter => E0ATestSupport.EmptyInterpreterOutput(),
                                _ => throw new InvalidOperationException()
                            }));
                var driver = new E0AReferenceRunDriver(
                    envelope,
                    provider,
                    new FixedTokenCounter(),
                    E0ATestSupport.Evidence(root, runId, envelope, genesis));

                var result = await driver.RunAsync(runId, genesis, CancellationToken.None);

                Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status, failedRole.ToString());
                Assert.AreEqual(0, result.AcceptedTurns, failedRole.ToString());
                Assert.AreEqual(genesis.StateHash, result.State.ProductionState.StateHash, failedRole.ToString());
                using var transcript = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "transcript.json")));
                Assert.AreEqual(0, transcript.RootElement.GetProperty("performances").GetArrayLength(), failedRole.ToString());
                var events = File.ReadAllText(Path.Combine(root, "events.ndjson"));
                Assert.IsFalse(events.Contains("turn.committed", StringComparison.Ordinal), failedRole.ToString());
            }
            finally
            {
                Delete(root);
            }
        }
    }

    [TestMethod]
    public async Task ProviderFailureAfterAcceptedTurn_PreservesPriorHistoryAndAddsNoSecondFiction()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-GAP-POST-ACCEPTED-FAILURE");
            var genesis = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var provider = new ScriptedProvider((attempt, call) =>
                call == 4
                    ? RoleAttemptReceipt.TechnicalFailure(attempt, "synthetic-second-turn-failure")
                    : E0ATestSupport.Success(
                        attempt,
                        attempt.Profile.Role switch
                        {
                            E0ARole.Performer => E0ATestSupport.PerformerOutput("Accepted first Turn."),
                            E0ARole.Integrity => E0ATestSupport.IntegrityOutput(),
                            E0ARole.Interpreter => E0ATestSupport.EmptyInterpreterOutput(),
                            _ => throw new InvalidOperationException()
                        }));
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, genesis));

            var result = await driver.RunAsync(runId, genesis, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(1, result.AcceptedTurns);
            Assert.AreEqual(4, provider.Calls);
            Assert.AreNotEqual(genesis.StateHash, result.State.ProductionState.StateHash);
            using var transcript = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "transcript.json")));
            var performances = transcript.RootElement.GetProperty("performances");
            Assert.AreEqual(1, performances.GetArrayLength());
            Assert.AreEqual("Accepted first Turn.", performances[0].GetProperty("text").GetString());
            Assert.AreEqual(
                1,
                File.ReadLines(Path.Combine(root, "events.ndjson"))
                    .Count(line => line.Contains("\"kind\":\"turn.committed\"", StringComparison.Ordinal)));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task MismatchedProviderReceipt_FailsConfiguredBoundaryWithoutSemanticAdoption()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-GAP-MALFORMED-RECEIPT");
            var genesis = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var unrelatedContext = DeterministicE0CausalCycle.ComposeContext(
                DeterministicE0CausalCycle.Initialize(genesis)).ContextEvaluation.Packet;
            var unrelatedAttempt = E0ARequestBuilder.Performer(
                RunId.From("E0A-GAP-MALFORMED-OTHER"),
                1,
                envelope.Performer,
                unrelatedContext);
            var provider = new ScriptedProvider((_, _) =>
                E0ATestSupport.Success(
                    unrelatedAttempt,
                    E0ATestSupport.PerformerOutput("Must not be adopted.")));
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, genesis));

            var result = await driver.RunAsync(runId, genesis, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.IsTrue(result.HasUnknownProviderUsage);
            Assert.AreEqual(0, Directory.GetFiles(root, "terminal.json", SearchOption.AllDirectories).Length);
            Assert.IsFalse(File.ReadAllText(Path.Combine(root, "transcript.json"))
                .Contains("Must not be adopted.", StringComparison.Ordinal));
            Assert.IsTrue(File.ReadAllText(Path.Combine(root, "events.ndjson"))
                .Contains("configured-receipt.rejected", StringComparison.Ordinal));
        }
        finally
        {
            Delete(root);
        }
    }

    [TestMethod]
    public async Task SavedTerminalArtifacts_ReconstructMutatingCausalRunWithControlsAddSupersedeDeactivate()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0A-GAP-SAVED-REPLAY");
            var genesis = E0ATestSupport.Genesis();
            var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
            var roster = genesis.RosterCharacterIds.Select(id => id.Value).ToArray();
            var provider = new ScriptedProvider((attempt, _) =>
            {
                if (attempt.Turn == 4 && attempt.Profile.Role == E0ARole.Performer)
                {
                    return RoleAttemptReceipt.TechnicalFailure(attempt, "stop-after-three-accepted-turns");
                }

                var output = attempt.Profile.Role switch
                {
                    E0ARole.Performer => PerformerOutputWithNonemptyControls(attempt, roster),
                    E0ARole.Integrity => E0ATestSupport.IntegrityOutput(),
                    E0ARole.Interpreter => MutationOutput(runId, attempt.Turn),
                    _ => throw new InvalidOperationException()
                };
                return E0ATestSupport.Success(attempt, output);
            });
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                E0ATestSupport.Evidence(root, runId, envelope, genesis));

            var result = await driver.RunAsync(runId, genesis, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.TechnicalFailure, result.Status);
            Assert.AreEqual(3, result.AcceptedTurns);

            var replay = DeterministicE0CausalCycle.Initialize(E0ATestSupport.Genesis());
            for (var turn = 1; turn <= 3; turn++)
            {
                var continuity = DeterministicE0CausalCycle.ComposeContext(replay);
                var context = continuity.ContextEvaluation.Packet;
                var performerBytes = ReadSavedStructuredOutput(root, runId, E0ARole.Performer, turn);
                var candidate = PerformerCandidateContract.ParseJson(context, performerBytes);
                Assert.IsTrue(candidate.Control.AddressedCharacterIds.Length > 0);
                Assert.IsTrue(candidate.Control.NominatedCharacterId.HasValue);

                var performerResult = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);
                var progress = DeterministicE0TurnOrchestrator.GateAttempt(replay, performerResult);
                var concerns = E0AIntegrityConcernParser.Parse(
                    ReadSavedStructuredOutput(root, runId, E0ARole.Integrity, turn));
                progress = DeterministicE0TurnOrchestrator.EvaluateIntegrity(progress, concerns);
                var proposal = StateInterpretationContract.ParseJson(
                    progress.InterpretationSource!,
                    ReadSavedStructuredOutput(root, runId, E0ARole.Interpreter, turn));
                progress = E0AReferenceAuthority.EvaluateAndRejectMandatoryReview(
                    progress,
                    proposal,
                    E0AReferenceAuthority.Policy(envelope));
                progress = DeterministicE0TurnOrchestrator.BindAcceptedTake(
                    E0ADeterministicIds.Take(runId, turn),
                    progress);
                var materializations = E0AReferenceAuthority.Materializations(
                    runId,
                    turn,
                    proposal,
                    progress.AuthorityEvaluation!);
                var post = DeterministicE0TurnOrchestrator.CommitAccepted(
                    E0ADeterministicIds.Commit(runId, turn),
                    progress,
                    materializations);
                replay = DeterministicE0CausalCycle.EstablishOpportunity(post).State;
            }

            Assert.AreEqual(result.State.ProductionState.StateHash, replay.ProductionState.StateHash);
            Assert.AreEqual(
                result.State.ProductionState.CurrentOpportunityCharacterId,
                replay.ProductionState.CurrentOpportunityCharacterId);
            Assert.AreEqual(
                ProductionRecordLifecycle.Inactive,
                replay.ProductionState.Records.Single(
                    record => record.RecordId == E0ADeterministicIds.Record(runId, 1, 0)).Lifecycle);
            Assert.AreEqual(
                ProductionRecordLifecycle.Inactive,
                replay.ProductionState.Records.Single(
                    record => record.RecordId == E0ADeterministicIds.Record(runId, 2, 0)).Lifecycle);
        }
        finally
        {
            Delete(root);
        }
    }

    private static byte[] PerformerOutputWithNonemptyControls(
        PreparedRoleAttempt attempt,
        IReadOnlyList<string> roster)
    {
        var subject = attempt.CharacterId!.Value.Value;
        var others = roster.Where(id => !string.Equals(id, subject, StringComparison.Ordinal)).ToArray();
        return JsonSerializer.SerializeToUtf8Bytes(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text = $"Saved replay Turn {attempt.Turn}." },
            control = new
            {
                addressedCharacterIds = new[] { others[0] },
                nominatedCharacterId = others[1]
            }
        });
    }

    private static byte[] MutationOutput(RunId runId, int turn) =>
        turn switch
        {
            1 => E0ATestSupport.ProposalOutput(
                E0ATestSupport.Mutation("pressure", "add", text: "Replay pressure one.")),
            2 => E0ATestSupport.ProposalOutput(
                E0ATestSupport.Mutation(
                    "pressure",
                    "supersede",
                    existingRecordId: E0ADeterministicIds.Record(runId, 1, 0).Value,
                    text: "Replay pressure two.")),
            3 => E0ATestSupport.ProposalOutput(
                E0ATestSupport.Mutation(
                    "pressure",
                    "deactivate",
                    existingRecordId: E0ADeterministicIds.Record(runId, 2, 0).Value,
                    text: null)),
            _ => throw new InvalidOperationException()
        };

    private static byte[] ReadSavedStructuredOutput(
        string root,
        RunId runId,
        E0ARole role,
        int turn)
    {
        var attemptId = E0ADeterministicIds.Attempt(
            runId,
            role,
            turn,
            E0ARunEnvelope.AttemptsPerRoleInvocation).Replace(':', '_');
        using var terminal = JsonDocument.Parse(File.ReadAllBytes(
            Path.Combine(root, "attempts", attemptId, "terminal.json")));
        var text = terminal.RootElement.GetProperty("structuredOutputUtf8").GetString()
            ?? throw new InvalidOperationException("Saved terminal output is missing.");
        return Encoding.UTF8.GetBytes(text);
    }

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
