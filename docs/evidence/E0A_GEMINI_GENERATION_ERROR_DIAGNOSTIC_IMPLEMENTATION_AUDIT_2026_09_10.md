# E0-A Gemini Generation Error Diagnostic — Implementation Audit

Date: 2026-09-10

Status: **PASS — EXACT SOURCE CANDIDATE `7868e5cb12a27260e288d95c248d6f846cf37701` — PROVIDER TRAFFIC NOT AUTHORIZED**

## Scope

This audit reviews the correction defined by `docs/blueprint/E0A_GEMINI_GENERATION_ERROR_DIAGNOSTIC_CORRECTION_AMENDMENT.md` and triggered by the sealed Q-E0A-03 Run 01 terminal analysis.

Base: `91b419575069b6ad5b2aef60380a1797aef9f856` (`origin/main` at candidate freeze).

Candidate: `7868e5cb12a27260e288d95c248d6f846cf37701`.

Changed executable/test surfaces are exactly:

- `src/Ensemble.E0.Harness/Gemini/E0AGeminiCountTokensFailureException.cs`;
- `src/Ensemble.E0.Harness/Gemini/E0AGeminiHttpFailureDiagnostic.cs` (new shared bounded parser);
- `src/Ensemble.E0.Harness/Gemini/GeminiGenerateContentPort.cs`;
- `tests/Ensemble.E0.Harness.Tests/GeminiGenerateContentPortWireTests.cs`.

No Core, Fixture, prompt/schema construction, model catalog, pricing, rate, retry, fallback, or evidence-authority code changed.
## Implementation result

The former `countTokens`-specific bounded parser was factored into `E0AGeminiHttpFailureDiagnostic` without changing its allowlist or output grammar. `E0AGeminiCountTokensFailureException` remains the countTokens-specific exception boundary and delegates to the shared parser using the existing `gemini-counttokens-http` stem.

`GeminiGenerateContentPort` now invokes the shared parser only when buffered `generateContent` or streaming `streamGenerateContent` returns a non-success HTTP status. Successful response parsing and all provider-request construction remain unchanged.

Generation diagnostics retain the existing HTTP-only baseline and may append only:

- canonical allowlisted Google RPC `status`;
- up to four distinct, ordinal-sorted `google.rpc.BadRequest.fieldViolations[].field` paths;
- only field paths that are syntactically safe and resolve against the exact outbound generation request body.

The existing 16 KiB error-body bound, malformed/oversized fallback, snake_case/camelCase property resolution, and exclusion of arbitrary provider prose remain shared and fail-closed.

## Regression oracle

Three new Harness tests prove:

1. streaming generation rejection preserves bounded structured diagnostic metadata and suppresses provider prose;
2. buffered generation rejection does the same;
3. malformed streaming-generation error JSON falls back to `gemini-http-400`.

Existing `GeminiCountTokensFailureDiagnosticTests` remain unchanged and continue to exercise the shared parser's privacy, path, cap, sorting, malformed-body, oversized-body, and status rules.
## Recursive audit

The candidate was checked for diagnostic leakage, accidental generation-payload mutation, Core/Fixture drift, retry/fallback expansion, provider-authorization expansion, duplicated parsing logic, and editor-introduced byte artifacts.

A pre-commit audit found UTF-8 BOMs introduced by the editing mechanism in existing C# files; those incidental artifacts were removed before the source candidate was committed. `git diff --check` then passed.

No evidence supports changing `responseFormat`, `thinkingConfig`, `store`, prompt/schema data, or another generation request field. The correction deliberately improves observability without adopting a speculative provider workaround.

Implementation audit result: **PASS**. Native validation of the exact candidate is required before promotion or any future provider request.
