using System.Collections.Immutable;
using Ensemble.E0.Core.Experiments.E0D;
using Ensemble.E0.Harness.Evidence;

namespace Ensemble.E0.Harness.Run;

internal static class E0DExperimentEvidenceContracts
{
    internal const string MethodIdentity = "E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01";
    internal const string MethodAuthorityCommit = "7959ffe59da72f735f86d4aec602224899577903";
    internal const string OmniscientHardGateChecklistVersion = "ensemble.e0d.hard-gates.omniscient.v1";
}

internal static class E0DHardGatePolicy
{
    private static readonly ImmutableArray<string> OmniscientReplacementChecks = ImmutableArray.Create(
        "Every disclosed record/category pair is inside the frozen category-preserving omniscient union.",
        "Every record required by the frozen omniscient union is present exactly once in its reference category, with relationship target identity preserved.",
        "No purely technical, credential, provider, hidden provenance-source, or non-context payload leaks into Character-facing context.",
        "The run is explicitly labeled and provenanced as the E0-D omniscient ablation.");

    internal static string ChecklistVersion(E0DExperimentVariant variant) =>
        variant == E0DExperimentVariant.OmniscientContext
            ? E0DExperimentEvidenceContracts.OmniscientHardGateChecklistVersion
            : E0AEvidenceContracts.HardGateChecklistVersion;

    internal static ImmutableArray<string> Checklist(E0DExperimentVariant variant) =>
        variant == E0DExperimentVariant.OmniscientContext
            ? OmniscientReplacementChecks.AddRange(E0AEvidenceContracts.HardGateChecklist.Skip(2))
            : E0AEvidenceContracts.HardGateChecklist;
}