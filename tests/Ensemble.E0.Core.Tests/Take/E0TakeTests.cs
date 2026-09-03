using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Take;

[TestClass]
public sealed class E0TakeTests
{
    [TestMethod]
    public void ContractAndDispositionValues_AreExactAndFailClosed()
    {
        Assert.AreEqual("ensemble.e0.take.v1", E0TakeContracts.ContractVersion);
        Assert.AreEqual(0, (int)E0TakeDisposition.Unspecified);
        Assert.AreEqual(1, (int)E0TakeDisposition.Accepted);
        Assert.AreEqual(2, (int)E0TakeDisposition.Rejected);
        Assert.AreEqual(3, (int)E0TakeDisposition.Alternate);
        CollectionAssert.AreEqual(
            new[]
            {
                nameof(E0TakeDisposition.Unspecified),
                nameof(E0TakeDisposition.Accepted),
                nameof(E0TakeDisposition.Rejected),
                nameof(E0TakeDisposition.Alternate)
            },
            Enum.GetNames<E0TakeDisposition>());

        var pipeline = ApprovedPipeline();
        Assert.Throws<E0TakeException>(() => Bind(pipeline, default, E0TakeDisposition.Accepted));
        Assert.Throws<E0TakeException>(() => Bind(pipeline, TakeId.From("TAKE-DEFAULT"), default));
        Assert.Throws<E0TakeException>(() =>
            Bind(pipeline, TakeId.From("TAKE-UNDEFINED"), (E0TakeDisposition)int.MaxValue));
    }

    [TestMethod]
    public void Bind_RetainsExactPerformanceAndProposalAndFreshAuthorityReplay()
    {
        var pipeline = ApprovedPipeline();
        var takeId = TakeId.From("TAKE-EXACT");

        var take = Bind(pipeline, takeId, E0TakeDisposition.Accepted);

        Assert.AreEqual(E0TakeContracts.ContractVersion, take.ContractVersion);
        Assert.AreEqual(takeId, take.TakeId);
        Assert.AreEqual(E0TakeDisposition.Accepted, take.Disposition);
        Assert.AreSame(pipeline.Candidate, take.Performance);
        Assert.AreSame(pipeline.Proposal, take.InterpretationProposal);
        Assert.AreNotSame(pipeline.Authority, take.AuthorityEvaluation);
        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, take.AuthorityEvaluation.Status);
        CollectionAssert.AreEqual(
            AuthoritySignature(pipeline.Authority),
            AuthoritySignature(take.AuthorityEvaluation));
    }

    [TestMethod]
    public void Bind_DoesNotTrustSuppliedAuthorityStatusOrDecisions()
    {
        var pipeline = ApprovedPipeline();
        var fabricatedDecision = ConstructNonPublic<StateAuthorityDecision>(
            0,
            StateAuthorityDisposition.Rejected,
            ImmutableArray.Create(StateAuthorityReasonCode.PolicyReviewRequired));
        var fabricated = ConstructNonPublic<StateAuthorityEvaluation>(
            StateAuthorityEvaluationStatus.ReviewRequired,
            ImmutableArray.Create(fabricatedDecision),
            pipeline.Authority.Trace);

        var take = E0Take.Bind(
            TakeId.From("TAKE-REPLAY"),
            pipeline.Context,
            pipeline.Candidate,
            pipeline.Integrity,
            pipeline.Proposal,
            fabricated,
            E0TakeDisposition.Accepted);

        Assert.AreNotSame(fabricated, take.AuthorityEvaluation);
        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, take.AuthorityEvaluation.Status);
        Assert.AreEqual(StateAuthorityDisposition.Approved, take.AuthorityEvaluation.Decisions.Single().Disposition);
    }

    [TestMethod]
    public void Bind_MalformedSuppliedAuthorityTraceFailsSafely()
    {
        var pipeline = ApprovedPipeline();
        var malformed = ConstructNonPublic<StateAuthorityEvaluation>(
            StateAuthorityEvaluationStatus.Complete,
            ImmutableArray<StateAuthorityDecision>.Empty,
            null);

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-MALFORMED-TRACE"),
            pipeline.Context,
            pipeline.Candidate,
            pipeline.Integrity,
            pipeline.Proposal,
            malformed,
            E0TakeDisposition.Accepted));

        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void Bind_IntegrityRejectCannotCreateTake()
    {
        var canonical = ApprovedPipeline();
        var fixture = LoadMissingRaft();
        var marloweContext = Compose(fixture, MissingRaftContract.MarloweId);
        var marloweCandidate = ParseCandidate(marloweContext, "No.");
        var rejectedInput = IntegrityCandidateInput.Bind(canonical.Context, marloweCandidate);
        var rejectedIntegrity = DeterministicIntegrityValidator.Validate(rejectedInput, concernEvidence: null);

        Assert.AreEqual(IntegrityDisposition.Reject, rejectedIntegrity.Disposition);
        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-INTEGRITY-REJECT"),
            canonical.Context,
            marloweCandidate,
            rejectedIntegrity,
            canonical.Proposal,
            canonical.Authority,
            E0TakeDisposition.Rejected));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateInterpretationException));
    }

    [TestMethod]
    public void Bind_IntegrityRequestAnotherTakeCannotCreateTake()
    {
        var pipeline = ApprovedPipeline();
        var input = IntegrityCandidateInput.Bind(pipeline.Context, pipeline.Candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray.Create(IntegrityConcernKind.IndeterminateSemanticIntegrity));
        var requestAnother = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(IntegrityDisposition.RequestAnotherTake, requestAnother.Disposition);
        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-REQUEST-ANOTHER"),
            pipeline.Context,
            pipeline.Candidate,
            requestAnother,
            pipeline.Proposal,
            pipeline.Authority,
            E0TakeDisposition.Alternate));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateInterpretationException));
    }

    [TestMethod]
    public void Bind_MismatchedContextCandidateFailsInStateInterpretationDomain()
    {
        var pipeline = ApprovedPipeline();
        var fixture = LoadMissingRaft();
        var marloweContext = Compose(fixture, MissingRaftContract.MarloweId);
        var marloweCandidate = ParseCandidate(marloweContext, "Marlowe output.");
        var marloweIntegrity = AcceptedIntegrity(marloweContext, marloweCandidate);

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-CONTEXT-MISMATCH"),
            pipeline.Context,
            marloweCandidate,
            marloweIntegrity,
            pipeline.Proposal,
            pipeline.Authority,
            E0TakeDisposition.Accepted));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateInterpretationException));
    }

    [TestMethod]
    public void Bind_MismatchedIntegrityEvaluationFailsInStateInterpretationDomain()
    {
        var pipeline = ApprovedPipeline(candidateText: "First candidate.");
        var secondCandidate = ParseCandidate(pipeline.Context, "Second candidate.");
        var secondIntegrity = AcceptedIntegrity(pipeline.Context, secondCandidate);

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-INTEGRITY-MISMATCH"),
            pipeline.Context,
            pipeline.Candidate,
            secondIntegrity,
            pipeline.Proposal,
            pipeline.Authority,
            E0TakeDisposition.Accepted));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateInterpretationException));
    }

    [TestMethod]
    public void Bind_ProposalCandidateIdentityMismatchFailsInStateAuthorityDomain()
    {
        var first = ApprovedPipeline(candidateText: "First candidate.");
        var second = ApprovedPipeline(candidateText: "Second candidate.");

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-PROPOSAL-CANDIDATE-MISMATCH"),
            second.Context,
            second.Candidate,
            second.Integrity,
            first.Proposal,
            first.Authority,
            E0TakeDisposition.Accepted));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateAuthorityException));
    }

    [TestMethod]
    public void Bind_ProposalSceneMismatchFailsInStateAuthorityDomain()
    {
        var pipeline = ApprovedPipeline();
        var mismatchedProposal = ConstructNonPublic<StateInterpretationProposal>(
            pipeline.Proposal.ContractVersion,
            pipeline.Proposal.CandidateContentIdentityContract,
            pipeline.Proposal.CandidateContentHash,
            SceneId.From("OTHER-SCENE"),
            pipeline.Proposal.Mutations);

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-SCENE-MISMATCH"),
            pipeline.Context,
            pipeline.Candidate,
            pipeline.Integrity,
            mismatchedProposal,
            pipeline.Authority,
            E0TakeDisposition.Accepted));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateAuthorityException));
    }

    [TestMethod]
    public void Bind_MismatchedReviewSetProposalIdentityFailsDuringFreshReplay()
    {
        var first = ApprovedPipeline(mutationText: "First proposal.");
        var second = ApprovedPipeline(mutationText: "Second proposal.");

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-REVIEWSET-MISMATCH"),
            first.Context,
            first.Candidate,
            first.Integrity,
            first.Proposal,
            second.Authority,
            E0TakeDisposition.Accepted));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateAuthorityException));
    }

    [TestMethod]
    public void Bind_FreshReviewRequiredOrRequiresReviewDecisionCannotCreateTake()
    {
        var pipeline = Pipeline(
            "No.",
            new[]
            {
                Mutation(
                    "relationship",
                    "add",
                    subjectCharacterId: "VOSS",
                    targetCharacterId: "WREN",
                    text: "Voss trusts Wren more.")
            },
            Array.Empty<StateMutationDomain>(),
            Array.Empty<StateAuthorityReviewChoice>());

        Assert.AreEqual(StateAuthorityEvaluationStatus.ReviewRequired, pipeline.Authority.Status);
        Assert.IsTrue(pipeline.Authority.Decisions.Any(
            decision => decision.Disposition == StateAuthorityDisposition.RequiresReview));

        Assert.Throws<E0TakeException>(() =>
            Bind(pipeline, TakeId.From("TAKE-REVIEW-REQUIRED"), E0TakeDisposition.Accepted));
    }

    [TestMethod]
    [DataRow("empty", 1)]
    [DataRow("approved", 1)]
    [DataRow("mixed", 1)]
    [DataRow("rejected", 1)]
    [DataRow("approved", 2)]
    [DataRow("mixed", 2)]
    [DataRow("rejected", 2)]
    [DataRow("approved", 3)]
    [DataRow("mixed", 3)]
    [DataRow("rejected", 3)]
    public void Bind_AllTerminalConsequenceSetsAreIndependentFromTakeDisposition(
        string consequenceSet,
        int dispositionValue)
    {
        var pipeline = consequenceSet switch
        {
            "empty" => EmptyPipeline(),
            "approved" => ApprovedPipeline(),
            "mixed" => MixedPipeline(),
            "rejected" => RejectedPipeline(),
            _ => throw new InvalidOperationException("Unsupported Take test consequence set.")
        };
        var disposition = (E0TakeDisposition)dispositionValue;

        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, pipeline.Authority.Status);
        Assert.IsFalse(pipeline.Authority.Decisions.Any(
            decision => decision.Disposition == StateAuthorityDisposition.RequiresReview));

        var take = Bind(
            pipeline,
            TakeId.From($"TAKE-{consequenceSet.ToUpperInvariant()}-{dispositionValue}"),
            disposition);

        Assert.AreEqual(disposition, take.Disposition);
        CollectionAssert.AreEqual(
            AuthoritySignature(pipeline.Authority),
            AuthoritySignature(take.AuthorityEvaluation));
    }

    [TestMethod]
    public void Bind_DistinctTakeIdsMayCarrySemanticallyIdenticalPackages()
    {
        var pipeline = ApprovedPipeline();
        var first = Bind(pipeline, TakeId.From("TAKE-A"), E0TakeDisposition.Accepted);
        var second = Bind(pipeline, TakeId.From("TAKE-B"), E0TakeDisposition.Accepted);

        Assert.AreNotEqual(first.TakeId, second.TakeId);
        Assert.AreSame(first.Performance, second.Performance);
        Assert.AreSame(first.InterpretationProposal, second.InterpretationProposal);
        CollectionAssert.AreEqual(
            AuthoritySignature(first.AuthorityEvaluation),
            AuthoritySignature(second.AuthorityEvaluation));
    }

    [TestMethod]
    public void Bind_RepeatedIdenticalInputsProduceEquivalentDeterministicTakeSemantics()
    {
        var pipeline = MixedPipeline();
        var takeId = TakeId.From("TAKE-REPEAT");

        var first = Bind(pipeline, takeId, E0TakeDisposition.Alternate);
        var second = Bind(pipeline, takeId, E0TakeDisposition.Alternate);

        Assert.AreEqual(first.TakeId, second.TakeId);
        Assert.AreEqual(first.Disposition, second.Disposition);
        Assert.AreSame(first.Performance, second.Performance);
        Assert.AreSame(first.InterpretationProposal, second.InterpretationProposal);
        CollectionAssert.AreEqual(
            AuthoritySignature(first.AuthorityEvaluation),
            AuthoritySignature(second.AuthorityEvaluation));
    }

    [TestMethod]
    public void Bind_NullRequiredInputsFailWithoutCreatingFallbackTake()
    {
        var pipeline = ApprovedPipeline();
        var takeId = TakeId.From("TAKE-NULLS");

        Assert.Throws<E0TakeException>(() => E0Take.Bind(
            takeId, null!, pipeline.Candidate, pipeline.Integrity, pipeline.Proposal, pipeline.Authority, E0TakeDisposition.Accepted));
        Assert.Throws<E0TakeException>(() => E0Take.Bind(
            takeId, pipeline.Context, null!, pipeline.Integrity, pipeline.Proposal, pipeline.Authority, E0TakeDisposition.Accepted));
        Assert.Throws<E0TakeException>(() => E0Take.Bind(
            takeId, pipeline.Context, pipeline.Candidate, null!, pipeline.Proposal, pipeline.Authority, E0TakeDisposition.Accepted));
        Assert.Throws<E0TakeException>(() => E0Take.Bind(
            takeId, pipeline.Context, pipeline.Candidate, pipeline.Integrity, null!, pipeline.Authority, E0TakeDisposition.Accepted));
        Assert.Throws<E0TakeException>(() => E0Take.Bind(
            takeId, pipeline.Context, pipeline.Candidate, pipeline.Integrity, pipeline.Proposal, null!, E0TakeDisposition.Accepted));
    }

    [TestMethod]
    public void Bind_UninitializedTakeIdNormalizesWithoutInnerRuntimeException()
    {
        var pipeline = ApprovedPipeline();

        var exception = Assert.Throws<E0TakeException>(() =>
            Bind(pipeline, default, E0TakeDisposition.Accepted));

        Assert.IsNull(exception.InnerException);
        Assert.AreEqual(0, exception.Data.Count);
    }

    [TestMethod]
    public void Bind_ExpectedUpstreamFailuresRetainSanitizedDomainException()
    {
        var first = ApprovedPipeline(candidateText: "First candidate.");
        var second = ApprovedPipeline(candidateText: "Second candidate.");

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-UPSTREAM-INNER"),
            second.Context,
            second.Candidate,
            second.Integrity,
            first.Proposal,
            first.Authority,
            E0TakeDisposition.Accepted));

        Assert.IsInstanceOfType(exception.InnerException, typeof(StateAuthorityException));
        Assert.AreEqual(0, exception.Data.Count);
        Assert.AreEqual(0, exception.InnerException!.Data.Count);
    }

    [TestMethod]
    public void Bind_ExceptionRepresentationDoesNotLeakCandidateContextOrMutationProse()
    {
        const string candidateSecret = "TOP-SECRET-CANDIDATE-LEAK-ALPHA";
        const string mutationSecret = "TOP-SECRET-MUTATION-LEAK-BETA";
        const string contextSentence = "The group has limited provisions.";
        var first = ApprovedPipeline(candidateText: candidateSecret, mutationText: mutationSecret);
        var second = ApprovedPipeline(candidateText: "Different candidate.", mutationText: "Different proposal.");

        var exception = Assert.Throws<E0TakeException>(() => E0Take.Bind(
            TakeId.From("TAKE-SANITIZE"),
            second.Context,
            second.Candidate,
            second.Integrity,
            first.Proposal,
            first.Authority,
            E0TakeDisposition.Accepted));
        var representation = exception.ToString();

        Assert.IsFalse(representation.Contains(candidateSecret, StringComparison.Ordinal));
        Assert.IsFalse(representation.Contains(mutationSecret, StringComparison.Ordinal));
        Assert.IsFalse(representation.Contains(contextSentence, StringComparison.Ordinal));
        Assert.AreEqual(0, exception.Data.Count);
        if (exception.InnerException is not null)
        {
            Assert.AreEqual(0, exception.InnerException.Data.Count);
        }
    }

    private static E0Take Bind(
        TakePipeline pipeline,
        TakeId takeId,
        E0TakeDisposition disposition) =>
        E0Take.Bind(
            takeId,
            pipeline.Context,
            pipeline.Candidate,
            pipeline.Integrity,
            pipeline.Proposal,
            pipeline.Authority,
            disposition);

    private static TakePipeline EmptyPipeline() =>
        Pipeline(
            "No.",
            Array.Empty<Dictionary<string, object?>>(),
            Array.Empty<StateMutationDomain>(),
            Array.Empty<StateAuthorityReviewChoice>());

    private static TakePipeline ApprovedPipeline(
        string candidateText = "No.",
        string mutationText = "Pressure increases.") =>
        Pipeline(
            candidateText,
            new[] { Mutation("pressure", "add", text: mutationText) },
            new[] { StateMutationDomain.Pressure },
            Array.Empty<StateAuthorityReviewChoice>());

    private static TakePipeline RejectedPipeline() =>
        Pipeline(
            "No.",
            new[]
            {
                Mutation(
                    "characterBelief",
                    "supersede",
                    subjectCharacterId: "VOSS",
                    existingRecordId: MissingRaftContract.ConVossId,
                    text: "Invalid replacement.")
            },
            new[] { StateMutationDomain.CharacterBelief },
            Array.Empty<StateAuthorityReviewChoice>());

    private static TakePipeline MixedPipeline() =>
        Pipeline(
            "No.",
            new[]
            {
                Mutation("pressure", "add", text: "Pressure increases."),
                Mutation(
                    "characterBelief",
                    "supersede",
                    subjectCharacterId: "VOSS",
                    existingRecordId: MissingRaftContract.ConVossId,
                    text: "Invalid replacement.")
            },
            new[] { StateMutationDomain.Pressure, StateMutationDomain.CharacterBelief },
            Array.Empty<StateAuthorityReviewChoice>());

    private static TakePipeline Pipeline(
        string candidateText,
        IReadOnlyList<Dictionary<string, object?>> mutations,
        IReadOnlyList<StateMutationDomain> autoApproveDomains,
        IReadOnlyList<StateAuthorityReviewChoice> reviewChoices)
    {
        var fixture = LoadMissingRaft();
        var context = Compose(fixture, MissingRaftContract.VossId);
        var candidate = ParseCandidate(context, candidateText);
        var integrity = AcceptedIntegrity(context, candidate);
        var source = StateInterpretationSource.Bind(context, candidate, integrity);
        var proposal = StateInterpretationContract.ParseJson(
            source,
            Encoding.UTF8.GetBytes(ProposalJson(mutations)));
        var snapshot = StateAuthoritySnapshot.Bind(fixture, ImmutableArray<RecordId>.Empty);
        var input = StateAuthorityInput.Bind(snapshot, source, proposal);
        var policy = StateAuthorityPolicy.Create(autoApproveDomains.ToImmutableArray());
        var reviewSet = StateAuthorityReviewSet.Bind(input, reviewChoices.ToImmutableArray());
        var authority = DeterministicStateAuthority.Evaluate(input, policy, reviewSet);
        return new TakePipeline(context, candidate, integrity, proposal, authority);
    }

    private static IntegrityValidationEvaluation AcceptedIntegrity(
        ContextPacket context,
        CandidatePerformance candidate)
    {
        var input = IntegrityCandidateInput.Bind(context, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);
        return DeterministicIntegrityValidator.Validate(input, evidence);
    }

    private static CandidatePerformance ParseCandidate(
        ContextPacket packet,
        string text,
        string[]? addressedCharacterIds = null,
        string? nominatedCharacterId = null) =>
        PerformerCandidateContract.ParseJson(
            packet,
            Encoding.UTF8.GetBytes(CandidateJson(text, addressedCharacterIds, nominatedCharacterId)));

    private static string CandidateJson(
        string text,
        string[]? addressedCharacterIds = null,
        string? nominatedCharacterId = null) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text },
            control = new
            {
                addressedCharacterIds = addressedCharacterIds ?? Array.Empty<string>(),
                nominatedCharacterId
            }
        });

    private static Dictionary<string, object?> Mutation(
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

    private static string ProposalJson(IReadOnlyList<Dictionary<string, object?>> mutations) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations
        });

    private static string[] AuthoritySignature(StateAuthorityEvaluation evaluation) =>
        evaluation.Decisions
            .Select(decision =>
                $"{decision.MutationIndex}|{decision.Disposition}|{string.Join(',', decision.Reasons)}")
            .Prepend(evaluation.Status.ToString())
            .ToArray();

    private static T ConstructNonPublic<T>(params object?[] args)
        where T : class
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(candidate => candidate.GetParameters().Length == args.Length);
        return (T)constructor.Invoke(args);
    }

    private static ContextPacket Compose(ValidatedFixture fixture, CharacterId subject)
    {
        var projection = CharacterBoundedAccessControl.Evaluate(fixture, subject).Projection;
        return DeterministicContextComposer.Compose(projection, subject).Packet;
    }

    private static ValidatedFixture LoadMissingRaft()
    {
        var document = FixtureLoader.Load(
            Encoding.UTF8.GetBytes(ReadFixture("missing-raft-0.1.0.json")));
        var fixture = GenericE0FixtureValidator.Validate(document);
        MissingRaftContract.Validate(fixture);
        return fixture;
    }

    private static string ReadFixture(string fileName) =>
        File.ReadAllText(
            Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName),
            Encoding.UTF8);

    private sealed record TakePipeline(
        ContextPacket Context,
        CandidatePerformance Candidate,
        IntegrityValidationEvaluation Integrity,
        StateInterpretationProposal Proposal,
        StateAuthorityEvaluation Authority);
}
