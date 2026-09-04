# H1 Patch 0016 — Accepted Take Continuity

Status: **BLUEPRINT PROPOSAL 0.1 — EXPLORATORY; RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Authoritative starting `main`:

`a2b1458509e595847360aaa0729e90a99eb98063`

Program authority:

`docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` — Director-approved Proposal 0.7

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
+ already-bound Accepted E0Take
+ explicit CommitId/materializations
    -> ProductionStateCheckpoint.Capture
    -> E0TakeStateBinding.BindWithAcceptedHistory
    -> DeterministicCausalCommit.Commit
    -> E0AcceptedPerformanceHistoryContinuity.RecordCommit
    -> DeterministicOpportunityAuthority.Establish
    -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
    -> caller manually selects/adopts the final synchronized objects
```

Every individual authority is fail-closed, but there is no single canonical deterministic composition that guarantees the caller receives a new live state only after **all** of those stages succeed.

That absence is the smallest remaining deterministic adoption seam before a provider-attempt/run layer can safely drive the engine.

---

## 2. Why this comes before provider attempts or a full run driver

A provider-attempt boundary must eventually distinguish successful Candidate output from refusal, timeout, cancellation, malformed output, partial streaming, retry and diagnostic-only failures. None of those outcomes may become fiction.

However, those technical outcomes should drive an already-canonical deterministic state transition rather than define it.

Likewise, a full run driver would need both:

1. technical attempt policy; and
2. deterministic adoption of an already-Accepted Take.

Combining both now would conflate technical execution with fictional authority and make failure semantics harder to falsify.

Patch 0016 therefore freezes only the second responsibility.

---

## 3. Frozen scope sentence

> Given one current synchronized E0 live state, the exact source ContextPacket, one already-Accepted E0Take, an explicit CommitId, and explicit record materializations, deterministically produce the next synchronized opportunity-bearing live state only if history-aware source proof, causal commit, accepted-Performance history advancement, Opportunity establishment, and Opportunity-history coupling all succeed.

Nothing in Patch 0016 invokes a model, interprets a technical failure, chooses retry policy, persists state, or decides whether a Candidate/Take should be Accepted.

---

## 4. Architectural position

The new composition belongs in:

`Ensemble.E0.Core.Continuity`

Reason:

- Context continuity already composes Production -> Access -> Context;
- Accepted-Performance continuity already couples causal commit and Opportunity replay to synchronized history;
- the new type coordinates those existing authorities without changing any of them;
- lower layers do not depend upward on Continuity;
- no new provider/application/platform dependency enters Core.

Do **not** create a `Run`, `SceneRunner`, provider, host, Application, or message-oriented abstraction in this patch.

---

## 5. Exact proposed public surface

Add exactly three public Continuity types:

```csharp
public static class E0AcceptedTakeContinuity

public sealed class E0AcceptedTakeContinuityResult

public sealed class E0AcceptedTakeContinuityException : Exception
```

`E0AcceptedTakeContinuityResult` and `E0AcceptedTakeContinuityException` have no public constructors or public setters.

### Exact method

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

### Exact result properties

```text
State                       : ProductionState
AcceptedPerformanceHistory  : E0AcceptedPerformanceHistory
OpportunityHistory          : E0OpportunityHistory
Commit                      : E0CausalCommit
Opportunity                 : E0OpportunityTransition
```

No checkpoint, binding, intermediate postcommit ProductionState, Context clone, Take clone, Director trace, provider data, retry data, timing data, or diagnostics appear on the public result.

The Accepted Take remains available causally through `Commit.Take`; do not duplicate it as another result property.

---

## 6. Exact deterministic algorithm

`Advance(...)` performs this exact order:

```text
1. validate required top-level inputs

2. checkpoint = ProductionStateCheckpoint.Capture(currentState)

3. binding = E0TakeStateBinding.BindWithAcceptedHistory(
       checkpoint,
       sourceContext,
       acceptedTake,
       acceptedPerformanceHistory)

4. commitResult = DeterministicCausalCommit.Commit(
       commitId,
       currentState,
       binding,
       materializations)

5. historyAfterCommit =
       E0AcceptedPerformanceHistoryContinuity.RecordCommit(
           acceptedPerformanceHistory,
           currentState,
           commitResult.Commit)

6. opportunityResult = DeterministicOpportunityAuthority.Establish(
       commitResult.ResultState,
       commitResult.Commit,
       sourceContext,
       opportunityHistory)

7. historyAfterOpportunity =
       E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
           historyAfterCommit,
           commitResult.ResultState,
           commitResult.Commit,
           opportunityHistory,
           opportunityResult.Event)

8. verify final synchronization invariants

9. return one E0AcceptedTakeContinuityResult containing only:
       opportunityResult.State
       historyAfterOpportunity
       opportunityResult.History
       commitResult.Commit
       opportunityResult.Event
```

The implementation must reuse those existing authorities directly. It must not copy their semantic algorithms into a new implementation.

---

## 7. Source Context remains an explicit input

Do **not** internally replace `sourceContext` with a freshly recomposed packet before binding.

Reason:

`BindWithAcceptedHistory(...)` is the existing sole full structured+rendered precommit proof that the supplied source Context is exactly derivable from current Production + accepted history.

If Patch 0016 silently recomposed its own packet and bound against that packet, the full source-Context proof would become partly tautological and would no longer prove the exact packet supplied by the caller.

The future provider-attempt/provenance layer may later prove that this exact `sourceContext` was the packet associated with a real provider request/attempt. Patch 0016 must preserve that seam rather than preempt it.

---

## 8. Accepted-only boundary

`Advance(...)` accepts only an `E0Take` whose disposition is already `Accepted`.

It does not:

- Accept a Take;
- choose Accepted vs Rejected vs Alternate;
- create a Take;
- retry a rejected/alternate Take;
- advance fictional history for Rejected/Alternate/failed technical attempts.

Existing `BindWithAcceptedHistory(...)` remains the authoritative Accepted-only gate and must reject other dispositions.

Patch 0016 may validate the top-level disposition early for a clearer transition-domain failure, but it must not create a second divergent definition of Take eligibility.

---

## 9. Explicit identity/materialization inputs remain caller-owned

`CommitId` and `E0RecordMaterializationSet` remain explicit inputs.

Patch 0016 does not generate IDs, use clocks/randomness, allocate durable record identity policy, or invent persistence semantics.

Reasons:

- deterministic commit identity is already part of the canonical causal hash envelope;
- record materialization is already an explicit authority input;
- future run/persistence layers may own deterministic or externally allocated identity policy;
- generating identities here would silently expand scope into orchestration/persistence policy.

---

## 10. Staged adoption law

All current Core transition objects are immutable and current authorities return new objects rather than mutating their inputs.

Patch 0016 strengthens caller semantics:

> No new live state is exposed by `Advance(...)` unless every required stage through accepted-history `RecordOpportunity(...)` succeeds.

Intermediate values remain local implementation variables only.

In particular, the public result must not expose the staged postcommit `ProductionState` because it is not a supported next live state: it has no current Opportunity and exists only inside the atomic deterministic composition.

If any stage fails, `Advance(...)` throws and returns no partial result. The caller retains its original immutable source objects.

This is an in-memory deterministic adoption guarantee, **not** durable transaction/persistence atomicity.

---

## 11. Final synchronization invariants

Before returning success, the implementation must prove at minimum:

```text
final State.SceneId
    == accepted-history SceneId
    == OpportunityHistory.SceneId

final State.StateHash
    == accepted-history CurrentStateHash
    == OpportunityHistory.LastOpportunityStateHash

final State.CurrentOpportunityCharacterId exists
    == Opportunity.SelectedCharacterId
    == OpportunityHistory.CharacterIds[last]

Opportunity.ResultStateHash
    == final State.StateHash

Commit.ResultStateHash
    == Opportunity.ParentStateHash

Commit.ParentStateHash
    == source currentState.StateHash
```

Because `E0AcceptedPerformanceHistory` fields are intentionally nonpublic, these checks are internal to Core and do not widen its public surface.

The implementation should reuse existing invariant helpers where they already express the required proof rather than reimplementing canonical semantics.

---

## 12. Failure domain

Patch 0016 adds one public catchable transition-domain exception:

`E0AcceptedTakeContinuityException`

with internal constructors only.

Known lower-layer domain failures are translated to bounded stage-specific messages, preserving their exception as `InnerException` where current project practice supports it.

Expected failure messages must not include:

- Candidate VisibleText;
- rendered/private Context text;
- mutation payload text;
- Character-private records;
- provider payloads;
- credentials/secrets;
- arbitrary untrusted snippets.

Unexpected programming/runtime failures are not broadly swallowed or relabeled.

The transition must not catch caller cancellation because Patch 0016 has no async/provider/cancellation API.

---

## 13. No new canonical identity

Patch 0016 adds no canonical event and no new hash envelope.

Canonical identities remain exactly those of:

- `E0CausalCommit`;
- resulting postcommit Production StateHash;
- `E0OpportunityTransition`;
- resulting opportunity-bearing Production StateHash;
- Context v1/v2/v3 identities.

The wrapper result itself has no ID, hash, schema version, canonical serializer, or replay event.

Reason:

The wrapper composes existing canonical authorities; it is not a new fictional fact or durable event.

---

## 14. Replay/persistence boundary

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

## 15. Determinism and historical compatibility

For identical valid inputs, `Advance(...)` must produce exactly the same canonical outcomes as manually composing the current approved authorities in the same order.

Patch 0016 therefore must preserve:

- Patch 0015 live reference oracle StateHashes;
- Context v1/v2/v3 canonical identities;
- Candidate/Take/Commit/Opportunity semantics;
- all historical exact oracles;
- repeated-item/self-history behavior;
- zero-mutation/all-durable-consequence-Rejected accepted-Performance history behavior;
- Director routing behavior.

No historical API is removed or semantically rewritten in this patch.

The manual lower-level authority calls remain valid building blocks/tests; Patch 0016 merely establishes the canonical higher-level live composition for an Accepted Take.

---

## 16. Provider/attempt boundary remains future scope

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

Those belong to the next provider-neutral technical-attempt architecture required by E0-A.

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

That future sequence is illustrative only; Patch 0016 freezes only the Accepted branch.

---

## 17. Complexity / ARM64 / battery

Patch 0016 adds no new asymptotic semantic work beyond the authorities it composes.

It invokes one existing sequence synchronously and holds only bounded references/intermediate immutable objects required for that sequence.

It adds:

- no background work;
- no polling;
- no filesystem/network access;
- no provider SDK;
- no GPU/NPU work;
- no Windows API;
- no clock/randomness;
- no x86 dependency/emulation.

Expected battery impact is therefore only the CPU/memory cost already incurred by the deterministic transition when called. No measured performance claim is made until target-device evidence exists.

---

## 18. Expected implementation surface

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

Reuse `Patch0015TestSupport` or refactor only the smallest test-helper surface needed to compare manual composition and the new composition. Do not alter production algorithms merely to simplify tests.

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

## 19. Required test matrix

### Success/reference equivalence

1. Genesis v2-source accepted oracle transition produces the exact Patch 0015 live postcommit/Opportunity StateHashes and selected MARLOWE.
2. The new wrapper result is canonically equivalent to the existing manual Patch 0015 sequence for identical inputs.
3. Second and later v3-source accepted Takes advance correctly.
4. Multi-turn routing/self-history/duplicate visible Performance behavior remains exact.
5. Accepted zero-mutation Take advances and records Performance.
6. Accepted Take whose durable consequences are all Rejected still records Performance and advances Opportunity.

### Source synchronization failures

7. stale/foreign Production state fails;
8. stale/foreign accepted history fails;
9. stale/foreign Opportunity history fails;
10. tampered/stale source Context fails;
11. source Context subject/opportunity mismatch fails;
12. source Context/history schema mismatch fails.

### Take/commit failures

13. Rejected Take fails with no result;
14. Alternate Take fails with no result;
15. duplicate/uninitialized CommitId fails;
16. invalid/missing record materializations fail;
17. Take/authority/state association tamper fails.

### Opportunity/final synchronization

18. Director address/nomination semantics remain inherited;
19. stale Opportunity routing history fails after staged local commit without exposing a partial result;
20. final state, accepted history, Opportunity history, commit and Opportunity event satisfy the exact synchronization equations.

### Determinism/public surface

21. repeated identical valid inputs produce byte/canonical-equivalent outputs;
22. public result and exception construction is closed;
23. Continuity public namespace adds exactly the three approved types;
24. no public intermediate postcommit Production state property exists;
25. no provider/network/filesystem/time/platform type enters the new API/dependency surface.

### Historical regression

26. full existing Core suite remains green;
27. Patch 0015 reference oracle remains exact;
28. Harness/fixture behavior is unchanged unless implementation unexpectedly changes executable source outside Core, which would require reopening scope.

---

## 20. Explicit non-scope

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

## 21. Why this is not overengineering

The current exact sequence is already duplicated as manual orchestration in tests and is required by every future real Accepted turn.

Centralizing it now provides one material invariant:

> a caller cannot accidentally adopt the postcommit state without also advancing accepted history and establishing/coupling the next Opportunity.

It does not add a new abstraction for a hypothetical future concern; it closes an existing deterministic adoption gap demonstrated by current code.

---

## 22. Approval / implementation boundary

Before approval, recursively audit:

```text
correctness
-> consistency
-> authority ownership
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
0 source-proof corrections
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
