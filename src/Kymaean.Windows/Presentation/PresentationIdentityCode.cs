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

            foreach (var (identity, code) in BuildSceneCodes(duplicates.Select(identityValueSelector).ToArray()))
            {
                result[identity] = code ?? throw new InvalidOperationException("Presentation identity fingerprints collide.");
            }
        }

        return result;
    }

    public static IReadOnlyDictionary<string, string?> BuildSceneCodes(IReadOnlyList<string> identities)
    {
#if SCENE_PRESENTATION_TESTS
        if (FullHashCollisionIdentitiesForTest is { } collision)
            return ResolveSceneHashes(identities.ToDictionary(id => id,
                id => collision.Contains(id) ? new string('F', 64) : HashIdentity(id), StringComparer.Ordinal));
#endif
        return ResolveSceneHashes(identities.ToDictionary(id => id, HashIdentity, StringComparer.Ordinal));
    }

    // Only tests compile the injection entry point. The retail build has no hash override.
#if SCENE_PRESENTATION_TESTS
    internal static IReadOnlySet<string>? FullHashCollisionIdentitiesForTest { get; set; }
    internal static IReadOnlyDictionary<string, string?> InjectFullCollisionForTest(
        IReadOnlyDictionary<string, string> hashes) => ResolveSceneHashes(hashes);
#endif

    private static IReadOnlyDictionary<string, string?> ResolveSceneHashes(
        IReadOnlyDictionary<string, string> hashes)
    {
        var result = new Dictionary<string, string?>(StringComparer.Ordinal);
        var fullCollisions = hashes.GroupBy(pair => pair.Value, StringComparer.Ordinal)
            .Where(group => group.Count() > 1).SelectMany(group => group.Select(pair => pair.Key))
            .ToHashSet(StringComparer.Ordinal);
        foreach (var (identity, hash) in hashes)
        {
            if (fullCollisions.Contains(identity))
            {
                result.Add(identity, null);
                continue;
            }

            var length = 8;
            while (hashes.Any(other => other.Key != identity &&
                       other.Value.AsSpan(0, length).SequenceEqual(hash.AsSpan(0, length))))
            {
                length += 4;
            }
            result.Add(identity, hash[..length]);
        }
        return result;
    }

    internal static string HashIdentity(string value)
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
