# E0-C Q-E0C-01 - Preparation Audit and Director Input

Date: 2026-09-12

Status: **CORRECTED METHOD AUDIT - DIRECTOR TIMING AMENDMENT RATIFIED - CANONICAL IDENT PREREGISTRATION RECONCILIATION REQUIRED - NO RUN OR PROVIDER TRAFFIC**

## Authority recovered

Authoritative `main` at preparation start: `61516baff6af5c2b6d33223e89543fcbd6bb78d2`.

`CURRENT_STATE.md` records Q-E0B-01 DONE under Director Option A and Q-E0C-01 ACTIVE for non-provider preparation only.

E0-B closure authority: `docs/evidence/E0B_Q_E0B_01_OPTION_A_DIRECTOR_DECISION_AND_CLOSURE_2026_09_12.md`.

Selected same-model reference: `docs/evidence/E0A_Q_E0A_03_RUN08_SELECTED_REFERENCE_DESCRIPTOR_2026_09_11.json`.

Prepared method proposal: `docs/blueprint/E0C_REPEATED_IDENTICAL_CONDITION_METHOD_PROPOSAL_01.md`.

The frozen roadmap specifies only that E0-C is repeated identical-condition runs returning to the E0-A reference. It does not specify repetition count, RunIds/roots, executable identity, blind-comparison topology, or provider activation details. Those gaps must be decided before execution.

## Zero-source-change audit

The cleanest E0-C implementation is no implementation at all.

The preserved native validator `C:\Users\Wiryl\Sol Dev\E0V-bb869fb` is clean at exact Run 08 executable `bb869fb1c505603612bc718f739b3f1b358e5539`; native tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64` peels to the same commit; the win-arm64 Harness executable and canonical Missing-Raft Fixture are present.

That checkout exposes the existing `e0a-run <variant> <provider-profile> <fixture> <run-id> <evidence-root> <executable-commit>` host path needed to repeat the selected reference with fresh identity only.

No Core, Harness, Fixture, prompt, schema, provider-profile, evidence-format, or deterministic-authority source change is required by E0-C. The later E0-B `d1073fe...` executable is therefore not recommended for the repetition arm: although valid for E0-B, substituting newer source would weaken identical-condition isolation without necessity.

## Sample-count audit

Repository law requires plural repeated identical-condition evidence but provides no numeric sample count.

Recommended plan is **two fresh repeats**, giving three observations including Run 08 if both fresh runs contribute.

Why two fresh repeats:

- one fresh repeat would provide only one anchor-vs-repeat comparison and poor protection against treating either transcript as an outlier;
- two fresh repeats permit anchor-vs-repeat comparisons plus a repeat-vs-repeat comparison when both contribute;
- three or more fresh repeats increase provider exposure and evidence burden without any frozen requirement or justified statistical model;
- three total observations remain descriptive architecture evidence only; no statistical-power/significance claim is made.

This is a minimum-sufficient recommendation, not Director authority.

## Anti-selection and terminal audit

Both fresh slots must be preregistered before the first transcript so the second run cannot be added or withheld based on whether the first result is artistically favorable.

Each slot is independently consumed by its first namespace claim. A technical/noncontributing terminal remains visible evidence and receives no replacement. The second preregistered slot remains an independent planned repeat, not a retry, and cannot execute automatically: it requires its own fresh namespace/provider/account gate.

No third fresh slot is available under the recommended plan. If the resulting batch is too incomplete to answer the repeatability question, E0-C closes with that limitation or returns to the Director for an explicit redesign decision; Engineering must not run until a clean sample appears.

## Comparison-method audit

E0-C should reuse the already-frozen ten-dimension E0 experiential rubric rather than invent a new post-transcript quality scale.

Recommended comparison law is a preregistered blind pairwise matrix among all contributing transcripts in the three-observation set: Run 08 / Repeat 1 / Repeat 2. Pair mappings, neutral speaker normalization, score choices, evidence requirement, no-master-score rule, and pre-unblinding seal are frozen before the first fresh transcript.

This changes the interpretation, not the rubric: E0-C asks how much experiential variation appears under the same controllable condition. It does not promote the 'best' repeat to a new reference and does not treat variation itself as success or failure.

Hard-gate-invalid/noncontributing runs are excluded from experiential scoring but retained in the reliability record.

## Execution-time authority still missing

No current provider facts were promoted by this preparation audit. Before each approved fresh run, activation must re-establish the exact executable/Fixture/namespace plus then-current public model lifecycle/pricing/data-use/reasoning/rate facts, authenticated project/key/tier/quota/capacity, protected credential readiness, and applicable Director/provider authority.

The proposal mints no RunId/root, reads no credential, probes no provider, sends no inference, and authorizes no spend.

## Recursive audit result

The proposal was checked against current `CURRENT_STATE.md`, `docs/PROJECT_EXECUTION_QUEUE.md`, the frozen roadmap sequence, the selected Run 08 descriptor, the E0-A reference envelope, Run 08 terminal/hard-gate evidence, the existing ten-dimension blind rubric, and the preserved exact native validator.

Corrections incorporated before freezing this preparation input:

- selected exact Run 08 executable reuse instead of the newer E0-B executable to avoid source-version drift;
- bounded the recommendation to two fresh repeats rather than an open-ended batch;
- preregistered both slots before first execution to prevent outcome-conditioned sample count;
- made every first namespace claim consume its slot and prohibited replacement runs;
- required separate execution-time gates for each slot so Repeat 2 cannot become an automatic retry;
- reused the established blind E0 rubric while changing the interpretation to descriptive repeatability rather than winner selection;
- prohibited statistical-significance claims from the small bounded sample;
- preserved hard-gate/reliability evidence independently from experiential comparison.

The original final pass did not identify one remaining controllable selection path: even with Slot 2 preregistered, its launch timing could still be delayed outcome-conditionally after Slot 1. Project Issue #107 identified that temporal-selection defect before execution. The Director explicitly ratified the correction in comment `5650353003` at `2026-09-13T02:44:41Z`: freeze Slot 1 -> Slot 2 order and one fixed UTC batch window before the first namespace claim; after the first claim the order/window are immutable; an objectively gated nonlaunch at window end is preserved as unexecuted/noncontributing rather than extending the batch.

With that amendment, the prior zero-source-change, two-repeat count, no-retry/replacement, independent hard-gate, and descriptive blind-comparison findings remain valid. The earlier chronological `IDENT` preregistration at commit `e440d104f8f63f0486865a49542bd0fc6684165f` can accept the timing amendment without changing its RunIds or frozen blind instrument; no objective repository-law defect was found that would justify replacing it with the later raced `REF` realization.

## Director corrected decision

The Director explicitly approved the corrected method on Project Issue #107, comment `5650353003`. The approved method preserves exactly two fresh repeats and adds the outcome-independent temporal law: Slot 1 then Slot 2, plus one explicit fixed UTC batch window frozen before the first namespace claim and immutable afterward.

The amendment itself creates no execution authority. Q-E0C-01 remains provider-blocked until the canonical Slot 1 activation closes its fresh authenticated project/key/tier/quota/capacity gate, freezes the exact UTC window, integrates durably, and passes exact-main Validation. Gemini provider traffic remains zero during this reconciliation.
