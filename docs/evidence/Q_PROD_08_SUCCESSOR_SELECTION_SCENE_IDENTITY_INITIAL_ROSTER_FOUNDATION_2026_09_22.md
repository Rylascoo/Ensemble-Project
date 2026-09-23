# Q-PROD-08 successor-selection audit — Scene identity / initial roster foundation

Date: 2026-09-22  
Status: **SUCCESSOR SELECTED / BOUNDED PRODUCT IMPLEMENTATION AUTHORIZED UNDER STANDING DIRECTOR DELEGATION**  
Exact baseline: `Rylascoo/Ensemble-Project@c97fb51bea2cc30c93b2e576c1bb1d9d5f22a014`  
Exact-main Validation: #1269 PASS

## Falsification

This is not the next package if Product already owns Scene identity/roster semantics, if an initial Scene roster cannot be represented without deciding Scene endings/current-Scene lifecycle/cardinality policy, or if Character Core/Constitution must be frozen first to preserve an existing dependency.

Fresh exact-source and authority review found none of those conditions.

Current Product owns Production identity/lifecycle, World current truth, persistent Character identity/name, explicit Character establishment and replay-derived Production Cast. It owns no Product Scene type or Scene event family.

Frozen Product law already distinguishes:

- Production Cast — everyone who exists in the Production;
- Scene Roster — everyone physically or socially present and eligible in a Scene;
- Current Attention / Opportunity — a separate later concept;
- Character != Performer.

The roadmap's next runtime-complete-Alpha dependency is `establish Cast/Scene`. Q-PROD-06/07 established only the Cast side.

## Why this is narrower than broad Scene work

This package does **not** resolve ODR-13 Scene endings or ODR-30 active-Scene size. It does not define a current/active Scene, Scene switching, roster add/remove, Scene completion, setting, pressure, access conditions, Opportunity, Performance, provider behavior or Stage presentation.

It establishes only the minimum persistent identity/membership substrate required before those later questions can be answered against real Product state.

## Why Character Constitution/Core is not selected first

Blueprint law defines Constitution semantically but intentionally does not freeze its durable Product representation. Product currently has no approved schema for Character Core/Constitution, Disposition or Circumstance, and transferred Design evidence explicitly leaves persistent Character schemas Product-defined.

Choosing a text blob, ordered statements, typed records or E0-derived record structure now would create a storage contract not required for the immediate Cast -> Scene dependency. Q-PROD-08 therefore leaves deeper Character state separately gated.

## Selected minimum contract

### 1. Scene identity

Introduce an Application-owned opaque `SceneId`.

- identity is not a title, display order, Production ID, Character ID or storage locator;
- no cross-Production/global-uniqueness claim is created;
- do not reuse E0 `SceneId` as Product authority;
- no final Core/assembly boundary is created.

### 2. Initial Scene roster

Introduce a `SceneRoster` containing only `CharacterId` values that already exist in the same Production Cast at the Scene-establishment event.

- duplicate Character IDs are invalid;
- roster membership means only Scene participation eligibility;
- roster membership does not imply Performer assignment, Opportunity, focus/selection, relationship, knowledge or importance;
- no minimum or maximum roster cardinality is frozen;
- no V1 “five” hypothesis or E0 “three” fixture rule becomes Product law;
- creator input order does not become dramatic/speaking/rank semantics. A replay projection may canonicalize roster display-independent order from Production Cast order.

### 3. Replay-derived Production Scenes

Add a replay-derived collection of established Scenes to `ProductionReplayProjection`.

- Scene establishment requires Production creation first;
- every roster Character must already exist in Production Cast at that event;
- duplicate Scene identity in one Production is invalid;
- Scene collection event order is deterministic provenance only, not dramatic rank/currentness;
- old histories replay to an empty Scene collection.

### 4. Causal persistence

Add one bounded creator-authority Product event family carrying exact Scene identity plus its initial roster.

Use the existing authoritative Production journal. Do not create a parallel Scene store.

Persistence/reopen/recovery/export/snapshot support must remain lossless, versioned and fail-closed, with snapshots rebuildable and non-authoritative.

### 5. Creator command boundary

Expose explicit Scene establishment only against the currently open Production through an Application/Persistence port analogous to existing bounded creation patterns.

Successful establishment returns the exact created Scene plus authoritative replay and:

- preserves Production name;
- preserves World current state;
- preserves Production Cast;
- does not change ProductSpace/navigation;
- does not make the new Scene “current”;
- does not cast a Performer;
- does not create fictional Character action.

No automatic retry, rollback or recovery semantics are inferred.

## Explicit exclusions

- creator-facing Scene title/name semantics or Scene UI;
- current/active Scene selection or switching;
- roster add/remove/reorder after establishment;
- Scene ending/completion/archival;
- Scene minimum/maximum participant policy;
- setting, Scene state, Pressure, access conditions or observation rules;
- Character Constitution/Disposition/Circumstance, relationships, knowledge, beliefs, memories or claims;
- Performer/provider/model assignment, understudies or cost;
- Opportunity/attention, Performance, Take, Rehearsal, consequence or branching;
- provider traffic or deferred-E0 consumption;
- final Core/runtime architecture, Alpha/Beta/release/WACK/Store.

## Required implementation evidence

Q-PROD-08 may change Application + Persistence + tests only, with Windows limited to compile/regression compatibility.

Require:

1. deterministic Scene identity/roster value tests;
2. replay old-history-empty-Scenes compatibility;
3. Scene-before-Production rejection;
4. unknown/non-Cast roster Character rejection;
5. duplicate Scene/roster identity rejection;
6. multiple Scene establishment with deterministic replay;
7. World and Production Cast preservation;
8. lossless journal/reopen/recovery/export/snapshot behavior;
9. no E0 Product dependency;
10. repository law/census/oracle checks;
11. native ARM64 Application/Persistence regression and ARM64 WinUI Release compile;
12. exact-candidate independent review before integration.

## Recursive audit

- **Frozen Product law:** clean; Cast and Scene Roster remain distinct.
- **ODR-13:** untouched; no ending mechanism.
- **ODR-30:** untouched; no participant limit.
- **Character != Performer:** preserved.
- **Current Attention:** not invented.
- **Character schema:** not expanded.
- **Persistence:** same Production journal remains source of truth.
- **E0 contamination:** no experimental Scene type or fixture cardinality imported.
- **Design:** no Scene UI or creator-facing naming is invented.
- **Provider/deferred-E0/release:** untouched.

No irreducible Product taste decision is required for this minimum identity/initial-membership foundation. Broader Scene lifecycle remains separately gated.
