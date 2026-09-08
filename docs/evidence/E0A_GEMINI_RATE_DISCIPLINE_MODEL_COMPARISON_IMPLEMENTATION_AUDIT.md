# E0-A Gemini Rate Discipline / Model Comparison — Implementation Audit

Status: **IMPLEMENTED / CLOUD COMPILER GATE PASS / NATIVE WINDOWS ARM64 VALIDATION PENDING / REAL PROVIDER EXECUTION NOT AUTHORIZED**

Recorded: 2026-09-07

## Authority

Approved architecture:

- `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md`
- architecture commit: `76fc0c64da4724a7a352a656a062d5c3ed431ad6`

Implementation branch:

- `e0a-gemini-rate-discipline-model-comparison`

Audited executable/test checkpoint:

- `277dcb5e3bdd99ef02df4bd91d3df309d4c707ef`

Baseline:

- `main` checkpoint `7490de24bfd2a9829f6afc1ae4b3831c98c50837`

This evidence does not change validation authority by itself. `CURRENT_STATE.md` remains the only active checkpoint/validation/next-action authority.

## Implemented boundary

The Harness now exposes three explicit comparison profiles:

```text
GEMINI-2.5-FLASH-LITE-NONE
  arm: CREATIVE-NONE
  model: gemini-2.5-flash-lite
  active Free-tier snapshot: 10 RPM / 250,000 input TPM
  Performer/Interpreter: thinkingBudget=0
  Integrity: thinkingBudget=3584

GEMINI-3.5-FLASH-LITE-MINIMAL
  arm: CREATIVE-MINIMAL
  model: gemini-3.5-flash-lite
  active Free-tier snapshot: 15 RPM / 250,000 input TPM
  Performer/Interpreter: thinkingLevel=minimal
  Integrity: thinkingLevel=high

GEMINI-2.5-FLASH-NONE
  arm: CREATIVE-NONE
  model: gemini-2.5-flash
  active Free-tier snapshot: 5 RPM / 250,000 input TPM
  Performer/Interpreter: thinkingBudget=0
  Integrity: thinkingBudget=3584
```

`CREATIVE-MINIMAL` remains distinct from true thinking-off. The implementation does not treat Gemini 3.5 Flash-Lite minimal thinking as zero thinking.

The live host requires an explicit arm/profile pairing before credential access. Unapproved or mismatched pairings fail closed.

## Rate discipline

The live Gemini path now injects one cancellation-aware rate-discipline boundary into the run driver.

For every Gemini API-bound operation:

- requests are smoothly spaced from the selected profile's RPM;
- `countTokens` is conservatively included because current provider evidence does not establish that it is exempt from request-rate accounting;
- generation additionally uses the exact preceding `countTokens` result to enforce a rolling 60-second input-TPM window;
- one generation whose exact input exceeds the selected profile TPM fails before inference;
- no retries or provider backoff retries were added;
- a provider `429` remains a technical/noncontributing terminal outcome;
- the existing linked 300-second role-attempt deadline covers pacing, token preflight, and inference.

The limiter clock/delay surface is injectable for deterministic tests. Live timing uses `Stopwatch` monotonic time and cancellation-aware `Task.Delay`.

## Output, reasoning, and spend controls

All profiles retain the provider-neutral 4,096 observed generated-token ceiling per role invocation.

- 2.5 creative roles still require zero reported reasoning tokens.
- 3.5 minimal creative roles may report reasoning tokens only inside the explicit `CREATIVE-MINIMAL` arm and only while combined observed generated usage remains within 4,096.
- implicit cached input and cache-write contribution remain terminal technical violations.
- 2.5 Performer/Interpreter reserve 4,096 output tokens; 2.5 Integrity reserves the 65,536 published model output limit.
- all 3.5 roles conservatively reserve 65,536 because minimal thinking is not guaranteed to be zero.
- reservations remain per-call and are immediately reconciled, not cumulatively held as simultaneous maxima.

Route-specific shadow pricing is recorded in run evidence. The USD 5 run ceiling is unchanged.

## Evidence and latency instrumentation

Run manifests now bind:

- exact provider-profile ID;
- requested model;
- thinking-control kind and configured budget/level;
- current RPM / input TPM snapshot;
- RPD as `unverified-pre-live` rather than an invented value;
- route-specific paid reference pricing;
- all-request rate-discipline scope.

Runtime events now capture:

- `countTokens` preflight elapsed time;
- generation-pacing elapsed time;
- provider-generation elapsed time;
- end-to-end role elapsed time;
- committed-turn elapsed time.

This is sufficient to compare quality together with user-perceived speed after separately authorized live runs.

## Scope audit

`git compare 7490de24bfd2a9829f6afc1ae4b3831c98c50837...277dcb5e3bdd99ef02df4bd91d3df309d4c707ef` reports:

- ahead: 16;
- behind: 0;
- merge base: exact baseline `7490de24...`;
- changed source is confined to `src/Ensemble.E0.Harness/...`;
- changed tests are confined to `tests/Ensemble.E0.Harness.Tests/...`;
- one approved blueprint was added;
- **no `src/Ensemble.E0.Core/...` file changed**;
- **no Core-test file changed**;
- **no fixture file changed**.

Therefore the patch does not alter deterministic H1/Core semantics or the frozen Missing Raft fixture.

## Recursive audit corrections incorporated

The implementation audit found and corrected two material design risks before this checkpoint:

1. The rate boundary was placed in the run driver, which already owns the exact `countTokens -> generation` sequence, instead of entangling Gemini wire parsing with policy.
2. Initial TPM smoothing was strengthened to an exact rolling 60-second generation-input token window. This avoids relying on a long-run average that could still violate a strict provider minute window.

Regression coverage includes the rolling-window case: a 200,000-token generation followed by a 100,000-token generation on a 250,000-TPM profile waits until the first usage exits the 60-second window.

## Cloud Validation gate

Workflow run:

- `34177033966`
- head: `277dcb5e3bdd99ef02df4bd91d3df309d4c707ef`
- conclusion: **SUCCESS**

Passed jobs:

- Compiler gate (ARM64 cross-compile): PASS
  - Core project build
  - Harness project build
  - Core test project build
  - Harness test project build
- Core tests (x64 regression, non-authoritative): PASS
- Repository law enforcement: PASS
- Oracle assertion coverage: PASS
- Document authority census: PASS

Validation classification: **cloud compiler/static gate plus non-authoritative x64 Core regression**.

The workflow does **not** execute Harness tests and does not run on native Windows ARM64. It is not native machine validation.

## Still unvalidated

At this checkpoint the following are explicitly **NOT PERFORMED / NOT PROVEN**:

- native Windows ARM64 execution at `277dcb5e...`;
- Harness test execution at this checkpoint;
- fresh native `win-arm64` build/smokes at this checkpoint;
- credentialless refusal for each of the three explicit arm/profile pairings;
- `countTokens` provider-network compatibility;
- Gemini 2.5 Flash-Lite live request/response compatibility;
- Gemini 3.5 Flash-Lite live request/response compatibility;
- live usage/reasoning accounting for either model family;
- live quota behavior;
- real provider inference;
- provider spend.

The previous promoted native authority remains `cc395a25162a0a682796bffb44060c799df0db32`; it validates the pre-comparison Gemini route only and must not be projected onto this new source checkpoint.

## Next validation gate

Director Windows ARM64 native validation must occur on an exact clean checkout of the final implementation/documentation branch head before any real provider request is authorized.

The native packet must include:

- architecture/runtime probe;
- Core tests;
- Harness tests;
- fresh native Harness build;
- Missing Raft and generic fixture smokes;
- credentialless refusal, before evidence creation, for all three approved arm/profile pairings.

No Gemini key is required for this validation and no provider-network request is authorized by it.
