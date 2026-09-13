# Ensemble Current State

Updated: 2026-09-13

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0C-01 repeated identical-condition runs**. E0-A/E0-B are **DONE**. Q-E0C-01 is **ACTIVE - SLOT 1 SEALED/HARD-GATE PASS/CONTRIBUTING; SLOT 2 FRESH ACTIVATION PENDING; PROVIDER TRAFFIC ZERO**.

Corrected method: `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md`; canonical preregistration: `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_PREREGISTRATION_2026_09_12.md`; standing association law: `docs/evidence/E0C_Q_E0C_01_STANDING_PROJECT_KEY_ASSOCIATION_DIRECTOR_AMENDMENT_2026_09_13.md`.

## Canonical batch
Slot 1 `E0C-Q01-IDENT-20260912-01`; Slot 2 `E0C-Q01-IDENT-20260912-02`. Exact executable `bb869fb1c505603612bc718f739b3f1b358e5539`; blind instrument SHA-256 `7857699de82904f52892f3e6d0f9794c80e50ee488c9e30ef9fdb13ba4edfd64`. Order is Slot 1 then Slot 2; immutable UTC window is **2026-09-13T20:30:00Z through 2026-09-13T23:30:00Z**. No retry/replacement/third slot/window extension.

## Slot 1 result
Terminal evidence: `docs/evidence/E0C_Q_E0C_01_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`. One launch ran `20:59:27Z`-`21:04:34Z`, exit 0: `AcceptedTurnCapReached`, 12/12 accepted, 72 API operations, exact returned `gemini-3.5-flash-lite` on all 36 generations, generation usage 73,134 input / 10,594 output incl. 7,603 reasoning / 0 cached, shadow USD 0.04842520. Runtime root `311c7196...`; hard gates PASS; Slot 1 is contributing and consumed.

The Harness-created authoritative claims are `run-359528129a...claim` and `root-7d18332dbe...claim`. Earlier predicted claim filenames were a preflight-oracle error; no replay authority exists. Provider traffic is zero after the terminal.

## Continuity
Q-E0D-01 remains blocked on E0-C closure. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-C/D. Only sealed 12/12 hard-gate-PASS transcripts enter blind comparison; no scoring/unblinding yet.

## Next
Integrate and exact-main validate Slot 1 terminal closeout/usage ledger. If still inside the frozen window, construct Slot 2's own fresh activation from post-Slot-1 namespace/native/provider/account/credential facts; integrate + exact-main validate it before any Slot 2 provider traffic.
