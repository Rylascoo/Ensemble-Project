# Ensemble Hypothesis Ledger

Status: unverified-assumption register. Hypotheses are not decisions, phase authority, provider admission, or validation evidence.

A hypothesis belongs here when engineering work depends on a fact that has not yet been established at the required evidence level. Resolved hypotheses should record the disposition and supporting evidence rather than silently disappear.

## Open hypotheses

### HYP-001 — Live Gemini request compatibility

Hypothesis: the currently approved `gemini-2.5-flash` REST request/streaming shapes used by the E0-A Harness remain accepted by the real Google Gemini API.

Current evidence: compiler, fake/provider-simulation tests, and credentialless native execution only. No real Gemini request has been made by the current integration.

Verification trigger: immediately before the first separately authorized provider-network run.

Falsifier: the real provider rejects or semantically contradicts the approved request/stream contract.

### HYP-002 — Account/key/model/quota availability

Hypothesis: the Director-selected project/account/key can access the intended model and required `countTokens` / generation routes within the approved E0-A envelope.

Current evidence: none at provider-network level.

Verification trigger: immediately before any authorized real provider request.

Falsifier: key type, model availability, quota, tier, region/project configuration, or provider account constraints do not satisfy the route.

### HYP-003 — Current pricing and data-use state

Hypothesis: the intended account/tier pricing, quota, and data-use terms are compatible with the approved E0-A run boundary at execution time.

Current evidence: dated provider-reference snapshots only; those do not prove current account state.

Verification trigger: immediately before any authorized provider-network execution or spend.

Falsifier: current provider terms, billing, retention/data-use, or quota differ materially from the assumptions required by the approved run.

### HYP-004 — Live provider usage/accounting mapping

Hypothesis: real Gemini response usage fields, thinking-token reporting, cache reporting, and terminal stream behavior map to the Harness accounting and fail-closed rules as expected.

Current evidence: deterministic/fake regression coverage only.

Verification trigger: first separately authorized real Gemini reference run.

Falsifier: observed provider responses cannot be reconciled with the approved accounting/provenance rules without changing runtime semantics.

## Closed or deferred matters that are not hypotheses

The following are decisions or scope gates and must not be reclassified as assumptions: Gemini is the sole current E0-A provider method; historical OpenAI executable support is retired; only `CREATIVE-NONE` is authorized on the current Gemini live path; real provider credentials/network/inference/spend require a separate Director gate; .NET 10 requires later Director approval; GitHub plan-dependent branch protection is deferred; Stage remains design-lane authority.
