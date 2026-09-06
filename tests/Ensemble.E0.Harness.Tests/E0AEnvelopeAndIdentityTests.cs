using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0AEnvelopeAndIdentityTests
{
    [TestMethod]
    public void ApprovedVariants_PinRoleReasoningAndSharedModel()
    {
        var variants = new[]
        {
            (Envelope: E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing()), Reasoning: E0AReasoningLevel.None),
            (Envelope: E0ARunEnvelope.CreativeLow(E0ATestSupport.Pricing()), Reasoning: E0AReasoningLevel.Low),
            (Envelope: E0ARunEnvelope.CreativeMedium(E0ATestSupport.Pricing()), Reasoning: E0AReasoningLevel.Medium),
            (Envelope: E0ARunEnvelope.CreativeHigh(E0ATestSupport.Pricing()), Reasoning: E0AReasoningLevel.High)
        };

        foreach (var item in variants)
        {
            item.Envelope.Validate();
            Assert.AreEqual(item.Reasoning, item.Envelope.Performer.Reasoning);
            Assert.AreEqual(E0AReasoningLevel.High, item.Envelope.Integrity.Reasoning);
            Assert.AreEqual(item.Reasoning, item.Envelope.Interpreter.Reasoning);
            Assert.AreEqual("gpt-5.6-sol", item.Envelope.Performer.Model);
            Assert.AreEqual(item.Envelope.Performer.Model, item.Envelope.Integrity.Model);
            Assert.AreEqual(item.Envelope.Performer.Model, item.Envelope.Interpreter.Model);
            Assert.IsTrue(item.Envelope.Performer.Stream);
            Assert.IsFalse(item.Envelope.Integrity.Stream);
            Assert.IsTrue(item.Envelope.Interpreter.Stream);
        }
    }

    [TestMethod]
    public void ApprovedEnvelopeConstants_AreExact()
    {
        Assert.AreEqual(12, E0ARunEnvelope.AcceptedTurnCap);
        Assert.AreEqual(1, E0ARunEnvelope.AttemptsPerRoleInvocation);
        Assert.AreEqual(0, E0ARunEnvelope.AutomaticRetries);
        Assert.AreEqual(300, E0ARunEnvelope.AttemptTimeoutSeconds);
        Assert.AreEqual(5.00m, E0ARunEnvelope.EstimatedSpendCeilingUsd);
        Assert.AreEqual(4096, E0ARunEnvelope.RoleMaxOutputTokens);
    }

    [TestMethod]
    public void DerivedIds_AreDeterministicAndBoundedWithoutArtificialMutationCeiling()
    {
        var runId = RunId.From("E0A-RUN-001");
        Assert.AreEqual("E0A-RUN-001:TAKE:007", E0ADeterministicIds.Take(runId, 7).Value);
        Assert.AreEqual("E0A-RUN-001:COMMIT:007", E0ADeterministicIds.Commit(runId, 7).Value);
        Assert.AreEqual("E0A-RUN-001:RECORD:007:003", E0ADeterministicIds.Record(runId, 7, 3).Value);
        Assert.AreEqual("E0A-RUN-001:RECORD:007:1234", E0ADeterministicIds.Record(runId, 7, 1234).Value);
        Assert.AreEqual("E0A-RUN-001:ATTEMPT:PERFORMER:007:01", E0ADeterministicIds.Attempt(runId, E0ARole.Performer, 7, 1));
        Assert.Throws<E0AHarnessException>(() => E0ADeterministicIds.Attempt(runId, E0ARole.Performer, 7, 2));

        var tooLong = RunId.From(new string('R', 97));
        Assert.Throws<E0AHarnessException>(() => E0ADeterministicIds.ValidateRunId(tooLong));
    }

    [TestMethod]
    public void PreparedAttempt_IdentityBindsRequestAndProfile()
    {
        var runId = RunId.From("E0A-IDENTITY");
        var context = DeterministicContext();
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var first = E0ARequestBuilder.Performer(runId, 1, envelope.Performer, context);
        var second = E0ARequestBuilder.Performer(runId, 2, envelope.Performer, context);

        Assert.AreNotEqual(first.AttemptId, second.AttemptId);
        Assert.AreNotEqual(first.IdentityHash, second.IdentityHash);
        Assert.AreEqual(first.RequestBodyHash, PreparedRoleAttempt.LowerSha256(first.RequestBody));

        using var body = JsonDocument.Parse(first.RequestBody);
        Assert.AreEqual("gpt-5.6-sol", body.RootElement.GetProperty("model").GetString());
    }

    [TestMethod]
    public void ConfiguredReceipt_RejectsCrossAttemptReuseAndTechnicalPayloadIsEmpty()
    {
        var runId = RunId.From("E0A-RECEIPT");
        var context = DeterministicContext();
        var envelope = E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing());
        var first = E0ARequestBuilder.Performer(runId, 1, envelope.Performer, context);
        var second = E0ARequestBuilder.Performer(runId, 2, envelope.Performer, context);
        var success = E0ATestSupport.Success(first, E0ATestSupport.PerformerOutput());

        Assert.AreSame(success, ConfiguredRoleAttemptBoundary.Accept(first, success));
        Assert.Throws<E0AHarnessException>(() => ConfiguredRoleAttemptBoundary.Accept(second, success));

        var technical = RoleAttemptReceipt.TechnicalFailure(first, "synthetic");
        Assert.IsNull(technical.StructuredOutput);
        Assert.IsNull(technical.StructuredOutputHash);
        Assert.IsNull(technical.Usage);
        Assert.IsNull(technical.ResponseId);
        Assert.IsNull(technical.ReturnedModel);
        Assert.AreSame(technical, ConfiguredRoleAttemptBoundary.Accept(first, technical));
    }

    private static Ensemble.E0.Core.Context.ContextPacket DeterministicContext() =>
        Ensemble.E0.Core.Cycle.DeterministicE0CausalCycle.ComposeContext(E0ATestSupport.Cycle()).ContextEvaluation.Packet;
}
