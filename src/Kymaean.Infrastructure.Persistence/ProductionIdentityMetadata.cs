using System.Buffers.Binary;
using System.Security.Cryptography;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

internal static class ProductionIdentityMetadata
{
    private const int HeaderLength = 16;
    private const int ChecksumLength = 32;
    private static readonly byte[] FormatFamilyMagic = "KYMIDN01"u8.ToArray();

    public static ProductionId Read(string path)
    {
        using var stream = new FileStream(
            path,
            new FileStreamOptions
            {
                Mode = FileMode.Open,
                Access = FileAccess.Read,
                Share = FileShare.Read,
                BufferSize = 4096,
                Options = FileOptions.SequentialScan,
            });

        if (stream.Length < HeaderLength + ChecksumLength)
        {
            throw new InvalidDataException(
                "Production identity metadata is shorter than its required framing.");
        }

        var contentLength = stream.Length - ChecksumLength;
        var header = new byte[HeaderLength];
        ReadRequired(stream, header);

        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        hash.AppendData(header);
        HashRemainingContent(stream, contentLength - HeaderLength, hash);

        var storedChecksum = new byte[ChecksumLength];
        ReadRequired(stream, storedChecksum);
        var computedChecksum = hash.GetHashAndReset();
        if (!CryptographicOperations.FixedTimeEquals(
                storedChecksum,
                computedChecksum))
        {
            throw new InvalidDataException(
                "Production identity metadata checksum does not match.");
        }

        if (!header.AsSpan(0, FormatFamilyMagic.Length)
                .SequenceEqual(FormatFamilyMagic))
        {
            throw new InvalidDataException(
                "Production identity metadata format family is not recognized.");
        }

        var version = BinaryPrimitives.ReadUInt32BigEndian(
            header.AsSpan(8, sizeof(uint)));
        ProductionPersistenceVersionPolicy.RequireIdentityMetadataVersion(
            version);

        var codeUnitCount = BinaryPrimitives.ReadUInt32BigEndian(
            header.AsSpan(12, sizeof(uint)));
        var expectedContentLength =
            HeaderLength + checked((long)codeUnitCount * sizeof(ushort));
        if (contentLength != expectedContentLength ||
            codeUnitCount > int.MaxValue)
        {
            throw new InvalidDataException(
                "Production identity metadata length does not match its declared identity length.");
        }

        stream.Position = HeaderLength;
        var characters = new char[checked((int)codeUnitCount)];
        var pair = new byte[sizeof(ushort)];
        for (var index = 0; index < characters.Length; index++)
        {
            ReadRequired(stream, pair);
            characters[index] = (char)BinaryPrimitives.ReadUInt16BigEndian(pair);
        }

        try
        {
            return new ProductionId(new string(characters));
        }
        catch (ArgumentException exception)
        {
            throw new InvalidDataException(
                "Production identity metadata violates the Application identity contract.",
                exception);
        }
    }

    private static void HashRemainingContent(
        FileStream stream,
        long remaining,
        IncrementalHash hash)
    {
        var buffer = new byte[8192];
        while (remaining > 0)
        {
            var length = checked((int)Math.Min(buffer.Length, remaining));
            ReadRequired(stream, buffer.AsSpan(0, length));
            hash.AppendData(buffer.AsSpan(0, length));
            remaining -= length;
        }
    }

    private static void ReadRequired(FileStream stream, Span<byte> buffer)
    {
        try
        {
            stream.ReadExactly(buffer);
        }
        catch (EndOfStreamException exception)
        {
            throw new InvalidDataException(
                "Production identity metadata ended unexpectedly.",
                exception);
        }
    }
}
