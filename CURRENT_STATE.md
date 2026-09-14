# Ensemble Current State

Updated: 2026-09-13

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0C-01 repeated identical-condition runs**. E0-A/E0-B are **DONE**. Q-E0C-01 is **ACTIVE - BOTH FRESH SLOTS CONSUMED/SEALED; P01 BLIND SCORE SEALED; MAPPING REVEAL PENDING; PROVIDER TRAFFIC ZERO**.

Corrected method: `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md`; canonical preregistration: `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_PREREGISTRATION_2026_09_12.md`; frozen blind instrument: `docs/evidence/E0C_Q_E0C_01_BLIND_REPEATABILITY_INSTRUMENT_2026_09_12.json`.

## Canonical batch
Exact executable `bb869fb1c505603612bc718f739b3f1b358e5539`; fixed order Slot 1 then Slot 2; immutable UTC window `2026-09-13T20:30:00Z` through `2026-09-13T23:30:00Z`. Both preregistered namespaces are consumed. No retry/replacement/third slot/window extension or further E0-C provider launch exists.

## Fresh results
Slot 1 `E0C-Q01-IDENT-20260912-01`: `AcceptedTurnCapReached`, 12/12, runtime seal valid, hard-gate PASS, **CONTRIBUTING**. Evidence: `docs/evidence/E0C_Q_E0C_01_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`.

Slot 2 `E0C-Q01-IDENT-20260912-02`: `InvalidOutput`, 0/12, one successful exact-model Performer generation after one token preflight, runtime seal valid, hard-gate PASS, **NONCONTRIBUTING**. The response emitted invalid canonical control ID `MARLOE` despite exact roster IDs/instruction being supplied, so no source/interface correction is earned. Evidence: `docs/evidence/E0C_Q_E0C_01_RUN02_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`.

## Comparison boundary
Frozen eligibility leaves only `E0C-P01` (Run 08 vs Slot 1). P02/P03 are removed solely because Slot 2 is noncontributing. Sanitized package `docs/evidence/E0C_Q_E0C_01_P01_BLIND_PACKAGE_2026_09_13.json` and mapping commitment `docs/evidence/E0C_Q_E0C_01_P01_MAPPING_COMMITMENT_2026_09_13.json` were integrated at exact main `5c0284cdf5f9a0ec7dfbef529f3196df1434e0ae`; push-triggered Validation #778 passed before scorer access. Blind score `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_2026_09_13.json` is sealed by `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_SEAL_2026_09_13.json`; mapping remained unknown during scoring and is still withheld. No weighted master score, statistical-significance claim, or reference replacement is authorized.

## Continuity
Q-E0D-01 remains blocked on E0-C closure. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-C/D. Provider traffic is **ZERO**.

## Next
Integrate and exact-main validate this blind score seal. Only after that durable seal passes may the withheld mapping be revealed and verified; then report the unblinded descriptive P01 matrix and close Q-E0C-01 without a weighted master score or statistical-significance claim.
