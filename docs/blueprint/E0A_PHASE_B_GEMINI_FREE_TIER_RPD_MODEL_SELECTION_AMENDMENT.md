# E0-A Phase B — Gemini Free-Tier RPD Model-Selection Amendment

Status: **APPROVED — DIRECTOR 2026-09-07; IMPLEMENTATION AUTHORIZED; REAL GEMINI NETWORK EXECUTION NOT AUTHORIZED**

Parent authority:

- `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`;
- `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`;
- `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md`.

This amendment supersedes the prior comparison amendment only where model membership, per-profile run cap, RPD evidence, pricing, and comparison order conflict. All deterministic Core authority, one-attempt/zero-retry law, role order, hard gates, $5 shadow ceiling, synthetic-only Free-tier boundary, RPM/TPM discipline, 300-second role timeout, evidence immutability, and later-phase exclusions remain unchanged.

## 1. Trigger and Director decision

Director-supplied AI Studio quota evidence on 2026-09-07 established the intended Free-tier project limits:

```text
gemini-3.5-flash-lite   15 RPM   250,000 input TPM   500 RPD
gemini-3.1-flash-lite   15 RPM   250,000 input TPM   500 RPD
gemini-2.5-flash-lite   10 RPM   250,000 input TPM    20 RPD
gemini-2.5-flash         5 RPM   250,000 input TPM    20 RPD
```

A successful accepted turn currently performs three sequential model roles. Every role has one exact `countTokens` preflight followed by at most one generation request. Therefore the conservative provider-operation upper bound is **six Gemini API-bound requests per accepted turn**, of which three are generation requests.

A 12-turn run can therefore require 72 API-bound requests / 36 generation requests. A 20-RPD route cannot guarantee the approved full reference envelope even if `countTokens` consumes no RPD. The first-real-provider plan must change before any provider request.

The approved live comparison set is now:

```text
GEMINI-3.5-FLASH-LITE-MINIMAL
  model       gemini-3.5-flash-lite
  arm         CREATIVE-MINIMAL
  turn cap    12
  quota       15 RPM / 250K input TPM / 500 RPD
  purpose     primary full-reference quality/speed candidate

GEMINI-3.1-FLASH-LITE-MINIMAL
  model       gemini-3.1-flash-lite
  arm         CREATIVE-MINIMAL
  turn cap    12
  quota       15 RPM / 250K input TPM / 500 RPD
  purpose     full-reference efficiency/cost comparator

GEMINI-2.5-FLASH-LITE-NONE
  model       gemini-2.5-flash-lite
  arm         CREATIVE-NONE
  turn cap    3
  quota       10 RPM / 250K input TPM / 20 RPD
  purpose     exact-no-thinking control only
```

`GEMINI-2.5-FLASH-NONE` is removed from the live-selectable Free-tier comparison. It may remain an internal historical compatibility anchor for tests that preserve the original normative route, but it must not be selectable by the live comparison CLI.

## 2. Conservative RPD admission law

E0-A cannot infer other project traffic or remaining daily quota from local state. RPD therefore has two layers:

1. **Static profile admissibility.** A live-selectable profile must have enough published RPD to cover its entire local worst-case run assuming every preflight and generation request could consume the same daily bucket.
2. **Immediate external re-verification.** Before each separately authorized real run, the Director must reverify the selected project/model's current RPD and current-day usage/availability sufficiently to support that run.

For the current orchestration:

```text
provider operations per accepted turn = 3 roles × (1 countTokens + 1 generation) = 6

3.5 Flash-Lite: 12 × 6 = 72 <= 500 RPD
3.1 Flash-Lite: 12 × 6 = 72 <= 500 RPD
2.5 Flash-Lite:  3 × 6 = 18 <=  20 RPD
```

The 2.5 Flash-Lite control cap is therefore **three accepted turns**, not four: four turns could require 24 provider operations and would exceed the conservative 20-RPD envelope if `countTokens` shares RPD accounting.

A provider `429` remains terminal technical/noncontributing. No retry or automatic fallback is introduced.

## 3. Thinking and response-metadata law

Both Gemini 3 Flash-Lite routes use:

```text
Performer    thinkingLevel=minimal
Integrity    thinkingLevel=high
Interpreter  thinkingLevel=minimal
```

`CREATIVE-MINIMAL` remains distinct from `CREATIVE-NONE`: minimal thinking does not guarantee literal zero reasoning.

Gemini 2.5 Flash-Lite retains:

```text
Performer    thinkingBudget=0
Integrity    thinkingBudget=3584
Interpreter  thinkingBudget=0
```

and remains the exact-no-thinking control.

The existing Gemini 3 opaque `thoughtSignature` rule generalizes from 3.5 Flash-Lite to both approved Gemini 3 Flash-Lite models: opaque signatures may be accepted as provider metadata and stripped before diagnostic persistence and semantic output, while explicit returned thought material (`thought=true`) remains technical/noncontributing. E0-A remains stateless across role requests and does not replay provider response history or thought signatures.

## 4. Pricing and lifecycle snapshot

Provider pricing was reverified for this amendment on 2026-09-07. Free-tier billed input/output is expected to be USD 0; deterministic shadow accounting uses current Standard paid rates:

```text
gemini-3.5-flash-lite   input $0.30/M   cached input $0.03/M   output incl thinking $2.50/M
gemini-3.1-flash-lite   input $0.25/M   cached input $0.025/M  output incl thinking $1.50/M
gemini-2.5-flash-lite   input $0.10/M   cached input $0.01/M   output incl thinking $0.40/M
```

Cached input remains conservatively shadow-costed at the full uncached input rate. All three selected models use the frozen 1,048,576 input / 65,536 output limit snapshot.

Gemini 3.1 Flash-Lite is a stable comparison route but has an announced earliest shutdown date of **2027-05-07**, with Gemini 3.5 Flash-Lite as Google's recommended replacement. This makes 3.1 useful experimental evidence, not a presumption of long-term production dependence.

All provider facts remain short-lived and fail closed on material change.

## 5. Runtime/evidence requirements

Implementation must:

- carry RPD and per-profile accepted-turn cap in the model profile;
- expose the run-specific cap and RPD in immutable manifest evidence;
- enforce the profile cap in the run driver without changing the global 12-turn reference ceiling;
- prove live-selectable profile static admissibility against the six-request-per-turn conservative bound;
- keep the historical 2.5 Flash compatibility route non-live-selectable;
- admit opaque thought signatures for both explicit Gemini 3 Flash-Lite profiles only;
- preserve existing RPM/TPM pacing, exact token preflight, spend, usage, cancellation, and timeout laws;
- change no Core source, Core tests, or fixtures.

The Harness does **not** maintain a synthetic local RPD counter across processes because it cannot observe other project traffic. External remaining daily capacity is a pre-live gate, not fabricated runtime knowledge.

## 6. Validation gate

Any source change invalidates current machine authority for the modified executable. Before a real provider request, the amended executable must pass:

1. targeted fake/unit regressions for profile set, request shape, RPD/cap admissibility, 3-turn control termination, evidence identity, pricing, and Gemini 3 signature handling;
2. the repository cloud Validation gate;
3. native Windows ARM64 Core/Harness tests and fresh Harness build at one exact clean checkout;
4. credentialless explicit live-profile probes for all three amended profiles, proving missing-key refusal before evidence creation;
5. repository validation-tag law for the new exact native checkout;
6. a fresh external account/provider snapshot and separate Director authorization for exactly one real run.

No provider network call, `countTokens`, inference, credential use, or spend is authorized by this amendment.

## 7. Comparison order and decision criterion

Subject to the gates above, the separately authorized order is:

```text
1. GEMINI-3.5-FLASH-LITE-MINIMAL   full 12-turn reference
2. GEMINI-3.1-FLASH-LITE-MINIMAL   full 12-turn reference
3. GEMINI-2.5-FLASH-LITE-NONE      3-turn exact-no-thinking control
```

No later run is automatically authorized by an earlier result.

Selection remains the **quality × committed-turn-latency frontier**. Evidence must compare structured/contract success, accepted-turn yield, blind narrative/character quality, role and end-to-end latency, observed throughput, reasoning/input/output tokens, and shadow cost. Cheapest, newest, or highest nominal RPM does not win automatically.
