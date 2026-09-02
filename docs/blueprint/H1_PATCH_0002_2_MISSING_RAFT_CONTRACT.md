# H1 Patch 0002.2 — Missing Raft Fixture Contract

Status: blueprint proposal — implementation not yet approved
Parent baseline: H1 Patch 0002.1 + 0002.1a

## Purpose
Add the canonical E0-A primary fixture, `ensemble.e0.missing-raft@0.1.0`, and prove that its frozen experimental structure is represented correctly by E0 Fixture Dialect v1 without adding story-specific rules to the generic fixture layer.

Patch 0002.2 asks one question:

> Can the approved Missing Raft experiment be encoded as one canonical fixture whose authority categories, Character ownership, chronology, provenance, relationships, epistemic asymmetry, and opening conditions are mechanically protected before hashing or Access Control exist?

## Architectural rule
Patch 0002.2 must preserve the validated generic fixture layer from Patch 0002.1/0002.1a.

Pipeline:

`raw fixture -> StrictJsonPreflight -> E0FixtureDocument -> GenericE0FixtureValidator -> ValidatedFixture -> MissingRaftContractValidator -> MissingRaftFixture`

The generic validator remains story-agnostic. `MissingRaftContractValidator` may know stable Missing Raft IDs, categories, ownership, chronology, targets, and required provenance edges. It must not parse English prose or duplicate narrative wording in C#.

Exact fixture prose is reviewed as canonical source content in this patch and becomes cryptographically frozen by ECJ-1 + SHA-256 in Patch 0003.

## No schema expansion by default
Patch 0002.2 should use the existing E0 Fixture Dialect v1 unchanged unless implementation proves an actual impossibility.

In particular, do not add generic tags, ACLs, numeric scores, relationship meters, causal-framework types, or ablation metadata merely for convenience.

E0-D relationship-context ablation can later use the Missing Raft contract's stable record-ID set and provenance closure rather than introducing fixture-level compatibility machinery now.

## Canonical fixture identity
- family: `ensemble.e0.missing-raft`
- version: `0.1.0`
- authoritative fixture ID: `ensemble.e0.missing-raft@0.1.0`
- schema: `ensemble.e0.fixture.v1`
- access contract: `ensemble.e0.character-bounded.v1`
- observation contract: `ensemble.e0.copresent-trio.v1`

Any content or structural change after freeze requires deliberate fixture-version review; Patch 0003 will bind the exact semantic representation to a hash.

## Character IDs and topology
Stable Character IDs:
- `MARLOWE`
- `VOSS`
- `WREN`

The fixture contains exactly these three Characters.
The Scene roster contains exactly these three Characters.
All remain co-present for the bounded E0-A observation window.
Initial opportunity is exactly `VOSS`.

## Production-authoritative structure
The canonical fixture must represent, without collapsing categories:

### Historical Truth
Stable records establish at minimum:
- Wren correctly secured the raft.
- The raft has real structural deterioration observed by Marlowe.
- Marlowe deliberately released the correctly secured raft without group consent.
- Marlowe had not warned the group before releasing it.
- A stronger current exists / strengthened.
- Once unmoored, the stronger current carried the raft away.
- The stronger current did not release the correctly secured mooring.
- Marlowe later returned from the shoreline.
- Relationship-history event RH-001 occurred.
- Relationship-history event RH-002 occurred.

### Unresolved Proposition
At minimum:
- whether the raft would in fact have failed remains unresolved.

No record may promote that proposition to Historical Truth.

### Current World / Scene state
The opening state must preserve:
- the raft is gone;
- stronger current conditions are present;
- provisions are limited enough that indefinite isolation is untenable;
- there is no immediate emergency;
- there is no storm countdown;
- there is no rescue timer;
- the three Characters are co-present.

### Pressure
Public Pressure must create consequential need without prescribing outcome. It may establish that the lost raft and limited provisions make continued inaction costly, but it must not prescribe confession, accusation, revelation, reconciliation, solution, or a moral winner.

## Character state contract

### MARLOWE
Constitution preserves the approved core:
- anticipates harm and responsibility;
- may act protectively without agreement;
- irreversible harm can outweigh consensus.

Disposition preserves:
- when delay seems dangerous, may act before fully explaining;
- explanation becomes harder after unilateral action.

Goal preserves:
- prevent the group from rushing into a dangerous underexamined decision while avoiding unnecessary destruction of trust.

Required private/subjective structure includes:
- observation/knowledge of real raft deterioration;
- knowledge of his own deliberate release;
- observation that he saw nobody visibly nearby at the act, without knowledge that nobody observed him;
- belief that the damage represented unacceptable risk;
- belief that the group was likely to proceed;
- belief that disclosure may damage later decision-making.

Marlowe must not begin with knowledge that Wren saw him returning from the shoreline.

### VOSS
Constitution preserves:
- consequential decisions should withstand explicit examination;
- seeks reasons under uncertainty.

Disposition preserves:
- distinguishes observation, inference, and assumption;
- may clarify at length.

Goal preserves:
- establish enough reliable shared information for the next consequential decision to be open and accountable.

Required private/subjective structure includes:
- knowledge that the current strengthened;
- belief that accidental raft loss is plausible but not certain.

Voss does not receive Production truth that Marlowe released the raft.

### WREN
Constitution preserves:
- autonomy and firsthand observation;
- resists overstating certainty.

Disposition preserves:
- under pressure, narrows speech and may withhold inference.

Goal preserves:
- avoid claiming more than is known while deciding whether speaking now clarifies matters or unfairly accuses.

Required private/subjective structure includes:
- knowledge/observation that Wren correctly secured the raft;
- observation of Marlowe returning from the shoreline later;
- no observation of the release itself;
- suspicion that Marlowe may know more than he has said;
- suspicion must remain distinct from knowledge.

Wren does not receive Production truth that Marlowe released the raft.

## Relationship history

### RH-001
Frozen meaning:
- rising tide threatened a cache;
- Marlowe moved supplies before agreement;
- no harm resulted;
- whether loss would otherwise have occurred remained unresolved;
- Voss objected to the act-first/explain-later pattern.

### RH-002
Frozen meaning:
- a tool was misplaced;
- Wren limited her statement to what she observed;
- Voss pressed toward a firmer conclusion;
- Wren refused to overstate;
- Marlowe supported that boundary.

RH-001 and RH-002 are Production-authoritative history records. Character memories and directional relationships may cite them through provenance; they are not duplicated as alternate truth systems.

## Directional relationships
Exactly six directional relationship records are required:
- Voss -> Marlowe: respects Marlowe's perception, grants less benefit of doubt on unilateral choices.
- Marlowe -> Voss: respects competence, expects pressure for explicit justification.
- Marlowe -> Wren: expects and respects refusal to overstate uncertainty.
- Wren -> Marlowe: expects respect for uncertainty because he supported that boundary before.
- Wren -> Voss: respects competence, expects pressure toward a firm conclusion.
- Voss -> Wren: respects observational care, expects withholding when pressed.

Unsupported protective/paternal framing of Marlowe toward Wren is excluded.

Relationship records must point to the correct target Character and cite the relevant RH provenance.

## Relationship-derived memories
Patch 0002.2 must include only memories earned by RH-001/RH-002 and needed for the approved fixture. Their provenance must point to the corresponding relationship-history record.

The Missing Raft contract will expose one stable `RelationshipContextRecordIds` set containing:
- all six relationship records;
- RH-001 and RH-002;
- every relationship-derived memory record.

This set is experiment-specific code metadata, not fixture-authored ACL/fictional state. E0-D may later remove exactly this set for the relationship-context ablation without adding tags to the generic fixture dialect.

## Chronology
Chronology remains the single semantically ordered collection.

Patch 0002.2 defines an exact ordered list of Historical Truth / relationship-history IDs that establish the fixture's past. The contract validator requires exact equality, not merely membership.

The chronology must make causal order unambiguous: relevant relationship history precedes the present raft event; correct securing precedes Marlowe's release; release precedes the current carrying the raft away; Wren's observation of Marlowe returning occurs after the release/loss event.

## Provenance requirements
The generic DAG invariant remains mandatory.

The Missing Raft validator additionally requires exact provenance for authority-relevant derived records. At minimum:
- current raft-gone state traces to the release/current/loss history as appropriate;
- Marlowe's knowledge of his own release traces to the authoritative release event;
- Wren's observation of Marlowe returning traces only to the return event, not to the release;
- Wren's suspicion traces to permitted observations/history rather than privileged Production truth;
- Voss's accidental-loss belief traces to her permitted current knowledge/observations rather than the hidden release event;
- relationships and relationship-derived memories trace to RH-001/RH-002.

The contract validator compares stable record IDs and provenance edges. It does not infer semantics from prose.

## Contract-validation strategy
`MissingRaftContractValidator` should fail closed unless:
- fixture identity/version/contracts are exact;
- Character IDs and Scene roster are exact;
- initial opportunity is Voss;
- expected record-ID sets appear in their exact authority/owner collections;
- no extra record appears in a frozen category without a fixture-version change;
- chronology exactly matches the frozen ordered ID list;
- each directional relationship has the expected owner, target, and provenance;
- required Character-private records are owned by the correct Character;
- prohibited privileged record IDs are absent from Voss/Wren subjective collections;
- required provenance edges are exact;
- `RelationshipContextRecordIds` exactly matches the frozen ablation surface.

This protects structure while leaving prose canonicalization/hash to Patch 0003.

## Tests
Use one canonical Missing Raft JSON fixture. Negative tests clone/mutate it one invariant at a time.

Required tests include:
- canonical Missing Raft fixture passes generic + Missing Raft validation;
- wrong fixture family/version fails;
- wrong Character set fails;
- wrong initial opportunity fails;
- chronology reorder/removal/addition fails;
- raft-failure record moved from unresolved proposition to Historical Truth fails;
- Wren observation provenance changed from return event to release event fails;
- Wren suspicion given privileged release provenance fails;
- Voss belief given privileged release provenance fails;
- Marlowe's private release knowledge removed or reassigned fails;
- relationship owner/target mismatch fails;
- RH provenance removed or changed fails;
- relationship-derived memory provenance changed fails;
- extra frozen-category record fails;
- generic provenance-cycle regression remains green;
- generic smoke fixture remains green and is rejected by Missing Raft-specific validation.

Do not create a directory of nearly identical malformed fixture files.

## Harness behavior
The existing Harness remains generic by default. Patch 0002.2 may add the smallest explicit Missing Raft validation selection needed for the runtime gate, but must not infer contract type from display prose.

Preferred minimal behavior: after generic validation, dispatch a known fixture-family contract by stable fixture identity. Unknown generic E0 fixtures remain valid generic fixtures; only Missing Raft is subjected to Missing Raft-specific law.

Do not build a plugin/registry framework for one experiment contract.

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
3. Harness validates the canonical Missing Raft fixture as `ensemble.e0.missing-raft@0.1.0`.
4. Generic smoke fixture remains valid through the generic path.
5. Final baseline comparison finds no duplicate schema, legacy compatibility layer, prose parser, generic over-abstraction, or scope leakage.

Only after this gate passes may Patch 0003 freeze the exact semantic fixture bytes with ECJ-1 + SHA-256.
