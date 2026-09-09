# E0-A Gemini 3.5 Flash-Lite Attempt 04 — Archive Audit

Status: **AUDIT COMPLETE — RUNTIME SEAL VALID — FIRST PERFORMER `countTokens` HTTP 400 / INVALID_ARGUMENT — EXACT REJECTED FIELD IDENTIFIED — NO GENERATION**

Date: **2026-09-09**

## Authority and artifact

Run: `E0A-REAL-G35L-20260909-04`  
Executable: `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`  
Validation tag: `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`  
Profile: `CREATIVE-MINIMAL` / `GEMINI-3.5-FLASH-LITE-MINIMAL`  
Fixture: canonical synthetic Missing Raft  
Authorization: `docs/evidence/E0A_GEMINI35_ATTEMPT04_DIRECTOR_AUTHORIZATION_2026_09_09.md`

The Director-machine packet recorded ZIP SHA-256:

`042038F0CE16F07248ECE653A2E6545E67174CD3552CF7326A4A830AE90F76B1`

The uploaded archive independently recomputed to that exact SHA-256. It is 7,932 bytes and contains exactly eight members. No absolute path, traversal path, duplicate normalized path, or suspicious compression ratio was found.

## Runtime-seal verification

All seven artifacts named by `run.final.json` are present and independently match their recorded SHA-256:

- `attempts/E0A-REAL-G35L-20260909-04_ATTEMPT_PERFORMER_001_01/request.json` — `2166406f335a193ede3a107b267a730c2dc354b2c27bda30b2ea291ad7e188b7`;
- `blind/mapping.json` — `23c6ba69a36ec3f8d91ff044bb940bc8e510dd6ef4826af3e57dcdb4c6fbca54`;
- `blind/transcript.json` — `e727aeae83a51b77385e348538bfdc3890f361b5b3fad23cd30ef395e02c3de1`;
- `events.ndjson` — `100e49b3738083d230d1ce3eec08f074ba3061d1235ffa15dfec995ff63fc714`;
- `manifest.json` — `5cd061e596a100620bfc027a1b96d5387c7386782d11d5f59b9e1f94f0f53fae`;
- `run.summary.json` — `ac1c12f425fb3db8c5557f8682325a6ea9105f3a2c49f3b2862ed5ca9ead4fc4`;
- `transcript.json` — `e727aeae83a51b77385e348538bfdc3890f361b5b3fad23cd30ef395e02c3de1`.

Using the exact `E0AEvidenceSealAuthority` algorithm from the executable lineage, the audit independently recomputed:

- `runtimeRoot = 3352234670f8b804f44ed18102e1197370153963c091f60b9cb08140c3077157`;
- `runtimeSealIdentity = d635be3bb8b0600221b039df926c093ff4dd3b35026bae3ad01bffc2b15ee54e`.

Both exactly match `run.final.json`. The runtime seal is internally valid.

## Exact terminal boundary

`events.ndjson` contains exactly four events:

1. `run.started`;
2. `context.composed` for turn 1 / `VOSS`;
3. `preflight.failed` for `PERFORMER:001:01`, code `input-token-count-failed`, elapsed `483.4467 ms`, with the approved bounded diagnostic:

   `gemini-counttokens-http-400;status=INVALID_ARGUMENT;field=generate_content_request.generation_config.response_format.text.mime_type`

4. `run.terminal` = `TechnicalFailure`, `acceptedTurns=0`, `estimatedSpendUsd=0`, `hasUnknownProviderUsage=false`.

There is no `preflight.input-tokens`, generation rate-ready event, spend reservation, provider completion/receipt, stream evidence, accepted performance, Integrity invocation, Interpreter invocation, or committed turn. Both visible and blind transcripts contain zero performances.

Therefore the exact terminal boundary is the **first Performer full-request `countTokens` preflight**. Google returned HTTP 400 / RPC `INVALID_ARGUMENT` and identified the exact rejected nested field as `generate_content_request.generation_config.response_format.text.mime_type`. Generation/inference was never reached.

## Request correlation

The archived prepared generation request is internally hash-bound. Its `requestBodyUtf8` independently hashes to the recorded `requestBodyHash` `eb13c82856afe0124d464889f5341ab0fa15189f10dcf177b82b7364ce1777b3`.

The generation request contains the expected E0-A surfaces:

- `systemInstruction`;
- `contents`;
- `generationConfig` with one candidate, `maxOutputTokens=4096`, minimal thinking, and structured `responseFormat.text.mimeType = application/json` plus JSON Schema;
- `store=false`.

At executable `e6e7...`, `GeminiGenerateContentPort.CountTokensRequestBody` injects nested `model = models/gemini-3.5-flash-lite` and then copies **every** top-level generation-request property into `generateContentRequest`. Thus the rejected `generationConfig.responseFormat.text.mimeType` was definitely present in the actual `countTokens` body.

## Current provider-contract resolution

Current Google Gemini documentation continues to document structured JSON output for generation, including `response_format` / JSON MIME type, and the Gemini 3.5 Flash-Lite model-specific surface identifies structured output as supported. Therefore this archive does **not** establish that the generation request's structured-output configuration is invalid.

Current Google GenAI SDK documentation for `CountTokensConfig`, however, explicitly states that `generationConfig` is **not supported by the Gemini Developer API**, while `systemInstruction` and `tools` are supported count-token configuration surfaces. Google's token-counting documentation also describes `countTokens` as counting request input.

The evidence-supported diagnosis is therefore:

**E0-A's Gemini token preflight incorrectly projects output/generation configuration into the Gemini Developer API `countTokens` request.**

The exact live-falsified behavior is the prior rule that the nested `generateContentRequest` should preserve every generation field. The generation payload itself remains untested by Attempt 04 and must not be changed on the strength of this evidence.

## Privacy / evidence audit

The archive contains no detected `AIza` API-key pattern, `GEMINI_API_KEY`/`OPENAI_API_KEY` assignment, bearer credential, or persisted `x-goog-api-key` value. No raw provider error body, provider message, violation description, header, URI credential, or arbitrary provider prose is persisted. The only provider rejection detail is the allowlisted canonical status/field diagnostic required by the frozen bounded-diagnostic amendment.

The packet's post-run checkout-integrity and credential-cleanup checks also passed. No credential value is part of this repository evidence.

## Recursive audit conclusion

A complete audit across archive integrity, runtime seal, provenance, privacy, terminal sequencing, request/source correlation, provider contract, spend/retry law, generation isolation, validation identity, and queue order found one required engineering correction and no basis for another provider call.

Selected correction:

- make Gemini `countTokens` an **input-semantic projection**, not a byte-preserving generation-request projection;
- for the current frozen E0-A request surface, retain nested `model`, exact `systemInstruction`, and exact `contents`;
- deliberately omit generation-only `generationConfig` and `store` from token counting;
- fail closed if the prepared request top-level surface changes, so future token-bearing features cannot be silently omitted;
- leave actual generation bytes, structured-output schema, model/profile, fixture, retries, rate/spend law, evidence law, and provider sequencing unchanged.

Contract: `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`.

## Disposition

- Attempt 04 authorization: **CONSUMED**;
- provider authorization: **NONE**;
- Attempt 04: **DO NOT RERUN**;
- Q-E0A-02 evidence-ingestion/audit objective: **DONE**;
- accepted fictional history: **NONE**;
- generation reached: **NO**;
- shadow spend: **$0.000000**;
- Q-E0A-03: **BLOCKED**;
- successor Q-E0A-04: non-network Harness correction + validation only.

Any source correction creates a new unvalidated executable checkpoint. Before any future provider request, that exact corrected checkout must pass the required hosted gates, full native Windows ARM64 validation, receive a new annotated validation tag, and obtain separate explicit Director provider authorization.