namespace Kymaean.Application;

public sealed class WorkspaceApplication
{
    public WorkspaceApplication(IProductionStore store)
    {
        ArgumentNullException.ThrowIfNull(store);
        Projection = store.LoadCurrent()
            ?? throw new InvalidOperationException("Production store returned no projection.");
        ActiveSpace = ProductSpace.Stage;
    }

    public WorkspaceProjection Projection { get; }

    public ProductSpace ActiveSpace { get; private set; }

    public void Navigate(ProductSpace destination)
    {
        if (!Enum.IsDefined(destination))
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        ActiveSpace = destination;
    }
}
