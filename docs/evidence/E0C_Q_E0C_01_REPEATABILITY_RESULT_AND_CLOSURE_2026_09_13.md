# E0-C Q-E0C-01 P01 Repeatability Result and Closure

Date: 2026-09-13

Status: **CLOSED — DIRECTOR ACCEPTED SEALED P01 AS UNAFFECTED EVIDENCE — VERIFIED MAPPING AUTHORIZED FOR DESCRIPTIVE CLOSURE ONLY — NO RESCORE — SLOT 2 BREACH PRESERVED INDEPENDENTLY — PROVIDER TRAFFIC ZERO**

## Director disposition

The Director disposition is:

> Accept the already-sealed P01 score as unaffected evidence. Authorize use of the already-verified committed mapping solely for descriptive E0-C closure. Do not rescore. Preserve Slot 2 as permanently consumed/noncontributing and record its execution-law breach independently.

This disposition resolves the hold created by the Slot 2 execution-law deviation and the later post-score mapping-exposure race. It authorizes use of the existing mapping only to interpret the already-sealed blind P01 score. It does not authorize rescoring, regeneration, replay, replacement, a third slot, provider traffic, or reinterpretation of Slot 2.

## Preserved evidence and chronology

Slot 1 `E0C-Q01-IDENT-20260912-01` remains the sole fresh contributing E0-C repeat: 12/12 accepted turns, valid runtime/evaluation seals, hard-gate PASS, and no defect found by the recursive two-slot audit.

Slot 2 `E0C-Q01-IDENT-20260912-02` remains permanently consumed/noncontributing: `InvalidOutput`, 0/12, valid runtime/evaluation seals, and no replay/replacement. Its first Performer request body matched Slot 1 byte-for-byte apart from run-specific prepared identity. The model emitted invalid control ID `MARLOE` instead of canonical `MARLOWE`; deterministic validation rejected it before accepted production state.

Independently, Slot 2 violated execution law because activation reused the authenticated pre-Slot-1 capacity snapshot plus arithmetic instead of obtaining the required fresh authenticated post-Slot-1 capacity observation. Provider success does not cure that breach, and the breach does not causally explain `MARLOE`. The independent deviation record remains `docs/evidence/E0C_Q_E0C_01_SLOT2_EXECUTION_LAW_PROTOCOL_DEVIATION_DIRECTOR_INPUT_2026_09_13.md`; recursive audit/hardening is `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_RECURSIVE_AUDIT_AND_PROCESS_HARDENING_2026_09_13.md`.

P01 was scored while blind. `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_2026_09_13.json` records `mappingKnownDuringScoring=false`; the immutable seal `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_SEAL_2026_09_13.json` records `mappingKnownAtSeal=false` and `scoresSealedBeforeMappingReveal=true`, binding score SHA-256 `e074ca6ad1c398fdcddfa97802f0e32d74c540655182cec2b06f73473d705988`. Exact-main Validation #783 passed before the mapping was read.

A concurrent Engineering session then verified the external mapping SHA-256 `5d9a40d1ed5619f97015b06b47074a60c1cd66e0cbd265a937be730ac874cf6c` against the pre-scorer commitment and read it after seal, before discovering the later Director hold. No score byte or scoring judgment changed after exposure. That chronology is preserved by `docs/evidence/E0C_Q_E0C_01_POSTSCORE_MAPPING_REVEAL_RACE_RECONCILIATION_2026_09_13.md`.
## Durable E0-C predecessor chain

This closure remains the current authority root for the E0-C method and evidence that explain the closed result:

- `docs/blueprint/E0C_REPEATED_IDENTICAL_CONDITION_METHOD_PROPOSAL_01.md`
- `docs/evidence/E0C_Q_E0C_01_PREPARATION_AUDIT_AND_DIRECTOR_INPUT_2026_09_12.md`
- `docs/evidence/E0C_Q_E0C_01_METHOD_DIRECTOR_DECISION_2026_09_12.md`
- `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_PREREGISTRATION_2026_09_12.md`
- `docs/evidence/E0C_Q_E0C_01_TIMING_LAW_RECONCILIATION_2026_09_12.md`
- `docs/evidence/E0C_Q_E0C_01_BLIND_REPEATABILITY_INSTRUMENT_2026_09_12.json`
- `docs/evidence/E0C_Q_E0C_01_PUBLIC_FACT_AUDIT_2026_09_12.md`
- `docs/evidence/E0C_Q_E0C_01_STANDING_PROJECT_KEY_ASSOCIATION_DIRECTOR_AMENDMENT_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_RUN01_PREACTIVATION_2026_09_12.md`
- `docs/evidence/E0C_Q_E0C_01_RUN01_FRESH_AUTHENTICATED_RATE_LIMIT_INTAKE_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_RUN01_PREEXECUTION_ACTIVATION_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_RUN01_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_RUN02_PREEXECUTION_ACTIVATION_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_RUN02_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_P01_BLIND_PACKAGE_2026_09_13.json`
- `docs/evidence/E0C_Q_E0C_01_P01_MAPPING_COMMITMENT_2026_09_13.json`
- `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_2026_09_13.json`
- `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_SEAL_2026_09_13.json`
- `docs/evidence/E0C_Q_E0C_01_SLOT2_EXECUTION_LAW_PROTOCOL_DEVIATION_DIRECTOR_INPUT_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_POSTSCORE_MAPPING_REVEAL_RACE_RECONCILIATION_2026_09_13.md`
- `docs/evidence/E0C_Q_E0C_01_TWO_SLOT_RECURSIVE_AUDIT_AND_PROCESS_HARDENING_2026_09_13.md`

## Mapping reveal and verification

The contemporaneous reveal record is now integrated as `docs/evidence/E0C_Q_E0C_01_P01_MAPPING_REVEAL_2026_09_13.json`. Its integration records the already-verified historical reveal; it is not a new reveal or rescore.

- LEFT = Slot 1 `E0C-Q01-IDENT-20260912-01`.
- RIGHT = selected reference Run 08 `E0A-Q03-G35L-20260911-08`.
- The revealed mapping hash exactly matches the committed pre-scorer mapping hash.
- Both source transcript hashes match the reveal record.
- Blind-package member contents match the revealed source transcripts.

## Unblinded descriptive P01 matrix

The sealed blind choices are unchanged by unblinding.

Slot 1 was preferred for **Character distinction**, **Coherence**, and **Assistant convergence avoidance**. The sealed evidence attributes these preferences to steadier separation among the speaking registers and cleaner local continuity.

Run 08 was preferred for **Social causality**, **Character agency**, **Relationship expression**, **Earned surprise**, **Anticipation**, **Memorability**, and **Mechanical invisibility**. The sealed evidence attributes these preferences to stronger causal escalation, trust/consent stakes, character-specific agency, forward pull, memorable conflict, and less visibly repetitive orchestration.

The frozen blind questions resolve consistently: Slot 1 felt most like distinct people; Run 08 made later actions feel more caused by earlier actions, preserved asymmetric knowledge more convincingly, and created stronger anticipation. The scorer classified the pair as **MEANINGFUL_DIFFERENCE**, not functionally equivalent for repeatability purposes.

## Interpretation boundary

P01 is accepted as unaffected **descriptive repeatability/dispersion evidence**. It is not a weighted or aggregate master score, not a statistical-significance result, and not a reference-replacement decision. Dimension counts must not be converted post hoc into a winner score.

Run 08 remains the selected E0-A same-model reference. Slot 1 demonstrates that a clean matched-condition repeat can preserve strong character distinction/coherence while materially differing in causal progression, relationship stakes, agency, anticipation, memorability, and mechanical visibility.

Slot 2 contributes no experiential score. It remains reliability/process evidence only: permanently consumed/noncontributing, with both the `InvalidOutput` terminal and the independent execution-sensitive capacity-gate breach preserved.

## Closure and successor boundary

Q-E0C-01 is **DONE**. Both fresh namespaces are consumed; no retry, replacement, third slot, alternate route/model/key/tier, window extension, rescore, or further E0-C provider launch exists.

Q-E0D-01 becomes the immediate experiment-order successor for **method/preregistration preparation only**. This closure does not authorize E0-D provider requests, credentials, spend, namespace claims, implementation shortcuts, or launch. Any such step requires its own applicable authority and gates.

The hardening findings from the two-slot recursive audit remain deferred to Q-E0-CONV and must not retroactively mutate E0-C evidence: machine-verifiable execution-sensitive gate provenance, explicit concurrent-review launch gates when intended, and evaluation of roster-bounded structured-output constraints after the frozen E0 boundary.

Provider traffic during this Director-disposition closure is **ZERO**.
