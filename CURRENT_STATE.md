# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64` (tag object `7337ec6c6ff7c6d9d12c9a1322d0c5981e5fdb83`). Director Windows ARM64: Core **622/622**, Harness **125/125**, fresh `win-arm64` build, fixture smokes and credentialless route gates: **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Correction: countTokens supplies required nested `generateContentRequest.model`, preserves generation payload, and bounds persisted diagnostics. Cloud source/test Validation `34188767617`: PASS. Later docs do not inherit native authority.

First-real attempt 01 at predecessor `3d6d8a7f...` is consumed/failed: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`.

Second-real attempt 02 at promoted `689655...` is **consumed / TechnicalFailure**: accepted turns `0`, shadow spend `$0.000000`, 8-file evidence root, cleanup PASS. Transcript/evidence hashes and terminal facts: `docs/evidence/E0A_SECOND_REAL_GEMINI_35_LITE_ATTEMPT_02_2026_09_08.md`. Archive root-cause audit is pending; console exit `3` is the host's generic non-success exit and does not establish cancellation.

Supporting current audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

No retry, third real run, 3.1/2.5 run, other fixture, fallback, or broader provider traffic is authorized. API keys must never enter repository/evidence/chat.

## Next
**Audit the preserved attempt-02 evidence ZIP against recorded SHA-256 `7CFB16091775FD9E20827B2FCCBF8361F44713191019BFC3B7FFF0416A5D2CA0`; identify the exact bounded failure boundary before any patch or further provider decision.**
