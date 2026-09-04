# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.15 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden
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

That is insufficient for Full Ensemble E0 because accepted Character-legible historical texture must survive independently invoked Performers without being misrepresented as durable Production state or delegated to provider conversation memory.

Patch 0015 adds only the deterministic accepted-Performance history projection and Context continuity needed to close that seam. It does not call a provider/model and does not implement the full Scene loop.

## 2. Source-grounded correction history

The recursive investigation rejected or corrected these directions before Proposal 0.15:

1. **Generic Character Context Consumption / PerformerInput — rejected.** Patch 0006 already freezes `ContextPacket -> Performer -> CandidatePerformance`; a second generic input layer duplicates authority.
2. **Character Interpretation — rejected.** It prematurely implies cognition/belief/memory authority.
3. **Observation-first — deferred.** General observation eligibility requires later location/hearing/attention/private-performance semantics absent from current E0.
4. **Immediate-one-Performance-only history — rejected.** Accepted historical texture can remain causally relevant without durable-state promotion; one-turn-only history would lose Scene continuity or encourage state explosion.
5. **Provider-attempt provenance — deferred.** Provider request/attempt/retry/spend/raw-output provenance belongs after semantic accepted-history continuity exists.
6. **Result-wrapper trust — removed.** History advancement reuses canonical `DeterministicCausalCommit.Replay(...)` and `DeterministicOpportunityAuthority.Replay(...)` instead of trusting result wrappers or duplicating transition algorithms.
7. **Redundant projected causal fields — removed.** History does not copy CommitId, TakeId, per-entry StateHashes, ContextPacketId, Candidate hash, typed control, or consequence packages.
8. **Live source-Context omission — closed.** `RecordCommit(...)` freshly recomposes the history-aware semantic source Context and requires the committed Performance subject/ContextPacketId to match before canonical replay/append.
9. **Public history-entry/count API — removed.** Character-safe history is inspectable only through `ContextPacket.RecentPerformances`; the synchronization token is opaque.
10. **Failure-domain ownership — corrected.** History transition errors are Continuity-owned; low history invariant errors are internal-only; history-aware Context and Take paths preserve existing public exception domains.
11. **Precommit live-path law — added.** Full Ensemble execution must successfully bind the Take against exact accepted history before commit; postcommit history projection independently rechecks the semantic source association and canonical event.
12. **Live API ambiguity — removed.** New methods are explicitly `ComposeWithAcceptedHistory(...)` and `BindWithAcceptedHistory(...)`, not overloads named like historical history-omitting APIs.
13. **Commit adoption — fail-closed.** A future orchestrator stages a commit result and adopts the postcommit Production/history pair only after `RecordCommit(...)` succeeds.
14. **Opportunity adoption — fail-closed.** A future orchestrator stages Opportunity establishment and adopts the opportunity-bearing Production/history/OpportunityHistory triple only after `RecordOpportunity(...)` succeeds.
15. **Inherited reflection-suite boundary — corrected.** Later public additions necessarily supersede several historical exact-absence/public-surface tests; this blueprint enumerates each identified narrow adaptation.
16. **Live oracle lineage — corrected.** Historical Patch 0012/0013 StateHashes were generated from fixture-derived Context v1. Patch 0015 live genesis uses Production-bound v2; Candidate and causal identities bind ContextPacketId, so live postcommit and Opportunity StateHashes must be newly derived.
17. **History token public state identity — removed.** `SceneId` and `CurrentStateHash` are synchronization internals, not information external callers need. The public token therefore has zero public properties.
18. **Character-legible text dependency inversion — corrected in Proposal 0.15.** The exact Patch 0006 VisibleText grammar cannot be reused by Context by calling upward into Performer. One neutral internal Domain-layer invariant now owns that grammar; Performer delegates to it without changing public Candidate behavior, while Context/CausalCommit reuse it without reversing dependency direction.
19. **Postcommit proof overclaim — corrected in Proposal 0.15.** `BindWithAcceptedHistory(...)` is the sole full precommit proof of exact structured **and rendered** source Context. `RecordCommit(...)` can independently re-prove exact structured semantic source association through ContextPacketId plus canonical event replay, but the causal event does not retain historical RenderedContext bytes and Patch 0015 must not claim that it does.
20. **Live oracle input ambiguity — removed in Proposal 0.15.** The new live reference branch uses an exact, frozen set of Candidate/Take/Commit/materialization/policy inputs so downstream StateHashes are reproducibly derivable rather than described as “same when practical.”

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
- Patch 0005 reserved plural `recentPerformances` and deferred non-empty item schema/order/population to a later accepted-history/commit slice.
- Patch 0012 owns atomic accepted causal commit and canonical commit replay.
- Patch 0013 owns effective Opportunity, routing history, canonical Opportunity replay, and Opportunity StateHash chaining.
- Patch 0014 owns exact Production-backed Access/Context continuity and exact precommit source Context proof while keeping recent Performance empty.
- Fixture Dialect v1 has exactly one Scene, exactly three roster Characters, and observation token `ensemble.e0.copresent-trio.v1`; that token was structural/dialect authority, not a general perception engine.

## 4. Exact Patch 0015 question

> Can Ensemble maintain a closed deterministic projection of successfully committed Character-legible Performances for the current bounded E0 Scene, synchronize it through the exact commit/Opportunity StateHash chain, prove precommit that a live Accepted Take is bound to the exact accumulated history-aware Context, independently prove postcommit that the established event refers to the exact accumulated structured semantic Context and canonical parent transition, and compose accepted history into later Character Context without turning Performance prose into projected truth, Observation, Knowledge, Memory, Belief, Claim, provider state, or a competing event store?

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
Domain.CharacterLegibleTextInvariants
    -> Context + Performer + CausalCommit internal validation

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

- Domain text invariants depend only on BCL text/Unicode primitives.
- Context never depends on Performer, CausalCommit, Opportunity, or Continuity.
- Performer delegates Candidate VisibleText grammar to the neutral Domain invariant; public Candidate semantics/messages remain unchanged.
- CausalCommit may continue depending on lower Context/Domain types.
- Accepted-history data/invariants live low enough for `E0TakeStateBinding` without introducing CausalCommit -> Opportunity/Continuity.
- History advancement alone lives in Continuity because it observes Context continuity, CausalCommit replay, and Opportunity replay.
- Opportunity remains unchanged and continues to depend on CausalCommit, never reverse.
- Production projection/state transition semantics remain unchanged.

## 6. Neutral Character-legible text invariant

Patch 0006 currently owns the exact Candidate VisibleText grammar privately inside Performer. Patch 0015 needs the same grammar for accepted-history DTO validation and v3 canonicalization without introducing `Context -> Performer`.

Implementation must extract that algorithm into one internal Domain-layer owner, conceptually:

```text
Ensemble.E0.Core.Domain.CharacterLegibleTextInvariants
```

The helper remains internal and has no public API. It preserves exactly the existing grammar:

- null invalid;
- empty string valid semantic silence;
- UTF-16 surrogate structure valid;
- non-empty text already Unicode NFC;
- Control characters forbidden except TAB U+0009 and LF U+000A;
- non-empty text contains at least one display-bearing Unicode scalar under the exact Patch 0006 category rule.

The neutral helper must expose enough internal failure classification for callers to map failures into their own exception domains without embedding input text in messages.

`PerformerCandidateContract` delegates to the neutral helper and preserves its current public exception type and exact externally tested semantics. No Candidate JSON, CandidatePerformance, control, ID, or hash contract changes.

Context/history paths reuse the same helper and normalize expected failure into their owning sanitized exception domains.

No second handwritten VisibleText validator is permitted.

## 7. Exact public and internal surface

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

## 8. History semantics

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

## 9. Internal history payload is Character-safe semantics only

History stores exactly:

```text
ImmutableArray<ContextRecentPerformance>
```

Each item stores only:

```text
SourceCharacterId
VisibleText
```

No per-entry CommitId, TakeId, parent/result StateHash, ContextPacketId, CandidateContentHash, Candidate identity contract, typed control, State Authority data, materializations, provider data, or epistemic classification.

The internal `CurrentStateHash` anchor binds synchronization to the authoritative transition chain. History does not partially duplicate causal event provenance.

## 10. StateHash synchronization is not history authentication

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

## 11. Initialization

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

## 12. Approved Full Ensemble live sequence

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
         sole full structured+rendered precommit source proof
    -> DeterministicCausalCommit.Commit
         result staged; not yet adopted into live pair
    -> E0AcceptedPerformanceHistoryContinuity.RecordCommit
         fresh structured semantic source identity proof
         + canonical commit replay + one append
    -> only then adopt postcommit Production + accepted-history pair
    -> DeterministicOpportunityAuthority.Establish
         result staged; not yet adopted into live triple
    -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
         canonical Opportunity replay + routing/history coupling
    -> only then adopt opportunity-bearing Production
       + accepted history + OpportunityHistory triple
```

Historical `Compose(checkpoint)` and `Bind(checkpoint, context, take)` remain regression/compatibility APIs but are not approved Full Ensemble history-bearing live calls.

For non-empty history, historical Bind rejects v3, so a normally constructed v3 commit binding must come through `BindWithAcceptedHistory`. At exact empty-history genesis, v2 remains historically compatible, but the approved Full Ensemble path still uses the history-aware binder.

## 13. Precommit proof authority

`BindWithAcceptedHistory(checkpoint, context, take, history)` is the authoritative full live precommit proof.

It must prove:

- exact synchronized history/state/Scene association;
- exact fresh Production Access;
- exact history-aware Context recomposition;
- canonical structured bytes;
- canonical rendered bytes;
- ContextPacketId;
- StructuredContextHash;
- RenderedContextHash;
- SourceStateHash;
- inherited Take/source/opportunity/Scene/roster association;
- inherited exact State Authority snapshot equivalence.

Only after this proof may the approved live path call `DeterministicCausalCommit.Commit`.

## 14. Commit advancement and postcommit proof limit

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

The content-addressed ContextPacketId proves exact **structured semantic** source Context association, including SourceStateHash and accepted recent-performance order/content. Canonical event replay independently proves the established event against the authoritative parent state.

`RecordCommit` does **not** possess the historical source `RenderedContext` object or its bytes and therefore does not independently re-compare the source rendered bytes. That full rendered proof belongs to the precommit `BindWithAcceptedHistory` boundary. Patch 0015 must never claim otherwise.

Provider transport/request bytes remain unproven and deferred.

A caller must not adopt a staged postcommit `ProductionState` into the Full Ensemble live pair unless `RecordCommit` succeeds for that exact parent/event.

## 15. Opportunity advancement

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
genesis:           OpportunityHistory = 1, PerformanceHistory = 0
after commit:      OpportunityHistory = H, PerformanceHistory = H
after opportunity: OpportunityHistory = H+1, PerformanceHistory = H
```

`DeterministicOpportunityAuthority.Replay` already revalidates source commit/history, recomputes Director selection, recomputes canonical Opportunity StateHash, and advances routing history. Patch 0015 must not duplicate those algorithms.

A caller must not adopt a staged opportunity-bearing Production/OpportunityHistory result into the Full Ensemble live triple unless `RecordOpportunity` succeeds.

## 16. Non-effective paths

No history entry is created for:

- Rejected or Alternate Take;
- Integrity Reject / RequestAnotherTake;
- unresolved State Authority review;
- malformed Candidate;
- provider error/refusal/timeout/cancellation;
- partial stream;
- failed causal commit;
- accepted event whose structured source Context omitted/mismatched required history;
- failed Opportunity establishment/replay;
- diagnostics/technical text.

A successfully Accepted zero-mutation Take appends its Performance.

A successfully Accepted Take whose proposed consequences are all authoritatively Rejected also appends its Performance because the accepted Character-legible Performance itself entered causal history even when no durable record changed.

## 17. Performance history and durable consequence are distinct layers

The same accepted causal event may later contribute independently to:

```text
RecentPerformances
    exact Character-legible historical occurrence

TrustedStateText
    separately authorized, committed, active, Access-permitted current consequence
```

Do not deduplicate, merge, or suppress one layer merely because prose overlaps.

Historical occurrence != durable consequence.

No durable consequence != no accepted Performance.

## 18. E0 history eligibility and Observation boundary

Fixture Dialect v1 fixes exactly:

```text
ensemble.e0.copresent-trio.v1
```

and Missing Raft fixes three co-present Characters in one bounded E0 Scene.

Patch 0015 is the first higher-layer contract to give that token this narrowly executable recent-Performance meaning:

> Every successfully committed `CandidatePerformance.VisibleText` in the current E0 Scene is common Character-legible recent Performance content for every current roster Character under `ensemble.e0.copresent-trio.v1` + the Patch 0015 accepted-history composition contract.

This E0 reference rule applies only to successful Accepted committed Performance, the current E0 Scene, current roster, and `VisibleText`, including semantic silence.

It does not expose typed address/nomination control, hidden reasoning, provider diagnostics, creator-only Production information, inferred consequences, or truth/Knowledge/Belief/Memory/Claim state.

This does not reinterpret historical v1/v2 packets, create `CharacterObservation` records, or establish a global rule that co-presence always means complete perception. Future private/spatial/inaudible/concealed Performance or multiple observation contracts require explicit new Observation authority.

Selective recipient perception is outside current E0 reference scope.

## 19. Self history

When a Character later receives another Opportunity, accepted recent history includes that Character’s own earlier accepted Performances as well as the others'. This is common E0 Scene-performance history; suppressing self history would introduce an unfrozen forgetting rule.

This does not materialize durable CharacterMemory.

## 20. Current-Scene history window and order

For E0 v3:

```text
recentPerformances = every accepted history item in exact append order
```

No sorting by Character/ID/time, deduplication, recency count, relevance scoring, truncation, summarization, paraphrase, token budgeting, or model compression.

Patch 0005 explicitly deferred causal ordering to accepted-history authority, and E0 excludes context optimization. A later product composer may window/retrieve/summarize already-eligible history only under a new versioned composition contract and controlled comparison.

No E0 history/turn quota is invented here because current run-protocol authority does not freeze one.

## 21. Context recent semantic DTO

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
- VisibleText is exact accepted Candidate semantics and validates through the one neutral Character-legible-text invariant;
- VisibleText may be empty only as already-defined semantic silence;
- no CommitId/TakeId/StateHash/ContextPacketId/Candidate hash/control/provenance enters the Character semantic DTO.

## 22. Context contracts and version matrix

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

Any hybrid schema/composition/render/recent shape fails packet canonicalization and exact binding. Standalone rendered serialization remains a rendering-object operation; cross-layer schema/render compatibility is enforced where the packet is available.

## 23. Genesis and historical compatibility

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

The new Full Ensemble path uses only explicitly named history-aware methods.

## 24. History-aware Production Context continuity

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
       project exact ordered history Entries
       compose internal v3
-> require Packet/Trace SourceStateHash == checkpoint StateHash
-> return existing E0ProductionContextContinuityResult
```

No arbitrary recent-prose parameter.

Internal history failures normalize to sanitized `E0ContextContinuityException`.

## 25. History-aware Take binding

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

## 26. Shared internal history validation/projector

One CausalCommit-layer internal helper validates/projects the opaque history for Context continuity, Take binding, and transition preconditions.

At minimum:

- history non-null;
- internal SceneId initialized;
- internal CurrentStateHash initialized;
- Entries initialized, never default;
- every item non-null;
- every source CharacterId initialized;
- exact neutral Character-legible VisibleText invariant;
- when projecting to current Context, every source Character resolves exactly once in current roster.

Identical repeated semantic items are valid and order-significant.

Do not attempt event replay from semantic entries; they intentionally do not contain full causal payloads.

## 27. Structured v3 canonicalization

V3 preserves v2 root order exactly:

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

Recent array order is causal append order and is never sorted. Historical roster/record/relationship sorting remains unchanged.

V3 identity:

```text
StructuredContextHash = SHA256(canonical v3 UTF-8 bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Different source Character, exact text, causal order, or silence/non-silence changes Context identity even if durable Production record semantics are otherwise identical.

## 28. Render-v2

V3 uses:

```text
ensemble.e0.context.render.v2
```

`TrustedStateText` and `OpportunityText` retain existing algorithms byte-for-byte. Only `RecentPerformanceText` gains non-empty behavior.

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

## 29. Silence rendering

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

## 30. Recent Performance remains untrusted creative content

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

## 31. CharacterClaim and epistemic promotion remain deferred

A prior accepted Performance may contain a factual-sounding claim. Patch 0015 establishes only that Character-legible Performance occurred under the E0 common-history rule.

It does not create or promote CharacterObservation, CharacterKnowledge, CharacterBelief, CharacterSuspicion, CharacterMemory, CharacterClaim, or objective truth.

Patch 0014 `CharacterClaimDisclosureDeferred` remains exact for Production records.

## 32. Boundary-specific failure domains and privacy

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

Expected failure representation must not contain Performance VisibleText, Context/private record prose, mutation text, provider/raw payload, credentials/secrets, or unknown untrusted snippets.

Expected upstream domain failures may be retained only where existing structural messages are already sanitized; do not concatenate untrusted upstream text. Unexpected programming/runtime failures are not relabeled as ordinary contract rejection.

## 33. Historical canonical authority remains immutable

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

## 34. Exact Patch 0015 live oracle branch

The Patch 0015 live oracle deliberately starts a **separate reference branch from exact Missing Raft genesis**. It reuses historical Patch 0012 oracle IDs and mutation semantics solely to isolate the effect of changing the source Context from v1 to Production-bound v2. The reused IDs do not collide because this is a separate genesis-derived reference chain, not continuation of the historical chain.

Exact inputs:

```text
Genesis StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Source Context
VOSS Production-bound v2
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

Candidate VisibleText
"No."

Candidate control
addressedCharacterIds = []
nominatedCharacterId = null

TakeId
TAKE-PATCH-0012-ORACLE

CommitId
COMMIT-PATCH-0012-ORACLE

Approved mutation
Pressure Add
text = "Pressure increases."
supportingRecordIds = []

State Authority policy
autoApproveDomains = [ Pressure ]
review choices = []

Materialized RecordId
PRESSURE-PATCH-0012-ORACLE
```

Static independent reconstruction already proves:

```text
v2-source CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1

v2-source ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3
```

Historical v1-source intermediates remain:

```text
CandidateContentHash
cced4de8efbaf3bf707c92192cdbe0f084a46156205c4f88c326e5f7535dc153

ProposalContentHash
16f20511ddd9655640d532be37b6431e9e9fa8abc07cda3033bf7dba5ed8e248
```

Because all mutation/materialization/Production inputs are otherwise identical, the Patch 0015 live postcommit **result Production projection bytes must equal the historical Patch 0012 oracle postcommit projection bytes exactly**. The new postcommit StateHash differs only because the canonical causal commit payload binds the v2 ContextPacketId and changed Candidate/proposal content hashes.

The first Opportunity oracle uses the same empty address/nomination control and genesis routing history. Existing least-intervention semantics must again select `MARLOWE`.

Because selected Character and underlying postcommit projection are otherwise identical, the Patch 0015 live Opportunity **result Production projection bytes must equal the historical Patch 0013 MARLOWE opportunity projection bytes exactly**. Its StateHash is nevertheless new because its parent StateHash is the new live postcommit hash.

Therefore independently derive and freeze before native validation:

```text
Patch0015LivePostCommitStateHash = <NEW; NOT YET FROZEN>
Patch0015LiveOpportunityStateHash = <NEW; NOT YET FROZEN>
```

Both must differ respectively from historical `05703456...` and `dc7e169f...` absent cryptographic collision.

The first v3 oracle then uses:

```text
accepted history = [ { VOSS, "No." } ]
MARLOWE v3 SourceStateHash = Patch0015LiveOpportunityStateHash
```

For that first v3 state, current Production records/roster/subject/opportunity match the historical evolved MARLOWE reference except for StateHash lineage. Therefore the v3 `TrustedStateText` must equal the historical evolved MARLOWE v2 `TrustedStateText` byte-for-byte. V3 adds accepted history only through structured `recentPerformances`, render-v2 identity, and `RecentPerformanceText`.

Before native validation, independent oracle evidence must:

1. reproduce all historical Section 33 hashes;
2. reproduce historical Candidate/proposal intermediate hashes;
3. reproduce the v2-source Candidate/proposal hashes above;
4. prove live postcommit projection bytes equal historical Patch 0012 oracle postcommit projection bytes;
5. derive/freeze new live postcommit StateHash;
6. prove MARLOWE selection;
7. prove live Opportunity result projection bytes equal historical Patch 0013 MARLOWE projection bytes;
8. derive/freeze new live Opportunity StateHash;
9. derive/freeze first-v3 structured byte count/hash/ContextPacketId;
10. derive/freeze render-v2 byte count/hash;
11. prove first-v3 TrustedStateText equals historical evolved MARLOWE v2 TrustedStateText;
12. prove multi-entry append order;
13. prove semantic silence differs from empty history and literal silence-marker text.

These are static reference-oracle requirements, not machine validation.

## 35. Existing downstream semantic contracts

Current source inspection confirms Candidate parsing and Integrity/State Interpretation association layers consume Context semantic identity/subject/opportunity rather than hard-coding v1/v2 schema tokens.

Patch 0015 must prove by tests that v3 traverses existing public:

```text
Performer Candidate
-> Integrity
-> State Interpretation
-> State Authority
-> E0Take.Bind
```

without semantic redesign.

The deliberate v3 live version gate is `BindWithAcceptedHistory`.

If implementation discovers another hidden schema gate, patch only the smallest version-assumption surface; do not redesign the pipeline.

## 36. Narrow inherited test adaptations

Historical evidence/docs remain immutable. Only executable tests whose historical premise is explicitly superseded by Patch 0015 may change.

### A. Patch 0005 Context “no recent history type” guard

File:

```text
tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs
```

Narrowly evolve `ContextBoundary_DoesNotExposeProvenanceAccessDecisionsOrRecentHistoryType` because Patch 0015 intentionally adds exactly one public Context Performance type: `ContextRecentPerformance`.

Preserve enduring assertions that ContextRecord/Relationship expose no provenance, ContextPacket exposes no Access decisions, trace exposes no denied rows, and provider-neutral rendering hides internal IDs/provenance.

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

Its private `Clone(...)` invokes the sole non-public `ContextPacket` constructor positionally. Adding `RecentPerformances` requires only a test-helper adaptation to pass `source.RecentPerformances` at the new constructor position. Preserve all existing tamper tests.

Current source search found no other direct ContextPacket non-public-constructor production-test construction site requiring the same adaptation.

No inherited test change is required merely because `E0TakeStateBinding` gains `BindWithAcceptedHistory`; current inherited structural tests resolve historical `Bind` by name/signature rather than freezing the total binding method count.

## 37. Required implementation tests if approved

At minimum:

### Neutral text invariant
- existing Patch 0006 Candidate text tests remain unchanged and pass;
- neutral helper exactly preserves null/empty/surrogate/NFC/control/display-bearing semantics;
- TAB and LF remain permitted where previously permitted;
- Performer maps neutral failures to existing PerformerCandidateException behavior;
- Context/history invalid semantic items fail in their own sanitized domains;
- no Context -> Performer dependency exists.

### Public/history shape
- exact genesis initialization succeeds; non-genesis initialization rejects;
- opaque history has zero public constructors, setters, properties, or declared methods;
- internal SceneId/CurrentStateHash/Entries exist exactly;
- internal Entries type exactly `ImmutableArray<ContextRecentPerformance>`;
- no public history entry/count/transcript/hash surface;
- history transition exception exists only in Continuity with no public constructor;
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
- RecordCommit proves structured semantic Context identity but does not claim independent historical rendered-byte proof;
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
- exact neutral Character-legible text grammar reused;
- silence exact;
- literal `[PERFORMANCE: SILENCE]` noncollision;
- no trailing LF;
- v2/v1 render-v1 bytes unchanged;
- duplicate display names do not change structured CharacterId attribution; no new uniqueness law.

### Downstream Take proof
- v3 Context passes existing Candidate -> Integrity -> Interpreter -> Authority -> Take public semantics;
- exact synchronized v3 live binder succeeds;
- stale/altered history and tampered structured/rendered bytes/hashes fail precommit binding;
- SourceStateHash alone cannot rescue mismatch;
- second v3-source Accepted Take commits and RecordCommit independently rechecks exact structured semantic source Context identity.

### Failure/privacy
- expected transition failure -> `E0AcceptedPerformanceHistoryException`;
- expected live Context failure -> `E0ContextContinuityException`;
- expected live Take failure -> `E0CausalCommitException`;
- no creative/private/provider/secret text in expected public failure representation;
- unexpected programming failures not swallowed.

### Oracle/determinism/dependency
- independently reproduce historical hashes;
- pin exact v2-source Candidate hash `6ce2a98d...` and proposal hash `ac0f7a91...`;
- prove live postcommit projection byte equality to historical Patch 0012 oracle projection;
- derive/freeze new live postcommit StateHash;
- prove MARLOWE remains selected;
- prove live Opportunity projection byte equality to historical Patch 0013 MARLOWE projection;
- derive/freeze new live Opportunity StateHash;
- derive/freeze first v3 structured/rendered bytes/hashes;
- repeated byte determinism and culture invariance;
- history order intentionally order-sensitive while unrelated source collections retain existing canonical invariance;
- no Context -> Performer/higher-layer dependency;
- no CausalCommit -> Opportunity/Continuity dependency;
- Production/Opportunity semantics/public shapes unchanged;
- no provider/network/filesystem/clock/random/Windows/NPU dependency.

## 38. Expected implementation surface

If approved, patch-first source should remain approximately:

```text
src/Ensemble.E0.Core/Domain/
    internal CharacterLegibleTextInvariants

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
    delegate existing private VisibleText grammar to neutral internal invariant

focused Patch 0015 tests
narrow inherited test adaptations listed in Section 36
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

If implementation requires semantic changes to those preferred no-change areas rather than narrow version/wiring/text-invariant extraction support, stop and reopen architecture.

## 39. Complexity and memory

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

## 40. ARM64/battery suitability

Patch 0015 adds synchronous deterministic CPU/memory work only at explicit Context/Take/commit/Opportunity/history boundaries.

No idle polling, background loop, filesystem requirement, network/provider call, GPU/NPU work, Windows AI API, wall clock/random source, or emulation path.

This is compatible by design with the current native ARM64 deterministic Core/Harness approach. It is not measured power/performance evidence.

## 41. Explicit non-scope

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

## 42. Proposal 0.15 resolved decisions

1. Patch 0015 is Accepted Performance History + Context Continuity, not generic PerformerInput, cognition, or general Observation.
2. History is a truly opaque public token with zero public properties.
3. Internal history stores only `ContextRecentPerformance` semantics plus Scene/StateHash synchronization metadata.
4. No per-entry causal IDs/hashes/control and no standalone history hash.
5. One neutral internal Domain invariant owns exact Character-legible VisibleText grammar; Context never depends on Performer.
6. Full Ensemble live path requires `BindWithAcceptedHistory` before commit.
7. The precommit binder is the sole full structured+rendered source Context proof.
8. `RecordCommit` independently recomposes exact structured semantic Context identity and canonical-replays the established commit before appending; it does not claim independent historical rendered-byte proof.
9. Commit result is staged and adopted only after history advancement succeeds.
10. `RecordOpportunity` canonical-replays the established transition and couples routing-history count with accepted-history count.
11. Opportunity result is staged and adopted only after history advancement succeeds.
12. New live Context/Take methods have explicit distinct names; historical methods remain exact.
13. Public history-transition exception belongs in Continuity; Context/Take preserve existing public exception domains.
14. Complete current-Scene accepted Performance order is the E0 unoptimized reference history window.
15. `copresent-trio.v1` gains only this higher-layer E0 common Character-legible recent-Performance rule; it is not generalized into a perception engine.
16. Self history is included; no unfrozen forgetting rule.
17. Recent Performance remains untrusted creative content and never becomes truth/Observation/epistemic state merely by inclusion.
18. Trusted durable consequence and recent Performance remain independent layers and may coexist.
19. Context v3 adds only recent semantic items plus new schema/composition/render contracts; historical v1/v2 bytes stay immutable.
20. Context trace remains unchanged.
21. Duplicate display-name behavior remains inherited; structured attribution stays exact by CharacterId.
22. Historical Patch0012/Patch0013 StateHashes are v1-source lineage and are not Patch0015 live hashes.
23. The live oracle reuses exact historical oracle IDs/mutation inputs on a separate genesis branch to isolate the v2 source-Context effect.
24. The independently reconstructed v2-source Candidate/Proposal hashes are `6ce2a98d...` / `ac0f7a91...`; downstream live StateHashes remain to be independently derived before native validation.
25. Live postcommit and Opportunity projection bytes must match their historical counterpart projections exactly; only causal lineage identity changes.
26. Five inherited test surfaces are identified for narrow adaptation: Patch0005 recent-history absence, Patch0012 CausalCommit public-type list, Patch0014 temporary history/constants/API absence, Patch0014 Continuity namespace/method exactness, and Patch0014 ContextPacket reflection clone helper.

## 43. Recursive audit order

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

## 44. Proposal 0.15 audit status

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
