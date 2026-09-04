using System.Collections.Immutable;
using System.Reflection;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class ProductionAccessStructuralTests
{
    [TestMethod]
    public void ProductionAccess_RejectsMalformedRosterAndCharacterSetMismatch()
    {
        var state = Patch0012TestSupport.Genesis();
        var shortRoster = CloneState(
            state,
            roster: ImmutableArray.Create(
                MissingRaftContract.MarloweId,
                MissingRaftContract.VossId));
        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(
                shortRoster,
                MissingRaftContract.VossId));

        var characters = state.Characters.ToBuilder();
        characters[0] = ConstructNonPublic<ProductionCharacter>(
            MissingRaftContract.WrenId,
            "Wren");
        var mismatchedCharacters = CloneState(
            state,
            characters: characters.ToImmutable());
        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(
                mismatchedCharacters,
                MissingRaftContract.VossId));
    }

    [TestMethod]
    public void ProductionAccess_RejectsInvalidDisplayNameAndRecordText()
    {
        var state = Patch0012TestSupport.Genesis();
        var characters = state.Characters.ToBuilder();
        characters[1] = ConstructNonPublic<ProductionCharacter>(
            MissingRaftContract.VossId,
            "\rInvalid");
        var invalidDisplay = CloneState(state, characters: characters.ToImmutable());
        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(
                invalidDisplay,
                MissingRaftContract.VossId));

        var invalidPressure = ConstructNonPublic<GlobalProductionRecord>(
            RecordId.From("PRESSURE-INVALID-TEXT"),
            ProductionRecordDomain.Pressure,
            ProductionRecordLifecycle.Active,
            ProductionRecordProtection.None,
            string.Empty,
            ImmutableArray<RecordId>.Empty);
        var invalidText = CloneState(
            state,
            records: ReplaceOrAppend(state.Records, invalidPressure));
        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(
                invalidText,
                MissingRaftContract.VossId));
    }

    [TestMethod]
    public void ProductionAccess_RejectsSubtypeDomainMismatchEvenWhenInactive()
    {
        var state = Patch0012TestSupport.Genesis();
        var malformed = ConstructNonPublic<GlobalProductionRecord>(
            RecordId.From("MALFORMED-INACTIVE-CLAIM"),
            ProductionRecordDomain.CharacterClaim,
            ProductionRecordLifecycle.Inactive,
            ProductionRecordProtection.None,
            "Malformed shape.",
            ImmutableArray<RecordId>.Empty);
        var malformedState = CloneState(
            state,
            records: ReplaceOrAppend(state.Records, malformed));

        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(
                malformedState,
                MissingRaftContract.VossId));
    }

    [TestMethod]
    public void ProductionAccess_RejectsMalformedRelationshipEndpoints()
    {
        var state = Patch0012TestSupport.Genesis();
        var malformed = ConstructNonPublic<RelationshipProductionRecord>(
            RecordId.From("REL-MALFORMED-SELF"),
            MissingRaftContract.VossId,
            MissingRaftContract.VossId,
            ProductionRecordLifecycle.Active,
            ProductionRecordProtection.None,
            "Invalid self relationship.",
            ImmutableArray<RecordId>.Empty);
        var malformedState = CloneState(
            state,
            records: ReplaceOrAppend(state.Records, malformed));

        Assert.Throws<CharacterAccessException>(() =>
            CharacterBoundedAccessControl.Evaluate(
                malformedState,
                MissingRaftContract.VossId));
    }

    [TestMethod]
    public void InactiveCharacterClaim_UsesInactiveReasonPrecedence()
    {
        var state = Patch0012TestSupport.Genesis();
        var claimId = RecordId.From("CLAIM-INACTIVE-PATCH-0014");
        var inactiveClaim = ConstructNonPublic<CharacterProductionRecord>(
            claimId,
            ProductionRecordDomain.CharacterClaim,
            MissingRaftContract.VossId,
            ProductionRecordLifecycle.Inactive,
            ProductionRecordProtection.None,
            "An inactive retained claim.",
            ImmutableArray<RecordId>.Empty);
        var source = CloneState(
            state,
            records: ReplaceOrAppend(state.Records, inactiveClaim));

        var evaluation = CharacterBoundedAccessControl.Evaluate(
            source,
            MissingRaftContract.VossId);
        var decision = evaluation.Decisions.Single(value => value.RecordId == claimId);

        Assert.AreEqual(AccessDisposition.Deny, decision.Disposition);
        Assert.AreEqual(AccessReason.InactiveRecordExcluded, decision.Reason);
    }

    [TestMethod]
    public void AccessReason_AppendsOnlyApprovedPatch0014Values()
    {
        CollectionAssert.AreEqual(
            new[]
            {
                "OwnedBySubject",
                "SharedSceneState",
                "PublicPressure",
                "ProductionAuthorityExcluded",
                "OwnedByOtherCharacterExcluded",
                "InactiveRecordExcluded",
                "CharacterClaimDisclosureDeferred"
            },
            Enum.GetNames<AccessReason>());
    }

    private static ImmutableArray<ProductionRecord> ReplaceOrAppend(
        ImmutableArray<ProductionRecord> records,
        ProductionRecord record) =>
        records
            .Where(existing => existing.RecordId != record.RecordId)
            .Append(record)
            .OrderBy(existing => existing.RecordId.Value, StringComparer.Ordinal)
            .ToImmutableArray();

    private static ProductionState CloneState(
        ProductionState state,
        ImmutableArray<ProductionCharacter>? characters = null,
        ImmutableArray<CharacterId>? roster = null,
        ImmutableArray<ProductionRecord>? records = null)
    {
        var assembly = typeof(ProductionState).Assembly;
        var projectionType = assembly.GetType(
            "Ensemble.E0.Core.Production.ProductionStateProjection",
            throwOnError: true)!;
        var projectionConstructor = projectionType.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Single(constructor => constructor.GetParameters().Length == 9);
        var projection = projectionConstructor.Invoke(new object?[]
        {
            state.OriginFixtureId,
            state.OriginFixtureFamilyId,
            state.OriginFixtureVersion,
            state.OriginFixtureHash,
            state.SceneId,
            characters ?? state.Characters,
            roster ?? state.RosterCharacterIds,
            state.CurrentOpportunityCharacterId,
            records ?? state.Records
        });

        var effectiveCommits = typeof(ProductionState).GetField(
            "_effectiveCommitIds",
            BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(state)!;
        var committedTakes = typeof(ProductionState).GetField(
            "_committedTakeIds",
            BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(state)!;
        var stateConstructor = typeof(ProductionState).GetConstructors(
                BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        return (ProductionState)stateConstructor.Invoke(new[]
        {
            projection,
            (object)state.StateHash,
            effectiveCommits,
            committedTakes
        });
    }

    private static T ConstructNonPublic<T>(params object?[] arguments)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == arguments.Length);
        return (T)constructor.Invoke(arguments);
    }
}
