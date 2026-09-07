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

## Repository Surface
Law 5 extends from the active source tree to the repository surface. Git history may preserve what is no longer active; active branch and evidence namespaces must not impersonate current work.

- **Branch lifecycle.** A branch becomes archivable when it has no remaining implementation, review, validation, or Director-disposition role because it was merged or squash-merged, superseded, rejected, or intentionally abandoned. Before deleting the branch ref, replace it with an annotated `archive/...` tag at the branch's final head recording the former branch name, disposition, and archive date. Active branches are moving work surfaces, not long-term history storage.
- **Validated checkouts.** Every checkout first named in `CURRENT_STATE.md` as machine-validated must already have, or receive at that moment, an annotated tag at that exact commit. The tag message records validation level, scope, and the supporting evidence document. A prose SHA may supplement that tag but must not be the sole durable locator.
- **Evidence surface.** `docs/evidence/` may contain both current authority and history, but their roles must be explicit. Evidence that remains authoritative must have an inbound reference from `CURRENT_STATE.md` or another current authoritative document. Superseded, failed, partial, or closed-checkpoint evidence that no longer informs current authority belongs under `docs/evidence/archive/`. Archive evidence preserves provenance but never implies current validation; unreferenced evidence is presumed archive material unless its continuing role is stated.
- **Repository lanes.** Engineering work branches exist only in `Rylascoo/Ensemble-Project`; website and visual-design work branches exist only in `Rylascoo/Ensemble-Website`. Cross-lane decisions may be referenced across repositories, but branch refs do not cross repositories.

This section extends Law 5; it creates no separate workflow or approval gate.

## Engineering artifact lifecycle

- **Smallest sufficient patch record.** Future patch documentation normally uses one compact architecture/decision contract, one evidence record, and a machine-readable oracle only when objective oracle values warrant one. Do not multiply documents merely to mirror workflow stages.
- **Handoffs are temporary.** A handoff is optional and active only when `CURRENT_STATE.md` names its exact `docs/handoff/...` path as the live transition artifact. Remove completed handoffs from the active tree after convergence; Git history preserves them.
- **Historical blueprints may remain historical.** A blueprint can remain under `docs/blueprint/` for architectural provenance without becoming current merely by existing there or appearing in a navigation index. Current authority still derives from the project authority chain.
- **Do not normalize history cosmetically.** Do not mass-rewrite frozen or historical artifacts to add generic status headers, disclaimers, or current terminology. Make only narrow corrections needed to resolve a present ambiguity.
- **Navigation is not promotion.** Document indexes may help discovery but cannot confer phase, architecture, product, policy, or validation authority.

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
