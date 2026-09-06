using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Turn;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiThinkingDisclosureTests
{
    [TestMethod]
    public void AllNormativeRolesExplicitlyDisableThoughtDisclosure()
    {
        var envelope = E0AReferenceRunHost.CreateEnvelope("CREATIVE-NONE");
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
        var ready = DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            DeterministicE0TurnOrchestrator.GateAttempt(
                cycle,
                Ensemble.E0.Core.PerformerAttempt.DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate)),
            System.Collections.Immutable.ImmutableArray<Ensemble.E0.Core.Integrity.IntegrityConcernKind>.Empty);

        var attempts = new[]
        {
            E0ARequestBuilder.Performer(RunId.From("E0A-GEMINI-NO-THOUGHTS-P"), 1, envelope.Performer, context),
            E0ARequestBuilder.Integrity(
                RunId.From("E0A-GEMINI-NO-THOUGHTS-G"),
                1,
                envelope.Integrity,
                context,
                input.CandidateContentHash,
                packet),
            E0ARequestBuilder.Interpreter(
                RunId.From("E0A-GEMINI-NO-THOUGHTS-I"),
                1,
                envelope.Interpreter,
                context,
                candidate,
                ready.InterpretationSource!)
        };

        foreach (var attempt in attempts)
        {
            using var body = JsonDocument.Parse(attempt.RequestBody);
            var thinking = body.RootElement
                .GetProperty("generationConfig")
                .GetProperty("thinkingConfig");
            Assert.IsFalse(thinking.GetProperty("includeThoughts").GetBoolean());
        }
    }
}
