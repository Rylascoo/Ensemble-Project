# Established Scenes / initial roster presentation contract

Status: **DESIGN PROPOSAL / STATIC SUFFICIENCY ACCEPTED BY DESIGN / DIRECTOR REVIEW PENDING / IMPLEMENTATION NOT AUTHORIZED**

Package: **Q-DESIGN-24**. Date: 2026-09-23.
Exact Product baseline: `a225fc56acc1ef4514538562491d28d7a6bb4fb4`.
Decision and falsification evidence: `docs/design/app/evidence/SCENE_PRESENTATION_CONTRACT_DECISION_2026_09_23.md`.

## Authority and purpose

The Director authorized a documentation-only test of integrated Q-PROD-08 sufficiency: opaque Scene identity, initial Scene Roster, Production-Cast-order canonicalization and replay-derived Production Scenes. This proposed contract closes that Design question without extending Product. Director review of this return and separate implementation authorization remain required.

Product inputs are `src/Kymaean.Application/SceneIdentity.cs`, `SceneCreation.cs`, `ProductionReplay.cs`, `ProductApplication.cs` and `src/Kymaean.Infrastructure.Persistence/FileProductionCatalog.cs` at the exact baseline. Q-PROD-08 authority and native evidence remain in `docs/evidence/Q_PROD_08_SUCCESSOR_SELECTION_SCENE_IDENTITY_INITIAL_ROSTER_FOUNDATION_2026_09_22.md` and `docs/evidence/Q_PROD_08_SCENE_IDENTITY_INITIAL_ROSTER_NATIVE_ARM64_VALIDATION_2026_09_22.md`.

This contract proposes presentation; it does not rename Product types or amend their semantics. Initial membership is not evidence of current presence, Character observation, knowledge, Performer assignment or eligibility for an actual running performance.

## Context and inspection

- Add a contextual **Scenes** capability beside **Characters** and **World truths** within Current Production. No durable navigation item or ProductSpace change.
- Heading: **Scenes**. Introductory copy: **Established Scenes and their initial rosters.**
- Render all established Scenes in replay collection order. Do not describe this as dramatic sequence, rank, timeline or currentness. No sorting/reordering control.
- Each Scene has a derived **Scene code** and an **Inspect initial roster** action. The action enters a bounded read-only detail surface; inspection is presentation context only.
- Empty collection: **No Scenes yet.** Keep **New Scene** available even when the Cast is empty.
- Detail heading: **Initial roster**. Show the Scene code and roster Character names, resolved by Character identity against the authoritative Production Cast, in canonical Cast order.
- Empty roster: **No Characters in this initial roster.** This is valid data, not an invalid, ended or incomplete Scene.
- Nonempty roster copy describes **Initial roster**, never “present now,” “active,” “on stage” or “off-scene.” No roster-edit action.
- Missing or inconsistent references fail closed as a presentation/data-boundary defect; do not omit members, substitute names or invent Characters.

## Distinguishing unnamed Scenes

Every Scene displays **Scene code XXXXXXXX**, including a sole Scene. Codes are derived from exact `SceneId.Value`, never from roster contents, display index or name.

1. Encode the identity as exact big-endian UTF-16 code units, without normalization or replacement of isolated surrogates.
2. Compute SHA-256 and begin with eight uppercase hexadecimal characters.
3. Resolve prefix collisions across **all established Scenes in this Production's authoritative replay**, not a viewport, filtered subset or open detail. Extend colliding codes by four characters until distinct.
4. A full-hash collision is a fail-closed presentation defect: do not offer ambiguous per-Scene actions or silently invent another Product distinction. Return for Design review; no Product mutation follows.

Use the existing presentation fingerprint convention as a precedent, not a claim that today's helper already implements this Scene contract. Codes are rendered text with programmatic equivalents. They may wrap without truncation; they are not color, icon, position or hover-only distinctions.

The same replay produces the same codes. Addition of a prefix-colliding Scene can lengthen an existing displayed code; no permanently stable short-code guarantee is made. Focus and detail context must always track opaque Product identity internally, never code text or list position. Recompute list/detail labels together from the same replay.

Codes are neither stored nor submitted as identity. They are not editable names, global identifiers, public identity APIs, dramatic order, rank, currentness, lifecycle or creator-authored durable semantics. No raw opaque identity or storage locator is displayed. Identical-roster and empty-roster Scenes remain independently inspectable because their codes derive from distinct Scene identities.

## Character distinction and initial-roster draft

**New Scene** opens a bounded form headed **New Scene**, with **Initial roster** and instructions: **Choose Characters from this Production. You can leave the initial roster empty.**

- Show every Character in Production Cast order with a labeled checkbox. Initially none are checked; never infer a selection from a previous Scene or draft.
- Preserve exact Character names and duplicate-name legality. Compute conditional Character codes from the **entire current Production Cast**, using the adopted Q-DESIGN-23 algorithm in `docs/design/app/contracts/PRODUCTION_CAST_CHARACTER_ESTABLISHMENT_PRESENTATION.md`.
- Reuse that same Cast-wide name/code mapping in the picker, roster detail, review and submitted-roster witness. Do not remove a duplicate's code merely because its namesake is absent from a particular roster.
- Checkboxes represent a local draft only. Checking a Character does not modify an established Scene, create a Character, make one present, select a Performer or grant Opportunity.
- The draft holds Character identities, not name/code matching. Show a **Review initial roster** summary in Cast order, including the valid empty state, before the final action **Establish Scene**. Review may be inline; it is not another Product state or mandatory extra wizard page.
- All Cast members remain reachable with vertical scrolling. No three/five-member cap, minimum-member validation, suggested speaking order, select-by-rank or cardinality policy.
- Empty Cast: **No Characters in this Production yet. You can establish a Scene with an empty initial roster.** Do not require Character creation or import an implicit cross-flow command.
- **Cancel** discards only the unsubmitted draft and returns to Scenes. No persistent Scene name field.

## Submission and results

Capture the submitted roster identities and their exact displayed name/code witnesses before the synchronous existing Application call. Disable repeated submission and navigation during that call; do not add an event-loop yield that allows the draft/context to change before invocation.

Call only the existing explicit `EstablishScene` operation against the open Production. Product canonicalization remains authoritative. Do not infer success from local draft validity.

On authoritative success, refresh all Scene and Cast presentations from the returned replay, clear the draft, return to Scenes, announce **Scene established.**, and focus the newly returned Scene's inspection action by its Product identity. Do not auto-open its detail, activate a Scene, navigate to Stage or change ProductSpace.

Existing typed failures remain distinct:

| Result | Creator-facing presentation | Return |
|---|---|---|
| `Incompatible` | **A Scene can't be established in this version.** | Back to last confirmed Scenes; no retry, upgrade, migration or recovery command invented. |
| `Invalid` | **Scene not confirmed. Kymaean couldn't confirm whether this Scene was established from the Production's returned contents.** | Preserve the submitted witness and require ordinary reopening under the non-confirmation rules below. Do not claim the Production is corrupt or that nothing committed. |
| Environmental I/O/access exception during submission | **Confirmation unavailable. Kymaean couldn't confirm whether this Scene was established.** | See non-confirmation rules below. |

Do not expose internal contract identifiers, exception text, journal paths or opaque IDs. Programming/presentation defects are not reclassified as Product `Invalid` or environmental non-confirmation. Preserve the typed `Invalid` result separately from an environmental exception even though both need conservative submission handling. These are bounded presentations of the current port, not new failure semantics.

The port does not prove non-commit for `Invalid`: persistence can append successfully, then Application can reject the returned replay against its cached projection (for example, another instance established a Scene since this instance opened the Production). Do not diagnose corruption or permit repeated submission from that stale projection. Unsupported concurrent use remains unsupported; this contract adds no multi-writer guarantee.

## Non-confirmation and ordinary reopening

For typed `Invalid` as well as environmental submission exceptions, keep two explicit groups: **Last confirmed Scenes** and **Submitted initial roster**. The latter contains only the captured Character name/code witnesses or **No Characters in this submitted initial roster.** It has no Scene code, because no authoritative created identity was returned through a successful Application result. Never insert a guessed Scene row.

Copy: **Open the Production again to inspect its recorded Scenes. A matching roster cannot confirm which submission created a Scene.**

Disable further Scene establishment until ordinary existing Production Open successfully re-establishes authoritative state. Back may leave the surface; revisiting it without that successful Open retains the uncertainty and creation block. Failed Open does not clear them. Do not add automatic retry, rollback, recovery, idempotency tokens or a persistent pending-submission schema.

Successful Open replaces last-confirmed presentation with its authoritative replay. It does not retrospectively mark the uncertain submission successful or failed. Multiple Scenes with identical rosters are lawful; neither a matching roster nor collection position identifies the attempted creation. Another explicit establishment is a **new** creator action, never “retry.” These transient witnesses are session-only; no durable completion notification or cross-restart attempt tracking is promised.

The Q-PROD-08 append repair returns exact candidate replay under the journal gate. Do not repeat Q-DESIGN-23's old separate-post-append-read explanation as current implementation fact. Environmental uncertainty is still handled conservatively; no new confirmation guarantee is invented.

## Focus, return and accessibility

| Transition | Required focus |
|---|---|
| Current Production -> Scenes | Quiet Back control; announce Scenes heading. |
| Scenes -> initial-roster detail | Detail Back; heading and Scene code are programmatically associated. |
| Detail -> Scenes | Exact originating Scene inspection action, keyed by Scene identity. |
| Scenes -> New Scene | Form heading/instructions; next Tab reaches first checkbox, or Establish Scene when Cast is empty. |
| Cancel -> Scenes | New Scene action. |
| Confirmed establishment -> Scenes | Exact returned Scene inspection action. |
| Incompatible -> Scenes | New Scene action; submission draft is discarded, not resubmitted. |
| Invalid or environmental non-confirmation | Focus its distinct status heading; Back remains available. Returning to Scenes without successful Open focuses its uncertainty notice, not a disabled New Scene action; retain the submitted witness. |
| Scenes -> Current Production | Scenes entry action. |

If a refreshed authoritative projection lacks a remembered focus identity, use the Scenes heading; never focus a different Scene by matching roster/code/index. Navigation and focus are not Undo, activation or Product selection.

Scene row accessible name: **Scene code <code>**; associated action: **Inspect initial roster for Scene code <code>**. Character checkbox names include exact name and conditional **Character code <code>**; checked state is separately exposed by the native control. Status and errors must be programmatically announced without color dependence. No essential content is available only through tooltips or hover. Long names/codes and large rosters remain reachable with wrapping/vertical scrolling at 720x520, scaling and keyboard use.

Preserve Home / Productions / Settings, contextual Current Production, quiet Back, Light F2 / Dark D3 parity, MAT F1 and STA F2 focus/selection separation. No accent/color encodes identity, roster eligibility, status or currentness. This package earns no High Contrast, screen-reader, native layout or runtime PASS.

## Falsifier and excluded authority

STOP and return the smallest Product decision if usable, accessible inspection/establishment requires persistent creator-authored Scene names or another missing Product semantic. Do not hide the deficiency in a label, local persistent state, fake selection model or reinterpretation of membership.

Static case analysis supports sufficiency for this bounded task; actual usability/native accessibility is not tested. Codes support exact distinction, not evocative naming or memorability. If Director review or later authorized native evidence finds code-based distinction inadequate, reopen the falsifier before implementation expansion.

No Windows implementation is authorized. No Product/Persistence schema, persistent Scene names, current/active Scene, switching, lifecycle, roster mutation, endings/cardinality law, Stage activity, Character Core, Performer/provider, Opportunity/Performance, Take/Rehearsal, consequence/branching, provider traffic, deferred-E0 execution, final architecture or release/WACK/Store authority is created.

## Evidence required only if implementation is later authorized

Native return must cover empty/multiple/identical-roster Scenes; empty Cast/roster; more than five members; duplicate exact Character names with Cast-wide codes; deterministic Scene code distinction and injected prefix/full-collision handling; long/wrapping names/codes; exact identity focus/return; successful authoritative establishment; typed failures including post-append Application rejection from stale caller state; real environmental uncertainty without guessed attribution; Invalid/environmental uncertainty surviving in-session navigation until successful ordinary Open; Light/Dark, 720x520, keyboard and UI Automation. These are future acceptance criteria, not executed evidence or permission to run them.
