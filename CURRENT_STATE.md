# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01/02/04 **DONE**; Q-E0A-03 **ACTIVE — ONE AUTHORIZED RUN; LOCAL PREFLIGHT BLOCKED ON HISTORICAL-ROOT DRIFT**. Attempt-04 audit: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`; Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native authority: `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64 Core **622/622**, Harness **131/131**, build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`; ledger `docs/VALIDATION_LEDGER.md`. Later docs inherit no native-runtime authority.

## Q-E0A-03
Contract: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`; audit: `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`; public gate: `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`.

Exact RunId `E0A-Q03-G35L-20260909-01` is **AUTHORIZED / UNCONSUMED**: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`. Attempt-02 local stop: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_02_2026_09_09.md`. Only 3.5 Flash-Lite Minimal, canonical Missing Raft, 12 turns, exact validated executable/tag/evidence root. 3.1/2.5, retries/reruns/fallbacks/probes/alternate scope remain unauthorized. Local failure before provider invocation does not consume; any terminal result after invocation begins does. Quota: 15 RPM / 250k input TPM / 500 RPD absent contrary signal. Snapshot valid only through **2026-09-14**.

## Continuity
`docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`. Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Website authority `main` `4a87523f9964a1fcf884100e0158d75d7b3809c0`: Q-DESIGN-18 method frozen; Q-DESIGN-19 next. Engineering does not alter Design state. E0 order/blind scoring unchanged.

## Next
Run read-only Director-machine repository forensics for `C:\Users\Wiryl\Sol Dev\Ensemble-Project`; do not reset/switch/clean/mutate it. Resolve why HEAD differs from preserved `689655eed677b789ab3ee395f1c65b4f2cb72cc8`. Only after integrity is reconciled may the guarded local preflight resume. Provider authorization remains UNCONSUMED; no provider traffic. E0-B+ / E0-E remain blocked.
