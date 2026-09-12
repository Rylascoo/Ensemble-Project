# Ensemble Current State

Updated: 2026-09-11

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/04 **DONE**. Q-E0A-03 **ACTIVE - RUN 08 CONTRIBUTING; HARD GATES PASS; DIRECTOR REFERENCE SELECTION + 2.5 CONTROL DISPOSITION PENDING; PROVIDER TRAFFIC BLOCKED**.

Runs 01-07 remain immutable/noncontributing. Run 08 terminal: `docs/evidence/E0A_Q_E0A_03_G35L_RUN08_TERMINAL_CONTRIBUTION_ANALYSIS_2026_09_11.md`; Director input: `docs/evidence/E0A_Q_E0A_03_POST_RUN08_REFERENCE_SELECTION_DIRECTOR_INPUT_2026_09_11.md`.

## Validation / evidence
Native authority remains `bb869fb1c505603612bc718f739b3f1b358e5539`; tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`; tag object `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`. Native ARM64 Core **622/622**, Harness **140/140**.

Run 08 activation PR #86 merged as `e25fa2e8672a15aca6bc29a7abacfeca2db3a5bc`; rebased exact-head Validation #689, E0-E preparation #41, and post-merge Validation #690 passed. Run 08 then executed exactly once: 12/12 accepted turns, 36/36 exact-model generations successful, 72 provider operations total, known usage throughout, shadow USD `0.05258350`.

Independent runtime audit: 102 rooted artifacts, zero digest mismatches, runtime root `e0249ac54353ca0fb550ba68bbb42eee6c0bf891e1fe2c7c61cbe3fb6420f459`, seal identity `3133fb3c83d177446494e761362a33bfcd8cc54da8729de46200430c544ace97`. `ensemble.e0a.hard-gates.v1` evaluation is sealed **PASS**, no findings. Provider traffic is **zero after Run 08**.

## Continuity
Exactly one full candidate now contributes: Run 08 / 3.5 Flash-Lite. The frozen contract therefore permits Director selection of Run 08 as the E0-A reference; no two-candidate blind comparison applies. Selection is not inferred. The planned 2.5 three-turn exact-no-thinking control cannot become the reference and still requires explicit Director execution-or-omission disposition.

Q-E0B-01 remains blocked until Q-E0A-03 closes. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C9 are DONE; C9A amendment integrated but its current realization is uncommissioned after bounded timeout falsification; C10 falsified; C11+ blocked.

## Next
Integrate the Run 08 terminal/contribution and Director-input package. Then obtain explicit Director dispositions for (1) Run 08 reference selection and (2) planned 2.5 control execution or omission. No provider call before a separately durable 2.5 activation if execution is chosen. If Run 08 is selected and the control is explicitly omitted, build the sealed reference descriptor and Q-E0A-03 closure package; Q-E0B-01 is the immediate lawful successor.
