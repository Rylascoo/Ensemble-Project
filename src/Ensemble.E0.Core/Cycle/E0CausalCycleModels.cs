using System.Collections.Immutable;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Opportunity;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Cycle;

public sealed class E0OpportunityBearingCycleState
{
    private E0OpportunityBearingCycleState(
        ProductionState productionState,
        E0AcceptedPerformanceHistory acceptedPerformanceHistory,
        E0OpportunityHistory opportunityHistory)
    {
        ProductionState = productionState;
        AcceptedPerformanceHistory = acceptedPerformanceHistory;
        OpportunityHistory = opportunityHistory;
    }

    public ProductionState ProductionState { get; }
    internal E0AcceptedPerformanceHistory AcceptedPerformanceHistory { get; }
    internal E0OpportunityHistory OpportunityHistory { get; }

    internal static E0OpportunityBearingCycleState Create(
        ProductionState productionState,
        E0AcceptedPerformanceHistory acceptedPerformanceHistory,
        E0OpportunityHistory opportunityHistory)
    {
        if (productionState is null || acceptedPerformanceHistory is null || opportunityHistory is null)
        {
            throw new E0CausalCycleInvariantException(
                "Opportunity-bearing cycle components are required.");
        }

        try
        {
            var roster = OpportunityInvariants.ValidateProductionRoster(productionState);
            OpportunityInvariants.RequireInitialized(
                productionState.StateHash,
                "Cycle Production StateHash is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                productionState.SceneId,
                "Cycle Production SceneId is uninitialized.");

            var currentOpportunity = productionState.CurrentOpportunityCharacterId;
            if (!currentOpportunity.HasValue)
            {
                throw new E0CausalCycleInvariantException(
                    "Opportunity-bearing cycle requires a current Opportunity.");
            }

            OpportunityInvariants.RequireInitialized(
                currentOpportunity.Value,
                "Cycle current Opportunity is uninitialized.");
            if (!roster.Contains(currentOpportunity.Value))
            {
                throw new E0CausalCycleInvariantException(
                    "Cycle current Opportunity is outside the Production roster.");
            }

            var acceptedEntries = AcceptedPerformanceHistoryInvariants.ValidateAndProject(
                acceptedPerformanceHistory,
                productionState.SceneId,
                productionState.StateHash,
                roster);

            OpportunityInvariants.RequireInitialized(
                opportunityHistory.SceneId,
                "Cycle Opportunity history SceneId is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                opportunityHistory.LastOpportunityStateHash,
                "Cycle Opportunity history StateHash is uninitialized.");

            if (opportunityHistory.SceneId != productionState.SceneId ||
                opportunityHistory.LastOpportunityStateHash != productionState.StateHash ||
                opportunityHistory.CharacterIds.IsDefault ||
                opportunityHistory.CharacterIds.Length == 0 ||
                opportunityHistory.CharacterIds[^1] != currentOpportunity.Value ||
                opportunityHistory.CharacterIds.Length != acceptedEntries.Length + 1)
            {
                throw new E0CausalCycleInvariantException(
                    "Opportunity-bearing cycle histories are not synchronized.");
            }

            return new E0OpportunityBearingCycleState(
                productionState,
                acceptedPerformanceHistory,
                opportunityHistory);
        }
        catch (E0CausalCycleInvariantException)
        {
            throw;
        }
        catch (E0AcceptedPerformanceHistoryInvariantException exception)
        {
            throw new E0CausalCycleInvariantException(
                "Opportunity-bearing accepted Performance history is invalid.",
                exception);
        }
        catch (E0OpportunityTransitionException exception)
        {
            throw new E0CausalCycleInvariantException(
                "Opportunity-bearing Production or Opportunity history is invalid.",
                exception);
        }
    }
}

public sealed class E0PostCommitCycleState
{
    private E0PostCommitCycleState(
        ProductionState productionState,
        E0AcceptedPerformanceHistory acceptedPerformanceHistory,
        E0CausalCommit commit,
        ContextPacket sourceContext,
        E0OpportunityHistory sourceOpportunityHistory)
    {
        ProductionState = productionState;
        AcceptedPerformanceHistory = acceptedPerformanceHistory;
        Commit = commit;
        SourceContext = sourceContext;
        SourceOpportunityHistory = sourceOpportunityHistory;
    }

    public ProductionState ProductionState { get; }
    public E0CausalCommit Commit { get; }
    internal E0AcceptedPerformanceHistory AcceptedPerformanceHistory { get; }
    internal ContextPacket SourceContext { get; }
    internal E0OpportunityHistory SourceOpportunityHistory { get; }

    internal static E0PostCommitCycleState Create(
        ProductionState productionState,
        E0AcceptedPerformanceHistory acceptedPerformanceHistory,
        E0CausalCommit commit,
        ContextPacket sourceContext,
        E0OpportunityHistory sourceOpportunityHistory)
    {
        if (productionState is null ||
            acceptedPerformanceHistory is null ||
            commit is null ||
            sourceContext is null ||
            sourceOpportunityHistory is null)
        {
            throw new E0CausalCycleInvariantException(
                "Postcommit cycle components are required.");
        }

        try
        {
            var roster = OpportunityInvariants.ValidateProductionRoster(productionState);
            OpportunityInvariants.RequireInitialized(
                productionState.StateHash,
                "Postcommit Production StateHash is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                productionState.SceneId,
                "Postcommit Production SceneId is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                commit.CommitId,
                "Postcommit CommitId is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                commit.ParentStateHash,
                "Postcommit parent StateHash is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                commit.ResultStateHash,
                "Postcommit result StateHash is uninitialized.");

            if (productionState.CurrentOpportunityCharacterId.HasValue ||
                !string.Equals(
                    commit.ContractVersion,
                    E0CausalCommitContracts.ContractVersion,
                    StringComparison.Ordinal) ||
                commit.ResultStateHash != productionState.StateHash ||
                commit.Take is null ||
                !string.Equals(
                    commit.Take.ContractVersion,
                    E0TakeContracts.ContractVersion,
                    StringComparison.Ordinal) ||
                commit.Take.Disposition != E0TakeDisposition.Accepted)
            {
                throw new E0CausalCycleInvariantException(
                    "Postcommit Production and causal commit are inconsistent.");
            }

            OpportunityInvariants.RequireInitialized(
                commit.Take.TakeId,
                "Postcommit TakeId is uninitialized.");
            if (!productionState.ContainsEffectiveCommitId(commit.CommitId) ||
                !productionState.ContainsCommittedTakeId(commit.Take.TakeId))
            {
                throw new E0CausalCycleInvariantException(
                    "Postcommit causal identities are not effective in Production.");
            }

            var performance = commit.Take.Performance;
            if (performance is null)
            {
                throw new E0CausalCycleInvariantException(
                    "Postcommit Accepted Take Performance is missing.");
            }

            OpportunityInvariants.RequireInitialized(
                performance.SubjectCharacterId,
                "Postcommit Performance subject is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                performance.ContextPacketId,
                "Postcommit Performance ContextPacketId is uninitialized.");

            var acceptedEntries = AcceptedPerformanceHistoryInvariants.ValidateAndProject(
                acceptedPerformanceHistory,
                productionState.SceneId,
                productionState.StateHash,
                roster);
            if (acceptedEntries.Length == 0)
            {
                throw new E0CausalCycleInvariantException(
                    "Postcommit accepted Performance history is empty.");
            }

            var lastAccepted = acceptedEntries[^1];
            if (lastAccepted.SourceCharacterId != performance.SubjectCharacterId ||
                !string.Equals(lastAccepted.VisibleText, performance.VisibleText, StringComparison.Ordinal))
            {
                throw new E0CausalCycleInvariantException(
                    "Postcommit accepted Performance history does not end at the committed Performance.");
            }

            OpportunityInvariants.RequireInitialized(
                sourceOpportunityHistory.SceneId,
                "Postcommit source Opportunity history SceneId is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                sourceOpportunityHistory.LastOpportunityStateHash,
                "Postcommit source Opportunity history StateHash is uninitialized.");
            if (sourceOpportunityHistory.SceneId != productionState.SceneId ||
                sourceOpportunityHistory.LastOpportunityStateHash != commit.ParentStateHash ||
                sourceOpportunityHistory.CharacterIds.IsDefault ||
                sourceOpportunityHistory.CharacterIds.Length == 0 ||
                sourceOpportunityHistory.CharacterIds[^1] != performance.SubjectCharacterId ||
                sourceOpportunityHistory.CharacterIds.Length != acceptedEntries.Length)
            {
                throw new E0CausalCycleInvariantException(
                    "Postcommit source Opportunity history is not synchronized.");
            }

            ValidateSourceContext(
                sourceContext,
                productionState,
                commit,
                performance.SubjectCharacterId,
                performance.ContextPacketId,
                roster);

            return new E0PostCommitCycleState(
                productionState,
                acceptedPerformanceHistory,
                commit,
                sourceContext,
                sourceOpportunityHistory);
        }
        catch (E0CausalCycleInvariantException)
        {
            throw;
        }
        catch (E0AcceptedPerformanceHistoryInvariantException exception)
        {
            throw new E0CausalCycleInvariantException(
                "Postcommit accepted Performance history is invalid.",
                exception);
        }
        catch (E0OpportunityTransitionException exception)
        {
            throw new E0CausalCycleInvariantException(
                "Postcommit Production or Opportunity history is invalid.",
                exception);
        }
    }

    private static void ValidateSourceContext(
        ContextPacket sourceContext,
        ProductionState productionState,
        E0CausalCommit commit,
        CharacterId subjectCharacterId,
        ContextPacketId contextPacketId,
        ImmutableArray<CharacterId> productionRoster)
    {
        OpportunityInvariants.RequireInitialized(
            sourceContext.SceneId,
            "Postcommit source Context SceneId is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            sourceContext.ContextPacketId,
            "Postcommit source ContextPacketId is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            sourceContext.SubjectCharacterId,
            "Postcommit source Context subject is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            sourceContext.OpportunityCharacterId,
            "Postcommit source Context Opportunity is uninitialized.");

        if (!sourceContext.SourceStateHash.HasValue)
        {
            throw new E0CausalCycleInvariantException(
                "Postcommit source Context has no parent StateHash.");
        }

        OpportunityInvariants.RequireInitialized(
            sourceContext.SourceStateHash.Value,
            "Postcommit source Context StateHash is uninitialized.");

        if (sourceContext.SourceStateHash.Value != commit.ParentStateHash ||
            sourceContext.SceneId != productionState.SceneId ||
            sourceContext.ContextPacketId != contextPacketId ||
            sourceContext.SubjectCharacterId != subjectCharacterId ||
            sourceContext.OpportunityCharacterId != subjectCharacterId)
        {
            throw new E0CausalCycleInvariantException(
                "Postcommit source Context does not match the causal source.");
        }

        var contextRoster = CanonicalizeContextRoster(sourceContext);
        if (!contextRoster.SequenceEqual(productionRoster))
        {
            throw new E0CausalCycleInvariantException(
                "Postcommit source Context roster does not match Production.");
        }
    }

    private static ImmutableArray<CharacterId> CanonicalizeContextRoster(ContextPacket context)
    {
        if (context.Roster.IsDefault || context.Roster.Length != 3)
        {
            throw new E0CausalCycleInvariantException(
                "Cycle source Context roster is invalid.");
        }

        var values = new HashSet<string>(StringComparer.Ordinal);
        var ids = ImmutableArray.CreateBuilder<CharacterId>(context.Roster.Length);
        foreach (var participant in context.Roster)
        {
            if (participant is null)
            {
                throw new E0CausalCycleInvariantException(
                    "Cycle source Context roster contains an invalid participant.");
            }

            string value;
            try
            {
                value = participant.CharacterId.Value;
            }
            catch (InvalidOperationException exception)
            {
                throw new E0CausalCycleInvariantException(
                    "Cycle source Context roster contains an uninitialized Character.",
                    exception);
            }

            if (!values.Add(value))
            {
                throw new E0CausalCycleInvariantException(
                    "Cycle source Context roster contains duplicate Characters.");
            }

            ids.Add(participant.CharacterId);
        }

        return ids
            .ToImmutable()
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }
}

public sealed class E0OpportunityBearingCycleResult
{
    private E0OpportunityBearingCycleResult(
        E0OpportunityBearingCycleState state,
        E0OpportunityTransition opportunityEvent,
        LeastInterventionDirectorEvaluation directorEvaluation)
    {
        State = state;
        OpportunityEvent = opportunityEvent;
        DirectorEvaluation = directorEvaluation;
    }

    public E0OpportunityBearingCycleState State { get; }
    public E0OpportunityTransition OpportunityEvent { get; }
    public LeastInterventionDirectorEvaluation DirectorEvaluation { get; }

    internal static E0OpportunityBearingCycleResult Create(
        E0OpportunityBearingCycleState state,
        E0OpportunityTransition opportunityEvent,
        LeastInterventionDirectorEvaluation directorEvaluation)
    {
        if (state is null || opportunityEvent is null || directorEvaluation is null)
        {
            throw new E0CausalCycleInvariantException(
                "Opportunity-bearing cycle result components are required.");
        }

        return new E0OpportunityBearingCycleResult(
            state,
            opportunityEvent,
            directorEvaluation);
    }
}

public sealed class E0CausalCycleException : Exception
{
    internal E0CausalCycleException(string message)
        : base(message)
    {
    }
}

internal sealed class E0CausalCycleInvariantException : Exception
{
    internal E0CausalCycleInvariantException(string message)
        : base(message)
    {
    }

    internal E0CausalCycleInvariantException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
