using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0ASpendAndRequestTests
{
    [TestMethod]
    public void SpendReservation_IsWorstCaseAndReconcilesCachedUsageConservatively()
    {
        var ledger = new E0ASpendLedger(E0ATestSupport.Pricing());
        var reservation = ledger.Reserve(100_000, 50_000);
        Assert.AreEqual(0.2m, reservation.ReservedUsd);
        Assert.AreEqual(0.2m, ledger.ReservedUsd);

        var reconciliation = ledger.Reconcile(
            reservation,
            new E0AUsage(100_000, 50_000, 10_000, 0));
        Assert.AreEqual(0.2m, reconciliation.EstimatedUsd);
        Assert.IsFalse(reconciliation.ReservationExceeded);
        Assert.IsFalse(reconciliation.RunCeilingExceeded);
        Assert.AreEqual(0.2m, ledger.EstimatedCommittedUsd);
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void SpendReservation_RefusesBeforeCrossingRunCeiling()
    {
        var ledger = new E0ASpendLedger(new E0APricingAssumptions(20m, 0m, 2m));
        Assert.Throws<E0ABudgetExceededException>(() => ledger.Reserve(250_000, 1));
        Assert.AreEqual(0m, ledger.EstimatedCommittedUsd);
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void PricingAssumptions_RejectZeroPaidTokenRates()
    {
        Assert.Throws<E0AHarnessException>(() =>
            E0ARunEnvelope.CreativeNone(new E0APricingAssumptions(0m, 0m, 2m)));
        Assert.Throws<E0AHarnessException>(() =>
            E0ARunEnvelope.CreativeNone(new E0APricingAssumptions(1m, 0m, 0m)));
        Assert.Throws<E0AHarnessException>(() =>
            E0ARunEnvelope.CreativeNone(new E0APricingAssumptions(1m, 2m, 2m)));
    }

    [TestMethod]
    public void SpendReconcile_RecordsReportedOverrunInsteadOfLosingObservedCost()
    {
        var inputLedger = new E0ASpendLedger(E0ATestSupport.Pricing());
        var inputReservation = inputLedger.Reserve(100, 100);
        var inputOverrun = inputLedger.Reconcile(
            inputReservation,
            new E0AUsage(101, 1, 101, 0));
        Assert.IsTrue(inputOverrun.ReservationExceeded);
        Assert.IsTrue(inputOverrun.EstimatedUsd > 0m);
        Assert.AreEqual(0m, inputLedger.ReservedUsd);
        Assert.AreEqual(inputOverrun.EstimatedUsd, inputLedger.EstimatedCommittedUsd);

        var outputLedger = new E0ASpendLedger(E0ATestSupport.Pricing());
        var outputReservation = outputLedger.Reserve(100, 100);
        var outputOverrun = outputLedger.Reconcile(
            outputReservation,
            new E0AUsage(100, 101, 0, 0));
        Assert.IsTrue(outputOverrun.ReservationExceeded);
        Assert.IsTrue(outputOverrun.EstimatedUsd > outputReservation.ReservedUsd);
        Assert.AreEqual(0m, outputLedger.ReservedUsd);
        Assert.AreEqual(outputOverrun.EstimatedUsd, outputLedger.EstimatedCommittedUsd);
    }

    [TestMethod]
    public void PerformerRequest_SeparatesInstructionsFromDataAndOmitsForbiddenControls()
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-REQUEST"), 1, envelope.Performer, context);

        using var document = JsonDocument.Parse(attempt.RequestBody);
        var root = document.RootElement;
        var systemPart = root.GetProperty("systemInstruction").GetProperty("parts")[0];
        Assert.AreEqual(E0APromptContracts.PerformerInstructions, systemPart.GetProperty("text").GetString());
        var content = root.GetProperty("contents")[0];
        Assert.AreEqual("user", content.GetProperty("role").GetString());
        Assert.AreEqual(JsonValueKind.String, content.GetProperty("parts")[0].GetProperty("text").ValueKind);
        Assert.IsFalse(root.GetProperty("store").GetBoolean());

        var generation = root.GetProperty("generationConfig");
        Assert.AreEqual(1, generation.GetProperty("candidateCount").GetInt32());
        Assert.AreEqual(E0ARunEnvelope.RoleMaxOutputTokens, generation.GetProperty("maxOutputTokens").GetInt32());
        Assert.AreEqual(0, generation.GetProperty("thinkingConfig").GetProperty("thinkingBudget").GetInt32());
        Assert.IsFalse(generation.GetProperty("thinkingConfig").GetProperty("includeThoughts").GetBoolean());
        Assert.AreEqual("application/json", generation.GetProperty("responseMimeType").GetString());
        Assert.AreEqual(JsonValueKind.Object, generation.GetProperty("responseJsonSchema").ValueKind);
        Assert.IsFalse(generation.TryGetProperty("responseFormat", out _));

        Assert.IsFalse(root.TryGetProperty("tools", out _));
        Assert.IsFalse(root.TryGetProperty("conversation", out _));
        Assert.IsFalse(root.TryGetProperty("previous_response_id", out _));
        Assert.IsFalse(generation.TryGetProperty("temperature", out _));
        Assert.IsFalse(generation.TryGetProperty("topP", out _));
    }

    [TestMethod]
    public void IntegrityRequest_BindsCandidateAndAssessmentPacketHashes()
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var cycle = E0ATestSupport.Cycle();
        var continuity = DeterministicE0CausalCycle.ComposeContext(cycle);
        var context = continuity.ContextEvaluation.Packet;
        var candidate = E0ATestSupport.Candidate(context, "No.");
        var input = IntegrityCandidateInput.Bind(context, candidate);
        var packet = E0AIntegrityAssessmentPacketBuilder.Build(
            cycle.ProductionState,
            continuity.AccessEvaluation,
            context,
            candidate);
        var attempt = E0ARequestBuilder.Integrity(
            RunId.From("E0A-INTEGRITY-REQUEST"),
            1,
            envelope.Integrity,
            context,
            input.CandidateContentHash,
            packet);

        Assert.AreEqual(input.CandidateContentHash, attempt.CandidateContentHash);
        Assert.AreEqual(packet.PacketHash, attempt.IntegrityPacketHash);
        Assert.AreEqual(context.ContextPacketId, attempt.ContextPacketId);
        Assert.AreEqual(E0APromptContracts.IntegrityPromptHash, attempt.PromptHash);
    }

    [TestMethod]
    public void InterpreterRequest_RejectsCandidateDifferentFromInterpretationSource()
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var cycle = E0ATestSupport.Cycle();
        var continuity = DeterministicE0CausalCycle.ComposeContext(cycle);
        var context = continuity.ContextEvaluation.Packet;
        var ready = E0ATestSupport.Ready(cycle, "Original candidate.");
        var different = E0ATestSupport.Candidate(context, "Different candidate.");

        Assert.Throws<E0AHarnessException>(() =>
            E0ARequestBuilder.Interpreter(
                RunId.From("E0A-INTERPRETER-MISMATCH"),
                1,
                envelope.Interpreter,
                context,
                different,
                ready.InterpretationSource!));
    }

    [TestMethod]
    public void InterpreterInstructions_ExposeDeterministicMutationShapeLaw()
    {
        var instructions = E0APromptContracts.InterpreterInstructions;

        StringAssert.Contains(instructions, "worldState, sceneState, unresolvedProposition, and pressure require null subjectCharacterId and targetCharacterId");
        StringAssert.Contains(instructions, "Character domains require a roster subjectCharacterId and null targetCharacterId");
        StringAssert.Contains(instructions, "Relationship is the only shape that permits a non-null targetCharacterId");
        StringAssert.Contains(instructions, "characterKnowledge, characterMemory, and characterClaim support add only");
        StringAssert.Contains(instructions, "characterClaim subjectCharacterId must equal the supplied Candidate subject Character");
        StringAssert.Contains(instructions, "For add use null existingRecordId and non-empty text");
        StringAssert.Contains(instructions, "for supersede use non-null existingRecordId and non-empty text");
        StringAssert.Contains(instructions, "for deactivate use non-null existingRecordId and null text");
        StringAssert.Contains(instructions, "supportingRecordIds must contain no duplicates");
        StringAssert.Contains(instructions, "do not emit exact duplicate semantic mutations");
    }

    [TestMethod]
    public void InterpreterSchema_PinsExactDomainAndOperationTokens()
    {
        using var document = JsonDocument.Parse(E0APromptContracts.InterpreterSchemaJson);
        var mutationProperties = document.RootElement
            .GetProperty("properties")
            .GetProperty("mutations")
            .GetProperty("items")
            .GetProperty("properties");
        var domains = mutationProperties.GetProperty("domain").GetProperty("enum")
            .EnumerateArray().Select(x => x.GetString()).ToArray();
        var operations = mutationProperties.GetProperty("operation").GetProperty("enum")
            .EnumerateArray().Select(x => x.GetString()).ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                "worldState", "sceneState", "unresolvedProposition", "characterKnowledge",
                "characterBelief", "characterSuspicion", "characterMemory", "characterGoal",
                "characterDisposition", "characterCircumstance", "characterClaim", "relationship",
                "pressure"
            },
            domains);
        CollectionAssert.AreEqual(new[] { "add", "supersede", "deactivate" }, operations);
    }
}
