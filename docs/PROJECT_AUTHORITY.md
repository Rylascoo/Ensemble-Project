# Ensemble Project Authority

## Source-of-truth order

1. Frozen Ensemble Blueprint and approved phase specifications.
2. GitHub `CURRENT_STATE.md` for the active engineering checkpoint, validation classification, and next action.
3. GitHub source, tests, fixture versions/hashes, commits, and PR evidence.
4. Director decisions and durable evidence/decision records.
5. Google Drive design repository for visual/design evidence and assets inside the design lane.
6. Historical chat/source material only when a narrow continuity ambiguity cannot be resolved from durable project state.

No navigation index, ledger, README, handoff, archived evidence file, or historical branch snapshot independently advances phase authority.

## Product and policy authority

The Director owns product constitution, Open Design Register resolution, provider admissibility, and any decision about what the product may do. Engineering and design surfaces supply inputs within their own lanes and do not author in this one. ODR proposals and resolutions are filed in `Ensemble-Project/docs` following the ODR-33 precedent, regardless of which lane raised the question. An input drafted outside its authoring surface's lane must say so in the document.

Program planning remains in `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md`. Open product/design questions remain tracked in `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`. Neither surface overrides `CURRENT_STATE.md` for the current engineering checkpoint.

## Engineering authority

GitHub is the canonical engineering workspace. Changes should be patch-first and reviewed against the smallest affected surface. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is active project law for engineering hygiene and repository-surface lifecycle unless stronger frozen authority overrides it.

## Register and navigation roles

- `docs/DOCUMENT_INDEX.md` is navigation only. It is deliberately excluded from document-census authority traversal, so linking a file there cannot make that file current.
- `docs/VALIDATION_LEDGER.md` records validation facts and durable validation tags. It cannot advance the current checkpoint or inflate a validation rung.
- `docs/HYPOTHESIS_LEDGER.md` records explicitly unverified assumptions and their verification triggers. A hypothesis never becomes a decision or fact merely by being listed.
- `docs/evidence/archive/` preserves historical provenance. Archive references do not establish current authority for active evidence.
- `docs/handoff/` contains temporary transition artifacts only when `CURRENT_STATE.md` identifies one as live.

For repository-document hygiene, `CURRENT_STATE.md` and this file are the durable authority-graph roots. Other active documents must derive their continuing role through explicit references from those roots or their reachable descendants; `docs/DOCUMENT_INDEX.md` may aid navigation but cannot confer authority reachability.

## Reasoning / task-scope workflow authority

`docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` is active cross-project workflow law for the remainder of Ensemble/Kymaean project creation.

For every substantive Director turn, GPT-5.6 Sol High reasoning should deliberately optimize task granularity before execution: choose the largest logically coupled, falsifiable scope that belongs to one objective, use the additional reasoning capacity for source reconciliation, architecture/dependency analysis, contradiction detection, falsification and recursive audit, and stop at the next consequential authority or external-validation gate.

The optimization target is reasoning density and completed dependency closure per Director turn—not response length, token use, code volume, file count, or artificially broad scope. Simple tasks remain simple.

This workflow protocol controls collaboration granularity only. It cannot override frozen architecture, `CURRENT_STATE.md`, source/test evidence, machine-validation authority, Director approval, security boundaries, WACK, Store certification, or any other higher authority defined by this project.

## Design authority

The connected Google Drive `Ensemble Project` folder is the canonical visual/design workspace for UI/UX architecture, mockups/prototypes, visual identity/artwork, motion/animation, Store/marketing assets, and design research/references.

Design artifacts are evidence and design direction unless explicitly promoted into an approved implementation specification. Design authority does not create engineering phase-status authority.

## Validation hierarchy

Static reasoning < compiler output < target-device runtime < hardware/NPU evidence < package validation < Store certification.

Never promote a lower validation level into a higher one.

## Continuity rule

Nothing important changes silently. Meaningful decisions, evidence, implementation milestones, reopened assumptions, and changes to active authority must be recorded in durable project state.
