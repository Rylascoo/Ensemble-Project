# Ensemble Current State

Updated: 2026-09-13

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0C-01 repeated identical-condition runs**. E0-A/E0-B are **DONE**. Q-E0C-01 is **ACTIVE - SLOT 1 FRESH RATE-LIMIT/TIER/CAPACITY GATE PASSED; EXACT PROJECT/ACCOUNT ASSOCIATION + UTC WINDOW PENDING; PROVIDER TRAFFIC ZERO**.

Corrected method authority: `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md`; explicit Director ratification is Project Issue #107 comment `5650353003`. Reconciliation: `docs/evidence/E0C_Q_E0C_01_TIMING_LAW_RECONCILIATION_2026_09_12.md`.

## Canonical batch
Canonical preregistration: `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_PREREGISTRATION_2026_09_12.md`. Slot 1 `E0C-Q01-IDENT-20260912-01` and Slot 2 `E0C-Q01-IDENT-20260912-02` are the only fresh E0-C slots. Exact executable remains `bb869fb1c505603612bc718f739b3f1b358e5539`; no source change/retry/replacement/third slot. Canonical blind instrument: `docs/evidence/E0C_Q_E0C_01_BLIND_REPEATABILITY_INSTRUMENT_2026_09_12.json`, SHA-256 `7857699de82904f52892f3e6d0f9794c80e50ee488c9e30ef9fdb13ba4edfd64`.

Slot order is frozen as Slot 1 then Slot 2. Before Slot 1 claims its namespace, activation must freeze one explicit UTC batch window with fixed start/end timestamps covering both planned launches. After the first claim, order/window are immutable. A gated nonlaunch at window end is preserved as unexecuted/noncontributing; no extension/probe/retry/replacement/route/source change.

The later PR #110/#111 `REF` realization is raced/superseded for execution authority and preserved only as historical provenance. Its green validations do not override the earlier Director timing decision.

## Validation / evidence
Canonical Slot 1 preactivation: `docs/evidence/E0C_Q_E0C_01_RUN01_PREACTIVATION_2026_09_12.md`. Fresh rate-limit intake: `docs/evidence/E0C_Q_E0C_01_RUN01_FRESH_AUTHENTICATED_RATE_LIMIT_INTAKE_2026_09_13.md`. Static/protected-container gates pass; fresh Free-tier 3.5 is `9/15 RPM`, `22.07K/250K TPM`, `36/500 RPD`, so quota/capacity passes. Exact Kymaean project-ID + testing-account association remains pending; UTC window unfrozen. Provider traffic remains zero.

## Continuity
Q-E0D-01 remains blocked on E0-C closure. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-C/D. Blind comparison uses only sealed 12/12 hard-gate-PASS transcripts; no master score/statistical-significance claim.

## Next
Obtain one fresh AI Studio association view identifying the intended Kymaean project ID and existing testing credential. If it matches precedent, freeze the UTC Slot-1-then-Slot-2 window in the exact Slot 1 activation, integrate it, then require exact-main Validation before any namespace claim/provider traffic.
