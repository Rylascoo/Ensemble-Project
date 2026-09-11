# E0-A Gemini Technical-Failure Diagnostic Classification Amendment

Date: 2026-09-10

Status: **FROZEN IMPLEMENTATION CONTRACT — ZERO PROVIDER TRAFFIC**

Implementation audit: docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_IMPLEMENTATION_AUDIT_2026_09_10.md.

## Trigger

Q-E0A-03 Run 05 (`E0A-Q03-G35L-20260910-05`) reached a successful Performer generation and successful Integrity generation, then terminated on the first Interpreter generation with `TechnicalFailure`, no response identity, no usage receipt, and diagnostic `gemini-malformed-or-transport`.

The Interpreter `countTokens` preflight succeeded immediately beforehand. No Interpreter stream event was persisted. The current catch boundary collapses `HttpRequestException`, `IOException`, `JsonException`, `DecoderFallbackException`, and `OverflowException` into one code, so the preserved evidence cannot distinguish a transport failure from malformed provider bytes/JSON or numeric overflow.

Run 05 is consumed and may never be retried or replayed. This amendment exists only to improve future fail-closed evidence classification before another provider run is considered.

## Frozen correction

`GeminiGenerateContentPort.ExecuteAsync` keeps the same caught exception set and fail-closed `TechnicalFailure` outcome, but maps the exception class to a bounded diagnostic code:

- `HttpRequestException` -> `gemini-http-transport`
- `IOException` -> `gemini-io-transport`
- `JsonException` -> `gemini-json-invalid`
- `DecoderFallbackException` -> `gemini-utf8-invalid`
- `OverflowException` -> `gemini-numeric-overflow`

`OperationCanceledException` continues to rethrow unchanged.

## Security and evidence boundary

The diagnostic may contain only the fixed code above. It must not contain exception messages, URLs, headers, provider response bodies, request bodies, credentials, model-generated text, filesystem paths, or other uncontrolled data.

Existing bounded HTTP non-success diagnostics remain unchanged. Existing response-shape, identity, safety, usage, finish-reason, thought-metadata, timeout/cancellation, rate, spend, evidence-seal, parser, Core, and causal-authority behavior remains unchanged.

## Explicit non-changes

This amendment does **not** change model/profile selection, endpoints, request serialization, structured-output schemas, prompts, fixture, token ceilings, rate limits, spend rules, retry count, fallback/substitution, accepted-turn law, semantic parsers, deterministic authority, or provider authorization.

It does not retrospectively classify Run 05 beyond the evidence already preserved. The old `gemini-malformed-or-transport` receipt remains historically correct for that executable.

## Required verification

Before promotion:

1. malformed UTF-8 must fail closed as `gemini-utf8-invalid`;
2. malformed SSE JSON must fail closed as `gemini-json-invalid`;
3. `HttpRequestException` must fail closed as `gemini-http-transport`;
4. streaming `IOException` must fail closed as `gemini-io-transport`;
5. full Harness and Core regressions must pass on native Windows ARM64;
6. fresh Harness build, fixture smokes, credentialless provider-edge gates, repository law, document census, and oracle guard must pass;
7. provider network, `countTokens`, generation, inference, and spend during validation must remain zero.

A later provider run requires a fresh RunId/evidence root and normal standing-authority preregistration after this exact correction is native-validated, tagged, integrated, and post-merge validated. Run 05 remains immutable and noncontributing.