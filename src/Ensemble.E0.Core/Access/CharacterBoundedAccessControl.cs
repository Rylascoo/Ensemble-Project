using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;

namespace Ensemble.E0.Core.Access;

public static class CharacterBoundedAccessControl
{
    public static CharacterAccessEvaluation Evaluate(
        ValidatedFixture fixture,
        CharacterId subjectCharacterId)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        try
        {
            _ = subjectCharacterId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new CharacterAccessException("Subject Character ID is uninitialized.", exception);
        }

        if (!string.Equals(
                fixture.AccessContract,
                E0FixtureDialect.AccessContract,
                StringComparison.Ordinal))
        {
            throw new CharacterAccessException(
                $"Unsupported access contract '{fixture.AccessContract}'.");
        }

        var subject = fixture.Characters.SingleOrDefault(character => character.Id == subjectCharacterId)
            ?? throw new CharacterAccessException(
                $"Character '{subjectCharacterId}' does not exist in the validated fixture.");

        if (!fixture.Scene.Roster.Contains(subjectCharacterId))
        {
            throw new CharacterAccessException(
                $"Character '{subjectCharacterId}' is not present in Scene '{fixture.Scene.Id}'.");
        }

        var charactersById = fixture.Characters.ToDictionary(character => character.Id);
        var roster = fixture.Scene.Roster
            .OrderBy(id => id.Value, StringComparer.Ordinal)
            .Select(id =>
            {
                if (!charactersById.TryGetValue(id, out var character))
                {
                    throw new CharacterAccessException(
                        $"Scene roster Character '{id}' does not resolve in the validated fixture.");
                }

                return new SceneParticipant(character.Id, character.DisplayName);
            })
            .ToImmutableArray();

        var projection = new CharacterAccessProjection(
            fixture.Scene.Id,
            subject.Id,
            roster,
            ProjectRecords(fixture.SceneState),
            ProjectRecords(fixture.Pressures),
            ProjectRecords(subject.Constitution),
            ProjectRecords(subject.Disposition),
            ProjectRecords(subject.Circumstance),
            ProjectRecords(subject.Observations),
            ProjectRecords(subject.Knowledge),
            ProjectRecords(subject.Beliefs),
            ProjectRecords(subject.Suspicions),
            ProjectRecords(subject.Memories),
            ProjectRecords(subject.Goals),
            ProjectRelationships(subject.Relationships));

        return new CharacterAccessEvaluation(
            projection,
            BuildDecisions(fixture, subject.Id));
    }

    private static ImmutableArray<PermittedRecord> ProjectRecords(
        IEnumerable<ValidatedRecord> records) =>
        records
            .OrderBy(record => record.Id.Value, StringComparer.Ordinal)
            .Select(record => new PermittedRecord(record.Id, record.Text))
            .ToImmutableArray();

    private static ImmutableArray<PermittedRelationship> ProjectRelationships(
        IEnumerable<ValidatedRelationship> relationships) =>
        relationships
            .OrderBy(relationship => relationship.Id.Value, StringComparer.Ordinal)
            .Select(relationship => new PermittedRelationship(
                relationship.Id,
                relationship.TargetCharacterId,
                relationship.Text))
            .ToImmutableArray();

    private static ImmutableArray<AccessDecision> BuildDecisions(
        ValidatedFixture fixture,
        CharacterId subjectCharacterId)
    {
        var decisions = new List<AccessDecision>();

        AddDecisions(
            decisions,
            fixture.HistoricalTruth,
            AccessDisposition.Deny,
            AccessReason.ProductionAuthorityExcluded);
        AddDecisions(
            decisions,
            fixture.UnresolvedPropositions,
            AccessDisposition.Deny,
            AccessReason.ProductionAuthorityExcluded);
        AddDecisions(
            decisions,
            fixture.WorldState,
            AccessDisposition.Deny,
            AccessReason.ProductionAuthorityExcluded);
        AddDecisions(
            decisions,
            fixture.SceneState,
            AccessDisposition.Permit,
            AccessReason.SharedSceneState);
        AddDecisions(
            decisions,
            fixture.Pressures,
            AccessDisposition.Permit,
            AccessReason.PublicPressure);

        foreach (var character in fixture.Characters)
        {
            var ownedBySubject = character.Id == subjectCharacterId;
            var disposition = ownedBySubject
                ? AccessDisposition.Permit
                : AccessDisposition.Deny;
            var reason = ownedBySubject
                ? AccessReason.OwnedBySubject
                : AccessReason.OwnedByOtherCharacterExcluded;

            AddCharacterDecisions(decisions, character, disposition, reason);
        }

        return decisions
            .OrderBy(decision => decision.RecordId.Value, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static void AddCharacterDecisions(
        ICollection<AccessDecision> decisions,
        ValidatedCharacter character,
        AccessDisposition disposition,
        AccessReason reason)
    {
        AddDecisions(decisions, character.Constitution, disposition, reason);
        AddDecisions(decisions, character.Disposition, disposition, reason);
        AddDecisions(decisions, character.Circumstance, disposition, reason);
        AddDecisions(decisions, character.Observations, disposition, reason);
        AddDecisions(decisions, character.Knowledge, disposition, reason);
        AddDecisions(decisions, character.Beliefs, disposition, reason);
        AddDecisions(decisions, character.Suspicions, disposition, reason);
        AddDecisions(decisions, character.Memories, disposition, reason);
        AddDecisions(decisions, character.Goals, disposition, reason);

        foreach (var relationship in character.Relationships)
        {
            decisions.Add(new AccessDecision(relationship.Id, disposition, reason));
        }
    }

    private static void AddDecisions(
        ICollection<AccessDecision> decisions,
        IEnumerable<ValidatedRecord> records,
        AccessDisposition disposition,
        AccessReason reason)
    {
        foreach (var record in records)
        {
            decisions.Add(new AccessDecision(record.Id, disposition, reason));
        }
    }
}
