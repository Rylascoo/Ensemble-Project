# Ensemble Current State

Updated: 2026-09-07

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Active branch: `e0a-gemini-rate-discipline-model-comparison`.

Current architecture: reference envelope + Gemini normative amendment + rate-discipline comparison amendment + **approved RPD/model-selection amendment** `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md` (`825309e4...`).

## Validation
**Promoted machine-tested checkout:** `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` / `validation/e0a-gemini-comparison-native-arm64`.

Director Windows ARM64 PASS there: Core 622/622, Harness 117/117, fresh Harness build, fixture smokes, and credentialless refusals for the predecessor three-profile executable. Evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`. Supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

Later docs do not extend native authority. Provider network, `countTokens`, inference, credential use, and spend remain NOT PERFORMED.

## Provider boundary
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier work is synthetic-fixture-only.

Director AI Studio evidence: 3.5 Flash-Lite = 15 RPM/250K TPM/500 RPD; 3.1 Flash-Lite = 15/250K/500; 2.5 Flash-Lite = 10/250K/20; 2.5 Flash = 5/250K/20.

Approved next live-selectable set: `GEMINI-3.5-FLASH-LITE-MINIMAL` full 12 turns; `GEMINI-3.1-FLASH-LITE-MINIMAL` full 12 turns; `GEMINI-2.5-FLASH-LITE-NONE` exact-no-thinking control capped at 3 turns. Conservative RPD admission assumes six Gemini API-bound operations per accepted turn. 2.5 Flash is removed from live selection. Each real run remains separately authorized.

## Next
**Implement the approved RPD/model-selection amendment in Harness/tests/evidence only.** Preserve Core/Core-tests/fixtures, existing RPM/TPM discipline, one attempt/zero retries, and historical 2.5 Flash compatibility anchor. Generalize opaque `thoughtSignature` handling to both approved Gemini 3 Flash-Lite profiles; expose RPD/per-profile turn cap in evidence; enforce the profile cap and static RPD admissibility.

Then recursively audit, pass cloud Validation, and run a fresh credentialless native Windows ARM64 gate at one exact checkout before any real provider decision.
