using System.Reflection;
using System.Text;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Performer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Performer;

[TestClass]
public sealed class PerformerCandidateBoundaryTests
{
    [TestMethod]
    public void NullContext_FailsWithCandidateDomainExceptionBeforeJsonInspection()
    {
        const string sentinel = "UNTRUSTED-PAYLOAD-SENTINEL";
        var bytes = Encoding.UTF8.GetBytes("{\"" + sentinel + "\":");

        var exception = Assert.Throws<PerformerCandidateException>(() =>
            PerformerCandidateContract.ParseJson(null!, bytes));

        Assert.AreEqual("ContextPacket is required.", exception.Message);
        Assert.IsFalse(
            exception.ToString().Contains(sentinel, StringComparison.Ordinal),
            exception.ToString());
    }

    [TestMethod]
    public void PublicParseSurface_IsExactlyContextPacketPlusCandidateBytes()
    {
        var method = typeof(PerformerCandidateContract).GetMethod(
            nameof(PerformerCandidateContract.ParseJson),
            BindingFlags.Public | BindingFlags.Static)!;
        var parameters = method.GetParameters();

        Assert.AreEqual(2, parameters.Length);
        Assert.AreEqual(typeof(ContextPacket), parameters[0].ParameterType);
        Assert.AreEqual(typeof(ReadOnlySpan<byte>), parameters[1].ParameterType);
        Assert.AreEqual(typeof(CandidatePerformance), method.ReturnType);
    }

    [TestMethod]
    public void CandidateException_HasNoPublicInnerExceptionConstructor()
    {
        var constructors = typeof(PerformerCandidateException)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        Assert.AreEqual(1, constructors.Length);
        CollectionAssert.AreEqual(
            new[] { typeof(string) },
            constructors[0].GetParameters().Select(parameter => parameter.ParameterType).ToArray());
    }
}
