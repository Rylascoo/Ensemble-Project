using System.Text;
using System.Text.Json.Nodes;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Fixture;

[TestClass]
public sealed class Ecj1FixtureIdentityTests
{
    private const int ExpectedMissingRaftCanonicalByteLength = 9112;

    [TestMethod]
    public void CanonicalMissingRaft_MatchesIndependentlyReviewedEcj1Identity()
    {
        var fixture = ValidateGeneric(ReadMissingRaftFixture());
        var canonicalBytes = Ecj1FixtureCanonicalizer.Serialize(fixture);

        Assert.AreEqual(ExpectedMissingRaftCanonicalByteLength, canonicalBytes.Length);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, FixtureHash.Compute(fixture));

        MissingRaftContract.Validate(fixture);
    }

    [TestMethod]
    public void RepeatedSerialization_IsByteIdentical()
    {
        var fixture = ValidateGeneric(ReadMissingRaftFixture());

        var first = Ecj1FixtureCanonicalizer.Serialize(fixture);
        var second = Ecj1FixtureCanonicalizer.Serialize(fixture);

        CollectionAssert.AreEqual(first, second);
    }

    [TestMethod]
    public void SourceFormattingAndRootPropertyOrder_DoNotChangeHash()
    {
        var canonical = ValidateGeneric(ReadMissingRaftFixture());
        var reorderedJson = MutateMissingRaft(root =>
        {
            var schemaVersion = root["schemaVersion"]!.DeepClone();
            root.Remove("schemaVersion");
            root["schemaVersion"] = schemaVersion;
        });

        var reordered = ValidateGeneric(reorderedJson);

        Assert.AreEqual(FixtureHash.Compute(canonical), FixtureHash.Compute(reordered));
        MissingRaftContract.Validate(reordered);
    }

    [TestMethod]
    public void SemanticallyUnorderedCollections_DoNotChangeHash()
    {
        var canonical = ValidateGeneric(ReadMissingRaftFixture());
        var reorderedJson = MutateMissingRaft(root =>
        {
            Reverse(root["characters"]!.AsArray());
            Reverse(root["scene"]!.AsObject()["roster"]!.AsArray());
            Reverse(root["historicalTruth"]!.AsArray());
            Reverse(Character(root, MissingRaftContract.MarloweCharacterId)["relationships"]!.AsArray());
            Reverse(CharacterRecord(
                root,
                MissingRaftContract.VossCharacterId,
                "beliefs",
                MissingRaftContract.BelVossAccidentalLossPlausibleId)["provenance"]!.AsArray());
        });

        var reordered = ValidateGeneric(reorderedJson);

        Assert.AreEqual(FixtureHash.Compute(canonical), FixtureHash.Compute(reordered));
        MissingRaftContract.Validate(reordered);
    }

    [TestMethod]
    public void SemanticTextMutation_ChangesHashAndFailsMissingRaftContract()
    {
        var fixture = ValidateGeneric(MutateMissingRaft(root =>
            CharacterRecord(
                root,
                MissingRaftContract.MarloweCharacterId,
                "beliefs",
                MissingRaftContract.BelMarloweDisclosureRiskId)["text"] =
                    JsonValue.Create("Marlowe believes disclosure now carries a different trust risk.")));

        Assert.AreNotEqual(MissingRaftContract.ExpectedFixtureHash, FixtureHash.Compute(fixture));
        Assert.Throws<FixtureValidationException>(() => MissingRaftContract.Validate(fixture));
    }

    [TestMethod]
    public void RelationshipTextMutation_ChangesHashAndFailsMissingRaftContract()
    {
        var fixture = ValidateGeneric(MutateMissingRaft(root =>
            Relationship(
                root,
                MissingRaftContract.WrenCharacterId,
                MissingRaftContract.RelWrenVossId)["text"] =
                    JsonValue.Create("Wren now describes Voss differently while preserving the same structural relationship.")));

        Assert.AreNotEqual(MissingRaftContract.ExpectedFixtureHash, FixtureHash.Compute(fixture));
        Assert.Throws<FixtureValidationException>(() => MissingRaftContract.Validate(fixture));
    }

    [TestMethod]
    public void ProvenanceSourceOrder_IsCanonicalizedAsASet()
    {
        var canonical = ValidateGeneric(ReadMissingRaftFixture());
        var reordered = ValidateGeneric(MutateMissingRaft(root =>
            Reverse(CharacterRecord(
                root,
                MissingRaftContract.WrenCharacterId,
                "suspicions",
                MissingRaftContract.SuspWrenMarloweKnowsMoreId)["provenance"]!.AsArray())));

        Assert.AreEqual(FixtureHash.Compute(canonical), FixtureHash.Compute(reordered));
        MissingRaftContract.Validate(reordered);
    }

    [TestMethod]
    public void CarriageReturnInSemanticText_IsRejectedRatherThanNormalized()
    {
        var json = MutateSmoke(root => Character(root, "CHAR-A")["displayName"] = JsonValue.Create("A\rB"));

        Assert.Throws<FixtureValidationException>(() => ValidateGeneric(json));
    }

    [TestMethod]
    public void UnpairedSurrogateInSemanticText_IsRejectedRatherThanReplacementEncoded()
    {
        var json = ReadSmokeFixture().Replace(
            "\"displayName\": \"A\"",
            "\"displayName\": \"\\uD800\"",
            StringComparison.Ordinal);

        Assert.Throws<FixtureValidationException>(() => ValidateGeneric(json));
    }

    [TestMethod]
    public void Ecj1StringEscaping_UsesTheFrozenByteContract()
    {
        const string semanticText = "Quote \" slash \\ tab\t line\n form\f ctrl\u0001 BMP é nonBMP 😀";
        var fixture = ValidateGeneric(MutateSmoke(root =>
            CharacterRecord(root, "CHAR-A", "constitution", "CON-A")["text"] = JsonValue.Create(semanticText)));

        var canonical = Encoding.UTF8.GetString(Ecj1FixtureCanonicalizer.Serialize(fixture));

        StringAssert.Contains(
            canonical,
            "\"text\":\"Quote \\\" slash \\\\ tab\\t line\\n form\\f ctrl\\u0001 BMP é nonBMP 😀\"");
    }

    [TestMethod]
    public void GenericSmoke_HasDeterministicHashWithoutMissingRaftAuthority()
    {
        var fixture = ValidateGeneric(ReadSmokeFixture());

        var hash = FixtureHash.Compute(fixture);

        Assert.AreEqual(64, hash.Length);
        Assert.AreEqual(hash, hash.ToLowerInvariant());
        Assert.Throws<FixtureValidationException>(() => MissingRaftContract.Validate(fixture));
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

    private static ValidatedFixture ValidateGeneric(string json)
    {
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        return GenericE0FixtureValidator.Validate(document);
    }

    private static string MutateMissingRaft(Action<JsonObject> mutation) => Mutate(ReadMissingRaftFixture(), mutation);

    private static string MutateSmoke(Action<JsonObject> mutation) => Mutate(ReadSmokeFixture(), mutation);

    private static string Mutate(string json, Action<JsonObject> mutation)
    {
        var root = JsonNode.Parse(json)?.AsObject()
            ?? throw new InvalidOperationException("Fixture did not parse as a JSON object.");

        mutation(root);
        return root.ToJsonString();
    }

    private static string ReadMissingRaftFixture() => ReadFixture("missing-raft-0.1.0.json");

    private static string ReadSmokeFixture() => ReadFixture("e0-fixture-v1.json");

    private static string ReadFixture(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName);
        return File.ReadAllText(path, Encoding.UTF8);
    }
}
