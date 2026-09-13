# E0-C - Repeated Identical-Condition Method Proposal 01

Date: 2026-09-12

Status: **PROPOSAL 0.1 - NON-PROVIDER PREPARATION COMPLETE - DIRECTOR METHOD DECISION REQUIRED - NO RUN OR NETWORK AUTHORITY**

## 1. Objective and frozen authority

The frozen E0 sequence defines E0-C as **repeated identical-condition runs** after E0-B. E0-C therefore tests repeatability/dispersion of the selected E0-A same-model reference condition under stochastic provider output; it does not test a new model, cast, Fixture, prompt, authority rule, or source implementation.

Selected reference authority is `docs/evidence/E0A_Q_E0A_03_RUN08_SELECTED_REFERENCE_DESCRIPTOR_2026_09_11.json`, sourced from contributing Run 08 `E0A-Q03-G35L-20260911-08`.

Recommended condition identifier: `E0C-IDENTICAL-REFERENCE-01`.

## 2. Exact reference condition

Every controllable experimental variable remains the selected Run 08 reference:

- provider/model/profile: Google Gemini API `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- service tier request: `standard`;
- Performer = native `minimal`, Integrity = native `high`, Interpreter = native `minimal`;
- Performer/Interpreter streaming behavior exactly as Run 08;
- `maxOutputTokens=4096`, accepted-Turn cap `12`, attempts per invocation `1`, automatic retries `0`, attempt timeout `300s`;
- estimated model-token spend ceiling USD `5.00` per fresh run;
- Fixture `ensemble.e0.missing-raft@0.1.0`, hash `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- same Character definitions, Access/Context law, prompt/schema contracts, deterministic Director/Integrity/State Authority/Take/commit/Opportunity semantics, evidence/sealing law, and hard-gate checklist.

Each fresh repeat begins from canonical Fixture genesis. No state, accepted history, provider conversation state, or hand-edited material carries between runs.

## 3. Recommended bounded sample plan

Recommend **two fresh repeat slots** in addition to already-selected Run 08, yielding three identical-condition observations if both fresh slots contribute.

This is the smallest nontrivial bounded batch that can compare the anchor against more than one fresh stochastic realization. It is sufficient for descriptive repeatability evidence but explicitly **not** for statistical-significance claims.

Both fresh slots must be preregistered before the first fresh execution. The Director may approve this two-repeat plan, modify the count before any RunId exists, or reject it.

No RunId or evidence root is created by this proposal.

## 4. Exact executable and zero-source-change law

Recommended executable is the exact Run 08 native-validated checkout `bb869fb1c505603612bc718f739b3f1b358e5539`, tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`.

The preserved Windows ARM64 validator worktree is clean at that exact commit; the Harness executable and canonical Fixture are present; the existing `e0a-run` path accepts the reference variant/profile, fresh RunId/root, Fixture, and executable commit.

No E0-C source amendment is recommended. Using the later E0-B mixed-cast executable merely because it is newer would add unnecessary source variation to an identical-condition experiment.

If exact Run 08 executable reuse becomes impossible, stop for Director/architecture review rather than silently substituting a newer executable.

## 5. Fresh-slot consumption and anti-cherry-picking law

Each approved fresh slot receives one unique RunId/root and exactly one launch after its own current execution-time gates pass.

- any namespace claim consumes that slot;
- technical failure, provider failure, cancellation, refusal, budget terminal, or other noncontributing terminal does not earn a retry or replacement slot;
- the second preregistered slot is an independent repeat, not a retry of the first;
- artistic outcome from the first fresh run cannot cancel, add, or replace the second slot;
- the second slot may launch only after a fresh pre-execution gate confirms its own namespace and current provider/account prerequisites;
- no third fresh slot exists under this recommended method.

For two approved fresh slots the inherited deterministic spend ceiling is USD `5.00` per run / USD `10.00` aggregate maximum shadow reservation. This is a ceiling, not spend authorization or a billing claim.

## 6. Contribution and hard-gate law

Every fresh run is sealed and hard-gate-reviewed independently under the same Run 08 evidence/checklist law.

A fresh run is experiential-comparison eligible only if it:

1. reaches all `12/12` accepted Turns;
2. has complete immutable runtime sealing and valid recomputation;
3. preserves exact configured provider/model/profile identity and complete provenance;
4. passes the independent hard-gate evaluation.

Noncontributing fresh runs remain E0-C reliability/condition evidence and are never hidden. They receive no experiential credit and are not replaced.

## 7. Blind repeatability comparison

Before the first fresh transcript exists, freeze a new E0-C blind scoring instrument using the existing ten-dimension E0 rubric and score choices `LEFT`, `RIGHT`, `TIE`, `INDETERMINATE` with evidence required per dimension, neutral speaker normalization, provider/model metadata hidden, no weighted master score, and scores sealed before mapping reveal.

The purpose is repeatability/dispersion, not selecting a new reference winner.

Eligible pair set is frozen from the preregistered batch:

- Run 08 vs fresh Repeat 1, if Repeat 1 contributes;
- Run 08 vs fresh Repeat 2, if Repeat 2 contributes;
- fresh Repeat 1 vs fresh Repeat 2, if both contribute.

Pair presentation identities/mappings must be frozen before any scoring. A hard-gate-invalid or noncontributing run simply removes its experiential pair(s); the terminal itself remains reported as reliability evidence.

After unblinding, report the per-dimension pairwise matrix and supporting evidence. Do not infer statistical significance, collapse the matrix into a post-hoc weighted master score, or treat stochastic difference as inherently good or bad.

## 8. Execution-time gates

This proposal authorizes no credential access, provider request, inference, or spend.

Before any fresh slot may execute, a separate activation must bind:

- Director-approved sample plan and exact preregistered slot identities;
- new unique RunId/root and absent deterministic claims for that slot;
- exact clean `bb869fb...` executable/tag/Fixture identity and native eligibility;
- frozen E0-C blind instrument and pair-mapping law;
- current public model lifecycle/pricing/data-use/reasoning/rate-limit facts;
- authenticated intended project/key/tier/quota/capacity evidence under current revalidation law;
- protected credential readiness and zero-retry/no-fallback boundaries;
- standing project-relevance/provider authority if still applicable and current.

No provider probe is permitted merely to test availability.

## 9. Director decision requested

Approve, modify, or reject the following coupled method:

> **E0C-IDENTICAL-REFERENCE-01:** execute exactly two fresh, independently consumed repeats of the selected Run 08 condition from the exact Run 08 native executable, with every controllable input/setting held identical; preregister both slots before the first run; allow no retry/replacement slots; seal and hard-gate each run independently; and blind-compare every contributing transcript pair using the frozen ten-dimension E0 rubric as descriptive repeatability evidence.

No execution decision should be inferred from silence or from this proposal's presence in the repository.
