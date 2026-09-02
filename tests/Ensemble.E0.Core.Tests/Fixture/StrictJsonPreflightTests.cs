using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Fixture;

[TestClass]
public sealed class StrictJsonPreflightTests
{
    [TestMethod]
    public void EmptyFixture_IsRejected()
    {
        Assert.Throws<FixtureValidationException>(() => StrictJsonPreflight.Validate([]));
    }

    [TestMethod]
    public void FixtureOverOneMiB_IsRejectedBeforeParsing()
    {
        var bytes = new byte[(1024 * 1024) + 1];

        Assert.Throws<FixtureValidationException>(() => StrictJsonPreflight.Validate(bytes));
    }
}
