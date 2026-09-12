# Ensemble Current State

Updated: 2026-09-11

## Authority
`Rylascoo/Ensemble-Project` is Engineering authority. This file alone owns phase/checkpoint/validation/next action. Bootstrap: `AGENTS.md`; authority: `docs/PROJECT_AUTHORITY.md`; sequencing: `docs/PROJECT_EXECUTION_QUEUE.md`.

## Checkpoint
Runtime **E0-A Experimental Harness - Phase B**. Q-E0A-01/02/03/04 are **DONE**. Q-E0B-01 is the immediate **ACTIVE** experiment-order successor; provider traffic remains **BLOCKED pending separate current activation/authority**.

Run 08 `E0A-Q03-G35L-20260911-08` is the Director-selected E0-A reference. Selected descriptor: `docs/evidence/E0A_Q_E0A_03_RUN08_SELECTED_REFERENCE_DESCRIPTOR_2026_09_11.json`; Q-E0A-03 closure: `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_CLOSURE_AUDIT_2026_09_11.md`.

Run 09 `E0A-Q03-G25L-20260911-09` executed once and is terminal/noncontributing: first Performer `countTokens` returned HTTP 404 / `NOT_FOUND`; zero accepted turns, zero generation, zero shadow spend, known usage. Its runtime seal independently verifies and hard-gate evaluation is PASS. Durable pre-run state still showed the authenticated 2.5 project/model/tier quota + same-day-capacity gate pending, so Run 09 is preserved as technical evidence but is not credited as a fully gate-compliant completed control. No retry/replay/replacement is authorized.

## Validation / evidence
Native authority remains `bb869fb1c505603612bc718f739b3f1b358e5539`; tag `validation/e0a-gemini-technical-failure-diagnostic-classification-native-arm64`; tag object `c4ff8dc4b9ebd31674208f4ba9c958d50fc975d5`. Native ARM64 Core **622/622**, Harness **140/140**.

Run 08: 12/12 accepted turns, 36/36 exact-model generations, 72 provider operations, known usage, shadow USD `0.05258350`; runtime root `e0249ac54353ca0fb550ba68bbb42eee6c0bf891e1fe2c7c61cbe3fb6420f459`; hard gates PASS. Run 09: one failed `countTokens` operation; runtime root `8b4689b380eb9c8a917aa7413b55a39256853c8ae32e70d16ca2111fb05740f9`; hard gates PASS.

## Continuity
Runs 01-07 remain immutable/noncontributing. Run 08 is the sole contributing full candidate and selected E0-A reference; no two-candidate blind comparison applies. Run 09 remains visible as failed 2.5 control evidence and does not invalidate Run 08 under the frozen closure law.

Q-E0B-01 remains blocked until Q-E0A-03 closes. Q-E0E-PREP is DONE; Q-E0E-RUN remains blocked through E0-A-D. Design authority is separate. Administrator C0-C9 are DONE; C9A R1/R2 are uncommissioned after bounded timeout falsifications, with no same-realization retry authorized; C10 falsified; C11+ blocked.

## Next
Begin Q-E0B-01 from the sealed Run 08 reference descriptor. Perform only the current non-provider preparation/audit needed to activate E0-B; do not use credentials or send provider traffic until a separate current E0-B activation and applicable Director/provider authority are durable. Preserve the frozen E0 experiment order A -> B -> C -> D -> E -> F -> G.
