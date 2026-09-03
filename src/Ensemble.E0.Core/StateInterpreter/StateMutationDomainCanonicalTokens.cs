namespace Ensemble.E0.Core.StateInterpreter;

internal static class StateMutationDomainCanonicalTokens
{
    internal static string Get(StateMutationDomain domain) =>
        domain switch
        {
            StateMutationDomain.WorldState => "worldState",
            StateMutationDomain.SceneState => "sceneState",
            StateMutationDomain.UnresolvedProposition => "unresolvedProposition",
            StateMutationDomain.CharacterKnowledge => "characterKnowledge",
            StateMutationDomain.CharacterBelief => "characterBelief",
            StateMutationDomain.CharacterSuspicion => "characterSuspicion",
            StateMutationDomain.CharacterMemory => "characterMemory",
            StateMutationDomain.CharacterGoal => "characterGoal",
            StateMutationDomain.CharacterDisposition => "characterDisposition",
            StateMutationDomain.CharacterCircumstance => "characterCircumstance",
            StateMutationDomain.CharacterClaim => "characterClaim",
            StateMutationDomain.Relationship => "relationship",
            StateMutationDomain.Pressure => "pressure",
            _ => throw new StateInterpretationException(
                "State mutation domain cannot be canonicalized.")
        };
}
