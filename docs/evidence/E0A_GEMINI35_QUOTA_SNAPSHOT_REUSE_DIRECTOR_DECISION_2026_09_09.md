# E0-A Gemini 3.5 Flash-Lite Quota Snapshot Reuse — Director Decision

Status: **APPROVED — DIRECTOR 2026-09-09 — QUOTA SNAPSHOT REUSE AUTHORIZED — PROVIDER TRAFFIC NOT AUTHORIZED**

## Authority

This decision records the Director's 2026-09-09 instruction that the current AI Studio quota values for the exact E0-A `gemini-3.5-flash-lite` Free-tier route remain unchanged and should be used until a material contrary signal appears.

It is subordinate to `CURRENT_STATE.md` and the frozen E0-A architecture. It supersedes only the per-run external quota-limit recheck cadence in Section 2 of `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` for this exact route. It does not authorize provider traffic or change any executable, fixture, retry, spend, privacy, evidence, or experiment-order law.

## Director-confirmed working quota snapshot

For the intended AI Studio Free-tier project/model route:

```text
gemini-3.5-flash-lite
RPM        15
input TPM  250,000
RPD        500
```

The Director confirmed on 2026-09-09 that these values remain the same as the previously supplied 2026-09-07 AI Studio snapshot.

The frozen full 12-turn E0-A envelope has a conservative upper bound of 72 provider operations, so the static RPD admission calculation remains `72 <= 500`.

## Reuse rule

A separate manual AI Studio quota-limit read is not required before every later separately authorized `gemini-3.5-flash-lite` E0-A run while this exact route remains materially unchanged.

The snapshot must be reverified before any later provider authorization if any of the following occurs:

- provider HTTP `429` / `RESOURCE_EXHAUSTED` or another quota/rate-limit signal;
- AI Studio shows different RPM, TPM, RPD, tier, or availability;
- project, account, billing tier, API key association, region/routing path, or selected model changes;
- Google announces or documents a material rate-limit or model-availability change;
- known unrelated project traffic materially changes available capacity;
- credible evidence conflicts with the recorded values.

A quota-triggered failure remains terminal for that authorization. It does not permit retry, fallback, or silent pacing changes.

## Boundaries not superseded

This decision does **not** override:

- `E0AGeminiPricingPolicy.SnapshotValidThrough = 2026-09-14` or its fail-closed pricing/data-use freshness guard;
- material provider pricing, terms, data-use, or lifecycle change triggers;
- the synthetic Missing Raft-only Free-tier boundary;
- the exact machine-tested executable requirement;
- one attempt per role invocation / zero automatic retries / no fallback;
- the $5 shadow-spend ceiling and immutable evidence laws;
- separate explicit Director authorization for each real provider execution.

Public provider facts were rechecked on 2026-09-09 against current Google Gemini documentation: active limits are project-specific and exposed in AI Studio; `gemini-3.5-flash-lite` has no announced shutdown; current Standard Free Tier remains free of charge and marks submitted content as used to improve Google products. Those public facts do not convert the Free-tier route into a production-admitted provider.

## Q-E0A-02 consequence

The quota-limit/account-value prerequisite for the current `gemini-3.5-flash-lite` route is satisfied under this Director decision until a trigger above occurs. Q-E0A-02 remains blocked only on the remaining exact provider-action authorization and any still-applicable non-quota execution preconditions.

Provider authorization remains **NONE**. No `countTokens`, generation/inference, credential use, spend, probe, retry, fallback, or other provider request is authorized by this decision.
