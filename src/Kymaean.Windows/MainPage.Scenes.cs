using Kymaean.Application;
using Kymaean.Windows.Presentation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Kymaean.Windows;

public sealed partial class MainPage
{
    private void OnOpenScenesClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel model) return;
        model.OpenScenes();
        UpdateLayout();
        if (model.HasSceneUncertainty) FocusSceneNotice();
        else SceneBackButton.Focus(FocusState.Keyboard);
        AnnounceScene("Scenes. Established Scenes and their initial rosters.");
    }

    private void OnSceneBackClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel model) return;
        if (model.IsSceneDetail)
        {
            FocusScene(model.ReturnFromSceneDetail());
        }
        else if (model.IsSceneDraft)
        {
            OnCancelSceneClicked(sender, e);
        }
        else
        {
            model.CloseScenes();
            UpdateLayout();
            ScenesButton.Focus(FocusState.Keyboard);
        }
    }

    private void OnInspectSceneClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel model && sender is Button { DataContext: ScenePresentationRow row }
            && model.InspectScene(row.Id))
        {
            UpdateLayout();
            SceneBackButton.Focus(FocusState.Keyboard);
            AnnounceScene($"Initial roster. {row.CodeLabel}.");
        }
    }

    private void OnNewSceneClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel model || !model.BeginSceneDraft()) return;
        // Clear native checkbox state as well as the identity draft, including reused containers.
        foreach (var checkBox in Descendants<CheckBox>(ScenePicker)) checkBox.IsChecked = false;
        UpdateLayout();
        SceneDraftHeading.Focus(FocusState.Programmatic);
        AnnounceScene("New Scene. Choose Characters from this Production. You can leave the initial roster empty.");
    }

    private void OnSceneCharacterChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainPageViewModel model && sender is CheckBox { DataContext: CharacterPresentationRow row } checkBox)
            model.SetSceneCharacter(row.Id, checkBox.IsChecked == true);
    }

    private void OnCancelSceneClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel model) return;
        model.CancelSceneDraft();
        UpdateLayout();
        NewSceneButton.Focus(FocusState.Keyboard);
    }

    private void OnEstablishSceneClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel model) return;
        // No yield before invocation; capture and synchronous submission are one UI turn.
        var created = model.EstablishScene();
        UpdateLayout();
        if (created is not null) FocusScene(created);
        else if (model.HasSceneUncertainty) FocusSceneNotice();
        else NewSceneButton.Focus(FocusState.Keyboard);
        AnnounceScene(model.SceneNotice);
    }

    private void FocusScene(SceneId? id)
    {
        UpdateLayout();
        var button = Descendants<Button>(SceneList).FirstOrDefault(candidate =>
            candidate.DataContext is ScenePresentationRow row && row.Id == id && row.CanInspect);
        if (button is not null)
        {
            button.StartBringIntoView();
            button.Focus(FocusState.Keyboard);
        }
        else ScenesHeading.Focus(FocusState.Programmatic);
    }

    private void FocusSceneNotice()
    {
        SceneNotice.StartBringIntoView();
        SceneNotice.Focus(FocusState.Programmatic);
    }

    private void AnnounceScene(string message)
    {
        if (message.Length == 0) return;
        var peer = FrameworkElementAutomationPeer.FromElement(this) ?? new FrameworkElementAutomationPeer(this);
        peer.RaiseNotificationEvent(AutomationNotificationKind.Other,
            AutomationNotificationProcessing.ImportantMostRecent, message, "ScenePresentation");
    }

    private static IEnumerable<T> Descendants<T>(DependencyObject root) where T : DependencyObject
    {
        for (var index = 0; index < VisualTreeHelper.GetChildrenCount(root); index++)
        {
            var child = VisualTreeHelper.GetChild(root, index);
            if (child is T match) yield return match;
            foreach (var nested in Descendants<T>(child)) yield return nested;
        }
    }
}
