# Ensemble Current State

Updated: 2026-09-22

## Authority

Application Product, Implementation and App Design authority: `Rylascoo/Ensemble-Project`. Fresh work reads `AGENTS.md`, this file, `docs/PROJECT_AUTHORITY.md`, `docs/PROJECT_EXECUTION_QUEUE.md`, `docs/VALIDATION_LEDGER.md`, and `docs/design/app/CURRENT_CONTRACTS.md` for Design/UI work.

Current tasks use roles, not persistent personas.

## Checkpoint

`DESIGN_ARCHITECTURE_READY = READY`. Q-PROD-02/03/04/05 and Q-DESIGN-20/21/22 are integrated. PR #247 merged at `abf7626b038625dd1a4ed32ee643d1232e011b8c`; exact-main Validation #1182 PASS. Post-closeout main `c41d07fcd8108fb57932b56db0c4a7061d15d533`; Validation #1184 PASS.

Q-UNITY-01 is complete and the active project is role-based / Project-rooted.

## Active simplification

Q-UNITY-02 retires disconnected first-slice workspace/demo Product scaffolding before real Character/Cast work. Evidence:
`docs/evidence/Q_UNITY_02_DISCONNECTED_WORKSPACE_DEMO_SCAFFOLD_RETIREMENT_2026_09_22.md`.

Scope is deletion of the unused `WorkspaceApplication` / `WorkspaceProjection` / `IProductionStore` / `Kymaean.Infrastructure.Demo` surface and its direct tests/build references only. Integrated ProductApplication/Persistence/Windows contracts and canonical E0 fixtures remain untouched.

## Open gates

Home A/B, FIRSTUSE, Character/Scene Product ontology, provider/Performer, Stage motion, deferred E0, High Contrast, Source Sans 3/S1 packaging, final architecture, WACK/Store and release remain separately gated.

## Next

Validate Q-UNITY-02. After integration, run Q-PROD-06 successor selection for the smallest persistent Character / Production Cast identity foundation; do not combine it with Scene membership or Performer casting.
