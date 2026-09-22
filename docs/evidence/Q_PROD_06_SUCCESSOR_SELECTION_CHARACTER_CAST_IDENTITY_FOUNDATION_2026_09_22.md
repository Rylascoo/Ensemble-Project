# Q-PROD-06 successor-selection audit — persistent Character / Production Cast identity foundation

Date: 2026-09-22  
Status: **SUCCESSOR SELECTED / BOUNDED IMPLEMENTATION AUTHORIZED UNDER STANDING DIRECTOR DELEGATION**  
Exact baseline: `Rylascoo/Ensemble-Project@f78181efc6eaac1d21e5a44635924f73a3984022`  
Exact-main Validation: #1196 PASS

## Falsification

This package is unnecessary or wrongly sequenced if live Product already persists stable Character identities/Production Cast, or if current authority requires Scene membership, Performer assignment, deeper Character ontology or provider behavior to be defined in the same transition.

Fresh source and authority review found neither condition.

## Evidence

Frozen Product law establishes:

- Characters are persistent creative entities, not prompts or models;
- `Character != Performer`;
- Production Cast is everyone who exists in the Production;
- Scene Roster is separate from Production Cast;
- a Character must survive Performer replacement without losing identity/history;
- Character reuse across Productions remains open and must not be silently decided.

The active roadmap requires `Create/open Production -> establish Cast/Scene -> ...`, while current Product source persists only Production creation and creator-replaced World current state.

Current App Design dependency evidence independently requires whole-Production persistent Character reach without making it a top-level People silo, and states that creating/selecting/editing a persistent Character must not imply Scene participation or casting.

Q-UNITY-02 removed the obsolete disconnected `WorkspaceCharacter` projection specifically so the real Product contract would not coexist with a misleading second model.

## Successor selection

The minimum correct successor is:

> **Production-owned persistent Character identity plus replay-derived Production Cast.**

Nothing deeper is required yet.

### 1. Product Character identity

Introduce an Application-owned opaque `CharacterId` and a bounded creator-facing Character summary containing only:

- stable Character identity;
- exact creator-facing Character name.

Identity is not the name, a provider/model, a storage locator, Scene membership, or presentation order.

The current contract makes no claim that a `CharacterId` is globally unique across Productions and no claim about whether future cross-Production reuse preserves or remaps it. ODR-22 remains open.

Do not reuse or reference `Ensemble.E0.Core.CharacterId`. That type belongs to the experimental E0 fixture/runtime lineage and its canonical fixture IDs are not Product persistence authority. Do not create a final `Kymaean.Core` assembly merely for this slice; final assembly boundaries remain gated.

### 2. Character name semantics

Character name is creator-facing display text, not identity.

For this minimum contract:

- nonblank exact .NET string;
- no silent trim, case-fold, Unicode normalization or punctuation rewrite;
- duplicate Character names within one Production are allowed;
- stable `CharacterId` disambiguates identity.

Allowing duplicate names is the lower-complexity Product rule and avoids making a human-readable name behave like an identifier. Any creator-facing duplicate-name disambiguation treatment is a later Design consumer, not Product state.

### 3. Production Cast semantics

Production Cast is a replay-derived collection of the persistent Characters established in that Production.

A Character entering Production Cast means only that the Character exists in the Production.

It does **not** imply:

- current-Scene participation;
- current selection/focus;
- Performer/provider assignment;
- opportunity/attention;
- Character knowledge/belief/memory;
- relationship state;
- Constitution/Disposition/Circumstance fields;
- Pressure, Take, Rehearsal or Stage behavior.

Replay order may remain deterministic event order; no creator-facing sort law is created.

### 4. Causal persistence

Add one bounded Product event family for Character creation/establishment carrying exact `CharacterId` + name.

The event belongs in the existing authoritative Production journal. Do not create a separate Character database/store.

Replay laws:

- Production creation must precede Character creation;
- duplicate `CharacterId` in one Production is invalid;
- duplicate Character name is valid;
- replay deterministically reconstructs Production Cast;
- current World state semantics are unchanged.

Persistence encoding must preserve the accepted exact string domain losslessly and remain versioned/fail-closed. Rebuildable snapshot changes remain cache-only and may version forward without becoming authority.

### 5. Creator command boundary

Character creation is explicit creator-authority work against the current open Production.

The existing catalog/persistence mutation boundary may issue a fresh opaque ID and return authoritative creation/replay state, mirroring the already-earned Production creation pattern without exposing identifier shape to UI.

Successful Character creation:

- updates authoritative replay/Production Cast;
- does not navigate or change ProductSpace;
- does not add the Character to a Scene;
- does not cast a Performer;
- does not create fictional action, memory or history beyond the creator-authored establishment event.

No automatic retry/recovery/rollback behavior is inferred.

## Deliberately excluded

- Character rename/delete/archive/merge;
- cross-Production Character portability or historical continuity;
- Scene roster/add/remove/current-situation semantics;
- Performer/provider/model assignment or understudies;
- Constitution, Disposition, Circumstance authoring;
- knowledge, belief, suspicion, memory, claim or relationship authoring;
- Pressure, Take, Rehearsal, Another Take or branching;
- Character artwork/presentation and duplicate-name UI;
- provider traffic or deferred E0 consumption;
- final Core/assembly architecture;
- final persistence/migration architecture;
- Alpha/Beta/release/WACK/Store.

## Recursive audit

- **Frozen Product law:** clean; persistent Character identity is established without weakening Character != Performer.
- **Scope separation:** clean; Production Cast does not become Scene Roster.
- **Persistence:** clean; one existing Production journal remains authority; no parallel Character store.
- **E0 contamination:** clean; experimental E0 Character IDs and fixture ontology are evidence only, not imported as Product authority.
- **Design coupling:** clean; Product establishes identity/existence only; presentation and whole-Production management remain Design consumers.
- **Migration burden:** bounded; the slice does not create a final Core assembly, portability model, lifecycle model or deeper Character schema.
- **Identity/display conflation:** clean; duplicate names remain legal and identity remains stable/opaque.
- **Provider/deferred-E0 boundary:** untouched.

No irreducible Director taste/policy decision remains inside this minimum identity foundation. The standing project-renovation delegation is sufficient to open the bounded implementation package.

## Authorized implementation boundary

Q-PROD-06 may now implement only the contract above in Application + Persistence + tests, with Windows limited to compile/regression compatibility unless a separate Design consumer is opened.

Require:

- deterministic Application replay/identity tests;
- Persistence lossless event/reopen/recovery/snapshot/export regression as affected;
- no E0 project dependency added to Kymaean Application/Persistence;
- repository law/document census/oracle guards;
- native ARM64 Application/Persistence regression;
- ARM64 WinUI Release compile;
- recursive exact-candidate review before integration.

No Character UI is authorized by this Product audit.
