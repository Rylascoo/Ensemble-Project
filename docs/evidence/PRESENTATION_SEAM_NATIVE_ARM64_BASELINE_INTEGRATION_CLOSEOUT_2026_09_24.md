# Presentation seam / native ARM64 baseline integration closeout

Date: 2026-09-24

Status: **INTEGRATED / CLOSEOUT COMPLETE**

## Integration

Director-authorized PR #273 merged into `main` at `14133395ac40df28e193c443f077dd7c02078e31`.

The exact integrated-main push Validation gate is **#1410 / run `36077914162` PASS**. Repository law, document authority census, oracle coverage, Core/Application/Persistence regression tests, E0 compiler gate, ordinary ARM64 WinUI cross-build, Director Preview ARM64 cross-build, Presentation tests and Preview tests all passed on the merge commit.

## Exact evidence lineage

- reviewed application seam: `c11c842c9e75ce1db1555a660919562bb74c055b`;
- exact native ARM64 baseline checkout: `5205f13dd6872372f4c9ed8f7b078dabb854646a`;
- final PR head: `233b516bd267ce09c609cc2be04da4be77535627`;
- PR-head Validation #1409 PASS;
- PR-head E0-E preparation #241 PASS;
- candidate evidence: `docs/evidence/PRESENTATION_SEAM_NATIVE_ARM64_BASELINE_2026_09_24.json`.

The final PR head changed only evidence/continuity after the exact native baseline checkout. The merge commit does **not** inherit native runtime authority beyond `5205f13...`.

## Earned result

The prior folder-only Presentation boundary was insufficient because tests recompiled source rather than exercising the Presentation assembly loaded by WinUI. The integrated structure now has one platform-neutral `net10.0` `Kymaean.Presentation` assembly consumed by both WinUI and Presentation tests. WinUI-only XAML, focus/automation and converter behavior remain Windows-owned.

`tools/native-arm64-baseline.ps1` is integrated as the repeatable target-machine baseline mechanism. Its wall-clock and memory measurements are regression evidence only, not Product KPIs or an optimization claim.

## Completion doctrine

The single authorized Presentation-seam architecture decision is complete. Broad architecture analysis now stops unless executable evidence falsifies the integrated structure.

This closeout creates no Product/Persistence expansion, provider traffic/spend, NPU correctness dependency, deferred-E0 activation, Current Production workspace-composition authority, Alpha/Beta/release, WACK or Store authority.
