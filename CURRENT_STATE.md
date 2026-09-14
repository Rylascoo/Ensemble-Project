# Ensemble Current State

Updated: 2026-09-13

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0 experimental program - Q-E0C-01 repeated identical-condition runs**. E0-A/E0-B are **DONE**. Q-E0C-01 is **ACTIVE / DIRECTOR HOLD - BOTH FRESH SLOTS CONSUMED; SLOT 1 CONTRIBUTING; SLOT 2 NONCONTRIBUTING + EXECUTION-LAW DEVIATION; P01 SCORE SEALED / MAPPING WITHHELD; PROVIDER TRAFFIC ZERO**.

Corrected method: `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md`; preregistration: `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_PREREGISTRATION_2026_09_12.md`; deviation/Director input: `docs/evidence/E0C_Q_E0C_01_SLOT2_EXECUTION_LAW_PROTOCOL_DEVIATION_DIRECTOR_INPUT_2026_09_13.md`.

## Batch result
Slot 1 `E0C-Q01-IDENT-20260912-01`: consumed, `AcceptedTurnCapReached`, 12/12, sealed/hard-gate PASS, **CONTRIBUTING**. Slot 2 `E0C-Q01-IDENT-20260912-02`: consumed, `InvalidOutput`, 0/12, exact-model provider success, sealed/hard-gate PASS, **NONCONTRIBUTING**; typed control emitted `MARLOE` instead of `MARLOWE`. No replay/replacement/third slot or further E0-C provider launch exists.

## Protocol deviation
Slot 2 reused the authenticated pre-Slot-1 quota snapshot plus conservative arithmetic instead of obtaining the fresh authenticated post-Slot-1 model/tier/quota/capacity observation that the standing amendment kept execution-sensitive. Slot 2 therefore should have remained blocked; provider success does not cure the breach.

## P01 containment
PR #121/#122 prepared and sanitized the blind package/mapping commitment; exact-main Validation #778 passed before scorer access. PR #123 then sealed the ten-dimension blind score at `e1c20e6f...`; exact-main Validation #783 passed. PR #125 integrated the protocol-deviation Director hold at main `506f4804...`; exact-main Validation #786 passed. Mapping was unknown during scoring/seal, remains withheld, and no reveal/unblind artifact exists. Preserve the score, but **do not rescore, reveal, unblind, regenerate, replace, or use it for closure** before Director disposition.

## Continuity
Q-E0D-01 remains blocked. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-C/D. Provider traffic is **ZERO**.

## Next
Director chooses whether to accept the already-sealed blind P01 score and authorize reveal/verification of the existing mapping for descriptive E0-C closure, or close E0-C without using experiential scoring because of the execution-process breach. No mapping reveal/unblinding, provider traffic, E0-D advancement, or E0-C replacement work before that decision.
