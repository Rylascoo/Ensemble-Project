# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. Frozen Blueprint 0.1 plus approved phase/patch specifications govern architecture. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy authority: `docs/PROJECT_AUTHORITY.md`.

## Current checkpoint

Runtime: **E0-A Experimental Harness — Phase B.** Engineering: **E-R1 repository/tooling/workflow restructuring** on `repo-restructure-e-r1`, baseline `db4bb1c5d5cb6f6de230da88f1c0352fb75aac23`.

Current E0-A architecture: `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md` plus `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`.

Exact machine-tested checkout: `cc395a25162a0a682796bffb44060c799df0db32`; tag `validation/e0a-pre-restructure-closure-native-arm64`; evidence `docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md`.

Director Windows ARM64: Core 622/622 PASS; Harness 103/103 PASS; fresh `win-arm64` build and fixture smokes PASS; credentialless Gemini refusal PASS; no provider-network execution, inference, or spend. Cloud ARM64-target builds are compiler authority only; x64 Core tests are required regressions, not native-runtime authority.

## Provider boundary

Google Gemini API is the sole current E0-A provider method; historical OpenAI executable support is retired. Only `CREATIVE-NONE` is authorized on the Gemini live path. Real credentials, `countTokens`, provider-network execution, inference, and spend remain **NOT AUTHORIZED**. Provider/account tier, availability, quota, pricing, and data-use terms require fresh verification before any authorized live run.

## E-R1

Contract: `docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md`.

Phase 0 **COMPLETE**: repository/tooling/document baseline reconciled; census false-authority modes identified; compiler, required Core regressions, oracle guard, and census passed.

Phase 1 **ACTIVE**: navigation/validation/hypothesis roles are separated; historical evidence is moving to `docs/evidence/archive/`; document authority is being falsified by reachability from durable roots rather than raw inbound-link counts.

Completed pre-restructure branch is archived at `archive/repo-pre-restructure-closure`; GitHub plan-dependent `main` protection remains deferred.

## Boundary / next

E-R1 may change docs, repository structure, static law checks, and CI/tooling. It does not authorize runtime/product/provider-policy changes, .NET 10, real Gemini execution, E0-B+, app/UI/persistence, Windows AI/NPU, packaging, WACK, Store, or design-lane decisions.

Next: close Phase 1 only after recursive authority-reachability audit finds no historical evidence stranded on the active surface; then enter Phase 2 mechanical repository-law enforcement.
