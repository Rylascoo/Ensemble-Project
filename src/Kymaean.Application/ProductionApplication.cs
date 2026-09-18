namespace Kymaean.Application;

public sealed class ProductionApplication
{
    private readonly IProductionEventStore _store;

    public ProductionApplication(IProductionEventStore store)
    {
        ArgumentNullException.ThrowIfNull(store);
        _store = store;
    }

    public ProductionReplayProjection Create(string productionName)
    {
        _store.Append(new ProductionCreatedEvent(productionName));
        return Open();
    }

    public ProductionReplayProjection Open() =>
        ProductionReplay.Rebuild(_store.LoadAll());

    public ProductionReplayProjection Recover() =>
        ProductionReplay.Rebuild(_store.Recover());
}
