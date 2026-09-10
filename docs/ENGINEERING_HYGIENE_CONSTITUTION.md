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

## Durable architecture / capability prohibitions

These rules are absorbed from the former Engineering personality prompt so fresh chats no longer depend on that prompt.

### Provider/model neutrality

**No named provider/model becomes a Core or Application architecture dependency.** Capability contracts and model-neutral domain/application state remain the default.

A user-selected Performer/provider/model name may be displayed as data where product/design law permits. Display does not license binding the Character/domain architecture to that provider/model, and provider identity must not become persistent Character identity by accident.

Provider-specific adapters/infrastructure may name the provider because that is the edge they implement; the dependency must not leak inward across the architectural boundary.

### Preview/experimental APIs

No preview/experimental API becomes a launch dependency without explicit Director authority. When proposing one, verify its **current** channel/status and actual intended scenario at that moment.

Do not preserve an experimental dependency merely because earlier work invested in it.

### No assumed local-AI/NPU task

Do not assume Ensemble requires a local AI/NPU workload merely because target hardware has an NPU or a Windows AI surface exists. Local/NPU routing must be earned by product/experiment evidence and applicable ODR/architecture authority.

Unsupported/private Windows APIs remain fail-closed and outside the retail baseline.

### MCP / App Actions

MCP/App Actions remain non-blocking for the first release unless stronger current authority explicitly changes that boundary. Do not turn optional extensibility into a launch dependency because an SDK/tool makes it possible.

### API semantic verification

**No API is adopted on the strength of its name or plausible branding.** Before an API becomes a dependency, gate, or plan item:

1. read its current documentation/contract;
2. state the scenario it was designed for;
3. verify that scenario matches Ensemble's need;
4. identify preview/experimental/support status;
5. distinguish marketing/category similarity from actual capability.

An audit that proves an API exists but not that it solves the required scenario is incomplete.

### `InvariantGlobalization`

**Never set `InvariantGlobalization=true` while the canonicalization/hash identity layer depends on Unicode normalization via `string.IsNormalized`/NFC behavior.**

Invariant globalization can make that normalization path fail and therefore threatens load-bearing canonical hash identity. Any future proposal to enable it must first prove the canonicalization contract no longer depends on unsupported normalization behavior and must receive the appropriate architecture authority.

## Validation / claim discipline

Never claim compilation, runtime behavior, NPU execution, measured hardware performance, WACK success, Store/Partner Center certification, provider success, or another validation rung that was not actually observed at the authoritative environment/gate.

A model/executor statement that tests "should pass" is not test evidence.

Browser/CDP evidence is browser evidence. Cross-compilation is compiler evidence. Native ARM64 target-device evidence remains separate.

Provider invocation/spend is an external execution event and requires current authority regardless of which agent/tool can technically send it.

## Test discipline

Behavioral tests and reference oracles gate the build according to current architecture/phase law.

### Reflection/public-surface rule

Discriminate reflection tests by **member visibility and what they assert**, not filename.

Keep public-surface architecture assertions such as:

- a type exposes zero public constructors;
- only these public properties/methods are available;
- an external caller cannot express an invalid state through the public contract.

These encode observable architectural law and can survive correct internal refactors.

Quarantine/remove tests that inspect private members, method bodies, IL, call graphs, or internal implementation sequencing merely to freeze the current implementation. A test that breaks on a correct refactor because it asserted private structure is a test defect.

Reflection used only as plumbing to construct an internal/non-public test object is not itself an architectural assertion and may remain when justified.

Filenames are not a reliable proxy. Classify the assertion by reading it.

### Reference oracles

Preserve reference-oracle assertions that encode durable externally meaningful/canonical behavior. If an oracle must change, require the authority/evidence that changed the underlying contract rather than casually updating expected output to make a test green.

## Patch / deliverable discipline

### Patch-first

Diagnose the smallest affected surface. For a failure:

`preserved evidence -> falsification criterion -> root cause or bounded hypotheses -> smallest justified correction/no-patch result -> targeted verification -> next authority/machine gate`.

Do not change runtime source merely because a failure occurred. Establish the defect first.

### Falsification first

Before proposing a patch, be able to state what observation would prove the patch unnecessary. If the evidence can still support a no-source-defect explanation, investigate before mutating source.

### Documentation-only work

A substantive implementation work package should not drift into documentation-only output merely because documents are easier to produce than executable evidence. Valid closure should normally include source/test/executable evidence or a specific consequential Director decision.

Documentation-only packages are valid when the requested objective is itself documentation, authority/continuity/orchestration repair, evidence analysis, or another genuinely document-native deliverable.

### Artifact size discipline

Keep `CURRENT_STATE.md` at or below the repository's enforced **3 KiB** cap.

For future bounded patch documentation, prefer a compact architecture/decision contract and one compact evidence record rather than multiplying stage documents. Historical personality guidance used 15 KiB blueprint / 8 KiB evidence targets; treat those as strong scoping targets for new patch-native artifacts when applicable, not a reason to rewrite historical evidence or violate a stronger frozen record format.

If a patch cannot be made understandable without a sprawling document set, reconsider whether the patch boundary is too broad.

### Deletion discipline — no arbitrary quota

The former personality prompt required every third patch to delete something. The durable repository law preserves the intent but **does not preserve that numeric quota**, because forcing unrelated deletion on an arbitrary patch count conflicts with patch-first/scope discipline.

Instead:

- delete anything made obsolete inside the affected boundary in the same patch;
- actively search for deletions/simplifications during Patch Hygiene Gate;
- perform repository/code/document deletion audits at the convergence checkpoints below.

## Repository surface

Law 5 extends from the active source tree to the repository surface. Git history may preserve what is no longer active; active branch and evidence namespaces must not impersonate current work.

- **Branch lifecycle.** A branch becomes archivable when it has no remaining implementation, review, validation, or Director-disposition role because it was merged, superseded, rejected, or intentionally abandoned. Before deleting the branch ref, replace it with an annotated `archive/...` tag at the branch's final head recording former branch name, disposition, and archive date. Active branches are moving work surfaces, not long-term history storage.
- **Trunk convergence.** `main` should return to the latest coherent accepted project baseline at meaningful gates. Do not normalize a work branch remaining hundreds of commits ahead after decisions converge; reconcile unique side-branch work, preserve archive/validation tags, then restore a trustworthy trunk.
- **State currency.** On a branch where `CURRENT_STATE.md` is operative, no more than three commits may follow its last modification. CI enforces this bounded distance. It does not require `CURRENT_STATE.md` to contain/equal HEAD, which would create self-referential churn.
- **Validated checkouts.** Every checkout first named in `CURRENT_STATE.md` as machine-validated must already have, or receive at that moment, an annotated tag at that exact commit. Tag message records validation level, scope, and evidence document. A prose SHA may supplement that tag but must not be the sole durable locator.
- **Evidence surface.** `docs/evidence/` may contain current authority/history, but roles must be explicit. Evidence remaining authoritative must have an inbound reference from `CURRENT_STATE.md` or another current authoritative document. Superseded/failed/partial/closed-checkpoint evidence that no longer informs current authority belongs under `docs/evidence/archive/`. Archive evidence preserves provenance but never implies current validation; unreferenced evidence is presumed archive material unless its continuing role is stated.
- **Repository lanes.** Engineering work branches exist only in `Rylascoo/Ensemble-Project`; Design/website branches only in `Rylascoo/Ensemble-Website`. Cross-lane decisions may be referenced, but branch refs do not cross repositories.
- **Linked-worktree ownership.** Filesystem placement does not transfer repository ownership. For nested/cross-lane worktrees, the Git common directory establishes ownership; inspect branch/HEAD, cleanliness including untracked files, remote state, and unique work before relocation/removal, and use owning-repo worktree machinery rather than raw deletion.
- **Artifact residency.** Apply `docs/REPOSITORY_RESIDENCY.md`: Engineering and central product/policy/orchestration records live here; Design-native UI/website/visual assets and Design-specific evidence live in `Ensemble-Website`/Drive. Never move an unclear artifact merely because subject matter looks visual or technical.

This section creates no separate product approval gate.

## Engineering artifact lifecycle

- **Smallest sufficient patch record.** Future patch documentation normally uses one compact architecture/decision contract, one evidence record, and a machine-readable oracle only when objective oracle values warrant one. Do not multiply documents merely to mirror workflow stages.
- **Handoffs are temporary.** A handoff is optional and active only when `CURRENT_STATE.md` names its exact `docs/handoff/...` path. Long personality/handoff prompts are not repository authority.
- **Historical blueprints may remain historical.** A blueprint can remain under `docs/blueprint/` for provenance without becoming current merely by existing or appearing in a navigation index.
- **Do not normalize history cosmetically.** Do not mass-rewrite frozen/historical artifacts to add generic status headers/disclaimers/current terminology. Make only narrow corrections needed for a present ambiguity.
- **Navigation is not promotion.** Indexes aid discovery but cannot confer phase/architecture/product/policy/validation authority.
- **One canonical cross-lane copy.** A current artifact with canonical home in the other lane is referenced, not duplicated. Migration preserves provenance and updates inbound links before a misplaced active copy is removed.

## Patch Hygiene Gate
Every patch is reviewed in this order:

Correctness -> Consistency -> Authority -> Scope -> Tests -> Simplicity -> Hygiene -> ARM64 suitability -> Vision -> Evidence -> Branch lifecycle -> Residency -> Continuity

The Hygiene gate asks:
- Did this introduce duplicate concepts or implementations?
- Did anything become obsolete and therefore deletable?
- Did naming/domain semantics drift?
- Did dependency direction worsen?
- Is there now a simpler representation than inherited?
- Did we add a generalization without a demonstrated second use case?
- Is there still one obvious canonical path?
- Would a new engineer know which implementation is authoritative?
- Did conceptual complexity grow only because capability genuinely grew?
- Did this create a stale branch, unexplained current document, duplicate cross-lane authority, or agent/automation authority leak?
- Did a provider/model/tool-specific detail leak inward across an architectural boundary?
- Did the patch make an unsupported validation or external-execution claim?

A patch may compile/pass tests and still fail this gate.

## Convergence audits
Perform broader cleanup/deletion audits at meaningful checkpoints:
- end of H1 deterministic spine;
- end of E0-A harness;
- E0 convergence review;
- first Windows application architecture baseline;
- Alpha;
- Beta/release architecture gate.

Each audit must actively search for code, branches, documents, duplicated authority, obsolete tooling/agent protocol, and abstractions to remove/reclassify—not only things to add/improve.

One complete recursive pass with no material correction or worthwhile simplification is the stop condition.