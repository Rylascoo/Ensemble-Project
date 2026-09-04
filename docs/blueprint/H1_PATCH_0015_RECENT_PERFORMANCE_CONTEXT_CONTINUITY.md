# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.8 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden
Parent promoted `main` checkpoint: `7475a9397cff9063673908c666a729f0f3cd4525`
Blueprint branch: `h1-patch-0015-blueprint`

## 1. Purpose

Close the accepted-history boundary deliberately reserved by Patch 0005 and deferred through Patches 0011–0014:

```text
Accepted Take
    -> atomic causal commit
        -> closed accepted-Performance history projection
            -> effective next Opportunity
                -> current Production-backed Access
                    -> bounded Context containing accepted Scene history
                        -> next Performer
```

Patch 0014 proves exact current `ProductionState -> Access -> Context` continuity but requires `recentPerformances = []` and empty `RecentPerformanceText`. Full Ensemble E0 still needs accepted Scene history to carry social causality across independently invoked Performers.

Patch 0015 supplies that deterministic seam without provider/model invocation, durable persistence, or full Scene-loop orchestration.

## 2. Correction history

1. **Generic PerformerInput / Context Consumption rejected.** Blueprint 0.1 and Patch 0006 already freeze `ContextPacket -> Performer/CandidatePerformance`; another generic input layer duplicates authority.
2. **Immediate-one-Performance history rejected.** Accepted historical texture may remain relevant without becoming durable state. Patch 0005 reserved plural `recentPerformances`; E0 therefore uses complete current-Scene accepted Performance order.
3. **Provider-attempt provenance deferred.** Provider framing/attempt/retry/spend/raw-output provenance belongs after semantic Scene continuity exists.
4. **Result-wrapper trust removed.** History advancement reuses `DeterministicCausalCommit.Replay` and `DeterministicOpportunityAuthority.Replay` rather than trusting result wrappers or duplicating transition logic.
5. **Redundant projected causal fields removed.** Per-entry parent/result hashes, candidate hash, CommitId, TakeId, and source ContextPacketId are not copied into history. Current Production StateHash + authoritative causal event own causal identity.
6. **Live source-Context omission closed.** `RecordCommit` freshly recomposes exact history-aware source Context and requires the committed Performance source subject/ContextPacketId to match before canonical replay/appending.
7. **Public history-entry/count surface removed.** History is opaque publicly; Character-safe history appears on `ContextPacket.RecentPerformances`.
8. **Failure domains normalized.** History, Context continuity, and Take binding preserve their own expected public failure domains.
9. **Exception ownership corrected in Proposal 0.8.** The low CausalCommit-layer history data/helper exposes no public history exception. Internal history invariant failures use an internal exception. The public history-transition exception belongs in `Continuity`, where `Initialize/RecordCommit/RecordOpportunity` are owned.

## 3. Authority basis

Frozen authority establishes:

- Character != Performer.
- generated attempt becomes history only through accepted Take + successful atomic causal commit;
- accepted Performance + approved consequence package are one causal event;
- append-only causal event history is conceptual source of truth;
- historical texture may remain true without durable projected-state promotion;
- Context conceptually distinguishes recent events / “what just happened” from trusted state;
- fictional dialogue is untrusted creative content;
- Candidate `VisibleText` is Character-legible Performance; empty text is valid silence;
- typed control is non-visible routing/intent metadata and not generic observation eligibility;
- Rejected/Alternate/failed/partial output never enters Production/recent history;
- Missing Raft E0 uses exactly three co-present Characters;
- general observation/location/hearing/attention remains reserved;
- Patch 0005 reserved non-empty recent-performance item schema/order for a later accepted-history/commit slice;
- Patch 0012 owns atomic accepted commit + replay;
- Patch 0013 owns effective Opportunity + routing history + replay;
- Patch 0014 owns exact Production-bound Context recomposition and keeps recent Performance empty.

## 4. Exact question

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
ContextRecentPerformance
    -> opaque accepted history DATA in CausalCommit

CausalCommit + Opportunity + Context continuity
    -> history ADVANCEMENT in Continuity

opaque history
    -> history-aware Context continuity

opaque history DATA
    -> history-aware exact Take binding in CausalCommit
```

Rules:

- Context never depends on CausalCommit/Opportunity/Continuity.
- CausalCommit may depend on lower Context types, as it already does.
- history data/invariants live low enough for Take binding without CausalCommit -> Continuity/Opportunity.
- history advancement lives in Continuity.
- Opportunity remains unchanged and depends on CausalCommit, never reverse.
- Production projection remains unchanged.

## 6. Exact new public surface

### `Ensemble.E0.Core.CausalCommit`

Add exactly:

```csharp
public sealed class E0AcceptedPerformanceHistory
```

No public constructor/setter. Public properties exactly:

```text
SceneId : SceneId
CurrentStateHash : StateHash
```

Internal property exactly:

```text
Entries : ImmutableArray<ContextRecentPerformance>
```

Internal history invariant failures use an internal-only exception type; no public history exception exists in CausalCommit.

### `Ensemble.E0.Core.Context`

Add exactly:

```csharp
public sealed class ContextRecentPerformance
```

Read-only:

```text
SourceCharacterId : CharacterId
VisibleText : string
```

Core-internal constructor.

`ContextPacket` adds:

```text
RecentPerformances : ImmutableArray<ContextRecentPerformance>
```

### `Ensemble.E0.Core.Continuity`

Add exactly:

```csharp
public static class E0AcceptedPerformanceHistoryContinuity
public sealed class E0AcceptedPerformanceHistoryException : Exception
```

History exception is publicly catchable but has no public constructor.

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

No public entry/transcript collection, history hash, raw-history composer, store, event interface, or generic orchestration abstraction.

## 7. History semantics

Internally history answers which Character-safe recent-Performance semantic items were admitted through the successful live accepted chain, in causal append order, and to which current Production StateHash that projection is synchronized.

Publicly it exposes only Scene/state synchronization identity.

It does not claim proposition truth, epistemic state, consequence authority, relevance, provider provenance, raw attempt history, or durable event reconstruction.

`ProductionState` remains current-state authority. `E0CausalCommit` remains causal-event authority. This object is a closed in-memory Context projection.

## 8. Internal payload is exactly Character-safe recent semantics

History stores only:

```text
ImmutableArray<ContextRecentPerformance>
    SourceCharacterId
    VisibleText
```

No per-entry CommitId/TakeId/StateHash/ContextPacketId/candidate hash/control/provenance.

Accepted provenance is established only at append:

```text
fresh exact history-aware source Context match
-> canonical causal replay succeeds
-> replayed postcommit StateHash becomes synchronization anchor
-> only then project subject + exact VisibleText
```

The postcommit StateHash binds the full authoritative commit payload. History does not partially duplicate it.

## 9. CurrentStateHash is synchronization, not history identity

```text
genesis opportunity state
    history hash anchor = genesis StateHash

successful commit replay
    append one ContextRecentPerformance
    anchor = replayed postcommit StateHash

successful Opportunity replay
    append nothing
    anchor = replayed opportunity StateHash
```

History-aware Context/Take require exact history Scene/StateHash == checkpoint Scene/StateHash.

StateHash equality alone does not authenticate arbitrarily fabricated history. Supported authority depends on closed construction/advancement. Reflection/runtime corruption and durable deserialization authentication are outside this patch.

## 10. Initialization

`E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)` calls:

```csharp
E0OpportunityHistory.Initialize(genesisState)
```

to reuse exact genesis/roster/current-opportunity/hash validation.

Result:

```text
SceneId = genesis.SceneId
CurrentStateHash = genesis.StateHash
Entries = []
```

No opening transcript.

Expected upstream/invariant failures normalize to sanitized Continuity-owned `E0AcceptedPerformanceHistoryException`.

## 11. RecordCommit: exact history-aware source proof before replay

```csharp
RecordCommit(history, parentState, committedEvent)
```

Required order:

1. validate inputs/history shape;
2. require history Scene == parent Scene;
3. require history CurrentStateHash == parent StateHash;
4. capture `ProductionStateCheckpoint` from parent;
5. freshly compose:

```csharp
var expected = E0ProductionContextContinuity.Compose(checkpoint, history);
```

6. require committed event’s exact Accepted Take Performance:

```text
SubjectCharacterId == expected.Packet.SubjectCharacterId
ContextPacketId == expected.Packet.ContextPacketId
```

7. call:

```csharp
DeterministicCausalCommit.Replay(parentState, committedEvent)
```

8. require fresh replayed Scene exact and current opportunity null;
9. append one new `ContextRecentPerformance(subject, exact VisibleText)`;
10. anchor history to fresh replayed postcommit StateHash.

The expected packet is freshly recomposed from exact synchronized Production + history. Its content-addressed ContextPacketId proves exact semantic source Context association. This does not prove provider transport bytes.

### Live compatibility firewall

Genesis live source is exact v2 because history is empty. After first accepted Performance live source must be v3. An evolved legacy history-omitting v2 commit may remain historically replayable but cannot pass `RecordCommit` into the live history chain.

### VisibleText invariant

History validation reuses exact Patch 0006 Candidate VisibleText semantics through internal-only code reuse. No duplicate text grammar.

### Failure ownership

Expected checkpoint/Context/replay/internal-history failures become sanitized `E0AcceptedPerformanceHistoryException`. Unexpected programming/runtime failures remain technical.

## 12. RecordOpportunity: canonical replay + projection coupling

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
2. require history CurrentStateHash == postCommit StateHash and Scene exact;
3. require history non-empty;
4. require sourceCommit.ResultStateHash == postCommit StateHash;
5. require last history semantic item exactly matches source commit Performance on:

```text
SourceCharacterId == sourceCommit.Take.Performance.SubjectCharacterId
VisibleText == sourceCommit.Take.Performance.VisibleText
```

The synchronized postcommit StateHash binds the omitted source commit identities/control/consequences.

6. require pre-replay E0 alternating relation:

```text
sourceOpportunityHistory.CharacterIds.Length == history.Entries.Length
```

7. call canonical:

```csharp
DeterministicOpportunityAuthority.Replay(
    postCommitState,
    sourceCommit,
    sourceOpportunityHistory,
    establishedEvent)
```

8. require fresh routing-history length == `history.Entries.Length + 1`;
9. require fresh routing-history last Character == event selected Character == result-state current opportunity;
10. append no Performance;
11. anchor history to fresh opportunity StateHash.

At genesis routing count=1/history count=0; after commit counts equal; after Opportunity routing count=history count+1.

Expected failures normalize to `E0AcceptedPerformanceHistoryException`.

## 13. Internal invariant failure vs public exceptions

Internal CausalCommit-layer history validation/projecting throws one internal-only exception (name implementation-private).

Public normalization:

```text
E0AcceptedPerformanceHistoryContinuity.*
    -> E0AcceptedPerformanceHistoryException

E0ProductionContextContinuity.Compose(checkpoint, history)
    -> E0ContextContinuityException

E0TakeStateBinding.Bind(checkpoint, context, take, history)
    -> E0CausalCommitException
```

Context/Take never expose the Continuity history-transition exception as a second failure domain.

Expected public exception Message/retained expected inner chain/Data/ToString must not contain Performance VisibleText, Context/private record prose, mutation text, provider payload, credentials/secrets, or unknown untrusted snippets.

Do not catch/relabel arbitrary unexpected exceptions.

## 14. Non-effective paths

No history entry for Rejected/Alternate Take, Integrity rejection/another-take, unresolved authority review, malformed Candidate, provider technical outcome, partial stream, failed commit, wrong-history source Context, failed Opportunity replay, or diagnostics.

Accepted zero-mutation and all-consequence-Rejected Takes do append because accepted Performance occurred in causal history.

## 15. E0 history window/order

For this bounded E0 Scene:

```text
recentPerformances = every history Entries item in append order
```

No sorting/dedup/relevance/window/truncation/summary/paraphrase/token budget/model compression.

Complete current-Scene history is the unoptimized reference. Later optimization requires new composition authority and controlled comparison.

## 16. Scene boundary

One current E0 Scene only. No cross-Scene carryover/retrieval/memory promotion, branches/canon merge, persistent loading, or cross-Production history.

## 17. E0 `copresent-trio.v1` recent-Performance eligibility

Current Fixture Dialect v1 has exactly:

```text
ensemble.e0.copresent-trio.v1
```

and Missing Raft fixes three co-present Characters for the E0 observation window.

Patch 0015 newly defines only this E0 reference behavior:

> Successfully committed Candidate `VisibleText` is common Character-legible Scene-performance history for every current roster Character under `ensemble.e0.copresent-trio.v1`.

Eligible: accepted committed VisibleText only.

Excluded: typed control, hidden reasoning, provider diagnostics, creator-only state, inferred consequences, truth/epistemic promotion.

This is not a global “co-presence means everyone perceives everything” rule. Future private/spatial/inaudible/concealed Performance semantics need explicit observation authority. Selective recipient perception is not supported by this E0 reference contract.

## 18. Recent Performance != CharacterObservation

General future path remains:

```text
Event -> observation eligibility -> CharacterObservation -> possible Memory/Belief/Claim
```

Patch 0015 creates no CharacterObservation and no automatic truth/Knowledge/Belief/Memory/Claim. General hearing/location/attention remains reserved.

## 19. Typed control remains hidden from recent semantics

Patch 0006 addressed/nominated IDs remain non-visible routing/intent metadata and never enter `ContextRecentPerformance` or `RecentPerformanceText`.

## 20. Context v3 semantic shape

Add:

```text
ContextRecentPerformance
- SourceCharacterId
- VisibleText
```

and `ContextPacket.RecentPerformances`.

Rules:

- v1/v2 exactly empty;
- v3 non-empty;
- exact accepted append order;
- each source Character resolves once in current roster;
- exact VisibleText including silence;
- no causal/control/provenance fields.

## 21. Context contracts/version matrix

Preserve exact historical:

```text
ensemble.e0.context.v1
ensemble.e0.context.full-authorized.v1
ensemble.e0.context.render.v1
ensemble.e0.context.v2
ensemble.e0.context.production-bound.v1
```

Add exactly:

```text
E0ContextContracts.AcceptedHistorySchemaVersion
= ensemble.e0.context.v3

E0ContextContracts.AcceptedHistoryCompositionContract
= ensemble.e0.context.production-bound.accepted-history.v1

E0ContextContracts.AcceptedHistoryRenderingContract
= ensemble.e0.context.render.v2
```

Matrix:

```text
v1/full-authorized/render-v1
    SourceStateHash null
    recent [] / rendered recent empty

v2/production-bound/render-v1
    SourceStateHash initialized
    recent [] / rendered recent empty

v3/production-bound.accepted-history/render-v2
    SourceStateHash initialized
    recent non-empty / rendered recent non-empty
```

Every hybrid fails closed.

## 22. Genesis

History-aware exact genesis + empty initialized history emits existing v2 unchanged. Empty v3 is invalid. Non-empty history at genesis and empty history at evolved history-aware state fail.

## 23. Historical compatibility vs live path

Keep old public v1/v2 APIs exact:

```csharp
DeterministicContextComposer.Compose(...)
E0ProductionContextContinuity.Compose(checkpoint)
E0TakeStateBinding.Bind(checkpoint, context, take)
```

Old one-arg Production Continuity remains evolved-v2-compatible; old three-arg binder remains v1/v2 and rejects v3.

Live history advancement is the firewall: after history exists, only exact v3-sourced commits can advance history. Future Full Ensemble Harness orchestration must use history-aware APIs; orchestration remains later scope.

## 24. History-aware Production Context continuity

Add:

```csharp
Compose(checkpoint, history)
```

Flow:

```text
validate checkpoint/history/Scene/StateHash
-> fresh Production Access once
-> empty exact genesis => internal v2
-> non-empty non-genesis => internal v3 from exact history Entries
-> verify Packet/Trace SourceStateHash
-> existing result shape
```

Internal history failure normalizes to `E0ContextContinuityException`.

## 25. History-aware exact Take binding

Add four-argument `Bind(checkpoint, context, take, history)`.

Allows exact genesis v2+empty history or v3+non-empty synchronized history. Rejects v1, evolved v2, and history/state mismatch.

V3 proof recomposes fresh Production Access + exact v3 Context, compares canonical structured/rendered bytes, ContextPacketId, both hashes, SourceStateHash, then runs inherited exact Take/StateAuthority snapshot proof.

Old three-arg binder remains v1/v2 only.

Internal history failure normalizes to `E0CausalCommitException`.

## 26. Shared internal history validator/projector

One CausalCommit-layer helper validates:

- non-null history;
- Entries initialized;
- SceneId/CurrentStateHash initialized;
- each ContextRecentPerformance non-null;
- each SourceCharacterId initialized;
- each VisibleText non-null + exact Patch 0006 VisibleText-valid;
- when projecting current Context, each source Character resolves exactly once in roster.

Identical repeated items are valid and order-significant; do not require uniqueness.

No event replay from history Entries; they intentionally contain only safe semantics.

## 27. Structured v3 canonicalization

Root order exactly preserves v2:

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

Recent array is append order, never sorted. Historical category ordering unchanged.

`StructuredContextHash = SHA256(canonical v3 bytes)`; `ContextPacketId = CTX:<hash>`.

## 28. Render-v2

V3 uses `ensemble.e0.context.render.v2`. TrustedStateText/OpportunityText algorithms remain byte-identical; only RecentPerformanceText becomes non-empty.

Non-silent exact form:

```text
[RECENT PERFORMANCES]
<source display name>:
[PERFORMANCE]
- <first source line>
  <continuation>

<next source display name>:
[PERFORMANCE]
- ...
```

Exact causal order, one blank line between entries, no trailing LF, roster display-name resolution, exact source text, existing LF/two-space continuation, no Character/causal IDs rendered.

Patch 0015 inherits Patch 0005 display-name-only rendering; no display-name uniqueness law is added.

## 29. Silence

Structured silence:

```json
{"sourceCharacterId":"...","visibleText":""}
```

Rendered:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

Literal non-silent marker text renders as ordinary bullet and cannot collide.

## 30. Untrusted creative layer

RecentPerformanceText remains separate from TrustedStateText. Prompt-like fictional history gains no system authority. Provider request framing remains deferred and must preserve authority separation.

## 31. Self-history

A later Character opportunity includes all accepted current-Scene history, including that Character’s own prior Performance. No special forgetting rule. This is not durable Memory.

## 32. Minimal disclosure

Reference Context includes only:

```text
current Character-safe Access projection
+ common accepted Character-legible Scene history
+ current opportunity
```

No Production-only truth, other-private state, denied audit rows, provenance, typed control, provider diagnostics, or creator-only data.

Complete Scene history is E0 no-optimization behavior, not final product policy.

## 33. Context trace unchanged

No history count/source/causal IDs added to trace. Packet recent semantics + hashes identify emitted content; causal provenance stays outside Context.

## 34. No history hash

No `PerformanceHistoryHash`. Production StateHash anchors transitions; v3 Context hashes exact disclosed semantics. Persistence may later define its own authenticated envelope.

## 35. Distinct authorities

- CausalCommit = event authority.
- OpportunityHistory = routing order.
- AcceptedPerformanceHistory = opaque live Context projection.

No layer absorbs another.

## 36. Production projection unchanged

History never enters `ProductionStateProjection`; historical texture stays separate from durable consequences and historical Production hashes remain stable.

## 37. CharacterClaim remains deferred

Accepted claims stay Performance occurrence, not truth/Knowledge/Belief/Memory/CharacterClaim. Patch 0014 CharacterClaim denial stays exact.

## 38. Observation-contract representation limitation

Production does not carry ObservationContract independently. Fixture Dialect v1 supports exactly `copresent-trio.v1`, so this common Character-legible rule is E0-v1-only. Future multiple observation contracts must surface explicit authority and reopen this assumption.

## 39. Multi-turn induction

```text
history at opportunity-bearing state
+ commit event
    -> exact history-aware source Context proof
    -> causal replay
    -> append one safe semantic item
+ matching source routing history + opportunity event
    -> Opportunity replay
    -> append nothing
    -> next opportunity-bearing history anchor
```

Repeat. No durable reconstruction/persistence claim.

## 40. Supported-path splice resistance

Closed constructors + exact state anchors + source Context proof + canonical replays + routing/history count coupling reject stale, skipped, cross-state, wrong-source, foreign/tampered normal API sequences.

No reflection/runtime-corruption/durable-store authentication claim.

## 41. Existing downstream semantics

V3 must traverse existing Candidate -> Integrity -> Interpreter -> State Authority -> Take without public semantic redesign. Any hidden implementation version gate is fixed narrowly only if encountered.

## 42. Historical canonical authority

Must remain exact:

```text
v1 VOSS structured: 2569 / bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b
v1 VOSS rendered:   1905 / ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88

v2 genesis VOSS StateHash: 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
v2 genesis structured: 2655 / 27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
v2 genesis rendered:   1905 / ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88

v2 evolved MARLOWE StateHash: dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
v2 evolved structured: 3456 / 9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
v2 evolved rendered:   2389 / 9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d

Production chain:
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

## 43. First v3 oracle

Canonical chain:

```text
VOSS accepted VisibleText = "No."
postcommit = 05703456...
next Character = MARLOWE
current StateHash = dc7e169f...
history = [ { VOSS, "No." } ]
Marlowe Context = v3
```

Before native validation independently derive exact v3 structured/rendered byte counts + hashes after reproducing inherited oracles. Also prove multi-entry order and silence distinction.

## 44. Narrow Patch 0014 test supersession

Update only temporary Patch0014 “not yet” reflection assertions:

- ContextPacket.RecentPerformances exists;
- one ContextRecentPerformance public type exists;
- three v3/render-v2 constants added;
- Production Continuity has two Compose overloads.

Historical evidence remains immutable.

## 45. Required implementation tests if approved

### Public/history shape
- genesis init/non-genesis reject;
- history public properties exactly SceneId + CurrentStateHash;
- no public constructor/setter/Entries/count/entry type;
- internal Entries exactly `ImmutableArray<ContextRecentPerformance>`;
- public history-transition exception only in Continuity, no public history exception in CausalCommit.

### Commit source proof
- genesis v2 commit appends;
- postcommit anchor exact;
- stale/foreign/tampered reject;
- v3 next commit appends;
- evolved legacy-v2 commit rejected from live history;
- altered history changes expected ContextPacketId and blocks append;
- zero-mutation/all-consequence-Rejected Accepted append;
- nonaccepted paths do not;
- appended item only subject + VisibleText.

### Opportunity coupling
- replay exact;
- sourceCommit.ResultStateHash == postcommit/history anchor;
- last safe item == source Performance subject+VisibleText;
- pre routing count == history count;
- post routing count == history count+1;
- no append; stale/foreign routing/event reject.

### Failure/privacy
- history advancement -> Continuity history exception;
- history-aware Context -> E0ContextContinuityException;
- history-aware Take -> E0CausalCommitException;
- internal invariant exception not public;
- expected failure representation contains no creative/private/provider/secret text;
- unexpected failures not swallowed.

### Multi-turn
- 2/3 accepted commits preserve exact order;
- identical repeated Performance entries remain distinct;
- same Character can recur;
- synchronization across commit/Opportunity cycles;
- historical texture without durable record persists;
- self history included;
- no provider-session memory.

### v1/v2/v3 compatibility
- all historical hashes/bytes exact;
- old one-arg Continuity exact;
- old three-arg binder v1/v2 exact/rejects v3;
- history-aware genesis emits v2;
- nonempty emits v3;
- evolved empty rejects;
- exact version matrix/hybrid rejection/root+item order.

### Disclosure/render
- recent DTO only CharacterId+VisibleText;
- no control/causal/provenance/private data;
- CharacterClaim denied/no epistemic materialization;
- exact single/multi/multiline/silence render;
- literal silence marker noncollision;
- no trailing LF;
- exact Candidate Unicode/NFC invariant reused.

### Take/downstream
- exact synchronized v3 binder succeeds;
- stale/altered history/tampered packet fails;
- SourceStateHash alone insufficient;
- v3 traverses existing Candidate/Integrity/Interpreter/Authority/Take;
- second v3-sourced Accepted Take commits; RecordCommit rechecks exact expected Context source.

### Determinism/dependency
- repeated/culture deterministic;
- unordered source invariance preserved; history order intentionally sensitive;
- independent first-v3 oracle;
- no Context higher dependency;
- no CausalCommit -> Opportunity/Continuity;
- Production/Opportunity public shapes unchanged;
- no provider/network/Windows/NPU dependency.

## 46. Expected implementation surface

```text
CausalCommit:
    opaque history data + internal invariant/projector
    history-aware Take binding

Continuity:
    history transition authority + public transition exception
    history-aware Production Context overload

Context:
    ContextRecentPerformance + packet property
    v3/render-v2 constants/canonicalization/internal composition

Performer:
    at most internal exact VisibleText-validator reuse

tests:
    focused Patch0015 + narrow Patch0014 temporary assertion supersession
```

No semantic changes expected to fixture, Access, Production projection/transitions, Candidate public behavior, Director, Opportunity hash, Integrity, Interpreter, State Authority, Take, or Harness runtime.

## 47. Complexity/memory

Let R=retained records, A=permitted records, B=permitted-state bytes, H=accepted Performance count, P=total VisibleText bytes.

```text
history validation        O(H + P where text validation required)
RecordCommit              fresh Access/Context + causal replay + immutable append
RecordOpportunity         Opportunity replay + routing/history checks
history-aware Context     O(R + A log A + B + H + P)
history-aware Take        fresh Context + inherited authority proof
```

No E0 turn quota is frozen; Patch 0015 does not invent one. Full-history/immutable append may become superlinear over long Scenes; product optimization remains later scope.

## 48. ARM64/battery

Synchronous deterministic work only on explicit boundaries; no idle/background/network/filesystem/provider/GPU/NPU/Windows AI/timer/random/emulation path. Compatible by design with current ARM64 Core/Harness; no measured claim.

## 49. Explicit non-scope

No general Observation engine, selective/private perception beyond E0 rule, CharacterClaim disclosure, epistemic promotion, history optimization, cross-Scene retrieval, provider execution/provenance, retry/spend/streaming, model-assisted Integrity/Interpreter call, full Scene-loop orchestration, Run/store, persistence/recovery, full genesis replay, branching/canon/retcon/rehearsal, World Resolver, WinUI, Windows AI/NPU, MSIX/WACK/Store.

## 50. Proposal 0.8 resolved decisions

1. History is opaque live Context projection, not event/Take history API.
2. Internal payload is exactly immutable `ContextRecentPerformance` items.
3. Causal IDs/hashes/control are not duplicated per entry; current StateHash + authoritative event own them.
4. RecordCommit independently proves exact expected history-aware source Context before replay.
5. RecordOpportunity couples postcommit StateHash, last safe semantic item, routing count, and canonical Opportunity replay.
6. Public history properties only SceneId/CurrentStateHash.
7. Public history-transition exception belongs in Continuity; internal history invariant exception stays nonpublic in CausalCommit; Context/Take preserve their own public exception domains.
8. No history hash or Context trace expansion.
9. Self history included.
10. E0 full current-Scene accepted history remains unoptimized common Character-legible reference behavior under `copresent-trio.v1`.
11. Legacy evolved v2 remains historical compatibility but cannot advance live history after first accepted Performance.

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

## 52. Proposal 0.8 audit status

The next pass must find either a concrete remaining defect/simplification or close the blueprint. Priority attacks:

1. StateHash sufficiency after removing per-entry causal IDs;
2. semantic cleanliness of reusing ContextRecentPerformance internally as history payload;
3. `copresent-trio.v1` scope versus reserved Observation engine;
4. v1/v2 byte preservation under v3 model/canonicalizer additions;
5. skip/duplicate/reorder/cross-state live induction;
6. exception privacy/domain consistency;
7. remaining public-surface necessity.

No implementation, approval evidence, implementation handoff, `CURRENT_STATE.md` update, or promotion until one complete pass finds no material correction or worthwhile simplification and the user explicitly approves the resulting blueprint.
