# E0-A Q-E0A-03 — Reference Evidence Closure Audit

Date: 2026-09-11

Status: **CLOSURE READY — RUN 08 SELECTED REFERENCE SEALED — RUN 09 TERMINAL LIMITATION PRESERVED — Q-E0B-01 SUCCESSOR**

## Authority and closure basis

This audit closes Q-E0A-03 under `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md` after the Director selected Run 08 and directed the frozen 2.5 control.

Selected reference decision: `docs/evidence/E0A_Q_E0A_03_RUN08_REFERENCE_AND_G25L_CONTROL_DIRECTOR_DECISION_2026_09_11.md`.

Selected reference terminal evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN08_TERMINAL_CONTRIBUTION_ANALYSIS_2026_09_11.md`.

Selected reference descriptor: `docs/evidence/E0A_Q_E0A_03_RUN08_SELECTED_REFERENCE_DESCRIPTOR_2026_09_11.json`.

2.5 control terminal evidence: `docs/evidence/E0A_Q_E0A_03_G25L_CONTROL_RUN09_TERMINAL_EVIDENCE_ANALYSIS_2026_09_11.md`.

## Complete Q-E0A-03 run record

| Run | Model | Accepted turns | Terminal / contribution |
|---|---|---:|---|
| Run 01 | 3.5 Flash-Lite | 0 | first generation HTTP 400; noncontributing |
| Run 02 | 3.5 Flash-Lite | 0 | structured-output generation HTTP 400; noncontributing |
| Run 03 | 3.5 Flash-Lite | 6 | Turn 7 Interpreter `InvalidOutput`; noncontributing |
| Run 04 | 3.5 Flash-Lite | 0 | deterministic control-ID rejection; noncontributing |
| Run 05 | 3.5 Flash-Lite | 0 | Interpreter-generation `TechnicalFailure`; noncontributing |
| Run 06 | 3.5 Flash-Lite | 0 | Performer generation HTTP 503 / `UNAVAILABLE`; noncontributing |
| Run 07 | 3.1 Flash-Lite | 2 | Turn 3 Integrity HTTP 503 / `UNAVAILABLE`; noncontributing |
| Run 08 | 3.5 Flash-Lite | 12 | `AcceptedTurnCapReached`; sealed hard-gate PASS; **selected contributing reference** |
| Run 09 | 2.5 Flash-Lite | 0 | first Performer `countTokens` HTTP 404 / `NOT_FOUND`; noncontributing control evidence |

No failed/noncontributing run is suppressed by reference selection.

## Selected reference seal

Run 08 is the only full 12-turn candidate that contributed. Its independent runtime audit verified 102 rooted artifacts with zero digest mismatches; runtime root `e0249ac54353ca0fb550ba68bbb42eee6c0bf891e1fe2c7c61cbe3fb6420f459`; runtime seal identity `3133fb3c83d177446494e761362a33bfcd8cc54da8729de46200430c544ace97`; hard-gate evaluation PASS with no findings.

The selected reference descriptor binds the exact source manifest, runtime seal, run-final digest, returned model identity, provider profile/service tier, reasoning controls, output/turn/retry/timeout/spend envelope, canonical Fixture identity/hash, executable commit, native validation tag, and hard-gate evaluation.

Descriptor SHA-256: `606f6be257efbad1c435dff2b109c28d64bd326e98a9fd3bfa5b114e24b8ff56`.

A two-candidate blind comparison is not applicable because exactly one full 12-turn candidate contributed. The absence of a blind comparison is therefore contract-conformant rather than missing evidence.

## Run 09 limitation

Run 09 consumed one provider-bound `countTokens` operation and terminated HTTP 404 / `NOT_FOUND` before generation or semantic consumption. Its runtime seal recomputes exactly and its independent hard-gate evaluation passes.

The final authenticated AI Studio 2.5 model/tier quota and same-day capacity gate was still described as pending in durable state immediately before execution, and no durable artifact proves it was completed before the request. This closure preserves that process defect explicitly. Run 09 is terminal technical evidence but is not credited as a fully gate-compliant successful control.

No retry or replacement is warranted or authorized. The frozen closure law states that a failed or inadmissible 2.5 control does not by itself invalidate an otherwise justified full-reference selection.


## Durable predecessor chain

The following current Q-E0A-03 records remain part of the closure provenance and are intentionally reachable through this audit:

- `docs/evidence/E0A_GEMINI_TECHNICAL_FAILURE_DIAGNOSTIC_CLASSIFICATION_INTEGRATION_CLOSEOUT_2026_09_10.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN06_PREEXECUTION_ACTIVATION_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN06_TERMINAL_EVIDENCE_ANALYSIS_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_G31L_RUN07_PREACTIVATION_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_G31L_RUN07_PREEXECUTION_ACTIVATION_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_G31L_RUN07_TERMINAL_EVIDENCE_ANALYSIS_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_POST_FULL_CANDIDATE_ROUTE_DECISION_INPUT_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_OPTION_A_DIRECTOR_DECISION_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN08_PREEXECUTION_ACTIVATION_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN08_TERMINAL_CONTRIBUTION_ANALYSIS_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_POST_RUN08_REFERENCE_SELECTION_DIRECTOR_INPUT_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_RUN08_REFERENCE_AND_G25L_CONTROL_DIRECTOR_DECISION_2026_09_11.md`
- `docs/evidence/E0A_Q_E0A_03_G25L_CONTROL_RUN09_PREEXECUTION_ACTIVATION_2026_09_11.md`

These records explain the executable/provider hardening path and Director dispositions but do not override this closure audit, `CURRENT_STATE.md`, or the selected reference descriptor.

## Closure checklist

1. Every separately authorized Q-E0A-03 run has a durable terminal record or preserved historical terminal evidence: **PASS**.
2. Selected Run 08 runtime seal independently verified: **PASS**.
3. Selected Run 08 hard-gate evaluation sealed PASS: **PASS**.
4. Two-candidate blind comparison required: **NO — NOT APPLICABLE**.
5. Comparison summary preserves failed/noncontributing runs: **PASS**.
6. Explicit Director reference selection: **PASS**.
7. Sealed reference descriptor: **PASS**.
8. State/queue transition to Q-E0A-03 DONE and lawful successor Q-E0B-01: **TO BE SATISFIED BY THIS CLOSEOUT CHANGESET**.

## Disposition

With this closeout changeset integrated, Q-E0A-03 is **DONE**. Run 08 / `GEMINI-3.5-FLASH-LITE-MINIMAL` is the E0-A selected reference configuration.

Q-E0B-01 becomes the immediate active experiment-order successor. This activation authorizes only E0-B non-provider preparation and evidence work already permitted by project law; it does **not** authorize credentials, provider traffic, inference, spend, or an E0-B run. Any provider execution remains separately gated by current authority and provider facts.

E0-E execution remains blocked until E0-B, E0-C, and E0-D have closed.
