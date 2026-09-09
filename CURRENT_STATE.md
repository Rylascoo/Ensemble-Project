# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` backlog order.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded diagnostic **CLOSED — MACHINE-VALIDATED**. Q-E0A-02 Attempt-04 audit **DONE**: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`.

## Validation
Native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits do not inherit native authority.

## Provider
Attempt 04: first Performer `countTokens` returned **HTTP 400 / INVALID_ARGUMENT**, field `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns, no generation, $0 shadow spend. ZIP SHA-256 `042038F0CE16F07248ECE653A2E6545E67174CD3552CF7326A4A830AE90F76B1`; runtime seal valid. Authorization **CONSUMED**; provider authorization **NONE**; **DO NOT RERUN**.

## Q-E0A-04
**ACTIVE — IMPLEMENTATION/AUDIT HOSTED GREEN — NATIVE VALIDATION PENDING.** Branch `e0a-gemini-counttokens-input-projection-correction`. Contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`.

Correction: `countTokens` projects nested model + exact `systemInstruction` + `contents`, omits output-only `generationConfig`/`store`, and fails closed on request-surface drift. Generation payload, Core, fixture, retry, rate, spend and provider sequencing are unchanged. Source checkpoint `6db7d8b6...` and continuity checkpoint `2f217c68...` each passed hosted validation; final documentation-inclusive head must also pass before native execution.

## Parallel
Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-13 DONE; Q-DESIGN-14 ACTIVE. Design work does not alter E0 order or blind scoring.

## Next
After final hosted green, native-validate the exact branch HEAD with **no credentials/provider traffic** using `docs/handoff/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_HANDOFF_2026_09_09.md`. If clean, create a new annotated validation tag and close Q-E0A-04. Q-E0A-03, E0-B+ and E0-E execution remain BLOCKED. Provider authorization is **NONE**.
