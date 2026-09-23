using System.Text;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class ScenePersistenceTests
{
    [TestMethod]
    public void CatalogEstablishesSceneAndReopensRosterInProductionCastOrder()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var production = catalog.CreateProduction("Harbor");
        Assert.IsTrue(production.IsSuccess);
        var first = catalog.CreateCharacter(production.Value.Id, "Marlowe");
        var second = catalog.CreateCharacter(production.Value.Id, "Wren");
        Assert.IsTrue(first.IsSuccess);
        Assert.IsTrue(second.IsSuccess);

        var established = catalog.EstablishScene(
            production.Value.Id,
            new SceneRoster(
                [second.Value.Character.Id, first.Value.Character.Id]));

        Assert.IsTrue(established.IsSuccess);
        CollectionAssert.AreEqual(
            new[] { first.Value.Character.Id, second.Value.Character.Id },
            established.Value.Scene.InitialRoster.CharacterIds.ToArray());

        var reopened = new FileProductionCatalog(directory.Path)
            .OpenProduction(production.Value.Id);
        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            established.Value.Scene,
            reopened.Value.ProductionScenes.Scenes.Single());
    }

    [TestMethod]
    public void SceneEventPreservesExactUtf16IdentityAndRoster()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        var characterValue = new string(new[] { 'C', (char)0xD800 });
        var sceneValue = new string(new[] { 'S', (char)0xDC00 });
        var characterId = new CharacterId(characterValue);
        store.Append(new ProductionCreatedEvent("Harbor"));
        store.Append(new CharacterCreatedEvent(characterId, "Marlowe"));
        store.Append(
            new CreatorEstablishedSceneEvent(
                new SceneId(sceneValue),
                new SceneRoster([characterId])));

        var history = new FileProductionEventStore(directory.Path).LoadAll();
        var established =
            Assert.IsInstanceOfType<CreatorEstablishedSceneEvent>(history[2]);

        CollectionAssert.AreEqual(
            sceneValue.Select(value => (ushort)value).ToArray(),
            established.SceneId.Value.Select(value => (ushort)value).ToArray());
        CollectionAssert.AreEqual(
            characterValue.Select(value => (ushort)value).ToArray(),
            established.InitialRoster.CharacterIds.Single().Value
                .Select(value => (ushort)value)
                .ToArray());
    }

    [TestMethod]
    public void SceneSurvivesSnapshotReopenRecoveryAndPortableExport()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var production = catalog.CreateProduction("Harbor");
        Assert.IsTrue(production.IsSuccess);
        var character = catalog.CreateCharacter(production.Value.Id, "Marlowe");
        Assert.IsTrue(character.IsSuccess);
        var established = catalog.EstablishScene(
            production.Value.Id,
            new SceneRoster([character.Value.Character.Id]));
        Assert.IsTrue(established.IsSuccess);

        var entry = Directory.GetDirectories(
            Path.Combine(directory.Path, "production-catalog"),
            "entry-*").Single();
        var snapshot = Path.Combine(entry, ".projection.snapshot");
        Assert.IsTrue(File.Exists(snapshot));
        var marker = new DateTime(2002, 3, 4, 5, 6, 7, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(snapshot, marker);

        var reopened = catalog.OpenProduction(production.Value.Id);
        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(marker, File.GetLastWriteTimeUtc(snapshot));
        Assert.AreEqual(
            established.Value.Scene,
            reopened.Value.ProductionScenes.Scenes.Single());

        File.Delete(Path.Combine(entry, ".journal.head"));
        var recovered = catalog.RecoverProduction(production.Value.Id);
        Assert.IsTrue(recovered.IsSuccess);
        Assert.AreEqual(
            established.Value.Scene,
            recovered.Value.ProductionScenes.Scenes.Single());

        var exported = new FileProductionExporter(directory.Path)
            .ExportProduction(production.Value.Id);
        Assert.IsTrue(exported.IsSuccess);
        var inspected = ProductionPortableExportInspector.Inspect(
            exported.Value.ToArray());
        Assert.IsTrue(inspected.IsSuccess);
        Assert.AreEqual(
            established.Value.Scene,
            inspected.Value.Projection.ProductionScenes.Scenes.Single());
    }

    [TestMethod]
    public void FutureSceneContractFailsAsCompatibilityError()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"Harbor\"}"));
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.creator-established-scene.v2\",\"sceneIdUtf16Be\":\"AFM=\",\"rosterCharacterIdsUtf16Be\":[]}"));

        var exception =
            Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
                () => new FileProductionEventStore(directory.Path).LoadAll());

        Assert.AreEqual("CreatorEstablishedScene event contract", exception.Artifact);
        Assert.AreEqual(
            "kymaean.production.creator-established-scene.v2",
            exception.FoundIdentifier);
        Assert.AreEqual(
            "kymaean.production.creator-established-scene.v1",
            exception.SupportedIdentifier);
    }

    [TestMethod]
    public void DuplicateRosterPayloadFailsClosed()
    {
        using var directory = new TestDirectory();
        var journal = new FileProductionJournal(directory.Path);
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"Harbor\"}"));
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.character-created.v1\",\"characterIdUtf16Be\":\"AEMALQAx\",\"characterNameUtf16Be\":\"AE0AYQByAGwAbwB3AGU=\"}"));
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.creator-established-scene.v1\",\"sceneIdUtf16Be\":\"AFMALQAx\",\"rosterCharacterIdsUtf16Be\":[\"AEMALQAx\",\"AEMALQAx\"]}"));

        Assert.ThrowsExactly<InvalidDataException>(
            () => new FileProductionEventStore(directory.Path).LoadAll());
    }

    [TestMethod]
    public void DuplicateSceneAndUnknownRosterRejectBeforeJournalMutation()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        var characterId = new CharacterId("C-1");
        var sceneId = new SceneId("S-1");
        store.Append(new ProductionCreatedEvent("Harbor"));
        store.Append(new CharacterCreatedEvent(characterId, "Marlowe"));
        store.Append(
            new CreatorEstablishedSceneEvent(
                sceneId,
                new SceneRoster([characterId])));
        var before = new FileProductionJournal(directory.Path).ReadAll().ToArray();

        Assert.ThrowsExactly<InvalidOperationException>(
            () => store.Append(
                new CreatorEstablishedSceneEvent(
                    sceneId,
                    new SceneRoster([characterId]))));
        Assert.ThrowsExactly<InvalidOperationException>(
            () => store.Append(
                new CreatorEstablishedSceneEvent(
                    new SceneId("S-2"),
                    new SceneRoster([new CharacterId("C-2")]))));

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
                "Kymaean.Scene.Persistence.Tests",
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
