# Ensemble Current State

Updated: 2026-09-07

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries active phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. Promoted `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`.

Active branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative amendment + comparison amendment `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md` (`76fc0c64...`). Cloud-validated source/test checkpoint: `8ed1563ec7a4c3ae919a649db0dc3c29e17a03e0`; no Core/Core-test/fixture delta from `main`.

## Validation
Source/test checkpoint `8ed1563e...`: Validation `34179384123` **PASS** — ARM64 cross-compile, x64 Core regression, repository/oracle/document gates; compiler/cloud-static only.

Director-machine execution at exact checkout `c9f706b42350c8b6cfc462e09cf71db6bc3a2355`: Windows ARM64 **PASS** — Core 622/622, Harness 117/117, fresh native Harness build, both fixture smokes, and expected missing-key refusal/no evidence root for all three approved profiles. Evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`.

This native result is recorded but **not yet promoted as repository machine-validation authority**: Repository Surface Law requires an annotated validation tag at exact checkout first. Promoted native authority therefore remains `cc395a25162a0a682796bffb44060c799df0db32` / `validation/e0a-pre-restructure-closure-native-arm64`. No provider network, `countTokens`, inference, or spend occurred.

## Provider boundary
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier experiments are synthetic-fixture-only; RPD remains `unverified-pre-live`.

Profiles: `GEMINI-2.5-FLASH-LITE-NONE` = `CREATIVE-NONE`, 10 RPM/250K TPM; `GEMINI-3.5-FLASH-LITE-MINIMAL` = `CREATIVE-MINIMAL`, 15 RPM/250K TPM; `GEMINI-2.5-FLASH-NONE` = `CREATIVE-NONE`, 5 RPM/250K TPM. Harness paces every Gemini API operation plus exact rolling generation-input TPM; one attempt/zero retries. Gemini 3.5 opaque `thoughtSignature` is stripped before evidence and semantic output; `thought=true` remains rejected.

## Next
Live handoff: `docs/handoff/E0A_POST_ER1_FIRST_REAL_GEMINI_RUN_DIRECTOR_GATE_HANDOFF_2026_09_07.md`.

**Next gate:** create/push annotated tag `validation/e0a-gemini-comparison-native-arm64` at exact `c9f706b42350c8b6cfc462e09cf71db6bc3a2355`; message must record native Windows ARM64 validation, credentialless/fake-only scope, and the evidence path above.

After the tag exists, promote that native authority here, then return to a separate Director live-run decision. Planned order: 2.5 Flash-Lite None -> 3.5 Flash-Lite Minimal -> 2.5 Flash None; each real run requires separate authorization.
