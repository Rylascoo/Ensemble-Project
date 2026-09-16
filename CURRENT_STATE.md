# Ensemble Current State

Updated: 2026-09-15

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE - P01 SLOT 1 CONSUMED / TERMINAL NONCONTRIBUTING `InvalidOutput` 2/12 / HARD GATES PASS / P01 EXPERIENTIAL PAIR INELIGIBLE**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. P01 pair window remains Director-amended to `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`; P02/P03 remain unchanged. P01 Slot 1 `E0D-Q01-P01-FULL-20260914-01` is permanently consumed; no retry/replay/replacement exists. P01 Slot 2 remains preregistered/unconsumed but separately unauthorized.

## Native/provider authority
Checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`; tag `validation/e0d-snapshot-refresh-native-arm64`; executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`; snapshot valid through UTC `2026-09-18`. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier with the new CurrentUser-protected credential; old credential ineligible.

## P01 Slot 1 terminal
PR #151 integrated the corrected-preclaim Director disposition to `main@fc42f5441b820d0dfd82654c37dbf4d2ddd025e2`; exact-main Validation #869 PASS. Fresh authenticated capacity was `0/15` RPM, `0/250K` TPM, `0/500` RPD. Slot 1 then consumed its claims/root and terminated `InvalidOutput` after 2 accepted turns when Turn-3 Performer control returned noncanonical `addressedCharacterIds=["MARLO"]`; no Turn-3 candidate entered Production. Sealed spend estimate `0.008278` USD; hard gates PASS / 0 findings. Terminal authority: `docs/evidence/E0D_Q_E0D_01_P01_SLOT1_TERMINAL_EVIDENCE_ANALYSIS_2026_09_15.md`.

## Next
**Do not retry Slot 1.** Director must separately decide whether to activate P01 Slot 2. If authorized, repeat fresh authenticated project/key/model/tier/RPM/TPM/RPD capacity and every frozen interlock before the exact Slot-2 claim; otherwise stop. P01 is already experiential-ineligible because Full is noncontributing. Later slots remain governed by their own frozen order/windows and separate authority. Q-E0E-RUN remains blocked until E0-D closes.
