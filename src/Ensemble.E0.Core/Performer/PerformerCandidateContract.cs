using System.Collections.Immutable;
using System.Text.Json;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Performer;

public static class PerformerCandidateContract
{
    public const string CandidateContractVersion = "ensemble.e0.performer.candidate.v1";
    public const string CandidateJsonSchemaVersion = "ensemble.e0.performer.candidate-json.v1";
    public const int MaxCandidateJsonBytes = 1024 * 1024;
    public const int MaxCandidateJsonDepth = 8;

    public static CandidatePerformance ParseJson(
        ContextPacket contextPacket,
        ReadOnlySpan<byte> utf8CandidateOutput)
    {
        var context = ValidateContext(contextPacket);
        ValidateTransportEnvelope(
            utf8CandidateOutput,
            context.MaxAddressedCharacterCount);
        var parsed = ParseTransport(
            utf8CandidateOutput,
            context.MaxAddressedCharacterCount);
        return BuildCandidate(context, parsed);
    }

    private static ValidatedCandidateContext ValidateContext(ContextPacket? contextPacket)
    {
        if (contextPacket is null)
        {
            throw new PerformerCandidateException("ContextPacket is required.");
        }

        var subjectValue = RequireInitialized(
            contextPacket.SubjectCharacterId,
            "ContextPacket.SubjectCharacterId");
        var opportunityValue = RequireInitialized(
            contextPacket.OpportunityCharacterId,
            "ContextPacket.OpportunityCharacterId");
        _ = RequireInitialized(
            contextPacket.ContextPacketId,
            "ContextPacket.ContextPacketId");

        if (!string.Equals(subjectValue, opportunityValue, StringComparison.Ordinal))
        {
            throw new PerformerCandidateException(
                "ContextPacket opportunity must equal the subject Character for the E0 candidate path.");
        }

        if (contextPacket.Roster.IsDefault)
        {
            throw new PerformerCandidateException("ContextPacket roster is uninitialized.");
        }

        var rosterIds = new HashSet<string>(StringComparer.Ordinal);
        var subjectCount = 0;

        foreach (var participant in contextPacket.Roster)
        {
            if (participant is null)
            {
                throw new PerformerCandidateException(
                    "ContextPacket roster contains an invalid participant.");
            }

            var participantId = RequireInitialized(
                participant.CharacterId,
                "ContextPacket.Roster.CharacterId");

            if (!rosterIds.Add(participantId))
            {
                throw new PerformerCandidateException(
                    "ContextPacket roster contains duplicate Character IDs.");
            }

            if (string.Equals(participantId, subjectValue, StringComparison.Ordinal))
            {
                subjectCount++;
            }
        }

        if (subjectCount != 1)
        {
            throw new PerformerCandidateException(
                "ContextPacket subject must resolve exactly once in the roster.");
        }

        return new ValidatedCandidateContext(
            contextPacket,
            rosterIds,
            Math.Max(0, contextPacket.Roster.Length - 1));
    }

    private static void ValidateTransportEnvelope(
        ReadOnlySpan<byte> utf8CandidateOutput,
        int maxAddressedCharacterCount)
    {
        if (utf8CandidateOutput.IsEmpty)
        {
            throw new PerformerCandidateException("Candidate JSON is empty.");
        }

        if (utf8CandidateOutput.Length > MaxCandidateJsonBytes)
        {
            throw new PerformerCandidateException(
                "Candidate JSON exceeds the parser safety ceiling.");
        }

        if (utf8CandidateOutput.Length >= 3 &&
            utf8CandidateOutput[0] == 0xEF &&
            utf8CandidateOutput[1] == 0xBB &&
            utf8CandidateOutput[2] == 0xBF)
        {
            throw new PerformerCandidateException(
                "Candidate JSON must not contain a UTF-8 BOM.");
        }

        var readerOptions = new JsonReaderOptions
        {
            AllowTrailingCommas = false,
            CommentHandling = JsonCommentHandling.Disallow,
            MaxDepth = MaxCandidateJsonDepth
        };

        var reader = new Utf8JsonReader(utf8CandidateOutput, readerOptions);
        var objectProperties = new Stack<HashSet<string>>();
        var sawRoot = false;
        var rootClosed = false;
        string? pendingPropertyName = null;
        var addressedArrayDepth = -1;
        var addressedEntryCount = 0;

        try
        {
            while (reader.Read())
            {
                if (!sawRoot)
                {
                    if (reader.TokenType != JsonTokenType.StartObject || reader.CurrentDepth != 0)
                    {
                        throw new PerformerCandidateException(
                            "Candidate JSON root must be an object.");
                    }

                    sawRoot = true;
                }
                else if (rootClosed)
                {
                    throw new PerformerCandidateException(
                        "Candidate JSON contains content after the root object.");
                }

                if (addressedArrayDepth >= 0)
                {
                    if (reader.TokenType == JsonTokenType.EndArray &&
                        reader.CurrentDepth == addressedArrayDepth)
                    {
                        addressedArrayDepth = -1;
                    }
                    else if (reader.CurrentDepth == addressedArrayDepth + 1)
                    {
                        addressedEntryCount++;
                        if (addressedEntryCount > maxAddressedCharacterCount)
                        {
                            throw new PerformerCandidateException(
                                "Candidate control addressedCharacterIds exceeds the roster-derived limit.");
                        }
                    }
                }

                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        objectProperties.Push(new HashSet<string>(StringComparer.Ordinal));
                        pendingPropertyName = null;
                        break;

                    case JsonTokenType.PropertyName:
                        if (objectProperties.Count == 0)
                        {
                            throw new PerformerCandidateException(
                                "Candidate JSON contains a property outside an object.");
                        }

                        var propertyName = reader.GetString();
                        if (propertyName is null)
                        {
                            throw new PerformerCandidateException(
                                "Candidate JSON contains an invalid property name.");
                        }

                        if (!objectProperties.Peek().Add(propertyName))
                        {
                            throw new PerformerCandidateException(
                                "Candidate JSON contains a duplicate property.");
                        }

                        pendingPropertyName = propertyName;
                        break;

                    case JsonTokenType.String:
                        _ = reader.GetString()
                            ?? throw new PerformerCandidateException(
                                "Candidate JSON contains an invalid string value.");
                        pendingPropertyName = null;
                        break;

                    case JsonTokenType.StartArray:
                        if (string.Equals(
                                pendingPropertyName,
                                "addressedCharacterIds",
                                StringComparison.Ordinal))
                        {
                            addressedArrayDepth = reader.CurrentDepth;
                            addressedEntryCount = 0;
                        }

                        pendingPropertyName = null;
                        break;

                    case JsonTokenType.EndObject:
                        if (objectProperties.Count == 0)
                        {
                            throw new PerformerCandidateException(
                                "Candidate JSON contains an unexpected object terminator.");
                        }

                        objectProperties.Pop();
                        pendingPropertyName = null;
                        if (reader.CurrentDepth == 0)
                        {
                            rootClosed = true;
                        }

                        break;

                    default:
                        pendingPropertyName = null;
                        break;
                }
            }
        }
        catch (JsonException)
        {
            throw new PerformerCandidateException(
                "Candidate JSON is syntactically invalid.");
        }
        catch (InvalidOperationException)
        {
            throw new PerformerCandidateException(
                "Candidate JSON contains invalid Unicode data.");
        }

        if (!sawRoot || !rootClosed || objectProperties.Count != 0)
        {
            throw new PerformerCandidateException(
                "Candidate JSON ended before the root object was complete.");
        }
    }

    private static ParsedCandidateTransport ParseTransport(
        ReadOnlySpan<byte> utf8CandidateOutput,
        int maxAddressedCharacterCount)
    {
        try
        {
            using var document = JsonDocument.Parse(
                utf8CandidateOutput.ToArray(),
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = false,
                    CommentHandling = JsonCommentHandling.Disallow,
                    MaxDepth = MaxCandidateJsonDepth
                });

            var root = document.RootElement;
            RequireObject(root, "candidate root");
            EnsureOnlyProperties(
                root,
                "candidate root",
                "schemaVersion",
                "performance",
                "control");

            var schemaVersion = RequireString(
                root,
                "schemaVersion",
                "candidate root");
            if (!string.Equals(
                    schemaVersion,
                    CandidateJsonSchemaVersion,
                    StringComparison.Ordinal))
            {
                throw new PerformerCandidateException(
                    "Candidate JSON schemaVersion is unsupported.");
            }

            var performance = RequireObjectProperty(
                root,
                "performance",
                "candidate root");
            EnsureOnlyProperties(performance, "performance", "text");
            var visibleText = RequireString(performance, "text", "performance");

            var control = RequireObjectProperty(
                root,
                "control",
                "candidate root");
            EnsureOnlyProperties(
                control,
                "control",
                "addressedCharacterIds",
                "nominatedCharacterId");

            var addressedElement = RequireProperty(
                control,
                "addressedCharacterIds",
                "control");
            if (addressedElement.ValueKind != JsonValueKind.Array)
            {
                throw new PerformerCandidateException(
                    "Candidate control addressedCharacterIds must be an array.");
            }

            var addressed = new List<string>();
            foreach (var item in addressedElement.EnumerateArray())
            {
                if (addressed.Count >= maxAddressedCharacterCount)
                {
                    throw new PerformerCandidateException(
                        "Candidate control addressedCharacterIds exceeds the roster-derived limit.");
                }

                if (item.ValueKind != JsonValueKind.String)
                {
                    throw new PerformerCandidateException(
                        "Candidate control addressedCharacterIds must contain only strings.");
                }

                addressed.Add(
                    item.GetString()
                    ?? throw new PerformerCandidateException(
                        "Candidate control addressedCharacterIds contains an invalid string."));
            }

            var nominationElement = RequireProperty(
                control,
                "nominatedCharacterId",
                "control");
            string? nominatedCharacterId;
            if (nominationElement.ValueKind == JsonValueKind.Null)
            {
                nominatedCharacterId = null;
            }
            else if (nominationElement.ValueKind == JsonValueKind.String)
            {
                nominatedCharacterId = nominationElement.GetString()
                    ?? throw new PerformerCandidateException(
                        "Candidate control nominatedCharacterId contains an invalid string.");
            }
            else
            {
                throw new PerformerCandidateException(
                    "Candidate control nominatedCharacterId must be a string or null.");
            }

            return new ParsedCandidateTransport(
                visibleText,
                addressed,
                nominatedCharacterId);
        }
        catch (JsonException)
        {
            throw new PerformerCandidateException(
                "Candidate JSON is syntactically invalid.");
        }
    }

    private static CandidatePerformance BuildCandidate(
        ValidatedCandidateContext context,
        ParsedCandidateTransport parsed)
    {
        ValidateVisibleText(parsed.VisibleText);

        var subjectValue = context.Packet.SubjectCharacterId.Value;
        var addressed = ImmutableArray.CreateBuilder<CharacterId>(
            parsed.AddressedCharacterIds.Count);
        var addressedValues = new HashSet<string>(StringComparer.Ordinal);

        foreach (var rawId in parsed.AddressedCharacterIds)
        {
            var characterId = ParseCharacterId(rawId, "addressedCharacterIds");
            var value = characterId.Value;

            if (!context.RosterIds.Contains(value))
            {
                throw new PerformerCandidateException(
                    "Candidate control addressedCharacterIds contains a Character outside the roster.");
            }

            if (string.Equals(value, subjectValue, StringComparison.Ordinal))
            {
                throw new PerformerCandidateException(
                    "Candidate subject may not address itself in typed control.");
            }

            if (!addressedValues.Add(value))
            {
                throw new PerformerCandidateException(
                    "Candidate control addressedCharacterIds contains a duplicate Character.");
            }

            addressed.Add(characterId);
        }

        if (addressed.Count > context.MaxAddressedCharacterCount)
        {
            throw new PerformerCandidateException(
                "Candidate control addressedCharacterIds exceeds the roster-derived limit.");
        }

        var orderedAddressed = addressed
            .ToImmutable()
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();

        CharacterId? nominatedCharacterId = null;
        if (parsed.NominatedCharacterId is not null)
        {
            var nomination = ParseCharacterId(
                parsed.NominatedCharacterId,
                "nominatedCharacterId");
            var value = nomination.Value;

            if (!context.RosterIds.Contains(value))
            {
                throw new PerformerCandidateException(
                    "Candidate control nominatedCharacterId is outside the roster.");
            }

            if (string.Equals(value, subjectValue, StringComparison.Ordinal))
            {
                throw new PerformerCandidateException(
                    "Candidate subject may not nominate itself in typed control.");
            }

            nominatedCharacterId = nomination;
        }

        if (parsed.VisibleText.Length == 0 &&
            (orderedAddressed.Length != 0 || nominatedCharacterId is not null))
        {
            throw new PerformerCandidateException(
                "Silent CandidatePerformance cannot contain address or nomination control.");
        }

        return new CandidatePerformance(
            CandidateContractVersion,
            context.Packet.SubjectCharacterId,
            context.Packet.ContextPacketId,
            parsed.VisibleText,
            new CandidatePerformanceControl(
                orderedAddressed,
                nominatedCharacterId));
    }

    private static void ValidateVisibleText(string? visibleText)
    {
        switch (CharacterLegibleTextInvariants.Validate(visibleText))
        {
            case CharacterLegibleTextFailure.None:
                return;
            case CharacterLegibleTextFailure.Required:
                throw new PerformerCandidateException(
                    "Candidate performance text is required.");
            case CharacterLegibleTextFailure.InvalidUnicode:
                throw new PerformerCandidateException(
                    "Candidate performance text contains invalid Unicode.");
            case CharacterLegibleTextFailure.NotNormalized:
                throw new PerformerCandidateException(
                    "Candidate performance text must already be Unicode NFC.");
            case CharacterLegibleTextFailure.ForbiddenControl:
                throw new PerformerCandidateException(
                    "Candidate performance text contains a forbidden control character.");
            case CharacterLegibleTextFailure.NoDisplayBearingScalar:
                throw new PerformerCandidateException(
                    "Non-silent CandidatePerformance must contain visible Character-legible content.");
            default:
                throw new PerformerCandidateException(
                    "Candidate performance text is invalid.");
        }
    }

    private static CharacterId ParseCharacterId(string rawId, string fieldName)
    {
        try
        {
            return CharacterId.From(rawId);
        }
        catch (Exception exception) when (
            exception is ArgumentException or InvalidOperationException)
        {
            throw new PerformerCandidateException(
                $"Candidate control field '{fieldName}' contains an invalid Character ID.");
        }
    }

    private static string RequireInitialized(CharacterId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException)
        {
            throw new PerformerCandidateException(
                $"Trusted {fieldName} is uninitialized.");
        }
    }

    private static string RequireInitialized(ContextPacketId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException)
        {
            throw new PerformerCandidateException(
                $"Trusted {fieldName} is uninitialized.");
        }
    }

    private static void RequireObject(JsonElement element, string objectName)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new PerformerCandidateException(
                $"Candidate JSON {objectName} must be an object.");
        }
    }

    private static JsonElement RequireObjectProperty(
        JsonElement parent,
        string propertyName,
        string parentName)
    {
        var element = RequireProperty(parent, propertyName, parentName);
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new PerformerCandidateException(
                $"Candidate JSON field '{propertyName}' must be an object.");
        }

        return element;
    }

    private static string RequireString(
        JsonElement parent,
        string propertyName,
        string parentName)
    {
        var element = RequireProperty(parent, propertyName, parentName);
        if (element.ValueKind != JsonValueKind.String)
        {
            throw new PerformerCandidateException(
                $"Candidate JSON field '{propertyName}' must be a string.");
        }

        return element.GetString()
            ?? throw new PerformerCandidateException(
                $"Candidate JSON field '{propertyName}' contains an invalid string.");
    }

    private static JsonElement RequireProperty(
        JsonElement parent,
        string propertyName,
        string parentName)
    {
        if (!parent.TryGetProperty(propertyName, out var element))
        {
            throw new PerformerCandidateException(
                $"Candidate JSON {parentName} is missing required field '{propertyName}'.");
        }

        return element;
    }

    private static void EnsureOnlyProperties(
        JsonElement element,
        string objectName,
        params string[] allowedProperties)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (!allowedProperties.Contains(property.Name, StringComparer.Ordinal))
            {
                throw new PerformerCandidateException(
                    $"Candidate JSON {objectName} contains an unknown property.");
            }
        }
    }

    private sealed class ValidatedCandidateContext
    {
        public ValidatedCandidateContext(
            ContextPacket packet,
            HashSet<string> rosterIds,
            int maxAddressedCharacterCount)
        {
            Packet = packet;
            RosterIds = rosterIds;
            MaxAddressedCharacterCount = maxAddressedCharacterCount;
        }

        public ContextPacket Packet { get; }
        public HashSet<string> RosterIds { get; }
        public int MaxAddressedCharacterCount { get; }
    }

    private sealed class ParsedCandidateTransport
    {
        public ParsedCandidateTransport(
            string visibleText,
            List<string> addressedCharacterIds,
            string? nominatedCharacterId)
        {
            VisibleText = visibleText;
            AddressedCharacterIds = addressedCharacterIds;
            NominatedCharacterId = nominatedCharacterId;
        }

        public string VisibleText { get; }
        public List<string> AddressedCharacterIds { get; }
        public string? NominatedCharacterId { get; }
    }
}
