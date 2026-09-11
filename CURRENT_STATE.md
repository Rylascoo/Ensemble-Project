# Ensemble Current State

Updated: 2026-09-11

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 05 NONCONTRIBUTING; DIAGNOSTIC HARDENING INTEGRATED/POST-MERGE VALIDATED; FRESH 3.5 PREREGISTRATION ELIGIBLE**.

Runs 03/04/05 are immutable/noncontributing and may never be replayed. Run 05 terminal evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN05_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

## Validation / integration
Current native authority is exact checkout `bb869fb1c505603612bc718f739b3f1b358e5539`; tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`; tag object `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`. Native ARM64 Core **622/622**, Harness **140/140**, fresh build/smokes/credentialless/repository gates PASS; provider network NONE.

PR #72 integrated the package to `main` as `fdfd5c69f6d439fd9cdc738be52366a3309b9597`; branch gates passed and post-merge Validation #645 (`34567839177`) succeeded. Integration closeout: `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_INTEGRATION_CLOSEOUT_2026_09_10.md`. Documentation/integration commits do not inherit native runtime authority.

## Continuity
Provider traffic is **zero** after Run 05. Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C9 are DONE; C10 is separate and creates no Engineering/provider/validation authority.

## Next
Preregister one fresh 3.5 Flash-Lite reference candidate against exact validated checkout `bb869fb1c505603612bc718f739b3f1b358e5539` under standing authority, then satisfy the frozen preexecution/activation gates before any provider traffic. Never replay Runs 03/04/05 or start 3.1/2.5 first.
