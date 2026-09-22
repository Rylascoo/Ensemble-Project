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

    private FileProductionEventStore(FileProductionJournal journal)
    {
        _journal = journal;
    }

    internal static FileProductionEventStore OpenExisting(string rootDirectory) =>
        new(FileProductionJournal.OpenExisting(rootDirectory));

    public void Append(ProductionEvent productionEvent)
    {
        ArgumentNullException.ThrowIfNull(productionEvent);
        var payload = ProductionEventCodec.Encode(productionEvent);
        _journal.AppendValidated(payload, ValidateEntries);
    }

    public IReadOnlyList<ProductionEvent> LoadAll() =>
        LoadValidatedHistory().Events;

    internal ValidatedProductionHistory LoadValidatedHistory() =>
        DecodeAndValidate(_journal.ReadAll());

    public IReadOnlyList<ProductionEvent> Recover() =>
        RecoverValidatedHistory().Events;

    internal ValidatedProductionHistory RecoverValidatedHistory()
    {
        ValidatedProductionHistory? recovered = null;
        _journal.RecoverValidated(
            entries => recovered = DecodeAndValidate(entries));

        return recovered
            ?? throw new InvalidOperationException(
                "Production recovery did not validate a history.");
    }

    private static void ValidateEntries(
        IReadOnlyList<ProductionJournalEntry> entries)
    {
        _ = DecodeAndValidate(entries);
    }

    private static ValidatedProductionHistory DecodeAndValidate(
        IReadOnlyList<ProductionJournalEntry> entries)
    {
        var events = new ProductionEvent[entries.Count];
        for (var index = 0; index < entries.Count; index++)
        {
            events[index] = ProductionEventCodec.Decode(entries[index].Payload);
        }

        ProductionReplayProjection? projection = null;
        ProductionJournalAnchor? anchor = null;
        if (events.Length > 0)
        {
            projection = ProductionReplay.Rebuild(events);
            var last = entries[^1];
            anchor = new ProductionJournalAnchor(
                last.Sequence,
                last.RecordHash);
        }

        return new ValidatedProductionHistory(
            events,
            entries,
            projection,
            anchor);
    }
}

internal sealed record ValidatedProductionHistory(
    IReadOnlyList<ProductionEvent> Events,
    IReadOnlyList<ProductionJournalEntry> JournalEntries,
    ProductionReplayProjection? Projection,
    ProductionJournalAnchor? Anchor);

internal sealed record ProductionJournalAnchor(
    ulong Sequence,
    string RecordHash);

internal static class ProductionEventCodec
{
    private const string ContractProperty = "contract";
    private const string ProductionNameProperty = "productionName";
    private const string ProductionNameCodeUnitsProperty = "productionNameUtf16Be";
    private const string TruthsProperty = "truthsUtf16Be";
    private const string CharacterIdCodeUnitsProperty = "characterIdUtf16Be";
    private const string CharacterNameCodeUnitsProperty = "characterNameUtf16Be";
    private const string ProductionCreatedContractFamily = "kymaean.production.created.v";
    private const string ReplacementContractFamily =
        "kymaean.production.creator-replaced-world-current-state.v";
    private const string CharacterCreatedContractFamily =
        "kymaean.production.character-created.v";

    public static byte[] Encode(ProductionEvent productionEvent)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        writer.WriteStartObject();

        switch (productionEvent)
        {
            case ProductionCreatedEvent created:
                writer.WriteString(
                    ContractProperty,
                    ProductionPersistenceVersionPolicy.ProductionCreatedContractV2);
                writer.WriteBase64String(
                    ProductionNameCodeUnitsProperty, Utf16CodeUnits.Encode(created.ProductionName));
                break;
            case CreatorReplacedWorldCurrentStateEvent replaced:
                writer.WriteString(
                    ContractProperty,
                    ProductionPersistenceVersionPolicy.CreatorReplacedWorldCurrentStateContractV1);
                writer.WriteStartArray(TruthsProperty);
                foreach (var truth in replaced.CurrentState.Truths)
                {
                    writer.WriteBase64StringValue(Utf16CodeUnits.Encode(truth.Text));
                }

                writer.WriteEndArray();
                break;
            case CharacterCreatedEvent created:
                writer.WriteString(
                    ContractProperty,
                    ProductionPersistenceVersionPolicy.CharacterCreatedContractV1);
                writer.WriteBase64String(
                    CharacterIdCodeUnitsProperty,
                    Utf16CodeUnits.Encode(created.CharacterId.Value));
                writer.WriteBase64String(
                    CharacterNameCodeUnitsProperty,
                    Utf16CodeUnits.Encode(created.CharacterName));
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
                ProductionPersistenceVersionPolicy.ProductionCreatedContractV1 =>
                    DecodeProductionCreated(root),
                ProductionPersistenceVersionPolicy.ProductionCreatedContractV2 =>
                    DecodeProductionCreatedV2(root),
                ProductionPersistenceVersionPolicy.CreatorReplacedWorldCurrentStateContractV1 =>
                    DecodeReplacement(root),
                ProductionPersistenceVersionPolicy.CharacterCreatedContractV1 =>
                    DecodeCharacterCreated(root),
                _ when contract.StartsWith(
                    ProductionCreatedContractFamily,
                    StringComparison.Ordinal) =>
                    throw ProductionPersistenceVersionPolicy.UnsupportedEventContract(
                        "ProductionCreated event contract",
                        contract,
                        ProductionPersistenceVersionPolicy.ProductionCreatedContractV1 + ", " +
                        ProductionPersistenceVersionPolicy.ProductionCreatedContractV2),
                _ when contract.StartsWith(ReplacementContractFamily, StringComparison.Ordinal) =>
                    throw ProductionPersistenceVersionPolicy.UnsupportedEventContract(
                        "CreatorReplacedWorldCurrentState event contract",
                        contract,
                        ProductionPersistenceVersionPolicy.CreatorReplacedWorldCurrentStateContractV1),
                _ when contract.StartsWith(CharacterCreatedContractFamily, StringComparison.Ordinal) =>
                    throw ProductionPersistenceVersionPolicy.UnsupportedEventContract(
                        "CharacterCreated event contract",
                        contract,
                        ProductionPersistenceVersionPolicy.CharacterCreatedContractV1),
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
        catch (InvalidOperationException exception)
        {
            throw new InvalidDataException(
                "Production event payload contains invalid JSON string data.", exception);
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

    private static ProductionCreatedEvent DecodeProductionCreatedV2(JsonElement root)
    {
        RequireExactProperties(root, ContractProperty, ProductionNameCodeUnitsProperty);
        return new ProductionCreatedEvent(ReadCodeUnits(root.GetProperty(ProductionNameCodeUnitsProperty)));
    }

    private static CreatorReplacedWorldCurrentStateEvent DecodeReplacement(JsonElement root)
    {
        RequireExactProperties(root, ContractProperty, TruthsProperty);
        var truths = root.GetProperty(TruthsProperty);
        if (truths.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException("World current truths must be an array.");
        }

        return new CreatorReplacedWorldCurrentStateEvent(new WorldCurrentState(
            truths.EnumerateArray().Select(value => new WorldCurrentTruth(ReadCodeUnits(value)))));
    }

    private static CharacterCreatedEvent DecodeCharacterCreated(JsonElement root)
    {
        RequireExactProperties(
            root,
            ContractProperty,
            CharacterIdCodeUnitsProperty,
            CharacterNameCodeUnitsProperty);
        return new CharacterCreatedEvent(
            new CharacterId(
                ReadCodeUnits(root.GetProperty(CharacterIdCodeUnitsProperty))),
            ReadCodeUnits(root.GetProperty(CharacterNameCodeUnitsProperty)));
    }

    private static string ReadCodeUnits(JsonElement value)
    {
        if (value.ValueKind != JsonValueKind.String || !value.TryGetBytesFromBase64(out var bytes))
        {
            throw new InvalidDataException("UTF-16 code-unit data must be Base64.");
        }

        return Utf16CodeUnits.Decode(bytes);
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
