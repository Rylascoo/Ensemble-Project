# Creator Ontology Extensibility Guard

Status: STRONG DIRECTION / OPEN DESIGN GUARD — not a frozen final product schema
Recorded: 2026-09-02

## Purpose

Preserve Kymaean/Ensemble's creative extensibility while the deterministic E0 reference ontology is still deliberately explicit and testable.

## Guard

The E0 reference categories used today — including Knowledge, Beliefs, Suspicions, Memories, Goals, Relationships, Pressures, Circumstance, and related UI language — are **not** authority to assume that the final creator-facing dramatic ontology must be a closed set of engine enums.

The product should preserve a small fixed deterministic vocabulary only where semantic distinctions materially affect:

- truth and proposition authority;
- access/disclosure/privacy;
- provenance;
- causal mutation/commit;
- observation eligibility;
- persistence/replay correctness;
- other hard engine authority boundaries.

Creator-authored dramatic concepts should remain capable of expressing ideas the engine did not pre-enumerate, such as an invented obligation, taboo, ritual, emotional tension, social convention, private vow, omen, professional rivalry, cultural rule, or other narrative concept.

## UI vocabulary is not automatically storage ontology

A surface may use a legible theatrical label such as:

- “What Wren suspects”;
- “What Wren is holding back”;
- “Pressures”;
- “Trust is fraying”;

without that label automatically requiring a permanent one-to-one database type or engine enum.

Future architecture should evaluate whether each creator-facing concept is:

1. a true engine-level authority primitive;
2. a creator-defined record expressed through a more general primitive;
3. a UI projection over authoritative data;
4. or a justified combination.

## E0 preservation

Do **not** generalize the validated E0 ontology merely to satisfy this guard.

The current explicit E0 categories are useful because they make the reference experiment deterministic, inspectable, and auditable. Premature replacement with a generalized meta-ontology would add abstraction before evidence.

Therefore:

- existing frozen E0 contracts remain unchanged;
- Patch 0004/0005/0006/0007 semantics remain authoritative for E0;
- this guard does not retroactively reclassify `Suspicion`, `Belief`, `Memory`, `Goal`, `Pressure`, or other current records;
- no implementation work is authorized by this document alone.

## Required revisit

Revisit this guard before freezing either of the following into durable product architecture:

- the State Interpreter's broad candidate-mutation ontology;
- the post-E0 durable Production-state / creator-facing Studio ontology.

At that point, prefer the smallest fixed authority vocabulary that preserves correctness while leaving dramatic meaning extensible.

## Design principle

> Keep authority semantics precise; keep creative semantics open.
