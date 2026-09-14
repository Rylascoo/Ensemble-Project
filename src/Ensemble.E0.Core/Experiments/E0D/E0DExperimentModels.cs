using System.Collections.Immutable;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Director;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Experiments.E0D;

public enum E0DExperimentVariant
{
    FullReference = 1,
    RelationshipsOmitted = 2,
    OmniscientContext = 3,
    RoundRobin = 4
}

public static class E0DExperimentContracts
{
    public const string FullReferenceVariantId = "E0D-FULL-REFERENCE-01";
    public const string RelationshipsOmittedVariantId = "E0D-RELATIONSHIPS-OMITTED-01";
    public const string OmniscientContextVariantId = "E0D-OMNISCIENT-CONTEXT-01";
    public const string RoundRobinVariantId = "E0D-ROUND-ROBIN-01";
    public const string RelationshipsOmittedCompositionContract =
        E0ContextContracts.E0DRelationshipsOmittedCompositionContract;
    public const string OmniscientCompositionContract =
        E0ContextContracts.E0DOmniscientCompositionContract;
    public const string RoundRobinStrategyContract =
        "ensemble.e0.director.e0d.round-robin.v1";

    internal static bool IsContextAblationCompositionContract(string value) =>
        string.Equals(value, RelationshipsOmittedCompositionContract, StringComparison.Ordinal) ||
        string.Equals(value, OmniscientCompositionContract, StringComparison.Ordinal);

    public static string VariantId(E0DExperimentVariant variant) => variant switch
    {
        E0DExperimentVariant.FullReference => FullReferenceVariantId,
        E0DExperimentVariant.RelationshipsOmitted => RelationshipsOmittedVariantId,
        E0DExperimentVariant.OmniscientContext => OmniscientContextVariantId,
        E0DExperimentVariant.RoundRobin => RoundRobinVariantId,
        _ => throw new E0DExperimentException("E0-D experiment variant is unsupported.")
    };

    public static E0DExperimentVariant ParseVariantId(string value) => value switch
    {
        FullReferenceVariantId => E0DExperimentVariant.FullReference,
        RelationshipsOmittedVariantId => E0DExperimentVariant.RelationshipsOmitted,
        OmniscientContextVariantId => E0DExperimentVariant.OmniscientContext,
        RoundRobinVariantId => E0DExperimentVariant.RoundRobin,
        _ => throw new E0DExperimentException("E0-D experiment variant ID is not frozen by the preregistration.")
    };
}


public sealed class E0DRoundRobinDirectorTrace
{
    internal E0DRoundRobinDirectorTrace(
        string strategyContract,
        DirectorOpportunityInput input,
        ImmutableArray<CharacterId> canonicalRoster,
        int sourceIndex)
    {
        StrategyContract = strategyContract;
        Input = input;
        CanonicalRoster = canonicalRoster;
        SourceIndex = sourceIndex;
    }

    public string StrategyContract { get; }
    public DirectorOpportunityInput Input { get; }
    public ImmutableArray<CharacterId> CanonicalRoster { get; }
    public int SourceIndex { get; }
}

public sealed class E0DRoundRobinDirectorEvaluation
{
    internal E0DRoundRobinDirectorEvaluation(
        DirectorOpportunityProposal proposal,
        E0DRoundRobinDirectorTrace trace)
    {
        Proposal = proposal;
        Trace = trace;
    }

    public DirectorOpportunityProposal Proposal { get; }
    public E0DRoundRobinDirectorTrace Trace { get; }
}

public sealed class E0DExperimentException : Exception
{
    public E0DExperimentException(string message) : base(message) { }
    public E0DExperimentException(string message, Exception innerException) : base(message, innerException) { }
}