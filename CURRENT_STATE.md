# Ensemble Current State

Updated: 2026-09-15

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — P01 SLOT 1 DIRECTOR LIVE AUTHORITY RECORDED; ACTIVATION PACKAGE INTEGRATION PENDING; LAUNCH HARD-GATED; PROVIDER TRAFFIC ZERO**.

## E0-D frozen execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. All six RunIds/evidence roots remain frozen, unclaimed and unconsumed. Immutable pair windows remain P01 `2026-09-16T14:30:00Z..23:30:00Z`, P02 `2026-09-17T14:30:00Z..23:30:00Z`, P03 `2026-09-18T14:30:00Z..23:30:00Z`.

## Renewed native authority
Exact checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`; tag `validation/e0d-snapshot-refresh-native-arm64`; evidence `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_NATIVE_ARM64_VALIDATION_2026_09_14.md`; executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`.

Native Windows ARM64: build PASS with 0 warnings/errors; Core 626/626; Harness 154/154; both fixture smokes PASS; credentialless provider-edge refusal PASS with no evidence root/network. Snapshot is valid inclusively through UTC `2026-09-18` and fails closed from `2026-09-19`.

## Continuity
Director authority now permits construction/integration of the exact P01 Slot 1 activation package and, only after that package is durably integrated with exact-main Validation green, the frozen P01 window is open, and a slot-fresh authenticated provider/account/key/model/tier/quota/capacity observation passes, exactly one execution of `E0D-Q01-P01-FULL-20260914-01`. Until those gates pass, no namespace claim, evidence-root creation, execution credential use, provider request, `countTokens`, generation, inference, scoring, or spend is authorized or consumed. Later slots remain separately unauthorized. Q-E0E-RUN remains blocked until E0-D closes. Q-ADMIN-03 predecessor, first successor and sealed `MA01-20260914-04` are all consumed/failed closed. Successor-04 proved two concurrent depth-1 read-only Scouts, exact fixture measurements, denied writes and zero plugin/MCP startup leakage, but an Administrator opening-authority hash race plus unresolved null root-exit evidence prevented admission. No retry is authorized; native subagents remain default-off; MA-02+ remain blocked; this creates no E0-D authority.

## Next
Construct and recursively audit the exact P01 Slot 1 activation package only, bind it to `E0D-Q01-P01-FULL-20260914-01` and the renewed native authority, integrate it through exact-head/exact-main Validation, then stop before any claim/provider traffic until the frozen P01 window is open and the required slot-fresh authenticated capacity observation passes. Do not activate P01 Slot 2 or any later slot without separate Director authority.
