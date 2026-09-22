using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class FileProductionCatalogCreationTests
{
    [TestMethod]
    public void CreatesOpaqueStableIdentitiesAndAllowsDuplicateNames()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);

        var first = catalog.CreateProduction("Same Name");
        var second = catalog.CreateProduction("Same Name");

        Assert.IsTrue(first.IsSuccess);
        Assert.IsTrue(second.IsSuccess);
        Assert.AreNotEqual(first.Value.Id, second.Value.Id);
        Assert.AreEqual("Same Name", first.Value.Replay.ProductionName);
        Assert.AreEqual("Same Name", second.Value.Replay.ProductionName);
        Assert.IsTrue(first.Value.Replay.WorldCurrentState.IsEmpty);
        Assert.IsTrue(second.Value.Replay.WorldCurrentState.IsEmpty);

        var listed = new FileProductionCatalog(directory.Path)
            .ListProductions();
        Assert.IsTrue(listed.IsSuccess);
        Assert.HasCount(2, listed.Value);
        Assert.AreEqual(2, listed.Value.Count(
            item => item.ProductionName == "Same Name"));

        Assert.IsTrue(
            new FileProductionCatalog(directory.Path)
                .OpenProduction(first.Value.Id)
                .IsSuccess);
        Assert.IsTrue(
            new FileProductionCatalog(directory.Path)
                .OpenProduction(second.Value.Id)
                .IsSuccess);

        AssertNoPendingCreation(directory.Path);
    }

    [TestMethod]
    public void CreatePreservesExactAcceptedUtf16Name()
    {
        using var directory = new TestDirectory();
        var name = new string(new[] { ' ', (char)0xD800, ' ' });
        var catalog = new FileProductionCatalog(directory.Path);

        var created = catalog.CreateProduction(name);
        Assert.IsTrue(created.IsSuccess);
        var opened = catalog.OpenProduction(created.Value.Id);
        var listed = catalog.ListProductions();

        Assert.IsTrue(opened.IsSuccess);
        Assert.IsTrue(listed.IsSuccess);
        AssertCodeUnits(name, created.Value.Replay.ProductionName);
        AssertCodeUnits(name, opened.Value.ProductionName);
        AssertCodeUnits(
            name,
            listed.Value.Single(
                item => item.Id == created.Value.Id).ProductionName);
    }

    [TestMethod]
    public void CreationOnlyReplayStartsWithEmptyWorldState()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);

        var created = catalog.CreateProduction("Harbor");

        Assert.IsTrue(created.IsSuccess);
        Assert.IsTrue(created.Value.Replay.WorldCurrentState.IsEmpty);

        var reopened = new FileProductionCatalog(directory.Path)
            .OpenProduction(created.Value.Id);
        Assert.IsTrue(reopened.IsSuccess);
        Assert.IsTrue(reopened.Value.WorldCurrentState.IsEmpty);
    }

    [TestMethod]
    public void InvalidCatalogPreventsCreationWithoutPublishingAnotherEntry()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        Directory.CreateDirectory(
            Path.Combine(
                CatalogDirectory(directory.Path),
                "entry-NOT-CANONICAL"));
        var before = Directory.GetFileSystemEntries(
            CatalogDirectory(directory.Path));

        var result = catalog.CreateProduction("New");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        CollectionAssert.AreEqual(
            before,
            Directory.GetFileSystemEntries(CatalogDirectory(directory.Path)));
        AssertNoPendingCreation(directory.Path);
    }

    [TestMethod]
    public void IncompatibleCatalogPreventsCreationWithoutPublishingAnotherEntry()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var entry = Path.Combine(
            CatalogDirectory(directory.Path),
            "entry-00000000000000000000000000000001");
        Directory.CreateDirectory(entry);
        WriteIdentity(
            entry,
            new ProductionId("future"),
            version: 2);
        _ = new ProductionApplication(
            new FileProductionEventStore(entry))
            .Create("Future");
        var before = Directory.GetFileSystemEntries(
            CatalogDirectory(directory.Path));

        var result = catalog.CreateProduction("New");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
        CollectionAssert.AreEqual(
            before,
            Directory.GetFileSystemEntries(CatalogDirectory(directory.Path)));
        AssertNoPendingCreation(directory.Path);
    }

    [TestMethod]
    public void InvalidCreationNameMutatesNothing()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var before = Directory.GetFileSystemEntries(
            CatalogDirectory(directory.Path));

        Assert.ThrowsExactly<ArgumentException>(
            () => catalog.CreateProduction("   "));

        CollectionAssert.AreEqual(
            before,
            Directory.GetFileSystemEntries(CatalogDirectory(directory.Path)));
        AssertNoPendingCreation(directory.Path);
    }

    private static string CatalogDirectory(string root) =>
        Path.Combine(root, "production-catalog");

    private static void AssertNoPendingCreation(string root) =>
        Assert.HasCount(
            0,
            Directory.EnumerateDirectories(
                root,
                ".production-create-*",
                SearchOption.TopDirectoryOnly).ToArray());

    private static void AssertCodeUnits(string expected, string actual) =>
        CollectionAssert.AreEqual(
            expected.Select(character => (ushort)character).ToArray(),
            actual.Select(character => (ushort)character).ToArray());

    private static void WriteIdentity(
        string entryDirectory,
        ProductionId productionId,
        uint version)
    {
        var value = productionId.Value;
        var content = new byte[
            16 + checked(value.Length * sizeof(ushort))];
        Encoding.ASCII.GetBytes("KYMIDN01").CopyTo(content, 0);
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
        File.WriteAllBytes(
            Path.Combine(entryDirectory, "identity.kid"),
            framed);
    }

    private sealed class TestDirectory : IDisposable
    {
        public TestDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Kymaean.Persistence.Creation.Tests",
                Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
