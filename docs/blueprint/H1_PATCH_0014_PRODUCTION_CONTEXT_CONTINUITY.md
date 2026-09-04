# H1 Patch 0014 — E0 Production Context Continuity

Status: blueprint proposal 0.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `e06668a2307433bf99b0501dc38a701db392c633`
Parent promoted implementation: H1 Patch 0013 squash merge `15b85a25fa7969d6db69030fa712eea329471e6b`
Parent full-Core-test authority: H1 Patch 0013 at `a3fae23dc4df302e834b031ecfc848a3bb2d37fc`
Parent native Core/Harness build + fixture authority: H1 Patch 0013 at `382e11f9fbe6774806152fad75b6a23cc8733187`
Branch: `h1-patch-0014-production-context-continuity-blueprint`

## 1. Purpose

Patch 0014 defines the smallest deterministic continuity boundary needed after H1 Patch 0013 so a newly established Current Opportunity can receive a Character-safe ContextPacket derived from the exact current ProductionState rather than from the immutable genesis fixture.

The completed H1 spine now reaches:

```text
source ProductionState with Current Opportunity
    -> Access / Context / Performer / Integrity / Interpretation / State Authority
        -> immutable Accepted Take
            -> Patch 0012 atomic causal commit
                -> no-opportunity postcommit ProductionState
                    -> Patch 0013 postcommit Director recomputation
                        -> effective-opportunity ProductionState
```

The next missing boundary is:

```text
Patch 0013 effective-opportunity ProductionState
    -> exact source checkpoint
        -> Production-backed deterministic Access Control
            -> deterministic accepted-history disclosure
                -> Production-bound Context Composer
                    -> exact next-turn ContextPacket
```

Patch 0014 closes only that boundary.

It does not invoke a provider, generate a second Performance, orchestrate a full Scene loop, persist causal history, or claim full multi-turn replay from genesis.

## 2. Recovered frozen authority

This proposal preserves the following already-approved laws:

- ProductionState is the authoritative current projection after genesis.
- ValidatedFixture remains immutable genesis input and is never mutated into evolved state.
- deterministic Access Control always precedes Context composition.
- prohibited information must be removed before any relevance/composition stage can receive it.
- Character-facing projections remain stripped of hidden provenance and creator-only authority metadata.
- a claim is not objective truth, knowledge, belief, memory, or observation merely because it exists.
- accepted Performance history and semantic Production state are distinct authorities.
- recent fictional Performance is untrusted creative content and must remain separate from trusted structured state/system authority.
- `recentPerformances` was intentionally reserved by Patch 0005 for later accepted-history authority.
- ProductionStateCheckpoint must be captured before a Performer-source Access/Context pipeline begins.
- StateHash is the history-sensitive identity of the exact Production source state.
- Patch 0013 must establish Current Opportunity before any next Access/Context/Performer work begins.
- the next context must not silently rebase onto a different Production state.
- deterministic authority owns disclosure and association; a model does not.

## 3. Why this patch is next

Patch 0013 explicitly deferred this boundary because current Access/Context has four missing capabilities:

1. Access accepts `ValidatedFixture`, not evolved `ProductionState`;
2. `CharacterAccessProjection` / `ContextPacket` do not carry the exact source `StateHash`;
3. CharacterClaim has no Character-context category;
4. accepted recent Performance is reserved but never populated.

Patch 0013 also established the ordering constraint:

```text
Patch 0012 causal commit
    -> no-opportunity state
Patch 0013 opportunity transition
    -> effective next-opportunity state
Patch 0014 Production -> Access -> Context continuity
    -> legal next-turn source ContextPacket
```

A full Scene-loop orchestrator before this bridge would have to invent disclosure semantics, source-state binding, or recent-history policy inside orchestration code. That would place constitutional authority in the wrong layer.

## 4. Scope boundary

Patch 0014 defines only:

- Production-backed Character Access evaluation for the current immutable ProductionState;
- lifecycle-aware projection of active Production records;
- explicit CharacterClaim disclosure semantics;
- exact source StateHash identity on the Production-backed Access projection;
- a new Production-bound Context schema/contract while preserving Patch 0005 v1 bytes;
- one exact accepted recent Performance item type;
- deterministic E0 immediate-recent-Performance disclosure from the immediately preceding Accepted causal commit;
- a higher-level continuity bridge that validates Patch 0012 -> Patch 0013 -> current checkpoint adjacency before Context composition;
- exact SourceStateHash propagation into the resulting ContextPacket and trace;
- exact next-turn association checks in E0TakeStateBinding;
- genesis compatibility/migration rules for the existing fixture-based context path;
- fixed canonical byte/hash tests for the new Production-bound Context format;
- deterministic repeatability/fail-closed tests for stale, foreign, malformed, or downgraded sources.

Patch 0014 does not define or implement:

- complete Scene-loop orchestration;
- provider/model invocation;
- retry, cancellation, streaming, cost, or spend policy;
- full multi-turn replay from genesis through an event sequence;
- durable event store/database/recovery transaction;
- arbitrary history retrieval or semantic memory search;
- more than the immediate accepted recent Performance;
- Observation / World Resolver;
- spatial/hearing/channel visibility beyond the frozen E0 co-present trio rule;
- audience/creator/Character perspective UX;
- branch/canon/retcon/rehearsal/Alternate promotion;
- final Production/Studio ontology;
- WinUI;
- Windows AI Foundry or NPU/QNN execution;
- MSIX packaging;
- WACK;
- Microsoft Store certification.

## 5. Dependency direction

Patch 0012 explicitly allows a later Access integration patch to depend on Production without reversing Production's dependency direction.

The lower deterministic layers remain conceptually:

```text
Domain / Fixture / internal Provenance
    -> Production
        -> Access
            -> Context
                -> Performer / Integrity / Interpreter / State Authority / Take
                    -> CausalCommit
                        -> Opportunity
```

Patch 0014 must not make Access depend on CausalCommit or Opportunity.

That tempting design is rejected because CausalCommit already consumes Context. An Access -> CausalCommit dependency would create the conceptual cycle:

```text
Access -> CausalCommit -> Context -> Access
```

Instead Patch 0014 adds one higher integration layer:

```text
Access + Context + CausalCommit + Opportunity
    -> Continuity
```

No lower layer depends on Continuity.

Continuity may therefore validate accepted causal history and opportunity adjacency, derive a stripped permitted recent-Performance item, and invoke lower Access/Context contracts without creating a dependency cycle.

## 6. Public integration namespace

Proposed new namespace:

```text
Ensemble.E0.Core.Continuity
```

Approved public surface should be limited to:

```text
public static class E0ProductionContextContinuity

public sealed class E0ContextContinuityException : Exception
```

Proposed methods:

```text
public static ContextCompositionEvaluation ComposeGenesis(
    ProductionStateCheckpoint sourceCheckpoint)

public static ContextCompositionEvaluation ComposeNextTurn(
    ProductionStateCheckpoint sourceCheckpoint,
    E0CausalCommit sourceCommit,
    E0OpportunityTransitionResult opportunityResult)
```

No public constructor.

No mutable continuity/session object.

No generic arbitrary-history input.

No overload accepting caller-created recent Performance data.

## 7. Why the continuity API accepts a pre-pipeline checkpoint

Patch 0012 freezes that source checkpoint capture occurs before Access/Context/Performance.

Patch 0014 must preserve that law directly in its public entry points.

Correct orchestration shape:

```text
var checkpoint = ProductionStateCheckpoint.Capture(currentState);
var context = E0ProductionContextContinuity.Compose...(
    checkpoint,
    ...causal inputs...);
```

The checkpoint:

- is O(1);
- retains the exact immutable ProductionState reference internally;
- exposes exact StateHash, SceneId, and Current Opportunity;
- cannot be rebound to another state;
- performs no full-state rehash.

Patch 0014 must use the already-computed StateHash rather than rehashing Production merely to bind Context.

## 8. Production-backed Access overload

Patch 0014 extends the existing Access authority without removing its fixture path.

Existing historical API remains:

```text
CharacterBoundedAccessControl.Evaluate(
    ValidatedFixture fixture,
    CharacterId subjectCharacterId)
```

New API:

```text
CharacterBoundedAccessControl.Evaluate(
    ProductionState sourceState,
    CharacterId subjectCharacterId)
```

The Production overload is deterministic and side-effect free.

It must not call Context, CausalCommit, Opportunity, provider/model code, clock, randomness, network, GPU, or NPU work.

## 9. CharacterAccessProjection evolution

The existing Character-safe projection remains the only trusted state-information input to Context Composer.

Patch 0014 adds:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<PermittedRecord>
```

Semantics:

- fixture-backed Patch 0004 projection: `SourceStateHash = null`, `Claims = []`;
- Production-backed Patch 0014 projection: `SourceStateHash = sourceState.StateHash`, `Claims = permitted active subject-owned CharacterClaim records`.

No ProductionRecord lifecycle/protection/provenance object is exposed through the Character-facing projection.

No raw ProductionRecord instance is exposed.

## 10. Production record projection law

ProductionState may contain active and inactive records across all E0 domains.

The Production-backed Access path evaluates every retained Production record exactly once for audit disposition, but only Active permitted records enter the Character-facing projection.

### Global domains

Active records:

```text
HistoricalTruth          -> Deny / ProductionAuthorityExcluded
UnresolvedProposition    -> Deny / ProductionAuthorityExcluded
WorldState               -> Deny / ProductionAuthorityExcluded
SceneState               -> Permit / SharedSceneState
Pressure                 -> Permit / PublicPressure
```

### Character-owned domains

For the subject Character:

```text
CharacterConstitution    -> Permit / OwnedBySubject
CharacterDisposition     -> Permit / OwnedBySubject
CharacterCircumstance    -> Permit / OwnedBySubject
CharacterObservation     -> Permit / OwnedBySubject
CharacterKnowledge       -> Permit / OwnedBySubject
CharacterBelief          -> Permit / OwnedBySubject
CharacterSuspicion       -> Permit / OwnedBySubject
CharacterMemory          -> Permit / OwnedBySubject
CharacterGoal            -> Permit / OwnedBySubject
CharacterClaim           -> Permit / OwnedBySubject
```

The same domains owned by another Character:

```text
Deny / OwnedByOtherCharacterExcluded
```

### Relationships

Active Relationship records whose subject is the Access subject:

```text
Permit / OwnedBySubject
```

Other Characters' Relationship records:

```text
Deny / OwnedByOtherCharacterExcluded
```

### Inactive records

Any inactive record:

```text
Deny / InactiveRecordExcluded
```

Lifecycle exclusion takes precedence because an inactive semantic record is no longer part of current effective Character/world state.

`AccessReason.InactiveRecordExcluded` is added as a new enum value.

## 11. CharacterClaim disclosure law

CharacterClaim remains a distinct epistemic category.

A Production CharacterClaim means:

```text
"this Character has claimed proposition P"
```

It does not mean:

```text
P is objective truth
P is known by this Character
P is believed by this Character
P was observed by another Character
P is remembered by another Character
```

Therefore Patch 0014 permits only the subject Character's own active CharacterClaim records into that subject's trusted Character-state projection.

Other Characters' durable claims are not automatically inserted into trusted state merely because the Production remembers that they made them.

For the immediate previous accepted Performance, co-present Characters may receive the visible Performance separately through the recent-Performance layer defined below.

Longer-term knowledge/memory of another Character's claim requires the separately committed semantic state that actually represents that knowledge/memory. Patch 0014 does not synthesize it.

## 12. Why this claim rule is the least-authority rule

The E0 ontology intentionally distinguishes objective truth, observation, claim, belief, memory, and knowledge.

Automatically giving every Character every durable claim would silently collapse:

```text
Production remembers a claim
```

into:

```text
this Character currently receives that claim as trusted context
```

without an observation/memory/knowledge authority proving the disclosure.

The subject-owned rule preserves Character continuity while avoiding cross-Character omniscience.

The immediate recent-Performance rule supplies the one E0 co-present interaction that the next turn actually needs.

## 13. Existing Context v1 remains byte-frozen

Patch 0005 v1 remains historical executable authority.

Exact existing constants remain unchanged:

```text
E0ContextContracts.SchemaVersion
= "ensemble.e0.context.v1"

E0ContextContracts.CompositionContract
= "ensemble.e0.context.full-authorized.v1"

E0ContextContracts.RenderingContract
= "ensemble.e0.context.render.v1"
```

Existing v1 canonical structured bytes remain unchanged.

Existing v1 canonical rendered bytes remain unchanged.

Existing v1 fixed ContextPacketId / StructuredContextHash / RenderedContextHash oracles remain unchanged.

Existing v1 `recentPerformances` remains exactly `[]`.

Patch 0014 must not retrofit SourceStateHash or Claims into the frozen v1 byte shape.

## 14. Production-bound Context v2

Patch 0014 adds a distinct Production-bound Context contract:

```text
E0ContextContracts.ProductionBoundSchemaVersion
= "ensemble.e0.context.v2"

E0ContextContracts.ProductionBoundCompositionContract
= "ensemble.e0.context.production-bound.v1"

E0ContextContracts.ProductionBoundRenderingContract
= "ensemble.e0.context.render.v2"
```

The new version is required because SourceStateHash and Claims add semantic fields not present in the frozen v1 byte contract.

This avoids pretending a byte-incompatible packet is still the same schema.

## 15. ContextPacket v2 public shape

Patch 0014 extends `ContextPacket` with:

```text
SourceStateHash : StateHash?
Claims : ImmutableArray<ContextRecord>
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

New public immutable item:

```text
public sealed class ContextRecentPerformance
- TakeId
- SubjectCharacterId
- VisibleText
```

Constructor remains internal.

No provider/model identity.

No raw provider response.

No Candidate control payload.

No Director trace/rule.

No Integrity/Interpreter/Authority trace.

No Production provenance graph.

No secret/credential data.

## 16. Why recent Performance does not include Candidate control

Candidate addressed/nominated control is deterministic routing input and is already causally bound through the Accepted Take and Patch 0013 opportunity transition.

The next Performer needs the fictional visible action that just occurred, not hidden routing machinery.

Exposing nomination/address metadata as Character-facing recent Performance could make internal control semantics look like diegetic knowledge even where the prose itself did not establish that meaning.

Therefore Patch 0014 recent Performance includes only:

- Accepted Take identity for audit/provenance correlation;
- source Character identity;
- exact accepted visible Performance text.

## 17. Exact v2 structured canonical order

The Production-bound canonical structured packet uses this root property order:

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

Exact conceptual JSON shape:

```json
{
  "schemaVersion":"ensemble.e0.context.v2",
  "compositionContract":"ensemble.e0.context.production-bound.v1",
  "sourceStateHash":"<64-lower-hex>",
  "sceneId":"...",
  "subjectCharacterId":"...",
  "opportunityCharacterId":"...",
  "roster":[
    {"characterId":"...","displayName":"..."}
  ],
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
  "relationships":[
    {"recordId":"...","targetCharacterId":"...","text":"..."}
  ],
  "recentPerformances":[
    {"takeId":"...","subjectCharacterId":"...","visibleText":"..."}
  ]
}
```

Arrays retain ordinal canonical ordering rules where they are sets/projections.

`recentPerformances` is chronological, not set-like. Patch 0014 permits only:

- zero items for exact genesis composition;
- exactly one item for `ComposeNextTurn`.

No sorter may reorder chronological recent Performance items.

## 18. v2 ContextPacket identity

Production-bound ContextPacket identity remains:

```text
ContextPacketId = "CTX:" + SHA256(canonical structured v2 bytes)
```

`StructuredContextHash` is the lowercase 64-hex digest portion.

Because `sourceStateHash` is inside the canonical structured bytes, two otherwise identical Character contexts derived from different causal Production states cannot silently share a Production-bound ContextPacketId.

This provides direct state-to-context identity binding without rehashing the Production projection.

## 19. Production-bound rendering

The v1 renderer remains byte-frozen.

The v2 trusted-state renderer preserves existing category separation and adds one explicit claim section:

```text
[WHO YOU ARE]
...

[WHAT IS HAPPENING]
...

[RIGHT NOW]
...

[WHAT YOU OBSERVED]
...

[WHAT YOU KNOW]
...

[WHAT YOU BELIEVE]
...

[WHAT YOU SUSPECT]
...

[WHAT YOU REMEMBER]
...

[WHAT YOU HAVE CLAIMED]
...

[WHO IS PRESENT]
...

[RELATIONSHIPS]
...

[WHAT YOU WANT]
...

[PRESSURES]
...
```

The claim heading is semantically important: it must never render claim text under Knowledge, World State, or another category that would upgrade its authority.

## 20. RecentPerformanceText rendering

Production-bound `RenderedContext.RecentPerformanceText` is a separate untrusted creative-content layer.

Genesis:

```text
RecentPerformanceText = ""
```

Next turn, exactly:

```text
[WHAT JUST HAPPENED]
<source display name>:
<exact accepted VisibleText>
```

No recent Performance text is concatenated into `TrustedStateText`.

No Performance text becomes system instruction authority merely because it is accepted creative history.

`OpportunityText` remains:

```text
You have the current opportunity to act.
```

## 21. ContextCompositionTrace evolution

Patch 0014 extends the trace with:

```text
SourceStateHash : StateHash?
IncludedRecentTakeIds : ImmutableArray<TakeId>
```

Fixture/v1 composition:

```text
SourceStateHash = null
IncludedRecentTakeIds = []
```

Production/v2 genesis:

```text
SourceStateHash = checkpoint.StateHash
IncludedRecentTakeIds = []
```

Production/v2 next turn:

```text
SourceStateHash = checkpoint.StateHash
IncludedRecentTakeIds = [ sourceCommit.Take.TakeId ]
```

Claims are included in existing IncludedRecordIds.

## 22. Composer API and anti-forgery rule

The existing public v1 method remains:

```text
DeterministicContextComposer.Compose(
    CharacterAccessProjection projection,
    CharacterId currentOpportunityCharacterId)
```

It remains the fixture/v1 compatibility path.

Patch 0014 adds an internal Production-bound composition path, conceptually:

```text
internal ComposeProductionBound(
    CharacterAccessProjection projection,
    CharacterId currentOpportunityCharacterId,
    ImmutableArray<ContextRecentPerformance> recentPerformances)
```

It is intentionally not public.

External callers must not be able to construct a Production-bound ContextPacket by injecting arbitrary recent Performance items without the continuity bridge proving their causal adjacency.

The public v1 composer must fail closed if handed a Production-backed projection with a non-null SourceStateHash rather than silently downgrading it to v1 and discarding state identity/Claims.

## 23. Genesis continuity path

`E0ProductionContextContinuity.ComposeGenesis(sourceCheckpoint)` succeeds only when the checkpoint source is an exact genesis ProductionState.

It must verify:

1. source checkpoint exists;
2. checkpoint identities are initialized;
3. checkpoint Current Opportunity exists and is in the roster;
4. recomputing the inherited exact genesis StateHash envelope over the checkpoint source Production projection yields exactly checkpoint.StateHash;
5. Production Access for checkpoint Current Opportunity succeeds;
6. Access projection SourceStateHash equals checkpoint.StateHash;
7. v2 composition succeeds with `recentPerformances = []`.

This method enables the deterministic spine to migrate to Production-bound Context from the opening turn without deleting the historical fixture/v1 path or changing its byte oracles.

## 24. Next-turn continuity path

`E0ProductionContextContinuity.ComposeNextTurn(...)` accepts:

```text
ProductionStateCheckpoint sourceCheckpoint
E0CausalCommit sourceCommit
E0OpportunityTransitionResult opportunityResult
```

The checkpoint represents the current next-opportunity ProductionState.

The source commit represents the immediately preceding Accepted Performance/consequence commit.

The opportunity result represents the immediately following Patch 0013 effective-opportunity transition.

The bridge must prove the exact adjacency:

```text
sourceCommit.ResultStateHash
    == opportunityResult.Event.ParentStateHash

opportunityResult.Event.ResultStateHash
    == sourceCheckpoint.StateHash

opportunityResult.State.StateHash
    == sourceCheckpoint.StateHash

opportunityResult.Event.SelectedCharacterId
    == sourceCheckpoint.CurrentOpportunityCharacterId

opportunityResult.History.LastOpportunityStateHash
    == sourceCheckpoint.StateHash

opportunityResult.History.CharacterIds[^1]
    == sourceCheckpoint.CurrentOpportunityCharacterId
```

It must also prove:

- all contract/version identities are supported;
- source commit contains an Accepted E0 Take;
- source CommitId and TakeId remain effective in current Production caches;
- source Take Scene equals current Scene;
- source Performance Character belongs to current roster;
- opportunity strategy contract remains the approved least-intervention v1 contract.

Any mismatch fails closed before Access/Context output exists.

## 25. Immediate recent Performance disclosure law

Blueprint 0.1 reserves Observation as a later subsystem but freezes E0 as exactly one co-present trio Scene.

Patch 0014 therefore adopts the narrow E0 rule:

```text
The exact visible Performance from the immediately preceding Accepted source Take
is permitted as recent Performance to the current roster Character receiving the next opportunity.
```

This is not a general all-scenes product law.

It is valid only because Patch 0014 remains inside the frozen E0 co-present-trio observation boundary.

When Observation/World Resolver exists, later contracts may replace this E0 immediate-disclosure rule for broader product scenarios.

## 26. Recent Performance source

The recent Performance item is derived only from:

```text
sourceCommit.Take
```

after the bridge proves the Patch 0012 -> Patch 0013 -> current-checkpoint chain.

Required disposition:

```text
Accepted
```

Rejected, Alternate, failed, cancelled, malformed, uncommitted, or merely generated Candidate output cannot enter recent Performance context.

Patch 0014 never accepts a caller-supplied arbitrary `CandidatePerformance` as recent history.

## 27. State source and recent history remain separate

ProductionState stores effective semantic projection.

E0CausalCommit stores accepted creative Performance/consequence history.

Patch 0014 must not duplicate raw accepted Performance text into Production records merely so Access can find it.

Correct separation:

```text
ProductionState
    -> trusted current state projection

E0CausalCommit
    -> immediate accepted Performance history

Continuity bridge
    -> validates adjacency
    -> strips/permits immediate recent history

Context v2
    -> keeps trusted state and recent Performance in separate fields
```

No new ProductionState field stores recent Performance text.

No StateHash projection change is required.

## 28. E0TakeStateBinding source-hash law

Patch 0012's `E0TakeStateBinding.Bind(...)` already receives:

```text
ProductionStateCheckpoint sourceCheckpoint
ContextPacket sourceContext
Accepted E0Take take
```

Patch 0014 strengthens association for Production-bound v2 Context:

```text
sourceContext.SourceStateHash == sourceCheckpoint.StateHash
```

must hold exactly.

A v2 Context with missing/uninitialized/foreign SourceStateHash fails closed.

This proves that the Production-bound Context identity used by the Accepted Take is bound to the exact checkpointed source state.

## 29. Legacy v1 downgrade guard

The existing fixture/v1 context path remains for historical regression and exact genesis compatibility.

However a v1 Context has no SourceStateHash and cannot safely represent an evolved post-genesis Performer source.

Therefore `E0TakeStateBinding.Bind(...)` may accept v1 only when the checkpoint source ProductionState is provably exact genesis by inherited genesis-hash recomputation.

For any evolved Production source state:

```text
v1 Context -> reject
v2 Production-bound Context with exact SourceStateHash -> required
```

This prevents a caller from bypassing Patch 0014 state binding by deliberately downgrading an evolved turn to the historical fixture Context format.

## 30. No new Production transition or StateHash kind

Patch 0014 composes information but does not mutate Production.

Therefore it adds:

- no new ProductionState mutation helper;
- no new Production StateHash envelope kind;
- no causal history event;
- no commit/event ID;
- no opportunity transition.

Inherited Patch 0012 genesis/causalCommit hashes remain unchanged.

Inherited Patch 0013 opportunityTransition hashes remain unchanged.

ContextPacket identity changes only because v2 has its own canonical Context byte contract.

## 31. Determinism

For identical:

- checkpoint source ProductionState;
- exact source causal commit;
- exact opportunity result;

Patch 0014 output must be byte-for-byte identical across repeat runs and ordinary supported cultures.

No output may depend on:

- current time/date;
- randomness;
- process ID;
- machine name;
- thread scheduling;
- dictionary insertion order;
- filesystem state;
- network state;
- provider/model state;
- GPU/NPU state;
- locale-sensitive sort/case behavior.

All canonical ordering remains ordinal.

## 32. Failure atomicity

Patch 0014 mutates no external state.

If any source, access, disclosure, canonicalization, or composition check fails:

- no ContextPacket is returned;
- no ProductionState changes;
- no history changes;
- no retry is performed;
- no alternate source is guessed;
- no stale packet is returned;
- no v1 downgrade fallback occurs for evolved state.

The caller receives one sanitized `E0ContextContinuityException` at the public continuity boundary.

## 33. Error-domain ownership

The public continuity bridge wraps expected lower deterministic failures from:

- Production/checkpoint identity validation;
- Access evaluation;
- Context composition/canonicalization;
- causal source validation;
- opportunity-result validation.

It must not expose provider/network/native errors because none belong in Patch 0014.

Existing direct Access/Context public APIs keep their existing exception domains.

## 34. Security / prompt-authority law

Patch 0014 is a security boundary as well as a continuity boundary.

It must preserve:

```text
system/application authority
!= trusted structured fictional state
!= accepted recent fictional Performance
!= future user/imported creative content
```

Specifically:

- Production authority records denied by Access never enter the packet;
- other Characters' private state never enters the packet;
- inactive records never enter the packet;
- record provenance/protection metadata never enters Character-facing content;
- recent Performance is never concatenated into TrustedStateText;
- recent Performance text is never treated as system instruction;
- CharacterClaim remains labeled as claim rather than fact/knowledge;
- no credentials/provider metadata enter the packet.

## 35. Memory and ARM64 battery implications

Patch 0014 remains deterministic CPU-side authority work.

That is intentional.

NPU offload is inappropriate for deterministic filtering/hash association because accelerator dispatch/setup would add complexity and energy cost without improving authority.

Per source-context composition:

- checkpoint capture stays O(1);
- SourceStateHash reuse is O(1) and avoids rehashing full Production;
- Access scans the bounded Production record set once and emits stripped immutable arrays;
- recent accepted history is bounded to at most one Performance item;
- Context canonicalization/rendering is proportional only to permitted packet content;
- no whole-session history copy occurs;
- no background task/polling loop is introduced;
- no idle CPU/GPU/NPU work exists.

This is appropriate for Windows ARM64 battery lifecycle because work occurs only at an explicit turn boundary and remains bounded.

A later semantic/relevance Context Composer may use NPU inference only after deterministic Access has removed prohibited information. Patch 0014 does not pre-empt that later provider choice.

## 36. Production Access validation invariants

The Production-backed Access overload must fail closed unless:

- ProductionState is non-null;
- StateHash/SceneId are initialized;
- roster is the frozen E0 three unique initialized Characters in canonical order;
- subject exists exactly once in Production Characters;
- subject exists exactly once in Scene roster;
- each retained ProductionRecord is structurally consistent with its domain/runtime record type;
- Character-owned record subject is in roster;
- Relationship subject/target are valid distinct roster Characters;
- record IDs remain unique;
- record text remains canonical valid text.

The Access layer does not trust malformed in-memory state merely because Production normally constructs valid instances.

## 37. Production Access equivalence at genesis

For an exact genesis ProductionState derived from a validated fixture, the Production-backed Access projection must be semantically equivalent to the existing fixture-backed projection for all categories that existed in the fixture contract:

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

Differences intentionally introduced by Patch 0014:

- Production projection carries SourceStateHash;
- Production projection has explicit Claims category (empty for current fixture genesis unless future valid fixture mapping provides claims);
- Access decisions may additionally account for lifecycle state.

This equivalence test protects the existing Character-sovereignty rules while changing the backing source from fixture to Production.

## 38. Context v1 regression law

The existing Patch 0005 Missing Raft reference oracle must remain byte-identical.

Patch 0014 tests must explicitly assert inherited exact v1:

- structured canonical JSON bytes;
- rendered canonical JSON bytes;
- StructuredContextHash;
- RenderedContextHash;
- ContextPacketId.

No v1 expected digest may be updated merely because v2 exists.

## 39. New independent v2 reference oracle

Implementation must derive at least one fixed Production-bound v2 Context oracle independently from Core implementation code.

Recommended reference path:

```text
Missing Raft fixture
    -> exact genesis ProductionState
        -> exact genesis checkpoint
            -> ComposeGenesis
```

and one evolved next-turn reference path:

```text
Missing Raft genesis
    -> accepted zero- or simple-mutation Patch 0012 causal commit
        -> Patch 0013 effective-opportunity transition
            -> checkpoint next-opportunity state
                -> ComposeNextTurn
```

The independent derivation must first reproduce inherited exact fixture/state/context hashes before its new v2 digest is accepted as a fixed oracle.

Do not generate the expected v2 digest by calling the same production canonicalizer under test.

## 40. Required behavioral tests — Production Access

At minimum:

1. exact genesis Production Access is semantically equivalent to fixture Access for inherited categories;
2. SourceStateHash is exact;
3. active post-commit state changes are reflected;
4. inactive records are absent from projection and explicitly denied;
5. subject-owned CharacterClaim is present only in Claims;
6. another Character's CharacterClaim is denied;
7. claim is not copied into Knowledge/Belief/Memory/World/Scene categories;
8. subject-owned Relationship is visible;
9. other Character Relationship is denied;
10. HistoricalTruth/WorldState/UnresolvedProposition remain denied;
11. provenance/protection/lifecycle metadata is stripped from Character-facing projection;
12. malformed roster/domain/relationship/record identity fails closed;
13. deterministic ordering is ordinal;
14. no public Production mutation is added by Access.

## 41. Required behavioral tests — Context v2

At minimum:

1. Production genesis v2 carries exact SourceStateHash;
2. genesis v2 recentPerformances is empty;
3. claims are structurally separate;
4. v2 TrustedStateText contains `[WHAT YOU HAVE CLAIMED]` only for claims;
5. genesis RecentPerformanceText is exact empty string;
6. ContextPacketId equals `CTX:` + StructuredContextHash;
7. exact v2 canonical property order is enforced;
8. exact recent item property order is enforced;
9. v2 repeat/culture determinism holds;
10. v1 canonical bytes/oracles remain unchanged;
11. public v1 composer refuses Production-backed projection downgrade;
12. malformed SourceStateHash fails closed;
13. duplicate record IDs across categories fail closed;
14. recent item Character must belong to roster;
15. recent TakeId must be initialized;
16. no recent text appears in TrustedStateText;
17. no claim appears as knowledge/world truth.

## 42. Required behavioral tests — next-turn continuity

At minimum:

1. valid Patch 0012 -> Patch 0013 -> checkpoint chain composes exactly one recent Performance;
2. recent TakeId equals source Accepted TakeId;
3. recent Character equals source Performance subject;
4. recent VisibleText equals exact Accepted Performance VisibleText;
5. current Context subject/opportunity equals checkpoint Current Opportunity;
6. SourceStateHash equals checkpoint StateHash;
7. stale/foreign source commit fails;
8. stale/foreign opportunity result fails;
9. opportunity ParentStateHash / commit ResultStateHash mismatch fails;
10. opportunity ResultStateHash / checkpoint mismatch fails;
11. opportunity selected Character / checkpoint opportunity mismatch fails;
12. history last hash mismatch fails;
13. history last Character mismatch fails;
14. source CommitId missing from effective cache fails;
15. source TakeId missing from committed cache fails;
16. non-Accepted source Take fails;
17. Scene mismatch fails;
18. source Performance Character outside roster fails;
19. unsupported opportunity strategy fails;
20. failure leaves every supplied immutable input unchanged.

## 43. Required behavioral tests — Take binding

At minimum:

1. production v2 Context with exact SourceStateHash binds;
2. production v2 Context with foreign SourceStateHash fails;
3. production v2 Context with uninitialized SourceStateHash fails;
4. evolved checkpoint + v1 Context fails downgrade guard;
5. exact genesis checkpoint + legacy v1 Context remains compatible;
6. existing Patch 0012 Accepted-Take/source-state tests remain valid;
7. v2 state-hash association does not replace existing Scene/subject/opportunity/roster/StateAuthority snapshot checks.

## 44. Required structural / reflection tests

At minimum:

- Continuity namespace exports only approved public types;
- Continuity exposes only ComposeGenesis and ComposeNextTurn;
- no public Continuity constructor/state mutation/history append API;
- ContextRecentPerformance has no public constructor/setter;
- Production Access overload exact signature is frozen;
- CharacterAccessProjection adds only approved fields;
- ContextPacket v2 fields are immutable;
- Context v1 constants remain exact;
- v2 constants remain exact;
- Context v1 public composer remains exact historical signature;
- Production-bound composer is not public;
- Access does not reference CausalCommit/Opportunity/Continuity;
- Production does not reference Access/Context/Continuity;
- no new Windows/network/random/time/provider/GPU/NPU public dependency exists.

## 45. Canonical preservation tests

Patch 0014 must explicitly retain inherited fixed oracles including:

- Patch 0003 fixture hash;
- Patch 0005 v1 Context hashes;
- Patch 0012 genesis Production StateHash;
- Patch 0012 causal-commit StateHash oracle;
- Patch 0013 opportunity-transition StateHash oracle.

The new Context v2 identity is additive and must not rewrite any prior oracle.

## 46. Simplicity guard

Patch 0014 must not introduce:

- a general event-store abstraction;
- a session aggregate;
- a repository/service locator;
- a generic context source interface hierarchy;
- background caches;
- semantic retrieval;
- vector search;
- model-assisted access filtering;
- observer framework;
- provider routing;
- async machinery where no asynchronous work exists;
- a second Production state representation.

The E0 continuity bridge is deliberately narrow and typed.

## 47. No full Scene-loop claim

After Patch 0014, Core can deterministically create the next legal ContextPacket after one completed causal-commit/opportunity cycle.

That is not the same as a complete Scene loop.

A complete loop still requires separately approved orchestration for repeated iteration, stop/budget semantics, provider attempts, failures/retries, and eventually full event-sequence reconstruction.

Patch 0014 may test one explicit second-turn context derivation but must not label E0-A behavioral harness execution complete.

## 48. No full replay claim

Patch 0014 composition is deterministic and recomputable from supplied authoritative inputs.

It does not define an ordered persistent event stream that reconstructs an arbitrary number of turns automatically.

Full multi-turn replay remains deferred.

## 49. No Observation claim

The immediate previous Accepted Performance is disclosed only under the frozen E0 co-present-trio constraint.

Patch 0014 does not infer:

- line of sight;
- hearing range;
- channel membership;
- concealment;
- private messages;
- spatial adjacency;
- attention gating.

Future Observation authority remains responsible for those broader semantics.

## 50. No provider/model claim

Patch 0014 produces a deterministic provider-neutral ContextPacket.

It does not decide:

- which model performs;
- Windows AI versus cloud provider;
- NPU/GPU/CPU inference;
- generation parameters;
- prompt template;
- retries;
- token budgets;
- cost authorization.

This separation is important for ARM64/NPU architecture: provider/runtime replacement remains outside deterministic creative authority.

## 51. Implementation surface expectation

If approved, implementation should prefer the smallest canonical surfaces:

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

Plus focused Patch 0014 tests/evidence.

ProductionState projection/canonicalizer should not need modification.

Opportunity implementation should not need modification.

## 52. Recursive audit order

Before approval, recursively audit:

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

## 53. Approval gate

Implementation must not begin until this blueprint reaches one complete zero-material-change recursive pass and receives explicit user approval.

Approval freezes:

- Patch 0014 purpose/scope;
- Production-backed Access law;
- CharacterClaim disclosure law;
- immediate recent-Performance disclosure law;
- Continuity dependency placement;
- v1 preservation/v2 Context versioning;
- SourceStateHash propagation;
- legacy downgrade guard;
- canonical v2 shape;
- test/non-goal boundaries.

Routine implementation may then patch the smallest source/test surface without redesigning these laws.
