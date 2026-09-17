# Ensemble Current State

Updated: 2026-09-17

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; queue: `docs/PROJECT_EXECUTION_QUEUE.md`; validation: `docs/VALIDATION_LEDGER.md`; Design locator: `docs/DESIGN_REPOSITORY.md`.

## Checkpoint
Director amendment `docs/KYMAEAN_PRODUCT_BUILD_AHEAD_OF_DEFERRED_E0_VALIDATION_DIRECTOR_AMENDMENT_2026_09_17.md` remains active: product construction proceeds while E0-D/E/F/G remain deferred validation/finalization obligations.

Q-PROD-01 is ACTIVE. PR #169 durably integrated the provisional persistence journal at Project `main@2c165539fcfb4541d90e177ac09a9800fc0169e8`; exact-head Validation #931 and E0-E preparation #113 passed, then exact-main push Validation #932 passed.

Native machine-test authority remains exact source `fff8298a066531cdfbd2bcb7bdacdbc7d024bcca`, tag `validation/q-prod-01-persistence-journal-native-arm64`: append-only framing, hash chaining, committed-head witnessing, tail-loss detection, and validated-suffix recovery in `Kymaean.Infrastructure.Persistence`. Later integration/docs commits do not inherit native authority.

Evidence: `docs/evidence/Q_PROD_01_PERSISTENCE_JOURNAL_NATIVE_ARM64_VALIDATION_2026_09_17.md`. First-slice provenance remains `docs/Q_PROD_01_FRESH_CHAT_HANDOFF_2026_09_17.md` and `docs/evidence/Q_PROD_01_FIRST_NATIVE_VERTICAL_SLICE_ARM64_VALIDATION_2026_09_17.md`.

## Validation
At `fff8298...` on native Windows ARM64: persistence tests 9/9 PASS; Application tests 6/6 PASS; WinUI ARM64 Release build PASS, 0 warnings/errors. The integrated candidate also passed hosted persistence/Application/Core/compiler/repository/document/oracle gates on #931 and exact-main #932.

This remains provisional P1 infrastructure: event serialization, deterministic replay/rebuild, snapshots, export/migration, storage/concurrency policy, crash/power-loss certification and complete P1 remain unearned.

Exact-final High Contrast for the earlier native UI package remains a Design-review limitation; do not bypass the remote safety boundary.

## Deferred E0 / provider
P03 Slot 1 remains on Director deferred-test hold with namespace unconsumed/provider traffic zero; Slot 2 remains unauthorized. Frozen `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` remains E0-only; product provider traffic requires a separate development association. Preserved E0-D chain: `docs/evidence/E0D_Q_E0D_01_P03_SLOT1_PREEXECUTION_ACTIVATION_2026_09_17.md`.

## Design coordination
Q-DESIGN-20 Design return is canonical at Website APPUI `bd20f25ae48f75ff3ceecfb355104c42babe373c` / L-262: architecture accepted; Engineering owes NC-01..NC-05 + corrected native evidence, then short Design acceptance. Final High Contrast limitation remains.

## Next
Continue P1 with the smallest model-neutral Product/Application persistence-event and deterministic replay/rebuild boundary. Keep provider work credentialless/offline, preserve replaceable seams, and leave deferred E0 untouched.