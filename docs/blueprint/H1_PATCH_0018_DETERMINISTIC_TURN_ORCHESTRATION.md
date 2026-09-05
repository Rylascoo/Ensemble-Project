# H1 Patch 0018 — Deterministic Turn Orchestration

Status: **BLUEPRINT PROPOSAL 0.6 — RECURSIVELY AUDITED; DIRECTOR APPROVAL REQUIRED; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-05
Parent `main`: `0548078a060267e136968c2bf97b384356b50554`
Latest executable authority: H1 Patch 0017 — Provider-Neutral Performer Attempt Boundary Proposal 0.2.

## 1. Purpose and falsification

Patch 0018 is unnecessary if Core already has one closed deterministic state machine joining current Cycle state to Patch 0017 attempt semantics, Integrity, Interpreter, State Authority, Take, and accepted commit while preserving Patch 0016's postcommit boundary.

It does not; those authorities exist individually but are still stitched manually.

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

Later Opportunity establishment remains the existing Patch 0016 operation. No provider execution/request, retry, spend, streaming, persistence, Scene termination, or repeated run loop enters this patch.

## 2. Ordering and adoption laws

Patch 0018 exposes no one-call commit+Opportunity operation. Accepted commit is authoritative before later Opportunity establishment; phase-two failure must never hide the valid postcommit token.

Patch 0011 freezes:

```text
Integrity Accept + Interpretation + terminal State Authority
 -> Take-bindable semantic package
 -> E0Take
```

So ReviewRequired has no Take/TakeId. `TakeId` enters only when binding a terminal TakeBindable package.

Other preserved laws:

- Character != Performer; Access/Context precede Performer use.
- technical failure/cancellation never becomes Character behavior;
- Integrity `RequestAnotherTake` creates no Take;
- Patch 0017 + fresh Context gating makes Patch 0008 deterministic Reject codes unreachable; seeing one is invariant failure;
- State Authority mutation decisions do not determine Take disposition;
- review choices are proposal/policy-specific;
- reference E0 accepts every Take-bindable package; Rejected/Alternate stay separately labeled deviations;
- only Accepted Take crosses causal commit;
- external Integrity/Interpreter technical failure causes no later semantic call and no fiction.

## 3. Exact public surface

Namespace `Ensemble.E0.Core.Turn` exports exactly:

```csharp
public enum E0TurnProgressDisposition
public sealed class E0TurnProgress
public static class DeterministicE0TurnOrchestrator
public sealed class E0TurnOrchestrationException : Exception
```

Exact disposition values:

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

Default/undefined invalid. These are orchestration progress, never Production truth or Take disposition.

`E0TurnProgress` public read-only properties exactly:

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

No public constructor/setter/declared instance method. Internally retain exact source `E0OpportunityBearingCycleState`; review/take-bindable states retain exact State Authority policy. Proposal becomes public once it exists because creator review must inspect the actual semantic proposal, not only its hash/projection.

Closed-state shape:

```text
TechnicalFailure/Cancelled: Candidate and later fields null
CandidateReady: Candidate != null; later fields null
RequestAnotherTake: Candidate != null; Integrity=RequestAnotherTake; later null
ReadyForInterpretation: Candidate != null; Integrity=Accept; InterpretationSource exact; later null
AuthorityReviewRequired: + Proposal != null; Authority.Status=ReviewRequired; Take null
TakeBindable: + Proposal != null; Authority.Status=Complete; Take null
AcceptedTakeReady: Take != null/Accepted; Candidate=Take.Performance;
                   Proposal=Take.InterpretationProposal;
                   Authority=Take.AuthorityEvaluation (fresh Take replay)
```

Every nonpublic factory proves its complete invariant at construction; no weaker enum/payload factory.

## 4. Exact operations

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

## 5. `GateAttempt`

Recompose exact current Context with `DeterministicE0CausalCycle.ComposeContext(source)`; never accept caller Context.

Require the attempt's ContextPacketId to equal the fresh Context and replay Patch 0017's canonical public binder:

```text
CandidateReady -> BindCandidate(freshContext, exact Candidate)
TechnicalFailure/Cancelled -> BindTechnicalOutcome(freshContext, exact disposition), Candidate must be null
```

Undefined/forged/stale shapes fail closed. Map the three Patch 0017 dispositions exactly. CandidateReady retains the exact Candidate reference. No Integrity work occurs here so an external advisory path remains possible.

## 6. `EvaluateIntegrity`

Valid only from CandidateReady.

Build fresh `IntegrityCandidateInput`. Patch 0008's only deterministic Reject codes are subject/context mismatch and ContextPacket identity mismatch; Patch 0017 + GateAttempt already prove both, so reject-code count must be zero or this stage fails.

`concernKinds` must be initialized/non-default; empty is valid. Bind through `IntegrityConcernEvidence.Bind`, then `DeterministicIntegrityValidator.Validate`.

```text
RequestAnotherTake -> RequestAnotherTake
Accept -> ReadyForInterpretation + exact StateInterpretationSource.Bind(...)
```

Unexpected Reject fails closed. Patch 0018 does not authenticate advisory-source provenance.

## 7. Interpreter boundary

From ReadyForInterpretation, external orchestration may execute its configured Interpreter and call:

```text
StateInterpretationContract.ParseJson(progress.InterpretationSource, rawBytes)
```

Patch 0018 receives only the semantic proposal. Interpreter refusal/error/timeout/cancellation means no `EvaluateAuthority` call and no fiction. Raw/authenticated provenance stays outside Core Turn.

## 8. `EvaluateAuthority`

Valid only from ReadyForInterpretation. Require non-null proposal/policy and initialized review choices (empty valid):

```text
fresh StateAuthoritySnapshot from retained source Production
 -> StateAuthorityInput.Bind(snapshot, retained InterpretationSource, proposal)
 -> StateAuthorityReviewSet.Bind(input, reviewChoices)
 -> DeterministicStateAuthority.Evaluate(input, policy, reviewSet)
```

Existing State Authority validates canonical policy/review shape and proposal identity. Retain exact proposal + policy.

```text
ReviewRequired -> AuthorityReviewRequired
Complete       -> TakeBindable
```

Both remain Take-free.

## 9. `ResolveAuthorityReview`

Valid only from AuthorityReviewRequired.

Accept a **complete replacement review-choice set** for the same retained proposal/policy. Reuse exact retained proposal, policy, source Cycle, Context, Candidate, Integrity and InterpretationSource; rebuild fresh snapshot/input/review set and reevaluate.

Return AuthorityReviewRequired if unresolved or TakeBindable if Complete. No proposal/policy substitution and no hidden incremental review merge.

## 10. `BindAcceptedTake`

Valid only from TakeBindable. This is the first operation to receive TakeId.

Apply only the approved reference policy:

```text
E0Take.Bind(
  takeId,
  retained Context,
  retained Candidate,
  retained Integrity,
  retained Proposal,
  retained Complete Authority,
  E0TakeDisposition.Accepted)
```

`E0Take.Bind` replays State Authority and stores its fresh canonical evaluation. Therefore AcceptedTakeReady must project from the returned Take:

```text
Candidate              = Take.Performance
InterpretationProposal = Take.InterpretationProposal
AuthorityEvaluation    = Take.AuthorityEvaluation
AcceptedTake           = Take
```

This prevents parallel pre-Take vs replayed-authority views. No Rejected/Alternate selector. Rejected mutations may reduce consequences but never reject the reference Take.

## 11. `CommitAccepted`

Valid only from AcceptedTakeReady. Delegate exactly to existing `DeterministicE0CausalCycle.CommitAcceptedTake` with retained source Cycle/Context/Take/materializations and return its `E0PostCommitCycleState` unchanged.

Duplicate no commit/binding/history/hash logic and do not establish Opportunity. Successful return is causal adoption; caller later invokes existing `EstablishOpportunity` explicitly.

## 12. Failure/privacy

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

## 13. Purity / implementation surface

All methods are synchronous deterministic transforms over explicit immutable semantic inputs. No clock/random/network/filesystem/environment/task/thread/`CancellationToken`/provider SDK/background work/ID allocation/persistence/retry/spend.

Before CommitAccepted, progress has zero Production mutation authority. TechnicalFailure, Cancelled, RequestAnotherTake, AuthorityReviewRequired, TakeBindable and AcceptedTakeReady do not consume Opportunity or enter accepted history.

Add only:

```text
src/Ensemble.E0.Core/Turn/E0TurnProgress.cs
src/Ensemble.E0.Core/Turn/DeterministicE0TurnOrchestrator.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnContractTests.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnOrchestrationTests.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnDeterminismTests.cs
```

Existing Core/Harness edits expected: zero. Physical placement is E0 semantic coordination only; it does not freeze the post-E0 Application assembly topology. Any required lower semantic change reopens architecture.

## 14. Gating tests

Prove:

- exact four types, eight enum values/properties, six method signatures, closed construction;
- GateAttempt recomposes Context, replays Patch 0017, rejects stale/forged Candidate and technical results;
- technical/cancelled mutate nothing; CandidateReady retains exact Candidate;
- lawful Candidate has zero deterministic Integrity rejects; impossible contrary path fails sanitized;
- default concerns fail; empty -> Accept; concerns -> RequestAnotherTake with no Take/state effect;
- Accept creates exact InterpretationSource;
- authority proposal/policy/review mismatch fails closed;
- ReviewRequired exposes exact Proposal/no Take; resolution cannot change proposal/policy and uses replacement review set;
- Complete authority -> TakeBindable with no TakeId/Take;
- only BindAcceptedTake receives TakeId, produces reference Accepted Take, and AcceptedTakeReady projects the Take's replayed authority;
- rejected mutation does not reject Take; equivalent inputs reproduce semantics;
- CommitAccepted rejects every non-AcceptedTakeReady state;
- accepted commit reproduces Patch 0016 hashes/history, returns postcommit without Opportunity; later explicit Opportunity reproduces successor and cannot erase postcommit on failure;
- public surface has no provider/model/network/task/cancellation/persistence/retry/spend/RunId/AttemptId contract;
- fixed messages leak no semantic/provider payload;
- all 598 inherited Core tests remain intact before observing new total.

Reflection only for public-surface assertions or impossible construction plumbing.

## 15. Non-scope / audit result

No provider/model execution/request framing; raw Performer/Integrity/Interpreter output; streaming; cancellation primitive; retry/backoff; spend/budget; provider/model/version/settings/metrics; AttemptId/RunId; authenticated provenance; secret storage; automatic reviewer/Interpreter; Rejected/Alternate reference Take selection; Scene termination; repeated run loop; persistence; branch/concurrency arbitration; cross-Scene continuity; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Recursive audit corrections through Proposal 0.6: preserved Patch 0016 postcommit adoption; recomposed/replayed stale-sensitive boundaries; kept probabilistic execution outside Core; removed unreachable Integrity Reject state; bound review to one proposal/policy; exposed proposal for creator review; aligned terminal State Authority -> TakeBindable -> TakeId/Take with Patch 0011; and projected AcceptedTakeReady from E0Take's fresh replayed Authority.

Final convergence pass:

```text
0 material correctness corrections outstanding
0 authority contradictions
0 stale-token gaps within Patch 0018 scope
0 Character/Performer or technical/fiction conflations
0 review-package identity gaps
0 premature Take/TakeId states
0 causal-adoption collapses
0 privacy/dependency violations
0 provider/platform dependencies
0 worthwhile in-scope public-surface simplifications
0 blueprint-cap violations
```

**Director approval is required before implementation.**
