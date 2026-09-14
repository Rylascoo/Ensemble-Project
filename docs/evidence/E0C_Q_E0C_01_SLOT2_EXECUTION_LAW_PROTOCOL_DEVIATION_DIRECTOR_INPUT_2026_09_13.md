# E0-C Q-E0C-01 - Slot 2 Execution-Law Protocol Deviation and Director Input

Date: 2026-09-13

Status: **PROTOCOL DEVIATION CONFIRMED - BOTH FRESH SLOTS REMAIN CONSUMED - P01 PACKAGE/MAPPING PRESERVED BUT WITHHELD - ZERO SCORER ACCESS - DIRECTOR DISPOSITION REQUIRED**

## Authoritative predecessors

Slot 2 terminal evidence is `docs/evidence/E0C_Q_E0C_01_RUN02_TERMINAL_EVIDENCE_ANALYSIS_2026_09_13.md`, integrated by PR #120 at main `cf47ae98807afc124fcfc4d9c718ae341861234e` and exact-main Validation #772. P01 pre-scorer preparation is `docs/evidence/E0C_Q_E0C_01_P01_BLIND_PACKAGE_2026_09_13.json` plus `docs/evidence/E0C_Q_E0C_01_P01_MAPPING_COMMITMENT_2026_09_13.json`. PR #121 created the initial package/commitment; PR #122 sanitized the package metadata without changing the withheld `mappingSha256`, merged as `5c0284cdf5f9a0ec7dfbef529f3196df1434e0ae`, and exact-main Validation #778 passed. The PR records preserve the pre-scorer boundary: no scorer access, score, observation, or mapping reveal occurred.

The frozen method, two-slot preregistration, and standing project/key amendment remain authoritative. No source, Fixture, executable, runtime evidence, provider route, or sealed transcript is changed by this reconciliation.

## Preserved Slot 2 terminal

Slot 2 `E0C-Q01-IDENT-20260912-02` remains consumed and noncontributing. Its one launch produced `InvalidOutput` at 0/12 after one successful exact-model Performer generation and one countTokens preflight. The typed control used out-of-roster `MARLOE` instead of canonical `MARLOWE`. Runtime sealing and independent hard-gate review remain valid; hard-gate PASS is an evidence-integrity result and does not create experiential eligibility or cure an execution-law defect.

No retry, replay, replacement, third slot, route/model/key/tier substitution, or new E0-C provider launch exists.

## Execution-law compliance defect

`docs/evidence/E0C_Q_E0C_01_STANDING_PROJECT_KEY_ASSOCIATION_DIRECTOR_AMENDMENT_2026_09_13.md` makes only the authenticated project/testing-key association durable. It explicitly states that fresh model/tier/quota/capacity evidence remains execution-sensitive.

Slot 2 activation instead reused the authenticated pre-Slot-1 `42/500 RPD`, `9/15 RPM`, `22.07K/250K TPM` snapshot and inferred post-Slot-1 headroom from Slot 1's 72 operations plus elapsed minute windows. That arithmetic was conservative and the actual Slot 2 provider generation succeeded, but it was not a new authenticated post-Slot-1 quota/capacity observation. Therefore the fresh execution-sensitive capacity gate was not actually closed and Slot 2 should have remained blocked.

This protocol defect is independent of the `InvalidOutput` cause. Provider success cannot retroactively cure it, and hosted repository Validation cannot override the frozen execution law. Because the namespace was claimed, the defect creates no replay/replacement authority.

## Claim-history correction

Canonical two-slot preregistration predicted the exact source-derived Slot 1 and Slot 2 claim filenames. A later launch-boundary checker used different Slot 1 claim paths; that checker was wrong. The Harness claim algorithm and preregistration were correct. The Slot 1 terminal record is corrected in this package.

## P01 containment after concurrent preparation

The frozen contribution law removes P02 and P03 because Slot 2 is noncontributing. P01 (Run 08 / Slot 1) is the only otherwise eligible experiential pair. PR #121 created its blind package and committed the randomized mapping hash; PR #122 then sanitized package metadata while preserving that mapping commitment. The true mapping remains withheld outside Git and no scorer access occurred.

The P01 package and mapping commitment are therefore preserved as prepared evidence but **must not be shown to a scorer, scored, unblinded, replaced, or regenerated** before Director disposition of this deviation. Their existence does not decide whether P01 may proceed.

## Director boundary

Engineering returns the deviation to the Director before scorer exposure, E0-C experiential closure, or E0-D advancement. Two lawful dispositions are presented without changing either consumed namespace:

1. preserve the batch and permit the already-prepared P01 blind package to proceed under the frozen ten-dimension scoring/seal/reveal law; or
2. close E0-C without experiential scoring because the Slot 2 execution process breached an execution-sensitive gate.

Under either disposition Slot 2 remains consumed/noncontributing, no replacement or third slot exists, and provider traffic remains zero. Until the Director chooses, P01 is **prepared but withheld**, Q-E0C-01 remains **ACTIVE / HOLD**, and Q-E0D-01 remains blocked.
