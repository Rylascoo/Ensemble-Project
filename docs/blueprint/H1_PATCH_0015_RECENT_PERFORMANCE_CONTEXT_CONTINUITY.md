# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.4 — EXPLORATORY; recursive adversarial audit in progress; implementation forbidden
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

Proposal 0.2 attempted to disclose only the immediately previous accepted Performance. That is too weak because Blueprint 0.1 explicitly permits accepted historical texture to remain true without promotion into durable projected state. One-turn-only history would lose earlier dialogue/action that may remain socially causal, or would pressure the system to promote every utterance/action into durable state.

Patch 0005 intentionally reserved a plural array. Proposal 0.4 therefore uses the complete accepted Performance sequence of the current bounded E0 Scene. Later product context optimization may deliberately window/retrieve/summarize under a new contract; E0 excludes that optimization.

### 2.3 Provider-attempt provenance — deferred

Provider request framing, Run/Attempt attribution, retry/spend/cancellation, external disclosure provenance, raw response retention, and provider execution remain necessary later. They should attach after the semantic Context supplied to a Performer can preserve accepted Scene continuity correctly.

### 2.4 Result-wrapper trust — corrected

Proposal 0.3 proposed history advancement from `E0CausalCommitResult` and `E0OpportunityTransitionResult` wrappers. That is unnecessary and weaker than the deterministic authority already implemented.

Proposal 0.4 instead advances history from immutable established events and reuses the canonical replay paths:

```text
E0CausalCommit
    -> DeterministicCausalCommit.Replay(...)

E0OpportunityTransition
    -> DeterministicOpportunityAuthority.Replay(...)
```

History does not duplicate commit mutation logic, Director selection logic, or opportunity StateHash logic.

### 2.5 Redundant projected hashes — removed

Proposal 0.3 history entries carried commit parent/result StateHashes and candidate-content identity/hash. They are removed.

Those values remain authoritative on the causal commit/Take chain. Copying them into a lightweight Context-history projection creates redundant public provenance without allowing independent reconstruction of the omitted full event chain.

The history projection retains one current Production StateHash synchronization anchor plus minimal event references.

## 3. Frozen authority basis

### Blueprint 0.1

- Character != Performer.
- A generated attempt becomes history only if accepted as a Take.
- Accepted Performance + approved consequences form one atomic causal commit.
- The conceptual source of truth is append-only causal event history.
- Accepted historical texture remains true even when it is not durable projected state.
- Context packets conceptually include recent events / “what just happened.”
- Imported text and fictional dialogue are untrusted creative content, separate from trusted state/system authority.
- Performance may be speech, action, silence, refusal, redirection, or another Character-legible response.
- Partial/rejected/cancelled output must not enter Production history.
- Missing Raft E0 uses exactly three co-present Characters in one bounded Scene.
- full observation/location/hearing/attention semantics remain reserved.

### Patch 0005

Patch 0005 freezes:

- `recentPerformances` as a root semantic array;
- v1 historical emptiness;
- `RecentPerformanceText` as a separate rendered authority layer;
- future non-empty item schema and causal ordering as responsibility of a later accepted-history/commit slice;
- no fabricated transcript/history.

Patch 0014 subsequently preserves those exact v1 bytes and adds Production-bound v2 while retaining empty recent Performance.

### Patch 0006

`CandidatePerformance.VisibleText` is the Character-legible Performance surface. Empty text is valid silence. Hidden reasoning and invisible pseudo-performance are outside CandidatePerformance. Typed address/nomination control is separate non-visible control and is explicitly not generic observation eligibility.

### Patch 0011

Rejected/Alternate Takes never enter Production history or recent-performance Context.

### Patch 0012

Only an Accepted Take bound to exact source authority may commit. `E0CausalCommit` owns the exact Accepted `E0Take`; that Take owns the exact `CandidatePerformance`. The causal StateHash chain binds the accepted Take semantic identity and resulting Production projection.

### Patch 0013

The postcommit state receives one canonical effective Opportunity transition. `E0OpportunityHistory` is routing history, not transcript/Performance history. Patch 0013 already exposes deterministic event replay.

### Patch 0014

Production-backed Context v2 is exact-state-bound. CharacterClaim and recent-Performance disclosure remain deferred. `SourceStateHash` is non-diegetic. Exact source Context is freshly recomposed at Take binding; metadata alone is insufficient.

## 4. Exact Patch 0015 question

> Can Ensemble maintain a closed deterministic projection of successfully committed Character-legible Performances for the current E0 Scene, synchronize that projection through the exact commit/opportunity StateHash chain, and compose it into later Character Context without turning historical dialogue/action into projected truth, Observation, Memory, Belief, Claim, or provider state?

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
CausalCommit data contract
    -> accepted Performance history data contract

CausalCommit + Opportunity replay authority
    -> accepted Performance history advancement in Continuity

closed accepted Performance history
    -> history-aware Context continuity

closed accepted Performance history
    -> history-aware exact Take source-context proof
```

Rules:

- `Context` does not depend on CausalCommit, Opportunity, or Continuity.
- accepted-history data lives low enough for `E0TakeStateBinding` to consume without `CausalCommit -> Continuity` or `CausalCommit -> Opportunity`.
- accepted-history advancement lives in `Continuity` because it observes both CausalCommit and Opportunity transitions.
- Opportunity remains unchanged and continues to depend on CausalCommit, not vice versa.
- Production projection remains unchanged.

## 6. Exact new public surface

### `Ensemble.E0.Core.CausalCommit`

Add exactly:

```csharp
public sealed class E0AcceptedPerformanceHistory
public sealed class E0AcceptedPerformanceHistoryEntry
public sealed class E0AcceptedPerformanceHistoryException : Exception
```

`E0AcceptedPerformanceHistory` public read-only properties:

```text
SceneId : SceneId
CurrentStateHash : StateHash
Entries : ImmutableArray<E0AcceptedPerformanceHistoryEntry>
```

`E0AcceptedPerformanceHistoryEntry` public read-only properties:

```text
CommitId : CommitId
TakeId : TakeId
SubjectCharacterId : CharacterId
SourceContextPacketId : ContextPacketId
VisibleText : string
```

No public constructors or setters.

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

`E0AcceptedPerformanceHistory` answers only:

> Which Character-legible Performances were successfully accepted into this bounded Scene’s causal history, in what causal order, and to which current Production StateHash has this live projection been advanced?

It does not answer:

- whether propositions in the Performance are objectively true;
- what any Character observed, remembers, knows, believes, suspects, or claims;
- which consequences became durable state;
- which Performance is relevant or important;
- provider/model provenance;
- raw attempts or diagnostics;
- durable event reconstruction.

Authoritative current state remains `ProductionState`.

Authoritative causal event remains `E0CausalCommit`.

The new history object is a **closed in-memory derived projection for current-Scene Performance continuity**.

## 8. Why the history entry is intentionally small

History entries retain only:

```text
CommitId
TakeId
SubjectCharacterId
SourceContextPacketId
VisibleText
```

Rationale:

- CommitId locates the authoritative causal event when event provenance exists.
- TakeId identifies the accepted Take occurrence.
- SubjectCharacterId identifies who performed.
- SourceContextPacketId identifies the exact semantic Context from which that Performance was produced.
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
- observation/knowledge/belief/memory/claim classification.

Those belong to richer authoritative/provenance layers already implemented or deliberately deferred.

## 9. `CurrentStateHash` is synchronization, not parallel authority

A list of accepted text is insufficient. The live projection must remain synchronized to the deterministic state transition chain.

```text
genesis opportunity-bearing state
    history.CurrentStateHash = genesis StateHash

successful atomic commit replay
    append exactly one Performance
    history.CurrentStateHash = replayed postcommit StateHash

successful opportunity replay
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

Authority comes from the fact that public construction/advancement is closed and every advancement replays the canonical causal event from the exact previous synchronized state. Patch 0015 does not claim that a deserialized/reflection-forged history object can be authenticated from `CurrentStateHash` alone. Durable reconstruction/authentication requires the future causal event store.

## 10. History initialization

`E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)` must reuse existing exact genesis/routing authority rather than duplicate it.

Preferred proof:

```text
E0OpportunityHistory.Initialize(genesisState)
```

This already proves:

- non-null Production state;
- initialized StateHash/SceneId;
- canonical E0 roster;
- exactly one valid current opportunity;
- exact genesis StateHash recomputation.

History initialization then returns:

```text
SceneId = genesisState.SceneId
CurrentStateHash = genesisState.StateHash
Entries = []
```

No opening transcript is invented.

Do not add a Production-state public/internal “history count” merely for this initialization.

## 11. Commit advancement reuses canonical causal replay

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
4. defensively validate the source history closed shape;
5. require supported causal-commit/Take/Candidate contracts and initialized event identities;
6. call exactly:

```csharp
DeterministicCausalCommit.Replay(parentState, committedEvent)
```

7. use the **fresh replayed state** as transition authority;
8. require replayed Scene association remains exact and postcommit state has no current opportunity;
9. append exactly one new history entry from `committedEvent.Take.Performance` plus CommitId/TakeId/source Context identity;
10. advance `CurrentStateHash` to the fresh replayed StateHash.

The history boundary must not reimplement mutation application, materialization law, commit StateHash canonicalization, duplicate effective-ID policy, or State Authority snapshot proof.

The caller does not supply an `E0CausalCommitResult` to history advancement.

### Candidate visible-text validation

The appended text comes only from the successfully replayed Accepted Take’s closed `CandidatePerformance`.

For defensive history validation, implementation may expose the already-existing Patch 0006 visible-text validator as **internal-only reusable logic** (for example by changing only helper accessibility or adding an internal forwarding helper). It must not create a second text grammar and must not change public Candidate semantics or bytes.

## 12. Opportunity advancement reuses canonical Opportunity replay

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
4. require the accepted-history last entry matches `sourceCommit.CommitId`, `sourceCommit.Take.TakeId`, source Character, and source ContextPacketId;
5. require `sourceCommit.ResultStateHash == postCommitState.StateHash`;
6. call exactly:

```csharp
DeterministicOpportunityAuthority.Replay(
    postCommitState,
    sourceCommit,
    sourceOpportunityHistory,
    establishedEvent)
```

7. use the fresh replay result as authority;
8. append no Performance entry;
9. advance only `CurrentStateHash` to the fresh replayed opportunity-bearing StateHash.

This reuses Patch 0013 source-chain validation, Director recomputation, event validation, canonical opportunity StateHash recomputation, Production transition, and routing-history advancement.

History does not duplicate Director or Opportunity logic and does not need to trust a caller-supplied `E0OpportunityTransitionResult` wrapper.

## 13. Failure and non-effective paths

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
- failed opportunity establishment/replay;
- technical diagnostic text.

A successfully Accepted zero-mutation Take **does** append its Performance because historical texture occurred even when no durable state record changed.

A successfully Accepted Take whose proposed consequences are all authoritatively Rejected likewise appends its Performance because the accepted Performance entered causal history atomically with the terminal consequence decision package.

## 14. Current-Scene causal order and E0 history window

For the bounded current E0 Scene:

```text
recentPerformances = every entry in E0AcceptedPerformanceHistory.Entries
```

in exact append order.

No:

- sorting by Character/ID/time;
- deduplication;
- relevance scoring;
- recency count;
- truncation;
- summarization;
- paraphrase;
- token budgeting;
- model compression.

Why complete current-Scene history:

- Patch 0005 intentionally deferred causal ordering to accepted-history authority;
- Blueprint 0.1 preserves historical texture without state explosion;
- E0 explicitly excludes context optimization;
- Full Ensemble E0 needs multi-turn social causality without provider-session memory;
- a one-Performance window discards accepted Scene texture too aggressively.

This is the deterministic E0 reference composer rule, not the final product long-context strategy.

A later version may deliberately window/retrieve/summarize only after preserving Access-before-relevance and accepted-history authority, under a new composition contract and separately controlled E0 comparison.

## 15. Scene boundary

History is scoped to exactly one current E0 Scene.

Patch 0015 does not define:

- cross-Scene transcript carryover;
- archive retrieval into new Scenes;
- scene-to-scene memory promotion;
- branches/canon merging;
- cross-Production Character history;
- persistent history loading.

A future Scene-transition architecture must explicitly decide what prior history becomes available under what Character-access/observation/memory law.

## 16. E0 `copresent-trio.v1` recent-Performance eligibility

Fixture Dialect v1 accepts exactly:

```text
ensemble.e0.copresent-trio.v1
```

and the canonical Missing Raft E0 fixture freezes exactly three co-present Characters for the bounded observation window.

Patch 0015 gives that E0-only reference contract one narrowly executable recent-Performance meaning:

> Successfully committed `CandidatePerformance.VisibleText` is common Character-legible Scene-performance history for every current roster Character under `ensemble.e0.copresent-trio.v1`.

This is newly specified Patch 0015 behavior. Earlier sources froze the token and co-presence but did not silently define this executable recent-history rule.

The rule is deliberately narrow:

- only accepted `VisibleText`;
- only current E0 Scene;
- only current roster;
- no typed control;
- no hidden reasoning;
- no provider diagnostics;
- no creator-only Production information;
- no inferred consequences;
- no claim-to-truth promotion.

This is **not** a global ontology rule that co-presence always implies complete perception. Future spatial/private/inaudible/concealed Performance grammars require an explicit observation contract and may not inherit `copresent-trio.v1` behavior.

For the E0 reference contract, a Performance cannot rely on private-performance semantics while also expecting selective recipient disclosure; selective perception is outside this contract.

## 17. Recent Performance is not CharacterObservation

Patch 0015 preserves the frozen future epistemic path:

```text
Event happened
    -> observation eligibility
        -> CharacterObservation
            -> possible Memory / Belief / Claim changes
```

Patch 0015 does not materialize `CharacterObservation` records.

Seeing recent accepted Performance in Context does not itself mean:

- every proposition in the prose is true;
- the Character gained Knowledge;
- the Character formed a Belief;
- the content became durable Memory;
- a spoken claim became a CharacterClaim record;
- general hearing/location/attention semantics are solved.

The E0 recent layer records that common Character-legible Performance occurred under the E0 reference disclosure contract. Epistemic consequences remain separately proposed/authorized.

## 18. Typed control remains non-visible

Patch 0006 `AddressedCharacterIds` and `NominatedCharacterId` are routing/intent metadata, not generic observation eligibility.

They never enter `ContextRecentPerformance` or `RecentPerformanceText`.

Exposing typed control to later Performers as historical prose/metadata would create a new knowledge channel not authorized by Patch 0006.

## 19. Context v3 semantic shape

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

Context receives only already-projected safe recent semantic items from the closed history layer. It does not accept raw arbitrary history/prose through a public composer.

## 20. Exact Context v3 contracts

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

## 21. Genesis behavior

Genesis has no accepted Performance history.

Therefore the **history-aware live path** with exact initialized empty history emits the existing Patch 0014 v2 packet unchanged.

No empty-history v3 packet exists.

Meaning:

```text
v2 = exact Production-bound Context with no accepted current-Scene Performance yet
v3 = exact Production-bound Context plus non-empty accepted current-Scene Performance history
```

A non-empty history paired with exact genesis fails.

An empty history paired with non-genesis history-aware continuation fails.

## 22. Historical v1/v2 compatibility and the live-path boundary

Patch 0015 must preserve machine-established v1/v2 bytes and compatibility behavior.

Existing APIs remain:

```csharp
DeterministicContextComposer.Compose(...)
E0ProductionContextContinuity.Compose(checkpoint)
E0TakeStateBinding.Bind(checkpoint, context, take)
```

The old one-argument Production Continuity path remains a **current-state-only compatibility path** and may still emit v2 for evolved Production because Patch 0014 explicitly established that behavior.

The old three-argument Take binding remains historical v1/v2 compatibility and must reject v3.

Patch 0015 does not retroactively break Patch 0014 executable behavior.

However, the new live Full Ensemble E0 continuation path must always use the history-aware overloads. After the first accepted commit, reference E0 orchestration must not silently fall back to the history-omitting compatibility path.

Patch 0015 does not yet implement that full orchestration loop, so this requirement is frozen for the later Harness Scene-loop patch rather than falsely claimed as globally compiler-enforced today.

## 23. History-aware Production Context Continuity

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
    -> if history empty:
           require exact genesis
           compose existing internal v2
       else:
           require non-genesis
           project exact ordered ContextRecentPerformance[]
           compose internal v3
    -> require packet/trace SourceStateHash == checkpoint StateHash
    -> return existing Access + Context evaluation result shape
```

The result type remains unchanged:

```text
E0ProductionContextContinuityResult
- AccessEvaluation
- ContextEvaluation
```

## 24. History-aware exact Take binding

Add:

```csharp
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

The four-argument live binding:

- accepts exact genesis v2 only with exact empty initialized history;
- accepts v3 only with non-empty synchronized history;
- rejects v1;
- rejects evolved v2;
- rejects empty-history evolved state;
- rejects non-empty history at genesis.

For v3 it must:

1. validate checkpoint/current state identities;
2. validate closed history shape;
3. require history StateHash/Scene match checkpoint;
4. fresh Production Access;
5. project exact ordered recent Performance semantics from history;
6. fresh internal v3 Context composition;
7. compare exact canonical structured bytes;
8. compare exact canonical rendered bytes;
9. compare ContextPacketId;
10. compare StructuredContextHash;
11. compare RenderedContextHash;
12. compare SourceStateHash;
13. then run the inherited exact Take/StateAuthority snapshot proof.

No arbitrary caller-supplied recent prose parameter exists.

The existing three-argument binding retains Patch 0014 behavior and must not acquire a v3 bypass.

## 25. Shared history validation

Use one internal CausalCommit-layer validation/projection helper for both history-aware Context continuity and history-aware Take binding.

It should defensively prove at minimum:

- history non-null;
- Entries not default;
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
- when projecting for a current Context, every source Character resolves exactly once in current roster.

Do not attempt full event replay from projected entries; the projection deliberately does not carry enough data for durable reconstruction.

CurrentStateHash equality alone is not treated as authentication. Closed initialization/advancement is the authority assumption for this in-memory projection.

## 26. Structured v3 canonicalization

V3 preserves v2 root property order:

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

Each `recentPerformances` item property order:

```text
sourceCharacterId
visibleText
```

Recent Performance array order is causal append order and is never sorted.

All historical roster/record/relationship canonical ordering stays unchanged.

V3 identity:

```text
StructuredContextHash = SHA256(canonical v3 structured bytes)
ContextPacketId = "CTX:" + StructuredContextHash
```

Therefore different Performance text, source Character, causal order, or silence/non-silence changes structured Context identity even if current durable state records are otherwise identical.

## 27. Render-v2 exact recent-Performance shape

A non-empty Character-visible recent layer changes rendering semantics, so v3 uses `ensemble.e0.context.render.v2`.

`TrustedStateText` and `OpportunityText` preserve their existing v1 algorithms byte-for-byte. Only `RecentPerformanceText` gains non-empty behavior.

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
- source text is never trimmed, paraphrased, summarized, repaired, or reordered;
- non-empty text uses the existing LF split / two-space continuation rendering discipline;
- Character IDs and causal/provenance IDs are not rendered.

## 28. Silence rendering

Empty `CandidatePerformance.VisibleText` is valid silence.

Structured v3 item:

```json
{"sourceCharacterId":"...","visibleText":""}
```

Rendered entry:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

No `[PERFORMANCE]` line or bullet body follows silence.

Literal non-silent text equal to `[PERFORMANCE: SILENCE]` renders through the ordinary non-silent form and therefore cannot collide with semantic silence.

## 29. Recent Performance remains untrusted creative content

`RecentPerformanceText` remains separate from `TrustedStateText`.

Future provider framing must preserve distinct authority layers for:

```text
system/Performer contract
trusted state
recent accepted fictional Performance
opportunity
imported/user content where applicable
```

A prior accepted line that contains prompt-like language does not gain system authority merely because it is retained in Scene history.

Patch 0015 does not define the provider request envelope.

## 30. Character self-history

The current Character receives prior accepted Performances from all roster Characters, including their own earlier Performances, when that Character later receives another opportunity.

Reason:

- the history is shared E0 Scene-performance history under `copresent-trio.v1`;
- a Character’s own accepted prior action is not other-Character private state;
- removing self history would create a special forgetting rule not authorized by Blueprint 0.1.

This does not materialize durable Memory. Future memory fallibility/retention remains separate.

## 31. Minimal-disclosure reconciliation

Full current-Scene recent Performance does not authorize disclosure of the whole Production.

The E0 reference path still discloses only:

```text
current Character-safe Access projection
+ common accepted Character-legible Scene Performance history
+ current opportunity
```

It still excludes:

- Production-only truth;
- other Characters’ private records;
- denied Access rows;
- fixture provenance;
- typed control;
- provider diagnostics;
- creator-only data.

E0 uses complete accepted current-Scene Performance history because semantic relevance/windowing is intentionally not part of the reference experiment. A later product composer may narrow this already-eligible history under a newly versioned contract.

## 32. `ContextCompositionTrace` remains unchanged

Do not add accepted-history count, source Character IDs, CommitIds, or TakeIds to `ContextCompositionTrace`.

Reasons:

- packet `RecentPerformances` already exposes the exact Character semantic items;
- structured/rendered hashes already identify exact emitted bytes;
- causal Commit/Take provenance remains in the separate accepted-history projection;
- adding trace duplicates information without a current consumer.

Existing trace source StateHash and dual hashes remain sufficient.

## 33. No separate history hash

Do not add `PerformanceHistoryHash` in Patch 0015.

Reasons:

- closed advancement is anchored to the existing Production StateHash chain;
- each accepted commit already binds Take/Candidate semantic identity;
- each v3 Context hashes exact disclosed history semantics directly;
- a second standalone history hash would look like parallel authority without making the projection durably reconstructible.

A future persisted history envelope may need its own canonical/authenticated identity. That is persistence scope.

## 34. Causal commit and Opportunity history remain authoritative in their own domains

`E0CausalCommit` remains the causal event authority and retains exact Accepted Take plus materializations and parent/result StateHashes.

`E0OpportunityHistory` remains routing-only:

```text
who held effective opportunity in routing order?
```

`E0AcceptedPerformanceHistory` answers:

```text
which Character-legible Performances were accepted in Scene causal order?
```

Do not add transcript semantics to Opportunity history and do not add routing semantics to accepted Performance entries.

## 35. Production projection remains unchanged

Recent Performance history must not enter `ProductionStateProjection`.

Doing so would:

- duplicate event history into current state;
- make historical texture look like durable projected state;
- cause unnecessary state growth;
- alter Patch 0012/0013 state-hash oracles;
- undermine the frozen historical-texture/durable-consequence distinction.

Trusted current consequences continue to reach Character Context only through committed Production records and current Access.

## 36. CharacterClaim remains deferred

A prior accepted Performance may contain a factual-sounding claim. That does not make the proposition a `CharacterClaim` record, Knowledge, Belief, Memory, or truth.

Patch 0014 `CharacterClaimDisclosureDeferred` remains exact for retained Production records.

Patch 0015 adds only common accepted E0 Performance occurrence history.

## 37. Observation-contract representation limitation

Current Production projection does not separately carry the fixture ObservationContract token.

Patch 0015 does not add it merely for this E0-only behavior.

Fixture Dialect v1 validates exactly one observation token, `ensemble.e0.copresent-trio.v1`, and Production genesis can originate only from validated E0 v1 fixture authority.

Therefore accepted-history disclosure is explicitly an **E0 Fixture Dialect v1 reference rule**, not a general future Production rule.

If a later fixture dialect supports multiple observation contracts, active observation/disclosure authority must become explicit at the appropriate state/context boundary. This assumption must then be reopened rather than inherited silently.

## 38. Multi-turn live proof without durable replay

Patch 0015 supports multiple live E0 turns by induction:

```text
closed history at current opportunity-bearing StateHash
    + established causal commit event
        -> replay commit from exact parent
            -> append one accepted Performance
            -> history at no-opportunity postcommit StateHash
                + source OpportunityHistory + established Opportunity event
                    -> replay Opportunity transition
                        -> append nothing
                        -> history at next opportunity-bearing StateHash
```

Repeat.

No older causal event is rediscovered from current projected state. No durable history deserialization/reconstruction is claimed.

Full replay from genesis, persistence/recovery, branch reconstruction, and stored-event authentication remain later scope.

## 39. Exact current-state association and limitations

History-aware Context/Take paths require exact current StateHash/Scene equality.

Normal closed progression therefore rejects:

- stale history;
- history from another Scene/current branch;
- skipped commit advancement;
- skipped opportunity advancement;
- foreign commit event;
- foreign opportunity event;
- evolved empty history.

Because public history constructors are closed, ordinary external callers cannot fabricate an alternate valid history object through supported C# construction.

Patch 0015 does **not** claim resistance to arbitrary reflection/runtime memory corruption, nor durable-store authentication without the future event chain.

## 40. Existing downstream semantic contracts remain version-agnostic where already designed

Patch 0006 intentionally does not gate Candidate parsing on a specific Context schema/composition contract. Integrity, Interpreter, State Authority, and Take operate on semantic association rather than hardcoding Context v1/v2 tokens.

Patch 0015 must prove by tests that an exact v3 packet can flow through the existing:

```text
PerformerCandidateContract
-> Integrity
-> State Interpretation
-> State Authority
-> E0Take.Bind
```

without changing those semantic contracts, before the new history-aware `E0TakeStateBinding` performs exact source-history proof.

If implementation discovers a hidden v1/v2 gate in those downstream surfaces, patch only the smallest version-assumption surface and preserve semantics; do not redesign the pipeline.

## 41. Historical canonical compatibility authority

Implementation must preserve these machine-established identities exactly.

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

Patch 0015 must not change any of those historical bytes/hashes.

## 42. Canonical first v3 oracle

Use the already-frozen Patch 0012 -> Patch 0013 independent oracle chain whose Candidate visible text is exactly:

```text
No.
```

Reference:

```text
VOSS accepted Performance = "No."
postcommit StateHash = 057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
next Character = MARLOWE
current StateHash = dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
history = [ VOSS -> "No." ]
current Context = MARLOWE v3
```

Before native validation, an independent implementation-oracle derivation must produce exact:

- v3 structured byte count;
- v3 StructuredContextHash;
- v3 ContextPacketId;
- render-v2 byte count;
- v3 RenderedContextHash.

The independent derivation must first reproduce the inherited v1/v2/Production oracle values above.

A multi-entry oracle/test must prove append order, and a silence oracle/test must distinguish one accepted silent Performance from empty history and literal marker text.

## 43. Narrow inherited-test supersession

Patch 0014 intentionally asserted at that checkpoint that Context had no recent-Performance public type/property and that only v1/v2 constants existed.

Patch 0015 explicitly supersedes only those temporary “not yet” assertions in:

```text
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ContractAuditTests.cs
```

Expected narrow updates:

- `ContextPacket.RecentPerformances` now exists;
- exactly one new `ContextRecentPerformance` public Context type exists;
- exactly three v3/render-v2 constants are added;
- Production Continuity now has two public `Compose` overloads.

Historical Patch 0014 evidence/docs remain immutable and truthful for their checkpoint.

No unrelated earlier contract assertion should be weakened merely to make Patch 0015 pass.

## 44. Required implementation tests if approved

### History initialization/closed construction

- exact genesis initialization succeeds with empty entries;
- non-genesis initialization rejects;
- no public history/entry constructors or setters;
- history exception is publicly catchable with no public constructor;
- initialization reuses exact genesis/routing authority and invents no transcript.

### Commit advancement

- exact established commit replays and appends exactly one entry;
- state anchor advances to replayed postcommit StateHash;
- stale history/current parent mismatch rejects before append;
- foreign/tampered commit event rejects through replay;
- zero-mutation Accepted commit appends;
- all-consequence-Rejected Accepted commit appends;
- Rejected/Alternate/noncommit paths cannot append;
- duplicate CommitId/TakeId cannot enter through normal replay progression;
- appended entry is exact CommitId/TakeId/subject/source Context/VisibleText only.

### Opportunity advancement

- exact established event replays through `DeterministicOpportunityAuthority.Replay`;
- history last entry must match the source commit;
- state anchor advances to fresh opportunity result StateHash;
- no Performance entry is appended;
- stale/foreign source routing history rejects;
- foreign/tampered opportunity event rejects;
- skipped commit/opportunity advancement causes later history-aware Context failure.

### Multi-turn

- two and three accepted commits yield exact two/three-entry causal order;
- same Character may appear multiple times without deduplication;
- history remains synchronized across commit -> opportunity -> commit -> opportunity;
- earlier accepted text remains present even if it created no durable Production record;
- self prior Performance remains present when that Character receives a later opportunity;
- no provider-session memory is required for the Context packet to retain accepted Scene history.

### V1/v2 compatibility

- all historical v1 byte/hash oracles unchanged;
- genesis v2 oracle unchanged;
- evolved Patch 0014 v2 oracle unchanged;
- old one-argument Production Continuity remains v2 behavior;
- old three-argument Take binding retains v1/v2 behavior and rejects v3.

### V3/history-aware version matrix

- history-aware exact genesis emits v2;
- four-argument live Take binding accepts that exact genesis v2 only with empty exact history;
- non-empty accepted history emits v3;
- evolved empty history rejects in history-aware path;
- v3 requires render-v2;
- v1/v2 require empty RecentPerformances/RecentPerformanceText;
- v3 requires non-empty recent semantics/text;
- every hybrid schema/composition/render combination rejects;
- exact v3 root/item property order.

### Disclosure/privacy/epistemic separation

- Context recent item contains only source CharacterId + exact VisibleText;
- typed address/nomination absent;
- CommitId/TakeId/StateHash/ContextPacketId/candidate hashes absent from Character recent semantics/rendered recent text;
- prior private Context state absent;
- CharacterClaim remains denied;
- no CharacterObservation/Knowledge/Belief/Memory record generated;
- factual-sounding accepted speech remains historical Performance, not truth;
- current Character receives own earlier accepted Performance under E0 shared Scene history.

### Rendering

- exact single-entry render;
- exact multi-entry blank-line/order behavior;
- multiline LF/two-space continuation exact;
- Unicode/NFC visible-text invariant reused exactly from Candidate semantics;
- exact silence rendering;
- literal `[PERFORMANCE: SILENCE]` cannot collide with semantic silence;
- no trailing LF;
- render-v2 canonical envelope exact.

### Exact Take binding

- exact synchronized v3 history/context/take accepts;
- stale history rejects;
- changed entry text rejects exact Context equivalence;
- changed source Character rejects;
- reordered/dropped/extra entries reject;
- tampered structured bytes/hash/ContextPacketId rejects;
- tampered rendered bytes/hash rejects;
- SourceStateHash metadata alone cannot rescue mismatched history;
- old binding cannot bypass v3 history proof.

### Downstream semantic compatibility

- v3 Context -> Candidate parser succeeds without Candidate-contract change;
- v3 Candidate -> Integrity -> Interpreter -> State Authority -> Take succeeds under existing contracts;
- second accepted v3-sourced Take can bind/commit through history-aware source proof;
- no provider/model execution required.

### Determinism/oracle

- repeat byte determinism;
- source collection order invariance remains where semantically unordered;
- accepted-history order remains intentionally order-sensitive;
- culture invariance (`ar-SA` or equivalent);
- canonical `No.` Patch0012->0013 v3 independent oracle;
- inherited Production/Context hashes preserved;
- no network/filesystem/clock/random/provider/Windows/NPU dependency.

### Public/dependency audit

- only approved public history/Context additions exist;
- Context has no dependency on CausalCommit/Opportunity/Continuity;
- CausalCommit history data has no Opportunity/Continuity behavior;
- CausalCommit does not depend on Opportunity for Take binding;
- history advancement alone depends upward on Opportunity inside Continuity;
- Opportunity history public shape unchanged;
- Production projection public/internal semantic shape unchanged;
- no generic PerformerInput/provider abstraction/event bus/store interface appears.

## 45. Expected implementation surface

Patch-first source surface should remain approximately:

```text
src/Ensemble.E0.Core/CausalCommit/
    E0AcceptedPerformanceHistory data + invariant/projection helper
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
    at most a private->internal/nonpublic reuse of the exact existing VisibleText validator; no semantic change

focused Patch 0015 tests
narrow Patch0014ContractAuditTests supersession
```

Preferred no-change semantics:

- Fixture JSON/dialect;
- Access policy;
- Production projection and transition semantics;
- Candidate public contract/parser behavior;
- Director selection policy;
- Opportunity canonicalizer/hash;
- Integrity semantics;
- State Interpreter semantics;
- State Authority semantics;
- Take semantics;
- Harness runtime/provider execution.

If implementation requires changing those semantics rather than merely wiring the new history/context version, stop and reopen architecture.

## 46. Complexity and memory

Let:

```text
R = retained Production records
A = permitted current records
B = permitted current-state bytes
H = accepted Performance count in current E0 Scene
P = total accepted VisibleText bytes in current E0 Scene
M = current commit mutation/materialization work
```

Approximate explicit-boundary costs:

```text
History validation:        O(H)
RecordCommit:              replay cost + O(H) immutable append/validation
RecordOpportunity:         Opportunity replay cost + O(H) validation
History-aware Access:      O(R)
History-aware Context:     O(A log A + B + H + P)
History-aware Take proof:  fresh Access/Context + inherited StateAuthority proof
```

Repeated full-history rendering/immutable append can make total long-Scene work superlinear. That is accepted for the bounded E0 reference harness; product-scale history storage/context optimization is not claimed.

Do not invent a hard Scene-turn/history byte ceiling in Patch 0015 without separate run-protocol authority.

## 47. ARM64/battery suitability

Patch 0015 adds synchronous deterministic CPU/memory work only at explicit commit/opportunity/context/binding boundaries.

No:

- idle polling;
- background loop;
- provider/network call;
- filesystem requirement;
- GPU/NPU work;
- Windows AI API;
- timer/random source;
- emulation path.

The design is compatible with the current native ARM64 deterministic Core/Harness approach by construction, while making no measured power/performance claim.

## 48. Explicit non-scope

Patch 0015 does not implement:

- general CharacterObservation generation;
- general location/hearing/attention/concealment observation engine;
- selective private Performance disclosure beyond the E0 `copresent-trio.v1` reference rule;
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

## 49. Resolved Proposal 0.3 audit questions

1. **Name** — `E0AcceptedPerformanceHistory`, not TakeHistory. The projection intentionally retains only accepted Character-legible Performance semantics plus minimal event references.
2. **CandidateContentHash** — removed from projected entry as redundant. The causal commit/Take chain owns it.
3. **Commit validation** — always reuse `DeterministicCausalCommit.Replay` before appending.
4. **Opportunity validation** — accept source commit + source `E0OpportunityHistory` + established event and reuse `DeterministicOpportunityAuthority.Replay`; do not duplicate Director/hash logic.
5. **Take binding dependency** — direct four-argument binding consumes the low CausalCommit-layer history data type; no higher Continuity token is needed.
6. **CurrentStateHash** — retained strictly as synchronization anchor to existing Production authority; not a history identity/authentication claim.
7. **Context trace** — unchanged; packet semantics + dual hashes + separate history projection already carry the needed information.
8. **Self history** — included on later turns; no special forgetting rule.
9. **Minimal disclosure** — complete current-Scene accepted Performance is justified only inside the explicit E0 common Character-legible `copresent-trio.v1` reference contract; Production/private state remains filtered.
10. **Future optimization** — a later newly versioned composer may narrow/summarize already-eligible history; E0 v3 remains the unoptimized reference baseline.

## 50. Recursive audit order

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

## 51. Proposal 0.4 audit status

Proposal 0.4 resolves every explicit Proposal 0.3 design question and removes two unnecessary trust/duplication surfaces:

- result-wrapper trust is replaced by existing canonical replay authority;
- redundant per-entry StateHash/candidate-hash copies are removed.

The next recursive pass must specifically attack:

1. whether `copresent-trio.v1` common recent-Performance eligibility is sufficiently narrow to coexist with the frozen reserved Observation boundary;
2. whether legacy evolved-v2 compatibility creates an unacceptable authority bypass before full Scene-loop orchestration exists;
3. whether exact v3 render/version semantics can preserve every historical v1/v2 oracle byte;
4. whether the proposed history object can remain a derived projection rather than drifting into a second event store;
5. whether any public surface can be removed further without making exact live multi-turn proof impossible.

No implementation, approval evidence, implementation handoff, `CURRENT_STATE.md` update, or promotion is permitted until a complete recursive pass finds no material correction or worthwhile simplification and the user explicitly approves the resulting blueprint.
