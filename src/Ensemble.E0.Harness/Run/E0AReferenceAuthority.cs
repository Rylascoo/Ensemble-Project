using System.Collections.Immutable;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Turn;

namespace Ensemble.E0.Harness.Run;

internal static class E0AReferenceAuthority
{
    internal static StateAuthorityPolicy Policy(E0ARunEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        return StateAuthorityPolicy.Create(envelope.AutoApproveDomains);
    }

    internal static E0TurnProgress EvaluateAndRejectMandatoryReview(
        E0TurnProgress ready,
        StateInterpretationProposal proposal,
        StateAuthorityPolicy policy)
    {
        var progress = DeterministicE0TurnOrchestrator.EvaluateAuthority(
            ready,
            proposal,
            policy,
            ImmutableArray<StateAuthorityReviewChoice>.Empty);

        if (progress.Disposition != E0TurnProgressDisposition.AuthorityReviewRequired)
        {
            return progress;
        }

        var evaluation = progress.AuthorityEvaluation
            ?? throw new E0AHarnessException("E0-A State Authority review evaluation is missing.");
        var rejects = evaluation.Decisions
            .Where(x => x.Disposition == StateAuthorityDisposition.RequiresReview)
            .Select(x => StateAuthorityReviewChoice.Reject(x.MutationIndex))
            .ToImmutableArray();

        if (rejects.Length == 0)
        {
            throw new E0AHarnessException("E0-A State Authority requested review without reviewable mutations.");
        }

        var resolved = DeterministicE0TurnOrchestrator.ResolveAuthorityReview(progress, rejects);
        if (resolved.Disposition != E0TurnProgressDisposition.TakeBindable ||
            resolved.AuthorityEvaluation?.Status != StateAuthorityEvaluationStatus.Complete)
        {
            throw new E0AHarnessException("E0-A deterministic review rejection did not reach terminal authority.");
        }

        return resolved;
    }

    internal static E0RecordMaterializationSet Materializations(
        RunId runId,
        int turn,
        StateInterpretationProposal proposal,
        StateAuthorityEvaluation evaluation)
    {
        ArgumentNullException.ThrowIfNull(proposal);
        ArgumentNullException.ThrowIfNull(evaluation);
        if (proposal.Mutations.Length != evaluation.Decisions.Length)
        {
            throw new E0AHarnessException("E0-A authority/materialization mutation count is inconsistent.");
        }

        var items = ImmutableArray.CreateBuilder<E0RecordMaterialization>();
        foreach (var decision in evaluation.Decisions)
        {
            if (decision.Disposition != StateAuthorityDisposition.Approved)
            {
                continue;
            }

            if (decision.MutationIndex < 0 || decision.MutationIndex >= proposal.Mutations.Length)
            {
                throw new E0AHarnessException("E0-A authority decision mutation index is invalid.");
            }

            if (CreatesRecord(proposal.Mutations[decision.MutationIndex]))
            {
                items.Add(E0RecordMaterialization.Create(
                    decision.MutationIndex,
                    E0ADeterministicIds.Record(runId, turn, decision.MutationIndex)));
            }
        }

        return E0RecordMaterializationSet.Bind(items.ToImmutable());
    }

    private static bool CreatesRecord(StateMutationCandidate mutation) => mutation switch
    {
        CharacterClaimMutationCandidate => true,
        AppendOnlyCharacterStateMutationCandidate => true,
        GlobalStateMutationCandidate global => global.Change is AddStateMutationChange or SupersedeStateMutationChange,
        MutableCharacterStateMutationCandidate character => character.Change is AddStateMutationChange or SupersedeStateMutationChange,
        RelationshipStateMutationCandidate relationship => relationship.Change is AddStateMutationChange or SupersedeStateMutationChange,
        _ => false
    };
}
