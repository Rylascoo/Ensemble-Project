using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Fixture;

public sealed class ValidatedFixture
{
    internal ValidatedFixture(
        FixtureId id,
        FixtureFamilyId familyId,
        FixtureVersion version,
        ValidatedScene scene,
        CharacterId initialOpportunity,
        ImmutableArray<RecordId> chronology,
        ImmutableArray<ValidatedRecord> historicalTruth,
        ImmutableArray<ValidatedRecord> unresolvedPropositions,
        ImmutableArray<ValidatedRecord> worldState,
        ImmutableArray<ValidatedRecord> sceneState,
        ImmutableArray<ValidatedCharacter> characters,
        ImmutableArray<ValidatedRecord> pressures)
    {
        Id = id;
        FamilyId = familyId;
        Version = version;
        Scene = scene;
        InitialOpportunity = initialOpportunity;
        Chronology = chronology;
        HistoricalTruth = historicalTruth;
        UnresolvedPropositions = unresolvedPropositions;
        WorldState = worldState;
        SceneState = sceneState;
        Characters = characters;
        Pressures = pressures;

        ProvenanceDagValidator.Validate(this);
    }

    public FixtureId Id { get; }
    public FixtureFamilyId FamilyId { get; }
    public FixtureVersion Version { get; }
    public string SchemaVersion => E0FixtureDialect.SchemaVersion;
    public string AccessContract => E0FixtureDialect.AccessContract;
    public string ObservationContract => E0FixtureDialect.ObservationContract;
    public ValidatedScene Scene { get; }
    public CharacterId InitialOpportunity { get; }
    public ImmutableArray<RecordId> Chronology { get; }
    public ImmutableArray<ValidatedRecord> HistoricalTruth { get; }
    public ImmutableArray<ValidatedRecord> UnresolvedPropositions { get; }
    public ImmutableArray<ValidatedRecord> WorldState { get; }
    public ImmutableArray<ValidatedRecord> SceneState { get; }
    public ImmutableArray<ValidatedCharacter> Characters { get; }
    public ImmutableArray<ValidatedRecord> Pressures { get; }
}

public sealed record ValidatedScene(SceneId Id, ImmutableArray<CharacterId> Roster);

public sealed record ValidatedRecord(
    RecordId Id,
    string Text,
    ImmutableArray<RecordId> Provenance);

public sealed record ValidatedRelationship(
    RecordId Id,
    CharacterId TargetCharacterId,
    string Text,
    ImmutableArray<RecordId> Provenance);

public sealed record ValidatedCharacter(
    CharacterId Id,
    string DisplayName,
    ImmutableArray<ValidatedRecord> Constitution,
    ImmutableArray<ValidatedRecord> Disposition,
    ImmutableArray<ValidatedRecord> Circumstance,
    ImmutableArray<ValidatedRecord> Observations,
    ImmutableArray<ValidatedRecord> Knowledge,
    ImmutableArray<ValidatedRecord> Beliefs,
    ImmutableArray<ValidatedRecord> Suspicions,
    ImmutableArray<ValidatedRecord> Memories,
    ImmutableArray<ValidatedRecord> Goals,
    ImmutableArray<ValidatedRelationship> Relationships);
