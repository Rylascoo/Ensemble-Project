# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. Frozen Blueprint 0.1 plus approved phase/patch specifications govern architecture. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy authority: `docs/PROJECT_AUTHORITY.md`.

## Current checkpoint

Runtime phase: **E0-A Experimental Harness — Phase B.** Current engineering work: **E-R1 repository/tooling/workflow restructuring** on branch `repo-restructure-e-r1`, baseline `db4bb1c5d5cb6f6de230da88f1c0352fb75aac23`.

Pre-restructure closure: PR #44 merge `372de76955407762b3f8b83fa2799a93a85e2f5c`; continuity PR #45 merge `db4bb1c5d5cb6f6de230da88f1c0352fb75aac23`.

Exact current machine-tested checkpoint: `cc395a25162a0a682796bffb44060c799df0db32`.
Tag: `validation/e0a-pre-restructure-closure-native-arm64`.
Evidence: `docs/evidence/E0A_PRE_RESTRUCTURE_CLOSURE_NATIVE_ARM64_VALIDATION.md`.

Director Windows ARM64: Core 622/622 PASS; Harness 103/103 PASS; fresh `win-arm64` Harness build and fixture smokes PASS; credentialless Gemini refusal PASS; no provider-network execution, inference, or spend. Cloud ARM64-target builds are compiler authority only; x64 Core tests are required regressions, not native-runtime authority.

## Provider boundary

Google Gemini API is the sole current E0-A provider method. Historical OpenAI executable support is retired. Only `CREATIVE-NONE` is authorized on the Gemini live path. Real credentials, `countTokens`, provider-network execution, inference, and spend remain **NOT AUTHORIZED**. Provider/account tier, availability, quota, pricing, and data-use terms require fresh verification before any authorized live run.

## E-R1 state

Governing work package: `docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md`.

Phase 0 **COMPLETE**: repository/branch/tag state reconciled; current solution/workflow/document surfaces inventoried; document-census false-authority mode identified and corrected in schema v2; full Validation gate PASS at `a686454105dc2661c752c351510884535209ea7c` (compiler, required Core regressions, oracle drift, document census).

Phase 1 **ACTIVE**: establish explicit navigation, validation-fact, hypothesis, and archive roles; then archive only evidence that remains demonstrably historical after the authority map is applied.

Completed branch `repo-pre-restructure-closure` is preserved by annotated tag `archive/repo-pre-restructure-closure` at `d9280cfce054f04954f17d1d5d0009f9efe0d607` and deleted. GitHub plan-dependent `main` protection remains deferred.

## Boundary / next

E-R1 may change docs, repository structure, static law checks, and CI/tooling. It does not authorize runtime/product/provider-policy changes, .NET 10, real Gemini execution, E0-B+, app/UI/persistence, Windows AI/NPU, packaging, WACK, Store, or design-lane decisions.

Next: complete E-R1 Phase 1 authority/document lifecycle, recursively audit it, then proceed to Phase 2 mechanical repository-law enforcement unless a new Director product/policy or external/native validation gate appears.
