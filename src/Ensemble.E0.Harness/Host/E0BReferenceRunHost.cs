using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Gemini;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Host;

internal static class E0BReferenceRunHost
{
    internal static E0ARunEnvelope ReferenceEnvelope() =>
        E0ARunEnvelope.GeminiComparison(E0AGeminiModelCatalog.FlashLite35MinimalId);

    internal static E0BMixedCastConfiguration ApprovedCast() =>
        E0BMixedCastConfiguration.Approved01(ReferenceEnvelope());

    internal static async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length != 4 || args.Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException(
                "Usage: Ensemble.E0.Harness e0b-run <fixture.json> <run-id> <evidence-root> <executable-commit>");
        }

        var fixturePath = args[0];
        var runId = RunId.From(args[1]);
        var evidenceRoot = args[2];
        var executableCommit = args[3];
        E0ADeterministicIds.ValidateRunId(runId);

        // Bind evidence provenance to the exact clean checkout before secret access.
        E0ARepositoryCheckoutGuard.Validate(executableCommit);
        if (Directory.Exists(evidenceRoot) || File.Exists(evidenceRoot))
        {
            throw new E0AHarnessException("E0-B run evidence directory already exists.");
        }

        var bytes = await File.ReadAllBytesAsync(fixturePath, cancellationToken).ConfigureAwait(false);
        var fixture = GenericE0FixtureValidator.Validate(FixtureLoader.Load(bytes));
        MissingRaftContract.Validate(fixture);
        var genesis = ProductionState.Initialize(fixture, ImmutableArray<RecordId>.Empty);

        var envelope = ReferenceEnvelope();
        var mixedCast = E0BMixedCastConfiguration.Approved01(envelope);

        // This short-lived code guard never substitutes for the separately required
        // authenticated pre-run quota/capacity verification.
        E0AGeminiPricingPolicy.RequireNonStaleSnapshot(DateTimeOffset.UtcNow);

        using var http = E0AReferenceRunHost.CreateProviderHttpClient();
        var provider = GeminiGenerateContentPort.FromEnvironment(http);
        using var rateRouter = new E0BMixedGeminiRateRouter(mixedCast);

        var evidence = new E0AFileEvidenceStore(
            evidenceRoot,
            runId,
            envelope,
            genesis.OriginFixtureId.Value,
            genesis.OriginFixtureVersion.Value,
            genesis.OriginFixtureHash,
            executableCommit,
            genesis.RosterCharacterIds,
            mixedCast);
        var driver = new E0AReferenceRunDriver(
            envelope,
            provider,
            provider,
            evidence,
            mixedCast: mixedCast,
            rateRouter: rateRouter);
        var result = await driver.RunAsync(runId, genesis, cancellationToken).ConfigureAwait(false);

        Console.WriteLine(
            $"E0-B {mixedCast.ConditionId} run terminal: {result.Status}; acceptedTurns={result.AcceptedTurns}; estimatedShadowSpendUsd={result.EstimatedSpendUsd:0.000000}");
        Console.WriteLine($"Evidence root: {evidence.RootPath}");
        return result.Status == E0ARunTerminalStatus.AcceptedTurnCapReached ? 0 : 3;
    }
}
