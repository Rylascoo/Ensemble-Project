# Ensemble Current State

Updated: 2026-09-12

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0C-01 repeated identical-condition runs**. E0-A/E0-B are **DONE**. Q-E0C-01 is **ACTIVE - CORRECTED METHOD APPROVED; CANONICAL IDENT BATCH PREREGISTERED; SLOT 1 PREACTIVATION ONLY; PROVIDER TRAFFIC ZERO**.

Corrected method authority: `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md`; explicit Director ratification is Project Issue #107 comment `5650353003`. Reconciliation: `docs/evidence/E0C_Q_E0C_01_TIMING_LAW_RECONCILIATION_2026_09_12.md`.

## Canonical batch
Canonical preregistration: `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_PREREGISTRATION_2026_09_12.md`. Slot 1 `E0C-Q01-IDENT-20260912-01` and Slot 2 `E0C-Q01-IDENT-20260912-02` are the only fresh E0-C slots. Exact executable remains `bb869fb1c505603612bc718f739b3f1b358e5539`; no source change/retry/replacement/third slot. Canonical blind instrument: `docs/evidence/E0C_Q_E0C_01_BLIND_REPEATABILITY_INSTRUMENT_2026_09_12.json`, SHA-256 `7857699de82904f52892f3e6d0f9794c80e50ee488c9e30ef9fdb13ba4edfd64`.

Slot order is frozen as Slot 1 then Slot 2. Before Slot 1 claims its namespace, activation must freeze one explicit UTC batch window with fixed start/end timestamps covering both planned launches. After the first claim, order/window are immutable. A gated nonlaunch at window end is preserved as unexecuted/noncontributing; no extension/probe/retry/replacement/route/source change.

The later PR #110/#111 `REF` realization is raced/superseded for execution authority and preserved only as historical provenance. Its green validations do not override the earlier Director timing decision.

## Validation / evidence
Canonical Slot 1 preactivation: `docs/evidence/E0C_Q_E0C_01_RUN01_PREACTIVATION_2026_09_12.md`. Public/native/namespace preparation passes; fresh authenticated intended project/key/Free-tier/current 3.5 RPM/TPM/RPD capacity remains required. The exact UTC batch window is not yet frozen. No credential/provider call is authorized by reconciliation.

## Continuity
Q-E0D-01 remains blocked on E0-C closure. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-C/D. Blind comparison uses only sealed 12/12 hard-gate-PASS transcripts; no master score/statistical-significance claim.

## Next
Construct and integrate one exact canonical Slot 1 activation: reverify unconsumed IDENT roots/claims, fresh authenticated project/key/tier/quota/capacity, protected credential readiness, and freeze the explicit UTC Slot-1-then-Slot-2 batch window. Require exact-main post-merge Validation before any namespace claim or provider traffic.
