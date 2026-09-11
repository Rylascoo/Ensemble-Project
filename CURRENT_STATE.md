# Ensemble Current State

Updated: 2026-09-11

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 06 PREREGISTERED; PREEXECUTION GATES PASS; ONE EXECUTION ELIGIBLE ONLY AFTER ACTIVATION INTEGRATION + POST-MERGE VALIDATION**.

Runs 03/04/05 are immutable/noncontributing and may never be replayed. Fresh Run 06 is exactly `E0A-Q03-G35L-20260911-06`; activation: `docs/evidence/E0A_Q_E0A_03_G35L_RUN06_PREEXECUTION_ACTIVATION_2026_09_11.md`.

## Validation / activation
Current native authority is exact checkout `bb869fb1c505603612bc718f739b3f1b358e5539`; tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`; tag object `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`. Native ARM64 Core **622/622**, Harness **140/140**, build/smokes/credentialless/repository gates PASS.

Run 05 hardening is integrated; PR #73 closeout merged to `main` as `51fadfa610548c11bb010effaae6fc644820c080` and post-merge Validation #648 (`34569178755`) passed. Integration closeout: `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_INTEGRATION_CLOSEOUT_2026_09_10.md`. Run 06 uses the same validated executable, frozen Missing-Raft Fixture, `GEMINI-3.5-FLASH-LITE-MINIMAL / CREATIVE-MINIMAL`, accepted cap 12, zero retries/fallback, and a fresh nonexistent evidence root. Public provider facts were rechecked 2026-09-11; freshness guard remains valid through 2026-09-14; quota snapshot remains reusable with no contrary signal.

## Continuity
Provider traffic is **zero after Run 05** and Run 06 consumption is **0 calls**. Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C9 are DONE; C10 is separate and creates no Engineering/provider/validation authority.

## Next
Integrate the Run 06 activation package through hosted CI and require green post-merge Validation. Then execute Run 06 exactly once from `bb869fb1c505603612bc718f739b3f1b358e5539`; its own `countTokens` is the first provider operation. No replay/retry/fallback/substitution; do not start 3.1/2.5 first.
