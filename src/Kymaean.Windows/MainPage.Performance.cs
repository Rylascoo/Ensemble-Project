using Kymaean.Windows.Presentation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Kymaean.Windows;

public sealed partial class MainPage
{
    private void OnPerformanceClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel model ||
            sender is not Button { DataContext: CharacterPresentationRow actor } || !model.OpenPerformance(actor.Id)) return;
        UpdateLayout();
        PerformanceBackButton.Focus(FocusState.Keyboard);
        AnnounceScene(model.PerformanceEntryAnnouncement);
    }

    private async void OnRequestPerformanceClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel { CanRequestPerformance: true } model) return;
        var completion = model.SubmitPerformanceAsync();
        UpdateLayout();
        if (model.IsApplicationBusy) FocusSceneTarget(PerformancePendingTarget);
        await completion;
        if (!ReferenceEquals(DataContext, model) || !model.IsPerformanceSurface) return;
        UpdateLayout();
        // Focus is the single completion announcement. Do not also raise a live-region
        // notification or automatically narrate the potentially long result contents.
        if (model.HasPerformanceOutcomeMessage) FocusSceneTarget(PerformanceOutcomeHeading);
    }

    private void OnPerformanceBackClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainPageViewModel model || model.IsInputBlocked) return;
        var id = model.ClosePerformance();
        UpdateLayout();
        var button = Descendants<Button>(PerformanceRoster).FirstOrDefault(candidate =>
            candidate.DataContext is CharacterPresentationRow row && row.Id == id);
        FocusSceneTarget(button ?? SceneRosterHeading);
    }
}
