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

Q-ADMIN-08 is DONE / INTEGRATED through PR #259; its platform validation remains in `docs/VALIDATION_LEDGER.md`. The Director adopted Q-DESIGN-24 on 2026-09-23: `docs/design/app/contracts/SCENE_INITIAL_ROSTER_PRESENTATION.md`; decision: `docs/design/app/evidence/SCENE_PRESENTATION_CONTRACT_DECISION_2026_09_23.md`. The Director subsequently authorized bounded Windows empirical implementation after recording the approved clarifications. Plan/evidence: `docs/evidence/Q_DESIGN_24_WINDOWS_EMPIRICAL_EVIDENCE_2026_09_23.md`. The presentation-code falsifier remains controlling.

| ID | Role | Status | Work | Hard boundary / successor |
|---|---|---|---|---|
| Q-DESIGN-24-WINDOWS | Implementation / Design | ACTIVE — HUMAN/NATIVE EVIDENCE BOUNDARY | Candidate bb209ac; source review clean; empirical initial-roster inspection/establishment test incomplete. | INCONCLUSIVE pending native/review/Design/Director creator/Narrator evidence; one draft PR, no merge or Product successor. |
| Q-ADMIN-07 | Governance / Recovery | DEFERRED — LOCAL RESIDUE ONLY | Disposition remaining local historical/deferred worktrees after owner/status/evidence review. | Preserve ambiguous local evidence and the unique historical PR #226 branch; merged remote refs require archive-tag-before-delete handling. |

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
