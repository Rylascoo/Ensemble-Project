using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Gemini;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Host;

internal static class E0AReferenceRunHost
{
    internal static E0APricingAssumptions ConservativePricing => E0AGeminiPricingPolicy.ConservativeShadowPricing;

    internal static async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length != 5 || args.Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException(
                "Usage: Ensemble.E0.Harness e0a-run CREATIVE-NONE <fixture.json> <run-id> <evidence-root> <executable-commit>");
        }

        var variant = args[0];
        var fixturePath = args[1];
        var runId = RunId.From(args[2]);
        var evidenceRoot = args[3];
        var executableCommit = args[4];
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

        // This is a short-lived experimental snapshot guard, not live provider
        // verification or ODR-26 policy law. A material provider change inside the
        // window still requires re-verification before inference.
        E0AGeminiPricingPolicy.RequireNonStaleSnapshot(DateTimeOffset.UtcNow);

        // Credential access is confined to this explicit live-run path. Missing
        // credentials fail before any run evidence directory is created.
        using var http = CreateProviderHttpClient();
        var provider = GeminiGenerateContentPort.FromEnvironment(http);

        var envelope = CreateEnvelope(variant);
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
            $"E0-A {envelope.Variant} run terminal: {result.Status}; acceptedTurns={result.AcceptedTurns}; estimatedShadowSpendUsd={result.EstimatedSpendUsd:0.000000}");
        Console.WriteLine($"Evidence root: {evidence.RootPath}");
        return result.Status == E0ARunTerminalStatus.AcceptedTurnCapReached ? 0 : 3;
    }

    internal static HttpClient CreateProviderHttpClient()
    {
        var http = new HttpClient
        {
            // The run driver owns the frozen 300-second attempt deadline through its
            // linked cancellation token. The transport must not introduce a shorter
            // independent timeout that can preempt token preflight or provider work.
            Timeout = Timeout.InfiniteTimeSpan
        };
        return http;
    }

    internal static E0ARunEnvelope CreateEnvelope(string variant) => variant switch
    {
        "CREATIVE-NONE" => E0ARunEnvelope.GeminiNormativeReference(ConservativePricing),
        "CREATIVE-LOW" or "CREATIVE-MEDIUM" or "CREATIVE-HIGH" =>
            throw new E0AHarnessException("E0-A Gemini LOW/MEDIUM/HIGH characterization is deferred pending a provider-specific resource-normalization amendment."),
        _ => throw new E0AHarnessException("E0-A live-run variant is not approved by the Gemini normative amendment.")
    };

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