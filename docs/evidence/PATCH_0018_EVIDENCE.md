# Patch 0018 Evidence — Deterministic Turn Orchestration

Status: **IMPLEMENTED / STATIC AUDIT CLEAN / NATIVE ARM64 VALIDATION PENDING**

Date: 2026-09-05

## Authority

Parent `main`: `0548078a060267e136968c2bf97b384356b50554`.

Approved architecture: `docs/blueprint/H1_PATCH_0018_DETERMINISTIC_TURN_ORCHESTRATION.md`, Proposal 0.6.

Blueprint approval evidence: `docs/evidence/H1_PATCH_0018_BLUEPRINT_APPROVAL.md`.

Approved blueprint branch head: `36e2bab519962a3de430d2e6f718b07405052f02`.

Last executable/test checkpoint before this evidence file:

`7e94614dc484aff8cb9b8e39c1a74a7b04ea3238`

Machine-readable oracle: `docs/evidence/PATCH_0018_ORACLE.json`.

## Falsification and implemented surface

Patch 0018 was necessary because existing Core contained each deterministic authority individually but no closed Turn state machine joining current synchronized Cycle state to Patch 0017 attempt semantics, Integrity, Interpreter proposal, State Authority, Take, and accepted commit while preserving Patch 0016's postcommit adoption boundary.

Added executable source only:

```text
src/Ensemble.E0.Core/Turn/E0TurnProgress.cs
src/Ensemble.E0.Core/Turn/DeterministicE0TurnOrchestrator.cs
```

Added tests only:

```text
tests/Ensemble.E0.Core.Tests/Turn/E0TurnContractTests.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnOrchestrationTests.cs
tests/Ensemble.E0.Core.Tests/Turn/E0TurnDeterminismTests.cs
```

No preexisting Core, Harness, fixture, framework, SDK, canonicalizer, or persistence source changed.

## Public contract

Exactly four exported Turn types:

```text
E0TurnProgressDisposition
E0TurnProgress
DeterministicE0TurnOrchestrator
E0TurnOrchestrationException
```

Exact progression:

```text
TechnicalFailure=1
Cancelled=2
CandidateReady=3
RequestAnotherTake=4
ReadyForInterpretation=5
AuthorityReviewRequired=6
TakeBindable=7
AcceptedTakeReady=8
```

Exactly six public operations:

```text
GateAttempt
EvaluateIntegrity
EvaluateAuthority
ResolveAuthorityReview
BindAcceptedTake
CommitAccepted
```

## Authority properties

- `GateAttempt` recomposes the current Cycle Context and replays Patch 0017 binding; stale Candidate and stale technical results fail closed.
- Every Turn progress validation re-proves exact current `ContextPacketId`; a same-state forged Context with a different packet identity cannot reenter progression.
- technical failure/cancellation are payload-free and cannot create Character behavior or mutate Production.
- Patch 0017 + current Context binding makes Patch 0008 deterministic Reject codes unreachable; an unexpected Reject is invariant failure, not a fictional outcome.
- Integrity `RequestAnotherTake` creates no Take and consumes no Opportunity.
- State Authority uses a fresh snapshot from retained Production; `ReviewRequired` remains Take-free.
- review continuation accepts only a complete replacement review-choice set against the exact retained proposal/policy.
- terminal State Authority becomes `TakeBindable` before any TakeId or Take exists.
- `BindAcceptedTake` applies only the approved reference `Accepted` policy.
- `E0Take.Bind` replay is projected back into `AcceptedTakeReady`; no parallel pre-Take authority view is retained.
- `CommitAccepted` delegates to Patch 0016 and returns `E0PostCommitCycleState`; it never establishes Opportunity.

## Failure/privacy

Exact public messages:

```text
E0 turn attempt gate failed.
E0 turn Integrity evaluation failed.
E0 turn State Authority evaluation failed.
E0 turn State Authority review resolution failed.
E0 turn accepted Take binding failed.
E0 turn accepted commit failed.
```

Public Turn exceptions retain no lower inner exception. No fixed failure message includes Context prose, Candidate text/control, proposal/review payload, provider/model/raw output, credentials, or arbitrary untrusted values. No blanket `catch (Exception)` was introduced.

## Test/oracle coverage

The new suite statically defines 24 Patch 0018 tests. If the inherited Patch 0017 total remains 598, the expected native total is 622; this count is **not validation evidence until observed on the Director machine**.

Coverage includes:

- exact public surface and no provider/platform/run-loop contract;
- stale Candidate and stale technical attempt rejection;
- technical/cancelled no-fiction behavior;
- Integrity Accept and RequestAnotherTake paths;
- mismatched Interpreter proposal rejection;
- proposal/policy-bound creator review continuation;
- TakeBindable-before-TakeId ordering;
- reference Accepted Take and replayed authority projection;
- rejected consequence mutation does not reject the reference Take;
- invalid TakeId/CommitId fixed-stage failure;
- direct inherited Cycle commit equivalence and explicit later Opportunity establishment;
- identical-input determinism;
- VOSS -> MARLOWE -> WREN -> VOSS inherited three-turn rotation;
- impossible forged same-state Context rejection without Candidate-text leakage.

Reflection is used only for public-surface assertions and impossible construction plumbing.

## Scope audit

No provider/model execution or request framing, raw/partial output, streaming, `CancellationToken`, retry/backoff, spend/budget, provider/model/version/settings/metrics, AttemptId/RunId, authenticated attempt provenance, secret storage, automatic Integrity/Interpreter execution, Rejected/Alternate reference Take selector, Scene termination, repeated run loop, persistence, cross-Scene continuity, ODR-12/13/30/32 resolution, WinUI, Windows AI/NPU, MSIX/WACK, or Store behavior was added.

## Static convergence result

Repository compare against current `main` is 0 commits behind and contains only the approved Patch 0018 blueprint/approval plus new Turn source/tests. No existing executable file changed.

Recursive static review found and corrected before machine validation:

- an unnecessary namespace qualification;
- persistent exact-Context freshness for all progress states;
- test-only reliance on Context CLR reference identity despite deterministic Context recomposition;
- test-only nonpublic Turn reflection that was not needed;
- missing stale-technical/proposal-mismatch/ID negative-path coverage.

Final static pass found no remaining material in-scope correction or worthwhile simplification.

## Native validation gate

No Patch 0018 compiler/runtime claim is made yet. The engineering assistant environment has no usable .NET compiler/repository checkout authority, and the repository has no GitHub Actions workflow.

Required Director-machine gate:

```text
Windows ARM64 / win-arm64
clean tracked + staged tree
Core + test compile
full Core test suite
Harness ARM64 build
Missing Raft fixture
Generic smoke fixture
```

Promotion is forbidden until that exact native evidence is observed and recorded.
