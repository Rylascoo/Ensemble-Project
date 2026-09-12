using System.Runtime.InteropServices;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;

if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.Arm64)
{
    Console.Error.WriteLine("Ensemble E0 Harness requires native Windows ARM64 execution.");
    return 2;
}

try
{
    if (args.Length > 0 && string.Equals(args[0], "e0a-run", StringComparison.Ordinal))
    {
        using var cancellation = new CancellationTokenSource();
        ConsoleCancelEventHandler handler = (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellation.Cancel();
        };
        Console.CancelKeyPress += handler;
        try
        {
            return await E0AReferenceRunHost.RunAsync(args[1..], cancellation.Token);
        }
        finally
        {
            Console.CancelKeyPress -= handler;
        }
    }

    if (args.Length > 0 && string.Equals(args[0], "e0b-run", StringComparison.Ordinal))
    {
        using var cancellation = new CancellationTokenSource();
        ConsoleCancelEventHandler handler = (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellation.Cancel();
        };
        Console.CancelKeyPress += handler;
        try
        {
            return await E0BReferenceRunHost.RunAsync(args[1..], cancellation.Token);
        }
        finally
        {
            Console.CancelKeyPress -= handler;
        }
    }
    if (args.Length > 0 && string.Equals(args[0], "e0a-evaluate", StringComparison.Ordinal))
    {
        return E0AReferenceRunHost.SealEvaluation(args[1..]);
    }

    if (args.Length != 1)
    {
        Console.Error.WriteLine("Usage: Ensemble.E0.Harness <fixture.json>");
        Console.Error.WriteLine("   or: Ensemble.E0.Harness e0a-run <CREATIVE-NONE|CREATIVE-MINIMAL> <provider-profile> <fixture.json> <run-id> <evidence-root> <executable-commit>");
        Console.Error.WriteLine("   or: Ensemble.E0.Harness e0b-run <fixture.json> <run-id> <evidence-root> <executable-commit>");
        Console.Error.WriteLine("   or: Ensemble.E0.Harness e0a-evaluate <evidence-root> <reviewer-id> <method-id> <pass|fail> [finding ...]");
        return 2;
    }

    var bytes = await File.ReadAllBytesAsync(args[0]);
    var document = FixtureLoader.Load(bytes);
    var fixture = GenericE0FixtureValidator.Validate(document);

    if (fixture.FamilyId == MissingRaftContract.FamilyId)
    {
        MissingRaftContract.Validate(fixture);
    }

    Console.WriteLine($"Fixture validated: {fixture.Id}");
    return 0;
}
catch (OperationCanceledException)
{
    Console.Error.WriteLine("E0-A operation cancelled.");
    return 3;
}
catch (Exception exception) when (
    exception is FixtureValidationException or
    E0AHarnessException or
    IOException or
    UnauthorizedAccessException or
    ArgumentException)
{
    Console.Error.WriteLine(exception.Message);
    return 1;
}
