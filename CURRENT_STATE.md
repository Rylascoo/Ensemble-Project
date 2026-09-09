# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 diagnostic, Q-E0A-02 Attempt-04 audit, and Q-E0A-04 `countTokens` input-projection correction are **DONE**. Q-E0A-03 remains **BLOCKED**. Attempt-04 audit: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`; Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native authority: `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; annotated tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`, independently verified to dereference to that checkout. Windows ARM64: Core **622/622**, Harness **131/131**, fresh build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`; ledger: `docs/VALIDATION_LEDGER.md`. Later documentation commits inherit no native runtime authority.

## Provider
Attempt 04: first Performer `countTokens` HTTP **400 / INVALID_ARGUMENT** at `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns/generation/spend. Authorization **CONSUMED**. Provider authorization **NONE**; no rerun absent a new explicit authorization.

## Q-E0A-04
**DONE — MACHINE-VALIDATED AND TAGGED.** Contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`; implementation audit: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`; failed Attempt-01 record: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md`.

## Continuity / parallel lanes
Continuity: `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`; quota decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-14 DONE; Q-DESIGN-15 ACTIVE under Website authority. E0 order/blind scoring unchanged.

## Next
No active engineering implementation branch. Q-E0A-03 is blocked until a usable, separately authorized and machine-validated provider route is available with frozen fixture/configuration provenance. E0-B+ and E0-E execution remain blocked by experiment order. Any provider request requires a new explicit Director authorization; provider authorization **NONE**.
