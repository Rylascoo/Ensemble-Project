# E0-C Q-E0C-01 - Slot 2 Execution-Law Protocol Deviation and Director Input

Date: 2026-09-13

Status: **PROTOCOL DEVIATION CONFIRMED - BOTH FRESH SLOTS CONSUMED - P01 BLIND SCORE SEALED BEFORE RECONCILIATION - MAPPING STILL WITHHELD - DIRECTOR DISPOSITION REQUIRED BEFORE REVEAL/CLOSURE**

## Current preserved authority

Slot 2 terminal evidence is `docs/evidence/E0C_Q_E0C_01_RUN02_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`, integrated by PR #120 at main `cf47ae98807afc124fcfc4d9c718ae341861234e`; exact-main Validation #772 passed.

P01 package/mapping preparation is preserved in `docs/evidence/E0C_Q_E0C_01_P01_BLIND_PACKAGE_2026_09_13.json` and `docs/evidence/E0C_Q_E0C_01_P01_MAPPING_COMMITMENT_2026_09_13.json`. PR #121 created them and PR #122 sanitized the package at main `5c0284cdf5f9a0ec7dfbef529f3196df1434e0ae`; exact-main Validation #778 passed before scorer access. The mapping commitment remains `5d9a40d1ed5619f97015b06b47074a60c1cd66e0cbd265a937be730ac874cf6c`.

Blind scoring then raced ahead before this deviation reconciliation reached `main`. PR #123 integrated `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_2026_09_13.json` and `docs/evidence/E0C_Q_E0C_01_P01_BLIND_SCORE_SEAL_2026_09_13.json` at main `e1c20e6f25fa876c8c9ee8fabb911bf8365025bc`; exact-main Validation #783 passed. The score records ten frozen dimensions, and the seal binds score SHA-256 `e074ca6ad1c398fdcddfa97802f0e32d74c540655182cec2b06f73473d705988`.

Critically, the score remained blind: `mappingKnownDuringScoring=false`, `mappingKnownAtSeal=false`, `scoresSealedBeforeMappingReveal=true`, and no E0-C reveal/unblind artifact exists on this main.

## Confirmed execution-law deviation

`docs/evidence/E0C_Q_E0C_01_STANDING_PROJECT_KEY_ASSOCIATION_DIRECTOR_AMENDMENT_2026_09_13.md` makes only the authenticated Kymaean project/testing-key association durable. It explicitly states that fresh model/tier/quota/capacity evidence remains execution-sensitive.

Slot 2 activation instead reused the authenticated pre-Slot-1 snapshot (`42/500 RPD`, `9/15 RPM`, `22.07K/250K TPM`) and inferred post-Slot-1 headroom from Slot 1's 72 operations plus elapsed minute windows. That arithmetic was conservative, and the actual Slot 2 provider call succeeded, but it was not a new authenticated post-Slot-1 quota/capacity observation. The execution-sensitive capacity gate was therefore not actually closed and Slot 2 should have remained blocked.

This is a protocol-compliance defect, not a causal explanation for Slot 2's `InvalidOutput`. Slot 2 remains permanently consumed/noncontributing. No retry, replacement, third slot, alternate route/model/key/tier, or window extension exists.

## Claim-history correction

Canonical two-slot preregistration correctly predicted the exact Slot 1 and Slot 2 claim filenames. A later launch-boundary checker used different Slot 1 claim paths; that checker was wrong. The Harness source-defined claim guard and canonical preregistration were correct. The Slot 1 terminal record is corrected in this package.

## Post-deviation blind-score containment

The frozen contribution law removes P02 and P03 because Slot 2 is noncontributing. P01 (Run 08 / Slot 1) is the only otherwise eligible experiential pair.

Because PR #123 completed blind scoring before this reconciliation reached `main`, the already-sealed P01 score is preserved as immutable post-deviation evidence. It must **not** be rescored, regenerated, replaced, revealed, unblinded, or used to close E0-C before Director disposition. The true LEFT/RIGHT mapping remains withheld outside Git.

The scoring race does not retroactively cure the Slot 2 execution-law defect. Conversely, because scoring remained blind and sealed before reveal, this reconciliation does not declare the score contaminated; it simply withholds any use/reveal decision from Engineering.

## Director boundary

Engineering returns the deviation to the Director before mapping reveal, unblinded reporting, E0-C closure, or E0-D advancement. Two lawful dispositions are presented without changing either consumed namespace or regenerating any scoring artifact:

1. accept the already-sealed blind P01 score as usable unaffected evidence, then authorize reveal/verification of the existing committed mapping and complete E0-C descriptive closure under the frozen instrument; or
2. close E0-C without using the experiential score because the Slot 2 execution process breached an execution-sensitive gate.

Under either disposition Slot 2 remains consumed/noncontributing, no replacement exists, and provider traffic remains zero. Until the Director chooses, Q-E0C-01 is **ACTIVE / HOLD**, the sealed P01 score is preserved-but-withheld from reveal/use, and Q-E0D-01 remains blocked.
