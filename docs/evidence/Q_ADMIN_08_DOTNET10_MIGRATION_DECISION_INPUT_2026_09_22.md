# Q-ADMIN-08 .NET 10 migration decision input — 2026-09-22

Status: **NATIVE DISPOSABLE PROBE COMPLETE / DIRECTOR DISPOSITION PENDING / RETARGET NOT AUTHORIZED**

Exact probe baseline: `Rylascoo/Ensemble-Project@a2ab3a4287b0b3c3e42eef961877c3a973efd9aa`  
Baseline exact-main Validation: #1277 PASS

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

## Native probe result

Completed on SurfSeven against exact validated baseline. Evidence: `docs/evidence/Q_ADMIN_08_DOTNET10_NATIVE_DISPOSABLE_PROBE_2026_09_22.md`.

Measured result:

- environment verifier and solution restore PASS under native ARM64 SDK 10.0.400;
- all eleven retargeted projects build with current packages and zero warnings/errors;
- ARM64 WinUI Release build PASS under `net10.0-windows10.0.26100.0`;
- repository law and document census PASS;
- actual test execution is blocked before assertions because the current .NET 10 invocation defaults to VSTest while the resolved Microsoft.Testing.Platform 2.1.0 rejects that path;
- Microsoft documentation identifies `global.json` test-runner selection `Microsoft.Testing.Platform` as the .NET 10 correction path;
- no package upgrade, source correction or Product-semantic change was required by the first probe;
- native app launch/package/release validation remains unearned.

The probe therefore narrows migration cost from “unknown project compatibility” to a bounded test-runner/workflow migration plus full fresh validation.

## Director dispositions after probe

- **RETARGET NOW** — dedicated .NET 10 migration package + full native revalidation.
- **RETARGET BY DATE/GATE** — retain .NET 9 temporarily with a blocking latest migration date before 2026-11-10.
- **ACCEPT UNSUPPORTED PERIOD** — explicit support/security exposure and latest exit date.

## Boundaries

No .NET 10 retarget or package upgrade is authorized here. Q-PROD-08 may proceed independently; any later retarget receives its own executable/native validation lineage.
