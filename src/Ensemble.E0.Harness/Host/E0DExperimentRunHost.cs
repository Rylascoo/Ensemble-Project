using System.Text.Json;
using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Experiments.E0D;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Gemini;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Host;

internal static class E0DExperimentRunHost
{
    internal static async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length != 5 || args.Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException(
                "Usage: Ensemble.E0.Harness e0d-run <experiment-variant-id> <fixture.json> <run-id> <evidence-root> <executable-commit>");
        }

        E0DExperimentVariant variant;
        try
        {
            variant = E0DExperimentContracts.ParseVariantId(args[0]);
        }
        catch (E0DExperimentException exception)
        {
            throw new E0AHarnessException(exception.Message);
        }

        var fixturePath = args[1];
        var runId = RunId.From(args[2]);
        var evidenceRoot = args[3];
        var executableCommit = args[4];
        E0ADeterministicIds.ValidateRunId(runId);
        E0ARepositoryCheckoutGuard.Validate(executableCommit);
        if (Directory.Exists(evidenceRoot) || File.Exists(evidenceRoot))
        {
            throw new E0AHarnessException("E0-D run evidence directory already exists.");
        }

        var bytes = await File.ReadAllBytesAsync(fixturePath, cancellationToken).ConfigureAwait(false);
        var fixture = GenericE0FixtureValidator.Validate(FixtureLoader.Load(bytes));
        MissingRaftContract.Validate(fixture);
        var genesis = ProductionState.Initialize(fixture, ImmutableArray<RecordId>.Empty);
        var envelope = E0AReferenceRunHost.CreateEnvelope(
            "CREATIVE-MINIMAL",
            E0AGeminiModelCatalog.FlashLite35MinimalId);

        E0AGeminiPricingPolicy.RequireNonStaleSnapshot(DateTimeOffset.UtcNow);
        using var http = E0AReferenceRunHost.CreateProviderHttpClient();
        var provider = GeminiGenerateContentPort.FromEnvironment(http);
        using var rateDiscipline = new E0ASmoothGeminiRateDiscipline(envelope.ModelProfile);
        var evidence = new E0AFileEvidenceStore(
            evidenceRoot,
            runId,
            envelope,
            genesis.OriginFixtureId.Value,
            genesis.OriginFixtureVersion.Value,
            genesis.OriginFixtureHash,
            executableCommit,
            genesis.RosterCharacterIds,
            e0dVariant: variant);
        var driver = new E0AReferenceRunDriver(
            envelope,
            provider,
            provider,
            evidence,
            rateDiscipline,
            e0dVariant: variant);
        var result = await driver.RunAsync(runId, genesis, cancellationToken).ConfigureAwait(false);

        Console.WriteLine(
            $"E0-D {E0DExperimentContracts.VariantId(variant)} run terminal: {result.Status}; acceptedTurns={result.AcceptedTurns}; estimatedShadowSpendUsd={result.EstimatedSpendUsd:0.000000}");
        Console.WriteLine($"Evidence root: {evidence.RootPath}");
        return result.Status == E0ARunTerminalStatus.AcceptedTurnCapReached ? 0 : 3;
    }

    private static void VerifyManifestVariant(string evidenceRoot, E0DExperimentVariant variant)
    {
        var manifestPath = Path.Combine(evidenceRoot, "manifest.json");
        if (!File.Exists(manifestPath))
        {
            throw new E0AHarnessException("E0-D evidence manifest is missing.");
        }
        try
        {
            using var document = JsonDocument.Parse(File.ReadAllBytes(manifestPath));
            var root = document.RootElement;
            if (!root.TryGetProperty("contract", out var contract) ||
                !string.Equals(contract.GetString(), "ensemble.e0d.run-manifest.v1", StringComparison.Ordinal) ||
                !root.TryGetProperty("experimentVariantId", out var variantId) ||
                !string.Equals(variantId.GetString(), E0DExperimentContracts.VariantId(variant), StringComparison.Ordinal))
            {
                throw new E0AHarnessException("E0-D evaluation variant does not match the sealed run manifest.");
            }
        }
        catch (JsonException exception)
        {
            throw new E0AHarnessException($"E0-D evidence manifest is invalid: {exception.Message}");
        }
    }
    internal static int SealEvaluation(string[] args)
    {
        if (args.Length < 5 || args.Take(5).Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException(
                "Usage: Ensemble.E0.Harness e0d-evaluate <experiment-variant-id> <evidence-root> <reviewer-id> <method-id> <pass|fail> [finding ...]");
        }

        E0DExperimentVariant variant;
        try
        {
            variant = E0DExperimentContracts.ParseVariantId(args[0]);
        }
        catch (E0DExperimentException exception)
        {
            throw new E0AHarnessException(exception.Message);
        }

        var passed = args[4] switch
        {
            "pass" => true,
            "fail" => false,
            _ => throw new E0AHarnessException("E0-D hard-gate result must be 'pass' or 'fail'.")
        };
        var findings = args.Skip(5).ToArray();
        if (findings.Any(string.IsNullOrWhiteSpace))
        {
            throw new E0AHarnessException("E0-D hard-gate findings must be nonblank.");
        }
        if (passed && findings.Length != 0)
        {
            throw new E0AHarnessException("A passing E0-D hard-gate evaluation cannot contain findings.");
        }
        if (!passed && findings.Length == 0)
        {
            throw new E0AHarnessException("A failing E0-D hard-gate evaluation requires at least one finding.");
        }

        VerifyManifestVariant(args[1], variant);
        var evaluation = new E0AHardGateEvaluation(
            E0DHardGatePolicy.ChecklistVersion(variant),
            args[2],
            args[3],
            passed,
            findings);
        E0AExistingEvidenceEvaluationSealer.Seal(
            args[1],
            evaluation,
            E0DHardGatePolicy.ChecklistVersion(variant));
        Console.WriteLine($"E0-D hard-gate evaluation sealed: {(passed ? "PASS" : "FAIL")}");
        return passed ? 0 : 4;
    }
}