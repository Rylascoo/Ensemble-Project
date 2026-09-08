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
Auth-transport differential **CONSUMED**. Same fresh unshared `AQ.` key/model/method/body: query leg locally inconclusive; header leg returned HTTP **200**, `totalTokens=8`. Evidence: `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`. Thus `x-goog-api-key` works live for `gemini-3.5-flash-lite:countTokens`; no auth-transport patch is justified.

## Authorized live run
Director authorization: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_DIRECTOR_AUTHORIZATION_2026_09_08.md`.

Exactly **one** third synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` reference run is authorized at promoted `689655...`, run id `E0A-REAL-G35L-20260908-03`, using the same still-active/unshared key that passed header `countTokens`; one attempt per role, zero retries, no fallback/other model/fixture/provider traffic. Any terminal result consumes authorization.

First packet launch failed locally from `C:\Users\Wiryl` at `git rev-parse --show-toplevel` before credential entry, evidence creation, `PROVIDER_INVOCATION_STARTED=YES`, or provider traffic. Authorization remains **UNCONSUMED**.

## Next
Run the corrected packet once from explicit checkout `C:\Users\Wiryl\Sol Dev\Ensemble-Project`. Preserve transcript/evidence ZIP, clear credentials immediately afterward, do not rerun after invocation starts, then audit the result before any further provider or source decision.
