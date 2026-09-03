# H1 Patch 0010 — Native ARM64 Validation Evidence

Status: COMPLETE for exercised deterministic State Authority review/decision gates

## Exact machine-tested executable/test head

`b07c8e161d221bc9bf2e57802de0aae7b542ce5a`

This SHA is the executable/test validation authority for Patch 0010.

The later squash-merge commit on `main` does not replace this exact tested-head authority.

## Environment authority

The following evidence was supplied from the user's native Windows ARM64 development machine. This record preserves observed machine results only. It does not promote those results into unexercised Production-state, NPU, WACK, or Store claims.

## Pre-validation compiler correction

The first machine attempt was performed at implementation head:

`401f6bf93aadd8879c1645876b63b729fc6ac962`

Observed build result:

- `Ensemble.E0.Core` failed with five `CS1503` errors in `DeterministicStateAuthority.cs`;
- each failure was a `RecordId` versus `string` lookup-key mismatch;
- `dotnet test` failed at the same compiler surface;
- Harness commands invoked afterward with `--no-build` executed previously built binaries and therefore are not Patch 0010 validation evidence.

Patch commit:

`b07c8e161d221bc9bf2e57802de0aae7b542ce5a`

changed only:

`src/Ensemble.E0.Core/StateAuthority/DeterministicStateAuthority.cs`

The correction normalized the internal `ExistingRecordId(...)` lookup helper to return the validated Record ID string used by the evaluator's string-keyed dictionaries/sets. No contract behavior or upstream source was redesigned.

All required machine gates were then rerun from the corrected exact head.

## ARM64 Harness/Core build

Command:

`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed at `b07c8e1...`:

- restore complete;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target `net9.0\win-arm64`;
- build succeeded;
- zero reported build errors.

## Full Core test execution

Command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed at `b07c8e1...`:

- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Core.Tests` succeeded;
- test execution succeeded;
- total `392`;
- succeeded `392`;
- failed `0`;
- skipped `0`;
- build succeeded.

Patch 0010 increases the latest full Core regression count from the machine-validated Patch 0009 baseline of 330 tests to 392 tests.

## Harness regressions

These Harness runs were executed only after the successful Patch 0010 build above.

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

Implementation PR:

`#22 — H1: implement deterministic State Authority`

Exact machine-tested branch head:

`b07c8e161d221bc9bf2e57802de0aae7b542ce5a`

Squash-merge commit on `main`:

`e2bc3a4130d768dca29698a32fcd6a40e63d5f5c`

The implementation delta contains exactly four State Authority source/test files:

1. `src/Ensemble.E0.Core/StateAuthority/StateAuthorityModels.cs`;
2. `src/Ensemble.E0.Core/StateAuthority/DeterministicStateAuthority.cs`;
3. `tests/Ensemble.E0.Core.Tests/StateAuthority/DeterministicStateAuthorityTests.cs`;
4. `tests/Ensemble.E0.Core.Tests/StateAuthority/StateAuthorityContractAuditTests.cs`.

No pre-existing Access, Context, Performer, Director, Integrity, State Interpreter, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, WACK, or Store source changed in that implementation PR.

## What this validates

For the exercised initial-fixture deterministic State Authority scope, the target-machine evidence validates:

- Patch 0010 source compiles as part of the ARM64 Harness/Core build;
- all 392 Core tests execute successfully;
- fixture-derived `StateAuthoritySnapshot` behavior exercised by the test suite;
- Proposal 0.6 structural State Authority input and proposal-identity behavior exercised by the test suite;
- deterministic hard-rule / explicit-review / policy-default decision behavior exercised by the test suite;
- creator-lock/SystemImmutable, reference/domain/ownership/conflict, mandatory-review, policy, empty-proposal, deterministic replay, and boundary regression tests exercised by the test suite;
- retained Missing Raft and generic smoke Harness behavior after the Patch 0010 build.

## Explicit nonclaims

This evidence does not establish:

- evolved multi-turn ProductionState review;
- ProductionState or StateHash;
- authoritative new RecordId allocation;
- mutation application;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- provisional-Take ordering relative to State Interpreter / State Authority;
- atomic causal Commit or CommitId;
- persistence/recovery;
- State Authority policy/review provenance authentication;
- provider/model State Interpreter execution;
- Scene-loop execution;
- World Resolver / Observation engine;
- Windows AI or NPU execution/performance;
- WinUI behavior;
- packaging / WACK;
- Microsoft Store certification.

## Validation authority rule

Static/adversarial review remains advisory.

This document records compiler/test/runtime authority only for the exact exercised commands and exact machine-tested head above.

WACK and Partner Center remain independent later validation authorities.