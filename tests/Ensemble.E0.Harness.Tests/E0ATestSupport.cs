using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Turn;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Tests;

internal static class E0ATestSupport
{
    internal static E0APricingAssumptions Pricing() => new(1m, 0.25m, 2m);

    internal static ProductionState Genesis()
    {
        var bytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Fixtures", "missing-raft-0.1.0.json"));
        var fixture = GenericE0FixtureValidator.Validate(FixtureLoader.Load(bytes));
        MissingRaftContract.Validate(fixture);
        return ProductionState.Initialize(fixture, ImmutableArray<RecordId>.Empty);
    }

    internal static E0OpportunityBearingCycleState Cycle() =>
        DeterministicE0CausalCycle.Initialize(Genesis());

    internal static E0TurnProgress CandidateReady(E0OpportunityBearingCycleState cycle, string text = "No.")
    {
        var context = DeterministicE0CausalCycle.ComposeContext(cycle).ContextEvaluation.Packet;
        var candidate = Candidate(context, text);
        var attempt = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);
        return DeterministicE0TurnOrchestrator.GateAttempt(cycle, attempt);
    }

    internal static E0TurnProgress Ready(E0OpportunityBearingCycleState cycle, string text = "No.") =>
        DeterministicE0TurnOrchestrator.EvaluateIntegrity(
            CandidateReady(cycle, text),
            ImmutableArray<IntegrityConcernKind>.Empty);

    internal static CandidatePerformance Candidate(ContextPacket context, string text) =>
        PerformerCandidateContract.ParseJson(
            context,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
                performance = new { text },
                control = new { addressedCharacterIds = Array.Empty<string>(), nominatedCharacterId = (string?)null }
            })));

    internal static byte[] PerformerOutput(string text = "No.") =>
        JsonSerializer.SerializeToUtf8Bytes(new
        {
            schemaVersion = PerformerCandidateContract.CandidateJsonSchemaVersion,
            performance = new { text },
            control = new { addressedCharacterIds = Array.Empty<string>(), nominatedCharacterId = (string?)null }
        });

    internal static byte[] IntegrityOutput(params string[] concerns) =>
        JsonSerializer.SerializeToUtf8Bytes(new { concerns });

    internal static byte[] EmptyInterpreterOutput() =>
        ProposalOutput();

    internal static byte[] ProposalOutput(params Dictionary<string, object?>[] mutations) =>
        JsonSerializer.SerializeToUtf8Bytes(new
        {
            schemaVersion = StateInterpretationContract.JsonSchemaVersion,
            mutations
        });

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

    internal static Dictionary<string, object?> PressureAdd(string text = "Pressure increases.") =>
        Mutation("pressure", "add", text: text);

    internal static Dictionary<string, object?> BeliefSupersede(string text = "Voss revises the working theory.") =>
        Mutation(
            "characterBelief",
            "supersede",
            subjectCharacterId: MissingRaftContract.VossCharacterId,
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: text);

    internal static Dictionary<string, object?> BeliefDeactivate() =>
        Mutation(
            "characterBelief",
            "deactivate",
            subjectCharacterId: MissingRaftContract.VossCharacterId,
            existingRecordId: MissingRaftContract.BelVossAccidentalLossPlausibleId,
            text: null);

    internal static string TempRunRoot() => Path.Combine(Path.GetTempPath(), "ensemble-e0a-tests", Guid.NewGuid().ToString("N"));

    internal static E0AFileEvidenceStore Evidence(
        string root,
        RunId runId,
        E0ARunEnvelope envelope,
        ProductionState state) =>
        new(
            root,
            runId,
            envelope,
            state.OriginFixtureId.Value,
            state.OriginFixtureHash,
            "TEST-EXECUTABLE-COMMIT",
            state.RosterCharacterIds);

    internal static RoleAttemptReceipt Success(
        PreparedRoleAttempt attempt,
        byte[] output,
        string model = "gpt-5.6-sol") =>
        RoleAttemptReceipt.Success(
            attempt,
            $"resp-{attempt.AttemptId}",
            model,
            new E0AUsage(10, 10, 0, 0),
            output);
}

internal sealed class FixedTokenCounter : IE0AInputTokenCounter
{
    private readonly long _tokens;
    internal FixedTokenCounter(long tokens = 100) => _tokens = tokens;
    public int Calls { get; private set; }
    public Task<long> CountInputTokensAsync(PreparedRoleAttempt attempt, CancellationToken cancellationToken)
    {
        Calls++;
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_tokens);
    }
}

internal sealed class ScriptedProvider : IE0AProviderRolePort
{
    private readonly Func<PreparedRoleAttempt, int, RoleAttemptReceipt> _script;
    internal ScriptedProvider(Func<PreparedRoleAttempt, int, RoleAttemptReceipt>? script = null) =>
        _script = script ?? Default;

    internal int Calls { get; private set; }
    internal List<E0ARole> Roles { get; } = new();

    public Task<RoleAttemptReceipt> ExecuteAsync(PreparedRoleAttempt attempt, IE0AProviderDiagnosticSink diagnostics, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Calls++;
        Roles.Add(attempt.Profile.Role);
        return Task.FromResult(_script(attempt, Calls));
    }

    private static RoleAttemptReceipt Default(PreparedRoleAttempt attempt, int _) =>
        E0ATestSupport.Success(
            attempt,
            attempt.Profile.Role switch
            {
                E0ARole.Performer => E0ATestSupport.PerformerOutput(),
                E0ARole.Integrity => E0ATestSupport.IntegrityOutput(),
                E0ARole.Interpreter => E0ATestSupport.EmptyInterpreterOutput(),
                _ => throw new InvalidOperationException()
            });
}

internal sealed class FailingTokenCounter : IE0AInputTokenCounter
{
    public int Calls { get; private set; }
    public Task<long> CountInputTokensAsync(PreparedRoleAttempt attempt, CancellationToken cancellationToken)
    {
        Calls++;
        cancellationToken.ThrowIfCancellationRequested();
        throw new E0AHarnessException("synthetic preflight failure");
    }
}

internal sealed class CollectingDiagnosticSink : IE0AProviderDiagnosticSink
{
    internal List<string> Events { get; } = new();
    public void RecordStreamEvent(PreparedRoleAttempt attempt, ReadOnlyMemory<byte> utf8Event) =>
        Events.Add(Encoding.UTF8.GetString(utf8Event.Span));
}
