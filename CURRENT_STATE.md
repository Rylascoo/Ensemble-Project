# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint

Runtime: **E0-A Experimental Harness — Phase B.** E-R1 is **CLOSED / PROMOTED / ARCHIVED**. Promoted `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`.

Active branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference-run envelope + Gemini normative amendment + comparison amendment `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md` at `76fc0c64da4724a7a352a656a062d5c3ed431ad6`.

Cloud-validated source/test checkpoint: `8ed1563ec7a4c3ae919a649db0dc3c29e17a03e0`. Evidence:
- `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`
- `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`

From `main` through `8ed1563e...`: no Core source, Core-test, or fixture delta.

## Validation

Promoted native authority remains `cc395a25162a0a682796bffb44060c799df0db32` / `validation/e0a-pre-restructure-closure-native-arm64` until Repository Surface validation-tag law is satisfied for the comparison checkout.

Source/test head `8ed1563e...`: cloud Validation `34179384123` **PASS** — ARM64 cross-compile of Core/Harness/test projects, x64 Core regression, repository laws, oracle coverage, document census. Compiler/cloud-static only.

Director-machine execution on exact documentation-inclusive checkout `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` produced a complete credentialless Windows ARM64 **PASS**: Core 622/622, Harness 117/117, fresh native Harness build, Missing Raft smoke, generic fixture smoke, and expected missing-key refusal with no evidence-root creation for all three approved arm/profile pairs. Evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`.

This Director-machine result is **recorded but not yet promoted as repository machine-validation authority** because the required annotated validation tag at exact checkout `c9f706b4...` has not yet been created/pushed. No provider network, `countTokens`, inference, or spend occurred.

## Provider boundary

Real Gemini credentials, provider `countTokens`, network execution, inference, and spend are **NOT AUTHORIZED**. Free-tier experiments are synthetic-fixture-only; RPD remains `unverified-pre-live`.

Approved profiles:
- `GEMINI-2.5-FLASH-LITE-NONE` — `CREATIVE-NONE`; 10 RPM / 250K TPM.
- `GEMINI-3.5-FLASH-LITE-MINIMAL` — `CREATIVE-MINIMAL`; 15 RPM / 250K TPM.
- `GEMINI-2.5-FLASH-NONE` — `CREATIVE-NONE`; 5 RPM / 250K TPM.

Harness paces every Gemini API operation and exact rolling generation-input TPM; one attempt / zero retries remains law. Transport admits only catalogued models. Gemini 3.5 opaque `thoughtSignature` metadata is stripped before streaming evidence and never enters semantic output; actual `thought=true` material remains rejected.

## Next

Live handoff: `docs/handoff/E0A_POST_ER1_FIRST_REAL_GEMINI_RUN_DIRECTOR_GATE_HANDOFF_2026_09_07.md`.

**Next gate: satisfy Repository Surface Law by creating and pushing annotated tag `validation/e0a-gemini-comparison-native-arm64` at exact validated checkout `c9f706b42350c8b6cfc462e09cf71db6bc3a2355`, with message recording native Windows ARM64 validation, credentialless/fake-only scope, and `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`.**

After the tag exists, promote that native authority in `CURRENT_STATE.md` and return to a separate Director live-run decision. Planned order: 2.5 Flash-Lite None -> 3.5 Flash-Lite Minimal -> 2.5 Flash None; each real run requires separate authorization.
