# Q-PROD-01 Production Event / Replay Native ARM64 Validation

Date: 2026-09-17

Status: **PROVISIONAL PRODUCT PERSISTENCE SLICE NATIVE-VALIDATED / INTEGRATION PENDING / NOT COMPLETE P1 OR FINAL ARCHITECTURE**

## Identity

- baseline Project main: `832bc347c8bc9decf903094038ca890f89075c95` after PR #170 and exact-main Validation #935 PASS;
- exact native machine-tested source: `789028a52b133dfc29c1bdb79ac5533b40713f78`;
- validation tag: `validation/q-prod-01-production-event-replay-native-arm64`;
- host: SurfSeven, native Windows ARM64, repository .NET 9 baseline.

## Earned implementation boundary

Application now owns a minimal model-neutral Production persistence port and deterministic replay contract without referencing `Ensemble.E0.Core`.

The only persisted Product event currently authorized is `ProductionCreatedEvent`, carrying the already-established Product concept of a Production name. Its infrastructure encoding is the versioned deterministic contract `kymaean.production.created.v1`.

`ProductionReplay.Rebuild(...)` derives a `ProductionReplayProjection` from ordered Product events. The projection is derived state and is not promoted to causal source-of-truth authority.

`Kymaean.Infrastructure.Persistence` implements the Application port over the previously validated append-only `FileProductionJournal`; dependency direction is Infrastructure -> Application. Unknown contracts, malformed JSON, unexpected v1 properties, empty history and duplicate creation fail closed.

## Native validation

At exact source `789028a...`:

- `Kymaean.Application.Tests`: **10/10 PASS** on `win-arm64` Release;
- `Kymaean.Infrastructure.Persistence.Tests`: **14/14 PASS** on `win-arm64` Release, including the nine raw-journal tests and five typed-event/store tests;
- `Kymaean.Windows` ARM64 Release build: **PASS**, 0 warnings / 0 errors;
- repository law: **PASS**, 10 classified projects and dependency direction PASS;
- document census: **331 / 210 / 30 / 91 / 0 unexplained current**;
- oracle assertion coverage: **279 documented / 17 asserted / 262 document-only**;
- `git diff --check`: PASS.

Application retains zero project references. Persistence references Application only.

## Recovery boundary discovered by recursive audit

An initial typed `Recover()` wrapper was rejected before commit. Raw journal recovery can promote a structurally valid suffix to the committed head before Product-event decoding occurs; a malformed or unsupported Product event could therefore be promoted before semantic validation rejected it.

The candidate deliberately exposes no typed semantic-recovery API. Product-event-aware recovery remains unearned until semantic validation participates before committed-head promotion. The existing raw journal recovery authority is not inflated into Product semantic recovery.

## Non-authority

This checkpoint does not freeze the broader causal event ontology, creator-facing Studio ontology, Production identity scheme, Cast/Scene lifecycle, E0 categories, snapshots, portable export/migration, storage location/concurrency policy, power-loss certification, complete close/reopen reconstruction, complete P1, final architecture, Alpha/Beta/release, WACK, Store, or deferred-E0 conclusions.

No Gemini/provider traffic occurred and no deferred-E0 namespace was consumed.

## Next

Integrate this exact source through hosted exact-head and exact-main validation. Continue P1 with semantic-aware recovery and the next smallest evidence-earned Product event/rebuild capability without leaking experimental E0 ontology into Application or treating projections as source of truth.
