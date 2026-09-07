# E0-A Post-Audit Hardening — Patch Group 2 Implementation

Status: **IMPLEMENTED — RECURSIVE STATIC AUDIT COMPLETE; NATIVE VALIDATION PENDING**

Date: 2026-09-06

## Authority and checkpoint

Repository: `Rylascoo/Ensemble-Project`

Branch: `e0a-phase-b-post-audit-hardening`

Audited `main` baseline / merge base:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Patch Group 2 source/test work started after continuity head:

`9ed964faa392d64bba68c0c824f49ebd57e9ad5d`

Exact executable/test checkpoint after Patch Group 2:

`f37cb8ec41e50d50aba027286326a10677db1040`

Closed-audit authority:

`docs/evidence/E0A_POST_AUDIT_HARDENING_CLOSED_FINDINGS.md`

Proposal 0.15 remains unchanged. Provider execution, credential use, network inference and spend remain unauthorized.

## Scope

Patch Group 2 implements only:

- **E-03 — Provider parsing**
- **E-07 — Timeout configuration**

The E-03 malformed external-data boundary includes the provider adapter's token-preflight/buffered/streaming decoding and the Integrity concern decoder that consumes the successful Integrity structured output. E-05 exact concern-name canonicalization remains deferred to Patch Group 5.

## E-03 — Provider parsing

### Falsification condition stated before editing

E-03 would have been unnecessary if every external provider JSON path already rejected wrong `ValueKind` and malformed Unicode inside the established provider-data failure domain without catching unexpected programming defects.

The audited implementation did not satisfy that condition.

### Correction

`OpenAIResponsesPort` now:

- checks JSON object/array/string/number `ValueKind` before typed access;
- treats malformed external response structure as a technical provider receipt rather than allowing typed-access exceptions to escape;
- uses strict UTF-8 decoding for streamed SSE bytes so invalid byte sequences become the existing technical transport/malformed outcome;
- validates decoded UTF-16 strings against unpaired surrogates before using provider identity, event type or semantic text;
- preserves the existing configured-path rule that only a valid completed response can supply semantic bytes;
- continues to let `OperationCanceledException` propagate to the run-driver deadline owner;
- adds no unfiltered broad exception relabeling.

`E0AIntegrityConcernParser` now narrowly validates/decodes each JSON string before enum parsing so malformed Unicode terminates through `E0AHarnessException`. The exact-name contract is intentionally not changed in this group.

### Regression coverage

Provider wire tests now prove:

- wrong-type input-token counts fail inside the Harness failure domain;
- wrong-type buffered response fields produce technical receipts with no semantic payload;
- malformed Unicode in buffered provider identity fails closed;
- wrong-type streaming event fields produce technical receipts;
- invalid UTF-8 streaming bytes produce technical receipts;
- existing successful buffered and streaming behavior remains covered;
- the Patch Group 1 genuinely stalled-stream cancellation regression remains intact.

Integrity parser coverage now proves malformed Unicode fails through the Harness domain while the existing duplicate-preservation/Core-rejection contract remains intact.

## E-07 — Timeout configuration

### Falsification condition stated before editing

E-07 would have been unnecessary if the effective HTTP transport could not expire before the frozen 300-second attempt cancellation deadline.

The audited host used a default `HttpClient` timeout, so that condition was false.

### Correction

The live host now creates its provider `HttpClient` through one internal factory with `HttpClient.Timeout = Timeout.InfiniteTimeSpan`.

The explicit run-driver linked cancellation token remains the sole attempt deadline and still uses `E0ARunEnvelope.AttemptTimeoutSeconds` (300 seconds) across token preflight and provider execution. No second shorter timeout policy is introduced.

### Regression coverage

`LiveHost_HttpTransportCannotPreemptFrozenAttemptDeadline` proves the actual host factory disables `HttpClient`'s independent timeout and that the frozen attempt constant remains 300 seconds.

## Recursive static audit

The Patch Group 2 implementation was recursively reviewed for:

- malformed external input;
- exception-domain correctness;
- cancellation/timeout composition;
- streaming/buffered/preflight consistency;
- semantic leakage;
- regression-test quality;
- simplicity;
- ARM64 suitability;
- scope discipline.

Audit result: **one complete pass found no remaining material Patch Group 2 correction or worthwhile in-scope simplification.**

No provider semantic payload is created from malformed data. Diagnostic stream recording remains provisional only. The host transport no longer competes with the deterministic attempt deadline.

## Core / scope statement

Patch Group 2 changes no `src/Ensemble.E0.Core/**` file.

No UI, product persistence, NPU, Store/package work, later phase behavior, retry policy, provider execution, credentials or spend were added.

## Validation boundary

No compiler/test-runtime/native validation claim is made for `f37cb8ec41e50d50aba027286326a10677db1040`.

No GitHub Actions result establishes compiler/runtime behavior. Grouped native Windows ARM64 validation remains deferred until all authorized hardening groups complete recursive static audit.

The prior Director-machine checkpoint `1cfdb3aa22abce62a5bd48e80706407670d1c6a9` remains historical pre-hardening executable/test evidence only.

## Gate

- Patch Group 2 implementation: **COMPLETE**
- Patch Group 2 recursive static audit: **COMPLETE**
- Patch Group 2 native validation: **NOT YET PERFORMED**
- Patch Group 3: **NOT STARTED IN THIS RECORD**
- Provider execution: **NOT AUTHORIZED**
- Merge to `main`: **NOT AUTHORIZED / NOT READY**
