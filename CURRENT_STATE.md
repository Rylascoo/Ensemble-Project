# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 diagnostic **CLOSED — MACHINE-VALIDATED**. Q-E0A-02 Attempt-04 audit **DONE**: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`. Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits inherit no native authority.

## Provider
Attempt 04: first Performer `countTokens` HTTP **400 / INVALID_ARGUMENT** at `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns/generation/spend. Authorization **CONSUMED**; provider authorization **NONE**; **DO NOT RERUN**.

## Q-E0A-04
**ACTIVE — ATTEMPT 01 FAILED; STALE TEST ORACLE DIAGNOSED; TEST-ONLY CORRECTION HOSTED GREEN; ATTEMPT 02 PENDING FINAL HOSTED GREEN.** Branch `e0a-gemini-counttokens-input-projection-correction`. Contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`; audit: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`; Attempt-01 evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md`.

Attempt 01 `de38d5d5...`: Core **622/622 PASS**, Harness **130/131 FAIL**. MSTest log SHA-256 `1A79B95C4683FD0B212721E03ADCD8335730292415604D845793BA50392A18BF` proved a stale 5-field full-copy oracle; Q-E0A-04 requires `model + systemInstruction + contents`. Test-only correction `48a6e67a5f5834b40bcca1b530b87f537140984e` passed hosted gate `34402647899`.

## Parallel
Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-14 DONE; Q-DESIGN-15 ACTIVE under Website authority. E0 order/blind scoring unchanged.

## Next
Require final hosted green, then run **Native Attempt 02 from the beginning in a new detached worktree**, preserving Attempt 01, with no provider credentials/traffic. Handoff: `docs/handoff/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_HANDOFF_2026_09_09.md`. Q-E0A-03, E0-B+, E0-E execution remain BLOCKED. Provider authorization **NONE**.
