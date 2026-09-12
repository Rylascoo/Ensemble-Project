namespace Ensemble.E0.Harness.Run;

internal enum E0ASpendEstimateStatus
{
    WithinVerifiedPricingAssumptions = 1,
    OutsideVerifiedInputTier = 2,
    UnrepresentableReportedUsage = 3
}

internal sealed class E0ASpendLedger
{
    private readonly E0APricingAssumptions _pricing;
    private readonly long _maxInputTokens;
    private decimal _estimatedCommittedUsd;
    private decimal _reservedUsd;
    private bool _reservationActive;
    private long _nextReservationId;
    private long _activeReservationId;
    private bool _hasUnknownProviderUsage;
    private E0ASpendEstimateStatus _estimateStatus = E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions;

    internal E0ASpendLedger(
        E0APricingAssumptions pricing,
        long maxInputTokens = E0AGeminiProviderPolicy.ModelInputTokenLimit)
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
    internal decimal ReservedUsd => _reservationActive ? _reservedUsd : 0m;
    internal bool HasActiveReservation => _reservationActive;
    internal bool HasUnknownProviderUsage => _hasUnknownProviderUsage;
    internal E0ASpendEstimateStatus EstimateStatus => _estimateStatus;

    internal E0ASpendReservation Reserve(long inputTokens, int maxOutputTokens) =>
        Reserve(inputTokens, maxOutputTokens, _pricing, _maxInputTokens);

    internal E0ASpendReservation Reserve(
        long inputTokens,
        int maxOutputTokens,
        E0APricingAssumptions pricing,
        long maxInputTokens)
    {
        ArgumentNullException.ThrowIfNull(pricing);
        pricing.Validate();
        if (inputTokens < 0 || maxOutputTokens <= 0 || maxInputTokens <= 0 || _reservationActive)
        {
            throw new E0AHarnessException("E0-A spend reservation input is invalid.");
        }
        if (inputTokens > maxInputTokens)
        {
            throw new E0ABudgetExceededException();
        }

        decimal reservation;
        try
        {
            reservation = ConservativeCost(inputTokens, maxOutputTokens, pricing);
            if (checked(_estimatedCommittedUsd + reservation) > E0ARunEnvelope.EstimatedSpendCeilingUsd)
            {
                throw new E0ABudgetExceededException();
            }
        }
        catch (OverflowException)
        {
            throw new E0ABudgetExceededException();
        }

        if (reservation <= 0m)
        {
            throw new E0AHarnessException("E0-A spend reservation is not representable as a positive amount.");
        }

        try
        {
            _nextReservationId = checked(_nextReservationId + 1);
        }
        catch (OverflowException)
        {
            throw new E0AHarnessException("E0-A spend reservation identity space is exhausted.");
        }

        _reservedUsd = reservation;
        _activeReservationId = _nextReservationId;
        _reservationActive = true;
        return new E0ASpendReservation(
            _activeReservationId,
            inputTokens,
            maxOutputTokens,
            reservation,
            pricing,
            maxInputTokens);
    }

    internal E0ASpendReconciliation Reconcile(E0ASpendReservation reservation, E0AUsage usage)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        ArgumentNullException.ThrowIfNull(usage);
        usage.Validate();
        ValidateCurrent(reservation);

        if (usage.InputTokens > reservation.MaxInputTokens)
        {
            return CommitFallback(
                reservation,
                usageKnown: true,
                status: E0ASpendEstimateStatus.OutsideVerifiedInputTier,
                reservationExceeded: true);
        }

        decimal estimated;
        try
        {
            estimated = ConservativeCost(usage.InputTokens, usage.OutputTokens, reservation.Pricing);
        }
        catch (OverflowException)
        {
            return CommitFallback(
                reservation,
                usageKnown: true,
                status: E0ASpendEstimateStatus.UnrepresentableReportedUsage,
                reservationExceeded: true);
        }

        var reservationExceeded =
            usage.InputTokens > reservation.InputTokens ||
            usage.OutputTokens > reservation.MaxOutputTokens ||
            usage.CacheWriteTokens != 0 ||
            estimated > reservation.ReservedUsd;

        decimal nextCommitted;
        try
        {
            nextCommitted = checked(_estimatedCommittedUsd + estimated);
        }
        catch (OverflowException)
        {
            return CommitFallback(
                reservation,
                usageKnown: true,
                status: E0ASpendEstimateStatus.UnrepresentableReportedUsage,
                reservationExceeded: true);
        }

        _estimatedCommittedUsd = nextCommitted;
        ClearReservation();
        return new E0ASpendReconciliation(
            EstimatedUsd: estimated,
            UsageKnown: true,
            EstimateStatus: E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions,
            ReservationExceeded: reservationExceeded,
            RunCeilingExceeded: _estimatedCommittedUsd > E0ARunEnvelope.EstimatedSpendCeilingUsd);
    }

    internal E0ASpendReconciliation CommitUnknown(E0ASpendReservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        ValidateCurrent(reservation);
        _hasUnknownProviderUsage = true;

        decimal nextCommitted;
        try
        {
            nextCommitted = checked(_estimatedCommittedUsd + reservation.ReservedUsd);
        }
        catch (OverflowException)
        {
            throw new E0AHarnessException("E0-A fallback spend estimate is not representable.");
        }

        _estimatedCommittedUsd = nextCommitted;
        ClearReservation();
        return new E0ASpendReconciliation(
            EstimatedUsd: reservation.ReservedUsd,
            UsageKnown: false,
            EstimateStatus: _estimateStatus,
            ReservationExceeded: false,
            RunCeilingExceeded: _estimatedCommittedUsd > E0ARunEnvelope.EstimatedSpendCeilingUsd);
    }

    internal void Release(E0ASpendReservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        ValidateCurrent(reservation);
        ClearReservation();
    }

    private E0ASpendReconciliation CommitFallback(
        E0ASpendReservation reservation,
        bool usageKnown,
        E0ASpendEstimateStatus status,
        bool reservationExceeded)
    {
        PromoteStatus(status);

        decimal nextCommitted;
        try
        {
            nextCommitted = checked(_estimatedCommittedUsd + reservation.ReservedUsd);
        }
        catch (OverflowException)
        {
            throw new E0AHarnessException("E0-A fallback spend estimate is not representable.");
        }

        _estimatedCommittedUsd = nextCommitted;
        ClearReservation();
        return new E0ASpendReconciliation(
            EstimatedUsd: reservation.ReservedUsd,
            UsageKnown: usageKnown,
            EstimateStatus: status,
            ReservationExceeded: reservationExceeded,
            RunCeilingExceeded: _estimatedCommittedUsd > E0ARunEnvelope.EstimatedSpendCeilingUsd);
    }

    private void ValidateCurrent(E0ASpendReservation reservation)
    {
        if (!_reservationActive ||
            reservation.ReservationId != _activeReservationId ||
            reservation.ReservedUsd != _reservedUsd)
        {
            throw new E0AHarnessException("E0-A spend reservation is not current.");
        }
    }

    private void ClearReservation()
    {
        _reservedUsd = 0m;
        _activeReservationId = 0;
        _reservationActive = false;
    }

    private void PromoteStatus(E0ASpendEstimateStatus status)
    {
        if (_estimateStatus == E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions)
        {
            _estimateStatus = status;
        }
    }

    private static decimal ConservativeCost(
        long inputTokens,
        long outputTokens,
        E0APricingAssumptions pricing) =>
        checked(
            (inputTokens / 1_000_000m * pricing.InputUsdPerMillionTokens) +
            (outputTokens / 1_000_000m * pricing.OutputUsdPerMillionTokens));
}

internal sealed record E0ASpendReservation(
    long ReservationId,
    long InputTokens,
    int MaxOutputTokens,
    decimal ReservedUsd,
    E0APricingAssumptions Pricing,
    long MaxInputTokens);

internal sealed record E0ASpendReconciliation(
    decimal EstimatedUsd,
    bool UsageKnown,
    E0ASpendEstimateStatus EstimateStatus,
    bool ReservationExceeded,
    bool RunCeilingExceeded)
{
    internal bool PricingAssumptionsValid =>
        EstimateStatus == E0ASpendEstimateStatus.WithinVerifiedPricingAssumptions;
}

internal sealed class E0ABudgetExceededException : Exception
{
    internal E0ABudgetExceededException()
        : base("E0-A estimated model-token spend ceiling would be exceeded.") { }
}
