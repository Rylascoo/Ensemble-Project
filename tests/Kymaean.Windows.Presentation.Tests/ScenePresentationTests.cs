using Kymaean.Application;
using Kymaean.Windows.Presentation;

namespace Kymaean.Windows.Presentation.Tests;

[TestClass]
public sealed class ScenePresentationTests
{
    // Independent Python hashlib + utf-16-be/surrogatepass, generated once outside shipped code.
    [TestMethod]
    public void GoldenVectorsPreserveExactUtf16CodeUnits()
    {
        var vectors = new Dictionary<string, string>
        {
            ["S-1"] = "46F8B8DFAD95D13CFA3E9F4D1A0B019F51A32B95E80B7F239C08F514C30B3F77",
            ["A"] = "C00B4D3C929CB5CC316691ED4636F634576F2C9B2954767234C5274E9DDE185D",
            ["\uD800"] = "6E6535D29BE7BFAC2971DC0853620D739DD43A62C41409D21D39CCB9B29E224B",
            ["\uDC00"] = "34739425D55F591D570B36E6354822DBCCD6453A78CBB9A61C05521248206762",
            ["\U0001F600"] = "D4E887A680E68155B74F6002DF62C2E8DA129F928162F0E44AB3025F7653134F",
            ["e\u0301"] = "174177030048D22DC78D91D1D3DD2BA807B0E025415DADDDB6A104620F7009DC",
            ["\u00E9"] = "1C9CFB2618360748B0CA7F2BC9829F6A46788BA3ADCFDF4B6AE055869B87AE7F"
        };
        foreach (var (id, hash) in vectors)
        {
            Assert.AreEqual(hash, PresentationIdentityCode.HashIdentity(id));
            Assert.AreEqual(hash[..8], PresentationIdentityCode.BuildSceneCodes([id])[id]);
        }
    }

    [TestMethod]
    public void PreservedRealPrefixCollisionLengthensOnlyCollidingCodes()
    {
        const string first = "QDESIGN24-53141", second = "QDESIGN24-81797";
        Assert.AreEqual("F55024D1", PresentationIdentityCode.BuildSceneCodes([first])[first]);
        var codes = PresentationIdentityCode.BuildSceneCodes([first, "QDESIGN24-4", second, "QDESIGN24-6"]);
        Assert.AreEqual("F55024D1675E", codes[first]);
        Assert.AreEqual("F55024D1F9F5", codes[second]);
        Assert.AreEqual("58955591", codes["QDESIGN24-4"]);
        Assert.AreEqual("4E3C642D", codes["QDESIGN24-6"]);
    }

    [TestMethod]
    public void FullHashCollisionPreservesNonCollidingIdentity()
    {
        var codes = PresentationIdentityCode.InjectFullCollisionForTest(new Dictionary<string, string>
        { ["A"] = new('A', 64), ["B"] = new('A', 64), ["C"] = new('C', 64) });
        Assert.IsNull(codes["A"]);
        Assert.IsNull(codes["B"]);
        Assert.AreEqual("CCCCCCCC", codes["C"]);
    }

    [TestMethod]
    public void RosterSubsetRetainsFullCastDuplicateCodeAndIdentity()
    {
        var cast = new ProductionCast([new(new("C1"), "Same"), new(new("C2"), "Same")]);
        var rows = CharacterPresentationRow.Build(cast.Characters);
        var scene = new EstablishedScene(new("S-1"), new([new("C2")]));
        var replay = new ProductionReplayProjection("A", WorldCurrentState.Empty, cast, new([scene]));
        var rendered = ScenePresentationRow.Build(replay, rows).Single();
        Assert.AreEqual(rows[1], rendered.Roster.Single());
        Assert.IsTrue(rendered.Roster.Single().HasCharacterCode);
        Assert.AreEqual("46F8B8DF", rendered.Code);
    }

    [TestMethod]
    public void UncertaintySurvivesNavigationOtherProductionAndFailedOpen()
    {
        var (vm, catalog) = Start();
        catalog.Failure = ProductAccessFailureKind.Invalid;
        vm.BeginSceneDraft();
        Assert.IsNull(vm.EstablishScene());
        Assert.IsTrue(vm.HasSceneUncertainty);
        foreach (var route in new[] { ShellRoute.Home, ShellRoute.Productions, ShellRoute.Settings, ShellRoute.CurrentProduction })
        {
            vm.NavigateShell(route);
            vm.NavigateShell(ShellRoute.CurrentProduction);
            vm.OpenScenes();
            Assert.IsFalse(vm.BeginSceneDraft());
            Assert.IsNull(vm.EstablishScene());
        }
        vm.CloseScenes(); vm.OpenScenes();
        Assert.IsFalse(vm.BeginSceneDraft());
        Assert.AreEqual(1, catalog.Calls);
        Open(vm, "B"); vm.OpenScenes();
        Assert.IsFalse(vm.HasSceneUncertainty);
        Assert.IsTrue(vm.BeginSceneDraft());
        catalog.Failure = null;
        Assert.IsNotNull(vm.EstablishScene());
        catalog.FailOpen = true;
        vm.SelectProduction(vm.ProductionRows.First(row => row.Id.Value == "A"));
        Assert.IsFalse(vm.OpenSelectedProduction());
        catalog.FailOpen = false;
        Assert.IsTrue(vm.RecoverSelectedProduction()); // Recovery is not ordinary Open.
        vm.OpenScenes();
        Assert.IsTrue(vm.HasSceneUncertainty);
        Assert.IsFalse(vm.BeginSceneDraft());
        catalog.ExternalScene("A", "externally-added");
        Assert.IsTrue(vm.OpenSelectedProduction());
        StringAssert.Contains(vm.StatusMessage, "not confirmed as either successful or unsuccessful");
        vm.OpenScenes();
        Assert.IsFalse(vm.HasSceneUncertainty);
        Assert.AreEqual(0, vm.SubmittedSceneRoster.Count);
        Assert.IsTrue(vm.SceneRows.Any(row => row.Id.Value == "externally-added"));
        Assert.IsTrue(vm.BeginSceneDraft());
    }

    [TestMethod]
    public void EmptyAndLargeRosterUseCastOrderAndRejectRepeatedSubmission()
    {
        var (vm, catalog) = Start();
        vm.BeginSceneDraft();
        foreach (var row in vm.SceneCast.Reverse()) vm.SetSceneCharacter(row.Id, true);
        CollectionAssert.AreEqual(vm.SceneCast.Select(row => row.Id).ToArray(), vm.SceneDraftRoster.Select(row => row.Id).ToArray());
        Assert.IsNotNull(vm.EstablishScene());
        Assert.AreEqual(7, vm.SceneRows.Single().Roster.Count);
        Assert.IsNull(vm.EstablishScene());
        vm.BeginSceneDraft(); Assert.IsTrue(vm.HasEmptySceneDraft);
        Assert.IsNotNull(vm.EstablishScene());
        Assert.IsTrue(vm.SceneRows.Last().IsEmpty);
        Assert.AreEqual(2, catalog.Calls);
    }

    [TestMethod]
    public void ExceptionTaxonomyAndPostSuccessPresentationFailureRemainDistinct()
    {
        foreach (var error in new Exception[] { new IOException(), new UnauthorizedAccessException() })
        {
            var (vm, catalog) = Start(); catalog.Error = error; vm.BeginSceneDraft();
            Assert.IsNull(vm.EstablishScene()); Assert.IsTrue(vm.HasSceneUncertainty);
            StringAssert.StartsWith(vm.SceneNotice, "Confirmation unavailable.");
        }
        var (programming, badCatalog) = Start(); badCatalog.Error = new InvalidOperationException(); programming.BeginSceneDraft();
        Assert.ThrowsExactly<InvalidOperationException>(() => programming.EstablishScene());
        Assert.IsFalse(programming.HasSceneUncertainty);
        var (success, _) = Start(); success.BeginSceneDraft();
        success.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(success.SceneRows) && success.SceneNotice == "Scene established.") throw new IOException("presentation failure");
        };
        Assert.ThrowsExactly<IOException>(() => success.EstablishScene());
        Assert.IsFalse(success.HasSceneUncertainty);
        Assert.AreEqual("Scene established.", success.SceneNotice);
        Assert.AreEqual(1, success.SceneRows.Count);
    }

    [TestMethod]
    public void IncompatibleDiscardsDraftWithoutUncertaintyAndSlowCallBlocksReentry()
    {
        var (vm, catalog) = Start(); catalog.Failure = ProductAccessFailureKind.Incompatible;
        vm.BeginSceneDraft(); vm.SetSceneCharacter(vm.SceneCast[0].Id, true);
        catalog.DuringCall = () =>
        {
            Assert.IsFalse(vm.IsSceneNavigationEnabled);
            vm.NavigateShell(ShellRoute.Home);
            Assert.IsTrue(vm.IsCurrentProduction);
            Assert.IsFalse(vm.BeginSceneDraft());
            Assert.IsNull(vm.EstablishScene());
            Thread.Sleep(30);
        };
        Assert.IsNull(vm.EstablishScene());
        Assert.IsFalse(vm.HasSceneUncertainty);
        Assert.AreEqual("A Scene can't be established in this version.", vm.SceneNotice);
        Assert.IsTrue(vm.BeginSceneDraft()); Assert.IsTrue(vm.HasEmptySceneDraft);
    }

    [TestMethod]
    public void FullCollisionBlocksOnlyAffectedActionsAndPersistsAcrossOpen()
    {
        var (vm, catalog) = Start();
        catalog.ExternalScene("A", "collision-a"); catalog.ExternalScene("A", "collision-b");
        catalog.ExternalScene("A", "unambiguous");
        PresentationIdentityCode.FullHashCollisionIdentitiesForTest = new HashSet<string> { "collision-a", "collision-b" };
        try
        {
            Open(vm, "A"); vm.OpenScenes();
            Assert.IsTrue(vm.HasSceneCollision); Assert.IsFalse(vm.HasSceneUncertainty);
            Assert.IsFalse(vm.BeginSceneDraft());
            Assert.IsFalse(vm.InspectScene(new("collision-a")));
            Assert.IsTrue(vm.InspectScene(new("unambiguous")));
            Assert.AreEqual(new SceneId("unambiguous"), vm.ReturnFromSceneDetail());
            Open(vm, "A"); vm.OpenScenes(); Assert.IsTrue(vm.HasSceneCollision);
            Open(vm, "B"); vm.OpenScenes(); Assert.IsTrue(vm.BeginSceneDraft());
            Assert.IsNotNull(vm.EstablishScene());
        }
        finally { PresentationIdentityCode.FullHashCollisionIdentitiesForTest = null; }
    }

    [TestMethod]
    public void RealJournalAppendThenInvalidCannotBeResubmittedUntilFreshOpen()
    {
        var root = Path.Combine(Path.GetTempPath(), "qdesign24-" + Guid.NewGuid());
        try
        {
            var catalog = new Kymaean.Infrastructure.Persistence.FileProductionCatalog(root);
            var production = catalog.CreateProduction("Fixture").Value;
            var vm = new MainPageViewModel(WindowsStartupResult.Product(ProductApplication.Start(catalog)));
            vm.SelectProduction(vm.ProductionRows.Single()); Assert.IsTrue(vm.OpenSelectedProduction()); vm.OpenScenes();
            // Another caller changes the journal behind Application's cached projection.
            Assert.IsTrue(catalog.EstablishScene(production.Id, SceneRoster.Empty).IsSuccess);
            vm.BeginSceneDraft(); Assert.IsNull(vm.EstablishScene());
            StringAssert.StartsWith(vm.SceneNotice, "Scene not confirmed.");
            Assert.IsTrue(vm.HasSceneUncertainty);
            Assert.AreEqual(2, catalog.OpenProduction(production.Id).Value.ProductionScenes.Scenes.Length);
            vm.CloseScenes(); vm.OpenScenes(); Assert.IsFalse(vm.BeginSceneDraft()); Assert.IsNull(vm.EstablishScene());
            Assert.AreEqual(2, catalog.OpenProduction(production.Id).Value.ProductionScenes.Scenes.Length);
            Assert.IsTrue(vm.OpenSelectedProduction()); vm.OpenScenes();
            Assert.AreEqual(2, vm.SceneRows.Count); Assert.IsFalse(vm.HasSceneUncertainty);
            Assert.IsTrue(vm.BeginSceneDraft()); Assert.IsNotNull(vm.EstablishScene());
            Assert.AreEqual(3, catalog.OpenProduction(production.Id).Value.ProductionScenes.Scenes.Length);
        }
        finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
    }

    private static (MainPageViewModel, Catalog) Start()
    {
        var catalog = new Catalog();
        var vm = new MainPageViewModel(WindowsStartupResult.Product(ProductApplication.Start(catalog)));
        Open(vm, "A"); vm.OpenScenes();
        return (vm, catalog);
    }
    private static void Open(MainPageViewModel vm, string id)
    {
        vm.SelectProduction(vm.ProductionRows.Single(row => row.Id.Value == id));
        Assert.IsTrue(vm.OpenSelectedProduction());
    }

    private sealed class Catalog : IProductionCatalog, IProductionSceneCreator
    {
        private readonly ProductionSummary[] _productions = [new(new("A"), "A"), new(new("B"), "B")];
        private readonly Dictionary<string, List<EstablishedScene>> _scenes = new() { ["A"] = [], ["B"] = [] };
        public ProductAccessFailureKind? Failure { get; set; }
        public Exception? Error { get; set; }
        public bool FailOpen { get; set; }
        public Action? DuringCall { get; set; }
        public int Calls { get; private set; }
        private static ProductionCast Cast => new(Enumerable.Range(0, 7).Select(i => new CharacterSummary(new($"C{i}"), "Same")));
        private ProductionReplayProjection Replay(string id) => new(id, WorldCurrentState.Empty, Cast, new(_scenes[id]));
        public void ExternalScene(string id, string sceneId) => _scenes[id].Add(new(new(sceneId), SceneRoster.Empty));
        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() => ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(_productions);
        public ProductAccessResult<ProductionReplayProjection> OpenProduction(ProductionId id) => FailOpen
            ? ProductAccessResult<ProductionReplayProjection>.Failure(ProductAccessFailureKind.Invalid)
            : ProductAccessResult<ProductionReplayProjection>.Success(Replay(id.Value));
        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(ProductionId id) => OpenProduction(id);
        public ProductAccessResult<SceneCreation> EstablishScene(ProductionId id, SceneRoster roster)
        {
            Calls++; DuringCall?.Invoke();
            if (Error is not null) throw Error;
            if (Failure is { } failure) return ProductAccessResult<SceneCreation>.Failure(failure);
            var scene = new EstablishedScene(new($"S{Calls}"), roster);
            _scenes[id.Value].Add(scene);
            return ProductAccessResult<SceneCreation>.Success(new(scene, Replay(id.Value)));
        }
    }
}
