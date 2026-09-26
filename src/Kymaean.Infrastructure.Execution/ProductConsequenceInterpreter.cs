using Kymaean.Application;

namespace Kymaean.Infrastructure.Execution;

public sealed class ProductConsequenceInterpreter(IProductExecutionRuntime runtime) : IProductConsequenceInterpreter
{
    private readonly IProductExecutionRuntime _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));

    public CharacterCircumstanceProposal Interpret(CharacterPerformanceContext context, PerformanceCandidate performance)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(performance);
        var request = new ConsequenceRequest(Guid.NewGuid(), context, performance);
        var response = _runtime.Interpret(request);
        if (response is null || response.InvocationId != request.InvocationId ||
            response.Completion != RuntimeCompletion.Completed || response.Consequences.IsDefault ||
            response.Consequences.Length != 1 || response.Consequences[0] is not { } consequence ||
            consequence.Family != "CharacterCircumstance" || string.IsNullOrWhiteSpace(consequence.Text))
            throw new ProductExecutorException("The runtime did not return exactly one attributed Character Circumstance.");
        // Product owns validation. Never trim, normalize, enrich or invent a consequence.
        return new CharacterCircumstanceProposal(consequence.Text);
    }
}
