# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.6 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden
Parent promoted `main` checkpoint: `7475a9397cff9063673908c666a729f0f3cd4525`
Blueprint branch: `h1-patch-0015-blueprint`

## 1. Purpose

Close the accepted-history boundary that Patch 0005 deliberately reserved and Patches 0011–0014 deliberately deferred:

```text
Accepted Take
    -> atomic causal commit
        -> closed accepted-Performance history projection
            -> effective next Opportunity
                -> current Production-backed Access
                    -> bounded Context containing accepted Scene history
                        -> next Performer
```

Patch 0014 proves exact current `ProductionState -> Access -> Context` continuity, but its v2 packet still requires:

```text
recentPerformances = []
RecentPerformanceText = ""
```

That is insufficient for Full Ensemble E0. Frozen Blueprint 0.1 requires social causality, coherence, accepted historical texture, and Context that can distinguish “what just happened.” Patch 0005 intentionally reserved plural `recentPerformances` and stated that a later accepted-history/commit slice must own its item schema, causal ordering, and safe population path.

Patch 0015 proposes that missing slice without provider/model invocation, durable persistence, or a full Scene-loop orchestrator.

## 2. Source-grounded correction history

### 2.1 Generic Character Context Consumption / PerformerInput — rejected

Blueprint 0.1 already freezes:

```text
Production State
    -> Access Control
        -> Context Composer
            -> bounded Character Context
                -> Performer
```

Patch 0006 already implements the semantic `ContextPacket -> CandidatePerformance` boundary. A second generic consumption/input layer would duplicate an existing authority seam without owning new authority.

### 2.2 Immediate-one-Performance-only Context — rejected

Proposal 0.2 attempted to disclose only the immediately previous accepted Performance. That is too weak because Blueprint 0.1 permits accepted historical texture to remain true without promotion into durable projected state. One-turn-only history would lose earlier dialogue/action that may remain socially causal, or pressure the system to promote every utterance/action into durable state.

Patch 0005 intentionally reserved a plural array. Proposal 0.6 therefore uses the complete accepted Performance sequence of the current bounded E0 Scene. Later product Context optimization may deliberately window/retrieve/summarize under a new contract; E0 excludes that optimization.

### 2.3 Provider-attempt provenance — deferred

Provider request framing, Run/Attempt attribution, retry/spend/cancellation, external disclosure provenance, raw response retention, and provider execution remain necessary later. They should attach after the semantic Context supplied to a Performer can preserve accepted Scene continuity correctly.

### 2.4 Result-wrapper trust — corrected

Proposal 0.3 proposed history advancement from result wrappers. Proposal 0.4 replaced that with the deterministic replay authorities already implemented:

```text
E0CausalCommit
    -> DeterministicCausalCommit.Replay(...)

E0OpportunityTransition
    -> DeterministicOpportunityAuthority.Replay(...)
```

History must not duplicate commit mutation logic, Director selection logic, or Opportunity StateHash logic.

### 2.5 Redundant projected hashes — removed

Proposal 0.3 history entries carried commit parent/result StateHashes and candidate-content identity/hash. Proposal 0.4 removed them. Those values remain authoritative on the causal commit/Take chain; duplicating them in a lightweight current-Scene Context projection creates surface without enabling independent event reconstruction.

### 2.6 Live source-Context omission — corrected

Proposal 0.4 still allowed an evolved legacy v2 Take to replay into accepted history because causal replay does not possess the source ContextPacket.

Proposal 0.5 corrected this: before replay/appending, `RecordCommit(...)` freshly composes the exact history-aware source Context from synchronized parent Production + source history and requires the committed Take’s source Character/ContextPacketId to equal that exact packet.

### 2.7 Public history-entry surface — removed

Proposal 0.4 exposed a history-entry type publicly. Proposal 0.5 made entries internal. External orchestration only needs to carry the closed history object. Character-safe history is inspectable on `ContextPacket.RecentPerformances`; Patch 0015 must not accidentally create a public transcript/event-store API.

### 2.8 Cross-boundary failure domains — normalized

Proposal 0.5 did not freeze how malformed/stale history failures propagate through three public domains.

Proposal 0.6 freezes one internal history invariant/projection implementation but preserves each existing public boundary’s failure type:

```text
history Initialize/RecordCommit/RecordOpportunity
    -> E0AcceptedPerformanceHistoryException

history-aware Production Context continuity
    -> E0ContextContinuityException

history-aware Take binding
    -> E0CausalCommitException
```

History validation errors crossing Context/Take boundaries are normalized into those boundary-owned exception types. Raw Performance text is never copied into public exception messages, `Data`, or retained unexpected diagnostic payload.

### 2.9 Public count removed

Proposal 0.5 exposed `AcceptedPerformanceCount`. It has no external consumer and duplicates internal `Entries.Length`.

Proposal 0.6 removes it. The history object publicly exposes only its Scene/state synchronization identity; internal transition logic uses the closed entry array length.

## 3. Frozen authority basis

### Blueprint 0.1

- Character != Performer.
- A generated attempt becomes history only if accepted as a Take.
- Accepted Performance + approved consequences form one atomic causal commit.
- The conceptual source of truth is append-only causal event history.
- Accepted historical texture remains true even when it is not durable projected state.
- Context packets conceptually include recent events / “what just happened.”
- fictional dialogue/imported text are untrusted creative content, separate from trusted state/system authority.
- Performance may be speech, action, silence, refusal, redirection, or another Character-legible response.
- partial/rejected/cancelled output must not enter Production history.
- Missing Raft E0 uses exactly three co-present Characters in one bounded Scene.
- full observation/location/hearing/attention semantics remain reserved.

### Patch 0005

Patch 0005 freezes:

- `recentPerformances` as a root semantic array;
- v1 historical emptiness;
- `RecentPerformanceText` as a separate rendered authority layer;
- future non-empty item schema/causal ordering as responsibility of a later accepted-history/commit slice;
- no fabricated transcript/history.

Patch 0014 preserves v1 bytes and adds Production-bound v2 while retaining empty recent Performance.

### Patch 0006

`CandidatePerformance.VisibleText` is the Character-legible Performance surface. Empty text is valid silence. Hidden reasoning/invisible pseudo-performance are outside CandidatePerformance. Typed address/nomination control is separate non-visible control and is explicitly not generic observation eligibility.

### Patch 0011

Rejected/Alternate Takes never enter Production history or recent-performance Context.

### Patch 0012

Only an Accepted Take bound to exact source authority may commit. `E0CausalCommit` owns the exact Accepted `E0Take`; that Take owns the exact `CandidatePerformance`. The causal StateHash chain binds accepted Take semantic identity and resulting Production projection. `DeterministicCausalCommit.Replay(...)` is canonical closed replay authority for an established commit event.

### Patch 0013

The postcommit state receives one canonical effective Opportunity transition. `E0OpportunityHistory` is routing history, not transcript/Performance history. `DeterministicOpportunityAuthority.Replay(...)` is canonical closed replay authority for an established Opportunity event.

### Patch 0014

Production-backed Context v2 is exact-state-bound. CharacterClaim and recent-Performance disclosure remain deferred. `SourceStateHash` is non-diegetic. Exact source Context is freshly recomposed at Take binding; metadata alone is insufficient.

## 4. Exact Patch 0015 question

> Can Ensemble maintain a closed deterministic projection of successfully committed Character-legible Performances for the current E0 Scene, synchronize that projection through the exact commit/Opportunity StateHash chain, prove that each newly recorded commit actually consumed the exact accumulated history-aware Character Context, and compose that history into later Character Context without turning historical dialogue/action into projected truth, Observation, Memory, Belief, Claim, or provider state?

## 5. Dependency direction

Preserve:

```text
Domain / Fixture / Production
    -> Access
        -> Context
            -> Performer / Integrity / Interpreter / StateAuthority / Take
                -> CausalCommit
                    -> Opportunity
```

Patch 0014 adds:

```text
ProductionStateCheckpoint + Access + Context
    -> Continuity
```

Patch 0015 adds only:

```text
CausalCommit
    -> accepted Performance history DATA contract

CausalCommit + Opportunity + Context continuity
    -> accepted Performance history ADVANCEMENT in Continuity

closed accepted Performance history
    -> history-aware Context continuity

closed accepted Performance history DATA
    -> history-aware exact Take source-context proof in CausalCommit
```

Rules:

- `Context` does not depend on CausalCommit, Opportunity, or Continuity.
- accepted-history data lives in CausalCommit low enough for `E0TakeStateBinding` to consume without a CausalCommit -> Continuity/Opportunity dependency.
- accepted-history advancement lives in Continuity because it observes Context continuity, CausalCommit replay, and Opportunity replay.
- Opportunity remains unchanged and continues to depend on CausalCommit, not vice versa.
- Production projection remains unchanged.

## 6. Exact new public surface

### `Ensemble.E0.Core.CausalCommit`

Add exactly:

```csharp
public sealed class E0AcceptedPerformanceHistory
public sealed class E0AcceptedPerformanceHistoryException : Exception
```

`E0AcceptedPerformanceHistory` has no public constructor/setter and exposes only:

```text
SceneId : SceneId
CurrentStateHash : StateHash
```

Its ordered entry array is internal.

Internal data type:

```text
E0AcceptedPerformanceHistoryEntry
- CommitId
- TakeId
- SubjectCharacterId
- SourceContextPacketId
- VisibleText
```

The internal entry type has no public surface contract.

### `Ensemble.E0.Core.Continuity`

Add exactly:

```csharp
public static class E0AcceptedPerformanceHistoryContinuity
```

Public methods:

```csharp
E0AcceptedPerformanceHistory Initialize(
    ProductionState genesisState)

E0AcceptedPerformanceHistory RecordCommit(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState parentState,
    E0CausalCommit committedEvent)

E0AcceptedPerformanceHistory RecordOpportunity(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState postCommitState,
    E0CausalCommit sourceCommit,
    E0OpportunityHistory sourceOpportunityHistory,
    E0OpportunityTransition establishedEvent)
```

### Existing public surfaces extended narrowly

```csharp
E0ProductionContextContinuity.Compose(
    ProductionStateCheckpoint checkpoint,
    E0AcceptedPerformanceHistory history)
```

```csharp
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

### `Ensemble.E0.Core.Context`

Add exactly:

```csharp
public sealed class ContextRecentPerformance
```

with read-only:

```text
SourceCharacterId : CharacterId
VisibleText : string
```

and add:

```text
ContextPacket.RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

No public Context-history constructor/composer is added.

## 7. What history is — and is not

`E0AcceptedPerformanceHistory` internally answers:

> Which Character-legible Performances were successfully accepted into this bounded Scene’s causal history, in what causal order, and to which current Production StateHash has this live projection been advanced?

Its public surface exposes only Scene/state synchronization identity, not transcript/event details.

It does not answer:

- whether propositions in Performance are objectively true;
- what any Character observed, remembers, knows, believes, suspects, or claims;
- which consequences became durable state;
- which Performance is relevant/important;
- provider/model provenance;
- raw attempts/diagnostics;
- durable event reconstruction.

Authoritative current state remains `ProductionState`.

Authoritative causal event remains `E0CausalCommit`.

The history object is a **closed in-memory derived projection for current-Scene Performance continuity**.

## 8. Why the internal history entry is intentionally small

Internal entries retain only:

```text
CommitId
TakeId
SubjectCharacterId
SourceContextPacketId
VisibleText
```

Rationale:

- CommitId associates the projection with its authoritative causal event when event provenance exists.
- TakeId identifies the accepted Take occurrence.
- SubjectCharacterId identifies who performed.
- SourceContextPacketId identifies the semantic Context from which that Performance was produced.
- VisibleText is the Character-legible content later Context needs.

Do not copy into the projection:

- commit parent/result StateHashes;
- candidate-content hash/identity contract;
- full Take;
- State Authority package;
- record materializations;
- typed address/nomination control;
- provider/model data;
- raw/partial outputs;
- credentials;
- Observation/Knowledge/Belief/Memory/Claim classification.

Those belong to richer authoritative/provenance layers already implemented or deliberately deferred.

## 9. `CurrentStateHash` is synchronization, not parallel authority

A list of accepted text is insufficient. The live projection must remain synchronized to the deterministic state transition chain.

```text
genesis opportunity-bearing state
    history.CurrentStateHash = genesis StateHash

successful atomic commit replay
    append exactly one Performance
    history.CurrentStateHash = replayed postcommit StateHash

successful Opportunity replay
    append nothing
    history.CurrentStateHash = replayed opportunity-bearing StateHash
```

Later history-aware Context/Take boundaries require:

```text
history.CurrentStateHash == checkpoint.StateHash
history.SceneId == checkpoint.SceneId
```

`CurrentStateHash` is not `PerformanceHistoryHash` and does not replace `StateHash` authority.

Important limitation:

> Equality of a caller-supplied StateHash field is not by itself proof that arbitrary Performance text belongs to that state.

Authority comes from closed initialization/advancement plus canonical replay and exact source-Context proof. Patch 0015 does not claim that a deserialized/reflection-forged history object can be authenticated from `CurrentStateHash` alone. Durable reconstruction/authentication requires the future causal event store.

## 10. History initialization

`E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)` reuses existing exact genesis/routing authority rather than duplicating it.

Required proof:

```csharp
E0OpportunityHistory.Initialize(genesisState)
```

This already proves non-null Production, initialized StateHash/SceneId, canonical E0 roster, one valid current opportunity, and exact genesis StateHash recomputation.

History initialization returns internally:

```text
SceneId = genesisState.SceneId
CurrentStateHash = genesisState.StateHash
Entries = []
```

No opening transcript is invented.

Initialization catches expected `E0OpportunityTransitionException`/identity failures and emits sanitized `E0AcceptedPerformanceHistoryException`. It does not concatenate source messages containing user/creative text.

## 11. Commit advancement proves exact history-aware source Context

Exact API:

```csharp
RecordCommit(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState parentState,
    E0CausalCommit committedEvent)
```

Required sequence:

1. validate non-null/initialized history/event association;
2. require `sourceHistory.SceneId == parentState.SceneId`;
3. require `sourceHistory.CurrentStateHash == parentState.StateHash`;
4. defensively validate source history closed shape;
5. capture exact parent checkpoint:

```csharp
var checkpoint = ProductionStateCheckpoint.Capture(parentState);
```

6. freshly compose exact expected live source Context:

```csharp
var expected = E0ProductionContextContinuity.Compose(
    checkpoint,
    sourceHistory);
```

7. require the established commit’s exact Accepted Take Performance to satisfy:

```text
Take.Disposition == Accepted
Take.Performance.SubjectCharacterId
    == expected.ContextEvaluation.Packet.SubjectCharacterId
Take.Performance.ContextPacketId
    == expected.ContextEvaluation.Packet.ContextPacketId
```

8. call exactly:

```csharp
DeterministicCausalCommit.Replay(parentState, committedEvent)
```

9. use fresh replayed state as causal transition authority;
10. require replayed Scene remains exact and postcommit state has no current opportunity;
11. append exactly one internal entry from CommitId, TakeId, Performance subject/source Context identity, and exact VisibleText;
12. advance `CurrentStateHash` to fresh replayed StateHash.

### Why ContextPacketId equality is sufficient here

The expected packet is freshly recomposed from exact synchronized Production checkpoint + closed history. `ContextPacketId` is content-addressed from exact canonical structured semantic Context bytes.

The committed Candidate/Integrity/Interpretation/Take chain already binds `CandidatePerformance.ContextPacketId` to its source Context identity. Exact equality therefore proves that the committed Performance was sourced from the exact expected semantic Character Context for this live history state.

This does **not** prove exact provider transport/request disclosure; provider-attempt provenance remains later scope.

### Compatibility firewall

- first live commit from exact genesis uses exact history-aware genesis v2 because no accepted history exists;
- after first accepted Performance, expected live Context is v3;
- an evolved legacy history-omitting v2 commit may remain valid under Patch 0014 historical APIs, but `RecordCommit(...)` rejects it from the new accepted-history chain because its source `ContextPacketId` differs from freshly recomposed v3.

### Visible-text invariant reuse

The appended text comes only from successfully replayed Accepted `CandidatePerformance`.

For defensive history validation, implementation may expose the exact existing Patch 0006 visible-text validator as internal-only reusable logic (helper accessibility/forwarder only). Do not create a second text grammar or alter Candidate public semantics.

### Failure normalization

`RecordCommit` owns the history-transition public boundary. Expected failures from checkpoint capture, history-aware Context composition, causal replay, and internal history validation are normalized to sanitized `E0AcceptedPerformanceHistoryException`.

Retained inner exceptions are allowed only for current upstream exception domains whose messages are structural/sanitized; no raw Performance text, Context prose, mutation text, provider data, credentials, or arbitrary payload snippets may enter the public exception representation.

Unexpected programming/runtime failures are not relabeled as ordinary history rejection.

## 12. Opportunity advancement reuses canonical replay and couples routing/history counts

Exact API:

```csharp
RecordOpportunity(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState postCommitState,
    E0CausalCommit sourceCommit,
    E0OpportunityHistory sourceOpportunityHistory,
    E0OpportunityTransition establishedEvent)
```

Required sequence:

1. validate non-null/initialized inputs;
2. require `sourceHistory.CurrentStateHash == postCommitState.StateHash`;
3. require exact Scene association;
4. require internal source history non-empty;
5. require the internal last accepted-history entry equals the complete minimal source projection of `sourceCommit`:

```text
CommitId
TakeId
SubjectCharacterId
SourceContextPacketId
VisibleText
```

6. require `sourceCommit.ResultStateHash == postCommitState.StateHash`;
7. require normal alternating E0 chain relation before replay:

```text
sourceOpportunityHistory.CharacterIds.Length
    == sourceHistory.Entries.Length
```

At genesis counts are `1 opportunity / 0 Performances`; after each accepted commit they are equal; after each successful Opportunity transition routing history advances by one while Performance history does not.

8. call exactly:

```csharp
DeterministicOpportunityAuthority.Replay(
    postCommitState,
    sourceCommit,
    sourceOpportunityHistory,
    establishedEvent)
```

9. use fresh replay result as authority;
10. require fresh result routing-history length equals `sourceHistory.Entries.Length + 1`;
11. require fresh result routing-history last Character == event selected Character == result Production current opportunity;
12. append no Performance entry;
13. advance only `CurrentStateHash` to fresh replayed opportunity-bearing StateHash.

This reuses Patch 0013 source-chain validation, Director recomputation, event validation, canonical Opportunity StateHash recomputation, Production transition, and routing-history advancement.

### Failure normalization

Expected history/input/replay failures are normalized to sanitized `E0AcceptedPerformanceHistoryException`; unexpected programming/runtime failures remain technical failures.

## 13. Public exception-domain preservation outside history advancement

The internal history validator/projector is shared, but public boundary ownership remains exact.

### History-aware Context continuity

`E0ProductionContextContinuity.Compose(checkpoint, history)` catches expected `E0AcceptedPerformanceHistoryException` and emits sanitized `E0ContextContinuityException` with the history exception retained only as a sanitized inner exception.

It does not expose `E0AcceptedPerformanceHistoryException` as a second expected public failure domain from this method.

### History-aware Take binding

`E0TakeStateBinding.Bind(checkpoint, context, take, history)` catches expected `E0AcceptedPerformanceHistoryException` and emits sanitized `E0CausalCommitException`, preserving the established CausalCommit binding failure domain.

It does not expose history exception as a second expected public failure domain from binding.

### Sanitization

No Patch 0015 public exception message, retained expected inner-exception chain, `Data`, or `ToString()` may contain:

- Performance `VisibleText`;
- trusted Context prose;
- private record text;
- mutation text;
- unknown payload snippets;
- provider content;
- credentials/secrets.

Do not catch/relabel arbitrary unexpected exceptions merely to satisfy this taxonomy.

## 14. Failure and non-effective paths

No accepted-history entry is created for:

- Rejected Take;
- Alternate Take;
- Integrity Reject;
- Integrity RequestAnotherTake;
- unresolved State Authority review;
- malformed Candidate;
- provider refusal/error/timeout/cancellation;
- partial stream;
- failed causal commit;
- causal commit whose source Context omits/mismatches required accepted history;
- failed Opportunity establishment/replay;
- technical diagnostic text.

A successfully Accepted zero-mutation Take **does** append its Performance because historical texture occurred even when no durable state record changed.

A successfully Accepted Take whose proposed consequences are all authoritatively Rejected likewise appends its Performance because the accepted Performance entered causal history atomically with the terminal consequence decision package.

## 15. Current-Scene causal order and E0 history window

For the bounded current E0 Scene:

```text
recentPerformances = every internal accepted Performance history entry
```

in exact append order.

No sorting, deduplication, relevance scoring, recency count, truncation, summarization, paraphrase, token budgeting, or model compression.

Why complete current-Scene history:

- Patch 0005 deferred causal ordering to accepted-history authority;
- Blueprint 0.1 preserves historical texture without state explosion;
- E0 excludes context optimization;
- Full Ensemble E0 needs multi-turn social causality without hidden provider-session memory;
- one-Performance history discards accepted Scene texture too aggressively.

This is deterministic E0 reference composition, not the final product long-context strategy. A later version may narrow already-eligible history under a new composition contract and separately controlled experiment.

## 16. Scene boundary

History is scoped to exactly one current E0 Scene.

Patch 0015 does not define cross-Scene transcript carryover, archive retrieval, scene-to-scene memory promotion, branches/canon merge, cross-Production Character history, or persistent history loading.

A future Scene-transition architecture must explicitly decide what prior history becomes available under what Character-access/observation/memory law.

## 17. E0 `copresent-trio.v1` recent-Performance eligibility

Fixture Dialect v1 accepts exactly:

```text
ensemble.e0.copresent-trio.v1
```

and canonical Missing Raft freezes exactly three co-present Characters for the bounded E0 observation window.

Patch 0015 gives that E0-only reference contract one narrowly executable recent-Performance meaning:

> Successfully committed `CandidatePerformance.VisibleText` is common Character-legible Scene-performance history for every current roster Character under `ensemble.e0.copresent-trio.v1`.

This is newly specified Patch 0015 behavior. Earlier sources froze the token/co-presence but did not silently define this executable recent-history rule.

The rule is limited to:

- successfully committed Accepted `VisibleText`;
- current E0 Scene/current roster;
- no typed control;
- no hidden reasoning;
- no provider diagnostics;
- no creator-only Production information;
- no inferred consequences;
- no claim-to-truth promotion.

This is **not** a global ontology rule that co-presence always implies complete perception. Future spatial/private/inaudible/concealed Performance grammars require an explicit observation contract and may not inherit this E0 behavior.

For the E0 reference contract, a Performance cannot rely on private-performance semantics while also expecting selective recipient disclosure; selective perception is outside this contract.

## 18. Recent Performance is not CharacterObservation

Patch 0015 preserves the future epistemic path:

```text
Event happened
    -> observation eligibility
        -> CharacterObservation
            -> possible Memory / Belief / Claim changes
```

Patch 0015 does not materialize `CharacterObservation` records.

Recent accepted Performance in Context does not mean every proposition is true, Knowledge was gained, Belief formed, durable Memory created, a CharacterClaim record exists, or general hearing/location/attention semantics are solved.

The recent layer records only that common Character-legible Performance occurred under the E0 reference disclosure contract. Epistemic consequences remain separately proposed/authorized.

## 19. Typed control remains non-visible

Patch 0006 `AddressedCharacterIds` and `NominatedCharacterId` are routing/intent metadata, not generic observation eligibility.

They never enter `ContextRecentPerformance` or `RecentPerformanceText`.

Exposing them as historical Character semantics would create a knowledge channel not authorized by Patch 0006.

## 20. Context v3 semantic shape

Add:

```text
ContextRecentPerformance
- SourceCharacterId
- VisibleText
```

`ContextPacket` adds:

```text
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

Rules:

- v1: exactly empty;
- v2: exactly empty;
- v3: one or more entries;
- v3 order equals accepted-history append order exactly;
- each source Character resolves exactly once in packet roster;
- text is copied exactly from accepted Candidate `VisibleText`, including valid silence;
- no CommitId/TakeId/StateHash/ContextPacketId/candidate hash/control enters Character semantic recent history.

Context receives only projected safe recent semantic items from the closed history layer. No public raw-history/prose composer is added.

## 21. Exact Context v3 contracts

Preserve historical constants/bytes:

```text
SchemaVersion = ensemble.e0.context.v1
CompositionContract = ensemble.e0.context.full-authorized.v1
RenderingContract = ensemble.e0.context.render.v1
ProductionBoundSchemaVersion = ensemble.e0.context.v2
ProductionBoundCompositionContract = ensemble.e0.context.production-bound.v1
```

Add exactly:

```text
AcceptedHistorySchemaVersion
    = ensemble.e0.context.v3

AcceptedHistoryCompositionContract
    = ensemble.e0.context.production-bound.accepted-history.v1

AcceptedHistoryRenderingContract
    = ensemble.e0.context.render.v2
```

Version matrix:

```text
v1 + full-authorized.v1 + render.v1
    SourceStateHash = null
    RecentPerformances = []
    RecentPerformanceText = ""

v2 + production-bound.v1 + render.v1
    SourceStateHash initialized
    RecentPerformances = []
    RecentPerformanceText = ""

v3 + production-bound.accepted-history.v1 + render.v2
    SourceStateHash initialized
    RecentPerformances non-empty
    RecentPerformanceText non-empty
```

Every hybrid combination fails closed.

## 22. Genesis behavior

Genesis has no accepted Performance history.

The history-aware live path with exact initialized empty history emits existing Patch 0014 v2 unchanged. No empty-history v3 packet exists.

```text
v2 = exact Production-bound Context with no accepted current-Scene Performance yet
v3 = exact Production-bound Context plus non-empty accepted current-Scene Performance history
```

Non-empty history at exact genesis fails. Empty history at non-genesis history-aware continuation fails.

## 23. Historical v1/v2 compatibility and live-path firewall

Preserve existing APIs:

```csharp
DeterministicContextComposer.Compose(...)
E0ProductionContextContinuity.Compose(checkpoint)
E0TakeStateBinding.Bind(checkpoint, context, take)
```

The old one-argument Production Continuity path remains a current-state-only compatibility path and may emit v2 for evolved Production because Patch 0014 established that behavior.

The old three-argument Take binding remains v1/v2 compatibility and must reject v3.

The new Full Ensemble accepted-history chain advances only through `RecordCommit(...)`, which freshly recomposes the required history-aware source Context. After the first accepted Performance, a legacy evolved-v2 commit therefore cannot enter the live chain.

```text
historical/regression compatibility
    old v1/v2 APIs remain callable

new Full Ensemble live chain
    closed history advancement requires exact history-aware v2-at-genesis / v3-after-history source Context
```

The future Harness Scene-loop patch must use history-aware APIs; Patch 0015 does not falsely claim that orchestration is implemented yet.

## 24. History-aware Production Context Continuity

Add:

```csharp
E0ProductionContextContinuity.Compose(
    ProductionStateCheckpoint checkpoint,
    E0AcceptedPerformanceHistory history)
```

Behavior:

```text
validate checkpoint/current Production
    -> validate closed history shape
    -> require history Scene == checkpoint Scene
    -> require history CurrentStateHash == checkpoint StateHash
    -> fresh Production Access once
    -> if internal history empty:
           require exact genesis
           compose existing internal v2
       else:
           require non-genesis
           project exact ordered ContextRecentPerformance[]
           compose internal v3
    -> require Packet/Trace SourceStateHash == checkpoint StateHash
    -> return existing Access + Context result shape
```

Expected history failures are normalized to `E0ContextContinuityException` as frozen in section 13.

## 25. History-aware exact Take binding

Add:

```csharp
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

Four-argument live binding:

- exact genesis v2 only with exact empty initialized history;
- v3 only with non-empty synchronized history;
- rejects v1;
- rejects evolved v2;
- rejects empty-history evolved state;
- rejects non-empty history at genesis.

For v3:

1. validate checkpoint/current state identities;
2. validate closed history shape;
3. require history StateHash/Scene match checkpoint;
4. fresh Production Access;
5. project exact ordered recent Performance semantics;
6. fresh internal v3 Context composition;
7. compare exact canonical structured bytes;
8. compare exact canonical rendered bytes;
9. compare ContextPacketId;
10. compare StructuredContextHash;
11. compare RenderedContextHash;
12. compare SourceStateHash;
13. then run inherited exact Take/StateAuthority snapshot proof.

No arbitrary caller-supplied recent prose parameter exists.

Existing three-argument binding retains Patch 0014 behavior and cannot acquire a v3 bypass.

Expected history failures are normalized to `E0CausalCommitException` as frozen in section 13.

## 26. Shared internal history validation/projector

Use one internal CausalCommit-layer helper for history-aware Context continuity, history-aware Take binding, and history advancement preconditions.

It proves at minimum:

- history non-null;
- internal Entries not default;
- SceneId initialized;
- CurrentStateHash initialized;
- every entry non-null;
- CommitId initialized;
- TakeId initialized;
- SubjectCharacterId initialized;
- SourceContextPacketId initialized;
- VisibleText non-null and valid under the exact existing Candidate visible-text invariant;
- CommitIds unique;
- TakeIds unique;
- when projecting for current Context, every source Character resolves exactly once in current roster.

Internal entry count is always `Entries.Length`; no stored/public duplicate count exists.

Do not attempt full event replay from projected entries. CurrentStateHash equality alone is not authentication; closed initialization/advancement is the authority assumption for this in-memory projection.

## 27. Structured v3 canonicalization

V3 preserves v2 root order:

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

Recent item property order:

```text
sourceCharacterId
visibleText
```

Recent array order is causal append order and never sorted. Historical roster/record/relationship ordering remains unchanged.

```text
StructuredContextHash = SHA256(canonical v3 structured bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Different Performance text, source Character, causal order, or silence/non-silence therefore changes v3 structured identity even if durable current-state records are otherwise identical.

## 28. Render-v2 exact recent-Performance shape

A non-empty Character-visible recent layer changes rendering semantics, so v3 uses `ensemble.e0.context.render.v2`.

`TrustedStateText` and `OpportunityText` preserve existing v1 algorithms byte-for-byte. Only `RecentPerformanceText` gains non-empty behavior.

Exact non-silent form:

```text
[RECENT PERFORMANCES]
<source display name>:
[PERFORMANCE]
- <first source line>
  <continuation source line>

<next source display name>:
[PERFORMANCE]
- <first source line>
  <continuation source line>
```

Rules:

- accepted causal order;
- exactly one blank line between entries;
- no trailing LF;
- source display name resolves from current packet roster;
- source text never trimmed/paraphrased/summarized/repaired/reordered;
- non-empty text uses existing LF split / two-space continuation rendering discipline;
- Character IDs and causal/provenance IDs are not rendered.

Roster/display-name semantics remain inherited: the safe structured roster carries both CharacterId and DisplayName, while provider-neutral human-readable rendering uses display names only. Patch 0015 does not add a new display-name uniqueness law or alter Patch 0005 rendering conventions.

## 29. Silence rendering

Empty Candidate `VisibleText` is valid silence.

Structured v3 item:

```json
{"sourceCharacterId":"...","visibleText":""}
```

Rendered entry:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

No `[PERFORMANCE]` line/bullet body follows silence.

Literal non-silent text `[PERFORMANCE: SILENCE]` renders through ordinary non-silent form, so it cannot collide with semantic silence.

## 30. Recent Performance remains untrusted creative content

`RecentPerformanceText` remains separate from `TrustedStateText`.

Future provider framing must preserve distinct authority layers for system/Performer contract, trusted state, recent accepted fictional Performance, opportunity, and imported/user content where applicable.

A prior accepted line containing prompt-like language gains no system authority by being retained in Scene history.

Patch 0015 does not define provider request framing.

## 31. Character self-history

The current Character receives prior accepted Performances from all roster Characters, including their own earlier Performances, on a later opportunity.

This is common E0 Scene-performance history under `copresent-trio.v1`; removing self history would introduce an unsupported special forgetting rule.

It does not materialize durable Memory. Future memory fallibility/retention remains separate.

## 32. Minimal-disclosure reconciliation

Full current-Scene recent Performance does not authorize whole-Production disclosure.

The E0 reference path discloses only:

```text
current Character-safe Access projection
+ common accepted Character-legible Scene Performance history
+ current opportunity
```

It still excludes Production-only truth, other Characters’ private records, denied Access rows, fixture provenance, typed control, provider diagnostics, and creator-only data.

E0 uses complete accepted current-Scene Performance history because semantic relevance/windowing is intentionally outside the reference experiment. A later product composer may narrow already-eligible history under a new contract.

## 33. `ContextCompositionTrace` remains unchanged

Do not add accepted-history count/source Characters/CommitIds/TakeIds to `ContextCompositionTrace`.

Packet `RecentPerformances` exposes exact Character semantic items; structured/rendered hashes identify exact emitted bytes; causal provenance remains in the internal history projection. Adding trace fields would duplicate information without a current consumer.

## 34. No separate history hash

Do not add `PerformanceHistoryHash`.

Closed advancement is anchored to Production StateHash; each accepted commit already binds Take/Candidate identity; each v3 Context hashes exact disclosed history semantics. A second history hash would look like parallel authority without making the projection durably reconstructible.

A future persisted history envelope may require its own canonical/authenticated identity. That is persistence scope.

## 35. Causal commit and Opportunity history stay authoritative in their domains

`E0CausalCommit` remains causal-event authority with exact Accepted Take, materializations, and parent/result StateHashes.

`E0OpportunityHistory` remains routing-only: who held effective opportunity in routing order?

`E0AcceptedPerformanceHistory` internally answers: which Character-legible Performances were accepted in Scene causal order?

Do not add transcript semantics to Opportunity history or routing semantics to accepted Performance entries.

## 36. Production projection remains unchanged

Recent Performance history must not enter `ProductionStateProjection`. Doing so would duplicate event history into current state, make historical texture look like durable state, cause unnecessary state growth, alter Patch 0012/0013 StateHash oracles, and undermine the frozen historical-texture/durable-consequence distinction.

Trusted current consequences continue to reach Character Context only through committed Production records and current Access.

## 37. CharacterClaim remains deferred

A prior accepted Performance may contain a factual-sounding claim. That does not make it a `CharacterClaim` record, Knowledge, Belief, Memory, or truth.

Patch 0014 `CharacterClaimDisclosureDeferred` remains exact for retained Production records. Patch 0015 adds only common accepted E0 Performance occurrence history.

## 38. Observation-contract representation limitation

Current Production projection does not separately carry fixture `ObservationContract`.

Patch 0015 does not add it solely for this E0-only behavior.

Fixture Dialect v1 validates exactly one observation token, `ensemble.e0.copresent-trio.v1`, and Production genesis can originate only from validated E0 v1 fixture authority.

Therefore accepted-history disclosure is explicitly an **E0 Fixture Dialect v1 reference rule**, not a general future Production rule.

If a later dialect supports multiple observation contracts, active observation/disclosure authority must become explicit at the appropriate boundary and this assumption must be reopened rather than inherited silently.

## 39. Multi-turn live proof without durable replay

Patch 0015 supports multiple live E0 turns by induction:

```text
closed history at current opportunity-bearing StateHash
    + established causal commit event
        -> fresh exact history-aware source Context proof
            -> replay commit from exact parent
                -> append one accepted Performance
                -> history at no-opportunity postcommit StateHash
                    + matching source OpportunityHistory + established Opportunity event
                        -> replay Opportunity transition
                            -> append nothing
                            -> history at next opportunity-bearing StateHash
```

Repeat.

No older causal event is rediscovered from current projection. No durable history deserialization/reconstruction is claimed.

Full replay from genesis, persistence/recovery, branch reconstruction, and stored-event authentication remain later scope.

## 40. Exact current-state association and limitations

Normal supported closed progression rejects stale history, wrong Scene/branch, skipped commit/Opportunity advancement, wrong source ContextPacketId, foreign/tampered commit or Opportunity event, mismatched routing/performance counts, and evolved empty history.

Because history constructors are closed and entries internal, ordinary external callers cannot manufacture alternate supported history contents through normal C# construction.

Patch 0015 does not claim resistance to arbitrary reflection/runtime corruption or durable-store authentication without the future causal event chain.

## 41. Existing downstream semantic contracts remain version-agnostic where designed

Patch 0006 intentionally does not gate Candidate parsing on specific Context schema/composition tokens. Integrity and State Interpretation rebind Candidate/source association; Take replays State Authority from those associations.

Patch 0015 must prove exact v3 can flow through:

```text
PerformerCandidateContract
-> Integrity
-> State Interpretation
-> State Authority
-> E0Take.Bind
```

without changing public semantics, before history-aware `E0TakeStateBinding` proves exact source history.

If implementation discovers a hidden v1/v2 gate, patch only the smallest version-assumption surface while preserving semantics; do not redesign the pipeline.

## 42. Historical canonical compatibility authority

Preserve exactly:

### Context v1 Missing Raft / VOSS

```text
Structured bytes = 2569
StructuredContextHash = bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b
Rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

### Context v2 genesis / VOSS

```text
Source StateHash = 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
Structured bytes = 2655
StructuredContextHash = 27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
ContextPacketId = CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
Rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

### Context v2 canonical evolved / MARLOWE

```text
Source StateHash = dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
Structured bytes = 3456
StructuredContextHash = 9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
ContextPacketId = CTX:9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
Rendered bytes = 2389
RenderedContextHash = 9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

### Production chain

```text
Genesis StateHash = 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
Patch 0012 postcommit = 057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
Patch 0013 opportunity result = dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

Patch 0015 must not change any historical bytes/hashes.

## 43. Canonical first v3 oracle

Use the frozen Patch 0012 -> Patch 0013 oracle chain whose Candidate visible text is exactly `No.`:

```text
VOSS accepted Performance = "No."
postcommit StateHash = 057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
next Character = MARLOWE
current StateHash = dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
history = [ VOSS -> "No." ]
current Context = MARLOWE v3
```

Before native validation, an independent implementation-oracle derivation must produce exact v3 structured byte count/hash/ContextPacketId and render-v2 byte count/hash, after first reproducing inherited v1/v2/Production oracles.

A multi-entry test/oracle proves append order. A silence test/oracle distinguishes accepted silence from empty history and literal marker text.

## 44. Narrow inherited-test supersession

Patch 0014 intentionally asserted at that checkpoint that Context had no recent-Performance public type/property and only v1/v2 constants/one Production Continuity overload existed.

Patch 0015 supersedes only those temporary “not yet” assertions in:

```text
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ContractAuditTests.cs
```

Expected narrow updates:

- `ContextPacket.RecentPerformances` exists;
- exactly one new `ContextRecentPerformance` public Context type exists;
- exactly three v3/render-v2 constants are added;
- Production Continuity has two public `Compose` overloads.

Historical Patch 0014 evidence/docs remain immutable and truthful for their checkpoint. No unrelated earlier contract assertion is weakened.

## 45. Required implementation tests if approved

### History construction/public surface

- exact genesis initialization succeeds with internal empty history;
- non-genesis initialization rejects;
- history exposes only SceneId + CurrentStateHash publicly;
- no public history constructor/setter;
- no public history-entry/transcript collection/type;
- history exception publicly catchable with no public constructor;
- initialization invents no transcript.

### Commit advancement/source-history proof

- exact genesis history + exact genesis v2-sourced commit replays/appends once;
- state anchor advances to replayed postcommit StateHash;
- stale history/parent rejects before append;
- foreign/tampered commit rejects through replay;
- after one accepted Performance, exact v3-sourced next commit appends;
- after one accepted Performance, legacy evolved-v2-sourced commit is rejected from `RecordCommit` even if otherwise replayable;
- changed/dropped/reordered/extra source history changes expected ContextPacketId and blocks advancement;
- zero-mutation Accepted commit appends;
- all-consequence-Rejected Accepted commit appends;
- Rejected/Alternate/noncommit paths cannot append;
- duplicate CommitId/TakeId cannot enter normal replay progression;
- internal entry exact shape only.

### Opportunity advancement

- exact event replays through `DeterministicOpportunityAuthority.Replay`;
- last internal history entry equals source commit across all five minimal fields, including exact VisibleText;
- pre-transition OpportunityHistory length == internal Performance-entry length;
- post-transition routing length == Performance-entry length + 1;
- state anchor advances to fresh Opportunity StateHash;
- no Performance entry appended;
- stale/foreign routing history rejects;
- foreign/tampered Opportunity event rejects;
- skipped commit/Opportunity advancement causes later history-aware Context failure.

### Failure-domain normalization/privacy

- history APIs emit `E0AcceptedPerformanceHistoryException` for expected history/upstream contract failures;
- history-aware Context emits `E0ContextContinuityException`, not history exception;
- history-aware Take binding emits `E0CausalCommitException`, not history exception;
- raw Performance/Context/mutation/provider/secret text absent from Message/inner chain/Data/ToString for expected failures;
- unexpected programming/runtime failures are not swallowed as ordinary history rejection.

### Multi-turn

- two/three accepted commits yield exact causal order internally and in Context recent semantics;
- same Character can recur without deduplication;
- synchronization holds across commit -> Opportunity -> commit -> Opportunity;
- earlier accepted text remains when it created no durable Production record;
- self prior Performance is present on later opportunity;
- no provider-session memory required.

### V1/v2 compatibility

- historical v1 byte/hash oracles unchanged;
- genesis v2 oracle unchanged;
- evolved Patch 0014 v2 oracle unchanged;
- old one-argument Continuity retains v2 behavior;
- old three-argument Take binding retains v1/v2 and rejects v3.

### V3/history-aware matrix

- history-aware exact genesis emits v2;
- four-argument live binder accepts exact genesis v2 only with exact empty history;
- non-empty history emits v3;
- evolved empty history rejects;
- v3 requires render-v2;
- v1/v2 require empty recent semantics/text;
- v3 requires non-empty recent semantics/text;
- every hybrid version combination rejects;
- exact v3 root/item order.

### Disclosure/privacy/epistemic separation

- Context recent item contains only source CharacterId + exact VisibleText;
- typed address/nomination absent;
- causal IDs/hashes absent from Character recent semantics/rendered recent text;
- prior private Context state absent;
- CharacterClaim remains denied;
- no CharacterObservation/Knowledge/Belief/Memory record generated;
- factual-sounding speech remains historical Performance, not truth;
- self history included under E0 common Scene rule.

### Rendering

- exact single/multi-entry render;
- blank-line/order/LF/two-space continuation exact;
- exact Candidate Unicode/NFC invariant reused;
- silence render exact;
- literal marker cannot collide with semantic silence;
- no trailing LF;
- render-v2 canonical envelope exact.

### Exact Take binding

- exact synchronized v3 history/context/take accepts;
- stale/changed/reordered/dropped/extra history rejects;
- tampered structured/rendered bytes/IDs/hashes reject;
- SourceStateHash alone cannot rescue mismatched history;
- old binding cannot bypass v3 proof.

### Downstream semantic compatibility

- v3 Context -> Candidate -> Integrity -> Interpreter -> State Authority -> Take works without public semantic contract change;
- second v3-sourced Accepted Take binds/commits via history-aware proof;
- `RecordCommit` independently rechecks expected history-aware ContextPacketId before appending;
- no provider/model execution required.

### Determinism/oracle

- repeated byte determinism;
- unordered source collection invariance remains;
- history order intentionally order-sensitive;
- culture invariance (`ar-SA` or equivalent);
- canonical `No.` Patch0012->0013 v3 independent oracle;
- inherited Production/Context hashes preserved;
- no network/filesystem/clock/random/provider/Windows/NPU dependency.

### Public/dependency audit

- only approved new public types/methods/properties;
- no public history entry/count/transcript collection;
- Context has no CausalCommit/Opportunity/Continuity dependency;
- CausalCommit history data has no Opportunity/Continuity behavior;
- CausalCommit does not depend on Opportunity for Take binding;
- advancement alone depends upward inside Continuity;
- Opportunity history public shape unchanged;
- Production projection shape unchanged;
- no generic PerformerInput/provider abstraction/event bus/store interface.

## 46. Expected implementation surface

Patch-first source surface:

```text
src/Ensemble.E0.Core/CausalCommit/
    opaque public E0AcceptedPerformanceHistory
    internal entries/invariant+projection helper
    history-aware E0TakeStateBinding overload

src/Ensemble.E0.Core/Continuity/
    E0AcceptedPerformanceHistoryContinuity
    history-aware E0ProductionContextContinuity overload

src/Ensemble.E0.Core/Context/
    ContextRecentPerformance
    ContextPacket.RecentPerformances
    v3/render-v2 constants
    v3 canonical serializer/version validation
    internal v3 composer
    render-v2 recent Performance rendering

src/Ensemble.E0.Core/Performer/
    at most internal reuse of exact existing VisibleText validator; no semantic change

focused Patch 0015 tests
narrow Patch0014ContractAuditTests supersession
```

Preferred no-change semantics: Fixture JSON/dialect, Access policy, Production projection/transitions, Candidate public contract/parser behavior, Director selection policy, Opportunity canonicalizer/hash, Integrity, Interpreter, State Authority, Take semantics, Harness provider/runtime execution.

If implementation requires changing those semantics rather than wiring new history/context behavior, stop and reopen architecture.

## 47. Complexity and memory

Let:

```text
R = retained Production records
A = permitted current records
B = permitted current-state bytes
H = accepted Performance count in current E0 Scene
P = total accepted VisibleText bytes in current E0 Scene
```

Approximate boundary costs:

```text
History validation:        O(H)
RecordCommit:              fresh Access/Context + commit replay + O(H)
RecordOpportunity:         Opportunity replay + O(H)
History-aware Access:      O(R)
History-aware Context:     O(A log A + B + H + P)
History-aware Take proof:  fresh Access/Context + inherited StateAuthority proof
```

Repeated full-history rendering/immutable append can make total long-Scene work superlinear. That is accepted for bounded E0 reference work; product-scale history storage/context optimization is not claimed.

Do not invent a hard Scene-turn/history byte ceiling without separate run-protocol authority.

## 48. ARM64/battery suitability

Patch 0015 adds synchronous deterministic CPU/memory work only at explicit commit/Opportunity/Context/binding boundaries.

No idle polling, background loop, provider/network call, filesystem requirement, GPU/NPU work, Windows AI API, timer/random source, or emulation path.

The design is compatible with current native ARM64 deterministic Core/Harness architecture by construction, without making measured power/performance claims.

## 49. Explicit non-scope

Patch 0015 does not implement:

- general CharacterObservation generation;
- general location/hearing/attention/concealment observation engine;
- selective private Performance disclosure beyond E0 `copresent-trio.v1`;
- CharacterClaim Context disclosure;
- Memory/Belief/Knowledge promotion from recent Performance;
- history relevance/windowing/summarization;
- cross-Scene history retrieval;
- provider/model invocation;
- provider attempt/request provenance;
- retries/spend/cancellation/streaming;
- model-assisted Integrity/provider call;
- State Interpreter provider call;
- full Scene-loop orchestration;
- Run manifest/store;
- durable causal-event persistence/recovery;
- full multi-turn replay from genesis;
- branch/canon/retcon/rehearsal;
- World Resolver;
- WinUI;
- Windows AI Foundry/NPU;
- MSIX/WACK/Store certification.

## 50. Proposal 0.6 resolved questions

1. `E0AcceptedPerformanceHistory` remains the name; it is not a Take/event store.
2. History entries and ordered transcript remain internal-only.
3. Public `AcceptedPerformanceCount` is removed as unnecessary duplication.
4. Candidate-content hash is not duplicated into history.
5. Commit advancement first proves exact expected history-aware source ContextPacketId/subject, then replays the canonical commit.
6. Opportunity advancement compares the entire minimal last history entry to source commit, including exact VisibleText, couples routing/performance counts, then replays canonical Opportunity authority.
7. Direct four-argument Take binding consumes low history data only; no higher verified-token abstraction.
8. CurrentStateHash is synchronization metadata only.
9. Context trace remains unchanged.
10. Self history remains included.
11. Full current-Scene Performance history is E0-only eligible content under explicit common Character-legible `copresent-trio.v1`; Production/private state remains filtered.
12. Later newly versioned Context may optimize eligible history; E0 v3 remains unoptimized reference.
13. Legacy evolved v2 remains historical compatibility but cannot enter new live history after history exists.
14. Boundary-specific exception domains are preserved; history validator errors are normalized at Context/Take boundaries and creative/private text never enters expected public exceptions.

## 51. Recursive audit order

Every material correction restarts from correctness:

```text
correctness
-> consistency
-> authority
-> accepted-history integrity
-> disclosure/privacy
-> epistemic separation
-> dependency direction
-> version/canonical compatibility
-> multi-turn coherence
-> failure behavior
-> scope
-> tests
-> simplicity
-> hygiene
-> ARM64 suitability
-> project vision
-> evidence
```

## 52. Proposal 0.6 audit status

Proposal 0.6 resolves the failure-domain and remaining obvious public-surface issues found after Proposal 0.5.

The next complete pass must attack the whole design again, especially:

1. E0 common Character-legible history versus the still-reserved general Observation boundary;
2. live-chain induction and all skip/duplicate/reorder/cross-branch cases;
3. v1/v2 byte immutability under the added `RecentPerformances` property and v3 serializer;
4. whether any internal history provenance field is still redundant or insufficient;
5. whether exception normalization can preserve privacy without hiding unexpected failures;
6. whether any public surface remains removable without losing exact live proof.

No implementation, approval evidence, implementation handoff, `CURRENT_STATE.md` update, or promotion is permitted until one full recursive pass finds no material correction or worthwhile simplification and the user explicitly approves the resulting blueprint.
