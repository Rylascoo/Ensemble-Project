# H1 Patch 0016 — Synchronized Causal Advancement

Status: **BLUEPRINT PROPOSAL 0.3 — EXPLORATORY; RECURSIVE ADVERSARIAL AUDIT RESTARTED FROM CORRECTNESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Authoritative current `main`:

`99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

Program authority:

`docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` — Director-approved Proposal 0.7

Workflow authority:

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` — active v0.2

Parent executable authority:

`H1 Patch 0015 — E0 Accepted Performance History + Context Continuity` — Proposal 0.15, native Windows ARM64 Core authority `571/571` PASS.

The commits after Patch 0015/ship-plan promotion and through current `main` are documentation/process-only. No executable Core/test/fixture authority changed.

---

## 1. Purpose

Patch 0016 closes the smallest deterministic causal-adoption seam still visible after Patch 0015.

Patch 0015 already proves each lower authority required for one accepted causal cycle, but caller/test code still assembles the live sequence manually:

```text
Opportunity-bearing Production + synchronized histories
 -> exact Full Ensemble Character Context
 -> externally obtained/bound Accepted E0Take
 -> history-aware precommit source proof
 -> atomic causal commit
 -> accepted-Performance history advancement
 -> FIRST ADOPTION BOUNDARY:
      committed no-Opportunity Production + synchronized accepted history
 -> deterministic Opportunity establishment
 -> accepted-history / Opportunity-history coupling
 -> SECOND ADOPTION BOUNDARY:
      next Opportunity-bearing Production + synchronized histories
```

Patch 0016 makes those **two existing Patch 0015 adoption boundaries** canonical through closed immutable phase values and pure deterministic transition functions.

It adds no provider execution, retry policy, persistence, new causal event/hash, Application-layer run orchestration, or full E0 runner.

---

## 2. Material correction from Proposals 0.1–0.2

Proposals 0.1–0.2 incorrectly attempted to expose only one all-or-nothing result after Opportunity coupling.

That conflicts with frozen Patch 0015 authority:

```text
Commit result staged
 -> RecordCommit succeeds
 -> postcommit Production/history pair may be adopted

Opportunity result staged
 -> RecordOpportunity succeeds
 -> next Opportunity-bearing Production/history/OpportunityHistory may be adopted
```

A later Opportunity failure cannot retroactively erase an already-valid Accepted Performance + consequence commit.

Therefore:

- one-shot Accepted-Take-to-next-Opportunity composition is **SUPERSEDED**;
- postcommit state is a real first adoption boundary;
- Opportunity establishment is a second deterministic transition from that adopted postcommit state.

Audit restarts from correctness.

---

## 3. Parallel exploratory-branch reconciliation

Two non-authoritative parallel Patch 0016 branches were inspected:

1. `h1-patch-0016-accepted-take-advancement-blueprint`
2. `h1-patch-0016-synchronized-causal-cycle-blueprint`

Neither branch overrides `main` or this proposal merely by existing or having a higher local proposal number.

Useful findings incorporated:

- from Accepted Take Advancement: use **Advancement**, not Core Orchestration, because the approved ship plan reserves capability-neutral Scene/run orchestration for the future Application layer;
- from Synchronized Causal Cycle: preserve Patch 0015's two adoption boundaries with closed immutable phase types;
- from both: exact source Context remains explicit evidence; no new canonical event/hash; caller supplies CommitId/materializations; lower authorities remain canonical; structural composition tests should prove the new layer delegates rather than reimplements.

Rejected from the parallel proposals:

- one-shot all-or-nothing advancement across Opportunity;
- `Ensemble.E0.Core.Orchestration` naming for this Core causal authority;
- exposing the raw `E0OpportunityTransitionResult` as the final live-adoption object, because it would make final Production/OpportunityHistory independently available from the accepted-history synchronization token and weaken the supported aggregate-adoption path;
- broad duplicate lower-domain validation solely to fail earlier.

---

## 4. Why Patch 0016 precedes provider attempts and the full runner

Blueprint 0.1 and frozen H1 architecture distinguish:

```text
provider attempt
 -> may fail technically with no CandidatePerformance

valid CandidatePerformance
 -> provisional semantic output

Integrity / Interpretation / State Authority
 -> Take-bindable deterministic package

Accepted E0Take
 -> selected Performance, still awaiting causal commit

successful causal commit
 -> effective historical Performance + approved consequences

successful Opportunity establishment/coupling
 -> next Character may act
```

Provider refusal, timeout, cancellation, malformed output, partial streaming, retry, spend and network errors must never become fictional action.

A provider-attempt layer should therefore target an already-canonical causal advancement API rather than define fictional adoption order itself.

A full runner now would combine technical attempt policy with fictional authority and exceed the smallest unresolved deterministic seam.

---

## 5. Architectural position

Add one new Core namespace:

```text
Ensemble.E0.Core.Advancement
```

Meaning:

> deterministic advancement of already-authorized causal state, not Application/use-case/provider orchestration.

It may depend downward on existing public/internal Core authority in:

```text
Domain
Production
Context
Take
CausalCommit
Continuity
Opportunity
Director
```

No existing lower subsystem may depend upward on Advancement.

The future Application layer may call Advancement while owning:

- provider-attempt sequencing;
- Scene/run state machines;
- cancellation boundaries;
- retry/spend policy;
- creator commands/use cases;
- persistence transaction coordination.

No provider SDK, Harness dependency, filesystem, network, clock, random, Task/thread/timer, Windows API, GPU/NPU/QNN/ONNX, UI, package or Store dependency enters Core.

---

## 6. State machine

```text
E0OpportunityBearingCycleState
    -- CommitAcceptedTake -->
E0PostCommitCycleState
    -- EstablishOpportunity -->
E0OpportunityBearingCycleState
```

Both state types are closed immutable capability values.

There is no hidden mutable Core singleton/current-cycle service.

A caller explicitly adopts one successful successor value. Calling a pure transition does not silently install a result as live state.

No branch/canon/rehearsal policy is created; E0 runner policy later owns one current adopted token.

---

## 7. Exact public surface

Add exactly five public types in `Ensemble.E0.Core.Advancement`:

```csharp
public sealed class E0OpportunityBearingCycleState
public sealed class E0PostCommitCycleState
public sealed class E0OpportunityBearingCycleResult
public static class DeterministicE0CausalAdvancement
public sealed class E0CausalAdvancementException : Exception
```

No additional public type, enum, interface, delegate, event, builder, allocator, replay API, status enum, provider abstraction, version constant or canonicalizer.

### 7.1 `E0OpportunityBearingCycleState`

Exactly one public get-only property:

```text
ProductionState : ProductionState
```

Internal synchronized state:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
OpportunityHistory         : E0OpportunityHistory
```

No public constructor, setter or declared public instance method.

### 7.2 `E0PostCommitCycleState`

Exactly two public get-only properties:

```text
ProductionState : ProductionState
Commit          : E0CausalCommit
```

Internal retained phase-two evidence/state:

```text
AcceptedPerformanceHistory : E0AcceptedPerformanceHistory
SourceContext               : ContextPacket
SourceOpportunityHistory    : E0OpportunityHistory
```

No public constructor, setter or declared public instance method.

This token is a validated **first-adoption successor**, not an uncommitted transaction promise.

### 7.3 `E0OpportunityBearingCycleResult`

Exactly three public get-only properties:

```text
OpportunityBearingState : E0OpportunityBearingCycleState
OpportunityEvent        : E0OpportunityTransition
DirectorEvaluation      : LeastInterventionDirectorEvaluation
```

No public constructor, setter or declared public instance method.

Do **not** expose the raw `E0OpportunityTransitionResult`; final Production and Opportunity history must remain adopted through the synchronized `OpportunityBearingState` token rather than as an independently selectable lower result.

### 7.4 `DeterministicE0CausalAdvancement`

Exactly four public static methods; no overloads:

```csharp
public static E0OpportunityBearingCycleState Initialize(
    ProductionState genesisState)

public static E0ProductionContextContinuityResult ComposeContext(
    E0OpportunityBearingCycleState source)

public static E0PostCommitCycleState CommitAcceptedTake(
    CommitId commitId,
    E0OpportunityBearingCycleState source,
    ContextPacket sourceContext,
    E0Take acceptedTake,
    E0RecordMaterializationSet materializations)

public static E0OpportunityBearingCycleResult EstablishOpportunity(
    E0PostCommitCycleState source)
```

Method parameter types enforce phase legality for supported callers:

- Context composition and causal commit require Opportunity-bearing state;
- Opportunity establishment requires validated postcommit state.

### 7.5 `E0CausalAdvancementException`

Public/catchable, sealed, no public constructor.

No public failure payload/enum is added.

One internal invariant exception is permitted if implementation needs it to separate closed-state construction failures from public stage normalization:

```text
E0CausalAdvancementInvariantException
```

It must remain nonpublic and carry only fixed structural messages.

---

## 8. Why closed phase tokens are necessary

Without a closed aggregate, a future caller can independently mix:

```text
ProductionState from lineage A
AcceptedPerformanceHistory from lineage B
OpportunityHistory from lineage C
```

and rely on later lower calls to discover mismatches after staging work.

Patch 0016 establishes one supported synchronized live-state capability.

Supported callers cannot raw-construct arbitrary phase values. They obtain them only from:

```text
Initialize(genesis)
CommitAcceptedTake(valid Opportunity-bearing token, ...)
EstablishOpportunity(valid Postcommit token)
```

Aggregate cross-component invariants are proved at token creation. Lower semantic authorities still freshly re-prove the specific data they own when invoked.

The wrapper does not create new fictional truth; it proves that already-existing immutable authority objects belong together at a legal phase boundary.

---

## 9. Invariant ownership and validation economy

Do not duplicate lower algorithms merely to fail sooner.

Reuse existing owners where possible:

```text
ProductionStateCheckpoint.Capture
 -> current-state/current-Opportunity validity

AcceptedPerformanceHistoryInvariants.ValidateAndProject
 -> accepted-history identity/state/roster/text validity

E0TakeStateBinding.BindWithAcceptedHistory
 -> exact supplied source Context + Accepted Take + state/history proof

DeterministicCausalCommit.Commit
 -> CommitId/materialization/current-state causal authority

E0AcceptedPerformanceHistoryContinuity.RecordCommit
 -> canonical causal replay + accepted-Performance history advancement

DeterministicOpportunityAuthority.Establish
 -> postcommit/source-Context/source-routing/Director authority

E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
 -> canonical Opportunity replay + cross-history coupling
```

New wrapper factories/checks own only **cross-object aggregate facts** necessary to prove phase tokens are synchronized.

`AcceptedPerformanceHistoryInvariants.ValidateAndProject(...)` also guarantees the accepted-history `ImmutableArray` is initialized before any count/last-item access; Patch 0016 must not read a default `Entries` array first.

`E0OpportunityHistory` has closed construction through existing validated initialization/advance paths. Patch 0016 checks cross-token anchor/count/last facts and need not rescan its entire historical sequence solely for wrapper creation.

---

## 10. Opportunity-bearing token invariant

Let:

```text
S = ProductionState
H = accepted Performance-history entry count
O = OpportunityHistory.CharacterIds count
```

At token creation prove:

1. `S` is present; StateHash/SceneId/current Opportunity are initialized under existing E0 rules;
2. current Opportunity exists and belongs to the canonical current E0 roster;
3. accepted history is valid and synchronized to `S.SceneId`, `S.StateHash`, and current roster using `AcceptedPerformanceHistoryInvariants.ValidateAndProject(...)`, yielding initialized entries/count H;
4. Opportunity history Scene equals `S.SceneId`;
5. Opportunity history `LastOpportunityStateHash == S.StateHash`;
6. Opportunity history CharacterIds is initialized and nonempty;
7. Opportunity history final Character equals `S.CurrentOpportunityCharacterId`;
8. exact count invariant:

```text
O == H + 1
```

Genesis: `H=0`, `O=1`.

After every completed accepted cycle: `H=n`, `O=n+1`.

No StateHash recomputation or new canonical algorithm is added.

---

## 11. `Initialize(...)`

Exact sequence:

```text
1. require genesis Production state through lower authority
2. acceptedHistory = E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)
3. opportunityHistory = E0OpportunityHistory.Initialize(genesisState)
4. construct/validate Opportunity-bearing token
5. return token
```

There is no public arbitrary `Bind/Rebind` API.

Both existing history initializers intentionally re-prove exact genesis. Bounded E0 may pay that duplicate validation rather than weakening/modifying prior authority for micro-optimization.

Expected lower/invariant failure is normalized to fixed top-level message:

```text
E0 causal advancement initialization failed.
```

---

## 12. `ComposeContext(...)`

Exact sequence:

```text
1. require nonnull Opportunity-bearing token
2. checkpoint = ProductionStateCheckpoint.Capture(source.ProductionState)
3. result = E0ProductionContextContinuity.ComposeWithAcceptedHistory(
       checkpoint,
       source.AcceptedPerformanceHistory)
4. return existing result unchanged
```

The existing result retains AccessEvaluation + ContextEvaluation provenance.

Inherited live Context law remains exact:

```text
exact genesis + initialized empty accepted history
 -> Context v2 / production-bound / render-v1

synchronized evolved state + nonempty accepted history
 -> Context v3 / production-bound.accepted-history / render-v2
```

Historical Context v1 remains compatibility-only in historical lower APIs/tests and is not emitted by `ComposeContext(...)`.

Expected lower failure is normalized to:

```text
E0 causal advancement Context composition failed.
```

---

## 13. Exact source Context remains required phase-one evidence

`CommitAcceptedTake(...)` requires the exact `sourceContext` that travelled through Candidate -> Integrity -> Interpreter -> State Authority -> Take.

It must pass that supplied packet unchanged into:

```text
E0TakeStateBinding.BindWithAcceptedHistory(...)
```

Do not replace it with a freshly recomposed packet inside commit advancement.

Patch 0015 intentionally made history-aware binding the sole full structured+rendered proof that the **supplied** packet equals current state/history-derived Context.

The future provider-attempt layer may later prove that this same Context artifact was associated with the real provider request/framing/disclosure. Patch 0016 preserves that seam and does not claim provider provenance.

---

## 14. `CommitAcceptedTake(...)` — first adoption boundary

Exact order:

```text
1. require nonnull source/sourceContext/acceptedTake/materializations as needed for bounded stage entry

2. checkpoint = ProductionStateCheckpoint.Capture(source.ProductionState)

3. binding = E0TakeStateBinding.BindWithAcceptedHistory(
       checkpoint,
       sourceContext,
       acceptedTake,
       source.AcceptedPerformanceHistory)

4. commitResult = DeterministicCausalCommit.Commit(
       commitId,
       source.ProductionState,
       binding,
       materializations)

5. keep commitResult staged/local

6. historyAfterCommit = E0AcceptedPerformanceHistoryContinuity.RecordCommit(
       source.AcceptedPerformanceHistory,
       source.ProductionState,
       commitResult.Commit)

7. construct validated E0PostCommitCycleState from:
       commitResult.ResultState
       historyAfterCommit
       commitResult.Commit
       exact sourceContext
       source.OpportunityHistory

8. return postcommit token only after its aggregate invariant succeeds
```

No Opportunity is established in this method.

Before step 8, no Patch 0016 successor is eligible for adoption.

After step 8, the returned postcommit token is a valid first-boundary successor. If later Opportunity establishment fails, this accepted causal commit is not rolled back by Patch 0016 semantics.

---

## 15. Postcommit token invariant

Let:

```text
P  = postcommit ProductionState
C  = committed E0CausalCommit
H  = accepted Performance-history count after commit
OH = retained source Opportunity-history count
X  = retained exact source Context
```

At creation prove cross-object facts necessary for phase two:

1. `P` has initialized identity/current E0 roster under existing rules;
2. `P.CurrentOpportunityCharacterId` is null;
3. `C.ResultStateHash == P.StateHash`;
4. `C.Take` exists and is `Accepted`;
5. `P` records `C.CommitId` as effective and `C.Take.TakeId` as committed under existing Production authority;
6. accepted history validates/synchronizes to `P.SceneId`, `P.StateHash`, current roster and yields initialized H entries;
7. `H > 0`;
8. accepted-history final entry equals `C.Take.Performance.SubjectCharacterId` + exact `VisibleText`;
9. retained source Opportunity history Scene equals `P.SceneId`;
10. retained source Opportunity history `LastOpportunityStateHash == C.ParentStateHash`;
11. retained source Opportunity history is initialized/nonempty;
12. retained source Opportunity history final Character equals committed Performance subject;
13. exact postcommit phase count:

```text
OH == H
```

14. `X.SourceStateHash` exists and equals `C.ParentStateHash`;
15. `X.SceneId == P.SceneId`;
16. `X.ContextPacketId == C.Take.Performance.ContextPacketId`;
17. `X.SubjectCharacterId == C.Take.Performance.SubjectCharacterId`;
18. `X.OpportunityCharacterId == C.Take.Performance.SubjectCharacterId`.

The postcommit factory does **not** reserialize/re-render/recompose `X`: phase one already passed exact `BindWithAcceptedHistory` proof and Context objects are closed/read-only. These checks only prove the retained exact source artifact is coupled to the commit/postcommit objects required by phase two.

No parent/result StateHash inequality rule is invented.

---

## 16. `EstablishOpportunity(...)` — second adoption boundary

Exact order:

```text
1. require nonnull validated postcommit token

2. opportunityResult = DeterministicOpportunityAuthority.Establish(
       source.ProductionState,
       source.Commit,
       source.SourceContext,
       source.SourceOpportunityHistory)

3. keep opportunityResult staged/local

4. acceptedHistoryAfterOpportunity =
       E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
           source.AcceptedPerformanceHistory,
           source.ProductionState,
           source.Commit,
           source.SourceOpportunityHistory,
           opportunityResult.Event)

5. construct validated E0OpportunityBearingCycleState from:
       opportunityResult.State
       acceptedHistoryAfterOpportunity
       opportunityResult.History

6. verify returned event/evaluation couple to that exact final lower result

7. construct E0OpportunityBearingCycleResult from:
       validated Opportunity-bearing token
       opportunityResult.Event
       opportunityResult.DirectorEvaluation

8. return only after all coupling succeeds
```

Before step 8 no next Opportunity-bearing Patch 0016 successor is eligible for adoption.

If phase two fails, the caller retains the already-valid `E0PostCommitCycleState`. Patch 0016 does not erase or fabricate a replacement for the accepted causal commit.

A future runner must not translate this deterministic failure into Character behavior, a fictional alternate Take, or invented next Opportunity. Retry/recovery/diagnostic policy remains later run/persistence architecture.

Expected failure normalizes to:

```text
E0 causal advancement Opportunity establishment failed.
```

---

## 17. Pure transition/adoption law

All Patch 0016 methods are synchronous pure deterministic compositions over immutable explicit inputs, except for ordinary allocation of immutable successor objects.

They mutate no hidden/global current state.

Repeating a call with equivalent immutable inputs and the same explicit identity/materialization inputs derives equivalent existing canonical identities.

A caller can mathematically derive multiple candidate successors from one immutable source by invoking pure functions with different explicit inputs. Patch 0016 creates no branch store or canon selector and installs none automatically.

The E0 run layer later owns one current adopted cycle token and may adopt exactly one successful successor under its separately frozen run policy.

---

## 18. Accepted-only / technical-failure boundary

`CommitAcceptedTake(...)` accepts an already-existing `E0Take`; inherited `BindWithAcceptedHistory` requires disposition `Accepted` and re-proves association.

Patch 0016 does not:

- create a Take;
- choose Accepted/Rejected/Alternate;
- retry rejected or alternate Takes;
- create a no-op event for failure;
- invoke providers;
- interpret refusal/timeout/cancellation/partial/malformed outputs.

Rejected/Alternate Takes fail before causal commit and produce no Patch 0016 successor.

Provider/attempt failures have no causal-advancement path here.

---

## 19. Identity/materialization ownership

Patch 0016 allocates no TakeId, CommitId, RecordId, RunId, AttemptId, request ID or timestamp.

Caller supplies:

- source Context;
- already-bound Take;
- CommitId;
- `E0RecordMaterializationSet`.

Existing lower authority validates them.

No clock/random/global counter enters Core.

---

## 20. No new canonical identity or replay authority

Patch 0016 introduces:

- no causal-cycle event;
- no cycle hash;
- no schema/version constant;
- no new Production field;
- no canonical serializer;
- no Replay method.

Existing canonical authorities remain:

```text
E0CausalCommit
DeterministicCausalCommit.Replay
E0OpportunityTransition
DeterministicOpportunityAuthority.Replay
E0AcceptedPerformanceHistoryContinuity.RecordCommit/RecordOpportunity
```

Future persistence/recovery composes those under separate architecture.

---

## 21. Reference oracle remains exact

No canonical bytes/hash inputs change.

First live lineage remains:

```text
source StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

source ContextPacketId
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1

ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3

CommitId
COMMIT-PATCH-0012-ORACLE

postcommit StateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c

Opportunity selected Character
MARLOWE

Opportunity-bearing StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

next MARLOWE ContextPacketId
CTX:ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f

next rendered Context hash
668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16
```

Any changed existing oracle identity reopens architecture unless a lower-layer defect is independently proven.

---

## 22. Failure/privacy boundary

One public exception domain:

`E0CausalAdvancementException`

Public stage messages are fixed and contain no untrusted payload:

```text
E0 causal advancement initialization failed.
E0 causal advancement Context composition failed.
E0 causal advancement accepted Take commit failed.
E0 causal advancement Opportunity establishment failed.
```

Expected lower exceptions are caught **narrowly at the stage that calls them**, never by a broad `catch (Exception)`.

Reachable expected domains include:

```text
E0AcceptedPerformanceHistoryException
E0OpportunityTransitionException
E0ContextContinuityException
E0CausalCommitException
E0CausalAdvancementInvariantException (internal, if used)
```

The implementation audit must inspect every retained `InnerException` chain. Preserve a lower exception as `InnerException` only when its complete reachable message chain is structural and cannot expose:

- Candidate VisibleText;
- rendered/private Context prose;
- mutation/record prose;
- provider/user/imported payload;
- credentials/secrets.

Otherwise normalize without retaining that unsafe inner chain.

Unexpected programming/runtime failures are not blanket-wrapped.

---

## 23. Complexity / memory / ARM64

New phase-token validation may scan accepted Performance history through the existing invariant owner when constructing a token:

```text
Opportunity-bearing token: O(H) + fixed E0 roster/cross checks
Postcommit token:          O(H) + fixed E0 roster/cross checks
```

Opportunity history is not rescanned because its own construction is closed/validated; only cross-token Scene/hash/count/last facts are checked.

Existing lower authorities retain their prior validation/replay costs. Patch 0016 does not invent another global history window/index/quota.

Postcommit state temporarily retains references to exact source Context and source Opportunity history because phase-two authority requires them. No payload copy is required.

No background/cache/polling/network/filesystem/provider/GPU/NPU/Windows work.

Native ARM64/battery suitability is architectural only until machine evidence; no measured performance claim.

---

## 24. Structural delegation law

Because Patch 0016 exists to own composition rather than duplicate semantics, implementation tests must prove direct call ownership.

`Initialize(...)` calls exactly once each:

```text
E0AcceptedPerformanceHistoryContinuity.Initialize
E0OpportunityHistory.Initialize
```

`ComposeContext(...)` calls exactly once each:

```text
ProductionStateCheckpoint.Capture
E0ProductionContextContinuity.ComposeWithAcceptedHistory
```

`CommitAcceptedTake(...)` calls exactly once each:

```text
ProductionStateCheckpoint.Capture
E0TakeStateBinding.BindWithAcceptedHistory
DeterministicCausalCommit.Commit
E0AcceptedPerformanceHistoryContinuity.RecordCommit
```

and directly calls neither Opportunity Establish nor RecordOpportunity.

`EstablishOpportunity(...)` calls exactly once each:

```text
DeterministicOpportunityAuthority.Establish
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
```

and directly calls no causal Commit/Bind.

New Advancement code directly calls none of:

```text
CharacterBoundedAccessControl
DeterministicContextComposer
DeterministicCausalCommit.Replay
DeterministicOpportunityAuthority.Replay
```

The lower authorities may themselves call canonical replay/composition as already frozen.

Structural tests also prove no broad catch-all normalization.

If existing IL-inspection test code would otherwise be duplicated, implementation may extract the smallest **test-only** shared helper while preserving all historical assertions.

---

## 25. Expected implementation surface

Preferred production additions:

```text
src/Ensemble.E0.Core/Advancement/
    E0CausalAdvancementModels.cs
    DeterministicE0CausalAdvancement.cs
```

Focused tests:

```text
tests/Ensemble.E0.Core.Tests/Advancement/
    E0CausalAdvancementContractAuditTests.cs
    E0CausalAdvancementTests.cs
    E0CausalAdvancementDeterminismTests.cs
    E0CausalAdvancementStructuralTests.cs
```

Optional smallest test-only IL helper extraction if genuinely earned by second use.

Default expectation: zero semantic edits to:

```text
Domain
Fixture
Production
Access
Context
Performer
Director
Integrity
StateInterpreter
StateAuthority
Take
CausalCommit
Opportunity
Continuity
Harness
```

No existing Continuity namespace public-surface guard should require relaxation because Patch 0016 adds a new `Advancement` namespace.

If implementation requires a semantic lower-layer modification rather than composing existing authority, stop and reopen architecture.

---

## 26. Required test matrix

Do not mirror every lower Patch 0012–0015 test through the wrapper. Lower suites remain authority for lower algorithms; Patch 0016 tests focus on aggregate ownership/type-state/adoption.

### Public/type-state closure

1. exactly five exported `Ensemble.E0.Core.Advancement` types;
2. exact four static method signatures/no overloads;
3. Opportunity-bearing token exactly one public get-only `ProductionState` property, no public ctor/setter/declared method;
4. Postcommit token exactly public get-only `ProductionState` + `Commit`, no public ctor/setter/declared method;
5. final result exactly `OpportunityBearingState`, `OpportunityEvent`, `DirectorEvaluation`, no public ctor/setter/declared method;
6. exception sealed/catchable/no public ctor;
7. no forbidden provider/network/filesystem/async/thread/time/random/Windows/hardware/persistence type in new public signatures;
8. method parameter types enforce phase legality.

### Initialize / source aggregate

9. exact Missing Raft genesis initializes H=0/O=1 synchronized token;
10. non-genesis/invalid source fails;
11. closed token construction rejects mixed otherwise-valid Production/history components in internal invariant tests without adding public rebind API.

### Context

12. genesis `ComposeContext` produces exact historical Production-bound v2 oracle;
13. evolved token produces exact v3 oracle;
14. repeated composition is exact/deterministic and preserves AccessEvaluation;
15. no historical v1 live Context is emitted.

### Commit phase / first adoption

16. exact first live Accepted Take produces frozen postcommit StateHash `a7e6e1d5...`;
17. returned postcommit Production has no current Opportunity;
18. accepted history advances exactly once and ends at committed Performance;
19. retained source Opportunity history does not advance;
20. no Opportunity event/result is publicly produced by phase one;
21. Rejected/Alternate Take fails before causal successor;
22. representative structured/rendered source-Context tamper fails through inherited history-aware binder;
23. representative CommitId/materialization failure fails with no postcommit successor;
24. zero-mutation Accepted Take still records Performance;
25. all-durable-consequence-Rejected Accepted Take still records Performance;
26. postcommit invariant rejects mixed commit/state/history/context/routing components through internal invariant tests.

### Opportunity phase / second adoption

27. first phase-two transition selects MARLOWE and preserves frozen Opportunity StateHash `e935dc6...`;
28. accepted history does not append during phase two; only its state anchor advances;
29. Opportunity history advances exactly once;
30. final exact count law `O = H + 1` holds;
31. next `ComposeContext` produces exact MARLOWE v3 oracle;
32. multi-turn `VOSS -> MARLOWE -> WREN -> VOSS` remains exact;
33. repeated identical Performance / Character recurrence / self-history remains exact;
34. a phase-two failure exposes no next Opportunity-bearing token and does not invalidate/mutate the existing postcommit token;
35. no fictional Performance/Opportunity is invented on deterministic phase-two failure.

### Equivalence / determinism / composition

36. wrapper-driven first cycle is canonically equivalent to independent Patch 0015 primitive-chain oracle at each corresponding adoption boundary;
37. identical repeated pure calls over same immutable inputs derive equivalent existing canonical identities;
38. culture change cannot affect existing canonical identities/order;
39. exact structural delegation laws in section 24 hold;
40. no Advancement replay/event/hash/version/canonicalizer exists.

### Historical/native regression

41. complete existing Core suite remains green;
42. all Patch 0015 reference-oracle values remain exact;
43. native Windows ARM64 full Core tests run on implementation head;
44. native Windows ARM64 Harness build runs because the referenced Core assembly changed;
45. existing Missing Raft and generic-smoke Harness fixtures execute successfully on the same executable tree.

Static analysis cannot satisfy items 43–45.

---

## 27. Explicit non-scope

Patch 0016 does **not** add:

- provider/model invocation;
- request/attempt/result provenance;
- partial streaming;
- refusal/timeout/cancellation/retry/backoff/spend/cost policy;
- automatic Candidate generation;
- automatic Integrity/Interpreter/State Authority generation;
- Take decision/review UX;
- Rejected/Alternate run policy;
- full Scene/run loop;
- run termination/budgets;
- E0 run evidence/transcript package;
- persistence/recovery/event store;
- cross-Scene history;
- general CharacterObservation/CharacterClaim promotion;
- World Resolver;
- branch/canon/retcon/rehearsal;
- WinUI/Application implementation;
- Windows AI/Aion/Phi/LoRA;
- Windows ML/QNN/NPU;
- App Actions/MCP;
- MSIX/IPackageValidator/WACK/Store.

---

## 28. What Patch 0016 unlocks

```text
E0OpportunityBearingCycleState
 -> ComposeContext
 -> future provider-attempt / Candidate / semantic pipeline
 -> Accepted E0Take
 -> CommitAcceptedTake
 -> validated E0PostCommitCycleState
 -> EstablishOpportunity
 -> validated next E0OpportunityBearingCycleState
```

Technical execution and fictional authority meet only at an explicit already-Accepted Take boundary.

Patch 0016 remains an intermediate H1 seam; provider-attempt/run-driver architecture still remains before H1/E0-A closure.

---

## 29. Proposal correction history

### 0.1

Initial one-shot Accepted-Take continuity wrapper.

### 0.1 -> 0.2

Added source-triple preflight, live v2/v3 law, explicit native Harness validation, one-supported-live-path rule, final synchronization equations and current-main reconciliation.

### 0.2 -> 0.3

Material authority correction after recursive audit and reconciliation with non-authoritative parallel Patch 0016 proposals:

- restored Patch 0015's frozen **two adoption boundaries**;
- rejected all-or-nothing causal+Opportunity wrapper semantics;
- introduced closed immutable phase tokens so synchronization is guaranteed by supported construction rather than independently mixable arguments;
- placed the Core capability in `Advancement`, not `Orchestration`, preserving the ship plan's future Application-layer orchestration ownership;
- preserved exact caller-supplied source Context through phase one and into phase two;
- prevented final result from exposing raw `E0OpportunityTransitionResult` as an alternative live-state adoption path;
- added explicit source/postcommit aggregate invariants while reusing lower semantic authorities;
- closed the default accepted-history `ImmutableArray` count-access hazard by requiring existing `ValidateAndProject` before count/last access;
- added structural delegation tests and native Harness/fixture gates.

Audit restarts from correctness at Proposal 0.3.

---

## 30. Recursive audit closure criterion

Implementation remains forbidden until one **fresh complete pass** over Proposal 0.3 or later finds:

```text
0 material correctness corrections
0 authority/adoption corrections
0 dependency-direction corrections
0 synchronization/type-state corrections
0 source-Context proof corrections
0 failure/privacy corrections
0 canonical/hash/version corrections
0 E0/ship-plan scope corrections
0 worthwhile public-surface simplifications
0 worthwhile test improvements
0 ARM64/battery/hygiene corrections
0 evidence corrections
```

Any material/worthwhile correction increments the proposal version and restarts from the affected authority layer.

Only one clean full pass permits blueprint-audit evidence and explicit Director approval request.
