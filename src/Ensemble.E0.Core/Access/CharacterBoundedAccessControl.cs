using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Core.Access;

public static class CharacterBoundedAccessControl
{
    public static CharacterAccessEvaluation Evaluate(
        ValidatedFixture fixture,
        CharacterId subjectCharacterId)
    {
        if (fixture is null)
        {
            throw new CharacterAccessException("Validated fixture is required.");
        }

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
            null,
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

    public static CharacterAccessEvaluation Evaluate(
        ProductionState sourceState,
        CharacterId subjectCharacterId)
    {
        if (sourceState is null)
        {
            throw new CharacterAccessException("Production state is required.");
        }

        string subjectValue;
        try
        {
            subjectValue = RequireCharacterId(subjectCharacterId);
            var stateHash = sourceState.StateHash.Value;
            if (!ProductionStateInvariants.IsLowerHexSha256(stateHash))
            {
                throw new CharacterAccessException(
                    "Production Access source StateHash is invalid.");
            }

            _ = SceneId.From(sourceState.SceneId.Value);
        }
        catch (CharacterAccessException)
        {
            throw;
        }
        catch (Exception exception) when (
            exception is InvalidOperationException or ArgumentException)
        {
            throw new CharacterAccessException(
                "Production Access source identity is invalid.",
                exception);
        }

        if (!string.Equals(
                sourceState.ContractVersion,
                ProductionStateContracts.StateContractVersion,
                StringComparison.Ordinal))
        {
            throw new CharacterAccessException(
                "Production Access source contract is unsupported.");
        }

        var roster = ValidateProductionCharactersAndRoster(sourceState, subjectCharacterId);
        var rosterIds = roster
            .Select(participant => participant.CharacterId.Value)
            .ToHashSet(StringComparer.Ordinal);

        var sceneState = ImmutableArray.CreateBuilder<PermittedRecord>();
        var pressures = ImmutableArray.CreateBuilder<PermittedRecord>();
        var constitution = ImmutableArray.CreateBuilder<PermittedRecord>();
        var disposition = ImmutableArray.CreateBuilder<PermittedRecord>();
        var circumstance = ImmutableArray.CreateBuilder<PermittedRecord>();
        var observations = ImmutableArray.CreateBuilder<PermittedRecord>();
        var knowledge = ImmutableArray.CreateBuilder<PermittedRecord>();
        var beliefs = ImmutableArray.CreateBuilder<PermittedRecord>();
        var suspicions = ImmutableArray.CreateBuilder<PermittedRecord>();
        var memories = ImmutableArray.CreateBuilder<PermittedRecord>();
        var goals = ImmutableArray.CreateBuilder<PermittedRecord>();
        var relationships = ImmutableArray.CreateBuilder<PermittedRelationship>();
        var decisions = ImmutableArray.CreateBuilder<AccessDecision>();

        if (sourceState.Records.IsDefault)
        {
            throw new CharacterAccessException("Production Access records are uninitialized.");
        }

        var previousRecordId = string.Empty;
        for (var index = 0; index < sourceState.Records.Length; index++)
        {
            var record = sourceState.Records[index]
                ?? throw new CharacterAccessException(
                    "Production Access contains an invalid record.");

            var recordId = RequireRecordId(record.RecordId);
            if (index != 0 && string.CompareOrdinal(previousRecordId, recordId) >= 0)
            {
                throw new CharacterAccessException(
                    "Production Access Record IDs are not unique and canonical.");
            }

            previousRecordId = recordId;
            ValidateRecordStructure(record, rosterIds);

            if (record.Lifecycle == ProductionRecordLifecycle.Inactive)
            {
                decisions.Add(new AccessDecision(
                    record.RecordId,
                    AccessDisposition.Deny,
                    AccessReason.InactiveRecordExcluded));
                continue;
            }

            switch (record)
            {
                case GlobalProductionRecord global:
                    ProjectGlobal(
                        global,
                        decisions,
                        sceneState,
                        pressures);
                    break;

                case CharacterProductionRecord character:
                    ProjectCharacter(
                        character,
                        subjectValue,
                        decisions,
                        constitution,
                        disposition,
                        circumstance,
                        observations,
                        knowledge,
                        beliefs,
                        suspicions,
                        memories,
                        goals);
                    break;

                case RelationshipProductionRecord relationship:
                    ProjectRelationship(
                        relationship,
                        subjectValue,
                        decisions,
                        relationships);
                    break;

                default:
                    throw new CharacterAccessException(
                        "Production Access record shape is unsupported.");
            }
        }

        var projection = new CharacterAccessProjection(
            sourceState.StateHash,
            sourceState.SceneId,
            subjectCharacterId,
            roster,
            sceneState.ToImmutable(),
            pressures.ToImmutable(),
            constitution.ToImmutable(),
            disposition.ToImmutable(),
            circumstance.ToImmutable(),
            observations.ToImmutable(),
            knowledge.ToImmutable(),
            beliefs.ToImmutable(),
            suspicions.ToImmutable(),
            memories.ToImmutable(),
            goals.ToImmutable(),
            relationships.ToImmutable());

        return new CharacterAccessEvaluation(projection, decisions.ToImmutable());
    }

    private static ImmutableArray<SceneParticipant> ValidateProductionCharactersAndRoster(
        ProductionState sourceState,
        CharacterId subjectCharacterId)
    {
        if (sourceState.Characters.IsDefault || sourceState.Characters.Length != 3 ||
            sourceState.RosterCharacterIds.IsDefault || sourceState.RosterCharacterIds.Length != 3)
        {
            throw new CharacterAccessException(
                "Production Access requires exactly three canonical Scene Characters.");
        }

        var participants = ImmutableArray.CreateBuilder<SceneParticipant>(3);
        var previousCharacterId = string.Empty;
        var previousRosterId = string.Empty;
        var subjectCharacterCount = 0;
        var subjectRosterCount = 0;

        for (var index = 0; index < 3; index++)
        {
            var character = sourceState.Characters[index]
                ?? throw new CharacterAccessException(
                    "Production Access contains an invalid Character.");
            var characterId = RequireCharacterId(character.CharacterId);
            var rosterId = RequireCharacterId(sourceState.RosterCharacterIds[index]);

            if (index != 0 &&
                (string.CompareOrdinal(previousCharacterId, characterId) >= 0 ||
                 string.CompareOrdinal(previousRosterId, rosterId) >= 0))
            {
                throw new CharacterAccessException(
                    "Production Access Character identities are not unique and canonical.");
            }

            if (!string.Equals(characterId, rosterId, StringComparison.Ordinal))
            {
                throw new CharacterAccessException(
                    "Production Access Characters do not exactly match the Scene roster.");
            }

            try
            {
                _ = CanonicalText.Required(
                    character.DisplayName,
                    "Production Character display name");
            }
            catch (FixtureValidationException exception)
            {
                throw new CharacterAccessException(
                    "Production Access Character display name is invalid.",
                    exception);
            }

            if (character.CharacterId == subjectCharacterId)
            {
                subjectCharacterCount++;
            }

            if (sourceState.RosterCharacterIds[index] == subjectCharacterId)
            {
                subjectRosterCount++;
            }

            participants.Add(new SceneParticipant(
                character.CharacterId,
                character.DisplayName));
            previousCharacterId = characterId;
            previousRosterId = rosterId;
        }

        if (subjectCharacterCount != 1 || subjectRosterCount != 1)
        {
            throw new CharacterAccessException(
                "Production Access subject must resolve exactly once in Characters and roster.");
        }

        return participants.ToImmutable();
    }

    private static void ValidateRecordStructure(
        ProductionRecord record,
        HashSet<string> rosterIds)
    {
        if (!Enum.IsDefined(record.Domain) || record.Domain == ProductionRecordDomain.Unspecified ||
            !Enum.IsDefined(record.Lifecycle) || record.Lifecycle == ProductionRecordLifecycle.Unspecified ||
            !Enum.IsDefined(record.Protection) || record.Protection == ProductionRecordProtection.Unspecified)
        {
            throw new CharacterAccessException(
                "Production Access record authority metadata is invalid.");
        }

        try
        {
            _ = CanonicalText.Required(record.Text, "Production record text");
        }
        catch (FixtureValidationException exception)
        {
            throw new CharacterAccessException(
                "Production Access record text is invalid.",
                exception);
        }

        switch (record)
        {
            case GlobalProductionRecord:
                if (record.Domain is not (
                    ProductionRecordDomain.HistoricalTruth or
                    ProductionRecordDomain.UnresolvedProposition or
                    ProductionRecordDomain.WorldState or
                    ProductionRecordDomain.SceneState or
                    ProductionRecordDomain.Pressure))
                {
                    throw new CharacterAccessException(
                        "Production Access global record domain is invalid.");
                }

                break;

            case CharacterProductionRecord character:
                if (record.Domain is not (
                    ProductionRecordDomain.CharacterConstitution or
                    ProductionRecordDomain.CharacterDisposition or
                    ProductionRecordDomain.CharacterCircumstance or
                    ProductionRecordDomain.CharacterObservation or
                    ProductionRecordDomain.CharacterKnowledge or
                    ProductionRecordDomain.CharacterBelief or
                    ProductionRecordDomain.CharacterSuspicion or
                    ProductionRecordDomain.CharacterMemory or
                    ProductionRecordDomain.CharacterGoal or
                    ProductionRecordDomain.CharacterClaim))
                {
                    throw new CharacterAccessException(
                        "Production Access Character record domain is invalid.");
                }

                var subject = RequireCharacterId(character.SubjectCharacterId);
                if (!rosterIds.Contains(subject))
                {
                    throw new CharacterAccessException(
                        "Production Access Character record subject is outside the Scene roster.");
                }

                break;

            case RelationshipProductionRecord relationship:
                if (record.Domain != ProductionRecordDomain.Relationship)
                {
                    throw new CharacterAccessException(
                        "Production Access Relationship record domain is invalid.");
                }

                var relationshipSubject = RequireCharacterId(relationship.SubjectCharacterId);
                var target = RequireCharacterId(relationship.TargetCharacterId);
                if (!rosterIds.Contains(relationshipSubject) ||
                    !rosterIds.Contains(target) ||
                    string.Equals(relationshipSubject, target, StringComparison.Ordinal))
                {
                    throw new CharacterAccessException(
                        "Production Access Relationship endpoints are invalid.");
                }

                break;

            default:
                throw new CharacterAccessException(
                    "Production Access record shape is unsupported.");
        }
    }

    private static void ProjectGlobal(
        GlobalProductionRecord record,
        ImmutableArray<AccessDecision>.Builder decisions,
        ImmutableArray<PermittedRecord>.Builder sceneState,
        ImmutableArray<PermittedRecord>.Builder pressures)
    {
        switch (record.Domain)
        {
            case ProductionRecordDomain.SceneState:
                decisions.Add(new AccessDecision(
                    record.RecordId,
                    AccessDisposition.Permit,
                    AccessReason.SharedSceneState));
                sceneState.Add(new PermittedRecord(record.RecordId, record.Text));
                break;

            case ProductionRecordDomain.Pressure:
                decisions.Add(new AccessDecision(
                    record.RecordId,
                    AccessDisposition.Permit,
                    AccessReason.PublicPressure));
                pressures.Add(new PermittedRecord(record.RecordId, record.Text));
                break;

            case ProductionRecordDomain.HistoricalTruth:
            case ProductionRecordDomain.UnresolvedProposition:
            case ProductionRecordDomain.WorldState:
                decisions.Add(new AccessDecision(
                    record.RecordId,
                    AccessDisposition.Deny,
                    AccessReason.ProductionAuthorityExcluded));
                break;

            default:
                throw new CharacterAccessException(
                    "Production Access global record domain is unsupported.");
        }
    }

    private static void ProjectCharacter(
        CharacterProductionRecord record,
        string subjectCharacterId,
        ImmutableArray<AccessDecision>.Builder decisions,
        ImmutableArray<PermittedRecord>.Builder constitution,
        ImmutableArray<PermittedRecord>.Builder disposition,
        ImmutableArray<PermittedRecord>.Builder circumstance,
        ImmutableArray<PermittedRecord>.Builder observations,
        ImmutableArray<PermittedRecord>.Builder knowledge,
        ImmutableArray<PermittedRecord>.Builder beliefs,
        ImmutableArray<PermittedRecord>.Builder suspicions,
        ImmutableArray<PermittedRecord>.Builder memories,
        ImmutableArray<PermittedRecord>.Builder goals)
    {
        if (record.Domain == ProductionRecordDomain.CharacterClaim)
        {
            decisions.Add(new AccessDecision(
                record.RecordId,
                AccessDisposition.Deny,
                AccessReason.CharacterClaimDisclosureDeferred));
            return;
        }

        if (!string.Equals(
                record.SubjectCharacterId.Value,
                subjectCharacterId,
                StringComparison.Ordinal))
        {
            decisions.Add(new AccessDecision(
                record.RecordId,
                AccessDisposition.Deny,
                AccessReason.OwnedByOtherCharacterExcluded));
            return;
        }

        decisions.Add(new AccessDecision(
            record.RecordId,
            AccessDisposition.Permit,
            AccessReason.OwnedBySubject));

        var projected = new PermittedRecord(record.RecordId, record.Text);
        switch (record.Domain)
        {
            case ProductionRecordDomain.CharacterConstitution:
                constitution.Add(projected);
                break;
            case ProductionRecordDomain.CharacterDisposition:
                disposition.Add(projected);
                break;
            case ProductionRecordDomain.CharacterCircumstance:
                circumstance.Add(projected);
                break;
            case ProductionRecordDomain.CharacterObservation:
                observations.Add(projected);
                break;
            case ProductionRecordDomain.CharacterKnowledge:
                knowledge.Add(projected);
                break;
            case ProductionRecordDomain.CharacterBelief:
                beliefs.Add(projected);
                break;
            case ProductionRecordDomain.CharacterSuspicion:
                suspicions.Add(projected);
                break;
            case ProductionRecordDomain.CharacterMemory:
                memories.Add(projected);
                break;
            case ProductionRecordDomain.CharacterGoal:
                goals.Add(projected);
                break;
            default:
                throw new CharacterAccessException(
                    "Production Access Character record domain is unsupported.");
        }
    }

    private static void ProjectRelationship(
        RelationshipProductionRecord record,
        string subjectCharacterId,
        ImmutableArray<AccessDecision>.Builder decisions,
        ImmutableArray<PermittedRelationship>.Builder relationships)
    {
        if (!string.Equals(
                record.SubjectCharacterId.Value,
                subjectCharacterId,
                StringComparison.Ordinal))
        {
            decisions.Add(new AccessDecision(
                record.RecordId,
                AccessDisposition.Deny,
                AccessReason.OwnedByOtherCharacterExcluded));
            return;
        }

        decisions.Add(new AccessDecision(
            record.RecordId,
            AccessDisposition.Permit,
            AccessReason.OwnedBySubject));
        relationships.Add(new PermittedRelationship(
            record.RecordId,
            record.TargetCharacterId,
            record.Text));
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

    private static string RequireCharacterId(CharacterId id)
    {
        try
        {
            var value = id.Value;
            if (CharacterId.From(value) != id)
            {
                throw new CharacterAccessException(
                    "Production Access Character ID is not canonical.");
            }

            return value;
        }
        catch (CharacterAccessException)
        {
            throw;
        }
        catch (Exception exception) when (
            exception is InvalidOperationException or ArgumentException)
        {
            throw new CharacterAccessException(
                "Production Access contains an invalid Character ID.",
                exception);
        }
    }

    private static string RequireRecordId(RecordId id)
    {
        try
        {
            var value = id.Value;
            if (RecordId.From(value) != id)
            {
                throw new CharacterAccessException(
                    "Production Access Record ID is not canonical.");
            }

            return value;
        }
        catch (CharacterAccessException)
        {
            throw;
        }
        catch (Exception exception) when (
            exception is InvalidOperationException or ArgumentException)
        {
            throw new CharacterAccessException(
                "Production Access contains an invalid Record ID.",
                exception);
        }
    }
}
