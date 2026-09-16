# Ensemble Current State

Updated: 2026-09-16

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE / HOLD — P01 CONSUMED/NONCONTRIBUTING — SUCCESSOR EXECUTABLE NATIVE VALIDATED + ADMITTED FOR REMAINING FROZEN P02/P03 PLAN — P02 SLOT 1 REQUIRES SEPARATE LIVE ACTIVATION**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. P01 Slot 1 `E0D-Q01-P01-FULL-20260914-01` and Slot 2 `E0D-Q01-P01-REL-20260914-02` are permanently consumed with no retry/replay/replacement; P01 is experientially ineligible. P02/P03 remain unconsumed. Director admissibility disposition: `docs/evidence/E0D_SUCCESSOR_EXECUTABLE_P02_P03_ADMISSIBILITY_DIRECTOR_DISPOSITION_2026_09_16.md`.

## Native/provider authority
Successor executable checkout `8770a6361233e8a877a966c45eb6f62c5b3ca182`, tag `validation/e0d-context-ablation-terminal-fix-native-arm64`, SHA-256 `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`; native evidence `docs/evidence/E0D_CONTEXT_ABLATION_TERMINAL_FINALIZATION_REPAIR_NATIVE_ARM64_VALIDATION_2026_09_16.md`. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier. Validation/admissibility does not itself authorize live slot execution.

## P01 terminal boundary
Slot 1: sealed `InvalidOutput` 2/12 from noncanonical Turn-3 `MARLO`; hard gates PASS / 0 findings. Slot 2: consumed 3/12; Turn-4 Performer HTTP 503 / `UNAVAILABLE`; baseline technical gate rejected the ablated context and threw before `Finish(...)`, leaving the immutable root unsealed. The successor repair changes future behavior only; neither P01 root may be modified or retried.

## Remaining frozen plan
P02 `E0D-P02-OMNISCIENT-CONTEXT` window is `2026-09-17T14:30:00Z..23:30:00Z`; next slot is ordinal 3 `P02-ABLATION` / `E0D-OMNISCIENT-CONTEXT-01` / RunId `E0D-Q01-P02-OMNI-20260914-03`. P03 remains frozen for `2026-09-18T14:30:00Z..23:30:00Z`. Each slot requires separate live authorization and a fresh immediate capacity/identity/interlock PASS.

## Next
**No further E0-D provider traffic yet.** Obtain and integrate separate Director live-activation authority for P02 Slot 1, then execute only inside its frozen window after fresh authenticated project/key/model/tier/RPM/TPM/RPD and all frozen interlocks pass. Q-E0E-RUN remains blocked until E0-D closes.
