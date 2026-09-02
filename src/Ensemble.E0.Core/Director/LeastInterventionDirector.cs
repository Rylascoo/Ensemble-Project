using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Director;

public static class LeastInterventionDirector
{
    public static LeastInterventionDirectorEvaluation Propose(DirectorOpportunityInput input)
    {
        if (input is null)
        {
            throw new DirectorOpportunityException("Director input is required.");
        }

        ValidateInput(input);

        var (selectedCharacterId, rule) = Select(input);
        var proposal = new DirectorOpportunityProposal(
            E0DirectorContracts.OpportunityContractVersion,
            input.SceneId,
            input.SourceCharacterId,
            selectedCharacterId);

        var neverOpportunitied = GetNeverOpportunitiedCharacterIds(input);
        var recentPattern = GetRecentAttentionPattern(input.OpportunityHistory);
        var trace = new LeastInterventionDirectorTrace(
            E0DirectorContracts.LeastInterventionStrategyContract,
            input,
            rule,
            neverOpportunitied,
            recentPattern);

        return new LeastInterventionDirectorEvaluation(proposal, trace);
    }

    private static (CharacterId SelectedCharacterId, LeastInterventionDirectorRule Rule) Select(
        DirectorOpportunityInput input)
    {
        if (input.NominatedCharacterId is CharacterId nominatedCharacterId)
        {
            return (nominatedCharacterId, LeastInterventionDirectorRule.Nomination);
        }

        if (input.AddressedCharacterIds.Length > 0)
        {
            return (
                SelectLeastRecent(input.AddressedCharacterIds, input.OpportunityHistory),
                LeastInterventionDirectorRule.DirectAddress);
        }

        return (
            SelectLeastRecent(input.RosterCharacterIds, input.OpportunityHistory),
            LeastInterventionDirectorRule.RecencyFallback);
    }

    private static CharacterId SelectLeastRecent(
        ImmutableArray<CharacterId> candidatePool,
        ImmutableArray<CharacterId> opportunityHistory)
    {
        if (candidatePool.IsDefault || candidatePool.Length == 0)
        {
            throw new DirectorOpportunityException("Director candidate pool must be non-empty.");
        }

        var lastIndices = new Dictionary<CharacterId, int>();
        for (var index = 0; index < opportunityHistory.Length; index++)
        {
            lastIndices[opportunityHistory[index]] = index;
        }

        CharacterId? selectedNeverSeen = null;
        foreach (var candidate in candidatePool)
        {
            if (lastIndices.ContainsKey(candidate))
            {
                continue;
            }

            if (selectedNeverSeen is null ||
                StringComparer.Ordinal.Compare(candidate.Value, selectedNeverSeen.Value.Value) < 0)
            {
                selectedNeverSeen = candidate;
            }
        }

        if (selectedNeverSeen is CharacterId neverSeen)
        {
            return neverSeen;
        }

        var selected = candidatePool[0];
        var selectedIndex = lastIndices[selected];

        for (var index = 1; index < candidatePool.Length; index++)
        {
            var candidate = candidatePool[index];
            var candidateIndex = lastIndices[candidate];

            if (candidateIndex < selectedIndex ||
                (candidateIndex == selectedIndex &&
                 StringComparer.Ordinal.Compare(candidate.Value, selected.Value) < 0))
            {
                selected = candidate;
                selectedIndex = candidateIndex;
            }
        }

        return selected;
    }

    private static ImmutableArray<CharacterId> GetNeverOpportunitiedCharacterIds(
        DirectorOpportunityInput input)
    {
        var seen = input.OpportunityHistory.ToHashSet();
        return input.RosterCharacterIds
            .Where(characterId => !seen.Contains(characterId))
            .OrderBy(characterId => characterId.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static DirectorRecentAttentionPattern GetRecentAttentionPattern(
        ImmutableArray<CharacterId> opportunityHistory)
    {
        if (opportunityHistory.Length >= 2 &&
            opportunityHistory[^1] == opportunityHistory[^2])
        {
            return DirectorRecentAttentionPattern.RepeatedSameCharacter;
        }

        if (opportunityHistory.Length >= 4)
        {
            var a = opportunityHistory[^4];
            var b = opportunityHistory[^3];
            var c = opportunityHistory[^2];
            var d = opportunityHistory[^1];

            if (a != b && a == c && b == d)
            {
                return DirectorRecentAttentionPattern.TwoCharacterAlternation;
            }
        }

        return DirectorRecentAttentionPattern.None;
    }

    private static void ValidateInput(DirectorOpportunityInput input)
    {
        try
        {
            _ = input.SceneId.Value;
            _ = input.SourceCharacterId.Value;
            _ = input.SourceContextPacketId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new DirectorOpportunityException(
                "Director input identity is not initialized.",
                exception);
        }

        if (input.RosterCharacterIds.IsDefault || input.RosterCharacterIds.Length != 3)
        {
            throw new DirectorOpportunityException(
                "Director E0 input roster must contain exactly three characters.");
        }

        if (input.AddressedCharacterIds.IsDefault)
        {
            throw new DirectorOpportunityException(
                "Director addressed-character input is not initialized.");
        }

        if (input.OpportunityHistory.IsDefault || input.OpportunityHistory.Length == 0)
        {
            throw new DirectorOpportunityException(
                "Director opportunity history must be initialized and non-empty.");
        }

        if (input.OpportunityHistory[^1] != input.SourceCharacterId)
        {
            throw new DirectorOpportunityException(
                "Director opportunity history must end at the source character.");
        }

        var rosterValues = new HashSet<string>(StringComparer.Ordinal);
        foreach (var characterId in input.RosterCharacterIds)
        {
            var value = RequireValue(
                characterId,
                "Director input roster contains an uninitialized character ID.");

            if (!rosterValues.Add(value))
            {
                throw new DirectorOpportunityException(
                    "Director input roster contains duplicate character IDs.");
            }
        }

        foreach (var historyCharacterId in input.OpportunityHistory)
        {
            var value = RequireValue(
                historyCharacterId,
                "Director opportunity history contains an uninitialized character ID.");

            if (!rosterValues.Contains(value))
            {
                throw new DirectorOpportunityException(
                    "Director opportunity history contains a character outside the E0 roster.");
            }
        }

        if (!rosterValues.Contains(input.SourceCharacterId.Value))
        {
            throw new DirectorOpportunityException(
                "Director source character is outside the E0 roster.");
        }

        foreach (var addressedCharacterId in input.AddressedCharacterIds)
        {
            var value = RequireValue(
                addressedCharacterId,
                "Director addressed-character input contains an uninitialized character ID.");

            if (!rosterValues.Contains(value))
            {
                throw new DirectorOpportunityException(
                    "Director addressed character is outside the E0 roster.");
            }
        }

        if (input.NominatedCharacterId is CharacterId nominatedCharacterId)
        {
            var value = RequireValue(
                nominatedCharacterId,
                "Director nominated-character input is uninitialized.");

            if (!rosterValues.Contains(value))
            {
                throw new DirectorOpportunityException(
                    "Director nominated character is outside the E0 roster.");
            }
        }
    }

    private static string RequireValue(CharacterId characterId, string message)
    {
        try
        {
            return characterId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new DirectorOpportunityException(message, exception);
        }
    }
}
