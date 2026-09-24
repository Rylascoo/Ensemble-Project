# Ensemble Current State

Updated: 2026-09-24

## Authority

Application Product, Implementation and App Design authority: `Rylascoo/Ensemble-Project`. Fresh work reads `AGENTS.md`, this file, `docs/PROJECT_AUTHORITY.md`, `docs/PROJECT_EXECUTION_QUEUE.md`, `docs/VALIDATION_LEDGER.md`, and `docs/design/app/CURRENT_CONTRACTS.md` for Design/UI work.

## Integrated checkpoint

`DESIGN_ARCHITECTURE_READY = READY`. Q-PROD-02/03/04/05/06/07/08 and Q-DESIGN-20/21/22/23 are integrated. Q-UNITY-01/02 are complete.

Q-ADMIN-08 is DONE / INTEGRATED through PR #259 at `7650fa40488fd19741c5bd0b65830d50fd62fa7b`; exact-main Validation #1291 PASS. .NET 10 is the integrated baseline.

Q-PREVIEW-01 is DONE / INTEGRATED through PR #263 at `247059a869d5f751f95b227d28f050be2b81a49c`; exact-main Validation #1310 PASS.

Integrated main at this candidate checkpoint remains `f685a21a699f3c9300a8db15358aa5d5d125115f`.

## Q-PROD-09 — first Product-native Performance

The Director's 2026-09-24 continuation after the final Q1/Q2/Q5 analysis authorizes the bounded implementation package recorded in `docs/FIRST_PRODUCT_NATIVE_PERFORMANCE_Q1_Q2_Q5_DIRECTOR_DISPOSITION_2026_09_24.md`. This supersedes the earlier volatile statement that Q-PROD-09 was not authorized; historical documents retain their true-at-the-time status.

Adopted for this first provisional milestone only:

- Q1: Product-native semantic port; `Kymaean.Application` does not reference `Ensemble.E0.Core`.
- Q2: explicit established `SceneId + CharacterId`; bounded acting-Character context contains only SceneId, CharacterId, CharacterName, and that Character's own committed Circumstances.
- Q5: each accepted first-slice Performance commits exactly one additive Character Circumstance consequence.
- Performer and consequence interpretation remain separate invocation-scoped Product ports.
- accepted visible Performance + consequence are one durable atomic Product event;
- Product-owned history revision binds Performance commit to the exact durable source history and rejects stale/ABA context before append;
- ordinary replay, recovery, snapshot rebuild and portable export preserve the accepted event; the acting Character receives the committed Circumstance in a later bounded context.

Exact machine-tested source:
`5038029c79b2177330cc06e81ac338f4b830ad3f`

Annotated native validation tag:
`validation/q-prod-09-first-product-native-performance-native-arm64-r2`
(tag object `aadeb922b5105a60458cbcdff0dd4cf9dac0173b`, exact peel verified).

Draft PR #266 is the implementation candidate and remains **UNMERGED**. Merge requires separate explicit Director authorization.

Native evidence:
`docs/evidence/Q_PROD_09_FIRST_PRODUCT_NATIVE_PERFORMANCE_NATIVE_ARM64_VALIDATION_2026_09_24.md`.

## Open boundaries

The first live AI Performance remains a later milestone. No provider/model selection, credential association, provider network traffic, spend, persistent Character-to-provider/model/understudy casting, automatic Director selection, Current/Active Scene, Scene switching/ending/lifecycle, ODR-19 resolution, deferred-E0 execution, final runtime architecture, Alpha/Beta/release, WACK or Store authority is created by Q-PROD-09.

Q-E0D-01, Q-E0E-RUN, Q-E0F-01, Q-E0G-01, Q-E0-CONV, Q-POSTE0-01 and Q-POSTE0-02 remain open exactly as recorded in the execution queue. Q-PROD-09 is provisional Product implementation under the 2026-09-17 build-ahead amendment and must reconcile later contradictory deferred-E0 evidence by changing Product code/history compatibility behavior, never by rewriting the experiment or silently reinterpreting accepted Product history.

## Parallel continuity preserved

Documentation continuity PR #265 remains draft, documentation-only and unmerged at `c0dbe79e7e491024f85f3cd51abdfcdd907da889`. Q-PROD-09 does not merge it by implication.

Q-DESIGN-24 corrected candidate `abada447f4a6e4a8660146d641b2984bb38c5dec`, draft PR #262, remains separate and INCONCLUSIVE. No Scene acceptance or merge is created. Current Production visual refinement remains inactive.

Q-ALPHA-01 remains BLOCKED.

## Next

Validate the documentation-only Q-PROD-09 closeout head through the ordinary hosted gate. After that, the next consequential action is **Director merge disposition for draft PR #266**. Do not merge PR #266, PR #265 or PR #262 by implication.
