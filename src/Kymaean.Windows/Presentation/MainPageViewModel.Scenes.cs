using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed partial class MainPageViewModel
{
    private sealed record SceneUncertainty(
        IReadOnlyList<ScenePresentationRow> LastConfirmed,
        IReadOnlyList<CharacterPresentationRow> Witness,
        string Message);

    private readonly Dictionary<ProductionId, SceneUncertainty> _sceneUncertainties = new();
    private readonly HashSet<CharacterId> _sceneDraft = new();
    private IReadOnlyList<ScenePresentationRow> _sceneRows = Array.Empty<ScenePresentationRow>();
    private IReadOnlyList<CharacterPresentationRow> _sceneCast = Array.Empty<CharacterPresentationRow>();
    private SceneId? _inspectedScene;
    private bool _sceneSubmitting;
    private string _sceneStatus = string.Empty;

    private ProductionId? SceneProductionId => _projection?.CurrentProduction?.Id;
    private SceneUncertainty? Uncertainty => SceneProductionId is { } id
        ? _sceneUncertainties.GetValueOrDefault(id) : null;
    public bool IsSceneSurface => IsCurrentProduction && _currentProductionPresentation is
        CurrentProductionPresentation.Scenes or CurrentProductionPresentation.SceneDetail or
        CurrentProductionPresentation.SceneDraft or CurrentProductionPresentation.SceneSubmitting;
    public bool IsScenes => IsSceneSurface && _currentProductionPresentation == CurrentProductionPresentation.Scenes;
    public bool IsSceneDetail => IsSceneSurface && _currentProductionPresentation == CurrentProductionPresentation.SceneDetail;
    public bool IsSceneDraft => IsSceneSurface && _currentProductionPresentation is
        CurrentProductionPresentation.SceneDraft or CurrentProductionPresentation.SceneSubmitting;
    public bool IsSceneNavigationEnabled => !_sceneSubmitting;
    public bool HasSceneUncertainty => Uncertainty is not null;
    public bool HasSceneCollision => SceneRows.Any(row => !row.CanInspect);
    public bool CanEstablishScene => IsSceneSurface && !_sceneSubmitting && !HasSceneUncertainty && !HasSceneCollision;
    public IReadOnlyList<ScenePresentationRow> SceneRows => Uncertainty?.LastConfirmed ?? _sceneRows;
    public bool HasNoScenes => SceneRows.Count == 0;
    public bool HasScenes => !HasNoScenes;
    public IReadOnlyList<CharacterPresentationRow> SceneCast => _sceneCast;
    public bool HasNoSceneCast => _sceneCast.Count == 0;
    public IReadOnlyList<CharacterPresentationRow> SceneDraftRoster => _sceneCast.Where(row => _sceneDraft.Contains(row.Id)).ToArray();
    public bool HasEmptySceneDraft => SceneDraftRoster.Count == 0;
    public ScenePresentationRow? InspectedScene => SceneRows.FirstOrDefault(row => row.Id == _inspectedScene);
    public IReadOnlyList<CharacterPresentationRow> SubmittedSceneRoster => Uncertainty?.Witness ?? Array.Empty<CharacterPresentationRow>();
    public bool HasEmptySubmittedSceneRoster => SubmittedSceneRoster.Count == 0;
    public string SceneNotice => Uncertainty?.Message ?? _sceneStatus;
    public bool HasSceneNotice => SceneNotice.Length != 0;
    public string SceneListHeading => HasSceneUncertainty ? "Last confirmed Scenes" : "Scenes";
    public string SceneListDescription => HasSceneUncertainty
        ? "Last confirmed Scenes and their initial rosters."
        : "Established Scenes and their initial rosters.";
    public string SceneListInstruction => HasSceneUncertainty
        ? "Choose a last confirmed Scene to inspect its initial roster."
        : "Choose a Scene to inspect its initial roster, or establish a new Scene.";
    public string SceneNewSceneHelp => HasSceneCollision
        ? "New Scene is unavailable because Kymaean cannot distinguish some established Scenes safely."
        : HasSceneUncertainty
            ? "New Scene is unavailable until the Production is opened again and its recorded Scenes are freshly loaded."
            : string.Empty;

    public void OpenScenes()
    {
        if (_sceneSubmitting) return;
        RequireOpenProduction();
        RefreshSceneRows();
        _sceneDraft.Clear();
        _sceneStatus = string.Empty;
        _currentProductionPresentation = CurrentProductionPresentation.Scenes;
        RaiseShellProperties();
    }

    public void CloseScenes()
    {
        if (_sceneSubmitting) return;
        _sceneDraft.Clear();
        _sceneStatus = string.Empty;
        _currentProductionPresentation = CurrentProductionPresentation.Overview;
        RaiseShellProperties();
    }

    public bool InspectScene(SceneId id)
    {
        if (!IsScenes || _sceneSubmitting || !SceneRows.Any(row => row.Id == id && row.CanInspect)) return false;
        _inspectedScene = id;
        _currentProductionPresentation = CurrentProductionPresentation.SceneDetail;
        RaiseSceneProperties();
        return true;
    }

    public SceneId? ReturnFromSceneDetail()
    {
        if (!IsSceneDetail) return null;
        _currentProductionPresentation = CurrentProductionPresentation.Scenes;
        RaiseSceneProperties();
        return _inspectedScene;
    }

    public bool BeginSceneDraft()
    {
        if (!IsScenes || !CanEstablishScene) return false;
        _sceneDraft.Clear();
        _sceneStatus = string.Empty;
        _currentProductionPresentation = CurrentProductionPresentation.SceneDraft;
        RaiseSceneProperties();
        return true;
    }

    public void SetSceneCharacter(CharacterId id, bool included)
    {
        if (!IsSceneDraft || !CanEstablishScene) return;
        if (!_sceneCast.Any(row => row.Id == id)) throw new ArgumentException("Character is not in the Cast.", nameof(id));
        if (included) _sceneDraft.Add(id); else _sceneDraft.Remove(id);
        OnPropertyChanged(nameof(SceneDraftRoster));
        OnPropertyChanged(nameof(HasEmptySceneDraft));
    }

    public void CancelSceneDraft()
    {
        if (!IsSceneDraft || _sceneSubmitting) return;
        _sceneDraft.Clear();
        _currentProductionPresentation = CurrentProductionPresentation.Scenes;
        RaiseSceneProperties();
    }

    public SceneId? EstablishScene()
    {
        if (!IsSceneDraft || !CanEstablishScene || _application is null || SceneProductionId is not { } productionId) return null;
        var witness = SceneDraftRoster.ToArray();
        var lastConfirmed = SceneRows.ToArray();
        _sceneSubmitting = true;
        _currentProductionPresentation = CurrentProductionPresentation.SceneSubmitting;
        RaiseSceneProperties();
        ProductAccessResult<SceneCreation> result;
        // Only invocation IO/access failures are non-confirmation. Refresh/notification defects
        // after a successful result must propagate without reclassifying that known success.
        try
        {
            result = _application.EstablishScene(witness.Select(row => row.Id));
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            _sceneUncertainties[productionId] = new(lastConfirmed, witness,
                "Confirmation unavailable. Kymaean couldn't confirm whether this Scene was established.");
            FinishSceneSubmission();
            return null;
        }
        finally
        {
            _sceneSubmitting = false;
        }

        if (!result.IsSuccess)
        {
            switch (result.FailureKind)
            {
                case ProductAccessFailureKind.Invalid:
                    _sceneUncertainties[productionId] = new(lastConfirmed, witness,
                        "Scene not confirmed. Kymaean couldn't confirm whether this Scene was established from the Production's returned contents.");
                    break;
                case ProductAccessFailureKind.Incompatible:
                    _sceneStatus = "A Scene can't be established in this version.";
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
            FinishSceneSubmission();
            return null;
        }

        _projection = _application.Query();
        _sceneStatus = "Scene established.";
        _sceneDraft.Clear();
        _currentProductionPresentation = CurrentProductionPresentation.Scenes;
        RefreshSceneRows();
        RaiseSceneProperties();
        return result.Value.Scene.Id;
    }

    private void FinishSceneSubmission()
    {
        _sceneSubmitting = false;
        _sceneDraft.Clear();
        _currentProductionPresentation = CurrentProductionPresentation.Scenes;
        RaiseSceneProperties();
    }

    private void OnSceneProductionOpened(bool recover)
    {
        _statusMessage = !recover && SceneProductionId is { } id && _sceneUncertainties.Remove(id)
            ? "Production reopened. Current Scenes are freshly loaded. The earlier submission was not confirmed as either successful or unsuccessful."
            : string.Empty;
        _sceneDraft.Clear();
        _inspectedScene = null;
        _sceneStatus = string.Empty;
        RefreshSceneRows();
    }

    private void RefreshSceneRows()
    {
        if (_projection?.CurrentProductionReplay is not { } replay) return;
        _sceneCast = CharacterPresentationRow.Build(replay.ProductionCast.Characters);
        _sceneRows = ScenePresentationRow.Build(replay, _sceneCast);
    }

    private void RaiseSceneProperties()
    {
        foreach (var property in new[] { nameof(IsCurrentProductionOverview), nameof(IsSceneSurface), nameof(IsScenes),
            nameof(IsSceneDetail), nameof(IsSceneDraft), nameof(IsSceneNavigationEnabled), nameof(HasSceneUncertainty),
            nameof(HasSceneCollision), nameof(CanEstablishScene), nameof(SceneRows), nameof(HasNoScenes), nameof(HasScenes), nameof(SceneCast),
            nameof(HasNoSceneCast), nameof(SceneDraftRoster), nameof(HasEmptySceneDraft), nameof(InspectedScene),
            nameof(SubmittedSceneRoster), nameof(HasEmptySubmittedSceneRoster), nameof(SceneNotice), nameof(HasSceneNotice),
            nameof(SceneListHeading), nameof(SceneListDescription), nameof(SceneListInstruction), nameof(SceneNewSceneHelp) }) OnPropertyChanged(property);
    }
}
