using Ensemble.E0.Core.Domain;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Tests;

internal static class E0AEvidenceStoreTestExtensions
{
    internal static void SealRuntime(
        this E0AFileEvidenceStore store,
        string terminalStatus,
        int acceptedTurns,
        decimal estimatedSpendUsd,
        string finalStateHash,
        CharacterId finalOpportunityCharacterId) =>
        store.SealRuntime(
            terminalStatus,
            acceptedTurns,
            estimatedSpendUsd,
            E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions,
            hasUnknownProviderUsage: false,
            finalStateHash,
            finalOpportunityCharacterId);
}
