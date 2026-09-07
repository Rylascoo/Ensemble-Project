# H1 Patch 0005 — Native Windows ARM64 Validation Evidence

Date: 2026-09-02
Status: COMPLETE for Patch 0005 exercised gates
Patch: H1 Patch 0005 — Deterministic Context Composer + Dual Context Identity
Canonical specification: `docs/blueprint/H1_PATCH_0005_DETERMINISTIC_CONTEXT_COMPOSER.md`

## Validation authority
This document records evidence supplied directly from the user's native Windows ARM64 machine.

Authority hierarchy remains:
- static review: advisory only;
- native `dotnet build`: compiler authority for the exercised build;
- native `dotnet test`: test-execution authority for the exercised suite;
- target-device Harness execution: runtime authority for the exercised Harness paths;
- no NPU, WACK, Store, or provider/model authority is claimed.

## Machine gate — first implementation head
Checked-out implementation head:

`44efd6358d8a94e769eee2ff9b2c7bd4715f5587`

### Native build
Command:

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Result:
- restore: PASS;
- `Ensemble.E0.Core`: PASS;
- `Ensemble.E0.Harness`: PASS;
- Harness target output: `net9.0\win-arm64`;
- overall build: PASS.

### Initial Core test gate
Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Result:
- 115 tests discovered;
- 113 succeeded;
- 2 failed;
- 0 skipped.

The two failures were isolated to stale independent-oracle/test expectations, not production code. The canonical Missing Raft fixture contains `Marlowe's perception`; the oracle had transcribed `Marlowe's competence`. The equal 10-byte word lengths preserved the expected byte counts while changing both SHA-256 values.

### Missing Raft Harness runtime
Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
$LASTEXITCODE
```

Result:
- fixture validation: PASS;
- exit code: `0`.

### Generic smoke Harness runtime
Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
$LASTEXITCODE
```

Result:
- fixture validation: PASS;
- exit code: `0`.

## Corrective patch scope
The correction changed only:

1. `docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`;
2. `tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs`.

GitHub comparison from `44efd6358d8a94e769eee2ff9b2c7bd4715f5587` through corrected machine-test head `befb6648c36400546ac4843d575bd64760b23445` confirms no production source changed.

Therefore:
- the native Harness build/runtime evidence at `44efd...` applies to the same production source bytes present at the corrected test head;
- only the oracle/test expectations required rerun.

## Corrected Core test gate
Checked-out corrected head:

`befb6648c36400546ac4843d575bd64760b23445`

Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Result:
- `Ensemble.E0.Core`: build PASS as part of test execution;
- `Ensemble.E0.Core.Tests`: build PASS;
- total: 115;
- succeeded: 115;
- failed: 0;
- skipped: 0;
- full Core test gate: PASS.

## Corrected frozen Voss Context identities
Canonical structured bytes:
`2569`

StructuredContextHash:
`bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`

ContextPacketId:
`CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`

Rendered envelope bytes:
`1905`

RenderedContextHash:
`ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`

Reference derivation details:
`docs/evidence/H1_PATCH_0005_REFERENCE_ORACLE.md`

## Existing fixture identity regression
The 115-test suite includes the frozen Missing Raft ECJ-1 regression:
- canonical UTF-8 bytes: `9112`;
- SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

No fixture JSON changed in Patch 0005.

## Patch 0005 exercised exit-gate result
PASS for the exercised Patch 0005 gates:
- approved deterministic Context Composer contract implemented;
- Access-Control-safe projection is the only Character information input;
- full-authorized reference composition preserves authority categories;
- deterministic provider-neutral rendering implemented;
- structured and rendered identities separated;
- ContextPacketId content-addressing implemented;
- recentPerformances remains exact empty schema array with no speculative history type;
- no denied Access IDs or fixture provenance enter Character context;
- shared canonical JSON extraction preserves ECJ-1 fixture identity;
- native ARM64 build passed;
- corrected complete Core suite passed 115/115;
- Missing Raft Harness regression passed/0;
- generic smoke Harness regression passed/0;
- corrective patch introduced no production-source drift.

## Explicitly not established
This evidence does not establish:
- provider/model behavior;
- Director behavior;
- accepted Take/history composition;
- non-empty recent-performance context;
- semantic relevance optimization;
- ProductionState/StateHash;
- causal commit/persistence;
- Windows AI/NPU execution;
- WinUI behavior;
- WACK;
- Microsoft Store certification.
