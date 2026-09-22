using System.Text;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class CharacterPersistenceTests
{
    [TestMethod]
    public void CatalogCreatesStableCharactersAllowsDuplicateNamesAndReopensCast()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var production = catalog.CreateProduction("Harbor");
        Assert.IsTrue(production.IsSuccess);

        var first = catalog.CreateCharacter(production.Value.Id, "Marlowe");
        var second = catalog.CreateCharacter(production.Value.Id, "Marlowe");

        Assert.IsTrue(first.IsSuccess);
        Assert.IsTrue(second.IsSuccess);
        Assert.AreNotEqual(first.Value.Character.Id, second.Value.Character.Id);

        var reopened = new FileProductionCatalog(directory.Path)
            .OpenProduction(production.Value.Id);
        Assert.IsTrue(reopened.IsSuccess);
        CollectionAssert.AreEqual(
            new[] { first.Value.Character, second.Value.Character },
            reopened.Value.ProductionCast.Characters.ToArray());
    }

    [TestMethod]
    public void CharacterEventPreservesExactUtf16IdentityAndName()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        store.Append(new ProductionCreatedEvent("Harbor"));

        var idValue = new string(new[] { 'I', (char)0xD800, 'D' });
        var name = new string(new[] { ' ', (char)0xDC00, ' ' });
        store.Append(
            new CharacterCreatedEvent(
                new CharacterId(idValue),
                name));

        var history = new FileProductionEventStore(directory.Path).LoadAll();
        var character =
            Assert.IsInstanceOfType<CharacterCreatedEvent>(history[1]);

        CollectionAssert.AreEqual(
            idValue.Select(value => (ushort)value).ToArray(),
            character.CharacterId.Value.Select(value => (ushort)value).ToArray());
        CollectionAssert.AreEqual(
            name.Select(value => (ushort)value).ToArray(),
            character.CharacterName.Select(value => (ushort)value).ToArray());
    }

    [TestMethod]
    public void CharacterCastSurvivesRecoverySnapshotAndPortableExport()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var production = catalog.CreateProduction("Harbor");
        Assert.IsTrue(production.IsSuccess);

        var name = new string(new[] { ' ', (char)0xD800, ' ' });
        var created = catalog.CreateCharacter(production.Value.Id, name);
        Assert.IsTrue(created.IsSuccess);

        var entry = Directory.GetDirectories(
            Path.Combine(directory.Path, "production-catalog"),
            "entry-*").Single();
        var snapshotPath = Path.Combine(entry, ".projection.snapshot");
        Assert.IsTrue(File.Exists(snapshotPath));

        var marker = new DateTime(
            2001, 2, 3, 4, 5, 6, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(snapshotPath, marker);

        var reopened = catalog.OpenProduction(production.Value.Id);
        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(marker, File.GetLastWriteTimeUtc(snapshotPath));
        Assert.AreEqual(
            created.Value.Character,
            reopened.Value.ProductionCast.Characters.Single());

        File.Delete(Path.Combine(entry, ".journal.head"));
        var recovered = catalog.RecoverProduction(production.Value.Id);
        Assert.IsTrue(recovered.IsSuccess);
        Assert.AreEqual(
            created.Value.Character,
            recovered.Value.ProductionCast.Characters.Single());

        var exported = new FileProductionExporter(directory.Path)
            .ExportProduction(production.Value.Id);
        Assert.IsTrue(exported.IsSuccess);
        var inspected = ProductionPortableExportInspector.Inspect(
            exported.Value.ToArray());
        Assert.IsTrue(inspected.IsSuccess);
        Assert.AreEqual(
            created.Value.Character,
            inspected.Value.Projection.ProductionCast.Characters.Single());
        CollectionAssert.AreEqual(
            name.Select(value => (ushort)value).ToArray(),
            inspected.Value.Projection.ProductionCast.Characters.Single()
                .CharacterName.Select(value => (ushort)value).ToArray());
    }

    [TestMethod]
    public void FutureCharacterContractFailsAsCompatibilityError()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"Harbor\"}"));
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.character-created.v2\",\"characterIdUtf16Be\":\"AEMA\",\"characterNameUtf16Be\":\"AE0A\"}"));

        var exception =
            Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
                () => new FileProductionEventStore(directory.Path).LoadAll());

        Assert.AreEqual("CharacterCreated event contract", exception.Artifact);
        Assert.AreEqual(
            "kymaean.production.character-created.v2",
            exception.FoundIdentifier);
        Assert.AreEqual(
            "kymaean.production.character-created.v1",
            exception.SupportedIdentifier);
    }

    [TestMethod]
    public void DuplicateCharacterIdentityIsRejectedBeforeJournalMutation()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        var id = new CharacterId("C-1");
        store.Append(new ProductionCreatedEvent("Harbor"));
        store.Append(new CharacterCreatedEvent(id, "Marlowe"));
        var before = new FileProductionJournal(directory.Path).ReadAll().ToArray();

        Assert.ThrowsExactly<InvalidOperationException>(
            () => store.Append(new CharacterCreatedEvent(id, "Wren")));

        var after = new FileProductionJournal(directory.Path).ReadAll().ToArray();
        Assert.AreEqual(before.Length, after.Length);
        for (var index = 0; index < before.Length; index++)
        {
            Assert.AreEqual(before[index].Sequence, after[index].Sequence);
            Assert.AreEqual(before[index].RecordHash, after[index].RecordHash);
        }
    }

    private sealed class TestDirectory : IDisposable
    {
        public TestDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Kymaean.Character.Persistence.Tests",
                Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(Path))
                {
                    Directory.Delete(Path, recursive: true);
                }
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }
}
