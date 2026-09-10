# Ensemble Current State

Updated: 2026-09-10

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; product/lane authority: `docs/PROJECT_AUTHORITY.md`; orchestration: `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - 3.5 STRUCTURED-OUTPUT COMPATIBILITY CORRECTION IMPLEMENTED; NATIVE VALIDATION PENDING**.

Run 02 `E0A-Q03-G35L-20260910-02` is immutable/consumed. Its first generation request returned HTTP 400 / `INVALID_ARGUMENT` at `generation_config.response_format.text.mime_type` after successful 649-token `countTokens`. Bounded follow-up diagnostics then proved `gemini-3.5-flash-lite` generation works and accepts the same Run 02 JSON Schema when encoded as `responseMimeType=application/json` plus `responseJsonSchema`; four diagnostic calls total are logged.

## Validation
Current promoted native authority remains `7868e5cb12a27260e288d95c248d6f846cf37701`; tag `validation/e0a-gemini-generation-error-diagnostic-native-arm64`, tag object `595fef66a79ac2939f439748fed096094b445cf1`. The compatibility correction is a newer **UNVALIDATED** executable until a fresh native Windows ARM64 validation/tag closes it.

## Evidence / provider boundary
Run 02 analysis: `docs/evidence/E0A_Q_E0A_03_G35L_RUN02_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`. Compatibility evidence: `docs/evidence/E0A_GEMINI35_STRUCTURED_OUTPUT_COMPATIBILITY_DIAGNOSTIC_2026_09_10.md`. Frozen correction: `docs/blueprint/E0A_GEMINI_GENERATECONTENT_STRUCTURED_OUTPUT_COMPATIBILITY_AMENDMENT.md`. Standing bounded Free-tier synthetic diagnostic authority and usage log remain `docs/evidence/GEMINI_API_TEST_KEY_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md` and `docs/evidence/GEMINI_API_USAGE_LEDGER.md`. No consumed reference run may be replayed.

## Continuity
Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority remains separate. Administrator C0/C1/C2 are DONE; C3 deterministic Skills remains the next earned Administrator gate and does not change Engineering/provider authority.

## Next
Recursively audit and native-validate the exact structured-output compatibility correction. If Core/Harness/build/smoke/credentialless/repository gates pass, bind a new annotated native validation tag and reconcile validation/state evidence. Only then create a fresh named 3.5 Flash-Lite full-reference run if the route remains admissible; Run 02 itself is never retried.
