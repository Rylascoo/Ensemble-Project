# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, build/smokes/credentialless gates **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Attempt 01: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Attempt 02: first Performer `countTokens` HTTP **401**, 0 turns, $0, no generation; audit: `docs/evidence/E0A_SECOND_REAL_GEMINI_35_LITE_ATTEMPT_02_ARCHIVE_AUDIT_2026_09_08.md`.

External diagnostic: `docs/evidence/E0A_GEMINI_AUTH_KEY_EXTERNAL_DIAGNOSTIC_2026_09_08.md` (exposed key must never be reused). Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Provider diagnostic
Auth differential **CONSUMED**: query leg locally inconclusive; same fresh unshared key via `x-goog-api-key` returned HTTP **200**, `totalTokens=8` for `gemini-3.5-flash-lite:countTokens`. Evidence: `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`. No auth patch justified.

## Third real run
Authorization `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_DIRECTOR_AUTHORIZATION_2026_09_08.md` is **CONSUMED**.

Attempt 03 (`E0A-REAL-G35L-20260908-03`) used the known-good unshared key at `689655...`. All local preflights passed; the one invocation terminated `TechnicalFailure`, 0 turns, $0, exit 3, empty stderr. Evidence root had 8 files. ZIP SHA-256 `8120E426AE206439A9E43F8D5C85C277B492B9055A0ED012133793B91EC1D77E`. Result: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_2026_09_08.md`.

Provider authorization: **NONE**. No retry/fourth run/fallback/other provider traffic.

## Next
**Attempt-03 archive audit only.** Verify ZIP and sealed hashes, identify the exact failure boundary, then decide whether any source/provider action is justified. Do not diagnose from exit 3 alone or make another provider request first.