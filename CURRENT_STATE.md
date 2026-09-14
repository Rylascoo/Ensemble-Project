# Ensemble Current State

Updated: 2026-09-14

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program — Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE — SOURCE SNAPSHOT REFRESH + NATIVE REVALIDATION AUTHORIZED; LIVE EXECUTION NOT AUTHORIZED; PROVIDER TRAFFIC ZERO**.

## E0-D frozen execution identity
Allocation remains `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. Six RunIds/evidence roots remain frozen, unclaimed and unconsumed. Pair windows remain immutable: P01 `2026-09-16T14:30:00Z..23:30:00Z`; P02 `2026-09-17T14:30:00Z..23:30:00Z`; P03 `2026-09-18T14:30:00Z..23:30:00Z`.

## Snapshot refresh boundary
Director authorization: `docs/evidence/E0D_SOURCE_SNAPSHOT_REFRESH_AND_NATIVE_REVALIDATION_DIRECTOR_AUTHORIZATION_2026_09_14.md`. Fresh official facts remain compatible with the frozen Run-08 route. Candidate source refresh changes only `E0AGeminiPricingPolicy`: `VerifiedOn=2026-09-14`, inclusive `SnapshotValidThrough=2026-09-18`, fail closed from UTC 2026-09-19. Deterministic/native validation is pending.

Existing promoted native authority remains `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891` / `validation/e0d-ablation-controls-native-arm64` until a renewed exact checkout is actually validated and tagged.

## Continuity
No namespace claim, evidence-root creation, provider request, `countTokens`, generation, inference, scoring, spend, or live activation is authorized or consumed. Every future slot still requires its own fresh execution-sensitive capacity observation and separate live-execution authority. Q-E0E-RUN remains blocked until E0-D closes. Q-ADMIN-03 MA-01 remains consumed/failed closed and creates no E0-D authority.

## Next
Complete deterministic regression/static checks and renewed native Windows ARM64 validation on the exact snapshot-refresh candidate. If every gate passes, create a new annotated validation tag and integrate the validation/evidence checkpoint. Stop before any slot claim or provider traffic.
