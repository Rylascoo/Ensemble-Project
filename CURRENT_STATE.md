# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded Gemini provider-error diagnostic is **CLOSED — MACHINE-VALIDATED**. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`. Gemini 3 signature: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native executable: `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Director Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits do not inherit native authority.

## Provider
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed. Exact rejected field remains unproven.

Director reconfirmed `gemini-3.5-flash-lite` at **15 RPM / 250,000 input TPM / 500 RPD** for reuse until a material contrary signal. Quota decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. Pricing/data-use freshness and route-change/error triggers remain binding.

**Attempt 04 AUTHORIZED — NOT YET CONSUMED.** Exactly one synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` E0-A run is authorized at `e6e7d6c8...`; one attempt/role, zero retries, no fallback or alternate traffic. Authorization: `docs/evidence/E0A_GEMINI35_ATTEMPT04_DIRECTOR_AUTHORIZATION_2026_09_09.md`. Provider authorization is limited to that exact run until consumed.

## Parallel E0-E
Q-E0E-PREP is **CLOSED — HOSTED NON-NETWORK PREPARATION VALIDATED**. Contract: `docs/blueprint/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CONTRACT.md`. Closeout: `docs/evidence/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CLOSEOUT_AUDIT_2026_09_09.md`. Q-E0E-RUN remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.

## Cross-lane
Q-DESIGN-09 DONE; Q-DESIGN-10 ACTIVE. Engineering authority/order unchanged.

## Next
Q-E0A-02 is **ACTIVE only for the exact unconsumed Attempt 04 authorization**. Execute from a new linked Windows ARM64 worktree pinned to the validation tag; preserve historical/native-validation worktrees. A failed first full-request `countTokens` terminates with no generation; success may continue within the same run. Any provider-contact terminal result consumes authorization; afterward provider authorization returns to NONE and evidence must be recursively audited. Handoff: `docs/handoff/E0A_GEMINI35_ATTEMPT04_AUTHORIZED_HANDOFF_2026_09_09.md`. No other provider or E0-E execution is authorized.
