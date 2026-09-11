# E0-A Gemini Technical-Failure Diagnostic Classification — Implementation Audit

Date: 2026-09-10

Status: **IMPLEMENTED — ZERO PROVIDER TRAFFIC — NATIVE VALIDATION PENDING**

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

These working-copy results are regression evidence only and do not promote native validation authority.

## Recursive audit

Correctness: fail-closed behavior is preserved; only observability changes. Consistency: fixed codes align with existing bounded-diagnostic practice. Authority/scope: no product, provider-admission, semantic, parser, Core, Fixture, retry, fallback, rate, spend, or accepted-turn law changes. Simplicity: one classifier and three added regressions beyond the updated UTF-8 assertion. Evidence hygiene: no provider traffic and no secret material. ARM64 suitability: source remains platform-neutral managed code; exact native validation remains required before promotion.

## Disposition

Freeze an exact source candidate only after repository-law/document-census/oracle/diff checks pass. Then validate that exact checkout on the Director Windows ARM64 host, create an annotated validation tag if clean, and only then promote the validation ledger/current state and integrate through hosted CI.