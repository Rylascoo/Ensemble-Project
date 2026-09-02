using System.Text;
using System.Text.Json.Nodes;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Fixture;

[TestClass]
public sealed class GenericE0FixtureValidatorTests
{
    [TestMethod]
    public void CanonicalSmokeFixture_ValidatesWithVersionedIdentity()
    {
        var fixture = Validate(ReadCanonicalFixture());

        Assert.AreEqual("ensemble.e0.smoke", fixture.FamilyId.Value);
        Assert.AreEqual("0.1.0", fixture.Version.Value);
        Assert.AreEqual("ensemble.e0.smoke@0.1.0", fixture.Id.Value);
        Assert.AreEqual(3, fixture.Characters.Length);
        Assert.AreEqual(3, fixture.Scene.Roster.Length);
    }

    [TestMethod]
    public void UnknownSchemaVersion_IsRejected() => AssertInvalid(Mutate(root =>
        root["schemaVersion"] = JsonValue.Create("ensemble.e0.fixture.v2")));

    [TestMethod]
    public void UnknownAccessContract_IsRejected() => AssertInvalid(Mutate(root =>
        root["accessContract"] = JsonValue.Create("ensemble.e0.unknown-access.v1")));

    [TestMethod]
    public void UnknownObservationContract_IsRejected() => AssertInvalid(Mutate(root =>
        root["observationContract"] = JsonValue.Create("ensemble.e0.unknown-observation.v1")));

    [TestMethod]
    public void FourthCharacter_IsRejected() => AssertInvalid(Mutate(root =>
    {
        var characters = root["characters"]!.AsArray();
        characters.Add(characters[0]!.DeepClone());
    }));

    [TestMethod]
    public void DuplicateFixtureGlobalRecordId_IsRejected() => AssertInvalid(Mutate(root =>
    {
        var characterB = Character(root, 1);
        characterB["constitution"]!.AsArray()[0]!.AsObject()["id"] = JsonValue.Create("CON-A");
    }));

    [TestMethod]
    public void UnknownRelationshipTarget_IsRejected() => AssertInvalid(Mutate(root =>
    {
        var relationship = Character(root, 0)["relationships"]!.AsArray()[0]!.AsObject();
        relationship["targetCharacterId"] = JsonValue.Create("CHAR-X");
    }));

    [TestMethod]
    public void SelfRelationship_IsRejected() => AssertInvalid(Mutate(root =>
    {
        var relationship = Character(root, 0)["relationships"]!.AsArray()[0]!.AsObject();
        relationship["targetCharacterId"] = JsonValue.Create("CHAR-A");
    }));

    [TestMethod]
    public void UnknownProvenanceRecord_IsRejected() => AssertInvalid(Mutate(root =>
    {
        var pressure = root["pressures"]!.AsArray()[0]!.AsObject();
        pressure["provenance"]!.AsArray()[0] = JsonValue.Create("UNKNOWN-RECORD");
    }));

    [TestMethod]
    public void SelfProvenance_IsRejected() => AssertInvalid(Mutate(root =>
    {
        var pressure = root["pressures"]!.AsArray()[0]!.AsObject();
        pressure["provenance"]!.AsArray()[0] = JsonValue.Create("PRESSURE-001");
    }));

    [TestMethod]
    public void MultiRecordProvenanceCycle_IsRejected() => AssertInvalid(Mutate(root =>
    {
        var constitution = Character(root, 0)["constitution"]!.AsArray()[0]!.AsObject();
        var relationship = Character(root, 0)["relationships"]!.AsArray()[0]!.AsObject();
        constitution["provenance"] = new JsonArray(JsonValue.Create("REL-A-B"));
        relationship["provenance"] = new JsonArray(JsonValue.Create("CON-A"));
    }));

    [TestMethod]
    public void ChronologyReferenceOutsideHistoricalTruth_IsRejected() => AssertInvalid(Mutate(root =>
        root["chronology"]!.AsArray()[0] = JsonValue.Create("PRESSURE-001")));

    [TestMethod]
    public void InitialOpportunityOutsideRoster_IsRejected() => AssertInvalid(Mutate(root =>
        root["initialOpportunity"] = JsonValue.Create("CHAR-X")));

    [TestMethod]
    public void SceneRosterOutsideCharacterSet_IsRejected() => AssertInvalid(Mutate(root =>
        root["scene"]!.AsObject()["roster"]!.AsArray()[2] = JsonValue.Create("CHAR-X")));

    [TestMethod]
    public void MissingRequiredCharacterCollection_IsRejected() => AssertInvalid(Mutate(root =>
        Character(root, 0).Remove("beliefs")));

    [TestMethod]
    public void NonNfcSemanticText_IsRejected() => AssertInvalid(Mutate(root =>
        Character(root, 0)["displayName"] = JsonValue.Create("e\u0301")));

    [TestMethod]
    public void FixtureAuthoredAccessPolicies_AreRejected() => AssertInvalid(Mutate(root =>
        root["accessPolicies"] = new JsonObject()));

    [TestMethod]
    public void JsonNumberToken_IsRejectedBeforeSchemaValidation()
    {
        var json = Mutate(root => root["numberProbe"] = JsonValue.Create(1));

        Assert.Throws<FixtureValidationException>(() => FixtureLoader.Load(Encoding.UTF8.GetBytes(json)));
    }

    [TestMethod]
    public void DuplicateJsonProperty_IsRejectedBeforeDeserialization()
    {
        const string json = "{\"schemaVersion\":\"ensemble.e0.fixture.v1\",\"schemaVersion\":\"ensemble.e0.fixture.v1\"}";

        Assert.Throws<FixtureValidationException>(() => FixtureLoader.Load(Encoding.UTF8.GetBytes(json)));
    }

    private static JsonObject Character(JsonObject root, int index) =>
        root["characters"]!.AsArray()[index]!.AsObject();

    private static void AssertInvalid(string json) =>
        Assert.Throws<FixtureValidationException>(() => Validate(json));

    private static ValidatedFixture Validate(string json)
    {
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        return GenericE0FixtureValidator.Validate(document);
    }

    private static string Mutate(Action<JsonObject> mutation)
    {
        var root = JsonNode.Parse(ReadCanonicalFixture())?.AsObject()
            ?? throw new InvalidOperationException("Canonical smoke fixture did not parse as a JSON object.");

        mutation(root);
        return root.ToJsonString();
    }

    private static string ReadCanonicalFixture()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "e0-fixture-v1.json");
        return File.ReadAllText(path, Encoding.UTF8);
    }
}
