# H1 Patch 0018 — Deterministic Turn Orchestration

Status: **BLUEPRINT PROPOSAL 0.2 — RECURSIVE AUDIT IN PROGRESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-05
Parent `main`: `0548078a060267e136968c2bf97b384356b50554`
Latest executable authority: H1 Patch 0017 — Provider-Neutral Performer Attempt Boundary Proposal 0.2.

## 1. Falsification

**This patch is unnecessary if current Core already exposes one closed deterministic turn-state machine that binds current synchronized Cycle state to a Patch 0017 attempt, distinguishes non-Take outcomes, advances accepted Candidate semantics through Integrity/Interpreter/State Authority/Take, and commits an Accepted Take without collapsing Patch 0016's postcommit adoption boundary.**

It does not. Current source has every individual deterministic semantic authority plus Patch 0017 fictional ingress, but tests/Harness still stitch them manually.

## 2. Exact purpose

Patch 0018 closes only this deterministic semantic seam:

```text
Opportunity-bearing Cycle state
  -> exact fresh Context
  -> Patch 0017 Performer attempt
  -> CandidateReady OR TechnicalFailure/Cancelled
  -> deterministic Integrity
  -> RequestAnotherTake OR ReadyForInterpretation
  -> supplied semantic StateInterpretationProposal
  -> deterministic State Authority
  -> ReviewRequired OR reference Accepted Take
  -> accepted causal commit
  -> VALID POSTCOMMIT STATE
```

Opportunity establishment remains Patch 0016's explicit second phase:

```text
VALID POSTCOMMIT STATE
  -> DeterministicE0CausalCycle.EstablishOpportunity(...)
  -> next Opportunity-bearing state
```

Patch 0018 executes no provider/model, builds no provider request, and owns no retry, spend, streaming, persistence, Scene termination, or Opportunity establishment.

## 3. Critical adoption law

Patch 0018 must **not** expose one `ExecuteTurn` that commits and then establishes Opportunity.

Patch 0016 intentionally makes accepted causal commit authoritative before later Opportunity establishment. A wrapper that committed and then threw during Opportunity establishment without returning the postcommit token could hide an already-authoritative adopted state.

Patch 0018 therefore ends at `E0PostCommitCycleState`. The caller explicitly invokes the existing Opportunity phase. If that later phase fails, the returned postcommit state remains valid and authoritative.

## 4. Existing laws preserved

1. Character != Performer.
2. Access Control and Context composition precede Performer use.
3. Technical failure/cancellation never becomes Character behavior.
4. Integrity `RequestAnotherTake` produces no `E0Take`.
5. A lawful Patch 0017 Candidate freshly gated to the same current Context cannot produce Patch 0008 deterministic Reject codes; such a result is an invariant failure, not a normal turn state.
6. State Authority consequence decisions do not determine Take disposition.
7. Reference E0 orchestration accepts every Take-bindable package; Rejected/Alternate are explicit labeled deviations outside this path.
8. Only an Accepted Take may enter causal commit.
9. Accepted Performance + approved consequences remain one atomic causal commit.
10. State Authority `ReviewRequired` is a normal non-Take outcome.
11. Provider/runtime failure in later Integrity/Interpreter execution remains outside this semantic orchestrator and cannot create fiction.

## 5. Namespace and exact public surface

New namespace:

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

No other public Patch 0018 type.

### Disposition grammar

Exact values:

```csharp
TechnicalFailure        = 1,
Cancelled               = 2,
CandidateReady          = 3,
RequestAnotherTake      = 4,
ReadyForInterpretation  = 5,
AuthorityReviewRequired = 6,
AcceptedTakeReady       = 7
```

Default `0` and undefined values are invalid.

These are orchestration progress states, not Production facts, Character states, provider failure subcategories, or Take dispositions.

## 6. `E0TurnProgress`

Public read-only properties exactly:

```text
Disposition          : E0TurnProgressDisposition
SourceContext        : ContextPacket
Candidate            : CandidatePerformance?
IntegrityEvaluation  : IntegrityValidationEvaluation?
InterpretationSource : StateInterpretationSource?
AuthorityEvaluation  : StateAuthorityEvaluation?
AcceptedTake         : E0Take?
```

No public constructor, setter, or declared instance method.

Every token internally retains the exact source `E0OpportunityBearingCycleState`; this is not exposed as a new mutable authority.

Construction invariants:

```text
TechnicalFailure / Cancelled
  Candidate=null
  Integrity=null
  InterpretationSource=null
  Authority=null
  AcceptedTake=null

CandidateReady
  Candidate!=null
  later fields null

RequestAnotherTake
  Candidate!=null
  Integrity.Disposition=RequestAnotherTake
  later fields null

ReadyForInterpretation
  Candidate!=null
  Integrity.Disposition=Accept
  InterpretationSource!=null and exactly bound
  Authority=null
  AcceptedTake=null

AuthorityReviewRequired
  Candidate!=null
  Integrity=Accept
  InterpretationSource!=null
  Authority.Status=ReviewRequired
  AcceptedTake=null

AcceptedTakeReady
  Candidate!=null
  Integrity=Accept
  InterpretationSource!=null
  Authority.Status=Complete
  AcceptedTake!=null
  AcceptedTake.Disposition=Accepted
```

Every nonpublic factory must prove its own complete invariant at the actual construction point. No factory may accept a preselected enum/payload combination without revalidation.

## 7. Exact public operations

Exactly four public static methods:

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

E0PostCommitCycleState CommitAccepted(
    CommitId commitId,
    E0TurnProgress source,
    E0RecordMaterializationSet materializations);
```

No async/task/cancellation/provider overload.

## 8. `GateAttempt`

`GateAttempt` recomposes the exact current Context by calling existing `DeterministicE0CausalCycle.ComposeContext(source)`.

It never trusts caller-supplied Context.

It fails closed unless:

- source and attempt are valid;
- `attemptResult.SourceContextPacketId` equals the fresh current `ContextPacketId`.

Thus stale technical failure is rejected exactly as stale Candidate output is rejected.

Mapping:

```text
Patch0017 TechnicalFailure -> Turn TechnicalFailure
Patch0017 Cancelled        -> Turn Cancelled
Patch0017 CandidateReady   -> Turn CandidateReady
```

CandidateReady retains the exact Candidate reference from Patch 0017. Patch 0017 proves semantic Candidate/Context association; GateAttempt additionally proves current-Cycle freshness.

No Integrity work occurs here, preserving an explicit boundary for external configured Integrity advisory execution.

## 9. `EvaluateIntegrity`

Valid only from `CandidateReady`.

It builds a fresh `IntegrityCandidateInput` from the token's exact Context + Candidate.

Patch 0008's only deterministic Reject codes are `SubjectContextMismatch` and `ContextPacketIdentityMismatch`. Patch 0017 plus GateAttempt already prove both associations. Therefore:

```text
freshInput.DeterministicRejectCodes.Length must equal 0
```

Any nonzero result fails the Patch 0018 Integrity stage as an invariant contradiction; it is not represented as a normal progress disposition.

`concernKinds` must be initialized/non-default. Empty is valid and means no semantic concerns. Non-empty values are bound through existing `IntegrityConcernEvidence.Bind` and evaluated by `DeterministicIntegrityValidator.Validate`.

Patch 0018 does not authenticate where advisory concern kinds came from. Later operational provenance must establish configured reviewer/provider identity and raw evidence when applicable.

Mapping:

```text
Integrity RequestAnotherTake -> RequestAnotherTake
Integrity Accept             -> ReadyForInterpretation
```

Any unexpected Integrity Reject fails closed.

On Accept, `StateInterpretationSource.Bind` runs immediately and is retained. That public source is the exact semantic token for later State Interpreter parsing/provider orchestration.

RequestAnotherTake creates no `E0Take` and leaves source Cycle state unchanged.

## 10. State Interpreter boundary

Patch 0018 receives no raw Interpreter bytes and calls no provider.

After `ReadyForInterpretation`, external orchestration may invoke its configured State Interpreter and use existing:

```text
StateInterpretationContract.ParseJson(
    progress.InterpretationSource,
    rawBytes)
```

to create a semantic `StateInterpretationProposal`.

Provider refusal/error/timeout/cancellation at this external stage produces no `PrepareTake` call and therefore no fiction. Exact downstream technical attempt taxonomy/provenance remains outside Patch 0018.

## 11. `PrepareTake`

Valid from `ReadyForInterpretation` or `AuthorityReviewRequired` only. Each call is a fresh deterministic reevaluation over explicitly supplied semantic inputs; a prior ReviewRequired result has no state authority.

Requirements:

- TakeId initialized;
- proposal non-null and compatible with retained `InterpretationSource`;
- policy non-null;
- `reviewChoices` initialized; empty valid;
- fresh `StateAuthoritySnapshot` captured from retained source Cycle Production;
- fresh `StateAuthorityInput.Bind(snapshot, interpretationSource, proposal)` succeeds;
- fresh `StateAuthorityReviewSet.Bind(input, reviewChoices)` succeeds;
- existing `DeterministicStateAuthority.Evaluate` runs.

If evaluation is `ReviewRequired`:

```text
Disposition = AuthorityReviewRequired
AuthorityEvaluation = exact fresh evaluation
AcceptedTake = null
```

This is a normal non-Take result. Caller may inspect the evaluation and reevaluate with explicit review choices.

If evaluation is `Complete`, Patch 0018 applies the approved reference E0 Take policy:

```text
E0Take.Bind(..., E0TakeDisposition.Accepted)
```

and returns `AcceptedTakeReady`.

Patch 0018 exposes no Rejected/Alternate choice. Those remain separately labeled experimental/intervention paths requiring attributable provenance.

State Authority mutation dispositions never silently alter Take disposition.

## 12. `CommitAccepted`

Valid only from fully proven `AcceptedTakeReady`.

It delegates exactly to:

```text
DeterministicE0CausalCycle.CommitAcceptedTake(
    commitId,
    retainedSourceCycle,
    retainedSourceContext,
    retainedAcceptedTake,
    materializations)
```

and returns the `E0PostCommitCycleState` unchanged.

No duplicate commit, binding, history, or StateHash algorithm is introduced. `CommitAccepted` does **not** establish Opportunity.

After successful return, accepted causal history is authoritative. Caller then explicitly invokes existing `DeterministicE0CausalCycle.EstablishOpportunity(postCommit)`.

## 13. Failure/privacy

Exact public failure messages:

```text
E0 turn attempt gate failed.
E0 turn Integrity evaluation failed.
E0 turn Take preparation failed.
E0 turn accepted commit failed.
```

No public Patch 0018 exception retains lower inner exceptions.

Failure representation must not expose Context prose, Candidate text/control, Interpreter proposal prose, review payload, provider/model/raw output, credentials, or arbitrary untrusted values.

No blanket `catch (Exception)`. Known lower semantic exceptions are converted only at their Patch 0018 stage boundary.

## 14. Purity, authority, and adoption

All operations are synchronous deterministic transformations over explicit immutable semantic inputs.

No clock, randomness, network, filesystem, environment state, task/thread, `CancellationToken`, provider SDK, background work, ID allocation, persistence, retry/backoff, or spend logic.

Before `CommitAccepted`, every progress token has zero Production mutation authority.

`TechnicalFailure`, `Cancelled`, `RequestAnotherTake`, and `AuthorityReviewRequired` do not consume Opportunity, mutate Production, or enter accepted history.

Only successful `CommitAccepted` crosses the causal adoption boundary.

## 15. Expected implementation surface

Source additions only:

```text
src/Ensemble.E0.Core/Turn/
  E0TurnProgress.cs
  DeterministicE0TurnOrchestrator.cs
```

Tests:

```text
tests/Ensemble.E0.Core.Tests/Turn/
  E0TurnContractTests.cs
  E0TurnOrchestrationTests.cs
  E0TurnDeterminismTests.cs
```

Expected existing Core/Harness edits: **zero**.

If implementation requires changing Patch 0017, Integrity, Interpreter, State Authority, Take, Cycle, Opportunity, continuity, canonicalization, framework, or SDK semantics, reopen the blueprint.

## 16. Required tests

Gating tests must prove:

- exact four-type Turn namespace surface;
- exact seven non-default progress dispositions;
- progress token closed/immutable with exact seven public properties;
- exact four public orchestrator methods/signatures;
- exception catchable but not publicly constructible;
- GateAttempt recomposes current Context and rejects stale Candidate/technical results;
- technical failure/cancellation retain no Candidate and mutate nothing;
- CandidateReady retains exact Candidate reference;
- lawful gated Candidate produces zero deterministic Integrity Reject codes;
- any impossible forged nonzero Reject-code path fails sanitized rather than creating a progress state;
- RequestAnotherTake creates no Take and leaves Cycle unchanged;
- Integrity Accept creates exact `StateInterpretationSource`;
- default/invalid concern input fails sanitized;
- State Authority ReviewRequired is typed, carries evaluation, creates no Take;
- Complete authority produces only Accepted Take under reference policy;
- mutation rejection does not reject the Take;
- repeated equivalent explicit inputs produce equivalent progress/Take semantics;
- stale/mismatched proposal/review inputs fail closed;
- CommitAccepted rejects every non-AcceptedTakeReady state;
- AcceptedTakeReady commit reproduces inherited Patch 0016 causal hashes/history;
- commit returns postcommit state and does not establish Opportunity;
- explicit later Opportunity establishment reproduces inherited Patch 0016 successor semantics;
- phase-two Opportunity failure can never erase returned postcommit token;
- public surface contains no provider/model/network/task/cancellation/persistence/retry/spend/RunId/AttemptId contract;
- exact sanitized messages leak no semantic/provider payload;
- all 598 inherited Core tests remain intact before new-test count is observed.

Reflection is permitted only for public-surface assertions or impossible construction plumbing, following member-visibility testing discipline.

## 17. Explicit non-scope

No provider/model execution; provider request/framing; raw Performer/Integrity/Interpreter output; streaming; cancellation primitive; retry/backoff; spend/budget; provider/model/version/settings/metrics; AttemptId/RunId; authenticated provenance; secret storage; automatic Integrity reviewer; automatic State Interpreter; Rejected/Alternate reference Take selection; Scene termination; run-loop repetition; persistence; cross-Scene continuity; ODR-12/13/30/32 resolution; WinUI; Windows AI/NPU; MSIX/WACK/Store.

## 18. Why this is smallest

A full provider-driven run loop would reverse dependency order and collapse operational evidence into Core. A one-call complete turn would violate Patch 0016's postcommit adoption boundary. A wrapper around an already-bound `E0Take` adds no missing authority.

This state machine closes only deterministic stitching currently performed manually while preserving external boundaries where real Performer, Integrity advisory, and State Interpreter execution later occur.

After Patch 0018 implementation/native validation, the next seam can be the minimal E0-A Application/Harness run driver that sequences those external executions around this state machine and records authenticated attempt provenance without changing Core causal law.

## 19. Recursive audit status

Proposal 0.2 corrections so far:

1. rejected one-call commit+Opportunity orchestration because it could hide Patch 0016's valid postcommit adoption state;
2. recomposes Context from retained Cycle rather than accepting caller Context;
3. keeps State Interpreter provider execution outside Core;
4. keeps State Authority ReviewRequired as typed non-Take progress;
5. hardcodes only the already-approved reference `Accepted` Take policy;
6. removed `IntegrityRejected` from normal progress after proving Patch 0017 + GateAttempt already exclude both Patch 0008 deterministic reject conditions;
7. made concern evidence mandatory/initialized for lawful CandidateReady Integrity evaluation instead of carrying an unreachable nullable branch.

Continue audit:

```text
correctness -> current authority -> stale-token closure -> Character/Performer separation
-> technical non-fictionalization -> Integrity semantics -> State Authority semantics
-> reference Take policy -> causal adoption -> failure privacy -> dependency direction
-> public-surface minimization -> testability -> simplicity -> hygiene -> ARM64 suitability
-> five-property product fit -> evidence discipline
```

**Implementation remains forbidden pending completion of recursive audit and explicit Director approval.**
