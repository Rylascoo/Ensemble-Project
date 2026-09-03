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
using Ensemble.E0.Core.Take;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Take;

[TestClass]
public sealed class E0TakeContractAuditTests
{
    private const string ExpectedVossStructuredHash =
        "bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b";
    private const string ExpectedVossRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";
    private const string ExpectedOracleCandidateHash =
        "18eb8c9ba9d35f34f84e1fdd983eeb2a19ce015a2e44457c4514576f984af3b2";

    [TestMethod]
    public void PublicTakeTypesAndContract_AreExactlyInCanonicalNamespace()
    {
        var takeTypes = typeof(E0Take).Assembly.GetTypes()
            .Where(type =>
                (type.IsPublic || type.IsNestedPublic) &&
                string.Equals(type.Namespace, "Ensemble.E0.Core.Take", StringComparison.Ordinal))
            .OrderBy(type => type.Name, StringComparer.Ordinal)
            .ToArray();

        CollectionAssert.AreEqual(
            new[]
            {
                nameof(E0Take),
                nameof(E0TakeContracts),
                nameof(E0TakeDisposition),
                nameof(E0TakeException)
            }.OrderBy(name => name, StringComparer.Ordinal).ToArray(),
            takeTypes.Select(type => type.Name).ToArray());
        var contractVersionField = typeof(E0TakeContracts).GetField(
            nameof(E0TakeContracts.ContractVersion),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("E0 Take contract version field is missing.");
        Assert.AreEqual("ensemble.e0.take.v1", contractVersionField.GetRawConstantValue());
    }

    [TestMethod]
    public void TakeConstructionSurface_IsSealedPrivateAndBindOnly()
    {
        Assert.IsTrue(typeof(E0Take).IsSealed);

        var constructors = typeof(E0Take)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.AreEqual(1, constructors.Length);
        Assert.IsTrue(constructors[0].IsPrivate);
        Assert.AreEqual(
            0,
            typeof(E0Take).GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);

        var operations = typeof(E0Take)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(method => !method.IsSpecialName)
            .ToArray();
        Assert.AreEqual(1, operations.Length);
        Assert.AreEqual(nameof(E0Take.Bind), operations[0].Name);
        Assert.IsTrue(operations[0].IsStatic);
        Assert.AreEqual(typeof(E0Take), operations[0].ReturnType);
    }

    [TestMethod]
    public void BindSignature_ReusesExactApprovedUpstreamTypesAndCanonicalTakeId()
    {
        var bind = typeof(E0Take).GetMethod(
            nameof(E0Take.Bind),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("E0Take.Bind is missing.");

        CollectionAssert.AreEqual(
            new[]
            {
                typeof(TakeId),
                typeof(ContextPacket),
                typeof(CandidatePerformance),
                typeof(IntegrityValidationEvaluation),
                typeof(StateInterpretationProposal),
                typeof(StateAuthorityEvaluation),
                typeof(E0TakeDisposition)
            },
            bind.GetParameters().Select(parameter => parameter.ParameterType).ToArray());

        var takeIdTypes = typeof(E0Take).Assembly.GetTypes()
            .Where(type => string.Equals(type.Name, nameof(TakeId), StringComparison.Ordinal))
            .ToArray();
        Assert.AreEqual(1, takeIdTypes.Length);
        Assert.AreEqual("Ensemble.E0.Core.Domain", takeIdTypes[0].Namespace);
    }

    [TestMethod]
    public void TakeRetainedSurface_IsExactImmutablePackageWithoutLaterAuthority()
    {
        var properties = typeof(E0Take)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public);

        CollectionAssert.AreEquivalent(
            new[]
            {
                nameof(E0Take.ContractVersion),
                nameof(E0Take.TakeId),
                nameof(E0Take.Disposition),
                nameof(E0Take.Performance),
                nameof(E0Take.InterpretationProposal),
                nameof(E0Take.AuthorityEvaluation)
            },
            properties.Select(property => property.Name).ToArray());
        Assert.IsTrue(properties.All(property => property.SetMethod is null));

        CollectionAssert.AreEqual(
            new[]
            {
                typeof(string),
                typeof(TakeId),
                typeof(E0TakeDisposition),
                typeof(CandidatePerformance),
                typeof(StateInterpretationProposal),
                typeof(StateAuthorityEvaluation)
            },
            properties
                .OrderBy(property => property.MetadataToken)
                .Select(property => property.PropertyType)
                .ToArray());

        foreach (var forbidden in new[]
                 {
                     "Context", "ContextPacket", "Integrity", "IntegrityEvaluation",
                     "StateInterpretationSource", "RunId", "Provider", "Model", "Confidence",
                     "Rationale", "Reasoning", "ChainOfThought", "CommitId", "Committed",
                     "History", "Historical", "Effective", "ProductionState", "StateHash",
                     "CurrentOpportunity", "Applied", "Persisted"
                 })
        {
            Assert.IsFalse(
                properties.Any(property => string.Equals(property.Name, forbidden, StringComparison.Ordinal)),
                forbidden);
        }
    }

    [TestMethod]
    public void TakeNamespace_ExposesNoApplicationPersistenceRoutingOrAllocationSurface()
    {
        var takeTypes = typeof(E0Take).Assembly.GetTypes()
            .Where(type => string.Equals(type.Namespace, "Ensemble.E0.Core.Take", StringComparison.Ordinal))
            .ToArray();
        var publicOperations = takeTypes
            .SelectMany(type => type.GetMethods(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Where(method => !method.IsSpecialName)
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);

        CollectionAssert.AreEquivalent(
            new[] { nameof(E0Take.Bind) },
            publicOperations.ToArray());

        foreach (var forbidden in new[]
                 {
                     "Apply", "Commit", "Persist", "Save", "Load", "Advance", "Promote",
                     "Mutate", "Rewrite", "Accept", "Reject", "Alternate", "Retry", "Spend",
                     "Allocate", "AllocateTakeId", "CreateTakeId", "GenerateTakeId",
                     "SetCurrentOpportunity", "TriggerPerformer", "CallProvider"
                 })
        {
            Assert.IsFalse(publicOperations.Contains(forbidden), forbidden);
        }

        var takeTypeNames = takeTypes.Select(type => type.Name).ToArray();
        foreach (var forbidden in new[]
                 {
                     "Commit", "ProductionState", "StateHash", "Persistence", "Repository",
                     "Provider", "Director", "Opportunity", "Retry", "Allocator", "Branch",
                     "Retcon", "Rehearsal", "WorldResolver", "WindowsAI", "Npu"
                 })
        {
            Assert.IsFalse(
                takeTypeNames.Any(name => name.Contains(forbidden, StringComparison.OrdinalIgnoreCase)),
                forbidden);
        }
    }

    [TestMethod]
    public void ExistingTakeId_HasNoNewCoreAllocatorOrContentDerivedTakeContract()
    {
        var assembly = typeof(E0Take).Assembly;
        var takeRelatedOperations = assembly.GetTypes()
            .Where(type =>
                string.Equals(type.Namespace, "Ensemble.E0.Core.Take", StringComparison.Ordinal) ||
                type == typeof(TakeId))
            .SelectMany(type => type.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Select(method => method.Name)
            .ToArray();

        foreach (var forbidden in new[]
                 {
                     "Allocate", "AllocateTakeId", "Generate", "GenerateTakeId", "NewTakeId",
                     "FromCandidateHash", "FromProposalHash", "FromRunId", "FromClock", "FromRandom"
                 })
        {
            Assert.IsFalse(
                takeRelatedOperations.Any(name => string.Equals(name, forbidden, StringComparison.Ordinal)),
                forbidden);
        }

        Assert.IsNotNull(typeof(TakeId).GetMethod(
            nameof(TakeId.From),
            BindingFlags.Public | BindingFlags.Static));
    }

    [TestMethod]
    public void TakeExceptionDomain_IsSealedCatchableAndNotPubliclyConstructible()
    {
        Assert.IsTrue(typeof(Exception).IsAssignableFrom(typeof(E0TakeException)));
        Assert.IsTrue(typeof(E0TakeException).IsSealed);
        Assert.AreEqual(
            0,
            typeof(E0TakeException)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Length);
        Assert.AreEqual(
            0,
            typeof(E0TakeException)
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Length);

        var internalConstructors = typeof(E0TakeException)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.AreEqual(2, internalConstructors.Length);
        Assert.IsTrue(internalConstructors.All(constructor => constructor.IsAssembly));
    }

    [TestMethod]
    public void BindCatchScope_NormalizesOnlyApprovedUpstreamDomainExceptions()
    {
        var bind = typeof(E0Take).GetMethod(
            nameof(E0Take.Bind),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("E0Take.Bind is missing.");
        var catchTypes = bind.GetMethodBody()!.ExceptionHandlingClauses
            .Where(clause => clause.Flags == ExceptionHandlingClauseOptions.Clause)
            .Select(clause => clause.CatchType)
            .ToArray();

        CollectionAssert.AreEquivalent(
            new[] { typeof(StateInterpretationException), typeof(StateAuthorityException) },
            catchTypes);
        Assert.IsFalse(catchTypes.Contains(typeof(Exception)));

        var takeIdGuard = typeof(E0Take).GetMethod(
            "RequireInitializedTakeId",
            BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("TakeId guard is missing.");
        var guardCatchTypes = takeIdGuard.GetMethodBody()!.ExceptionHandlingClauses
            .Where(clause => clause.Flags == ExceptionHandlingClauseOptions.Clause)
            .Select(clause => clause.CatchType)
            .ToArray();
        CollectionAssert.AreEqual(new[] { typeof(InvalidOperationException) }, guardCatchTypes);
    }

    [TestMethod]
    public void MissingRaftFrozenContextCandidateAndFixtureIdentities_RemainUnchanged()
    {
        var fixture = LoadMissingRaft();
        var packet = Compose(fixture, MissingRaftContract.VossId);
        var candidate = ParseCandidate(
            packet,
            "Wren?",
            new[] { "WREN", "MARLOWE" },
            "WREN");
        var candidateInput = IntegrityCandidateInput.Bind(packet, candidate);
        var ecj1 = Ecj1FixtureCanonicalizer.Serialize(fixture);

        Assert.AreEqual(ExpectedVossStructuredHash, packet.StructuredContextHash);
        Assert.AreEqual(
            ContextPacketId.From($"CTX:{ExpectedVossStructuredHash}"),
            packet.ContextPacketId);
        Assert.AreEqual(ExpectedVossRenderedHash, packet.RenderedContextHash);
        Assert.AreEqual(ExpectedOracleCandidateHash, candidateInput.CandidateContentHash);
        Assert.AreEqual(9112, ecj1.Length);
        Assert.AreEqual(MissingRaftContract.ExpectedFixtureHash, Sha256Lower(ecj1));
    }

    [TestMethod]
    public void FreshTakeReplay_PreservesStateAuthoritySemanticIdentityAndTraceConfiguration()
    {
        var fixture = LoadMissingRaft();
        var context = Compose(fixture, MissingRaftContract.VossId);
        var candidate = ParseCandidate(context, "No.");
        var integrity = AcceptedIntegrity(context, candidate);
        var source = StateInterpretationSource.Bind(context, candidate, integrity);
        var proposal = StateInterpretationContract.ParseJson(
            source,
            Encoding.UTF8.GetBytes(ProposalJson(
                Mutation("pressure", "add", text: "Pressure increases."))));
        var snapshot = StateAuthoritySnapshot.Bind(fixture, ImmutableArray<RecordId>.Empty);
        var input = StateAuthorityInput.Bind(snapshot, source, proposal);
        var policy = StateAuthorityPolicy.Create(
            ImmutableArray.Create(StateMutationDomain.Pressure));
        var reviewSet = StateAuthorityReviewSet.Bind(
            input,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);
        var supplied = DeterministicStateAuthority.Evaluate(input, policy, reviewSet);

        var take = E0Take.Bind(
            TakeId.From("TAKE-AUTHORITY-IDENTITY"),
            context,
            candidate,
            integrity,
            proposal,
            supplied,
            E0TakeDisposition.Accepted);

        Assert.AreNotSame(supplied, take.AuthorityEvaluation);
        Assert.AreNotSame(supplied.Trace.Input, take.AuthorityEvaluation.Trace.Input);
        Assert.AreSame(supplied.Trace.Input.Snapshot, take.AuthorityEvaluation.Trace.Input.Snapshot);
        Assert.AreSame(supplied.Trace.Policy, take.AuthorityEvaluation.Trace.Policy);
        Assert.AreSame(supplied.Trace.ReviewSet, take.AuthorityEvaluation.Trace.ReviewSet);
        Assert.AreEqual(
            supplied.Trace.Input.ProposalContentIdentityContract,
            take.AuthorityEvaluation.Trace.Input.ProposalContentIdentityContract);
        Assert.AreEqual(
            supplied.Trace.Input.ProposalContentHash,
            take.AuthorityEvaluation.Trace.Input.ProposalContentHash);
        CollectionAssert.AreEqual(
            AuthoritySignature(supplied),
            AuthoritySignature(take.AuthorityEvaluation));
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

    private static string ProposalJson(params Dictionary<string, object?>[] mutations) =>
        JsonSerializer.Serialize(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations
        });

    private static string[] AuthoritySignature(StateAuthorityEvaluation evaluation) =>
        evaluation.Decisions
            .Select(decision =>
                $"{decision.MutationIndex}|{decision.Disposition}|{string.Join(",", decision.Reasons)}")
            .Prepend(evaluation.Status.ToString())
            .ToArray();

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

    private static string Sha256Lower(byte[] bytes) =>
        Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
}
