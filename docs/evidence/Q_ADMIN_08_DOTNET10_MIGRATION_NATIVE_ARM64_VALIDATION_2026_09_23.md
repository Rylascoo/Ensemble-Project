# Q-ADMIN-08 .NET 10 migration — native ARM64 validation

Status: **DIRECTOR-AUTHORIZED / IMPLEMENTATION AND VALIDATION IN PROGRESS**

## Authority and scope

On 2026-09-23 the Director chose **RETARGET NOW** and authorized a dedicated platform/tooling package from fresh live `main@5ae2bf2307df4faa8c7342303419ba3fc5e620e1` (push Validation #1286 PASS). The earlier disposable probe at `a2ab3a4287b0b3c3e42eef961877c3a973efd9aa` predates integrated Q-PROD-08 and is decision evidence only: `docs/evidence/Q_ADMIN_08_DOTNET10_MIGRATION_DECISION_INPUT_2026_09_22.md`.

One primary writer owns branch `codex/qadmin08-dotnet10-migration-2026-09-23` in `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\codex-qadmin08-dotnet10-20260923`. Authorized changes: SDK 10.0.400 feature band, all eleven project TFMs to net10, Microsoft Testing Platform selection/invocation and necessary CI/operator guidance. Package versions remain unchanged; a required package-version change is a stop boundary. Product, persistence contracts, Character/Scene behavior, Design/UX, provider behavior, deferred E0 and release policy are excluded. Q-PROD-09 is not authorized. The accepted Q-PROD-08 validation-tag repair remains continuity only.

## Fresh .NET 9 comparison baseline

Measured on SurfSeven at exact base above using native ARM64 SDK 9.0.317 / runtime 9.0.19: environment/RID PASS; eleven forced-evaluation restores and eleven Release builds PASS, all zero warnings/errors; Core 628/628, Harness 155/155, Application 84/84, Persistence 148/148, preparation 10/10 PASS, no skips. Both credential-free fixture smokes PASS. This includes integrated Q-PROD-08, unlike the earlier probe.

Native development registration of a copied exact layout under a new disposable identity launched a responsive ARM64 `Kymaean` window with the empty Home state; clean close and removal PASS. No existing package identity/data was reused. The validation apparatus corrected an inappropriate staged-package flag and an incorrect Home label assertion before the successful smoke; neither required a project change.

Raw baseline commands/results, resolved dependency manifests and external cross-runtime probe live under `C:\Users\Wiryl\Sol Dev\admin-scratch\qadmin08-dotnet10-migration-20260923`. Candidate validation, exact executable SHA/tag, cross-runtime comparisons, independent review and hosted gates remain pending. Compilation alone is not migration completion.

## Exit boundary

Record fresh exact-candidate native tests/build/launch, dependency/output/persistence comparisons, repository-law/census/oracle checks and independent behaviorally no-write review; push one branch, open one draft PR and run hosted push/PR gates. Stop at Director executable merge authorization. Q-ADMIN-07 and all unrelated historical branches/worktrees remain preserved.
