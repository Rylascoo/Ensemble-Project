# Ensemble Current State

Updated: 2026-09-15

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE / HOLD — P01 BOTH SLOTS CONSUMED + NONCONTRIBUTING — EXPERIENTIAL PAIR INELIGIBLE — SLOT 2 PROVIDER 503 AFTER 3/12 + UNSEALED RUNTIME-FINALIZATION DEFECT — LATER LIVE EXECUTION HELD**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`; P01 timing amendment: `docs/evidence/E0D_P01_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_15.md`. Slot 1 `E0D-Q01-P01-FULL-20260914-01` and Slot 2 `E0D-Q01-P01-REL-20260914-02` are permanently consumed; neither may be retried/replayed/replaced. P02/P03 remain unconsumed but are not live-authorized while the executable defect is unresolved.

## Native/provider authority
Last admitted executable remains checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`, tag `validation/e0d-snapshot-refresh-native-arm64`, SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier with the eligible CurrentUser-protected credential. This executable is now **inadmissible for further E0-D live launches pending repair/disposition** because Slot 2 exposed a terminal-evidence finalization defect.

## P01 terminal boundary
Slot 1: `InvalidOutput` 2/12 from noncanonical Turn-3 `MARLO`; sealed runtime; hard gates PASS / 0 findings; USD `0.008278`; terminal authority `docs/evidence/E0D_Q_E0D_01_P01_SLOT1_TERMINAL_EVIDENCE_ANALYSIS_2026_09_15.md`.

Slot 2: consumed at `2026-09-16T02:16:12Z`; 3/12 accepted turns; Turn-4 Performer generation returned HTTP 503 / `UNAVAILABLE`; 20 Gemini operations; conservative estimate USD `0.17455550` with failed-call usage unknown. The E0-D technical-outcome path then routed through baseline `DeterministicE0TurnOrchestrator.GateAttempt`, rejected the experimental context packet as stale, and threw before `Finish(...)`; therefore no `run.terminal`, `run.final.json`, runtime seal or formal evaluation seal exists. Preserve the root exactly. Authority: `docs/evidence/E0D_Q_E0D_01_P01_SLOT2_UNSEALED_TECHNICAL_TERMINAL_ANALYSIS_2026_09_15.md`.

## Next
**No further E0-D provider traffic.** Engineering must produce and regression-test a narrow context-ablation-aware technical/cancelled terminal fix, renew native Windows ARM64 validation including runtime-sealing smokes, then obtain Director disposition on the changed executable and remaining P02/P03 admissibility. P01 has no experiential score. Q-E0E-RUN remains blocked until E0-D closes.