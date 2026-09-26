using Kymaean.Application;

namespace Kymaean.Infrastructure.Execution;

public sealed class ProductPerformer(IProductExecutionRuntime runtime) : IProductPerformer
{
    private readonly IProductExecutionRuntime _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));

    public PerformanceCandidate Perform(CharacterPerformanceContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var request = new PerformerRequest(Guid.NewGuid(), context);
        var response = _runtime.Perform(request);
        if (response is null || response.InvocationId != request.InvocationId ||
            response.Completion != RuntimeCompletion.Completed || response.VisibleText is null)
            throw new ProductExecutorException("The runtime did not return a complete, attributed Performance.");
        try { return new PerformanceCandidate(response.VisibleText); }
        catch (ArgumentException error)
        {
            throw new ProductExecutorException("The runtime returned invalid Performance text.", error);
        }
    }
}
