using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class ProductApplicationPerformanceTests
{
    [TestMethod]
    public void PerformBuildsActorBoundedContextAndCommitsOneAtomicPerformance()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var marlowe = new CharacterSummary(new CharacterId("C-1"), "Marlowe");
        var wren = new CharacterSummary(new CharacterId("C-2"), "Wren");
        var cast = new ProductionCast([marlowe, wren]);
        var scene = new EstablishedScene(
            new SceneId("S-1"),
            new SceneRoster([marlowe.Id, wren.Id]));
        var previousMarlowe = Accepted(
            scene.Id,
            marlowe.Id,
            "Earlier.",
            "Marlowe is already watching the door.");
        var previousWren = Accepted(
            scene.Id,
            wren.Id,
            "Elsewhere.",
            "Wren is keeping her distance.");
        var before = new ProductionReplayProjection(
            "Harbor",
            new WorldCurrentState([new WorldCurrentTruth("The hidden latch is broken.")]),
            cast,
            new ProductionScenes([scene]),
            new AcceptedPerformanceHistory([previousMarlowe, previousWren]));
        var accepted = Accepted(
            scene.Id,
            marlowe.Id,
            "I brace the door.",
            "Marlowe is braced against the door.");
        var after = new ProductionReplayProjection(
            before.ProductionName,
            before.WorldCurrentState,
            before.ProductionCast,
            before.ProductionScenes,
            new AcceptedPerformanceHistory(
                before.AcceptedPerformanceHistory.Performances.Add(accepted)));
        var catalog = new PerformanceCatalog(production, before)
        {
            CommitResult = ProductAccessResult<ProductionReplayProjection>.Success(after)
        };
        var performer = new CapturingPerformer(
            new PerformanceCandidate(accepted.VisibleText));
        var interpreter = new CapturingConsequenceInterpreter(
            new CharacterCircumstanceProposal(accepted.Consequence.Text));
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var result = application.Perform(
            new PerformanceOpportunity(scene.Id, marlowe.Id),
            performer,
            interpreter);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(accepted, result.Value.AcceptedPerformance);
        Assert.AreEqual(after, result.Value.Replay);
        Assert.AreEqual(after, application.Query().CurrentProductionReplay);
        Assert.AreEqual(1, performer.InvocationCount);
        Assert.AreEqual(1, interpreter.InvocationCount);
        Assert.AreEqual(1, catalog.CommitCount);

        var context = performer.LastContext!;
        Assert.AreEqual(scene.Id, context.SceneId);
        Assert.AreEqual(marlowe.Id, context.CharacterId);
        Assert.AreEqual("Marlowe", context.CharacterName);
        CollectionAssert.AreEqual(
            new[] { previousMarlowe.Consequence },
            context.Circumstances.ToArray());

        CollectionAssert.AreEquivalent(
            new[] { "SceneId", "CharacterId", "CharacterName", "Circumstances" },
            typeof(CharacterPerformanceContext)
                .GetProperties()
                .Select(property => property.Name)
                .ToArray());
    }

    [TestMethod]
    public void UnknownSceneOrCharacterOutsideRosterFailsBeforePerformerAndCommit()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var marlowe = new CharacterSummary(new CharacterId("C-1"), "Marlowe");
        var wren = new CharacterSummary(new CharacterId("C-2"), "Wren");
        var cast = new ProductionCast([marlowe, wren]);
        var scene = new EstablishedScene(
            new SceneId("S-1"),
            new SceneRoster([marlowe.Id]));
        var replay = new ProductionReplayProjection(
            "Harbor",
            WorldCurrentState.Empty,
            cast,
            new ProductionScenes([scene]));
        var catalog = new PerformanceCatalog(production, replay);
        var performer = new CapturingPerformer(
            new PerformanceCandidate("No."));
        var interpreter = new CapturingConsequenceInterpreter(
            new CharacterCircumstanceProposal("No consequence."));
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var missingScene = application.Perform(
            new PerformanceOpportunity(new SceneId("S-404"), marlowe.Id),
            performer,
            interpreter);
        var outsideRoster = application.Perform(
            new PerformanceOpportunity(scene.Id, wren.Id),
            performer,
            interpreter);

        Assert.IsFalse(missingScene.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, missingScene.FailureKind);
        Assert.IsFalse(outsideRoster.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Invalid, outsideRoster.FailureKind);
        Assert.AreEqual(0, performer.InvocationCount);
        Assert.AreEqual(0, interpreter.InvocationCount);
        Assert.AreEqual(0, catalog.CommitCount);
        Assert.AreEqual(replay, application.Query().CurrentProductionReplay);
    }

    [TestMethod]
    public void PerformRequiresOpenProductionAndCommitCapability()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var character = new CharacterSummary(new CharacterId("C-1"), "Marlowe");
        var scene = new EstablishedScene(
            new SceneId("S-1"),
            new SceneRoster([character.Id]));
        var replay = new ProductionReplayProjection(
            "Harbor",
            WorldCurrentState.Empty,
            new ProductionCast([character]),
            new ProductionScenes([scene]));
        var performer = new CapturingPerformer(
            new PerformanceCandidate("Line."));
        var interpreter = new CapturingConsequenceInterpreter(
            new CharacterCircumstanceProposal("Marlowe is alert."));
        var opportunity = new PerformanceOpportunity(scene.Id, character.Id);

        var unopened = Start(new PerformanceCatalog(production, replay));
        Assert.ThrowsExactly<InvalidOperationException>(
            () => unopened.Perform(opportunity, performer, interpreter));

        var readOnly = Start(new ReadOnlyCatalog(production, replay));
        Assert.IsTrue(readOnly.OpenProduction(production.Id).IsSuccess);
        Assert.ThrowsExactly<InvalidOperationException>(
            () => readOnly.Perform(opportunity, performer, interpreter));
        Assert.AreEqual(0, performer.InvocationCount);
        Assert.AreEqual(0, interpreter.InvocationCount);
    }

    [TestMethod]
    public void TypedCommitFailurePreservesPriorProjection()
    {
        var production = new ProductionSummary(new ProductionId("P-1"), "Harbor");
        var character = new CharacterSummary(new CharacterId("C-1"), "Marlowe");
        var scene = new EstablishedScene(
            new SceneId("S-1"),
            new SceneRoster([character.Id]));
        var before = new ProductionReplayProjection(
            "Harbor",
            WorldCurrentState.Empty,
            new ProductionCast([character]),
            new ProductionScenes([scene]));
        var catalog = new PerformanceCatalog(production, before)
        {
            CommitResult =
                ProductAccessResult<ProductionReplayProjection>.Failure(
                    ProductAccessFailureKind.Incompatible)
        };
        var application = Start(catalog);
        Assert.IsTrue(application.OpenProduction(production.Id).IsSuccess);

        var result = application.Perform(
            new PerformanceOpportunity(scene.Id, character.Id),
            new CapturingPerformer(
                new PerformanceCandidate("Line.")),
            new CapturingConsequenceInterpreter(
                new CharacterCircumstanceProposal("Marlowe is alert.")));

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ProductAccessFailureKind.Incompatible, result.FailureKind);
        Assert.AreEqual(before, application.Query().CurrentProductionReplay);
    }

    private static AcceptedPerformance Accepted(
        SceneId sceneId,
        CharacterId characterId,
        string text,
        string circumstance) =>
        new(
            sceneId,
            characterId,
            text,
            new CharacterCircumstance(circumstance));

    private static ProductApplication Start(IProductionCatalog catalog)
    {
        var started = ProductApplication.Start(catalog);
        Assert.IsTrue(started.IsSuccess);
        return started.Value;
    }

    private sealed class CapturingPerformer : IProductPerformer
    {
        private readonly PerformanceCandidate _candidate;

        public CapturingPerformer(PerformanceCandidate candidate) =>
            _candidate = candidate;

        public int InvocationCount { get; private set; }

        public CharacterPerformanceContext? LastContext { get; private set; }

        public PerformanceCandidate Perform(CharacterPerformanceContext context)
        {
            InvocationCount++;
            LastContext = context;
            return _candidate;
        }
    }

    private sealed class CapturingConsequenceInterpreter :
        IProductConsequenceInterpreter
    {
        private readonly CharacterCircumstanceProposal _proposal;

        public CapturingConsequenceInterpreter(
            CharacterCircumstanceProposal proposal) =>
            _proposal = proposal;

        public int InvocationCount { get; private set; }

        public CharacterCircumstanceProposal Interpret(
            CharacterPerformanceContext context,
            PerformanceCandidate performance)
        {
            InvocationCount++;
            return _proposal;
        }
    }

    private sealed class PerformanceCatalog :
        IProductionCatalog,
        IProductionPerformanceCommitter
    {
        private readonly ProductionSummary _production;
        private readonly ProductionReplayProjection _replay;

        public PerformanceCatalog(
            ProductionSummary production,
            ProductionReplayProjection replay)
        {
            _production = production;
            _replay = replay;
        }

        public ProductAccessResult<ProductionReplayProjection> CommitResult { get; set; } =
            ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);

        public int CommitCount { get; private set; }

        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                new[] { _production });

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Success(_replay);

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            OpenProduction(productionId);

        public ProductAccessResult<ProductionReplayProjection> CommitAcceptedPerformance(
            ProductionId productionId,
            ProductionReplayProjection expectedSource,
            AcceptedPerformance acceptedPerformance)
        {
            Assert.AreEqual(_production.Id, productionId);
            Assert.AreEqual(expectedSource, _replay);
            CommitCount++;
            return CommitResult;
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

        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() =>
            ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
                new[] { _production });

        public ProductAccessResult<ProductionReplayProjection> OpenProduction(
            ProductionId productionId) =>
            ProductAccessResult<ProductionReplayProjection>.Success(_replay);

        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
            ProductionId productionId) =>
            OpenProduction(productionId);
    }
}
