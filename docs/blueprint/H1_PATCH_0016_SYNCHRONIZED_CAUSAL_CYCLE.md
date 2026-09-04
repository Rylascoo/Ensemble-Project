# H1 Patch 0016 — Synchronized Causal Cycle

Status: **Blueprint Proposal 0.4 — RECURSIVE ADVERSARIAL AUDIT RESTARTED; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Authoritative parent `main`: `5186b0ab624165ab9872630592b164bf3764273d`

Program authority: `Kymaean Architecture & Ship Plan — Proposal 0.7`

Latest executable authority: `H1 Patch 0015 — Proposal 0.15`

---

## 1. Purpose

Patch 0016 closes the smallest deterministic orchestration seam immediately above Patch 0015.

Patch 0015 already proves the lower authorities for one accepted causal cycle, but the live sequence is still manually assembled in test support:

```text
Opportunity-bearing Production/history
 -> exact bounded Character Context
 -> externally obtained Accepted Take
 -> history-aware source proof
 -> atomic causal commit
 -> accepted-Performance history advancement
 -> FIRST ADOPTION BOUNDARY: committed no-Opportunity Production/history
 -> deterministic Opportunity establishment
 -> accepted-history/Opportunity-history coupling
 -> SECOND ADOPTION BOUNDARY: next Opportunity-bearing synchronized state
```

Patch 0016 makes those adoption boundaries canonical without adding provider execution, retries, persistence, or the full E0 runner.

State machine:

```text
E0OpportunityBearingCycleState
    -- CommitAcceptedTake -->
E0PostCommitCycleState
    -- EstablishOpportunity -->
E0OpportunityBearingCycleState
```

It is a pure immutable value-state machine. Core owns no hidden mutable current-cycle singleton. A successful call returns a validated successor value; the runner/caller explicitly adopts one successor as its current state.

A postcommit successor has completed every proof required for Patch 0015's first adoption boundary. Failure to establish the next Opportunity later cannot retroactively erase that accepted causal commit.

No new fictional semantics or canonical identity are introduced.

---

## 2. Frozen authority preserved

Blueprint 0.1 requires:

- generated Performance enters Production history only through an Accepted Take;
- provider failure/refusal/timeout/retry and cancelled/unaccepted partial output never become fictional action;
- Accepted Performance + approved consequences commit atomically;
- deterministic authority owns state mutation;
- Director owns attention/opportunity, not world truth;
- hard integrity failures are separate from experiential quality;
- E0 uses minimal developer tooling/provenance and excludes product UI, Windows AI/NPU, Store work, long-running cross-Scene persistence, full observation, and World Resolver.

Patch 0015 freezes:

```text
Commit result staged
 -> RecordCommit succeeds
 -> postcommit Production/history is eligible for adoption

Opportunity result staged
 -> RecordOpportunity succeeds
 -> Opportunity-bearing Production/history/OpportunityHistory is eligible for adoption
```

Approved Ship Plan 0.7 requires this deterministic-spine closure before provider-backed E0-A.

---

## 3. Alternatives rejected

1. **Provider-attempt/provenance first** — deferred until technical outcomes can target a stable causal boundary.
2. **Full E0 runner** — too broad; would conflate provider execution, retry/cancellation, Candidate/Integrity/Interpreter/State Authority, Take choice, causal transition, Opportunity transition, and run budgets/provenance.
3. **One-shot Accepted-Take-to-next-Opportunity call** — wrong because it hides Patch 0015's first adoption boundary.
4. **One mutable state plus phase enum** — invalid phase combinations become conventionally expressible.
5. **Hidden mutable coordinator** — conflicts with explicit-input deterministic architecture and complicates repeatability/testing.

Selected: two closed immutable type states + pure deterministic transitions.

---

## 4. Dependency direction

New Core namespace only:

```text
Ensemble.E0.Core.Orchestration
```

`Orchestration` depends downward on existing Production/Context/Continuity/Take/CausalCommit/Opportunity/Director/Domain authority.

No lower namespace depends upward on Orchestration.

No Harness, provider/network, persistence/filesystem, clock/randomness, Task/thread/timer/background, Windows, Windows AI, GPU/NPU/QNN/ONNX, UI, package, or Store dependency.

---

## 5. Exact public surface

Exactly five public Orchestration types:

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

Internal:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
OpportunityHistory         : E0OpportunityHistory
```

No public constructor/setter/declared instance method.

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

This is a validated first-boundary successor, not a staged transaction/rollback object.

### `E0OpportunityBearingCycleResult`

Public read-only:

```text
OpportunityBearingState : E0OpportunityBearingCycleState
OpportunityEvent        : E0OpportunityTransition
DirectorEvaluation      : LeastInterventionDirectorEvaluation
```

No public constructor/setter/declared instance method.

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

Signatures enforce phase legality: Context composition/commit require Opportunity-bearing state; Opportunity establishment requires postcommit state.

### `E0CausalCycleException`

Public/catchable; no public constructor.

Exactly one additional internal Orchestration exception is permitted:

```text
E0CausalCycleInvariantException
```

It is nonpublic and used only to separate closed-wrapper construction failures from public stage normalization. No other new public/internal error hierarchy is needed.

---

## 6. Construction and invariant ownership

Cycle state objects are closed, immutable capability values.

Construction pattern:

```text
internal validated factory
 -> existing lower invariant owners
 -> new cross-component coupling checks
 -> private construction
```

Supported callers cannot raw-construct invalid cycle tokens.

Aggregate wrapper invariants are checked **once when a new wrapper is created**. Public methods do not rescan every aggregate invariant merely to defend against unsupported reflection mutation. Existing lower authorities still freshly validate the lower data they own when called.

Reuse existing owners:

```text
AcceptedPerformanceHistoryInvariants.ValidateAndProject(...)
    -> accepted-history state/roster/text validity + entry projection

OpportunityInvariants.ValidateProductionRoster(...)
OpportunityInvariants.RequireInitialized(...)
    -> E0 roster and strong-ID validity where required
```

`E0OpportunityHistory` itself has closed construction (`private` constructor, validated `Initialize/Advance`). Therefore Patch 0016 does not rescan every historical Opportunity Character. It checks only cross-object facts that can be wrong when otherwise-valid tokens are mixed:

```text
Scene
LastOpportunityStateHash
last/current Character
history count
```

This avoids inventing another Opportunity-history validator and keeps added validation bounded primarily by accepted-history length.

---

## 7. Opportunity-bearing state invariant

Let:

```text
S = ProductionState
H = accepted Performance history entry count
O = OpportunityHistory.CharacterIds count
```

Validated at wrapper creation:

1. existing E0 Production roster validation succeeds;
2. StateHash/SceneId/current Opportunity are initialized;
3. current Opportunity exists and belongs to roster;
4. existing accepted-history invariant proves exact synchronization to `S.SceneId`, `S.StateHash`, roster and yields H entries;
5. Opportunity history Scene equals `S.SceneId`;
6. Opportunity history `LastOpportunityStateHash == S.StateHash`;
7. Opportunity history is initialized/nonempty;
8. its final Character equals current Opportunity;
9. exact count:

```text
O == H + 1
```

Genesis: `H=0, O=1`.

After every completed cycle: `H=n, O=n+1`.

No additional StateHash recomputation/canonical algorithm is added.

---

## 8. `Initialize(...)`

1. require genesis Production state;
2. `E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)`;
3. `E0OpportunityHistory.Initialize(genesisState)`;
4. create validated Opportunity-bearing wrapper;
5. return it.

No arbitrary public rebinding API.

Duplicate genesis validation inside the two existing history initializers is accepted for bounded E0 rather than modifying prior authority solely for micro-optimization.

Expected lower/invariant failure is normalized to:

```text
E0 causal cycle initialization failed.
```

---

## 9. `ComposeContext(...)`

1. reject null source token;
2. capture fresh `ProductionStateCheckpoint`;
3. call:

```text
E0ProductionContextContinuity.ComposeWithAcceptedHistory(
    checkpoint,
    source.AcceptedPerformanceHistory)
```

4. return the existing result unchanged.

Lower Continuity freshly re-proves accepted-history/state binding plus Access/Context composition.

Provenance remains available through:

```text
AccessEvaluation
ContextEvaluation
```

Exact behavior remains v2 at empty genesis and v3 at synchronized evolved history. No Context/canonical change.

Expected lower failure normalizes to:

```text
E0 causal cycle Context composition failed.
```

---

## 10. `CommitAcceptedTake(...)` — phase one

1. reject missing required inputs with safe fixed failure;
2. capture fresh checkpoint;
3. `E0TakeStateBinding.BindWithAcceptedHistory(...)` using source accepted history;
4. `DeterministicCausalCommit.Commit(...)`;
5. keep lower result staged/local;
6. `E0AcceptedPerformanceHistoryContinuity.RecordCommit(...)`;
7. create postcommit wrapper from result state, advanced accepted history, commit event, exact source Context, and source Opportunity history;
8. postcommit factory proves Section 11;
9. return only after that proof succeeds.

Before step 9 there is no Patch 0016 successor eligible for adoption.

After success the caller may adopt the returned postcommit value. Core mutates no hidden global current state.

Expected lower/invariant failure normalizes to:

```text
E0 causal cycle accepted Take commit failed.
```

---

## 11. Postcommit state invariant

Let:

```text
P  = postcommit ProductionState
C  = committed E0CausalCommit
H  = accepted history count after commit
OH = retained source Opportunity-history count
X  = retained source Context
```

Validated once at wrapper creation:

1. existing Production roster validation succeeds;
2. `P.StateHash`, `P.SceneId`, `C.CommitId`, `C.ParentStateHash`, `C.ResultStateHash`, and Accepted Take identities are initialized through existing helpers/objects;
3. `P.CurrentOpportunityCharacterId` is null;
4. `C.ResultStateHash == P.StateHash`;
5. `C.Take.Disposition == Accepted`;
6. `P.ContainsEffectiveCommitId(C.CommitId)`;
7. `P.ContainsCommittedTakeId(C.Take.TakeId)`;
8. existing accepted-history invariant proves synchronization to `P.SceneId`, `P.StateHash`, roster and yields H entries;
9. H is nonzero;
10. final accepted-history item equals committed Performance subject + exact `VisibleText`;
11. source Opportunity-history Scene equals `P.SceneId`;
12. source Opportunity-history `LastOpportunityStateHash == C.ParentStateHash`;
13. source Opportunity history is initialized/nonempty;
14. its final Character equals committed Performance subject;
15. exact phase count:

```text
OH == H
```

16. `X.SourceStateHash` is initialized and equals `C.ParentStateHash`;
17. `X.SceneId == P.SceneId`;
18. `X.ContextPacketId == C.Take.Performance.ContextPacketId`;
19. `X.SubjectCharacterId == C.Take.Performance.SubjectCharacterId`;
20. `X.OpportunityCharacterId == C.Take.Performance.SubjectCharacterId`;
21. X roster is initialized, contains exactly the E0 roster once each, and its Character IDs equal P roster under existing canonical roster semantics.

No parent/result-hash inequality rule is added.

The post wrapper does **not** independently recanonicalize/re-render X; phase one already passed exact `BindWithAcceptedHistory` source proof and Context is immutable. These checks prove only that the retained exact source object still couples to the commit/postcommit data required for phase two.

---

## 12. `EstablishOpportunity(...)` — phase two

1. reject null postcommit token;
2. call `DeterministicOpportunityAuthority.Establish(...)` with token Production, Commit, source Context, source Opportunity history;
3. keep lower Opportunity result staged/local;
4. call `E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(...)`;
5. create new Opportunity-bearing wrapper from Opportunity state, returned accepted-history advancement, and Opportunity history;
6. wrapper proves Section 7;
7. construct result from validated wrapper + exact Opportunity event + exact Director evaluation;
8. return.

Before step 8 no next Patch 0016 Opportunity-bearing successor is eligible for adoption.

If phase two fails, the caller retains its already-created postcommit successor. The failure does not rollback the accepted causal commit.

For E0 run semantics, a deterministic causal-cycle/Opportunity failure is a **hard integrity/system failure**. A future runner must record it diagnostically and stop/fail closed; it must not turn the failure into Character behavior, silently request another fictional Take, or invent a next Opportunity. Recovery policy is later scope.

Expected lower/invariant failure normalizes to:

```text
E0 causal cycle Opportunity establishment failed.
```

---

## 13. Pure transition/adoption law

Methods return deterministic successor values and never mutate their source token.

Repeating a call with the same immutable source and same explicit inputs should derive equivalent existing canonical identities.

A caller can technically derive more than one alternative successor from the same source by calling pure functions with different explicit inputs. Patch 0016 does not create a branch store, canon selector, or multiple-live-head authority.

For E0 the runner owns one current cycle token and explicitly adopts exactly one successful successor at each boundary. Unadopted pure calculations are not silently installed as live Production state.

Branch/rehearsal/canon policy remains post-E0.

---

## 14. Non-Accepted/technical outcomes cannot cross phase one

`CommitAcceptedTake(...)` requires existing Accepted `E0Take` authority; history-aware binding/causal authority re-prove this.

Rejected/Alternate Takes cannot commit.

Patch 0016 defines no RunId, AttemptId, provider/model identity, request/result DTO, partial stream, refusal/timeout enum, retry/spend counter, understudy choice, or cancellation ownership.

Future flow:

```text
OpportunityBearingCycleState
 -> ComposeContext
 -> provider attempt(s) / diagnostics
 -> Candidate -> Integrity -> Interpreter -> State Authority -> Take
 -> Accepted only
 -> CommitAcceptedTake
```

Provider failure therefore has no causal-cycle path in this layer.

---

## 15. Canonical identities unchanged

No persistent event/cycle event/cycle hash/new canonicalizer/new Production field.

Frozen Patch 0015 identities remain exact:

```text
postcommit StateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c

Opportunity StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

MARLOWE v3 structured Context hash
ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
```

No new CommitId/TakeId allocation policy; IDs/materializations remain explicit deterministic inputs for now.

---

## 16. Failure/privacy boundary

Public stage messages are fixed and contain no untrusted content:

```text
E0 causal cycle initialization failed.
E0 causal cycle Context composition failed.
E0 causal cycle accepted Take commit failed.
E0 causal cycle Opportunity establishment failed.
```

Internal invariant failures use fixed structural messages and are normalized by the public stage.

Expected lower exceptions are caught narrowly only where reachable, including `E0AcceptedPerformanceHistoryException`, `E0OpportunityTransitionException`, `E0ContextContinuityException`, and `E0CausalCommitException`.

Implementation audit must inspect every retained `InnerException` chain. Preserve an inner lower exception only if its complete reachable message chain is structural and cannot expose VisibleText, Character-private Context prose, mutation prose, provider/user/imported payload, credentials, or secrets. Otherwise omit/sanitize it.

Unexpected programming/runtime failures are not blanket-wrapped.

---

## 17. Complexity/memory

Wrapper construction adds one accepted-history validation pass plus fixed-size E0 roster/cross-object checks:

```text
Opportunity-bearing creation: O(H) + O(3)
Postcommit creation:          O(H) + O(3)
```

Opportunity history is not rescanned because its own construction is closed/validated; cross-token count/last/hash checks are O(1).

Existing lower authorities retain their own validation costs. Patch 0016 adds no repeated aggregate-wrapper scan before each method.

Long-Scene E0 may remain cumulatively superlinear because Patch 0015 intentionally keeps full accepted history; no quota/window/index is invented here.

Memory is references to immutable existing objects. Postcommit temporarily retains exact source Context + source Opportunity history only because phase-two Opportunity authority requires them.

No background/cache work.

---

## 18. ARM64/battery suitability

Synchronous deterministic CPU/memory only; no x86/emulation, provider/network, filesystem, idle/background work, clock/randomness, Task/thread/timer, Windows API, or GPU/NPU wake.

No measured performance/battery claim.

---

## 19. Expected implementation surface

Source additions only by default:

```text
src/Ensemble.E0.Core/Orchestration/
    E0CausalCycleModels.cs
    DeterministicE0CausalCycle.cs
```

Tests additions only by default:

```text
tests/Ensemble.E0.Core.Tests/Orchestration/
    E0CausalCycleContractAuditTests.cs
    E0CausalCycleTests.cs
    E0CausalCycleDeterminismTests.cs
```

Existing `Patch0015TestSupport.AcceptedTake(...)`, materialization helpers, and frozen lower oracles may be reused by new tests without editing Patch 0015 files.

Default expectation: **zero existing Patch 0015 source/test edits and zero Harness edits**.

Any required semantic modification to Context/CausalCommit/Opportunity/Continuity/Take reopens architecture. If a concrete inherited reflection guard rejects the new namespace, only that exact guard may be adapted and documented; none is currently identified.

---

## 20. Required test matrix

### Public/type-state closure

- exactly five exported Orchestration types;
- exact signatures/properties;
- no public constructors/setters/extra instance methods on state/results;
- exception no public constructor;
- internal invariant exception nonpublic;
- no provider/network/filesystem/Windows/clock/task/thread/timer/GPU/NPU/QNN/ONNX/persistence public signature;
- method parameter types enforce phase legality.

### Validated factories

Reflection may invoke the supported internal validated factories with mixed otherwise-valid immutable components solely to prove cross-object checks:

- accepted-history/state mismatch;
- source Opportunity-history state/count/last mismatch;
- postcommit effective Commit/Take mismatch;
- source Context parent-StateHash mismatch;
- source Context subject/opportunity mismatch;
- source Context roster mismatch.

No public rebind API is added for tests.

### Initialize / Context

- exact Missing Raft genesis;
- invalid/non-genesis fails;
- exact genesis v2 Context oracle;
- evolved exact v3 oracle;
- repeated Context composition identity/bytes stable;
- AccessEvaluation preserved.

### Commit phase

- exact first live Accepted Take -> frozen postcommit StateHash;
- no current Opportunity;
- accepted history advances exactly once;
- source Opportunity history does not advance;
- Rejected/Alternate fail;
- stale/tampered source Context fails through history-aware binding;
- duplicate IDs/materialization rules remain lower authority;
- zero-mutation/all-rejected-durable-consequence Accepted Take preserves Performance history;
- no Opportunity established in phase one.

### Opportunity phase

- exact first route MARLOWE;
- frozen Opportunity StateHash;
- accepted history does not append in phase two;
- Opportunity history advances once;
- final O=H+1;
- next Context exact MARLOWE v3 oracle;
- multi-turn `VOSS -> MARLOWE -> WREN -> VOSS` unchanged;
- repeated identical Performances/recurrence/self-history unchanged.

### Two adoption / purity / hard failure

- postcommit successor exists before phase two;
- phase-one successor exact committed/no-Opportunity state;
- no staged commit successor before RecordCommit;
- no staged Opportunity successor before RecordOpportunity;
- source wrappers remain unchanged;
- repeated identical calls yield equivalent canonical successor identities;
- invalid phase-two construction/failure cannot mutate prior postcommit Production;
- cycle failure produces no Character Performance/next Opportunity and is classified as integrity/system failure in test semantics.

### Dependency/hygiene

- new Orchestration files only introduce new namespace;
- lower source unchanged by default;
- no new canonical serializer/event/hash/provider/platform/persistence dependency;
- Harness unchanged.

---

## 21. Historical lower APIs remain exact

Patch 0016 composes but does not replace/delete prior Context continuity, history-aware binding, CausalCommit Commit/Replay, accepted-history initialize/RecordCommit/RecordOpportunity, Opportunity Establish/Replay, or Opportunity-history Initialize contracts.

No pre-release compatibility right attaches to the new wrapper if later E0 evidence proves it wrong.

---

## 22. Explicit non-scope

Provider/model invocation; request/attempt/retry/spend/streaming provenance; cancellation ownership; refusal/timeout/backoff; understudy selection; Candidate generation; model-assisted Integrity/Interpreter; Take review UX; rejection/alternate retry orchestration; full Scene/run loop; run termination/budgets; E0 run evidence/transcript package; persistence/recovery; cross-Scene history; observation/CharacterClaim disclosure; World Resolver; branch/canon/retcon/rehearsal; WinUI; Windows AI/Aion/Phi/LoRA; Windows ML/QNN/NPU; App Actions/MCP; MSIX/IPackageValidator/WACK/Store.

---

## 23. What Patch 0016 unlocks

```text
OpportunityBearingCycleState
 -> ComposeContext
 -> future technical attempt layer
 -> Candidate / Integrity / Interpreter / State Authority / Take
 -> Accepted only
 -> CommitAcceptedTake
 -> validated PostCommitCycleState
 -> EstablishOpportunity
 -> validated next OpportunityBearingCycleState
```

Technical execution and fictional authority meet at one explicit accepted-commit boundary while preserving Patch 0015's two state-adoption checkpoints.

---

## 24. Proposal correction history

### 0.1 -> 0.2

Type-state law; clearer result naming; removed invented hash inequality; tighter privacy; stopped proposing Patch 0015 test refactor.

### 0.2 -> 0.3

Pure value/adoption semantics; closed-construction validation; reuse existing invariant owners; eliminated repeated aggregate rescans; clarified unadopted pure alternatives are not live branches.

### 0.3 -> 0.4

- strengthened postcommit token so it alone proves every retained source coupling phase two requires: effective Commit/Take IDs, source Context parent StateHash, subject/opportunity, and roster;
- introduced one internal invariant exception so factories remain context-neutral while public cycle methods emit stage-specific safe errors;
- relied on closed `E0OpportunityHistory` construction instead of rescanning its entire sequence, reducing added wrapper validation to O(H)+fixed E0 checks;
- classified deterministic cycle/Opportunity failure as a hard integrity/system failure for future runner semantics, never a fictional retry/action.

Audit restarts from correctness at Proposal 0.4.

---

## 25. Recursive audit closure criterion

Implementation forbidden until one fresh complete pass finds:

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
