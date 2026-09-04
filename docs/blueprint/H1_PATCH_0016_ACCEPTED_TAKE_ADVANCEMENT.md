# H1 Patch 0016 — Deterministic Accepted Take Advancement

Status: **Blueprint Proposal 0.7 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden**

Date: 2026-09-04

Authoritative parent `main`: `a2b1458509e595847360aaa0729e90a99eb98063`

Parent executable authority: `H1 Patch 0015 — E0 Accepted Performance History + Context Continuity`

Program authority: `Kymaean Architecture & Ship Plan — approved Proposal 0.7`

Architecture branch: `h1-patch-0016-accepted-take-advancement-blueprint`

---

## 1. Purpose

Patch 0016 closes the smallest deterministic composition seam still visible after Patch 0015:

```text
opportunity-bearing Production state
+ synchronized Accepted Performance history
+ synchronized Opportunity history
+ Full Ensemble source ContextPacket carried through the Take pipeline
+ already-bound Accepted E0 Take
+ caller-supplied CommitId
+ caller-supplied RecordId materializations
    -> deterministic Accepted-Take advancement
        -> full source Context/Take reproof
        -> exact causal commit
        -> exact Accepted Performance history advancement
        -> exact deterministic Opportunity establishment
        -> exact Opportunity-history coupling
        -> next synchronized opportunity-bearing result
```

Patch 0015 proved every lower authority but intentionally left their live adoption composition in test support. `Patch0015TestSupport.RunTurn(...)` currently performs the primitive chain manually.

Patch 0016 makes that approved sequence one canonical **production deterministic advancement boundary** without introducing provider execution, provider-attempt semantics, persistence, a new causal event, a new hash, or a full E0 run driver.

Patch 0016 is an intermediate H1 seam. It does **not** by itself declare the H1 deterministic spine complete or trigger the mandatory end-of-H1 convergence audit; later attempt/run-driver architecture remains before that checkpoint.

---

## 2. Why this comes before provider attempt/provenance

Frozen Blueprint 0.1 distinguishes:

```text
provider attempt -> may fail technically with no CandidatePerformance
CandidatePerformance -> provisional semantic output
Integrity / Interpretation / State Authority -> Take-bindable package
Accepted E0Take -> selected for later atomic causal commit
successful causal commit -> effective historical Performance + approved consequences
```

Provider failure/refusal/timeout/retry/cancellation, malformed output, and partial streamed output must never become fiction.

The current Core proves the semantic path from valid Candidate through Accepted Take and all lower deterministic authorities. Before a run driver consumes technical provider attempts, it needs one canonical target for what happens after an Accepted Take exists.

Provider-attempt architecture first would have no canonical state-advancement target. Full-run architecture first would conflate technical outcomes with fictional authority.

Patch 0016 closes Accepted-Take advancement first. Later provider-attempt/run-driver work calls it only after an attempt produces an Accepted Take.

---

## 3. Frozen authority basis

Blueprint 0.1 requires Character != Performer; Access before Context; technical failures cannot become fiction; partial/cancelled/unaccepted output cannot enter Production history; Integrity -> Interpreter -> deterministic State Authority; Accepted Take remains provisional until commit; accepted Performance + approved consequences form one atomic causal commit; Director manages opportunity only; rejected/alternate attempts do not advance effective Opportunity history; E0 provenance preserves Context, Director inputs/decisions, validation outcomes, and technical attempts; E0 remains a behavioral harness rather than product/platform work.

Patch 0012 freezes `ProductionStateCheckpoint`, `E0TakeStateBinding`, exact Accepted-Take/state proof, `DeterministicCausalCommit.Commit/Replay`, caller-supplied CommitId/materializations, and no Core allocator.

Patch 0013 freezes `E0OpportunityHistory`, `DeterministicOpportunityAuthority.Establish/Replay`, and effective routing only after successful causal commit.

Patch 0014 freezes Production-backed deterministic Access + Context continuity.

Patch 0015 freezes:

- opaque synchronized `E0AcceptedPerformanceHistory`;
- `ComposeWithAcceptedHistory` as Full Ensemble source-Context composition;
- `BindWithAcceptedHistory` as Full Ensemble precommit proof;
- supplied source Context compared against fresh state/history recomposition including structured/rendered bytes;
- empty accepted history valid only at exact genesis using Context v2;
- nonempty evolved history requires Context v3/render-v2;
- historical v1 is separate compatibility only through historical `Bind(...)`;
- staged commit not adopted until `RecordCommit(...)` succeeds;
- staged Opportunity not adopted until `RecordOpportunity(...)` succeeds;
- `RecordOpportunity(...)` owns cross-history count/coupling + canonical Opportunity replay;
- exact v2 -> commit -> Opportunity -> v3 reference lineage.

Approved Ship Plan Phase A calls for deterministic run/turn orchestration while provider SDK execution remains outside Core.

---

## 4. Current source seam

Patch 0015 test support currently performs:

```text
Capture(state)
 -> source Context already composed for Candidate/Take
 -> BindWithAcceptedHistory(source Context, ...)
 -> Commit(...)
 -> RecordCommit(...)
 -> Establish(... same source Context ...)
 -> RecordOpportunity(...)
 -> caller manually adopts final outputs
```

Every primitive is authoritative, but no production Core API owns the post-Take composition.

Future callers must not be free to adopt postcommit Production early, establish/adopt Opportunity before history coupling, route from a different Context than the one proved at commit, choose different failure/adoption ordering, or duplicate the sequence across harness/provider paths.

---

## 5. Highest deterministic E0 layer

Add `Ensemble.E0.Core.Orchestration`.

It may depend downward on existing public boundaries in:

```text
Domain
Production
Context
Take
CausalCommit
Continuity
Opportunity
```

No lower subsystem may depend upward on Orchestration.

This layer is deterministic semantic advancement only, not provider/process/UI orchestration.

No provider SDK, filesystem, network, clock, random, Task, thread, cancellation token, Windows API, GPU/NPU/QNN/ONNX, persistence, or package dependency enters Core.

---

## 6. Exact public surface

Proposal 0.7 adds exactly:

```csharp
namespace Ensemble.E0.Core.Orchestration;

public static class DeterministicAcceptedTakeAdvancement
{
    public static E0AcceptedTakeAdvancementResult Advance(
        ProductionState currentState,
        E0AcceptedPerformanceHistory acceptedPerformanceHistory,
        E0OpportunityHistory opportunityHistory,
        ContextPacket sourceContext,
        E0Take acceptedTake,
        CommitId commitId,
        E0RecordMaterializationSet materializations);
}

public sealed class E0AcceptedTakeAdvancementResult
{
    public E0CausalCommit CausalCommit { get; }
    public E0AcceptedPerformanceHistory AcceptedPerformanceHistory { get; }
    public E0OpportunityTransitionResult Opportunity { get; }
}

public sealed class E0AcceptedTakeAdvancementException : Exception
{
    // public/catchable type; no public constructor
}
```

No additional public type/enum/interface/delegate/event/record/builder/allocator/replay/status/failure/provider/version surface.

`E0OpportunityTransitionResult` already carries Event, final State, final Opportunity History, and DirectorEvaluation. Reusing it only after coupling succeeds preserves E0 Director evidence without duplicate wrapper properties.

Authoritative events remain `E0CausalCommit` and `E0OpportunityTransition`; no turn event/hash/version is created.

---

## 7. Source Context remains required evidence

Candidate/Take retain Context identity but not the complete original structured/rendered source packet. Patch 0015 made `BindWithAcceptedHistory(...)` the full proof that the supplied source Context artifact equals fresh deterministic state/history composition, including rendered bytes.

Patch 0016 therefore requires `sourceContext` and passes that same canonical semantic artifact to both `BindWithAcceptedHistory` and `DeterministicOpportunityAuthority.Establish`.

Managed reference identity is not constitutional; canonical semantic/rendered equality and association are.

Provider-attempt provenance later must prove actual provider request/framing/disclosure. Patch 0016 does not claim that role.

---

## 8. Full Ensemble Context-version gate

Inherited unchanged:

```text
exact genesis + initialized empty accepted history
 -> Context v2 / production-bound / render-v1

synchronized evolved state + nonempty accepted history
 -> Context v3 / production-bound.accepted-history / render-v2
```

Historical Context v1 remains compatible only in lower historical APIs/tests and is not accepted by Patch 0016 live advancement. Evolved v2 and genesis v3 fail through existing history-aware binding. Patch 0016 adds no version logic.

---

## 9. Result immutability

`E0AcceptedTakeAdvancementResult` has no public constructor/setter and exactly:

```text
CausalCommit
AcceptedPerformanceHistory
Opportunity
```

It exposes no intermediate postcommit State/history, duplicate final State/History/Event fields, source Context/Access evaluation, provider data, cost/retry data, raw output, or hidden reasoning.

Final state/history/Director evidence remain available through `Opportunity`; accepted Performance history and causal event remain direct properties.

---

## 10. Identity/materialization inputs

Patch 0016 allocates no TakeId/CommitId/RecordId/RunId/AttemptId/request identity/timestamp. Caller supplies source Context, Accepted Take, CommitId, and `E0RecordMaterializationSet`; lower authorities retain validation. No clock/random identity enters Core.

---

## 11. Pure ordered composition: no new domain validator

`Advance(...)` owns **ordering, stage-specific failure normalization, and final result exposure only**.

It does not add even convenience/null/disposition validation when an existing canonical lower boundary already owns that invariant. In particular:

- `ProductionStateCheckpoint.Capture` validates the current state/current Opportunity;
- `BindWithAcceptedHistory` validates source Context, Accepted Take disposition/association, accepted history, source-state snapshot, and Full Ensemble v2/v3 Context shape;
- `Commit` validates CommitId/materializations/current commit identities;
- `RecordCommit` validates accepted-history causal advancement;
- `Establish` validates Opportunity source history + Director transition;
- `RecordOpportunity` validates cross-history count/coupling + canonical Opportunity replay.

The exact method sequence is:

```text
A. checkpoint = ProductionStateCheckpoint.Capture(currentState)

B. binding = E0TakeStateBinding.BindWithAcceptedHistory(
       checkpoint,
       sourceContext,
       acceptedTake,
       acceptedPerformanceHistory)

C. commitResult = DeterministicCausalCommit.Commit(
       commitId,
       currentState,
       binding,
       materializations)

D. historyAfterCommit = E0AcceptedPerformanceHistoryContinuity.RecordCommit(
       acceptedPerformanceHistory,
       currentState,
       commitResult.Commit)

E. opportunityResult = DeterministicOpportunityAuthority.Establish(
       commitResult.ResultState,
       commitResult.Commit,
       sourceContext,
       opportunityHistory)

F. historyAfterOpportunity = E0AcceptedPerformanceHistoryContinuity.RecordOpportunity(
       historyAfterCommit,
       commitResult.ResultState,
       commitResult.Commit,
       opportunityHistory,
       opportunityResult.Event)

G. construct E0AcceptedTakeAdvancementResult only after F succeeds
```

No Patch 0016 Access/Context recomposition, null/disposition/domain validator, Opportunity-history validator, replay, canonicalizer, or final synchronization algorithm is added.

The no-opportunity postcommit state and history-after-commit token remain method-local staged values.

---

## 12. Stage-specific failure normalization

The new abstraction is allowed to translate expected lower exceptions because callers of the high-level advancement should not need to know which internal composed boundary failed.

Each stage is caught **narrowly at that stage**, never by one broad catch-all:

```text
Capture / Bind / Commit
    E0CausalCommitException

RecordCommit / RecordOpportunity
    E0AcceptedPerformanceHistoryException

Establish
    E0OpportunityTransitionException
```

Each becomes `E0AcceptedTakeAdvancementException` with a fixed stage-only message and original exception as `InnerException`.

Examples of acceptable top-level message categories:

```text
source checkpoint failed
source Take binding failed
causal commit failed
accepted Performance history advancement failed
Opportunity establishment failed
accepted-history/Opportunity coupling failed
```

The exact strings may be frozen during implementation only if tests need stable developer diagnostics; they are not a user-facing product contract.

No top-level message may copy Candidate VisibleText, Context prose, record/mutation prose, provider output, credentials/secrets, or arbitrary untrusted text.

Unexpected programming/runtime failures are not relabeled.

---

## 13. Why staged failure is safe

A stale/foreign Opportunity history may be discovered after a causal commit has been computed in memory. This is safe because Core objects are immutable, no global/persistent state changes, and no staged result is exposed before full success.

A duplicate earlier validator would create authority drift solely to fail sooner. E0 prefers authority clarity over redundant optimization.

---

## 14. All-or-nothing result exposure != durable transactionality

A result is exposed only if source proof, causal commit, accepted-history advancement, Opportunity establishment, and cross-history coupling all succeed.

Failure leaves source immutable objects unchanged.

This is not crash consistency, event-store atomicity, persistence, or recovery. Constitutional causal atomicity remains `Accepted Take + approved consequences -> E0CausalCommit`; Opportunity remains a separate event.

---

## 15. Rejected/Alternate/technical paths

`Advance` handles Accepted Takes only because `BindWithAcceptedHistory` requires Accepted disposition.

Provider failure/refusal/timeout/cancellation/partial/malformed output, Integrity Reject/RequestAnotherTake, Rejected Take, and Alternate Take stay outside this API. Supplying a non-Accepted Take fails at the canonical binder, is normalized as a source-binding failure, and produces no result/no no-op event.

Source Opportunity remains effective outside this API.

---

## 16. No replay / duplicate event authority

Patch 0016 adds no Replay API. Existing `DeterministicCausalCommit.Replay`, `DeterministicOpportunityAuthority.Replay`, `RecordCommit`, and `RecordOpportunity` remain canonical. Future persistence composes them under separate architecture.

---

## 17. Reference oracle unchanged

No canonical bytes/hash inputs change. Patch 0015 first live inputs must preserve:

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
CausalCommit.ResultStateHash
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c
Opportunity selected Character
MARLOWE
result.Opportunity.State.StateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151
```

Next Context from final state + accepted history remains:

```text
ContextPacketId
CTX:ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
RenderedContextHash
668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16
```

Changed existing oracle identity reopens architecture unless a lower defect is separately proven.

---

## 18. Determinism

Identical valid immutable inputs produce equivalent causal event, Opportunity result/event, final Production state, accepted history, Opportunity history, and Director evaluation. No clock/random/global counter/filesystem/environment/culture-sensitive ordering/provider/network/mutable singleton influences result.

---

## 19. Test-only primitive oracle vs production composition

`Patch0015TestSupport.RunTurn(...)` remains an explicit test-only primitive-chain oracle because lower Patch 0015 tests intentionally inspect staged intermediates.

Future production/harness Accepted-Take advancement uses Patch 0016 Orchestration. No second production implementation is permitted. Remove/narrow helper paths that cease serving lower-stage tests.

---

## 20. Structural composition regression

Because the new capability is specifically **composition ownership**, implementation must include a structural regression over `DeterministicAcceptedTakeAdvancement.Advance` proving exactly one call to each canonical boundary:

```text
ProductionStateCheckpoint.Capture
E0TakeStateBinding.BindWithAcceptedHistory
DeterministicCausalCommit.Commit
E0AcceptedPerformanceHistoryContinuity.RecordCommit
DeterministicOpportunityAuthority.Establish
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
```

and zero direct calls to:

```text
CharacterBoundedAccessControl
DeterministicContextComposer
E0ProductionContextContinuity.Compose/ComposeWithAcceptedHistory
DeterministicCausalCommit.Replay
DeterministicOpportunityAuthority.Replay
```

The test should also inspect exception clauses sufficiently to prove there is no `catch (Exception)`/catch-all normalization on `Advance`.

The repository already has IL call inspection in `Patch0012StructuralImplementationTests`. If reuse would otherwise duplicate that parser, implementation may extract the smallest **test-only** shared IL inspection helper and adapt Patch 0012 tests without weakening their assertions. The second concrete use earns that test abstraction; no production abstraction is created.

---

## 21. Expected implementation surface

Preferred production source:

```text
src/Ensemble.E0.Core/Orchestration/AcceptedTakeAdvancement.cs
```

Focused tests:

```text
tests/Ensemble.E0.Core.Tests/Orchestration/AcceptedTakeAdvancementTests.cs
tests/Ensemble.E0.Core.Tests/Orchestration/Patch0016ContractAuditTests.cs
```

Optional test-only shared IL helper extraction only as described above. Narrow inherited exact-reflection/public-surface adaptations only if legitimately required.

No semantic change expected in Domain, Access, Context, Performer, Director, Integrity, StateInterpreter, StateAuthority, Take, Production, CausalCommit, Opportunity, Continuity, Fixture, or Harness. If a lower semantic algorithm must change, reopen architecture.

---

## 22. Required test matrix

### Surface/dependency
1. Orchestration namespace exactly three approved public types.
2. `Advance` exactly seven approved parameters/result type and no overload.
3. source Context required.
4. result exactly `CausalCommit`, `AcceptedPerformanceHistory`, `Opportunity`.
5. result/exception no public constructors/setters; static class only `Advance`.
6. no persistence/provider/network/clock/random/Task/thread/cancellation/Windows/GPU/NPU/QNN/ONNX public dependency.
7. no event/hash/version/allocator/replay surface.
8. structural sequence/call ownership test from section 20 passes.
9. exception clauses contain no catch-all and only expected lower exception types.

### Live Context/source proof
10. exact v2 succeeds at exact empty-history genesis.
11. historical v1 rejected by Patch 0016 while lower historical APIs unchanged.
12. exact v3 succeeds evolved.
13. evolved v2 and genesis v3 fail.
14. stale Context fails.
15. structured tamper fails.
16. rendered tamper fails.
17. matching Candidate ContextPacketId cannot bypass mismatched source artifact.

### Successful reference behavior
18. Patch 0015 first live oracle exact identities unchanged.
19. high-level result equals primitive-chain oracle for same inputs.
20. `Opportunity.DirectorEvaluation` equals primitive-chain evaluation.
21. zero-mutation Accepted Take advances historical Performance + Opportunity.
22. all-durable-consequence-Rejected Accepted Take still advances historical Performance + Opportunity.
23. multi-turn v3 sequence remains synchronized/deterministic.
24. repeated identical Performances + Character recurrence preserved.

### Existing lower failures through composition
25. null/current-state failure normalizes from Capture rather than Patch 0016 validator.
26. null/stale accepted-history/source Context/non-Accepted Take failures normalize from Bind rather than Patch 0016 validator.
27. stale/foreign Opportunity-history fails with no final wrapper.
28. duplicate CommitId fails closed.
29. duplicate effective TakeId fails closed.
30. missing required RecordId materialization fails closed.
31. source immutable objects unchanged after failure.
32. errors do not echo untrusted Performance/Context/record/mutation prose.

Do not duplicate lower validators, add dependency-injection seams, or forge impossible private state merely to force every internal stage. Existing lower tests remain authority for independently injectable lower failures.

### Determinism/compatibility
33. identical repeated calls equivalent.
34. culture change does not alter identity/order.
35. historical Context v1/v2 + Patch 0015 v3 canonical values unchanged.
36. no advancement Replay; lower replay remains canonical.

---

## 23. Complexity / ARM64

Patch 0016 composes existing synchronous deterministic work. New production overhead is wrapper construction + narrow stage error translation only. No background work, idle wake, async/thread scheduling, network, filesystem, provider call, GPU/NPU, Windows API, cache, or polling. No measured performance/battery/NPU claim.

---

## 24. Explicit non-scope

No provider request/SDK/model assignment; no attempt/request/result provenance; no partial-stream storage; no retry/backoff/spend/cost/cancellation/timeout taxonomy; no RunId/AttemptId allocation; no full Scene/run loop; no repeated-attempt policy; no E0-A provider adapter; no transcript/blind-review package; no durable persistence/recovery/cross-Scene replay; no general Observation/CharacterClaim promotion; no World Resolver; no branching/Rehearsal/Another Take UX; no WinUI/Windows AI/Windows ML/QNN/NPU; no App Actions/MCP; no MSIX/IPackageValidator/WACK/Store.

---

## 25. What this enables next

```text
source Context + Accepted Take
 -> DeterministicAcceptedTakeAdvancement.Advance(...)
     -> CausalCommit
     -> synchronized accepted history
     -> canonical Opportunity result + Director evidence
```

Later provider-attempt architecture can remain technical until Candidate/Take exists and cannot own causal advancement. Patch 0016 does not yet close H1.

---

## 26. Correction history

### 0.1 -> 0.2
Restored source Context as required evidence; internal recomposition would weaken Patch 0015 structured+rendered source proof.

### 0.2 -> 0.3
Reused `E0OpportunityTransitionResult` so Director evidence survives without duplicate wrapper properties.

### 0.3 -> 0.4
Removed duplicate Opportunity-history precheck/final replay; existing Opportunity/Continuity authorities own those invariants.

### 0.4 -> 0.5
Added Domain dependency, explicit Full Ensemble v2-genesis/v3-evolved live gate, historical-v1 compatibility distinction, and clarified Patch 0016 is not end-of-H1 convergence.

### 0.5 -> 0.6
Added structural composition regression and allowed minimal test-only IL helper extraction only if needed.

### 0.6 -> 0.7
Removed all Patch 0016 domain/null/disposition validation that existing lower authorities already own. Patch 0016 now owns only ordered composition, stage-specific narrow failure normalization, and final result exposure. Structural tests now also guard the Capture call and absence of catch-all relabeling.

---

## 27. Recursive adversarial audit gate

Before approval, one complete fresh pass must find:

```text
0 material correctness corrections
0 consistency corrections
0 authority corrections
0 source Context proof corrections
0 Director/provenance corrections
0 atomicity/adoption corrections
0 synchronization corrections
0 dependency-direction corrections
0 canonical/replay corrections
0 disclosure/privacy corrections
0 failure-behavior corrections
0 scope corrections
0 worthwhile test improvements
0 worthwhile simplifications
0 hygiene corrections
0 ARM64 corrections
0 Blueprint/ship-plan conflicts
0 evidence corrections
```

Any correction restarts the pass.

No implementation branch, approval evidence, handoff, source change, or `CURRENT_STATE.md` promotion before explicit Director approval of the final audited proposal.