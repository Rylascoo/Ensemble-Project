using Ensemble.E0.Core.Domain;

namespace Ensemble.E0.Harness.Run;

internal static class E0ADeterministicIds
{
    internal const int MaxRunIdLength = 96;

    internal static void ValidateRunId(RunId runId)
    {
        string value;
        try
        {
            value = runId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0AHarnessException("E0-A RunId is uninitialized.");
        }

        if (value.Length > MaxRunIdLength)
        {
            throw new E0AHarnessException("E0-A RunId exceeds the approved length bound.");
        }
    }

    internal static TakeId Take(RunId runId, int turn)
    {
        ValidateTurn(runId, turn);
        return TakeId.From($"{runId.Value}:TAKE:{turn:D3}");
    }

    internal static CommitId Commit(RunId runId, int turn)
    {
        ValidateTurn(runId, turn);
        return CommitId.From($"{runId.Value}:COMMIT:{turn:D3}");
    }

    internal static RecordId Record(RunId runId, int turn, int mutationIndex)
    {
        ValidateTurn(runId, turn);
        if (mutationIndex < 0)
        {
            throw new E0AHarnessException("E0-A mutation index must be nonnegative.");
        }

        return RecordId.From($"{runId.Value}:RECORD:{turn:D3}:{mutationIndex:D3}");
    }

    internal static string Attempt(RunId runId, E0ARole role, int turn, int attempt)
    {
        ValidateTurn(runId, turn);
        if (!Enum.IsDefined(role) || attempt != E0ARunEnvelope.AttemptsPerRoleInvocation)
        {
            throw new E0AHarnessException("E0-A attempt identity input is invalid.");
        }

        return $"{runId.Value}:ATTEMPT:{role.ToString().ToUpperInvariant()}:{turn:D3}:{attempt:D2}";
    }

    private static void ValidateTurn(RunId runId, int turn)
    {
        ValidateRunId(runId);
        if (turn is < 1 or > E0ARunEnvelope.AcceptedTurnCap)
        {
            throw new E0AHarnessException("E0-A turn number is outside the approved envelope.");
        }
    }
}
