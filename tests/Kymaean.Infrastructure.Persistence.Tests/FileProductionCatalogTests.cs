using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class FileProductionCatalogTests
{
    [TestMethod]
    public void EmptyCatalogListsSuccessfully()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);

        var result = catalog.ListProductions();

        Assert.IsTrue(result.IsSuccess);
        Assert.HasCount(0, result.Value);
        Assert.IsTrue(Directory.Exists(CatalogDirectory(directory.Path)));
    }

    [TestMethod]
    public void UnrelatedSiblingUnderApplicationRootIsIgnored()
    {
        using var directory = new TestDirectory();
        Directory.CreateDirectory(Path.Combine(directory.Path, "settings"));
        var catalog = new FileProductionCatalog(directory.Path);

        var result = catalog.ListProductions();

        Assert.IsTrue(result.IsSuccess);
        Assert.HasCount(0, result.Value);
    }

    [TestMethod]
    public void ExactIdentityMetadataPreservesFullDemonstratedApplicationDomain()
    {
        using var directory = new TestDirectory();
        var values = new[]
        {
            new string('x', 200),
            new string(new[] { '\uD800' }),
            "a/b\\c:*?\"<>|\0",
            "CaseSensitive",
            "casesensitive",
        };

        for (var index = 0; index < values.Length; index++)
        {
            CreateProduction(
                directory.Path,
                index + 1,
                new ProductionId(values[index]),
                $"Production {index}");
        }

        var result = new FileProductionCatalog(directory.Path).ListProductions();

        Assert.IsTrue(result.IsSuccess);
        Assert.HasCount(values.Length, result.Value);
        CollectionAssert.AreEquivalent(
            values,
            result.Value.Select(summary => summary.Id.Value).ToArray());
    }

    [TestMethod]
    public void OpenUsesExactIdentityRatherThanLocatorOrProductionName()
    {
        using var directory = new TestDirectory();
        var productionId = new ProductionId(new string('q', 200));
        CreateProduction(
            directory.Path,
            7,
            productionId,
            "The Glass Harbor");

        var result = new FileProductionCatalog(directory.Path)
            .OpenProduction(productionId);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("The Glass Harbor", result.Value.ProductionName);
    }

    [TestMethod]
    public void UnknownIdFailsWithoutCreatingProductionStorage()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var before = Directory.GetFileSystemEntries(
            CatalogDirectory(directory.Path));

        var result = catalog.OpenProduction(new ProductionId("missing"));

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        CollectionAssert.AreEqual(
            before,
            Directory.GetFileSystemEntries(CatalogDirectory(directory.Path)));
    }

    [TestMethod]
    public void MalformedLocatorFailsWholeCatalog()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var entry = Path.Combine(
            CatalogDirectory(directory.Path),
            "entry-NOT-CANONICAL");
        Directory.CreateDirectory(entry);

        var result = catalog.ListProductions();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void NonDirectoryArtifactInsideOwnedNamespaceFailsWholeCatalog()
    {
        using var directory = new TestDirectory();
        _ = new FileProductionCatalog(directory.Path);
        File.WriteAllText(
            Path.Combine(
                CatalogDirectory(directory.Path),
                "entry-00000000000000000000000000000001"),
            "not a directory");

        var result = new FileProductionCatalog(directory.Path).ListProductions();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void DuplicateExactIdentityFailsWholeCatalogAndSelectedOpen()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("duplicate");
        CreateProduction(directory.Path, 1, id, "First");
        CreateProduction(directory.Path, 2, id, "Second");
        var catalog = new FileProductionCatalog(directory.Path);

        var listed = catalog.ListProductions();
        var opened = catalog.OpenProduction(id);

        Assert.IsFalse(listed.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, listed.FailureKind);
        Assert.IsFalse(opened.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, opened.FailureKind);
    }

    [TestMethod]
    public void MissingIdentityMetadataFailsWholeCatalog()
    {
        using var directory = new TestDirectory();
        var entry = CreateEntryDirectory(directory.Path, 1);
        CreateProductionHistory(entry, "Harbor");

        var result = new FileProductionCatalog(directory.Path).ListProductions();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void TruncatedIdentityMetadataMapsToInvalid()
    {
        using var directory = new TestDirectory();
        var entry = CreateEntryDirectory(directory.Path, 1);
        File.WriteAllBytes(
            IdentityPath(entry),
            Encoding.ASCII.GetBytes("KYMIDN01"));
        CreateProductionHistory(entry, "Harbor");

        var result = new FileProductionCatalog(directory.Path).ListProductions();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void FutureIdentityMetadataVersionMapsToIncompatible()
    {
        using var directory = new TestDirectory();
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, new ProductionId("future"), version: 2);
        CreateProductionHistory(entry, "Future");

        var result = new FileProductionCatalog(directory.Path).ListProductions();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
    }

    [TestMethod]
    public void CorruptIdentityMetadataChecksumMapsToInvalid()
    {
        using var directory = new TestDirectory();
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, new ProductionId("corrupt"));
        CreateProductionHistory(entry, "Corrupt");
        var bytes = File.ReadAllBytes(IdentityPath(entry));
        bytes[^1] ^= 0xff;
        File.WriteAllBytes(IdentityPath(entry), bytes);

        var result = new FileProductionCatalog(directory.Path).ListProductions();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void EnvironmentalIdentityReadFailureIsNotCoercedIntoProductResult()
    {
        using var directory = new TestDirectory();
        var entry = CreateEntryDirectory(directory.Path, 1);
        Directory.CreateDirectory(IdentityPath(entry));
        CreateProductionHistory(entry, "Harbor");

        Assert.ThrowsExactly<UnauthorizedAccessException>(
            () => new FileProductionCatalog(directory.Path).ListProductions());
    }

    [TestMethod]
    public void EmptyDiscoveredProductionFailsWholeCatalog()
    {
        using var directory = new TestDirectory();
        var entry = CreateEntryDirectory(directory.Path, 1);
        var id = new ProductionId("empty");
        WriteIdentity(entry, id);
        var catalog = new FileProductionCatalog(directory.Path);

        var listed = catalog.ListProductions();
        var opened = catalog.OpenProduction(id);
        var recovered = catalog.RecoverProduction(id);

        Assert.IsFalse(listed.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, listed.FailureKind);
        Assert.IsFalse(opened.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, opened.FailureKind);
        Assert.IsFalse(recovered.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, recovered.FailureKind);
    }

    [TestMethod]
    public void OpenDoesNotRecoverMissingCommittedHead()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("harbor");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "The Glass Harbor");
        File.Delete(HeadPath(entry));

        var result = new FileProductionCatalog(directory.Path).OpenProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.IsFalse(File.Exists(HeadPath(entry)));
    }

    [TestMethod]
    public void RecoverExplicitlyRestoresValidMissingHead()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("harbor");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "The Glass Harbor");
        File.Delete(HeadPath(entry));
        var catalog = new FileProductionCatalog(directory.Path);

        var recovered = catalog.RecoverProduction(id);
        var reopened = catalog.OpenProduction(id);

        Assert.IsTrue(recovered.IsSuccess);
        Assert.AreEqual("The Glass Harbor", recovered.Value.ProductionName);
        Assert.IsTrue(File.Exists(HeadPath(entry)));
        Assert.IsTrue(reopened.IsSuccess);
    }

    [TestMethod]
    public void FutureEventContractMapsToIncompatible()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("future-event");
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, id);
        new FileProductionJournal(entry).Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.created.v3\",\"productionName\":\"Future\"}"));

        var result = new FileProductionCatalog(directory.Path).OpenProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
    }

    [TestMethod]
    public void CorruptCommittedHistoryMapsToInvalid()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("corrupt-event");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Corrupt Me");
        var entryPath = Directory.GetFiles(entry, "*.kjr").Single();
        var bytes = File.ReadAllBytes(entryPath);
        bytes[^1] ^= 0xff;
        File.WriteAllBytes(entryPath, bytes);

        var result = new FileProductionCatalog(directory.Path).OpenProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void ReplayInvalidHistoryMapsToInvalid()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("invalid-replay");
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, id);
        var journal = new FileProductionJournal(entry);
        journal.Append(CreatedPayload("First"));
        journal.Append(CreatedPayload("Second"));

        var result = new FileProductionCatalog(directory.Path).OpenProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void MissingCatalogNamespacePropagatesEnvironmentalFailure()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        Directory.Delete(CatalogDirectory(directory.Path), recursive: true);

        Assert.ThrowsExactly<DirectoryNotFoundException>(
            () => catalog.ListProductions());
        Assert.ThrowsExactly<DirectoryNotFoundException>(
            () => catalog.OpenProduction(new ProductionId("missing")));
    }

    [TestMethod]
    public void ProductApplicationStartsAndOpensThroughConcreteCatalog()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("application-open");
        CreateProduction(
            directory.Path,
            1,
            id,
            "Application Harbor");

        var started = ProductApplication.Start(
            new FileProductionCatalog(directory.Path));

        Assert.IsTrue(started.IsSuccess);
        Assert.HasCount(1, started.Value.Query().Productions);

        var opened = started.Value.OpenProduction(id);

        Assert.IsTrue(opened.IsSuccess);
        Assert.AreEqual(
            "Application Harbor",
            opened.Value.CurrentProductionReplay!.ProductionName);
    }

    [TestMethod]
    public void ProductApplicationCanExplicitlyRecoverPreviouslyKnownProduction()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("application-recover");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Recovery Harbor");
        var started = ProductApplication.Start(
            new FileProductionCatalog(directory.Path));
        Assert.IsTrue(started.IsSuccess);

        File.Delete(HeadPath(entry));

        var recovered = started.Value.RecoverProduction(id);

        Assert.IsTrue(recovered.IsSuccess);
        Assert.AreEqual(
            "Recovery Harbor",
            recovered.Value.CurrentProductionReplay!.ProductionName);
        Assert.IsTrue(File.Exists(HeadPath(entry)));
    }

    private static string CreateProduction(
        string applicationRoot,
        int locatorSeed,
        ProductionId productionId,
        string productionName)
    {
        var entry = CreateEntryDirectory(applicationRoot, locatorSeed);
        WriteIdentity(entry, productionId);
        CreateProductionHistory(entry, productionName);
        return entry;
    }

    private static string CreateEntryDirectory(
        string applicationRoot,
        int locatorSeed)
    {
        _ = new FileProductionCatalog(applicationRoot);
        var entry = Path.Combine(
            CatalogDirectory(applicationRoot),
            $"entry-{locatorSeed:x32}");
        Directory.CreateDirectory(entry);
        return entry;
    }

    private static void WriteIdentity(
        string entryDirectory,
        ProductionId productionId,
        uint version = 1)
    {
        var value = productionId.Value;
        var content = new byte[
            16 + checked(value.Length * sizeof(ushort))];

        Encoding.ASCII.GetBytes("KYMIDN01")
            .CopyTo(content, 0);
        BinaryPrimitives.WriteUInt32BigEndian(
            content.AsSpan(8, sizeof(uint)),
            version);
        BinaryPrimitives.WriteUInt32BigEndian(
            content.AsSpan(12, sizeof(uint)),
            checked((uint)value.Length));

        var offset = 16;
        foreach (var character in value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                content.AsSpan(offset, sizeof(ushort)),
                character);
            offset += sizeof(ushort);
        }

        var checksum = SHA256.HashData(content);
        var framed = new byte[content.Length + checksum.Length];
        content.CopyTo(framed, 0);
        checksum.CopyTo(framed, content.Length);
        File.WriteAllBytes(IdentityPath(entryDirectory), framed);
    }

    private static void CreateProductionHistory(
        string entryDirectory,
        string productionName)
    {
        var application = new ProductionApplication(
            new FileProductionEventStore(entryDirectory));
        Assert.AreEqual(
            productionName,
            application.Create(productionName).ProductionName);
    }

    private static byte[] CreatedPayload(string productionName) =>
        Encoding.UTF8.GetBytes(
            $"{{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"{productionName}\"}}");

    private static string CatalogDirectory(string applicationRoot) =>
        Path.Combine(applicationRoot, "production-catalog");

    private static string IdentityPath(string entryDirectory) =>
        Path.Combine(entryDirectory, "identity.kid");

    private static string HeadPath(string entryDirectory) =>
        Path.Combine(entryDirectory, ".journal.head");

    private sealed class TestDirectory : IDisposable
    {
        public TestDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Kymaean.Persistence.Catalog.Tests",
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
