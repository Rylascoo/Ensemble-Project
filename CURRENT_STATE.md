# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. Frozen Blueprint 0.1 plus approved phase/patch specifications govern architecture. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy authority: `docs/PROJECT_AUTHORITY.md`.

## Current checkpoint

Runtime: **E0-A Experimental Harness — Phase B.** Engineering: **E-R1 repository/tooling/workflow restructuring** on `repo-restructure-e-r1`, baseline `db4bb1c5d5cb6f6de230da88f1c0352fb75aac23`.

Current E0-A architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md` plus `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`.

Machine-tested checkout: `cc395a25162a0a682796bffb44060c799df0db32`; tag `validation/e0a-pre-restructure-closure-native-arm64`; evidence `docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md`. Director Windows ARM64: Core 622/622, Harness 103/103, fresh build/smokes and credentialless Gemini refusal PASS; no provider network/inference/spend.

## Provider boundary

Google Gemini API is the sole current E0-A provider method; historical OpenAI executable support is retired. Only `CREATIVE-NONE` is authorized on the Gemini live path. Real credentials, `countTokens`, provider-network execution, inference, and spend remain **NOT AUTHORIZED**. Volatile provider facts require fresh verification before any authorized live run.

## E-R1

Contract: `docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md`.
Closure evidence: `docs/evidence/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE_EVIDENCE.md`.

Phases 0–3 **COMPLETE**. Phase 2 repository laws and Phase 3 artifact/lifecycle normalization are CI-blocking where objective.

Phase 4 **COMPLETE** at `fd655bae81064378a0c58fc627e43dbdd88fef58`; gate `34169804442` PASS. Reviewed Node-24 GitHub Actions are exact-SHA pinned; future remote action refs must also be full SHAs. .NET remains 9.0.317.

Phase 5 **ACTIVE — PROMOTION PENDING**. Recursive closure audit found E-R1 30 commits ahead / 0 behind its exact baseline, with no `src/`, `tests/`, or `fixtures/` delta. Current native authority therefore remains `cc395a…`. Active evidence authority is clean; no provider execution occurred.

Pre-restructure branch is archived at `archive/repo-pre-restructure-closure`; plan-dependent `main` protection remains deferred.

## Boundary / next

E-R1 does not authorize runtime/provider-policy changes, .NET 10, real Gemini execution, E0-B+, app/UI/persistence, Windows AI/NPU, packaging, WACK, Store, or design-lane decisions.

Next: promote E-R1 through reviewable PR(s), reconcile post-merge continuity, then archive-tag and retire `repo-restructure-e-r1`. Real Gemini execution remains separately gated and unauthorized.
