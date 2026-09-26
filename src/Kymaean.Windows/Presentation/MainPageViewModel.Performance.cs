using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed partial class MainPageViewModel
{
    private readonly Func<Action, bool> _publish;
    private PerformancePublication? _performancePublication;
    private PerformanceTarget? _performanceTarget;
    private bool _performanceAttempted;

    public bool IsPerformanceSurface => IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.Performance;
    public PerformanceTarget? PerformanceIdentity => IsPerformanceSurface ? _performanceTarget : null;
    public string PerformanceCharacter => _performanceTarget is { } target
        ? target.CharacterName + (target.CharacterCode is { } code ? $", Character code {code}" : "") : "";
    public string PerformanceScene => _performanceTarget is { } target ? $"Scene code {target.SceneCode}" : "";
    public string PerformanceEntryAnnouncement => $"Performance. {PerformanceScene}. {PerformanceCharacter}.";
    private PerformancePublication? CurrentPerformanceResult => IsPerformanceSurface &&
        LastPerformancePublication?.Terminal.Request.Target == _performanceTarget ? LastPerformancePublication : null;
    public bool HasRecordedPerformance => CurrentPerformanceResult?.Terminal.Outcome == PerformanceOutcome.KnownSuccess;
    public bool HasPerformanceResult => CurrentPerformanceResult is not null;
    public bool HasEmptyPerformance => HasRecordedPerformance && PerformanceText.Length == 0;
    public string PerformanceText => CurrentPerformanceResult?.Terminal.Execution?.AcceptedPerformance.VisibleText ?? "";
    public string PerformanceConsequence => CurrentPerformanceResult?.Terminal.Execution?.AcceptedPerformance.Consequence.Text ?? "";
    public string PerformanceConsequenceHeading => $"Added circumstance for {PerformanceCharacter}";
    public bool CanRequestPerformance => IsPerformanceSurface && !_performanceAttempted && !IsInputBlocked &&
        HasPerformanceExecutors && !IsPerformanceReopenRequired && _performanceTarget is not null;
    public string PerformanceOutcomeMessage => CurrentPerformanceResult is { } result
        ? result.Terminal.Outcome switch
        {
            PerformanceOutcome.KnownSuccess => result.RenderingFailed
                ? "Performance recorded. Its result could not be displayed." : "Performance recorded.",
            PerformanceOutcome.ProductIncompatible => "Performance unavailable in this version.",
            _ => PerformanceFailureMessage
        } : IsPerformanceReopenRequired ? PerformanceFailureMessage
        : !HasPerformanceExecutors ? "Performance is unavailable." : "";
    public bool HasPerformanceOutcomeMessage => PerformanceOutcomeMessage.Length != 0;
    public bool ShowPerformanceReopenHelp => IsPerformanceSurface && IsPerformanceReopenRequired;
    public bool ShowEarlierPerformanceWarning => IsPerformanceSurface && HasEarlierUnknownPerformance && !IsPerformanceReopenRequired;
    public string EarlierPerformanceWitness => _application?.Terminals.LastOrDefault(item =>
        item.Request.Target.ProductionId == _projection?.CurrentProduction?.Id && item.RequiresReopen)?.Request.Target is { } old
        ? $"Earlier unconfirmed request: Scene code {old.SceneCode}; {old.CharacterName}" +
            (old.CharacterCode is { } code ? $", Character code {code}." : ".") : "";

    public bool OpenPerformance(CharacterId actor)
    {
        if (IsInputBlocked || !IsSceneDetail || InspectedScene is not { } scene || HasSceneUncertainty || HasSceneCollision) return false;
        _application?.InvalidatePresentation();
        _performanceTarget = CapturePerformanceTarget(scene.Id, actor);
        if (_performanceTarget is null) return false;
        _performancePublication = null;
        _performanceAttempted = false;
        _currentProductionPresentation = CurrentProductionPresentation.Performance;
        RaiseSceneProperties();
        RaisePerformanceProperties();
        return true;
    }

    public CharacterId? ClosePerformance()
    {
        if (IsInputBlocked || !IsPerformanceSurface) return null;
        var actor = _performanceTarget?.CharacterId;
        _application?.InvalidatePresentation();
        _performancePublication = null;
        _performanceTarget = null;
        _currentProductionPresentation = CurrentProductionPresentation.SceneDetail;
        RaiseSceneProperties();
        RaisePerformanceProperties();
        return actor;
    }

    public Task<PerformanceAdmission> SubmitPerformanceAsync()
    {
        if (!CanRequestPerformance || _performanceTarget is null)
            return Task.FromResult(IsApplicationBusy ? PerformanceAdmission.Busy : PerformanceAdmission.Unavailable);
        _performanceAttempted = true;
        return RequestPerformanceAsync(_performanceTarget);
    }

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
        : string.Empty;

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
            nameof(PerformanceAvailabilityMessage), nameof(IsPerformanceSurface), nameof(PerformanceIdentity),
            nameof(PerformanceCharacter), nameof(PerformanceScene), nameof(CanRequestPerformance),
            nameof(HasRecordedPerformance), nameof(HasPerformanceResult), nameof(HasEmptyPerformance),
            nameof(PerformanceText), nameof(PerformanceConsequence), nameof(PerformanceConsequenceHeading),
            nameof(PerformanceOutcomeMessage), nameof(HasPerformanceOutcomeMessage),
            nameof(ShowPerformanceReopenHelp), nameof(ShowEarlierPerformanceWarning),
            nameof(EarlierPerformanceWitness) }) OnPropertyChanged(name);
    }
}
