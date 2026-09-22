# Q-DESIGN-23 successor-selection audit — Production Cast / Character establishment presentation

Date: 2026-09-22  
Status: **SUCCESSOR SELECTED / BOUNDED DESIGN CONTRACT AUDIT AUTHORIZED UNDER STANDING DIRECTOR DELEGATION**  
Exact integrated baseline: `Rylascoo/Ensemble-Project@1ca940a8235411b378aa2e50737bc261ce79494b`  
Exact-main Validation: #1226 PASS

## Falsification

This is not the next package if the integrated application still lacks persistent Character identity/Production Cast authority, if creator-facing Character establishment requires Scene lifecycle semantics in the same transition, or if current Product law already authorizes a richer Character editor.

Fresh integrated source and authority review found none of those conditions.

Q-PROD-06 now supplies:

- Application-owned opaque `CharacterId`;
- exact nonblank creator-facing Character name;
- duplicate Character names as valid Product state;
- replay-derived ordered Production Cast;
- explicit Character establishment against the current open Production;
- authoritative replay confirmation;
- lossless persistence/reopen/recovery/export and rebuildable snapshot support.

It deliberately does not supply Character rename/delete/archive/merge, deeper Character fields, Scene membership, Performer/provider assignment, Pressure, Take or Rehearsal semantics.

## Why full Scene work is not selected next

The runtime-complete Alpha path still requires:

`Create/open Production -> establish Cast/Scene -> cast Performer backend -> ...`

but current authority does not yet support treating "Scene" as one bundled implementation package.

In particular:

- ODR-13 keeps the product Scene-ending mechanism open;
- ODR-30 keeps active-Scene size a Product/UX hypothesis rather than a fixed limit;
- current App Design contracts explicitly say Scene lifecycle and membership behavior are not yet Product authority;
- Q-PROD-06 explicitly separated Production Cast from Scene Roster and excluded Scene add/remove/current-situation semantics.

Therefore a broad Scene lifecycle package would silently decide unresolved Product questions. That is not authorized.

## Existing Design evidence

The transferred/current Design archive contains strong whole-Production Character-management studies. Their reusable law includes:

- persistent Character reach belongs inside current-Production context, not a permanent top-level People destination;
- Character identity remains separate from Performer assignment;
- management selection/navigation must not imply fictional action or Scene participation;
- Back/Close preserves origin and focus rather than acting as Undo.

Those studies also contain historical/example concepts such as Off-Scene labels, Scene selection anchors, editing/shaping and richer Character detail. Those concepts are **not** automatically current Product authority. The next contract must strip unsupported semantics rather than implementing the prototype literally.

## Successor selection

The smallest correct successor is:

> **A bounded App Design contract for inspecting Production Cast and establishing a new persistent Character from the current Production.**

This package is Design/presentation definition first. Native implementation follows only after the contract is adopted.

### 1. Production Cast inspection

The current-Production surface may expose the replay-derived Production Cast.

Minimum truthful presentation:

- valid empty Cast is ordinary Product state;
- each Character is presented by creator-facing name;
- opaque Character identity is not exposed as creator-facing code merely to disambiguate ordinary unique names;
- duplicate names must remain distinguishable without redefining name as identity;
- ordering may follow deterministic replay order; Design must not imply dramatic rank, opportunity or Scene membership from that order.

Any duplicate-name disambiguation must be presentation-only and stable enough for accessibility/focus, without inventing a new Product identifier contract.

### 2. Character establishment

The creator may explicitly establish a Character using the existing `ProductApplication.CreateCharacter` contract.

Presentation law:

- exact nonblank name input;
- no trim/case-fold/normalization performed by UI;
- duplicate name is allowed and must not be blocked as a validation error;
- success is confirmed only from the returned authoritative Character + replay;
- successful creation updates the visible Production Cast;
- creation does not navigate to a Scene, assign a Performer, establish opportunity, or create fictional action/history beyond the creator-authored establishment event.

### 3. Context and navigation

The capability belongs to **Current Production** depth.

It must not:

- create a fourth durable navigation destination;
- create permanent People/Characters top-level navigation;
- imply that viewing/selecting a Character changes ProductSpace, Scene participation, selection in a future Scene, Performer assignment or fictional salience.

Back/Close should restore the invoking current-Production control and preserve effective Product state.

### 4. Failure/state presentation

The contract must distinguish:

- ordinary valid empty Cast;
- local blank-name validation before submission;
- typed `Invalid` / `Incompatible` result without claiming successful establishment;
- environmental post-submission non-confirmation honestly, consistent with existing persistence law that an append may commit before a subsequent authoritative replay/read fails.

The UI must not invent retry, rollback, recovery or idempotence semantics.

## Deliberately excluded

- Character rename/delete/archive/merge;
- Character Constitution, Disposition, Circumstance, relationships, knowledge, beliefs, memories or claims;
- Scene creation, current Scene, roster membership/add/remove, Scene endings or active-Scene maximum;
- Off-Scene/current-Scene labels until Scene membership exists in Product authority;
- Performer/provider/model assignment, understudies or cost;
- Pressure, Opportunity, Performance, Take, Rehearsal or branching;
- provider traffic/deferred-E0 consumption;
- final architecture, Alpha/Beta/release/WACK/Store;
- literal adoption of historical prototype fixture copy.

## Required contract audit

Q-DESIGN-23 should reconcile the smallest affected App Design surfaces only:

- `docs/design/app/CURRENT_CONTRACTS.md`;
- existing whole-Production Character-management evidence as constrained reference;
- shell/back/focus/accessibility law;
- current Product/Application source at the exact integrated baseline.

The resulting contract should define creator-language copy, empty/duplicate/failure states, entry/return focus, accessibility names, and Light/Dark parity without inventing Product semantics.

After Design adoption, open a separate bounded Windows implementation package using the integrated Product contract. Native implementation must receive target-device ARM64 review/validation before integration.

## Recursive audit

- **Product sequencing:** clean; consumes Q-PROD-06 without broadening it.
- **Scene boundary:** clean; no Scene lifecycle/roster law is inferred.
- **Character != Performer:** preserved.
- **Identity/display separation:** preserved; duplicate names remain legal.
- **Navigation:** current-Production depth, no People silo.
- **Persistence truth:** success is replay-confirmed; environmental ambiguity is not relabeled as failure.
- **Historical Design evidence:** constrained reference only; unsupported fixture semantics are not adopted.
- **Provider/deferred-E0:** untouched.

No irreducible Product policy decision is required to define this minimum presentation contract. The standing Design/implementation sequencing authority is sufficient to open Q-DESIGN-23 contract work.
