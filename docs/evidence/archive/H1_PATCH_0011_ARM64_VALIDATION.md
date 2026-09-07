# H1 Patch 0011 — Native ARM64 Validation Evidence

Status: COMPLETE for exercised E0 Take Semantics gates

## Exact machine-tested executable/test head

`4250011c167cd9850ad891aaea4ee053216cf135`

This SHA is the executable/test validation authority for Patch 0011.

A later merge commit on `main` must not replace this exact tested-head authority.

## Environment authority

The following evidence was supplied from the user's native Windows ARM64 development machine. This record preserves observed machine results only. It does not promote those results into unexercised Production-state, NPU, WACK, packaging, or Store claims.

## First target-machine attempt

The first machine attempt was performed at implementation head:

`0231be52cce3c5f693e551b5d82f7fa1df619979`

Observed:

- `dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug` succeeded;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target was `net9.0\win-arm64`;
- `dotnet test` did not execute because the test project failed analyzer-as-error compilation with six `MSTEST0032` diagnostics;
- five diagnostics were compile-time-known constant assertions in `E0TakeTests.cs`;
- one diagnostic was the compile-time-known Take contract-version assertion in `E0TakeContractAuditTests.cs`;
- the production Take implementation itself had compiled successfully before those test analyzer failures;
- Missing Raft Harness validation succeeded after the successful Harness build;
- the generic smoke command failed because the handoff referenced stale path `fixtures\smoke\smoke-0.1.0.json`; the canonical repository fixture is `fixtures\smoke\e0-fixture-v1.json`.

Smallest-surface corrections were then applied only to the two Take test files plus the stale validation command in the Patch 0011 implementation handoff. No analyzer suppression was introduced and no production Take semantics changed.

## Final ARM64 Harness/Core build

Commands began from a clean working tree at exact head:

`4250011c167cd9850ad891aaea4ee053216cf135`

Command:

`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed:

- restore completed;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target `net9.0\win-arm64`;
- build succeeded.

## Full Core test execution

Command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed:

- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Core.Tests` succeeded;
- test execution succeeded;
- total `430`;
- succeeded `430`;
- failed `0`;
- skipped `0`;
- build succeeded.

Patch 0011 increases the latest full Core regression count from the machine-validated Patch 0010 baseline of 392 tests to 430 tests.

## Harness regressions

These Harness runs were executed after the successful final Patch 0011 build above.

### Missing Raft

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json`

Observed:

- `Fixture validated: ensemble.e0.missing-raft@0.1.0`.

### Generic smoke

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json`

Observed:

- `Fixture validated: ensemble.e0.smoke@0.1.0`.

## Validated implementation scope

Implementation branch:

`h1-patch-0011-take-semantics-implementation`

Approved implementation baseline on `main`:

`3e9de4f10828f330c0c79fbf76c010422d757b2b`

Exact machine-tested branch head:

`4250011c167cd9850ad891aaea4ee053216cf135`

The exact branch delta from the approved baseline contains only:

1. `src/Ensemble.E0.Core/Take/TakeModels.cs` — added;
2. `tests/Ensemble.E0.Core.Tests/Take/E0TakeTests.cs` — added;
3. `tests/Ensemble.E0.Core.Tests/Take/E0TakeContractAuditTests.cs` — added;
4. `docs/handoff/E0A_H1_PATCH_0011_IMPLEMENTATION_HANDOFF.md` — one corrected generic-smoke fixture path.

No pre-existing Access, Context, Performer, Director, Integrity, State Interpreter, State Authority, Fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, WACK, or Store source changed in this validated Patch 0011 implementation delta.

## What this validates

For the exercised immutable Take-semantics scope, the target-machine evidence validates:

- Patch 0011 Take source compiles as part of the native ARM64 Harness/Core build;
- all 430 Core tests execute successfully;
- canonical `ensemble.e0.take.v1` contract and exact disposition values exercised by the test suite;
- reuse of the existing canonical `TakeId` type exercised by contract audits;
- private Take construction and sole rich `E0Take.Bind(...)` construction surface exercised by contract audits;
- exact retained Candidate Performance and State Interpretation Proposal identity exercised by tests;
- fresh deterministic State Authority replay rather than trust in caller-supplied Status/Decisions exercised by tests;
- Integrity Reject / RequestAnotherTake and malformed/mismatched upstream associations fail closed in exercised tests;
- fresh `ReviewRequired` / `RequiresReview` authority cannot bind a Take in exercised tests;
- Accepted, Rejected, and Alternate Take dispositions remain independent from terminal Approved/Rejected consequence sets in exercised tests;
- no Patch 0012 commit/state/persistence authority surface is exposed by the audited Take namespace;
- exception normalization and no-content-leak contract surfaces exercised by tests;
- frozen Missing Raft context/candidate/fixture identities remain unchanged;
- retained Missing Raft and generic smoke Harness behavior after the final Patch 0011 build.

## Explicit nonclaims

This evidence does not establish:

- evolved multi-turn ProductionState;
- ProductionState or StateHash;
- authoritative ContextPacket / StateAuthoritySnapshot common-state identity;
- authoritative new RecordId allocation;
- mutation application;
- atomic causal Commit or CommitId;
- stale-state runtime/freshness application;
- persistence/recovery;
- authenticated provider/policy/review/Take-disposition provenance machinery;
- provider/model State Interpreter execution;
- Scene-loop execution;
- World Resolver / Observation engine;
- final branching/rehearsal/retcon/alternate-promotion UX;
- Windows AI or NPU execution/performance;
- WinUI behavior;
- packaging / WACK;
- Microsoft Store certification.

## Validation authority rule

Static/adversarial review remains advisory.

This document records compiler/test/runtime authority only for the exact exercised commands and exact machine-tested head above.

WACK and Partner Center remain independent later validation authorities.
