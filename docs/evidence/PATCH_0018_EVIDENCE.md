# Patch 0018 Evidence — Deterministic Turn Orchestration

Status: **IMPLEMENTED / DIRECTOR-MACHINE ARM64 VALIDATED / PROMOTION READY**

Date: 2026-09-05

## Authority

Parent `main`: `0548078a060267e136968c2bf97b384356b50554`.

Approved architecture: `docs/blueprint/H1_PATCH_0018_DETERMINISTIC_TURN_ORCHESTRATION.md`, Proposal 0.6.

Blueprint approval evidence: `docs/evidence/H1_PATCH_0018_BLUEPRINT_APPROVAL.md`.

Approved blueprint branch head: `36e2bab519962a3de430d2e6f718b07405052f02`.

Last executable/test checkpoint before evidence-only commits:

`7e94614dc484aff8cb9b8e39c1a74a7b04ea3238`

Exact checkout validated on the Director's native Windows ARM64 machine:

`023b469b801239c2b0d597aec1e7712aa4a8faa7`

Commits after the executable/test checkpoint and before the validated checkout were documentation-only (`PATCH_0018_EVIDENCE.md` and `PATCH_0018_ORACLE.json`).

Machine-readable oracle: `docs/evidence/PATCH_0018_ORACLE.json`.

## Implemented surface

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

Public Turn namespace is exactly four types with eight non-default progress dispositions and six public orchestration operations.

## Authority properties

- `GateAttempt` recomposes current Cycle Context and replays Patch 0017 binding; stale Candidate and stale technical results fail closed.
- every Turn progress validation re-proves exact current `ContextPacketId`;
- technical failure/cancellation are payload-free and cannot create Character behavior or mutate Production;
- impossible Patch 0008 deterministic Reject after Patch 0017/current-Context proof fails closed;
- Integrity `RequestAnotherTake` creates no Take and consumes no Opportunity;
- State Authority evaluates against a fresh retained-Production snapshot;
- `ReviewRequired` remains Take-free and review continuation remains bound to the exact proposal/policy;
- terminal State Authority becomes `TakeBindable` before TakeId/Take;
- `BindAcceptedTake` applies only the reference `Accepted` policy and projects the fresh `E0Take.Bind` Authority replay;
- `CommitAccepted` delegates to Patch 0016 and returns `E0PostCommitCycleState` without establishing Opportunity.

## Failure/privacy

Exact public failure messages remain fixed:

```text
E0 turn attempt gate failed.
E0 turn Integrity evaluation failed.
E0 turn State Authority evaluation failed.
E0 turn State Authority review resolution failed.
E0 turn accepted Take binding failed.
E0 turn accepted commit failed.
```

Public Turn exceptions retain no lower inner exception. No fixed failure message includes Context prose, Candidate text/control, proposal/review payload, provider/model/raw output, credentials, or arbitrary untrusted values. No blanket `catch (Exception)` was introduced.

## Test coverage

Patch 0018 adds 24 tests over the inherited 598-test baseline. Native validation observed the expected total of 622.

Coverage includes exact public surface; stale Candidate/technical rejection; non-fictional technical/cancelled paths; Integrity Accept/RequestAnotherTake; mismatched Interpreter proposal rejection; proposal/policy-bound review; TakeBindable-before-TakeId; Accepted Take replay projection; rejected mutation not rejecting the reference Take; invalid TakeId/CommitId failure; inherited Cycle commit equivalence; explicit later Opportunity establishment; identical-input determinism; VOSS -> MARLOWE -> WREN -> VOSS rotation; and forged same-state Context rejection without Candidate-text leakage.

Reflection is used only for public-surface assertions and impossible construction plumbing.

## Director-machine native ARM64 validation

Evidence source: **Director machine**, not the engineering assistant environment.

Observed environment:

```text
PROCESSOR_ARCHITECTURE = ARM64
Windows = 10.0.26200
RID = win-arm64
repository-selected SDK = 9.0.317
SDK commit = 26570c2743
MSBuild = 17.14.51+25f168cee
host = 10.0.11 arm64
```

Observed repository state at exact checkout `023b469b801239c2b0d597aec1e7712aa4a8faa7`:

```text
TRACKED_DIFF_EXIT=0
STAGED_DIFF_EXIT=0
```

Observed Core gate:

```text
Ensemble.E0.Core compile PASS
Ensemble.E0.Core.Tests compile PASS
total: 622
succeeded: 622
failed: 0
skipped: 0
CORE_TEST_EXIT=0
```

Observed Harness/runtime gates:

```text
Harness build PASS
HARNESS_BUILD_EXIT=0

Fixture validated: ensemble.e0.missing-raft@0.1.0
MISSING_RAFT_EXIT=0

Fixture validated: ensemble.e0.smoke@0.1.0
GENERIC_SMOKE_EXIT=0
```

These are target-device observations from the Director's native Windows ARM64 machine. They do not establish WinUI runtime, Windows AI/NPU execution, MSIX/WACK, or Store certification.

## Scope / recursive convergence audit

No provider/model execution or request framing, raw/partial output, streaming, `CancellationToken`, retry/backoff, spend/budget, provider/model/version/settings/metrics, AttemptId/RunId, authenticated attempt provenance, secret storage, automatic Integrity/Interpreter execution, Rejected/Alternate reference Take selector, Scene termination, repeated run loop, persistence, cross-Scene continuity, ODR-12/13/30/32 resolution, WinUI, Windows AI/NPU, MSIX/WACK, or Store behavior was added.

Final recursive static + Director-machine pass checked correctness, consistency, approved architecture, stale-token closure, Character/Performer separation, Access-before-Context preservation, technical-failure non-fictionalization, causal/Take boundaries, privacy, dependency direction, scope, tests/oracles, simplicity, hygiene, ARM64 suitability, five-property product fit, and evidence discipline.

Result:

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
0 worthwhile in-scope simplifications found in the final pass
```

Patch 0018 is ready for promotion.
