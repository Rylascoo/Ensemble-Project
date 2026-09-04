# H1 Patch 0014 — E0 Production Context Continuity

Status: blueprint proposal 0.8 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `e06668a2307433bf99b0501dc38a701db392c633`
Parent promoted implementation: H1 Patch 0013 squash merge `15b85a25fa7969d6db69030fa712eea329471e6b`
Parent full-Core-test authority: H1 Patch 0013 at `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`
Parent native Core/Harness build + fixture authority: H1 Patch 0013 at `382e11f9fbe6774806152fad75b6a23cc8733187`
Branch: `h1-patch-0014-production-context-continuity-blueprint`

## 1. Purpose

Patch 0014 defines the smallest deterministic bridge from authoritative current `ProductionState` into the next Character-safe `ContextPacket`.

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

It does **not** disclose post-genesis `CharacterClaim` history or recent accepted Performance because current E0 lacks the separate memory/observation authority needed to make those disclosures safe.

## 2. Frozen authority preserved

Patch 0014 preserves these approved laws:

- `ProductionState` is authoritative current projection after genesis;
- `ValidatedFixture` remains immutable genesis input and is never evolved in place;
- deterministic Access Control precedes Context composition;
- prohibited information is removed before any relevance/composition stage can receive it;
- Access returns the maximal **currently authorized** set; relevance, token budgeting, summarization, and final prompt construction remain later concerns;
- Character-facing projections strip provenance and creator-only authority metadata;
- objective truth, observation, claim, belief, memory, knowledge, and recent Performance remain distinct authority categories;
- `StateHash` is the history-sensitive identity of the exact Production source state;
- `ProductionStateCheckpoint` is captured before Access/Context/Performance;
- a current opportunity must exist before a Performer-source checkpoint is legal;
- Context may not silently rebase onto another Production state;
- deterministic authority, not a model, owns disclosure and source association;
- recent fictional Performance remains a separate untrusted creative-content layer;
- co-presence, social address, nomination, and Director selection do not establish observation eligibility;
- `CharacterClaim` is an interpreted claim proposition, not Memory or Performance-history replay;
- a source StateHash field alone is not accepted as proof that disclosed Context content was actually derived from that source state.

## 3. Why the patch narrowed during audit

Earlier proposals attempted to add both `CharacterClaim` disclosure and immediate recent-Performance disclosure.

Recursive audit rejected both automatic rules:

1. frozen Blueprint 0.1 says E0 co-presence must not imply that everyone sees or hears everything;
2. Patch 0006 explicitly says addressed/nominated control is not observation eligibility;
3. Patch 0009 makes `CharacterClaim` source-only and Add-only, but explicitly keeps Claim distinct from Memory and says it is not a Performance-history rewrite.

Because current Core has no observation-eligibility producer and no claim-to-current-recall authority, Patch 0014 fails closed instead of turning either retained history form into Character knowledge.

The correct architecture is therefore:

```text
Patch 0014
    current Production -> Access -> exact-state-bound Context

later separately approved authority
    observation / recent Performance disclosure
    and any creator-approved claim/social-history disclosure semantics
```

## 4. Scope boundary

Patch 0014 defines only:

- Production-backed Character Access evaluation;
- lifecycle-aware current-state projection;
- explicit deny/defer treatment for `CharacterClaim`;
- exact source `StateHash` identity on Production-backed Access projection and `ContextPacket`;
- Context structured schema/composition v2 while preserving Patch 0005 v1 bytes and v1 rendering;
- one narrow public Production-context continuity entry point using `ProductionStateCheckpoint`;
- exact fresh Production-derived Context equivalence proof in `E0TakeStateBinding` for v2;
- safe historical v1 binding compatibility only after exact-genesis + exact Production-derived v1 recomposition proof;
- strict v1/v2 anti-downgrade and hybrid-shape rejection;
- fixed independent v2 structured Context oracles;
- deterministic/fail-closed regression coverage.

Patch 0014 does not define or implement:

- `CharacterClaim` context disclosure;
- recent Performance population;
- observation eligibility or `CharacterObservation` generation;
- a new rendering contract;
- complete Scene-loop orchestration;
- provider/model invocation;
- retry/cancellation/streaming/spend policy;
- full multi-turn replay;
- persistence/event store/recovery;
- arbitrary history retrieval;
- relevance/token budgeting/summarization/compaction;
- active-record indexes or caches;
- World Resolver;
- broader spatial/hearing/channel semantics;
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

Patch 0014 adds one higher bridge:

```text
ProductionStateCheckpoint + Access + Context
    -> Continuity
```

Rules:

- Production remains independent of Access/Context/Continuity;
- Access may depend on Production but not Context/CausalCommit/Opportunity/Continuity;
- Context may depend on Access/Production types required for state-bound composition;
- Continuity references the historical CausalCommit-owned checkpoint type but CausalCommit does not depend on Continuity;
- CausalCommit may directly call lower Access + Context composition for exact source-context equivalence validation;
- CausalCommit must not call Continuity, avoiding `CausalCommit -> Continuity -> CausalCommit` conceptual recursion;
- Opportunity remains unchanged.

No dependency cycle is introduced.

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

There is no separate Genesis/NextTurn API because Context derives from authoritative **current state**, not from the historical route used to reach it.

There is no source CausalCommit, Opportunity event/result, Director trace, history object, recent Performance, provider, or arbitrary caller-supplied content input.

## 7. Checkpoint-first law

Correct orchestration is:

```text
var checkpoint = ProductionStateCheckpoint.Capture(currentState);
var context = E0ProductionContextContinuity.Compose(checkpoint);
```

Checkpoint capture remains O(1), retains the exact immutable source-state reference internally, exposes exact `StateHash`/`SceneId`/non-null CurrentOpportunity, cannot rebind, and performs no full-state rehash.

Patch 0014 reuses `checkpoint.StateHash` directly.

## 8. Production-backed Access overload

Historical fixture API remains unchanged:

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

It preserves the established `ensemble.e0.character-bounded.v1` privacy laws for all inherited categories; Patch 0014 does not create an alternate omniscient Access mode.

## 9. CharacterAccessProjection evolution

The Character-safe projection remains the only trusted state-information input to Context Composer.

Patch 0014 adds exactly one property:

```text
SourceStateHash : StateHash?
```

Exact shape:

```text
fixture-backed historical projection:
    SourceStateHash = null

Production-backed projection:
    SourceStateHash = sourceState.StateHash
```

`SourceStateHash` is **non-diegetic system association metadata**. It binds deterministic Core stages to one exact Production source, but it is not fictional knowledge and must never be rendered into `TrustedStateText`, `RecentPerformanceText`, `OpportunityText`, Character prose, or a future provider creative-context layer merely because it exists on the projection/packet.

No raw `ProductionRecord`, lifecycle, protection, provenance, origin fixture metadata, effective CommitId/TakeId cache, denied audit data, `CharacterClaim`, or recent history enters the projection.

## 10. Production Access policy

Production Access evaluates every retained record for a local deterministic `AccessDecision`. Only Active permitted records enter Character-facing content.

### Active global domains

```text
HistoricalTruth          -> Deny / ProductionAuthorityExcluded
UnresolvedProposition    -> Deny / ProductionAuthorityExcluded
WorldState               -> Deny / ProductionAuthorityExcluded
SceneState               -> Permit / SharedSceneState
Pressure                 -> Permit / PublicPressure
```

### Active inherited Character-owned domains

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
    -> Permit / OwnedBySubject
```

The same domains owned by another Character are:

```text
Deny / OwnedByOtherCharacterExcluded
```

### CharacterClaim

Any Active `CharacterClaim`, including one owned by the Access subject:

```text
Deny / CharacterClaimDisclosureDeferred
```

This is deliberate. The Production record establishes that a claim was authoritatively committed as a claim; it does not establish that the Character currently remembers the claim, that another Character perceived it, or that it should be surfaced as present-tense Character guidance.

### Relationships

```text
active Relationship with subject == Access subject
    -> Permit / OwnedBySubject

other Character's Relationship
    -> Deny / OwnedByOtherCharacterExcluded
```

### Inactive retained records

Any inactive record:

```text
Deny / InactiveRecordExcluded
```

Lifecycle exclusion takes precedence over domain/ownership reason because the record is no longer current effective state.

Patch 0014 appends two `AccessReason` values, preserving all existing integer positions:

```text
InactiveRecordExcluded
CharacterClaimDisclosureDeferred
```

## 11. Production Access structural invariants

Production Access fails closed unless:

- state is non-null and exact supported Production contract;
- `StateHash`/`SceneId` are initialized;
- `ProductionState.Characters` contains exactly three unique initialized Character IDs in ordinal canonical order;
- every Production Character display name satisfies the inherited canonical text discipline;
- roster contains exactly three unique initialized E0 Characters in ordinal canonical order;
- Production Character IDs and roster IDs are exactly the same set;
- subject resolves exactly once in Production Characters and roster;
- retained `RecordId`s are unique and in ordinal canonical order;
- every record identity/text is valid and canonical;
- runtime `ProductionRecord` subtype agrees with domain;
- Character-owned record subjects belong to roster;
- Relationship subject/target are distinct valid roster Characters;
- lifecycle/protection enum values are defined;
- no unsupported/Unspecified domain is accepted.

Inactive and denied `CharacterClaim` records are structurally validated too. Denial never turns malformed state into valid state.

Production Access must reuse existing canonical/domain helpers where practical rather than inventing a second incompatible text/identity law.

## 12. Observation and claim boundaries remain closed

Patch 0014 does not infer observation from:

- co-presence;
- Candidate `VisibleText`;
- `AddressedCharacterIds`;
- `NominatedCharacterId`;
- Director selection;
- relationship state;
- SceneState;
- causal-commit adjacency.

Patch 0014 does not infer current recall/knowledge from `CharacterClaim` storage.

Therefore both historical layers remain absent from Character-facing Context:

```text
CharacterClaim -> denied by Access
recentPerformances -> []
RecentPerformanceText -> ""
```

This is intentional fail-closed behavior.

## 13. Context v1 remains byte-frozen

Exact existing constants remain:

```text
E0ContextContracts.SchemaVersion
= "ensemble.e0.context.v1"

E0ContextContracts.CompositionContract
= "ensemble.e0.context.full-authorized.v1"

E0ContextContracts.RenderingContract
= "ensemble.e0.context.render.v1"
```

Existing Patch 0005 structured bytes, rendered bytes, byte lengths, hashes, `ContextPacketId`, and independent fixed oracles remain unchanged.

Existing v1 structured JSON retains exact root/property order and exact:

```text
"recentPerformances":[]
```

## 14. Production-bound structured Context v2

Patch 0014 adds only:

```text
E0ContextContracts.ProductionBoundSchemaVersion
= "ensemble.e0.context.v2"

E0ContextContracts.ProductionBoundCompositionContract
= "ensemble.e0.context.production-bound.v1"
```

It does **not** add a rendering-contract version because Patch 0014 does not change Character-visible rendering semantics.

Production v2 uses the existing exact:

```text
E0ContextContracts.RenderingContract
= "ensemble.e0.context.render.v1"
```

This keeps rendering authority versioned only when rendering actually changes.

## 15. ContextPacket / Trace model evolution

`ContextPacket` gains exactly:

```text
SourceStateHash : StateHash?
```

`ContextCompositionTrace` gains exactly:

```text
SourceStateHash : StateHash?
```

No Claims property.

No `ContextRecentPerformance` type.

No recent Take/history IDs.

All historical/new authority outputs remain read-only and non-publicly constructible.

## 16. Closed v1/v2 shape invariants

### v1

A v1 packet is valid only when:

```text
SchemaVersion == ensemble.e0.context.v1
CompositionContract == ensemble.e0.context.full-authorized.v1
Rendered.RenderingContract == ensemble.e0.context.render.v1
SourceStateHash == null
RecentPerformanceText == ""
```

Historical public v1 Compose requires:

```text
projection.SourceStateHash == null
```

### v2

A v2 packet is valid only when:

```text
SchemaVersion == ensemble.e0.context.v2
CompositionContract == ensemble.e0.context.production-bound.v1
Rendered.RenderingContract == ensemble.e0.context.render.v1
SourceStateHash.HasValue
SourceStateHash.Value initialized
RecentPerformanceText == ""
```

Both canonical structured versions require exact empty:

```text
"recentPerformances":[]
```

No unsupported schema/composition/rendering combination may canonicalize.

## 17. Canonicalizer dispatch law

`ContextPacketCanonicalizer.SerializeStructured(ContextPacket)` validates complete version/shape before serializing:

```text
valid v1 -> exact historical v1 bytes
valid v2 -> exact production-bound v2 bytes
hybrid/unsupported -> fail closed
```

It never infers schema from `SourceStateHash` alone, ignores a state hash under v1, or serializes v2 bytes under v1 tokens.

`SerializeRendered(RenderedContext)` does not require a new branch: Patch 0014 uses the exact existing `ensemble.e0.context.render.v1` contract and exact rendering envelope.

This keeps the rendering canonicalizer/source unchanged unless implementation needs only defensive shared validation that provably preserves bytes.

## 18. Exact v2 structured canonical order

Production v2 changes v1 structured bytes only by:

1. using v2 schema/composition tokens;
2. inserting `sourceStateHash` immediately after `compositionContract`.

Exact root order:

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
  "relationships":[{"recordId":"...","targetCharacterId":"...","text":"..."}],
  "recentPerformances":[]
}
```

All collection ordering remains existing ordinal canonical order.

## 19. v2 Context identity

```text
StructuredContextHash = SHA256(canonical structured v2 bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

`SourceStateHash` is part of canonical structured bytes, so otherwise identical Character-safe content from different Production history cannot share a v2 `ContextPacketId`.

No Production rehash is required for composition.

Important limitation:

```text
SourceStateHash equality
!= proof that all disclosed packet content was derived from that state
```

Exact source-content proof is therefore performed by fresh deterministic recomposition at effective Take-to-state binding.

## 20. Rendering preservation law

Patch 0014 adds no new trusted-state heading or recent text.

Production-backed rendering calls the same existing deterministic renderer over the same permitted category set.

Therefore:

- rendering contract remains `ensemble.e0.context.render.v1`;
- `recentPerformanceText` remains exactly empty;
- `OpportunityText` remains exactly `You have the current opportunity to act.`;
- `SourceStateHash` is never rendered;
- provider-neutral formatting remains unchanged.

Strong genesis invariant:

```text
exact genesis fixture/v1 Context
and exact genesis Production/v2 Context
for the same subject/opportunity

=> identical canonical rendered bytes
=> identical RenderedContextHash
```

Their structured bytes/`ContextPacketId`s differ because v2 contains source-state identity and new schema/composition tokens.

## 21. ContextCompositionTrace law

Exact trace behavior:

```text
fixture/v1:
    SourceStateHash = null

Production/v2:
    SourceStateHash = sourceCheckpoint.StateHash
```

All existing `IncludedRecordIds` and `IncludedRosterCharacterIds` semantics remain unchanged.

`CharacterClaim`/denied record IDs remain absent from Context trace because Context receives only `CharacterAccessProjection`, not Access decisions.

## 22. Composer API / anti-forgery law

Existing public API remains exactly:

```text
DeterministicContextComposer.Compose(
    CharacterAccessProjection projection,
    CharacterId currentOpportunityCharacterId)
```

It remains the v1 compatibility path and fails closed when `projection.SourceStateHash` is non-null.

Patch 0014 adds only an internal Production-bound composition path that requires a Production-backed projection with initialized `SourceStateHash` and emits v2 structured identity while reusing exact v1 rendering.

No public caller can request v2 by injecting an arbitrary `StateHash`.

Internal construction alone is not treated as sufficient source-derivation proof; later `E0TakeStateBinding` fresh-recomposes the expected packet from the exact checkpoint before accepting the Take/state association.

## 23. Continuity composition law

`E0ProductionContextContinuity.Compose(checkpoint)` performs:

```text
checkpoint exact immutable source state
    -> validate checkpoint/current Production identities
        -> Production Access for checkpoint CurrentOpportunity
            -> require projection.SourceStateHash == checkpoint.StateHash
                -> internal deterministic Context v2 composition
                    -> require Packet.SourceStateHash == checkpoint.StateHash
                    -> require Trace.SourceStateHash == checkpoint.StateHash
                        -> return ContextCompositionEvaluation
```

Continuity does not consume source CausalCommit/Opportunity history because current `ProductionState` is the authority required for current-state disclosure.

## 24. E0TakeStateBinding v2 exact source-content law

For a v2 source Context, binding first requires exact shape/identity:

```text
sourceContext.SourceStateHash.HasValue
sourceContext.SourceStateHash.Value == sourceCheckpoint.StateHash
sourceContext.SchemaVersion == ensemble.e0.context.v2
sourceContext.CompositionContract == ensemble.e0.context.production-bound.v1
sourceContext.Rendered.RenderingContract == ensemble.e0.context.render.v1
```

Those fields are necessary but not sufficient.

Binding must then prove exact Production derivation without trusting the packet's own StateHash declaration:

1. freshly evaluate Production Access from `sourceCheckpoint.SourceState` for `sourceCheckpoint.CurrentOpportunityCharacterId`;
2. require fresh projection `SourceStateHash == sourceCheckpoint.StateHash`;
3. pass that fresh projection through the internal deterministic production-bound Context composer;
4. compare expected and supplied v2 packet using exact canonical structured bytes, exact canonical rendered bytes, `ContextPacketId`, `StructuredContextHash`, and `RenderedContextHash`;
5. continue all existing Scene/subject/opportunity/roster/StateAuthority-snapshot checks.

Exact rule:

```text
matching SourceStateHash + mismatched disclosed content -> reject
matching content + foreign SourceStateHash -> reject
exact fresh Production-recomposed v2 Context -> permitted
```

This closes the Patch 0013-deferred proof that the accepted source Context's disclosed state is exactly what the source Production state authorizes.

It proves deterministic semantic/byte equivalence to exact-source recomposition. It does **not** authenticate historical object provenance or external provider-request provenance; those remain separate concerns.

CausalCommit performs this using lower Access + Context authorities directly and must not call Continuity.

## 25. Historical v1 compatibility requires exact recomposition

A v1 Context has no StateHash and can be publicly produced from any valid fixture. Exact-genesis proof alone is insufficient.

`E0TakeStateBinding` may accept v1 only after both:

### A. exact genesis proof

Recompute inherited genesis `StateHash` over checkpoint source Production projection and require exact checkpoint `StateHash` equality.

### B. exact Production-derived v1 recomposition

1. freshly evaluate Production Access for checkpoint CurrentOpportunity;
2. require fresh projection `SourceStateHash == checkpoint.StateHash`;
3. create a private compatibility projection with the **same permitted fields** and `SourceStateHash` removed to null;
4. pass that compatibility projection through frozen public v1 `DeterministicContextComposer`;
5. compare expected and supplied v1 packet using exact canonical structured bytes, exact canonical rendered bytes, `ContextPacketId`, `StructuredContextHash`, and `RenderedContextHash`.

No Access policy or Context rendering is duplicated manually.

Exact rule:

```text
exact genesis + exact Production-recomposed v1 Context -> permitted
genesis + different v1 Context -> reject
evolved state + any v1 Context -> reject
evolved state + exact state-bound v2 Context -> required
```

As with v2, byte equivalence proves that the supplied packet contains exactly the authorized disclosure for the source state; it does not prove which historical object originally produced those bytes.

## 26. No new Production/history authority

Patch 0014 adds no Production mutation helper, `StateHash` transition kind, causal history event, CommitId/TakeId semantics, Opportunity transition, Observation event, or claim-history event.

Inherited Patch 0012 and Patch 0013 `StateHash` oracles remain unchanged.

## 27. Determinism

Identical authoritative current-state/checkpoint input produces byte-identical Production Access/Context output across repeats and ordinary supported cultures.

No output depends on clock/date, randomness, machine/process identity, thread scheduling, dictionary insertion order, filesystem/network/provider/GPU/NPU state, or locale-sensitive ordering.

## 28. Failure atomicity

Patch 0014 mutates no external state.

Any checkpoint, Production, Access, canonicalization, version-shape, source-context equivalence, or Context failure returns no accepted binding/Context result and changes no Production/history input.

No retry, alternate source, guessed repair, stale packet, observation guess, claim disclosure fallback, or evolved-state v1 downgrade occurs.

Public Continuity failures are sanitized `E0ContextContinuityException`; direct lower APIs retain existing exception domains.

## 29. Security / disclosure law

Patch 0014 preserves:

```text
system/application authority
!= trusted structured fictional state
!= accepted recent fictional Performance
!= future user/imported creative content
```

Specifically:

- HistoricalTruth/WorldState/Unresolved remain denied;
- other Characters' private state remains denied;
- inactive records remain denied;
- `CharacterClaim` remains denied pending explicit disclosure authority;
- provenance/protection/lifecycle/origin/cache metadata never enters Character-facing projection;
- `SourceStateHash` is system association metadata only and never enters rendered Character text or a future creative provider frame by default;
- co-presence/control/routing never becomes observation authority;
- recent Performance remains empty;
- v1/v2 hybrid shape cannot canonicalize by dropping state identity;
- a matching state-hash field cannot authorize mismatched disclosed content.

## 30. Growth and memory law

Let:

```text
R = total retained Production records including inactive history
A = active permitted records copied to this Character's projection
```

Production Access is O(R) because it audits retained records.

Lossless Production-bound Context composition is O(A).

Effective v2 Take/state binding performs a fresh source-context equivalence proof and is therefore O(R + A), not O(1).

Legacy exact-genesis v1 binding also performs O(R + A) recomposition.

A can grow as Knowledge/Memory/etc. are added. `CharacterClaim` growth does not enlarge Context in Patch 0014 because claims are denied, but it still contributes to R.

Patch 0014 does not add relevance ranking, token budgeting, summarization, compaction, active-record indexing, semantic retrieval, attestation tokens, or cache layers merely to avoid deterministic recomposition.

## 31. ARM64 / battery implications

Patch 0014 uses deterministic CPU work—not NPU inference—for Access filtering, validation, ordinal ordering, canonicalization, hashing, and source-context equivalence proof.

- checkpoint capture O(1);
- Context composition O(R + A) including Access;
- v2 Take/state source-context proof O(R + A);
- no whole causal-event history copy;
- no idle/background/network/provider/GPU/NPU work;
- legacy v1 recomposition only for exact-genesis compatibility.

This supports low idle impact but is not a constant long-session turn-cost claim. If profiling later shows recomposition cost is material, any cache/attestation/index optimization must preserve the same exact authority law and receive separate design review.

Retail performance/battery claims require later device profiling.

## 32. Genesis equivalence laws

For exact genesis `ProductionState` derived from a `ValidatedFixture`:

### Access

Production Access must be semantically equivalent to fixture Access for all inherited permitted categories and roster.

Production-only differences:

```text
SourceStateHash = genesis StateHash
additional local AccessDecision coverage for lifecycle/CharacterClaim domain where applicable
```

Current E0 fixture genesis has no `CharacterClaim` records.

### Context rendering

For same subject/opportunity:

```text
fixture/v1 canonical rendered bytes
== Production/v2 canonical rendered bytes
```

and therefore `RenderedContextHash` is equal.

Structured bytes and `ContextPacketId` must differ because v2 adds state identity/versioning.

## 33. v1 regression law

Patch 0014 retains exact Patch 0005 v1:

- structured canonical bytes + byte length;
- rendered canonical bytes + byte length;
- `StructuredContextHash`;
- `RenderedContextHash`;
- `ContextPacketId`;
- exact empty `recentPerformances`;
- exact public Compose signature;
- exact `ensemble.e0.context.render.v1` rendering contract.

No v1 fixed digest is updated merely because v2 exists.

## 34. Independent v2 reference oracles

Implementation must derive fixed v2 structured Context oracles independently from production Context canonicalizer code.

Required references:

### Genesis

```text
Missing Raft fixture
    -> exact genesis ProductionState
        -> checkpoint
            -> E0ProductionContextContinuity.Compose
```

The independent derivation must first reproduce the exact inherited v1 structured/rendered hashes and genesis Production `StateHash`. It must prove the v2 genesis `RenderedContextHash` is identical to v1 before fixing the new v2 `StructuredContextHash`/`ContextPacketId`.

### Evolved state

```text
Missing Raft genesis
    -> exact Patch 0012 Accepted causal commit
        -> exact Patch 0013 opportunity transition
            -> current-state checkpoint
                -> E0ProductionContextContinuity.Compose
```

The derivation must reproduce inherited Patch0012/Patch0013 hashes before fixing evolved v2 Context identity.

`recentPerformances` remains exactly empty.

## 35. Required Production Access tests

At minimum:

1. genesis Production Access equals fixture Access for inherited permitted content;
2. `SourceStateHash` exact;
3. exact three Production Characters match exact three roster IDs;
4. Production Character display names preserve canonical text and malformed names fail closed;
5. active committed inherited-domain changes follow existing policy;
6. inactive records absent from projection and explicitly denied;
7. active subject-owned `CharacterClaim` denied with `CharacterClaimDisclosureDeferred`;
8. active other-owned `CharacterClaim` uses the same deferred claim reason;
9. inactive `CharacterClaim` uses `InactiveRecordExcluded` precedence;
10. no `CharacterClaim` text/ID enters projection;
11. subject-owned Relationship visible, other-subject Relationship denied;
12. HistoricalTruth/WorldState/Unresolved denied;
13. provenance/protection/lifecycle stripped;
14. malformed roster/Character-set/domain/subtype/relationship/record fails;
15. inactive/claim-denied malformed records still fail structural validation;
16. retained/output ordering ordinal;
17. supported Production contract constant remains exact and any future representable unsupported contract fails closed;
18. no Access dependency on Context/CausalCommit/Opportunity/Continuity.

## 36. Required Context v2 tests

At minimum:

1. `SourceStateHash` exact for genesis and evolved states;
2. `ContextPacket` has no Claims or recent-history public property/type;
3. exact `recentPerformances:[]`;
4. `RecentPerformanceText` exact empty;
5. rendering contract remains exact v1;
6. genesis v1/v2 rendered canonical bytes identical;
7. genesis v1/v2 `RenderedContextHash` identical;
8. genesis v1/v2 `StructuredContextHash`/`ContextPacketId` differ;
9. exact v2 root property order;
10. `ContextPacketId = "CTX:" + StructuredContextHash`;
11. duplicate IDs still fail existing Context validation;
12. v2 repeat/culture determinism;
13. Trace `SourceStateHash` exact;
14. denied claim/provenance/audit IDs absent;
15. `SourceStateHash` absent from all rendered layers.

## 37. Required version/downgrade tests

1. v1 public Compose rejects projection with `SourceStateHash`;
2. v1 packet with `SourceStateHash` fails v1 canonical shape;
3. v2 packet requires initialized `SourceStateHash`;
4. v2 requires exact schema/composition + existing render-v1 triple;
5. hybrid/unsupported triples fail;
6. unsupported schema fails;
7. v1 canonical bytes/hashes remain exact;
8. v2 cannot carry non-empty `RecentPerformanceText` in Patch0014;
9. no `ProductionBoundRenderingContract` constant is introduced.

## 38. Required Continuity tests

1. exact genesis checkpoint composes v2;
2. exact Patch0013 evolved checkpoint composes v2 without source commit/opportunity inputs;
3. Context subject/opportunity equals checkpoint CurrentOpportunity;
4. Access/Packet/Trace `SourceStateHash` all equal checkpoint.StateHash;
5. null/default/uninitialized checkpoint/source state fails;
6. no-opportunity state cannot be checkpointed;
7. malformed current Production fails through Access;
8. repeated Compose byte-identical;
9. failure leaves checkpoint/source state unchanged;
10. Continuity exposes no recent-history/Observation/Director inputs.

## 39. Required Take-binding tests

1. exact freshly recomposed v2 source Context binds;
2. foreign/uninitialized v2 `SourceStateHash` fails;
3. matching v2 `SourceStateHash` with mismatched disclosed record/text/roster/rendered bytes fails;
4. hybrid v2 contract fails;
5. evolved checkpoint + any v1 Context fails;
6. exact genesis + exact Production-recomposed v1 succeeds;
7. exact genesis + semantically different v1 fails;
8. v1 and v2 source proof reuse fresh Production Access + existing Context composer rather than duplicated policy/rendering;
9. structured-byte mismatch fails;
10. rendered-byte mismatch fails;
11. `ContextPacketId`/stored hash mismatch fails;
12. existing Scene/subject/opportunity/roster/StateAuthority checks remain;
13. state hash is additive association, not replacement for exact source-content proof;
14. source-content proof failure changes no Production/Take/history input.

## 40. Required structural/reflection tests

At minimum:

- Continuity namespace exports only approved two types;
- `E0ProductionContextContinuity` exposes only one public Compose;
- no Continuity history/Observation/Director inputs;
- Production Access overload exact signature;
- `AccessReason` appends only `InactiveRecordExcluded` and `CharacterClaimDisclosureDeferred`;
- `CharacterAccessProjection` adds exactly `SourceStateHash`;
- `ContextPacket` adds exactly `SourceStateHash`;
- `ContextCompositionTrace` adds exactly `SourceStateHash`;
- no Claims property;
- no `ContextRecentPerformance` type;
- v1 constants exact;
- v2 adds only schema/composition constants, no render-v2 constant;
- historical public Context Compose remains sole public composer;
- Production-bound Context composer internal;
- Access has no Context/CausalCommit/Opportunity/Continuity reference;
- Production has no Access/Context/Continuity reference;
- CausalCommit lower Access/Context dependency is limited to exact source-context equivalence validation;
- CausalCommit has no Continuity dependency;
- Opportunity source unchanged;
- no Windows/network/random/time/provider/GPU/NPU public dependency.

## 41. Canonical preservation

Patch 0014 retains fixed inherited oracles including:

- Patch 0003 fixture hash;
- Patch 0005 v1 Context structured/rendered hashes;
- Patch 0012 genesis Production `StateHash`;
- Patch 0012 causal-commit `StateHash`;
- Patch 0013 opportunity-transition `StateHash`.

v2 structured Context identity is additive and rewrites none of them.

## 42. Simplicity guard

Patch 0014 does not introduce:

- event-store/session aggregate;
- repository/service locator;
- generic context-source hierarchy;
- background cache/index;
- semantic/vector search;
- model-assisted Access;
- Observation framework;
- claim context category;
- recent Performance type;
- provider routing;
- source-attestation/capability-token object merely to avoid recomposition;
- unnecessary async;
- second Production representation;
- new rendering contract.

## 43. Explicit nonclaims

After Patch 0014, Core can deterministically compose an exact-state-bound Character-safe `ContextPacket` from any legal current Production checkpoint and can re-prove, at Take/state binding, that a supplied source Context is byte-equivalent to fresh exact-source recomposition.

It still cannot claim:

- historical object provenance for an otherwise byte-identical Context packet;
- external provider-request provenance;
- the next Character received what the prior Character just did;
- retained `CharacterClaim` history is available as current Character recall;
- a complete behavioral Scene loop exists;
- complete multi-turn Character interaction quality;
- full replay;
- final Context relevance/token strategy;
- retail long-session performance/battery bounds;
- Observation semantics;
- provider/model execution;
- Windows AI/NPU execution;
- packaging/WACK/Store behavior.

## 44. Expected implementation surface

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

`ContextPacketCanonicalizer.SerializeRendered` and the existing trusted-state renderer should preferably remain source-unchanged because rendering semantics do not change.

ProductionState projection/canonicalizer, CausalCommit transition engine, Opportunity, Performer, Integrity, Interpreter, and State Authority should not require semantic redesign.

## 45. Proposal evolution

### Proposal 0.2
- history-adjacency proof for then-proposed recent Performance;
- closed version-shape rules;
- Production structural validation.

### Proposal 0.3
- exact legacy-v1 Production-derived proof;
- silence correction.

### Proposal 0.4
- legacy proof reuse through Access + v1 Composer;
- honest O(R)/O(A) growth accounting.

### Proposal 0.5
- removed automatic recent-Performance disclosure after Observation audit;
- removed recent-history public surface and collapsed Continuity to current-checkpoint Compose.

### Proposal 0.6
- removed automatic `CharacterClaim` disclosure after epistemic audit showed Add-only Claim is not Memory/current recall authority;
- `CharacterClaim` now fails closed with explicit `CharacterClaimDisclosureDeferred` Access reason;
- Context/Access model additions reduced to `SourceStateHash` only;
- removed render-v2 proposal and preserved exact existing rendering contract/source because Character-visible rendering does not change;
- added genesis invariant that v1/v2 rendered bytes/hashes are identical while structured identities differ.

### Proposal 0.7
- closed the remaining Patch0013-deferred source-derivation proof;
- `SourceStateHash` equality is necessary but no longer treated as sufficient authority;
- v2 Take/state binding fresh-recomposes exact Production Access + production-bound Context and compares canonical structured/rendered bytes and identities;
- CausalCommit reuses lower Access + Context directly rather than depending on Continuity;
- corrected v2 binding complexity from O(1) to O(R + A);
- explicitly declined an attestation/capability-token shortcut until profiling proves recomposition cost warrants more machinery.

### Proposal 0.8
- tightened Production Access structural proof so the exact three Production Characters and roster must match as sets and Character display names remain canonically valid;
- classified `SourceStateHash` explicitly as non-diegetic system association metadata that must never enter rendered Character text or future creative provider framing by default;
- converted the currently non-representable unsupported Production contract test into a contract/future fail-closed requirement rather than pretending current sealed `ProductionState` can construct another contract version.

Proposal 0.8 materially hardens safe-projection boundaries and restarts recursive audit from correctness.

## 46. Recursive audit order

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

Any material correction restarts from correctness.

## 47. Approval gate

Implementation must not begin until one complete recursive pass finds zero material corrections or worthwhile improvements and the user explicitly approves.

Approval freezes:

- Patch0014 Production->Access->Context boundary;
- Production Access/lifecycle/`CharacterClaim`-deny law;
- exact Production Character/roster/display-identity validation;
- reserved Observation/recent-Performance boundary;
- single checkpoint-only Continuity API;
- `SourceStateHash`-only Access/Context/Trace evolution and non-diegetic metadata law;
- v1 preservation and structured v2 schema/composition versioning;
- existing v1 rendering preservation;
- exact Production-recomposed source-context proof for both v2 and legacy v1 binding;
- canonical v2 structured packet behavior;
- growth/nonclaim boundaries;
- tests and non-goals.
