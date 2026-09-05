# H1 Patch 0018 — Deterministic Turn Orchestration

Status: **BLUEPRINT PROPOSAL 0.4 — RECURSIVE AUDIT IN PROGRESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-05
Parent `main`: `0548078a060267e136968c2bf97b384356b50554`
Latest executable authority: H1 Patch 0017 — Provider-Neutral Performer Attempt Boundary Proposal 0.2.

## 1. Falsification and purpose

This patch is unnecessary if Core already has one closed deterministic turn-state machine joining current Cycle state to Patch 0017 attempt semantics, Integrity, Interpreter, State Authority, Take, and accepted commit while preserving Patch 0016's postcommit boundary.

It does not. Those authorities exist individually, but tests/Harness still stitch them manually.

Patch 0018 closes only:

```text
Opportunity-bearing state
 -> fresh exact Context
 -> Patch0017 attempt
 -> CandidateReady | TechnicalFailure | Cancelled
 -> Integrity
 -> RequestAnotherTake | ReadyForInterpretation
 -> supplied StateInterpretationProposal
 -> State Authority
 -> ReviewRequired [resolve against same proposal/policy] | reference Accepted Take
 -> accepted commit
 -> VALID POSTCOMMIT STATE
```

Opportunity establishment stays explicit:

```text
postcommit -> DeterministicE0CausalCycle.EstablishOpportunity -> next state
```

No provider execution/request, retry, spend, streaming, persistence, Scene termination, or Opportunity establishment enters this patch.

## 2. Adoption law

Do not add one `ExecuteTurn` that commits and then establishes Opportunity.

Patch 0016 makes accepted commit authoritative before Opportunity establishment. A wrapper could otherwise commit, fail in phase two, and hide the valid postcommit token. Patch 0018 ends at `E0PostCommitCycleState`; later Opportunity failure cannot erase that returned state.

## 3. Preserved laws

- Character != Performer; Access/Context precede Performer use.
- Technical failure/cancellation never becomes Character behavior.
- Integrity `RequestAnotherTake` creates no Take.
- After Patch 0017 + fresh Context gating, Patch 0008 deterministic Reject codes are unreachable; seeing one is invariant failure, not normal progress.
- State Authority mutation decisions do not determine Take disposition.
- State Authority review is proposal/policy-specific; review choices may never drift onto a different package.
- Reference E0 accepts every Take-bindable package; Rejected/Alternate remain separately labeled deviations.
- Only Accepted Take crosses causal commit.
- State Authority `ReviewRequired` is a normal non-Take result.
- External Integrity/Interpreter provider failure causes no later semantic-stage call and no fiction.

## 4. Exact public surface

Namespace:

```text
Ensemble.E0.Core.Turn
```

Exactly four public types:

```csharp
public enum E0TurnProgressDisposition
public sealed class E0TurnProgress
public static class DeterministicE0TurnOrchestrator
public sealed class E0TurnOrchestrationException : Exception
```

Disposition values exactly:

```csharp
TechnicalFailure        = 1,
Cancelled               = 2,
CandidateReady          = 3,
RequestAnotherTake      = 4,
ReadyForInterpretation  = 5,
AuthorityReviewRequired = 6,
AcceptedTakeReady       = 7
```

Default/undefined values invalid. These are orchestration states only, never Production truth or Take disposition.

### `E0TurnProgress`

Exact public read-only properties:

```text
Disposition          : E0TurnProgressDisposition
SourceContext        : ContextPacket
Candidate            : CandidatePerformance?
IntegrityEvaluation  : IntegrityValidationEvaluation?
InterpretationSource : StateInterpretationSource?
AuthorityEvaluation  : StateAuthorityEvaluation?
AcceptedTake         : E0Take?
```

No public constructor/setter/declared instance method. Internally retain exact source `E0OpportunityBearingCycleState`. Review/accepted-ready states additionally retain the original TakeId, proposal, and State Authority policy needed to preserve review identity; these are not additional public properties because the public Authority evaluation/Accepted Take already expose the appropriate semantic evidence.

Closed-state invariants:

```text
TechnicalFailure/Cancelled:
  Candidate, Integrity, InterpretationSource, Authority, Take = null
CandidateReady:
  Candidate != null; later fields null
RequestAnotherTake:
  Candidate != null; Integrity=RequestAnotherTake; later fields null
ReadyForInterpretation:
  Candidate != null; Integrity=Accept; InterpretationSource exact; Authority/Take null
AuthorityReviewRequired:
  above + Authority.Status=ReviewRequired; Take null; exact TakeId/proposal/policy retained
AcceptedTakeReady:
  above + Authority.Status=Complete; Take.Disposition=Accepted
```

Every nonpublic factory proves its complete invariant at construction; no preselected enum/payload bypass factory.

## 5. Exact operations

Exactly five public static methods:

```csharp
E0TurnProgress GateAttempt(
    E0OpportunityBearingCycleState source,
    E0PerformerAttemptResult attemptResult);

E0TurnProgress EvaluateIntegrity(
    E0TurnProgress source,
    ImmutableArray<IntegrityConcernKind> concernKinds);

E0TurnProgress PrepareTake(
    TakeId takeId,
    E0TurnProgress source,
    StateInterpretationProposal proposal,
    StateAuthorityPolicy policy,
    ImmutableArray<StateAuthorityReviewChoice> reviewChoices);

E0TurnProgress ResolveAuthorityReview(
    E0TurnProgress source,
    ImmutableArray<StateAuthorityReviewChoice> reviewChoices);

E0PostCommitCycleState CommitAccepted(
    CommitId commitId,
    E0TurnProgress source,
    E0RecordMaterializationSet materializations);
```

No async/task/cancellation/provider overload.

## 6. `GateAttempt`

Recompose exact current Context with existing `DeterministicE0CausalCycle.ComposeContext(source)`. Never accept caller Context.

Fail unless source/attempt are valid and:

```text
attemptResult.SourceContextPacketId == freshContext.ContextPacketId
```

This rejects stale Candidate and stale technical results alike.

Map Patch 0017 exactly:

```text
TechnicalFailure -> TechnicalFailure
Cancelled        -> Cancelled
CandidateReady   -> CandidateReady
```

CandidateReady retains the exact Patch 0017 Candidate reference. No Integrity work occurs here, preserving the external advisory boundary.

## 7. `EvaluateIntegrity`

Valid only from `CandidateReady`.

Build fresh `IntegrityCandidateInput` from retained Context + Candidate. Patch 0008 has only subject/context mismatch and ContextPacket identity mismatch deterministic Reject codes. Patch 0017 + GateAttempt already prove both associations, so fresh reject-code count must be zero; otherwise fail the stage.

`concernKinds` must be initialized/non-default. Empty is valid. Bind through existing `IntegrityConcernEvidence.Bind` and call `DeterministicIntegrityValidator.Validate`.

Patch 0018 does not authenticate the advisory source; later operational provenance must.

Expected dispositions:

```text
RequestAnotherTake -> RequestAnotherTake
Accept             -> ReadyForInterpretation
```

Unexpected Reject fails closed.

On Accept, immediately create/retain exact `StateInterpretationSource.Bind(Context, Candidate, Integrity)`. RequestAnotherTake creates no Take and mutates nothing.

## 8. Interpreter boundary

After `ReadyForInterpretation`, external orchestration may execute its configured Interpreter and use:

```text
StateInterpretationContract.ParseJson(progress.InterpretationSource, rawBytes)
```

Patch 0018 receives only the semantic proposal. Interpreter refusal/error/timeout/cancellation causes no `PrepareTake` call and no fiction. Raw output/authenticated attempt provenance remain outside Core Turn.

## 9. `PrepareTake`

Valid **only** from `ReadyForInterpretation`.

Require initialized TakeId, non-null proposal/policy, initialized review choices (empty valid). Retain the exact TakeId/proposal/policy and evaluate:

```text
fresh StateAuthoritySnapshot from retained source Production
 -> StateAuthorityInput.Bind(snapshot, retained InterpretationSource, proposal)
 -> StateAuthorityReviewSet.Bind(input, reviewChoices)
 -> DeterministicStateAuthority.Evaluate(input, policy, reviewSet)
```

If `ReviewRequired`, return `AuthorityReviewRequired` with exact fresh evaluation, no Take, and retained original TakeId/proposal/policy.

If `Complete`, apply only approved reference policy:

```text
E0Take.Bind(..., E0TakeDisposition.Accepted)
```

and return `AcceptedTakeReady`.

No Rejected/Alternate selector. Mutation rejection may reduce committed consequences but never rejects the Performance/Take.

## 10. `ResolveAuthorityReview`

Valid only from `AuthorityReviewRequired`.

It accepts **only new review choices**. It must reuse the internally retained exact TakeId, `StateInterpretationProposal`, `StateAuthorityPolicy`, source Cycle, Context, Candidate, Integrity, and InterpretationSource from the original `PrepareTake` call.

It rebuilds a fresh snapshot/input for the same retained proposal, binds the new review set, and reevaluates existing State Authority.

If still `ReviewRequired`, return a new closed `AuthorityReviewRequired` token retaining the same TakeId/proposal/policy. If `Complete`, bind the same retained package as reference `Accepted` Take and return `AcceptedTakeReady`.

No caller can substitute a new proposal/policy while resolving review. This prevents review-choice drift across mutation packages.

## 11. `CommitAccepted`

Valid only from closed `AcceptedTakeReady`.

Delegate exactly:

```text
DeterministicE0CausalCycle.CommitAcceptedTake(
  commitId,
  retainedSourceCycle,
  retainedSourceContext,
  retainedAcceptedTake,
  materializations)
```

Return `E0PostCommitCycleState` unchanged. Duplicate no commit/binding/history/hash logic. Do not establish Opportunity.

Successful return is causal adoption. Caller then explicitly uses existing `EstablishOpportunity`.

## 12. Failure/privacy

Exact public messages:

```text
E0 turn attempt gate failed.
E0 turn Integrity evaluation failed.
E0 turn Take preparation failed.
E0 turn State Authority review resolution failed.
E0 turn accepted commit failed.
```

`E0TurnOrchestrationException` is public/catchable, sealed, no public constructor. Retain no lower inner exception. No message/ToString path may expose Context prose, Candidate text/control, proposal/review payload, provider/model/raw output, credentials, or arbitrary untrusted values.

No blanket `catch (Exception)`; convert only known lower semantic exceptions at their stage boundary.

## 13. Purity and authority

All methods are synchronous deterministic transforms over explicit immutable semantic inputs.

No clock/random/network/filesystem/environment/task/thread/`CancellationToken`/provider SDK/background work/ID allocation/persistence/retry/spend.

Before `CommitAccepted`, progress tokens have zero Production mutation authority. TechnicalFailure, Cancelled, RequestAnotherTake, and AuthorityReviewRequired do not consume Opportunity or enter accepted history.

## 14. Implementation surface

Add only:

```text
src/Ensemble.E0.Core/Turn/
  E0TurnProgress.cs
  DeterministicE0TurnOrchestrator.cs

tests/Ensemble.E0.Core.Tests/Turn/
  E0TurnContractTests.cs
  E0TurnOrchestrationTests.cs
  E0TurnDeterminismTests.cs
```

Expected existing Core/Harness edits: zero. Any required change to Patch 0017, Integrity, Interpreter, State Authority, Take, Cycle, Opportunity, continuity, canonicalization, framework, or SDK reopens architecture.

## 15. Gating tests

Prove at minimum:

- exact four-type namespace, seven enum values, seven progress properties, five method signatures, closed exception/result construction;
- GateAttempt recomposes Context and rejects stale Candidate/technical results;
- technical/cancelled mutate nothing; CandidateReady retains exact Candidate;
- lawful gated Candidate yields zero deterministic Integrity Reject codes; impossible forged contrary path fails sanitized;
- default concern array fails; empty concerns -> Accept; concerns -> RequestAnotherTake with no Take/state effect;
- Accept creates exact InterpretationSource;
- PrepareTake only accepts ReadyForInterpretation;
- ReviewRequired is typed/Take-free and retains original review package internally;
- ResolveAuthorityReview cannot substitute proposal/policy/TakeId and can remain ReviewRequired or produce AcceptedTakeReady;
- Complete authority yields only Accepted Take; rejected mutation does not reject Take;
- equivalent explicit inputs reproduce progress/Take semantics;
- stale/mismatched proposal/review inputs fail closed;
- CommitAccepted rejects every non-AcceptedTakeReady state;
- accepted commit reproduces Patch 0016 hashes/history and returns postcommit without Opportunity establishment;
- later explicit Opportunity establishment reproduces Patch 0016 successor; failure cannot erase returned postcommit;
- public surface has no provider/model/network/task/cancellation/persistence/retry/spend/RunId/AttemptId contract;
- fixed messages leak no semantic/provider payload;
- all 598 inherited Core tests remain intact before new-test count is observed.

Reflection only for public-surface assertions or impossible construction plumbing, per visibility-based testing discipline.

## 16. Non-scope

No provider/model execution/request framing; raw Performer/Integrity/Interpreter output; streaming; cancellation primitive; retry/backoff; spend/budget; provider/model/version/settings/metrics; AttemptId/RunId; authenticated provenance; secret storage; automatic Integrity reviewer/Interpreter; Rejected/Alternate reference Take selection; Scene termination; repeated run loop; persistence; branch/concurrency arbitration; cross-Scene continuity; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

## 17. Recursive audit status

Corrections through Proposal 0.4:

1. rejected one-call commit+Opportunity because it could hide Patch 0016 postcommit adoption;
2. recomposes Context from Cycle instead of trusting caller Context;
3. preserves external Interpreter/provider boundary;
4. types ReviewRequired as non-Take progress;
5. exposes only approved reference Accepted Take policy;
6. removed unreachable IntegrityRejected state after proving Patch 0017 + GateAttempt close both deterministic Reject conditions;
7. removed nullable concern-evidence branch;
8. compressed blueprint to respect project size discipline;
9. replaced repeat `PrepareTake` from ReviewRequired with `ResolveAuthorityReview`, retaining exact TakeId/proposal/policy so review choices cannot drift onto a different mutation package.

Remaining audit order:

```text
correctness -> authority -> stale-token closure -> Character/Performer separation
-> technical non-fictionalization -> Integrity -> State Authority -> Take policy
-> causal adoption -> privacy -> dependency -> surface minimization -> tests
-> simplicity -> hygiene -> ARM64 -> five-property fit -> evidence
```

**Implementation remains forbidden pending clean recursive closure and explicit Director approval.**
