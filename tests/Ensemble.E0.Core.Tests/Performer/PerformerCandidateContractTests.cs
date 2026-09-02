using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Performer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Performer;

[TestClass]
public sealed class PerformerCandidateContractTests
{
    private const string ExpectedVossStructuredHash =
        "bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b";
    private const string ExpectedVossRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";

    [TestMethod]
    public void ValidVossCandidate_IsSemanticProviderNeutralAndBoundToPacketIdentity()
    {
        var packet = ComposeVoss();
        var candidate = Parse(
            packet,
            CandidateJson(
                "Before we decide what happened, what did each of us actually observe?",
                new[] { "WREN", "MARLOWE" },
                "WREN"));

        Assert.AreEqual(PerformerCandidateContract.CandidateContractVersion, candidate.ContractVersion);
        Assert.AreNotEqual(
            PerformerCandidateContract.CandidateJsonSchemaVersion,
            candidate.ContractVersion);
        Assert.AreEqual(packet.SubjectCharacterId, candidate.SubjectCharacterId);
        Assert.AreEqual(packet.ContextPacketId, candidate.ContextPacketId);
        Assert.AreEqual(
            "Before we decide what happened, what did each of us actually observe?",
            candidate.VisibleText);
        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "WREN" },
            candidate.Control.AddressedCharacterIds.Select(id => id.Value).ToArray());
        Assert.AreEqual("WREN", candidate.Control.NominatedCharacterId!.Value.Value);
    }

    [TestMethod]
    public void SemanticAndTransportVersions_AreIndependentlyVersioned()
    {
        var candidate = Parse(ComposeVoss(), CandidateJson("I need a moment."));

        Assert.AreEqual(
            "ensemble.e0.performer.candidate.v1",
            PerformerCandidateContract.CandidateContractVersion);
        Assert.AreEqual(
            "ensemble.e0.performer.candidate-json.v1",
            PerformerCandidateContract.CandidateJsonSchemaVersion);
        Assert.AreNotEqual(
            PerformerCandidateContract.CandidateJsonSchemaVersion,
            candidate.ContractVersion);
        Assert.IsNull(typeof(CandidatePerformance).GetProperty("SchemaVersion"));
    }

    [TestMethod]
    public void Silence_IsExactEmptyTextWithEmptyControl()
    {
        var candidate = Parse(ComposeVoss(), CandidateJson(string.Empty));

        Assert.AreEqual(string.Empty, candidate.VisibleText);
        Assert.AreEqual(0, candidate.Control.AddressedCharacterIds.Length);
        Assert.IsNull(candidate.Control.NominatedCharacterId);
    }

    [TestMethod]
    public void NonSilent_EmptyControlIsValid()
    {
        var candidate = Parse(ComposeVoss(), CandidateJson("No."));

        Assert.AreEqual("No.", candidate.VisibleText);
        Assert.AreEqual(0, candidate.Control.AddressedCharacterIds.Length);
        Assert.IsNull(candidate.Control.NominatedCharacterId);
    }

    [TestMethod]
    public void NonSilent_AddressOnlyIsValid()
    {
        var candidate = Parse(
            ComposeVoss(),
            CandidateJson("Marlowe, wait.", new[] { "MARLOWE" }));

        CollectionAssert.AreEqual(
            new[] { "MARLOWE" },
            candidate.Control.AddressedCharacterIds.Select(id => id.Value).ToArray());
        Assert.IsNull(candidate.Control.NominatedCharacterId);
    }

    [TestMethod]
    public void NonSilent_NominationOnlyIsValid()
    {
        var candidate = Parse(
            ComposeVoss(),
            CandidateJson("Wren?", nominatedCharacterId: "WREN"));

        Assert.AreEqual(0, candidate.Control.AddressedCharacterIds.Length);
        Assert.AreEqual("WREN", candidate.Control.NominatedCharacterId!.Value.Value);
    }

    [TestMethod]
    public void NominationMayOverlapAddress()
    {
        var candidate = Parse(
            ComposeVoss(),
            CandidateJson("Wren?", new[] { "WREN" }, "WREN"));

        Assert.AreEqual("WREN", candidate.Control.AddressedCharacterIds.Single().Value);
        Assert.AreEqual("WREN", candidate.Control.NominatedCharacterId!.Value.Value);
    }

    [TestMethod]
    public void VisibleText_PreservesWhitespaceLfTabFormatAndUnicodeExactly()
    {
        const string text = "  Café\tturns\naway.\u200D  ";
        var candidate = Parse(ComposeVoss(), CandidateJson(text));

        Assert.AreEqual(text, candidate.VisibleText);
    }

    [TestMethod]
    [DataRow("   ")]
    [DataRow("\t\n")]
    [DataRow("\u200D")]
    [DataRow("\u0301")]
    public void InvisibleOrWhitespaceOnlyNonEmptyPerformance_Fails(string text)
    {
        AssertCandidateFailure(ComposeVoss(), CandidateJson(text));
    }

    [TestMethod]
    public void NonNfcText_FailsRatherThanNormalizing()
    {
        const string decomposed = "Cafe\u0301";
        Assert.IsFalse(decomposed.IsNormalized(NormalizationForm.FormC));
        AssertCandidateFailure(ComposeVoss(), CandidateJson(decomposed));
    }

    [TestMethod]
    [DataRow("A\rB")]
    [DataRow("A\0B")]
    [DataRow("A\u0001B")]
    [DataRow("A\u007FB")]
    public void ForbiddenControlScalars_Fail(string text)
    {
        AssertCandidateFailure(ComposeVoss(), CandidateJson(text));
    }

    [TestMethod]
    public void EscapedIsolatedSurrogate_Fails()
    {
        var json =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"\\uD800\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";

        AssertCandidateFailure(ComposeVoss(), json);
    }

    [TestMethod]
    public void SilenceWithAddress_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson(string.Empty, new[] { "MARLOWE" }));
    }

    [TestMethod]
    public void SilenceWithNomination_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson(string.Empty, nominatedCharacterId: "WREN"));
    }

    [TestMethod]
    public void SelfAddress_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("I answer myself.", new[] { "VOSS" }));
    }

    [TestMethod]
    public void DuplicateAddress_FailsRatherThanDeduplicating()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("Marlowe.", new[] { "MARLOWE", "MARLOWE" }));
    }

    [TestMethod]
    public void AddressOutsideRoster_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("Someone else.", new[] { "CHAR-X" }));
    }

    [TestMethod]
    public void AddressIsCaseSensitive()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("Marlowe.", new[] { "marlowe" }));
    }

    [TestMethod]
    public void AddressArrayBeyondRosterMinusSubject_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("Everyone.", new[] { "MARLOWE", "WREN", "CHAR-X" }));
    }

    [TestMethod]
    public void SelfNomination_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("Me again.", nominatedCharacterId: "VOSS"));
    }

    [TestMethod]
    public void NominationOutsideRoster_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("You.", nominatedCharacterId: "CHAR-X"));
    }

    [TestMethod]
    public void NominationIsCaseSensitive()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            CandidateJson("Wren?", nominatedCharacterId: "wren"));
    }

    [TestMethod]
    public void PropertyReorderingAndStructuralWhitespace_DoNotChangeSemanticCandidate()
    {
        var packet = ComposeVoss();
        var canonical = Parse(packet, CandidateJson("Wren?", new[] { "WREN" }, "WREN"));
        const string reordered =
            " { \"control\" : { \"nominatedCharacterId\" : \"WREN\", \"addressedCharacterIds\" : [ \"WREN\" ] }, \"performance\" : { \"text\" : \"Wren?\" }, \"schemaVersion\" : \"ensemble.e0.performer.candidate-json.v1\" } ";
        var actual = Parse(packet, reordered);

        AssertSemanticEqual(canonical, actual);
    }

    [TestMethod]
    public void EquivalentJsonEscapes_ProduceSameSemanticCandidate()
    {
        var packet = ComposeVoss();
        var canonical = Parse(packet, CandidateJson("Hello"));
        const string escaped =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"\\u0048ello\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";

        AssertSemanticEqual(canonical, Parse(packet, escaped));
    }

    [TestMethod]
    public void MarkdownCodeFenceWrapper_Fails()
    {
        AssertCandidateFailure(
            ComposeVoss(),
            "```json\n" + CandidateJson("Hello") + "\n```");
    }

    [TestMethod]
    public void EmptyJson_Fails()
    {
        AssertCandidateFailure(ComposeVoss(), Array.Empty<byte>());
    }

    [TestMethod]
    public void ExactlyOneMiBValidJson_Succeeds()
    {
        var bytes = ExactSizedCandidateJson(PerformerCandidateContract.MaxCandidateJsonBytes);
        var candidate = PerformerCandidateContract.ParseJson(ComposeVoss(), bytes);

        Assert.AreEqual(
            PerformerCandidateContract.MaxCandidateJsonBytes,
            bytes.Length);
        Assert.IsTrue(candidate.VisibleText.Length > 1_000_000);
    }

    [TestMethod]
    public void OneMiBPlusOne_FailsBeforeParsing()
    {
        var bytes = ExactSizedCandidateJson(PerformerCandidateContract.MaxCandidateJsonBytes + 1);
        Assert.AreEqual(
            PerformerCandidateContract.MaxCandidateJsonBytes + 1,
            bytes.Length);
        AssertCandidateFailure(ComposeVoss(), bytes);
    }

    [TestMethod]
    public void JsonDepthGreaterThanEight_Fails()
    {
        var json =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null},\"x\":[[[[[[[[[0]]]]]]]]]}";
        AssertCandidateFailure(ComposeVoss(), json);
    }

    [TestMethod]
    public void SemanticContractVersionIsNotAcceptedAsTransportSchema()
    {
        var json = CandidateJson("Hello").Replace(
            PerformerCandidateContract.CandidateJsonSchemaVersion,
            PerformerCandidateContract.CandidateContractVersion,
            StringComparison.Ordinal);
        AssertCandidateFailure(ComposeVoss(), json);
    }

    [TestMethod]
    public void TransportSchemaAndPropertyNames_AreCaseSensitive()
    {
        var wrongSchemaCase = CandidateJson("Hello").Replace(
            "candidate-json.v1",
            "CANDIDATE-JSON.V1",
            StringComparison.Ordinal);
        AssertCandidateFailure(ComposeVoss(), wrongSchemaCase);

        var wrongPropertyCase = CandidateJson("Hello").Replace(
            "\"performance\"",
            "\"Performance\"",
            StringComparison.Ordinal);
        AssertCandidateFailure(ComposeVoss(), wrongPropertyCase);
    }

    [TestMethod]
    public void UnknownPropertiesFailAtEveryContractObject()
    {
        var root = CandidateJson("Hello").Replace(
            "{\"schemaVersion\"",
            "{\"unknown\":true,\"schemaVersion\"",
            StringComparison.Ordinal);
        var performance = CandidateJson("Hello").Replace(
            "\"performance\":{\"text\":\"Hello\"}",
            "\"performance\":{\"text\":\"Hello\",\"unknown\":true}",
            StringComparison.Ordinal);
        var control = CandidateJson("Hello").Replace(
            "\"nominatedCharacterId\":null}",
            "\"nominatedCharacterId\":null,\"unknown\":true}",
            StringComparison.Ordinal);

        AssertCandidateFailure(ComposeVoss(), root);
        AssertCandidateFailure(ComposeVoss(), performance);
        AssertCandidateFailure(ComposeVoss(), control);
    }

    [TestMethod]
    public void MissingRequiredProperties_Fail()
    {
        const string missingPerformance =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";
        const string missingText =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";
        const string missingControlField =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[]}}";

        AssertCandidateFailure(ComposeVoss(), missingPerformance);
        AssertCandidateFailure(ComposeVoss(), missingText);
        AssertCandidateFailure(ComposeVoss(), missingControlField);
    }

    [TestMethod]
    public void DuplicateDecodedProperties_FailAtRootAndNestedLevels()
    {
        const string duplicateRoot =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";
        const string duplicateNested =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\",\"text\":\"B\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";
        const string escapedDuplicate =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\",\"te\\u0078t\":\"B\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";

        AssertCandidateFailure(ComposeVoss(), duplicateRoot);
        AssertCandidateFailure(ComposeVoss(), duplicateNested);
        AssertCandidateFailure(ComposeVoss(), escapedDuplicate);
    }

    [TestMethod]
    public void CommentsTrailingCommasAndBom_Fail()
    {
        var comment = CandidateJson("Hello").Replace(
            "{\"schemaVersion\"",
            "{/*comment*/\"schemaVersion\"",
            StringComparison.Ordinal);
        var trailingComma = CandidateJson("Hello").Replace(
            "\"nominatedCharacterId\":null}",
            "\"nominatedCharacterId\":null,}",
            StringComparison.Ordinal);
        var normal = Encoding.UTF8.GetBytes(CandidateJson("Hello"));
        var bom = new byte[normal.Length + 3];
        bom[0] = 0xEF;
        bom[1] = 0xBB;
        bom[2] = 0xBF;
        Buffer.BlockCopy(normal, 0, bom, 3, normal.Length);

        AssertCandidateFailure(ComposeVoss(), comment);
        AssertCandidateFailure(ComposeVoss(), trailingComma);
        AssertCandidateFailure(ComposeVoss(), bom);
    }

    [TestMethod]
    public void MalformedUtf8MalformedJsonAndTrailingContent_Fail()
    {
        AssertCandidateFailure(ComposeVoss(), new byte[] { (byte)'{', 0xFF, (byte)'}' });
        AssertCandidateFailure(ComposeVoss(), "{\"schemaVersion\":");
        AssertCandidateFailure(ComposeVoss(), CandidateJson("Hello") + "true");
    }

    [TestMethod]
    public void WrongJsonTokenTypes_Fail()
    {
        var cases = new[]
        {
            "{\"schemaVersion\":null,\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}",
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":[],\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}",
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":1},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}",
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":[]}",
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":\"WREN\",\"nominatedCharacterId\":null}}",
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[1],\"nominatedCharacterId\":null}}",
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":1}}"
        };

        foreach (var json in cases)
        {
            AssertCandidateFailure(ComposeVoss(), json);
        }
    }

    [TestMethod]
    public void ExceptionRepresentation_DoesNotEchoUntrustedVisibleText()
    {
        const string sentinel = "VISIBLE-SENTINEL";
        AssertFailureDoesNotContain(
            ComposeVoss(),
            CandidateJson(sentinel + "\u0001"),
            sentinel);
    }

    [TestMethod]
    public void ExceptionRepresentation_DoesNotEchoInvalidId()
    {
        const string sentinel = "INVALID-ID-SENTINEL";
        AssertFailureDoesNotContain(
            ComposeVoss(),
            CandidateJson("Hello", new[] { sentinel }),
            sentinel);
    }

    [TestMethod]
    public void ExceptionRepresentation_DoesNotEchoMismatchedSchema()
    {
        const string sentinel = "SCHEMA-SENTINEL";
        var json = CandidateJson("Hello").Replace(
            PerformerCandidateContract.CandidateJsonSchemaVersion,
            sentinel,
            StringComparison.Ordinal);
        AssertFailureDoesNotContain(ComposeVoss(), json, sentinel);
    }

    [TestMethod]
    public void ExceptionRepresentation_DoesNotEchoUnknownPropertyName()
    {
        const string sentinel = "UNKNOWN-PROPERTY-SENTINEL";
        var json = CandidateJson("Hello").Replace(
            "{\"schemaVersion\"",
            "{\"UNKNOWN-PROPERTY-SENTINEL\\nX\":true,\"schemaVersion\"",
            StringComparison.Ordinal);
        AssertFailureDoesNotContain(ComposeVoss(), json, sentinel);
    }

    [TestMethod]
    public void ExceptionRepresentation_DoesNotEchoMalformedPayloadContent()
    {
        const string sentinel = "MALFORMED-SENTINEL";
        var json =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"" +
            sentinel +
            "\"}";
        AssertFailureDoesNotContain(ComposeVoss(), json, sentinel);
    }

    [TestMethod]
    public void CandidateAuthorityTypes_CannotBePubliclyConstructed()
    {
        foreach (var type in new[]
                 {
                     typeof(CandidatePerformance),
                     typeof(CandidatePerformanceControl)
                 })
        {
            Assert.AreEqual(
                0,
                type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length,
                type.FullName);
        }
    }

    [TestMethod]
    public void ParseJson_IsOnlyPublicCandidateConstructionOperation()
    {
        var methods = typeof(PerformerCandidateContract)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(method => method.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(new[] { nameof(PerformerCandidateContract.ParseJson) }, methods);
    }

    [TestMethod]
    public void CandidateSurface_ContainsNoTransportProviderStateMutationTakeOrRoutingAuthority()
    {
        var propertyNames = typeof(CandidatePerformance)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);
        var controlPropertyNames = typeof(CandidatePerformanceControl)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

        CollectionAssert.AreEquivalent(
            new[] { "ContractVersion", "SubjectCharacterId", "ContextPacketId", "VisibleText", "Control" },
            propertyNames.ToArray());
        CollectionAssert.AreEquivalent(
            new[] { "AddressedCharacterIds", "NominatedCharacterId" },
            controlPropertyNames.ToArray());

        foreach (var forbidden in new[]
                 {
                     "RenderingContract", "RenderedContextHash", "SchemaVersion", "Provider", "Model",
                     "Request", "RawOutput", "Fixture", "Provenance", "AccessDecisions", "Mutation",
                     "Confidence", "Reasoning", "Accepted", "Committed", "Validated", "Director",
                     "Opportunity", "CandidateId", "TakeId"
                 })
        {
            Assert.IsFalse(propertyNames.Contains(forbidden), forbidden);
            Assert.IsFalse(controlPropertyNames.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void NoPerformanceKindTaxonomyWasIntroduced()
    {
        Assert.IsFalse(
            typeof(CandidatePerformance).Assembly.GetTypes()
                .Any(type => string.Equals(type.Name, "PerformanceKind", StringComparison.Ordinal)));
    }

    [TestMethod]
    public void RepeatedParse_IsSemanticallyIdenticalAndDoesNotMutateContextPacket()
    {
        var packet = ComposeVoss();
        var beforeId = packet.ContextPacketId;
        var beforeHash = packet.StructuredContextHash;
        var json = CandidateJson("Wren?", new[] { "WREN" }, "WREN");

        var first = Parse(packet, json);
        var second = Parse(packet, json);

        AssertSemanticEqual(first, second);
        Assert.AreEqual(beforeId, packet.ContextPacketId);
        Assert.AreEqual(beforeHash, packet.StructuredContextHash);
        Assert.AreEqual(ExpectedVossStructuredHash, packet.StructuredContextHash);
        Assert.AreEqual(ExpectedVossRenderedHash, packet.RenderedContextHash);
    }

    [TestMethod]
    public void FrozenContextAndFixtureIdentitiesRemainUnchanged()
    {
        var fixture = LoadMissingRaft();
        var packet = Compose(fixture, MissingRaftContract.VossId);
        var ecj1 = Ecj1FixtureCanonicalizer.Serialize(fixture);

        Assert.AreEqual(ExpectedVossStructuredHash, packet.StructuredContextHash);
        Assert.AreEqual(ExpectedVossRenderedHash, packet.RenderedContextHash);
        Assert.AreEqual(9112, ecj1.Length);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, Sha256Lower(ecj1));
    }

    private static CandidatePerformance Parse(ContextPacket packet, string json) =>
        PerformerCandidateContract.ParseJson(packet, Encoding.UTF8.GetBytes(json));

    private static CandidatePerformance Parse(ContextPacket packet, byte[] bytes) =>
        PerformerCandidateContract.ParseJson(packet, bytes);

    private static void AssertCandidateFailure(ContextPacket packet, string json) =>
        AssertCandidateFailure(packet, Encoding.UTF8.GetBytes(json));

    private static void AssertCandidateFailure(ContextPacket packet, byte[] bytes) =>
        Assert.Throws<PerformerCandidateException>(() =>
            PerformerCandidateContract.ParseJson(packet, bytes));

    private static void AssertFailureDoesNotContain(
        ContextPacket packet,
        string json,
        string sentinel)
    {
        var exception = Assert.Throws<PerformerCandidateException>(() =>
            PerformerCandidateContract.ParseJson(packet, Encoding.UTF8.GetBytes(json)));
        Assert.IsFalse(
            exception.ToString().Contains(sentinel, StringComparison.Ordinal),
            exception.ToString());
    }

    private static void AssertSemanticEqual(
        CandidatePerformance expected,
        CandidatePerformance actual)
    {
        Assert.AreEqual(expected.ContractVersion, actual.ContractVersion);
        Assert.AreEqual(expected.SubjectCharacterId, actual.SubjectCharacterId);
        Assert.AreEqual(expected.ContextPacketId, actual.ContextPacketId);
        Assert.AreEqual(expected.VisibleText, actual.VisibleText);
        CollectionAssert.AreEqual(
            expected.Control.AddressedCharacterIds.Select(id => id.Value).ToArray(),
            actual.Control.AddressedCharacterIds.Select(id => id.Value).ToArray());
        Assert.AreEqual(
            expected.Control.NominatedCharacterId?.Value,
            actual.Control.NominatedCharacterId?.Value);
    }

    private static string CandidateJson(
        string text,
        string[]? addressedCharacterIds = null,
        string? nominatedCharacterId = null) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text },
            control = new
            {
                addressedCharacterIds = addressedCharacterIds ?? Array.Empty<string>(),
                nominatedCharacterId
            }
        });

    private static byte[] ExactSizedCandidateJson(int targetByteLength)
    {
        const string prefix =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"";
        const string suffix =
            "\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":null}}";
        var fixedBytes = Encoding.UTF8.GetByteCount(prefix) + Encoding.UTF8.GetByteCount(suffix);
        var fillerLength = targetByteLength - fixedBytes;
        Assert.IsTrue(fillerLength > 0);
        var bytes = Encoding.UTF8.GetBytes(prefix + new string('A', fillerLength) + suffix);
        Assert.AreEqual(targetByteLength, bytes.Length);
        return bytes;
    }

    private static ContextPacket ComposeVoss() =>
        Compose(LoadMissingRaft(), MissingRaftContract.VossId);

    private static ContextPacket Compose(ValidatedFixture fixture, CharacterId subject)
    {
        var projection = CharacterBoundedAccessControl.Evaluate(fixture, subject).Projection;
        return DeterministicContextComposer.Compose(projection, subject).Packet;
    }

    private static ValidatedFixture LoadMissingRaft()
    {
        var fixture = Validate(ReadFixture("missing-raft-0.1.0.json"));
        MissingRaftContract.Validate(fixture);
        return fixture;
    }

    private static ValidatedFixture Validate(string json)
    {
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        return GenericE0FixtureValidator.Validate(document);
    }

    private static string ReadFixture(string fileName) =>
        File.ReadAllText(
            Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName),
            Encoding.UTF8);

    private static string Sha256Lower(byte[] bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
}
