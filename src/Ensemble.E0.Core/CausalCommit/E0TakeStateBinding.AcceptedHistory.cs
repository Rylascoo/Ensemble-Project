using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Core.CausalCommit;

public sealed partial class E0TakeStateBinding
{
    public static E0TakeStateBinding BindWithAcceptedHistory(
        ProductionStateCheckpoint sourceCheckpoint,
        ContextPacket sourceContext,
        Ensemble.E0.Core.Take.E0Take take,
        E0AcceptedPerformanceHistory history)
    {
        if (history is null)
        {
            throw new E0CausalCommitException(
                "Causal commit accepted Performance history is required.");
        }

        return BindCore(sourceCheckpoint, sourceContext, take, history);
    }

    private static void ValidateExactSourceContextWithAcceptedHistory(
        ProductionStateCheckpoint sourceCheckpoint,
        ContextPacket sourceContext,
        E0AcceptedPerformanceHistory history)
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

            var access = CharacterBoundedAccessControl.Evaluate(
                sourceCheckpoint.SourceState,
                sourceCheckpoint.CurrentOpportunityCharacterId);

            ContextPacket expected;
            if (entries.Length == 0)
            {
                if (!isGenesis || !IsProductionBoundV2(sourceContext))
                {
                    throw new E0CausalCommitException(
                        "History-aware causal binding permits empty history only for exact genesis Context v2.");
                }

                expected = DeterministicContextComposer.ComposeProductionBound(
                    access.Projection,
                    sourceCheckpoint.CurrentOpportunityCharacterId).Packet;
            }
            else
            {
                if (isGenesis || !IsAcceptedHistoryV3(sourceContext))
                {
                    throw new E0CausalCommitException(
                        "History-aware causal binding requires Context v3 for synchronized nonempty history.");
                }

                expected = DeterministicContextComposer.ComposeProductionBoundWithAcceptedHistory(
                    access.Projection,
                    sourceCheckpoint.CurrentOpportunityCharacterId,
                    entries).Packet;
            }

            RequireExactContext(expected, sourceContext);
        }
        catch (E0CausalCommitException)
        {
            throw;
        }
        catch (E0AcceptedPerformanceHistoryInvariantException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit accepted Performance history proof failed.",
                exception);
        }
        catch (CharacterAccessException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit source Production Access proof failed.",
                exception);
        }
        catch (ContextCompositionException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit source accepted-history Context recomposition proof failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0CausalCommitException(
                "Causal commit accepted-history source identity is invalid.",
                exception);
        }
    }

    private static bool IsAcceptedHistoryV3(ContextPacket context) =>
        string.Equals(
            context.SchemaVersion,
            E0ContextContracts.AcceptedHistorySchemaVersion,
            StringComparison.Ordinal) &&
        string.Equals(
            context.CompositionContract,
            E0ContextContracts.AcceptedHistoryCompositionContract,
            StringComparison.Ordinal) &&
        string.Equals(
            context.Rendered.RenderingContract,
            E0ContextContracts.AcceptedHistoryRenderingContract,
            StringComparison.Ordinal);
}
