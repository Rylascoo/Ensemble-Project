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
public sealed class DeterministicStateAuthorityTests
{
    [TestMethod]
    public void SnapshotBind_ProjectsCanonicalProseFreeAuthorityState()
    {
        var fixture = LoadMissingRaft();
        var snapshot = StateAuthoritySnapshot.Bind(fixture, ImmutableArray<RecordId>.Empty);

        Assert.AreEqual(MissingRaftContract.ExpectedSceneId, snapshot.SceneId);
        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "VOSS", "WREN" },
            snapshot.RosterCharacterIds.Select(id => id.Value).ToArray());
        Assert.IsTrue(snapshot.Records.Length > 0);
        CollectionAssert.AreEqual(
            snapshot.Records.Select(record => record.RecordId.Value).OrderBy(value => value, StringComparer.Ordinal).ToArray(),
            snapshot.Records.Select(record => record.RecordId.Value).ToArray());
        Assert.AreEqual(
            snapshot.Records.Length,
            snapshot.Records.Select(record => record.RecordId.Value).Distinct(StringComparer.Ordinal).Count());
    }

    [TestMethod]
    public void SnapshotBind_SystemImmutableDomainsAreProtected()
    {
        var snapshot = Snapshot();

        AssertProtection(snapshot, MissingRaftContract.HtMarloweReleasedRaftId, StateAuthorityRecordProtection.SystemImmutable);
        AssertProtection(snapshot, MissingRaftContract.ConVossId, StateAuthorityRecordProtection.SystemImmutable);
        AssertProtection(snapshot, MissingRaftContract.ObsWrenMarloweReturnId, StateAuthorityRecordProtection.SystemImmutable);
        AssertProtection(snapshot, MissingRaftContract.BelVossAccidentalLossPlausibleId, StateAuthorityRecordProtection.None);
    }

    [TestMethod]
    public void SnapshotBind_CreatorLockOverlayIsExactAndOrderInvariant()
    {
        var fixture = LoadMissingRaft();
        var first = StateAuthoritySnapshot.Bind(
            fixture,
            ImmutableArray.Create(
                RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId),
                RecordId.From(MissingRaftContract.GoalVossId)));
        var second = StateAuthoritySnapshot.Bind(
            fixture,
            ImmutableArray.Create(
                RecordId.From(MissingRaftContract.GoalVossId),
                RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId)));

        CollectionAssert.AreEqual(
            SnapshotSignature(first),
            SnapshotSignature(second));
        AssertProtection(first, MissingRaftContract.BelVossAccidentalLossPlausibleId, StateAuthorityRecordProtection.CreatorLocked);
        AssertProtection(first, MissingRaftContract.GoalVossId, StateAuthorityRecordProtection.CreatorLocked);
    }

    [TestMethod]
    public void SnapshotBind_UnknownOrDuplicateCreatorLockFails()
    {
        var fixture = LoadMissingRaft();
        Assert.Throws<StateAuthorityException>(() =>
            StateAuthoritySnapshot.Bind(fixture, ImmutableArray.Create(RecordId.From("UNKNOWN-RECORD"))));
        Assert.Throws<StateAuthorityException>(() =>
            StateAuthoritySnapshot.Bind(
                fixture,
                ImmutableArray.Create(
                    RecordId.From(MissingRaftContract.GoalVossId),
                    RecordId.From(MissingRaftContract.GoalVossId))));
    }

    [TestMethod]
    public void InputBind_IsProposalBoundAndProseFree()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Voss believes Wren is withholding something."));

        Assert.AreEqual(StateAuthorityContracts.ProposalContentIdentityContract, pipeline.Input.ProposalContentIdentityContract);
        Assert.AreEqual(64, pipeline.Input.ProposalContentHash.Length);
        Assert.AreEqual(1, pipeline.Input.Mutations.Length);
        var mutation = (CharacterStateAuthorityMutationInput)pipeline.Input.Mutations.Single();
        Assert.AreEqual(StateMutationDomain.CharacterBelief, mutation.Domain);
        Assert.AreEqual(MissingRaftContract.VossId, mutation.SubjectCharacterId);
        Assert.IsInstanceOfType(mutation.Transition, typeof(AddStateAuthorityTransition));
    }

    [TestMethod]
    public void InputSurface_ExcludesInterpreterProseProviderAndCommitAuthority()
    {
        CollectionAssert.AreEquivalent(
            new[] { "ProposalContentIdentityContract", "ProposalContentHash", "Snapshot", "Mutations" },
            PublicPropertyNames(typeof(StateAuthorityInput)).ToArray());

        var authorityTypes = typeof(StateAuthorityInput).Assembly.GetTypes()
            .Where(type =>
                (type.IsPublic || type.IsNestedPublic) &&
                string.Equals(type.Namespace, "Ensemble.E0.Core.StateAuthority", StringComparison.Ordinal))
            .ToArray();
        foreach (var forbidden in new[]
                 {
                     "VisibleText", "Text", "Provider", "Model", "Confidence", "Rationale",
                     "Reasoning", "TakeId", "CommitId", "StateHash", "ProductionState"
                 })
        {
            Assert.IsFalse(
                authorityTypes.SelectMany(PublicPropertyNames).Contains(forbidden),
                forbidden);
        }
    }

    [TestMethod]
    public void InputBind_ProposalTextChangesIdentityWithoutChangingStructuralAuthorityInput()
    {
        var first = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "First semantic proposal."));
        var second = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Second semantic proposal."));

        Assert.AreNotEqual(first.Input.ProposalContentHash, second.Input.ProposalContentHash);
        Assert.AreEqual(MutationSignature(first.Input.Mutations.Single()), MutationSignature(second.Input.Mutations.Single()));
    }

    [TestMethod]
    public void InputBind_SupportOrderingCanonicalizesToSameProposalIdentity()
    {
        var first = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Same proposal.",
            supportingRecordIds: new[] { MissingRaftContract.GoalVossId, MissingRaftContract.BelVossAccidentalLossPlausibleId }));
        var second = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Same proposal.",
            supportingRecordIds: new[] { MissingRaftContract.BelVossAccidentalLossPlausibleId, MissingRaftContract.GoalVossId }));

        Assert.AreEqual(first.Input.ProposalContentHash, second.Input.ProposalContentHash);
        CollectionAssert.AreEqual(
            first.Input.Mutations.Single().SupportingRecordIds.Select(id => id.Value).ToArray(),
            second.Input.Mutations.Single().SupportingRecordIds.Select(id => id.Value).ToArray());
    }

    [TestMethod]
    public void InputBind_CanonicalIdentityUsesApprovedDomainSeparatedShape()
    {
        var pipeline = AuthorityPipeline(Mutation("worldState", "add", text: "Proposed global."));
        var source = pipeline.Source;
        var expected =
            "{\"identityContract\":\"" + StateAuthorityContracts.ProposalContentIdentityContract +
            "\",\"contractVersion\":\"" + StateInterpretationContract.ContractVersion +
            "\",\"candidateContentIdentityContract\":\"" + source.CandidateContentIdentityContract +
            "\",\"candidateContentHash\":\"" + source.CandidateContentHash +
            "\",\"sourceSceneId\":\"" + source.SourceSceneId.Value +
            "\",\"mutations\":[{\"domain\":\"worldState\",\"operation\":\"add\",\"subjectCharacterId\":null,\"targetCharacterId\":null,\"existingRecordId\":null,\"text\":\"Proposed global.\",\"supportingRecordIds\":[]}]}";
        var expectedHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(expected))).ToLowerInvariant();

        Assert.AreEqual(expectedHash, pipeline.Input.ProposalContentHash);
    }

    [TestMethod]
    public void InputBind_ProposalFromDifferentSourceFails()
    {
        var first = AcceptedPipeline("First Candidate.");
        var second = AcceptedPipeline("Second Candidate.");
        var proposal = Parse(first.Source, ProposalJson(Mutation("pressure", "add", text: "Pressure changes.")));
        var snapshot = Snapshot();

        Assert.Throws<StateAuthorityException>(() =>
            StateAuthorityInput.Bind(snapshot, second.Source, proposal));
    }

    [TestMethod]
    public void PolicyCreate_IsCanonicalAndOrderInvariant()
    {
        var first = Policy(StateMutationDomain.Pressure, StateMutationDomain.CharacterBelief);
        var second = Policy(StateMutationDomain.CharacterBelief, StateMutationDomain.Pressure);

        CollectionAssert.AreEqual(first.AutoApproveDomains.ToArray(), second.AutoApproveDomains.ToArray());
        CollectionAssert.AreEqual(
            new[] { StateMutationDomain.CharacterBelief, StateMutationDomain.Pressure },
            first.AutoApproveDomains.ToArray());
    }

    [TestMethod]
    [DataRow("worldState")]
    [DataRow("sceneState")]
    [DataRow("characterKnowledge")]
    [DataRow("characterMemory")]
    [DataRow("characterDisposition")]
    [DataRow("relationship")]
    public void PolicyCreate_AlwaysMandatoryDomainCannotAutoApprove(string domain)
    {
        Assert.Throws<StateAuthorityException>(() => Policy(Domain(domain)));
    }

    [TestMethod]
    public void PolicyCreate_DuplicateOrUndefinedDomainFails()
    {
        Assert.Throws<StateAuthorityException>(() =>
            Policy(StateMutationDomain.Pressure, StateMutationDomain.Pressure));
        Assert.Throws<StateAuthorityException>(() =>
            Policy((StateMutationDomain)int.MaxValue));
    }

    [TestMethod]
    public void ReviewSetBind_IsProposalBoundAndOrderInvariant()
    {
        var pipeline = AuthorityPipeline(
            Mutation("characterBelief", "add", subjectCharacterId: "VOSS", text: "A."),
            Mutation("pressure", "add", text: "B."));
        var first = StateAuthorityReviewSet.Bind(
            pipeline.Input,
            ImmutableArray.Create(
                StateAuthorityReviewChoice.Reject(1),
                StateAuthorityReviewChoice.Approve(0)));
        var second = StateAuthorityReviewSet.Bind(
            pipeline.Input,
            ImmutableArray.Create(
                StateAuthorityReviewChoice.Approve(0),
                StateAuthorityReviewChoice.Reject(1)));

        Assert.AreEqual(pipeline.Input.ProposalContentHash, first.ProposalContentHash);
        CollectionAssert.AreEqual(
            first.Choices.Select(ChoiceSignature).ToArray(),
            second.Choices.Select(ChoiceSignature).ToArray());
    }

    [TestMethod]
    public void ReviewSetBind_OutOfRangeOrDuplicateChoiceFails()
    {
        var pipeline = AuthorityPipeline(Mutation("pressure", "add", text: "A."));
        Assert.Throws<StateAuthorityException>(() =>
            StateAuthorityReviewSet.Bind(
                pipeline.Input,
                ImmutableArray.Create(StateAuthorityReviewChoice.Approve(1))));
        Assert.Throws<StateAuthorityException>(() =>
            StateAuthorityReviewSet.Bind(
                pipeline.Input,
                ImmutableArray.Create(
                    StateAuthorityReviewChoice.Approve(0),
                    StateAuthorityReviewChoice.Reject(0))));
    }

    [TestMethod]
    [DataRow("worldState")]
    [DataRow("sceneState")]
    [DataRow("characterKnowledge")]
    [DataRow("characterMemory")]
    [DataRow("characterDisposition")]
    [DataRow("relationship")]
    public void Evaluate_MandatoryReviewDomainsRequireExplicitReview(string domain)
    {
        var mutation = ValidAddMutation(domain);
        var pipeline = AuthorityPipeline(mutation);
        var evaluation = Evaluate(pipeline.Input, Policy(), NoReview(pipeline.Input));

        Assert.AreEqual(StateAuthorityEvaluationStatus.ReviewRequired, evaluation.Status);
        AssertDecision(
            evaluation.Decisions.Single(),
            StateAuthorityDisposition.RequiresReview,
            StateAuthorityReasonCode.MandatoryReview);
    }

    [TestMethod]
    public void Evaluate_UnresolvedSupersedeAndDeactivateAreMandatoryReview()
    {
        foreach (var operation in new[] { "supersede", "deactivate" })
        {
            var pipeline = AuthorityPipeline(Mutation(
                "unresolvedProposition",
                operation,
                existingRecordId: MissingRaftContract.UpRaftWouldFailId,
                text: operation == "deactivate" ? null : "Reframed uncertainty."));
            var evaluation = Evaluate(pipeline.Input, Policy(StateMutationDomain.UnresolvedProposition), NoReview(pipeline.Input));
            AssertDecision(
                evaluation.Decisions.Single(),
                StateAuthorityDisposition.RequiresReview,
                StateAuthorityReasonCode.MandatoryReview);
        }
    }

    [TestMethod]
    [DataRow("unresolvedProposition")]
    [DataRow("characterBelief")]
    [DataRow("characterSuspicion")]
    [DataRow("characterGoal")]
    [DataRow("characterCircumstance")]
    [DataRow("characterClaim")]
    [DataRow("pressure")]
    public void Evaluate_PolicyEligibleDomainCanAutoApprove(string domain)
    {
        var pipeline = AuthorityPipeline(ValidAddMutation(domain));
        var evaluation = Evaluate(
            pipeline.Input,
            Policy(Domain(domain)),
            NoReview(pipeline.Input));

        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, evaluation.Status);
        AssertDecision(
            evaluation.Decisions.Single(),
            StateAuthorityDisposition.Approved,
            StateAuthorityReasonCode.PolicyAutoApproved);
    }

    [TestMethod]
    public void Evaluate_PolicyEligibleWithoutAutoApprovalRequiresReview()
    {
        var pipeline = AuthorityPipeline(ValidAddMutation("characterBelief"));
        var evaluation = Evaluate(pipeline.Input, Policy(), NoReview(pipeline.Input));

        AssertDecision(
            evaluation.Decisions.Single(),
            StateAuthorityDisposition.RequiresReview,
            StateAuthorityReasonCode.PolicyReviewRequired);
    }

    [TestMethod]
    public void Evaluate_ExplicitReviewOverridesPolicyDefault()
    {
        var pipeline = AuthorityPipeline(ValidAddMutation("characterBelief"));
        var autoPolicy = Policy(StateMutationDomain.CharacterBelief);

        var rejected = Evaluate(
            pipeline.Input,
            autoPolicy,
            Review(pipeline.Input, StateAuthorityReviewChoice.Reject(0)));
        AssertDecision(
            rejected.Decisions.Single(),
            StateAuthorityDisposition.Rejected,
            StateAuthorityReasonCode.PolicyAutoApproved,
            StateAuthorityReasonCode.ExplicitReviewRejected);

        var approved = Evaluate(
            pipeline.Input,
            Policy(),
            Review(pipeline.Input, StateAuthorityReviewChoice.Approve(0)));
        AssertDecision(
            approved.Decisions.Single(),
            StateAuthorityDisposition.Approved,
            StateAuthorityReasonCode.PolicyReviewRequired,
            StateAuthorityReasonCode.ExplicitReviewApproved);
    }

    [TestMethod]
    public void Evaluate_ExplicitReviewResolvesMandatoryReview()
    {
        var pipeline = AuthorityPipeline(ValidAddMutation("relationship"));
        var evaluation = Evaluate(
            pipeline.Input,
            Policy(),
            Review(pipeline.Input, StateAuthorityReviewChoice.Approve(0)));

        AssertDecision(
            evaluation.Decisions.Single(),
            StateAuthorityDisposition.Approved,
            StateAuthorityReasonCode.MandatoryReview,
            StateAuthorityReasonCode.ExplicitReviewApproved);
    }

    [TestMethod]
    public void Evaluate_MissingSupportingRecordHardRejects()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Proposal.",
            supportingRecordIds: new[] { "MISSING-SUPPORT" }));
        var evaluation = Evaluate(
            pipeline.Input,
            Policy(StateMutationDomain.CharacterBelief),
            NoReview(pipeline.Input));

        AssertDecision(
            evaluation.Decisions.Single(),
            StateAuthorityDisposition.Rejected,
            StateAuthorityReasonCode.SupportingRecordMissing);
    }

    [TestMethod]
    public void Evaluate_MissingExistingRecordHardRejects()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "characterBelief",
            "supersede",
            subjectCharacterId: "VOSS",
            existingRecordId: "MISSING-RECORD",
            text: "Proposal."));
        var evaluation = Evaluate(pipeline.Input, Policy(StateMutationDomain.CharacterBelief), NoReview(pipeline.Input));

        AssertDecision(
            evaluation.Decisions.Single(),
            StateAuthorityDisposition.Rejected,
            StateAuthorityReasonCode.ExistingRecordMissing);
    }

    [TestMethod]
    public void Evaluate_ExistingRecordDomainMismatchHardRejects()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "characterGoal",
            "supersede",
            subjectCharacterId: "VOSS",
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: "Proposal."));
        var decision = Evaluate(pipeline.Input, Policy(StateMutationDomain.CharacterGoal), NoReview(pipeline.Input)).Decisions.Single();

        Assert.AreEqual(StateAuthorityDisposition.Rejected, decision.Disposition);
        CollectionAssert.Contains(decision.Reasons.ToArray(), StateAuthorityReasonCode.ExistingRecordDomainMismatch);
    }

    [TestMethod]
    public void Evaluate_ExistingRecordSubjectMismatchHardRejects()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "characterBelief",
            "supersede",
            subjectCharacterId: "VOSS",
            existingRecordId: MissingRaftContract.BelMarloweDamageUnacceptableId,
            text: "Proposal."));
        var decision = Evaluate(pipeline.Input, Policy(StateMutationDomain.CharacterBelief), NoReview(pipeline.Input)).Decisions.Single();

        CollectionAssert.Contains(decision.Reasons.ToArray(), StateAuthorityReasonCode.ExistingRecordSubjectMismatch);
    }

    [TestMethod]
    public void Evaluate_RelationshipTargetMismatchHardRejects()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "relationship",
            "supersede",
            subjectCharacterId: "VOSS",
            targetCharacterId: "WREN",
            existingRecordId: MissingRaftContract.RelVossMarloweId,
            text: "Proposal."));
        var decision = Evaluate(pipeline.Input, Policy(), NoReview(pipeline.Input)).Decisions.Single();

        CollectionAssert.Contains(decision.Reasons.ToArray(), StateAuthorityReasonCode.ExistingRecordTargetMismatch);
    }

    [TestMethod]
    public void Evaluate_CreatorLockedExistingRecordHardRejects()
    {
        var pipeline = AuthorityPipelineWithLocks(
            ImmutableArray.Create(RecordId.From(MissingRaftContract.BelVossAccidentalLossPlausibleId)),
            Mutation(
                "characterBelief",
                "supersede",
                subjectCharacterId: "VOSS",
                existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
                text: "Proposal."));
        var decision = Evaluate(pipeline.Input, Policy(StateMutationDomain.CharacterBelief), NoReview(pipeline.Input)).Decisions.Single();

        AssertDecision(
            decision,
            StateAuthorityDisposition.Rejected,
            StateAuthorityReasonCode.ExistingRecordProtected);
    }

    [TestMethod]
    public void Evaluate_ConflictingExistingRecordTargetRejectsAllContenders()
    {
        var first = Mutation(
            "characterBelief",
            "supersede",
            subjectCharacterId: "VOSS",
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: "First replacement.");
        var second = Mutation(
            "characterBelief",
            "deactivate",
            subjectCharacterId: "VOSS",
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: null);
        var pipeline = AuthorityPipeline(first, second);
        var evaluation = Evaluate(pipeline.Input, Policy(StateMutationDomain.CharacterBelief), NoReview(pipeline.Input));

        Assert.AreEqual(2, evaluation.Decisions.Length);
        foreach (var decision in evaluation.Decisions)
        {
            Assert.AreEqual(StateAuthorityDisposition.Rejected, decision.Disposition);
            CollectionAssert.Contains(decision.Reasons.ToArray(), StateAuthorityReasonCode.ConflictingExistingRecordTarget);
        }
    }

    [TestMethod]
    public void Evaluate_HardRejectWithReviewChoiceFailsClosed()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Proposal.",
            supportingRecordIds: new[] { "MISSING-SUPPORT" }));

        Assert.Throws<StateAuthorityException>(() =>
            Evaluate(
                pipeline.Input,
                Policy(StateMutationDomain.CharacterBelief),
                Review(pipeline.Input, StateAuthorityReviewChoice.Approve(0))));
    }

    [TestMethod]
    public void Evaluate_CompoundHardReasonsUseFixedContractOrdering()
    {
        var pipeline = AuthorityPipeline(Mutation(
            "characterGoal",
            "supersede",
            subjectCharacterId: "VOSS",
            existingRecordId: MissingRaftContract.ConVossId,
            text: "Proposal.",
            supportingRecordIds: new[] { "MISSING-SUPPORT" }));
        var decision = Evaluate(pipeline.Input, Policy(StateMutationDomain.CharacterGoal), NoReview(pipeline.Input)).Decisions.Single();

        CollectionAssert.AreEqual(
            new[]
            {
                StateAuthorityReasonCode.SupportingRecordMissing,
                StateAuthorityReasonCode.ExistingRecordDomainMismatch,
                StateAuthorityReasonCode.ExistingRecordProtected
            },
            decision.Reasons.ToArray());
    }

    [TestMethod]
    public void Evaluate_ReviewSetFromDifferentProposalIdentityFails()
    {
        var first = AuthorityPipeline(Mutation("pressure", "add", text: "First."));
        var second = AuthorityPipeline(Mutation("pressure", "add", text: "Second."));
        var reviewSet = Review(first.Input, StateAuthorityReviewChoice.Approve(0));

        Assert.Throws<StateAuthorityException>(() =>
            Evaluate(second.Input, Policy(), reviewSet));
    }

    [TestMethod]
    public void Evaluate_EmptyProposalIsCompleteWithNoDecisions()
    {
        var pipeline = AuthorityPipeline();
        var evaluation = Evaluate(pipeline.Input, Policy(), NoReview(pipeline.Input));

        Assert.AreEqual(StateAuthorityEvaluationStatus.Complete, evaluation.Status);
        Assert.AreEqual(0, evaluation.Decisions.Length);
    }

    [TestMethod]
    public void Evaluate_SameInputsReplayDeterministically()
    {
        var pipeline = AuthorityPipeline(
            ValidAddMutation("characterBelief"),
            ValidAddMutation("relationship"),
            Mutation("pressure", "add", text: "Pressure."));
        var policy = Policy(StateMutationDomain.CharacterBelief, StateMutationDomain.Pressure);
        var reviewSet = Review(pipeline.Input, StateAuthorityReviewChoice.Approve(1));
        var expected = EvaluationSignature(Evaluate(pipeline.Input, policy, reviewSet));

        for (var index = 0; index < 50; index++)
        {
            Assert.AreEqual(expected, EvaluationSignature(Evaluate(pipeline.Input, policy, reviewSet)));
        }
    }

    [TestMethod]
    public void Evaluate_DoesNotMutateFixtureOrCreateCommitAuthority()
    {
        var fixture = LoadMissingRaft();
        var before = Ecj1FixtureCanonicalizer.Serialize(fixture);
        var pipeline = AuthorityPipeline(fixture, ImmutableArray<RecordId>.Empty, ValidAddMutation("characterBelief"));
        _ = Evaluate(
            pipeline.Input,
            Policy(StateMutationDomain.CharacterBelief),
            NoReview(pipeline.Input));
        var after = Ecj1FixtureCanonicalizer.Serialize(fixture);

        CollectionAssert.AreEqual(before, after);
        foreach (var forbidden in new[] { "TakeId", "CommitId", "StateHash", "ProductionState", "MutationApplication" })
        {
            Assert.IsFalse(
                PublicPropertyNames(typeof(StateAuthorityEvaluation)).Contains(forbidden) ||
                PublicPropertyNames(typeof(StateAuthorityDecision)).Contains(forbidden),
                forbidden);
        }
    }

    [TestMethod]
    public void AuthorityProducedTypesAreImmutableAndNonPubliclyConstructible()
    {
        foreach (var type in new[]
                 {
                     typeof(StateAuthoritySnapshot),
                     typeof(StateAuthorityInput),
                     typeof(StateAuthorityPolicy),
                     typeof(StateAuthorityReviewSet),
                     typeof(StateAuthorityDecision),
                     typeof(StateAuthorityTrace),
                     typeof(StateAuthorityEvaluation)
                 })
        {
            Assert.AreEqual(0, type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length, type.Name);
            Assert.IsTrue(
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .All(property => property.SetMethod is null),
                type.Name);
        }
    }

    [TestMethod]
    public void EvaluatorApi_IsNarrowAndContainsNoProviderModelOrApplySurface()
    {
        var methods = typeof(DeterministicStateAuthority)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        Assert.AreEqual(1, methods.Length);
        Assert.AreEqual(nameof(DeterministicStateAuthority.Evaluate), methods[0].Name);
        CollectionAssert.AreEqual(
            new[] { typeof(StateAuthorityInput), typeof(StateAuthorityPolicy), typeof(StateAuthorityReviewSet) },
            methods[0].GetParameters().Select(parameter => parameter.ParameterType).ToArray());
    }

    private static void AssertProtection(
        StateAuthoritySnapshot snapshot,
        string recordId,
        StateAuthorityRecordProtection expected)
    {
        Assert.AreEqual(
            expected,
            snapshot.Records.Single(record => record.RecordId == RecordId.From(recordId)).Protection);
    }

    private static void AssertDecision(
        StateAuthorityDecision decision,
        StateAuthorityDisposition disposition,
        params StateAuthorityReasonCode[] reasons)
    {
        Assert.AreEqual(disposition, decision.Disposition);
        CollectionAssert.AreEqual(reasons, decision.Reasons.ToArray());
    }

    private static string[] SnapshotSignature(StateAuthoritySnapshot snapshot) =>
        snapshot.Records
            .Select(record => $"{record.RecordId.Value}|{record.RecordDomain}|{record.Lifecycle}|{record.Protection}")
            .ToArray();

    private static string MutationSignature(StateAuthorityMutationInput mutation)
    {
        var subject = mutation is CharacterStateAuthorityMutationInput character
            ? character.SubjectCharacterId.Value
            : mutation is RelationshipStateAuthorityMutationInput relationship
                ? relationship.SubjectCharacterId.Value
                : "-";
        var target = mutation is RelationshipStateAuthorityMutationInput relation
            ? relation.TargetCharacterId.Value
            : "-";
        var existing = mutation.Transition switch
        {
            SupersedeStateAuthorityTransition supersede => supersede.ExistingRecordId.Value,
            DeactivateStateAuthorityTransition deactivate => deactivate.ExistingRecordId.Value,
            _ => "-"
        };
        return $"{mutation.MutationIndex}|{mutation.Domain}|{mutation.Transition.GetType().Name}|{subject}|{target}|{existing}|{string.Join(",", mutation.SupportingRecordIds.Select(id => id.Value))}";
    }

    private static string ChoiceSignature(StateAuthorityReviewChoice choice) =>
        $"{choice.MutationIndex}|{choice.Choice}";

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

    private static StateAuthoritySnapshot Snapshot() =>
        StateAuthoritySnapshot.Bind(LoadMissingRaft(), ImmutableArray<RecordId>.Empty);

    private static AuthorityPipelineResult AuthorityPipeline(
        params Dictionary<string, object?>[] mutations) =>
        AuthorityPipeline(LoadMissingRaft(), ImmutableArray<RecordId>.Empty, mutations);

    private static AuthorityPipelineResult AuthorityPipelineWithLocks(
        ImmutableArray<RecordId> locks,
        params Dictionary<string, object?>[] mutations) =>
        AuthorityPipeline(LoadMissingRaft(), locks, mutations);

    private static AuthorityPipelineResult AuthorityPipeline(
        ValidatedFixture fixture,
        ImmutableArray<RecordId> locks,
        params Dictionary<string, object?>[] mutations)
    {
        var accepted = AcceptedPipeline(fixture, "No.");
        var proposal = Parse(accepted.Source, ProposalJson(mutations));
        var snapshot = StateAuthoritySnapshot.Bind(fixture, locks);
        var input = StateAuthorityInput.Bind(snapshot, accepted.Source, proposal);
        return new AuthorityPipelineResult(snapshot, accepted.Source, proposal, input);
    }

    private static AcceptedPipelineResult AcceptedPipeline(string text) =>
        AcceptedPipeline(LoadMissingRaft(), text);

    private static AcceptedPipelineResult AcceptedPipeline(ValidatedFixture fixture, string text)
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
        return new AcceptedPipelineResult(source);
    }

    private static StateInterpretationProposal Parse(StateInterpretationSource source, string json) =>
        StateInterpretationContract.ParseJson(source, Encoding.UTF8.GetBytes(json));

    private static Dictionary<string, object?> ValidAddMutation(string domain) =>
        domain switch
        {
            "worldState" or "sceneState" or "unresolvedProposition" or "pressure" =>
                Mutation(domain, "add", text: "Proposed global state."),
            "characterKnowledge" or "characterMemory" =>
                Mutation(domain, "add", subjectCharacterId: "VOSS", text: "Proposed append-only state."),
            "characterBelief" or "characterSuspicion" or "characterGoal" or
            "characterDisposition" or "characterCircumstance" =>
                Mutation(domain, "add", subjectCharacterId: "VOSS", text: "Proposed Character state."),
            "characterClaim" =>
                Mutation(domain, "add", subjectCharacterId: "VOSS", text: "Voss claims something."),
            "relationship" =>
                Mutation(
                    domain,
                    "add",
                    subjectCharacterId: "VOSS",
                    targetCharacterId: "WREN",
                    text: "Proposed relationship effect."),
            _ => throw new InvalidOperationException("Unsupported State Authority test domain.")
        };

    private static StateMutationDomain Domain(string domain) =>
        domain switch
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
            _ => throw new InvalidOperationException("Unsupported State Authority test domain.")
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

    private sealed record AcceptedPipelineResult(StateInterpretationSource Source);

    private sealed record AuthorityPipelineResult(
        StateAuthoritySnapshot Snapshot,
        StateInterpretationSource Source,
        StateInterpretationProposal Proposal,
        StateAuthorityInput Input);
}
