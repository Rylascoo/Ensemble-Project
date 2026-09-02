# H1 Patch 0002.1 ARM64 Validation

Date: 2026-09-01
Tested commit head: `f0147b09`
Scope: E0 Fixture Dialect v1 generic validation only

## Validation authority
This record captures evidence supplied from the user's target Windows ARM64 development machine. It is authoritative only for the compiler, tests, and runtime behavior actually exercised.

The development environment had previously confirmed .NET SDK `9.0.317`; this validation submission did not repeat `dotnet --version`.

## Compiler gate
Command:

`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Result:
- restore completed successfully;
- `Ensemble.E0.Core` succeeded and produced `src\Ensemble.E0.Core\bin\Debug\net9.0\Ensemble.E0.Core.dll`;
- `Ensemble.E0.Harness` succeeded and produced `src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll`;
- build succeeded in 1.6 seconds;
- no build errors were reported.

Verdict: PASS for compilation of the tested Patch 0002.1 commit on the target machine.

## Test-execution gate
Command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Result:
- Core compiled successfully;
- Core.Tests compiled successfully;
- test execution succeeded;
- total: 30;
- succeeded: 30;
- failed: 0;
- skipped: 0;
- test duration: 0.7 seconds;
- overall command build succeeded in 2.3 seconds.

Verdict: PASS for the 30 tests exercised by this commit.

## Runtime smoke gate
Command exercised the Harness against:

`fixtures\smoke\e0-fixture-v1.json`

Observed output:

`Fixture validated: ensemble.e0.smoke@0.1.0`

Observed process exit code: `0`.

Verdict: PASS for native Harness startup and the generic E0 Fixture Dialect v1 load/validation path exercised by the smoke fixture.

## Validation boundary
This evidence does not validate Missing Raft-specific semantics, ECJ-1 canonicalization, SHA-256 fixture hashing, deterministic Access Control, Context Composer, Production-state construction, persistence, causal commits, provider/AI behavior, Windows AI/NPU execution, WinUI, packaging, WACK, or Store certification.
