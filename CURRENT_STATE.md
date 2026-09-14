# Ensemble Current State

Updated: 2026-09-14

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **BLOCKED — SNAPSHOT REFRESH + NATIVE REVALIDATION COMPLETE; LIVE EXECUTION NOT AUTHORIZED; PROVIDER TRAFFIC ZERO**.

## E0-D frozen execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. All six RunIds/evidence roots remain frozen, unclaimed and unconsumed. Immutable pair windows remain P01 `2026-09-16T14:30:00Z..23:30:00Z`, P02 `2026-09-17T14:30:00Z..23:30:00Z`, P03 `2026-09-18T14:30:00Z..23:30:00Z`.

## Renewed native authority
Exact checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`; tag `validation/e0d-snapshot-refresh-native-arm64`; evidence `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_NATIVE_ARM64_VALIDATION_2026_09_14.md`; executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`.

Native Windows ARM64: build PASS with 0 warnings/errors; Core 626/626; Harness 154/154; both fixture smokes PASS; credentialless provider-edge refusal PASS with no evidence root/network. Snapshot is valid inclusively through UTC `2026-09-18` and fails closed from `2026-09-19`.

## Continuity
No namespace claim, evidence-root creation, execution credential use, provider request, `countTokens`, generation, inference, scoring, or spend is authorized or consumed. Every slot still requires a fresh execution-sensitive authenticated capacity observation at its own launch boundary after any predecessor terminal. Q-E0E-RUN remains blocked until E0-D closes. Q-ADMIN-03 predecessor MA-01 remains consumed/failed before Scout spawn. Its single-use successor is also consumed/failed closed after spawning exactly two read-only Scouts: child evidence proved exact fixture reads and denied writes, but root output corrupted one required hash and automatic Cloudflare MCP startup attempted prohibited network access. No retry is authorized; native subagents remain default-off; MA-02+ remain blocked. It creates no E0-D authority.

## Next
Obtain separate Director authorization for the exact P01 Slot 1 activation/live-execution package. Before any claim or provider traffic, recheck current provider/account/key/model/tier/quota facts and obtain the required slot-fresh capacity observation. Do not activate later slots early.
