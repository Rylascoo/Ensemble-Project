# Ensemble Project Authority

## Source-of-truth order

1. Frozen Ensemble Blueprint and approved phase specifications.
2. GitHub `CURRENT_STATE.md` for the active engineering checkpoint, validation classification, and next action.
3. GitHub source, tests, fixture versions/hashes, commits, and PR evidence.
4. Director decisions and durable evidence/decision records.
5. Google Drive design repository / `Rylascoo/Ensemble-Website` for visual/design evidence and assets inside the design lane.
6. Historical chat/source material only when a narrow continuity ambiguity cannot be resolved from durable project state.

No navigation index, ledger, README, handoff, archived evidence file, historical branch snapshot, personality prompt, agent bootstrap file, Administrator task list, dispatch issue, external-model report, or execution queue independently advances phase authority.

## Product and policy authority

The Director owns product constitution, Open Design Register resolution, provider admissibility/spend authorization, and any decision about what the product may do. Engineering and design surfaces supply inputs within their own lanes and do not author in this one.

ODR proposals and resolutions are filed in `Ensemble-Project/docs` following the ODR-33 precedent, regardless of which lane raised the question. An input drafted outside its authoring surface's lane must say so in the document.

Program planning remains in `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Open product/design questions remain tracked in `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`. Neither surface overrides `CURRENT_STATE.md` for the current engineering checkpoint.

`docs/PROJECT_EXECUTION_QUEUE.md` is the durable operational sequencing/backlog register. It exists to prevent authorized work, deferred work, prerequisites, and cross-lane gates from being lost between chats. It cannot advance a phase, activate provider traffic, resolve an ODR, promote validation, or override `CURRENT_STATE.md`, frozen Blueprint order, current source/evidence, Director decisions, or a lane-specific current-state file. Queue preparation status must be distinguished from experiment/runtime execution status.

## Engineering authority

GitHub is the canonical engineering workspace. Changes should be patch-first and reviewed against the smallest affected surface. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is active project law for engineering hygiene and repository-surface lifecycle unless stronger frozen authority overrides it.

Root `AGENTS.md` is the durable Engineering Sol/fresh-chat/bootstrap and closeout procedure. It governs how state is resolved; it does not itself state what the current phase is.

The former long Engineering personality prompt is historical migration input once its still-valid durable rules are absorbed into repository law. A prompt does not outrank this repository.

## Agent / orchestration authority

`docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md` is the **canonical cross-project agent-orchestration law**.

The role hierarchy is:

- **Director** — product/policy/provider/spend/consequential authority;
- **Engineering Sol** — accountable Engineering manager for this repository;
- **Design Sol** — accountable Design/brand/UI/website manager for `Rylascoo/Ensemble-Website` and visual-master Drive assets;
- **Ensemble Project Administrator** — future operational dispatcher/project-operations agent, never a third project authority;
- **Codex execution agents** — bounded executors/tool users;
- **scarce specialist models** — high-capability allocations for justified tasks, never authority;
- **independent reviewers such as Claude** — adversarial/advisory evidence, never authority.

The Administrator may resolve refs/queue/CI, construct work packages, create isolated execution contexts, route questions, and collect evidence. It may not resolve ODRs, authorize provider traffic/spend, promote validation, choose Design taste, redesign frozen architecture/methods, or silently adjudicate cross-lane disagreement.

A dispatch packet transports work and context. It never creates authority or a second backlog.

`docs/PROJECT_EXECUTION_QUEUE.md` remains the single cross-project backlog/sequencing register even after a future GitHub/Administrator dispatch surface is activated.

## Tooling / model allocation authority

`docs/AGENT_TOOLING_CAPABILITY_SNAPSHOT.md` is a dated **volatile operating snapshot** for current model/tool/effort allocation. It may change as product capabilities, plan limits, billing, or model menus change.

It is not product architecture, provider authority, phase authority, or validation authority.

Exact model names must not be frozen into Core/Application architecture merely because they are current project-management tools.

## Register, evidence, and navigation roles

- `docs/DOCUMENT_INDEX.md` is navigation only. It is deliberately excluded from document-census authority traversal, so linking a file there cannot make that file current.
- `docs/PROJECT_EXECUTION_QUEUE.md` tracks ordered operational work, prerequisites, parallel preparation, and backlog closure; it cannot override stronger authority or make a blocked item executable.
- `docs/VALIDATION_LEDGER.md` records validation facts and durable validation tags. It cannot advance the current checkpoint or inflate a validation rung.
- `docs/HYPOTHESIS_LEDGER.md` records explicitly unverified assumptions and their verification triggers. A hypothesis never becomes a decision or fact merely by being listed.
- `docs/EVIDENCE_LANE_CHARTER.md` defines the independent measurement/falsification lane and its E0-E authorship/preparation exception. It carries no phase, ODR, design, provider, or product authority.
- `docs/ORACLE_INDEX_GUARD.md` documents the machine-readable assertion-coverage guard; it is engineering tooling law, not product authority.
- `docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a strong-direction/open-design guard and does not become a frozen schema without Director disposition.
- `docs/evidence/archive/` preserves historical provenance. Archive references do not establish current authority for active evidence.
- `docs/handoff/README.md` defines temporary handoff semantics. A handoff is live only when `CURRENT_STATE.md` identifies its exact path.

For repository-document hygiene, `CURRENT_STATE.md` and this file are the durable authority-graph roots. Any document intended to carry continuing current authority must derive that role through explicit references from those roots or their reachable descendants. Historical/support documents may remain outside that graph only when their historical classification is explicit and machine-recognizable. Active evidence and other current documents fail closed when unexplained by the authority graph.

## Repository residency

`docs/REPOSITORY_RESIDENCY.md` defines canonical homes and the one-canonical-home rule across Engineering, Design, and Drive.

Product/policy central records and canonical cross-project orchestration/queue law remain in `Ensemble-Project` even when they concern Design/UI/website questions. Design-native artifacts belong in `Ensemble-Website` and/or the canonical Drive workspace; cross-lane copies never acquire authority by duplication.

## Reasoning / task-scope workflow authority

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` is the canonical active cross-project reasoning/task-scope workflow law.

The durable optimization target is maximum useful reasoning, synthesis, falsification, and completed dependency closure per Director turn—not response length, token use, code volume, file count, model prestige, or artificially broad scope.

The protocol also defines when work remains with the accountable Sol, when Codex execution is appropriate, when scarce specialist capacity is justified, and how independent review is allocated.

It cannot override frozen architecture, `CURRENT_STATE.md`, source/test evidence, machine-validation authority, Director approval, security/provider boundaries, WACK, Store certification, or any other higher authority defined by this project.

## Design authority

The connected Google Drive `Ensemble Project` folder is the canonical visual/master-asset workspace for UI/UX architecture, mockups/prototypes, visual identity/artwork, motion/animation, Store/marketing assets, and design research/references. `Rylascoo/Ensemble-Website` is the canonical version-controlled Design/website repository.

Design artifacts are evidence and Design direction unless explicitly promoted into an approved implementation specification or central Director decision. Design authority does not create engineering phase-status authority.

Engineering may consume a Design contract as input but does not gain permission to mutate Design-owned law merely by implementing it.

## Validation hierarchy

Static reasoning < compiler output < target-device runtime < hardware/NPU evidence < package validation/WACK < Store/Partner Center certification.

Never promote a lower validation level into a higher one.

Browser/CDP evidence is browser evidence only; external-model review is model review only; neither becomes native Windows validation by inference.

## Continuity rule

Nothing important changes silently. Meaningful decisions, evidence, implementation milestones, provider-authorization consumption, reopened assumptions, changes to active authority, execution-queue transitions, orchestration-law changes, and cross-lane residency corrections must be recorded in durable project state.

Fresh chats resolve exact live refs before relying on volatile facts; default-branch search is discovery, not active-branch evidence.

If a Director-supplied handoff contains a real event newer than GitHub, do not pretend the repository already contains it. Record the evidence provenance honestly, reconcile the current state/queue boundary, and leave unverified local evidence explicitly pending inspection.