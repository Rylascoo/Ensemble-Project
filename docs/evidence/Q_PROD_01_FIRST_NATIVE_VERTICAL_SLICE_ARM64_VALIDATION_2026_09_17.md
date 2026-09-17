# Q-PROD-01 First Native Vertical Slice — Windows ARM64 Validation

Date: 2026-09-17

Status: **PROVISIONAL PRODUCT SLICE IMPLEMENTED / NATIVE ARM64 PACKAGE + RUNTIME PASS / DESIGN RETURN PACKET PARTIALLY COMPLETE / NOT FINAL ARCHITECTURE OR RELEASE AUTHORITY**

## Authority and source identity

Director sequencing authority: `docs/KYMAEAN_PRODUCT_BUILD_AHEAD_OF_DEFERRED_E0_VALIDATION_DIRECTOR_AMENDMENT_2026_09_17.md`.

Exact source checkpoint: `34a3745e12c42247fe4e1663697bf55452f9d505` on `product/q-prod-01-vertical-slice-2026-09-17`, based on Project `main@dfc641e22f26a81ad1a40cad37626aa41923eb2d`.

Implementation commits:

- `f25f5eb9756f459c248a9cce2a8b2d16edf85a91` — first native Kymaean vertical slice;
- `34a3745e12c42247fe4e1663697bf55452f9d505` — Windows-native 720×520 minimum-track contract.

The Application boundary exposes `IProductionStore`; the deterministic Missing Raft fixture is contained in `Kymaean.Infrastructure.Demo` and is not production persistence or final ontology authority. `Kymaean.Windows` owns the provisional WinUI shell and presentation layer.

## Native validation results

Validation ran on SurfSeven, native Windows ARM64, using the repository .NET 9 baseline.

- `Kymaean.Application.Tests`: **6/6 PASS** on `win-arm64` Release.
- `Kymaean.Windows` Release ARM64 compile: **PASS**, 0 warnings / 0 errors.
- unsigned ARM64 MSIX generation: **PASS**, 0 warnings / 0 errors using validation-only suppression of optional Appx symbol packaging tooling;
- package architecture: **Arm64**;
- native machine identity: `0xAA64`;
- package activation: **PASS** with a responsive `Kymaean` window from the exact unpacked validation payload.

Exact local validation artifact:

```text
C:\Users\Wiryl\Sol Dev\Ensemble-Project-LocalEvidence\q-prod-01-native-34a3745\Kymaean.Windows_1.0.0.0_arm64.msix
SHA-256 ab1b012e7dc27ad3e2f3960123bc1d18f80d092f0bdbceb87b6d43e24210b0ea
bytes   28059348
```

The local evidence directory is machine-local validation evidence, not repository authority and not a release artifact.

## UI / interaction evidence

The shell implements the current Design reference topology **Home / Productions / Settings** while keeping product-semantic spaces separate:

- Studio -> `Production shaping`;
- Stage -> `Live Stage`;
- Archive -> `History`.

Light and Dark same-state captures were generated from the final package; Stage remains invariant dark. Keyboard UI Automation on the final package proves `Production shaping -> Live Stage -> History -> Back`, and Back restores keyboard focus to `Open live stage` without mutating Production truth.

The native resize audit found clipping below the usable shell envelope. Engineering corrected this at the Win32 `WM_GETMINMAXINFO` boundary. The final package reports an exact native minimum track size of **720×520**. Programmatic `SetWindowPos` can bypass minimum-track negotiation and is not treated as user-resize evidence.

A native High Contrast observation was completed on the immediately preceding UI-identical source checkpoint before the minimum-window-only correction. An attempted exact-final High Contrast recapture was blocked by the remote-control safety layer; no bypass was attempted. Exact-final High Contrast recapture therefore remains a review-return item, not a claimed final-package validation fact.

## Design handoff boundary

Engineering consumed the active APPUI native handoff from `Rylascoo/Ensemble-Website` branch `design/appui-home-threshold-reentry-2026-09-17@4c73d02c6a1c85d860024a9e2d55b3ef490906b3`, especially `docs/KYMAEAN_APPUI_NATIVE_IMPLEMENTATION_HANDOFF_01.md` and `docs/evidence/APPUI_01_NATIVE_IMPLEMENTATION_REFERENCE_MAP_01.json`.

Q-DESIGN-20 still requires a stable Engineering ref plus the complete review-return packet. This source checkpoint is stable, but Design dispatch must wait until Project integration is durable and the remaining exact-final High Contrast observation is either obtained through a permitted local path or explicitly carried as a known review limitation.

## Non-authority

This validation does not freeze final Product navigation taxonomy, persistence format, provider runtime, typography/icon/Stage asset packaging, final architecture, Alpha/Beta/release readiness, WACK, Store behavior, or deferred E0 conclusions.

No Gemini/provider traffic occurred during Q-PROD-01 implementation or validation. The frozen E0 provider association remains untouched.

## Exact next action

Integrate this Q-PROD-01 candidate through exact-head hosted validation and exact-main validation. After durable Project integration, reconcile `CURRENT_STATE.md`/queue continuity, complete or explicitly disposition the exact-final High Contrast review item, and dispatch the stable native checkpoint to Design Sol for Q-DESIGN-20 convergence. Then continue Q-PROD-01 into production persistence/provider/UI integration without claiming final architecture.
