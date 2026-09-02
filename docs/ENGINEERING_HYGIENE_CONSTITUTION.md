# Ensemble Engineering Hygiene Constitution

Status: project law
Applies to: all engineering changes unless a stronger frozen specification explicitly overrides it

## Core rule
Every accepted implementation must leave the active codebase at least as coherent as the validated baseline it replaces. Functionality is not complete if it leaves avoidable architectural debt behind.

## Laws
1. **One canonical implementation.** A concept or operation should normally have one authoritative implementation. When a new implementation supersedes an old one, remove the old path unless a real compatibility obligation requires it.
2. **No imaginary compatibility debt.** Before Ensemble has shipped compatibility obligations, prefer correcting the canonical design over preserving obsolete internal formats, APIs, aliases, or adapters.
3. **Compare every patch with the previous validated baseline.** Review not only whether new code works, but whether naming, dependency direction, authority boundaries, testability, and total conceptual complexity remain coherent.
4. **Refactor inside the affected boundary when necessary.** Patch-first means avoid unrelated rewrites; it does not require layering workarounds on top of a design that the current patch proves should change.
5. **Delete obsolete code aggressively.** Git preserves history. The active tree should not preserve abandoned implementations, commented-out code, dead helpers, superseded DTOs, unused abstractions, or stale experiments "just in case."
6. **Abstractions must earn themselves.** Introduce an abstraction only for a demonstrated axis of variation or a constitutional boundary. Do not build speculative frameworks for hypothetical future use.
7. **Domain language is architecture.** Truth, claim, observation, knowledge, belief, suspicion, memory, possibility, pressure, performance, and consequence are not interchangeable names. Naming drift is treated as architectural drift.
8. **Make invalid authority difficult to express.** Prefer distinct immutable types and construction boundaries over mutable discriminator fields or convention-only rules.
9. **Keep deterministic logic pure where practical.** Validation, access rules, canonicalization, authority transitions, and other deterministic mechanisms should use explicit inputs and outputs without hidden clocks, randomness, mutable globals, network access, or implicit filesystem state.
10. **Infrastructure stays at the edges.** Core domain code should not depend on UI, provider APIs, Windows AI, NPU APIs, filesystem concerns, or process orchestration unless the domain contract genuinely requires it.
11. **Warnings remain errors.** Do not normalize warning debt or defer known compiler hygiene issues without an explicit blocking reason.
12. **No TODO graveyard.** Deferred work must point to a named phase/boundary or remain outside the active source entirely.
13. **Validation levels never inflate.** Static review, compiler validation, runtime validation, hardware/NPU evidence, package validation, and Store certification remain distinct authorities.
14. **Earlier investment creates no preservation right.** If later evidence proves an earlier implementation wrong and no external compatibility obligation exists, correct or replace it instead of creating a legacy adapter.

## Patch Hygiene Gate
Every patch is reviewed in this order:

Correctness -> Consistency -> Authority -> Scope -> Tests -> Simplicity -> Hygiene -> ARM64 suitability -> Vision -> Evidence

The Hygiene gate asks:
- Did this introduce duplicate concepts or implementations?
- Did anything become obsolete and therefore deletable?
- Did naming or domain semantics drift?
- Did dependency direction worsen?
- Is there now a simpler representation than the inherited one?
- Did we add a generalization without a demonstrated second use case?
- Is there still one obvious canonical path?
- Would a new engineer know which implementation is authoritative?
- Did conceptual complexity grow only because capability genuinely grew?

A patch may compile and pass tests and still fail this gate.

## Convergence audits
Perform broader cleanup/deletion audits at meaningful checkpoints:
- end of H1 deterministic spine
- end of E0-A harness
- E0 convergence review
- first Windows application architecture baseline
- Alpha
- Beta/release architecture gate

Each audit must actively search for code and abstractions to remove, not only code to add or improve.
