using System.Collections.Immutable;
using System.Reflection;
using System.Security.Cryptography;
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.StateAuthority;

[TestClass]
public sealed class StateAuthorityContractAuditTests
{
    private const string ExpectedVossStructuredHash =
        "bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b";
    private const string ExpectedVossRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const string ExpectedOracleCandidateHash =
        "18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2";

    [TestMethod]
    public void SnapshotSurfaceAndInitialFixtureCoverage_MatchApprovedContract()
    {
        var fixture = LoadMissingRaft();
        var snapshot = StateAuthoritySnapshot.Bind(fixture, ImmutableArray<RecordId>.Empty);

        CollectionAssert.AreEquivalent(
            new[] { "SceneId", "RosterCharacterIds", "Records" },
            PublicPropertyNames(typeof(StateAuthoritySnapshot)).ToArray());
        Assert.AreEqual(0, typeof(StateAuthoritySnapshot)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);

        var expectedIds = AllFixtureRecordIds(fixture)
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();
        var actualIds = snapshot.Records
            .Select(record => record.RecordId.Value)
            .ToArray();
        CollectionAssert.AreEqual(expectedIds, actualIds);
        Assert.IsTrue(snapshot.Records.All(
            record => record.Lifecycle == StateAuthorityRecordLifecycle.Active));

        var world = snapshot.Records.Single(
            record => record.RecordId == RecordId.From(MissingRaftContract.WorldCurrentStrengthenedId));
        Assert.IsInstanceOfType(world, typeof(GlobalStateAuthorityRecordDescriptor));
        Assert.AreEqual(StateAuthorityRecordDomain.WorldState, world.RecordDomain);

        var belief = (CharacterStateAuthorityRecordDescriptor)snapshot.Records.Single(
            record => record.RecordId == RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId));
        Assert.AreEqual(StateAuthorityRecordDomain.CharacterBelief, belief.RecordDomain);
        Assert.AreEqual(MissingRaftContract.VossId, belief.SubjectCharacterId);

        var relationship = (RelationshipStateAuthorityRecordDescriptor)snapshot.Records.Single(
            record => record.RecordId == RecordId.From(MissingRaftContract.RelVossWrenId));
        Assert.AreEqual(MissingRaftContract.VossId, relationship.SubjectCharacterId);
        Assert.AreEqual(MissingRaftContract.WrenId, relationship.TargetCharacterId);
    }

    [TestMethod]
    public void SystemImmutableCreatorLockOverlay_RemainsSystemImmutable()
    {
        var fixture = LoadMissingRaft();
        var snapshot = StateAuthoritySnapshot.Bind(
            fixture,
            ImmutableArray.Create(
                RecordId.From(MissingRaftContract.HtMarloweReleasedRaftId),
                RecordId.From(MissingRaftContract.ConVossId),
                RecordId.From(MissingRaftContract.ObsWrenMarloweReturnId)));

        foreach (var recordId in new[]
                 {
                     MissingRaftContract.HtMarloweReleasedRaftId,
                     MissingRaftContract.ConVossId,
                     MissingRaftContract.ObsWrenMarloweReturnId
                 })
        {
            Assert.AreEqual(
                StateAuthorityRecordProtection.SystemImmutable,
                snapshot.Records.Single(record => record.RecordId == RecordId.From(recordId)).Protection);
        }
    }

    [TestMethod]
    public void ProposalIdentity_IsSemanticAndSensitiveToSupportMutationOrderAndCandidate()
    {
        var semanticA = AuthorityPipelineFromJson(
            "{\"schemaVersion\":\"ensemble.e0.state-interpreter.proposal-json.v1\",\"mutations\":[{\"domain\":\"characterBelief\",\"operation\":\"add\",\"subjectCharacterId\":\"VOSS\",\"targetCharacterId\":null,\"existingRecordId\":null,\"text\":\"Same.\",\"supportingRecordIds\":[\"GOAL-VOSS\",\"BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE\"]}]}" );
        var semanticB = AuthorityPipelineFromJson(
            "{ \"mutations\" : [ { \"supportingRecordIds\" : [\"BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE\",\"GOAL-VOSS\"], \"text\" : \"Same.\", \"existingRecordId\" : null, \"targetCharacterId\" : null, \"subjectCharacterId\" : \"VOSS\", \"operation\" : \"add\", \"domain\" : \"characterBelief\" } ], \"schemaVersion\" : \"ensemble.e0.state-interpreter.proposal-json.v1\" }" );
        Assert.AreEqual(semanticA.Input.ProposalContentHash, semanticB.Input.ProposalContentHash);

        var supportChanged = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Same.",
            supportingRecordIds: new[] { MissingRaftContract.GoalVossId }));
        Assert.AreNotEqual(semanticA.Input.ProposalContentHash, supportChanged.Input.ProposalContentHash);

        var firstOrder = AuthorityPipeline(
            ValidAddMutation("characterBelief"),
            ValidAddMutation("pressure"));
        var secondOrder = AuthorityPipeline(
            ValidAddMutation("pressure"),
            ValidAddMutation("characterBelief"));
        Assert.AreNotEqual(firstOrder.Input.ProposalContentHash, secondOrder.Input.ProposalContentHash);
        CollectionAssert.AreEqual(
            new[] { 0, 1 },
            firstOrder.Input.Mutations.Select(mutation => mutation.MutationIndex).ToArray());

        var firstCandidate = AuthorityPipelineForCandidate("First candidate.", ValidAddMutation("pressure"));
        var secondCandidate = AuthorityPipelineForCandidate("Second candidate.", ValidAddMutation("pressure"));
        Assert.AreNotEqual(firstCandidate.Input.ProposalContentHash, secondCandidate.Input.ProposalContentHash);
    }

    [TestMethod]
    public void InternalMutationConstructors_EnforceApprovedDomainFamiliesAndAppendOnlyTransitions()
    {
        var addPipeline = AuthorityPipeline(ValidAddMutation("pressure"));
        var addTransition = addPipeline.Input.Mutations.Single().Transition;
        var supports = ImmutableArray<RecordId>.Empty;

        AssertInternalConstructorThrows(
            typeof(GlobalStateAuthorityMutationInput),
            0,
            StateMutationDomain.CharacterBelief,
            addTransition,
            supports);

        AssertInternalConstructorThrows(
            typeof(CharacterStateAuthorityMutationInput),
            0,
            StateMutationDomain.Pressure,
            MissingRaftContract.VossId,
            addTransition,
            supports);

        var supersedePipeline = AuthorityPipeline(Mutation(
            "characterBelief",
            "supersede",
            subjectCharacterId: "VOSS",
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: "Replacement."));
        var supersedeTransition = supersedePipeline.Input.Mutations.Single().Transition;

        AssertInternalConstructorThrows(
            typeof(CharacterStateAuthorityMutationInput),
            0,
            StateMutationDomain.CharacterKnowledge,
            MissingRaftContract.VossId,
            supersedeTransition,
            supports);
        AssertInternalConstructorThrows(
            typeof(CharacterStateAuthorityMutationInput),
            0,
            StateMutationDomain.CharacterMemory,
            MissingRaftContract.VossId,
            supersedeTransition,
            supports);
        AssertInternalConstructorThrows(
            typeof(CharacterStateAuthorityMutationInput),
            0,
            StateMutationDomain.CharacterClaim,
            MissingRaftContract.VossId,
            supersedeTransition,
            supports);
    }

    [TestMethod]
    public void PolicyAndReviewPublicSurfaces_ContainNoConfidenceProviderOrUiMode()
    {
        CollectionAssert.AreEquivalent(
            new[] { "ContractVersion", "AutoApproveDomains" },
            PublicPropertyNames(typeof(StateAuthorityPolicy)).ToArray());
        CollectionAssert.AreEquivalent(
            new[] { "ContractVersion", "ProposalContentIdentityContract", "ProposalContentHash", "Choices" },
            PublicPropertyNames(typeof(StateAuthorityReviewSet)).ToArray());

        foreach (var type in new[] { typeof(StateAuthorityPolicy), typeof(StateAuthorityReviewSet) })
        {
            var properties = PublicPropertyNames(type);
            foreach (var forbidden in new[] { "Confidence", "Provider", "Model", "Mode", "Rationale", "Text" })
            {
                Assert.IsFalse(properties.Contains(forbidden), $"{type.Name}.{forbidden}");
            }
        }

        Assert.Throws<StateAuthorityException>(() => StateAuthorityReviewChoice.Approve(-1));
        Assert.Throws<StateAuthorityException>(() => StateAuthorityReviewChoice.Reject(-1));
    }

    [TestMethod]
    public void SystemImmutableTargetRejects_ButProtectedRecordCanRemainSupport()
    {
        var targetPipeline = AuthorityPipeline(Mutation(
            "characterGoal",
            "supersede",
            subjectCharacterId: "VOSS",
            existingRecordId: MissingRaftContract.ConVossId,
            text: "Not allowed."));
        var rejected = Evaluate(
            targetPipeline.Input,
            Policy(StateMutationDomain.CharacterGoal),
            NoReview(targetPipeline.Input)).Decisions.Single();
        CollectionAssert.Contains(
            rejected.Reasons.ToArray(),
            StateAuthorityReasonCode.ExistingRecordProtected);

        var supportPipeline = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Supported proposal.",
            supportingRecordIds: new[] { MissingRaftContract.HtMarloweReleasedRaftId }));
        var approved = Evaluate(
            supportPipeline.Input,
            Policy(StateMutationDomain.CharacterBelief),
            NoReview(supportPipeline.Input)).Decisions.Single();
        Assert.AreEqual(StateAuthorityDisposition.Approved, approved.Disposition);
    }

    [TestMethod]
    public void ExplicitReviewReasonComposition_CoversMandatoryPolicyReviewAndAutoApprovePaths()
    {
        var mandatory = AuthorityPipeline(ValidAddMutation("relationship"));
        AssertDecision(
            Evaluate(
                mandatory.Input,
                Policy(),
                Review(mandatory.Input, StateAuthorityReviewChoice.Reject(0))).Decisions.Single(),
            StateAuthorityDisposition.Rejected,
            StateAuthorityReasonCode.MandatoryReview,
            StateAuthorityReasonCode.ExplicitReviewRejected);

        var policyReview = AuthorityPipeline(ValidAddMutation("characterBelief"));
        AssertDecision(
            Evaluate(
                policyReview.Input,
                Policy(),
                Review(policyReview.Input, StateAuthorityReviewChoice.Reject(0))).Decisions.Single(),
            StateAuthorityDisposition.Rejected,
            StateAuthorityReasonCode.PolicyReviewRequired,
            StateAuthorityReasonCode.ExplicitReviewRejected);

        var autoApproved = AuthorityPipeline(ValidAddMutation("pressure"));
        AssertDecision(
            Evaluate(
                autoApproved.Input,
                Policy(StateMutationDomain.Pressure),
                Review(autoApproved.Input, StateAuthorityReviewChoice.Approve(0))).Decisions.Single(),
            StateAuthorityDisposition.Approved,
            StateAuthorityReasonCode.PolicyAutoApproved,
            StateAuthorityReasonCode.ExplicitReviewApproved);
    }

    [TestMethod]
    public void ReviewChoiceOrdering_DoesNotAffectEvaluationAndPendingDominatesStatus()
    {
        var pipeline = AuthorityPipeline(
            ValidAddMutation("characterBelief"),
            ValidAddMutation("relationship"),
            ValidAddMutation("pressure"));
        var policy = Policy(StateMutationDomain.CharacterBelief, StateMutationDomain.Pressure);
        var first = Review(
            pipeline.Input,
            StateAuthorityReviewChoice.Approve(1),
            StateAuthorityReviewChoice.Reject(2));
        var second = Review(
            pipeline.Input,
            StateAuthorityReviewChoice.Reject(2),
            StateAuthorityReviewChoice.Approve(1));

        Assert.AreEqual(
            EvaluationSignature(Evaluate(pipeline.Input, policy, first)),
            EvaluationSignature(Evaluate(pipeline.Input, policy, second)));

        var pending = AuthorityPipeline(
            ValidAddMutation("characterBelief"),
            ValidAddMutation("relationship"));
        var mixed = Evaluate(
            pending.Input,
            Policy(StateMutationDomain.CharacterBelief),
            NoReview(pending.Input));
        Assert.AreEqual(StateAuthorityEvaluationStatus.ReviewRequired, mixed.Status);
        Assert.AreEqual(StateAuthorityDisposition.Approved, mixed.Decisions[0].Disposition);
        Assert.AreEqual(StateAuthorityDisposition.RequiresReview, mixed.Decisions[1].Disposition);
    }

    [TestMethod]
    public void TraceIsExactProseFreeInputPolicyReviewSetAndClaimApprovalDoesNotPromoteDomain()
    {
        var pipeline = AuthorityPipeline(ValidAddMutation("characterClaim"));
        var policy = Policy(StateMutationDomain.CharacterClaim);
        var review = NoReview(pipeline.Input);
        var evaluation = Evaluate(pipeline.Input, policy, review);

        Assert.AreSame(pipeline.Input, evaluation.Trace.Input);
        Assert.AreSame(policy, evaluation.Trace.Policy);
        Assert.AreSame(review, evaluation.Trace.ReviewSet);
        Assert.AreEqual(1, evaluation.Decisions.Length);
        Assert.AreEqual(StateMutationDomain.CharacterClaim, pipeline.Input.Mutations.Single().Domain);
        Assert.AreEqual(StateAuthorityDisposition.Approved, evaluation.Decisions.Single().Disposition);
        Assert.AreEqual(1, pipeline.Input.Mutations.Length);
    }

    [TestMethod]
    public void FrozenPatch0008ContextAndFixtureRegressionIdentitiesRemainUnchanged()
    {
        var fixture = LoadMissingRaft();
        var packet = Compose(fixture, MissingRaftContract.VossId);
        var candidate = PerformerCandidateContract.ParseJson(
            packet,
            Encoding.UTF8.GetBytes(CandidateJson(
                "Wren?",
                new[] { "WREN", "MARLOWE" },
                "WREN")));
        var candidateInput = IntegrityCandidateInput.Bind(packet, candidate);
        var ecj1 = Ecj1FixtureCanonicalizer.Serialize(fixture);

        Assert.AreEqual(ExpectedOracleCandidateHash, candidateInput.CandidateContentHash);
        Assert.AreEqual(ExpectedVossStructuredHash, packet.StructuredContextHash);
        Assert.AreEqual(ExpectedVossRenderedHash, packet.RenderedContextHash);
        Assert.AreEqual(9112, ecj1.Length);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, Sha256Lower(ecj1));
    }

    private static void AssertInternalConstructorThrows(Type type, params object[] arguments)
    {
        var constructor = type
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        try
        {
            _ = constructor.Invoke(arguments);
            Assert.Fail($"Expected internal {type.Name} constructor to reject the invalid shape.");
        }
        catch (TargetInvocationException exception)
        {
            Assert.IsInstanceOfType(exception.InnerException, typeof(StateAuthorityException));
        }
    }

    private static IEnumerable<string> AllFixtureRecordIds(ValidatedFixture fixture)
    {
        foreach (var record in fixture.HistoricalTruth.Concat(fixture.UnresolvedPropositions).Concat(fixture.WorldState).Concat(fixture.SceneState).Concat(fixture.Pressures))
        {
            yield return record.Id.Value;
        }

        foreach (var character in fixture.Characters)
        {
            foreach (var record in character.Constitution
                         .Concat(character.Disposition)
                         .Concat(character.Circumstance)
                         .Concat(character.Observations)
                         .Concat(character.Knowledge)
                         .Concat(character.Beliefs)
                         .Concat(character.Suspicions)
                         .Concat(character.Memories)
                         .Concat(character.Goals))
            {
                yield return record.Id.Value;
            }

            foreach (var relationship in character.Relationships)
            {
                yield return relationship.Id.Value;
            }
        }
    }

    private static void AssertDecision(
        StateAuthorityDecision decision,
        StateAuthorityDisposition disposition,
        params StateAuthorityReasonCode[] reasons)
    {
        Assert.AreEqual(disposition, decision.Disposition);
        CollectionAssert.AreEqual(reasons, decision.Reasons.ToArray());
    }

    private static string EvaluationSignature(StateAuthorityEvaluation evaluation) =>
        $"{evaluation.Status}|" + string.Join(
            ";",
            evaluation.Decisions.Select(decision =>
                $"{decision.MutationIndex}:{decision.Disposition}:{string.Join(",", decision.Reasons)}"));

    private static StateAuthorityEvaluation Evaluate(
        StateAuthorityInput input,
        StateAuthorityPolicy policy,
        StateAuthorityReviewSet reviewSet) =>
        DeterministicStateAuthority.Evaluate(input, policy, reviewSet);

    private static StateAuthorityPolicy Policy(params StateMutationDomain[] domains) =>
        StateAuthorityPolicy.Create(domains.ToImmutableArray());

    private static StateAuthorityReviewSet NoReview(StateAuthorityInput input) =>
        StateAuthorityReviewSet.Bind(input, ImmutableArray<StateAuthorityReviewChoice>.Empty);

    private static StateAuthorityReviewSet Review(
        StateAuthorityInput input,
        params StateAuthorityReviewChoice[] choices) =>
        StateAuthorityReviewSet.Bind(input, choices.ToImmutableArray());

    private static Pipeline AuthorityPipeline(params Dictionary<string, object?>[] mutations) =>
        AuthorityPipelineForCandidate("No.", mutations);

    private static Pipeline AuthorityPipelineForCandidate(
        string candidateText,
        params Dictionary<string, object?>[] mutations)
    {
        var fixture = LoadMissingRaft();
        var accepted = AcceptedPipeline(fixture, candidateText);
        var proposal = StateInterpretationContract.ParseJson(
            accepted.Source,
            Encoding.UTF8.GetBytes(ProposalJson(mutations)));
        var snapshot = StateAuthoritySnapshot.Bind(fixture, ImmutableArray<RecordId>.Empty);
        var input = StateAuthorityInput.Bind(snapshot, accepted.Source, proposal);
        return new Pipeline(accepted.Source, input);
    }

    private static Pipeline AuthorityPipelineFromJson(string proposalJson)
    {
        var fixture = LoadMissingRaft();
        var accepted = AcceptedPipeline(fixture, "No.");
        var proposal = StateInterpretationContract.ParseJson(
            accepted.Source,
            Encoding.UTF8.GetBytes(proposalJson));
        var snapshot = StateAuthoritySnapshot.Bind(fixture, ImmutableArray<RecordId>.Empty);
        var input = StateAuthorityInput.Bind(snapshot, accepted.Source, proposal);
        return new Pipeline(accepted.Source, input);
    }

    private static Accepted AcceptedPipeline(ValidatedFixture fixture, string text)
    {
        var packet = Compose(fixture, MissingRaftContract.VossId);
        var candidate = PerformerCandidateContract.ParseJson(
            packet,
            Encoding.UTF8.GetBytes(CandidateJson(text)));
        var integrityInput = IntegrityCandidateInput.Bind(packet, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            integrityInput,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var evaluation = DeterministicIntegrityValidator.Validate(integrityInput, evidence);
        var source = StateInterpretationSource.Bind(packet, candidate, evaluation);
        return new Accepted(source);
    }

    private static Dictionary<string, object?> ValidAddMutation(string domain) =>
        domain switch
        {
            "characterBelief" => Mutation(domain, "add", subjectCharacterId: "VOSS", text: "Belief."),
            "relationship" => Mutation(domain, "add", subjectCharacterId: "VOSS", targetCharacterId: "WREN", text: "Relationship."),
            "characterClaim" => Mutation(domain, "add", subjectCharacterId: "VOSS", text: "Claim."),
            "pressure" => Mutation(domain, "add", text: "Pressure."),
            _ => throw new InvalidOperationException("Unsupported audit-test domain.")
        };

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

    private static string ProposalJson(params Dictionary<string, object?>[] mutations) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations
        });

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

    private static ContextPacket Compose(ValidatedFixture fixture, CharacterId subject)
    {
        var projection = CharacterBoundedAccessControl.Evaluate(fixture, subject).Projection;
        return DeterministicContextComposer.Compose(projection, subject).Packet;
    }

    private static HashSet<string> PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

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

    private static string Sha256Lower(byte[] bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

    private sealed record Accepted(StateInterpretationSource Source);
    private sealed record Pipeline(StateInterpretationSource Source, StateAuthorityInput Input);
}
