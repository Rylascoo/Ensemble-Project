# Ensemble Current State

Updated: 2026-09-15

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program â€” Q-E0D-01 ablation controls**. E0-A/E0-B/E0-C are **DONE**. E0-D is **ACTIVE â€” P01 SLOT 1 PRECLAIM CHECKOUT-GUARD REFUSAL; NAMESPACE UNCONSUMED; PROVIDER TRAFFIC ZERO; DIRECTOR DISPOSITION REQUIRED BEFORE ANY FURTHER EXECUTION**.

## E0-D execution identity
Allocation: `docs/evidence/E0D_Q_E0D_01_PREEXECUTION_ALLOCATION_2026_09_14.json`. P01 timing is Director-amended by `docs/evidence/E0D_P01_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_15.md` to `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`; P02/P03 remain unchanged. All deterministic evidence roots/claims remain unclaimed; P01 Slot 1 execution disposition is unresolved after a preclaim runtime guard refusal.

## Native/provider authority
Checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`; tag `validation/e0d-snapshot-refresh-native-arm64`; executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`; snapshot valid through UTC `2026-09-18`. Provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier with the new CurrentUser-protected credential; the old credential remains ineligible.

## Continuity
PR #150 integrated the timing amendment to `main@40409d0d438afe4631c07e41d38fa1179d358664`; exact-main Validation #866 PASS. Immediately prelaunch AI Studio showed exact `Gemini 3.5 Flash Lite` at `0/15` RPM, `0/250K` TPM, `0/500` RPD. The authorized Harness child then exited `1` with `E0-A Git worktree does not match the live-run authority.` before evidence-store construction. `docs/evidence/E0D_P01_SLOT1_PRECLAIM_CHECKOUT_GUARD_REFUSAL_2026_09_15.md` records absent root/claims, zero provider traffic, source-order proof, and the corrected-but-unexecuted local launcher.

## Next
**Hard stop before any further P01 execution.** The Director must explicitly decide whether the preclaim checkout-guard refusal exhausted the one-execution/no-retry authority or whether one corrected preclaim launch is authorized because no namespace/provider operation occurred. Do not infer either outcome. If a corrected launch is authorized, repeat the immediately-preclaim authenticated capacity observation and every frozen interlock first. P01 Slot 2 and later slots remain separately unauthorized; Q-E0E-RUN remains blocked.
