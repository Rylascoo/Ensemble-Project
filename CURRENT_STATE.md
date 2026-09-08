# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `689655eed677b789ab3ee395f1c65b4f2cb72cc8` / `validation/e0a-gemini-counttokens-correction-native-arm64`. Director Windows ARM64: Core **622/622**, Harness **125/125**, build/smokes/credentialless gates **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Earlier live evidence remains reachable through `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`, `docs/evidence/E0A_SECOND_REAL_GEMINI_35_LITE_ATTEMPT_02_ARCHIVE_AUDIT_2026_09_08.md`, `docs/evidence/E0A_GEMINI_AUTH_KEY_EXTERNAL_DIAGNOSTIC_2026_09_08.md`, `docs/evidence/E0A_GEMINI_AUTH_TRANSPORT_DIFFERENTIAL_RESULT_2026_09_08.md`, and supporting audits `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Provider evidence / decision
Attempt 03 archive verifies completely. First Performer `countTokens` returned HTTP **400** after `480.4031 ms`; 0 turns, $0, no generation. Full audit: `docs/evidence/E0A_THIRD_REAL_GEMINI_35_LITE_ATTEMPT_03_ARCHIVE_AUDIT_2026_09_08.md`.

The preceding same-key/header/model/method simple `countTokens` succeeded, so authentication transport is closed. Google docs conflict on 3.5 Flash-Lite structured-output support and the 400 body was not retained; exact rejected field remains unproven. Classification: **3.5 Flash-Lite frozen-request live compatibility unproven**.

Director decision: `docs/evidence/E0A_GEMINI_PROVIDER_COMPATIBILITY_DIRECTOR_DECISION_2026_09_08.md`. **3.5 Flash-Lite live execution PAUSED; bounded provider-error/request-compatibility engineering amendment AUTHORIZED; 3.1 Flash-Lite provider execution DEFERRED. Provider authorization: NONE.** No credential use, `countTokens`, generation/probe, inference, spend, fallback, or other provider traffic.

## Next
Active handoff: `docs/handoff/E0A_GEMINI_BOUNDED_PROVIDER_ERROR_DIAGNOSTIC_ENGINEERING_HANDOFF_2026_09_08.md`.

Fresh engineering chat: read this file first, then the handoff; construct/audit/implement the smallest Harness-local bounded diagnostic amendment. Any source change requires cloud validation plus new native Windows ARM64 validation and annotated validation tag before any future provider request.
