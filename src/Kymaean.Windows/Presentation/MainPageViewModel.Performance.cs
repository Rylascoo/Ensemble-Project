using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed partial class MainPageViewModel
{
    private readonly Func<Action, bool> _publish;
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
        ? "Performance is unavailable."
        : IsApplicationBusy ? "Request in progress. Navigation and changes are unavailable until it finishes."
        : _application?.LastPublication is { RenderingFailed: true, Terminal.Outcome: PerformanceOutcome.KnownSuccess } failed &&
            failed.Terminal.Request.Target.ProductionId == _projection?.CurrentProduction?.Id
            ? "Performance recorded. Its result could not be displayed."
        : IsPerformanceReopenRequired ? PerformanceFailureMessage
        : HasEarlierUnknownPerformance ? "Production freshly opened. The earlier request remains unconfirmed; any new request is a separate action."
        : "Execution foundation available in this isolated test session.";

    private string PerformanceFailureMessage
    {
        get
        {
            var result = _application?.Terminals.LastOrDefault(item =>
                item.Request.Target.ProductionId == _projection?.CurrentProduction?.Id);
            return result?.Cause switch
            {
                PerformanceFailureCause.ProductInvalid => "Performance not confirmed. Kymaean could not confirm whether this Performance was recorded.",
                PerformanceFailureCause.ProductIncompatible => "Performance unavailable in this version.",
                PerformanceFailureCause.InvocationIo or PerformanceFailureCause.InvocationAccess =>
                    "Confirmation unavailable. Kymaean could not confirm whether this Performance was recorded.",
                _ => "Technical failure. Open the Production again to reload its recorded state."
            };
        }
    }

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
        await activation.Completion!.ConfigureAwait(false);
        var delivered = 0;
        var publicationFinished = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        void Publish()
        {
            if (Interlocked.CompareExchange(ref delivered, 1, 0) != 0) return;
            try
            {
                var published = _application.Publish(activation.Request!, terminal =>
                {
                    _performancePublication = _application.LastPublication;
                    if (terminal.Projection is { } projection) _projection = projection;
                    RaisePerformanceProperties();
                });
                if (published) _performancePublication = _application.LastPublication;
                NotifyPerformanceSafely();
            }
            finally { publicationFinished.TrySetResult(); }
        }
        void RejectDispatch()
        {
            if (Interlocked.CompareExchange(ref delivered, 1, 0) != 0) return;
            // This path may run on the worker: retain truth and freeze safely, with no UI events.
            _application.RejectPublication(activation.Request!);
            publicationFinished.TrySetResult();
        }
        try { if (!_publish(Publish)) RejectDispatch(); }
        catch (Exception) { RejectDispatch(); }
        await publicationFinished.Task.ConfigureAwait(false);
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
