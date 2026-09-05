# Ensemble Current State

Updated: 2026-09-05

## Authority

Frozen Blueprint 0.1 and approved patch blueprints govern architecture. `Rylascoo/Ensemble-Project` is current engineering authority. Read `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` before substantive work, resolve current `main`, and never promote validation beyond observed evidence.

## Current checkpoint

Phase: `E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed patch: **H1 Patch 0016 — Synchronized Causal Cycle — Proposal 0.5**

Promotion PR: `#31`
Executable promotion commit: `eaf33a723bcd17608d4f0e5e591bd08a28174c06`

Architecture/evidence:
- `docs/blueprint/H1_PATCH_0016_SYNCHRONIZED_CAUSAL_CYCLE.md`
- `docs/evidence/PATCH_0016_EVIDENCE.md`
- `docs/evidence/PATCH_0016_ORACLE.json`

## Native validation

Patch 0016 ARM64 evidence is **Director-machine-sourced** from the Director's native Windows ARM64 machine, not the engineering assistant environment.

Exact successful Core-test head: `aa1346964aeb0f27b9d9ff609514150f438d4474`.

Observed: Core compile PASS; test-project compile PASS; **584/584 tests PASS**, 0 failed/skipped; `CORE_TEST_EXIT=0`; tracked/staged clean.

Earlier executable head `09bf644f75de875870ba3d625dd381d83e4ff4c8`: Harness ARM64 build PASS; Missing Raft fixture PASS; generic smoke PASS. Later executable/test change touched one test file only; no `src/`, Harness, fixture, framework, SDK, canonicalizer, or oracle changed.

No WinUI, Windows AI/NPU, MSIX/WACK, or Store authority exists.

## Implemented H1 spine

Fixture/domain -> Access -> Context -> Performer Candidate -> Director Opportunity -> Integrity -> State Interpreter -> State Authority -> Take -> atomic causal commit -> effective Opportunity -> Production-bound Context -> accepted Performance history -> synchronized two-phase causal cycle.

Patch 0016 preserves accepted postcommit causal history if later Opportunity establishment fails.

E0 Context currently reconstructs permitted authoritative state each turn and includes all accepted current-Scene Performances in append order; no windowing/retrieval/summarization. Post-E0 scaling is ODR-32 in `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md`.

## Standing decisions

- .NET 10 migration requires explicit later Director approval.
- `IPackageValidator` is not a first-release validation gate; stale ship-plan wording needs maintenance.
- E5c exception-runtime-type provenance: open question, no verified defect/fix.
- Stage representation remains design-lane authority.

## Next action

Blueprint the smallest remaining H1 boundary for a complete run driver: a provider-neutral Performer attempt/result ingress that distinguishes a valid Candidate from technical failure/cancellation without provider SDKs, persistence, full Scene lifecycle, or post-E0 Context optimization.

No implementation before falsification, recursive audit, and explicit Director approval.
