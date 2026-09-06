# E0-A Phase B — Reference Run Envelope Implementation Evidence

Status: **STATIC IMPLEMENTATION AUDIT CLEAN — DIRECTOR-MACHINE NATIVE WINDOWS ARM64 VALIDATION PASSED**
Date: 2026-09-06

Machine-readable oracle: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_ORACLE.json`.

Native validation evidence: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_NATIVE_ARM64_VALIDATION.md`.

## Authority

Parent `main` resolved before work:

`2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`

Approved Proposal 0.15 architecture commit:

`e1d0b4aea0f7ba29cf85e765bea33d14eab35fde`

Pre-handoff executable/audit checkpoint:

`74305ce3c501d5da0762bd88598d6bd83f627d14`

Expected handoff-only head resolved exactly as:

`0ab39bafd5c6a0ec94b9aa659627b5b7c68b80c3`

The two descendants after `74305ce3...` were confirmed documentation-only: the implementation handoff and its README index entry.

Resumed-audit executable/test checkpoint:

`81f199e51ea636c2cd9ad0350d815fa52dd6373f`

Initial native-validation checkout:

`a456a74ff6995ecfebc25a6134b2ca25d6260432`

That attempt exposed a Harness test-oracle compilation defect only: six `MSTEST0032` analyzer errors against assertions comparing literals to C# `const` fields. Core tests, native ARM64 Harness build, and both fixture smokes passed in that attempt. The failed attempt is preserved in `E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_NATIVE_VALIDATION_ATTEMPT_01.md`.

The required test-only correction produced the final validated executable/test checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

All descendants after that exact checkout created to record successful validation are evidence/documentation-only. The validated implementation/test bytes remain those at `3749210393282f6aa2ac4ceb0176b6adb5df189e`.

## Falsification

This implementation is not represented as validated unless the evidence identifies the exact tested checkout, the Director-machine architecture is ARM64 with .NET RID `win-arm64`, all Core and Harness tests pass, the Harness builds natively, both canonical fixture smokes pass, and pre/post tracked and staged trees remain clean.

The successful validation satisfies those conditions for the authorized fake/local scope.

## Resumed recursive-audit corrections

The resumed audit found and corrected two provider-edge defects without changing approved architecture:

1. malformed provider usage with `reasoning_tokens > output_tokens` could escape receipt construction instead of closing as a technical receipt; the parser now rejects the invalid usage shape before receipt construction, with focused wire coverage;
2. streaming success previously sourced semantic bytes from accumulated provisional `response.output_text.delta` events. Streaming deltas are now diagnostics only; the closed `response.completed.response.output` supplies successful semantic bytes. A focused oracle test proves a provisional delta cannot override the completed output.

The pre-existing malformed-usage wire test also asserted a diagnostic code the implementation could not produce; it now asserts the actual fail-closed `provider-response-incomplete` path.

Native validation then exposed one test-oracle construction defect: six compile-time-tautological assertions in `ApprovedEnvelopeConstants_AreExact`. The correction changed only the Harness test oracle so the exact approved constants are verified without `MSTEST0032`; no production constant, production source, Core source, fixture, or Proposal 0.15 behavior changed.

## Final zero-material-correction pass

One complete static pass after the provider-edge corrections and again after the test-only native-validation correction found no remaining material correction or worthwhile simplification within Proposal 0.15 scope.

Checked and clean:

- architecture authority and Proposal 0.15 scope;
- no `Ensemble.E0.Core` modification and no product Application/persistence/UI entry;
- Harness -> Core dependency direction;
- deterministic Access Control before Context Composition;
- Performer / Integrity / Interpreter disclosure bounds;
- prepared-attempt and closed-receipt configured-path association;
- cancellation/timeout ownership and zero automatic retries;
- spend reservation, observed-usage reconciliation and fail-closed overrun behavior;
- Integrity concern semantics and deterministic State Authority review rejection;
- accepted Take + consequence commit followed immediately by synchronized next Opportunity;
- evidence manifest, runtime seal, evaluation seal, replay/provenance material and blind-output separation;
- strict structured-output request shape and current official Responses streaming/input-token assumptions;
- Harness tests/reference oracles, simplicity and managed-code ARM64 suitability;
- the five project properties: Character continuity, bounded perspective, agency without hidden authorship, causal persistence and creator sovereignty.

Current official OpenAI documentation was checked only for volatile wire facts. No provider request, credential access or spend occurred.

## Director-machine native validation

Exact validated checkout:

`3749210393282f6aa2ac4ceb0176b6adb5df189e`

Director-machine environment:

```text
PROCESSOR_ARCHITECTURE=ARM64
Windows 10.0.26200
.NET SDK 9.0.317
RID win-arm64
Host 10.0.11
Host Architecture arm64
```

Results:

```text
Core tests:    622 / 622 passed; 0 failed; 0 skipped; exit 0
Harness tests:  39 /  39 passed; 0 failed; 0 skipped; exit 0
Harness build:  PASS; exit 0
Missing Raft:   PASS; exit 0
Generic smoke:  PASS; exit 0
Tracked tree:   clean before and after
Staged tree:    clean before and after
```

One unrelated pre-existing untracked file, `patch0012-local-edit.txt`, was reported. No untracked file existed under `src/`, `tests/`, or `fixtures/`.

`OPENAI_API_KEY` was explicitly absent for the validation gate. No provider request, credential use, or spend occurred.

## Conclusion

E0-A Phase B Reference Run Envelope Proposal 0.15 is statically audited and native-Windows-ARM64 validated for its approved fake/local implementation scope at exact executable/test checkout `3749210393282f6aa2ac4ceb0176b6adb5df189e`.

Real OpenAI provider execution remains a separate explicit Director credential/spend gate and is not authorized or claimed by this evidence.
