# Ensemble Current State

Updated: 2026-09-11

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 06 CONSUMED/NONCONTRIBUTING; 3.1 PREACTIVATION NEXT; PROVIDER TRAFFIC NOT AUTHORIZED**.

Runs 03/04/05/06 are immutable/noncontributing and may never be replayed. Run 06 terminal evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN06_TERMINAL_EVIDENCE_ANALYSIS_2026_09_11.md`.

## Validation / evidence
Current native authority remains exact checkout `bb869fb1c505603612bc718f739b3f1b358e5539`; tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`; tag object `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`. Native ARM64 Core **622/622**, Harness **140/140**, build/smokes/credentialless/repository gates PASS.

Run 05 diagnostic-hardening integration closeout: `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_INTEGRATION_CLOSEOUT_2026_09_10.md`. Run 06 activation predecessor: `docs/evidence/E0A_Q_E0A_03_G35L_RUN06_PREEXECUTION_ACTIVATION_2026_09_11.md`; PR #75 merged as `2ec8905f2998f94e2650e89510989753c589ce43` and post-merge Validation #656 (`34570969833`) passed. Run 06 then executed exactly once from the validated executable. Diagnostic hardening worked; no source correction is justified by Run 06.

## Continuity
Run 06 terminal: `TechnicalFailure`, accepted turns `0`; Performer `countTokens` PASS at `724`; first Performer generation HTTP `503` / `UNAVAILABLE`; no response ID, returned model, usage receipt, or structured output; shadow estimate USD `0.16405720`; unknown provider usage `true`. Provider traffic is **zero after Run 06**. No retry/replay/fallback/substitution occurred or is authorized.

Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C9 are DONE; C10 is separate and creates no Engineering/provider/validation authority.

## Next
Preactivate the frozen `GEMINI-3.1-FLASH-LITE-MINIMAL` full-reference comparator. Before any 3.1 provider traffic, independently reverify current model/lifecycle, Free-tier route, account/project/key association, RPM/TPM/RPD limits, pricing/data-use assumptions, executable/profile compatibility, freshness guard, exact validated executable/tag, fresh RunId/evidence root, and standing-authority boundaries. Durable exact-run activation and applicable Director authority are required before traffic. Do not retry 3.5 or start 2.5 first.
