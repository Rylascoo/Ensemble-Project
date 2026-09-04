# H1 Patch 0016 — Deterministic Accepted Take Advancement

Status: **Blueprint Proposal 0.9 — EXPLORATORY; recursive adversarial audit restarted from correctness; implementation forbidden**

Date: 2026-09-04

Authoritative parent `main`: `5186b0ab624165ab9872630592b164bf3764273d`

Parent executable authority: `H1 Patch 0015 — E0 Accepted Performance History + Context Continuity`

Program authority: `Kymaean Architecture & Ship Plan — approved Proposal 0.7`

Architecture branch: `h1-patch-0016-accepted-take-advancement-blueprint`

---

## 1. Purpose

Patch 0016 closes the smallest deterministic causal-adoption seam still visible after Patch 0015:

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

Patch 0016 makes that approved sequence one canonical **Core causal-advancement boundary** without introducing provider execution, provider-attempt semantics, persistence, a new causal event/hash, Application-layer orchestration, or a full E0 run driver.

Patch 0016 is an intermediate H1 seam. It does not by itself declare H1 complete or trigger the mandatory end-of-H1 convergence audit; later attempt/run-driver architecture remains before that checkpoint.

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

Approved Ship Plan Phase A calls for remaining deterministic seams, including ownership of synchronized Production/history/Opportunity across a live cycle. Its target product architecture reserves **capability-neutral Scene/run orchestration** for the future Application layer while Core owns fictional and causal authority.

Patch 0016 therefore uses the Core term **Advancement**, not Core Orchestration. It owns one causal-authority composition; it does not own a use-case/run state machine.

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

## 5. Highest deterministic causal-advancement layer

Add:

```text
Ensemble.E0.Core.Advancement
```

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

No lower subsystem may depend upward on `Advancement`.

The future Application layer may call Advancement as one Core authority while owning provider-attempt sequencing, Scene/run state machines, cancellation boundaries, and use-case coordination.

No provider SDK, filesystem, network, clock, random, Task, thread, cancellation token, Windows API, GPU/NPU/QNN/ONNX, persistence, or package dependency enters Core.

---

## 6. Exact public surface

Proposal 0.9 adds exactly:

```csharp
namespace Ensemble.E0.Core.Advancement;

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

## 9. Result immutability/minimality

`E0AcceptedTakeAdvancementResult` has no public constructor/setter and exactly:

```text
CausalCommit
AcceptedPerformanceHistory
Opportunity
```

Each is necessary for the next layer:

- `CausalCommit` preserves the effective Take, materializations, and canonical causal event required by provenance/replay/persistence work;
- `AcceptedPerformanceHistory` is the opaque synchronized token needed for the next Full Ensemble Context;
- `Opportunity` carries the final Production state, effective routing history, canonical Opportunity event, and Director evaluation.

It exposes no intermediate postcommit State/history, duplicate final State/History/Event fields, source Context/Access evaluation, provider data, cost/retry data, raw output, or hidden reasoning.

---

## 10. Identity/materialization inputs

Patch 0016 allocates no TakeId/CommitId/RecordId/RunId/AttemptId/request identity/timestamp. Caller supplies source Context, Accepted Take, CommitId, and `E0RecordMaterializationSet`; lower authorities retain validation. No clock/random identity enters Core.

---

## 11. Pure ordered composition: no new domain validator

`Advance(...)` owns **ordering, stage-specific failure normalization, and final result exposure only**.

It adds no convenience/null/disposition validation when an existing canonical lower boundary already owns the invariant:

- `ProductionStateCheckpoint.Capture` validates current state/current Opportunity;
- `BindWithAcceptedHistory` validates source Context, Accepted Take disposition/association, accepted history, source-state snapshot, and Full Ensemble v2/v3 shape;
- `Commit` validates CommitId/materializations/effective identities;
- `RecordCommit` validates accepted-history causal advancement;
- `Establish` validates Opportunity source history + Director transition;
- `RecordOpportunity` validates cross-history count/coupling + canonical Opportunity replay.

Exact sequence:

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

Each expected lower exception is caught narrowly at the stage that calls it, never by a broad catch-all:

```text
Capture / Bind / Commit
 -> E0CausalCommitException

RecordCommit / RecordOpportunity
 -> E0AcceptedPerformanceHistoryException

Establish
 -> E0OpportunityTransitionException
```

Each becomes `E0AcceptedTakeAdvancementException` with a fixed stage-only message and original exception as `InnerException`.

Acceptable categories include source-checkpoint failure, source-Take-binding failure, causal-commit failure, accepted-history advancement failure, Opportunity-establishment failure, and accepted-history/Opportunity-coupling failure.

No top-level message copies Candidate VisibleText, Context prose, record/mutation prose, provider output, credentials/secrets, or arbitrary untrusted text. Unexpected programming/runtime failures are not relabeled.

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

## 19. Test-only primitive oracle vs production advancement

`Patch0015TestSupport.RunTurn(...)` remains an explicit test-only primitive-chain oracle because lower Patch 0015 tests intentionally inspect staged intermediates.

Future production/harness Accepted-Take advancement uses Patch 0016 Advancement. No second production implementation is permitted. Remove/narrow helper paths that cease serving lower-stage tests.

---

## 20. Structural composition regression

Because the new capability exists to own **composition**, implementation must structurally prove `DeterministicAcceptedTakeAdvancement.Advance` calls exactly once:

```text
ProductionStateCheckpoint.Capture
E0TakeStateBinding.BindWithAcceptedHistory
DeterministicCausalCommit.Commit
E0AcceptedPerformanceHistoryContinuity.RecordCommit
DeterministicOpportunityAuthority.Establish
E0AcceptedPerformanceHistoryContinuity.RecordOpportunity
```

and directly calls none of:

```text
CharacterBoundedAccessControl
DeterministicContextComposer
E0ProductionContextContinuity.Compose/ComposeWithAcceptedHistory
DeterministicCausalCommit.Replay
DeterministicOpportunityAuthority.Replay
```

Inspect exception clauses to prove no `catch (Exception)`/catch-all normalization.

The repository already has IL call inspection in `Patch0012StructuralImplementationTests`. If reuse would duplicate that parser, extract the smallest test-only shared IL helper and adapt Patch 0012 tests without weakening assertions. No production abstraction is created.

---

## 21. Expected implementation surface

Preferred production source:

```text
src/Ensemble.E0.Core/Advancement/AcceptedTakeAdvancement.cs
```

Focused tests:

```text
tests/Ensemble.E0.Core.Tests/Advancement/AcceptedTakeAdvancementTests.cs
tests/Ensemble.E0.Core.Tests/Advancement/Patch0016ContractAuditTests.cs
```

Optional test-only IL helper extraction as above. Narrow inherited exact-public-surface adaptations only if legitimately required.

No semantic change expected in Domain, Access, Context, Performer, Director, Integrity, StateInterpreter, StateAuthority, Take, Production, CausalCommit, Opportunity, Continuity, Fixture, or Harness. If a lower semantic algorithm must change, reopen architecture.

---

## 22. Required test matrix

The wrapper tests prove the new composition contract; existing lower tests remain authority for lower algorithms. Do not mirror the entire Patch 0015 matrix through the wrapper.

### Surface/composition
1. Advancement namespace exactly three approved public types.
2. `Advance` exactly seven approved parameters/result type and no overload.
3. result exactly `CausalCommit`, `AcceptedPerformanceHistory`, `Opportunity`; no public ctor/setter.
4. no forbidden provider/platform/persistence/async dependency and no event/hash/version/allocator/replay surface.
5. exact structural call ownership/order set from section 20 and no catch-all.

### Live success
6. exact Patch 0015 v2-genesis oracle advances through wrapper with unchanged canonical identities.
7. high-level output equals primitive-chain oracle for the same inputs, including DirectorEvaluation.
8. evolved v3 source advances a second turn and returns synchronized final Production/accepted-history/Opportunity-history outputs.
9. zero-mutation Accepted Take still advances historical Performance + Opportunity.
10. all-durable-consequence-Rejected Accepted Take still advances historical Performance + Opportunity.
11. multi-turn recurrence/repeated Performance behavior remains unchanged through at least one wrapper-driven sequence.

### Representative propagated failures
12. historical v1 source Context fails through the inherited history-aware binder; lower v1 compatibility tests remain unchanged.
13. one rendered/structured source-Context tamper representative fails through the wrapper; Patch 0015 remains authority for the exhaustive source-Context tamper matrix.
14. one non-Accepted Take representative fails through the binder and produces no wrapper.
15. stale/foreign Opportunity history fails through `Establish` with no wrapper.
16. one Commit-stage identity/materialization failure fails with no wrapper.
17. source immutable objects remain unchanged on failure.
18. wrapper message is stage-only and does not echo untrusted Performance/Context/record/mutation prose.

Do not forge impossible private state, add DI seams, or duplicate lower validators merely to exercise every internal branch.

### Determinism/compatibility
19. identical repeated calls over same immutable inputs are equivalent.
20. culture change does not alter canonical identities/order.
21. historical Context v1/v2 and Patch 0015 v3 oracle values remain unchanged.
22. no Advancement Replay method; lower replay remains canonical.

---

## 23. Complexity / ARM64

Patch 0016 composes existing synchronous deterministic work. New production overhead is wrapper construction + narrow stage error translation only. No background work, idle wake, async/thread scheduling, network, filesystem, provider call, GPU/NPU, Windows API, cache, or polling. No measured performance/battery/NPU claim.

---

## 24. Explicit non-scope

No provider request/SDK/model assignment; no attempt/request/result provenance; no partial-stream storage; no retry/backoff/spend/cost/cancellation/timeout taxonomy; no RunId/AttemptId allocation; no Application/Scene/run loop; no repeated-attempt policy; no E0-A provider adapter; no transcript/blind-review package; no durable persistence/recovery/cross-Scene replay; no general Observation/CharacterClaim promotion; no World Resolver; no branching/Rehearsal/Another Take UX; no WinUI/Windows AI/Windows ML/QNN/NPU; no App Actions/MCP; no MSIX/IPackageValidator/WACK/Store.

---

## 25. What this enables next

```text
source Context + Accepted Take
 -> DeterministicAcceptedTakeAdvancement.Advance(...)
     -> CausalCommit
     -> synchronized accepted history
     -> canonical Opportunity result + Director evidence
```

The later provider-attempt/Application-run layer can remain technical until Candidate/Take exists and cannot own causal advancement. Patch 0016 does not yet close H1.

---

## 26. Correction history

### 0.1 -> 0.2
Restored source Context as required evidence; internal recomposition would weaken Patch 0015 structured+rendered source proof.

### 0.2 -> 0.3
Reused `E0OpportunityTransitionResult` so Director evidence survives without duplicate wrapper properties.

### 0.3 -> 0.4
Removed duplicate Opportunity-history precheck/final replay; existing Opportunity/Continuity authorities own those invariants.

### 0.4 -> 0.5
Added Domain dependency, explicit Full Ensemble v2-genesis/v3-evolved gate, historical-v1 compatibility distinction, and clarified Patch 0016 is not end-of-H1 convergence.

### 0.5 -> 0.6
Added structural composition regression and optional minimal test-only IL helper extraction.

### 0.6 -> 0.7
Removed redundant top-layer domain/null/disposition validation. Patch 0016 owns only ordered composition, narrow stage error normalization, and final result exposure.

### 0.7 -> 0.8
Rebased onto current documentation-only ship-plan checkpoint `5186b0ab624165ab9872630592b164bf3764273d`; architecture provenance only.

### 0.8 -> 0.9
Renamed the Core top layer from `Orchestration` to `Advancement` so the approved Ship Plan can reserve capability-neutral Scene/run orchestration for the future Application layer. Also reduced wrapper tests to composition-specific coverage instead of redundantly rerunning the full Patch 0015 lower-layer matrix.

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