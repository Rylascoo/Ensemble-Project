# Ensemble Current State

Updated: 2026-09-13

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0C-01 repeated identical-condition runs**. E0-A/E0-B are **DONE**. Q-E0C-01 is **ACTIVE - BOTH FRESH SLOTS CONSUMED; SLOT 1 CONTRIBUTING; SLOT 2 NONCONTRIBUTING + EXECUTION-LAW DEVIATION; DIRECTOR DISPOSITION HOLD; PROVIDER TRAFFIC ZERO**.

Corrected method: `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md`; preregistration: `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_PREREGISTRATION_2026_09_12.md`; reconciliation/Director input: `docs/evidence/E0C_Q_E0C_01_RUN02_TERMINAL_PROTOCOL_RECONCILIATION_AND_DIRECTOR_INPUT_2026_09_13.md`.

## Batch result
Slot 1 `E0C-Q01-IDENT-20260912-01`: one consumed launch, `AcceptedTurnCapReached`, 12/12, exact model, sealed/hard-gate PASS, contributing. Runtime root `311c7196...`; terminal evidence `docs/evidence/E0C_Q_E0C_01_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`.

Slot 2 `E0C-Q01-IDENT-20260912-02`: one consumed launch at `23:15:47Z`, exit 3 at `23:15:53Z`, `InvalidOutput`, 0/12. First Performer generation succeeded on exact `gemini-3.5-flash-lite`; typed control used out-of-roster `MARLOE` instead of `MARLOWE`. Two API operations, 724 input / 144 output, shadow USD 0.00057720. Runtime root `6c590aa1...`; noncontributing; no replay/replacement.

## Protocol deviation
Post-launch audit confirmed Slot 2 reused the authenticated pre-Slot-1 quota snapshot plus arithmetic instead of obtaining a fresh authenticated post-Slot-1 model/tier/quota/capacity observation. The standing association amendment explicitly kept that gate execution-sensitive. Slot 2 therefore should have remained blocked; the successful provider response does not cure the procedural defect. Canonical preregistration claim filenames were correct; the earlier contrary Slot 1 terminal sentence has been repaired.

## Continuity
Frozen blind law leaves only P01 (Run 08 / Slot 1) otherwise eligible; P02/P03 are removed because Slot 2 is noncontributing. No mapping/scoring/unblinding may begin until Director disposition of the protocol deviation. Q-E0D-01 and Q-E0E-RUN remain blocked.

## Next
Director decides whether to preserve the batch and permit P01-only blind repeatability comparison, or close E0-C without experiential scoring because of the execution-process breach. No further E0-C provider traffic, retry, replacement, third slot, window extension, model/route/key/tier substitution, scoring, or E0-D advancement before that decision.
