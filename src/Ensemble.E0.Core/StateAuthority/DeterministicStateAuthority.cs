using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.StateInterpreter;

namespace Ensemble.E0.Core.StateAuthority;

public static class DeterministicStateAuthority
{
    public static StateAuthorityEvaluation Evaluate(
        StateAuthorityInput input,
        StateAuthorityPolicy policy,
        StateAuthorityReviewSet reviewSet)
    {
        ValidateEvaluationInputs(input, policy, reviewSet);

        var recordsById = input.Snapshot.Records.ToDictionary(
            descriptor => descriptor.RecordId.Value,
            StringComparer.Ordinal);
        var reviewByIndex = reviewSet.Choices.ToDictionary(choice => choice.MutationIndex);
        var conflictTargets = BuildConflictTargets(input, recordsById);
        var decisions = ImmutableArray.CreateBuilder<StateAuthorityDecision>(input.Mutations.Length);

        foreach (var mutation in input.Mutations)
        {
            var hardReasons = EvaluateHardRules(mutation, recordsById, conflictTargets);
            var hasReviewChoice = reviewByIndex.TryGetValue(mutation.MutationIndex, out var reviewChoice);

            if (hardReasons.Length != 0)
            {
                if (hasReviewChoice)
                {
                    throw new StateAuthorityException(
                        "State Authority review input attempts to override a hard-rejected mutation.");
                }

                decisions.Add(new StateAuthorityDecision(
                    mutation.MutationIndex,
                    StateAuthorityDisposition.Rejected,
                    hardReasons));
                continue;
            }

            decisions.Add(EvaluateReviewAndPolicy(
                mutation,
                policy,
                hasReviewChoice ? reviewChoice : null));
        }

        var immutableDecisions = decisions.ToImmutable();
        var status = immutableDecisions.Any(
            decision => decision.Disposition == StateAuthorityDisposition.RequiresReview)
            ? StateAuthorityEvaluationStatus.ReviewRequired
            : StateAuthorityEvaluationStatus.Complete;

        return new StateAuthorityEvaluation(
            status,
            immutableDecisions,
            new StateAuthorityTrace(input, policy, reviewSet));
    }

    private static void ValidateEvaluationInputs(
        StateAuthorityInput input,
        StateAuthorityPolicy policy,
        StateAuthorityReviewSet reviewSet)
    {
        if (input is null)
        {
            throw new StateAuthorityException("State Authority input is required.");
        }

        if (policy is null)
        {
            throw new StateAuthorityException("State Authority policy is required.");
        }

        if (reviewSet is null)
        {
            throw new StateAuthorityException("State Authority review set is required.");
        }

        if (!string.Equals(
                input.ProposalContentIdentityContract,
                StateAuthorityContracts.ProposalContentIdentityContract,
                StringComparison.Ordinal) ||
            !IsLowerHexHash(input.ProposalContentHash))
        {
            throw new StateAuthorityException(
                "State Authority input proposal identity is invalid.");
        }

        if (!string.Equals(
                policy.ContractVersion,
                StateAuthorityContracts.PolicyContractVersion,
                StringComparison.Ordinal))
        {
            throw new StateAuthorityException(
                "State Authority policy contract is unsupported.");
        }

        if (!string.Equals(
                reviewSet.ContractVersion,
                StateAuthorityContracts.ReviewSetContractVersion,
                StringComparison.Ordinal) ||
            !string.Equals(
                reviewSet.ProposalContentIdentityContract,
                input.ProposalContentIdentityContract,
                StringComparison.Ordinal) ||
            !string.Equals(
                reviewSet.ProposalContentHash,
                input.ProposalContentHash,
                StringComparison.Ordinal))
        {
            throw new StateAuthorityException(
                "State Authority review set does not match the evaluated proposal identity.");
        }

        if (input.Mutations.IsDefault ||
            input.Snapshot is null ||
            input.Snapshot.Records.IsDefault ||
            policy.AutoApproveDomains.IsDefault ||
            reviewSet.Choices.IsDefault)
        {
            throw new StateAuthorityException(
                "State Authority evaluation input contains an invalid collection state.");
        }

        for (var index = 0; index < input.Mutations.Length; index++)
        {
            var mutation = input.Mutations[index]
                ?? throw new StateAuthorityException(
                    "State Authority evaluation input contains an invalid mutation.");
            if (mutation.MutationIndex != index)
            {
                throw new StateAuthorityException(
                    "State Authority mutation indices must preserve proposal order.");
            }
        }

        var previousPolicyDomain = -1;
        foreach (var domain in policy.AutoApproveDomains)
        {
            if (!Enum.IsDefined(domain) ||
                StateAuthorityPolicy.IsAlwaysMandatoryReviewDomain(domain) ||
                !StateAuthorityPolicy.IsPolicyEligibleDomain(domain) ||
                (int)domain <= previousPolicyDomain)
            {
                throw new StateAuthorityException(
                    "State Authority policy is not canonical or contains an invalid domain.");
            }

            previousPolicyDomain = (int)domain;
        }

        var previousChoiceIndex = -1;
        foreach (var choice in reviewSet.Choices)
        {
            if (choice is null ||
                !Enum.IsDefined(choice.Choice) ||
                choice.MutationIndex < 0 ||
                choice.MutationIndex >= input.Mutations.Length ||
                choice.MutationIndex <= previousChoiceIndex)
            {
                throw new StateAuthorityException(
                    "State Authority review set is not canonical or contains an invalid choice.");
            }

            previousChoiceIndex = choice.MutationIndex;
        }
    }

    private static HashSet<string> BuildConflictTargets(
        StateAuthorityInput input,
        IReadOnlyDictionary<string, StateAuthorityRecordDescriptor> recordsById)
    {
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var mutation in input.Mutations)
        {
            var existingRecordId = ExistingRecordId(mutation.Transition);
            if (existingRecordId is null || !recordsById.ContainsKey(existingRecordId))
            {
                continue;
            }

            counts.TryGetValue(existingRecordId, out var count);
            counts[existingRecordId] = count + 1;
        }

        return counts
            .Where(pair => pair.Value > 1)
            .Select(pair => pair.Key)
            .ToHashSet(StringComparer.Ordinal);
    }

    private static ImmutableArray<StateAuthorityReasonCode> EvaluateHardRules(
        StateAuthorityMutationInput mutation,
        IReadOnlyDictionary<string, StateAuthorityRecordDescriptor> recordsById,
        HashSet<string> conflictTargets)
    {
        var reasons = new HashSet<StateAuthorityReasonCode>();

        foreach (var supportingRecordId in mutation.SupportingRecordIds)
        {
            if (!recordsById.ContainsKey(supportingRecordId.Value))
            {
                reasons.Add(StateAuthorityReasonCode.SupportingRecordMissing);
                break;
            }
        }

        var existingRecordId = ExistingRecordId(mutation.Transition);
        if (existingRecordId is not null)
        {
            if (!recordsById.TryGetValue(existingRecordId, out var existing))
            {
                reasons.Add(StateAuthorityReasonCode.ExistingRecordMissing);
            }
            else
            {
                if (existing.Lifecycle != StateAuthorityRecordLifecycle.Active)
                {
                    reasons.Add(StateAuthorityReasonCode.ExistingRecordInactive);
                }

                if (existing.RecordDomain != RecordDomainForMutation(mutation.Domain))
                {
                    reasons.Add(StateAuthorityReasonCode.ExistingRecordDomainMismatch);
                }

                EvaluateOwnershipRules(mutation, existing, reasons);

                if (existing.Protection != StateAuthorityRecordProtection.None)
                {
                    reasons.Add(StateAuthorityReasonCode.ExistingRecordProtected);
                }

                if (conflictTargets.Contains(existingRecordId))
                {
                    reasons.Add(StateAuthorityReasonCode.ConflictingExistingRecordTarget);
                }
            }
        }

        return reasons
            .OrderBy(reason => (int)reason)
            .ToImmutableArray();
    }

    private static void EvaluateOwnershipRules(
        StateAuthorityMutationInput mutation,
        StateAuthorityRecordDescriptor existing,
        ISet<StateAuthorityReasonCode> reasons)
    {
        switch (mutation)
        {
            case CharacterStateAuthorityMutationInput characterMutation:
                if (existing is not CharacterStateAuthorityRecordDescriptor characterRecord ||
                    characterRecord.SubjectCharacterId != characterMutation.SubjectCharacterId)
                {
                    reasons.Add(StateAuthorityReasonCode.ExistingRecordSubjectMismatch);
                }

                break;

            case RelationshipStateAuthorityMutationInput relationshipMutation:
                if (existing is not RelationshipStateAuthorityRecordDescriptor relationshipRecord ||
                    relationshipRecord.SubjectCharacterId != relationshipMutation.SubjectCharacterId)
                {
                    reasons.Add(StateAuthorityReasonCode.ExistingRecordSubjectMismatch);
                }

                if (existing is not RelationshipStateAuthorityRecordDescriptor targetRecord ||
                    targetRecord.TargetCharacterId != relationshipMutation.TargetCharacterId)
                {
                    reasons.Add(StateAuthorityReasonCode.ExistingRecordTargetMismatch);
                }

                break;
        }
    }

    private static StateAuthorityDecision EvaluateReviewAndPolicy(
        StateAuthorityMutationInput mutation,
        StateAuthorityPolicy policy,
        StateAuthorityReviewChoice? reviewChoice)
    {
        if (IsMandatoryReview(mutation))
        {
            return reviewChoice?.Choice switch
            {
                StateAuthorityReviewChoiceKind.Approve => new StateAuthorityDecision(
                    mutation.MutationIndex,
                    StateAuthorityDisposition.Approved,
                    ImmutableArray.Create(
                        StateAuthorityReasonCode.MandatoryReview,
                        StateAuthorityReasonCode.ExplicitReviewApproved)),
                StateAuthorityReviewChoiceKind.Reject => new StateAuthorityDecision(
                    mutation.MutationIndex,
                    StateAuthorityDisposition.Rejected,
                    ImmutableArray.Create(
                        StateAuthorityReasonCode.MandatoryReview,
                        StateAuthorityReasonCode.ExplicitReviewRejected)),
                null => new StateAuthorityDecision(
                    mutation.MutationIndex,
                    StateAuthorityDisposition.RequiresReview,
                    ImmutableArray.Create(StateAuthorityReasonCode.MandatoryReview)),
                _ => throw new StateAuthorityException(
                    "State Authority review choice is undefined.")
            };
        }

        if (!StateAuthorityPolicy.IsPolicyEligibleDomain(mutation.Domain))
        {
            throw new StateAuthorityException(
                "State Authority mutation reached policy evaluation without a valid classification.");
        }

        var autoApproved = policy.AutoApproveDomains.Contains(mutation.Domain);
        if (autoApproved)
        {
            return reviewChoice?.Choice switch
            {
                StateAuthorityReviewChoiceKind.Approve => new StateAuthorityDecision(
                    mutation.MutationIndex,
                    StateAuthorityDisposition.Approved,
                    ImmutableArray.Create(
                        StateAuthorityReasonCode.PolicyAutoApproved,
                        StateAuthorityReasonCode.ExplicitReviewApproved)),
                StateAuthorityReviewChoiceKind.Reject => new StateAuthorityDecision(
                    mutation.MutationIndex,
                    StateAuthorityDisposition.Rejected,
                    ImmutableArray.Create(
                        StateAuthorityReasonCode.PolicyAutoApproved,
                        StateAuthorityReasonCode.ExplicitReviewRejected)),
                null => new StateAuthorityDecision(
                    mutation.MutationIndex,
                    StateAuthorityDisposition.Approved,
                    ImmutableArray.Create(StateAuthorityReasonCode.PolicyAutoApproved)),
                _ => throw new StateAuthorityException(
                    "State Authority review choice is undefined.")
            };
        }

        return reviewChoice?.Choice switch
        {
            StateAuthorityReviewChoiceKind.Approve => new StateAuthorityDecision(
                mutation.MutationIndex,
                StateAuthorityDisposition.Approved,
                ImmutableArray.Create(
                    StateAuthorityReasonCode.PolicyReviewRequired,
                    StateAuthorityReasonCode.ExplicitReviewApproved)),
            StateAuthorityReviewChoiceKind.Reject => new StateAuthorityDecision(
                mutation.MutationIndex,
                StateAuthorityDisposition.Rejected,
                ImmutableArray.Create(
                    StateAuthorityReasonCode.PolicyReviewRequired,
                    StateAuthorityReasonCode.ExplicitReviewRejected)),
            null => new StateAuthorityDecision(
                mutation.MutationIndex,
                StateAuthorityDisposition.RequiresReview,
                ImmutableArray.Create(StateAuthorityReasonCode.PolicyReviewRequired)),
            _ => throw new StateAuthorityException(
                "State Authority review choice is undefined.")
        };
    }

    private static bool IsMandatoryReview(StateAuthorityMutationInput mutation)
    {
        if (StateAuthorityPolicy.IsAlwaysMandatoryReviewDomain(mutation.Domain))
        {
            return true;
        }

        return mutation.Domain == StateMutationDomain.UnresolvedProposition &&
            mutation.Transition is not AddStateAuthorityTransition;
    }

    private static string? ExistingRecordId(StateAuthorityTransition transition) =>
        transition switch
        {
            AddStateAuthorityTransition => null,
            SupersedeStateAuthorityTransition supersede => supersede.ExistingRecordId.Value,
            DeactivateStateAuthorityTransition deactivate => deactivate.ExistingRecordId.Value,
            _ => throw new StateAuthorityException(
                "State Authority mutation contains an unsupported transition.")
        };

    private static StateAuthorityRecordDomain RecordDomainForMutation(StateMutationDomain domain) =>
        domain switch
        {
            StateMutationDomain.WorldState => StateAuthorityRecordDomain.WorldState,
            StateMutationDomain.SceneState => StateAuthorityRecordDomain.SceneState,
            StateMutationDomain.UnresolvedProposition => StateAuthorityRecordDomain.UnresolvedProposition,
            StateMutationDomain.CharacterKnowledge => StateAuthorityRecordDomain.CharacterKnowledge,
            StateMutationDomain.CharacterBelief => StateAuthorityRecordDomain.CharacterBelief,
            StateMutationDomain.CharacterSuspicion => StateAuthorityRecordDomain.CharacterSuspicion,
            StateMutationDomain.CharacterMemory => StateAuthorityRecordDomain.CharacterMemory,
            StateMutationDomain.CharacterGoal => StateAuthorityRecordDomain.CharacterGoal,
            StateMutationDomain.CharacterDisposition => StateAuthorityRecordDomain.CharacterDisposition,
            StateMutationDomain.CharacterCircumstance => StateAuthorityRecordDomain.CharacterCircumstance,
            StateMutationDomain.CharacterClaim => StateAuthorityRecordDomain.CharacterClaim,
            StateMutationDomain.Relationship => StateAuthorityRecordDomain.Relationship,
            StateMutationDomain.Pressure => StateAuthorityRecordDomain.Pressure,
            _ => throw new StateAuthorityException(
                "State Authority mutation contains an undefined domain.")
        };

    private static bool IsLowerHexHash(string? value) =>
        value is { Length: 64 } &&
        value.All(character =>
            (character >= '0' && character <= '9') ||
            (character >= 'a' && character <= 'f'));
}
