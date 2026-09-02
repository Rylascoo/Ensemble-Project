# H1 Patch 0001 — Native ARM64 Runtime Sanity Evidence

Date: 2026-09-01
Branch: `h1-source`
Scope: H1 Patch 0001 only
Validation level: target-device runtime authority for process startup and architecture-guard behavior only

## Precondition
The same machine had already produced a successful native Windows ARM64 .NET 9 build of the H1 Core and Harness projects.

## Command
```powershell
dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build
```

## Observed output
```text
Usage: Ensemble.E0.Harness <fixture.json>
```

Immediately afterward:

```powershell
$LASTEXITCODE
```

Observed:

```text
2
```

## Authority interpretation
The Harness checks `OperatingSystem.IsWindows()` and `RuntimeInformation.ProcessArchitecture == Architecture.Arm64` before checking command-line arguments. Reaching the expected missing-fixture usage path therefore demonstrates that this compiled Harness process started successfully on the target Windows machine and passed its ARM64 runtime guard.

Exit code `2` is the intentional result for missing/invalid command-line arguments and is not a runtime failure.

This evidence does not validate fixture loading/semantics, Access Control, Context Composer, persistence, provider/model behavior, NPU execution, packaging, WACK, or Store certification.

## Result
H1 Patch 0001 now has:
- native Windows ARM64 compiler evidence;
- native Windows ARM64 process-start/runtime-guard evidence.

The next engineering boundary is H1 Patch 0002: the full typed Missing Raft fixture schema and deterministic semantic invariant validation.
