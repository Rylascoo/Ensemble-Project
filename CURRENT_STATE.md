# Ensemble Current State

Updated: 2026-09-07

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Active branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative amendment + rate-discipline comparison amendment + **RPD/model-selection amendment** `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout remains:** `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` / `validation/e0a-gemini-comparison-native-arm64` — Windows ARM64 Core 622/622, Harness 117/117, fresh Harness build, fixture smokes, predecessor-profile credentialless gate PASS. Evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`.

**Current amended source/test checkpoint:** `b0286c3427eb3e2b9f0e8401098596db055f9c7e`; Validation `34185024153` **PASS** — ARM64 cross-compile, x64 Core regression, repository/oracle/document gates. Harness tests compiled but were not executed in cloud. Implementation audit: `docs/evidence/E0A_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_IMPLEMENTATION_AUDIT.md`. Supporting predecessor audits retained by the amendment: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

No Core/Core-test/fixture delta exists in this amendment. Later documentation commits do not extend native authority. Provider network, `countTokens`, inference, credential use, and spend remain NOT PERFORMED.

## Provider boundary
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier work is synthetic-fixture-only.

Director AI Studio evidence: 3.5 Flash-Lite = 15 RPM/250K TPM/500 RPD; 3.1 Flash-Lite = 15/250K/500; 2.5 Flash-Lite = 10/250K/20; 2.5 Flash = 5/250K/20.

Live-selectable profiles: `GEMINI-3.5-FLASH-LITE-MINIMAL` cap 12; `GEMINI-3.1-FLASH-LITE-MINIMAL` cap 12; `GEMINI-2.5-FLASH-LITE-NONE` cap 3. Conservative RPD admission assumes six Gemini API-bound operations per accepted turn: 72/500, 72/500, 18/20 respectively. Historical 2.5 Flash is non-live. One attempt/zero retries remains law.

## Next
**Fresh credentialless native Windows ARM64 validation at one exact clean documentation-inclusive checkout.** Run Core/Harness tests, fresh Harness build, both fixture smokes, missing-key/no-evidence-root probes for all three live profiles, and a retired-2.5-Flash rejection probe before credential access/evidence creation.

If native PASS, create the required annotated validation tag at that exact checkout, promote it here, then return to a separate Director decision for exactly one real synthetic Missing Raft run using `GEMINI-3.5-FLASH-LITE-MINIMAL`. No real run is authorized yet.
