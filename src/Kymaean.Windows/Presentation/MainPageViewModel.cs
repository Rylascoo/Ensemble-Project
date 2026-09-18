using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private readonly ProductApplication? _application;
    private ProductApplicationProjection? _projection;
    private ProductionSummary? _selectedProduction;
    private ShellRoute _activeShellRoute = ShellRoute.Home;
    private string _statusMessage = string.Empty;

    public MainPageViewModel(WindowsStartupResult startup)
    {
        ArgumentNullException.ThrowIfNull(startup);

        if (startup.IsInfrastructureFailure)
        {
            _statusMessage =
                "Kymaean could not access local app data.";
            return;
        }

        var productStartup = startup.ProductStartup;
        if (productStartup.IsSuccess)
        {
            _application = productStartup.Value;
            _projection = _application.Query();
        }
        else
        {
            _statusMessage = DescribeFailure(
                productStartup.FailureKind,
                "The local Production catalog");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ShellRoute ActiveShellRoute => _activeShellRoute;

    public IReadOnlyList<ProductionSummary> Productions =>
        _projection is null
            ? Array.Empty<ProductionSummary>()
            : _projection.Productions;

    public bool HasProductions => Productions.Count > 0;

    public bool HasCurrentProduction =>
        _projection?.HasCurrentProduction == true;

    public string CurrentProductionName =>
        _projection?.CurrentProduction?.ProductionName ?? string.Empty;

    public string ActiveProductSpace =>
        _projection?.ActiveProductSpace?.ToString() ?? string.Empty;

    public bool IsHome => ActiveShellRoute == ShellRoute.Home;
    public bool IsProductions => ActiveShellRoute == ShellRoute.Productions;
    public bool IsCurrentProduction =>
        ActiveShellRoute == ShellRoute.CurrentProduction;
    public bool IsSettings => ActiveShellRoute == ShellRoute.Settings;

    public bool CanAccessSelection =>
        _application is not null && _selectedProduction is not null;

    public bool HasStatusMessage =>
        !string.IsNullOrWhiteSpace(_statusMessage);

    public string StatusMessage => _statusMessage;

    public string LibrarySummary =>
        _application is null
            ? "The local Production catalog is unavailable."
            : Productions.Count switch
            {
                0 => "No Productions are available in local app data.",
                1 => "1 Production is available in local app data.",
                _ => $"{Productions.Count} Productions are available in local app data."
            };

    public void NavigateShell(ShellRoute route)
    {
        if (!Enum.IsDefined(route))
        {
            throw new ArgumentOutOfRangeException(nameof(route));
        }

        if (route == ShellRoute.CurrentProduction && !HasCurrentProduction)
        {
            return;
        }

        if (_application is not null)
        {
            _projection = _application.Navigate(
                route switch
                {
                    ShellRoute.Home => ApplicationScope.Home,
                    ShellRoute.Productions => ApplicationScope.ProductionLibrary,
                    ShellRoute.CurrentProduction => ApplicationScope.CurrentProduction,
                    ShellRoute.Settings => ApplicationScope.Settings,
                    _ => throw new ArgumentOutOfRangeException(nameof(route))
                });
        }

        _activeShellRoute = route;
        RaiseShellProperties();
    }

    public void SelectProduction(ProductionSummary? production)
    {
        _selectedProduction = production;
        OnPropertyChanged(nameof(CanAccessSelection));
    }

    public bool OpenSelectedProduction() =>
        AccessSelectedProduction(recover: false);

    public bool RecoverSelectedProduction() =>
        AccessSelectedProduction(recover: true);

    private bool AccessSelectedProduction(bool recover)
    {
        if (_application is null || _selectedProduction is null)
        {
            return false;
        }

        var result = recover
            ? _application.RecoverProduction(_selectedProduction.Id)
            : _application.OpenProduction(_selectedProduction.Id);

        if (!result.IsSuccess)
        {
            _statusMessage = DescribeFailure(
                result.FailureKind,
                "The selected Production");
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(HasStatusMessage));
            return false;
        }

        _projection = result.Value;
        _activeShellRoute = ShellRoute.CurrentProduction;
        _statusMessage = string.Empty;

        OnPropertyChanged(nameof(CurrentProductionName));
        OnPropertyChanged(nameof(ActiveProductSpace));
        OnPropertyChanged(nameof(HasCurrentProduction));
        OnPropertyChanged(nameof(StatusMessage));
        OnPropertyChanged(nameof(HasStatusMessage));
        RaiseShellProperties();
        return true;
    }

    private void RaiseShellProperties()
    {
        OnPropertyChanged(nameof(ActiveShellRoute));
        OnPropertyChanged(nameof(IsHome));
        OnPropertyChanged(nameof(IsProductions));
        OnPropertyChanged(nameof(IsCurrentProduction));
        OnPropertyChanged(nameof(IsSettings));
    }

    private static string DescribeFailure(
        ProductAccessFailureKind failureKind,
        string subject) =>
        failureKind switch
        {
            ProductAccessFailureKind.Incompatible =>
                $"{subject} uses an unsupported saved-data version.",
            ProductAccessFailureKind.Invalid =>
                $"{subject} contains invalid saved state.",
            _ => throw new ArgumentOutOfRangeException(nameof(failureKind))
        };

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
