# Ensemble Current State

Updated: 2026-09-03

## Source of truth

1. GitHub `Rylascoo/Ensemble-Project` is authoritative current engineering state.
2. Read this file first in every fresh Kymaean engineering chat.
3. Canonical approved blueprints and evidence under `docs/` govern patch-specific architecture and validation.
4. Google Drive `Ensemble Project` is supporting design/research material, not executable validation authority.
5. Archived DeskShifter V7 material is immutable regression/reference material only.

## Validation authority

Do not promote a lower validation level into a higher one.

- static/adversarial analysis: advisory;
- Visual Studio / `dotnet build` on native Windows ARM64: compiler authority for the exercised build;
- `dotnet test` on native Windows ARM64: test execution authority for the exercised suite;
- actual device/runtime exercise: runtime authority for the exercised path only;
- WACK: package-validation authority;
- Partner Center: Microsoft Store certification authority.

No current evidence establishes Windows AI/NPU execution, WACK success, packaging success, or Store certification.

## Current phase

`E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed patch:

`H1 Patch 0012 — E0 Atomic Causal Commit`

Architecture:

`FROZEN — Proposal 0.10`

Implementation:

`COMPLETE FOR EXERCISED PATCH 0012 SCOPE`

Native validation:

`COMPLETE FOR EXERCISED CORE/TEST/HARNESS GATES`

Promotion:

`PROMOTED TO MAIN`

## Current Git authority

Patch 0012 implementation/promotion PR:

`#26 — H1: implement and validate E0 Atomic Causal Commit`

PR #26 squash-merge commit on `main`:

`bc208e6c9b46acf3c98b454df13bfc65efca5a38`

Exact native Windows ARM64 machine-tested executable/test head:

`39bc078c130ab1165c6a81c1673dd5cd25da3724`

This tested SHA remains the Patch 0012 compiler/test/Harness authority.

The later squash-merge commit and post-promotion documentation checkpoint do **not** replace that tested-head authority.

Historical implementation branch:

`h1-patch-0012-atomic-causal-commit-implementation`

Approved implementation baseline before Patch 0012 source work:

`163696f4a89aa3a3b1ea4975167c15b827328d1d`

## Patch 0012 machine validation

Machine evidence:

`docs/evidence/H1_PATCH_0012_ARM64_VALIDATION.md`

Observed on the user's native Windows ARM64 development machine at exact executable/test head `39bc078c130ab1165c6a81c1673dd5cd25da3724`:

- full Core tests: `473/473` PASS;
- failed: `0`;
- skipped: `0`;
- native Core/Harness Debug build: PASS;
- Harness target: `net9.0\win-arm64`;
- Missing Raft Harness: PASS;
- Missing Raft exit code: `0`;
- generic smoke Harness: PASS;
- generic smoke exit code: `0`.

Canonical machine-established Patch 0012 hash oracles:

Genesis `StateHash`:

`30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`

Oracle post-commit `StateHash`:

`057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30`

The temporary diagnostic used to obtain the complete post-commit hash was removed before the tested head.

## Patch 0012 final recursive audit

Final evidence:

`docs/evidence/H1_PATCH_0012_FINAL_IMPLEMENTATION_AUDIT.md`

Audit order:

`correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence`

Final result after native convergence:

- zero material corrections;
- zero worthwhile implementation improvements within Patch 0012 scope.

The production implementation did not change after corrected source/test head:

`94f13c06ecd7834b9c1e7abc777b4e3ef6d92c3a`

From that corrected source head to exact tested head `39bc078c130ab1165c6a81c1673dd5cd25da3724`, executable/test changes were limited to the two fixed StateHash-oracle test files; other changes were documentation/evidence.

A comparison from pre-diagnostic checkpoint `181ba982b6e35f61e85c246f24d0fa44d3b81cee` to tested head `39bc078c130ab1165c6a81c1673dd5cd25da3724` shows the net diagnostic-recovery source/test delta is exactly three oracle replacements and no production source change.

## Patch 0012 canonical authority

Canonical blueprint:

`docs/blueprint/H1_PATCH_0012_ATOMIC_CAUSAL_COMMIT.md`

Approved Proposal:

`0.10`

Exact recursively audited proposal head:

`ad1e84121653457454f6bf90e831e2064a79f224`

Blueprint approval evidence:

`docs/evidence/H1_PATCH_0012_BLUEPRINT_APPROVAL.md`

Implementation handoff:

`docs/handoff/E0A_H1_PATCH_0012_IMPLEMENTATION_HANDOFF.md`

Historical static implementation audit:

`docs/evidence/H1_PATCH_0012_STATIC_IMPLEMENTATION_AUDIT.md`

Important: the historical static audit preserves pre-native oracle expectations and must be read as historical advisory evidence. Its old hash-oracle values were superseded after the valid provenance canonicalization correction and final native validation.

Historical native attempt evidence:

- `docs/evidence/H1_PATCH_0012_NATIVE_VALIDATION_ATTEMPT_01.md`;
- `docs/evidence/H1_PATCH_0012_NATIVE_VALIDATION_ATTEMPT_02.md`.

Historical failed attempts remain intentionally preserved rather than rewritten.

## Patch 0012 implemented boundary

Patch 0012 implements the approved Production/CausalCommit boundary only.

Canonical data flow:

```text
immutable ProductionState
    -> O(1) ProductionStateCheckpoint before Access/Context
        -> existing Access / Context / Performance / Integrity / Interpretation / StateAuthority
            -> immutable Accepted E0Take
                -> Accepted-only exact Take/source-state binding
                    -> exact StateHash freshness
                        -> deterministic atomic causal commit
                            -> E0CausalCommit
                            -> result ProductionState
```

Core frozen laws now implemented and exercised include:

1. exact Accepted Performance plus every retained Approved consequence becomes effective together or neither does;
2. retained Rejected consequences never become effective;
3. only Accepted Takes cross the causal-commit binding boundary;
4. `ProductionState` is immutable current projection; `ValidatedFixture` remains immutable genesis input;
5. `ProductionStateCheckpoint.Capture(...)` is O(1) and does not rehash/reproject/revalidate the full ledger;
6. binding owns one exact Production-derived StateAuthority snapshot association proof;
7. Commit uses exact `currentState.StateHash == binding.SourceStateHash` freshness and does not repeat the snapshot proof;
8. Replay independently checks its parent Production-derived StateAuthority snapshot because Replay has no source binding;
9. existing `CommitId`, `RecordId`, and `TakeId` remain canonical;
10. caller-supplied materialization exists exactly for retained Approved Add/Supersede consequences;
11. Approved Deactivate needs no materialization;
12. retained Rejected mutations have no materialization and no state effect;
13. RecordIds are globally unique across active/inactive Production history;
14. Add/Supersede/Deactivate follow retained proposal/decision semantics and do not make a second approval decision;
15. Supersede/Deactivate ExistingRecordId lineage is not synthesized into support provenance;
16. Fixture and Production share one internal RecordId provenance-DAG primitive;
17. valid unordered fixture provenance is canonicalized rather than rejected;
18. Production -> StateAuthority mapping is explicit semantic mapping, not numeric enum casting;
19. no SnapshotHash exists;
20. causal event surface is minimal: CommitId, ParentStateHash, ResultStateHash, exact Accepted Take, exact RecordMaterializations;
21. successful commit consumes Current Opportunity and sets result opportunity to null;
22. zero-mutation and all-Rejected Accepted Takes remain valid history-advancing commits;
23. canonical Production projection / causal payload / StateHash byte rules are frozen and machine-oracled;
24. effective CommitId/TakeId indexes remain derived duplicate-rejection caches excluded from projection hashing;
25. Commit and Replay share one deterministic transition engine;
26. Replay is one-step only and does not claim full session replay from genesis;
27. expected failure domains remain sanitized `ProductionStateException` and `E0CausalCommitException` boundaries;
28. Patch 0012 adds no idle/background/network/provider/clock/random/GPU/NPU work.

## Explicit Patch 0012 non-scope

Do not extend Patch 0012 into any of the following without a separately approved next architecture:

- evolved `ProductionState -> Access/Context` integration;
- CharacterClaim / recent-Performance disclosure policy;
- next Director opportunity transition;
- full multi-turn replay from genesis;
- durable persistence/recovery;
- branch/canon/retcon/rehearsal;
- provider/model execution;
- Scene loop;
- Observation / World Resolver;
- WinUI;
- Windows AI Foundry / NPU integration;
- MSIX packaging;
- WACK;
- Store certification.

## Open design guard

`docs/blueprint/CREATOR_ONTOLOGY_EXTENSIBILITY_GUARD.md` remains a STRONG DIRECTION / OPEN DESIGN GUARD, not a frozen creator-facing schema.

> Keep authority semantics precise; keep creative semantics open.

Patch 0009–0012 deterministic authority semantics do not freeze the final creator-facing Production/Studio ontology, evolved Character Context disclosure model, final branch/rehearsal UX, or broader creative semantics.

## Prior completed machine authority

H1 Patch 0011 — E0 Take Semantics:

- canonical Proposal 0.15;
- exact machine-tested head `4250011c167cd9850ad891aaea4ee053216cf135`;
- `430/430` Core tests PASS;
- native `win-arm64` Harness build PASS;
- Missing Raft and generic smoke PASS;
- promoted by PR #24 squash merge `82e572d0f56a4e4a9791722a2352e39d62e3d59d`.

Patch 0012 supersedes Patch 0011 as the latest fully machine-validated executable checkpoint for the exercised deterministic-spine scope, while Patch 0011 evidence remains authoritative for its exact historical head.

## Earlier patch status

H1 Patches 0003–0010 are approved/canonical and machine-validated for their exercised gates. Dedicated blueprint and evidence files under `docs/blueprint/` and `docs/evidence/` remain the detailed historical authority.

Do not reload or summarize all historical patches in fresh chats unless required by the current task.

## Fresh-chat bootstrap

For the next Kymaean engineering chat:

1. read this `CURRENT_STATE.md` first;
2. resolve current `main` before changing source;
3. treat PR #26 as merged and Patch 0012 as complete;
4. preserve exact machine-tested executable authority `39bc078c130ab1165c6a81c1673dd5cd25da3724` even though `main` contains the later squash-merge and checkpoint commits;
5. do not reopen approved Proposal 0.10 or redesign completed Patch 0012;
6. do not enter deferred Patch 0012 non-scope by silently extending the completed patch;
7. read only the canonical roadmap and source files needed to define the next patch boundary;
8. use the same patch-first and recursive-audit discipline for the next approved implementation.

## Next action

Patch 0012 implementation, native validation, recursive audit, evidence, PR review, and promotion are complete.

Next engineering work must begin as a new patch from the canonical Kymaean roadmap rather than by extending Patch 0012 opportunistically.
