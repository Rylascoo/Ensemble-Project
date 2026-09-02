# H1 Patch 0005 — Deterministic Context Composer + Dual Context Identity

Status: blueprint proposal 0.1 — APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0004
Branch: `h1-patch-0005-context-composer-blueprint`

## 1. Purpose

Implement the second deterministic disclosure boundary in the E0 H1 spine:

`Production State -> deterministic Access Control -> CharacterAccessProjection -> deterministic Context Composer -> bounded ContextPacket -> later Performer boundary`

Patch 0005 answers one narrow question:

> Given only the Character-safe output of Patch 0004 and the current opportunity Character, can Ensemble deterministically produce a bounded, category-preserving, provider-neutral context packet whose semantic content and exact rendered disclosure are independently identifiable, without recovering prohibited Production state or entering provider/runtime orchestration?

Patch 0005 does **not** call a model, choose a provider, implement the Director, persist accepted Takes, interpret consequences, or create a final prompt/request protocol.

## 2. Authority and continuity

Current GitHub authority establishes:

- `main` is held at validated Patch 0004;
- `CharacterAccessProjection` is the only approved Character-safe information input boundary;
- Access Control returns the maximal permitted set and explicitly leaves relevance, token budgeting, summarization, and prompt construction to later Context Composer work;
- the future Context Composer must accept the `CharacterAccessProjection`, not `CharacterAccessEvaluation`, so denied audit IDs cannot become a disclosure side channel;
- `InitialOpportunity` / Director state is separate from Access Control and may be combined only after the access boundary;
- Character-facing state has already stripped authoritative provenance;
- ContextPacketHash/StateHash were deliberately deferred beyond Patch 0004.

Frozen Blueprint 0.1 additionally requires:

- Context Composer answers: **of the permitted material, what matters now?**;
- prohibited information must never be exposed to a relevance system and then merely instructed not to use it;
- context packets conceptually preserve identity, current situation, knowledge, belief, memory, presence, relationships, goals, pressure, recent events, and current opportunity as distinct concerns;
- imported/user text and fictional dialogue are untrusted creative content and must remain distinct from trusted structured state/system authority;
- E0 uses explicit context packets and provenance capture;
- E0 explicitly excludes context/token/cost optimization; inefficiency is acceptable while behavioral ambiguity and integrity violations are not.

Earlier approved H1 continuity also recorded an intent to distinguish a **StructuredContextHash** from a **RenderedContextHash**. The parent H1 blueprint file is not present in current GitHub, so that recollection is not treated as executable authority by itself. Explicit approval of this Patch 0005 blueprint re-checkpoints the dual-identity rule in the authoritative repository.

## 3. Why Patch 0005 is deterministic and lossless for E0

Blueprint 0.1 permits a future Context Composer to use semantic or probabilistic relevance **only inside the already-authorized set**. That is a product capability, not an E0 requirement.

E0 explicitly excludes context optimization. Introducing a relevance model now would add a new probabilistic variable before the basic behavioral architecture is validated and would make E0-A harder to interpret.

Therefore `ensemble.e0.context.full-authorized.v1` uses this E0 reference rule:

> Every record already permitted by `CharacterAccessProjection` is included exactly once in its existing authority category. No permitted record is dropped, summarized, paraphrased, merged, reclassified, or semantically ranked by Patch 0005.

This is intentionally a **lossless bounded composer**:

- bounded because Access Control has already removed prohibited Production/other-Character information;
- lossless because the current E0 composer includes the full permitted projection;
- deterministic because no model, embedding, heuristic relevance score, token budget, randomness, clock, or provider state participates.

A later approved composer may narrow the permitted set for quality/cost once E0 evidence exists. That later optimization must preserve the same Access-Control-before-relevance ordering and must be evaluated as a new behavior, not silently substituted into the E0 reference packet.

## 4. Architecture boundary

### Input

Patch 0005 public composition input is exactly:

```text
CharacterAccessProjection
CurrentOpportunityCharacterId
```

The Composer must not accept:

- `ValidatedFixture`;
- `ValidatedCharacter`;
- `ValidatedRecord`;
- `CharacterAccessEvaluation`;
- Access Control deny decisions;
- Production chronology;
- Production HistoricalTruth / WorldState / UnresolvedProposition;
- fixture provenance;
- arbitrary free-form Director instructions;
- provider/model configuration;
- imported/user content;
- credentials;
- diagnostics.

The current opportunity is a structural run-control input only. In Patch 0005 it contains no arbitrary prose and grants no additional knowledge.

### Output

Preferred output:

```text
ContextCompositionEvaluation
- Packet
- Trace
```

where:

- `Packet` is the immutable Character-safe context artifact that later Performer/provider work may consume;
- `Trace` is a local deterministic composition audit and must not be supplied as Performer context.

As with Patch 0004, public safe outputs are read-only and their constructors remain Core-internal so external assemblies cannot fabricate an authoritative ContextPacket through public construction.

## 5. Context contract identifiers

Patch 0005 freezes:

```text
ContextSchemaVersion = ensemble.e0.context.v1
CompositionContract = ensemble.e0.context.full-authorized.v1
```

`ContextSchemaVersion` identifies the structured packet schema.

`CompositionContract` identifies the selection behavior: full inclusion of every already-permitted record, category-preserving and deterministically ordered.

Neither identifier grants access or truth authority.

## 6. Structured ContextPacket schema

The structured packet preserves authority distinctions instead of flattening context into prose.

Canonical logical shape:

```text
ContextPacket
- ContextPacketId
- SchemaVersion
- CompositionContract
- SceneId
- SubjectCharacterId
- OpportunityCharacterId
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
- RecentPerformances
- StructuredContextHash
- Rendered
- RenderedContextHash
```

### Safe record shapes

The packet may reuse or copy only already-safe disclosure values:

```text
ContextRecord
- RecordId
- Text

ContextRelationship
- RecordId
- TargetCharacterId
- Text

ContextParticipant
- CharacterId
- DisplayName
```

No ContextPacket state record contains authoritative fixture provenance.

### RecentPerformances

`RecentPerformances` is a fixed packet section because Blueprint 0.1 requires later context to distinguish “what just happened.” However, Patch 0005 has no accepted-history authority producer yet.

Therefore:

- the section exists in schema v1;
- every Patch 0005 opening packet contains an empty `RecentPerformances` array;
- Patch 0005 must not fabricate transcript/history content merely to populate it;
- a later approved H1 history/commit slice may populate this section only from a safe accepted-history projection, while preserving the separate authority layer described in Section 9.

This freezes the semantic slot without pretending persistence already exists.

## 7. Exact inclusion matrix

For `ensemble.e0.context.full-authorized.v1`, composition is mechanical:

| CharacterAccessProjection field | ContextPacket behavior |
| --- | --- |
| SceneId | include |
| SubjectCharacterId | include |
| Roster | include all, unchanged semantically |
| SceneState | include all |
| Pressures | include all |
| Constitution | include all |
| Disposition | include all |
| Circumstance | include all |
| Observations | include all |
| Knowledge | include all |
| Beliefs | include all |
| Suspicions | include all |
| Memories | include all |
| Goals | include all |
| Relationships | include all |
| Access decisions | forbidden input |
| denied Record IDs | impossible through approved input boundary |
| fixture provenance | forbidden input |
| chronology | forbidden input |

No story-text parsing is permitted.

No category may be merged into another. In particular:

- Knowledge remains Knowledge;
- Belief remains Belief;
- Suspicion remains Suspicion;
- Memory remains Memory;
- Observation remains Observation;
- Relationship state remains directional Relationship state.

This protects the Blueprint 0.1 “statement != fact” law through the Performer-context boundary.

## 8. Current opportunity contract

`CurrentOpportunityCharacterId` is supplied separately from `CharacterAccessProjection`.

Patch 0005 fails closed unless:

1. it is initialized;
2. it equals `CharacterAccessProjection.SubjectCharacterId`;
3. it appears in the packet roster.

This proves the packet is being composed for the Character whose agency is currently salient.

The opportunity input does **not**:

- force speech;
- prescribe an action;
- add a direction;
- reveal hidden state;
- change goals/beliefs;
- grant a new record;
- select a provider;
- authorize cost or retries.

In the initial Missing Raft opening packet the current opportunity is `VOSS`, matching the frozen fixture.

Patch 0005 does not implement the Director. A later Director may choose a new Character opportunity; the Composer continues to receive only the resulting safe structural opportunity identity unless a stronger approved contract explicitly adds more run-control context.

## 9. Rendered context is provider-neutral and authority-layered

Patch 0005 renders the structured packet into deterministic provider-neutral context **without** constructing a final provider request.

The rendered result is deliberately separated into layers:

```text
RenderedContext
- TrustedStateText
- RecentPerformanceText
- OpportunityText
```

### TrustedStateText

Contains only structured state already present in the approved packet:

1. subject identity;
2. current Scene state;
3. circumstance;
4. observations;
5. knowledge;
6. beliefs;
7. suspicions;
8. memories;
9. who is present;
10. relationships;
11. goals;
12. pressures.

### RecentPerformanceText

Patch 0005 renders this as an empty string because `RecentPerformances` is empty.

When a later approved history slice populates it, accepted fictional dialogue/performance text remains in this separate untrusted-creative-content layer rather than being mixed into trusted structured state.

### OpportunityText

Contains only a fixed deterministic statement that the subject has the current opportunity. It does not contain arbitrary Director prose.

### System contract exclusion

The Performer system contract is not part of `RenderedContext` and is not covered by `RenderedContextHash`.

A later provider adapter must keep:

- system/behavioral contract;
- rendered trusted state;
- recent fictional performance;
- user/imported content;

in distinct provider-authority layers to the extent the provider protocol permits.

Patch 0005 therefore does not claim that `RenderedContextHash` is a final provider-request hash.

## 10. Fixed provider-neutral rendering contract

Patch 0005 rendering is not artistic summarization. It is a deterministic textual projection of packet fields.

All sections are emitted in a fixed order with LF (`U+000A`) line endings only. Empty record categories are represented explicitly as `- none` so absence is deterministic rather than silently ambiguous.

Preferred fixed headings:

```text
[WHO YOU ARE]
[WHAT IS HAPPENING]
[RIGHT NOW]
[WHAT YOU OBSERVED]
[WHAT YOU KNOW]
[WHAT YOU BELIEVE]
[WHAT YOU SUSPECT]
[WHAT YOU REMEMBER]
[WHO IS PRESENT]
[RELATIONSHIPS]
[WHAT YOU WANT]
[PRESSURES]
```

Rendering rules:

- record text is emitted verbatim after validation; no paraphrase or summary;
- record IDs are omitted from provider-neutral text because they are diagnostic identity, not creative content;
- roster renders display names only;
- relationships render the target display name plus relationship text;
- relationship targets must resolve in the packet roster or composition fails closed;
- list order follows the canonical structured order;
- no provider/model name, fixture hash, denied ID, provenance source ID, commit/debug data, or access-decision data appears in rendered text.

`OpportunityText` is fixed as:

```text
You have the current opportunity to act.
```

This is run-control context, not a command to speak or a prescribed outcome.

The exact newline/trailing-newline contract must be frozen in implementation tests before machine validation.

## 11. Deterministic structured canonicalization

`StructuredContextHash` is SHA-256 over a deterministic canonical JSON representation of the structured semantic packet **excluding**:

- `ContextPacketId`;
- `StructuredContextHash` itself;
- `Rendered` text;
- `RenderedContextHash`.

Canonical root property order:

1. `schemaVersion`
2. `compositionContract`
3. `sceneId`
4. `subjectCharacterId`
5. `opportunityCharacterId`
6. `roster`
7. `sceneState`
8. `pressures`
9. `constitution`
10. `disposition`
11. `circumstance`
12. `observations`
13. `knowledge`
14. `beliefs`
15. `suspicions`
16. `memories`
17. `goals`
18. `relationships`
19. `recentPerformances`

Nested order:

- roster entry: `characterId`, `displayName`;
- record: `recordId`, `text`;
- relationship: `recordId`, `targetCharacterId`, `text`;
- future recent performance entry: `takeId`, `characterId`, `text`.

Canonical ordering:

- roster by `CharacterId`, ordinal;
- every record collection by `RecordId`, ordinal;
- relationships by relationship `RecordId`, ordinal;
- future recent performances preserve causal sequence and are **not** set-sorted.

String/UTF-8 discipline reuses the frozen ECJ-1 byte rules:

- UTF-8, no BOM;
- minified canonical JSON;
- NFC semantic text;
- reject NUL, CR, and invalid surrogate sequences;
- LF preserved;
- exact short escapes for quote, backslash, backspace, tab, LF, form feed;
- other C0 controls as lowercase `\u00xx`;
- valid non-BMP scalars emitted directly as shortest UTF-8.

### Shared canonical JSON primitive

Do not duplicate ECJ-1 string escaping in a second implementation.

Patch 0005 may extract the already-frozen JSON string/UTF-8 primitive from `Ecj1FixtureCanonicalizer` into one small internal canonical-JSON helper used by both fixture and context canonicalizers.

That refactor is permitted only if the existing Missing Raft ECJ-1 regression remains byte-identical at exactly:

- `9112` bytes;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

No fixture semantic/order rule moves into a generic reflection/serializer framework.

## 12. Dual context identity

Patch 0005 freezes two distinct hashes.

### StructuredContextHash

```text
SHA-256(canonical structured semantic ContextPacket content)
```

External representation:

- exactly 64 lowercase ASCII hex characters.

It answers:

> What semantic Character context was selected and structurally available for this opportunity?

### RenderedContextHash

`RenderedContextHash` is SHA-256 over a canonical three-layer rendered envelope:

```json
{"trustedStateText":"...","recentPerformanceText":"...","opportunityText":"..."}
```

using the same canonical JSON string/UTF-8 scalar rules.

It answers:

> What exact provider-neutral textual disclosure did the Context Composer render from that structured packet?

It does **not** identify a final provider request, system prompt, credentials, adapter framing, model configuration, or provider response.

## 13. ContextPacketId

`ContextPacketId` is content-addressed from the structured semantic identity:

```text
CTX:<StructuredContextHash>
```

The existing `ContextPacketId` strong ID can represent this value within the 128-character canonical-ID limit.

Consequences:

- identical structured semantic packets receive identical `ContextPacketId` values;
- a renderer-only change may preserve `ContextPacketId` while changing `RenderedContextHash`;
- a semantic change to included state/opportunity changes both `StructuredContextHash` and `ContextPacketId`;
- packet identity does not grant access, truth, signing, or publisher authenticity.

Every later external disclosure should record both packet ID and rendered hash so semantic context identity and exact rendered disclosure remain distinguishable.

## 14. Composition trace

Patch 0005 retains a local audit surface separate from Performer context.

Preferred minimal trace:

```text
ContextCompositionTrace
- CompositionContract
- IncludedRecordIds
- IncludedRosterCharacterIds
- OpportunityCharacterId
- StructuredContextHash
- RenderedContextHash
```

Rules:

- included Record IDs contain every packet record exactly once;
- IDs sort ordinally;
- roster IDs sort ordinally;
- the trace contains no denied Access Control IDs;
- the trace contains no authoritative fixture provenance;
- the trace contains no record text;
- the future Performer/provider API must accept `ContextPacket`, not `ContextCompositionEvaluation`.

This trace is E0 diagnostic/experimental provenance, not Character knowledge.

## 15. Fail-closed behavior

Use one small Context-Composer-specific domain exception.

Composition fails closed when:

- packet input is null;
- current opportunity Character ID is uninitialized;
- opportunity Character differs from packet subject;
- subject is absent from roster;
- roster has duplicate Character IDs;
- a relationship target cannot resolve to a roster participant;
- a safe packet text value violates the canonical text discipline unexpectedly;
- a packet Record ID appears in more than one authority category;
- output hash/ID invariants cannot be satisfied.

Patch 0005 may trust Patch 0004's normal invariants but should defensively guard conditions that would make a packet ambiguous or forgeable.

A composition failure is technical authority failure. It must never become fictional behavior.

## 16. Determinism and mutation rules

For identical semantic `CharacterAccessProjection + CurrentOpportunityCharacterId` input:

- packet structured content is byte-identical;
- `StructuredContextHash` is identical;
- `ContextPacketId` is identical;
- rendered layer text is byte-identical;
- `RenderedContextHash` is identical;
- composition trace is identical.

The Composer must not mutate `CharacterAccessProjection` or any safe record.

No filesystem state, clock, culture, randomness, dictionary enumeration, network, model output, provider state, process-global mutable state, or machine architecture may influence composition.

## 17. Missing Raft exact opening expectations

For the frozen opening fixture, initial opportunity is `VOSS`.

The Patch 0005 Missing Raft reference composition therefore uses:

```text
ValidatedFixture
-> MissingRaftContract.Validate
-> CharacterBoundedAccessControl.Evaluate(... VOSS ...).Projection
-> DeterministicContextComposer.Compose(projection, VOSS)
```

The packet must include exactly the already-approved Voss access projection:

Shared:
- all four SceneState records;
- `PRESSURE-ISOLATION`;
- roster identities for MARLOWE, VOSS, WREN.

Voss-owned:
- `CON-VOSS`;
- `DISP-VOSS`;
- `KNOW-VOSS-CURRENT-STRENGTHENED`;
- `BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE`;
- `MEM-VOSS-RH001`;
- `MEM-VOSS-RH002`;
- `GOAL-VOSS`;
- `REL-VOSS-MARLOWE`;
- `REL-VOSS-WREN`.

The packet must not contain, in structured content, rendering, or trace:

- `WORLD-CURRENT-STRENGTHENED`;
- `HT-MARLOWE-RELEASED-RAFT`;
- `KNOW-MARLOWE-RELEASED-RAFT`;
- Wren's Observation/Suspicion;
- any other denied Production or other-Character record;
- any hidden fixture provenance source ID.

The opening `RecentPerformances` section is empty.

Patch 0005 should also compose deterministic packets for Marlowe and Wren using explicit matching opportunity IDs in tests, but those are synthetic composition-boundary checks rather than claims that the opening fixture initially selects them.

## 18. Required tests

Use the existing canonical smoke/Missing Raft fixtures and Patch 0004 access operation. Do not create duplicate fixture copies.

Required coverage:

1. generic smoke Character packet composes from `CharacterAccessProjection` only;
2. Missing Raft Voss opening packet exact included Record-ID set equals Patch 0004 permitted set;
3. Marlowe and Wren synthetic matching-opportunity packet sets equal their Patch 0004 permitted sets;
4. no denied Production/other-Character IDs enter packet, rendering, or trace;
5. access audit decisions cannot enter Composer input/output;
6. every permitted record appears exactly once and in its original category;
7. Knowledge/Belief/Suspicion/Memory/Observation categories remain distinct;
8. relationship directionality and target display-name rendering remain correct;
9. roster identity is complete and deterministic;
10. current opportunity must equal subject and roster membership;
11. `RecentPerformances` is present and empty in Patch 0005;
12. trusted/recent/opportunity rendered layers remain separate;
13. rendered output contains no Record IDs/provenance/debug/provider data;
14. empty categories render deterministically;
15. source reordering of semantically unordered fixture/access collections does not change packet bytes/hashes/rendering;
16. repeated composition is byte-identical;
17. semantic record text mutation changes StructuredContextHash, ContextPacketId, and RenderedContextHash;
18. a category move with the same text/ID changes StructuredContextHash even if rendered prose might remain similar;
19. display-name mutation changes hashes where represented;
20. opportunity mutation fails closed when it no longer equals subject;
21. StructuredContextHash is exactly 64 lowercase hex;
22. RenderedContextHash is exactly 64 lowercase hex;
23. ContextPacketId equals `CTX:<StructuredContextHash>`;
24. renderer-only test mutation changes RenderedContextHash without redefining structured semantic hash contract;
25. projection/context output constructors are non-public and safe authority objects cannot be externally fabricated through public constructors;
26. composition does not mutate access projection or alter frozen Missing Raft fixture hash;
27. shared canonical JSON extraction preserves existing ECJ-1 exact 9112-byte Missing Raft output/digest;
28. all existing 90 Core tests remain green;
29. existing Missing Raft Harness runtime remains PASS/0;
30. existing generic smoke Harness runtime remains PASS/0.

Tests should compare exact bytes/hashes/sets, not story-text heuristics.

## 19. Harness behavior

Patch 0005 does not need a new end-user/debug CLI mode merely to print Character context.

Normal Harness validation output remains unchanged so denied or sensitive context details are not casually emitted to console.

Native Core tests are sufficient to exercise packet composition and exact hashes during this patch. Existing Harness Missing Raft and smoke validation remain regression gates.

If a tiny internal test-only helper is needed, prefer test code over a new public Harness surface.

## 20. E0-D ablation boundaries

Patch 0005 must not turn experimental controls into hidden production switches.

### Relationships omitted

The later E0-D relationship-omission control operates **after Access Control** on already-permitted material and before/at composition. It must be a separately labeled experimental composition path, not a mutable boolean inside the reference `full-authorized.v1` contract.

### Omniscient context

The later E0-D omniscient condition remains outside safe Access Control and outside the reference Composer. It must not be implemented as a permissive flag or alternate branch inside `DeterministicContextComposer`.

Patch 0005 implements only the safe E0 reference composition contract.

## 21. Security and authority properties

- Context Composer can only reduce/reformat already-permitted information; Patch 0005's reference contract chooses not to reduce it.
- It cannot recover data stripped by Access Control.
- It never receives denied Access Control decisions.
- It never traverses authoritative fixture provenance.
- It never interprets story prose to infer access or truth category.
- It never merges Claim/Belief/Suspicion/Knowledge/Memory into a generic fact bucket.
- Current opportunity adds salience, not knowledge authority.
- Rendered provider-neutral text omits internal Record IDs and provenance.
- Recent fictional performance remains a separate future authority layer.
- Packet/trace constructors are not public authority-forging surfaces.
- hashes establish deterministic identity/integrity of local content only; they are not signatures or authorization.
- technical failure remains technical and never becomes fiction.

## 22. ARM64 and battery suitability

Patch 0005 is deterministic in-memory CPU work over a fixture already bounded by E0 limits.

Expected cost is small linear traversal plus ordinal sorting/canonicalization and two SHA-256 operations.

It performs:

- no network I/O;
- no background polling;
- no provider call;
- no AI inference;
- no GPU work;
- no NPU work.

This is deliberate. Access/context authority and identity are safer and cheaper as deterministic Core logic. NPU resources remain reserved for future workloads that actually require inference and demonstrate value.

No NPU execution/performance claim is made.

## 23. Explicit exclusions

Patch 0005 does not implement:

- semantic/probabilistic relevance ranking;
- embeddings/vector search;
- token budgeting or truncation;
- summarization/paraphrase/deduplication;
- provider prompt/system-contract construction;
- provider/model adapters or calls;
- provider request hash;
- Director/opportunity selection;
- accepted Take/history projection;
- non-empty recent performance context;
- Context cost optimization;
- E0-D omniscient execution;
- E0-D relationship-omission execution;
- Integrity Validator;
- State Interpreter / State Authority;
- ProductionState / StateHash;
- causal commit/persistence/recovery replay;
- observation engine;
- WinUI;
- Windows AI/NPU;
- packaging/WACK/Store work.

## 24. Exit gate

Before promotion:

1. this blueprint is explicitly approved as the canonical Patch 0005 implementation specification;
2. implementation begins from then-current `main` on a dedicated branch;
3. Composer public input is limited to `CharacterAccessProjection + CurrentOpportunityCharacterId`;
4. exact `full-authorized.v1` inclusion matrix receives adversarial review;
5. packet categories remain structurally distinct;
6. packet/render/trace carry no denied Access Control IDs or fixture provenance;
7. structured canonicalization/property order is frozen and independently reviewed;
8. shared canonical JSON extraction, if performed, leaves ECJ-1 exactly byte-identical;
9. dual hash semantics are proven distinct and deterministic;
10. `ContextPacketId = CTX:<StructuredContextHash>` is proven exact;
11. rendering is provider-neutral, layered, deterministic, LF-only, and contains no internal IDs/provenance;
12. opening RecentPerformances remains empty rather than fabricated;
13. Missing Raft Voss exact packet-set test passes;
14. Marlowe/Wren synthetic boundary packets pass exact-set tests;
15. source-order invariance and repeated-byte identity tests pass;
16. output authority objects cannot be publicly forged;
17. native Windows ARM64 Core/Harness build passes with warnings-as-errors;
18. full Core test suite passes on target machine;
19. Missing Raft and generic smoke Harness regressions pass/0;
20. final hygiene review finds no provider abstraction, relevance model, ACL bypass, omniscient flag, persistence, or later-patch scope creep;
21. evidence distinguishes exact machine-tested executable head from later documentation-only closure commits.

## 25. Material approval decisions

Explicit approval freezes these persistent Patch 0005 decisions:

1. Patch 0005 is the deterministic Context Composer boundary immediately after validated Access Control;
2. E0 reference composition is lossless/full-authorized rather than semantic relevance filtering;
3. Composer accepts only `CharacterAccessProjection + CurrentOpportunityCharacterId`;
4. opportunity identity must equal packet subject and grants no additional knowledge;
5. packet schema preserves all authority categories separately;
6. `RecentPerformances` is a fixed schema section but remains empty until an approved accepted-history authority exists;
7. provider-neutral rendering separates trusted state, recent performance, and opportunity layers;
8. system/provider request construction remains outside Context Composer;
9. structured packet identity and exact rendered disclosure use separate SHA-256 hashes;
10. structured hash uses deterministic canonical JSON with frozen property order and ECJ-1 byte/string discipline;
11. canonical JSON scalar/string emission is shared rather than duplicated, with exact ECJ-1 regression required;
12. `ContextPacketId` is content-addressed as `CTX:<StructuredContextHash>`;
13. local composition trace is separate from Performer-facing packet context;
14. no semantic relevance, token optimization, provider calls, Director logic, persistence, or E0-D bypass enters Patch 0005.

Implementation must not begin until these decisions are approved.
