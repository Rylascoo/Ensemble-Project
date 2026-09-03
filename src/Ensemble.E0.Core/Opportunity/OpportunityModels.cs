using System.Collections.Immutable;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Opportunity;

public static class E0OpportunityTransitionContracts
{
    public const string ContractVersion = "ensemble.e0.opportunity-transition.v1";
}

public sealed class E0OpportunityHistory
{
    private E0OpportunityHistory(
        SceneId sceneId,
        StateHash lastOpportunityStateHash,
        ImmutableArray<CharacterId> characterIds)
    {
        SceneId = sceneId;
        LastOpportunityStateHash = lastOpportunityStateHash;
        CharacterIds = characterIds;
    }

    public SceneId SceneId { get; }
    public StateHash LastOpportunityStateHash { get; }
    public ImmutableArray<CharacterId> CharacterIds { get; }

    public static E0OpportunityHistory Initialize(ProductionState genesisState)
    {
        if (genesisState is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity history genesis Production state is required.");
        }

        try
        {
            _ = genesisState.StateHash.Value;
            _ = genesisState.SceneId.Value;
            var roster = OpportunityInvariants.ValidateProductionRoster(genesisState);
            var currentOpportunity = genesisState.CurrentOpportunityCharacterId;
            if (!currentOpportunity.HasValue)
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity history genesis state must have a current opportunity.");
            }

            OpportunityInvariants.RequireInitialized(
                currentOpportunity.Value,
                "Opportunity history genesis current opportunity is uninitialized.");
            if (!roster.Contains(currentOpportunity.Value))
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity history genesis current opportunity is outside the Scene roster.");
            }

            var recomputedGenesisHash =
                ProductionStateCanonicalizer.ComputeGenesisHash(genesisState.Projection);
            if (recomputedGenesisHash != genesisState.StateHash)
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity history may initialize only from an exact genesis Production state.");
            }

            return new E0OpportunityHistory(
                genesisState.SceneId,
                genesisState.StateHash,
                ImmutableArray.Create(currentOpportunity.Value));
        }
        catch (CanonicalJsonException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity history genesis canonicalization failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity history genesis identity is uninitialized.",
                exception);
        }
    }

    internal E0OpportunityHistory Advance(
        ProductionState resultState,
        CharacterId selectedCharacterId)
    {
        if (resultState is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity history result Production state is required.");
        }

        OpportunityInvariants.RequireInitialized(
            selectedCharacterId,
            "Opportunity history selected Character is uninitialized.");
        if (resultState.SceneId != SceneId ||
            resultState.CurrentOpportunityCharacterId != selectedCharacterId)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity history result state is inconsistent with the selected Character.");
        }

        try
        {
            _ = resultState.StateHash.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity history result StateHash is uninitialized.",
                exception);
        }

        return new E0OpportunityHistory(
            SceneId,
            resultState.StateHash,
            CharacterIds.Add(selectedCharacterId));
    }
}

public sealed class E0OpportunityTransition
{
    internal E0OpportunityTransition(
        StateHash parentStateHash,
        StateHash resultStateHash,
        string strategyContract,
        CharacterId selectedCharacterId)
    {
        ContractVersion = E0OpportunityTransitionContracts.ContractVersion;
        ParentStateHash = parentStateHash;
        ResultStateHash = resultStateHash;
        StrategyContract = strategyContract;
        SelectedCharacterId = selectedCharacterId;
    }

    public string ContractVersion { get; }
    public StateHash ParentStateHash { get; }
    public StateHash ResultStateHash { get; }
    public string StrategyContract { get; }
    public CharacterId SelectedCharacterId { get; }
}

public sealed class E0OpportunityTransitionResult
{
    internal E0OpportunityTransitionResult(
        E0OpportunityTransition @event,
        ProductionState state,
        E0OpportunityHistory history,
        LeastInterventionDirectorEvaluation directorEvaluation)
    {
        Event = @event;
        State = state;
        History = history;
        DirectorEvaluation = directorEvaluation;
    }

    public E0OpportunityTransition Event { get; }
    public ProductionState State { get; }
    public E0OpportunityHistory History { get; }
    public LeastInterventionDirectorEvaluation DirectorEvaluation { get; }
}

public sealed class E0OpportunityTransitionException : Exception
{
    internal E0OpportunityTransitionException(string message)
        : base(message)
    {
    }

    internal E0OpportunityTransitionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
