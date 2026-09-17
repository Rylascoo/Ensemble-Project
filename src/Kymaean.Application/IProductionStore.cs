namespace Kymaean.Application;

public interface IProductionStore
{
    WorkspaceProjection LoadCurrent();
}
