using System.Runtime.InteropServices;
using Ensemble.E0.Core.Fixture;

if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.Arm64)
{
    Console.Error.WriteLine("Ensemble E0 Harness requires native Windows ARM64 execution.");
    return 2;
}

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: Ensemble.E0.Harness <fixture.json>");
    return 2;
}

try
{
    var bytes = await File.ReadAllBytesAsync(args[0]);
    var document = FixtureLoader.Load(bytes);
    var fixture = GenericE0FixtureValidator.Validate(document);

    Console.WriteLine($"Fixture validated: {fixture.Id}");
    return 0;
}
catch (Exception exception) when (exception is FixtureValidationException or IOException or UnauthorizedAccessException)
{
    Console.Error.WriteLine(exception.Message);
    return 1;
}
