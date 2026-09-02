using System.Text;
using System.Text.Json.Nodes;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Access;

[TestClass]
public sealed class CharacterBoundedAccessControlTests
{
    [TestMethod]
    public void GenericSmoke_ProjectsOnlySharedAndSubjectOwnedInformation()
    {
        var fixture = LoadFixture("e0-fixture-v1.json");

        var evaluation = CharacterBoundedAccessControl.Evaluate(fixture, CharacterId.From("CHAR-A"));
        var actualIds = ProjectionRecordIds(evaluation.Projection);
        var expectedIds = new[] { "CON-A", "PRESSURE-001", "REL-A-B" };

        AssertEquivalent(expectedIds, actualIds);
        Assert.AreEqual("SCENE-001", evaluation.Projection.SceneId.Value);
        Assert.AreEqual("CHAR-A", evaluation.Projection.SubjectCharacterId.Value);
        CollectionAssert.AreEqual(
            new[] { "CHAR-A", "CHAR-B", "CHAR-C" },
            evaluation.Projection.Roster.Select(participant => participant.CharacterId.Value).ToArray());
        CollectionAssert.AreEqual(
            new[] { "A", "B", "C" },
            evaluation.Projection.Roster.Select(participant => participant.DisplayName).ToArray());
    }

    [TestMethod]
    [DataRow(MissingRaftContract.MarloweCharacterId)]
    [DataRow(MissingRaftContract.VossCharacterId)]
    [DataRow(MissingRaftContract.WrenCharacterId)]
    public void MissingRaft_ExactPermittedRecordSetMatchesApprovedMatrix(string characterId)
    {
        var fixture = LoadMissingRaft();
        var evaluation = CharacterBoundedAccessControl.Evaluate(fixture, CharacterId.From(characterId));

        var expected = ExpectedMissingRaftPermittedIds(characterId);
        var actual = ProjectionRecordIds(evaluation.Projection);

        AssertEquivalent(expected, actual);
    }

    [TestMethod]
    public void ProductionAuthorityAndChronology_AreExcludedForEveryMissingRaftCharacter()
    {
        var fixture = LoadMissingRaft();
        var productionIds = fixture.HistoricalTruth
            .Concat(fixture.UnresolvedPropositions)
            .Concat(fixture.WorldState)
            .Select(record => record.Id)
            .ToArray();

        foreach (var character in fixture.Characters)
        {
            var evaluation = CharacterBoundedAccessControl.Evaluate(fixture, character.Id);
            var projectedIds = ProjectionRecordIds(evaluation.Projection).ToHashSet(StringComparer.Ordinal);

            foreach (var productionId in productionIds)
            {
                var decision = evaluation.Decisions.Single(value => value.RecordId == productionId);
                Assert.AreEqual(AccessDisposition.Deny, decision.Disposition);
                Assert.AreEqual(AccessReason.ProductionAuthorityExcluded, decision.Reason);
                Assert.IsFalse(projectedIds.Contains(productionId.Value));
            }

            foreach (var chronologyId in fixture.Chronology)
            {
                Assert.IsFalse(projectedIds.Contains(chronologyId.Value));
            }
        }
    }

    [TestMethod]
    public void SceneStateAndPressure_AreSharedWithEveryMissingRaftCharacter()
    {
        var fixture = LoadMissingRaft();
        var expectedSceneState = fixture.SceneState.Select(record => record.Id.Value).ToArray();
        var expectedPressures = fixture.Pressures.Select(record => record.Id.Value).ToArray();

        foreach (var character in fixture.Characters)
        {
            var projection = CharacterBoundedAccessControl.Evaluate(fixture, character.Id).Projection;

            AssertEquivalent(
                expectedSceneState,
                projection.SceneState.Select(record => record.RecordId.Value));
            AssertEquivalent(
                expectedPressures,
                projection.Pressures.Select(record => record.RecordId.Value));
        }
    }

    [TestMethod]
    public void OtherCharacterPrivateRecordsAndInboundRelationships_AreDenied()
    {
        var fixture = LoadMissingRaft();

        foreach (var subject in fixture.Characters)
        {
            var evaluation = CharacterBoundedAccessControl.Evaluate(fixture, subject.Id);
            var projectedIds = ProjectionRecordIds(evaluation.Projection).ToHashSet(StringComparer.Ordinal);
            var otherOwnedIds = fixture.Characters
                .Where(character => character.Id != subject.Id)
                .SelectMany(CharacterOwnedRecordIds)
                .ToArray();

            foreach (var recordId in otherOwnedIds)
            {
                var decision = evaluation.Decisions.Single(value => value.RecordId == recordId);
                Assert.AreEqual(AccessDisposition.Deny, decision.Disposition);
                Assert.AreEqual(AccessReason.OwnedByOtherCharacterExcluded, decision.Reason);
                Assert.IsFalse(projectedIds.Contains(recordId.Value));
            }
        }
    }

    [TestMethod]
    public void CharacterFacingProjectionTypes_CannotCarryProvenance()
    {
        Assert.IsNull(typeof(PermittedRecord).GetProperty("Provenance"));
        Assert.IsNull(typeof(PermittedRelationship).GetProperty("Provenance"));
        Assert.IsNull(typeof(SceneParticipant).GetProperty("Provenance"));
        Assert.IsNull(typeof(AccessDecision).GetProperty("Text"));
        Assert.IsNull(typeof(AccessDecision).GetProperty("Provenance"));
    }

    [TestMethod]
    public void ProvenanceSources_DoNotBecomeAccessibleThroughPermittedRecords()
    {
        var fixture = LoadMissingRaft();

        var voss = CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.VossId).Projection;
        var vossIds = ProjectionRecordIds(voss).ToHashSet(StringComparer.Ordinal);
        Assert.IsTrue(vossIds.Contains(MissingRaftContract.KnowVossCurrentStrengthenedId));
        Assert.IsFalse(vossIds.Contains(MissingRaftContract.WorldCurrentStrengthenedId));

        var wren = CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.WrenId).Projection;
        var wrenIds = ProjectionRecordIds(wren).ToHashSet(StringComparer.Ordinal);
        Assert.IsTrue(wrenIds.Contains(MissingRaftContract.KnowWrenSecuredRaftId));
        Assert.IsFalse(wrenIds.Contains(MissingRaftContract.HtWrenSecuredRaftId));

        foreach (var character in fixture.Characters)
        {
            var projection = CharacterBoundedAccessControl.Evaluate(fixture, character.Id).Projection;
            var ids = ProjectionRecordIds(projection).ToHashSet(StringComparer.Ordinal);
            Assert.IsTrue(ids.Contains(MissingRaftContract.SceneRaftGoneId));
            Assert.IsFalse(ids.Contains(MissingRaftContract.HtCurrentCarriedRaftAwayId));
        }
    }

    [TestMethod]
    public void MissingRaftEpistemicAsymmetry_IsPreservedAtAccessBoundary()
    {
        var fixture = LoadMissingRaft();

        var marloweIds = ProjectionRecordIds(
            CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.MarloweId).Projection)
            .ToHashSet(StringComparer.Ordinal);
        Assert.IsTrue(marloweIds.Contains(MissingRaftContract.KnowMarloweReleasedRaftId));
        Assert.IsFalse(marloweIds.Contains(MissingRaftContract.ObsWrenMarloweReturnId));
        Assert.IsFalse(marloweIds.Contains(MissingRaftContract.SuspWrenMarloweKnowsMoreId));
        Assert.IsFalse(marloweIds.Contains(MissingRaftContract.HtMarloweReleasedRaftId));

        var vossIds = ProjectionRecordIds(
            CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.VossId).Projection)
            .ToHashSet(StringComparer.Ordinal);
        Assert.IsFalse(vossIds.Contains(MissingRaftContract.KnowMarloweReleasedRaftId));
        Assert.IsFalse(vossIds.Contains(MissingRaftContract.HtMarloweReleasedRaftId));

        var wrenIds = ProjectionRecordIds(
            CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.WrenId).Projection)
            .ToHashSet(StringComparer.Ordinal);
        Assert.IsFalse(wrenIds.Contains(MissingRaftContract.KnowMarloweReleasedRaftId));
        Assert.IsFalse(wrenIds.Contains(MissingRaftContract.HtMarloweReleasedRaftId));
        Assert.IsTrue(wrenIds.Contains(MissingRaftContract.SuspWrenMarloweKnowsMoreId));
    }

    [TestMethod]
    public void Decisions_CoverEveryFixtureRecordExactlyOnceAndAreOrdinallySorted()
    {
        var fixture = LoadMissingRaft();
        var evaluation = CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.VossId);
        var expectedIds = AllFixtureRecordIds(fixture)
            .Select(id => id.Value)
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();
        var actualIds = evaluation.Decisions
            .Select(decision => decision.RecordId.Value)
            .ToArray();
        var expectedDistinctCount = actualIds.Length;
        var actualDistinctCount = actualIds.Distinct(StringComparer.Ordinal).Count();

        CollectionAssert.AreEqual(expectedIds, actualIds);
        Assert.AreEqual(expectedDistinctCount, actualDistinctCount);
    }

    [TestMethod]
    public void SourceReordering_DoesNotChangeProjectionOrDecisionOrder()
    {
        var canonical = LoadMissingRaft();
        var reorderedJson = MutateMissingRaft(root =>
        {
            Reverse(root["characters"]!.AsArray());
            Reverse(root["scene"]!.AsObject()["roster"]!.AsArray());
            Reverse(root["historicalTruth"]!.AsArray());
            Reverse(root["sceneState"]!.AsArray());
            Reverse(root["pressures"]!.AsArray());
            Reverse(Character(root, MissingRaftContract.MarloweCharacterId)["relationships"]!.AsArray());
            Reverse(Character(root, MissingRaftContract.MarloweCharacterId)["beliefs"]!.AsArray());
            Reverse(CharacterRecord(
                root,
                MissingRaftContract.WrenCharacterId,
                "suspicions",
                MissingRaftContract.SuspWrenMarloweKnowsMoreId)["provenance"]!.AsArray());
        });
        var reordered = ValidateMissingRaft(reorderedJson);

        foreach (var characterId in new[]
                 {
                     MissingRaftContract.MarloweCharacterId,
                     MissingRaftContract.VossCharacterId,
                     MissingRaftContract.WrenCharacterId
                 })
        {
            var id = CharacterId.From(characterId);
            var canonicalEvaluation = CharacterBoundedAccessControl.Evaluate(canonical, id);
            var reorderedEvaluation = CharacterBoundedAccessControl.Evaluate(reordered, id);
            var expectedProjectionSignature = ProjectionSignature(canonicalEvaluation.Projection);
            var actualProjectionSignature = ProjectionSignature(reorderedEvaluation.Projection);
            var expectedDecisionSignatures = canonicalEvaluation.Decisions
                .Select(DecisionSignature)
                .ToArray();
            var actualDecisionSignatures = reorderedEvaluation.Decisions
                .Select(DecisionSignature)
                .ToArray();

            Assert.AreEqual(expectedProjectionSignature, actualProjectionSignature);
            CollectionAssert.AreEqual(expectedDecisionSignatures, actualDecisionSignatures);
        }
    }

    [TestMethod]
    public void Evaluation_DoesNotMutateFixtureOrChangeFrozenHash()
    {
        var fixture = LoadMissingRaft();
        var expectedHash = FixtureHash.Compute(fixture);

        _ = CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.MarloweId);
        _ = CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.VossId);
        _ = CharacterBoundedAccessControl.Evaluate(fixture, MissingRaftContract.WrenId);

        var actualHash = FixtureHash.Compute(fixture);
        Assert.AreEqual(expectedHash, actualHash);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, actualHash);
    }

    [TestMethod]
    public void UnknownCharacter_FailsClosed()
    {
        var fixture = LoadMissingRaft();

        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(fixture, CharacterId.From("UNKNOWN-CHARACTER")));
    }

    private static IEnumerable<string> ExpectedMissingRaftPermittedIds(string characterId)
    {
        var shared = new[]
        {
            MissingRaftContract.SceneRaftGoneId,
            MissingRaftContract.SceneProvisionsLimitedId,
            MissingRaftContract.SceneNoImmediateEmergencyId,
            MissingRaftContract.SceneNoExternalCountdownId,
            MissingRaftContract.PressureIsolationId
        };

        var owned = characterId switch
        {
            MissingRaftContract.MarloweCharacterId => new[]
            {
                MissingRaftContract.ConMarloweId,
                MissingRaftContract.DispMarloweId,
                MissingRaftContract.ObsMarloweRaftDeteriorationId,
                MissingRaftContract.ObsMarloweNobodyVisibleId,
                MissingRaftContract.KnowMarloweReleasedRaftId,
                MissingRaftContract.KnowMarloweNotWarnedId,
                MissingRaftContract.BelMarloweDamageUnacceptableId,
                MissingRaftContract.BelMarloweGroupLikelyProceedId,
                MissingRaftContract.BelMarloweDisclosureRiskId,
                MissingRaftContract.MemMarloweRh001Id,
                MissingRaftContract.MemMarloweRh002Id,
                MissingRaftContract.GoalMarloweId,
                MissingRaftContract.RelMarloweVossId,
                MissingRaftContract.RelMarloweWrenId
            },
            MissingRaftContract.VossCharacterId => new[]
            {
                MissingRaftContract.ConVossId,
                MissingRaftContract.DispVossId,
                MissingRaftContract.KnowVossCurrentStrengthenedId,
                MissingRaftContract.BelVossAccidentalLossPlausibleId,
                MissingRaftContract.MemVossRh001Id,
                MissingRaftContract.MemVossRh002Id,
                MissingRaftContract.GoalVossId,
                MissingRaftContract.RelVossMarloweId,
                MissingRaftContract.RelVossWrenId
            },
            MissingRaftContract.WrenCharacterId => new[]
            {
                MissingRaftContract.ConWrenId,
                MissingRaftContract.DispWrenId,
                MissingRaftContract.ObsWrenMarloweReturnId,
                MissingRaftContract.KnowWrenSecuredRaftId,
                MissingRaftContract.SuspWrenMarloweKnowsMoreId,
                MissingRaftContract.MemWrenRh002Id,
                MissingRaftContract.GoalWrenId,
                MissingRaftContract.RelWrenMarloweId,
                MissingRaftContract.RelWrenVossId
            },
            _ => throw new InvalidOperationException($"Unknown Missing Raft Character '{characterId}'.")
        };

        return shared.Concat(owned);
    }

    private static IEnumerable<string> ProjectionRecordIds(CharacterAccessProjection projection) =>
        projection.SceneState.Select(record => record.RecordId.Value)
            .Concat(projection.Pressures.Select(record => record.RecordId.Value))
            .Concat(projection.Constitution.Select(record => record.RecordId.Value))
            .Concat(projection.Disposition.Select(record => record.RecordId.Value))
            .Concat(projection.Circumstance.Select(record => record.RecordId.Value))
            .Concat(projection.Observations.Select(record => record.RecordId.Value))
            .Concat(projection.Knowledge.Select(record => record.RecordId.Value))
            .Concat(projection.Beliefs.Select(record => record.RecordId.Value))
            .Concat(projection.Suspicions.Select(record => record.RecordId.Value))
            .Concat(projection.Memories.Select(record => record.RecordId.Value))
            .Concat(projection.Goals.Select(record => record.RecordId.Value))
            .Concat(projection.Relationships.Select(record => record.RecordId.Value));

    private static IEnumerable<RecordId> CharacterOwnedRecordIds(ValidatedCharacter character) =>
        character.Constitution.Select(record => record.Id)
            .Concat(character.Disposition.Select(record => record.Id))
            .Concat(character.Circumstance.Select(record => record.Id))
            .Concat(character.Observations.Select(record => record.Id))
            .Concat(character.Knowledge.Select(record => record.Id))
            .Concat(character.Beliefs.Select(record => record.Id))
            .Concat(character.Suspicions.Select(record => record.Id))
            .Concat(character.Memories.Select(record => record.Id))
            .Concat(character.Goals.Select(record => record.Id))
            .Concat(character.Relationships.Select(record => record.Id));

    private static IEnumerable<RecordId> AllFixtureRecordIds(ValidatedFixture fixture) =>
        fixture.HistoricalTruth.Select(record => record.Id)
            .Concat(fixture.UnresolvedPropositions.Select(record => record.Id))
            .Concat(fixture.WorldState.Select(record => record.Id))
            .Concat(fixture.SceneState.Select(record => record.Id))
            .Concat(fixture.Pressures.Select(record => record.Id))
            .Concat(fixture.Characters.SelectMany(CharacterOwnedRecordIds));

    private static string ProjectionSignature(CharacterAccessProjection projection) => string.Join(
        "|",
        projection.Roster.Select(participant => $"R:{participant.CharacterId.Value}:{participant.DisplayName}")
            .Concat(projection.SceneState.Select(record => $"S:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Pressures.Select(record => $"P:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Constitution.Select(record => $"C:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Disposition.Select(record => $"D:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Circumstance.Select(record => $"I:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Observations.Select(record => $"O:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Knowledge.Select(record => $"K:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Beliefs.Select(record => $"B:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Suspicions.Select(record => $"U:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Memories.Select(record => $"M:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Goals.Select(record => $"G:{record.RecordId.Value}:{record.Text}"))
            .Concat(projection.Relationships.Select(record =>
                $"L:{record.RecordId.Value}:{record.TargetCharacterId.Value}:{record.Text}")));

    private static string DecisionSignature(AccessDecision decision) =>
        $"{decision.RecordId.Value}:{decision.Disposition}:{decision.Reason}";

    private static void AssertEquivalent(IEnumerable<string> expected, IEnumerable<string> actual)
    {
        var expectedValues = expected.OrderBy(value => value, StringComparer.Ordinal).ToArray();
        var actualValues = actual.OrderBy(value => value, StringComparer.Ordinal).ToArray();
        CollectionAssert.AreEqual(expectedValues, actualValues);
    }

    private static ValidatedFixture LoadMissingRaft() =>
        ValidateMissingRaft(ReadFixture("missing-raft-0.1.0.json"));

    private static ValidatedFixture ValidateMissingRaft(string json)
    {
        var fixture = Validate(json);
        MissingRaftContract.Validate(fixture);
        return fixture;
    }

    private static ValidatedFixture LoadFixture(string fileName) => Validate(ReadFixture(fileName));

    private static ValidatedFixture Validate(string json)
    {
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        return GenericE0FixtureValidator.Validate(document);
    }

    private static string ReadFixture(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName);
        return File.ReadAllText(path, Encoding.UTF8);
    }

    private static string MutateMissingRaft(Action<JsonObject> mutation)
    {
        var root = JsonNode.Parse(ReadFixture("missing-raft-0.1.0.json"))?.AsObject()
            ?? throw new InvalidOperationException("Missing Raft fixture did not parse as a JSON object.");

        mutation(root);
        return root.ToJsonString();
    }

    private static JsonObject Character(JsonObject root, string characterId) =>
        root["characters"]!.AsArray()
            .Select(node => node!.AsObject())
            .Single(character => character["id"]!.GetValue<string>() == characterId);

    private static JsonObject CharacterRecord(
        JsonObject root,
        string characterId,
        string collection,
        string recordId) =>
        Character(root, characterId)[collection]!.AsArray()
            .Select(node => node!.AsObject())
            .Single(record => record["id"]!.GetValue<string>() == recordId);

    private static void Reverse(JsonArray array)
    {
        for (var left = 0; left < array.Count / 2; left++)
        {
            var right = array.Count - 1 - left;
            var leftValue = array[left]!.DeepClone();
            var rightValue = array[right]!.DeepClone();
            array[left] = rightValue;
            array[right] = leftValue;
        }
    }
}
