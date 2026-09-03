using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Core.Provenance;

internal enum RecordProvenanceGraphFailure
{
    DuplicateRecordId,
    MissingSupportingRecord,
    Cycle
}

internal sealed class RecordProvenanceGraphException : Exception
{
    internal RecordProvenanceGraphException(
        RecordProvenanceGraphFailure failure,
        string? recordIdValue = null)
        : base("Record provenance graph is invalid.")
    {
        Failure = failure;
        RecordIdValue = recordIdValue;
    }

    internal RecordProvenanceGraphFailure Failure { get; }
    internal string? RecordIdValue { get; }
}

internal static class RecordProvenanceGraphValidator
{
    internal static void Validate(
        IEnumerable<KeyValuePair<RecordId, ImmutableArray<RecordId>>> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        var graph = new Dictionary<string, ImmutableArray<RecordId>>(StringComparer.Ordinal);
        foreach (var pair in records)
        {
            var recordId = RequireInitialized(pair.Key);
            if (pair.Value.IsDefault)
            {
                throw new RecordProvenanceGraphException(
                    RecordProvenanceGraphFailure.MissingSupportingRecord);
            }

            if (!graph.TryAdd(recordId, pair.Value))
            {
                throw new RecordProvenanceGraphException(
                    RecordProvenanceGraphFailure.DuplicateRecordId,
                    recordId);
            }
        }

        var indegree = graph.Keys.ToDictionary(value => value, _ => 0, StringComparer.Ordinal);
        foreach (var sources in graph.Values)
        {
            foreach (var sourceId in sources)
            {
                var sourceValue = RequireInitialized(sourceId);
                if (!indegree.ContainsKey(sourceValue))
                {
                    throw new RecordProvenanceGraphException(
                        RecordProvenanceGraphFailure.MissingSupportingRecord,
                        sourceValue);
                }

                indegree[sourceValue]++;
            }
        }

        var ready = new Queue<string>(
            indegree
                .Where(pair => pair.Value == 0)
                .Select(pair => pair.Key)
                .OrderBy(value => value, StringComparer.Ordinal));
        var visitedCount = 0;

        while (ready.Count != 0)
        {
            var recordId = ready.Dequeue();
            visitedCount++;

            foreach (var sourceId in graph[recordId])
            {
                var sourceValue = sourceId.Value;
                indegree[sourceValue]--;
                if (indegree[sourceValue] == 0)
                {
                    ready.Enqueue(sourceValue);
                }
            }
        }

        if (visitedCount != graph.Count)
        {
            throw new RecordProvenanceGraphException(
                RecordProvenanceGraphFailure.Cycle);
        }
    }

    private static string RequireInitialized(RecordId id)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException)
        {
            throw new RecordProvenanceGraphException(
                RecordProvenanceGraphFailure.MissingSupportingRecord);
        }
    }
}
