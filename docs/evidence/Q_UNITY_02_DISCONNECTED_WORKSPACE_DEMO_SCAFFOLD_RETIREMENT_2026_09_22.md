# Q-UNITY-02 disconnected workspace/demo scaffold retirement

Date: 2026-09-22  
Status: **BOUNDED SIMPLIFICATION CANDIDATE / NO PRODUCT SEMANTIC CHANGE**

Exact base: `c41d07fcd8108fb57932b56db0c4a7061d15d533`; exact-main Validation #1184 PASS.

## Finding

Fresh source-reference reconciliation before opening a real persistent Character/Cast contract found a second, obsolete application model still compiled beside the integrated Product stack:

- `WorkspaceApplication`;
- `WorkspaceProjection` / `WorkspaceCharacter`;
- `IProductionStore`;
- `Kymaean.Infrastructure.Demo`;
- `MissingRaftDemoProductionStore`;
- their two Application-test classes and test-project fixture/reference plumbing.

Current executable Windows composition does not consume this surface. Repository search finds `WorkspaceApplication` only in its own source/tests and `WorkspaceCharacter` only in that disconnected demo projection. Existing runtime-composition evidence already records that the packaged Windows application contains Application + Persistence and **does not contain Kymaean.Infrastructure.Demo**.

Historical first-slice evidence explicitly classified the Missing Raft adapter as deterministic demo infrastructure, not production persistence or final ontology authority.

The active roadmap requires Alpha convergence to remove obsolete E0/product scaffolding.

## Disposition

Retire this disconnected **product demo/workspace** scaffold now, before introducing the real integrated Character/Cast model.

Delete:

- `src/Kymaean.Application/WorkspaceApplication.cs`;
- `src/Kymaean.Application/WorkspaceProjection.cs`;
- `src/Kymaean.Application/IProductionStore.cs`;
- `src/Kymaean.Infrastructure.Demo/**`;
- `tests/Kymaean.Application.Tests/WorkspaceApplicationTests.cs`;
- `tests/Kymaean.Application.Tests/MissingRaftDemoProductionStoreTests.cs`.

Also remove the Demo project from `Ensemble.sln`, Application-test references, and repository dependency law.

## Preserved boundaries

This package does **not**:

- delete or modify the canonical Missing Raft E0 fixture;
- alter `Ensemble.E0.Core` or Harness semantics;
- alter integrated `ProductApplication`, Production persistence/events/replay, Windows presentation, or Q-PROD-04/05 contracts;
- create Character, Scene, Performer, provider, lifecycle or UI semantics;
- rewrite historical evidence that accurately records the old first-slice scaffold.

The purpose is to prevent the upcoming real Character/Cast Product work from coexisting with a misleading second `WorkspaceCharacter` / Studio-Stage-Archive projection model in the shipping Application assembly.

## Acceptance

Require repository law/document census, Application/Persistence regression, ARM64 WinUI compiler gate, and native ARM64 Application/Persistence/build validation. Historical evidence remains historical; no runtime authority is inferred from this cleanup itself.

After integration, run Q-PROD-06 successor selection for the smallest real **persistent Character / Production Cast identity foundation** before any Scene-membership contract.

**APPROVED UNDER DIRECTOR Q-UNITY SIMPLIFICATION CONTINUATION.**

## Exact-head hosted validation

Exact executable candidate reviewed and validated: `31da99d240eb593697a18ae314f320473b1194ab`.

- PR Validation #1189: **PASS**.
- E0-E preparation #182: **PASS**.
- Validation jobs include repository law, document authority census, oracle coverage, Product Application regression, Product persistence regression, Core regression, and ARM64 cross-compile gates.
- Hosted validation remains compiler/regression evidence; it is not promoted into native ARM64 runtime authority.

## Native Windows ARM64 validation

Native validation ran on SurfSeven against detached exact source
`31da99d240eb593697a18ae314f320473b1194ab`.

- application environment verifier: **PASS** on ARM64 OS / ARM64 process;
- `tools/test-application.ps1 -Full`: **PASS** for native win-arm64 Application and Persistence regressions;
- Persistence regression observed: **135/135 PASS**;
- `Kymaean.Windows` Release `win-arm64` build: **PASS**, 0 warnings / 0 errors;
- full `Ensemble.sln` Release build: **PASS**, 0 warnings / 0 errors;
- `git diff --check`: **PASS**;
- final worktree status: **clean**.

This validation does not claim package registration, packaged runtime, WACK, Store, provider, or deferred-E0 behavior because Q-UNITY-02 does not change those surfaces and they were not exercised here.

## Independent exact-candidate review

Read-only review of exact candidate `31da99d...` is **CLEAN**.

The review confirmed:

- all executable deletions are confined to the disconnected `WorkspaceApplication` / `WorkspaceProjection` / `IProductionStore` / Demo surface and its direct tests/build-law references;
- current `ProductApplication` remains intact;
- current Windows composition references only `Kymaean.Application` + `Kymaean.Infrastructure.Persistence`;
- the canonical `fixtures/missing-raft/missing-raft-0.1.0.json` remains present and unchanged by this PR;
- `Ensemble.E0.Core` / Harness source is untouched;
- historical evidence that names `IProductionStore`, `WorkspaceProjection`, or `Kymaean.Infrastructure.Demo` remains preserved as historical evidence;
- no current Product contract or executable Windows dependency requires the removed types;
- the result reduces duplicate ontology/persistence ambiguity before real Character/Cast identity work.

The evidence-record commit that follows this review is documentation-only. Native evidence remains bound to executable source `31da99d...`; final PR integration still requires exact-head hosted validation after this record is committed.
