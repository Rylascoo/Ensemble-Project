using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private readonly WorkspaceApplication _workspace;
    private readonly Stack<ProductSpace> _productHistory = new();
    private ShellRoute _activeShellRoute = ShellRoute.Home;

    public MainPageViewModel(WorkspaceApplication workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        _workspace = workspace;

        ShowShapingCommand = new RelayCommand(() => NavigateProduct(ProductSpace.Studio));
        ShowLiveStageCommand = new RelayCommand(() => NavigateProduct(ProductSpace.Stage));
        ShowHistoryCommand = new RelayCommand(() => NavigateProduct(ProductSpace.Archive));
        BackCommand = new RelayCommand(GoBack);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand ShowShapingCommand { get; }
    public ICommand ShowLiveStageCommand { get; }
    public ICommand ShowHistoryCommand { get; }
    public ICommand BackCommand { get; }

    public ShellRoute ActiveShellRoute => _activeShellRoute;
    public ProductSpace ActiveSpace => _workspace.ActiveSpace;

    public string ProductionName => _workspace.Projection.Studio.ProductionName;
    public string CurrentSituation => _workspace.Projection.Stage.SceneName;
    public IReadOnlyList<WorkspaceCharacter> Cast => _workspace.Projection.Studio.Cast;
    public IReadOnlyList<string> SituationLines => _workspace.Projection.Stage.SituationLines;
    public IReadOnlyList<string> RecentHistory => _workspace.Projection.Archive.RecentHistory;
    public int HistoricalEventCount => _workspace.Projection.Archive.HistoricalEventCount;

    public string OpportunityDisplayName
    {
        get
        {
            var opportunityId = _workspace.Projection.Stage.OpportunityCharacterId;
            return _workspace.Projection.Stage.PresentCharacters
                .FirstOrDefault(character => character.Id == opportunityId)?.DisplayName
                ?? opportunityId;
        }
    }

    public bool IsHome => ActiveShellRoute == ShellRoute.Home;
    public bool IsProductions => ActiveShellRoute == ShellRoute.Productions;
    public bool IsSettings => ActiveShellRoute == ShellRoute.Settings;
    public bool IsShaping => ActiveSpace == ProductSpace.Studio;
    public bool IsLiveStage => ActiveSpace == ProductSpace.Stage;
    public bool IsHistory => ActiveSpace == ProductSpace.Archive;
    public bool IsBackAvailable => _productHistory.Count > 0;

    public string ActiveSpaceTitle => ActiveSpace switch
    {
        ProductSpace.Studio => "Production shaping",
        ProductSpace.Stage => "Live Stage",
        ProductSpace.Archive => "History",
        _ => throw new InvalidOperationException("Unknown product space.")
    };

    public string ActiveSpaceSupport => ActiveSpace switch
    {
        ProductSpace.Studio => "What could happen",
        ProductSpace.Stage => "What is happening",
        ProductSpace.Archive => "What happened and what remains",
        _ => throw new InvalidOperationException("Unknown product space.")
    };

    public void NavigateShell(ShellRoute route)
    {
        if (!Enum.IsDefined(route))
        {
            throw new ArgumentOutOfRangeException(nameof(route));
        }

        if (_activeShellRoute == route)
        {
            return;
        }

        _activeShellRoute = route;
        OnPropertyChanged(nameof(ActiveShellRoute));
        OnPropertyChanged(nameof(IsHome));
        OnPropertyChanged(nameof(IsProductions));
        OnPropertyChanged(nameof(IsSettings));
    }

    private void NavigateProduct(ProductSpace destination)
    {
        if (_workspace.ActiveSpace == destination)
        {
            return;
        }

        _productHistory.Push(_workspace.ActiveSpace);
        ApplyProductDestination(destination);
        OnPropertyChanged(nameof(IsBackAvailable));
    }

    private void GoBack()
    {
        if (_productHistory.Count == 0)
        {
            return;
        }

        ApplyProductDestination(_productHistory.Pop());
        OnPropertyChanged(nameof(IsBackAvailable));
    }

    private void ApplyProductDestination(ProductSpace destination)
    {
        _workspace.Navigate(destination);
        OnPropertyChanged(nameof(ActiveSpace));
        OnPropertyChanged(nameof(IsShaping));
        OnPropertyChanged(nameof(IsLiveStage));
        OnPropertyChanged(nameof(IsHistory));
        OnPropertyChanged(nameof(ActiveSpaceTitle));
        OnPropertyChanged(nameof(ActiveSpaceSupport));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
