using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed class PresentationStartupResult
{
    private readonly ProductAccessResult<ProductOperationCoordinator>? _productStartup;

    private PresentationStartupResult(
        ProductAccessResult<ProductOperationCoordinator>? productStartup,
        bool isInfrastructureFailure)
    {
        _productStartup = productStartup;
        IsInfrastructureFailure = isInfrastructureFailure;
    }

    public bool IsInfrastructureFailure { get; }

    public ProductAccessResult<ProductOperationCoordinator> ProductStartup =>
        !IsInfrastructureFailure && _productStartup is not null
            ? _productStartup
            : throw new InvalidOperationException(
                "Infrastructure startup failure has no Product startup result.");

    public static PresentationStartupResult Product(
        ProductAccessResult<ProductApplication> productStartup)
    {
        ArgumentNullException.ThrowIfNull(productStartup);
        return new PresentationStartupResult(productStartup.IsSuccess
            ? ProductAccessResult<ProductOperationCoordinator>.Success(ProductOperationCoordinator.Own(productStartup.Value))
            : ProductAccessResult<ProductOperationCoordinator>.Failure(productStartup.FailureKind), false);
    }

    internal static PresentationStartupResult Owned(ProductOperationCoordinator owner) =>
        new(ProductAccessResult<ProductOperationCoordinator>.Success(owner), false);

    public static PresentationStartupResult InfrastructureFailure() =>
        new(null, true);
}
