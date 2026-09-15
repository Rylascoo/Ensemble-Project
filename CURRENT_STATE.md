# Ensemble Current State

Updated: 2026-09-15

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — P01 SLOT 1 ACTIVATION INTEGRATED + EXACT-MAIN GREEN; NAMESPACE UNCONSUMED; WINDOW + SLOT-FRESH CAPACITY GATES PENDING; PROVIDER TRAFFIC ZERO**.

## E0-D frozen execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. All six RunIds/evidence roots remain frozen, unclaimed and unconsumed. Immutable pair windows remain P01 `2026-09-16T14:30:00Z..23:30:00Z`, P02 `2026-09-17T14:30:00Z..23:30:00Z`, P03 `2026-09-18T14:30:00Z..23:30:00Z`.

## Renewed native authority
Exact checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`; tag `validation/e0d-snapshot-refresh-native-arm64`; evidence `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_NATIVE_ARM64_VALIDATION_2026_09_14.md`; executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`.

Native Windows ARM64: build PASS with 0 warnings/errors; Core 626/626; Harness 154/154; both fixture smokes PASS; credentialless provider-edge refusal PASS with no evidence root/network. Snapshot is valid inclusively through UTC `2026-09-18` and fails closed from `2026-09-19`.

## Continuity
PR #146 integrated the exact P01 Slot 1 activation package as `main@64ac4225536a5ce50e4d7ce7d21d5edc257f68ff`; branch-push Validation #850, PR Validation #851, E0-E preparation #90, and push-triggered exact-main Validation #852 all passed. Closeout `docs/evidence/E0D_Q_E0D_01_P01_SLOT1_ACTIVATION_INTEGRATION_CLOSEOUT_2026_09_15.md` records the archived source lifecycle and fresh unconsumed runtime boundary. The reserved root and claims remain absent, the retained validator remains clean at `0dacdbf6bd5453c192568cd4718207e145dfcf40`, and provider traffic remains zero. Exactly one execution of `E0D-Q01-P01-FULL-20260914-01` is now repository-integrated but remains hard-gated on UTC being inside the immutable P01 window and an immediately pre-claim authenticated provider/account/key/model/Free-tier/RPM/TPM/RPD capacity PASS. Later slots remain separately unauthorized. Q-E0E-RUN remains blocked. Q-ADMIN-03 remains terminally failed closed with native subagents default-off and MA-02+ blocked.

## Next
Stop before any claim/provider traffic until UTC is inside `2026-09-16T14:30:00Z..23:30:00Z`. At the launch boundary, obtain the required slot-fresh authenticated provider/account/key/model/Free-tier/RPM/TPM/RPD capacity observation and recheck every frozen identity/interlock. If all gates pass, launch `E0D-Q01-P01-FULL-20260914-01` exactly once. Do not activate P01 Slot 2 or any later slot without separate Director authority.
