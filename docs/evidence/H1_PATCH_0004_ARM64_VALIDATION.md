# H1 Patch 0004 — ARM64 Validation Evidence

Date: 2026-09-02
Patch: H1 Patch 0004 — Deterministic Character-Bounded Access Control
Canonical blueprint: `docs/blueprint/H1_PATCH_0004_DETERMINISTIC_ACCESS_CONTROL.md`
Implementation branch: `h1-patch-0004-access-control-implementation`
Machine-tested executable head: `b228cd134f8e3258bb54fcb8e8f1fb01c21b96f8`

## Authority

This record preserves evidence supplied from the user's native Windows ARM64 development machine.

Validation hierarchy remains:

- static review: advisory only;
- native Visual Studio / `dotnet` ARM64 build output: compiler authority;
- `dotnet test` on the target machine: test-execution authority for exercised tests;
- actual target-device execution: runtime authority for exercised Harness behavior;
- NPU execution requires separate explicit hardware evidence;
- WACK and Partner Center remain separate later authorities.

Documentation commits after the machine-tested executable head do not increase executable validation authority.

## Exact tested source

The user fetched and switched to:

`h1-patch-0004-access-control-implementation`

`git rev-parse HEAD` returned exactly:

`b228cd134f8e3258bb54fcb8e8f1fb01c21b96f8`

The branch was reported already up to date with origin before validation.

## Native ARM64 compiler gate

Command:

`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Result: **PASS**

Observed outputs:

- `Ensemble.E0.Core` succeeded -> `src\Ensemble.E0.Core\bin\Debug\net9.0\Ensemble.E0.Core.dll`
- `Ensemble.E0.Harness` succeeded -> `src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll`
- build succeeded with no reported warning/error failure.

This establishes compiler authority for the tested source and confirms the Harness build targeted native `win-arm64`.

## Core test gate

Command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Result: **PASS**

Observed summary:

- total: `90`
- succeeded: `90`
- failed: `0`
- skipped: `0`

This includes the prior 73-test baseline plus the Patch 0004 access-control cases covering the approved category/ownership matrix, provenance-leak exclusion, exact Missing Raft permitted sets, directional relationships, deterministic ordering, complete decision coverage, fixture/hash immutability, fail-closed inputs, complete Character-owned category coverage, and non-forgeable public projection authority.

## Canonical Missing Raft runtime regression

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json`

Observed:

`Fixture validated: ensemble.e0.missing-raft@0.1.0`

Exit code: `0`

Result: **PASS**

The frozen Missing Raft source and ECJ-1/SHA-256 identity therefore remained compatible with the Patch 0004 implementation under this exercised Harness path.

## Generic smoke runtime regression

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json`

Observed:

`Fixture validated: ensemble.e0.smoke@0.1.0`

Exit code: `0`

Result: **PASS**

Unknown/generic fixture-family validation behavior remains intact under the exercised Harness path.

## Static/adversarial review

Status: **PASS — advisory only**.

Final comparison against approved Patch 0004 baseline shows exactly four executable/test files added:

1. `src/Ensemble.E0.Core/Access/CharacterAccessModels.cs`
2. `src/Ensemble.E0.Core/Access/CharacterBoundedAccessControl.cs`
3. `tests/Ensemble.E0.Core.Tests/Access/CharacterBoundedAccessControlBoundaryTests.cs`
4. `tests/Ensemble.E0.Core.Tests/Access/CharacterBoundedAccessControlTests.cs`

No pre-existing executable source, fixture JSON, schema, ECJ-1/hash implementation, Harness CLI, or project configuration was modified by the executable implementation.

Reviewed properties:

- access depends only on Production authority category, structural Character ownership, Scene roster, and the single known E0 access contract;
- Production `HistoricalTruth`, `UnresolvedProposition`, `WorldState`, chronology, and every other Character's private records are absent from the Character-facing projection;
- shared `SceneState`, public root `Pressure`, roster identities, and the subject's own records/outbound Relationships are projected;
- provenance is never traversed to enlarge access;
- Character-facing projection records contain no provenance or authoritative `Validated*` object references;
- safe projection/audit constructors are Core-internal, preventing external assemblies from fabricating Access Control authority objects through public constructors;
- access-decision audit remains separate from Character-facing projection and carries no record text/provenance;
- deterministic ordering uses stable IDs and is invariant to source reordering for semantically unordered collections;
- Access Control does not mutate `ValidatedFixture` or change the frozen Missing Raft hash;
- no ACL language/framework, policy registry, omniscient bypass, Context Composer logic, Director logic, persistence, provider/AI, WinUI, NPU, packaging, WACK, or Store scope was introduced.

## Validation conclusion

H1 Patch 0004 satisfies its exercised native ARM64 compiler, Core test, and Harness regression gates at executable head:

`b228cd134f8e3258bb54fcb8e8f1fb01c21b96f8`

The evidence supports promotion of Patch 0004, subject to final repository hygiene/promotion checks.

## Explicitly not established

This evidence does **not** establish:

- Context Composer behavior or ContextPacketHash;
- ProductionState/StateHash;
- Director or opportunity orchestration;
- E0-D omniscient/relationship-ablation execution;
- Performer/provider/model behavior;
- Integrity Validator, State Interpreter, State Authority, or causal commits;
- Windows AI or NPU execution/performance;
- WinUI behavior;
- packaging/WACK;
- Microsoft Store certification.
