# H1 Patch 0005 — Independent Context Reference Oracle

Date: 2026-09-02
Status: pre-machine-validation reference evidence
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

The semantic input was constructed independently from the approved Patch 0004 Voss access set and Patch 0005 canonical property/order rules. No production Context canonicalizer or renderer was used to derive the expected values.

## Independent derivation
Two separate reference serialization paths were used:

1. an independently constructed ordered semantic object serialized as minified UTF-8 JSON with non-ASCII scalar preservation;
2. a manual explicit canonical JSON emitter implementing the approved property order and ECJ-1 scalar/string escaping rules.

The two paths produced byte-identical structured content and byte-identical rendered-envelope content.

## Frozen Voss reference values
Structured canonical UTF-8 length:
`2569` bytes

StructuredContextHash:
`ce172d8d6a6aedd2c24465c1011ea14466a405f2476031d72a84b62266e126be`

ContextPacketId:
`CTX:ce172d8d6a6aedd2c24465c1011ea14466a405f2476031d72a84b62266e126be`

TrustedStateText UTF-8 length:
`1696` bytes

Rendered canonical envelope UTF-8 length:
`1905` bytes

RenderedContextHash:
`b116d7264c50e22083e250605336c28eb6f2da12475748c495eecc73a5ce89b5`

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
- Marlowe: Voss respects Marlowe's competence but grants him less benefit of the doubt on unilateral choices.
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

## Promotion rule
Production tests may use the frozen values in this document as an oracle. Executable promotion still requires native Windows ARM64 build, complete Core tests, Missing Raft runtime regression, generic smoke runtime regression, and final static/hygiene/scope review.