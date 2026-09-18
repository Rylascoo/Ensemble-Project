namespace Kymaean.Application;

public interface IProductionEventStore
{
    IReadOnlyList<ProductionEvent> LoadAll();

    IReadOnlyList<ProductionEvent> Recover();

    void Append(ProductionEvent productionEvent);
}
