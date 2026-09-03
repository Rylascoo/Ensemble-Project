# H1 Patch 0013 — Native ARM64 Validation Evidence

Date: 2026-09-03
Status: COMPLETE FOR EXERCISED PATCH 0013 CORE/TEST/HARNESS GATES

## Validation authority boundary

This document records only machine-observed compiler, test, and Harness evidence supplied from the user's native Windows ARM64 development machine.

It does not promote those observations into unexercised runtime, persistence, Windows AI/NPU, packaging, WACK, or Microsoft Store claims.

## Exact exercised heads

Patch 0013 reached complete exercised validation across two adjacent implementation heads because the first native run exposed a test-only MSTest analyzer defect.

### Final Core test authority

`a3fae23dc4df302e834b031ecfc848a3bb2d37fc`

This is the exact head at which the complete Core test assembly compiled and all `496` tests executed successfully.

### Native Core/Harness build and fixture authority

`382e11f9fbe6774806152fad75b6a23cc8733187`

This is the exact head at which the native `win-arm64` Harness/Core build and both exercised Harness fixture validations succeeded.

The only repository change between `382e11f9fbe6774806152fad75b6a23cc8733187` and `a3fae23dc4df302e834b031ecfc848a3bb2d37fc` is:

`tests/Ensemble.E0.Core.Tests/Opportunity/Patch0013ContractAuditTests.cs`

with `11` additions and `2` deletions.

There are zero production-source changes between those heads. Therefore the production Core/Harness source exercised successfully at `382e11f...` is identical to the production Core/Harness source present at the final successful Core test head `a3fae23...`.

This evidence intentionally preserves the exact SHA for each exercised gate rather than claiming commands were rerun where they were not.

Historical first-attempt evidence:

`docs/evidence/H1_PATCH_0013_NATIVE_VALIDATION_ATTEMPT_01.md`

## Full Core test execution

Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Observed at exact head `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`:

- restore completed;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Core.Tests` succeeded;
- test execution succeeded;
- total: `496`;
- succeeded: `496`;
- failed: `0`;
- skipped: `0`;
- test duration reported: `1.8s`;
- full command build succeeded.

Patch 0013 therefore increases the latest complete Core regression count from Patch 0012's machine-validated `473` tests to `496` tests.

## Patch 0013 fixed reference oracle

The successful 496-test suite includes:

`Patch0013ReferenceOracleTests.MissingRaftPatch0012Oracle_ThenFallbackOpportunity_HasExactPatch0013Digest`

That test preserves and checks all three canonical reference hashes:

Genesis `StateHash`:

`30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`

Patch 0012 reference causal-commit post-state `StateHash`:

`057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30`

Patch 0013 effective-opportunity result `StateHash`:

`dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310`

Because all 496 tests passed at the exact final Core-test head, this fixed Patch 0013 oracle assertion passed as part of the exercised suite.

## Native ARM64 Harness/Core build

Command:

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Observed at exact head `382e11f9fbe6774806152fad75b6a23cc8733187`:

- restore completed;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target was `net9.0\win-arm64`;
- build succeeded.

## Harness regressions

Both commands below were exercised at exact head `382e11f9fbe6774806152fad75b6a23cc8733187` after the successful native Harness build.

### Missing Raft

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
```

Observed:

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
```

### Generic smoke

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Observed:

```text
Fixture validated: ensemble.e0.smoke@0.1.0
```

The user did not separately query `$LASTEXITCODE`; this record therefore preserves the successful validation messages and normal command completion without inventing an explicit numeric exit-code observation.

## Native convergence history

Patch 0013 required one patch-first correction after the first native attempt:

1. at `382e11f9fbe6774806152fad75b6a23cc8733187`, production Core and Harness compiled and both fixture validations succeeded, but the test project failed to compile because two `MSTEST0032` diagnostics identified compile-time always-true assertions;
2. only the affected contract-audit test file was changed;
3. at `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`, the full test project compiled and all `496/496` Core tests passed;
4. no production source changed during that correction cycle.

The failed attempt is intentionally preserved rather than rewritten.

## Validated Patch 0013 implementation boundary

Canonical blueprint:

`docs/blueprint/H1_PATCH_0013_EFFECTIVE_OPPORTUNITY_AUTHORITY.md`

Approved Proposal:

`0.6`

Implementation branch:

`h1-patch-0013-effective-opportunity-authority-implementation`

Parent promoted `main` checkpoint:

`8c89f998fe6f42e04a75b9090fbcc10f0574f5a2`

The exercised machine gate covers the compiled/tested deterministic Patch 0013 surface including:

- closed `E0OpportunityHistory` genesis initialization and append discipline;
- source causal-chain and history-anchor validation;
- fresh postcommit Director binding on the live path;
- reuse of the existing least-intervention Director strategy;
- deterministic effective-opportunity establishment;
- narrow null-to-selected Production opportunity transition;
- minimal opportunity-transition event surface;
- disjoint `kind = opportunityTransition` StateHash envelope under the inherited hash contract;
- one-step Replay reconstruction and tamper rejection;
- preservation of inherited Patch 0012 genesis and causal-commit canonical hash oracles;
- fail-closed contract, malformed-input, authority-splice, cache, context, replay, and invariant tests;
- prior Core regression surface represented by the full 496-test suite.

## What this validates

For the exercised Patch 0013 scope, target-machine evidence establishes:

- Patch 0013 production Core source compiles;
- native `win-arm64` Harness/Core compilation succeeds for the production source carried into the final test head;
- all `496` Core tests execute successfully at the final corrected test head;
- the inherited Patch 0012 fixed StateHash oracles remain unchanged and pass;
- the fixed Patch 0013 opportunity-transition StateHash oracle passes;
- live establishment, replay, anti-splice, closed history, minimal event, narrow Production mutation, deterministic Director reuse, and public-contract regression tests pass;
- Missing Raft Harness validation remains successful;
- generic smoke Harness validation remains successful.

## Explicit nonclaims

This evidence does not establish:

- evolved Production -> Access/Context integration;
- CharacterClaim / recent-Performance disclosure policy;
- full multi-turn replay from genesis;
- durable persistence/recovery;
- branch/canon/retcon/rehearsal behavior;
- authenticated provider/model execution;
- full Scene-loop execution;
- World Resolver / Observation behavior;
- Windows AI Foundry or NPU execution/performance;
- WinUI behavior;
- MSIX packaging;
- WACK success;
- Microsoft Store certification.

## Validation authority rule

Static/adversarial review remains advisory.

The full Core test authority is the exact successful test head `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`.

The native Core/Harness build and exercised fixture authority is the exact immediately preceding head `382e11f9fbe6774806152fad75b6a23cc8733187`, whose production/Harness source is repository-identical to the final test head.

Later documentation, PR, merge, or checkpoint commits must not replace those exact machine-observed authorities.
