using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Experiments.E0D;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.CausalCommit;

public sealed partial class E0TakeStateBinding
{
    internal static E0TakeStateBinding BindWithAcceptedHistoryE0D(
        ProductionStateCheckpoint sourceCheckpoint,
        ContextPacket sourceContext,
        E0Take take,
        E0AcceptedPerformanceHistory history,
        ContextPacket expectedContext)
    {
        ArgumentNullException.ThrowIfNull(history);
        ArgumentNullException.ThrowIfNull(expectedContext);
        if (!E0DExperimentContracts.IsContextAblationCompositionContract(
                sourceContext.CompositionContract) ||
            !string.Equals(
                expectedContext.CompositionContract,
                sourceContext.CompositionContract,
                StringComparison.Ordinal))
        {
            throw new E0CausalCommitException(
                "E0-D causal binding requires one frozen context-ablation contract.");
        }

        return BindCore(
            sourceCheckpoint,
            sourceContext,
            take,
            history,
            expectedContext);
    }

    private static void ValidateExactExperimentalSourceContextWithAcceptedHistory(
        ProductionStateCheckpoint sourceCheckpoint,
        ContextPacket sourceContext,
        E0AcceptedPerformanceHistory history,
        ContextPacket expectedContext)
    {
        try
        {
            var entries = AcceptedPerformanceHistoryInvariants.ValidateAndProject(
                history,
                sourceCheckpoint.SceneId,
                sourceCheckpoint.StateHash,
                sourceCheckpoint.SourceState.RosterCharacterIds);
            var genesisHash = ProductionStateCanonicalizer.ComputeGenesisHash(
                sourceCheckpoint.SourceState.Projection);
            var isGenesis = genesisHash == sourceCheckpoint.StateHash;

            if (entries.Length == 0)
            {
                if (!isGenesis || !IsE0DProductionBoundV2(sourceContext))
                {
                    throw new E0CausalCommitException(
                        "E0-D history-aware binding permits empty history only for exact genesis Context v2.");
                }
            }
            else if (isGenesis || !IsE0DAcceptedHistoryV3(sourceContext))
            {
                throw new E0CausalCommitException(
                    "E0-D history-aware binding requires Context v3 for synchronized nonempty history.");
            }

            RequireExactContext(expectedContext, sourceContext);
        }
        catch (E0CausalCommitException)
        {
            throw;
        }
        catch (E0AcceptedPerformanceHistoryInvariantException exception)
        {
            throw new E0CausalCommitException(
                "E0-D causal binding accepted Performance history proof failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0CausalCommitException(
                "E0-D causal binding source identity is invalid.",
                exception);
        }
    }

    private static bool IsE0DProductionBoundV2(ContextPacket context) =>
        context.Rendered is not null &&
        string.Equals(context.SchemaVersion, E0ContextContracts.ProductionBoundSchemaVersion, StringComparison.Ordinal) &&
        E0DExperimentContracts.IsContextAblationCompositionContract(context.CompositionContract) &&
        string.Equals(context.Rendered.RenderingContract, E0ContextContracts.RenderingContract, StringComparison.Ordinal);

    private static bool IsE0DAcceptedHistoryV3(ContextPacket context) =>
        context.Rendered is not null &&
        string.Equals(context.SchemaVersion, E0ContextContracts.AcceptedHistorySchemaVersion, StringComparison.Ordinal) &&
        E0DExperimentContracts.IsContextAblationCompositionContract(context.CompositionContract) &&
        string.Equals(context.Rendered.RenderingContract, E0ContextContracts.AcceptedHistoryRenderingContract, StringComparison.Ordinal);
}
