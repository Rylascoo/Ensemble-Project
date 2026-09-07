# H1 Patch 0012 — Native ARM64 Validation Evidence

Date: 2026-09-03
Status: COMPLETE for exercised E0 Atomic Causal Commit gates

## Exact machine-tested executable/test head

`39bc078c130ab1165c6a81c1673dd5cd25da3724`

This SHA is the compiler/test/Harness validation authority for Patch 0012.

A later documentation commit or merge commit must not replace this exact tested-head authority.

## Environment authority

The following evidence was supplied from the user's native Windows ARM64 development machine. This record preserves only the commands and machine-observed results actually exercised.

It does not promote these results into unexercised runtime, persistence, Windows AI/NPU, packaging, WACK, or Store claims.

## Validation history

Patch 0012 reached complete native validation only after two patch-first correction cycles:

1. native attempt 01 exposed four test-source compile diagnostics while Core/Harness itself built successfully;
2. native attempt 02 compiled and executed the test assembly but exposed one shared fixture-provenance canonicalization defect causing 101 failures;
3. the isolated provenance correction plus focused regression reduced the remaining failures to three stale fixed hash oracles;
4. native diagnostics established the exact canonical hashes after the intended provenance-order normalization;
5. only the fixed genesis/post-commit oracle constants changed in the final tested executable/test delta.

Detailed earlier attempt records remain:

- `docs/evidence/H1_PATCH_0012_NATIVE_VALIDATION_ATTEMPT_01.md`;
- `docs/evidence/H1_PATCH_0012_NATIVE_VALIDATION_ATTEMPT_02.md`.

## Canonical hash-oracle correction

The provenance correction intentionally canonicalizes semantically unordered fixture provenance into ordinal RecordId order for Production storage. That changed canonical Production projection bytes and therefore changed the fixed StateHash test oracles without changing transition authority.

Native ARM64 diagnostics established:

- genesis StateHash: `30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`;
- oracle post-commit StateHash: `057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30`.

The temporary diagnostic test used only to extract the complete post-commit hash was removed before the final tested head.

A branch comparison from the pre-diagnostic checkpoint `181ba982b6e35f61e85c246f24d0fa44d3b81cee` to exact tested head `39bc078c130ab1165c6a81c1673dd5cd25da3724` showed the net diagnostic-recovery delta contained only:

- the genesis hash oracle replacement in `ProductionStateTests.cs`;
- the genesis hash oracle replacement in `DeterministicCausalCommitTests.cs`;
- the post-commit hash oracle replacement in `DeterministicCausalCommitTests.cs`.

No production implementation source changed in that final oracle-recovery delta, and the original deterministic assertions remained present.

## Full Core test execution

Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Observed at exact head `39bc078c130ab1165c6a81c1673dd5cd25da3724`:

- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Core.Tests` succeeded;
- test execution succeeded;
- total: `473`;
- succeeded: `473`;
- failed: `0`;
- skipped: `0`;
- build succeeded.

Patch 0012 therefore increases the latest complete Core regression count from Patch 0011's machine-validated `430` tests to `473` tests.

## Native ARM64 Harness/Core build

Command:

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Observed at the same exact head:

- restore completed;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target was `net9.0\win-arm64`;
- build succeeded.

## Harness regressions

The following commands were run after the successful native Harness build above.

### Missing Raft

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
$LASTEXITCODE
```

Observed:

- `Fixture validated: ensemble.e0.missing-raft@0.1.0`;
- exit code `0`.

### Generic smoke

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
$LASTEXITCODE
```

Observed:

- `Fixture validated: ensemble.e0.smoke@0.1.0`;
- exit code `0`.

## Validated implementation scope

Implementation branch:

`h1-patch-0012-atomic-causal-commit-implementation`

Approved implementation baseline on `main`:

`163696f4a89aa3a3b1ea4975167c15b827328d1d`

Exact machine-tested executable/test head:

`39bc078c130ab1165c6a81c1673dd5cd25da3724`

The exercised machine gate validates the compiled/tested Patch 0012 implementation surface including the approved ProductionState, StateHash, Production-derived StateAuthority binding, Accepted-only Take/source-state binding, record materialization, deterministic atomic causal commit, one-step Replay, shared provenance validation, and the retained prior deterministic regression surface represented by the full 473-test suite.

## What this validates

For the exercised Patch 0012 scope, target-machine evidence establishes:

- Patch 0012 Core source compiles as part of the native ARM64 Harness/Core build;
- the Harness target is native `win-arm64`;
- all `473` Core tests execute successfully;
- canonical Production genesis and causal-commit StateHash oracles match the tested implementation;
- valid non-ordinal fixture provenance is accepted and canonicalized without changing its edge set;
- immutable ProductionState/StateHash public-contract tests pass;
- exact Production-to-StateAuthority semantic mapping tests pass;
- Accepted-only checkpoint/binding association and freshness tests pass;
- zero-mutation and all-Rejected Accepted commit tests pass;
- Add/Supersede/Deactivate/mixed materialization/application tests pass;
- stale binding, duplicate identity, collision, provenance, and malformed-input fail-closed tests pass;
- canonical causal event payload and post-commit hash tests pass;
- one-step Replay reconstruction/tamper tests pass;
- expected exception-domain sanitization tests pass;
- existing Missing Raft fixture behavior remains valid;
- existing generic smoke fixture behavior remains valid.

## Explicit nonclaims

This evidence does not establish:

- evolved Production -> Access/Context integration;
- next-Director-opportunity transition authority;
- full multi-turn replay from genesis;
- durable persistence, recovery, branch/canon, retcon, or rehearsal behavior;
- authenticated provider/model execution;
- Scene-loop execution;
- World Resolver / Observation behavior;
- Windows AI Foundry or NPU execution/performance;
- WinUI behavior;
- MSIX packaging;
- WACK success;
- Microsoft Store certification.

## Validation authority rule

Static/adversarial review remains advisory.

This document records compiler/test/Harness authority only for the exact exercised commands and exact machine-tested head above.

Device runtime beyond the exercised Harness paths, WACK, and Partner Center remain independent later validation authorities.
