using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Provenance;

namespace Ensemble.E0.Core.Fixture;

internal static class ProvenanceDagValidator
{
    public static void Validate(ValidatedFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        var graph = new List<KeyValuePair<RecordId, ImmutableArray<RecordId>>>();
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
                graph.Add(new KeyValuePair<RecordId, ImmutableArray<RecordId>>(
                    relationship.Id,
                    relationship.Provenance));
            }
        }

        try
        {
            RecordProvenanceGraphValidator.Validate(graph);
        }
        catch (RecordProvenanceGraphException exception)
        {
            throw exception.Failure switch
            {
                RecordProvenanceGraphFailure.MissingSupportingRecord when exception.RecordIdValue is not null =>
                    new FixtureValidationException(
                        $"Fixture provenance cites unknown record '{exception.RecordIdValue}'."),
                RecordProvenanceGraphFailure.Cycle =>
                    new FixtureValidationException("Fixture provenance must form an acyclic graph."),
                _ => new FixtureValidationException("Fixture provenance graph is invalid.")
            };
        }
    }

    private static void Add(
        ICollection<KeyValuePair<RecordId, ImmutableArray<RecordId>>> graph,
        ImmutableArray<ValidatedRecord> records)
    {
        foreach (var record in records)
        {
            graph.Add(new KeyValuePair<RecordId, ImmutableArray<RecordId>>(
                record.Id,
                record.Provenance));
        }
    }
}
