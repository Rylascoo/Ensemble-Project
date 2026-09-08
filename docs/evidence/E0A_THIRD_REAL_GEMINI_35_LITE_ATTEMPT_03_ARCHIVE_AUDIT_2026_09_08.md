# E0-A Third Real Gemini 3.5 Flash-Lite Attempt 03 — Archive Audit

Status: **AUDIT COMPLETE — RUNTIME SEAL VALID — FIRST PERFORMER `countTokens` HTTP 400 — NO GENERATION — PROVIDER CONTRACT REVIEW REQUIRED**

Date: **2026-09-08**

## Authority and artifact

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

At executable `689655...`, `GeminiGenerateContentPort.CountTokensRequestBody` correctly wraps the prepared generation request as `generateContentRequest`, injecting nested `model = models/gemini-3.5-flash-lite`; `CreateRequest` sends the credential in `x-goog-api-key`. This is the post-attempt-01 correction and is structurally consistent with Google's current `models.countTokens` reference, which permits a full `GenerateContentRequest` and requires its model identity.

The immediately preceding live diagnostic also proved the same fresh Auth key, same `x-goog-api-key` transport, same `gemini-3.5-flash-lite:countTokens` method, and a simple content body return HTTP 200 / `totalTokens=8`. Accordingly, attempt 03 is not explained by the key, header transport, model endpoint, or `countTokens` method alone.

## Provider-contract inconsistency found

The frozen comparison architecture requires **structured JSON output** for the Gemini 3.5 Flash-Lite route. The preserved request therefore sends `generationConfig.responseFormat` with a JSON schema.

Google's current GenerateContent structured-output documentation (reverified 2026-09-08 at `https://ai.google.dev/gemini-api/docs/generate-content/structured-output`) states that its listed models support structured output. That explicit support table includes `Gemini 3.1 Flash-Lite`, `Gemini 3.5 Flash`, and the 2.5 family, but **does not list Gemini 3.5 Flash-Lite**. Google's model/release documentation separately confirms `gemini-3.5-flash-lite` is a current stable model and supports the relevant thinking level; the omission is therefore a capability/admissibility issue, not a model-lifecycle issue.

Because this project is fail-closed on provider facts, a model absent from an explicit provider support table cannot be treated as proven compatible with a required contract feature. The current approved 3.5 Flash-Lite profile was admitted without an explicit structured-output capability gate. That is a material architecture-audit gap.

The archive does **not** retain Google's 400 response body, so the audit cannot prove that `responseFormat` is the precise field Google rejected. The strongest evidence-supported root-cause candidate is nevertheless a model/request-feature incompatibility in the full `GenerateContentRequest`, with structured-output support the first concrete incompatibility found. The exact provider field violation remains unproven.

## Consequence

- Attempt 03 authorization is consumed.
- Provider authorization is **NONE**.
- No retry or fourth 3.5 Flash-Lite reference run is justified.
- No authentication-transport patch is justified.
- The current 3.5 Flash-Lite live profile should be treated as **provider-contract inadmissible/pending Director resolution** until structured-output capability is explicitly established or the comparison architecture moves to a documented-compatible model.
- `gemini-3.1-flash-lite` is explicitly present in Google's current structured-output support table and remains an approved comparison profile, but no real 3.1 run is automatically authorized by this audit.
- Before any further provider request, model admissibility must include every request-critical capability, not only quota/pricing/thinking/lifecycle.

## Improvement identified

The bounded `countTokens` diagnostic currently preserves HTTP status but discards the structured provider error body. A later engineering patch may safely retain a bounded provider status/reason and validated field-violation path without persisting arbitrary provider text. That would improve future fault localization, but it is not required to establish this audit's exact HTTP boundary and must receive normal source/native validation before use.
