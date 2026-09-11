# E0-A Role Control Identity Alignment — Implementation Audit

Date: 2026-09-10

Status: **IMPLEMENTED / RECURSIVELY AUDITED — NATIVE PROMOTION PENDING**

## Trigger and diagnosis

Run 04 `E0A-Q03-G35L-20260910-04` is immutable/noncontributing after its first Performer generation returned schema-valid control IDs `Marlowe` and `Wren` while deterministic roster identity requires exact `MARLOWE` and `WREN`.

The rejection is correct. The provider request exposed the subject canonical ID but exposed other roster members only through rendered display names. The model therefore lacked the exact vocabulary required by typed control.

Trigger evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN04_TERMINAL_EVIDENCE_ANALYSIS_2026_09_10.md`.

Frozen correction: `docs/blueprint/E0A_ROLE_CONTROL_IDENTITY_ALIGNMENT_AMENDMENT.md`.

## Implemented surface

`src/Ensemble.E0.Harness/Run/E0APromptContracts.cs` now adds `rosterCharacterIds` to the existing bounded `ContextData`, derived directly from `ContextPacket.Roster` canonical IDs. No Character-owned state is added.

Performer instructions require exact case-sensitive reuse of those IDs for typed address/nomination control, prohibit display-name/case conversion, self-address/self-nomination, and duplicate addressed IDs.

Interpreter instructions apply the same exact-ID requirement to non-null mutation subject/target fields while preserving the prior mutation-shape law.

`tests/Ensemble.E0.Harness.Tests/E0ASpendAndRequestTests.cs` verifies both outbound role contexts contain the exact roster ID sequence and verifies the new instruction law.
## Regression evidence before source-candidate commit

On the Director Windows ARM64 host with SDK 9.0.317:

- Core tests: **622/622 PASS** (`net9.0|arm64`);
- Harness tests: **137/137 PASS** (`net9.0|arm64`);
- builds completed with zero warnings/errors.

These working-copy results establish regression readiness only; they are not promoted validation authority. Promotion requires the normal clean detached exact-commit validation.

## Recursive patch-hygiene audit

Correctness: the model now receives the exact identifiers its deterministic output contract already requires. No emitted identity is normalized or repaired after generation.

Consistency/authority: canonical identity continues to originate in `ContextPacket.Roster`; Core parsers and authority remain unchanged and fail closed.

Scope/simplicity: one structured context field and two instruction clauses solve the demonstrated interface gap. No new abstraction, adapter, retry path, mapping table, or provider-specific Core dependency is introduced.

Tests/ARM64: targeted request assertions are part of the full 137-test Harness pass; the unchanged Core remains 622/622 on ARM64.

Evidence/continuity: Run 04 consumption is recorded in the Gemini usage ledger; `CURRENT_STATE.md` and the execution queue mark Run 04 terminal and prohibit provider traffic during repair validation.

Branch/residency: the work remains in `Rylascoo/Ensemble-Project` on one Engineering branch. No Design-owned artifact is touched.

## Stop condition

One complete recursive pass found no further justified implementation expansion. Native exact-checkout validation and annotated tagging are the next gate; provider traffic remains prohibited until integration and fresh activation.