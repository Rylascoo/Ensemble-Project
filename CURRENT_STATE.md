# Ensemble Current State

Updated: 2026-09-22

## Authority

Application Product, Implementation and App Design authority: `Rylascoo/Ensemble-Project`. Fresh work reads `AGENTS.md`, this file, `docs/PROJECT_AUTHORITY.md`, `docs/PROJECT_EXECUTION_QUEUE.md`, `docs/VALIDATION_LEDGER.md`, and `docs/design/app/CURRENT_CONTRACTS.md` for Design/UI work.

Current tasks use roles, not persistent personas.

## Checkpoint

`DESIGN_ARCHITECTURE_READY = READY`. Q-PROD-02/03/04/05 and Q-DESIGN-20/21/22 are integrated.

Q-UNITY-01 project unification is complete: `docs/Q_UNITY_01_PROJECT_UNIFICATION_AND_SIMPLIFICATION_2026_09_22.md`.

Q-UNITY-02 is **DONE / INTEGRATED** through PR #248; exact-main Validation #1192 PASS. The disconnected workspace/demo scaffold is retired without changing canonical E0 or integrated Product semantics. Evidence: `docs/evidence/Q_UNITY_02_DISCONNECTED_WORKSPACE_DEMO_SCAFFOLD_RETIREMENT_2026_09_22.md`.

## Open gates

Home A/B, FIRSTUSE, rename/delete/import/restore, Character semantics beyond identity/name, Scene ontology/lifecycle, provider/Performer, Stage motion, deferred E0, High Contrast, Source Sans 3/S1 packaging, final architecture, WACK/Store and release remain separately gated.

Q-PROD-06 is **DONE / INTEGRATED** through PR #249; exact-main Validation #1226 PASS. Product owns persistent Character identity/name, establishment and replay-derived Production Cast; `Character != Performer` and Production Cast != Scene Roster.

Q-PROD-07 / Q-DESIGN-23 is **DONE / INTEGRATED** through PR #252 at `7d79581aac869f8e81fc0a5267b1f14fde394246`; push-triggered exact-main Validation #1266 PASS. Current-Production **Characters** is the adopted read-only Cast list + bounded **New Character** flow. Native/runtime authority remains bound to exact executable `9ea74391adf0a2f259c205ae88128324d4a52148`. Evidence: `docs/evidence/Q_PROD_07_CHARACTER_PRESENTATION_NATIVE_ARM64_VALIDATION_2026_09_22.md`.

## Next

Q-PROD-08 is **SELECTED / IMPLEMENTATION AUTHORIZED**: Product Scene identity + initial roster foundation only. Scene identity is opaque; initial roster is a subset of existing Production Cast; replay/persistence use the existing Production journal. Current Scene, roster mutation, Scene ending/cardinality, Character Core, provider/Performer, Opportunity/Performance and Scene UI remain excluded. Evidence: `docs/evidence/Q_PROD_08_SUCCESSOR_SELECTION_SCENE_IDENTITY_INITIAL_ROSTER_FOUNDATION_2026_09_22.md`.

Implementation requires the normal isolated writer, exact review and native ARM64 validation path before integration. Deferred-E0 exposure/reversibility is now an explicit package guard.

Q-ADMIN-08: **.NET 10 decision input / native probe pending**; retarget remains Director-gated.
