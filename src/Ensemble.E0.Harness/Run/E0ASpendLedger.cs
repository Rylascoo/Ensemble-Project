namespace Ensemble.E0.Harness.Run;

internal sealed class E0ASpendLedger
{
    private readonly E0APricingAssumptions _pricing;
    private decimal _estimatedCommittedUsd;
    private decimal _reservedUsd;

    internal E0ASpendLedger(E0APricingAssumptions pricing)
    {
        _pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
        _pricing.Validate();
    }

    internal decimal EstimatedCommittedUsd => _estimatedCommittedUsd;
    internal decimal ReservedUsd => _reservedUsd;

    internal E0ASpendReservation Reserve(long inputTokens, int maxOutputTokens)
    {
        if (inputTokens < 0 || maxOutputTokens <= 0 || _reservedUsd != 0m)
        {
            throw new E0AHarnessException("E0-A spend reservation input is invalid.");
        }

        var reservation = Cost(inputTokens, 0, maxOutputTokens);
        if (_estimatedCommittedUsd + reservation > E0ARunEnvelope.EstimatedSpendCeilingUsd)
        {
            throw new E0ABudgetExceededException();
        }

        _reservedUsd = reservation;
        return new E0ASpendReservation(inputTokens, maxOutputTokens, reservation);
    }

    internal decimal Reconcile(E0ASpendReservation reservation, E0AUsage usage)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        ArgumentNullException.ThrowIfNull(usage);
        usage.Validate();

        if (_reservedUsd == 0m || reservation.ReservedUsd != _reservedUsd)
        {
            throw new E0AHarnessException("E0-A spend reservation is not current.");
        }
        if (usage.InputTokens > reservation.InputTokens ||
            usage.OutputTokens > reservation.MaxOutputTokens)
        {
            throw new E0AHarnessException("E0-A reported usage exceeds its reserved token ceilings.");
        }

        var actual = Cost(
            usage.InputTokens - usage.CachedInputTokens,
            usage.CachedInputTokens,
            usage.OutputTokens);
        if (actual > reservation.ReservedUsd)
        {
            throw new E0AHarnessException("E0-A reported usage exceeds its conservative reservation.");
        }

        _estimatedCommittedUsd += actual;
        _reservedUsd = 0m;
        return actual;
    }

    internal void Release(E0ASpendReservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        if (_reservedUsd == 0m || reservation.ReservedUsd != _reservedUsd)
        {
            throw new E0AHarnessException("E0-A spend reservation is not current.");
        }

        _reservedUsd = 0m;
    }

    private decimal Cost(long uncachedInputTokens, long cachedInputTokens, long outputTokens) =>
        (uncachedInputTokens / 1_000_000m * _pricing.InputUsdPerMillionTokens) +
        (cachedInputTokens / 1_000_000m * _pricing.CachedInputUsdPerMillionTokens) +
        (outputTokens / 1_000_000m * _pricing.OutputUsdPerMillionTokens);
}

internal sealed record E0ASpendReservation(long InputTokens, int MaxOutputTokens, decimal ReservedUsd);

internal sealed class E0ABudgetExceededException : Exception
{
    internal E0ABudgetExceededException()
        : base("E0-A estimated model-token spend ceiling would be exceeded.") { }
}
