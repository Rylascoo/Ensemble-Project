# Q-PROD-02 / Q-DESIGN-20 R2 native ARM64 validation

Date: 2026-09-21. Status: Engineering implementation candidate; App Design acceptance pending.

## Identity and scope

- Exact base and live `main` at opening: `c27689ab690c23265e8dbe6cfc6691e428723ee0`.
- Exact tested source: `f7678811c715f3c3cfe6daed95a38737fedc0ba2` on `codex/qprod02-qdesign20-r2-windows-presentation-2026-09-21`.
- Annotated source tag: `validation/q-prod-02-r2-native-arm64-compile-2026-09-21`.
- Owning Design requirement: `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_20_POST_COMPOSITION_RECONCILIATION_01.json`; the Director-supplied fresh Design Sol read-only addendum confirms its four returns and keeps Q-DESIGN-20 acceptance pending.
- Only `MainPage.xaml`, `MainPage.xaml.cs`, and `Presentation/MainPageViewModel.cs` changed in the tested source.

## Engineering return

| Requirement | Exact implementation |
|---|---|
| R2-01 | Home / Productions / Settings remain the three durable NavigationView items. Successful Open or Recover shows Current Production context with no selected permanent item. ApplicationScope.CurrentProduction remains the Product truth. |
| R2-02 | Restored restrained, edge-open top and right MAT F1 subtractive field channels as non-interactive geometry; no state semantics assigned. |
| R2-03 | Removed `persisted`, `local app data`, catalog and saved-state wording from ordinary creator copy. Explicit Recover remains. |
| R2-04 | Restored a quiet system E72B Back carrier with accessible name, tooltip and native focus visuals. It returns to ProductionLibrary via existing ApplicationScope navigation and focuses the Productions item. |

NC-01 remains satisfied by absence of opportunity UI. NC-03 transparent NavigationView selected backgrounds remain. No World-current UI, Product API, persistence, provider or Design authority was changed.

## Exact native evidence

- Host: Windows 11 ARM64, native ARM64 PowerShell and .NET SDK 9.0.317; environment verifier `ENVIRONMENT_PREREQUISITES=PASS` for 10 projects.
- `pwsh -NoProfile -File tools/test-application.ps1 -Full`: Application **57/57 PASS** and Persistence **129/129 PASS**, `net9.0|arm64`, Release.
- `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`: **PASS**, 0 warnings and 0 errors.
- `tools/repository-law-check.py`: PASS; `tools/document-census.py --summary --check`: 514 inventory / 393 current / 30 historical / 91 archive / 0 unexplained, PASS at tested source; `tools/oracle-index.py --check --baseline c27689ab690c23265e8dbe6cfc6691e428723ee0`: PASS, 674 documented hashes / 17 asserted / 657 document-only. `git diff --check`: PASS.
- Independent exact-base-to-source behavioral no-write review found no actionable source defects. Effective reviewer sandbox was workspace-write, so technical read-only containment was not proven. The reviewer made no edits or artifact-writing validation runs.
- Draft PR #244 opened at candidate `c5d14a01ec150e117b05ce5c2e997ab80d949042`. At that exact head, GitHub Validation gate #1126 (`35642478667`) and E0-E preparation gate #163 (`35642478651`) both completed successfully. All three exact-SHA workflow runs were normalized by `ci-status`; the earlier Validation gate #1125 also passed. Hosted checks do not raise the native validation rung.

The first test attempt failed before restore because the managed sandbox could not read the user's NuGet configuration. A scoped approved rerun restored the pinned project packages and passed. This is a harness-access event, not a Product failure.

## Validation limits and next boundary

No exact-source UI launch, Open/Recover interaction capture, Back focus observation, Light/Dark screenshot, High Contrast recapture, MSIX packaging, WACK or Store validation was performed. The workflow does not admit packaging as a routine action in this package. Native compilation and Application/Persistence tests do not prove visual or interactive Design acceptance. Source Sans 3 and S1 packaging placeholders remain; High Contrast has its prior limitation.

Return PR #244 and this exact source/evidence to App Design Sol under Q-DESIGN-20. Design Sol alone accepts or requests correction after the remaining UI evidence. Do not merge without Director authorization. New World-current inspection/replacement UI remains a later separate lane; append and authoritative replay remain separately gated.
