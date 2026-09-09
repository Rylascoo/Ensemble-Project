# E0-A Gemini `countTokens` Input Projection Correction Amendment

Status: **FROZEN ENGINEERING AMENDMENT — IMPLEMENTATION AUTHORIZED — PROVIDER TRAFFIC NOT AUTHORIZED**

Recorded: **2026-09-09**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen E0-A reference architecture, and the audited Attempt-04 evidence in `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`.

## Objective

Correct one live-falsified Gemini Developer API boundary: E0-A currently copies output/generation-only configuration into the nested `generateContentRequest` used by `models.countTokens`. Attempt 04 proved that Google rejects the nested `generation_config.response_format.text.mime_type` field before token counting.

The correction is Harness-local. It does **not** change Core semantics, fixtures, prompts, structured context, role configuration, generation payload bytes, structured-output schema, thinking configuration, model selection, rate discipline, attempt/retry law, spend ceiling, evidence contracts, comparison strategy, experiment sequencing, or provider authorization.

## External contract snapshot

Verified 2026-09-09 against current Google documentation:

- `models.countTokens` counts request **input** and accepts either `contents` or a nested `generateContentRequest`; the nested form exists to include input-steering surfaces such as system instruction and function declarations: `https://ai.google.dev/api/tokens`.
- Current Google GenAI SDK documentation for `CountTokensConfig.generationConfig` states: configuration used to generate the response is **not supported by the Gemini Developer API**: `https://googleapis.github.io/js-genai/release_docs/interfaces/types.CountTokensConfig.html` and `https://googleapis.github.io/dotnet-genai/api/Google.GenAI.Types.CountTokensConfig.html`.
- The same `CountTokensConfig` surfaces document `systemInstruction` and `tools` as count-token inputs.
- Current Gemini structured-output documentation continues to support JSON response formatting for generation. Attempt 04 therefore does not authorize a generation-shape change.

## Frozen projection contract

For the current E0-A prepared Gemini request body, `CountTokensRequestBody` must fail closed unless the top-level JSON object has exactly these four distinct properties:

1. `systemInstruction`;
2. `contents`;
3. `generationConfig`;
4. `store`.

The current `store` value must be JSON `false`, and the prepared generation body must continue to omit top-level `model`.

The outbound `countTokens` body must then be exactly one nested object:

```json
{
  "generateContentRequest": {
    "model": "models/<exact profile model>",
    "systemInstruction": <exact prepared systemInstruction>,
    "contents": <exact prepared contents>
  }
}
```

Rules:

- nested `model` is injected from the exact validated profile model;
- `systemInstruction` is copied semantically/byte-value-equivalently from the prepared generation request;
- `contents` is copied semantically/byte-value-equivalently from the prepared generation request;
- `generationConfig` is **deliberately omitted** from `countTokens` because the Gemini Developer API does not support that count-token configuration surface;
- `store` is **deliberately omitted** because it is generation/service behavior, not input content to tokenize;
- no other source property may be silently dropped or forwarded. Any new top-level prepared-request surface fails locally until this contract is explicitly amended and tested.

This fail-closed rule is intentional. Future tool declarations, cached content, or another token-bearing input cannot enter generation while being silently absent from preflight accounting.

## Generation immutability

`ExecuteAsync` / `generateContent` / `streamGenerateContent` continue to send `attempt.RequestBody` unchanged. In particular, this amendment does not remove or alter:

- `generationConfig.candidateCount`;
- `generationConfig.maxOutputTokens`;
- structured `generationConfig.responseFormat.text.mimeType` or schema;
- `generationConfig.thinkingConfig`;
- `store=false`.

Attempt 04 did not reach generation. Its evidence cannot be used to infer that those generation fields are invalid.

## Diagnostic compatibility

The existing bounded `countTokens` provider-error diagnostic remains unchanged. Field-path validation continues to resolve only against the **actual outbound corrected `countTokens` body**.

Tests that use synthetic `google.rpc.BadRequest.fieldViolations` must therefore use a path that exists in the corrected projection, such as a path beneath `generateContentRequest.contents`, rather than a deliberately omitted `generationConfig` path.

No raw provider message, description, response body, header, URI, credential, or arbitrary provider prose becomes eligible for persistence.

## Spend / token accounting consequence

Google documents `countTokens` as counting input tokens. The corrected projection retains the current E0-A token-bearing prompt surfaces: system instruction and contents. Output ceilings remain reserved separately under the existing spend model; actual generation usage remains authoritative after a successful provider role call.

This amendment does not create a new local estimate for unsupported generation configuration and does not weaken the existing spend ceiling or input-envelope checks.

## Required regression oracle

Implementation is incomplete unless tests prove all of the following:

1. corrected token projection includes exact nested model, system instruction, and contents;
2. corrected token projection omits `generationConfig` and `store`;
3. direct generation request construction still contains the frozen `generationConfig`, structured JSON response format/schema, thinking control, and `store=false`;
4. a prepared request with any unknown/additional top-level property fails locally before HTTP;
5. missing/duplicate/noncanonical current top-level properties fail locally before HTTP;
6. the bounded structured diagnostic still extracts only eligible status/field data using a field path present in the corrected body;
7. provider message/description/canary secret remain excluded;
8. all current model-route token-count tests continue to inject the exact nested model;
9. no Core/fixture semantics change;
10. complete Harness/Core regressions remain green.

## Validation consequence

Any implementation of this amendment creates a **new unvalidated executable checkpoint**. Hosted compiler/test success does not inherit native Windows ARM64 runtime authority from `e6e7d6c8...`.

Before any future provider request, the exact corrected checkout must:

1. pass standard hosted repository validation;
2. pass the complete required native Windows ARM64 Core/Harness/build/smoke/credentialless validation package;
3. receive a new annotated validation tag bound to that exact machine-tested checkout;
4. complete a fresh recursive audit of the correction and validation evidence;
5. receive separate explicit Director authorization for any provider traffic.

Provider authorization is **NONE**. This amendment authorizes source/test/documentation work and non-network validation only.