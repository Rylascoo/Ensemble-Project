# Patch 0016 Evidence — Synchronized Causal Cycle

Status: **IMPLEMENTED / NATIVE ARM64 ATTEMPT 01 PARTIAL PASS / TEST-ONLY CORRECTION PENDING REVALIDATION**

Date: 2026-09-05

## Authority

Parent `main`: `99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

Approved architecture: `docs/blueprint/H1_PATCH_0016_SYNCHRONIZED_CAUSAL_CYCLE.md`, Proposal 0.5.

Director approval: 2026-09-05 project-conversation continuation after the clean Proposal 0.5 audit/recommendation.

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

## Tests/oracles added

Test design covers:

- exact public surface and phase signatures;
- no provider/platform/persistence/allocation public contract;
- exact Missing Raft genesis v2 Context lineage;
- exact Patch 0015 postcommit and Opportunity StateHashes;
- exact next MARLOWE v3 Context hashes;
- Rejected/Alternate fail-closed behavior;
- stale prior Context/Take rejection;
- mixed otherwise-valid state/history tokens fail closed through the public Context boundary;
- deterministic replay from identical explicit inputs;
- three-cycle `VOSS -> MARLOWE -> WREN -> VOSS` recurrence;
- phase-two failure preserving the valid postcommit predecessor.

Inherited reference values were copied from the frozen Patch 0015 reference oracle; no lower oracle assertion was removed or weakened.

## Recursive corrections before machine validation

1. Added the missing `Ensemble.E0.Core.Fixture` import in the determinism tests.
2. Replaced bypassable internal state constructors with private constructors plus validated internal factories.
3. Removed the extra invariant-helper type and moved validation ownership into the closed state types, leaving one internal invariant exception only.
4. Added an explicit mixed-token fail-closed case.

Each material correction restarted the audit from the affected authority layer.

## Native Windows ARM64 validation attempt 01

Exact attempted head:

`09bf644f75de875870ba3d625dd381d83e4ff4c8`

Observed machine authority supplied by the Director:

- `PROCESSOR_ARCHITECTURE=ARM64`;
- Windows `10.0.26200`;
- RID `win-arm64`;
- repository-selected .NET SDK `9.0.317`;
- .NET host `10.0.11`, architecture `arm64`;
- tracked diff clean: `TRACKED_DIFF_EXIT=0`;
- staged diff clean: `STAGED_DIFF_EXIT=0`.

Core production compiled successfully during `dotnet test`, but the test project failed compilation with exactly three errors, all in `E0CausalCycleDeterminismTests.cs`:

```text
CS0122 E0CausalCycleInvariantException is inaccessible
CS1061 E0OpportunityBearingCycleState.AcceptedPerformanceHistory is inaccessible
CS1061 E0OpportunityBearingCycleState.OpportunityHistory is inaccessible
```

`CORE_TEST_EXIT=1`.

Harness/runtime gates at the same exact head passed:

- Harness build: PASS, `HARNESS_BUILD_EXIT=0`;
- Missing Raft fixture: PASS, `MISSING_RAFT_EXIT=0`;
- generic smoke fixture: PASS, `GENERIC_SMOKE_EXIT=0`.

Therefore attempt 01 establishes native ARM64 Core **production compilation**, Harness compilation, and both fixture runtime validations for the executable source at `09bf644f...`, but does not establish Core test-project compilation or Core test execution.

## Patch-first correction after attempt 01

Correction commit:

`d199a1ea2f38a658c226b8191c29ba296c69748a`

Changed executable/test surface from attempted head `09bf644f...`:

- one test file only: `tests/Ensemble.E0.Core.Tests/Cycle/E0CausalCycleDeterminismTests.cs`.

No `src/`, Harness, fixture, framework, SDK, canonicalizer, or oracle change was made.

The mixed-token test now uses reflection only as test construction plumbing to forge an otherwise unreachable mixed state, then proves rejection through the public `DeterministicE0CausalCycle.ComposeContext(...)` boundary. It no longer references any internal Cycle type/member at compile time.

## Advisory side findings retained

Structural tests: current exact-main GitHub code search resolves `BindingFlags` in 26 test files. Literal-use totals 202 vs 456 were not independently reproduced by an executable checkout here, so neither count is promoted as fact or used for Patch 0016 decisions. Future structural-test work must count the actual checkout and classify by member visibility/assertion semantics, not filename.

E5c: no authority-sensitive production path was found that accepts an exception from an untrusted boundary and treats runtime type as provenance. Status remains **open question / no verified defect / no fix authorized**.

## Remaining validation gate

Because the post-attempt correction is test-only, the successful Harness/fixture authority at `09bf644f...` remains applicable to unchanged executable source. The required next machine gate is the full Core test suite at the latest branch head after pulling the correction/evidence commits.

Run:

```powershell
git pull --ff-only
git rev-parse HEAD
git diff --quiet; "TRACKED_DIFF_EXIT=$LASTEXITCODE"
git diff --cached --quiet; "STAGED_DIFF_EXIT=$LASTEXITCODE"
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
"CORE_TEST_EXIT=$LASTEXITCODE"
```

Only observed output from that gate may promote Patch 0016 to full native Core test authority.
