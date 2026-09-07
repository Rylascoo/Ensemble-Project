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

Phase 0 **COMPLETE**: baseline/tooling/document assumptions reconciled.

Phase 1 **COMPLETE** at `0b267f9eadec603e934b55283c6d2801953a87ae`; gate `34168768724` PASS; active evidence authority-root reachability clean.

Phase 2 **COMPLETE** at `5723d567291860e9c9192976a82a4f53a70ece4e`; gate `34169337847` PASS. Project graph, ARM64 Harness identity, warnings/determinism, current-state cap, retired OpenAI source/test surface, scaffolding, handoff authority, oracle coverage, and evidence reachability are CI-blocking.

Phase 3 **COMPLETE** at `e5df6381513b1730cfc7db895e8cb23462567ba0`; gate `34169478970` PASS. Minimal artifact lifecycle, historical/non-authoritative navigation, and temporary-handoff rules are normalized without mass-rewriting history.

Phase 4 **ACTIVE**: reviewed GitHub Actions are being migrated from deprecated Node-20 major tags to exact Node-24 commit pins; .NET remains 9.0.317.

Pre-restructure branch is archived at `archive/repo-pre-restructure-closure`; plan-dependent `main` protection remains deferred.

## Boundary / next

E-R1 may change docs, repository structure, static checks, and CI/tooling. It does not authorize runtime/provider-policy changes, .NET 10, real Gemini execution, E0-B+, app/UI/persistence, Windows AI/NPU, packaging, WACK, Store, or design-lane decisions.

Next: finish/falsify Phase 4 CI hygiene, then enter Phase 5 recursive closure and promotion audit.
