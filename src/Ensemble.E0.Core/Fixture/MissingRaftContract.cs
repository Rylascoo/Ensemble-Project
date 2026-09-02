using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Fixture;

public static class MissingRaftContract
{
    public const string FamilyIdValue = "ensemble.e0.missing-raft";
    public const string VersionValue = "0.1.0";
    public const string FixtureIdValue = "ensemble.e0.missing-raft@0.1.0";
    public const string SceneIdValue = "SCENE-MISSING-RAFT";

    public const string MarloweCharacterId = "MARLOWE";
    public const string VossCharacterId = "VOSS";
    public const string WrenCharacterId = "WREN";
    public const string MarloweDisplayName = "Marlowe";
    public const string VossDisplayName = "Dr. Voss";
    public const string WrenDisplayName = "Wren";

    public const string Rh001Id = "RH-001";
    public const string Rh002Id = "RH-002";
    public const string HtWrenSecuredRaftId = "HT-WREN-SECURED-RAFT";
    public const string HtRaftDeterioratedId = "HT-RAFT-DETERIORATED";
    public const string HtMarloweReleasedRaftId = "HT-MARLOWE-RELEASED-RAFT";
    public const string HtMarloweNotWarnedId = "HT-MARLOWE-NOT-WARNED";
    public const string HtCurrentCarriedRaftAwayId = "HT-CURRENT-CARRIED-RAFT-AWAY";
    public const string HtCurrentDidNotReleaseMooringId = "HT-CURRENT-DID-NOT-RELEASE-MOORING";
    public const string HtMarloweReturnedShorelineId = "HT-MARLOWE-RETURNED-SHORELINE";

    public const string UpRaftWouldFailId = "UP-RAFT-WOULD-FAIL";
    public const string UpRh001CacheWouldBeLostId = "UP-RH001-CACHE-WOULD-BE-LOST";
    public const string WorldCurrentStrengthenedId = "WORLD-CURRENT-STRENGTHENED";
    public const string SceneRaftGoneId = "SCENE-RAFT-GONE";
    public const string SceneProvisionsLimitedId = "SCENE-PROVISIONS-LIMITED";
    public const string SceneNoImmediateEmergencyId = "SCENE-NO-IMMEDIATE-EMERGENCY";
    public const string SceneNoExternalCountdownId = "SCENE-NO-EXTERNAL-COUNTDOWN";
    public const string PressureIsolationId = "PRESSURE-ISOLATION";

    public const string ConMarloweId = "CON-MARLOWE";
    public const string DispMarloweId = "DISP-MARLOWE";
    public const string ObsMarloweRaftDeteriorationId = "OBS-MARLOWE-RAFT-DETERIORATION";
    public const string ObsMarloweNobodyVisibleId = "OBS-MARLOWE-NOBODY-VISIBLE";
    public const string KnowMarloweReleasedRaftId = "KNOW-MARLOWE-RELEASED-RAFT";
    public const string KnowMarloweNotWarnedId = "KNOW-MARLOWE-NOT-WARNED";
    public const string BelMarloweDamageUnacceptableId = "BEL-MARLOWE-DAMAGE-UNACCEPTABLE";
    public const string BelMarloweGroupLikelyProceedId = "BEL-MARLOWE-GROUP-LIKELY-PROCEED";
    public const string BelMarloweDisclosureRiskId = "BEL-MARLOWE-DISCLOSURE-RISK";
    public const string MemMarloweRh001Id = "MEM-MARLOWE-RH001";
    public const string MemMarloweRh002Id = "MEM-MARLOWE-RH002";
    public const string GoalMarloweId = "GOAL-MARLOWE";
    public const string RelMarloweVossId = "REL-MARLOWE-VOSS";
    public const string RelMarloweWrenId = "REL-MARLOWE-WREN";

    public const string ConVossId = "CON-VOSS";
    public const string DispVossId = "DISP-VOSS";
    public const string KnowVossCurrentStrengthenedId = "KNOW-VOSS-CURRENT-STRENGTHENED";
    public const string BelVossAccidentalLossPlausibleId = "BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE";
    public const string MemVossRh001Id = "MEM-VOSS-RH001";
    public const string MemVossRh002Id = "MEM-VOSS-RH002";
    public const string GoalVossId = "GOAL-VOSS";
    public const string RelVossMarloweId = "REL-VOSS-MARLOWE";
    public const string RelVossWrenId = "REL-VOSS-WREN";

    public const string ConWrenId = "CON-WREN";
    public const string DispWrenId = "DISP-WREN";
    public const string ObsWrenMarloweReturnId = "OBS-WREN-MARLOWE-RETURN";
    public const string KnowWrenSecuredRaftId = "KNOW-WREN-SECURED-RAFT";
    public const string SuspWrenMarloweKnowsMoreId = "SUSP-WREN-MARLOWE-KNOWS-MORE";
    public const string MemWrenRh002Id = "MEM-WREN-RH002";
    public const string GoalWrenId = "GOAL-WREN";
    public const string RelWrenMarloweId = "REL-WREN-MARLOWE";
    public const string RelWrenVossId = "REL-WREN-VOSS";

    public static FixtureFamilyId FamilyId { get; } = FixtureFamilyId.From(FamilyIdValue);
    public static FixtureVersion Version { get; } = FixtureVersion.From(VersionValue);
    public static FixtureId ExpectedFixtureId { get; } = Domain.FixtureId.From(FixtureIdValue);
    public static SceneId ExpectedSceneId { get; } = Domain.SceneId.From(SceneIdValue);
    public static CharacterId MarloweId { get; } = CharacterId.From(MarloweCharacterId);
    public static CharacterId VossId { get; } = CharacterId.From(VossCharacterId);
    public static CharacterId WrenId { get; } = CharacterId.From(WrenCharacterId);

    public static ImmutableHashSet<RecordId> RelationshipHistoryRecordIds { get; } = RecordIds(Rh001Id, Rh002Id);

    public static ImmutableHashSet<RecordId> RelationshipRecordIds { get; } = RecordIds(
        RelMarloweVossId,
        RelMarloweWrenId,
        RelVossMarloweId,
        RelVossWrenId,
        RelWrenMarloweId,
        RelWrenVossId);

    public static ImmutableHashSet<RecordId> RelationshipDerivedMemoryRecordIds { get; } = RecordIds(
        MemMarloweRh001Id,
        MemMarloweRh002Id,
        MemVossRh001Id,
        MemVossRh002Id,
        MemWrenRh002Id);

    public static ImmutableHashSet<RecordId> RelationshipContextRecordIds { get; } =
        RelationshipHistoryRecordIds
            .Union(RelationshipRecordIds)
            .Union(RelationshipDerivedMemoryRecordIds)
            .ToImmutableHashSet();

    private static readonly ImmutableHashSet<RecordId> NoRecordIds = ImmutableHashSet<RecordId>.Empty;

    private static readonly ImmutableHashSet<CharacterId> CharacterIds =
        ImmutableHashSet.Create(MarloweId, VossId, WrenId);

    private static readonly ImmutableHashSet<RecordId> HistoricalTruthIds = RecordIds(
        Rh001Id,
        Rh002Id,
        HtWrenSecuredRaftId,
        HtRaftDeterioratedId,
        HtMarloweReleasedRaftId,
        HtMarloweNotWarnedId,
        HtCurrentCarriedRaftAwayId,
        HtCurrentDidNotReleaseMooringId,
        HtMarloweReturnedShorelineId);

    private static readonly ImmutableHashSet<RecordId> UnresolvedPropositionIds =
        RecordIds(UpRaftWouldFailId, UpRh001CacheWouldBeLostId);

    private static readonly ImmutableHashSet<RecordId> WorldStateIds = RecordIds(WorldCurrentStrengthenedId);

    private static readonly ImmutableHashSet<RecordId> SceneStateIds = RecordIds(
        SceneRaftGoneId,
        SceneProvisionsLimitedId,
        SceneNoImmediateEmergencyId,
        SceneNoExternalCountdownId);

    private static readonly ImmutableHashSet<RecordId> PressureIds = RecordIds(PressureIsolationId);

    private static readonly ImmutableArray<RecordId> ChronologyIds = ImmutableArray.Create(
        RecordId.From(Rh001Id),
        RecordId.From(Rh002Id),
        RecordId.From(HtWrenSecuredRaftId),
        RecordId.From(HtRaftDeterioratedId),
        RecordId.From(HtMarloweReleasedRaftId),
        RecordId.From(HtCurrentCarriedRaftAwayId),
        RecordId.From(HtMarloweReturnedShorelineId));

    private static readonly ImmutableDictionary<RecordId, CharacterId> RelationshipTargets =
        new Dictionary<RecordId, CharacterId>
        {
            [RecordId.From(RelMarloweVossId)] = VossId,
            [RecordId.From(RelMarloweWrenId)] = WrenId,
            [RecordId.From(RelVossMarloweId)] = MarloweId,
            [RecordId.From(RelVossWrenId)] = WrenId,
            [RecordId.From(RelWrenMarloweId)] = MarloweId,
            [RecordId.From(RelWrenVossId)] = VossId
        }.ToImmutableDictionary();

    public static void Validate(ValidatedFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        RequireEqual(fixture.FamilyId, FamilyId, "Fixture family");
        RequireEqual(fixture.Version, Version, "Fixture version");
        RequireEqual(fixture.Id, ExpectedFixtureId, "Fixture ID");
        RequireEqual(fixture.SchemaVersion, E0FixtureDialect.SchemaVersion, "Schema version");
        RequireEqual(fixture.AccessContract, E0FixtureDialect.AccessContract, "Access contract");
        RequireEqual(fixture.ObservationContract, E0FixtureDialect.ObservationContract, "Observation contract");

        RequireEqual(fixture.Scene.Id, ExpectedSceneId, "Scene ID");
        RequireExactSet(fixture.Characters.Select(character => character.Id), CharacterIds, "Character set");
        RequireExactSet(fixture.Scene.Roster, CharacterIds, "Scene roster");
        RequireEqual(fixture.InitialOpportunity, VossId, "Initial opportunity");

        RequireExactSet(fixture.HistoricalTruth.Select(record => record.Id), HistoricalTruthIds, "HistoricalTruth");
        RequireExactSet(fixture.UnresolvedPropositions.Select(record => record.Id), UnresolvedPropositionIds, "UnresolvedProposition");
        RequireExactSet(fixture.WorldState.Select(record => record.Id), WorldStateIds, "WorldState");
        RequireExactSet(fixture.SceneState.Select(record => record.Id), SceneStateIds, "SceneState");
        RequireExactSet(fixture.Pressures.Select(record => record.Id), PressureIds, "Pressure");

        if (!fixture.Chronology.SequenceEqual(ChronologyIds))
        {
            throw new FixtureValidationException("Missing Raft chronology does not match the frozen sequence.");
        }

        var characters = fixture.Characters.ToDictionary(character => character.Id);
        ValidateMarlowe(characters[MarloweId]);
        ValidateVoss(characters[VossId]);
        ValidateWren(characters[WrenId]);

        ValidateProvenance(fixture.HistoricalTruth);
        ValidateProvenance(fixture.UnresolvedPropositions);
        ValidateProvenance(fixture.WorldState);
        ValidateProvenance(fixture.SceneState);
        ValidateProvenance(fixture.Pressures);

        foreach (var character in fixture.Characters)
        {
            ValidateProvenance(CharacterRecords(character));
            ValidateRelationshipProvenance(character.Relationships);
            ValidateRelationshipTargets(character.Relationships);
        }

        EnsureDoesNotCiteHiddenRelease(characters[VossId]);
        EnsureDoesNotCiteHiddenRelease(characters[WrenId]);

        var derivedRelationshipContext = RelationshipHistoryRecordIds
            .Union(RelationshipRecordIds)
            .Union(RelationshipDerivedMemoryRecordIds)
            .ToImmutableHashSet();

        if (!RelationshipContextRecordIds.SetEquals(derivedRelationshipContext))
        {
            throw new FixtureValidationException("RelationshipContextRecordIds must be the exact derived union of its component sets.");
        }
    }

    private static void ValidateMarlowe(ValidatedCharacter character)
    {
        RequireEqual(character.DisplayName, MarloweDisplayName, "MARLOWE display name");
        ValidateCharacterCollections(
            character,
            constitution: RecordIds(ConMarloweId),
            disposition: RecordIds(DispMarloweId),
            observations: RecordIds(ObsMarloweRaftDeteriorationId, ObsMarloweNobodyVisibleId),
            knowledge: RecordIds(KnowMarloweReleasedRaftId, KnowMarloweNotWarnedId),
            beliefs: RecordIds(BelMarloweDamageUnacceptableId, BelMarloweGroupLikelyProceedId, BelMarloweDisclosureRiskId),
            suspicions: NoRecordIds,
            memories: RecordIds(MemMarloweRh001Id, MemMarloweRh002Id),
            goals: RecordIds(GoalMarloweId),
            relationships: RecordIds(RelMarloweVossId, RelMarloweWrenId));
    }

    private static void ValidateVoss(ValidatedCharacter character)
    {
        RequireEqual(character.DisplayName, VossDisplayName, "VOSS display name");
        ValidateCharacterCollections(
            character,
            constitution: RecordIds(ConVossId),
            disposition: RecordIds(DispVossId),
            observations: NoRecordIds,
            knowledge: RecordIds(KnowVossCurrentStrengthenedId),
            beliefs: RecordIds(BelVossAccidentalLossPlausibleId),
            suspicions: NoRecordIds,
            memories: RecordIds(MemVossRh001Id, MemVossRh002Id),
            goals: RecordIds(GoalVossId),
            relationships: RecordIds(RelVossMarloweId, RelVossWrenId));
    }

    private static void ValidateWren(ValidatedCharacter character)
    {
        RequireEqual(character.DisplayName, WrenDisplayName, "WREN display name");
        ValidateCharacterCollections(
            character,
            constitution: RecordIds(ConWrenId),
            disposition: RecordIds(DispWrenId),
            observations: RecordIds(ObsWrenMarloweReturnId),
            knowledge: RecordIds(KnowWrenSecuredRaftId),
            beliefs: NoRecordIds,
            suspicions: RecordIds(SuspWrenMarloweKnowsMoreId),
            memories: RecordIds(MemWrenRh002Id),
            goals: RecordIds(GoalWrenId),
            relationships: RecordIds(RelWrenMarloweId, RelWrenVossId));
    }

    private static void ValidateCharacterCollections(
        ValidatedCharacter character,
        ImmutableHashSet<RecordId> constitution,
        ImmutableHashSet<RecordId> disposition,
        ImmutableHashSet<RecordId> observations,
        ImmutableHashSet<RecordId> knowledge,
        ImmutableHashSet<RecordId> beliefs,
        ImmutableHashSet<RecordId> suspicions,
        ImmutableHashSet<RecordId> memories,
        ImmutableHashSet<RecordId> goals,
        ImmutableHashSet<RecordId> relationships)
    {
        RequireExactSet(character.Constitution.Select(record => record.Id), constitution, $"{character.Id} Constitution");
        RequireExactSet(character.Disposition.Select(record => record.Id), disposition, $"{character.Id} Disposition");
        RequireExactSet(character.Circumstance.Select(record => record.Id), NoRecordIds, $"{character.Id} Circumstance");
        RequireExactSet(character.Observations.Select(record => record.Id), observations, $"{character.Id} Observation");
        RequireExactSet(character.Knowledge.Select(record => record.Id), knowledge, $"{character.Id} Knowledge");
        RequireExactSet(character.Beliefs.Select(record => record.Id), beliefs, $"{character.Id} Belief");
        RequireExactSet(character.Suspicions.Select(record => record.Id), suspicions, $"{character.Id} Suspicion");
        RequireExactSet(character.Memories.Select(record => record.Id), memories, $"{character.Id} Memory");
        RequireExactSet(character.Goals.Select(record => record.Id), goals, $"{character.Id} Goal");
        RequireExactSet(character.Relationships.Select(record => record.Id), relationships, $"{character.Id} Relationship");
    }

    private static void ValidateProvenance(IEnumerable<ValidatedRecord> records)
    {
        foreach (var record in records)
        {
            RequireExactSet(record.Provenance, ExpectedProvenance(record.Id), $"Provenance for {record.Id}");
        }
    }

    private static void ValidateRelationshipProvenance(IEnumerable<ValidatedRelationship> relationships)
    {
        foreach (var relationship in relationships)
        {
            RequireExactSet(
                relationship.Provenance,
                ExpectedProvenance(relationship.Id),
                $"Provenance for {relationship.Id}");
        }
    }

    private static void ValidateRelationshipTargets(IEnumerable<ValidatedRelationship> relationships)
    {
        foreach (var relationship in relationships)
        {
            if (!RelationshipTargets.TryGetValue(relationship.Id, out var expectedTarget) ||
                relationship.TargetCharacterId != expectedTarget)
            {
                throw new FixtureValidationException(
                    $"Relationship '{relationship.Id}' has an invalid target Character.");
            }
        }
    }

    private static void EnsureDoesNotCiteHiddenRelease(ValidatedCharacter character)
    {
        var hiddenRelease = RecordId.From(HtMarloweReleasedRaftId);

        if (CharacterRecords(character).Any(record => record.Provenance.Contains(hiddenRelease)) ||
            character.Relationships.Any(relationship => relationship.Provenance.Contains(hiddenRelease)))
        {
            throw new FixtureValidationException(
                $"Character '{character.Id}' may not cite hidden release truth '{hiddenRelease}'.");
        }
    }

    private static IEnumerable<ValidatedRecord> CharacterRecords(ValidatedCharacter character) =>
        character.Constitution
            .Concat(character.Disposition)
            .Concat(character.Circumstance)
            .Concat(character.Observations)
            .Concat(character.Knowledge)
            .Concat(character.Beliefs)
            .Concat(character.Suspicions)
            .Concat(character.Memories)
            .Concat(character.Goals);

    private static ImmutableHashSet<RecordId> ExpectedProvenance(RecordId id) => id.Value switch
    {
        SceneRaftGoneId => RecordIds(HtCurrentCarriedRaftAwayId),
        PressureIsolationId => RecordIds(SceneRaftGoneId, SceneProvisionsLimitedId),
        UpRaftWouldFailId => RecordIds(HtRaftDeterioratedId),
        UpRh001CacheWouldBeLostId => RecordIds(Rh001Id),

        ObsMarloweRaftDeteriorationId => RecordIds(HtRaftDeterioratedId),
        KnowMarloweReleasedRaftId => RecordIds(HtMarloweReleasedRaftId),
        KnowMarloweNotWarnedId => RecordIds(HtMarloweNotWarnedId),
        BelMarloweDamageUnacceptableId => RecordIds(ObsMarloweRaftDeteriorationId),
        MemMarloweRh001Id => RecordIds(Rh001Id),
        MemMarloweRh002Id => RecordIds(Rh002Id),
        RelMarloweVossId => RecordIds(Rh001Id),
        RelMarloweWrenId => RecordIds(Rh002Id),

        KnowVossCurrentStrengthenedId => RecordIds(WorldCurrentStrengthenedId),
        BelVossAccidentalLossPlausibleId => RecordIds(KnowVossCurrentStrengthenedId, SceneRaftGoneId),
        MemVossRh001Id => RecordIds(Rh001Id),
        MemVossRh002Id => RecordIds(Rh002Id),
        RelVossMarloweId => RecordIds(Rh001Id),
        RelVossWrenId => RecordIds(Rh002Id),

        KnowWrenSecuredRaftId => RecordIds(HtWrenSecuredRaftId),
        ObsWrenMarloweReturnId => RecordIds(HtMarloweReturnedShorelineId),
        SuspWrenMarloweKnowsMoreId => RecordIds(KnowWrenSecuredRaftId, ObsWrenMarloweReturnId),
        MemWrenRh002Id => RecordIds(Rh002Id),
        RelWrenMarloweId => RecordIds(Rh002Id),
        RelWrenVossId => RecordIds(Rh002Id),

        _ => NoRecordIds
    };

    private static ImmutableHashSet<RecordId> RecordIds(params string[] values) =>
        values.Select(RecordId.From).ToImmutableHashSet();

    private static void RequireExactSet<T>(
        IEnumerable<T> actual,
        ImmutableHashSet<T> expected,
        string label)
        where T : notnull
    {
        var values = actual.ToArray();
        if (values.Length != expected.Count || !values.ToHashSet().SetEquals(expected))
        {
            throw new FixtureValidationException($"Missing Raft {label} does not match the frozen contract.");
        }
    }

    private static void RequireEqual<T>(T actual, T expected, string label)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
        {
            throw new FixtureValidationException($"Missing Raft {label} does not match the frozen contract.");
        }
    }
}
