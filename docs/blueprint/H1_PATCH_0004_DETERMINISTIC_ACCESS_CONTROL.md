# H1 Patch 0004 — Deterministic Character-Bounded Access Control

Status: blueprint proposal 0.2 — APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0003
Branch: `h1-patch-0004-access-control-blueprint`

## 1. Purpose

Implement the first deterministic information-authority boundary between validated Production state and later context composition.

Patch 0004 answers one narrow question:

> Given a validated E0 Fixture Dialect v1 fixture and one Character in its Scene roster, can Ensemble deterministically derive the complete set of information that Character is permitted to receive, while excluding Production-only truth and every other Character's private state before any semantic/relevance system can inspect it?

Patch 0004 does **not** compose prompts, choose relevance, call a model, implement the Director, build ProductionState, or create provider/runtime orchestration.

## 2. Authority and continuity

The following law is already frozen or canonical before Patch 0004:

- `Production State -> deterministic Access Control -> permitted Character information -> Context Composer -> bounded context -> Performer`.
- Access Control answers **what may this Character receive**. Context Composer answers **what matters now among already-permitted material**.
- A probabilistic or semantic system may never see prohibited information and merely be instructed not to use it.
- The E0 access contract is `ensemble.e0.character-bounded.v1`.
- Fixture-authored arbitrary ACLs are forbidden; access derives from authority category, structural Character ownership, Scene roster membership, and the known access contract.
- Production-authoritative `HistoricalTruth`, `UnresolvedProposition`, and `WorldState` are not Character-access authority in E0. A Character receives subjective knowledge/belief/etc. that is explicitly owned by that Character instead.
- `SceneState` is shared opening Scene information for the E0 roster.
- root-level `Pressure` is public E0 pressure and is available to the Scene roster.
- a Character receives their own `Constitution`, `Disposition`, `Circumstance`, `Observation`, `Knowledge`, `Belief`, `Suspicion`, `Memory`, `Goal`, and outbound `Relationship` records; they do not receive another Character's private collections or inbound relationship records merely because those records target them.
- provenance explains evidence/derivation and **never grants access**.
- an allowed record may not leak inaccessible provenance source content or source IDs into Character-facing context.
- E0-A through E0-G rely on character-bounded context as the reference architecture; the E0-D omniscient-context condition is a deliberate experimental ablation, not a production Access Control mode.

This document checkpoints those rules as the canonical implementation specification for Patch 0004 and resolves the implementation-level gaps that are not yet represented in GitHub.

## 3. Architecture boundary

Input:

`ValidatedFixture + CharacterId`

Output:

`CharacterAccessEvaluation`

where:

- `CharacterAccessEvaluation.Projection` is the immutable, Character-safe projection that a later Context Composer may consume;
- `CharacterAccessEvaluation.Decisions` is a local deterministic audit trail of permit/deny decisions and must **not** be supplied to Context Composer or a Performer.

Access Control consumes only the already validated immutable fixture. It does not mutate `ValidatedFixture`, rewrite fixture records, alter ECJ-1 identity, or create new authority.

Preferred implementation shape:

- one explicit `CharacterBoundedAccessControl` for the one currently supported access contract;
- one small immutable `CharacterAccessProjection`;
- small stripped projection records that cannot carry hidden provenance;
- one deterministic access-decision audit representation.

Do not add:

- an ACL language;
- policy files;
- fixture-authored allow/deny lists;
- interfaces/DI/strategy registries for hypothetical future access contracts;
- role-based access frameworks;
- claims/policy engines;
- reflection-driven projection;
- model-assisted access classification;
- a Context Composer implementation;
- an omniscient/debug switch in the safe Access Control path.

If later product requirements genuinely introduce another access contract, refactor from the proven single-contract boundary then.

## 4. Exact `character-bounded.v1` access matrix

For a requested Character that is present in the current Scene roster:

| Fixture information | Character access |
| --- | --- |
| Scene identity | PERMIT |
| Scene roster Character IDs and display names | PERMIT |
| `HistoricalTruth` | DENY |
| `UnresolvedProposition` | DENY |
| `WorldState` | DENY |
| `SceneState` | PERMIT all |
| root `Pressure` | PERMIT all |
| requested Character `Constitution` | PERMIT all owned |
| requested Character `Disposition` | PERMIT all owned |
| requested Character `Circumstance` | PERMIT all owned |
| requested Character `Observation` | PERMIT all owned |
| requested Character `Knowledge` | PERMIT all owned |
| requested Character `Belief` | PERMIT all owned |
| requested Character `Suspicion` | PERMIT all owned |
| requested Character `Memory` | PERMIT all owned |
| requested Character `Goal` | PERMIT all owned |
| requested Character outbound `Relationship` | PERMIT all owned |
| every other Character subjective record | DENY |
| every other Character outbound `Relationship` | DENY |
| fixture chronology | DENY / not part of Character projection |
| fixture initial opportunity | not part of Access Control projection; later opportunity/Director input remains separate |
| fixture/schema/access/observation contract metadata | local authority metadata; not Character content |
| record provenance | never included in Character-facing projection in Patch 0004 |

### Why Production truth remains excluded

E0 deliberately models epistemic asymmetry through Character-owned state. If Voss knows the current strengthened, Voss receives `KNOW-VOSS-CURRENT-STRENGTHENED`; she does not receive `WORLD-CURRENT-STRENGTHENED`. If Wren knows she secured the raft, she receives `KNOW-WREN-SECURED-RAFT`; she does not receive `HT-WREN-SECURED-RAFT`.

The same rule applies even when Character-owned content is semantically consistent with Production truth. Structural authority category, not semantic similarity, controls access.

### Why SceneState and Pressure are shared

Patch 0002.2 explicitly freezes Missing Raft SceneState as common opening conditions and requires Patch 0004 to preserve that disclosure intent. E0 Fixture Dialect v1 defines root Pressure as public to the Scene roster.

Access Control therefore permits these categories before relevance selection. Context Composer may later choose which permitted records matter for a specific opportunity, but it may not widen the set.

## 5. Public Scene identity metadata

For `character-bounded.v1`, every roster Character may receive the minimal structural identity of everyone co-present:

- `CharacterId`;
- `DisplayName`.

This is the bounded answer to “who is present.” It does not expose another Character's Constitution, Disposition, Circumstance, observations, knowledge, beliefs, suspicions, memories, goals, relationships, or provenance.

The Character-safe projection may therefore contain:

```text
SceneId
SubjectCharacterId
Roster: [ CharacterId + DisplayName ]
```

Roster entries canonicalize by `CharacterId` using ordinal ordering so semantically irrelevant source-array order does not affect the deterministic projection.

`InitialOpportunity` is intentionally excluded from this projection. Opportunity/attention is Director or run-control state, not Character information authority. A later Context Composer may combine the safe access projection with a separately authorized current-opportunity input.

## 6. Character-facing projection types must strip provenance

Do **not** pass `ValidatedRecord`, `ValidatedRelationship`, `ValidatedCharacter`, or `ValidatedFixture` into Context Composer as the Access Control output.

Those types retain authoritative provenance and/or unrelated private state. In Missing Raft, an allowed record can cite a Production-authoritative source that the Character is not permitted to receive. Merely passing the source ID can itself leak semantically meaningful hidden information.

Patch 0004 therefore introduces the smallest safe projection payloads:

```text
PermittedRecord
- RecordId
- Text

PermittedRelationship
- RecordId
- TargetCharacterId
- Text

SceneParticipant
- CharacterId
- DisplayName
```

No Character-facing projection record contains provenance.

This does not destroy Production provenance. The authoritative `ValidatedFixture` retains its complete DAG locally. Access Control is a disclosure projection, not a replacement source of truth.

Future local diagnostics and experimental provenance may inspect authoritative provenance independently; the Performer-facing path may not.

## 7. `CharacterAccessProjection` shape

The projection should preserve authority categories rather than flatten all permitted text into one generic list.

Preferred shape:

```text
CharacterAccessProjection
- SceneId
- SubjectCharacterId
- Roster
- SceneState
- Pressures
- Constitution
- Disposition
- Circumstance
- Observations
- Knowledge
- Beliefs
- Suspicions
- Memories
- Goals
- Relationships
```

Each record collection is an immutable array sorted by canonical `RecordId` using ordinal comparison. Relationships sort by their relationship Record ID.

The projection is the **maximal permitted set** for that Character under the access contract. Access Control does not perform relevance filtering, token budgeting, summarization, semantic selection, deduplication, paraphrase, or prompt formatting.

A later Context Composer may only select from or transform this permitted projection under its own approved contract.

## 8. Deterministic access-decision audit

E0 experimental provenance requires access decisions to be attributable and reviewable. Patch 0004 therefore produces a deterministic local decision for every fixture record exactly once.

Preferred minimal representation:

```text
AccessDecision
- RecordId
- Disposition: Permit | Deny
- Reason
```

Stable reason values:

- `OwnedBySubject`
- `SharedSceneState`
- `PublicPressure`
- `ProductionAuthorityExcluded`
- `OwnedByOtherCharacterExcluded`

All decisions sort ordinally by `RecordId`.

The audit does **not** carry record text or provenance. Record IDs are sufficient because fixture Record IDs are globally unique.

`CharacterAccessEvaluation` keeps the two surfaces explicit:

```text
Projection    -> later Context Composer may consume
Decisions     -> local audit / experiment provenance only
```

The future Context Composer API must accept the projection, not the entire evaluation wrapper. This prevents denied Record IDs from becoming a new disclosure side channel.

Patch 0004 does not yet serialize, hash, persist, or log access decisions. It only makes the deterministic result available so later harness/provenance work can do so without recomputing policy differently.

## 9. Provenance rule

Access is determined without provenance traversal.

Algorithmically:

- permit or deny a record from its authority category and structural owner only;
- never inspect a record's provenance to determine permission;
- never recursively add provenance sources;
- never copy provenance source IDs into the Character-facing projection;
- an independently permitted source remains independently permitted because of its own category/ownership rule, not because another record cites it.

Examples:

- `KNOW-VOSS-CURRENT-STRENGTHENED` is permitted to Voss because Voss owns it. `WORLD-CURRENT-STRENGTHENED` remains denied despite being its provenance source.
- `KNOW-WREN-SECURED-RAFT` is permitted to Wren. `HT-WREN-SECURED-RAFT` remains denied.
- `SCENE-RAFT-GONE` is permitted to all roster Characters. `HT-CURRENT-CARRIED-RAFT-AWAY` remains denied despite being its provenance source.
- Marlowe's release Knowledge is permitted to Marlowe. The Production HistoricalTruth record remains denied as Production authority even though Marlowe's subjective Knowledge is consistent with it.

This is required to preserve epistemic category boundaries rather than merely hide selected prose.

## 10. Relationship directionality

Relationships are Character-owned directional state.

A Character receives only relationships structurally nested under that Character.

Therefore:

- Marlowe may receive `REL-MARLOWE-VOSS` and `REL-MARLOWE-WREN`;
- Voss may receive `REL-VOSS-MARLOWE` and `REL-VOSS-WREN`;
- Wren may receive `REL-WREN-MARLOWE` and `REL-WREN-VOSS`;
- no Character receives an inbound relationship merely because they are its target.

Target `CharacterId` remains visible inside the subject's permitted relationship because the target is a co-present roster identity.

Patch 0004 does not implement the E0-D “relationships omitted” ablation. That later control operates only on already-permitted context and must not be encoded as an Access Control policy variant.

## 11. Chronology and historical IDs

Fixture chronology is Production-authoritative experiment structure and is not Character-facing context.

This matters because canonical IDs may themselves be semantically descriptive. Passing a chronology containing `HT-MARLOWE-RELEASED-RAFT`, for example, would leak the hidden event even if its prose were removed.

Character history reaches a Performer through permitted Character-owned memories/knowledge/etc., not through the Production chronology.

Patch 0004 therefore does not include chronology or its Record IDs in `CharacterAccessProjection`.

## 12. Fail-closed behavior

`CharacterBoundedAccessControl` operates only on a non-null `ValidatedFixture` and a concrete Character identity.

It fails closed when:

- the requested Character does not exist in the validated fixture;
- the requested Character is not in the current Scene roster;
- the fixture is not governed by the supported `ensemble.e0.character-bounded.v1` contract;
- an internal condition required to construct an unambiguous projection is violated.

Use one small Access-Control-specific domain exception rather than misclassifying runtime access failure as story output or provider failure.

Do not silently return an empty projection for an invalid subject. Empty access can be a valid semantic result for a category; invalid authority selection must remain distinguishable.

Patch 0004 may trust invariants already guaranteed by `ValidatedFixture` and must not duplicate the entire generic fixture validator. Under E0 Fixture Dialect v1, every validated Character is already required to be in the Scene roster; therefore the separate “Character exists but is not rostered” branch is defensive only and is not a separately constructible public test fixture state.

For Missing Raft, known-family structural/hash validation remains a prerequisite in the Harness pipeline before Access Control. Generic Access Control itself must not depend on `MissingRaftContract`.

## 13. Determinism and ordering

For identical validated semantic input and subject Character, Access Control must produce the same projection and decision sequence regardless of source ordering of semantically unordered arrays.

Canonical ordering rules:

- roster by `CharacterId`, ordinal;
- each permitted record collection by `RecordId`, ordinal;
- relationships by relationship `RecordId`, ordinal;
- decisions by `RecordId`, ordinal.

Chronology is not part of the projection.

No culture-sensitive ordering, dictionary-enumeration authority, filesystem state, clock, randomness, network, process-global mutable state, or model output may affect access.

Patch 0004 does not define an AccessProjectionHash. ContextPacketHash/StateHash remain later work.

## 14. Missing Raft exact expectations

The generic matrix must produce these qualitative results for the frozen Missing Raft fixture:

### MARLOWE

Permitted:
- shared SceneState and `PRESSURE-ISOLATION`;
- Marlowe Constitution/Disposition;
- Marlowe observations;
- Marlowe release/not-warned Knowledge;
- Marlowe beliefs;
- Marlowe RH-001/RH-002 memories;
- Marlowe Goal;
- Marlowe's two outbound Relationships.

Denied:
- every Production `HistoricalTruth`, `UnresolvedProposition`, and `WorldState` record;
- all Voss/Wren subjective records and Relationships;
- chronology.

In particular, Wren's return Observation and suspicion are not accessible to Marlowe.

### VOSS

Permitted:
- shared SceneState and Pressure;
- Voss Constitution/Disposition;
- Voss current-strengthened Knowledge;
- Voss accidental-loss belief;
- Voss RH-001/RH-002 memories;
- Voss Goal;
- Voss's two outbound Relationships.

Denied:
- all Production truth/state categories excluded by the matrix, including `WORLD-CURRENT-STRENGTHENED` itself;
- Marlowe's hidden-release Knowledge and all other Marlowe/Wren private records;
- chronology.

### WREN

Permitted:
- shared SceneState and Pressure;
- Wren Constitution/Disposition;
- Wren return Observation;
- Wren secured-raft Knowledge;
- Wren suspicion;
- Wren RH-002 memory;
- Wren Goal;
- Wren's two outbound Relationships.

Denied:
- all excluded Production categories, including the hidden release truth;
- all Marlowe/Voss private records;
- chronology.

In particular, Wren receives suspicion, not release Knowledge.

Every Character sees the three roster identities (`MARLOWE`, `VOSS`, `WREN`) and exact display names because all three are co-present.

## 15. Required tests

Use existing canonical fixture sources and mutation/in-memory assertions. Do not create alternate Access-Control-specific fixture copies.

Required coverage:

1. generic smoke fixture produces a valid bounded projection for a roster Character;
2. all roster identities and display names are visible, with no other Character state attached;
3. subject-owned records across every Character category are permitted;
4. subject-owned directional relationships are permitted;
5. inbound/other-owner relationships are denied;
6. all other Character-owned private records are denied;
7. `SceneState` is shared with every roster Character;
8. root `Pressure` is shared with every roster Character;
9. `HistoricalTruth`, `UnresolvedProposition`, and `WorldState` are denied to every Character;
10. chronology and its IDs never enter the Character projection;
11. permitted records contain ID/text only and cannot expose provenance;
12. an allowed record whose provenance cites a denied Production record does not cause the source record or source ID to enter the projection;
13. Missing Raft Voss receives `KNOW-VOSS-CURRENT-STRENGTHENED` but not `WORLD-CURRENT-STRENGTHENED`;
14. Missing Raft Wren receives `KNOW-WREN-SECURED-RAFT` but not `HT-WREN-SECURED-RAFT`;
15. Missing Raft SceneState disclosure does not leak its HistoricalTruth provenance source;
16. Marlowe receives his subjective release Knowledge but not the Production HistoricalTruth record through the access projection;
17. Voss and Wren do not receive Marlowe's hidden-release Knowledge;
18. Marlowe does not receive Wren's return Observation/suspicion;
19. all three exact Missing Raft permitted Record-ID sets match the approved matrix;
20. decisions contain every fixture record exactly once and carry the correct permit/deny reason;
21. decision order and projection collection order are deterministic under source reordering of semantically unordered arrays;
22. access evaluation does not mutate the fixture or alter the frozen Missing Raft fixture hash;
23. requesting an unknown Character fails closed;
24. generic smoke still validates/runs through the existing Harness path;
25. canonical Missing Raft still validates/runs with its frozen ECJ-1 hash;
26. all existing 73 Core tests remain green.

Tests should prefer exact set equality over substring/prose heuristics. The Access Control implementation must not parse story text.

## 16. Harness behavior

Patch 0004 does not need a new public CLI/debug command merely to expose access internals.

The current Harness fixture-validation interface remains unchanged. Native test execution is sufficient to exercise deterministic Access Control on the target ARM64 machine during this patch.

Later Context Composer/Harness integration will consume `CharacterAccessProjection` directly.

Regression runtime checks still run:

- canonical Missing Raft validation;
- generic smoke validation.

Do not print denied Record IDs or private access-decision details as normal Harness output.

## 17. E0-D omniscient ablation

The required E0-D omniscient-context comparison intentionally removes the reference architecture's character-bounded information asymmetry.

Patch 0004 must **not** implement this as:

- an `isOmniscient` flag;
- a debug bypass;
- a special Character ID;
- a second permissive rule inside `CharacterBoundedAccessControl`.

That would convert an experimental control into a latent production bypass.

Later E0-D harness work must build the omniscient control as an explicitly labeled experiment path outside the safe reference Access Control operation, with provenance clearly marking the intentional ablation. Full Ensemble/reference runs continue to fail closed on any access-boundary violation.

## 18. Security and authority properties

- Access decisions are deterministic Core authority, not model recommendations.
- Access Control does not infer narrative meaning from prose.
- Provenance never grants access.
- Hash equality never grants access.
- Being a relationship target never grants access to the source Character's relationship state.
- Being co-present grants only the fixed shared Scene metadata/SceneState/Pressure defined above, not another Character's internal state.
- Denied record text and denied provenance source IDs never enter the Character-facing projection.
- The audit-decision surface is local and must remain separate from the Context Composer/Performer surface.
- A denied item is not a technical/provider failure and must never become fictional behavior.

## 19. ARM64 and battery suitability

Access Control is a small deterministic in-memory Core operation over a fixture already bounded by the 1 MiB E0 limit.

Expected work is linear over fixture records plus small ordinal sorts for deterministic output. It performs no I/O, no background work, no AI inference, no GPU work, and no NPU work.

This is intentional. Access authority is cheaper, safer, and more testable as deterministic CPU logic than as inference. The NPU remains reserved for workloads that later demonstrate a real inference benefit.

No NPU execution or performance claim is made by Patch 0004.

## 20. Explicit exclusions

Patch 0004 does not implement:

- Context Composer;
- context token/relevance optimization;
- ContextPacketHash;
- ProductionState or StateHash;
- Director/opportunity selection;
- omniscient E0-D control execution;
- relationship-omission E0-D control execution;
- Performer/provider calls;
- Integrity Validator;
- State Interpreter or State Authority;
- causal commit/persistence;
- full observation engine;
- Audience View / Creator View presentation projection;
- final Take a Seat UX;
- provider secrets/cost controls;
- Windows AI/NPU;
- WinUI;
- packaging/WACK/Store work.

## 21. Exit gate

Before promotion:

1. this blueprint is explicitly approved as the canonical Patch 0004 implementation specification;
2. implementation begins from the then-current `main` on a dedicated implementation branch;
3. the frozen Missing Raft source and its ECJ-1 digest remain unchanged;
4. generic Access Control has no Missing-Raft-specific policy branch;
5. exact category/ownership matrix receives static/adversarial review;
6. Character-facing projection is proven not to carry provenance or denied Record IDs;
7. access-decision audit is proven separate from Character-facing projection;
8. deterministic ordering/reordering invariance tests pass;
9. exact three-Character Missing Raft access-set tests pass;
10. native Windows ARM64 Core/Harness build passes with warnings-as-errors;
11. full Core test suite passes on the target machine;
12. existing Missing Raft and generic smoke Harness runtime regressions pass;
13. final hygiene review finds no ACL framework, policy registry, omniscient bypass, provenance traversal, Context Composer leakage, or later-patch scope creep;
14. validation evidence preserves the distinction between machine-tested executable head and any later documentation-only closure commits.

## 22. Material approval decisions

Explicit approval of this blueprint freezes the following persistent implementation decisions for `ensemble.e0.character-bounded.v1`:

1. Production `HistoricalTruth`, `UnresolvedProposition`, and `WorldState` are denied to Character projections;
2. all `SceneState` and root `Pressure` are shared with the Scene roster;
3. each Character receives only their own subjective collections and outbound Relationships;
4. all roster Character IDs/display names are public structural Scene identity metadata;
5. chronology and initial-opportunity state are not part of the Access Control projection;
6. Character-facing projection records strip provenance entirely;
7. Access Control returns a maximal permitted set, leaving relevance to Context Composer;
8. the local audit records permit/deny decisions separately and is forbidden from the Performer-facing surface;
9. one concrete single-contract Access Control implementation is preferred over an ACL/policy framework;
10. E0-D omniscient context remains an explicit experimental bypass outside the safe reference Access Control operation.

Implementation must not begin until these decisions are approved.
