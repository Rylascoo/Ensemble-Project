# KYMAEAN PROJECT REASONING + TASK-SCOPE OPTIMIZATION PROTOCOL

Status: ACTIVE CANONICAL CROSS-PROJECT WORKFLOW LAW

Protocol version: 0.4

Date: 2026-09-09

## 1. Purpose

For the remainder of Ensemble/Kymaean project creation, each Director turn and routed agent work package should be deliberately scoped so the strongest appropriate reasoning/execution surface is used efficiently.

The optimization target is:

> **Maximum useful reasoning, synthesis, falsification, and completed dependency closure per Director turn — not maximum response length, token use, code volume, artifact count, model prestige, or number of simultaneous objectives.**

Per-reply token-value law:

> **Whenever applicable, every fresh or continuing project chat should maximize useful value per reply: useful reasoning, synthesis, verification, and dependency closure per returned token and Director turn — not artificial verbosity or capacity consumption.**

This protocol changes collaboration granularity and execution allocation only. It does not alter product architecture, repository authority, validation authority, frozen design methods, provider authorization, implementation boundaries, or Director creative/product authority.

`docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md` defines role/dispatch authority. This protocol defines how to allocate reasoning/execution within that authority.

### Anti-churn clarification

For this project, `churn` primarily means:

- unnecessary clarifying questions when project authority or safe defaults already resolve the issue;
- repeated requests to approve intermediate work already inside an approved objective/delegation;
- returning to the Director merely because one internal subartifact, critique, packet, patch, or audit stage is complete;
- splitting one causal work unit across turns without a real authority/external-dependency reason;
- asking the Director to compose a Codex/Claude/cross-lane prompt that the owning Sol/Administrator can specify;
- using the Director as manual middleware for repository facts an agent can resolve.

It does **not** mean `do less work per turn`.

Governing interpretation:

> **Do more of the already-authorized coherent work before returning; ask fewer unnecessary questions; stop only when Director/external input is consequential or execution cannot continue safely.**

## 2. Mandatory task-scope optimization

Before a substantive task, choose the largest scope that remains one coherent, falsifiable objective and can be completed with current authority, evidence, and tools.

A well-optimized scope should normally:

1. resolve current authoritative checkpoint/ref(s);
2. identify the actual defect, decision, artifact, experiment, or outcome;
3. include directly dependent reading/synthesis;
4. include logically coupled implementation/design/artifact work that would otherwise create artificial approval fragmentation;
5. include contradiction, edge-case, and falsification analysis;
6. use an external executor/reviewer only when it adds material capability/information;
7. recursively audit completed work until one full pass finds no material correction/worthwhile improvement in scope;
8. update durable project state when a meaningful checkpoint changes;
9. continue already-authorized downstream operations;
10. stop at the next **genuinely consequential** Director, architecture, implementation, security, provider/spend, runtime, WACK, Store, external-dependency, or validation gate.

A `gate` is not consequential because a document labels it a gate. It is consequential when proceeding would require new authority, a creative/product choice, architecture/scope change, provider/spend event, user-machine execution, external evidence, or another dependency that cannot be completed safely from current authority.

## 3. Manager / executor / reviewer allocation

### 3A. Accountable Sol manager surfaces

Engineering Sol and Design Sol remain the primary reasoning/authority-reconciliation surfaces for their lanes.

At this protocol checkpoint, the default project-manager surface is **GPT-5.6 Sol High**. Exact model labels are volatile; `docs/AGENT_TOOLING_CAPABILITY_SNAPSHOT.md` carries current recommendations.

Use the owning Sol for:

- source/authority reconciliation;
- architecture/dependency reasoning;
- creative/design method reasoning;
- root-cause/hypothesis distinction;
- provider/security/privacy/accessibility/ARM64 implications;
- test strategy and regression reasoning;
- interpreting machine/browser/render/external-review evidence;
- deciding what should *not* change;
- recursive audit;
- final in-lane adoption of executor/reviewer findings.

Do not spend High reasoning merely producing more prose, files, variants, or broader scope without a dependency reason.

### 3B. Codex — execution environment, not authority

Codex may be the normal execution surface for bounded work when local repository/worktree access, browser/CDP, tests, scripts, computer use, or other mechanical tooling provides material fidelity/efficiency.

Appropriate uses include:

- implementation of an already-resolved patch/work package;
- targeted refactors with tests;
- repository-wide mechanical census/reference graphs;
- branch/worktree mechanics;
- deterministic browser packet materialization/preflight;
- reproducible evidence collection;
- local toolchain/runtime investigation where available.

Codex must receive exact baseline/scope/non-goals/evidence/stop conditions and return evidence to the owning Sol/Administrator.

Do not conflate `Codex` with `Astra`.

### 3C. GPT-6 Astra — scarce Codex specialist

GPT-6 Astra is a scarce high-capability model allocation inside Codex. Use it when the stronger model materially improves expected reliability or information gain.

High-value cases include:

1. difficult full-checkout/codebase-scale audits;
2. complex browser/runtime/computer-use diagnosis;
3. major cross-cutting transformation where stronger reasoning materially reduces risk;
4. deliberately independent high-capability falsification where context contamination/confirmation bias matters.

Do not use Astra simply because Codex is being used, the repository is large, or work is tedious.

Optimize for highest-value information gain per scarce task, not maximum session usage.

### 3D. Independent review / Claude compatibility

A genuinely independent reviewer can add value for:

- consequential architecture/security review;
- alternate diagnosis of difficult failures;
- regression/falsification review of substantial patches;
- consequential design-method critique;
- milestone/release integrity review.

Independence is not ritual. R0/R1 work normally does not require a second model.

Until the dedicated Claude setup is complete:

- do not assume automated Claude connectivity;
- do not silently invoke programmatic Claude use;
- owning Sol/Administrator may generate a complete manual relay packet when independence is justified;
- returned Claude/other independent-review findings remain advisory until reconciled by the owning Sol.

### 3E. Administrator allocation

The future Ensemble Project Administrator normally handles operational triage/routing/ref/queue/CI/worktree/dispatch mechanics, not product/architecture/design adjudication.

The Administrator should use the lowest-cost current model/reasoning configuration that preserves required quality and escalate only when task complexity warrants it. Current recommendations live in the tooling snapshot.

The Administrator may dispatch multiple independent mechanics in parallel. It must serialize authority-sensitive integration and dependency-ordered work.

## 4. External work-package contract

Before any Codex/Astra/Claude Code/other executor invocation, define:

- one bounded question/transformation;
- authoritative repository/ref baseline;
- exact allowed files/surface or bounded search domain;
- expected machine-checkable output/evidence where practical;
- explicit non-goals/prohibited mutations;
- validation/test/browser requirements;
- failure/stop conditions;
- evidence/provenance returned;
- authority that remains outside the executor.

Do not combine unrelated audits to fill a scarce session.

Whenever the Director must manually invoke an external surface, provide:

```text
SURFACE: <surface>
MODEL: <current recommended model>
REASONING/EFFORT: <current level>
MODE: <read-only | implement | browser | execute | review | other>
USAGE CLASS: <normal | scarce>
```

followed by one complete paste-ready prompt.

Verify current model/effort/tool availability before dispatch. Model menus are volatile execution metadata, not constitutional law.

## 5. Review / independence classes

Follow `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`:

- **R0 mechanical** — executor self-check + machine validation/CI;
- **R1 ordinary** — executor + owning Sol recursive audit;
- **R2 consequential** — owning Sol + one independent reviewer when separation adds material information;
- **R3 critical** — owning Sol + independent reviewer + optional scarce specialist falsification;
- **R4 Director** — evidence reconciled first, then Director decision.

Review class never lowers provider/security/native validation or other external gate requirements.

## 6. Scope-shaping laws

### Semantic coupling over size

Combine work because pieces depend on one another, not because a larger reply appears more capable.

### Closure over fragmentation

If source synthesis, a small correction, targeted verification, evidence/state update, and next already-authorized operation are one causal unit, complete them together.

### Questions are a last resort

Do not ask the Director to choose/restate information when:

- authoritative GitHub/Drive/project evidence already answers it;
- approved phase/proposal supplies authority;
- it is an internal method choice without product/creative authority consequence;
- a safe deterministic default can be used/documented;
- next step is archival/audit/packet preparation/targeted verification;
- an owning Sol/Administrator can construct the external prompt itself.

Ask only when proceeding would invent a material requirement, cross a real authority boundary, or risk a wrong irreversible/external action.

### Consequential gates remain explicit

Do not silently cross:

- major creative convergence/identity selection;
- frozen architecture redesign;
- implementation authorization when only design/architecture is approved;
- new product scope;
- security-sensitive authorization;
- provider request/spend authorization;
- native runtime/hardware validation;
- WACK/Store certification;
- another gate owned by Director/user machine/external validator.

### Standing continuation

Once a bounded objective is authorized, complete downstream logically coupled internal work until a consequential boundary.

Examples:

- compiler/runtime evidence -> diagnose -> smallest patch -> targeted tests -> checkpoint -> next native gate;
- frozen design method -> materialize -> deterministic preflight -> Design Sol interpretation/audit -> next subjective/authority gate;
- repository drift -> classify -> deterministic repair -> validation -> closeout.

### Frozen-method / evidence discipline

Do not change frozen criteria, fixtures, carriers, failure rules, reroll/retune law, provider route, exact executable, or review sequencing after result visibility unless stronger authority opens a new program/amendment.

A failed experiment may fail. Do not rescue it by changing its contract after seeing the result.

### Simple tasks stay simple

A status check, exact lookup, narrow correction, or confirmation should remain compact even when High reasoning is available.

### No background promises

Complete available work in the current turn. If a hard external gate prevents completion, report the exact boundary rather than promising future background work.

## 7. Default task patterns

### Engineering compiler/runtime failure

`current authority -> preserved failure evidence -> falsification sentence -> root cause/hypotheses -> smallest justified patch or no-patch conclusion -> targeted regression/static checks -> checkpoint if changed -> next native/external gate`

Do not modify runtime source before evidence establishes a defect.

### Provider/external execution failure

`authorization state -> preserved terminal evidence -> separate runtime facts from hypotheses -> classify failure -> decide whether source/config/provider condition is implicated -> patch only if evidence warrants -> no retry/new provider traffic without new authority`

### Architecture proposal

`authority -> unresolved question -> alternatives -> implications -> adversarial/falsification pass -> recursive audit -> recommendation -> authorized follow-through -> consequential Director gate`

### Deterministic design/browser work

`frozen method -> exact execution contract -> Codex/browser/materialization when useful -> machine evidence -> Design Sol interpretation -> recursive audit -> continuity`

### Visual exploration

`durable laws -> sterile experiment scope -> divergent work/evidence -> critique/falsification -> archive -> transferable findings -> next lateral step if authorized -> stop before unapproved convergence/external dependency`

### Artifact/checkpoint work

`source truth -> artifact update -> internal consistency audit -> provenance/archive -> CURRENT_STATE/queue update when meaningful -> readback/recheck -> authorized continuation`

### External specialist audit

`define unique capability/independence gap -> bounded work package -> require evidence -> return to owning Sol -> verify baseline -> interpret/reconcile -> recursive audit -> durable state update`

## 8. Recursive audit rule

After any material correction discovered during an audit, restart the audit from the relevant authority layer rather than assuming downstream conclusions remain valid.

A work package is complete when one full pass finds:

- no material error;
- no authority inconsistency;
- no meaningful scope violation;
- no unsupported validation/provider claim;
- no imaginary automation/connection assumption;
- no duplicate backlog/authority surface;
- no worthwhile improvement inside the current objective;
- no remaining already-authorized operation whose completion would avoid an unnecessary Director round trip.

This does not require polishing forever. Improvements outside the bounded objective become later queue work rather than scope creep.

## 9. Relationship to other protocols

This protocol strengthens rather than replaces:

- patch-first Engineering;
- evidence -> smallest unresolved question -> recursive audit -> one justified next action;
- Engineering hygiene/deletion/convergence law;
- design exploration/refinement separation;
- Surface != law;
- validation hierarchy;
- repository residency / Drive provenance;
- fresh-chat continuity;
- Director authority at consequential gates;
- the canonical Agent Orchestration Protocol.

Required synthesis:

> **The smallest unresolved question defines the objective; the owning Sol decides how much logically coupled work should close around it; Codex executes mechanics when useful; scarce/independent models are used only when they add material capability or information; `stop` means the next real authority/external-dependency boundary, not every intermediate artifact.**

## 10. Fresh-chat requirement

Every fresh Ensemble/Kymaean manager chat must inherit this protocol through the owning repository bootstrap.

Do not ask the Director to restate this permission.

Do not reinterpret anti-churn as reduced initiative.

Do not assume Codex/Astra/Claude should inherit ordinary project authority. External execution/review remains subordinate to owning Sol/Director/repository law.

The canonical copy of this cross-project protocol lives in `Rylascoo/Ensemble-Project`. A Design-repository compatibility copy must be retired/reclassified after its inbound references are migrated; do not maintain two independently evolving canonical cross-project copies.