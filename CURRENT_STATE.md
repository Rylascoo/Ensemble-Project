# Ensemble Current State

Updated: 2026-09-15

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — P01 SLOT 1 AUTHORIZED; ASSOCIATION + PROTECTED REBIND PASS; P01 WINDOW DIRECTOR-AMENDED; CAPACITY PENDING; NAMESPACE UNCONSUMED; PROVIDER TRAFFIC ZERO**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. All six RunIds/evidence roots remain unclaimed/unconsumed. `docs/evidence/E0D_P01_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_15.md` supersedes only P01 timing: P01 `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`; P02 `2026-09-17T14:30:00Z..23:30:00Z`; P03 `2026-09-18T14:30:00Z..23:30:00Z`.

## Native authority
Checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`; tag `validation/e0d-snapshot-refresh-native-arm64`; executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`. Core 626/626; Harness 154/154; build/smokes/credentialless provider-edge PASS. Snapshot valid through UTC `2026-09-18`; fail closed from `2026-09-19`.

## Continuity
PR #149 integrated protected readiness at `main@0ea146c38a83f201001c1da7b9da31e943ebad1d`; Validation #863 PASS. P01 association: `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier. `docs/evidence/E0D_P01_SLOT1_PROTECTED_CREDENTIAL_REBIND_READINESS_2026_09_15.md` records the new CurrentUser-protected credential ready with zero provider traffic; the old credential is ineligible. RunId/root/model/profile/fixture/executable/no-retry law is unchanged. Later slots remain separately unauthorized; Q-E0E-RUN remains blocked.

## Next
Do not claim/send provider traffic until the timing amendment is integrated and exact-main Validation is green. While UTC is inside `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`, re-run the non-consuming census, freshly verify the amended association, exact model, Free tier, sufficient RPM/TPM/RPD, all frozen interlocks and selection of the new protected credential; then launch `E0D-Q01-P01-FULL-20260914-01` exactly once only if every gate passes. P01 Slot 2 and later slots require separate Director authority.
