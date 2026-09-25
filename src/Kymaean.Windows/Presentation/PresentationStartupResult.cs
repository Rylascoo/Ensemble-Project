using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed class PresentationStartupResult
{
    private readonly ProductAccessResult<ProductApplication>? _productStartup;

    private PresentationStartupResult(
        ProductAccessResult<ProductApplication>? productStartup,
        bool isInfrastructureFailure)
    {
        _productStartup = productStartup;
        IsInfrastructureFailure = isInfrastructureFailure;
    }

    public bool IsInfrastructureFailure { get; }

    public ProductAccessResult<ProductApplication> ProductStartup =>
        !IsInfrastructureFailure && _productStartup is not null
            ? _productStartup
            : throw new InvalidOperationException(
                "Infrastructure startup failure has no Product startup result.");

    public static PresentationStartupResult Product(
        ProductAccessResult<ProductApplication> productStartup)
    {
        ArgumentNullException.ThrowIfNull(productStartup);
        return new PresentationStartupResult(productStartup, false);
    }

    public static PresentationStartupResult InfrastructureFailure() =>
        new(null, true);
}
