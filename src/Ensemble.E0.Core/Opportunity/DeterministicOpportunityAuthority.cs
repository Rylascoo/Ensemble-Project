using System.Collections.Immutable;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Serialization;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Opportunity;

public static class DeterministicOpportunityAuthority
{
    public static E0OpportunityTransitionResult Establish(
        ProductionState postCommitState,
        E0CausalCommit sourceCommit,
        ContextPacket sourceContext,
        E0OpportunityHistory sourceHistory)
    {
        var facts = ValidateSourceChain(postCommitState, sourceCommit, sourceHistory);

        if (sourceContext is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source ContextPacket is required.");
        }

        LeastInterventionDirectorEvaluation evaluation;
        try
        {
            OpportunityInvariants.RequireInitialized(
                sourceContext.SceneId,
                "Opportunity source Context SceneId is uninitialized.");
            OpportunityInvariants.RequireInitialized(
                sourceContext.ContextPacketId,
                "Opportunity source ContextPacketId is uninitialized.");

            if (sourceContext.SceneId != postCommitState.SceneId)
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity source Context Scene does not match Production state.");
            }

            if (sourceContext.ContextPacketId != facts.Candidate.ContextPacketId)
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity source ContextPacket does not match the Accepted Candidate.");
            }

            var input = DirectorOpportunityInput.Bind(
                sourceContext,
                facts.Candidate,
                sourceHistory.CharacterIds);

            if (!input.RosterCharacterIds.SequenceEqual(facts.RosterCharacterIds))
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity source Context roster does not match Production state.");
            }

            evaluation = LeastInterventionDirector.Propose(input);
        }
        catch (DirectorOpportunityException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity postcommit Director recomputation failed.",
                exception);
        }

        return ApplyLive(
            postCommitState,
            sourceHistory,
            facts.RosterCharacterIds,
            evaluation);
    }

    public static E0OpportunityTransitionResult Replay(
        ProductionState parentState,
        E0CausalCommit sourceCommit,
        E0OpportunityHistory sourceHistory,
        E0OpportunityTransition establishedEvent)
    {
        var facts = ValidateSourceChain(parentState, sourceCommit, sourceHistory);
        ValidateReplayEvent(parentState, establishedEvent, facts.RosterCharacterIds);

        LeastInterventionDirectorEvaluation evaluation;
        try
        {
            var input = new DirectorOpportunityInput(
                parentState.SceneId,
                facts.Candidate.SubjectCharacterId,
                facts.Candidate.ContextPacketId,
                facts.RosterCharacterIds,
                facts.Candidate.Control.AddressedCharacterIds,
                facts.Candidate.Control.NominatedCharacterId,
                sourceHistory.CharacterIds);

            evaluation = LeastInterventionDirector.Propose(input);
        }
        catch (DirectorOpportunityException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay Director recomputation failed.",
                exception);
        }

        if (evaluation.Proposal.SelectedCharacterId != establishedEvent.SelectedCharacterId)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay selected Character does not match the event.");
        }

        var selectedCharacterId = establishedEvent.SelectedCharacterId;
        var resultProjection = parentState.Projection with
        {
            CurrentOpportunityCharacterId = selectedCharacterId
        };

        StateHash recomputedHash;
        try
        {
            recomputedHash = OpportunityCanonicalizer.ComputeResultHash(
                parentState.StateHash,
                establishedEvent.StrategyContract,
                selectedCharacterId,
                resultProjection);
        }
        catch (CanonicalJsonException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay canonicalization failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay identity is uninitialized.",
                exception);
        }

        if (recomputedHash != establishedEvent.ResultStateHash)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay result StateHash does not match the event.");
        }

        try
        {
            var resultState = parentState.WithEstablishedOpportunity(
                selectedCharacterId,
                recomputedHash);
            var resultHistory = sourceHistory.Advance(
                resultState,
                selectedCharacterId);
            return new E0OpportunityTransitionResult(
                establishedEvent,
                resultState,
                resultHistory,
                evaluation);
        }
        catch (ProductionStateException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay Production transition failed.",
                exception);
        }
    }

    private static E0OpportunityTransitionResult ApplyLive(
        ProductionState postCommitState,
        E0OpportunityHistory sourceHistory,
        ImmutableArray<CharacterId> rosterCharacterIds,
        LeastInterventionDirectorEvaluation evaluation)
    {
        if (evaluation is null || evaluation.Proposal is null || evaluation.Trace is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity Director evaluation is invalid.");
        }

        if (!string.Equals(
                evaluation.Trace.StrategyContract,
                E0DirectorContracts.LeastInterventionStrategyContract,
                StringComparison.Ordinal))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity Director strategy contract is unsupported.");
        }

        var selectedCharacterId = evaluation.Proposal.SelectedCharacterId;
        OpportunityInvariants.RequireInitialized(
            selectedCharacterId,
            "Opportunity selected Character is uninitialized.");
        if (!rosterCharacterIds.Contains(selectedCharacterId))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity selected Character is outside the Production roster.");
        }

        var resultProjection = postCommitState.Projection with
        {
            CurrentOpportunityCharacterId = selectedCharacterId
        };

        StateHash resultStateHash;
        try
        {
            resultStateHash = OpportunityCanonicalizer.ComputeResultHash(
                postCommitState.StateHash,
                E0DirectorContracts.LeastInterventionStrategyContract,
                selectedCharacterId,
                resultProjection);
        }
        catch (CanonicalJsonException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity transition canonicalization failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity transition identity is uninitialized.",
                exception);
        }

        try
        {
            var establishedEvent = new E0OpportunityTransition(
                postCommitState.StateHash,
                resultStateHash,
                E0DirectorContracts.LeastInterventionStrategyContract,
                selectedCharacterId);
            var resultState = postCommitState.WithEstablishedOpportunity(
                selectedCharacterId,
                resultStateHash);
            var resultHistory = sourceHistory.Advance(
                resultState,
                selectedCharacterId);

            return new E0OpportunityTransitionResult(
                establishedEvent,
                resultState,
                resultHistory,
                evaluation);
        }
        catch (ProductionStateException exception)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity Production transition failed.",
                exception);
        }
    }

    private static OpportunitySourceFacts ValidateSourceChain(
        ProductionState postCommitState,
        E0CausalCommit sourceCommit,
        E0OpportunityHistory sourceHistory)
    {
        if (postCommitState is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity postcommit Production state is required.");
        }

        if (sourceCommit is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source causal commit is required.");
        }

        if (sourceHistory is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source history is required.");
        }

        OpportunityInvariants.RequireInitialized(
            postCommitState.StateHash,
            "Opportunity Production StateHash is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            postCommitState.SceneId,
            "Opportunity Production SceneId is uninitialized.");
        var rosterCharacterIds = OpportunityInvariants.ValidateProductionRoster(postCommitState);

        if (postCommitState.CurrentOpportunityCharacterId.HasValue)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity authority requires a no-opportunity postcommit Production state.");
        }

        if (!string.Equals(
                sourceCommit.ContractVersion,
                E0CausalCommitContracts.ContractVersion,
                StringComparison.Ordinal))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source causal commit contract is unsupported.");
        }

        OpportunityInvariants.RequireInitialized(
            sourceCommit.CommitId,
            "Opportunity source CommitId is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            sourceCommit.ParentStateHash,
            "Opportunity source parent StateHash is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            sourceCommit.ResultStateHash,
            "Opportunity source result StateHash is uninitialized.");

        if (sourceCommit.ResultStateHash != postCommitState.StateHash)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source causal commit did not produce the supplied Production state.");
        }

        var take = sourceCommit.Take;
        if (take is null ||
            !string.Equals(
                take.ContractVersion,
                E0TakeContracts.ContractVersion,
                StringComparison.Ordinal) ||
            take.Disposition != E0TakeDisposition.Accepted)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source causal commit does not contain an Accepted E0 Take.");
        }

        OpportunityInvariants.RequireInitialized(
            take.TakeId,
            "Opportunity source TakeId is uninitialized.");
        if (!postCommitState.ContainsEffectiveCommitId(sourceCommit.CommitId) ||
            !postCommitState.ContainsCommittedTakeId(take.TakeId))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source causal identities are not effective in the supplied Production state.");
        }

        var candidate = take.Performance;
        ValidateCandidate(candidate, rosterCharacterIds);

        if (take.InterpretationProposal is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source Take interpretation is missing.");
        }

        OpportunityInvariants.RequireInitialized(
            take.InterpretationProposal.SourceSceneId,
            "Opportunity source Take SceneId is uninitialized.");
        if (take.InterpretationProposal.SourceSceneId != postCommitState.SceneId)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source Take Scene does not match Production state.");
        }

        if (!rosterCharacterIds.Contains(candidate.SubjectCharacterId))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source Character is outside the Production roster.");
        }

        ValidateSourceHistory(
            sourceHistory,
            postCommitState.SceneId,
            rosterCharacterIds,
            sourceCommit.ParentStateHash,
            candidate.SubjectCharacterId);

        return new OpportunitySourceFacts(candidate, rosterCharacterIds);
    }

    private static void ValidateCandidate(
        CandidatePerformance candidate,
        ImmutableArray<CharacterId> rosterCharacterIds)
    {
        if (candidate is null ||
            !string.Equals(
                candidate.ContractVersion,
                PerformerCandidateContract.CandidateContractVersion,
                StringComparison.Ordinal))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source CandidatePerformance is invalid.");
        }

        OpportunityInvariants.RequireInitialized(
            candidate.SubjectCharacterId,
            "Opportunity source Candidate Character is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            candidate.ContextPacketId,
            "Opportunity source Candidate ContextPacketId is uninitialized.");

        if (candidate.Control is null || candidate.Control.AddressedCharacterIds.IsDefault)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source Candidate control is invalid.");
        }

        var rosterValues = rosterCharacterIds
            .Select(id => id.Value)
            .ToHashSet(StringComparer.Ordinal);
        var addressedValues = new HashSet<string>(StringComparer.Ordinal);
        foreach (var addressedCharacterId in candidate.Control.AddressedCharacterIds)
        {
            var value = OpportunityInvariants.RequireInitialized(
                addressedCharacterId,
                "Opportunity source addressed Character is uninitialized.");
            if (!rosterValues.Contains(value) ||
                addressedCharacterId == candidate.SubjectCharacterId ||
                !addressedValues.Add(value))
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity source Candidate addressed control is invalid.");
            }
        }

        var canonicalAddressed = candidate.Control.AddressedCharacterIds
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
        if (!canonicalAddressed.SequenceEqual(candidate.Control.AddressedCharacterIds))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source Candidate addressed control is not canonical.");
        }

        if (candidate.Control.NominatedCharacterId is CharacterId nominatedCharacterId)
        {
            var value = OpportunityInvariants.RequireInitialized(
                nominatedCharacterId,
                "Opportunity source nominated Character is uninitialized.");
            if (!rosterValues.Contains(value) ||
                nominatedCharacterId == candidate.SubjectCharacterId)
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity source Candidate nomination is invalid.");
            }
        }
    }

    private static void ValidateSourceHistory(
        E0OpportunityHistory sourceHistory,
        SceneId sceneId,
        ImmutableArray<CharacterId> rosterCharacterIds,
        StateHash expectedLastOpportunityStateHash,
        CharacterId expectedLastCharacterId)
    {
        OpportunityInvariants.RequireInitialized(
            sourceHistory.SceneId,
            "Opportunity source history SceneId is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            sourceHistory.LastOpportunityStateHash,
            "Opportunity source history StateHash is uninitialized.");

        if (sourceHistory.SceneId != sceneId)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source history belongs to another Scene.");
        }

        if (sourceHistory.CharacterIds.IsDefault || sourceHistory.CharacterIds.Length == 0)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source history is empty or uninitialized.");
        }

        var rosterValues = rosterCharacterIds
            .Select(id => id.Value)
            .ToHashSet(StringComparer.Ordinal);
        foreach (var characterId in sourceHistory.CharacterIds)
        {
            var value = OpportunityInvariants.RequireInitialized(
                characterId,
                "Opportunity source history contains an uninitialized Character.");
            if (!rosterValues.Contains(value))
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity source history contains a Character outside the Production roster.");
            }
        }

        if (sourceHistory.LastOpportunityStateHash != expectedLastOpportunityStateHash)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source history does not bind to the source causal parent state.");
        }

        if (sourceHistory.CharacterIds[^1] != expectedLastCharacterId)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity source history does not end at the Accepted source Character.");
        }
    }

    private static void ValidateReplayEvent(
        ProductionState parentState,
        E0OpportunityTransition establishedEvent,
        ImmutableArray<CharacterId> rosterCharacterIds)
    {
        if (establishedEvent is null)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay event is required.");
        }

        if (!string.Equals(
                establishedEvent.ContractVersion,
                E0OpportunityTransitionContracts.ContractVersion,
                StringComparison.Ordinal))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay event contract is unsupported.");
        }

        OpportunityInvariants.RequireInitialized(
            establishedEvent.ParentStateHash,
            "Opportunity replay parent StateHash is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            establishedEvent.ResultStateHash,
            "Opportunity replay result StateHash is uninitialized.");
        OpportunityInvariants.RequireInitialized(
            establishedEvent.SelectedCharacterId,
            "Opportunity replay selected Character is uninitialized.");

        if (establishedEvent.ParentStateHash != parentState.StateHash)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay parent StateHash does not match the event.");
        }

        if (!string.Equals(
                establishedEvent.StrategyContract,
                E0DirectorContracts.LeastInterventionStrategyContract,
                StringComparison.Ordinal))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay strategy contract is unsupported.");
        }

        if (!rosterCharacterIds.Contains(establishedEvent.SelectedCharacterId))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity replay selected Character is outside the Production roster.");
        }
    }

    private sealed record OpportunitySourceFacts(
        CandidatePerformance Candidate,
        ImmutableArray<CharacterId> RosterCharacterIds);
}

internal static class OpportunityInvariants
{
    internal static ImmutableArray<CharacterId> ValidateProductionRoster(ProductionState state)
    {
        var roster = state.RosterCharacterIds;
        if (roster.IsDefault || roster.Length != 3)
        {
            throw new E0OpportunityTransitionException(
                "Opportunity Production roster must contain exactly three Characters.");
        }

        var values = new HashSet<string>(StringComparer.Ordinal);
        foreach (var characterId in roster)
        {
            var value = RequireInitialized(
                characterId,
                "Opportunity Production roster contains an uninitialized Character.");
            if (!values.Add(value))
            {
                throw new E0OpportunityTransitionException(
                    "Opportunity Production roster contains duplicate Characters.");
            }
        }

        var canonical = roster
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
        if (!canonical.SequenceEqual(roster))
        {
            throw new E0OpportunityTransitionException(
                "Opportunity Production roster is not canonical.");
        }

        return roster;
    }

    internal static string RequireInitialized(CharacterId id, string message)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(message, exception);
        }
    }

    internal static string RequireInitialized(SceneId id, string message)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(message, exception);
        }
    }

    internal static string RequireInitialized(ContextPacketId id, string message)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(message, exception);
        }
    }

    internal static string RequireInitialized(CommitId id, string message)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(message, exception);
        }
    }

    internal static string RequireInitialized(TakeId id, string message)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(message, exception);
        }
    }

    internal static string RequireInitialized(StateHash hash, string message)
    {
        try
        {
            return hash.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0OpportunityTransitionException(message, exception);
        }
    }
}
