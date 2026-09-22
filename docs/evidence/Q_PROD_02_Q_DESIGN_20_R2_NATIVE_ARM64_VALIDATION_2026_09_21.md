# Q-PROD-02 / Q-DESIGN-20 R2 native ARM64 validation

Date: 2026-09-21. Status: Engineering implementation integrated; App Design accepted; Q-PROD-02 / Q-DESIGN-20 closed.

## Identity and scope

- Exact base and live `main` at opening: `c27689ab690c23265e8dbe6cfc6691e428723ee0`.
- Exact accessibility-corrected executable source: `150f7c57b523f6d95e8f85a5b1c20ad769c3a120` on `codex/qprod02-qdesign20-r2-windows-presentation-2026-09-21`.
- Prior full visual/native interaction source: `95d133dfbb8e6e0c5b9519dd2c0b668416c93218`, preserved by `validation/q-prod-02-r2-native-arm64-ui-2026-09-21`. Earlier compile-only source `f7678811c715f3c3cfe6daed95a38737fedc0ba2` and its tag remain historical evidence.
- Owning Design requirement: `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_20_POST_COMPOSITION_RECONCILIATION_01.json`.
- Candidate Design acceptance: `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_20_R2_NATIVE_ACCEPTANCE_2026_09_21.json`.
- Relative to the fully captured `95d133...` source, the final correction changes only Windows accessibility exposure for the generated Production row: the `ListView` container-creation event assigns the creator-facing `ProductionName` as the Automation name. It changes no Product/Persistence contract, visible creator copy, MAT geometry, theme role or navigation behavior.

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
- At exact accessibility-corrected source `150f7c...`, `pwsh -NoProfile -File tools/test-application.ps1`: Application **57/57 PASS**, `net9.0|arm64`, Release. Persistence was not rerun because the final delta is Windows-only UI Automation naming; the preceding full run at `f7678811c715f3c3cfe6daed95a38737fedc0ba2` passed Application **57/57** and Persistence **129/129**.
- At exact accessibility-corrected source `150f7c...`, `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`: **PASS**, 0 warnings and 0 errors.
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
- Creator-facing Home, Productions and Current Production copy showed no backend, storage, catalog or persistence language; `Recover` remained the explicit earned operation. At this full visual source, UI Automation exposed the generated ListItem as `ProductionSummary { Id = qprod02-native-95d133d, ProductionName = Glass Harbor }` even though the visible creator label was `Glass Harbor`. The acceptance review classified that assistive-technology exposure as a bounded R2-03 accessibility defect; the exact-current correction and verification are recorded below.
- The host's original `AppsUseLightTheme=0` was restored after the Light capture. The temporary scratch-only test package was unregistered; the screenshot artifacts remain in the named scratch directory. High Contrast was not toggled: the previously recorded remote-safety limitation remains, and no permitted normal path was established for this session.

## Exact-current accessibility correction

At exact source `150f7c57b523f6d95e8f85a5b1c20ad769c3a120`, the generated Production-row container now receives `AutomationProperties.Name = ProductionName` during container creation. This is presentation/accessibility plumbing only; it does not change the visible row, Product identity, persistence format, Open/Recover semantics, MAT F1 geometry, theme roles or Back behavior.

Native Windows ARM64 verification at this exact source established:

- Application tests: **57/57 PASS**.
- WinUI Release ARM64 build: **PASS**, 0 warnings / 0 errors.
- Temporary ARM64 MSIX SHA-256: `F0E0FF8C7CE857FC1832F6CBCCD0C681D14366BA570205EFB981F208172BDB9F`. Publish emitted only the previously understood symbols-package warning because `mspdbcmf.exe` is unavailable; the main Release build remained warning-free.
- A real `Glass Harbor` fixture was created through the existing Application/Persistence path in the temporary package LocalState.
- Native UI Automation on the Productions surface reported ListItem names `Home`, `Productions`, `Settings`, and exactly `Glass Harbor`.
- `GLASS_HARBOR_EXACT_COUNT=1`, `INTERNAL_LEAK_COUNT=0`, `UIA_ROW_NAME_PASS=True`.
- The temporary package was unregistered and scratch fixture/package material was cleaned after the check; the worktree remained clean.

The full Light/Dark, MAT F1, Open/Recover, Back/focus and 720×520 screenshots remain bound to visually identical source `95d133...`; the exact-current delta has no visual styling or layout effect.

## Validation limits and next boundary

Design acceptance is **Q-DESIGN-20 = ACCEPTED / INTEGRATED** at exact accessibility-corrected source `150f7c...`, recorded in `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_20_R2_NATIVE_ACCEPTANCE_2026_09_21.json`. PR #244 merged at `2470ed1068454e3ff5cfd573dcc266328bd1d1cc`; push-triggered exact-main Validation #1145 passed.

High Contrast exact-source recapture remains unearned because no normal permitted toggle path was established; no bypass was attempted. Source Sans 3 and S1 shipping/native packaging remain unresolved placeholders. The temporary unsigned MSIX is not package-quality, WACK, Store or release validation.

Q-PROD-02 and Q-DESIGN-20 are closed. Q-DESIGN-21 may now define the bounded creator World-current inspection/replacement presentation contract. No World-current UI implementation is authorized until that contract is adopted and a separate Engineering package opens; append and authoritative replay remain separately gated.
