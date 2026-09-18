using Kymaean.Application;

namespace Kymaean.Windows;

public sealed class WindowsStartupResult
{
    private readonly ProductAccessResult<ProductApplication>? _productStartup;

    private WindowsStartupResult(
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

    internal static WindowsStartupResult Product(
        ProductAccessResult<ProductApplication> productStartup)
    {
        ArgumentNullException.ThrowIfNull(productStartup);
        return new WindowsStartupResult(productStartup, false);
    }

    internal static WindowsStartupResult InfrastructureFailure() =>
        new(null, true);
}

internal static class WindowsStartupComposition
{
    public static WindowsStartupResult Compose(
        Func<ProductAccessResult<ProductApplication>> composeProduct)
    {
        ArgumentNullException.ThrowIfNull(composeProduct);

        try
        {
            return WindowsStartupResult.Product(composeProduct());
        }
        catch (IOException)
        {
            return WindowsStartupResult.InfrastructureFailure();
        }
        catch (UnauthorizedAccessException)
        {
            return WindowsStartupResult.InfrastructureFailure();
        }
    }
}
