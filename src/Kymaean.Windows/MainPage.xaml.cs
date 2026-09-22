using Kymaean.Application;
using Kymaean.Windows.Presentation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
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

        if (e.Parameter is not WindowsStartupResult startup)
        {
            throw new InvalidOperationException(
                "Main page requires the Windows startup result.");
        }

        DataContext = new MainPageViewModel(startup);
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

        if (viewModel.ActiveShellRoute != route)
        {
            viewModel.NavigateShell(route);
        }
    }

    private void OnProductionContainerContentChanging(
        ListViewBase sender,
        ContainerContentChangingEventArgs args)
    {
        if (args.ItemContainer is ListViewItem container &&
            args.Item is ProductionSummary production)
        {
            AutomationProperties.SetName(
                container,
                production.ProductionName);
        }
    }

    private void OnProductionSelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (DataContext is MainPageViewModel viewModel &&
            sender is ListView listView)
        {
            viewModel.SelectProduction(
                listView.SelectedItem as ProductionSummary);
        }
    }

    private void OnOpenProductionClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel viewModel &&
            viewModel.OpenSelectedProduction())
        {
            ShellNavigation.SelectedItem = null;
        }
    }

    private void OnRecoverProductionClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel viewModel &&
            viewModel.RecoverSelectedProduction())
        {
            ShellNavigation.SelectedItem = null;
        }
    }

    private void OnBackToProductionsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.NavigateShell(ShellRoute.Productions);
        ShellNavigation.SelectedItem = ProductionsNavigationItem;
        ProductionsNavigationItem.Focus(FocusState.Keyboard);
    }

    private void OnOpenWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.OpenWorldTruths();
        WorldTruthBackButton.Focus(FocusState.Keyboard);
    }

    private void OnBackFromWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.CloseWorldTruthsToOverview();
        WorldTruthsButton.Focus(FocusState.Keyboard);
    }

    private void OnEditWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.BeginWorldTruthEdit();
        AddWorldTruthButton.Focus(FocusState.Keyboard);
    }

    private void OnAddWorldTruthClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel viewModel)
        {
            viewModel.AddWorldTruthDraft();
        }
    }

    private void OnRemoveWorldTruthClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel viewModel &&
            sender is Button button &&
            button.DataContext is WorldTruthDraftItem item)
        {
            viewModel.RemoveWorldTruthDraft(item);
        }
    }

    private void OnReviewWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel viewModel &&
            viewModel.ReviewWorldTruthReplacement())
        {
            ReplaceWorldTruthsButton.Focus(FocusState.Keyboard);
        }
    }

    private void OnBackToWorldTruthEditClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.ReturnToWorldTruthEdit();
        AddWorldTruthButton.Focus(FocusState.Keyboard);
    }

    private void OnCancelWorldTruthProposalClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.CancelWorldTruthProposal();
        EditWorldTruthsButton.Focus(FocusState.Keyboard);
    }

    private async void OnReplaceWorldTruthsClicked(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel viewModel ||
            !viewModel.BeginWorldTruthReplacementSubmission())
        {
            return;
        }

        await Task.Yield();
        viewModel.CompleteWorldTruthReplacementSubmission();

        if (viewModel.IsWorldTruthInspection)
        {
            EditWorldTruthsButton.Focus(FocusState.Keyboard);
        }
        else
        {
            WorldTruthBackButton.Focus(FocusState.Keyboard);
        }
    }
}
