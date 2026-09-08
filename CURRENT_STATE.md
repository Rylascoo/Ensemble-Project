# Ensemble Current State

Updated: 2026-09-07

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Active branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative amendment + `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md` (`76fc0c64...`). Cloud source/test checkpoint `8ed1563ec7a4c3ae919a649db0dc3c29e17a03e0`; no Core/Core-test/fixture delta from `main` through it.

## Validation
**Promoted machine-tested checkout:** `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` / `validation/e0a-gemini-comparison-native-arm64`.

Director Windows ARM64 PASS: Core 622/622, Harness 117/117, fresh Harness build, both fixture smokes, and expected missing-key/no-evidence-root refusal for all three profiles. Evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`.

Current supporting audits: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`; `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

Cloud source/test gate `34179384123` PASS. Later documentation commits do not extend native authority. Provider network, `countTokens`, inference, credential use, and spend were NOT PERFORMED.

## Provider boundary
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier work is synthetic-fixture-only.

Director AI Studio quota evidence now establishes: 2.5 Flash-Lite = 10 RPM / 250K TPM / **20 RPD**; 3.5 Flash-Lite = 15 RPM / 250K TPM / **500 RPD**; 2.5 Flash = 5 RPM / 250K TPM / **20 RPD**. It also exposes 3.1 Flash-Lite as an unapproved candidate at 15 RPM / 250K TPM / **500 RPD**.

Current approved profiles remain unchanged pending amendment. Harness paces every Gemini API operation and exact rolling generation-input TPM; one attempt/zero retries. Gemini 3.5 `thoughtSignature` is stripped before evidence/semantics; `thought=true` is rejected.

## Next
**Reopen the first-real-provider model-selection gate before any provider call.** A maximum 12-turn run can require 36 generation calls, so either 2.5 Free-tier profile (20 RPD) cannot guarantee completion of the approved envelope even before unresolved `countTokens` RPD treatment is considered.

Recursively audit a narrow comparison amendment centered on the 500-RPD Flash-Lite routes, preserving 3.5 Flash-Lite as required comparison and deciding whether 3.1 Flash-Lite should join/replace a 2.5 arm. No real run is authorized until the Director approves the amended set/order and the exact executable is revalidated if source changes are required.
