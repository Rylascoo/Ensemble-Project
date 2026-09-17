using Kymaean.Application;
using Kymaean.Windows.Presentation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Kymaean.Windows;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is not WorkspaceApplication workspace)
        {
            throw new InvalidOperationException("Main page requires a workspace application.");
        }

        DataContext = new MainPageViewModel(workspace);
        ShellNavigation.SelectedItem = HomeNavigationItem;
    }
    private void OnNavigationSelectionChanged(
        NavigationView sender,
        NavigationViewSelectionChangedEventArgs args)
    {
        if (DataContext is not MainPageViewModel viewModel ||
            args.SelectedItemContainer?.Tag is not string routeName ||
            !Enum.TryParse<ShellRoute>(routeName, out var route))
        {
            return;
        }

        viewModel.NavigateShell(route);
    }

    private void OnBackClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel ||
            !viewModel.BackCommand.CanExecute(null))
        {
            return;
        }

        viewModel.BackCommand.Execute(null);
        FocusActiveProductControl(viewModel.ActiveSpace);
    }
    private void FocusActiveProductControl(ProductSpace space)
    {
        var control = space switch
        {
            ProductSpace.Studio => ShapingButton,
            ProductSpace.Stage => LiveStageButton,
            ProductSpace.Archive => HistoryButton,
            _ => throw new InvalidOperationException("Unknown product space.")
        };

        control.Focus(FocusState.Keyboard);
    }
}
