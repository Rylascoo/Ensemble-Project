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
