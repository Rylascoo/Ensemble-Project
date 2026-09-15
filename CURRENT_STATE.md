# Ensemble Current State

Updated: 2026-09-15

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — P01 SLOT 1 AUTHORIZED; PROJECT/KEY ASSOCIATION ROTATED; PROTECTED LOCAL REBIND + WINDOW/CAPACITY GATES PENDING; NAMESPACE UNCONSUMED; PROVIDER TRAFFIC ZERO**.

## E0-D frozen execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. All six RunIds/evidence roots remain frozen, unclaimed and unconsumed. Immutable pair windows remain P01 `2026-09-16T14:30:00Z..23:30:00Z`, P02 `2026-09-17T14:30:00Z..23:30:00Z`, P03 `2026-09-18T14:30:00Z..23:30:00Z`.

## Renewed native authority
Exact checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`; tag `validation/e0d-snapshot-refresh-native-arm64`; evidence `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_NATIVE_ARM64_VALIDATION_2026_09_14.md`; executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`.

Native Windows ARM64: build PASS with 0 warnings/errors; Core 626/626; Harness 154/154; both fixture smokes PASS; credentialless provider-edge refusal PASS with no evidence root/network. Snapshot is valid inclusively through UTC `2026-09-18` and fails closed from `2026-09-19`.

## Continuity
PR #147 closed activation integration at exact-main `e8b702a5b37d208d7caeeb6ffeb5a6beacf24f85` with Validation #855 PASS. Director amendment `docs/evidence/E0D_P01_SLOT1_PROVIDER_ASSOCIATION_ROTATION_DIRECTOR_AMENDMENT_2026_09_15.md` supersedes only the prior authenticated project/key association for future P01 execution: project `Ensemble Testing`, project ID `gen-lang-client-0793779417`, credential label `Gemini API Key`, Free tier. The secret key is not repository data and has not been supplied to chat; the old protected credential must not be used for P01. RunId/root/model/profile/fixture/executable/window/no-retry law remain unchanged. Later slots remain separately unauthorized; Q-E0E-RUN remains blocked.

## Next
Before execution, bind the new key into the protected CurrentUser credential mechanism without exposing plaintext and verify local readiness without provider traffic. Stop before claim/provider traffic until UTC is inside `2026-09-16T14:30:00Z..23:30:00Z`. At the launch boundary, freshly confirm the amended project/key association, exact model, Free tier and sufficient RPM/TPM/RPD capacity; recheck every frozen interlock; then launch `E0D-Q01-P01-FULL-20260914-01` exactly once only if all gates pass. Do not activate P01 Slot 2 or any later slot without separate Director authority.
