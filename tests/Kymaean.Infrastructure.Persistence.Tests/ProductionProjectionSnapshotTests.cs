using System.Buffers.Binary;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;

namespace Kymaean.Infrastructure.Persistence.Tests;

[TestClass]
public sealed class ProductionProjectionSnapshotTests
{
    private const int SnapshotPrefixLength =
        8 + sizeof(uint) + sizeof(ulong) + 32 + sizeof(uint);
    private const int SnapshotChecksumLength = 32;
    private const string SnapshotFileName = ".projection.snapshot";
    private const string PendingSnapshotPrefix = ".pending-projection-snapshot-";

    [TestMethod]
    public void MissingSnapshotIsRebuiltFromAuthoritativeJournal()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-missing");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "The Glass Harbor");
        var catalog = new FileProductionCatalog(directory.Path);

        var first = catalog.OpenProduction(id);
        Assert.IsTrue(first.IsSuccess);
        Assert.IsTrue(File.Exists(SnapshotPath(entry)));

        File.Delete(SnapshotPath(entry));

        var reopened = catalog.OpenProduction(id);

        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            "The Glass Harbor",
            reopened.Value.ProductionName);
        AssertSnapshot(entry, "The Glass Harbor");
    }

    [TestMethod]
    public void MalformedSnapshotFallsBackAndRebuilds()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-malformed");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Malformed Harbor");
        var catalog = new FileProductionCatalog(directory.Path);
        Assert.IsTrue(catalog.OpenProduction(id).IsSuccess);

        File.WriteAllBytes(
            SnapshotPath(entry),
            Encoding.ASCII.GetBytes("malformed snapshot"));

        var reopened = catalog.OpenProduction(id);

        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            "Malformed Harbor",
            reopened.Value.ProductionName);
        AssertSnapshot(entry, "Malformed Harbor");
    }

    [TestMethod]
    public void ValidSnapshotIsRecognizedWithoutRewrite()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-reuse");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Reuse Harbor");
        var catalog = new FileProductionCatalog(directory.Path);
        Assert.IsTrue(catalog.OpenProduction(id).IsSuccess);

        var snapshotPath = SnapshotPath(entry);
        var marker = new DateTime(
            2001,
            2,
            3,
            4,
            5,
            6,
            DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(snapshotPath, marker);

        var reopened = catalog.OpenProduction(id);

        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            "Reuse Harbor",
            reopened.Value.ProductionName);
        Assert.AreEqual(
            marker,
            File.GetLastWriteTimeUtc(snapshotPath));
        AssertSnapshot(entry, "Reuse Harbor");
    }

    [TestMethod]
    public void StaleSnapshotAnchorIsIgnoredAndRebuilt()
    {
        using var directory = new TestDirectory();
        var firstId = new ProductionId("snapshot-first");
        var secondId = new ProductionId("snapshot-second");
        var firstEntry = CreateProduction(
            directory.Path,
            1,
            firstId,
            "First Harbor");
        var secondEntry = CreateProduction(
            directory.Path,
            2,
            secondId,
            "Second Harbor");
        var catalog = new FileProductionCatalog(directory.Path);

        Assert.IsTrue(catalog.OpenProduction(firstId).IsSuccess);
        Assert.IsTrue(catalog.OpenProduction(secondId).IsSuccess);

        File.Copy(
            SnapshotPath(firstEntry),
            SnapshotPath(secondEntry),
            overwrite: true);

        var reopened = catalog.OpenProduction(secondId);

        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            "Second Harbor",
            reopened.Value.ProductionName);
        AssertSnapshot(secondEntry, "Second Harbor");
    }

    [TestMethod]
    public void CorruptSnapshotChecksumFallsBackAndRebuilds()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-checksum");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Checksum Harbor");
        var catalog = new FileProductionCatalog(directory.Path);
        Assert.IsTrue(catalog.OpenProduction(id).IsSuccess);

        var bytes = File.ReadAllBytes(SnapshotPath(entry));
        bytes[^1] ^= 0xff;
        File.WriteAllBytes(SnapshotPath(entry), bytes);

        var reopened = catalog.OpenProduction(id);

        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            "Checksum Harbor",
            reopened.Value.ProductionName);
        AssertSnapshot(entry, "Checksum Harbor");
    }

    [TestMethod]
    public void UnsupportedSnapshotVersionFallsBackAndRebuilds()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-version");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Version Harbor");
        var catalog = new FileProductionCatalog(directory.Path);
        Assert.IsTrue(catalog.OpenProduction(id).IsSuccess);

        var bytes = File.ReadAllBytes(SnapshotPath(entry));
        BinaryPrimitives.WriteUInt32BigEndian(
            bytes.AsSpan(8, sizeof(uint)),
            2);
        RecomputeSnapshotChecksum(bytes);
        File.WriteAllBytes(SnapshotPath(entry), bytes);

        var reopened = catalog.OpenProduction(id);

        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            "Version Harbor",
            reopened.Value.ProductionName);
        Assert.AreEqual(
            3U,
            SnapshotVersion(entry));
        AssertSnapshot(entry, "Version Harbor");
    }

    [TestMethod]
    public void SameAnchorWrongProjectionNeverOverridesJournalTruth()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-wrong-projection");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "True Harbor");
        var catalog = new FileProductionCatalog(directory.Path);
        Assert.IsTrue(catalog.OpenProduction(id).IsSuccess);

        var bytes = File.ReadAllBytes(SnapshotPath(entry));
        RewriteSnapshotName(
            bytes,
            "Fake Harbor");
        RecomputeSnapshotChecksum(bytes);
        File.WriteAllBytes(SnapshotPath(entry), bytes);
        Assert.AreEqual(
            "Fake Harbor",
            SnapshotName(entry));

        var reopened = catalog.OpenProduction(id);

        Assert.IsTrue(reopened.IsSuccess);
        Assert.AreEqual(
            "True Harbor",
            reopened.Value.ProductionName);
        AssertSnapshot(entry, "True Harbor");
    }

    [TestMethod]
    public void ValidSnapshotCannotMaskCorruptJournal()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-journal-corrupt");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Corrupt Harbor");
        var catalog = new FileProductionCatalog(directory.Path);
        Assert.IsTrue(catalog.OpenProduction(id).IsSuccess);
        AssertSnapshot(entry, "Corrupt Harbor");

        var journalPath = Directory
            .GetFiles(entry, "*.kjr")
            .Single();
        var journalBytes = File.ReadAllBytes(journalPath);
        journalBytes[^1] ^= 0xff;
        File.WriteAllBytes(journalPath, journalBytes);

        var reopened = catalog.OpenProduction(id);

        Assert.IsFalse(reopened.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Invalid,
            reopened.FailureKind);
    }

    [TestMethod]
    public void PlausibleSnapshotCannotMaskReplayInvalidJournal()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-replay-invalid");
        var entry = CreateEntryDirectory(
            directory.Path,
            1);
        WriteIdentity(entry, id);

        var journal = new FileProductionJournal(entry);
        journal.Append(CreatedPayload("True Harbor"));
        journal.Append(CreatedPayload("Second Create"));

        var anchor = FinalJournalAnchor(entry);
        File.WriteAllBytes(
            SnapshotPath(entry),
            BuildSnapshot(
                anchor.Sequence,
                anchor.RecordHash,
                "True Harbor"));
        AssertSnapshot(entry, "True Harbor");

        var result = new FileProductionCatalog(directory.Path)
            .OpenProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Invalid,
            result.FailureKind);
    }

    [TestMethod]
    public void PlausibleSnapshotCannotMaskIncompatibleJournal()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-incompatible");
        var entry = CreateEntryDirectory(
            directory.Path,
            1);
        WriteIdentity(entry, id);

        var journal = new FileProductionJournal(entry);
        journal.Append(
            Encoding.UTF8.GetBytes(
                "{\"contract\":\"kymaean.production.created.v3\",\"productionName\":\"Future Harbor\"}"));

        var anchor = FinalJournalAnchor(entry);
        File.WriteAllBytes(
            SnapshotPath(entry),
            BuildSnapshot(
                anchor.Sequence,
                anchor.RecordHash,
                "Future Harbor"));
        AssertSnapshot(entry, "Future Harbor");

        var result = new FileProductionCatalog(directory.Path)
            .OpenProduction(id);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
    }

    [TestMethod]
    public void ExplicitRecoveryRefreshesSnapshotAfterAuthoritativeReplay()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-recover");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Recovery Harbor");
        var catalog = new FileProductionCatalog(directory.Path);
        Assert.IsTrue(catalog.OpenProduction(id).IsSuccess);

        File.WriteAllBytes(
            SnapshotPath(entry),
            Encoding.ASCII.GetBytes("malformed snapshot"));
        File.Delete(HeadPath(entry));

        var recovered = catalog.RecoverProduction(id);

        Assert.IsTrue(recovered.IsSuccess);
        Assert.AreEqual(
            "Recovery Harbor",
            recovered.Value.ProductionName);
        Assert.IsTrue(File.Exists(HeadPath(entry)));
        AssertSnapshot(entry, "Recovery Harbor");
    }

    [TestMethod]
    public void SnapshotWriteFailureDoesNotInvalidateDurableProduct()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-write-failure");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Durable Harbor");
        Directory.CreateDirectory(SnapshotPath(entry));

        var result = new FileProductionCatalog(directory.Path)
            .OpenProduction(id);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(
            "Durable Harbor",
            result.Value.ProductionName);
        Assert.IsTrue(
            Directory.Exists(SnapshotPath(entry)));
    }

    [TestMethod]
    public void InterruptedPendingSnapshotIsDiscardedBeforeRebuild()
    {
        using var directory = new TestDirectory();
        var id = new ProductionId("snapshot-pending");
        var entry = CreateProduction(
            directory.Path,
            1,
            id,
            "Pending Harbor");
        var pendingPath = Path.Combine(
            entry,
            $"{PendingSnapshotPrefix}orphan");
        File.WriteAllBytes(
            pendingPath,
            Encoding.ASCII.GetBytes("partial snapshot"));

        var result = new FileProductionCatalog(directory.Path)
            .OpenProduction(id);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(
            "Pending Harbor",
            result.Value.ProductionName);
        Assert.IsFalse(File.Exists(pendingPath));
        AssertSnapshot(entry, "Pending Harbor");
    }

    private static string CreateProduction(
        string applicationRoot,
        int locatorSeed,
        ProductionId productionId,
        string productionName)
    {
        var entry = CreateEntryDirectory(
            applicationRoot,
            locatorSeed);
        WriteIdentity(
            entry,
            productionId);
        var application = new ProductionApplication(
            new FileProductionEventStore(entry));
        Assert.AreEqual(
            productionName,
            application.Create(productionName).ProductionName);
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
        ProductionId productionId)
    {
        var value = productionId.Value;
        var content = new byte[
            16 + checked(value.Length * sizeof(ushort))];

        Encoding.ASCII.GetBytes("KYMIDN01")
            .CopyTo(content, 0);
        BinaryPrimitives.WriteUInt32BigEndian(
            content.AsSpan(8, sizeof(uint)),
            1);
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
        checksum.CopyTo(
            framed,
            content.Length);
        File.WriteAllBytes(
            IdentityPath(entryDirectory),
            framed);
    }

    private static byte[] BuildSnapshot(
        ulong sequence,
        string recordHash,
        string productionName)
    {
        var hash = Convert.FromHexString(recordHash);
        var content = new byte[
            SnapshotPrefixLength +
            (productionName.Length * sizeof(ushort)) +
            sizeof(uint) +
            sizeof(uint)];

        Encoding.ASCII.GetBytes("KYMSNP01")
            .CopyTo(content, 0);
        BinaryPrimitives.WriteUInt32BigEndian(
            content.AsSpan(8, sizeof(uint)),
            3);
        BinaryPrimitives.WriteUInt64BigEndian(
            content.AsSpan(12, sizeof(ulong)),
            sequence);
        hash.CopyTo(content, 20);
        BinaryPrimitives.WriteUInt32BigEndian(
            content.AsSpan(52, sizeof(uint)),
            checked((uint)productionName.Length));

        var offset = SnapshotPrefixLength;
        foreach (var character in productionName)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                content.AsSpan(offset, sizeof(ushort)),
                character);
            offset += sizeof(ushort);
        }

        var checksum = SHA256.HashData(content);
        var framed = new byte[
            content.Length + SnapshotChecksumLength];
        content.CopyTo(framed, 0);
        checksum.CopyTo(framed, content.Length);
        return framed;
    }

    private static void RewriteSnapshotName(
        byte[] bytes,
        string productionName)
    {
        var nameLength = BinaryPrimitives.ReadUInt32BigEndian(
            bytes.AsSpan(52, sizeof(uint)));
        Assert.AreEqual(
            checked((int)nameLength),
            productionName.Length);

        var offset = SnapshotPrefixLength;
        foreach (var character in productionName)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                bytes.AsSpan(offset, sizeof(ushort)),
                character);
            offset += sizeof(ushort);
        }
    }

    private static void RecomputeSnapshotChecksum(
        byte[] bytes)
    {
        var contentLength =
            bytes.Length - SnapshotChecksumLength;
        SHA256.HashData(
                bytes.AsSpan(0, contentLength))
            .CopyTo(
                bytes.AsSpan(
                    contentLength,
                    SnapshotChecksumLength));
    }

    private static void AssertSnapshot(
        string entryDirectory,
        string expectedName)
    {
        var bytes = File.ReadAllBytes(
            SnapshotPath(entryDirectory));
        var contentLength =
            bytes.Length - SnapshotChecksumLength;
        Assert.IsGreaterThanOrEqualTo(
            SnapshotPrefixLength,
            contentLength);

        var expectedChecksum = SHA256.HashData(
            bytes.AsSpan(0, contentLength));
        Assert.IsTrue(
            CryptographicOperations.FixedTimeEquals(
                bytes.AsSpan(
                    contentLength,
                    SnapshotChecksumLength),
                expectedChecksum));
        Assert.AreEqual(
            3U,
            SnapshotVersion(entryDirectory));
        Assert.AreEqual(
            expectedName,
            SnapshotName(entryDirectory));
    }

    private static uint SnapshotVersion(
        string entryDirectory)
    {
        var bytes = File.ReadAllBytes(
            SnapshotPath(entryDirectory));
        return BinaryPrimitives.ReadUInt32BigEndian(
            bytes.AsSpan(8, sizeof(uint)));
    }

    private static string SnapshotName(
        string entryDirectory)
    {
        var bytes = File.ReadAllBytes(
            SnapshotPath(entryDirectory));
        var nameLength = checked((int)
            BinaryPrimitives.ReadUInt32BigEndian(
                bytes.AsSpan(52, sizeof(uint))));
        var characters = new char[nameLength];
        var offset = SnapshotPrefixLength;
        for (var index = 0; index < characters.Length; index++)
        {
            characters[index] =
                (char)BinaryPrimitives.ReadUInt16BigEndian(
                    bytes.AsSpan(
                        offset,
                        sizeof(ushort)));
            offset += sizeof(ushort);
        }

        return new string(characters);
    }

    private static JournalAnchor FinalJournalAnchor(
        string entryDirectory)
    {
        var path = Directory
            .GetFiles(entryDirectory, "*.kjr")
            .OrderBy(
                Path.GetFileName,
                StringComparer.Ordinal)
            .Last();
        var parts = Path
            .GetFileNameWithoutExtension(path)
            .Split(
                '-',
                2,
                StringSplitOptions.None);
        return new JournalAnchor(
            ulong.Parse(
                parts[0],
                CultureInfo.InvariantCulture),
            parts[1]);
    }

    private static byte[] CreatedPayload(
        string productionName) =>
        Encoding.UTF8.GetBytes(
            $"{{\"contract\":\"kymaean.production.created.v1\",\"productionName\":\"{productionName}\"}}");

    private static string CatalogDirectory(
        string applicationRoot) =>
        Path.Combine(
            applicationRoot,
            "production-catalog");

    private static string IdentityPath(
        string entryDirectory) =>
        Path.Combine(
            entryDirectory,
            "identity.kid");

    private static string SnapshotPath(
        string entryDirectory) =>
        Path.Combine(
            entryDirectory,
            SnapshotFileName);

    private static string HeadPath(
        string entryDirectory) =>
        Path.Combine(
            entryDirectory,
            ".journal.head");

    private sealed record JournalAnchor(
        ulong Sequence,
        string RecordHash);

    private sealed class TestDirectory : IDisposable
    {
        public TestDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Kymaean.Persistence.Snapshot.Tests",
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
                    Directory.Delete(
                        Path,
                        recursive: true);
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
