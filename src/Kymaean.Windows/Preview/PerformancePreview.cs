#if KYMAEAN_DIRECTOR_PREVIEW
using System.Text.Json;
using Kymaean.Application;
using Kymaean.Infrastructure.Execution;
using Kymaean.Infrastructure.Persistence;
using Kymaean.Windows.Presentation;

namespace Kymaean.Windows.Preview;

// All code in this file is compiled out of shipping Windows. Profile metadata is
// development apparatus; no provider/model/casting configuration enters Product.
internal static class PerformancePreview
{
    internal static string? Scenario { get; private set; }
    internal static string? SelectedScenario(string dataRoot)
    {
        var profile = Path.GetDirectoryName(dataRoot)!;
        using var metadata = JsonDocument.Parse(File.ReadAllText(PreviewEnvironment.SafePath(profile, "profile.json")));
        var kind = metadata.RootElement.GetProperty("kind").GetString();
        if (kind?.StartsWith("performance-", StringComparison.Ordinal) != true) return null;
        return kind is "performance-success" or "performance-empty" or "performance-incompatible" or
            "performance-invalid" or "performance-io" or "performance-access" or "performance-render"
            ? kind : throw new IOException("Unknown deterministic Preview scenario.");
    }

    internal static ProductAccessResult<ProductApplication> Start(FileProductionCatalog catalog, string scenario)
    {
        Scenario = scenario;
        var startup = ProductApplication.Start(new ScenarioCatalog(catalog, scenario));
        if (startup.IsSuccess)
        {
            var runtime = new DeterministicPreviewRuntime(scenario);
            ProductOperationCoordinator.OwnDirectorPreview(startup.Value,
                new ProductPerformer(runtime), new ProductConsequenceInterpreter(runtime));
        }
        return startup;
    }

    private sealed class DeterministicPreviewRuntime(string scenario) : IProductExecutionRuntime
    {
        public PerformerResponse Perform(PerformerRequest request)
        {
            // Deliberate offline delay makes responsive Pending/duplicate guards observable.
            Thread.Sleep(5000);
            var text = scenario == "performance-empty" ? "" : string.Join("\n\n", Enumerable.Repeat(
                "I will wait beside the harbor steps. The last ferry has gone, but the lantern is still burning. " +
                "When you return, I will tell you what I saw beneath the old stair.", 8));
            return new(request.InvocationId, RuntimeCompletion.Completed, text);
        }
        public ConsequenceResponse Interpret(ConsequenceRequest request) => new(request.InvocationId,
            RuntimeCompletion.Completed, [new("CharacterCircumstance", string.Join(" ", Enumerable.Repeat(
                "The Character has committed to waiting beside the harbor steps until the other person returns.", 8)))]);
    }

    private sealed class ScenarioCatalog(FileProductionCatalog inner, string scenario) : IProductionCatalog,
        IProductionCreator, IProductionCharacterCreator, IProductionSceneCreator, IProductionWorldStateWriter,
        IProductionPerformanceCommitter
    {
        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() => inner.ListProductions();
        public ProductAccessResult<ProductionReplayProjection> OpenProduction(ProductionId id) => inner.OpenProduction(id);
        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(ProductionId id) => inner.RecoverProduction(id);
        public ProductAccessResult<ProductionCreation> CreateProduction(string name) => inner.CreateProduction(name);
        public ProductAccessResult<CharacterCreation> CreateCharacter(ProductionId id, string name) => inner.CreateCharacter(id, name);
        public ProductAccessResult<SceneCreation> EstablishScene(ProductionId id, SceneRoster roster) => inner.EstablishScene(id, roster);
        public ProductAccessResult<ProductionReplayProjection> ReplaceWorldCurrentState(ProductionId id, WorldCurrentState state) => inner.ReplaceWorldCurrentState(id, state);
        public ProductAccessResult<ProductionReplayProjection> CommitAcceptedPerformance(ProductionId id,
            ProductionReplayProjection expected, AcceptedPerformance accepted)
        {
            if (scenario == "performance-incompatible")
                return ProductAccessResult<ProductionReplayProjection>.Failure(ProductAccessFailureKind.Incompatible);
            var result = inner.CommitAcceptedPerformance(id, expected, accepted);
            if (!result.IsSuccess) return result;
            // Exact post-append faults exercise conservative unknown-outcome presentation.
            if (scenario == "performance-io") throw new IOException("Deterministic Preview post-commit fault.");
            if (scenario == "performance-access") throw new UnauthorizedAccessException("Deterministic Preview post-commit fault.");
            return scenario == "performance-invalid"
                ? ProductAccessResult<ProductionReplayProjection>.Failure(ProductAccessFailureKind.Invalid) : result;
        }
    }
}
#endif
