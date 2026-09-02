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

        var indegree = graph.Keys.ToDictionary(id => id, _ => 0);

        foreach (var (_, sources) in graph)
        {
            foreach (var sourceId in sources)
            {
                if (!indegree.ContainsKey(sourceId))
                {
                    throw new FixtureValidationException(
                        $"Fixture provenance cites unknown record '{sourceId}'.");
                }

                indegree[sourceId]++;
            }
        }

        var ready = new Queue<RecordId>(indegree.Where(pair => pair.Value == 0).Select(pair => pair.Key));
        var visitedCount = 0;

        while (ready.Count > 0)
        {
            var id = ready.Dequeue();
            visitedCount++;

            foreach (var sourceId in graph[id])
            {
                indegree[sourceId]--;
                if (indegree[sourceId] == 0)
                {
                    ready.Enqueue(sourceId);
                }
            }
        }

        if (visitedCount != graph.Count)
        {
            throw new FixtureValidationException("Fixture provenance must form an acyclic graph.");
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
}
