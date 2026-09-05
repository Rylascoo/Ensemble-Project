# H1 Patch 0018 — Deterministic Turn Orchestration

Status: **BLUEPRINT PROPOSAL 0.5 — RECURSIVE AUDIT IN PROGRESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-05
Parent `main`: `0548078a060267e136968c2bf97b384356b50554`
Latest executable authority: H1 Patch 0017 — Provider-Neutral Performer Attempt Boundary Proposal 0.2.

## 1. Falsification / scope

This patch is unnecessary if Core already has one closed deterministic state machine joining current Cycle state to Patch 0017 attempt semantics, Integrity, Interpreter, State Authority, Take, and accepted commit without collapsing Patch 0016's postcommit boundary.

It does not. Those authorities exist individually; tests/Harness still stitch them manually.

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
 -> ReviewRequired [same proposal/policy] | TakeBindable
 -> reference Accepted Take
 -> accepted commit
 -> VALID POSTCOMMIT STATE
```

Opportunity establishment remains explicit after the returned postcommit state. No provider execution/request, retry, spend, streaming, persistence, Scene termination, or Opportunity establishment enters this patch.

## 2. Adoption / ordering laws

Do not add one `ExecuteTurn` that commits and then establishes Opportunity. Patch 0016 makes accepted commit authoritative before Opportunity establishment; later failure must not hide the valid postcommit token.

Do not allocate/bind a Take before State Authority is terminal. Patch 0011 freezes:

```text
Integrity Accept + Interpretation + terminal State Authority
 -> Take-bindable semantic package
 -> E0Take
```

Therefore ReviewRequired carries no Take and no reserved TakeId. `TakeId` enters only when binding a terminal TakeBindable package.

## 3. Preserved laws

- Character != Performer; Access/Context precede Performer use.
- Technical failure/cancellation never becomes Character behavior.
- Integrity `RequestAnotherTake` creates no Take.
- Patch 0017 + fresh Context gating makes Patch 0008 deterministic Reject codes unreachable; seeing one is invariant failure.
- State Authority mutation decisions never determine Take disposition.
- Review choices are proposal/policy-specific and cannot drift to another package.
- Reference E0 accepts every Take-bindable package; Rejected/Alternate remain separately labeled deviations.
- Only Accepted Take crosses causal commit.
- `ReviewRequired` is normal non-Take progress.
- External Integrity/Interpreter provider failure causes no later semantic call and no fiction.

## 4. Exact public surface

Namespace `Ensemble.E0.Core.Turn` exports exactly:

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
TakeBindable            = 7,
AcceptedTakeReady       = 8
```

Default/undefined invalid. These are orchestration progress only, never Production truth or Take disposition.

### `E0TurnProgress`

Exact public read-only properties:

```text
Disposition            : E0TurnProgressDisposition
SourceContext          : ContextPacket
Candidate              : CandidatePerformance?
IntegrityEvaluation    : IntegrityValidationEvaluation?
InterpretationSource   : StateInterpretationSource?
InterpretationProposal : StateInterpretationProposal?
AuthorityEvaluation    : StateAuthorityEvaluation?
AcceptedTake           : E0Take?
```

No public constructor/setter/declared instance method. Internally retain exact source `E0OpportunityBearingCycleState`; review/take-bindable states retain the exact State Authority policy. The proposal is public once it exists because creator review must be able to inspect the actual semantic proposal, not only its authority hash/projection.

Closed-state invariants:

```text
TechnicalFailure/Cancelled:
  Candidate and all later fields null
CandidateReady:
  Candidate != null; later fields null
RequestAnotherTake:
  Candidate != null; Integrity=RequestAnotherTake; later fields null
ReadyForInterpretation:
  Candidate != null; Integrity=Accept; InterpretationSource exact; proposal/later null
AuthorityReviewRequired:
  above + proposal != null + Authority.Status=ReviewRequired; Take null
TakeBindable:
  above + proposal != null + Authority.Status=Complete; Take null
AcceptedTakeReady:
  TakeBindable invariants + AcceptedTake.Disposition=Accepted
```

Every nonpublic factory must prove its complete invariant; no preselected enum/payload bypass factory.

## 5. Exact operations

Exactly six public static methods:

```csharp
E0TurnProgress GateAttempt(
    E0OpportunityBearingCycleState source,
    E0PerformerAttemptResult attemptResult);

E0TurnProgress EvaluateIntegrity(
    E0TurnProgress source,
    ImmutableArray<IntegrityConcernKind> concernKinds);

E0TurnProgress EvaluateAuthority(
    E0TurnProgress source,
    StateInterpretationProposal proposal,
    StateAuthorityPolicy policy,
    ImmutableArray<StateAuthorityReviewChoice> reviewChoices);

E0TurnProgress ResolveAuthorityReview(
    E0TurnProgress source,
    ImmutableArray<StateAuthorityReviewChoice> reviewChoices);

E0TurnProgress BindAcceptedTake(
    TakeId takeId,
    E0TurnProgress source);

E0PostCommitCycleState CommitAccepted(
    CommitId commitId,
    E0TurnProgress source,
    E0RecordMaterializationSet materializations);
```

No async/task/cancellation/provider overload.

## 6. `GateAttempt`

Recompose exact current Context with `DeterministicE0CausalCycle.ComposeContext(source)`; never accept caller Context.

Require `attemptResult.SourceContextPacketId == freshContext.ContextPacketId`, then **replay Patch 0017's canonical public binder** rather than trusting fields alone:

- CandidateReady: `BindCandidate(freshContext, exact Candidate)`;
- TechnicalFailure/Cancelled: `BindTechnicalOutcome(freshContext, exact disposition)` and require null Candidate.

Any mismatch/undefined/forged shape fails closed. This rejects stale Candidate and stale technical results alike.

Map Patch 0017 disposition exactly and retain exact Candidate reference for CandidateReady. No Integrity work occurs here, preserving the external advisory boundary.

## 7. `EvaluateIntegrity`

Valid only from `CandidateReady`.

Build fresh `IntegrityCandidateInput` from retained Context + Candidate. Patch 0008 has only subject/context mismatch and ContextPacket identity mismatch deterministic Reject codes; Patch 0017 + GateAttempt already prove both, so reject-code count must be zero or the stage fails.

`concernKinds` must be initialized/non-default; empty is valid. Bind through `IntegrityConcernEvidence.Bind`, then `DeterministicIntegrityValidator.Validate`.

Expected results:

```text
RequestAnotherTake -> RequestAnotherTake
Accept             -> ReadyForInterpretation
```

Unexpected Reject fails closed. On Accept, immediately retain exact `StateInterpretationSource.Bind(Context, Candidate, Integrity)`. Patch 0018 does not authenticate advisory-source provenance.

## 8. Interpreter boundary

After `ReadyForInterpretation`, external orchestration may execute its configured Interpreter and use:

```text
StateInterpretationContract.ParseJson(progress.InterpretationSource, rawBytes)
```

Patch 0018 receives only that semantic proposal. Interpreter refusal/error/timeout/cancellation causes no `EvaluateAuthority` call and no fiction. Raw/authenticated provenance stays outside Core Turn.

## 9. `EvaluateAuthority`

Valid only from `ReadyForInterpretation`.

Require non-null proposal/policy and initialized review choices (empty valid), then:

```text
fresh StateAuthoritySnapshot from retained source Production
 -> StateAuthorityInput.Bind(snapshot, retained InterpretationSource, proposal)
 -> StateAuthorityReviewSet.Bind(input, reviewChoices)
 -> DeterministicStateAuthority.Evaluate(input, policy, reviewSet)
```

Existing State Authority validates canonical policy/review shape and proposal identity. Retain exact proposal + policy.

Map:

```text
ReviewRequired -> AuthorityReviewRequired
Complete       -> TakeBindable
```

Both have `AcceptedTake=null`.

## 10. `ResolveAuthorityReview`

Valid only from `AuthorityReviewRequired`.

Accept a **complete replacement review-choice set** for the same retained proposal/policy. Reuse exact retained proposal, policy, source Cycle, Context, Candidate, Integrity, and InterpretationSource; rebuild fresh snapshot/input/review set and reevaluate State Authority.

Return `AuthorityReviewRequired` if unresolved or `TakeBindable` if Complete. No caller can substitute proposal/policy during review. Replacing the review set is explicit; no hidden incremental merge semantics.

## 11. `BindAcceptedTake`

Valid only from `TakeBindable`.

This is the first Patch 0018 operation to receive `TakeId`. Apply only the approved reference policy:

```text
E0Take.Bind(
  takeId,
  retained Context,
  retained Candidate,
  retained Integrity,
  retained proposal,
  retained Complete Authority,
  E0TakeDisposition.Accepted)
```

Return `AcceptedTakeReady`, retaining the exact returned Take. No Rejected/Alternate selector. Rejected State Authority mutations may reduce consequences but never reject the Performance/Take.

## 12. `CommitAccepted`

Valid only from `AcceptedTakeReady`.

Delegate exactly to `DeterministicE0CausalCycle.CommitAcceptedTake(commitId, retainedSourceCycle, retainedSourceContext, retainedAcceptedTake, materializations)` and return its `E0PostCommitCycleState` unchanged.

Duplicate no commit/binding/history/hash logic and do not establish Opportunity. Successful return is causal adoption; caller then explicitly invokes existing `EstablishOpportunity`.

## 13. Failure/privacy

Exact public messages:

```text
E0 turn attempt gate failed.
E0 turn Integrity evaluation failed.
E0 turn State Authority evaluation failed.
E0 turn State Authority review resolution failed.
E0 turn accepted Take binding failed.
E0 turn accepted commit failed.
```

`E0TurnOrchestrationException` is public/catchable, sealed, no public constructor. Retain no lower inner exception. No failure representation may expose Context prose, Candidate text/control, proposal/review payload, provider/model/raw output, credentials, or arbitrary untrusted values.

No blanket `catch (Exception)`; convert only known lower semantic exceptions at their stage boundary.

## 14. Purity / authority

All methods are synchronous deterministic transforms over explicit immutable semantic inputs.

No clock/random/network/filesystem/environment/task/thread/`CancellationToken`/provider SDK/background work/ID allocation/persistence/retry/spend.

Before `CommitAccepted`, progress tokens have zero Production mutation authority. TechnicalFailure, Cancelled, RequestAnotherTake, AuthorityReviewRequired, TakeBindable, and AcceptedTakeReady do not consume Opportunity or enter accepted history. Only successful commit crosses causal adoption.

## 15. Implementation surface / tests

Add only:

```text
src/Ensemble.E0.Core/Turn/E0TurnProgress.cs
src/Ensemble.E0.Core/Turn/DeterministicE0TurnOrchestrator.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnContractTests.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnOrchestrationTests.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnDeterminismTests.cs
```

Expected existing Core/Harness edits: zero. Any required lower semantic change reopens architecture.

Gating tests must prove:

- exact four types, eight enum values, eight progress properties, six method signatures, closed construction;
- GateAttempt recomposes Context, replays Patch 0017 binder, rejects stale/forged Candidate and technical results;
- technical/cancelled mutate nothing; CandidateReady retains exact Candidate;
- lawful gated Candidate has zero deterministic Integrity Reject codes; impossible contrary path fails sanitized;
- default concerns fail; empty -> Accept; concerns -> RequestAnotherTake with no Take/state effect;
- Accept creates exact InterpretationSource;
- authority proposal/policy/review association fails closed when stale/mismatched;
- ReviewRequired exposes exact proposal and no Take;
- review resolution cannot change proposal/policy and uses complete replacement review set;
- Complete authority yields TakeBindable with no TakeId/Take;
- only BindAcceptedTake receives TakeId and creates reference Accepted Take; rejected mutation does not reject Take;
- equivalent inputs reproduce progress/Take semantics;
- CommitAccepted rejects every non-AcceptedTakeReady state;
- accepted commit reproduces Patch 0016 hashes/history and returns postcommit without Opportunity establishment;
- explicit later Opportunity establishment reproduces Patch 0016 successor; failure cannot erase returned postcommit;
- public surface has no provider/model/network/task/cancellation/persistence/retry/spend/RunId/AttemptId contract;
- fixed messages leak no semantic/provider payload;
- all 598 inherited Core tests remain intact before observing new total.

Reflection only for public-surface assertions or impossible construction plumbing, per visibility-based testing discipline.

## 16. Non-scope / audit status

No provider/model execution/request framing; raw Performer/Integrity/Interpreter output; streaming; cancellation primitive; retry/backoff; spend/budget; provider/model/version/settings/metrics; AttemptId/RunId; authenticated provenance; secret storage; automatic reviewer/Interpreter; Rejected/Alternate reference Take selection; Scene termination; repeated run loop; persistence; branch/concurrency arbitration; cross-Scene continuity; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Corrections through Proposal 0.5:

1. preserved Patch 0016 postcommit adoption instead of commit+Opportunity wrapper;
2. recomposes Context and replays Patch 0017 binding rather than trusting stale/forged tokens;
3. preserves external Integrity/Interpreter/provider boundaries;
4. removed unreachable IntegrityRejected state/null concern branch;
5. keeps ReviewRequired proposal/policy-specific with explicit same-package resolution;
6. exposes full proposal for intelligible creator review;
7. aligns Patch 0011 ordering: terminal authority -> TakeBindable -> first TakeId/Accepted Take;
8. exposes only approved reference Accepted Take policy;
9. remains under the project blueprint size cap.

Remaining audit order:
`correctness -> authority -> stale-token closure -> privacy -> dependency -> surface minimization -> tests -> simplicity -> hygiene -> ARM64 -> five-property fit -> evidence`.

**Implementation remains forbidden pending clean recursive closure and explicit Director approval.**
