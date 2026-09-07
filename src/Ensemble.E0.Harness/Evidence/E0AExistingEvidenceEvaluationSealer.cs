namespace Ensemble.E0.Harness.Evidence;

internal static class E0AExistingEvidenceEvaluationSealer
{
    internal static void Seal(string root, E0AHardGateEvaluation evaluation) =>
        E0AEvidenceSealAuthority.SealEvaluation(root, evaluation);
}
