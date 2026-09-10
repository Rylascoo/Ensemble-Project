# Ensemble Current State

Updated: 2026-09-09

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; product/lane authority: `docs/PROJECT_AUTHORITY.md`; orchestration: `docs/PROJECT_AGENT_ORCHESTRATION_PROTOCOL.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness — Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE — RUN 01 TERMINATED; AUTHORIZATION CONSUMED; TERMINAL EVIDENCE ANALYSIS NEXT**.

Exact RunId `E0A-Q03-G35L-20260909-01`; route `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL`; executable `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; validation tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`. Director reports `PROVIDER_INVOCATION_STARTED=YES`, terminal `TechnicalFailure`, accepted turns **0**, estimated shadow spend USD **0.164035**. Exact Run 01 authorization is **CONSUMED**; provider authority is **NONE**. No retry/rerun/fallback/probe/alternate provider/model is authorized.

## Validation
Native authority remains `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`; tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`. Windows ARM64 Core **622/622**, Harness **131/131**, build/smokes/credentialless PASS from the existing validation checkpoint. The terminal provider result does not promote that validation rung.

## Evidence boundary
Continuity intake: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_CONTINUITY_INTAKE_2026_09_09.md`. It records Director-supplied terminal facts and local evidence pointers/hashes; the connected repository-side migration did **not** inspect the local ZIP/root and makes no causal diagnosis. Preserve runtime facts separately from hypotheses. Do not modify runtime source until preserved evidence proves a defect.

## Continuity
Q-E0E-PREP remains DONE; Q-E0E-RUN remains blocked through E0-A→D. Design authority is separate; latest reconciled Website checkpoint during this migration was `b0464e6a63bee3075a9da5acc74864a57ff78e5f` with Q-DESIGN-19 still the Design next action. Long personality/handoff prompts are non-authoritative continuity aids after repository-law migration.

## Next
On the Director Windows ARM64 machine, inspect the preserved Run 01 evidence root/ZIP/console, verify supplied hashes/integrity, classify the `TechnicalFailure`, and create the terminal evidence analysis record. Determine from evidence whether any source correction is warranted. **Do not change runtime source before a defect is established. Do not send provider traffic without new explicit Director authorization.**