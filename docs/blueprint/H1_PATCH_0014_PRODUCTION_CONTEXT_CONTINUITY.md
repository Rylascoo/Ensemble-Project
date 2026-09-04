# H1 Patch 0014 — E0 Production Context Continuity

Status: blueprint proposal 0.5 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `e06668a2307433bf99b0501dc38a701db392c633`
Parent promoted implementation: H1 Patch 0013 squash merge `15b85a25fa7969d6db69030fa712eea329471e6b`
Parent full-Core-test authority: H1 Patch 0013 at `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`
Parent native Core/Harness build + fixture authority: H1 Patch 0013 at `382e11f9fbe6774806152fad75b6a23cc8733187`
Branch: `h1-patch-0014-production-context-continuity-blueprint`

## 1. Purpose

Patch 0014 defines the smallest deterministic bridge from authoritative current ProductionState into the next Character-safe ContextPacket.

The completed H1 deterministic spine already reaches:

```text
source ProductionState with Current Opportunity
    -> historical fixture-backed Access / Context / Performer / Integrity / Interpretation / State Authority
        -> immutable Accepted Take
            -> Patch 0012 atomic causal commit
                -> no-opportunity postcommit ProductionState
                    -> Patch 0013 effective-opportunity transition
                        -> current ProductionState with next Current Opportunity
```

The remaining source-context seam is:

```text
current ProductionState with Current Opportunity
    -> exact pre-pipeline ProductionStateCheckpoint
        -> Production-backed deterministic Access Control
            -> Production-bound deterministic Context Composer
                -> exact state-bound Character ContextPacket
```

Patch 0014 closes only this seam.

It deliberately does **not** populate accepted recent Performance history because frozen Blueprint 0.1 reserves Observation eligibility and explicitly forbids assuming that co-presence means everyone sees or hears everything.

## 2. Frozen authority preserved

Patch 0014 preserves these already-approved laws:

- ProductionState is authoritative current projection after genesis;
- ValidatedFixture remains immutable genesis input and is never evolved in place;
- deterministic Access Control precedes Context composition;
- prohibited information is removed before any relevance/composition stage can receive it;
- Access returns the maximal permitted set; relevance, token budgeting, summarization, and final prompt construction remain later concerns;
- Character-facing projections strip hidden provenance and creator-only authority metadata;
- objective truth, observation, claim, belief, memory, knowledge, and recent Performance remain distinct authority categories;
- StateHash is the history-sensitive identity of the exact Production source state;
- ProductionStateCheckpoint is captured before Access/Context/Performance;
- Current Opportunity must exist before a Performer-source checkpoint is legal;
- a ContextPacket must not silently rebase onto another Production state;
- deterministic authority, not a model, owns information disclosure and source association;
- recent fictional Performance remains a separate untrusted creative-content layer;
- E0 co-presence does not itself establish observation eligibility.

## 3. Why the previous recent-Performance proposal was rejected

Earlier Patch 0014 proposals attempted to populate the immediately preceding Accepted Performance for the newly selected Character.

Recursive audit rejected that rule because frozen Blueprint 0.1 states:

```text
E0 keeps all three Characters co-present to avoid premature spatial/channel complexity,
but the ontology must not assume everyone sees or hears everything.
```

Patch 0006 also freezes that `AddressedCharacterIds` and `NominatedCharacterId` are not observation eligibility.

Current Core has no authoritative observation-eligibility producer for post-genesis Performance events. Therefore Patch 0014 must not manufacture one merely to fill `recentPerformances`.

The correct sequence is:

```text
Patch 0014
    Production -> Access -> state-bound Context continuity

later separately approved boundary
    observation/recent-Performance eligibility + disclosure
```

That later boundary may remain E0-minimal, but it requires its own authority design.

## 4. Scope boundary

Patch 0014 defines only:

- Production-backed Character Access evaluation;
- lifecycle-aware projection of current effective Production records;
- explicit CharacterClaim disclosure semantics;
- exact source StateHash identity on Production-backed Access projection and ContextPacket;
- Production-bound Context schema/rendering contract v2 while preserving Patch 0005 v1 bytes;
- one narrow public Production-context continuity entry point using ProductionStateCheckpoint;
- exact v2 source-StateHash association in E0TakeStateBinding;
- safe historical v1 binding compatibility only after exact-genesis + exact Production-derived v1 recomposition proof;
- strict v1/v2 anti-downgrade and hybrid-shape rejection;
- fixed independent v2 Context canonical/hash oracles;
- deterministic/fail-closed regression coverage.

Patch 0014 does not define or implement:

- recent Performance population;
- observation eligibility or CharacterObservation generation;
- complete Scene-loop orchestration;
- provider/model invocation;
- retry/cancellation/streaming/spend policy;
- full multi-turn replay from genesis;
- persistence/event store/recovery;
- arbitrary history retrieval;
- semantic search/relevance ranking;
- token budgeting/summarization/compaction;
- active-record indexes or performance caches;
- World Resolver;
- broad spatial/hearing/channel rules;
- perspective UX;
- branch/canon/retcon/rehearsal;
- final Production/Studio ontology;
- WinUI, Windows AI/NPU, MSIX, WACK, or Store certification.

## 5. Dependency direction

Lower deterministic dependencies remain:

```text
Domain / Fixture / internal Provenance
    -> Production
        -> Access
            -> Context
                -> Performer / Integrity / Interpreter / State Authority / Take
                    -> CausalCommit
                        -> Opportunity
```

Patch 0014 adds:

```text
ProductionStateCheckpoint + Access + Context
    -> Continuity
```

`ProductionStateCheckpoint` is historically owned by CausalCommit, so Continuity references that type. CausalCommit does not depend on Continuity.

Access must not depend on Context, CausalCommit, Opportunity, or Continuity.

Context may depend on Access/Production types needed for the state-bound packet.

CausalCommit may additionally depend directly on lower Access only for the legacy-v1 compatibility proof in this patch. That direction creates no cycle.

Production remains independent of Access/Context/Continuity.

Opportunity remains unchanged.

## 6. Public Continuity surface

New namespace:

```text
Ensemble.E0.Core.Continuity
```

Approved public types:

```text
public static class E0ProductionContextContinuity
public sealed class E0ContextContinuityException : Exception
```

Sole public method:

```text
public static ContextCompositionEvaluation Compose(
    ProductionStateCheckpoint sourceCheckpoint)
```

There is no separate Genesis/NextTurn API because Context composition depends on authoritative **current state**, not on how that state was historically reached.

There is no CausalCommit, Opportunity, Director, recent-history, provider, or arbitrary caller-supplied context-content input.

## 7. Checkpoint-first law

Correct orchestration is:

```text
var checkpoint = ProductionStateCheckpoint.Capture(currentState);
var context = E0ProductionContextContinuity.Compose(checkpoint);
```

Checkpoint capture remains O(1), retains the exact immutable source-state reference internally, exposes exact StateHash/SceneId/non-null CurrentOpportunity, cannot be rebound, and performs no full-state rehash.

Patch 0014 reuses checkpoint.StateHash directly.

## 8. Production-backed Access overload

Historical fixture API remains:

```text
CharacterBoundedAccessControl.Evaluate(
    ValidatedFixture fixture,
    CharacterId subjectCharacterId)
```

Patch 0014 adds:

```text
CharacterBoundedAccessControl.Evaluate(
    ProductionState sourceState,
    CharacterId subjectCharacterId)
```

The Production overload is deterministic, synchronous, side-effect free, and performs no Context/CausalCommit/Opportunity/Continuity/provider/model/clock/random/network/GPU/NPU work.

It accepts only exact supported Production contract:

```text
ensemble.e0.production-state.v1
```

## 9. CharacterAccessProjection evolution

The Character-safe projection remains the only trusted state-information input to Context Composer.

Patch 0014 adds:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<PermittedRecord>
```

Exact shape:

```text
fixture-backed historical projection:
    SourceStateHash = null
    Claims = []

Production-backed projection:
    SourceStateHash = sourceState.StateHash
    Claims = active permitted subject-owned CharacterClaim records
```

No raw ProductionRecord, lifecycle, protection, provenance, origin fixture metadata, effective CommitId/TakeId cache, or denied audit data enters the projection.

## 10. Production Access policy

Production Access evaluates every retained record for a local deterministic AccessDecision. Only Active permitted records enter Character-facing content.

### Active global domains

```text
HistoricalTruth          -> Deny / ProductionAuthorityExcluded
UnresolvedProposition    -> Deny / ProductionAuthorityExcluded
WorldState               -> Deny / ProductionAuthorityExcluded
SceneState               -> Permit / SharedSceneState
Pressure                 -> Permit / PublicPressure
```

### Active Character-owned domains

For the Access subject:

```text
CharacterConstitution
CharacterDisposition
CharacterCircumstance
CharacterObservation
CharacterKnowledge
CharacterBelief
CharacterSuspicion
CharacterMemory
CharacterGoal
CharacterClaim
    -> Permit / OwnedBySubject
```

The same domains owned by another Character are:

```text
Deny / OwnedByOtherCharacterExcluded
```

### Relationships

```text
active Relationship with subject == Access subject
    -> Permit / OwnedBySubject

other Character's Relationship
    -> Deny / OwnedByOtherCharacterExcluded
```

### Inactive retained records

Any inactive record is:

```text
Deny / InactiveRecordExcluded
```

Patch 0014 appends `InactiveRecordExcluded` to AccessReason, preserving existing enum values.

Lifecycle exclusion takes precedence because inactive semantic records are no longer current effective state.

## 11. Production Access structural invariants

Production Access fails closed unless:

- state is non-null and exact supported Production contract;
- StateHash/SceneId are initialized;
- roster contains exactly three unique initialized E0 Characters in ordinal canonical order;
- subject resolves exactly once in Production Characters and roster;
- retained RecordIds are unique;
- every record identity/text is valid and canonical;
- runtime ProductionRecord subtype agrees with domain;
- Character-owned record subjects belong to roster;
- Relationship subject/target are distinct valid roster Characters;
- lifecycle/protection enum values are defined;
- records are in canonical ordinal RecordId order;
- no unsupported/Unspecified domain is accepted.

Inactive retained records are structurally validated too; lifecycle does not excuse malformed retained history.

## 12. CharacterClaim disclosure law

A Production CharacterClaim is an accepted interpreted proposition attributable to its source Character.

It is not objective truth, Knowledge, Belief, Memory, or another Character's Observation.

Patch 0014 permits only the Access subject's own active CharacterClaim records into the distinct `Claims` category.

Other Characters' durable claims are not automatically disclosed as trusted state merely because Production remembers them.

If another Character should later know/remember/believe a claim, that requires separately authoritative semantic state in the appropriate category.

This preserves:

```text
Production remembers Character A claimed P
!=
Character B knows/believes/remembers P
```

## 13. Observation boundary remains closed

Patch 0014 does not infer observation from:

- co-presence;
- Candidate VisibleText;
- AddressedCharacterIds;
- NominatedCharacterId;
- Director selection;
- relationship state;
- SceneState;
- causal commit adjacency.

Therefore Production-bound Context v2 keeps the reserved recent Performance layer empty:

```text
recentPerformances = []
RecentPerformanceText = ""
```

This is intentional fail-closed behavior, not missing implementation accidentally described as complete.

## 14. Context v1 remains byte-frozen

Exact existing constants remain:

```text
E0ContextContracts.SchemaVersion
= "ensemble.e0.context.v1"

E0ContextContracts.CompositionContract
= "ensemble.e0.context.full-authorized.v1"

E0ContextContracts.RenderingContract
= "ensemble.e0.context.render.v1"
```

Existing Patch 0005 structured bytes, rendered bytes, byte lengths, StructuredContextHash, RenderedContextHash, ContextPacketId, and independent fixed oracles remain unchanged.

Existing v1 canonical structured JSON still contains exactly:

```text
"recentPerformances":[]
```

and no sourceStateHash or claims field.

## 15. Production-bound Context v2 contracts

Patch 0014 adds:

```text
E0ContextContracts.ProductionBoundSchemaVersion
= "ensemble.e0.context.v2"

E0ContextContracts.ProductionBoundCompositionContract
= "ensemble.e0.context.production-bound.v1"

E0ContextContracts.ProductionBoundRenderingContract
= "ensemble.e0.context.render.v2"
```

A new schema is required because SourceStateHash and Claims add semantic fields not present in frozen v1 bytes.

## 16. ContextPacket model evolution

The existing closed ContextPacket gains only:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<ContextRecord>
```

No `ContextRecentPerformance` public type is added in Patch 0014.

The reserved recentPerformances array remains a canonical empty field, not yet an object-model authority surface.

ContextCompositionTrace gains only:

```text
SourceStateHash : StateHash?
```

No recent Take/history IDs are added.

All additions remain read-only with no public constructors/setters.

## 17. Closed v1/v2 shape invariants

Hybrid shapes are invalid.

### v1

A v1 packet is valid only when:

```text
SchemaVersion == ensemble.e0.context.v1
CompositionContract == ensemble.e0.context.full-authorized.v1
Rendered.RenderingContract == ensemble.e0.context.render.v1
SourceStateHash == null
Claims.Length == 0
RecentPerformanceText == ""
```

The historical public v1 composer also requires its CharacterAccessProjection to have:

```text
SourceStateHash == null
Claims.Length == 0
```

### v2

A v2 packet is valid only when:

```text
SchemaVersion == ensemble.e0.context.v2
CompositionContract == ensemble.e0.context.production-bound.v1
Rendered.RenderingContract == ensemble.e0.context.render.v2
SourceStateHash.HasValue
SourceStateHash.Value initialized
Claims initialized
RecentPerformanceText == ""
```

Both canonical structured versions require exact empty:

```text
"recentPerformances":[]
```

No unsupported schema/composition/rendering combination is canonicalizable.

## 18. Canonicalizer dispatch law

`ContextPacketCanonicalizer.SerializeStructured(ContextPacket)` validates the full version/shape before serializing:

```text
valid v1 -> exact historical v1 bytes
valid v2 -> exact production-bound v2 bytes
hybrid/unsupported -> fail closed
```

It never ignores non-empty Claims, infers schema from nullable fields, or serializes v2 content under v1 tokens.

`SerializeRendered(RenderedContext)` accepts only the exact supported rendering contract shape and preserves v1 bytes unchanged.

## 19. Exact v2 structured canonical order

Root property order is exactly:

```text
schemaVersion
compositionContract
sourceStateHash
sceneId
subjectCharacterId
opportunityCharacterId
roster
sceneState
pressures
constitution
disposition
circumstance
observations
knowledge
beliefs
suspicions
memories
goals
claims
relationships
recentPerformances
```

Conceptual exact shape:

```json
{
  "schemaVersion":"ensemble.e0.context.v2",
  "compositionContract":"ensemble.e0.context.production-bound.v1",
  "sourceStateHash":"<64-lower-hex>",
  "sceneId":"...",
  "subjectCharacterId":"...",
  "opportunityCharacterId":"...",
  "roster":[{"characterId":"...","displayName":"..."}],
  "sceneState":[{"recordId":"...","text":"..."}],
  "pressures":[{"recordId":"...","text":"..."}],
  "constitution":[{"recordId":"...","text":"..."}],
  "disposition":[{"recordId":"...","text":"..."}],
  "circumstance":[{"recordId":"...","text":"..."}],
  "observations":[{"recordId":"...","text":"..."}],
  "knowledge":[{"recordId":"...","text":"..."}],
  "beliefs":[{"recordId":"...","text":"..."}],
  "suspicions":[{"recordId":"...","text":"..."}],
  "memories":[{"recordId":"...","text":"..."}],
  "goals":[{"recordId":"...","text":"..."}],
  "claims":[{"recordId":"...","text":"..."}],
  "relationships":[{"recordId":"...","targetCharacterId":"...","text":"..."}],
  "recentPerformances":[]
}
```

All set/projection arrays remain ordinally canonical by existing identity rules.

## 20. v2 Context identity

Production-bound identity is:

```text
StructuredContextHash = SHA256(canonical structured v2 bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Because SourceStateHash is inside canonical structured bytes, otherwise identical Character context derived from different causal Production states has different v2 ContextPacketId.

No Production rehash is required.

## 21. Production-bound rendering

The v1 renderer remains byte-frozen.

The v2 trusted-state renderer preserves existing section order and inserts exactly one distinct section after `[WHAT YOU REMEMBER]`:

```text
[WHAT YOU HAVE CLAIMED]
```

Claim text never renders under Knowledge, World State, Observation, Belief, or another upgraded category.

For v2 in Patch 0014:

```text
RecentPerformanceText = ""
OpportunityText = "You have the current opportunity to act."
```

No fake “nothing happened” text, speaker label, or other narration is invented.

## 22. ContextCompositionTrace v2

Trace adds only SourceStateHash.

Exact law:

```text
fixture/v1:
    SourceStateHash = null

Production/v2:
    SourceStateHash = sourceCheckpoint.StateHash
```

Claims are included in existing IncludedRecordIds.

No denied record IDs or Production provenance enter the Context trace.

## 23. Composer API / anti-forgery law

Existing public historical API remains exactly:

```text
DeterministicContextComposer.Compose(
    CharacterAccessProjection projection,
    CharacterId currentOpportunityCharacterId)
```

It remains v1-only and rejects a projection carrying SourceStateHash or Claims.

Patch 0014 adds only an internal production-bound composition path that accepts a Production-backed CharacterAccessProjection and current opportunity and always emits empty recentPerformances.

No public caller can request v2 by supplying arbitrary StateHash, Claims, or history.

## 24. Continuity composition law

`E0ProductionContextContinuity.Compose(checkpoint)` performs:

```text
checkpoint exact immutable source state
    -> validate supported current Production identity/CurrentOpportunity
        -> CharacterBoundedAccessControl.Evaluate(
               checkpoint.SourceState,
               checkpoint.CurrentOpportunityCharacterId)
            -> require Access SourceStateHash == checkpoint.StateHash
                -> internal v2 deterministic Context composition
                    -> require Packet/Trace SourceStateHash == checkpoint.StateHash
                        -> return ContextCompositionEvaluation
```

Continuity does not need source CausalCommit or Opportunity event inputs because the current ProductionState already is the authoritative projection used for context.

This removes duplicated historical authority from context composition.

## 25. E0TakeStateBinding v2 source-state law

For a production-bound v2 source Context, binding additionally requires exact:

```text
sourceContext.SourceStateHash.HasValue
sourceContext.SourceStateHash.Value == sourceCheckpoint.StateHash
sourceContext.SchemaVersion == ensemble.e0.context.v2
sourceContext.CompositionContract == ensemble.e0.context.production-bound.v1
sourceContext.Rendered.RenderingContract == ensemble.e0.context.render.v2
```

This is additive to the existing Scene/subject/opportunity/roster/StateAuthority-snapshot checks.

Because normal v2 construction is closed behind Continuity/internal Context composition, SourceStateHash is a trusted source-association field in normal Core flow.

## 26. Historical v1 compatibility requires exact recomposition

A v1 Context has no StateHash and can be publicly produced from any valid fixture. Exact-genesis proof alone is therefore insufficient.

E0TakeStateBinding may accept v1 only after both:

### A. exact genesis proof

Recompute the inherited genesis StateHash envelope over the checkpoint source Production projection and require exact equality with checkpoint.StateHash.

### B. exact Production-derived v1 recomposition

1. freshly evaluate Production Access for checkpoint Current Opportunity;
2. require Access SourceStateHash == checkpoint.StateHash;
3. require fresh Access Claims == [];
4. create a private compatibility projection from exactly that fresh permitted content with only Patch0014 additions removed:
   - SourceStateHash -> null;
   - Claims -> [];
5. pass the compatibility projection through the frozen public v1 DeterministicContextComposer;
6. compare expected and supplied v1 packet using exact canonical structured bytes, exact canonical rendered bytes, ContextPacketId, StructuredContextHash, and RenderedContextHash.

The compatibility adapter does not re-decide Access policy or hand-render Context. It reuses fresh Production Access and frozen v1 Context authority.

Exact rule:

```text
exact genesis + exact Production-recomposed v1 Context -> permitted
genesis + foreign/different v1 Context -> reject
evolved current state + any v1 Context -> reject
evolved current state + exact state-bound v2 Context -> required
```

This is an O(R + A) legacy-only path. Normal v2 binding uses O(1) StateHash equality for the new source-association check.

## 27. No Production mutation or history event

Patch 0014 adds no:

- Production mutation helper;
- Production StateHash envelope kind;
- causal history event;
- CommitId/TakeId semantics;
- Opportunity transition;
- Observation event.

Patch 0012 genesis/causalCommit and Patch 0013 opportunityTransition StateHash oracles remain unchanged.

## 28. Determinism

Identical authoritative current-state/checkpoint input produces byte-identical v2 Access/Context output across repeats and ordinary supported cultures.

No output depends on clock/date, randomness, process/machine identity, thread scheduling, dictionary insertion order, filesystem/network/provider/GPU/NPU state, or locale-sensitive ordering.

## 29. Failure atomicity

Patch 0014 mutates no external state.

Any checkpoint, Production, Access, canonicalization, version-shape, v1-equivalence, or Context failure returns no ContextPacket and changes no Production/history input.

No retry, alternate source, guessed repair, stale packet, observation guess, or evolved-state v1 downgrade occurs.

Public Continuity failures are sanitized as `E0ContextContinuityException`.

Direct Access/Context APIs retain existing exception domains.

## 30. Security / disclosure law

Patch 0014 preserves:

```text
system/application authority
!= trusted structured fictional state
!= accepted recent fictional Performance
!= future user/imported creative content
```

Specifically:

- Production HistoricalTruth/WorldState/Unresolved remain denied;
- other Characters' private state remains denied;
- inactive records remain denied;
- provenance/protection/lifecycle/origin/cache metadata never enters Character-facing projection;
- CharacterClaim remains explicitly a claim;
- co-presence/control/routing never silently becomes observation authority;
- recent Performance stays empty rather than becoming accidental omniscience;
- v1/v2 hybrid shapes cannot canonicalize by dropping authority-bearing fields.

## 31. Growth and memory law

Let:

```text
R = total retained Production records, including inactive history
A = active permitted records copied to this Character's projection
```

Production Access is O(R) because Production retains inactive records and Access audits every retained record.

Production-bound lossless Context is O(A).

A can grow because E0 includes Add-only domains such as Knowledge, Memory, and CharacterClaim.

Patch 0014 does not add relevance ranking, token budgeting, summarization, compaction, active-record indexing, semantic retrieval, or a performance cache to hide this fact.

These remain later measured design/optimization boundaries, consistent with frozen E0 exclusions.

## 32. ARM64 / battery implications

Patch 0014 uses bounded-by-current-state deterministic CPU work, not NPU inference, for Access filtering, structural validation, ordinal ordering, and hash association.

- checkpoint capture remains O(1);
- v2 source association is O(1);
- Access is O(R) only at explicit composition boundaries;
- Context copy/hash/render is O(A);
- no whole causal-event history is copied;
- no idle/background/network/provider/GPU/NPU work is introduced;
- legacy v1 exact recomposition occurs only on exact genesis compatibility.

This supports low idle battery impact but does not claim constant long-session per-turn cost. Retail performance/battery claims require later device profiling.

## 33. Genesis Production-Access equivalence

For an exact genesis ProductionState derived from a ValidatedFixture, Production Access must be semantically equivalent to historical fixture Access for:

- roster;
- SceneState;
- Pressure;
- Constitution;
- Disposition;
- Circumstance;
- Observation;
- Knowledge;
- Belief;
- Suspicion;
- Memory;
- Goal;
- Relationship.

Intentional Production additions are SourceStateHash, empty Claims, and lifecycle-aware Access decisions.

## 34. v1 regression law

Patch 0014 explicitly retains Patch 0005 exact v1:

- structured canonical bytes and byte length;
- rendered canonical bytes and byte length;
- StructuredContextHash;
- RenderedContextHash;
- ContextPacketId;
- exact empty recentPerformances;
- exact historical public Compose signature;
- no recent-Performance public type.

No v1 digest is updated merely because v2 exists.

## 35. Independent v2 reference oracles

Implementation must derive fixed v2 Context oracles independently from production Context canonicalizer code.

Required references:

### Genesis

```text
Missing Raft fixture
    -> exact genesis ProductionState
        -> checkpoint
            -> E0ProductionContextContinuity.Compose
```

### Evolved state

```text
Missing Raft genesis
    -> exact Patch 0012 Accepted causal commit
        -> exact Patch 0013 opportunity transition
            -> checkpoint result state
                -> E0ProductionContextContinuity.Compose
```

The independent derivation must first reproduce inherited fixture/v1 Context and Production/Patch0013 hashes before new v2 digests are accepted.

The evolved oracle's `recentPerformances` remains exactly empty.

## 36. Required Production Access tests

At minimum:

1. exact genesis Production Access equals fixture Access for inherited categories;
2. SourceStateHash exact;
3. unsupported Production contract fails;
4. active committed state changes follow policy;
5. inactive records absent from projection and explicitly denied;
6. subject-owned CharacterClaim appears only in Claims;
7. other Character's CharacterClaim denied;
8. claim absent from Knowledge/Belief/Memory/World/Scene categories;
9. subject-owned Relationship visible, other-subject Relationship denied;
10. HistoricalTruth/WorldState/Unresolved denied;
11. provenance/protection/lifecycle metadata stripped;
12. malformed roster/domain/subtype/relationship/record identity fails;
13. inactive malformed records still fail structural validation;
14. retained record and output ordering is ordinal;
15. no Access dependency on Context/CausalCommit/Opportunity/Continuity.

## 37. Required Context v2 tests

At minimum:

1. v2 SourceStateHash exact for genesis and evolved states;
2. Claims distinct and correctly rendered;
3. claim never upgraded into another epistemic/global category;
4. `recentPerformances` exact empty array;
5. RecentPerformanceText exact empty string;
6. OpportunityText unchanged;
7. exact v2 root property order;
8. ContextPacketId equals `CTX:` + StructuredContextHash;
9. duplicate record IDs across categories fail;
10. v2 repeat/culture determinism;
11. Trace SourceStateHash exact;
12. denied audit IDs/provenance absent.

## 38. Required version/downgrade tests

1. v1 projection with SourceStateHash fails public v1 Compose;
2. v1 projection with Claims fails public v1 Compose;
3. v1 packet with v2-only fields fails canonicalization;
4. v2 packet requires initialized SourceStateHash;
5. v2 requires exact schema/composition/rendering triple;
6. hybrid triples fail;
7. unsupported schema fails;
8. canonicalizer never drops Claims;
9. v1 canonical bytes/hashes stay exact;
10. v2 cannot carry non-empty RecentPerformanceText in Patch0014.

## 39. Required Continuity tests

1. exact genesis checkpoint composes v2;
2. exact Patch0013 evolved checkpoint composes v2 without source commit/opportunity arguments;
3. current Context subject/opportunity equals checkpoint CurrentOpportunity;
4. Access/Packet/Trace SourceStateHash all equal checkpoint.StateHash;
5. null/default/uninitialized checkpoint inputs fail;
6. no-opportunity state cannot become source checkpoint;
7. malformed current Production state fails closed through Access;
8. repeated Compose is byte-identical;
9. failure leaves checkpoint/source state unchanged;
10. Continuity exposes no recent-history/Observation/Director inputs.

## 40. Required Take-binding tests

1. exact v2 SourceStateHash binds;
2. foreign/uninitialized v2 SourceStateHash fails;
3. hybrid v2 contract fails;
4. evolved checkpoint + any v1 Context fails;
5. exact genesis + exact Production-recomposed v1 succeeds;
6. exact genesis + semantically different foreign v1 Context fails;
7. v1 compatibility expected packet is recomposed from fresh Production Access + frozen v1 Composer;
8. v1 structured-byte mismatch fails;
9. v1 rendered-byte mismatch fails;
10. existing Scene/subject/opportunity/roster/StateAuthority checks remain;
11. v2 StateHash check is additive, not replacement authority.

## 41. Required structural/reflection tests

At minimum:

- Continuity namespace exports only approved two types;
- E0ProductionContextContinuity exposes only one public Compose method;
- no mutable Continuity/session/history/Observation API;
- Production Access overload exact signature;
- CharacterAccessProjection additions exactly SourceStateHash + Claims;
- ContextPacket additions exactly SourceStateHash + Claims;
- ContextCompositionTrace addition exactly SourceStateHash;
- no ContextRecentPerformance public type;
- v1/v2 contract strings exact;
- historical public Context Compose signature remains exact and sole public composer;
- production-bound Context composer is internal;
- Access has no Context/CausalCommit/Opportunity/Continuity reference;
- Production has no Access/Context/Continuity reference;
- CausalCommit Access dependency is limited to legacy-v1 equivalence validation;
- Opportunity source unchanged;
- no Windows/network/random/time/provider/GPU/NPU public dependency.

## 42. Canonical preservation

Patch 0014 retains fixed inherited oracles including:

- Patch 0003 fixture hash;
- Patch 0005 v1 Context hashes;
- Patch 0012 genesis Production StateHash;
- Patch 0012 causal-commit StateHash;
- Patch 0013 opportunity-transition StateHash.

v2 Context identity is additive and rewrites none of them.

## 43. Simplicity guard

Patch 0014 does not introduce:

- event-store/session aggregate;
- repository/service locator;
- generic context-source interface hierarchy;
- background cache/index;
- vector/semantic search;
- model-assisted Access;
- Observation framework;
- recent Performance history type;
- provider routing;
- unnecessary async;
- second Production representation.

Correctness comes before speculative optimization.

## 44. Explicit nonclaims

After Patch 0014, Core can deterministically compose a Character-safe, exact-state-bound ContextPacket from any legal current Production source checkpoint.

It still cannot claim that the next Character has been told what the prior Character just did. That requires observation/recent-history authority not yet defined.

Patch 0014 therefore does not establish:

- complete behavioral Scene loop;
- complete multi-turn Character interaction quality;
- full replay;
- final Context relevance/token strategy;
- retail long-session performance/battery bounds;
- Observation semantics;
- provider/model execution;
- Windows AI/NPU execution;
- packaging/WACK/Store behavior.

## 45. Expected implementation surface

Likely modified:

```text
src/Ensemble.E0.Core/Access/CharacterAccessModels.cs
src/Ensemble.E0.Core/Access/CharacterBoundedAccessControl.cs
src/Ensemble.E0.Core/Context/ContextModels.cs
src/Ensemble.E0.Core/Context/ContextPacketCanonicalizer.cs
src/Ensemble.E0.Core/Context/DeterministicContextComposer.cs
src/Ensemble.E0.Core/CausalCommit/CausalCommitModels.cs
```

Likely added:

```text
src/Ensemble.E0.Core/Continuity/E0ProductionContextContinuity.cs
```

Plus focused Patch0014 tests/evidence.

ProductionState projection/canonicalizer, CausalCommit transition engine, Opportunity implementation, Performer, Integrity, Interpreter, and State Authority should not require semantic redesign.

## 46. Proposal evolution

### Proposal 0.2

- explicit history-adjacency proof for then-proposed recent Performance;
- closed v1/v2 hybrid-shape rules;
- Production contract/record validation.

### Proposal 0.3

- exact Production-derived legacy-v1 disclosure proof;
- silence rendering correction.

### Proposal 0.4

- legacy proof reuse through fresh Access + frozen v1 Composer;
- honest O(R)/O(A) growth accounting;
- relevance/token/index optimization explicitly deferred.

### Proposal 0.5

- removed automatic recent-Performance disclosure after frozen Observation audit showed co-presence/control cannot establish perception;
- removed ContextRecentPerformance and recent-Take trace surface;
- collapsed Continuity to one current-checkpoint `Compose` path with no CausalCommit/Opportunity input;
- retained `recentPerformances:[]` / empty RecentPerformanceText until a separately approved observation/recent-history authority exists.

Proposal 0.5 materially narrows scope and therefore restarts recursive audit from correctness.

## 47. Recursive audit order

```text
correctness
-> consistency
-> authority
-> disclosure/privacy
-> dependency direction
-> canonical/version compatibility
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64/battery suitability
-> project vision
-> evidence
```

Any material correction restarts the pass from correctness.

## 48. Approval gate

Implementation must not begin until one complete recursive pass finds zero material corrections or worthwhile improvements and the user explicitly approves.

Approval freezes:

- Patch0014 Production->Access->Context boundary;
- Production Access/lifecycle/CharacterClaim disclosure law;
- reserved Observation/recent-Performance boundary;
- single checkpoint-only Continuity API;
- v1 preservation/v2 versioning and shape rules;
- SourceStateHash propagation;
- exact legacy-v1 recomposition proof;
- canonical v2 packet/rendering behavior;
- growth/nonclaim boundaries;
- tests and non-goals.
