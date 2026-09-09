# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog without overriding this state.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded Gemini provider-error/request-compatibility diagnostic is **CLOSED — MACHINE-VALIDATED**. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`. Gemini 3 signature support: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
**Promoted native executable:** `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` / `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64` / tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Director Windows ARM64: Core **622/622**, Harness **130/130**, build/smokes/credentialless gates **PASS**. Evidence: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_NATIVE_ARM64_VALIDATION_2026_09_08.md`. Later commits do not inherit native runtime authority.

## Provider decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method `countTokens` succeeded; auth transport is closed as the failure explanation. Exact rejected field remains unproven.

**3.5 Flash-Lite live execution PAUSED; 3.1 Flash-Lite execution DEFERRED. Provider authorization: NONE.** Validation does not authorize credential use or provider traffic.

## Parallel E0-E
Q-E0E-PREP is **CLOSED — HOSTED NON-NETWORK PREPARATION VALIDATED**. Contract: `docs/blueprint/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CONTRACT.md`. Closeout: `docs/evidence/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CLOSEOUT_AUDIT_2026_09_09.md`.

Prepared tree `90c887358e973f7c4e3b1cd7ba2f56e0028affe9`: run `34314008749` passed win-arm64 cross-compile and deterministic tests **10/10**; identical-tree anchor `b24522de9c0845644ff2b05c8041a76322e6ac95` passed standard run `34314069624`. This is preparation/compiler evidence only, not native-runtime or behavioral evidence.

**Q-E0E-RUN remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.** No provider request, credentials, spend, transcript production, scoring, or unblinding is authorized.

## Cross-lane continuity
Q-DESIGN-08 DONE; Q-DESIGN-09 ACTIVE. MOT-01 preflight is design sequencing only; engineering checkpoint, validation, provider authority, Q-DESIGN-02, and E0 order are unchanged.

## Next
There is **no active engineering implementation branch**. Q-E0A-02 remains **BLOCKED** pending provider/account/pricing/quota re-verification plus explicit Director authorization. Fresh-chat transition: `docs/handoff/E0A_PROVIDER_COMPATIBILITY_BLOCKED_HANDOFF_2026_09_09.md`. No E0-E execution action is authorized.
