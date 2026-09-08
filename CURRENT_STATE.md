# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, build/smokes/credentialless gates **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Attempt 01: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Attempt 02 audit: `docs/evidence/E0A_SECOND_REAL_GEMINI_35_LITE_ATTEMPT_02_ARCHIVE_AUDIT_2026_09_08.md`. External key diagnostic: `docs/evidence/E0A_GEMINI_AUTH_KEY_EXTERNAL_DIAGNOSTIC_2026_09_08.md` (exposed key never reusable).

Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Provider evidence
Auth differential **CONSUMED**: same fresh unshared key via `x-goog-api-key` returned HTTP **200**, `totalTokens=8` for simple `gemini-3.5-flash-lite:countTokens`. Evidence: `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`.

Attempt 03 authorization is **CONSUMED**. ZIP/hash/runtime seal all verify. Exact failure: first Performer `countTokens` returned HTTP **400** after `480.4031 ms`; 0 turns, $0, no generation/inference. Audit: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_ARCHIVE_AUDIT_2026_09_08.md`.

The preserved full request uses structured JSON `responseFormat`. Google's current explicit structured-output support table does **not** list Gemini 3.5 Flash-Lite, while it does list Gemini 3.1 Flash-Lite. Thus the 3.5 Flash-Lite profile has a material provider-contract admissibility gap. This is the strongest root-cause candidate for HTTP 400, but the exact Google field violation is unproven because the 400 body was not retained.

Provider authorization: **NONE**. No retry/fourth 3.5 run/fallback/other provider traffic.

## Next
**Director model-admissibility decision only:** suspend/remove 3.5 Flash-Lite from the live comparison unless structured-output capability is explicitly established, and decide whether to advance the already-approved 3.1 Flash-Lite comparator. No provider request or source patch before that decision. A bounded structured-error diagnostic improvement is optional future engineering work and would require new native validation.
