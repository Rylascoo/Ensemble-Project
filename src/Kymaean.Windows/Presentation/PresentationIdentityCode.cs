using System.Buffers.Binary;
using System.Security.Cryptography;

namespace Kymaean.Windows.Presentation;

internal static class PresentationIdentityCode
{
    public static IReadOnlyDictionary<string, string> BuildDuplicateCodes<T>(
        IReadOnlyList<T> items,
        Func<T, string> nameSelector,
        Func<T, string> identityValueSelector)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(nameSelector);
        ArgumentNullException.ThrowIfNull(identityValueSelector);

        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var group in items.GroupBy(nameSelector, StringComparer.Ordinal))
        {
            var duplicates = group.ToArray();
            if (duplicates.Length < 2)
            {
                continue;
            }

            var hashes = duplicates.ToDictionary(
                item => identityValueSelector(item),
                item => HashIdentity(identityValueSelector(item)),
                StringComparer.Ordinal);
            var prefixLengths = duplicates.ToDictionary(
                item => identityValueSelector(item),
                _ => 8,
                StringComparer.Ordinal);

            while (true)
            {
                var collisions = duplicates
                    .GroupBy(
                        item =>
                        {
                            var identity = identityValueSelector(item);
                            return hashes[identity][..prefixLengths[identity]];
                        },
                        StringComparer.Ordinal)
                    .Where(candidate => candidate.Count() > 1)
                    .ToArray();

                if (collisions.Length == 0)
                {
                    break;
                }

                foreach (var collision in collisions)
                {
                    foreach (var item in collision)
                    {
                        var identity = identityValueSelector(item);
                        var nextLength = prefixLengths[identity] + 4;
                        if (nextLength > hashes[identity].Length)
                        {
                            throw new InvalidOperationException(
                                "Presentation identity fingerprints collide.");
                        }

                        prefixLengths[identity] = nextLength;
                    }
                }
            }

            foreach (var item in duplicates)
            {
                var identity = identityValueSelector(item);
                result[identity] = hashes[identity][..prefixLengths[identity]];
            }
        }

        return result;
    }

    private static string HashIdentity(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var bytes = new byte[checked(value.Length * sizeof(ushort))];
        for (var index = 0; index < value.Length; index++)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                bytes.AsSpan(index * sizeof(ushort), sizeof(ushort)),
                value[index]);
        }

        return Convert.ToHexString(SHA256.HashData(bytes));
    }
}
