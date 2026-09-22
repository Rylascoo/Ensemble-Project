using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class CharacterReplayTests
{
    [TestMethod]
    public void CharacterIdentityAndNameRejectBlankValues()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new CharacterId("   "));
        Assert.ThrowsExactly<ArgumentException>(
            () => new CharacterSummary(new CharacterId("C-1"), "   "));
        Assert.ThrowsExactly<ArgumentException>(
            () => new CharacterCreatedEvent(new CharacterId("C-1"), "   "));
    }

    [TestMethod]
    public void CharacterCannotExistBeforeProductionCreation()
    {
        ProductionEvent[] history =
        [
            new CharacterCreatedEvent(new CharacterId("C-1"), "Marlowe")
        ];

        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(history));
    }

    [TestMethod]
    public void DuplicateIdentityFailsClosedWhileDuplicateNamesRemainLegal()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("Harbor"),
            new CharacterCreatedEvent(new CharacterId("C-1"), "Marlowe"),
            new CharacterCreatedEvent(new CharacterId("C-2"), "Marlowe")
        ];

        var projection = ProductionReplay.Rebuild(history);

        Assert.HasCount(2, projection.ProductionCast.Characters);
        Assert.AreEqual("C-1", projection.ProductionCast.Characters[0].Id.Value);
        Assert.AreEqual("C-2", projection.ProductionCast.Characters[1].Id.Value);

        ProductionEvent[] duplicate =
        [
            .. history,
            new CharacterCreatedEvent(new CharacterId("C-1"), "Wren")
        ];

        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(duplicate));
    }

    [TestMethod]
    public void WorldReplacementPreservesProductionCast()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("Harbor"),
            new CharacterCreatedEvent(new CharacterId("C-1"), "Marlowe"),
            new CreatorReplacedWorldCurrentStateEvent(
                new WorldCurrentState(
                    [new WorldCurrentTruth("The tide is rising.")]))
        ];

        var projection = ProductionReplay.Rebuild(history);

        Assert.HasCount(1, projection.ProductionCast.Characters);
        Assert.AreEqual(
            "Marlowe",
            projection.ProductionCast.Characters.Single().CharacterName);
        Assert.AreEqual(
            "The tide is rising.",
            projection.WorldCurrentState.Truths.Single().Text);
    }

    [TestMethod]
    public void ProductionCastEqualityIsStructuralAndOrderSensitive()
    {
        var first = Cast(("C-1", "Marlowe"), ("C-2", "Wren"));
        var same = Cast(("C-1", "Marlowe"), ("C-2", "Wren"));
        var reordered = Cast(("C-2", "Wren"), ("C-1", "Marlowe"));

        Assert.AreEqual(first, same);
        Assert.AreNotEqual(first, reordered);
    }

    private static ProductionCast Cast(
        params (string Id, string Name)[] values) =>
        new(values.Select(
            value => new CharacterSummary(
                new CharacterId(value.Id),
                value.Name)));
}
