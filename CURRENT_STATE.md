# Ensemble Current State

Updated: 2026-09-17

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — P01 FULLY CONSUMED/NONCONTRIBUTING — P02 FULLY CONSUMED/NONCONTRIBUTING / EXPERIENTIALLY INELIGIBLE — P03 FROZEN/UNCONSUMED/SEPARATELY UNAUTHORIZED**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. P01 Slots 1/2 are permanently consumed/noncontributing. P02 Slot 1 `E0D-Q01-P02-OMNI-20260914-03` and Slot 2 `E0D-Q01-P02-FULL-20260914-04` both sealed `InvalidOutput` at 2/12 accepted turns with hard gates PASS; neither may be retried/replayed/replaced or experientially scored. P03 remains unconsumed. P03 window remains `2026-09-18T14:30:00Z..23:30:00Z`.

## Native/provider authority
Promoted checkout `8770a6361233e8a877a966c45eb6f62c5b3ca182`, tag `validation/e0d-context-ablation-terminal-fix-native-arm64`, executable SHA-256 `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`; native evidence `docs/evidence/E0D_CONTEXT_ABLATION_TERMINAL_FINALIZATION_REPAIR_NATIVE_ARM64_VALIDATION_2026_09_16.md`; admissibility `docs/evidence/E0D_SUCCESSOR_EXECUTABLE_P02_P03_ADMISSIBILITY_DIRECTOR_DISPOSITION_2026_09_16.md`. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier.

## P02 terminal closure
Slot-1 terminal: `docs/evidence/E0D_Q_E0D_01_P02_SLOT1_TERMINAL_EVIDENCE_ANALYSIS_2026_09_16.md`. Slot-2 authorization/activation: `docs/evidence/E0D_P02_SLOT2_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_16.md` and `docs/evidence/E0D_Q_E0D_01_P02_SLOT2_PREEXECUTION_ACTIVATION_2026_09_16.md`. Slot-2 terminal: `docs/evidence/E0D_Q_E0D_01_P02_SLOT2_TERMINAL_EVIDENCE_ANALYSIS_2026_09_17.md`. Slot 2 consumed exactly once, used 14 Gemini operations, sealed `InvalidOutput` 2/12 with known usage and ordinary Full-reference hard gates PASS. Both P02 members are noncontributing; the pair is fully consumed and experientially ineligible.

## Next
Integrate this P02 Slot-2 terminal closeout through exact-head hosted Validation + E0-E preparation and push-triggered exact-main Validation. Do **not** retry either P02 slot. P03 remains separately unauthorized: no claim, provider traffic, scoring, or execution until a fresh explicit Director live-execution authorization and all frozen launch-boundary gates are satisfied inside its fixed window. Q-E0E-RUN remains blocked until E0-D closes.
