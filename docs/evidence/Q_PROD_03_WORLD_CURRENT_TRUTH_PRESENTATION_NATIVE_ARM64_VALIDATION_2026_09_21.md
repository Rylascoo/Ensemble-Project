# Q-PROD-03 World-current truth presentation — native ARM64 validation

Date: 2026-09-21. Status: exact-source native candidate; Q-DESIGN-21 accepted for integration; PR #245 unmerged.

## Identity

- Base / adopted Q-DESIGN-21 contract main: `d77685f73f6271e4cf86902b5471da90fbe11ad4`; exact-main Validation #1147 PASS.
- Exact tested executable source: `75faeb827162c89b537d2a944473b89eabfc22ea`.
- Draft PR: #245, `qprod03/world-current-truth-presentation-2026-09-21`.
- Governing contract: `docs/design/app/contracts/WORLD_CURRENT_TRUTH_PRESENTATION.md`.
- No Product/Persistence source file changed.

## Native validation

Host: SurfSeven, Windows 11 ARM64, native ARM64 PowerShell / .NET 9.0.317.

- `tools/test-application.ps1 -Full`: Application **57/57 PASS**, Persistence **129/129 PASS**, native `win-arm64`.
- `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`: **PASS**, 0 warnings / 0 errors.
- Temporary ARM64 MSIX SHA-256: `BF50ABA83CA9E10B893BFCBA3D7364E3C1528B0EE966972FF195BCE44991D364`. Publish emitted only the known symbols-package warning because `mspdbcmf.exe` is unavailable; this does not alter the warning-free Release build.
- Exact-source hosted checks: push Validation #1151 PASS; PR Validation #1152 PASS; E0-E preparation #174 PASS.

## Exact native UI return

A scratch-only catalog contained five valid Productions before launch: Glass Harbor, Empty Harbor, Future Harbor, Broken Harbor and Fog Harbor. Incompatible/Invalid source states were injected only after each affected Production had opened so startup/list semantics were not altered.

PASS:

1. **Contextual route:** Current Production exposes `World truths`; no durable navigation item or ProductSpace was added.
2. **Non-empty inspection:** Glass Harbor displayed authoritative `Harbor bell`.
3. **Valid empty inspection:** Empty Harbor displayed `There are no current world truths.`
4. **Back/focus:** Back from Current world truths restored keyboard focus to `World truths`.
5. **Draft validation:** whitespace-only/empty row blocked with `Each world truth needs text, or remove the empty row.`; exact duplicate blocked with `Each current world truth must be unique.`
6. **Exact text:** UI replacement preserved `  élan  ` as UTF-16 units `0020,0020,0065,0301,006C,0061,006E,0020,0020`; `Zeta` preserved as `005A,0065,0074,0061`. No trim or Unicode normalization occurred.
7. **Canonical review:** Review carried `This will replace the entire current world truth set.` and used ordinally sorted proposal data.
8. **Confirmed replay:** `Replace current world truths` returned to authoritative inspection with `Current world truths updated.`; external reopen saw the exact replayed code units above.
9. **Empty whole replacement:** removing all proposal rows was reviewable and successfully produced the authoritative empty state.
10. **Incompatible:** a future replacement-contract family injected after open produced the creator-language Incompatible state, plus `Not applied`.
11. **Invalid:** corrupt committed-head state injected after open produced the creator-language Invalid state, plus `Not applied`.
12. **Environmental non-confirmation:** real journal-lock contention raised environmental I/O and produced **Confirmation unavailable**, `Kymaean couldn't confirm whether the replacement became current.`, `Last confirmed world truths`, and `Submitted replacement`. No replacement/retry control remained visible.
13. **Re-establishment:** ordinary existing Production Open after contention restored authoritative inspection and re-enabled `Edit replacement`; no new recovery/retry Product contract was introduced.
14. **Accessibility:** UI Automation found zero internal `WorldCurrentTruth`, `WorldTruthDraftItem`, `ProductionSummary`, fixture-ID or entry-path names on the tested truth surface.
15. **Theme/width:** Light F2 and Dark D3 carry the same information; 720x520 retained the World-current surface without observed clipping of essential meaning.

## Captures

| Artifact | SHA-256 |
|---|---|
| `dark-world-truths-1200x800.png` | `E1A2DA041940D7B5866411556BBE7A5BE2BFDD9DA8507AE69535337B4E93CD43` |
| `light-world-truths-1200x800.png` | `362790C190548C179B62EAF1EFD63F263D8B8C466AB07504D10577603AEB2092` |
| `dark-world-truths-720x520.png` | `B8FDAC32387C481AF81B40CE59E68F86AD916145148EAB8CDF5D345DB442062D` |
| `dark-world-truth-review-1200x800.png` | `E31942B9D9E2C3850C33597CFC355E3C904891EC30790A413D256A82CB9B502D` |
| `dark-confirmation-unavailable-1200x800.png` | `BAC674AEF023A93767451E7DB4C2F957E77CFD5ED2D959CFE48E74D9FE7C261D` |

Artifacts remain in `C:\Users\Wiryl\Sol Dev\admin-scratch\qprod03-native-75faeb8\`; screenshots are local evidence, not repository assets.

## Cleanup and limits

The temporary package was unregistered; the host's original Dark app theme was restored; the exact-source worktree was clean. High Contrast exact-source recapture was not performed. Source Sans 3/S1 packaging, WACK, Store and release authority remain open.

No history/timeline/query, Scene/current-situation, Character knowledge/belief, lifecycle, provider/Performer, Stage, retry/rollback/idempotence, Home A/B or FIRSTUSE semantics were added.
