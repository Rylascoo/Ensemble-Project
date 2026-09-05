# Patch 0016 Evidence — Synchronized Causal Cycle

Status: **IMPLEMENTED / STATIC AUDIT CLEAN / NATIVE ARM64 VALIDATED FOR EXERCISED CORE/TEST/HARNESS/FIXTURE GATES**

Date: 2026-09-05

## Authority

Parent `main`: `99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

Approved architecture: `docs/blueprint/H1_PATCH_0016_SYNCHRONIZED_CAUSAL_CYCLE.md`, Proposal 0.5.

Director approval: 2026-09-05 project-conversation continuation after the clean Proposal 0.5 audit/recommendation.

Machine-readable inherited oracle: `docs/evidence/PATCH_0016_ORACLE.json`.

## Falsification

Patch 0016 would be unnecessary if production source already owned the synchronized Production/accepted-history/Opportunity state and advanced one accepted causal cycle without caller stitching.

It did not. Patch 0015 test support manually sequenced checkpoint -> Context -> Accepted Take binding -> causal commit -> accepted-history advancement -> Opportunity establishment -> synchronized next-turn history.

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

The implementation preserves two adoption boundaries:

```text
Opportunity-bearing synchronized state
 -> bounded Context
 -> externally prepared Accepted Take
 -> atomic commit + accepted-history proof
 -> valid no-Opportunity postcommit state
 -> deterministic Opportunity + history coupling
 -> next synchronized Opportunity-bearing state
```

Cycle states use private constructors. Internal factories freshly prove cross-authority synchronization. The only additional nonpublic Cycle type is `E0CausalCycleInvariantException`.

The public Cycle boundary emits fixed structural errors without lower `InnerException` chains. Rejected/Alternate Takes cannot cross commit adoption. A phase-two Opportunity failure cannot erase or mutate the already valid postcommit predecessor.

## Five-property audit

- **Character continuity:** accepted Character-legible history advances only through existing causal commit/continuity authority.
- **Bounded perspective:** Context remains `Production -> Access Control -> permitted projection -> Context Composer`.
- **Agency without hidden authorship:** Cycle creates no Candidate content, Take disposition, consequence decision, or Director outcome.
- **Causal persistence:** accepted Performance + approved consequences remain atomic; phase-two failure cannot roll them back.
- **Creator sovereignty:** review policy, Take choice, IDs/materializations, provider attempts, retry/spend, and later recovery remain outside Cycle authority.

Static recursive audit reached a zero-material-correction pass after three implementation corrections: missing test import; closed state construction; removal of redundant invariant-helper authority. A later mixed-token test was added without changing production semantics.

## Tests/oracles

Coverage includes:

- exact public surface and phase signatures;
- no provider/platform/persistence/allocation public contract;
- exact Missing Raft genesis v2 Context lineage;
- exact Patch 0015 postcommit and Opportunity StateHashes;
- exact next MARLOWE v3 Context hashes;
- Rejected/Alternate fail-closed behavior;
- stale prior Context/Take rejection;
- mixed otherwise-valid state/history tokens rejected through the public Context boundary;
- deterministic replay from identical explicit inputs;
- three-cycle `VOSS -> MARLOWE -> WREN -> VOSS` recurrence;
- phase-two failure preserving the valid postcommit predecessor.

Inherited reference-oracle assertions were preserved.

## Native Windows ARM64 validation

Machine authority supplied by the Director:

```text
PROCESSOR_ARCHITECTURE = ARM64
OS = Windows 10.0.26200
RID = win-arm64
repository-selected .NET SDK = 9.0.317
.NET host = 10.0.11 arm64
```

### Attempt 01

Exact head: `09bf644f75de875870ba3d625dd381d83e4ff4c8`

Observed:

- tracked/staged diff clean;
- Core production compilation: PASS during `dotnet test`;
- Core test-project compilation: FAIL with three test-only accessibility errors in `E0CausalCycleDeterminismTests.cs`;
- `CORE_TEST_EXIT=1`;
- Harness Debug build: PASS, `HARNESS_BUILD_EXIT=0`;
- Missing Raft fixture: PASS, `MISSING_RAFT_EXIT=0`;
- generic smoke fixture: PASS, `GENERIC_SMOKE_EXIT=0`.

Root cause: the new test directly referenced internal Cycle members/types from the separate test assembly. Production semantics were not implicated.

Patch-first correction commit: `d199a1ea2f38a658c226b8191c29ba296c69748a`.

The correction changed only `tests/Ensemble.E0.Core.Tests/Cycle/E0CausalCycleDeterminismTests.cs`: reflection is now construction plumbing only, and rejection is proven through public `DeterministicE0CausalCycle.ComposeContext(...)`. No `src/`, Harness, fixture, framework, SDK, canonicalizer, or oracle change occurred.

### Attempt 02 — successful Core gate

Exact machine-tested head:

`aa1346964aeb0f27b9d9ff609514150f438d4474`

Observed before test execution:

```text
git rev-parse HEAD = aa1346964aeb0f27b9d9ff609514150f438d4474
TRACKED_DIFF_EXIT=0
STAGED_DIFF_EXIT=0
```

Observed `dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`:

```text
Ensemble.E0.Core succeeded
Ensemble.E0.Core.Tests succeeded
Test summary: total: 584, failed: 0, succeeded: 584, skipped: 0
Build succeeded
CORE_TEST_EXIT=0
```

This establishes native Windows ARM64 compiler and full Core-test execution authority for the exercised Patch 0016 source/test tree at `aa134696...`.

Because the only executable/test change after Attempt 01 was the single test-file correction above, the successful Harness build and both fixture runtime executions observed at `09bf644f...` remain applicable to unchanged `src/`, Harness, and fixtures.

## Validation conclusion

For exercised Patch 0016 gates:

- Core production compilation: **PASS**;
- Core test-project compilation: **PASS**;
- full Core suite: **584/584 PASS**;
- Harness ARM64 build: **PASS**;
- Missing Raft fixture runtime: **PASS**;
- generic smoke fixture runtime: **PASS**.

Not established by this patch: WinUI runtime behavior, Windows AI/NPU execution, measured TOPS, MSIX/WACK, or Store certification.

## Advisory findings retained

- Structural-test review: exact-main GitHub search resolves `BindingFlags` in 26 test files; disputed literal totals 202 vs 456 remain unpromoted until counted from an executable current checkout.
- E5c: no authority-sensitive production path has been found that accepts an exception from an untrusted boundary and treats runtime exception type as provenance. Status remains open question / no verified defect / no fix authorized.
