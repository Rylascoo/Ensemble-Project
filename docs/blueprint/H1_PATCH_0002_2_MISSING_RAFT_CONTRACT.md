# H1 Patch 0002.2 — Missing Raft Fixture Contract

Status: blueprint proposal 0.2 — adversarially revised, implementation not yet approved
Parent baseline: H1 Patch 0002.1 + 0002.1a

## Purpose
Add the canonical E0-A primary fixture, `ensemble.e0.missing-raft@0.1.0`, and prove that its frozen experimental structure is represented correctly by E0 Fixture Dialect v1 without adding story-specific rules to the generic fixture layer.

Patch 0002.2 asks one question:

> Can the approved Missing Raft experiment be encoded as one canonical fixture whose authority categories, Character ownership, chronology, provenance, relationships, epistemic asymmetry, and opening conditions are mechanically protected before hashing or Access Control exist?

## Architectural rule
Patch 0002.2 preserves the validated generic fixture layer from Patch 0002.1/0002.1a.

Pipeline:

`raw fixture -> StrictJsonPreflight -> E0FixtureDocument -> GenericE0FixtureValidator -> ValidatedFixture -> MissingRaftContract.Validate(ValidatedFixture)`

Successful Missing Raft validation does not create a second domain representation. The same immutable `ValidatedFixture` remains authoritative. `MissingRaftContract` contributes only experiment-specific validation law and stable metadata needed by later E0 controls.

The generic validator remains story-agnostic. `MissingRaftContract` may know stable Missing Raft IDs, categories, ownership, chronology, relationship targets, and required provenance edges. It must not parse English prose or duplicate narrative wording in C#.

Exact fixture prose is reviewed as canonical source content in this patch and becomes cryptographically frozen by ECJ-1 + SHA-256 in Patch 0003.

## Mechanical protection versus semantic-source review
Patch 0002.2 must not pretend structural validation can understand English meaning.

Mechanically enforced in C#:
- fixture/version/contracts;
- exact record IDs and authority collections;
- exact Character ownership;
- exact relationship owner/target pairs;
- exact chronology IDs/order;
- required provenance edges and the generic provenance DAG;
- absence of extra records in frozen collections;
- exact relationship-ablation metadata.

Reviewed in the canonical JSON source, then frozen by Patch 0003 hash:
- the exact wording and meaning of Constitution, Disposition, Goal, belief, suspicion, memory, relationship, Pressure, and truth records;
- no moral winner;
- no prescribed confession, accusation, revelation, reconciliation, or solution;
- no hidden narrative cue that changes the approved epistemic situation while retaining a valid record ID.

A structural PASS before Patch 0003 therefore means the approved semantic source is represented in the right places; it is not yet cryptographic proof that prose bytes cannot change.

## No schema expansion by default
Use E0 Fixture Dialect v1 unchanged unless implementation demonstrates an actual impossibility.

Do not add generic tags, ACLs, numeric scores, relationship meters, causal-framework types, experiment flags, or ablation metadata for convenience.

## Canonical fixture identity
- family: `ensemble.e0.missing-raft`
- version: `0.1.0`
- authoritative fixture ID: `ensemble.e0.missing-raft@0.1.0`
- schema: `ensemble.e0.fixture.v1`
- access contract: `ensemble.e0.character-bounded.v1`
- observation contract: `ensemble.e0.copresent-trio.v1`

Any semantic or structural change after freeze requires deliberate fixture-version review. Patch 0003 binds the reviewed semantic representation to a hash.

## Character IDs and topology
Stable Character IDs:
- `MARLOWE`
- `VOSS`
- `WREN`

The fixture contains exactly these three Characters. The Scene roster contains exactly these three Characters. All remain co-present for the bounded E0-A observation window. Initial opportunity is exactly `VOSS`.

Roster/topology is the authority for co-presence; do not duplicate co-presence as a redundant SceneState record.

## Exact Production-authoritative record catalog
The contract requires the following exact IDs in the following authority collections. No extra record may enter these frozen collections without a fixture-version change.

### HistoricalTruth
- `RH-001` — rising-tide/cache relationship-history event.
- `RH-002` — misplaced-tool/uncertainty-boundary relationship-history event.
- `HT-WREN-SECURED-RAFT` — Wren correctly secured the raft.
- `HT-RAFT-DETERIORATED` — real structural deterioration existed and was observed by Marlowe.
- `HT-MARLOWE-RELEASED-RAFT` — Marlowe deliberately released the correctly secured raft without group consent.
- `HT-MARLOWE-NOT-WARNED` — Marlowe had not warned the group before the release.
- `HT-CURRENT-CARRIED-RAFT-AWAY` — once unmoored, the stronger current carried the raft away.
- `HT-CURRENT-DID-NOT-RELEASE-MOORING` — the stronger current did not release the correctly secured mooring.
- `HT-MARLOWE-RETURNED-SHORELINE` — Marlowe later returned from the shoreline.

HistoricalTruth records establish what happened. Counterfactual uncertainty is not embedded as truth merely because it is discussed inside a historical episode.

### UnresolvedProposition
- `UP-RAFT-WOULD-FAIL` — whether the raft would actually have failed remains unresolved.
- `UP-RH001-CACHE-WOULD-BE-LOST` — whether the cache would have been lost without Marlowe's RH-001 action remains unresolved.

This second proposition prevents RH-001's counterfactual uncertainty from being flattened into HistoricalTruth prose.

### WorldState
- `WORLD-CURRENT-STRENGTHENED` — stronger current conditions are objectively present.

WorldState is Production-authoritative. Character access to a WorldState fact must be represented by approved Character state or later deterministic access law; provenance alone never grants access.

### SceneState
SceneState contains opening conditions intended to be common Scene context rather than hidden Production truth:
- `SCENE-RAFT-GONE`
- `SCENE-PROVISIONS-LIMITED`
- `SCENE-NO-IMMEDIATE-EMERGENCY`
- `SCENE-NO-EXTERNAL-COUNTDOWN`

`SCENE-NO-EXTERNAL-COUNTDOWN` preserves the approved absence of storm/rescue-timer pressure without inventing separate duplicated countdown concepts.

### Pressure
Exactly one opening public Pressure record:
- `PRESSURE-ISOLATION`

Its reviewed source meaning is that raft loss plus limited provisions make indefinite inaction untenable without creating an immediate emergency or prescribing what any Character must do.

Required provenance:
- `SCENE-RAFT-GONE` -> `HT-CURRENT-CARRIED-RAFT-AWAY`.
- `PRESSURE-ISOLATION` -> `SCENE-RAFT-GONE`, `SCENE-PROVISIONS-LIMITED`.
- `UP-RAFT-WOULD-FAIL` -> `HT-RAFT-DETERIORATED`.
- `UP-RH001-CACHE-WOULD-BE-LOST` -> `RH-001`.

Base authored truth/state records may have empty provenance when they are not derived from another fixture record.

## Exact Character record catalog
Character ownership is structural because records are nested under their owning Character. The contract freezes every Character collection by exact ID set.

No separate Character-specific Circumstance record is introduced in fixture 0.1.0. Immediate shared conditions belong in SceneState; approved Character-specific immediate intent belongs in Goal/belief/suspicion. This avoids duplicating the same opening state merely to populate a field. All three `circumstance` arrays are therefore exactly empty for this fixture version.

### MARLOWE
Constitution:
- `CON-MARLOWE`

Disposition:
- `DISP-MARLOWE`

Observations:
- `OBS-MARLOWE-RAFT-DETERIORATION`
- `OBS-MARLOWE-NOBODY-VISIBLE`

Knowledge:
- `KNOW-MARLOWE-RELEASED-RAFT`
- `KNOW-MARLOWE-NOT-WARNED`

Beliefs:
- `BEL-MARLOWE-DAMAGE-UNACCEPTABLE`
- `BEL-MARLOWE-GROUP-LIKELY-PROCEED`
- `BEL-MARLOWE-DISCLOSURE-RISK`

Suspicions:
- none

Memories:
- `MEM-MARLOWE-RH001`
- `MEM-MARLOWE-RH002`

Goals:
- `GOAL-MARLOWE`

Relationships:
- `REL-MARLOWE-VOSS` -> target `VOSS`
- `REL-MARLOWE-WREN` -> target `WREN`

Required provenance:
- `OBS-MARLOWE-RAFT-DETERIORATION` -> `HT-RAFT-DETERIORATED`.
- `KNOW-MARLOWE-RELEASED-RAFT` -> `HT-MARLOWE-RELEASED-RAFT`.
- `KNOW-MARLOWE-NOT-WARNED` -> `HT-MARLOWE-NOT-WARNED`.
- `BEL-MARLOWE-DAMAGE-UNACCEPTABLE` -> `OBS-MARLOWE-RAFT-DETERIORATION`.
- `MEM-MARLOWE-RH001` -> `RH-001`.
- `MEM-MARLOWE-RH002` -> `RH-002`.
- `REL-MARLOWE-VOSS` -> `RH-001`.
- `REL-MARLOWE-WREN` -> `RH-002`.

`OBS-MARLOWE-NOBODY-VISIBLE`, `BEL-MARLOWE-GROUP-LIKELY-PROCEED`, and `BEL-MARLOWE-DISCLOSURE-RISK` are approved authored subjective starting records and need not invent unsupported causal provenance.

Marlowe must not begin with any Wren-private record or knowledge that Wren observed him returning from the shoreline.

### VOSS
Constitution:
- `CON-VOSS`

Disposition:
- `DISP-VOSS`

Observations:
- none

Knowledge:
- `KNOW-VOSS-CURRENT-STRENGTHENED`

Beliefs:
- `BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE`

Suspicions:
- none

Memories:
- `MEM-VOSS-RH001`
- `MEM-VOSS-RH002`

Goals:
- `GOAL-VOSS`

Relationships:
- `REL-VOSS-MARLOWE` -> target `MARLOWE`
- `REL-VOSS-WREN` -> target `WREN`

Required provenance:
- `KNOW-VOSS-CURRENT-STRENGTHENED` -> `WORLD-CURRENT-STRENGTHENED`.
- `BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE` -> `KNOW-VOSS-CURRENT-STRENGTHENED`, `SCENE-RAFT-GONE`.
- `MEM-VOSS-RH001` -> `RH-001`.
- `MEM-VOSS-RH002` -> `RH-002`.
- `REL-VOSS-MARLOWE` -> `RH-001`.
- `REL-VOSS-WREN` -> `RH-002`.

Voss receives no Character-owned record whose content is the hidden release truth, and no Voss record may cite `HT-MARLOWE-RELEASED-RAFT`.

### WREN
Constitution:
- `CON-WREN`

Disposition:
- `DISP-WREN`

Observations:
- `OBS-WREN-MARLOWE-RETURN`

Knowledge:
- `KNOW-WREN-SECURED-RAFT`

Beliefs:
- none

Suspicions:
- `SUSP-WREN-MARLOWE-KNOWS-MORE`

Memories:
- `MEM-WREN-RH002`

Goals:
- `GOAL-WREN`

Relationships:
- `REL-WREN-MARLOWE` -> target `MARLOWE`
- `REL-WREN-VOSS` -> target `VOSS`

Required provenance:
- `KNOW-WREN-SECURED-RAFT` -> `HT-WREN-SECURED-RAFT`.
- `OBS-WREN-MARLOWE-RETURN` -> `HT-MARLOWE-RETURNED-SHORELINE`.
- `SUSP-WREN-MARLOWE-KNOWS-MORE` -> `KNOW-WREN-SECURED-RAFT`, `OBS-WREN-MARLOWE-RETURN`.
- `MEM-WREN-RH002` -> `RH-002`.
- `REL-WREN-MARLOWE` -> `RH-002`.
- `REL-WREN-VOSS` -> `RH-002`.

Wren receives no observation of the release itself, no knowledge record for the release, and no Wren record may cite `HT-MARLOWE-RELEASED-RAFT`.

## Canonical Character source meanings
The JSON prose must preserve the approved meanings below. These are source-review invariants in 0002.2 and become hash-bound in Patch 0003.

Marlowe:
- Constitution: anticipates harm/responsibility; may act protectively without agreement; irreversible harm can outweigh consensus.
- Disposition: when delay seems dangerous, may act before fully explaining; explanation becomes harder afterward.
- Goal: prevent rushing into a dangerous underexamined decision while avoiding unnecessary trust destruction.
- Beliefs: deterioration is unacceptable risk; group likely to proceed; disclosure may damage later decision-making.

Voss:
- Constitution: consequential decisions should withstand explicit examination; seeks reasons under uncertainty.
- Disposition: distinguishes observation, inference, and assumption; may clarify at length.
- Goal: establish enough reliable shared information for the next consequential decision to be open and accountable.
- Belief: accidental raft loss is plausible but not certain.

Wren:
- Constitution: autonomy and firsthand observation; resists overstating certainty.
- Disposition: under pressure, narrows speech and may withhold inference.
- Goal: avoid claiming more than is known while deciding whether speaking now clarifies matters or unfairly accuses.
- Suspicion: Marlowe may know more than he has said; this remains suspicion, not knowledge.

## Relationship history source meanings
`RH-001` source meaning:
- rising tide threatened a cache;
- Marlowe moved supplies before agreement;
- no harm resulted;
- Voss objected to the act-first/explain-later pattern.

The counterfactual question of whether loss would otherwise have occurred is represented separately as `UP-RH001-CACHE-WOULD-BE-LOST`.

`RH-002` source meaning:
- a tool was misplaced;
- Wren limited her statement to what she observed;
- Voss pressed toward a firmer conclusion;
- Wren refused to overstate;
- Marlowe supported that boundary.

## Directional relationship source meanings
Exactly six directional relationship records are required:
- `REL-VOSS-MARLOWE`: respects Marlowe's perception, grants less benefit of doubt on unilateral choices.
- `REL-MARLOWE-VOSS`: respects competence, expects pressure for explicit justification.
- `REL-MARLOWE-WREN`: expects/respects refusal to overstate uncertainty.
- `REL-WREN-MARLOWE`: expects respect for uncertainty because Marlowe supported that boundary before.
- `REL-WREN-VOSS`: respects competence, expects pressure toward a firm conclusion.
- `REL-VOSS-WREN`: respects observational care, expects withholding when pressed.

Unsupported protective/paternal framing of Marlowe toward Wren is excluded from the reviewed source content.

## Relationship-derived memories and E0-D ablation
The canonical fixture uses exactly five relationship-history memories:
- `MEM-MARLOWE-RH001`
- `MEM-MARLOWE-RH002`
- `MEM-VOSS-RH001`
- `MEM-VOSS-RH002`
- `MEM-WREN-RH002`

No Wren RH-001 memory is invented because the approved RH-001 meaning does not require one for her.

Define three experiment-specific immutable ID sets in `MissingRaftContract`:
- `RelationshipHistoryRecordIds` = `RH-001`, `RH-002`.
- `RelationshipRecordIds` = all six `REL-*` IDs.
- `RelationshipDerivedMemoryRecordIds` = the five `MEM-*` IDs above.

`RelationshipContextRecordIds` is the derived union of those three sets, not a second manually maintained list.

E0-D relationship omission later means **exclude this relationship-context set from otherwise matched Character context construction**. It does not delete records from the fixture, alter Production truth, change fixture version, or rewrite provenance. This preserves the frozen-fixture requirement while changing only the intended context variable.

## Chronology
Chronology is the single semantically ordered collection, but it need not repeat every HistoricalTruth record. It contains only past events whose ordering is experimentally relevant.

Exact chronology for fixture 0.1.0:
1. `RH-001`
2. `RH-002`
3. `HT-WREN-SECURED-RAFT`
4. `HT-RAFT-DETERIORATED`
5. `HT-MARLOWE-RELEASED-RAFT`
6. `HT-CURRENT-CARRIED-RAFT-AWAY`

`HT-MARLOWE-NOT-WARNED`, `HT-CURRENT-DID-NOT-RELEASE-MOORING`, and `HT-MARLOWE-RETURNED-SHORELINE` are authoritative facts but do not need extra total-order semantics in chronology. The reviewed source wording preserves that Marlowe returned later; Wren observed the return, not the release.

This avoids inventing an unnecessary exact ordering between every background fact while still protecting the causally important sequence: relationship history -> securing/deterioration -> release -> raft carried away.

## Provenance is not access
This rule is explicit for all later H1 work:

> A provenance edge explains evidence/derivation. It never grants Character access to the provenance source.

Therefore:
- Access Control must never recursively traverse provenance and add source records to a Character's permitted set.
- Context Composer must never receive prohibited Production records merely because an allowed Character record cites them.
- A context packet may include an allowed record's meaning without disclosing inaccessible provenance source content/IDs unless that provenance is independently permitted.
- Missing Raft contract validation may inspect complete provenance because it runs inside authoritative local Core before Character projection.

This preserves useful causal auditability without turning provenance into a secret-leak channel.

## Contract-validation strategy
`MissingRaftContract.Validate` fails closed unless:
- fixture identity/version/contracts are exact;
- Character IDs and Scene roster are exact;
- initial opportunity is Voss;
- every Production authority collection matches its exact frozen record-ID set;
- every Character collection matches its exact frozen owner-specific record-ID set;
- all three Circumstance collections are empty;
- chronology exactly matches the frozen six-ID ordered list;
- each relationship has exact owner, target, and provenance;
- required provenance edges match exactly where frozen above;
- Voss/Wren records do not cite hidden release truth;
- `RelationshipContextRecordIds` equals the derived union of the three frozen relationship-context component sets;
- the generic provenance DAG invariant remains satisfied.

Use one source of truth for each stable ID inside `MissingRaftContract`; tests and ablation metadata must reference those constants/sets rather than retyping string literals throughout implementation.

## Tests
Use one canonical Missing Raft JSON fixture. Negative tests clone/mutate it one invariant at a time.

Required tests:
- canonical Missing Raft fixture passes generic + Missing Raft validation;
- wrong fixture family/version fails when the Missing Raft contract is invoked;
- wrong Character set fails;
- wrong initial opportunity fails;
- Production category move/add/remove fails;
- Character owner/category move/add/remove fails;
- chronology reorder/removal/addition fails;
- `UP-RAFT-WOULD-FAIL` moved to HistoricalTruth fails;
- `UP-RH001-CACHE-WOULD-BE-LOST` moved to HistoricalTruth fails;
- Wren observation provenance changed from return to release fails;
- Wren suspicion given privileged release provenance fails;
- Voss belief given privileged release provenance fails;
- Marlowe release knowledge removed/reassigned fails;
- Marlowe not-warned knowledge removed/reassigned fails;
- relationship owner/target mismatch fails;
- RH provenance removed/changed fails;
- relationship-derived memory provenance changed fails;
- derived relationship-context union remains exact;
- generic provenance-cycle regression remains green;
- generic smoke fixture remains green and is rejected when the Missing Raft contract is explicitly invoked.

Do not create a directory of nearly identical malformed fixture files.

## Harness behavior
The existing Harness remains generic by default.

For runtime validation, use the smallest explicit family dispatch:
- after generic validation, if `fixture.FamilyId` is the known Missing Raft family, invoke `MissingRaftContract.Validate(fixture)`;
- if the family matches but version/contract law is wrong, fail closed;
- unknown generic E0 fixture families remain valid generic fixtures;
- never infer contract type from display prose.

Do not introduce a registry/plugin/factory framework for one experiment contract.

## Explicit exclusions
Patch 0002.2 does not add:
- ECJ-1 canonical serialization;
- SHA-256 fixture identity/hash enforcement;
- deterministic Access Control implementation;
- Context Composer;
- ProductionState construction;
- persistence / causal commits;
- Performer / Director / Validator / Interpreter runtime orchestration;
- providers or AI;
- WinUI / Windows AI / NPU / packaging / Store work.

## Exit gate
Before merge, on the target Windows ARM64 machine:
1. Harness/Core build passes with warnings-as-errors.
2. Entire Core test suite passes.
3. Harness validates canonical Missing Raft as `ensemble.e0.missing-raft@0.1.0`.
4. Generic smoke fixture remains valid through the generic path.
5. A direct Missing Raft-contract call rejects the generic smoke fixture.
6. Final baseline comparison finds no duplicate schema, second fixture domain representation, legacy compatibility layer, prose parser, generic over-abstraction, provenance-access leak, or scope leakage.

Only after this gate passes may Patch 0003 freeze the exact semantic fixture representation with ECJ-1 + SHA-256.
