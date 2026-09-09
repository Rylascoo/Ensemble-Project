# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog without overriding this state.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 bounded Gemini provider-error/request-compatibility diagnostic is **CLOSED — MACHINE-VALIDATED**. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`. Gemini 3 signature support: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
**Promoted native executable:** `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a` / `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64` / tag object `9b17563474b25920da8615afefdfa3fcfdab884b`. Director Windows ARM64: Core **622/622**, Harness **130/130**, fresh build/smokes/credentialless gates **PASS**. Later commits do not inherit native runtime authority.

## Provider decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed as the failure explanation. Exact rejected field remains unproven.

**3.5 Flash-Lite live execution PAUSED; 3.1 Flash-Lite execution DEFERRED. Provider authorization: NONE.** Validation does not authorize credential use or provider traffic.

## Parallel E0-E
Q-E0E-PREP contract is **FROZEN**; deterministic non-network preconstruction has passed initial hosted compiler/static tests and is in **FINAL RECURSIVE HARDENING — FINAL HOSTED GATE PENDING** on `e0e-single-model-playwright-control-preparation`. Contract: `docs/blueprint/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CONTRACT.md`. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`.

**E0-E execution remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.** No provider request, credentials, spend, transcript production, scoring, or unblinding is authorized by preparation.

## Next
Q-E0A-02 remains **BLOCKED** pending provider/account/pricing/quota re-verification plus explicit Director authorization. Active E0-A transition: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`. Finish Q-E0E-PREP recursive audit and hosted non-network gates; compiler/static evidence is preparation evidence, not E0-E behavioral or native-runtime authority.
