# E0-A Gemini Bounded Provider-Error Diagnostic Amendment

Status: **FROZEN ENGINEERING AMENDMENT — IMPLEMENTATION AUTHORIZED — PROVIDER TRAFFIC NOT AUTHORIZED**

Recorded: 2026-09-08

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen E0-A architecture, and `docs/evidence/E0A_GEMINI_PROVIDER_COMPATIBILITY_DIRECTOR_DECISION_2026_09_08.md`.

## Objective

Make a future Gemini `countTokens` request rejection diagnostically useful without persisting arbitrary provider prose, raw response bodies, headers, URIs, credentials, or secret-bearing material.

The amendment is Harness-local. It does not change Core semantics, fixtures, E0-A rate discipline, attempt count, retry law, spend policy, generation payloads, deterministic authority, comparison strategy, provider authorization, or experiment sequencing.

## External contract snapshot

Verified 2026-09-08 against current Google documentation:

- Gemini/Google REST errors use the Google `Status` model with structured `code`, `message`, and `details` fields: `https://ai.google.dev/api/files`.
- `google.rpc.BadRequest` is the standard request-syntax detail type and carries `fieldViolations`: `https://docs.cloud.google.com/dotnet/docs/reference/Google.Api.CommonProtos/latest/Google.Rpc.BadRequest`.
- Google documents `BadRequest.fieldViolations[].field` as the request-field location, with list indexes represented in square brackets; `description` is explanatory prose: `https://developers.google.com/data-manager/api/devguides/concepts/understand-errors`.

These generic Google error-model facts do not assert that every Gemini rejection will contain `BadRequest`. Structured extraction is opportunistic and fail-closed; absence or malformed detail falls back to the existing HTTP-status diagnostic.

## Frozen diagnostic contract

For a non-success Gemini `countTokens` HTTP response, the only evidence-eligible provider diagnostic is a canonical bounded string:

`gemini-counttokens-http-<HTTP>[;status=<STATUS>][;field=<PATH>]...`

where:

1. `<HTTP>` is the actual HTTP status integer and must be in `100..599`.
2. `<STATUS>` is optional and retained only when `error.status` is one of the canonical Google RPC status names: `CANCELLED`, `UNKNOWN`, `INVALID_ARGUMENT`, `DEADLINE_EXCEEDED`, `NOT_FOUND`, `ALREADY_EXISTS`, `PERMISSION_DENIED`, `RESOURCE_EXHAUSTED`, `FAILED_PRECONDITION`, `ABORTED`, `OUT_OF_RANGE`, `UNIMPLEMENTED`, `INTERNAL`, `UNAVAILABLE`, `DATA_LOSS`, or `UNAUTHENTICATED`.
3. `<PATH>` is optional and comes only from a detail object whose `@type` is exactly `type.googleapis.com/google.rpc.BadRequest` and from its `fieldViolations[].field` member.
4. A retained field path must be at most 160 UTF-16 code units, use only request-path syntax composed of ASCII identifier segments, dots, and optional decimal list indexes in square brackets, and every identifier segment must correspond to a property name present in the exact outbound `countTokens` request body (camelCase or its deterministic snake_case form).
5. At most four distinct field paths are retained. Retained paths are deduplicated and sorted with ordinal comparison before serialization.
6. The complete structured error body is read only up to 16 KiB. If it exceeds that bound, cannot be read, is not valid JSON, does not contain the standard `error` object, or otherwise fails validation, structured details are discarded and the diagnostic falls back to `gemini-counttokens-http-<HTTP>`.

## Explicit exclusions

Never persist or derive diagnostic text from:

- `error.message`;
- `fieldViolations[].description`;
- `fieldViolations[].reason`;
- raw response bodies;
- arbitrary unknown detail types;
- `ErrorInfo` domain/reason/metadata;
- `RequestInfo` identifiers;
- response headers;
- request or response URIs;
- API keys, authorization material, credential values, or environment-variable values;
- provider prose embedded anywhere else in the response.

The raw error body may exist transiently in bounded memory solely for structured parsing and is never written to evidence.

## Failure behavior

- A valid structured detail enriches the existing HTTP diagnostic only; it does not alter terminal status, retry behavior, spend accounting, provider sequencing, or request count.
- A malformed, oversized, unknown, or privacy-ineligible structured detail is ignored, not treated as a second error and not surfaced through exception prose.
- Transport failure before an HTTP response remains `gemini-counttokens-transport`.
- Invalid successful `countTokens` response shape remains `gemini-counttokens-response-invalid`.
- Arbitrary `E0AHarnessException` text remains ineligible for evidence persistence.

## Required regression oracle

The implementation is not complete unless tests prove all of the following:

1. valid `INVALID_ARGUMENT` + `google.rpc.BadRequest.fieldViolations[].field` yields only the canonical structured diagnostic;
2. provider `message`, violation `description`, unrelated details, and a canary secret never enter the diagnostic or evidence;
3. malformed JSON falls back to HTTP-only;
4. oversized error bodies fall back to HTTP-only;
5. invalid/unknown status values are omitted;
6. syntactically invalid or request-unrelated field paths are omitted;
7. more than four field paths are deterministically capped, deduplicated, and sorted;
8. the run driver persists the bounded Gemini diagnostic and still suppresses arbitrary exception text;
9. no generation/provider role call occurs after `countTokens` rejection;
10. existing Harness/Core regressions remain green.

## Validation consequence

Any source change implementing this contract creates a new unvalidated executable checkpoint. Cloud compiler/test success is not native Windows ARM64 runtime authority. Before any future provider request, the exact amended checkout must pass the required native Windows ARM64 validation and receive a new annotated validation tag.

Provider authorization remains **NONE**.
