# Ensemble Project Execution Queue

Status: ACTIVE OPERATIONAL REGISTER — sequencing and backlog only. This file does not carry phase, provider, product, design, or validation authority. `CURRENT_STATE.md` remains the only active engineering checkpoint/next-action authority; lane-specific current-state files retain their own volatile boundaries.

Updated: 2026-09-09

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
| Q-E0A-01 | Engineering | DONE | Implement the bounded Gemini provider-error/request-compatibility diagnostic amendment on `e0a-gemini-bounded-provider-error-diagnostic`. | No credential use, provider request, inference, spend, E0-A rerun, 3.1 execution, or `main` merge without the applicable authority. | Closed at machine-tested checkout `e6e7d6c8c7187a87df2c97f2373dd3adbee7ce4a`; Windows ARM64 Core 622/622, Harness 130/130, build/smokes/credentialless PASS; annotated validation tag `validation/e0a-gemini-bounded-provider-error-diagnostic-native-arm64`. Successor Q-E0A-02 remains BLOCKED. |
| Q-E0A-02 | Engineering / Director | BLOCKED | Any further live provider compatibility request needed to resolve E0-A. | Q-E0A-01 machine-validated; provider/account/pricing/quota facts reverified; explicit Director authorization. | Durable provider evidence either validates a usable E0-A route or narrows/falsifies it without inflating authority. |
| Q-E0A-03 | Engineering / Evidence | BLOCKED | Complete the E0-A same-model isolated-cast reference evidence package. | A usable, authorized provider route and successful/decisive reference execution; frozen fixture/configuration provenance. | E0-A closes with auditable run evidence and the reference configuration required by later controls. |
| Q-E0B-01 | Evidence / Engineering support | BLOCKED | E0-B mixed-model cast. | E0-A closed. Preserve fixture/Character definitions; E0-B is the deliberate provider/model-diversity exception. | E0-B evidence closed and clearly compared against E0-A. |
| Q-E0C-01 | Evidence | BLOCKED | E0-C repeated identical-condition runs. | E0-B closed; return to the E0-A same-model reference configuration. | Repetition evidence closes without changing initial state or fixed settings. |
| Q-E0D-01 | Evidence | BLOCKED | E0-D ablation controls. | E0-C closed; use E0-A same-model reference configuration; vary one named architectural variable at a time. | Required ablations close with isolation intact. |
| Q-E0E-PREP | Evidence | DONE | Prepare and preconstruct the non-network E0-E single-model playwright control arm. | Director authorization 2026-09-08 permitted preparation only; no provider execution, scoring/unblinding, or E0-A-D mutation. | Closed by `docs/evidence/E0E_SINGLE_MODEL_PLAYWRIGHT_CONTROL_PREPARATION_CLOSEOUT_AUDIT_2026_09_09.md`; frozen contract and throwaway implementation passed recursive audit, hosted win-arm64 cross-compile, deterministic tests 10/10, and standard repository validation. Successor Q-E0E-RUN remains BLOCKED. |
| Q-E0E-RUN | Evidence | BLOCKED | Execute and score E0-E single-model playwright control. | E0-A, E0-B, E0-C, and E0-D closed; E0-E preparation complete; exact E0-A reference configuration available; any required provider execution separately authorized/current. | Blind scoring recorded before unblinding; E0-E evidence closed as an architecture-isolating control. |
| Q-E0F-01 | Evidence / Engineering support | BLOCKED | E0-F integrity and failure injection. | E0-E closed; cases remain separately labeled from experiential comparisons. | Hard-gate failure evidence closes without contaminating behavioral scoring. |
| Q-E0G-01 | Evidence | BLOCKED | E0-G substantially different generalization fixture. | E0-F closed; new no-secret/generalization fixture frozen; first pass uses same-model reference principle. | Generalization evidence closes or falsifies/revises the architecture. |
| Q-E0-CONV | Engineering / Evidence / Director | BLOCKED | End-of-E0 convergence, simplification/deletion audit, and post-E0 disposition. | E0-A through E0-G complete with hard gates satisfied or explicit falsification. | Director approves the post-E0 architecture direction or orders revision/simplification. |

## Parallel design lane

| ID | Lane | Status | Work | Prerequisite / hard boundary | Exit / successor |
|---|---|---|---|---|---|
| Q-DESIGN-01 | Design Sol | DONE | DPSC-RSP-01 / DPSC-C01 bounded successor program. Revision 1 and Revision 2 were rejected pre-render; Case A closed the program with 0 of 2 maximum renderer expenditures consumed and no canonical scaffold. | Authoritative closure: `Rylascoo/Ensemble-Website/CURRENT_STATE.md` and `docs/evidence/HERO_DPSC_C01_REVISION_2_NEUTRAL_SCAFFOLD_FALSIFICATION_AND_PROGRAM_CLOSURE_01.txt` in the design repository. DPSC-E1 did not occur, so C02 never became available. | Durable closure preserved; successor is Q-DESIGN-03. |
| Q-DESIGN-03 | Design Sol | DONE | Post-program hero reconciliation after D9 closure, HTCR-01 mechanism-level DPSC success, and DPSC-RSP-01 zero-render transfer falsification. | Closure adopted on `Rylascoo/Ensemble-Website/main`; `docs/evidence/HERO_POST_PROGRAM_RECONCILIATION_01.txt` preserves the evidence and `CURRENT_STATE.md` carries the current design boundary. | Frozen hero contract preserved; static hero causal-mechanism / renderer research paused; successor is Q-DESIGN-04. |
| Q-DESIGN-04 | Design Sol | DONE | SHARED-BRAND visual-identity divergence re-entry audit. Resolved Phase-B/shared-brand authority, inventoried legitimately open non-hero identity lanes, preserved hero work as constraints/history rather than an incumbent surface, and selected TYP-01 as the strongest orthogonal non-render question. | Adopted on `Rylascoo/Ensemble-Website/main` at `c4ff2eb3537d0ddc0272aec126a9dc5488f0cc5f`; `docs/evidence/SHARED_BRAND_VISUAL_IDENTITY_DIVERGENCE_REENTRY_AUDIT_01.txt` preserves the audit and Website `CURRENT_STATE.md` carries the current boundary. | TYP-01 selected without convergence or final-identity authority; successor is Q-DESIGN-05. |
| Q-DESIGN-05 | Design Sol | DONE | Constructed **TYP-01 — Typographic / Editorial Identity Independence** as a frozen non-image-generative method contract and deterministic dual-context specimen matrix. | Adopted on `Rylascoo/Ensemble-Website/main` at `4130bb9645525a6773a7cf5508175da00658530d`; `docs/evidence/TYP_01_TYPOGRAPHIC_EDITORIAL_IDENTITY_INDEPENDENCE_METHOD_AND_MATRIX_01.json`, Website `CURRENT_STATE.md`, and Design Ledger L-033 carry the exact contract and boundary. | Method is frozen with no typography/identity convergence; successor is Q-DESIGN-06. |
| Q-DESIGN-06 | Design Sol | DONE | Materialized and preflighted the deterministic TYP-01 24-specimen text-only packet. Frozen accessibility testing produced zero surviving scored families before blinded review; no reviewer scores exist. | Closure adopted in `Rylascoo/Ensemble-Website` at `b3b14322d7832b144be0e042fd99bc52530e411d`; Website `CURRENT_STATE.md`, Design Ledger L-035, and `docs/evidence/TYP_01_PREFLIGHT_FAILURE_AND_TERMINATION_01.json` carry the authoritative result. Do not reinterpret control failures as scored-family results or rescue the failed packet by changing frozen family/accessibility law. | TYP-01 strong-independence is closed under its frozen contract; no typography/identity convergence is created. Successor is Q-DESIGN-07. |
| Q-DESIGN-07 | Design Sol | DONE | Conducted the fresh Phase-B orthogonal shared-brand question audit after TYP-01 zero-survivor closure and selected **MOT-01 — Cross-Surface Motion Signature Independence** as the single next bounded question. | Adopted on `Rylascoo/Ensemble-Website/main` at `e1351eeb5c8928a7a9e51d51be081c2248a15b0d`; `docs/evidence/SHARED_BRAND_ORTHOGONAL_REENTRY_AUDIT_02.txt`, Website `CURRENT_STATE.md`, and Design Ledger L-036 carry the exact result. MOT-01 is abstract Shared-Brand motion research only and does not unlock Q-DESIGN-02. | MOT-01 selected without final-motion, Stage-motion, implementation or convergence authority; successor is Q-DESIGN-08. |
| Q-DESIGN-08 | Design Sol | DONE | Constructed the **MOT-01 — Cross-Surface Motion Signature Independence** frozen method contract and deterministic dual-context temporal specimen matrix. | Adopted on `Rylascoo/Ensemble-Website/main` at `38e3ea61d336900fb05a0a6b2e8e5206aa23a5d0`; `docs/evidence/MOT_01_CROSS_SURFACE_MOTION_SIGNATURE_INDEPENDENCE_METHOD_AND_MATRIX_01.json`, Website `CURRENT_STATE.md`, and Design Ledger L-037 carry the exact contract and boundary. No final motion system, timing tokens, Stage motion or convergence authority was created. | Method frozen and recursively clean; successor is Q-DESIGN-09. |
| Q-DESIGN-09 | Design Sol | DONE | Materialized the deterministic MOT-01 browser-native packet and completed structural + accessibility preflight with no reviewer exposure or scoring. | Closed on `Rylascoo/Ensemble-Website/main` at `c3999320a60bed0d455d985966bc0eee57f7c5bf`; `docs/evidence/MOT_01_STRUCTURAL_ACCESSIBILITY_PREFLIGHT_01.json`, Website `CURRENT_STATE.md`, and Design Ledger L-039 carry the authoritative result. Attempt 01 was an invalid wall-clock diagnostic and created no family result; Attempt 02 is the valid clean deterministic preflight. | Packet/preflight frozen and clean; successor is Q-DESIGN-10. |
| Q-DESIGN-10 | Design Sol / Director / independent human | ACTIVE | Collect the frozen MOT-01 **blinded primary review records** against the exact anonymous packet; no family aggregation or unblinding yet. | Use Website main `c3999320a60bed0d455d985966bc0eee57f7c5bf` without source changes. Preserve anonymous IDs and the frozen review law. Exactly three primary records are required; at most one may be from the construction author and at least one must be a Director/independent human record. Reviewers must not see family/source IDs before their records are frozen. No evidence-aware retuning, F5, reroll, production motion token selection, Stage motion, app implementation, website deployment or final identity decision. | Three blind primary records are durably frozen with reviewer provenance and no premature unblinding; only then may a separate successor gate unblind, calibrate controls, aggregate and apply the frozen conjunctive pass/fail + termination law. |
| Q-DESIGN-02 | Design Sol | BLOCKED | Transcript-dependent Stage motion / integration decisions that require behavioral evidence. | Real blinded E0-A vs E0-E evidence as required by the design repository. | Design work resumes from its own current-state/ledger authority; no engineering queue item dictates pixels. |

## Administrative continuity

| ID | Lane | Status | Work | Prerequisite / hard boundary | Exit / successor |
|---|---|---|---|---|---|
| Q-ADMIN-01 | Engineering | DONE | Close post-convergence archive/ref and local-residue cleanup. | Seven additional exact annotated archive tags were verified before stale-ref disposition; cross-repository ownership was established before worktree disposition. | Six stale Project remote refs removed; Website-owned W1 relocated intact; archived W2 removed; abandoned Patch 0012 scratch removed after hash/semantic audit; Project root clean. Evidence: `docs/evidence/REPOSITORY_CONTINUITY_RENOVATION_AUDIT_2026_09_08.md`. |

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
2. Did it expose a new consequential task not already represented by the queue, ODR, hypothesis ledger, roadmap, or lane ledger?
3. Is the next executable item still consistent with the exact current-state authority?
4. Is any later item being prepared or executed before its prerequisite boundary?

A clean closeout leaves those answers reconciled in durable repository state, not merely in conversation.
