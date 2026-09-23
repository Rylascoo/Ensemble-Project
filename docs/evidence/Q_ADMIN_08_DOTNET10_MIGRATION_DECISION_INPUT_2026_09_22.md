# Q-ADMIN-08 .NET 10 migration decision input — 2026-09-22

Status: **STATIC DECISION INPUT / NATIVE DISPOSABLE PROBE PENDING / RETARGET NOT AUTHORIZED**

Exact Project baseline: `Rylascoo/Ensemble-Project@e82e9c236a07740c5983309d713f1bc1fe07a95e`  
Baseline exact-main Validation: #1274 PASS

## Decision question

Should Ensemble retarget its active .NET 9 solution to .NET 10 before .NET 9 reaches end of support on 2026-11-10?

This record does not change `global.json`, TargetFrameworks, package versions, CI, runtime architecture or release authority.

## Current public support facts

Retrieved 2026-09-22 from current Microsoft-owned surfaces:

- .NET 9 is STS / Maintenance and ends support **2026-11-10**.
- .NET 10 is LTS / Active and ends support **2028-11-14**.
- Current official Windows App SDK WinUI C# templates support `net8.0`, `net9.0` and `net10.0`, including `net10.0-windows10.0.26100.0`.
- Microsoft.Windows.SDK.BuildTools.WinApp 0.6.1 requires .NET 8.0+ and Windows App SDK 1.4+.
- MSTest 4.1.0 / MSTest.Sdk 4.1.0 advertise .NET 8.0+ support; NuGet computes net10 compatibility on the MSTest package surface.
- Microsoft.WindowsAppSDK 2.5.1 is a current Microsoft-owned release dated 2026-09-16. Package metadata does not prove this exact Ensemble WinUI project works under .NET 10.

Sources:
- https://dotnet.microsoft.com/en-us/platform/support/policy
- https://github.com/microsoft/WindowsAppSDK/blob/main/dev/Templates/Dotnet/README.md
- https://www.nuget.org/packages/Microsoft.WindowsAppSDK/2.5.1
- https://www.nuget.org/packages/Microsoft.Windows.SDK.BuildTools.WinApp/0.6.1
- https://www.nuget.org/packages/MSTest.Sdk/4.1.0

## Exact Ensemble exposure

At the baseline:

- `global.json` requests SDK `9.0.100` with `rollForward: latestFeature`;
- eleven project files target `net9.0` or `net9.0-windows10.0.26100.0`;
- `Kymaean.Windows` targets `net9.0-windows10.0.26100.0`, minimum Windows `10.0.17763.0`;
- it references `Microsoft.WindowsAppSDK 2.5.1`, `Microsoft.Windows.SDK.BuildTools 10.0.28000.2705`, and `Microsoft.Windows.SDK.BuildTools.WinApp 0.6.1`;
- tests use `MSTest.Sdk/4.1.0`;
- current native/CI evidence belongs to the .NET 9 lineage and cannot be promoted to a retargeted executable.

## Static assessment

No current public-platform fact rules out .NET 10: current WinUI templates explicitly support net10, WinApp run tooling requires .NET 8+, and MSTest supports .NET 8+.

Unproven on Ensemble: restore resolution, all-project compile, tests, ARM64 WinUI/XAML build, native runtime, MSIX tooling, warnings-as-errors changes and deterministic/oracle stability.

## Required disposable native probe

Before a Director retarget decision, use an isolated throwaway worktree from fresh validated `main`. Do not push it or mutate authority.

Temporarily change only SDK/TFM targeting needed for measurement:
1. temporary `global.json` .NET 10 SDK selection;
2. `net9.0` -> `net10.0`;
3. `net9.0-windows10.0.26100.0` -> `net10.0-windows10.0.26100.0`.

Do not change package versions merely to obtain green. First-probe failures are evidence.

Measure restore/dependency resolution; all Product/E0/experiment builds; Application/Persistence/Core/Harness and relevant experiment tests; ARM64 WinUI Release with zero warnings/errors; TFM-sensitive repository/oracle guards; and native ARM64 launch/smoke only if compilation succeeds.

## Director dispositions after probe

- **RETARGET NOW** — dedicated .NET 10 migration package + full native revalidation.
- **RETARGET BY DATE/GATE** — retain .NET 9 temporarily with a blocking latest migration date before 2026-11-10.
- **ACCEPT UNSUPPORTED PERIOD** — explicit support/security exposure and latest exit date.

## Boundaries

No .NET 10 retarget or package upgrade is authorized here. Q-PROD-08 may proceed independently; any later retarget receives its own executable/native validation lineage.
