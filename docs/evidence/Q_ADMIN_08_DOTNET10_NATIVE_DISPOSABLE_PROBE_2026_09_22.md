# Q-ADMIN-08 .NET 10 native disposable probe — 2026-09-22

Status: **PROBE COMPLETE / FRAMEWORK BUILD COMPATIBILITY PASS / TEST-RUNNER MIGRATION REQUIRED / NO RETARGET AUTHORITY**

Exact baseline: `Rylascoo/Ensemble-Project@a2ab3a4287b0b3c3e42eef961877c3a973efd9aa`  
Baseline exact-main Validation: #1277 PASS  
Host: SurfSeven — Windows ARM64

## Purpose and mutation boundary

Measure the minimum technical cost of moving the current Ensemble checkout from .NET 9 to .NET 10 without changing Product semantics, packages, source code or repository authority.

The probe used a detached throwaway worktree and was never pushed. Its only temporary tracked changes were:

- `global.json`: SDK request `9.0.100` -> `10.0.400`;
- eleven project TargetFramework substitutions `net9.0` -> `net10.0`;
- Windows TFM `net9.0-windows10.0.26100.0` -> `net10.0-windows10.0.26100.0`.

After an initial apparatus-only text-writing attempt introduced formatting noise, the worktree was reset to exact baseline before any build/test measurement and the substitutions were reapplied byte-cleanly. Final probe diff: 12 targeting-line substitutions only; `git diff --check` PASS. No package version was changed.

## Native environment

Observed on SurfSeven:

- native ARM64 OS/process;
- Windows 10.0.26200;
- RID `win-arm64`;
- .NET SDK `10.0.400`;
- .NET host/runtime `10.0.11`, ARM64;
- repository environment verifier PASS with `net10.0-windows10.0.26100.0`;
- .NET 9.0.317 remains installed alongside .NET 10.

`dotnet restore Ensemble.sln`: **PASS** with current package versions.

## Build evidence

Every retargeted project compiled under SDK 10.0.400 without source or package correction.

Build-only pass:

- `Ensemble.E0.Core` — PASS, 0 warnings / 0 errors;
- `Ensemble.E0.Harness` — PASS, 0 warnings / 0 errors;
- `Ensemble.E0.Core.Tests` — PASS, 0 warnings / 0 errors;
- `Ensemble.E0.Harness.Tests` — PASS, 0 warnings / 0 errors;
- `Kymaean.Application` — PASS, 0 warnings / 0 errors;
- `Kymaean.Infrastructure.Persistence` — PASS, 0 warnings / 0 errors;
- `Kymaean.Application.Tests` — PASS, 0 warnings / 0 errors;
- `Kymaean.Infrastructure.Persistence.Tests` — PASS, 0 warnings / 0 errors;
- `Ensemble.E0.PlaywrightControl` — PASS, 0 warnings / 0 errors;
- `Ensemble.E0.PlaywrightControl.Tests` — PASS, 0 warnings / 0 errors;
- `Kymaean.Windows` ARM64 Release / `win-arm64` — PASS, 0 warnings / 0 errors.

The Windows build produced `net10.0` Application/Persistence outputs and `net10.0-windows10.0.26100.0\win-arm64\Kymaean.Windows.dll`.

Repository-law check: **PASS**.  
Document-authority census: **PASS**.

No native application launch, MSIX/WACK or Store validation was performed; those remain later validation rungs.

## Test-runner finding — not an Ensemble test failure

No retargeted test suite reached test execution under the repository's existing `dotnet test` invocation.

Application, Persistence, E0 Core, E0 Harness and Playwright-control test projects all stop during test-platform setup with the same Microsoft.Testing.Platform error:

> Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK and later.

The resolved Microsoft.Testing.Platform package is 2.1.0. This result is a test-runner/tooling migration requirement, **not a failing Ensemble test assertion**.

Current Microsoft .NET 10 documentation states that test-runner selection is available starting in .NET 10 and that Microsoft Testing Platform is selected through `global.json`:

```json
{
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

Source: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test

The existing package version satisfies Microsoft's documented MTP >= 1.7 floor, so the first measured correction is runner selection / test-command workflow, not a package-upgrade finding. This probe intentionally did not apply that correction.

## Measured migration shape

The first probe supports these bounded conclusions:

1. **Framework/project build compatibility is strong.** The complete current project set and ARM64 WinUI Release compile unchanged under .NET 10.
2. **Current package versions did not block restore or compilation.**
3. **The current test execution workflow is incompatible with SDK 10 as configured.**
4. A real migration package must update the repository's .NET 10 test-runner selection and reconcile test commands/CI before tests can earn a pass.
5. No Product, Persistence-event, Scene, provider, Design or E0 semantic change is indicated by this probe.
6. Prior .NET 9 native validation does not transfer to a future .NET 10 executable; migration requires full fresh native validation.

## Falsification / remaining unknowns

This probe does not prove the full migration ready. A corrected migration candidate remains falsified if, after the required MTP runner transition:

- any actual test assertion fails;
- deterministic/oracle outputs change without authority;
- native WinUI launch/runtime behavior fails;
- packaging/MSIX behavior requires unsupported changes;
- a package or SDK change beyond the measured targeting/runner boundary becomes necessary.

## Role provenance

- Product/Governance analysis and probe contract: ChatGPT Project governance/Product-architecture role.
- Native execution: authorized disposable SurfSeven measurement from this Project chat.
- Independent advisory trigger: Director-supplied Claude read-only review identified the support-horizon risk; it did not review this exact probe.
- Director authority: Director agreed to the exposure guard + .NET 10 decision-input/probe sequence in the current Project chat.
- Retarget authority: **NOT GRANTED**.
- Independent review of a future executable migration candidate: still required according to normal package/review law.

## Disposition input

Technical evidence now supports treating .NET 10 as a plausible bounded migration rather than an unknown platform rewrite. The known first correction is test-runner workflow migration. The Director still chooses whether to authorize a dedicated retarget package, defer it to a named pre-EOS gate/date, or explicitly accept an unsupported interval.
