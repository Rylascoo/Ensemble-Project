using Ensemble.E0.Harness.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0AHardeningLiveHostPreservationTests
{
    [TestMethod]
    public void LiveHost_HttpTransportCannotPreemptFrozenAttemptDeadline()
    {
        using var http = E0AReferenceRunHost.CreateProviderHttpClient();

        Assert.AreEqual(Timeout.InfiniteTimeSpan, http.Timeout);
    }
}
