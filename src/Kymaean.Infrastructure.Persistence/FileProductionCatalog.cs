using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

public sealed class FileProductionCatalog : IProductionCatalog
{
    private const string CatalogDirectoryName = "production-catalog";
    private const string EntryDirectoryPrefix = "entry-";
    private const int LocatorHexLength = 32;
    private const string IdentityFileName = "identity.kid";

    private readonly string _catalogDirectory;

    public FileProductionCatalog(string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        var applicationRoot = Path.GetFullPath(rootDirectory);
        Directory.CreateDirectory(applicationRoot);

        _catalogDirectory = Path.Combine(
            applicationRoot,
            CatalogDirectoryName);
        Directory.CreateDirectory(_catalogDirectory);
    }

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

    public ProductAccessResult<ProductionReplayProjection> RecoverProduction(
        ProductionId productionId) =>
        AccessProduction(productionId, recover: true);

    private ProductAccessResult<ProductionReplayProjection> AccessProduction(
        ProductionId productionId,
        bool recover)
    {
        ArgumentNullException.ThrowIfNull(productionId);

        var entries = ReadEntries();
        if (!entries.IsSuccess)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                entries.FailureKind);
        }

        var match = entries.Value.FirstOrDefault(
            entry => entry.Id == productionId);
        if (match is null)
        {
            return ProductAccessResult<ProductionReplayProjection>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return ReadProjection(match.DirectoryPath, recover);
    }

    private ProductAccessResult<IReadOnlyList<CatalogEntry>> ReadEntries()
    {
        var entries = new List<CatalogEntry>();
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
                return ProductAccessResult<IReadOnlyList<CatalogEntry>>.Failure(
                    ProductAccessFailureKind.Invalid);
            }

            var identity = ReadIdentity(path);
            if (!identity.IsSuccess)
            {
                return ProductAccessResult<IReadOnlyList<CatalogEntry>>.Failure(
                    identity.FailureKind);
            }

            if (!identities.Add(identity.Value))
            {
                return ProductAccessResult<IReadOnlyList<CatalogEntry>>.Failure(
                    ProductAccessFailureKind.Invalid);
            }

            entries.Add(new CatalogEntry(identity.Value, path));
        }

        return ProductAccessResult<IReadOnlyList<CatalogEntry>>.Success(
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
        bool recover)
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

        try
        {
            var history = recover
                ? store.RecoverValidatedHistory()
                : store.LoadValidatedHistory();
            var projection = history.Projection
                ?? ProductionReplay.Rebuild(history.Events);

            if (history.Anchor is { } anchor)
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

    private sealed record CatalogEntry(
        ProductionId Id,
        string DirectoryPath);
}
