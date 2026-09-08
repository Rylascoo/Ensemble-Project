# Ensemble Hypothesis Ledger

Status: unverified-assumption register. Hypotheses are not decisions, phase authority, provider admission, or validation evidence.

A hypothesis belongs here when engineering work depends on a fact that has not yet been established at the required evidence level. Resolved or narrowed hypotheses should record the disposition and supporting evidence rather than silently disappear.

## Open hypotheses

### HYP-001 — Live Gemini request compatibility

Hypothesis: the approved REST request/streaming shapes for the three E0-A comparison profiles remain accepted by the real Google Gemini API:

- `gemini-2.5-flash-lite` / `GEMINI-2.5-FLASH-LITE-NONE`;
- `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- `gemini-2.5-flash` / `GEMINI-2.5-FLASH-NONE`.

Current evidence: current provider documentation, compiler/static validation, fake/provider-simulation coverage, and historical credentialless native execution for the predecessor 2.5 Flash route. No real Gemini request has been made by the current comparison integration.

Verification trigger: immediately before and during each separately Director-authorized provider-network comparison run.

Falsifier: the real provider rejects or semantically contradicts the approved request/stream contract for the selected profile.

### HYP-002 — Account/key/model/quota availability

Hypothesis: the Director-selected Free-tier project and replacement key can access the selected comparison model and required `countTokens` / generation routes within the approved E0-A envelope at execution time.

Current evidence: Director-supplied AI Studio account UI on 2026-09-07 established the intended project as Free tier, replacement key creation, and the current quota matrix. Approved-profile rows are: Gemini 2.5 Flash-Lite 10 RPM / 250,000 input TPM / 20 RPD; Gemini 3.5 Flash-Lite 15 RPM / 250,000 input TPM / 500 RPD; Gemini 2.5 Flash 5 RPM / 250,000 input TPM / 20 RPD. The same account surface exposes Gemini 3.1 Flash-Lite as an unapproved candidate at 15 RPM / 250,000 input TPM / 500 RPD. No provider-network access has been exercised.

The RPD evidence materially narrows this hypothesis: either approved 2.5 Free-tier route cannot guarantee completion of a maximum 12-turn reference run because three generation calls per accepted turn imply up to 36 generation requests, exceeding 20 RPD before unresolved `countTokens` RPD treatment is considered. The 500-RPD Flash-Lite rows do not have that single-run RPD blocker.

Verification trigger: immediately before any authorized real provider request and after any model-set amendment.

Falsifier: replacement-key association/type, model availability, quota, tier, region/project configuration, or provider account constraints do not satisfy the selected route.

### HYP-003 — Current pricing and data-use state

Hypothesis: the intended account/tier pricing, quota, and data-use terms remain compatible with the approved synthetic-only E0-A run boundary at execution time.

Current evidence: provider-reference snapshots and Director-supplied Free-tier account status dated 2026-09-07. The approved Free-tier route remains synthetic-fixture-only because current unpaid-service terms permit provider product/model improvement use of submitted content.

Verification trigger: immediately before any authorized provider-network execution or spend and whenever the short-lived provider snapshot expires or materially changes.

Falsifier: current provider terms, billing, retention/data-use, pricing, or quota differ materially from the assumptions required by the approved run.

### HYP-004 — Live provider usage/accounting mapping

Hypothesis: real Gemini response usage fields, reasoning/thinking-token reporting, cache reporting, and terminal stream behavior map to the Harness accounting and fail-closed rules as expected for both 2.5 budget-controlled profiles and the 3.5 minimal-thinking profile.

Current evidence: deterministic/fake regression coverage only. No current comparison profile has provider-network usage evidence.

Verification trigger: first separately authorized real run of each materially distinct provider behavior family.

Falsifier: observed provider responses cannot be reconciled with the approved accounting/provenance rules without changing runtime semantics.

### HYP-005 — `countTokens` quota accounting

Hypothesis: Google may or may not charge `models.countTokens(generateContentRequest)` against the same project request-rate and/or daily request buckets used by generation; current authoritative provider documentation does not establish an exemption or shared-bucket rule strongly enough for the E0-A gate.

Current evidence: no live quota experiment and no authoritative provider statement resolving the relationship.

Current engineering disposition: the Harness conservatively rate-paces every Gemini API-bound operation, including `countTokens`, while applying exact rolling input-TPM discipline to generation from the preceding token count. This is a fail-closed implementation choice, not evidence that Google actually counts `countTokens` against generation RPM or RPD.

Verification trigger: authoritative provider clarification or separately authorized live evidence that can distinguish the quota behavior without weakening other E0-A controls.

Falsifier/narrowing evidence: authoritative or empirical evidence establishes that `countTokens` is governed by a distinct/exempt quota surface. Any resulting pacing relaxation requires a separate audited amendment; it must not be inferred automatically.

## Closed or deferred matters that are not hypotheses

The following are decisions or scope gates and must not be reclassified as assumptions: Gemini remains the sole current E0-A provider method; historical OpenAI executable support is retired; the three named arm/profile pairings remain the currently approved implementation set until amended; **no real comparison run is currently authorized**; Free-tier live experiments are synthetic-fixture-only; .NET 10 requires later Director approval; GitHub plan-dependent branch protection is deferred; Stage remains design-lane authority.
