using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed class ProductionPresentationRow
{
    private ProductionPresentationRow(
        ProductionSummary summary,
        string? productionCode)
    {
        Summary = summary;
        ProductionCode = productionCode;
    }

    public ProductionSummary Summary { get; }

    public ProductionId Id => Summary.Id;

    public string ProductionName => Summary.ProductionName;

    public string? ProductionCode { get; }

    public bool HasProductionCode =>
        !string.IsNullOrEmpty(ProductionCode);

    public string ProductionCodeLabel =>
        HasProductionCode
            ? $"Production code {ProductionCode}"
            : string.Empty;

    public string AccessibleName =>
        HasProductionCode
            ? $"{ProductionName}, Production code {ProductionCode}"
            : ProductionName;

    public static IReadOnlyList<ProductionPresentationRow> Build(
        IReadOnlyList<ProductionSummary> productions)
    {
        ArgumentNullException.ThrowIfNull(productions);

        var duplicateCodes = PresentationIdentityCode.BuildDuplicateCodes(
            productions,
            production => production.ProductionName,
            production => production.Id.Value);

        return productions
            .Select(
                production =>
                    new ProductionPresentationRow(
                        production,
                        duplicateCodes.GetValueOrDefault(
                            production.Id.Value)))
            .ToArray();
    }
}
