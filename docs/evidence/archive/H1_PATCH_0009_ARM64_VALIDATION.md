# H1 Patch 0009 — Native ARM64 Validation Evidence

Status: COMPLETE for exercised deterministic State Interpreter proposal gates

## Exact machine-tested executable/test head

`a257cf7553398a323d8ce790aa600950eb88c1b9`

This SHA is the executable/test validation authority for Patch 0009.

## Environment authority

The following evidence was supplied from the user's native Windows ARM64 development machine. This document records observed machine results only; it does not promote those results into unexercised runtime, provider, NPU, WACK, or Store claims.

## ARM64 Harness/Core build

Command:

`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Observed:

- restore complete;
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
- total `330`;
- succeeded `330`;
- failed `0`;
- skipped `0`;
- build succeeded.

Patch 0009 contributes 77 State Interpreter test executions over the machine-validated Patch 0008 baseline of 253 tests.

## Harness regressions

### Missing Raft

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json`

Observed:

- `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- exit `0`.

### Generic smoke

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json`

Observed:

- `Fixture validated: ensemble.e0.smoke@0.1.0`;
- exit `0`.

## Validated implementation scope

The exact executable/test delta from implementation approval checkpoint
`23fb7d2833484dc13abed240dc600ff3bc4c084c`
to machine-tested head
`a257cf7553398a323d8ce790aa600950eb88c1b9`
contains only:

1. `src/Ensemble.E0.Core/StateInterpreter/StateInterpretationModels.cs`;
2. `src/Ensemble.E0.Core/StateInterpreter/StateInterpretationContract.cs`;
3. `tests/Ensemble.E0.Core.Tests/StateInterpreter/StateInterpretationContractTests.cs`.

No pre-existing Access, Context, Performer, Director, Integrity, fixture, Harness, project configuration, persistence, UI, Windows AI/NPU, packaging, or Store source changed in that machine-tested executable/test delta.

## What this validates

The exercised gates validate the deterministic Patch 0009 boundary for:

- structural `StateInterpretationSource` binding to the exact Patch 0008 Accept association;
- least-privilege trusted Source identity;
- strict bounded State Interpreter proposal JSON parsing;
- typed immutable State mutation proposal/change variants;
- E0 domain/operation restrictions;
- Add-only Knowledge and Memory semantics;
- source-only Add CharacterClaim proposition semantics;
- strict RecordId/CharacterId/text/Unicode/duplicate handling;
- exact duplicate semantic mutation rejection;
- trusted Candidate/Scene identity copying from Source rather than model-authored JSON;
- deterministic parser behavior and frozen upstream regression identities.

## Explicit nonclaims

This evidence does not establish:

- State Interpreter model/provider execution or request composition;
- authenticated provider/review provenance;
- accepted/rejected/alternate Take semantics or TakeId allocation;
- deterministic State Authority decisions;
- ProductionState / StateHash;
- authoritative RecordId allocation for new State records;
- mutation application;
- atomic Performance + consequence commit/persistence/recovery;
- effective Current Opportunity mutation/history append;
- Scene-loop execution or next-Performer triggering;
- Observation engine or World Resolver;
- E0-D round-robin execution;
- E0-E playwright-control Interpreter protocol;
- Windows AI / NPU execution or performance;
- WinUI behavior;
- packaging / WACK;
- Microsoft Store certification.

Any documentation-only commits after the exact machine-tested head do not increase executable validation authority.
