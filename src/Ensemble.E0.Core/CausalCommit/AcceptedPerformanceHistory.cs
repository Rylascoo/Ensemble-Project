using System.Collections.Immutable;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Core.CausalCommit;

public sealed class E0AcceptedPerformanceHistory
{
    internal E0AcceptedPerformanceHistory(
        SceneId sceneId,
        StateHash currentStateHash,
        ImmutableArray<ContextRecentPerformance> entries)
    {
        SceneId = sceneId;
        CurrentStateHash = currentStateHash;
        Entries = entries;
    }

    internal SceneId SceneId { get; }
    internal StateHash CurrentStateHash { get; }
    internal ImmutableArray<ContextRecentPerformance> Entries { get; }
}

internal sealed class E0AcceptedPerformanceHistoryInvariantException : Exception
{
    internal E0AcceptedPerformanceHistoryInvariantException(string message)
        : base(message)
    {
    }

    internal E0AcceptedPerformanceHistoryInvariantException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}

internal static class AcceptedPerformanceHistoryInvariants
{
    internal static E0AcceptedPerformanceHistory Create(
        SceneId sceneId,
        StateHash currentStateHash,
        ImmutableArray<ContextRecentPerformance> entries)
    {
        ValidateCore(sceneId, currentStateHash, entries);
        return new E0AcceptedPerformanceHistory(sceneId, currentStateHash, entries);
    }

    internal static ImmutableArray<ContextRecentPerformance> ValidateAndProject(
        E0AcceptedPerformanceHistory history,
        SceneId expectedSceneId,
        StateHash expectedStateHash,
        ImmutableArray<CharacterId> currentRoster)
    {
        if (history is null)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                "Accepted Performance history is required.");
        }

        ValidateCore(history.SceneId, history.CurrentStateHash, history.Entries);
        RequireInitialized(expectedSceneId, "expected SceneId");
        RequireInitialized(expectedStateHash, "expected StateHash");

        if (history.SceneId != expectedSceneId ||
            history.CurrentStateHash != expectedStateHash)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                "Accepted Performance history is not synchronized with the expected Production state.");
        }

        if (currentRoster.IsDefault || currentRoster.Length == 0)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                "Accepted Performance history roster is invalid.");
        }

        var rosterValues = new HashSet<string>(StringComparer.Ordinal);
        foreach (var characterId in currentRoster)
        {
            var value = RequireInitialized(characterId, "roster CharacterId");
            if (!rosterValues.Add(value))
            {
                throw new E0AcceptedPerformanceHistoryInvariantException(
                    "Accepted Performance history roster contains duplicate Character IDs.");
            }
        }

        foreach (var entry in history.Entries)
        {
            var source = RequireInitialized(
                entry.SourceCharacterId,
                "source CharacterId");
            if (!rosterValues.Contains(source))
            {
                throw new E0AcceptedPerformanceHistoryInvariantException(
                    "Accepted Performance history contains a source Character outside the current roster.");
            }
        }

        return history.Entries;
    }

    internal static E0AcceptedPerformanceHistory Append(
        E0AcceptedPerformanceHistory source,
        StateHash resultStateHash,
        CharacterId sourceCharacterId,
        string visibleText)
    {
        if (source is null)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                "Accepted Performance source history is required.");
        }

        ValidateCore(source.SceneId, source.CurrentStateHash, source.Entries);
        RequireInitialized(resultStateHash, "result StateHash");
        RequireInitialized(sourceCharacterId, "source CharacterId");
        ValidateVisibleText(visibleText);

        return new E0AcceptedPerformanceHistory(
            source.SceneId,
            resultStateHash,
            source.Entries.Add(
                new ContextRecentPerformance(sourceCharacterId, visibleText)));
    }

    internal static E0AcceptedPerformanceHistory AdvanceState(
        E0AcceptedPerformanceHistory source,
        StateHash resultStateHash)
    {
        if (source is null)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                "Accepted Performance source history is required.");
        }

        ValidateCore(source.SceneId, source.CurrentStateHash, source.Entries);
        RequireInitialized(resultStateHash, "result StateHash");
        return new E0AcceptedPerformanceHistory(
            source.SceneId,
            resultStateHash,
            source.Entries);
    }

    private static void ValidateCore(
        SceneId sceneId,
        StateHash currentStateHash,
        ImmutableArray<ContextRecentPerformance> entries)
    {
        RequireInitialized(sceneId, "SceneId");
        RequireInitialized(currentStateHash, "StateHash");
        if (entries.IsDefault)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                "Accepted Performance history entries are uninitialized.");
        }

        foreach (var entry in entries)
        {
            if (entry is null)
            {
                throw new E0AcceptedPerformanceHistoryInvariantException(
                    "Accepted Performance history contains an invalid entry.");
            }

            RequireInitialized(entry.SourceCharacterId, "source CharacterId");
            ValidateVisibleText(entry.VisibleText);
        }
    }

    private static void ValidateVisibleText(string? visibleText)
    {
        if (CharacterLegibleTextInvariants.Validate(visibleText) !=
            CharacterLegibleTextFailure.None)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                "Accepted Performance history text is invalid.");
        }
    }

    private static string RequireInitialized(CharacterId id, string fieldName)
    {
        try
        {
            return id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                $"Accepted Performance history {fieldName} is uninitialized.",
                exception);
        }
    }

    private static void RequireInitialized(SceneId id, string fieldName)
    {
        try
        {
            _ = id.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                $"Accepted Performance history {fieldName} is uninitialized.",
                exception);
        }
    }

    private static void RequireInitialized(StateHash hash, string fieldName)
    {
        try
        {
            _ = hash.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new E0AcceptedPerformanceHistoryInvariantException(
                $"Accepted Performance history {fieldName} is uninitialized.",
                exception);
        }
    }
}
