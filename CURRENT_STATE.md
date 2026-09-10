# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01/02/04 **DONE**; Q-E0A-03 **ACTIVE — ONE AUTHORIZED RUN; ATTEMPT-03 WRAPPER FALSE GATE RESOLVED; GUARDED LOCAL PREFLIGHT READY**. Attempt-04 audit: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`; Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Native authority: `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64 Core **622/622**, Harness **131/131**, build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`; ledger `docs/VALIDATION_LEDGER.md`.

## Q-E0A-03
Contract: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`; planning audit: `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`; public gate: `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`.

RunId `E0A-Q03-G35L-20260909-01` is **AUTHORIZED / UNCONSUMED**: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`. Attempts 01/02 and root restoration are preserved through that record and `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_HISTORICAL_ROOT_FORENSIC_RESOLUTION_2026_09_09.md`. Attempt 03: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_03_2026_09_09.md`; it stopped before credential/provider traffic because the wrapper incorrectly compared the raw Windows working-tree JSON digest to the canonical `FixtureHash.Compute` identity. Corrected law is exact clean executable + exact fixture Git blob `6c2ed0e1081ee4e1165dbfb162b3028cbd1136fd` + native Missing-Raft smoke exercising `MissingRaftContract.Validate`; do not raw-`Get-FileHash` the working-tree JSON against the canonical Fixture hash. Scope remains 3.5 Flash-Lite Minimal, canonical Missing Raft, 12 turns, exact executable/tag/evidence root; no retries/reruns/fallbacks/probes/alternates. Local pre-provider failure does not consume; provider invocation does. Quota: 15 RPM / 250k input TPM / 500 RPD absent contrary signal; snapshot through **2026-09-14**.

## Continuity
`docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`. Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Website authority `4a87523f9964a1fcf884100e0158d75d7b3809c0`; Q-DESIGN-19 next. Engineering does not alter Design state.

## Next
Run the corrected guarded Windows ARM64 local preflight for `E0A-Q03-G35L-20260909-01` against the exact current authorization-record blob. Require exact fixture Git blob `6c2ed0e1081ee4e1165dbfb162b3028cbd1136fd` and passing native Missing-Raft canonical-validation smoke; do not compare raw working-tree file bytes to the canonical Fixture hash. Only if every gate passes, securely supply process-local `GEMINI_API_KEY` and launch exactly one provider run. Evidence root `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0A-Q03-G35L-20260909-01`. Provider invocation consumes authorization; no retry/rerun/fallback. E0-B+ / E0-E remain blocked.
