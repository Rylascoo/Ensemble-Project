# Website App-Design Migration Completeness Audit — 2026-09-24

Status: **COMPLETE / MATERIAL APP CONTINUITY PRESERVED / NO PRODUCT OR DESIGN DECISION CHANGED**

## Scope

This was a one-time forensic audit before `Rylascoo/Ensemble-Website` is renovated into a website-focused repository.

Exact boundaries:

- Project base: `34ff50c23b411da33d16e615ace780088b257f4a`
- Q-ADMIN-05 Website source: `846c60ee448abf7d5c57bdd64de55cb9294880e2`
- Website pre-renovation snapshot: `43801c0be01f7892b42b7127468e844b9e2aa822`

The audit covered current Website `main`, all 176 non-main remote branch refs visible at audit time, the frozen Q-ADMIN-05 receipt/oracle, and current Project app-design authority. Exact branch heads and branch-side audit-surface counts are preserved in `docs/evidence/WEBSITE_APP_DESIGN_MIGRATION_COMPLETENESS_CENSUS_2026_09_24.json`.

## Census and subtraction

- 996 distinct branch-side blob variants were observed across changed paths.
- A conservative filename/content-family pass produced 512 app/shared/unclassified candidates not byte-identical to the pre-renovation snapshot.
- 153 were exact Q-ADMIN-05 source-blob matches.
- The remaining 359 candidates were reviewed by source path, branch provenance, semantic title/status/content family, and current Project representation.
- Q-ADMIN-05 remains sound as the active app-design transfer: no second current application-authority graph was found.
- The 51 commits between Q-ADMIN-05 source and the pre-renovation snapshot changed 19 files; those changes are Website routing/transition, Website production/Reference-17, ledger/history, and repository tooling rather than a new app-authority program.

## Material continuity recovered

### Q-ADMIN-05 D01/D02 pointer evidence

Q-ADMIN-05 deliberately left Home A/B and FIRSTUSE as exact Website-pinned evidence. Eight source objects are now mirrored byte-for-byte under:

`docs/design/app/archive/website-audit-2026-09-24/pointers/`

This removes day-to-day dependence on live Website branch refs while retaining the original pinned refs as provenance. It does **not** select Home A/B or adopt FIRSTUSE.

### Branch-only Phase-C predecessor capsule

A second bounded set was materially valuable despite not being current authority. The exact Website source checkpoint is `405c8d014300e8154f1cd5dc0e0c326d0498533a` (historical branch `design/phase-c-completion-audit-2026-09-13`).

- **CPS-01:** CPS-B / “Distributed Recognition Mesh” was explicitly Director-selected as the Phase-C Character-presence architecture.
- **DUR-01:** the frozen reduction/accessibility matrix closed `PASS_INTEGRATED_DURABILITY`.
- **APPICON-01:** E06 was the closed single-survivor exact-C0 app-icon deployment envelope; shipping/package/native icon authority remained explicitly unearned.

Their canonical evidence, packets, deterministic carriers and probes are mirrored under:

`docs/design/app/archive/website-audit-2026-09-24/phase-c/`
Because the CPS-01 decision explicitly derives from IMG-01, four minimal IMG-01 decision-context records are mirrored beside that capsule: the frozen method, family definitions, method-bound evaluation, and Director-prior concordance that selected CPS-01 as the successor gate. Raw renderer outputs, renderer packets and shared creative masters remain outside Project.

These are **historical predecessor references only**. Current Project-native Stage, Character, accessibility and visual-system contracts control current work. The archive cannot reopen Phase C, promote IMG-F1 into final identity, override current contracts, select a shipping app icon, or create implementation authority.

## Reviewed but not imported

- Intermediate Phase-1/2/3 AppUI handoffs, probes and predecessor prototypes: later Project contracts/reference carriers represent the surviving law.
- IMG-01 raw renderer outputs/shared masters and CHARART/mascot artwork: not imported; canonical visual masters remain Drive-owned. Only the four IMG-01 decision-context records required to make CPS-01 provenance locally intelligible are mirrored.
- MOT/TYP/SYM and other shared research harnesses: current Project contracts already carry the selected app-facing law; source experiment machinery is historical/cross-surface evidence.
- Website hero, Reference-17, wordmark/Website production and site-specific motion: Website-owned.
- Branch-local ledgers, generated indexes, temporary workflows and corpus tooling: repository history/tooling, not application-design authority.
- `renovation/w1-txt-corpus-coverage`: Website repository-tooling work, not app material. No Project transfer is warranted; Website ownership must disposition its unique commit before branch retirement.
- Character/mascot exploration material: not imported.
- Existing Bellweather mentions in Project: retained as transferred historical evidence/non-authority boundaries; no blanket rewrite is justified.

## Website branch dependency result

Project does not require the **live branch refs** for:

- `design/appui-home-threshold-reentry-2026-09-17` once its exact D01 source identities are archive-tagged/preserved;
- `design/appui01-working-compositions-2026-09-14`, whose accepted app-design source identities are already sealed by Q-ADMIN-05;
- `design/odr-33-resolution-2026-09-06`, which is Website/history rather than a current app dependency.
`renovation/w1-txt-corpus-coverage` remains a Website-side content-disposition question, not a Project dependency.

Branch deletion remains Website governance and must follow the Director-authorized archival cleanup rule; this audit grants no Website mutation authority.

## Integrity

`docs/design/app/TRANSFER_RECEIPT.json` is unchanged. Imported mirrors retain exact source bytes. `MANIFEST.json` records source ref, path, blob, bytes and SHA-256 for every mirrored object.

No Product semantics, runtime behavior, validation rung, ODR result, current Design decision, provider/spend authority, or release gate changes.
