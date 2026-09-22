using System.Buffers.Binary;
using System.Security.Cryptography;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

internal static class ProductionProjectionSnapshotCache
{
    private const int HashLength = 32;
    private const int ChecksumLength = 32;
    private const string SnapshotFileName = ".projection.snapshot";
    private const string PendingPrefix = ".pending-projection-snapshot-";

    private static readonly byte[] FormatFamilyMagic = "KYMSNP01"u8.ToArray();

    public static void ReconcileBestEffort(
        string entryDirectory,
        ProductionJournalAnchor anchor,
        ProductionReplayProjection authoritativeProjection,
        bool forceWrite)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entryDirectory);
        ArgumentNullException.ThrowIfNull(anchor);
        ArgumentNullException.ThrowIfNull(authoritativeProjection);

        try
        {
            DeletePendingFiles(entryDirectory);

            if (!forceWrite &&
                TryRead(
                    entryDirectory,
                    anchor,
                    out var cachedProjection) &&
                cachedProjection == authoritativeProjection)
            {
                return;
            }

            Write(
                entryDirectory,
                anchor,
                authoritativeProjection);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private static bool TryRead(
        string entryDirectory,
        ProductionJournalAnchor expectedAnchor,
        out ProductionReplayProjection? projection)
    {
        projection = null;
        var path = Path.Combine(entryDirectory, SnapshotFileName);
        if (!File.Exists(path))
        {
            return false;
        }

        var bytes = File.ReadAllBytes(path);
        if (bytes.Length <
            FormatFamilyMagic.Length + sizeof(uint) + sizeof(ulong) +
            HashLength + (3 * sizeof(uint)) + ChecksumLength)
        {
            return false;
        }

        var contentLength = bytes.Length - ChecksumLength;
        var content = bytes.AsSpan(0, contentLength);
        var storedChecksum = bytes.AsSpan(contentLength, ChecksumLength);
        if (!CryptographicOperations.FixedTimeEquals(
                storedChecksum,
                SHA256.HashData(content)))
        {
            return false;
        }

        var offset = 0;
        if (!TryReadExact(content, ref offset, FormatFamilyMagic.Length, out var magic)
            || !magic.SequenceEqual(FormatFamilyMagic)
            || !TryReadUInt32(content, ref offset, out var version)
            || version != ProductionPersistenceVersionPolicy.ProductionProjectionSnapshotVersion
            || !TryReadUInt64(content, ref offset, out var sequence)
            || sequence != expectedAnchor.Sequence
            || !TryReadExact(content, ref offset, HashLength, out var storedRecordHash))
        {
            return false;
        }

        var expectedHash = Convert.FromHexString(expectedAnchor.RecordHash);
        if (!CryptographicOperations.FixedTimeEquals(
                storedRecordHash,
                expectedHash))
        {
            return false;
        }

        try
        {
            if (!TryReadUtf16String(content, ref offset, out var productionName)
                || !TryReadUInt32(content, ref offset, out var truthCount)
                || truthCount > int.MaxValue)
            {
                return false;
            }

            var truths = new List<WorldCurrentTruth>(checked((int)truthCount));
            for (var index = 0U; index < truthCount; index++)
            {
                if (!TryReadUtf16String(content, ref offset, out var truth))
                {
                    return false;
                }
                truths.Add(new WorldCurrentTruth(truth));
            }

            if (!TryReadUInt32(content, ref offset, out var characterCount)
                || characterCount > int.MaxValue)
            {
                return false;
            }

            var characters =
                new List<CharacterSummary>(checked((int)characterCount));
            for (var index = 0U; index < characterCount; index++)
            {
                if (!TryReadUtf16String(content, ref offset, out var characterId)
                    || !TryReadUtf16String(content, ref offset, out var characterName))
                {
                    return false;
                }

                characters.Add(
                    new CharacterSummary(
                        new CharacterId(characterId),
                        characterName));
            }

            if (offset != content.Length)
            {
                return false;
            }

            projection = new ProductionReplayProjection(
                productionName,
                new WorldCurrentState(truths),
                new ProductionCast(characters));
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (OverflowException)
        {
            return false;
        }

        return true;
    }

    private static void Write(
        string entryDirectory,
        ProductionJournalAnchor anchor,
        ProductionReplayProjection projection)
    {
        var recordHash = Convert.FromHexString(anchor.RecordHash);
        if (recordHash.Length != HashLength)
        {
            throw new InvalidOperationException(
                "Validated journal anchor is not SHA-256 sized.");
        }

        using var stream = new MemoryStream();
        stream.Write(FormatFamilyMagic);
        WriteUInt32(
            stream,
            ProductionPersistenceVersionPolicy.ProductionProjectionSnapshotVersion);
        WriteUInt64(stream, anchor.Sequence);
        stream.Write(recordHash);
        WriteUtf16String(stream, projection.ProductionName);

        WriteUInt32(
            stream,
            checked((uint)projection.WorldCurrentState.Truths.Length));
        foreach (var truth in projection.WorldCurrentState.Truths)
        {
            WriteUtf16String(stream, truth.Text);
        }

        WriteUInt32(
            stream,
            checked((uint)projection.ProductionCast.Characters.Length));
        foreach (var character in projection.ProductionCast.Characters)
        {
            WriteUtf16String(stream, character.Id.Value);
            WriteUtf16String(stream, character.CharacterName);
        }

        var content = stream.ToArray();
        var checksum = SHA256.HashData(content);
        var framed = new byte[content.Length + checksum.Length];
        content.CopyTo(framed, 0);
        checksum.CopyTo(framed, content.Length);

        var pendingPath = Path.Combine(
            entryDirectory,
            $"{PendingPrefix}{Guid.NewGuid():N}");
        var finalPath = Path.Combine(entryDirectory, SnapshotFileName);

        try
        {
            using (var output = new FileStream(
                       pendingPath,
                       new FileStreamOptions
                       {
                           Mode = FileMode.CreateNew,
                           Access = FileAccess.Write,
                           Share = FileShare.None,
                           BufferSize = 4096,
                           Options =
                               FileOptions.SequentialScan |
                               FileOptions.WriteThrough,
                       }))
            {
                output.Write(framed);
                output.Flush(flushToDisk: true);
            }

            File.Move(pendingPath, finalPath, overwrite: true);
        }
        finally
        {
            TryDeletePending(pendingPath);
        }
    }

    private static bool TryReadUtf16String(
        ReadOnlySpan<byte> content,
        ref int offset,
        out string value)
    {
        value = string.Empty;
        if (!TryReadUInt32(content, ref offset, out var length)
            || length > int.MaxValue
            || length > (content.Length - offset) / sizeof(ushort))
        {
            return false;
        }

        var byteLength = checked((int)length * sizeof(ushort));
        value = Utf16CodeUnits.Decode(content.Slice(offset, byteLength));
        offset += byteLength;
        return true;
    }

    private static bool TryReadUInt32(
        ReadOnlySpan<byte> content,
        ref int offset,
        out uint value)
    {
        value = 0;
        if (offset < 0 || offset > content.Length - sizeof(uint))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt32BigEndian(
            content.Slice(offset, sizeof(uint)));
        offset += sizeof(uint);
        return true;
    }

    private static bool TryReadUInt64(
        ReadOnlySpan<byte> content,
        ref int offset,
        out ulong value)
    {
        value = 0;
        if (offset < 0 || offset > content.Length - sizeof(ulong))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt64BigEndian(
            content.Slice(offset, sizeof(ulong)));
        offset += sizeof(ulong);
        return true;
    }

    private static bool TryReadExact(
        ReadOnlySpan<byte> content,
        ref int offset,
        int length,
        out ReadOnlySpan<byte> value)
    {
        value = default;
        if (offset < 0 || length < 0 || offset > content.Length - length)
        {
            return false;
        }

        value = content.Slice(offset, length);
        offset += length;
        return true;
    }

    private static void WriteUtf16String(Stream stream, string value)
    {
        WriteUInt32(stream, checked((uint)value.Length));
        stream.Write(Utf16CodeUnits.Encode(value));
    }

    private static void WriteUInt32(Stream stream, uint value)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        stream.Write(bytes);
    }

    private static void WriteUInt64(Stream stream, ulong value)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        stream.Write(bytes);
    }

    private static void DeletePendingFiles(string entryDirectory)
    {
        foreach (var pendingPath in Directory.EnumerateFiles(
                     entryDirectory,
                     $"{PendingPrefix}*",
                     SearchOption.TopDirectoryOnly))
        {
            File.Delete(pendingPath);
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
}
