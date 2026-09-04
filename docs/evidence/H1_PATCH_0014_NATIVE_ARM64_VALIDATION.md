# H1 Patch 0014 — Native Windows ARM64 Validation Evidence

Date: 2026-09-03
Status: MACHINE VALIDATION COMPLETE FOR APPROVED PATCH 0014 IMPLEMENTATION

## Validation authority

This evidence records two exact machine-tested repository heads and preserves their authority boundaries.

### Exact full Core-test authority

Repository head:

```text
4ac0250005c8d88c3b815c5d53cfca0a982e454c
```

Machine environment reported by the user:

```text
PROCESSOR_ARCHITECTURE = ARM64
.NET SDK = 9.0.317
RID = win-arm64
.NET host architecture = arm64
```

Tracked worktree checks immediately before the test run:

```text
TRACKED_DIFF_EXIT=0
STAGED_DIFF_EXIT=0
```

Native command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Machine result:

```text
Ensemble.E0.Core succeeded
Ensemble.E0.Core.Tests succeeded
Test summary: total: 538, failed: 0, succeeded: 538, skipped: 0
Build succeeded
CORE_TEST_EXIT=0
```

Therefore `4ac0250005c8d88c3b815c5d53cfca0a982e454c` is the exact native Windows ARM64 compiler/test authority for the full Patch 0014 Core test suite.

## Exact Harness / fixture runtime authority

Repository head:

```text
84b3e23db55910f746670cd2e06a67b8a5dea2b3
```

The user ran the following on the same native Windows ARM64 machine:

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Result:

```text
Ensemble.E0.Core succeeded
Ensemble.E0.Harness succeeded
Build succeeded
HARNESS_BUILD_EXIT=0
```

Missing Raft fixture runtime:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
```

Result:

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
MISSING_RAFT_EXIT=0
```

Generic smoke fixture runtime:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Result:

```text
Fixture validated: ensemble.e0.smoke@0.1.0
GENERIC_SMOKE_EXIT=0
```

Therefore `84b3e23db55910f746670cd2e06a67b8a5dea2b3` is the exact native Windows ARM64 Harness build and fixture-runtime authority.

## Why Harness rerun was not required after the compiler correction

The first full Core-test attempt at `84b3e23...` failed only because:

```text
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextContinuityTests.cs(44,25):
CS0103: The name 'MissingRaftContract' does not exist in the current context
```

Patch-first correction commit:

```text
4ac0250005c8d88c3b815c5d53cfca0a982e454c
```

GitHub compare `84b3e23... -> 4ac0250...` proves exactly one changed file and one added line:

```text
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextContinuityTests.cs
+ using Ensemble.E0.Core.Fixture;
```

No `src/` file, Harness file, fixture, production contract, canonicalizer, or runtime code changed between those two machine-tested heads.

Accordingly:

- the full Core compiler/test authority belongs to `4ac0250...`;
- the Harness/runtime authority remains `84b3e23...`;
- the Harness result is not relabeled as though it ran at `4ac0250...`;
- a repeated Harness run would provide no new production-tree evidence for this one-line test-only correction.

## Local untracked file

The user's worktree contained:

```text
?? patch0012-local-edit.txt
```

This file was untracked and was not modified, staged, compiled, or used by Patch 0014 validation. The authoritative tracked-diff checks at the final Core-test head were both clean.

## Validation scope

This evidence establishes:

- native ARM64 .NET 9 Core compilation;
- all 538 Core tests passing;
- native ARM64 Harness compilation for the unchanged production tree;
- successful Missing Raft fixture validation;
- successful generic smoke fixture validation.

This evidence does **not** establish:

- WinUI runtime behavior;
- Windows AI Foundry or NPU execution;
- package/MSIX/WACK validation;
- Microsoft Store certification;
- performance, thermals, battery, or TOPS characteristics.

Those remain outside H1 Patch 0014 scope and require their own later authority gates.
