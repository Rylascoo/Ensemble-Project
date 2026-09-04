# H1 Patch 0016 — Synchronized Causal Cycle

Status: **Blueprint Proposal 0.3 — RECURSIVE ADVERSARIAL AUDIT RESTARTED; IMPLEMENTATION FORBIDDEN**

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

Patch 0015 already proves every lower authority required for one accepted causal cycle, but the live sequence is still manually composed in test support:

```text
opportunity-bearing Production/history state
 -> exact Character Context
 -> externally obtain/bind Accepted Take
 -> history-aware precommit proof
 -> causal commit
 -> accepted-Performance history advancement
 -> first adoption point: committed postcommit Production/history
 -> deterministic Opportunity establishment
 -> accepted-history/Opportunity-history coupling
 -> second adoption point: next opportunity-bearing synchronized state
```

Patch 0016 makes those adoption boundaries canonical without provider execution, retry policy, persistence, or a full E0 runner.

The state machine is:

```text
E0OpportunityBearingCycleState
   -- CommitAcceptedTake -->
E0PostCommitCycleState
   -- EstablishOpportunity -->
E0OpportunityBearingCycleState
```

It is a **pure immutable value-state machine**. Core owns no hidden mutable current-cycle singleton. Each successful method returns a validated successor value; the caller adopts that successor by replacing its own current token.

This distinction matters:

- a returned postcommit token has completed every proof required for Patch 0015's first adoption boundary and is safe to adopt;
- it is not automatically installed into some global Core state;
- if phase two later fails, the caller may retain the already-validated/adopted postcommit token rather than semantically rolling back the accepted causal commit.

No new fictional semantics or canonical identity are introduced.

---

## 2. Authority basis

### Frozen Blueprint 0.1

Blueprint 0.1 requires:

- a generated Performance enters Production history only as an Accepted Take;
- provider failure/refusal/timeout/retry cannot become fictional action;
- cancelled/rejected/failed partial output is diagnostic only;
- Accepted Performance + approved consequences commit atomically;
- deterministic authority owns canonical state mutation;
- Director owns attention/opportunity rather than world truth;
- E0 uses a minimal developer harness with explicit provenance;
- E0 excludes WinUI, long-running cross-Scene persistence, Windows AI/NPU, Store work, full observation, and World Resolver.

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

Patch 0015 explicitly freezes:

```text
Commit result staged
 -> RecordCommit succeeds
 -> postcommit Production/history may be adopted

Opportunity result staged
 -> RecordOpportunity succeeds
 -> opportunity-bearing Production/history/OpportunityHistory may be adopted
```

Patch 0016 preserves that two-adoption law exactly.

### Approved ship plan 0.7

The approved program map requires deterministic-spine closure before provider-backed E0-A. Technical attempts/provenance should target a stable deterministic cycle rather than define its causal adoption order.

---

## 3. Alternatives rejected

### Provider-attempt/provenance first

Deferred. Technical success/refusal/timeout/cancellation/partial/retry/spend semantics should drive a correct deterministic fictional state machine later.

### Full E0 runner

Rejected as too broad. It would combine provider execution, retries, Candidate parsing, Integrity, Interpreter, State Authority, Take selection, causal transition, Opportunity transition, and run budgets/provenance.

### One-shot Accepted-Take-to-next-Opportunity call

Rejected as semantically wrong because it hides the first Patch 0015 adoption boundary.

### One mutable state + phase enum

Rejected because the phases have mutually exclusive invariants and invalid combinations would be conventionally expressible.

### Hidden mutable cycle coordinator

Rejected. It would make retries/testing/branch-like derivations depend on hidden mutation and would blur the project's explicit-input deterministic style.

### Selected

Two closed immutable type states + pure deterministic transition functions.

---

## 4. Dependency direction

Add only:

```text
Ensemble.E0.Core.Orchestration
```

Direction:

```text
existing Core authorities
        ^
        |
  Orchestration
```

`Orchestration` may depend downward on Production, Context, Continuity, Take, CausalCommit, Opportunity, Director, and Domain types already required by those contracts.

No lower namespace may depend upward on Orchestration.

No Patch 0016 dependency on Harness, provider/network, filesystem/persistence, clocks/randomness, Tasks/threads/timers, Windows, Windows AI, GPU/NPU/QNN/ONNX, UI, package, or Store APIs.

---

## 5. Exact public surface

Exactly five new public types:

```csharp
public sealed class E0OpportunityBearingCycleState
public sealed class E0PostCommitCycleState
public sealed class E0OpportunityBearingCycleResult
public static class DeterministicE0CausalCycle
public sealed class E0CausalCycleException : Exception
```

### `E0OpportunityBearingCycleState`

Public read-only:

```text
ProductionState : ProductionState
```

Internal synchronized components:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
OpportunityHistory         : E0OpportunityHistory
```

No public constructor/setter/declared instance method.

Construction is closed behind one internal validated factory owned by the type/cycle implementation. There is no raw internal construction path that bypasses the wrapper's cross-history invariant.

### `E0PostCommitCycleState`

Public read-only:

```text
ProductionState : ProductionState
Commit          : E0CausalCommit
```

Internal:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
SourceContext               : ContextPacket
SourceOpportunityHistory    : E0OpportunityHistory
```

No public constructor/setter/declared instance method.

It is the validated successor value at the first adoption boundary, not a staged transaction promise.

### `E0OpportunityBearingCycleResult`

Public read-only:

```text
OpportunityBearingState : E0OpportunityBearingCycleState
OpportunityEvent        : E0OpportunityTransition
DirectorEvaluation      : LeastInterventionDirectorEvaluation
```

No public constructor/setter/declared instance method.

It exposes provenance evidence without exposing the lower raw State/history result beside the synchronized wrapper.

### `DeterministicE0CausalCycle`

Exactly four public static methods; no overloads:

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

Signature-level phase law:

```text
ComposeContext + CommitAcceptedTake
    only OpportunityBearing

EstablishOpportunity
    only PostCommit
```

### `E0CausalCycleException`

Public/catchable, no public constructor.

It represents wrapper-owned synchronization/stage failure and normalizes expected lower boundary failures with safe stage messages.

---

## 6. Construction-time invariant ownership

Patch 0016 does **not** continuously rescan every wrapper invariant before every method merely to defend against unsupported reflection mutation.

Instead:

1. wrapper construction is closed;
2. wrapper components are immutable existing objects;
3. each wrapper validates the aggregate invariant once before construction succeeds;
4. subsequent lower calls re-prove the lower inputs they own.

This avoids adding redundant O(history) wrapper scans around lower operations that already validate accepted history/Context/Opportunity chain data.

The wrapper's own invariant code must reuse existing neutral/lower owners rather than copy them:

```text
AcceptedPerformanceHistoryInvariants.ValidateAndProject(...)
    owns accepted-history state/roster/text validation

OpportunityInvariants.ValidateProductionRoster(...)
OpportunityInvariants.RequireInitialized(...)
    own current E0 Production roster/strong-ID validation
```

Patch 0016 adds only the cross-component facts no lower single authority owns:

- Opportunity-history state-hash/current-Character coupling to the wrapper Production state;
- accepted-history-count vs Opportunity-history-count phase induction;
- postcommit retained-source coupling needed to safely enter phase two.

No new lower invariant helper is created merely to duplicate existing logic.

---

## 7. Opportunity-bearing state invariant

Let:

```text
S = ProductionState
H = accepted Performance history count
O = OpportunityHistory.CharacterIds count
```

Validated at wrapper creation:

1. Production roster is the existing canonical E0 roster.
2. `S.StateHash`/`S.SceneId`/current Opportunity are initialized through existing strong-ID helpers.
3. current Opportunity exists and belongs to roster.
4. existing accepted-history invariant validates exact synchronization to `S.SceneId`, `S.StateHash`, and roster and yields H entries.
5. Opportunity history Scene equals `S.SceneId`.
6. Opportunity history `LastOpportunityStateHash == S.StateHash`.
7. Opportunity history is initialized/nonempty.
8. every Opportunity-history Character is initialized and belongs to roster.
9. final Opportunity-history Character equals current Opportunity.
10. exact phase count:

```text
O == H + 1
```

Genesis:

```text
H=0, O=1
```

Every completed cycle:

```text
H=n, O=n+1
```

No extra StateHash recomputation is added by the wrapper. It trusts only states produced by exact genesis/lower deterministic authorities and proves the cross-object bindings above.

---

## 8. `Initialize(...)`

Algorithm:

1. require genesis Production state;
2. call `E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)`;
3. call `E0OpportunityHistory.Initialize(genesisState)`;
4. create `E0OpportunityBearingCycleState`, whose validated factory proves Section 7;
5. return it.

No arbitrary public `Bind(state, histories...)` exists.

The two lower history initializers both independently validate genesis today. Bounded duplicate genesis validation is accepted for E0; Patch 0016 does not modify lower authority just to remove it.

---

## 9. `ComposeContext(...)`

Input type itself proves the cycle is in the Opportunity-bearing phase.

Algorithm:

1. reject null wrapper;
2. capture checkpoint from `source.ProductionState`;
3. call:

```text
E0ProductionContextContinuity.ComposeWithAcceptedHistory(
    checkpoint,
    source.AcceptedPerformanceHistory)
```

4. return exact existing `E0ProductionContextContinuityResult`.

The lower Continuity authority freshly validates accepted-history/state synchronization and Access/Context derivation.

It preserves:

```text
AccessEvaluation
ContextEvaluation
```

for future E0 provenance.

No Context version/bytes/hash changes:

```text
empty exact genesis -> v2
nonempty evolved accepted history -> v3
```

---

## 10. `CommitAcceptedTake(...)` — phase one

Algorithm:

1. reject null wrapper/required inputs with safe fixed stage behavior;
2. capture fresh checkpoint from `source.ProductionState`;
3. call:

```text
E0TakeStateBinding.BindWithAcceptedHistory(
    checkpoint,
    sourceContext,
    acceptedTake,
    source.AcceptedPerformanceHistory)
```

4. call:

```text
DeterministicCausalCommit.Commit(
    commitId,
    source.ProductionState,
    binding,
    materializations)
```

5. keep lower commit result local/staged;
6. call:

```text
E0AcceptedPerformanceHistoryContinuity.RecordCommit(
    source.AcceptedPerformanceHistory,
    source.ProductionState,
    commitResult.Commit)
```

7. create `E0PostCommitCycleState` from:

```text
commitResult.ResultState
advanced accepted history
commitResult.Commit
sourceContext
source.OpportunityHistory
```

8. postcommit factory proves Section 11;
9. only then return the validated successor.

### First adoption boundary

Before step 9, no Patch 0016 successor is available to adopt.

After success, the returned token has completed all existing commit + `RecordCommit` proof required for the first Patch 0015 adoption boundary. The caller may replace its current Opportunity-bearing token with this postcommit token.

The method itself mutates no hidden global current state.

---

## 11. Postcommit state invariant

Let:

```text
P  = postcommit ProductionState
C  = E0CausalCommit
H  = accepted history count after commit
OH = retained source Opportunity-history count
X  = retained source Context
```

Validated once at postcommit wrapper creation:

1. existing Production roster validation succeeds.
2. `P.StateHash`/`P.SceneId` and relevant commit identities are initialized through existing helpers/lower event validation.
3. `P.CurrentOpportunityCharacterId` is null.
4. `C.ResultStateHash == P.StateHash`.
5. `C.Take` is Accepted under existing causal authority.
6. existing accepted-history invariant proves synchronization to `P.SceneId`, `P.StateHash`, roster and yields H entries.
7. H is nonzero.
8. final accepted-history semantic item equals `C.Take.Performance.SubjectCharacterId` + exact `VisibleText`.
9. source Opportunity history Scene equals `P.SceneId`.
10. source Opportunity history `LastOpportunityStateHash == C.ParentStateHash`.
11. source Opportunity history is nonempty; every Character is initialized/in roster.
12. its final Character equals the committed Performance subject.
13. exact phase count:

```text
OH == H
```

14. source Context Scene equals `P.SceneId`.
15. source Context ID equals committed Performance `ContextPacketId`.
16. source Context subject equals committed Performance subject.

No rule is added that parent/result hashes must differ. Existing canonical authorities remain sole hash authority.

The exact source Context is retained internally because existing Opportunity authority intentionally recomputes Director input from that accepted Candidate's source Context identity/roster.

---

## 12. `EstablishOpportunity(...)` — phase two

Algorithm:

1. reject null postcommit wrapper;
2. call existing:

```text
DeterministicOpportunityAuthority.Establish(
    source.ProductionState,
    source.Commit,
    source.SourceContext,
    source.SourceOpportunityHistory)
```

3. keep result local/staged;
4. call existing:

```text
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
    source.AcceptedPerformanceHistory,
    source.ProductionState,
    source.Commit,
    source.SourceOpportunityHistory,
    opportunity.Event)
```

5. create `E0OpportunityBearingCycleState` from:

```text
opportunity.State
returned accepted-history advancement
opportunity.History
```

6. its validated factory proves Section 7;
7. construct result with:

```text
OpportunityBearingState = validated wrapper
OpportunityEvent        = opportunity.Event
DirectorEvaluation      = opportunity.DirectorEvaluation
```

8. return result.

### Second adoption boundary

Before step 8, no next Opportunity-bearing Patch 0016 successor is available to adopt.

After success, the caller may replace the postcommit token with `result.OpportunityBearingState`.

If phase two fails, the previously returned postcommit token remains a valid first-boundary successor; Patch 0016 does not model the failure as rollback of committed fiction.

The transition is pure/deterministic. Repeating `EstablishOpportunity` on the same immutable postcommit token with unchanged code/data is expected to derive the same canonical lower identities rather than mutate the token.

---

## 13. Non-Accepted Takes have no cycle commit path

`CommitAcceptedTake(...)` accepts the existing `E0Take` authority package, but `BindWithAcceptedHistory`/causal authority require:

```text
Disposition == Accepted
```

Rejected/Alternate Takes cannot cross phase one.

Future request-another-take/reject/alternate/provider retry behavior remains outside Patch 0016.

---

## 14. Pure derivation does not freeze branch semantics

Because transitions are pure, callers can technically invoke a transition more than once from the same source value, just as current lower deterministic functions can be called more than once.

Patch 0016 does not create a branch store, canon-selection system, or multiple-live-head authority.

For E0, the runner owns exactly one current cycle token and explicitly chooses/adopts one successful successor at each boundary. Alternative pure calculations that are never adopted are not automatically installed as the live E0 state.

Future branch/rehearsal/canon policy remains post-E0 scope.

---

## 15. Technical attempts remain outside Patch 0016

No Patch 0016 `RunId`, `AttemptId`, provider/model identity, request/result DTO, partial stream, refusal/timeout enum, retry counter, cost/spend record, understudy decision, or cancellation-token policy.

Future flow:

```text
OpportunityBearingCycleState
 -> ComposeContext
 -> technical attempt(s)
 -> Candidate / Integrity / Interpreter / State Authority / Take
 -> Accepted only
 -> CommitAcceptedTake
```

Technical failure does not call the accepted-commit boundary and therefore has no fictional transition path in this layer.

---

## 16. No new canonical/persistent identity

No new persistent event, cycle event/hash, StateHash algorithm, ContextPacketId algorithm, CommitId/TakeId derivation, Opportunity canonicalizer, history hash, or Production orchestration field.

Cycle wrappers are in-memory synchronization/capability values only.

Frozen Patch 0015 identities must remain exact, including:

```text
postcommit StateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c

Opportunity StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

MARLOWE v3 structured Context hash
ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
```

---

## 17. Failure/privacy boundary

`E0CausalCycleException` top-level messages are fixed by stage:

```text
E0 causal cycle initialization failed.
E0 causal cycle Context composition failed.
E0 causal cycle accepted Take commit failed.
E0 causal cycle Opportunity establishment failed.
```

Wrapper-construction invariant failures use the corresponding stage message; implementation may use private/internal detail only as safe InnerException evidence.

Expected lower exceptions are caught narrowly only where reachable, e.g.:

```text
E0AcceptedPerformanceHistoryException
E0OpportunityTransitionException
E0ContextContinuityException
E0CausalCommitException
```

Implementation audit must inspect every retained InnerException path. New public error text never contains VisibleText, Character-private Context prose, mutation prose, provider/user/imported payload, credentials, or secrets.

Unexpected programming/runtime failures are not blanket-wrapped.

---

## 18. Complexity/memory

### Wrapper validation

Validation occurs once per newly created cycle state:

```text
Opportunity-bearing factory: O(H + O)
Postcommit factory:         O(H + O)
```

where H/O are bounded E0 history lengths.

Existing lower methods still perform their own required proofs. Patch 0016 does not add an extra full wrapper rescan before every `ComposeContext`, `CommitAcceptedTake`, or `EstablishOpportunity` call.

Cumulative long-Scene behavior may remain superlinear because existing Patch 0015 full-history validation/composition is intentionally unoptimized in E0. Patch 0016 does not invent a quota/window/index merely to optimize the experiment.

### Memory

Wrappers retain references to immutable existing objects; no Production/event payload duplication.

Postcommit state temporarily retains source Context + source Opportunity history solely for exact phase-two authority. These are internal and can become unreachable after the caller adopts the next Opportunity-bearing state.

No background work or cache is added.

---

## 19. ARM64/battery suitability

Synchronous deterministic CPU/memory only.

No x86/emulation, provider/network, filesystem, idle/background work, clock/randomness, Tasks/threads/timers, Windows APIs, or GPU/NPU wake.

No measured performance/battery claim.

---

## 20. Expected implementation surface

Preferred source additions only:

```text
src/Ensemble.E0.Core/Orchestration/
    E0CausalCycleModels.cs
    DeterministicE0CausalCycle.cs
```

Preferred test additions only:

```text
tests/Ensemble.E0.Core.Tests/Orchestration/
    E0CausalCycleContractAuditTests.cs
    E0CausalCycleTests.cs
    E0CausalCycleDeterminismTests.cs
```

Default expectation: **zero existing Patch 0015 source/test edits and zero Harness edits**.

New tests cross-check frozen lower oracles independently through the new public orchestration surface. Patch 0015 tests remain lower-authority regression evidence.

If a concrete inherited reflection/public-surface guard necessarily fails because the new namespace exists, adapt only that exact guard and record why. None is currently identified.

Any semantic modification required in Context/CausalCommit/Opportunity/Continuity/Take reopens architecture.

---

## 21. Required test matrix

### Public/type-state closure

- exactly five exported Orchestration types;
- exact properties/method signatures;
- no public constructors/setters/extra instance methods on state/result types;
- no public exception constructor;
- no provider/network/filesystem/Windows/clock/task/thread/timer/GPU/NPU/QNN/ONNX/persistence signature;
- compile-time method parameter types enforce phase legality.

### Validated factories/invariants

Internal validated factories may be invoked via reflection in tests solely to prove they reject mismatched existing immutable components:

- stale accepted history;
- stale Opportunity history;
- Scene mismatch;
- current/last Opportunity mismatch;
- history-count mismatch;
- roster mismatch;
- postcommit source-context/commit mismatch.

No public arbitrary rebind API is added for these tests.

### Initialize / Context

- Missing Raft genesis initializes;
- invalid/non-genesis fails;
- genesis Context exact v2 oracle;
- evolved Context exact v3 oracle;
- repeated Context composition preserves bytes/identities;
- AccessEvaluation remains returned.

### Commit phase

- first live Accepted Take -> exact frozen postcommit StateHash;
- postcommit has no Opportunity;
- accepted history advances once;
- retained source Opportunity history does not advance;
- Rejected/Alternate fail;
- stale/tampered Context fails through history-aware binding;
- stale state/duplicate IDs/materialization rules stay inherited;
- zero-mutation/all-durable-consequence-Rejected Accepted Take preserves historical Performance;
- phase one never establishes Opportunity.

### Opportunity phase

- first live route selects MARLOWE;
- exact frozen Opportunity StateHash;
- accepted history no append in phase two;
- Opportunity history advances once;
- final `O=H+1`;
- next Context exact MARLOWE v3 oracle;
- multi-turn `VOSS -> MARLOWE -> WREN -> VOSS` unchanged;
- repeated identical Performance/Character recurrence/self-history unchanged.

### Two-adoption / purity

- phase-one successor exists before phase-two invocation;
- phase-one successor is exact postcommit/no-Opportunity state;
- phase-two consumes only postcommit type;
- phase-two failure/tampered test input cannot mutate the already-created phase-one Production object;
- no staged lower commit result is exposed as a cycle successor before RecordCommit;
- no staged Opportunity result is exposed before RecordOpportunity;
- repeated call on identical immutable state/input yields equivalent canonical successor identities;
- source wrapper remains unchanged after every call.

### Dependency/hygiene

- only new Orchestration source introduces Orchestration namespace;
- lower source unchanged by default;
- no new serializer/event/hash/platform/provider/persistence dependency;
- Harness unchanged.

---

## 22. Historical lower APIs remain exact

Patch 0016 composes but does not replace/delete prior public lower APIs, including Context continuity, history-aware binding, causal Commit/Replay, accepted-history initialization/advancement, Opportunity Establish/Replay, and Opportunity-history initialization.

These remain exact prior-patch authority/testing contracts.

No external compatibility obligation is created for Patch 0016 before shipping; later evidence may still justify correcting this new wrapper representation.

---

## 23. Explicit non-scope

No provider/model invocation; request/attempt/retry/spend/streaming provenance; cancellation ownership; refusal/timeout/backoff; understudy selection; Candidate generation; model-assisted Integrity/Interpreter; Take review UX; rejection/alternate retry orchestration; full Scene/run loop; run termination/budgets; E0 run bundle/transcript package; persistence/recovery; cross-Scene history; observation/CharacterClaim disclosure; World Resolver; branch/canon/retcon/rehearsal; WinUI; Windows AI/Aion/Phi/LoRA; Windows ML/QNN/NPU; App Actions/MCP; MSIX/IPackageValidator/WACK/Store.

---

## 24. What Patch 0016 unlocks

```text
OpportunityBearingCycleState
 -> ComposeContext
 -> future technical attempt orchestration
 -> Candidate / Integrity / Interpreter / State Authority / Take
 -> Accepted only:
      CommitAcceptedTake
      -> validated PostCommitCycleState
      -> EstablishOpportunity
      -> validated next OpportunityBearingCycleState
```

Technical execution and fictional authority now meet at one explicit accepted-commit boundary.

---

## 25. Proposal correction history

### 0.1 -> 0.2

- explicit type-state phase law;
- clearer final result property name;
- removed invented parent/result-hash inequality rule;
- tightened InnerException/privacy audit;
- removed default Patch 0015 test refactor.

### 0.2 -> 0.3

- clarified pure value semantics vs hidden global adoption;
- added explicit caller adoption law so repeated pure derivation does not silently create multiple live heads;
- moved wrapper aggregate validation to closed construction rather than rescanning before every method;
- required reuse of existing accepted-history and Opportunity invariant owners;
- removed unsupported reflection-forgery defensive requirements from ordinary runtime paths while retaining targeted factory tests;
- made added complexity/memory cost explicit.

Audit restarts from correctness at Proposal 0.3.

---

## 26. Recursive audit closure criterion

Implementation forbidden until one complete fresh pass finds:

```text
0 material correctness corrections
0 authority/adoption corrections
0 dependency-direction corrections
0 synchronization/type-state corrections
0 failure/privacy corrections
0 canonical/hash/version corrections
0 E0-scope corrections
0 worthwhile public-surface simplifications
0 worthwhile test improvements
0 ARM64/battery/hygiene corrections
0 ship-plan inconsistencies
0 evidence corrections
```

Any material/worthwhile correction increments Proposal version and restarts from the affected authority layer.

Only a clean pass permits blueprint-audit evidence and explicit Director approval request.
