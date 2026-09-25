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
    public void SceneRowsExposeMeaningfulInitialRosterPreview()
    {
        var cast = new ProductionCast([
            new(new("C1"), "Marlowe"),
            new(new("C2"), "Marlowe"),
            new(new("C3"), "Third"),
            new(new("C4"), "Fourth")
        ]);
        var castRows = CharacterPresentationRow.Build(cast.Characters);
        var empty = new EstablishedScene(new("empty"), SceneRoster.Empty);
        var populated = new EstablishedScene(new("populated"), new([new("C1"), new("C2"), new("C3"), new("C4")]));
        var replay = new ProductionReplayProjection("A", WorldCurrentState.Empty, cast, new([empty, populated]));
        var rows = ScenePresentationRow.Build(replay, castRows);

        Assert.AreEqual("No Characters in this initial roster.", rows[0].RosterSummary);
        StringAssert.StartsWith(rows[1].RosterSummary, "Initial roster: Marlowe, Character code ");
        StringAssert.Contains(rows[1].RosterSummary, "; Marlowe, Character code ");
        StringAssert.Contains(rows[1].RosterSummary, "; Third");
        StringAssert.EndsWith(rows[1].RosterSummary, "; +1 more");
        Assert.AreEqual($"Inspect initial roster for {rows[1].CodeLabel}", rows[1].InspectName);
        Assert.AreEqual($"Initial roster. {rows[1].CodeLabel}", rows[1].DetailHeadingName);
    }

    [TestMethod]
    public void CurrentProductionOverviewUsesBoundedEarnedProjectionPreviews()
    {
        var (vm, catalog) = Start();
        catalog.ExternalWorld("A", "First", "Second", "Third", "Fourth");
        for (var i = 0; i < 5; i++) catalog.ExternalScene("A", $"overview-{i}");

        Open(vm, "A");

        Assert.IsTrue(vm.IsCurrentProductionOverview);
        CollectionAssert.AreEqual(
            new[] { "First", "Fourth", "Second" },
            vm.OverviewWorldTruths.ToArray());
        Assert.IsTrue(vm.HasMoreOverviewWorldTruths);
        Assert.AreEqual(3, vm.OverviewCharacterRows.Count);
        Assert.IsTrue(vm.HasMoreOverviewCharacters);
        Assert.AreEqual(3, vm.OverviewSceneRows.Count);
        Assert.IsTrue(vm.HasMoreOverviewScenes);
        CollectionAssert.AreEqual(
            vm.SceneRows.Take(3).Select(row => row.Id).ToArray(),
            vm.OverviewSceneRows.Select(row => row.Id).ToArray());
    }

    [TestMethod]
    public void CurrentProductionOverviewPreservesSceneNonconfirmationWitness()
    {
        var (vm, catalog) = Start();
        catalog.ExternalScene("A", "confirmed-before-attempt");
        Open(vm, "A");
        vm.OpenScenes();
        catalog.Failure = ProductAccessFailureKind.Invalid;
        Assert.IsTrue(vm.BeginSceneDraft());
        Assert.IsNull(vm.EstablishScene());
        Assert.IsTrue(vm.HasSceneUncertainty);

        vm.CloseScenes();

        Assert.IsTrue(vm.IsCurrentProductionOverview);
        Assert.AreEqual("Last confirmed Scenes", vm.SceneListHeading);
        Assert.AreEqual(1, vm.OverviewSceneRows.Count);
        Assert.AreEqual(
            new SceneId("confirmed-before-attempt"),
            vm.OverviewSceneRows.Single().Id);
        StringAssert.StartsWith(vm.SceneNotice, "Scene not confirmed.");
    }

    [TestMethod]
    public void EmptySceneNonconfirmationNeverClaimsAuthoritativeAbsence()
    {
        foreach (var environmental in new[] { false, true })
        {
            var (vm, catalog) = Start();
            if (environmental)
            {
                catalog.Error = new IOException();
            }
            else
            {
                catalog.Failure = ProductAccessFailureKind.Invalid;
            }

            Assert.IsTrue(vm.BeginSceneDraft());
            Assert.IsNull(vm.EstablishScene());
            Assert.IsTrue(vm.HasSceneUncertainty);
            Assert.IsTrue(vm.HasNoScenes);
            Assert.AreEqual(
                "There were no last confirmed Scenes.",
                vm.SceneEmptyMessage);

            vm.CloseScenes();

            Assert.IsTrue(vm.IsCurrentProductionOverview);
            Assert.AreEqual("Last confirmed Scenes", vm.SceneListHeading);
            Assert.IsTrue(vm.HasNoScenes);
            Assert.AreEqual(
                "There were no last confirmed Scenes.",
                vm.SceneEmptyMessage);
        }
    }

    [TestMethod]
    public void OverviewPreservesWorldAndCharacterNonconfirmationOnReturn()
    {
        var (vm, catalog) = Start();
        vm.CloseScenes();

        vm.OpenWorldTruths();
        vm.BeginWorldTruthEdit();
        vm.AddWorldTruthDraft();
        vm.WorldTruthDraftItems.Single().Text = "Proposed";
        Assert.IsTrue(vm.ReviewWorldTruthReplacement());
        Assert.IsTrue(vm.BeginWorldTruthReplacementSubmission());
        catalog.WorldError = new IOException();
        vm.CompleteWorldTruthReplacementSubmission();
        Assert.IsTrue(vm.IsWorldTruthConfirmationUnavailable);

        vm.CloseWorldTruthsToOverview();

        Assert.IsTrue(vm.IsCurrentProductionOverview);
        Assert.IsTrue(vm.HasWorldTruthOverviewUncertainty);
        Assert.AreEqual(
            "Kymaean couldn't confirm whether the replacement became current.",
            vm.WorldTruthOverviewDescription);
        Assert.IsTrue(vm.HasNoOverviewWorldTruths);
        Assert.AreEqual(
            "The last confirmed set was empty.",
            vm.WorldTruthOverviewEmptyMessage);
        CollectionAssert.AreEqual(
            new[] { "Proposed" },
            vm.SubmittedWorldTruths.ToArray());

        catalog.WorldError = null;
        catalog.SetCast("A");
        Open(vm, "A");
        vm.OpenCharacters();
        vm.BeginCharacterCreation();
        vm.CharacterNameDraft = "Marlowe";
        Assert.IsTrue(vm.BeginCharacterCreationSubmission());
        catalog.CharacterError = new IOException();
        Assert.IsNull(vm.CompleteCharacterCreationSubmission());
        Assert.IsTrue(vm.IsCharacterConfirmationUnavailable);

        vm.CloseCharactersToOverview();

        Assert.IsTrue(vm.IsCurrentProductionOverview);
        Assert.IsTrue(vm.HasCharacterOverviewUncertainty);
        Assert.AreEqual(
            "Kymaean couldn't confirm whether this Character was created.",
            vm.CharacterOverviewDescription);
        Assert.IsTrue(vm.HasNoOverviewCharacters);
        Assert.AreEqual(
            "There were no last confirmed Characters.",
            vm.CharacterOverviewEmptyMessage);
        Assert.AreEqual("Marlowe", vm.SubmittedCharacterName);
    }

    [TestMethod]
    public void ConfirmedDuplicateCharacterRefreshesSceneOverviewDisambiguation()
    {
        var (vm, catalog) = Start();
        catalog.SetCast("A", "Marlowe");
        catalog.ExternalScene("A", "scene-with-marlowe", "C0");
        Open(vm, "A");

        var before = vm.OverviewSceneRows.Single().Roster.Single();
        Assert.AreEqual(new CharacterId("C0"), before.Id);
        Assert.IsFalse(before.HasCharacterCode);

        vm.OpenCharacters();
        vm.BeginCharacterCreation();
        vm.CharacterNameDraft = "Marlowe";
        Assert.IsTrue(vm.BeginCharacterCreationSubmission());
        Assert.IsNotNull(vm.CompleteCharacterCreationSubmission());
        vm.CloseCharactersToOverview();

        Assert.AreEqual(2, vm.OverviewCharacterRows.Count);
        Assert.IsTrue(vm.OverviewCharacterRows.All(row => row.HasCharacterCode));
        var after = vm.OverviewSceneRows.Single().Roster.Single();
        Assert.AreEqual(new CharacterId("C0"), after.Id);
        Assert.IsTrue(after.HasCharacterCode);
        Assert.AreEqual(
            vm.OverviewCharacterRows.Single(row => row.Id == after.Id).CharacterCode,
            after.CharacterCode);
    }

    [TestMethod]
    public void UncertaintySurvivesNavigationOtherProductionAndFailedOpen()
    {
        var (vm, catalog) = Start();
        catalog.Failure = ProductAccessFailureKind.Invalid;
        vm.BeginSceneDraft();
        Assert.IsNull(vm.EstablishScene());
        Assert.IsTrue(vm.HasSceneUncertainty);
        Assert.AreEqual("Last confirmed Scenes", vm.SceneListHeading);
        Assert.AreEqual("Last confirmed Scenes and their initial rosters.", vm.SceneListDescription);
        Assert.AreEqual("Choose a last confirmed Scene to inspect its initial roster.", vm.SceneListInstruction);
        StringAssert.Contains(vm.SceneNewSceneHelp, "opened again");
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
        var collisionIds = new HashSet<string>
        {
            "collision-a",
            "collision-b"
        };
        var (vm, catalog) = Start(
            identities =>
                PresentationIdentityCode.InjectFullCollisionForTest(
                    identities.ToDictionary(
                        identity => identity,
                        identity =>
                            collisionIds.Contains(identity)
                                ? new string('F', 64)
                                : PresentationIdentityCode.HashIdentity(identity),
                        StringComparer.Ordinal)));

        catalog.ExternalScene("A", "collision-a");
        catalog.ExternalScene("A", "collision-b");
        catalog.ExternalScene("A", "unambiguous");

        Open(vm, "A");
        vm.OpenScenes();
        Assert.IsTrue(vm.HasSceneCollision);
        Assert.IsFalse(vm.HasSceneUncertainty);
        StringAssert.Contains(
            vm.SceneNewSceneHelp,
            "cannot distinguish");
        Assert.IsFalse(vm.BeginSceneDraft());
        Assert.IsFalse(vm.InspectScene(new("collision-a")));
        Assert.IsTrue(vm.InspectScene(new("unambiguous")));
        Assert.AreEqual(
            new SceneId("unambiguous"),
            vm.ReturnFromSceneDetail());

        Open(vm, "A");
        vm.OpenScenes();
        Assert.IsTrue(vm.HasSceneCollision);

        Open(vm, "B");
        vm.OpenScenes();
        Assert.IsTrue(vm.BeginSceneDraft());
        Assert.IsNotNull(vm.EstablishScene());
    }

    [TestMethod]
    public void RealJournalAppendThenInvalidCannotBeResubmittedUntilFreshOpen()
    {
        var root = Path.Combine(Path.GetTempPath(), "qdesign24-" + Guid.NewGuid());
        try
        {
            var catalog = new Kymaean.Infrastructure.Persistence.FileProductionCatalog(root);
            var production = catalog.CreateProduction("Fixture").Value;
            var vm = new MainPageViewModel(PresentationStartupResult.Product(ProductApplication.Start(catalog)));
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

    [TestMethod]
    public void RealOsExclusiveJournalLockBlocksAppendAndFailedOpenPreservesWitness()
    {
        if (!OperatingSystem.IsWindows()) return; // OS evidence is native Windows only.
        var root = Path.Combine(Path.GetTempPath(), "qdesign24-lock-" + Guid.NewGuid());
        try
        {
            var catalog = new Kymaean.Infrastructure.Persistence.FileProductionCatalog(root);
            var production = catalog.CreateProduction("Locked fixture").Value;
            var vm = new MainPageViewModel(PresentationStartupResult.Product(ProductApplication.Start(catalog)));
            vm.SelectProduction(vm.ProductionRows.Single()); Assert.IsTrue(vm.OpenSelectedProduction()); vm.OpenScenes();
            var entry = Directory.GetDirectories(Path.Combine(root, "production-catalog"), "entry-*").Single();
            using (var lease = new FileStream(Path.Combine(entry, ".journal.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                vm.BeginSceneDraft(); Assert.IsNull(vm.EstablishScene());
                Assert.IsTrue(vm.HasSceneUncertainty);
                Assert.IsFalse(vm.OpenSelectedProduction());
                StringAssert.Contains(vm.StatusMessage, "couldn't open");
                Assert.IsTrue(vm.HasSceneUncertainty);
                vm.NavigateShell(ShellRoute.Home); vm.NavigateShell(ShellRoute.CurrentProduction); vm.OpenScenes();
                Assert.IsFalse(vm.BeginSceneDraft()); Assert.IsNull(vm.EstablishScene());
            }
            Assert.AreEqual(0, catalog.OpenProduction(production.Id).Value.ProductionScenes.Scenes.Length);
            Assert.IsTrue(vm.OpenSelectedProduction()); vm.OpenScenes();
            Assert.IsFalse(vm.HasSceneUncertainty); Assert.IsTrue(vm.BeginSceneDraft());
            Assert.IsNotNull(vm.EstablishScene());
        }
        finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
    }

    private static (MainPageViewModel, Catalog) Start(
        Func<IReadOnlyList<string>, IReadOnlyDictionary<string, string?>>? sceneCodeBuilder = null)
    {
        var catalog = new Catalog();
        var startup =
            PresentationStartupResult.Product(
                ProductApplication.Start(catalog));
        var vm = sceneCodeBuilder is null
            ? new MainPageViewModel(startup)
            : new MainPageViewModel(startup, sceneCodeBuilder);
        Open(vm, "A");
        vm.OpenScenes();
        return (vm, catalog);
    }
    private static void Open(MainPageViewModel vm, string id)
    {
        vm.SelectProduction(vm.ProductionRows.Single(row => row.Id.Value == id));
        Assert.IsTrue(vm.OpenSelectedProduction());
    }

    private sealed class Catalog :
        IProductionCatalog,
        IProductionSceneCreator,
        IProductionCharacterCreator,
        IProductionWorldStateWriter
    {
        private readonly ProductionSummary[] _productions = [new(new("A"), "A"), new(new("B"), "B")];
        private readonly Dictionary<string, List<EstablishedScene>> _scenes = new() { ["A"] = [], ["B"] = [] };
        private readonly Dictionary<string, WorldCurrentState> _worlds = new()
        {
            ["A"] = WorldCurrentState.Empty,
            ["B"] = WorldCurrentState.Empty
        };
        private readonly Dictionary<string, List<CharacterSummary>> _casts = new()
        {
            ["A"] = DefaultCast(),
            ["B"] = DefaultCast()
        };
        public ProductAccessFailureKind? Failure { get; set; }
        public Exception? Error { get; set; }
        public Exception? CharacterError { get; set; }
        public Exception? WorldError { get; set; }
        public bool FailOpen { get; set; }
        public Action? DuringCall { get; set; }
        public int Calls { get; private set; }
        private static List<CharacterSummary> DefaultCast() =>
            Enumerable.Range(0, 7)
                .Select(i => new CharacterSummary(new($"C{i}"), "Same"))
                .ToList();
        private ProductionCast Cast(string id) => new(_casts[id]);
        private ProductionReplayProjection Replay(string id) => new(id, _worlds[id], Cast(id), new(_scenes[id]));
        public void SetCast(string id, params string[] names) =>
            _casts[id] = names.Select((name, index) => new CharacterSummary(new($"C{index}"), name)).ToList();
        public void ExternalScene(string id, string sceneId, params string[] characterIds) =>
            _scenes[id].Add(
                new(
                    new(sceneId),
                    characterIds.Length == 0
                        ? SceneRoster.Empty
                        : new SceneRoster(characterIds.Select(value => new CharacterId(value)))));
        public void ExternalWorld(string id, params string[] truths) =>
            _worlds[id] = new WorldCurrentState(truths.Select(truth => new WorldCurrentTruth(truth)));
        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() => ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(_productions);
        public ProductAccessResult<ProductionReplayProjection> OpenProduction(ProductionId id) => FailOpen
            ? ProductAccessResult<ProductionReplayProjection>.Failure(ProductAccessFailureKind.Invalid)
            : ProductAccessResult<ProductionReplayProjection>.Success(Replay(id.Value));
        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(ProductionId id) => OpenProduction(id);
        public ProductAccessResult<CharacterCreation> CreateCharacter(ProductionId id, string characterName)
        {
            if (CharacterError is not null) throw CharacterError;
            var list = _casts[id.Value];
            var character = new CharacterSummary(new($"C{list.Count}"), characterName);
            list.Add(character);
            return ProductAccessResult<CharacterCreation>.Success(
                new CharacterCreation(character, Replay(id.Value)));
        }
        public ProductAccessResult<ProductionReplayProjection> ReplaceWorldCurrentState(
            ProductionId id,
            WorldCurrentState currentState)
        {
            if (WorldError is not null) throw WorldError;
            _worlds[id.Value] = currentState;
            return ProductAccessResult<ProductionReplayProjection>.Success(Replay(id.Value));
        }
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
