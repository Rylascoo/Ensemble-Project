# Production Cast / Character establishment presentation contract

Status: **ADOPTED / INTEGRATED THROUGH Q-PROD-07**

Date: 2026-09-22  
Exact Product baseline: `Rylascoo/Ensemble-Project@fcbc9677af7720711db058a303c5510ccba85aa4`  
Adoption-baseline Validation: #1235 PASS  
Integrated main: `7d79581aac869f8e81fc0a5267b1f14fde394246`; exact-main Validation #1266 PASS

## Purpose

Present the integrated Q-PROD-06 persistent Character identity / Production Cast contract without inventing Scene membership, a richer Character editor, Performer/provider assignment, or a permanent People navigation silo.

The Product contract now guarantees:

- opaque persistent `CharacterId`;
- exact nonblank creator-facing `CharacterName`;
- duplicate exact Character names are valid;
- Production Cast is replay-derived deterministic Character order;
- explicit Character creation is available only against the current open Production;
- successful creation returns the exact created Character plus authoritative Production replay;
- Character creation does not change Production name, World current state, ProductSpace or navigation;
- `Character != Performer`;
- Production Cast is not Scene Roster.

## Creator-facing terminology

The Product collection is internally **Production Cast**. The minimum creator-facing surface is labeled **Characters**.

Reason: this surface contains persistent fictional Characters, not Performer/provider assignments. Using **Characters** keeps the first native presentation literal and prevents ordinary theatrical "cast" language from being misread as actor/model assignment. This does not rename or weaken the Product primitive.

## Information architecture

- Entry lives inside contextual **Current Production** depth.
- Overview action: **Characters**.
- Heading: **Characters**.
- The capability is not Home, Productions, Settings, Stage, a fourth durable NavigationView item, or a permanent People/Characters route.
- Entering/leaving this surface does not change ProductSpace.
- **World truths** and **Characters** remain sibling current-Production capabilities; neither contains or implies the other.

## Production Characters inspection

The minimum surface is deliberately list-first and read-only.

- Show every replay-derived Production Cast Character in Product order.
- Show creator-facing Character name exactly as stored.
- A valid empty Cast is ordinary Product state.
- Empty copy: **No Characters yet.**
- Primary empty/non-empty action: **New Character**.
- No Character detail inspector is created because current Product exposes no approved Character detail beyond identity + name.
- No local "management selection" state is created merely to make the list feel interactive.
- Keyboard focus on a Character row is navigation/accessibility state only; it is not Product selection, dramatic salience, Opportunity, Scene membership or creator edit state.
- Product order must not be described as importance, chronology, speaking order, Scene order or rank.

## Duplicate-name disambiguation

Duplicate exact Character names remain Product-valid.

For a visible group with the same exact `CharacterName`:

1. serialize `CharacterId.Value` as exact big-endian UTF-16 code units;
2. SHA-256 that byte sequence;
3. render **Character code XXXXXXXX** using the first 8 uppercase hex characters;
4. if two codes collide inside the same visible same-name group, extend the colliding codes by 4 hex characters at a time until unique;
5. a full-hash collision fails closed as a presentation defect rather than inventing another identity rule.

The code appears only when disambiguation is needed. It is presentation metadata, not Product state, a storage locator, a public Character ID, an editable field, a sort key, or part of the Character name.

Accessibility name:

- unique-name row: the visible Character name;
- duplicate-name row: **<Character name>, Character code <code>**.

Implementation should share the already-adopted identity-fingerprint mechanism with duplicate Production presentation rather than maintain two semantically identical hashing algorithms.

## Character creation interaction

1. **New Character** reveals a bounded creation form within the Characters surface.
2. Field label: **Character name**.
3. The UI blocks null/empty/whitespace-only values before Product submission.
4. Accepted text is exact. The UI must not silently trim, case-fold, Unicode-normalize or rewrite punctuation.
5. Duplicate names are allowed. The UI must not treat an exact duplicate name as an error.
6. Final action: **Create Character**.
7. Secondary action: **Cancel**.
8. During the synchronous Product call, repeated submission is disabled. No percentage, fictional time, provider state, retry, rollback or recovery semantics are created.
9. On authoritative success:
   - refresh the Characters list from returned authoritative replay;
   - return to the Characters inspection state;
   - keyboard-focus the exact newly created Character row by returned `CharacterId`;
   - do not create a Product selection state;
   - do not navigate or change ProductSpace;
   - creator-facing confirmation may read **Character created.**

The establishment event is creator authority. It is not a fictional Character action, Performance, memory, Scene arrival, casting event or relationship change.

## Entry / return focus

- Opening **Characters** moves keyboard focus to the quiet Back control on the Characters surface.
- Back returns to Current Production overview and restores focus to the **Characters** entry action.
- Cancelling Character creation returns focus to **New Character**.
- Successful creation focuses the exact created Character row, using returned identity rather than matching name text.
- Navigation is not Undo. Back/Cancel never removes an already successful Character establishment.

## Failure and confirmation states

### Local validation

Blank/whitespace-only input is blocked before Product submission:

**Character name is required.**

### Typed Product failure

Existing Product `Invalid` / `Incompatible` classifications remain authoritative.

Creator-language examples:

- `Incompatible`: **A Character can't be created in this version.**
- `Invalid`: **Kymaean can't create this Character because this Production's contents are invalid.**

Do not expose contract identifiers, storage locators, journal paths, opaque Character IDs or internal Product object names.

### Environmental post-submission non-confirmation

Persistence append and subsequent authoritative replay/read are separately gated. An environmental I/O/access exception after submission does not prove whether establishment committed.

Use a distinct state:

**Confirmation unavailable**

**Kymaean couldn't confirm whether this Character was created.**

Show two separate groups:

- **Last confirmed Characters**
- **Submitted Character**

The submitted group contains the exact submitted Character name only. It is not shown inside the confirmed Characters list.

While confirmation is unavailable:

- do not assert success;
- do not assert rollback or failure;
- do not offer automatic retry/recovery;
- disable further Character creation from this surface;
- instruct the creator to open the Production again to re-establish authoritative Character state.

Ordinary existing Production Open is the re-establishment path; this contract creates no new recovery Product command.

## Visual law

Preserve the integrated shell and existing visual system:

- Home / Productions / Settings remain the only durable routes;
- Current Production remains contextual;
- Light F2 / Dark D3 carry identical meaning;
- MAT F1 restrained continuous field and quiet separator hierarchy;
- STA F2 focus/selection separation;
- quiet native Back carrier;
- teal/accent must not encode Character identity, Scene membership, Performer/provider assignment, creation success/failure or Character rank.

The Characters list should read as Production context, not a people database, gallery, cast-card wall or Stage composition.

At 720×520:

- heading/context;
- Characters list;
- New Character;
- name entry + Create/Cancel;
- duplicate Character-code witness;
- typed/non-confirmation states

must remain reachable without page-level horizontal overflow.

High Contrast exact-source proof, Source Sans 3 shipping typography and S1 packaging remain separately open; this contract does not promote them.

## Accessibility law

- Heading exposes **Characters** as the surface heading.
- Entry action accessible name: **Characters**.
- Creation action accessible name: **New Character**.
- Name field accessible name: **Character name**.
- Submit accessible name: **Create Character**.
- Back accessible name/tooltip remains **Back**.
- Unique rows expose only exact Character name.
- Duplicate rows append conditional **Character code <code>**.
- Focus and any visual hover/pressed state must not imply Product selection or Scene state.
- Status/failure/non-confirmation copy must be programmatically available without relying on color.
- Internal `CharacterId`, `CharacterSummary`, `ProductionCast`, fixture names, journal/storage paths and test identifiers must not leak into creator-facing UI Automation names.

## Explicit non-authority

This contract does not authorize:

- Character rename/delete/archive/merge;
- Character Constitution, Disposition, Circumstance, relationships, knowledge, beliefs, memories, claims or artwork;
- Scene creation, current Scene, Scene Roster, Scene membership/add/remove, Off-Scene/current-Scene labels, Scene endings or active-Scene maximum;
- Performer/provider/model assignment, understudies, availability or cost;
- Pressure, Opportunity, Performance, Take, Rehearsal, branching or transcript behavior;
- permanent People/Characters navigation;
- Home A/B or FIRSTUSE adoption;
- provider traffic or deferred-E0 execution;
- final architecture, Alpha/Beta/release, WACK or Store authority.

Historical whole-Production Character-management prototypes remain constrained reference. Their Scene labels, management-selection model, editing/shaping affordances and richer detail are not adopted by this contract.

## Native return required

Q-PROD-07 must return exact native evidence for:

1. Current Production overview exposes **Characters** without changing durable navigation.
2. Valid empty Cast shows **No Characters yet.** and New Character remains available.
3. Existing Characters render exact names in replay order.
4. Exact UTF-16 Character name creation preserves accepted code units.
5. Whitespace-only name is blocked before Product submission.
6. Exact duplicate names are allowed.
7. Duplicate rows receive distinct conditional Character codes; unique rows show no unnecessary code.
8. Character codes and row identity are presentation-only; no raw `CharacterId` leaks.
9. Successful creation refreshes authoritative Cast, preserves World current state and ProductSpace/navigation, and focuses the exact returned Character row.
10. Back restores **Characters** entry focus; Cancel restores **New Character** focus.
11. Typed `Invalid` / `Incompatible` states use creator language and preserve prior usable presentation.
12. Real environmental contention exercises **Confirmation unavailable**, separating Last confirmed Characters from Submitted Character and offering no retry/rollback.
13. Ordinary Production Open re-establishes authoritative Character inspection after non-confirmation.
14. Existing World truths presentation remains unchanged and continues to preserve Cast.
15. Light F2 / Dark D3 parity and 720×520 reachability pass.
16. UI Automation exposes zero internal Character/Product/storage/test identifiers.

Product/Persistence mutation, Scene semantics, Character-edit semantics or provider behavior are stop conditions for Q-PROD-07.

## Recursive audit

- **Product authority:** exact; every visible state maps to integrated identity/name/Cast/CreateCharacter behavior.
- **Scene leakage:** removed; historical Off-Scene/current-Scene concepts are not adopted.
- **Character vs Performer:** preserved by creator-facing terminology and explicit exclusions.
- **Identity vs display:** duplicate names remain legal; conditional code is presentation-only.
- **Complexity:** reduced from historical management prototype to list + bounded creation; no empty detail pane or fake editor.
- **Navigation:** no People silo and no new ProductSpace.
- **Persistence honesty:** environmental post-submission ambiguity is distinct from confirmed success/failure.
- **Accessibility:** exact focus/return and duplicate-name naming law are explicit.
- **Implementation duplication:** identity-fingerprint logic should be shared with existing Production presentation.
- **Visual continuity:** existing shell/F2/D3/MAT F1/STA F2 retained; no new visual system invented.
- **Migration burden:** no new Product schema or final architecture decision.
- **Provider/deferred-E0/release:** untouched.

**APPROVED BY STANDING DIRECTOR DELEGATION — CLEAN RECURSIVE AUDIT.**
