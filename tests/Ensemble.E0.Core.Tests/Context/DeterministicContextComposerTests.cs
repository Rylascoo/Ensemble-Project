using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Context;

[TestClass]
public sealed class DeterministicContextComposerTests
{
    private const string ExpectedVossStructuredHash =
        "bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b";
    private const string ExpectedVossRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const int ExpectedVossStructuredByteLength = 2569;
    private const int ExpectedVossRenderedEnvelopeByteLength = 1905;

    [TestMethod]
    public void MissingRaftVoss_MatchesIndependentReferenceHashesAndByteLengths()
    {
        var fixture = LoadMissingRaft();
        var evaluation = Compose(fixture, MissingRaftContract.VossId);
        var structuredBytes = ContextPacketCanonicalizer.SerializeStructured(evaluation.Packet);
        var renderedBytes = ContextPacketCanonicalizer.SerializeRendered(evaluation.Packet.Rendered);

        Assert.AreEqual(ExpectedVossStructuredHash, evaluation.Packet.StructuredContextHash);
        Assert.AreEqual(ExpectedVossRenderedHash, evaluation.Packet.RenderedContextHash);
        Assert.AreEqual(
            $"CTX:{ExpectedVossStructuredHash}",
            evaluation.Packet.ContextPacketId.Value);
        Assert.AreEqual(ExpectedVossStructuredByteLength, structuredBytes.Length);
        Assert.AreEqual(ExpectedVossRenderedEnvelopeByteLength, renderedBytes.Length);
        Assert.AreEqual(ExpectedVossStructuredHash, Sha256Lower(structuredBytes));
        Assert.AreEqual(ExpectedVossRenderedHash, Sha256Lower(renderedBytes));
    }

    [TestMethod]
    [DataRow(MissingRaftContract.MarloweCharacterId)]
    [DataRow(MissingRaftContract.VossCharacterId)]
    [DataRow(MissingRaftContract.WrenCharacterId)]
    public void MissingRaft_AllCharactersPreserveExactAccessRecordSet(string characterId)
    {
        var fixture = LoadMissingRaft();
        var id = CharacterId.From(characterId);
        var access = CharacterBoundedAccessControl.Evaluate(fixture, id).Projection;
        var context = DeterministicContextComposer.Compose(access, id).Packet;

        CollectionAssert.AreEqual(
            AccessRecordIds(access).OrderBy(value => value, StringComparer.Ordinal).ToArray(),
            PacketRecordIds(context).OrderBy(value => value, StringComparer.Ordinal).ToArray());
    }

    [TestMethod]
    public void MissingRaft_DeniedProductionAndOtherCharacterIdsNeverEnterPacketRenderOrTrace()
    {
        var fixture = LoadMissingRaft();
        var evaluation = Compose(fixture, MissingRaftContract.VossId);
        var packetIds = PacketRecordIds(evaluation.Packet).ToHashSet(StringComparer.Ordinal);
        var traceIds = evaluation.Trace.IncludedRecordIds
            .Select(id => id.Value)
            .ToHashSet(StringComparer.Ordinal);
        var rendered = evaluation.Packet.Rendered.TrustedStateText;

        var denied = fixture.HistoricalTruth
            .Concat(fixture.UnresolvedPropositions)
            .Concat(fixture.WorldState)
            .Select(record => record.Id.Value)
            .Concat(fixture.Characters
                .Where(character => character.Id != MissingRaftContract.VossId)
                .SelectMany(CharacterOwnedIds))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        foreach (var deniedId in denied)
        {
            Assert.IsFalse(packetIds.Contains(deniedId), deniedId);
            Assert.IsFalse(traceIds.Contains(deniedId), deniedId);
            Assert.IsFalse(rendered.Contains(deniedId, StringComparison.Ordinal), deniedId);
        }
    }

    [TestMethod]
    public void MissingRaftVoss_PreservesEpistemicCategories()
    {
        var packet = Compose(LoadMissingRaft(), MissingRaftContract.VossId).Packet;

        CollectionAssert.AreEqual(
            new[] { MissingRaftContract.KnowVossCurrentStrengthenedId },
            packet.Knowledge.Select(record => record.RecordId.Value).ToArray());
        CollectionAssert.AreEqual(
            new[] { MissingRaftContract.BelVossAccidentalLossPlausibleId },
            packet.Beliefs.Select(record => record.RecordId.Value).ToArray());
        Assert.AreEqual(0, packet.Observations.Length);
        Assert.AreEqual(0, packet.Suspicions.Length);
        Assert.AreEqual(0, packet.Circumstance.Length);
    }

    [TestMethod]
    public void MissingRaftVoss_RenderingMatchesCanonicalFixtureRelationshipText()
    {
        var actual = Compose(LoadMissingRaft(), MissingRaftContract.VossId)
            .Packet.Rendered.TrustedStateText;

        StringAssert.Contains(
            actual,
            "[RELATIONSHIPS]\n- Marlowe: Voss respects Marlowe's perception but grants him less benefit of the doubt on unilateral choices.");
        Assert.IsFalse(actual.Contains("Marlowe's competence", StringComparison.Ordinal));
        Assert.IsFalse(actual.EndsWith('\n'));
        Assert.IsFalse(actual.Contains('\r'));
    }

    [TestMethod]
    public void RenderedLayers_AreSeparateAndExact()
    {
        var rendered = Compose(LoadMissingRaft(), MissingRaftContract.VossId).Packet.Rendered;

        Assert.AreEqual(E0ContextContracts.RenderingContract, rendered.RenderingContract);
        Assert.AreEqual(string.Empty, rendered.RecentPerformanceText);
        Assert.AreEqual("You have the current opportunity to act.", rendered.OpportunityText);
        Assert.IsFalse(rendered.TrustedStateText.EndsWith('\n'));
        Assert.IsFalse(rendered.OpportunityText.EndsWith('\n'));
    }

    [TestMethod]
    public void StructuredCanonicalJson_ReservesExactEmptyRecentPerformancesAndExcludesRenderingContract()
    {
        var packet = Compose(LoadMissingRaft(), MissingRaftContract.VossId).Packet;
        var json = Encoding.UTF8.GetString(ContextPacketCanonicalizer.SerializeStructured(packet));

        StringAssert.Contains(json, "\"recentPerformances\":[]");
        Assert.IsFalse(json.Contains("renderingContract", StringComparison.Ordinal));
        Assert.IsFalse(json.Contains("renderedContextHash", StringComparison.Ordinal));
        Assert.IsFalse(json.Contains("trustedStateText", StringComparison.Ordinal));
    }

    [TestMethod]
    public void RenderedCanonicalJson_IncludesRenderingContractAndExactLayerOrder()
    {
        var packet = Compose(LoadMissingRaft(), MissingRaftContract.VossId).Packet;
        var json = Encoding.UTF8.GetString(
            ContextPacketCanonicalizer.SerializeRendered(packet.Rendered));

        Assert.IsTrue(json.StartsWith(
            "{\"renderingContract\":\"ensemble.e0.context.render.v1\",\"trustedStateText\":",
            StringComparison.Ordinal));
        Assert.IsTrue(
            json.IndexOf("\"trustedStateText\":", StringComparison.Ordinal) <
            json.IndexOf("\"recentPerformanceText\":", StringComparison.Ordinal));
        Assert.IsTrue(
            json.IndexOf("\"recentPerformanceText\":", StringComparison.Ordinal) <
            json.IndexOf("\"opportunityText\":", StringComparison.Ordinal));
    }

    [TestMethod]
    public void RepeatedComposition_IsByteIdentical()
    {
        var fixture = LoadMissingRaft();
        var first = Compose(fixture, MissingRaftContract.VossId).Packet;
        var second = Compose(fixture, MissingRaftContract.VossId).Packet;

        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeStructured(first),
            ContextPacketCanonicalizer.SerializeStructured(second));
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeRendered(first.Rendered),
            ContextPacketCanonicalizer.SerializeRendered(second.Rendered));
        Assert.AreEqual(first.ContextPacketId, second.ContextPacketId);
        Assert.AreEqual(first.StructuredContextHash, second.StructuredContextHash);
        Assert.AreEqual(first.RenderedContextHash, second.RenderedContextHash);
    }

    [TestMethod]
    public void SourceReordering_DoesNotChangeContextIdentityOrRendering()
    {
        var canonical = LoadMissingRaft();
        var root = JsonNode.Parse(ReadFixture("missing-raft-0.1.0.json"))!.AsObject();
        Reverse(root["characters"]!.AsArray());
        Reverse(root["scene"]!.AsObject()["roster"]!.AsArray());
        Reverse(root["sceneState"]!.AsArray());
        Reverse(root["pressures"]!.AsArray());
        var voss = Character(root, MissingRaftContract.VossCharacterId);
        Reverse(voss["memories"]!.AsArray());
        Reverse(voss["relationships"]!.AsArray());
        var reordered = Validate(root.ToJsonString());
        MissingRaftContract.Validate(reordered);

        var expected = Compose(canonical, MissingRaftContract.VossId).Packet;
        var actual = Compose(reordered, MissingRaftContract.VossId).Packet;

        Assert.AreEqual(expected.ContextPacketId, actual.ContextPacketId);
        Assert.AreEqual(expected.StructuredContextHash, actual.StructuredContextHash);
        Assert.AreEqual(expected.RenderedContextHash, actual.RenderedContextHash);
        Assert.AreEqual(expected.Rendered.TrustedStateText, actual.Rendered.TrustedStateText);
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeStructured(expected),
            ContextPacketCanonicalizer.SerializeStructured(actual));
    }

    [TestMethod]
    public void SemanticTextMutation_ChangesStructuredPacketAndRenderedIdentity()
    {
        var canonical = LoadFixture("e0-fixture-v1.json");
        var root = JsonNode.Parse(ReadFixture("e0-fixture-v1.json"))!.AsObject();
        var subject = Character(root, "CHAR-A");
        subject["constitution"]!.AsArray()[0]!.AsObject()["text"] =
            "A changed constitutional statement.";
        var mutated = Validate(root.ToJsonString());

        var expected = Compose(canonical, CharacterId.From("CHAR-A")).Packet;
        var actual = Compose(mutated, CharacterId.From("CHAR-A")).Packet;

        Assert.AreNotEqual(expected.ContextPacketId, actual.ContextPacketId);
        Assert.AreNotEqual(expected.StructuredContextHash, actual.StructuredContextHash);
        Assert.AreNotEqual(expected.RenderedContextHash, actual.RenderedContextHash);
    }

    [TestMethod]
    public void SameRecordMovedToDifferentAuthorityCategory_ChangesStructuredIdentity()
    {
        var canonical = LoadMissingRaft();
        var root = JsonNode.Parse(ReadFixture("missing-raft-0.1.0.json"))!.AsObject();
        var voss = Character(root, MissingRaftContract.VossCharacterId);
        var knowledge = voss["knowledge"]!.AsArray();
        var moved = knowledge[0]!;
        knowledge.RemoveAt(0);
        voss["beliefs"]!.AsArray().Add(moved);
        var mutated = Validate(root.ToJsonString());

        var expected = Compose(canonical, MissingRaftContract.VossId).Packet;
        var actual = Compose(mutated, MissingRaftContract.VossId).Packet;

        Assert.AreNotEqual(expected.StructuredContextHash, actual.StructuredContextHash);
        Assert.AreNotEqual(expected.ContextPacketId, actual.ContextPacketId);
        Assert.IsTrue(actual.Knowledge.IsEmpty);
        Assert.AreEqual(2, actual.Beliefs.Length);
    }

    [TestMethod]
    public void MultilineRecordText_RendersContinuationLinesWithoutRewritingSource()
    {
        var root = JsonNode.Parse(ReadFixture("e0-fixture-v1.json"))!.AsObject();
        var subject = Character(root, "CHAR-A");
        subject["constitution"]!.AsArray()[0]!.AsObject()["text"] = "Line one\nLine two";
        var fixture = Validate(root.ToJsonString());
        var rendered = Compose(fixture, CharacterId.From("CHAR-A")).Packet.Rendered.TrustedStateText;

        StringAssert.Contains(rendered, "- Line one\n  Line two");
        StringAssert.Contains(rendered, "[RIGHT NOW]\n- none");
    }

    [TestMethod]
    public void MismatchedOpportunity_FailsClosed()
    {
        var fixture = LoadMissingRaft();
        var projection = CharacterBoundedAccessControl.Evaluate(
            fixture,
            MissingRaftContract.VossId).Projection;

        Assert.Throws<ContextCompositionException>(() =>
            DeterministicContextComposer.Compose(projection, MissingRaftContract.WrenId));
    }

    [TestMethod]
    public void UninitializedOpportunity_FailsClosed()
    {
        var fixture = LoadMissingRaft();
        var projection = CharacterBoundedAccessControl.Evaluate(
            fixture,
            MissingRaftContract.VossId).Projection;

        Assert.Throws<ContextCompositionException>(() =>
            DeterministicContextComposer.Compose(projection, default));
    }

    [TestMethod]
    public void ContextAuthorityOutputs_CannotBePubliclyConstructed()
    {
        foreach (var type in new[]
                 {
                     typeof(ContextParticipant),
                     typeof(ContextRecord),
                     typeof(ContextRelationship),
                     typeof(ContextRecentPerformance),
                     typeof(RenderedContext),
                     typeof(ContextPacket),
                     typeof(ContextCompositionTrace),
                     typeof(ContextCompositionEvaluation)
                 })
        {
            Assert.AreEqual(
                0,
                type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length,
                type.FullName);
        }
    }

    [TestMethod]
    public void ContextBoundary_ExposesOnlyApprovedRecentPerformanceSemanticType()
    {
        Assert.IsNull(typeof(ContextRecord).GetProperty("Provenance"));
        Assert.IsNull(typeof(ContextRelationship).GetProperty("Provenance"));
        Assert.IsNull(typeof(ContextPacket).GetProperty("AccessDecisions"));
        Assert.IsNull(typeof(ContextCompositionTrace).GetProperty("DeniedRecordIds"));

        var recentHistoryTypes = typeof(ContextPacket).Assembly.GetExportedTypes()
            .Where(type => string.Equals(
                type.Namespace,
                typeof(ContextPacket).Namespace,
                StringComparison.Ordinal))
            .Where(type => type.Name.Contains("Performance", StringComparison.Ordinal))
            .ToArray();
        Assert.AreEqual(1, recentHistoryTypes.Length);
        Assert.AreEqual(typeof(ContextRecentPerformance), recentHistoryTypes[0]);
        CollectionAssert.AreEquivalent(
            new[] { "SourceCharacterId", "VisibleText" },
            typeof(ContextRecentPerformance)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(property => property.Name)
                .ToArray());
        Assert.AreEqual(0, typeof(ContextRecentPerformance)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);
        Assert.IsFalse(typeof(ContextRecentPerformance)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Any(property => property.SetMethod?.IsPublic == true));
    }

    [TestMethod]
    public void ProviderNeutralRendering_OmitsInternalIdsAndProvenance()
    {
        var rendered = Compose(LoadMissingRaft(), MissingRaftContract.VossId)
            .Packet.Rendered.TrustedStateText;

        foreach (var forbidden in new[]
                 {
                     "CON-VOSS",
                     "KNOW-VOSS-CURRENT-STRENGTHENED",
                     "REL-VOSS-MARLOWE",
                     "SCENE-MISSING-RAFT",
                     "WORLD-CURRENT-STRENGTHENED",
                     "HT-MARLOWE-RELEASED-RAFT"
                 })
        {
            Assert.IsFalse(rendered.Contains(forbidden, StringComparison.Ordinal), forbidden);
        }
    }

    [TestMethod]
    public void Trace_CoversEveryPacketRecordExactlyOnceAndIsOrdinallySorted()
    {
        var evaluation = Compose(LoadMissingRaft(), MissingRaftContract.VossId);
        var expected = PacketRecordIds(evaluation.Packet)
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();
        var actual = evaluation.Trace.IncludedRecordIds
            .Select(id => id.Value)
            .ToArray();

        CollectionAssert.AreEqual(expected, actual);
        Assert.AreEqual(actual.Length, actual.Distinct(StringComparer.Ordinal).Count());
        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "VOSS", "WREN" },
            evaluation.Trace.IncludedRosterCharacterIds.Select(id => id.Value).ToArray());
    }

    [TestMethod]
    public void Composition_DoesNotMutateAccessProjectionOrFrozenFixtureIdentity()
    {
        var fixture = LoadMissingRaft();
        var projection = CharacterBoundedAccessControl.Evaluate(
            fixture,
            MissingRaftContract.VossId).Projection;
        var beforeAccess = string.Join("|", AccessRecordIds(projection));
        var beforeFixtureHash = FixtureHash.Compute(fixture);

        _ = DeterministicContextComposer.Compose(projection, MissingRaftContract.VossId);

        Assert.AreEqual(beforeAccess, string.Join("|", AccessRecordIds(projection)));
        Assert.AreEqual(beforeFixtureHash, FixtureHash.Compute(fixture));
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, FixtureHash.Compute(fixture));
    }

    [TestMethod]
    public void Ecj1Extraction_PreservesFrozenMissingRaftBytesAndDigest()
    {
        var fixture = LoadMissingRaft();
        var bytes = Ecj1FixtureCanonicalizer.Serialize(fixture);

        Assert.AreEqual(9112, bytes.Length);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, Sha256Lower(bytes));
    }

    [TestMethod]
    public void GenericSmoke_ComposesFromSafeProjectionOnly()
    {
        var fixture = LoadFixture("e0-fixture-v1.json");
        var subject = CharacterId.From("CHAR-A");
        var evaluation = Compose(fixture, subject);
        var method = typeof(DeterministicContextComposer).GetMethod(
            nameof(DeterministicContextComposer.Compose),
            BindingFlags.Public | BindingFlags.Static)!;

        CollectionAssert.AreEqual(
            new[] { typeof(CharacterAccessProjection), typeof(CharacterId) },
            method.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(subject, evaluation.Packet.SubjectCharacterId);
        Assert.AreEqual(subject, evaluation.Packet.OpportunityCharacterId);
        Assert.AreEqual(E0ContextContracts.SchemaVersion, evaluation.Packet.SchemaVersion);
        Assert.AreEqual(E0ContextContracts.CompositionContract, evaluation.Packet.CompositionContract);
    }

    [TestMethod]
    public void HashAndPacketIdFormats_AreExact()
    {
        var packet = Compose(LoadMissingRaft(), MissingRaftContract.VossId).Packet;

        StringAssert.Matches(
            packet.StructuredContextHash,
            new System.Text.RegularExpressions.Regex("^[0-9a-f]{64}$"));
        StringAssert.Matches(
            packet.RenderedContextHash,
            new System.Text.RegularExpressions.Regex("^[0-9a-f]{64}$"));
        Assert.AreEqual($"CTX:{packet.StructuredContextHash}", packet.ContextPacketId.Value);
    }

    private static ContextCompositionEvaluation Compose(
        ValidatedFixture fixture,
        CharacterId subject)
    {
        var projection = CharacterBoundedAccessControl.Evaluate(fixture, subject).Projection;
        return DeterministicContextComposer.Compose(projection, subject);
    }

    private static ValidatedFixture LoadMissingRaft()
    {
        var fixture = LoadFixture("missing-raft-0.1.0.json");
        MissingRaftContract.Validate(fixture);
        return fixture;
    }

    private static ValidatedFixture LoadFixture(string fileName) =>
        Validate(ReadFixture(fileName));

    private static ValidatedFixture Validate(string json)
    {
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        return GenericE0FixtureValidator.Validate(document);
    }

    private static string ReadFixture(string fileName) =>
        File.ReadAllText(
            Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName),
            Encoding.UTF8);

    private static JsonObject Character(JsonObject root, string characterId) =>
        root["characters"]!.AsArray()
            .Select(node => node!.AsObject())
            .Single(character => string.Equals(
                character["id"]!.GetValue<string>(),
                characterId,
                StringComparison.Ordinal));

    private static void Reverse(JsonArray array)
    {
        var values = array.ToArray();
        array.Clear();
        for (var index = values.Length - 1; index >= 0; index--)
        {
            array.Add(values[index]);
        }
    }

    private static IEnumerable<string> AccessRecordIds(CharacterAccessProjection projection) =>
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

    private static IEnumerable<string> PacketRecordIds(ContextPacket packet) =>
        packet.SceneState.Select(record => record.RecordId.Value)
            .Concat(packet.Pressures.Select(record => record.RecordId.Value))
            .Concat(packet.Constitution.Select(record => record.RecordId.Value))
            .Concat(packet.Disposition.Select(record => record.RecordId.Value))
            .Concat(packet.Circumstance.Select(record => record.RecordId.Value))
            .Concat(packet.Observations.Select(record => record.RecordId.Value))
            .Concat(packet.Knowledge.Select(record => record.RecordId.Value))
            .Concat(packet.Beliefs.Select(record => record.RecordId.Value))
            .Concat(packet.Suspicions.Select(record => record.RecordId.Value))
            .Concat(packet.Memories.Select(record => record.RecordId.Value))
            .Concat(packet.Goals.Select(record => record.RecordId.Value))
            .Concat(packet.Relationships.Select(record => record.RecordId.Value));

    private static IEnumerable<string> CharacterOwnedIds(ValidatedCharacter character) =>
        character.Constitution.Select(record => record.Id.Value)
            .Concat(character.Disposition.Select(record => record.Id.Value))
            .Concat(character.Circumstance.Select(record => record.Id.Value))
            .Concat(character.Observations.Select(record => record.Id.Value))
            .Concat(character.Knowledge.Select(record => record.Id.Value))
            .Concat(character.Beliefs.Select(record => record.Id.Value))
            .Concat(character.Suspicions.Select(record => record.Id.Value))
            .Concat(character.Memories.Select(record => record.Id.Value))
            .Concat(character.Goals.Select(record => record.Id.Value))
            .Concat(character.Relationships.Select(record => record.Id.Value));

    private static string Sha256Lower(byte[] bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
}
