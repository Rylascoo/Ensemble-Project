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
    public void SpendReservation_IsWorstCaseAndReconcilesCachedUsage()
    {
        var ledger = new E0ASpendLedger(E0ATestSupport.Pricing());
        var reservation = ledger.Reserve(1_000_000, 500_000);
        Assert.AreEqual(2.0m, reservation.ReservedUsd);
        Assert.AreEqual(2.0m, ledger.ReservedUsd);

        var actual = ledger.Reconcile(
            reservation,
            new E0AUsage(1_000_000, 500_000, 100_000, 0));
        Assert.AreEqual(1.925m, actual);
        Assert.AreEqual(1.925m, ledger.EstimatedCommittedUsd);
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void SpendReservation_RefusesBeforeCrossingRunCeiling()
    {
        var ledger = new E0ASpendLedger(E0ATestSupport.Pricing());
        Assert.Throws<E0ABudgetExceededException>(() => ledger.Reserve(5_000_000, E0ARunEnvelope.RoleMaxOutputTokens));
        Assert.AreEqual(0m, ledger.EstimatedCommittedUsd);
        Assert.AreEqual(0m, ledger.ReservedUsd);
    }

    [TestMethod]
    public void PerformerRequest_SeparatesInstructionsFromDataAndOmitsForbiddenControls()
    {
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var context = DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
        var attempt = E0ARequestBuilder.Performer(RunId.From("E0A-REQUEST"), 1, envelope.Performer, context);

        using var document = JsonDocument.Parse(attempt.RequestBody);
        var root = document.RootElement;
        Assert.AreEqual(E0APromptContracts.PerformerInstructions, root.GetProperty("instructions").GetString());
        Assert.AreEqual(JsonValueKind.String, root.GetProperty("input").ValueKind);
        Assert.IsFalse(root.GetProperty("store").GetBoolean());
        Assert.AreEqual("default", root.GetProperty("service_tier").GetString());
        Assert.AreEqual("disabled", root.GetProperty("truncation").GetString());
        Assert.AreEqual(4096, root.GetProperty("max_output_tokens").GetInt32());
        Assert.IsFalse(root.TryGetProperty("tools", out _));
        Assert.IsFalse(root.TryGetProperty("conversation", out _));
        Assert.IsFalse(root.TryGetProperty("previous_response_id", out _));
        Assert.IsFalse(root.TryGetProperty("temperature", out _));
        Assert.IsFalse(root.TryGetProperty("top_p", out _));
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
}
