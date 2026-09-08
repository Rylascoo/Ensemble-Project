# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Current promoted machine-tested checkout:** `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` / `validation/e0a-gemini-rpd-model-selection-native-arm64`: Windows ARM64 Core **622/622**, Harness **122/122**, build/smokes/credentialless route gates PASS.

Attempt 01: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Archive audit resolved failure to first Performer `countTokens`; no successful token count, spend reservation, generation, inference, accepted fiction, or provider receipt.

Correction `689655eed677b789ab3ee395f1c65b4f2cb72cc8` adds required nested `generateContentRequest.model`, preserves generation payload, and bounds countTokens diagnostics. Cloud Validation `34188767617`: PASS. No Core/Core-test/fixture delta.

Director-host packet at exact correction checkout `689655...`: Windows ARM64 Core **622/622**, Harness **125/125**, fresh `win-arm64` build, both fixture smokes, three current missing-key gates, retired-route rejection, HEAD/cleanliness and credential absence: **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`. Recorded, **not promoted** until annotated validation tag exists.

Supporting current audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

No provider network, `countTokens`, inference, spend, retry, second run, other fixture, or broader provider traffic is authorized. API keys must never enter repository/evidence/chat.

## Next
**Create and verify an annotated validation tag at exact correction checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8`.** Then promote it in `CURRENT_STATE.md` and `docs/VALIDATION_LEDGER.md`. Any later real-provider invocation requires new explicit Director authorization.
