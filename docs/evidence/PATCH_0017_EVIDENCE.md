# Patch 0017 Evidence — Provider-Neutral Performer Attempt Boundary

Status: **IMPLEMENTED / STATIC AUDIT CLEAN / NATIVE ARM64 VALIDATION PENDING**

Date: 2026-09-05

## Authority

Parent `main`: `b85baf70f1b93c5d7d368c78b9faff55082351cd`.

Approved architecture: `docs/blueprint/H1_PATCH_0017_PERFORMER_ATTEMPT_BOUNDARY.md`, Proposal 0.2.

Director approval: explicit project-conversation approval on 2026-09-05.

Machine-readable oracle: `docs/evidence/PATCH_0017_ORACLE.json`.

Latest executable authority before this patch: H1 Patch 0016 — Synchronized Causal Cycle Proposal 0.5.

## Falsification

Patch 0017 would be unnecessary if current source already exposed one closed provider-neutral boundary binding either a valid `CandidatePerformance` or non-fictional technical non-performance to the exact semantic `ContextPacket`.

It did not. Existing source had strict Candidate semantics and the synchronized causal cycle, but no attempt/result boundary and no run-orchestration implementation.

## Implemented surface

Added only:

```text
src/Ensemble.E0.Core/PerformerAttempt/E0PerformerAttemptModels.cs
src/Ensemble.E0.Core/PerformerAttempt/DeterministicE0PerformerAttemptBoundary.cs
tests/Ensemble.E0.Core.Tests/PerformerAttempt/E0PerformerAttemptContractTests.cs
tests/Ensemble.E0.Core.Tests/PerformerAttempt/E0PerformerAttemptTests.cs
```

No existing Core production source changed. No Harness source changed. No framework/SDK retarget occurred.

Public namespace surface is exactly four types:

```text
E0PerformerAttemptDisposition
E0PerformerAttemptResult
DeterministicE0PerformerAttemptBoundary
E0PerformerAttemptException
```

Disposition grammar is exactly:

```text
CandidateReady = 1
TechnicalFailure = 2
Cancelled = 3
```

## Semantics

A successful Candidate binding proves:

- source Context is present;
- source ContextPacketId, subject, and Opportunity are initialized;
- E0 subject equals Opportunity;
- Candidate semantic contract version is exact;
- Candidate ContextPacketId equals source ContextPacketId;
- Candidate subject equals source subject.

The result preserves the exact supplied Candidate reference and copies semantic Context identity from the trusted Context object.

`TechnicalFailure` and `Cancelled` contain no Candidate and no Character-legible payload. Provider refusal, timeout, transport/provider error, and invalid Candidate output are intentionally not subdivided in Core. Exact operational reason belongs to later diagnostics/provenance and retry policy.

Patch 0017 does not claim exact provider disclosure and therefore carries no RenderedContextHash, provider request identity, provider/model identity, raw output, metrics, AttemptId, or RunId.

## Construction closure

Initial implementation used private result construction plus internal factories accepting only preselected ContextPacketId/disposition. Recursive review found that future same-assembly code could then bypass the full semantic proof.

Correction: both internal factories now accept the full source Context and perform the complete invariant proof at the actual construction point. The public boundary is only a thin entry point. Invalid construction emits only the approved fixed Patch 0017 exception messages.

This preserves the closed-token discipline established by Patch 0016.

## Failure/privacy

Exact public failure messages:

```text
E0 Performer attempt candidate binding failed.
E0 Performer attempt technical outcome binding failed.
```

No lower inner exception is retained. Validation does not echo Context prose, Candidate VisibleText, raw provider output, credentials, model/provider information, or arbitrary untrusted values.

## Tests

Added contract coverage for:

- exact four-type exported namespace surface;
- exact three-value non-default disposition grammar;
- closed immutable three-property result projection;
- exact two-method boundary surface;
- catchable/non-publicly-constructible exception;
- absence of provider/model/request/raw/provenance/RunId/AttemptId/Take/Commit/task/thread/cancellation-token/filesystem/network/platform/persistence public contracts.

Added behavior coverage for:

- exact Context + Candidate -> `CandidateReady` with same Candidate reference;
- stale Candidate rejected against next-cycle Context;
- forged wrong-subject Candidate fails closed without text leakage;
- `TechnicalFailure` and `Cancelled` produce null Candidate and preserve source state;
- `CandidateReady`, default, and undefined enum values cannot enter technical binding;
- invalid Candidate transport may be mapped externally to payload-free `TechnicalFailure`;
- null public inputs yield exact sanitized messages.

All inherited Patch 0016/reference tests remain untouched.

## Scope audit

No provider SDK, provider request construction, exact disclosure proof, raw/partial output, streaming, `CancellationToken`, retry/backoff, spend/budget, provider/model/version/settings, latency/token/cost metrics, AttemptId/RunId, authenticated attempt provenance, secret storage, real Integrity/Interpreter provider execution, Take review policy, run/Scene orchestration, persistence, cross-Scene continuity, ODR resolution, WinUI, Windows AI/NPU, MSIX/WACK, or Store behavior is added.

## Recursive static audit

Final static pass checked:

```text
correctness
consistency
approved architecture
construction closure
Character/Performer separation
Access-before-Context preservation
technical-failure non-fictionalization
causal/Take authority boundaries
failure privacy
public-surface minimization
dependency direction
provider/platform isolation
test semantics
hygiene
ARM64 suitability
five-property product fit
evidence discipline
```

Result:

```text
0 material correctness corrections outstanding
0 authority contradictions
0 construction bypasses
0 Character/Performer conflations
0 Access-before-Context violations
0 technical-failure fictionalization paths
0 causal/Take authority leaks
0 provider/platform dependencies
0 provenance overclaims
0 new IDs/hashes/persistence contracts
0 worthwhile in-scope simplifications found in the final pass
```

## Validation gate

No compiler, test, Harness, runtime, or native ARM64 claim is made from the engineering assistant environment.

Native Windows ARM64 validation remains required on the Director machine before promotion.
