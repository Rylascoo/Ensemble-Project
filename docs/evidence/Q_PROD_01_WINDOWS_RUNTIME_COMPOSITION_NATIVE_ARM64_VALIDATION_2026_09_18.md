# Q-PROD-01 Windows Runtime / Composition — Native ARM64 Validation

Date: 2026-09-18

Status: **INTEGRATED WINDOWS PRODUCTION COMPOSITION / NATIVE SOURCE AUTHORITY PRESERVED**

## Identity

- Lease: `ENG3-QPROD01-WINCOMP-01` / Issue #186.
- Exact validated base: `aa1adecbac58b87ec97d9ce6cf977deb9b7224ee`; push-triggered Validation #1020 PASS.
- Exact native source: `ac8122c24d81731671802b57220d3c631e8c9975`.
- Validation tag: `validation/q-prod-01-windows-runtime-composition-native-arm64`.
- Integrated by PR #208 to `main@032f6781c2687a07644a33532967c8e49c85437a`.
- Branch-push Validation #1021 PASS; PR exact-head Validation #1022 PASS; push-triggered exact-main Validation #1023 PASS.

## Exact source scope

Exactly ten Windows-owned paths changed, all under `src/Kymaean.Windows/**`:

- `App.xaml.cs`
- `Kymaean.Windows.csproj`
- `MainPage.xaml`
- `MainPage.xaml.cs`
- `MainWindow.xaml.cs`
- `Presentation/HistoryCountToTextConverter.cs` — removed
- `Presentation/MainPageViewModel.cs`
- `Presentation/RelayCommand.cs` — removed
- `Presentation/ShellRoute.cs`
- `WindowsStartupResult.cs` — added

No Application, Persistence implementation, provider, deferred-E0, Design, shared-authority or repository-tool source changed in the native source commit.

## Earned Windows boundary

- Production runtime composition now consumes `Kymaean.Application` + `Kymaean.Infrastructure.Persistence`; production Demo/missing-raft bootstrap is removed.
- `ApplicationData.Current.LocalFolder.Path` is supplied as the opaque app-private root to `FileProductionCatalog`.
- `ProductApplication.Start` remains the Application-owned typed bootstrap boundary.
- Windows adds only a Windows-owned infrastructure-startup discriminant for environmental failures before a usable Product application exists.
- The environmental catch is bounded to `IOException` and `UnauthorizedAccessException`; no broad `Exception` catch or Persistence-specific exception/text/layout inspection exists.
- Typed Product `Incompatible` / `Invalid` remains distinct from generic Windows infrastructure unavailability.
- Empty catalog remains a usable Product shell.
- Production open and explicit recover remain separate operations; no `RECOVERY_REQUIRED` inference or automatic recovery is introduced.
- Failed selected access does not manufacture or replace a usable current Product.
- Presentation is limited to currently earned Production library/name and Product-space orientation; no persisted Cast/Scene/Situation/Take/history/Studio/Stage/Archive content is fabricated.

## Exact-source native Windows ARM64 validation

At exact source `ac8122c...` on SurfSeven:

- Kymaean.Application tests Release win-arm64: **38/38 PASS**.
- Kymaean.Infrastructure.Persistence tests Release win-arm64: **52/52 PASS**.
- Kymaean.Windows Release win-arm64: **PASS, 0 warnings / 0 errors**.
- repository law: PASS, including portable CURRENT_STATE cap/currency and dependency direction.
- document authority census: PASS.
- oracle guard: **288 documented / 17 asserted / 271 document-only**.
- diff hygiene and clean-worktree checks: PASS.
- deterministic Windows composition probe: PASS:
  - typed Product startup failure remains typed;
  - `IOException` -> generic infrastructure state;
  - `UnauthorizedAccessException` -> generic infrastructure state;
  - exception details are not surfaced;
  - `InvalidOperationException` escapes rather than being swallowed;
  - Open routes exactly once;
  - Recover routes exactly once.
- unsigned ARM64 MSIX build: PASS.
- package contains `Kymaean.Application.dll`, `Kymaean.Infrastructure.Persistence.dll`, `Kymaean.Windows.dll`.
- package contains no `Kymaean.Infrastructure.Demo` or missing-raft payload.
- packaged launch: PASS.
- packaged relaunch against the same LocalState: PASS.
- Persistence-owned `LocalState\production-catalog` is reused across relaunch.
- temporary package registration was removed and no Kymaean.Windows process remained running.

Engineer #2 independently confirmed the final rebased Windows tree is exactly identical to the previously reviewed corrected tree and reran native Application/Persistence/Windows checks successfully.

## Integration and authority

- PR #208 exact head `ac8122c...` was mergeable and exactly one commit ahead of validated base `aa1adecb...`.
- Manager audit confirmed all ten changes remained inside Engineer #3's Windows-owned surface.
- Validation tag peels to exact native source.
- Protected expected-head merge produced `main@032f6781c2687a07644a33532967c8e49c85437a`.
- Push-triggered exact-main Validation #1023 PASS, all eight jobs.
- The temporary repository-law allowance for the legacy Windows -> Application + Demo graph is removed in the immediate manager closeout; canonical Windows graph becomes hard Application + Persistence law.

## Design / product milestone and non-authority

`DESIGN_ARCHITECTURE_READY = NOT READY`. Windows now truthfully composes the real Application/Persistence boundary, but authoritative persisted Production-internal content beyond `ProductionName` remains unearned.

This checkpoint does not establish richer Product ontology, Product creation/rename/delete, snapshot/export semantics, final concurrency/power-loss certification, provider behavior, final architecture, WACK/Store authority, or deferred-E0 conclusions.

No provider traffic occurred and no deferred-E0 namespace was consumed.
