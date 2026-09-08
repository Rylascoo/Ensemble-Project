# E0-A Post-E-R1 Fresh-Chat Handoff — Gemini Comparison Native / Live Gates

Status: ACTIVE FRESH-CHAT TRANSITION
Recorded: 2026-09-07
Authority: `CURRENT_STATE.md` names this exact path. This handoff is temporary and cannot advance phase, validation, product, or provider authority by itself.

## Fresh-chat first action

1. Read `CURRENT_STATE.md` first.
2. Resolve current `main`, active work branch/head, relationship to `main`, and latest Validation gate. Do not assume the snapshot below remains current.
3. Then read:
   - `docs/PROJECT_AUTHORITY.md`
   - `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`
   - `docs/VALIDATION_LEDGER.md`
   - `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`
   - `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`
   - `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md`
   - `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`
   - `docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md`
   - `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`
4. Treat repository state, not prior-chat prose, as authoritative.

## Closed historical boundary

E-R1 repository/tooling/workflow restructuring is CLOSED / PROMOTED / ARCHIVED and must not be reopened.

- PR #46 merge: `21a10aff823734418f36284744a1fd26aef3bcf6`.
- PR #47 continuity merge: `139621dc63e1a67aca5f1a5eedb7c1347ede9fd5`.
- Archived E-R1 branch head: `bec3db07cef41296855be8f702d2f3637c472dc1`, preserved by `archive/repo-restructure-e-r1`.
- Promoted native authority remains `cc395a25162a0a682796bffb44060c799df0db32` under `validation/e0a-pre-restructure-closure-native-arm64`.

Do not confuse that historical native checkpoint with validation of the new comparison Harness source.

## Current comparison work

Work branch:

```text
e0a-gemini-rate-discipline-model-comparison
```

Approved architecture commit:

```text
76fc0c64da4724a7a352a656a062d5c3ed431ad6
```

Audited executable/test checkpoint:

```text
277dcb5e3bdd99ef02df4bd91d3df309d4c707ef
```

Cloud Validation run `34177033966` passed all five jobs at that executable/test checkpoint, including ARM64 cross-compilation of the Harness and Harness tests. This does not execute Harness tests and is not native Windows ARM64 evidence.

No Core source, Core-test, or fixture file changed from `main` through that checkpoint.

## Provider/account evidence

Director-supplied AI Studio evidence established Free-tier project limits on 2026-09-07:

```text
gemini-2.5-flash-lite   10 RPM   250,000 input TPM
gemini-3.5-flash-lite   15 RPM   250,000 input TPM
gemini-2.5-flash         5 RPM   250,000 input TPM
```

RPD was not established; do not invent it.

The previously exposed API key was replaced. Never request or record the replacement key value. Replacement-key availability is not live-run authorization.

Free-tier provider execution remains synthetic-fixture-only.

## Approved comparison matrix

```text
CREATIVE-NONE
  GEMINI-2.5-FLASH-LITE-NONE
  gemini-2.5-flash-lite
  Performer/Interpreter thinkingBudget=0
  Integrity thinkingBudget=3584

CREATIVE-MINIMAL
  GEMINI-3.5-FLASH-LITE-MINIMAL
  gemini-3.5-flash-lite
  Performer/Interpreter thinkingLevel=minimal
  Integrity thinkingLevel=high

CREATIVE-NONE
  GEMINI-2.5-FLASH-NONE
  gemini-2.5-flash
  Performer/Interpreter thinkingBudget=0
  Integrity thinkingBudget=3584
```

`CREATIVE-MINIMAL` must never be described as literal thinking-off.

Planned live comparison order, each requiring separate Director authorization after native validation:

```text
1. GEMINI-2.5-FLASH-LITE-NONE
2. GEMINI-3.5-FLASH-LITE-MINIMAL
3. GEMINI-2.5-FLASH-NONE
```

## Implemented rate discipline

The live path conservatively paces every Gemini API-bound operation, including `countTokens`, because provider evidence has not shown `countTokens` to be RPM-exempt.

Generation additionally enforces an exact rolling 60-second input-TPM window from the preceding exact token count. No retries were added. Provider `429` remains a technical/noncontributing terminal outcome. Existing 300-second per-role attempt deadline remains authoritative and includes pacing/preflight/provider work.

Because an accepted turn currently contains three sequential roles and each role contains a `countTokens` plus generation request, quota and real model latency must be measured as product-quality variables rather than treated as incidental infrastructure.

## Current consequential gate — native validation

**Do not present a real-run authorization gate yet.** The next gate is Director Windows ARM64 validation of one exact clean checkout of the active comparison branch.

Required native validation:

1. prove native Windows ARM64 host identity using the established host-behavior method;
2. run Core tests;
3. run Harness tests;
4. perform a fresh native `win-arm64` Harness build;
5. run Missing Raft and generic fixture smokes;
6. with `GEMINI_API_KEY` absent, invoke all three approved arm/profile live paths and prove each fails before evidence creation with:

```text
GEMINI_API_KEY is required at the E0-A provider edge.
```

Credentialless commands use the new live syntax:

```text
e0a-run <CREATIVE-NONE|CREATIVE-MINIMAL> <provider-profile> <fixture.json> <run-id> <evidence-root> <executable-commit>
```

Use three distinct nonexistent evidence roots and run IDs. The executable-commit argument must be the exact checked-out HEAD. Preserve expected-failure stderr and exit codes according to `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

This gate requires no Gemini key and must perform no `countTokens`, network request, inference, or spend.

## Later live gate

Only after native validation is recorded and recursively audited may the project return to a separate Director decision for one exact real profile/run.

Before any such run, freshly reverify:

- intended project and replacement Auth key association without exposing the credential;
- Free-tier/billing status;
- selected model availability;
- RPM / input TPM / RPD;
- pricing;
- data-use terms;
- snapshot freshness;
- evidence destination and checkout identity.

Do not call Gemini, invoke provider `countTokens`, perform inference, or incur spend without explicit live-run authorization in that chat.

## Continuity discipline

Do not reopen E-R1. Do not reintroduce OpenAI executable support. Do not enter E0-B+, app/UI/persistence, Windows AI/NPU, packaging/WACK/Store, or design-lane work from this handoff.

Perform recursive correctness/consistency/authority/scope/tests/simplicity/hygiene/ARM64/evidence review before any consequential reply. Once this handoff's transition purpose is fully served, remove it from the active tree and fold durable continuity into `CURRENT_STATE.md`.
