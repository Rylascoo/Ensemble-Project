using System.Collections.Immutable;
using System.Diagnostics;
using Ensemble.E0.Core.Cycle;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.PerformerAttempt;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Turn;
using Ensemble.E0.Harness.Evidence;

namespace Ensemble.E0.Harness.Run;

internal enum E0ARunTerminalStatus
{
    AcceptedTurnCapReached = 1,
    TechnicalFailure = 2,
    Cancelled = 3,
    BudgetExceeded = 4,
    InvalidOutput = 5,
    IntegrityConcern = 6,
    ModelIdentityChanged = 7
}

internal sealed record E0ARunResult(
    E0ARunTerminalStatus Status,
    int AcceptedTurns,
    decimal EstimatedSpendUsd,
    E0ASpendEstimateStatus SpendEstimateStatus,
    bool HasUnknownProviderUsage,
    E0OpportunityBearingCycleState State);

internal sealed class E0AReferenceRunDriver
{
    private readonly E0ARunEnvelope _envelope;
    private readonly IE0AProviderRolePort _provider;
    private readonly IE0AInputTokenCounter _tokenCounter;
    private readonly IE0AEvidenceSink _evidence;
    private readonly E0ASpendLedger _spend;
    private string? _observedModel;
    private bool _started;

    internal E0AReferenceRunDriver(
        E0ARunEnvelope envelope,
        IE0AProviderRolePort provider,
        IE0AInputTokenCounter tokenCounter,
        IE0AEvidenceSink evidence)
    {
        _envelope = envelope ?? throw new ArgumentNullException(nameof(envelope));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _tokenCounter = tokenCounter ?? throw new ArgumentNullException(nameof(tokenCounter));
        _evidence = evidence ?? throw new ArgumentNullException(nameof(evidence));
        _envelope.Validate();
        _spend = new E0ASpendLedger(_envelope.Pricing);
    }

    internal async Task<E0ARunResult> RunAsync(
        RunId runId,
        ProductionState genesis,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(genesis);
        E0ADeterministicIds.ValidateRunId(runId);
        if (_started)
        {
            throw new E0AHarnessException("E0-A reference run driver is single-use.");
        }
        _started = true;

        var state = DeterministicE0CausalCycle.Initialize(genesis);
        var policy = E0AReferenceAuthority.Policy(_envelope);
        var acceptedTurns = 0;
        _evidence.RecordEvent("run.started", new
        {
            stateHash = state.ProductionState.StateHash.Value,
            opportunityCharacterId = state.ProductionState.CurrentOpportunityCharacterId!.Value.Value
        });

        for (var turn = 1; turn <= E0ARunEnvelope.AcceptedTurnCap; turn++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Finish(E0ARunTerminalStatus.Cancelled, acceptedTurns, state);
            }

            var continuity = DeterministicE0CausalCycle.ComposeContext(state);
            var context = continuity.ContextEvaluation.Packet;
            _evidence.RecordEvent("context.composed", new
            {
                turn,
                stateHash = state.ProductionState.StateHash.Value,
                contextPacketId = context.ContextPacketId.Value,
                subjectCharacterId = context.SubjectCharacterId.Value,
                access = continuity.AccessEvaluation.Decisions.Select(x => new
                {
                    recordId = x.RecordId.Value,
                    disposition = x.Disposition.ToString(),
                    reason = x.Reason.ToString()
                }).ToArray()
            });

            var performerAttempt = E0ARequestBuilder.Performer(runId, turn, _envelope.Performer, context);
            var performerCall = await CallRoleAsync(performerAttempt, cancellationToken).ConfigureAwait(false);
            if (performerCall.TerminalStatus.HasValue)
            {
                var terminalReceipt = performerCall.Receipt;
                if (terminalReceipt is not null)
                {
                    var technical = terminalReceipt.Outcome == E0ARoleAttemptOutcome.Cancelled
                        ? E0PerformerAttemptDisposition.Cancelled
                        : E0PerformerAttemptDisposition.TechnicalFailure;
                    var technicalResult = DeterministicE0PerformerAttemptBoundary.BindTechnicalOutcome(context, technical);
                    _ = DeterministicE0TurnOrchestrator.GateAttempt(state, technicalResult);
                }
                return Finish(performerCall.TerminalStatus.Value, acceptedTurns, state);
            }
            if (cancellationToken.IsCancellationRequested)
            {
                return Finish(E0ARunTerminalStatus.Cancelled, acceptedTurns, state);
            }

            var performerReceipt = performerCall.Receipt
                ?? throw new E0AHarnessException("E0-A successful Performer call has no provider receipt.");
            CandidatePerformance candidate;
            try
            {
                candidate = PerformerCandidateContract.ParseJson(context, performerReceipt.StructuredOutput!);
            }
            catch (PerformerCandidateException)
            {
                return Finish(E0ARunTerminalStatus.InvalidOutput, acceptedTurns, state);
            }

            var integrityInput = IntegrityCandidateInput.Bind(context, candidate);
            _evidence.RecordEvent("performer.candidate", new
            {
                turn,
                candidateContentHash = integrityInput.CandidateContentHash,
                subjectCharacterId = candidate.SubjectCharacterId.Value,
                contextPacketId = candidate.ContextPacketId.Value,
                visibleText = candidate.VisibleText,
                addressedCharacterIds = candidate.Control.AddressedCharacterIds.Select(x => x.Value).ToArray(),
                nominatedCharacterId = candidate.Control.NominatedCharacterId.HasValue
                    ? candidate.Control.NominatedCharacterId.Value.Value
                    : null
            });

            var performerResult = DeterministicE0PerformerAttemptBoundary.BindCandidate(context, candidate);
            var progress = DeterministicE0TurnOrchestrator.GateAttempt(state, performerResult);

            if (integrityInput.DeterministicRejectCodes.Length != 0)
            {
                throw new E0AHarnessException("E0-A configured candidate unexpectedly failed deterministic Integrity binding.");
            }

            var packet = E0AIntegrityAssessmentPacketBuilder.Build(
                state.ProductionState,
                continuity.AccessEvaluation,
                context,
                candidate);
            var integrityAttempt = E0ARequestBuilder.Integrity(
                runId,
                turn,
                _envelope.Integrity,
                context,
                integrityInput.CandidateContentHash,
                packet);
            var integrityCall = await CallRoleAsync(integrityAttempt, cancellationToken).ConfigureAwait(false);
            if (integrityCall.TerminalStatus.HasValue)
            {
                return Finish(integrityCall.TerminalStatus.Value, acceptedTurns, state);
            }
            if (cancellationToken.IsCancellationRequested)
            {
                return Finish(E0ARunTerminalStatus.Cancelled, acceptedTurns, state);
            }

            ImmutableArray<IntegrityConcernKind> concerns;
            try
            {
                var integrityReceipt = integrityCall.Receipt
                    ?? throw new E0AHarnessException("E0-A successful Integrity call has no provider receipt.");
                concerns = E0AIntegrityConcernParser.Parse(integrityReceipt.StructuredOutput!);
            }
            catch (E0AHarnessException)
            {
                return Finish(E0ARunTerminalStatus.InvalidOutput, acceptedTurns, state);
            }

            try
            {
                progress = DeterministicE0TurnOrchestrator.EvaluateIntegrity(progress, concerns);
            }
            catch (E0TurnOrchestrationException)
            {
                return Finish(E0ARunTerminalStatus.InvalidOutput, acceptedTurns, state);
            }
            _evidence.RecordEvent("integrity.evaluated", new
            {
                turn,
                candidateContentHash = integrityInput.CandidateContentHash,
                concerns = concerns.Select(x => x.ToString()).ToArray(),
                disposition = progress.IntegrityEvaluation?.Disposition.ToString(),
                turnDisposition = progress.Disposition.ToString()
            });
            if (progress.Disposition == E0TurnProgressDisposition.RequestAnotherTake)
            {
                return Finish(E0ARunTerminalStatus.IntegrityConcern, acceptedTurns, state);
            }
            if (progress.Disposition != E0TurnProgressDisposition.ReadyForInterpretation ||
                progress.InterpretationSource is null)
            {
                throw new E0AHarnessException("E0-A Integrity progression is invalid.");
            }

            var interpreterAttempt = E0ARequestBuilder.Interpreter(
                runId,
                turn,
                _envelope.Interpreter,
                context,
                candidate,
                progress.InterpretationSource);
            var interpreterCall = await CallRoleAsync(interpreterAttempt, cancellationToken).ConfigureAwait(false);
            if (interpreterCall.TerminalStatus.HasValue)
            {
                return Finish(interpreterCall.TerminalStatus.Value, acceptedTurns, state);
            }
            if (cancellationToken.IsCancellationRequested)
            {
                return Finish(E0ARunTerminalStatus.Cancelled, acceptedTurns, state);
            }

            StateInterpretationProposal proposal;
            var interpreterReceipt = interpreterCall.Receipt
                ?? throw new E0AHarnessException("E0-A successful Interpreter call has no provider receipt.");
            try
            {
                proposal = StateInterpretationContract.ParseJson(
                    progress.InterpretationSource,
                    interpreterReceipt.StructuredOutput!);
            }
            catch (StateInterpretationException)
            {
                return Finish(E0ARunTerminalStatus.InvalidOutput, acceptedTurns, state);
            }
            _evidence.RecordEvent("interpreter.proposal", new
            {
                turn,
                candidateContentHash = progress.InterpretationSource.CandidateContentHash,
                structuredOutputHash = interpreterReceipt.StructuredOutputHash,
                mutationCount = proposal.Mutations.Length
            });

            progress = E0AReferenceAuthority.EvaluateAndRejectMandatoryReview(progress, proposal, policy);
            if (progress.Disposition != E0TurnProgressDisposition.TakeBindable ||
                progress.AuthorityEvaluation is null)
            {
                throw new E0AHarnessException("E0-A State Authority did not reach Take-bindable terminal state.");
            }
            _evidence.RecordEvent("authority.evaluated", new
            {
                turn,
                status = progress.AuthorityEvaluation.Status.ToString(),
                decisions = progress.AuthorityEvaluation.Decisions.Select(x => new
                {
                    mutationIndex = x.MutationIndex,
                    disposition = x.Disposition.ToString(),
                    reasons = x.Reasons.Select(reason => reason.ToString()).ToArray()
                }).ToArray()
            });

            if (cancellationToken.IsCancellationRequested)
            {
                return Finish(E0ARunTerminalStatus.Cancelled, acceptedTurns, state);
            }

            progress = DeterministicE0TurnOrchestrator.BindAcceptedTake(
                E0ADeterministicIds.Take(runId, turn),
                progress);
            var materials = E0AReferenceAuthority.Materializations(
                runId,
                turn,
                proposal,
                progress.AuthorityEvaluation!);
            var post = DeterministicE0TurnOrchestrator.CommitAccepted(
                E0ADeterministicIds.Commit(runId, turn),
                progress,
                materials);

            var next = DeterministicE0CausalCycle.EstablishOpportunity(post).State;
            acceptedTurns++;
            _evidence.RecordAcceptedPerformance(turn, candidate.SubjectCharacterId, candidate);
            _evidence.RecordEvent("turn.committed", new
            {
                turn,
                takeId = progress.AcceptedTake!.TakeId.Value,
                commitId = post.Commit.CommitId.Value,
                postCommitStateHash = post.ProductionState.StateHash.Value,
                nextStateHash = next.ProductionState.StateHash.Value,
                nextOpportunityCharacterId = next.ProductionState.CurrentOpportunityCharacterId!.Value.Value
            });
            state = next;
        }

        return Finish(E0ARunTerminalStatus.AcceptedTurnCapReached, acceptedTurns, state);
    }

    private async Task<RoleCall> CallRoleAsync(
        PreparedRoleAttempt attempt,
        CancellationToken cancellationToken)
    {
        _evidence.RecordPrepared(attempt);
        using var attemptTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        attemptTimeout.CancelAfter(TimeSpan.FromSeconds(E0ARunEnvelope.AttemptTimeoutSeconds));

        long inputTokens;
        var preflightClock = Stopwatch.StartNew();
        try
        {
            inputTokens = await _tokenCounter.CountInputTokensAsync(attempt, attemptTimeout.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            preflightClock.Stop();
            var external = cancellationToken.IsCancellationRequested;
            _evidence.RecordEvent(
                external ? "preflight.cancelled" : "preflight.timeout",
                new { attemptId = attempt.AttemptId, elapsedMs = preflightClock.Elapsed.TotalMilliseconds });
            return new RoleCall(
                null,
                external ? E0ARunTerminalStatus.Cancelled : E0ARunTerminalStatus.TechnicalFailure);
        }
        catch (E0AHarnessException)
        {
            preflightClock.Stop();
            _evidence.RecordEvent("preflight.failed", new
            {
                attemptId = attempt.AttemptId,
                code = "input-token-count-failed",
                elapsedMs = preflightClock.Elapsed.TotalMilliseconds
            });
            return new RoleCall(null, E0ARunTerminalStatus.TechnicalFailure);
        }
        preflightClock.Stop();

        if (inputTokens < 0)
        {
            _evidence.RecordEvent("preflight.failed", new
            {
                attemptId = attempt.AttemptId,
                code = "input-token-count-invalid",
                inputTokens,
                elapsedMs = preflightClock.Elapsed.TotalMilliseconds
            });
            return new RoleCall(null, E0ARunTerminalStatus.TechnicalFailure);
        }
        _evidence.RecordEvent("preflight.input-tokens", new
        {
            attemptId = attempt.AttemptId,
            inputTokens,
            elapsedMs = preflightClock.Elapsed.TotalMilliseconds
        });

        E0ASpendReservation reservation;
        try
        {
            reservation = _spend.Reserve(inputTokens, attempt.Profile.MaxOutputTokens);
        }
        catch (E0ABudgetExceededException)
        {
            _evidence.RecordEvent("budget.exceeded", new { attemptId = attempt.AttemptId, inputTokens });
            return new RoleCall(null, E0ARunTerminalStatus.BudgetExceeded);
        }
        _evidence.RecordEvent("spend.reserved", new
        {
            attemptId = attempt.AttemptId,
            inputTokens = reservation.InputTokens,
            maxOutputTokens = reservation.MaxOutputTokens,
            reservedUsd = reservation.ReservedUsd,
            committedUsdBeforeCall = _spend.EstimatedCommittedUsd
        });

        RoleAttemptReceipt raw;
        var providerClock = Stopwatch.StartNew();
        try
        {
            raw = await _provider.ExecuteAsync(attempt, _evidence, attemptTimeout.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            var external = cancellationToken.IsCancellationRequested;
            raw = external
                ? RoleAttemptReceipt.Cancelled(attempt, "cancelled")
                : RoleAttemptReceipt.TechnicalFailure(attempt, "timeout");
        }
        providerClock.Stop();
        _evidence.RecordEvent("provider.completed", new
        {
            attemptId = attempt.AttemptId,
            outcome = raw.Outcome.ToString(),
            elapsedMs = providerClock.Elapsed.TotalMilliseconds
        });

        RoleAttemptReceipt receipt;
        try
        {
            receipt = ConfiguredRoleAttemptBoundary.Accept(attempt, raw);
        }
        catch (E0AHarnessException)
        {
            var fallback = _spend.CommitUnknown(reservation);
            RecordSpendReconciliation(attempt, fallback, "configured-receipt-rejected");
            _evidence.RecordEvent("configured-receipt.rejected", new { attemptId = attempt.AttemptId });
            return new RoleCall(null, E0ARunTerminalStatus.TechnicalFailure);
        }

        _evidence.RecordReceipt(attempt, receipt);
        var reconciliation = receipt.Usage is null
            ? _spend.CommitUnknown(reservation)
            : _spend.Reconcile(reservation, receipt.Usage);
        RecordSpendReconciliation(attempt, reconciliation, receipt.DiagnosticCode);

        if (!reconciliation.PricingAssumptionsValid)
        {
            _evidence.RecordEvent("pricing.assumptions-invalid", new
            {
                attemptId = attempt.AttemptId,
                estimateStatus = reconciliation.EstimateStatus.ToString(),
                reportedInputTokens = receipt.Usage?.InputTokens,
                reportedOutputTokens = receipt.Usage?.OutputTokens,
                fallbackEstimatedUsd = reconciliation.EstimatedUsd
            });
            return new RoleCall(receipt, E0ARunTerminalStatus.TechnicalFailure);
        }

        if (reconciliation.ReservationExceeded || reconciliation.RunCeilingExceeded)
        {
            _evidence.RecordEvent("usage.reservation-mismatch", new
            {
                attemptId = attempt.AttemptId,
                reservedInputTokens = reservation.InputTokens,
                reportedInputTokens = receipt.Usage?.InputTokens,
                reservedMaxOutputTokens = reservation.MaxOutputTokens,
                reportedOutputTokens = receipt.Usage?.OutputTokens,
                reservedUsd = reservation.ReservedUsd,
                estimatedUsd = reconciliation.EstimatedUsd,
                usageKnown = reconciliation.UsageKnown,
                runCeilingExceeded = reconciliation.RunCeilingExceeded
            });
            return new RoleCall(receipt, E0ARunTerminalStatus.TechnicalFailure);
        }

        if (receipt.Outcome == E0ARoleAttemptOutcome.Success)
        {
            if (_observedModel is null)
            {
                _observedModel = receipt.ReturnedModel;
            }
            else if (!string.Equals(_observedModel, receipt.ReturnedModel, StringComparison.Ordinal))
            {
                return new RoleCall(receipt, E0ARunTerminalStatus.ModelIdentityChanged);
            }
        }

        var terminal = receipt.Outcome switch
        {
            E0ARoleAttemptOutcome.Success => (E0ARunTerminalStatus?)null,
            E0ARoleAttemptOutcome.Cancelled => E0ARunTerminalStatus.Cancelled,
            E0ARoleAttemptOutcome.TechnicalFailure => E0ARunTerminalStatus.TechnicalFailure,
            _ => throw new E0AHarnessException("E0-A provider outcome is invalid.")
        };
        return new RoleCall(receipt, terminal);
    }

    private void RecordSpendReconciliation(
        PreparedRoleAttempt attempt,
        E0ASpendReconciliation reconciliation,
        string? reason)
    {
        _evidence.RecordEvent("spend.reconciled", new
        {
            attemptId = attempt.AttemptId,
            estimatedUsd = reconciliation.EstimatedUsd,
            committedEstimatedUsd = _spend.EstimatedCommittedUsd,
            usageKnown = reconciliation.UsageKnown,
            estimateStatus = reconciliation.EstimateStatus.ToString(),
            pricingAssumptionsValid = reconciliation.PricingAssumptionsValid,
            reservationExceeded = reconciliation.ReservationExceeded,
            runCeilingExceeded = reconciliation.RunCeilingExceeded,
            reason
        });
    }

    private E0ARunResult Finish(
        E0ARunTerminalStatus status,
        int acceptedTurns,
        E0OpportunityBearingCycleState state)
    {
        var opportunity = state.ProductionState.CurrentOpportunityCharacterId
            ?? throw new E0AHarnessException("E0-A terminal state has no synchronized Opportunity.");
        _evidence.RecordEvent("run.terminal", new
        {
            status = status.ToString(),
            acceptedTurns,
            estimatedSpendUsd = _spend.EstimatedCommittedUsd,
            spendEstimateStatus = _spend.EstimateStatus.ToString(),
            hasUnknownProviderUsage = _spend.HasUnknownProviderUsage,
            finalStateHash = state.ProductionState.StateHash.Value,
            finalOpportunityCharacterId = opportunity.Value
        });
        _evidence.SealRuntime(
            status.ToString(),
            acceptedTurns,
            _spend.EstimatedCommittedUsd,
            _spend.EstimateStatus,
            _spend.HasUnknownProviderUsage,
            state.ProductionState.StateHash.Value,
            opportunity);
        return new E0ARunResult(
            status,
            acceptedTurns,
            _spend.EstimatedCommittedUsd,
            _spend.EstimateStatus,
            _spend.HasUnknownProviderUsage,
            state);
    }

    private sealed record RoleCall(RoleAttemptReceipt? Receipt, E0ARunTerminalStatus? TerminalStatus);
}
