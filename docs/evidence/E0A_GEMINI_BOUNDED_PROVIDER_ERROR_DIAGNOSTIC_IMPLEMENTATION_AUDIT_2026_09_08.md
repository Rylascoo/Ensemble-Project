# E0-A Gemini Bounded Provider-Error Diagnostic Implementation Audit — 2026-09-08

Status: **IMPLEMENTED — HOSTED COMPILER/STATIC GATE PASS — NATIVE WINDOWS ARM64 VALIDATION REQUIRED — PROVIDER AUTHORIZATION NONE**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`, and `docs/evidence/E0A_GEMINI_PROVIDER_COMPATIBILITY_DIRECTOR_DECISION_2026_09_08.md`.

## Falsification target

The amendment fails if a rejected Gemini `countTokens` call can persist arbitrary provider prose, raw error bodies, headers, credentials, secret-bearing metadata, an unvalidated request-field string, or more structured detail than the frozen bounded contract permits. It also fails if the diagnostic changes retry/rate/spend/attempt/generation semantics or is mistaken for provider compatibility evidence before target-device validation.

## External contract snapshot

Verified 2026-09-08 before implementation against current Google documentation:

- Google/Gemini REST errors use the shared `Status` model with `code`, `message`, and structured `details`: `https://ai.google.dev/api/files`.
- `google.rpc.BadRequest` carries request field violations: `https://docs.cloud.google.com/dotnet/docs/reference/Google.Api.CommonProtos/latest/Google.Rpc.BadRequest`.
- Google documents `BadRequest.fieldViolations[].field` as the request-field location and `description` as explanatory prose: `https://developers.google.com/data-manager/api/devguides/concepts/understand-errors`.

These are generic Google error-model facts, not evidence that every Gemini rejection includes `BadRequest`.

## Implemented boundary

The Harness now treats a failed `countTokens` response as follows:

- HTTP status remains the mandatory bounded fact: `gemini-counttokens-http-<HTTP>`.
- `error.status` is retained only from the frozen canonical Google RPC-status allowlist.
- request-field detail is accepted only from exact `type.googleapis.com/google.rpc.BadRequest` `fieldViolations[].field` entries;
- each field path must use bounded ASCII request-path syntax and resolve through the actual exact outbound `countTokens` JSON hierarchy, including real array indexes; deterministic snake_case aliases of actual camelCase property names are accepted;
- field paths are deduplicated, ordinal-sorted, and capped at four;
- the error body is streamed headers-first and read only through the 16 KiB + one-byte oversize sentinel; oversized or malformed diagnostic material collapses to HTTP-only;
- provider `message`, violation `description`, unknown detail types, ErrorInfo/RequestInfo material, headers, URIs, and credential values are never serialized into the diagnostic.

A dedicated `E0AGeminiCountTokensFailureException` carries trusted structured diagnostics to the run driver. The existing generic `E0AHarnessException` path retains only its pre-existing narrow allowlist and therefore does not gain authority to persist arbitrary structured provider text.

No Core type, fixture, generation payload, rate discipline, retry policy, attempt count, spend policy, provider sequence, scoring, renderer, or product authority changed.

## Regression construction

`GeminiGenerateContentPortWireTests` now supplies an HTTP 400 Google-style `BadRequest` body containing secret-bearing `message` and `description` canaries and asserts that only the canonical status plus an exact request field path escapes.

`GeminiCountTokensFailureDiagnosticTests` falsifies:

1. malformed JSON -> HTTP-only;
2. body larger than 16 KiB -> HTTP-only;
3. unknown status, unknown details, nonexistent request fields, and out-of-range indexes -> omitted;
4. provider prose canary -> absent;
5. valid fields -> deduplicated, ordinal-sorted, maximum four;
6. snake_case field location -> accepted only when it resolves to the exact camelCase request hierarchy.

The existing run-driver tests continue to prove that bounded legacy token-count codes can be recorded and arbitrary generic `E0AHarnessException` text is suppressed. The driver now separately recognizes the dedicated bounded Gemini failure type, and token-count failure still terminates before any generation role call or spend reservation.

## Hosted validation

Hosted workflow run `34307310623` at branch checkpoint `9442b5ced9a6c8eee897ab2370507059a9c49a79` completed successfully:

- Core build: PASS;
- Harness build: PASS;
- Core test-project build: PASS;
- Harness test-project build: PASS;
- required x64 Core regression execution: PASS;
- repository-law enforcement: PASS;
- oracle assertion coverage: PASS;
- document authority census: PASS.

The workflow does **not** execute Harness tests. Therefore this is compiler/static authority plus the required non-authoritative x64 Core regression, not Harness runtime or Windows ARM64 validation.

The later exact pre-native candidate `20f76e7d5f13915471581fb9263d6a0eb1e5343c` also passed the full hosted workflow (`34308724485`), including ARM64 cross-compile and required x64 Core regression. This still did not execute Harness tests.

## Native Windows ARM64 attempt 01 — FAIL

Director-machine validation at exact detached checkout `20f76e7d5f13915471581fb9263d6a0eb1e5343c` used native `win-arm64` / .NET 9 and produced:

- Core: **622/622 PASS**;
- Harness: **128/130 FAIL**;
- native Harness log SHA-256: `959880FED1B2736B739E7BAD3B4DFEEC9767620A396CCAF4D45B7B9FAB411B9A`;
- failed tests: `CountTokensRejectsWrongTypedTotalWithoutEscapingProviderBoundary` and `CountTokensRejectsNonObjectRootWithoutEscapingProviderBoundary`;
- both failures were exact exception-type expectation mismatches: the stale tests required `E0AHarnessException`, while the frozen amendment intentionally returns dedicated `E0AGeminiCountTokensFailureException` with `gemini-counttokens-response-invalid` for malformed successful `countTokens` responses.

The validation script stopped at Harness failure before build/smoke/credentialless completion. A subsequent read-only integrity check confirmed the validation worktree remained clean/detached at `20f76e7d...` and the historical validated root remained clean/detached at `689655eed...`.

Disposition: **validation failed; no validation tag authorized**. The correction is test-only: preserve the dedicated production exception boundary and update the two stale malformed-response regressions to assert that exact type plus exact bounded diagnostic. No production source semantics are changed by this correction.

## Concurrent-main reconciliation

During implementation, `main` advanced from `a254c101056e6c3ce514356fb531efddebed8f` to later Design Sol queue-only checkpoints. Those queue contents were adopted without changing E0-A authority, and topology was reconciled with content-preserving ancestry merges where required. The E0-A branch remains ahead of current `main` without engineering divergence; cross-lane queue content is not E0-A authority.

## Validation boundary

The promoted native executable authority remains `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`, with Director Windows ARM64 Core 622/622 and Harness 125/125 plus build/smoke/credentialless PASS.

The amended branch contains later source and test changes. It has **no native runtime authority yet**. Before any future provider request, the final corrected checkout must:

1. pass hosted gates on that exact checkout;
2. pass the required Windows ARM64 Core and Harness test/build/smoke/credentialless validation;
3. receive a new annotated validation tag at that exact checkout;
4. be reflected in `docs/VALIDATION_LEDGER.md` / `CURRENT_STATE.md` without inflating the validation rung;
5. still require separate explicit Director authorization before any provider traffic.

No provider request, credential use, inference, spend, 3.1 execution, E0-A rerun, scoring, or renderer action occurred during this amendment or failed native validation.
