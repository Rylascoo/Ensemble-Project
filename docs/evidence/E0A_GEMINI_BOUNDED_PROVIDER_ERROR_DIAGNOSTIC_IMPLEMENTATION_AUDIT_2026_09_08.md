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

## Concurrent-main reconciliation

During implementation, `main` advanced from `a254c101056e6c3ce5143e16a8dbd27ee6e1a750` to `548f1a5813ab7f3051c35c0de4b491cf6f3751b4` through one Design Sol queue-only commit. Its only file change was `docs/PROJECT_EXECUTION_QUEUE.md`, closing Q-DESIGN-03 and opening Q-DESIGN-04. The exact queue content was adopted on the E0-A branch without changing E0-A authority. Commit `17290f87b44a94769442c20ea3c4ed956b9cf8e3` then reconciled topology by preserving the audited E0-A tree byte-for-byte while adding `548f1a5813ab7f3051c35c0de4b491cf6f3751b4` as a second parent. No E0-A source/test/document content changed in that merge commit.

## Validation boundary

The promoted native executable authority remains `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`, with Director Windows ARM64 Core 622/622 and Harness 125/125 plus build/smoke/credentialless PASS.

The amended branch contains later source and test changes. It has **no native runtime authority yet**. Before any future provider request, the final reconciled checkout must:

1. pass the required Windows ARM64 Core and Harness test/build/smoke/credentialless validation;
2. receive a new annotated validation tag at that exact checkout;
3. be reflected in `docs/VALIDATION_LEDGER.md` / `CURRENT_STATE.md` without inflating the validation rung;
4. still require separate explicit Director authorization before any provider traffic.

No provider request, credential use, inference, spend, 3.1 execution, E0-A rerun, scoring, or renderer action occurred during this amendment.
