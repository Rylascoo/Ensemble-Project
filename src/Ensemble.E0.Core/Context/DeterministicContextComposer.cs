using System.Collections.Immutable;
using System.Security.Cryptography;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;

namespace Ensemble.E0.Core.Context;

public static class DeterministicContextComposer
{
    private const string OpportunityText = "You have the current opportunity to act.";

    public static ContextCompositionEvaluation Compose(
        CharacterAccessProjection projection,
        CharacterId currentOpportunityCharacterId)
    {
        if (projection is null)
        {
            throw new ContextCompositionException("Character Access projection is required.");
        }

        try
        {
            _ = currentOpportunityCharacterId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new ContextCompositionException(
                "Current opportunity Character ID is uninitialized.",
                exception);
        }

        if (currentOpportunityCharacterId != projection.SubjectCharacterId)
        {
            throw new ContextCompositionException(
                "Current opportunity Character must match the Access projection subject.");
        }

        try
        {
            var roster = CopyAndValidateRoster(projection);
            var subjectMatches = roster.Count(
                participant => participant.CharacterId == projection.SubjectCharacterId);
            if (subjectMatches != 1)
            {
                throw new ContextCompositionException(
                    "Access projection subject must resolve exactly once in the Context roster.");
            }

            if (!roster.Any(participant => participant.CharacterId == currentOpportunityCharacterId))
            {
                throw new ContextCompositionException(
                    "Current opportunity Character must be present in the Context roster.");
            }

            var sceneState = CopyRecords(projection.SceneState, nameof(projection.SceneState));
            var pressures = CopyRecords(projection.Pressures, nameof(projection.Pressures));
            var constitution = CopyRecords(projection.Constitution, nameof(projection.Constitution));
            var disposition = CopyRecords(projection.Disposition, nameof(projection.Disposition));
            var circumstance = CopyRecords(projection.Circumstance, nameof(projection.Circumstance));
            var observations = CopyRecords(projection.Observations, nameof(projection.Observations));
            var knowledge = CopyRecords(projection.Knowledge, nameof(projection.Knowledge));
            var beliefs = CopyRecords(projection.Beliefs, nameof(projection.Beliefs));
            var suspicions = CopyRecords(projection.Suspicions, nameof(projection.Suspicions));
            var memories = CopyRecords(projection.Memories, nameof(projection.Memories));
            var goals = CopyRecords(projection.Goals, nameof(projection.Goals));
            var relationships = CopyRelationships(projection.Relationships);

            ValidateUniqueRecordIds(
                sceneState,
                pressures,
                constitution,
                disposition,
                circumstance,
                observations,
                knowledge,
                beliefs,
                suspicions,
                memories,
                goals,
                relationships);
            ValidateRelationshipTargets(roster, relationships);

            var content = new ContextSemanticContent(
                E0ContextContracts.SchemaVersion,
                E0ContextContracts.CompositionContract,
                projection.SceneId,
                projection.SubjectCharacterId,
                currentOpportunityCharacterId,
                roster,
                sceneState,
                pressures,
                constitution,
                disposition,
                circumstance,
                observations,
                knowledge,
                beliefs,
                suspicions,
                memories,
                goals,
                relationships);

            var structuredBytes = ContextPacketCanonicalizer.SerializeStructured(content);
            var structuredHash = Sha256Lower(structuredBytes);
            var packetId = ContextPacketId.From($"CTX:{structuredHash}");

            var rendered = Render(content);
            var renderedBytes = ContextPacketCanonicalizer.SerializeRendered(rendered);
            var renderedHash = Sha256Lower(renderedBytes);

            ValidateHash(structuredHash, nameof(structuredHash));
            ValidateHash(renderedHash, nameof(renderedHash));
            if (!string.Equals(
                    packetId.Value,
                    $"CTX:{structuredHash}",
                    StringComparison.Ordinal))
            {
                throw new ContextCompositionException(
                    "ContextPacketId does not match the structured Context hash.");
            }

            var packet = new ContextPacket(
                packetId,
                content.SchemaVersion,
                content.CompositionContract,
                content.SceneId,
                content.SubjectCharacterId,
                content.OpportunityCharacterId,
                content.Roster,
                content.SceneState,
                content.Pressures,
                content.Constitution,
                content.Disposition,
                content.Circumstance,
                content.Observations,
                content.Knowledge,
                content.Beliefs,
                content.Suspicions,
                content.Memories,
                content.Goals,
                content.Relationships,
                structuredHash,
                rendered,
                renderedHash);

            var trace = new ContextCompositionTrace(
                content.CompositionContract,
                rendered.RenderingContract,
                IncludedRecordIds(content),
                content.Roster
                    .Select(participant => participant.CharacterId)
                    .OrderBy(id => id.Value, StringComparer.Ordinal)
                    .ToImmutableArray(),
                content.OpportunityCharacterId,
                structuredHash,
                renderedHash);

            return new ContextCompositionEvaluation(packet, trace);
        }
        catch (FixtureValidationException exception)
        {
            throw new ContextCompositionException(
                $"Context input violates canonical text discipline: {exception.Message}",
                exception);
        }
        catch (ArgumentException exception)
        {
            throw new ContextCompositionException(
                $"Context identity construction failed: {exception.Message}",
                exception);
        }
    }

    private static ImmutableArray<ContextParticipant> CopyAndValidateRoster(
        CharacterAccessProjection projection)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var participants = projection.Roster
            .OrderBy(participant => participant.CharacterId.Value, StringComparer.Ordinal)
            .Select(participant =>
            {
                var id = participant.CharacterId.Value;
                if (!seen.Add(id))
                {
                    throw new ContextCompositionException(
                        $"Context roster contains duplicate Character ID '{id}'.");
                }

                var displayName = CanonicalText.Required(
                    participant.DisplayName,
                    $"Context roster display name for '{id}'");
                return new ContextParticipant(participant.CharacterId, displayName);
            })
            .ToImmutableArray();

        if (participants.IsDefaultOrEmpty)
        {
            throw new ContextCompositionException("Context roster may not be empty.");
        }

        return participants;
    }

    private static ImmutableArray<ContextRecord> CopyRecords(
        ImmutableArray<PermittedRecord> records,
        string fieldName) =>
        records
            .OrderBy(record => record.RecordId.Value, StringComparer.Ordinal)
            .Select(record => new ContextRecord(
                record.RecordId,
                CanonicalText.Required(
                    record.Text,
                    $"{fieldName} record '{record.RecordId.Value}'")))
            .ToImmutableArray();

    private static ImmutableArray<ContextRelationship> CopyRelationships(
        ImmutableArray<PermittedRelationship> relationships) =>
        relationships
            .OrderBy(relationship => relationship.RecordId.Value, StringComparer.Ordinal)
            .Select(relationship => new ContextRelationship(
                relationship.RecordId,
                relationship.TargetCharacterId,
                CanonicalText.Required(
                    relationship.Text,
                    $"Relationship '{relationship.RecordId.Value}'")))
            .ToImmutableArray();

    private static void ValidateRelationshipTargets(
        ImmutableArray<ContextParticipant> roster,
        ImmutableArray<ContextRelationship> relationships)
    {
        var rosterIds = roster
            .Select(participant => participant.CharacterId.Value)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var relationship in relationships)
        {
            var target = relationship.TargetCharacterId.Value;
            if (!rosterIds.Contains(target))
            {
                throw new ContextCompositionException(
                    $"Relationship '{relationship.RecordId}' targets non-roster Character '{target}'.");
            }
        }
    }

    private static void ValidateUniqueRecordIds(
        ImmutableArray<ContextRecord> sceneState,
        ImmutableArray<ContextRecord> pressures,
        ImmutableArray<ContextRecord> constitution,
        ImmutableArray<ContextRecord> disposition,
        ImmutableArray<ContextRecord> circumstance,
        ImmutableArray<ContextRecord> observations,
        ImmutableArray<ContextRecord> knowledge,
        ImmutableArray<ContextRecord> beliefs,
        ImmutableArray<ContextRecord> suspicions,
        ImmutableArray<ContextRecord> memories,
        ImmutableArray<ContextRecord> goals,
        ImmutableArray<ContextRelationship> relationships)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);

        AddUnique(seen, sceneState.Select(record => record.RecordId));
        AddUnique(seen, pressures.Select(record => record.RecordId));
        AddUnique(seen, constitution.Select(record => record.RecordId));
        AddUnique(seen, disposition.Select(record => record.RecordId));
        AddUnique(seen, circumstance.Select(record => record.RecordId));
        AddUnique(seen, observations.Select(record => record.RecordId));
        AddUnique(seen, knowledge.Select(record => record.RecordId));
        AddUnique(seen, beliefs.Select(record => record.RecordId));
        AddUnique(seen, suspicions.Select(record => record.RecordId));
        AddUnique(seen, memories.Select(record => record.RecordId));
        AddUnique(seen, goals.Select(record => record.RecordId));
        AddUnique(seen, relationships.Select(record => record.RecordId));
    }

    private static void AddUnique(HashSet<string> seen, IEnumerable<RecordId> ids)
    {
        foreach (var id in ids)
        {
            var value = id.Value;
            if (!seen.Add(value))
            {
                throw new ContextCompositionException(
                    $"Context packet contains duplicate Record ID '{value}'.");
            }
        }
    }

    private static RenderedContext Render(ContextSemanticContent content)
    {
        var namesById = content.Roster.ToDictionary(
            participant => participant.CharacterId,
            participant => participant.DisplayName);
        var subjectName = namesById[content.SubjectCharacterId];

        var sections = new[]
        {
            string.Join(
                "\n",
                "[WHO YOU ARE]",
                "Name:",
                RenderEntries(new[] { subjectName }),
                "Constitution:",
                RenderEntries(content.Constitution.Select(record => record.Text)),
                "Disposition:",
                RenderEntries(content.Disposition.Select(record => record.Text))),
            RenderSection(
                "[WHAT IS HAPPENING]",
                content.SceneState.Select(record => record.Text)),
            RenderSection("[RIGHT NOW]", content.Circumstance.Select(record => record.Text)),
            RenderSection(
                "[WHAT YOU OBSERVED]",
                content.Observations.Select(record => record.Text)),
            RenderSection("[WHAT YOU KNOW]", content.Knowledge.Select(record => record.Text)),
            RenderSection("[WHAT YOU BELIEVE]", content.Beliefs.Select(record => record.Text)),
            RenderSection(
                "[WHAT YOU SUSPECT]",
                content.Suspicions.Select(record => record.Text)),
            RenderSection("[WHAT YOU REMEMBER]", content.Memories.Select(record => record.Text)),
            RenderSection(
                "[WHO IS PRESENT]",
                content.Roster.Select(participant => participant.DisplayName)),
            "[RELATIONSHIPS]\n" + RenderRelationships(content.Relationships, namesById),
            RenderSection("[WHAT YOU WANT]", content.Goals.Select(record => record.Text)),
            RenderSection("[PRESSURES]", content.Pressures.Select(record => record.Text))
        };

        return new RenderedContext(
            E0ContextContracts.RenderingContract,
            string.Join("\n\n", sections),
            string.Empty,
            OpportunityText);
    }

    private static string RenderSection(string heading, IEnumerable<string> entries) =>
        $"{heading}\n{RenderEntries(entries)}";

    private static string RenderEntries(IEnumerable<string> entries)
    {
        var materialized = entries.ToArray();
        if (materialized.Length == 0)
        {
            return "- none";
        }

        return string.Join("\n", materialized.Select(RenderBullet));
    }

    private static string RenderRelationships(
        ImmutableArray<ContextRelationship> relationships,
        IReadOnlyDictionary<CharacterId, string> namesById)
    {
        if (relationships.IsDefaultOrEmpty)
        {
            return "- none";
        }

        return string.Join(
            "\n",
            relationships.Select(relationship =>
            {
                if (!namesById.TryGetValue(relationship.TargetCharacterId, out var targetName))
                {
                    throw new ContextCompositionException(
                        $"Relationship '{relationship.RecordId}' target cannot be rendered.");
                }

                var lines = relationship.Text.Split('\n');
                var first = $"- {targetName}: {lines[0]}";
                if (lines.Length == 1)
                {
                    return first;
                }

                return first + "\n" + string.Join(
                    "\n",
                    lines.Skip(1).Select(line => $"  {line}"));
            }));
    }

    private static string RenderBullet(string value)
    {
        var lines = value.Split('\n');
        var first = $"- {lines[0]}";
        if (lines.Length == 1)
        {
            return first;
        }

        return first + "\n" + string.Join(
            "\n",
            lines.Skip(1).Select(line => $"  {line}"));
    }

    private static ImmutableArray<RecordId> IncludedRecordIds(ContextSemanticContent content) =>
        content.SceneState.Select(record => record.RecordId)
            .Concat(content.Pressures.Select(record => record.RecordId))
            .Concat(content.Constitution.Select(record => record.RecordId))
            .Concat(content.Disposition.Select(record => record.RecordId))
            .Concat(content.Circumstance.Select(record => record.RecordId))
            .Concat(content.Observations.Select(record => record.RecordId))
            .Concat(content.Knowledge.Select(record => record.RecordId))
            .Concat(content.Beliefs.Select(record => record.RecordId))
            .Concat(content.Suspicions.Select(record => record.RecordId))
            .Concat(content.Memories.Select(record => record.RecordId))
            .Concat(content.Goals.Select(record => record.RecordId))
            .Concat(content.Relationships.Select(record => record.RecordId))
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .ToImmutableArray();

    private static string Sha256Lower(byte[] bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

    private static void ValidateHash(string hash, string fieldName)
    {
        if (hash.Length != 64 || hash.Any(character => !IsLowerHex(character)))
        {
            throw new ContextCompositionException(
                $"{fieldName} must be exactly 64 lowercase hexadecimal characters.");
        }
    }

    private static bool IsLowerHex(char character) =>
        character is >= '0' and <= '9' or >= 'a' and <= 'f';
}
