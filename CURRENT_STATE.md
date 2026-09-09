# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog/prerequisites without overriding this state.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Repository convergence/continuity cleanup are **CLOSED**. The bounded provider-error/request-compatibility amendment is **IMPLEMENTED; FINAL HOSTED RERUN PENDING; NATIVE VALIDATION BLOCKED** on `e0a-gemini-bounded-provider-error-diagnostic`. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`.

Base architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`; Gemini 3 signature evidence: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
**Promoted native executable remains:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`: Windows ARM64 Core **622/622**, Harness **125/125**, build/smokes/credentialless **PASS**. Earlier hosted run `34307310623` passed compiler/build, x64 Core regression, repository law, oracle coverage, and document census, but final parser hardening followed it; the exact current checkout requires a fresh hosted gate and has **no native runtime authority**.

Concurrent queue-only `main` `548f1a58...` is content/topology-reconciled; merge `17290f87...` changed no E0-A tree content.

## Provider decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed as the failure explanation. Exact rejected field remains unproven.

**3.5 Flash-Lite live execution PAUSED; bounded diagnostic amendment AUTHORIZED; 3.1 Flash-Lite execution DEFERRED. Provider authorization: NONE.** No credential use or provider traffic.

## Parallel E0-E
**Single-model playwright control preparation remains AUTHORIZED; execution remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.** Non-network only. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`.

## Next
Require the exact E0-A checkout to pass hosted compiler/static gates; recursively audit it; then validate that exact checkout on native Windows ARM64 with Core + Harness tests, build/smokes, and credentialless gates. If PASS, create an annotated validation tag and update ledger/state. Active transition: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`. **Validation does not authorize provider traffic.**
