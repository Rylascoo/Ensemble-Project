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
using Ensemble.E0.Core.Tests.Patch0012;

namespace Ensemble.E0.Core.Tests.Continuity;

internal static class Patch0015TestSupport
{
    internal static LiveTurn FirstLiveTurn(
        string suffix = "FIRST",
        string visibleText = "No.",
        IReadOnlyList<Dictionary<string, object?>>? mutations = null,
        IReadOnlyList<StateMutationDomain>? autoApproveDomains = null,
        E0RecordMaterializationSet? materializations = null)
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var acceptedHistory = E0AcceptedPerformanceHistoryContinuity.Initialize(state);
        var opportunityHistory = E0OpportunityHistory.Initialize(state);
        return RunTurn(
            state,
            acceptedHistory,
            opportunityHistory,
            suffix,
            visibleText,
            mutations ?? Array.Empty<Dictionary<string, object?>>(),
            autoApproveDomains ?? Array.Empty<StateMutationDomain>(),
            materializations ?? EmptyMaterializations());
    }

    internal static LiveTurn FirstOracleTurn()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var acceptedHistory = E0AcceptedPerformanceHistoryContinuity.Initialize(state);
        var opportunityHistory = E0OpportunityHistory.Initialize(state);
        return RunTurn(
            state,
            acceptedHistory,
            opportunityHistory,
            suffix: "LIVE-ORACLE",
            visibleText: "No.",
            mutations: new[] { Patch0012TestSupport.PressureAdd() },
            autoApproveDomains: new[] { StateMutationDomain.Pressure },
            materializations: Materials((0, "PRESSURE-PATCH-0012-ORACLE")),
            takeId: "TAKE-PATCH-0012-ORACLE",
            commitId: "COMMIT-PATCH-0012-ORACLE");
    }

    internal static LiveTurn RunNextTurn(
        LiveTurn previous,
        string suffix,
        string visibleText,
        IReadOnlyList<Dictionary<string, object?>>? mutations = null,
        IReadOnlyList<StateMutationDomain>? autoApproveDomains = null,
        E0RecordMaterializationSet? materializations = null) =>
        RunTurn(
            previous.Opportunity.State,
            previous.HistoryAfterOpportunity,
            previous.Opportunity.History,
            suffix,
            visibleText,
            mutations ?? Array.Empty<Dictionary<string, object?>>(),
            autoApproveDomains ?? Array.Empty<StateMutationDomain>(),
            materializations ?? EmptyMaterializations());

    private static LiveTurn RunTurn(
        ProductionState state,
        E0AcceptedPerformanceHistory acceptedHistory,
        E0OpportunityHistory opportunityHistory,
        string suffix,
        string visibleText,
        IReadOnlyList<Dictionary<string, object?>> mutations,
        IReadOnlyList<StateMutationDomain> autoApproveDomains,
        E0RecordMaterializationSet materializations,
        string? takeId = null,
        string? commitId = null)
    {
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var contextResult = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
            checkpoint,
            acceptedHistory);
        var context = contextResult.ContextEvaluation.Packet;
        var take = AcceptedTake(
            state,
            context,
            visibleText,
            mutations,
            autoApproveDomains,
            takeId ?? $"TAKE-PATCH-0015-{suffix}");
        var binding = E0TakeStateBinding.BindWithAcceptedHistory(
            checkpoint,
            context,
            take,
            acceptedHistory);
        var commitResult = DeterministicCausalCommit.Commit(
            CommitId.From(commitId ?? $"COMMIT-PATCH-0015-{suffix}"),
            state,
            binding,
            materializations);
        var afterCommit = E0AcceptedPerformanceHistoryContinuity.RecordCommit(
            acceptedHistory,
            state,
            commitResult.Commit);
        var opportunity = DeterministicOpportunityAuthority.Establish(
            commitResult.ResultState,
            commitResult.Commit,
            context,
            opportunityHistory);
        var afterOpportunity = E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
            afterCommit,
            commitResult.ResultState,
            commitResult.Commit,
            opportunityHistory,
            opportunity.Event);

        return new LiveTurn(
            state,
            acceptedHistory,
            opportunityHistory,
            checkpoint,
            contextResult,
            take,
            binding,
            commitResult,
            afterCommit,
            opportunity,
            afterOpportunity);
    }

    internal static E0Take AcceptedTake(
        ProductionState sourceState,
        ContextPacket sourceContext,
        string visibleText,
        IReadOnlyList<Dictionary<string, object?>> mutations,
        IReadOnlyList<StateMutationDomain> autoApproveDomains,
        string takeId)
    {
        var candidate = PerformerCandidateContract.ParseJson(
            sourceContext,
            Encoding.UTF8.GetBytes(CandidateJson(visibleText)));
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
            Encoding.UTF8.GetBytes(ProposalJson(mutations)));
        var snapshot = StateAuthoritySnapshot.Bind(sourceState);
        var authorityInput = StateAuthorityInput.Bind(snapshot, interpretationSource, proposal);
        var policy = StateAuthorityPolicy.Create(autoApproveDomains.ToImmutableArray());
        var reviewSet = StateAuthorityReviewSet.Bind(
            authorityInput,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        var authority = DeterministicStateAuthority.Evaluate(
            authorityInput,
            policy,
            reviewSet);

        return E0Take.Bind(
            TakeId.From(takeId),
            sourceContext,
            candidate,
            integrity,
            proposal,
            authority,
            E0TakeDisposition.Accepted);
    }

    internal static E0RecordMaterializationSet EmptyMaterializations() =>
        E0RecordMaterializationSet.Bind(ImmutableArray<E0RecordMaterialization>.Empty);

    internal static E0RecordMaterializationSet Materials(
        params (int Index, string RecordId)[] values) =>
        E0RecordMaterializationSet.Bind(
            values.Select(value =>
                E0RecordMaterialization.Create(
                    value.Index,
                    RecordId.From(value.RecordId))).ToImmutableArray());

    private static string CandidateJson(string text) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text },
            control = new
            {
                addressedCharacterIds = Array.Empty<string>(),
                nominatedCharacterId = (string?)null
            }
        });

    private static string ProposalJson(
        IReadOnlyList<Dictionary<string, object?>> mutations) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations
        });

    internal sealed record LiveTurn(
        ProductionState SourceState,
        E0AcceptedPerformanceHistory SourceAcceptedHistory,
        E0OpportunityHistory SourceOpportunityHistory,
        ProductionStateCheckpoint Checkpoint,
        E0ProductionContextContinuityResult Context,
        E0Take Take,
        E0TakeStateBinding Binding,
        E0CausalCommitResult Commit,
        E0AcceptedPerformanceHistory HistoryAfterCommit,
        E0OpportunityTransitionResult Opportunity,
        E0AcceptedPerformanceHistory HistoryAfterOpportunity);
}
