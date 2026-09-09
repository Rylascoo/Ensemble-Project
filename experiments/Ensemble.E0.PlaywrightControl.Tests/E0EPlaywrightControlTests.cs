using System.Text;
using System.Text.Json;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.PlaywrightControl.Tests;

[TestClass]
public sealed class E0EPlaywrightControlTests
{
    [TestMethod]
    public void ContractHashes_AreFrozen()
    {
        Assert.AreEqual(
            "aaa94e69e18512ca54de1bf2288546b57a8d8a53075338f046a3ea12690eefa2",
            E0EPlaywrightContract.PromptSha256);
        Assert.AreEqual(
            "34bd76d442f1eeffc07049092c1d03a769f33a8e279ff2dd8765a0449213d219",
            E0EPlaywrightContract.SchemaSha256);
    }

    [TestMethod]
    public void ReferenceConfiguration_IsStableAndFailClosed()
    {
        var (_, reference) = FixtureAndReference();
        reference.Validate();
        var first = reference.IdentitySha256;
        var second = reference.IdentitySha256;
        Assert.AreEqual(first, second);
        Assert.AreEqual(64, first.Length);

        Assert.Throws<E0EPreparationException>(() =>
            (reference with { AttemptsPerInvocation = 2 }).Validate());
        Assert.Throws<E0EPreparationException>(() =>
            (reference with { AutomaticRetries = 1 }).Validate());
        Assert.Throws<E0EPreparationException>(() =>
            (reference with { AttemptTimeoutSeconds = 299 }).Validate());
        Assert.Throws<E0EPreparationException>(() =>
            (reference with { EstimatedSpendCeilingUsd = 5.01m }).Validate());
        Assert.Throws<E0EPreparationException>(() =>
            (reference with { MaxOutputTokens = 4097 }).Validate());
        Assert.Throws<E0EPreparationException>(() =>
            (reference with { SourceManifestSha256 = new string('A', 64) }).Validate());
    }

    [TestMethod]
    public void CompleteScenePacket_ContainsFrozenFixtureAndAllSemanticCategories()
    {
        var (fixture, reference) = FixtureAndReference();
        var speaker = fixture.Scene.Roster[0];
        var prior = new[] { new E0ERecordedTurn(1, speaker, "A visible first performance.") };

        var packet = E0ECompleteScenePacketBuilder.Build(fixture, reference, prior);
        using var document = JsonDocument.Parse(packet.Utf8);
        var root = document.RootElement;

        Assert.AreEqual("ensemble.e0e.complete-scene.v1", root.GetProperty("contract").GetString());
        Assert.AreEqual(2, root.GetProperty("currentTurn").GetInt32());
        Assert.AreEqual(reference.AcceptedTurnCap, root.GetProperty("acceptedTurnCap").GetInt32());
        Assert.AreEqual(fixture.Id.Value, root.GetProperty("fixture").GetProperty("id").GetString());
        Assert.AreEqual(reference.FixtureHash, root.GetProperty("fixture").GetProperty("hash").GetString());
        Assert.AreEqual(3, root.GetProperty("scene").GetProperty("roster").GetArrayLength());
        Assert.AreEqual(1, root.GetProperty("priorPerformances").GetArrayLength());
        Assert.IsFalse(root.TryGetProperty("initialOpportunityCharacterId", out _));

        Assert.AreEqual(JsonValueKind.Array, root.GetProperty("chronology").ValueKind);
        Assert.AreEqual(JsonValueKind.Array, root.GetProperty("historicalTruth").ValueKind);
        Assert.AreEqual(JsonValueKind.Array, root.GetProperty("unresolvedPropositions").ValueKind);
        Assert.AreEqual(JsonValueKind.Array, root.GetProperty("worldState").ValueKind);
        Assert.AreEqual(JsonValueKind.Array, root.GetProperty("sceneState").ValueKind);
        Assert.AreEqual(JsonValueKind.Array, root.GetProperty("pressures").ValueKind);

        var characters = root.GetProperty("characters");
        Assert.AreEqual(3, characters.GetArrayLength());
        foreach (var character in characters.EnumerateArray())
        {
            _ = character.GetProperty("constitution");
            _ = character.GetProperty("disposition");
            _ = character.GetProperty("circumstance");
            _ = character.GetProperty("observations");
            _ = character.GetProperty("knowledge");
            _ = character.GetProperty("beliefs");
            _ = character.GetProperty("suspicions");
            _ = character.GetProperty("memories");
            _ = character.GetProperty("goals");
            _ = character.GetProperty("relationships");
        }
    }

    [TestMethod]
    public void CompleteScenePacket_RejectsReferenceFixtureMismatch()
    {
        var (fixture, reference) = FixtureAndReference();
        var mismatched = reference with { FixtureHash = new string('c', 64) };
        mismatched.Validate();
        Assert.Throws<E0EPreparationException>(() =>
            E0ECompleteScenePacketBuilder.Build(fixture, mismatched, Array.Empty<E0ERecordedTurn>()));
    }

    [TestMethod]
    public void PlaywrightParser_AcceptsExactlyOneRosterPerformance()
    {
        var (fixture, _) = FixtureAndReference();
        var speaker = fixture.Scene.Roster[1];
        var bytes = ValidTurnBytes(speaker.Value, "I will answer for myself.");

        var parsed = E0EPlaywrightContract.Parse(bytes, fixture.Scene.Roster);

        Assert.AreEqual(speaker, parsed.SpeakerCharacterId);
        Assert.AreEqual("I will answer for myself.", parsed.Text);
    }

    [TestMethod]
    public void PlaywrightParser_RejectsUnknownDuplicateAndInvalidContent()
    {
        var (fixture, _) = FixtureAndReference();
        var speaker = fixture.Scene.Roster[0].Value;

        var extra = Encoding.UTF8.GetBytes(
            $"{{\"schemaVersion\":\"{E0EPlaywrightContract.SchemaVersion}\",\"speakerCharacterId\":\"{speaker}\",\"performance\":{{\"text\":\"ok\"}},\"extra\":true}}");
        Assert.Throws<E0EPreparationException>(() =>
            E0EPlaywrightContract.Parse(extra, fixture.Scene.Roster));

        var duplicate = Encoding.UTF8.GetBytes(
            $"{{\"schemaVersion\":\"{E0EPlaywrightContract.SchemaVersion}\",\"speakerCharacterId\":\"{speaker}\",\"speakerCharacterId\":\"{speaker}\",\"performance\":{{\"text\":\"ok\"}}}}");
        Assert.Throws<E0EPreparationException>(() =>
            E0EPlaywrightContract.Parse(duplicate, fixture.Scene.Roster));

        Assert.Throws<E0EPreparationException>(() =>
            E0EPlaywrightContract.Parse(ValidTurnBytes("outsider", "ok"), fixture.Scene.Roster));
        Assert.Throws<E0EPreparationException>(() =>
            E0EPlaywrightContract.Parse(ValidTurnBytes(speaker, "   "), fixture.Scene.Roster));
        Assert.Throws<E0EPreparationException>(() =>
            E0EPlaywrightContract.Parse(ValidTurnBytes(speaker, "bad\u0001text"), fixture.Scene.Roster));
        Assert.Throws<E0EPreparationException>(() =>
            E0EPlaywrightContract.Parse(ValidTurnBytes(speaker, "e\u0301"), fixture.Scene.Roster));

        var valid = ValidTurnBytes(speaker, "ok");
        var bom = new byte[valid.Length + 3];
        bom[0] = 0xEF;
        bom[1] = 0xBB;
        bom[2] = 0xBF;
        valid.CopyTo(bom, 3);
        Assert.Throws<E0EPreparationException>(() =>
            E0EPlaywrightContract.Parse(bom, fixture.Scene.Roster));
    }

    [TestMethod]
    public void Transcript_EnforcesSequentialTurnsAndCap()
    {
        var (fixture, reference) = FixtureAndReference();
        var bounded = reference with { AcceptedTurnCap = 2 };
        bounded.Validate();
        var transcript = new E0EControlTranscript(bounded, fixture.Scene.Roster);

        var first = transcript.Record(new E0EPlaywrightTurn(fixture.Scene.Roster[0], "First."));
        var second = transcript.Record(new E0EPlaywrightTurn(fixture.Scene.Roster[2], "Second."));

        Assert.AreEqual(1, first.Turn);
        Assert.AreEqual(2, second.Turn);
        Assert.Throws<E0EPreparationException>(() =>
            transcript.Record(new E0EPlaywrightTurn(fixture.Scene.Roster[1], "Third.")));
    }

    [TestMethod]
    public void BlindPackage_MatchesE0AShapeAndExcludesIdentityMetadata()
    {
        var (fixture, reference) = FixtureAndReference();
        var turns = new[]
        {
            new E0ERecordedTurn(1, fixture.Scene.Roster[2], "Line one."),
            new E0ERecordedTurn(2, fixture.Scene.Roster[0], "Line two.")
        };

        var package = E0EBlindPackageBuilder.Build(fixture.Scene.Roster, turns, reference.AcceptedTurnCap);
        using var transcriptDocument = JsonDocument.Parse(package.Transcript.Utf8);
        var root = transcriptDocument.RootElement;
        Assert.AreEqual(1, root.EnumerateObject().Count());
        var performances = root.GetProperty("performances");
        Assert.AreEqual(2, performances.GetArrayLength());
        foreach (var item in performances.EnumerateArray())
        {
            Assert.AreEqual(3, item.EnumerateObject().Count());
            _ = item.GetProperty("turn").GetInt32();
            Assert.IsTrue(item.GetProperty("speaker").GetString()!.StartsWith("SPEAKER-", StringComparison.Ordinal));
            _ = item.GetProperty("text").GetString();
        }

        var blindText = Encoding.UTF8.GetString(package.Transcript.Utf8.Span);
        foreach (var characterId in fixture.Scene.Roster.Select(id => id.Value))
        {
            Assert.IsFalse(blindText.Contains(characterId, StringComparison.Ordinal));
        }
        Assert.IsFalse(blindText.Contains(reference.Provider, StringComparison.Ordinal));
        Assert.IsFalse(blindText.Contains(reference.Model, StringComparison.Ordinal));

        using var mappingDocument = JsonDocument.Parse(package.Mapping.Utf8);
        Assert.AreEqual(3, mappingDocument.RootElement.GetProperty("mapping").GetArrayLength());
    }

    [TestMethod]
    public void PreparedRequestIdentity_BindsPriorControlHistoryAndRejectsMismatches()
    {
        var (fixture, reference) = FixtureAndReference();
        var emptyPacket = E0ECompleteScenePacketBuilder.Build(
            fixture,
            reference,
            Array.Empty<E0ERecordedTurn>());
        var first = E0EPreparedTurnRequest.Build(reference, emptyPacket, 1);

        Assert.Throws<E0EPreparationException>(() =>
            E0EPreparedTurnRequest.Build(reference, emptyPacket, 2));

        var tamperedText = Encoding.UTF8.GetString(emptyPacket.Utf8.Span)
            .Replace(reference.FixtureHash, new string('c', 64), StringComparison.Ordinal);
        var tamperedPacket = new E0EJsonArtifact(Encoding.UTF8.GetBytes(tamperedText));
        Assert.Throws<E0EPreparationException>(() =>
            E0EPreparedTurnRequest.Build(reference, tamperedPacket, 1));

        var prior = new[]
        {
            new E0ERecordedTurn(1, fixture.Scene.Roster[0], "A prior visible performance.")
        };
        var nextPacket = E0ECompleteScenePacketBuilder.Build(fixture, reference, prior);
        var second = E0EPreparedTurnRequest.Build(reference, nextPacket, 2);

        Assert.AreNotEqual(emptyPacket.Sha256, nextPacket.Sha256);
        Assert.AreNotEqual(first.RequestIdentitySha256, second.RequestIdentitySha256);
        Assert.AreEqual(reference.IdentitySha256, second.ReferenceConfigurationSha256);
        Assert.AreEqual(E0EPlaywrightContract.PromptSha256, second.PromptSha256);
        Assert.AreEqual(E0EPlaywrightContract.SchemaSha256, second.SchemaSha256);
    }

    [TestMethod]
    public void PreparationAssembly_HasNoDirectHttpClientDependency()
    {
        var references = typeof(E0EReferenceConfiguration).Assembly
            .GetReferencedAssemblies()
            .Select(name => name.Name)
            .ToArray();

        CollectionAssert.DoesNotContain(references, "System.Net.Http");
    }

    private static byte[] ValidTurnBytes(string speaker, string text) =>
        JsonSerializer.SerializeToUtf8Bytes(new
        {
            schemaVersion = E0EPlaywrightContract.SchemaVersion,
            speakerCharacterId = speaker,
            performance = new { text }
        });

    private static (ValidatedFixture Fixture, E0EReferenceConfiguration Reference) FixtureAndReference()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "missing-raft-0.1.0.json");
        var fixture = GenericE0FixtureValidator.Validate(FixtureLoader.Load(File.ReadAllBytes(path)));
        var reference = new E0EReferenceConfiguration(
            SourceRunId: "e0a-test-reference",
            SourceManifestSha256: new string('a', 64),
            SourceRuntimeSealIdentity: new string('b', 64),
            ReferenceConfigurationIdentity: "E0A-TEST-REFERENCE",
            Provider: "TEST-PROVIDER",
            Model: "TEST-MODEL",
            ProviderReturnedModel: null,
            ProviderProfileId: "TEST-PROFILE",
            ServiceTier: "TEST-TIER",
            CreativeReasoningControl: "TEST-CONTROL",
            CreativeReasoningValue: "TEST-VALUE",
            Stream: true,
            MaxOutputTokens: 4096,
            AcceptedTurnCap: 12,
            AttemptsPerInvocation: 1,
            AutomaticRetries: 0,
            AttemptTimeoutSeconds: 300,
            EstimatedSpendCeilingUsd: 5.00m,
            FixtureId: fixture.Id.Value,
            FixtureVersion: fixture.Version.Value,
            FixtureHash: FixtureHash.Compute(fixture));
        reference.Validate();
        return (fixture, reference);
    }
}
