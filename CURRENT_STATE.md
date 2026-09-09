# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` backlog order.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded diagnostic **CLOSED — MACHINE-VALIDATED**. Q-E0A-02 Attempt-04 audit **DONE**: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`. Gemini-3 signature compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits do not inherit native authority.

## Provider
Attempt 04: first Performer `countTokens` returned **HTTP 400 / INVALID_ARGUMENT**, field `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns, no generation, $0 shadow spend. ZIP SHA-256 `042038F0CE16F07248ECE653A2E6545E67174CD3552CF7326A4A830AE90F76B1`; runtime seal valid. Authorization **CONSUMED**; provider authorization **NONE**; **DO NOT RERUN**.

## Q-E0A-04
**ACTIVE — NATIVE VALIDATION ATTEMPT 01 FAILED; DIAGNOSIS REQUIRED.** Branch `e0a-gemini-counttokens-input-projection-correction`. Contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`. Implementation audit: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`. Failed native record: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md`.

Candidate `de38d5d52279c22a1786e11200239c445e04377b` passed hosted validation `34385049237`, then on Director Windows ARM64 passed Core **622/622** but failed Harness **130/131**. The packet hard-stopped before build/smokes/credentialless gates. No validation tag; promoted native authority is unchanged. Exact failing Harness test/assertion is not yet established from submitted output.

## Parallel
Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-13 DONE; Q-DESIGN-14 ACTIVE. Design work does not alter E0 order or blind scoring.

## Next
Read the existing native Harness TestResults log from the detached `de38d5d5...` worktree and establish the exact failing test, expected/actual values, and stack trace before any source/test patch. Do not rerun provider traffic. Native handoff: `docs/handoff/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_HANDOFF_2026_09_09.md`. Q-E0A-03, E0-B+ and E0-E execution remain BLOCKED. Provider authorization is **NONE**.
