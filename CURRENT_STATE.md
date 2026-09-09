# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 diagnostic, Q-E0A-02 Attempt-04 audit, and Q-E0A-04 `countTokens` input-projection correction are **DONE**; Q-E0A-03 is **BLOCKED FOR PROVIDER EXECUTION**. Attempt-04: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`; Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native authority: `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64: Core **622/622**, Harness **131/131**, build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`; ledger: `docs/VALIDATION_LEDGER.md`. Later docs inherit no native runtime authority.

## Provider / Q-E0A-03
Attempt 04 failed at first Performer `countTokens`: HTTP **400 / INVALID_ARGUMENT** at `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns/generation/spend; authorization consumed. Closure contract: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`; recursive audit: `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`. First prospective live arm: 12-turn `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL`. Each real run needs separate explicit Director authorization. The 3-turn 2.5 Flash-Lite arm is control-only. Provider authorization **NONE**.

## Continuity / parallel lanes
Continuity: `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`. Repository navigation, handoff lifecycle, and design-residency locators are reconciled; no live `docs/handoff/...` artifact exists. Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-16/17 DONE; Q-DESIGN-18 ACTIVE under Website authority. Website `main` `bddf700994d9e68a98080b266fe0bd146a97809c` carries CS2 envelope closure and CLR-01 selection; F1A/CS2 remain provisional. E0 order/blind scoring unchanged.

## Next
Public preauthorization audit: `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`. Next gate is explicit Director authorization for one named 3.5 run, then local native preflight. Snapshot guard expires after **2026-09-14**. Provider authorization **NONE**; E0-B+ / E0-E remain blocked.
