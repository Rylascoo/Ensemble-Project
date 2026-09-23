#if KYMAEAN_DIRECTOR_PREVIEW
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Kymaean.Windows.Preview;

// Development composition only. This entire type is absent from ordinary builds.
internal static class PreviewEnvironment
{
    public const string Compatibility = "qprod08-journal-v1";

    public static string Root(string localState) => SafePath(localState, "DirectorPreview");

    public static string SafePath(string root, params string[] parts)
    {
        var fullRoot = Path.GetFullPath(root);
        var path = Path.GetFullPath(Path.Combine(new[] { fullRoot }.Concat(parts).ToArray()));
        if (!path.StartsWith(fullRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new IOException("Preview path escapes its root.");
        for (var current = new DirectoryInfo(path); current is not null; current = current.Parent)
            if (current.Exists && (current.Attributes & FileAttributes.ReparsePoint) != 0)
                throw new IOException("Preview paths cannot traverse reparse points.");
        if (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
            throw new IOException("Preview paths cannot be reparse points.");
        return path;
    }

    public static FileStream Acquire(string localState)
    {
        var root = Root(localState);
        Directory.CreateDirectory(root);
        return new FileStream(SafePath(root, "session.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }

    public static string SelectedDataRoot(string localState)
    {
        var root = Root(localState);
        using var selected = JsonDocument.Parse(File.ReadAllText(SafePath(root, "active-profile.json")));
        var id = selected.RootElement.GetProperty("id").GetString() ?? "";
        if (!Regex.IsMatch(id, "^[a-f0-9]{32}$")) throw new IOException("Invalid Preview profile identity.");
        var profile = SafePath(root, "profiles", id);
        using var metadata = JsonDocument.Parse(File.ReadAllText(SafePath(profile, "profile.json")));
        if (metadata.RootElement.GetProperty("id").GetString() != id ||
            metadata.RootElement.GetProperty("compatibility").GetString() != Compatibility)
            throw new IOException("Preview profile metadata or compatibility mismatch.");
        var data = SafePath(profile, "data");
        if (!Directory.Exists(data)) throw new IOException("Preview data is missing; never recreate it silently.");
        // Walk descendants before handing them to existing persistence, which owns Product validation.
        CheckTree(data);
        return data;
    }

    public static void CheckTree(string path)
    {
        foreach (var entry in Directory.EnumerateFileSystemEntries(path))
        {
            var attributes = File.GetAttributes(entry);
            if ((attributes & FileAttributes.ReparsePoint) != 0) throw new IOException("Preview data contains a reparse point.");
            if ((attributes & FileAttributes.Directory) != 0) CheckTree(entry);
        }
    }

    public static void RecordLaunch(string localState, bool success)
    {
        var root = Root(localState);
        var receipts = SafePath(root, "launches");
        Directory.CreateDirectory(receipts);
        var source = typeof(PreviewEnvironment).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "DirectorPreviewSource").Value;
        var receipt = new { source, success, pid = Environment.ProcessId,
            executable = Environment.ProcessPath, architecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
            profile = success ? Path.GetFileName(Path.GetDirectoryName(SelectedDataRoot(localState))) : null };
        File.WriteAllText(SafePath(receipts, $"{DateTime.UtcNow:yyyyMMddTHHmmssfffffff}-{Guid.NewGuid():N}.json"), JsonSerializer.Serialize(receipt));
    }
}
#endif
