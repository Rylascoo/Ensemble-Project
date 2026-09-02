using System.Text;
using System.Text.Json.Nodes;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Access;

[TestClass]
public sealed class CharacterBoundedAccessControlBoundaryTests
{
    [TestMethod]
    public void GenericFixture_AllSubjectOwnedCategoriesArePermitted()
    {
        var root = JsonNode.Parse(ReadFixture("e0-fixture-v1.json"))?.AsObject()
            ?? throw new InvalidOperationException("Smoke fixture did not parse as a JSON object.");
        var subject = Character(root, "CHAR-A");

        AddRecord(subject, "disposition", "DISP-A", "A disposition.");
        AddRecord(subject, "circumstance", "CIRC-A", "A circumstance.");
        AddRecord(subject, "observations", "OBS-A", "A observation.");
        AddRecord(subject, "knowledge", "KNOW-A", "A knowledge.");
        AddRecord(subject, "beliefs", "BEL-A", "A belief.");
        AddRecord(subject, "suspicions", "SUSP-A", "A suspicion.");
        AddRecord(subject, "memories", "MEM-A", "A memory.");
        AddRecord(subject, "goals", "GOAL-A", "A goal.");

        var fixture = Validate(root.ToJsonString());
        var projection = CharacterBoundedAccessControl.Evaluate(fixture, CharacterId.From("CHAR-A")).Projection;

        Assert.AreEqual("CON-A", projection.Constitution.Single().RecordId.Value);
        Assert.AreEqual("DISP-A", projection.Disposition.Single().RecordId.Value);
        Assert.AreEqual("CIRC-A", projection.Circumstance.Single().RecordId.Value);
        Assert.AreEqual("OBS-A", projection.Observations.Single().RecordId.Value);
        Assert.AreEqual("KNOW-A", projection.Knowledge.Single().RecordId.Value);
        Assert.AreEqual("BEL-A", projection.Beliefs.Single().RecordId.Value);
        Assert.AreEqual("SUSP-A", projection.Suspicions.Single().RecordId.Value);
        Assert.AreEqual("MEM-A", projection.Memories.Single().RecordId.Value);
        Assert.AreEqual("GOAL-A", projection.Goals.Single().RecordId.Value);
        Assert.AreEqual("REL-A-B", projection.Relationships.Single().RecordId.Value);
    }

    [TestMethod]
    public void InvalidAuthorityInputs_FailWithCharacterAccessException()
    {
        ValidatedFixture? missingFixture = null;
        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(missingFixture!, CharacterId.From("CHAR-A")));

        var fixture = Validate(ReadFixture("e0-fixture-v1.json"));
        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(fixture, default));
    }

    private static void AddRecord(JsonObject character, string collection, string id, string text)
    {
        character[collection]!.AsArray().Add(new JsonObject
        {
            ["id"] = JsonValue.Create(id),
            ["text"] = JsonValue.Create(text),
            ["provenance"] = new JsonArray()
        });
    }

    private static JsonObject Character(JsonObject root, string characterId) =>
        root["characters"]!.AsArray()
            .Select(node => node!.AsObject())
            .Single(character => character["id"]!.GetValue<string>() == characterId);

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
}
