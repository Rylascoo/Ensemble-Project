using System.Collections.Immutable;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Execution;

// No runtime implementation, discovery, credentials or provider configuration lives here.
// A future admitted transport must preserve this actor-only context and exact output.
public interface IProductExecutionRuntime
{
    PerformerResponse Perform(PerformerRequest request);
    ConsequenceResponse Interpret(ConsequenceRequest request);
}

public sealed record PerformerRequest(Guid InvocationId, CharacterPerformanceContext Context);
public sealed record ConsequenceRequest(
    Guid InvocationId, CharacterPerformanceContext Context, PerformanceCandidate Performance);
public enum RuntimeCompletion { Completed, Refused, Failed, Truncated }
public sealed record PerformerResponse(Guid InvocationId, RuntimeCompletion Completion, string? VisibleText);
public sealed record RuntimeConsequence(string Family, string? Text);
public sealed record ConsequenceResponse(
    Guid InvocationId, RuntimeCompletion Completion, ImmutableArray<RuntimeConsequence> Consequences);

public sealed class ProductExecutorException : Exception
{
    public ProductExecutorException(string message, Exception? inner = null) : base(message, inner) { }
}
