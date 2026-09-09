# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. This file alone owns phase/checkpoint/validation/next action. `AGENTS.md` owns bootstrap/closeout; `docs/PROJECT_AUTHORITY.md` policy/lane authority; `docs/PROJECT_EXECUTION_QUEUE.md` sequencing.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. Q-E0A-01 diagnostic, Q-E0A-02 Attempt-04 audit, and Q-E0A-04 `countTokens` input-projection correction are **DONE**. Q-E0A-03 remains **BLOCKED FOR PROVIDER EXECUTION**. Attempt-04 audit: `docs/evidence/E0A_GEMINI35_ATTEMPT04_ARCHIVE_AUDIT_2026_09_09.md`; Gemini-3 compatibility: `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Validation
Promoted native authority: `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; annotated tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64: Core **622/622**, Harness **131/131**, fresh build/smokes/credentialless PASS; provider network NOT PERFORMED; spend $0. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_2026_09_09.md`; ledger: `docs/VALIDATION_LEDGER.md`. Later documentation commits inherit no native runtime authority.

## Provider
Attempt 04: first Performer `countTokens` HTTP **400 / INVALID_ARGUMENT** at `generate_content_request.generation_config.response_format.text.mime_type`; 0 turns/generation/spend. Authorization **CONSUMED**. Provider authorization **NONE**; no rerun absent a new explicit authorization.

## Q-E0A-03
Planning/closure method is frozen in `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`; recursive audit: `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`. First prospective live arm is 12-turn `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL`; each real run requires separate Director authorization. The 3-turn 2.5 Flash-Lite arm is control-only and cannot become the E0-A reference.

## Continuity / parallel lanes
Continuity: `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`; quota decision: `docs/evidence/E0A_GEMINI35_QUOTA_SNAPSHOT_REUSE_DIRECTOR_DECISION_2026_09_09.md`. Q-E0E-PREP CLOSED; Q-E0E-RUN BLOCKED through E0-A→D. Q-DESIGN-15 DONE; Q-DESIGN-16 ACTIVE under Website authority; CS2 Balanced Crossfade Exchange is provisional for eligible context shifts. E0 order/blind scoring unchanged.

## Next
No active engineering implementation branch. Before any Q-E0A-03 provider authorization, reverify the applicable volatile public provider facts and confirm all per-run activation gates; the current executable pricing/data-use guard expires after **2026-09-14**. Provider authorization remains **NONE**. E0-B+ and E0-E execution remain blocked by experiment order.
