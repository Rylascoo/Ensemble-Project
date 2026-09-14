using System.Collections.Immutable;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Experiments.E0D;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Harness.Run;

internal static class E0DIntegrityAssessmentPacketBuilder
{
    internal static E0AIntegrityAssessmentPacket Build(
        ProductionState state,
        CharacterAccessEvaluation access,
        ContextPacket context,
        CandidatePerformance candidate,
        E0DExperimentVariant variant)
    {
        if (variant != E0DExperimentVariant.OmniscientContext)
        {
            return E0AIntegrityAssessmentPacketBuilder.Build(state, access, context, candidate);
        }

        // Reuse the reference builder as a fail-closed association/decision validator,
        // then replace only constraints made inapplicable by the omniscient ablation.
        _ = E0AIntegrityAssessmentPacketBuilder.Build(state, access, context, candidate);
        var decisions = access.Decisions.ToDictionary(x => x.RecordId.Value, StringComparer.Ordinal);
        var visible = VisibleRecordIds(context);
        var constraints = new Dictionary<string, E0AIntegrityConstraint>(StringComparer.Ordinal);

        foreach (var record in state.Records)
        {
            if (record.Lifecycle != ProductionRecordLifecycle.Active)
            {
                continue;
            }
            var decision = decisions[record.RecordId.Value];
            var deniedAndStillHidden =
                decision.Disposition == AccessDisposition.Deny && !visible.Contains(record.RecordId.Value);
            var protectedRecord = record.Protection is
                ProductionRecordProtection.SystemImmutable or
                ProductionRecordProtection.CreatorLocked;
            if (!deniedAndStillHidden && !protectedRecord)
            {
                continue;
            }

            constraints[record.RecordId.Value] = new E0AIntegrityConstraint(
                record.RecordId,
                record.Text,
                deniedAndStillHidden ? decision.Reason : null,
                record.Protection);
        }

        return new E0AIntegrityAssessmentPacket(
            context,
            candidate,
            constraints.Values
                .OrderBy(x => x.RecordId.Value, StringComparer.Ordinal)
                .ToImmutableArray());
    }

    private static HashSet<string> VisibleRecordIds(ContextPacket context) =>
        context.SceneState.Select(x => x.RecordId.Value)
            .Concat(context.Pressures.Select(x => x.RecordId.Value))
            .Concat(context.Constitution.Select(x => x.RecordId.Value))
            .Concat(context.Disposition.Select(x => x.RecordId.Value))
            .Concat(context.Circumstance.Select(x => x.RecordId.Value))
            .Concat(context.Observations.Select(x => x.RecordId.Value))
            .Concat(context.Knowledge.Select(x => x.RecordId.Value))
            .Concat(context.Beliefs.Select(x => x.RecordId.Value))
            .Concat(context.Suspicions.Select(x => x.RecordId.Value))
            .Concat(context.Memories.Select(x => x.RecordId.Value))
            .Concat(context.Goals.Select(x => x.RecordId.Value))
            .Concat(context.Relationships.Select(x => x.RecordId.Value))
            .ToHashSet(StringComparer.Ordinal);
}