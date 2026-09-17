namespace Kymaean.Application;

public interface IProductionEventStore
{
    IReadOnlyList<ProductionEvent> LoadAll();

    void Append(ProductionEvent productionEvent);
}
