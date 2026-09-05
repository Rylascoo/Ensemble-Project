# Ensemble Current State

Updated: 2026-09-05

## Authority

1. `Rylascoo/Ensemble-Project` is current engineering authority.
2. Frozen Blueprint 0.1 and approved patch blueprints govern architecture.
3. Read `docs/PROJECT_REASONING_TASK_SCOPE_OPTIMIZATION_PROTOCOL.md` before substantive work.
4. Resolve current `main` before modifying source.
5. Never promote validation beyond observed evidence.

## Current phase

`E0-A Harness Implementation — H1 Deterministic Spine`

Latest completed executable patch:

`H1 Patch 0016 — Synchronized Causal Cycle — Proposal 0.5`

Promotion PR: `#31`
Executable promotion checkpoint on `main`:

`eaf33a723bcd17608d4f0e5e591bd08a28174c06`

Canonical architecture:

`docs/blueprint/H1_PATCH_0016_SYNCHRONIZED_CAUSAL_CYCLE.md`

Canonical evidence/oracle:

- `docs/evidence/PATCH_0016_EVIDENCE.md`
- `docs/evidence/PATCH_0016_ORACLE.json`

## Native validation authority

Patch 0016 ARM64 evidence is **Director-machine-sourced** from the Director's native Windows ARM64 machine; it was not executed by the engineering assistant environment.

Observed machine: Windows `10.0.26200`, `ARM64`, RID `win-arm64`, repository-selected .NET SDK `9.0.317`.

Exact successful Core-test head:

`aa1346964aeb0f27b9d9ff609514150f438d4474`

Observed: Core compile PASS; test-project compile PASS; `584/584` tests PASS; `0` failed; `0` skipped; `CORE_TEST_EXIT=0`; tracked/staged tree clean.

Earlier executable head `09bf644f75de875870ba3d625dd381d83e4ff4c8`: Harness ARM64 build PASS; Missing Raft fixture PASS; generic smoke fixture PASS. Subsequent executable/test correction touched one test file only; no `src/`, Harness, fixture, framework, SDK, canonicalizer, or oracle changed.

No evidence establishes WinUI runtime, Windows AI/NPU execution, MSIX/WACK, or Store certification.

## Implemented deterministic spine

Current H1 path now includes fixture/domain foundation, Access Control, Context composition, Performer Candidate, Director Opportunity, Integrity, State Interpreter, State Authority, Take semantics, atomic causal commit, effective Opportunity authority, Production-bound Context continuity, accepted Performance-history continuity, and the synchronized two-phase causal cycle.

Patch 0016 preserves the valid postcommit adoption boundary: accepted causal history cannot be rolled back merely because later Opportunity establishment fails.

## Context continuity reference

E0 reconstructs current permitted state each turn from authoritative `ProductionState` through deterministic Access Control. Accepted current-Scene Character-legible Performances are a separate append-ordered projection; E0 v3 includes all of them. No recency windowing, retrieval, truncation, summarization, paraphrase, token budgeting, or model compression occurs in E0.

Post-E0 scaling is ODR-32. See `docs/OPEN_DESIGN_REGISTER_CONTINUATION.md` for ODR-12/13/30/32. These do not block the current E0-A reference path.

## Standing open decisions

- .NET 10 migration remains a later explicit Director decision; current repository baseline remains unchanged.
- `IPackageValidator` is not a first-release validation gate; prior contrary plan language requires maintenance correction.
- E5c exception-runtime-type provenance remains an open question with no verified defect/fix authorized.
- Stage representation remains a design-lane question, not engineering authority.

## Next action

Identify and blueprint the smallest remaining H1/E0-A boundary needed for a complete run driver. Current leading seam: a minimal provider-neutral technical attempt/result plus cancellation/failure contract, without provider SDK binding, persistence, full Scene lifecycle, or post-E0 Context optimization.

Do not implement that next boundary until its architecture is falsified, recursively audited, and explicitly approved.
