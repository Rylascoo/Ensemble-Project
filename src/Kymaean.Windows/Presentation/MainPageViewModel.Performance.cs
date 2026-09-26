using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed partial class MainPageViewModel
{
    private PerformancePublication? _performancePublication;

    public bool IsApplicationBusy => _application?.IsBusy == true;
    public bool IsInputBlocked => IsApplicationBusy || _sceneSubmitting || _isProductionCreationPending ||
        IsCharacterSubmitting || IsWorldTruthSubmitting;
    public bool HasPerformanceExecutors => _application?.HasExecutors == true;
    public bool IsPerformanceReopenRequired => _projection?.CurrentProduction is { } production &&
        _application?.RequiresReopen(production.Id) == true;
    public bool HasEarlierUnknownPerformance => _projection?.CurrentProduction is { } production &&
        _application?.Terminals.Any(result => result.Request.Target.ProductionId == production.Id &&
            (result.Outcome == PerformanceOutcome.OutcomeUnknown ||
             result.Outcome == PerformanceOutcome.ExecutorFailure && result.RequiresReopen)) == true;
    public PerformancePublication? LastPerformancePublication =>
        _performancePublication?.Terminal.Request.Target.ProductionId == _projection?.CurrentProduction?.Id
            ? _performancePublication : null;
    public string PerformanceAvailabilityMessage => !HasPerformanceExecutors
        ? "Performance is unavailable. An admitted execution runtime is not installed."
        : IsApplicationBusy ? "Request in progress. Navigation and changes are unavailable until it finishes."
        : IsPerformanceReopenRequired ? "Confirmation unavailable. Open this Production again before making changes. The earlier request may have been recorded."
        : HasEarlierUnknownPerformance ? "Production freshly opened. The earlier request remains unconfirmed; any new request is a separate action."
        : "Execution foundation available in this isolated test session.";

    // No ordinary UI activation is wired in this offline package. This internal seam lets
    // tests prove the full Presentation lifecycle with injected test-only executors.
    internal PerformanceTarget? CapturePerformanceTarget(SceneId scene, CharacterId actor) =>
        !IsInputBlocked && !HasSceneUncertainty && !HasSceneCollision
            ? _application?.CaptureTarget(scene, actor) : null;

    internal async Task<PerformanceAdmission> RequestPerformanceAsync(PerformanceTarget target)
    {
        if (IsInputBlocked) return PerformanceAdmission.Busy;
        if (_application is null) return PerformanceAdmission.Unavailable;
        var activation = _application.Begin(target);
        if (activation.Admission != PerformanceAdmission.Accepted) return activation.Admission;
        // A notification fault must not abandon observation/release or lose a later commit.
        NotifyPerformanceSafely();
        await activation.Completion!;
        var published = _application.Publish(activation.Request!, terminal =>
        {
            _performancePublication = _application.LastPublication;
            if (terminal.Projection is { } projection) _projection = projection;
            RaisePerformanceProperties();
        });
        if (published) _performancePublication = _application.LastPublication;
        NotifyPerformanceSafely();
        return PerformanceAdmission.Accepted;
    }

    public void DetachPerformancePresentation()
    {
        if (_application is null) return;
        _application.StateChanged -= NotifyPerformanceSafely;
        _application.InvalidatePresentation();
    }

    private void NotifyPerformanceSafely()
    {
        try { RaisePerformanceProperties(); }
        catch (Exception) { /* Stored terminal truth never depends on a UI subscriber. */ }
    }

    private void RaisePerformanceProperties()
    {
        foreach (var name in new[] { nameof(IsApplicationBusy), nameof(IsInputBlocked),
            nameof(IsSceneNavigationEnabled), nameof(CanAccessSelection), nameof(CanEstablishScene),
            nameof(IsCharacterCreationEnabled), nameof(IsProductionCreationEnabled),
            nameof(HasPerformanceExecutors), nameof(IsPerformanceReopenRequired),
            nameof(HasEarlierUnknownPerformance), nameof(LastPerformancePublication),
            nameof(PerformanceAvailabilityMessage) }) OnPropertyChanged(name);
    }
}
