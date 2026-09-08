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
Real Gemini credentials/network/`countTokens`/inference/spend are **NOT AUTHORIZED**. Free-tier work is synthetic-fixture-only. RPD: `unverified-pre-live`.

Profiles: `GEMINI-2.5-FLASH-LITE-NONE` = `CREATIVE-NONE`, 10 RPM/250K TPM; `GEMINI-3.5-FLASH-LITE-MINIMAL` = `CREATIVE-MINIMAL`, 15 RPM/250K TPM; `GEMINI-2.5-FLASH-NONE` = `CREATIVE-NONE`, 5 RPM/250K TPM. Harness paces every Gemini API operation and exact rolling generation-input TPM; one attempt/zero retries. Gemini 3.5 `thoughtSignature` is stripped before evidence/semantics; `thought=true` is rejected.

## Next
**Separate Director first-real-provider decision for exactly one synthetic Missing Raft run using `GEMINI-2.5-FLASH-LITE-NONE`.**

Before authorization, reverify intended AI Studio project: replacement key type/association, billing tier/status, model availability, active RPM/input-TPM/RPD, pricing/data-use terms/snapshot freshness, exact executable checkout, and evidence destination. Then recursively audit and request explicit Director authorization.

Do not bundle later arms. Planned order: 2.5 Flash-Lite None -> 3.5 Flash-Lite Minimal -> 2.5 Flash None; each requires separate authorization.
