# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint

Runtime: **E0-A Experimental Harness — Phase B.** E-R1 is **CLOSED / PROMOTED / ARCHIVED** and must not be reopened. Promoted `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`.

Active branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture:
- `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`
- `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`
- `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md` — commit `76fc0c64da4724a7a352a656a062d5c3ed431ad6`.

Audited executable/test checkpoint: `277dcb5e3bdd99ef02df4bd91d3df309d4c707ef`. Evidence: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`. From `main`: 16 ahead / 0 behind; no Core, Core-test, or fixture delta.

## Validation

Promoted native authority: `cc395a25162a0a682796bffb44060c799df0db32`, tag `validation/e0a-pre-restructure-closure-native-arm64`: Core 622/622, Harness 103/103, native build/smokes and credentialless predecessor Gemini refusal PASS; no provider network/inference/spend.

Checkpoint `277dcb5e...`: cloud Validation `34177033966` PASS — ARM64 cross-compile of Core/Harness/test projects, x64 Core regression, repository laws, oracle coverage, document census. **Not native runtime validation; Harness tests were not executed.**

Native Windows ARM64 validation of the comparison branch is **PENDING**.

## Provider boundary

Real Gemini network execution, provider `countTokens`, inference, and spend are **NOT AUTHORIZED**. Free-tier experiments are synthetic-fixture-only. RPD is `unverified-pre-live`. The exposed key was replaced; never record its replacement value.

Approved profiles:
- `GEMINI-2.5-FLASH-LITE-NONE` — `CREATIVE-NONE`; 10 RPM / 250K TPM; true thinking-off creative roles.
- `GEMINI-3.5-FLASH-LITE-MINIMAL` — `CREATIVE-MINIMAL`; 15 RPM / 250K TPM; minimal, not zero, creative thinking.
- `GEMINI-2.5-FLASH-NONE` — `CREATIVE-NONE`; 5 RPM / 250K TPM; true thinking-off creative roles.

Harness paces every Gemini API operation, including `countTokens`, and enforces exact rolling generation-input TPM. One attempt / zero retries remains law. Comparison measures quality, latency, token use, yield, and shadow cost.

## Next

Live handoff: `docs/handoff/E0A_POST_ER1_FIRST_REAL_GEMINI_RUN_DIRECTOR_GATE_HANDOFF_2026_09_07.md`.

**Next gate: Director Windows ARM64 native validation of exact clean active-branch checkout.** Run Core + Harness tests, fresh native Harness build, fixture smokes, and credentialless refusal for all three approved arm/profile pairings. No Gemini network request.

After native evidence, return to a separate Director live-run decision. Planned order: 2.5 Flash-Lite None -> 3.5 Flash-Lite Minimal -> 2.5 Flash None. Each real run requires separate authorization.
