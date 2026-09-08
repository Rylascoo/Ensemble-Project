# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` / `validation/e0a-gemini-rpd-model-selection-native-arm64`. Native Windows ARM64: Core **622/622**, Harness **122/122**, fresh Harness build, fixture smokes, credentialless live-route gates, retired-route rejection, exact HEAD/cleanliness: **PASS**.

First-real attempt audit: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Supporting audits: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Provider boundary
The one-run Director authorization is **CONSUMED**. Attempt 01 at `3d6d8a7f...` terminated `TechnicalFailure`, accepted turns 0, shadow spend `$0.000000`. Preserved ZIP SHA-256 `98400A524D7500A16B07A5D4EB2AB7CF79E455FFEDA08F2237616C110523FC07` was verified; all sealed artifact hashes matched and no API-key marker was found.

Archive audit resolves the first failure to Performer `countTokens` preflight (`input-token-count-failed`, 566.1856 ms). No successful token count, spend reservation, provider generation call, inference, accepted fiction, or provider receipt occurred. Attempt 01 did not retain the exact countTokens HTTP status/body.

Root cause: nested `generateContentRequest` omitted required `model`. Correction `689655eed677b789ab3ee395f1c65b4f2cb72cc8` adds `model = models/{selected-model}` only to countTokens, preserves generation payload, and records only bounded countTokens HTTP/transport/response diagnostics while suppressing arbitrary exception text. Cloud Validation `34188767617`: **PASS**. No Core/Core-test/fixture delta. Correction is **NOT native-validated yet**.

No retry, second run, 3.1/2.5 run, other fixture, or broader provider traffic is authorized. API keys must never enter repository/evidence/chat.

## Next
**Fresh credentialless Windows ARM64 validation of exact correction checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8`.** Run Core + Harness tests, fresh Harness build, fixture smokes, current missing-key gates and retired-route rejection; no provider credential/network/countTokens/inference/spend. Any later real-provider invocation requires new explicit Director authorization.