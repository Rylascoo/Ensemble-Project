# Ensemble Project Execution Queue

Status: **ACTIVE CURRENT-WORK REGISTER**

Updated: 2026-09-24

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

Q-ADMIN-08, Q-PREVIEW-01, Q-PROD-09 and Q-DESIGN-24 are integrated. Q-DESIGN-24 PR #271 merged at `20ba896e54ee533905af7b14a240d829f6ac3fc9`; exact-main Validation #1393 (`36071107439`) PASS. Design/native acceptance remains exact `bdd6c094446be79b37e28c2ddb4a6c31996646b5`. PR #262/#269 remain unmerged historical branches; human Scene-code comprehension/discoverability, Narrator intelligibility and High Contrast remain later falsifiers.

Program completion doctrine: Windows 11 ARM64 / Microsoft Store is the sole shipping target until release. Cross-platform work is deferred. One bounded platform-neutral Presentation seam may proceed only if it improves current Windows quality/testability without Product-semantic change; after that, architecture analysis stops unless executable evidence falsifies the structure. NPU/local AI remains optional measured infrastructure, not a correctness dependency.

| ID | Role | Status | Work | Hard boundary / successor |
|---|---|---|---|---|
| Presentation seam | Product / Architecture / Implementation | MERGE-READY — DIRECTOR AUTHORITY REQUIRED | Falsifier resolved: prior tests recompiled Presentation source; candidate now uses one platform-neutral `Kymaean.Presentation` assembly shared by WinUI/tests. Exact reviewed app source `c11c842...`; native baseline checkout `5205f13...`; evidence: `docs/evidence/PRESENTATION_SEAM_NATIVE_ARM64_BASELINE_2026_09_24.json`. | No Product/Persistence semantic change or cross-platform UI implementation. Merge requires Director authorization; after integration, architecture analysis stops absent executable falsification. |
| Native ARM64 CI / performance baseline | Implementation | ESTABLISHED ON CANDIDATE — INTEGRATION PENDING | `tools/native-arm64-baseline.ps1` provides repeatable clean-SHA ARM64 Presentation tests, full Preview technical gate, exact refresh, verified launches and memory/timing samples. Exact `5205f13...` native + hosted evidence PASS. | Measurement/regression discipline only; timings are not product KPIs or optimization claims. Integration follows the seam merge gate. |
| Current Production workspace composition | Design | DEFERRED — DIRECTOR DISPOSITION | Future bounded composition candidate identified during Preview reconciliation. | Not activated by Q-PREVIEW-01 or Q-DESIGN-24; no whole-app redesign. |
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
