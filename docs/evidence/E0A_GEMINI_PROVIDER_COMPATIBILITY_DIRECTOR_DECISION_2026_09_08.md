# E0-A Gemini Provider-Compatibility Director Decision — 2026-09-08

Status: **APPROVED — GEMINI 3.5 FLASH-LITE LIVE EXECUTION PAUSED — BOUNDED PROVIDER-ERROR / REQUEST-COMPATIBILITY ENGINEERING AMENDMENT AUTHORIZED — PROVIDER TRAFFIC NOT AUTHORIZED**

## Authority

Director decision recorded 2026-09-08 after completion of:

- `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_2026_09_08.md`;
- `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_ARCHIVE_AUDIT_2026_09_08.md`;
- `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`.

`CURRENT_STATE.md` remains the only active phase/checkpoint/validation/next-action authority. This record preserves the Director decision and does not independently advance validation authority.

## Decision

The Director approves all of the following as one bounded engineering direction:

1. **Pause Gemini 3.5 Flash-Lite live execution.** Do not perform a retry, fourth 3.5 Flash-Lite reference run, fallback, or other 3.5 provider request while the full frozen request remains live-compatibility unproven.
2. **Proceed with a bounded provider-error diagnostic / request-compatibility engineering amendment.** The purpose is to make future provider-side request rejection diagnosable without retaining arbitrary provider prose, raw response bodies, secrets, or credentials.
3. **Do not advance the Gemini 3.1 Flash-Lite comparator yet.** Its provider gate is deferred until the diagnostic/request-compatibility amendment is designed, implemented, recursively audited, and machine-validated, so another reference-run authorization is not spent against the same opaque failure class.
4. **No provider traffic is authorized by this decision.** No credential use, `countTokens`, generation/inference, provider probe, spend, or external provider request is authorized.

## Engineering boundary

The authorized amendment is expected to remain Harness-local and as small as possible. It may modify the Gemini provider/error-diagnostic surface, its Harness tests, and the documentation/evidence needed to bind the change. It must not alter deterministic Core semantics or the frozen Missing Raft fixture unless a separate Director-approved architecture decision explicitly requires that change.

The diagnostic design must remain fail-closed and privacy-safe. At minimum it must distinguish bounded structured provider error facts from arbitrary text. Candidate retained facts may include validated provider status/code values and validated structured field-violation paths such as `google.rpc.BadRequest.fieldViolations[].field`; arbitrary descriptions/messages, raw bodies, headers, URIs containing credentials, and credential material remain ineligible for persistence.

This decision does not pre-decide the exact schema or implementation. The fresh engineering chat must reconcile the exact Google error envelope and current repository contracts before freezing the smallest amendment.

## Validation consequence

Any source change creates a new executable checkpoint and therefore cannot inherit the existing native machine-tested authority for the modified executable. Before any future provider request on the amended path, the project must complete the applicable cloud gates, native Windows ARM64 Core/Harness tests, fresh native Harness build and relevant credentialless/request-shape smokes, then create the required annotated validation tag for the exact machine-tested checkout.

No provider-validation claim may be made from static/compiler/native credentialless evidence alone.

## Next gate

Prepare and execute the bounded engineering amendment in a fresh engineering chat using the active handoff named by `CURRENT_STATE.md`. Stop at the next external/native-validation or Director provider-authorization gate. Provider authorization remains **NONE** throughout this engineering work unless separately granted later.
