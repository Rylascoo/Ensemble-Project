using System.Collections.Immutable;
using Kymaean.Application;

namespace Kymaean.Application.Tests;

[TestClass]
public sealed class WorkspaceApplicationTests
{
    [TestMethod]
    public void StartsOnStageForAnAlreadyOpenDemoProduction()
    {
        var application = new WorkspaceApplication(new StubProductionStore());

        Assert.AreEqual(ProductSpace.Stage, application.ActiveSpace);
        Assert.AreEqual("Demo", application.Projection.Studio.ProductionName);
    }

    [TestMethod]
    public void NavigationChangesOnlyTheActiveProductSpace()
    {
        var application = new WorkspaceApplication(new StubProductionStore());
        var projection = application.Projection;

        application.Navigate(ProductSpace.Studio);
        Assert.AreEqual(ProductSpace.Studio, application.ActiveSpace);
        application.Navigate(ProductSpace.Archive);
        Assert.AreEqual(ProductSpace.Archive, application.ActiveSpace);
        Assert.AreSame(projection, application.Projection);
    }

    [TestMethod]
    public void RejectsUnknownProductSpace()
    {
        var application = new WorkspaceApplication(new StubProductionStore());

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => application.Navigate((ProductSpace)99));
    }

    [TestMethod]
    public void ApplicationLayerDoesNotReferenceExperimentalCore()
    {
        var references = typeof(WorkspaceApplication).Assembly.GetReferencedAssemblies();

        Assert.IsFalse(references.Any(reference =>
            string.Equals(reference.Name, "Ensemble.E0.Core", StringComparison.Ordinal)));
    }

    private sealed class StubProductionStore : IProductionStore
    {
        public WorkspaceProjection LoadCurrent() => new(
            new StudioProjection("Demo", ImmutableArray<WorkspaceCharacter>.Empty, 1),
            new StageProjection("Scene", ImmutableArray<WorkspaceCharacter>.Empty,
                ImmutableArray<string>.Empty, "CHARACTER"),
            new ArchiveProjection(ImmutableArray<string>.Empty, 0));
    }
}
