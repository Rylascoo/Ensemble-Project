using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed partial class MainPageViewModel : INotifyPropertyChanged
{
    private const int OverviewPreviewLimit = 3;
    private readonly ProductApplication? _application;
    private ProductApplicationProjection? _projection;
    private IReadOnlyList<ProductionPresentationRow> _productionRows =
        Array.Empty<ProductionPresentationRow>();
    private ProductionPresentationRow? _selectedProductionRow;
    private IReadOnlyList<CharacterPresentationRow> _characterRows =
        Array.Empty<CharacterPresentationRow>();
    private IReadOnlyList<CharacterPresentationRow> _lastConfirmedCharacterRows =
        Array.Empty<CharacterPresentationRow>();
    private string _characterNameDraft = string.Empty;
    private string _submittedCharacterName = string.Empty;
    private string _characterCreationValidationMessage = string.Empty;
    private string _characterStatusMessage = string.Empty;
    private string _characterFailureMessage = string.Empty;
    private bool _characterConfirmationUnavailable;
    private bool _isProductionCreationFormOpen;
    private bool _isProductionCreationPending;
    private string _productionNameDraft = string.Empty;
    private string _submittedProductionName = string.Empty;
    private string _productionCreationValidationMessage = string.Empty;
    private ShellRoute _activeShellRoute = ShellRoute.Home;
    private CurrentProductionPresentation _currentProductionPresentation =
        CurrentProductionPresentation.Overview;
    private IReadOnlyList<string> _currentWorldTruths = Array.Empty<string>();
    private IReadOnlyList<string> _reviewWorldTruths = Array.Empty<string>();
    private IReadOnlyList<string> _submittedWorldTruths = Array.Empty<string>();
    private IReadOnlyList<string> _lastConfirmedWorldTruths = Array.Empty<string>();
    private IReadOnlyList<string> _notAppliedWorldTruths = Array.Empty<string>();
    private string _statusMessage = string.Empty;
    private string _worldTruthDraftValidationMessage = string.Empty;
    private string _worldTruthStatusMessage = string.Empty;
    private string _worldTruthFailureMessage = string.Empty;
    private bool _worldTruthConfirmationUnavailable;
    private readonly Func<IReadOnlyList<string>, IReadOnlyDictionary<string, string?>> _sceneCodeBuilder;

    public MainPageViewModel(PresentationStartupResult startup)
        : this(startup, PresentationIdentityCode.BuildSceneCodes)
    {
    }

    internal MainPageViewModel(
        PresentationStartupResult startup,
        Func<IReadOnlyList<string>, IReadOnlyDictionary<string, string?>> sceneCodeBuilder)
    {
        ArgumentNullException.ThrowIfNull(startup);
        ArgumentNullException.ThrowIfNull(sceneCodeBuilder);
        _sceneCodeBuilder = sceneCodeBuilder;

        if (startup.IsInfrastructureFailure)
        {
            _statusMessage =
                "Kymaean could not open your Productions.";
            return;
        }

        var productStartup = startup.ProductStartup;
        if (productStartup.IsSuccess)
        {
            _application = productStartup.Value;
            _projection = _application.Query();
            RefreshProductionRows();
        }
        else
        {
            _statusMessage = DescribeFailure(
                productStartup.FailureKind,
                "The Production library");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ShellRoute ActiveShellRoute => _activeShellRoute;

    public IReadOnlyList<ProductionSummary> Productions =>
        _projection is null
            ? Array.Empty<ProductionSummary>()
            : _projection.Productions;

    public IReadOnlyList<ProductionPresentationRow> ProductionRows =>
        _productionRows;

    public ProductionPresentationRow? SelectedProductionRow =>
        _selectedProductionRow;

    public bool HasProductions => ProductionRows.Count > 0;
    public bool HasNoProductions => !HasProductions;

    public bool HasCurrentProduction =>
        _projection?.HasCurrentProduction == true;

    public string CurrentProductionName =>
        _projection?.CurrentProduction?.ProductionName ?? string.Empty;

    public string ActiveProductSpace =>
        _projection?.ActiveProductSpace?.ToString() ?? string.Empty;

    public string PageTitle => ActiveShellRoute switch
    {
        ShellRoute.Home => "Home",
        ShellRoute.Productions => "Productions",
        ShellRoute.CurrentProduction => CurrentProductionName,
        ShellRoute.Settings => "Settings",
        _ => throw new ArgumentOutOfRangeException(nameof(ActiveShellRoute))
    };

    public string PageSummary => IsCurrentProduction
        ? "Current Production"
        : LibrarySummary;

    public bool IsHome => ActiveShellRoute == ShellRoute.Home;
    public bool IsProductions => ActiveShellRoute == ShellRoute.Productions;
    public bool IsCurrentProduction =>
        ActiveShellRoute == ShellRoute.CurrentProduction;
    public bool IsSettings => ActiveShellRoute == ShellRoute.Settings;

    public bool IsCurrentProductionOverview =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.Overview;

    public bool IsWorldTruthSurface =>
        IsCurrentProduction &&
        _currentProductionPresentation is
            CurrentProductionPresentation.WorldTruthInspection or
            CurrentProductionPresentation.WorldTruthEdit or
            CurrentProductionPresentation.WorldTruthReview or
            CurrentProductionPresentation.WorldTruthSubmitting or
            CurrentProductionPresentation.WorldTruthTypedFailure or
            CurrentProductionPresentation.WorldTruthConfirmationUnavailable;

    public bool IsWorldTruthInspection =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.WorldTruthInspection;

    public bool IsWorldTruthEdit =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.WorldTruthEdit;

    public bool IsWorldTruthReview =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.WorldTruthReview;

    public bool IsWorldTruthSubmitting =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.WorldTruthSubmitting;

    public bool IsWorldTruthTypedFailure =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.WorldTruthTypedFailure;

    public bool IsWorldTruthConfirmationUnavailable =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.WorldTruthConfirmationUnavailable;

    public bool IsCharacterSurface =>
        IsCurrentProduction &&
        _currentProductionPresentation is
            CurrentProductionPresentation.CharacterInspection or
            CurrentProductionPresentation.CharacterCreation or
            CurrentProductionPresentation.CharacterSubmitting or
            CurrentProductionPresentation.CharacterTypedFailure or
            CurrentProductionPresentation.CharacterConfirmationUnavailable;

    public bool IsCharacterInspection =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.CharacterInspection;

    public bool IsCharacterCreation =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.CharacterCreation;

    public bool IsCharacterSubmitting =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.CharacterSubmitting;

    public bool IsCharacterTypedFailure =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.CharacterTypedFailure;

    public bool IsCharacterConfirmationUnavailable =>
        IsCurrentProduction &&
        _currentProductionPresentation == CurrentProductionPresentation.CharacterConfirmationUnavailable;

    public bool CanAccessSelection =>
        _application is not null && _selectedProductionRow is not null;

    public bool HasStatusMessage =>
        !string.IsNullOrWhiteSpace(_statusMessage);

    public string StatusMessage => _statusMessage;

    public string LibrarySummary =>
        _application is null
            ? "Your Productions are unavailable."
            : Productions.Count switch
            {
                0 => "No Productions yet.",
                1 => "1 Production is available.",
                _ => $"{Productions.Count} Productions are available."
            };

    public IReadOnlyList<string> CurrentWorldTruths => _currentWorldTruths;
    public bool HasCurrentWorldTruths => _currentWorldTruths.Count > 0;
    public bool HasNoCurrentWorldTruths => !HasCurrentWorldTruths;
    public IReadOnlyList<string> OverviewWorldTruths =>
        _currentWorldTruths.Take(OverviewPreviewLimit).ToArray();
    public bool HasMoreOverviewWorldTruths =>
        _currentWorldTruths.Count > OverviewPreviewLimit;

    public ObservableCollection<WorldTruthDraftItem> WorldTruthDraftItems { get; } =
        new();

    public IReadOnlyList<string> ReviewWorldTruths => _reviewWorldTruths;
    public bool HasReviewWorldTruths => _reviewWorldTruths.Count > 0;
    public bool HasNoReviewWorldTruths => !HasReviewWorldTruths;

    public IReadOnlyList<string> SubmittedWorldTruths => _submittedWorldTruths;
    public bool HasSubmittedWorldTruths => _submittedWorldTruths.Count > 0;
    public bool HasNoSubmittedWorldTruths => !HasSubmittedWorldTruths;

    public IReadOnlyList<string> LastConfirmedWorldTruths => _lastConfirmedWorldTruths;
    public bool HasLastConfirmedWorldTruths => _lastConfirmedWorldTruths.Count > 0;
    public bool HasNoLastConfirmedWorldTruths => !HasLastConfirmedWorldTruths;

    public IReadOnlyList<string> NotAppliedWorldTruths => _notAppliedWorldTruths;
    public bool HasNotAppliedWorldTruths => _notAppliedWorldTruths.Count > 0;
    public bool HasNoNotAppliedWorldTruths => !HasNotAppliedWorldTruths;

    public string WorldTruthDraftValidationMessage =>
        _worldTruthDraftValidationMessage;

    public bool HasWorldTruthDraftValidationMessage =>
        !string.IsNullOrWhiteSpace(_worldTruthDraftValidationMessage);

    public string WorldTruthStatusMessage => _worldTruthStatusMessage;

    public bool HasWorldTruthStatusMessage =>
        !string.IsNullOrWhiteSpace(_worldTruthStatusMessage);

    public string WorldTruthFailureMessage => _worldTruthFailureMessage;

    public IReadOnlyList<CharacterPresentationRow> CharacterRows =>
        _characterRows;

    public bool HasCharacters => _characterRows.Count > 0;
    public bool HasNoCharacters => !HasCharacters;
    public IReadOnlyList<CharacterPresentationRow> OverviewCharacterRows =>
        _characterRows.Take(OverviewPreviewLimit).ToArray();
    public bool HasMoreOverviewCharacters =>
        _characterRows.Count > OverviewPreviewLimit;

    public IReadOnlyList<CharacterPresentationRow> LastConfirmedCharacterRows =>
        _lastConfirmedCharacterRows;

    public bool HasLastConfirmedCharacters =>
        _lastConfirmedCharacterRows.Count > 0;

    public bool HasNoLastConfirmedCharacters =>
        !HasLastConfirmedCharacters;

    public string SubmittedCharacterName => _submittedCharacterName;

    public string CharacterNameDraft
    {
        get => _characterNameDraft;
        set
        {
            if (string.Equals(
                    _characterNameDraft,
                    value,
                    StringComparison.Ordinal))
            {
                return;
            }

            _characterNameDraft = value;
            OnPropertyChanged();

            if (!string.IsNullOrEmpty(
                    _characterCreationValidationMessage))
            {
                SetCharacterCreationValidationMessage(string.Empty);
            }
        }
    }

    public string CharacterCreationValidationMessage =>
        _characterCreationValidationMessage;

    public bool HasCharacterCreationValidationMessage =>
        !string.IsNullOrWhiteSpace(
            _characterCreationValidationMessage);

    public string CharacterStatusMessage => _characterStatusMessage;

    public bool HasCharacterStatusMessage =>
        !string.IsNullOrWhiteSpace(_characterStatusMessage);

    public string CharacterFailureMessage => _characterFailureMessage;

    public bool HasCharacterFailureMessage =>
        !string.IsNullOrWhiteSpace(_characterFailureMessage);

    public bool IsCharacterCreationEnabled =>
        IsCharacterCreation && !_characterConfirmationUnavailable;

    public bool IsProductionCreationFormOpen =>
        _isProductionCreationFormOpen;

    public bool IsProductionCreationFormClosed =>
        !_isProductionCreationFormOpen;

    public bool IsProductionCreationPending =>
        _isProductionCreationPending;

    public bool IsProductionCreationEnabled =>
        _isProductionCreationFormOpen && !_isProductionCreationPending;

    public string ProductionNameDraft
    {
        get => _productionNameDraft;
        set
        {
            if (string.Equals(
                    _productionNameDraft,
                    value,
                    StringComparison.Ordinal))
            {
                return;
            }

            _productionNameDraft = value;
            OnPropertyChanged();

            if (!string.IsNullOrEmpty(
                    _productionCreationValidationMessage))
            {
                SetProductionCreationValidationMessage(string.Empty);
            }
        }
    }

    public string ProductionCreationValidationMessage =>
        _productionCreationValidationMessage;

    public bool HasProductionCreationValidationMessage =>
        !string.IsNullOrWhiteSpace(
            _productionCreationValidationMessage);

    public void NavigateShell(ShellRoute route)
    {
        if (_sceneSubmitting) return;
        if (!Enum.IsDefined(route))
        {
            throw new ArgumentOutOfRangeException(nameof(route));
        }

        if (route == ShellRoute.CurrentProduction && !HasCurrentProduction)
        {
            return;
        }

        if (route != ShellRoute.Productions &&
            _isProductionCreationFormOpen)
        {
            CloseProductionCreationForm();
        }

        if (_application is not null)
        {
            _projection = _application.Navigate(
                route switch
                {
                    ShellRoute.Home => ApplicationScope.Home,
                    ShellRoute.Productions => ApplicationScope.ProductionLibrary,
                    ShellRoute.CurrentProduction => ApplicationScope.CurrentProduction,
                    ShellRoute.Settings => ApplicationScope.Settings,
                    _ => throw new ArgumentOutOfRangeException(nameof(route))
                });
        }

        _statusMessage = string.Empty;
        OnPropertyChanged(nameof(StatusMessage));
        OnPropertyChanged(nameof(HasStatusMessage));
        _activeShellRoute = route;
        RaiseShellProperties();
    }

    public void SelectProduction(ProductionPresentationRow? production)
    {
        if (_sceneSubmitting) return;
        _selectedProductionRow = production;
        OnPropertyChanged(nameof(SelectedProductionRow));
        OnPropertyChanged(nameof(CanAccessSelection));
    }

    public void OpenProductionCreationForm()
    {
        if (_sceneSubmitting) return;
        if (_application is null)
        {
            return;
        }

        _statusMessage = string.Empty;
        _productionNameDraft = string.Empty;
        _submittedProductionName = string.Empty;
        _isProductionCreationFormOpen = true;
        _isProductionCreationPending = false;
        SetProductionCreationValidationMessage(string.Empty);
        OnPropertyChanged(nameof(StatusMessage));
        OnPropertyChanged(nameof(HasStatusMessage));
        RaiseProductionCreationProperties();
    }

    public void CancelProductionCreation()
    {
        if (_isProductionCreationPending)
        {
            return;
        }

        CloseProductionCreationForm();
    }

    public bool BeginProductionCreationSubmission()
    {
        if (_application is null ||
            !_isProductionCreationFormOpen ||
            _isProductionCreationPending)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(_productionNameDraft))
        {
            SetProductionCreationValidationMessage(
                "Production name is required.");
            return false;
        }

        _submittedProductionName = _productionNameDraft;
        _isProductionCreationPending = true;
        SetProductionCreationValidationMessage(string.Empty);
        RaiseProductionCreationProperties();
        return true;
    }

    public ProductionPresentationRow?
        CompleteProductionCreationSubmission()
    {
        if (_application is null || !_isProductionCreationPending)
        {
            return null;
        }

        try
        {
            var result = _application.CreateProduction(
                _submittedProductionName);

            if (!result.IsSuccess)
            {
                SetProductionCreationValidationMessage(
                    result.FailureKind switch
                    {
                        ProductAccessFailureKind.Incompatible =>
                            "A Production can't be created in this version.",
                        ProductAccessFailureKind.Invalid =>
                            "Kymaean can't create this Production because its contents are invalid.",
                        _ => throw new ArgumentOutOfRangeException()
                    });
                return null;
            }

            _projection = _application.Query();
            RefreshProductionRows(result.Value.Id);

            var createdRow = _selectedProductionRow
                ?? throw new InvalidOperationException(
                    "Created Production is missing from the current library.");

            _statusMessage = "Production created.";
            CloseProductionCreationForm();
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(HasStatusMessage));
            OnPropertyChanged(nameof(PageSummary));
            return createdRow;
        }
        catch (IOException)
        {
            _statusMessage =
                "Kymaean couldn't create this Production.";
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(HasStatusMessage));
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            _statusMessage =
                "Kymaean couldn't create this Production.";
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(HasStatusMessage));
            return null;
        }
        finally
        {
            _isProductionCreationPending = false;
            RaiseProductionCreationProperties();
        }
    }

    public bool OpenSelectedProduction() =>
        AccessSelectedProduction(recover: false);

    public bool RecoverSelectedProduction() =>
        AccessSelectedProduction(recover: true);

    public void OpenCharacters()
    {
        if (_sceneSubmitting) return;
        if (_application is null || !HasCurrentProduction)
        {
            throw new InvalidOperationException(
                "A Production must be open before inspecting its Characters.");
        }

        if (_characterConfirmationUnavailable)
        {
            _currentProductionPresentation =
                CurrentProductionPresentation.CharacterConfirmationUnavailable;
        }
        else
        {
            RefreshCharacterRows();
            ClearCharacterTransientState();
            _currentProductionPresentation =
                CurrentProductionPresentation.CharacterInspection;
        }

        RaiseCharacterProperties();
    }

    public void CloseCharactersToOverview()
    {
        if (!_characterConfirmationUnavailable)
        {
            ClearCharacterTransientState();
        }

        _currentProductionPresentation =
            CurrentProductionPresentation.Overview;
        RaiseCharacterProperties();
    }

    public void BeginCharacterCreation()
    {
        if (!IsCharacterInspection ||
            _characterConfirmationUnavailable)
        {
            return;
        }

        _characterNameDraft = string.Empty;
        _submittedCharacterName = string.Empty;
        _characterCreationValidationMessage = string.Empty;
        _characterFailureMessage = string.Empty;
        _characterStatusMessage = string.Empty;
        _lastConfirmedCharacterRows =
            Array.Empty<CharacterPresentationRow>();
        _currentProductionPresentation =
            CurrentProductionPresentation.CharacterCreation;
        RaiseCharacterProperties();
    }

    public void CancelCharacterCreation()
    {
        if (!IsCharacterCreation)
        {
            return;
        }

        ClearCharacterTransientState();
        _currentProductionPresentation =
            CurrentProductionPresentation.CharacterInspection;
        RaiseCharacterProperties();
    }

    public bool BeginCharacterCreationSubmission()
    {
        if (_application is null ||
            !IsCharacterCreation ||
            _characterConfirmationUnavailable)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(_characterNameDraft))
        {
            SetCharacterCreationValidationMessage(
                "Character name is required.");
            return false;
        }

        _submittedCharacterName = _characterNameDraft;
        _lastConfirmedCharacterRows = _characterRows.ToArray();
        _characterFailureMessage = string.Empty;
        _characterStatusMessage = string.Empty;
        _currentProductionPresentation =
            CurrentProductionPresentation.CharacterSubmitting;
        RaiseCharacterProperties();
        return true;
    }

    public CharacterPresentationRow?
        CompleteCharacterCreationSubmission()
    {
        if (_application is null || !IsCharacterSubmitting)
        {
            return null;
        }

        try
        {
            var result = _application.CreateCharacter(
                _submittedCharacterName);

            if (!result.IsSuccess)
            {
                _characterFailureMessage = result.FailureKind switch
                {
                    ProductAccessFailureKind.Incompatible =>
                        "A Character can't be created in this version.",
                    ProductAccessFailureKind.Invalid =>
                        "Kymaean can't create this Character because this Production's contents are invalid.",
                    _ => throw new ArgumentOutOfRangeException()
                };
                _currentProductionPresentation =
                    CurrentProductionPresentation.CharacterTypedFailure;
                RaiseCharacterProperties();
                return null;
            }

            _projection = _application.Query();
            var createdRow = RefreshCharacterRows(
                result.Value.Character.Id)
                ?? throw new InvalidOperationException(
                    "Created Character is missing from the authoritative Production Cast.");

            _characterConfirmationUnavailable = false;
            _characterNameDraft = string.Empty;
            _submittedCharacterName = string.Empty;
            _lastConfirmedCharacterRows =
                Array.Empty<CharacterPresentationRow>();
            _characterCreationValidationMessage = string.Empty;
            _characterFailureMessage = string.Empty;
            _characterStatusMessage = "Character created.";
            _currentProductionPresentation =
                CurrentProductionPresentation.CharacterInspection;
            RaiseCharacterProperties();
            return createdRow;
        }
        catch (IOException)
        {
            EnterCharacterConfirmationUnavailable();
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            EnterCharacterConfirmationUnavailable();
            return null;
        }
    }

    public void ReturnFromCharacterTypedFailure()
    {
        if (!IsCharacterTypedFailure)
        {
            return;
        }

        ClearCharacterTransientState();
        _currentProductionPresentation =
            CurrentProductionPresentation.CharacterInspection;
        RaiseCharacterProperties();
    }

    public void OpenWorldTruths()
    {
        if (_sceneSubmitting) return;
        RequireOpenProduction();

        if (_worldTruthConfirmationUnavailable)
        {
            _currentProductionPresentation =
                CurrentProductionPresentation.WorldTruthConfirmationUnavailable;
        }
        else
        {
            RefreshCurrentWorldTruthsFromProjection();
            _currentProductionPresentation =
                CurrentProductionPresentation.WorldTruthInspection;
        }

        RaiseWorldTruthProperties();
    }

    public void CloseWorldTruthsToOverview()
    {
        if (!_worldTruthConfirmationUnavailable)
        {
            ClearWorldTruthProposal();
        }

        _currentProductionPresentation =
            CurrentProductionPresentation.Overview;
        RaiseWorldTruthProperties();
    }

    public void BeginWorldTruthEdit()
    {
        RequireOpenProduction();
        if (_worldTruthConfirmationUnavailable)
        {
            throw new InvalidOperationException(
                "World current truth cannot be edited until authoritative state is re-established.");
        }

        WorldTruthDraftItems.Clear();
        foreach (var truth in _currentWorldTruths)
        {
            WorldTruthDraftItems.Add(new WorldTruthDraftItem(truth));
        }

        _reviewWorldTruths = Array.Empty<string>();
        _worldTruthDraftValidationMessage = string.Empty;
        _worldTruthStatusMessage = string.Empty;
        _currentProductionPresentation =
            CurrentProductionPresentation.WorldTruthEdit;
        RaiseWorldTruthProperties();
    }

    public void AddWorldTruthDraft()
    {
        if (!IsWorldTruthEdit)
        {
            return;
        }

        WorldTruthDraftItems.Add(new WorldTruthDraftItem());
        SetDraftValidationMessage(string.Empty);
    }

    public void RemoveWorldTruthDraft(WorldTruthDraftItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (!IsWorldTruthEdit)
        {
            return;
        }

        WorldTruthDraftItems.Remove(item);
        SetDraftValidationMessage(string.Empty);
    }

    public bool ReviewWorldTruthReplacement()
    {
        if (!IsWorldTruthEdit)
        {
            return false;
        }

        if (!TryCanonicalizeDraft(out var canonical))
        {
            return false;
        }

        _reviewWorldTruths = canonical;
        _currentProductionPresentation =
            CurrentProductionPresentation.WorldTruthReview;
        RaiseWorldTruthProperties();
        return true;
    }

    public void ReturnToWorldTruthEdit()
    {
        if (!IsWorldTruthReview)
        {
            return;
        }

        _currentProductionPresentation =
            CurrentProductionPresentation.WorldTruthEdit;
        RaiseWorldTruthProperties();
    }

    public void CancelWorldTruthProposal()
    {
        if (_worldTruthConfirmationUnavailable)
        {
            return;
        }

        ClearWorldTruthProposal();
        _currentProductionPresentation =
            CurrentProductionPresentation.WorldTruthInspection;
        RaiseWorldTruthProperties();
    }

    public bool BeginWorldTruthReplacementSubmission()
    {
        if (!IsWorldTruthReview || _worldTruthConfirmationUnavailable)
        {
            return false;
        }

        _submittedWorldTruths = _reviewWorldTruths.ToArray();
        _lastConfirmedWorldTruths = _currentWorldTruths.ToArray();
        _worldTruthStatusMessage = string.Empty;
        _worldTruthFailureMessage = string.Empty;
        _currentProductionPresentation =
            CurrentProductionPresentation.WorldTruthSubmitting;
        RaiseWorldTruthProperties();
        return true;
    }

    public void CompleteWorldTruthReplacementSubmission()
    {
        if (!IsWorldTruthSubmitting || _application is null)
        {
            return;
        }

        try
        {
            var replacement = new WorldCurrentState(
                _submittedWorldTruths.Select(
                    text => new WorldCurrentTruth(text)));

            var result = _application.ReplaceWorldCurrentState(replacement);
            if (!result.IsSuccess)
            {
                _notAppliedWorldTruths = _submittedWorldTruths.ToArray();
                _worldTruthFailureMessage = result.FailureKind switch
                {
                    ProductAccessFailureKind.Incompatible =>
                        "This Production's current world truths can't be changed in this version.",
                    ProductAccessFailureKind.Invalid =>
                        "Kymaean can't change this Production's current world truths because its contents are invalid.",
                    _ => throw new ArgumentOutOfRangeException()
                };
                _currentProductionPresentation =
                    CurrentProductionPresentation.WorldTruthTypedFailure;
                RaiseWorldTruthProperties();
                return;
            }

            _projection = result.Value;
            _worldTruthConfirmationUnavailable = false;
            RefreshCurrentWorldTruthsFromProjection();
            ClearWorldTruthProposal();
            _worldTruthStatusMessage =
                "Current world truths updated.";
            _currentProductionPresentation =
                CurrentProductionPresentation.WorldTruthInspection;
            RaiseWorldTruthProperties();
        }
        catch (IOException)
        {
            EnterWorldTruthConfirmationUnavailable();
        }
        catch (UnauthorizedAccessException)
        {
            EnterWorldTruthConfirmationUnavailable();
        }
    }

    private bool AccessSelectedProduction(bool recover)
    {
        if (_sceneSubmitting) return false;
        if (_application is null || _selectedProductionRow is null)
        {
            return false;
        }

        ProductAccessResult<ProductApplicationProjection> result;
        try
        {
            result = recover
                ? _application.RecoverProduction(_selectedProductionRow.Id)
                : _application.OpenProduction(_selectedProductionRow.Id);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            _statusMessage = "Kymaean couldn't open the selected Production. Its last confirmed state is unchanged.";
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(HasStatusMessage));
            return false;
        }

        if (!result.IsSuccess)
        {
            _statusMessage = DescribeFailure(
                result.FailureKind,
                "The selected Production");
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(HasStatusMessage));
            return false;
        }

        _projection = result.Value;
        OnSceneProductionOpened(recover);
        _activeShellRoute = ShellRoute.CurrentProduction;
        ResetWorldTruthPresentationFromAuthoritativeProjection();
        ResetCharacterPresentationFromAuthoritativeProjection();

        OnPropertyChanged(nameof(CurrentProductionName));
        OnPropertyChanged(nameof(ActiveProductSpace));
        OnPropertyChanged(nameof(HasCurrentProduction));
        OnPropertyChanged(nameof(StatusMessage));
        OnPropertyChanged(nameof(HasStatusMessage));
        RaiseShellProperties();
        return true;
    }

    private CharacterPresentationRow? RefreshCharacterRows(
        CharacterId? focusedId = null)
    {
        var characters =
            _projection?.CurrentProductionReplay?.ProductionCast.Characters
                .ToArray()
            ?? Array.Empty<CharacterSummary>();

        _characterRows = CharacterPresentationRow.Build(characters);

        OnPropertyChanged(nameof(CharacterRows));
        OnPropertyChanged(nameof(HasCharacters));
        OnPropertyChanged(nameof(HasNoCharacters));
        OnPropertyChanged(nameof(OverviewCharacterRows));
        OnPropertyChanged(nameof(HasMoreOverviewCharacters));

        return focusedId is null
            ? null
            : _characterRows.FirstOrDefault(
                row => row.Id == focusedId);
    }

    private void ResetCharacterPresentationFromAuthoritativeProjection()
    {
        _characterConfirmationUnavailable = false;
        RefreshCharacterRows();
        ClearCharacterTransientState();
    }

    private void ClearCharacterTransientState()
    {
        _characterNameDraft = string.Empty;
        _submittedCharacterName = string.Empty;
        _lastConfirmedCharacterRows =
            Array.Empty<CharacterPresentationRow>();
        _characterCreationValidationMessage = string.Empty;
        _characterStatusMessage = string.Empty;
        _characterFailureMessage = string.Empty;
    }

    private void EnterCharacterConfirmationUnavailable()
    {
        _characterConfirmationUnavailable = true;
        _characterCreationValidationMessage = string.Empty;
        _characterStatusMessage = string.Empty;
        _characterFailureMessage = string.Empty;
        _currentProductionPresentation =
            CurrentProductionPresentation.CharacterConfirmationUnavailable;
        RaiseCharacterProperties();
    }

    private void SetCharacterCreationValidationMessage(
        string message)
    {
        _characterCreationValidationMessage = message;
        OnPropertyChanged(
            nameof(CharacterCreationValidationMessage));
        OnPropertyChanged(
            nameof(HasCharacterCreationValidationMessage));
    }

    private void RefreshProductionRows(
        ProductionId? selectedId = null)
    {
        selectedId ??= _selectedProductionRow?.Id;

        _productionRows = ProductionPresentationRow.Build(
            Productions);
        _selectedProductionRow = selectedId is null
            ? null
            : _productionRows.FirstOrDefault(
                row => row.Id == selectedId);

        OnPropertyChanged(nameof(ProductionRows));
        OnPropertyChanged(nameof(SelectedProductionRow));
        OnPropertyChanged(nameof(HasProductions));
        OnPropertyChanged(nameof(HasNoProductions));
        OnPropertyChanged(nameof(CanAccessSelection));
        OnPropertyChanged(nameof(LibrarySummary));
    }

    private void CloseProductionCreationForm()
    {
        _isProductionCreationFormOpen = false;
        _productionNameDraft = string.Empty;
        _submittedProductionName = string.Empty;
        _productionCreationValidationMessage = string.Empty;
        OnPropertyChanged(nameof(ProductionNameDraft));
        RaiseProductionCreationProperties();
    }

    private void SetProductionCreationValidationMessage(
        string message)
    {
        _productionCreationValidationMessage = message;
        OnPropertyChanged(
            nameof(ProductionCreationValidationMessage));
        OnPropertyChanged(
            nameof(HasProductionCreationValidationMessage));
    }

    private void RaiseProductionCreationProperties()
    {
        OnPropertyChanged(
            nameof(IsProductionCreationFormOpen));
        OnPropertyChanged(
            nameof(IsProductionCreationFormClosed));
        OnPropertyChanged(
            nameof(IsProductionCreationPending));
        OnPropertyChanged(
            nameof(IsProductionCreationEnabled));
        OnPropertyChanged(nameof(ProductionNameDraft));
        OnPropertyChanged(
            nameof(ProductionCreationValidationMessage));
        OnPropertyChanged(
            nameof(HasProductionCreationValidationMessage));
    }

    private void ResetWorldTruthPresentationFromAuthoritativeProjection()
    {
        _worldTruthConfirmationUnavailable = false;
        _currentProductionPresentation =
            CurrentProductionPresentation.Overview;
        RefreshCurrentWorldTruthsFromProjection();
        ClearWorldTruthProposal();
        _lastConfirmedWorldTruths = Array.Empty<string>();
        _notAppliedWorldTruths = Array.Empty<string>();
        _worldTruthFailureMessage = string.Empty;
        _worldTruthStatusMessage = string.Empty;
    }

    private void RefreshCurrentWorldTruthsFromProjection()
    {
        _currentWorldTruths =
            _projection?.CurrentProductionReplay?.WorldCurrentState.Truths
                .Select(truth => truth.Text)
                .ToArray()
            ?? Array.Empty<string>();

        OnPropertyChanged(nameof(CurrentWorldTruths));
        OnPropertyChanged(nameof(HasCurrentWorldTruths));
        OnPropertyChanged(nameof(HasNoCurrentWorldTruths));
        OnPropertyChanged(nameof(OverviewWorldTruths));
        OnPropertyChanged(nameof(HasMoreOverviewWorldTruths));
    }

    private bool TryCanonicalizeDraft(out string[] canonical)
    {
        var values = WorldTruthDraftItems
            .Select(item => item.Text)
            .ToArray();

        if (values.Any(string.IsNullOrWhiteSpace))
        {
            canonical = Array.Empty<string>();
            SetDraftValidationMessage(
                "Each world truth needs text, or remove the empty row.");
            return false;
        }

        var unique = new HashSet<string>(StringComparer.Ordinal);
        if (values.Any(value => !unique.Add(value)))
        {
            canonical = Array.Empty<string>();
            SetDraftValidationMessage(
                "Each current world truth must be unique.");
            return false;
        }

        canonical = values
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();
        SetDraftValidationMessage(string.Empty);
        return true;
    }

    private void EnterWorldTruthConfirmationUnavailable()
    {
        _worldTruthConfirmationUnavailable = true;
        _currentProductionPresentation =
            CurrentProductionPresentation.WorldTruthConfirmationUnavailable;
        _worldTruthFailureMessage = string.Empty;
        _worldTruthStatusMessage = string.Empty;
        RaiseWorldTruthProperties();
    }

    private void ClearWorldTruthProposal()
    {
        WorldTruthDraftItems.Clear();
        _reviewWorldTruths = Array.Empty<string>();
        _submittedWorldTruths = Array.Empty<string>();
        _notAppliedWorldTruths = Array.Empty<string>();
        _worldTruthDraftValidationMessage = string.Empty;
        _worldTruthFailureMessage = string.Empty;
    }

    private void SetDraftValidationMessage(string message)
    {
        _worldTruthDraftValidationMessage = message;
        OnPropertyChanged(nameof(WorldTruthDraftValidationMessage));
        OnPropertyChanged(nameof(HasWorldTruthDraftValidationMessage));
    }

    private void RequireOpenProduction()
    {
        if (_application is null || !HasCurrentProduction)
        {
            throw new InvalidOperationException(
                "A Production must be open before inspecting its current world truths.");
        }
    }

    private void RaiseShellProperties()
    {
        OnPropertyChanged(nameof(ActiveShellRoute));
        OnPropertyChanged(nameof(IsHome));
        OnPropertyChanged(nameof(IsProductions));
        OnPropertyChanged(nameof(IsCurrentProduction));
        OnPropertyChanged(nameof(IsSettings));
        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(PageSummary));
        RaiseWorldTruthProperties();
        RaiseCharacterProperties();
        RaiseSceneProperties();
    }

    private void RaiseCharacterProperties()
    {
        OnPropertyChanged(nameof(IsCurrentProductionOverview));
        OnPropertyChanged(nameof(IsCharacterSurface));
        OnPropertyChanged(nameof(IsCharacterInspection));
        OnPropertyChanged(nameof(IsCharacterCreation));
        OnPropertyChanged(nameof(IsCharacterSubmitting));
        OnPropertyChanged(nameof(IsCharacterTypedFailure));
        OnPropertyChanged(nameof(IsCharacterConfirmationUnavailable));
        OnPropertyChanged(nameof(CharacterRows));
        OnPropertyChanged(nameof(HasCharacters));
        OnPropertyChanged(nameof(HasNoCharacters));
        OnPropertyChanged(nameof(LastConfirmedCharacterRows));
        OnPropertyChanged(nameof(HasLastConfirmedCharacters));
        OnPropertyChanged(nameof(HasNoLastConfirmedCharacters));
        OnPropertyChanged(nameof(SubmittedCharacterName));
        OnPropertyChanged(nameof(CharacterNameDraft));
        OnPropertyChanged(nameof(CharacterCreationValidationMessage));
        OnPropertyChanged(nameof(HasCharacterCreationValidationMessage));
        OnPropertyChanged(nameof(CharacterStatusMessage));
        OnPropertyChanged(nameof(HasCharacterStatusMessage));
        OnPropertyChanged(nameof(CharacterFailureMessage));
        OnPropertyChanged(nameof(HasCharacterFailureMessage));
        OnPropertyChanged(nameof(IsCharacterCreationEnabled));
    }

    private void RaiseWorldTruthProperties()
    {
        OnPropertyChanged(nameof(IsCurrentProductionOverview));
        OnPropertyChanged(nameof(IsWorldTruthSurface));
        OnPropertyChanged(nameof(IsWorldTruthInspection));
        OnPropertyChanged(nameof(IsWorldTruthEdit));
        OnPropertyChanged(nameof(IsWorldTruthReview));
        OnPropertyChanged(nameof(IsWorldTruthSubmitting));
        OnPropertyChanged(nameof(IsWorldTruthTypedFailure));
        OnPropertyChanged(nameof(IsWorldTruthConfirmationUnavailable));
        OnPropertyChanged(nameof(ReviewWorldTruths));
        OnPropertyChanged(nameof(HasReviewWorldTruths));
        OnPropertyChanged(nameof(HasNoReviewWorldTruths));
        OnPropertyChanged(nameof(SubmittedWorldTruths));
        OnPropertyChanged(nameof(HasSubmittedWorldTruths));
        OnPropertyChanged(nameof(HasNoSubmittedWorldTruths));
        OnPropertyChanged(nameof(LastConfirmedWorldTruths));
        OnPropertyChanged(nameof(HasLastConfirmedWorldTruths));
        OnPropertyChanged(nameof(HasNoLastConfirmedWorldTruths));
        OnPropertyChanged(nameof(NotAppliedWorldTruths));
        OnPropertyChanged(nameof(HasNotAppliedWorldTruths));
        OnPropertyChanged(nameof(HasNoNotAppliedWorldTruths));
        OnPropertyChanged(nameof(WorldTruthDraftValidationMessage));
        OnPropertyChanged(nameof(HasWorldTruthDraftValidationMessage));
        OnPropertyChanged(nameof(WorldTruthStatusMessage));
        OnPropertyChanged(nameof(HasWorldTruthStatusMessage));
        OnPropertyChanged(nameof(WorldTruthFailureMessage));
    }

    private static string DescribeFailure(
        ProductAccessFailureKind failureKind,
        string subject) =>
        failureKind switch
        {
            ProductAccessFailureKind.Incompatible =>
                $"Kymaean cannot open {subject.ToLowerInvariant()} in this version.",
            ProductAccessFailureKind.Invalid =>
                $"Kymaean cannot open {subject.ToLowerInvariant()} because its contents are invalid.",
            _ => throw new ArgumentOutOfRangeException(nameof(failureKind))
        };

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    private enum CurrentProductionPresentation
    {
        Overview,
        WorldTruthInspection,
        WorldTruthEdit,
        WorldTruthReview,
        WorldTruthSubmitting,
        WorldTruthTypedFailure,
        WorldTruthConfirmationUnavailable,
        CharacterInspection,
        CharacterCreation,
        CharacterSubmitting,
        CharacterTypedFailure,
        CharacterConfirmationUnavailable,
        Scenes,
        SceneDetail,
        SceneDraft,
        SceneSubmitting
    }
}
