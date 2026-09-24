# Ensemble Project Execution Queue

Status: **ACTIVE CURRENT-WORK REGISTER**

Updated: 2026-09-23

The complete pre-unification sequencing/history is preserved at `docs/evidence/archive/PROJECT_EXECUTION_QUEUE_PRE_UNIFICATION_2026_09_22.md`.

This file now carries only current, deferred, or blocking work. Completed history belongs in evidence/validation/Design ledgers and Git history, not in the everyday execution surface.

## Queue law

1. `CURRENT_STATE.md` is the volatile project checkpoint and next-action authority.
2. This queue sequences current work but cannot override Blueprint, Product/Director decisions, current source/evidence, Design authority or validation law.
3. `ACTIVE` may execute now. `PREPARATION-READY` may prepare but not cross its stated gate. `DEFERRED` is intentionally postponed. `BLOCKED` requires its prerequisite. `DONE` should normally be removed from this active file after durable closeout.
4. Provider traffic, deferred-E0 consumption, merge, destructive history cleanup and release gates require their own authority.
5. One implementation writer per bounded package. Independent review/Design acceptance remain separate when required.
6. Historical persona names do not create managers; current work declares task roles.

## Current application work

Q-ADMIN-08 and Q-PREVIEW-01 are integrated. Corrected Q-DESIGN-24 remains separate in draft PR #262 and INCONCLUSIVE. A Preview-only local composite at `dd56b30aeaa01e69ccd0435a3b680d6c1b4cd350` earned TECHNICAL PREVIEW ADMISSION PASS on SurfSeven; this is not Scene Design acceptance or Product integration authority.

| ID | Role | Status | Work | Hard boundary / successor |
|---|---|---|---|---|
| First Product-native Performance — final decision analysis | Product / Architecture | **ACTIVE — READ-ONLY / FINAL ANALYSIS** | Resolve Q1 E0→Product acquisition mode, Q2 minimum Character context/access, and Q5 first consequence domain with concrete code footprints, falsifiers and deferred-E0 exposure. | No implementation, provider traffic, deferred-E0 consumption or Design mutation. Stopping rule: after Director disposition of Q1/Q2/Q5, next Product artifact is implementation unless new executable evidence is proven necessary. |
| Q-DESIGN-24-WINDOWS | Implementation / Design | PAUSED — HUMAN ACCEPTANCE INCONCLUSIVE | Preserve corrected `abada447` candidate, first failure, Preview admission evidence and Scene-code falsifier in separate PR #262/local Preview apparatus. | Preview admission is technical only; no Scene acceptance/merge. Human comprehension/Narrator claims remain. |
| Current Production workspace composition | Design | DEFERRED — DIRECTOR DISPOSITION | Future bounded composition candidate identified during Preview reconciliation. | Not activated; no whole-app redesign. |
| Q-ADMIN-07 | Governance / Recovery | DEFERRED — LOCAL RESIDUE ONLY | Disposition remaining local historical/deferred worktrees after owner/status/evidence review. | Remote merged refs require explicit scope amendment plus archive-tag-before-delete law. Preserve diverged/ambiguous evidence. |

## First Product-native Performance Director boundary

Pending-integration Director dispositions are recorded on this continuity branch in:

`docs/FIRST_PRODUCT_NATIVE_PERFORMANCE_DIRECTOR_DISPOSITION_2026_09_23.md`

Key boundary: a deterministic test-adapter Performer counts for the first Product-native Performance, while first live AI Performance remains later. The first Product-native milestone must expose its committed consequence in the acting Character's own later bounded Product context.

The final analysis owns only Q1/Q2/Q5. Opportunity, Scene targeting, provisional acceptance and invocation-scoped deterministic Performer binding have already been bounded for the first milestone; the record above is controlling continuity evidence until integrated.

## Deferred E0 / architecture gates

| ID | Status | Boundary |
|---|---|---|
| Q-E0D-01 | DEFERRED VALIDATION | Preserve consumed P01/P02 evidence and unconsumed P03 namespace; no auto-launch/provider traffic. Current chain: `docs/evidence/E0D_CURRENT_DEFERRED_VALIDATION_INDEX_2026_09_22.md`. |
| Q-E0E-RUN | DEFERRED | Resume only under current deferred-E0/Director/provider authority. |
| Q-E0F-01 | DEFERRED | Resume after E0-E closure. |
| Q-E0G-01 | DEFERRED | Resume after E0-F closure. |
| Q-E0-CONV | BLOCKED | Requires E0-A through E0-G closure or explicit falsification/disposition. |
| Q-POSTE0-01 | BLOCKED | Final/frozen post-E0 runtime architecture requires deferred-E0 reconciliation. |
| Q-POSTE0-02 | BLOCKED | Finalization after post-E0 architecture freeze. |

Any future Q-PROD package that adds durable Performance/Take/consequence history must satisfy `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` deferred-validation exposure law before executable closeout.

## Other gated Design / release work

| ID | Status | Boundary |
|---|---|---|
| Q-DESIGN-02 | BLOCKED | Transcript-dependent Stage motion/integration requires behavioral evidence. |
| Q-ALPHA-01 | BLOCKED | Runtime-complete Alpha and integrated target-device matrix. |
| Q-BETA-01 | BLOCKED | Beta/release architecture convergence and hardening. |
| Q-RELEASE-01 | BLOCKED | Exact ARM64 MSIX candidate, security/dependency provenance, install/upgrade testing and WACK. |
| Q-STORE-01 | BLOCKED | Director Store submission decision and Partner Center certification. |

## Standing registers

- `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md` — unresolved Product/Design questions.
- `docs/HYPOTHESIS_LEDGER.md` — unverified assumptions and triggers.
- `docs/VALIDATION_LEDGER.md` — exact validation facts/rungs.
- `docs/design/app/CURRENT_CONTRACTS.md` — active application-Design bootstrap.
- `docs/evidence/LEGACY_CURRENT_EVIDENCE_INDEX_2026_09_22.md` — compact reachability for still-current historical evidence removed from the active queue.
- `docs/roadmap/KYMAEAN_ARCHITECTURE_AND_SHIP_PLAN.md` — program order.
