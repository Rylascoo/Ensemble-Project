using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using Ensemble.E0.Core.Experiments.E0D;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0DExperimentHarnessTests
{
    [TestMethod]
    public void OmniscientIntegrity_RemovesOnlyDeniedConstraintsThatAreActuallyDisclosed()
    {
        var cycle = E0ATestSupport.Cycle();
        var continuity = E0DExperimentalCycle.ComposeContext(cycle, E0DExperimentVariant.OmniscientContext);
        var context = continuity.ContextEvaluation.Packet;
        var candidate = E0ATestSupport.Candidate(context, "Still here.");
        var packet = E0DIntegrityAssessmentPacketBuilder.Build(
            cycle.ProductionState,
            continuity.AccessEvaluation,
            context,
            candidate,
            E0DExperimentVariant.OmniscientContext);
        var constrained = packet.Constraints.Select(x => x.RecordId.Value).ToHashSet(StringComparer.Ordinal);

        Assert.IsTrue(ContextRecordIds(context).Contains(MissingRaftContract.KnowMarloweReleasedRaftId));
        Assert.IsFalse(constrained.Contains(MissingRaftContract.KnowMarloweReleasedRaftId));
        Assert.IsFalse(ContextRecordIds(context).Contains(MissingRaftContract.HtMarloweReleasedRaftId));
        Assert.IsTrue(constrained.Contains(MissingRaftContract.HtMarloweReleasedRaftId));
    }

    [TestMethod]
    public void OmniscientHardGateProfile_ReplacesOnlyFirstTwoReferenceGates()
    {
        var checklist = E0DHardGatePolicy.Checklist(E0DExperimentVariant.OmniscientContext);

        Assert.AreEqual(E0AEvidenceContracts.HardGateChecklist.Length + 2, checklist.Length);
        Assert.IsFalse(checklist.Contains(E0AEvidenceContracts.HardGateChecklist[0]));
        Assert.IsFalse(checklist.Contains(E0AEvidenceContracts.HardGateChecklist[1]));
        CollectionAssert.AreEqual(E0AEvidenceContracts.HardGateChecklist.Skip(2).ToArray(), checklist.Skip(4).ToArray());
        Assert.AreEqual(
            E0DExperimentEvidenceContracts.OmniscientHardGateChecklistVersion,
            E0DHardGatePolicy.ChecklistVersion(E0DExperimentVariant.OmniscientContext));
    }

    [TestMethod]
    public void E0DManifest_IsExplicitlyVariantBoundAndKeepsRun08ReferenceEnvelope()
    {
        var root = E0ATestSupport.TempRunRoot();
        var runId = Ensemble.E0.Core.Domain.RunId.From($"E0D-TEST-{Guid.NewGuid():N}");
        var state = E0ATestSupport.Genesis();
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        _ = new E0AFileEvidenceStore(
            root,
            runId,
            envelope,
            state.OriginFixtureId.Value,
            state.OriginFixtureVersion.Value,
            state.OriginFixtureHash,
            E0ATestSupport.TestExecutableCommit,
            state.RosterCharacterIds,
            e0dVariant: E0DExperimentVariant.RelationshipsOmitted);

        using var manifest = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "manifest.json")));
        var value = manifest.RootElement;
        Assert.AreEqual("ensemble.e0d.run-manifest.v1", value.GetProperty("contract").GetString());
        Assert.AreEqual("E0-D", value.GetProperty("experimentProgram").GetString());
        Assert.AreEqual(E0DExperimentContracts.RelationshipsOmittedVariantId, value.GetProperty("experimentVariantId").GetString());
        Assert.AreEqual(E0DExperimentContracts.RelationshipsOmittedVariantId, value.GetProperty("conditionId").GetString());
        Assert.AreEqual(E0DExperimentEvidenceContracts.MethodIdentity, value.GetProperty("referenceEnvelopeBlueprint").GetString());
        Assert.AreEqual(E0DExperimentEvidenceContracts.MethodAuthorityCommit, value.GetProperty("approvedBlueprintCommit").GetString());
        Assert.AreEqual("CREATIVE-MINIMAL", value.GetProperty("variant").GetString());
        Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35MinimalId, value.GetProperty("providerProfileId").GetString());
    }

    [TestMethod]
    public async Task ContextAblationTerminalOutcomes_SealRuntimeWithoutNetwork()
    {
        foreach (var variant in new[]
                 {
                     E0DExperimentVariant.RelationshipsOmitted,
                     E0DExperimentVariant.OmniscientContext
                 })
        {
            foreach (var cancelled in new[] { false, true })
            {
                var root = E0ATestSupport.TempRunRoot();
                var variantId = variant == E0DExperimentVariant.RelationshipsOmitted ? "REL" : "OMNI";
                var outcomeId = cancelled ? "C" : "T";
                var runId = Ensemble.E0.Core.Domain.RunId.From(
                    $"E0D-TERM-{variantId}-{outcomeId}-{Guid.NewGuid():N}");
                var state = E0ATestSupport.Genesis();
                var envelope = E0AReferenceRunHost.CreateEnvelope(
                    "CREATIVE-MINIMAL",
                    E0AGeminiModelCatalog.FlashLite35MinimalId);
                var evidence = new E0AFileEvidenceStore(
                    root, runId, envelope, state.OriginFixtureId.Value,
                    state.OriginFixtureVersion.Value, state.OriginFixtureHash,
                    E0ATestSupport.TestExecutableCommit, state.RosterCharacterIds,
                    e0dVariant: variant);
                var provider = new ScriptedProvider((attempt, _) =>
                    cancelled
                        ? RoleAttemptReceipt.Cancelled(attempt, "synthetic-cancelled")
                        : RoleAttemptReceipt.TechnicalFailure(attempt, "synthetic-technical-failure"));
                var driver = new E0AReferenceRunDriver(
                    envelope, provider, new FixedTokenCounter(), evidence,
                    e0dVariant: variant);

                var result = await driver.RunAsync(runId, state, CancellationToken.None);

                var expectedStatus = cancelled
                    ? E0ARunTerminalStatus.Cancelled
                    : E0ARunTerminalStatus.TechnicalFailure;
                Assert.AreEqual(expectedStatus, result.Status, $"{variant}/{cancelled}");
                Assert.AreEqual(0, result.AcceptedTurns, $"{variant}/{cancelled}");
                Assert.AreEqual(1, provider.Calls, $"{variant}/{cancelled}");
                Assert.IsTrue(File.Exists(Path.Combine(root, "run.summary.json")), $"{variant}/{cancelled}");
                Assert.IsTrue(File.Exists(Path.Combine(root, "run.final.json")), $"{variant}/{cancelled}");
                Assert.AreEqual(
                    1,
                    File.ReadLines(Path.Combine(root, "events.ndjson"))
                        .Count(line => line.Contains("\"kind\":\"run.terminal\"", StringComparison.Ordinal)),
                    $"{variant}/{cancelled}");
                using var final = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "run.final.json")));
                Assert.AreEqual(expectedStatus.ToString(), final.RootElement.GetProperty("terminalStatus").GetString());
                Assert.AreEqual(0, final.RootElement.GetProperty("acceptedTurns").GetInt32());
            }
        }
    }

    [TestMethod]
    public async Task RoundRobinDriver_ProducesFrozenTwelveTurnSubjectScheduleWithoutNetwork()
    {
        var root = E0ATestSupport.TempRunRoot();
        var runId = Ensemble.E0.Core.Domain.RunId.From($"E0D-RR-{Guid.NewGuid():N}");
        var state = E0ATestSupport.Genesis();
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        var evidence = new E0AFileEvidenceStore(
            root,
            runId,
            envelope,
            state.OriginFixtureId.Value,
            state.OriginFixtureVersion.Value,
            state.OriginFixtureHash,
            E0ATestSupport.TestExecutableCommit,
            state.RosterCharacterIds,
            e0dVariant: E0DExperimentVariant.RoundRobin);
        var provider = new ScriptedProvider();
        var counter = new FixedTokenCounter();
        var driver = new E0AReferenceRunDriver(
            envelope,
            provider,
            counter,
            evidence,
            e0dVariant: E0DExperimentVariant.RoundRobin);

        var result = await driver.RunAsync(runId, state, CancellationToken.None);

        Assert.AreEqual(E0ARunTerminalStatus.AcceptedTurnCapReached, result.Status);
        Assert.AreEqual(12, result.AcceptedTurns);
        Assert.AreEqual(36, provider.Calls);
        var contexts = File.ReadLines(Path.Combine(root, "events.ndjson"))
            .Select(line => JsonDocument.Parse(line))
            .Where(document => document.RootElement.GetProperty("kind").GetString() == "context.composed")
            .Select(document => document.RootElement.GetProperty("data").GetProperty("subjectCharacterId").GetString())
            .ToArray();
        CollectionAssert.AreEqual(new[]
        {
            "VOSS", "WREN", "MARLOWE", "VOSS", "WREN", "MARLOWE",
            "VOSS", "WREN", "MARLOWE", "VOSS", "WREN", "MARLOWE"
        }, contexts);
        foreach (var document in File.ReadLines(Path.Combine(root, "events.ndjson"))
                     .Select(line => JsonDocument.Parse(line))
                     .Where(document => document.RootElement.GetProperty("kind").GetString() == "e0d.opportunity.established"))
        {
            Assert.AreEqual(
                E0DExperimentContracts.RoundRobinStrategyContract,
                document.RootElement.GetProperty("data").GetProperty("strategyContract").GetString());
        }
    }

    [TestMethod]
    public async Task OmniscientEvaluationSeal_AcceptsFrozenVariantChecklistVersion()
    {
        var root = E0ATestSupport.TempRunRoot();
        var runId = Ensemble.E0.Core.Domain.RunId.From($"E0D-OMNI-{Guid.NewGuid():N}");
        var state = E0ATestSupport.Genesis();
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        var evidence = new E0AFileEvidenceStore(
            root, runId, envelope, state.OriginFixtureId.Value,
            state.OriginFixtureVersion.Value, state.OriginFixtureHash,
            E0ATestSupport.TestExecutableCommit, state.RosterCharacterIds,
            e0dVariant: E0DExperimentVariant.OmniscientContext);
        var driver = new E0AReferenceRunDriver(
            envelope, new ScriptedProvider(), new FixedTokenCounter(), evidence,
            e0dVariant: E0DExperimentVariant.OmniscientContext);
        var result = await driver.RunAsync(runId, state, CancellationToken.None);
        Assert.AreEqual(E0ARunTerminalStatus.AcceptedTurnCapReached, result.Status);

        var exit = E0DExperimentRunHost.SealEvaluation(new[]
        {
            E0DExperimentContracts.OmniscientContextVariantId,
            root,
            "TEST-REVIEWER",
            E0DExperimentEvidenceContracts.MethodIdentity,
            "pass"
        });
        Assert.AreEqual(0, exit);
        Assert.IsTrue(File.Exists(Path.Combine(root, "evaluation.final.json")));
    }

    [TestMethod]
    public async Task RelationshipsOmittedDriver_ReachesFrozenTwelveTurnCapWithoutNetwork()
    {
        var result = await RunVariantAsync(E0DExperimentVariant.RelationshipsOmitted, "REL");
        Assert.AreEqual(E0ARunTerminalStatus.AcceptedTurnCapReached, result.Status);
        Assert.AreEqual(12, result.AcceptedTurns);
    }

    [TestMethod]
    public async Task FullReferenceDriver_UsesReferenceOpportunityStrategyWithoutNetwork()
    {
        var (result, root) = await RunVariantWithRootAsync(E0DExperimentVariant.FullReference, "FULL");
        Assert.AreEqual(E0ARunTerminalStatus.AcceptedTurnCapReached, result.Status);
        Assert.AreEqual(12, result.AcceptedTurns);
        foreach (var document in File.ReadLines(Path.Combine(root, "events.ndjson"))
                     .Select(line => JsonDocument.Parse(line))
                     .Where(document => document.RootElement.GetProperty("kind").GetString() == "e0d.opportunity.established"))
        {
            Assert.AreEqual(
                Ensemble.E0.Core.Director.E0DirectorContracts.LeastInterventionStrategyContract,
                document.RootElement.GetProperty("data").GetProperty("strategyContract").GetString());
        }
    }

    private static async Task<E0ARunResult> RunVariantAsync(E0DExperimentVariant variant, string suffix)
    {
        var (result, _) = await RunVariantWithRootAsync(variant, suffix);
        return result;
    }

    private static async Task<(E0ARunResult Result, string Root)> RunVariantWithRootAsync(
        E0DExperimentVariant variant,
        string suffix)
    {
        var root = E0ATestSupport.TempRunRoot();
        var runId = Ensemble.E0.Core.Domain.RunId.From($"E0D-{suffix}-{Guid.NewGuid():N}");
        var state = E0ATestSupport.Genesis();
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);
        var evidence = new E0AFileEvidenceStore(
            root, runId, envelope, state.OriginFixtureId.Value,
            state.OriginFixtureVersion.Value, state.OriginFixtureHash,
            E0ATestSupport.TestExecutableCommit, state.RosterCharacterIds,
            e0dVariant: variant);
        var driver = new E0AReferenceRunDriver(
            envelope, new ScriptedProvider(), new FixedTokenCounter(), evidence,
            e0dVariant: variant);
        return (await driver.RunAsync(runId, state, CancellationToken.None), root);
    }

    private static HashSet<string> ContextRecordIds(Ensemble.E0.Core.Context.ContextPacket context) =>
        context.SceneState.Select(x => x.RecordId.Value)
            .Concat(context.Pressures.Select(x => x.RecordId.Value))
            .Concat(context.Constitution.Select(x => x.RecordId.Value))
            .Concat(context.Disposition.Select(x => x.RecordId.Value))
            .Concat(context.Circumstance.Select(x => x.RecordId.Value))
            .Concat(context.Observations.Select(x => x.RecordId.Value))
            .Concat(context.Knowledge.Select(x => x.RecordId.Value))
            .Concat(context.Beliefs.Select(x => x.RecordId.Value))
            .Concat(context.Suspicions.Select(x => x.RecordId.Value))
            .Concat(context.Memories.Select(x => x.RecordId.Value))
            .Concat(context.Goals.Select(x => x.RecordId.Value))
            .Concat(context.Relationships.Select(x => x.RecordId.Value))
            .ToHashSet(StringComparer.Ordinal);
}