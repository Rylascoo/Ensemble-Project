# Q-PROD-06 Character / Production Cast identity foundation — native ARM64 validation

Date: 2026-09-22  
Status: **INTEGRATED / INDEPENDENT EXACT-CANDIDATE REVIEW CLEAN**  
Baseline main: `0cfadc6121a187335df3016d925ea0584b40a1b9`  
Draft PR: #249  
Exact executable source: `90905c8952f77a7d6f926350fa5ada0796be56bf`
Authorization / successor selection: `docs/evidence/Q_PROD_06_SUCCESSOR_SELECTION_CHARACTER_CAST_IDENTITY_FOUNDATION_2026_09_22.md`

## Earned scope

The candidate implements only the authorized Q-PROD-06 foundation:

- Application-owned opaque `CharacterId`;
- exact nonblank creator-facing Character name with no silent trim/case-fold/Unicode normalization;
- duplicate Character names allowed; identity remains the stable disambiguator;
- `CharacterCreatedEvent` in the existing authoritative Production journal;
- deterministic replay-derived Production Cast;
- explicit Character creation against the current open Production;
- lossless versioned Character event persistence, reopen, explicit recovery and portable-export replay;
- projection snapshot cache version 3 containing World current state plus Production Cast while remaining rebuildable/non-authoritative.

It does not add Character UI, Scene membership, Performer/provider assignment, Character knowledge/belief/memory, relationships, Constitution/Disposition/Circumstance authoring, Pressure, Take, Rehearsal, provider traffic, deferred-E0 consumption or final Core/architecture authority. `Character != Performer` remains preserved.

## Recursive review repairs

The first no-write exact-candidate review of `f656561b6c08f0e7e5e73e2a27a8e3e043534d44` found that `ProductApplication.ReplaceWorldCurrentState` could accept a writer-returned replay whose Production Cast changed. The bounded repair at `60949870cb0f1e28adf8ac0b6a42fa826b06eb8d` plus regression `a89a90c820b57c3307a668a5ee4e68384d3ce930` now requires Cast equality across World-current replacement and preserves prior Application state on mismatch.

A subsequent recursive cache audit found snapshot v3 trusted malformed truth/Character counts as `List` capacities before proving the corresponding bytes existed. Repair `02ab5c4f75fc74abe186debfac1f7ee2951dc269` removed untrusted preallocation; exact executable `90905c8952f77a7d6f926350fa5ada0796be56bf` adds checksummed oversized-count regressions proving malformed cache falls back to authoritative journal replay.

A fresh no-write review of exact `90905c8952f77a7d6f926350fa5ada0796be56bf` against baseline main found no further substantive correctness, authority, scope, replay, event-codec, recovery/export, snapshot, command-invariant or test finding. Existing per-Production fail-fast concurrency law remains unchanged; no retry/rollback/global transaction was introduced.

## Native Windows ARM64 evidence

Host: Director Windows ARM64 device SurfSeven.

Environment verifier PASS on exact source:

- native ARM64 OS and process;
- repository .NET SDK policy PASS;
- Windows target/SDK prerequisites PASS;
- nine-project solution prerequisites PASS.

Remote process launch omitted the standard `ProgramFiles(x86)` environment variable even though `C:\Program Files (x86)\Windows Kits\10` exists. The verifier was rerun with only that standard path supplied process-locally; no repository or persistent machine configuration was changed.

Exact native regression:

- `tools/test-application.ps1 -Full` — Application **72/72 PASS**;
- `tools/test-application.ps1 -Full` — Persistence **141/141 PASS**;
- `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64` — **PASS, 0 warnings / 0 errors**;
- exact detached worktree HEAD `90905c8952f77a7d6f926350fa5ada0796be56bf`;
- `git status --short` clean and `git diff --check` clean.

## Hosted exact-source gates

Exact executable source `90905c8952f77a7d6f926350fa5ada0796be56bf`:

- Validation #1211 — **PASS**;
- E0-E preparation #186 — **PASS**;
- Product Application regression — PASS;
- Product Persistence regression — PASS;
- ARM64 WinUI cross-compile — PASS;
- repository law, document authority census and oracle assertion coverage — PASS.

No provider traffic or deferred-E0 execution was performed.

## Evidence boundary

Native runtime/test authority belongs only to executable source `90905c8952f77a7d6f926350fa5ada0796be56bf`. Subsequent evidence/state commits may describe this result but do not inherit or inflate native authority.

Documentation closeout head `63cf46c0aaa4947316e451ec3ab0d534f3d53b25` passed Validation #1220 and E0-E preparation #190 after the successor-selection provenance edge was restored. Final PR head `784ac6dc7b324bb5daf12eaaf65086c3834fe1ec` passed Validation #1225 and E0-E preparation #192.

PR #249 merged normally at `1ca940a8235411b378aa2e50737bc261ce79494b`. Push-triggered exact-main Validation #1226 PASS confirmed the integrated merge commit. Q-PROD-06 is closed/integrated. Native authority remains bound to executable source `90905c8952f77a7d6f926350fa5ada0796be56bf`; merge and documentation commits do not inflate it.
