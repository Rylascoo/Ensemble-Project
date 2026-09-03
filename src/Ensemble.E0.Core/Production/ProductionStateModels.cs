using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Provenance;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Production;

public static class ProductionStateContracts
{
    public const string StateContractVersion = "ensemble.e0.production-state.v1";
    public const string StateHashContractVersion = "ensemble.e0.production-state-hash.sha256.v1";
}

public enum ProductionRecordDomain
{
    Unspecified = 0,
    HistoricalTruth = 1,
    UnresolvedProposition = 2,
    WorldState = 3,
    SceneState = 4,
    CharacterConstitution = 5,
    CharacterDisposition = 6,
    CharacterCircumstance = 7,
    CharacterObservation = 8,
    CharacterKnowledge = 9,
    CharacterBelief = 10,
    CharacterSuspicion = 11,
    CharacterMemory = 12,
    CharacterGoal = 13,
    CharacterClaim = 14,
    Relationship = 15,
    Pressure = 16
}

public enum ProductionRecordLifecycle
{
    Unspecified = 0,
    Active = 1,
    Inactive = 2
}

public enum ProductionRecordProtection
{
    Unspecified = 0,
    None = 1,
    SystemImmutable = 2,
    CreatorLocked = 3
}

public readonly record struct StateHash
{
    private readonly string? _value;

    private StateHash(string value) => _value = value;

    public string Value =>
        _value ?? throw new InvalidOperationException("StateHash is uninitialized.");

    internal static StateHash Create(string lowerHexSha256)
    {
        if (!ProductionStateInvariants.IsLowerHexSha256(lowerHexSha256))
        {
            throw new InvalidOperationException("StateHash canonical digest is invalid.");
        }

        return new StateHash(lowerHexSha256);
    }

    public override string ToString() => Value;
}

public sealed class ProductionCharacter
{
    internal ProductionCharacter(CharacterId characterId, string displayName)
    {
        CharacterId = characterId;
        DisplayName = displayName;
    }

    public CharacterId CharacterId { get; }
    public string DisplayName { get; }
}

public abstract class ProductionRecord
{
    internal ProductionRecord(
        RecordId recordId,
        ProductionRecordDomain domain,
        ProductionRecordLifecycle lifecycle,
        ProductionRecordProtection protection,
        string text,
        ImmutableArray<RecordId> provenance)
    {
        RecordId = recordId;
        Domain = domain;
        Lifecycle = lifecycle;
        Protection = protection;
        Text = text;
        Provenance = provenance;
    }

    public RecordId RecordId { get; }
    public ProductionRecordDomain Domain { get; }
    public ProductionRecordLifecycle Lifecycle { get; }
    public ProductionRecordProtection Protection { get; }
    public string Text { get; }
    public ImmutableArray<RecordId> Provenance { get; }

    internal abstract ProductionRecord WithLifecycle(ProductionRecordLifecycle lifecycle);
}

public sealed class GlobalProductionRecord : ProductionRecord
{
    internal GlobalProductionRecord(
        RecordId recordId,
        ProductionRecordDomain domain,
        ProductionRecordLifecycle lifecycle,
        ProductionRecordProtection protection,
        string text,
        ImmutableArray<RecordId> provenance)
        : base(recordId, domain, lifecycle, protection, text, provenance)
    {
    }

    internal override ProductionRecord WithLifecycle(ProductionRecordLifecycle lifecycle) =>
        new GlobalProductionRecord(RecordId, Domain, lifecycle, Protection, Text, Provenance);
}

public sealed class CharacterProductionRecord : ProductionRecord
{
    internal CharacterProductionRecord(
        RecordId recordId,
        ProductionRecordDomain domain,
        CharacterId subjectCharacterId,
        ProductionRecordLifecycle lifecycle,
        ProductionRecordProtection protection,
        string text,
        ImmutableArray<RecordId> provenance)
        : base(recordId, domain, lifecycle, protection, text, provenance)
    {
        SubjectCharacterId = subjectCharacterId;
    }

    public CharacterId SubjectCharacterId { get; }

    internal override ProductionRecord WithLifecycle(ProductionRecordLifecycle lifecycle) =>
        new CharacterProductionRecord(
            RecordId,
            Domain,
            SubjectCharacterId,
            lifecycle,
            Protection,
            Text,
            Provenance);
}

public sealed class RelationshipProductionRecord : ProductionRecord
{
    internal RelationshipProductionRecord(
        RecordId recordId,
        CharacterId subjectCharacterId,
        CharacterId targetCharacterId,
        ProductionRecordLifecycle lifecycle,
        ProductionRecordProtection protection,
        string text,
        ImmutableArray<RecordId> provenance)
        : base(
            recordId,
            ProductionRecordDomain.Relationship,
            lifecycle,
            protection,
            text,
            provenance)
    {
        SubjectCharacterId = subjectCharacterId;
        TargetCharacterId = targetCharacterId;
    }

    public CharacterId SubjectCharacterId { get; }
    public CharacterId TargetCharacterId { get; }

    internal override ProductionRecord WithLifecycle(ProductionRecordLifecycle lifecycle) =>
        new RelationshipProductionRecord(
            RecordId,
            SubjectCharacterId,
            TargetCharacterId,
            lifecycle,
            Protection,
            Text,
            Provenance);
}

internal sealed record ProductionStateProjection(
    FixtureId OriginFixtureId,
    FixtureFamilyId OriginFixtureFamilyId,
    FixtureVersion OriginFixtureVersion,
    string OriginFixtureHash,
    SceneId SceneId,
    ImmutableArray<ProductionCharacter> Characters,
    ImmutableArray<CharacterId> RosterCharacterIds,
    CharacterId? CurrentOpportunityCharacterId,
    ImmutableArray<ProductionRecord> Records);

public sealed class ProductionState
{
    private readonly ProductionStateProjection _projection;
    private readonly ImmutableHashSet<string> _effectiveCommitIds;
    private readonly ImmutableHashSet<string> _committedTakeIds;

    internal ProductionState(
        ProductionStateProjection projection,
        StateHash stateHash,
        ImmutableHashSet<string> effectiveCommitIds,
        ImmutableHashSet<string> committedTakeIds)
    {
        _projection = projection;
        StateHash = stateHash;
        _effectiveCommitIds = effectiveCommitIds;
        _committedTakeIds = committedTakeIds;
    }

    public string ContractVersion => ProductionStateContracts.StateContractVersion;
    public StateHash StateHash { get; }
    public FixtureId OriginFixtureId => _projection.OriginFixtureId;
    public FixtureFamilyId OriginFixtureFamilyId => _projection.OriginFixtureFamilyId;
    public FixtureVersion OriginFixtureVersion => _projection.OriginFixtureVersion;
    public string OriginFixtureHash => _projection.OriginFixtureHash;
    public SceneId SceneId => _projection.SceneId;
    public ImmutableArray<ProductionCharacter> Characters => _projection.Characters;
    public ImmutableArray<CharacterId> RosterCharacterIds => _projection.RosterCharacterIds;
    public CharacterId? CurrentOpportunityCharacterId => _projection.CurrentOpportunityCharacterId;
    public ImmutableArray<ProductionRecord> Records => _projection.Records;

    internal ProductionStateProjection Projection => _projection;

    public static ProductionState Initialize(
        ValidatedFixture fixture,
        ImmutableArray<RecordId> creatorLockedRecordIds)
    {
        if (fixture is null)
        {
            throw new ProductionStateException("Production genesis fixture is required.");
        }

        try
        {
            var projection = ProductionGenesisProjection.Create(fixture, creatorLockedRecordIds);
            var stateHash = ProductionStateCanonicalizer.ComputeGenesisHash(projection);
            return new ProductionState(
                projection,
                stateHash,
                ImmutableHashSet.Create<string>(StringComparer.Ordinal),
                ImmutableHashSet.Create<string>(StringComparer.Ordinal));
        }
        catch (FixtureValidationException exception)
        {
            throw new ProductionStateException(
                "Production genesis fixture validation failed.",
                exception);
        }
        catch (ProductionGenesisProjectionException exception)
        {
            throw new ProductionStateException(
                "Production genesis mapping failed.",
                exception);
        }
        catch (RecordProvenanceGraphException exception)
        {
            throw new ProductionStateException(
                "Production genesis provenance graph is invalid.",
                exception);
        }
        catch (CanonicalJsonException exception)
        {
            throw new ProductionStateException(
                "Production genesis canonicalization failed.",
                exception);
        }
    }

    internal bool ContainsEffectiveCommitId(CommitId commitId) =>
        _effectiveCommitIds.Contains(commitId.Value);

    internal bool ContainsCommittedTakeId(TakeId takeId) =>
        _committedTakeIds.Contains(takeId.Value);

    internal ProductionState WithCommittedTransition(
        ProductionStateProjection projection,
        StateHash stateHash,
        CommitId commitId,
        TakeId takeId) =>
        new(
            projection,
            stateHash,
            _effectiveCommitIds.Add(commitId.Value),
            _committedTakeIds.Add(takeId.Value));
}

public sealed class ProductionStateException : Exception
{
    internal ProductionStateException(string message)
        : base(message)
    {
    }

    internal ProductionStateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

internal sealed class ProductionGenesisProjectionException : Exception
{
    internal ProductionGenesisProjectionException(string message)
        : base(message)
    {
    }
}

internal static class ProductionGenesisProjection
{
    internal static ProductionStateProjection Create(
        ValidatedFixture fixture,
        ImmutableArray<RecordId> creatorLockedRecordIds)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        return CreateCore(
            fixture,
            creatorLockedRecordIds,
            FixtureHash.Compute(fixture));
    }

    internal static ProductionStateProjection CreateForAuthority(
        ValidatedFixture fixture,
        ImmutableArray<RecordId> creatorLockedRecordIds)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        return CreateCore(fixture, creatorLockedRecordIds, originFixtureHash: string.Empty);
    }

    private static ProductionStateProjection CreateCore(
        ValidatedFixture fixture,
        ImmutableArray<RecordId> creatorLockedRecordIds,
        string originFixtureHash)
    {
        if (creatorLockedRecordIds.IsDefault)
        {
            throw new ProductionGenesisProjectionException(
                "Creator-lock input is required.");
        }

        RequireInitialized(fixture.Id);
        RequireInitialized(fixture.FamilyId);
        _ = fixture.Version.Value;
        RequireInitialized(fixture.Scene.Id);

        var characters = fixture.Characters
            .Select(character =>
            {
                RequireInitialized(character.Id);
                if (character.DisplayName is null)
                {
                    throw new ProductionGenesisProjectionException(
                        "Fixture Character display identity is invalid.");
                }

                return new ProductionCharacter(character.Id, character.DisplayName);
            })
            .OrderBy(character => character.CharacterId.Value, StringComparer.Ordinal)
            .ToImmutableArray();

        EnsureStrictUniqueCharacters(characters);
        var knownCharacters = characters
            .Select(character => character.CharacterId.Value)
            .ToHashSet(StringComparer.Ordinal);

        var roster = CanonicalizeRoster(fixture.Scene.Roster, knownCharacters);
        RequireInitialized(fixture.InitialOpportunity);
        if (!roster.Contains(fixture.InitialOpportunity))
        {
            throw new ProductionGenesisProjectionException(
                "Initial opportunity is outside the Scene roster.");
        }

        var records = ImmutableArray.CreateBuilder<ProductionRecord>();
        AddGlobal(records, fixture.HistoricalTruth, ProductionRecordDomain.HistoricalTruth, ProductionRecordProtection.SystemImmutable);
        AddGlobal(records, fixture.UnresolvedPropositions, ProductionRecordDomain.UnresolvedProposition, ProductionRecordProtection.None);
        AddGlobal(records, fixture.WorldState, ProductionRecordDomain.WorldState, ProductionRecordProtection.None);
        AddGlobal(records, fixture.SceneState, ProductionRecordDomain.SceneState, ProductionRecordProtection.None);
        AddGlobal(records, fixture.Pressures, ProductionRecordDomain.Pressure, ProductionRecordProtection.None);

        foreach (var character in fixture.Characters)
        {
            if (!knownCharacters.Contains(character.Id.Value))
            {
                throw new ProductionGenesisProjectionException(
                    "Fixture Character identity is inconsistent.");
            }

            AddCharacter(records, character.Constitution, character.Id, ProductionRecordDomain.CharacterConstitution, ProductionRecordProtection.SystemImmutable);
            AddCharacter(records, character.Disposition, character.Id, ProductionRecordDomain.CharacterDisposition, ProductionRecordProtection.None);
            AddCharacter(records, character.Circumstance, character.Id, ProductionRecordDomain.CharacterCircumstance, ProductionRecordProtection.None);
            AddCharacter(records, character.Observations, character.Id, ProductionRecordDomain.CharacterObservation, ProductionRecordProtection.SystemImmutable);
            AddCharacter(records, character.Knowledge, character.Id, ProductionRecordDomain.CharacterKnowledge, ProductionRecordProtection.None);
            AddCharacter(records, character.Beliefs, character.Id, ProductionRecordDomain.CharacterBelief, ProductionRecordProtection.None);
            AddCharacter(records, character.Suspicions, character.Id, ProductionRecordDomain.CharacterSuspicion, ProductionRecordProtection.None);
            AddCharacter(records, character.Memories, character.Id, ProductionRecordDomain.CharacterMemory, ProductionRecordProtection.None);
            AddCharacter(records, character.Goals, character.Id, ProductionRecordDomain.CharacterGoal, ProductionRecordProtection.None);

            foreach (var relationship in character.Relationships)
            {
                RequireInitialized(relationship.Id);
                RequireInitialized(relationship.TargetCharacterId);
                if (!roster.Contains(character.Id) || !roster.Contains(relationship.TargetCharacterId))
                {
                    throw new ProductionGenesisProjectionException(
                        "Fixture relationship references a Character outside the Scene roster.");
                }

                if (character.Id == relationship.TargetCharacterId)
                {
                    throw new ProductionGenesisProjectionException(
                        "Fixture relationship cannot target its subject Character.");
                }

                records.Add(new RelationshipProductionRecord(
                    relationship.Id,
                    character.Id,
                    relationship.TargetCharacterId,
                    ProductionRecordLifecycle.Active,
                    ProductionRecordProtection.None,
                    relationship.Text,
                    ValidateProvenance(relationship.Provenance)));
            }
        }

        var canonicalRecords = records
            .ToImmutable()
            .OrderBy(record => record.RecordId.Value, StringComparer.Ordinal)
            .ToImmutableArray();
        EnsureStrictUniqueRecords(canonicalRecords);

        var locks = ValidateCreatorLocks(creatorLockedRecordIds, canonicalRecords);
        if (locks.Count != 0)
        {
            canonicalRecords = canonicalRecords
                .Select(record =>
                    locks.Contains(record.RecordId.Value) &&
                    record.Protection != ProductionRecordProtection.SystemImmutable
                        ? WithProtection(record, ProductionRecordProtection.CreatorLocked)
                        : record)
                .ToImmutableArray();
        }

        RecordProvenanceGraphValidator.Validate(
            canonicalRecords.Select(record =>
                new KeyValuePair<RecordId, ImmutableArray<RecordId>>(
                    record.RecordId,
                    record.Provenance)));

        if (originFixtureHash.Length != 0 &&
            !ProductionStateInvariants.IsLowerHexSha256(originFixtureHash))
        {
            throw new ProductionGenesisProjectionException(
                "Fixture hash is invalid.");
        }

        return new ProductionStateProjection(
            fixture.Id,
            fixture.FamilyId,
            fixture.Version,
            originFixtureHash,
            fixture.Scene.Id,
            characters,
            roster,
            fixture.InitialOpportunity,
            canonicalRecords);
    }

    private static ImmutableArray<CharacterId> CanonicalizeRoster(
        ImmutableArray<CharacterId> roster,
        HashSet<string> knownCharacters)
    {
        if (roster.IsDefault || roster.Length != 3)
        {
            throw new ProductionGenesisProjectionException(
                "Scene roster must contain exactly three E0 Characters.");
        }

        var values = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in roster)
        {
            var value = RequireInitialized(id);
            if (!values.Add(value) || !knownCharacters.Contains(value))
            {
                throw new ProductionGenesisProjectionException(
                    "Scene roster is invalid.");
            }
        }

        return roster
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static HashSet<string> ValidateCreatorLocks(
        ImmutableArray<RecordId> creatorLockedRecordIds,
        ImmutableArray<ProductionRecord> records)
    {
        var known = records
            .Select(record => record.RecordId.Value)
            .ToHashSet(StringComparer.Ordinal);
        var locks = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in creatorLockedRecordIds)
        {
            var value = RequireInitialized(id);
            if (!locks.Add(value))
            {
                throw new ProductionGenesisProjectionException(
                    "Creator-lock input contains a duplicate Record ID.");
            }

            if (!known.Contains(value))
            {
                throw new ProductionGenesisProjectionException(
                    "Creator-lock input contains an unknown Record ID.");
            }
        }

        return locks;
    }

    private static ProductionRecord WithProtection(
        ProductionRecord record,
        ProductionRecordProtection protection) =>
        record switch
        {
            GlobalProductionRecord global => new GlobalProductionRecord(
                global.RecordId,
                global.Domain,
                global.Lifecycle,
                protection,
                global.Text,
                global.Provenance),
            CharacterProductionRecord character => new CharacterProductionRecord(
                character.RecordId,
                character.Domain,
                character.SubjectCharacterId,
                character.Lifecycle,
                protection,
                character.Text,
                character.Provenance),
            RelationshipProductionRecord relationship => new RelationshipProductionRecord(
                relationship.RecordId,
                relationship.SubjectCharacterId,
                relationship.TargetCharacterId,
                relationship.Lifecycle,
                protection,
                relationship.Text,
                relationship.Provenance),
            _ => throw new ProductionGenesisProjectionException(
                "Production record shape is unsupported.")
        };

    private static void AddGlobal(
        ImmutableArray<ProductionRecord>.Builder target,
        ImmutableArray<ValidatedRecord> source,
        ProductionRecordDomain domain,
        ProductionRecordProtection protection)
    {
        foreach (var record in source)
        {
            target.Add(new GlobalProductionRecord(
                record.Id,
                domain,
                ProductionRecordLifecycle.Active,
                protection,
                record.Text,
                ValidateProvenance(record.Provenance)));
        }
    }

    private static void AddCharacter(
        ImmutableArray<ProductionRecord>.Builder target,
        ImmutableArray<ValidatedRecord> source,
        CharacterId subjectCharacterId,
        ProductionRecordDomain domain,
        ProductionRecordProtection protection)
    {
        RequireInitialized(subjectCharacterId);
        foreach (var record in source)
        {
            target.Add(new CharacterProductionRecord(
                record.Id,
                domain,
                subjectCharacterId,
                ProductionRecordLifecycle.Active,
                protection,
                record.Text,
                ValidateProvenance(record.Provenance)));
        }
    }

    private static ImmutableArray<RecordId> ValidateProvenance(ImmutableArray<RecordId> provenance)
    {
        if (provenance.IsDefault)
        {
            throw new ProductionGenesisProjectionException(
                "Record provenance is invalid.");
        }

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in provenance)
        {
            var value = RequireInitialized(id);
            if (!seen.Add(value))
            {
                throw new ProductionGenesisProjectionException(
                    "Record provenance contains duplicate Record IDs.");
            }
        }

        return provenance
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static void EnsureStrictUniqueCharacters(ImmutableArray<ProductionCharacter> characters)
    {
        var previous = string.Empty;
        for (var index = 0; index < characters.Length; index++)
        {
            var value = characters[index].CharacterId.Value;
            if (index != 0 && string.CompareOrdinal(previous, value) >= 0)
            {
                throw new ProductionGenesisProjectionException(
                    "Production Characters are not unique.");
            }

            previous = value;
        }
    }

    private static void EnsureStrictUniqueRecords(ImmutableArray<ProductionRecord> records)
    {
        var previous = string.Empty;
        for (var index = 0; index < records.Length; index++)
        {
            var value = RequireInitialized(records[index].RecordId);
            if (index != 0 && string.CompareOrdinal(previous, value) >= 0)
            {
                throw new ProductionGenesisProjectionException(
                    "Production Record IDs are not globally unique.");
            }

            previous = value;
        }
    }

    private static string RequireInitialized(FixtureId id)
    {
        try { return id.Value; }
        catch (InvalidOperationException) { throw new ProductionGenesisProjectionException("FixtureId is uninitialized."); }
    }

    private static string RequireInitialized(FixtureFamilyId id)
    {
        try { return id.Value; }
        catch (InvalidOperationException) { throw new ProductionGenesisProjectionException("FixtureFamilyId is uninitialized."); }
    }

    private static string RequireInitialized(SceneId id)
    {
        try { return id.Value; }
        catch (InvalidOperationException) { throw new ProductionGenesisProjectionException("SceneId is uninitialized."); }
    }

    private static string RequireInitialized(CharacterId id)
    {
        try { return id.Value; }
        catch (InvalidOperationException) { throw new ProductionGenesisProjectionException("CharacterId is uninitialized."); }
    }

    private static string RequireInitialized(RecordId id)
    {
        try { return id.Value; }
        catch (InvalidOperationException) { throw new ProductionGenesisProjectionException("RecordId is uninitialized."); }
    }
}

internal static class ProductionStateInvariants
{
    internal static bool IsLowerHexSha256(string? value)
    {
        if (value is null || value.Length != 64)
        {
            return false;
        }

        foreach (var character in value)
        {
            if (!((character >= '0' && character <= '9') ||
                  (character >= 'a' && character <= 'f')))
            {
                return false;
            }
        }

        return true;
    }
}
