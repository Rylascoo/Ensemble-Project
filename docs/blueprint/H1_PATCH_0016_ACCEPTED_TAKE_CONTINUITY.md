# H1 Patch 0016 — Accepted Take Continuity

Status: **BLUEPRINT PROPOSAL 0.2 — EXPLORATORY; RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Authoritative current `main` for this proposal:

`99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

The executable source/test tree remains the Patch 0015 baseline; `a2b1458509e595847360aaa0729e90a99eb98063 -> 99e0b9fc7fc346a7276928df8f92a6e133f58c2f` contains documentation/process updates only.

Program authority:

`docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` — Director-approved Proposal 0.7

Workflow authority:

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` — active protocol v0.2

Parent executable authority:

`H1 Patch 0015 — E0 Accepted Performance History + Context Continuity` — Proposal 0.15, native ARM64 Core authority `571/571` PASS.

---

## 1. Problem

Patch 0015 proves every deterministic authority required to evolve one already-Accepted Take into the next opportunity-bearing Production state, but the exact adoption sequence still exists only as caller choreography, most visibly in `Patch0015TestSupport.RunTurn(...)`:

```text
current opportunity-bearing ProductionState
+ synchronized E0AcceptedPerformanceHistory
+ synchronized E0OpportunityHistory
+ exact source ContextPacket
+ already-Accepted E0Take
+ explicit CommitId/materializations
    -> ProductionStateCheckpoint.Capture
    -> E0TakeStateBinding.BindWithAcceptedHistory
    -> DeterministicCausalCommit.Commit
    -> E0AcceptedPerformanceHistoryContinuity.RecordCommit
    -> DeterministicOpportunityAuthority.Establish
    -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
    -> caller manually selects/adopts the final synchronized objects
```

Every individual authority is fail-closed, but there is no single canonical deterministic composition that guarantees:

1. the three source live-state components are mutually synchronized before transition;
2. no staged postcommit object becomes the caller's next supported live state; and
3. the caller receives a new live state only after **all** commit/history/Opportunity coupling succeeds.

That is the smallest remaining deterministic adoption seam before provider-attempt/run policy can safely drive the engine.

---

## 2. Why this comes before provider attempts or a full run driver

A provider-attempt boundary must eventually distinguish successful Candidate output from refusal, timeout, cancellation, malformed output, partial streaming, retry and diagnostic-only failures. None of those outcomes may become fiction.

Those technical outcomes should drive an already-canonical deterministic state transition rather than define it.

A full run driver would need both:

1. technical attempt policy; and
2. deterministic adoption of an already-Accepted Take.

Combining both now would conflate technical execution with fictional authority and make failure semantics harder to falsify.

Patch 0016 freezes only the second responsibility.

---

## 3. Frozen scope sentence

> Given one current synchronized E0 live state, the exact source ContextPacket, one already-Accepted E0Take, an explicit CommitId, and explicit record materializations, deterministically produce the next synchronized opportunity-bearing live state only if source-triple synchronization, history-aware source proof, causal commit, accepted-Performance history advancement, Opportunity establishment, Opportunity-history coupling, and final synchronization all succeed.

Nothing in Patch 0016 invokes a model, interprets a technical failure, chooses retry policy, persists state, or decides whether a Candidate/Take should be Accepted.

---

## 4. Architectural position

The new composition belongs in:

`Ensemble.E0.Core.Continuity`

Reason:

- Context continuity already composes Production -> Access -> Context;
- Accepted-Performance continuity already couples causal commit and Opportunity replay to synchronized history;
- the new type coordinates those existing authorities without changing their semantic algorithms;
- lower layers do not depend upward on Continuity;
- no new provider/application/platform dependency enters Core.

Do **not** create a `Run`, `SceneRunner`, provider, host, Application, persistence, or message-oriented abstraction in this patch.

---

## 5. Exact proposed public surface

Add exactly three public Continuity types:

```csharp
public static class E0AcceptedTakeContinuity

public sealed class E0AcceptedTakeContinuityResult

public sealed class E0AcceptedTakeContinuityException : Exception
```

`E0AcceptedTakeContinuityResult` and `E0AcceptedTakeContinuityException` have no public constructors or public setters.

`E0AcceptedTakeContinuityResult` declares exactly five public instance properties and no declared public methods:

```text
State                       : ProductionState
AcceptedPerformanceHistory  : E0AcceptedPerformanceHistory
OpportunityHistory          : E0OpportunityHistory
Commit                      : E0CausalCommit
Opportunity                 : E0OpportunityTransition
```

### Exact method

`E0AcceptedTakeContinuity` declares exactly one public static method:

```csharp
public static E0AcceptedTakeContinuityResult Advance(
    ProductionState currentState,
    E0AcceptedPerformanceHistory acceptedPerformanceHistory,
    E0OpportunityHistory opportunityHistory,
    ContextPacket sourceContext,
    E0Take acceptedTake,
    CommitId commitId,
    E0RecordMaterializationSet materializations)
```

No overloads in Patch 0016.

No checkpoint, binding, intermediate postcommit ProductionState, Context clone, Take clone, Director trace, provider data, retry data, timing data, or diagnostics appear on the public result.

The Accepted Take remains available causally through `Commit.Take`; do not duplicate it as another result property.

Patch 0016 adds no public contract-version constant because the wrapper is not a new canonical event or serialized protocol.

---

## 6. Source synchronized-live-state preflight

Before staging a binding or commit, `Advance(...)` proves the input live-state triple is synchronized.

Let:

```text
H = acceptedPerformanceHistory.Entries.Length
```

The preflight requires:

```text
acceptedPerformanceHistory.SceneId
    == currentState.SceneId
    == opportunityHistory.SceneId

acceptedPerformanceHistory.CurrentStateHash
    == currentState.StateHash
    == opportunityHistory.LastOpportunityStateHash

opportunityHistory.CharacterIds is initialized and nonempty

opportunityHistory.CharacterIds.Length
    == H + 1

currentState.CurrentOpportunityCharacterId exists
    == opportunityHistory.CharacterIds[last]
```

The accepted-history fields remain internal; no public surface is widened.

The preflight performs only O(1) anchor/count/last-item checks beyond existing downstream validation. It does **not** rescan or recanonicalize full histories. Existing `BindWithAcceptedHistory(...)`, causal replay, Opportunity authority, and history continuity remain responsible for their full semantic validation.

If the source triple is not synchronized, `Advance(...)` fails before staging a causal commit.

---

## 7. Exact deterministic algorithm

`Advance(...)` performs this exact order:

```text
1. validate required top-level inputs

2. checkpoint = ProductionStateCheckpoint.Capture(currentState)

3. validate source synchronized-live-state preflight

4. binding = E0TakeStateBinding.BindWithAcceptedHistory(
       checkpoint,
       sourceContext,
       acceptedTake,
       acceptedPerformanceHistory)

5. commitResult = DeterministicCausalCommit.Commit(
       commitId,
       currentState,
       binding,
       materializations)

6. historyAfterCommit =
       E0AcceptedPerformanceHistoryContinuity.RecordCommit(
           acceptedPerformanceHistory,
           currentState,
           commitResult.Commit)

7. opportunityResult = DeterministicOpportunityAuthority.Establish(
       commitResult.ResultState,
       commitResult.Commit,
       sourceContext,
       opportunityHistory)

8. historyAfterOpportunity =
       E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
           historyAfterCommit,
           commitResult.ResultState,
           commitResult.Commit,
           opportunityHistory,
           opportunityResult.Event)

9. verify final synchronization invariants

10. return one E0AcceptedTakeContinuityResult containing only:
        opportunityResult.State
        historyAfterOpportunity
        opportunityResult.History
        commitResult.Commit
        opportunityResult.Event
```

The implementation must reuse those existing authorities directly. It must not copy their semantic algorithms into a new implementation.

---

## 8. Source Context remains an explicit input

Do **not** internally replace `sourceContext` with a freshly recomposed packet before binding.

`BindWithAcceptedHistory(...)` is the existing sole full structured+rendered precommit proof that the supplied source Context is exactly derivable from current Production + accepted history.

If Patch 0016 silently recomposed its own packet and bound against that packet, the source-Context proof would become partly tautological and would no longer prove the exact packet supplied by the caller.

The future provider-attempt/provenance layer may later prove that this exact `sourceContext` was associated with a real provider request/attempt. Patch 0016 must preserve that seam rather than preempt it.

### Live Context version law

Because the new live path uses `BindWithAcceptedHistory(...)`:

- exact empty genesis requires Production-bound Context v2;
- synchronized nonempty accepted history requires Context v3;
- historical Context v1 is **not** accepted by `E0AcceptedTakeContinuity.Advance(...)`.

Historical v1 remains supported only by the already-existing compatibility APIs and historical tests. Patch 0016 does not delete or alter that compatibility surface.

---

## 9. Accepted-only boundary

`Advance(...)` accepts only an `E0Take` whose disposition is already `Accepted`.

It does not:

- Accept a Take;
- choose Accepted vs Rejected vs Alternate;
- create a Take;
- retry a rejected/alternate Take;
- advance fictional history for Rejected/Alternate/failed technical attempts.

Existing `BindWithAcceptedHistory(...)` remains the semantic Accepted-only authority. Patch 0016 may fail early on an obviously non-Accepted top-level Take for a bounded transition-domain error, but it must not create a divergent definition of Take eligibility.

---

## 10. Explicit identity/materialization inputs remain caller-owned

`CommitId` and `E0RecordMaterializationSet` remain explicit inputs.

Patch 0016 does not generate IDs, use clocks/randomness, allocate durable record identity policy, or invent persistence semantics.

Reasons:

- deterministic commit identity is already part of the canonical causal hash envelope;
- record materialization is already an explicit authority input;
- future run/persistence layers may own deterministic or externally allocated identity policy;
- generating identities here would silently expand scope into orchestration/persistence policy.

Calling `Advance(...)` repeatedly with the **same immutable source state and the same valid explicit identities** is deterministic and may reproduce the same result. Duplicate-identity rejection applies when that identity is already effective in the supplied current Production state, exactly as existing Causal Commit authority defines it.

---

## 11. Staged adoption law

All current Core transition objects are immutable and current authorities return new objects rather than mutating their inputs.

Patch 0016 strengthens caller semantics:

> No new supported live state is exposed by `Advance(...)` unless every required stage through accepted-history `RecordOpportunity(...)` and final synchronization succeeds.

Intermediate values remain local implementation variables only.

In particular, the public result must not expose the staged postcommit `ProductionState` because it has no current Opportunity and is not the supported next live state.

If any stage fails, `Advance(...)` throws and returns no partial result. The caller retains its original immutable source objects.

This is an in-memory deterministic adoption guarantee, **not** durable transaction/persistence atomicity.

---

## 12. Final synchronization invariants

Let source accepted-history count be `H` and source Opportunity-history count therefore be `H + 1`.

Before returning success, prove at minimum:

```text
final State.SceneId
    == final accepted-history SceneId
    == final OpportunityHistory.SceneId

final State.StateHash
    == final accepted-history CurrentStateHash
    == final OpportunityHistory.LastOpportunityStateHash

final State.CurrentOpportunityCharacterId exists
    == Opportunity.SelectedCharacterId
    == final OpportunityHistory.CharacterIds[last]

final accepted-history count
    == H + 1

final OpportunityHistory.CharacterIds.Length
    == H + 2

final accepted-history last SourceCharacterId
    == Commit.Take.Performance.SubjectCharacterId

final accepted-history last VisibleText
    == Commit.Take.Performance.VisibleText

Opportunity.ResultStateHash
    == final State.StateHash

Commit.ResultStateHash
    == Opportunity.ParentStateHash

Commit.ParentStateHash
    == source currentState.StateHash
```

Existing lower authorities already preserve historical prefixes and canonical transition semantics. Patch 0016 adds only O(1) composition checks rather than rescanning history prefixes or reimplementing those algorithms.

---

## 13. One supported live Accepted-Take adoption path

After Patch 0016 implementation is approved, validated, and promoted:

> production/Harness code that performs a complete live E0 Accepted-Take adoption must call `E0AcceptedTakeContinuity.Advance(...)` rather than manually selecting intermediate lower-layer results.

The lower-level public APIs remain valid because they are independent authorities required for:

- unit tests;
- canonical replay;
- dedicated equivalence tests;
- future persistence/recovery composition;
- other separately approved higher-level authorities.

They are not removed or semantically changed.

Historical Patch 0015 test support may retain manual composition where it serves independent regression/oracle evidence. New live orchestration code must not create a second hand-rolled adoption sequence.

---

## 14. Failure domain

Patch 0016 adds one public catchable transition-domain exception:

`E0AcceptedTakeContinuityException`

with internal constructors only.

Known lower-layer domain failures are translated to bounded stage-specific messages. Inner exceptions may be retained only when the caught exception is itself an existing bounded Core-domain failure whose public representation contains no sensitive/untrusted payload text.

Expected public failure representation must not include:

- Candidate VisibleText;
- rendered/private Context text;
- mutation payload text;
- Character-private record content;
- provider payloads;
- credentials/secrets;
- arbitrary untrusted snippets.

Unexpected programming/runtime failures are not broadly swallowed or relabeled.

The transition has no async/provider/cancellation API and therefore does not catch or reinterpret caller cancellation.

---

## 15. No new canonical identity

Patch 0016 adds no canonical event and no new hash envelope.

Canonical identities remain exactly those of:

- `E0CausalCommit`;
- resulting postcommit Production StateHash;
- `E0OpportunityTransition`;
- resulting opportunity-bearing Production StateHash;
- Context v1/v2/v3 identities.

The wrapper result has no ID, hash, schema version, canonical serializer, or replay event.

It composes existing canonical authorities; it is not a new fictional fact or durable event.

---

## 16. Replay/persistence boundary

Patch 0016 is not durable replay.

A future causal event store may persist/replay the existing canonical `E0CausalCommit` and `E0OpportunityTransition` events under a separately approved persistence architecture.

Patch 0016 returns both canonical events so future callers can capture them without exposing the unsupported intermediate postcommit state.

It does not:

- write files/databases;
- define an event-store schema;
- define transaction logs;
- recover after process crash;
- replay from genesis;
- define cross-Scene persistence;
- define branches/canon/retcon.

---

## 17. Determinism and historical compatibility

For identical valid inputs, `Advance(...)` must produce exactly the same canonical outcomes as manually composing the current approved authorities in the same order.

Patch 0016 therefore preserves:

- Patch 0015 live reference oracle StateHashes;
- Context v1/v2/v3 historical identities and v2/v3 live rules;
- Candidate/Take/Commit/Opportunity semantics;
- all historical exact oracles;
- repeated-item/self-history behavior;
- zero-mutation/all-durable-consequence-Rejected accepted-Performance history behavior;
- Director routing behavior.

No historical API is removed or semantically rewritten.

---

## 18. Provider/attempt boundary remains future scope

Patch 0016 has no concept of:

- ProviderId/model ID;
- request/attempt/result ID;
- raw provider output;
- streaming chunks;
- token counts/cost;
- timeout;
- refusal;
- cancellation;
- retry;
- malformed output diagnostic;
- network error;
- model acquisition/readiness;
- AI provenance.

Those belong to the later provider-neutral technical-attempt architecture required by E0-A.

A future run driver should conceptually become:

```text
current synchronized live state
    -> compose exact Character Context
    -> technical Performer attempt(s)
    -> valid Candidate
    -> Integrity / Interpretation / State Authority
    -> Take decision
    -> if Accepted:
           E0AcceptedTakeContinuity.Advance(...)
       else:
           no fictional state advancement under separately frozen run policy
```

That sequence is illustrative only; Patch 0016 freezes only the Accepted branch.

---

## 19. Complexity / ARM64 / battery

Patch 0016 adds no new asymptotic semantic work beyond the existing authorities it composes.

Its own source/final synchronization checks are O(1) in history length. It deliberately avoids another full-history scan; the existing lower authorities retain their already-approved validation/replay costs.

The wrapper is synchronous and holds only bounded references/intermediate immutable objects required for the call.

It adds:

- no background work;
- no polling;
- no filesystem/network access;
- no provider SDK;
- no GPU/NPU work;
- no Windows API;
- no clock/randomness;
- no x86 dependency/emulation.

No measured performance/battery claim is made.

---

## 20. Expected implementation surface

If approved, prefer the smallest canonical surface:

```text
src/Ensemble.E0.Core/Continuity/
    E0AcceptedTakeContinuity.cs

tests/Ensemble.E0.Core.Tests/Continuity/
    AcceptedTakeContinuityTests.cs
    Patch0016ContractAuditTests.cs
```

Narrow inherited adaptation expected:

`ProductionContextContinuityTests.ContinuityPublicSurface_IsClosedAndMinimal`

must add exactly:

```text
E0AcceptedTakeContinuity
E0AcceptedTakeContinuityResult
E0AcceptedTakeContinuityException
```

to the approved Continuity public namespace list while preserving every existing member/signature restriction.

`Patch0015ContractAuditTests.Patch0015_AddsNoPersistenceProviderClockNetworkWindowsOrHardwarePublicSurface` should continue to pass **without relaxation** because the new Continuity signatures contain none of those forbidden dependency classes.

Reuse existing Patch 0015 fixtures/helpers where useful. Keep at least one independent manual-composition helper/test so the wrapper is compared against the lower authorities rather than testing itself against itself.

No semantic source change is expected in:

- Domain;
- Fixture;
- Access;
- Context;
- Performer;
- Director;
- Integrity;
- StateInterpreter;
- StateAuthority;
- Take;
- CausalCommit;
- Opportunity;
- Production.

If implementation requires semantic modification to those authorities rather than narrow reuse/wiring, stop and reopen architecture.

---

## 21. Required test matrix

### Success/reference equivalence

1. Genesis v2-source accepted oracle transition produces exact Patch 0015 postcommit and Opportunity StateHashes and selects MARLOWE.
2. Wrapper result is canonically equivalent to an independent manual Patch 0015 composition for identical inputs.
3. Second and later v3-source Accepted Takes advance correctly.
4. Multi-turn routing/self-history/identical repeated Performance behavior remains exact.
5. Accepted zero-mutation Take records Performance and advances Opportunity.
6. Accepted Take whose durable consequences are all Rejected records Performance and advances Opportunity.

### Source-triple synchronization

7. accepted-history foreign Scene/hash fails before staged commit;
8. Opportunity-history foreign Scene/hash fails before staged commit;
9. Opportunity-history count not equal to accepted count + 1 fails before staged commit;
10. Opportunity-history last Character not equal to current Opportunity fails before staged commit;
11. default/empty Opportunity history fails before staged commit.

### Source Context proof

12. tampered/stale source Context fails;
13. source Context subject/opportunity mismatch fails;
14. source Context/history schema mismatch fails;
15. historical Context v1 is rejected by the new live wrapper even at genesis;
16. exact genesis v2 and evolved v3 remain accepted as defined by Patch 0015.

### Take/commit failures

17. Rejected Take fails with no result;
18. Alternate Take fails with no result;
19. uninitialized CommitId fails;
20. CommitId already effective in the supplied current Production state fails;
21. null or semantically invalid record materializations fail;
22. Take/authority/state association tamper fails through inherited authority.

### Opportunity/final synchronization

23. inherited address/nomination Director semantics remain unchanged;
24. a routing history that passes simple source anchors but fails inherited Opportunity semantic validation yields no partial result;
25. final source/result StateHash equations are exact;
26. final accepted-history count is source H+1 and ends at the committed Performance;
27. final Opportunity-history count is source H+2 and ends at the selected Character;
28. no public intermediate postcommit Production state is exposed.

### Determinism/public surface

29. repeated identical valid immutable inputs produce canonical-equivalent outputs;
30. result has exactly five approved get-only properties, no public constructor/setter/declared method;
31. exception has no public constructor;
32. static authority has exactly one approved method/signature;
33. Continuity namespace adds exactly the three approved public types;
34. no provider/network/filesystem/task/thread/time/random/Windows/hardware type enters the new public signatures.

### Historical/native regression

35. full existing Core suite remains green;
36. Patch 0015 reference oracle remains exact;
37. native Windows ARM64 full Core tests run for the implementation head;
38. native Windows ARM64 Harness build runs because the referenced Core assembly changed;
39. existing Missing Raft and generic-smoke Harness fixtures execute successfully on the same post-change executable tree, unless a machine-observed failure requires patch-first correction.

Static analysis cannot satisfy items 37–39.

---

## 22. Explicit non-scope

Patch 0016 does **not** add:

- provider/model invocation;
- request/attempt/result provenance;
- retry/spend/streaming/cancellation;
- automatic Take selection;
- Rejected/Alternate run policy;
- full Scene/run loop;
- Scene ending;
- general observation eligibility;
- CharacterClaim/epistemic promotion;
- persistence/recovery;
- cross-Scene history;
- relevance/token optimization;
- World Resolver;
- branches/rehearsal/canon/retcon;
- WinUI/Application layer;
- Windows AI/Foundry/QNN/NPU;
- App Actions/MCP;
- MSIX/WACK/Store work.

---

## 23. Why this is not overengineering

The exact sequence already exists as manual orchestration in tests and is required by every future real Accepted turn.

Centralizing it creates one material invariant:

> production/runtime code cannot accidentally adopt a postcommit state while omitting accepted-history advancement or next-Opportunity coupling.

It also establishes one stable deterministic target for the future provider-attempt/run layer.

This is not an abstraction for a hypothetical future concern; it closes a demonstrated current adoption gap.

---

## 24. Approval / implementation boundary

Before approval, recursively audit:

```text
correctness
-> consistency
-> authority ownership
-> source-triple proof
-> source-Context proof
-> staged adoption/failure atomicity
-> history/Opportunity synchronization
-> dependency direction
-> canonical/version compatibility
-> public-surface minimality
-> scope
-> tests
-> simplicity
-> ARM64/battery suitability
-> E0/ship-plan alignment
-> evidence
```

Close architecture only after one complete pass finds:

```text
0 material correctness corrections
0 consistency corrections
0 authority corrections
0 source-triple/source-Context proof corrections
0 synchronization corrections
0 dependency corrections
0 canonical/version corrections
0 failure-behavior corrections
0 scope corrections
0 worthwhile test improvements
0 worthwhile simplifications
0 hygiene/ARM64/vision/evidence corrections
```

Until that occurs and the Director explicitly approves the final proposal:

**IMPLEMENTATION IS FORBIDDEN.**
