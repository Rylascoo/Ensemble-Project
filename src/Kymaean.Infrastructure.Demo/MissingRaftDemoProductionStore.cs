using System.Collections.Immutable;
using Ensemble.E0.Core.Fixture;
using Kymaean.Application;

namespace Kymaean.Infrastructure.Demo;

public sealed class MissingRaftDemoProductionStore : IProductionStore
{
    private readonly byte[] _fixtureBytes;

    public MissingRaftDemoProductionStore(ReadOnlySpan<byte> fixtureBytes)
    {
        if (fixtureBytes.IsEmpty)
        {
            throw new ArgumentException("Demo fixture bytes are required.", nameof(fixtureBytes));
        }

        _fixtureBytes = fixtureBytes.ToArray();
    }

    public static MissingRaftDemoProductionStore FromFile(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return new MissingRaftDemoProductionStore(File.ReadAllBytes(path));
    }

    public WorkspaceProjection LoadCurrent()
    {
        var document = FixtureLoader.Load(_fixtureBytes);
        var fixture = GenericE0FixtureValidator.Validate(document);

        var cast = fixture.Characters
            .Select(character => new WorkspaceCharacter(
                character.Id.Value,
                character.DisplayName))
            .ToImmutableArray();

        var situation = fixture.SceneState
            .Select(record => record.Text)
            .ToImmutableArray();

        var recentHistory = fixture.HistoricalTruth
            .TakeLast(Math.Min(3, fixture.HistoricalTruth.Length))
            .Select(record => record.Text)
            .ToImmutableArray();

        return new WorkspaceProjection(
            new StudioProjection("Missing Raft", cast, 1),
            new StageProjection(
                "Missing Raft",
                cast,
                situation,
                fixture.InitialOpportunity.Value),
            new ArchiveProjection(
                recentHistory,
                fixture.HistoricalTruth.Length));
    }
}
