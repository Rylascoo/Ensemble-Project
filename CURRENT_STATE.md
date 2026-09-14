# Ensemble Current State

Updated: 2026-09-13

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0C-01 repeated identical-condition runs**. E0-A/E0-B are **DONE**. Q-E0C-01 is **ACTIVE / DIRECTOR HOLD — BOTH FRESH SLOTS CONSUMED; SLOT 1 CONTRIBUTING; SLOT 2 NONCONTRIBUTING + EXECUTION-LAW DEVIATION; P01 SCORE SEALED; POST-SEAL MAPPING EXPOSURE RACE RECONCILED; PROVIDER TRAFFIC ZERO**.

Deviation authority: `docs/evidence/E0C_Q_E0C_01_SLOT2_EXECUTION_LAW_PROTOCOL_DEVIATION_DIRECTOR_INPUT_2026_09_13.md`. Reveal-race successor: `docs/evidence/E0C_Q_E0C_01_POSTSCORE_MAPPING_REVEAL_RACE_RECONCILIATION_2026_09_13.md`.

## Preserved results
Slot 1 `E0C-Q01-IDENT-20260912-01`: consumed, 12/12, sealed/hard-gate PASS, **CONTRIBUTING**. Slot 2 `E0C-Q01-IDENT-20260912-02`: consumed, `InvalidOutput`, 0/12, sealed/hard-gate PASS, **NONCONTRIBUTING**; no retry/replacement/third slot exists.

Slot 2 should have remained blocked because the required fresh authenticated post-Slot-1 execution-sensitive capacity observation was not obtained. Provider success does not cure that protocol defect. Recursive audit: `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_RECURSIVE_AUDIT_AND_PROCESS_HARDENING_2026_09_13.md`; Slot 1 is internally clean, first-turn request conditions match, and the capacity breach is independent of the `MARLOE` invalid output. Frozen law defines Slot 2 as the second fresh repeat; the post-execution concurrent-audit expectation was not durably encoded as a launch review gate and does not relabel the consumed run.

## P01 containment
PR #123 sealed the ten-dimension blind score while mapping was unknown; exact-main Validation #783 passed. Before any closure commit, a concurrent Engineering session followed the then-current reveal-next boundary, verified the external mapping SHA-256 `5d9a40d1ed5619f97015b06b47074a60c1cd66e0cbd265a937be730ac874cf6c`, and read the mapping. A subsequent race check discovered PR #125 had reached main and imposed the Director hold; exact-main Validation #786 passed.

The score/seal remain immutable and were not altered after exposure. No E0-C closure or E0-D advancement was integrated. Do not rescore, regenerate, replace, or use/publish the unblinded result for closure before Director disposition.

## Continuity
Q-E0D-01 remains blocked. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-C/D. Provider traffic is **ZERO**.

## Next
Director chooses whether to accept the already-sealed P01 score as unaffected evidence and authorize use of the already-verified mapping for descriptive closure without rescoring, or close E0-C without experiential-score use because of the execution-process breach. No E0-D advancement or provider traffic before that decision.