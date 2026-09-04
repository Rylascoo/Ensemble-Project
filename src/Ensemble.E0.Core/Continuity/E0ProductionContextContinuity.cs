using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;

namespace Ensemble.E0.Core.Continuity;

public sealed class E0ProductionContextContinuityResult
{
    internal E0ProductionContextContinuityResult(
        CharacterAccessEvaluation accessEvaluation,
        ContextCompositionEvaluation contextEvaluation)
    {
        AccessEvaluation = accessEvaluation;
        ContextEvaluation = contextEvaluation;
    }

    public CharacterAccessEvaluation AccessEvaluation { get; }
    public ContextCompositionEvaluation ContextEvaluation { get; }
}

public static class E0ProductionContextContinuity
{
    public static E0ProductionContextContinuityResult Compose(
        ProductionStateCheckpoint sourceCheckpoint)
    {
        if (sourceCheckpoint is null)
        {
            throw new E0ContextContinuityException(
                "Production Context continuity source checkpoint is required.");
        }

        try
        {
            _ = sourceCheckpoint.StateHash.Value;
            _ = sourceCheckpoint.SceneId.Value;
            _ = sourceCheckpoint.CurrentOpportunityCharacterId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0ContextContinuityException(
                "Production Context continuity source checkpoint identity is uninitialized.",
                exception);
        }

        var sourceState = sourceCheckpoint.SourceState;
        if (sourceState is null)
        {
            throw new E0ContextContinuityException(
                "Production Context continuity source state is unavailable.");
        }

        try
        {
            if (sourceState.StateHash != sourceCheckpoint.StateHash ||
                sourceState.SceneId != sourceCheckpoint.SceneId ||
                sourceState.CurrentOpportunityCharacterId !=
                    sourceCheckpoint.CurrentOpportunityCharacterId)
            {
                throw new E0ContextContinuityException(
                    "Production Context continuity checkpoint does not match its source state.");
            }

            var access = CharacterBoundedAccessControl.Evaluate(
                sourceState,
                sourceCheckpoint.CurrentOpportunityCharacterId);

            if (!access.Projection.SourceStateHash.HasValue ||
                access.Projection.SourceStateHash.Value != sourceCheckpoint.StateHash)
            {
                throw new E0ContextContinuityException(
                    "Production Context continuity Access source identity is inconsistent.");
            }

            var context = DeterministicContextComposer.ComposeProductionBound(
                access.Projection,
                sourceCheckpoint.CurrentOpportunityCharacterId);

            if (!context.Packet.SourceStateHash.HasValue ||
                context.Packet.SourceStateHash.Value != sourceCheckpoint.StateHash ||
                !context.Trace.SourceStateHash.HasValue ||
                context.Trace.SourceStateHash.Value != sourceCheckpoint.StateHash)
            {
                throw new E0ContextContinuityException(
                    "Production Context continuity Context source identity is inconsistent.");
            }

            return new E0ProductionContextContinuityResult(access, context);
        }
        catch (E0ContextContinuityException)
        {
            throw;
        }
        catch (CharacterAccessException exception)
        {
            throw new E0ContextContinuityException(
                "Production Context continuity Access evaluation failed.",
                exception);
        }
        catch (ContextCompositionException exception)
        {
            throw new E0ContextContinuityException(
                "Production Context continuity Context composition failed.",
                exception);
        }
        catch (InvalidOperationException exception)
        {
            throw new E0ContextContinuityException(
                "Production Context continuity source state is invalid.",
                exception);
        }
    }
}

public sealed class E0ContextContinuityException : Exception
{
    internal E0ContextContinuityException(string message)
        : base(message)
    {
    }

    internal E0ContextContinuityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
