# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority; `AGENTS.md` defines exact-ref bootstrap/closeout, `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority, and `docs/PROJECT_EXECUTION_QUEUE.md` preserves ordered backlog/prerequisites without overriding this state.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Repository convergence/continuity cleanup are **CLOSED**. The bounded provider-error/request-compatibility amendment is **IMPLEMENTED; HOSTED COMPILER/STATIC GATE PASS; NATIVE VALIDATION PENDING** on `e0a-gemini-bounded-provider-error-diagnostic`. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`.

Current base architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`; Gemini 3 signature support: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
**Promoted native executable remains:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, build/smokes/credentialless **PASS**. Hosted run `34307310623` at source/test checkpoint `9442b5ce...`: ARM64 cross-compile/build gates, x64 Core regression, repository law, oracle coverage, and document census **PASS**; Harness tests were built, not executed. The amendment has **no native runtime authority yet**.

## Provider evidence / decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed as that failure's explanation. Exact rejected field remains unproven. Classification: **3.5 Flash-Lite frozen-request live compatibility unproven**.

Director decision: **3.5 Flash-Lite live execution PAUSED; bounded diagnostic amendment AUTHORIZED; 3.1 Flash-Lite execution DEFERRED. Provider authorization: NONE.** No credential use or provider traffic.

## Parallel evidence-lane preparation
**E0-E single-model playwright control preparation remains AUTHORIZED; execution remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.** Preparation is non-network only. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`.

## Next
Reconcile the concurrent queue-only `main` parent without changing E0-A content; recursively audit the final checkout; then perform new native Windows ARM64 validation and create an annotated validation tag before any provider request. Active transition: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`.
