using System.Collections.Immutable;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Performer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Director;

[TestClass]
public sealed class LeastInterventionDirectorTests
{
    private const string ExpectedVossStructuredHash =
        "bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b";
    private const string ExpectedVossRenderedHash =
        "ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88";

    [TestMethod]
    public void MissingRaftOpeningOpportunity_RemainsFixtureVoss()
    {
        var packet = ComposeVoss();

        Assert.AreEqual(MissingRaftContract.VossId, packet.OpportunityCharacterId);
        Assert.AreEqual(MissingRaftContract.VossId, packet.SubjectCharacterId);
    }

    [TestMethod]
    public void Bind_ValidVossInputCopiesOnlyStructuralDirectorValues()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(
            packet,
            "Wren?",
            new[] { "MARLOWE", "WREN" },
            "WREN");
        var history = History("VOSS");

        var input = DirectorOpportunityInput.Bind(packet, candidate, history);

        Assert.AreEqual(packet.SceneId, input.SceneId);
        Assert.AreEqual(packet.SubjectCharacterId, input.SourceCharacterId);
        Assert.AreEqual(packet.ContextPacketId, input.SourceContextPacketId);
        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "VOSS", "WREN" },
            Values(input.RosterCharacterIds));
        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "WREN" },
            Values(input.AddressedCharacterIds));
        Assert.AreEqual("WREN", input.NominatedCharacterId!.Value.Value);
        CollectionAssert.AreEqual(new[] { "VOSS" }, Values(input.OpportunityHistory));
    }

    [TestMethod]
    public void Bind_IsStructuralNotCausalAuthenticationSurface()
    {
        var input = Bind(
            ComposeVoss(),
            ParseCandidate(ComposeVoss(), "Wren?", nominatedCharacterId: "WREN"),
            History("MARLOWE", "WREN", "VOSS"));

        var propertyNames = PublicPropertyNames(typeof(DirectorOpportunityInput));

        Assert.IsFalse(propertyNames.Contains("Authoritative"));
        Assert.IsFalse(propertyNames.Contains("Authenticated"));
        Assert.IsFalse(propertyNames.Contains("Committed"));
        Assert.IsFalse(propertyNames.Contains("TakeId"));
        Assert.IsFalse(propertyNames.Contains("CommitId"));
        Assert.AreEqual("VOSS", input.OpportunityHistory[^1].Value);
    }

    [TestMethod]
    public void Bind_ContextAndCandidateSubjectMismatchFails()
    {
        var vossPacket = ComposeVoss();
        var vossCandidate = ParseCandidate(vossPacket, "No.");
        var marlowePacket = ComposeMarlowe();

        Assert.Throws<DirectorOpportunityException>(() =>
            DirectorOpportunityInput.Bind(
                marlowePacket,
                vossCandidate,
                History("MARLOWE")));
    }

    [TestMethod]
    public void Bind_ContextPacketIdentityMismatchFailsUsingIndependentValidPackets()
    {
        var originalPacket = ComposeVoss();
        var originalCandidate = ParseCandidate(originalPacket, "No.");
        var variantPacket = ComposeVossVariant();

        Assert.AreEqual(originalPacket.SubjectCharacterId, variantPacket.SubjectCharacterId);
        Assert.AreNotEqual(originalPacket.ContextPacketId, variantPacket.ContextPacketId);
        Assert.Throws<DirectorOpportunityException>(() =>
            DirectorOpportunityInput.Bind(
                variantPacket,
                originalCandidate,
                History("VOSS")));
    }

    [TestMethod]
    public void Bind_HistoryMustBeInitializedNonEmptyRosterBoundAndEndAtSource()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "No.");

        Assert.Throws<DirectorOpportunityException>(() =>
            DirectorOpportunityInput.Bind(packet, candidate, default));
        Assert.Throws<DirectorOpportunityException>(() =>
            DirectorOpportunityInput.Bind(packet, candidate, ImmutableArray<CharacterId>.Empty));
        Assert.Throws<DirectorOpportunityException>(() =>
            DirectorOpportunityInput.Bind(packet, candidate, History("CHAR-X", "VOSS")));
        Assert.Throws<DirectorOpportunityException>(() =>
            DirectorOpportunityInput.Bind(packet, candidate, History("VOSS", "WREN")));
    }

    [TestMethod]
    public void Bind_PreservesExactOpportunityEventOrderIncludingRepeatedCharacterIds()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "No.");
        var history = History("MARLOWE", "VOSS", "VOSS");

        var input = DirectorOpportunityInput.Bind(packet, candidate, history);

        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "VOSS", "VOSS" },
            Values(input.OpportunityHistory));
    }

    [TestMethod]
    public void DirectorInputAndOutputAuthorityTypes_HaveNoPublicConstructors()
    {
        foreach (var type in new[]
                 {
                     typeof(DirectorOpportunityInput),
                     typeof(DirectorOpportunityProposal),
                     typeof(LeastInterventionDirectorEvaluation),
                     typeof(LeastInterventionDirectorTrace)
                 })
        {
            Assert.AreEqual(
                0,
                type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length,
                type.FullName);
        }
    }

    [TestMethod]
    public void DirectorInputSurface_IsLeastPrivilegeAndContainsNoRichContentOrAuthorityFlags()
    {
        var properties = PublicPropertyNames(typeof(DirectorOpportunityInput));

        CollectionAssert.AreEquivalent(
            new[]
            {
                "SceneId",
                "SourceCharacterId",
                "SourceContextPacketId",
                "RosterCharacterIds",
                "AddressedCharacterIds",
                "NominatedCharacterId",
                "OpportunityHistory"
            },
            properties.ToArray());

        foreach (var forbidden in new[]
                 {
                     "VisibleText", "Rendered", "RenderedContextHash", "RenderingContract",
                     "Knowledge", "Beliefs", "Suspicions", "Memories", "Goals", "Relationships",
                     "Pressures", "AccessDecisions", "Provenance", "WorldState", "Truth",
                     "Provider", "Model", "TakeId", "CommitId", "Authoritative", "Authenticated"
                 })
        {
            Assert.IsFalse(properties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void DirectorPublicConstructionAndStrategySurface_IsNarrow()
    {
        var bindMethods = typeof(DirectorOpportunityInput)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(method => method.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var strategyMethods = typeof(LeastInterventionDirector)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(method => method.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var propose = typeof(LeastInterventionDirector).GetMethod(
            nameof(LeastInterventionDirector.Propose),
            BindingFlags.Public | BindingFlags.Static)!;

        CollectionAssert.AreEqual(new[] { nameof(DirectorOpportunityInput.Bind) }, bindMethods);
        CollectionAssert.AreEqual(new[] { nameof(LeastInterventionDirector.Propose) }, strategyMethods);
        CollectionAssert.AreEqual(
            new[] { typeof(DirectorOpportunityInput) },
            propose.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        Assert.AreEqual(typeof(LeastInterventionDirectorEvaluation), propose.ReturnType);
    }

    [TestMethod]
    public void SemanticProposalSurface_IsStrategyNeutralAndE0Singular()
    {
        var properties = PublicPropertyNames(typeof(DirectorOpportunityProposal));

        CollectionAssert.AreEquivalent(
            new[] { "ContractVersion", "SceneId", "SourceCharacterId", "SelectedCharacterId" },
            properties.ToArray());

        foreach (var forbidden in new[]
                 {
                     "StrategyContract", "ContextPacketId", "Rule", "Basis", "Reason", "Score",
                     "Weight", "Probability", "Control", "History", "SelectedCharacterIds",
                     "Provider", "Model", "TakeId", "CommitId", "Applied", "Effective"
                 })
        {
            Assert.IsFalse(properties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void LeastInterventionTrace_IsMinimalAndStrategySpecific()
    {
        var traceProperties = PublicPropertyNames(typeof(LeastInterventionDirectorTrace));
        var evaluationProperties = PublicPropertyNames(typeof(LeastInterventionDirectorEvaluation));

        CollectionAssert.AreEquivalent(
            new[]
            {
                "StrategyContract",
                "Input",
                "Rule",
                "NeverOpportunitiedCharacterIds",
                "RecentAttentionPattern"
            },
            traceProperties.ToArray());
        CollectionAssert.AreEquivalent(
            new[] { "Proposal", "Trace" },
            evaluationProperties.ToArray());
        Assert.IsNull(typeof(Ensemble.E0.Core.Director).AssemblyQualifiedName);
    }

    [TestMethod]
    public void Nomination_SelectsNominatedCharacter()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Wren?", nominatedCharacterId: "WREN"),
            History("VOSS"));

        Assert.AreEqual("WREN", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.Nomination, evaluation.Trace.Rule);
        Assert.AreEqual(E0DirectorContracts.OpportunityContractVersion, evaluation.Proposal.ContractVersion);
        Assert.AreEqual(E0DirectorContracts.LeastInterventionStrategyContract, evaluation.Trace.StrategyContract);
    }

    [TestMethod]
    public void Nomination_WinsOverAddressedPool()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Wren?", new[] { "MARLOWE" }, "WREN"),
            History("VOSS"));

        Assert.AreEqual("WREN", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.Nomination, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void SingleDirectAddress_SelectsAddressedCharacter()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Marlowe.", new[] { "MARLOWE" }),
            History("VOSS"));

        Assert.AreEqual("MARLOWE", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.DirectAddress, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void MultipleDirectAddresses_UseLeastRecentWithinAddressedPool()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Both of you.", new[] { "WREN", "MARLOWE" }),
            History("MARLOWE", "WREN", "VOSS"));

        Assert.AreEqual("MARLOWE", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.DirectAddress, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void EmptyControl_UsesCompleteRosterRecencyFallback()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "No."),
            History("VOSS"));

        Assert.AreEqual("MARLOWE", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.RecencyFallback, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void RecencyFallback_NaturallyAvoidsSourceWithoutHardExclusion()
    {
        var packet = ComposeVoss();
        var input = Bind(packet, ParseCandidate(packet, "No."), History("MARLOWE", "WREN", "VOSS"));
        var evaluation = LeastInterventionDirector.Propose(input);

        Assert.AreNotEqual(input.SourceCharacterId, evaluation.Proposal.SelectedCharacterId);
        CollectionAssert.Contains(Values(input.RosterCharacterIds), input.SourceCharacterId.Value);
    }

    [TestMethod]
    public void NeverSeenCharacter_BeatsSeenCharactersInRecencyFallback()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "No."),
            History("MARLOWE", "VOSS"));

        Assert.AreEqual("WREN", evaluation.Proposal.SelectedCharacterId.Value);
    }

    [TestMethod]
    public void NeverSeenTie_UsesOrdinalCharacterId()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(packet, ParseCandidate(packet, "No."), History("VOSS"));

        Assert.AreEqual("MARLOWE", evaluation.Proposal.SelectedCharacterId.Value);
    }

    [TestMethod]
    public void Nomination_MaySelectGloballyMoreRecentCharacter()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Marlowe?", nominatedCharacterId: "MARLOWE"),
            History("WREN", "MARLOWE", "VOSS"));

        Assert.AreEqual("MARLOWE", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.Nomination, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void AddressedPool_MayExcludeGloballyLessRecentUnaddressedCharacter()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Wren?", new[] { "WREN" }),
            History("MARLOWE", "WREN", "VOSS"));

        Assert.AreEqual("WREN", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.DirectAddress, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void VisibleTextChanges_CannotAffectDirectorInputOrSelection()
    {
        var packet = ComposeVoss();
        var firstCandidate = ParseCandidate(packet, "First text.", new[] { "WREN" }, "WREN");
        var secondCandidate = ParseCandidate(packet, "Completely different text.", new[] { "WREN" }, "WREN");
        var history = History("VOSS");

        var first = Propose(packet, firstCandidate, history);
        var second = Propose(packet, secondCandidate, history);

        AssertDirectorSemanticEqual(first, second);
    }

    [TestMethod]
    public void Silence_NaturallyUsesRecencyFallback()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(packet, ParseCandidate(packet, string.Empty), History("VOSS"));

        Assert.AreEqual(LeastInterventionDirectorRule.RecencyFallback, evaluation.Trace.Rule);
        Assert.AreEqual("MARLOWE", evaluation.Proposal.SelectedCharacterId.Value);
    }

    [TestMethod]
    public void NeverOpportunitiedDiagnostic_DoesNotOverrideNomination()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Wren?", nominatedCharacterId: "WREN"),
            History("VOSS"));

        CollectionAssert.AreEqual(
            new[] { "MARLOWE", "WREN" },
            Values(evaluation.Trace.NeverOpportunitiedCharacterIds));
        Assert.AreEqual("WREN", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.Nomination, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void RepeatedSameCharacterEffectiveEvents_AreDiagnosticOnly()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Wren?", nominatedCharacterId: "WREN"),
            History("MARLOWE", "VOSS", "VOSS"));

        Assert.AreEqual(
            DirectorRecentAttentionPattern.RepeatedSameCharacter,
            evaluation.Trace.RecentAttentionPattern);
        Assert.AreEqual("WREN", evaluation.Proposal.SelectedCharacterId.Value);
    }

    [TestMethod]
    public void TwoCharacterAlternation_IsDiagnosticOnly()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Marlowe?", nominatedCharacterId: "MARLOWE"),
            History("MARLOWE", "VOSS", "MARLOWE", "VOSS"));

        Assert.AreEqual(
            DirectorRecentAttentionPattern.TwoCharacterAlternation,
            evaluation.Trace.RecentAttentionPattern);
        Assert.AreEqual("MARLOWE", evaluation.Proposal.SelectedCharacterId.Value);
        Assert.AreEqual(LeastInterventionDirectorRule.Nomination, evaluation.Trace.Rule);
    }

    [TestMethod]
    public void NonAlternatingRecentHistory_HasNoPatternDiagnostic()
    {
        var packet = ComposeVoss();
        var evaluation = Propose(
            packet,
            ParseCandidate(packet, "Wren?", nominatedCharacterId: "WREN"),
            History("MARLOWE", "WREN", "MARLOWE", "VOSS"));

        Assert.AreEqual(
            DirectorRecentAttentionPattern.None,
            evaluation.Trace.RecentAttentionPattern);
    }

    [TestMethod]
    public void DirectorDiagnosticsExposeNoFairnessApplicationSurface()
    {
        var proposalProperties = PublicPropertyNames(typeof(DirectorOpportunityProposal));
        var traceProperties = PublicPropertyNames(typeof(LeastInterventionDirectorTrace));
        var publicMethodNames = typeof(LeastInterventionDirector)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var forbidden in new[]
                 {
                     "Quota", "Fairness", "Score", "Weight", "Probability", "MaximumGap",
                     "Apply", "Promote", "Commit", "Trigger", "FallbackTarget"
                 })
        {
            Assert.IsFalse(proposalProperties.Contains(forbidden), forbidden);
            Assert.IsFalse(traceProperties.Contains(forbidden), forbidden);
            Assert.IsFalse(publicMethodNames.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void BindAndPropose_DoNotMutateContextCandidateHistoryOrInput()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "Wren?", new[] { "WREN" }, "WREN");
        var history = History("MARLOWE", "VOSS");
        var beforePacketId = packet.ContextPacketId;
        var beforeText = candidate.VisibleText;
        var beforeHistory = Values(history);

        var input = DirectorOpportunityInput.Bind(packet, candidate, history);
        var beforeInputHistory = Values(input.OpportunityHistory);
        _ = LeastInterventionDirector.Propose(input);

        Assert.AreEqual(beforePacketId, packet.ContextPacketId);
        Assert.AreEqual(beforeText, candidate.VisibleText);
        CollectionAssert.AreEqual(beforeHistory, Values(history));
        CollectionAssert.AreEqual(beforeInputHistory, Values(input.OpportunityHistory));
    }

    [TestMethod]
    public void DirectorSurface_HasNoApplyTriggerHistoryMutationTakeStateWorldOrProviderAuthority()
    {
        var assemblyTypes = typeof(LeastInterventionDirector).Assembly
            .GetTypes()
            .Where(type => string.Equals(type.Namespace, "Ensemble.E0.Core.Director", StringComparison.Ordinal))
            .ToArray();
        var publicOperations = assemblyTypes
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);
        var propertyNames = assemblyTypes
            .SelectMany(type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var forbiddenOperation in new[]
                 {
                     "Apply", "Promote", "Trigger", "Append", "Commit", "Persist", "Mutate", "ResolveWorld"
                 })
        {
            Assert.IsFalse(publicOperations.Contains(forbiddenOperation), forbiddenOperation);
        }

        foreach (var forbiddenProperty in new[]
                 {
                     "TakeId", "CommitId", "WorldState", "Mutation", "Truth", "Observation",
                     "Provider", "Model", "Effective", "Applied", "CurrentOpportunity"
                 })
        {
            Assert.IsFalse(propertyNames.Contains(forbiddenProperty), forbiddenProperty);
        }
    }

    [TestMethod]
    public void SameContextPacketSemanticReconstruction_CanBindSameCandidate()
    {
        var firstPacket = ComposeVoss();
        var reconstructedPacket = ComposeVoss();
        var candidate = ParseCandidate(firstPacket, "Wren?", nominatedCharacterId: "WREN");

        Assert.AreEqual(firstPacket.ContextPacketId, reconstructedPacket.ContextPacketId);
        var input = DirectorOpportunityInput.Bind(
            reconstructedPacket,
            candidate,
            History("VOSS"));

        Assert.AreEqual(candidate.ContextPacketId, input.SourceContextPacketId);
    }

    [TestMethod]
    public void DirectorContextIdentity_DoesNotClaimRenderedOrProviderDisclosureIdentity()
    {
        var inputProperties = PublicPropertyNames(typeof(DirectorOpportunityInput));
        var proposalProperties = PublicPropertyNames(typeof(DirectorOpportunityProposal));
        var traceProperties = PublicPropertyNames(typeof(LeastInterventionDirectorTrace));

        foreach (var forbidden in new[]
                 {
                     "RenderingContract", "RenderedContextHash", "Request", "Provider", "Model",
                     "AttemptId", "RawOutput"
                 })
        {
            Assert.IsFalse(inputProperties.Contains(forbidden), forbidden);
            Assert.IsFalse(proposalProperties.Contains(forbidden), forbidden);
            Assert.IsFalse(traceProperties.Contains(forbidden), forbidden);
        }
    }

    [TestMethod]
    public void RepeatedBindAndPropose_AreDeterministicallyEquivalent()
    {
        var packet = ComposeVoss();
        var candidate = ParseCandidate(packet, "Both.", new[] { "WREN", "MARLOWE" });
        var history = History("WREN", "MARLOWE", "VOSS");

        var first = Propose(packet, candidate, history);
        var second = Propose(packet, candidate, history);

        AssertDirectorSemanticEqual(first, second);
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

    [TestMethod]
    public void Propose_NullInputFailsInDirectorExceptionDomain()
    {
        Assert.Throws<DirectorOpportunityException>(() =>
            LeastInterventionDirector.Propose(null!));
    }

    private static LeastInterventionDirectorEvaluation Propose(
        ContextPacket packet,
        CandidatePerformance candidate,
        ImmutableArray<CharacterId> history) =>
        LeastInterventionDirector.Propose(Bind(packet, candidate, history));

    private static DirectorOpportunityInput Bind(
        ContextPacket packet,
        CandidatePerformance candidate,
        ImmutableArray<CharacterId> history) =>
        DirectorOpportunityInput.Bind(packet, candidate, history);

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

    private static ImmutableArray<CharacterId> History(params string[] ids) =>
        ids.Select(CharacterId.From).ToImmutableArray();

    private static string[] Values(ImmutableArray<CharacterId> ids) =>
        ids.Select(id => id.Value).ToArray();

    private static HashSet<string> PublicPropertyNames(Type type) =>
        type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

    private static void AssertDirectorSemanticEqual(
        LeastInterventionDirectorEvaluation expected,
        LeastInterventionDirectorEvaluation actual)
    {
        Assert.AreEqual(expected.Proposal.ContractVersion, actual.Proposal.ContractVersion);
        Assert.AreEqual(expected.Proposal.SceneId, actual.Proposal.SceneId);
        Assert.AreEqual(expected.Proposal.SourceCharacterId, actual.Proposal.SourceCharacterId);
        Assert.AreEqual(expected.Proposal.SelectedCharacterId, actual.Proposal.SelectedCharacterId);
        Assert.AreEqual(expected.Trace.StrategyContract, actual.Trace.StrategyContract);
        Assert.AreEqual(expected.Trace.Rule, actual.Trace.Rule);
        Assert.AreEqual(expected.Trace.RecentAttentionPattern, actual.Trace.RecentAttentionPattern);
        CollectionAssert.AreEqual(
            Values(expected.Trace.NeverOpportunitiedCharacterIds),
            Values(actual.Trace.NeverOpportunitiedCharacterIds));
        CollectionAssert.AreEqual(
            Values(expected.Trace.Input.RosterCharacterIds),
            Values(actual.Trace.Input.RosterCharacterIds));
        CollectionAssert.AreEqual(
            Values(expected.Trace.Input.AddressedCharacterIds),
            Values(actual.Trace.Input.AddressedCharacterIds));
        CollectionAssert.AreEqual(
            Values(expected.Trace.Input.OpportunityHistory),
            Values(actual.Trace.Input.OpportunityHistory));
        Assert.AreEqual(
            expected.Trace.Input.NominatedCharacterId?.Value,
            actual.Trace.Input.NominatedCharacterId?.Value);
    }

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
            throw new InvalidOperationException("Director test fixture variant replacement did not apply.");
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
