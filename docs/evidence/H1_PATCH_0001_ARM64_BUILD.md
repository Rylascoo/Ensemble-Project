# H1 Patch 0001 — Native ARM64 Compiler Evidence

Date: 2026-09-01
Branch: `h1-source`
Scope: H1 Patch 0001 only
Validation level: compiler authority

## Machine evidence
- OS platform: Windows
- OS version: 10.0.26200
- RID: `win-arm64`
- .NET host architecture: `arm64`
- Installed .NET SDK used for project after `global.json` pin: `9.0.317`
- Installed .NET 9 runtime: `Microsoft.NETCore.App 9.0.19`

## Command
```powershell
dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug
```

## Observed result
```text
Restore complete (0.5s)
Ensemble.E0.Core succeeded (1.9s) → src\Ensemble.E0.Core\bin\Debug\net9.0\Ensemble.E0.Core.dll
Ensemble.E0.Harness succeeded (0.3s) → src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll
Build succeeded in 3.2s
```

## Authority interpretation
This evidence proves that the current H1 Patch 0001 Core and Harness source compiles successfully on the user's native Windows ARM64 development machine with the intended .NET 9 SDK.

It does not prove runtime behavior, fixture semantics, Access Control, context composition, persistence, NPU execution, packaging, WACK, or Store certification.

## Next validation gate
Run the compiled H1 Harness once on the same machine to validate process startup and the runtime architecture guard before promoting Patch 0001 beyond compiler-validated status.
