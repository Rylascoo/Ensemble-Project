# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. Frozen Blueprint 0.1 plus approved phase/patch specifications govern architecture. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy authority: `docs/PROJECT_AUTHORITY.md`.

## Current checkpoint

Runtime: **E0-A Experimental Harness — Phase B.** E-R1 repository/tooling/workflow restructuring is **COMPLETE / PROMOTED / ARCHIVED**. Promotion: PR #46 merge `21a10aff823734418f36284744a1fd26aef3bcf6`; continuity: PR #47 merge `139621dc63e1a67aca5f1a5eedb7c1347ede9fd5`; final branch head `bec3db07cef41296855be8f702d2f3637c472dc1` is preserved by `archive/repo-restructure-e-r1`; work branch retired.

Current E0-A architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md` plus `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`.

Machine-tested checkout: `cc395a25162a0a682796bffb44060c799df0db32`; tag `validation/e0a-pre-restructure-closure-native-arm64`; evidence `docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md`. Director Windows ARM64: Core 622/622, Harness 103/103, fresh build/smokes and credentialless Gemini refusal PASS; no provider network/inference/spend.

## Provider boundary

Google Gemini API is the sole current E0-A provider method; historical OpenAI executable support is retired. Only `CREATIVE-NONE` is authorized on the Gemini live path. Real credentials, `countTokens`, provider-network execution, inference, and spend remain **NOT AUTHORIZED**. Volatile provider facts require fresh verification before any authorized live run.

## E-R1 closure

Contract: `docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md`.
Evidence: `docs/evidence/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE_EVIDENCE.md`.

Phases 0–5 are **COMPLETE**. Audited implementation/tooling head `fd655bae81064378a0c58fc627e43dbdd88fef58`; gate `34169804442` PASS. Documentation-inclusive branch/PR gates `34170023136` and `34170086319` PASS. Final promoted `main` gate `34170749305` PASS. E-R1 contained no `src/`, `tests/`, or `fixtures/` delta, so native authority remains `cc395a…`.

Plan-dependent `main` protection remains deferred.

## Boundary / next

E-R1 does not authorize runtime/provider-policy changes, .NET 10, real Gemini execution, E0-B+, app/UI/persistence, Windows AI/NPU, packaging, WACK, Store, or design-lane decisions.

Live fresh-chat transition: `docs/handoff/E0A_POST_ER1_FIRST_REAL_GEMINI_RUN_DIRECTOR_GATE_HANDOFF_2026_09_07.md`.

Next consequential gate: Director disposition/authorization of the first real Gemini `CREATIVE-NONE` run. Until explicit authorization, no provider-network execution, inference, or spend may occur.
