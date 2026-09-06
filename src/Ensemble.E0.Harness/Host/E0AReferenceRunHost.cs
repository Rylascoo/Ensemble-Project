using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.OpenAI;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Host;

internal static class E0AReferenceRunHost
{
    // Conservative bound verified against GPT-5.6 Sol pricing on 2026-09-06.
    // Base promotional rates are 4.00 / 0.40 / 20.00 USD per 1M text tokens.
    // Above 272K input, input is 2x and output 1.5x; cache writes are 1.25x
    // uncached input. We therefore use 10.00 / 0.80 / 30.00 for every request.
    internal static E0APricingAssumptions ConservativePricing { get; } = new(10.00m, 0.80m, 30.00m);

    internal static async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length != 4 || args.Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException(
                "Usage: Ensemble.E0.Harness e0a-run <fixture.json> <run-id> <evidence-root> <executable-commit>");
        }

        var fixturePath = args[0];
        var runId = RunId.From(args[1]);
        var evidenceRoot = args[2];
        var executableCommit = args[3];
        E0ADeterministicIds.ValidateRunId(runId);

        // Bind manifest provenance to the exact clean checkout before secret access.
        E0ARepositoryCheckoutGuard.Validate(executableCommit);
        if (Directory.Exists(evidenceRoot) || File.Exists(evidenceRoot))
        {
            throw new E0AHarnessException("E0-A run evidence directory already exists.");
        }

        var bytes = await File.ReadAllBytesAsync(fixturePath, cancellationToken).ConfigureAwait(false);
        var fixture = GenericE0FixtureValidator.Validate(FixtureLoader.Load(bytes));
        MissingRaftContract.Validate(fixture);
        var genesis = ProductionState.Initialize(fixture, ImmutableArray<RecordId>.Empty);

        // Credential access is deliberately confined to this explicit live-run path.
        // Missing credentials fail before any run evidence directory is created.
        using var http = new HttpClient();
        var provider = OpenAIResponsesPort.FromEnvironment(http);

        var envelope = E0ARunEnvelope.CreativeNone(ConservativePricing);
        var evidence = new E0AFileEvidenceStore(
            evidenceRoot,
            runId,
            envelope,
            genesis.OriginFixtureId.Value,
            genesis.OriginFixtureVersion.Value,
            genesis.OriginFixtureHash,
            executableCommit,
            genesis.RosterCharacterIds);
        var driver = new E0AReferenceRunDriver(envelope, provider, provider, evidence);
        var result = await driver.RunAsync(runId, genesis, cancellationToken).ConfigureAwait(false);

        Console.WriteLine(
            $"E0-A run terminal: {result.Status}; acceptedTurns={result.AcceptedTurns}; estimatedSpendUsd={result.EstimatedSpendUsd:0.000000}");
        Console.WriteLine($"Evidence root: {evidence.RootPath}");
        return result.Status == E0ARunTerminalStatus.AcceptedTurnCapReached ? 0 : 3;
    }

    internal static int SealEvaluation(string[] args)
    {
        if (args.Length < 4 || args.Take(4).Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException(
                "Usage: Ensemble.E0.Harness e0a-evaluate <evidence-root> <reviewer-id> <method-id> <pass|fail> [finding ...]");
        }

        var passed = args[3] switch
        {
            "pass" => true,
            "fail" => false,
            _ => throw new E0AHarnessException("E0-A hard-gate result must be 'pass' or 'fail'.")
        };
        var findings = args.Skip(4).ToArray();
        if (findings.Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException("E0-A hard-gate findings must be nonblank.");
        }
        if (passed && findings.Length != 0)
        {
            throw new E0AHarnessException("A passing E0-A hard-gate evaluation cannot contain findings.");
        }
        if (!passed && findings.Length == 0)
        {
            throw new E0AHarnessException("A failing E0-A hard-gate evaluation requires at least one finding.");
        }

        var evaluation = new E0AHardGateEvaluation(
            E0AEvidenceContracts.HardGateChecklistVersion,
            args[1],
            args[2],
            passed,
            findings);
        E0AExistingEvidenceEvaluationSealer.Seal(args[0], evaluation);

        Console.WriteLine($"E0-A hard-gate evaluation sealed: {(passed ? "PASS" : "FAIL")}");
        return passed ? 0 : 4;
    }
}
