namespace Kymaean.Application;

public sealed record ProductionId
{
    public ProductionId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}

public sealed record ProductionSummary
{
    public ProductionSummary(ProductionId id, string productionName)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);
        Id = id;
        ProductionName = productionName;
    }

    public ProductionId Id { get; }

    public string ProductionName { get; }
}
