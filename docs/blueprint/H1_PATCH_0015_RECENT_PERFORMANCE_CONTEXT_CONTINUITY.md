# H1 Patch 0015 — E0 Accepted Performance History + Context Continuity

Status: Blueprint Proposal 0.12 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden
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
8. **Failure domains normalized.** History transition, Context continuity, and Take binding preserve their own expected public failure domains.
9. **Exception ownership corrected.** Public history-transition exception belongs in Continuity; low CausalCommit history invariants use an internal-only exception.
10. **Precommit live-path law added.** Approved Full Ensemble execution must prove history-aware Take binding before causal commit; `RecordCommit` independently rechecks after commit before projecting history.
11. **Live APIs disambiguated.** The new history-bearing methods are not overloads named identically to historical history-omitting methods. They are explicitly named `ComposeWithAcceptedHistory(...)` and `BindWithAcceptedHistory(...)`. This preserves Patch 0014 APIs byte/source behavior while reducing accidental live-path omission in future orchestration and code review.
12. **Opportunity adoption made fail-closed.** Opportunity establishment and accepted-history advancement form a coordinated immutable live transition. A future orchestrator must not adopt the opportunity-bearing Production result until `RecordOpportunity(...)` has replayed the exact transition and produced the matching advanced history. On failure, the previously synchronized no-opportunity postcommit Production/history pair remains the only supported live pair.
13. **Inherited reflection-suite boundary corrected in Proposal 0.12.** Patch 0012 freezes the entire public `CausalCommit` namespace in `Patch0012ContractAuditTests`; adding the opaque public `E0AcceptedPerformanceHistory` necessarily supersedes exactly that historical type-list assertion. Patch 0015 must update it narrowly while preserving every enduring Patch 0012 CausalCommit restriction. Patch 0014 temporary Context/Continuity “not yet” assertions are likewise evolved narrowly. Historical evidence remains immutable.

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

> Can Ensemble maintain a closed deterministic projection of successfully committed Character-legible Performances for the current E0 Scene, synchronize it through the exact commit/Opportunity StateHash chain, prove before commit and again at history projection that each live accepted Performance consumed the exact accumulated history-aware Character Context, and compose that history into later Character Context without turning dialogue/action into projected truth, Observation, Memory, Belief, Claim, provider state, or a second event store?

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

Extend existing `E0TakeStateBinding` with exactly one new public method:

```csharp
E0TakeStateBinding.BindWithAcceptedHistory(
    ProductionStateCheckpoint checkpoint,
    ContextPacket context,
    E0Take take,
    E0AcceptedPerformanceHistory history)
```

Historical `Bind(checkpoint, context, take)` remains exact and unchanged.

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

History transition methods:

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

Extend `E0ProductionContextContinuity` with exactly:

```csharp
E0ProductionContextContinuity.ComposeWithAcceptedHistory(
    ProductionStateCheckpoint checkpoint,
    E0AcceptedPerformanceHistory history)
```

Historical `Compose(checkpoint)` remains exact and unchanged.

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

Accepted provenance is established at append time:

```text
precommit exact history-aware Take-state binding on approved live path
-> deterministic commit result
-> RecordCommit fresh exact history-aware source Context recheck
-> canonical causal replay succeeds
-> replayed postcommit StateHash becomes synchronization anchor
-> only then project subject + exact VisibleText
```

The postcommit StateHash binds the full authoritative commit payload. History does not partially duplicate it.

## 9. CurrentStateHash is synchronization, not history identity

```text
genesis opportunity state
    history anchor = genesis StateHash

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

`E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)` calls existing `E0OpportunityHistory.Initialize(genesisState)` to reuse exact genesis/roster/current-opportunity/hash validation.

Result:

```text
SceneId = genesis.SceneId
CurrentStateHash = genesis.StateHash
Entries = []
```

No opening transcript. Expected upstream/invariant failures normalize to sanitized Continuity-owned `E0AcceptedPerformanceHistoryException`.

## 11. Approved Full Ensemble live transition sequence

Patch 0015 does not implement the full Scene loop, but freezes the only approved history-bearing transition sequence for later reference orchestration:

```text
current opportunity-bearing ProductionState
+ synchronized E0AcceptedPerformanceHistory
+ synchronized E0OpportunityHistory
    -> ProductionStateCheckpoint.Capture
    -> E0ProductionContextContinuity.ComposeWithAcceptedHistory(checkpoint, history)
         v2 only at exact empty-history genesis
         v3 after any accepted history exists
    -> Performer Candidate
    -> Integrity
    -> State Interpretation
    -> State Authority
    -> E0Take.Bind(..., Accepted)
    -> E0TakeStateBinding.BindWithAcceptedHistory(
           checkpoint,
           exactContext,
           take,
           history)
         MUST succeed before commit
    -> DeterministicCausalCommit.Commit(...)
         commit result is provisional to the live pair
    -> E0AcceptedPerformanceHistoryContinuity.RecordCommit(...)
         independent source-Context recheck + canonical replay
    -> only then adopt postcommit Production + advanced history as synchronized live pair
    -> DeterministicOpportunityAuthority.Establish(
           postcommit Production,
           source commit,
           exact source Context,
           synchronized source OpportunityHistory)
         opportunity result is provisional to the live pair
    -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(...)
         canonical Opportunity replay + history/routing coupling
    -> only then adopt opportunity-bearing Production + advanced accepted history
       + advanced OpportunityHistory as the next synchronized live triple
```

Historical `Compose(checkpoint)` and `Bind(checkpoint, context, take)` remain available for regression/compatibility but are not approved Full Ensemble history-bearing precommit calls.

An evolved history-omitting v2 Context fails `BindWithAcceptedHistory` before a live commit result is produced.

`RecordCommit` independently repeats the source Context proof so an established commit event supplied from an unapproved/legacy path cannot be projected into accepted live history.

Both Production transition results are immutable values. Failure to advance accepted history does not mutate the previously synchronized pair/triple. A future orchestrator must stage each transition result locally and publish/adopt it only after the corresponding accepted-history advancement succeeds.

## 12. RecordCommit: independent exact source proof + replay

Required order:

1. validate inputs/history shape;
2. require history Scene == parent Scene and CurrentStateHash == parent StateHash;
3. capture parent checkpoint;
4. freshly call `ComposeWithAcceptedHistory(checkpoint, history)`;
5. require committed exact Accepted Take Performance SubjectCharacterId + ContextPacketId equal expected packet;
6. call `DeterministicCausalCommit.Replay(parentState, committedEvent)`;
7. require fresh replayed Scene exact and current opportunity null;
8. append one `ContextRecentPerformance(subject, exact VisibleText)`;
9. anchor history to fresh postcommit StateHash.

The content-addressed ContextPacketId proves exact semantic source Context association; provider transport bytes remain unproven/deferred.

History validation reuses exact Patch 0006 VisibleText semantics internally. Expected failures normalize to `E0AcceptedPerformanceHistoryException`; unexpected failures remain technical.

A caller must not adopt a separately obtained postcommit `ProductionState` into the live chain unless this history advancement has also succeeded for the same established commit event and parent state.

## 13. RecordOpportunity: canonical replay + projection coupling

Required order:

1. validate inputs/history;
2. require history CurrentStateHash == postCommit StateHash and Scene exact;
3. require history non-empty;
4. require sourceCommit.ResultStateHash == postCommit StateHash;
5. require last history semantic item matches source commit Performance on SourceCharacterId + exact VisibleText;
6. require `sourceOpportunityHistory.CharacterIds.Length == history.Entries.Length`;
7. call `DeterministicOpportunityAuthority.Replay(postCommitState, sourceCommit, sourceOpportunityHistory, establishedEvent)`;
8. require fresh routing-history length == `history.Entries.Length + 1`;
9. require fresh routing-history last Character == event selected Character == result-state current opportunity;
10. append no Performance;
11. anchor history to fresh opportunity StateHash.

Postcommit StateHash binds omitted commit identity/control/consequence data.

At genesis routing count=1/history count=0; after commit counts equal; after Opportunity routing count=history count+1.

Expected failures normalize to `E0AcceptedPerformanceHistoryException`.

The established Opportunity result remains provisional to the live chain until this method succeeds. If it fails, no advanced accepted history exists and the caller must retain the previously synchronized no-opportunity postcommit Production/history pair rather than adopting the opportunity-bearing state.

## 14. Performance history and trusted consequence may coexist

A single accepted causal event may legitimately appear later in two distinct layers:

```text
RecentPerformances
    exact Character-legible historical Performance occurrence

TrustedStateText
    separately authorized, committed, active, Access-permitted current consequence
```

Never deduplicate/merge/suppress one because wording overlaps. Historical occurrence != durable consequence. Absence of durable consequence does not erase accepted Performance.

## 15. Boundary-specific failure domains

Internal CausalCommit history invariant/projector uses one internal-only exception.

Public normalization:

```text
E0AcceptedPerformanceHistoryContinuity.*
    -> E0AcceptedPerformanceHistoryException

E0ProductionContextContinuity.ComposeWithAcceptedHistory(...)
    -> E0ContextContinuityException

E0TakeStateBinding.BindWithAcceptedHistory(...)
    -> E0CausalCommitException
```

Historical methods retain historical exception behavior.

Expected public failure representation must not contain Performance VisibleText, Context/private record prose, mutation text, provider payload, credentials/secrets, or unknown untrusted snippets. Unexpected failures are not relabeled.

## 16. Non-effective paths

No history entry for Rejected/Alternate Take, Integrity rejection/another-take, unresolved authority review, malformed Candidate, provider technical outcome, partial stream, failed commit, wrong-history source Context, failed Opportunity replay, or diagnostics.

Accepted zero-mutation and all-consequence-Rejected Takes do append because accepted Performance occurred in causal history.

## 17. E0 history window/order

`recentPerformances = every history Entries item in append order` for the current bounded E0 Scene.

No sorting/dedup/relevance/window/truncation/summary/paraphrase/token budget/model compression. Later optimization requires new composition authority and controlled comparison.

## 18. Scene boundary

One current E0 Scene only. No cross-Scene carryover/retrieval/memory promotion, branches/canon merge, persistent loading, or cross-Production history.

## 19. E0 `copresent-trio.v1` eligibility

Fixture Dialect v1 supports exactly `ensemble.e0.copresent-trio.v1`; Missing Raft fixes three co-present Characters for E0.

Patch 0015 newly defines only this E0 reference behavior:

> Successfully committed Candidate `VisibleText` is common Character-legible Scene-performance history for every current roster Character under `ensemble.e0.copresent-trio.v1`.

Eligible: accepted committed VisibleText only.

Excluded: typed control, hidden reasoning, provider diagnostics, creator-only state, inferred consequences, truth/epistemic promotion.

This is not a global “co-presence means everyone perceives everything” law. Future private/spatial/inaudible/concealed semantics require explicit observation authority. Selective recipient perception is outside E0 reference scope.

The token previously had structural/dialect identity but no executable recent-Performance consumer. Patch 0015 is the first contract to give it this narrowly scoped E0 history-disclosure behavior; no prior historical packet bytes or observation records are reinterpreted.

## 20. Recent Performance != CharacterObservation

Future path remains `Event -> observation eligibility -> CharacterObservation -> possible Memory/Belief/Claim`.

Patch 0015 creates no CharacterObservation and no automatic truth/Knowledge/Belief/Memory/Claim. General hearing/location/attention remains reserved.

## 21. Typed control remains hidden

Patch 0006 addressed/nominated IDs remain non-visible routing/intent metadata and never enter `ContextRecentPerformance` or `RecentPerformanceText`.

## 22. Context v3 semantic shape

Add `ContextRecentPerformance(SourceCharacterId, VisibleText)` and `ContextPacket.RecentPerformances`.

Rules:

- v1/v2 require initialized empty array;
- v3 requires initialized non-empty array;
- default/uninitialized array fails every version;
- v3 order exact accepted append order;
- source Character resolves exactly once in current roster;
- exact VisibleText including silence;
- no causal/control/provenance fields.

## 23. Context contracts/version matrix

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
AcceptedHistorySchemaVersion = ensemble.e0.context.v3
AcceptedHistoryCompositionContract = ensemble.e0.context.production-bound.accepted-history.v1
AcceptedHistoryRenderingContract = ensemble.e0.context.render.v2
```

Matrix:

```text
v1/full-authorized/render-v1
    SourceStateHash null
    RecentPerformances initialized empty
    RecentPerformanceText empty

v2/production-bound/render-v1
    SourceStateHash initialized
    RecentPerformances initialized empty
    RecentPerformanceText empty

v3/production-bound.accepted-history/render-v2
    SourceStateHash initialized
    RecentPerformances initialized non-empty
    RecentPerformanceText non-empty
```

Every hybrid/default shape fails closed.

## 24. Genesis

`ComposeWithAcceptedHistory` at exact genesis + empty initialized history emits existing v2 unchanged. Empty v3 invalid. Non-empty history at genesis and empty history at evolved history-aware state fail.

## 25. Historical compatibility vs live methods

Keep historical methods exact:

```csharp
DeterministicContextComposer.Compose(...)
E0ProductionContextContinuity.Compose(checkpoint)
E0TakeStateBinding.Bind(checkpoint, context, take)
```

New live methods have distinct names:

```csharp
ComposeWithAcceptedHistory(...)
BindWithAcceptedHistory(...)
```

No overload ambiguity or silent method substitution.

## 26. History-aware Production Context continuity

`ComposeWithAcceptedHistory(checkpoint, history)`:

```text
validate checkpoint/history/Scene/StateHash
-> fresh Production Access once
-> empty exact genesis => internal v2
-> non-empty non-genesis => internal v3 from exact history Entries
-> verify Packet/Trace SourceStateHash
-> existing result shape
```

Internal history failure normalizes to `E0ContextContinuityException`.

## 27. History-aware exact Take binding

`BindWithAcceptedHistory(checkpoint, context, take, history)`:

- exact genesis v2 + empty history succeeds;
- v3 + non-empty synchronized history succeeds;
- v1/evolved v2/history-state mismatch fail.

V3 proof: fresh Production Access + exact v3 composition; exact structured/rendered canonical bytes; ContextPacketId; StructuredContextHash; RenderedContextHash; SourceStateHash; then inherited exact Take/StateAuthority snapshot proof.

Internal history failure normalizes to `E0CausalCommitException`.

## 28. Shared internal history validator/projector

One CausalCommit-layer helper validates non-null history, initialized Entries/SceneId/StateHash, non-null items, initialized source IDs, exact Candidate VisibleText validity, and current-roster membership during Context projection.

Identical repeated items are valid/order-significant. No event replay from semantic history items.

## 29. Structured v3 canonicalization

Root order preserves v2:

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

Item order: `sourceCharacterId`, `visibleText`.

Recent array append order, never sorted. Historical category ordering unchanged.

`StructuredContextHash = SHA256(canonical v3 bytes)`; `ContextPacketId = CTX:<hash>`.

## 30. Render-v2

V3 uses `ensemble.e0.context.render.v2`. TrustedStateText/OpportunityText algorithms stay byte-identical; only RecentPerformanceText becomes non-empty.

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

Exact causal order, one blank line between entries, no trailing LF, roster display-name resolution, exact text, existing LF/two-space continuation, no Character/causal IDs rendered.

Patch 0015 inherits Patch 0005 display-name-only rendering; no new display-name uniqueness law. Structured v3 attribution remains exact by CharacterId; provider-specific disambiguation/framing remains deferred rather than changing historical roster/display-name semantics in this patch.

## 31. Silence

Structured silence item has empty `visibleText`.

Rendered:

```text
<source display name>:
[PERFORMANCE: SILENCE]
```

Literal non-silent marker text renders through ordinary bullet form and cannot collide.

## 32. Untrusted creative layer

RecentPerformanceText remains separate from TrustedStateText. Prompt-like fictional history gains no system authority. Provider framing remains deferred and must preserve authority separation.

## 33. Self-history

Later opportunities include all accepted current-Scene Performance history, including the current Character’s own earlier Performance. No unsupported forgetting rule; still not durable Memory.

## 34. Minimal disclosure

Reference Context includes only Character-safe Access projection + common accepted Character-legible Scene history + current opportunity. No Production-only truth, other-private state, denied audit rows, provenance, typed control, provider diagnostics, or creator-only data.

Complete Scene history is E0 no-optimization behavior, not final product policy.

## 35. Context trace unchanged

No history count/source/causal IDs added to trace. Packet recent semantics + hashes identify emitted content; causal provenance stays outside Context.

## 36. No history hash

No `PerformanceHistoryHash`. Production StateHash anchors transition identity; v3 Context hashes exact disclosed semantics. Persistence may later define authenticated history/event envelopes.

## 37. Distinct authorities

- CausalCommit = event authority.
- OpportunityHistory = routing order.
- AcceptedPerformanceHistory = opaque live Context projection.

No layer absorbs another.

## 38. Production projection unchanged

History never enters `ProductionStateProjection`; historical texture stays separate from durable consequences and historical Production hashes remain stable.

## 39. CharacterClaim remains deferred

Accepted claims stay Performance occurrence, not truth/Knowledge/Belief/Memory/CharacterClaim. Patch 0014 CharacterClaim denial remains exact.

## 40. Observation-contract representation limitation

Production does not carry ObservationContract independently. Fixture Dialect v1 supports exactly `copresent-trio.v1`, so this common Character-legible rule is E0-v1-only. Future multiple observation contracts must surface explicit authority and reopen this assumption.

## 41. Multi-turn induction

```text
history at opportunity-bearing state
-> ComposeWithAcceptedHistory
-> Candidate/Integrity/Interpretation/Authority/Take
-> BindWithAcceptedHistory MUST succeed
-> causal commit result staged locally
-> RecordCommit independently rechecks/replays/appends
-> adopt synchronized no-opportunity Production/history pair
-> Opportunity establishment result staged locally
-> RecordOpportunity replays/anchors
-> adopt synchronized opportunity-bearing Production/accepted-history/OpportunityHistory triple
-> next ComposeWithAcceptedHistory
```

Repeat. No durable reconstruction/persistence claim.

## 42. Supported-path splice resistance

Closed constructors + exact state anchors + explicitly named required precommit source-history binding + independent RecordCommit source recheck + canonical commit/Opportunity replays + routing/history count coupling + fail-closed staged adoption reject stale, skipped, cross-state, wrong-source, foreign/tampered normal API sequences.

No reflection/runtime-corruption/durable-store authentication claim.

## 43. Existing downstream semantics

V3 must traverse existing Candidate -> Integrity -> Interpreter -> State Authority -> Take without public semantic redesign. Any hidden implementation version gate is fixed narrowly only if encountered.

## 44. Historical canonical authority

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

## 45. First v3 oracle

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

## 46. Narrow inherited-test supersession

Patch 0015 intentionally evolves only assertions whose historical exact-public-surface or “not yet” premise is superseded by this separately approved boundary.

### Patch 0012

`tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012ContractAuditTests.cs`

Narrowly update `NewPublicNamespaces_ContainOnlyApprovedPatch0012Surface` so the CausalCommit namespace expected public types additionally contains exactly:

```text
E0AcceptedPerformanceHistory
```

Do not weaken the same test's Production namespace exact list. Do not weaken `Patch0012_PublicSurface_HasNoPersistenceProviderWindowsOrHardwareDependencies`, `CausalCommit_ExposesNoStateOnlyCommitOrEventRewriteDeleteAPI`, or other enduring Patch 0012 invariants. The new history type must satisfy those enduring restrictions automatically.

### Patch 0014

`tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ContractAuditTests.cs`

Narrowly evolve only the temporary assertions that history/render-v2/live history APIs did not yet exist:

- `ContextPacket.RecentPerformances` now exists;
- one `ContextRecentPerformance` public Context type exists;
- three v3/render-v2 constants are added;
- `E0ProductionContextContinuity` additionally has `ComposeWithAcceptedHistory`;
- `E0TakeStateBinding` additionally has `BindWithAcceptedHistory` where reflected by Patch 0015 tests.

Historical Patch0012/Patch0014 evidence/docs remain immutable and truthful for their checkpoints. No unrelated earlier contract assertion is weakened merely to make Patch 0015 pass.

## 47. Required implementation tests if approved

### Public/history shape
- genesis init/non-genesis reject;
- history public properties exactly SceneId + CurrentStateHash;
- no public constructor/setter/Entries/count/entry type;
- internal Entries exactly `ImmutableArray<ContextRecentPerformance>`;
- public history-transition exception only in Continuity; internal invariant exception nonpublic;
- Patch0012 exact CausalCommit public-type list changes only by `E0AcceptedPerformanceHistory` and all enduring Patch0012 dependency/hardware/persistence restrictions still pass.

### API disambiguation/precommit
- historical `Compose` remains one-argument only; live method exact name `ComposeWithAcceptedHistory`;
- historical `Bind` remains three-argument only; live method exact name `BindWithAcceptedHistory`;
- exact empty-history genesis v2 passes live binder before commit;
- exact nonempty-history v3 passes live binder before commit;
- evolved legacy-v2 and v1 fail live binder before commit;
- full live sequence binds -> commits -> RecordCommit successfully.

### Commit projection defense
- genesis v2 commit appends;
- postcommit anchor exact;
- stale/foreign/tampered reject;
- v3 next commit appends;
- separately produced legacy evolved-v2 commit rejected by RecordCommit defense-in-depth;
- altered history changes expected ContextPacketId and blocks append;
- zero-mutation/all-consequence-Rejected Accepted append;
- nonaccepted paths do not;
- appended item only subject + VisibleText;
- failed RecordCommit returns no advanced history and the prior synchronized opportunity-bearing state/history remain usable.

### Opportunity coupling/adoption
- live Opportunity establishment result is treated as provisional until RecordOpportunity succeeds;
- replay exact;
- sourceCommit.ResultStateHash == postcommit/history anchor;
- last safe item == source Performance subject+VisibleText;
- pre routing count == history count;
- post routing count == history count+1;
- no Performance append;
- stale/foreign routing/event reject;
- failed RecordOpportunity returns no advanced history; the prior synchronized no-opportunity postcommit state/history remain the supported pair;
- successful RecordOpportunity yields an anchor equal to the replayed/established opportunity StateHash and only then permits adoption of the opportunity-bearing state plus advanced OpportunityHistory.

### Trusted-state/recent separation
- same accepted event may appear as recent Performance and separately as accessible durable consequence;
- no semantic dedup/merge;
- no durable consequence still leaves accepted recent Performance.

### Failure/privacy
- history advancement -> Continuity history exception;
- live Context -> E0ContextContinuityException;
- live Take -> E0CausalCommitException;
- internal invariant exception nonpublic;
- expected failure representation contains no creative/private/provider/secret text;
- unexpected failures not swallowed.

### Multi-turn
- 2/3 accepted commits preserve exact order;
- identical repeated Performance entries remain distinct;
- same Character can recur;
- synchronization across staged commit/adopt -> staged Opportunity/adopt cycles;
- earlier accepted texture without durable record persists;
- self history included;
- no provider-session memory.

### v1/v2/v3 compatibility
- all historical hashes/bytes exact;
- old Compose/Bind exact;
- history-aware genesis emits v2;
- nonempty emits v3;
- evolved empty rejects;
- default RecentPerformances rejects all versions;
- exact matrix/hybrid rejection/root+item order.

### Disclosure/render
- recent DTO only CharacterId+VisibleText;
- no control/causal/provenance/private data;
- CharacterClaim denied/no epistemic materialization;
- exact single/multi/multiline/silence render;
- literal silence marker noncollision;
- no trailing LF;
- exact Candidate Unicode/NFC invariant reused;
- duplicate display names do not alter structured CharacterId attribution; no new display-name uniqueness rule is introduced by Patch 0015.

### Take/downstream
- exact synchronized v3 live binder succeeds;
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

## 48. Expected implementation surface

```text
CausalCommit:
    opaque history data + internal invariant/projector
    BindWithAcceptedHistory

Continuity:
    history transition authority + public transition exception
    ComposeWithAcceptedHistory

Context:
    ContextRecentPerformance + packet property
    v3/render-v2 constants/canonicalization/internal composition

Performer:
    at most internal exact VisibleText-validator reuse

tests:
    focused Patch0015
    narrow Patch0012 CausalCommit public-type-list supersession
    narrow Patch0014 temporary history/render/API assertion supersession
```

No semantic changes expected to fixture, Access, Production projection/transitions, Candidate public behavior, Director, Opportunity hash, Integrity, Interpreter, State Authority, Take, or Harness runtime.

## 49. Complexity/memory

Let R=retained records, A=permitted records, B=permitted-state bytes, H=accepted Performance count, P=total VisibleText bytes.

```text
history validation        O(H + P where text validation required)
precommit live binding    O(R + A log A + B + H + P) + inherited authority proof
RecordCommit              fresh Access/Context + causal replay + immutable append
RecordOpportunity         Opportunity replay + routing/history checks
history-aware Context     O(R + A log A + B + H + P)
```

No E0 turn quota is frozen; Patch 0015 does not invent one. Full-history/immutable append may become superlinear over long Scenes; product optimization remains later scope.

## 50. ARM64/battery

Synchronous deterministic work only on explicit boundaries; no idle/background/network/filesystem/provider/GPU/NPU/Windows AI/timer/random/emulation path. Compatible by design with current ARM64 Core/Harness; no measured claim.

## 51. Explicit non-scope

No general Observation engine, selective/private perception beyond E0 rule, CharacterClaim disclosure, epistemic promotion, history optimization, cross-Scene retrieval, provider execution/provenance, retry/spend/streaming, model-assisted Integrity/Interpreter call, full Scene-loop orchestration, Run/store, persistence/recovery, full genesis replay, branching/canon/retcon/rehearsal, World Resolver, WinUI, Windows AI/NPU, MSIX/WACK/Store.

## 52. Proposal 0.12 resolved decisions

1. History is opaque live Context projection, not event/Take history API.
2. Internal payload is exactly immutable `ContextRecentPerformance` items.
3. Causal IDs/hashes/control are not duplicated per entry; current StateHash + authoritative event own them.
4. Approved Full Ensemble live path requires explicitly named `BindWithAcceptedHistory` before causal commit.
5. `RecordCommit` independently repeats exact source-history Context association then canonical replay before projection.
6. Commit result is staged and must not become the live Production/history pair until RecordCommit succeeds.
7. `RecordOpportunity` couples postcommit StateHash, last safe semantic item, routing count, and canonical Opportunity replay.
8. Opportunity result is staged and must not become the live Production/history/OpportunityHistory triple until RecordOpportunity succeeds.
9. Public history properties only SceneId/CurrentStateHash.
10. Public history-transition exception belongs in Continuity; internal invariant exception nonpublic; Context/Take preserve their existing public exception domains.
11. New live Context/Take methods have distinct names rather than overloads, preserving historical APIs and reducing accidental omission.
12. No history hash or Context trace expansion.
13. Trusted durable consequence and recent Performance are separate layers and may coexist without deduplication.
14. Self history included.
15. E0 full current-Scene accepted history is unoptimized common Character-legible reference behavior under `copresent-trio.v1`; Patch 0015 explicitly creates that narrow consumer behavior rather than pretending it was previously executable.
16. Duplicate display-name ambiguity is inherited from existing display-name-only rendering; structured v3 source attribution remains exact by CharacterId, and Patch 0015 does not add a new uniqueness law solely for recent history.
17. Legacy evolved v2 remains historical compatibility but cannot pass the approved live binder or RecordCommit projection after history exists.
18. Patch0012 exact CausalCommit public-type reflection evolves only for `E0AcceptedPerformanceHistory`; enduring Patch0012 no-persistence/no-provider/no-state-only-commit restrictions remain exact.

## 53. Recursive audit order

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

## 54. Proposal 0.12 audit status

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

No implementation, approval evidence, implementation handoff, `CURRENT_STATE.md` update, or promotion until that pass is clean and the user explicitly approves the resulting blueprint.
