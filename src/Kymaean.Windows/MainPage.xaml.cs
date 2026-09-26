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

        DataContext = new MainPageViewModel(startup.Presentation, work => DispatcherQueue.TryEnqueue(() => work()));
        ShellNavigation.SelectedItem = HomeNavigationItem;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        if (DataContext is MainPageViewModel viewModel) viewModel.DetachPerformancePresentation();
        base.OnNavigatedFrom(e);
    }

    private void OnNavigationSelectionChanged(
        NavigationView sender,
        NavigationViewSelectionChangedEventArgs args)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
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
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (args.ItemContainer is ListViewItem container &&
            args.Item is ProductionPresentationRow production)
        {
            AutomationProperties.SetName(
                container,
                production.AccessibleName);
        }
    }

    private void OnProductionSelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is MainPageViewModel viewModel &&
            sender is ListView listView)
        {
            viewModel.SelectProduction(
                listView.SelectedItem as ProductionPresentationRow);
        }
    }

    private void OnNewProductionClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.OpenProductionCreationForm();
        ProductionNameTextBox.Focus(FocusState.Keyboard);
    }

    private void OnCancelProductionCreationClicked(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.CancelProductionCreation();
        NewProductionButton.Focus(FocusState.Keyboard);
    }

    private async void OnCreateProductionClicked(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel ||
            !viewModel.BeginProductionCreationSubmission())
        {
            return;
        }

        var created = viewModel.CompleteProductionCreationSubmission();
        if (created is null)
        {
            ProductionNameTextBox.Focus(FocusState.Keyboard);
            return;
        }

        ProductionList.SelectedItem = created;
        ProductionList.ScrollIntoView(created);
        await Task.Yield();

        if (ProductionList.ContainerFromItem(created)
            is ListViewItem container)
        {
            container.Focus(FocusState.Keyboard);
        }
        else
        {
            ProductionList.Focus(FocusState.Keyboard);
        }
    }

    private void OnOpenProductionClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is MainPageViewModel viewModel &&
            viewModel.OpenSelectedProduction())
        {
            ShellNavigation.SelectedItem = null;
            AnnounceScene(viewModel.StatusMessage);
        }
    }

    private void OnRecoverProductionClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is MainPageViewModel viewModel &&
            viewModel.RecoverSelectedProduction())
        {
            ShellNavigation.SelectedItem = null;
        }
    }

    private void OnBackToProductionsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.NavigateShell(ShellRoute.Productions);
        ShellNavigation.SelectedItem = ProductionsNavigationItem;
        ProductionsNavigationItem.Focus(FocusState.Keyboard);
    }

    private void OnCharacterContainerContentChanging(
        ListViewBase sender,
        ContainerContentChangingEventArgs args)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (args.ItemContainer is ListViewItem container &&
            args.Item is CharacterPresentationRow character)
        {
            AutomationProperties.SetName(
                container,
                character.AccessibleName);
        }
    }

    private void OnSceneContainerContentChanging(
        ListViewBase sender,
        ContainerContentChangingEventArgs args)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (args.ItemContainer is ListViewItem container &&
            args.Item is ScenePresentationRow scene)
        {
            AutomationProperties.SetName(container, scene.CodeLabel);
        }
    }

    private void OnOpenCharactersClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.OpenCharacters();
        CharacterBackButton.Focus(FocusState.Keyboard);
    }

    private void OnBackFromCharactersClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.CloseCharactersToOverview();
        CharactersButton.Focus(FocusState.Keyboard);
    }

    private void OnNewCharacterClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.BeginCharacterCreation();
        CharacterNameTextBox.Focus(FocusState.Keyboard);
    }

    private void OnCancelCharacterCreationClicked(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.CancelCharacterCreation();
        NewCharacterButton.Focus(FocusState.Keyboard);
    }

    private async void OnCreateCharacterClicked(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel ||
            !viewModel.BeginCharacterCreationSubmission())
        {
            CharacterNameTextBox.Focus(FocusState.Keyboard);
            return;
        }

        var created = viewModel.CompleteCharacterCreationSubmission();
        if (created is null)
        {
            if (viewModel.IsCharacterTypedFailure)
            {
                CharacterTypedFailureBackButton.Focus(FocusState.Keyboard);
            }
            else
            {
                CharacterBackButton.Focus(FocusState.Keyboard);
            }

            return;
        }

        CharacterList.ScrollIntoView(created);
        await Task.Yield();

        if (CharacterList.ContainerFromItem(created)
            is ListViewItem container)
        {
            container.Focus(FocusState.Keyboard);
        }
        else
        {
            CharacterList.Focus(FocusState.Keyboard);
        }
    }

    private void OnBackFromCharacterFailureClicked(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.ReturnFromCharacterTypedFailure();
        NewCharacterButton.Focus(FocusState.Keyboard);
    }

    private void OnOpenWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.OpenWorldTruths();
        WorldTruthBackButton.Focus(FocusState.Keyboard);
    }

    private void OnBackFromWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.CloseWorldTruthsToOverview();
        WorldTruthsButton.Focus(FocusState.Keyboard);
    }

    private void OnEditWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.BeginWorldTruthEdit();
        AddWorldTruthButton.Focus(FocusState.Keyboard);
    }

    private void OnAddWorldTruthClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is MainPageViewModel viewModel)
        {
            viewModel.AddWorldTruthDraft();
        }
    }

    private void OnRemoveWorldTruthClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is MainPageViewModel viewModel &&
            sender is Button button &&
            button.DataContext is WorldTruthDraftItem item)
        {
            viewModel.RemoveWorldTruthDraft(item);
        }
    }

    private void OnReviewWorldTruthsClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is MainPageViewModel viewModel &&
            viewModel.ReviewWorldTruthReplacement())
        {
            ReplaceWorldTruthsButton.Focus(FocusState.Keyboard);
        }
    }

    private void OnBackToWorldTruthEditClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
        if (DataContext is not MainPageViewModel viewModel)
        {
            return;
        }

        viewModel.ReturnToWorldTruthEdit();
        AddWorldTruthButton.Focus(FocusState.Keyboard);
    }

    private void OnCancelWorldTruthProposalClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
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
        if (DataContext is MainPageViewModel { IsApplicationBusy: true }) return;
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
