# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Current promoted machine-tested checkout remains:** `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` / `validation/e0a-gemini-comparison-native-arm64` until the amended checkout receives its required annotated tag.

**Amended native Windows ARM64 checkout:** `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99` — Core **622/622**, Harness **122/122**, fresh `win-arm64` Harness build PASS, Missing Raft + generic fixture smokes PASS, all three current credentialless live-profile gates PASS with no evidence roots, retired 2.5 Flash pre-credential rejection PASS, exact HEAD/cleanliness PASS. Evidence: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_NATIVE_ARM64_VALIDATION_2026_09_08.md`.

Native attempt 01 at `31436ee5...` failed Harness 121/122 because a stale readiness test treated retired 2.5 Flash as live; correction `b4d39cd91d1c23bad1f0354702fb64411e82780d` changed only that test surface and cloud Validation `34185909785` passed. No partial promotion. Audit: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md`. Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

No Core/Core-test/fixture delta. Later documentation commits do not inherit native authority. Provider network, `countTokens`, inference, credentials, spend: **NOT PERFORMED**.

## Provider boundary
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier work is synthetic-fixture-only.

AI Studio: 3.5 Flash-Lite = 15 RPM/250K TPM/500 RPD; 3.1 Flash-Lite = 15/250K/500; 2.5 Flash-Lite = 10/250K/20; 2.5 Flash = 5/250K/20.

Live profiles: `GEMINI-3.5-FLASH-LITE-MINIMAL` cap 12; `GEMINI-3.1-FLASH-LITE-MINIMAL` cap 12; `GEMINI-2.5-FLASH-LITE-NONE` cap 3. Conservative RPD: 72/500, 72/500, 18/20. Historical 2.5 Flash is non-live. One attempt/zero retries remains law.

## Next
**Annotated validation-tag promotion gate.** Create and remotely verify an annotated tag that dereferences exactly to `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99`; then promote that checkout here and in `docs/VALIDATION_LEDGER.md`.

Only after promotion return to a separate Director decision for exactly one real synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` run. No real run is authorized yet.
