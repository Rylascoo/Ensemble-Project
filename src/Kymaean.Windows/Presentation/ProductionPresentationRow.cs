using System.Buffers.Binary;
using System.Security.Cryptography;
using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public sealed class ProductionPresentationRow
{
    private ProductionPresentationRow(
        ProductionSummary summary,
        string? productionCode)
    {
        Summary = summary;
        ProductionCode = productionCode;
    }

    public ProductionSummary Summary { get; }

    public ProductionId Id => Summary.Id;

    public string ProductionName => Summary.ProductionName;

    public string? ProductionCode { get; }

    public bool HasProductionCode =>
        !string.IsNullOrEmpty(ProductionCode);

    public string ProductionCodeLabel =>
        HasProductionCode
            ? $"Production code {ProductionCode}"
            : string.Empty;

    public string AccessibleName =>
        HasProductionCode
            ? $"{ProductionName}, Production code {ProductionCode}"
            : ProductionName;

    public static IReadOnlyList<ProductionPresentationRow> Build(
        IReadOnlyList<ProductionSummary> productions)
    {
        ArgumentNullException.ThrowIfNull(productions);

        var duplicateCodes = new Dictionary<
            ProductionId,
            string>();

        foreach (var group in productions.GroupBy(
                     production => production.ProductionName,
                     StringComparer.Ordinal))
        {
            var duplicates = group.ToArray();
            if (duplicates.Length < 2)
            {
                continue;
            }

            var hashes = duplicates.ToDictionary(
                production => production.Id,
                production => HashIdentity(
                    production.Id.Value));
            var prefixLengths = duplicates.ToDictionary(
                production => production.Id,
                _ => 8);

            while (true)
            {
                var collisions = duplicates
                    .GroupBy(
                        production =>
                            hashes[production.Id][
                                ..prefixLengths[production.Id]],
                        StringComparer.Ordinal)
                    .Where(candidate => candidate.Count() > 1)
                    .ToArray();

                if (collisions.Length == 0)
                {
                    break;
                }

                foreach (var collision in collisions)
                {
                    foreach (var production in collision)
                    {
                        var nextLength =
                            prefixLengths[production.Id] + 4;
                        if (nextLength > hashes[production.Id].Length)
                        {
                            throw new InvalidOperationException(
                                "Production identity fingerprints collide.");
                        }

                        prefixLengths[production.Id] =
                            nextLength;
                    }
                }
            }

            foreach (var production in duplicates)
            {
                duplicateCodes[production.Id] =
                    hashes[production.Id][
                        ..prefixLengths[production.Id]];
            }
        }

        return productions
            .Select(
                production =>
                    new ProductionPresentationRow(
                        production,
                        duplicateCodes.GetValueOrDefault(
                            production.Id)))
            .ToArray();
    }

    private static string HashIdentity(string value)
    {
        var bytes = new byte[
            checked(value.Length * sizeof(ushort))];

        for (var index = 0;
             index < value.Length;
             index++)
        {
            BinaryPrimitives.WriteUInt16BigEndian(
                bytes.AsSpan(
                    index * sizeof(ushort),
                    sizeof(ushort)),
                value[index]);
        }

        return Convert.ToHexString(
            SHA256.HashData(bytes));
    }
}
