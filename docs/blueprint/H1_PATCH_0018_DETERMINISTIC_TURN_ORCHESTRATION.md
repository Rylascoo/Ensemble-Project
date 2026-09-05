# H1 Patch 0018 — Deterministic Turn Orchestration

Status: **BLUEPRINT PROPOSAL 0.1 — RECURSIVE AUDIT IN PROGRESS; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-05
Parent `main`: `0548078a060267e136968c2bf97b384356b50554`
Latest executable authority: H1 Patch 0017 — Provider-Neutral Performer Attempt Boundary Proposal 0.2.

## 1. Falsification

**This patch is unnecessary if current Core already exposes one closed deterministic turn-state machine that binds the current synchronized Cycle state to a Patch 0017 attempt, distinguishes non-Take outcomes, advances accepted Candidate semantics through Integrity/Interpreter/State Authority/Take, and commits an Accepted Take without collapsing Patch 0016's postcommit adoption boundary.**

It does not. Current source has every individual deterministic semantic authority plus Patch 0017 fictional ingress, but tests/Harness still stitch those authorities manually.

## 2. Exact purpose

Patch 0018 closes only the deterministic semantic orchestration seam:

```text
Opportunity-bearing Cycle state
  -> exact fresh Context
  -> Patch 0017 Performer attempt result
  -> Candidate-ready gate OR technical/cancelled terminal result
  -> deterministic Integrity evaluation
  -> Reject / RequestAnotherTake OR ReadyForInterpretation
  -> supplied semantic StateInterpretationProposal
  -> deterministic State Authority
  -> ReviewRequired OR reference Accepted Take
  -> accepted causal commit
  -> VALID POSTCOMMIT STATE
```

Opportunity establishment remains the existing explicit Patch 0016 second phase:

```text
VALID POSTCOMMIT STATE
  -> DeterministicE0CausalCycle.EstablishOpportunity(...)
  -> next Opportunity-bearing state
```

Patch 0018 does not execute any provider/model, construct provider requests, retry, spend, stream, persist, terminate Scenes, or establish Opportunity itself.

## 3. Critical adoption law

Patch 0018 must **not** expose a one-call `ExecuteTurn` that commits and then establishes Opportunity.

Patch 0016 intentionally makes accepted causal commit authoritative before later Opportunity establishment. If a wrapper committed and then threw during Opportunity establishment without returning the postcommit token, the caller could lose the authoritative adopted state.

Therefore Patch 0018 ends at `E0PostCommitCycleState`. The caller must invoke the existing Patch 0016 Opportunity phase explicitly. If that later phase fails, the returned postcommit state remains valid and authoritative.

## 4. Existing laws preserved

1. Character != Performer.
2. Access Control and Context composition precede Performer use.
3. Technical failure/cancellation never becomes Character behavior.
4. Integrity Reject / RequestAnotherTake produces no `E0Take`.
5. State Authority consequence decisions do not determine Take disposition.
6. Reference E0 orchestration accepts every Take-bindable package; Rejected/Alternate are explicit labeled deviations outside this reference path.
7. Only an Accepted Take may enter causal commit.
8. Accepted Performance + approved consequences remain one atomic causal commit.
9. State Authority `ReviewRequired` is a normal non-Take outcome, not a technical exception.
10. Provider/runtime failure in later Integrity/Interpreter execution remains outside this semantic orchestrator and produces no call that could create fiction.

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
TechnicalFailure      = 1,
Cancelled             = 2,
CandidateReady        = 3,
IntegrityRejected     = 4,
RequestAnotherTake    = 5,
ReadyForInterpretation= 6,
AuthorityReviewRequired = 7,
AcceptedTakeReady     = 8
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

Every constructed token internally retains the exact source `E0OpportunityBearingCycleState`; it is not exposed as a new mutable authority.

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

IntegrityRejected / RequestAnotherTake
  Candidate!=null
  Integrity!=null with matching disposition
  later fields null

ReadyForInterpretation
  Candidate!=null
  Integrity=Accept
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

All factories must prove their own full invariant at the actual construction point. No internal factory may accept a preselected enum/payload combination without revalidation.

## 7. Exact public operations

Exactly four public static methods:

```csharp
E0TurnProgress GateAttempt(
    E0OpportunityBearingCycleState source,
    E0PerformerAttemptResult attemptResult);

E0TurnProgress EvaluateIntegrity(
    E0TurnProgress source,
    ImmutableArray<IntegrityConcernKind>? concernKinds);

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

`GateAttempt` first recomposes the exact current Context from the supplied Opportunity-bearing Cycle state by calling the existing `DeterministicE0CausalCycle.ComposeContext(source)`.

It does not trust a caller-supplied Context.

It fails closed unless:

- source is valid;
- attempt result is valid;
- `attemptResult.SourceContextPacketId` equals the exact freshly composed `ContextPacketId`.

This satisfies Patch 0017's stale-result obligation. A stale technical failure is rejected just as a stale Candidate is rejected.

Disposition mapping:

```text
Patch0017 TechnicalFailure -> Turn TechnicalFailure
Patch0017 Cancelled        -> Turn Cancelled
Patch0017 CandidateReady   -> Turn CandidateReady
```

For CandidateReady, the exact Candidate reference from the attempt result is retained. Patch 0017 already proves semantic Candidate/Context association; GateAttempt additionally proves current-Cycle freshness.

No Integrity work occurs yet. This leaves an explicit boundary where an external configured Integrity advisory path may run without Core pretending to execute it.

## 9. `EvaluateIntegrity`

Valid only from `CandidateReady`.

It builds a fresh `IntegrityCandidateInput` from the token's exact Context + Candidate.

If deterministic Reject codes are present:

- `concernKinds` must be null;
- `DeterministicIntegrityValidator.Validate(input, null)` is used.

Otherwise:

- `concernKinds` must be present and non-default;
- empty means no semantic concerns;
- non-empty contains the configured advisory concern kinds;
- Core binds them through existing `IntegrityConcernEvidence.Bind` and calls the existing validator.

This patch does not authenticate where the advisory concern kinds came from. Later operational provenance must establish configured reviewer/provider identity and raw evidence when applicable.

Mapping:

```text
Integrity Reject             -> IntegrityRejected
Integrity RequestAnotherTake -> RequestAnotherTake
Integrity Accept             -> ReadyForInterpretation
```

On Accept, `StateInterpretationSource.Bind` is executed immediately and retained. That public source is the exact semantic token for later State Interpreter parsing/provider orchestration.

Reject and RequestAnotherTake create no `E0Take` and leave source Cycle state unchanged.

## 10. State Interpreter boundary

Patch 0018 does not receive raw Interpreter bytes and does not call a provider.

After `ReadyForInterpretation`, external orchestration may invoke its configured State Interpreter and then use existing:

```text
StateInterpretationContract.ParseJson(
    progress.InterpretationSource,
    rawBytes)
```

to create a semantic `StateInterpretationProposal`.

Provider refusal/error/timeout/cancellation at this external stage produces no `PrepareTake` call and therefore no fiction. Exact downstream technical attempt taxonomy/provenance remains outside Patch 0018.

## 11. `PrepareTake`

Valid from `ReadyForInterpretation` or `AuthorityReviewRequired` only. Allowing a fresh call after ReviewRequired makes each review attempt an explicit deterministic reevaluation over newly supplied semantic inputs; no prior review result has state authority.

Requirements:

- TakeId initialized;
- proposal non-null and exactly compatible with retained `InterpretationSource`;
- policy non-null;
- `reviewChoices` initialized (empty valid);
- fresh `StateAuthoritySnapshot` captured from the retained source Cycle Production;
- fresh `StateAuthorityInput.Bind(snapshot, interpretationSource, proposal)` succeeds;
- fresh `StateAuthorityReviewSet.Bind(input, reviewChoices)` succeeds;
- existing `DeterministicStateAuthority.Evaluate` runs.

If evaluation is `ReviewRequired`:

```text
Disposition = AuthorityReviewRequired
AuthorityEvaluation = exact fresh evaluation
AcceptedTake = null
```

This is a normal deterministic non-Take result. Caller may inspect the evaluation and rerun `PrepareTake` with explicit review choices.

If evaluation is `Complete`, Patch 0018 applies the already-approved **reference E0 Take policy**:

```text
E0Take.Bind(..., E0TakeDisposition.Accepted)
```

and returns `AcceptedTakeReady`.

Patch 0018 exposes no Rejected/Alternate choice. Those remain separately labeled experimental/intervention paths requiring attributable provenance.

State Authority mutation dispositions never silently alter Take disposition.

## 12. `CommitAccepted`

Valid only from a fully proven `AcceptedTakeReady` token.

It delegates exactly to:

```text
DeterministicE0CausalCycle.CommitAcceptedTake(
    commitId,
    retainedSourceCycle,
    retainedSourceContext,
    retainedAcceptedTake,
    materializations)
```

and returns the resulting `E0PostCommitCycleState` unchanged.

No duplicate commit, binding, history, or state-hash algorithm is introduced.

`CommitAccepted` does **not** establish the next Opportunity.

After successful return, accepted causal history is authoritative. The caller then explicitly invokes existing `DeterministicE0CausalCycle.EstablishOpportunity(postCommit)`.

## 13. Failure/privacy contract

Exact public failure messages:

```text
E0 turn attempt gate failed.
E0 turn Integrity evaluation failed.
E0 turn Take preparation failed.
E0 turn accepted commit failed.
```

No public Patch 0018 exception retains lower inner exceptions.

Failure representation must not expose Context prose, Candidate text/control, Interpreter proposal prose, review payload, provider/model/raw output, credentials, or arbitrary untrusted values.

No blanket `catch (Exception)`.

Known lower semantic exceptions are converted only at the corresponding Patch 0018 stage boundary.

## 14. Purity, authority, and adoption

All operations are synchronous deterministic transformations over explicit immutable semantic inputs.

No clock, randomness, network, filesystem, environment state, task/thread, `CancellationToken`, provider SDK, background work, ID allocation, persistence, retry/backoff, or spend logic.

Before `CommitAccepted`, every progress token has zero Production mutation authority.

`TechnicalFailure`, `Cancelled`, `IntegrityRejected`, `RequestAnotherTake`, and `AuthorityReviewRequired` do not consume Opportunity, mutate Production, or enter accepted history.

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

Expected edits to existing Core/Harness source: **zero**.

If implementation requires changing Patch 0017, Integrity, Interpreter, State Authority, Take, Cycle, Opportunity, continuity, canonicalization, framework, or SDK semantics, reopen the blueprint.

## 16. Required tests

Gating tests must prove:

- exact four-type Turn namespace surface;
- exact eight non-default progress dispositions;
- progress token closed/immutable with exact seven public properties;
- exact four public orchestrator methods/signatures;
- exception catchable but not publicly constructible;
- GateAttempt recomposes current Context and rejects stale Candidate/technical results;
- technical failure/cancellation retain no Candidate and mutate nothing;
- CandidateReady retains exact Candidate reference;
- Integrity Reject / RequestAnotherTake create no Take and leave Cycle unchanged;
- Integrity Accept creates exact `StateInterpretationSource`;
- malformed/default concern input fails sanitized;
- State Authority ReviewRequired is typed, carries evaluation, creates no Take;
- Complete authority produces only an Accepted Take under reference policy;
- mutation rejection does not reject the Take;
- repeated equivalent explicit inputs produce equivalent progress/Take semantics;
- stale/mismatched proposal/review inputs fail closed;
- CommitAccepted rejects every non-AcceptedTakeReady progress state;
- AcceptedTakeReady commit reproduces inherited Patch 0016 causal hashes/history semantics;
- commit returns postcommit state and does not establish Opportunity;
- explicit later Opportunity establishment reproduces inherited Patch 0016 successor semantics;
- phase-two Opportunity failure can never erase the returned postcommit token;
- public surface contains no provider/model/network/task/cancellation/persistence/retry/spend/RunId/AttemptId contract;
- exact sanitized messages leak no semantic/provider payload;
- all 598 inherited Core tests remain intact before new-test count is observed.

Reflection is permitted only for public-surface assertions or impossible construction plumbing, following member-visibility testing discipline.

## 17. Explicit non-scope

No provider/model execution; provider request/framing; raw Performer/Integrity/Interpreter output; streaming; cancellation primitive; retry/backoff; spend/budget; provider/model/version/settings/metrics; AttemptId/RunId; authenticated provenance; secret storage; automatic Integrity reviewer; automatic State Interpreter; Rejected/Alternate reference Take selection; Scene termination; run-loop repetition; persistence; cross-Scene continuity; ODR-12/13/30/32 resolution; WinUI; Windows AI/NPU; MSIX/WACK/Store.

## 18. Why this is the smallest useful orchestrator

A full provider-driven run loop would reverse dependency order and collapse operational evidence into Core. A one-call complete turn would violate Patch 0016's postcommit adoption boundary. Another generic wrapper around already-bound `E0Take` would add no missing authority.

This state machine closes only the repeated deterministic stitching currently performed manually while preserving explicit external boundaries where real Performer, Integrity advisory, and State Interpreter execution will later occur.

After Patch 0018 is implemented/native-validated, the next architecture seam can be the minimal E0-A Application/Harness run driver that sequences those external executions around this deterministic state machine and records authenticated attempt provenance without changing Core causal law.

## 19. Recursive audit target

Before approval, audit:

```text
correctness -> current authority -> stale-token closure -> Character/Performer separation
-> technical non-fictionalization -> Integrity semantics -> State Authority semantics
-> reference Take policy -> causal adoption -> failure privacy -> dependency direction
-> public-surface minimization -> testability -> simplicity -> hygiene -> ARM64 suitability
-> five-property product fit -> evidence discipline
```

**Implementation remains forbidden pending completion of recursive audit and explicit Director approval.**
