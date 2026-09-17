# Ensemble Current State

Updated: 2026-09-17

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — P01 FULLY CONSUMED/NONCONTRIBUTING — P02 FULLY CONSUMED/NONCONTRIBUTING / EXPERIENTIALLY INELIGIBLE — P03 SLOT 1 DIRECTOR-AUTHORIZED / ACTIVATION PACKAGED / PREWINDOW HOLD / NAMESPACE UNCONSUMED / PROVIDER TRAFFIC ZERO — P03 SLOT 2 UNAUTHORIZED**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. P01/P02 are permanently consumed/noncontributing; P02 is experientially ineligible. P03 Slot 1 is `P03-FULL` / `E0D-FULL-REFERENCE-01` / `E0D-Q01-P03-FULL-20260914-05`; Slot 2 remains frozen/unconsumed. P03 window remains `2026-09-18T14:30:00Z..23:30:00Z`.

## Native/provider authority
Promoted checkout `8770a6361233e8a877a966c45eb6f62c5b3ca182`, tag `validation/e0d-context-ablation-terminal-fix-native-arm64`, executable SHA-256 `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`; native evidence `docs/evidence/E0D_CONTEXT_ABLATION_TERMINAL_FINALIZATION_REPAIR_NATIVE_ARM64_VALIDATION_2026_09_16.md`; admissibility `docs/evidence/E0D_SUCCESSOR_EXECUTABLE_P02_P03_ADMISSIBILITY_DIRECTOR_DISPOSITION_2026_09_16.md`. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier.

## P02 closure / P03 Slot 1 authority
P02 terminal: `docs/evidence/E0D_Q_E0D_01_P02_SLOT2_TERMINAL_EVIDENCE_ANALYSIS_2026_09_17.md`. P03 Slot-1 Director authorization: `docs/evidence/E0D_P03_SLOT1_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_17.md`; activation: `docs/evidence/E0D_Q_E0D_01_P03_SLOT1_PREEXECUTION_ACTIVATION_2026_09_17.md`. No P03 claim, evidence root or provider traffic exists.

## Next
Integrate the P03 Slot-1 authorization/activation through exact-head hosted Validation + E0-E preparation and push-triggered exact-main Validation. Then hold until the frozen window opens; only inside it obtain fresh authenticated capacity and reverify every frozen interlock before exactly one no-retry Slot-1 launch. P03 Slot 2 remains separately unauthorized. Q-E0E-RUN remains blocked until E0-D closes.
