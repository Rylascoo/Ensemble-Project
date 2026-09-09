# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 diagnostic **CLOSED — MACHINE-VALIDATED**. Q-E0A-02 Attempt-04 audit **DONE**: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`. Q-E0A-04 `countTokens` input-projection correction **DONE — MACHINE-VALIDATED**. Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`. Continuity: `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`; quota decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`.

## Validation
Promoted native authority is exact checkout `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; annotated tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`, independently verified to dereference to the exact checkout. Windows ARM64: Core **622/622**, Harness **131/131**, fresh build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`. Later documentation commits inherit no native runtime authority.

## Provider
Attempt 04: first Performer `countTokens` HTTP **400 / INVALID_ARGUMENT** at `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns/generation/spend. Authorization **CONSUMED**. Provider authorization **NONE**; **DO NOT RERUN** absent a new explicit authorization.

## Q-E0A-04
**DONE — MACHINE-VALIDATED AND TAGGED.** Frozen contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`; implementation audit: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`; Attempt-01 evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md`; successful native evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`.

Attempt 01 `de38d5d5...` failed only on a stale comparison oracle after Core 622/622 and Harness 130/131. Test-only correction restored the frozen `model + systemInstruction + contents` oracle. Attempt 02 validated exact checkout `3a010df5...`; the first packet's PowerShell-helper interruption was validation-apparatus-only, and the continuation independently proved no leaked evidence root, repeated every credentialless gate, and completed preservation/cleanliness checks.

## Parallel
Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-14 DONE; Q-DESIGN-15 ACTIVE under Website authority. E0 order/blind scoring unchanged.

## Next
There is no active engineering implementation branch. Q-E0A-03 remains **BLOCKED** until a usable, separately authorized and machine-validated provider route is available with frozen fixture/configuration provenance. E0-B+, E0-E execution remain BLOCKED by experiment order. Any provider request requires a new explicit Director authorization; provider authorization is **NONE**.
