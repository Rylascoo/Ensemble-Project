# E0-A Gemini GenerateContent Structured-Output Compatibility Amendment

Status: **FROZEN ENGINEERING AMENDMENT - IMPLEMENTATION AUTHORIZED**

Recorded: **2026-09-10**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen E0-A reference architecture, Run 02 terminal evidence, and the temporary standing Gemini Free-tier synthetic diagnostic authority.

## Objective

Correct one live-falsified Gemini Developer API generation boundary. Q-E0A-03 Run 02 proved that `gemini-3.5-flash-lite` rejects the current `generationConfig.responseFormat.text.mimeType` request surface with HTTP 400 / `INVALID_ARGUMENT`.

A bounded compatibility batch then proved all of the following on the same Free-tier Auth-key project:

1. bare `gemini-3.5-flash-lite` `streamGenerateContent` succeeds;
2. `responseMimeType` plus `responseSchema` succeeds for a simple structured schema;
3. the actual Run 02 request transformed only from `responseFormat.text` to `responseMimeType` plus `responseJsonSchema` succeeds;
4. a separately captured near-runtime repeat of item 3 returned HTTP 200 with a candidate and provider usage metadata.

The provider route and model therefore remain usable. The incompatibility is the structured-output encoding used by the Harness, not general 3.5 Flash-Lite generation availability.

## Current provider contract snapshot

Verified 2026-09-10 against Google's current GenerateContent API reference: `GenerationConfig` exposes `responseMimeType`, `responseSchema`, `responseJsonSchema`, and `responseFormat`; `responseJsonSchema` accepts JSON Schema and requires a compatible `responseMimeType`. The current structured-output support table omits Gemini 3.5 Flash-Lite even though the live route accepts the legacy GenerateContent fields.
## Frozen request correction

For every current Gemini GenerateContent role request, preserve the existing JSON Schema value exactly but replace this generation configuration:

```json
"responseFormat": { "text": { "mimeType": "application/json", "schema": <schema> } }
```

with:

```json
"responseMimeType": "application/json",
"responseJsonSchema": <schema>
```

No `responseFormat` member may remain in the generated E0-A request body.

The correction is shared across current Gemini comparison profiles because they use the same GenerateContent request builder and the provider contract is endpoint-level. Live compatibility has been established specifically for 3.5 Flash-Lite; later model-specific provider evidence remains independently required by the E0 experiment plan.

## Preserved surfaces

This amendment does not change system instructions, user/context data, JSON Schema content, `candidateCount`, `maxOutputTokens`, `thinkingConfig`, `store=false`, model/profile selection, role semantics, count-token projection, API-key transport, rate pacing, token accounting, spend reservation, retry/fallback law, evidence sealing, or accepted-turn rules.

The existing `countTokens` projection continues to omit `generationConfig` entirely. No count-token request is altered by this amendment.

## Required regression oracles

Implementation is incomplete unless tests prove that direct generation requests contain `responseMimeType=application/json` and an object-valued `responseJsonSchema`; omit `responseFormat` and `responseSchema`; preserve the exact schema semantics already hashed on the prepared attempt; preserve thinking/output/store controls; and leave count-token projection behavior unchanged.
## Validation consequence

Implementation creates a new unvalidated executable checkpoint. Hosted CI does not inherit native Windows ARM64 runtime authority from `7868e5cb12a27260e288d95c248d6f846cf37701`.

Before a new full-reference provider run, the exact corrected checkout must pass the complete Core/Harness regression suite, fresh native ARM64 build and fixture smokes, credentialless provider-edge gates, repository law, document census, and oracle coverage; receive a new annotated native validation tag; and complete a recursive implementation/validation audit.

Temporary standing Gemini diagnostic authority remains governed by `docs/evidence/GEMINI_API_TEST_KEY_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md`. This amendment does not replay Run 02 or convert diagnostic calls into reference evidence.
