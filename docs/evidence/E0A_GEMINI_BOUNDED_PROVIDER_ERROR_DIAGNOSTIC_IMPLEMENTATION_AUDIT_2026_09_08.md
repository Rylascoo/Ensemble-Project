# E0-A Gemini Bounded Provider-Error Diagnostic Implementation Audit — 2026-09-08

Status: **IMPLEMENTED — HOSTED PASS — NATIVE WINDOWS ARM64 PASS — PROMOTED MACHINE-TESTED CHECKOUT — PROVIDER AUTHORIZATION NONE**

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

- HTTP status remains the mandatory bounded fact: `gemini-counttokens-http-<HTTP>`;
- `error.status` is retained only from the frozen canonical Google RPC-status allowlist;
- request-field detail is accepted only from exact `type.googleapis.com/google.rpc.BadRequest` `fieldViolations[].field` entries;
- each field path must use bounded ASCII request-path syntax and resolve through the actual exact outbound `countTokens` JSON hierarchy, including real array indexes; deterministic snake_case aliases of actual camelCase property names are accepted;
- field paths are deduplicated, ordinal-sorted, and capped at four;
- the error body is streamed headers-first and read only through the 16 KiB + one-byte oversize sentinel; oversized, malformed, or unreadable diagnostic material collapses to HTTP-only;
- provider `message`, violation `description`, unknown detail types, ErrorInfo/RequestInfo material, headers, URIs, and credential values are never serialized into the diagnostic.

A dedicated `E0AGeminiCountTokensFailureException` carries trusted structured diagnostics to the run driver. The existing generic `E0AHarnessException` path retains only its pre-existing narrow allowlist and therefore does not gain authority to persist arbitrary structured provider text.

No Core type, fixture, generation payload, rate discipline, retry policy, attempt count, spend policy, provider sequence, scoring, renderer, or product authority changed.

## Regression construction

`GeminiGenerateContentPortWireTests` supplies an HTTP 400 Google-style `BadRequest` body containing secret-bearing `message` and `description` canaries and asserts that only the canonical status plus an exact request field path escapes.

`GeminiCountTokensFailureDiagnosticTests` falsifies malformed/oversized bodies, unknown status/detail material, nonexistent or out-of-range request paths, provider prose leakage, deterministic dedupe/sort, the four-path cap, and exact snake_case-to-request-hierarchy resolution.

The run driver recognizes the dedicated bounded Gemini failure type before the legacy safe-code filter; generic Harness exception text remains non-persistable.

## Hosted validation

Hosted workflow `34309626093` at exact corrected checkout `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` completed successfully:

- Core build: PASS;
- Harness build: PASS;
- Core test-project build: PASS;
- Harness test-project build: PASS;
- required x64 Core regression execution: PASS;
- repository-law enforcement: PASS;
- oracle assertion coverage: PASS;
- document authority census: PASS.

The workflow does not execute Harness tests; hosted authority therefore remained compiler/static plus the required non-authoritative x64 Core regression.

## Native Windows ARM64 attempt 01 — FAIL

At exact detached checkout `20f76e7d5f13915471581fb9263d6a0eb1e5343c`:

- Core: 622/622 PASS;
- Harness: 128/130 FAIL;
- log SHA-256: `959880FED1B2736B739E7BAD3B4DFEEC9767620A396CCAF4D45B7B9FAB411B9A`;
- failures: `CountTokensRejectsWrongTypedTotalWithoutEscapingProviderBoundary` and `CountTokensRejectsNonObjectRootWithoutEscapingProviderBoundary`.

Both failures were strict exception-type expectation mismatches: the old tests required `E0AHarnessException`, while the frozen amendment intentionally returns `E0AGeminiCountTokensFailureException` with `gemini-counttokens-response-invalid` for malformed successful `countTokens` responses. The validation stopped before build/smoke/credentialless completion. No tag was authorized.

Correction: test-only. The two stale tests were updated to assert the dedicated type and exact bounded diagnostic. Production source semantics were unchanged.

## Native Windows ARM64 attempt 02 — PASS

At exact clean detached checkout `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` on Director Windows ARM64:

- Core: 622/622 PASS;
- Harness: 130/130 PASS;
- fresh Debug `win-arm64` Harness build: PASS, 0 warnings / 0 errors;
- missing-Raft fixture smoke: PASS;
- generic fixture smoke: PASS;
- current-profile credentialless gates: PASS;
- retired-profile pre-credential rejection: PASS;
- provider network/inference: NOT PERFORMED;
- spend: 0;
- validation worktree remained clean/detached at the exact checkout;
- historical validated root remained clean/detached at `689655eed677b789ab3ee395f1c65b4f2cb72cc8`.

Native evidence: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

## Promotion

Annotated validation tag:

`validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`

Tag object `9b17563474b25920da8615afefdfa3fcfdab884b` was independently verified to dereference exactly to machine-tested checkout `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`.

The temporary connector closeout probe was archived at `archive/tmp-e0a-native-closeout-probe` and its redundant branch removed. It carried zero unique project content.

## Concurrent-main reconciliation

During implementation `main` advanced through Design Sol queue work. E0-A adopted legitimate queue state when needed but did not import the Design-lane edit that attempted to write engineering-authoritative `CURRENT_STATE.md`. Current `main` is an ancestor of the E0-A closeout lineage; no Design-side unique branch is merged by assumption.

## Final boundary

Q-E0A-01 is machine-validated and may close after its durable state/queue bookkeeping passes repository gates. Q-E0A-02 remains blocked. Before any future provider request, provider/account/pricing/quota facts must be reverified and explicit Director authorization must be granted.

Validation does not establish live Gemini compatibility and does not authorize credentials, provider traffic, inference, spend, 3.1 execution, E0-A rerun, scoring, or renderer action.
