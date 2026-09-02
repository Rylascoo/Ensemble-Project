using Ensemble.E0.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Domain;

[TestClass]
public sealed class StrongIdsTests
{
    [TestMethod]
    public void ValidStrongIds_StringifyToCanonicalValue()
    {
        Assert.AreEqual("fixture-family", FixtureFamilyId.From("fixture-family").ToString());
        Assert.AreEqual("fixture", FixtureId.From("fixture").ToString());
        Assert.AreEqual("scene", SceneId.From("scene").ToString());
        Assert.AreEqual("character", CharacterId.From("character").ToString());
        Assert.AreEqual("record", RecordId.From("record").ToString());
        Assert.AreEqual("run", RunId.From("run").ToString());
        Assert.AreEqual("take", TakeId.From("take").ToString());
        Assert.AreEqual("commit", CommitId.From("commit").ToString());
        Assert.AreEqual("context", ContextPacketId.From("context").ToString());
    }

    [TestMethod]
    public void FixtureFamilyId_RejectsVersionDelimiter()
    {
        Assert.Throws<ArgumentException>(() => FixtureFamilyId.From("fixture@0.1.0"));
    }

    [TestMethod]
    public void DefaultStrongIds_RejectStringification()
    {
        Assert.Throws<InvalidOperationException>(() => default(FixtureFamilyId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(FixtureId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(SceneId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(CharacterId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(RecordId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(RunId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(TakeId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(CommitId).ToString());
        Assert.Throws<InvalidOperationException>(() => default(ContextPacketId).ToString());
    }
}
