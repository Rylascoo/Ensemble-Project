using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.Tests.Patch0012;

internal static class Patch0012TestSupport
{
    internal static ValidatedFixture LoadMissingRaft()
    {
        var document = FixtureLoader.Load(
            Encoding.UTF8.GetBytes(ReadFixture("missing-raft-0.1.0.json")));
        var fixture = GenericE0FixtureValidator.Validate(document);
        MissingRaftContract.Validate(fixture);
        return fixture;
    }

    internal static ProductionState Genesis(
        ValidatedFixture? fixture = null,
        ImmutableArray<RecordId>? creatorLocks = null) =>
        ProductionState.Initialize(
            fixture ?? LoadMissingRaft(),
            creatorLocks ?? ImmutableArray<RecordId>.Empty);

    internal static Pipeline BuildPipeline(
        IReadOnlyList<Dictionary<string, object?>> mutations,
        IReadOnlyList<StateMutationDomain> autoApproveDomains,
        IReadOnlyList<StateAuthorityReviewChoice>? reviewChoices = null,
        string candidateText = "No.",
        ValidatedFixture? fixture = null,
        ImmutableArray<RecordId>? creatorLocks = null)
    {
        fixture ??= LoadMissingRaft();
        var context = Compose(fixture, MissingRaftContract.VossId);
        var candidate = PerformerCandidateContract.ParseJson(
            context,
            Encoding.UTF8.GetBytes(CandidateJson(candidateText)));
        var integrityInput = IntegrityCandidateInput.Bind(context, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            integrityInput,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var integrity = DeterministicIntegrityValidator.Validate(integrityInput, evidence);
        var source = StateInterpretationSource.Bind(context, candidate, integrity);
        var proposal = StateInterpretationContract.ParseJson(
            source,
            Encoding.UTF8.GetBytes(ProposalJson(mutations)));
        var snapshot = StateAuthoritySnapshot.Bind(
            fixture,
            creatorLocks ?? ImmutableArray<RecordId>.Empty);
        var authorityInput = StateAuthorityInput.Bind(snapshot, source, proposal);
        var policy = StateAuthorityPolicy.Create(autoApproveDomains.ToImmutableArray());
        var reviewSet = StateAuthorityReviewSet.Bind(
            authorityInput,
            (reviewChoices ?? Array.Empty<StateAuthorityReviewChoice>()).ToImmutableArray());
        var authority = DeterministicStateAuthority.Evaluate(
            authorityInput,
            policy,
            reviewSet);
        return new Pipeline(
            fixture,
            context,
            candidate,
            integrity,
            proposal,
            authority);
    }

    internal static E0Take AcceptedTake(
        Pipeline pipeline,
        string takeId = "TAKE-PATCH-0012") =>
        E0Take.Bind(
            TakeId.From(takeId),
            pipeline.Context,
            pipeline.Candidate,
            pipeline.Integrity,
            pipeline.Proposal,
            pipeline.Authority,
            E0TakeDisposition.Accepted);

    internal static E0Take Take(
        Pipeline pipeline,
        E0TakeDisposition disposition,
        string takeId) =>
        E0Take.Bind(
            TakeId.From(takeId),
            pipeline.Context,
            pipeline.Candidate,
            pipeline.Integrity,
            pipeline.Proposal,
            pipeline.Authority,
            disposition);

    internal static Dictionary<string, object?> Mutation(
        string domain,
        string operation,
        string? subjectCharacterId = null,
        string? targetCharacterId = null,
        string? existingRecordId = null,
        string? text = "Proposed state.",
        string[]? supportingRecordIds = null) =>
        new(StringComparer.Ordinal)
        {
            ["domain"] = domain,
            ["operation"] = operation,
            ["subjectCharacterId"] = subjectCharacterId,
            ["targetCharacterId"] = targetCharacterId,
            ["existingRecordId"] = existingRecordId,
            ["text"] = text,
            ["supportingRecordIds"] = supportingRecordIds ?? Array.Empty<string>()
        };

    internal static Dictionary<string, object?> PressureAdd(
        string text = "Pressure increases.",
        params string[] supports) =>
        Mutation(
            "pressure",
            "add",
            text: text,
            supportingRecordIds: supports);

    internal static Dictionary<string, object?> BeliefSupersede(
        string text = "Voss revises the working theory.",
        params string[] supports) =>
        Mutation(
            "characterBelief",
            "supersede",
            subjectCharacterId: MissingRaftContract.VossCharacterId,
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: text,
            supportingRecordIds: supports);

    internal static Dictionary<string, object?> BeliefDeactivate(
        params string[] supports) =>
        Mutation(
            "characterBelief",
            "deactivate",
            subjectCharacterId: MissingRaftContract.VossCharacterId,
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: null,
            supportingRecordIds: supports);

    internal static Dictionary<string, object?> InvalidBeliefSupersede() =>
        Mutation(
            "characterBelief",
            "supersede",
            subjectCharacterId: MissingRaftContract.VossCharacterId,
            existingRecordId: MissingRaftContract.ConVossId,
            text: "Invalid replacement.");

    internal static ContextPacket Compose(ValidatedFixture fixture, CharacterId subject)
    {
        var projection = CharacterBoundedAccessControl.Evaluate(fixture, subject).Projection;
        return DeterministicContextComposer.Compose(projection, subject).Packet;
    }

    internal static string ReadFixture(string fileName) =>
        File.ReadAllText(
            Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName),
            Encoding.UTF8);

    private static string CandidateJson(string text) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text },
            control = new
            {
                addressedCharacterIds = Array.Empty<string>(),
                nominatedCharacterId = (string?)null
            }
        });

    private static string ProposalJson(
        IReadOnlyList<Dictionary<string, object?>> mutations) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations
        });

    internal sealed record Pipeline(
        ValidatedFixture Fixture,
        ContextPacket Context,
        CandidatePerformance Candidate,
        IntegrityValidationEvaluation Integrity,
        StateInterpretationProposal Proposal,
        StateAuthorityEvaluation Authority);
}
