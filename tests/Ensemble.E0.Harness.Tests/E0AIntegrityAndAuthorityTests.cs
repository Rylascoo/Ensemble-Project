using System.Text;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Turn;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0AIntegrityAndAuthorityTests
{
    [TestMethod]
    public void IntegrityPacket_ContainsExactlyActiveDeniedOrProtectedRecords()
    {
        var cycle = E0ATestSupport.Cycle();
        var continuity = DeterministicE0CausalCycle.ComposeContext(cycle);
        var context = continuity.ContextEvaluation.Packet;
        var candidate = E0ATestSupport.Candidate(context, "No.");
        var packet = E0AIntegrityAssessmentPacketBuilder.Build(
            cycle.ProductionState,
            continuity.AccessEvaluation,
            context,
            candidate);

        var decisions = continuity.AccessEvaluation.Decisions.ToDictionary(x => x.RecordId.Value, StringComparer.Ordinal);
        var expected = cycle.ProductionState.Records
            .Where(record => record.Lifecycle == ProductionRecordLifecycle.Active)
            .Where(record =>
                decisions[record.RecordId.Value].Disposition == AccessDisposition.Deny ||
                record.Protection is ProductionRecordProtection.SystemImmutable or ProductionRecordProtection.CreatorLocked)
            .Select(record => record.RecordId.Value)
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();
        var actual = packet.Constraints.Select(x => x.RecordId.Value).ToArray();

        CollectionAssert.AreEqual(expected, actual);
        Assert.AreEqual(actual.Length, actual.Distinct(StringComparer.Ordinal).Count());
        Assert.IsTrue(packet.Constraints.All(x =>
            x.DenialReason.HasValue ||
            x.Protection is ProductionRecordProtection.SystemImmutable or ProductionRecordProtection.CreatorLocked));
    }

    [TestMethod]
    public void IntegrityPacket_RejectsMismatchedAccessSubject()
    {
        var cycle = E0ATestSupport.Cycle();
        var continuity = DeterministicE0CausalCycle.ComposeContext(cycle);
        var context = continuity.ContextEvaluation.Packet;
        var candidate = E0ATestSupport.Candidate(context, "No.");
        var mismatched = CharacterBoundedAccessControl.Evaluate(
            cycle.ProductionState,
            CharacterId.From("MARLOWE"));

        Assert.Throws<E0AHarnessException>(() =>
            E0AIntegrityAssessmentPacketBuilder.Build(
                cycle.ProductionState,
                mismatched,
                context,
                candidate));
    }

    [TestMethod]
    public void ConcernParser_PreservesDuplicatesForCoreRejection()
    {
        var name = nameof(IntegrityConcernKind.PotentialTechnicalArtifactLeak);
        var parsed = E0AIntegrityConcernParser.Parse(E0ATestSupport.IntegrityOutput(name, name));
        Assert.AreEqual(2, parsed.Length);
        Assert.AreEqual(parsed[0], parsed[1]);

        var progress = E0ATestSupport.CandidateReady(E0ATestSupport.Cycle());
        Assert.Throws<E0TurnOrchestrationException>(() =>
            DeterministicE0TurnOrchestrator.EvaluateIntegrity(progress, parsed));
    }

    [TestMethod]
    public void ConcernParser_MalformedUnicodeFailsInsideHarnessDomain()
    {
        var malformed = Encoding.UTF8.GetBytes("{\"concerns\":[\"\\uD800\"]}");

        Assert.Throws<E0AHarnessException>(() => E0AIntegrityConcernParser.Parse(malformed));
    }

    [TestMethod]
    public void ReferenceAuthority_RejectsMandatoryReviewAndReachesTakeBindable()
    {
        var cycle = E0ATestSupport.Cycle();
        var ready = E0ATestSupport.Ready(cycle);
        var proposal = StateInterpretationContract.ParseJson(
            ready.InterpretationSource!,
            E0ATestSupport.ProposalOutput(E0ATestSupport.Mutation("sceneState", "add", text: "A visible state change.")));
        var policy = E0AReferenceAuthority.Policy(E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing()));

        var resolved = E0AReferenceAuthority.EvaluateAndRejectMandatoryReview(ready, proposal, policy);

        Assert.AreEqual(E0TurnProgressDisposition.TakeBindable, resolved.Disposition);
        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, resolved.AuthorityEvaluation!.Status);
        Assert.AreEqual(StateAuthorityDisposition.Rejected, resolved.AuthorityEvaluation.Decisions[0].Disposition);
    }

    [TestMethod]
    public void Materializations_ExistOnlyForApprovedRecordCreatingTransitions()
    {
        var runId = RunId.From("E0A-MATERIALS");

        var pressure = Bindable(E0ATestSupport.ProposalOutput(E0ATestSupport.PressureAdd()));
        var pressureMaterials = E0AReferenceAuthority.Materializations(
            runId, 1, pressure.Proposal, pressure.Progress.AuthorityEvaluation!);
        Assert.AreEqual(1, pressureMaterials.Items.Length);
        Assert.AreEqual("E0A-MATERIALS:RECORD:001:000", pressureMaterials.Items[0].RecordId.Value);

        var supersede = Bindable(E0ATestSupport.ProposalOutput(E0ATestSupport.BeliefSupersede()));
        var supersedeMaterials = E0AReferenceAuthority.Materializations(
            runId, 2, supersede.Proposal, supersede.Progress.AuthorityEvaluation!);
        Assert.AreEqual(1, supersedeMaterials.Items.Length);

        var deactivate = Bindable(E0ATestSupport.ProposalOutput(E0ATestSupport.BeliefDeactivate()));
        var deactivateMaterials = E0AReferenceAuthority.Materializations(
            runId, 3, deactivate.Proposal, deactivate.Progress.AuthorityEvaluation!);
        Assert.AreEqual(0, deactivateMaterials.Items.Length);
    }

    private static (StateInterpretationProposal Proposal, E0TurnProgress Progress) Bindable(byte[] proposalBytes)
    {
        var ready = E0ATestSupport.Ready(E0ATestSupport.Cycle());
        var proposal = StateInterpretationContract.ParseJson(ready.InterpretationSource!, proposalBytes);
        var policy = E0AReferenceAuthority.Policy(E0ARunEnvelope.CreativeNone(E0ATestSupport.Pricing()));
        return (proposal, E0AReferenceAuthority.EvaluateAndRejectMandatoryReview(ready, proposal, policy));
    }
}
