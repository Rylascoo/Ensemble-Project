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

Promoted native authority remains `cc395a25162a0a682796bffb44060c799df0db32` / `validation/e0a-pre-restructure-closure-native-arm64`; it does not validate comparison Harness source.

Source/test head `8ed1563e...`: cloud Validation `34179384123` **PASS** — ARM64 cross-compile of Core/Harness/test projects, x64 Core regression, repository laws, oracle coverage, document census. **Compiler/cloud-static only; Harness tests were not executed.**

Native Windows ARM64 validation of the comparison branch is **PENDING**.

## Provider boundary

Real Gemini credentials, provider `countTokens`, network execution, inference, and spend are **NOT AUTHORIZED**. Free-tier experiments are synthetic-fixture-only; RPD remains `unverified-pre-live`.

Approved profiles:
- `GEMINI-2.5-FLASH-LITE-NONE` — `CREATIVE-NONE`; 10 RPM / 250K TPM.
- `GEMINI-3.5-FLASH-LITE-MINIMAL` — `CREATIVE-MINIMAL`; 15 RPM / 250K TPM.
- `GEMINI-2.5-FLASH-NONE` — `CREATIVE-NONE`; 5 RPM / 250K TPM.

Harness paces every Gemini API operation and exact rolling generation-input TPM; one attempt / zero retries remains law. Transport admits only catalogued models. Gemini 3.5 opaque `thoughtSignature` metadata is stripped before streaming evidence and never enters semantic output; actual `thought=true` material remains rejected.

## Next

Live handoff: `docs/handoff/E0A_POST_ER1_FIRST_REAL_GEMINI_RUN_DIRECTOR_GATE_HANDOFF_2026_09_07.md`.

**Next gate: Director Windows ARM64 native validation on one exact clean documentation-inclusive active-branch checkout.** Run Core + Harness tests, fresh native Harness build, fixture smokes, and credentialless refusal for all three arm/profile pairs. No Gemini network request.

After native evidence, return to a separate Director live-run decision. Planned order: 2.5 Flash-Lite None -> 3.5 Flash-Lite Minimal -> 2.5 Flash None; each real run requires separate authorization.
