# E0-A Gemini 3.5 Thought-Signature / Comparison Transport Compatibility Audit

Status: **PATCHED / CLOUD COMPILER-STATIC PASS / NATIVE WINDOWS ARM64 PENDING / REAL PROVIDER EXECUTION NOT AUTHORIZED**

Recorded: 2026-09-07

## Trigger

A recursive pre-native provider audit found two material compatibility defects after the initial comparison implementation:

1. `GeminiGenerateContentPort.RequireGeminiAttempt` still admitted only the historical `gemini-2.5-flash` compatibility anchor, so the approved 2.5 Flash-Lite and 3.5 Flash-Lite profiles would fail before HTTP.
2. The inherited 2.5 evidence policy rejected every `thoughtSignature`. Current Google GenerateContent documentation states that Gemini 3 may return an opaque `thoughtSignature` on non-function-call parts, including the final text part, and this remains possible with minimal thinking.

Provider references reverified 2026-09-07:

- `https://ai.google.dev/gemini-api/docs/generate-content/thought-signatures` — last updated 2026-09-04 UTC.
- `https://ai.google.dev/api/generate-content` — `Part.thoughtSignature` is optional base64-encoded opaque bytes metadata.
- `https://ai.google.dev/gemini-api/docs/generate-content/thinking` — GenerateContent is stateless; Gemini 3 may return signatures on part types; function-call signature replay is mandatory, ordinary text/chat replay is recommended rather than strictly validated.

E0-A has no tools/function calling and does not replay one role response as Gemini conversation history for a later role call. The amendment therefore does not introduce signature continuity into Ensemble semantics.

## Patch

Source/test checkpoint: `8ed1563ec7a4c3ae919a649db0dc3c29e17a03e0`.

Transport now:

- admits only models present in `E0AGeminiModelCatalog`;
- accepts opaque `thoughtSignature` metadata only for the explicit `gemini-3.5-flash-lite` profile;
- strips every allowed signature before streaming diagnostic persistence;
- never places a signature in `RoleAttemptReceipt` or structured semantic output;
- continues to reject actual `thought=true` material before diagnostic persistence;
- keeps the historical 2.5 paths strict against unexpected signatures.

Test support now defaults synthetic returned-model identity to `attempt.Profile.Model`, preventing false model-identity failures in comparison-profile driver tests.

## Regression surface

Harness tests cover:

- exact `countTokens` URI admission for all three approved models;
- Gemini 3.5 streaming signature acceptance plus signature-free persisted diagnostic JSON;
- Gemini 3.5 buffered signature acceptance outside semantic output;
- Gemini 3.5 `thought=true` rejection even when signature metadata is present;
- Gemini 2.5 signature rejection;
- model-relative synthetic receipts.

These tests are **compiled but not yet executed** by cloud CI.

## Validation

GitHub Validation run `34179384123` at exact source/test head `8ed1563ec7a4c3ae919a649db0dc3c29e17a03e0`: **SUCCESS**.

Passed: ARM64 cross-compile of Core/Harness/Core-tests/Harness-tests, x64 Core regression, repository law, oracle assertion coverage, document authority census.

Classification: compiler/cloud-static plus non-authoritative x64 Core regression. It is not native Windows ARM64 runtime evidence.

Compare from promoted `main` `7490de24bfd2a9829f6afc1ae4b3831c98c50837` through `8ed1563e...` shows no Core source, Core-test, or fixture change.

## Remaining gate

Native Windows ARM64 validation must execute the Harness tests, fresh build/smokes, and credentialless refusal for all three arm/profile pairs on one exact clean documentation-inclusive branch checkout. No Gemini credential, provider `countTokens`, network request, inference, or spend is authorized by this evidence.
