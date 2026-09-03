using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Core.StateAuthority;

internal static class ProductionStateAuthoritySnapshot
{
    internal static StateAuthoritySnapshot Bind(ProductionState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        try
        {
            _ = state.StateHash.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new StateAuthorityException(
                "State Authority Production state identity is uninitialized.",
                exception);
        }

        return BindProjection(state.Projection);
    }

    internal static StateAuthoritySnapshot BindProjection(ProductionStateProjection projection)
    {
        ArgumentNullException.ThrowIfNull(projection);
        StateAuthoritySnapshot.RequireInitialized(projection.SceneId, "Production SceneId");

        if (projection.RosterCharacterIds.IsDefault || projection.RosterCharacterIds.Length != 3)
        {
            throw new StateAuthorityException(
                "State Authority Production roster is invalid.");
        }

        var roster = ValidateRoster(projection.RosterCharacterIds);
        if (projection.Records.IsDefault)
        {
            throw new StateAuthorityException(
                "State Authority Production records are invalid.");
        }

        var descriptors = projection.Records
            .Select(ProjectRecord)
            .OrderBy(descriptor => descriptor.RecordId.Value, StringComparer.Ordinal)
            .ToImmutableArray();

        var previous = string.Empty;
        for (var index = 0; index < descriptors.Length; index++)
        {
            var value = StateAuthoritySnapshot.RequireInitialized(
                descriptors[index].RecordId,
                "Production RecordId");
            if (index != 0 && string.CompareOrdinal(previous, value) >= 0)
            {
                throw new StateAuthorityException(
                    "State Authority Production records are not globally unique and canonical.");
            }

            previous = value;
        }

        return new StateAuthoritySnapshot(projection.SceneId, roster, descriptors);
    }

    private static ImmutableArray<CharacterId> ValidateRoster(
        ImmutableArray<CharacterId> roster)
    {
        var previous = string.Empty;
        for (var index = 0; index < roster.Length; index++)
        {
            var value = StateAuthoritySnapshot.RequireInitialized(
                roster[index],
                "Production roster CharacterId");
            if (index != 0 && string.CompareOrdinal(previous, value) >= 0)
            {
                throw new StateAuthorityException(
                    "State Authority Production roster is not canonical.");
            }

            previous = value;
        }

        return roster;
    }

    private static StateAuthorityRecordDescriptor ProjectRecord(ProductionRecord record)
    {
        if (record is null)
        {
            throw new StateAuthorityException(
                "State Authority Production contains an invalid record.");
        }

        var domain = MapDomain(record.Domain);
        var lifecycle = MapLifecycle(record.Lifecycle);
        var protection = MapProtection(record.Protection);

        return record switch
        {
            GlobalProductionRecord =>
                new GlobalStateAuthorityRecordDescriptor(
                    record.RecordId,
                    domain,
                    lifecycle,
                    protection),
            CharacterProductionRecord character =>
                new CharacterStateAuthorityRecordDescriptor(
                    character.RecordId,
                    domain,
                    character.SubjectCharacterId,
                    lifecycle,
                    protection),
            RelationshipProductionRecord relationship =>
                new RelationshipStateAuthorityRecordDescriptor(
                    relationship.RecordId,
                    relationship.SubjectCharacterId,
                    relationship.TargetCharacterId,
                    lifecycle,
                    protection),
            _ => throw new StateAuthorityException(
                "State Authority Production record shape is unsupported.")
        };
    }

    internal static StateAuthorityRecordDomain MapDomain(ProductionRecordDomain domain) =>
        domain switch
        {
            ProductionRecordDomain.HistoricalTruth => StateAuthorityRecordDomain.HistoricalTruth,
            ProductionRecordDomain.UnresolvedProposition => StateAuthorityRecordDomain.UnresolvedProposition,
            ProductionRecordDomain.WorldState => StateAuthorityRecordDomain.WorldState,
            ProductionRecordDomain.SceneState => StateAuthorityRecordDomain.SceneState,
            ProductionRecordDomain.CharacterConstitution => StateAuthorityRecordDomain.CharacterConstitution,
            ProductionRecordDomain.CharacterDisposition => StateAuthorityRecordDomain.CharacterDisposition,
            ProductionRecordDomain.CharacterCircumstance => StateAuthorityRecordDomain.CharacterCircumstance,
            ProductionRecordDomain.CharacterObservation => StateAuthorityRecordDomain.CharacterObservation,
            ProductionRecordDomain.CharacterKnowledge => StateAuthorityRecordDomain.CharacterKnowledge,
            ProductionRecordDomain.CharacterBelief => StateAuthorityRecordDomain.CharacterBelief,
            ProductionRecordDomain.CharacterSuspicion => StateAuthorityRecordDomain.CharacterSuspicion,
            ProductionRecordDomain.CharacterMemory => StateAuthorityRecordDomain.CharacterMemory,
            ProductionRecordDomain.CharacterGoal => StateAuthorityRecordDomain.CharacterGoal,
            ProductionRecordDomain.CharacterClaim => StateAuthorityRecordDomain.CharacterClaim,
            ProductionRecordDomain.Relationship => StateAuthorityRecordDomain.Relationship,
            ProductionRecordDomain.Pressure => StateAuthorityRecordDomain.Pressure,
            _ => throw new StateAuthorityException(
                "State Authority Production record domain is invalid.")
        };

    internal static StateAuthorityRecordLifecycle MapLifecycle(ProductionRecordLifecycle lifecycle) =>
        lifecycle switch
        {
            ProductionRecordLifecycle.Active => StateAuthorityRecordLifecycle.Active,
            ProductionRecordLifecycle.Inactive => StateAuthorityRecordLifecycle.Inactive,
            _ => throw new StateAuthorityException(
                "State Authority Production record lifecycle is invalid.")
        };

    internal static StateAuthorityRecordProtection MapProtection(ProductionRecordProtection protection) =>
        protection switch
        {
            ProductionRecordProtection.None => StateAuthorityRecordProtection.None,
            ProductionRecordProtection.SystemImmutable => StateAuthorityRecordProtection.SystemImmutable,
            ProductionRecordProtection.CreatorLocked => StateAuthorityRecordProtection.CreatorLocked,
            _ => throw new StateAuthorityException(
                "State Authority Production record protection is invalid.")
        };
}

internal static class StateAuthoritySnapshotSemanticComparer
{
    internal static bool Equals(
        StateAuthoritySnapshot left,
        StateAuthoritySnapshot right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null ||
            left.SceneId != right.SceneId ||
            left.RosterCharacterIds.IsDefault ||
            right.RosterCharacterIds.IsDefault ||
            !left.RosterCharacterIds.SequenceEqual(right.RosterCharacterIds) ||
            left.Records.IsDefault ||
            right.Records.IsDefault ||
            left.Records.Length != right.Records.Length)
        {
            return false;
        }

        for (var index = 0; index < left.Records.Length; index++)
        {
            if (!DescriptorEquals(left.Records[index], right.Records[index]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool DescriptorEquals(
        StateAuthorityRecordDescriptor left,
        StateAuthorityRecordDescriptor right)
    {
        if (left is null || right is null ||
            left.GetType() != right.GetType() ||
            left.RecordId != right.RecordId ||
            left.RecordDomain != right.RecordDomain ||
            left.Lifecycle != right.Lifecycle ||
            left.Protection != right.Protection)
        {
            return false;
        }

        return (left, right) switch
        {
            (GlobalStateAuthorityRecordDescriptor, GlobalStateAuthorityRecordDescriptor) => true,
            (CharacterStateAuthorityRecordDescriptor a, CharacterStateAuthorityRecordDescriptor b) =>
                a.SubjectCharacterId == b.SubjectCharacterId,
            (RelationshipStateAuthorityRecordDescriptor a, RelationshipStateAuthorityRecordDescriptor b) =>
                a.SubjectCharacterId == b.SubjectCharacterId &&
                a.TargetCharacterId == b.TargetCharacterId,
            _ => false
        };
    }
}
