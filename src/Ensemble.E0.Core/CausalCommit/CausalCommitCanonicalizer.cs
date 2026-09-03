using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Serialization;
using Ensemble.E0.Core.StateAuthority;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Take;

namespace Ensemble.E0.Core.CausalCommit;

internal static class CausalCommitCanonicalizer
{
    internal static StateHash ComputeResultHash(
        StateHash parentStateHash,
        CommitId commitId,
        E0Take take,
        E0RecordMaterializationSet materializations,
        ProductionStateProjection resultProjection)
    {
        var builder = new StringBuilder();
        builder.Append('{');
        var first = true;
        AppendStringProperty(
            builder,
            ref first,
            "hashContract",
            ProductionStateContracts.StateHashContractVersion);
        AppendStringProperty(builder, ref first, "kind", "causalCommit");
        AppendStringProperty(builder, ref first, "parentStateHash", parentStateHash.Value);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "commitPayload");
        AppendCommitPayload(builder, commitId, take, materializations);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "resultProjection");
        ProductionStateCanonicalizer.AppendProjection(builder, resultProjection);
        builder.Append('}');

        var bytes = CanonicalJson.EncodeUtf8(builder.ToString());
        var digest = SHA256.HashData(bytes);
        return StateHash.Create(Convert.ToHexString(digest).ToLowerInvariant());
    }

    internal static byte[] SerializeCommitPayload(
        CommitId commitId,
        E0Take take,
        E0RecordMaterializationSet materializations)
    {
        var builder = new StringBuilder();
        AppendCommitPayload(builder, commitId, take, materializations);
        return CanonicalJson.EncodeUtf8(builder.ToString());
    }

    private static void AppendCommitPayload(
        StringBuilder builder,
        CommitId commitId,
        E0Take take,
        E0RecordMaterializationSet materializations)
    {
        builder.Append('{');
        var first = true;
        AppendStringProperty(
            builder,
            ref first,
            "schemaVersion",
            E0CausalCommitContracts.ContractVersion);
        AppendStringProperty(builder, ref first, "commitId", commitId.Value);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "take");
        AppendTake(builder, take);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "recordMaterializations");
        AppendMaterializations(builder, materializations);
        builder.Append('}');
    }

    private static void AppendTake(StringBuilder builder, E0Take take)
    {
        var authority = take.AuthorityEvaluation;
        var trace = authority.Trace;

        builder.Append('{');
        var first = true;
        AppendStringProperty(builder, ref first, "contractVersion", take.ContractVersion);
        AppendStringProperty(builder, ref first, "takeId", take.TakeId.Value);
        AppendStringProperty(builder, ref first, "disposition", "accepted");
        AppendStringProperty(builder, ref first, "performanceSubjectCharacterId", take.Performance.SubjectCharacterId.Value);
        AppendStringProperty(builder, ref first, "performanceContextPacketId", take.Performance.ContextPacketId.Value);
        AppendStringProperty(builder, ref first, "candidateContentIdentityContract", take.InterpretationProposal.CandidateContentIdentityContract);
        AppendStringProperty(builder, ref first, "candidateContentHash", take.InterpretationProposal.CandidateContentHash);
        AppendStringProperty(builder, ref first, "proposalContentIdentityContract", trace.Input.ProposalContentIdentityContract);
        AppendStringProperty(builder, ref first, "proposalContentHash", trace.Input.ProposalContentHash);
        AppendStringProperty(builder, ref first, "sourceSceneId", take.InterpretationProposal.SourceSceneId.Value);
        AppendStringProperty(builder, ref first, "authorityContractVersion", authority.ContractVersion);
        AppendStringProperty(builder, ref first, "authorityStatus", "complete");

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "authorityPolicy");
        AppendPolicy(builder, trace.Policy);

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "authorityReviewSet");
        AppendReviewSet(builder, trace.ReviewSet);

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "authorityDecisions");
        AppendDecisions(builder, authority);
        builder.Append('}');
    }

    private static void AppendPolicy(StringBuilder builder, StateAuthorityPolicy policy)
    {
        builder.Append('{');
        var first = true;
        AppendStringProperty(builder, ref first, "contractVersion", policy.ContractVersion);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "autoApproveDomains");
        builder.Append('[');
        for (var index = 0; index < policy.AutoApproveDomains.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            CanonicalJson.AppendString(builder, StateMutationDomainCanonicalTokens.Get(policy.AutoApproveDomains[index]));
        }

        builder.Append(']');
        builder.Append('}');
    }

    private static void AppendReviewSet(StringBuilder builder, StateAuthorityReviewSet reviewSet)
    {
        builder.Append('{');
        var first = true;
        AppendStringProperty(builder, ref first, "contractVersion", reviewSet.ContractVersion);
        AppendStringProperty(builder, ref first, "proposalContentIdentityContract", reviewSet.ProposalContentIdentityContract);
        AppendStringProperty(builder, ref first, "proposalContentHash", reviewSet.ProposalContentHash);
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "choices");
        builder.Append('[');
        for (var index = 0; index < reviewSet.Choices.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            var choice = reviewSet.Choices[index];
            builder.Append('{');
            var choiceFirst = true;
            AppendIntegerProperty(builder, ref choiceFirst, "mutationIndex", choice.MutationIndex);
            AppendStringProperty(
                builder,
                ref choiceFirst,
                "choice",
                choice.Choice switch
                {
                    StateAuthorityReviewChoiceKind.Approve => "approve",
                    StateAuthorityReviewChoiceKind.Reject => "reject",
                    _ => throw new E0CausalCommitException("Causal commit cannot canonicalize an undefined review choice.")
                });
            builder.Append('}');
        }

        builder.Append(']');
        builder.Append('}');
    }

    private static void AppendDecisions(StringBuilder builder, StateAuthorityEvaluation authority)
    {
        builder.Append('[');
        for (var index = 0; index < authority.Decisions.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            var decision = authority.Decisions[index];
            builder.Append('{');
            var decisionFirst = true;
            AppendIntegerProperty(builder, ref decisionFirst, "mutationIndex", decision.MutationIndex);
            AppendStringProperty(
                builder,
                ref decisionFirst,
                "disposition",
                decision.Disposition switch
                {
                    StateAuthorityDisposition.Approved => "approved",
                    StateAuthorityDisposition.Rejected => "rejected",
                    _ => throw new E0CausalCommitException("Causal commit cannot canonicalize a nonterminal authority decision.")
                });
            CanonicalJson.AppendSeparator(builder, ref decisionFirst);
            CanonicalJson.AppendPropertyName(builder, "reasons");
            builder.Append('[');
            for (var reasonIndex = 0; reasonIndex < decision.Reasons.Length; reasonIndex++)
            {
                if (reasonIndex != 0)
                {
                    builder.Append(',');
                }

                CanonicalJson.AppendString(builder, ReasonToken(decision.Reasons[reasonIndex]));
            }

            builder.Append(']');
            builder.Append('}');
        }

        builder.Append(']');
    }

    private static void AppendMaterializations(StringBuilder builder, E0RecordMaterializationSet materializations)
    {
        builder.Append('[');
        for (var index = 0; index < materializations.Items.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            var item = materializations.Items[index];
            builder.Append('{');
            var first = true;
            AppendIntegerProperty(builder, ref first, "mutationIndex", item.MutationIndex);
            AppendStringProperty(builder, ref first, "recordId", item.RecordId.Value);
            builder.Append('}');
        }

        builder.Append(']');
    }

    private static string ReasonToken(StateAuthorityReasonCode reason) =>
        reason switch
        {
            StateAuthorityReasonCode.SupportingRecordMissing => "supportingRecordMissing",
            StateAuthorityReasonCode.ExistingRecordMissing => "existingRecordMissing",
            StateAuthorityReasonCode.ExistingRecordInactive => "existingRecordInactive",
            StateAuthorityReasonCode.ExistingRecordDomainMismatch => "existingRecordDomainMismatch",
            StateAuthorityReasonCode.ExistingRecordSubjectMismatch => "existingRecordSubjectMismatch",
            StateAuthorityReasonCode.ExistingRecordTargetMismatch => "existingRecordTargetMismatch",
            StateAuthorityReasonCode.ExistingRecordProtected => "existingRecordProtected",
            StateAuthorityReasonCode.ConflictingExistingRecordTarget => "conflictingExistingRecordTarget",
            StateAuthorityReasonCode.MandatoryReview => "mandatoryReview",
            StateAuthorityReasonCode.PolicyReviewRequired => "policyReviewRequired",
            StateAuthorityReasonCode.PolicyAutoApproved => "policyAutoApproved",
            StateAuthorityReasonCode.ExplicitReviewApproved => "explicitReviewApproved",
            StateAuthorityReasonCode.ExplicitReviewRejected => "explicitReviewRejected",
            _ => throw new E0CausalCommitException("Causal commit cannot canonicalize an undefined authority reason.")
        };

    private static void AppendStringProperty(StringBuilder builder, ref bool first, string name, string value)
    {
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, name);
        CanonicalJson.AppendString(builder, value);
    }

    private static void AppendIntegerProperty(StringBuilder builder, ref bool first, string name, int value)
    {
        if (value < 0)
        {
            throw new E0CausalCommitException("Causal commit canonical integer must be nonnegative.");
        }

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, name);
        builder.Append(value.ToString(CultureInfo.InvariantCulture));
    }
}
