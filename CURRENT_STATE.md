# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone carries phase/checkpoint/validation/next-action authority. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` owns product/policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequences backlog without overriding this state.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Repository convergence/continuity cleanup are **CLOSED**. The bounded provider-error/request-compatibility amendment is **IMPLEMENTED; NATIVE ATTEMPT 01 FAILED; TEST-ONLY REGRESSION CORRECTION IN PROGRESS** on `e0a-gemini-bounded-provider-error-diagnostic`. Contract: `docs/blueprint/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_AMENDMENT.md`. Audit: `docs/evidence/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_IMPLEMENTATION_AUDIT_2026_09_08.md`.

## Validation
**Promoted native executable remains:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`: Windows ARM64 Core **622/622**, Harness **125/125**, build/smokes/credentialless **PASS**.

Hosted run `34308724485` at `20f76e7d5f13915471581fb9263d6a0eb1e5343c` **PASS**. Native attempt 01 at that exact clean detached checkout: Core **622/622 PASS**; Harness **128/130 FAIL**. Both failures were stale malformed-`countTokens` tests expecting generic `E0AHarnessException` instead of the frozen dedicated `E0AGeminiCountTokensFailureException`. No build/smoke/credentialless completion followed the Harness failure; no validation tag was authorized. The branch now contains a test-only expectation repair and requires a fresh exact hosted gate before native rerun.

## Provider decision
Attempt 03: first Performer `countTokens` HTTP **400**, 0 turns, $0, no generation. Same-key/header/model/method simple `countTokens` succeeded; auth transport is closed as the failure explanation. Exact rejected field remains unproven.

**3.5 Flash-Lite live execution PAUSED; bounded diagnostic amendment AUTHORIZED; 3.1 Flash-Lite execution DEFERRED. Provider authorization: NONE.** No credential use or provider traffic.

## Parallel E0-E
**Single-model playwright control preparation remains AUTHORIZED; execution remains BLOCKED until E0-A -> E0-B -> E0-C -> E0-D close.** Non-network only. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`.

## Next
Re-audit current `main` concurrency, reconcile only if required, pass hosted gates on the exact corrected E0-A checkout, then rerun native Windows ARM64 Core + Harness tests, build/smokes, and credentialless gates on that same checkout. If PASS, create an annotated validation tag and update ledger/state. Active transition: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`. **Validation does not authorize provider traffic.**
