using System.Text.Json;
using Kymaean.DirectorPreview;
using Kymaean.Infrastructure.Persistence;
using Kymaean.Windows.Preview;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class ProfileTests
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "kymaean-preview-tests", Guid.NewGuid().ToString("N"));
    private const string Source = "69c68001836ed984df672587cecd41d340a31337";
    private string Create(string kind) => JsonSerializer.SerializeToElement(Profiles.Create(_root, kind, kind, Source)).GetProperty("id").GetString()!;

    [TestMethod]
    public void DirectorEditsSurviveOtherProfileCreationAndSelection()
    {
        using var lease = PreviewEnvironment.Acquire(_root);
        var director = Create("director");
        Profiles.Select(_root, director);
        var data = PreviewEnvironment.SelectedDataRoot(_root);
        new FileProductionCatalog(data).CreateProduction("Director's work");
        var empty = Create("empty");
        Profiles.Select(_root, empty);
        Assert.AreEqual(0, new FileProductionCatalog(PreviewEnvironment.SelectedDataRoot(_root)).ListProductions().Value.Count);
        Profiles.Select(_root, director);
        Assert.AreEqual("Director's work", new FileProductionCatalog(data).ListProductions().Value.Single().ProductionName);
        Assert.Throws<IOException>(() => Create("director"));
    }

    [TestMethod]
    public void RichRecipesProduceIndependentEquivalentInstances()
    {
        using var lease = PreviewEnvironment.Acquire(_root);
        var a = Create("rich"); var b = Create("rich");
        Assert.AreNotEqual(a, b);
        var summaries = JsonSerializer.SerializeToElement(Profiles.Verify(_root));
        foreach (var summary in summaries.EnumerateArray())
        {
            var productions = summary.GetProperty("productions").EnumerateArray().ToArray();
            Assert.AreEqual(2, productions.Length);
            var rich = productions.Single(p => p.GetProperty("name").GetString() == "The Harbor at Dawn");
            Assert.AreEqual(7, rich.GetProperty("cast").GetInt32());
            Assert.AreEqual(4, rich.GetProperty("scenes").GetInt32());
            Assert.AreEqual(2, rich.GetProperty("truths").GetInt32());
        }
    }

    [TestMethod]
    public void FalsifierIncludesFourPreservedCodeVectorIdentities()
    {
        using var lease = PreviewEnvironment.Acquire(_root);
        Profiles.Select(_root, Create("falsifier"));
        var catalog = new FileProductionCatalog(PreviewEnvironment.SelectedDataRoot(_root));
        var production = catalog.ListProductions().Value.Single(p => p.ProductionName == "The Harbor at Dawn");
        var scenes = catalog.OpenProduction(production.Id).Value.ProductionScenes.Scenes;
        Assert.AreEqual(8, scenes.Length);
        foreach (var id in new[] { "QDESIGN24-4", "QDESIGN24-6", "QDESIGN24-53141", "QDESIGN24-81797" })
            Assert.IsTrue(scenes.Any(s => s.Id.Value == id));
    }

    [TestMethod]
    public void SessionLockExcludesMaintenanceAndSecondApp()
    {
        using var lease = PreviewEnvironment.Acquire(_root);
        Assert.Throws<IOException>(() => PreviewEnvironment.Acquire(_root));
    }

    [TestMethod]
    public void InvalidSelectionDoesNotChangeExistingProfile()
    {
        using var lease = PreviewEnvironment.Acquire(_root);
        var id = Create("empty"); Profiles.Select(_root, id);
        var before = PreviewEnvironment.SelectedDataRoot(_root);
        Assert.Throws<ArgumentException>(() => Profiles.Select(_root, "../escape"));
        Assert.Throws<IOException>(() => Profiles.Select(_root, new string('a', 32)));
        Assert.AreEqual(before, PreviewEnvironment.SelectedDataRoot(_root));
    }

    [TestMethod]
    public void IncompatibleProfileFailsWithoutRecreatingData()
    {
        using var lease = PreviewEnvironment.Acquire(_root);
        var id = Create("empty"); Profiles.Select(_root, id);
        var path = Path.Combine(PreviewEnvironment.Root(_root), "profiles", id, "profile.json");
        var content = File.ReadAllText(path).Replace(PreviewEnvironment.Compatibility, "future-schema");
        File.WriteAllText(path, content);
        Assert.Throws<IOException>(() => PreviewEnvironment.SelectedDataRoot(_root));
        Assert.AreEqual(content, File.ReadAllText(path));
    }

    [TestMethod]
    public void PathEscapeRejected() => Assert.Throws<IOException>(() => PreviewEnvironment.SafePath(_root, "..", "other"));
}
