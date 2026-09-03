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
using Ensemble.E0.Core.StateInterpreter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.StateInterpreter;

[TestClass]
public sealed class StateInterpretationContractTests
{
    private const string ExpectedVossStructuredHash =
        "bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b";
    private const string ExpectedVossRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const string ExpectedOracleCandidateHash =
        "18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2";

    [TestMethod]
    public void SourceBind_CanonicalVossAcceptProducesLeastPrivilegeSource()
    {
        var pipeline = AcceptedPipeline("No.");

        Assert.AreEqual(
            E0IntegrityContracts.CandidateContentIdentityContract,
            pipeline.Source.CandidateContentIdentityContract);
        Assert.AreEqual(
            IntegrityCandidateInput.Bind(pipeline.Packet, pipeline.Candidate).CandidateContentHash,
            pipeline.Source.CandidateContentHash);
        Assert.AreEqual(pipeline.Packet.SceneId, pipeline.Source.SourceSceneId);
        Assert.AreEqual(pipeline.Packet.SubjectCharacterId, pipeline.Source.SourceCharacterId);
        Assert.AreEqual(pipeline.Packet.ContextPacketId, pipeline.Source.SourceContextPacketId);
    }

    [TestMethod]
    public void SourceSurface_ContainsOnlyApprovedStructuralFields()
    {
        CollectionAssert.AreEquivalent(
            new[]
            {
                "CandidateContentIdentityContract",
                "CandidateContentHash",
                "SourceSceneId",
                "SourceCharacterId",
                "SourceContextPacketId",
                "RosterCharacterIds"
            },
            PublicPropertyNames(typeof(StateInterpretationSource)).ToArray());
    }

    [TestMethod]
    public void SourceConstructionAndApi_AreNarrowAndNonForgeable()
    {
        Assert.AreEqual(
            0,
            typeof(StateInterpretationSource)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);

        CollectionAssert.AreEqual(
            new[] { nameof(StateInterpretationSource.Bind) },
            PublicStaticMethodNames(typeof(StateInterpretationSource)));

        var bind = typeof(StateInterpretationSource).GetMethod(
            nameof(StateInterpretationSource.Bind),
            BindingFlags.Public | BindingFlags.Static)!;
        CollectionAssert.AreEqual(
            new[]
            {
                typeof(ContextPacket),
                typeof(CandidatePerformance),
                typeof(IntegrityValidationEvaluation)
            },
            bind.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
    }

    [TestMethod]
    public void SourceBind_RequestAnotherTakeEvaluationFails()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "No.");
        var input = IntegrityCandidateInput.Bind(packet, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray.Create(IntegrityConcernKind.IndeterminateSemanticIntegrity));
        var evaluation = DeterministicIntegrityValidator.Validate(input, evidence);

        Assert.AreEqual(IntegrityDisposition.RequestAnotherTake, evaluation.Disposition);
        Assert.Throws<StateInterpretationException>(() =>
            StateInterpretationSource.Bind(packet, candidate, evaluation));
    }

    [TestMethod]
    public void SourceBind_EvaluationForDifferentCandidateFails()
    {
        var packet = ComposeVoss();
        var firstCandidate = ParseCandidate(packet, "First.");
        var secondCandidate = ParseCandidate(packet, "Second.");
        var firstEvaluation = Accept(packet, firstCandidate);

        Assert.Throws<StateInterpretationException>(() =>
            StateInterpretationSource.Bind(packet, secondCandidate, firstEvaluation));
    }

    [TestMethod]
    public void SourceBind_UnsupportedIntegrityValidationContractFails()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "No.");
        var input = IntegrityCandidateInput.Bind(packet, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);

        var traceConstructor = typeof(IntegrityValidationTrace)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        var trace = (IntegrityValidationTrace)(
            traceConstructor.Invoke(
                new object?[] { "unsupported.integrity.contract", input, evidence }) ??
            throw new InvalidOperationException(
                "IntegrityValidationTrace reflection construction returned null."));

        var evaluationConstructor = typeof(IntegrityValidationEvaluation)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();
        var evaluation = (IntegrityValidationEvaluation)(
            evaluationConstructor.Invoke(
                new object[] { IntegrityDisposition.Accept, trace }) ??
            throw new InvalidOperationException(
                "IntegrityValidationEvaluation reflection construction returned null."));

        Assert.Throws<StateInterpretationException>(() =>
            StateInterpretationSource.Bind(packet, candidate, evaluation));
    }

    [TestMethod]
    public void SourceRoster_IsCanonicalOrdinalAndExactlyThree()
    {
        var source = AcceptedPipeline("No.").Source;

        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "VOSS", "WREN" },
            source.RosterCharacterIds.Select(id => id.Value).ToArray());
    }

    [TestMethod]
    public void SourceSurface_ExcludesProseIntegrityAuthorityAndProviderFields()
    {
        var properties = PublicPropertyNames(typeof(StateInterpretationSource));
        foreach (var forbidden in new[]
                 {
                     "VisibleText", "Context", "Rendered", "IntegrityEvaluation",
                     "ValidationContract", "ConcernEvidence", "Provider", "Model",
                     "TakeId", "CommitId", "State", "Authoritative", "Authenticated"
                 })
        {
            Assert.IsFalse(properties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void Parse_EmptyMutationListIsValidAndCopiesTrustedIdentity()
    {
        var source = AcceptedPipeline("No.").Source;
        var proposal = Parse(source, ProposalJson());

        Assert.AreEqual(0, proposal.Mutations.Length);
        Assert.AreEqual(source.CandidateContentIdentityContract, proposal.CandidateContentIdentityContract);
        Assert.AreEqual(source.CandidateContentHash, proposal.CandidateContentHash);
        Assert.AreEqual(source.SourceSceneId, proposal.SourceSceneId);
    }

    [TestMethod]
    public void ProposalSurface_IsExactAndConstructorNonPublic()
    {
        CollectionAssert.AreEquivalent(
            new[]
            {
                "ContractVersion",
                "CandidateContentIdentityContract",
                "CandidateContentHash",
                "SourceSceneId",
                "Mutations"
            },
            PublicPropertyNames(typeof(StateInterpretationProposal)).ToArray());
        Assert.AreEqual(
            0,
            typeof(StateInterpretationProposal)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);
    }

    [TestMethod]
    public void Parse_RepresentativeCharacterBeliefAddProducesTypedSemanticMutation()
    {
        var source = AcceptedPipeline("No.").Source;
        var mutation = Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Voss believes Wren may know more.",
            supportingRecordIds: new[] { "REC-B", "REC-A" });

        var proposal = Parse(source, ProposalJson(mutation));
        var result = (MutableCharacterStateMutationCandidate)proposal.Mutations.Single();
        var change = (AddStateMutationChange)result.Change;

        Assert.AreEqual(StateMutationDomain.CharacterBelief, result.Domain);
        Assert.AreEqual("VOSS", result.SubjectCharacterId.Value);
        Assert.AreEqual("Voss believes Wren may know more.", change.Text);
        CollectionAssert.AreEqual(
            new[] { "REC-A", "REC-B" },
            result.SupportingRecordIds.Select(id => id.Value).ToArray());
    }

    [TestMethod]
    public void Parse_ModelAuthoredCandidateOrSceneIdentityFieldsAreRejected()
    {
        var source = AcceptedPipeline("No.").Source;
        var json = JsonSerializer.Serialize(new Dictionary<string, object?>
        {
            ["schemaVersion"] = StateInterpretationContract.JsonSchemaVersion,
            ["candidateContentHash"] = source.CandidateContentHash,
            ["sourceSceneId"] = source.SourceSceneId.Value,
            ["mutations"] = Array.Empty<object>()
        });

        Assert.Throws<StateInterpretationException>(() => Parse(source, json));
    }

    [TestMethod]
    public void Parse_PropertyOrderAndWhitespaceAreInsignificant()
    {
        var source = AcceptedPipeline("No.").Source;
        var json =
            "{  \"mutations\" : [ ], \"schemaVersion\" : \"" +
            StateInterpretationContract.JsonSchemaVersion +
            "\" }";

        Assert.AreEqual(0, Parse(source, json).Mutations.Length);
    }

    [TestMethod]
    public void Parse_UnknownRootPropertyFailsWithoutEchoingName()
    {
        var source = AcceptedPipeline("No.").Source;
        var json =
            "{\"schemaVersion\":\"" + StateInterpretationContract.JsonSchemaVersion +
            "\",\"mutations\":[],\"SECRET_UNKNOWN_PROPERTY\":1}";

        var exception = Assert.Throws<StateInterpretationException>(() => Parse(source, json));
        Assert.IsFalse(exception.Message.Contains("SECRET_UNKNOWN_PROPERTY", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Parse_MissingRequiredRootPropertyFails()
    {
        var source = AcceptedPipeline("No.").Source;
        var json = "{\"schemaVersion\":\"" +
                   StateInterpretationContract.JsonSchemaVersion + "\"}";

        Assert.Throws<StateInterpretationException>(() => Parse(source, json));
    }

    [TestMethod]
    public void Parse_DecodedDuplicatePropertyFails()
    {
        var source = AcceptedPipeline("No.").Source;
        var json =
            "{\"schemaVersion\":\"" + StateInterpretationContract.JsonSchemaVersion +
            "\",\"\\u006dutations\":[],\"mutations\":[]}";

        Assert.Throws<StateInterpretationException>(() => Parse(source, json));
    }

    [TestMethod]
    public void Parse_WrongMutationsTypeFails()
    {
        var source = AcceptedPipeline("No.").Source;
        var json =
            "{\"schemaVersion\":\"" + StateInterpretationContract.JsonSchemaVersion +
            "\",\"mutations\":{}}";

        Assert.Throws<StateInterpretationException>(() => Parse(source, json));
    }

    [TestMethod]
    public void Parse_ContentAfterRootFails()
    {
        var source = AcceptedPipeline("No.").Source;
        Assert.Throws<StateInterpretationException>(() =>
            Parse(source, ProposalJson() + "{}"));
    }

    [TestMethod]
    public void Parse_CommentsFail()
    {
        var source = AcceptedPipeline("No.").Source;
        var json = ProposalJson().Insert(1, "/*comment*/");

        Assert.Throws<StateInterpretationException>(() => Parse(source, json));
    }

    [TestMethod]
    public void Parse_TrailingCommaFails()
    {
        var source = AcceptedPipeline("No.").Source;
        var json =
            "{\"schemaVersion\":\"" + StateInterpretationContract.JsonSchemaVersion +
            "\",\"mutations\":[],}";

        Assert.Throws<StateInterpretationException>(() => Parse(source, json));
    }

    [TestMethod]
    public void Parse_Utf8BomFails()
    {
        var source = AcceptedPipeline("No.").Source;
        var body = Encoding.UTF8.GetBytes(ProposalJson());
        var bytes = new byte[body.Length + 3];
        bytes[0] = 0xEF;
        bytes[1] = 0xBB;
        bytes[2] = 0xBF;
        body.CopyTo(bytes, 3);

        Assert.Throws<StateInterpretationException>(() =>
            StateInterpretationContract.ParseJson(source, bytes));
    }

    [TestMethod]
    public void Parse_OneMiBInclusiveSucceedsAndPlusOneFails()
    {
        var source = AcceptedPipeline("No.").Source;
        var minimal = ProposalJson();
        var minimalBytes = Encoding.UTF8.GetByteCount(minimal);
        var padding = StateInterpretationContract.MaxProposalJsonBytes - minimalBytes;
        var exact = minimal.Insert(minimal.Length - 1, new string(' ', padding));
        var exactBytes = Encoding.UTF8.GetBytes(exact);

        Assert.AreEqual(StateInterpretationContract.MaxProposalJsonBytes, exactBytes.Length);
        Assert.AreEqual(
            0,
            StateInterpretationContract.ParseJson(source, exactBytes).Mutations.Length);

        var tooLarge = exact.Insert(exact.Length - 1, " ");
        Assert.Throws<StateInterpretationException>(() =>
            StateInterpretationContract.ParseJson(source, Encoding.UTF8.GetBytes(tooLarge)));
    }

    [TestMethod]
    public void Parse_DepthGreaterThanEightFails()
    {
        var source = AcceptedPipeline("No.").Source;
        var json =
            "{\"schemaVersion\":\"" + StateInterpretationContract.JsonSchemaVersion +
            "\",\"mutations\":[[[[[[[[[{}]]]]]]]]]}";

        Assert.Throws<StateInterpretationException>(() => Parse(source, json));
    }

    [TestMethod]
    public void Parse_FailureDoesNotEchoCreativeText()
    {
        var source = AcceptedPipeline("No.").Source;
        var mutation = Mutation(
            "characterBelief",
            "replace",
            subjectCharacterId: "VOSS",
            text: "UNIQUE_SECRET_CREATIVE_TEXT");

        var exception = Assert.Throws<StateInterpretationException>(() =>
            Parse(source, ProposalJson(mutation)));
        Assert.IsFalse(
            exception.Message.Contains("UNIQUE_SECRET_CREATIVE_TEXT", StringComparison.Ordinal));
    }

    [TestMethod]
    public void SemanticConstructors_AreNonPublic()
    {
        foreach (var type in new[]
                 {
                     typeof(StateInterpretationProposal),
                     typeof(StateInterpretationSource),
                     typeof(AddStateMutationChange),
                     typeof(SupersedeStateMutationChange),
                     typeof(DeactivateStateMutationChange),
                     typeof(GlobalStateMutationCandidate),
                     typeof(AppendOnlyCharacterStateMutationCandidate),
                     typeof(MutableCharacterStateMutationCandidate),
                     typeof(CharacterClaimMutationCandidate),
                     typeof(RelationshipStateMutationCandidate)
                 })
        {
            Assert.AreEqual(
                0,
                type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length,
                type.Name);
        }
    }

    [TestMethod]
    public void SemanticChangeShapes_MakeInvalidOperationPayloadsUnavailable()
    {
        CollectionAssert.AreEquivalent(
            new[] { "Text" },
            PublicPropertyNames(typeof(AddStateMutationChange)).ToArray());
        CollectionAssert.AreEquivalent(
            new[] { "ExistingRecordId", "Text" },
            PublicPropertyNames(typeof(SupersedeStateMutationChange)).ToArray());
        CollectionAssert.AreEquivalent(
            new[] { "ExistingRecordId" },
            PublicPropertyNames(typeof(DeactivateStateMutationChange)).ToArray());
    }

    [TestMethod]
    public void GlobalMutationShape_HasNoCharacterIdentity()
    {
        var mutation = ParseSingle(
            Mutation("worldState", "add", text: "The hatch is open."));

        Assert.IsInstanceOfType(mutation, typeof(GlobalStateMutationCandidate));
        var properties = PublicPropertyNames(mutation.GetType());
        Assert.IsFalse(properties.Contains("SubjectCharacterId"));
        Assert.IsFalse(properties.Contains("TargetCharacterId"));
    }

    [TestMethod]
    public void AppendOnlyCharacterShape_ExposesAddChangeOnly()
    {
        var mutation = (AppendOnlyCharacterStateMutationCandidate)ParseSingle(
            Mutation(
                "characterMemory",
                "add",
                subjectCharacterId: "VOSS",
                text: "Voss remembers the exchange."));

        Assert.IsInstanceOfType(mutation.Change, typeof(AddStateMutationChange));
        Assert.AreEqual("VOSS", mutation.SubjectCharacterId.Value);
    }

    [TestMethod]
    public void MutableCharacterShape_HasSubjectAndNoTarget()
    {
        var mutation = (MutableCharacterStateMutationCandidate)ParseSingle(
            Mutation(
                "characterGoal",
                "add",
                subjectCharacterId: "WREN",
                text: "Wren wants an explanation."));

        Assert.AreEqual("WREN", mutation.SubjectCharacterId.Value);
        Assert.IsFalse(PublicPropertyNames(mutation.GetType()).Contains("TargetCharacterId"));
    }

    [TestMethod]
    public void RelationshipShape_RequiresSubjectAndTarget()
    {
        var mutation = (RelationshipStateMutationCandidate)ParseSingle(
            Mutation(
                "relationship",
                "add",
                subjectCharacterId: "VOSS",
                targetCharacterId: "WREN",
                text: "Voss becomes more attentive to Wren."));

        Assert.AreEqual("VOSS", mutation.SubjectCharacterId.Value);
        Assert.AreEqual("WREN", mutation.TargetCharacterId.Value);
    }

    [TestMethod]
    public void CharacterClaimShape_IsSourceAddOnlyPropositionWithoutChangeObject()
    {
        var mutation = (CharacterClaimMutationCandidate)ParseSingle(
            Mutation(
                "characterClaim",
                "add",
                subjectCharacterId: "VOSS",
                text: "Voss claims the loss was accidental."));

        Assert.AreEqual("VOSS", mutation.SubjectCharacterId.Value);
        Assert.AreEqual("Voss claims the loss was accidental.", mutation.Text);
        Assert.IsFalse(PublicPropertyNames(mutation.GetType()).Contains("Change"));
        Assert.IsFalse(PublicPropertyNames(mutation.GetType()).Contains("ExistingRecordId"));
    }

    [TestMethod]
    [DataRow("worldState")]
    [DataRow("sceneState")]
    [DataRow("unresolvedProposition")]
    [DataRow("characterKnowledge")]
    [DataRow("characterBelief")]
    [DataRow("characterSuspicion")]
    [DataRow("characterMemory")]
    [DataRow("characterGoal")]
    [DataRow("characterDisposition")]
    [DataRow("characterCircumstance")]
    [DataRow("characterClaim")]
    [DataRow("relationship")]
    [DataRow("pressure")]
    public void AllApprovedE0Domains_ParseUnderTheirExactShape(string domain)
    {
        var mutation = ValidMutationForDomain(domain);
        var result = ParseSingle(mutation);

        Assert.AreEqual(domain, DomainTransportName(result.Domain));
    }

    [TestMethod]
    [DataRow("characterKnowledge", "supersede")]
    [DataRow("characterKnowledge", "deactivate")]
    [DataRow("characterMemory", "supersede")]
    [DataRow("characterMemory", "deactivate")]
    public void KnowledgeAndMemory_RejectSupersedeAndDeactivate(string domain, string operation)
    {
        var mutation = MutationForOperation(
            domain,
            operation,
            subjectCharacterId: "VOSS");

        Assert.Throws<StateInterpretationException>(() => ParseSingle(mutation));
    }

    [TestMethod]
    [DataRow("constitution")]
    [DataRow("historicalTruth")]
    [DataRow("observation")]
    [DataRow("presentationPerspective")]
    [DataRow("directorOpportunity")]
    [DataRow("canon")]
    public void AbsentAuthorityDomains_AreRejected(string domain)
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(domain, "add", text: "Not allowed.")));
    }

    [TestMethod]
    public void Relationship_NonRosterCharacterFails()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "relationship",
                "add",
                subjectCharacterId: "VOSS",
                targetCharacterId: "OUTSIDER",
                text: "Not allowed.")));
    }

    [TestMethod]
    public void Relationship_SelfTargetFails()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "relationship",
                "add",
                subjectCharacterId: "VOSS",
                targetCharacterId: "VOSS",
                text: "Not allowed.")));
    }

    [TestMethod]
    public void CharacterClaim_NonSourceCharacterFails()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "characterClaim",
                "add",
                subjectCharacterId: "WREN",
                text: "Wren allegedly claimed something.")));
    }

    [TestMethod]
    public void CharacterClaim_NonAddOperationFails()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(MutationForOperation(
                "characterClaim",
                "supersede",
                subjectCharacterId: "VOSS")));
    }

    [TestMethod]
    [DataRow("delete")]
    [DataRow("replace")]
    public void DeleteAndReplaceOperations_AreRejected(string operation)
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "worldState",
                operation,
                existingRecordId: "REC-A",
                text: "Not allowed.")));
    }

    [TestMethod]
    public void ExistingRecordId_SyntaxIsValidatedWithoutClaimingExistence()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "worldState",
                "supersede",
                existingRecordId: " BAD ",
                text: "Replacement proposal.")));

        var valid = (GlobalStateMutationCandidate)ParseSingle(Mutation(
            "worldState",
            "supersede",
            existingRecordId: "NOT-KNOWN-TO-EXIST",
            text: "Syntactically valid proposal reference."));
        Assert.AreEqual(
            "NOT-KNOWN-TO-EXIST",
            ((SupersedeStateMutationChange)valid.Change).ExistingRecordId.Value);
    }

    [TestMethod]
    public void SupportingRecordIds_DuplicateFails()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "worldState",
                "add",
                text: "Proposed.",
                supportingRecordIds: new[] { "REC-A", "REC-A" })));
    }

    [TestMethod]
    public void SupportingRecordIds_AreCanonicalOrdinal()
    {
        var result = ParseSingle(Mutation(
            "worldState",
            "add",
            text: "Proposed.",
            supportingRecordIds: new[] { "REC-C", "REC-A", "REC-B" }));

        CollectionAssert.AreEqual(
            new[] { "REC-A", "REC-B", "REC-C" },
            result.SupportingRecordIds.Select(id => id.Value).ToArray());
    }

    [TestMethod]
    public void ExactDuplicateMutation_FailsAfterSupportingIdCanonicalization()
    {
        var first = Mutation(
            "worldState",
            "add",
            text: "Same proposal.",
            supportingRecordIds: new[] { "REC-B", "REC-A" });
        var second = Mutation(
            "worldState",
            "add",
            text: "Same proposal.",
            supportingRecordIds: new[] { "REC-A", "REC-B" });

        Assert.Throws<StateInterpretationException>(() =>
            Parse(AcceptedPipeline("No.").Source, ProposalJson(first, second)));
    }

    [TestMethod]
    public void NonIdenticalConflictingMutations_RemainRepresentableForStateAuthority()
    {
        var source = AcceptedPipeline("No.").Source;
        var first = Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Voss believes A.");
        var second = Mutation(
            "characterBelief",
            "add",
            subjectCharacterId: "VOSS",
            text: "Voss believes B.");

        Assert.AreEqual(2, Parse(source, ProposalJson(first, second)).Mutations.Length);
    }

    [TestMethod]
    public void MutationArrayOrder_IsPreservedWithoutPriorityField()
    {
        var source = AcceptedPipeline("No.").Source;
        var proposal = Parse(
            source,
            ProposalJson(
                Mutation("worldState", "add", text: "First."),
                Mutation("worldState", "add", text: "Second.")));

        var first = (GlobalStateMutationCandidate)proposal.Mutations[0];
        var second = (GlobalStateMutationCandidate)proposal.Mutations[1];
        Assert.AreEqual("First.", ((AddStateMutationChange)first.Change).Text);
        Assert.AreEqual("Second.", ((AddStateMutationChange)second.Change).Text);
        Assert.IsFalse(PublicPropertyNames(typeof(StateMutationCandidate)).Contains("Priority"));
    }

    [TestMethod]
    public void MutationText_IsPreservedExactlyWithoutTrimOrRewrite()
    {
        const string text = "  Exact wording\nwith a second line.  ";
        var mutation = (GlobalStateMutationCandidate)ParseSingle(
            Mutation("worldState", "add", text: text));

        Assert.AreEqual(text, ((AddStateMutationChange)mutation.Change).Text);
    }

    [TestMethod]
    [DataRow("e\u0301")]
    [DataRow("\u0001visible")]
    [DataRow("\u200B")]
    public void InvalidMutationText_NonNfcForbiddenControlOrInvisibleOnlyFails(string text)
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation("worldState", "add", text: text)));
    }

    [TestMethod]
    public void ChangeOperationShapes_EnforceNullabilityExactly()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "worldState",
                "add",
                existingRecordId: "REC-A",
                text: "Add cannot carry existing ID.")));
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "worldState",
                "supersede",
                text: "Supersede requires ID.")));
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "worldState",
                "deactivate",
                existingRecordId: "REC-A",
                text: "Deactivate cannot carry text.")));

        var deactivate = (GlobalStateMutationCandidate)ParseSingle(Mutation(
            "worldState",
            "deactivate",
            existingRecordId: "REC-A",
            text: null));
        Assert.IsInstanceOfType(deactivate.Change, typeof(DeactivateStateMutationChange));
    }

    [TestMethod]
    public void GlobalDomain_WithCharacterIdentityFails()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "worldState",
                "add",
                subjectCharacterId: "VOSS",
                text: "Not allowed.")));
    }

    [TestMethod]
    public void CharacterDomain_WithTargetIdentityFails()
    {
        Assert.Throws<StateInterpretationException>(() =>
            ParseSingle(Mutation(
                "characterBelief",
                "add",
                subjectCharacterId: "VOSS",
                targetCharacterId: "WREN",
                text: "Not allowed.")));
    }

    [TestMethod]
    public void PublicParserSurface_HasNoApplyTakeCommitProviderOrRetryOperation()
    {
        CollectionAssert.AreEqual(
            new[] { nameof(StateInterpretationContract.ParseJson) },
            PublicStaticMethodNames(typeof(StateInterpretationContract)));

        var properties = PublicPropertyNames(typeof(StateInterpretationProposal));
        foreach (var forbidden in new[]
                 {
                     "Accepted", "Disposition", "TakeId", "CommitId", "Provider", "Model",
                     "Confidence", "Rationale", "Priority", "CurrentOpportunity", "StateHash"
                 })
        {
            Assert.IsFalse(properties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void StateInterpreterNamespace_HasNoStateAuthorityTakeProviderPersistenceOrWorldResolverType()
    {
        var names = typeof(StateInterpretationContract).Assembly
            .GetTypes()
            .Where(type => string.Equals(
                type.Namespace,
                "Ensemble.E0.Core.StateInterpreter",
                StringComparison.Ordinal))
            .Select(type => type.Name)
            .ToArray();

        foreach (var forbidden in new[]
                 {
                     "StateAuthority", "ProductionState", "Provider", "Retry", "Persistence",
                     "WorldResolver", "TakeManager", "CommitAuthority"
                 })
        {
            Assert.IsFalse(
                names.Any(name => name.Contains(forbidden, StringComparison.Ordinal)),
                forbidden);
        }
    }

    [TestMethod]
    public void CandidateContentHashRegression_OracleRemainsUnchanged()
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

    private static StateMutationCandidate ParseSingle(Dictionary<string, object?> mutation)
    {
        var source = AcceptedPipeline("No.").Source;
        return Parse(source, ProposalJson(mutation)).Mutations.Single();
    }

    private static StateInterpretationProposal Parse(
        StateInterpretationSource source,
        string json) =>
        StateInterpretationContract.ParseJson(source, Encoding.UTF8.GetBytes(json));

    private static (
        ContextPacket Packet,
        CandidatePerformance Candidate,
        IntegrityValidationEvaluation Evaluation,
        StateInterpretationSource Source) AcceptedPipeline(string text)
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, text);
        var evaluation = Accept(packet, candidate);
        var source = StateInterpretationSource.Bind(packet, candidate, evaluation);
        return (packet, candidate, evaluation, source);
    }

    private static IntegrityValidationEvaluation Accept(
        ContextPacket packet,
        CandidatePerformance candidate)
    {
        var input = IntegrityCandidateInput.Bind(packet, candidate);
        var evidence = IntegrityConcernEvidence.Bind(
            input,
            ImmutableArray<IntegrityConcernKind>.Empty);
        return DeterministicIntegrityValidator.Validate(input, evidence);
    }

    private static Dictionary<string, object?> ValidMutationForDomain(string domain) =>
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
            _ => throw new InvalidOperationException("Unsupported test domain.")
        };

    private static Dictionary<string, object?> MutationForOperation(
        string domain,
        string operation,
        string? subjectCharacterId = null)
    {
        var existingRecordId = operation == "add" ? null : "REC-A";
        var text = operation == "deactivate" ? null : "Proposed state.";
        return Mutation(
            domain,
            operation,
            subjectCharacterId: subjectCharacterId,
            existingRecordId: existingRecordId,
            text: text);
    }

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

    private static string ProposalJson(
        params Dictionary<string, object?>[] mutations) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations
        });

    private static string DomainTransportName(StateMutationDomain domain) =>
        domain switch
        {
            StateMutationDomain.WorldState => "worldState",
            StateMutationDomain.SceneState => "sceneState",
            StateMutationDomain.UnresolvedProposition => "unresolvedProposition",
            StateMutationDomain.CharacterKnowledge => "characterKnowledge",
            StateMutationDomain.CharacterBelief => "characterBelief",
            StateMutationDomain.CharacterSuspicion => "characterSuspicion",
            StateMutationDomain.CharacterMemory => "characterMemory",
            StateMutationDomain.CharacterGoal => "characterGoal",
            StateMutationDomain.CharacterDisposition => "characterDisposition",
            StateMutationDomain.CharacterCircumstance => "characterCircumstance",
            StateMutationDomain.CharacterClaim => "characterClaim",
            StateMutationDomain.Relationship => "relationship",
            StateMutationDomain.Pressure => "pressure",
            _ => throw new InvalidOperationException("Unsupported semantic domain.")
        };

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
