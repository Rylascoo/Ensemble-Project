# H1 Patch 0005 — Deterministic Context Composer + Dual Context Identity

Status: blueprint proposal 0.3 — APPROVAL REQUIRED; implementation not started
Parent baseline: validated H1 Patch 0004
Branch: `h1-patch-0005-context-composer-blueprint`

## 1. Purpose

Implement the second deterministic disclosure boundary in the E0 H1 spine:

`Production State -> deterministic Access Control -> CharacterAccessProjection -> deterministic Context Composer -> bounded ContextPacket -> later Performer boundary`

Patch 0005 answers one narrow question:

> Given only the Character-safe output of Patch 0004 and the current opportunity Character, can Ensemble deterministically produce a bounded, category-preserving, provider-neutral context packet whose semantic content and exact rendered disclosure are independently identifiable, without recovering prohibited Production state or entering provider/runtime orchestration?

Patch 0005 does **not** call a model, choose a provider, implement the Director, persist accepted Takes, interpret consequences, or create a final provider request.

## 2. Authority and continuity

Current GitHub authority establishes:

- `main` is held at validated Patch 0004;
- `CharacterAccessProjection` is the only approved Character-safe information input boundary;
- Access Control returns the maximal permitted set and explicitly leaves relevance, token budgeting, summarization, and prompt construction to later Context Composer work;
- the future Context Composer must accept `CharacterAccessProjection`, not `CharacterAccessEvaluation`, so denied audit IDs cannot become a disclosure side channel;
- initial/current opportunity state is separate from Access Control and may be combined only after the access boundary;
- Character-facing state has already stripped authoritative fixture provenance;
- ContextPacketHash/StateHash were deliberately deferred beyond Patch 0004.

Frozen Blueprint 0.1 additionally requires:

- Context Composer answers: **of the permitted material, what matters now?**;
- prohibited information must never be shown to a relevance system and then merely instructed not to use it;
- context packets conceptually keep identity, current situation, knowledge, belief, memory, presence, relationships, goals, pressure, recent events, and current opportunity distinct;
- fictional dialogue and imported/user text are untrusted creative content, distinct from trusted structured state/system authority;
- E0 uses explicit context packets and provenance capture;
- E0 explicitly excludes context/token/cost optimization; inefficiency is acceptable while behavioral ambiguity and integrity violations are not.

Earlier approved H1 continuity recorded an intent to distinguish `StructuredContextHash` from `RenderedContextHash`. The parent H1 blueprint file is absent from current GitHub, so recollection is not treated as executable authority by itself. Explicit approval of this Patch 0005 blueprint re-checkpoints the dual-identity rule in the authoritative repository.

## 3. Deterministic full-authorized reference composition

Blueprint 0.1 permits a future Composer to use semantic/probabilistic relevance **only inside the already-authorized set**. That is a later product capability, not an E0 requirement.

E0 explicitly excludes context optimization. Introducing relevance inference now would add a new probabilistic variable before the behavioral architecture is validated.

Patch 0005 therefore freezes:

`CompositionContract = ensemble.e0.context.full-authorized.v1`

Reference rule:

> Every record already present in `CharacterAccessProjection` is included exactly once in its existing authority category. No permitted record is dropped, summarized, paraphrased, merged, reclassified, ranked, or token-truncated.

This is a **lossless bounded composer**:

- bounded because Patch 0004 has already removed prohibited Production/other-Character information;
- lossless because every permitted record remains available in the E0 reference packet;
- deterministic because no model, embedding, relevance score, token budget, randomness, clock, or provider state participates.

A future approved Composer may narrow the permitted set after E0 evidence exists. It must preserve Access-Control-before-relevance and must be treated as a new behavioral contract, not silently substituted into the E0 reference.

## 4. Architecture boundary

### Public input

Exactly:

```text
CharacterAccessProjection
CurrentOpportunityCharacterId
```

The Composer must not accept:

- `ValidatedFixture`, `ValidatedCharacter`, or `ValidatedRecord`;
- `CharacterAccessEvaluation` or Access deny decisions;
- Production chronology, HistoricalTruth, WorldState, or UnresolvedProposition;
- fixture provenance;
- arbitrary free-form Director instruction;
- provider/model configuration;
- imported/user content;
- credentials or diagnostics.

Current opportunity is structural run-control only. It grants no knowledge.

### Output

```text
ContextCompositionEvaluation
- Packet
- Trace
```

`Packet` is the immutable Character-safe artifact that later Performer/provider work may consume.

`Trace` is local deterministic experimental provenance and must not be supplied as Performer context.

All public packet/render/trace types are read-only and have Core-internal constructors. External assemblies may inspect Composer output but cannot fabricate authoritative ContextPacket objects through public constructors.

## 5. Context contracts

Patch 0005 freezes:

```text
ContextSchemaVersion = ensemble.e0.context.v1
CompositionContract = ensemble.e0.context.full-authorized.v1
RenderingContract = ensemble.e0.context.render.v1
```

- `ContextSchemaVersion` versions structured semantic packet shape.
- `CompositionContract` versions selection behavior.
- `RenderingContract` versions exact provider-neutral text rendering behavior.

None grants access or truth authority.

## 6. Structured ContextPacket schema

Logical packet shape:

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

Safe value shapes implemented by Patch 0005:

```text
ContextParticipant
- CharacterId
- DisplayName

ContextRecord
- RecordId
- Text

ContextRelationship
- RecordId
- TargetCharacterId
- Text
```

No packet state record contains authoritative fixture provenance or denied Access Control data.

### RecentPerformances

`recentPerformances` is a fixed root schema-v1 array because frozen Blueprint 0.1 requires later context to distinguish “what just happened.” Patch 0005 has no accepted-history authority producer yet.

Therefore:

- every Patch 0005 packet canonicalizes `recentPerformances` as exactly `[]`;
- `RecentPerformanceText` is exactly empty;
- Patch 0005 introduces **no executable recent-performance DTO, Take-history projection, or non-empty serializer path**;
- Patch 0005 must not fabricate transcript/history content;
- a later explicitly approved accepted-history/commit slice owns the item schema and safe population path before `recentPerformances` may become non-empty;
- accepted fictional performance must remain a separate authority layer from trusted structured state.

This reserves the semantic section without inventing history infrastructure early.

## 7. Exact inclusion matrix

For `full-authorized.v1`:

| CharacterAccessProjection field | ContextPacket |
| --- | --- |
| SceneId | include |
| SubjectCharacterId | include |
| Roster | include all |
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
| denied Record IDs | unavailable through approved input |
| fixture provenance | forbidden input |
| chronology | forbidden input |

No story-text parsing is permitted.

Authority categories are never merged. Knowledge remains Knowledge; Belief remains Belief; Suspicion remains Suspicion; Memory remains Memory; Observation remains Observation; directional Relationship remains Relationship.

This carries the frozen “statement != fact” law through the Performer-context boundary.

## 8. Current opportunity contract

`CurrentOpportunityCharacterId` is supplied separately from Access Control.

Composition fails closed unless it:

1. is initialized;
2. equals `CharacterAccessProjection.SubjectCharacterId`;
3. appears in the roster.

Opportunity does not force speech, prescribe an outcome, add direction prose, reveal hidden state, change beliefs/goals, select a provider, or authorize cost/retries.

For the frozen Missing Raft opening packet, opportunity is `VOSS`.

Patch 0005 does not implement the Director. A future Director may select a different Character; the reference Composer continues to receive only the resulting safe Character identity unless a stronger approved contract deliberately adds run-control context.

## 9. Rendered context is provider-neutral and layered

Patch 0005 produces deterministic provider-neutral context, not a final provider prompt/request.

```text
RenderedContext
- RenderingContract
- TrustedStateText
- RecentPerformanceText
- OpportunityText
```

`TrustedStateText` contains only packet state already permitted by Access Control.

`RecentPerformanceText` is empty in Patch 0005. Later accepted fictional performance must remain in this separate untrusted-creative layer.

`OpportunityText` contains only fixed run-control text.

The Performer system/behavior contract is outside `RenderedContext` and outside `RenderedContextHash`.

A later provider adapter must preserve separate authority layers for system contract, trusted state, recent fictional performance, and imported/user content to the extent the provider protocol permits.

`RenderedContextHash` is therefore **not** a final provider-request hash.

## 10. Exact provider-neutral rendering contract

All generated line endings are LF (`U+000A`). No rendered layer ends with a trailing LF.

`TrustedStateText` contains these sections in exact order, separated by exactly one blank line:

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

### `[WHO YOU ARE]`

Exact subsection order:

```text
Name:
<display-name bullet list>
Constitution:
<constitution bullet list>
Disposition:
<disposition bullet list>
```

Name contains exactly the subject display name resolved from roster.

### Other sections

- `[WHAT IS HAPPENING]` -> SceneState
- `[RIGHT NOW]` -> Circumstance
- `[WHAT YOU OBSERVED]` -> Observations
- `[WHAT YOU KNOW]` -> Knowledge
- `[WHAT YOU BELIEVE]` -> Beliefs
- `[WHAT YOU SUSPECT]` -> Suspicions
- `[WHAT YOU REMEMBER]` -> Memories
- `[WHO IS PRESENT]` -> roster display names
- `[RELATIONSHIPS]` -> subject directional Relationships
- `[WHAT YOU WANT]` -> Goals
- `[PRESSURES]` -> Pressures

### Bullet rendering

Normal text entry:

```text
- <first line>
  <second source line>
  <additional source lines>
```

Source LF is preserved semantically by splitting on LF and indenting continuation lines with exactly two spaces. Source text is never trimmed, paraphrased, summarized, or reordered.

Empty list:

```text
- none
```

Roster renders display names only.

Relationship:

```text
- <target display name>: <first relationship-text line>
  <relationship continuation lines>
```

Relationship target must resolve uniquely in roster or composition fails closed.

Record IDs, Character IDs, SceneId, fixture hash, provenance IDs, access decisions, provider/model data, and diagnostics are omitted from provider-neutral text.

`RecentPerformanceText` is exactly the empty string.

`OpportunityText` is exactly, with no trailing LF:

```text
You have the current opportunity to act.
```

This states salience; it does not require speech or a predetermined action.

## 11. Structured canonicalization

`StructuredContextHash` is SHA-256 over deterministic canonical JSON of structured semantic packet content, excluding:

- ContextPacketId;
- StructuredContextHash;
- RenderedContext;
- RenderedContextHash.

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

Nested property order:

- participant: `characterId`, `displayName`;
- record: `recordId`, `text`;
- relationship: `recordId`, `targetCharacterId`, `text`.

Ordering:

- roster by CharacterId, ordinal;
- every record collection by RecordId, ordinal;
- relationships by relationship RecordId, ordinal;
- `recentPerformances` is exactly empty in Patch 0005; future non-empty causal ordering is owned by the later accepted-history contract.

String/UTF-8 discipline reuses frozen ECJ-1 byte rules:

- UTF-8, no BOM, minified JSON;
- NFC text;
- reject NUL, CR, invalid surrogate sequence;
- LF preserved;
- short escapes for quote, backslash, backspace, tab, LF, form feed;
- other C0 controls as lowercase `\u00xx`;
- valid non-BMP scalars direct as shortest UTF-8.

### Shared canonical JSON primitive

Do not create a second JSON string-escaping implementation.

Patch 0005 may extract only the frozen JSON scalar/string + UTF-8 emission primitive from `Ecj1FixtureCanonicalizer` into a small internal helper shared by fixture and context canonicalizers.

The extraction is valid only if Missing Raft ECJ-1 remains exactly:

- `9112` UTF-8 bytes;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

Fixture-specific property/order rules remain explicit in `Ecj1FixtureCanonicalizer`; no reflection/serializer framework is introduced.

## 12. Dual context identity

### StructuredContextHash

```text
SHA-256(canonical structured semantic packet content)
```

External form: exactly 64 lowercase ASCII hex characters.

It identifies **what semantic Character context was selected for this opportunity**.

### RenderedContextHash

SHA-256 over this canonical rendered envelope, in exact property order:

```json
{"renderingContract":"ensemble.e0.context.render.v1","trustedStateText":"...","recentPerformanceText":"...","opportunityText":"..."}
```

using the same canonical JSON string/UTF-8 rules.

It identifies **which rendering contract and exact provider-neutral textual disclosure were emitted by Context Composer**.

It does not identify a system prompt, provider adapter framing, provider request, credentials, model settings, or provider response.

Changing rendering behavior requires a new RenderingContract value unless the change is proven byte-identical for every valid input under the same contract.

## 13. ContextPacketId

Content-addressed rule:

```text
ContextPacketId = CTX:<StructuredContextHash>
```

The existing 128-character strong ID supports the 68-character value.

Therefore:

- identical structured semantic packets have identical ContextPacketId;
- renderer-only contract change may keep ContextPacketId while changing RenderingContract/RenderedContextHash;
- semantic state/category/opportunity change changes StructuredContextHash and ContextPacketId;
- ContextPacketId is content identity, not authorization, source-fixture provenance, signing, or publisher identity.

Fixture/run/provider provenance remains a higher orchestration concern. A later run record should associate fixture identity + run/performer identity + ContextPacketId + RenderingContract + RenderedContextHash; Patch 0005 does not smuggle fixture identity back through the Character-safe Composer input.

## 14. Composition trace

Local-only audit:

```text
ContextCompositionTrace
- CompositionContract
- RenderingContract
- IncludedRecordIds
- IncludedRosterCharacterIds
- OpportunityCharacterId
- StructuredContextHash
- RenderedContextHash
```

Rules:

- every packet RecordId appears exactly once;
- Record IDs sort ordinally;
- roster IDs sort ordinally;
- no denied Access Control ID appears;
- no fixture provenance appears;
- no record text appears;
- future provider/Performer API accepts `ContextPacket`, not `ContextCompositionEvaluation`.

Trace is E0 diagnostic/experimental provenance, not Character knowledge.

## 15. Fail-closed behavior

Use one small Context-Composer-specific exception.

Fail closed when:

- projection is null;
- opportunity CharacterId is uninitialized;
- opportunity differs from subject;
- subject cannot resolve exactly once in roster;
- roster Character IDs are duplicated;
- relationship target cannot resolve exactly once in roster;
- packet RecordId appears in more than one category;
- safe text unexpectedly violates canonical string discipline;
- any output hash/ContextPacketId invariant fails.

Patch 0005 may trust normal Patch 0004 invariants but defensively guards any condition that would make packet authority ambiguous.

Composition failure remains technical authority failure and must never become fiction.

## 16. Determinism and immutability

For identical semantic `CharacterAccessProjection + CurrentOpportunityCharacterId` under the same contracts:

- canonical structured bytes are identical;
- StructuredContextHash and ContextPacketId are identical;
- all rendered layer strings are identical;
- RenderedContextHash is identical;
- trace is identical.

Composer never mutates Access projection or safe records.

No filesystem state, culture, clock, randomness, dictionary enumeration, network, model output, provider state, process-global mutable state, or machine architecture influences composition.

## 17. Missing Raft opening reference

Frozen opening opportunity: `VOSS`.

Reference path:

```text
ValidatedFixture
-> MissingRaftContract.Validate
-> CharacterBoundedAccessControl.Evaluate(fixture, VOSS).Projection
-> DeterministicContextComposer.Compose(projection, VOSS)
```

Packet includes exactly the approved Voss Access set.

Shared:

- `SCENE-RAFT-GONE`
- `SCENE-PROVISIONS-LIMITED`
- `SCENE-NO-IMMEDIATE-EMERGENCY`
- `SCENE-NO-EXTERNAL-COUNTDOWN`
- `PRESSURE-ISOLATION`
- MARLOWE/VOSS/WREN roster identities.

Voss-owned:

- `CON-VOSS`
- `DISP-VOSS`
- `KNOW-VOSS-CURRENT-STRENGTHENED`
- `BEL-VOSS-ACCIDENTAL-LOSS-PLAUSIBLE`
- `MEM-VOSS-RH001`
- `MEM-VOSS-RH002`
- `GOAL-VOSS`
- `REL-VOSS-MARLOWE`
- `REL-VOSS-WREN`

Packet, render, and trace must not contain `WORLD-CURRENT-STRENGTHENED`, `HT-MARLOWE-RELEASED-RAFT`, `KNOW-MARLOWE-RELEASED-RAFT`, Wren private Observation/Suspicion, another denied record, or hidden fixture provenance ID.

`recentPerformances` is empty.

Tests also compose Marlowe/Wren packets with synthetic matching opportunity CharacterIds to prove generic boundary behavior; this does not alter the fixture's actual opening opportunity.

## 18. Required tests

Use existing canonical smoke/Missing Raft fixtures and Patch 0004 Access Control. No duplicate fixture copies.

Required coverage:

1. generic smoke packet composes from CharacterAccessProjection only;
2. Missing Raft Voss exact packet Record-ID set equals Patch 0004 permitted set;
3. Marlowe/Wren synthetic matching-opportunity packet sets equal their permitted sets;
4. denied Production/other-Character IDs never enter packet, render, or trace;
5. Access audit decisions cannot enter Composer input/output;
6. every permitted record appears exactly once in original category;
7. Knowledge/Belief/Suspicion/Memory/Observation remain structurally distinct;
8. relationship directionality and target-name rendering are correct;
9. roster identity is complete and deterministic;
10. opportunity must equal subject and belong to roster;
11. `recentPerformances` canonicalizes exactly as `[]`; RecentPerformanceText is empty;
12. no executable recent-performance type/non-empty path is added;
13. trusted/recent/opportunity render layers are separate;
14. exact TrustedStateText headings/order/blank lines/no-trailing-LF contract;
15. bullet continuation and empty-list rendering contract;
16. provider-neutral render contains no Record IDs, Character IDs, SceneId, provenance, provider/debug data;
17. source reordering of semantically unordered fixture collections does not change structured bytes/hashes/render;
18. repeated composition is byte-identical;
19. generic semantic text mutation changes StructuredContextHash, ContextPacketId, and RenderedContextHash;
20. same ID/text moved to a different authority category changes StructuredContextHash;
21. display-name mutation changes identities where semantically represented;
22. mismatched/uninitialized opportunity fails closed;
23. both hashes are exactly 64 lowercase hex;
24. ContextPacketId equals `CTX:<StructuredContextHash>` exactly;
25. RenderingContract is included in rendered hash envelope/trace but excluded from StructuredContextHash;
26. output constructors are non-public and packet authority cannot be forged through public constructors;
27. composition does not mutate Access projection or alter Missing Raft fixture hash;
28. shared canonical JSON extraction preserves exact ECJ-1 9112-byte/digest regression;
29. independently derive and freeze expected Missing Raft Voss StructuredContextHash and RenderedContextHash before executable promotion;
30. all existing 90 Core tests remain green;
31. Missing Raft Harness regression remains PASS/0;
32. generic smoke Harness regression remains PASS/0.

Independent hash derivation must use a reference path separate from the production Context canonicalizer/renderer so the test does not merely confirm the same implementation against itself.

## 19. Harness behavior

No new normal CLI/debug command is required to print Character context.

Existing Harness validation output remains unchanged so context details are not casually emitted.

Core tests exercise exact composition/hash behavior. Existing Missing Raft and smoke runtime validations remain regression gates.

## 20. E0-D ablation boundaries

### Relationships omitted

Later E0-D relationship omission operates only on already-permitted material as a separately labeled experimental composition path. It is **not** a mutable boolean inside `full-authorized.v1`.

### Omniscient context

Later E0-D omniscient context remains outside safe Access Control and outside the reference Composer. No permissive/debug/omniscient flag enters `DeterministicContextComposer`.

Patch 0005 implements only the safe E0 reference contract.

## 21. Security and authority properties

- Composer receives only Character-safe Access output.
- It cannot recover data stripped by Access Control.
- It never receives denied access decisions or fixture provenance.
- It never parses story prose to infer access or truth.
- It never collapses Knowledge/Belief/Suspicion/Memory/Observation into “facts.”
- Opportunity adds salience, not knowledge authority.
- provider-neutral text omits internal IDs/provenance.
- recent fictional performance remains a separate future authority layer.
- packet/trace constructors are non-public.
- hashes establish deterministic local identity/integrity only, not signatures or authorization.
- technical failure never becomes fiction.

## 22. ARM64 and battery suitability

Small deterministic in-memory CPU work only: linear traversal, ordinal sorting/canonicalization, and two SHA-256 operations over already-bounded E0 context.

No network I/O, background polling, AI inference, GPU, or NPU work.

This is intentional: disclosure authority and content identity are cheaper and safer as deterministic Core logic. No NPU execution/performance claim is made.

## 23. Explicit exclusions

Patch 0005 does not implement:

- semantic/probabilistic relevance ranking;
- embeddings/vector search;
- token budgeting/truncation;
- summarization/paraphrase/deduplication;
- final provider system prompt/request construction;
- provider/model adapters or calls;
- provider-request hash;
- Director/opportunity selection;
- accepted Take/history projection;
- recent-performance DTO/non-empty recent-performance path;
- context/cost optimization;
- E0-D omniscient or relationship-omission execution;
- Integrity Validator;
- State Interpreter / State Authority;
- ProductionState / StateHash;
- causal commit/persistence/recovery replay;
- observation engine;
- WinUI, Windows AI/NPU, packaging, WACK, or Store work.

## 24. Exit gate

Before promotion:

1. blueprint explicitly approved as canonical Patch 0005 specification;
2. implementation starts from then-current `main` on dedicated branch;
3. public Composer input is only CharacterAccessProjection + opportunity CharacterId;
4. full-authorized inclusion matrix passes adversarial review;
5. authority categories remain separate;
6. packet/render/trace carry no denied IDs or fixture provenance;
7. exact structured canonical property/order rules are frozen/tested;
8. shared canonical JSON extraction leaves ECJ-1 byte-identical;
9. dual hash semantics and separate RenderingContract are deterministic;
10. ContextPacketId exact content-addressing rule passes;
11. exact rendered layer/heading/newline/bullet contract passes;
12. `recentPerformances` remains exact empty array with no speculative executable DTO/path;
13. Missing Raft exact Voss packet and Marlowe/Wren generic-boundary tests pass;
14. source-order invariance and repeated-byte identity pass;
15. packet authority cannot be publicly forged;
16. expected Voss structured/rendered hashes are independently derived and frozen;
17. native Windows ARM64 Core/Harness build passes warnings-as-errors;
18. full Core suite passes on target machine;
19. Missing Raft and smoke Harness regressions pass/0;
20. final hygiene review finds no provider abstraction, relevance model, ACL/omniscient bypass, persistence, or later-patch scope creep;
21. validation evidence distinguishes machine-tested executable head from documentation-only closure commits.

## 25. Material approval decisions

Explicit approval freezes:

1. Patch 0005 is the deterministic Context Composer boundary immediately after validated Access Control;
2. E0 reference composition is lossless/full-authorized, not semantic relevance filtering;
3. Composer input is only CharacterAccessProjection + CurrentOpportunityCharacterId;
4. opportunity must equal subject and grants no knowledge;
5. packet schema preserves authority categories separately;
6. `recentPerformances` is reserved as exact empty schema-v1 array, with item type/population deferred to accepted-history authority;
7. provider-neutral rendering separates trusted state, recent performance, and opportunity layers;
8. `RenderingContract = ensemble.e0.context.render.v1` versions exact rendered-byte behavior;
9. final system/provider-request construction remains outside Composer;
10. StructuredContextHash and RenderedContextHash are separate SHA-256 identities;
11. structured hash uses explicit canonical JSON with frozen order and ECJ-1 byte discipline;
12. canonical JSON string/UTF-8 emission is shared rather than duplicated, with exact ECJ-1 regression required;
13. ContextPacketId is `CTX:<StructuredContextHash>`;
14. packet identity is semantic context identity, not fixture/run provenance or authorization;
15. local composition trace remains separate from Performer-facing packet;
16. no relevance inference, token optimization, provider calls, Director logic, persistence, recent-history implementation, or E0-D bypass enters Patch 0005.

Implementation must not begin until these decisions are approved.
