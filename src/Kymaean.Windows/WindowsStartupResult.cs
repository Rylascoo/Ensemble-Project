using Kymaean.Application;
using Kymaean.Windows.Presentation;

namespace Kymaean.Windows;

public sealed class WindowsStartupResult
{
    private WindowsStartupResult(PresentationStartupResult presentation)
    {
        ArgumentNullException.ThrowIfNull(presentation);
        Presentation = presentation;
    }

    internal PresentationStartupResult Presentation { get; }

    public bool IsInfrastructureFailure =>
        Presentation.IsInfrastructureFailure;

    public ProductAccessResult<ProductApplication> ProductStartup =>
        Presentation.ProductStartup;

    internal static WindowsStartupResult Product(
        ProductAccessResult<ProductApplication> productStartup)
    {
        ArgumentNullException.ThrowIfNull(productStartup);
        return new WindowsStartupResult(
            PresentationStartupResult.Product(productStartup));
    }

    internal static WindowsStartupResult InfrastructureFailure() =>
        new(PresentationStartupResult.InfrastructureFailure());
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
