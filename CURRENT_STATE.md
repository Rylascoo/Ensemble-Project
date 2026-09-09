# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` backlog order.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded provider-error diagnostic is **CLOSED — MACHINE-VALIDATED**. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`. Gemini 3 signature: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Native executable: `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`; tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless PASS. Later commits do not inherit native authority.

## Provider
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed. Rejected field remains unproven.

Director reconfirmed `gemini-3.5-flash-lite` at **15 RPM / 250,000 input TPM / 500 RPD** until a material contrary signal. Quota: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. Pricing/data-use freshness and change/error triggers remain binding.

**Attempt 04 AUTHORIZED — NOT YET CONSUMED.** One synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` run at `e6e7d6c8...`; one attempt/role, zero retries, no fallback/alternate traffic. Authorization: `docs/evidence/E0A_GEMINI35_ATTEMPT04_DIRECTOR_AUTHORIZATION_2026_09_09.md`. Provider authority is limited to that run until consumed.

## Parallel E0-E
Q-E0E-PREP **CLOSED — HOSTED NON-NETWORK PREPARATION VALIDATED**. Contract: `docs/blueprint/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CONTRACT.md`. Closeout: `docs/evidence/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CLOSEOUT_AUDIT_2026_09_09.md`. Q-E0E-RUN remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.

## Cross-lane
Q-DESIGN-10 DONE by Director + Design Sol adjudication; F1 Ordered Stagger is provisional design incumbent; Q-DESIGN-11 ACTIVE. This design-workflow change does not alter E0 blind scoring or engineering order.

## Next
Q-E0A-02 is **ACTIVE only for unconsumed Attempt 04**. Execute from a new linked Windows ARM64 worktree pinned to the validation tag. Failed first full-request `countTokens` => terminate/no generation; success may continue in the same run. Provider-contact terminal result consumes authorization; then authority returns to NONE and evidence requires recursive audit. Handoff: `docs/handoff/E0A_GEMINI35_ATTEMPT04_AUTHORIZED_HANDOFF_2026_09_09.md`. No other provider/E0-E execution is authorized.
