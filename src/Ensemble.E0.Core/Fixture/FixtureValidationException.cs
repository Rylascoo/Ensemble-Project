namespace Ensemble.E0.Core.Fixture;

public sealed class FixtureValidationException : Exception
{
    public FixtureValidationException(string message)
        : base(message)
    {
    }

    public FixtureValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
