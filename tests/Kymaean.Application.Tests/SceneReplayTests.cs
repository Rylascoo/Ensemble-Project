using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class SceneReplayTests
{
    [TestMethod]
    public void SceneIdentityAndRosterHaveDeterministicValueSemantics()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new SceneId("   "));
        Assert.ThrowsExactly<ArgumentException>(
            () => new SceneRoster(
                [new CharacterId("C-1"), new CharacterId("C-1")]));

        var cast = Cast("C-1", "C-2", "C-3");
        var first = SceneRoster.Canonicalize(
            cast,
            [new CharacterId("C-3"), new CharacterId("C-1")]);
        var same = SceneRoster.Canonicalize(
            cast,
            [new CharacterId("C-1"), new CharacterId("C-3")]);

        Assert.AreEqual(first, same);
        CollectionAssert.AreEqual(
            new[] { "C-1", "C-3" },
            first.CharacterIds.Select(id => id.Value).ToArray());
    }

    [TestMethod]
    public void ReplayProjectionRejectsSceneRosterOutsideOrOutOfCastOrder()
    {
        var cast = Cast("C-1", "C-2");

        Assert.ThrowsExactly<ArgumentException>(
            () => new ProductionReplayProjection(
                "Harbor",
                WorldCurrentState.Empty,
                cast,
                new ProductionScenes(
                    [new EstablishedScene(
                        new SceneId("S-1"),
                        new SceneRoster([new CharacterId("C-3")]))])));

        Assert.ThrowsExactly<ArgumentException>(
            () => new ProductionReplayProjection(
                "Harbor",
                WorldCurrentState.Empty,
                cast,
                new ProductionScenes(
                    [new EstablishedScene(
                        new SceneId("S-1"),
                        new SceneRoster(
                            [new CharacterId("C-2"), new CharacterId("C-1")]))])));
    }

    [TestMethod]
    public void OldHistoryReplaysWithNoScenes()
    {
        var replay = ProductionReplay.Rebuild(
            [new ProductionCreatedEvent("Harbor")]);

        Assert.IsTrue(replay.ProductionScenes.IsEmpty);
    }

    [TestMethod]
    public void SceneCannotExistBeforeProductionCreation()
    {
        ProductionEvent[] history =
        [
            new CreatorEstablishedSceneEvent(
                new SceneId("S-1"),
                SceneRoster.Empty)
        ];

        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(history));
    }

    [TestMethod]
    public void UnknownRosterCharacterAndDuplicateSceneFailClosed()
    {
        ProductionEvent[] unknownRoster =
        [
            new ProductionCreatedEvent("Harbor"),
            new CharacterCreatedEvent(new CharacterId("C-1"), "Marlowe"),
            new CreatorEstablishedSceneEvent(
                new SceneId("S-1"),
                new SceneRoster([new CharacterId("C-2")]))
        ];

        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(unknownRoster));

        ProductionEvent[] duplicateScene =
        [
            new ProductionCreatedEvent("Harbor"),
            new CreatorEstablishedSceneEvent(
                new SceneId("S-1"),
                SceneRoster.Empty),
            new CreatorEstablishedSceneEvent(
                new SceneId("S-1"),
                SceneRoster.Empty)
        ];

        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(duplicateScene));
    }

    [TestMethod]
    public void MultipleScenesPreserveEstablishmentOrderAndRostersCanonicalizeByCast()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("Harbor"),
            new CharacterCreatedEvent(new CharacterId("C-2"), "Wren"),
            new CharacterCreatedEvent(new CharacterId("C-1"), "Marlowe"),
            new CreatorEstablishedSceneEvent(
                new SceneId("S-2"),
                new SceneRoster(
                    [new CharacterId("C-1"), new CharacterId("C-2")])),
            new CreatorEstablishedSceneEvent(
                new SceneId("S-1"),
                SceneRoster.Empty)
        ];

        var replay = ProductionReplay.Rebuild(history);

        CollectionAssert.AreEqual(
            new[] { "S-2", "S-1" },
            replay.ProductionScenes.Scenes.Select(scene => scene.Id.Value).ToArray());
        CollectionAssert.AreEqual(
            new[] { "C-2", "C-1" },
            replay.ProductionScenes.Scenes[0].InitialRoster.CharacterIds
                .Select(id => id.Value)
                .ToArray());
    }

    [TestMethod]
    public void SceneEstablishmentPreservesWorldAndProductionCastWithoutCardinalityLaw()
    {
        var characters = Enumerable.Range(1, 6)
            .Select(index => new CharacterId($"C-{index}"))
            .ToArray();
        var history = new List<ProductionEvent>
        {
            new ProductionCreatedEvent("Harbor"),
            new CreatorReplacedWorldCurrentStateEvent(
                new WorldCurrentState([new WorldCurrentTruth("The tide is rising.")]))
        };
        history.AddRange(
            characters.Select(
                (id, index) => new CharacterCreatedEvent(id, $"Character {index}")));
        history.Add(
            new CreatorEstablishedSceneEvent(
                new SceneId("S-1"),
                new SceneRoster(characters.Reverse())));

        var replay = ProductionReplay.Rebuild(history);

        Assert.AreEqual("The tide is rising.", replay.WorldCurrentState.Truths.Single().Text);
        Assert.HasCount(6, replay.ProductionCast.Characters);
        Assert.HasCount(6, replay.ProductionScenes.Scenes.Single()
            .InitialRoster.CharacterIds);
        CollectionAssert.AreEqual(
            characters.Select(id => id.Value).ToArray(),
            replay.ProductionScenes.Scenes.Single().InitialRoster.CharacterIds
                .Select(id => id.Value)
                .ToArray());
    }

    [TestMethod]
    public void ApplicationSceneIdentityDoesNotDependOnE0()
    {
        Assert.IsFalse(
            typeof(SceneId).Assembly.GetReferencedAssemblies().Any(
                assembly => string.Equals(
                    assembly.Name,
                    "Ensemble.E0.Core",
                    StringComparison.Ordinal)));
    }

    private static ProductionCast Cast(params string[] ids) =>
        new(ids.Select(
            id => new CharacterSummary(new CharacterId(id), $"Name {id}")));
}
