# Ensemble Current State

Updated: 2026-09-07

## Authority
`Rylascoo/Ensemble-Project` is engineering authority. `CURRENT_STATE.md` alone carries active phase/checkpoint/validation/next-action authority. Product/policy authority: `docs/PROJECT_AUTHORITY.md`.

## Checkpoint
Runtime: **E0-A Experimental Harness — Phase B**. E-R1 is **CLOSED / PROMOTED / ARCHIVED**. Promoted `main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`.

Active work branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture: reference envelope + Gemini normative amendment + comparison amendment `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md` (`76fc0c64...`). Cloud-validated source/test checkpoint: `8ed1563ec7a4c3ae919a649db0dc3c29e17a03e0`; no Core/Core-test/fixture delta from `main` through that checkpoint.

## Validation
**Current promoted machine-tested checkout:** `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` / annotated tag `validation/e0a-gemini-comparison-native-arm64`.

Director Windows ARM64 PASS: Core 622/622, Harness 117/117, fresh native Harness build, Missing Raft + generic fixture smokes, and expected missing-key refusal with no evidence root for all three approved profiles. Evidence: `docs/evidence/E0A_GEMINI_COMPARISON_NATIVE_ARM64_VALIDATION_2026_09_07.md`.

Cloud source/test gate `34179384123` PASS; later documentation continuity does not extend native authority beyond the tagged checkout. No provider network, `countTokens`, inference, credential use, or spend occurred during validation.

## Provider boundary
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier experiments remain synthetic-fixture-only. RPD remains `unverified-pre-live`.

Profiles: `GEMINI-2.5-FLASH-LITE-NONE` = `CREATIVE-NONE`, 10 RPM/250K TPM; `GEMINI-3.5-FLASH-LITE-MINIMAL` = `CREATIVE-MINIMAL`, 15 RPM/250K TPM; `GEMINI-2.5-FLASH-NONE` = `CREATIVE-NONE`, 5 RPM/250K TPM. Harness paces every Gemini API operation plus exact rolling generation-input TPM; one attempt/zero retries. Gemini 3.5 opaque `thoughtSignature` is stripped before evidence/semantic output; `thought=true` remains rejected.

## Next
**Next gate: separate Director first-real-provider decision for exactly one synthetic Missing Raft run using `GEMINI-2.5-FLASH-LITE-NONE`.**

Before authorization, reverify in the intended AI Studio project: replacement key type/association, billing tier/status, exact model availability, active RPM/input-TPM/RPD, pricing/data-use terms and snapshot freshness; also fix the exact executable checkout/evidence destination. Then perform one recursive gate audit and request explicit Director authorization.

Do not bundle the 3.5 or 2.5 Flash comparison arms into the first live authorization. Planned order remains 2.5 Flash-Lite None -> 3.5 Flash-Lite Minimal -> 2.5 Flash None, with separate authorization for each.
