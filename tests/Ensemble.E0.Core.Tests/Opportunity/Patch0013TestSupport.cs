using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Ensemble.E0.Core.Tests.Patch0012;

namespace Ensemble.E0.Core.Tests.Opportunity;

internal static class Patch0013TestSupport
{
    internal static Scenario BuildScenario(
        string suffix,
        string[]? addressedCharacterIds = null,
        string? nominatedCharacterId = null,
        string candidateText = "No.")
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var context = Patch0012TestSupport.Compose(fixture, MissingRaftContract.VossId);
        var candidate = PerformerCandidateContract.ParseJson(
            context,
            Encoding.UTF8.GetBytes(CandidateJson(
                candidateText,
                addressedCharacterIds ?? Array.Empty<string>(),
                nominatedCharacterId)));

        var integrityInput = IntegrityCandidateInput.Bind(context, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            integrityInput,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var integrity = DeterministicIntegrityValidator.Validate(integrityInput, evidence);
        var source = StateInterpretationSource.Bind(context, candidate, integrity);
        var proposal = StateInterpretationContract.ParseJson(
            source,
            Encoding.UTF8.GetBytes(EmptyProposalJson()));
        var snapshot = StateAuthoritySnapshot.Bind(
            fixture,
            ImmutableArray<RecordId>.Empty);
        var authorityInput = StateAuthorityInput.Bind(snapshot, source, proposal);
        var policy = StateAuthorityPolicy.Create(ImmutableArray<StateMutationDomain>.Empty);
        var reviewSet = StateAuthorityReviewSet.Bind(
            authorityInput,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        var authority = DeterministicStateAuthority.Evaluate(
            authorityInput,
            policy,
            reviewSet);
        var take = E0Take.Bind(
            TakeId.From($"TAKE-PATCH-0013-{suffix}"),
            context,
            candidate,
            integrity,
            proposal,
            authority,
            E0TakeDisposition.Accepted);

        var genesis = ProductionState.Initialize(
            fixture,
            ImmutableArray<RecordId>.Empty);
        var history = E0OpportunityHistory.Initialize(genesis);
        var checkpoint = ProductionStateCheckpoint.Capture(genesis);
        var binding = E0TakeStateBinding.Bind(checkpoint, context, take);
        var commitResult = DeterministicCausalCommit.Commit(
            CommitId.From($"COMMIT-PATCH-0013-{suffix}"),
            genesis,
            binding,
            E0RecordMaterializationSet.Bind(
                ImmutableArray<E0RecordMaterialization>.Empty));

        return new Scenario(
            fixture,
            context,
            candidate,
            take,
            genesis,
            history,
            commitResult.Commit,
            commitResult.ResultState);
    }

    internal static E0OpportunityTransitionResult Establish(Scenario scenario) =>
        DeterministicOpportunityAuthority.Establish(
            scenario.PostCommitState,
            scenario.SourceCommit,
            scenario.Context,
            scenario.SourceHistory);

    private static string CandidateJson(
        string text,
        string[] addressedCharacterIds,
        string? nominatedCharacterId) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text },
            control = new
            {
                addressedCharacterIds,
                nominatedCharacterId
            }
        });

    private static string EmptyProposalJson() =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations = Array.Empty<object>()
        });

    internal sealed record Scenario(
        ValidatedFixture Fixture,
        ContextPacket Context,
        CandidatePerformance Candidate,
        E0Take Take,
        ProductionState GenesisState,
        E0OpportunityHistory SourceHistory,
        E0CausalCommit SourceCommit,
        ProductionState PostCommitState);
}
