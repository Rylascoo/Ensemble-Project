# E0-A Post-Audit Hardening — Patch Group 1 Implementation

Status: **IMPLEMENTED — RECURSIVE STATIC AUDIT COMPLETE; NATIVE VALIDATION PENDING**

Date: 2026-09-06

## Authority and baseline

Repository: `Rylascoo/Ensemble-Project`

Branch: `e0a-phase-b-post-audit-hardening`

Audited `main` baseline / merge base:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Exact executable/test checkpoint after Patch Group 1:

`171881c1247e1c466fec6abd3e92335a055eb4f2`

Draft review surface: PR #39.

Proposal 0.15 remains unchanged. Core remains unchanged. Provider execution, credential use, network inference, and spend remain unauthorized.

## Scope implemented

Patch Group 1 contains only:

- **E-01 — Integrity failure terminal sealing**
- **E-02 — Streaming cancellation boundary**

No later audit finding is implemented here.

## Changed executable/test surface

Exactly four files differ from the audited baseline:

- `src/Ensemble.E0.Harness/Run/E0AReferenceRunDriver.cs`
- `src/Ensemble.E0.Harness/OpenAI/OpenAIResponsesPort.cs`
- `tests/Ensemble.E0.Harness.Tests/E0ARunDriverTests.cs`
- `tests/Ensemble.E0.Harness.Tests/OpenAIResponsesPortWireTests.cs`

No `src/Ensemble.E0.Core/**` file changed.

## E-01 — Integrity failure terminal sealing

### Defect

The Harness parsed a successful Integrity receipt and delegated concern binding/evaluation to `DeterministicE0TurnOrchestrator.EvaluateIntegrity`, but a known deterministic orchestration rejection could escape the run loop before `Finish(...)`. That bypassed the normal run-terminal and runtime-evidence sealing path.

Duplicate concern kinds are intentionally preserved by the Harness parser and rejected by Core binding semantics. Proposal 0.15 does not authorize a parallel Harness Integrity validator.

### Correction

At the exact `EvaluateIntegrity` call site, catch only `E0TurnOrchestrationException` and terminate through the existing `InvalidOutput -> Finish(...)` path.

The correction does **not** add `catch (Exception)`, does not alter Core, and does not add Harness-side duplicate/canonical Integrity authority. Unexpected non-orchestration programming failures remain visible.

### Regression coverage

`DuplicateIntegrityConcerns_TerminateAsInvalidOutputBeforeInterpreterAndSealEvidence` proves:

- duplicate concerns reach Core and fail deterministically;
- terminal status is `InvalidOutput`;
- only Performer and Integrity provider roles are invoked;
- Interpreter is not invoked;
- Production state is unchanged;
- both completed provider terminal receipts remain evidenced;
- no accepted Performance enters transcript history;
- no commit occurs;
- `run.terminal` and `run.final.json` are produced;
- the runtime seal contains a 64-character root digest.

The pre-existing parser/Core-boundary test remains unchanged and continues to prove that duplicates are preserved for Core rejection.

## E-02 — Streaming cancellation boundary

### Defect

`ExecuteStreamingAsync` used `StreamReader.EndOfStream` as the loop condition before `ReadLineAsync(cancellationToken)`. `EndOfStream` may synchronously probe the underlying stream, creating a non-cancellable blocking point before the intended cancellable read.

### Correction

Replace synchronous EOF probing with an unconditional loop whose only input read is:

`await reader.ReadLineAsync(cancellationToken)`

A `null` line is treated as EOF.

Streaming diagnostic recording, refusal/failure handling, `response.completed` semantic authority, and the outer run-driver timeout/cancellation ownership remain unchanged.

### Regression coverage

`StreamingStalledBody_CancelsWithoutSynchronousEofProbe` uses a stream that:

- forbids/counts synchronous reads;
- signals when an asynchronous read actually begins;
- stalls until its cancellation token is cancelled.

The test cancels only after the asynchronous read has begun and proves the operation exits by cancellation with zero synchronous reads.

Existing successful streaming tests remain intact and continue to prove provisional diagnostic capture and completed-response semantic authority.

## Recursive static audit

The completed Patch Group 1 surface was recursively checked for:

- correctness;
- authority boundaries;
- evidence lifecycle behavior;
- simplicity;
- regression-test quality;
- ARM64 suitability;
- scope discipline.

Two intermediate implementation choices were removed during audit:

1. a new `integrity.rejected` evidence event was removed because it added unnecessary evidence taxonomy;
2. a duplicate-specific catch filter was removed because it duplicated Core semantic classification at the Harness boundary.

Final implementation uses the existing typed Core orchestration failure boundary and the existing terminal/evidence path.

Static closure result: **no material Patch Group 1 correction or worthwhile in-scope simplification remains identified.**

## Validation status

No compiler, test-runtime, Windows ARM64, fixture-smoke, or live-host validation claim is made for `171881c1247e1c466fec6abd3e92335a055eb4f2`.

The prior Director-machine native validation checkpoint `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` remains historical evidence for the pre-hardening executable/test surface only.

The hardening branch currently has no GitHub Actions workflow producing compiler/test evidence. Native Windows ARM64 validation remains required before the hardening branch can replace the prior executable/test authority.

Per the approved hardening branch strategy, Patch Group 1 remains isolated on the branch; later hardening groups and their recursive audits precede the final grouped native ARM64 validation unless the Director separately requests an earlier targeted validation checkpoint.

## Gate

- Patch Group 1 implementation: **COMPLETE**
- Patch Group 1 recursive static audit: **COMPLETE**
- Patch Group 1 native validation: **NOT YET PERFORMED**
- Patch Group 2: **NOT STARTED IN THIS RECORD**
- Provider execution: **NOT AUTHORIZED**
- Merge to `main`: **NOT AUTHORIZED / NOT READY**
