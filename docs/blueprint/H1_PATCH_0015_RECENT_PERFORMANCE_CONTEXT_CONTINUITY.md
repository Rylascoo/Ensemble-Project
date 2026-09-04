# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.14 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden
Parent promoted `main` checkpoint: `7475a9397cff9063673908c666a729f0f3cd4525`
Blueprint branch: `h1-patch-0015-blueprint`

## 1. Purpose

Close the accepted-history seam deliberately reserved by Patch 0005 and deferred through Patches 0011–0014:

```text
Accepted Take
    -> atomic causal commit
        -> closed accepted-Performance history projection
            -> effective next Opportunity
                -> current Production-backed Access
                    -> bounded Context containing accepted Scene history
                        -> next Performer
```

Patch 0014 proves exact current `ProductionState -> Access -> Context` continuity but intentionally leaves:

```text
recentPerformances = []
RecentPerformanceText = ""
```

That is insufficient for Full Ensemble E0 because accepted Character-legible historical texture must survive independently invoked Performers without being misrepresented as durable Production state or silently delegated to provider conversation memory.

Patch 0015 adds only the deterministic accepted-Performance history projection and Context continuity needed to close that seam. It does not call a provider/model and does not implement the full Scene loop.

## 2. Source-grounded correction history

The recursive investigation rejected or corrected the following directions before Proposal 0.14:

1. **Generic Character Context Consumption / PerformerInput — rejected.** Blueprint 0.1 and Patch 0006 already freeze `ContextPacket -> Performer -> CandidatePerformance`; a second generic input layer duplicates existing authority.
2. **Character Interpretation — rejected.** It prematurely implies cognition/belief/memory authority.
3. **Observation-first — deferred.** Full observation eligibility requires later location/hearing/attention/private-performance semantics not present in current E0.
4. **Immediate-one-Performance-only history — rejected.** Accepted historical texture can remain causally relevant without durable-state promotion; one-turn-only history would lose Scene continuity or encourage state explosion.
5. **Provider-attempt provenance — deferred.** Provider request/attempt/retry/spend/raw-output provenance belongs after semantic accepted-history continuity exists.
6. **Result-wrapper trust — removed.** History advancement reuses canonical `DeterministicCausalCommit.Replay(...)` and `DeterministicOpportunityAuthority.Replay(...)` instead of trusting result wrappers or duplicating transition algorithms.
7. **Redundant projected causal fields — removed.** History does not copy CommitId, TakeId, per-entry StateHashes, ContextPacketId, candidate hash, typed control, or consequence packages.
8. **Live source-Context omission — closed.** `RecordCommit(...)` freshly recomposes the exact history-aware source Context and requires the committed Performance subject/ContextPacketId to match before canonical replay/append.
9. **Public history-entry/count API — removed.** Character-safe history is inspectable only through `ContextPacket.RecentPerformances`; the synchronization token itself is opaque.
10. **Failure-domain ownership — corrected.** History transition errors are Continuity-owned; CausalCommit history invariant errors are internal-only; history-aware Context and Take paths preserve their existing public exception domains.
11. **Precommit live-path law — added.** Full Ensemble execution must successfully bind the Take against exact accepted history before commit; postcommit `RecordCommit(...)` independently rechecks as defense in depth.
12. **Live API ambiguity — removed.** New methods are explicitly `ComposeWithAcceptedHistory(...)` and `BindWithAcceptedHistory(...)`, not overloads named like the historical history-omitting APIs.
13. **Commit adoption — fail-closed.** A future orchestrator stages a commit result and adopts the postcommit Production/history pair only after `RecordCommit(...)` succeeds.
14. **Opportunity adoption — fail-closed.** A future orchestrator stages Opportunity establishment and adopts the opportunity-bearing Production/history/OpportunityHistory triple only after `RecordOpportunity(...)` succeeds.
15. **Inherited reflection-suite boundary — corrected.** Later public additions necessarily supersede several historical exact-absence/public-surface tests; Proposal 0.14 enumerates each currently identified narrow test adaptation instead of discovering them during implementation.
16. **Live oracle lineage — corrected.** Historical Patch 0012/0013 state hashes were generated from a fixture-derived v1 source Context. Patch 0015 live genesis uses Production-bound v2; Candidate and causal identities bind ContextPacketId, so the live postcommit and Opportunity StateHashes must be newly derived and cannot reuse historical values.
17. **History token public state identity — removed in Proposal 0.14.** `SceneId` and `CurrentStateHash` are synchronization internals, not information external callers need. Exposing them duplicates Production identity and makes the derived token look more authoritative than it is. The public token therefore has zero public properties.

Every material correction restarts the recursive audit from correctness.

## 3. Frozen authority basis

Patch 0015 relies on established project authority:

- Character != Performer.
- Performer receives bounded Character Context and proposes Candidate Performance; Performer owns neither truth nor persistence.
- A generated attempt becomes fictional history only through accepted Take + successful atomic causal commit.
- Accepted Performance + terminal consequence package form one causal event.
- Append-only causal event history is the conceptual source of truth.
- Historical texture may remain true because it occurred even when it is not durable projected state.
- Context conceptually includes recent events / “what just happened” separately from trusted state.
- `CandidatePerformance.VisibleText` is exact Character-legible Performance; empty string is valid silence.
- Typed address/nomination control is non-visible routing/intent metadata, not generic observation eligibility.
- Rejected/Alternate/failed/partial/provider-technical output never enters Production or recent Performance history.
- Patch 0005 reserved plural `recentPerformances` and explicitly deferred non-empty item schema/order/population to a later accepted-history/commit slice.
- Patch 0012 owns atomic accepted causal commit and canonical commit replay.
- Patch 0013 owns effective Opportunity, routing history, canonical Opportunity replay, and Opportunity StateHash chaining.
- Patch 0014 owns exact Production-backed Access/Context continuity and exact source Context proof while keeping recent Performance empty.
- Fixture Dialect v1 has exactly one Scene, exactly three roster Characters, and exactly the observation token `ensemble.e0.copresent-trio.v1`; that token was previously structural/dialect authority, not a general executable perception engine.

## 4. Exact Patch 0015 question

> Can Ensemble maintain a closed deterministic projection of successfully committed Character-legible Performances for the current bounded E0 Scene, synchronize it through the exact commit/Opportunity StateHash chain, prove before commit and again at history projection that each live accepted Performance consumed the exact accumulated history-aware Character Context, and compose that accepted history into later Character Context without turning Performance prose into projected truth, Observation, Knowledge, Memory, Belief, Claim, provider state, or a competing event store?

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
ProductionStateCheckpoint + Access + Context -> Continuity
```

Patch 0015 adds:

```text
ContextRecentPerformance
    -> opaque accepted-history DATA in CausalCommit

opaque accepted-history DATA
    -> history-aware exact Take proof in CausalCommit

CausalCommit + Opportunity + Context continuity
    -> accepted-history ADVANCEMENT in Continuity

opaque accepted history
    -> history-aware Production Context continuity
```

Rules:

- `Context` never depends on CausalCommit, Opportunity, or Continuity.
- CausalCommit may continue depending on lower Context types.
- Accepted-history data/invariants live low enough for `E0TakeStateBinding` without introducing CausalCommit -> Opportunity/Continuity.
- History advancement alone lives in Continuity because it observes Context continuity, CausalCommit replay, and Opportunity replay.
- Opportunity remains unchanged and continues to depend on CausalCommit, never reverse.
- Production projection/state transition semantics remain unchanged.

## 6. Exact public and internal surface

### CausalCommit namespace

Add exactly:

```csharp
public sealed class E0AcceptedPerformanceHistory
```

It is a true opaque token:

```text
public constructors = 0
public setters      = 0
public properties   = 0
public methods declared on the type = 0
```

Internal state exactly:

```text
SceneId : SceneId
CurrentStateHash : StateHash
Entries : ImmutableArray<ContextRecentPerformance>
```

The internal invariant/projector helper may expose those internals only inside Core. Its exception type is internal-only.

Extend existing `E0TakeStateBinding` with exactly one new public static method:

```csharp
E0TakeStateBinding BindWithAcceptedHistory(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

Historical:

```csharp
Bind(checkpoint, context, take)
```

remains exact and unchanged.

### Context namespace

Add exactly:

```csharp
public sealed class ContextRecentPerformance
```

Read-only public properties exactly:

```text
SourceCharacterId : CharacterId
VisibleText       : string
```

No public constructor/setter.

`ContextPacket` adds exactly:

```text
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

No public history composer/raw-prose constructor is added.

### Continuity namespace

Add exactly:

```csharp
public static class E0AcceptedPerformanceHistoryContinuity
public sealed class E0AcceptedPerformanceHistoryException : Exception
```

The exception is publicly catchable with no public constructor.

Public transition methods exactly:

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

Extend `E0ProductionContextContinuity` with exactly:

```csharp
E0ProductionContextContinuityResult ComposeWithAcceptedHistory(
    ProductionStateCheckpoint checkpoint,
    E0AcceptedPerformanceHistory history)
```

Historical `Compose(checkpoint)` remains exact.

No public entry/transcript collection, count property, history hash, event-store abstraction, history repository, provider abstraction, or Scene-loop orchestrator.

## 7. History semantics

`E0AcceptedPerformanceHistory` is a closed in-memory derived Context projection. Internally it answers:

> Which Character-safe recent-Performance semantic items were admitted through the successful accepted live chain, in what append order, and to which current Production StateHash has this projection been synchronized?

It does not answer:

- whether propositions in Performance prose are true;
- what a Character observed, knows, believes, remembers, suspects, or claims;
- which consequences became durable state;
- provider/model/request/attempt provenance;
- relevance/importance;
- durable event reconstruction.

Authority remains:

```text
ProductionState               current-state authority
E0CausalCommit                causal-event authority
E0OpportunityHistory          routing-history projection
E0AcceptedPerformanceHistory  opaque live Context projection only
```

## 8. Internal history payload is Character-safe semantics only

History stores exactly:

```text
ImmutableArray<ContextRecentPerformance>
```

Each item stores only:

```text
SourceCharacterId
VisibleText
```

No per-entry:

- CommitId;
- TakeId;
- parent/result StateHash;
- ContextPacketId;
- CandidateContentHash;
- Candidate identity contract;
- typed control;
- State Authority data;
- materializations;
- provider data;
- Observation/Knowledge/Belief/Memory/Claim classification.

The current internal `CurrentStateHash` anchor binds the authoritative transition chain. History does not partially duplicate causal event provenance.

## 9. StateHash synchronization is not history authentication

Internal progression:

```text
exact genesis opportunity-bearing state
    CurrentStateHash = genesis StateHash
    Entries = []

successful canonical commit replay
    append exactly one recent semantic item
    CurrentStateHash = replayed postcommit StateHash

successful canonical Opportunity replay
    append nothing
    CurrentStateHash = replayed Opportunity StateHash
```

History-aware Context/Take require internal history SceneId/CurrentStateHash to equal the exact checkpoint Scene/StateHash.

StateHash equality alone does not prove arbitrary caller-fabricated transcript content. Supported authority depends on closed normal construction/advancement. Patch 0015 makes no claim against reflection/runtime memory corruption and no claim that a future deserialized history token can be authenticated without causal-event reconstruction.

## 10. Initialization

`E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)` must call existing:

```csharp
E0OpportunityHistory.Initialize(genesisState)
```

and rely on it for exact genesis Production/roster/current-opportunity/genesis-hash validation rather than duplicating that logic.

Success creates internal:

```text
SceneId = genesis.SceneId
CurrentStateHash = genesis.StateHash
Entries = initialized empty
```

No opening transcript is invented.

Expected upstream/closed-history failures normalize to sanitized `E0AcceptedPerformanceHistoryException`.

## 11. Approved Full Ensemble live sequence

Patch 0015 does not implement orchestration, but freezes the only approved future history-bearing sequence:

```text
current opportunity-bearing ProductionState
+ synchronized opaque accepted history
+ synchronized E0OpportunityHistory
    -> ProductionStateCheckpoint.Capture
    -> E0ProductionContextContinuity.ComposeWithAcceptedHistory
         exact empty genesis => v2
         accepted history    => v3
    -> Performer Candidate
    -> Integrity
    -> State Interpretation
    -> State Authority
    -> E0Take.Bind(... Accepted ...)
    -> E0TakeStateBinding.BindWithAcceptedHistory MUST succeed
    -> DeterministicCausalCommit.Commit
         result staged; not yet adopted into live pair
    -> E0AcceptedPerformanceHistoryContinuity.RecordCommit
         fresh exact source-Context proof + canonical commit replay + one append
    -> only then adopt postcommit Production + accepted-history pair
    -> DeterministicOpportunityAuthority.Establish
         result staged; not yet adopted into live triple
    -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
         canonical Opportunity replay + routing/history coupling
    -> only then adopt opportunity-bearing Production
       + accepted history + OpportunityHistory triple
```

Historical `Compose(checkpoint)` and `Bind(checkpoint, context, take)` remain regression/compatibility APIs but are not approved Full Ensemble history-bearing live calls.

## 12. Commit advancement

Exact API:

```csharp
RecordCommit(history, parentState, committedEvent)
```

Required order:

1. validate non-null inputs and closed history internals;
2. require internal history SceneId == parent SceneId;
3. require internal history CurrentStateHash == parent StateHash;
4. capture exact parent `ProductionStateCheckpoint`;
5. freshly call `ComposeWithAcceptedHistory(checkpoint, sourceHistory)`;
6. require committed Take disposition Accepted and Performance subject/ContextPacketId equal the freshly composed expected packet subject/ContextPacketId;
7. call exactly `DeterministicCausalCommit.Replay(parentState, committedEvent)`;
8. require fresh replay Scene association exact and current opportunity null;
9. append exactly one `ContextRecentPerformance` from committed Performance subject + exact VisibleText;
10. advance internal CurrentStateHash to the fresh replayed postcommit StateHash;
11. return a new history token; never mutate the source token.

The content-addressed ContextPacketId proves exact semantic source Context association. It does not prove provider transport/request bytes; provider disclosure provenance remains later scope.

The approved live sequence already performs `BindWithAcceptedHistory` before `Commit`; `RecordCommit` intentionally repeats exact source-history proof so a historical/foreign established commit event cannot be projected into the new live history chain.

A caller must not adopt a staged postcommit `ProductionState` into the Full Ensemble live pair unless `RecordCommit` succeeds for that exact parent/event.

## 13. Opportunity advancement

Exact API:

```csharp
RecordOpportunity(
    history,
    postCommitState,
    sourceCommit,
    sourceOpportunityHistory,
    establishedEvent)
```

Required order:

1. validate inputs/history;
2. require internal history CurrentStateHash == postcommit StateHash and Scene exact;
3. require history non-empty;
4. require sourceCommit.ResultStateHash == postcommit StateHash;
5. require last history semantic item equals source commit Performance on exact source Character + exact VisibleText;
6. require pre-transition `sourceOpportunityHistory.CharacterIds.Length == history.Entries.Length`;
7. call exactly `DeterministicOpportunityAuthority.Replay(postCommitState, sourceCommit, sourceOpportunityHistory, establishedEvent)`;
8. require fresh result routing-history length == history.Entries.Length + 1;
9. require fresh routing-history last Character == established event SelectedCharacterId == fresh result-state current opportunity;
10. append no Performance;
11. advance only internal CurrentStateHash to fresh Opportunity StateHash;
12. return a new history token.

Count induction:

```text
genesis:          OpportunityHistory = 1, PerformanceHistory = 0
after commit:     OpportunityHistory = H, PerformanceHistory = H
after opportunity OpportunityHistory = H+1, PerformanceHistory = H
```

`DeterministicOpportunityAuthority.Replay` already revalidates source commit/history, recomputes Director selection, recomputes the canonical Opportunity StateHash, and advances routing history. Patch 0015 must not duplicate those algorithms.

A caller must not adopt a staged opportunity-bearing Production/OpportunityHistory result into the Full Ensemble live triple unless `RecordOpportunity` succeeds.

## 14. Non-effective paths

No history entry is created for:

- Rejected or Alternate Take;
- Integrity Reject / RequestAnotherTake;
- unresolved State Authority review;
- malformed Candidate;
- provider error/refusal/timeout/cancellation;
- partial stream;
- failed causal commit;
- accepted event whose source Context omitted/mismatched required history;
- failed Opportunity establishment/replay;
- diagnostics/technical text.

A successfully Accepted zero-mutation Take does append its Performance.

A successfully Accepted Take whose proposed consequences are all authoritatively Rejected also appends its Performance because the accepted Character-legible Performance itself entered causal history even when no durable record changed.

## 15. Performance history and durable consequence are distinct layers

The same accepted causal event may later contribute independently to:

```text
RecentPerformances
    exact Character-legible historical occurrence

TrustedStateText
    separately authorized, committed, active, Access-permitted current consequence
```

Do not deduplicate, merge, or suppress one layer merely because their prose overlaps.

Historical occurrence != durable consequence.

No durable consequence != no accepted Performance.

## 16. E0 history eligibility and Observation boundary

Fixture Dialect v1 fixes exactly:

```text
ensemble.e0.copresent-trio.v1
```

and Missing Raft fixes three co-present Characters in one bounded E0 Scene.

Patch 0015 is the **first** contract to give that token this narrowly executable recent-Performance meaning:

> Every successfully committed `CandidatePerformance.VisibleText` in the current E0 Scene is common Character-legible recent Performance content for every current roster Character under `ensemble.e0.copresent-trio.v1`.

This new E0 reference rule applies only to:

- successful Accepted committed Performance;
- current E0 Scene;
- current roster;
- `VisibleText`, including semantic silence.

It does not expose:

- typed address/nomination control;
- hidden reasoning;
- provider diagnostics;
- creator-only Production information;
- inferred consequences;
- truth/Knowledge/Belief/Memory/Claim state.

This does **not** reinterpret historical packets or create `CharacterObservation` records, and it is not a global rule that co-presence always means complete perception. Future private/spatial/inaudible/concealed Performance or multiple observation contracts require explicit new Observation authority.

Selective recipient perception is outside current E0 reference scope.

## 17. Self history

When a Character later receives another Opportunity, accepted recent history includes that Character’s own earlier accepted Performances as well as the others'.

Reason: this is common E0 Scene-performance history; suppressing self history would introduce an unfrozen forgetting rule.

This still does not materialize durable CharacterMemory.

## 18. Current-Scene history window and order

For E0 v3:

```text
recentPerformances = every accepted history item in exact append order
```

No:

- sorting by Character/ID/time;
- deduplication;
- recency count;
- relevance scoring;
- truncation;
- summarization;
- paraphrase;
- token budgeting;
- model compression.

Patch 0005 explicitly deferred causal ordering to accepted-history authority, and E0 excludes context optimization. A later product composer may window/retrieve/summarize already-eligible history only under a new versioned composition contract and controlled comparison.

No E0 history/turn quota is invented here because current run-protocol authority does not freeze one.

## 19. Context recent semantic DTO

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

- v1/v2 require initialized empty `RecentPerformances`;
- v3 requires initialized non-empty `RecentPerformances`;
- default/uninitialized ImmutableArray fails every version;
- v3 array order equals accepted causal append order exactly;
- each source Character resolves exactly once in current packet roster;
- VisibleText is copied exactly from accepted Candidate semantics and may be empty only as the already-defined silence;
- no CommitId/TakeId/StateHash/ContextPacketId/candidate hash/control/provenance enters the Character semantic DTO.

The exact existing Patch 0006 `VisibleText` validator must be reused internally rather than reimplemented. A private helper may be widened to internal or forwarded by an internal helper; Candidate public semantics do not change.

## 20. Context contracts and version matrix

Historical constants remain exact:

```text
SchemaVersion                   = ensemble.e0.context.v1
CompositionContract             = ensemble.e0.context.full-authorized.v1
RenderingContract               = ensemble.e0.context.render.v1
ProductionBoundSchemaVersion    = ensemble.e0.context.v2
ProductionBoundCompositionContract
                                = ensemble.e0.context.production-bound.v1
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

Exact packet matrix:

```text
v1 + full-authorized.v1 + render.v1
    SourceStateHash = null
    RecentPerformances = initialized empty
    RecentPerformanceText = ""

v2 + production-bound.v1 + render.v1
    SourceStateHash initialized
    RecentPerformances = initialized empty
    RecentPerformanceText = ""

v3 + production-bound.accepted-history.v1 + render.v2
    SourceStateHash initialized
    RecentPerformances = initialized non-empty
    RecentPerformanceText = non-empty
```

A `ContextPacket` with any hybrid schema/composition/render/recent shape fails packet canonicalization and exact binding. Standalone rendered serialization remains a rendering-object operation; cross-layer schema/render compatibility is enforced where the packet is available.

## 21. Genesis and historical compatibility

Exact genesis contains no accepted Performance history.

Therefore:

```text
ComposeWithAcceptedHistory(exact genesis checkpoint, exact empty history)
    -> existing Production-bound v2 packet unchanged
```

No empty-history v3 packet exists.

History-aware nonempty history at exact genesis fails.

History-aware empty history at evolved state fails.

Historical APIs remain exact:

```csharp
DeterministicContextComposer.Compose(...)
E0ProductionContextContinuity.Compose(checkpoint)
E0TakeStateBinding.Bind(checkpoint, context, take)
```

The old one-argument Production Continuity API may still emit evolved v2 because Patch 0014 established that compatibility behavior.

The historical three-argument Take binding retains Patch 0014 v1/v2 behavior and rejects v3.

The new Full Ensemble path uses only the explicitly named history-aware methods.

## 22. History-aware Production Context continuity

`ComposeWithAcceptedHistory(checkpoint, history)`:

```text
validate checkpoint/current Production
-> validate opaque history internals
-> require internal history SceneId == checkpoint SceneId
-> require internal history CurrentStateHash == checkpoint StateHash
-> fresh Production Access exactly once
-> if history empty:
       require exact genesis
       compose existing internal v2
   else:
       require non-genesis
       project exact ordered history Entries to ContextRecentPerformance[]
       compose internal v3
-> require Packet/Trace SourceStateHash == checkpoint StateHash
-> return existing E0ProductionContextContinuityResult
```

No arbitrary recent-prose parameter.

Internal history failures normalize to sanitized `E0ContextContinuityException`.

## 23. History-aware Take binding

`BindWithAcceptedHistory(checkpoint, context, take, history)`:

- accepts exact genesis v2 only with exact initialized empty history;
- accepts v3 only with exact synchronized non-empty history;
- rejects v1;
- rejects evolved v2;
- rejects state/Scene/history mismatches.

For v3 it performs:

1. checkpoint/history structural and association validation;
2. fresh Production Access;
3. exact ordered recent semantic projection from history;
4. fresh internal v3 Context composition;
5. exact canonical structured-byte comparison;
6. exact canonical rendered-byte comparison;
7. ContextPacketId comparison;
8. StructuredContextHash comparison;
9. RenderedContextHash comparison;
10. SourceStateHash comparison;
11. inherited exact Take/source/opportunity/Scene/roster checks;
12. inherited exact State Authority snapshot proof.

SourceStateHash metadata alone is insufficient.

Internal history failures normalize to sanitized `E0CausalCommitException`.

## 24. Shared internal history validation/projector

One CausalCommit-layer internal helper validates/projects the opaque history for Context continuity, Take binding, and transition preconditions.

At minimum:

- history non-null;
- internal SceneId initialized;
- internal CurrentStateHash initialized;
- Entries initialized, never default;
- every item non-null;
- every source CharacterId initialized;
- exact existing Candidate VisibleText invariant;
- when projecting to current Context, every source Character resolves exactly once in current roster.

Identical repeated semantic items are valid and order-significant.

Do not attempt event replay from the semantic entries; they intentionally do not contain full causal payloads.

## 25. Structured v3 canonicalization

V3 preserves the v2 root order exactly:

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

Each recent item property order:

```text
sourceCharacterId
visibleText
```

Recent array order is causal append order and is never sorted.

Historical roster/record/relationship sorting remains unchanged.

V3 identity:

```text
StructuredContextHash = SHA256(canonical v3 UTF-8 bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Different source Character, exact text, causal order, or silence/non-silence changes Context identity even if durable Production record semantics are otherwise identical.

## 26. Render-v2

V3 uses:

```text
ensemble.e0.context.render.v2
```

`TrustedStateText` and `OpportunityText` retain their existing algorithms byte-for-byte. Only `RecentPerformanceText` gains non-empty behavior.

Exact non-silent form:

```text
[RECENT PERFORMANCES]
<source display name>:
[PERFORMANCE]
- <first source line>
  <continuation source line>

<next source display name>:
[PERFORMANCE]
- ...
```

Rules:

- accepted causal order;
- exactly one blank line between entries;
- no trailing LF;
- source display name resolved from current packet roster;
- exact source text, never trim/repair/paraphrase/summarize/reorder;
- same LF split / two-space continuation discipline as existing Context rendering;
- no Character IDs or causal/provenance IDs rendered.

Patch 0015 inherits Patch 0005 display-name-only provider-neutral rendering and does not add a display-name uniqueness law solely for recent history. Structured source attribution remains exact by CharacterId. Provider-specific disambiguation/framing remains later scope.

## 27. Silence rendering

Empty Candidate VisibleText is semantic silence.

Structured item:

```json
{"sourceCharacterId":"...","visibleText":""}
```

Rendered entry:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

No `[PERFORMANCE]` line or bullet body follows semantic silence.

Literal non-silent text equal to `[PERFORMANCE: SILENCE]` renders under ordinary non-silent bullet form, so the two cannot collide.

## 28. Recent Performance remains untrusted creative content

`RecentPerformanceText` remains separate from `TrustedStateText`.

Prompt-like fictional content gains no system authority because it appeared in accepted history.

Future provider request framing must preserve separate authority layers for at least:

```text
system/Performer contract
trusted current state
recent accepted fictional Performance
current opportunity
imported/user content where applicable
```

Patch 0015 does not define provider request bytes.

## 29. CharacterClaim and epistemic promotion remain deferred

A prior accepted Performance may contain a factual-sounding claim. Patch 0015 establishes only that Character-legible Performance occurred under the E0 common-history rule.

It does not create or promote:

- CharacterObservation;
- CharacterKnowledge;
- CharacterBelief;
- CharacterSuspicion;
- CharacterMemory;
- CharacterClaim;
- objective truth.

Patch 0014 `CharacterClaimDisclosureDeferred` remains exact for Production records.

## 30. Boundary-specific failure domains and privacy

Expected public failures normalize as:

```text
E0AcceptedPerformanceHistoryContinuity.*
    -> E0AcceptedPerformanceHistoryException

E0ProductionContextContinuity.ComposeWithAcceptedHistory(...)
    -> E0ContextContinuityException

E0TakeStateBinding.BindWithAcceptedHistory(...)
    -> E0CausalCommitException
```

Historical methods retain historical failure contracts.

Expected failure representation must not contain:

- Performance VisibleText;
- Context/private record prose;
- mutation text;
- provider/raw payload;
- credentials/secrets;
- unknown untrusted snippets.

Expected upstream domain failures may be retained only where their existing structural messages are already sanitized; do not concatenate untrusted upstream text. Unexpected programming/runtime failures are not relabeled as ordinary contract rejection.

## 31. Historical canonical authority remains immutable

Historical Context and Production identities remain exact:

```text
Context v1 Missing Raft / VOSS
structured bytes = 2569
StructuredContextHash = bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b
rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88

Context v2 genesis / VOSS
Production StateHash = 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
structured bytes = 2655
StructuredContextHash = 27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
ContextPacketId = CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
rendered bytes = 1905
RenderedContextHash = ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88

historical v1-source Patch 0012 postcommit StateHash
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30

historical v1-source Patch 0013 Opportunity StateHash
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310

Context v2 evolved / MARLOWE at historical dc7e state
structured bytes = 3456
StructuredContextHash = 9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
rendered bytes = 2389
RenderedContextHash = 9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

No Patch 0015 code may alter those historical bytes/hashes.

## 32. Patch 0015 live oracle lineage

Historical Patch0012 test support constructs the oracle Candidate from fixture-derived v1 Context. Patch 0015 live genesis instead uses the exact Production-bound VOSS v2 Context.

Because Candidate content identity includes `ContextPacketId`, and proposal identity includes Candidate content identity, static independent reconstruction already proves the following intermediate values for otherwise identical `"No."` / empty-control / Pressure-add semantics:

### Historical v1-source intermediates

```text
CandidateContentHash
cced4de8efbaf3bf707c92192cdbe0f084a46156205c4f88c326e5f7535dc153

ProposalContentHash
16f20511ddd9655640d532be37b6431e9e9fa8abc07cda3033bf7dba5ed8e248
```

### Patch 0015 v2-source intermediates

Source ContextPacketId:

```text
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
```

Independent canonical Candidate-content reconstruction yields:

```text
CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1
```

Independent State Authority proposal-content reconstruction using the same Pressure mutation yields:

```text
ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3
```

These are static architecture-audit derivations only, not machine validation.

Therefore the first approved live chain must derive new values:

```text
Patch0015LivePostCommitStateHash = <NEW; NOT YET FROZEN>
Patch0015LiveOpportunityStateHash = <NEW; NOT YET FROZEN>
```

and they must differ respectively from historical `05703456...` and `dc7e169f...` absent cryptographic collision.

The unchanged Director semantics with empty address/nomination and genesis routing history are expected to select `MARLOWE`; the independent oracle must prove that alongside the new StateHashes.

The first v3 oracle then uses:

```text
genesis exact Production-bound VOSS v2 Context
VOSS accepted VisibleText = "No."
same canonical Pressure mutation/materialization semantics as historical oracle when practical
new live postcommit StateHash
new live MARLOWE Opportunity StateHash
accepted history = [ { VOSS, "No." } ]
MARLOWE v3 SourceStateHash = new live Opportunity StateHash
```

Before native validation, independent oracle evidence must:

1. reproduce all historical Section 31 hashes;
2. reproduce historical Candidate/proposal intermediate hashes;
3. derive and freeze the new v2-source Candidate/proposal intermediates above;
4. independently derive new live postcommit StateHash;
5. independently derive selected MARLOWE + new live Opportunity StateHash;
6. independently derive first-v3 structured byte count/hash/ContextPacketId;
7. independently derive render-v2 byte count/hash;
8. prove multi-entry append order;
9. prove semantic silence differs from empty history and literal silence-marker text.

## 33. Existing downstream semantic contracts

Current source inspection confirms the Candidate parser and Integrity/State Interpretation association layers consume Context semantic identity/subject/opportunity rather than hard-coding v1/v2 schema tokens.

Patch 0015 must prove by tests that v3 traverses the existing public:

```text
Performer Candidate
-> Integrity
-> State Interpretation
-> State Authority
-> E0Take.Bind
```

without semantic redesign.

The deliberate v3 live version gate is the new `BindWithAcceptedHistory` boundary.

If implementation discovers an additional hidden schema gate, patch only the smallest version-assumption surface; do not redesign the pipeline.

## 34. Narrow inherited test adaptations

Historical evidence/docs remain immutable. Only executable tests whose historical premise is explicitly superseded by Patch 0015 may change.

### A. Patch 0005 Context “no recent history type” guard

File:

```text
tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs
```

Narrowly evolve `ContextBoundary_DoesNotExposeProvenanceAccessDecisionsOrRecentHistoryType` because Patch 0015 intentionally adds exactly one public Context Performance type: `ContextRecentPerformance`.

Preserve all enduring assertions that ContextRecord/Relationship expose no provenance, ContextPacket exposes no Access decisions, trace exposes no denied rows, and provider-neutral rendering hides internal IDs/provenance.

Add/retain closed-construction coverage so `ContextRecentPerformance` has no public constructor/setter.

### B. Patch 0012 CausalCommit exact public namespace guard

File:

```text
tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012ContractAuditTests.cs
```

Narrowly update the exact CausalCommit public-type list to add only:

```text
E0AcceptedPerformanceHistory
```

Do not weaken the Production namespace exact list, no-persistence/provider/Windows/hardware dependency tests, no state-only commit API, or no rewrite/delete API.

### C. Patch 0014 temporary Context/Continuity “not yet” guard

File:

```text
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ContractAuditTests.cs
```

Narrowly evolve only superseded assertions:

- `ContextPacket.RecentPerformances` now exists;
- exactly one `ContextRecentPerformance` public Context Performance type exists;
- exactly three v3/render-v2 contract constants are added;
- historical `Compose(checkpoint)` remains exact while `ComposeWithAcceptedHistory(...)` is additionally present.

All unrelated Patch 0014 public/authority restrictions remain exact.

### D. Patch 0014 Continuity namespace/method exact guard

File:

```text
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextContinuityTests.cs
```

Narrowly update `ContinuityPublicSurface_IsClosedAndMinimal` so the public Continuity namespace additionally contains exactly:

```text
E0AcceptedPerformanceHistoryContinuity
E0AcceptedPerformanceHistoryException
```

Replace the historical `.Single()` assumption over `E0ProductionContextContinuity` public methods with exact signature assertions for:

```text
Compose(ProductionStateCheckpoint)
ComposeWithAcceptedHistory(ProductionStateCheckpoint, E0AcceptedPerformanceHistory)
```

Preserve old Compose return type/behavior and closed result/exception construction.

### E. Patch 0014 ContextPacket reflection clone helper

File:

```text
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextBindingTests.cs
```

Its private `Clone(...)` invokes the sole non-public `ContextPacket` constructor positionally. Adding `RecentPerformances` requires only a test-helper adaptation to pass `source.RecentPerformances` at the new constructor position. This is not a contract weakening and must preserve all existing tamper tests.

Current source search found no other direct `typeof(ContextPacket).GetConstructors(...)` production-test construction site requiring the same adaptation.

## 35. Required implementation tests if approved

At minimum:

### Public/history shape
- exact genesis initialization succeeds; non-genesis initialization rejects;
- opaque history has zero public constructors, setters, properties, or declared methods;
- internal SceneId/CurrentStateHash/Entries exist exactly as designed;
- internal Entries type exactly `ImmutableArray<ContextRecentPerformance>`;
- no public history entry/count/transcript/hash surface;
- history transition exception only in Continuity with no public constructor;
- internal invariant exception nonpublic.

### API disambiguation/live precommit
- historical `Compose` one-argument signature unchanged;
- live exact name/signature `ComposeWithAcceptedHistory`;
- historical `Bind` three-argument signature unchanged;
- live exact name/signature `BindWithAcceptedHistory`;
- exact empty-history genesis v2 passes live binder before commit;
- v1 and evolved legacy v2 fail live binder;
- exact v3 with synchronized non-empty history passes;
- Full Ensemble sequence uses successful live binder before commit.

### Commit advancement
- exact genesis-v2-source live commit appends once;
- new postcommit anchor is fresh replay StateHash;
- stale/foreign/tampered event/history rejects;
- after one accepted item, exact v3-source next commit appends;
- separately established history-omitting evolved-v2 commit cannot enter RecordCommit live projection;
- changed/dropped/reordered/extra history changes expected ContextPacketId and blocks append;
- Accepted zero-mutation/all-consequence-Rejected append;
- nonaccepted/failed paths do not;
- appended semantic item contains only subject + exact VisibleText;
- failed RecordCommit returns no advanced token and caller retains previous synchronized state/history pair.

### Opportunity advancement
- exact established event replays through canonical Opportunity replay;
- source commit/result StateHash/history anchor exact;
- last semantic item matches source Performance subject+text;
- pre/post routing-history count induction exact;
- no Performance append;
- stale/foreign routing history/event rejects;
- failed RecordOpportunity leaves prior no-opportunity postcommit pair as supported live pair;
- successful RecordOpportunity anchor equals fresh Opportunity StateHash before adoption.

### Multi-turn
- two/three accepted commits preserve exact causal semantic order;
- identical repeated items remain distinct;
- same Character may recur;
- self history retained;
- history remains synchronized across staged commit/adopt and Opportunity/adopt cycles;
- earlier accepted text survives even without durable consequence;
- no provider-session memory required.

### Version/canonical compatibility
- all historical v1/v2 byte/hash oracles exact;
- old Compose/Bind behavior exact;
- history-aware genesis emits unchanged v2;
- nonempty history emits v3;
- evolved empty history rejects;
- default RecentPerformances rejects all packet versions;
- exact v1/v2/v3 matrix and hybrid rejection;
- exact root/item order;
- historical rendered hashes unchanged.

### Disclosure/epistemic separation
- recent DTO only source CharacterId + VisibleText;
- no typed control/causal IDs/hashes/private source Context/provider fields;
- CharacterClaim still denied from Production Access;
- no CharacterObservation/Knowledge/Belief/Memory materialization;
- factual-sounding Performance remains historical speech/action, not truth;
- same event may coexist in recent Performance and separately trusted durable consequence without deduplication.

### Rendering
- exact single/multi entry format/order/blank lines;
- multiline LF/two-space continuation;
- exact Candidate Unicode/NFC grammar reused;
- silence exact;
- literal `[PERFORMANCE: SILENCE]` noncollision;
- no trailing LF;
- v2/v1 render-v1 bytes unchanged;
- duplicate display names do not change structured CharacterId attribution; no new uniqueness law.

### Downstream Take proof
- v3 Context passes existing Candidate -> Integrity -> Interpreter -> Authority -> Take public semantics;
- exact synchronized v3 live binder succeeds;
- stale/altered history and tampered structured/rendered bytes/hashes fail;
- SourceStateHash alone cannot rescue mismatch;
- second v3-source Accepted Take commits and RecordCommit independently rechecks exact source Context.

### Failure/privacy
- expected transition failure -> `E0AcceptedPerformanceHistoryException`;
- expected live Context failure -> `E0ContextContinuityException`;
- expected live Take failure -> `E0CausalCommitException`;
- no creative/private/provider/secret text in expected public failure representation;
- unexpected programming failures not swallowed.

### Oracle/determinism/dependency
- independently reproduce historical hashes;
- pin independent v2-source Candidate hash `6ce2a98d...` and proposal hash `ac0f7a91...`;
- derive/freeze new live postcommit and Opportunity hashes and prove they differ from historical lineage;
- prove MARLOWE remains selected;
- derive/freeze first v3 structured/rendered bytes/hashes;
- repeated byte determinism and culture invariance;
- history order intentionally order-sensitive while unrelated source collections retain existing canonical invariance;
- no Context -> higher-layer dependency;
- no CausalCommit -> Opportunity/Continuity dependency;
- Production/Opportunity semantics/public shapes unchanged;
- no provider/network/filesystem/clock/random/Windows/NPU dependency.

## 36. Expected implementation surface

If approved, patch-first source should remain approximately:

```text
src/Ensemble.E0.Core/Context/
    ContextRecentPerformance
    ContextPacket.RecentPerformances
    v3/render-v2 constants
    v3 packet/version canonicalization
    internal v3 composer + recent rendering

src/Ensemble.E0.Core/CausalCommit/
    opaque E0AcceptedPerformanceHistory
    internal history invariant/projector
    BindWithAcceptedHistory

src/Ensemble.E0.Core/Continuity/
    E0AcceptedPerformanceHistoryContinuity
    E0AcceptedPerformanceHistoryException
    ComposeWithAcceptedHistory

src/Ensemble.E0.Core/Performer/
    at most private->internal reuse/forwarding of existing exact VisibleText validation

focused Patch 0015 tests
narrow inherited test adaptations listed in Section 34
```

Preferred no-change semantics/source:

- Fixture JSON/dialect;
- Access policy;
- Production projection and transition logic;
- Candidate public contract/parser behavior;
- Director policy;
- Opportunity canonicalizer/authority public semantics;
- Integrity semantics;
- State Interpreter semantics;
- State Authority semantics;
- E0Take semantics;
- Harness provider/runtime execution.

If implementation requires semantic changes to those preferred no-change areas rather than narrow version/wiring support, stop and reopen architecture.

## 37. Complexity and memory

Let:

```text
R = retained Production records
A = permitted current records
B = permitted current-state bytes
H = accepted Performance count in current E0 Scene
P = total accepted VisibleText bytes
```

Expected explicit-boundary complexity:

```text
history validation        O(H + P where text validation is required)
history-aware Context     O(R + A log A + B + H + P)
history-aware Take proof  fresh history-aware Context + inherited State Authority proof
RecordCommit              fresh history-aware Context + canonical commit replay + immutable append
RecordOpportunity         canonical Opportunity replay + O(H) history validation/coupling
```

`ImmutableArray` append copies O(H) references/items; repeated full-Scene rendering can make total long-Scene work superlinear. This is accepted for bounded E0 reference behavior; product-scale storage/retrieval/summarization is not claimed.

Do not invent an E0 turn/history ceiling without separate run-protocol authority.

## 38. ARM64/battery suitability

Patch 0015 adds synchronous deterministic CPU/memory work only at explicit Context/Take/commit/Opportunity/history boundaries.

No:

- idle polling;
- background loop;
- filesystem requirement;
- network/provider call;
- GPU/NPU work;
- Windows AI API;
- wall clock/random source;
- emulation path.

This is compatible by design with the current native ARM64 deterministic Core/Harness approach. It is not measured power/performance evidence.

## 39. Explicit non-scope

Patch 0015 does not implement:

- general CharacterObservation generation;
- general location/hearing/attention/concealment/private Performance observation;
- CharacterClaim Context disclosure;
- Knowledge/Belief/Memory promotion from recent Performance;
- history relevance/windowing/summarization;
- cross-Scene history retrieval;
- provider/model invocation;
- provider request/attempt provenance;
- retry/spend/cancellation/streaming;
- model-assisted Integrity or State Interpreter call;
- full Scene-loop orchestration;
- Run manifest/store;
- durable causal-event persistence/recovery;
- full replay from genesis;
- branch/canon/retcon/rehearsal;
- World Resolver;
- WinUI;
- Windows AI Foundry/NPU;
- MSIX/WACK/Store certification.

## 40. Proposal 0.14 resolved decisions

1. Patch 0015 is Accepted Performance History + Context Continuity, not generic PerformerInput, cognition, or Observation.
2. History is a truly opaque public token with zero public properties.
3. Internal history stores only `ContextRecentPerformance` semantics plus Scene/StateHash synchronization metadata.
4. No per-entry causal IDs/hashes/control and no standalone history hash.
5. Full Ensemble live path requires `BindWithAcceptedHistory` before commit.
6. `RecordCommit` independently recomposes exact accepted-history source Context and canonical-replays the established commit before appending.
7. Commit result is staged and adopted only after history advancement succeeds.
8. `RecordOpportunity` canonical-replays the established transition and couples routing-history count with accepted-history count.
9. Opportunity result is staged and adopted only after history advancement succeeds.
10. New live Context/Take methods have explicit distinct names; historical methods remain exact.
11. Public history-transition exception belongs in Continuity; Context/Take preserve existing public exception domains.
12. Complete current-Scene accepted Performance order is the E0 unoptimized reference history window.
13. `copresent-trio.v1` gains only this new E0 common Character-legible recent-Performance rule; it is not generalized into a perception engine.
14. Self history is included; no unfrozen forgetting rule.
15. Recent Performance remains untrusted creative content and never becomes truth/Observation/epistemic state merely by inclusion.
16. Trusted durable consequence and recent Performance remain independent layers and may coexist.
17. Context v3 adds only recent semantic items plus a new schema/composition/render contract; historical v1/v2 bytes stay immutable.
18. Context trace remains unchanged.
19. Duplicate display-name behavior remains inherited; structured attribution stays exact by CharacterId.
20. Historical Patch0012/Patch0013 state hashes are v1-source lineage and are not Patch0015 live hashes.
21. The independently reconstructed v2-source Candidate/Proposal hashes are `6ce2a98d...` / `ac0f7a91...`; downstream live StateHashes remain to be independently derived before native validation.
22. Five inherited test surfaces are currently identified for narrow adaptation: Patch0005 recent-history absence, Patch0012 CausalCommit public-type list, Patch0014 temporary history/constants/API absence, Patch0014 Continuity namespace/method exactness, and Patch0014 ContextPacket reflection clone helper.

## 41. Recursive audit order

Every material correction restarts:

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

## 42. Proposal 0.14 audit status

Run a complete fresh pass. Close only if it finds:

```text
0 material correctness corrections
0 consistency corrections
0 authority corrections
0 accepted-history integrity corrections
0 disclosure/privacy corrections
0 epistemic corrections
0 dependency corrections
0 version/canonical corrections
0 multi-turn corrections
0 failure-behavior corrections
0 scope corrections
0 worthwhile test improvements
0 worthwhile simplifications
0 hygiene/ARM64/vision/evidence corrections
```

No implementation, approval evidence, implementation handoff, `CURRENT_STATE.md` update, or promotion is permitted until a complete pass is clean and the user explicitly approves the resulting blueprint.
