# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime **E0-A Experimental Harness — Phase B**. Q-E0A-01/02/04 **DONE**; Q-E0A-03 **ACTIVE — ONE AUTHORIZED RUN; ATTEMPT-03 FALSE GATE RESOLVED; GUARDED LOCAL PREFLIGHT READY**.

## Validation
Native authority `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64 Core **622/622**, Harness **131/131**, build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`.

## Q-E0A-03
Contract `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`; planning `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`; public gate `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`.

RunId `E0A-Q03-G35L-20260909-01` is **AUTHORIZED / UNCONSUMED** via `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`. Attempt 03 `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_03_2026_09_09.md` stopped before credential/provider traffic: wrapper compared raw Windows JSON bytes to canonical `FixtureHash.Compute` identity. Correct gate: clean exact executable + fixture Git blob `6c2ed0e1081ee4e1165dbfb162b3028cbd1136fd` + native Missing-Raft smoke exercising `MissingRaftContract.Validate`. Do not raw-`Get-FileHash` the working-tree JSON against the canonical Fixture hash.

Scope remains 3.5 Flash-Lite Minimal, canonical Missing Raft, 12 turns, exact executable/tag/evidence root; no retry/rerun/fallback/probe/alternate. Local pre-provider failure does not consume; provider invocation does. Quota 15 RPM / 250k input TPM / 500 RPD absent contrary signal; snapshot through **2026-09-14**.

## Continuity
`docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`; Gemini-3 compatibility `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`. Q-E0E-PREP CLOSED; Q-E0E-RUN blocked through E0-A→D. Design/Website authority remains separate.

## Next
Run corrected guarded Windows ARM64 preflight for this RunId. Require fixture blob `6c2ed0e1081ee4e1165dbfb162b3028cbd1136fd` and passing Missing-Raft canonical-validation smoke. Only after every gate passes may process-local `GEMINI_API_KEY` be supplied and exactly one provider run launched. Provider invocation consumes authorization; no retry/rerun/fallback. E0-B+ / E0-E remain blocked.
