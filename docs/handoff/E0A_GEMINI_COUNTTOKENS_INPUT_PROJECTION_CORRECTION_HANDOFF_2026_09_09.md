# E0-A Fresh-Chat Handoff — Gemini `countTokens` Input Projection Correction

Status: **Q-E0A-04 ACTIVE — NON-NETWORK ENGINEERING ONLY — PROVIDER AUTHORIZATION NONE**

Recorded: 2026-09-09

Authority: temporary transition guidance only while `CURRENT_STATE.md` names this path. It cannot enlarge the frozen amendment, inherit native validation, authorize provider traffic, or advance E0-B+.

## Fresh-chat first action

1. Resolve current `Rylascoo/Ensemble-Project` refs and read `CURRENT_STATE.md` at exact current `main` first.
2. Read `docs/PROJECT_AUTHORITY.md`, `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`, `docs/PROJECT_EXECUTION_QUEUE.md`, and `docs/VALIDATION_LEDGER.md`.
3. Read:
   - `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`;
   - `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`;
   - `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`;
   - `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_NATIVE_ARM64_VALIDATION_2026_09_08.md`;
   - `docs/evidence/E0A_GEMINI35_ATTEMPT04_DIRECTOR_AUTHORIZATION_2026_09_09.md` for consumed historical scope only.
4. Resolve branch `e0a-gemini-counttokens-input-projection-correction` and compare it to current `main` before acting.
5. Provider authorization is **NONE**. Do not use credentials or send any provider request.

## Evidence-established defect

Attempt 04 at native-valid executable `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` reached the first Performer `countTokens` request and returned:

`gemini-counttokens-http-400;status=INVALID_ARGUMENT;field=generate_content_request.generation_config.response_format.text.mime_type`

The uploaded ZIP independently matches SHA-256 `042038F0CE16F07248ECE653A2E6545E67174CD3552CF7326A4A830AE90F76B1`; its runtime root/seal and all seven artifact hashes validate. There were zero accepted turns, no generation, no spend reservation, no provider generation receipt, and no semantic state change.

Current source copies every prepared generation field into nested `generateContentRequest` for `countTokens`, including output-only `generationConfig` and `store`. Current Google GenAI SDK documentation states `CountTokensConfig.generationConfig` is not supported by the Gemini Developer API.

## Frozen correction

Implement only `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`.

For the current exact prepared-request top-level surface (`systemInstruction`, `contents`, `generationConfig`, `store`):

- fail closed unless the surface is exactly that known shape, with distinct properties and `store=false`;
- token-count nested request contains only:
  - exact `model = models/<profile model>`;
  - exact `systemInstruction`;
  - exact `contents`;
- deliberately omit `generationConfig` and `store` from `countTokens`;
- fail closed on any unknown future source property rather than silently undercounting it.

Actual generation continues to send `attempt.RequestBody` unchanged, including structured response format/schema, thinking control and `store=false`.

## Required test correction

The existing regression `CountTokens_AddsRequiredNestedModelPreservesGenerationFieldsAndUsesApiKeyHeader` encodes the falsified full-projection behavior and must be replaced with an input-projection oracle.

Required tests must prove:

- exact nested model/systemInstruction/contents;
- omission of `generationConfig` and `store` from token-count request;
- generation request still carries its frozen generation config/structured JSON shape;
- unknown/missing/duplicate/noncanonical top-level request surfaces fail before HTTP;
- bounded structured diagnostic still extracts eligible fields from the corrected outbound body and never provider prose/secrets;
- current model-route coverage and all existing regressions remain green.

## Validation boundary

Any source change creates a new unvalidated executable. Hosted green status does not inherit the native authority of `e6e7...`.

After implementation and recursive audit:

1. require the standard hosted Validation gate at the exact implementation head;
2. prepare a separate native Windows ARM64 validation worktree/checkpoint;
3. run complete Core + Harness tests, fresh win-arm64 Harness build, fixture/generic/credentialless smokes, checkout/credential hygiene;
4. if native clean, create a new annotated validation tag at that exact checkout and update validation evidence/ledger/state;
5. only a later explicit Director authorization can permit any provider traffic.

## Standing exclusions

Do not:

- rerun Attempt 04;
- change the generation payload in response to a countTokens-only rejection;
- remove structured output or change model/profile/fixture;
- add retries/fallbacks/probes;
- alter Core or fixture semantics;
- broaden the bounded provider-error diagnostic unless separately justified;
- advance Q-E0A-03/E0-B+/E0-E execution before their prerequisites.

Recursively audit correctness, evidence, provider-contract interpretation, token/spend accounting, privacy, source/test scope, native-validation identity, branch topology and continuity until one complete pass finds no material correction or worthwhile improvement.