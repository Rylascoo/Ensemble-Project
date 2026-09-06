namespace Ensemble.E0.Harness.Run;

internal sealed class E0ASpendLedger
{
    private readonly E0APricingAssumptions _pricing;
    private readonly long _maxInputTokens;
    private decimal _estimatedCommittedUsd;
    private decimal _reservedUsd;

    internal E0ASpendLedger(
        E0APricingAssumptions pricing,
        long maxInputTokens = E0APricingPolicy.StandardTierMaxInputTokens)
    {
        _pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
        _pricing.Validate();
        if (maxInputTokens <= 0)
        {
            throw new E0AHarnessException("E0-A maximum input-token limit is invalid.");
        }
        _maxInputTokens = maxInputTokens;
    }

    internal decimal EstimatedCommittedUsd => _estimatedCommittedUsd;
    internal decimal ReservedUsd => _reservedUsd;

    internal E0ASpendReservation Reserve(long inputTokens, int maxOutputTokens)
    {
        if (inputTokens < 0 || maxOutputTokens <= 0 || _reservedUsd != 0m)
        {
            throw new E0AHarnessException("E0-A spend reservation input is invalid.");
        }
        if (inputTokens > _maxInputTokens)
        {
            throw new E0ABudgetExceededException();
        }

        var reservation = ConservativeCost(inputTokens, maxOutputTokens);
        if (_estimatedCommittedUsd + reservation > E0ARunEnvelope.EstimatedSpendCeilingUsd)
        {
            throw new E0ABudgetExceededException();
        }

        _reservedUsd = reservation;
        return new E0ASpendReservation(inputTokens, maxOutputTokens, reservation);
    }

    internal E0ASpendReconciliation Reconcile(E0ASpendReservation reservation, E0AUsage usage)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        ArgumentNullException.ThrowIfNull(usage);
        usage.Validate();

        if (_reservedUsd == 0m || reservation.ReservedUsd != _reservedUsd)
        {
            throw new E0AHarnessException("E0-A spend reservation is not current.");
        }

        // Conservative reconciliation intentionally does not depend on provider
        // cache categories being a disjoint partition. All reported input is
        // charged at the envelope's full conservative input rate.
        var estimated = ConservativeCost(usage.InputTokens, usage.OutputTokens);
        var reservationExceeded =
            usage.InputTokens > reservation.InputTokens ||
            usage.OutputTokens > reservation.MaxOutputTokens ||
            usage.CacheWriteTokens != 0 ||
            estimated > reservation.ReservedUsd;

        _estimatedCommittedUsd += estimated;
        _reservedUsd = 0m;
        return new E0ASpendReconciliation(
            estimated,
            reservationExceeded,
            _estimatedCommittedUsd > E0ARunEnvelope.EstimatedSpendCeilingUsd);
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

    private decimal ConservativeCost(long inputTokens, long outputTokens) =>
        (inputTokens / 1_000_000m * _pricing.InputUsdPerMillionTokens) +
        (outputTokens / 1_000_000m * _pricing.OutputUsdPerMillionTokens);
}

internal sealed record E0ASpendReservation(long InputTokens, int MaxOutputTokens, decimal ReservedUsd);

internal sealed record E0ASpendReconciliation(
    decimal EstimatedUsd,
    bool ReservationExceeded,
    bool RunCeilingExceeded);

internal sealed class E0ABudgetExceededException : Exception
{
    internal E0ABudgetExceededException()
        : base("E0-A estimated model-token spend ceiling would be exceeded.") { }
}
