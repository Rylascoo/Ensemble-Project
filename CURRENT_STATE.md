# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` backlog order.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded diagnostic is **CLOSED — MACHINE-VALIDATED**. Q-E0A-02 Attempt-04 evidence audit is **DONE**. Archive audit: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`. Gemini 3 signature: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Native authority remains `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits do not inherit native authority.

## Provider
Attempt 04: first Performer `countTokens` returned **HTTP 400 / INVALID_ARGUMENT** with bounded field `generate_content_request.generation_config.response_format.text.mime_type`; 0 accepted turns, no generation, $0 shadow spend. ZIP SHA-256 `042038F0CE16F07248ECE653A2E6545E67174CD3552CF7326A4A830AE90F76B1`; runtime seal independently valid. Authorization **CONSUMED**; provider authorization **NONE**; **DO NOT RERUN**.

Diagnosis: current token preflight wrongly forwards output-only `generationConfig` into Gemini Developer API `countTokens`. Actual generation payload remains untested/unchanged.

## Active correction
Q-E0A-04 **ACTIVE — HOSTED GREEN / NATIVE VALIDATION PENDING** on `e0a-gemini-counttokens-input-projection-correction`. Frozen contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`. Hosted source checkpoint `6db7d8b6ac2cbe84f0ff5c8d99754a938a0f59f7` passed Validation gate `34383337117`. The implementation projects nested model + exact `systemInstruction` + `contents`, omits `generationConfig`/`store`, and fails closed on request-surface drift. Generation payload, Core, fixture, retry, rate, spend, and provider sequencing semantics are unchanged. New native Windows ARM64 validation and a new annotated validation tag are required before provider use.

## Parallel
Q-E0E-PREP CLOSED; Q-E0E-RUN remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close. Q-DESIGN-13 DONE; Q-DESIGN-14 ACTIVE for non-production F1A deployment-envelope exemplars + direct Director/Design Sol adjudication. F1A 70/460 remains provisional; E0 blind scoring and engineering order unchanged.

## Next
Recursively audit the exact Q-E0A-04 implementation and continuity-merged branch, then native-validate the resulting exact branch HEAD without credentials/provider traffic. Handoff: `docs/handoff/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_HANDOFF_2026_09_09.md`. Q-E0A-03, E0-B+, and E0-E execution remain BLOCKED. Provider authorization is **NONE**.
