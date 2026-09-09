# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog without overriding this state.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Repository convergence/continuity cleanup are **CLOSED**. Q-E0A-01 bounded Gemini provider-error/request-compatibility diagnostic is **CLOSED — MACHINE-VALIDATED**. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`. Gemini 3 signature support remains current: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
**Promoted native executable:** `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` / `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64` / tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Director Windows ARM64: Core **622/622**, Harness **130/130**, fresh build, fixture smokes and credentialless gates **PASS**. Evidence: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Native attempt 01 at `20f76e7d...` failed 128/130 Harness because two stale tests expected the old generic exception; the correction was test-only. Later documentation commits do not inherit native runtime authority.

## Provider decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed as the failure explanation. Exact rejected field remains unproven.

**3.5 Flash-Lite live execution PAUSED; 3.1 Flash-Lite execution DEFERRED. Provider authorization: NONE.** Validation does not authorize credential use or provider traffic.

## Parallel E0-E
**Single-model playwright control preparation remains AUTHORIZED; execution remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.** Non-network only. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`.

## Cross-lane continuity
The Q-DESIGN-06 TYP-01 preflight closure and Q-DESIGN-07 design-audit activation recorded in `docs/PROJECT_EXECUTION_QUEUE.md` are design-lane sequencing only; they change no engineering checkpoint, validation rung, provider authorization, or E0 ordering.

## Next
Q-E0A-02 is **BLOCKED**. Before any further live provider compatibility request: reverify current provider/account/pricing/quota facts and obtain explicit Director authorization. Q-E0E-PREP may continue only within its existing non-network preparation boundary. Active E0-A transition record: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`.
