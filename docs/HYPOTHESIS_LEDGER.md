# Ensemble Hypothesis Ledger

Status: unverified-assumption register. Hypotheses are not decisions, phase authority, provider admission, or validation evidence.

A hypothesis belongs here when engineering work depends on a fact that has not yet been established at the required evidence level. Resolved or narrowed hypotheses should record the disposition and supporting evidence rather than silently disappear.

## Open / narrowed hypotheses

### HYP-001 — Live Gemini request compatibility — NARROWED

Hypothesis: the approved REST request/streaming shapes for the current E0-A comparison profiles remain accepted by the real Google Gemini API:

- `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- `gemini-3.1-flash-lite` / `GEMINI-3.1-FLASH-LITE-MINIMAL`;
- `gemini-2.5-flash-lite` / `GEMINI-2.5-FLASH-LITE-NONE`.

Current evidence: 3.5 Flash-Lite is **not proven compatible with the full frozen request**. A fresh unshared Auth key returned HTTP 200 / `totalTokens=8` for a simple `gemini-3.5-flash-lite:countTokens` request via `x-goog-api-key`, but attempt 03 sent the first full Performer `GenerateContentRequest` through `countTokens` and Google returned HTTP 400 before generation. The exact rejected field is unknown because the bounded diagnostic did not retain Google's structured error body. Google documentation is also internally inconsistent on 3.5 Flash-Lite structured-output support. Full audit: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_ARCHIVE_AUDIT_2026_09_08.md`.

Disposition: the 3.5 Flash-Lite live path is paused by `docs/evidence/E0A_GEMINI_PROVIDER_COMPATIBILITY_DIRECTOR_DECISION_2026_09_08.md`. 3.1 Flash-Lite and 2.5 Flash-Lite full-request compatibility remain unverified; 3.1 provider execution is deferred until the bounded provider-error/request-compatibility amendment is implemented and machine-validated. Historical `gemini-2.5-flash` remains an internal compatibility anchor and is not live-selectable.

Verification trigger: after the approved bounded diagnostic amendment is machine-validated, any later provider compatibility check still requires separate Director authorization and fresh provider/account verification.

Falsifier/narrowing evidence: a real provider rejects or semantically contradicts the approved request/stream contract for the selected profile. Attempt 03 already supplies such narrowing evidence for the 3.5 full-request `countTokens` preflight.

### HYP-002 — Account/key/model/quota availability — PARTIALLY ESTABLISHED / VOLATILE

Hypothesis: the Director-selected Free-tier project and a valid replacement key can access the selected model and required `countTokens` / generation routes within the approved E0-A envelope at execution time.

Current evidence: Director-supplied AI Studio UI on 2026-09-07 established Free tier and the quota matrix: Gemini 3.5 Flash-Lite 15 RPM / 250,000 input TPM / 500 RPD; Gemini 3.1 Flash-Lite 15 / 250,000 / 500; Gemini 2.5 Flash-Lite 10 / 250,000 / 20. Historical Gemini 2.5 Flash is 5 / 250,000 / 20 and is excluded from live selection. On 2026-09-08 a fresh Auth key successfully authenticated a simple 3.5 Flash-Lite `countTokens` request through `x-goog-api-key`; an external diagnostic also demonstrated Gemini API generation access with a separate exposed test key, which must never be reused. These facts do not establish remaining current-day quota or future key/project state.

The static local RPD admission bound remains closed for one complete configured run: 3.5 Flash-Lite requires at most 72 provider operations against 500 RPD; 3.1 Flash-Lite 72/500; the three-turn 2.5 Flash-Lite control 18/20. This does not observe traffic outside the Harness.

Verification trigger: immediately before any separately authorized future provider request, including project/key association, model availability, sufficient remaining daily capacity, and all other volatile provider conditions.

Falsifier: key/project association, model availability, remaining/current quota, tier, region/project configuration, or provider account constraints do not satisfy the selected route.

### HYP-003 — Current pricing, lifecycle, and data-use state

Hypothesis: the intended account/tier pricing and data-use terms remain compatible with the approved synthetic-only E0-A run boundary at execution time.

Current evidence: provider-reference snapshots and Director-supplied Free-tier account status dated 2026-09-07, with provider facts rechecked during the 2026-09-08 live-diagnostic sequence. The approved Free-tier route remains synthetic-fixture-only because unpaid-service terms permit provider product/model improvement use of submitted content. Current shadow rates are recorded in the RPD/model-selection amendment. Gemini 3.1 Flash-Lite currently has an announced earliest shutdown date of 2027-05-07, with 3.5 Flash-Lite as the recommended successor.

Verification trigger: immediately before any authorized provider-network execution and whenever the short-lived provider snapshot expires or materially changes.

Falsifier: current provider terms, billing, retention/data-use, pricing, lifecycle, or quota differ materially from the assumptions required by the approved run.

### HYP-004 — Live provider usage/accounting mapping

Hypothesis: real Gemini response usage fields, reasoning/thinking-token reporting, cache reporting, thought-signature behavior, and terminal stream behavior map to the Harness accounting and fail-closed rules as expected for the Gemini 3 thinking-level family (3.5/3.1 Flash-Lite) and the 2.5 Flash-Lite budget-controlled family.

Current evidence: deterministic/fake regression coverage plus real 3.5 Flash-Lite `countTokens` boundary evidence. No E0-A comparison run has reached successful generation/inference, so real generation usage/reasoning/cache/stream mapping remains unverified for every current comparison profile.

Verification trigger: first separately authorized successful generation path for each materially distinct provider behavior family, after the current diagnostic/request-compatibility gate is resolved and machine-validated.

Falsifier: observed provider responses cannot be reconciled with the approved accounting/provenance rules without changing runtime semantics.

### HYP-005 — `countTokens` quota accounting

Hypothesis: Google may or may not charge `models.countTokens(generateContentRequest)` against the same project request-rate and/or daily request buckets used by generation; current authoritative provider documentation does not establish an exemption or shared-bucket rule strongly enough for the E0-A gate.

Current evidence: real `countTokens` requests have occurred, but no controlled live quota-accounting experiment or authoritative provider statement resolves their relationship to generation RPM/RPD accounting.

Current engineering disposition: the Harness conservatively rate-paces every Gemini API-bound operation, including `countTokens`; applies exact rolling input-TPM discipline to generation; and statically admits RPD as though all six possible `countTokens`+generation operations per accepted turn share the daily bucket. This is a fail-closed choice, not evidence that Google actually accounts for `countTokens` that way.

Verification trigger: authoritative provider clarification or separately authorized live evidence that can distinguish quota behavior without weakening other E0-A controls.

Falsifier/narrowing evidence: authoritative or empirical evidence establishes that `countTokens` is governed by a distinct/exempt quota surface. Any pacing or RPD-bound relaxation requires a separate audited amendment; it must not be inferred automatically.

## Closed or deferred matters that are not hypotheses

The following are decisions or scope gates and must not be reclassified as assumptions: Gemini remains the sole current E0-A provider method; historical OpenAI executable support is retired; the implemented comparison-profile set is 3.5 Flash-Lite Minimal (12 turns), 3.1 Flash-Lite Minimal (12 turns), and 2.5 Flash-Lite None (3-turn control), with historical 2.5 Flash non-live; **Gemini 3.5 Flash-Lite live execution is paused**; **Gemini 3.1 Flash-Lite provider execution is deferred pending the bounded provider-error/request-compatibility amendment**; **provider authorization is NONE**; Free-tier live experiments remain synthetic-fixture-only; .NET 10 requires later Director approval; GitHub plan-dependent branch protection is deferred; Stage remains design-lane authority.
