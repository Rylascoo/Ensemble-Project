# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority; `AGENTS.md` defines exact-ref bootstrap/closeout, `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority, and `docs/PROJECT_EXECUTION_QUEUE.md` preserves ordered backlog/prerequisites without overriding this state. Lane-only queue reconciliation does not change engineering phase, checkpoint, validation, provider, or next-action authority.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Repository convergence and continuity cleanup are **CLOSED**. The bounded provider-error/request-compatibility amendment is **IN IMPLEMENTATION** on `e0a-gemini-bounded-provider-error-diagnostic`: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`.

Current base architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`; Gemini 3 signature support: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
**Promoted executable checkout remains:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, build/smokes/credentialless gates **PASS**. The active amendment branch now contains later source changes and therefore has **no native runtime authority yet**.

## Provider evidence / decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; authentication transport is closed as that failure's explanation. Exact rejected field remains unproven. Classification: **3.5 Flash-Lite frozen-request live compatibility unproven**.

Director decision: **3.5 Flash-Lite live execution PAUSED; bounded provider-error/request-compatibility engineering amendment AUTHORIZED; 3.1 Flash-Lite provider execution DEFERRED. Provider authorization: NONE.** No credential use or provider traffic.

## Parallel evidence-lane preparation
**E0-E single-model playwright control preparation is AUTHORIZED now; E0-E execution remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D are closed.** Preparation is non-network only. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`.

## Next
Complete the bounded diagnostic implementation/tests and cloud validation on `e0a-gemini-bounded-provider-error-diagnostic`; then stop at a new native Windows ARM64 validation + annotated-tag gate before any provider request. Active transition contract: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`.
