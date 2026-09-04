# H1 Patch 0016 — Synchronized Causal Advancement

Status: **BLUEPRINT PROPOSAL 0.4 — EXPLORATORY; RECURSIVE ADVERSARIAL AUDIT RESTARTED FROM CORRECTNESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-04

Authoritative current `main`:

`99e0b9fc7fc346a7276928df8f92a6e133f58c2f`

Program authority:

`docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` — Director-approved Proposal 0.7

Workflow authority:

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` — active v0.2

Parent executable authority:

`H1 Patch 0015 — E0 Accepted Performance History + Context Continuity` — Proposal 0.15; native Windows ARM64 full Core authority `571/571` PASS.

The commits after Patch 0015/ship-plan promotion and through current `main` are documentation/process-only. No executable Core/test/fixture authority changed.

---

## 1. Purpose

Patch 0016 closes the smallest deterministic causal-adoption seam still visible after Patch 0015.

Patch 0015 already proves every lower authority required for one accepted causal cycle, but caller/test code still assembles the live sequence manually:

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

Patch 0016 makes those two already-frozen Patch 0015 adoption boundaries canonical through closed immutable phase values and pure deterministic transition functions.

It adds no provider execution, retry policy, persistence, new causal event/hash, Application-layer run orchestration, or full E0 runner.

---

## 2. Why two adoption boundaries are mandatory

Earlier exploratory one-shot designs attempted to return only a final next-Opportunity result.

That is incompatible with Patch 0015:

```text
Commit result staged
 -> RecordCommit succeeds
 -> postcommit Production/history pair becomes eligible for adoption

Opportunity result staged
 -> RecordOpportunity succeeds
 -> next Opportunity-bearing Production/history/OpportunityHistory becomes eligible for adoption
```

A later Opportunity failure cannot retroactively erase an already-valid Accepted Performance + approved-consequence commit.

Therefore:

- one-shot Accepted-Take-to-next-Opportunity composition is superseded;
- postcommit state is a real first successor/adoption boundary;
- Opportunity establishment is a second deterministic transition from that postcommit successor.

---

## 3. Why this precedes provider attempts and the full runner

Frozen E0 authority distinguishes:

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

A provider-attempt layer should therefore target an already-canonical causal Advancement boundary. A full runner now would conflate technical attempt policy with fictional authority.

Patch 0016 closes the deterministic post-Accepted-Take seam first.

---

## 4. Exploratory-branch reconciliation

Non-authoritative parallel Patch 0016 explorations were inspected:

- `h1-patch-0016-accepted-take-advancement-blueprint`;
- `h1-patch-0016-synchronized-causal-cycle-blueprint`;
- earlier one-shot state on this branch.

Useful conclusions retained:

1. use **Advancement**, not Core `Orchestration`, because the approved ship plan reserves capability-neutral Scene/run orchestration for the future Application layer;
2. preserve two adoption boundaries with closed phase types;
3. carry the exact supplied source Context through commit proof and Opportunity establishment;
4. add no canonical cycle event/hash/version;
5. caller supplies CommitId/materializations;
6. lower deterministic authorities remain canonical;
7. structural tests prove delegation rather than semantic reimplementation.

Rejected:

- one-shot all-or-nothing causal + Opportunity advancement;
- Core `Orchestration` naming;
- raw final `E0OpportunityTransitionResult` as an alternative live-state adoption path;
- broad duplicate lower-domain validation solely to fail sooner;
- provider-attempt or run-driver scope in Patch 0016.

---

## 5. Architectural position

Add one Core namespace:

```text
Ensemble.E0.Core.Advancement
```

Meaning:

> deterministic advancement of already-authorized causal state, not Application/use-case/provider orchestration.

Advancement may depend downward on existing Core authority in:

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

The future Application layer may call Advancement while owning provider-attempt sequencing, Scene/run state machines, cancellation, retry/spend policy, creator commands and persistence transaction coordination.

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

There is no hidden mutable current-cycle service. A caller explicitly adopts a returned successor value. Pure transition calls do not install global/live state.

Patch 0016 creates no branch/canon/rehearsal policy. A later runner may own one current adopted cycle token under separately approved policy.

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

No additional public enum, interface, delegate, event, builder, allocator, replay API, status/failure object, provider abstraction, version constant or canonicalizer.

### `E0OpportunityBearingCycleState`

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

### `E0PostCommitCycleState`

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

This token is a validated first-adoption successor, not an uncommitted transaction promise.

### `E0OpportunityBearingCycleResult`

Exactly three public get-only properties:

```text
OpportunityBearingState : E0OpportunityBearingCycleState
OpportunityEvent        : E0OpportunityTransition
DirectorEvaluation      : LeastInterventionDirectorEvaluation
```

No public constructor, setter or declared public instance method.

Do not expose the raw `E0OpportunityTransitionResult`. Final Production and Opportunity history must remain adopted through the synchronized `OpportunityBearingState` rather than as independently selectable lower results.

### `DeterministicE0CausalAdvancement`

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

Parameter types enforce supported phase legality.

### `E0CausalAdvancementException`

Public/catchable, sealed, no public constructor.

One internal invariant exception is permitted if needed to normalize closed-state construction failures. It must remain nonpublic and use fixed structural messages only.

---

## 8. Closed phase-token law

Without a closed aggregate a future caller could independently mix otherwise-valid:

```text
ProductionState from lineage A
AcceptedPerformanceHistory from lineage B
OpportunityHistory from lineage C
```

Patch 0016 makes one supported synchronized capability value.

Supported public callers obtain tokens only through:

```text
Initialize(genesis)
CommitAcceptedTake(valid Opportunity-bearing token, ...)
EstablishOpportunity(valid postcommit token)
```

Closed construction prevents public component mixing. Internal token creation checks only cross-object facts needed to prove the aggregate phase; lower authorities remain responsible for their own semantics.

Do not add a public Bind/Rebind/raw factory solely for tests.

---

## 9. Invariant ownership and validation economy

Reuse existing owners rather than copy them:

```text
ProductionStateCheckpoint.Capture
 -> current-state/current-Opportunity proof

OpportunityInvariants / existing Production internals
 -> current E0 roster + strong identity facts where aggregate construction requires them

AcceptedPerformanceHistoryInvariants.ValidateAndProject
 -> accepted-history state/roster/text validity + initialized ordered entries

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

New code must not read accepted-history count/last item before existing validation proves its `ImmutableArray` initialized.

`E0OpportunityHistory` already has closed validated construction. Advancement checks only aggregate Scene/hash/count/last coupling and does not rescan its entire historical sequence solely for wrapper creation.

---

## 10. Opportunity-bearing state invariant

Let:

```text
S = ProductionState
H = accepted Performance-history count
O = OpportunityHistory.CharacterIds count
```

At token creation prove:

1. S is present with initialized StateHash/SceneId/current Opportunity under E0 rules;
2. current Opportunity belongs to the canonical current E0 roster;
3. accepted history validates/synchronizes to S.SceneId, S.StateHash and current roster, yielding initialized H entries;
4. Opportunity history Scene == S.SceneId;
5. Opportunity history LastOpportunityStateHash == S.StateHash;
6. Opportunity history CharacterIds is initialized/nonempty;
7. its final Character == S.CurrentOpportunityCharacterId;
8. exact count law:

```text
O == H + 1
```

Genesis: H=0, O=1.

After every completed accepted cycle: H=n, O=n+1.

No StateHash recomputation or new canonical algorithm is added.

---

## 11. `Initialize(...)`

Exact composition:

```text
acceptedHistory = E0AcceptedPerformanceHistoryContinuity.Initialize(genesisState)
opportunityHistory = E0OpportunityHistory.Initialize(genesisState)
construct/validate E0OpportunityBearingCycleState(genesisState, acceptedHistory, opportunityHistory)
return token
```

Both existing history initializers intentionally re-prove exact genesis. Bounded E0 accepts that duplicate validation rather than weakening prior authority for micro-optimization.

Expected failure normalizes to fixed public stage message:

```text
E0 causal advancement initialization failed.
```

---

## 12. `ComposeContext(...)`

Exact composition:

```text
checkpoint = ProductionStateCheckpoint.Capture(source.ProductionState)
return E0ProductionContextContinuity.ComposeWithAcceptedHistory(
    checkpoint,
    source.AcceptedPerformanceHistory)
```

The returned existing result preserves `AccessEvaluation` + `ContextEvaluation` evidence.

Inherited live Context law remains exact:

```text
exact genesis + initialized empty accepted history
 -> Context v2 / production-bound / render-v1

synchronized evolved state + nonempty accepted history
 -> Context v3 / production-bound.accepted-history / render-v2
```

Historical Context v1 remains compatibility-only in historical lower APIs/tests and is never emitted by Advancement `ComposeContext`.

Expected failure message:

```text
E0 causal advancement Context composition failed.
```

---

## 13. Exact source Context remains phase-one evidence

`CommitAcceptedTake(...)` requires the exact supplied `sourceContext` that travelled through Candidate -> Integrity -> Interpreter -> State Authority -> Take.

It passes that packet unchanged into:

```text
E0TakeStateBinding.BindWithAcceptedHistory(...)
```

Do not replace it with a freshly recomposed packet inside Advancement.

Patch 0015 intentionally makes the history-aware binder the full structured+rendered proof that the supplied Context equals current state/history-derived Context.

A future provider-attempt layer may later prove that this same Context artifact was associated with the provider request/framing/disclosure. Patch 0016 does not claim provider provenance.

---

## 14. `CommitAcceptedTake(...)` — first adoption boundary

Exact order:

```text
1. checkpoint = ProductionStateCheckpoint.Capture(source.ProductionState)

2. binding = E0TakeStateBinding.BindWithAcceptedHistory(
       checkpoint,
       sourceContext,
       acceptedTake,
       source.AcceptedPerformanceHistory)

3. commitResult = DeterministicCausalCommit.Commit(
       commitId,
       source.ProductionState,
       binding,
       materializations)

4. keep commitResult staged/local

5. historyAfterCommit = E0AcceptedPerformanceHistoryContinuity.RecordCommit(
       source.AcceptedPerformanceHistory,
       source.ProductionState,
       commitResult.Commit)

6. construct/validate E0PostCommitCycleState from:
       commitResult.ResultState
       historyAfterCommit
       commitResult.Commit
       exact sourceContext
       source.OpportunityHistory

7. return postcommit token only after aggregate construction succeeds
```

No Opportunity is established in this method.

Before step 7, no Patch 0016 successor is eligible for adoption. After step 7, the returned postcommit token is a valid first-boundary successor.

---

## 15. Postcommit state invariant

Let:

```text
P  = postcommit ProductionState
C  = committed E0CausalCommit
H  = accepted Performance-history count after commit
OH = retained source Opportunity-history count
X  = retained exact source Context
```

At token creation prove cross-object facts necessary for phase two:

1. P has initialized StateHash/SceneId and canonical current E0 roster;
2. P.CurrentOpportunityCharacterId is null;
3. C.ResultStateHash == P.StateHash;
4. C.Take exists and is Accepted;
5. P records C.CommitId as effective and C.Take.TakeId as committed;
6. accepted history validates/synchronizes to P.SceneId, P.StateHash and current roster, yielding initialized H entries;
7. H > 0;
8. final accepted-history entry equals C.Take.Performance.SubjectCharacterId + exact VisibleText;
9. retained source Opportunity history Scene == P.SceneId;
10. retained source Opportunity history LastOpportunityStateHash == C.ParentStateHash;
11. retained source Opportunity history is initialized/nonempty;
12. its final Character == committed Performance subject;
13. exact phase count:

```text
OH == H
```

14. X.SourceStateHash exists and == C.ParentStateHash;
15. X.SceneId == P.SceneId;
16. X.ContextPacketId == C.Take.Performance.ContextPacketId;
17. X.SubjectCharacterId == C.Take.Performance.SubjectCharacterId;
18. X.OpportunityCharacterId == C.Take.Performance.SubjectCharacterId.

Do not reserialize/re-render/recompose X here. Phase one already passed exact history-aware source proof and Context objects are closed/read-only.

No parent/result StateHash inequality rule is invented.

---

## 16. `EstablishOpportunity(...)` — second adoption boundary

Exact order:

```text
1. opportunityResult = DeterministicOpportunityAuthority.Establish(
       source.ProductionState,
       source.Commit,
       source.SourceContext,
       source.SourceOpportunityHistory)

2. keep opportunityResult staged/local

3. acceptedHistoryAfterOpportunity =
       E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
           source.AcceptedPerformanceHistory,
           source.ProductionState,
           source.Commit,
           source.SourceOpportunityHistory,
           opportunityResult.Event)

4. construct/validate E0OpportunityBearingCycleState from:
       opportunityResult.State
       acceptedHistoryAfterOpportunity
       opportunityResult.History

5. verify event/evaluation couple to that exact final lower result

6. return E0OpportunityBearingCycleResult(
       validated Opportunity-bearing token,
       opportunityResult.Event,
       opportunityResult.DirectorEvaluation)
```

No next Opportunity-bearing successor is eligible for adoption until step 6 returns successfully.

If phase two fails, the caller still owns the already-valid immutable `E0PostCommitCycleState`; Patch 0016 does not erase or fabricate a replacement for the accepted causal commit.

For a supported valid postcommit token, current E0 lower invariants are intended to make phase two deterministic and total. A lower failure from such a token is therefore an integrity/system defect, not Character behavior, not an Alternate Take, and not fictional retry policy.

Do not add fault-injection seams or forge impossible private state solely to manufacture a public phase-two failure test. Verify the failure/adoption law through closed-construction invariant tests, structural delegation, exception normalization, and immutability of existing valid tokens.

Expected failure message:

```text
E0 causal advancement Opportunity establishment failed.
```

---

## 17. Pure transition/adoption law

All Patch 0016 methods are synchronous deterministic compositions over immutable explicit inputs, apart from ordinary immutable allocation.

They mutate no hidden/global current state.

Repeating a call with equivalent immutable inputs and the same explicit CommitId/materializations derives equivalent existing canonical identities.

A caller can mathematically derive multiple candidate successors from one immutable source with different explicit inputs. Patch 0016 creates no branch store/canon selector and installs none automatically.

A later E0 run layer may own one current adopted token under separately approved run policy.

---

## 18. Accepted-only / technical-failure boundary

`CommitAcceptedTake(...)` accepts an existing `E0Take`. Inherited history-aware binding requires disposition `Accepted` and re-proves association.

Patch 0016 does not create a Take, select Accepted/Rejected/Alternate, retry rejected/alternate Takes, create no-op failure events, invoke providers, or interpret refusal/timeout/cancellation/partial/malformed output.

Rejected/Alternate Takes produce no Advancement successor.

Provider/attempt failures have no causal-advancement path here.

---

## 19. Identity/materialization ownership

Patch 0016 allocates no TakeId, CommitId, RecordId, RunId, AttemptId, request ID or timestamp.

Caller supplies source Context, already-bound Take, CommitId and `E0RecordMaterializationSet`.

Existing lower authority validates them.

No clock/random/global counter enters Core.

---

## 20. No new canonical identity or replay authority

Patch 0016 adds no causal-cycle event/hash/schema/version, Production field, serializer/canonicalizer or Replay method.

Canonical replay remains:

```text
E0CausalCommit
 -> DeterministicCausalCommit.Replay

E0OpportunityTransition
 -> DeterministicOpportunityAuthority.Replay

accepted-history coupling
 -> E0AcceptedPerformanceHistoryContinuity.RecordCommit/RecordOpportunity
```

Future persistence/recovery composes those under separate architecture.

---

## 21. Reference oracle remains exact

Patch 0016 changes no canonical bytes/hash inputs.

Exact first live lineage remains:

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

Any change to an existing oracle identity reopens architecture unless an independent lower-layer defect is proven.

---

## 22. Failure/privacy boundary

One public failure domain:

`E0CausalAdvancementException`

Fixed public stage messages only:

```text
E0 causal advancement initialization failed.
E0 causal advancement Context composition failed.
E0 causal advancement accepted Take commit failed.
E0 causal advancement Opportunity establishment failed.
```

Catch expected lower exceptions narrowly at the stage that invokes them; never use broad `catch (Exception)` normalization.

Reachable expected domains include existing Continuity/Opportunity/CausalCommit exceptions plus the optional internal Advancement invariant exception.

A retained `InnerException` is permitted only after implementation audit proves the complete reachable message chain structural and incapable of exposing Candidate VisibleText, rendered/private Context prose, mutation/record prose, provider/user/imported payload or credentials/secrets. Otherwise normalize without retaining the unsafe inner chain.

Unexpected programming/runtime failures are not blanket-wrapped.

---

## 23. Complexity / memory / ARM64

New aggregate construction may scan accepted Performance history through its existing invariant owner:

```text
Opportunity-bearing token: O(H) + fixed E0 cross checks
Postcommit token:          O(H) + fixed E0 cross checks
```

Opportunity history is not rescanned; only Scene/hash/count/last coupling is checked.

Existing lower replay/validation costs remain unchanged. Patch 0016 adds no history window/index/quota.

Postcommit state temporarily retains references to exact source Context and source Opportunity history because phase two requires them. No payload copy is required.

No background/cache/polling/network/filesystem/provider/GPU/NPU/Windows work.

Native ARM64/battery suitability is architectural until machine evidence; no measured performance claim.

---

## 24. Structural delegation law

Patch 0016 owns composition, not duplicated semantics.

Direct call ownership:

```text
Initialize
 -> E0AcceptedPerformanceHistoryContinuity.Initialize exactly once
 -> E0OpportunityHistory.Initialize exactly once

ComposeContext
 -> ProductionStateCheckpoint.Capture exactly once
 -> E0ProductionContextContinuity.ComposeWithAcceptedHistory exactly once

CommitAcceptedTake
 -> ProductionStateCheckpoint.Capture exactly once
 -> E0TakeStateBinding.BindWithAcceptedHistory exactly once
 -> DeterministicCausalCommit.Commit exactly once
 -> E0AcceptedPerformanceHistoryContinuity.RecordCommit exactly once
 -> directly calls neither Opportunity Establish nor RecordOpportunity

EstablishOpportunity
 -> DeterministicOpportunityAuthority.Establish exactly once
 -> E0AcceptedPerformanceHistoryContinuity.RecordOpportunity exactly once
 -> directly calls no causal Commit/Bind
```

Advancement directly calls none of:

```text
CharacterBoundedAccessControl
DeterministicContextComposer
DeterministicCausalCommit.Replay
DeterministicOpportunityAuthority.Replay
```

Lower authorities may continue to call canonical replay/composition as already frozen.

Structural tests also prove no broad catch-all normalization.

If existing IL-inspection code would otherwise be duplicated, implementation may extract the smallest test-only shared helper without weakening historical assertions.

---

## 25. Expected implementation surface

Preferred production additions only:

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

Optional smallest test-only IL helper extraction if genuinely earned.

Default expectation: zero semantic edits to Domain, Fixture, Production, Access, Context, Performer, Director, Integrity, StateInterpreter, StateAuthority, Take, CausalCommit, Opportunity, Continuity or Harness.

Because Patch 0016 adds a new namespace, existing exact public-surface tests for lower namespaces should remain unchanged.

If implementation requires a lower semantic modification rather than composing existing authority, stop and reopen architecture.

---

## 26. Required test matrix

Do not mirror every lower Patch 0012–0015 test. Lower suites remain authority for lower algorithms; Patch 0016 tests aggregate ownership/type-state/adoption only.

### Public/type-state closure

1. exactly five exported Advancement types;
2. exact four public static method signatures/no overloads;
3. Opportunity-bearing token has exactly public get-only ProductionState and no public ctor/setter/declared method;
4. Postcommit token has exactly public get-only ProductionState + Commit and no public ctor/setter/declared method;
5. final result has exactly OpportunityBearingState + OpportunityEvent + DirectorEvaluation and no public ctor/setter/declared method;
6. exception sealed/catchable/no public ctor;
7. no forbidden provider/network/filesystem/async/thread/time/random/Windows/hardware/persistence type in new public signatures;
8. parameter types enforce phase legality.

### Initialize / aggregate closure

9. exact Missing Raft genesis initializes H=0/O=1 synchronized token;
10. non-genesis/invalid initialization fails;
11. internal validated construction rejects representative mixed otherwise-valid Production/history components without a public rebind API.

### Context

12. genesis ComposeContext produces exact historical Production-bound v2 oracle;
13. evolved token produces exact v3 oracle;
14. repeated composition is exact/deterministic and preserves AccessEvaluation;
15. no historical v1 live Context is emitted.

### Commit phase / first adoption

16. exact first live Accepted Take produces frozen postcommit StateHash `a7e6e1d5...`;
17. postcommit Production has no current Opportunity;
18. accepted history advances exactly once and ends at committed Performance;
19. retained source Opportunity history does not advance;
20. phase one exposes no Opportunity event/result;
21. Rejected/Alternate Take fails before causal successor;
22. representative structured/rendered source-Context tamper fails through inherited history-aware binder;
23. representative CommitId/materialization failure produces no postcommit successor;
24. zero-mutation Accepted Take still records Performance;
25. all-durable-consequence-Rejected Accepted Take still records Performance;
26. internal postcommit construction rejects representative mixed commit/state/history/context/routing components.

### Opportunity phase / second adoption

27. first phase-two transition selects MARLOWE and preserves frozen Opportunity StateHash `e935dc6...`;
28. accepted history does not append in phase two; only its state anchor advances;
29. Opportunity history advances exactly once;
30. final O=H+1 holds;
31. next ComposeContext produces exact MARLOWE v3 oracle;
32. multi-turn `VOSS -> MARLOWE -> WREN -> VOSS` remains exact;
33. repeated identical Performance / Character recurrence / self-history remain exact;
34. final-success construction exposes no independently adoptable raw Opportunity Production/history pair;
35. closed-construction and immutable/pure-transition tests prove an unsuccessful phase-two operation cannot mutate/erase an already-returned postcommit successor; do not add fault injection or private-state corruption merely to force such failure.

### Equivalence / determinism / composition

36. wrapper-driven first cycle is canonically equivalent to independent Patch 0015 primitive-chain oracle at each adoption boundary;
37. identical repeated pure calls over the same immutable inputs derive equivalent canonical identities;
38. culture change cannot alter existing identities/order;
39. exact structural delegation law holds;
40. no Advancement replay/event/hash/version/canonicalizer exists.

### Historical/native regression

41. complete existing Core suite remains green;
42. all Patch 0015 reference-oracle values remain exact;
43. native Windows ARM64 full Core tests run on implementation head;
44. native Windows ARM64 Harness build runs because referenced Core changed;
45. Missing Raft and generic-smoke Harness fixtures execute successfully on the same executable tree.

Static analysis cannot satisfy 43–45.

---

## 27. Explicit non-scope

Patch 0016 does not add:

- provider/model invocation;
- request/attempt/result provenance;
- partial streaming;
- refusal/timeout/cancellation/retry/backoff/spend/cost policy;
- automatic Candidate generation;
- automatic Integrity/Interpreter/State Authority generation;
- Take decision/review UX;
- Rejected/Alternate run policy;
- full Scene/run loop or run termination/budgets;
- E0 run evidence/transcript package;
- persistence/recovery/event store/cross-Scene history;
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
 -> future provider attempt / Candidate / semantic pipeline
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

### 0.1–0.2 — superseded one-shot exploration

Attempted one all-or-nothing Accepted-Take-to-next-Opportunity result; added source-state/history preflight and Full Ensemble v2/v3 source law.

### 0.3 — two-boundary Synchronized Causal Advancement

Reconciled the parallel Advancement/Causal Cycle explorations and corrected the material authority error:

- restored both Patch 0015 adoption boundaries;
- introduced closed immutable phase tokens;
- renamed the Core capability to Advancement rather than Orchestration;
- preserved exact supplied source Context through both phases;
- prevented raw final Opportunity result from becoming an alternate adoption path;
- reused lower semantic owners and bounded aggregate validation;
- added type-state/delegation/native regression gates.

### 0.4 — phase-two testability correction

- retained Proposal 0.3 two-boundary causal semantics and current authoritative `main`/workflow provenance;
- removed the requirement to forge an impossible corrupted valid postcommit token solely to manufacture a phase-two runtime failure;
- made explicit that current supported valid postcommit state is intended to make phase two deterministic/total, so any lower failure is an integrity/system defect rather than fictional behavior;
- preserved the failure/adoption law through closed-construction invariant tests, structural delegation, fixed exception normalization and immutable-token tests;
- consolidated repetitive exploratory prose without changing public surface or lower authority.

Audit restarts from correctness at Proposal 0.4.

---

## 30. Recursive audit closure criterion

Implementation remains forbidden until one fresh complete pass over Proposal 0.4 or later finds:

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
