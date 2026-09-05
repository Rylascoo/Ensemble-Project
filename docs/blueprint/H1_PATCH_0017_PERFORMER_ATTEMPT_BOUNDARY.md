# H1 Patch 0017 — Provider-Neutral Performer Attempt Boundary

Status: **BLUEPRINT PROPOSAL 0.2 — RECURSIVELY AUDITED; DIRECTOR APPROVAL REQUIRED; IMPLEMENTATION FORBIDDEN**

Date: 2026-09-05
Parent `main`: `b85baf70f1b93c5d7d368c78b9faff55082351cd`
Latest executable authority: H1 Patch 0016 — Synchronized Causal Cycle Proposal 0.5.

## 1. Falsification

**This patch is unnecessary if current source already exposes one closed provider-neutral boundary that correlates either a valid `CandidatePerformance` or a typed technical non-performance outcome to the exact bounded `ContextPacket`, without turning provider/runtime failure into Character behavior or Production history.**

It does not. Current source has the semantic Candidate contract and synchronized causal cycle, but no attempt/result contract and no run-orchestration implementation.

## 2. Purpose

Patch 0017 closes only this fictional-ingress seam:

```text
bounded ContextPacket
    -> external/harness Performer execution later
        -> technical attempt outcome
            -> valid semantic Candidate OR no Candidate
                -> later deterministic run orchestration
```

It answers only:

> For this exact semantic Context, did the attempt yield one already-valid semantic Candidate, end in technical non-performance, or get cancelled?

It does not execute a provider, authenticate provenance, retry, spend, stream, allocate AttemptId/RunId, or decide Take/commit behavior.

## 3. Why this may live in Core

The ship plan assigns provider execution/sequencing and cancellation boundaries to later Application/AI layers. That remains unchanged.

Patch 0017 is only a **fictional-ingress guard** around existing Core semantic objects:

- source identity is `ContextPacketId`;
- successful payload is existing `CandidatePerformance`;
- technical outcomes contain no prose or operational detail;
- no provider SDK/network/task/cancellation primitive enters Core;
- later Application/provider code remains responsible for truthful operational classification and authenticated provenance.

Creating an Application assembly solely for this tiny semantic guard would prematurely freeze project topology; the ship plan leaves exact project names/assembly count open.

## 4. Preserved laws

1. Character != Performer.
2. Access Control precedes Context composition and external Performer use.
3. Candidate is provisional semantic output, not truth/acceptance/persistence.
4. Provider refusal/error/timeout and invalid output are technical non-performance.
5. Character refusal/redirection/silence can be valid Candidate Performance.
6. Partial/cancelled/malformed/unaccepted output never enters Production history.
7. Take semantics begin only after a valid Candidate exists and later gates succeed.
8. Accepted Performance + approved consequences remain the atomic fictional causal commit.
9. Provider/model/request/raw-output/metrics provenance remains outside this result.
10. Technical failure/cancellation cannot establish Opportunity, mutate state, or create fictional prose.

## 5. Exact public surface

New namespace:

```text
Ensemble.E0.Core.PerformerAttempt
```

Exactly four public types:

```csharp
public enum E0PerformerAttemptDisposition
public sealed class E0PerformerAttemptResult
public static class DeterministicE0PerformerAttemptBoundary
public sealed class E0PerformerAttemptException : Exception
```

### Disposition

Exact values:

```csharp
CandidateReady = 1,
TechnicalFailure = 2,
Cancelled = 3
```

`0`/default and undefined values are invalid.

`TechnicalFailure` deliberately collapses provider refusal, timeout, transport/provider error, and output that fails the existing Candidate contract. Core needs only the semantic fact that no valid Candidate exists. Exact operational reason belongs to later diagnostics/provenance and retry policy.

`Cancelled` remains separate because later deterministic run control may stop rather than retry. Patch 0017 itself defines neither policy.

Character refusal is never `TechnicalFailure`; it is `CandidateReady` when represented by a valid Candidate.

### Result

`E0PerformerAttemptResult` has exactly:

```text
Disposition           : E0PerformerAttemptDisposition
SourceContextPacketId : ContextPacketId
Candidate             : CandidatePerformance?
```

All read-only. No public constructor/setter/declared instance method.

Invariant:

```text
Disposition == CandidateReady <=> Candidate is non-null
```

`TechnicalFailure` and `Cancelled` always have `Candidate == null`.

No RenderedContextHash, provider request identity, raw/partial output, error detail, model/provider name, cost/latency/token data, AttemptId, RunId, retry count, or cancellation object.

### Boundary

Exactly two public static methods:

```csharp
E0PerformerAttemptResult BindCandidate(
    ContextPacket sourceContext,
    CandidatePerformance candidate);

E0PerformerAttemptResult BindTechnicalOutcome(
    ContextPacket sourceContext,
    E0PerformerAttemptDisposition disposition);
```

### Exception

`E0PerformerAttemptException` is public/catchable, sealed, with no public constructor.

## 6. Semantic Context association only

Both methods require a non-null source Context with initialized `ContextPacketId`.

`SourceContextPacketId` is copied from the supplied Core `ContextPacket`; callers never supply it independently.

This proves only which semantic Context the result is bound to. It does **not** prove what an external provider actually received.

Patch 0017 deliberately omits `RenderedContextHash`. Patch 0006 reserves exact rendering/request disclosure for later provider-attempt provenance because only orchestration can truthfully record transmitted framing/bytes.

No Context recanonicalization, Access recomputation, denied-data inspection, or provider-request reconstruction.

## 7. Candidate binding

`BindCandidate(sourceContext, candidate)` succeeds only when:

1. both inputs are non-null;
2. source ContextPacketId, SubjectCharacterId, OpportunityCharacterId are initialized;
3. source subject equals opportunity for the current E0 single-opportunity path;
4. Candidate ContractVersion equals `PerformerCandidateContract.CandidateContractVersion`;
5. Candidate ContextPacketId is initialized and equals source ContextPacketId;
6. Candidate SubjectCharacterId is initialized and equals source subject.

Result:

```text
Disposition = CandidateReady
SourceContextPacketId = source ContextPacketId
Candidate = exact supplied Candidate object
```

No clone/rewrite/reparse/accept/commit. Foreign/stale Context Candidate fails closed.

## 8. Technical outcome binding

`BindTechnicalOutcome(sourceContext, disposition)` accepts only:

```text
TechnicalFailure
Cancelled
```

It rejects `CandidateReady`, default `0`, and undefined values.

Result:

```text
Disposition = requested technical disposition
SourceContextPacketId = source ContextPacketId
Candidate = null
```

No technical outcome contains Character-legible text or any payload that can be mistaken for Performance.

The caller may classify the outcome, but this Core object does not authenticate that classification. Later provider/runtime provenance establishes actual refusal/timeout/error/invalid-output/cancellation facts.

## 9. Invalid output stays with existing parser

`PerformerCandidateContract.ParseJson(...)` already owns strict Candidate transport parsing.

Later orchestration may do:

```text
raw completion
  -> ParseJson(sourceContext, bytes)
      success -> BindCandidate(...)
      Candidate-contract failure -> BindTechnicalOutcome(..., TechnicalFailure)
```

The operational evidence layer may separately record that the technical failure reason was invalid output. Patch 0017 does not receive raw bytes or duplicate parser logic.

## 10. Stale result law for the next orchestrator

Patch 0017 binds a result to one semantic Context but does not own the current Cycle state.

Therefore the next run-orchestration authority must fail closed unless the attempt result's `SourceContextPacketId` equals the exact Context freshly composed for its current opportunity-bearing Cycle state. A stale technical outcome or stale Candidate result cannot affect a later turn merely because it is structurally valid.

Patch 0017 does not implement that orchestrator.

## 11. Failure/privacy

Exact public messages:

```text
E0 Performer attempt candidate binding failed.
E0 Performer attempt technical outcome binding failed.
```

No public failure representation may include Context prose, Candidate VisibleText, raw output, provider/model data, credentials, or other untrusted content. Do not retain lower inner exceptions. No blanket `catch (Exception)`.

## 12. Purity and authority

Both methods are synchronous deterministic validation/construction over explicit immutable inputs.

No clock, randomness, network, filesystem, task/thread, `CancellationToken`, environment state, provider SDK, background work, mutable singleton, or ID allocation.

The result has zero Production/Take/Integrity/Interpreter/StateAuthority/Opportunity mutation authority.

Technical outcome does not consume current Opportunity. The same Cycle state remains available for later deterministic stop/retry policy.

## 13. Implementation surface

Source additions only:

```text
src/Ensemble.E0.Core/PerformerAttempt/
  E0PerformerAttemptModels.cs
  DeterministicE0PerformerAttemptBoundary.cs
```

Tests:

```text
tests/Ensemble.E0.Core.Tests/PerformerAttempt/
  E0PerformerAttemptContractTests.cs
  E0PerformerAttemptTests.cs
```

Expected existing Core edits: **zero**.
Expected Harness edits: **zero**.
Expected framework/SDK edits: **zero**.

If implementation requires changing Candidate, Context, Cycle, Take, Integrity, Interpreter, State Authority, Opportunity, or causal-commit semantics, reopen the blueprint.

## 14. Required tests

Gating tests must prove:

- exact four-type namespace surface and exact three-value enum;
- default/undefined disposition invalid;
- result exactly three read-only properties with no public construction;
- boundary exactly two public methods;
- exception catchable but not publicly constructible;
- exact Context/Candidate produces `CandidateReady` and same Candidate reference;
- stale/foreign Context Candidate fails closed;
- wrong Candidate subject fails closed via test construction plumbing;
- `TechnicalFailure` and `Cancelled` produce null Candidate + exact source ContextPacketId;
- `CandidateReady`, default, undefined fail through technical binding;
- technical outcomes mutate no source object;
- Candidate parser failure can be mapped externally to `TechnicalFailure` without raw prose entering result;
- public Patch 0017 surface contains no provider/model/request/raw payload/provenance/RunId/AttemptId/Take/Commit/Task/Thread/CancellationToken/filesystem/network/platform/persistence contract;
- fixed failure messages contain no source/candidate prose;
- all Patch 0016/inherited reference-oracle tests remain intact.

Public-surface reflection allowed; private/IL/call-graph assertions not required.

## 15. Non-scope

No provider/model SDK; network request; provider failure subcategory taxonomy; final provider prompt/framing; exact disclosure/request hash; raw/partial output; streaming; `CancellationToken`; retry/backoff; spend/budget; provider/model/version/settings; latency/token/cost; AttemptId/RunId; authenticated provenance; secret storage; Integrity-assessor execution; Interpreter provider execution; Take review/retry policy; full run/turn state machine; Scene termination; persistence; cross-Scene continuity; ODR-12/13/30/32 resolution; WinUI; Windows AI/NPU; MSIX/WACK/Store.

## 16. Why no Application assembly yet

A separate Application layer is valid later architecture, but its assembly is not required to express this semantic seam and exact project topology remains intentionally unfrozen.

The next run-orchestration blueprint may introduce a concrete Application structure only if its actual caller/dependency needs justify it.

## 17. Why this is smallest

A full run driver still needs a closed representation for “no valid Candidate.” Provider provenance first conflates H1 semantic control flow with Phase-B operational evidence. Retry/spend first lacks a stable input. Provider SDK first reverses dependency order.

Patch 0017 supplies only the missing discriminated semantic result.

After implementation/native validation, the next H1 question is the minimal deterministic run/turn orchestrator consuming `E0PerformerAttemptResult`, existing Integrity/Interpreter/State Authority/Take authority, and Patch 0016 Cycle.

## 18. Recursive audit

Proposal 0.2 was checked against frozen Blueprint 0.1, current `main`, Patch 0006 Candidate authority, Patch 0011 Take semantics, Patch 0015 failure/history exclusions, Patch 0016 Cycle, Ship Plan Phase A/B separation, and the five engineering properties.

Corrections made before this clean pass:

1. removed `RenderedContextHash` to avoid false exact-disclosure provenance;
2. excluded provider/model/raw-output/metrics/AttemptId provenance;
3. avoided a premature Application assembly;
4. kept raw invalid-output parsing in existing Candidate contract;
5. collapsed refusal/timeout/transport/invalid-output into `TechnicalFailure` because Core needs no operational failure taxonomy;
6. retained `Cancelled` only as the minimal distinct control outcome likely needed by later deterministic run policy;
7. added the explicit stale-result obligation for the next orchestrator.

Result:

```text
0 material correctness corrections outstanding
0 Character/Performer conflations
0 Access-before-Context violations
0 technical-failure fictionalization paths
0 causal/Take authority leaks
0 provider/platform dependencies
0 provenance overclaims
0 premature operational taxonomy
0 new IDs/hashes/persistence contracts
0 worthwhile in-scope public-surface simplifications
```

**Implementation remains forbidden pending explicit Director approval.**
