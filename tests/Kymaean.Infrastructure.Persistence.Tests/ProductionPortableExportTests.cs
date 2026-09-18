using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class ProductionPortableExportTests
{
    private const int ExportHeaderLength = 20;
    private const int ChecksumLength = 32;

    [TestMethod]
    public void ExportIsDeterministicAndPreservesExactIdentityAndRawPayload()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId(
            "portable/" + new string(new[] { '\uD800' }) + "/identity");
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, id);
        var payload = Encoding.UTF8.GetBytes(
            "{\n  \"contract\" : \"kymaean.production.created.v1\",\n  \"productionName\" : \"Portable Harbor\"\n}");
        new FileProductionJournal(entry).Append(payload);
        var exporter = new FileProductionExporter(directory.Path);

        var first = exporter.ExportProduction(id);
        var second = exporter.ExportProduction(id);

        Assert.IsTrue(first.IsSuccess);
        Assert.IsTrue(second.IsSuccess);
        var firstBytes = first.Value.ToArray();
        var secondBytes = second.Value.ToArray();
        CollectionAssert.AreEqual(firstBytes, secondBytes);
        Assert.AreEqual(firstBytes.Length, first.Value.Length);

        var parsed = Parse(firstBytes);
        Assert.AreEqual(1U, parsed.Version);
        Assert.AreEqual(id.Value, parsed.ProductionId);
        Assert.HasCount(1, parsed.Payloads);
        CollectionAssert.AreEqual(payload, parsed.Payloads[0]);

        firstBytes[0] ^= 0xff;
        CollectionAssert.AreEqual(secondBytes, first.Value.ToArray());
    }

    [TestMethod]
    public void ExportContainsNoCatalogLocatorOrLocalOnlyArtifactsAndDoesNotMutateSource()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("artifact-exclusion");
        var entry = CreateProduction(
            directory.Path,
            0x2a,
            id,
            "Clean Harbor");

        var locator = Path.GetFileName(entry);
        var snapshotMarker = Encoding.UTF8.GetBytes("snapshot-local-marker");
        var pendingMarker = Encoding.UTF8.GetBytes("pending-local-marker");
        var credentialMarker = Encoding.UTF8.GetBytes("api-key-local-marker");
        File.WriteAllBytes(
            Path.Combine(entry, ".projection.snapshot"),
            snapshotMarker);
        File.WriteAllBytes(
            Path.Combine(entry, ".pending-entry-export-marker"),
            pendingMarker);
        File.WriteAllBytes(
            Path.Combine(entry, "provider-local-state.bin"),
            credentialMarker);

        var before = CaptureTree(entry);

        var result = new FileProductionExporter(directory.Path)
            .ExportProduction(id);

        Assert.IsTrue(result.IsSuccess);
        var bytes = result.Value.ToArray();
        var utf8 = Encoding.UTF8.GetString(bytes);
        Assert.IsFalse(utf8.Contains(locator, StringComparison.Ordinal));
        Assert.IsFalse(Contains(bytes, snapshotMarker));
        Assert.IsFalse(Contains(bytes, pendingMarker));
        Assert.IsFalse(Contains(bytes, credentialMarker));
        Assert.IsFalse(Contains(
            bytes,
            Encoding.ASCII.GetBytes("KYMIDN01")));
        Assert.IsFalse(Contains(
            bytes,
            Encoding.ASCII.GetBytes("KYMJRN01")));
        Assert.IsFalse(Contains(
            bytes,
            Encoding.ASCII.GetBytes("KYMJHD01")));
        Assert.IsFalse(Contains(
            bytes,
            Encoding.ASCII.GetBytes("KYMSNP01")));

        var after = CaptureTree(entry);
        CollectionAssert.AreEqual(
            before.Keys.OrderBy(value => value, StringComparer.Ordinal).ToArray(),
            after.Keys.OrderBy(value => value, StringComparer.Ordinal).ToArray());
        foreach (var path in before.Keys)
        {
            CollectionAssert.AreEqual(before[path], after[path], path);
        }
    }

    [TestMethod]
    public void ExportDoesNotCreateSnapshotOrRecoverMissingCommittedHead()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("no-recovery");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "No Recovery Harbor");
        var snapshotPath = Path.Combine(entry, ".projection.snapshot");
        Assert.IsFalse(File.Exists(snapshotPath));
        File.Delete(Path.Combine(entry, ".journal.head"));

        var result = new FileProductionExporter(directory.Path)
            .ExportProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.IsFalse(File.Exists(Path.Combine(entry, ".journal.head")));
        Assert.IsFalse(File.Exists(snapshotPath));
    }

    [TestMethod]
    public void UnknownProductionFailsInvalidWithoutCreatingStorage()
    {
        using var directory = new TestDirectory();
        var catalog = new FileProductionCatalog(directory.Path);
        var catalogDirectory = Path.Combine(
            directory.Path,
            "production-catalog");
        var before = Directory.GetFileSystemEntries(catalogDirectory);

        var result = new FileProductionExporter(directory.Path)
            .ExportProduction(
                new ProductionId("missing-export"));

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        CollectionAssert.AreEqual(
            before,
            Directory.GetFileSystemEntries(catalogDirectory));
    }

    [TestMethod]
    public void CorruptCommittedJournalFailsInvalid()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("corrupt-export");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Corrupt Export Harbor");
        var journalPath = Directory.GetFiles(entry, "*.kjr").Single();
        var bytes = File.ReadAllBytes(journalPath);
        bytes[^1] ^= 0xff;
        File.WriteAllBytes(journalPath, bytes);

        var result = new FileProductionExporter(directory.Path)
            .ExportProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void FutureEventContractFailsIncompatible()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("future-export");
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, id);
        new FileProductionJournal(entry).Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.created.v2\",\"productionName\":\"Future Harbor\"}"));

        var result = new FileProductionExporter(directory.Path)
            .ExportProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
    }

    [TestMethod]
    public void FutureIdentityMetadataFailsIncompatible()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("future-identity-export");
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, id, version: 2);
        CreateHistory(entry, "Future Identity Harbor");

        var result = new FileProductionExporter(directory.Path)
            .ExportProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
    }

    [TestMethod]
    public void ReplayInvalidHistoryFailsInvalid()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("replay-invalid-export");
        var entry = CreateEntryDirectory(directory.Path, 1);
        WriteIdentity(entry, id);
        var journal = new FileProductionJournal(entry);
        journal.Append(CreatedPayload("First"));
        journal.Append(CreatedPayload("Second"));

        var result = new FileProductionExporter(directory.Path)
            .ExportProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
    }

    [TestMethod]
    public void InspectorAcceptsValidatedPortableExport()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("inspect-export");
        _ = CreateProduction(
            directory.Path,
            1,
            id,
            "Inspect Harbor");

        var exported = new FileProductionExporter(directory.Path)
            .ExportProduction(id);
        Assert.IsTrue(exported.IsSuccess);

        var inspected = ProductionPortableExportInspector
            .Inspect(exported.Value.ToArray());

        Assert.IsTrue(inspected.IsSuccess);
        Assert.AreEqual(id, inspected.Value.ProductionId);
        Assert.AreEqual(1, inspected.Value.EventCount);
        Assert.AreEqual(
            "Inspect Harbor",
            inspected.Value.Projection.ProductionName);
    }

    [TestMethod]
    public void InspectorRejectsCorruptChecksum()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("checksum-export");
        _ = CreateProduction(
            directory.Path,
            1,
            id,
            "Checksum Harbor");
        var exported = new FileProductionExporter(directory.Path)
            .ExportProduction(id);
        Assert.IsTrue(exported.IsSuccess);
        var bytes = exported.Value.ToArray();
        bytes[^1] ^= 0xff;

        var inspected = ProductionPortableExportInspector
            .Inspect(bytes);

        Assert.IsFalse(inspected.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Invalid,
            inspected.FailureKind);
    }

    [TestMethod]
    public void InspectorRejectsUnsupportedPortableExportVersion()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("future-package");
        _ = CreateProduction(
            directory.Path,
            1,
            id,
            "Future Package Harbor");
        var exported = new FileProductionExporter(directory.Path)
            .ExportProduction(id);
        Assert.IsTrue(exported.IsSuccess);
        var bytes = exported.Value.ToArray();
        BinaryPrimitives.WriteUInt32BigEndian(
            bytes.AsSpan(8, sizeof(uint)),
            2);
        RecomputePackageChecksum(bytes);

        var inspected = ProductionPortableExportInspector
            .Inspect(bytes);

        Assert.IsFalse(inspected.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            inspected.FailureKind);
    }

    [TestMethod]
    public void InspectorRejectsTruncatedPortableExport()
    {
        var inspected = ProductionPortableExportInspector
            .Inspect(new byte[] { 1, 2, 3, 4 });

        Assert.IsFalse(inspected.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Invalid,
            inspected.FailureKind);
    }

    [TestMethod]
    public void ExportToFileFinalizesOutsideSourceAndInspectsSuccessfully()
    {
        using var directory = new TestDirectory();
        var sourceRoot = directory.CreateSubdirectory("source");
        var outputRoot = directory.CreateSubdirectory("output");
        var id = new ProductionId("durable-export");
        _ = CreateProduction(
            sourceRoot,
            1,
            id,
            "Durable Export Harbor");
        var destination = Path.Combine(
            outputRoot,
            "durable.kymprod");

        var exported = new FileProductionExporter(sourceRoot)
            .ExportProductionToFile(
                id,
                destination);
        var inspected = ProductionPortableExportInspector
            .InspectFile(destination);

        Assert.IsTrue(exported.IsSuccess);
        Assert.AreEqual(id, exported.Value);
        Assert.IsTrue(inspected.IsSuccess);
        Assert.AreEqual(id, inspected.Value.ProductionId);
        Assert.AreEqual(
            "Durable Export Harbor",
            inspected.Value.Projection.ProductionName);
    }

    [TestMethod]
    public void ExportToFileFailureLeavesNoPendingArtifact()
    {
        using var directory = new TestDirectory();
        var sourceRoot = directory.CreateSubdirectory("source");
        var outputRoot = directory.CreateSubdirectory("output");
        var id = new ProductionId("failed-destination");
        _ = CreateProduction(
            sourceRoot,
            1,
            id,
            "Failed Destination Harbor");
        var destination = Path.Combine(
            outputRoot,
            "blocked.kymprod");
        Directory.CreateDirectory(destination);

        try
        {
            _ = new FileProductionExporter(sourceRoot)
                .ExportProductionToFile(
                    id,
                    destination);
            Assert.Fail(
                "Blocked export destination unexpectedly succeeded.");
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }

        Assert.IsTrue(Directory.Exists(destination));
        Assert.AreEqual(
            0,
            Directory.GetFiles(
                outputRoot,
                "*.pending-export-*")
            .Length);
    }

    [TestMethod]
    public void ExportToFileRejectsDestinationInsideSourceRoot()
    {
        using var directory = new TestDirectory();
        var sourceRoot = directory.CreateSubdirectory("source");
        var id = new ProductionId("inside-source");
        var entry = CreateProduction(
            sourceRoot,
            1,
            id,
            "Inside Source Harbor");
        var before = CaptureTree(entry);
        var destination = Path.Combine(
            sourceRoot,
            "portable.kymprod");

        Assert.ThrowsExactly<ArgumentException>(
            () => new FileProductionExporter(sourceRoot)
                .ExportProductionToFile(
                    id,
                    destination));

        Assert.IsFalse(File.Exists(destination));
        var after = CaptureTree(entry);
        CollectionAssert.AreEqual(
            before.Keys.OrderBy(value => value, StringComparer.Ordinal).ToArray(),
            after.Keys.OrderBy(value => value, StringComparer.Ordinal).ToArray());
        foreach (var path in before.Keys)
        {
            CollectionAssert.AreEqual(
                before[path],
                after[path],
                path);
        }
    }

    private static void RecomputePackageChecksum(byte[] bytes)
    {
        var contentLength =
            bytes.Length - ChecksumLength;
        SHA256.HashData(
                bytes.AsSpan(
                    0,
                    contentLength))
            .CopyTo(
                bytes.AsSpan(
                    contentLength,
                    ChecksumLength));
    }

    private static ParsedExport Parse(byte[] bytes)
    {
        Assert.IsGreaterThanOrEqualTo(
            ExportHeaderLength + ChecksumLength,
            bytes.Length);
        var contentLength = bytes.Length - ChecksumLength;
        var content = bytes.AsSpan(0, contentLength);
        var checksum = bytes.AsSpan(contentLength, ChecksumLength);
        Assert.IsTrue(
            CryptographicOperations.FixedTimeEquals(
                checksum,
                SHA256.HashData(content)));

        CollectionAssert.AreEqual(
            Encoding.ASCII.GetBytes("KYMEXP01"),
            content[..8].ToArray());
        var version = BinaryPrimitives.ReadUInt32BigEndian(
            content.Slice(8, sizeof(uint)));
        var identityLength = checked((int)
            BinaryPrimitives.ReadUInt32BigEndian(
                content.Slice(12, sizeof(uint))));
        var eventCount = checked((int)
            BinaryPrimitives.ReadUInt32BigEndian(
                content.Slice(16, sizeof(uint))));

        var offset = 20;
        var characters = new char[identityLength];
        for (var index = 0; index < characters.Length; index++)
        {
            characters[index] = (char)BinaryPrimitives.ReadUInt16BigEndian(
                content.Slice(offset, sizeof(ushort)));
            offset += sizeof(ushort);
        }

        var payloads = new List<byte[]>(eventCount);
        for (var index = 0; index < eventCount; index++)
        {
            var payloadLength = checked((int)
                BinaryPrimitives.ReadUInt64BigEndian(
                    content.Slice(offset, sizeof(ulong))));
            offset += sizeof(ulong);
            payloads.Add(
                content.Slice(offset, payloadLength).ToArray());
            offset += payloadLength;
        }

        Assert.AreEqual(contentLength, offset);
        return new ParsedExport(
            version,
            new string(characters),
            payloads.AsReadOnly());
    }

    private static bool Contains(
        ReadOnlySpan<byte> haystack,
        ReadOnlySpan<byte> needle)
    {
        if (needle.Length == 0)
        {
            return true;
        }

        for (var offset = 0;
             offset <= haystack.Length - needle.Length;
             offset++)
        {
            if (haystack.Slice(offset, needle.Length)
                .SequenceEqual(needle))
            {
                return true;
            }
        }

        return false;
    }

    private static Dictionary<string, byte[]> CaptureTree(
        string rootDirectory) =>
        Directory
            .EnumerateFiles(
                rootDirectory,
                "*",
                SearchOption.AllDirectories)
            .ToDictionary(
                path => Path.GetRelativePath(rootDirectory, path),
                File.ReadAllBytes,
                StringComparer.Ordinal);

    private static string CreateProduction(
        string applicationRoot,
        int locatorSeed,
        ProductionId productionId,
        string productionName)
    {
        var entry = CreateEntryDirectory(
            applicationRoot,
            locatorSeed);
        WriteIdentity(entry, productionId);
        CreateHistory(entry, productionName);
        return entry;
    }

    private static void CreateHistory(
        string entryDirectory,
        string productionName)
    {
        var application = new ProductionApplication(
            new FileProductionEventStore(entryDirectory));
        Assert.AreEqual(
            productionName,
            application.Create(productionName).ProductionName);
    }

    private static string CreateEntryDirectory(
        string applicationRoot,
        int locatorSeed)
    {
        _ = new FileProductionCatalog(applicationRoot);
        var entry = Path.Combine(
            applicationRoot,
            "production-catalog",
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
        var framed = new byte[
            content.Length + checksum.Length];
        content.CopyTo(framed, 0);
        checksum.CopyTo(framed, content.Length);
        File.WriteAllBytes(
            Path.Combine(entryDirectory, "identity.kid"),
            framed);
    }

    private static byte[] CreatedPayload(
        string productionName) =>
        Encoding.UTF8.GetBytes(
            $"{{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"{productionName}\"}}");

    private sealed record ParsedExport(
        uint Version,
        string ProductionId,
        IReadOnlyList<byte[]> Payloads);

    private sealed class TestDirectory : IDisposable
    {
        public TestDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Kymaean.Persistence.Export.Tests",
                Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public string CreateSubdirectory(string name)
        {
            var path = System.IO.Path.Combine(
                Path,
                name);
            Directory.CreateDirectory(path);
            return path;
        }

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
