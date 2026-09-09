# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 diagnostic, Q-E0A-02 Attempt-04 audit, and Q-E0A-04 `countTokens` input-projection correction are **DONE**; Q-E0A-03 is **ACTIVE FOR EXACTLY ONE AUTHORIZED RUN, LOCAL PREFLIGHT PENDING**. Attempt-04: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`; Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native authority: `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64: Core **622/622**, Harness **131/131**, build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`; ledger: `docs/VALIDATION_LEDGER.md`. Later docs inherit no native runtime authority.

## Provider / Q-E0A-03
Attempt 04 failed at first Performer `countTokens`: HTTP **400 / INVALID_ARGUMENT** at `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns/generation/spend; authorization consumed. Closure contract: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`; recursive audit: `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`. Public preauthorization audit: `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`.

Director authorization for exact RunId `E0A-Q03-G35L-20260909-01` is **AUTHORIZED / UNCONSUMED**, recorded in `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`. Scope is only `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL`, model `gemini-3.5-flash-lite`, 12-turn cap, canonical Missing Raft fixture/hash, exact promoted executable/tag, and exact named evidence root. Gemini 3.1, Gemini 2.5, retries, reruns, fallbacks, probes, alternate fixtures/profiles/RunIds, and unrelated provider traffic remain **NOT AUTHORIZED**. Purely local preflight failure before provider invocation does not consume this authorization; once provider invocation begins, any terminal result consumes it.

The route-specific quota decision remains 15 RPM / 250,000 input TPM / 500 RPD until a recorded contrary signal. Current official-provider facts were freshly rechecked on 2026-09-09 with no material contrary signal. Executable pricing/data-use freshness guard remains valid only through **2026-09-14**.

## Continuity / parallel lanes
Continuity: `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`. Repository navigation, handoff lifecycle, and design-residency locators are reconciled; no live `docs/handoff/...` artifact exists. Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Website authority independently advanced to `main` `4a87523f9964a1fcf884100e0158d75d7b3809c0`: Q-DESIGN-18 CLR-01 method is frozen and Q-DESIGN-19 materialization/preflight is the next Design action. Engineering does not alter that state. F1A/CS2 remain provisional; E0 order/blind scoring unchanged.

## Next
Perform the guarded native Windows ARM64 **local-only preflight** for exact RunId `E0A-Q03-G35L-20260909-01`. If and only if every local gate passes while the provider snapshot remains fresh and no material contrary signal exists, securely enter `GEMINI_API_KEY` process-locally and launch exactly the one authorized Q-E0A-03 run. Evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0A-Q03-G35L-20260909-01`. After provider invocation begins, authorization is consumed regardless of terminal result; no rerun/retry/fallback. E0-B+ / E0-E remain blocked.
