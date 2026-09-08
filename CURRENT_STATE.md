# Ensemble Current State

Updated: 2026-09-08

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative + rate-discipline comparison + RPD/model-selection amendment `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` / `validation/e0a-gemini-comparison-native-arm64` — Windows ARM64 Core 622/622, Harness 117/117, fresh Harness build, fixture smokes, predecessor credentialless gate PASS. Evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`.

**Corrected amended source/test checkpoint:** `b4d39cd91d1c23bad1f0354702fb64411e82780d`; Validation `34185909785` **PASS** — ARM64 cross-compile, x64 Core regression, repository/oracle/document gates. Harness tests compiled, not run in cloud. Audit: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md`. Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

Native attempt 01 at `31436ee5...`: ARM64 host PASS; Core 622/622 PASS; Harness 121/122 FAIL because a predecessor readiness test still treated retired 2.5 Flash as live. Test-only correction is `b4d39cd9...`; remaining native stages were not reached. No partial promotion.

No Core/Core-test/fixture delta. Provider network, `countTokens`, inference, credentials, spend: NOT PERFORMED.

## Provider boundary
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier work is synthetic-fixture-only.

AI Studio: 3.5 Flash-Lite = 15 RPM/250K TPM/500 RPD; 3.1 Flash-Lite = 15/250K/500; 2.5 Flash-Lite = 10/250K/20; 2.5 Flash = 5/250K/20.

Live profiles: `GEMINI-3.5-FLASH-LITE-MINIMAL` cap 12; `GEMINI-3.1-FLASH-LITE-MINIMAL` cap 12; `GEMINI-2.5-FLASH-LITE-NONE` cap 3. Conservative RPD: 72/500, 72/500, 18/20. Historical 2.5 Flash is non-live. One attempt/zero retries remains law.

## Next
**Fresh credentialless native Windows ARM64 validation at one exact clean documentation-inclusive checkout containing `b4d39cd9...` plus this continuity state.** Run Core/Harness tests, fresh Harness build, both fixture smokes, missing-key/no-evidence-root probes for all three live profiles, and retired-2.5-Flash rejection before credential access/evidence creation.

If PASS, create the annotated validation tag, promote it here, then return to a separate Director decision for exactly one real synthetic Missing Raft `GEMINI-3.5-FLASH-LITE-MINIMAL` run. No real run is authorized yet.
