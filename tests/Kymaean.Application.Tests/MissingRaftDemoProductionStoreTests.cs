using Kymaean.Infrastructure.Demo;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class MissingRaftDemoProductionStoreTests
{
    [TestMethod]
    public void CanonicalFixtureProjectsIntoProductSafeWorkspaceState()
    {
        var fixturePath = Path.Combine(
            AppContext.BaseDirectory,
            "Fixtures",
            "missing-raft-0.1.0.json");

        var projection = MissingRaftDemoProductionStore
            .FromFile(fixturePath)
            .LoadCurrent();

        Assert.AreEqual("Missing Raft", projection.Studio.ProductionName);
        Assert.AreEqual(3, projection.Studio.Cast.Length);
        CollectionAssert.AreEqual(
            new[] { "Marlowe", "Dr. Voss", "Wren" },
            projection.Studio.Cast.Select(character => character.DisplayName).ToArray());
    }

    [TestMethod]
    public void CanonicalFixtureSuppliesStageAndArchiveProjection()
    {
        var fixturePath = Path.Combine(
            AppContext.BaseDirectory,
            "Fixtures",
            "missing-raft-0.1.0.json");

        var projection = MissingRaftDemoProductionStore
            .FromFile(fixturePath)
            .LoadCurrent();

        Assert.AreEqual("Missing Raft", projection.Stage.SceneName);
        Assert.AreEqual("VOSS", projection.Stage.OpportunityCharacterId);
        Assert.AreEqual(4, projection.Stage.SituationLines.Length);
        Assert.IsGreaterThan(0, projection.Archive.HistoricalEventCount);
        Assert.IsLessThanOrEqualTo(3, projection.Archive.RecentHistory.Length);
    }
}
