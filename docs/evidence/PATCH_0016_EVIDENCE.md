# Patch 0016 Evidence — Synchronized Causal Cycle

Status: **IMPLEMENTED / STATIC AUDIT CLEAN / NATIVE ARM64 VALIDATION PENDING**

Date: 2026-09-05

## Authority

Parent `main`: `99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

Approved architecture: `docs/blueprint/H1_PATCH_0016_SYNCHRONIZED_CAUSAL_CYCLE.md`, Proposal 0.5.

Director approval: 2026-09-05 project-conversation continuation after the clean Proposal 0.5 audit/recommendation.

Executable/test static checkpoint before evidence-only commits: `dfbbe4ea349dada682be28a4691adb741535a91b` on `h1-patch-0016-causal-cycle-implementation`.

Machine-readable inherited oracle: `docs/evidence/PATCH_0016_ORACLE.json`.

## Falsification

Patch 0016 would be unnecessary if production source already owned the synchronized Production/accepted-history/Opportunity state and advanced one accepted causal cycle without caller stitching.

It did not. Patch 0015 test support manually sequenced checkpoint -> Context -> Accepted Take binding -> causal commit -> accepted-history commit advancement -> Opportunity establishment -> accepted-history/Opportunity advancement.

## Implemented surface

Added only:

```text
src/Ensemble.E0.Core/Cycle/E0CausalCycleModels.cs
src/Ensemble.E0.Core/Cycle/DeterministicE0CausalCycle.cs
tests/Ensemble.E0.Core.Tests/Cycle/E0CausalCycleContractTests.cs
tests/Ensemble.E0.Core.Tests/Cycle/E0CausalCycleTests.cs
tests/Ensemble.E0.Core.Tests/Cycle/E0CausalCycleDeterminismTests.cs
```

No existing Patch 0015 production source changed. No Harness source changed. No framework/SDK/model retarget occurred.

Public Cycle surface is exactly five types and four phase-typed operations: `Initialize`, `ComposeContext`, `CommitAcceptedTake`, `EstablishOpportunity`.

## Authority behavior

The implementation preserves two explicit adoption boundaries:

```text
Opportunity-bearing synchronized state
 -> bounded Context
 -> externally prepared Accepted Take
 -> atomic commit + accepted-history proof
 -> validated no-Opportunity postcommit state
 -> deterministic Opportunity + history coupling
 -> next synchronized Opportunity-bearing state
```

Cycle states use private constructors. Their internal factories freshly prove cross-authority synchronization before construction. The only additional nonpublic Cycle type is `E0CausalCycleInvariantException`.

The public Cycle boundary emits fixed structural errors without retaining lower `InnerException` chains. Rejected/Alternate Takes cannot cross the first boundary. A phase-two Opportunity failure cannot erase or mutate the already valid postcommit predecessor.

## Five-property audit

- **Character continuity:** accepted Character-legible history advances only through existing causal commit/continuity authority.
- **Bounded perspective:** Context remains `Production -> Access Control -> permitted projection -> Context Composer`.
- **Agency without hidden authorship:** Cycle creates no Candidate content, Take disposition, consequence decision, or Director outcome.
- **Causal persistence:** accepted Performance + approved consequences remain atomic; phase-two failure cannot roll them back.
- **Creator sovereignty:** review policy, Take choice, IDs/materializations, provider attempts, retry/spend, and later recovery remain outside Cycle authority.

Result: static recursive audit found no remaining material correction or worthwhile in-scope simplification.

## Tests/oracles added

Static test design covers:

- exact public surface and phase signatures;
- no provider/platform/persistence/allocation public contract;
- exact Missing Raft genesis v2 Context lineage;
- exact Patch 0015 postcommit and Opportunity StateHashes;
- exact next MARLOWE v3 Context hashes;
- Rejected/Alternate fail-closed behavior;
- stale prior Context/Take rejection;
- deterministic replay from identical explicit inputs;
- three-cycle `VOSS -> MARLOWE -> WREN -> VOSS` recurrence;
- phase-two failure preserving the valid postcommit predecessor.

Inherited reference values were copied from the frozen Patch 0015 reference oracle; no lower oracle assertion was removed or weakened.

## Recursive corrections made before the clean pass

1. Added the missing `Ensemble.E0.Core.Fixture` import in the determinism tests.
2. Replaced bypassable internal state constructors with private constructors plus validated internal factories.
3. Removed the extra invariant-helper type and moved validation ownership into the closed state types, leaving one internal invariant exception only.

Each material correction restarted the audit from the affected authority layer.

## Advisory side findings retained

Structural tests: current exact-main GitHub code search resolves `BindingFlags` in 26 test files. Literal-use totals 202 vs 456 were not independently reproduced by an executable checkout here, so neither count is promoted as fact or used for Patch 0016 decisions. Future structural-test work must count the actual checkout and classify by member visibility/assertion semantics, not filename.

E5c: no authority-sensitive production path was found that accepts an exception from an untrusted boundary and treats runtime type as provenance. Status remains **open question / no verified defect / no fix authorized**.

## Validation authority

Not claimed here: compilation, test execution, Harness runtime, native ARM64 behavior, WinUI, Windows AI/NPU, package/WACK, or Store certification.

The available execution environment has no usable local .NET compiler/runtime for this repository. Native Windows ARM64 remains the next gate.

Run from the repository root on the Director's native Windows ARM64 machine, after switching to the exact implementation branch/head and confirming tracked/staged cleanliness:

```powershell
$env:PROCESSOR_ARCHITECTURE
dotnet --info
git rev-parse HEAD
git diff --quiet; "TRACKED_DIFF_EXIT=$LASTEXITCODE"
git diff --cached --quiet; "STAGED_DIFF_EXIT=$LASTEXITCODE"
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
"CORE_TEST_EXIT=$LASTEXITCODE"
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
"HARNESS_BUILD_EXIT=$LASTEXITCODE"
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
"MISSING_RAFT_EXIT=$LASTEXITCODE"
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
"GENERIC_SMOKE_EXIT=$LASTEXITCODE"
```

Only observed output from that gate may promote Patch 0016 to compiler/runtime/native authority.
