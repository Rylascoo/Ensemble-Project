# Q-PROD-07 Production Characters presentation — native ARM64 validation

Date: 2026-09-22  
Status: **VALIDATED / EXACT-SOURCE REVIEW CLEAN / DESIGN ACCEPTANCE READY**  
Exact integrated base: `4f836d53a4e18ae92fa408cc87a9d9e2c3702fbe`  
Exact executable source: `9ea74391adf0a2f259c205ae88128324d4a52148`  
Governing Design contract: `docs/design/app/contracts/PRODUCTION_CAST_CHARACTER_ESTABLISHMENT_PRESENTATION.md`

## Scope

Q-PROD-07 changes Windows presentation only.

Changed executable files:

- `src/Kymaean.Windows/MainPage.xaml`
- `src/Kymaean.Windows/MainPage.xaml.cs`
- `src/Kymaean.Windows/Presentation/MainPageViewModel.cs`
- `src/Kymaean.Windows/Presentation/CharacterPresentationRow.cs`
- `src/Kymaean.Windows/Presentation/PresentationIdentityCode.cs`
- `src/Kymaean.Windows/Presentation/ProductionPresentationRow.cs`

No Product/Application/Persistence semantic source, provider/E0 source, fixture, CI or release configuration changed.

## Implementation

The candidate adds:

- current-Production contextual **Characters** sibling to **World truths**;
- read-only replay-derived Character rows;
- valid empty **No Characters yet.** state;
- bounded **New Character** / **Character name** / **Create Character** flow;
- exact creator text preservation and duplicate-name legality;
- conditional presentation-only Character codes;
- exact returned-Character focus by `CharacterId` without Product selection state;
- typed Invalid/Incompatible creator-language failure presentation;
- environmental **Confirmation unavailable** with separate **Last confirmed Characters** and **Submitted Character**;
- ordinary Production Open/Recover re-establishment;
- exact Back/Cancel focus return.

The earlier Production-code SHA-256/UTF-16BE algorithm was factored into one Windows-presentation helper shared by Production and Character rows. Product identity remains opaque and unchanged.

## Recursive implementation audit and corrections

### Compiler correction

Initial source `526e0984e64119c7fd912dcc31e4c042b1127abc` failed the ARM64 compile because the new shared duplicate-code dictionary is keyed by identity string while `ProductionPresentationRow` still looked up with the `ProductionId` wrapper.

Correction `9ea74391adf0a2f259c205ae88128324d4a52148` changed only that lookup to `production.Id.Value`. No Product semantics or visible presentation law changed.

### Exact-source no-write review

A fresh no-write review of exact executable source `9ea74391adf0a2f259c205ae88128324d4a52148` against integrated base found no remaining substantive finding.

Reviewed:

- source scope is Windows presentation only;
- Character states are explicitly partitioned from World-truth states;
- successful creation uses synchronous `ProductApplication.CreateCharacter` without pre-Product event-loop yield;
- authoritative replay refresh and exact returned-ID row focus;
- typed failures preserve prior confirmed presentation;
- environmental ambiguity asserts neither success nor rollback;
- ordinary Open/Recover clears ambiguity from authoritative replay;
- no Character management-selection Product state is invented;
- no Scene/Performer/provider/Pressure/Take/Rehearsal semantics enter changed source;
- shared Production/Character fingerprint algorithm is deterministic and fail-closed;
- no new durable navigation or ProductSpace transition is introduced.

Result: **CLEAN**.

## Native Windows ARM64 evidence

Host: SurfSeven, Windows ARM64, native ARM64 PowerShell / .NET SDK 9.0.317.

At exact source `9ea74391adf0a2f259c205ae88128324d4a52148`:

- application environment verifier: **PASS**, 9 projects;
- Application tests: **72/72 PASS**, native `win-arm64`;
- Persistence tests: **141/141 PASS**, native `win-arm64`;
- `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`: **PASS, 0 warnings / 0 errors**;
- final validator worktree: clean;
- `git diff <base> --check`: clean.

A fresh exact-source ARM64 MSIX was also produced for disposable validation:

- SHA-256: `88CE8B964615396496C7C24413B6187F7CD1691589F45519DC685399C36888CD`;
- 27,978,853 bytes;
- package publish emitted only the known symbols-package warning because `mspdbcmf.exe` is unavailable;
- the ordinary Release build above remained warning-free.

Unsigned-package deployment was rejected by Windows because the manifest publisher is not in the unsigned namespace. This was a packaging-validation setup result, not a Product failure. Native UI validation instead used a disposable development registration of the exact generated build layout plus unchanged tracked assets. No release/package authority is claimed.

## Exact native UI return

A deterministic scratch-only Production `Character Harbor` was established with authoritative World current truth `Harbor bell`.

PASS:

1. **Contextual entry:** Current Production exposes **Characters** next to **World truths**; durable navigation remains exactly Home / Productions / Settings.
2. **Empty Cast:** empty authoritative Cast shows **No Characters yet.** and **New Character**.
3. **Entry focus:** opening Characters places keyboard focus on **Back**.
4. **Whitespace validation:** whitespace-only name displays **Character name is required.** before Product submission.
5. **Exact UTF-16:** UI-created `  élan  ` replayed as exact code units `0020,0020,0065,0301,006C,0061,006E,0020,0020`; no trim or Unicode normalization occurred.
6. **Success/focus:** successful establishment displays **Character created.** and keyboard-focuses the exact returned Character row.
7. **Duplicate-name legality:** a second exact `  élan  ` is accepted. The two rows expose distinct presentation-only codes `B9F48982` and `7AEE4B9A`; the second returned row holds keyboard focus.
8. **Fingerprint identity:** those two codes equal the first eight uppercase SHA-256 hex characters of the exact UTF-16BE code-unit serialization of their authoritative Character IDs.
9. **Back/Cancel:** Character Back restores focus to **Characters**; Cancel restores focus to **New Character**.
10. **World regression:** **World truths** still presents authoritative `Harbor bell`; World Back restores **World truths** focus.
11. **ProductSpace/navigation:** ordinary reopen shows Product space **Stage**, plus sibling **World truths** / **Characters**; Characters is not a durable NavigationView item.
12. **Incompatible:** after opening the Production, injecting a future Character-created contract family produces **Character not created** + **A Character can't be created in this version.** while preserving the prior confirmed Character rows.
13. **Invalid:** after opening the Production, corrupting committed-head state produces **Character not created** + **Kymaean can't create this Character because this Production's contents are invalid.**
14. **Environmental non-confirmation:** real exclusive `.journal.lock` contention produces **Confirmation unavailable**, exact non-confirming copy, separate **Last confirmed Characters** and **Submitted Character**, and no New Character control.
15. **Re-establishment:** after releasing contention, ordinary Production Open clears non-confirmation, restores **New Character**, and authoritative replay does not contain the uncommitted submitted `Wren`.
16. **Accessibility:** both normal and non-confirmation UIA scans found zero `CharacterId`, `CharacterSummary`, `ProductionCast`, replay type, raw 32-hex Character ID, catalog locator, entry path or test-identifier leakage.
17. **720×520:** UIA reports no horizontal scrolling and a real vertical ScrollPattern; scrolling 0→100 brings **New Character** on-screen. Heading and duplicate rows remain accessible.
18. **Light/Dark:** the same duplicate-Character state was captured under Dark and Light application themes with identical information.
19. **Shared Production-code regression:** after adding a second exact `Character Harbor` Production, the existing library still displays conditional Production codes `81F1004D` and `F904DEBD`, exactly matching the identities under the shared helper.

## Captures

| Artifact | SHA-256 |
|---|---|
| `dark-characters-720x520.png` | `393058FA2CF3D1887F9DD4BF5EE5F94E6EAEE492E024AAF9CA054F00CC3442F2` |
| `light-characters-720x520.png` | `3095B1C89649A39D0DF64C064D5D6CF86A03CB1B976B689FB060985B34FCBE3B` |

Scratch-only evidence remains under `C:\Users\Wiryl\Sol Dev\admin-scratch\qprod07-native-9ea7439` and the scratch helper under `qprod07-helper-9ea7439`. These are validation apparatus, not repository or shipping assets.

## Hosted gates

At exact executable source, Validation #1254 had all executable jobs PASS and failed only repository law because `CURRENT_STATE.md` was stale by eight commits.

After documentation-only checkpoint refresh, head `2a72a9f7521bd66e1a3211f4bc74a1491fc497c0` passed Validation #1256 completely, including:

- Product Application regression;
- Product Persistence regression;
- Core regression;
- ARM64 WinUI compiler gate;
- E0 compiler gate;
- repository law;
- document authority census;
- oracle assertion coverage.

The documentation commit does not inherit native runtime authority; native/UI evidence remains bound to exact executable source `9ea74391adf0a2f259c205ae88128324d4a52148`.

## Cleanup and limits

- disposable package registration removed;
- original host app theme restored to Dark;
- validator worktree clean at exact executable source after resolving a no-content EOL/stat marker whose normalized blob already equaled HEAD;
- no provider traffic or deferred-E0 execution;
- High Contrast exact-source proof remains open;
- Source Sans 3 / S1 shipping packaging, WACK, Store and release remain open;
- no Character edit/deeper ontology, Scene membership/lifecycle, Performer/provider, Pressure, Take or Rehearsal semantics are created.

Q-PROD-07 is ready for Q-DESIGN-23 native acceptance and final integration closeout.
