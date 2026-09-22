# Q-PROD-05 Create Production presentation — native ARM64 validation

Date: 2026-09-22. Status: exact-source native implementation integrated; Q-PROD-05 / Q-DESIGN-22 closed.

## Identity

- Exact base: `a6bac97f3fe543d36aa28067d8c566f7d0882d7c`; Q-UNITY-01 exact-main Validation #1169 PASS.
- Initial UI candidate: `ca950c70297ff9f6bd85785ed98397c9c1835272`.
- Pre-review native candidate: `e0016e150afcdc671baa69e699e88d3a23d2d3cf`.
- Exact tested executable source after review correction: `200f2c1faa73b524353822f3ee09eced74dc019d`.
- Documentation-only successor before this acceptance closeout: `f833b07b253aea86ca6fb8e8dd2ce621cdda0725`.
- Draft PR: #247.
- Governing Design contract: `docs/design/app/contracts/CREATE_PRODUCTION_PRESENTATION.md`.
- Concurrent-candidate reconciliation: `docs/evidence/Q_PROD_05_CONCURRENT_BRANCH_RECONCILIATION_2026_09_22.md`.
- No Product/Persistence source file changed.

## Review correction

Exact-diff review of the pre-review candidate found one actionable Windows sequencing defect: the Create handler yielded to the UI event loop after capturing a pending submission but before invoking Product creation. Navigation during that yield could close the creation form and clear submitted presentation state before completion resumed.

The bounded correction at `200f2c1...` removes only that pre-Product yield. Submission now captures exact text, marks the form pending, and immediately invokes the synchronous Product boundary. The later post-success yield remains solely to allow the exact returned list-row container to materialize before keyboard focus is applied.

No Product contract, persistence behavior, visible layout, fingerprint algorithm or creator copy changed in that correction.

## Exact native ARM64 validation

Host: SurfSeven, Windows 11 ARM64, native ARM64 PowerShell 7.6.6 and .NET SDK 9.0.317.

At exact source `200f2c1...`:

- environment verifier: **PASS**, 10 projects;
- `tools/test-application.ps1 -Full`:
  - Application **66/66 PASS**;
  - Persistence **135/135 PASS**;
  - native `win-arm64`;
- `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`: **PASS**, 0 warnings / 0 errors;
- temporary ARM64 MSIX SHA-256: `348369C0EB9B138660975A0042CD0A3A3D65B522C6F283B610681334AC8F2D31`.

Publish emitted only the known symbols-package warning because `mspdbcmf.exe` is unavailable; the main Release build remained warning-free.

Hosted exact-source checks:

- push Validation #1176: **PASS**;
- PR Validation #1177: **PASS**;
- E0-E preparation #178: **PASS**.

The docs-only successor `f833b07...` also passed push Validation #1178, PR Validation #1179 and E0-E preparation #179.

## Exact native UI return

The target-device session verified:

1. **Empty library:** Productions displays **No Productions yet.** and **New Production**.
2. **Whitespace blocking:** whitespace-only name submission displays **Production name is required.** and external catalog inspection remains `COUNT=0`.
3. **Exact text:** a UI-created name containing two leading spaces, `e` + U+0301, `lan`, and two trailing spaces reopens with UTF-16 code units exactly `0020,0020,0065,0301,006C,0061,006E,0020,0020`; no trim or normalization occurred.
4. **No implicit Open:** successful creation returns to Productions and never enters Current Production.
5. **Exact return/focus:** the created ListItem receives keyboard focus after success.
6. **Unique-name presentation:** a lone `Glass Harbor` row exposes no Production code.
7. **Duplicate-name presentation:** after creating a second exact `Glass Harbor`, both rows receive distinct presentation-only codes. This run produced `596DBE15` for the seeded row and `08324EE5` for the newly created row.
8. **Exact duplicate focus:** the focused UIA name after creation was `Glass Harbor, Production code 08324EE5`, proving the returned ProductionId selected the new duplicate rather than a name match.
9. **Open/Recover regression:** explicit Open and explicit Recover remain functional on the selected duplicate row.
10. **Accessibility:** UI Automation found zero tested `ProductionPresentationRow`, `ProductionSummary`, or raw 32-hex identity leakage.
11. **Responsive/theme:** Dark and Light preserve the same information. The 720×520 creation/list states use the existing vertical ScrollViewer and show no page-level horizontal overflow; creation controls and duplicate witnesses remain reachable through ordinary vertical scrolling.

## Captures

| Artifact | SHA-256 |
|---|---|
| `dark-empty-productions-1200x800.png` | `E3762DC5D17E7B035A102DDDC8B532019F24ADF617517FE7794F7B386EEA2DCE` |
| `dark-create-production-720x520.png` | `FBF31DB1C3DBB134F286FE3F7D592E4836D987BA71A03D89D5B1E9B9C9467115` |
| `dark-duplicate-productions-1200x800.png` | `4CABBF5934885F617F1B8F443E10333C76CF61773A792FA5564FF0D309FE4DA9` |
| `light-duplicate-productions-1200x800.png` | `4BEC0BF8AD7448C5B2608A2A3DED02E60162A99A537B55FF464377923BFD442F` |
| `light-duplicate-productions-720x520.png` | `9B12C2F42B7E39394E52C05CBA669FBC1DBDC1D1032F6F30A66DF92A23D20FED` |

Scratch-only captures remain in `C:\Users\Wiryl\Sol Dev\admin-scratch\qprod05-native-200f2c1\`.

## Cleanup and limits

The temporary package was unregistered, the host's original Dark app theme was restored, and the exact-source worktree was clean.

High Contrast exact-source recapture remains unearned. Source Sans 3/S1 shipping packaging, WACK, Store and release authority remain open. FIRSTUSE and Home A/B remain unchanged. No rename/delete/import/restore, Character/Scene, provider/Performer, Stage or deferred-E0 semantics were added.

## Integration closeout

Final PR head `96fe9397c2e44894dab9769ec2d01af5798cfb7a` passed push Validation #1180, PR Validation #1181 and E0-E preparation #180. PR #247 merged to Project main at `abf7626b038625dd1a4ed32ee643d1232e011b8c`; exact-main Validation #1182 passed.

Q-PROD-05 and Q-DESIGN-22 are closed. Native/runtime authority remains bound to exact tested executable source `200f2c1faa73b524353822f3ee09eced74dc019d`. The merge and this documentation closeout do not promote unresolved FIRSTUSE/Home, High Contrast, packaging, provider, deferred-E0 or release gates.

