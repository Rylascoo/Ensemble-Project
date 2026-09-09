# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 diagnostic **CLOSED — MACHINE-VALIDATED**. Q-E0A-02 Attempt-04 audit **DONE**: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`. Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`. Continuity: `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`; quota decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`.

## Validation
Promoted native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` until the new annotated tag is created and verified. Q-E0A-04 exact checkout `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2` is **NATIVE WINDOWS ARM64 VALIDATED**: Core **622/622**, Harness **131/131**, fresh build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`.

## Provider
Attempt 04: first Performer `countTokens` HTTP **400 / INVALID_ARGUMENT** at `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns/generation/spend. Authorization **CONSUMED**; provider authorization **NONE**; **DO NOT RERUN**.

## Q-E0A-04
**ACTIVE — IMPLEMENTATION AND NATIVE VALIDATION COMPLETE; ANNOTATED TAG / LEDGER / PROMOTION CLOSEOUT ONLY.** Branch `e0a-gemini-counttokens-input-projection-correction`. Contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`; implementation audit: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`; Attempt-01 evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md`.

Attempt 02 validated exact candidate `3a010df5...`. The first packet stopped only because its PowerShell helper used an unavailable overload after native tests/build/smokes passed; continuation proved no leaked evidence root, repeated all credentialless gates with a PS5.1-safe validator, and completed preservation/cleanliness checks.

## Parallel
Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-14 DONE; Q-DESIGN-15 ACTIVE under Website authority. E0 order/blind scoring unchanged.

## Next
Create/push annotated tag `validation/e0a-gemini-counttokens-input-projection-native-arm64` targeting exact `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; independently verify tag object + dereference; then update `docs/VALIDATION_LEDGER.md`, queue/current-state closeout, promote/reconcile `main`, and archive branch as appropriate. Q-E0A-03, E0-B+, E0-E execution remain BLOCKED. Provider authorization **NONE**.
