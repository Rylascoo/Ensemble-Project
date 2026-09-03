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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Integrity;

[TestClass]
public sealed class DeterministicIntegrityValidatorTests
{
    private const string ExpectedVossStructuredHash =
        "bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b";
    private const string ExpectedVossRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const string ExpectedOracleCandidateHash =
        "18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2";

    [TestMethod]
    public void Bind_ValidMissingRaftVossCandidateProducesLeastPrivilegeInput()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(
            packet,
            "Wren?",
            new[] { "WREN", "MARLOWE" },
            "WREN");

        var input = IntegrityCandidateInput.Bind(packet, candidate);

        Assert.AreEqual(
            E0IntegrityContracts.CandidateContentIdentityContract,
            input.CandidateContentIdentityContract);
        Assert.AreEqual(packet.ContextPacketId, input.SourceContextPacketId);
        Assert.AreEqual(0, input.DeterministicRejectCodes.Length);
        Assert.AreEqual(ExpectedOracleCandidateHash, input.CandidateContentHash);
    }

    [TestMethod]
    public void IntegrityInputSurface_ContainsOnlyApprovedStructuralFields()
    {
        var properties = PublicPropertyNames(typeof(IntegrityCandidateInput));

        CollectionAssert.AreEquivalent(
            new[]
            {
                "CandidateContentIdentityContract",
                "CandidateContentHash",
                "SourceContextPacketId",
                "DeterministicRejectCodes"
            },
            properties.ToArray());

        foreach (var forbidden in new[]
                 {
                     "Candidate", "VisibleText", "Control", "Context", "Rendered", "Knowledge",
                     "Beliefs", "Suspicions", "Memories", "Goals", "Relationships", "Pressures",
                     "Provider", "Model", "Assessor", "TakeId", "CommitId", "Authoritative",
                     "Authenticated"
                 })
        {
            Assert.IsFalse(properties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void IntegrityConstructionSurface_IsNarrowAndNonForgeable()
    {
        Assert.AreEqual(
            0,
            typeof(IntegrityCandidateInput)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);
        Assert.AreEqual(
            0,
            typeof(IntegrityConcernEvidence)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);
        Assert.AreEqual(
            0,
            typeof(IntegrityValidationTrace)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);
        Assert.AreEqual(
            0,
            typeof(IntegrityValidationEvaluation)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);

        CollectionAssert.AreEqual(
            new[] { nameof(IntegrityCandidateInput.Bind) },
            PublicStaticMethodNames(typeof(IntegrityCandidateInput)));
        CollectionAssert.AreEqual(
            new[] { nameof(IntegrityConcernEvidence.Bind) },
            PublicStaticMethodNames(typeof(IntegrityConcernEvidence)));
        CollectionAssert.AreEqual(
            new[] { nameof(DeterministicIntegrityValidator.Validate) },
            PublicStaticMethodNames(typeof(DeterministicIntegrityValidator)));
    }

    [TestMethod]
    public void ValidatorApi_AcceptsOnlyLeastPrivilegeInputAndOptionalConcernEvidence()
    {
        var validate = typeof(DeterministicIntegrityValidator).GetMethod(
            nameof(DeterministicIntegrityValidator.Validate),
            BindingFlags.Public | BindingFlags.Static)!;

        CollectionAssert.AreEqual(
            new[] { typeof(IntegrityCandidateInput), typeof(IntegrityConcernEvidence) },
            validate.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(IntegrityValidationEvaluation), validate.ReturnType);
    }

    [TestMethod]
    public void CandidateContentHash_MatchesIndependentCanonicalOracle()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(
            packet,
            "Wren?",
            new[] { "WREN", "MARLOWE" },
            "WREN");

        var input = IntegrityCandidateInput.Bind(packet, candidate);

        Assert.AreEqual(ExpectedOracleCandidateHash, input.CandidateContentHash);
    }

    [TestMethod]
    public void CandidateContentHash_IsDeterministicForIdenticalCandidateSemantics()
    {
        var packet = ComposeVoss();
        var first = ParseCandidate(packet, "No.", new[] { "WREN" }, "MARLOWE");
        var second = ParseCandidate(packet, "No.", new[] { "WREN" }, "MARLOWE");

        Assert.AreEqual(
            IntegrityCandidateInput.Bind(packet, first).CandidateContentHash,
            IntegrityCandidateInput.Bind(packet, second).CandidateContentHash);
    }

    [TestMethod]
    public void CandidateContentHash_ChangesWithVisibleTextAddressNominationAndContextIdentity()
    {
        var packet = ComposeVoss();
        var variantPacket = ComposeVossVariant();
        var baseline = ParseCandidate(packet, "No.");
        var textVariant = ParseCandidate(packet, "Different.");
        var addressVariant = ParseCandidate(packet, "No.", new[] { "WREN" });
        var nominationVariant = ParseCandidate(packet, "No.", nominatedCharacterId: "WREN");
        var contextVariant = ParseCandidate(variantPacket, "No.");

        var baselineHash = IntegrityCandidateInput.Bind(packet, baseline).CandidateContentHash;

        Assert.AreNotEqual(
            baselineHash,
            IntegrityCandidateInput.Bind(packet, textVariant).CandidateContentHash);
        Assert.AreNotEqual(
            baselineHash,
            IntegrityCandidateInput.Bind(packet, addressVariant).CandidateContentHash);
        Assert.AreNotEqual(
            baselineHash,
            IntegrityCandidateInput.Bind(packet, nominationVariant).CandidateContentHash);
        Assert.AreNotEqual(
            baselineHash,
            IntegrityCandidateInput.Bind(variantPacket, contextVariant).CandidateContentHash);
    }

    [TestMethod]
    public void CandidateContentHash_AddressInputOrderIsStableThroughPatch0006CanonicalControl()
    {
        var packet = ComposeVoss();
        var first = ParseCandidate(packet, "Both.", new[] { "WREN", "MARLOWE" });
        var second = ParseCandidate(packet, "Both.", new[] { "MARLOWE", "WREN" });

        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "WREN" },
            first.Control.AddressedCharacterIds.Select(id => id.Value).ToArray());
        Assert.AreEqual(
            IntegrityCandidateInput.Bind(packet, first).CandidateContentHash,
            IntegrityCandidateInput.Bind(packet, second).CandidateContentHash);
    }

    [TestMethod]
    public void CandidateContentHash_IsLowercase64HexAndNotAuthorityIdentity()
    {
        var packet = ComposeVoss();
        var input = IntegrityCandidateInput.Bind(packet, ParseCandidate(packet, "No."));

        Assert.AreEqual(64, input.CandidateContentHash.Length);
        StringAssert.Matches(input.CandidateContentHash, new System.Text.RegularExpressions.Regex("^[0-9a-f]{64}$"));

        var properties = PublicPropertyNames(typeof(IntegrityCandidateInput));
        Assert.IsFalse(properties.Contains("CandidateId"));
        Assert.IsFalse(properties.Contains("AttemptId"));
        Assert.IsFalse(properties.Contains("TakeId"));
        Assert.IsFalse(properties.Contains("CommitId"));
    }

    [TestMethod]
    public void SameSubjectDifferentContextCandidate_ProducesContextIdentityRejectOnly()
    {
        var originalPacket = ComposeVoss();
        var sourcePacket = ComposeVossVariant();
        var candidate = ParseCandidate(originalPacket, "No.");

        var input = IntegrityCandidateInput.Bind(sourcePacket, candidate);

        CollectionAssert.AreEqual(
            new[] { IntegrityDeterministicRejectCode.ContextPacketIdentityMismatch },
            input.DeterministicRejectCodes.ToArray());
    }

    [TestMethod]
    public void DifferentSubjectValidCandidate_ProducesBothRejectCodesInFrozenOrder()
    {
        var sourcePacket = ComposeVoss();
        var marlowePacket = ComposeMarlowe();
        var candidate = ParseCandidate(marlowePacket, "No.");

        var input = IntegrityCandidateInput.Bind(sourcePacket, candidate);

        CollectionAssert.AreEqual(
            new[]
            {
                IntegrityDeterministicRejectCode.SubjectContextMismatch,
                IntegrityDeterministicRejectCode.ContextPacketIdentityMismatch
            },
            input.DeterministicRejectCodes.ToArray());
    }

    [TestMethod]
    public void DeterministicRejectCodes_AreDistinctAndBounded()
    {
        var sourcePacket = ComposeVoss();
        var marlowePacket = ComposeMarlowe();
        var input = IntegrityCandidateInput.Bind(
            sourcePacket,
            ParseCandidate(marlowePacket, "No."));

        Assert.IsTrue(input.DeterministicRejectCodes.Length <= 2);
        Assert.AreEqual(
            input.DeterministicRejectCodes.Length,
            input.DeterministicRejectCodes.Distinct().Count());
    }

    [TestMethod]
    public void ConcernEvidence_EmptyInitializedSetIsValidForZeroRejectInput()
    {
        var input = CleanInput("No.");

        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);

        Assert.AreEqual(E0IntegrityContracts.ConcernEvidenceContract, evidence.EvidenceContract);
        Assert.AreEqual(0, evidence.Concerns.Length);
        Assert.AreEqual(input.CandidateContentHash, evidence.CandidateContentHash);
        Assert.AreEqual(
            input.CandidateContentIdentityContract,
            evidence.CandidateContentIdentityContract);
    }

    [TestMethod]
    public void ConcernEvidence_StoresKindsInFrozenCanonicalOrder()
    {
        var input = CleanInput("No.");
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray.Create(
                IntegrityConcernKind.IndeterminateSemanticIntegrity,
                IntegrityConcernKind.PotentialTechnicalArtifactLeak,
                IntegrityConcernKind.PotentialInaccessibleInformationUse,
                IntegrityConcernKind.PotentialLockedAuthorityViolation,
                IntegrityConcernKind.PotentialProtectedInformationExposure));

        CollectionAssert.AreEqual(
            new[]
            {
                IntegrityConcernKind.PotentialInaccessibleInformationUse,
                IntegrityConcernKind.PotentialProtectedInformationExposure,
                IntegrityConcernKind.PotentialLockedAuthorityViolation,
                IntegrityConcernKind.PotentialTechnicalArtifactLeak,
                IntegrityConcernKind.IndeterminateSemanticIntegrity
            },
            evidence.Concerns.ToArray());
    }

    [TestMethod]
    public void ConcernEvidence_DefaultUndefinedDuplicateAndOverCountFailClosed()
    {
        var input = CleanInput("No.");

        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityConcernEvidence.Bind(input, default));
        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityConcernEvidence.Bind(
                input,
                ImmutableArray.Create((IntegrityConcernKind)999)));
        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityConcernEvidence.Bind(
                input,
                ImmutableArray.Create(
                    IntegrityConcernKind.PotentialTechnicalArtifactLeak,
                    IntegrityConcernKind.PotentialTechnicalArtifactLeak)));
        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityConcernEvidence.Bind(
                input,
                ImmutableArray.Create(
                    IntegrityConcernKind.PotentialInaccessibleInformationUse,
                    IntegrityConcernKind.PotentialProtectedInformationExposure,
                    IntegrityConcernKind.PotentialLockedAuthorityViolation,
                    IntegrityConcernKind.PotentialTechnicalArtifactLeak,
                    IntegrityConcernKind.IndeterminateSemanticIntegrity,
                    IntegrityConcernKind.PotentialInaccessibleInformationUse)));
    }

    [TestMethod]
    public void ConcernEvidenceSurface_HasNoRationaleConfidenceDispositionOrAuthenticityFields()
    {
        var properties = PublicPropertyNames(typeof(IntegrityConcernEvidence));

        CollectionAssert.AreEquivalent(
            new[]
            {
                "EvidenceContract",
                "CandidateContentIdentityContract",
                "CandidateContentHash",
                "Concerns"
            },
            properties.ToArray());

        foreach (var forbidden in new[]
                 {
                     "Rationale", "Reasoning", "Confidence", "Score", "Probability", "Disposition",
                     "Assessor", "Provider", "Model", "Authenticated", "Authoritative", "VisibleText",
                     "ProtectedText", "Mutation"
                 })
        {
            Assert.IsFalse(properties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void ConcernEvidence_CannotBindToDeterministicRejectInput()
    {
        var sourcePacket = ComposeVossVariant();
        var candidate = ParseCandidate(ComposeVoss(), "No.");
        var rejectInput = IntegrityCandidateInput.Bind(sourcePacket, candidate);

        Assert.AreNotEqual(0, rejectInput.DeterministicRejectCodes.Length);
        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityConcernEvidence.Bind(
                rejectInput,
                ImmutableArray<IntegrityConcernKind>.Empty));
    }

    [TestMethod]
    public void DeterministicReject_WithNullEvidenceReturnsRejectAndNullTraceEvidence()
    {
        var sourcePacket = ComposeVossVariant();
        var candidate = ParseCandidate(ComposeVoss(), "No.");
        var input = IntegrityCandidateInput.Bind(sourcePacket, candidate);

        var evaluation = DeterministicIntegrityValidator.Validate(input, concernEvidence: null);

        Assert.AreEqual(IntegrityDisposition.Reject, evaluation.Disposition);
        Assert.IsNull(evaluation.Trace.ConcernEvidence);
        Assert.AreSame(input, evaluation.Trace.Input);
    }

    [TestMethod]
    public void DeterministicReject_WithNonNullEvidenceFailsInsteadOfIgnoringEvidence()
    {
        var cleanInput = CleanInput("No.");
        var evidence = IntegrityConcernEvidence.Bind(
            cleanInput,
            ImmutableArray<IntegrityConcernKind>.Empty);

        var sourcePacket = ComposeVossVariant();
        var candidate = ParseCandidate(ComposeVoss(), "No.");
        var rejectInput = IntegrityCandidateInput.Bind(sourcePacket, candidate);

        Assert.Throws<IntegrityValidationException>(() =>
            DeterministicIntegrityValidator.Validate(rejectInput, evidence));
    }

    [TestMethod]
    public void ZeroRejectInput_RequiresConcernEvidence()
    {
        var input = CleanInput("No.");

        Assert.Throws<IntegrityValidationException>(() =>
            DeterministicIntegrityValidator.Validate(input, concernEvidence: null));
    }

    [TestMethod]
    public void ConcernEvidence_ForDifferentCandidateHashFailsClosed()
    {
        var firstInput = CleanInput("First.");
        var secondInput = CleanInput("Second.");
        var evidence = IntegrityConcernEvidence.Bind(
            firstInput,
            ImmutableArray<IntegrityConcernKind>.Empty);

        Assert.AreNotEqual(firstInput.CandidateContentHash, secondInput.CandidateContentHash);
        Assert.Throws<IntegrityValidationException>(() =>
            DeterministicIntegrityValidator.Validate(secondInput, evidence));
    }

    [TestMethod]
    public void SemanticConcern_OnOtherwiseBoundCandidateRequestsAnotherTake()
    {
        var input = CleanInput("No.");
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray.Create(
                IntegrityConcernKind.PotentialInaccessibleInformationUse));

        var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(IntegrityDisposition.RequestAnotherTake, evaluation.Disposition);
        Assert.AreSame(evidence, evaluation.Trace.ConcernEvidence);
    }

    [TestMethod]
    public void EmptyConcernEvidence_OnOtherwiseBoundCandidateAcceptsEvaluationOnly()
    {
        var input = CleanInput("No.");
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);

        var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(IntegrityDisposition.Accept, evaluation.Disposition);
        Assert.AreSame(evidence, evaluation.Trace.ConcernEvidence);
    }

    [TestMethod]
    public void IntegrityTraceAndEvaluationSurfaces_AreMinimalAndNonAuthoritative()
    {
        CollectionAssert.AreEquivalent(
            new[] { "Disposition", "Trace" },
            PublicPropertyNames(typeof(IntegrityValidationEvaluation)).ToArray());
        CollectionAssert.AreEquivalent(
            new[] { "ValidationContract", "Input", "ConcernEvidence" },
            PublicPropertyNames(typeof(IntegrityValidationTrace)).ToArray());

        var input = CleanInput("No.");
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);
        var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(E0IntegrityContracts.ValidationContract, evaluation.Trace.ValidationContract);
        Assert.AreSame(input, evaluation.Trace.Input);
    }

    [TestMethod]
    public void IntegrityEvaluationSurface_HasNoTakeStateHistoryOrEligibilityAuthority()
    {
        var integrityTypes = typeof(DeterministicIntegrityValidator).Assembly
            .GetTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Integrity",
                StringComparison.Ordinal))
            .ToArray();
        var propertyNames = integrityTypes
            .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);
        var publicOperations = integrityTypes
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var forbidden in new[]
                 {
                     "TakeId", "CommitId", "State", "Mutation", "History", "AcceptedTake",
                     "Eligible", "Eligibility", "Attestation", "Effective", "Applied", "CurrentOpportunity"
                 })
        {
            Assert.IsFalse(propertyNames.Contains(forbidden), forbidden);
        }

        foreach (var forbidden in new[]
                 {
                     "Apply", "Commit", "Persist", "Promote", "Trigger", "Retry", "Spend", "Mutate"
                 })
        {
            Assert.IsFalse(publicOperations.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void FalseClaim_IsNotDeterministicallyRejectedForTruthConflict()
    {
        var input = CleanInput("Wren stole the raft. I know it.");
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);

        var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(0, input.DeterministicRejectCodes.Length);
        Assert.AreEqual(IntegrityDisposition.Accept, evaluation.Disposition);
    }

    [TestMethod]
    public void TechnicalWords_DoNotAutomaticallyCreateArtifactConcern()
    {
        var input = CleanInput("The HTTP provider timed out twice.");
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);

        var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(IntegrityDisposition.Accept, evaluation.Disposition);
        Assert.AreEqual(0, evidence.Concerns.Length);
    }

    [TestMethod]
    public void IndeterminateSemanticIntegrity_IsTypedCompletedReviewConcernNotTechnicalFailureState()
    {
        var input = CleanInput("No.");
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray.Create(
                IntegrityConcernKind.IndeterminateSemanticIntegrity));

        var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(IntegrityDisposition.RequestAnotherTake, evaluation.Disposition);
        CollectionAssert.AreEqual(
            new[] { IntegrityConcernKind.IndeterminateSemanticIntegrity },
            evidence.Concerns.ToArray());
    }

    [TestMethod]
    public void RequestAnotherTake_HasNoRetrySpendOrProviderExecutionSurface()
    {
        var methodNames = typeof(DeterministicIntegrityValidator).Assembly
            .GetTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Integrity",
                StringComparison.Ordinal))
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var forbidden in new[]
                 {
                     "Retry", "Regenerate", "CallProvider", "Spend", "IncrementBudget", "UseUnderstudy"
                 })
        {
            Assert.IsFalse(methodNames.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void BindAndValidate_DoNotRewriteOrMutateCandidateContextOrEvidence()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "Exact performer output.", new[] { "WREN" });
        var originalText = candidate.VisibleText;
        var originalPacketId = packet.ContextPacketId;

        var input = IntegrityCandidateInput.Bind(packet, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray.Create(IntegrityConcernKind.PotentialLockedAuthorityViolation));
        var originalConcerns = evidence.Concerns.ToArray();
        _ = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(originalText, candidate.VisibleText);
        Assert.AreEqual(originalPacketId, packet.ContextPacketId);
        CollectionAssert.AreEqual(originalConcerns, evidence.Concerns.ToArray());
    }

    [TestMethod]
    public void IntegrityNamespace_HasNoDirectorStateTakeCommitOrProviderDependencyInPublicApi()
    {
        var integrityTypes = typeof(DeterministicIntegrityValidator).Assembly
            .GetTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Integrity",
                StringComparison.Ordinal))
            .ToArray();
        var referencedPublicTypes = integrityTypes
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .SelectMany(method =>
                method.GetParameters().Select(parameter => parameter.ParameterType)
                    .Append(method.ReturnType))
            .Concat(
                integrityTypes.SelectMany(type =>
                    type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(property => property.PropertyType)))
            .Select(type => type.FullName ?? type.Name)
            .ToArray();

        foreach (var typeName in referencedPublicTypes)
        {
            Assert.IsFalse(typeName.Contains("Director", StringComparison.Ordinal), typeName);
            Assert.IsFalse(typeName.Contains("State", StringComparison.Ordinal), typeName);
            Assert.IsFalse(typeName.Contains("Take", StringComparison.Ordinal), typeName);
            Assert.IsFalse(typeName.Contains("Commit", StringComparison.Ordinal), typeName);
            Assert.IsFalse(typeName.Contains("Provider", StringComparison.Ordinal), typeName);
        }
    }

    [TestMethod]
    public void RejectRequestAndAcceptEvaluations_DoNotExposeHistoryMutationOrTriggerOperations()
    {
        var operations = typeof(DeterministicIntegrityValidator).Assembly
            .GetTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Integrity",
                StringComparison.Ordinal))
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var forbidden in new[]
                 {
                     "AppendHistory", "EnterHistory", "AdvanceOpportunity", "SetCurrentOpportunity",
                     "TriggerPerformer", "AcceptTake", "RejectTake"
                 })
        {
            Assert.IsFalse(operations.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void SyntheticEvidence_CanProduceSyntheticAcceptWithoutAuthenticityClaim()
    {
        var input = CleanInput("No.");
        var syntheticEvidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);

        var evaluation = DeterministicIntegrityValidator.Validate(input, syntheticEvidence);

        Assert.AreEqual(IntegrityDisposition.Accept, evaluation.Disposition);
        var properties = PublicPropertyNames(typeof(IntegrityConcernEvidence));
        Assert.IsFalse(properties.Contains("Authenticated"));
        Assert.IsFalse(properties.Contains("AssessorId"));
        Assert.IsFalse(properties.Contains("ProviderAttemptId"));
    }

    [TestMethod]
    public void Validation_IsDeterministicallyEquivalentForIdenticalBoundInputsAndEvidence()
    {
        var firstInput = CleanInput("No.");
        var secondInput = CleanInput("No.");
        var concerns = ImmutableArray.Create(
            IntegrityConcernKind.PotentialProtectedInformationExposure,
            IntegrityConcernKind.PotentialTechnicalArtifactLeak);
        var firstEvidence = IntegrityConcernEvidence.Bind(firstInput, concerns);
        var secondEvidence = IntegrityConcernEvidence.Bind(secondInput, concerns);

        var first = DeterministicIntegrityValidator.Validate(firstInput, firstEvidence);
        var second = DeterministicIntegrityValidator.Validate(secondInput, secondEvidence);

        Assert.AreEqual(first.Disposition, second.Disposition);
        Assert.AreEqual(first.Trace.ValidationContract, second.Trace.ValidationContract);
        Assert.AreEqual(
            first.Trace.Input.CandidateContentHash,
            second.Trace.Input.CandidateContentHash);
        CollectionAssert.AreEqual(
            first.Trace.ConcernEvidence!.Concerns.ToArray(),
            second.Trace.ConcernEvidence!.Concerns.ToArray());
    }

    [TestMethod]
    public void NullBindingAndValidationInputsFailInIntegrityExceptionDomain()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "No.");
        var cleanInput = IntegrityCandidateInput.Bind(packet, candidate);

        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityCandidateInput.Bind(null!, candidate));
        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityCandidateInput.Bind(packet, null!));
        Assert.Throws<IntegrityValidationException>(() =>
            IntegrityConcernEvidence.Bind(
                null!,
                ImmutableArray<IntegrityConcernKind>.Empty));
        Assert.Throws<IntegrityValidationException>(() =>
            DeterministicIntegrityValidator.Validate(null!, concernEvidence: null));
        Assert.Throws<IntegrityValidationException>(() =>
            DeterministicIntegrityValidator.Validate(cleanInput, concernEvidence: null));
    }

    [TestMethod]
    public void ContractConstants_AreExactAndVersionDomainsRemainOnlyThree()
    {
        Assert.AreEqual(
            "ensemble.e0.integrity.candidate-content.v1",
            E0IntegrityContracts.CandidateContentIdentityContract);
        Assert.AreEqual(
            "ensemble.e0.integrity.concerns.v1",
            E0IntegrityContracts.ConcernEvidenceContract);
        Assert.AreEqual(
            "ensemble.e0.integrity.validation.v1",
            E0IntegrityContracts.ValidationContract);

        var fields = typeof(E0IntegrityContracts)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(field => field.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        CollectionAssert.AreEqual(
            new[]
            {
                nameof(E0IntegrityContracts.CandidateContentIdentityContract),
                nameof(E0IntegrityContracts.ConcernEvidenceContract),
                nameof(E0IntegrityContracts.ValidationContract)
            }.OrderBy(name => name, StringComparer.Ordinal).ToArray(),
            fields);
    }

    [TestMethod]
    public void ConcernKinds_AreExactlyTheFiveFrozenE0Kinds()
    {
        CollectionAssert.AreEqual(
            new[]
            {
                nameof(IntegrityConcernKind.PotentialInaccessibleInformationUse),
                nameof(IntegrityConcernKind.PotentialProtectedInformationExposure),
                nameof(IntegrityConcernKind.PotentialLockedAuthorityViolation),
                nameof(IntegrityConcernKind.PotentialTechnicalArtifactLeak),
                nameof(IntegrityConcernKind.IndeterminateSemanticIntegrity)
            },
            Enum.GetNames<IntegrityConcernKind>());
    }

    [TestMethod]
    public void Dispositions_AreExactlyAcceptRejectAndRequestAnotherTake()
    {
        CollectionAssert.AreEqual(
            new[]
            {
                nameof(IntegrityDisposition.Accept),
                nameof(IntegrityDisposition.Reject),
                nameof(IntegrityDisposition.RequestAnotherTake)
            },
            Enum.GetNames<IntegrityDisposition>());
    }

    [TestMethod]
    public void SameStructuredContextSemanticReconstruction_ProducesSameCandidateHash()
    {
        var firstPacket = ComposeVoss();
        var reconstructedPacket = ComposeVoss();
        var firstCandidate = ParseCandidate(firstPacket, "No.");
        var reconstructedCandidate = ParseCandidate(reconstructedPacket, "No.");

        Assert.AreEqual(firstPacket.ContextPacketId, reconstructedPacket.ContextPacketId);
        Assert.AreEqual(
            IntegrityCandidateInput.Bind(firstPacket, firstCandidate).CandidateContentHash,
            IntegrityCandidateInput.Bind(reconstructedPacket, reconstructedCandidate).CandidateContentHash);
    }

    [TestMethod]
    public void CandidateContentIdentity_DoesNotClaimRenderedProviderOrAttemptIdentity()
    {
        var properties = PublicPropertyNames(typeof(IntegrityCandidateInput));

        foreach (var forbidden in new[]
                 {
                     "RenderingContract", "RenderedContextHash", "Request", "Provider", "Model",
                     "AttemptId", "RawOutput", "CandidateId", "TakeId", "CommitId"
                 })
        {
            Assert.IsFalse(properties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void IntegritySurface_HasNoSemanticAssessorTransportOrPlaywrightControlImplementation()
    {
        var integrityTypeNames = typeof(DeterministicIntegrityValidator).Assembly
            .GetTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.Integrity",
                StringComparison.Ordinal))
            .Select(type => type.Name)
            .ToArray();

        foreach (var forbidden in new[]
                 {
                     "Assessor", "Provider", "Transport", "Playwright", "RetryEngine", "CostPolicy"
                 })
        {
            Assert.IsFalse(
                integrityTypeNames.Any(name => name.Contains(forbidden, StringComparison.Ordinal)),
                forbidden);
        }
    }

    [TestMethod]
    public void FrozenContextAndFixtureIdentitiesRemainUnchanged()
    {
        var fixture = LoadMissingRaft();
        var packet = Compose(fixture, MissingRaftContract.VossId);
        var ecj1 = Ecj1FixtureCanonicalizer.Serialize(fixture);

        Assert.AreEqual(ExpectedVossStructuredHash, packet.StructuredContextHash);
        Assert.AreEqual(ExpectedVossRenderedHash, packet.RenderedContextHash);
        Assert.AreEqual(9112, ecj1.Length);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, Sha256Lower(ecj1));
    }

    private static IntegrityCandidateInput CleanInput(string text)
    {
        var packet = ComposeVoss();
        return IntegrityCandidateInput.Bind(packet, ParseCandidate(packet, text));
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

    private static string[] PublicStaticMethodNames(Type type) =>
        type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(method => method.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

    private static HashSet<string> PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

    private static ContextPacket ComposeVoss() =>
        Compose(LoadMissingRaft(), MissingRaftContract.VossId);

    private static ContextPacket ComposeMarlowe() =>
        Compose(LoadMissingRaft(), MissingRaftContract.MarloweId);

    private static ContextPacket ComposeVossVariant()
    {
        var canonical = ReadFixture("missing-raft-0.1.0.json");
        var variant = canonical.Replace(
            "The group has limited provisions.",
            "The group has very limited provisions.",
            StringComparison.Ordinal);

        if (string.Equals(canonical, variant, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Integrity test fixture variant replacement did not apply.");
        }

        return Compose(Validate(variant), MissingRaftContract.VossId);
    }

    private static ContextPacket Compose(ValidatedFixture fixture, CharacterId subject)
    {
        var projection = CharacterBoundedAccessControl.Evaluate(fixture, subject).Projection;
        return DeterministicContextComposer.Compose(projection, subject).Packet;
    }

    private static ValidatedFixture LoadMissingRaft()
    {
        var fixture = Validate(ReadFixture("missing-raft-0.1.0.json"));
        MissingRaftContract.Validate(fixture);
        return fixture;
    }

    private static ValidatedFixture Validate(string json)
    {
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        return GenericE0FixtureValidator.Validate(document);
    }

    private static string ReadFixture(string fileName) =>
        File.ReadAllText(
            Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName),
            Encoding.UTF8);

    private static string Sha256Lower(byte[] bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
}
