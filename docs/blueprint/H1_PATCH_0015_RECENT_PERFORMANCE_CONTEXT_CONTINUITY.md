# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.7 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden
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

Blueprint 0.1 already freezes `Production State -> Access -> Context Composer -> bounded Character Context -> Performer`, and Patch 0006 implements `ContextPacket -> CandidatePerformance`. A second generic consumption/input layer would duplicate an existing authority seam.

### 2.2 Immediate-one-Performance-only Context — rejected

Proposal 0.2 retained only the immediately prior accepted Performance. That loses older accepted Scene texture that Blueprint 0.1 explicitly allows to remain historical without becoming durable projected state. Patch 0005 intentionally reserved a plural array. The E0 reference therefore keeps complete current-Scene accepted Performance order; later product optimization requires a new contract.

### 2.3 Provider-attempt provenance — deferred

Provider request framing, Run/Attempt attribution, retry/spend/cancellation, external disclosure provenance, raw response retention, and provider execution remain later scope. They should attach only after semantic Context can preserve accepted Scene continuity correctly.

### 2.4 Result-wrapper trust — corrected

Proposal 0.3 trusted result wrappers. Proposal 0.4 instead reused canonical deterministic replay:

```text
E0CausalCommit -> DeterministicCausalCommit.Replay(...)
E0OpportunityTransition -> DeterministicOpportunityAuthority.Replay(...)
```

History must not duplicate commit mutation, Director selection, or Opportunity StateHash logic.

### 2.5 Redundant projected causal fields — progressively removed

Proposal 0.3 copied commit parent/result StateHashes and candidate-content identity/hash into history entries. Proposal 0.4 removed those. Proposal 0.7 goes further and removes CommitId, TakeId, and SourceContextPacketId from history entries as well.

Reason: after a successful canonical commit replay, the history `CurrentStateHash` is the exact causal StateHash produced from the parent StateHash + complete commit payload + result projection. That commit payload already binds CommitId, TakeId, source Context identity via candidate-content identity, typed control, authority package, and consequences.

Duplicating those identities inside the derived Context-history projection does not add supported-path authority. It makes the projection look like a partial event store while still omitting enough event data to reconstruct/authenticate history independently.

Proposal 0.7 therefore stores only the Character-safe semantic material the projection exists to disclose:

```text
SourceCharacterId
VisibleText
```

Causal identity remains exclusively on `E0CausalCommit` + Production StateHash.

### 2.6 Live source-Context omission — corrected

Proposal 0.4 still allowed an evolved legacy v2 Take to replay into accepted history because causal replay does not possess the source ContextPacket.

Proposal 0.5 fixed this. Before replay/appending, `RecordCommit(...)` freshly composes the exact history-aware source Context from synchronized parent Production + source history and requires the committed Take’s source Character/ContextPacketId to equal that exact packet.

### 2.7 Public history-entry/count surface — removed

Proposal 0.4 exposed a history-entry type publicly; Proposal 0.5 made entries internal; Proposal 0.6 removed public `AcceptedPerformanceCount`.

Proposal 0.7 retains only public `SceneId` + `CurrentStateHash`. External orchestration carries an opaque closed history object. Character-safe history details are inspectable only through `ContextPacket.RecentPerformances`, where they belong.

### 2.8 Cross-boundary failure domains — normalized

One internal history invariant/projector is shared, but public failure ownership remains:

```text
history Initialize/RecordCommit/RecordOpportunity
    -> E0AcceptedPerformanceHistoryException

history-aware Production Context continuity
    -> E0ContextContinuityException

history-aware Take binding
    -> E0CausalCommitException
```

Creative/private text is never copied into expected public exception representations.

## 3. Frozen authority basis

### Blueprint 0.1

- Character != Performer.
- Generated attempt becomes history only if accepted as a Take.
- Accepted Performance + approved consequences form one atomic causal commit.
- Conceptual source of truth is append-only causal event history.
- Accepted historical texture remains true even when not durable projected state.
- Context conceptually includes recent events / “what just happened.”
- fictional dialogue/imported text are untrusted creative content separate from trusted state/system authority.
- Performance may be speech, action, silence, refusal, redirection, or another Character-legible response.
- partial/rejected/cancelled output must not enter Production history.
- Missing Raft E0 uses exactly three co-present Characters in one bounded Scene.
- full observation/location/hearing/attention semantics remain reserved.

### Patch 0005

- `recentPerformances` is a reserved root semantic array;
- historical v1 emptiness is exact;
- `RecentPerformanceText` is a separate rendered authority layer;
- later accepted-history/commit authority owns non-empty item schema/order/population;
- transcript/history must not be fabricated.

Patch 0014 adds Production-bound v2 while retaining exact empty recent Performance.

### Patch 0006

`CandidatePerformance.VisibleText` is Character-legible Performance; empty text is valid silence. Hidden reasoning/invisible pseudo-performance are excluded. Typed address/nomination control is non-visible and explicitly not generic observation eligibility.

### Patch 0011

Rejected/Alternate Takes never enter Production history or recent-performance Context.

### Patch 0012

Only Accepted Take bound to exact source authority may commit. `E0CausalCommit` owns exact Accepted Take and parent/result StateHashes; the Take owns exact CandidatePerformance. `DeterministicCausalCommit.Replay(...)` is canonical replay authority.

### Patch 0013

Postcommit state receives canonical effective Opportunity. `E0OpportunityHistory` is routing-only. `DeterministicOpportunityAuthority.Replay(...)` is canonical Opportunity replay authority.

### Patch 0014

Production-bound Context v2 is exact-state-bound. CharacterClaim/recent Performance remain deferred. Exact source Context is freshly recomposed at Take binding; StateHash metadata alone is insufficient.

## 4. Exact Patch 0015 question

> Can Ensemble maintain a closed deterministic projection of successfully committed Character-legible Performances for the current E0 Scene, synchronize it through the exact commit/Opportunity StateHash chain, prove each newly recorded commit consumed the exact accumulated history-aware Character Context, and compose that history into later Character Context without turning dialogue/action into projected truth, Observation, Memory, Belief, Claim, provider state, or a second event store?

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

Patch 0014:

```text
ProductionStateCheckpoint + Access + Context -> Continuity
```

Patch 0015:

```text
Context semantic recent item
    -> opaque accepted Performance history DATA in CausalCommit

CausalCommit + Opportunity + Context continuity
    -> accepted history ADVANCEMENT in Continuity

opaque accepted history
    -> history-aware Context continuity

opaque accepted history
    -> history-aware exact Take source-context proof in CausalCommit
```

Rules:

- Context never depends on CausalCommit, Opportunity, or Continuity.
- CausalCommit may depend on existing lower Context types, as it already does for source binding.
- history data lives low enough for Take binding without CausalCommit -> Continuity/Opportunity.
- advancement lives in Continuity because it observes Context continuity + CausalCommit replay + Opportunity replay.
- Opportunity remains unchanged and depends on CausalCommit, never reverse.
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

Internally it retains exactly:

```text
ImmutableArray<ContextRecentPerformance> Entries
```

No separate accepted-history entry DTO exists.

### `Ensemble.E0.Core.Context`

Add exactly:

```csharp
public sealed class ContextRecentPerformance
```

Read-only fields:

```text
SourceCharacterId : CharacterId
VisibleText : string
```

Its constructor is Core-internal.

`ContextPacket` adds:

```text
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

### `Ensemble.E0.Core.Continuity`

Add exactly:

```csharp
public static class E0AcceptedPerformanceHistoryContinuity
```

Public methods:

```csharp
E0AcceptedPerformanceHistory Initialize(ProductionState genesisState)

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

No public raw-history/prose composer, transcript collection, entry constructor, history hash, store, or event interface is added.

## 7. What history is — and is not

Internally `E0AcceptedPerformanceHistory` answers:

> Which Character-safe recent-Performance semantic items were successfully admitted through the live accepted commit chain, in exact causal order, and to which Production StateHash has this projection been advanced?

Publicly it exposes only Scene/state synchronization identity.

It does not answer proposition truth, Character observation/knowledge/belief/memory/claim, durable consequences, relevance, provider provenance, raw attempts, or durable event reconstruction.

Authoritative current state remains `ProductionState`; authoritative causal event remains `E0CausalCommit`.

History is a **closed in-memory Context projection**, not an event ledger.

## 8. Why history stores only Context-safe semantics

Internal entries are the same immutable semantic DTO later placed in Context:

```text
ContextRecentPerformance
- SourceCharacterId
- VisibleText
```

No CommitId/TakeId/StateHash/ContextPacketId/candidate hash/control is copied per entry.

The accepted provenance is established at append time:

1. exact history-aware source Context is freshly recomposed and matched to committed Candidate source identity;
2. canonical causal replay succeeds;
3. replayed postcommit StateHash becomes the history synchronization anchor;
4. only then are Candidate subject + exact VisibleText projected into history.

The postcommit StateHash cryptographically binds the complete causal commit payload. The history projection does not duplicate that payload.

## 9. `CurrentStateHash` is synchronization, not parallel authority

```text
genesis opportunity-bearing state
    history.CurrentStateHash = genesis StateHash

successful commit replay
    append one ContextRecentPerformance
    history.CurrentStateHash = replayed postcommit StateHash

successful Opportunity replay
    append nothing
    history.CurrentStateHash = replayed opportunity-bearing StateHash
```

History-aware Context/Take require exact Scene + CurrentStateHash equality to checkpoint.

`CurrentStateHash` is not a history hash and is not sufficient to authenticate arbitrarily fabricated history. Supported authority comes from closed initialization/advancement. Durable deserialization/authentication remains future event-store scope.

## 10. History initialization

`Initialize(genesisState)` must call existing:

```csharp
E0OpportunityHistory.Initialize(genesisState)
```

and rely on its exact genesis/roster/opportunity/hash validation.

Result:

```text
SceneId = genesisState.SceneId
CurrentStateHash = genesisState.StateHash
Entries = []
```

No opening transcript is invented.

Expected upstream failures normalize to sanitized `E0AcceptedPerformanceHistoryException`; unexpected programming failures remain technical.

## 11. Commit advancement proves exact history-aware source Context

Exact API:

```csharp
RecordCommit(
    E0AcceptedPerformanceHistory sourceHistory,
    ProductionState parentState,
    E0CausalCommit committedEvent)
```

Required sequence:

1. validate history/event/current parent association;
2. require history Scene == parent Scene;
3. require history CurrentStateHash == parent StateHash;
4. validate internal history semantic shape;
5. capture `ProductionStateCheckpoint` from parent;
6. freshly compose exact expected live Context:

```csharp
var expected = E0ProductionContextContinuity.Compose(checkpoint, sourceHistory);
```

7. require committed event has exact Accepted Take and:

```text
Take.Performance.SubjectCharacterId
    == expected.ContextEvaluation.Packet.SubjectCharacterId
Take.Performance.ContextPacketId
    == expected.ContextEvaluation.Packet.ContextPacketId
```

8. call canonical:

```csharp
DeterministicCausalCommit.Replay(parentState, committedEvent)
```

9. require fresh replayed Scene exact and postcommit CurrentOpportunity null;
10. append exactly one new `ContextRecentPerformance` using committed Candidate subject + exact VisibleText;
11. advance CurrentStateHash to fresh replayed postcommit StateHash.

### Source proof

The expected packet is freshly recomposed from exact synchronized Production + closed history. `ContextPacketId` content-addresses exact canonical structured semantic Context. Candidate/Integrity/Interpretation/Take already bind Candidate source Context identity. Equality therefore proves exact semantic source Context for the committed Performance.

It does not prove provider transport bytes.

### Compatibility firewall

At exact genesis, expected live Context is existing v2. After first accepted Performance, expected live Context is v3. A legacy evolved history-omitting v2 commit may remain valid historically but cannot enter the new history chain because its source ContextPacketId differs from expected v3.

### VisibleText validation

Appended text comes only from successfully replayed Accepted Candidate. Shared defensive history validation must reuse exact Patch 0006 VisibleText invariants through internal-only code reuse, never a duplicate grammar.

### Failure normalization

Expected checkpoint/Context/replay/history failures normalize to sanitized `E0AcceptedPerformanceHistoryException`; no creative/private payload text enters expected public exception representation.

## 12. Opportunity advancement reuses canonical replay and couples projections

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
2. require history CurrentStateHash == postCommit StateHash and Scene exact;
3. require history Entries non-empty;
4. require `sourceCommit.ResultStateHash == postCommitState.StateHash`;
5. require last history semantic entry equals source commit Performance exactly on:

```text
SourceCharacterId == sourceCommit.Take.Performance.SubjectCharacterId
VisibleText == sourceCommit.Take.Performance.VisibleText
```

The synchronized postcommit StateHash already binds the rest of the source commit payload, including CommitId/TakeId/Context/control/consequence identity.

6. require pre-replay alternating-chain relation:

```text
sourceOpportunityHistory.CharacterIds.Length
    == sourceHistory.Entries.Length
```

7. call canonical:

```csharp
DeterministicOpportunityAuthority.Replay(
    postCommitState,
    sourceCommit,
    sourceOpportunityHistory,
    establishedEvent)
```

8. require fresh result routing-history length == `sourceHistory.Entries.Length + 1`;
9. require fresh routing-history last Character == event selected Character == result Production current opportunity;
10. append no Performance;
11. advance only CurrentStateHash to fresh opportunity-bearing StateHash.

At genesis routing count is 1 while Performance count is 0. After each accepted commit counts are equal; after each successful Opportunity transition routing count is Performance count + 1.

Expected failures normalize to sanitized `E0AcceptedPerformanceHistoryException`.

## 13. Boundary-specific failure domains

One internal history validator/projector is shared, but public ownership remains exact:

```text
History transition API
    -> E0AcceptedPerformanceHistoryException

E0ProductionContextContinuity.Compose(checkpoint, history)
    -> E0ContextContinuityException

E0TakeStateBinding.Bind(checkpoint, context, take, history)
    -> E0CausalCommitException
```

Context/Take catch expected history invariant failures and wrap them in their established public domain. Retained expected inner exceptions must be structural/sanitized.

No expected Patch 0015 public exception `Message`, retained expected inner chain, `Data`, or `ToString()` may contain Performance VisibleText, Context prose, private record text, mutation text, provider payload, credentials/secrets, or unknown untrusted snippets.

Unexpected programming/runtime failures are not relabeled as ordinary contract rejection.

## 14. Failure/non-effective paths

No history entry is created for Rejected/Alternate Takes, Integrity reject/another-take, unresolved authority review, malformed Candidate, provider refusal/error/timeout/cancellation, partial stream, failed commit, commit sourced from wrong history Context, failed Opportunity replay, or diagnostic text.

Successfully Accepted zero-mutation and all-consequence-Rejected Takes **do** append because their Performance entered accepted causal history.

## 15. Current-Scene causal order and E0 history window

```text
recentPerformances = every history Entries item, exact append order
```

No sorting, deduplication, relevance, recency count, truncation, summary, paraphrase, token budget, or model compression.

Complete current-Scene history is the unoptimized E0 reference because Patch 0005 deferred ordering to accepted-history authority, Blueprint 0.1 preserves historical texture, and E0 excludes context optimization. Later narrowing requires a new composition contract and controlled experiment.

## 16. Scene boundary

History covers exactly one current E0 Scene. No cross-Scene carryover/retrieval/memory promotion, branches/canon merge, cross-Production continuity, or persistent loading is defined.

## 17. E0 `copresent-trio.v1` recent-Performance eligibility

Current Fixture Dialect v1 accepts exactly:

```text
ensemble.e0.copresent-trio.v1
```

and Missing Raft fixes three co-present Characters for the bounded E0 observation window.

Patch 0015 newly gives that E0-only reference token one narrow recent-Performance meaning:

> Successfully committed Candidate `VisibleText` is common Character-legible Scene-performance history for every current roster Character under `ensemble.e0.copresent-trio.v1`.

Only Accepted committed VisibleText is eligible. Typed control, hidden reasoning, provider diagnostics, creator-only state, inferred consequences, and claim-to-truth promotion are excluded.

This is not a global ontology rule that co-presence always implies complete perception. Future private/spatial/inaudible/concealed Performance semantics require explicit observation authority and do not inherit this rule automatically.

Under the E0 reference contract, selective recipient perception is not supported.

## 18. Recent Performance is not CharacterObservation

Patch 0015 preserves:

```text
Event happened
    -> observation eligibility
        -> CharacterObservation
            -> possible Memory / Belief / Claim changes
```

No CharacterObservation is generated. Recent Performance in Context does not make its propositions true or create Knowledge/Belief/Memory/Claim. General hearing/location/attention remains unsolved.

## 19. Typed control remains non-visible

Patch 0006 `AddressedCharacterIds`/`NominatedCharacterId` remain routing/intent metadata and never enter `ContextRecentPerformance` or `RecentPerformanceText`.

## 20. Context v3 semantic shape

`ContextRecentPerformance`:

```text
SourceCharacterId
VisibleText
```

`ContextPacket.RecentPerformances : ImmutableArray<ContextRecentPerformance>`.

Rules:

- v1/v2 exactly empty;
- v3 one or more;
- order exactly accepted append order;
- every source Character resolves exactly once in current roster;
- VisibleText exact, including silence;
- no causal/control/provenance fields enter Character semantic recent history.

## 21. Exact Context v3 contracts

Preserve:

```text
ensemble.e0.context.v1
ensemble.e0.context.full-authorized.v1
ensemble.e0.context.render.v1
ensemble.e0.context.v2
ensemble.e0.context.production-bound.v1
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

Matrix:

```text
v1 + full-authorized.v1 + render.v1
    SourceStateHash null
    RecentPerformances []
    RecentPerformanceText ""

v2 + production-bound.v1 + render.v1
    SourceStateHash initialized
    RecentPerformances []
    RecentPerformanceText ""

v3 + production-bound.accepted-history.v1 + render.v2
    SourceStateHash initialized
    RecentPerformances non-empty
    RecentPerformanceText non-empty
```

Every hybrid fails closed.

## 22. Genesis behavior

History-aware exact genesis with empty initialized history emits existing v2 unchanged; empty-history v3 does not exist.

Non-empty history at genesis and empty history at evolved history-aware continuation fail.

## 23. Historical v1/v2 compatibility and live firewall

Preserve:

```csharp
DeterministicContextComposer.Compose(...)
E0ProductionContextContinuity.Compose(checkpoint)
E0TakeStateBinding.Bind(checkpoint, context, take)
```

Old Production Continuity remains current-state-only v2 compatibility, including evolved Production. Old three-argument Take binding remains v1/v2 compatibility and rejects v3.

The new accepted-history chain advances only through `RecordCommit`, which freshly recomposes required history-aware source Context. Thus evolved legacy v2 cannot enter the live chain after history exists.

Future Full Ensemble Harness orchestration must use history-aware APIs; that orchestrator is not part of Patch 0015.

## 24. History-aware Production Context continuity

Add:

```csharp
E0ProductionContextContinuity.Compose(
    ProductionStateCheckpoint checkpoint,
    E0AcceptedPerformanceHistory history)
```

Flow:

```text
validate checkpoint/state
-> validate history
-> require Scene/StateHash equality
-> fresh Production Access once
-> empty exact genesis: internal v2
-> non-empty non-genesis: internal v3 using exact history Entries
-> require Packet/Trace SourceStateHash == checkpoint
-> return existing Access + Context result
```

Expected history failures normalize to `E0ContextContinuityException`.

## 25. History-aware exact Take binding

Add:

```csharp
E0TakeStateBinding.Bind(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

Live overload:

- allows exact genesis v2 only with empty exact history;
- allows v3 only with non-empty synchronized history;
- rejects v1/evolved v2/history-state mismatches.

V3 proof:

```text
validate checkpoint/history
-> fresh Production Access
-> fresh internal v3 composition from exact ordered history Entries
-> exact canonical structured bytes
-> exact canonical rendered bytes
-> ContextPacketId
-> StructuredContextHash
-> RenderedContextHash
-> SourceStateHash
-> inherited exact Take/StateAuthority snapshot proof
```

Existing three-argument binder remains v1/v2-only compatibility. Expected history failures normalize to `E0CausalCommitException`.

## 26. Shared internal history validation/projector

Use one internal CausalCommit-layer helper.

Validate:

- history non-null;
- Entries not default;
- SceneId/CurrentStateHash initialized;
- every `ContextRecentPerformance` non-null;
- every SourceCharacterId initialized;
- every VisibleText non-null and exact Patch 0006 VisibleText-valid;
- when projecting for current Context, each source Character resolves exactly once in roster.

Do **not** require uniqueness of entries: identical visible Performances by the same Character at different accepted moments are legitimate and order-significant.

Do not replay events from history Entries. They intentionally carry only Context-safe semantics.

## 27. Structured v3 canonicalization

V3 root order remains v2:

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

Item order:

```text
sourceCharacterId
visibleText
```

Recent array is causal append order, never sorted. Historical roster/record/relationship ordering unchanged.

`StructuredContextHash = SHA256(canonical v3 structured bytes)` and `ContextPacketId = CTX:<hash>`.

## 28. Render-v2 exact recent-Performance shape

V3 uses `ensemble.e0.context.render.v2` because recent Character-visible content becomes non-empty.

TrustedStateText + OpportunityText algorithms stay byte-identical to render-v1. RecentPerformanceText exact non-silent form:

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

Exact rules: causal order, one blank line between entries, no trailing LF, current roster display-name resolution, no trimming/paraphrase/repair/reorder, existing LF/two-space continuation, no Character/causal IDs rendered.

Patch 0015 inherits Patch 0005 display-name-only rendering and does not invent a new uniqueness law.

## 29. Silence rendering

Structured silence item:

```json
{"sourceCharacterId":"...","visibleText":""}
```

Rendered:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

Literal non-silent text `[PERFORMANCE: SILENCE]` renders in ordinary bullet form and cannot collide.

## 30. Recent Performance remains untrusted creative content

RecentPerformanceText stays separate from TrustedStateText. Future provider framing must preserve authority separation. Prompt-like prior dialogue gains no system authority. Provider request framing remains deferred.

## 31. Character self-history

All prior accepted current-Scene Performances remain eligible for every later current Character, including that Character’s own prior Performance. No unsupported forgetting rule is introduced; this still does not create durable Memory.

## 32. Minimal-disclosure reconciliation

E0 reference disclosure remains only:

```text
current Character-safe Access projection
+ common accepted Character-legible current-Scene Performance history
+ current opportunity
```

Production-only truth, other-private state, denied audit rows, provenance, typed control, provider diagnostics, and creator-only data remain excluded.

Complete current-Scene history is an E0 no-optimization rule, not final product context policy.

## 33. Context trace remains unchanged

Do not add history count/source/causal IDs to `ContextCompositionTrace`. Packet recent semantics + exact hashes already describe emitted Character history; causal event provenance remains outside Context.

## 34. No separate history hash

Do not add `PerformanceHistoryHash`. Production StateHash is transition authority; v3 Context hashes exact disclosed history. A separate history hash would create parallel-looking authority without durable reconstruction value.

## 35. Causal commit / Opportunity history / accepted history remain distinct

- `E0CausalCommit`: authoritative causal event.
- `E0OpportunityHistory`: routing order.
- `E0AcceptedPerformanceHistory`: opaque live projection of Context-safe accepted Performance semantics.

No layer absorbs another.

## 36. Production projection unchanged

Recent Performance history never enters `ProductionStateProjection`. Historical texture stays separate from durable current-state consequences; Patch 0012/0013 hashes remain unchanged.

## 37. CharacterClaim remains deferred

Accepted factual-sounding dialogue stays Performance occurrence, not truth/Knowledge/Belief/Memory/CharacterClaim. Patch 0014 CharacterClaim denial remains exact.

## 38. Observation-contract representation limitation

Production does not separately carry fixture ObservationContract. Current Fixture Dialect v1 supports exactly `ensemble.e0.copresent-trio.v1`; therefore Patch 0015’s common Character-legible Performance eligibility is explicitly an E0 Fixture Dialect v1 reference rule only.

If future dialects support multiple observation contracts, active observation/disclosure authority must become explicit and this assumption must be reopened.

## 39. Multi-turn live induction

```text
history at current opportunity StateHash
+ established commit
    -> exact history-aware source Context proof
    -> canonical commit replay
    -> append one safe Performance semantic item
    -> postcommit history anchor
+ matching source OpportunityHistory + established Opportunity
    -> canonical Opportunity replay
    -> append nothing
    -> next opportunity history anchor
```

Repeat. No durable event reconstruction, persistence, or reverse discovery is claimed.

## 40. Supported-path splice/skip resistance

Normal APIs reject stale/wrong-Scene history, skipped commit/Opportunity advancement, wrong source Context, foreign/tampered events, mismatched routing/performance counts, and evolved empty history.

Because history constructors are closed and Entries internal immutable Context DTOs, ordinary external callers cannot alter history contents through supported C# construction.

Reflection/runtime corruption and durable-store authentication are not claimed.

## 41. Existing downstream semantic contracts

Exact v3 must pass existing `PerformerCandidateContract -> Integrity -> State Interpretation -> State Authority -> E0Take.Bind` without public semantic redesign. Any implementation-only hardcoded v1/v2 assumption is patched narrowly if found.

## 42. Historical canonical compatibility authority

Preserve exactly:

```text
Context v1 VOSS:
2569 structured bytes
bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b
1905 rendered bytes
ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88

Context v2 genesis VOSS:
StateHash 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
2655 structured bytes
27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
1905 rendered bytes
ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88

Context v2 evolved MARLOWE:
StateHash dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
3456 structured bytes
9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
2389 rendered bytes
9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d

Production chain:
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

## 43. Canonical first v3 oracle

Use existing Patch0012 -> Patch0013 chain:

```text
VOSS Performance = "No."
postcommit StateHash = 057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
next Character = MARLOWE
current StateHash = dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
history semantic Entries = [ { VOSS, "No." } ]
current Context = MARLOWE v3
```

Before native validation, independently derive exact v3 structured bytes/hash/ContextPacketId and render-v2 bytes/hash after reproducing inherited oracles. Also prove multi-entry order and silence distinction.

## 44. Narrow inherited-test supersession

Narrowly update `Patch0014ContractAuditTests` temporary “not yet” assertions only:

- ContextPacket.RecentPerformances now exists;
- exactly one new public ContextRecentPerformance type exists;
- exactly three v3/render-v2 constants added;
- Production Continuity has two public Compose overloads.

Historical Patch0014 evidence remains immutable.

## 45. Required implementation tests if approved

### History/public surface
- genesis init succeeds, non-genesis rejects;
- public history properties exactly SceneId + CurrentStateHash;
- no public history constructor/setter/Entries/count/entry type;
- history internal Entries type exactly `ImmutableArray<ContextRecentPerformance>`;
- history exception publicly catchable/no public constructor.

### Commit advancement/source proof
- exact genesis v2-sourced commit appends once;
- state anchor advances to replayed postcommit hash;
- stale/foreign/tampered rejects;
- after one Performance exact v3-sourced commit appends;
- legacy evolved-v2-sourced commit rejected from live history even if historically replayable;
- changed/dropped/reordered/extra history changes expected ContextPacketId and blocks advancement;
- zero-mutation and all-consequence-Rejected Accepted commits append;
- rejected/alternate/noncommit do not append;
- appended semantic item has only source Character + exact VisibleText.

### Opportunity advancement
- exact event replays;
- last history semantic item matches source commit Performance subject + exact VisibleText;
- source commit result hash == current postcommit state/history anchor;
- pre routing-history length == Performance Entries length;
- post routing length == Entries length + 1;
- no Performance append;
- stale/foreign routing/event rejects.

### Failure domains/privacy
- history APIs -> history exception;
- history-aware Context -> E0ContextContinuityException;
- history-aware Take -> E0CausalCommitException;
- expected exception representation contains no creative/private/provider/secret payload;
- unexpected failures not swallowed.

### Multi-turn
- 2/3 accepted commits preserve exact causal order;
- identical repeated Performances remain distinct entries;
- same Character may recur;
- commit/Opportunity synchronization holds;
- historical text without durable record persists in Context;
- self prior Performance included;
- no provider-session memory dependency.

### v1/v2 compatibility
- all historical byte/hash oracles exact;
- old one-argument Continuity unchanged;
- old three-argument binder v1/v2 unchanged and rejects v3.

### v3 matrix/canonicalization
- history-aware genesis -> v2;
- live four-arg binder genesis v2 + empty history;
- nonempty -> v3;
- evolved empty rejects;
- exact schema/composition/render combinations only;
- exact root/item order;
- history order intentionally changes hash.

### Disclosure/epistemic separation
- recent item only source Character + VisibleText;
- no control/causal IDs/provenance/private state;
- CharacterClaim remains denied;
- no Observation/Knowledge/Belief/Memory generation;
- claims remain speech, not truth.

### Rendering
- exact single/multiple/multiline/silence forms;
- literal silence marker noncollision;
- no trailing LF;
- exact render-v2 envelope;
- exact Candidate VisibleText Unicode/NFC validation reused.

### Take exactness/downstream compatibility
- synchronized v3 Context/Take binds;
- stale/altered history fails exact recomposition;
- tampered structured/rendered identities fail;
- SourceStateHash alone insufficient;
- v3 flows through existing Candidate/Integrity/Interpreter/Authority/Take semantics;
- second v3-sourced accepted Take commits and RecordCommit independently rechecks source Context.

### Determinism/dependency
- repeat/culture invariance;
- inherited unordered source invariance preserved;
- canonical first v3 independent oracle;
- Context has no higher-layer dependency;
- CausalCommit history contains no Opportunity behavior;
- Production/Opportunity public shapes unchanged;
- no provider/network/Windows/NPU dependency.

## 46. Expected implementation surface

```text
src/Ensemble.E0.Core/CausalCommit/
    opaque E0AcceptedPerformanceHistory
    internal validation/projection helper
    history-aware E0TakeStateBinding overload

src/Ensemble.E0.Core/Continuity/
    E0AcceptedPerformanceHistoryContinuity
    history-aware E0ProductionContextContinuity overload

src/Ensemble.E0.Core/Context/
    ContextRecentPerformance
    ContextPacket.RecentPerformances
    v3/render-v2 constants
    v3 canonicalizer/version validation
    internal v3 composer/rendering

src/Ensemble.E0.Core/Performer/
    at most internal reuse of exact existing VisibleText validation

focused Patch0015 tests
narrow Patch0014ContractAuditTests supersession
```

Preferred no-change semantics: fixture/dialect, Access, Production projection/transitions, Candidate public behavior, Director policy, Opportunity hash, Integrity, Interpreter, State Authority, Take, Harness runtime/provider execution.

## 47. Complexity/memory

Let R=retained records, A=permitted records, B=permitted state bytes, H=accepted current-Scene Performance count, P=total accepted VisibleText bytes.

```text
History validation          O(H + P validation where required)
RecordCommit                fresh Access/Context + commit replay + immutable append
RecordOpportunity           Opportunity replay + O(1) last-entry + routing/history checks
History-aware Context       O(R + A log A + B + H + P)
History-aware Take proof    same fresh Context work + inherited authority proof
```

Immutable append/full-history rendering can make long-run work superlinear. E0 has no turn quota frozen and excludes context optimization, so Patch0015 does not invent a hard history cap. No background work exists; later run-budget/context optimization can bound product behavior under separate authority.

## 48. ARM64/battery suitability

Synchronous deterministic work only at explicit boundaries. No idle polling/background loop/network/filesystem/provider/GPU/NPU/Windows AI/timer/random/emulation path. Compatible by design with current native ARM64 Core/Harness architecture; no measured performance claim.

## 49. Explicit non-scope

No general Observation engine, selective/private Performance perception beyond E0 rule, CharacterClaim disclosure, epistemic promotion, history optimization, cross-Scene retrieval, provider invocation/provenance, retries/spend/streaming, model-assisted Integrity/Interpreter call, full Scene-loop orchestration, Run/store, durable persistence/recovery, full replay from genesis, branch/canon/retcon/rehearsal, World Resolver, WinUI, Windows AI/NPU, MSIX/WACK/Store.

## 50. Proposal 0.7 resolved questions

1. History is an opaque Context projection, not Take/event history API.
2. Internal history payload is exactly `ImmutableArray<ContextRecentPerformance>`; no separate history-entry DTO.
3. Per-entry CommitId/TakeId/StateHash/ContextPacketId/candidate hash are removed because causal StateHash + authoritative commit own that identity.
4. RecordCommit independently proves exact history-aware source Context before canonical replay.
5. RecordOpportunity uses history anchor + source commit result hash + last safe semantic item + routing-history coupling + canonical Opportunity replay.
6. No public history Entries/count.
7. Boundary exception domains preserved/sanitized.
8. No history hash.
9. Context trace unchanged.
10. Self history included.
11. E0 full current-Scene history remains unoptimized reference behavior under the narrowly defined `copresent-trio.v1` Character-legible rule.
12. Legacy evolved v2 remains compatibility only and cannot advance live history after first accepted Performance.

## 51. Recursive audit order

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

## 52. Proposal 0.7 audit status

Proposal 0.7 removes the last obvious quasi-event-store duplication from history. The next complete pass must attack the design from correctness again, particularly:

1. whether StateHash + closed replay/source-Context proof is sufficient once per-entry causal IDs are removed;
2. whether `ContextRecentPerformance` reuse inside the opaque CausalCommit-layer history remains conceptually clean and one-directional;
3. E0 common Character-legible history versus reserved general Observation authority;
4. v1/v2 byte immutability and v3 render/canonical matrix;
5. all normal supported skip/duplicate/reorder/cross-branch sequences;
6. whether any remaining public surface is removable without losing exact live proof.

No implementation, approval evidence, implementation handoff, `CURRENT_STATE.md` update, or promotion until one complete recursive pass finds no material correction or worthwhile simplification and the user explicitly approves the resulting blueprint.
