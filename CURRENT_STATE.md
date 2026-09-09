# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog without overriding this state.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Repository convergence/continuity cleanup are **CLOSED**. The bounded provider-error/request-compatibility amendment is **IMPLEMENTED; EXACT HOSTED CHECKPOINT REFRESH; NATIVE VALIDATION BLOCKED** on `e0a-gemini-bounded-provider-error-diagnostic`. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`.

Base architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`; Gemini 3 signature evidence: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
**Promoted native executable remains:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`: Windows ARM64 Core **622/622**, Harness **125/125**, build/smokes/credentialless **PASS**.

Hosted run `34307857685` at source/test checkpoint `ce2531075b17ee130366faf8681ecff85bb84af6` **PASS**. Later branch commits only reconciled current `main` ancestry, adopted its Design Sol queue state, and refreshed this state; no source/test/fixture/runtime project surface changed. The exact current checkout still requires its hosted gate before native validation. Hosted CI compiles Harness tests but does not execute them; the amended checkout has **no native runtime authority**.

## Provider decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed as the failure explanation. Exact rejected field remains unproven.

**3.5 Flash-Lite live execution PAUSED; bounded diagnostic amendment AUTHORIZED; 3.1 Flash-Lite execution DEFERRED. Provider authorization: NONE.** No credential use or provider traffic.

## Parallel E0-E
**Single-model playwright control preparation remains AUTHORIZED; execution remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.** Non-network only. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`.

## Next
Pass hosted gates on the exact current E0-A checkout, recursively audit it, then validate that same checkout on native Windows ARM64 with Core + Harness tests, build/smokes, and credentialless gates. If PASS, create an annotated validation tag and update ledger/state. Active transition: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`. **Validation does not authorize provider traffic.**
