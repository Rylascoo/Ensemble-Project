using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

public sealed class FileProductionCatalog :
    IProductionCatalog,
    IProductionWorldStateWriter,
    IProductionCreator,
    IProductionCharacterCreator,
    IProductionSceneCreator,
    IProductionPerformanceCommitter
{
    private const string CatalogDirectoryName = "production-catalog";
    private const string EntryDirectoryPrefix = "entry-";
    private const int LocatorHexLength = 32;
    private const string IdentityFileName = "identity.kid";

    private const string CreationPendingPrefix = ".production-create-";

    private readonly string _applicationRoot;
    private readonly string _catalogDirectory;

    public FileProductionCatalog(string rootDirectory)
        : this(rootDirectory, createDirectories: true)
    {
    }

    private FileProductionCatalog(
        string rootDirectory,
        bool createDirectories)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        _applicationRoot = Path.GetFullPath(rootDirectory);
        _catalogDirectory = Path.Combine(
            _applicationRoot,
            CatalogDirectoryName);

        if (createDirectories)
        {
            Directory.CreateDirectory(_applicationRoot);
            Directory.CreateDirectory(_catalogDirectory);
            return;
        }

        RequireExistingDirectory(_applicationRoot);
        RequireExistingDirectory(_catalogDirectory);
    }

    internal static FileProductionCatalog OpenExisting(string rootDirectory) =>
        new(rootDirectory, createDirectories: false);

    public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions()
    {
        var entries = ReadEntries();
        if (!entries.IsSuccess)
        {
            return ProductAccessResult<IReadOnlyList<ProductionSummary>>.Failure(
                entries.FailureKind);
        }

        var summaries = new List<ProductionSummary>(entries.Value.Count);
        foreach (var entry in entries.Value)
        {
            var projection = ReadProjection(
                entry.DirectoryPath,
                recover: false);
            if (!projection.IsSuccess)
            {
                return ProductAccessResult<IReadOnlyList<ProductionSummary>>.Failure(
                    projection.FailureKind);
            }

            summaries.Add(
                new ProductionSummary(
                    entry.Id,
                    projection.Value.ProductionName));
        }

        summaries.Sort(
            (left, right) => StringComparer.Ordinal.Compare(
                left.Id.Value,
                right.Id.Value));
        return ProductAccessResult<IReadOnlyList<ProductionSummary>>.Success(
            summaries.AsReadOnly());
    }

    public ProductAccessResult<ProductionReplayProjection> OpenProduction(
        ProductionId productionId) =>
        AccessProduction(productionId, recover: false);

    public ProductAccessResult<ProductionCreation> CreateProduction(
        string productionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productionName);

        var listed = ListProductions();
        if (!listed.IsSuccess)
        {
            return ProductAccessResult<ProductionCreation>.Failure(
                listed.FailureKind);
        }

        ProductionId productionId;
        do
        {
            productionId = new ProductionId(Guid.NewGuid().ToString("N"));
        }
        while (listed.Value.Any(item => item.Id == productionId));

        string finalPath;
        do
        {
            finalPath = Path.Combine(
                _catalogDirectory,
                EntryDirectoryPrefix + Guid.NewGuid().ToString("N"));
        }
        while (Directory.Exists(finalPath) || File.Exists(finalPath));

        string pendingPath;
        do
        {
            pendingPath = Path.Combine(
                _applicationRoot,
                CreationPendingPrefix + Guid.NewGuid().ToString("N"));
        }
        while (Directory.Exists(pendingPath) || File.Exists(pendingPath));

        var published = false;
        try
        {
            Directory.CreateDirectory(pendingPath);
            ProductionIdentityMetadata.Write(
                Path.Combine(pendingPath, IdentityFileName),
                productionId);

            var replay = new ProductionApplication(
                new FileProductionEventStore(pendingPath))
                .Create(productionName);

            if (!string.Equals(
                    replay.ProductionName,
                    productionName,
                    StringComparison.Ordinal) ||
                !replay.WorldCurrentState.IsEmpty ||
                !replay.ProductionCast.IsEmpty ||
                !replay.ProductionScenes.IsEmpty)
            {
                throw new InvalidDataException(
                    "New Production replay does not match its creation contract.");
            }

            Directory.Move(pendingPath, finalPath);
            published = true;

            return ProductAccessResult<ProductionCreation>.Success(
                new ProductionCreation(productionId, replay));
        }
        finally
        {
            if (!published)
            {
                TryDeleteDirectory(pendingPath);
            }
        }
    }

    public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
        ProductionId productionId) =>
        AccessProduction(productionId, recover: true);

    internal ProductAccessResult<ProductionCatalogEntry> ResolveProductionEntry(
        ProductionId productionId)
    {
        ArgumentNullException.ThrowIfNull(productionId);

        var entries = ReadEntries();
        if (!entries.IsSuccess)
        {
            return ProductAccessResult<ProductionCatalogEntry>.Failure(
                entries.FailureKind);
        }

        var match = entries.Value.FirstOrDefault(
            entry => entry.Id == productionId);
        return match is null
            ? ProductAccessResult<ProductionCatalogEntry>.Failure(
                ProductAccessFailureKind.Invalid)
            : ProductAccessResult<ProductionCatalogEntry>.Success(match);
    }

    public ProductAccessResult<CharacterCreation> CreateCharacter(
        ProductionId productionId,
        string characterName)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(characterName);

        var entry = ResolveProductionEntry(productionId);
        if (!entry.IsSuccess)
        {
            return ProductAccessResult<CharacterCreation>.Failure(
                entry.FailureKind);
        }

        var current = ReadProjection(
            entry.Value.DirectoryPath,
            recover: false);
        if (!current.IsSuccess)
        {
            return ProductAccessResult<CharacterCreation>.Failure(
                current.FailureKind);
        }

        CharacterId characterId;
        do
        {
            characterId = new CharacterId(Guid.NewGuid().ToString("N"));
        }
        while (current.Value.ProductionCast.Characters.Any(
            character => character.Id == characterId));

        var character = new CharacterSummary(
            characterId,
            characterName);
        var updated = ReadProjection(
            entry.Value.DirectoryPath,
            recover: false,
            append: new CharacterCreatedEvent(
                characterId,
                characterName));
        if (!updated.IsSuccess)
        {
            return ProductAccessResult<CharacterCreation>.Failure(
                updated.FailureKind);
        }

        return ProductAccessResult<CharacterCreation>.Success(
            new CharacterCreation(character, updated.Value));
    }

    public ProductAccessResult<SceneCreation> EstablishScene(
        ProductionId productionId,
        SceneRoster initialRoster)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ArgumentNullException.ThrowIfNull(initialRoster);

        var entry = ResolveProductionEntry(productionId);
        if (!entry.IsSuccess)
        {
            return ProductAccessResult<SceneCreation>.Failure(
                entry.FailureKind);
        }

        var current = ReadProjection(
            entry.Value.DirectoryPath,
            recover: false);
        if (!current.IsSuccess)
        {
            return ProductAccessResult<SceneCreation>.Failure(
                current.FailureKind);
        }

        SceneRoster canonicalRoster;
        try
        {
            canonicalRoster = SceneRoster.Canonicalize(
                current.Value.ProductionCast,
                initialRoster.CharacterIds);
        }
        catch (ArgumentException)
        {
            return ProductAccessResult<SceneCreation>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        SceneId sceneId;
        do
        {
            sceneId = new SceneId(Guid.NewGuid().ToString("N"));
        }
        while (current.Value.ProductionScenes.Scenes.Any(
            scene => scene.Id == sceneId));

        var scene = new EstablishedScene(sceneId, canonicalRoster);
        var updated = ReadProjection(
            entry.Value.DirectoryPath,
            recover: false,
            append: new CreatorEstablishedSceneEvent(
                sceneId,
                canonicalRoster));
        if (!updated.IsSuccess)
        {
            return ProductAccessResult<SceneCreation>.Failure(
                updated.FailureKind);
        }

        return ProductAccessResult<SceneCreation>.Success(
            new SceneCreation(scene, updated.Value));
    }

    public ProductAccessResult<ProductionReplayProjection> CommitAcceptedPerformance(
        ProductionId productionId,
        AcceptedPerformance acceptedPerformance)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ArgumentNullException.ThrowIfNull(acceptedPerformance);

        var entry = ResolveProductionEntry(productionId);
        if (!entry.IsSuccess)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                entry.FailureKind);
        }

        return ReadProjection(
            entry.Value.DirectoryPath,
            recover: false,
            append: new AcceptedPerformanceCommittedEvent(
                acceptedPerformance));
    }

    public ProductAccessResult<ProductionReplayProjection> ReplaceWorldCurrentState(
        ProductionId productionId,
        WorldCurrentState currentState)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ArgumentNullException.ThrowIfNull(currentState);
        var entry = ResolveProductionEntry(productionId);
        if (!entry.IsSuccess)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(entry.FailureKind);
        }

        return ReadProjection(
            entry.Value.DirectoryPath,
            recover: false,
            append: new CreatorReplacedWorldCurrentStateEvent(currentState));
    }

    private ProductAccessResult<ProductionReplayProjection> AccessProduction(
        ProductionId productionId,
        bool recover)
    {
        var entry = ResolveProductionEntry(productionId);
        if (!entry.IsSuccess)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                entry.FailureKind);
        }

        return ReadProjection(
            entry.Value.DirectoryPath,
            recover);
    }

    private ProductAccessResult<IReadOnlyList<ProductionCatalogEntry>> ReadEntries()
    {
        var entries = new List<ProductionCatalogEntry>();
        var identities = new HashSet<ProductionId>();

        var paths = Directory
            .EnumerateFileSystemEntries(
                _catalogDirectory,
                "*",
                SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName, StringComparer.Ordinal)
            .ToArray();

        foreach (var path in paths)
        {
            var attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.Directory) == 0 ||
                (attributes & FileAttributes.ReparsePoint) != 0 ||
                !IsCanonicalLocator(Path.GetFileName(path)))
            {
                return ProductAccessResult<IReadOnlyList<ProductionCatalogEntry>>.Failure(
                    ProductAccessFailureKind.Invalid);
            }

            var identity = ReadIdentity(path);
            if (!identity.IsSuccess)
            {
                return ProductAccessResult<IReadOnlyList<ProductionCatalogEntry>>.Failure(
                    identity.FailureKind);
            }

            if (!identities.Add(identity.Value))
            {
                return ProductAccessResult<IReadOnlyList<ProductionCatalogEntry>>.Failure(
                    ProductAccessFailureKind.Invalid);
            }

            entries.Add(
                new ProductionCatalogEntry(
                    identity.Value,
                    path));
        }

        return ProductAccessResult<IReadOnlyList<ProductionCatalogEntry>>.Success(
            entries.AsReadOnly());
    }

    private static ProductAccessResult<ProductionId> ReadIdentity(
        string entryDirectory)
    {
        try
        {
            return ProductAccessResult<ProductionId>.Success(
                ProductionIdentityMetadata.Read(
                    Path.Combine(entryDirectory, IdentityFileName)));
        }
        catch (ProductionPersistenceCompatibilityException)
        {
            return ProductAccessResult<ProductionId>.Failure(
                ProductAccessFailureKind.Incompatible);
        }
        catch (FileNotFoundException)
        {
            return ProductAccessResult<ProductionId>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (DirectoryNotFoundException)
        {
            return ProductAccessResult<ProductionId>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (InvalidDataException)
        {
            return ProductAccessResult<ProductionId>.Failure(
                ProductAccessFailureKind.Invalid);
        }
    }

    private static ProductAccessResult<ProductionReplayProjection> ReadProjection(
        string directoryPath,
        bool recover,
        ProductionEvent? append = null)
    {
        FileProductionEventStore store;
        try
        {
            store = FileProductionEventStore.OpenExisting(directoryPath);
        }
        catch (DirectoryNotFoundException)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        ProductionReplayProjection projection;
        ProductionJournalAnchor? anchor;
        try
        {
            var history = append is not null
                ? store.AppendValidatedHistory(append)
                : recover
                    ? store.RecoverValidatedHistory()
                    : store.LoadValidatedHistory();
            projection = history.Projection
                ?? ProductionReplay.Rebuild(history.Events);
            anchor = history.Anchor;
        }
        catch (DirectoryNotFoundException)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (ProductionPersistenceCompatibilityException)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Incompatible);
        }
        catch (ProductionJournalCorruptionException)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (InvalidDataException)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (InvalidOperationException)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        if (anchor is not null)
        {
            ProductionProjectionSnapshotCache.ReconcileBestEffort(
                directoryPath,
                anchor,
                projection,
                forceWrite: recover);
        }

        return ProductAccessResult<ProductionReplayProjection>.Success(
            projection);
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private static void RequireExistingDirectory(string path)
    {
        var attributes = File.GetAttributes(path);
        if ((attributes & FileAttributes.Directory) == 0)
        {
            throw new IOException(
                "Production catalog root is not a directory.");
        }
    }

    private static bool IsCanonicalLocator(string locator)
    {
        if (locator.Length != EntryDirectoryPrefix.Length + LocatorHexLength ||
            !locator.StartsWith(
                EntryDirectoryPrefix,
                StringComparison.Ordinal))
        {
            return false;
        }

        var token = locator.AsSpan(EntryDirectoryPrefix.Length);
        for (var index = 0; index < token.Length; index++)
        {
            var character = token[index];
            if (!((character >= '0' && character <= '9') ||
                  (character >= 'a' && character <= 'f')))
            {
                return false;
            }
        }

        return true;
    }
}

internal sealed record ProductionCatalogEntry(
    ProductionId Id,
    string DirectoryPath);
