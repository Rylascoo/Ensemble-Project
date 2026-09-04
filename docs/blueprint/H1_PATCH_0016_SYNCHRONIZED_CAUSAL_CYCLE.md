# H1 Patch 0016 — Synchronized Causal Cycle

Status: **Blueprint Proposal 0.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Authoritative parent `main` checkpoint:

`5186b0ab624165ab9872630592b164bf3764273d`

Program authority:

`Kymaean Architecture & Ship Plan — Proposal 0.7`

Latest completed executable authority:

`H1 Patch 0015 — E0 Accepted Performance History + Context Continuity — Proposal 0.15`

---

## 1. Purpose

Patch 0016 closes the smallest deterministic orchestration seam left immediately above Patch 0015.

Patch 0015 already proves every lower authority needed for one accepted causal cycle, but the live sequence currently exists only as manually composed calls in tests:

```text
opportunity-bearing Production/history state
 -> capture checkpoint
 -> compose exact Character Context
 -> externally obtain/bind an Accepted Take
 -> history-aware precommit proof
 -> causal commit
 -> accepted-Performance history advancement
 -> adopt committed postcommit state/history pair
 -> deterministic Opportunity establishment
 -> accepted-history/Opportunity-history coupling
 -> adopt next opportunity-bearing synchronized state
```

The next patch must make the **state adoption boundaries themselves canonical** without introducing provider execution, retry policy, persistence, or the full E0 run driver.

The key Patch 0015 law is that this is **not one all-or-nothing transition**. There are two valid authority/adoption points:

```text
1. Accepted Take successfully commits
   + accepted Performance history advances
      => postcommit Production/history pair is authoritative

2. Opportunity subsequently establishes
   + Opportunity/history coupling validates
      => next opportunity-bearing synchronized state is authoritative
```

If Opportunity establishment fails after step 1, the accepted causal commit must not disappear merely because the next Opportunity could not yet be established.

Patch 0016 therefore introduces a two-phase deterministic E0 causal-cycle state machine:

```text
OpportunityBearing
    -- CommitAcceptedTake -->
PostCommitAwaitingOpportunity
    -- EstablishOpportunity -->
OpportunityBearing
```

This patch composes existing authority. It does not invent new fictional semantics.

---

## 2. Authority basis

### Frozen Blueprint 0.1

Blueprint 0.1 requires:

- a generated Performance becomes Production history only through an Accepted Take;
- technical provider failure/refusal/timeout/retry must never become fictional action;
- cancelled/rejected/failed partial output is diagnostic only;
- Accepted Performance and approved consequences commit atomically;
- deterministic authority decides state mutation;
- Director manages attention/opportunity rather than world truth;
- E0 uses a minimal developer harness and explicit provenance;
- E0 does not include WinUI, persistence across multiple Scenes, Windows AI/NPU, Store work, full observation, or World Resolver.

Patch 0016 remains wholly inside those boundaries.

### Patch 0015

Patch 0015 freezes and natively exercises:

```text
ProductionStateCheckpoint.Capture
E0ProductionContextContinuity.ComposeWithAcceptedHistory
E0TakeStateBinding.BindWithAcceptedHistory
DeterministicCausalCommit.Commit
E0AcceptedPerformanceHistoryContinuity.RecordCommit
DeterministicOpportunityAuthority.Establish
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
```

It also freezes staged adoption:

- Commit result is not adopted until `RecordCommit(...)` succeeds.
- Opportunity result is not adopted until `RecordOpportunity(...)` succeeds.
- The committed postcommit pair is nevertheless a valid intermediate authority state before Opportunity re-establishment.

Patch 0016 must preserve those laws exactly.

### Approved ship plan 0.7

The ship plan requires H1 deterministic-spine closure before provider-backed E0-A experimentation and explicitly allows provider-neutral attempt/provenance contracts only when required by deterministic orchestration.

Patch 0016 does **not** yet require such a contract. The correct deterministic state machine can be closed around an already-approved Accepted Take first.

---

## 3. Why this boundary comes before provider-attempt provenance

Three candidate next boundaries were considered.

### Candidate A — provider-attempt/provenance first

Rejected for this patch.

Provider attempts must eventually represent success, refusal, timeout, cancellation, malformed/partial output, retry, spend, provider/model identity, and diagnostics. However, those technical attempts should drive an already-correct deterministic fictional state machine.

Freezing attempt orchestration before the state-adoption machine risks coupling diagnostics/retry semantics to an accidental manual call sequence.

### Candidate B — full E0 run orchestrator

Rejected as too broad.

A full runner would combine:

- provider execution;
- attempts/retries/cancellation;
- Candidate parsing;
- Integrity;
- State Interpretation;
- State Authority;
- Take choice;
- causal commit;
- Opportunity advancement;
- run-level limits/provenance.

That would collapse technical execution and fictional authority into one patch and make Blueprint hard-gate failures harder to localize.

### Candidate C — single atomic Accepted-Take-to-next-Opportunity method

Rejected as semantically wrong.

A one-shot method that returns only the final opportunity-bearing state would hide Patch 0015's first authoritative adoption point. If Opportunity establishment then failed, the caller would have no canonical representation of the already-committed accepted Performance.

### Selected boundary

**Two-phase synchronized causal-cycle orchestration.**

It is the smallest layer that turns Patch 0015's proven lower authorities into a canonical live state machine without absorbing provider execution or changing causal semantics.

---

## 4. Dependency direction

Add one new top-level Core namespace:

```text
Ensemble.E0.Core.Orchestration
```

Dependency direction:

```text
Domain / Production / Context / Performer / Integrity /
Interpreter / StateAuthority / Take / CausalCommit /
Opportunity / Continuity / Director
                  ^
                  |
             Orchestration
```

More explicitly:

```text
Orchestration
 -> Production
 -> Context
 -> Continuity
 -> Take
 -> CausalCommit
 -> Opportunity
 -> Director (only through public evaluation type returned by Opportunity authority)
```

No existing lower namespace may depend on `Orchestration`.

`Orchestration` must not depend on:

- Harness;
- provider SDKs;
- network APIs;
- filesystem/persistence;
- clocks/randomness;
- Windows APIs;
- Windows AI;
- GPU/NPU/QNN/ONNX;
- UI;
- package/Store APIs.

---

## 5. Exact public surface candidate

Proposal 0.1 allows exactly five new public types in `Ensemble.E0.Core.Orchestration`:

```csharp
public sealed class E0OpportunityBearingCycleState
public sealed class E0PostCommitCycleState
public sealed class E0OpportunityBearingCycleResult
public static class DeterministicE0CausalCycle
public sealed class E0CausalCycleException : Exception
```

No other public Orchestration type is permitted in Patch 0016.

### 5.1 `E0OpportunityBearingCycleState`

Closed construction: no public constructor or setter.

Public read-only surface:

```text
ProductionState : ProductionState
```

Internal synchronized state:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
OpportunityHistory         : E0OpportunityHistory
```

The histories remain internal to the cycle token so callers cannot accidentally treat three independently carried values as one validated live state. Existing lower-level APIs remain available for historical/test compatibility, but Patch 0016's canonical live orchestration path carries synchronization as one token.

### 5.2 `E0PostCommitCycleState`

Represents the **first Patch 0015 adoption point**.

Closed construction: no public constructor or setter.

Public read-only surface:

```text
ProductionState : ProductionState
Commit          : E0CausalCommit
```

Internal state retained for the second phase:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
SourceContext               : ContextPacket
SourceOpportunityHistory    : E0OpportunityHistory
```

The public `ProductionState` is the committed postcommit state and therefore has no current Opportunity.

The token is not a transaction promise and not a staged/rollback object. It represents already-authoritative committed fiction after `RecordCommit(...)` has succeeded.

### 5.3 `E0OpportunityBearingCycleResult`

Closed construction: no public constructor or setter.

Public read-only surface:

```text
State              : E0OpportunityBearingCycleState
OpportunityEvent   : E0OpportunityTransition
DirectorEvaluation : LeastInterventionDirectorEvaluation
```

This result exposes provenance-relevant Opportunity evidence without returning the lower `E0OpportunityTransitionResult`, whose raw State/history components would duplicate the canonical synchronized wrapper.

### 5.4 `DeterministicE0CausalCycle`

Exactly four public static methods:

```csharp
E0OpportunityBearingCycleState Initialize(
    ProductionState genesisState)

E0ProductionContextContinuityResult ComposeContext(
    E0OpportunityBearingCycleState source)

E0PostCommitCycleState CommitAcceptedTake(
    CommitId commitId,
    E0OpportunityBearingCycleState source,
    ContextPacket sourceContext,
    E0Take acceptedTake,
    E0RecordMaterializationSet materializations)

E0OpportunityBearingCycleResult EstablishOpportunity(
    E0PostCommitCycleState source)
```

No overloads in Patch 0016.

### 5.5 `E0CausalCycleException`

Public/catchable, with no public constructor.

It normalizes expected lower-authority failure at the orchestration stage without exposing Character-visible text, provider payloads, arbitrary imported content, credentials, or secrets in its own message.

---

## 6. Opportunity-bearing state invariant

An `E0OpportunityBearingCycleState` is valid only when all of the following hold.

Let:

```text
S = ProductionState
H = accepted Performance history entries count
O = OpportunityHistory.CharacterIds count
```

Requirements:

1. `S` is initialized and has an initialized current Opportunity.
2. `S.CurrentOpportunityCharacterId` belongs to the current roster.
3. accepted Performance history is exactly synchronized to `S.SceneId` + `S.StateHash` and current roster.
4. Opportunity history is initialized.
5. Opportunity history Scene equals `S.SceneId`.
6. Opportunity history `LastOpportunityStateHash == S.StateHash`.
7. Opportunity history is nonempty.
8. Opportunity history's last Character equals `S.CurrentOpportunityCharacterId`.
9. every Opportunity-history Character belongs to the current roster.
10. exact count induction holds:

```text
O == H + 1
```

Genesis:

```text
H = 0
O = 1
```

After every completed cycle:

```text
H = n
O = n + 1
```

This is the synchronized state from which the next Character Context may be composed.

---

## 7. `Initialize(...)`

Input:

```text
exact genesis ProductionState
```

Algorithm:

1. reject null/uninitialized input through existing lower authorities;
2. call `E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)`;
3. call `E0OpportunityHistory.Initialize(genesisState)`;
4. validate the complete opportunity-bearing invariant from Section 6;
5. return a closed `E0OpportunityBearingCycleState`.

Do not create a public `Bind(arbitraryState, arbitraryHistories)` method in Patch 0016.

Reason:

E0 begins from exact genesis, and later valid opportunity-bearing tokens are produced by this same cycle. An arbitrary public rebinding API would expand the invalid-state surface without an E0 requirement. Future persistence/recovery may define a separate replay/recovery boundary after E0.

---

## 8. `ComposeContext(...)`

This method exists because accepted/opportunity histories are deliberately not exposed publicly from the synchronized state token.

Algorithm:

1. validate the opportunity-bearing invariant;
2. capture `ProductionStateCheckpoint` from the token's Production state;
3. call existing:

```text
E0ProductionContextContinuity.ComposeWithAcceptedHistory(
    checkpoint,
    acceptedHistory)
```

4. return the existing `E0ProductionContextContinuityResult` unchanged.

Expected behavior remains:

```text
exact empty genesis -> historical Production-bound Context v2
synchronized nonempty evolved history -> Context v3
```

Patch 0016 adds no Context schema/rendering/canonicalization change.

This method makes the new live state token usable without making its synchronization components independently public.

---

## 9. `CommitAcceptedTake(...)` — phase one

Input state must be `OpportunityBearing`.

Algorithm:

1. validate source token/invariants;
2. reject null/uninitialized explicit inputs with fixed stage-safe failure behavior;
3. capture a fresh `ProductionStateCheckpoint` from `source.ProductionState`;
4. call existing:

```text
E0TakeStateBinding.BindWithAcceptedHistory(
    checkpoint,
    sourceContext,
    acceptedTake,
    source.AcceptedPerformanceHistory)
```

5. call existing:

```text
DeterministicCausalCommit.Commit(
    commitId,
    source.ProductionState,
    binding,
    materializations)
```

6. treat the commit result as staged;
7. call existing:

```text
E0AcceptedPerformanceHistoryContinuity.RecordCommit(
    source.AcceptedPerformanceHistory,
    source.ProductionState,
    commitResult.Commit)
```

8. only after `RecordCommit(...)` succeeds, validate the new postcommit invariant;
9. return `E0PostCommitCycleState` containing the committed Production state/history plus the source Context and prior Opportunity history required for phase two.

### Phase-one authority law

The returned `E0PostCommitCycleState` is **already authoritative**.

The accepted Take and approved consequences have committed, and accepted Performance history has advanced.

No current Opportunity exists yet.

Failure before step 9 returns no postcommit token and therefore does not authorize adoption of the staged commit result.

---

## 10. Postcommit invariant

Let the postcommit token contain:

```text
P = postcommit ProductionState
C = committed E0CausalCommit
H = postcommit accepted Performance history entries count
OH = retained source OpportunityHistory count
X = retained source Context
```

Requirements:

1. `P.CurrentOpportunityCharacterId` is null.
2. `C.ResultStateHash == P.StateHash`.
3. `C.ParentStateHash` is initialized and differs from the new result hash absent a cryptographic collision.
4. accepted Performance history is synchronized to `P.SceneId` + `P.StateHash` + current roster.
5. accepted history is nonempty.
6. its final semantic item equals `C.Take.Performance` subject + exact `VisibleText`.
7. retained source Opportunity history Scene equals `P.SceneId`.
8. retained source Opportunity history `LastOpportunityStateHash == C.ParentStateHash`.
9. retained source Opportunity history is nonempty.
10. its last Character equals `C.Take.Performance.SubjectCharacterId`.
11. exact count coupling holds:

```text
OH == H
```

12. retained source Context Scene equals `P.SceneId`.
13. retained source Context ID equals `C.Take.Performance.ContextPacketId`.
14. retained source Context subject equals `C.Take.Performance.SubjectCharacterId`.

The postcommit token retains only what phase two needs. It does not add a new persistent event or hash.

---

## 11. `EstablishOpportunity(...)` — phase two

Input state must be `PostCommitAwaitingOpportunity`.

Algorithm:

1. validate the complete postcommit invariant;
2. call existing:

```text
DeterministicOpportunityAuthority.Establish(
    source.ProductionState,
    source.Commit,
    source.SourceContext,
    source.SourceOpportunityHistory)
```

3. treat the Opportunity result as staged;
4. call existing:

```text
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
    source.AcceptedPerformanceHistory,
    source.ProductionState,
    source.Commit,
    source.SourceOpportunityHistory,
    opportunity.Event)
```

5. only after `RecordOpportunity(...)` succeeds, combine:

```text
opportunity.State
+ returned accepted-history advancement
+ opportunity.History
```

6. validate the full opportunity-bearing invariant;
7. return `E0OpportunityBearingCycleResult` exposing:

```text
canonical synchronized State token
Opportunity event
Director evaluation
```

### Phase-two failure law

If phase two fails, the caller still possesses the already-authoritative `E0PostCommitCycleState` returned by phase one.

Patch 0016 does not define retry/recovery/persistence policy for that state. It merely prevents a later Opportunity failure from semantically rolling back an accepted causal commit.

---

## 12. No fictional path for non-Accepted Takes

`CommitAcceptedTake(...)` accepts `E0Take` as a typed input because existing Take is the authority package, but it must require:

```text
Disposition == Accepted
```

This is already re-proved by `BindWithAcceptedHistory(...)` and causal commit authority.

`Rejected` and `Alternate` Takes cannot enter this causal-cycle transition.

Patch 0016 does not decide what a future runner does after rejection/alternate/request-another-take. That belongs with attempt/run orchestration.

---

## 13. Technical attempts remain outside Patch 0016

Patch 0016 intentionally defines no:

- `RunId`;
- `AttemptId`;
- provider/model identity;
- provider request/result DTO;
- partial streaming buffer;
- refusal/timeout/error enum;
- retry counter;
- token/cost record;
- understudy decision;
- cancellation token policy.

Future provider-attempt orchestration will consume `ComposeContext(...)`, perform technical work outside fictional authority, and call `CommitAcceptedTake(...)` **only after** it has produced a valid Accepted Take through the established Candidate/Integrity/Interpreter/StateAuthority/Take chain.

Therefore:

```text
technical failure
   -> diagnostics / attempt policy
   -> no CommitAcceptedTake call
   -> no fictional state transition
```

This directly supports Blueprint hard gates without pretending Patch 0016 implements those diagnostics yet.

---

## 14. No new canonical identity

Patch 0016 adds no:

- event type;
- StateHash algorithm;
- ContextPacketId algorithm;
- CommitId derivation;
- TakeId derivation;
- Opportunity canonicalization;
- history hash;
- cycle hash;
- orchestration contract field inside Production.

The orchestration tokens are in-memory synchronization wrappers only.

Given identical lower-authority inputs, all existing Patch 0015 reference identities must remain exact, including the first live postcommit hash, Opportunity hash, and MARLOWE Context v3 identity.

---

## 15. Failure-domain design

Expected lower-authority failures are normalized by stage to `E0CausalCycleException`.

Recommended fixed public stage messages:

```text
Initialize:
"E0 causal cycle initialization failed."

ComposeContext:
"E0 causal cycle Context composition failed."

CommitAcceptedTake:
"E0 causal cycle accepted Take commit failed."

EstablishOpportunity:
"E0 causal cycle Opportunity establishment failed."
```

The exception may retain the deterministic lower exception as `InnerException` for developer diagnostics only if the recursive implementation audit confirms lower messages contain no forbidden untrusted/private payload.

Do not embed in the new top-level message:

- `VisibleText`;
- Character private Context prose;
- mutation prose;
- provider output;
- imported/user content;
- credentials/secrets.

Expected caught lower exception families are narrowly stage-specific, such as:

```text
E0AcceptedPerformanceHistoryException
E0OpportunityTransitionException
E0ContextContinuityException
E0CausalCommitException
```

Unexpected programming/runtime failures must not be indiscriminately relabeled.

---

## 16. Determinism and memory implications

Patch 0016 adds no new canonical bytes and no new semantic collection.

It retains references to existing immutable state/history/context/event objects for one causal-cycle phase.

### Time complexity

Each method is bounded by the existing lower authority it composes.

`Initialize`:

```text
O(existing genesis validation)
```

`ComposeContext`:

```text
O(existing Access + Context composition + accepted-history projection)
```

`CommitAcceptedTake`:

```text
O(history-aware binding + causal commit + causal replay/history validation)
```

`EstablishOpportunity`:

```text
O(Director proposal + Opportunity canonicalization/replay + history coupling)
```

No additional polling, parallelism, retry loop, timer, or background task is introduced.

### Memory

Cycle tokens contain references to immutable existing objects. They do not duplicate Production/event payloads by design.

The postcommit token temporarily retains the source Context and prior Opportunity history because those exact objects are required to establish/replay the next Opportunity. This lifetime ends when the caller replaces it with the returned next opportunity-bearing token.

---

## 17. ARM64 and battery suitability

Patch 0016 is synchronous deterministic CPU/memory orchestration only.

It introduces:

- no x86 dependency;
- no emulation;
- no network;
- no provider SDK;
- no filesystem;
- no idle/background work;
- no clock/randomness;
- no GPU/NPU wake;
- no Windows API.

Therefore it preserves the current low-idle-cost ARM64 architecture by construction. No measured performance/battery claim is made until machine evidence exists.

---

## 18. Expected implementation surface

If approved, the preferred source surface is:

```text
src/Ensemble.E0.Core/Orchestration/
    E0CausalCycleModels.cs
    DeterministicE0CausalCycle.cs
```

Potentially one internal invariant helper may share one of those files if justified. Do not create an extra project/assembly for this E0 boundary.

Expected tests:

```text
tests/Ensemble.E0.Core.Tests/Orchestration/
    E0CausalCycleContractAuditTests.cs
    E0CausalCycleTests.cs
    E0CausalCycleDeterminismTests.cs
```

Existing Patch 0015 helper/tests may be narrowly refactored to exercise the new canonical live cycle where doing so removes duplicated manual orchestration, provided historical oracle coverage remains exact.

No Harness/provider implementation is part of Patch 0016.

---

## 19. Required tests

### Closed public surface

Prove exactly five new public Orchestration types and exact method/property signatures.

Prove:

- state/result classes have no public constructors/setters;
- exception has no public constructor;
- no public Orchestration surface mentions provider, network, filesystem, Windows, clock, Task/thread/timer, GPU/NPU/QNN/ONNX, persistence/repository types.

### Initialize

- exact Missing Raft genesis initializes;
- null/non-genesis/invalid genesis fails closed through the approved stage failure;
- initial current Opportunity and Context v2 identity remain exact.

### Opportunity-bearing invariant

Using implementation-internal/reflection adversarial construction where justified:

- stale accepted history rejected;
- stale Opportunity history rejected;
- wrong Scene rejected;
- wrong last Opportunity Character rejected;
- current Opportunity mismatch rejected;
- count mismatch rejected;
- roster mismatch rejected.

### Compose Context

- genesis produces exact historical v2 oracle;
- evolved state produces exact v3 oracle;
- repeated calls over identical state return byte/identity-equivalent existing Context results;
- no new schema/version/hash.

### Commit phase

- exact Patch 0015 first live Accepted Take produces exact existing postcommit StateHash;
- postcommit state has no current Opportunity;
- accepted history advances exactly once;
- source Opportunity history does not advance during phase one;
- Rejected/Alternate Take rejected;
- stale/tampered source Context rejected through history-aware binding;
- stale source state cannot commit;
- duplicate CommitId/TakeId behavior remains inherited;
- record materialization rules remain inherited;
- all-approved, mixed-approved/rejected, zero-mutation/all-rejected accepted Takes preserve existing causal semantics;
- no Opportunity is silently established during phase one.

### Opportunity phase

- exact first live oracle selects MARLOWE and produces exact existing Opportunity StateHash;
- accepted history does not append during Opportunity establishment;
- Opportunity history advances exactly once;
- final state satisfies `O == H + 1`;
- next Context is exact existing MARLOWE v3 oracle;
- multi-turn route remains `VOSS -> MARLOWE -> WREN -> VOSS` under the existing reference sequence;
- repeated identical Performance items/Character recurrence/self-history remain unchanged.

### Two-adoption proof

A dedicated structural/behavioral test must prove phase one and phase two cannot be collapsed accidentally:

- `CommitAcceptedTake(...)` returns an externally retainable postcommit token before any Opportunity call;
- the token's Production state is the exact committed StateHash with no Opportunity;
- `EstablishOpportunity(...)` consumes that token later;
- a deliberately invalid/tampered phase-two token or downstream failure must not change the already-returned phase-one Production object/history semantics.

This is the central Patch 0016 regression law.

### Dependency direction

- existing lower namespaces do not reference `Ensemble.E0.Core.Orchestration`;
- Orchestration adds no provider/platform/persistence dependency;
- Harness remains unchanged unless a later separately approved patch uses the cycle.

---

## 20. Historical compatibility

Patch 0016 does not delete or alter the existing public lower-authority APIs merely because it introduces the preferred live composition path.

Historical APIs remain required by existing contracts/tests and for precise lower-layer testing:

```text
E0ProductionContextContinuity.Compose / ComposeWithAcceptedHistory
E0TakeStateBinding.Bind / BindWithAcceptedHistory
DeterministicCausalCommit.Commit / Replay
E0AcceptedPerformanceHistoryContinuity.Initialize / RecordCommit / RecordOpportunity
DeterministicOpportunityAuthority.Establish / Replay
E0OpportunityHistory.Initialize
```

The new Orchestration layer composes them; it does not become their replacement authority.

No imaginary external compatibility obligation is created for Patch 0016's new wrapper API before shipping.

---

## 21. Explicit non-scope

Patch 0016 does **not** implement or decide:

- provider/model invocation;
- provider request/attempt/retry/spend/streaming provenance;
- cancellation token ownership;
- refusal/timeout/backoff policy;
- understudy selection;
- Candidate generation;
- model-assisted Integrity;
- model-assisted State Interpreter;
- Take review UX;
- full run/Scene loop;
- run termination;
- run-level call/cost budgets;
- immutable E0 run evidence bundle;
- transcript/blind-review generation;
- persistence/recovery;
- cross-Scene history;
- CharacterObservation;
- CharacterClaim disclosure;
- World Resolver;
- branches/retcon/rehearsal;
- WinUI;
- Windows AI / Aion / Phi / LoRA;
- Windows ML / QNN / NPU;
- App Actions/MCP;
- MSIX/IPackageValidator/WACK/Store.

---

## 22. What Patch 0016 unlocks

After Patch 0016 is implemented and natively validated, a later H1/E0-A boundary can define technical Performer attempts against a stable deterministic target:

```text
OpportunityBearingCycleState
 -> ComposeContext
 -> provider attempt(s), diagnostics, retry/cancel/spend policy
 -> Candidate/Integrity/Interpreter/StateAuthority/Take
 -> if Accepted:
      CommitAcceptedTake
      -> authoritative PostCommitCycleState
      -> EstablishOpportunity
      -> next OpportunityBearingCycleState
 -> if not Accepted:
      no causal-cycle commit
```

This makes provider failures structurally incapable of becoming fiction unless a later layer violates an explicit call boundary.

---

## 23. Recursive audit closure criterion

Implementation remains forbidden until one complete fresh audit pass finds:

```text
0 material correctness corrections
0 authority corrections
0 Patch-0015 adoption-law corrections
0 dependency-direction corrections
0 synchronization/invariant corrections
0 failure-domain/privacy corrections
0 canonical/hash/version corrections
0 E0-scope corrections
0 worthwhile public-surface simplifications
0 worthwhile test improvements
0 ARM64/battery/hygiene corrections
0 program-plan inconsistencies
0 evidence corrections
```

Any material or worthwhile correction restarts the audit from the relevant authority layer and increments the Proposal version.

Only after a clean pass may dedicated blueprint-audit evidence be created and explicit Director approval requested.
