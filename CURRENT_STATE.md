# Ensemble Current State

Updated: 2026-09-14

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — EXPERIMENT-ONLY IMPLEMENTATION NATIVE WINDOWS ARM64 VALIDATED; HOSTED INTEGRATION PENDING; PROVIDER TRAFFIC ZERO**.

## E0-D implementation + validation
Frozen method: `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md`; preregistration: `docs/evidence/E0D_Q_E0D_01_PREREGISTRATION_2026_09_14.json`; Director authorization: `docs/evidence/E0D_IMPLEMENTATION_NATIVE_VALIDATION_DIRECTOR_AUTHORIZATION_2026_09_14.md`.

Implementation audit: `docs/evidence/E0D_ABLATION_CONTROLS_IMPLEMENTATION_AUDIT_2026_09_14.md`. Native validation: `docs/evidence/E0D_ABLATION_CONTROLS_NATIVE_ARM64_VALIDATION_2026_09_14.md`.

Rebased implementation source is `2afda26825d7ecad11677f858d523300419aa23a`. Exact native-validated checkpoint is `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891`, preserved by `validation/e0d-ablation-controls-native-arm64` (tag object `8eae0730664a4dfa21debdae5a2f73e070d5982d`).

Native ARM64 result: Release build PASS with 0 warnings/errors; Core 626/626; Harness 154/154; both fixture smokes PASS; repository law/census/oracle/diff/clean/source-identity gates PASS; credentialless `e0d-run` exits 1 before network and creates no evidence root. Executable SHA-256: `498c25f528ab1ba26a592d247737186ac839c61b56154303d6e57a94319d0769`.

## Continuity
No E0-D live RunId/evidence namespace, execution window, credential use, provider request, `countTokens`, generation, inference, spend, scoring, or live activation is authorized or consumed. E0-C remains closed; Q-E0E-RUN remains blocked until E0-D closes.

Q-ADMIN-03 MA-01 remains a separate bounded read-only Scout admission opening and creates no E0-D/provider/validation authority.

## Next
Integrate the validated E0-D implementation/evidence through required exact-head hosted Validation + E0-E preparation gates. Merge only if both pass on the exact PR head, then require push-triggered exact-main Validation. Stop before E0-D live activation; RunIds/windows/provider authority require a separate Director gate.
