using System.Collections.Immutable;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;

namespace Ensemble.E0.Core.Director;

public static class E0DirectorContracts
{
    public const string OpportunityContractVersion = "ensemble.e0.director.opportunity.v1";
    public const string LeastInterventionStrategyContract = "ensemble.e0.director.least-intervention.v1";
}

public enum LeastInterventionDirectorRule
{
    Nomination,
    DirectAddress,
    RecencyFallback
}

public enum DirectorRecentAttentionPattern
{
    None,
    RepeatedSameCharacter,
    TwoCharacterAlternation
}

public sealed class DirectorOpportunityInput
{
    internal DirectorOpportunityInput(
        SceneId sceneId,
        CharacterId sourceCharacterId,
        ContextPacketId sourceContextPacketId,
        ImmutableArray<CharacterId> rosterCharacterIds,
        ImmutableArray<CharacterId> addressedCharacterIds,
        CharacterId? nominatedCharacterId,
        ImmutableArray<CharacterId> opportunityHistory)
    {
        SceneId = sceneId;
        SourceCharacterId = sourceCharacterId;
        SourceContextPacketId = sourceContextPacketId;
        RosterCharacterIds = rosterCharacterIds;
        AddressedCharacterIds = addressedCharacterIds;
        NominatedCharacterId = nominatedCharacterId;
        OpportunityHistory = opportunityHistory;
    }

    public SceneId SceneId { get; }
    public CharacterId SourceCharacterId { get; }
    public ContextPacketId SourceContextPacketId { get; }
    public ImmutableArray<CharacterId> RosterCharacterIds { get; }
    public ImmutableArray<CharacterId> AddressedCharacterIds { get; }
    public CharacterId? NominatedCharacterId { get; }
    public ImmutableArray<CharacterId> OpportunityHistory { get; }

    public static DirectorOpportunityInput Bind(
        ContextPacket sourceContext,
        CandidatePerformance sourceCandidate,
        ImmutableArray<CharacterId> opportunityHistory)
    {
        if (sourceContext is null)
        {
            throw new DirectorOpportunityException("Director source context is required.");
        }

        if (sourceCandidate is null)
        {
            throw new DirectorOpportunityException("Director source candidate is required.");
        }

        EnsureSourceIdentitiesInitialized(sourceContext, sourceCandidate);

        if (sourceContext.SubjectCharacterId != sourceContext.OpportunityCharacterId)
        {
            throw new DirectorOpportunityException(
                "Director source context subject must equal its opportunity character.");
        }

        if (sourceCandidate.SubjectCharacterId != sourceContext.SubjectCharacterId)
        {
            throw new DirectorOpportunityException(
                "Director source candidate subject does not match the source context subject.");
        }

        if (sourceCandidate.ContextPacketId != sourceContext.ContextPacketId)
        {
            throw new DirectorOpportunityException(
                "Director source candidate context identity does not match the source context.");
        }

        if (sourceContext.Roster.IsDefault || sourceContext.Roster.Length != 3)
        {
            throw new DirectorOpportunityException(
                "Director E0 source roster must contain exactly three characters.");
        }

        var rosterBuilder = ImmutableArray.CreateBuilder<CharacterId>(sourceContext.Roster.Length);
        var rosterValues = new HashSet<string>(StringComparer.Ordinal);
        var sourceOccurrences = 0;

        foreach (var participant in sourceContext.Roster)
        {
            if (participant is null)
            {
                throw new DirectorOpportunityException(
                    "Director E0 source roster contains an invalid participant.");
            }

            EnsureInitialized(participant.CharacterId, "Director E0 source roster contains an uninitialized character ID.");

            if (!rosterValues.Add(participant.CharacterId.Value))
            {
                throw new DirectorOpportunityException(
                    "Director E0 source roster contains duplicate character IDs.");
            }

            if (participant.CharacterId == sourceContext.SubjectCharacterId)
            {
                sourceOccurrences++;
            }

            rosterBuilder.Add(participant.CharacterId);
        }

        if (sourceOccurrences != 1)
        {
            throw new DirectorOpportunityException(
                "Director source character must appear exactly once in the E0 roster.");
        }

        if (opportunityHistory.IsDefault || opportunityHistory.Length == 0)
        {
            throw new DirectorOpportunityException(
                "Director opportunity history must be initialized and non-empty.");
        }

        var historyBuilder = ImmutableArray.CreateBuilder<CharacterId>(opportunityHistory.Length);
        foreach (var characterId in opportunityHistory)
        {
            EnsureInitialized(characterId, "Director opportunity history contains an uninitialized character ID.");

            if (!rosterValues.Contains(characterId.Value))
            {
                throw new DirectorOpportunityException(
                    "Director opportunity history contains a character outside the E0 roster.");
            }

            historyBuilder.Add(characterId);
        }

        if (opportunityHistory[^1] != sourceContext.SubjectCharacterId)
        {
            throw new DirectorOpportunityException(
                "Director opportunity history must end at the source character.");
        }

        if (sourceCandidate.Control is null || sourceCandidate.Control.AddressedCharacterIds.IsDefault)
        {
            throw new DirectorOpportunityException(
                "Director source candidate control is not initialized.");
        }

        var rosterCharacterIds = rosterBuilder
            .ToImmutable()
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
        var addressedCharacterIds = sourceCandidate.Control.AddressedCharacterIds
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();

        return new DirectorOpportunityInput(
            sourceContext.SceneId,
            sourceContext.SubjectCharacterId,
            sourceContext.ContextPacketId,
            rosterCharacterIds,
            addressedCharacterIds,
            sourceCandidate.Control.NominatedCharacterId,
            historyBuilder.ToImmutable());
    }

    private static void EnsureSourceIdentitiesInitialized(
        ContextPacket sourceContext,
        CandidatePerformance sourceCandidate)
    {
        try
        {
            _ = sourceContext.SceneId.Value;
            _ = sourceContext.SubjectCharacterId.Value;
            _ = sourceContext.OpportunityCharacterId.Value;
            _ = sourceContext.ContextPacketId.Value;
            _ = sourceCandidate.SubjectCharacterId.Value;
            _ = sourceCandidate.ContextPacketId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new DirectorOpportunityException(
                "Director source identity is not initialized.",
                exception);
        }
    }

    private static void EnsureInitialized(CharacterId characterId, string message)
    {
        try
        {
            _ = characterId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new DirectorOpportunityException(message, exception);
        }
    }
}

public sealed class DirectorOpportunityProposal
{
    internal DirectorOpportunityProposal(
        string contractVersion,
        SceneId sceneId,
        CharacterId sourceCharacterId,
        CharacterId selectedCharacterId)
    {
        ContractVersion = contractVersion;
        SceneId = sceneId;
        SourceCharacterId = sourceCharacterId;
        SelectedCharacterId = selectedCharacterId;
    }

    public string ContractVersion { get; }
    public SceneId SceneId { get; }
    public CharacterId SourceCharacterId { get; }
    public CharacterId SelectedCharacterId { get; }
}

public sealed class LeastInterventionDirectorTrace
{
    internal LeastInterventionDirectorTrace(
        string strategyContract,
        DirectorOpportunityInput input,
        LeastInterventionDirectorRule rule,
        ImmutableArray<CharacterId> neverOpportunitiedCharacterIds,
        DirectorRecentAttentionPattern recentAttentionPattern)
    {
        StrategyContract = strategyContract;
        Input = input;
        Rule = rule;
        NeverOpportunitiedCharacterIds = neverOpportunitiedCharacterIds;
        RecentAttentionPattern = recentAttentionPattern;
    }

    public string StrategyContract { get; }
    public DirectorOpportunityInput Input { get; }
    public LeastInterventionDirectorRule Rule { get; }
    public ImmutableArray<CharacterId> NeverOpportunitiedCharacterIds { get; }
    public DirectorRecentAttentionPattern RecentAttentionPattern { get; }
}

public sealed class LeastInterventionDirectorEvaluation
{
    internal LeastInterventionDirectorEvaluation(
        DirectorOpportunityProposal proposal,
        LeastInterventionDirectorTrace trace)
    {
        Proposal = proposal;
        Trace = trace;
    }

    public DirectorOpportunityProposal Proposal { get; }
    public LeastInterventionDirectorTrace Trace { get; }
}

public sealed class DirectorOpportunityException : Exception
{
    public DirectorOpportunityException(string message)
        : base(message)
    {
    }

    public DirectorOpportunityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
