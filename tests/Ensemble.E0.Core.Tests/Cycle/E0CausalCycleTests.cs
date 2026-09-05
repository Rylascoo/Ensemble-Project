using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Ensemble.E0.Core.Tests.Continuity;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Cycle;

[TestClass]
public sealed class E0CausalCycleTests
{
    private const string ExpectedGenesisV2StructuredHash =
        "27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565";
    private const string ExpectedGenesisV2RenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const string ExpectedLivePostCommitStateHash =
        "a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c";
    private const string ExpectedLiveOpportunityStateHash =
        "e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151";
    private const string ExpectedV3StructuredHash =
        "ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f";
    private const string ExpectedV3RenderedHash =
        "668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16";

    [TestMethod]
    public void FirstAcceptedCycle_PreservesExactPatch0015OracleLineage()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var genesis = Patch0012TestSupport.Genesis(fixture);
        var source = DeterministicE0CausalCycle.Initialize(genesis);
        var sourceContext = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;

        Assert.AreSame(genesis, source.ProductionState);
        Assert.AreEqual(MissingRaftContract.VossId, sourceContext.SubjectCharacterId);
        Assert.AreEqual(E0ContextContracts.ProductionBoundSchemaVersion, sourceContext.SchemaVersion);
        Assert.AreEqual(ExpectedGenesisV2StructuredHash, sourceContext.StructuredContextHash);
        Assert.AreEqual(ExpectedGenesisV2RenderedHash, sourceContext.RenderedContextHash);

        var take = Patch0015TestSupport.AcceptedTake(
            genesis,
            sourceContext,
            "No.",
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure },
            "TAKE-PATCH-0012-ORACLE");
        var postCommit = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0012-ORACLE"),
            source,
            sourceContext,
            take,
            Patch0015TestSupport.Materials((0, "PRESSURE-PATCH-0012-ORACLE")));

        Assert.AreEqual(ExpectedLivePostCommitStateHash, postCommit.ProductionState.StateHash.Value);
        Assert.AreEqual(ExpectedLivePostCommitStateHash, postCommit.Commit.ResultStateHash.Value);
        Assert.IsFalse(postCommit.ProductionState.CurrentOpportunityCharacterId.HasValue);
        Assert.AreEqual(MissingRaftContract.VossId, source.ProductionState.CurrentOpportunityCharacterId);

        var established = DeterministicE0CausalCycle.EstablishOpportunity(postCommit);
        Assert.AreEqual(MissingRaftContract.MarloweId, established.OpportunityEvent.SelectedCharacterId);
        Assert.AreEqual(ExpectedLiveOpportunityStateHash, established.OpportunityEvent.ResultStateHash.Value);
        Assert.AreEqual(ExpectedLiveOpportunityStateHash, established.State.ProductionState.StateHash.Value);
        Assert.AreEqual(
            MissingRaftContract.MarloweId,
            established.State.ProductionState.CurrentOpportunityCharacterId);

        var nextContext = DeterministicE0CausalCycle.ComposeContext(established.State).ContextEvaluation.Packet;
        Assert.AreEqual(E0ContextContracts.AcceptedHistorySchemaVersion, nextContext.SchemaVersion);
        Assert.AreEqual(ExpectedV3StructuredHash, nextContext.StructuredContextHash);
        Assert.AreEqual(ExpectedV3RenderedHash, nextContext.RenderedContextHash);
        Assert.AreEqual(1, nextContext.RecentPerformances.Length);
        Assert.AreEqual(MissingRaftContract.VossId, nextContext.RecentPerformances[0].SourceCharacterId);
        Assert.AreEqual("No.", nextContext.RecentPerformances[0].VisibleText);
    }

    [TestMethod]
    public void RejectedAndAlternateTakes_CannotCrossFirstAdoptionBoundary()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var genesis = Patch0012TestSupport.Genesis(fixture);
        var source = DeterministicE0CausalCycle.Initialize(genesis);
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var sourceHash = source.ProductionState.StateHash;

        foreach (var disposition in new[] { E0TakeDisposition.Rejected, E0TakeDisposition.Alternate })
        {
            var take = BuildTake(
                source.ProductionState,
                context,
                disposition,
                $"TAKE-PATCH-0016-{disposition}",
                "This rejected text must never become history.");

            var exception = Assert.Throws<E0CausalCycleException>(() =>
                DeterministicE0CausalCycle.CommitAcceptedTake(
                    CommitId.From($"COMMIT-PATCH-0016-{disposition}"),
                    source,
                    context,
                    take,
                    Patch0015TestSupport.EmptyMaterializations()));

            Assert.AreEqual("E0 causal cycle accepted Take commit failed.", exception.Message);
            Assert.IsNull(exception.InnerException);
            Assert.AreEqual(sourceHash, source.ProductionState.StateHash);
            Assert.AreEqual(MissingRaftContract.VossId, source.ProductionState.CurrentOpportunityCharacterId);
        }
    }

    [TestMethod]
    public void StalePriorContextAndTake_CannotCommitAgainstNextCycleState()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var genesis = Patch0012TestSupport.Genesis(fixture);
        var source = DeterministicE0CausalCycle.Initialize(genesis);
        var context = DeterministicE0CausalCycle.ComposeContext(source).ContextEvaluation.Packet;
        var take = BuildTake(
            source.ProductionState,
            context,
            E0TakeDisposition.Accepted,
            "TAKE-PATCH-0016-FIRST",
            "No.");
        var post = DeterministicE0CausalCycle.CommitAcceptedTake(
            CommitId.From("COMMIT-PATCH-0016-FIRST"),
            source,
            context,
            take,
            Patch0015TestSupport.EmptyMaterializations());
        var next = DeterministicE0CausalCycle.EstablishOpportunity(post).State;
        var nextHash = next.ProductionState.StateHash;

        var exception = Assert.Throws<E0CausalCycleException>(() =>
            DeterministicE0CausalCycle.CommitAcceptedTake(
                CommitId.From("COMMIT-PATCH-0016-STALE"),
                next,
                context,
                take,
                Patch0015TestSupport.EmptyMaterializations()));

        Assert.AreEqual("E0 causal cycle accepted Take commit failed.", exception.Message);
        Assert.AreEqual(nextHash, next.ProductionState.StateHash);
        Assert.AreEqual(MissingRaftContract.MarloweId, next.ProductionState.CurrentOpportunityCharacterId);
    }

    [TestMethod]
    public void NullStageInputs_FailWithExactSanitizedMessages()
    {
        Assert.AreEqual(
            "E0 causal cycle initialization failed.",
            Assert.Throws<E0CausalCycleException>(() =>
                DeterministicE0CausalCycle.Initialize(null!)).Message);
        Assert.AreEqual(
            "E0 causal cycle Context composition failed.",
            Assert.Throws<E0CausalCycleException>(() =>
                DeterministicE0CausalCycle.ComposeContext(null!)).Message);
        Assert.AreEqual(
            "E0 causal cycle Opportunity establishment failed.",
            Assert.Throws<E0CausalCycleException>(() =>
                DeterministicE0CausalCycle.EstablishOpportunity(null!)).Message);
    }

    private static E0Take BuildTake(
        ProductionState sourceState,
        ContextPacket context,
        E0TakeDisposition disposition,
        string takeId,
        string visibleText)
    {
        var candidate = PerformerCandidateContract.ParseJson(
            context,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
                performance = new { text = visibleText },
                control = new
                {
                    addressedCharacterIds = Array.Empty<string>(),
                    nominatedCharacterId = (string?)null
                }
            })));
        var integrityInput = IntegrityCandidateInput.Bind(context, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            integrityInput,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var integrity = DeterministicIntegrityValidator.Validate(integrityInput, evidence);
        var interpretationSource = StateInterpretationSource.Bind(context, candidate, integrity);
        var proposal = StateInterpretationContract.ParseJson(
            interpretationSource,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                schemaVersion = StateInterpretationContract.JsonSchemaVersion,
                mutations = Array.Empty<object>()
            })));
        var authorityInput = StateAuthorityInput.Bind(
            StateAuthoritySnapshot.Bind(sourceState),
            interpretationSource,
            proposal);
        var policy = StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty);
        var reviewSet = StateAuthorityReviewSet.Bind(
            authorityInput,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        var authority = DeterministicStateAuthority.Evaluate(authorityInput, policy, reviewSet);

        return E0Take.Bind(
            TakeId.From(takeId),
            context,
            candidate,
            integrity,
            proposal,
            authority,
            disposition);
    }
}
