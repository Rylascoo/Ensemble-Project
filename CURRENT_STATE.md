# Ensemble Current State

Updated: 2026-09-14

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **BLOCKED — IMPLEMENTATION + NATIVE VALIDATION INTEGRATED; LIVE ACTIVATION NOT AUTHORIZED; PROVIDER TRAFFIC ZERO**.

## E0-D implementation + integration
Frozen method: `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md`; preregistration: `docs/evidence/E0D_Q_E0D_01_PREREGISTRATION_2026_09_14.json`; Director authorization: `docs/evidence/E0D_IMPLEMENTATION_NATIVE_VALIDATION_DIRECTOR_AUTHORIZATION_2026_09_14.md`.

Implementation audit: `docs/evidence/E0D_ABLATION_CONTROLS_IMPLEMENTATION_AUDIT_2026_09_14.md`; native validation: `docs/evidence/E0D_ABLATION_CONTROLS_NATIVE_ARM64_VALIDATION_2026_09_14.md`; integration closeout: `docs/evidence/E0D_ABLATION_CONTROLS_IMPLEMENTATION_INTEGRATION_CLOSEOUT_2026_09_14.md`.

Exact native authority remains checkpoint `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891`, preserved by `validation/e0d-ablation-controls-native-arm64` (tag object `8eae0730664a4dfa21debdae5a2f73e070d5982d`). Release ARM64 build passed with 0 warnings/errors; Core 626/626; Harness 154/154; both fixture smokes and credentialless no-network edge passed.
PR #135 exact head `10c44b6e1255719be2f8b0058205e7906f57b035` passed Validation #818 and E0-E preparation #79, merged as `b142d63e3d179f94f588bd19adb9cec4b99cad1b`, and push-triggered exact-main Validation #819 passed. The source branch is archive-tagged at `archive/e0d-experiment-implementation-2026-09-14` and retired.

## Continuity
No E0-D live RunId/evidence namespace, execution window, credential use, provider request, `countTokens`, generation, inference, spend, scoring, or live activation is authorized or consumed. E0-C remains closed; Q-E0E-RUN remains blocked until E0-D closes.

Q-ADMIN-03 MA-01 remains a separate bounded read-only Scout admission lane and creates no E0-D/provider authority.

## Next
Obtain separate Director authority before any E0-D live activation preparation that allocates RunIds/namespaces/windows or uses credentials/provider traffic. Until then, preserve the six frozen slots and exact validated implementation unchanged.
