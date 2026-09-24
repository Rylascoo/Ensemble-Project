using System.Collections.Immutable;

namespace Kymaean.Application;

public sealed class ProductApplication
{
    private readonly IProductionCatalog _catalog;
    private ImmutableArray<ProductionSummary> _productions;
    private ProductionSummary? _currentProduction;
    private ProductionReplayProjection? _currentProductionReplay;
    private ApplicationScope _activeScope = ApplicationScope.Home;
    private ProductSpace _currentProductSpace = ProductSpace.Stage;

    private ProductApplication(
        IProductionCatalog catalog,
        ImmutableArray<ProductionSummary> productions)
    {
        _catalog = catalog;
        _productions = productions;
    }

    public static ProductAccessResult<ProductApplication> Start(
        IProductionCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);

        var listed = catalog.ListProductions();
        if (!listed.IsSuccess)
        {
            return ProductAccessResult<ProductApplication>.Failure(
                listed.FailureKind);
        }

        var productions = listed.Value.ToImmutableArray();
        if (!IsValidProductionList(productions))
        {
            return ProductAccessResult<ProductApplication>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return ProductAccessResult<ProductApplication>.Success(
            new ProductApplication(catalog, productions));
    }

    public ProductApplicationProjection Query() =>
        new(
            _activeScope,
            _productions,
            _currentProduction,
            _currentProductionReplay,
            _activeScope == ApplicationScope.CurrentProduction
                ? _currentProductSpace
                : null);

    public ProductApplicationProjection Navigate(ApplicationScope destination)
    {
        if (!Enum.IsDefined(destination))
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        if (destination == ApplicationScope.CurrentProduction
            && _currentProduction is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before entering its workspace.");
        }

        _activeScope = destination;
        return Query();
    }

    public ProductAccessResult<ProductApplicationProjection> OpenProduction(
        ProductionId productionId,
        ProductSpace destination = ProductSpace.Stage)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ValidateProductSpace(destination);

        var summary = FindProduction(productionId);
        if (summary is null)
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return CompleteProductionAccess(
            summary,
            _catalog.OpenProduction(productionId),
            destination);
    }

    public ProductAccessResult<ProductApplicationProjection> RecoverProduction(
        ProductionId productionId,
        ProductSpace destination = ProductSpace.Stage)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ValidateProductSpace(destination);

        var summary = FindProduction(productionId);
        if (summary is null)
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return CompleteProductionAccess(
            summary,
            _catalog.RecoverProduction(productionId),
            destination);
    }

    public ProductAccessResult<ProductionCreation> CreateProduction(
        string productionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);

        if (_catalog is not IProductionCreator creator)
        {
            throw new InvalidOperationException(
                "The configured Production catalog does not support Production creation.");
        }

        var created = creator.CreateProduction(productionName);
        if (!created.IsSuccess)
        {
            return ProductAccessResult<ProductionCreation>.Failure(
                created.FailureKind);
        }

        var creation = created.Value;
        if (_productions.Any(item => item.Id == creation.Id) ||
            !string.Equals(
                productionName,
                creation.Replay.ProductionName,
                StringComparison.Ordinal) ||
            !creation.Replay.WorldCurrentState.IsEmpty ||
            !creation.Replay.ProductionCast.IsEmpty ||
            !creation.Replay.ProductionScenes.IsEmpty ||
            !creation.Replay.AcceptedPerformanceHistory.IsEmpty)
        {
            return ProductAccessResult<ProductionCreation>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        _productions = _productions
            .Add(creation.Summary)
            .OrderBy(item => item.Id.Value, StringComparer.Ordinal)
            .ToImmutableArray();

        return ProductAccessResult<ProductionCreation>.Success(creation);
    }

    public ProductAccessResult<CharacterCreation> CreateCharacter(
        string characterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(characterName);

        if (_currentProduction is null || _currentProductionReplay is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before establishing a Character.");
        }

        if (_catalog is not IProductionCharacterCreator creator)
        {
            throw new InvalidOperationException(
                "The configured Production catalog does not support Character creation.");
        }

        var before = _currentProductionReplay;
        var created = creator.CreateCharacter(
            _currentProduction.Id,
            characterName);
        if (!created.IsSuccess)
        {
            return ProductAccessResult<CharacterCreation>.Failure(
                created.FailureKind);
        }

        var creation = created.Value;
        var character = creation.Character;
        var replay = creation.Replay;

        if (before.ProductionCast.Characters.Any(
                existing => existing.Id == character.Id)
            || !string.Equals(
                characterName,
                character.CharacterName,
                StringComparison.Ordinal)
            || !string.Equals(
                before.ProductionName,
                replay.ProductionName,
                StringComparison.Ordinal)
            || !before.WorldCurrentState.Equals(replay.WorldCurrentState)
            || !before.ProductionScenes.Equals(replay.ProductionScenes)
            || !before.AcceptedPerformanceHistory.Equals(
                replay.AcceptedPerformanceHistory))
        {
            return ProductAccessResult<CharacterCreation>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        var expectedCast = new ProductionCast(
            before.ProductionCast.Characters.Add(character));
        if (!expectedCast.Equals(replay.ProductionCast))
        {
            return ProductAccessResult<CharacterCreation>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        _currentProductionReplay = replay;
        return ProductAccessResult<CharacterCreation>.Success(creation);
    }

    public ProductAccessResult<SceneCreation> EstablishScene(
        IEnumerable<CharacterId> initialRosterCharacterIds)
    {
        ArgumentNullException.ThrowIfNull(initialRosterCharacterIds);

        if (_currentProduction is null || _currentProductionReplay is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before establishing a Scene.");
        }

        if (_catalog is not IProductionSceneCreator creator)
        {
            throw new InvalidOperationException(
                "The configured Production catalog does not support Scene establishment.");
        }

        var before = _currentProductionReplay;
        var canonicalRoster = SceneRoster.Canonicalize(
            before.ProductionCast,
            initialRosterCharacterIds);
        var established = creator.EstablishScene(
            _currentProduction.Id,
            canonicalRoster);
        if (!established.IsSuccess)
        {
            return ProductAccessResult<SceneCreation>.Failure(
                established.FailureKind);
        }

        var creation = established.Value;
        var scene = creation.Scene;
        var replay = creation.Replay;

        if (before.ProductionScenes.Scenes.Any(existing => existing.Id == scene.Id)
            || !canonicalRoster.Equals(scene.InitialRoster)
            || !string.Equals(
                before.ProductionName,
                replay.ProductionName,
                StringComparison.Ordinal)
            || !before.WorldCurrentState.Equals(replay.WorldCurrentState)
            || !before.ProductionCast.Equals(replay.ProductionCast)
            || !before.AcceptedPerformanceHistory.Equals(
                replay.AcceptedPerformanceHistory))
        {
            return ProductAccessResult<SceneCreation>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        var expectedScenes = new ProductionScenes(
            before.ProductionScenes.Scenes.Add(scene));
        if (!expectedScenes.Equals(replay.ProductionScenes))
        {
            return ProductAccessResult<SceneCreation>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        _currentProductionReplay = replay;
        return ProductAccessResult<SceneCreation>.Success(creation);
    }

    public ProductAccessResult<PerformanceExecution> Perform(
        PerformanceOpportunity opportunity,
        IProductPerformer performer)
    {
        ArgumentNullException.ThrowIfNull(opportunity);
        ArgumentNullException.ThrowIfNull(performer);

        if (_currentProduction is null || _currentProductionReplay is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before performing a Character.");
        }

        if (_catalog is not IProductionPerformanceCommitter committer)
        {
            throw new InvalidOperationException(
                "The configured Production catalog does not support accepted Performance commits.");
        }

        var before = _currentProductionReplay;
        var scene = before.ProductionScenes.Scenes
            .SingleOrDefault(existing => existing.Id == opportunity.SceneId);
        var character = before.ProductionCast.Characters
            .SingleOrDefault(existing => existing.Id == opportunity.CharacterId);

        if (scene is null ||
            character is null ||
            !scene.InitialRoster.CharacterIds.Contains(opportunity.CharacterId))
        {
            return ProductAccessResult<PerformanceExecution>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        var context = new CharacterPerformanceContext(
            opportunity.SceneId,
            opportunity.CharacterId,
            character.CharacterName,
            before.AcceptedPerformanceHistory.Performances
                .Where(performance =>
                    performance.CharacterId == opportunity.CharacterId)
                .Select(performance => performance.Consequence));

        var candidate = performer.Perform(context)
            ?? throw new InvalidOperationException(
                "The Performer returned no Performance candidate.");

        var accepted = ProvisionalPerformanceAcceptancePolicy.Accept(
            opportunity,
            candidate);

        var committed = committer.CommitAcceptedPerformance(
            _currentProduction.Id,
            accepted);
        if (!committed.IsSuccess)
        {
            return ProductAccessResult<PerformanceExecution>.Failure(
                committed.FailureKind);
        }

        var replay = committed.Value;
        if (!string.Equals(
                before.ProductionName,
                replay.ProductionName,
                StringComparison.Ordinal)
            || !before.WorldCurrentState.Equals(replay.WorldCurrentState)
            || !before.ProductionCast.Equals(replay.ProductionCast)
            || !before.ProductionScenes.Equals(replay.ProductionScenes))
        {
            return ProductAccessResult<PerformanceExecution>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        var expectedHistory = new AcceptedPerformanceHistory(
            before.AcceptedPerformanceHistory.Performances.Add(accepted));
        if (!expectedHistory.Equals(replay.AcceptedPerformanceHistory))
        {
            return ProductAccessResult<PerformanceExecution>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        _currentProductionReplay = replay;
        return ProductAccessResult<PerformanceExecution>.Success(
            new PerformanceExecution(accepted, replay));
    }

    public ProductApplicationProjection NavigateProduction(ProductSpace destination)
    {
        if (_currentProduction is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before navigating within it.");
        }

        ValidateProductSpace(destination);
        _currentProductSpace = destination;
        _activeScope = ApplicationScope.CurrentProduction;
        return Query();
    }

    public ProductAccessResult<ProductApplicationProjection>
        ReplaceWorldCurrentState(WorldCurrentState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState);

        if (_currentProduction is null || _currentProductionReplay is null)
        {
            throw new InvalidOperationException(
                "A Production must be open before changing its World current state.");
        }

        if (_catalog is not IProductionWorldStateWriter writer)
        {
            throw new InvalidOperationException(
                "The configured Production catalog does not support World current-state mutation.");
        }

        var before = _currentProductionReplay;
        var access = writer.ReplaceWorldCurrentState(
            _currentProduction.Id,
            currentState);

        if (!access.IsSuccess)
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                access.FailureKind);
        }

        var replay = access.Value;
        if (!string.Equals(
                _currentProduction.ProductionName,
                replay.ProductionName,
                StringComparison.Ordinal)
            || !replay.WorldCurrentState.Equals(currentState)
            || !before.ProductionCast.Equals(replay.ProductionCast)
            || !before.ProductionScenes.Equals(replay.ProductionScenes)
            || !before.AcceptedPerformanceHistory.Equals(
                replay.AcceptedPerformanceHistory))
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        _currentProductionReplay = replay;

        return ProductAccessResult<ProductApplicationProjection>.Success(
            Query());
    }

    private ProductAccessResult<ProductApplicationProjection>
        CompleteProductionAccess(
            ProductionSummary summary,
            ProductAccessResult<ProductionReplayProjection> access,
            ProductSpace destination)
    {
        if (!access.IsSuccess)
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                access.FailureKind);
        }

        var replay = access.Value;
        if (!string.Equals(
                summary.ProductionName,
                replay.ProductionName,
                StringComparison.Ordinal))
        {
            return ProductAccessResult<ProductApplicationProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        _currentProduction = summary;
        _currentProductionReplay = replay;
        _currentProductSpace = destination;
        _activeScope = ApplicationScope.CurrentProduction;

        return ProductAccessResult<ProductApplicationProjection>.Success(
            Query());
    }

    private ProductionSummary? FindProduction(ProductionId productionId) =>
        _productions.FirstOrDefault(item => item.Id == productionId);

    private static void ValidateProductSpace(ProductSpace space)
    {
        if (!Enum.IsDefined(space))
        {
            throw new ArgumentOutOfRangeException(nameof(space));
        }
    }

    private static bool IsValidProductionList(
        ImmutableArray<ProductionSummary> productions)
    {
        var ids = new HashSet<ProductionId>();

        foreach (var production in productions)
        {
            if (production is null || !ids.Add(production.Id))
            {
                return false;
            }
        }

        return true;
    }
}
