# H1 Patch 0001a — Native ARM64 Validation Evidence

Date: 2026-09-01
Branch: `h1-hygiene-baseline`
Scope: H1 Patch 0001a hygiene baseline only
Validation level: compiler + test execution + process-start/runtime-guard behavior actually exercised

## Toolchain evidence
- `dotnet --version`: `9.0.317`
- Project policy: stable .NET 9 via `global.json` (`9.0.100` + `latestFeature`, prerelease disabled)
- Previously established machine identity remains Windows native ARM64 (`win-arm64`, .NET host `arm64`)

## Build command
```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

Observed result:
```text
Restore complete (0.4s)
Ensemble.E0.Core succeeded (1.6s) → src\Ensemble.E0.Core\bin\Debug\net9.0\Ensemble.E0.Core.dll
Ensemble.E0.Harness succeeded (0.3s) → src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll
Build succeeded in 2.7s
```

## Test command
```powershell
dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

Observed result:
```text
Restore complete (4.2s)
Ensemble.E0.Core succeeded (0.2s) → src\Ensemble.E0.Core\bin\Debug\net9.0\Ensemble.E0.Core.dll
Ensemble.E0.Core.Tests succeeded (0.8s) → tests\Ensemble.E0.Core.Tests\bin\Debug\net9.0\Ensemble.E0.Core.Tests.dll
Ensemble.E0.Core.Tests test succeeded (0.9s)

Test summary: total: 2, failed: 0, succeeded: 2, skipped: 0, duration: 0.6s
Build succeeded in 6.7s
```

The exercised regression tests prove:
- valid current strong IDs stringify to their canonical value;
- default/uninitialized current strong IDs fail closed during stringification.

## Runtime sanity command
```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build
```

Observed output:
```text
Usage: Ensemble.E0.Harness <fixture.json>
```

Observed `$LASTEXITCODE`:
```text
2
```

## Authority interpretation
This evidence validates Patch 0001a on the user's Windows ARM64 development machine for the specific build, regression tests, and process-start/runtime-guard path exercised above.

It does not validate fixture semantics, Access Control, Context Composer, persistence, provider behavior, Windows AI/NPU execution, packaging, WACK, or Store certification.
