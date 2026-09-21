using System.Buffers.Binary;

namespace Kymaean.Infrastructure.Persistence;

internal static class Utf16CodeUnits
{
    // Text encoders can replace unpaired surrogates. Persist char values directly.
    public static byte[] Encode(string value)
    {
        var bytes = new byte[checked(value.Length * sizeof(ushort))];
        for (var index = 0; index < value.Length; index++)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                bytes.AsSpan(index * sizeof(ushort), sizeof(ushort)), value[index]);
        }

        return bytes;
    }

    public static string Decode(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length % sizeof(ushort) != 0)
        {
            throw new InvalidDataException("UTF-16 code-unit data has an odd byte count.");
        }

        var characters = new char[bytes.Length / sizeof(ushort)];
        for (var index = 0; index < characters.Length; index++)
        {
            characters[index] = (char)BinaryPrimitives.ReadUInt16BigEndian(
                bytes.Slice(index * sizeof(ushort), sizeof(ushort)));
        }

        return new string(characters);
    }
}
