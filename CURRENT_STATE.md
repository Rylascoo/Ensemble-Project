# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01/02/04 **DONE**; Q-E0A-03 **ACTIVE — ONE AUTHORIZED RUN; ROOT CAUSE RESOLVED; LOCAL RESTORATION PENDING**. Attempt-04 audit: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`; Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Native authority: `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64 Core **622/622**, Harness **131/131**, build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`; ledger `docs/VALIDATION_LEDGER.md`.

## Q-E0A-03
Contract: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`; planning audit: `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`; public gate: `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`.

RunId `E0A-Q03-G35L-20260909-01` is **AUTHORIZED / UNCONSUMED**: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`. Attempt-02: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_02_2026_09_09.md`; forensic resolution: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_HISTORICAL_ROOT_FORENSIC_RESOLUTION_2026_09_09.md`. Scope remains 3.5 Flash-Lite Minimal, canonical Missing Raft, 12 turns, exact executable/tag/evidence root; no retries/reruns/fallbacks/probes/alternates. Local pre-provider failure does not consume; provider invocation does. Quota: 15 RPM / 250k input TPM / 500 RPD absent contrary signal; snapshot through **2026-09-14**.

## Continuity
`docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`. Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Website authority `4a87523f9964a1fcf884100e0158d75d7b3809c0`; Q-DESIGN-19 next. Engineering does not alter Design state.

## Next
Director machine: `git -C "C:\Users\Wiryl\Sol Dev\Ensemble-Project" switch --detach 689655eed677b789ab3ee395f1c65b4f2cb72cc8`; do not reset/clean/delete/force-move the preserved cross-lane branch. Verify exact HEAD, detached state, clean status. Only then may guarded Q-E0A-03 preflight resume against the current authorization blob. Authorization remains UNCONSUMED; no provider traffic. E0-B+ / E0-E blocked.
