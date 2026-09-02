# H1 Patch 0005 — Independent Context Reference Oracle

Date: 2026-09-02
Status: corrected pre-machine-validation reference evidence
Purpose: record independently derived Missing Raft / Voss Context identities before executable promotion.

## Authority level
This document records independent reference derivation and static/adversarial evidence only. It is **not** Windows ARM64 compiler, test-execution, runtime, NPU, WACK, or Store evidence.

## Reference input
Canonical fixture:
`fixtures/missing-raft/missing-raft-0.1.0.json`

Reference subject/current opportunity:
`VOSS`

Approved contracts:
- Context schema: `ensemble.e0.context.v1`
- composition: `ensemble.e0.context.full-authorized.v1`
- rendering: `ensemble.e0.context.render.v1`

The semantic input is derived directly from the canonical Patch 0004 Voss access projection and Patch 0005 canonical property/order rules. No production Context canonicalizer or renderer is used to derive the expected values.

## Correction after first native test gate
The first native ARM64 Patch 0005 test run at executable head `44efd6358d8a94e769eee2ff9b2c7bd4715f5587` compiled successfully and discovered all 115 tests, but 2 tests failed.

Static comparison isolated both failures to this oracle/test fixture transcription, not production Composer behavior. The canonical Missing Raft fixture defines `REL-VOSS-MARLOWE` as:

`Voss respects Marlowe's perception but grants him less benefit of the doubt on unilateral choices.`

The original independent oracle had accidentally substituted `competence` for canonical `perception`. Because both words contain ten ASCII bytes, the structured and rendered byte-length expectations remained correct while both SHA-256 values differed. The exact-render expectation therefore failed for the same single transcription error.

The canonical fixture is authoritative. Production code remains unchanged. This document and the corresponding test expectations are corrected to the fixture text.

Corrective test/evidence head prepared for native rerun:
`f5aa4f000bfa3f7f0cc3c432df6b5638f7f2c774`

The corrective delta from the first machine-tested head contains only this evidence file and `tests/Ensemble.E0.Core.Tests/Context/DeterministicContextComposerTests.cs`; no production source changed.

## Corrected independent derivation
Two independent reference serialization paths were rerun using the exact canonical fixture text:

1. an independently constructed ordered semantic object serialized as minified UTF-8 JSON with non-ASCII scalar preservation;
2. a manual explicit canonical JSON emitter implementing the approved property order and ECJ-1 scalar/string escaping rules.

The two paths again produced byte-identical structured content and byte-identical rendered-envelope content.

## Corrected frozen Voss reference values
Structured canonical UTF-8 length:
`2569` bytes

StructuredContextHash:
`bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`

ContextPacketId:
`CTX:bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b`

TrustedStateText UTF-8 length:
`1696` bytes

Rendered canonical envelope UTF-8 length:
`1905` bytes

RenderedContextHash:
`ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88`

## Exact Voss TrustedStateText
No trailing LF is present.

```text
[WHO YOU ARE]
Name:
- Dr. Voss
Constitution:
- Consequential decisions should withstand explicit examination; Voss seeks reasons under uncertainty.
Disposition:
- Voss distinguishes observation, inference, and assumption; she may clarify at length.

[WHAT IS HAPPENING]
- No storm, rescue timer, or other external countdown is active at Scene opening.
- There is no immediate emergency at Scene opening.
- The group has limited provisions.
- The raft is gone at Scene opening.

[RIGHT NOW]
- none

[WHAT YOU OBSERVED]
- none

[WHAT YOU KNOW]
- Voss knows that the current has strengthened.

[WHAT YOU BELIEVE]
- Voss believes an accidental raft loss is plausible given the stronger current, but not certain.

[WHAT YOU SUSPECT]
- none

[WHAT YOU REMEMBER]
- Voss remembers Marlowe moving supplies threatened by the rising tide before agreement, no harm resulting, and her objection to the act-first, explain-later pattern.
- Voss remembers Wren limiting her statement about the misplaced tool to observation, Voss pressing toward a firmer conclusion, Wren refusing to overstate, and Marlowe supporting that boundary.

[WHO IS PRESENT]
- Marlowe
- Dr. Voss
- Wren

[RELATIONSHIPS]
- Marlowe: Voss respects Marlowe's perception but grants him less benefit of the doubt on unilateral choices.
- Wren: Voss respects Wren's observational care and expects Wren to withhold conclusions when pressed.

[WHAT YOU WANT]
- Establish enough reliable shared information for the next consequential decision to be open and accountable.

[PRESSURES]
- The raft is gone and provisions are limited, making indefinite inaction untenable without creating an immediate emergency or prescribing any Character response.
```

## Rendered envelope contract
The rendered identity is SHA-256 over canonical JSON with this exact property order:

```json
{"renderingContract":"ensemble.e0.context.render.v1","trustedStateText":"...","recentPerformanceText":"","opportunityText":"You have the current opportunity to act."}
```

## Existing fixture identity preservation requirement
Patch 0005's shared canonical JSON primitive extraction must leave Missing Raft ECJ-1 exactly unchanged:

- canonical fixture bytes: `9112`
- fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`

## Validation record so far
At head `44efd6358d8a94e769eee2ff9b2c7bd4715f5587` on the user's native Windows ARM64 machine:
- Core/Harness build: PASS;
- tests discovered: 115;
- tests succeeded: 113;
- tests failed: 2, both traced to the stale oracle transcription above;
- Missing Raft Harness regression: PASS/0;
- generic smoke Harness regression: PASS/0.

These results do not yet establish Patch 0005 executable completion. A rerun at the corrected test head is required.

## Promotion rule
Production tests use the corrected values in this document as the oracle. Executable promotion still requires native Windows ARM64 build, complete Core tests, Missing Raft runtime regression, generic smoke runtime regression, and final static/hygiene/scope review.