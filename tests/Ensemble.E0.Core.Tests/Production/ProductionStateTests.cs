using System.Collections.Immutable;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Production;

[TestClass]
public sealed class ProductionStateTests
{
    private const string ExpectedGenesisStateHash =
        "6e5bb762614a6885721a5d160b99b8c67dc6d096a560e3f04a15fc10a7451376";

    [TestMethod]
    public void PublicContractsAndEnumValues_AreExact()
    {
        Assert.AreEqual(
            "ensemble.e0.production-state.v1",
            ProductionStateContracts.StateContractVersion);
        Assert.AreEqual(
            "ensemble.e0.production-state-hash.sha256.v1",
            ProductionStateContracts.StateHashContractVersion);

        CollectionAssert.AreEqual(
            Enumerable.Range(0, 17).ToArray(),
            Enum.GetValues<ProductionRecordDomain>().Select(value => (int)value).ToArray());
        CollectionAssert.AreEqual(
            new[]
            {
                "Unspecified", "HistoricalTruth", "UnresolvedProposition", "WorldState",
                "SceneState", "CharacterConstitution", "CharacterDisposition",
                "CharacterCircumstance", "CharacterObservation", "CharacterKnowledge",
                "CharacterBelief", "CharacterSuspicion", "CharacterMemory", "CharacterGoal",
                "CharacterClaim", "Relationship", "Pressure"
            },
            Enum.GetNames<ProductionRecordDomain>());
        CollectionAssert.AreEqual(
            new[] { 0, 1, 2 },
            Enum.GetValues<ProductionRecordLifecycle>().Select(value => (int)value).ToArray());
        CollectionAssert.AreEqual(
            new[] { "Unspecified", "Active", "Inactive" },
            Enum.GetNames<ProductionRecordLifecycle>());
        CollectionAssert.AreEqual(
            new[] { 0, 1, 2, 3 },
            Enum.GetValues<ProductionRecordProtection>().Select(value => (int)value).ToArray());
        CollectionAssert.AreEqual(
            new[] { "Unspecified", "None", "SystemImmutable", "CreatorLocked" },
            Enum.GetNames<ProductionRecordProtection>());
    }

    [TestMethod]
    public void StateHash_IsClosedStrongValueAndDefaultFails()
    {
        var valueProperty = typeof(StateHash).GetProperty(nameof(StateHash.Value))!;
        Assert.Throws<InvalidOperationException>(() => _ = default(StateHash).Value);
        Assert.AreEqual(0, typeof(StateHash)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            .Length);
        Assert.AreEqual(0, typeof(StateHash)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Count(method => method.Name is "From" or "Parse" or "Create"));
        Assert.AreEqual(typeof(string), valueProperty.PropertyType);
    }

    [TestMethod]
    public void ProductionPublicConstruction_IsClosedAndImmutable()
    {
        foreach (var type in new[]
                 {
                     typeof(ProductionState),
                     typeof(ProductionCharacter),
                     typeof(GlobalProductionRecord),
                     typeof(CharacterProductionRecord),
                     typeof(RelationshipProductionRecord),
                     typeof(ProductionStateException)
                 })
        {
            Assert.AreEqual(
                0,
                type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length,
                type.FullName);
        }

        foreach (var type in new[]
                 {
                     typeof(ProductionState),
                     typeof(ProductionCharacter),
                     typeof(GlobalProductionRecord),
                     typeof(CharacterProductionRecord),
                     typeof(RelationshipProductionRecord)
                 })
        {
            Assert.IsFalse(
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Any(property => property.SetMethod?.IsPublic == true),
                type.FullName);
        }

        CollectionAssert.AreEquivalent(
            new[]
            {
                "ContractVersion", "StateHash", "OriginFixtureId", "OriginFixtureFamilyId",
                "OriginFixtureVersion", "OriginFixtureHash", "SceneId", "Characters",
                "RosterCharacterIds", "CurrentOpportunityCharacterId", "Records"
            },
            PublicPropertyNames(typeof(ProductionState)).ToArray());
        Assert.IsFalse(PublicPropertyNames(typeof(ProductionState)).Contains("Chronology"));
        Assert.IsFalse(PublicPropertyNames(typeof(ProductionState)).Contains("AccessContract"));
        Assert.IsFalse(PublicPropertyNames(typeof(ProductionState)).Contains("ObservationContract"));
    }

    [TestMethod]
    public void Genesis_PreservesExactFixtureIdentityProjectionAndProtection()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);

        Assert.AreEqual(MissingRaftContract.ExpectedFixtureId, state.OriginFixtureId);
        Assert.AreEqual(MissingRaftContract.FamilyId, state.OriginFixtureFamilyId);
        Assert.AreEqual(MissingRaftContract.Version, state.OriginFixtureVersion);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, state.OriginFixtureHash);
        Assert.AreEqual(MissingRaftContract.ExpectedSceneId, state.SceneId);
        Assert.AreEqual(MissingRaftContract.VossId, state.CurrentOpportunityCharacterId);
        CollectionAssert.AreEqual(
            new[] { "MARLOWE|Marlowe", "VOSS|Dr. Voss", "WREN|Wren" },
            state.Characters
                .Select(character => $"{character.CharacterId.Value}|{character.DisplayName}")
                .ToArray());
        CollectionAssert.AreEqual(
            new[] { MissingRaftContract.MarloweId, MissingRaftContract.VossId, MissingRaftContract.WrenId },
            state.RosterCharacterIds.ToArray());

        Assert.AreEqual(49, state.Records.Length);
        CollectionAssert.AreEqual(
            state.Records.Select(record => record.RecordId.Value)
                .OrderBy(value => value, StringComparer.Ordinal)
                .ToArray(),
            state.Records.Select(record => record.RecordId.Value).ToArray());
        Assert.AreEqual(
            state.Records.Length,
            state.Records.Select(record => record.RecordId.Value).Distinct(StringComparer.Ordinal).Count());
        Assert.IsTrue(state.Records.All(record => record.Lifecycle == ProductionRecordLifecycle.Active));

        AssertProtection(state, MissingRaftContract.HtMarloweReleasedRaftId, ProductionRecordProtection.SystemImmutable);
        AssertProtection(state, MissingRaftContract.ConVossId, ProductionRecordProtection.SystemImmutable);
        AssertProtection(state, MissingRaftContract.ObsWrenMarloweReturnId, ProductionRecordProtection.SystemImmutable);
        AssertProtection(state, MissingRaftContract.BelVossAccidentalLossPlausibleId, ProductionRecordProtection.None);

        var belief = (CharacterProductionRecord)state.Records.Single(
            record => record.RecordId == RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId));
        Assert.AreEqual(ProductionRecordDomain.CharacterBelief, belief.Domain);
        Assert.AreEqual(MissingRaftContract.VossId, belief.SubjectCharacterId);
        CollectionAssert.AreEqual(
            new[]
            {
                RecordId.From(MissingRaftContract.KnowVossCurrentStrengthenedId),
                RecordId.From(MissingRaftContract.SceneRaftGoneId)
            },
            belief.Provenance.ToArray());

        var relationship = (RelationshipProductionRecord)state.Records.Single(
            record => record.RecordId == RecordId.From(MissingRaftContract.RelVossWrenId));
        Assert.AreEqual(MissingRaftContract.VossId, relationship.SubjectCharacterId);
        Assert.AreEqual(MissingRaftContract.WrenId, relationship.TargetCharacterId);
        Assert.AreEqual(ProductionRecordDomain.Relationship, relationship.Domain);
    }

    [TestMethod]
    public void CreatorLocks_AreValidatedAndCannotWeakenSystemImmutable()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var locked = ProductionState.Initialize(
            fixture,
            ImmutableArray.Create(
                RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId),
                RecordId.From(MissingRaftContract.ConVossId)));

        AssertProtection(
            locked,
            MissingRaftContract.BelVossAccidentalLossPlausibleId,
            ProductionRecordProtection.CreatorLocked);
        AssertProtection(
            locked,
            MissingRaftContract.ConVossId,
            ProductionRecordProtection.SystemImmutable);

        Assert.Throws<ProductionStateException>(() =>
            ProductionState.Initialize(fixture, default));
        Assert.Throws<ProductionStateException>(() =>
            ProductionState.Initialize(
                fixture,
                ImmutableArray.Create(RecordId.From("UNKNOWN-RECORD"))));
        Assert.Throws<ProductionStateException>(() =>
            ProductionState.Initialize(
                fixture,
                ImmutableArray.Create(
                    RecordId.From(MissingRaftContract.GoalVossId),
                    RecordId.From(MissingRaftContract.GoalVossId))));
        Assert.Throws<ProductionStateException>(() =>
            ProductionState.Initialize(
                fixture,
                ImmutableArray.Create(default(RecordId))));
    }

    [TestMethod]
    public void GenesisStateHash_UsesFixedCanonicalByteOracle()
    {
        var state = Patch0012TestSupport.Genesis();
        var canonicalBytes = InvokeInternalBytes(
            "Ensemble.E0.Core.Production.ProductionStateCanonicalizer",
            "SerializeProjection",
            state);
        var expectedBytes = Encoding.UTF8.GetBytes(BuildIndependentProjectionJson(state));

        CollectionAssert.AreEqual(expectedBytes, canonicalBytes);
        Assert.AreEqual(ExpectedGenesisStateHash, state.StateHash.Value);
        Assert.AreEqual(
            ExpectedGenesisStateHash,
            ComputeGenesisEnvelopeHash(expectedBytes));
        Assert.AreEqual(64, state.StateHash.Value.Length);
        Assert.IsTrue(state.StateHash.Value.All(character =>
            (character >= '0' && character <= '9') ||
            (character >= 'a' && character <= 'f')));
    }

    [TestMethod]
    public void FixtureAndProductionStateAuthoritySnapshots_AreSemanticallyEquivalentAtGenesis()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var state = Patch0012TestSupport.Genesis(fixture);
        var fixtureSnapshot = StateAuthoritySnapshot.Bind(fixture, ImmutableArray<RecordId>.Empty);
        var stateSnapshot = StateAuthoritySnapshot.Bind(state);

        AssertSnapshotEquivalent(fixtureSnapshot, stateSnapshot);
    }

    [TestMethod]
    public void ProductionExceptions_AreClosedAndSanitized()
    {
        var exception = Assert.Throws<ProductionStateException>(() =>
            ProductionState.Initialize(
                Patch0012TestSupport.LoadMissingRaft(),
                ImmutableArray.Create(RecordId.From("SECRET-UNKNOWN-RECORD"))));

        Assert.AreEqual(0, exception.Data.Count);
        Assert.IsFalse(exception.ToString().Contains("SECRET-UNKNOWN-RECORD", StringComparison.Ordinal));
        Assert.IsTrue(typeof(ProductionStateException).IsSealed);
        Assert.AreEqual(
            0,
            typeof(ProductionStateException)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);
    }

    private static void AssertProtection(
        ProductionState state,
        string recordId,
        ProductionRecordProtection expected) =>
        Assert.AreEqual(
            expected,
            state.Records.Single(record => record.RecordId == RecordId.From(recordId)).Protection);

    private static void AssertSnapshotEquivalent(
        StateAuthoritySnapshot expected,
        StateAuthoritySnapshot actual)
    {
        Assert.AreEqual(expected.SceneId, actual.SceneId);
        CollectionAssert.AreEqual(expected.RosterCharacterIds.ToArray(), actual.RosterCharacterIds.ToArray());
        Assert.AreEqual(expected.Records.Length, actual.Records.Length);
        for (var index = 0; index < expected.Records.Length; index++)
        {
            var left = expected.Records[index];
            var right = actual.Records[index];
            Assert.AreEqual(left.GetType(), right.GetType());
            Assert.AreEqual(left.RecordId, right.RecordId);
            Assert.AreEqual(left.RecordDomain, right.RecordDomain);
            Assert.AreEqual(left.Lifecycle, right.Lifecycle);
            Assert.AreEqual(left.Protection, right.Protection);

            if (left is CharacterStateAuthorityRecordDescriptor leftCharacter &&
                right is CharacterStateAuthorityRecordDescriptor rightCharacter)
            {
                Assert.AreEqual(leftCharacter.SubjectCharacterId, rightCharacter.SubjectCharacterId);
            }

            if (left is RelationshipStateAuthorityRecordDescriptor leftRelationship &&
                right is RelationshipStateAuthorityRecordDescriptor rightRelationship)
            {
                Assert.AreEqual(leftRelationship.SubjectCharacterId, rightRelationship.SubjectCharacterId);
                Assert.AreEqual(leftRelationship.TargetCharacterId, rightRelationship.TargetCharacterId);
            }
        }
    }

    private static byte[] InvokeInternalBytes(string typeName, string methodName, params object[] args)
    {
        var type = typeof(ProductionState).Assembly.GetType(typeName, throwOnError: true)!;
        var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Internal method {typeName}.{methodName} is missing.");
        return (byte[])method.Invoke(null, args)!;
    }

    private static string ComputeGenesisEnvelopeHash(byte[] projectionBytes)
    {
        var prefix = Encoding.UTF8.GetBytes(
            "{\"hashContract\":\"ensemble.e0.production-state-hash.sha256.v1\",\"kind\":\"genesis\",\"projection\":");
        var bytes = new byte[prefix.Length + projectionBytes.Length + 1];
        Buffer.BlockCopy(prefix, 0, bytes, 0, prefix.Length);
        Buffer.BlockCopy(projectionBytes, 0, bytes, prefix.Length, projectionBytes.Length);
        bytes[^1] = (byte)'}';
        return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    }

    private static string BuildIndependentProjectionJson(ProductionState state)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(
                   stream,
                   new JsonWriterOptions
                   {
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                       Indented = false
                   }))
        {
            writer.WriteStartObject();
            writer.WriteString("schemaVersion", ProductionStateContracts.StateContractVersion);
            writer.WriteStartObject("origin");
            writer.WriteString("fixtureId", state.OriginFixtureId.Value);
            writer.WriteString("fixtureFamilyId", state.OriginFixtureFamilyId.Value);
            writer.WriteString("fixtureVersion", state.OriginFixtureVersion.Value);
            writer.WriteString("fixtureHash", state.OriginFixtureHash);
            writer.WriteEndObject();
            writer.WriteString("sceneId", state.SceneId.Value);
            writer.WriteStartArray("characters");
            foreach (var character in state.Characters)
            {
                writer.WriteStartObject();
                writer.WriteString("id", character.CharacterId.Value);
                writer.WriteString("displayName", character.DisplayName);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("roster");
            foreach (var characterId in state.RosterCharacterIds)
            {
                writer.WriteStringValue(characterId.Value);
            }

            writer.WriteEndArray();
            if (state.CurrentOpportunityCharacterId.HasValue)
            {
                writer.WriteString(
                    "currentOpportunityCharacterId",
                    state.CurrentOpportunityCharacterId.Value.Value);
            }
            else
            {
                writer.WriteNull("currentOpportunityCharacterId");
            }

            writer.WriteStartArray("records");
            foreach (var record in state.Records)
            {
                writer.WriteStartObject();
                writer.WriteString("id", record.RecordId.Value);
                writer.WriteString("domain", DomainToken(record.Domain));
                writer.WriteString("lifecycle", record.Lifecycle == ProductionRecordLifecycle.Active ? "active" : "inactive");
                writer.WriteString(
                    "protection",
                    record.Protection switch
                    {
                        ProductionRecordProtection.None => "none",
                        ProductionRecordProtection.SystemImmutable => "systemImmutable",
                        ProductionRecordProtection.CreatorLocked => "creatorLocked",
                        _ => throw new InvalidOperationException("Unexpected Production protection.")
                    });

                switch (record)
                {
                    case CharacterProductionRecord character:
                        writer.WriteString("subjectCharacterId", character.SubjectCharacterId.Value);
                        writer.WriteNull("targetCharacterId");
                        break;
                    case RelationshipProductionRecord relationship:
                        writer.WriteString("subjectCharacterId", relationship.SubjectCharacterId.Value);
                        writer.WriteString("targetCharacterId", relationship.TargetCharacterId.Value);
                        break;
                    default:
                        writer.WriteNull("subjectCharacterId");
                        writer.WriteNull("targetCharacterId");
                        break;
                }

                writer.WriteString("text", record.Text);
                writer.WriteStartArray("provenance");
                foreach (var sourceId in record.Provenance)
                {
                    writer.WriteStringValue(sourceId.Value);
                }

                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static string DomainToken(ProductionRecordDomain domain) =>
        domain switch
        {
            ProductionRecordDomain.HistoricalTruth => "historicalTruth",
            ProductionRecordDomain.UnresolvedProposition => "unresolvedProposition",
            ProductionRecordDomain.WorldState => "worldState",
            ProductionRecordDomain.SceneState => "sceneState",
            ProductionRecordDomain.CharacterConstitution => "characterConstitution",
            ProductionRecordDomain.CharacterDisposition => "characterDisposition",
            ProductionRecordDomain.CharacterCircumstance => "characterCircumstance",
            ProductionRecordDomain.CharacterObservation => "characterObservation",
            ProductionRecordDomain.CharacterKnowledge => "characterKnowledge",
            ProductionRecordDomain.CharacterBelief => "characterBelief",
            ProductionRecordDomain.CharacterSuspicion => "characterSuspicion",
            ProductionRecordDomain.CharacterMemory => "characterMemory",
            ProductionRecordDomain.CharacterGoal => "characterGoal",
            ProductionRecordDomain.CharacterClaim => "characterClaim",
            ProductionRecordDomain.Relationship => "relationship",
            ProductionRecordDomain.Pressure => "pressure",
            _ => throw new InvalidOperationException("Unexpected Production domain.")
        };

    private static HashSet<string> PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);
}
