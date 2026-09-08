# E0-A Phase B — Gemini Rate Discipline and Model Comparison Amendment

Status: **APPROVED — DIRECTOR 2026-09-07; IMPLEMENTATION AUTHORIZED; REAL GEMINI NETWORK EXECUTION NOT AUTHORIZED**

Parent authority:

- `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md` Proposal 0.15;
- `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`.

This amendment is narrow. It adds provider-rate discipline and a controlled three-model comparison plan. It does not alter deterministic H1/Core authority, fixture semantics, causal authority, State Authority, Take/commit law, blind-review isolation, accepted-turn cap, one-attempt law, zero-retry law, cancellation semantics, hard gates, or later-phase exclusions.

## 1. Director decision

The first-real-provider program must optimize **quality and user-perceived speed together**, not price or model age in isolation. The Harness therefore gains three explicit Gemini comparison profiles:

```text
GEMINI-2.5-FLASH-LITE-NONE
  model       gemini-2.5-flash-lite
  arm         CREATIVE-NONE
  Performer   thinkingBudget=0
  Integrity   thinkingBudget=3584
  Interpreter thinkingBudget=0
  purpose     primary strict-contract speed/cost candidate

GEMINI-3.5-FLASH-LITE-MINIMAL
  model       gemini-3.5-flash-lite
  arm         CREATIVE-MINIMAL
  Performer   thinkingLevel=minimal
  Integrity   thinkingLevel=high
  Interpreter thinkingLevel=minimal
  purpose     required next-generation speed/quality comparison

GEMINI-2.5-FLASH-NONE
  model       gemini-2.5-flash
  arm         CREATIVE-NONE
  Performer   thinkingBudget=0
  Integrity   thinkingBudget=3584
  Interpreter thinkingBudget=0
  purpose     quality/reference anchor
```

`CREATIVE-MINIMAL` is deliberately distinct from `CREATIVE-NONE`. Google does not guarantee literal zero thinking for Gemini 3.5 Flash-Lite at `thinkingLevel=minimal`; the experiment must not relabel that behavior as true thinking-off.

No profile is Production-provider admission. No real provider call is authorized by this amendment.

## 2. Account-specific rate snapshot

AI Studio account evidence supplied by the Director on 2026-09-07 established the active Free-tier project limits:

```text
gemini-2.5-flash-lite   10 RPM   250,000 input TPM
gemini-3.5-flash-lite   15 RPM   250,000 input TPM
gemini-2.5-flash         5 RPM   250,000 input TPM
```

The project UI did not surface a usable RPD value in the captured evidence. The Harness must not invent one. RPD and all volatile quota facts remain a pre-live re-verification item.

Rate limits are project-scoped. The current Free-tier route remains synthetic-fixture-only.

## 3. Rate-discipline law

The live Harness must never depend on burst luck or retries to remain inside the admitted account envelope.

Before every Gemini API-bound HTTP request, including `countTokens` and generation, the provider edge applies deterministic smooth request pacing derived from the selected profile's verified RPM. This conservative scope is intentional because current provider documentation does not establish that `countTokens` is exempt from the project's request-rate accounting.

Generation additionally applies input-token throughput discipline using the exact preceding `countTokens` result and the selected profile's verified input TPM.

Implementation requirements:

- pacing exists only at the Gemini Harness/provider edge;
- no Core dependency or Core change;
- no automatic retries or provider backoff retries;
- a provider `429` remains a technical/noncontributing terminal result;
- request pacing is cancellation-aware;
- the clock/delay boundary is injectable so tests are deterministic and do not sleep;
- live pacing uses a monotonic time source;
- the 300-second role-attempt timeout remains authoritative and includes preflight/pacing/provider time;
- evidence records the selected RPM/TPM snapshot and pacing policy.

The initial conservative implementation paces all Gemini HTTP operations. Later live evidence may justify a narrower `countTokens` treatment only through a separate audited amendment.

## 4. Output/thinking law across profiles

All profiles retain the provider-neutral intended generated-token ceiling of **4,096 observed tokens per role invocation**, where observed generated output is candidate tokens plus thought tokens.

For all profiles:

- `maxOutputTokens=4096` remains the request candidate limit;
- candidate output above 4,096 is technical/noncontributing;
- observed candidate + thought total above 4,096 is technical/noncontributing;
- nonzero cached input or cache-write contribution is technical/noncontributing;
- thought summaries/signatures are not requested, persisted, or semantically consumed.

For Gemini 2.5 profiles, Performer and Interpreter must report zero reasoning tokens. Integrity may reason within the existing observed-total law.

For Gemini 3.5 Flash-Lite, `minimal` does not guarantee zero reasoning. Nonzero Performer/Interpreter reasoning is therefore allowed only for the explicit `CREATIVE-MINIMAL` comparison arm and remains counted inside the 4,096 observed generated-token ceiling.

## 5. Conservative spend reservation

The USD 5 shadow ceiling remains unchanged.

For Gemini 2.5 profiles:

- Performer/Interpreter reserve against the configured 4,096 candidate ceiling;
- Integrity reserves against the model's published 65,536 output limit because `thinkingBudget` is advisory.

For Gemini 3.5 Flash-Lite, every role conservatively reserves against the published 65,536 output limit because `minimal` can still produce thinking and the provider documentation does not establish a hard combined candidate+thought cap from `maxOutputTokens`.

Reservations remain per-call and are reconciled immediately against observed usage; maximum reservations are not accumulated as if all calls were simultaneously outstanding.

## 6. Pricing snapshot

Provider pricing was reverified for this amendment on 2026-09-07. Free-tier billed cost is expected to be USD 0, while the deterministic shadow ceiling uses the selected model's current Standard paid rates:

```text
gemini-2.5-flash-lite
  input          $0.10 / 1M tokens
  cached input   $0.01 / 1M tokens
  output         $0.40 / 1M tokens including thinking

gemini-3.5-flash-lite
  input          $0.30 / 1M tokens
  cached input   $0.03 / 1M tokens
  output         $2.50 / 1M tokens including thinking

gemini-2.5-flash
  input          $0.30 / 1M tokens
  cached input   $0.03 / 1M tokens
  output         $2.50 / 1M tokens including thinking
```

As before, cached input is conservatively shadow-costed at the full uncached input rate. The snapshot is short-lived and fail-closed; material provider change reopens it immediately.

## 7. Model limits and transport

All three selected model profiles currently use the same published model limits frozen for this experimental snapshot:

```text
input token limit    1,048,576
output token limit      65,536
```

Transport remains native Gemini `generateContent` / `streamGenerateContent` plus exact `models.countTokens(generateContentRequest)`, `GEMINI_API_KEY`, `store=false`, no tools, no provider conversation state, structured JSON output, standard inference tier, and one candidate.

Gemini 2.5 request bodies use `thinkingBudget`. Gemini 3.5 request bodies use `thinkingLevel`. A request must never send both controls.

## 8. Explicit live profile selection

A live run must identify both the experiment arm and provider profile explicitly. No live call may silently inherit a model default.

Approved pairings are:

```text
CREATIVE-NONE    GEMINI-2.5-FLASH-LITE-NONE
CREATIVE-NONE    GEMINI-2.5-FLASH-NONE
CREATIVE-MINIMAL GEMINI-3.5-FLASH-LITE-MINIMAL
```

All other arm/profile pairings fail closed before credential access or evidence creation.

## 9. Comparison evidence

The same frozen Missing Raft fixture and deterministic authority path are used for all comparison runs. Evidence must make model/profile identity explicit and collect enough timing/resource provenance to compare:

- contract/structured-output success;
- accepted-turn yield and terminal status;
- blind narrative/character quality under the existing hard-gate process;
- `countTokens` preflight latency;
- provider-role latency;
- total committed-turn wall time;
- observed accepted turns per minute;
- input/output/reasoning tokens;
- shadow cost per accepted turn and per run.

The comparison selects the **quality/speed frontier**. Cheapest, newest, or highest-RPM does not win automatically.

## 10. Validation and live gate

Implementation is authorized in the smallest Harness/test/evidence/documentation surface needed for this amendment.

Before any real provider request:

1. fake/unit tests for profile selection, request shape, rate discipline, usage policy, spend reservation, evidence identity, and timing must pass;
2. Core must remain unchanged;
3. native Windows ARM64 Core/Harness tests and fresh Harness build must pass;
4. credentialless explicit live-profile invocations must fail before evidence creation with the expected `GEMINI_API_KEY` refusal;
5. replacement Auth key/project association, Free-tier status, model availability, RPM/TPM/RPD, pricing, and data-use terms must be reverified;
6. the Director must separately authorize the exact first real profile/run.

This amendment does **not** authorize `countTokens`, provider network execution, inference, or spend.

## 11. Current preferred test order

Subject to the validation gate above, the planned comparison order is:

```text
1. GEMINI-2.5-FLASH-LITE-NONE
2. GEMINI-3.5-FLASH-LITE-MINIMAL
3. GEMINI-2.5-FLASH-NONE
```

The order reflects product goals: first test the strict zero-thinking profile with materially better throughput, then the highest-RPM next-generation comparison, then use Gemini 2.5 Flash as the quality/reference anchor. A failed or poor-quality earlier profile does not authorize automatic fallback execution; every real run remains separately Director-authorized.
