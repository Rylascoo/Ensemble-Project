# Ensemble Current State

Updated: 2026-09-17

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; queue: `docs/PROJECT_EXECUTION_QUEUE.md`; validation: `docs/VALIDATION_LEDGER.md`; Design locator: `docs/DESIGN_REPOSITORY.md`.

## Checkpoint
Director amendment `docs/KYMAEAN_PRODUCT_BUILD_AHEAD_OF_DEFERRED_E0_VALIDATION_DIRECTOR_AMENDMENT_2026_09_17.md` remains active: provisional product construction may proceed while E0-D/E/F/G remain deferred validation/finalization obligations.

Q-PROD-01 is ACTIVE. Persistence-journal integration/continuity is durable on Project `main@832bc347c8bc9decf903094038ca890f89075c95` after PR #170 and exact-main Validation #935 PASS.

Successor Product-event/replay candidate is native-validated at exact source `789028a52b133dfc29c1bdb79ac5533b40713f78`, tag `validation/q-prod-01-production-event-replay-native-arm64`, with integration pending. Application now owns a model-neutral Product event-store port, `ProductionCreatedEvent`, and deterministic `ProductionReplayProjection`; Infrastructure encodes `kymaean.production.created.v1` over the append-only journal. Application retains zero project references.

Evidence: `docs/evidence/Q_PROD_01_PRODUCTION_EVENT_REPLAY_NATIVE_ARM64_VALIDATION_2026_09_17.md`. First-slice provenance remains `docs/Q_PROD_01_FRESH_CHAT_HANDOFF_2026_09_17.md`.

## Validation
At `789028a...` on native Windows ARM64: Application **10/10 PASS**; persistence **14/14 PASS**; WinUI ARM64 Release build **PASS, 0 warnings/errors**; repository law PASS; census 331/210/30/91/0; oracle 279/17/262.

Recursive audit rejected a typed `Recover()` wrapper because raw journal recovery can promote a suffix before Product-event decoding. Typed semantic-aware recovery therefore remains unearned. Broader causal ontology, snapshots, export/migration, complete close/reopen reconstruction, power-loss certification and complete P1 remain open.

## Deferred E0 / provider
P03 Slot 1 remains on Director deferred-test hold with namespace unconsumed/provider traffic zero; Slot 2 remains unauthorized. Frozen E0 provider association remains E0-only; product provider traffic requires a separate development association. Preserved E0-D chain: `docs/evidence/E0D_Q_E0D_01_P03_SLOT1_PREEXECUTION_ACTIVATION_2026_09_17.md`.

## Design coordination
Q-DESIGN-20 was dispatched to Design Sol; no returned Design authority has been consumed. Engineering does not mutate Design-owned source.

## Next
Integrate the exact Product-event/replay candidate through hosted exact-head and exact-main validation. Then continue the smallest semantic-aware recovery / replay capability behind replaceable seams while leaving deferred E0 untouched.
