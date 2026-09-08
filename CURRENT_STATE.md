# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Current promoted machine-tested checkout:** `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` / `validation/e0a-gemini-rpd-model-selection-native-arm64`: Windows ARM64 Core **622/622**, Harness **122/122**, fresh Harness build, fixture smokes, credentialless live-route gates and retired-route rejection PASS.

Attempt 01 evidence: `docs/evidence/E0A_FIRST_REAL_GEMINI_35_LITE_ATTEMPT_01_2026_09_08.md`. Its preserved archive resolved failure to first Performer `countTokens`; no successful token count, spend reservation, generation, inference, accepted fiction, or provider receipt occurred.

Correction `689655eed677b789ab3ee395f1c65b4f2cb72cc8` adds required nested `generateContentRequest.model`, preserves generation payload, and bounds persisted countTokens diagnostics. Cloud Validation `34188767617`: PASS. No Core/Core-test/fixture delta.

Director-host execution packet for exact correction checkout `689655...`: Windows ARM64 Core **622/622**, Harness **125/125**, fresh `win-arm64` Harness build, both fixture smokes, three current missing-key gates, retired-route rejection, exact HEAD/cleanliness and credential absence: **PASS**. Evidence: `docs/evidence/E0A_GEMINI_COUNTTOKENS_CORRECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`. This result is recorded but **not promoted** until the required annotated validation tag exists.

No provider network, `countTokens`, inference, spend, retry, second run, other fixture, or broader provider traffic is authorized. API keys must never enter repository/evidence/chat.

## Next
**Create and verify an annotated validation tag at exact correction checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8`.** After tag verification, promote it in `CURRENT_STATE.md` and `docs/VALIDATION_LEDGER.md`. Any later real-provider invocation requires new explicit Director authorization.
