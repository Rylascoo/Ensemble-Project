# E0-A Gemini Generation Error Diagnostic Correction Amendment

Status: **FROZEN ENGINEERING CORRECTION — IMPLEMENTED AND NATIVE-VALIDATED — PROVIDER TRAFFIC NOT AUTHORIZED**

Recorded: 2026-09-10

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, the frozen E0-A reference envelope, `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`, and the immutable Q-E0A-03 Run 01 evidence.

## Trigger

Q-E0A-03 Run `E0A-Q03-G35L-20260909-01` passed the corrected Gemini `countTokens` preflight with 649 input tokens, reached the first Performer generation request, and terminated on provider HTTP 400. The sealed terminal receipt retained only `gemini-http-400`.

The preserved evidence therefore proves a Harness diagnostic-observability defect: generation HTTP failures discarded bounded structured Google error metadata that the Harness already safely retained for `countTokens`. The evidence does **not** prove which generation request field, if any, Google rejected.

## Frozen correction boundary

For non-success Gemini `generateContent` and `streamGenerateContent` responses, the Harness may enrich the existing `gemini-http-<HTTP>` diagnostic using the same bounded Google error grammar and privacy rules already frozen for `countTokens`:

`gemini-http-<HTTP>[;status=<STATUS>][;field=<PATH>]...`

The structured parser is shared with the existing `countTokens` diagnostic implementation so both boundaries retain the same allowlist, size cap, path validation, deterministic ordering, and provider-prose exclusions.

The `countTokens` external diagnostic remains exactly `gemini-counttokens-http-<HTTP>[;status=<STATUS>][;field=<PATH>]...`.
## Explicit non-changes

This correction does not change:

- generation request bytes, prompt text, response schema, thinking controls, `store`, streaming selection, model/profile selection, Fixture, or Core semantics;
- provider admission, account/key/quota policy, pricing, spend ceiling, attempt count, retry, fallback, cancellation, or experiment ordering;
- evidence immutability, accepted Performance authority, State Authority, or causal commit;
- Q-E0A-03 provider authorization. Provider authority remains **NONE**.

Arbitrary provider `message`, `description`, unknown detail data, raw response bodies, headers, credentials, URIs, and secret-bearing metadata remain ineligible for persisted diagnostics.

## Regression oracle

The correction is complete only if tests prove:

1. streaming generation rejection retains only validated canonical status/field metadata;
2. buffered generation rejection retains only validated canonical status/field metadata;
3. provider prose is absent from the persisted diagnostic;
4. malformed error JSON falls back to `gemini-http-<HTTP>`;
5. existing bounded `countTokens` diagnostic tests remain green;
6. full Core and Harness regressions remain green.

## Validation consequence

The source change creates a new executable checkpoint. Before any future provider request, the exact corrected checkout must be validated on native Windows ARM64 and bound to an annotated validation tag. That validation does not itself authorize provider traffic.
