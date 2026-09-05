using System.Reflection;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Tests.Continuity;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.PerformerAttempt;

[TestClass]
public sealed class E0PerformerAttemptTests
{
    [TestMethod]
    public void ExactContextAndCandidate_BindCandidateReadyWithoutRewritingCandidate()
    {
        var source = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var candidate = Candidate(context, "No.");

        var result = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);

        Assert.AreEqual(E0PerformerAttemptDisposition.CandidateReady, result.Disposition);
        Assert.AreEqual(context.ContextPacketId, result.SourceContextPacketId);
        Assert.AreSame(candidate, result.Candidate);
        Assert.AreEqual("No.", result.Candidate!.VisibleText);
    }

    [TestMethod]
    public void StaleCandidate_CannotBindToNextCycleContext()
    {
        var source = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var staleCandidate = Candidate(context, "No.");
        var acceptedTake = Patch0015TestSupport.AcceptedTake(
            source.ProductionState,
            context,
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            "TAKE-PATCH-0017-STALE");
        var post = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0017-STALE"),
            source,
            context,
            acceptedTake,
            Patch0015TestSupport.EmptyMaterializations());
        var next = DeterministicE0CausalCycle.EstablishOpportunity(post).State;
        var nextContext = DeterministicE0CausalCycle.ComposeContext(next).ContextEvaluation.Packet;

        var exception = Assert.Throws<E0PerformerAttemptException>(() =>
            DeterministicE0PerformerAttemptBoundary.BindCandidate(nextContext, staleCandidate));

        Assert.AreEqual("E0 Performer attempt candidate binding failed.", exception.Message);
        Assert.IsNull(exception.InnerException);
        Assert.AreNotEqual(context.ContextPacketId, nextContext.ContextPacketId);
    }

    [TestMethod]
    public void WrongSubjectCandidate_FailsClosedWithoutLeakingCandidateText()
    {
        var source = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var valid = Candidate(context, "PRIVATE-CANDIDATE-TEXT");
        var constructor = typeof(CandidatePerformance)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        var forged = (CandidatePerformance)constructor.Invoke(new object[]
        {
            valid.ContractVersion,
            CharacterId.From("MARLOWE"),
            valid.ContextPacketId,
            valid.VisibleText,
            valid.Control
        });

        var exception = Assert.Throws<E0PerformerAttemptException>(() =>
            DeterministicE0PerformerAttemptBoundary.BindCandidate(context, forged));

        Assert.AreEqual("E0 Performer attempt candidate binding failed.", exception.Message);
        Assert.IsFalse(exception.ToString().Contains("PRIVATE-CANDIDATE-TEXT", StringComparison.Ordinal));
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void TechnicalFailureAndCancellation_HaveNoCandidateAndDoNotMutateSource()
    {
        var source = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var sourceHash = source.ProductionState.StateHash;
        var contextId = context.ContextPacketId;

        foreach (var disposition in new[]
                 {
                     E0PerformerAttemptDisposition.TechnicalFailure,
                     E0PerformerAttemptDisposition.Cancelled
                 })
        {
            var result = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
                context,
                disposition);

            Assert.AreEqual(disposition, result.Disposition);
            Assert.AreEqual(contextId, result.SourceContextPacketId);
            Assert.IsNull(result.Candidate);
            Assert.AreEqual(sourceHash, source.ProductionState.StateHash);
            Assert.AreEqual(contextId, context.ContextPacketId);
        }
    }

    [TestMethod]
    public void CandidateReadyDefaultAndUndefined_CannotEnterTechnicalBinding()
    {
        var source = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;

        foreach (var disposition in new[]
                 {
                     E0PerformerAttemptDisposition.CandidateReady,
                     default,
                     (E0PerformerAttemptDisposition)99
                 })
        {
            var exception = Assert.Throws<E0PerformerAttemptException>(() =>
                DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(context, disposition));

            Assert.AreEqual("E0 Performer attempt technical outcome binding failed.", exception.Message);
            Assert.IsNull(exception.InnerException);
        }
    }

    [TestMethod]
    public void InvalidCandidateTransport_CanMapOnlyToPayloadFreeTechnicalFailure()
    {
        var source = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        const string rawInvalid = "{\"secret\":\"DO-NOT-LEAK\"}";

        Assert.Throws<PerformerCandidateException>(() =>
            PerformerCandidateContract.ParseJson(context, Encoding.UTF8.GetBytes(rawInvalid)));

        var result = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
            context,
            E0PerformerAttemptDisposition.TechnicalFailure);

        Assert.AreEqual(E0PerformerAttemptDisposition.TechnicalFailure, result.Disposition);
        Assert.AreEqual(context.ContextPacketId, result.SourceContextPacketId);
        Assert.IsNull(result.Candidate);
        Assert.IsFalse(result.ToString()!.Contains("DO-NOT-LEAK", StringComparison.Ordinal));
    }

    [TestMethod]
    public void NullInputs_FailWithExactSanitizedMessages()
    {
        var source = DeterministicE0CausalCycle.Initialize(Patch0012TestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var candidate = Candidate(context, "No.");

        Assert.AreEqual(
            "E0 Performer attempt candidate binding failed.",
            Assert.Throws<E0PerformerAttemptException>(() =>
                DeterministicE0PerformerAttemptBoundary.BindCandidate(null!, candidate)).Message);
        Assert.AreEqual(
            "E0 Performer attempt candidate binding failed.",
            Assert.Throws<E0PerformerAttemptException>(() =>
                DeterministicE0PerformerAttemptBoundary.BindCandidate(context, null!)).Message);
        Assert.AreEqual(
            "E0 Performer attempt technical outcome binding failed.",
            Assert.Throws<E0PerformerAttemptException>(() =>
                DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(
                    null!,
                    E0PerformerAttemptDisposition.TechnicalFailure)).Message);
    }

    private static CandidatePerformance Candidate(ContextPacket context, string text) =>
        PerformerCandidateContract.ParseJson(
            context,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
                performance = new { text },
                control = new
                {
                    addressedCharacterIds = Array.Empty<string>(),
                    nominatedCharacterId = (string?)null
                }
            })));
}
