# Ensemble Project Execution Queue

Status: ACTIVE OPERATIONAL REGISTER — sequencing and backlog only. This file does not carry phase, provider, product, design, or validation authority. `CURRENT_STATE.md` remains the only active engineering checkpoint/next-action authority; lane-specific current-state files retain their own volatile boundaries.

Updated: 2026-09-08

## Queue law

1. Fresh chats read this queue after the authority/bootstrap surfaces and reconcile it against exact live refs before starting work.
2. A queue item may be worked only when its status and prerequisites permit it. A later experiment does not become executable merely because its design can be discussed.
3. `ACTIVE` means work may proceed now inside the stated lane and scope. `PREPARATION-READY` means non-executing preparation may proceed now while the actual experiment remains blocked. `BLOCKED` means the trigger is not yet satisfied. `DEFERRED` means intentionally postponed by authority. `ADMIN-BLOCKED` means no project meaning is unresolved but a required repository operation is unavailable. `DONE` means the item has durable closure evidence.
4. The frozen E0 experiment order is A -> B -> C -> D -> E -> F -> G. Preparation of a later arm may occur only when separately authorized and must not consume the experiment, provider traffic, scoring, renderer budget, or prerequisite state.
5. When an item changes status, ownership, prerequisite, or exit condition, update this register in the same logical closeout that updates the authoritative state/evidence surface. Never rely on chat memory to carry a queue transition.
6. If this queue conflicts with `CURRENT_STATE.md`, a frozen blueprint, a Director decision, current source/evidence, or a lane-specific authority file, the stronger source wins and the queue must be corrected.
7. Open ODR items and hypotheses remain in their dedicated registers; this queue links execution triggers and must not silently resolve them.

## Active and near-term execution

| ID | Lane | Status | Work | Prerequisite / hard boundary | Exit / successor |
|---|---|---|---|---|---|
| Q-E0A-01 | Engineering | ACTIVE | Implement the bounded Gemini provider-error/request-compatibility diagnostic amendment on `e0a-gemini-bounded-provider-error-diagnostic`. | No credential use, provider request, inference, spend, E0-A rerun, 3.1 execution, or `main` merge without the applicable authority. | Smallest audited Harness-local diagnostic lands; cloud checks pass; any source change then requires native Windows ARM64 validation and a new annotated validation tag. |
| Q-E0A-02 | Engineering / Director | BLOCKED | Any further live provider compatibility request needed to resolve E0-A. | Q-E0A-01 machine-validated; provider/account/pricing/quota facts reverified; explicit Director authorization. | Durable provider evidence either validates a usable E0-A route or narrows/falsifies it without inflating authority. |
| Q-E0A-03 | Engineering / Evidence | BLOCKED | Complete the E0-A same-model isolated-cast reference evidence package. | A usable, authorized provider route and successful/decisive reference execution; frozen fixture/configuration provenance. | E0-A closes with auditable run evidence and the reference configuration required by later controls. |
| Q-E0B-01 | Evidence / Engineering support | BLOCKED | E0-B mixed-model cast. | E0-A closed. Preserve fixture/Character definitions; E0-B is the deliberate provider/model-diversity exception. | E0-B evidence closed and clearly compared against E0-A. |
| Q-E0C-01 | Evidence | BLOCKED | E0-C repeated identical-condition runs. | E0-B closed; return to the E0-A same-model reference configuration. | Repetition evidence closes without changing initial state or fixed settings. |
| Q-E0D-01 | Evidence | BLOCKED | E0-D ablation controls. | E0-C closed; use E0-A same-model reference configuration; vary one named architectural variable at a time. | Required ablations close with isolation intact. |
| Q-E0E-PREP | Evidence | PREPARATION-READY | Prepare and, if the frozen preparation contract earns it, preconstruct the non-network E0-E single-model playwright control arm. | Director authorization 2026-09-08 permits preparation only. Must preserve the E0-A reference-model/settings identity, complete-Scene playwright condition, blind-review compatibility, and throwaway experimental boundary. No provider execution, scoring/unblinding, or E0-A-D mutation. | Preparation packet is recursively clean and marked ready-for-sequence; any preconstruction is compiler/static only and cannot count as E0-E evidence. Live packet: `docs/handoff/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_HANDOFF_2026_09_08.md`. |
| Q-E0E-RUN | Evidence | BLOCKED | Execute and score E0-E single-model playwright control. | E0-A, E0-B, E0-C, and E0-D closed; E0-E preparation complete; exact E0-A reference configuration available; any required provider execution separately authorized/current. | Blind scoring recorded before unblinding; E0-E evidence closed as an architecture-isolating control. |
| Q-E0F-01 | Evidence / Engineering support | BLOCKED | E0-F integrity and failure injection. | E0-E closed; cases remain separately labeled from experiential comparisons. | Hard-gate failure evidence closes without contaminating behavioral scoring. |
| Q-E0G-01 | Evidence | BLOCKED | E0-G substantially different generalization fixture. | E0-F closed; new no-secret/generalization fixture frozen; first pass uses same-model reference principle. | Generalization evidence closes or falsifies/revises the architecture. |
| Q-E0-CONV | Engineering / Evidence / Director | BLOCKED | End-of-E0 convergence, simplification/deletion audit, and post-E0 disposition. | E0-A through E0-G complete with hard gates satisfied or explicit falsification. | Director approves the post-E0 architecture direction or orders revision/simplification. |

## Parallel design lane

| ID | Lane | Status | Work | Prerequisite / hard boundary | Exit / successor |
|---|---|---|---|---|---|
| Q-DESIGN-01 | Design Sol | ACTIVE | Prepare the separate bounded rendered-successor program proposal based on DPSC-01. | Follow `Rylascoo/Ensemble-Website/CURRENT_STATE.md`. Renderer expenditure remains stopped until candidate-construction method, budget, material-difference rule, pre-render rejection rules, and termination consequence are frozen. | Design Sol records clean recursive closure and advances only within its lane. |
| Q-DESIGN-02 | Design Sol | BLOCKED | Transcript-dependent Stage motion / integration decisions that require behavioral evidence. | Real blinded E0-A vs E0-E evidence as required by the design repository. | Design work resumes from its own current-state/ledger authority; no engineering queue item dictates pixels. |

## Administrative continuity

| ID | Lane | Status | Work | Prerequisite / hard boundary | Exit / successor |
|---|---|---|---|---|---|
| Q-ADMIN-01 | Engineering | ADMIN-BLOCKED | Archive/delete reconciled stale engineering branch refs. | Create and verify required annotated `archive/...` tags first. Current connected writer cannot create those tags. | Archive tags exist, zero unique accepted work is reverified, stale refs removed. |

## Post-E0 program order

These items remain blocked until E0 convergence; detailed requirements stay in `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` and are expanded into bounded work packages only when their gate approaches.

| ID | Status | Program gate |
|---|---|---|
| Q-POSTE0-01 | BLOCKED | Freeze post-E0 product-runtime architecture. |
| Q-POSTE0-02 | BLOCKED | Parallel productization: durable Production persistence/recovery, native Windows shell, production provider/local capabilities, and UI/design implementation convergence. |
| Q-ALPHA-01 | BLOCKED | Runtime-complete Alpha and integrated target-device matrix. |
| Q-BETA-01 | BLOCKED | Beta/release architecture convergence and hardening. |
| Q-RELEASE-01 | BLOCKED | Exact ARM64 MSIX candidate, security/dependency provenance, install/upgrade testing, and WACK. |
| Q-STORE-01 | BLOCKED | Director Store submission decision and Partner Center certification. |

## Standing registers that must not be forgotten

- `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`: unresolved product/design questions remain open until Director resolution.
- `docs/HYPOTHESIS_LEDGER.md`: each open/narrowed hypothesis carries its own verification trigger; queue work must honor those triggers when relevant.
- `docs/VALIDATION_LEDGER.md`: exact machine-tested checkpoints and validation rung; later documentation never inherits runtime authority automatically.
- `docs/evidence/DESIGN_REPOSITORY_POINTER.md` and `docs/REPOSITORY_RESIDENCY.md`: cross-lane/design location and canonical-home rules.
- `Rylascoo/Ensemble-Website/docs/evidence/DESIGN_LEDGER.md`: Design Sol closure/reopening history.

## Closeout check

Before a substantive project work package ends, ask all four questions:

1. Did this work change the status or prerequisite of any queue item?
2. Did it expose a new consequential task not already represented in the queue, ODR, hypothesis ledger, roadmap, or lane ledger?
3. Is the next executable item still consistent with the exact current-state authority?
4. Is any later item being prepared or executed before its prerequisite boundary?

A clean closeout leaves those answers reconciled in durable repository state, not merely in conversation.
