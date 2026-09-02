using System.Text;
using System.Text.Json.Nodes;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Fixture;

[TestClass]
public sealed class MissingRaftContractTests
{
    [TestMethod]
    public void CanonicalMissingRaftFixture_PassesGenericAndMissingRaftValidation()
    {
        var fixture = ValidateGeneric(ReadMissingRaftFixture());

        MissingRaftContract.Validate(fixture);

        Assert.AreEqual(MissingRaftContract.FamilyId, fixture.FamilyId);
        Assert.AreEqual(MissingRaftContract.Version, fixture.Version);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureId, fixture.Id);
    }

    [TestMethod]
    public void GenericSmokeFixture_RemainsGenericButFailsExplicitMissingRaftValidation()
    {
        var fixture = ValidateGeneric(ReadFixture("e0-fixture-v1.json"));

        Assert.Throws<FixtureValidationException>(() => MissingRaftContract.Validate(fixture));
    }

    [TestMethod]
    public void WrongFixtureFamily_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        root["fixture"]!.AsObject()["id"] = JsonValue.Create("ensemble.e0.other")));

    [TestMethod]
    public void WrongFixtureVersion_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        root["fixture"]!.AsObject()["version"] = JsonValue.Create("0.1.1")));

    [TestMethod]
    public void WrongSceneId_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        root["scene"]!.AsObject()["id"] = JsonValue.Create("SCENE-OTHER")));

    [TestMethod]
    public void WrongDisplayName_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        Character(root, MissingRaftContract.VossCharacterId)["displayName"] = JsonValue.Create("Voss")));

    [TestMethod]
    public void WrongCharacterSet_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
    {
        const string replacementId = "WREN-OTHER";
        Character(root, MissingRaftContract.WrenCharacterId)["id"] = JsonValue.Create(replacementId);

        var roster = root["scene"]!.AsObject()["roster"]!.AsArray();
        ReplaceArrayValue(roster, MissingRaftContract.WrenCharacterId, replacementId);

        foreach (var characterNode in root["characters"]!.AsArray())
        {
            var relationships = characterNode!.AsObject()["relationships"]!.AsArray();
            foreach (var relationshipNode in relationships)
            {
                var relationship = relationshipNode!.AsObject();
                if (relationship["targetCharacterId"]!.GetValue<string>() == MissingRaftContract.WrenCharacterId)
                {
                    relationship["targetCharacterId"] = JsonValue.Create(replacementId);
                }
            }
        }
    }));

    [TestMethod]
    public void WrongInitialOpportunity_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        root["initialOpportunity"] = JsonValue.Create(MissingRaftContract.MarloweCharacterId)));

    [TestMethod]
    public void ProductionCategoryMove_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        MoveRecord(
            root["unresolvedPropositions"]!.AsArray(),
            root["historicalTruth"]!.AsArray(),
            MissingRaftContract.UpRaftWouldFailId)));

    [TestMethod]
    public void ProductionRecordAddition_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        root["worldState"]!.AsArray().Add(NewRecord("WORLD-EXTRA"))));

    [TestMethod]
    public void ProductionRecordRemoval_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        RemoveRecord(root["sceneState"]!.AsArray(), MissingRaftContract.SceneNoImmediateEmergencyId)));

    [TestMethod]
    public void CharacterOwnerMove_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        MoveRecord(
            Character(root, MissingRaftContract.MarloweCharacterId)["beliefs"]!.AsArray(),
            Character(root, MissingRaftContract.VossCharacterId)["beliefs"]!.AsArray(),
            MissingRaftContract.BelMarloweGroupLikelyProceedId)));

    [TestMethod]
    public void CharacterCategoryMove_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        MoveRecord(
            Character(root, MissingRaftContract.MarloweCharacterId)["knowledge"]!.AsArray(),
            Character(root, MissingRaftContract.MarloweCharacterId)["beliefs"]!.AsArray(),
            MissingRaftContract.KnowMarloweNotWarnedId)));

    [TestMethod]
    public void CharacterRecordAddition_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        Character(root, MissingRaftContract.WrenCharacterId)["beliefs"]!.AsArray().Add(NewRecord("BEL-WREN-EXTRA"))));

    [TestMethod]
    public void CharacterRecordRemoval_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        RemoveRecord(
            Character(root, MissingRaftContract.MarloweCharacterId)["beliefs"]!.AsArray(),
            MissingRaftContract.BelMarloweDisclosureRiskId)));

    [TestMethod]
    public void ChronologyReorder_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
    {
        var chronology = root["chronology"]!.AsArray();
        var first = chronology[0]!.GetValue<string>();
        var second = chronology[1]!.GetValue<string>();
        chronology[0] = JsonValue.Create(second);
        chronology[1] = JsonValue.Create(first);
    }));

    [TestMethod]
    public void ChronologyRemoval_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        root["chronology"]!.AsArray().RemoveAt(0)));

    [TestMethod]
    public void ChronologyAddition_IsRejectedByMissingRaftContract() => AssertContractInvalid(Mutate(root =>
        root["chronology"]!.AsArray().Add(JsonValue.Create(MissingRaftContract.HtMarloweNotWarnedId))));

    [TestMethod]
    [DataRow(MissingRaftContract.UpRaftWouldFailId)]
    [DataRow(MissingRaftContract.UpRh001CacheWouldBeLostId)]
    public void UnresolvedPropositionMovedToHistoricalTruth_IsRejected(string recordId) => AssertContractInvalid(Mutate(root =>
        MoveRecord(root["unresolvedPropositions"]!.AsArray(), root["historicalTruth"]!.AsArray(), recordId)));

    [TestMethod]
    public void WrenObservationProvenanceChangedFromReturnToRelease_IsRejected() => AssertContractInvalid(Mutate(root =>
        SetProvenance(
            CharacterRecord(root, MissingRaftContract.WrenCharacterId, "observations", MissingRaftContract.ObsWrenMarloweReturnId),
            MissingRaftContract.HtMarloweReleasedRaftId)));

    [TestMethod]
    public void WrenSuspicionGivenHiddenReleaseProvenance_IsRejected() => AssertContractInvalid(Mutate(root =>
        AddProvenance(
            CharacterRecord(root, MissingRaftContract.WrenCharacterId, "suspicions", MissingRaftContract.SuspWrenMarloweKnowsMoreId),
            MissingRaftContract.HtMarloweReleasedRaftId)));

    [TestMethod]
    public void VossBeliefGivenHiddenReleaseProvenance_IsRejected() => AssertContractInvalid(Mutate(root =>
        AddProvenance(
            CharacterRecord(root, MissingRaftContract.VossCharacterId, "beliefs", MissingRaftContract.BelVossAccidentalLossPlausibleId),
            MissingRaftContract.HtMarloweReleasedRaftId)));

    [TestMethod]
    public void MarloweReleaseKnowledgeRemoval_IsRejected() => AssertContractInvalid(Mutate(root =>
        RemoveRecord(
            Character(root, MissingRaftContract.MarloweCharacterId)["knowledge"]!.AsArray(),
            MissingRaftContract.KnowMarloweReleasedRaftId)));

    [TestMethod]
    public void MarloweNotWarnedKnowledgeReassignment_IsRejected() => AssertContractInvalid(Mutate(root =>
        MoveRecord(
            Character(root, MissingRaftContract.MarloweCharacterId)["knowledge"]!.AsArray(),
            Character(root, MissingRaftContract.WrenCharacterId)["knowledge"]!.AsArray(),
            MissingRaftContract.KnowMarloweNotWarnedId)));

    [TestMethod]
    public void RelationshipOwnerMismatch_IsRejected() => AssertContractInvalid(Mutate(root =>
        MoveRecord(
            Character(root, MissingRaftContract.MarloweCharacterId)["relationships"]!.AsArray(),
            Character(root, MissingRaftContract.WrenCharacterId)["relationships"]!.AsArray(),
            MissingRaftContract.RelMarloweVossId)));

    [TestMethod]
    public void RelationshipTargetMismatch_IsRejected() => AssertContractInvalid(Mutate(root =>
        Relationship(root, MissingRaftContract.MarloweCharacterId, MissingRaftContract.RelMarloweVossId)["targetCharacterId"] =
            JsonValue.Create(MissingRaftContract.WrenCharacterId)));

    [TestMethod]
    public void RelationshipHistoryProvenanceRemoval_IsRejected() => AssertContractInvalid(Mutate(root =>
        SetProvenance(Relationship(root, MissingRaftContract.MarloweCharacterId, MissingRaftContract.RelMarloweVossId))));

    [TestMethod]
    public void RelationshipHistoryProvenanceChange_IsRejected() => AssertContractInvalid(Mutate(root =>
        SetProvenance(
            Relationship(root, MissingRaftContract.WrenCharacterId, MissingRaftContract.RelWrenVossId),
            MissingRaftContract.Rh001Id)));

    [TestMethod]
    public void RelationshipMemoryProvenanceChange_IsRejected() => AssertContractInvalid(Mutate(root =>
        SetProvenance(
            CharacterRecord(root, MissingRaftContract.WrenCharacterId, "memories", MissingRaftContract.MemWrenRh002Id),
            MissingRaftContract.Rh001Id)));

    [TestMethod]
    public void RelationshipContextRecordIds_AreExactlyDerivedFromComponents()
    {
        var derived = MissingRaftContract.RelationshipHistoryRecordIds
            .Union(MissingRaftContract.RelationshipRecordIds)
            .Union(MissingRaftContract.RelationshipDerivedMemoryRecordIds)
            .ToHashSet();

        Assert.IsTrue(MissingRaftContract.RelationshipContextRecordIds.SetEquals(derived));
    }

    private static JsonObject Character(JsonObject root, string characterId) =>
        root["characters"]!.AsArray()
            .Select(node => node!.AsObject())
            .Single(character => character["id"]!.GetValue<string>() == characterId);

    private static JsonObject CharacterRecord(JsonObject root, string characterId, string collection, string recordId) =>
        Record(Character(root, characterId)[collection]!.AsArray(), recordId);

    private static JsonObject Relationship(JsonObject root, string characterId, string relationshipId) =>
        Record(Character(root, characterId)["relationships"]!.AsArray(), relationshipId);

    private static JsonObject Record(JsonArray records, string recordId) =>
        records.Select(node => node!.AsObject())
            .Single(record => record["id"]!.GetValue<string>() == recordId);

    private static void MoveRecord(JsonArray source, JsonArray destination, string recordId)
    {
        var record = Record(source, recordId);
        source.Remove(record);
        destination.Add(record);
    }

    private static void RemoveRecord(JsonArray records, string recordId) =>
        records.Remove(Record(records, recordId));

    private static void SetProvenance(JsonObject record, params string[] provenanceIds)
    {
        var provenance = new JsonArray();
        foreach (var provenanceId in provenanceIds)
        {
            provenance.Add(JsonValue.Create(provenanceId));
        }

        record["provenance"] = provenance;
    }

    private static void AddProvenance(JsonObject record, string provenanceId) =>
        record["provenance"]!.AsArray().Add(JsonValue.Create(provenanceId));

    private static JsonObject NewRecord(string id) => new()
    {
        ["id"] = id,
        ["text"] = "Contract mutation probe.",
        ["provenance"] = new JsonArray()
    };

    private static void ReplaceArrayValue(JsonArray array, string oldValue, string newValue)
    {
        for (var index = 0; index < array.Count; index++)
        {
            if (array[index]!.GetValue<string>() == oldValue)
            {
                array[index] = JsonValue.Create(newValue);
            }
        }
    }

    private static void AssertContractInvalid(string json)
    {
        var fixture = ValidateGeneric(json);
        Assert.Throws<FixtureValidationException>(() => MissingRaftContract.Validate(fixture));
    }

    private static ValidatedFixture ValidateGeneric(string json)
    {
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        return GenericE0FixtureValidator.Validate(document);
    }

    private static string Mutate(Action<JsonObject> mutation)
    {
        var root = JsonNode.Parse(ReadMissingRaftFixture())?.AsObject()
            ?? throw new InvalidOperationException("Canonical Missing Raft fixture did not parse as a JSON object.");

        mutation(root);
        return root.ToJsonString();
    }

    private static string ReadMissingRaftFixture() => ReadFixture("missing-raft-0.1.0.json");

    private static string ReadFixture(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName);
        return File.ReadAllText(path, Encoding.UTF8);
    }
}
