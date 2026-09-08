# Repository Convergence Integrity Audit — 2026-09-08

Status: **PROMOTION CANDIDATE — final hosted CI required before `main` fast-forward**

Scope: `Rylascoo/Ensemble-Project` repository authority, branches, executable-validation boundary, document graph, continuity/bootstrap law, cross-repository residency, and reciprocal continuity controls in `Rylascoo/Ensemble-Website`. No provider traffic and no product/runtime source change were authorized by this audit.

## Falsification contract

Convergence fails if any of the following remains true at promotion:

- a live engineering branch contains unique work not incorporated or dispositioned;
- `main` would omit the accepted E0-A lineage;
- any post-`689655...` change alters `src/`, `tests/`, fixtures, project/build runtime identity, or otherwise invalidates the promoted native executable claim without new native validation;
- a current document is unexplained by the authority graph;
- `CURRENT_STATE.md` exceeds 3 KiB or more than three commits follow its last update;
- README/program-map prose independently carries volatile checkpoint/next-action authority;
- a design-native artifact is incorrectly canonical in `Ensemble-Project`, or engineering-native implementation is incorrectly canonical in `Ensemble-Website`;
- the current provider prohibition, validation rung, or Director decision is broadened by repository cleanup.

## Findings and corrections

### 1. Trunk / branch topology

Starting engineering state: `main` `7490de24...`; E0-A branch `a6e12b03...` was 140 commits ahead; evidence-lane branch `570413bc...` had one unique side commit. This made default-branch reads materially unsafe.

The convergence lineage starts at `a6e12b03...`. Durable evidence-lane content was independently reconciled, then commit `87474231...` merged `570413bc...` as a second parent without changing the accepted tree. Therefore both former work lines are ancestry-reconciled before trunk promotion; neither is being silently discarded.

Connected GitHub write tools in this session expose branch movement but not annotated-tag creation or branch-ref deletion. Repository law requires an annotated `archive/...` tag before deletion, so the old branch refs are **administratively archivable but intentionally not deleted here**. After `main` promotion they carry no unique accepted work. This limitation does not justify violating the archive-tag rule.

### 2. Executable validation identity

Promoted native executable authority remains exact checkout `689655eed677b789ab3ee395f1c65b4f2cb72cc8`, tag `validation/e0a-gemini-counttokens-correction-native-arm64`: Director Windows ARM64 Core 622/622, Harness 125/125, build/smoke/credentialless gates PASS.

Repository comparison from `689655...` through the convergence lineage found no later `src/` or `tests/` change; fixture source also predates that checkout. Convergence changes are documentation, evidence, repository tooling, and CI governance. Hosted ARM64-target compiler gates and required off-target Core regression checks passed repeatedly during convergence; those remain compiler/advisory evidence and do not replace native Windows ARM64 authority.

### 3. Document authority and continuity

Added root `AGENTS.md` as a non-phase bootstrap contract: resolve live refs first; read `CURRENT_STATE.md` at the exact active ref; treat default-branch search as discovery only; resolve validation/provider/Director boundaries before work; self-heal deterministic in-lane drift but stop on genuine authority ambiguity.

Added N=3 `CURRENT_STATE.md` commit-distance enforcement alongside the existing 3 KiB cap. The guard correctly caught a four-commit distance after the evidence-lane merge because Git ancestry includes the reconciled side-parent commit. State was refreshed rather than weakening the rule.

`tools/document-census.py` now distinguishes `current`, `historical`, and `archive` surfaces and fails closed on any unexplained current document. The strengthened first run exposed eleven previously tolerated files; ten closed/superseded records were explicitly classified historical, while the still-current Gemini 3 thought-signature audit regained an authority path from `CURRENT_STATE.md`. The next census passed with zero unexplained current documents.

README was reduced to stable orientation and pointers. `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` no longer hard-codes a stale checkpoint or immediate task; volatile checkpoint/action truth is delegated to `CURRENT_STATE.md`. An unintended punctuation-only change introduced while reconstructing that file was detected by diff and reversed in a dedicated one-line correction.

### 4. Cross-repository residency

Added `docs/REPOSITORY_RESIDENCY.md`: one canonical home; classify by authority/lifecycle rather than appearance; central product/policy/ODR decisions remain in `Ensemble-Project`; design-native UI/website/brand/design evidence belongs in `Ensemble-Website` and/or canonical Drive; ambiguous artifacts are not moved until resolved.

Manual/tree review found **no proven Design Sol artifact misplaced in `Ensemble-Project`**. Design-looking central records (`OPEN_DESIGN_REGISTER_CONTINUATION`, ODR-33, design-repository locator, ship plan) have legitimate engineering/product-policy residency. No file was moved merely because it looked visual.

Engineering CI now rejects design-native top-level workspaces (`assets/`, `prototypes/`, `site/`, `intelligence/`, `updates/`). Reciprocal website governance rejects engineering-native `src/`, `tests/`, `Ensemble.sln`, and `Directory.Build.props` roots.

`Rylascoo/Ensemble-Website` received the reciprocal exact-ref bootstrap, 3 KiB + N=3 state discipline, and residency boundary on commit `b02dd14c1121d06d3868e0aa73b3758ef96ee193`. Its document-status and publication/continuity/residency workflows passed, and that commit was fast-forwarded to website `main`. No render, criterion, prototype, pixel, design score, or selection changed.

### 5. Provider / product boundary

Attempt-03 interpretation is unchanged: first Performer full-request `countTokens` HTTP 400 before generation; same-key/header/model/method simple body succeeded; authentication transport is closed as the explanation; exact rejected field remains unproven.

Director decision remains unchanged: 3.5 Flash-Lite live execution PAUSED; bounded Harness-local provider-error/request-compatibility engineering amendment AUTHORIZED; 3.1 Flash-Lite provider execution DEFERRED; **Provider authorization NONE**. Repository convergence made no credential, `countTokens`, generation, probe, inference, spend, fallback, or other provider request.

## Promotion gate / stop condition

Before `main` moves, this candidate head must pass the full hosted gate: ARM64-target compiler build, required x64 Core regression (non-native authority), repository-law enforcement, oracle assertion coverage, and fail-closed document census. After that success, fast-forward `main`; then write the closed state and create a fresh bounded-diagnostic branch from the reconciled trunk.

Recursive audit stop condition: one complete pass finds no material correction, inconsistency, unexplained current document, wrong-lane artifact, executable drift, unincorporated unique engineering work, or worthwhile simplification inside the authorized convergence scope. Administrative branch-ref deletion remains separately constrained by the annotated-tag requirement and available writer surface.
