# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded Gemini provider-error diagnostic is **CLOSED — MACHINE-VALIDATED**. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`.

## Validation
Promoted native executable: `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Director Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits do not inherit native authority.

## Provider
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed. Exact rejected field remains unproven.

Director reconfirmed 2026-09-09 that `gemini-3.5-flash-lite` remains **15 RPM / 250,000 input TPM / 500 RPD** and directed reuse until a material contrary signal. Decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. This supersedes only per-run quota-limit rereads; pricing/data-use freshness and route-change/error triggers remain binding.

**3.5 Flash-Lite PAUSED pending exact Attempt 04 authorization; 3.1 Flash-Lite DEFERRED. Provider authorization: NONE.**

## Parallel E0-E
Q-E0E-PREP is **CLOSED — HOSTED NON-NETWORK PREPARATION VALIDATED**. Contract: `docs/blueprint/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CONTRACT.md`. Closeout: `docs/evidence/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CLOSEOUT_AUDIT_2026_09_09.md`. Q-E0E-RUN remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.

## Cross-lane
Q-DESIGN-09 DONE; Q-DESIGN-10 ACTIVE. Engineering authority/order unchanged.

## Next
No active source implementation branch. Q-E0A-02 remains **BLOCKED only on exact Director provider-action authorization and applicable execution preconditions**; the current 3.5 quota prerequisite is satisfied until a listed trigger occurs. Fresh-chat transition: `docs/handoff/E0A_PROVIDER_COMPATIBILITY_BLOCKED_HANDOFF_2026_09_09.md`. No E0-E execution is authorized.
