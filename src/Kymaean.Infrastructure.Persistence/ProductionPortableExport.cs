using System.Buffers.Binary;
using System.Security.Cryptography;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

public sealed class ProductionPortableExport
{
    private readonly byte[] _content;

    internal ProductionPortableExport(byte[] content)
    {
        ArgumentNullException.ThrowIfNull(content);
        _content = content.ToArray();
    }

    public int Length => _content.Length;

    public byte[] ToArray() => _content.ToArray();

    internal ReadOnlyMemory<byte> Content => _content;
}

public sealed record ProductionPortableExportSummary(
    ProductionId ProductionId,
    int EventCount,
    ProductionReplayProjection Projection);

public static class ProductionPortableExportInspector
{
    public static ProductAccessResult<ProductionPortableExportSummary> Inspect(
        ReadOnlyMemory<byte> package)
    {
        try
        {
            return ProductAccessResult<ProductionPortableExportSummary>.Success(
                ProductionPortableExportCodec.Decode(package));
        }
        catch (ProductionPersistenceCompatibilityException)
        {
            return ProductAccessResult<ProductionPortableExportSummary>.Failure(
                ProductAccessFailureKind.Incompatible);
        }
        catch (InvalidDataException)
        {
            return ProductAccessResult<ProductionPortableExportSummary>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (InvalidOperationException)
        {
            return ProductAccessResult<ProductionPortableExportSummary>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (OverflowException)
        {
            return ProductAccessResult<ProductionPortableExportSummary>.Failure(
                ProductAccessFailureKind.Invalid);
        }
    }

    public static ProductAccessResult<ProductionPortableExportSummary> InspectFile(
        string packagePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(packagePath);
        return Inspect(
            File.ReadAllBytes(
                Path.GetFullPath(packagePath)));
    }
}

internal static class ProductionPortableExportCodec
{
    private const int ChecksumLength = 32;
    private const int FixedHeaderLength =
        8 + sizeof(uint) + sizeof(uint) + sizeof(uint);
    private const string PendingMarker = ".pending-export-";
    private static readonly byte[] FormatFamilyMagic = "KYMEXP01"u8.ToArray();

    public static ProductionPortableExport Encode(
        ProductionId productionId,
        IReadOnlyList<ProductionJournalEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ArgumentNullException.ThrowIfNull(entries);
        if (entries.Count == 0)
        {
            throw new InvalidOperationException(
                "A portable Production export requires authoritative event history.");
        }

        using var stream = new MemoryStream();
        stream.Write(FormatFamilyMagic);
        WriteUInt32(
            stream,
            ProductionPersistenceVersionPolicy.ProductionPortableExportVersion);

        var identity = productionId.Value;
        WriteUInt32(
            stream,
            checked((uint)identity.Length));
        WriteUInt32(
            stream,
            checked((uint)entries.Count));

        Span<byte> pair = stackalloc byte[sizeof(ushort)];
        foreach (var character in identity)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                pair,
                character);
            stream.Write(pair);
        }

        Span<byte> length = stackalloc byte[sizeof(ulong)];
        foreach (var entry in entries)
        {
            ArgumentNullException.ThrowIfNull(entry);
            var payload = entry.Payload.Span;
            BinaryPrimitives.WriteUInt64BigEndian(
                length,
                checked((ulong)payload.Length));
            stream.Write(length);
            stream.Write(payload);
        }

        var content = stream.ToArray();
        var checksum = SHA256.HashData(content);
        var framed = new byte[
            checked(content.Length + ChecksumLength)];
        content.CopyTo(framed, 0);
        checksum.CopyTo(
            framed,
            content.Length);
        return new ProductionPortableExport(framed);
    }

    public static ProductionPortableExportSummary Decode(
        ReadOnlyMemory<byte> package)
    {
        var bytes = package.Span;
        if (bytes.Length < FixedHeaderLength + ChecksumLength)
        {
            throw new InvalidDataException(
                "Production portable export is shorter than its required framing.");
        }

        var contentLength = bytes.Length - ChecksumLength;
        var content = bytes[..contentLength];
        var storedChecksum = bytes.Slice(
            contentLength,
            ChecksumLength);
        var computedChecksum = SHA256.HashData(content);
        if (!CryptographicOperations.FixedTimeEquals(
                storedChecksum,
                computedChecksum))
        {
            throw new InvalidDataException(
                "Production portable export checksum does not match.");
        }

        var offset = 0;
        RequireAvailable(
            content,
            offset,
            FormatFamilyMagic.Length);
        if (!content.Slice(
                offset,
                FormatFamilyMagic.Length)
            .SequenceEqual(FormatFamilyMagic))
        {
            throw new InvalidDataException(
                "Production portable export format family is not recognized.");
        }

        offset += FormatFamilyMagic.Length;
        var version = ReadUInt32(
            content,
            ref offset);
        ProductionPersistenceVersionPolicy.RequirePortableExportVersion(
            version);

        var identityCodeUnits = ReadUInt32(
            content,
            ref offset);
        var eventCount = ReadUInt32(
            content,
            ref offset);
        if (identityCodeUnits > int.MaxValue ||
            eventCount > int.MaxValue)
        {
            throw new InvalidDataException(
                "Production portable export declares an unsupported item count.");
        }

        var identityLength = checked((int)identityCodeUnits);
        var identityBytes = checked(
            identityLength * sizeof(ushort));
        RequireAvailable(
            content,
            offset,
            identityBytes);

        var characters = new char[identityLength];
        for (var index = 0; index < characters.Length; index++)
        {
            characters[index] =
                (char)BinaryPrimitives.ReadUInt16BigEndian(
                    content.Slice(
                        offset,
                        sizeof(ushort)));
            offset += sizeof(ushort);
        }

        ProductionId productionId;
        try
        {
            productionId = new ProductionId(
                new string(characters));
        }
        catch (ArgumentException exception)
        {
            throw new InvalidDataException(
                "Production portable export identity violates the Application identity contract.",
                exception);
        }

        var events = new List<ProductionEvent>(
            checked((int)eventCount));
        for (var index = 0; index < eventCount; index++)
        {
            var payloadLength = ReadUInt64(
                content,
                ref offset);
            if (payloadLength > int.MaxValue)
            {
                throw new InvalidDataException(
                    "Production portable export event payload is too large.");
            }

            var length = checked((int)payloadLength);
            RequireAvailable(
                content,
                offset,
                length);
            events.Add(
                ProductionEventCodec.Decode(
                    content.Slice(
                        offset,
                        length)
                    .ToArray()));
            offset += length;
        }

        if (offset != content.Length)
        {
            throw new InvalidDataException(
                "Production portable export contains trailing unframed content.");
        }

        var projection = ProductionReplay.Rebuild(
            events.AsReadOnly());
        return new ProductionPortableExportSummary(
            productionId,
            events.Count,
            projection);
    }

    public static void WriteFinalized(
        ProductionPortableExport package,
        string destinationPath)
    {
        ArgumentNullException.ThrowIfNull(package);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        var finalPath = Path.GetFullPath(destinationPath);
        var directory = Path.GetDirectoryName(finalPath)
            ?? throw new ArgumentException(
                "Production export destination has no parent directory.",
                nameof(destinationPath));
        var pendingPath = Path.Combine(
            directory,
            "." + Path.GetFileName(finalPath) +
            PendingMarker +
            Guid.NewGuid().ToString("N"));

        try
        {
            using (var stream = new FileStream(
                       pendingPath,
                       new FileStreamOptions
                       {
                           Mode = FileMode.CreateNew,
                           Access = FileAccess.Write,
                           Share = FileShare.None,
                           BufferSize = 8192,
                           Options =
                               FileOptions.SequentialScan |
                               FileOptions.WriteThrough,
                       }))
            {
                stream.Write(package.Content.Span);
                stream.Flush(flushToDisk: true);
            }

            File.Move(
                pendingPath,
                finalPath,
                overwrite: true);
        }
        finally
        {
            TryDeletePending(pendingPath);
        }
    }

    private static void TryDeletePending(string pendingPath)
    {
        try
        {
            if (File.Exists(pendingPath))
            {
                File.Delete(pendingPath);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private static uint ReadUInt32(
        ReadOnlySpan<byte> content,
        ref int offset)
    {
        RequireAvailable(
            content,
            offset,
            sizeof(uint));
        var value = BinaryPrimitives.ReadUInt32BigEndian(
            content.Slice(
                offset,
                sizeof(uint)));
        offset += sizeof(uint);
        return value;
    }

    private static ulong ReadUInt64(
        ReadOnlySpan<byte> content,
        ref int offset)
    {
        RequireAvailable(
            content,
            offset,
            sizeof(ulong));
        var value = BinaryPrimitives.ReadUInt64BigEndian(
            content.Slice(
                offset,
                sizeof(ulong)));
        offset += sizeof(ulong);
        return value;
    }

    private static void RequireAvailable(
        ReadOnlySpan<byte> content,
        int offset,
        int length)
    {
        if (offset < 0 ||
            length < 0 ||
            offset > content.Length - length)
        {
            throw new InvalidDataException(
                "Production portable export framing exceeds available content.");
        }
    }

    private static void WriteUInt32(
        Stream stream,
        uint value)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(
            bytes,
            value);
        stream.Write(bytes);
    }
}
