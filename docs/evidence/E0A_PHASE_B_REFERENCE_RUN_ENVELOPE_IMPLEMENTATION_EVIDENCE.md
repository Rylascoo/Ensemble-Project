# E0-A Phase B — Reference Run Envelope Implementation Evidence

Status: **STATIC IMPLEMENTATION AUDIT CLEAN — NATIVE WINDOWS ARM64 VALIDATION PENDING**
Date: 2026-09-05

Machine-readable oracle: `docs/evidence/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_ORACLE.json`.

## Authority

Parent `main` resolved before work:

`2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`

Approved Proposal 0.15 architecture commit:

`e1d0b4aea0f7ba29cf85e765bea33d14eab35fde`

Pre-handoff executable/audit checkpoint:

`74305ce3c501d5da0762bd88598d6bd83f627d14`

Expected handoff-only head was resolved exactly as:

`0ab39bafd5c6a0ec94b9aa659627b5b7c68b80c3`

The two descendants after `74305ce3...` were confirmed documentation-only: the implementation handoff and its README index entry.

Resumed-audit executable/test checkpoint:

`81f199e51ea636c2cd9ad0350d815fa52dd6373f`

Commits after that checkpoint in this evidence pair are documentation/oracle only. The final Director-machine validation checkout is the resolved branch head after this evidence commit; no executable change may be inserted before validation without reopening the audit.

## Falsification

This implementation is not ready for promotion if the Director-machine checkout differs from the resolved validation commit, if any Core file differs from `main`, if the Core or Harness tests fail, if the Harness fails native ARM64 build/smoke, or if either canonical fixture fails validation.

## Resumed recursive-audit corrections

The resumed audit found and corrected two provider-edge defects without changing approved architecture:

1. malformed provider usage with `reasoning_tokens > output_tokens` could escape receipt construction instead of closing as a technical receipt; the parser now rejects the invalid usage shape before receipt construction, with focused wire coverage;
2. streaming success previously sourced semantic bytes from accumulated provisional `response.output_text.delta` events. Streaming deltas are now diagnostics only; the closed `response.completed.response.output` supplies successful semantic bytes. A focused oracle test proves a provisional delta cannot override the completed output.

The pre-existing malformed-usage wire test also asserted a diagnostic code the implementation could not produce; it now asserts the actual fail-closed `provider-response-incomplete` path.

## Final zero-material-correction pass

One complete pass after those corrections found no remaining material correction or worthwhile simplification within Proposal 0.15 scope.

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

## Native validation gate

Native compiler/test/runtime authority remains **PENDING**. It must be produced on the Director's Windows ARM64 machine against one exact resolved branch head.

Required validation set:

1. verify exact branch/head and clean tracked/staged tree;
2. record Windows/ARM64/.NET environment;
3. run the complete existing Core test project;
4. run the new Harness test project;
5. build the Harness natively for its `win-arm64` target;
6. run Missing Raft fixture smoke with `--no-build`;
7. run generic fixture smoke with `--no-build`.

Do not provide `OPENAI_API_KEY`; do not execute the reference provider path. Provider credentials and spend remain a separate explicit Director gate.

Any resulting evidence must be labeled **Director-machine-sourced** and must record the exact checkout plus actual test totals rather than predicted totals.
