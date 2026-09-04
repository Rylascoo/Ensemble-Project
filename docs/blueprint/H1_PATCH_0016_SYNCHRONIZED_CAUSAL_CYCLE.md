# H1 Patch 0016 — Synchronized Causal Cycle

Status: **Blueprint Proposal 0.2 — RECURSIVE ADVERSARIAL AUDIT RESTARTED; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Authoritative parent `main` checkpoint:

`5186b0ab624165ab9872630592b164bf3764273d`

Program authority:

`Kymaean Architecture & Ship Plan — Proposal 0.7`

Latest completed executable authority:

`H1 Patch 0015 — E0 Accepted Performance History + Context Continuity — Proposal 0.15`

---

## 1. Purpose

Patch 0016 closes the smallest deterministic orchestration seam immediately above Patch 0015.

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

The next patch makes the **state adoption boundaries canonical** without introducing provider execution, retry policy, persistence, or the full E0 run driver.

Patch 0015 freezes two distinct adoption points:

```text
1. Accepted Take commits
   + accepted Performance history advances
      => postcommit Production/history pair is authoritative

2. Opportunity subsequently establishes
   + Opportunity/history coupling validates
      => next opportunity-bearing synchronized state is authoritative
```

Therefore Patch 0016 is not one atomic Accepted-Take-to-next-Opportunity call. It is a two-phase deterministic state machine:

```text
OpportunityBearing
    -- CommitAcceptedTake -->
PostCommitAwaitingOpportunity
    -- EstablishOpportunity -->
OpportunityBearing
```

A later Opportunity failure must never semantically erase an Accepted Take whose causal commit/history advancement already succeeded.

Patch 0016 composes existing authority. It invents no new fictional semantics or canonical identity.

---

## 2. Authority basis

### Frozen Blueprint 0.1

Blueprint 0.1 requires:

- a generated Performance becomes Production history only through an Accepted Take;
- technical provider failure/refusal/timeout/retry never becomes fictional action;
- cancelled/rejected/failed partial output remains diagnostic only;
- Accepted Performance and approved consequences commit atomically;
- deterministic authority owns canonical state mutation;
- Director manages attention/opportunity rather than world truth;
- E0 uses a minimal developer harness and explicit provenance;
- E0 excludes WinUI, long-running cross-Scene persistence, Windows AI/NPU, Store work, full observation, and World Resolver.

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

It explicitly states:

- Commit result is staged until `RecordCommit(...)` succeeds.
- Opportunity result is staged until `RecordOpportunity(...)` succeeds.
- Once Commit + `RecordCommit(...)` succeed, the committed no-Opportunity Production/history pair is a valid authority state even before the next Opportunity exists.

Patch 0016 preserves those laws exactly.

### Approved ship plan 0.7

The ship plan requires H1 deterministic-spine closure before provider-backed E0-A experimentation. Provider-neutral attempt/provenance contracts may be added later when they can target a stable deterministic causal-cycle boundary.

---

## 3. Alternatives rejected

### A — provider-attempt/provenance first

Deferred.

Provider attempts eventually need success/refusal/timeout/cancellation/malformed-or-partial output/retry/spend/provider-model identity/diagnostics. Those technical outcomes should drive an already-correct fictional state machine rather than define its adoption order.

### B — full E0 run orchestrator

Rejected as too broad.

It would combine provider execution, retries/cancellation, Candidate parsing, Integrity, State Interpretation, State Authority, Take choice, causal commit, Opportunity advancement, and run-level provenance/budgets.

### C — one-shot Accepted-Take-to-next-Opportunity transition

Rejected as semantically wrong.

It would hide Patch 0015's first authoritative adoption point. A failure during Opportunity establishment could then appear to roll back already-committed fiction.

### D — one mutable cycle-state type plus phase enum

Rejected.

The two phases have mutually exclusive invariants:

```text
Opportunity-bearing:
current Opportunity exists
OpportunityCount = AcceptedPerformanceCount + 1

Postcommit:
current Opportunity is null
SourceOpportunityCount = AcceptedPerformanceCount
```

Two distinct closed types make illegal phase combinations difficult to express and follow the project's authority-in-types law.

### Selected

Two-phase synchronized causal-cycle orchestration with distinct type states.

---

## 4. Dependency direction

Add one Core namespace:

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

`Orchestration` may depend downward on existing deterministic authorities. No existing lower namespace may depend upward on `Orchestration`.

Patch 0016 must add no dependency on:

- Harness;
- provider SDKs/network;
- filesystem/persistence;
- clock/randomness;
- tasks/threads/timers/background work;
- Windows APIs;
- Windows AI;
- GPU/NPU/QNN/ONNX;
- UI;
- package/Store APIs.

---

## 5. Exact public surface

Proposal 0.2 permits exactly five new public types in `Ensemble.E0.Core.Orchestration`:

```csharp
public sealed class E0OpportunityBearingCycleState
public sealed class E0PostCommitCycleState
public sealed class E0OpportunityBearingCycleResult
public static class DeterministicE0CausalCycle
public sealed class E0CausalCycleException : Exception
```

No additional public Orchestration type is allowed in Patch 0016.

### 5.1 `E0OpportunityBearingCycleState`

No public constructor or setter.

Public read-only surface:

```text
ProductionState : ProductionState
```

Internal synchronized components:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
OpportunityHistory         : E0OpportunityHistory
```

The histories stay internal so callers do not carry three independently combinable values as if they were a validated live state.

### 5.2 `E0PostCommitCycleState`

Represents Patch 0015's **first adoption point**.

No public constructor or setter.

Public read-only surface:

```text
ProductionState : ProductionState
Commit          : E0CausalCommit
```

Internal components retained only for phase two:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
SourceContext               : ContextPacket
SourceOpportunityHistory    : E0OpportunityHistory
```

The public Production state is already-authoritative committed fiction and has no current Opportunity.

This token is not a staged transaction/rollback promise.

### 5.3 `E0OpportunityBearingCycleResult`

No public constructor or setter.

Public read-only surface:

```text
OpportunityBearingState : E0OpportunityBearingCycleState
OpportunityEvent        : E0OpportunityTransition
DirectorEvaluation      : LeastInterventionDirectorEvaluation
```

This preserves provenance-relevant Opportunity evidence without exposing the lower `E0OpportunityTransitionResult` raw State/history pair beside the canonical synchronized wrapper.

### 5.4 `DeterministicE0CausalCycle`

Exactly four public static methods, no overloads:

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

The signatures themselves enforce phase legality:

```text
ComposeContext / CommitAcceptedTake
    accept only Opportunity-bearing state

EstablishOpportunity
    accepts only Postcommit state
```

There is no API to compose the next Character Context from a postcommit no-Opportunity state and no API to commit another Take before Opportunity re-establishment.

### 5.5 `E0CausalCycleException`

Public/catchable; no public constructor.

Expected lower-authority failures are normalized at the orchestration stage with fixed safe messages. Unexpected programming/runtime failures are not indiscriminately relabeled.

---

## 6. Opportunity-bearing invariant

Let:

```text
S = ProductionState
H = accepted Performance history entry count
O = OpportunityHistory.CharacterIds count
```

An `E0OpportunityBearingCycleState` is valid only when:

1. `S` is initialized.
2. `S.CurrentOpportunityCharacterId` exists and is initialized.
3. current Opportunity belongs to the current roster.
4. accepted Performance history is exactly synchronized to `S.SceneId`, `S.StateHash`, and current roster using the existing accepted-history invariant authority.
5. Opportunity history Scene equals `S.SceneId`.
6. Opportunity history `LastOpportunityStateHash == S.StateHash`.
7. Opportunity history is initialized and nonempty.
8. every Opportunity-history Character belongs to the roster.
9. its last Character equals `S.CurrentOpportunityCharacterId`.
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

No additional state hash/canonical proof is invented here. Valid wrapper states are created only from exact genesis or from the existing lower deterministic authorities.

---

## 7. `Initialize(...)`

Input: exact genesis `ProductionState`.

Algorithm:

1. validate input through existing lower authorities;
2. call `E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)`;
3. call `E0OpportunityHistory.Initialize(genesisState)`;
4. validate Section 6 synchronization;
5. return the closed opportunity-bearing token.

Patch 0016 deliberately adds no public arbitrary rebinding method such as:

```text
Bind(state, acceptedHistory, opportunityHistory)
```

E0 starts at genesis and valid later tokens are produced by this cycle. Persistence/recovery may later define a separate replay/recovery entry point after E0 evidence requires it.

The duplicate genesis validation performed inside the two existing history initializers is accepted for bounded E0 rather than modifying lower authority solely to optimize this wrapper.

---

## 8. `ComposeContext(...)`

Because synchronized histories are internal, the cycle provides the canonical live Context bridge.

Algorithm:

1. validate the opportunity-bearing token;
2. `ProductionStateCheckpoint.Capture(source.ProductionState)`;
3. call existing:

```text
E0ProductionContextContinuity.ComposeWithAcceptedHistory(
    checkpoint,
    source.AcceptedPerformanceHistory)
```

4. return the existing `E0ProductionContextContinuityResult` unchanged.

This deliberately preserves both:

- `AccessEvaluation` for E0 provenance;
- `ContextEvaluation` for the exact bounded Character Context/trace.

Expected schema behavior remains:

```text
exact empty genesis -> Production-bound Context v2
synchronized nonempty evolved history -> Context v3
```

No Context schema/rendering/canonicalization changes.

---

## 9. `CommitAcceptedTake(...)` — phase one

Input state type: `E0OpportunityBearingCycleState` only.

Algorithm:

1. validate the source token;
2. validate required explicit inputs without echoing content;
3. capture a fresh Production checkpoint;
4. call existing history-aware source proof:

```text
E0TakeStateBinding.BindWithAcceptedHistory(
    checkpoint,
    sourceContext,
    acceptedTake,
    source.AcceptedPerformanceHistory)
```

5. call existing causal authority:

```text
DeterministicCausalCommit.Commit(
    commitId,
    source.ProductionState,
    binding,
    materializations)
```

6. keep that result staged;
7. call existing history advancement/replay:

```text
E0AcceptedPerformanceHistoryContinuity.RecordCommit(
    source.AcceptedPerformanceHistory,
    source.ProductionState,
    commitResult.Commit)
```

8. only after step 7 succeeds, validate the postcommit invariant;
9. return `E0PostCommitCycleState` with:

```text
commitResult.ResultState
advanced accepted history
commitResult.Commit
exact source Context
source Opportunity history
```

### First-adoption law

The returned postcommit token is already authoritative:

- Accepted Take happened;
- approved consequences committed atomically;
- accepted Performance history advanced;
- Production has no current Opportunity yet.

Failure before the token is created authorizes no staged commit-result adoption.

---

## 10. Postcommit invariant

Let:

```text
P  = postcommit ProductionState
C  = committed E0CausalCommit
H  = postcommit accepted Performance history count
OH = retained source Opportunity-history count
X  = retained source Context
```

Requirements:

1. `P.CurrentOpportunityCharacterId` is null.
2. `C.ResultStateHash == P.StateHash`.
3. `C.ParentStateHash` is initialized.
4. `C.Take` is Accepted and structurally initialized under existing authority.
5. accepted Performance history is synchronized to `P.SceneId`, `P.StateHash`, and current roster.
6. accepted history is nonempty.
7. its final semantic item equals `C.Take.Performance` subject plus exact `VisibleText`.
8. retained source Opportunity-history Scene equals `P.SceneId`.
9. retained source Opportunity-history `LastOpportunityStateHash == C.ParentStateHash`.
10. retained source Opportunity history is initialized/nonempty and all Characters remain in roster.
11. retained source Opportunity history ends at `C.Take.Performance.SubjectCharacterId`.
12. exact count coupling holds:

```text
OH == H
```

13. retained source Context Scene equals `P.SceneId`.
14. retained source Context ID equals `C.Take.Performance.ContextPacketId`.
15. retained source Context subject equals `C.Take.Performance.SubjectCharacterId`.

Patch 0016 does **not** add an executable rule that parent/result hashes must differ. Existing canonical authorities own hash semantics; the cycle only proves the required equality/binding relationships.

---

## 11. `EstablishOpportunity(...)` — phase two

Input state type: `E0PostCommitCycleState` only.

Algorithm:

1. validate the complete postcommit invariant;
2. call existing Opportunity authority:

```text
DeterministicOpportunityAuthority.Establish(
    source.ProductionState,
    source.Commit,
    source.SourceContext,
    source.SourceOpportunityHistory)
```

3. keep that result staged;
4. call existing history/Opportunity coupling replay:

```text
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
    source.AcceptedPerformanceHistory,
    source.ProductionState,
    source.Commit,
    source.SourceOpportunityHistory,
    opportunity.Event)
```

5. only after step 4 succeeds, combine:

```text
opportunity.State
returned accepted-history advancement
opportunity.History
```

6. validate Section 6 synchronization;
7. return `E0OpportunityBearingCycleResult` with:

```text
OpportunityBearingState = canonical synchronized wrapper
OpportunityEvent        = exact lower event
DirectorEvaluation      = exact lower evaluation/trace
```

### Second-phase failure law

If phase two fails, the caller still retains the already-authoritative postcommit token returned by phase one.

Patch 0016 does not yet define persistence, retry, repair, or crash recovery for that token. It only prevents Opportunity failure from being modeled as rollback of committed fiction.

---

## 12. Non-Accepted Takes cannot enter the causal cycle

`CommitAcceptedTake(...)` accepts existing `E0Take` authority but requires:

```text
Disposition == Accepted
```

Existing `BindWithAcceptedHistory(...)` and causal commit authority re-prove this.

`Rejected` and `Alternate` Takes cannot commit through Patch 0016.

What a future runner does after reject/alternate/request-another-take belongs to the later technical attempt/run-orchestration boundary.

---

## 13. Technical attempts remain outside Patch 0016

No Patch 0016 type or method defines:

- `RunId` / `AttemptId`;
- provider/model identity;
- provider request/result;
- partial streaming buffer;
- refusal/timeout/error status;
- retry count;
- token/cost/spend record;
- understudy choice;
- cancellation-token ownership.

Future attempt orchestration will use:

```text
OpportunityBearingCycleState
 -> ComposeContext
 -> technical provider attempt(s)
 -> Candidate / Integrity / Interpreter / State Authority / Take
 -> if Accepted only:
      CommitAcceptedTake
```

Thus provider failure has no causal-cycle path unless a later layer explicitly and incorrectly calls the accepted-commit boundary.

---

## 14. No new canonical identity or persistent authority

Patch 0016 adds no:

- persistent event type;
- cycle event;
- cycle hash;
- StateHash algorithm;
- ContextPacketId algorithm;
- CommitId/TakeId derivation;
- Opportunity canonicalization;
- history hash;
- orchestration field in Production.

The wrapper types are in-memory synchronization/capability tokens only.

Given identical lower inputs, all Patch 0015 reference identities remain exact, including:

```text
live postcommit StateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c

live Opportunity StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

first MARLOWE Context v3 hash
ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
```

---

## 15. Failure domains and privacy

Expected stage failures normalize to `E0CausalCycleException` with fixed top-level messages:

```text
Initialize:
E0 causal cycle initialization failed.

ComposeContext:
E0 causal cycle Context composition failed.

CommitAcceptedTake:
E0 causal cycle accepted Take commit failed.

EstablishOpportunity:
E0 causal cycle Opportunity establishment failed.
```

Expected lower exception families are caught only where that stage can produce them, including narrowly:

```text
E0AcceptedPerformanceHistoryException
E0OpportunityTransitionException
E0ContextContinuityException
E0CausalCommitException
```

The implementation audit must verify every retained `InnerException` path before approval. It may preserve the lower deterministic exception as `InnerException` only when that lower message is fixed/structural and does not contain forbidden payload. Otherwise the stage exception omits it.

New top-level failure text must never contain:

- `VisibleText`;
- Character-private Context prose;
- mutation prose;
- provider output;
- imported/user content;
- credentials/secrets.

Unexpected programming/runtime failures are not blanket-wrapped.

---

## 16. Determinism, complexity, and memory

No new canonical bytes or semantic collections are created.

### Complexity

Each operation is bounded by existing lower authority:

```text
Initialize
  O(existing genesis validation)

ComposeContext
  O(existing Access + Context + accepted-history projection)

CommitAcceptedTake
  O(history-aware binding + causal commit + causal replay/history validation)

EstablishOpportunity
  O(Director proposal + Opportunity canonicalization/replay + history coupling)
```

No polling/retry/timer/parallel/background loop is added.

### Memory

Opportunity-bearing state retains references to:

```text
ProductionState
accepted Performance history
Opportunity history
```

Postcommit state temporarily retains references to:

```text
postcommit ProductionState
accepted Performance history
commit event
source Context
source Opportunity history
```

No Production/event payload is copied merely for the wrapper.

Retaining the exact source Context through phase two is necessary because existing `DeterministicOpportunityAuthority.Establish(...)` intentionally binds Director recomputation to that accepted Candidate's Context identity/roster. It is kept internal and normally released when the caller replaces the postcommit token with the next opportunity-bearing token.

---

## 17. ARM64/battery suitability

Patch 0016 is synchronous deterministic CPU/memory orchestration only.

It adds no:

- x86 dependency/emulation;
- network/provider SDK;
- filesystem;
- background/idle work;
- clock/randomness;
- Task/thread/timer;
- GPU/NPU wake;
- Windows API.

This preserves the current ARM64/low-idle architecture by construction. No measured performance/battery claim is made.

---

## 18. Expected implementation surface

Preferred source additions only:

```text
src/Ensemble.E0.Core/Orchestration/
    E0CausalCycleModels.cs
    DeterministicE0CausalCycle.cs
```

An internal invariant helper may live in one of those files. Do not create another project/assembly.

Preferred test additions only:

```text
tests/Ensemble.E0.Core.Tests/Orchestration/
    E0CausalCycleContractAuditTests.cs
    E0CausalCycleTests.cs
    E0CausalCycleDeterminismTests.cs
```

**Default implementation expectation:** no existing Patch 0015 source or test file changes.

Patch 0015 tests remain lower-layer authority and should not be rewritten merely to consume the new wrapper. New Patch 0016 tests independently reproduce/cross-check the frozen Patch 0015 oracle through the orchestration API.

If implementation discovers a concrete inherited reflection/public-surface guard that necessarily rejects the new Orchestration namespace, adapt only that exact guard and record the reason. No such required inherited adaptation has been identified during Proposal 0.2 audit so far.

Any semantic change required in existing Context/CausalCommit/Opportunity/Continuity/Take authority reopens architecture rather than being silently patched.

Harness remains unchanged in Patch 0016.

---

## 19. Required test matrix

### Public/type-state surface

Prove exactly five public Orchestration types and exact signatures.

Prove state/result classes:

- no public constructors;
- no public setters;
- no extra declared public methods.

Prove exception has no public constructor.

Prove no public Orchestration signature contains provider/network/filesystem/Windows/clock/task/thread/timer/GPU/NPU/QNN/ONNX/persistence/repository types.

Compile-time signatures themselves prove:

- `ComposeContext`/`CommitAcceptedTake` cannot accept postcommit state;
- `EstablishOpportunity` cannot accept opportunity-bearing state.

### Initialize

- Missing Raft exact genesis initializes;
- null/non-genesis/invalid genesis fails closed;
- initial current Opportunity is exact;
- first `ComposeContext` preserves exact v2 identity.

### Wrapper synchronization adversarial tests

Using reflection only for test-only construction of otherwise impossible invalid wrappers where needed:

- stale accepted history rejected;
- stale Opportunity history rejected;
- Scene mismatch rejected;
- wrong last Opportunity Character rejected;
- current Opportunity mismatch rejected;
- count mismatch rejected;
- roster mismatch rejected.

### Context bridge

- genesis -> exact existing v2 oracle;
- evolved opportunity-bearing state -> exact existing v3 oracle;
- repeated composition on identical state is byte/identity equivalent;
- AccessEvaluation remains available;
- no new schema/version/hash.

### Commit phase

- exact Patch 0015 first live Accepted Take -> exact existing postcommit StateHash;
- returned postcommit state has no current Opportunity;
- accepted history advances exactly once internally;
- source Opportunity history does not advance in phase one;
- Rejected/Alternate Takes fail;
- stale/tampered Context fails through exact history-aware binding;
- stale Production cannot commit;
- duplicate CommitId/TakeId remains inherited;
- record-materialization rules remain inherited;
- zero-mutation and all-durable-consequence-Rejected Accepted Takes retain historical Performance semantics;
- phase one never silently establishes the next Opportunity.

### Opportunity phase

- exact first live oracle selects MARLOWE;
- exact existing Opportunity StateHash preserved;
- accepted history does not append in phase two;
- Opportunity history advances exactly once;
- final `O == H + 1`;
- next `ComposeContext` is exact existing MARLOWE v3 oracle;
- multi-turn reference route remains `VOSS -> MARLOWE -> WREN -> VOSS`;
- repeated identical Performances, Character recurrence, and self-history remain unchanged.

### Two-adoption proof

Dedicated regression must prove:

1. `CommitAcceptedTake(...)` returns an externally retainable `E0PostCommitCycleState` before any Opportunity call.
2. Its Production state is the exact committed postcommit StateHash and has no Opportunity.
3. `EstablishOpportunity(...)` consumes that different type later.
4. A deliberately invalid phase-two wrapper/failure does not mutate or replace the already-returned postcommit Production object.
5. No API returns a staged commit result as a live cycle state before accepted-history advancement succeeds.
6. No API returns a staged Opportunity result as a live cycle state before `RecordOpportunity(...)` succeeds.

### Dependency/hygiene

- exactly the new Orchestration source depends on the new namespace;
- lower source remains unchanged by default;
- no provider/persistence/platform dependency;
- no new canonical serializer/event/hash;
- no Harness change.

---

## 20. Historical lower APIs remain exact

Patch 0016 composes but does not replace/delete:

```text
E0ProductionContextContinuity.Compose
E0ProductionContextContinuity.ComposeWithAcceptedHistory
E0TakeStateBinding.Bind
E0TakeStateBinding.BindWithAcceptedHistory
DeterministicCausalCommit.Commit / Replay
E0AcceptedPerformanceHistoryContinuity.Initialize
E0AcceptedPerformanceHistoryContinuity.RecordCommit
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
DeterministicOpportunityAuthority.Establish / Replay
E0OpportunityHistory.Initialize
```

They remain necessary lower authority/testing contracts frozen by prior patches.

No external compatibility obligation is created for the new Patch 0016 wrapper before shipping; if later evidence proves its representation wrong, the canonical pre-release design may still be corrected.

---

## 21. Explicit non-scope

Patch 0016 does not implement/decide:

- provider/model invocation;
- request/attempt/retry/spend/streaming provenance;
- cancellation-token ownership;
- refusal/timeout/backoff;
- understudy selection;
- Candidate generation;
- model-assisted Integrity/Interpreter;
- Take review UX;
- rejection/alternate retry orchestration;
- full Scene/run loop;
- run termination;
- run-level call/cost budgets;
- immutable E0 run bundle;
- transcript/blind-review package;
- persistence/recovery;
- cross-Scene history;
- observation/CharacterClaim disclosure;
- World Resolver;
- branches/retcon/rehearsal;
- WinUI;
- Windows AI/Aion/Phi/LoRA;
- Windows ML/QNN/NPU;
- App Actions/MCP;
- MSIX/IPackageValidator/WACK/Store.

---

## 22. What Patch 0016 unlocks

After Patch 0016 is implemented/natively validated, a later H1/E0-A boundary can safely define technical Performer attempts against this deterministic target:

```text
OpportunityBearingCycleState
 -> ComposeContext
 -> technical provider attempt(s)
 -> Candidate / Integrity / Interpreter / State Authority / Take
 -> if Accepted:
      CommitAcceptedTake
      -> authoritative E0PostCommitCycleState
      -> EstablishOpportunity
      -> next OpportunityBearingCycleState
 -> if not Accepted:
      no causal-cycle commit
```

This makes the separation between technical execution and fictional authority explicit in the call graph.

---

## 23. Proposal 0.2 corrections from the first audit pass

Relative to Proposal 0.1:

1. made distinct type-state signatures an explicit architectural law rather than only a modeling consequence;
2. renamed final result property from generic `State` to `OpportunityBearingState`;
3. removed any implied executable requirement that parent/result StateHashes must differ; existing canonical authorities remain sole hash authority;
4. strengthened privacy review for retained `InnerException` paths;
5. clarified why exact source Context retention through phase two is necessary and internal;
6. changed implementation expectation from optional Patch 0015 helper refactoring to **new-source/new-test additions only by default**;
7. made semantic changes to lower Patch 0015 authority an architecture-reopen condition rather than an implementation convenience.

The recursive audit restarts from correctness at Proposal 0.2.

---

## 24. Recursive audit closure criterion

Implementation remains forbidden until one complete fresh pass finds:

```text
0 material correctness corrections
0 authority corrections
0 Patch-0015 two-adoption corrections
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

Any material/worthwhile correction restarts the audit from the relevant authority layer and increments the Proposal version.

Only after a clean pass may blueprint-audit evidence be created and explicit Director approval requested.
