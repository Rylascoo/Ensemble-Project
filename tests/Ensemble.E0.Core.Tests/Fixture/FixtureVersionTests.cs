using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Fixture;

[TestClass]
public sealed class FixtureVersionTests
{
    [TestMethod]
    public void CanonicalStableVersion_IsAccepted()
    {
        Assert.AreEqual("0.1.0", FixtureVersion.From("0.1.0").ToString());
    }

    [TestMethod]
    [DataRow("01.0.0")]
    [DataRow("1.0")]
    [DataRow("1.0.0-beta")]
    [DataRow("1.0.0+build")]
    [DataRow("")]
    public void NonCanonicalOrUnsupportedVersion_IsRejected(string value)
    {
        Assert.Throws<ArgumentException>(() => FixtureVersion.From(value));
    }

    [TestMethod]
    public void DefaultVersion_RejectsStringification()
    {
        Assert.Throws<InvalidOperationException>(() => default(FixtureVersion).ToString());
    }
}
