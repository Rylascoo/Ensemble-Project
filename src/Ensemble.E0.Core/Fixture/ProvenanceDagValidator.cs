using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Fixture;

internal static class ProvenanceDagValidator
{
    public static void Validate(ValidatedFixture fixture)
    {
        var graph = new Dictionary<RecordId, ImmutableArray<RecordId>>();

        Add(graph, fixture.HistoricalTruth);
        Add(graph, fixture.UnresolvedPropositions);
        Add(graph, fixture.WorldState);
        Add(graph, fixture.SceneState);
        Add(graph, fixture.Pressures);

        foreach (var character in fixture.Characters)
        {
            Add(graph, character.Constitution);
            Add(graph, character.Disposition);
            Add(graph, character.Circumstance);
            Add(graph, character.Observations);
            Add(graph, character.Knowledge);
            Add(graph, character.Beliefs);
            Add(graph, character.Suspicions);
            Add(graph, character.Memories);
            Add(graph, character.Goals);

            foreach (var relationship in character.Relationships)
            {
                graph.Add(relationship.Id, relationship.Provenance);
            }
        }

        var visiting = new HashSet<RecordId>();
        var visited = new HashSet<RecordId>();

        foreach (var id in graph.Keys)
        {
            Visit(id, graph, visiting, visited);
        }
    }

    private static void Add(
        Dictionary<RecordId, ImmutableArray<RecordId>> graph,
        ImmutableArray<ValidatedRecord> records)
    {
        foreach (var record in records)
        {
            graph.Add(record.Id, record.Provenance);
        }
    }

    private static void Visit(
        RecordId id,
        IReadOnlyDictionary<RecordId, ImmutableArray<RecordId>> graph,
        HashSet<RecordId> visiting,
        HashSet<RecordId> visited)
    {
        if (visited.Contains(id))
        {
            return;
        }

        if (!visiting.Add(id))
        {
            throw new FixtureValidationException(
                $"Fixture provenance contains a cycle involving record '{id}'.");
        }

        foreach (var sourceId in graph[id])
        {
            Visit(sourceId, graph, visiting, visited);
        }

        visiting.Remove(id);
        visited.Add(id);
    }
}
