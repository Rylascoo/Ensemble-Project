# E0-A Gemini 3.5 Flash-Lite Structured-Output Compatibility Diagnostic

Date: 2026-09-10

Status: **CLOSED - LEGACY GENERATECONTENT SHAPE ACCEPTED**

Authority: `docs/evidence/GEMINI_API_TEST_KEY_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md`.

## Trigger

Q-E0A-03 Run 02 reached `gemini-3.5-flash-lite:streamGenerateContent` after a successful 649-token `countTokens` call and returned HTTP 400 / `INVALID_ARGUMENT` with provider field path `generation_config.response_format.text.mime_type`.

## Diagnostic batch 01

Synthetic/no-secret Free-tier calls, same model and endpoint:

1. bare generation without structured-output controls -> HTTP 200;
2. `responseMimeType=application/json` plus simple `responseSchema` -> HTTP 200;
3. the actual Run 02 request transformed only from `responseFormat.text` to `responseMimeType` plus `responseJsonSchema`, with diagnostic output cap 128 -> HTTP 200.

The local batch-01 serializer then failed while constructing its summary object. That post-processing failure occurred after all three HTTP responses and does not alter the observed provider statuses. No retry loop occurred.

## Durable capture batch 02

One additional bounded near-runtime call repeated item 3 solely to preserve request/response evidence.

- evidence root: `GEMINI35-FORMAT-COMPAT-20260910-02` on the Director machine;
- HTTP: `200`;
- candidate present: `true`;
- request SHA-256: `1661cd402e398714fc3ddb794a2918c239e31783043225145a5b11cb36884d17`;
- response-body SHA-256: `3faed305fd55f4636857c61873afa0eb5021d0bec4369026d2d779af36eb151b`;
- provider usage: prompt `649`, candidate `114`, total `763` tokens;
- finish reason: `MAX_TOKENS`, expected from the deliberately reduced 128-token diagnostic cap;
- joined candidate text was incomplete JSON, so this call proves request/format acceptance, not completion of a reference role output.
## Provider/document reconciliation

Google's current GenerateContent API reference exposes `responseMimeType`, `responseSchema`, and `responseJsonSchema` inside `GenerationConfig`; `responseJsonSchema` accepts JSON Schema when paired with a compatible MIME type. Current structured-output guidance separately omits Gemini 3.5 Flash-Lite from its model-support table. The live diagnostic therefore governs this exact route: 3.5 Flash-Lite accepts the legacy GenerateContent structured-output fields even though the newer `responseFormat.text` shape used by the Harness is rejected.

## Engineering disposition

The defect is localized to request encoding. Preserve the exact existing JSON Schema and change the shared Gemini GenerateContent builder to `responseMimeType=application/json` plus `responseJsonSchema=<same schema>`, with `responseFormat` absent. Do not switch models or change prompts, thinking, output ceilings, token preflight, rate/spend controls, or experiment semantics on the strength of this diagnostic.

Provider calls consumed by this investigation: **4** total (3 in batch 01, 1 in capture batch 02). The earlier launcher failures consumed **0** provider calls.
