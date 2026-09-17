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
    public void SingleCreationEventRebuildsProductionName()
    {
        ProductionEvent[] history =
        [
            new ProductionCreatedEvent("The Glass Harbor")
        ];

        var state = ProductionReplay.Rebuild(history);

        Assert.AreEqual("The Glass Harbor", state.ProductionName);
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
}
