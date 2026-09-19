using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class ProductionReplayTests
{
    [TestMethod]
    public void CreationEventRejectsBlankProductionName()
    {
        Assert.ThrowsExactly<ArgumentException>(
            () => new ProductionCreatedEvent("   "));
    }

    [TestMethod]
    public void WorldCurrentTruthRejectsBlankText()
    {
        Assert.ThrowsExactly<ArgumentException>(
            () => new WorldCurrentTruth("   "));
    }

    [TestMethod]
    public void WorldCurrentStateCanonicalizesTruthOrder()
    {
        var first = State("The bridge is flooded.", "Dawn has broken.");
        var second = State("Dawn has broken.", "The bridge is flooded.");

        Assert.AreEqual(first, second);
        Assert.AreEqual("Dawn has broken.", first.Truths[0].Text);
        Assert.AreEqual("The bridge is flooded.", first.Truths[1].Text);
    }

    [TestMethod]
    public void WorldCurrentStateRejectsDuplicateTruth()
    {
        Assert.ThrowsExactly<ArgumentException>(
            () => State("The gate is locked.", "The gate is locked."));
    }

    [TestMethod]
    public void SingleCreationEventRebuildsProductionName()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("The Glass Harbor")
        ];

        var state = ProductionReplay.Rebuild(history);

        Assert.AreEqual("The Glass Harbor", state.ProductionName);
        Assert.IsTrue(state.WorldCurrentState.IsEmpty);
    }

    [TestMethod]
    public void WorldStateBeforeCreationFailsClosed()
    {
        ProductionEvent[] history =
        [
            new WorldCurrentStateReplacedEvent(
                State("The house is dark."))
        ];

        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(history));
    }

    [TestMethod]
    public void LatestWorldCurrentStateReplacementWinsDeterministically()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("The Glass Harbor"),
            new WorldCurrentStateReplacedEvent(
                State("The bridge is intact.")),
            new WorldCurrentStateReplacedEvent(
                State(
                    "The bridge is flooded.",
                    "The ferry has stopped running."))
        ];

        var state = ProductionReplay.Rebuild(history);

        Assert.AreEqual("The Glass Harbor", state.ProductionName);
        Assert.AreEqual(
            State(
                "The ferry has stopped running.",
                "The bridge is flooded."),
            state.WorldCurrentState);
    }

    [TestMethod]
    public void EmptyWorldCurrentStateCanBeEstablishedCausally()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("The Glass Harbor"),
            new WorldCurrentStateReplacedEvent(
                State("The bridge is flooded.")),
            new WorldCurrentStateReplacedEvent(
                WorldCurrentState.Empty)
        ];

        var state = ProductionReplay.Rebuild(history);

        Assert.IsTrue(state.WorldCurrentState.IsEmpty);
    }

    [TestMethod]
    public void EmptyHistoryFailsClosed()
    {
        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(Array.Empty<ProductionEvent>()));
    }

    [TestMethod]
    public void DuplicateCreationEventFailsClosed()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("First"),
            new ProductionCreatedEvent("Second")
        ];

        Assert.ThrowsExactly<InvalidOperationException>(
            () => ProductionReplay.Rebuild(history));
    }

    private static WorldCurrentState State(params string[] truths) =>
        new(truths.Select(truth => new WorldCurrentTruth(truth)));
}
