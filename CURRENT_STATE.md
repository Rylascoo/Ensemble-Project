# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, fresh build/smokes/credentialless gates **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Attempt 01 (`3d6d8a7f...`) failed before token count: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Attempt 02 (`689655...`) failed at first Performer `countTokens`: HTTP **401**, 0 turns, $0 spend, no generation. Audit: `docs/evidence/E0A_SECOND_REAL_GEMINI_35_LITE_ATTEMPT_02_ARCHIVE_AUDIT_2026_09_08.md`.

External diagnostic: new `AQ.` key reached `gemini-3.6-flash:generateContent` via `?key=`. Evidence: `docs/evidence/E0A_GEMINI_AUTH_KEY_EXTERNAL_DIAGNOSTIC_2026_09_08.md`. Exposed key must never be reused.

Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Provider diagnostic
Auth differential **CONSUMED**. Same fresh unshared key/model/method/body: query leg locally inconclusive; header leg HTTP **200**, `totalTokens=8`. Evidence: `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`. Thus `x-goog-api-key` works live for `gemini-3.5-flash-lite:countTokens`; no auth patch justified.

## Third real run
Authorization record: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_DIRECTOR_AUTHORIZATION_2026_09_08.md` — **CONSUMED**.

Attempt 03 at promoted `689655...`, id `E0A-REAL-G35L-20260908-03`, used the same known-good unshared key. All repository/native/build/fixture/artifact/key-integrity preflights passed; invocation started once and terminated `TechnicalFailure`, 0 accepted turns, $0 shadow spend, exit 3, empty stderr. Evidence root: 8 files. ZIP SHA-256 `8120E426AE206439A9E43F8D5C85C277B492B9055A0ED012133793B91EC1D77E`. Result: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_2026_09_08.md`.

Provider authorization: **NONE**. No retry/fourth run/fallback/other provider traffic is authorized.

## Next
**Attempt-03 archive audit only.** Obtain the preserved ZIP, verify its SHA-256 and sealed artifact hashes, recursively identify the exact failure boundary, then decide whether any source/provider action is justified. Do not infer diagnosis from exit code 3 alone and do not make another provider request before the archive audit.