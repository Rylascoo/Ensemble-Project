# E0-A Third Real Gemini 3.5 Flash-Lite Attempt 03 — Archive Audit

Status: **AUDIT COMPLETE — RUNTIME SEAL VALID — FIRST PERFORMER `countTokens` HTTP 400 — NO GENERATION — PROVIDER REQUEST-COMPATIBILITY REVIEW REQUIRED**

Date: **2026-09-08**

## Authority and artifact

Authorization: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_DIRECTOR_AUTHORIZATION_2026_09_08.md`  
Attempt record: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_2026_09_08.md`  
Run: `E0A-REAL-G35L-20260908-03`  
Executable: `689655eed677b789ab3ee395f1c65b4f2cb72cc8`  
Profile: `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`  
Fixture: canonical synthetic Missing Raft

Director-machine transcript recorded the evidence ZIP SHA-256 as:

`8120E426AE206439A9E43F8D5C85C277B492B9055A0ED012133793B91EC1D77E`

The uploaded archive independently recomputed to that exact SHA-256.

## Runtime-seal verification

All seven artifacts named by `run.final.json` were present and independently SHA-256 verified against the seal:

- `attempts/E0A-REAL-G35L-20260908-03_ATTEMPT_PERFORMER_001_01/request.json` — `0ead141b219b43fb3072ef81dcc269fbb9d9db66a77736ba57a83e3103f11537`;
- `blind/mapping.json` — `23c6ba69a36ec3f8d91ff044bb940bc8e510dd6ef4826af3e57dcdb4c6fbca54`;
- `blind/transcript.json` — `e727aeae83a51b77385e348538bfdc3890f361b5b3fad23cd30ef395e02c3de1`;
- `events.ndjson` — `5b7ca6699ac81d531250316ff627ab82653cb4f27d89b44f4bc06327d0d43ab3`;
- `manifest.json` — `8f417ed87775054b04b9b8acaaf9f730a4e52060960630ea3e3f65538c6a18ab`;
- `run.summary.json` — `ac1c12f425fb3db8c5557f8682325a6ea9105f3a2c49f3b2862ed5ca9ead4fc4`;
- `transcript.json` — `e727aeae83a51b77385e348538bfdc3890f361b5b3fad23cd30ef395e02c3de1`.

Using the exact `E0AEvidenceSealAuthority` algorithm from executable checkout `689655...`, the audit recomputed:

- `runtimeRoot = 490b3457f6c9e61bec53fc76d90d8db7c17c08e435a3746282398f10cca73070`;
- `runtimeSealIdentity = 5d1f8ae19d7fc73ad9e9c05f27e996ddcf6db40ac2b226ca49c771c655360983`.

Both exactly match `run.final.json`. The runtime seal is internally valid.

No `AQ.` credential prefix, `GEMINI_API_KEY`, `x-goog-api-key`, `?key=`, or common legacy API-key prefix was present anywhere in the archive.

## Exact event sequence

`events.ndjson` contains exactly four runtime events:

1. `run.started` from initial state hash `30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`, opportunity `VOSS`;
2. `context.composed` for turn 1 / subject `VOSS`;
3. `preflight.failed` for `PERFORMER:001:01` with:
   - code `input-token-count-failed`;
   - provider diagnostic `gemini-counttokens-http-400`;
   - elapsed `480.4031 ms`;
4. `run.terminal` = `TechnicalFailure`, `acceptedTurns=0`, `estimatedSpendUsd=0`, `hasUnknownProviderUsage=false`.

There is no `preflight.input-tokens`, spend reservation, generation pacing/completion, provider receipt, stream event, accepted performance, Integrity invocation, Interpreter invocation, or committed turn.

Therefore the exact terminal boundary is the **first Performer `countTokens` HTTP request**, which reached Google and returned **HTTP 400**. Generation/inference was never reached.

## Request/source correlation

The preserved Performer request identifies model `gemini-3.5-flash-lite`, `thinkingLevel=minimal`, one candidate, `maxOutputTokens=4096`, structured JSON `responseFormat`, `store=false`, system instruction, and bounded user content.

At executable `689655...`, `GeminiGenerateContentPort.CountTokensRequestBody` wraps that prepared generation request as `generateContentRequest`, injecting nested `model = models/gemini-3.5-flash-lite`; `CreateRequest` sends the credential in `x-goog-api-key`. This is the post-attempt-01 correction.

Google's current API reference documents `models.countTokens` as accepting `generateContentRequest` of type `GenerateContentRequest`. The current GenerateContent reference documents the fields used by this preserved request, including `systemInstruction`, `generationConfig`, `store`, `thinkingConfig`, and `generationConfig.responseFormat`. The `responseFormat.text.schema` shape is the current JSON-Schema structured-output surface.

The immediately preceding live diagnostic proved the same fresh Auth key, same `x-goog-api-key` transport, same `gemini-3.5-flash-lite:countTokens` method, and a simple content body return HTTP 200 / `totalTokens=8`. Accordingly, attempt 03 is not explained by the key, header transport, model endpoint, or `countTokens` method alone. The failure is triggered by some aspect of the fuller `GenerateContentRequest` or its provider-side validation.

## Provider-documentation inconsistency found

The frozen comparison architecture requires structured JSON output and the preserved request uses the currently documented `generationConfig.responseFormat` JSON-Schema surface.

Google's current model-specific `Gemini 3.5 Flash-Lite` page explicitly marks **Structured outputs Supported**. However, Google's current general GenerateContent structured-output guide says “The following models support structured output” and its explicit support table omits Gemini 3.5 Flash-Lite while listing Gemini 3.1 Flash-Lite, Gemini 3.5 Flash, and the 2.5 family.

Those official provider surfaces are inconsistent. The omission therefore cannot by itself prove that Gemini 3.5 Flash-Lite lacks structured-output capability, and the model-specific page prevents classifying the route as definitively unsupported on that basis. It does establish a provider-fact inconsistency that should have been caught by the model-admissibility audit before live execution.

The archive does **not** retain Google's 400 response body. We therefore cannot identify the exact rejected field, distinguish a provider implementation defect from a request-feature interaction, or prove that `responseFormat` caused the 400. The evidence-supported classification is narrower: **the full frozen 3.5 Flash-Lite `GenerateContentRequest` is not yet proven live-compatible with `countTokens`; provider-side validation returned HTTP 400 for the first Performer request.**

## Consequence

- Attempt 03 authorization is consumed.
- Provider authorization is **NONE**.
- No retry or fourth 3.5 Flash-Lite reference run is justified from the current evidence.
- No authentication-transport patch is justified.
- The 3.5 Flash-Lite profile remains **live-compatibility unproven / pending Director resolution**, not proven generally incapable of structured output.
- `gemini-3.1-flash-lite` is explicitly present in Google's current general structured-output support table and remains an approved comparator, but no real 3.1 run is automatically authorized by this audit.
- Before any further provider request, model/request admissibility should reconcile all request-critical provider facts and the official-document inconsistency above.

## Improvement identified

The bounded `countTokens` diagnostic currently preserves HTTP status but discards the structured provider error body. A later engineering patch could safely retain bounded provider error classification such as `error.status` plus validated `google.rpc.BadRequest.fieldViolations[].field` paths while discarding arbitrary descriptions/text. That would materially improve fault localization without preserving provider prose or secrets, but it is a source change and requires normal cloud/native Windows ARM64 validation and a new validation tag before provider use.
