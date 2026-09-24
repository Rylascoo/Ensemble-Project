using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class ProductApplicationCharacterTests
{
    [TestMethod]
    public void CreateCharacterUpdatesReplayWithoutChangingNavigation()
    {
        var production = Summary("P-1", "Harbor");
        var before = Replay("Harbor", Cast(("C-1", "Marlowe")));
        var character = Character("C-2", "Marlowe");
        var after = Replay(
            "Harbor",
            Cast(("C-1", "Marlowe"), ("C-2", "Marlowe")));
        var catalog = new CharacterCatalog(production, before)
        {
            CreationResult =
                ProductAccessResult<CharacterCreation>.Success(
                    new CharacterCreation(character, after))
        };
        var application = Start(catalog);
        Assert.IsTrue(
            application.OpenProduction(production.Id, ProductSpace.Archive)
                .IsSuccess);

        var result = application.CreateCharacter("Marlowe");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(character, result.Value.Character);
        var projection = application.Query();
        Assert.AreEqual(ApplicationScope.CurrentProduction, projection.ActiveScope);
        Assert.AreEqual(ProductSpace.Archive, projection.ActiveProductSpace);
        Assert.AreEqual(after, projection.CurrentProductionReplay);
        Assert.HasCount(2, after.ProductionCast.Characters);
        Assert.AreEqual(1, catalog.CreateCount);
    }

    [TestMethod]
    public void CreateCharacterAcceptsConcurrentPerformanceHistoryExtension()
    {
        var production = Summary("P-1", "Harbor");
        var marlowe = Character("C-1", "Marlowe");
        var wren = Character("C-2", "Wren");
        var scene = new EstablishedScene(
            new SceneId("S-0"),
            new SceneRoster([marlowe.Id]));
        var before = new ProductionReplayProjection(
            "Harbor",
            WorldCurrentState.Empty,
            new ProductionCast([marlowe]),
            new ProductionScenes([scene]));
        var concurrent = new AcceptedPerformance(
            scene.Id,
            marlowe.Id,
            "I keep watch.",
            new CharacterCircumstance("Marlowe is keeping watch."));
        var after = new ProductionReplayProjection(
            "Harbor",
            WorldCurrentState.Empty,
            new ProductionCast([marlowe, wren]),
            new ProductionScenes([scene]),
            new AcceptedPerformanceHistory([concurrent]));
        var catalog = new CharacterCatalog(production, before)
        {
            CreationResult =
                ProductAccessResult<CharacterCreation>.Success(
                    new CharacterCreation(wren, after))
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var result = application.CreateCharacter("Wren");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(after, application.Query().CurrentProductionReplay);
    }

    [TestMethod]
    public void CreateCharacterRequiresOpenProductionAndCapability()
    {
        var production = Summary("P-1", "Harbor");
        var catalog = new CharacterCatalog(
            production,
            Replay("Harbor", ProductionCast.Empty));
        var application = Start(catalog);

        Assert.ThrowsExactly<InvalidOperationException>(
            () => application.CreateCharacter("Marlowe"));
        Assert.AreEqual(0, catalog.CreateCount);

        var readOnly = Start(
            new ReadOnlyCatalog(
                production,
                Replay("Harbor", ProductionCast.Empty)));
        Assert.IsTrue(readOnly.OpenProduction(production.Id).IsSuccess);
        Assert.ThrowsExactly<InvalidOperationException>(
            () => readOnly.CreateCharacter("Marlowe"));
    }

    [TestMethod]
    public void TypedFailurePreservesCurrentApplicationState()
    {
        var production = Summary("P-1", "Harbor");
        var before = Replay("Harbor", Cast(("C-1", "Marlowe")));
        var catalog = new CharacterCatalog(production, before)
        {
            CreationResult =
                ProductAccessResult<CharacterCreation>.Failure(
                    ProductAccessFailureKind.Incompatible)
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);
        var stateBefore = application.Query();

        var result = application.CreateCharacter("Wren");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(
            ProductAccessFailureKind.Incompatible,
            result.FailureKind);
        Assert.AreEqual(stateBefore, application.Query());
    }

    [TestMethod]
    public void ReturnedStateMustBeExactSingleCharacterAppend()
    {
        var production = Summary("P-1", "Harbor");
        var before = Replay("Harbor", Cast(("C-1", "Marlowe")));
        var character = Character("C-2", "Wren");
        var catalog = new CharacterCatalog(production, before)
        {
            CreationResult =
                ProductAccessResult<CharacterCreation>.Success(
                    new CharacterCreation(
                        character,
                        Replay(
                            "Harbor",
                            Cast(
                                ("C-1", "Marlowe"),
                                ("C-2", "Wren"),
                                ("C-3", "Extra")))))
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var result = application.CreateCharacter("Wren");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.AreEqual(before, application.Query().CurrentProductionReplay);
    }

    [TestMethod]
    public void ReturnedStateCannotChangeWorldOrName()
    {
        var production = Summary("P-1", "Harbor");
        var before = Replay("Harbor", Cast(("C-1", "Marlowe")));
        var catalog = new CharacterCatalog(production, before)
        {
            CreationResult =
                ProductAccessResult<CharacterCreation>.Success(
                    new CharacterCreation(
                        Character("C-2", "Changed"),
                        new ProductionReplayProjection(
                            "Harbor",
                            new WorldCurrentState(
                                [new WorldCurrentTruth("Invented.")]),
                            Cast(
                                ("C-1", "Marlowe"),
                                ("C-2", "Changed")))))
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var result = application.CreateCharacter("Wren");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, result.FailureKind);
        Assert.AreEqual(before, application.Query().CurrentProductionReplay);
    }

    [TestMethod]
    public void ExactCharacterNameIsNotNormalized()
    {
        var production = Summary("P-1", "Harbor");
        var before = Replay("Harbor", ProductionCast.Empty);
        var name = new string(new[] { ' ', (char)0xD800, ' ' });
        var character = new CharacterSummary(new CharacterId("C-1"), name);
        var after = Replay("Harbor", new ProductionCast([character]));
        var catalog = new CharacterCatalog(production, before)
        {
            CreationResult =
                ProductAccessResult<CharacterCreation>.Success(
                    new CharacterCreation(character, after))
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var result = application.CreateCharacter(name);

        Assert.IsTrue(result.IsSuccess);
        CollectionAssert.AreEqual(
            name.Select(value => (ushort)value).ToArray(),
            result.Value.Character.CharacterName
                .Select(value => (ushort)value).ToArray());
    }

    private static ProductApplication Start(IProductionCatalog catalog)
    {
        var started = ProductApplication.Start(catalog);
        Assert.IsTrue(started.IsSuccess);
        return started.Value;
    }

    private static ProductionSummary Summary(string id, string name) =>
        new(new ProductionId(id), name);

    private static CharacterSummary Character(string id, string name) =>
        new(new CharacterId(id), name);

    private static ProductionCast Cast(
        params (string Id, string Name)[] values) =>
        new(values.Select(value => Character(value.Id, value.Name)));

    private static ProductionReplayProjection Replay(
        string productionName,
        ProductionCast cast) =>
        new(productionName, WorldCurrentState.Empty, cast);

    private sealed class CharacterCatalog :
        IProductionCatalog,
        IProductionCharacterCreator
    {
        private readonly ProductionSummary _production;
        private readonly ProductionReplayProjection _openReplay;

        public CharacterCatalog(
            ProductionSummary production,
            ProductionReplayProjection openReplay)
        {
            _production = production;
            _openReplay = openReplay;
        }

        public ProductAccessResult<CharacterCreation> CreationResult { get; set; } =
            ProductAccessResult<CharacterCreation>.Failure(
                ProductAccessFailureKind.Invalid);

        public int CreateCount { get; private set; }

        public ProductAccessResult<IReadOnlyList<ProductionSummary>>
            ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                new[] { _production });

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId) =>
            productionId == _production.Id
                ? ProductAccessResult<ProductionReplayProjection>.Success(_openReplay)
                : ProductAccessResult<ProductionReplayProjection>.Failure(
                    ProductAccessFailureKind.Invalid);

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            OpenProduction(productionId);

        public ProductAccessResult<CharacterCreation> CreateCharacter(
            ProductionId productionId,
            string characterName)
        {
            Assert.AreEqual(_production.Id, productionId);
            CreateCount++;
            return CreationResult;
        }
    }

    private sealed class ReadOnlyCatalog : IProductionCatalog
    {
        private readonly ProductionSummary _production;
        private readonly ProductionReplayProjection _replay;

        public ReadOnlyCatalog(
            ProductionSummary production,
            ProductionReplayProjection replay)
        {
            _production = production;
            _replay = replay;
        }

        public ProductAccessResult<IReadOnlyList<ProductionSummary>>
            ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                new[] { _production });

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Success(_replay);

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Success(_replay);
    }
}
