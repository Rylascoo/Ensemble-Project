# E0-A Q-E0A-03 Post-Run-08 Reference Selection - Director Input

Date: 2026-09-11

Status: **DIRECTOR DECISION INPUT - NO PROVIDER AUTHORITY**

Authority boundary: reference selection, provider admission, and disposition of the planned 2.5 control are Director-owned. Engineering/Evidence may classify sealed evidence and recommend a disposition but cannot make those decisions here.

## Current evidence state

Run 08 `E0A-Q03-G35L-20260911-08` is the first Q-E0A-03 full candidate to satisfy the frozen 12-turn contribution law. Its runtime seal is independently verified and its `ensemble.e0a.hard-gates.v1` evaluation is sealed PASS with no findings. Evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN08_TERMINAL_CONTRIBUTION_ANALYSIS_2026_09_11.md`.

No prior run is suppressed:

| Run | Model | Accepted turns | Terminal / contribution |
|---|---|---:|---|
| Run 01 | 3.5 Flash-Lite | 0 | corrected `countTokens` reached provider; first generation HTTP 400; noncontributing |
| Run 02 | 3.5 Flash-Lite | 0 | generation HTTP 400 localized to former structured-output field; noncontributing |
| Run 03 | 3.5 Flash-Lite | 6 | `InvalidOutput` at Turn 7 Interpreter semantic contract; noncontributing |
| Run 04 | 3.5 Flash-Lite | 0 | deterministic canonical control-ID rejection; noncontributing |
| Run 05 | 3.5 Flash-Lite | 0 | Interpreter-generation `TechnicalFailure`; noncontributing |
| Run 06 | 3.5 Flash-Lite | 0 | first Performer generation HTTP 503 / `UNAVAILABLE`; noncontributing |
| Run 07 | 3.1 Flash-Lite | 2 | Turn 3 Integrity HTTP 503 / `UNAVAILABLE`; noncontributing |
| Run 08 | 3.5 Flash-Lite | 12 | `AcceptedTurnCapReached`; runtime/evaluation seals valid; **CONTRIBUTING** |

Runs 01-07 remain immutable evidence of compatibility, local-contract hardening, and reliability. Run 08 does not retroactively convert any prior run into contribution.

## Reference-selection law

The frozen closure contract states that only a contributing 12-turn candidate may become the E0-A reference. When exactly one full candidate contributes after the other receives its separately authorized noncontributing result, the contributing candidate may be selected as the reference while the failed comparator remains reliability/compatibility evidence.

That condition is now satisfied by Run 08. No blind 3.5-vs-3.1 quality comparison is applicable because two full candidates did not contribute.

## Engineering recommendation - reference

**Select Run 08 as the E0-A reference.**

Rationale:

1. It is the only full candidate that reached 12 accepted turns and passed the independent hard-gate evaluation.
2. It uses the already admitted Standard/Free 3.5 profile, unchanged Fixture, unchanged deterministic authority, one attempt per role invocation, and zero retries/fallbacks.
3. Selection preserves rather than hides Runs 01-07; their failure modes remain part of the reference-acquisition reliability record.
4. No alternate full candidate is eligible, so no post-hoc scalar quality weighting or blind winner choice is required.

Selection would authorize creation of the sealed reference descriptor; it would not itself authorize any provider call.

## Planned 2.5 Flash-Lite control

The three-turn `gemini-2.5-flash-lite` exact-no-thinking control remains a planned control, not a full-reference candidate. It cannot replace or defeat Run 08's full-reference eligibility.

The control was frozen before the current output existed. For experimental completeness, Engineering recommends **retaining and separately executing the planned three-turn control after Run 08 reference selection**, but only after its own current model/project/key/quota/pricing/data-use/native/profile activation gates are audited and made durable. This requires separate provider authority; this input supplies none.

A lawful alternative is explicit omission. The closure contract permits omission only by Director disposition, with the limitation reported rather than silently skipping the admissible planned control. Omission minimizes provider consumption but leaves the frozen no-thinking control unobserved.

## Director decisions requested

After this input is integrated, record two explicit dispositions:

1. **Reference:** select Run 08 as the E0-A reference, or reject/hold selection.
2. **2.5 control:** separately authorize preregistration/activation of the planned three-turn control, or explicitly omit it with the unobserved-control limitation recorded.

Until those decisions are durable, Q-E0A-03 remains **ACTIVE / REFERENCE SELECTION PENDING**, Q-E0B-01 remains blocked, and provider traffic remains **NOT AUTHORIZED**.
