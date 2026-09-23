using System.Text.Json;
using System.Text.RegularExpressions;
using Kymaean.Application;
using Kymaean.Infrastructure.Persistence;
using Kymaean.Windows.Preview;

namespace Kymaean.DirectorPreview;

internal static class Profiles
{
    internal static object Create(string localState, string kind, string name, string source)
    {
        if (kind is not ("director" or "empty" or "rich" or "falsifier")) throw new ArgumentException("Unknown profile kind.");
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!Regex.IsMatch(source, "^[a-f0-9]{40}$")) throw new ArgumentException("Exact source required.");
        var root = PreviewEnvironment.Root(localState);
        var profiles = PreviewEnvironment.SafePath(root, "profiles");
        Directory.CreateDirectory(profiles);
        PreviewEnvironment.CheckTree(profiles);
        if (kind == "director" && Directory.EnumerateFiles(profiles, "profile.json", SearchOption.AllDirectories).Any(IsDirector))
            throw new IOException("Director workspace already exists. Select it; never reset it.");
        var id = Guid.NewGuid().ToString("N");
        var profile = PreviewEnvironment.SafePath(profiles, id);
        var data = PreviewEnvironment.SafePath(profile, "data");
        Directory.CreateDirectory(data);
        var catalog = new FileProductionCatalog(data);
        if (kind is "rich" or "falsifier") Seed(catalog, data, kind);
        var metadata = new { id, name, kind, recipe = kind is "rich" or "falsifier" ? $"{kind}-v1" : null,
            source, compatibility = PreviewEnvironment.Compatibility, createdUtc = DateTime.UtcNow,
            preservation = "Never overwrite. Recipe changes and reseeding create a new instance." };
        File.WriteAllText(PreviewEnvironment.SafePath(profile, "profile.json"), JsonSerializer.Serialize(metadata));
        return metadata;
    }

    private static bool IsDirector(string file)
    {
        using var metadata = JsonDocument.Parse(File.ReadAllText(file));
        return metadata.RootElement.GetProperty("kind").GetString() == "director";
    }

    internal static object Select(string localState, string id)
    {
        if (!Regex.IsMatch(id, "^[a-f0-9]{32}$")) throw new ArgumentException("Invalid profile ID.");
        var root = PreviewEnvironment.Root(localState);
        var profile = PreviewEnvironment.SafePath(root, "profiles", id);
        ValidateProfile(profile, id);
        var pending = PreviewEnvironment.SafePath(root, $"selection-{Guid.NewGuid():N}.tmp");
        File.WriteAllText(pending, JsonSerializer.Serialize(new { id }));
        File.Move(pending, PreviewEnvironment.SafePath(root, "active-profile.json"), true);
        return new { id, data = PreviewEnvironment.SelectedDataRoot(localState) };
    }

    internal static object[] Verify(string localState)
    {
        var profiles = PreviewEnvironment.SafePath(PreviewEnvironment.Root(localState), "profiles");
        PreviewEnvironment.CheckTree(profiles);
        return Directory.EnumerateDirectories(profiles).Select(path =>
        {
            var id = Path.GetFileName(path);
            ValidateProfile(path, id);
            var catalog = new FileProductionCatalog(PreviewEnvironment.SafePath(path, "data"));
            var list = catalog.ListProductions();
            if (!list.IsSuccess) throw new IOException("Profile catalog cannot be replayed.");
            var summary = list.Value.Select(p =>
            {
                var replay = catalog.OpenProduction(p.Id);
                if (!replay.IsSuccess) throw new IOException("Profile Production cannot be replayed.");
                return new { name = p.ProductionName, cast = replay.Value.ProductionCast.Characters.Length,
                    scenes = replay.Value.ProductionScenes.Scenes.Length, truths = replay.Value.WorldCurrentState.Truths.Length };
            }).ToArray();
            return (object)new { id, productions = summary };
        }).ToArray();
    }

    private static void ValidateProfile(string profile, string id)
    {
        using var metadata = JsonDocument.Parse(File.ReadAllText(PreviewEnvironment.SafePath(profile, "profile.json")));
        if (metadata.RootElement.GetProperty("id").GetString() != id ||
            metadata.RootElement.GetProperty("compatibility").GetString() != PreviewEnvironment.Compatibility)
            throw new IOException("Profile identity/compatibility mismatch.");
        var data = PreviewEnvironment.SafePath(profile, "data");
        if (!Directory.Exists(data)) throw new IOException("Missing profile data.");
        PreviewEnvironment.CheckTree(data);
    }

    private static void Seed(FileProductionCatalog catalog, string data, string kind)
    {
        var app = ProductApplication.Start(catalog).Value;
        var first = app.CreateProduction("The Harbor at Dawn").Value;
        var entry = Directory.EnumerateDirectories(Path.Combine(data, "production-catalog"), "entry-*").Single();
        app.CreateProduction("An Empty Production");
        app.OpenProduction(first.Id);
        var cast = new List<CharacterId>();
        foreach (var name in new[] { "Marlowe", "Marlowe", "Iona", "Wren", "Dr. Voss", "Keir", "A character with a deliberately long name for wrapping" })
            cast.Add(app.CreateCharacter(name).Value.Character.Id);
        catalog.ReplaceWorldCurrentState(first.Id, new WorldCurrentState(new[] {
            new WorldCurrentTruth("The harbor closes at dawn."), new WorldCurrentTruth("The lower archive is reached by the old stair.") }));
        app.OpenProduction(first.Id);
        _ = app.EstablishScene(Array.Empty<CharacterId>()).Value;
        _ = app.EstablishScene(Array.Empty<CharacterId>()).Value;
        _ = app.EstablishScene(new[] { cast[1] }).Value;
        _ = app.EstablishScene(cast).Value;
        if (kind == "falsifier")
        {
            // Fixed opaque identities are engineering apparatus, not a creator API or naming feature.
            var store = new FileProductionEventStore(entry);
            foreach (var identity in new[] { "QDESIGN24-4", "QDESIGN24-6", "QDESIGN24-53141", "QDESIGN24-81797" })
                store.Append(new CreatorEstablishedSceneEvent(new SceneId(identity), SceneRoster.Empty));
        }
    }
}
