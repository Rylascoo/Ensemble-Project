using System.Reflection;
using System.Text;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Fixture;
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
    public void InvalidCanonicalCharacterIdSyntax_BecomesSanitizedCandidateException()
    {
        const string sentinel = "BAD/ID/SENTINEL";
        var packet = ComposeVoss();
        var addressJson =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[\"" +
            sentinel +
            "\"],\"nominatedCharacterId\":null}}";
        var nominationJson =
            "{\"schemaVersion\":\"ensemble.e0.performer.candidate-json.v1\",\"performance\":{\"text\":\"A\"},\"control\":{\"addressedCharacterIds\":[],\"nominatedCharacterId\":\"" +
            sentinel +
            "\"}}";

        foreach (var json in new[] { addressJson, nominationJson })
        {
            var exception = Assert.Throws<PerformerCandidateException>(() =>
                PerformerCandidateContract.ParseJson(
                    packet,
                    Encoding.UTF8.GetBytes(json)));
            Assert.IsFalse(
                exception.ToString().Contains(sentinel, StringComparison.Ordinal),
                exception.ToString());
        }
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

    private static ContextPacket ComposeVoss()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Fixtures",
            "missing-raft-0.1.0.json");
        var json = File.ReadAllText(path, Encoding.UTF8);
        var document = FixtureLoader.Load(Encoding.UTF8.GetBytes(json));
        var fixture = GenericE0FixtureValidator.Validate(document);
        MissingRaftContract.Validate(fixture);
        var projection = CharacterBoundedAccessControl.Evaluate(
            fixture,
            MissingRaftContract.VossId).Projection;
        return DeterministicContextComposer.Compose(
            projection,
            MissingRaftContract.VossId).Packet;
    }
}
