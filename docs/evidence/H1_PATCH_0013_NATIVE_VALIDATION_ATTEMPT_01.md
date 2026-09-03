# H1 Patch 0013 — Native Validation Attempt 01

Date: 2026-09-03
Status: HISTORICAL FAILED TEST-PROJECT COMPILE GATE; CORE/HARNESS GATES SUCCEEDED

## Authority boundary

This document preserves the first Patch 0013 native Windows ARM64 validation attempt exactly as observed from the user's development machine.

It is historical evidence. It does not overwrite or weaken the later successful Core test execution at the corrected test head.

## Exact exercised head

`382e11f9fbe6774806152fad75b6a23cc8733187`

Implementation branch:

`h1-patch-0013-effective-opportunity-authority-implementation`

## Core test command

Command:

```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Observed:

- restore completed;
- `Ensemble.E0.Core` compiled successfully;
- `Ensemble.E0.Core.Tests` failed to compile because MSTest analyzer `MSTEST0032` emitted two errors in `Patch0013ContractAuditTests.cs`;
- diagnostics were at lines 220 and 223 in that exact checked-out source;
- both diagnostics stated that the assertion condition was known to be always true;
- test execution did not begin;
- the command ended with `Build failed with 2 error(s)`.

The two failing assertions directly compared string literals against public `const` contract fields. The analyzer therefore evaluated them as compile-time constants and correctly rejected the assertions as ineffective regression tests.

This was a test-source defect only. The production Core assembly had already compiled successfully in the same command.

## Patch-first correction

The smallest affected surface was corrected in:

`tests/Ensemble.E0.Core.Tests/Opportunity/Patch0013ContractAuditTests.cs`

The correction replaced only the two compile-time-constant assertions with reflection-based inspection of the public constant fields, preserving the intended public-contract regression check without suppressing the analyzer.

Correction commit:

`a3fae23dc4df302e834b031ecfc848a3bb2d37fc`

Repository comparison from this attempt head to the correction head shows exactly one changed file:

- `tests/Ensemble.E0.Core.Tests/Opportunity/Patch0013ContractAuditTests.cs`;
- `11` additions;
- `2` deletions;
- zero production-source changes.

## Native ARM64 Harness/Core build at this head

Command:

```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Observed:

- restore completed;
- `Ensemble.E0.Core` succeeded;
- `Ensemble.E0.Harness` succeeded;
- Harness output target was `net9.0\win-arm64`;
- build succeeded.

## Harness fixture executions at this head

### Missing Raft

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json
```

Observed output:

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
```

### Generic smoke

Command:

```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json
```

Observed output:

```text
Fixture validated: ensemble.e0.smoke@0.1.0
```

The user did not separately print `$LASTEXITCODE`, so this evidence records the successful validation messages and normal command completion but does not invent an explicit numeric exit-code observation.

## What attempt 01 establishes

At exact head `382e11f9fbe6774806152fad75b6a23cc8733187`:

- Patch 0013 production Core compiled successfully;
- the native `win-arm64` Harness compiled successfully;
- Missing Raft fixture validation completed successfully;
- generic smoke fixture validation completed successfully;
- full Core test execution was blocked only by two test-project analyzer diagnostics.

It does not establish that the Core test suite passed at this head because the test assembly did not compile.

## Nonclaims

This evidence does not establish unexercised runtime paths, durable persistence, evolved Production -> Access/Context integration, Windows AI Foundry/NPU behavior, WinUI behavior, packaging, WACK, or Store certification.
