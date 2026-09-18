using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

public sealed class FileProductionExporter
{
    private readonly string _applicationRoot;
    private readonly FileProductionCatalog _catalog;

    public FileProductionExporter(string applicationRoot)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationRoot);
        _applicationRoot = Path.GetFullPath(applicationRoot);
        _catalog = FileProductionCatalog.OpenExisting(
            _applicationRoot);
    }

    public ProductAccessResult<ProductionPortableExport> ExportProduction(
        ProductionId productionId)
    {
        ArgumentNullException.ThrowIfNull(productionId);

        var entry = _catalog.ResolveProductionEntry(productionId);
        if (!entry.IsSuccess)
        {
            return ProductAccessResult<ProductionPortableExport>.Failure(
                entry.FailureKind);
        }

        ValidatedProductionHistory history;
        try
        {
            var store = FileProductionEventStore.OpenExisting(
                entry.Value.DirectoryPath);
            history = store.LoadValidatedHistory();
            _ = history.Projection
                ?? ProductionReplay.Rebuild(history.Events);
        }
        catch (DirectoryNotFoundException)
        {
            return ProductAccessResult<ProductionPortableExport>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (ProductionPersistenceCompatibilityException)
        {
            return ProductAccessResult<ProductionPortableExport>.Failure(
                ProductAccessFailureKind.Incompatible);
        }
        catch (ProductionJournalCorruptionException)
        {
            return ProductAccessResult<ProductionPortableExport>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (InvalidDataException)
        {
            return ProductAccessResult<ProductionPortableExport>.Failure(
                ProductAccessFailureKind.Invalid);
        }
        catch (InvalidOperationException)
        {
            return ProductAccessResult<ProductionPortableExport>.Failure(
                ProductAccessFailureKind.Invalid);
        }

        return ProductAccessResult<ProductionPortableExport>.Success(
            ProductionPortableExportCodec.Encode(
                entry.Value.Id,
                history.JournalEntries));
    }

    public ProductAccessResult<ProductionId> ExportProductionToFile(
        ProductionId productionId,
        string destinationPath)
    {
        ArgumentNullException.ThrowIfNull(productionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        var finalPath = Path.GetFullPath(destinationPath);
        if (IsWithinSourceRoot(finalPath))
        {
            throw new ArgumentException(
                "Portable Production export destination must be outside the live application root.",
                nameof(destinationPath));
        }

        var package = ExportProduction(productionId);
        if (!package.IsSuccess)
        {
            return ProductAccessResult<ProductionId>.Failure(
                package.FailureKind);
        }

        ProductionPortableExportCodec.WriteFinalized(
            package.Value,
            finalPath);
        return ProductAccessResult<ProductionId>.Success(
            productionId);
    }

    private bool IsWithinSourceRoot(string candidatePath)
    {
        var relative = Path.GetRelativePath(
            _applicationRoot,
            candidatePath);
        if (Path.IsPathRooted(relative) ||
            StringComparer.Ordinal.Equals(
                relative,
                ".."))
        {
            return false;
        }

        if (relative.StartsWith(
                ".." + Path.DirectorySeparatorChar,
                StringComparison.Ordinal))
        {
            return false;
        }

        if (Path.AltDirectorySeparatorChar != Path.DirectorySeparatorChar &&
            relative.StartsWith(
                ".." + Path.AltDirectorySeparatorChar,
                StringComparison.Ordinal))
        {
            return false;
        }

        return true;
    }
}
