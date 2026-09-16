# Ensemble Current State

Updated: 2026-09-16

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — P01 CONSUMED/NONCONTRIBUTING — P02 SLOT 1 CONSUMED/NONCONTRIBUTING — P02 PAIR EXPERIENTIALLY INELIGIBLE — P02 SLOT 2 DIRECTOR-AUTHORIZED / ACTIVATION PACKAGED / NAMESPACE UNCONSUMED / PROVIDER TRAFFIC ZERO**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. P01 Slots 1/2 are permanently consumed/noncontributing. P02 Slot 1 `E0D-Q01-P02-OMNI-20260914-03` is permanently consumed at sealed `InvalidOutput` 2/12 with hard gates PASS. P02 Slot 2 is `P02-FULL` / `E0D-FULL-REFERENCE-01` / `E0D-Q01-P02-FULL-20260914-04`; P03 remains unconsumed. P02 timing remains `2026-09-16T19:33:00Z..2026-09-17T04:33:00Z`; P03 remains `2026-09-18T14:30:00Z..23:30:00Z`.

## Native/provider authority
Successor checkout `8770a6361233e8a877a966c45eb6f62c5b3ca182`, tag `validation/e0d-context-ablation-terminal-fix-native-arm64`, executable SHA-256 `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`; native evidence `docs/evidence/E0D_CONTEXT_ABLATION_TERMINAL_FINALIZATION_REPAIR_NATIVE_ARM64_VALIDATION_2026_09_16.md`; admissibility `docs/evidence/E0D_SUCCESSOR_EXECUTABLE_P02_P03_ADMISSIBILITY_DIRECTOR_DISPOSITION_2026_09_16.md`. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier.

## P02 predecessor / Slot 2 authority
Slot-1 terminal analysis: `docs/evidence/E0D_Q_E0D_01_P02_SLOT1_TERMINAL_EVIDENCE_ANALYSIS_2026_09_16.md`; no retry/replacement/scoring is permitted and the pair remains experientially ineligible. The Director separately authorized exact Slot 2 by replying `i approve`; durable authorization: `docs/evidence/E0D_P02_SLOT2_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_16.md`; activation: `docs/evidence/E0D_Q_E0D_01_P02_SLOT2_PREEXECUTION_ACTIVATION_2026_09_16.md`. Slot-2 root/claims remain absent and no Slot-2 provider traffic has occurred.

## Next
Integrate the Slot-2 authorization/activation through exact-head hosted gates and push-triggered exact-main Validation. Then, only while the amended P02 window remains open, obtain a fresh post-Slot-1 authenticated capacity observation and reverify every frozen identity/root/claim/process/credential interlock. If all pass, execute `E0D-Q01-P02-FULL-20260914-04` exactly once; no retry/replay/replacement/automatic extension. P03 remains separately unauthorized. Q-E0E-RUN remains blocked until E0-D closes.
