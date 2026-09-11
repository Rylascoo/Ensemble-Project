# E0-A Gemini Technical-Failure Diagnostic Classification — Implementation Audit

Date: 2026-09-10

Status: **IMPLEMENTED — NATIVE WINDOWS ARM64 VALIDATED — ZERO PROVIDER TRAFFIC**

## Trigger

Run 05 `E0A-Q03-G35L-20260910-05` is immutable, consumed, and noncontributing. Performer and Integrity generation succeeded. Interpreter `countTokens` succeeded, then the first Interpreter generation returned `TechnicalFailure` before any response identity, usage receipt, or evidence-eligible stream event was preserved.

The preexisting `GeminiGenerateContentPort.ExecuteAsync` catch boundary collapsed `HttpRequestException`, `IOException`, `JsonException`, `DecoderFallbackException`, and `OverflowException` into one `gemini-malformed-or-transport` code. The sealed Run 05 evidence therefore cannot determine which technical class occurred.

## Implemented boundary

`src/Ensemble.E0.Harness/Gemini/GeminiGenerateContentPort.cs` now maps only the caught exception type to fixed diagnostic codes:

- `HttpRequestException` -> `gemini-http-transport`
- `IOException` -> `gemini-io-transport`
- `JsonException` -> `gemini-json-invalid`
- `DecoderFallbackException` -> `gemini-utf8-invalid`
- `OverflowException` -> `gemini-numeric-overflow`

No exception message, stack, response body, request body, credential, or semantic provider output is added to the diagnostic.
## Test coverage

`GeminiHardeningRegressionTests` now distinguishes:

- malformed streaming UTF-8 -> `gemini-utf8-invalid`;
- malformed streaming JSON -> `gemini-json-invalid`;
- simulated `HttpRequestException` -> `gemini-http-transport`;
- simulated `IOException` -> `gemini-io-transport`.

The `OverflowException` mapping remains defensive; no artificial provider-semantic route was added merely to force that branch.

Working-copy Windows ARM64-target results before exact-checkout freeze:

- Core: 622/622 PASS;
- Harness: 140/140 PASS.

These working-copy results were regression evidence only. Exact checkout `bb869fb1c505603612bc718f739b3f1b358e5539` subsequently passed native Windows ARM64 validation: Core 622/622, Harness 140/140, fresh build/smokes/credentialless/repository gates PASS. Durable evidence: `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_NATIVE_ARM64_VALIDATION_2026_09_10.md`; annotated tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`.

## Recursive audit

Correctness: fail-closed behavior is preserved; only observability changes. Consistency: fixed codes align with existing bounded-diagnostic practice. Authority/scope: no product, provider-admission, semantic, parser, Core, Fixture, retry, fallback, rate, spend, or accepted-turn law changes. Simplicity: one classifier and three added regressions beyond the updated UTF-8 assertion. Evidence hygiene: no provider traffic and no secret material. ARM64 suitability: source remains platform-neutral managed code; exact native Windows ARM64 validation passed at the tagged checkout.

## Disposition

The correction is native-validated and tagged. The remaining boundary is documentation-only promotion plus integration through hosted CI and green post-merge Validation. No fresh provider run is eligible before that integration closes.