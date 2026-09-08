# E0-A First Real Gemini 3.5 Flash-Lite — Attempt 01

Status: **AUTHORIZATION CONSUMED — TERMINAL TECHNICAL FAILURE — COUNT-TOKENS REQUEST-SHAPE DEFECT CORRECTED — CLOUD GATES PASS — NATIVE VALIDATION PENDING — NO RETRY AUTHORIZED**

Date: 2026-09-08

## Authority

Director authorization record: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_DIRECTOR_AUTHORIZATION_2026_09_08.md`.

Authorized executable checkout: `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` (`validation/e0a-gemini-rpd-model-selection-native-arm64`).

Authorized route: exactly one synthetic Missing Raft `CREATIVE-MINIMAL` / `GEMINI-3.5-FLASH-LITE-MINIMAL` run. No retry or second provider run was authorized.

## Director-machine transcript facts

The one authorized invocation began at `2026-09-08T04:39:59.7873582Z` and ended at `2026-09-08T04:40:01.2732976Z`.

```text
exit code                   3
terminal status             TechnicalFailure
accepted turns              0
estimated shadow spend USD  0.000000
stderr                       empty
evidence root                created
evidence file count          8
post-run credential cleanup PASS
```

Transcript SHA-256:

`8B3E772498ADE9378AB0BC16BE822E863228966C31FC8C460D2CB902C525BE56`

Evidence ZIP SHA-256:

`98400A524D7500A16B07A5D4EB2AB7CF79E455FFEDA08F2237616C110523FC07`

The uploaded ZIP independently matched that SHA-256. Every artifact listed by `run.final.json` matched its recorded SHA-256. No API-key marker (`AIza`, `GEMINI_API_KEY`, `x-goog-api-key`, `apiKey`, or `api_key`) was present in the archived text evidence.

The Director checked AI Studio shortly after termination and reported that the dashboard showed no usage. This is useful account-surface evidence but is not by itself proof that no Gemini HTTP request reached the service.

## Archive audit

`events.ndjson` contains, in order:

1. `run.started`;
2. `context.composed` for turn 1 / VOSS;
3. `preflight.failed` for `E0A-REAL-G35L-20260908-01:ATTEMPT:PERFORMER:001:01`, code `input-token-count-failed`, elapsed `566.1856 ms`;
4. `run.terminal` = `TechnicalFailure`, accepted turns `0`, estimated spend `0`, unknown provider usage `false`.

There is exactly one prepared attempt request and no terminal provider receipt, no `preflight.input-tokens`, no generation `rate.ready`, no `spend.reserved`, no `provider.completed`, and no stream evidence. Therefore the first failure occurred inside the Performer `countTokens` preflight. Generation/inference was **not reached**.

The archive cannot prove whether the service received the HTTP request because the validated token-counter transport collapsed non-success status, response-shape failure, and transport failure into the same `E0AHarnessException` and did not preserve provider HTTP status/body. A retry is not authorized merely to obtain that missing diagnostic.

## Request-shape defect

The archived prepared generation request is valid as the generation payload and intentionally omits a top-level `model` because generation binds the model in the endpoint path.

At the validated executable checkout, `GeminiGenerateContentPort.CountInputTokensAsync` constructed the count-token body as:

```text
{ "generateContentRequest": <exact generation request body> }
```

It therefore also omitted `generateContentRequest.model`.

Provider contract reverified 2026-09-08:

- `https://ai.google.dev/api/tokens` defines `models.countTokens` and its nested `generateContentRequest` as a `GenerateContentRequest`;
- the Gemini GenerateContent contract requires model identity in `models/{model}` form;
- `googleapis/python-genai#432` records the corresponding API 400 diagnostic when a nested generated request omitted the model: `CountTokensRequest.generate_content_request.model: model is not specified`.

The existing wire regression `CountTokens_ProjectsExactGenerateContentRequestAndUsesApiKeyHeader` enforced byte-identical projection of the generation body into `generateContentRequest`. That test therefore preserved the defect instead of checking the provider schema.

Because attempt 01 did not retain the provider HTTP diagnostic, its exact returned HTTP status/message is not historical evidence and must not be invented.

## Correction

Correction checkpoint:

`689655eed677b789ab3ee395f1c65b4f2cb72cc8`

Cloud Validation:

`34188767617` — **PASS** across ARM64 cross-compilation, x64 Core regression, repository law, oracle assertion coverage, and document authority census.

The correction is limited to Harness and Harness-test surfaces:

- `CountInputTokensAsync` now builds a schema-specific nested `generateContentRequest` with `model = models/{attempt.Profile.Model}` while preserving every field from the prepared generation request and leaving the actual generation payload unchanged;
- the token preflight now classifies non-success HTTP as bounded `gemini-counttokens-http-<status>`, malformed response as `gemini-counttokens-response-invalid`, and transport failure as `gemini-counttokens-transport` without persisting response bodies or credentials;
- the run driver persists only that bounded allowlisted diagnostic and discards arbitrary exception text;
- the predecessor byte-identity wire test was replaced by a schema-complete nested-model/preserved-fields test;
- all current live comparison models now have a regression proving the nested count-token model matches their exact route;
- driver regressions prove bounded HTTP diagnostic persistence and suppression of unapproved exception text.

Compare from the archive-audit checkpoint to the correction changes only:

- `src/Ensemble.E0.Harness/Gemini/GeminiGenerateContentPort.cs`;
- `src/Ensemble.E0.Harness/Run/E0AReferenceRunDriver.cs`;
- `tests/Ensemble.E0.Harness.Tests/GeminiGenerateContentPortWireTests.cs`;
- `tests/Ensemble.E0.Harness.Tests/GeminiModelComparisonTests.cs`;
- `tests/Ensemble.E0.Harness.Tests/GeminiRunDriverPolicyTests.cs`.

No Core, Core-test, or fixture change is present.

Cloud compilation is not native Windows ARM64 runtime authority and the Harness tests are not executed by the cloud gate. The correction therefore remains **native validation pending**.

## Provider/accounting disposition

- authorization: **CONSUMED**;
- accepted fictional history: **NONE**;
- first failing boundary: **Performer `countTokens` preflight**;
- generation/inference reached: **NO**;
- spend reservation reached: **NO**;
- provider generation request reached: **NO**;
- exact attempt-01 `countTokens` HTTP status/body: **NOT CAPTURED**;
- AI Studio usage display shortly after run: **none observed by Director**;
- second run / retry / 3.1 / 2.5 execution: **NOT AUTHORIZED**.

## Next gate

Run fresh native Windows ARM64 validation on the corrected exact documentation-inclusive checkout after continuity reconciliation. No provider credential/network request belongs in that validation. Any later real-provider invocation requires a new explicit Director authorization.