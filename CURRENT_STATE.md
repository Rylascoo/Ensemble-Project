# Ensemble Current State

Updated: 2026-09-16

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE / HOLD — P01 BOTH SLOTS CONSUMED/NONCONTRIBUTING — PAIR EXPERIENTIALLY INELIGIBLE — SLOT-2 TERMINAL-FINALIZATION DEFECT REPAIRED + NATIVE VALIDATED — P02/P03 AWAIT DIRECTOR DISPOSITION**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`; P01 timing amendment: `docs/evidence/E0D_P01_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_15.md`. P01 Slot 1 `E0D-Q01-P01-FULL-20260914-01` and Slot 2 `E0D-Q01-P01-REL-20260914-02` are permanently consumed with no retry/replay/replacement. P02/P03 remain unconsumed and not live-authorized.

## Native/provider authority
Successor native validation identity: checkout `8770a6361233e8a877a966c45eb6f62c5b3ca182`, tag `validation/e0d-context-ablation-terminal-fix-native-arm64`, executable SHA-256 `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`; evidence: `docs/evidence/E0D_CONTEXT_ABLATION_TERMINAL_FINALIZATION_REPAIR_NATIVE_ARM64_VALIDATION_2026_09_16.md`. Core 628/628, Harness 155/155, Release win-arm64 0 warnings/errors, fixture smokes and credentialless/no-root provider-edge gate PASS; branch Validation #885 PASS. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier. Validation creates no later-slot execution authority.

## P01 terminal boundary
Slot 1: sealed `InvalidOutput` 2/12 from noncanonical Turn-3 `MARLO`; hard gates PASS / 0 findings; authority `docs/evidence/E0D_Q_E0D_01_P01_SLOT1_TERMINAL_EVIDENCE_ANALYSIS_2026_09_15.md`. Slot 2: consumed 3/12; Turn-4 Performer HTTP 503 / `UNAVAILABLE`; baseline technical gate rejected the ablated context and threw before `Finish(...)`, leaving the immutable root unsealed. Authority: `docs/evidence/E0D_Q_E0D_01_P01_SLOT2_UNSEALED_TECHNICAL_TERMINAL_ANALYSIS_2026_09_15.md`. The successor repair changes future executable behavior only; it does not repair the consumed root.

## Next
**No further E0-D provider traffic yet.** Director must decide whether P02/P03 remain admissible under the validated successor executable. Any authorized later slot still requires its frozen identity plus fresh authenticated capacity/interlocks immediately before claim/provider traffic. Q-E0E-RUN remains blocked until E0-D closes.
