using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class WorldCurrentPersistenceTests
{
    private const string Replacement = "kymaean.production.creator-replaced-world-current-state.v1";

    [TestMethod]
    public void EveryUtf16CodeUnitSurvivesTheRealJournalAndExport()
    {
        var text = new string(Enumerable.Range(0, 65536).Select(value => (char)value).ToArray());
        using var fixture = new Fixture(text);
        var state = State(text);
        Assert.IsTrue(fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, state).IsSuccess);
        var reopened = fixture.Catalog.OpenProduction(fixture.Id);
        AssertUnits(text, reopened.Value.ProductionName);
        AssertState(state, reopened.Value.WorldCurrentState);
        AssertSnapshot(fixture, text, state);
        var export = new FileProductionExporter(fixture.Root).ExportProduction(fixture.Id);
        var inspected = ProductionPortableExportInspector.Inspect(export.Value.ToArray());
        Assert.IsTrue(inspected.IsSuccess);
        AssertUnits(text, inspected.Value.Projection.ProductionName);
        AssertState(state, inspected.Value.Projection.WorldCurrentState);
    }

    public static IEnumerable<object[]> TextCases => new[]
    {
        "\uD800", "\uDC00", "\uD83D\uDE00", "e\u0301", "a\0b", " truth ", "世界",
    }.Select(value => new object[] { value });

    [TestMethod]
    [DynamicData(nameof(TextCases))]
    public void ExactCodeUnitsSurviveCreationReplacementReopenSnapshotAndExport(string text)
    {
        using var fixture = new Fixture(text);
        var state = State(text);
        var result = fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, state);
        Assert.IsTrue(result.IsSuccess);
        AssertUnits(text, result.Value.ProductionName);
        AssertState(state, result.Value.WorldCurrentState);
        Assert.HasCount(2, fixture.Store.LoadAll());
        var reopened = new FileProductionCatalog(fixture.Root).OpenProduction(fixture.Id);
        Assert.IsTrue(reopened.IsSuccess);
        AssertUnits(text, reopened.Value.ProductionName);
        AssertState(state, reopened.Value.WorldCurrentState);
        AssertSnapshot(fixture, text, state);

        var raw = new FileProductionJournal(fixture.Entry).ReadAll();
        using var created = JsonDocument.Parse(raw[0].Payload);
        Assert.AreEqual("kymaean.production.created.v2", created.RootElement.GetProperty("contract").GetString());
        using var replaced = JsonDocument.Parse(raw[1].Payload);
        Assert.AreEqual(Replacement, replaced.RootElement.GetProperty("contract").GetString());
        if (text == "\uD800")
        {
            Assert.AreEqual("2AA=", replaced.RootElement.GetProperty("truthsUtf16Be")[0].GetString());
        }

        var before = Capture(fixture.Entry);
        var export = new FileProductionExporter(fixture.Root).ExportProduction(fixture.Id);
        Assert.IsTrue(export.IsSuccess);
        var package = export.Value.ToArray();
        var offset = 20 + fixture.Id.Value.Length * 2;
        foreach (var entry in raw)
        {
            var length = checked((int)BinaryPrimitives.ReadUInt64BigEndian(package.AsSpan(offset, 8)));
            offset += 8;
            CollectionAssert.AreEqual(entry.Payload.ToArray(), package.AsSpan(offset, length).ToArray());
            offset += length;
        }

        Assert.AreEqual(package.Length - 32, offset); // No cache/layout/credential sections.
        var inspected = ProductionPortableExportInspector.Inspect(package);
        Assert.IsTrue(inspected.IsSuccess);
        Assert.AreEqual(2, inspected.Value.EventCount);
        AssertUnits(text, inspected.Value.Projection.ProductionName);
        AssertState(state, inspected.Value.Projection.WorldCurrentState);
        AssertTree(before, fixture.Entry);
    }

    [TestMethod]
    public void MixedAndEmptyReplacementsAppendOnceEachAndLatestWins()
    {
        using var fixture = new Fixture();
        var states = new[] { State("z", "\uD800", "a", "\uDC00"), State("latest"), WorldCurrentState.Empty };
        var count = 1;
        foreach (var state in states)
        {
            var before = new FileProductionJournal(fixture.Entry).ReadAll();
            var result = fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, state);
            Assert.IsTrue(result.IsSuccess);
            AssertState(state, result.Value.WorldCurrentState);
            var history = fixture.Store.LoadAll();
            Assert.HasCount(++count, history);
            Assert.IsInstanceOfType<CreatorReplacedWorldCurrentStateEvent>(history[^1]);
            var after = new FileProductionJournal(fixture.Entry).ReadAll();
            CollectionAssert.AreEqual(before.Select(x => x.RecordHash).ToArray(), after.Take(before.Count).Select(x => x.RecordHash).ToArray());
            AssertState(state, fixture.Catalog.OpenProduction(fixture.Id).Value.WorldCurrentState);
            var exported = new FileProductionExporter(fixture.Root).ExportProduction(fixture.Id);
            AssertState(state, ProductionPortableExportInspector.Inspect(exported.Value.ToArray()).Value.Projection.WorldCurrentState);
        }
    }

    [TestMethod]
    public void UnknownIdentityCreatesNothingAndNullArgumentsAreRejected()
    {
        using var fixture = new Fixture();
        var before = Capture(fixture.Root);
        var result = fixture.Catalog.ReplaceWorldCurrentState(new ProductionId("missing"), State("truth"));
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        AssertTree(before, fixture.Root);
        Assert.ThrowsExactly<ArgumentNullException>(() => fixture.Catalog.ReplaceWorldCurrentState(null!, State("truth")));
        Assert.ThrowsExactly<ArgumentNullException>(() => fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, null!));
    }

    public static IEnumerable<object[]> InvalidReplacementPayloads => new[]
    {
        "{not-json",
        "{}",
        "{\"contract\":\"" + Replacement + "\"}",
        Payload("null"), Payload("{}"), Payload("[null]"), Payload("[5]"),
        Payload("[\"!\"]"), Payload("[\"AA==\"]"), Payload("[\"\"]"),
        Payload("[\"ACA=\"]"), Payload("[\"AGE=\",\"AGE=\"]"),
        Payload("[]").Replace("}", ",\"extra\":0}"),
        Payload("[]").Replace("}", ",\"truthsUtf16Be\":[]}"),
        Payload("[]").Replace("}", ",\"contract\":\"" + Replacement + "\"}"),
        Payload("[\"\\uD800\"]"),
    }.Select(value => new object[] { value });

    [TestMethod]
    [DynamicData(nameof(InvalidReplacementPayloads))]
    public void MalformedReplacementFailsClosedWithoutFurtherAppend(string payload)
    {
        using var fixture = new Fixture();
        new FileProductionJournal(fixture.Entry).Append(Encoding.UTF8.GetBytes(payload));
        var before = Capture(fixture.Entry);
        Assert.ThrowsExactly<InvalidDataException>(() => fixture.Store.LoadAll());
        Assert.AreEqual(ProductAccessFailureKind.Invalid,
            fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("next")).FailureKind);
        AssertTree(before, fixture.Entry);
    }

    [TestMethod]
    [DataRow("kymaean.production.creator-replaced-world-current-state.v2")]
    [DataRow("kymaean.production.created.v3")]
    public void FutureFamiliesAreIncompatibleAndNeverAppend(string contract)
    {
        using var fixture = new Fixture();
        new FileProductionJournal(fixture.Entry).Append(Encoding.UTF8.GetBytes("{\"contract\":\"" + contract + "\"}"));
        var before = Capture(fixture.Entry);
        Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(() => fixture.Store.LoadAll());
        Assert.AreEqual(ProductAccessFailureKind.Incompatible,
            fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("next")).FailureKind);
        AssertTree(before, fixture.Entry);
    }

    [TestMethod]
    [DataRow("{\"contract\":\"kymaean.production.created.v2\"}")]
    [DataRow("{\"contract\":\"kymaean.production.created.v2\",\"productionNameUtf16Be\":\"AA==\"}")]
    [DataRow("{\"contract\":\"kymaean.production.created.v2\",\"productionNameUtf16Be\":\"!\"}")]
    [DataRow("{\"contract\":\"kymaean.production.created.v2\",\"productionNameUtf16Be\":\"AGE=\",\"extra\":1}")]
    [DataRow("{\"contract\":\"kymaean.production.created.v2\",\"productionNameUtf16Be\":\"AGE=\",\"productionNameUtf16Be\":\"AGI=\"}")]
    public void MalformedCreationV2IsInvalid(string payload)
    {
        using var fixture = new Fixture(create: false);
        new FileProductionJournal(fixture.Entry).Append(Encoding.UTF8.GetBytes(payload));
        Assert.ThrowsExactly<InvalidDataException>(() => fixture.Store.LoadAll());
    }

    [TestMethod]
    public void LegacyCreationRemainsReadableAndPreviouslyLostInformationIsNotInvented()
    {
        using var fixture = new Fixture(create: false);
        new FileProductionJournal(fixture.Entry).Append(Encoding.UTF8.GetBytes(
            "{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"Old\\uFFFD\"}"));
        AssertUnits("Old\uFFFD", ProductionReplay.Rebuild(fixture.Store.LoadAll()).ProductionName);
        Assert.IsTrue(fixture.Catalog.OpenProduction(fixture.Id).Value.WorldCurrentState.IsEmpty);
        Assert.IsTrue(fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("\uD800")).IsSuccess);
        var export = new FileProductionExporter(fixture.Root).ExportProduction(fixture.Id);
        var inspected = ProductionPortableExportInspector.Inspect(export.Value.ToArray());
        Assert.IsTrue(inspected.IsSuccess);
        AssertUnits("Old\uFFFD", inspected.Value.Projection.ProductionName);
        AssertState(State("\uD800"), inspected.Value.Projection.WorldCurrentState);
    }

    [TestMethod]
    public void WriteNeverAutoRecoversAndRecoveryRefreshesFullSnapshot()
    {
        using var fixture = new Fixture();
        Assert.IsTrue(fixture.Catalog.OpenProduction(fixture.Id).IsSuccess);
        var head = File.ReadAllBytes(fixture.Head);
        fixture.Store.Append(new CreatorReplacedWorldCurrentStateEvent(State("\uD800")));
        File.WriteAllBytes(fixture.Head, head); // Complete but uncommitted suffix.
        var before = Capture(fixture.Entry);
        Assert.AreEqual(ProductAccessFailureKind.Invalid,
            fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("other")).FailureKind);
        AssertTree(before, fixture.Entry);
        var recovered = fixture.Catalog.RecoverProduction(fixture.Id);
        Assert.IsTrue(recovered.IsSuccess);
        AssertState(State("\uD800"), recovered.Value.WorldCurrentState);
        AssertSnapshot(fixture, "Harbor", State("\uD800"));
    }

    [TestMethod]
    public void ReplayInvalidHistoryCannotProduceSuccessfulWrite()
    {
        using var fixture = new Fixture(create: false);
        new FileProductionJournal(fixture.Entry).Append(Encoding.UTF8.GetBytes(Payload("[]")));
        var before = Capture(fixture.Entry);
        Assert.AreEqual(ProductAccessFailureKind.Invalid,
            fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("other")).FailureKind);
        AssertTree(before, fixture.Entry);
    }

    [TestMethod]
    public void ContentionRemainsEnvironmentalAndOtherProductionIsIndependent()
    {
        using var fixture = new Fixture();
        using var other = new Fixture();
        using (var gate = new FileStream(Path.Combine(fixture.Entry, ".journal.lock"), FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            Assert.ThrowsExactly<IOException>(() => fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("blocked")));
            Assert.IsTrue(other.Catalog.ReplaceWorldCurrentState(other.Id, State("independent")).IsSuccess);
        }

        Assert.HasCount(1, fixture.Store.LoadAll());
        Assert.IsTrue(fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("after release")).IsSuccess);
        Assert.HasCount(2, fixture.Store.LoadAll());
    }

    [TestMethod]
    public void RealApplicationWriterPreservesPriorProjectionOnTypedFailure()
    {
        using var fixture = new Fixture();
        var app = ProductApplication.Start(fixture.Catalog).Value;
        Assert.IsTrue(app.OpenProduction(fixture.Id, ProductSpace.Studio).IsSuccess);
        Assert.IsTrue(app.ReplaceWorldCurrentState(State("usable")).IsSuccess);
        var before = app.Query();
        File.WriteAllBytes(fixture.Head, new byte[] { 1, 2, 3 });
        var result = app.ReplaceWorldCurrentState(State("failed"));
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.AreEqual(before, app.Query());
    }

    [TestMethod]
    public void SnapshotIsCompleteReusableAndOldOrInvalidCacheCannotEraseState()
    {
        using var fixture = new Fixture();
        var state = State("\uD800", "\uDC00", " second ");
        Assert.IsTrue(fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, state).IsSuccess);
        var snapshot = File.ReadAllBytes(fixture.Snapshot);
        var marker = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(fixture.Snapshot, marker);
        Assert.IsTrue(fixture.Catalog.OpenProduction(fixture.Id).IsSuccess);
        Assert.AreEqual(marker, File.GetLastWriteTimeUtc(fixture.Snapshot));
        AssertSnapshot(fixture, "Harbor", state);

        // A valid v1 frame contains just the name, even at the current journal anchor.
        var nameEnd = 56 + "Harbor".Length * 2;
        var legacy = snapshot.AsSpan(0, nameEnd).ToArray();
        BinaryPrimitives.WriteUInt32BigEndian(legacy.AsSpan(8, 4), 1);
        File.WriteAllBytes(fixture.Snapshot, Frame(legacy));
        AssertState(state, fixture.Catalog.OpenProduction(fixture.Id).Value.WorldCurrentState);
        AssertSnapshot(fixture, "Harbor", state);

        // A checksummed v3 cache with an invalid name is still just a cache miss.
        var invalid = snapshot[..^32];
        for (var offset = 56; offset < nameEnd; offset += 2)
        {
            BinaryPrimitives.WriteUInt16BigEndian(invalid.AsSpan(offset, 2), ' ');
        }

        File.WriteAllBytes(fixture.Snapshot, Frame(invalid));
        AssertState(state, fixture.Catalog.OpenProduction(fixture.Id).Value.WorldCurrentState);
        AssertSnapshot(fixture, "Harbor", state);
        File.WriteAllBytes(fixture.Head, new byte[] { 1 });
        var before = File.ReadAllBytes(fixture.Snapshot);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, fixture.Catalog.RecoverProduction(fixture.Id).FailureKind);
        CollectionAssert.AreEqual(before, File.ReadAllBytes(fixture.Snapshot));
    }

    [TestMethod]
    public void CacheWriteFailureDoesNotInvalidateCommittedReplacement()
    {
        using var fixture = new Fixture();
        Directory.CreateDirectory(fixture.Snapshot);
        Assert.IsTrue(fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("committed")).IsSuccess);
        AssertState(State("committed"), ProductionReplay.Rebuild(fixture.Store.LoadAll()).WorldCurrentState);
    }

    [TestMethod]
    [DataRow("kymaean.production.creator-replaced-world-current-state.v2", "[]", ProductAccessFailureKind.Incompatible)]
    [DataRow(Replacement, "[\"AA==\"]", ProductAccessFailureKind.Invalid)]
    [DataRow(Replacement, "[\"!\"]", ProductAccessFailureKind.Invalid)]
    public void PortableInspectorClassifiesInvalidAndFutureReplacementPayloads(
        string contract, string truths, ProductAccessFailureKind expected)
    {
        using var fixture = new Fixture();
        Assert.IsTrue(fixture.Catalog.ReplaceWorldCurrentState(fixture.Id, State("truth")).IsSuccess);
        var export = new FileProductionExporter(fixture.Root).ExportProduction(fixture.Id).Value.ToArray();
        var offset = 20 + fixture.Id.Value.Length * 2;
        var creationLength = checked((int)BinaryPrimitives.ReadUInt64BigEndian(export.AsSpan(offset, 8)));
        offset += 8 + creationLength;
        var payload = Encoding.UTF8.GetBytes(Payload(truths).Replace(Replacement, contract, StringComparison.Ordinal));
        var content = new byte[offset + 8 + payload.Length];
        export.AsSpan(0, offset).CopyTo(content);
        BinaryPrimitives.WriteUInt64BigEndian(content.AsSpan(offset, 8), (ulong)payload.Length);
        payload.CopyTo(content, offset + 8);
        Assert.AreEqual(expected, ProductionPortableExportInspector.Inspect(Frame(content)).FailureKind);
    }

    [TestMethod]
    public void ReplacementEncodingIsDeterministicAcrossInputOrdering()
    {
        using var left = new Fixture();
        using var right = new Fixture();
        Assert.IsTrue(left.Catalog.ReplaceWorldCurrentState(left.Id, State("b", "\uD800", "a")).IsSuccess);
        Assert.IsTrue(right.Catalog.ReplaceWorldCurrentState(right.Id, State("a", "b", "\uD800")).IsSuccess);
        CollectionAssert.AreEqual(
            new FileProductionJournal(left.Entry).ReadAll()[1].Payload.ToArray(),
            new FileProductionJournal(right.Entry).ReadAll()[1].Payload.ToArray());
    }

    private static string Payload(string truths) => "{\"contract\":\"" + Replacement + "\",\"truthsUtf16Be\":" + truths + "}";
    private static WorldCurrentState State(params string[] values) => new(values.Select(value => new WorldCurrentTruth(value)));

    private static void AssertUnits(string expected, string actual)
    {
        Assert.AreEqual(expected.Length, actual.Length);
        for (var index = 0; index < expected.Length; index++)
        {
            Assert.AreEqual((ushort)expected[index], (ushort)actual[index]);
        }
    }

    private static void AssertState(WorldCurrentState expected, WorldCurrentState actual)
    {
        Assert.AreEqual(expected.Truths.Length, actual.Truths.Length);
        for (var index = 0; index < expected.Truths.Length; index++)
        {
            AssertUnits(expected.Truths[index].Text, actual.Truths[index].Text);
        }
    }

    private static void AssertSnapshot(Fixture fixture, string name, WorldCurrentState state)
    {
        var bytes = File.ReadAllBytes(fixture.Snapshot);
        Assert.AreEqual(5U, BinaryPrimitives.ReadUInt32BigEndian(bytes.AsSpan(8, 4)));
        CollectionAssert.AreEqual(SHA256.HashData(bytes.AsSpan(0, bytes.Length - 32)), bytes[^32..]);
        var offset = 52;
        AssertUnits(name, ReadSnapshotText(bytes, ref offset));
        var count = BinaryPrimitives.ReadUInt32BigEndian(bytes.AsSpan(offset, 4));
        offset += 4;
        Assert.AreEqual((uint)state.Truths.Length, count);
        foreach (var truth in state.Truths)
        {
            AssertUnits(truth.Text, ReadSnapshotText(bytes, ref offset));
        }

        var characterCount =
            BinaryPrimitives.ReadUInt32BigEndian(bytes.AsSpan(offset, 4));
        offset += 4;
        Assert.AreEqual(0U, characterCount);
        var sceneCount =
            BinaryPrimitives.ReadUInt32BigEndian(bytes.AsSpan(offset, 4));
        offset += 4;
        Assert.AreEqual(0U, sceneCount);
        var performanceCount =
            BinaryPrimitives.ReadUInt32BigEndian(bytes.AsSpan(offset, 4));
        offset += 4;
        Assert.AreEqual(0U, performanceCount);
        Assert.AreEqual(bytes.Length - 32, offset);
    }

    private static string ReadSnapshotText(byte[] bytes, ref int offset)
    {
        var count = checked((int)BinaryPrimitives.ReadUInt32BigEndian(bytes.AsSpan(offset, 4)));
        offset += 4;
        var chars = new char[count];
        for (var index = 0; index < count; index++)
        {
            chars[index] = (char)BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(offset, 2));
            offset += 2;
        }

        return new string(chars);
    }

    private static Dictionary<string, byte[]> Capture(string root) => Directory.GetFiles(root, "*", SearchOption.AllDirectories).ToDictionary(path => path, File.ReadAllBytes);
    private static void AssertTree(Dictionary<string, byte[]> before, string root)
    {
        var after = Capture(root);
        CollectionAssert.AreEquivalent(before.Keys.ToArray(), after.Keys.ToArray());
        foreach (var pair in before) { CollectionAssert.AreEqual(pair.Value, after[pair.Key]); }
    }

    private static byte[] Frame(byte[] content) => content.Concat(SHA256.HashData(content)).ToArray();

    private sealed class Fixture : IDisposable
    {
        public Fixture(string name = "Harbor", bool create = true)
        {
            Catalog = new FileProductionCatalog(Root);
            Directory.CreateDirectory(Entry);
            var content = new byte[18];
            "KYMIDN01"u8.CopyTo(content);
            BinaryPrimitives.WriteUInt32BigEndian(content.AsSpan(8, 4), 1);
            BinaryPrimitives.WriteUInt32BigEndian(content.AsSpan(12, 4), 1);
            BinaryPrimitives.WriteUInt16BigEndian(content.AsSpan(16, 2), 'p');
            File.WriteAllBytes(Path.Combine(Entry, "identity.kid"), Frame(content));
            Store = new FileProductionEventStore(Entry);
            if (create) { Store.Append(new ProductionCreatedEvent(name)); }
        }

        public string Root { get; } = Path.Combine(Path.GetTempPath(), "kymaean-world-tests", Guid.NewGuid().ToString("N"));
        public string Entry => Path.Combine(Root, "production-catalog", "entry-00000000000000000000000000000001");
        public string Head => Path.Combine(Entry, ".journal.head");
        public string Snapshot => Path.Combine(Entry, ".projection.snapshot");
        public ProductionId Id { get; } = new("p");
        public FileProductionCatalog Catalog { get; }
        public FileProductionEventStore Store { get; }
        public void Dispose() => Directory.Delete(Root, recursive: true);
    }
}
