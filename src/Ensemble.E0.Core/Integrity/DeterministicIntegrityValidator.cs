namespace Ensemble.E0.Core.Integrity;

public static class DeterministicIntegrityValidator
{
    public static IntegrityValidationEvaluation Validate(
        IntegrityCandidateInput input,
        IntegrityConcernEvidence? concernEvidence)
    {
        IntegrityValidationInvariants.ValidateInput(input);

        if (input.DeterministicRejectCodes.Length != 0)
        {
            if (concernEvidence is not null)
            {
                throw new IntegrityValidationException(
                    "Deterministic Integrity Reject input must not include semantic concern evidence.");
            }

            return new IntegrityValidationEvaluation(
                IntegrityDisposition.Reject,
                new IntegrityValidationTrace(
                    E0IntegrityContracts.ValidationContract,
                    input,
                    concernEvidence: null));
        }

        IntegrityValidationInvariants.ValidateEvidence(input, concernEvidence);

        var disposition = concernEvidence!.Concerns.Length == 0
            ? IntegrityDisposition.Accept
            : IntegrityDisposition.RequestAnotherTake;

        return new IntegrityValidationEvaluation(
            disposition,
            new IntegrityValidationTrace(
                E0IntegrityContracts.ValidationContract,
                input,
                concernEvidence));
    }
}
