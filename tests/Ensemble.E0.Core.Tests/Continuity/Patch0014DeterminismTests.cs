using System.Collections.Immutable;
using System.Globalization;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Continuity;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.Continuity;

[TestClass]
public sealed class Patch0014DeterminismTests
{
    [TestMethod]
    public void HiddenProductionAuthorityChange_ChangesStructuredIdentityButNotRenderedContext()
    {
        var fixture = Patch0012TestSupport.LoadMissingRaft();
        var baseline = ProductionState.Initialize(
            fixture,
            ImmutableArray<RecordId>.Empty);
        var locked = ProductionState.Initialize(
            fixture,
            ImmutableArray.Create(
                RecordId.From(MissingRaftContract.WorldCurrentStrengthenedId)));

        Assert.AreNotEqual(baseline.StateHash, locked.StateHash);

        var baselineContext = E0ProductionContextContinuity.Compose(
            ProductionStateCheckpoint.Capture(baseline)).ContextEvaluation.Packet;
        var lockedContext = E0ProductionContextContinuity.Compose(
            ProductionStateCheckpoint.Capture(locked)).ContextEvaluation.Packet;

        Assert.AreNotEqual(
            baselineContext.StructuredContextHash,
            lockedContext.StructuredContextHash);
        Assert.AreNotEqual(
            baselineContext.ContextPacketId,
            lockedContext.ContextPacketId);
        CollectionAssert.AreEqual(
            ContextPacketCanonicalizer.SerializeRendered(baselineContext.Rendered),
            ContextPacketCanonicalizer.SerializeRendered(lockedContext.Rendered));
        Assert.AreEqual(
            baselineContext.RenderedContextHash,
            lockedContext.RenderedContextHash);
        Assert.AreEqual(
            baselineContext.Rendered.TrustedStateText,
            lockedContext.Rendered.TrustedStateText);
    }

    [TestMethod]
    public void ProductionBoundContinuity_IsCultureInvariant()
    {
        var scenario = Patch0014TestSupport.Genesis();
        var expected = scenario.Result.ContextEvaluation.Packet;
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ar-SA");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ar-SA");

            var actual = E0ProductionContextContinuity.Compose(
                scenario.Checkpoint).ContextEvaluation.Packet;

            CollectionAssert.AreEqual(
                ContextPacketCanonicalizer.SerializeStructured(expected),
                ContextPacketCanonicalizer.SerializeStructured(actual));
            CollectionAssert.AreEqual(
                ContextPacketCanonicalizer.SerializeRendered(expected.Rendered),
                ContextPacketCanonicalizer.SerializeRendered(actual.Rendered));
            Assert.AreEqual(expected.ContextPacketId, actual.ContextPacketId);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }
}
