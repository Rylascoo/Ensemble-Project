using System.Text.Json;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Persistence;

public sealed class FileProductionEventStore : IProductionEventStore
{
    private readonly FileProductionJournal _journal;

    public FileProductionEventStore(string rootDirectory)
    {
        _journal = new FileProductionJournal(rootDirectory);
    }

    public void Append(ProductionEvent productionEvent)
    {
        ArgumentNullException.ThrowIfNull(productionEvent);
        _journal.Append(ProductionEventCodec.Encode(productionEvent));
    }

    public IReadOnlyList<ProductionEvent> LoadAll() =>
        DecodeEntries(_journal.ReadAll());

    private static IReadOnlyList<ProductionEvent> DecodeEntries(
        IReadOnlyList<ProductionJournalEntry> entries)
    {
        var events = new ProductionEvent[entries.Count];
        for (var index = 0; index < entries.Count; index++)
        {
            events[index] = ProductionEventCodec.Decode(entries[index].Payload);
        }

        return events;
    }
}

internal static class ProductionEventCodec
{
    private const string ContractProperty = "contract";
    private const string ProductionNameProperty = "productionName";
    private const string ProductionCreatedV1 = "kymaean.production.created.v1";

    public static byte[] Encode(ProductionEvent productionEvent)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        writer.WriteStartObject();

        switch (productionEvent)
        {
            case ProductionCreatedEvent created:
                writer.WriteString(ContractProperty, ProductionCreatedV1);
                writer.WriteString(ProductionNameProperty, created.ProductionName);
                break;
            default:
                throw new NotSupportedException(
                    $"Production event type '{productionEvent.GetType().Name}' cannot be persisted.");
        }

        writer.WriteEndObject();
        writer.Flush();
        return stream.ToArray();
    }

    public static ProductionEvent Decode(ReadOnlyMemory<byte> payload)
    {
        try
        {
            using var document = JsonDocument.Parse(payload);
            var root = document.RootElement;
            RequireObject(root);

            var contract = ReadRequiredString(root, ContractProperty);
            return contract switch
            {
                ProductionCreatedV1 => DecodeProductionCreated(root),
                _ => throw new InvalidDataException(
                    $"Unsupported Production event contract '{contract}'.")
            };
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(
                "Production event payload is not valid JSON.", exception);
        }
        catch (ArgumentException exception)
        {
            throw new InvalidDataException(
                "Production event payload violates its Application contract.", exception);
        }
    }

    private static ProductionEvent DecodeProductionCreated(JsonElement root)
    {
        RequireExactProperties(root, ContractProperty, ProductionNameProperty);
        return new ProductionCreatedEvent(
            ReadRequiredString(root, ProductionNameProperty));
    }

    private static void RequireObject(JsonElement root)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException(
                "Production event payload must be a JSON object.");
        }
    }

    private static string ReadRequiredString(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String ||
            property.GetString() is not { } value)
        {
            throw new InvalidDataException(
                $"Production event property '{propertyName}' must be a string.");
        }

        return value;
    }

    private static void RequireExactProperties(
        JsonElement root,
        params string[] expectedProperties)
    {
        var expected = new HashSet<string>(
            expectedProperties,
            StringComparer.Ordinal);
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var property in root.EnumerateObject())
        {
            if (!expected.Contains(property.Name) || !seen.Add(property.Name))
            {
                throw new InvalidDataException(
                    $"Production event contains unexpected or duplicate property '{property.Name}'.");
            }
        }

        if (seen.Count != expected.Count)
        {
            throw new InvalidDataException(
                "Production event is missing one or more required properties.");
        }
    }
}
