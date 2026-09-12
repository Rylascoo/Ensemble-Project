using System.Text.Json;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Ensemble.E0.Harness.Evidence;
using Ensemble.E0.Harness.Host;
using Ensemble.E0.Harness.Run;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class E0BMixedCastTests
{
    [TestMethod]
    public void ApprovedCondition_PinsOnlyVossPerformerToGemini31()
    {
        var envelope = E0BReferenceRunHost.ReferenceEnvelope();
        var cast = E0BMixedCastConfiguration.Approved01(envelope);

        Assert.AreEqual(E0BMixedCastConfiguration.ApprovedConditionId, cast.ConditionId);
        Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35Minimal.Model,
            cast.PerformerFor(CharacterId.From(MissingRaftContract.MarloweCharacterId)).Model);
        Assert.AreEqual(E0AGeminiModelCatalog.FlashLite31Minimal.Model,
            cast.PerformerFor(CharacterId.From(MissingRaftContract.VossCharacterId)).Model);
        Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35Minimal.Model,
            cast.PerformerFor(CharacterId.From(MissingRaftContract.WrenCharacterId)).Model);
        Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35Minimal.Model, envelope.Integrity.Model);
        Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35Minimal.Model, envelope.Interpreter.Model);
    }

    [TestMethod]
    public void ApprovedCondition_PreparedIdentityRejectsSameTurnCrossRouteReceiptReuse()
    {
        var runId = RunId.From("E0B-MIXED-CAST-ROUTE-IDENTITY");
        var envelope = E0BReferenceRunHost.ReferenceEnvelope();
        var cast = E0BMixedCastConfiguration.Approved01(envelope);
        var cycle = DeterministicE0CausalCycle.Initialize(E0ATestSupport.Genesis());
        var context = DeterministicE0CausalCycle.ComposeContext(cycle).ContextEvaluation.Packet;
        Assert.AreEqual(MissingRaftContract.VossCharacterId, context.SubjectCharacterId.Value);

        var approved = E0ARequestBuilder.Performer(runId, 1, cast.PerformerFor(context.SubjectCharacterId), context);
        var wrongRoute = E0ARequestBuilder.Performer(runId, 1, envelope.Performer, context);
        Assert.AreEqual(approved.AttemptId, wrongRoute.AttemptId);
        Assert.AreNotEqual(approved.IdentityHash, wrongRoute.IdentityHash);

        var wrongReceipt = E0ATestSupport.Success(wrongRoute, E0ATestSupport.PerformerOutput());
        Assert.Throws<E0AHarnessException>(() => ConfiguredRoleAttemptBoundary.Accept(approved, wrongReceipt));
    }

    [TestMethod]
    public async Task ApprovedCondition_DriverRoutesEachPerformerAndKeepsOtherRolesOnReference()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0B-MIXED-CAST-FAKE-FULL");
            var state = E0ATestSupport.Genesis();
            var envelope = E0BReferenceRunHost.ReferenceEnvelope();
            var cast = E0BMixedCastConfiguration.Approved01(envelope);
            var provider = new ScriptedProvider();
            var counter = new FixedTokenCounter();
            var evidence = Evidence(root, runId, envelope, cast, state);
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                counter,
                evidence,
                mixedCast: cast,
                rateRouter: E0ANoopGeminiRateRouter.Instance);

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.AcceptedTurnCapReached, result.Status);
            Assert.AreEqual(12, result.AcceptedTurns);
            Assert.AreEqual(36, provider.Attempts.Count);
            Assert.AreEqual(36, counter.Calls);
            AssertPerformerRouting(provider.Attempts, cast);
            AssertReferenceRoleRouting(provider.Attempts, envelope);

            var performer = provider.Attempts.First(x => x.Profile.Role == E0ARole.Performer);
            var integrity = provider.Attempts.First(x => x.Profile.Role == E0ARole.Integrity && x.Turn == performer.Turn);
            var performerReceipt = E0ATestSupport.Success(performer, E0ATestSupport.PerformerOutput());
            Assert.Throws<E0AHarnessException>(() => ConfiguredRoleAttemptBoundary.Accept(integrity, performerReceipt));
        }
        finally
        {
            Delete(root);
        }
    }
    [TestMethod]
    public async Task ApprovedCondition_FailsClosedOnWrongReturnedModelForRoute()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0B-MIXED-CAST-WRONG-MODEL");
            var state = E0ATestSupport.Genesis();
            var envelope = E0BReferenceRunHost.ReferenceEnvelope();
            var cast = E0BMixedCastConfiguration.Approved01(envelope);
            var provider = new ScriptedProvider((attempt, _) => E0ATestSupport.Success(
                attempt,
                E0ATestSupport.PerformerOutput(),
                model: "wrong-model"));
            var evidence = Evidence(root, runId, envelope, cast, state);
            var driver = new E0AReferenceRunDriver(
                envelope,
                provider,
                new FixedTokenCounter(),
                evidence,
                mixedCast: cast,
                rateRouter: E0ANoopGeminiRateRouter.Instance);

            var result = await driver.RunAsync(runId, state, CancellationToken.None);

            Assert.AreEqual(E0ARunTerminalStatus.ModelIdentityChanged, result.Status);
            Assert.AreEqual(0, result.AcceptedTurns);
            Assert.AreEqual(1, provider.Calls);
        }
        finally
        {
            Delete(root);
        }
    }
    [TestMethod]
    public void ApprovedCondition_ManifestCarriesFixedCastRoutesAndSharedPacing()
    {
        var root = E0ATestSupport.TempRunRoot();
        try
        {
            var runId = RunId.From("E0B-MIXED-CAST-MANIFEST");
            var state = E0ATestSupport.Genesis();
            var envelope = E0BReferenceRunHost.ReferenceEnvelope();
            var cast = E0BMixedCastConfiguration.Approved01(envelope);
            _ = Evidence(root, runId, envelope, cast, state);

            using var document = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, "manifest.json")));
            var manifest = document.RootElement;
            Assert.AreEqual("ensemble.e0b.run-manifest.v1", manifest.GetProperty("contract").GetString());
            Assert.AreEqual(E0BMixedCastConfiguration.ApprovedConditionId, manifest.GetProperty("conditionId").GetString());
            Assert.AreEqual(E0AEvidenceContracts.E0BMixedCastApprovedCommit, manifest.GetProperty("approvedBlueprintCommit").GetString());
            Assert.AreEqual(JsonValueKind.Null, manifest.GetProperty("providerProfileId").ValueKind);
            Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35MinimalId,
                manifest.GetProperty("referenceProviderProfileId").GetString());
            Assert.AreEqual(JsonValueKind.Null, manifest.GetProperty("roles").ValueKind);
            Assert.AreEqual(3, manifest.GetProperty("referenceRoles").GetArrayLength());

            var castEntries = manifest.GetProperty("performerCast").EnumerateArray().ToArray();
            Assert.AreEqual(3, castEntries.Length);
            Assert.AreEqual(E0AGeminiModelCatalog.FlashLite31MinimalId,
                ProfileFor(castEntries, MissingRaftContract.VossCharacterId));
            Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35MinimalId,
                ProfileFor(castEntries, MissingRaftContract.MarloweCharacterId));
            Assert.AreEqual(E0AGeminiModelCatalog.FlashLite35MinimalId,
                ProfileFor(castEntries, MissingRaftContract.WrenCharacterId));

            var transport = manifest.GetProperty("providerTransport");
            Assert.AreEqual("route-specific-plus-shared-gemini-project-aggregate",
                transport.GetProperty("rateDisciplineScope").GetString());
            Assert.AreEqual(15, transport.GetProperty("sharedProjectRequestsPerMinute").GetInt32());
            Assert.AreEqual(250_000L, transport.GetProperty("sharedProjectInputTokensPerMinute").GetInt64());
            Assert.AreEqual(72, transport.GetProperty("worstCaseRunProviderRequests").GetInt32());

            var pricingRoutes = manifest.GetProperty("pricing").GetProperty("routes").EnumerateArray().ToArray();
            Assert.AreEqual(2, pricingRoutes.Length);
            Assert.AreEqual(0.25m, RoutePricing(pricingRoutes, E0AGeminiModelCatalog.FlashLite31MinimalId));
            Assert.AreEqual(0.30m, RoutePricing(pricingRoutes, E0AGeminiModelCatalog.FlashLite35MinimalId));
        }
        finally
        {
            Delete(root);
        }
    }
    [TestMethod]
    public async Task MixedRateRouter_PacesAcrossDifferentRoutesAtSharedProjectBoundary()
    {
        var envelope = E0BReferenceRunHost.ReferenceEnvelope();
        var cast = E0BMixedCastConfiguration.Approved01(envelope);
        var clock = new FakeClock();
        using var router = new E0BMixedGeminiRateRouter(cast, clock);
        var first = cast.PerformerFor(CharacterId.From(MissingRaftContract.MarloweCharacterId));
        var second = cast.PerformerFor(CharacterId.From(MissingRaftContract.VossCharacterId));

        await router.BeforeRequestAsync(first, E0AGeminiRequestKind.CountTokens, null, CancellationToken.None);
        await router.BeforeRequestAsync(second, E0AGeminiRequestKind.CountTokens, null, CancellationToken.None);

        CollectionAssert.AreEqual(new[] { TimeSpan.FromSeconds(4) }, clock.Delays.ToArray());
        Assert.AreEqual(1, router.RequestsFor(E0AGeminiModelCatalog.FlashLite35MinimalId));
        Assert.AreEqual(1, router.RequestsFor(E0AGeminiModelCatalog.FlashLite31MinimalId));
    }
    [TestMethod]
    public void SpendLedger_UsesSelectedRoutePricingWhileKeepingOneGlobalCeiling()
    {
        var ledger = new E0ASpendLedger(E0AGeminiModelCatalog.FlashLite35Minimal.ConservativeShadowPricing);
        var pricing31 = E0AGeminiModelCatalog.FlashLite31Minimal.ConservativeShadowPricing;
        var reservation31 = ledger.Reserve(1_000, 1_000, pricing31, 1_048_576);
        var expected31 = (1_000m / 1_000_000m * 0.25m) + (1_000m / 1_000_000m * 1.50m);
        Assert.AreEqual(expected31, reservation31.ReservedUsd);
        _ = ledger.Reconcile(reservation31, new E0AUsage(1_000, 1_000));

        var pricing35 = E0AGeminiModelCatalog.FlashLite35Minimal.ConservativeShadowPricing;
        var reservation35 = ledger.Reserve(1_000, 1_000, pricing35, 1_048_576);
        var expected35 = (1_000m / 1_000_000m * 0.30m) + (1_000m / 1_000_000m * 2.50m);
        Assert.AreEqual(expected35, reservation35.ReservedUsd);
        var reconciliation35 = ledger.Reconcile(reservation35, new E0AUsage(1_000, 1_000));

        Assert.AreEqual(expected31 + expected35, ledger.EstimatedCommittedUsd);
        Assert.IsFalse(reconciliation35.RunCeilingExceeded);
    }

    private static E0AFileEvidenceStore Evidence(
        string root,
        RunId runId,
        E0ARunEnvelope envelope,
        E0BMixedCastConfiguration cast,
        Ensemble.E0.Core.Production.ProductionState state) =>
        new(
            root,
            runId,
            envelope,
            state.OriginFixtureId.Value,
            state.OriginFixtureVersion.Value,
            state.OriginFixtureHash,
            E0ATestSupport.TestExecutableCommit,
            state.RosterCharacterIds,
            cast);
    private static void AssertPerformerRouting(
        IReadOnlyList<PreparedRoleAttempt> attempts,
        E0BMixedCastConfiguration cast)
    {
        var performers = attempts.Where(x => x.Profile.Role == E0ARole.Performer).ToArray();
        Assert.AreEqual(12, performers.Length);
        var vossSeen = false;
        foreach (var attempt in performers)
        {
            Assert.IsTrue(attempt.CharacterId.HasValue);
            var characterId = attempt.CharacterId!.Value;
            var expected = cast.PerformerFor(characterId);
            Assert.AreEqual(expected.Model, attempt.Profile.Model);
            Assert.AreEqual(expected.Reasoning, attempt.Profile.Reasoning);
            if (characterId.Value == MissingRaftContract.VossCharacterId)
            {
                vossSeen = true;
                Assert.AreEqual(E0AGeminiModelCatalog.FlashLite31Minimal.Model, attempt.Profile.Model);
            }
        }
        Assert.IsTrue(vossSeen, "The fixed alternate VOSS route must be exercised by the 12-turn fake run.");
    }

    private static void AssertReferenceRoleRouting(
        IReadOnlyList<PreparedRoleAttempt> attempts,
        E0ARunEnvelope envelope)
    {
        var integrity = attempts.Where(x => x.Profile.Role == E0ARole.Integrity).ToArray();
        var interpreter = attempts.Where(x => x.Profile.Role == E0ARole.Interpreter).ToArray();
        Assert.AreEqual(12, integrity.Length);
        Assert.AreEqual(12, interpreter.Length);
        Assert.IsTrue(integrity.All(x => x.Profile.Model == envelope.Integrity.Model));
        Assert.IsTrue(interpreter.All(x => x.Profile.Model == envelope.Interpreter.Model));
        Assert.IsTrue(integrity.All(x => x.Profile.Reasoning == E0AReasoningLevel.High));
        Assert.IsTrue(interpreter.All(x => x.Profile.Reasoning == E0AReasoningLevel.Minimal));
    }
    private static string? ProfileFor(JsonElement[] entries, string characterId) =>
        entries.Single(x => x.GetProperty("characterId").GetString() == characterId)
            .GetProperty("providerProfileId").GetString();

    private static decimal RoutePricing(JsonElement[] entries, string profileId) =>
        entries.Single(x => x.GetProperty("providerProfileId").GetString() == profileId)
            .GetProperty("publishedInputUsdPerMillionTokens").GetDecimal();

    private static void Delete(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private sealed class FakeClock : IE0AGeminiRateClock
    {
        private long _milliseconds;
        internal List<TimeSpan> Delays { get; } = new();

        public long GetTimestamp() => _milliseconds;

        public TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp) =>
            TimeSpan.FromMilliseconds(endingTimestamp - startingTimestamp);

        public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Delays.Add(delay);
            _milliseconds += checked((long)Math.Round(delay.TotalMilliseconds, MidpointRounding.AwayFromZero));
            return Task.CompletedTask;
        }
    }
}
