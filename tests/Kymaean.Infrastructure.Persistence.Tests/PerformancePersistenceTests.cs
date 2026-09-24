using System.Text;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class PerformancePersistenceTests
{
    [TestMethod]
    public void AcceptedPerformanceSurvivesReopenRecoverySnapshotAndPortableExport()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var production = catalog.CreateProduction("Harbor");
        Assert.IsTrue(production.IsSuccess);
        var marlowe = catalog.CreateCharacter(production.Value.Id, "Marlowe");
        var wren = catalog.CreateCharacter(production.Value.Id, "Wren");
        Assert.IsTrue(marlowe.IsSuccess);
        Assert.IsTrue(wren.IsSuccess);
        var scene = catalog.EstablishScene(
            production.Value.Id,
            new SceneRoster(
                [marlowe.Value.Character.Id, wren.Value.Character.Id]));
        Assert.IsTrue(scene.IsSuccess);

        var accepted = new AcceptedPerformance(
            scene.Value.Scene.Id,
            marlowe.Value.Character.Id,
            "I brace the door.",
            new CharacterCircumstance(
                "Marlowe is braced against the door."));

        var committed = catalog.CommitAcceptedPerformance(
            production.Value.Id,
            accepted);

        Assert.IsTrue(committed.IsSuccess);
        Assert.AreEqual(
            accepted,
            committed.Value.AcceptedPerformanceHistory.Performances.Single());

        var entry = Directory.GetDirectories(
            Path.Combine(directory.Path, "production-catalog"),
            "entry-*").Single();
        var snapshot = Path.Combine(entry, ".projection.snapshot");
        Assert.IsTrue(File.Exists(snapshot));
        var marker = new DateTime(
            2003, 4, 5, 6, 7, 8, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(snapshot, marker);

        var reopened = new FileProductionCatalog(directory.Path)
            .OpenProduction(production.Value.Id);
        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(marker, File.GetLastWriteTimeUtc(snapshot));
        Assert.AreEqual(
            accepted,
            reopened.Value.AcceptedPerformanceHistory.Performances.Single());

        File.Delete(Path.Combine(entry, ".journal.head"));
        var recovered = catalog.RecoverProduction(production.Value.Id);
        Assert.IsTrue(recovered.IsSuccess);
        Assert.AreEqual(
            accepted,
            recovered.Value.AcceptedPerformanceHistory.Performances.Single());

        var exported = new FileProductionExporter(directory.Path)
            .ExportProduction(production.Value.Id);
        Assert.IsTrue(exported.IsSuccess);
        var inspected = ProductionPortableExportInspector.Inspect(
            exported.Value.ToArray());
        Assert.IsTrue(inspected.IsSuccess);
        Assert.AreEqual(5, inspected.Value.EventCount);
        Assert.AreEqual(
            accepted,
            inspected.Value.Projection.AcceptedPerformanceHistory
                .Performances.Single());
    }

    [TestMethod]
    public void FutureAcceptedPerformanceContractFailsAsCompatibilityError()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        store.Append(new ProductionCreatedEvent("Harbor"));

        new FileProductionJournal(directory.Path).Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.accepted-performance-committed.v2\"}"));

        var exception =
            Assert.ThrowsExactly<ProductionPersistenceCompatibilityException>(
                () => new FileProductionEventStore(directory.Path).LoadAll());

        Assert.AreEqual(
            "AcceptedPerformanceCommitted event contract",
            exception.Artifact);
        Assert.AreEqual(
            "kymaean.production.accepted-performance-committed.v2",
            exception.FoundIdentifier);
        Assert.AreEqual(
            "kymaean.production.accepted-performance-committed.v1",
            exception.SupportedIdentifier);
    }

    [TestMethod]
    public void PerformanceOutsideEstablishedRosterIsRejectedBeforeJournalMutation()
    {
        using var directory = new TestDirectory();
        var store = new FileProductionEventStore(directory.Path);
        var characterId = new CharacterId("C-1");
        store.Append(new ProductionCreatedEvent("Harbor"));
        store.Append(new CharacterCreatedEvent(characterId, "Marlowe"));
        var before = new FileProductionJournal(directory.Path)
            .ReadAll()
            .ToArray();

        Assert.ThrowsExactly<InvalidOperationException>(
            () => store.Append(
                new AcceptedPerformanceCommittedEvent(
                    new AcceptedPerformance(
                        new SceneId("S-404"),
                        characterId,
                        "I wait.",
                        new CharacterCircumstance(
                            "Marlowe is waiting.")))));

        var after = new FileProductionJournal(directory.Path)
            .ReadAll()
            .ToArray();
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
                "Kymaean.Performance.Persistence.Tests",
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
