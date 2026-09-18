# Ensemble Current State

Updated: 2026-09-17

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; queue: `docs/PROJECT_EXECUTION_QUEUE.md`; validation: `docs/VALIDATION_LEDGER.md`; Design locator: `docs/DESIGN_REPOSITORY.md`.

## Checkpoint
Director amendment `docs/KYMAEAN_PRODUCT_BUILD_AHEAD_OF_DEFERRED_E0_VALIDATION_DIRECTOR_AMENDMENT_2026_09_17.md` remains active: provisional product construction may proceed while E0-D/E/F/G remain deferred validation/finalization obligations.

Q-PROD-01 is ACTIVE. Event/replay remains durably integrated. Semantic-aware recovery is native-validated at exact source `97720ffc937853b59110c184392b1f6f97f39420`, tag `validation/q-prod-01-semantic-recovery-native-arm64`, based on validated Project `main@77d7ab7a36a27f5c4aa8d86aaf1df9878440f496`; integration is pending.

The model-neutral event store now exposes `Recover()`; typed append and recovery validate decode + deterministic replay under the journal lock before record mutation or head promotion. Invalid typed candidate history therefore cannot become durable through this boundary. Evidence: `docs/evidence/Q_PROD_01_SEMANTIC_RECOVERY_NATIVE_ARM64_VALIDATION_2026_09_17.md`. First-slice provenance remains `docs/Q_PROD_01_FRESH_CHAT_HANDOFF_2026_09_17.md`.

## Validation
At `97720ff...` on native Windows ARM64: Application **10/10 PASS**; persistence **20/20 PASS**; WinUI ARM64 Release **PASS, 0 warnings/errors**; repository law PASS; census 333/212/30/91/0; oracle 284/17/267.

Broader causal ontology, snapshots, export/migration, complete Production close/reopen reconstruction, final storage/concurrency policy, power-loss certification and complete P1 remain open.

## Deferred E0 / provider
P03 Slot 1 remains on Director deferred-test hold with namespace unconsumed/provider traffic zero; Slot 2 remains unauthorized. Frozen E0 provider association remains E0-only; product provider traffic requires a separate development association. Preserved E0-D chain: `docs/evidence/E0D_Q_E0D_01_P03_SLOT1_PREEXECUTION_ACTIVATION_2026_09_17.md`.

## Design coordination
Q-DESIGN-20 NC-01..NC-05 are durably integrated on Project `main@2fea5c084eb3b77bfc8f0dcc1241055627adc63c` via PR #174; exact-main Validation #947 PASS. Native authority remains `248435944815a34e4caac761ed308ed3b07e325b`, tag `validation/q-design-20-native-corrections-arm64`. Short Design acceptance is requested on Website issue #107; Source Sans 3/S1 placeholders and the High Contrast limitation remain.

## Next
Integrate the exact semantic-recovery candidate; then continue the smallest evidence-earned P1 capability while awaiting Q-DESIGN-20 acceptance. Leave deferred E0/provider traffic untouched.
