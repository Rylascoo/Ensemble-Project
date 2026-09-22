# Ensemble Current State

Updated: 2026-09-22

## Authority

Application Product, Implementation and App Design authority: `Rylascoo/Ensemble-Project`. Fresh work reads `AGENTS.md`, this file, `docs/PROJECT_AUTHORITY.md`, `docs/PROJECT_EXECUTION_QUEUE.md`, `docs/VALIDATION_LEDGER.md`, and `docs/design/app/CURRENT_CONTRACTS.md` for Design/UI work.

Current tasks use roles, not persistent personas.

## Checkpoint

`DESIGN_ARCHITECTURE_READY = READY`. Q-PROD-02/03/04/05 and Q-DESIGN-20/21/22 are integrated.

Q-UNITY-01 project unification is complete: `docs/Q_UNITY_01_PROJECT_UNIFICATION_AND_SIMPLIFICATION_2026_09_22.md`.

Q-UNITY-02 is **DONE / INTEGRATED**. PR #248 merged at `b939238ad990a0141b2b969954372bcf9875224d`; exact-merge-main Validation #1192 PASS. Exact native executable source `31da99d240eb593697a18ae314f320473b1194ab`: ARM64 environment PASS, Application/Persistence `-Full` regression PASS, Persistence 135/135 PASS, ARM64 WinUI Release build PASS with 0 warnings/errors, full solution Release build PASS. The disconnected `WorkspaceApplication` / `WorkspaceProjection` / `IProductionStore` / `Kymaean.Infrastructure.Demo` model is retired without changing canonical E0 fixtures or integrated Product/Persistence/Windows semantics.

Evidence: `docs/evidence/Q_UNITY_02_DISCONNECTED_WORKSPACE_DEMO_SCAFFOLD_RETIREMENT_2026_09_22.md`.

## Open gates

Home A/B, FIRSTUSE, rename/delete/import/restore, Character semantics beyond identity/name, Scene ontology/lifecycle, provider/Performer, Stage motion, deferred E0, High Contrast, Source Sans 3/S1 packaging, final architecture, WACK/Store and release remain separately gated.

Q-PROD-06 is **DONE / INTEGRATED**. PR #249 merged at `1ca940a8235411b378aa2e50737bc261ce79494b`; push-triggered exact-main Validation #1226 PASS. Exact native executable source `90905c8952f77a7d6f926350fa5ada0796be56bf`: ARM64 environment verifier PASS, Application 72/72 PASS, Persistence 141/141 PASS, WinUI ARM64 Release build PASS with 0 warnings/errors. The integrated Product now owns opaque persistent Character identity, exact creator-facing Character name, explicit Character establishment and replay-derived Production Cast. `Character != Performer` and Production Cast != Scene Roster remain preserved.

Evidence: `docs/evidence/Q_PROD_06_CHARACTER_CAST_IDENTITY_FOUNDATION_NATIVE_ARM64_VALIDATION_2026_09_22.md`.

Q-DESIGN-23 is **ADOPTED / ENGINEERING HANDOFF READY / NATIVE ACCEPTANCE PENDING**. Contract: `docs/design/app/contracts/PRODUCTION_CAST_CHARACTER_ESTABLISHMENT_PRESENTATION.md`. The minimum creator-facing surface is current-Production **Characters**: read-only replay-derived Character rows plus bounded **New Character** creation, conditional presentation-only duplicate Character codes, exact focus/return, and explicit environmental non-confirmation. Historical Off-Scene/current-Scene/edit/deeper-detail semantics remain excluded.

## Next

Implement Q-PROD-07 in Windows presentation only against the integrated Q-PROD-06 Product contract and adopted Q-DESIGN-23 contract. Product/Persistence changes are a stop condition. Require exact tests, ARM64 WinUI build/runtime evidence, recursive review, and Design native acceptance before integration.
