using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Performer;

public sealed class CandidatePerformance
{
    internal CandidatePerformance(
        string contractVersion,
        CharacterId subjectCharacterId,
        ContextPacketId contextPacketId,
        string visibleText,
        CandidatePerformanceControl control)
    {
        ContractVersion = contractVersion;
        SubjectCharacterId = subjectCharacterId;
        ContextPacketId = contextPacketId;
        VisibleText = visibleText;
        Control = control;
    }

    public string ContractVersion { get; }
    public CharacterId SubjectCharacterId { get; }
    public ContextPacketId ContextPacketId { get; }
    public string VisibleText { get; }
    public CandidatePerformanceControl Control { get; }
}

public sealed class CandidatePerformanceControl
{
    internal CandidatePerformanceControl(
        ImmutableArray<CharacterId> addressedCharacterIds,
        CharacterId? nominatedCharacterId)
    {
        AddressedCharacterIds = addressedCharacterIds;
        NominatedCharacterId = nominatedCharacterId;
    }

    public ImmutableArray<CharacterId> AddressedCharacterIds { get; }
    public CharacterId? NominatedCharacterId { get; }
}

public sealed class PerformerCandidateException : Exception
{
    public PerformerCandidateException(string message)
        : base(message)
    {
    }
}
