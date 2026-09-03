using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.StateInterpreter;

public static class StateInterpretationContract
{
    public const string ContractVersion =
        "ensemble.e0.state-interpreter.proposal.v1";
    public const string JsonSchemaVersion =
        "ensemble.e0.state-interpreter.proposal-json.v1";
    public const int MaxProposalJsonBytes = 1024 * 1024;
    public const int MaxProposalJsonDepth = 8;

    public static StateInterpretationProposal ParseJson(
        StateInterpretationSource source,
        ReadOnlySpan<byte> utf8Proposal)
    {
        StateInterpretationInvariants.ValidateSource(source);
        ValidateTransportEnvelope(utf8Proposal);
        var parsed = ParseTransport(utf8Proposal);
        return BuildProposal(source, parsed);
    }

    private static void ValidateTransportEnvelope(ReadOnlySpan<byte> utf8Proposal)
    {
        if (utf8Proposal.IsEmpty)
        {
            throw new StateInterpretationException(
                "State Interpreter proposal JSON is empty.");
        }

        if (utf8Proposal.Length > MaxProposalJsonBytes)
        {
            throw new StateInterpretationException(
                "State Interpreter proposal JSON exceeds the parser safety ceiling.");
        }

        if (utf8Proposal.Length >= 3 &&
            utf8Proposal[0] == 0xEF &&
            utf8Proposal[1] == 0xBB &&
            utf8Proposal[2] == 0xBF)
        {
            throw new StateInterpretationException(
                "State Interpreter proposal JSON must not contain a UTF-8 BOM.");
        }

        var options = new JsonReaderOptions
        {
            AllowTrailingCommas = false,
            CommentHandling = JsonCommentHandling.Disallow,
            MaxDepth = MaxProposalJsonDepth
        };

        var reader = new Utf8JsonReader(utf8Proposal, options);
        var objectProperties = new Stack<HashSet<string>>();
        var sawRoot = false;
        var rootClosed = false;

        try
        {
            while (reader.Read())
            {
                if (!sawRoot)
                {
                    if (reader.TokenType != JsonTokenType.StartObject ||
                        reader.CurrentDepth != 0)
                    {
                        throw new StateInterpretationException(
                            "State Interpreter proposal JSON root must be an object.");
                    }

                    sawRoot = true;
                }
                else if (rootClosed)
                {
                    throw new StateInterpretationException(
                        "State Interpreter proposal JSON contains content after the root object.");
                }

                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        objectProperties.Push(new HashSet<string>(StringComparer.Ordinal));
                        break;

                    case JsonTokenType.PropertyName:
                        if (objectProperties.Count == 0)
                        {
                            throw new StateInterpretationException(
                                "State Interpreter proposal JSON contains a property outside an object.");
                        }

                        var propertyName = reader.GetString();
                        if (propertyName is null)
                        {
                            throw new StateInterpretationException(
                                "State Interpreter proposal JSON contains an invalid property name.");
                        }

                        if (!objectProperties.Peek().Add(propertyName))
                        {
                            throw new StateInterpretationException(
                                "State Interpreter proposal JSON contains a duplicate property.");
                        }

                        break;

                    case JsonTokenType.String:
                        _ = reader.GetString()
                            ?? throw new StateInterpretationException(
                                "State Interpreter proposal JSON contains an invalid string value.");
                        break;

                    case JsonTokenType.EndObject:
                        if (objectProperties.Count == 0)
                        {
                            throw new StateInterpretationException(
                                "State Interpreter proposal JSON contains an unexpected object terminator.");
                        }

                        objectProperties.Pop();
                        if (reader.CurrentDepth == 0)
                        {
                            rootClosed = true;
                        }

                        break;
                }
            }
        }
        catch (JsonException)
        {
            throw new StateInterpretationException(
                "State Interpreter proposal JSON is syntactically invalid.");
        }
        catch (InvalidOperationException)
        {
            throw new StateInterpretationException(
                "State Interpreter proposal JSON contains invalid Unicode data.");
        }

        if (!sawRoot || !rootClosed || objectProperties.Count != 0)
        {
            throw new StateInterpretationException(
                "State Interpreter proposal JSON ended before the root object was complete.");
        }
    }

    private static ParsedTransport ParseTransport(ReadOnlySpan<byte> utf8Proposal)
    {
        try
        {
            using var document = JsonDocument.Parse(
                utf8Proposal.ToArray(),
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = false,
                    CommentHandling = JsonCommentHandling.Disallow,
                    MaxDepth = MaxProposalJsonDepth
                });

            var root = document.RootElement;
            RequireObject(root, "root");
            EnsureOnlyProperties(root, "root", "schemaVersion", "mutations");

            var schemaVersion = RequireString(root, "schemaVersion", "root");
            if (!string.Equals(schemaVersion, JsonSchemaVersion, StringComparison.Ordinal))
            {
                throw new StateInterpretationException(
                    "State Interpreter proposal JSON schemaVersion is unsupported.");
            }

            var mutationsElement = RequireProperty(root, "mutations", "root");
            if (mutationsElement.ValueKind != JsonValueKind.Array)
            {
                throw new StateInterpretationException(
                    "State Interpreter proposal JSON mutations must be an array.");
            }

            var mutations = new List<ParsedMutation>();
            foreach (var mutationElement in mutationsElement.EnumerateArray())
            {
                RequireObject(mutationElement, "mutation");
                EnsureOnlyProperties(
                    mutationElement,
                    "mutation",
                    "domain",
                    "operation",
                    "subjectCharacterId",
                    "targetCharacterId",
                    "existingRecordId",
                    "text",
                    "supportingRecordIds");

                var domain = RequireString(mutationElement, "domain", "mutation");
                var operation = RequireString(mutationElement, "operation", "mutation");
                var subjectCharacterId = RequireNullableString(
                    mutationElement,
                    "subjectCharacterId",
                    "mutation");
                var targetCharacterId = RequireNullableString(
                    mutationElement,
                    "targetCharacterId",
                    "mutation");
                var existingRecordId = RequireNullableString(
                    mutationElement,
                    "existingRecordId",
                    "mutation");
                var text = RequireNullableString(mutationElement, "text", "mutation");

                var supportElement = RequireProperty(
                    mutationElement,
                    "supportingRecordIds",
                    "mutation");
                if (supportElement.ValueKind != JsonValueKind.Array)
                {
                    throw new StateInterpretationException(
                        "State Interpreter proposal supportingRecordIds must be an array.");
                }

                var supportingRecordIds = new List<string>();
                foreach (var supportId in supportElement.EnumerateArray())
                {
                    if (supportId.ValueKind != JsonValueKind.String)
                    {
                        throw new StateInterpretationException(
                            "State Interpreter proposal supportingRecordIds must contain only strings.");
                    }

                    supportingRecordIds.Add(
                        supportId.GetString()
                        ?? throw new StateInterpretationException(
                            "State Interpreter proposal supportingRecordIds contains an invalid string."));
                }

                mutations.Add(new ParsedMutation(
                    domain,
                    operation,
                    subjectCharacterId,
                    targetCharacterId,
                    existingRecordId,
                    text,
                    supportingRecordIds));
            }

            return new ParsedTransport(mutations);
        }
        catch (JsonException)
        {
            throw new StateInterpretationException(
                "State Interpreter proposal JSON is syntactically invalid.");
        }
    }

    private static StateInterpretationProposal BuildProposal(
        StateInterpretationSource source,
        ParsedTransport parsed)
    {
        var roster = StateInterpretationInvariants.RosterValues(source);
        var mutations = ImmutableArray.CreateBuilder<StateMutationCandidate>(
            parsed.Mutations.Count);
        var semanticKeys = new HashSet<string>(StringComparer.Ordinal);

        foreach (var parsedMutation in parsed.Mutations)
        {
            var mutation = BuildMutation(source, roster, parsedMutation);
            if (!semanticKeys.Add(BuildSemanticMutationKey(mutation)))
            {
                throw new StateInterpretationException(
                    "State Interpreter proposal contains an exact duplicate semantic mutation.");
            }

            mutations.Add(mutation);
        }

        return new StateInterpretationProposal(
            ContractVersion,
            source.CandidateContentIdentityContract,
            source.CandidateContentHash,
            source.SourceSceneId,
            mutations.ToImmutable());
    }

    private static StateMutationCandidate BuildMutation(
        StateInterpretationSource source,
        HashSet<string> roster,
        ParsedMutation parsed)
    {
        var domain = ParseDomain(parsed.Domain);
        var operation = ParseOperation(parsed.Operation);
        var supportingRecordIds = ParseSupportingRecordIds(parsed.SupportingRecordIds);

        return domain switch
        {
            StateMutationDomain.WorldState or
            StateMutationDomain.SceneState or
            StateMutationDomain.UnresolvedProposition or
            StateMutationDomain.Pressure => BuildGlobalMutation(
                domain,
                operation,
                parsed,
                supportingRecordIds),

            StateMutationDomain.CharacterKnowledge or
            StateMutationDomain.CharacterMemory => BuildAppendOnlyCharacterMutation(
                domain,
                operation,
                parsed,
                roster,
                supportingRecordIds),

            StateMutationDomain.CharacterBelief or
            StateMutationDomain.CharacterSuspicion or
            StateMutationDomain.CharacterGoal or
            StateMutationDomain.CharacterDisposition or
            StateMutationDomain.CharacterCircumstance => BuildMutableCharacterMutation(
                domain,
                operation,
                parsed,
                roster,
                supportingRecordIds),

            StateMutationDomain.CharacterClaim => BuildClaimMutation(
                source,
                operation,
                parsed,
                roster,
                supportingRecordIds),

            StateMutationDomain.Relationship => BuildRelationshipMutation(
                operation,
                parsed,
                roster,
                supportingRecordIds),

            _ => throw new StateInterpretationException(
                "State Interpreter proposal contains an unsupported mutation domain.")
        };
    }

    private static GlobalStateMutationCandidate BuildGlobalMutation(
        StateMutationDomain domain,
        MutationOperation operation,
        ParsedMutation parsed,
        ImmutableArray<RecordId> supportingRecordIds)
    {
        RequireNull(parsed.SubjectCharacterId, "subjectCharacterId");
        RequireNull(parsed.TargetCharacterId, "targetCharacterId");

        return new GlobalStateMutationCandidate(
            domain,
            BuildChange(operation, parsed.ExistingRecordId, parsed.Text),
            supportingRecordIds);
    }

    private static AppendOnlyCharacterStateMutationCandidate BuildAppendOnlyCharacterMutation(
        StateMutationDomain domain,
        MutationOperation operation,
        ParsedMutation parsed,
        HashSet<string> roster,
        ImmutableArray<RecordId> supportingRecordIds)
    {
        if (operation != MutationOperation.Add)
        {
            throw new StateInterpretationException(
                "State Interpreter append-only Character domains support only add proposals in E0.");
        }

        var subject = ParseRosterCharacterId(
            parsed.SubjectCharacterId,
            roster,
            "subjectCharacterId");
        RequireNull(parsed.TargetCharacterId, "targetCharacterId");

        var change = BuildChange(operation, parsed.ExistingRecordId, parsed.Text);
        if (change is not AddStateMutationChange addChange)
        {
            throw new StateInterpretationException(
                "State Interpreter append-only Character mutation could not be constructed safely.");
        }

        return new AppendOnlyCharacterStateMutationCandidate(
            domain,
            subject,
            addChange,
            supportingRecordIds);
    }

    private static MutableCharacterStateMutationCandidate BuildMutableCharacterMutation(
        StateMutationDomain domain,
        MutationOperation operation,
        ParsedMutation parsed,
        HashSet<string> roster,
        ImmutableArray<RecordId> supportingRecordIds)
    {
        var subject = ParseRosterCharacterId(
            parsed.SubjectCharacterId,
            roster,
            "subjectCharacterId");
        RequireNull(parsed.TargetCharacterId, "targetCharacterId");

        return new MutableCharacterStateMutationCandidate(
            domain,
            subject,
            BuildChange(operation, parsed.ExistingRecordId, parsed.Text),
            supportingRecordIds);
    }

    private static CharacterClaimMutationCandidate BuildClaimMutation(
        StateInterpretationSource source,
        MutationOperation operation,
        ParsedMutation parsed,
        HashSet<string> roster,
        ImmutableArray<RecordId> supportingRecordIds)
    {
        if (operation != MutationOperation.Add)
        {
            throw new StateInterpretationException(
                "State Interpreter CharacterClaim supports only add proposals.");
        }

        var subject = ParseRosterCharacterId(
            parsed.SubjectCharacterId,
            roster,
            "subjectCharacterId");
        if (subject != source.SourceCharacterId)
        {
            throw new StateInterpretationException(
                "State Interpreter CharacterClaim subject must equal the source Character.");
        }

        RequireNull(parsed.TargetCharacterId, "targetCharacterId");
        RequireNull(parsed.ExistingRecordId, "existingRecordId");
        var text = RequireMutationText(parsed.Text);

        return new CharacterClaimMutationCandidate(
            subject,
            text,
            supportingRecordIds);
    }

    private static RelationshipStateMutationCandidate BuildRelationshipMutation(
        MutationOperation operation,
        ParsedMutation parsed,
        HashSet<string> roster,
        ImmutableArray<RecordId> supportingRecordIds)
    {
        var subject = ParseRosterCharacterId(
            parsed.SubjectCharacterId,
            roster,
            "subjectCharacterId");
        var target = ParseRosterCharacterId(
            parsed.TargetCharacterId,
            roster,
            "targetCharacterId");

        if (subject == target)
        {
            throw new StateInterpretationException(
                "State Interpreter Relationship subject and target must be distinct Characters.");
        }

        return new RelationshipStateMutationCandidate(
            subject,
            target,
            BuildChange(operation, parsed.ExistingRecordId, parsed.Text),
            supportingRecordIds);
    }

    private static StateMutationChange BuildChange(
        MutationOperation operation,
        string? existingRecordId,
        string? text) =>
        operation switch
        {
            MutationOperation.Add => BuildAdd(existingRecordId, text),
            MutationOperation.Supersede => BuildSupersede(existingRecordId, text),
            MutationOperation.Deactivate => BuildDeactivate(existingRecordId, text),
            _ => throw new StateInterpretationException(
                "State Interpreter proposal contains an unsupported mutation operation.")
        };

    private static AddStateMutationChange BuildAdd(
        string? existingRecordId,
        string? text)
    {
        RequireNull(existingRecordId, "existingRecordId");
        return new AddStateMutationChange(RequireMutationText(text));
    }

    private static SupersedeStateMutationChange BuildSupersede(
        string? existingRecordId,
        string? text)
    {
        if (existingRecordId is null)
        {
            throw new StateInterpretationException(
                "State Interpreter supersede proposal requires existingRecordId.");
        }

        return new SupersedeStateMutationChange(
            ParseRecordId(existingRecordId, "existingRecordId"),
            RequireMutationText(text));
    }

    private static DeactivateStateMutationChange BuildDeactivate(
        string? existingRecordId,
        string? text)
    {
        if (existingRecordId is null)
        {
            throw new StateInterpretationException(
                "State Interpreter deactivate proposal requires existingRecordId.");
        }

        RequireNull(text, "text");
        return new DeactivateStateMutationChange(
            ParseRecordId(existingRecordId, "existingRecordId"));
    }

    private static ImmutableArray<RecordId> ParseSupportingRecordIds(
        List<string> rawIds)
    {
        var ids = ImmutableArray.CreateBuilder<RecordId>(rawIds.Count);
        var values = new HashSet<string>(StringComparer.Ordinal);

        foreach (var rawId in rawIds)
        {
            var id = ParseRecordId(rawId, "supportingRecordIds");
            if (!values.Add(id.Value))
            {
                throw new StateInterpretationException(
                    "State Interpreter proposal supportingRecordIds contains a duplicate Record ID.");
            }

            ids.Add(id);
        }

        return ids
            .ToImmutable()
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static StateMutationDomain ParseDomain(string rawDomain) =>
        rawDomain switch
        {
            "worldState" => StateMutationDomain.WorldState,
            "sceneState" => StateMutationDomain.SceneState,
            "unresolvedProposition" => StateMutationDomain.UnresolvedProposition,
            "characterKnowledge" => StateMutationDomain.CharacterKnowledge,
            "characterBelief" => StateMutationDomain.CharacterBelief,
            "characterSuspicion" => StateMutationDomain.CharacterSuspicion,
            "characterMemory" => StateMutationDomain.CharacterMemory,
            "characterGoal" => StateMutationDomain.CharacterGoal,
            "characterDisposition" => StateMutationDomain.CharacterDisposition,
            "characterCircumstance" => StateMutationDomain.CharacterCircumstance,
            "characterClaim" => StateMutationDomain.CharacterClaim,
            "relationship" => StateMutationDomain.Relationship,
            "pressure" => StateMutationDomain.Pressure,
            _ => throw new StateInterpretationException(
                "State Interpreter proposal contains an unsupported mutation domain.")
        };

    private static MutationOperation ParseOperation(string rawOperation) =>
        rawOperation switch
        {
            "add" => MutationOperation.Add,
            "supersede" => MutationOperation.Supersede,
            "deactivate" => MutationOperation.Deactivate,
            _ => throw new StateInterpretationException(
                "State Interpreter proposal contains an unsupported mutation operation.")
        };

    private static CharacterId ParseRosterCharacterId(
        string? rawId,
        HashSet<string> roster,
        string fieldName)
    {
        if (rawId is null)
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal field '{fieldName}' requires a Character ID.");
        }

        CharacterId id;
        try
        {
            id = CharacterId.From(rawId);
        }
        catch (Exception exception) when (
            exception is ArgumentException or InvalidOperationException)
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal field '{fieldName}' contains an invalid Character ID.");
        }

        if (!roster.Contains(id.Value))
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal field '{fieldName}' contains a Character outside the source roster.");
        }

        return id;
    }

    private static RecordId ParseRecordId(string rawId, string fieldName)
    {
        try
        {
            return RecordId.From(rawId);
        }
        catch (Exception exception) when (
            exception is ArgumentException or InvalidOperationException)
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal field '{fieldName}' contains an invalid Record ID.");
        }
    }

    private static string RequireMutationText(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new StateInterpretationException(
                "State Interpreter mutation text must be non-empty.");
        }

        for (var index = 0; index < text.Length; index++)
        {
            var character = text[index];
            if (char.IsHighSurrogate(character))
            {
                if (index + 1 >= text.Length ||
                    !char.IsLowSurrogate(text[index + 1]))
                {
                    throw new StateInterpretationException(
                        "State Interpreter mutation text contains invalid Unicode.");
                }

                index++;
            }
            else if (char.IsLowSurrogate(character))
            {
                throw new StateInterpretationException(
                    "State Interpreter mutation text contains invalid Unicode.");
            }
        }

        if (!text.IsNormalized(NormalizationForm.FormC))
        {
            throw new StateInterpretationException(
                "State Interpreter mutation text must already be Unicode NFC.");
        }

        var hasDisplayBearingScalar = false;
        foreach (var rune in text.EnumerateRunes())
        {
            var category = Rune.GetUnicodeCategory(rune);
            if (category == UnicodeCategory.Control &&
                rune.Value is not 0x09 and not 0x0A)
            {
                throw new StateInterpretationException(
                    "State Interpreter mutation text contains a forbidden control character.");
            }

            if (!Rune.IsWhiteSpace(rune) &&
                category is not UnicodeCategory.Control and
                not UnicodeCategory.Format and
                not UnicodeCategory.NonSpacingMark and
                not UnicodeCategory.SpacingCombiningMark and
                not UnicodeCategory.EnclosingMark)
            {
                hasDisplayBearingScalar = true;
            }
        }

        if (!hasDisplayBearingScalar)
        {
            throw new StateInterpretationException(
                "State Interpreter mutation text must contain display-bearing content.");
        }

        return text;
    }

    private static string BuildSemanticMutationKey(StateMutationCandidate mutation)
    {
        var builder = new StringBuilder();
        AppendKeySegment(
            builder,
            ((int)mutation.Domain).ToString(CultureInfo.InvariantCulture));

        switch (mutation)
        {
            case GlobalStateMutationCandidate global:
                AppendKeySegment(builder, "global");
                AppendChangeKey(builder, global.Change);
                break;

            case AppendOnlyCharacterStateMutationCandidate appendOnly:
                AppendKeySegment(builder, "append-character");
                AppendKeySegment(builder, appendOnly.SubjectCharacterId.Value);
                AppendChangeKey(builder, appendOnly.Change);
                break;

            case MutableCharacterStateMutationCandidate mutable:
                AppendKeySegment(builder, "mutable-character");
                AppendKeySegment(builder, mutable.SubjectCharacterId.Value);
                AppendChangeKey(builder, mutable.Change);
                break;

            case CharacterClaimMutationCandidate claim:
                AppendKeySegment(builder, "claim");
                AppendKeySegment(builder, claim.SubjectCharacterId.Value);
                AppendKeySegment(builder, claim.Text);
                break;

            case RelationshipStateMutationCandidate relationship:
                AppendKeySegment(builder, "relationship");
                AppendKeySegment(builder, relationship.SubjectCharacterId.Value);
                AppendKeySegment(builder, relationship.TargetCharacterId.Value);
                AppendChangeKey(builder, relationship.Change);
                break;

            default:
                throw new StateInterpretationException(
                    "State Interpreter semantic mutation type is unsupported.");
        }

        AppendKeySegment(builder, "support");
        foreach (var supportId in mutation.SupportingRecordIds)
        {
            AppendKeySegment(builder, supportId.Value);
        }

        return builder.ToString();
    }

    private static void AppendChangeKey(
        StringBuilder builder,
        StateMutationChange change)
    {
        switch (change)
        {
            case AddStateMutationChange add:
                AppendKeySegment(builder, "add");
                AppendKeySegment(builder, add.Text);
                break;

            case SupersedeStateMutationChange supersede:
                AppendKeySegment(builder, "supersede");
                AppendKeySegment(builder, supersede.ExistingRecordId.Value);
                AppendKeySegment(builder, supersede.Text);
                break;

            case DeactivateStateMutationChange deactivate:
                AppendKeySegment(builder, "deactivate");
                AppendKeySegment(builder, deactivate.ExistingRecordId.Value);
                break;

            default:
                throw new StateInterpretationException(
                    "State Interpreter semantic change type is unsupported.");
        }
    }

    private static void AppendKeySegment(StringBuilder builder, string value)
    {
        builder.Append(value.Length.ToString(CultureInfo.InvariantCulture));
        builder.Append(':');
        builder.Append(value);
        builder.Append(';');
    }

    private static void RequireNull(string? value, string fieldName)
    {
        if (value is not null)
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal field '{fieldName}' must be null for this mutation shape.");
        }
    }

    private static void RequireObject(JsonElement element, string objectName)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal JSON {objectName} must be an object.");
        }
    }

    private static string RequireString(
        JsonElement parent,
        string propertyName,
        string parentName)
    {
        var element = RequireProperty(parent, propertyName, parentName);
        if (element.ValueKind != JsonValueKind.String)
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal JSON field '{propertyName}' must be a string.");
        }

        return element.GetString()
            ?? throw new StateInterpretationException(
                $"State Interpreter proposal JSON field '{propertyName}' contains an invalid string.");
    }

    private static string? RequireNullableString(
        JsonElement parent,
        string propertyName,
        string parentName)
    {
        var element = RequireProperty(parent, propertyName, parentName);
        if (element.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        if (element.ValueKind != JsonValueKind.String)
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal JSON field '{propertyName}' must be a string or null.");
        }

        return element.GetString()
            ?? throw new StateInterpretationException(
                $"State Interpreter proposal JSON field '{propertyName}' contains an invalid string.");
    }

    private static JsonElement RequireProperty(
        JsonElement parent,
        string propertyName,
        string parentName)
    {
        if (!parent.TryGetProperty(propertyName, out var element))
        {
            throw new StateInterpretationException(
                $"State Interpreter proposal JSON {parentName} is missing a required property.");
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
                throw new StateInterpretationException(
                    $"State Interpreter proposal JSON {objectName} contains an unknown property.");
            }
        }
    }

    private enum MutationOperation
    {
        Add,
        Supersede,
        Deactivate
    }

    private sealed class ParsedTransport
    {
        public ParsedTransport(List<ParsedMutation> mutations) => Mutations = mutations;

        public List<ParsedMutation> Mutations { get; }
    }

    private sealed class ParsedMutation
    {
        public ParsedMutation(
            string domain,
            string operation,
            string? subjectCharacterId,
            string? targetCharacterId,
            string? existingRecordId,
            string? text,
            List<string> supportingRecordIds)
        {
            Domain = domain;
            Operation = operation;
            SubjectCharacterId = subjectCharacterId;
            TargetCharacterId = targetCharacterId;
            ExistingRecordId = existingRecordId;
            Text = text;
            SupportingRecordIds = supportingRecordIds;
        }

        public string Domain { get; }
        public string Operation { get; }
        public string? SubjectCharacterId { get; }
        public string? TargetCharacterId { get; }
        public string? ExistingRecordId { get; }
        public string? Text { get; }
        public List<string> SupportingRecordIds { get; }
    }
}
