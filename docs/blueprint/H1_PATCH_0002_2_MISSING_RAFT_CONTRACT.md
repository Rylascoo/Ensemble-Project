# H1 Patch 0002.2 — Missing Raft Fixture Contract

Status: blueprint proposal 0.3 — adversarially revised, implementation not yet approved
Parent baseline: H1 Patch 0002.1 + 0002.1a

## 1. Purpose
Add the canonical E0-A primary fixture, `ensemble.e0.missing-raft@0.1.0`, and prove that its approved experimental structure is represented correctly by E0 Fixture Dialect v1 before hashing or Access Control implementation.

Question:

> Can the approved Missing Raft experiment be encoded as one canonical fixture whose authority categories, Character ownership, chronology, provenance, relationships, epistemic asymmetry, and opening conditions are mechanically protected without contaminating the generic fixture layer with story-specific machinery?

## 2. Architecture boundary
Preserve Patch 0002.1/0002.1a unchanged unless implementation proves an actual impossibility.

Pipeline:

`raw fixture -> StrictJsonPreflight -> E0FixtureDocument -> GenericE0FixtureValidator -> ValidatedFixture -> MissingRaftContract.Validate(ValidatedFixture)`

`MissingRaftContract.Validate` does not create another fixture/domain representation. The same immutable `ValidatedFixture` remains authoritative. Missing Raft contributes only experiment-specific validation law and stable experiment metadata.

Do not add a `MissingRaftFixture` wrapper, schema fork, registry/plugin framework, generic tags, fixture ACLs, numeric scores, relationship meters, generic causal framework, or generic ablation metadata.

## 3. Mechanical law versus semantic-source review
Patch 0002.2 must not pretend C# can infer narrative meaning from prose.

Mechanically enforce:
- exact fixture family/version/contracts;
- exact Scene ID, Character IDs, display names, roster, and initial opportunity;
- exact record IDs in exact authority/owner collections;
- exact relationship owner/target pairs;
- exact chronology IDs/order;
- exact required provenance where frozen below;
- generic provenance DAG;
- no extra records in frozen collections;
- exact relationship-context ablation metadata.

Review in the canonical JSON source, then hash-bind in Patch 0003:
- exact Constitution/Disposition/Goal/belief/suspicion/memory/relationship/Pressure/truth wording;
- no moral winner;
- no prescribed confession, accusation, revelation, reconciliation, or solution;
- no hidden textual cue that changes the approved epistemic situation while retaining a structurally valid ID.

A 0002.2 structural PASS proves placement/ownership/graph correctness. It is not yet cryptographic proof against prose mutation; Patch 0003 provides that proof.

## 4. Canonical source and identity
Exactly one canonical Missing Raft source file:

`fixtures/missing-raft/missing-raft-0.1.0.json`

Tests and Harness runtime validation use this file directly or copy/link this same source into test output. Do not maintain a second valid Missing Raft fixture copy.

Identity:
- schema: `ensemble.e0.fixture.v1`
- family: `ensemble.e0.missing-raft`
- version: `0.1.0`
- authoritative FixtureId: `ensemble.e0.missing-raft@0.1.0`
- access contract: `ensemble.e0.character-bounded.v1`
- observation contract: `ensemble.e0.copresent-trio.v1`

After this fixture version is frozen, a semantic or structural change requires deliberate fixture-version review. Patch 0003 then binds the reviewed semantic representation to SHA-256.

## 5. Scene and Character identity
Scene ID:
- `SCENE-MISSING-RAFT`

Characters and exact display names:
- `MARLOWE` -> `Marlowe`
- `VOSS` -> `Dr. Voss`
- `WREN` -> `Wren`

The fixture contains exactly these Characters. Scene roster contains exactly this set. Roster order remains semantically unordered under E0 Fixture Dialect v1. All three remain co-present for the bounded E0-A observation window. Initial opportunity is exactly `VOSS`.

Roster/topology is the authority for co-presence; do not duplicate co-presence as a SceneState record.

## 6. Exact Production-authoritative catalog
No extra record may enter these frozen collections without a fixture-version change.

### HistoricalTruth
- `RH-001` — rising-tide/cache relationship-history event.
- `RH-002` — misplaced-tool/uncertainty-boundary relationship-history event.
- `HT-WREN-SECURED-RAFT` — Wren correctly secured the raft.
- `HT-RAFT-DETERIORATED` — real structural deterioration existed.
- `HT-MARLOWE-RELEASED-RAFT` — Marlowe deliberately released the correctly secured raft without group consent.
- `HT-MARLOWE-NOT-WARNED` — Marlowe had not warned the group before release.
- `HT-CURRENT-CARRIED-RAFT-AWAY` — once unmoored, the stronger current carried the raft away.
- `HT-CURRENT-DID-NOT-RELEASE-MOORING` — the stronger current did not release the correctly secured mooring.
- `HT-MARLOWE-RETURNED-SHORELINE` — Marlowe later returned from the shoreline.

`HT-RAFT-DETERIORATED` establishes objective condition only. Marlowe's perception of it remains separately authoritative as his Character-owned Observation; do not duplicate the observation inside HistoricalTruth.

### UnresolvedProposition
- `UP-RAFT-WOULD-FAIL` — whether the raft would actually have failed remains unresolved.
- `UP-RH001-CACHE-WOULD-BE-LOST` — whether the cache would have been lost without Marlowe's RH-001 action remains unresolved.

The RH-001 counterfactual is intentionally separate from `RH-001`; uncertainty must not be flattened into historical fact.

### WorldState
- `WORLD-CURRENT-STRENGTHENED` — stronger current conditions are objectively present.

WorldState remains Production-authoritative. Voss's approved awareness is separately represented by Character-owned Knowledge.

### SceneState
- `SCENE-RAFT-GONE`
- `SCENE-PROVISIONS-LIMITED`
- `SCENE-NO-IMMEDIATE-EMERGENCY`
- `SCENE-NO-EXTERNAL-COUNTDOWN`

These records are structurally classified as opening SceneState because the approved experiment treats them as common Scene conditions rather than private Character information. **Patch 0002.2 does not itself implement or prove their future disclosure.** Patch 0004 Access Control must explicitly preserve that intent; no current claim of context delivery is made here.

`SCENE-NO-EXTERNAL-COUNTDOWN` preserves the absence of a storm/rescue timer without creating separate duplicate countdown concepts.

### Pressure
Exactly one opening Pressure:
- `PRESSURE-ISOLATION`

Reviewed meaning: raft loss plus limited provisions make indefinite inaction untenable without creating an immediate emergency or prescribing a Character response.

### Frozen Production provenance
- `SCENE-RAFT-GONE` -> `HT-CURRENT-CARRIED-RAFT-AWAY`.
- `PRESSURE-ISOLATION` -> `SCENE-RAFT-GONE`, `SCENE-PROVISIONS-LIMITED`.
- `UP-RAFT-WOULD-FAIL` -> `HT-RAFT-DETERIORATED`.
- `UP-RH001-CACHE-WOULD-BE-LOST` -> `RH-001`.

Base authored truth/state records may have empty provenance when they are not derived from another fixture record.

## 7. Exact Character catalog
Ownership is structural because Character records are nested under their owner. Every collection below is frozen by exact ID set. All three `circumstance` arrays are exactly empty in fixture 0.1.0; immediate shared conditions belong in SceneState and approved Character-specific intent belongs in Goal/belief/suspicion. Do not duplicate those facts solely to populate Circumstance.

### MARLOWE
Constitution: `CON-MARLOWE`

Disposition: `DISP-MARLOWE`

Circumstance: none

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

Suspicions: none

Memories:
- `MEM-MARLOWE-RH001`
- `MEM-MARLOWE-RH002`

Goal: `GOAL-MARLOWE`

Relationships:
- `REL-MARLOWE-VOSS` -> `VOSS`
- `REL-MARLOWE-WREN` -> `WREN`

Frozen provenance:
- `OBS-MARLOWE-RAFT-DETERIORATION` -> `HT-RAFT-DETERIORATED`.
- `KNOW-MARLOWE-RELEASED-RAFT` -> `HT-MARLOWE-RELEASED-RAFT`.
- `KNOW-MARLOWE-NOT-WARNED` -> `HT-MARLOWE-NOT-WARNED`.
- `BEL-MARLOWE-DAMAGE-UNACCEPTABLE` -> `OBS-MARLOWE-RAFT-DETERIORATION`.
- `MEM-MARLOWE-RH001` -> `RH-001`.
- `MEM-MARLOWE-RH002` -> `RH-002`.
- `REL-MARLOWE-VOSS` -> `RH-001`.
- `REL-MARLOWE-WREN` -> `RH-002`.

`OBS-MARLOWE-NOBODY-VISIBLE`, `BEL-MARLOWE-GROUP-LIKELY-PROCEED`, and `BEL-MARLOWE-DISCLOSURE-RISK` are authored starting state and remain provenance-empty rather than inventing unsupported causes.

Marlowe begins without Wren-private records or knowledge that Wren observed his return.

### VOSS
Constitution: `CON-VOSS`

Disposition: `DISP-VOSS`

Circumstance: none

Observations: none

Knowledge: `KNOW-VOSS-CURRENT-STRENGTHENED`

Beliefs: `BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE`

Suspicions: none

Memories:
- `MEM-VOSS-RH001`
- `MEM-VOSS-RH002`

Goal: `GOAL-VOSS`

Relationships:
- `REL-VOSS-MARLOWE` -> `MARLOWE`
- `REL-VOSS-WREN` -> `WREN`

Frozen provenance:
- `KNOW-VOSS-CURRENT-STRENGTHENED` -> `WORLD-CURRENT-STRENGTHENED`.
- `BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE` -> `KNOW-VOSS-CURRENT-STRENGTHENED`, `SCENE-RAFT-GONE`.
- `MEM-VOSS-RH001` -> `RH-001`.
- `MEM-VOSS-RH002` -> `RH-002`.
- `REL-VOSS-MARLOWE` -> `RH-001`.
- `REL-VOSS-WREN` -> `RH-002`.

Voss receives no Character-owned hidden-release record. No Voss record may cite `HT-MARLOWE-RELEASED-RAFT`.

### WREN
Constitution: `CON-WREN`

Disposition: `DISP-WREN`

Circumstance: none

Observations: `OBS-WREN-MARLOWE-RETURN`

Knowledge: `KNOW-WREN-SECURED-RAFT`

Beliefs: none

Suspicions: `SUSP-WREN-MARLOWE-KNOWS-MORE`

Memories: `MEM-WREN-RH002`

Goal: `GOAL-WREN`

Relationships:
- `REL-WREN-MARLOWE` -> `MARLOWE`
- `REL-WREN-VOSS` -> `VOSS`

Frozen provenance:
- `KNOW-WREN-SECURED-RAFT` -> `HT-WREN-SECURED-RAFT`.
- `OBS-WREN-MARLOWE-RETURN` -> `HT-MARLOWE-RETURNED-SHORELINE`.
- `SUSP-WREN-MARLOWE-KNOWS-MORE` -> `KNOW-WREN-SECURED-RAFT`, `OBS-WREN-MARLOWE-RETURN`.
- `MEM-WREN-RH002` -> `RH-002`.
- `REL-WREN-MARLOWE` -> `RH-002`.
- `REL-WREN-VOSS` -> `RH-002`.

Wren has no observation of the release, no knowledge record for the release, and no Wren record may cite `HT-MARLOWE-RELEASED-RAFT`.

## 8. Canonical semantic meanings
These meanings govern canonical JSON authoring in 0002.2 and become cryptographically bound in Patch 0003.

Marlowe:
- Constitution: anticipates harm/responsibility; may act protectively without agreement; irreversible harm can outweigh consensus.
- Disposition: when delay seems dangerous, may act before fully explaining; explanation becomes harder afterward.
- Goal: prevent rushing into a dangerous underexamined decision while avoiding unnecessary trust destruction.
- Beliefs: deterioration is unacceptable risk; group likely to proceed; disclosure may damage later decision-making.
- Observation boundary: he saw nobody visibly nearby during the act but does not know whether he was observed.

Voss:
- Constitution: consequential decisions should withstand explicit examination; seeks reasons under uncertainty.
- Disposition: distinguishes observation, inference, and assumption; may clarify at length.
- Goal: establish enough reliable shared information for the next consequential decision to be open and accountable.
- Belief: accidental raft loss is plausible but not certain.

Wren:
- Constitution: autonomy/firsthand observation; resists overstating certainty.
- Disposition: under pressure, narrows speech and may withhold inference.
- Goal: avoid claiming more than is known while deciding whether speaking now clarifies matters or unfairly accuses.
- Suspicion: Marlowe may know more than he has said; this is not knowledge.

`RH-001`:
- rising tide threatened a cache;
- Marlowe moved supplies before agreement;
- no harm resulted;
- Voss objected to the act-first/explain-later pattern.

Whether loss would otherwise have occurred is `UP-RH001-CACHE-WOULD-BE-LOST`, not part of HistoricalTruth.

`RH-002`:
- a tool was misplaced;
- Wren limited her statement to observation;
- Voss pressed toward a firmer conclusion;
- Wren refused to overstate;
- Marlowe supported that boundary.

Directional relationships:
- `REL-VOSS-MARLOWE`: respects perception, grants less benefit of doubt on unilateral choices.
- `REL-MARLOWE-VOSS`: respects competence, expects explicit-justification pressure.
- `REL-MARLOWE-WREN`: expects/respects refusal to overstate uncertainty.
- `REL-WREN-MARLOWE`: expects respect for uncertainty because Marlowe supported that boundary before.
- `REL-WREN-VOSS`: respects competence, expects pressure toward a firm conclusion.
- `REL-VOSS-WREN`: respects observational care, expects withholding when pressed.

Unsupported protective/paternal framing of Marlowe toward Wren is excluded.

## 9. Relationship memories and E0-D surface
Exactly five relationship-history memories:
- `MEM-MARLOWE-RH001`
- `MEM-MARLOWE-RH002`
- `MEM-VOSS-RH001`
- `MEM-VOSS-RH002`
- `MEM-WREN-RH002`

No Wren RH-001 memory is invented because the approved RH-001 meaning does not require one for her.

Define in `MissingRaftContract`:
- `RelationshipHistoryRecordIds` = `RH-001`, `RH-002`.
- `RelationshipRecordIds` = all six `REL-*` IDs.
- `RelationshipDerivedMemoryRecordIds` = the five `MEM-*` IDs above.
- `RelationshipContextRecordIds` = derived union of the three component sets; never maintain it as a second manual list.

For the later E0-D relationship-omitted control, this set is an **exclusion surface for otherwise matched context construction**. It never deletes fixture records, changes Production truth, alters fixture version, or rewrites provenance. Thus the E0-D variant changes context only while retaining the same frozen fixture.

## 10. Chronology
Chronology is semantically ordered but need not contain every HistoricalTruth record. It contains only past events whose relative ordering matters to the experiment.

Exact 0.1.0 chronology:
1. `RH-001`
2. `RH-002`
3. `HT-WREN-SECURED-RAFT`
4. `HT-RAFT-DETERIORATED`
5. `HT-MARLOWE-RELEASED-RAFT`
6. `HT-CURRENT-CARRIED-RAFT-AWAY`

`HT-MARLOWE-NOT-WARNED`, `HT-CURRENT-DID-NOT-RELEASE-MOORING`, and `HT-MARLOWE-RETURNED-SHORELINE` remain authoritative facts but do not receive unnecessary total-order semantics. The reviewed source preserves only the approved temporal meaning needed for the return: Wren observes Marlowe returning later, not the release itself.

## 11. Provenance is not access
Constitutional implementation rule for later H1 work:

> Provenance explains evidence/derivation. A provenance edge never grants Character access to its source.

Therefore:
- Access Control must never recursively traverse provenance to enlarge a permitted Character set.
- Context Composer must never receive a prohibited Production record merely because an allowed record cites it.
- An allowed record may enter context without exposing inaccessible provenance source content or source IDs unless those sources are independently permitted.
- Missing Raft validation may inspect complete provenance because it runs locally inside authoritative Core before Character projection.

This preserves causal auditability without turning provenance into a secret-leak channel.

## 12. Contract implementation shape
One `MissingRaftContract` class owns all stable Missing Raft IDs and immutable expected sets. Do not repeat string literals across validator/tests/ablation metadata.

`MissingRaftContract.Validate(ValidatedFixture fixture)` fails closed unless:
- identity/version/contracts are exact;
- Scene ID, display names, Character set, roster set, and initial opportunity are exact;
- every Production collection matches its exact frozen ID set;
- every Character collection matches its exact frozen owner-specific ID set;
- all Circumstance collections are empty;
- chronology matches the six-ID sequence exactly;
- every relationship owner/target and frozen provenance edge is exact;
- Voss/Wren records do not cite hidden release truth;
- `RelationshipContextRecordIds` equals its derived component-set union;
- generic provenance DAG remains valid.

The class may use small private helpers for exact-set/sequence checks. Do not create a generic contract-validation framework for one fixture.

## 13. Harness behavior
Harness stays generic by default.

Minimal explicit dispatch after generic validation:
- if `fixture.FamilyId` equals the known Missing Raft family, invoke `MissingRaftContract.Validate(fixture)`;
- matching family with wrong version/contract law fails closed;
- unknown generic E0 fixture families remain valid generic fixtures;
- never infer contract type from display prose.

No registry/plugin/factory abstraction is earned here.

## 14. Tests
Use the single canonical Missing Raft source. Negative tests clone/mutate it one invariant at a time.

Required coverage:
- canonical fixture passes generic + Missing Raft validation;
- generic smoke fixture remains valid generically and fails when Missing Raft contract is explicitly invoked;
- wrong fixture family/version fails under Missing Raft validation;
- wrong Scene ID/display name/Character set/initial opportunity fails;
- Production category move/add/remove fails;
- Character owner/category move/add/remove fails;
- chronology reorder/removal/addition fails;
- either unresolved proposition moved to HistoricalTruth fails;
- Wren observation provenance changed from return to release fails;
- Wren suspicion given hidden-release provenance fails;
- Voss belief given hidden-release provenance fails;
- Marlowe release/not-warned knowledge removed/reassigned fails;
- relationship owner/target mismatch fails;
- RH relationship provenance removed/changed fails;
- relationship-memory provenance changed fails;
- relationship-context derived union remains exact;
- generic provenance-cycle regression remains green.

Do not create a directory of nearly identical malformed fixtures.

## 15. Source-content review gate
Before machine validation, perform one explicit human/source review of `fixtures/missing-raft/missing-raft-0.1.0.json` against Section 8 and the approved E0-A fixture:
- no omitted approved fact/state;
- no extra dramatic premise;
- no moral winner;
- no forced outcome;
- no epistemic leak hidden in prose;
- no relationship wording drift;
- no accidental claim that raft failure was certain;
- no accidental claim that current released a correctly secured mooring.

Only a source-reviewed fixture proceeds to ARM64 validation. Patch 0003 then replaces this review-only immutability boundary with semantic canonical bytes + SHA-256.

## 16. Explicit exclusions
Patch 0002.2 does not add:
- ECJ-1 canonical serialization or SHA-256 enforcement;
- deterministic Access Control implementation;
- Context Composer;
- ProductionState construction;
- persistence/causal commits;
- Performer/Director/Validator/Interpreter runtime orchestration;
- providers/AI;
- WinUI, Windows AI, NPU, packaging, WACK, or Store work.

## 17. Exit gate
Before merge:
1. source-content review passes;
2. Harness/Core build passes with warnings-as-errors on target Windows ARM64;
3. entire Core test suite passes;
4. Harness validates `fixtures/missing-raft/missing-raft-0.1.0.json` as `ensemble.e0.missing-raft@0.1.0`;
5. generic smoke fixture remains valid through generic path;
6. direct Missing Raft validation rejects generic smoke fixture;
7. final baseline comparison finds no duplicate schema, second fixture representation, legacy compatibility layer, prose parser, generic over-abstraction, provenance-access leak, or scope leakage.

Only after this gate passes may Patch 0003 freeze the exact semantic fixture representation with ECJ-1 + SHA-256.
