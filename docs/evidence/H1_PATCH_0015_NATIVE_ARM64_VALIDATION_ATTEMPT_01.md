# H1 Patch 0015 Native ARM64 Validation Attempt 01

Status: PARTIAL PASS / ONE TEST FAILURE

Validation checkpoint:

`5cb055e6dddea721aee98fee7f633191543e6490`

Audited executable/test tree remains:

`69dac983d8bb0663eda24e3b988626517f7d6218`

Machine authority supplied by the user on native Windows ARM64.

## Environment

- `PROCESSOR_ARCHITECTURE=ARM64`
- Windows RID: `win-arm64`
- .NET SDK selected by repository `global.json`: `9.0.317`
- .NET host architecture: `arm64`
- .NET host version: `10.0.11`
- tracked working-tree diff: clean (`TRACKED_DIFF_EXIT=0`)
- staged diff: clean (`STAGED_DIFF_EXIT=0`)

## Core test gate

Command:

`dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug`

Observed:

- `Ensemble.E0.Core` compilation: PASS
- `Ensemble.E0.Core.Tests` compilation: PASS
- total tests: 571
- succeeded: 570
- failed: 1
- skipped: 0
- `CORE_TEST_EXIT=1`

The console summary did not identify the failing test or assertion. The authoritative test log path reported by the runner is:

`tests\Ensemble.E0.Core.Tests\bin\Debug\net9.0\TestResults\Ensemble.E0.Core.Tests_net9.0_arm64.log`

No source correction is authorized from this summary alone. The exact failed-test identity and assertion evidence must be inspected before patching.

## Harness build gate

Command:

`dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug`

Result: PASS

`HARNESS_BUILD_EXIT=0`

The produced Harness target is native `win-arm64`.

## Missing Raft fixture gate

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json`

Observed:

`Fixture validated: ensemble.e0.missing-raft@0.1.0`

`MISSING_RAFT_EXIT=0`

Result: PASS

## Generic smoke fixture gate

Command:

`dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\e0-fixture-v1.json`

Observed:

`Fixture validated: ensemble.e0.smoke@0.1.0`

`GENERIC_SMOKE_EXIT=0`

Result: PASS

## Authority conclusion

Native compiler authority now establishes that Patch 0015 Core, tests, and Harness compile successfully on Windows ARM64 at checkpoint `5cb055e6...`.

Native runtime authority establishes both Harness fixture validations pass.

Full Core-test authority is NOT yet established because one of 571 tests failed. The next action is to extract the exact failed-test name, exception/assertion message, expected value, actual value, and stack location from the reported `.log`, then patch only the smallest affected surface and rerun the Core test gate.
