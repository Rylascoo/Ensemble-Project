# Q-PROD-02 / Q-DESIGN-20 R2 native ARM64 validation

Date: 2026-09-21. Status: Engineering implementation candidate; App Design acceptance pending.

## Identity and scope

- Exact base and live `main` at opening: `c27689ab690c23265e8dbe6cfc6691e428723ee0`.
- Exact tested executable source: `95d133dfbb8e6e0c5b9519dd2c0b668416c93218` on `codex/qprod02-qdesign20-r2-windows-presentation-2026-09-21`.
- Annotated source tag: `validation/q-prod-02-r2-native-arm64-ui-2026-09-21`. Earlier compile-only source `f7678811c715f3c3cfe6daed95a38737fedc0ba2` and its tag remain preserved historical evidence.
- Owning Design requirement: `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_20_POST_COMPOSITION_RECONCILIATION_01.json`; the Director-supplied fresh Design Sol read-only addendum confirms its four returns and keeps Q-DESIGN-20 acceptance pending.
- The final source correction after the earlier candidate changed only two attribute values in `MainPage.xaml`: Back's automation name and tooltip now both equal `Back`, matching the accepted Q-DESIGN-20 native source `248435944815a34e4caac761ed308ed3b07e325b`. The package implementation remains limited to `MainPage.xaml`, `MainPage.xaml.cs`, and `Presentation/MainPageViewModel.cs` relative to base.

## Engineering return

| Requirement | Exact implementation |
|---|---|
| R2-01 | Home / Productions / Settings remain the three durable NavigationView items. Successful Open or Recover shows Current Production context with no selected permanent item. ApplicationScope.CurrentProduction remains the Product truth. |
| R2-02 | Restored restrained, edge-open top and right MAT F1 subtractive field channels as non-interactive geometry; no state semantics assigned. |
| R2-03 | Removed `persisted`, `local app data`, catalog and saved-state wording from ordinary creator copy. Explicit Recover remains. |
| R2-04 | Restored a quiet system E72B Back carrier with accessible name and tooltip exactly `Back`, and native focus visuals. It returns to ProductionLibrary via existing ApplicationScope navigation and focuses the Productions item. |

NC-01 remains satisfied by absence of opportunity UI. NC-03 transparent NavigationView selected backgrounds remain. No World-current UI, Product API, persistence, provider or Design authority was changed.

## Exact native evidence

- Host: Windows 11 ARM64, native ARM64 PowerShell and .NET SDK 9.0.317; environment verifier `ENVIRONMENT_PREREQUISITES=PASS` for 10 projects.
- At exact final source, `pwsh -NoProfile -File tools/test-application.ps1`: Application **57/57 PASS**, `net9.0|arm64`, Release. Persistence was not rerun for the two XAML accessibility strings; the preceding full run at `f7678811c715f3c3cfe6daed95a38737fedc0ba2` passed Application **57/57** and Persistence **129/129**.
- At exact final source, `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`: **PASS**, 0 warnings and 0 errors.
- `tools/repository-law-check.py`: PASS; `tools/document-census.py --summary --check`: 514 inventory / 393 current / 30 historical / 91 archive / 0 unexplained, PASS at tested source; `tools/oracle-index.py --check --baseline c27689ab690c23265e8dbe6cfc6691e428723ee0`: PASS, 674 documented hashes / 17 asserted / 657 document-only. `git diff --check`: PASS.
- Independent exact-base-to-source behavioral no-write review found no actionable source defects. Effective reviewer sandbox was workspace-write, so technical read-only containment was not proven. The reviewer made no edits or artifact-writing validation runs.
- Draft PR #244 opened at candidate `c5d14a01ec150e117b05ce5c2e997ab80d949042`. At that exact head, GitHub Validation gate #1126 (`35642478667`) and E0-E preparation gate #163 (`35642478651`) both completed successfully. All three exact-SHA workflow runs were normalized by `ci-status`; the earlier Validation gate #1125 also passed. Hosted checks do not raise the native validation rung.

The first test attempt failed before restore because the managed sandbox could not read the user's NuGet configuration. A scoped approved rerun restored the pinned project packages and passed. This is a harness-access event, not a Product failure.

## Exact-source native UI return

The source `95d133dfbb8e6e0c5b9519dd2c0b668416c93218` was published as an unsigned, temporary ARM64 MSIX solely to launch the native WinUI application for this requested evidence. The MSIX is `src/Kymaean.Windows/AppPackages/Kymaean.Windows_1.0.0.0_arm64_Test/Kymaean.Windows_1.0.0.0_arm64.msix`, SHA-256 `899CC6FCB7A5451D3F1F9F2ED548DC353DAB562B780751B397E4035E25D21F14`, 27,962,967 bytes. Publish succeeded with one symbols-package warning (`mspdbcmf.exe` unavailable); the main ARM64 WinUI Release build above had zero warnings. The unpacked application was registered in an isolated scratch directory. This is an interactive source check, not WACK, Store, installed-release, or Design acceptance evidence.

The temporary test-package catalog initially contained no Productions. A single `Glass Harbor` fixture (`qprod02-native-95d133d`) was created through the real `ProductionApplication` and `FileProductionCatalog` using the existing persistence format. Its `identity.kid` SHA-256 is `0EE3EF12A35A20E9C3C739E2749C45387F535672CC3C06753FABF7DE03920C06`. Screenshots are in `C:\Users\Wiryl\Sol Dev\admin-scratch\qprod02-native-95d133d\`:

| Identity | SHA-256 | Observation |
|---|---|---|
| `dark-current-production-1200x800.png` | `AD3571FE02BA42A86683CD817E9E6CFF11A01AB814C8FD7160E94494D7AABC6E` | Dark D3 Current Production after Recover; 1200×800 native window; MAT F1 top/right subtractive channels visible. |
| `light-current-production-1200x800.png` | `B594AE45018261A2AC71806323AD53B81D0E8511E41913B52FCCB6757DA36B7C` | Same `Glass Harbor` contextual Current Production state after Open, Light F2; 1200×800 native window; MAT F1 top/right channels visible. |
| `dark-current-production-720x520.png` | `32FB6DE95260B4273C6A04A94C34BE1860E10F4E618C4703CB3847A6BEF74544` | Dark contextual Recover state at actual 720×520 bounds; Home, Productions, Settings, Back, Production title and content visible. |
| `dark-back-hover.png` | `8541814C53A2060B9D1006E76C55E22EE290826F12E30C038826E20673A6A40F` | Native pointer hover exposed a visible `Back` tooltip; UI Automation also reported `ControlType.ToolTip`, Name `Back`. |

Native UI Automation and direct observation found:

- A responsive `Kymaean` native ARM64 window launched from the exact-source package. The durable NavigationView items were exactly Home, Productions, Settings in Home, library and contextual views; Current Production was contextual content, not a fourth item.
- Selecting the fixture and invoking **Open selected Production** entered `Glass Harbor` / `Current Production`. Back had automation Name exactly `Back` and returned specifically to ProductionLibrary. UI Automation focus after return was the Productions navigation item.
- Selecting the same fixture and invoking **Recover selected Production** entered the same contextual view. The Back tooltip text was exactly `Back` on native hover.
- The 720×520 observation retained the three shell items, E72B Back, title, and core content without observed clipping. Both 1200×800 captures showed restrained, edge-open MAT F1 field geometry without Product meaning.
- Creator-facing Home, Productions and Current Production copy showed no backend, storage, catalog or persistence language; `Recover` remained the explicit earned operation. The UI Automation ListItem name for the fixture did expose `ProductionSummary { Id = qprod02-native-95d133d, ProductionName = Glass Harbor }`, while its visible creator label was `Glass Harbor`. This accessibility-tree observation predates the requested two-string Back correction and is returned for Design review; it was not changed under this scope.
- The host's original `AppsUseLightTheme=0` was restored after the Light capture. The temporary scratch-only test package was unregistered; the screenshot artifacts remain in the named scratch directory. High Contrast was not toggled: the previously recorded remote-safety limitation remains, and no permitted normal path was established for this session.

## Validation limits and next boundary

Exact-source launch, Open/Recover, Back accessibility/focus, responsive layout, Light/Dark and MAT F1 evidence is above. This evidence does not itself grant Q-DESIGN-20 acceptance. The temporary unsigned MSIX is not package-quality validation; WACK and Store checks were not run. Source Sans 3 and S1 shipping placeholders remain, and High Contrast retains the prior limitation.

Return PR #244 and this exact source/evidence to App Design Sol under Q-DESIGN-20. Design Sol alone accepts or requests correction. Do not merge without Director authorization. New World-current inspection/replacement UI remains a later separate lane; append and authoritative replay remain separately gated.
