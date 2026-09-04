using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Ensemble.E0.Core.Tests.Opportunity;
using Ensemble.E0.Core.Tests.Patch0012;

namespace Ensemble.E0.Core.Tests.Continuity;

internal static class Patch0014TestSupport
{
    internal static (ValidatedFixture Fixture, ProductionState State, ProductionStateCheckpoint Checkpoint, E0ProductionContextContinuityResult Result)
        Genesis()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var result = E0ProductionContextContinuity.Compose(checkpoint);
        return (fixture, state, checkpoint, result);
    }

    internal static (Patch0013TestSupport.Scenario Scenario, E0OpportunityTransitionResult Opportunity, ProductionStateCheckpoint Checkpoint, E0ProductionContextContinuityResult Result)
        Evolved(string suffix = "CONTINUITY")
    {
        var scenario = Patch0013TestSupport.BuildScenario(suffix);
        var opportunity = Patch0013TestSupport.Establish(scenario);
        var checkpoint = ProductionStateCheckpoint.Capture(opportunity.State);
        var result = E0ProductionContextContinuity.Compose(checkpoint);
        return (scenario, opportunity, checkpoint, result);
    }

    internal static ValidatedFixture LoadGenericSmoke()
    {
        var document = FixtureLoader.Load(
            Encoding.UTF8.GetBytes(Patch0012TestSupport.ReadFixture("e0-fixture-v1.json")));
        return GenericE0FixtureValidator.Validate(document);
    }

    internal static E0Take AcceptedTake(
        ProductionState sourceState,
        ContextPacket sourceContext,
        string suffix = "V2")
    {
        var candidate = PerformerCandidateContract.ParseJson(
            sourceContext,
            Encoding.UTF8.GetBytes(CandidateJson()));
        var integrityInput = IntegrityCandidateInput.Bind(sourceContext, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            integrityInput,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var integrity = DeterministicIntegrityValidator.Validate(integrityInput, evidence);
        var interpretationSource = StateInterpretationSource.Bind(
            sourceContext,
            candidate,
            integrity);
        var proposal = StateInterpretationContract.ParseJson(
            interpretationSource,
            Encoding.UTF8.GetBytes(EmptyProposalJson()));
        var snapshot = StateAuthoritySnapshot.Bind(sourceState);
        var authorityInput = StateAuthorityInput.Bind(
            snapshot,
            interpretationSource,
            proposal);
        var policy = StateAuthorityPolicy.Create(
            ImmutableArray<StateMutationDomain>.Empty);
        var reviewSet = StateAuthorityReviewSet.Bind(
            authorityInput,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        var authority = DeterministicStateAuthority.Evaluate(
            authorityInput,
            policy,
            reviewSet);

        return E0Take.Bind(
            TakeId.From($"TAKE-PATCH-0014-{suffix}"),
            sourceContext,
            candidate,
            integrity,
            proposal,
            authority,
            E0TakeDisposition.Accepted);
    }

    private static string CandidateJson() =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text = "Continue." },
            control = new
            {
                addressedCharacterIds = Array.Empty<string>(),
                nominatedCharacterId = (string?)null
            }
        });

    private static string EmptyProposalJson() =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations = Array.Empty<object>()
        });
}
