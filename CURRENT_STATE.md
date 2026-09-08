# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, fresh build/smokes/credentialless route gates **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Attempt 01 (`3d6d8a7f...`) failed before successful token count: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Attempt 02 (`689655...`) failed at first Performer `countTokens`: HTTP **401**, 0 turns, $0 shadow spend, no generation. Audit: `docs/evidence/E0A_SECOND_REAL_GEMINI_35_LITE_ATTEMPT_02_ARCHIVE_AUDIT_2026_09_08.md`.

External diagnostic: new `AQ.` key reached `gemini-3.6-flash:generateContent` via `?key=`. Evidence: `docs/evidence/E0A_GEMINI_AUTH_KEY_EXTERNAL_DIAGNOSTIC_2026_09_08.md`. That exposed key must never be reused.

Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Provider diagnostic result
Auth-transport differential **CONSUMED**. Same fresh unshared `AQ.` key/model/method/body: query leg produced no HTTP response due local Windows PowerShell `MethodInvocationException`; header leg returned HTTP **200**, `totalTokens=8`. Evidence: `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`.

Thus `x-goog-api-key` works live for `gemini-3.5-flash-lite:countTokens` with a fresh Auth key. Attempt 02's 401 is not evidence of generic header-auth incompatibility. Query-param behavior remains unresolved; no Harness auth-transport patch is justified.

Provider authorization: **NONE**.

## Next
**Known-good-key reference-run decision.** Decide whether to authorize one third synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` run at promoted `689655...` using the same still-unshared key that just succeeded via header, one attempt per role / zero retries / no fallback or other model. If that key is revoked, re-establish a replacement key's header `countTokens` validity before a reference run.
